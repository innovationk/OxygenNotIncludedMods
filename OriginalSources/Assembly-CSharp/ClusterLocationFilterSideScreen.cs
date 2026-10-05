// Decompiled with JetBrains decompiler
// Type: ClusterLocationFilterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ClusterLocationFilterSideScreen : SideScreenContent
{
  private LogicClusterLocationSensor sensor;
  [SerializeField]
  private GameObject rowPrefab;
  [SerializeField]
  private GameObject listContainer;
  [SerializeField]
  private LocText headerLabel;
  private Dictionary<AxialI, GameObject> worldRows = new Dictionary<AxialI, GameObject>();
  private GameObject emptySpaceRow;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<LogicClusterLocationSensor>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.sensor = target.GetComponent<LogicClusterLocationSensor>();
    this.Build();
  }

  private void ClearRows()
  {
    if ((UnityEngine.Object) this.emptySpaceRow != (UnityEngine.Object) null)
      Util.KDestroyGameObject(this.emptySpaceRow);
    foreach (KeyValuePair<AxialI, GameObject> worldRow in this.worldRows)
      Util.KDestroyGameObject(worldRow.Value);
    this.worldRows.Clear();
  }

  private void Build()
  {
    this.headerLabel.SetText((string) STRINGS.UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.HEADER);
    this.ClearRows();
    this.emptySpaceRow = Util.KInstantiateUI(this.rowPrefab, this.listContainer);
    this.emptySpaceRow.SetActive(true);
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (!worldContainer.IsModuleInterior)
      {
        GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer);
        gameObject.gameObject.name = worldContainer.GetProperName();
        AxialI myWorldLocation = worldContainer.GetMyWorldLocation();
        Debug.Assert(!this.worldRows.ContainsKey(myWorldLocation), (object) ("Adding two worlds/POI with the same cluster location to ClusterLocationFilterSideScreen UI: " + worldContainer.GetProperName()));
        this.worldRows.Add(myWorldLocation, gameObject);
      }
    }
    this.Refresh();
  }

  private void Refresh()
  {
    this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText((string) STRINGS.UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.EMPTY_SPACE_ROW);
    this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite((object) "hex_soft").first;
    this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Color.black;
    this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = (System.Action) (() =>
    {
      this.sensor.SetSpaceEnabled(!this.sensor.ActiveInSpace);
      this.Refresh();
    });
    this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.ActiveInSpace ? 1 : 0);
    foreach (KeyValuePair<AxialI, GameObject> worldRow in this.worldRows)
    {
      KeyValuePair<AxialI, GameObject> kvp = worldRow;
      ClusterGridEntity cmp = ClusterGrid.Instance.cellContents[kvp.Key][0];
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(cmp.GetProperName());
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite((object) cmp).first;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite((object) cmp).second;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = (System.Action) (() =>
      {
        this.sensor.SetLocationEnabled(kvp.Key, !this.sensor.CheckLocationSelected(kvp.Key));
        this.Refresh();
      });
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.CheckLocationSelected(kvp.Key) ? 1 : 0);
      kvp.Value.SetActive(ClusterGrid.Instance.GetCellRevealLevel(kvp.Key) == ClusterRevealLevel.Visible);
    }
  }
}
