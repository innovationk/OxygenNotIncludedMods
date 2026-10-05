// Decompiled with JetBrains decompiler
// Type: ToggleGeothermalVentConnection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ToggleGeothermalVentConnection : Toggleable
{
  [MyCmpGet]
  private KBatchedAnimController buildingAnimController;
  private Facing workerFacing;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetWorkTime(10f);
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim(GeothermalVentConfig.TOGGLE_ANIM_OVERRIDE)
    };
    this.workAnims = new HashedString[1]
    {
      (HashedString) GeothermalVentConfig.TOGGLE_ANIMATION
    };
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
    this.workLayer = Grid.SceneLayer.Front;
    this.synchronizeAnims = false;
    this.workAnimPlayMode = KAnim.PlayMode.Once;
    this.SetOffsets(new CellOffset[1]{ CellOffset.none });
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.buildingAnimController.Play((HashedString) GeothermalVentConfig.TOGGLE_ANIMATION);
    if (!((Object) this.workerFacing == (Object) null) && !((Object) this.workerFacing.gameObject != (Object) worker.gameObject))
      return;
    this.workerFacing = worker.GetComponent<Facing>();
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if ((Object) this.workerFacing != (Object) null)
      this.workerFacing.Face(this.workerFacing.transform.GetLocalPosition().x + 0.5f);
    return base.OnWorkTick(worker, dt);
  }
}
