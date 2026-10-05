// Decompiled with JetBrains decompiler
// Type: BlinkMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BlinkMonitor : 
  GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>
{
  public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State satisfied;
  public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State blinking;
  public StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.TargetParameter eyes;
  private static HashedString HASH_SNAPTO_EYES = (HashedString) "snapto_eyes";

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.CreateEyes)).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.DestroyEyes));
    this.satisfied.ScheduleGoTo(new Func<BlinkMonitor.Instance, float>(BlinkMonitor.GetRandomBlinkTime), (StateMachine.BaseState) this.blinking);
    this.blinking.EnterTransition(this.satisfied, GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Not(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Transition.ConditionCallback(BlinkMonitor.CanBlink))).Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.BeginBlinking)).Update(new System.Action<BlinkMonitor.Instance, float>(BlinkMonitor.UpdateBlinking), UpdateRate.RENDER_EVERY_TICK).Target(this.eyes).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.EndBlinking));
  }

  private static bool CanBlink(BlinkMonitor.Instance smi)
  {
    return !smi.eye_anim.IsNullOrWhiteSpace() && SpeechMonitor.IsAllowedToPlaySpeech(smi.Kpid, smi.AnimController) && smi.Navigator.CurrentNavType != NavType.Ladder;
  }

  private static float GetRandomBlinkTime(BlinkMonitor.Instance smi)
  {
    return UnityEngine.Random.Range(TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMin, TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMax);
  }

  private static void CreateEyes(BlinkMonitor.Instance smi)
  {
    smi.eyes = Util.KInstantiate(Assets.GetPrefab((Tag) EyeAnimation.ID)).GetComponent<KBatchedAnimController>();
    smi.eyes.gameObject.SetActive(true);
    smi.sm.eyes.Set(smi.eyes.gameObject, smi, false);
  }

  private static void DestroyEyes(BlinkMonitor.Instance smi)
  {
    if (!((UnityEngine.Object) smi.eyes != (UnityEngine.Object) null))
      return;
    Util.KDestroyGameObject((Component) smi.eyes);
    smi.eyes = (KBatchedAnimController) null;
  }

  public static void BeginBlinking(BlinkMonitor.Instance smi)
  {
    smi.eyes.Play((HashedString) smi.eye_anim);
    BlinkMonitor.UpdateBlinking(smi, 0.0f);
  }

  public static void EndBlinking(BlinkMonitor.Instance smi)
  {
    smi.SymbolOverrideController.RemoveSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, 3);
  }

  public static void UpdateBlinking(BlinkMonitor.Instance smi, float dt)
  {
    int currentFrameIndex = smi.eyes.GetCurrentFrameIndex();
    KAnimBatch batch = smi.eyes.GetBatch();
    KAnim.Anim.Frame frame;
    if (currentFrameIndex == -1 || batch == null || !smi.eyes.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
      return;
    HashedString symbol_name = HashedString.Invalid;
    for (int index1 = 0; index1 < frame.numElements; ++index1)
    {
      int index2 = frame.firstElementIdx + index1;
      if (index2 < batch.group.data.frameElements.Count)
      {
        KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[index2];
        if (!(frameElement.symbol == HashedString.Invalid))
        {
          symbol_name = (HashedString) frameElement.symbol;
          break;
        }
      }
    }
    smi.SymbolOverrideController.AddSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, smi.eyes.AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) symbol_name), 3);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Tuning : TuningData<BlinkMonitor.Tuning>
  {
    public float randomBlinkIntervalMin;
    public float randomBlinkIntervalMax;
  }

  public new class Instance : 
    GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.GameInstance
  {
    public KBatchedAnimController eyes;
    public string eye_anim;

    public KPrefabID Kpid { get; private set; }

    public KBatchedAnimController AnimController { get; private set; }

    public Navigator Navigator { get; private set; }

    public SymbolOverrideController SymbolOverrideController { get; private set; }

    public Instance(IStateMachineTarget master, BlinkMonitor.Def def)
      : base(master, def)
    {
      this.Kpid = master.GetComponent<KPrefabID>();
      this.AnimController = master.GetComponent<KBatchedAnimController>();
      this.Navigator = master.GetComponent<Navigator>();
      this.SymbolOverrideController = master.GetComponent<SymbolOverrideController>();
    }

    public bool IsBlinking() => this.IsInsideState((StateMachine.BaseState) this.sm.blinking);

    public void Blink() => this.GoTo((StateMachine.BaseState) this.sm.blinking);
  }
}
