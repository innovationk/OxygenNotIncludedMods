// Decompiled with JetBrains decompiler
// Type: StateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;

#nullable disable
public abstract class StateMachine
{
  protected string name;
  protected int maxDepth;
  protected StateMachine.BaseState defaultState;
  protected StateMachine.Parameter[] parameters = new StateMachine.Parameter[0];
  public int dataTableSize;
  public int updateTableSize;
  public StateMachineDebuggerSettings.Entry debugSettings;
  public bool saveHistory;

  public StateMachine() => this.name = this.GetType().FullName;

  public virtual void FreeResources()
  {
    this.name = (string) null;
    if (this.defaultState != null)
      this.defaultState.FreeResources();
    this.defaultState = (StateMachine.BaseState) null;
    this.parameters = (StateMachine.Parameter[]) null;
  }

  public abstract string[] GetStateNames();

  public abstract StateMachine.BaseState GetState(string name);

  public abstract void BindStates();

  public abstract System.Type GetStateMachineInstanceType();

  public int version { get; protected set; }

  public StateMachine.SerializeType serializable { get; protected set; }

  public virtual void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) null;
  }

  public void InitializeStateMachine()
  {
    this.debugSettings = StateMachineDebuggerSettings.Get().CreateEntry(this.GetType());
    StateMachine.BaseState default_state = (StateMachine.BaseState) null;
    this.InitializeStates(out default_state);
    DebugUtil.Assert(default_state != null);
    this.defaultState = default_state;
  }

  public void CreateStates(object state_machine)
  {
    foreach (FieldInfo field in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
    {
      bool flag = false;
      foreach (object customAttribute in field.GetCustomAttributes(false))
      {
        if (customAttribute.GetType() == typeof (StateMachine.DoNotAutoCreate))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        if (field.FieldType.IsSubclassOf(typeof (StateMachine.BaseState)))
        {
          StateMachine.BaseState instance = (StateMachine.BaseState) Activator.CreateInstance(field.FieldType);
          this.CreateStates((object) instance);
          field.SetValue(state_machine, (object) instance);
        }
        else if (field.FieldType.IsSubclassOf(typeof (StateMachine.Parameter)))
        {
          StateMachine.Parameter instance = (StateMachine.Parameter) field.GetValue(state_machine);
          if (instance == null)
          {
            instance = (StateMachine.Parameter) Activator.CreateInstance(field.FieldType);
            field.SetValue(state_machine, (object) instance);
          }
          instance.name = field.Name;
          instance.idx = this.parameters.Length;
          this.parameters = this.parameters.Append<StateMachine.Parameter>(instance);
        }
        else if (field.FieldType.IsSubclassOf(typeof (StateMachine)))
          field.SetValue(state_machine, (object) this);
      }
    }
  }

  public StateMachine.BaseState GetDefaultState() => this.defaultState;

  public int GetMaxDepth() => this.maxDepth;

  public override string ToString() => this.name;

  public sealed class DoNotAutoCreate : Attribute
  {
  }

  public enum Status
  {
    Initialized,
    Running,
    Failed,
    Success,
  }

  public class BaseDef
  {
    public bool preventStartSMIOnSpawn;

    public StateMachine.Instance CreateSMI(IStateMachineTarget master)
    {
      return Singleton<StateMachineManager>.Instance.CreateSMIFromDef(master, this);
    }

    public System.Type GetStateMachineType() => this.GetType().DeclaringType;

    public virtual void Configure(GameObject prefab)
    {
    }
  }

  public class Category(string id) : Resource(id)
  {
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public abstract class Instance
  {
    public string serializationSuffix;
    protected LoggerFSSSS log;
    protected StateMachine.Status status;
    protected StateMachine stateMachine;
    protected Stack<StateEvent.Context> subscribedEvents = new Stack<StateEvent.Context>();
    protected int stackSize;
    protected StateMachine.Parameter.Context[] parameterContexts;
    public object[] dataTable;
    public StateMachine.Instance.UpdateTableEntry[] updateTable;
    private System.Action<object> scheduleGoToCallback;
    public System.Action<string, StateMachine.Status> OnStop;
    public bool breakOnGoTo;
    public bool enableConsoleLogging;
    public bool isCrashed;
    public static bool error;

    public abstract StateMachine.BaseState GetCurrentState();

    public abstract void GoTo(StateMachine.BaseState state);

    public abstract float timeinstate { get; }

    public abstract IStateMachineTarget GetMaster();

    public abstract void StopSM(string reason);

    public abstract SchedulerHandle Schedule(
      float time,
      System.Action<object> callback,
      object callback_data = null);

    public abstract SchedulerHandle ScheduleNextFrame(System.Action<object> callback, object callback_data = null);

    public virtual void FreeResources()
    {
      this.stateMachine = (StateMachine) null;
      if (this.subscribedEvents != null)
        this.subscribedEvents.Clear();
      this.subscribedEvents = (Stack<StateEvent.Context>) null;
      this.parameterContexts = (StateMachine.Parameter.Context[]) null;
      this.dataTable = (object[]) null;
      this.updateTable = (StateMachine.Instance.UpdateTableEntry[]) null;
    }

    public Instance(StateMachine state_machine, IStateMachineTarget master)
    {
      this.stateMachine = state_machine;
      this.CreateParameterContexts();
      this.log = new LoggerFSSSS(this.stateMachine.name);
    }

    public virtual void PostParamsInitialized()
    {
    }

    public bool IsRunning() => this.GetCurrentState() != null;

    public void GoTo(string state_name)
    {
      DebugUtil.DevAssert(!KMonoBehaviour.isLoadingScene, "Using Goto while scene was loaded");
      this.GoTo(this.stateMachine.GetState(state_name));
    }

    public int GetStackSize() => this.stackSize;

    public StateMachine GetStateMachine() => this.stateMachine;

    [Conditional("UNITY_EDITOR")]
    public void Log(string a, string b = "", string c = "", string d = "")
    {
    }

    public bool IsConsoleLoggingEnabled()
    {
      return this.enableConsoleLogging || this.stateMachine.debugSettings.enableConsoleLogging;
    }

    public bool IsBreakOnGoToEnabled()
    {
      return this.breakOnGoTo || this.stateMachine.debugSettings.breakOnGoTo;
    }

    public LoggerFSSSS GetLog() => this.log;

    public StateMachine.Parameter.Context[] GetParameterContexts() => this.parameterContexts;

    public StateMachine.Parameter.Context GetParameterContext(StateMachine.Parameter parameter)
    {
      return this.parameterContexts[parameter.idx];
    }

    public StateMachine.Status GetStatus() => this.status;

    public void SetStatus(StateMachine.Status status) => this.status = status;

    public void Error()
    {
      if (StateMachine.Instance.error)
        return;
      this.isCrashed = true;
      StateMachine.Instance.error = true;
      RestartWarning.ShouldWarn = true;
    }

    public override string ToString()
    {
      string str = "";
      if (this.GetCurrentState() != null)
        str = this.GetCurrentState().name;
      else if (this.GetStatus() != StateMachine.Status.Initialized)
        str = this.GetStatus().ToString();
      return $"{this.stateMachine.ToString()}({str})";
    }

    public virtual void StartSM()
    {
      if (this.IsRunning())
        return;
      StateMachineController component = this.GetComponent<StateMachineController>();
      MyAttributes.OnStart((object) this, (KMonoBehaviour) component);
      StateMachine.BaseState defaultState = this.stateMachine.GetDefaultState();
      DebugUtil.Assert(defaultState != null);
      if (component.Restore(this))
        return;
      this.PostParamsInitialized();
      this.GoTo(defaultState);
    }

    public bool HasTag(Tag tag) => this.GetComponent<KPrefabID>().HasTag(tag);

    public bool IsInsideState(StateMachine.BaseState state)
    {
      StateMachine.BaseState currentState = this.GetCurrentState();
      if (currentState == null)
        return false;
      bool flag = state == currentState;
      int index = 0;
      while (!flag && index < currentState.branch.Length && !(flag = state == currentState.branch[index]))
        ++index;
      return flag;
    }

    public void ScheduleGoTo(float time, StateMachine.BaseState state)
    {
      if (this.scheduleGoToCallback == null)
        this.scheduleGoToCallback = (System.Action<object>) (d => this.GoTo((StateMachine.BaseState) d));
      this.Schedule(time, this.scheduleGoToCallback, (object) state);
    }

    public int Subscribe(int hash, System.Action<object> handler)
    {
      return this.GetMaster().Subscribe(hash, handler);
    }

    public int Subscribe(int hash, System.Action<object, object> handler, object context)
    {
      return this.GetMaster().Subscribe(hash, handler, context);
    }

    public void Unsubscribe(int hash, System.Action<object> handler)
    {
      this.GetMaster().Unsubscribe(hash, handler);
    }

    public void Unsubscribe(int id) => this.GetMaster().Unsubscribe(id);

    public void Unsubscribe(ref int id)
    {
      this.GetMaster().Unsubscribe(id);
      id = -1;
    }

    public void Trigger(int hash, object data = null)
    {
      this.GetMaster().GetComponent<KPrefabID>().Trigger(hash, data);
    }

    [Obsolete("Use BoxingTrigger to avoid sended boxing object to garbage collection, be careful to unbox parameter in any handlers")]
    public void Trigger<T>(int hash, T data) where T : struct => this.Trigger(hash, (object) data);

    public void BoxingTrigger(int hash, bool data)
    {
      this.Trigger(hash, (object) BoxedBools.Box(data));
    }

    public void BoxingTrigger<T>(int hash, T data) where T : struct
    {
      Boxed<T> data1 = Boxed<T>.Get(data);
      this.Trigger(hash, (object) data1);
      data1.Release();
    }

    public ComponentType Get<ComponentType>() => this.GetComponent<ComponentType>();

    public ComponentType GetComponent<ComponentType>()
    {
      return this.GetMaster().GetComponent<ComponentType>();
    }

    private void CreateParameterContexts()
    {
      this.parameterContexts = new StateMachine.Parameter.Context[this.stateMachine.parameters.Length];
      for (int index = 0; index < this.stateMachine.parameters.Length; ++index)
        this.parameterContexts[index] = this.stateMachine.parameters[index].CreateContext();
    }

    public GameObject gameObject => this.GetMaster().gameObject;

    public Transform transform => this.gameObject.transform;

    public struct UpdateTableEntry
    {
      public HandleVector<int>.Handle handle;
      public StateMachineUpdater.BaseUpdateBucket bucket;
    }
  }

  [DebuggerDisplay("{longName}")]
  public class BaseState
  {
    public string name;
    public string longName;
    public StateMachine.BaseState defaultState;
    public List<StateEvent> events;
    public List<StateMachine.BaseTransition> transitions;
    public List<StateMachine.UpdateAction> updateActions;
    public List<StateMachine.Action> enterActions;
    public List<StateMachine.Action> exitActions;
    public StateMachine.BaseState[] branch;
    public StateMachine.BaseState parent;

    public BaseState()
    {
      this.branch = new StateMachine.BaseState[1];
      this.branch[0] = this;
    }

    public void FreeResources()
    {
      if (this.name == null)
        return;
      this.name = (string) null;
      if (this.defaultState != null)
        this.defaultState.FreeResources();
      this.defaultState = (StateMachine.BaseState) null;
      this.events = (List<StateEvent>) null;
      for (int index = 0; this.transitions != null && index < this.transitions.Count; ++index)
        this.transitions[index].Clear();
      this.transitions = (List<StateMachine.BaseTransition>) null;
      this.enterActions = (List<StateMachine.Action>) null;
      this.exitActions = (List<StateMachine.Action>) null;
      if (this.branch != null)
      {
        for (int index = 0; index < this.branch.Length; ++index)
          this.branch[index].FreeResources();
      }
      this.branch = (StateMachine.BaseState[]) null;
      this.parent = (StateMachine.BaseState) null;
    }

    public int GetStateCount() => this.branch.Length;

    public StateMachine.BaseState GetState(int idx) => this.branch[idx];
  }

  public class BaseTransition
  {
    public int idx;
    public string name;
    public StateMachine.BaseState sourceState;
    public StateMachine.BaseState targetState;

    public BaseTransition(
      int idx,
      string name,
      StateMachine.BaseState source_state,
      StateMachine.BaseState target_state)
    {
      this.idx = idx;
      this.name = name;
      this.sourceState = source_state;
      this.targetState = target_state;
    }

    public virtual void Evaluate(StateMachine.Instance smi)
    {
    }

    public virtual StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
    {
      return new StateMachine.BaseTransition.Context(this);
    }

    public virtual void Unregister(
      StateMachine.Instance smi,
      StateMachine.BaseTransition.Context context)
    {
    }

    public void Clear()
    {
      this.name = (string) null;
      if (this.sourceState != null)
        this.sourceState.FreeResources();
      this.sourceState = (StateMachine.BaseState) null;
      if (this.targetState != null)
        this.targetState.FreeResources();
      this.targetState = (StateMachine.BaseState) null;
    }

    public struct Context
    {
      public int idx;
      public int handlerId;

      public Context(StateMachine.BaseTransition transition)
      {
        this.idx = transition.idx;
        this.handlerId = 0;
      }
    }
  }

  public struct UpdateAction
  {
    public int updateTableIdx;
    public UpdateRate updateRate;
    public int nextBucketIdx;
    public StateMachineUpdater.BaseUpdateBucket[] buckets;
    public object updater;
  }

  public struct Action(string name, object callback)
  {
    public string name = name;
    public object callback = callback;
  }

  public class ParameterTransition(
    int idx,
    string name,
    StateMachine.BaseState source_state,
    StateMachine.BaseState target_state) : StateMachine.BaseTransition(idx, name, source_state, target_state)
  {
  }

  public abstract class Parameter
  {
    public string name;
    public int idx;

    public abstract StateMachine.Parameter.Context CreateContext();

    public abstract class Context
    {
      public StateMachine.Parameter parameter;

      public Context(StateMachine.Parameter parameter) => this.parameter = parameter;

      public abstract void Serialize(BinaryWriter writer);

      public abstract void Deserialize(IReader reader, StateMachine.Instance smi);

      public virtual void Cleanup()
      {
      }

      public abstract void ShowEditor(StateMachine.Instance base_smi);

      public abstract void ShowDevTool(StateMachine.Instance base_smi);
    }
  }

  public enum SerializeType
  {
    Never,
    ParamsOnly,
    CurrentStateOnly_DEPRECATED,
    Both_DEPRECATED,
  }
}
