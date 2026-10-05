// Decompiled with JetBrains decompiler
// Type: Structure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Structure")]
public class Structure : KMonoBehaviour
{
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpReq]
  private Operational operational;
  public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);
  private bool isEntombed;
  private HandleVector<int>.Handle partitionerEntry;
  private static EventSystem.IntraObjectHandler<Structure> RocketLandedDelegate = new EventSystem.IntraObjectHandler<Structure>((Action<Structure, object>) ((cmp, data) => cmp.RocketChanged(data)));

  public bool IsEntombed() => this.isEntombed;

  public static bool IsBuildingEntombed(Building building)
  {
    if (!Grid.IsValidCell(Grid.PosToCell((KMonoBehaviour) building)))
      return false;
    for (int index = 0; index < building.PlacementCells.Length; ++index)
    {
      int placementCell = building.PlacementCells[index];
      if (Grid.Element[placementCell].IsSolid && !Grid.Foundation[placementCell])
        return true;
    }
    return false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Structure.OnSpawn", (object) this.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
    this.OnSolidChanged((object) null);
    this.Subscribe<Structure>(-887025858, Structure.RocketLandedDelegate);
  }

  public void UpdatePosition()
  {
    GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.building.GetExtents());
  }

  private void RocketChanged(object data) => this.OnSolidChanged(data);

  private void OnSolidChanged(object data)
  {
    bool flag = Structure.IsBuildingEntombed(this.building);
    if (flag == this.isEntombed)
      return;
    this.isEntombed = flag;
    if (this.isEntombed)
      this.GetComponent<KPrefabID>().AddTag(GameTags.Entombed);
    else
      this.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
    this.operational.SetFlag(Structure.notEntombedFlag, !this.isEntombed);
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, this.isEntombed, (object) this);
    this.Trigger(-1089732772, (object) BoxedBools.Box(this.isEntombed));
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }
}
