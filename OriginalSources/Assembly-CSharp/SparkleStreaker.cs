// Decompiled with JetBrains decompiler
// Type: SparkleStreaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class SparkleStreaker : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance>
{
  private Vector3 offset = new Vector3(0.0f, 0.0f, 0.1f);
  public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State neutral;
  public SparkleStreaker.OverjoyedStates overjoyed;
  public string soundPath = GlobalAssets.GetSound("SparkleStreaker_lp");
  public HashedString SPARKLE_STREAKER_MOVING_PARAMETER = (HashedString) "sparkleStreaker_moving";

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.neutral;
    this.root.TagTransition(GameTags.Dead, (GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State) null);
    this.neutral.TagTransition(GameTags.Overjoyed, (GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State) this.overjoyed);
    this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsSparkleStreaker").ToggleLoopingSound(this.soundPath).Enter((StateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      smi.sparkleStreakFX = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, smi.master.transform.GetPosition() + this.offset);
      smi.sparkleStreakFX.transform.SetParent(smi.master.transform);
      smi.sparkleStreakFX.SetActive(true);
      smi.CreatePasserbyReactable();
    })).Exit((StateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      Util.KDestroyGameObject(smi.sparkleStreakFX);
      smi.ClearPasserbyReactable();
    }));
    this.overjoyed.idle.Enter((StateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.SetSparkleSoundParam(0.0f))).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.moving, (StateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.IsMoving()));
    this.overjoyed.moving.Enter((StateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.SetSparkleSoundParam(1f))).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.idle, (StateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => !smi.IsMoving()));
  }

  public class OverjoyedStates : 
    GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State moving;
  }

  public new class Instance(IStateMachineTarget master) : 
    GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.GameInstance(master)
  {
    private Reactable passerbyReactable;
    public GameObject sparkleStreakFX;

    public void CreatePasserbyReactable()
    {
      if (this.passerbyReactable != null)
        return;
      EmoteReactable emoteReactable = new EmoteReactable(this.gameObject, (HashedString) "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, localCooldown: 600f);
      Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
      emoteReactable.SetEmote(clapCheer).SetThought(Db.Get().Thoughts.Happy).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
      emoteReactable.RegisterEmoteStepCallbacks((HashedString) "clapcheer_pre", new System.Action<GameObject>(this.AddReactionEffect), (System.Action<GameObject>) null);
      this.passerbyReactable = (Reactable) emoteReactable;
    }

    private void AddReactionEffect(GameObject reactor)
    {
      reactor.GetComponent<Effects>().Add("SawSparkleStreaker", true);
    }

    private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
    {
      return transition.end == NavType.Floor;
    }

    public void ClearPasserbyReactable()
    {
      if (this.passerbyReactable == null)
        return;
      this.passerbyReactable.Cleanup();
      this.passerbyReactable = (Reactable) null;
    }

    public bool IsMoving() => this.smi.master.GetComponent<Navigator>().IsMoving();

    public void SetSparkleSoundParam(float val)
    {
      this.GetComponent<LoopingSounds>().SetParameter(GlobalAssets.GetSound("SparkleStreaker_lp"), (HashedString) "sparkleStreaker_moving", val);
    }
  }
}
