// Decompiled with JetBrains decompiler
// Type: CritterEmoteMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CritterEmoteMonitor : 
  GameStateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>
{
  public GameStateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.State cooldown;
  public GameStateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.State express;

  public static bool ShouldEmote(CritterEmoteMonitor.Instance smi) => true;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.cooldown;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    List<CritterEmotion> cooldownsToRemove = new List<CritterEmotion>();
    this.cooldown.ScheduleGoTo((Func<CritterEmoteMonitor.Instance, float>) (smi => UnityEngine.Random.Range(37.5f, 75f)), (StateMachine.BaseState) this.express).Enter((StateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.State.Callback) (smi => NameDisplayScreen.Instance.SetThoughtBubbleDisplay(smi.gameObject, false, (string) null, (Sprite) null, (Sprite) null))).Update((System.Action<CritterEmoteMonitor.Instance, float>) ((smi, dt) =>
    {
      foreach (KeyValuePair<CritterEmotion, float> cooldown in smi.cooldowns)
      {
        if ((double) Time.timeSinceLevelLoad > (double) smi.cooldowns[cooldown.Key] + 30.0)
          cooldownsToRemove.Add(cooldown.Key);
      }
      foreach (CritterEmotion key in cooldownsToRemove)
        smi.cooldowns.Remove(key);
      cooldownsToRemove.Clear();
    }));
    this.express.ToggleBehaviour(GameTags.Creatures.Behaviours.CritterEmoteBehaviour, new StateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.Transition.ConditionCallback(CritterEmoteMonitor.ShouldEmote), (System.Action<CritterEmoteMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.cooldown)));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<CritterEmoteMonitor, CritterEmoteMonitor.Instance, IStateMachineTarget, CritterEmoteMonitor.Def>.GameInstance,
    IDevQuickAction
  {
    public Emote emotePositive;
    public Emote emoteNegative;
    public List<CritterEmotion> currentNegativeEmotions = new List<CritterEmotion>();
    public List<CritterEmotion> currentPositiveEmotions = new List<CritterEmotion>();
    public const float SPECIFIC_EMOTE_COOLDOWN = 30f;
    public Dictionary<CritterEmotion, float> cooldowns = new Dictionary<CritterEmotion, float>();

    public Instance(IStateMachineTarget master, CritterEmoteMonitor.Def def)
      : base(master, def)
    {
      this.emotePositive = Db.Get().Emotes.Critter.Positive;
      this.emoteNegative = Db.Get().Emotes.Critter.Negative;
    }

    public CritterEmotion GetCritterEmotion()
    {
      if (this.currentNegativeEmotions.Count > 0)
      {
        float num = float.PositiveInfinity;
        CritterEmotion critterEmotion = (CritterEmotion) null;
        foreach (CritterEmotion currentNegativeEmotion in this.currentNegativeEmotions)
        {
          if (!this.cooldowns.ContainsKey(currentNegativeEmotion))
            return currentNegativeEmotion;
          float cooldown = this.cooldowns[currentNegativeEmotion];
          if ((double) cooldown < (double) num)
          {
            num = cooldown;
            critterEmotion = currentNegativeEmotion;
          }
        }
        return critterEmotion;
      }
      if (this.currentPositiveEmotions.Count <= 0)
        return (CritterEmotion) null;
      float num1 = 0.0f;
      CritterEmotion critterEmotion1 = (CritterEmotion) null;
      foreach (CritterEmotion currentPositiveEmotion in this.currentPositiveEmotions)
      {
        if (!this.cooldowns.ContainsKey(currentPositiveEmotion))
          return currentPositiveEmotion;
        float cooldown = this.cooldowns[currentPositiveEmotion];
        if ((double) cooldown < (double) num1)
        {
          num1 = cooldown;
          critterEmotion1 = currentPositiveEmotion;
        }
      }
      return critterEmotion1;
    }

    public void AddCritterEmotion(CritterEmotion emotion)
    {
      if (this.smi.GetSMI<BabyMonitor.Instance>() != null)
        return;
      if (!emotion.isPositiveEmotion)
      {
        if (this.currentNegativeEmotions.Contains(emotion))
          return;
        this.currentNegativeEmotions.Add(emotion);
      }
      else
      {
        if (this.currentPositiveEmotions.Contains(emotion))
          return;
        this.currentPositiveEmotions.Add(emotion);
      }
      if (!this.smi.IsInsideState((StateMachine.BaseState) this.sm.cooldown) || this.cooldowns.ContainsKey(emotion))
        return;
      this.smi.GoTo((StateMachine.BaseState) this.sm.express);
    }

    public void RemoveCritterEmotion(CritterEmotion emotion)
    {
      this.currentNegativeEmotions.RemoveAll((Predicate<CritterEmotion>) (e => e.id == emotion.id));
      this.currentPositiveEmotions.RemoveAll((Predicate<CritterEmotion>) (e => e.id == emotion.id));
    }

    public List<DevQuickActionInstruction> GetDevInstructions()
    {
      return new List<DevQuickActionInstruction>()
      {
        new DevQuickActionInstruction("Emote/Play", (System.Action) (() => this.smi.GoTo((StateMachine.BaseState) this.smi.sm.express)))
      };
    }
  }
}
