// Decompiled with JetBrains decompiler
// Type: DevToolBigBaseMutations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using UnityEngine;

#nullable disable
public class DevToolBigBaseMutations : DevTool
{
  protected override void RenderTo(DevPanel panel)
  {
    if ((Object) Game.Instance != (Object) null)
      this.ShowButtons();
    else
      ImGui.Text("Game not available");
  }

  private void ShowButtons()
  {
    if (ImGui.Button("Destroy Ladders"))
      this.DestroyGameObjects<Ladder>(Components.Ladders, Tag.Invalid);
    if (ImGui.Button("Destroy Tiles"))
      this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.FloorTiles);
    if (ImGui.Button("Destroy Wires"))
      this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.Wires);
    if (!ImGui.Button("Destroy Pipes"))
      return;
    this.DestroyGameObjects<BuildingComplete>(Components.BuildingCompletes, GameTags.Pipes);
  }

  private void DestroyGameObjects<T>(Components.Cmps<T> componentsList, Tag filterForTag)
  {
    for (int idx = componentsList.Count - 1; idx >= 0; --idx)
    {
      if (!((object) componentsList[idx]).IsNullOrDestroyed() && (!(filterForTag != Tag.Invalid) || ((object) componentsList[idx] as KMonoBehaviour).gameObject.HasTag(filterForTag)))
        Util.KDestroyGameObject((Component) ((object) componentsList[idx] as KMonoBehaviour));
    }
  }
}
