// Decompiled with JetBrains decompiler
// Type: ArtifactModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ArtifactModule : SingleEntityReceptacle, IRenderEveryTick, IHexCellCollector
{
  [MyCmpReq]
  private KBatchedAnimController animController;
  [MyCmpReq]
  private RocketModuleCluster module;
  private Clustercraft craft;

  protected override void OnSpawn()
  {
    this.craft = this.module.CraftInterface.GetComponent<Clustercraft>();
    if (this.craft.Status == Clustercraft.CraftStatus.InFlight && (UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
      this.occupyingObject.SetActive(false);
    base.OnSpawn();
    this.Subscribe(705820818, new Action<object>(this.OnEnterSpace));
    this.Subscribe(-1165815793, new Action<object>(this.OnExitSpace));
  }

  public void RenderEveryTick(float dt) => this.ArtifactTrackModulePosition();

  private void ArtifactTrackModulePosition()
  {
    this.occupyingObjectRelativePosition = this.animController.Offset + Vector3.up * 0.5f + new Vector3(0.0f, 0.0f, -1f);
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.PositionOccupyingObject();
  }

  private void OnEnterSpace(object data)
  {
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.occupyingObject.SetActive(false);
  }

  private void OnExitSpace(object data)
  {
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.occupyingObject.SetActive(true);
  }

  public bool CheckIsCollecting() => false;

  public string GetProperName() => this.GetComponent<RocketModuleCluster>().GetProperName();

  public Sprite GetUISprite()
  {
    return Def.GetUISprite((object) this.gameObject.GetComponent<KPrefabID>().PrefabID()).first;
  }

  public float GetCapacity() => 1f;

  public float GetMassStored() => (float) this.storage.items.Count;

  public float TimeInState() => 0.0f;

  public string GetCapacityBarText() => $"{this.GetMassStored()} / {this.GetCapacity()}";
}
