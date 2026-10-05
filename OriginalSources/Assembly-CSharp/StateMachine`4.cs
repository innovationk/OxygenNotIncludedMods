// Decompiled with JetBrains decompiler
// Type: StateMachine`4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : 
  StateMachine
  where StateMachineInstanceType : StateMachine.Instance
  where MasterType : IStateMachineTarget
{
  private List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State> states = new List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State>();
  public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter masterTarget;
  [StateMachine.DoNotAutoCreate]
  protected StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

  public override string[] GetStateNames()
  {
    List<string> stringList = new List<string>();
    foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
      stringList.Add(state.name);
    return stringList.ToArray();
  }

  public void Target(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
  {
    this.stateTarget = target;
  }

  public void BindState(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state,
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state,
    string state_name)
  {
    if (parent_state != null)
      state_name = $"{parent_state.name}.{state_name}";
    state.name = state_name;
    state.longName = $"{this.name}.{state_name}";
    List<StateMachine.BaseState> baseStateList = parent_state == null ? new List<StateMachine.BaseState>() : new List<StateMachine.BaseState>((IEnumerable<StateMachine.BaseState>) parent_state.branch);
    baseStateList.Add((StateMachine.BaseState) state);
    state.parent = (StateMachine.BaseState) parent_state;
    state.branch = baseStateList.ToArray();
    this.maxDepth = Math.Max(state.branch.Length, this.maxDepth);
    this.states.Add(state);
  }

  public void BindStates(
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state,
    object state_machine)
  {
    foreach (FieldInfo field in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
    {
      if (field.FieldType.IsSubclassOf(typeof (StateMachine.BaseState)))
      {
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) field.GetValue(state_machine);
        if (state != parent_state)
        {
          string name = field.Name;
          this.BindState(parent_state, state, name);
          this.BindStates(state, (object) state);
        }
      }
    }
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    base.InitializeStates(out default_state);
  }

  public override void BindStates()
  {
    this.BindStates((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) null, (object) this);
  }

  public override System.Type GetStateMachineInstanceType() => typeof (StateMachineInstanceType);

  public override StateMachine.BaseState GetState(string state_name)
  {
    foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
    {
      if (state.name == state_name)
        return (StateMachine.BaseState) state;
    }
    return (StateMachine.BaseState) null;
  }

  public override void FreeResources()
  {
    for (int index = 0; index < this.states.Count; ++index)
      this.states[index].FreeResources();
    this.states.Clear();
    base.FreeResources();
  }

  public class GenericInstance : StateMachine.Instance
  {
    private float stateEnterTime;
    private int gotoId;
    private int currentActionIdx = -1;
    private SchedulerHandle updateHandle;
    private Stack<StateMachine.BaseState> gotoStack = new Stack<StateMachine.BaseState>();
    protected Stack<StateMachine.BaseTransition.Context> transitionStack = new Stack<StateMachine.BaseTransition.Context>();
    protected StateMachineController controller;
    private SchedulerGroup currentSchedulerGroup;
    private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] stateStack;

    public StateMachineType sm { get; private set; }

    protected StateMachineInstanceType smi => (StateMachineInstanceType) this;

    public MasterType master { get; private set; }

    public DefType def { get; set; }

    public bool isMasterNull
    {
      get => this.internalSm.masterTarget.IsNull((StateMachineInstanceType) this);
    }

    private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> internalSm
    {
      get
      {
        return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>) (object) this.sm;
      }
    }

    protected virtual void OnCleanUp()
    {
    }

    public override float timeinstate => Time.time - this.stateEnterTime;

    public override void FreeResources()
    {
      this.updateHandle.FreeResources();
      this.updateHandle = new SchedulerHandle();
      this.controller = (StateMachineController) null;
      if (this.gotoStack != null)
        this.gotoStack.Clear();
      this.gotoStack = (Stack<StateMachine.BaseState>) null;
      if (this.transitionStack != null)
        this.transitionStack.Clear();
      this.transitionStack = (Stack<StateMachine.BaseTransition.Context>) null;
      if (this.currentSchedulerGroup != null)
        this.currentSchedulerGroup.FreeResources();
      this.currentSchedulerGroup = (SchedulerGroup) null;
      if (this.stateStack != null)
      {
        for (int index = 0; index < this.stateStack.Length; ++index)
        {
          if (this.stateStack[index].schedulerGroup != null)
            this.stateStack[index].schedulerGroup.FreeResources();
        }
      }
      this.stateStack = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[]) null;
      base.FreeResources();
    }

    public GenericInstance(MasterType master)
      : base((StateMachine) (object) Singleton<StateMachineManager>.Instance.CreateStateMachine<StateMachineType>(), (IStateMachineTarget) master)
    {
      this.master = master;
      this.stateStack = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[this.stateMachine.GetMaxDepth()];
      for (int index = 0; index < this.stateStack.Length; ++index)
        this.stateStack[index].schedulerGroup = Singleton<StateMachineManager>.Instance.CreateSchedulerGroup();
      this.sm = (StateMachineType) this.stateMachine;
      this.dataTable = new object[this.GetStateMachine().dataTableSize];
      this.updateTable = new StateMachine.Instance.UpdateTableEntry[this.GetStateMachine().updateTableSize];
      this.controller = master.GetComponent<StateMachineController>();
      if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
        this.controller = master.gameObject.AddComponent<StateMachineController>();
      this.internalSm.masterTarget.Set(master.gameObject, this.smi, false);
      this.controller.AddStateMachineInstance((StateMachine.Instance) this);
    }

    public override IStateMachineTarget GetMaster() => (IStateMachineTarget) this.master;

    private void PushEvent(StateEvent evt)
    {
      this.subscribedEvents.Push(evt.Subscribe((StateMachine.Instance) this));
    }

    private void PopEvent()
    {
      StateEvent.Context context = this.subscribedEvents.Pop();
      context.stateEvent.Unsubscribe((StateMachine.Instance) this, context);
    }

    private bool TryEvaluateTransitions(StateMachine.BaseState state, int goto_id)
    {
      if (state.transitions == null)
        return true;
      bool transitions = true;
      for (int index = 0; index < state.transitions.Count; ++index)
      {
        StateMachine.BaseTransition transition = state.transitions[index];
        if (goto_id != this.gotoId)
        {
          transitions = false;
          break;
        }
        transition.Evaluate((StateMachine.Instance) this.smi);
      }
      return transitions;
    }

    private void PushTransitions(StateMachine.BaseState state)
    {
      if (state.transitions == null)
        return;
      for (int index = 0; index < state.transitions.Count; ++index)
        this.PushTransition(state.transitions[index]);
    }

    private void PushTransition(StateMachine.BaseTransition transition)
    {
      this.transitionStack.Push(transition.Register((StateMachine.Instance) this.smi));
    }

    private void PopTransition(
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
    {
      StateMachine.BaseTransition.Context context = this.transitionStack.Pop();
      state.transitions[context.idx].Unregister((StateMachine.Instance) this.smi, context);
    }

    private void PushState(StateMachine.BaseState state)
    {
      int gotoId1 = this.gotoId;
      this.currentActionIdx = -1;
      if (state.events != null)
      {
        foreach (StateEvent evt in state.events)
          this.PushEvent(evt);
      }
      this.PushTransitions(state);
      if (state.updateActions != null)
      {
        for (int index = 0; index < state.updateActions.Count; ++index)
        {
          StateMachine.UpdateAction updateAction = state.updateActions[index];
          int updateTableIdx = updateAction.updateTableIdx;
          int nextBucketIdx = updateAction.nextBucketIdx;
          updateAction.nextBucketIdx = (updateAction.nextBucketIdx + 1) % updateAction.buckets.Length;
          UpdateBucketWithUpdater<StateMachineInstanceType> bucket = (UpdateBucketWithUpdater<StateMachineInstanceType>) updateAction.buckets[nextBucketIdx];
          this.smi.updateTable[updateTableIdx].bucket = (StateMachineUpdater.BaseUpdateBucket) bucket;
          this.smi.updateTable[updateTableIdx].handle = bucket.Add(this.smi, Singleton<StateMachineUpdater>.Instance.GetFrameTime(updateAction.updateRate, bucket.frame), (UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater) updateAction.updater);
          state.updateActions[index] = updateAction;
        }
      }
      this.stateEnterTime = Time.time;
      this.stateStack[this.stackSize++].state = state;
      this.currentSchedulerGroup = this.stateStack[this.stackSize - 1].schedulerGroup;
      if (!this.TryEvaluateTransitions(state, gotoId1) || gotoId1 != this.gotoId)
        return;
      this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) state, state.enterActions);
      int gotoId2 = this.gotoId;
    }

    private void ExecuteActions(
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state,
      List<StateMachine.Action> actions)
    {
      if (actions == null)
        return;
      int gotoId = this.gotoId;
      for (++this.currentActionIdx; this.currentActionIdx < actions.Count && gotoId == this.gotoId; ++this.currentActionIdx)
      {
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback) actions[this.currentActionIdx].callback;
        try
        {
          callback(this.smi);
        }
        catch (Exception ex)
        {
          if (!StateMachine.Instance.error)
          {
            this.Error();
            string str = "(NULL).";
            IStateMachineTarget master = this.GetMaster();
            if (!master.isNull)
            {
              KPrefabID component = master.GetComponent<KPrefabID>();
              str = !((UnityEngine.Object) component != (UnityEngine.Object) null) ? $"({this.gameObject.name})." : $"({component.PrefabTag.ToString()}).";
            }
            string errorMessage = $"Exception in: {str}{this.stateMachine.ToString()}.{state.name}.";
            if (this.currentActionIdx > 0 && this.currentActionIdx < actions.Count)
              errorMessage += actions[this.currentActionIdx].name;
            DebugUtil.LogException((UnityEngine.Object) this.controller, errorMessage, ex);
          }
        }
      }
      this.currentActionIdx = 2147483646;
    }

    private void PopState()
    {
      this.currentActionIdx = -1;
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry state1 = this.stateStack[--this.stackSize];
      StateMachine.BaseState state2 = state1.state;
      for (int index = 0; state2.transitions != null && index < state2.transitions.Count; ++index)
        this.PopTransition((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) state2);
      if (state2.events != null)
      {
        for (int index = 0; index < state2.events.Count; ++index)
          this.PopEvent();
      }
      if (state2.updateActions != null)
      {
        foreach (StateMachine.UpdateAction updateAction in state2.updateActions)
        {
          int updateTableIdx = updateAction.updateTableIdx;
          UpdateBucketWithUpdater<StateMachineInstanceType> bucket = (UpdateBucketWithUpdater<StateMachineInstanceType>) this.smi.updateTable[updateTableIdx].bucket;
          this.smi.updateTable[updateTableIdx].bucket = (StateMachineUpdater.BaseUpdateBucket) null;
          HandleVector<int>.Handle handle = this.smi.updateTable[updateTableIdx].handle;
          bucket.Remove(handle);
        }
      }
      state1.schedulerGroup.Reset();
      this.currentSchedulerGroup = state1.schedulerGroup;
      this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) state2, state2.exitActions);
    }

    public override SchedulerHandle Schedule(
      float time,
      System.Action<object> callback,
      object callback_data = null)
    {
      return Singleton<StateMachineManager>.Instance.Schedule((string) null, time, callback, callback_data, this.currentSchedulerGroup);
    }

    public override SchedulerHandle ScheduleNextFrame(System.Action<object> callback, object callback_data = null)
    {
      return Singleton<StateMachineManager>.Instance.ScheduleNextFrame((string) null, callback, callback_data, this.currentSchedulerGroup);
    }

    public override void StartSM()
    {
      if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && !this.controller.HasStateMachineInstance((StateMachine.Instance) this))
        this.controller.AddStateMachineInstance((StateMachine.Instance) this);
      base.StartSM();
    }

    public override void StopSM(string reason)
    {
      if (StateMachine.Instance.error)
        return;
      if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
        this.controller.RemoveStateMachineInstance((StateMachine.Instance) this);
      if (!this.IsRunning())
        return;
      ++this.gotoId;
      while (this.stackSize > 0)
        this.PopState();
      if ((object) this.master != null && (UnityEngine.Object) this.controller != (UnityEngine.Object) null)
        this.controller.RemoveStateMachineInstance((StateMachine.Instance) this);
      if (this.status == StateMachine.Status.Running)
        this.SetStatus(StateMachine.Status.Failed);
      if (this.OnStop != null)
        this.OnStop(reason, this.status);
      for (int index = 0; index < this.parameterContexts.Length; ++index)
        this.parameterContexts[index].Cleanup();
      this.OnCleanUp();
    }

    private void FinishStateInProgress(StateMachine.BaseState state)
    {
      if (state.enterActions == null)
        return;
      this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State) state, state.enterActions);
    }

    public override void GoTo(StateMachine.BaseState base_state)
    {
      if (App.IsExiting || StateMachine.Instance.error || this.isMasterNull)
        return;
      if (this.smi.IsNullOrDestroyed())
        return;
      try
      {
        if (this.IsBreakOnGoToEnabled())
          Debugger.Break();
        if (base_state != null)
        {
          while (base_state.defaultState != null)
            base_state = base_state.defaultState;
        }
        if (this.GetCurrentState() == null)
          this.SetStatus(StateMachine.Status.Running);
        if (this.gotoStack.Count > 100)
        {
          string str = $"Potential infinite transition loop detected in state machine: {this.ToString()}\nGoto stack:\n";
          foreach (StateMachine.BaseState baseState in this.gotoStack)
            str = $"{str}\n{baseState.name}";
          Debug.LogError((object) str);
          this.Error();
        }
        else
        {
          this.gotoStack.Push(base_state);
          if (base_state == null)
          {
            this.StopSM("StateMachine.GoTo(null)");
            this.gotoStack.Pop();
          }
          else
          {
            int num = ++this.gotoId;
            StateMachine.BaseState[] branch = (base_state as StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State).branch;
            int index1 = 0;
            while (index1 < this.stackSize && index1 < branch.Length && this.stateStack[index1].state == branch[index1])
              ++index1;
            int index2 = this.stackSize - 1;
            if (index2 >= 0 && index2 == index1 - 1)
              this.FinishStateInProgress(this.stateStack[index2].state);
            while (this.stackSize > index1 && num == this.gotoId)
              this.PopState();
            for (int index3 = index1; index3 < branch.Length && num == this.gotoId; ++index3)
              this.PushState(branch[index3]);
            this.gotoStack.Pop();
          }
        }
      }
      catch (Exception ex)
      {
        if (StateMachine.Instance.error)
          return;
        this.Error();
        string str1 = "(Stop)";
        if (base_state != null)
          str1 = base_state.name;
        string str2 = "(NULL).";
        if (!this.GetMaster().isNull)
          str2 = $"({this.gameObject.name}).";
        DebugUtil.LogErrorArgs((UnityEngine.Object) this.controller, (object) $"{$"Exception in: {str2}{this.stateMachine.ToString()}.GoTo({str1})"}\n{ex.ToString()}");
      }
    }

    public override StateMachine.BaseState GetCurrentState()
    {
      return this.stackSize > 0 ? this.stateStack[this.stackSize - 1].state : (StateMachine.BaseState) null;
    }

    public struct StackEntry
    {
      public StateMachine.BaseState state;
      public SchedulerGroup schedulerGroup;
    }
  }

  public class State : StateMachine.BaseState
  {
    protected StateMachineType sm;

    public delegate void Callback(StateMachineInstanceType smi)
      where StateMachineInstanceType : StateMachine.Instance
      where MasterType : IStateMachineTarget;
  }

  public new abstract class ParameterTransition : StateMachine.ParameterTransition
  {
    public ParameterTransition(
      int idx,
      string name,
      StateMachine.BaseState source_state,
      StateMachine.BaseState target_state)
      : base(idx, name, source_state, target_state)
    {
    }
  }

  public class Transition : StateMachine.BaseTransition
  {
    public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

    public Transition(
      string name,
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state,
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state,
      int idx,
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition)
      : base(idx, name, (StateMachine.BaseState) source_state, (StateMachine.BaseState) target_state)
    {
      this.condition = condition;
    }

    public override string ToString()
    {
      return this.targetState != null ? $"{this.name}->{this.targetState.name}" : this.name + "->(Stop)";
    }

    public delegate bool ConditionCallback(StateMachineInstanceType smi)
      where StateMachineInstanceType : StateMachine.Instance
      where MasterType : IStateMachineTarget;
  }

  public abstract class Parameter<ParameterType> : StateMachine.Parameter
  {
    public ParameterType defaultValue;
    public bool isSignal;

    public Parameter()
    {
    }

    public Parameter(ParameterType default_value) => this.defaultValue = default_value;

    public ParameterType Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
    {
      ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this)).Set(value, smi, silenceEvents);
      return value;
    }

    public ParameterType Get(StateMachineInstanceType smi)
    {
      return ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this)).value;
    }

    public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context GetContext(
      StateMachineInstanceType smi)
    {
      return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this);
    }

    public delegate bool Callback(StateMachineInstanceType smi, ParameterType p)
      where StateMachineInstanceType : StateMachine.Instance
      where MasterType : IStateMachineTarget;

    public class Transition : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition
    {
      private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter;
      private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback;
      private System.Action<StateMachineInstanceType> evaluateAction;
      private System.Action<StateMachineInstanceType> triggerAction;

      public Transition(
        int idx,
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter,
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state,
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback)
        : base(idx, parameter.name, (StateMachine.BaseState) null, (StateMachine.BaseState) state)
      {
        this.parameter = parameter;
        this.callback = callback;
        this.evaluateAction = new System.Action<StateMachineInstanceType>(((StateMachine.BaseTransition) this).Evaluate);
        this.triggerAction = new System.Action<StateMachineInstanceType>(this.Trigger);
      }

      public override void Evaluate(StateMachine.Instance smi)
      {
        StateMachineInstanceType smi1 = smi as StateMachineInstanceType;
        Debug.Assert((object) smi1 != null);
        if (this.parameter.isSignal && this.callback == null)
          return;
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context parameterContext = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi1.GetParameterContext((StateMachine.Parameter) this.parameter);
        if (!this.callback(smi1, parameterContext.value))
          return;
        smi1.GoTo(this.targetState);
      }

      private void Trigger(StateMachineInstanceType smi) => smi.GoTo(this.targetState);

      public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
      {
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context parameterContext = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this.parameter);
        if (this.parameter.isSignal && this.callback == null)
          parameterContext.onDirty += this.triggerAction;
        else
          parameterContext.onDirty += this.evaluateAction;
        return new StateMachine.BaseTransition.Context((StateMachine.BaseTransition) this);
      }

      public override void Unregister(
        StateMachine.Instance smi,
        StateMachine.BaseTransition.Context transitionContext)
      {
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context parameterContext = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context) smi.GetParameterContext((StateMachine.Parameter) this.parameter);
        if (this.parameter.isSignal && this.callback == null)
          parameterContext.onDirty -= this.triggerAction;
        else
          parameterContext.onDirty -= this.evaluateAction;
      }

      public override string ToString()
      {
        return this.targetState != null ? $"{this.parameter.name}->{this.targetState.name}" : this.parameter.name + "->(Stop)";
      }
    }

    public new abstract class Context : StateMachine.Parameter.Context
    {
      public ParameterType value;
      public System.Action<StateMachineInstanceType> onDirty;

      public Context(StateMachine.Parameter parameter, ParameterType default_value)
        : base(parameter)
      {
        this.value = default_value;
      }

      public virtual void Set(
        ParameterType value,
        StateMachineInstanceType smi,
        bool silenceEvents = false)
      {
        if (EqualityComparer<ParameterType>.Default.Equals(value, this.value))
          return;
        this.value = value;
        if (silenceEvents || this.onDirty == null)
          return;
        this.onDirty(smi);
      }
    }
  }

  public class BoolParameter : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>
  {
    public BoolParameter()
    {
    }

    public BoolParameter(bool default_value)
      : base(default_value)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.BoolParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, bool default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer)
      {
        writer.Write(this.value ? (byte) 1 : (byte) 0);
      }

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        this.value = reader.ReadByte() > (byte) 0;
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        bool v = this.value;
        if (!ImGui.Checkbox(this.parameter.name, ref v))
          return;
        StateMachineInstanceType smi = (StateMachineInstanceType) base_smi;
        this.Set(v, smi);
      }
    }
  }

  public class Vector3Parameter : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>
  {
    public Vector3Parameter()
    {
    }

    public Vector3Parameter(Vector3 default_value)
      : base(default_value)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Vector3Parameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, Vector3 default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer)
      {
        writer.Write(this.value.x);
        writer.Write(this.value.y);
        writer.Write(this.value.z);
      }

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        this.value.x = reader.ReadSingle();
        this.value.y = reader.ReadSingle();
        this.value.z = reader.ReadSingle();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        Vector3 v = this.value;
        if (!ImGui.InputFloat3(this.parameter.name, ref v))
          return;
        StateMachineInstanceType smi = (StateMachineInstanceType) base_smi;
        this.Set(v, smi);
      }
    }
  }

  public class EnumParameter<EnumType>(EnumType default_value) : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>(default_value)
  {
    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EnumParameter<EnumType>.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, EnumType default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer)
      {
        writer.Write((int) (object) this.value);
      }

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        this.value = (EnumType) (ValueType) reader.ReadInt32();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        string[] names = Enum.GetNames(typeof (EnumType));
        Array values = Enum.GetValues(typeof (EnumType));
        int current_item = Array.IndexOf(values, (object) this.value);
        if (!ImGui.Combo(this.parameter.name, ref current_item, names, names.Length))
          return;
        StateMachineInstanceType smi = (StateMachineInstanceType) base_smi;
        this.Set((EnumType) values.GetValue(current_item), smi);
      }
    }
  }

  public class FloatParameter : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>
  {
    public FloatParameter()
    {
    }

    public FloatParameter(float default_value)
      : base(default_value)
    {
    }

    public float Delta(float delta_value, StateMachineInstanceType smi)
    {
      float num1 = this.Get(smi) + delta_value;
      double num2 = (double) this.Set(num1, smi);
      return num1;
    }

    public float DeltaClamp(
      float delta_value,
      float min_value,
      float max_value,
      StateMachineInstanceType smi)
    {
      float num1 = Mathf.Clamp(this.Get(smi) + delta_value, min_value, max_value);
      double num2 = (double) this.Set(num1, smi);
      return num1;
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, float default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer) => writer.Write(this.value);

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        this.value = reader.ReadSingle();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        float v = this.value;
        if (!ImGui.InputFloat(this.parameter.name, ref v))
          return;
        StateMachineInstanceType smi = (StateMachineInstanceType) base_smi;
        this.Set(v, smi);
      }
    }
  }

  public class IntParameter : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>
  {
    public IntParameter()
    {
    }

    public IntParameter(int default_value)
      : base(default_value)
    {
    }

    public int Delta(int delta_value, StateMachineInstanceType smi)
    {
      int num = this.Get(smi) + delta_value;
      this.Set(num, smi);
      return num;
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.IntParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, int default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer) => writer.Write(this.value);

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        this.value = reader.ReadInt32();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        int v = this.value;
        if (!ImGui.InputInt(this.parameter.name, ref v))
          return;
        StateMachineInstanceType smi = (StateMachineInstanceType) base_smi;
        this.Set(v, smi);
      }
    }
  }

  public class LongParameter : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>
  {
    public LongParameter()
    {
    }

    public LongParameter(long default_value)
      : base(default_value)
    {
    }

    public long Delta(long delta_value, StateMachineInstanceType smi)
    {
      long num = this.Get(smi) + delta_value;
      this.Set(num, smi);
      return num;
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.LongParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, long default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer) => writer.Write(this.value);

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        this.value = reader.ReadInt64();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        long num = this.value;
      }
    }
  }

  public class ResourceParameter<ResourceType> : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>
    where ResourceType : Resource
  {
    public ResourceParameter()
      : base(default (ResourceType))
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ResourceParameter<ResourceType>.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, ResourceType default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer)
      {
        string str = "";
        if ((object) this.value != null)
        {
          if (this.value.Guid == (ResourceGuid) null)
            Debug.LogError((object) ("Cannot serialize resource with invalid guid: " + this.value.Id));
          else
            str = this.value.Guid.Guid;
        }
        writer.WriteKleiString(str);
      }

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        string id = reader.ReadKleiString();
        if (!(id != ""))
          return;
        ResourceGuid guid = new ResourceGuid(id);
        this.value = Db.Get().GetResource<ResourceType>(guid);
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        string fmt = "None";
        if ((object) this.value != null)
          fmt = this.value.ToString();
        ImGui.LabelText(this.parameter.name, fmt);
      }
    }
  }

  public class TagParameter : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>
  {
    public TagParameter()
    {
    }

    public TagParameter(Tag default_value)
      : base(default_value)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, Tag default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer) => writer.Write(this.value.GetHash());

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        this.value = new Tag(reader.ReadInt32());
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        ImGui.LabelText(this.parameter.name, this.value.ToString());
      }
    }
  }

  public class AxialIParameter : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<AxialI>
  {
    public AxialIParameter()
    {
    }

    public AxialIParameter(AxialI default_value)
      : base(default_value)
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.AxialIParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, AxialI default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<AxialI>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer)
      {
        writer.Write(this.value.r);
        writer.Write(this.value.q);
      }

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        this.value.r = reader.ReadInt32();
        this.value.q = reader.ReadInt32();
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        ImGui.LabelText(this.parameter.name, this.value.ToString());
      }
    }
  }

  public class ObjectParameter<ObjectType> : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>
    where ObjectType : class
  {
    public ObjectParameter()
      : base(default (ObjectType))
    {
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ObjectParameter<ObjectType>.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, ObjectType default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer)
      {
        DebugUtil.DevLogError("ObjectParameter cannot be serialized");
      }

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        DebugUtil.DevLogError("ObjectParameter cannot be serialized");
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        string fmt = "None";
        if ((object) this.value != null)
          fmt = this.value.ToString();
        ImGui.LabelText(this.parameter.name, fmt);
      }
    }
  }

  public class TargetParameter : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>
  {
    public TargetParameter()
      : base((GameObject) null)
    {
    }

    public SMT GetSMI<SMT>(StateMachineInstanceType smi) where SMT : StateMachine.Instance
    {
      GameObject go = this.Get(smi);
      if ((UnityEngine.Object) go != (UnityEngine.Object) null)
      {
        SMT smi1 = go.GetSMI<SMT>();
        if ((object) smi1 != null)
          return smi1;
        Debug.LogError((object) $"{go.name} does not have state machine {typeof (StateMachineType).Name}");
      }
      return default (SMT);
    }

    public bool IsNull(StateMachineInstanceType smi) => (UnityEngine.Object) this.Get(smi) == (UnityEngine.Object) null;

    public ComponentType Get<ComponentType>(StateMachineInstanceType smi)
    {
      GameObject gameObject = this.Get(smi);
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        ComponentType component = gameObject.GetComponent<ComponentType>();
        if ((object) component != null)
          return component;
        Debug.LogError((object) $"{gameObject.name} does not have component {typeof (ComponentType).Name}");
      }
      return default (ComponentType);
    }

    public ComponentType AddOrGet<ComponentType>(StateMachineInstanceType smi) where ComponentType : Component
    {
      GameObject gameObject = this.Get(smi);
      if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
        return default (ComponentType);
      ComponentType componentType = gameObject.GetComponent<ComponentType>();
      if ((UnityEngine.Object) componentType == (UnityEngine.Object) null)
        componentType = gameObject.AddComponent<ComponentType>();
      return componentType;
    }

    public void Set(KMonoBehaviour value, StateMachineInstanceType smi)
    {
      GameObject gameObject = (GameObject) null;
      if ((UnityEngine.Object) value != (UnityEngine.Object) null)
        gameObject = value.gameObject;
      this.Set(gameObject, smi, false);
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(StateMachine.Parameter parameter, GameObject default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Context(parameter, default_value)
    {
      private StateMachineInstanceType m_smi;
      private int objectDestroyedHandler = -1;
      private static System.Action<object, object> OnObjectDestroyedDispatcher = (System.Action<object, object>) ((context, data) => Unsafe.As<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context>(context).OnObjectDestroyed(data));

      public override void Serialize(BinaryWriter writer)
      {
        if ((UnityEngine.Object) this.value != (UnityEngine.Object) null)
        {
          int instanceId = this.value.GetComponent<KPrefabID>().InstanceID;
          writer.Write(instanceId);
        }
        else
          writer.Write(0);
      }

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
        try
        {
          int instance_id = reader.ReadInt32();
          if (instance_id == 0)
            return;
          KPrefabID instance = KPrefabIDTracker.Get().GetInstance(instance_id);
          if ((UnityEngine.Object) instance != (UnityEngine.Object) null)
          {
            this.value = instance.gameObject;
            this.objectDestroyedHandler = instance.Subscribe(1969584890, new System.Action<object>(this.OnObjectDestroyed));
          }
          this.m_smi = (StateMachineInstanceType) smi;
        }
        catch (Exception ex)
        {
          if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
            return;
          Debug.LogWarning((object) ("Missing statemachine target params. " + ex.Message));
        }
      }

      public override void Cleanup()
      {
        base.Cleanup();
        if (!((UnityEngine.Object) this.value != (UnityEngine.Object) null))
          return;
        this.value.GetComponent<KMonoBehaviour>().Unsubscribe(ref this.objectDestroyedHandler);
      }

      public override void Set(GameObject value, StateMachineInstanceType smi, bool silenceEvents = false)
      {
        this.m_smi = smi;
        if ((UnityEngine.Object) this.value != (UnityEngine.Object) null)
          this.value.GetComponent<KMonoBehaviour>().Unsubscribe(ref this.objectDestroyedHandler);
        if ((UnityEngine.Object) value != (UnityEngine.Object) null)
          this.objectDestroyedHandler = value.GetComponent<KMonoBehaviour>().Subscribe(1969584890, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context.OnObjectDestroyedDispatcher, (object) this);
        base.Set(value, smi, silenceEvents);
      }

      private void OnObjectDestroyed(object data) => this.Set((GameObject) null, this.m_smi, false);

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        if ((UnityEngine.Object) this.value != (UnityEngine.Object) null)
          ImGui.LabelText(this.parameter.name, this.value.name);
        else
          ImGui.LabelText(this.parameter.name, "null");
      }
    }
  }

  public class SignalParameter
  {
  }

  public class Signal : 
    StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>
  {
    public Signal()
      : base((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter) null)
    {
      this.isSignal = true;
    }

    public void Trigger(StateMachineInstanceType smi)
    {
      ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context) smi.GetParameterContext((StateMachine.Parameter) this)).Set((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter) null, smi);
    }

    public override StateMachine.Parameter.Context CreateContext()
    {
      return (StateMachine.Parameter.Context) new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context((StateMachine.Parameter) this, this.defaultValue);
    }

    public new class Context(
      StateMachine.Parameter parameter,
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter default_value) : 
      StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context(parameter, default_value)
    {
      public override void Serialize(BinaryWriter writer)
      {
      }

      public override void Deserialize(IReader reader, StateMachine.Instance smi)
      {
      }

      public override void Set(
        StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter value,
        StateMachineInstanceType smi,
        bool silenceEvents = false)
      {
        if (silenceEvents || this.onDirty == null)
          return;
        this.onDirty(smi);
      }

      public override void ShowEditor(StateMachine.Instance base_smi)
      {
      }

      public override void ShowDevTool(StateMachine.Instance base_smi)
      {
        if (!ImGui.Button(this.parameter.name))
          return;
        this.Set((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter) null, (StateMachineInstanceType) base_smi, false);
      }
    }
  }
}
