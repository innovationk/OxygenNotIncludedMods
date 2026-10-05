// Decompiled with JetBrains decompiler
// Type: DevToolSpaceScannerNetwork
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Collections.Generic;

#nullable disable
public class DevToolSpaceScannerNetwork : DevTool
{
  private ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry> tableDrawer;

  public DevToolSpaceScannerNetwork()
  {
    this.tableDrawer = ImGuiObjectTableDrawer<DevToolSpaceScannerNetwork.Entry>.New().Column("WorldId", (Func<DevToolSpaceScannerNetwork.Entry, object>) (e => (object) e.worldId)).Column("Network Quality (0->1)", (Func<DevToolSpaceScannerNetwork.Entry, object>) (e => (object) e.networkQuality)).Column("Targets Detected", (Func<DevToolSpaceScannerNetwork.Entry, string>) (e => e.targetsString)).FixedHeight(300f).Build();
  }

  protected override void RenderTo(DevPanel panel)
  {
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      ImGui.Text("Game instance is null");
    else if (Game.Instance.spaceScannerNetworkManager == null)
      ImGui.Text("SpaceScannerNetworkQualityManager instance is null");
    else if ((UnityEngine.Object) ClusterManager.Instance == (UnityEngine.Object) null)
    {
      ImGui.Text("ClusterManager instance is null");
    }
    else
    {
      if (ImGui.CollapsingHeader("Worlds Data"))
        this.tableDrawer.Draw(DevToolSpaceScannerNetwork.GetData());
      if (!ImGui.CollapsingHeader("Full DevToolSpaceScannerNetwork Info"))
        return;
      ImGuiEx.DrawObject((object) Game.Instance.spaceScannerNetworkManager);
    }
  }

  public static IEnumerable<DevToolSpaceScannerNetwork.Entry> GetData()
  {
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
      yield return new DevToolSpaceScannerNetwork.Entry(worldContainer.id, Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(worldContainer.id), DevToolSpaceScannerNetwork.GetTargetsString(worldContainer));
  }

  public static string GetTargetsString(WorldContainer world)
  {
    SpaceScannerWorldData scannerWorldData;
    return !Game.Instance.spaceScannerNetworkManager.DEBUG_GetWorldIdToDataMap().TryGetValue(world.id, out scannerWorldData) || scannerWorldData.targetIdsDetected.Count == 0 ? "<none>" : string.Join(",", (IEnumerable<string>) scannerWorldData.targetIdsDetected);
  }

  public readonly struct Entry(int worldId, float networkQuality, string targetsString)
  {
    public readonly int worldId = worldId;
    public readonly float networkQuality = networkQuality;
    public readonly string targetsString = targetsString;
  }
}
