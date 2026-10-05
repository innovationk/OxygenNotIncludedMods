// Decompiled with JetBrains decompiler
// Type: CodexWidget`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public abstract class CodexWidget<SubClass> : ICodexWidget, IHasDlcRestrictions
{
  public int preferredWidth { get; set; }

  public int preferredHeight { get; set; }

  protected CodexWidget()
  {
    this.preferredWidth = -1;
    this.preferredHeight = -1;
  }

  protected CodexWidget(int preferredWidth, int preferredHeight)
  {
    this.preferredWidth = preferredWidth;
    this.preferredHeight = preferredHeight;
  }

  public abstract void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

  protected void ConfigurePreferredLayout(GameObject contentGameObject)
  {
    LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
    componentInChildren.minWidth = (float) this.preferredWidth;
    componentInChildren.minHeight = (float) this.preferredHeight;
    componentInChildren.preferredHeight = (float) this.preferredHeight;
    componentInChildren.preferredWidth = (float) this.preferredWidth;
  }

  public string[] requiredAtLeastOneDlcIds { get; set; }

  public string[] requiredDlcIds { get; set; }

  public string[] forbiddenDlcIds { get; set; }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public string[] GetAnyRequiredDlcIds() => this.requiredAtLeastOneDlcIds;
}
