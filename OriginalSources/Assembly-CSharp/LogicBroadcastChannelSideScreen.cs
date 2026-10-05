// Decompiled with JetBrains decompiler
// Type: LogicBroadcastChannelSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LogicBroadcastChannelSideScreen : SideScreenContent
{
  private LogicBroadcastReceiver sensor;
  [SerializeField]
  private GameObject rowPrefab;
  [SerializeField]
  private GameObject listContainer;
  [SerializeField]
  private LocText headerLabel;
  [SerializeField]
  private GameObject noChannelRow;
  private Dictionary<LogicBroadcaster, GameObject> broadcasterRows = new Dictionary<LogicBroadcaster, GameObject>();
  private GameObject emptySpaceRow;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<LogicBroadcastReceiver>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.sensor = target.GetComponent<LogicBroadcastReceiver>();
    this.Build();
  }

  private void ClearRows()
  {
    if ((UnityEngine.Object) this.emptySpaceRow != (UnityEngine.Object) null)
      Util.KDestroyGameObject(this.emptySpaceRow);
    foreach (KeyValuePair<LogicBroadcaster, GameObject> broadcasterRow in this.broadcasterRows)
      Util.KDestroyGameObject(broadcasterRow.Value);
    this.broadcasterRows.Clear();
  }

  private void Build()
  {
    this.headerLabel.SetText((string) STRINGS.UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.HEADER);
    this.ClearRows();
    foreach (LogicBroadcaster logicBroadcaster in Components.LogicBroadcasters)
    {
      if (!logicBroadcaster.IsNullOrDestroyed())
      {
        GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer);
        gameObject.gameObject.name = logicBroadcaster.gameObject.GetProperName();
        Debug.Assert(!this.broadcasterRows.ContainsKey(logicBroadcaster), (object) ("Adding two of the same broadcaster to LogicBroadcastChannelSideScreen UI: " + logicBroadcaster.gameObject.GetProperName()));
        this.broadcasterRows.Add(logicBroadcaster, gameObject);
        gameObject.SetActive(true);
      }
    }
    this.noChannelRow.SetActive(Components.LogicBroadcasters.Count == 0);
    this.Refresh();
  }

  private void Refresh()
  {
    foreach (KeyValuePair<LogicBroadcaster, GameObject> broadcasterRow in this.broadcasterRows)
    {
      KeyValuePair<LogicBroadcaster, GameObject> kvp = broadcasterRow;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.gameObject.GetProperName());
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("DistanceLabel").SetText((string) (LogicBroadcastReceiver.CheckRange(this.sensor.gameObject, kvp.Key.gameObject) ? STRINGS.UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.IN_RANGE : STRINGS.UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.OUT_OF_RANGE));
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite((object) kvp.Key.gameObject).first;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite((object) kvp.Key.gameObject).second;
      WorldContainer myWorld = kvp.Key.GetMyWorld();
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").sprite = myWorld.IsModuleInterior ? Assets.GetSprite((HashedString) "icon_category_rocketry") : Def.GetUISprite((object) myWorld.GetComponent<ClusterGridEntity>()).first;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").color = myWorld.IsModuleInterior ? Color.white : Def.GetUISprite((object) myWorld.GetComponent<ClusterGridEntity>()).second;
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = (System.Action) (() =>
      {
        this.sensor.SetChannel(kvp.Key);
        this.Refresh();
      });
      kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState((UnityEngine.Object) this.sensor.GetChannel() == (UnityEngine.Object) kvp.Key ? 1 : 0);
    }
  }
}
