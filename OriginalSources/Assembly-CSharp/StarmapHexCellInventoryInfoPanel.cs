// Decompiled with JetBrains decompiler
// Type: StarmapHexCellInventoryInfoPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class StarmapHexCellInventoryInfoPanel(SimpleInfoScreen simpleInfoScreen) : SimpleInfoPanel(simpleInfoScreen)
{
  private Dictionary<Tag, GameObject> itemRows = new Dictionary<Tag, GameObject>();

  public override void Refresh(CollapsibleDetailContentPanel panel, GameObject selectedTarget)
  {
    StarmapHexCellInventory hexCellInventory;
    if (!this.IsValidTarget(selectedTarget, out hexCellInventory))
    {
      panel.gameObject.SetActive(false);
    }
    else
    {
      panel.SetTitle((string) STRINGS.UI.CLUSTERMAP.HEXCELL_INVENTORY.UI_PANEL.TITLE);
      this.RefreshElements(panel, hexCellInventory);
      panel.gameObject.SetActive(true);
    }
  }

  private void RefreshElements(
    CollapsibleDetailContentPanel panel,
    StarmapHexCellInventory hexCellInventory)
  {
    foreach (KeyValuePair<Tag, GameObject> itemRow in this.itemRows)
    {
      if ((UnityEngine.Object) itemRow.Value != (UnityEngine.Object) null)
        itemRow.Value.SetActive(false);
    }
    if ((UnityEngine.Object) hexCellInventory == (UnityEngine.Object) null)
      return;
    List<StarmapHexCellInventory.SerializedItem> serializedItemList = new List<StarmapHexCellInventory.SerializedItem>((IEnumerable<StarmapHexCellInventory.SerializedItem>) hexCellInventory.Items);
    serializedItemList.Sort((Comparison<StarmapHexCellInventory.SerializedItem>) ((a, b) => b.Mass.CompareTo(a.Mass)));
    foreach (StarmapHexCellInventory.SerializedItem serializedItem in serializedItemList)
    {
      Tag id = serializedItem.ID;
      GameObject gameObject;
      if (!this.itemRows.TryGetValue(id, out gameObject))
      {
        gameObject = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, panel.Content.gameObject, true);
        this.itemRows.Add(id, gameObject);
      }
      gameObject.SetActive(true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) id);
      component.GetReference<Image>("Icon").sprite = uiSprite.first;
      component.GetReference<Image>("Icon").color = uiSprite.second;
      component.GetReference<LocText>("NameLabel").text = serializedItem.IsEntity ? serializedItem.ID.ProperName() : ElementLoader.GetElement(id).name;
      component.GetReference<LocText>("ValueLabel").text = serializedItem.IsEntity ? GameUtil.GetFormattedUnits(serializedItem.Mass) : GameUtil.GetFormattedMass(serializedItem.Mass);
      component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
    }
  }

  public bool IsValidTarget(GameObject go, out StarmapHexCellInventory hexCellInventory)
  {
    hexCellInventory = (StarmapHexCellInventory) null;
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return false;
    hexCellInventory = go.GetComponent<StarmapHexCellInventory>();
    return (UnityEngine.Object) hexCellInventory != (UnityEngine.Object) null;
  }
}
