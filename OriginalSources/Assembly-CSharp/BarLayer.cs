// Decompiled with JetBrains decompiler
// Type: BarLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BarLayer : GraphLayer
{
  public GameObject bar_container;
  public GameObject prefab_bar;
  public GraphedBarFormatting[] bar_formats;
  private List<GraphedBar> bars = new List<GraphedBar>();

  public int bar_count => this.bars.Count;

  public void NewBar(int[] values, float x_position, string ID = "")
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab_bar, this.bar_container, true);
    if (ID == "")
      ID = this.bars.Count.ToString();
    gameObject.name = $"bar_{ID}";
    GraphedBar component = gameObject.GetComponent<GraphedBar>();
    component.SetFormat(this.bar_formats[this.bars.Count % this.bar_formats.Length]);
    int[] values1 = new int[values.Length];
    for (int index = 0; index < values.Length; ++index)
      values1[index] = (int) ((double) this.graph.rectTransform().rect.height * (double) this.graph.GetRelativeSize(new Vector2(0.0f, (float) values[index])).y);
    component.SetValues(values1, this.graph.GetRelativePosition(new Vector2(x_position, 0.0f)).x);
    this.bars.Add(component);
  }

  public void ClearBars()
  {
    foreach (GraphedBar bar in this.bars)
    {
      if ((Object) bar != (Object) null && (Object) bar.gameObject != (Object) null)
        Object.DestroyImmediate((Object) bar.gameObject);
    }
    this.bars.Clear();
  }
}
