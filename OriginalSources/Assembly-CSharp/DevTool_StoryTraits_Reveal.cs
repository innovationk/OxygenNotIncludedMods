// Decompiled with JetBrains decompiler
// Type: DevTool_StoryTraits_Reveal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevTool_StoryTraits_Reveal : DevTool
{
  protected override void RenderTo(DevPanel panel)
  {
    int index;
    bool forUniqueBuilding = DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out index);
    if (ImGuiEx.Button("Focus on headquaters", forUniqueBuilding))
      DevToolUtil.FocusCameraOnCell(index);
    if (!forUniqueBuilding)
      ImGuiEx.TooltipForPrevious("Couldn't find headquaters");
    if (!ImGui.CollapsingHeader("Search world for entity", ImGuiTreeNodeFlags.DefaultOpen))
      return;
    IReadOnlyList<WorldGenSpawner.Spawnable> allSpawnables = this.GetAllSpawnables();
    if (allSpawnables == null)
    {
      ImGui.Text("Couldn't find a list of spawnables");
    }
    else
    {
      foreach (string prefabId in this.GetPrefabIDsToSearchFor())
      {
        int cellIndex;
        int num = this.GetCellIndexForSpawnable(prefabId, allSpawnables, out cellIndex) ? 1 : 0;
        string str = $"\"{prefabId}\"";
        bool enabled = num != 0;
        if (ImGuiEx.Button("Reveal and focus on " + str, enabled))
          DevToolUtil.RevealAndFocusAt(cellIndex);
        if (!enabled)
          ImGuiEx.TooltipForPrevious("Couldn't find a cell that contained a spawnable with component " + str);
      }
    }
  }

  public IEnumerable<string> GetPrefabIDsToSearchFor()
  {
    yield return "MegaBrainTank";
    yield return "GravitasCreatureManipulator";
    yield return "LonelyMinionHouse";
    yield return "FossilDig";
    yield return "HijackedHeadquarters";
  }

  private bool GetCellIndexForSpawnable(
    string prefabId,
    IReadOnlyList<WorldGenSpawner.Spawnable> spawnablesToSearch,
    out int cellIndex)
  {
    foreach (WorldGenSpawner.Spawnable spawnable in (IEnumerable<WorldGenSpawner.Spawnable>) spawnablesToSearch)
    {
      if (prefabId == spawnable.spawnInfo.id)
      {
        cellIndex = spawnable.cell;
        return true;
      }
    }
    cellIndex = -1;
    return false;
  }

  private IReadOnlyList<WorldGenSpawner.Spawnable> GetAllSpawnables()
  {
    WorldGenSpawner firstObjectByType = Object.FindFirstObjectByType<WorldGenSpawner>(FindObjectsInactive.Include);
    return (Object) firstObjectByType == (Object) null ? (IReadOnlyList<WorldGenSpawner.Spawnable>) null : firstObjectByType.GetSpawnables() ?? (IReadOnlyList<WorldGenSpawner.Spawnable>) null;
  }
}
