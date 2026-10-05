// Decompiled with JetBrains decompiler
// Type: DevToolUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class DevToolUI : DevTool
{
  private List<RaycastResult> m_raycast_hits = new List<RaycastResult>();
  private RaycastResult? m_last_pinged_hit;

  protected override void RenderTo(DevPanel panel)
  {
    this.RepopulateRaycastHits();
    this.DrawPingObject();
    this.DrawRaycastHits();
  }

  private void DrawPingObject()
  {
    if (this.m_last_pinged_hit.HasValue)
    {
      GameObject gameObject = this.m_last_pinged_hit.Value.gameObject;
      if ((!((Object) gameObject != (Object) null) ? 0 : ((bool) (Object) gameObject ? 1 : 0)) != 0)
      {
        ImGui.Text($"Last Pinged: \"{DevToolUI.GetQualifiedName(gameObject)}\"");
        ImGui.SameLine();
        if (ImGui.Button("Inspect"))
          DevToolSceneInspector.Inspect((object) gameObject);
        ImGui.Spacing();
        ImGui.Spacing();
      }
      else
        this.m_last_pinged_hit = new RaycastResult?();
    }
    ImGui.Text("Press \",\" to ping the top hovered ui object");
    ImGui.Spacing();
    ImGui.Spacing();
  }

  private void Internal_Ping(RaycastResult raycastResult)
  {
    GameObject gameObject = raycastResult.gameObject;
    this.m_last_pinged_hit = new RaycastResult?(raycastResult);
  }

  public static void PingHoveredObject()
  {
    using (ListPool<RaycastResult, DevToolUI>.PooledList pooledList = PoolsFor<DevToolUI>.AllocateList<RaycastResult>())
    {
      UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
      if ((Object) current == (Object) null || !(bool) (Object) current)
        return;
      UnityEngine.EventSystems.EventSystem eventSystem = current;
      PointerEventData eventData = new PointerEventData(current);
      eventData.position = (Vector2) Input.mousePosition;
      ListPool<RaycastResult, DevToolUI>.PooledList raycastResults = pooledList;
      eventSystem.RaycastAll(eventData, (List<RaycastResult>) raycastResults);
      DevToolUI devTool = DevToolManager.Instance.panels.AddOrGetDevTool<DevToolUI>();
      if (pooledList.Count <= 0)
        return;
      devTool.Internal_Ping(pooledList[0]);
    }
  }

  private void DrawRaycastHits()
  {
    if (this.m_raycast_hits.Count <= 0)
    {
      ImGui.Text("Didn't hit any ui");
    }
    else
    {
      ImGui.Text("Raycast Hits:");
      ImGui.Indent();
      for (int index = 0; index < this.m_raycast_hits.Count; ++index)
      {
        RaycastResult raycastHit = this.m_raycast_hits[index];
        ImGui.BulletText($"[{index}] {DevToolUI.GetQualifiedName(raycastHit.gameObject)}");
      }
      ImGui.Unindent();
    }
  }

  private void RepopulateRaycastHits()
  {
    this.m_raycast_hits.Clear();
    UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
    if ((Object) current == (Object) null || !(bool) (Object) current)
      return;
    UnityEngine.EventSystems.EventSystem eventSystem = current;
    PointerEventData eventData = new PointerEventData(current);
    eventData.position = (Vector2) Input.mousePosition;
    List<RaycastResult> raycastHits = this.m_raycast_hits;
    eventSystem.RaycastAll(eventData, raycastHits);
  }

  private static string GetQualifiedName(GameObject game_object)
  {
    KScreen componentInParent = game_object.GetComponentInParent<KScreen>();
    return (Object) componentInParent != (Object) null ? $"{componentInParent.gameObject.name} :: {game_object.name}" : game_object.name ?? "";
  }
}
