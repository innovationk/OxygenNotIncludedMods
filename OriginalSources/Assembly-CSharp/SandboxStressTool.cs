// Decompiled with JetBrains decompiler
// Type: SandboxStressTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxStressTool : BrushTool
{
  public static SandboxStressTool instance;
  protected HashSet<int> recentlyAffectedCells = new HashSet<int>();
  protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
  private string UISoundPath = GlobalAssets.GetSound("SandboxTool_Happy");
  private EventInstance ev;
  private Dictionary<MinionIdentity, AttributeModifier> moraleAdjustments = new Dictionary<MinionIdentity, AttributeModifier>();

  public static void DestroyInstance() => SandboxStressTool.instance = (SandboxStressTool) null;

  private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    SandboxStressTool.instance = this;
  }

  protected override string GetDragSound() => "";

  public void Activate() => PlayerController.Instance.ActivateTool((InterfaceTool) this);

  protected override void OnActivateTool()
  {
    base.OnActivateTool();
    SandboxToolParameterMenu.instance.gameObject.SetActive(true);
    SandboxToolParameterMenu.instance.DisableParameters();
    SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.stressAdditiveSlider.row.SetActive(true);
    SandboxToolParameterMenu.instance.stressAdditiveSlider.SetValue(5f);
    SandboxToolParameterMenu.instance.moraleSlider.SetValue(0.0f);
    if (!DebugHandler.InstantBuildMode)
      return;
    SandboxToolParameterMenu.instance.moraleSlider.row.SetActive(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    SandboxToolParameterMenu.instance.gameObject.SetActive(false);
    this.StopSound();
  }

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    foreach (int recentlyAffectedCell in this.recentlyAffectedCells)
      colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, this.recentlyAffectedCellColor));
    foreach (int cellsInRadiu in this.cellsInRadius)
      colors.Add(new ToolMenu.CellColorData(cellsInRadiu, this.radiusIndicatorColor));
  }

  public override void OnMouseMove(Vector3 cursorPos) => base.OnMouseMove(cursorPos);

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    base.OnLeftClickDown(cursor_pos);
    KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click"));
  }

  protected override void OnPaintCell(int cell, int distFromOrigin)
  {
    base.OnPaintCell(cell, distFromOrigin);
    for (int idx = 0; idx < Components.LiveMinionIdentities.Count; ++idx)
    {
      GameObject gameObject = Components.LiveMinionIdentities[idx].gameObject;
      if (Grid.PosToCell(gameObject) == cell)
      {
        float num1 = -1f * SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.StressAdditive");
        double num2 = (double) Db.Get().Amounts.Stress.Lookup(Components.LiveMinionIdentities[idx].gameObject).ApplyDelta(num1);
        if ((double) num1 >= 0.0)
          PopFXManager.Instance.SpawnFX(Assets.GetSprite((HashedString) "crew_state_angry"), GameUtil.GetFormattedPercent(num1), gameObject.transform);
        else
          PopFXManager.Instance.SpawnFX(Assets.GetSprite((HashedString) "crew_state_happy"), GameUtil.GetFormattedPercent(num1), gameObject.transform);
        this.PlaySound(num1, gameObject.transform.GetPosition());
        int intSetting = SandboxToolParameterMenu.instance.settings.GetIntSetting("SandbosTools.MoraleAdjustment");
        AttributeInstance attributeInstance = gameObject.GetAttributes().Get(Db.Get().Attributes.QualityOfLife);
        MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
        if (this.moraleAdjustments.ContainsKey(component))
        {
          attributeInstance.Remove(this.moraleAdjustments[component]);
          this.moraleAdjustments.Remove(component);
        }
        if (intSetting != 0)
        {
          AttributeModifier modifier = new AttributeModifier(attributeInstance.Id, (float) intSetting, (Func<string>) (() => (string) DUPLICANTS.MODIFIERS.SANDBOXMORALEADJUSTMENT.NAME));
          modifier.SetValue((float) intSetting);
          attributeInstance.Add(modifier);
          this.moraleAdjustments.Add(component, modifier);
        }
      }
    }
  }

  private void PlaySound(float sliderValue, Vector3 position)
  {
    this.ev = KFMOD.CreateInstance(this.UISoundPath);
    int num1 = (int) this.ev.set3DAttributes(position.To3DAttributes());
    int num2 = (int) this.ev.setParameterByNameWithLabel("SanboxTool_Effect", (double) sliderValue >= 0.0 ? "Decrease" : "Increase");
    int num3 = (int) this.ev.start();
  }

  private void StopSound()
  {
    int num1 = (int) this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    int num2 = (int) this.ev.release();
  }
}
