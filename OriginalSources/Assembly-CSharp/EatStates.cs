// Decompiled with JetBrains decompiler
// Type: EatStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class EatStates : 
  GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>
{
  private static Effect PredationStunEffect = EatStates.CreatePredationStunEffect();
  public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.ApproachSubState<Pickupable> goingtoeat;
  public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State arrivedAtEdible;
  public EatStates.PounceState pounce;
  public EatStates.EatingState eating;
  public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State failedHunt;
  public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State behaviourcomplete;
  public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.Vector3Parameter offset;
  public StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.TargetParameter target;
  private static float HUNT_WILD_MIN_AGE = 0.825f;
  private static MathUtil.MinMax HUNT_WILD_PRED_RATE = new MathUtil.MinMax(0.1f, 1.1f);
  private static MathUtil.MinMax HUNT_TAME_PRED_RATE = new MathUtil.MinMax(0.4f, 1.05f);

  private static Effect CreatePredationStunEffect()
  {
    return new Effect("StunnedEat", "", "", 5f, false, false, true, "")
    {
      tag = new Tag?(GameTags.Creatures.StunnedBeingEaten)
    };
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.goingtoeat;
    this.root.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.SetTarget)).Exit(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.UnreserveEdible));
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state1 = this.goingtoeat.MoveTo(new Func<EatStates.Instance, int>(EatStates.GetEdibleCell), this.arrivedAtEdible, this.behaviourcomplete, false);
    string name1 = (string) CREATURES.STATUSITEMS.HUNGRY.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.HUNGRY.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state1.ToggleStatusItem(name1, tooltip1, render_overlay: render_overlay1, category: category1);
    this.arrivedAtEdible.EnterTransition((GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State) this.pounce, (StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.Transition.ConditionCallback) (smi => smi.IsPredator)).Transition((GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State) this.eating, (StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.Transition.ConditionCallback) (smi => !smi.IsPredator));
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state2 = this.pounce.Face(this.target).DefaultState(this.pounce.pre);
    string name2 = (string) CREATURES.STATUSITEMS.HUNTING.NAME;
    string tooltip2 = (string) CREATURES.STATUSITEMS.HUNTING.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    state2.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2);
    this.pounce.pre.PlayAnim("pounce_pre").OnAnimQueueComplete(this.pounce.roll);
    this.pounce.roll.Enter((StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback) (smi =>
    {
      if (EatStates.CheckHuntSuccess(smi))
        smi.GoTo((StateMachine.BaseState) this.pounce.hit);
      else
        smi.GoTo((StateMachine.BaseState) this.pounce.miss);
    }));
    this.pounce.hit.Enter((StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback) (smi => EatStates.FreezeEdible(smi))).QueueAnim("pounce_hit").OnAnimQueueComplete((GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State) this.eating);
    this.pounce.miss.Enter((StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback) (smi => EatStates.OnPounceMiss(smi))).QueueAnim("pounce_miss").OnAnimQueueComplete(this.failedHunt);
    this.failedHunt.PlayAnim("idle_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, (StateMachine.BaseState) this.behaviourcomplete);
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State state3 = this.eating.EnterTransition(this.behaviourcomplete, (StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.Transition.ConditionCallback) (smi => EatStates.EdibleGotAway(smi))).Face(this.target).DefaultState(this.eating.pre);
    string name3 = (string) CREATURES.STATUSITEMS.EATING.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.EATING.TOOLTIP;
    StatusItemCategory main3 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay3 = new HashedString();
    StatusItemCategory category3 = main3;
    state3.ToggleStatusItem(name3, tooltip3, render_overlay: render_overlay3, category: category3);
    this.eating.pre.Enter((StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback) (smi => EatStates.FreezeEdible(smi))).QueueAnim((Func<EatStates.Instance, string>) (smi => smi.eatAnims[0])).OnAnimQueueComplete(this.eating.loop);
    this.eating.loop.Enter(new StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback(EatStates.EatComplete)).QueueAnim((Func<EatStates.Instance, string>) (smi => smi.eatAnims[1])).OnAnimQueueComplete(this.eating.pst);
    this.eating.pst.QueueAnim((Func<EatStates.Instance, string>) (smi => smi.eatAnims[2])).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.Enter((StateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State.Callback) (smi => smi.solidConsumer.ClearTargetEdible())).PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.WantsToEat);
  }

  private static void SetTarget(EatStates.Instance smi)
  {
    smi.solidConsumer = smi.GetSMI<SolidConsumerMonitor.Instance>();
    smi.sm.target.Set(smi.solidConsumer.targetEdible, smi, false);
    EatStates.ReserveEdible(smi);
    smi.OverrideEatAnims(smi, smi.solidConsumer.GetTargetEdibleEatAnims());
    smi.sm.offset.Set(smi.solidConsumer.targetEdibleOffset, smi);
  }

  private static void ReserveEdible(EatStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    DebugUtil.Assert(!go.HasTag(GameTags.Creatures.ReservedByCreature));
    go.AddTag(GameTags.Creatures.ReservedByCreature);
  }

  private static void UnreserveEdible(EatStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    if (go.HasTag(GameTags.Creatures.ReservedByCreature))
      go.RemoveTag(GameTags.Creatures.ReservedByCreature);
    else
      Debug.LogWarningFormat((UnityEngine.Object) smi.gameObject, "{0} UnreserveEdible but it wasn't reserved: {1}", (object) smi.gameObject, (object) go);
  }

  private static void EatComplete(EatStates.Instance smi)
  {
    PrimaryElement primaryElement = smi.sm.target.Get<PrimaryElement>(smi);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
      smi.lastMealElement = primaryElement.Element;
    smi.Trigger(1386391852, (object) smi.sm.target.Get<KPrefabID>(smi));
  }

  private static bool EdibleGotAway(EatStates.Instance smi)
  {
    int edibleCell = EatStates.GetEdibleCell(smi);
    return Grid.PosToCell((StateMachine.Instance) smi) != edibleCell;
  }

  private static void FreezeEdible(EatStates.Instance smi)
  {
    if (!smi.IsPredator)
      return;
    GameObject gameObject = smi.sm.target.Get(smi);
    Effects component1 = gameObject.GetComponent<Effects>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.Add(EatStates.PredationStunEffect, false);
    Brain component2 = gameObject.GetComponent<Brain>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    Game.BrainScheduler.PrioritizeBrain(component2);
  }

  private static void OnPounceMiss(EatStates.Instance smi)
  {
    smi.GetComponent<Effects>().Add("PredatorFailedHunt", true);
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    go.Trigger(-787691065, (object) smi.GetComponent<FactionAlignment>());
  }

  private static bool HuntPredicateWild(GameObject obj)
  {
    if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
      return false;
    AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(obj);
    if (amountInstance == null)
      return true;
    float t = amountInstance.value / amountInstance.GetMax();
    return (double) t >= (double) EatStates.HUNT_WILD_MIN_AGE && (double) UnityEngine.Random.Range(0.0f, 1f) < (double) EatStates.HUNT_WILD_PRED_RATE.Lerp(t);
  }

  private static bool HuntPredicateTame(GameObject obj)
  {
    if ((UnityEngine.Object) obj == (UnityEngine.Object) null)
      return false;
    AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(obj);
    if (amountInstance == null)
      return true;
    float t = amountInstance.value / amountInstance.GetMax();
    return (double) UnityEngine.Random.Range(0.0f, 1f) < (double) EatStates.HUNT_TAME_PRED_RATE.Lerp(t);
  }

  private static bool CheckHuntSuccess(EatStates.Instance smi)
  {
    WildnessMonitor.Instance smi1 = smi.gameObject.GetSMI<WildnessMonitor.Instance>();
    GameObject go = smi.sm.target.Get(smi);
    WildnessMonitor.Instance smi2 = (UnityEngine.Object) go != (UnityEngine.Object) null ? go.GetSMI<WildnessMonitor.Instance>() : (WildnessMonitor.Instance) null;
    return ((smi1 == null ? 0 : (smi1.IsWild() ? 1 : 0)) & (smi2 == null ? (false ? 1 : 0) : (smi2.IsWild() ? 1 : 0))) != 0 ? EatStates.HuntPredicateWild(go) : EatStates.HuntPredicateTame(go);
  }

  private static int GetEdibleCell(EatStates.Instance smi)
  {
    return (UnityEngine.Object) smi.Edible == (UnityEngine.Object) null ? Grid.InvalidCell : Grid.PosToCell(smi.Edible.transform.GetPosition() + smi.sm.offset.Get(smi));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.GameInstance
  {
    public Element lastMealElement;
    public SolidConsumerMonitor.Instance solidConsumer;
    public string[] eatAnims = new string[3]
    {
      "eat_pre",
      "eat_loop",
      "eat_pst"
    };

    public GameObject Edible => this.smi.sm.target.Get(this);

    public bool IsPredator { get; private set; }

    public void OverrideEatAnims(EatStates.Instance smi, string[] preLoopPstAnims)
    {
      Debug.Assert(preLoopPstAnims != null && preLoopPstAnims.Length == 3);
      smi.eatAnims = preLoopPstAnims;
    }

    public Instance(Chore<EatStates.Instance> chore, EatStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToEat);
      chore.AddPrecondition(ChorePreconditions.instance.DoesntHaveTag, (object) GameTags.Creatures.SuppressedDiet);
      this.IsPredator = this.gameObject.GetComponent<FactionAlignment>().Alignment == FactionManager.FactionID.Predator;
    }

    public Element GetLatestMealElement() => this.lastMealElement;
  }

  public class PounceState : 
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
  {
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State roll;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State hit;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State miss;
  }

  public class EatingState : 
    GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State
  {
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pre;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State loop;
    public GameStateMachine<EatStates, EatStates.Instance, IStateMachineTarget, EatStates.Def>.State pst;
  }
}
