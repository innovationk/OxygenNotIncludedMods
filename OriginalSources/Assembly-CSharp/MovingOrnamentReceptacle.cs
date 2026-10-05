// Decompiled with JetBrains decompiler
// Type: MovingOrnamentReceptacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MovingOrnamentReceptacle : OrnamentReceptacle, ISim1000ms
{
  [MyCmpReq]
  private SnapOn snapOn;
  private Navigator navigator;
  private KPrefabID prefabID;
  private KBatchedAnimTracker occupyingTracker;
  private KAnimLink animLink;
  private CavityInfo lastCavity;

  protected override void OnPrefabInit()
  {
    this.prefabID = this.GetComponent<KPrefabID>();
    base.OnPrefabInit();
    this.Subscribe(144050788, new Action<object>(this.OnRoomUpdate));
    this.UpdateCavity();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "snapTo_ornament", false);
  }

  protected override void PositionOccupyingObject()
  {
    KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
    component.transform.SetLocalPosition(new Vector3(0.0f, 0.0f, -0.1f));
    this.occupyingTracker = this.occupyingObject.AddComponent<KBatchedAnimTracker>();
    this.occupyingTracker.symbol = new HashedString("snapTo_ornament");
    this.occupyingTracker.forceAlwaysVisible = true;
    this.animLink = new KAnimLink((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), (KAnimControllerBase) component);
  }

  protected override void ClearOccupant()
  {
    if ((UnityEngine.Object) this.occupyingTracker != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.occupyingTracker);
      this.occupyingTracker = (KBatchedAnimTracker) null;
    }
    if (this.animLink != null)
    {
      this.animLink.Unregister();
      this.animLink = (KAnimLink) null;
    }
    base.ClearOccupant();
  }

  public void Sim1000ms(float dt) => this.UpdateCavity();

  private void OnRoomUpdate(object roomInfo)
  {
    if (roomInfo != null)
      return;
    this.UpdateCavity();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.Unsubscribe(144050788, new Action<object>(this.OnRoomUpdate));
    this.UnregisterFromLastCavity();
  }

  public void UpdateCavity()
  {
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this.gameObject));
    if (this.lastCavity == cavityForCell)
      return;
    this.UnregisterFromLastCavity();
    if (cavityForCell != null)
    {
      cavityForCell.AddEntity(this.prefabID);
      Game.Instance.roomProber.UpdateRoom(cavityForCell);
    }
    this.lastCavity = cavityForCell;
  }

  private void UnregisterFromLastCavity()
  {
    if (this.lastCavity != null)
    {
      this.lastCavity.RemoveFromCavity(this.prefabID, this.lastCavity.otherEntities);
      Game.Instance.roomProber.UpdateRoom(this.lastCavity);
    }
    this.lastCavity = (CavityInfo) null;
  }
}
