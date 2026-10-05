// Decompiled with JetBrains decompiler
// Type: CreatureCalorieMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CreatureCalorieMonitor : 
  GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>
{
  public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State normal;
  public CreatureCalorieMonitor.PauseStates pause;
  private CreatureCalorieMonitor.HungryStates hungry;
  private Effect outOfCaloriesTame;
  public StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.FloatParameter starvationStartTime;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.normal;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.root.EventHandler(GameHashes.CaloriesConsumed, (GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameEvent.Callback) ((smi, data) => smi.OnCaloriesConsumed(data))).EventHandler(GameHashes.PoopStatesCompleted, (GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameEvent.Callback) ((smi, data) => smi.Poop(data == null ? (PoopData) null : (PoopData) data))).ToggleBehaviour(GameTags.Creatures.Poop, new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback(CreatureCalorieMonitor.ReadyToPoop)).Update(new System.Action<CreatureCalorieMonitor.Instance, float>(CreatureCalorieMonitor.UpdateMetabolismCalorieModifier));
    this.normal.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause).Transition((GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State) this.hungry, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsHungry()), UpdateRate.SIM_1000ms);
    this.hungry.DefaultState(this.hungry.hungry).ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsHungry()));
    this.hungry.hungry.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause).Transition(this.normal, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsHungry()), UpdateRate.SIM_1000ms).Transition((GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State) this.hungry.outofcalories, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsOutOfCalories()), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry);
    this.hungry.outofcalories.DefaultState(this.hungry.outofcalories.wild).Transition(this.hungry.hungry, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsOutOfCalories()), UpdateRate.SIM_1000ms);
    this.hungry.outofcalories.wild.TagTransition(GameTags.Creatures.PausedHunger, this.pause.commonPause).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.tame, true).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry).ToggleCritterEmotion(Db.Get().CritterEmotions.Hungry);
    double num;
    this.hungry.outofcalories.tame.Enter("StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.StarvationStartTime)).Exit("ClearStarvationTime", (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback) (smi => num = (double) this.starvationStartTime.Set(Mathf.Min((float) -((double) GameClock.Instance.GetTime() - (double) this.starvationStartTime.Get(smi)), 0.0f), smi))).Transition(this.hungry.outofcalories.starvedtodeath, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => (double) smi.GetDeathTimeRemaining() <= 0.0), UpdateRate.SIM_1000ms).TagTransition(GameTags.Creatures.PausedHunger, this.pause.starvingPause).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.wild).ToggleStatusItem((string) CREATURES.STATUSITEMS.STARVING.NAME, (string) CREATURES.STATUSITEMS.STARVING.TOOLTIP, notification_type: NotificationType.BadMinor, resolve_string_callback: (Func<string, CreatureCalorieMonitor.Instance, string>) ((str, smi) => str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(smi.GetDeathTimeRemaining())))).ToggleNotification((Func<CreatureCalorieMonitor.Instance, Notification>) (smi => new Notification((string) CREATURES.STATUSITEMS.STARVING.NOTIFICATION_NAME, NotificationType.BadMinor, (Func<List<Notification>, object, string>) ((notifications, data) => (string) CREATURES.STATUSITEMS.STARVING.NOTIFICATION_TOOLTIP + notifications.ReduceMessages(false))))).ToggleEffect((Func<CreatureCalorieMonitor.Instance, Effect>) (smi => this.outOfCaloriesTame)).ToggleCritterEmotion(Db.Get().CritterEmotions.Hungry);
    this.hungry.outofcalories.starvedtodeath.Enter((StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback) (smi => smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation)));
    this.pause.commonPause.TagTransition(GameTags.Creatures.PausedHunger, this.normal, true);
    this.pause.starvingPause.Exit("Recalculate StarvationStartTime", new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.RecalculateStartTimeOnUnpause)).TagTransition(GameTags.Creatures.PausedHunger, this.hungry.outofcalories.tame, true);
    this.outOfCaloriesTame = new Effect("OutOfCaloriesTame", (string) CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, (string) CREATURES.MODIFIERS.OUT_OF_CALORIES.TOOLTIP, 0.0f, false, false, false);
    this.outOfCaloriesTame.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, (string) CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME));
  }

  private static bool ReadyToPoop(CreatureCalorieMonitor.Instance smi)
  {
    return smi.stomach.IsReadyToPoop() && (double) Time.time - (double) smi.lastMealOrPoopTime >= (double) smi.def.minimumTimeBeforePooping && !smi.IsInsideState((StateMachine.BaseState) smi.sm.pause);
  }

  private static void UpdateMetabolismCalorieModifier(CreatureCalorieMonitor.Instance smi, float dt)
  {
    if (smi.IsInsideState((StateMachine.BaseState) smi.sm.pause))
      return;
    smi.deltaCalorieMetabolismModifier.SetValue((float) (1.0 - (double) smi.metabolism.GetTotalValue() / 100.0));
  }

  private static void StarvationStartTime(CreatureCalorieMonitor.Instance smi)
  {
    if ((double) smi.sm.starvationStartTime.Get(smi) > 0.0)
      return;
    double num = (double) smi.sm.starvationStartTime.Set(GameClock.Instance.GetTime(), smi);
  }

  private static void RecalculateStartTimeOnUnpause(CreatureCalorieMonitor.Instance smi)
  {
    float f = smi.sm.starvationStartTime.Get(smi);
    if ((double) f >= 0.0)
      return;
    float num1 = GameClock.Instance.GetTime() - Mathf.Abs(f);
    double num2 = (double) smi.sm.starvationStartTime.Set(num1, smi);
  }

  public struct CaloriesConsumedEvent
  {
    public Tag tag;
    public float calories;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public Diet diet;
    public float hungryRatio = 0.9f;
    public float minConsumedCaloriesBeforePooping = 100f;
    public float maxPoopSizeKG = -1f;
    public float minimumTimeBeforePooping = 10f;
    public float deathTimer = 6000f;

    public override void Configure(GameObject prefab)
    {
      prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
    }

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      List<Descriptor> descriptors = new List<Descriptor>();
      descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.DIET_HEADER, (string) UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER));
      CreatureCalorieMonitor.Stomach stomach = obj.GetSMI<CreatureCalorieMonitor.Instance>().stomach;
      float calorie_loss_per_second = 0.0f;
      foreach (AttributeModifier selfModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
      {
        if (selfModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
          calorie_loss_per_second = selfModifier.Value;
      }
      if (stomach.diet.consumedTags.Count > 0)
      {
        string newValue1 = string.Join(", ", stomach.diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>());
        string newValue2 = "";
        if (stomach.diet.CanEatAnyPlantDirectly)
          newValue2 = string.Join("\n", stomach.diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t =>
          {
            float consumer_caloriesLossPerCaloriesPerKG = -calorie_loss_per_second / t.Value;
            GameObject prefab = Assets.GetPrefab((Tag) t.Key.ToString());
            IPlantConsumptionInstructions consumptionInstructions = prefab.GetComponent<IPlantConsumptionInstructions>() ?? prefab.GetSMI<IPlantConsumptionInstructions>();
            return consumptionInstructions == null ? (string) null : UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", consumptionInstructions.GetFormattedConsumptionPerCycle(consumer_caloriesLossPerCaloriesPerKG));
          })).Where<string>((Func<string, bool>) (s => !string.IsNullOrEmpty(s))).ToArray<string>());
        if (this.diet.CanEatPreyCritter)
        {
          if (this.diet.CanEatAnyPlantDirectly)
            newValue2 += "\n";
          newValue2 += string.Join("\n", stomach.diet.consumedTags.FindAll((Predicate<KeyValuePair<Tag, float>>) (t => ((IEnumerable<Diet.Info>) this.diet.preyInfos).FirstOrDefault<Diet.Info>((Func<Diet.Info, bool>) (info => info.consumedTags.Contains(t.Key))) != null)).Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedPreyConsumptionValuePerCycle(t.Key, -calorie_loss_per_second / t.Value)))).ToArray<string>());
        }
        if (this.diet.CanEatAnySolid)
        {
          if (this.diet.CanEatAnyPlantDirectly || this.diet.CanEatPreyCritter)
            newValue2 += "\n";
          newValue2 += string.Join("\n", stomach.diet.consumedTags.FindAll((Predicate<KeyValuePair<Tag, float>>) (t => ((IEnumerable<Diet.Info>) this.diet.solidEdiblesInfo).FirstOrDefault<Diet.Info>((Func<Diet.Info, bool>) (info => info.consumedTags.Contains(t.Key))) != null)).Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram)))).ToArray<string>());
        }
        descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue1), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue2)));
      }
      if (stomach.diet.producedTags.Count > 0)
      {
        string newValue3 = string.Join(", ", stomach.diet.producedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>());
        string newValue4 = "";
        if (stomach.diet.CanEatAnyPlantDirectly)
        {
          List<KeyValuePair<Tag, float>> source = new List<KeyValuePair<Tag, float>>();
          foreach (KeyValuePair<Tag, float> producedTag in stomach.diet.producedTags)
          {
            foreach (Diet.Info directlyEatenPlantInfo in this.diet.directlyEatenPlantInfos)
            {
              if (directlyEatenPlantInfo.producedElement == producedTag.Key)
              {
                float consumed_mass = (float) (-(double) calorie_loss_per_second / (double) directlyEatenPlantInfo.caloriesPerKg * 600.0);
                float producedMass = directlyEatenPlantInfo.ConvertConsumptionMassToProducedMass(consumed_mass);
                source.Add(new KeyValuePair<Tag, float>(producedTag.Key, producedMass / 600f));
              }
            }
          }
          newValue4 = string.Join("\n", source.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM_FROM_PLANT.text.Replace("{Item}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram)))).ToArray<string>()) + "\n";
        }
        else if (stomach.diet.CanEatAnySolid)
        {
          List<KeyValuePair<Tag, float>> source = new List<KeyValuePair<Tag, float>>();
          foreach (KeyValuePair<Tag, float> producedTag in stomach.diet.producedTags)
          {
            foreach (Diet.Info info in this.diet.solidEdiblesInfo)
            {
              if (info.producedElement == producedTag.Key)
                source.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
            }
          }
          newValue4 += string.Join("\n", source.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f)))).ToArray<string>());
        }
        descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue3), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", newValue4)));
      }
      return descriptors;
    }
  }

  public class PauseStates : 
    GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
  {
    public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State commonPause;
    public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvingPause;
  }

  public class HungryStates : 
    GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
  {
    public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State hungry;
    public CreatureCalorieMonitor.HungryStates.OutOfCaloriesState outofcalories;

    public class OutOfCaloriesState : 
      GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
    {
      public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State wild;
      public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State tame;
      public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvedtodeath;
    }
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class Stomach
  {
    [Serialize]
    private List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> caloriesConsumed = new List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry>();
    [Serialize]
    private bool shouldContinuingPooping;
    private float minConsumedCaloriesBeforePooping;
    private float maxPoopSizeInKG;
    private GameObject owner;

    public Diet diet { get; private set; }

    public Stomach(
      GameObject owner,
      float minConsumedCaloriesBeforePooping,
      float max_poop_size_in_kg)
    {
      this.diet = DietManager.Instance.GetPrefabDiet(owner);
      this.owner = owner;
      this.minConsumedCaloriesBeforePooping = minConsumedCaloriesBeforePooping;
      this.maxPoopSizeInKG = max_poop_size_in_kg;
    }

    public void Poop() => this.PoopInStorage((Storage) null);

    public void PoopVoid(Sprite popupImage, string popupText)
    {
      this.PoopInStorage((Storage) null, true, popupImage, popupText);
    }

    public void PoopInStorage(
      Storage storage,
      bool skipPoopSpawn = false,
      Sprite customPopupImage = null,
      string customPopupMessage = null)
    {
      bool flag1 = (UnityEngine.Object) storage != (UnityEngine.Object) null;
      this.shouldContinuingPooping = true;
      float num1 = 0.0f;
      Tag tag = Tag.Invalid;
      byte disease_idx = byte.MaxValue;
      int num2 = 0;
      int disease_delta = 0;
      bool flag2 = false;
      for (int index = 0; index < this.caloriesConsumed.Count; ++index)
      {
        CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[index];
        if ((double) caloriesConsumedEntry.calories > 0.0)
        {
          Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
          if (dietInfo != null && (!(tag != Tag.Invalid) || !(tag != dietInfo.producedElement)))
          {
            float max = (double) this.maxPoopSizeInKG < 0.0 ? float.MaxValue : this.maxPoopSizeInKG;
            float b = Mathf.Clamp(max - num1, 0.0f, max);
            float produced_mass = Mathf.Min(dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(caloriesConsumedEntry.calories)), b);
            num1 += produced_mass;
            tag = dietInfo.producedElement;
            if (dietInfo.diseaseIdx != byte.MaxValue)
            {
              disease_idx = dietInfo.diseaseIdx;
              if (!flag1 && dietInfo.emmitDiseaseOnCell)
                disease_delta += (int) ((double) dietInfo.diseasePerKgProduced * (double) produced_mass);
              else
                num2 += (int) ((double) dietInfo.diseasePerKgProduced * (double) produced_mass);
            }
            caloriesConsumedEntry.calories = Mathf.Clamp(caloriesConsumedEntry.calories - dietInfo.ConvertConsumptionMassToCalories(dietInfo.ConvertProducedMassToConsumptionMass(produced_mass)), 0.0f, float.MaxValue);
            this.caloriesConsumed[index] = caloriesConsumedEntry;
            flag2 = flag2 || dietInfo.produceSolidTile;
          }
        }
      }
      if ((double) num1 <= 0.0 || tag == Tag.Invalid)
      {
        this.shouldContinuingPooping = false;
      }
      else
      {
        string str = (string) null;
        Element element = ElementLoader.GetElement(tag);
        Sprite sprite = (Sprite) null;
        Color color = Color.white;
        if (element != null)
        {
          str = element.name;
          sprite = global::Def.GetUISprite((object) element).first;
          color = element.IsSolid ? Color.white : (Color) element.substance.colour;
        }
        int cell = Grid.PosToCell(this.owner.transform.GetPosition());
        float temperature = this.owner.GetComponent<PrimaryElement>().Temperature;
        DebugUtil.DevAssert(!(flag1 & flag2), "Stomach cannot both store poop & create a solid tile.");
        if (!skipPoopSpawn)
        {
          if (flag1)
          {
            if (element == null)
            {
              GameObject prefab = Assets.GetPrefab(tag);
              GameObject go = GameUtil.KInstantiate(prefab, Grid.CellToPos(cell, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore);
              PrimaryElement component = go.GetComponent<PrimaryElement>();
              component.Mass = num1;
              component.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
              component.Temperature = temperature;
              go.SetActive(true);
              storage.Store(go, true);
              str = go.GetProperName();
              sprite = global::Def.GetUISprite((object) prefab).first;
            }
            else if (element.IsLiquid)
              storage.AddLiquid(element.id, num1, temperature, disease_idx, num2);
            else if (element.IsGas)
              storage.AddGasChunk(element.id, num1, temperature, disease_idx, num2, false);
            else
              storage.AddOre(element.id, num1, temperature, disease_idx, num2);
          }
          else
          {
            if (element == null)
            {
              GameObject prefab = Assets.GetPrefab(tag);
              GameObject go = GameUtil.KInstantiate(prefab, Grid.CellToPos(cell, CellAlignment.Top, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore);
              PrimaryElement component = go.GetComponent<PrimaryElement>();
              component.Mass = num1;
              component.AddDisease(disease_idx, num2, "CreatureCalorieMonitor.Poop");
              component.Temperature = temperature;
              go.SetActive(true);
              str = go.GetProperName();
              sprite = global::Def.GetUISprite((object) prefab).first;
            }
            else if (element.IsLiquid)
              FallingWater.instance.AddParticle(cell, element.idx, num1, temperature, disease_idx, num2, true);
            else if (element.IsGas)
              SimMessages.AddRemoveSubstance(cell, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num1, temperature, disease_idx, num2);
            else if (flag2)
            {
              int num3 = this.owner.GetComponent<Facing>().GetFrontCell();
              if (!Grid.IsValidCell(num3))
              {
                Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}", (object) this.owner, (object) element.name, (object) num3, (object) cell);
                num3 = cell;
              }
              SimMessages.AddRemoveSubstance(num3, element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num1, temperature, disease_idx, num2);
            }
            else
              element.substance.SpawnResource(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore), num1, temperature, disease_idx, num2);
            if (disease_delta > 0)
              SimMessages.ModifyDiseaseOnCell(cell, disease_idx, disease_delta);
          }
        }
        if ((double) this.GetTotalConsumedCalories() <= 0.0)
          this.shouldContinuingPooping = false;
        KPrefabID component1 = this.owner.GetComponent<KPrefabID>();
        if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component1.PrefabTag))
          Game.Instance.savedInfo.creaturePoopAmount.Add(component1.PrefabTag, 0.0f);
        Game.Instance.savedInfo.creaturePoopAmount[component1.PrefabTag] += num1;
        Sprite mainIcon = (UnityEngine.Object) customPopupImage == (UnityEngine.Object) null ? sprite : customPopupImage;
        string text = string.IsNullOrEmpty(customPopupMessage) ? str : customPopupMessage;
        PopFX popFx = PopFXManager.Instance.SpawnFX(mainIcon, PopFXManager.Instance.sprite_Plus, text, this.owner.transform, Vector3.zero);
        if ((UnityEngine.Object) popFx != (UnityEngine.Object) null)
          popFx.SetIconTint(color);
        this.owner.Trigger(-1844238272);
      }
    }

    public List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> GetCalorieEntries()
    {
      return this.caloriesConsumed;
    }

    public float GetTotalConsumedCalories()
    {
      float consumedCalories = 0.0f;
      foreach (CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry in this.caloriesConsumed)
      {
        if ((double) caloriesConsumedEntry.calories > 0.0)
        {
          Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
          if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
            consumedCalories += caloriesConsumedEntry.calories;
        }
      }
      return consumedCalories;
    }

    public float GetFullness()
    {
      return this.GetTotalConsumedCalories() / this.minConsumedCaloriesBeforePooping;
    }

    public bool IsReadyToPoop()
    {
      float consumedCalories = this.GetTotalConsumedCalories();
      if ((double) consumedCalories <= 0.0)
        return false;
      return this.shouldContinuingPooping || (double) consumedCalories >= (double) this.minConsumedCaloriesBeforePooping;
    }

    public void Consume(Tag tag, float calories)
    {
      for (int index = 0; index < this.caloriesConsumed.Count; ++index)
      {
        CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[index];
        if (caloriesConsumedEntry.tag == tag)
        {
          caloriesConsumedEntry.calories += calories;
          this.caloriesConsumed[index] = caloriesConsumedEntry;
          return;
        }
      }
      this.caloriesConsumed.Add(new CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry()
      {
        tag = tag,
        calories = calories
      });
    }

    public Tag GetNextPoopEntry()
    {
      for (int index = 0; index < this.caloriesConsumed.Count; ++index)
      {
        CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[index];
        if ((double) caloriesConsumedEntry.calories > 0.0)
        {
          Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
          if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
            return dietInfo.producedElement;
        }
      }
      return Tag.Invalid;
    }

    [Serializable]
    public struct CaloriesConsumedEntry
    {
      public Tag tag;
      public float calories;
    }
  }

  public new class Instance : 
    GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameInstance
  {
    public AmountInstance calories;
    [Serialize]
    public CreatureCalorieMonitor.Stomach stomach;
    public float lastMealOrPoopTime;
    public AttributeInstance metabolism;
    public AttributeModifier deltaCalorieMetabolismModifier;
    public KPrefabID prefabID;

    public float HungryRatio => this.def.hungryRatio;

    public Instance(IStateMachineTarget master, CreatureCalorieMonitor.Def def)
      : base(master, def)
    {
      this.calories = Db.Get().Amounts.Calories.Lookup(this.gameObject);
      this.calories.value = this.calories.GetMax() * 0.9f;
      this.stomach = new CreatureCalorieMonitor.Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, def.maxPoopSizeKG);
      this.metabolism = this.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
      this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, (string) DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, is_readonly: false);
      this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
    }

    public override void StartSM()
    {
      this.prefabID = this.gameObject.GetComponent<KPrefabID>();
      base.StartSM();
    }

    public void OnCaloriesConsumed(object data)
    {
      CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>) data).value;
      this.calories.value += caloriesConsumedEvent.calories;
      this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
      this.lastMealOrPoopTime = Time.time;
    }

    public float GetDeathTimeRemaining()
    {
      return this.smi.def.deathTimer - (GameClock.Instance.GetTime() - this.sm.starvationStartTime.Get(this.smi));
    }

    public void Poop() => this.Poop((PoopData) null);

    public void Poop(PoopData data)
    {
      this.lastMealOrPoopTime = Time.time;
      if (data == null)
        this.stomach.PoopInStorage((Storage) null);
      else
        this.stomach.PoopInStorage(data.storage, data.skipSpawningPoop, data.popupIcon, data.popupMessage);
    }

    public float GetCalories0to1() => this.calories.value / this.calories.GetMax();

    public bool IsHungry() => (double) this.GetCalories0to1() < (double) this.HungryRatio;

    public bool IsOutOfCalories() => (double) this.GetCalories0to1() <= 0.0;
  }
}
