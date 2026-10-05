// Decompiled with JetBrains decompiler
// Type: BalloonArtist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using System;
using System.Runtime.Serialization;
using TUNING;

#nullable disable
public class BalloonArtist : GameStateMachine<BalloonArtist, BalloonArtist.Instance>
{
  public StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.IntParameter balloonsGivenOut;
  public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State neutral;
  public BalloonArtist.OverjoyedStates overjoyed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.neutral;
    this.root.TagTransition(GameTags.Dead, (GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State) null);
    this.neutral.TagTransition(GameTags.Overjoyed, (GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State) this.overjoyed);
    this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>((StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.Parameter<int>) this.balloonsGivenOut, this.overjoyed.exitEarly, (StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.Parameter<int>.Callback) ((smi, p) => p >= TRAITS.JOY_REACTIONS.BALLOON_ARTIST.NUM_BALLOONS_TO_GIVE)).Exit((StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      smi.numBalloonsGiven = 0;
      this.balloonsGivenOut.Set(0, smi);
    }));
    this.overjoyed.idle.Enter((StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      if (!smi.IsRecTime())
        return;
      smi.GoTo((StateMachine.BaseState) this.overjoyed.balloon_stand);
    })).ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistPlanning).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.balloon_stand, (StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsRecTime()));
    this.overjoyed.balloon_stand.ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistHandingOut).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.idle, (StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsRecTime())).ToggleChore((Func<BalloonArtist.Instance, Chore>) (smi => (Chore) new BalloonArtistChore(smi.master)), this.overjoyed.idle);
    this.overjoyed.exitEarly.Enter((StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.ExitJoyReactionEarly()));
  }

  public class OverjoyedStates : 
    GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State balloon_stand;
    public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State exitEarly;
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    [Serialize]
    public int numBalloonsGiven;
    [NonSerialized]
    private BalloonOverrideSymbolIter balloonSymbolIter;
    private const string TARGET_SYMBOL_TO_OVERRIDE = "body";
    private const int TARGET_OVERRIDE_PRIORITY = 0;

    [OnDeserialized]
    private void OnDeserialized()
    {
      this.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, this.smi);
    }

    public void Internal_InitBalloons()
    {
      JoyResponseOutfitTarget responseOutfitTarget = JoyResponseOutfitTarget.FromMinion(this.master.gameObject);
      if (!this.balloonSymbolIter.IsNullOrDestroyed() && this.balloonSymbolIter.facade.AndThen<string>((Func<BalloonArtistFacadeResource, string>) (f => f.Id)) == responseOutfitTarget.ReadFacadeId())
        return;
      this.balloonSymbolIter = responseOutfitTarget.ReadFacadeId().AndThen<BalloonArtistFacadeResource>((Func<string, BalloonArtistFacadeResource>) (id => Db.Get().Permits.BalloonArtistFacades.Get(id))).AndThen<BalloonOverrideSymbolIter>((Func<BalloonArtistFacadeResource, BalloonOverrideSymbolIter>) (permit => permit.GetSymbolIter())).UnwrapOr(new BalloonOverrideSymbolIter((Option<BalloonArtistFacadeResource>) Option.None));
      this.SetBalloonSymbolOverride(this.balloonSymbolIter.Current());
    }

    public bool IsRecTime()
    {
      return this.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
    }

    public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverrideSymbol)
    {
      if (balloonOverrideSymbol.animFile.IsNone())
        this.master.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) "body", Assets.GetAnim((HashedString) "balloon_anim_kanim").GetData().build.GetSymbol((KAnimHashedString) "body"));
      else
        this.master.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) "body", balloonOverrideSymbol.symbol.Unwrap());
    }

    public BalloonOverrideSymbol GetCurrentBalloonSymbolOverride()
    {
      return this.balloonSymbolIter.Current();
    }

    public void ApplyNextBalloonSymbolOverride()
    {
      this.SetBalloonSymbolOverride(this.balloonSymbolIter.Next());
    }

    public void GiveBalloon()
    {
      ++this.numBalloonsGiven;
      this.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, this.smi);
    }

    public void ExitJoyReactionEarly()
    {
      JoyBehaviourMonitor.Instance smi = this.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
      smi.sm.exitEarly.Trigger(smi);
    }
  }
}
