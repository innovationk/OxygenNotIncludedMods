// Decompiled with JetBrains decompiler
// Type: KleiInventoryUISubcategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KleiInventoryUISubcategory : KMonoBehaviour
{
  [SerializeField]
  private GameObject dummyPrefab;
  public string subcategoryID;
  public GridLayoutGroup gridLayout;
  public List<GameObject> dummyItems;
  [SerializeField]
  private LayoutElement headerLayout;
  [SerializeField]
  private Image icon;
  [SerializeField]
  private LocText label;
  [SerializeField]
  private MultiToggle expandButton;
  private bool stateExpanded = true;

  public bool IsOpen => this.stateExpanded;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.expandButton.onClick = (System.Action) (() => this.ToggleOpen(!this.stateExpanded));
  }

  public void SetIdentity(string label, Sprite icon)
  {
    this.label.SetText(label);
    this.icon.sprite = icon;
  }

  public void RefreshDisplay()
  {
    foreach (GameObject dummyItem in this.dummyItems)
      dummyItem.SetActive(false);
    int num1 = 0;
    for (int index = 0; index < this.gridLayout.transform.childCount; ++index)
    {
      if (this.gridLayout.transform.GetChild(index).gameObject.activeSelf)
        ++num1;
    }
    this.gameObject.SetActive(num1 != 0);
    int num2 = 0;
    int num3 = num1 % this.gridLayout.constraintCount;
    if (num3 > 0)
      num2 = this.gridLayout.constraintCount - num3;
    while (num2 > this.dummyItems.Count)
      this.dummyItems.Add(Util.KInstantiateUI(this.dummyPrefab, this.gridLayout.gameObject));
    for (int index = 0; index < num2; ++index)
    {
      this.dummyItems[index].SetActive(true);
      this.dummyItems[index].transform.SetAsLastSibling();
    }
    this.headerLayout.minWidth = this.transform.parent.rectTransform().rect.width - 8f;
  }

  public void ToggleOpen(bool open)
  {
    this.gridLayout.gameObject.SetActive(open);
    this.stateExpanded = open;
    this.expandButton.ChangeState(this.stateExpanded ? 1 : 0);
  }
}
