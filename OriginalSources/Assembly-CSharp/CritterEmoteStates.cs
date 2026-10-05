// Decompiled with JetBrains decompiler
// Type: CritterEmoteStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using UnityEngine;

#nullable disable
public class CritterEmoteStates : 
  GameStateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>
{
  public GameStateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.State playing;
  public GameStateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Enter((StateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.State.Callback) (smi =>
    {
      smi.emotion = smi.GetSMI<CritterEmoteMonitor.Instance>().GetCritterEmotion();
      if (smi.emotion != null)
        smi.GoTo((StateMachine.BaseState) this.playing);
      else
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    this.playing.ToggleAnims((Func<CritterEmoteStates.Instance, KAnimFile>) (smi => smi.emoteBuildFile)).PlayAnims((Func<CritterEmoteStates.Instance, HashedString[]>) (smi => !smi.emotion.isPositiveEmotion ? new HashedString[1]
    {
      (HashedString) "react_neg"
    } : new HashedString[1]{ (HashedString) "react_pos" })).ScheduleGoTo(10f, (StateMachine.BaseState) this.behaviourcomplete).OnAnimQueueComplete(this.behaviourcomplete).Enter((StateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.State.Callback) (smi =>
    {
      CritterEmoteMonitor.Instance smi1 = smi.GetSMI<CritterEmoteMonitor.Instance>();
      smi.emotion = smi1.GetCritterEmotion();
      if (!smi1.cooldowns.ContainsKey(smi.emotion))
        smi1.cooldowns.Add(smi.emotion, Time.timeSinceLevelLoad);
      else
        smi1.cooldowns[smi.emotion] = Time.timeSinceLevelLoad;
      if (!((UnityEngine.Object) smi.emotion.sprite != (UnityEngine.Object) null))
        return;
      NameDisplayScreen.Instance.SetThoughtBubbleDisplay(smi.gameObject, true, "", Assets.GetSprite((HashedString) "bubble_alert"), smi.emotion.sprite);
      smi.hasSetThoughtBubble = true;
    })).Exit((StateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.State.Callback) (smi =>
    {
      if (!smi.hasSetThoughtBubble)
        return;
      NameDisplayScreen.Instance.SetThoughtBubbleDisplay(smi.gameObject, false, (string) null, (Sprite) null, (Sprite) null);
      smi.hasSetThoughtBubble = false;
    }));
    this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Behaviours.CritterEmoteBehaviour);
  }

  public class Def : StateMachine.BaseDef
  {
    public KAnimFile emoteBuildFile;

    public Def(KAnimFile emoteBuildFile) => this.emoteBuildFile = emoteBuildFile;
  }

  public new class Instance : 
    GameStateMachine<CritterEmoteStates, CritterEmoteStates.Instance, IStateMachineTarget, CritterEmoteStates.Def>.GameInstance
  {
    public KAnimFile emoteBuildFile;
    public CritterEmotion emotion;
    public bool hasSetThoughtBubble;

    public Instance(Chore<CritterEmoteStates.Instance> chore, CritterEmoteStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Behaviours.CritterEmoteBehaviour);
      this.emoteBuildFile = def.emoteBuildFile;
    }
  }
}
