// Decompiled with JetBrains decompiler
// Type: SandboxSampleTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SandboxSampleTool : InterfaceTool
{
  protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);
  private int currentCell;
  private EventInstance ev;

  public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
  {
    colors = new HashSet<ToolMenu.CellColorData>();
    colors.Add(new ToolMenu.CellColorData(this.currentCell, this.radiusIndicatorColor));
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    this.currentCell = Grid.PosToCell(cursorPos);
  }

  public override void OnLeftClickDown(Vector3 cursor_pos)
  {
    int cell = Grid.PosToCell(cursor_pos);
    if (!Grid.IsValidCell(cell))
    {
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.DEBUG_TOOLS.INVALID_LOCATION, (Transform) null, cursor_pos, force_spawn: true);
    }
    else
    {
      SandboxSampleTool.Sample(cell);
      KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click"));
      this.PlaySound();
    }
  }

  public static void Sample(int cell)
  {
    SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.SelectedElement", (int) Grid.Element[cell].idx);
    SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.Mass", Mathf.Round(Grid.Mass[cell] * 100f) / 100f);
    SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.Temperature", Mathf.Round(Grid.Temperature[cell] * 10f) / 10f);
    SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.DiseaseCount", Grid.DiseaseCount[cell]);
    SandboxToolParameterMenu.instance.RefreshDisplay();
  }

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
    this.StopSound();
  }

  private void PlaySound()
  {
    Element element = ElementLoader.elements[SandboxToolParameterMenu.instance.settings.GetIntSetting("SandboxTools.SelectedElement")];
    float volume = 1f;
    float pitch = 1f;
    string path = GlobalAssets.GetSound("Ore_bump_Rock");
    switch (element.state & Element.State.Solid)
    {
      case Element.State.Vacuum:
        path = GlobalAssets.GetSound("ConduitBlob_Gas");
        break;
      case Element.State.Gas:
        path = GlobalAssets.GetSound("ConduitBlob_Gas");
        break;
      case Element.State.Liquid:
        path = GlobalAssets.GetSound("ConduitBlob_Liquid");
        break;
      case Element.State.Solid:
        path = GlobalAssets.GetSound("Ore_bump_" + element.substance.GetMiningSound()) ?? GlobalAssets.GetSound("Ore_bump_Rock");
        volume = 0.7f;
        pitch = 2f;
        break;
    }
    this.ev = KFMOD.CreateInstance(path);
    int num1 = (int) this.ev.set3DAttributes(SoundListenerController.Instance.transform.GetPosition().To3DAttributes());
    int num2 = (int) this.ev.setVolume(volume);
    int num3 = (int) this.ev.setPitch(pitch);
    int num4 = (int) this.ev.setParameterByName("blobCount", (float) Random.Range(0, 6));
    int num5 = (int) this.ev.setParameterByName("SandboxToggle", 1f);
    int num6 = (int) this.ev.start();
  }

  private void StopSound()
  {
    int num1 = (int) this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    int num2 = (int) this.ev.release();
  }
}
