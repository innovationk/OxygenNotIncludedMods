// Decompiled with JetBrains decompiler
// Type: TMPro.Examples.Benchmark01
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace TMPro.Examples;

public class Benchmark01 : MonoBehaviour
{
  public int BenchmarkType;
  public TMP_FontAsset TMProFont;
  public Font TextMeshFont;
  private TextMeshPro m_textMeshPro;
  private TextContainer m_textContainer;
  private TextMesh m_textMesh;
  private const string label01 = "The <#0050FF>count is: </color>{0}";
  private const string label02 = "The <color=#0050FF>count is: </color>";
  private Material m_material01;
  private Material m_material02;

  private IEnumerator Start()
  {
    if (this.BenchmarkType == 0)
    {
      this.m_textMeshPro = this.gameObject.AddComponent<TextMeshPro>();
      this.m_textMeshPro.autoSizeTextContainer = true;
      if ((Object) this.TMProFont != (Object) null)
        this.m_textMeshPro.font = this.TMProFont;
      this.m_textMeshPro.fontSize = 48f;
      this.m_textMeshPro.alignment = TextAlignmentOptions.Center;
      this.m_textMeshPro.extraPadding = true;
      this.m_textMeshPro.textWrappingMode = TextWrappingModes.NoWrap;
      this.m_material01 = this.m_textMeshPro.font.material;
      this.m_material02 = UnityEngine.Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Drop Shadow");
    }
    else if (this.BenchmarkType == 1)
    {
      this.m_textMesh = this.gameObject.AddComponent<TextMesh>();
      if ((Object) this.TextMeshFont != (Object) null)
      {
        this.m_textMesh.font = this.TextMeshFont;
        this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
      }
      else
      {
        this.m_textMesh.font = UnityEngine.Resources.Load("Fonts/ARIAL", typeof (Font)) as Font;
        this.m_textMesh.GetComponent<Renderer>().sharedMaterial = this.m_textMesh.font.material;
      }
      this.m_textMesh.fontSize = 48 /*0x30*/;
      this.m_textMesh.anchor = TextAnchor.MiddleCenter;
    }
    for (int i = 0; i <= 1000000; ++i)
    {
      if (this.BenchmarkType == 0)
      {
        this.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0}", (float) (i % 1000));
        if (i % 1000 == 999)
          this.m_textMeshPro.fontSharedMaterial = (Object) this.m_textMeshPro.fontSharedMaterial == (Object) this.m_material01 ? (this.m_textMeshPro.fontSharedMaterial = this.m_material02) : (this.m_textMeshPro.fontSharedMaterial = this.m_material01);
      }
      else if (this.BenchmarkType == 1)
        this.m_textMesh.text = "The <color=#0050FF>count is: </color>" + (i % 1000).ToString();
      yield return (object) null;
    }
    yield return (object) null;
  }
}
