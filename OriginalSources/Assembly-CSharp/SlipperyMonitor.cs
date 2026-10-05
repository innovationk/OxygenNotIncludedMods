// Decompiled with JetBrains decompiler
// Type: SlipperyMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class SlipperyMonitor : 
  GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>
{
  public const string EFFECT_NAME = "RecentlySlippedTracker";
  public const float SLIP_FAIL_TIMEOUT = 8f;
  public const float PROBABILITY_OF_SLIP = 0.05f;
  public const float STRESS_DAMAGE = 3f;
  public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State safe;
  public SlipperyMonitor.UnsafeCellState unsafeCell;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.safe;
    this.safe.EventTransition(GameHashes.NavigationCellChanged, (GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State) this.unsafeCell, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell));
    this.unsafeCell.EventTransition(GameHashes.NavigationCellChanged, this.safe, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(SlipperyMonitor.IsStandingOnASlipperyCell))).DefaultState((GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State) this.unsafeCell.atRisk);
    this.unsafeCell.atRisk.EventTransition(GameHashes.EquipmentChanged, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).EventTransition(GameHashes.EffectAdded, this.unsafeCell.immune, new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)).DefaultState(this.unsafeCell.atRisk.idle);
    this.unsafeCell.atRisk.idle.EventHandlerTransition(GameHashes.NavigationCellChanged, this.unsafeCell.atRisk.slip, new Func<SlipperyMonitor.Instance, object, bool>(SlipperyMonitor.RollDTwenty));
    this.unsafeCell.atRisk.slip.ToggleReactable(new Func<SlipperyMonitor.Instance, Reactable>(this.GetReactable)).ScheduleGoTo(8f, (StateMachine.BaseState) this.unsafeCell.atRisk.idle);
    this.unsafeCell.immune.EventTransition(GameHashes.EquipmentChanged, (GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State) this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces))).EventTransition(GameHashes.EffectRemoved, (GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State) this.unsafeCell.atRisk, GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Not(new StateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.Transition.ConditionCallback(this.IsImmuneToSlipperySurfaces)));
  }

  public bool IsImmuneToSlipperySurfaces(SlipperyMonitor.Instance smi) => smi.IsImmune;

  public Reactable GetReactable(SlipperyMonitor.Instance smi) => (Reactable) smi.CreateReactable();

  private static bool IsStandingOnASlipperyCell(SlipperyMonitor.Instance smi)
  {
    if (!smi.IsWalking)
      return false;
    int cell1 = Grid.PosToCell((StateMachine.Instance) smi);
    int cell2 = Grid.OffsetCell(cell1, 0, -1);
    GameObject gameObject = Grid.Objects[cell2, 9];
    return (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null) || !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.PreventsSlipping)) && ((!Grid.IsValidCell(cell1) ? 0 : (Grid.Element[cell1].IsSlippery ? 1 : 0)) != 0 || (!Grid.IsValidCell(cell2) || !Grid.Element[cell2].IsSolid ? 0 : (Grid.Element[cell2].IsSlippery ? 1 : 0)) != 0);
  }

  private static bool RollDTwenty(SlipperyMonitor.Instance smi, object o)
  {
    return (double) UnityEngine.Random.value <= 0.05000000074505806;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class UnsafeCellState : 
    GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
  {
    public SlipperyMonitor.RiskStates atRisk;
    public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State immune;
  }

  public class RiskStates : 
    GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State
  {
    public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State idle;
    public GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.State slip;
  }

  public new class Instance : 
    GameStateMachine<SlipperyMonitor, SlipperyMonitor.Instance, IStateMachineTarget, SlipperyMonitor.Def>.GameInstance
  {
    private Effect effect;
    public Effects effects;
    private Navigator navigator;

    public bool IsImmune
    {
      get
      {
        return this.effects.HasEffect("RecentlySlippedTracker") || this.effects.HasImmunityTo(this.effect);
      }
    }

    public bool IsWalking
    {
      get
      {
        return (UnityEngine.Object) this.navigator != (UnityEngine.Object) null && this.navigator.CurrentNavType == NavType.Floor;
      }
    }

    public Instance(IStateMachineTarget master, SlipperyMonitor.Def def)
      : base(master, def)
    {
      this.effects = this.GetComponent<Effects>();
      this.effect = Db.Get().effects.Get("RecentlySlippedTracker");
      this.navigator = this.GetComponent<Navigator>();
    }

    public SlipperyMonitor.SlipReactable CreateReactable()
    {
      return new SlipperyMonitor.SlipReactable(this);
    }
  }

  public class SlipReactable : Reactable
  {
    private SlipperyMonitor.Instance smi;
    private float startTime;
    private const string ANIM_FILE_NAME = "anim_slip_kanim";
    private const float DURATION = 4.3f;

    public SlipReactable(SlipperyMonitor.Instance _smi)
      : base(_smi.gameObject, (HashedString) "Slip", Db.Get().ChoreTypes.Slip, 1, 1, lifeSpan: 8f)
    {
      this.smi = _smi;
    }

    public override bool InternalCanBegin(
      GameObject new_reactor,
      Navigator.ActiveTransition transition)
    {
      if ((UnityEngine.Object) this.reactor != (UnityEngine.Object) null || (UnityEngine.Object) new_reactor == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject != (UnityEngine.Object) new_reactor || this.smi == null)
        return false;
      Navigator component = new_reactor.GetComponent<Navigator>();
      return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.CurrentNavType == NavType.Floor;
    }

    protected override void InternalBegin()
    {
      this.startTime = Time.time;
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) DUPLICANTS.MODIFIERS.SLIPPED.NAME, this.gameObject.transform);
      KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
      component.AddAnimOverrides(Assets.GetAnim((HashedString) "anim_slip_kanim"), 1f);
      component.Play((HashedString) "slip_pre");
      component.Queue((HashedString) "slip_loop");
      component.Queue((HashedString) "slip_pst");
      this.reactor.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.Slippering);
    }

    public override void Update(float dt)
    {
      if ((double) Time.time - (double) this.startTime <= 4.3000001907348633)
        return;
      this.Cleanup();
      this.ApplyStress();
      this.ApplyTrackerEffect();
    }

    public void ApplyTrackerEffect() => this.smi.effects.Add("RecentlySlippedTracker", true);

    private void ApplyStress()
    {
      double num = (double) this.smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Stress.Id).ApplyDelta(3f);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, $"{3f.ToString()}% {Db.Get().Amounts.Stress.Name}", this.gameObject.transform);
      ReportManager.Instance.ReportValueWithGameObjectContext(ReportManager.ReportType.StressDelta, 3f, this.gameObject, (string) DUPLICANTS.MODIFIERS.SLIPPED.NAME);
    }

    protected override void InternalEnd()
    {
      if (!((UnityEngine.Object) this.reactor != (UnityEngine.Object) null))
        return;
      KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      this.reactor.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.Slippering);
      component.RemoveAnimOverrides(Assets.GetAnim((HashedString) "anim_slip_kanim"));
    }

    protected override void InternalCleanup()
    {
    }
  }
}
