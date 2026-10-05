// Decompiled with JetBrains decompiler
// Type: CommandModuleWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/CommandModuleWorkable")]
public class CommandModuleWorkable : Workable
{
  private static CellOffset[] entryOffsets = new CellOffset[5]
  {
    new CellOffset(0, 0),
    new CellOffset(0, 1),
    new CellOffset(0, 2),
    new CellOffset(0, 3),
    new CellOffset(0, 4)
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsets(CommandModuleWorkable.entryOffsets);
    this.synchronizeAnims = false;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_incubator_kanim")
    };
    this.SetWorkTime(float.PositiveInfinity);
    this.showProgressBar = false;
  }

  protected override void OnStartWork(WorkerBase worker) => base.OnStartWork(worker);

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if (!((Object) worker != (Object) null))
      return base.OnWorkTick(worker, dt);
    if (DlcManager.IsExpansion1Active())
    {
      GameObject gameObject = worker.gameObject;
      this.CompleteWork(worker);
      this.GetComponent<ClustercraftExteriorDoor>().FerryMinion(gameObject);
      return true;
    }
    GameObject gameObject1 = worker.gameObject;
    this.CompleteWork(worker);
    this.GetComponent<MinionStorage>().SerializeMinion(gameObject1);
    return true;
  }

  protected override void OnStopWork(WorkerBase worker) => base.OnStopWork(worker);

  protected override void OnCompleteWork(WorkerBase worker)
  {
  }
}
