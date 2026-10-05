// Decompiled with JetBrains decompiler
// Type: VisibleAreaUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class VisibleAreaUpdater
{
  private GridVisibleArea VisibleArea;
  private Action<int> OutsideViewFirstTimeCallback;
  private Action<int> InsideViewFirstTimeCallback;
  private Action<int> UpdateCallback;
  private string Name;

  public VisibleAreaUpdater(
    Action<int> outside_view_first_time_cb,
    Action<int> inside_view_first_time_cb,
    string name)
  {
    this.OutsideViewFirstTimeCallback = outside_view_first_time_cb;
    this.InsideViewFirstTimeCallback = inside_view_first_time_cb;
    this.UpdateCallback = new Action<int>(this.InternalUpdateCell);
    this.Name = name;
  }

  public void Update()
  {
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null) || this.VisibleArea != null)
      return;
    this.VisibleArea = CameraController.Instance.VisibleArea;
    this.VisibleArea.Run(this.InsideViewFirstTimeCallback);
  }

  private void InternalUpdateCell(int cell)
  {
    this.OutsideViewFirstTimeCallback(cell);
    this.InsideViewFirstTimeCallback(cell);
  }

  public void UpdateCell(int cell)
  {
    if (this.VisibleArea == null)
      return;
    this.VisibleArea.RunIfVisible(cell, this.UpdateCallback);
  }
}
