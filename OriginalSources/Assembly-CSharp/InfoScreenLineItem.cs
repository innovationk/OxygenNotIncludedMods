// Decompiled with JetBrains decompiler
// Type: InfoScreenLineItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenLineItem")]
public class InfoScreenLineItem : KMonoBehaviour
{
  [SerializeField]
  private LocText locText;
  [SerializeField]
  private ToolTip toolTip;
  private string text;
  private string tooltip;

  public void SetText(string text) => this.locText.text = text;

  public void SetTooltip(string tooltip) => this.toolTip.toolTip = tooltip;
}
