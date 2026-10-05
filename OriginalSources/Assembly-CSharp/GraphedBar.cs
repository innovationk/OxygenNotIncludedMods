// Decompiled with JetBrains decompiler
// Type: GraphedBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/GraphedBar")]
[Serializable]
public class GraphedBar : KMonoBehaviour
{
  public GameObject segments_container;
  public GameObject prefab_segment;
  private List<GameObject> segments = new List<GameObject>();
  private GraphedBarFormatting format;

  public void SetFormat(GraphedBarFormatting format) => this.format = format;

  public void SetValues(int[] values, float x_position)
  {
    this.ClearValues();
    this.gameObject.rectTransform().anchorMin = new Vector2(x_position, 0.0f);
    this.gameObject.rectTransform().anchorMax = new Vector2(x_position, 1f);
    this.gameObject.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float) this.format.width);
    for (int index = 0; index < values.Length; ++index)
    {
      GameObject gameObject = Util.KInstantiateUI(this.prefab_segment, this.segments_container, true);
      LayoutElement component = gameObject.GetComponent<LayoutElement>();
      component.preferredHeight = (float) values[index];
      component.minWidth = (float) this.format.width;
      gameObject.GetComponent<Image>().color = this.format.colors[index % this.format.colors.Length];
      this.segments.Add(gameObject);
    }
  }

  public void ClearValues()
  {
    foreach (UnityEngine.Object segment in this.segments)
      UnityEngine.Object.DestroyImmediate(segment);
    this.segments.Clear();
  }
}
