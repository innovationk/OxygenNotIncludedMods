// Decompiled with JetBrains decompiler
// Type: WorkableReactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WorkableReactable : Reactable
{
  protected Workable workable;
  private WorkerBase worker;
  public WorkableReactable.AllowedDirection allowedDirection;

  public WorkableReactable(
    Workable workable,
    HashedString id,
    ChoreType chore_type,
    WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any)
    : base(workable.gameObject, id, chore_type, 1, 1)
  {
    this.workable = workable;
    this.allowedDirection = allowed_direction;
  }

  public override bool InternalCanBegin(
    GameObject new_reactor,
    Navigator.ActiveTransition transition)
  {
    if ((Object) this.workable == (Object) null || (Object) this.reactor != (Object) null)
      return false;
    Brain component1 = new_reactor.GetComponent<Brain>();
    if ((Object) component1 == (Object) null || !component1.IsRunning())
      return false;
    Navigator component2 = new_reactor.GetComponent<Navigator>();
    if ((Object) component2 == (Object) null || !component2.IsMoving())
      return false;
    if (this.allowedDirection == WorkableReactable.AllowedDirection.Any)
      return true;
    Facing component3 = new_reactor.GetComponent<Facing>();
    if ((Object) component3 == (Object) null)
      return false;
    bool facing = component3.GetFacing();
    return (!facing || this.allowedDirection != WorkableReactable.AllowedDirection.Right) && (facing || this.allowedDirection != WorkableReactable.AllowedDirection.Left);
  }

  protected override void InternalBegin()
  {
    this.worker = this.reactor.GetComponent<WorkerBase>();
    this.worker.StartWork(new WorkerBase.StartWorkInfo(this.workable));
  }

  public override void Update(float dt)
  {
    if ((Object) this.worker.GetWorkable() == (Object) null)
    {
      this.End();
    }
    else
    {
      if (this.worker.Work(dt) == WorkerBase.WorkResult.InProgress)
        return;
      this.End();
    }
  }

  protected override void InternalEnd()
  {
    if (!((Object) this.worker != (Object) null))
      return;
    this.worker.StopWork();
  }

  protected override void InternalCleanup()
  {
  }

  public enum AllowedDirection
  {
    Any,
    Left,
    Right,
  }
}
