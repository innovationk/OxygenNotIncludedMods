// Decompiled with JetBrains decompiler
// Type: CodexEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CodexEntry : IHasDlcRestrictions
{
  public EntryDevLog log = new EntryDevLog();
  private List<ContentContainer> _contentContainers = new List<ContentContainer>();
  private string _id;
  private string _parentId;
  private string _category;
  private string _title;
  private string _name;
  private string _subtitle;
  private List<SubEntry> _subEntries = new List<SubEntry>();
  private List<CodexEntry_MadeAndUsed> _contentMadeAndUsed = new List<CodexEntry_MadeAndUsed>();
  private Sprite _icon;
  private Color _iconColor = Color.white;
  private string _iconPrefabID;
  private string _iconLockID;
  private string _iconAssetName;
  private bool _disabled;
  private bool _searchOnly;
  private int _customContentLength;
  private string _sortString;
  private bool _showBeforeGeneratedCategoryLinks;
  private bool _insertMergeContentAtBottom;

  public CodexEntry()
  {
  }

  public CodexEntry(string category, List<ContentContainer> contentContainers, string name)
  {
    this.category = category;
    this.name = name;
    this.contentContainers = contentContainers;
    if (!string.IsNullOrEmpty(this.sortString))
      return;
    this.sortString = UI.StripLinkFormatting(name);
  }

  public CodexEntry(string category, string titleKey, List<ContentContainer> contentContainers)
  {
    this.category = category;
    this.title = titleKey;
    this.contentContainers = contentContainers;
    if (!string.IsNullOrEmpty(this.sortString))
      return;
    this.sortString = UI.StripLinkFormatting(this.title);
  }

  public List<ContentContainer> contentContainers
  {
    get => this._contentContainers;
    private set => this._contentContainers = value;
  }

  public static List<string> ContentContainerDebug(List<ContentContainer> _contentContainers)
  {
    List<string> stringList = new List<string>();
    foreach (ContentContainer contentContainer in _contentContainers)
    {
      if (contentContainer != null)
      {
        string str = $"<b>{contentContainer.contentLayout.ToString()} container: {(contentContainer.content == null ? 0 : contentContainer.content.Count).ToString()} items</b>";
        if (contentContainer.content != null)
        {
          str += "\n";
          for (int index = 0; index < contentContainer.content.Count; ++index)
            str = $"{str}    • {contentContainer.content[index].ToString()}: {CodexEntry.GetContentWidgetDebugString(contentContainer.content[index])}\n";
        }
        stringList.Add(str);
      }
      else
        stringList.Add("null container");
    }
    return stringList;
  }

  private static string GetContentWidgetDebugString(ICodexWidget widget)
  {
    switch (widget)
    {
      case CodexText codexText:
        return codexText.text;
      case CodexLabelWithIcon codexLabelWithIcon:
        return $"{codexLabelWithIcon.label.text} / {codexLabelWithIcon.icon.spriteName}";
      case CodexImage codexImage:
        return codexImage.spriteName;
      case CodexVideo codexVideo:
        return codexVideo.name;
      case CodexIndentedLabelWithIcon indentedLabelWithIcon:
        return $"{indentedLabelWithIcon.label.text} / {indentedLabelWithIcon.icon.spriteName}";
      default:
        return "";
    }
  }

  public void CreateContentContainerCollection()
  {
    this.contentContainers = new List<ContentContainer>();
  }

  public void InsertContentContainer(int index, ContentContainer container)
  {
    this.contentContainers.Insert(index, container);
  }

  public void RemoveContentContainerAt(int index) => this.contentContainers.RemoveAt(index);

  public void AddContentContainer(ContentContainer container)
  {
    this.contentContainers.Add(container);
  }

  public void AddContentContainerRange(IEnumerable<ContentContainer> containers)
  {
    this.contentContainers.AddRange(containers);
  }

  public void RemoveContentContainer(ContentContainer container)
  {
    this.contentContainers.Remove(container);
  }

  public ICodexWidget GetFirstWidget()
  {
    for (int index1 = 0; index1 < this.contentContainers.Count; ++index1)
    {
      if (this.contentContainers[index1].content != null)
      {
        for (int index2 = 0; index2 < this.contentContainers[index1].content.Count; ++index2)
        {
          if (this.contentContainers[index1].content[index2] != null && Game.IsCorrectDlcActiveForCurrentSave(this.contentContainers[index1].content[index2] as IHasDlcRestrictions))
            return this.contentContainers[index1].content[index2];
        }
      }
    }
    return (ICodexWidget) null;
  }

  public string[] requiredAtLeastOneDlcIds { get; set; }

  public string[] requiredDlcIds { get; set; }

  public string[] forbiddenDlcIds { get; set; }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;

  public string[] GetAnyRequiredDlcIds() => this.requiredAtLeastOneDlcIds;

  public string id
  {
    get => this._id;
    set => this._id = value;
  }

  public string parentId
  {
    get => this._parentId;
    set => this._parentId = value;
  }

  public string category
  {
    get => this._category;
    set => this._category = value;
  }

  public string title
  {
    get => this._title;
    set => this._title = value;
  }

  public string name
  {
    get => this._name;
    set => this._name = value;
  }

  public string subtitle
  {
    get => this._subtitle;
    set => this._subtitle = value;
  }

  public List<SubEntry> subEntries
  {
    get => this._subEntries;
    set => this._subEntries = value;
  }

  public List<CodexEntry_MadeAndUsed> contentMadeAndUsed
  {
    get => this._contentMadeAndUsed;
    set => this._contentMadeAndUsed = value;
  }

  public Sprite icon
  {
    get => this._icon;
    set => this._icon = value;
  }

  public Color iconColor
  {
    get => this._iconColor;
    set => this._iconColor = value;
  }

  public string iconPrefabID
  {
    get => this._iconPrefabID;
    set => this._iconPrefabID = value;
  }

  public string iconLockID
  {
    get => this._iconLockID;
    set => this._iconLockID = value;
  }

  public string iconAssetName
  {
    get => this._iconAssetName;
    set => this._iconAssetName = value;
  }

  public bool disabled
  {
    get => this._disabled;
    set => this._disabled = value;
  }

  public bool searchOnly
  {
    get => this._searchOnly;
    set => this._searchOnly = value;
  }

  public int customContentLength
  {
    get => this._customContentLength;
    set => this._customContentLength = value;
  }

  public string sortString
  {
    get => this._sortString;
    set => this._sortString = value;
  }

  public bool showBeforeGeneratedCategoryLinks
  {
    get => this._showBeforeGeneratedCategoryLinks;
    set => this._showBeforeGeneratedCategoryLinks = value;
  }

  public bool insertMergeContentAtBottom
  {
    get => this._insertMergeContentAtBottom;
    set => this._insertMergeContentAtBottom = value;
  }
}
