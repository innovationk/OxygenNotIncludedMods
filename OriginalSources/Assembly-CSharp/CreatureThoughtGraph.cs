// Decompiled with JetBrains decompiler
// Type: CreatureThoughtGraph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CreatureThoughtGraph : 
  GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>
{
  public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChanged;
  public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Signal thoughtsChangedImmediate;
  public StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.FloatParameter thoughtDisplayTime;
  public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State initialdelay;
  public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State nothoughts;
  public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State displayingthought;
  public GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State cooldown;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.initialdelay;
    this.initialdelay.ScheduleGoTo(1f, (StateMachine.BaseState) this.nothoughts);
    this.nothoughts.OnSignal(this.thoughtsChanged, this.displayingthought, (StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Parameter<StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.SignalParameter>.Callback) ((smi, param) => smi.HasThoughts())).OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Parameter<StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.SignalParameter>.Callback) ((smi, param) => smi.HasThoughts()));
    this.displayingthought.Enter("CreateBubble", (StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State.Callback) (smi => smi.CreateBubble())).Exit("DestroyBubble", (StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.State.Callback) (smi => smi.DestroyBubble())).ScheduleGoTo((Func<CreatureThoughtGraph.Instance, float>) (smi => this.thoughtDisplayTime.Get(smi)), (StateMachine.BaseState) this.cooldown);
    this.cooldown.OnSignal(this.thoughtsChangedImmediate, this.displayingthought, (StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.Parameter<StateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.SignalParameter>.Callback) ((smi, param) => smi.HasImmediateThought())).ScheduleGoTo(20f, (StateMachine.BaseState) this.nothoughts);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<CreatureThoughtGraph, CreatureThoughtGraph.Instance, IStateMachineTarget, CreatureThoughtGraph.Def>.GameInstance
  {
    private List<Thought> thoughts = new List<Thought>();
    public Thought currentThought;

    public Instance(IStateMachineTarget master, CreatureThoughtGraph.Def def)
      : base(master, def)
    {
      NameDisplayScreen.Instance.RegisterComponent(this.gameObject, (object) this);
    }

    protected override void OnCleanUp() => base.OnCleanUp();

    public bool HasThoughts() => this.thoughts.Count > 0;

    public bool HasImmediateThought()
    {
      bool flag = false;
      for (int index = 0; index < this.thoughts.Count; ++index)
      {
        if (this.thoughts[index].showImmediately)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public void AddThought(Thought thought)
    {
      if (this.thoughts.Contains(thought))
        return;
      this.thoughts.Add(thought);
      if (thought.showImmediately)
        this.sm.thoughtsChangedImmediate.Trigger(this.smi);
      else
        this.sm.thoughtsChanged.Trigger(this.smi);
    }

    public void RemoveThought(Thought thought)
    {
      if (!this.thoughts.Contains(thought))
        return;
      this.thoughts.Remove(thought);
      this.sm.thoughtsChanged.Trigger(this.smi);
    }

    private int SortThoughts(Thought a, Thought b)
    {
      if (a.showImmediately == b.showImmediately)
        return b.priority.CompareTo(a.priority);
      return !a.showImmediately ? 1 : -1;
    }

    public void CreateBubble()
    {
      if (this.thoughts.Count == 0)
        return;
      this.thoughts.Sort(new Comparison<Thought>(this.SortThoughts));
      Thought thought = this.thoughts[0];
      if ((UnityEngine.Object) thought.modeSprite != (UnityEngine.Object) null)
        NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(this.gameObject, true, (string) thought.hoverText, thought.bubbleSprite, thought.sprite, thought.modeSprite);
      else
        NameDisplayScreen.Instance.SetThoughtBubbleDisplay(this.gameObject, true, (string) thought.hoverText, thought.bubbleSprite, thought.sprite);
      double num = (double) this.sm.thoughtDisplayTime.Set(thought.showTime, this);
      this.currentThought = thought;
      if (!thought.showImmediately)
        return;
      this.thoughts.RemoveAt(0);
    }

    public void DestroyBubble()
    {
      NameDisplayScreen.Instance.SetThoughtBubbleDisplay(this.gameObject, false, (string) null, (Sprite) null, (Sprite) null);
      NameDisplayScreen.Instance.SetThoughtBubbleConvoDisplay(this.gameObject, false, (string) null, (Sprite) null, (Sprite) null, (Sprite) null);
    }
  }
}
