// Decompiled with JetBrains decompiler
// Type: ChorePreconditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChorePreconditions
{
  private static ChorePreconditions _instance;
  public Chore.Precondition IsPreemptable;
  public Chore.Precondition HasUrge;
  public Chore.Precondition IsValid;
  public Chore.Precondition IsPermitted;
  public Chore.Precondition IsAssignedtoMe;
  public Chore.Precondition IsInMyRoom;
  public Chore.Precondition IsPreferredAssignable;
  public Chore.Precondition IsPreferredAssignableOrUrgentBladder;
  public Chore.Precondition IsNotTransferArm;
  public Chore.Precondition HasSkillPerk;
  public Chore.Precondition IsMinion;
  public Chore.Precondition IsMoreSatisfyingEarly;
  public Chore.Precondition IsMoreSatisfyingLate;
  public Chore.Precondition IsChattable;
  public Chore.Precondition IsNotRedAlert;
  public Chore.Precondition IsScheduledTime;
  public Chore.Precondition CanMoveTo;
  public Chore.Precondition CanMoveToCell;
  public Chore.Precondition CanMoveToDynamicCell;
  public Chore.Precondition CanMoveToDynamicCellUntilBegun;
  public Chore.Precondition CanPickup;
  public Chore.Precondition IsAwake;
  public Chore.Precondition IsStanding;
  public Chore.Precondition IsMoving;
  public Chore.Precondition IsOffLadder;
  public Chore.Precondition NotInTube;
  public Chore.Precondition ConsumerHasTrait;
  public Chore.Precondition IsOperational;
  public Chore.Precondition IsNotMarkedForDeconstruction;
  public Chore.Precondition IsNotMarkedForDisable;
  public Chore.Precondition IsFunctional;
  public Chore.Precondition IsOverrideTargetNullOrMe;
  public Chore.Precondition NotChoreCreator;
  public Chore.Precondition IsGettingMoreStressed;
  public Chore.Precondition IsAllowedByAutomation;
  public Chore.Precondition HasTag;
  public Chore.Precondition DoesntHaveTag;
  public Chore.Precondition CheckBehaviourPrecondition;
  public Chore.Precondition CanDoWorkerPrioritizable;
  public Chore.Precondition IsExclusivelyAvailableWithOtherChores;
  public Chore.Precondition IsBladderFull;
  public Chore.Precondition IsBladderNotFull;
  public Chore.Precondition NoDeadBodies;
  public Chore.Precondition IsNotARobot;
  public Chore.Precondition IsNotABionic;
  public Chore.Precondition IsBionic;
  public Chore.Precondition NotCurrentlyPeeing;
  public Chore.Precondition IsRocketTravelling;

  public static ChorePreconditions instance
  {
    get
    {
      if (ChorePreconditions._instance == null)
        ChorePreconditions._instance = new ChorePreconditions();
      return ChorePreconditions._instance;
    }
  }

  public static void DestroyInstance() => ChorePreconditions._instance = (ChorePreconditions) null;

  public ChorePreconditions()
  {
    Chore.Precondition precondition = new Chore.Precondition();
    precondition.id = nameof (IsPreemptable);
    precondition.sortOrder = 1;
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_PREEMPTABLE;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.isAttemptingOverride || context.chore.CanPreempt(context) || (UnityEngine.Object) context.chore.driver == (UnityEngine.Object) null);
    precondition.canExecuteOnAnyThread = false;
    this.IsPreemptable = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (HasUrge);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_URGE;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.chore.choreType.urge == null)
        return true;
      foreach (Urge urge in context.consumerState.consumer.GetUrges())
      {
        if (context.chore.SatisfiesUrge(urge))
          return true;
      }
      return false;
    });
    precondition.canExecuteOnAnyThread = true;
    this.HasUrge = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsValid);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_VALID;
    precondition.sortOrder = -4;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !context.chore.isNull && context.chore.IsValid());
    precondition.canExecuteOnAnyThread = false;
    this.IsValid = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsPermitted);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_PERMITTED;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.consumer.IsPermittedOrEnabled(context.choreTypeForPermission, context.chore));
    precondition.canExecuteOnAnyThread = true;
    this.IsPermitted = precondition;
    precondition = new Chore.Precondition();
    precondition.id = "IsAssignedToMe";
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_ASSIGNED_TO_ME;
    precondition.sortOrder = 10;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Assignable assignable = (Assignable) data;
      IAssignableIdentity component = context.consumerState.gameObject.GetComponent<IAssignableIdentity>();
      return component != null && assignable.IsAssignedTo(component);
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsAssignedtoMe = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsInMyRoom);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_IN_MY_ROOM;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      CavityInfo cavityForCell1 = Game.Instance.roomProber.GetCavityForCell((int) data);
      Room room1 = (Room) null;
      if (cavityForCell1 != null)
        room1 = cavityForCell1.room;
      if (room1 != null)
      {
        if ((UnityEngine.Object) context.consumerState.ownable != (UnityEngine.Object) null)
        {
          foreach (Component owner in room1.GetOwners())
          {
            if ((UnityEngine.Object) owner.gameObject == (UnityEngine.Object) context.consumerState.gameObject)
              return true;
          }
        }
        else
        {
          Room room2 = (Room) null;
          if (context.chore is FetchChore chore2 && (UnityEngine.Object) chore2.destination != (UnityEngine.Object) null)
          {
            CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((KMonoBehaviour) chore2.destination));
            if (cavityForCell2 != null)
              room2 = cavityForCell2.room;
            return room2 != null && room2 == room1;
          }
          if (!(context.chore is WorkChore<Tinkerable>))
            return false;
          CavityInfo cavityForCell3 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((context.chore as WorkChore<Tinkerable>).gameObject));
          if (cavityForCell3 != null)
            room2 = cavityForCell3.room;
          return room2 != null && room2 == room1;
        }
      }
      return false;
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsInMyRoom = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsPreferredAssignable);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_PREFERRED_ASSIGNABLE;
    precondition.sortOrder = 10;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Assignable assignable = (Assignable) data;
      Navigator component;
      context.chore.gameObject.TryGetComponent<Navigator>(out component);
      return Game.Instance.assignmentManager.GetPreferredAssignables(context.consumerState.assignables, component, assignable.slot).Contains(assignable);
    });
    precondition.canExecuteOnAnyThread = true;
    this.IsPreferredAssignable = precondition;
    precondition = new Chore.Precondition();
    precondition.id = "IsPreferredAssignableOrUrgent";
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_PREFERRED_ASSIGNABLE_OR_URGENT_BLADDER;
    precondition.sortOrder = 10;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Assignable candidate = (Assignable) data;
      if (Game.Instance.assignmentManager.IsPreferredAssignable(context.consumerState.assignables, candidate))
        return true;
      PeeChoreMonitor.Instance smi1 = context.consumerState.gameObject.GetSMI<PeeChoreMonitor.Instance>();
      if (smi1 != null)
        return smi1.IsInsideState((StateMachine.BaseState) smi1.sm.critical);
      GunkMonitor.Instance smi2 = context.consumerState.gameObject.GetSMI<GunkMonitor.Instance>();
      return smi2 != null && GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold(smi2);
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsPreferredAssignableOrUrgentBladder = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsNotTransferArm);
    precondition.description = "";
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !context.consumerState.hasSolidTransferArm);
    precondition.canExecuteOnAnyThread = true;
    this.IsNotTransferArm = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (HasSkillPerk);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_SKILL_PERK;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      MinionResume resume = context.consumerState.resume;
      if (!(bool) (UnityEngine.Object) resume)
        return false;
      switch (data)
      {
        case SkillPerk _:
          SkillPerk perk = data as SkillPerk;
          return resume.HasPerk(perk);
        case HashedString perkId3:
          return resume.HasPerk(perkId3);
        case string _:
          HashedString perkId2 = (HashedString) (string) data;
          return resume.HasPerk(perkId2);
        default:
          return false;
      }
    });
    precondition.canExecuteOnAnyThread = true;
    this.HasSkillPerk = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsMinion);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MINION;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (UnityEngine.Object) context.consumerState.resume != (UnityEngine.Object) null);
    precondition.canExecuteOnAnyThread = true;
    this.IsMinion = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsMoreSatisfyingEarly);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MORE_SATISFYING;
    precondition.sortOrder = -2;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.isAttemptingOverride || context.skipMoreSatisfyingEarlyPrecondition || context.consumerState.selectable.IsSelected)
        return true;
      Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
      if (currentChore == null)
        return true;
      if (context.masterPriority.priority_class != currentChore.masterPriority.priority_class)
        return context.masterPriority.priority_class > currentChore.masterPriority.priority_class;
      if ((UnityEngine.Object) context.consumerState.consumer != (UnityEngine.Object) null && context.personalPriority != context.consumerState.consumer.GetPersonalPriority(currentChore.choreType))
        return context.personalPriority > context.consumerState.consumer.GetPersonalPriority(currentChore.choreType);
      return context.masterPriority.priority_value != currentChore.masterPriority.priority_value ? context.masterPriority.priority_value > currentChore.masterPriority.priority_value : context.priority > currentChore.choreType.priority;
    });
    precondition.canExecuteOnAnyThread = true;
    this.IsMoreSatisfyingEarly = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsMoreSatisfyingLate);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MORE_SATISFYING;
    precondition.sortOrder = 10000;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.isAttemptingOverride || !context.consumerState.selectable.IsSelected && !context.skipMoreSatisfyingEarlyPrecondition)
        return true;
      Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
      if (currentChore == null)
        return true;
      if (context.masterPriority.priority_class != currentChore.masterPriority.priority_class)
        return context.masterPriority.priority_class > currentChore.masterPriority.priority_class;
      if ((UnityEngine.Object) context.consumerState.consumer != (UnityEngine.Object) null && context.personalPriority != context.consumerState.consumer.GetPersonalPriority(currentChore.choreType))
        return context.personalPriority > context.consumerState.consumer.GetPersonalPriority(currentChore.choreType);
      return context.masterPriority.priority_value != currentChore.masterPriority.priority_value ? context.masterPriority.priority_value > currentChore.masterPriority.priority_value : context.priority > currentChore.choreType.priority;
    });
    precondition.canExecuteOnAnyThread = true;
    this.IsMoreSatisfyingLate = precondition;
    precondition = new Chore.Precondition();
    precondition.id = "CanChat";
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_CHAT;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      KMonoBehaviour cmp = (KMonoBehaviour) data;
      return !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !((UnityEngine.Object) context.consumerState.navigator == (UnityEngine.Object) null) && !((UnityEngine.Object) cmp == (UnityEngine.Object) null) && context.consumerState.navigator.CanReach(Grid.PosToCell(cmp));
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsChattable = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsNotRedAlert);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_RED_ALERT;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority || !context.chore.gameObject.GetMyWorld().IsRedAlert());
    precondition.canExecuteOnAnyThread = false;
    this.IsNotRedAlert = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsScheduledTime);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_SCHEDULED_TIME;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if (context.chore.gameObject.GetMyWorld().IsRedAlert())
        return true;
      ScheduleBlockType type = (ScheduleBlockType) data;
      ScheduleBlock scheduleBlock = context.consumerState.scheduleBlock;
      return scheduleBlock == null || scheduleBlock.IsAllowed(type);
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsScheduledTime = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (CanMoveTo);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
        return false;
      KMonoBehaviour kmonoBehaviour = (KMonoBehaviour) data;
      if ((UnityEngine.Object) kmonoBehaviour == (UnityEngine.Object) null)
        return false;
      IApproachable approachable = (IApproachable) kmonoBehaviour;
      int cost;
      if (!context.consumerState.consumer.GetNavigationCost(approachable, out cost))
        return false;
      context.cost += cost;
      return true;
    });
    precondition.canExecuteOnAnyThread = false;
    this.CanMoveTo = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (CanMoveToCell);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
        return false;
      int cell = (int) data;
      int cost;
      if (!Grid.IsValidCell(cell) || !context.consumerState.consumer.GetNavigationCost(cell, out cost))
        return false;
      context.cost += cost;
      return true;
    });
    precondition.canExecuteOnAnyThread = true;
    this.CanMoveToCell = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (CanMoveToDynamicCell);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
        return false;
      Func<int> func = (Func<int>) data;
      if (func == null)
        return false;
      int cell = func();
      int cost;
      if (!Grid.IsValidCell(cell) || !context.consumerState.consumer.GetNavigationCost(cell, out cost))
        return false;
      context.cost += cost;
      return true;
    });
    precondition.canExecuteOnAnyThread = false;
    this.CanMoveToDynamicCell = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (CanMoveToDynamicCellUntilBegun);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
        return false;
      if (context.chore.InProgress())
        return true;
      Func<int> func = (Func<int>) data;
      if (func == null)
        return false;
      int cell = func();
      int cost;
      if (!Grid.IsValidCell(cell) || !context.consumerState.consumer.GetNavigationCost(cell, out cost))
        return false;
      context.cost += cost;
      return true;
    });
    precondition.canExecuteOnAnyThread = false;
    this.CanMoveToDynamicCellUntilBegun = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (CanPickup);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_PICKUP;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Pickupable pickupable = (Pickupable) data;
      return !((UnityEngine.Object) pickupable == (UnityEngine.Object) null) && !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && pickupable.CouldBePickedUpByMinion(context.consumerState.prefabid.InstanceID) && context.consumerState.consumer.CanReach((IApproachable) pickupable);
    });
    precondition.canExecuteOnAnyThread = false;
    this.CanPickup = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsAwake);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_AWAKE;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
        return false;
      StaminaMonitor.Instance smi = context.consumerState.consumer.GetSMI<StaminaMonitor.Instance>();
      return smi == null || !smi.IsInsideState((StateMachine.BaseState) smi.sm.sleepy.sleeping);
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsAwake = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsStanding);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_STANDING;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !((UnityEngine.Object) context.consumerState.navigator == (UnityEngine.Object) null) && context.consumerState.navigator.CurrentNavType == NavType.Floor);
    precondition.canExecuteOnAnyThread = true;
    this.IsStanding = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsMoving);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MOVING;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !((UnityEngine.Object) context.consumerState.navigator == (UnityEngine.Object) null) && context.consumerState.navigator.IsMoving());
    precondition.canExecuteOnAnyThread = true;
    this.IsMoving = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsOffLadder);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_OFF_LADDER;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !((UnityEngine.Object) context.consumerState.navigator == (UnityEngine.Object) null) && context.consumerState.navigator.CurrentNavType != NavType.Ladder && context.consumerState.navigator.CurrentNavType != NavType.Pole);
    precondition.canExecuteOnAnyThread = true;
    this.IsOffLadder = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (NotInTube);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.NOT_IN_TUBE;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !((UnityEngine.Object) context.consumerState.navigator == (UnityEngine.Object) null) && context.consumerState.navigator.CurrentNavType != NavType.Tube);
    precondition.canExecuteOnAnyThread = true;
    this.NotInTube = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (ConsumerHasTrait);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.HAS_TRAIT;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      string trait_id = (string) data;
      Traits traits = context.consumerState.traits;
      return !((UnityEngine.Object) traits == (UnityEngine.Object) null) && traits.HasTrait(trait_id);
    });
    precondition.canExecuteOnAnyThread = true;
    this.ConsumerHasTrait = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsOperational);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_OPERATIONAL;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as Operational).IsOperational);
    precondition.canExecuteOnAnyThread = true;
    this.IsOperational = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsNotMarkedForDeconstruction);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Deconstructable deconstructable = data as Deconstructable;
      return (UnityEngine.Object) deconstructable == (UnityEngine.Object) null || !deconstructable.IsMarkedForDeconstruction();
    });
    precondition.canExecuteOnAnyThread = true;
    this.IsNotMarkedForDeconstruction = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsNotMarkedForDisable);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DISABLE;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      BuildingEnabledButton buildingEnabledButton = data as BuildingEnabledButton;
      if ((UnityEngine.Object) buildingEnabledButton == (UnityEngine.Object) null)
        return true;
      return buildingEnabledButton.IsEnabled && !buildingEnabledButton.WaitingForDisable;
    });
    precondition.canExecuteOnAnyThread = true;
    this.IsNotMarkedForDisable = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsFunctional);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_FUNCTIONAL;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (data as Operational).IsFunctional);
    precondition.canExecuteOnAnyThread = true;
    this.IsFunctional = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsOverrideTargetNullOrMe);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_OVERRIDE_TARGET_NULL_OR_ME;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.isAttemptingOverride || (UnityEngine.Object) context.chore.overrideTarget == (UnityEngine.Object) null || (UnityEngine.Object) context.chore.overrideTarget == (UnityEngine.Object) context.consumerState.consumer);
    precondition.canExecuteOnAnyThread = true;
    this.IsOverrideTargetNullOrMe = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (NotChoreCreator);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.NOT_CHORE_CREATOR;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      GameObject gameObject = (GameObject) data;
      return !((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null) && !((UnityEngine.Object) context.consumerState.gameObject == (UnityEngine.Object) gameObject);
    });
    precondition.canExecuteOnAnyThread = false;
    this.NotChoreCreator = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsGettingMoreStressed);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_GETTING_MORE_STRESSED;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => (double) Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).GetDelta() > 0.0);
    precondition.canExecuteOnAnyThread = false;
    this.IsGettingMoreStressed = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsAllowedByAutomation);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_ALLOWED_BY_AUTOMATION;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => ((Automatable) data).AllowedByAutomation(context.consumerState.hasSolidTransferArm));
    precondition.canExecuteOnAnyThread = true;
    this.IsAllowedByAutomation = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (HasTag);
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Tag tag = (Tag) data;
      return context.consumerState.prefabid.HasTag(tag);
    });
    precondition.canExecuteOnAnyThread = true;
    this.HasTag = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (DoesntHaveTag);
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Tag tag = (Tag) data;
      return !context.consumerState.prefabid.HasTag(tag);
    });
    precondition.canExecuteOnAnyThread = true;
    this.DoesntHaveTag = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (CheckBehaviourPrecondition);
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Tag tag = (Tag) data;
      return context.consumerState.consumer.RunBehaviourPrecondition(tag);
    });
    precondition.canExecuteOnAnyThread = false;
    this.CheckBehaviourPrecondition = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (CanDoWorkerPrioritizable);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CAN_DO_RECREATION;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null || !(data is IWorkerPrioritizable workerPrioritizable2))
        return false;
      int priority = 0;
      if (!workerPrioritizable2.GetWorkerPriority(context.consumerState.worker, out priority))
        return false;
      context.consumerPriority += priority;
      return true;
    });
    precondition.canExecuteOnAnyThread = false;
    this.CanDoWorkerPrioritizable = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsExclusivelyAvailableWithOtherChores);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.EXCLUSIVELY_AVAILABLE;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      foreach (Chore chore3 in (List<Chore>) data)
      {
        if (chore3 != context.chore && (UnityEngine.Object) chore3.driver != (UnityEngine.Object) null)
          return false;
      }
      return true;
    });
    precondition.canExecuteOnAnyThread = true;
    this.IsExclusivelyAvailableWithOtherChores = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsBladderFull);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.BLADDER_FULL;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      BladderMonitor.Instance smi3 = context.consumerState.gameObject.GetSMI<BladderMonitor.Instance>();
      if (smi3 != null && smi3.NeedsToPee())
        return true;
      GunkMonitor.Instance smi4 = context.consumerState.gameObject.GetSMI<GunkMonitor.Instance>();
      return smi4 != null && GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold(smi4);
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsBladderFull = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsBladderNotFull);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.BLADDER_NOT_FULL;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      BladderMonitor.Instance smi5 = context.consumerState.gameObject.GetSMI<BladderMonitor.Instance>();
      if (smi5 != null && smi5.NeedsToPee())
        return false;
      GunkMonitor.Instance smi6 = context.consumerState.gameObject.GetSMI<GunkMonitor.Instance>();
      return smi6 == null || !GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold(smi6);
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsBladderNotFull = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (NoDeadBodies);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.NO_DEAD_BODIES;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => Components.LiveMinionIdentities.Count == Components.MinionIdentities.Count);
    precondition.canExecuteOnAnyThread = true;
    this.NoDeadBodies = precondition;
    precondition = new Chore.Precondition();
    precondition.id = "NoRobots";
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.NOT_A_ROBOT;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object exempt_robot) =>
    {
      Tag tag = (Tag) (exempt_robot as string);
      return (UnityEngine.Object) context.consumerState.resume != (UnityEngine.Object) null || context.consumerState.prefabid.PrefabTag == tag;
    });
    precondition.canExecuteOnAnyThread = true;
    this.IsNotARobot = precondition;
    precondition = new Chore.Precondition();
    precondition.id = "NoBionic";
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.NOT_A_BIONIC;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.PrefabTag != (Tag) BionicMinionConfig.ID);
    precondition.canExecuteOnAnyThread = true;
    this.IsNotABionic = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsBionic);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_A_BIONIC;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => context.consumerState.prefabid.PrefabTag == (Tag) BionicMinionConfig.ID);
    precondition.canExecuteOnAnyThread = true;
    this.IsBionic = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (NotCurrentlyPeeing);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.CURRENTLY_PEEING;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      bool flag = true;
      Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
      if (currentChore != null)
      {
        string id = currentChore.choreType.Id;
        flag = id != Db.Get().ChoreTypes.BreakPee.Id && id != Db.Get().ChoreTypes.Pee.Id && id != Db.Get().ChoreTypes.ExpellGunk.Id;
      }
      return flag;
    });
    precondition.canExecuteOnAnyThread = true;
    this.NotCurrentlyPeeing = precondition;
    precondition = new Chore.Precondition();
    precondition.id = nameof (IsRocketTravelling);
    precondition.description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_ROCKET_TRAVELLING;
    precondition.fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Clustercraft component = ClusterManager.Instance.GetWorld(context.chore.gameObject.GetMyWorldId()).GetComponent<Clustercraft>();
      return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.IsTravellingAndFueled();
    });
    precondition.canExecuteOnAnyThread = false;
    this.IsRocketTravelling = precondition;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }
}
