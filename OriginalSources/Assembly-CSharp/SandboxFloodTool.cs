// Decompiled with JetBrains decompiler
// Type: SandboxFloodTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxFloodTool : FloodTool
{
  public static SandboxFloodTool instance;
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  protected List<int> cellsToAffect = new List<int>();
  protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
  private EventInstance ev;

  public static void DestroyInstance() => SandboxFloodTool.instance = (SandboxFloodTool) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxFloodTool.instance = this;
    this.floodCriteria = (Func<int, FloodFill.BoundaryCheckResult>) (cell => !Grid.IsValidCell(cell) || Grid.Element[cell] != Grid.Element[this.mouseCell] || (int) Grid.WorldIdx[cell] != (int) Grid.WorldIdx[this.mouseCell] ? FloodFill.BoundaryCheckResult.Halt : FloodFill.BoundaryCheckResult.Continue);
    this.paintArea = (Action<List<int>>) (cells =>
    {
      foreach (int cell in cells)
        this.PaintCell(cell);
    });
  }

  private void PaintCell(int cell)
  {
    this.recentlyAffectedCells.Add(cell);
    Game.CallbackInfo callbackInfo = new Game.CallbackInfo((System.Action) (() => this.recentlyAffectedCells.Remove(cell)));
    Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
    byte index1 = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
    Klei.AI.Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
    if (disease != null)
      index1 = Db.Get().Diseases.GetIndex(disease.id);
    int index2 = Game.Instance.callbackManager.Add(callbackInfo).index;
    int gameCell = cell;
    int id = (int) element.id;
    CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
    double floatSetting1 = (double) this.settings.GetFloatSetting("SandboxTools.Mass");
    double floatSetting2 = (double) this.settings.GetFloatSetting("SandbosTools.Temperature");
    int num = index2;
    int diseaseIdx = (int) index1;
    int intSetting = this.settings.GetIntSetting("SandboxTools.DiseaseCount");
    int callbackIdx = num;
    SimMessages.ReplaceElement(gameCell, (SimHashes) id, sandBoxTool, (float) floatSetting1, (float) floatSetting2, (byte) diseaseIdx, intSetting, callbackIdx);
  }

  private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
    SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
    int num = (int) this.ev.release();
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int recentlyAffectedCell in this.recentlyAffectedCells)
      colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, this.recentlyAffectedCellColor));
    foreach (int cell in this.cellsToAffect)
      colors.Add(new ToolMenu.CellColorData(cell, (Color) this.areaColour));
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    this.cellsToAffect = this.Flood(Grid.PosToCell(cursorPos));
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
    this.ev = KFMOD.CreateInstance(!element.IsSolid ? (!element.IsGas ? (!element.IsLiquid ? GlobalAssets.GetSound("Break_Rock") : GlobalAssets.GetSound("SandboxTool_Bucket_Liquid")) : GlobalAssets.GetSound("SandboxTool_Bucket_Gas")) : GlobalAssets.GetSound("Break_" + element.substance.GetMiningBreakSound()) ?? GlobalAssets.GetSound("Break_Rock"));
    int num1 = (int) this.ev.set3DAttributes(SoundListenerController.Instance.transform.GetPosition().To3DAttributes());
    int num2 = (int) this.ev.setParameterByName("SandboxToggle", 1f);
    int num3 = (int) this.ev.start();
    KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Bucket"));
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
}
