// Decompiled with JetBrains decompiler
// Type: DevToolWarmthZonesVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DevToolWarmthZonesVisualizer : DevTool
{
  private const int MAX_COLOR_VARIANTS = 3;
  private Color WARM_CELL_COLOR = Color.red;
  private Color[] colors;

  private void SetupColors()
  {
    if (this.colors != null)
      return;
    this.colors = new Color[3];
    for (int warmValue = 1; warmValue <= 3; ++warmValue)
      this.colors[warmValue - 1] = this.CreateColorForWarmthValue(warmValue);
  }

  private Color CreateColorForWarmthValue(int warmValue)
  {
    return (this.WARM_CELL_COLOR * ((float) Mathf.Clamp(warmValue, 1, 3) / 3f)) with
    {
      a = this.WARM_CELL_COLOR.a
    };
  }

  private Color GetBorderColor(int warmValue) => this.colors[Mathf.Clamp(warmValue, 0, 3)];

  private Color GetFillColor(int warmValue)
  {
    return this.GetBorderColor(warmValue) with { a = 0.3f };
  }

  protected override void RenderTo(DevPanel panel)
  {
    this.SetupColors();
    foreach (int key in WarmthProvider.WarmCells.Keys)
    {
      if (Grid.IsValidCell(key) && WarmthProvider.IsWarmCell(key))
      {
        int warmthValue = WarmthProvider.GetWarmthValue(key);
        Option<(Vector2, Vector2)> screenRect = new DevToolEntityTarget.ForSimCell(key).GetScreenRect();
        string text = warmthValue.ToString();
        DevToolEntity.DrawScreenRect(screenRect.Unwrap(), (Option<string>) text, (Option<Color>) this.GetBorderColor(warmthValue - 1), (Option<Color>) this.GetFillColor(warmthValue - 1), new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment.Center));
      }
    }
  }
}
