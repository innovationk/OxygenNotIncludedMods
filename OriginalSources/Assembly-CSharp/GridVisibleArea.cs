// Decompiled with JetBrains decompiler
// Type: GridVisibleArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GridVisibleArea
{
  private GridArea[] VisibleAreas = new GridArea[3];
  private GridArea[] VisibleAreasExtended = new GridArea[3];
  private List<GridVisibleArea.Callback> Callbacks = new List<GridVisibleArea.Callback>();
  public bool debugFreezeVisibleArea;
  public bool debugFreezeVisibleAreasExtended;
  private readonly int _padding;

  public GridArea CurrentArea => this.VisibleAreas[0];

  public GridArea PreviousArea => this.VisibleAreas[1];

  public GridArea PreviousPreviousArea => this.VisibleAreas[2];

  public GridArea CurrentAreaExtended => this.VisibleAreasExtended[0];

  public GridArea PreviousAreaExtended => this.VisibleAreasExtended[1];

  public GridArea PreviousPreviousAreaExtended => this.VisibleAreasExtended[2];

  public GridVisibleArea()
  {
  }

  public GridVisibleArea(int padding) => this._padding = padding;

  public void Update()
  {
    if (!this.debugFreezeVisibleArea)
    {
      this.VisibleAreas[2] = this.VisibleAreas[1];
      this.VisibleAreas[1] = this.VisibleAreas[0];
      this.VisibleAreas[0] = GridVisibleArea.GetVisibleArea();
    }
    if (!this.debugFreezeVisibleAreasExtended)
    {
      this.VisibleAreasExtended[2] = this.VisibleAreasExtended[1];
      this.VisibleAreasExtended[1] = this.VisibleAreasExtended[0];
      this.VisibleAreasExtended[0] = GridVisibleArea.GetVisibleAreaExtended(this._padding);
    }
    foreach (GridVisibleArea.Callback callback in this.Callbacks)
      callback.OnUpdate();
  }

  public void AddCallback(string name, System.Action on_update)
  {
    this.Callbacks.Add(new GridVisibleArea.Callback()
    {
      Name = name,
      OnUpdate = on_update
    });
  }

  public void Run(Action<int> in_view)
  {
    if (in_view == null)
      return;
    this.CurrentArea.Run(in_view);
  }

  public void RunExtended(Action<int> in_view)
  {
    if (in_view == null)
      return;
    this.CurrentAreaExtended.Run(in_view);
  }

  public void Run(
    Action<int> outside_view,
    Action<int> inside_view,
    Action<int> inside_view_second_time)
  {
    if (outside_view != null)
      this.PreviousArea.RunOnDifference(this.CurrentArea, outside_view);
    if (inside_view != null)
      this.CurrentArea.RunOnDifference(this.PreviousArea, inside_view);
    if (inside_view_second_time == null)
      return;
    this.PreviousArea.RunOnDifference(this.PreviousPreviousArea, inside_view_second_time);
  }

  public void RunExtended(
    Action<int> outside_view,
    Action<int> inside_view,
    Action<int> inside_view_second_time)
  {
    if (outside_view != null)
      this.PreviousAreaExtended.RunOnDifference(this.CurrentAreaExtended, outside_view);
    if (inside_view != null)
      this.CurrentAreaExtended.RunOnDifference(this.PreviousAreaExtended, inside_view);
    if (inside_view_second_time == null)
      return;
    this.PreviousAreaExtended.RunOnDifference(this.PreviousPreviousAreaExtended, inside_view_second_time);
  }

  public void RunIfVisible(int cell, Action<int> action)
  {
    this.CurrentArea.RunIfInside(cell, action);
  }

  public void RunIfVisibleExtended(int cell, Action<int> action)
  {
    this.CurrentAreaExtended.RunIfInside(cell, action);
  }

  public static GridArea GetVisibleArea() => GridVisibleArea.GetVisibleAreaExtended(0);

  public static GridArea GetVisibleAreaExtended(int padding)
  {
    GridArea visibleAreaExtended = new GridArea();
    Camera mainCamera = Game.MainCamera;
    if ((UnityEngine.Object) mainCamera != (UnityEngine.Object) null)
    {
      Vector3 worldPoint1 = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.transform.GetPosition().z));
      Vector3 worldPoint2 = mainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, mainCamera.transform.GetPosition().z));
      worldPoint1.x += (float) padding;
      worldPoint1.y += (float) padding;
      worldPoint2.x -= (float) padding;
      worldPoint2.y -= (float) padding;
      if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
      {
        Vector2I worldOffset;
        Vector2I worldSize;
        CameraController.Instance.GetWorldCamera(out worldOffset, out worldSize);
        visibleAreaExtended.SetExtents(Math.Max((int) ((double) worldPoint2.x - 0.5), worldOffset.x), Math.Max((int) ((double) worldPoint2.y - 0.5), worldOffset.y), Math.Min((int) ((double) worldPoint1.x + 1.5), worldSize.x + worldOffset.x), Math.Min((int) ((double) worldPoint1.y + 1.5), worldSize.y + worldOffset.y));
      }
      else
        visibleAreaExtended.SetExtents(Math.Max((int) ((double) worldPoint2.x - 0.5), 0), Math.Max((int) ((double) worldPoint2.y - 0.5), 0), Math.Min((int) ((double) worldPoint1.x + 1.5), Grid.WidthInCells), Math.Min((int) ((double) worldPoint1.y + 1.5), Grid.HeightInCells));
    }
    return visibleAreaExtended;
  }

  public struct Callback
  {
    public System.Action OnUpdate;
    public string Name;
  }
}
