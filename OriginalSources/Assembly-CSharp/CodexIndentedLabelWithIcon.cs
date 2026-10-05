// Decompiled with JetBrains decompiler
// Type: CodexIndentedLabelWithIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CodexIndentedLabelWithIcon : CodexWidget<CodexIndentedLabelWithIcon>
{
  public CodexImage icon { get; set; }

  public CodexText label { get; set; }

  public string stringKey { get; set; } = "";

  public string batchedAnimPrefabSourceID { get; set; } = "";

  public string spriteName { get; set; } = "";

  public CodexIndentedLabelWithIcon()
  {
    this.icon = new CodexImage();
    this.label = new CodexText();
  }

  public CodexIndentedLabelWithIcon(
    string text,
    CodexTextStyle style,
    Tuple<Sprite, Color> coloredSprite)
  {
    this.icon = new CodexImage(coloredSprite);
    this.label = new CodexText(text, style);
  }

  public CodexIndentedLabelWithIcon(
    string text,
    CodexTextStyle style,
    Tuple<Sprite, Color> coloredSprite,
    int iconWidth,
    int iconHeight)
  {
    this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
    this.label = new CodexText(text, style);
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    if (!string.IsNullOrEmpty(this.stringKey))
      this.label.stringKey = this.stringKey;
    if (!string.IsNullOrEmpty(this.batchedAnimPrefabSourceID))
    {
      GameObject prefab = Assets.TryGetPrefab((Tag) this.batchedAnimPrefabSourceID);
      KBatchedAnimController component = (Object) prefab != (Object) null ? prefab.GetComponent<KBatchedAnimController>() : (KBatchedAnimController) null;
      KAnimFile animFile = (Object) component != (Object) null ? component.AnimFiles[0] : (KAnimFile) null;
      this.icon.sprite = (Object) animFile != (Object) null ? Def.GetUISpriteFromMultiObjectAnim(animFile) : (Sprite) null;
    }
    if (!string.IsNullOrEmpty(this.spriteName))
      this.icon.sprite = Assets.GetSprite((HashedString) this.spriteName);
    Image componentInChildren = contentGameObject.GetComponentInChildren<Image>();
    this.icon.ConfigureImage(componentInChildren);
    this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
    if (this.icon.preferredWidth == -1 || this.icon.preferredHeight == -1)
      return;
    LayoutElement component1 = componentInChildren.GetComponent<LayoutElement>();
    component1.minWidth = (float) this.icon.preferredHeight;
    component1.minHeight = (float) this.icon.preferredWidth;
    component1.preferredHeight = (float) this.icon.preferredHeight;
    component1.preferredWidth = (float) this.icon.preferredWidth;
  }
}
