// Decompiled with JetBrains decompiler
// Type: SpiceGrinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpiceGrinder : 
  GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>
{
  public static Dictionary<Tag, SpiceGrinder.Option> SettingOptions = (Dictionary<Tag, SpiceGrinder.Option>) null;
  public static readonly Operational.Flag spiceSet = new Operational.Flag(nameof (spiceSet), Operational.Flag.Type.Functional);
  public static Operational.Flag inKitchen = new Operational.Flag(nameof (inKitchen), Operational.Flag.Type.Functional);
  public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State inoperational;
  public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State operational;
  public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State ready;
  public StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.BoolParameter isReady;

  public static void InitializeSpices()
  {
    Spices spices = Db.Get().Spices;
    SpiceGrinder.SettingOptions = new Dictionary<Tag, SpiceGrinder.Option>();
    for (int idx = 0; idx < spices.Count; ++idx)
    {
      Spice spice = spices[idx];
      if (DlcManager.IsCorrectDlcSubscribed((IHasDlcRestrictions) spice))
        SpiceGrinder.SettingOptions.Add((Tag) spice.Id, new SpiceGrinder.Option(spice));
    }
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inoperational;
    this.root.Enter(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.OnEnterRoot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameEvent.Callback(this.OnStorageChanged));
    this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational)).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).Enter((StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback) (smi =>
    {
      smi.Play(smi.SelectedOption != null ? "off" : "default");
      smi.CancelFetches("inoperational");
      if (smi.SelectedOption != null)
        return;
      smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected);
    })).Exit((StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback) (smi => smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected)));
    this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>((StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Parameter<bool>) this.isReady, this.ready, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsTrue).Update((System.Action<SpiceGrinder.StatesInstance, float>) ((smi, dt) =>
    {
      if (!((UnityEngine.Object) smi.CurrentFood != (UnityEngine.Object) null) || smi.HasOpenFetches)
        return;
      this.isReady.Set(smi.CanSpice(smi.CurrentFood.Calories), smi);
    }), UpdateRate.SIM_1000ms).PlayAnim("on");
    this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>((StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Parameter<bool>) this.isReady, this.operational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsFalse).ToggleRecurringChore(new Func<SpiceGrinder.StatesInstance, Chore>(this.CreateChore));
  }

  private void UpdateInKitchen(SpiceGrinder.StatesInstance smi)
  {
    smi.GetComponent<Operational>().SetFlag(SpiceGrinder.inKitchen, smi.roomTracker.IsInCorrectRoom());
  }

  private void OnEnterRoot(SpiceGrinder.StatesInstance smi) => smi.Initialize();

  private bool IsOperational(SpiceGrinder.StatesInstance smi) => smi.IsOperational;

  private void OnStorageChanged(SpiceGrinder.StatesInstance smi, object data)
  {
    smi.UpdateMeter();
    smi.UpdateFoodSymbol();
    if (smi.SelectedOption == null)
      return;
    bool flag = (double) smi.AvailableFood > 0.0 && smi.CanSpice(smi.CurrentFood.Calories);
    smi.sm.isReady.Set(flag, smi);
  }

  private Chore CreateChore(SpiceGrinder.StatesInstance smi)
  {
    return (Chore) new WorkChore<SpiceGrinderWorkable>(Db.Get().ChoreTypes.Cook, (IStateMachineTarget) smi.workable);
  }

  public class Option : IConfigurableConsumerOption
  {
    public readonly Tag Id;
    public readonly Spice Spice;
    private string name;
    private string fullDescription;
    private string spiceDescription;
    private string ingredientDescriptions;
    private Effect statBonus;

    public Effect StatBonus
    {
      get
      {
        if (this.statBonus == null)
          return (Effect) null;
        if (string.IsNullOrEmpty(this.spiceDescription))
        {
          this.CreateDescription();
          this.GetName();
        }
        this.statBonus.Name = this.name;
        this.statBonus.description = this.spiceDescription;
        return this.statBonus;
      }
    }

    public Option(Spice spice)
    {
      this.Id = new Tag(spice.Id);
      this.Spice = spice;
      if (spice.StatBonus == null)
        return;
      this.statBonus = new Effect(spice.Id, this.GetName(), this.spiceDescription, 600f, true, false, false);
      this.statBonus.Add(spice.StatBonus);
      Db.Get().effects.Add(this.statBonus);
    }

    public Tag GetID() => (Tag) this.Spice.Id;

    public string GetName()
    {
      if (string.IsNullOrEmpty(this.name))
      {
        string key = $"STRINGS.ITEMS.SPICES.{this.Spice.Id.ToUpper()}.NAME";
        StringEntry result;
        Strings.TryGet(key, out result);
        this.name = "MISSING " + key;
        if (result != null)
          this.name = (string) result;
      }
      return this.name;
    }

    public string GetDetailedDescription()
    {
      if (string.IsNullOrEmpty(this.fullDescription))
        this.CreateDescription();
      return this.fullDescription;
    }

    public string GetDescription()
    {
      if (!string.IsNullOrEmpty(this.spiceDescription))
        return this.spiceDescription;
      string key = $"STRINGS.ITEMS.SPICES.{this.Spice.Id.ToUpper()}.DESC";
      StringEntry result;
      Strings.TryGet(key, out result);
      this.spiceDescription = "MISSING " + key;
      if (result != null)
        this.spiceDescription = result.String;
      return this.spiceDescription;
    }

    private void CreateDescription()
    {
      string key = $"STRINGS.ITEMS.SPICES.{this.Spice.Id.ToUpper()}.DESC";
      StringEntry result;
      Strings.TryGet(key, out result);
      this.spiceDescription = "MISSING " + key;
      if (result != null)
        this.spiceDescription = result.String;
      this.ingredientDescriptions = $"\n\n<b>{BUILDINGS.PREFABS.SPICEGRINDER.INGREDIENTHEADER}</b>";
      for (int index = 0; index < this.Spice.Ingredients.Length; ++index)
      {
        Spice.Ingredient ingredient = this.Spice.Ingredients[index];
        GameObject prefab = Assets.GetPrefab(ingredient.IngredientSet == null || ingredient.IngredientSet.Length == 0 ? (Tag) (string) null : ingredient.IngredientSet[0]);
        this.ingredientDescriptions += $"\n{"    • "}{prefab.GetProperName()} {ingredient.AmountKG}{GameUtil.GetUnitTypeMassOrUnit(prefab)}";
      }
      this.fullDescription = this.spiceDescription + this.ingredientDescriptions;
    }

    public Sprite GetIcon() => Assets.GetSprite((HashedString) this.Spice.Image);

    public IConfigurableConsumerIngredient[] GetIngredients()
    {
      return (IConfigurableConsumerIngredient[]) this.Spice.Ingredients;
    }
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class StatesInstance : 
    GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameInstance
  {
    private static string HASH_FOOD = "food";
    private KBatchedAnimController kbac;
    private KBatchedAnimController foodKBAC;
    [MyCmpReq]
    public RoomTracker roomTracker;
    [MyCmpReq]
    public SpiceGrinderWorkable workable;
    [Serialize]
    private int spiceHash;
    private SpiceInstance currentSpice;
    private Edible currentFood;
    private Storage seedStorage;
    private Storage foodStorage;
    private MeterController meter;
    private Tag[] foodFilter = new Tag[1];
    private FilteredStorage foodStorageFilter;
    private Operational operational;
    private Guid missingResourceStatusItem = Guid.Empty;
    private StatusItem mutantSeedStatusItem;
    private FetchChore[] SpiceFetches;
    [Serialize]
    private bool allowMutantSeeds = true;

    public bool IsOperational
    {
      get => (UnityEngine.Object) this.operational != (UnityEngine.Object) null && this.operational.IsOperational;
    }

    public float AvailableFood
    {
      get => !((UnityEngine.Object) this.foodStorage == (UnityEngine.Object) null) ? this.foodStorage.MassStored() : 0.0f;
    }

    public SpiceGrinder.Option SelectedOption
    {
      get
      {
        return !(this.currentSpice.Id == Tag.Invalid) ? SpiceGrinder.SettingOptions[this.currentSpice.Id] : (SpiceGrinder.Option) null;
      }
    }

    public Edible CurrentFood
    {
      get
      {
        GameObject first = this.foodStorage.FindFirst(GameTags.Edible);
        this.currentFood = (UnityEngine.Object) first != (UnityEngine.Object) null ? first.GetComponent<Edible>() : (Edible) null;
        return this.currentFood;
      }
    }

    public bool HasOpenFetches
    {
      get
      {
        return Array.Exists<FetchChore>(this.SpiceFetches, (Predicate<FetchChore>) (fetch => fetch != null));
      }
    }

    public bool AllowMutantSeeds
    {
      get => this.allowMutantSeeds;
      set
      {
        this.allowMutantSeeds = value;
        this.ToggleMutantSeedFetches(this.allowMutantSeeds);
      }
    }

    public StatesInstance(IStateMachineTarget master, SpiceGrinder.Def def)
      : base(master, def)
    {
      this.workable.Grinder = this;
      Storage[] components = this.gameObject.GetComponents<Storage>();
      this.foodStorage = components[0];
      this.seedStorage = components[1];
      this.operational = this.GetComponent<Operational>();
      this.kbac = this.GetComponent<KBatchedAnimController>();
      this.foodStorageFilter = new FilteredStorage((KMonoBehaviour) this.GetComponent<KPrefabID>(), this.foodFilter, (IUserControlledCapacity) null, false, Db.Get().ChoreTypes.CookFetch);
      this.foodStorageFilter.SetHasMeter(false);
      this.meter = new MeterController((KAnimControllerBase) this.kbac, "meter_target", nameof (meter), Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[2]
      {
        "meter_frame",
        "meter_level"
      });
      this.SetupFoodSymbol();
      this.UpdateFoodSymbol();
      this.Subscribe(-905833192, new System.Action<object>(this.OnCopySettings));
      this.sm.UpdateInKitchen(this);
      Prioritizable.AddRef(this.gameObject);
      this.Subscribe(493375141, new System.Action<object>(this.OnRefreshUserMenu));
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      Prioritizable.RemoveRef(this.gameObject);
    }

    public void Initialize()
    {
      if (DlcManager.IsExpansion1Active())
      {
        this.mutantSeedStatusItem = new StatusItem("SPICEGRINDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false);
        if (this.AllowMutantSeeds)
        {
          KSelectable component = this.GetComponent<KSelectable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.AddStatusItem(this.mutantSeedStatusItem);
        }
      }
      SpiceGrinder.Option spiceOption;
      SpiceGrinder.SettingOptions.TryGetValue(new Tag(this.spiceHash), out spiceOption);
      this.OnOptionSelected(spiceOption);
      this.sm.OnStorageChanged(this, (object) null);
      this.UpdateMeter();
    }

    private void OnRefreshUserMenu(object data)
    {
      if (!DlcManager.FeatureRadiationEnabled())
        return;
      Game.Instance.userMenu.AddButton(this.smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", (string) (this.smi.AllowMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT), (System.Action) (() =>
      {
        this.smi.AllowMutantSeeds = !this.smi.AllowMutantSeeds;
        this.OnRefreshUserMenu((object) this.smi);
      }), tooltipText: (string) UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP));
    }

    public void ToggleMutantSeedFetches(bool allow)
    {
      if (!DlcManager.IsExpansion1Active())
        return;
      this.UpdateMutantSeedFetches();
      if (allow)
      {
        this.seedStorage.storageFilters.Add(GameTags.MutatedSeed);
        KSelectable component = this.GetComponent<KSelectable>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.AddStatusItem(this.mutantSeedStatusItem);
      }
      else
      {
        if ((double) this.seedStorage.GetMassAvailable(GameTags.MutatedSeed) > 0.0)
          this.seedStorage.Drop(GameTags.MutatedSeed);
        this.seedStorage.storageFilters.Remove(GameTags.MutatedSeed);
        KSelectable component = this.GetComponent<KSelectable>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.RemoveStatusItem(this.mutantSeedStatusItem);
      }
    }

    private void UpdateMutantSeedFetches()
    {
      if (this.SpiceFetches == null)
        return;
      Tag[] tags = new Tag[2]
      {
        GameTags.Seed,
        GameTags.CropSeed
      };
      for (int index = this.SpiceFetches.Length - 1; index >= 0; --index)
      {
        FetchChore spiceFetch = this.SpiceFetches[index];
        if (spiceFetch != null)
        {
          foreach (Tag tag in this.SpiceFetches[index].tags)
          {
            if (Assets.GetPrefab(tag).HasAnyTags(tags))
            {
              spiceFetch.Cancel("MutantSeedChanges");
              this.SpiceFetches[index] = this.CreateFetchChore(spiceFetch.tags, spiceFetch.amount);
            }
          }
        }
      }
    }

    private void OnCopySettings(object data)
    {
      SpiceGrinderWorkable component = ((GameObject) data).GetComponent<SpiceGrinderWorkable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      this.currentSpice = component.Grinder.currentSpice;
      SpiceGrinder.Option spiceOption;
      SpiceGrinder.SettingOptions.TryGetValue(new Tag(component.Grinder.spiceHash), out spiceOption);
      this.OnOptionSelected(spiceOption);
      this.allowMutantSeeds = component.Grinder.AllowMutantSeeds;
    }

    public void SetupFoodSymbol()
    {
      GameObject gameObject = Util.NewGameObject(this.gameObject, "foodSymbol");
      gameObject.SetActive(false);
      Vector3 column = (Vector3) this.kbac.GetSymbolTransform((HashedString) SpiceGrinder.StatesInstance.HASH_FOOD, out bool _).GetColumn(3) with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse)
      };
      gameObject.transform.SetPosition(column);
      this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
      this.foodKBAC.AnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "mushbar_kanim")
      };
      this.foodKBAC.initialAnim = "object";
      this.kbac.SetSymbolVisiblity((KAnimHashedString) SpiceGrinder.StatesInstance.HASH_FOOD, false);
    }

    public void UpdateFoodSymbol()
    {
      bool flag = (double) this.AvailableFood > 0.0 && (UnityEngine.Object) this.CurrentFood != (UnityEngine.Object) null;
      this.foodKBAC.gameObject.SetActive(flag);
      if (!flag)
        return;
      this.foodKBAC.SwapAnims(this.CurrentFood.GetComponent<KBatchedAnimController>().AnimFiles);
      this.foodKBAC.Play((HashedString) "object", KAnim.PlayMode.Loop);
    }

    public void UpdateMeter()
    {
      this.meter.SetPositionPercent(this.seedStorage.MassStored() / this.seedStorage.capacityKg);
    }

    public void SpiceFood()
    {
      float num = this.CurrentFood.Calories / 1000f;
      this.CurrentFood.SpiceEdible(this.currentSpice, SpiceGrinderConfig.SpicedStatus);
      this.foodStorage.Drop(this.CurrentFood.gameObject, true);
      this.currentFood = (Edible) null;
      this.UpdateFoodSymbol();
      foreach (Spice.Ingredient ingredient in SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients)
      {
        float amount = (float) ((double) num * (double) ingredient.AmountKG / 1000.0);
        for (int index = ingredient.IngredientSet.Length - 1; (double) amount > 0.0 && index >= 0; --index)
        {
          float amount_consumed;
          this.seedStorage.ConsumeAndGetDisease(ingredient.IngredientSet[index], amount, out amount_consumed, out SimUtil.DiseaseInfo _, out float _);
          amount -= amount_consumed;
        }
      }
      this.sm.isReady.Set(false, this);
    }

    public bool CanSpice(float kcalToSpice)
    {
      bool can_spice = true;
      float num1 = kcalToSpice / 1000f;
      Spice.Ingredient[] ingredients = SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients;
      Dictionary<Tag, float> missing_spices = new Dictionary<Tag, float>();
      for (int index1 = 0; index1 < ingredients.Length; ++index1)
      {
        Spice.Ingredient ingredient = ingredients[index1];
        float num2 = 0.0f;
        for (int index2 = 0; ingredient.IngredientSet != null && index2 < ingredient.IngredientSet.Length; ++index2)
          num2 += this.seedStorage.GetMassAvailable(ingredient.IngredientSet[index2]);
        float num3 = (float) ((double) num1 * (double) ingredient.AmountKG / 1000.0);
        can_spice &= (double) num3 <= (double) num2;
        if ((double) num3 > (double) num2)
        {
          missing_spices.Add(ingredient.IngredientSet[0], num3 - num2);
          if (this.SpiceFetches != null && this.SpiceFetches[index1] == null)
            this.SpiceFetches[index1] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
        }
      }
      this.UpdateSpiceIngredientStatus(can_spice, missing_spices);
      return can_spice;
    }

    private FetchChore CreateFetchChore(Tag[] ingredientIngredientSet, float amount)
    {
      return this.CreateFetchChore(new HashSet<Tag>((IEnumerable<Tag>) ingredientIngredientSet), amount);
    }

    private FetchChore CreateFetchChore(HashSet<Tag> ingredients, float amount)
    {
      float num = Mathf.Max(amount, 1f);
      ChoreType cookFetch = Db.Get().ChoreTypes.CookFetch;
      Storage seedStorage = this.seedStorage;
      double amount1 = (double) num;
      HashSet<Tag> tags = ingredients;
      Tag invalid = Tag.Invalid;
      System.Action<Chore> action = new System.Action<Chore>(this.ClearFetchChore);
      Tag[] forbidden_tags;
      if (!this.AllowMutantSeeds)
        forbidden_tags = new Tag[1]{ GameTags.MutatedSeed };
      else
        forbidden_tags = (Tag[]) null;
      System.Action<Chore> on_complete = action;
      return new FetchChore(cookFetch, seedStorage, (float) amount1, tags, FetchChore.MatchCriteria.MatchID, invalid, forbidden_tags, on_complete: on_complete);
    }

    private void ClearFetchChore(Chore obj)
    {
      if (!(obj is FetchChore fetchChore) || !fetchChore.isComplete || this.SpiceFetches == null)
        return;
      for (int index = this.SpiceFetches.Length - 1; index >= 0; --index)
      {
        if (this.SpiceFetches[index] == fetchChore)
        {
          float amount = fetchChore.originalAmount - fetchChore.amount;
          if ((double) amount > 0.0)
          {
            this.SpiceFetches[index] = this.CreateFetchChore(fetchChore.tags, amount);
            break;
          }
          this.SpiceFetches[index] = (FetchChore) null;
          break;
        }
      }
    }

    private void UpdateSpiceIngredientStatus(bool can_spice, Dictionary<Tag, float> missing_spices)
    {
      KSelectable component = this.GetComponent<KSelectable>();
      if (!can_spice)
      {
        if (this.missingResourceStatusItem != Guid.Empty)
          this.missingResourceStatusItem = component.ReplaceStatusItem(this.missingResourceStatusItem, (StatusItem) Db.Get().BuildingStatusItems.MaterialsUnavailable, (object) missing_spices);
        else
          this.missingResourceStatusItem = component.AddStatusItem((StatusItem) Db.Get().BuildingStatusItems.MaterialsUnavailable, (object) missing_spices);
      }
      else
        this.missingResourceStatusItem = component.RemoveStatusItem(this.missingResourceStatusItem);
    }

    public void OnOptionSelected(SpiceGrinder.Option spiceOption)
    {
      this.smi.GetComponent<Operational>().SetFlag(SpiceGrinder.spiceSet, spiceOption != null);
      if (spiceOption == null)
      {
        this.kbac.Play((HashedString) "default");
        this.kbac.SetSymbolTint((KAnimHashedString) "stripe_anim2", Color.white);
      }
      else
        this.kbac.Play((HashedString) (this.IsOperational ? "on" : "off"));
      this.CancelFetches("SpiceChanged");
      if (this.currentSpice.Id != Tag.Invalid)
      {
        this.seedStorage.DropAll();
        this.UpdateMeter();
        this.sm.isReady.Set(false, this);
      }
      if (this.missingResourceStatusItem != Guid.Empty)
        this.missingResourceStatusItem = this.GetComponent<KSelectable>().RemoveStatusItem(this.missingResourceStatusItem);
      if (spiceOption == null)
        return;
      this.currentSpice = new SpiceInstance()
      {
        Id = spiceOption.Id,
        TotalKG = spiceOption.Spice.TotalKG
      };
      this.SetSpiceSymbolColours(spiceOption.Spice);
      this.spiceHash = this.currentSpice.Id.GetHash();
      this.seedStorage.capacityKg = this.currentSpice.TotalKG * 10f;
      Spice.Ingredient[] ingredients = spiceOption.Spice.Ingredients;
      this.SpiceFetches = new FetchChore[ingredients.Length];
      Dictionary<Tag, float> missing_spices = new Dictionary<Tag, float>();
      for (int index = 0; index < ingredients.Length; ++index)
      {
        Spice.Ingredient ingredient = ingredients[index];
        float num = (UnityEngine.Object) this.CurrentFood != (UnityEngine.Object) null ? (float) ((double) this.CurrentFood.Calories * (double) ingredient.AmountKG / 1000000.0) : 0.0f;
        if ((double) this.seedStorage.GetMassAvailable(ingredient.IngredientSet[0]) < (double) num)
          this.SpiceFetches[index] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
        if ((UnityEngine.Object) this.CurrentFood != (UnityEngine.Object) null)
          missing_spices.Add(ingredient.IngredientSet[0], num);
      }
      if ((UnityEngine.Object) this.CurrentFood != (UnityEngine.Object) null)
        this.UpdateSpiceIngredientStatus(false, missing_spices);
      this.foodFilter[0] = this.currentSpice.Id;
      this.foodStorageFilter.FilterChanged();
    }

    public void CancelFetches(string reason)
    {
      if (this.SpiceFetches == null)
        return;
      for (int index = 0; index < this.SpiceFetches.Length; ++index)
      {
        if (this.SpiceFetches[index] != null)
        {
          this.SpiceFetches[index].Cancel(reason);
          this.SpiceFetches[index] = (FetchChore) null;
        }
      }
    }

    private void SetSpiceSymbolColours(Spice spice)
    {
      this.kbac.SetSymbolTint((KAnimHashedString) "stripe_anim2", spice.PrimaryColor);
      this.kbac.SetSymbolTint((KAnimHashedString) "stripe_anim1", spice.SecondaryColor);
      this.kbac.SetSymbolTint((KAnimHashedString) "grinder", spice.PrimaryColor);
    }
  }
}
