// Decompiled with JetBrains decompiler
// Type: AssignableSlotInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class AssignableSlotInstance
{
  public string ID;
  public AssignableSlot slot;
  public Assignable assignable;
  private bool unassigning;

  public Assignables assignables { get; private set; }

  public GameObject gameObject => this.assignables.gameObject;

  public AssignableSlotInstance(Assignables assignables, AssignableSlot slot)
    : this(slot.Id, assignables, slot)
  {
  }

  public AssignableSlotInstance(string id, Assignables assignables, AssignableSlot slot)
  {
    this.ID = id;
    this.slot = slot;
    this.assignables = assignables;
  }

  public void Assign(Assignable assignable)
  {
    if ((Object) this.assignable == (Object) assignable)
      return;
    this.Unassign(false);
    this.assignable = assignable;
    this.assignables.Trigger(-1585839766, (object) this);
  }

  public virtual void Unassign(bool trigger_event = true)
  {
    if (this.unassigning || !this.IsAssigned())
      return;
    this.unassigning = true;
    this.assignable.Unassign();
    if (trigger_event)
      this.assignables.Trigger(-1585839766, (object) this);
    this.assignable = (Assignable) null;
    this.unassigning = false;
  }

  public bool IsAssigned() => (Object) this.assignable != (Object) null;

  public bool IsUnassigning() => this.unassigning;
}
