// Decompiled with JetBrains decompiler
// Type: SingleItemSelectionRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SingleItemSelectionRow : KMonoBehaviour
{
  [SerializeField]
  protected Image icon;
  [SerializeField]
  protected LocText labelText;
  [SerializeField]
  protected Image BG;
  [SerializeField]
  protected Image outline;
  [SerializeField]
  protected Color outlineHighLightColor = (Color) new Color32((byte) 168, (byte) 74, (byte) 121, byte.MaxValue);
  [SerializeField]
  protected Color BGHighLightColor = (Color) new Color32((byte) 168, (byte) 74, (byte) 121, (byte) 80 /*0x50*/);
  [SerializeField]
  protected Color outlineDefaultColor = (Color) new Color32((byte) 204, (byte) 204, (byte) 204, byte.MaxValue);
  protected Color regularColor = Color.white;
  [SerializeField]
  public KButton button;
  public Action<SingleItemSelectionRow> Clicked;

  public virtual string InvalidTagTitle
  {
    get => (string) STRINGS.UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.NO_SELECTION;
  }

  public Tag InvalidTag { get; protected set; } = GameTags.Void;

  public Tag tag { get; protected set; }

  public bool IsVisible => this.gameObject.activeSelf;

  public bool IsSelected { get; protected set; }

  protected override void OnPrefabInit()
  {
    this.regularColor = this.outline.color;
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!((UnityEngine.Object) this.button != (UnityEngine.Object) null))
      return;
    this.button.onPointerEnter += (System.Action) (() =>
    {
      if (this.IsSelected)
        return;
      this.outline.color = this.outlineHighLightColor;
    });
    this.button.onPointerExit += (System.Action) (() =>
    {
      if (this.IsSelected)
        return;
      this.outline.color = this.regularColor;
    });
    this.button.onClick += new System.Action(this.OnItemClicked);
  }

  public virtual void SetVisibleState(bool isVisible) => this.gameObject.SetActive(isVisible);

  protected virtual void OnItemClicked()
  {
    Action<SingleItemSelectionRow> clicked = this.Clicked;
    if (clicked == null)
      return;
    clicked(this);
  }

  public virtual void SetTag(Tag tag)
  {
    this.tag = tag;
    this.SetText(tag == this.InvalidTag ? this.InvalidTagTitle : tag.ProperName());
    if (tag != this.InvalidTag)
    {
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) tag);
      this.SetIcon(uiSprite.first, uiSprite.second);
    }
    else
      this.SetIcon((Sprite) null, Color.white);
  }

  protected virtual void SetText(string assignmentStr)
  {
    this.labelText.text = !string.IsNullOrEmpty(assignmentStr) ? assignmentStr : "-";
  }

  public virtual void SetSelected(bool selected)
  {
    this.IsSelected = selected;
    this.outline.color = selected ? this.outlineHighLightColor : this.outlineDefaultColor;
    this.BG.color = selected ? this.BGHighLightColor : Color.white;
  }

  protected virtual void SetIcon(Sprite sprite, Color color)
  {
    this.icon.sprite = sprite;
    this.icon.color = color;
    this.icon.gameObject.SetActive((UnityEngine.Object) sprite != (UnityEngine.Object) null);
  }
}
