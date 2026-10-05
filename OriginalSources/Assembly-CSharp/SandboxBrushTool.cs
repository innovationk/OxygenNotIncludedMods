// Decompiled with JetBrains decompiler
// Type: SandboxBrushTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxBrushTool : BrushTool
{
  public static SandboxBrushTool instance;
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();
  private EventInstance audioEvent;

  public static void DestroyInstance() => SandboxBrushTool.instance = (SandboxBrushTool) null;

  private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxBrushTool.instance = this;
  }

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.elementSelector.onValueChanged += new Action<object>(this.OnElementChanged);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
    int num = (int) this.audioEvent.release();
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int recentlyAffectedCell in this.recentlyAffectedCells)
    {
      Color color = new Color(this.recentAffectedCellColor[recentlyAffectedCell].r, this.recentAffectedCellColor[recentlyAffectedCell].g, this.recentAffectedCellColor[recentlyAffectedCell].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 10f), -1f, 1f, 0.1f, 0.2f));
      colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, color));
    }
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public override void SetBrushSize(int radius)
  {
    this.brushRadius = radius;
    this.brushOffsets.Clear();
    for (int x = 0; x < this.brushRadius * 2; ++x)
    {
      for (int y = 0; y < this.brushRadius * 2; ++y)
      {
        if ((double) Vector2.Distance(new Vector2((float) x, (float) y), new Vector2((float) this.brushRadius, (float) this.brushRadius)) < (double) this.brushRadius - 0.800000011920929)
          this.brushOffsets.Add(new Vector2((float) (x - this.brushRadius), (float) (y - this.brushRadius)));
      }
    }
  }

  protected override void OnPaintCell(int cell, int distFromOrigin)
  {
    base.OnPaintCell(cell, distFromOrigin);
    this.recentlyAffectedCells.Add(cell);
    Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
    if (!this.recentAffectedCellColor.ContainsKey(cell))
      this.recentAffectedCellColor.Add(cell, (Color) element.substance.uiColour);
    else
      this.recentAffectedCellColor[cell] = (Color) element.substance.uiColour;
    int index1 = Game.Instance.callbackManager.Add(new Game.CallbackInfo((System.Action) (() =>
    {
      this.recentlyAffectedCells.Remove(cell);
      this.recentAffectedCellColor.Remove(cell);
    }))).index;
    byte index2 = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
    Klei.AI.Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
    if (disease != null)
      index2 = Db.Get().Diseases.GetIndex(disease.id);
    float num1 = element.id == SimHashes.Vacuum ? 0.0f : this.settings.GetFloatSetting("SandboxTools.Mass");
    int gameCell = cell;
    int id = (int) element.id;
    CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
    double mass = (double) num1;
    double floatSetting = (double) this.settings.GetFloatSetting("SandbosTools.Temperature");
    int num2 = index1;
    int diseaseIdx = (int) index2;
    int intSetting = this.settings.GetIntSetting("SandboxTools.DiseaseCount");
    int callbackIdx = num2;
    SimMessages.ReplaceElement(gameCell, (SimHashes) id, sandBoxTool, (float) mass, (float) floatSetting, (byte) diseaseIdx, intSetting, callbackIdx);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.SandboxCopyElement))
    {
      int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
      if (Grid.IsValidCell(cell))
        SandboxSampleTool.Sample(cell);
    }
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click"));
  }

  public override void OnLeftClickUp(Vector3 cursor_pos)
  {
    base.OnLeftClickUp(cursor_pos);
    this.StopSound();
  }

  private void OnElementChanged(object new_element) => this.clearVisitedCells();

  protected override string GetDragSound()
  {
    return $"SandboxTool_Brush_{(ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")].state & Element.State.Solid).ToString()}_Add";
  }

  protected override void PlaySound()
  {
    base.PlaySound();
    Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
    string path;
    switch (element.state & Element.State.Solid)
    {
      case Element.State.Vacuum:
        path = GlobalAssets.GetSound("SandboxTool_Brush_Gas");
        break;
      case Element.State.Gas:
        path = GlobalAssets.GetSound("SandboxTool_Brush_Gas");
        break;
      case Element.State.Liquid:
        path = GlobalAssets.GetSound("SandboxTool_Brush_Liquid");
        break;
      case Element.State.Solid:
        path = GlobalAssets.GetSound("Brush_" + element.substance.GetOreBumpSound()) ?? GlobalAssets.GetSound("Brush_Rock");
        break;
      default:
        path = GlobalAssets.GetSound("Brush_Rock");
        break;
    }
    this.audioEvent = KFMOD.CreateInstance(path);
    int num1 = (int) this.audioEvent.set3DAttributes(SoundListenerController.Instance.transform.GetPosition().To3DAttributes());
    int num2 = (int) this.audioEvent.start();
  }

  private void StopSound()
  {
    int num1 = (int) this.audioEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    int num2 = (int) this.audioEvent.release();
  }
}
