// Decompiled with JetBrains decompiler
// Type: Assignable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class Assignable : KMonoBehaviour, ISaveLoadable
{
  public string slotID;
  private AssignableSlot _slot;
  public IAssignableIdentity assignee;
  [Serialize]
  protected Ref<KMonoBehaviour> assignee_identityRef = new Ref<KMonoBehaviour>();
  [Serialize]
  protected string assignee_slotInstanceID;
  [Serialize]
  private string assignee_groupID = "";
  public AssignableSlot[] subSlots;
  public bool canBePublic;
  [Serialize]
  private bool canBeAssigned = true;
  public int priority;
  private List<Func<MinionAssignablesProxy, bool>> autoassignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();
  private List<Func<MinionAssignablesProxy, bool>> assignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();
  public Func<Assignables, string> customAssignmentUITooltipFunc;
  public Func<Assignables, string> customAssignablesUITooltipFunc;

  public AssignableSlot slot
  {
    get
    {
      if (this._slot == null)
        this._slot = Db.Get().AssignableSlots.Get(this.slotID);
      return this._slot;
    }
  }

  public bool CanBeAssigned => this.canBeAssigned;

  public event Action<IAssignableIdentity> OnAssign;

  [System.Runtime.Serialization.OnDeserialized]
  internal void OnDeserialized()
  {
  }

  private void RestoreAssignee()
  {
    IAssignableIdentity savedAssignee = this.GetSavedAssignee();
    if (savedAssignee == null)
      return;
    AssignableSlotInstance savedSlotInstance = this.GetSavedSlotInstance(savedAssignee);
    this.Assign(savedAssignee, savedSlotInstance);
  }

  private AssignableSlotInstance GetSavedSlotInstance(IAssignableIdentity savedAsignee)
  {
    if (savedAsignee != null && savedAsignee is MinionIdentity || savedAsignee is StoredMinionIdentity || savedAsignee is MinionAssignablesProxy)
    {
      Ownables soleOwner = savedAsignee.GetSoleOwner();
      if ((UnityEngine.Object) soleOwner != (UnityEngine.Object) null)
      {
        AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
        if (slots != null)
        {
          AssignableSlotInstance first = slots.FindFirst<AssignableSlotInstance>((Func<AssignableSlotInstance, bool>) (i => i.ID == this.assignee_slotInstanceID));
          if (first != null)
            return first;
        }
      }
      Equipment component = soleOwner.GetComponent<Equipment>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        AssignableSlotInstance[] slots = component.GetSlots(this.slot);
        if (slots != null)
        {
          AssignableSlotInstance first = slots.FindFirst<AssignableSlotInstance>((Func<AssignableSlotInstance, bool>) (i => i.ID == this.assignee_slotInstanceID));
          if (first != null)
            return first;
        }
      }
    }
    return (AssignableSlotInstance) null;
  }

  private IAssignableIdentity GetSavedAssignee()
  {
    if ((UnityEngine.Object) this.assignee_identityRef.Get() != (UnityEngine.Object) null)
      return this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
    return !string.IsNullOrEmpty(this.assignee_groupID) ? (IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups[this.assignee_groupID] : (IAssignableIdentity) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RestoreAssignee();
    Components.AssignableItems.Add(this);
    Game.Instance.assignmentManager.Add(this);
    if (this.assignee == null && this.canBePublic)
      this.Assign((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups["public"]);
    this.assignmentPreconditions.Add((Func<MinionAssignablesProxy, bool>) (proxy =>
    {
      GameObject targetGameObject = proxy.GetTargetGameObject();
      return targetGameObject.GetComponent<KMonoBehaviour>().GetMyWorldId() == this.GetMyWorldId() || targetGameObject.IsMyParentWorld(this.gameObject);
    }));
    this.autoassignmentPreconditions.Add((Func<MinionAssignablesProxy, bool>) (proxy =>
    {
      Operational component = this.GetComponent<Operational>();
      return !((UnityEngine.Object) component != (UnityEngine.Object) null) || component.IsOperational;
    }));
  }

  protected override void OnCleanUp()
  {
    this.Unassign();
    Components.AssignableItems.Remove(this);
    Game.Instance.assignmentManager.Remove(this);
    base.OnCleanUp();
  }

  public bool CanAutoAssignTo(IAssignableIdentity identity)
  {
    MinionAssignablesProxy identity1 = identity as MinionAssignablesProxy;
    if ((UnityEngine.Object) identity1 == (UnityEngine.Object) null)
      return true;
    if (!this.CanAssignTo((IAssignableIdentity) identity1))
      return false;
    foreach (Func<MinionAssignablesProxy, bool> autoassignmentPrecondition in this.autoassignmentPreconditions)
    {
      if (!autoassignmentPrecondition(identity1))
        return false;
    }
    return true;
  }

  public bool CanAssignTo(IAssignableIdentity identity)
  {
    MinionAssignablesProxy assignablesProxy = identity as MinionAssignablesProxy;
    if ((UnityEngine.Object) assignablesProxy == (UnityEngine.Object) null)
      return true;
    foreach (Func<MinionAssignablesProxy, bool> assignmentPrecondition in this.assignmentPreconditions)
    {
      if (!assignmentPrecondition(assignablesProxy))
        return false;
    }
    return true;
  }

  public bool IsAssigned() => this.assignee != null;

  public bool IsAssignedTo(IAssignableIdentity identity)
  {
    Debug.Assert(identity != null, (object) "IsAssignedTo identity is null");
    Ownables soleOwner = identity.GetSoleOwner();
    Debug.Assert((UnityEngine.Object) soleOwner != (UnityEngine.Object) null, (object) "IsAssignedTo identity sole owner is null");
    if (this.assignee != null)
    {
      foreach (Ownables owner in this.assignee.GetOwners())
      {
        Debug.Assert((bool) (UnityEngine.Object) owner, (object) "Assignable owners list contained null");
        if ((UnityEngine.Object) owner.gameObject == (UnityEngine.Object) soleOwner.gameObject)
          return true;
      }
    }
    return false;
  }

  public virtual void Assign(IAssignableIdentity new_assignee)
  {
    this.Assign(new_assignee, (AssignableSlotInstance) null);
  }

  public virtual void Assign(
    IAssignableIdentity new_assignee,
    AssignableSlotInstance specificSlotInstance)
  {
    if (new_assignee == this.assignee)
      return;
    switch (new_assignee)
    {
      case KMonoBehaviour _:
        if (!this.CanAssignTo(new_assignee))
          return;
        this.assignee_identityRef.Set((KMonoBehaviour) new_assignee);
        this.assignee_groupID = "";
        break;
      case AssignmentGroup _:
        this.assignee_identityRef.Set((KMonoBehaviour) null);
        this.assignee_groupID = ((AssignmentGroup) new_assignee).id;
        break;
    }
    this.GetComponent<KPrefabID>().AddTag(GameTags.Assigned);
    this.assignee = new_assignee;
    this.assignee_slotInstanceID = (string) null;
    if (this.slot != null)
    {
      switch (new_assignee)
      {
        case MinionIdentity _:
        case StoredMinionIdentity _:
        case MinionAssignablesProxy _:
          if (specificSlotInstance == null)
          {
            Ownables soleOwner = new_assignee.GetSoleOwner();
            if ((UnityEngine.Object) soleOwner != (UnityEngine.Object) null)
            {
              AssignableSlotInstance slot = soleOwner.GetSlot(this.slot);
              if (slot != null)
              {
                this.assignee_slotInstanceID = slot.ID;
                slot.Assign(this);
              }
            }
            Equipment component = soleOwner.GetComponent<Equipment>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            {
              AssignableSlotInstance slot = component.GetSlot(this.slot);
              if (slot != null)
              {
                this.assignee_slotInstanceID = slot.ID;
                slot.Assign(this);
                break;
              }
              break;
            }
            break;
          }
          this.assignee_slotInstanceID = specificSlotInstance.ID;
          specificSlotInstance.Assign(this);
          break;
      }
    }
    if (this.OnAssign != null)
      this.OnAssign(new_assignee);
    this.Trigger(684616645, (object) new_assignee);
  }

  public virtual void Unassign()
  {
    if (this.assignee == null)
      return;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Assigned);
    if (this.slot != null)
    {
      Ownables soleOwner = this.assignee.GetSoleOwner();
      if ((bool) (UnityEngine.Object) soleOwner)
      {
        AssignableSlotInstance[] slots1 = soleOwner.GetSlots(this.slot);
        (slots1 == null ? (AssignableSlotInstance) null : slots1.FindFirst<AssignableSlotInstance>((Func<AssignableSlotInstance, bool>) (s => (UnityEngine.Object) s.assignable == (UnityEngine.Object) this)))?.Unassign();
        Equipment component = soleOwner.GetComponent<Equipment>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
          (slots2 == null ? (AssignableSlotInstance) null : slots2.FindFirst<AssignableSlotInstance>((Func<AssignableSlotInstance, bool>) (s => (UnityEngine.Object) s.assignable == (UnityEngine.Object) this)))?.Unassign();
        }
      }
    }
    this.assignee = (IAssignableIdentity) null;
    if (this.canBePublic)
      this.Assign((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups["public"]);
    this.assignee_slotInstanceID = (string) null;
    this.assignee_identityRef.Set((KMonoBehaviour) null);
    this.assignee_groupID = "";
    if (this.OnAssign != null)
      this.OnAssign((IAssignableIdentity) null);
    this.Trigger(684616645, (object) null);
  }

  public void SetCanBeAssigned(bool state) => this.canBeAssigned = state;

  public void AddAssignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
  {
    this.assignmentPreconditions.Add(precondition);
  }

  public void AddAutoassignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
  {
    this.autoassignmentPreconditions.Add(precondition);
  }

  public int GetNavigationCost(Navigator navigator)
  {
    int navigationCost1 = -1;
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    IApproachable component = this.GetComponent<IApproachable>();
    CellOffset[] cellOffsetArray = component != null ? component.GetOffsets() : new CellOffset[1];
    DebugUtil.DevAssert((UnityEngine.Object) navigator != (UnityEngine.Object) null, "Navigator is mysteriously null");
    if ((UnityEngine.Object) navigator == (UnityEngine.Object) null)
      return -1;
    foreach (CellOffset offset in cellOffsetArray)
    {
      int cell2 = Grid.OffsetCell(cell1, offset);
      int navigationCost2 = navigator.GetNavigationCost(cell2);
      if (navigationCost2 != -1 && (navigationCost1 == -1 || navigationCost2 < navigationCost1))
        navigationCost1 = navigationCost2;
    }
    return navigationCost1;
  }
}
