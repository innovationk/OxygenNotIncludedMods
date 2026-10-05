// Decompiled with JetBrains decompiler
// Type: Pump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Pump")]
public class Pump : KMonoBehaviour, ISim1000ms
{
  public static readonly Operational.Flag PumpableFlag = new Operational.Flag("vent", Operational.Flag.Type.Requirement);
  [MyCmpReq]
  private Operational operational;
  [MyCmpGet]
  private KSelectable selectable;
  [MyCmpGet]
  private ElementConsumer consumer;
  [MyCmpGet]
  private ConduitDispenser dispenser;
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private KBatchedAnimController controller;
  private const float OperationalUpdateInterval = 1f;
  private float elapsedTime;
  private bool pumpable;
  private Guid conduitBlockedStatusGuid;
  private Guid noElementStatusGuid;
  private SimHashes lastElementConsumed = SimHashes.Vacuum;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.consumer.EnableConsumption(false);
    this.consumer.OnElementConsumedStored += new Action<Sim.ConsumedMassInfo>(this.OnElementConsumedStored);
  }

  private void OnElementConsumedStored(Sim.ConsumedMassInfo elementConsumedInfo)
  {
    if (this.dispenser.conduitType != ConduitType.Liquid)
      return;
    Element element = ElementLoader.elements[(int) elementConsumedInfo.removedElemIdx];
    if (this.lastElementConsumed != element.id && element.id != SimHashes.Vacuum)
      GameUtil.TintLiquidSymbolOnBuilding("water", this.controller, element);
    this.lastElementConsumed = element.id;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.elapsedTime = 0.0f;
    this.pumpable = this.UpdateOperational();
    this.dispenser.GetConduitManager().AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.LastPostUpdate);
  }

  protected override void OnCleanUp()
  {
    this.dispenser.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.OnConduitUpdate));
    base.OnCleanUp();
  }

  public void Sim1000ms(float dt)
  {
    this.elapsedTime += dt;
    if ((double) this.elapsedTime >= 1.0)
    {
      this.pumpable = this.UpdateOperational();
      this.elapsedTime = 0.0f;
    }
    if (this.operational.IsOperational && this.pumpable)
      this.operational.SetActive(true);
    else
      this.operational.SetActive(false);
  }

  private bool UpdateOperational()
  {
    Element.State expected_state = Element.State.Vacuum;
    switch (this.dispenser.conduitType)
    {
      case ConduitType.Gas:
        expected_state = Element.State.Gas;
        break;
      case ConduitType.Liquid:
        expected_state = Element.State.Liquid;
        break;
    }
    bool flag = this.IsPumpable(expected_state, (int) this.consumer.consumptionRadius);
    this.noElementStatusGuid = this.selectable.ToggleStatusItem(expected_state == Element.State.Gas ? Db.Get().BuildingStatusItems.NoGasElementToPump : Db.Get().BuildingStatusItems.NoLiquidElementToPump, this.noElementStatusGuid, !flag);
    this.operational.SetFlag(Pump.PumpableFlag, !this.storage.IsFull() & flag);
    return flag;
  }

  private bool IsPumpable(Element.State expected_state, int radius)
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    for (int index1 = 0; index1 < (int) this.consumer.consumptionRadius; ++index1)
    {
      for (int index2 = 0; index2 < (int) this.consumer.consumptionRadius; ++index2)
      {
        int index3 = cell + index2 + Grid.WidthInCells * index1;
        if (Grid.Element[index3].IsState(expected_state))
          return true;
      }
    }
    return false;
  }

  private void OnConduitUpdate(float dt)
  {
    this.conduitBlockedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.ConduitBlocked, this.conduitBlockedStatusGuid, this.dispenser.blocked);
  }

  public ConduitType conduitType => this.dispenser.conduitType;
}
