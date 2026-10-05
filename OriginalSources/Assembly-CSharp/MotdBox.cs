// Decompiled with JetBrains decompiler
// Type: MotdBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MotdBox : KMonoBehaviour
{
  [SerializeField]
  private GameObject pageCarouselContainer;
  [SerializeField]
  private GameObject pageCarouselButtonPrefab;
  [SerializeField]
  private RawImage image;
  [SerializeField]
  private LocText headerLabel;
  [SerializeField]
  private LocText imageLabel;
  [SerializeField]
  private URLOpenFunction urlOpener;
  private int selectedPage;
  private GameObject[] pageButtons;
  private MotdBox.PageData[] pageDatas;

  public void Config(MotdBox.PageData[] data)
  {
    this.pageDatas = data;
    if (this.pageButtons != null)
    {
      for (int index = this.pageButtons.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.pageButtons[index]);
      this.pageButtons = (GameObject[]) null;
    }
    this.pageButtons = new GameObject[data.Length];
    for (int index = 0; index < this.pageButtons.Length; ++index)
    {
      int idx = index;
      GameObject gameObject = Util.KInstantiateUI(this.pageCarouselButtonPrefab, this.pageCarouselContainer);
      gameObject.SetActive(true);
      this.pageButtons[index] = gameObject;
      gameObject.GetComponent<MultiToggle>().onClick += (System.Action) (() => this.SwitchPage(idx));
    }
    this.SwitchPage(0);
  }

  private void SwitchPage(int newPage)
  {
    this.selectedPage = newPage;
    for (int index = 0; index < this.pageButtons.Length; ++index)
      this.pageButtons[index].GetComponent<MultiToggle>().ChangeState(index == this.selectedPage ? 1 : 0);
    this.image.texture = (Texture) this.pageDatas[newPage].Texture;
    this.headerLabel.SetText(this.pageDatas[newPage].HeaderText);
    this.urlOpener.SetURL(this.pageDatas[newPage].URL);
    if (string.IsNullOrEmpty(this.pageDatas[newPage].ImageText))
    {
      this.imageLabel.gameObject.SetActive(false);
      this.imageLabel.SetText("");
    }
    else
    {
      this.imageLabel.gameObject.SetActive(true);
      this.imageLabel.SetText(this.pageDatas[newPage].ImageText);
    }
  }

  public class PageData
  {
    public Texture2D Texture { get; set; }

    public string HeaderText { get; set; }

    public string ImageText { get; set; }

    public string URL { get; set; }
  }
}
