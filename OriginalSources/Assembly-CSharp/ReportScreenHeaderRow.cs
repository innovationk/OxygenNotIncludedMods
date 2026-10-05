// Decompiled with JetBrains decompiler
// Type: ReportScreenHeaderRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeaderRow")]
public class ReportScreenHeaderRow : KMonoBehaviour
{
  [SerializeField]
  public LocText name;
  [SerializeField]
  private LayoutElement spacer;
  [SerializeField]
  private Image bgImage;
  public float groupSpacerWidth;
  private float nameWidth = 164f;
  [SerializeField]
  private Color oddRowColor;

  public void SetLine(ReportManager.ReportGroup reportGroup)
  {
    LayoutElement component = this.name.GetComponent<LayoutElement>();
    double nameWidth;
    float num = (float) (nameWidth = (double) this.nameWidth);
    component.preferredWidth = (float) nameWidth;
    component.minWidth = num;
    this.spacer.minWidth = this.groupSpacerWidth;
    this.name.text = reportGroup.stringKey;
  }
}
