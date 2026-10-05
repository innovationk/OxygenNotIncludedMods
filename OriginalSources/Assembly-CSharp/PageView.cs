// Decompiled with JetBrains decompiler
// Type: PageView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/PageView")]
public class PageView : KMonoBehaviour
{
  [SerializeField]
  private MultiToggle nextButton;
  [SerializeField]
  private MultiToggle prevButton;
  [SerializeField]
  private LocText pageLabel;
  [SerializeField]
  private int childrenPerPage = 8;
  private int currentPage;
  private int oldChildCount;
  public Action<int> OnChangePage;

  public int ChildrenPerPage => this.childrenPerPage;

  private void Update()
  {
    if (this.oldChildCount == this.transform.childCount)
      return;
    this.oldChildCount = this.transform.childCount;
    this.RefreshPage();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.nextButton.onClick += (System.Action) (() =>
    {
      this.currentPage = (this.currentPage + 1) % this.pageCount;
      if (this.OnChangePage != null)
        this.OnChangePage(this.currentPage);
      this.RefreshPage();
    });
    this.prevButton.onClick += (System.Action) (() =>
    {
      --this.currentPage;
      if (this.currentPage < 0)
        this.currentPage += this.pageCount;
      if (this.OnChangePage != null)
        this.OnChangePage(this.currentPage);
      this.RefreshPage();
    });
  }

  private int pageCount
  {
    get
    {
      int pageCount = this.transform.childCount / this.childrenPerPage;
      if (this.transform.childCount % this.childrenPerPage != 0)
        ++pageCount;
      return pageCount;
    }
  }

  private void RefreshPage()
  {
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      if (index < this.currentPage * this.childrenPerPage)
        this.transform.GetChild(index).gameObject.SetActive(false);
      else if (index >= this.currentPage * this.childrenPerPage + this.childrenPerPage)
        this.transform.GetChild(index).gameObject.SetActive(false);
      else
        this.transform.GetChild(index).gameObject.SetActive(true);
    }
    LocText pageLabel = this.pageLabel;
    int num = this.currentPage % this.pageCount + 1;
    string str1 = num.ToString();
    num = this.pageCount;
    string str2 = num.ToString();
    string sourceText = $"{str1}/{str2}";
    pageLabel.SetText(sourceText);
  }
}
