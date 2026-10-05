// Decompiled with JetBrains decompiler
// Type: CodexCollapsibleHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CodexCollapsibleHeader : CodexWidget<CodexCollapsibleHeader>
{
  protected ContentContainer contents;
  private string label;
  private GameObject contentsGameObject;

  protected GameObject ContentsGameObject
  {
    get
    {
      if ((UnityEngine.Object) this.contentsGameObject == (UnityEngine.Object) null)
        this.contentsGameObject = this.contents.go;
      return this.contentsGameObject;
    }
    set => this.contentsGameObject = value;
  }

  public CodexCollapsibleHeader(string label, ContentContainer contents)
  {
    this.label = label;
    this.contents = contents;
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
    LocText reference1 = component.GetReference<LocText>("Label");
    reference1.text = this.label;
    reference1.textStyleSetting = textStyles[CodexTextStyle.Subtitle];
    reference1.ApplySettings();
    MultiToggle reference2 = component.GetReference<MultiToggle>("ExpandToggle");
    reference2.ChangeState(1);
    reference2.onClick = (System.Action) (() => this.ToggleCategoryOpen(contentGameObject, !this.ContentsGameObject.activeSelf));
  }

  private void ToggleCategoryOpen(GameObject header, bool open)
  {
    header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
    this.ContentsGameObject.SetActive(open);
  }
}
