// Decompiled with JetBrains decompiler
// Type: OrnamentReceptacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class OrnamentReceptacle : SingleEntityReceptacle
{
  protected StatusItem ornamentDisabledStatusItem;
  protected StatusItem noItemDisplayedStatusItem;
  private bool refreshAnims;

  public bool IsHoldingOrnament
  {
    get => (UnityEngine.Object) this.Occupant != (UnityEngine.Object) null && this.Occupant.HasTag(GameTags.Ornament);
  }

  public bool IsOperational
  {
    get => (UnityEngine.Object) this.operational == (UnityEngine.Object) null || this.operational.IsOperational;
  }

  protected override void OnPrefabInit()
  {
    this.ornamentDisabledStatusItem = Db.Get().BuildingStatusItems.OrnamentDisabled;
    this.noItemDisplayedStatusItem = Db.Get().BuildingStatusItems.PedestalNoItemDisplayed;
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection.Any);
    this.AddAdditionalCriteria((Func<GameObject, bool>) (obj => obj.HasTag(GameTags.PedestalDisplayable)));
    if ((UnityEngine.Object) this.occupyingObject == (UnityEngine.Object) null && (double) this.storage.MassStored() > 0.0)
      this.OnDepositObject(this.storage.items[0]);
    else
      this.RefreshDecorTag();
  }

  protected override void ClearOccupant()
  {
    base.ClearOccupant();
    this.RefreshDecorTag();
    Game.Instance.roomProber.TriggerBuildingChangedEvent(Grid.PosToCell(this.gameObject), (object) this.gameObject);
  }

  protected override void OnDepositObject(GameObject depositedObject)
  {
    base.OnDepositObject(depositedObject);
    this.RefreshDecorTag();
    Game.Instance.roomProber.TriggerBuildingChangedEvent(Grid.PosToCell(this.gameObject), (object) this.gameObject);
  }

  protected override void OnOperationalChanged(object data)
  {
    base.OnOperationalChanged(data);
    this.RefreshDecorTag();
    Game.Instance.roomProber.TriggerBuildingChangedEvent(Grid.PosToCell(this.gameObject), (object) this.gameObject);
    this.UpdateStatusItem();
  }

  protected override void PositionOccupyingObject()
  {
    base.PositionOccupyingObject();
    this.refreshAnims = true;
  }

  public override void Render1000ms(float dt)
  {
    base.Render1000ms(dt);
    if (!this.refreshAnims)
      return;
    if ((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null)
    {
      KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
      component.enabled = false;
      component.enabled = true;
    }
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    component1.enabled = false;
    component1.enabled = true;
    this.refreshAnims = false;
  }

  protected override void UpdateStatusItem(KSelectable selectable)
  {
    base.UpdateStatusItem(selectable);
    if ((!((UnityEngine.Object) this.operational != (UnityEngine.Object) null) || !this.IsHoldingOrnament ? 0 : (!this.operational.IsOperational ? 1 : 0)) != 0)
      selectable.AddStatusItem(this.ornamentDisabledStatusItem);
    else
      selectable.RemoveStatusItem(this.ornamentDisabledStatusItem);
    if ((UnityEngine.Object) this.Occupant == (UnityEngine.Object) null && ((UnityEngine.Object) this.operational == (UnityEngine.Object) null || this.operational.IsOperational))
      selectable.AddStatusItem(this.noItemDisplayedStatusItem);
    else
      selectable.RemoveStatusItem(this.noItemDisplayedStatusItem);
  }

  public virtual void RefreshDecorTag()
  {
    KPrefabID component = this.gameObject.GetComponent<KPrefabID>();
    int num1 = component.HasTag(GameTags.Decoration) ? 1 : 0;
    bool flag = (UnityEngine.Object) this.Occupant != (UnityEngine.Object) null && ((UnityEngine.Object) this.operational == (UnityEngine.Object) null || this.operational.IsOperational);
    if (flag)
      component.AddTag(GameTags.Decoration);
    else
      component.RemoveTag(GameTags.Decoration);
    int num2 = flag ? 1 : 0;
    if (num1 == num2)
      return;
    Game.Instance.roomProber.TriggerBuildingChangedEvent(Grid.PosToCell(this.gameObject), (object) component);
  }
}
