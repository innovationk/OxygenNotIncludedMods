// Decompiled with JetBrains decompiler
// Type: DispenserSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DispenserSideScreen : SideScreenContent
{
  [SerializeField]
  private KButton dispenseButton;
  [SerializeField]
  private RectTransform itemRowContainer;
  [SerializeField]
  private GameObject itemRowPrefab;
  private IDispenser targetDispenser;
  private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<IDispenser>() != null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetDispenser = target.GetComponent<IDispenser>();
    this.Refresh();
  }

  private void Refresh()
  {
    this.dispenseButton.ClearOnClick();
    foreach (KeyValuePair<Tag, GameObject> row in this.rows)
      UnityEngine.Object.Destroy((UnityEngine.Object) row.Value);
    this.rows.Clear();
    foreach (Tag dispensedItem in this.targetDispenser.DispensedItems())
    {
      GameObject gameObject = Util.KInstantiateUI(this.itemRowPrefab, this.itemRowContainer.gameObject, true);
      this.rows.Add(dispensedItem, gameObject);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<Image>("Icon").sprite = Def.GetUISprite((object) dispensedItem).first;
      component.GetReference<LocText>("Label").text = Assets.GetPrefab(dispensedItem).GetProperName();
      gameObject.GetComponent<MultiToggle>().ChangeState(dispensedItem == this.targetDispenser.SelectedItem() ? 0 : 1);
    }
    if (this.targetDispenser.HasOpenChore())
    {
      this.dispenseButton.onClick += (System.Action) (() =>
      {
        this.targetDispenser.OnCancelDispense();
        this.Refresh();
      });
      this.dispenseButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_CANCEL;
    }
    else
    {
      this.dispenseButton.onClick += (System.Action) (() =>
      {
        this.targetDispenser.OnOrderDispense();
        this.Refresh();
      });
      this.dispenseButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_DISPENSE;
    }
    this.targetDispenser.OnStopWorkEvent -= new System.Action(this.Refresh);
    this.targetDispenser.OnStopWorkEvent += new System.Action(this.Refresh);
  }

  private void SelectTag(Tag tag)
  {
    this.targetDispenser.SelectItem(tag);
    this.Refresh();
  }
}
