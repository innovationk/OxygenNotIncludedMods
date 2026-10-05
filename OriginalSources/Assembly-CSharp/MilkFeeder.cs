// Decompiled with JetBrains decompiler
// Type: MilkFeeder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MilkFeeder : 
  GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>
{
  private const string TINT_METER_SYMBOL_NAME = "meter_fill";
  private const string TINT_SYMBOL_NAME = "Milk_fg";
  private const string TINT_SYMBOL2_NAME = "Milk_fill_fg";
  private MilkFeeder.OffState off;
  private MilkFeeder.OnState on;
  public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.BoolParameter isReadyToStartFeeding;
  public StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.ObjectParameter<DrinkMilkStates.Instance> currentFeedingCritter;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.root.Enter((StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback) (smi => smi.UpdateStorageMeter())).EventHandler(GameHashes.OnStorageChange, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback) (smi => smi.UpdateStorageMeter())).EventHandler(GameHashes.OnStorageChange, new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback(MilkFeeder.RefreshLiquidColor));
    this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State) this.on, new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback(MilkFeeder.ShouldBeOn)).EventTransition(GameHashes.BuildingStrawChange, (GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State) this.on, new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback(MilkFeeder.ShouldBeOn)).Enter(new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback(MilkFeeder.RefreshLiquidColor)).DefaultState(this.off.noOperational);
    this.off.noOperational.EventTransition(GameHashes.OperationalChanged, this.off.strawBlocked, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi => MilkFeeder.IsOperational(smi) && MilkFeeder.IsStrawBlocked(smi))).EventTransition(GameHashes.OperationalChanged, this.off.noLiquidOnStraw, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi => MilkFeeder.IsOperational(smi) && MilkFeeder.IsStrawOutsideLiquid(smi)));
    this.off.strawBlocked.ToggleStatusItem(Db.Get().BuildingStatusItems.OutputTileBlocked).EventTransition(GameHashes.OperationalChanged, this.off.noOperational, GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Not(new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback(MilkFeeder.IsOperational))).EventTransition(GameHashes.BuildingStrawChange, this.off.noLiquidOnStraw, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi => !MilkFeeder.IsStrawBlocked(smi) && MilkFeeder.IsStrawOutsideLiquid(smi)));
    this.off.noLiquidOnStraw.ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged).EventTransition(GameHashes.OperationalChanged, this.off.noOperational, GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Not(new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback(MilkFeeder.IsOperational))).EventTransition(GameHashes.BuildingStrawChange, this.off.strawBlocked, new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback(MilkFeeder.IsStrawBlocked));
    this.on.DefaultState(this.on.pre).EventTransition(GameHashes.BuildingStrawChange, this.on.pst, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi => !MilkFeeder.ShouldBeOn(smi) && smi.GetCurrentState() != this.on.pre)).EventTransition(GameHashes.BuildingStrawChange, (GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State) this.off, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi => !MilkFeeder.ShouldBeOn(smi) && smi.GetCurrentState() == this.on.pre)).EventTransition(GameHashes.OperationalChanged, this.on.pst, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi => !MilkFeeder.ShouldBeOn(smi) && smi.GetCurrentState() != this.on.pre)).EventTransition(GameHashes.OperationalChanged, (GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State) this.off, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi => !MilkFeeder.ShouldBeOn(smi) && smi.GetCurrentState() == this.on.pre)).Enter(new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback(MilkFeeder.RefreshLiquidColor));
    this.on.pre.PlayAnim("working_pre").OnAnimQueueComplete((GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State) this.on.working);
    this.on.working.PlayAnim("on").DefaultState(this.on.working.empty);
    this.on.working.empty.PlayAnim("empty").EnterTransition(this.on.working.refilling, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi => smi.HasEnoughMilkForOneFeeding())).EventHandler(GameHashes.OnStorageChange, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback) (smi =>
    {
      if (!smi.HasEnoughMilkForOneFeeding())
        return;
      smi.GoTo((StateMachine.BaseState) this.on.working.refilling);
    }));
    this.on.working.refilling.Enter(new StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback(MilkFeeder.RefreshLiquidColor)).PlayAnim("fill").OnAnimQueueComplete(this.on.working.full);
    this.on.working.full.PlayAnim("full").Enter((StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback) (smi => this.isReadyToStartFeeding.Set(true, smi))).Exit((StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback) (smi => this.isReadyToStartFeeding.Set(false, smi))).ParamTransition<DrinkMilkStates.Instance>((StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Parameter<DrinkMilkStates.Instance>) this.currentFeedingCritter, this.on.working.emptying, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Parameter<DrinkMilkStates.Instance>.Callback) ((smi, val) => val != null));
    this.on.working.emptying.EnterTransition(this.on.working.full, (StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.Transition.ConditionCallback) (smi =>
    {
      DrinkMilkMonitor.Instance smi1 = this.currentFeedingCritter.Get(smi).GetSMI<DrinkMilkMonitor.Instance>();
      return smi1 != null && !smi1.def.consumesMilk;
    })).PlayAnim("emptying").OnAnimQueueComplete(this.on.working.empty).Exit((StateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State.Callback) (smi => smi.StopFeeding()));
    this.on.pst.PlayAnim("working_pst").OnAnimQueueComplete((GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State) this.off);
  }

  public static bool ShouldBeOn(MilkFeeder.Instance smi)
  {
    return MilkFeeder.IsOperational(smi) && !MilkFeeder.IsStrawBlocked(smi) && !MilkFeeder.IsStrawOutsideLiquid(smi);
  }

  public static bool IsOperational(MilkFeeder.Instance smi) => smi.IsOperational;

  public static bool IsStrawBlocked(MilkFeeder.Instance smi) => smi.IsStrawBlocked;

  public static bool IsStrawOutsideLiquid(MilkFeeder.Instance smi) => smi.IsStrawOutsideLiquid;

  public static void RefreshLiquidColor(MilkFeeder.Instance smi) => smi.RefreshLiquidColor();

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public CellOffset drinkCellOffset;
    public Tag elementProducedTag;
    public float unitsProducedPerFeeding;
    public bool tintMeter;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      List<Descriptor> descs = new List<Descriptor>();
      go.GetSMI<MilkFeeder.Instance>();
      for (int index = 0; index < MilkFeederConfig.EffectsPerDrinkableLiquid.Length; ++index)
      {
        Tag first = MilkFeederConfig.EffectsPerDrinkableLiquid[index].first;
        string second = MilkFeederConfig.EffectsPerDrinkableLiquid[index].second;
        Descriptor descriptor = new Descriptor();
        descriptor.SetupDescriptor((string) Strings.Get($"STRINGS.CREATURES.MODIFIERS.{second.ToUpper()}.NAME"), "");
        descs.Add(descriptor);
        Effect.AddModifierDescriptions(descs, second, true, "STRINGS.CREATURES.STATS.");
      }
      return descs;
    }
  }

  public class OffState : 
    GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
  {
    public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State noOperational;
    public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State strawBlocked;
    public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State noLiquidOnStraw;
  }

  public class OnState : 
    GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
  {
    public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pre;
    public MilkFeeder.OnState.WorkingState working;
    public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State pst;

    public class WorkingState : 
      GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State
    {
      public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State empty;
      public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State refilling;
      public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State full;
      public GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.State emptying;
    }
  }

  public new class Instance : 
    GameStateMachine<MilkFeeder, MilkFeeder.Instance, IStateMachineTarget, MilkFeeder.Def>.GameInstance
  {
    public Storage milkStorage;
    public MeterController storageMeter;
    private CellOffset strawCellOffset = new CellOffset(0, 0);
    private Operational operational;
    private BuildingPointStraw straw;

    public bool IsOperational
    {
      get => (UnityEngine.Object) this.operational != (UnityEngine.Object) null && this.operational.IsOperational;
    }

    public bool IsStrawInstalled => (UnityEngine.Object) this.straw != (UnityEngine.Object) null;

    public bool IsStrawOutsideLiquid => this.IsStrawInstalled && !this.straw.isInLiquid;

    public bool IsStrawBlocked => this.IsStrawInstalled && this.straw.currentDepth <= 0;

    public Instance(IStateMachineTarget master, MilkFeeder.Def def)
      : base(master, def)
    {
      this.milkStorage = this.GetComponent<Storage>();
      this.operational = this.GetComponent<Operational>();
      this.straw = this.GetComponent<BuildingPointStraw>();
      this.storageMeter = new MeterController((KAnimControllerBase) this.smi.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
      this.Subscribe(360192579, new System.Action<object>(this.OnStrawChanged));
    }

    private void OnStrawChanged(object o)
    {
      this.strawCellOffset = ((BuildingPointStraw) o).GetBottomCellOffset();
    }

    public override void StartSM()
    {
      base.StartSM();
      Components.MilkFeeders.Add(this.smi.GetMyWorldId(), this);
      this.RefreshLiquidColor();
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      Components.MilkFeeders.Remove(this.smi.GetMyWorldId(), this);
    }

    public CellOffset GetDrinkCellOffset() => this.def.drinkCellOffset + this.strawCellOffset;

    public void UpdateStorageMeter()
    {
      this.storageMeter.SetPositionPercent(1f - Mathf.Clamp01(this.milkStorage.RemainingCapacity() / this.milkStorage.capacityKg));
    }

    public bool IsReserved() => this.HasTag(GameTags.Creatures.ReservedByCreature);

    public void SetReserved(bool isReserved)
    {
      if (isReserved)
      {
        Debug.Assert(!this.HasTag(GameTags.Creatures.ReservedByCreature));
        this.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
      }
      else if (this.HasTag(GameTags.Creatures.ReservedByCreature))
        this.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
      else
        Debug.LogWarningFormat((UnityEngine.Object) this.smi.gameObject, "Tried to unreserve a MilkFeeder that wasn't reserved");
    }

    public void RefreshLiquidColor()
    {
      PrimaryElement firstWithMass = this.milkStorage.FindFirstWithMass(this.def.elementProducedTag, this.def.unitsProducedPerFeeding);
      if ((UnityEngine.Object) firstWithMass == (UnityEngine.Object) null)
        return;
      Element elementByTag = ElementLoader.FindElementByTag(firstWithMass.PrefabID());
      if (this.def.tintMeter && (UnityEngine.Object) this.milkStorage != (UnityEngine.Object) null)
      {
        KBatchedAnimController meterController = this.storageMeter.meterController;
        if ((UnityEngine.Object) meterController != (UnityEngine.Object) null)
          GameUtil.TintLiquidSymbolOnBuilding("meter_fill", meterController, elementByTag);
      }
      KBatchedAnimController[] componentsInChildren = this.gameObject.GetComponentsInChildren<KBatchedAnimController>();
      if (componentsInChildren == null || componentsInChildren.Length == 0)
        return;
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        KBatchedAnimController controller = componentsInChildren[index];
        GameUtil.TintLiquidSymbolOnBuilding("Milk_fg", controller, elementByTag);
        GameUtil.TintLiquidSymbolOnBuilding("Milk_fill_fg", controller, elementByTag);
      }
    }

    public bool IsReadyToStartFeeding() => this.sm.isReadyToStartFeeding.Get(this.smi);

    public void RequestToStartFeeding(DrinkMilkStates.Instance feedingCritter)
    {
      this.sm.currentFeedingCritter.Set(feedingCritter, this.smi);
    }

    public void StopFeeding()
    {
      this.sm.currentFeedingCritter.Get(this.smi)?.RequestToStopFeeding();
      this.sm.currentFeedingCritter.Set((DrinkMilkStates.Instance) null, this.smi);
    }

    public bool HasEnoughMilkForOneFeeding()
    {
      return (UnityEngine.Object) this.milkStorage.FindFirstWithMass(this.def.elementProducedTag, this.def.unitsProducedPerFeeding) != (UnityEngine.Object) null;
    }

    public Tag ConsumeMilkForOneFeeding()
    {
      Tag tag = this.milkStorage.FindFirstWithMass(this.def.elementProducedTag, this.def.unitsProducedPerFeeding).PrefabID();
      this.milkStorage.ConsumeIgnoringDisease(tag, this.def.unitsProducedPerFeeding);
      return tag;
    }

    public bool IsInCreaturePenRoom()
    {
      Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject);
      return roomOfGameObject != null && roomOfGameObject.roomType == Db.Get().RoomTypes.CreaturePen;
    }
  }
}
