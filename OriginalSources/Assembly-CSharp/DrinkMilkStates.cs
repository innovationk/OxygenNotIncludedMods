// Decompiled with JetBrains decompiler
// Type: DrinkMilkStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Linq;
using UnityEngine;

#nullable disable
public class DrinkMilkStates : 
  GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>
{
  public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State goingToDrink;
  public DrinkMilkStates.EatingState drink;
  public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State behaviourComplete;
  public StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.TargetParameter targetMilkFeeder;
  public StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.Signal requestedToStopFeeding;

  private static void SetSceneLayer(DrinkMilkStates.Instance smi, Grid.SceneLayer layer)
  {
    SegmentedCreature.Instance smi1 = smi.GetSMI<SegmentedCreature.Instance>();
    if (smi1 != null && smi1.segments != null)
    {
      foreach (SegmentedCreature.CreatureSegment creatureSegment in smi1.segments.Reverse<SegmentedCreature.CreatureSegment>())
        creatureSegment.animController.SetSceneLayer(layer);
    }
    else
      smi.GetComponent<KBatchedAnimController>().SetSceneLayer(layer);
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingToDrink;
    this.root.Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.SetTarget)).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.CheckIfCramped)).Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.ReserveMilkFeeder)).Exit(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.UnreserveMilkFeeder)).Transition(this.behaviourComplete, (StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.Transition.ConditionCallback) (smi =>
    {
      MilkFeeder.Instance targetMilkFeeder = DrinkMilkStates.GetTargetMilkFeeder(smi);
      if (!targetMilkFeeder.IsNullOrDestroyed() && MilkFeeder.ShouldBeOn(targetMilkFeeder))
        return false;
      smi.GetComponent<KAnimControllerBase>().Queue((HashedString) "idle_loop", KAnim.PlayMode.Loop);
      return true;
    }));
    GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State state1 = this.goingToDrink.Target(this.targetMilkFeeder).EventHandlerTransition(GameHashes.BuildingStrawChange, (GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State) null, (Func<DrinkMilkStates.Instance, object, bool>) ((smi, obj) => true)).Target(this.masterTarget).MoveTo(new Func<DrinkMilkStates.Instance, int>(DrinkMilkStates.GetCellToDrinkFrom), (GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State) this.drink);
    string name1 = (string) CREATURES.STATUSITEMS.LOOKINGFORMILK.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.LOOKINGFORMILK.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state1.ToggleStatusItem(name1, tooltip1, render_overlay: render_overlay1, category: category1);
    GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State state2 = this.drink.DefaultState(this.drink.pre).Enter("FaceMilkFeeder", new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.FaceMilkFeeder));
    string name2 = (string) CREATURES.STATUSITEMS.DRINKINGMILK.NAME;
    string tooltip2 = (string) CREATURES.STATUSITEMS.DRINKINGMILK.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state2.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2).Enter((StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback) (smi => DrinkMilkStates.SetSceneLayer(smi, smi.def.shouldBeBehindMilkTank ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Creatures))).Exit((StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback) (smi => DrinkMilkStates.SetSceneLayer(smi, Grid.SceneLayer.Creatures)));
    this.drink.pre.Target(this.targetMilkFeeder).EventHandlerTransition(GameHashes.BuildingStrawChange, (GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State) null, (Func<DrinkMilkStates.Instance, object, bool>) ((smi, obj) => true)).Target(this.masterTarget).QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkPre)).OnAnimQueueComplete(this.drink.loop);
    this.drink.loop.Target(this.targetMilkFeeder).EventHandlerTransition(GameHashes.BuildingStrawChange, (GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State) null, (Func<DrinkMilkStates.Instance, object, bool>) ((smi, obj) => true)).Target(this.masterTarget).QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkLoop), true).Enter((StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback) (smi =>
    {
      MilkFeeder.Instance targetMilkFeeder = DrinkMilkStates.GetTargetMilkFeeder(smi);
      if (targetMilkFeeder != null)
        targetMilkFeeder.RequestToStartFeeding(smi);
      else
        smi.GoTo((StateMachine.BaseState) this.drink.pst);
    })).OnSignal(this.requestedToStopFeeding, this.drink.pst);
    this.drink.pst.Enter(new StateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State.Callback(DrinkMilkStates.DrinkMilkComplete)).QueueAnim(new Func<DrinkMilkStates.Instance, string>(DrinkMilkStates.GetAnimDrinkPst)).OnAnimQueueComplete(this.behaviourComplete);
    this.behaviourComplete.QueueAnim("idle_loop", true).BehaviourComplete(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder);
  }

  private static MilkFeeder.Instance GetTargetMilkFeeder(DrinkMilkStates.Instance smi)
  {
    if (smi.sm.targetMilkFeeder.IsNullOrDestroyed())
      return (MilkFeeder.Instance) null;
    GameObject go = smi.sm.targetMilkFeeder.Get(smi);
    if (go.IsNullOrDestroyed())
      return (MilkFeeder.Instance) null;
    MilkFeeder.Instance smi1 = go.GetSMI<MilkFeeder.Instance>();
    return go.IsNullOrDestroyed() || smi1.IsNullOrStopped() ? (MilkFeeder.Instance) null : smi1;
  }

  private static void SetTarget(DrinkMilkStates.Instance smi)
  {
    smi.sm.targetMilkFeeder.Set(smi.GetSMI<DrinkMilkMonitor.Instance>().targetMilkFeeder.gameObject, smi, false);
  }

  private static void CheckIfCramped(DrinkMilkStates.Instance smi)
  {
    smi.critterIsCramped = smi.GetSMI<DrinkMilkMonitor.Instance>().doesTargetMilkFeederHaveSpaceForCritter;
  }

  private static void ReserveMilkFeeder(DrinkMilkStates.Instance smi)
  {
    DrinkMilkStates.GetTargetMilkFeeder(smi)?.SetReserved(true);
  }

  private static void UnreserveMilkFeeder(DrinkMilkStates.Instance smi)
  {
    DrinkMilkStates.GetTargetMilkFeeder(smi)?.SetReserved(false);
  }

  private static void DrinkMilkComplete(DrinkMilkStates.Instance smi)
  {
    MilkFeeder.Instance targetMilkFeeder = DrinkMilkStates.GetTargetMilkFeeder(smi);
    if (targetMilkFeeder == null)
      return;
    smi.GetSMI<DrinkMilkMonitor.Instance>().NotifyFinishedDrinkingMilkFrom(targetMilkFeeder);
  }

  private static int GetCellToDrinkFrom(DrinkMilkStates.Instance smi)
  {
    MilkFeeder.Instance targetMilkFeeder = DrinkMilkStates.GetTargetMilkFeeder(smi);
    return targetMilkFeeder == null ? Grid.InvalidCell : smi.GetSMI<DrinkMilkMonitor.Instance>().GetDrinkCellOf(targetMilkFeeder, smi.critterIsCramped);
  }

  private static string GetAnimDrinkPre(DrinkMilkStates.Instance smi)
  {
    return smi.critterIsCramped ? "drink_cramped_pre" : "drink_pre";
  }

  private static string GetAnimDrinkLoop(DrinkMilkStates.Instance smi)
  {
    return smi.critterIsCramped ? "drink_cramped_loop" : "drink_loop";
  }

  private static string GetAnimDrinkPst(DrinkMilkStates.Instance smi)
  {
    return smi.critterIsCramped ? "drink_cramped_pst" : "drink_pst";
  }

  private static void FaceMilkFeeder(DrinkMilkStates.Instance smi)
  {
    MilkFeeder.Instance targetMilkFeeder = DrinkMilkStates.GetTargetMilkFeeder(smi);
    if (targetMilkFeeder == null)
      return;
    bool isRotated = targetMilkFeeder.GetComponent<Rotatable>().IsRotated;
    float num = !smi.critterIsCramped ? (!isRotated ? -20f : 20f) : (!isRotated ? 20f : -20f);
    IApproachable approachable = smi.sm.targetMilkFeeder.Get<IApproachable>(smi);
    if (approachable == null)
      return;
    float target_x = approachable.transform.GetPosition().x + num;
    smi.GetComponent<Facing>().Face(target_x);
  }

  public class Def : StateMachine.BaseDef
  {
    public bool shouldBeBehindMilkTank = true;
    public DrinkMilkStates.Def.DrinkCellOffsetGetFn drinkCellOffsetGetFn = new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_CritterOneByOne);

    public static CellOffset DrinkCellOffsetGet_CritterOneByOne(
      MilkFeeder.Instance milkFeederInstance,
      DrinkMilkMonitor.Instance critterInstance,
      bool isCramped)
    {
      return milkFeederInstance.GetComponent<Rotatable>().GetRotatedCellOffset(milkFeederInstance.GetDrinkCellOffset());
    }

    public static CellOffset DrinkCellOffsetGet_GassyMoo(
      MilkFeeder.Instance milkFeederInstance,
      DrinkMilkMonitor.Instance critterInstance,
      bool isCramped)
    {
      Rotatable component = milkFeederInstance.GetComponent<Rotatable>();
      CellOffset rotatedCellOffset = component.GetRotatedCellOffset(milkFeederInstance.GetDrinkCellOffset());
      if (component.IsRotated)
        --rotatedCellOffset.x;
      if (isCramped)
      {
        if (component.IsRotated)
          rotatedCellOffset.x += 2;
        else
          rotatedCellOffset.x -= 2;
      }
      return rotatedCellOffset;
    }

    public static CellOffset DrinkCellOffsetGet_TwoByTwo(
      MilkFeeder.Instance milkFeederInstance,
      DrinkMilkMonitor.Instance critterInstance,
      bool isCramped)
    {
      Rotatable component = milkFeederInstance.GetComponent<Rotatable>();
      CellOffset rotatedCellOffset = component.GetRotatedCellOffset(milkFeederInstance.GetDrinkCellOffset());
      if (!isCramped)
      {
        int x1 = Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) milkFeederInstance), rotatedCellOffset)).x;
        int x2 = Grid.PosToXY(critterInstance.transform.position).x;
        if (x1 > x2 && !component.IsRotated)
          ++rotatedCellOffset.x;
        else if (x1 < x2 && component.IsRotated)
          --rotatedCellOffset.x;
        else if (x1 == x2)
        {
          if (component.IsRotated)
            --rotatedCellOffset.x;
          else
            ++rotatedCellOffset.x;
        }
      }
      else if (component.IsRotated)
        ++rotatedCellOffset.x;
      else
        --rotatedCellOffset.x;
      return rotatedCellOffset;
    }

    public delegate CellOffset DrinkCellOffsetGetFn(
      MilkFeeder.Instance milkFeederInstance,
      DrinkMilkMonitor.Instance critterInstance,
      bool isCramped);
  }

  public new class Instance : 
    GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.GameInstance
  {
    public bool critterIsCramped;

    public Instance(Chore<DrinkMilkStates.Instance> chore, DrinkMilkStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder);
    }

    public void RequestToStopFeeding() => this.sm.requestedToStopFeeding.Trigger(this.smi);
  }

  public class EatingState : 
    GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State
  {
    public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State pre;
    public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State loop;
    public GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>.State pst;
  }
}
