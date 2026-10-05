// Decompiled with JetBrains decompiler
// Type: ICheckboxListGroupControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public interface ICheckboxListGroupControl
{
  string Title { get; }

  string Description { get; }

  ICheckboxListGroupControl.ListGroup[] GetData();

  bool SidescreenEnabled();

  int CheckboxSideScreenSortOrder();

  struct ListGroup(
    string title,
    ICheckboxListGroupControl.CheckboxItem[] checkboxItems,
    Func<string, string> resolveTitleCallback = null,
    System.Action onItemClicked = null)
  {
    public Func<string, string> resolveTitleCallback = resolveTitleCallback;
    public System.Action onItemClicked = onItemClicked;
    public string title = title;
    public ICheckboxListGroupControl.CheckboxItem[] checkboxItems = checkboxItems;
  }

  struct CheckboxItem
  {
    public string text;
    public string tooltip;
    public bool isOn;
    public Func<string, bool> overrideLinkActions;
    public Func<string, object, string> resolveTooltipCallback;
  }
}
