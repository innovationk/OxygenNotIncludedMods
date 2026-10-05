// Decompiled with JetBrains decompiler
// Type: Chore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public abstract class Chore
{
  public PrioritySetting masterPriority;
  public bool showAvailabilityInHoverText = true;
  public Action<Chore> onExit;
  public Action<Chore> onComplete;
  private static int nextId;
  public const int MAX_PLAYER_BASIC_PRIORITY = 9;
  public const int MIN_PLAYER_BASIC_PRIORITY = 1;
  public const int MAX_PLAYER_HIGH_PRIORITY = 0;
  public const int MIN_PLAYER_HIGH_PRIORITY = 0;
  public const int MAX_PLAYER_EMERGENCY_PRIORITY = 1;
  public const int MIN_PLAYER_EMERGENCY_PRIORITY = 1;
  public const int DEFAULT_BASIC_PRIORITY = 5;
  public const int MAX_BASIC_PRIORITY = 10;
  public const int MIN_BASIC_PRIORITY = 0;
  public static bool ENABLE_PERSONAL_PRIORITIES = true;
  public static PrioritySetting DefaultPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

  public abstract int id { get; protected set; }

  public abstract int priorityMod { get; protected set; }

  public abstract ChoreType choreType { get; protected set; }

  public abstract ChoreDriver driver { get; protected set; }

  public abstract ChoreDriver lastDriver { get; protected set; }

  public abstract bool isNull { get; }

  public abstract GameObject gameObject { get; }

  public abstract bool SatisfiesUrge(Urge urge);

  public abstract bool IsValid();

  public abstract IStateMachineTarget target { get; protected set; }

  public abstract bool isComplete { get; protected set; }

  public abstract bool IsPreemptable { get; protected set; }

  public abstract ChoreConsumer overrideTarget { get; protected set; }

  public abstract Prioritizable prioritizable { get; protected set; }

  public abstract ChoreProvider provider { get; set; }

  public abstract bool runUntilComplete { get; set; }

  public abstract bool isExpanded { get; protected set; }

  public abstract List<Chore.PreconditionInstance> GetPreconditions();

  public abstract bool CanPreempt(Chore.Precondition.Context context);

  public abstract void PrepareChore(ref Chore.Precondition.Context context);

  public abstract void Cancel(string reason);

  public abstract ReportManager.ReportType GetReportType();

  public abstract string GetReportName(string context = null);

  public abstract void AddPrecondition(Chore.Precondition precondition, object data = null);

  public abstract void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> incomplete_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override);

  public void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
    this.CollectChores(consumer_state, succeeded_contexts, (List<Chore.Precondition.Context>) null, failed_contexts, is_attempting_override);
  }

  public abstract void Cleanup();

  public abstract void Fail(string reason);

  public abstract void Reserve(ChoreDriver reserver);

  public abstract void Begin(Chore.Precondition.Context context);

  public abstract bool InProgress();

  public virtual string ResolveString(string str) => str;

  public static int GetNextChoreID() => ++Chore.nextId;

  public delegate bool PreconditionFn(ref Chore.Precondition.Context context, object data);

  [DebuggerDisplay("{condition}")]
  public struct PreconditionInstance
  {
    public Chore.Precondition condition;
    public object data;
  }

  [DebuggerDisplay("{id}")]
  public struct Precondition
  {
    public string id;
    public string description;
    public int sortOrder;
    public Chore.PreconditionFn fn;
    public bool canExecuteOnAnyThread;

    [DebuggerDisplay("{chore.GetType()}, {chore.gameObject.name}, {failedPreconditionId}")]
    public struct Context : 
      IComparable<Chore.Precondition.Context>,
      IEquatable<Chore.Precondition.Context>
    {
      public PrioritySetting masterPriority;
      public int personalPriority;
      public int priority;
      public int priorityMod;
      public int interruptPriority;
      public int cost;
      public int consumerPriority;
      public Chore chore;
      public ChoreConsumerState consumerState;
      public int failedPreconditionId;
      public bool skippedPreconditions;
      public object data;
      public bool isAttemptingOverride;
      public ChoreType choreTypeForPermission;
      public bool skipMoreSatisfyingEarlyPrecondition;

      public Context(
        Chore chore,
        ChoreConsumerState consumer_state,
        bool is_attempting_override,
        object data = null)
      {
        this.masterPriority = chore.masterPriority;
        this.personalPriority = consumer_state.consumer.GetPersonalPriority(chore.choreType);
        this.priority = 0;
        this.priorityMod = chore.priorityMod;
        this.consumerPriority = 0;
        this.interruptPriority = 0;
        this.cost = 0;
        this.chore = chore;
        this.consumerState = consumer_state;
        this.failedPreconditionId = -1;
        this.skippedPreconditions = false;
        this.isAttemptingOverride = is_attempting_override;
        this.data = data;
        this.choreTypeForPermission = chore.choreType;
        this.skipMoreSatisfyingEarlyPrecondition = (UnityEngine.Object) RootMenu.Instance != (UnityEngine.Object) null && RootMenu.Instance.IsBuildingChorePanelActive();
        this.SetPriority(chore);
      }

      public void Set(
        Chore chore,
        ChoreConsumerState consumer_state,
        bool is_attempting_override,
        object data = null)
      {
        this.masterPriority = chore.masterPriority;
        this.priority = 0;
        this.priorityMod = chore.priorityMod;
        this.consumerPriority = 0;
        this.interruptPriority = 0;
        this.cost = 0;
        this.chore = chore;
        this.consumerState = consumer_state;
        this.failedPreconditionId = -1;
        this.skippedPreconditions = false;
        this.isAttemptingOverride = is_attempting_override;
        this.data = data;
        this.choreTypeForPermission = chore.choreType;
        this.SetPriority(chore);
      }

      public void SetPriority(Chore chore)
      {
        this.priority = Game.Instance.advancedPersonalPriorities ? chore.choreType.explicitPriority : chore.choreType.priority;
        this.priorityMod = chore.priorityMod;
        this.interruptPriority = chore.choreType.interruptPriority;
      }

      public bool IsSuccess() => this.failedPreconditionId == -1 && !this.skippedPreconditions;

      public bool IsComplete() => !this.skippedPreconditions;

      public bool IsPotentialSuccess()
      {
        if (this.IsSuccess() || (UnityEngine.Object) this.chore.driver == (UnityEngine.Object) this.consumerState.choreDriver)
          return true;
        if (this.failedPreconditionId != -1)
        {
          if (this.failedPreconditionId >= 0 && this.failedPreconditionId < this.chore.GetPreconditions().Count)
            return this.chore.GetPreconditions()[this.failedPreconditionId].condition.id == ChorePreconditions.instance.IsMoreSatisfyingLate.id;
          DebugUtil.DevLogErrorFormat("failedPreconditionId out of range {0}/{1}", (object) this.failedPreconditionId, (object) this.chore.GetPreconditions().Count);
        }
        return false;
      }

      private void DoPreconditions(bool mainThreadOnly)
      {
        bool flag = Game.IsOnMainThread();
        List<Chore.PreconditionInstance> preconditions = this.chore.GetPreconditions();
        this.skippedPreconditions = false;
        for (int index = 0; index < preconditions.Count; ++index)
        {
          Chore.PreconditionInstance preconditionInstance = preconditions[index];
          if (preconditionInstance.condition.canExecuteOnAnyThread)
          {
            if (mainThreadOnly)
              continue;
          }
          else if (!flag)
          {
            this.skippedPreconditions = true;
            continue;
          }
          if (!preconditionInstance.condition.fn(ref this, preconditionInstance.data))
          {
            this.failedPreconditionId = index;
            this.skippedPreconditions = false;
            break;
          }
        }
      }

      public void RunPreconditions() => this.DoPreconditions(false);

      public void FinishPreconditions() => this.DoPreconditions(true);

      public int CompareTo(Chore.Precondition.Context obj)
      {
        bool flag1 = this.failedPreconditionId != -1;
        bool flag2 = obj.failedPreconditionId != -1;
        if (flag1 == flag2)
        {
          int num1 = this.masterPriority.priority_class - obj.masterPriority.priority_class;
          if (num1 != 0)
            return num1;
          int num2 = this.personalPriority - obj.personalPriority;
          if (num2 != 0)
            return num2;
          int num3 = this.masterPriority.priority_value - obj.masterPriority.priority_value;
          if (num3 != 0)
            return num3;
          int num4 = this.priority - obj.priority;
          if (num4 != 0)
            return num4;
          int num5 = this.priorityMod - obj.priorityMod;
          if (num5 != 0)
            return num5;
          int num6 = this.consumerPriority - obj.consumerPriority;
          if (num6 != 0)
            return num6;
          int num7 = obj.cost - this.cost;
          if (num7 != 0)
            return num7;
          if (this.chore == null && obj.chore == null)
            return 0;
          if (this.chore == null)
            return -1;
          return obj.chore == null ? 1 : this.chore.id - obj.chore.id;
        }
        return !flag1 ? 1 : -1;
      }

      public override bool Equals(object obj)
      {
        return this.CompareTo((Chore.Precondition.Context) obj) == 0;
      }

      public bool Equals(Chore.Precondition.Context other) => this.CompareTo(other) == 0;

      public override int GetHashCode() => base.GetHashCode();

      public static bool operator ==(Chore.Precondition.Context x, Chore.Precondition.Context y)
      {
        return x.CompareTo(y) == 0;
      }

      public static bool operator !=(Chore.Precondition.Context x, Chore.Precondition.Context y)
      {
        return x.CompareTo(y) != 0;
      }

      public static bool ShouldFilter(string filter, string text)
      {
        if (string.IsNullOrEmpty(filter))
          return false;
        return string.IsNullOrEmpty(text) || text.ToLower().IndexOf(filter) < 0;
      }
    }
  }
}
