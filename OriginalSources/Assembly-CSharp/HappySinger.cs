// Decompiled with JetBrains decompiler
// Type: HappySinger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class HappySinger : GameStateMachine<HappySinger, HappySinger.Instance>
{
  private Vector3 offset = new Vector3(0.0f, 0.0f, 0.1f);
  public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State neutral;
  public HappySinger.OverjoyedStates overjoyed;
  public string soundPath = GlobalAssets.GetSound("DupeSinging_NotesFX_LP");

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.neutral;
    this.root.TagTransition(GameTags.Dead, (GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State) null);
    this.neutral.TagTransition(GameTags.Overjoyed, (GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State) this.overjoyed);
    this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsJoySinger").ToggleLoopingSound(this.soundPath).ToggleAnims("anim_loco_singer_kanim").ToggleAnims("anim_idle_singer_kanim").EventHandler(GameHashes.TagsChanged, (GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, obj) =>
    {
      if (!((UnityEngine.Object) smi.musicParticleFX != (UnityEngine.Object) null))
        return;
      smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
    })).Enter((StateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      smi.musicParticleFX = Util.KInstantiate(EffectPrefabs.Instance.HappySingerFX, smi.master.transform.GetPosition() + this.offset);
      smi.musicParticleFX.transform.SetParent(smi.master.transform);
      smi.CreatePasserbyReactable();
      smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
    })).Update((System.Action<HappySinger.Instance, float>) ((smi, dt) =>
    {
      if (smi.SpeechMonitorInstance.IsPlayingSpeech() || !SpeechMonitor.IsAllowedToPlaySpeech(smi.Kpid, smi.AnimController))
        return;
      Db.Get().Thoughts.CatchyTune.PlayAsSpeech(smi.SpeechMonitorInstance);
    }), UpdateRate.SIM_1000ms).Exit((StateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      smi.musicParticleFX.SetActive(false);
      Util.KDestroyGameObject(smi.musicParticleFX);
      smi.ClearPasserbyReactable();
    }));
  }

  public class OverjoyedStates : 
    GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State idle;
    public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State moving;
  }

  public new class Instance : 
    GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.GameInstance
  {
    private Reactable passerbyReactable;
    public GameObject musicParticleFX;
    private SpeechMonitor.Instance speechMonitorInstance;

    public KPrefabID Kpid { get; private set; }

    public KBatchedAnimController AnimController { get; private set; }

    public SpeechMonitor.Instance SpeechMonitorInstance
    {
      get
      {
        if (this.speechMonitorInstance == null)
          this.speechMonitorInstance = this.master.gameObject.GetSMI<SpeechMonitor.Instance>();
        return this.speechMonitorInstance;
      }
    }

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.Kpid = master.GetComponent<KPrefabID>();
      this.AnimController = master.GetComponent<KBatchedAnimController>();
    }

    public void CreatePasserbyReactable()
    {
      if (this.passerbyReactable != null)
        return;
      EmoteReactable emoteReactable = new EmoteReactable(this.gameObject, (HashedString) "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, localCooldown: 600f);
      Emote sing = Db.Get().Emotes.Minion.Sing;
      emoteReactable.SetEmote(sing).SetThought(Db.Get().Thoughts.CatchyTune).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
      emoteReactable.RegisterEmoteStepCallbacks((HashedString) "react", new System.Action<GameObject>(this.AddReactionEffect), (System.Action<GameObject>) null);
      this.passerbyReactable = (Reactable) emoteReactable;
    }

    private void AddReactionEffect(GameObject reactor) => reactor.Trigger(-1278274506);

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
  }
}
