// Decompiled with JetBrains decompiler
// Type: ContentContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization.Converters;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ContentContainer : IHasDlcRestrictions
{
  public GameObject go;

  public ContentContainer() => this.content = new List<ICodexWidget>();

  public ContentContainer(List<ICodexWidget> content, ContentContainer.ContentLayout contentLayout)
  {
    this.content = content;
    this.contentLayout = contentLayout;
  }

  public List<ICodexWidget> content { get; set; }

  public string lockID { get; set; }

  public string[] requiredDlcIds { get; set; }

  public string[] forbiddenDlcIds { get; set; }

  [StringEnumConverter]
  public ContentContainer.ContentLayout contentLayout { get; set; }

  public bool showBeforeGeneratedContent { get; set; }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public enum ContentLayout
  {
    Vertical,
    Horizontal,
    Grid,
    GridTwoColumn,
    GridTwoColumnTall,
  }
}
