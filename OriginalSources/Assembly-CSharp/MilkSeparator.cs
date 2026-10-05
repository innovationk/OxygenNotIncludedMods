// Decompiled with JetBrains decompiler
// Type: MilkSeparator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MilkSeparator : 
  GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>
{
  public const string FAT_IN_METER_COLOR_SYMBOL_NAME = "meter_fat";
  public const string FAT_COLOR_SYMBOL_NAME = "fat";
  public const string INPUT_LIQUID_SYMBOL_NAME = "liquid_reservoir";
  public const string OUTPUT_LIQUID_SYMBOL_NAME = "meter_liquid_cycle";
  public const string WORK_PRE_ANIM_NAME = "separating_pre";
  public const string WORK_ANIM_NAME = "separating_loop";
  public const string WORK_POST_ANIM_NAME = "separating_pst";
  public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State noOperational;
  public MilkSeparator.OperationalStates operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.noOperational;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.EventHandler(GameHashes.OnConduitObjectDispensed, new GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.GameEvent.Callback(MilkSeparator.RefreshLastObjectDispensed)).EventHandler(GameHashes.OnStorageChange, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.RefreshMeters));
    this.noOperational.TagTransition(GameTags.Operational, (GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State) this.operational).PlayAnim("off");
    this.operational.TagTransition(GameTags.Operational, this.noOperational, true).PlayAnim("on").DefaultState(this.operational.idle);
    this.operational.idle.EventTransition(GameHashes.OnStorageChange, this.operational.working.pre, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanBeginSeparate)).EnterTransition(this.operational.full, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.RequiresEmptying));
    this.operational.working.pre.QueueAnim("separating_pre").OnAnimQueueComplete(this.operational.working.work);
    this.operational.working.work.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.BeginSeparation)).PlayAnim("separating_loop", KAnim.PlayMode.Loop).Update(new System.Action<MilkSeparator.Instance, float>(MilkSeparator.SpawnCaviar)).ToggleStatusItem((Func<MilkSeparator.Instance, StatusItem>) (smi => !smi.IsProducingCaviar() ? (StatusItem) null : Db.Get().BuildingStatusItems.MilkSeparatorProducingCaviar), (Func<MilkSeparator.Instance, object>) (smi => (object) smi)).EventTransition(GameHashes.OnStorageChange, this.operational.working.post, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanNOTKeepSeparating)).Exit(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.EndSeparation));
    this.operational.working.post.QueueAnim("separating_pst").OnAnimQueueComplete(this.operational.idle);
    this.operational.full.PlayAnim("ready").ToggleRecurringChore(new Func<MilkSeparator.Instance, Chore>(MilkSeparator.CreateEmptyChore)).WorkableCompleteTransition((Func<MilkSeparator.Instance, Workable>) (smi => (Workable) smi.workable), this.operational.emptyComplete).ToggleStatusItem(Db.Get().BuildingStatusItems.MilkSeparatorNeedsEmptying);
    this.operational.emptyComplete.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.DropMilkFat)).ScheduleActionNextFrame("AfterMilkFatDrop", (System.Action<MilkSeparator.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.operational.idle)));
  }

  public static void SpawnCaviar(MilkSeparator.Instance smi, float dt) => smi.SpawnCaviar(dt);

  public static void BeginSeparation(MilkSeparator.Instance smi) => smi.operational.SetActive(true);

  public static void EndSeparation(MilkSeparator.Instance smi) => smi.operational.SetActive(false);

  public static bool CanBeginSeparate(MilkSeparator.Instance smi)
  {
    return !smi.MilkFatLimitReached && smi.HasEnoughMassToStartConverting();
  }

  public static bool CanKeepSeparating(MilkSeparator.Instance smi)
  {
    return !smi.MilkFatLimitReached && smi.CanConvertAtAll();
  }

  public static bool CanNOTKeepSeparating(MilkSeparator.Instance smi)
  {
    return !MilkSeparator.CanKeepSeparating(smi);
  }

  public static bool RequiresEmptying(MilkSeparator.Instance smi) => smi.MilkFatLimitReached;

  public static bool ThereIsCapacityForMilkFat(MilkSeparator.Instance smi)
  {
    return !smi.MilkFatLimitReached;
  }

  public static void DropMilkFat(MilkSeparator.Instance smi) => smi.DropMilkFat();

  public static void RefreshLastObjectDispensed(MilkSeparator.Instance smi, object o)
  {
    smi.RefreshLastObjectDispensed(o);
  }

  public static void RefreshMeters(MilkSeparator.Instance smi) => smi.RefreshMeters();

  private static Chore CreateEmptyChore(MilkSeparator.Instance smi)
  {
    WorkChore<EmptyMilkSeparatorWorkable> emptyChore = new WorkChore<EmptyMilkSeparatorWorkable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) smi.workable);
    emptyChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, (object) null);
    return (Chore) emptyChore;
  }

  public class Def : StateMachine.BaseDef, IConverterByproduct
  {
    public float MILK_FAT_CAPACITY = 100f;
    public float CAVIAR_PRODUCTION_RATE;
    public Tag MILK_TAG;
    public Tag MILK_FAT_TAG;
    public Tag CAVIAR_TAG;
    public Tag FISHMILK_TAG;
    public Tag MILK_SEPARATED_LIQUID_OUTPUT_TAG;
    public Tag FISHMILK_SEPARATED_LIQUID_OUTPUT_TAG;

    public Tag ByproductAssociatedInputTag => this.FISHMILK_TAG;

    public Tag ByproductTag => this.CAVIAR_TAG;

    public float ByproductRate => this.CAVIAR_PRODUCTION_RATE;

    public bool ByproductIsContinuous => true;

    public Def()
    {
      this.MILK_FAT_TAG = ElementLoader.FindElementByHash(SimHashes.MilkFat).tag;
      this.MILK_TAG = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
      this.FISHMILK_TAG = ElementLoader.FindElementByHash(SimHashes.FishMilk).tag;
      this.MILK_SEPARATED_LIQUID_OUTPUT_TAG = ElementLoader.FindElementByHash(SimHashes.Brine).tag;
      this.FISHMILK_SEPARATED_LIQUID_OUTPUT_TAG = ElementLoader.FindElementByHash(SimHashes.Mucus).tag;
      this.CAVIAR_TAG = new Tag("Caviar");
    }

    public void GetByproductDescriptors(GameObject obj, List<Descriptor> descriptors)
    {
      if ((double) this.CAVIAR_PRODUCTION_RATE <= 0.0)
        return;
      string str = this.CAVIAR_TAG.ProperName();
      string formattedMass = GameUtil.GetFormattedMass(this.CAVIAR_PRODUCTION_RATE, GameUtil.TimeSlice.PerSecond, floatFormat: "{0:0.##}");
      descriptors.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTED_INPUTTEMP, (object) str, (object) formattedMass), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_INPUTTEMP, (object) str, (object) formattedMass)));
    }
  }

  public class WorkingStates : 
    GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
  {
    public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State pre;
    public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State work;
    public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State post;
  }

  public class OperationalStates : 
    GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
  {
    public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State idle;
    public MilkSeparator.WorkingStates working;
    public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State full;
    public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State emptyComplete;
  }

  public new class Instance : 
    GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.GameInstance
  {
    [MyCmpGet]
    public EmptyMilkSeparatorWorkable workable;
    [MyCmpGet]
    public Operational operational;
    [MyCmpGet]
    private Storage storage;
    private ElementConverter[] elementConverters;
    private SymbolOverrideController symbolOverrideController;
    private float caviarMassAccumulated;
    private KBatchedAnimController animController;
    private MeterController fatMeter;
    private Tag lastObjectDispensedTag = Tag.Invalid;

    public float SolidOutputStored => this.MilkFatStored + this.CaviarStored;

    public float CaviarStored => this.storage.GetMassAvailable(this.def.CAVIAR_TAG);

    public float MilkFatStored => this.storage.GetMassAvailable(this.def.MILK_FAT_TAG);

    public float MilkStored => this.storage.GetMassAvailable(this.def.MILK_TAG);

    public float FishMilkStored => this.storage.GetMassAvailable(this.def.FISHMILK_TAG);

    public float SolidOutputStoragePercentage
    {
      get => Mathf.Clamp(this.SolidOutputStored / this.def.MILK_FAT_CAPACITY, 0.0f, 1f);
    }

    public bool MilkFatLimitReached
    {
      get => (double) this.SolidOutputStored >= (double) this.def.MILK_FAT_CAPACITY;
    }

    public Instance(IStateMachineTarget master, MilkSeparator.Def def)
      : base(master, def)
    {
      this.animController = this.GetComponent<KBatchedAnimController>();
      this.fatMeter = new MeterController((KAnimControllerBase) this.animController, "meter_target_1", "meter_fat", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[1]
      {
        "meter_target_1"
      });
      this.elementConverters = master.gameObject.GetComponents<ElementConverter>();
    }

    public override void StartSM()
    {
      base.StartSM();
      this.workable.OnWork_PST_Begins = new System.Action(this.Play_Empty_MeterAnimation);
      this.RefreshMeters();
    }

    private void Play_Empty_MeterAnimation()
    {
      this.fatMeter.SetPositionPercent(0.0f);
      this.fatMeter.meterController.Play((HashedString) "meter_fat_empty");
    }

    public bool HasEnoughMassToStartConverting()
    {
      for (int index = 0; index < this.elementConverters.Length; ++index)
      {
        if (this.elementConverters[index].HasEnoughMassToStartConverting())
          return true;
      }
      return false;
    }

    public bool CanConvertAtAll()
    {
      for (int index = 0; index < this.elementConverters.Length; ++index)
      {
        if (this.elementConverters[index].CanConvertAtAll())
          return true;
      }
      return false;
    }

    public bool IsProducingCaviar()
    {
      return (double) this.def.CAVIAR_PRODUCTION_RATE > 0.0 && (double) this.storage.GetAmountAvailable(this.def.FISHMILK_TAG) > 0.0;
    }

    public void SpawnCaviar(float dt)
    {
      if ((double) this.def.CAVIAR_PRODUCTION_RATE <= 0.0 || (double) this.storage.GetAmountAvailable(this.def.FISHMILK_TAG) <= 0.0)
        return;
      this.caviarMassAccumulated += this.def.CAVIAR_PRODUCTION_RATE * dt;
      if ((double) this.caviarMassAccumulated < 1.0)
        return;
      GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(this.def.CAVIAR_TAG), Grid.SceneLayer.Ore);
      go.GetComponent<PrimaryElement>().Units = (float) Mathf.FloorToInt(this.caviarMassAccumulated / 1f);
      go.SetActive(true);
      this.storage.Store(go, true);
      this.caviarMassAccumulated %= 1f;
    }

    public void DropMilkFat()
    {
      List<GameObject> obj_list = new List<GameObject>();
      this.storage.Drop(this.def.MILK_FAT_TAG, obj_list);
      this.storage.Drop(this.def.CAVIAR_TAG, obj_list);
      Vector3 dropSpawnLocation = this.GetDropSpawnLocation();
      foreach (GameObject gameObject in obj_list)
        gameObject.transform.position = dropSpawnLocation;
    }

    private Vector3 GetDropSpawnLocation()
    {
      Vector3 column = (Vector3) this.animController.GetSymbolTransform(new HashedString("object"), out bool _).GetColumn(3) with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.Ore)
      };
      int cell = Grid.PosToCell(column);
      return Grid.IsValidCell(cell) && !Grid.Solid[cell] ? column : this.transform.GetPosition();
    }

    public void RefreshLastObjectDispensed(object o)
    {
      if (o == null)
        return;
      PrimaryElement primaryElement = o as PrimaryElement;
      if ((UnityEngine.Object) primaryElement == (UnityEngine.Object) null)
        return;
      this.lastObjectDispensedTag = primaryElement.Element.tag;
    }

    public Color GetFatColor()
    {
      bool flag1 = (double) this.MilkFatStored >= (double) this.CaviarStored;
      bool flag2 = (double) this.MilkStored >= (double) this.FishMilkStored;
      return !(((double) this.MilkFatStored > 0.0 ? 0 : ((double) this.CaviarStored <= 0.0 ? 1 : 0)) != 0 ? flag2 : flag1) ? CaviarTuning.COLOR : (Color) ElementLoader.FindElementByTag(this.def.MILK_FAT_TAG).substance.colour;
    }

    public void RefreshMeters()
    {
      if (this.fatMeter.meterController.currentAnim != (HashedString) "meter_fat")
        this.fatMeter.meterController.Play((HashedString) "meter_fat", KAnim.PlayMode.Paused);
      this.fatMeter.SetPositionPercent(this.SolidOutputStoragePercentage);
      double milkFatStored = (double) this.MilkFatStored;
      double caviarStored = (double) this.CaviarStored;
      Tag tag1 = (double) this.MilkStored >= (double) this.FishMilkStored ? this.def.MILK_TAG : this.def.FISHMILK_TAG;
      Tag tag2 = this.lastObjectDispensedTag != Tag.Invalid ? this.lastObjectDispensedTag : (tag1 == this.def.FISHMILK_TAG ? this.def.FISHMILK_SEPARATED_LIQUID_OUTPUT_TAG : this.def.MILK_SEPARATED_LIQUID_OUTPUT_TAG);
      Color fatColor = this.GetFatColor();
      this.fatMeter.meterController.SetSymbolTint(new KAnimHashedString("meter_fat"), fatColor);
      this.animController.SetSymbolTint((KAnimHashedString) "fat", fatColor);
      GameUtil.TintLiquidSymbolOnBuilding("liquid_reservoir", this.animController, ElementLoader.FindElementByTag(tag1));
      GameUtil.TintLiquidSymbolOnBuilding("meter_liquid_cycle", this.animController, ElementLoader.FindElementByTag(tag2));
    }
  }
}
