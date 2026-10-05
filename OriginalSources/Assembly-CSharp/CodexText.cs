// Decompiled with JetBrains decompiler
// Type: CodexText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CodexText : CodexWidget<CodexText>
{
  public string text { get; set; }

  public string messageID { get; set; }

  public CodexTextStyle style { get; set; }

  public string stringKey
  {
    set => this.text = (string) Strings.Get(value);
    get => "--> " + (this.text ?? "NULL");
  }

  public CodexText() => this.style = CodexTextStyle.Body;

  public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body, string id = null)
  {
    this.text = text;
    this.style = style;
    if (id == null)
      return;
    this.messageID = id;
  }

  public void ConfigureLabel(
    LocText label,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    label.gameObject.SetActive(true);
    label.AllowLinks = this.style == CodexTextStyle.Body;
    label.textStyleSetting = textStyles[this.style];
    label.text = this.text;
    label.ApplySettings();
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
    this.ConfigurePreferredLayout(contentGameObject);
  }
}
