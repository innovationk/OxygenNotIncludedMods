// Decompiled with JetBrains decompiler
// Type: ZoneTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ZoneTile")]
public class ZoneTile : KMonoBehaviour
{
  [MyCmpReq]
  public Building building;
  private bool wasReplaced;
  private static readonly EventSystem.IntraObjectHandler<ZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<ZoneTile>((Action<ZoneTile, object>) ((component, data) => component.OnObjectReplaced(data)));

  protected override void OnSpawn()
  {
    foreach (int placementCell in this.building.PlacementCells)
      SimMessages.ModifyCellWorldZone(placementCell, (byte) 0);
    this.Subscribe<ZoneTile>(1606648047, ZoneTile.OnObjectReplacedDelegate);
  }

  protected override void OnCleanUp()
  {
    if (this.wasReplaced)
      return;
    this.ClearZone();
  }

  private void OnObjectReplaced(object data)
  {
    this.ClearZone();
    this.wasReplaced = true;
  }

  private void ClearZone()
  {
    foreach (int placementCell in this.building.PlacementCells)
    {
      GameObject gameObject;
      if (!Grid.ObjectLayers[(int) this.building.Def.ObjectLayer].TryGetValue(placementCell, out gameObject) || !((UnityEngine.Object) gameObject != (UnityEngine.Object) this.gameObject) || !((UnityEngine.Object) gameObject != (UnityEngine.Object) null) || !((UnityEngine.Object) gameObject.GetComponent<ZoneTile>() != (UnityEngine.Object) null))
      {
        SubWorld.ZoneType subWorldZoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(placementCell);
        byte zone_id = subWorldZoneType == SubWorld.ZoneType.Space ? byte.MaxValue : (byte) subWorldZoneType;
        SimMessages.ModifyCellWorldZone(placementCell, zone_id);
      }
    }
  }
}
