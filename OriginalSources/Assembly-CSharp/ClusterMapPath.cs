// Decompiled with JetBrains decompiler
// Type: ClusterMapPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

#nullable disable
public class ClusterMapPath : MonoBehaviour
{
  private List<Vector2> m_nodes;
  private Color m_color;
  public UILineRenderer lineRenderer;
  public Image pathStart;
  public Image pathEnd;

  public void Init()
  {
    this.lineRenderer = this.gameObject.GetComponentInChildren<UILineRenderer>();
    this.gameObject.SetActive(true);
  }

  public void Init(List<Vector2> nodes, Color color)
  {
    this.m_nodes = nodes;
    this.m_color = color;
    this.lineRenderer = this.gameObject.GetComponentInChildren<UILineRenderer>();
    this.UpdateColor();
    this.UpdateRenderer();
    this.gameObject.SetActive(true);
  }

  public void SetColor(Color color)
  {
    this.m_color = color;
    this.UpdateColor();
  }

  private void UpdateColor()
  {
    this.lineRenderer.color = this.m_color;
    this.pathStart.color = this.m_color;
    this.pathEnd.color = this.m_color;
  }

  public void SetPoints(List<Vector2> points)
  {
    this.m_nodes = points;
    this.UpdateRenderer();
  }

  private void UpdateRenderer()
  {
    this.lineRenderer.Points = ProcGen.Util.GetPointsOnCatmullRomSpline(this.m_nodes, 10).ToArray<Vector2>();
    if (this.lineRenderer.Points.Length > 1)
    {
      this.pathStart.transform.localPosition = (Vector3) this.lineRenderer.Points[0];
      this.pathStart.gameObject.SetActive(true);
      Vector2 point1 = this.lineRenderer.Points[this.lineRenderer.Points.Length - 1];
      Vector2 point2 = this.lineRenderer.Points[this.lineRenderer.Points.Length - 2];
      this.pathEnd.transform.localPosition = (Vector3) point1;
      this.pathEnd.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector3) (point1 - point2));
      this.pathEnd.gameObject.SetActive(true);
    }
    else
    {
      this.pathStart.gameObject.SetActive(false);
      this.pathEnd.gameObject.SetActive(false);
    }
  }

  public float GetRotationForNextSegment()
  {
    return this.m_nodes.Count > 1 ? Vector2.SignedAngle(Vector2.up, this.m_nodes[1] - this.m_nodes[0]) : 0.0f;
  }
}
