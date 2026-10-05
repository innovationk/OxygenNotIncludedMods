// Decompiled with JetBrains decompiler
// Type: GraphBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/GraphBase")]
public class GraphBase : KMonoBehaviour
{
  [Header("Axis")]
  public GraphAxis axis_x;
  public GraphAxis axis_y;
  [Header("References")]
  public GameObject prefab_guide_x;
  public GameObject prefab_guide_y;
  public GameObject prefab_guide_horizontal_label;
  public GameObject prefab_guide_vertical_label;
  public GameObject guides_x;
  public GameObject guides_y;
  public LocText label_title;
  public LocText label_x;
  public LocText label_y;
  public string graphName;
  protected List<GameObject> horizontalGuides = new List<GameObject>();
  protected List<GameObject> verticalGuides = new List<GameObject>();
  private const int points_per_guide_line = 2;

  public Vector2 GetRelativePosition(Vector2 absolute_point)
  {
    Vector2 zero = Vector2.zero;
    float num1 = Mathf.Max(1f, this.axis_x.max_value - this.axis_x.min_value);
    float num2 = absolute_point.x - this.axis_x.min_value;
    zero.x = num2 / num1;
    float num3 = Mathf.Max(1f, this.axis_y.max_value - this.axis_y.min_value);
    float num4 = absolute_point.y - this.axis_y.min_value;
    zero.y = num4 / num3;
    return zero;
  }

  public Vector2 GetRelativeSize(Vector2 absolute_size) => this.GetRelativePosition(absolute_size);

  public void ClearGuides()
  {
    this.ClearVerticalGuides();
    this.ClearHorizontalGuides();
  }

  public void ClearHorizontalGuides()
  {
    foreach (GameObject horizontalGuide in this.horizontalGuides)
    {
      if ((Object) horizontalGuide != (Object) null)
        Object.DestroyImmediate((Object) horizontalGuide);
    }
    this.horizontalGuides.Clear();
  }

  public void ClearVerticalGuides()
  {
    foreach (GameObject verticalGuide in this.verticalGuides)
    {
      if ((Object) verticalGuide != (Object) null)
        Object.DestroyImmediate((Object) verticalGuide);
    }
    this.verticalGuides.Clear();
  }

  public void RefreshGuides()
  {
    this.ClearGuides();
    this.RefreshHorizontalGuides();
    this.RefreshVerticalGuides();
  }

  public void RefreshHorizontalGuides()
  {
    if (!((Object) this.prefab_guide_x != (Object) null))
      return;
    GameObject parent = Util.KInstantiateUI(this.prefab_guide_x, this.guides_x, true);
    parent.name = "guides_horizontal";
    Vector2[] vector2Array = new Vector2[2 * ((double) this.axis_y.guide_frequency > 0.0 ? (int) ((double) this.axis_y.range / (double) this.axis_y.guide_frequency) : 0)];
    for (int index = 0; index < vector2Array.Length; index += 2)
    {
      Vector2 absolute_point1 = new Vector2(this.axis_x.min_value, (float) index * (this.axis_y.guide_frequency / 2f));
      vector2Array[index] = this.GetRelativePosition(absolute_point1);
      Vector2 absolute_point2 = new Vector2(this.axis_x.max_value, (float) index * (this.axis_y.guide_frequency / 2f));
      vector2Array[index + 1] = this.GetRelativePosition(absolute_point2);
      if ((Object) this.prefab_guide_horizontal_label != (Object) null)
      {
        GameObject go = Util.KInstantiateUI(this.prefab_guide_horizontal_label, parent, true);
        go.GetComponent<LocText>().alignment = TextAlignmentOptions.MidlineLeft;
        go.GetComponent<LocText>().text = ((int) this.axis_y.guide_frequency * (index / 2)).ToString();
        go.rectTransform().SetLocalPosition((Vector3) (new Vector2(8f, (float) index * (this.gameObject.rectTransform().rect.height / (float) vector2Array.Length)) - this.gameObject.rectTransform().rect.size / 2f));
      }
    }
    parent.GetComponent<UILineRenderer>().Points = vector2Array;
    this.horizontalGuides.Add(parent);
  }

  public void RefreshVerticalGuides()
  {
    if (!((Object) this.prefab_guide_y != (Object) null))
      return;
    GameObject parent = Util.KInstantiateUI(this.prefab_guide_y, this.guides_y, true);
    parent.name = "guides_vertical";
    Vector2[] vector2Array = new Vector2[2 * ((double) this.axis_x.guide_frequency > 0.0 ? (int) ((double) this.axis_x.range / (double) this.axis_x.guide_frequency) : 0)];
    for (int index = 0; index < vector2Array.Length; index += 2)
    {
      Vector2 absolute_point1 = new Vector2((float) index * (this.axis_x.guide_frequency / 2f), this.axis_y.min_value);
      vector2Array[index] = this.GetRelativePosition(absolute_point1);
      Vector2 absolute_point2 = new Vector2((float) index * (this.axis_x.guide_frequency / 2f), this.axis_y.max_value);
      vector2Array[index + 1] = this.GetRelativePosition(absolute_point2);
      if ((Object) this.prefab_guide_vertical_label != (Object) null)
      {
        GameObject go = Util.KInstantiateUI(this.prefab_guide_vertical_label, parent, true);
        go.GetComponent<LocText>().alignment = TextAlignmentOptions.Bottom;
        go.GetComponent<LocText>().text = ((int) this.axis_x.guide_frequency * (index / 2)).ToString();
        go.rectTransform().SetLocalPosition((Vector3) (new Vector2((float) index * (this.gameObject.rectTransform().rect.width / (float) vector2Array.Length), 4f) - this.gameObject.rectTransform().rect.size / 2f));
      }
    }
    parent.GetComponent<UILineRenderer>().Points = vector2Array;
    this.verticalGuides.Add(parent);
  }
}
