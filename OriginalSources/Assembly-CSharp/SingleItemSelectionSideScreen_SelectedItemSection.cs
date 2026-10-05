// Decompiled with JetBrains decompiler
// Type: SingleItemSelectionSideScreen_SelectedItemSection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class SingleItemSelectionSideScreen_SelectedItemSection : KMonoBehaviour
{
  [Header("References")]
  [SerializeField]
  private LocText title;
  [SerializeField]
  private LocText contentText;
  [SerializeField]
  private KImage image;

  public Tag Item { private set; get; }

  public void Clear() => this.SetItem((Tag) (string) null);

  public void SetItem(Tag item)
  {
    this.Item = item;
    if (this.Item != GameTags.Void)
    {
      this.SetTitleText((string) UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.TITLE);
      this.SetContentText(this.Item.ProperName());
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) this.Item);
      this.SetImage(uiSprite.first, uiSprite.second);
    }
    else
    {
      this.SetTitleText((string) UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_TITLE);
      this.SetContentText((string) UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_MESSAGE);
      this.SetImage((Sprite) null, Color.white);
    }
  }

  private void SetTitleText(string text) => this.title.text = text;

  private void SetContentText(string text) => this.contentText.text = text;

  private void SetImage(Sprite sprite, Color color)
  {
    this.image.sprite = sprite;
    this.image.color = color;
    this.image.gameObject.SetActive((Object) sprite != (Object) null);
  }
}
