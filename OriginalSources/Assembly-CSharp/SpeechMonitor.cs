// Decompiled with JetBrains decompiler
// Type: SpeechMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpeechMonitor : 
  GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>
{
  public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State satisfied;
  public SpeechMonitor.Playing talking;
  public static string PREFIX_SAD = "sad";
  public static string PREFIX_HAPPY = "happy";
  public static string PREFIX_SINGER = "sing";
  public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.TargetParameter mouth;
  private static HashedString HASH_SNAPTO_MOUTH = (HashedString) "snapto_mouth";
  private static HashedString GENERIC_CONVO_ANIM_NAME = new HashedString("anim_generic_convo_kanim");
  private static KAnim.Anim.FrameElement INVALID_FRAME_ELEMENT = new KAnim.Anim.FrameElement()
  {
    symbol = (KAnimHashedString) HashedString.Invalid
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.CreateMouth)).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.DestroyMouth));
    this.satisfied.DoNothing();
    this.talking.Enter((StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback) (smi =>
    {
      SpeechMonitor.StartAudio(smi);
      smi.mouth.Play((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi));
      if (smi.Kpid.HasTag(GameTags.DoNotInterruptMe))
        smi.GoTo((StateMachine.BaseState) this.talking.animGoverned);
      else if (smi.ev.isValid())
        smi.GoTo((StateMachine.BaseState) this.talking.audioGoverned);
      else
        smi.GoTo((StateMachine.BaseState) this.talking.fallback);
    })).Exit((StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback) (smi => smi.SymbolOverrideController.RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3)));
    this.talking.audioGoverned.Transition(this.satisfied, new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.Transition.ConditionCallback(SpeechMonitor.IsAudioStopped)).Update(new System.Action<SpeechMonitor.Instance, float>(SpeechMonitor.LipFlap), UpdateRate.RENDER_EVERY_TICK);
    this.talking.animGoverned.TagTransition(GameTags.DoNotInterruptMe, this.satisfied, true).Update(new System.Action<SpeechMonitor.Instance, float>(SpeechMonitor.LipFlap), UpdateRate.RENDER_EVERY_TICK).Update((System.Action<SpeechMonitor.Instance, float>) ((smi, dt) =>
    {
      if (!SpeechMonitor.IsAudioStopped(smi))
        return;
      SpeechMonitor.StartAudio(smi);
    }), UpdateRate.RENDER_EVERY_TICK);
    this.talking.fallback.Enter((StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback) (smi => smi.mouth.Queue((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi)))).Target(this.mouth).OnAnimQueueComplete(this.satisfied);
  }

  private static void CreateMouth(SpeechMonitor.Instance smi)
  {
    smi.mouth = Util.KInstantiate(Assets.GetPrefab((Tag) MouthAnimation.ID)).GetComponent<KBatchedAnimController>();
    smi.mouth.gameObject.SetActive(true);
    smi.sm.mouth.Set(smi.mouth.gameObject, smi, false);
    smi.SetMouthId();
  }

  private static void DestroyMouth(SpeechMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) smi.mouth != (UnityEngine.Object) null))
      return;
    Util.KDestroyGameObject((Component) smi.mouth);
    smi.mouth = (KBatchedAnimController) null;
  }

  private static string GetRandomSpeechAnim(SpeechMonitor.Instance smi)
  {
    return smi.speechPrefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString() + smi.mouthId;
  }

  public static bool IsAllowedToPlaySpeech(KPrefabID prefabID, KBatchedAnimController controller)
  {
    if (prefabID.HasTag(GameTags.Dead) || prefabID.HasTag(GameTags.Incapacitated))
      return false;
    KAnim.Anim currentAnim = controller.GetCurrentAnim();
    if (currentAnim == null)
      return true;
    return GameAudioSheets.Get().IsAnimAllowedToPlaySpeech(currentAnim) && SpeechMonitor.CanOverrideHead(controller);
  }

  private static bool CanOverrideHead(KBatchedAnimController kbac)
  {
    bool flag = true;
    KAnim.Anim currentAnim = kbac.GetCurrentAnim();
    if (currentAnim == null)
      flag = false;
    else if ((HashedString) currentAnim.animFile.name != SpeechMonitor.GENERIC_CONVO_ANIM_NAME)
    {
      int currentFrameIndex = kbac.GetCurrentFrameIndex();
      if (currentFrameIndex <= 0)
      {
        flag = false;
      }
      else
      {
        KAnim.Anim.Frame frame;
        if (KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag).TryGetFrame(currentFrameIndex, out frame) && frame.hasHead)
          flag = false;
      }
    }
    return flag;
  }

  private static KAnim.Anim.FrameElement GetFirstFrameElement(KBatchedAnimController controller)
  {
    int currentFrameIndex = controller.GetCurrentFrameIndex();
    if (currentFrameIndex == -1)
      return SpeechMonitor.INVALID_FRAME_ELEMENT;
    KAnimBatch batch = controller.GetBatch();
    KAnim.Anim.Frame frame;
    if (batch == null || !batch.group.data.TryGetFrame(currentFrameIndex, out frame))
      return SpeechMonitor.INVALID_FRAME_ELEMENT;
    List<KAnim.Anim.FrameElement> frameElements = batch.group.data.frameElements;
    for (int index1 = 0; index1 < frame.numElements; ++index1)
    {
      int index2 = frame.firstElementIdx + index1;
      int num = index2 < frameElements.Count ? 1 : 0;
      DebugUtil.DevAssert(num != 0, "Frame element index out of range");
      if (num != 0)
      {
        KAnim.Anim.FrameElement firstFrameElement = frameElements[index2];
        if (!(firstFrameElement.symbol == HashedString.Invalid))
          return firstFrameElement;
      }
    }
    return SpeechMonitor.INVALID_FRAME_ELEMENT;
  }

  private static void StartAudio(SpeechMonitor.Instance smi)
  {
    smi.ev.clearHandle();
    if (smi.voiceEvent == null)
      return;
    smi.ev = VoiceSoundEvent.PlayVoice(smi.voiceEvent, smi.AnimController, 0.0f, false);
  }

  private static bool IsAudioStopped(SpeechMonitor.Instance smi)
  {
    if (!smi.ev.isValid())
      return true;
    PLAYBACK_STATE state;
    int playbackState = (int) smi.ev.getPlaybackState(out state);
    if (state != PLAYBACK_STATE.STOPPING && state != PLAYBACK_STATE.STOPPED)
      return false;
    smi.ev.clearHandle();
    return true;
  }

  private static void LipFlap(SpeechMonitor.Instance smi, float dt)
  {
    if (!smi.mouth.IsStopped())
      return;
    smi.mouth.Play((HashedString) SpeechMonitor.GetRandomSpeechAnim(smi));
    DebugUtil.DevAssert(!smi.mouth.IsStopped(), "Mouth animation should be playing");
  }

  public class Playing : 
    GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State
  {
    public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State audioGoverned;
    public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State animGoverned;
    public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State fallback;
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Tuning : TuningData<SpeechMonitor.Tuning>
  {
    public float randomSpeechIntervalMin;
    public float randomSpeechIntervalMax;
    public int speechCount;
  }

  public new class Instance : 
    GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.GameInstance
  {
    public KBatchedAnimController mouth;
    public string speechPrefix = "happy";
    public string voiceEvent;
    public EventInstance ev;
    public string mouthId;

    public KBatchedAnimController AnimController { get; private set; }

    public SymbolOverrideController SymbolOverrideController { get; private set; }

    public MinionIdentity MinionIdentity { get; private set; }

    public KPrefabID Kpid { get; private set; }

    public Instance(IStateMachineTarget master, SpeechMonitor.Def def)
      : base(master, def)
    {
      this.AnimController = master.GetComponent<KBatchedAnimController>();
      this.SymbolOverrideController = master.GetComponent<SymbolOverrideController>();
      this.MinionIdentity = master.GetComponent<MinionIdentity>();
      this.Kpid = master.GetComponent<KPrefabID>();
    }

    public bool IsPlayingSpeech() => this.IsInsideState((StateMachine.BaseState) this.sm.talking);

    public void PlaySpeech(string speech_prefix, string voice_event)
    {
      this.speechPrefix = speech_prefix;
      this.voiceEvent = voice_event;
      this.GoTo((StateMachine.BaseState) this.sm.talking);
    }

    public void DrawMouth()
    {
      KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(this.smi.mouth);
      int num = firstFrameElement.symbol != HashedString.Invalid ? 1 : 0;
      DebugUtil.DevAssert(num != 0, "Mouth frame element invalid");
      if (num == 0)
        return;
      KAnim.Build build = this.smi.mouth.AnimFiles[0].GetData().build;
      KAnim.Build.Symbol symbol1 = build.GetSymbol(firstFrameElement.symbol);
      this.SymbolOverrideController.AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, symbol1, 3);
      KAnim.Build.Symbol symbol2 = KAnimBatchManager.Instance().GetBatchGroupData(this.AnimController.batchGroupID).GetSymbol((KAnimHashedString) SpeechMonitor.HASH_SNAPTO_MOUTH);
      DebugUtil.DevAssert(build == symbol1.build, "Mouth build mismatch");
      KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(build.batchTag).symbolFrameInstances[symbol1.firstFrameIdx + firstFrameElement.frame] with
      {
        buildImageIdx = this.SymbolOverrideController.GetAtlasIdx(build.GetTexture(0))
      };
      this.AnimController.SetSymbolOverride(symbol2.firstFrameIdx, ref symbolFrameInstance);
    }

    public void SetMouthId()
    {
      Personality personality = Db.Get().Personalities.Get(this.MinionIdentity.personalityResourceId);
      if (personality.speech_mouth <= 0)
        return;
      this.smi.mouthId = $"_{personality.speech_mouth:000}";
    }
  }
}
