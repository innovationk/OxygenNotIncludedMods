// Decompiled with JetBrains decompiler
// Type: GenericUIProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/GenericUIProgressBar")]
public class GenericUIProgressBar : KMonoBehaviour
{
  public Image fill;
  public LocText label;
  private float maxValue;

  public void SetMaxValue(float max) => this.maxValue = max;

  public void SetFillPercentage(float value)
  {
    this.fill.fillAmount = value;
    this.label.text = $"{Util.FormatWholeNumber(Mathf.Min(this.maxValue, this.maxValue * value))}/{this.maxValue.ToString()}";
  }
}
