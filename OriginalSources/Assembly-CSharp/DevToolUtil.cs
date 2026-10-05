// Decompiled with JetBrains decompiler
// Type: DevToolUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class DevToolUtil
{
  public static DevPanel Open(DevTool devTool)
  {
    return DevToolManager.Instance.panels.AddPanelFor(devTool);
  }

  public static DevPanel Open<T>() where T : DevTool, new()
  {
    return DevToolManager.Instance.panels.AddPanelFor<T>();
  }

  public static DevPanel DebugObject<T>(T obj)
  {
    return DevToolUtil.Open((DevTool) new DevToolObjectViewer<T>((Func<T>) (() => obj)));
  }

  public static DevPanel DebugObject<T>(Func<T> get_obj_fn)
  {
    return DevToolUtil.Open((DevTool) new DevToolObjectViewer<T>(get_obj_fn));
  }

  public static void Close(DevTool devTool) => devTool.ClosePanel();

  public static void Close(DevPanel devPanel) => devPanel.Close();

  public static string GenerateDevToolName(DevTool devTool)
  {
    return DevToolUtil.GenerateDevToolName(devTool.GetType());
  }

  public static string GenerateDevToolName(System.Type devToolType)
  {
    string devToolName1;
    if (DevToolManager.Instance != null && DevToolManager.Instance.devToolNameDict.TryGetValue(devToolType, out devToolName1))
      return devToolName1;
    string devToolName2 = devToolType.Name;
    if (devToolName2.StartsWith("DevTool_"))
      devToolName2 = devToolName2.Substring("DevTool_".Length);
    else if (devToolName2.StartsWith("DevTool"))
      devToolName2 = devToolName2.Substring("DevTool".Length);
    return devToolName2;
  }

  public static bool CanRevealAndFocus(GameObject gameObject)
  {
    return DevToolUtil.TryGetCellIndexFor(gameObject, out int _);
  }

  public static void RevealAndFocus(GameObject gameObject)
  {
    int cellIndex;
    if (DevToolUtil.TryGetCellIndexFor(gameObject, out cellIndex))
      return;
    DevToolUtil.RevealAndFocusAt(cellIndex);
    if (!gameObject.GetComponent<KSelectable>().IsNullOrDestroyed())
      SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
    else
      SelectTool.Instance.Select((KSelectable) null);
  }

  public static void FocusCameraOnCell(int cellIndex)
  {
    CameraController.Instance.SetPosition(Grid.CellToPos2D(cellIndex));
  }

  public static bool TryGetCellIndexFor(GameObject gameObject, out int cellIndex)
  {
    cellIndex = -1;
    if (gameObject.IsNullOrDestroyed() || !gameObject.GetComponent<RectTransform>().IsNullOrDestroyed())
      return false;
    cellIndex = Grid.PosToCell(gameObject);
    return true;
  }

  public static bool TryGetCellIndexForUniqueBuilding(string prefabId, out int index)
  {
    index = -1;
    BuildingComplete[] objectsByType = UnityEngine.Object.FindObjectsByType<BuildingComplete>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
    if (objectsByType == null)
      return false;
    foreach (BuildingComplete buildingComplete in objectsByType)
    {
      if (prefabId == buildingComplete.Def.PrefabID)
      {
        index = buildingComplete.GetCell();
        return true;
      }
    }
    return false;
  }

  public static void RevealAndFocusAt(int cellIndex)
  {
    int x1;
    int y1;
    Grid.CellToXY(cellIndex, out x1, out y1);
    GridVisibility.Reveal(x1 + 2, y1 + 2, 10, 10f);
    DevToolUtil.FocusCameraOnCell(cellIndex);
    int index;
    if (!DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out index))
      return;
    Vector3 pos2D1 = Grid.CellToPos2D(cellIndex);
    Vector3 pos2D2 = Grid.CellToPos2D(index);
    float num = 2f / Vector3.Distance(pos2D1, pos2D2);
    for (float t = 0.0f; (double) t < 1.0; t += num)
    {
      int x2;
      int y2;
      Grid.PosToXY(Vector3.Lerp(pos2D1, pos2D2, t), out x2, out y2);
      GridVisibility.Reveal(x2 + 2, y2 + 2, 4, 4f);
    }
  }

  public enum TextAlignment
  {
    Center,
    Left,
    Right,
  }
}
