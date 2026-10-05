// Decompiled with JetBrains decompiler
// Type: StampTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StampTool : InterfaceTool
{
  public static StampTool Instance;
  private StampToolPreview preview;
  public TemplateContainer stampTemplate;
  public GameObject PlacerPrefab;
  private bool ready = true;
  private bool selectAffected;
  private bool deactivateOnStamp;

  public static void DestroyInstance() => StampTool.Instance = (StampTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    StampTool.Instance = this;
    this.preview = new StampToolPreview((InterfaceTool) this, new IStampToolPreviewPlugin[4]
    {
      (IStampToolPreviewPlugin) new StampToolPreview_Placers(this.PlacerPrefab),
      (IStampToolPreviewPlugin) new StampToolPreview_Area(),
      (IStampToolPreviewPlugin) new StampToolPreview_SolidLiquidGas(),
      (IStampToolPreviewPlugin) new StampToolPreview_Prefabs()
    });
  }

  private void Update() => this.preview.Refresh(Grid.PosToCell(this.GetCursorPos()));

  public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
  {
    this.selectAffected = SelectAffected;
    this.deactivateOnStamp = DeactivateOnStamp;
    if (this.stampTemplate == template || template == null || template.cells == null)
      return;
    this.stampTemplate = template;
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    this.StartCoroutine(this.preview.Setup(template));
  }

  private Vector3 GetCursorPos() => PlayerController.GetCursorPos(KInputManager.GetMousePos());

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    this.Stamp((Vector2) cursor_pos);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.BuildMenuKeyQ))
    {
      Vector3 cursorPos = this.GetCursorPos();
      DebugBaseTemplateButton.Instance.ClearSelection();
      if (this.stampTemplate.cells != null)
      {
        for (int index = 0; index < this.stampTemplate.cells.Count; ++index)
          DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int) ((double) cursorPos.x + (double) this.stampTemplate.cells[index].location_x), (int) ((double) cursorPos.y + (double) this.stampTemplate.cells[index].location_y)));
      }
    }
    base.OnKeyDown(e);
  }

  private void Stamp(Vector2 pos)
  {
    if (!this.ready)
      return;
    int cell1 = Grid.PosToCell(pos);
    Vector2f size = this.stampTemplate.info.size;
    int x1 = Mathf.FloorToInt((float) (-(double) size.X / 2.0));
    int cell2 = Grid.OffsetCell(cell1, x1, 0);
    int cell3 = Grid.PosToCell(pos);
    size = this.stampTemplate.info.size;
    int x2 = Mathf.FloorToInt(size.X / 2f);
    int cell4 = Grid.OffsetCell(cell3, x2, 0);
    int cell5 = Grid.PosToCell(pos);
    size = this.stampTemplate.info.size;
    int y1 = 1 + Mathf.FloorToInt((float) (-(double) size.Y / 2.0));
    int cell6 = Grid.OffsetCell(cell5, 0, y1);
    int cell7 = Grid.PosToCell(pos);
    size = this.stampTemplate.info.size;
    int y2 = 1 + Mathf.FloorToInt(size.Y / 2f);
    int cell8 = Grid.OffsetCell(cell7, 0, y2);
    if (!Grid.IsValidBuildingCell(cell2) || !Grid.IsValidBuildingCell(cell4) || !Grid.IsValidBuildingCell(cell8) || !Grid.IsValidBuildingCell(cell6))
      return;
    this.ready = false;
    bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause();
    if (this.stampTemplate.cells != null)
    {
      this.preview.OnPlace();
      List<GameObject> gameObjectList = new List<GameObject>();
      for (int index = 0; index < this.stampTemplate.cells.Count; ++index)
      {
        for (int layer = 0; layer < 34; ++layer)
        {
          GameObject gameObject = Grid.Objects[Grid.XYToCell((int) ((double) pos.x + (double) this.stampTemplate.cells[index].location_x), (int) ((double) pos.y + (double) this.stampTemplate.cells[index].location_y)), layer];
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && !gameObjectList.Contains(gameObject))
            gameObjectList.Add(gameObject);
        }
      }
      foreach (GameObject original in gameObjectList)
      {
        if ((UnityEngine.Object) original != (UnityEngine.Object) null)
          Util.KDestroyGameObject(original);
      }
    }
    TemplateLoader.Stamp(this.stampTemplate, pos, (System.Action) (() => this.CompleteStamp(pauseOnComplete)));
    if (this.selectAffected)
    {
      DebugBaseTemplateButton.Instance.ClearSelection();
      if (this.stampTemplate.cells != null)
      {
        for (int index = 0; index < this.stampTemplate.cells.Count; ++index)
          DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int) ((double) pos.x + (double) this.stampTemplate.cells[index].location_x), (int) ((double) pos.y + (double) this.stampTemplate.cells[index].location_y)));
      }
    }
    if (!this.deactivateOnStamp)
      return;
    this.DeactivateTool();
  }

  private void CompleteStamp(bool pause)
  {
    if (pause)
      SpeedControlScreen.Instance.Pause();
    this.ready = true;
    this.OnDeactivateTool((InterfaceTool) null);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    if (this.gameObject.activeSelf)
      return;
    this.preview.Cleanup();
    this.stampTemplate = (TemplateContainer) null;
  }
}
