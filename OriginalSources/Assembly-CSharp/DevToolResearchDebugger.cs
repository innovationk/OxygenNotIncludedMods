// Decompiled with JetBrains decompiler
// Type: DevToolResearchDebugger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DevToolResearchDebugger : DevTool
{
  public DevToolResearchDebugger() => this.RequiresGameRunning = true;

  protected override void RenderTo(DevPanel panel)
  {
    TechInstance activeResearch = Research.Instance.GetActiveResearch();
    if (activeResearch == null)
    {
      ImGui.Text("No Active Research");
    }
    else
    {
      ImGui.Text("Active Research");
      ImGui.Text("ID: " + activeResearch.tech.Id);
      ImGui.Text("Name: " + Util.StripTextFormatting(activeResearch.tech.Name));
      ImGui.Separator();
      ImGui.Text("Active Research Inventory");
      foreach (KeyValuePair<string, float> keyValuePair in new Dictionary<string, float>((IDictionary<string, float>) activeResearch.progressInventory.PointsByTypeID))
      {
        if (activeResearch.tech.RequiresResearchType(keyValuePair.Key))
        {
          float max = activeResearch.tech.costsByResearchTypeID[keyValuePair.Key];
          float v = keyValuePair.Value;
          if (ImGui.Button("Fill"))
            v = max;
          ImGui.SameLine();
          ImGui.SetNextItemWidth(100f);
          ImGui.InputFloat(keyValuePair.Key, ref v, 1f, 10f);
          ImGui.SameLine();
          ImGui.Text($"of {max}");
          activeResearch.progressInventory.PointsByTypeID[keyValuePair.Key] = Mathf.Clamp(v, 0.0f, max);
        }
      }
      ImGui.Separator();
      ImGui.Text("Global Points Inventory");
      foreach (KeyValuePair<string, float> keyValuePair in Research.Instance.globalPointInventory.PointsByTypeID)
        ImGui.Text($"{keyValuePair.Key}: {keyValuePair.Value.ToString()}");
    }
  }
}
