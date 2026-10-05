// Decompiled with JetBrains decompiler
// Type: FoodRehydrator.AccessabilityManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace FoodRehydrator;

public class AccessabilityManager : KMonoBehaviour
{
  [MyCmpReq]
  private Operational operational;
  private GameObject reserver;
  private Workable activeWorkable;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.FoodRehydrators.Add(this.gameObject);
    this.Subscribe(824508782, new Action<object>(this.ActiveChangedHandler));
  }

  protected override void OnCleanUp()
  {
    Components.FoodRehydrators.Remove(this.gameObject);
    base.OnCleanUp();
  }

  public void Reserve(GameObject reserver)
  {
    this.reserver = reserver;
    Debug.Assert((UnityEngine.Object) reserver != (UnityEngine.Object) null && (UnityEngine.Object) reserver.GetComponent<MinionResume>() != (UnityEngine.Object) null);
  }

  public void Unreserve()
  {
    this.activeWorkable = (Workable) null;
    this.reserver = (GameObject) null;
  }

  public void SetActiveWorkable(Workable work)
  {
    DebugUtil.DevAssert((UnityEngine.Object) this.activeWorkable == (UnityEngine.Object) null || (UnityEngine.Object) work == (UnityEngine.Object) null, "FoodRehydrator::AccessabilityManager activating a second workable");
    this.activeWorkable = work;
    this.operational.SetActive((UnityEngine.Object) this.activeWorkable != (UnityEngine.Object) null);
  }

  public bool CanAccess(GameObject worker)
  {
    if (!this.operational.IsOperational)
      return false;
    return (UnityEngine.Object) this.reserver == (UnityEngine.Object) null || (UnityEngine.Object) this.reserver == (UnityEngine.Object) worker;
  }

  protected void ActiveChangedHandler(object obj)
  {
    if (this.operational.IsActive)
      return;
    this.CancelActiveWorkable();
  }

  public void CancelActiveWorkable()
  {
    if (!((UnityEngine.Object) this.activeWorkable != (UnityEngine.Object) null))
      return;
    this.activeWorkable.StopWork(this.activeWorkable.worker, true);
  }
}
