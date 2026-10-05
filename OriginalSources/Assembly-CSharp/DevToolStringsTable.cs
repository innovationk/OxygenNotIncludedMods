// Decompiled with JetBrains decompiler
// Type: DevToolStringsTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevToolStringsTable : DevTool
{
  private List<(string id, string value)> m_cached_entries;
  private const int MAX_ENTRIES_TO_DRAW = 3000;
  private string m_search_filter = "";

  protected override void RenderTo(DevPanel panel)
  {
    if (this.m_cached_entries == null)
    {
      this.m_cached_entries = new List<(string, string)>();
      DevToolStringsTable.RegenerateCacheWithFilter(this.m_cached_entries, this.m_search_filter);
    }
    if (!ImGui.CollapsingHeader($"Entries ({this.m_cached_entries.Count})###ID_LocStringEntries", ImGuiTreeNodeFlags.DefaultOpen))
      return;
    if (ImGuiEx.InputFilter("Filter", ref this.m_search_filter))
      DevToolStringsTable.RegenerateCacheWithFilter(this.m_cached_entries, this.m_search_filter);
    ImGui.Columns(2, "LocStrings");
    ImGui.Text("Key");
    ImGui.NextColumn();
    ImGui.Text("Value");
    ImGui.NextColumn();
    ImGui.Separator();
    int num = Mathf.Min(3000, this.m_cached_entries.Count);
    for (int index = 0; index < num; ++index)
    {
      (string id, string value) = this.m_cached_entries[index];
      if (ImGui.Selectable($"{id}###ID_{index}_key"))
      {
        this.m_search_filter = id;
        DevToolStringsTable.RegenerateCacheWithFilter(this.m_cached_entries, this.m_search_filter);
        break;
      }
      ImGuiEx.TooltipForPrevious(id ?? "");
      ImGui.NextColumn();
      if (ImGui.Selectable($"{value}###ID_{index}_value"))
      {
        this.m_search_filter = value;
        DevToolStringsTable.RegenerateCacheWithFilter(this.m_cached_entries, this.m_search_filter);
        break;
      }
      ImGuiEx.TooltipForPrevious(value ?? "");
      ImGui.NextColumn();
    }
    ImGui.Columns(1);
    if (this.m_cached_entries.Count <= 3000)
      return;
    ImGui.Separator();
    ImGui.Text($"* Stopped drawing entries because there are too many to draw (limit: {3000}, current: {this.m_cached_entries.Count}) *");
  }

  public static void RegenerateCacheWithFilter(
    List<(string id, string value)> cached_entries,
    string filter)
  {
    cached_entries.Clear();
    if (!string.IsNullOrWhiteSpace(filter))
    {
      string normalized_filter = filter.ToLowerInvariant().Trim();
      Strings.VisitEntries((StringTable.EntryVisitor) ((id, value) =>
      {
        if (!id.ToLowerInvariant().Contains(normalized_filter) && !value.ToLowerInvariant().Contains(normalized_filter))
          return;
        cached_entries.Add((id, value));
      }));
    }
    else
      Strings.VisitEntries((StringTable.EntryVisitor) ((id, value) => cached_entries.Add((id, value))));
  }
}
