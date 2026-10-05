// Decompiled with JetBrains decompiler
// Type: Floodable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Floodable")]
public class Floodable : KMonoBehaviour
{
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpGet]
  private SimCellOccupier simCellOccupier;
  [MyCmpReq]
  private Operational operational;
  public static Operational.Flag notFloodedFlag = new Operational.Flag("not_flooded", Operational.Flag.Type.Functional);
  private bool isFlooded;
  private HandleVector<int>.Handle partitionerEntry;

  public bool IsFlooded => this.isFlooded;

  public BuildingDef Def => this.building.Def;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Floodable.OnSpawn", (object) this.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
    this.OnElementChanged((object) null);
  }

  private void OnElementChanged(object data)
  {
    bool flag = false;
    for (int index = 0; index < this.building.PlacementCells.Length; ++index)
    {
      if (Grid.IsSubstantialLiquid(this.building.PlacementCells[index]))
      {
        flag = true;
        break;
      }
    }
    if (flag == this.isFlooded)
      return;
    this.isFlooded = flag;
    this.operational.SetFlag(Floodable.notFloodedFlag, !this.isFlooded);
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Flooded, this.isFlooded, (object) this);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }
}
