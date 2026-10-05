// Decompiled with JetBrains decompiler
// Type: FetchChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class FetchChore : Chore<FetchChore.StatesInstance>
{
  public HashSet<Tag> tags;
  public Tag tagsFirst;
  public FetchChore.MatchCriteria criteria;
  public int tagsHash;
  public bool validateRequiredTagOnTagChange;
  public Tag requiredTag;
  public Tag[] forbiddenTags;
  public int forbidHash;
  public Automatable automatable;
  public bool allowMultifetch = true;
  private HandleVector<int>.Handle partitionerEntry;
  private int onOnlyFetchMarkedItemsSettingChangedHandle = -1;
  public static readonly Chore.Precondition IsFetchTargetAvailable = new Chore.Precondition()
  {
    id = nameof (IsFetchTargetAvailable),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      FetchChore chore = (FetchChore) context.chore;
      Pickupable pickup = (Pickupable) context.data;
      bool flag;
      if ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
      {
        pickup = chore.FindFetchTarget(context.consumerState);
        flag = (UnityEngine.Object) pickup != (UnityEngine.Object) null;
      }
      else
        flag = FetchManager.IsFetchablePickup(pickup, chore, context.consumerState.storage);
      if (flag)
      {
        if ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
        {
          Debug.Log((object) $"Failed to find fetch target for {chore.destination}");
          return false;
        }
        context.data = (object) pickup;
        if (context.consumerState.worker.IsFetchDrone())
        {
          int cost;
          if (((UnityEngine.Object) pickup.targetWorkable == (UnityEngine.Object) null || (UnityEngine.Object) pickup.targetWorkable.GetComponent<Pickupable>() != (UnityEngine.Object) null) && context.consumerState.consumer.GetNavigationCost((IApproachable) pickup, out cost))
          {
            context.cost += cost;
            return true;
          }
        }
        else
        {
          int cost;
          if (context.consumerState.consumer.GetNavigationCost((IApproachable) pickup, out cost))
          {
            context.cost += cost;
            return true;
          }
        }
      }
      return false;
    }),
    canExecuteOnAnyThread = false
  };
  public static readonly Chore.Precondition CanFetchDroneComplete = new Chore.Precondition()
  {
    id = nameof (CanFetchDroneComplete),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_FETCH_DRONE_COMPLETE_FETCH,
    canExecuteOnAnyThread = false,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (!context.consumerState.worker.IsFetchDrone())
        return true;
      FetchChore chore = (FetchChore) context.chore;
      Pickupable pickup = (Pickupable) context.data;
      bool flag;
      if ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
      {
        pickup = chore.FindFetchTarget(context.consumerState);
        flag = (UnityEngine.Object) pickup != (UnityEngine.Object) null;
      }
      else
        flag = FetchManager.IsFetchablePickup(pickup, chore, context.consumerState.storage);
      return flag && !((UnityEngine.Object) data == (UnityEngine.Object) context.consumerState.gameObject) && ((UnityEngine.Object) pickup.targetWorkable == (UnityEngine.Object) null || (UnityEngine.Object) (pickup.targetWorkable as Pickupable) != (UnityEngine.Object) null) && context.consumerState.consumer.navigator.CanReach(pickup.cachedCell, pickup.GetOffsets());
    })
  };

  public float originalAmount => this.smi.sm.requestedamount.Get(this.smi);

  public float amount
  {
    get => this.smi.sm.actualamount.Get(this.smi);
    set
    {
      double num = (double) this.smi.sm.actualamount.Set(value, this.smi);
    }
  }

  public Pickupable fetchTarget
  {
    get => this.smi.sm.chunk.Get<Pickupable>(this.smi);
    set => this.smi.sm.chunk.Set((KMonoBehaviour) value, this.smi);
  }

  public GameObject fetcher
  {
    get => this.smi.sm.fetcher.Get(this.smi);
    set => this.smi.sm.fetcher.Set(value, this.smi, false);
  }

  public Storage destination { get; private set; }

  public void FetchAreaBegin(Chore.Precondition.Context context, float amount_to_be_fetched)
  {
    this.amount = amount_to_be_fetched;
    this.smi.sm.fetcher.Set(context.consumerState.gameObject, this.smi, false);
    ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, context.chore.choreType.Name, GameUtil.GetChoreName((Chore) this, context.data));
    base.Begin(context);
  }

  public void FetchAreaEnd(ChoreDriver driver, Pickupable pickupable, bool is_success)
  {
    if (is_success)
    {
      ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName((Chore) this, (object) pickupable));
      this.fetchTarget = pickupable;
      this.driver = driver;
      this.fetcher = driver.gameObject;
      this.Succeed(nameof (FetchAreaEnd));
      SaveGame.Instance.ColonyAchievementTracker.LogFetchChore(this.fetcher, this.choreType);
    }
    else
    {
      this.SetOverrideTarget((ChoreConsumer) null);
      this.Fail("FetchAreaFail");
    }
  }

  public Pickupable FindFetchTarget(ChoreConsumerState consumer_state)
  {
    if (!((UnityEngine.Object) this.destination != (UnityEngine.Object) null))
      return (Pickupable) null;
    return consumer_state.hasSolidTransferArm ? consumer_state.solidTransferArm.FindFetchTarget(this.destination, this) : Game.Instance.fetchManager.FindFetchTarget(this.destination, this);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    Pickupable pickupable = (Pickupable) context.data;
    if ((UnityEngine.Object) pickupable == (UnityEngine.Object) null)
      pickupable = this.FindFetchTarget(context.consumerState);
    this.smi.sm.source.Set(pickupable.gameObject, this.smi, false);
    pickupable.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
    base.Begin(context);
  }

  protected override void End(string reason)
  {
    Pickupable pickupable = this.smi.sm.source.Get<Pickupable>(this.smi);
    if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
      pickupable.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
    base.End(reason);
  }

  private void OnTagsChanged(object _)
  {
    if (!((UnityEngine.Object) this.smi.sm.chunk.Get(this.smi) != (UnityEngine.Object) null))
      return;
    this.Fail("Tags changed");
  }

  public override void PrepareChore(ref Chore.Precondition.Context context)
  {
    context.chore = (Chore) new FetchAreaChore(context);
  }

  public float AmountWaitingToFetch()
  {
    return (UnityEngine.Object) this.fetcher == (UnityEngine.Object) null ? this.originalAmount : this.amount;
  }

  public static float GetMinimumFetchAmount(HashSet<Tag> match_tags)
  {
    float a = 1f;
    foreach (Tag matchTag in match_tags)
    {
      GameObject prefab = Assets.GetPrefab(matchTag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        PrimaryElement component = prefab.GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.MassPerUnit > 1.0)
          a = Mathf.Max(a, component.MassPerUnit);
      }
      else
      {
        foreach (GameObject gameObject in Assets.GetPrefabsWithTag(matchTag))
        {
          PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.MassPerUnit > 1.0)
            a = Mathf.Max(a, component.MassPerUnit);
        }
      }
    }
    return a;
  }

  public static float GetMinimumFetchAmount(Tag requested_tag, float requested_amount)
  {
    float a = requested_amount;
    GameObject prefab = Assets.TryGetPrefab(requested_tag);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      PrimaryElement component = prefab.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.MassPerUnit > 1.0)
        return Mathf.Max(a, component.MassPerUnit);
    }
    foreach (GameObject gameObject in Assets.GetPrefabsWithTag(requested_tag))
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.MassPerUnit > 1.0)
        a = Mathf.Max(a, component.MassPerUnit);
    }
    return a;
  }

  public FetchChore(
    ChoreType choreType,
    Storage destination,
    float amount,
    HashSet<Tag> tags,
    FetchChore.MatchCriteria criteria,
    Tag required_tag,
    Tag[] forbidden_tags = null,
    ChoreProvider chore_provider = null,
    bool run_until_complete = true,
    Action<Chore> on_complete = null,
    Action<Chore> on_begin = null,
    Action<Chore> on_end = null,
    Operational.State operational_requirement = Operational.State.Operational,
    int priority_mod = 0)
    : base(choreType, (IStateMachineTarget) destination, chore_provider, run_until_complete, on_complete, on_begin, on_end, priority_mod: priority_mod)
  {
    if (choreType == null)
      Debug.LogError((object) "You must specify a chore type for fetching!");
    this.tagsFirst = tags.Count > 0 ? tags.First<Tag>() : Tag.Invalid;
    if ((double) amount <= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
      DebugUtil.LogWarningArgs((object) $"Chore {choreType.Id} is requesting {this.tagsFirst} {amount} to {((UnityEngine.Object) destination != (UnityEngine.Object) null ? (object) destination.name : (object) "to nowhere")}");
    this.SetPrioritizable((UnityEngine.Object) destination.prioritizable != (UnityEngine.Object) null ? destination.prioritizable : destination.GetComponent<Prioritizable>());
    this.smi = new FetchChore.StatesInstance(this);
    double num = (double) this.smi.sm.requestedamount.Set(amount, this.smi);
    this.destination = destination;
    DebugUtil.DevAssert(criteria != FetchChore.MatchCriteria.MatchTags || tags.Count <= 1, "For performance reasons fetch chores are limited to one tag when matching tags!");
    this.tags = tags;
    this.criteria = criteria;
    this.tagsHash = FetchChore.ComputeHashCodeForTags((IEnumerable<Tag>) tags);
    this.requiredTag = required_tag;
    this.forbiddenTags = forbidden_tags != null ? forbidden_tags : new Tag[0];
    this.forbidHash = FetchChore.ComputeHashCodeForTags((IEnumerable<Tag>) this.forbiddenTags);
    DebugUtil.DevAssert(!tags.Contains(GameTags.Preserved), "Fetch chore fetching invalid tags.");
    if (destination.GetOnlyFetchMarkedItems())
    {
      DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!");
      this.requiredTag = GameTags.Garbage;
    }
    this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Work);
    this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, (object) destination);
    this.AddPrecondition(FetchChore.IsFetchTargetAvailable, (object) null);
    this.AddPrecondition(FetchChore.CanFetchDroneComplete, (object) destination.gameObject);
    Deconstructable component1 = this.target.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) component1);
    BuildingEnabledButton component2 = this.target.GetComponent<BuildingEnabledButton>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, (object) component2);
    if (operational_requirement != Operational.State.None)
    {
      Operational component3 = destination.GetComponent<Operational>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      {
        Chore.Precondition precondition = ChorePreconditions.instance.IsOperational;
        if (operational_requirement == Operational.State.Functional)
          precondition = ChorePreconditions.instance.IsFunctional;
        this.AddPrecondition(precondition, (object) component3);
      }
    }
    this.partitionerEntry = GameScenePartitioner.Instance.Add(destination.name, (object) this, Grid.PosToCell((KMonoBehaviour) destination), GameScenePartitioner.Instance.fetchChoreLayer, (Action<object>) null);
    this.onOnlyFetchMarkedItemsSettingChangedHandle = destination.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
    this.automatable = destination.GetComponent<Automatable>();
    if (!(bool) (UnityEngine.Object) this.automatable)
      return;
    this.AddPrecondition(ChorePreconditions.instance.IsAllowedByAutomation, (object) this.automatable);
  }

  private void OnOnlyFetchMarkedItemsSettingChanged(object data)
  {
    if (!((UnityEngine.Object) this.destination != (UnityEngine.Object) null))
      return;
    if (this.destination.GetOnlyFetchMarkedItems())
    {
      DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!");
      this.requiredTag = GameTags.Garbage;
    }
    else
      this.requiredTag = Tag.Invalid;
  }

  private void OnMasterPriorityChanged(
    PriorityScreen.PriorityClass priorityClass,
    int priority_value)
  {
    this.masterPriority.priority_class = priorityClass;
    this.masterPriority.priority_value = priority_value;
  }

  public override void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> incomplete_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
  }

  public void CollectChoresFromGlobalChoreProvider(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
    this.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded_contexts, (List<Chore.Precondition.Context>) null, failed_contexts, is_attempting_override);
  }

  public void CollectChoresFromGlobalChoreProvider(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded_contexts,
    List<Chore.Precondition.Context> incomplete_contexts,
    List<Chore.Precondition.Context> failed_contexts,
    bool is_attempting_override)
  {
    base.CollectChores(consumer_state, succeeded_contexts, incomplete_contexts, failed_contexts, is_attempting_override);
  }

  public override void Cleanup()
  {
    base.Cleanup();
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    if (!((UnityEngine.Object) this.destination != (UnityEngine.Object) null))
      return;
    this.destination.Unsubscribe(ref this.onOnlyFetchMarkedItemsSettingChangedHandle);
  }

  public static int ComputeHashCodeForTags(IEnumerable<Tag> tags)
  {
    int hashCodeForTags = 0;
    foreach (Tag tag in tags)
      hashCodeForTags ^= tag.GetHash();
    return hashCodeForTags;
  }

  public enum MatchCriteria
  {
    MatchID,
    MatchTags,
  }

  public class StatesInstance(FetchChore master) : 
    GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore>
  {
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter fetcher;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter source;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter chunk;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter requestedamount;
    public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter actualamount;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
    }
  }
}
