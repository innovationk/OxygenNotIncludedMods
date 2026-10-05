// Decompiled with JetBrains decompiler
// Type: ResearchTreeTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ResearchTreeTitle : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  private LocText treeLabel;
  [SerializeField]
  private Image BG;

  public void SetLabel(string txt) => this.treeLabel.text = txt;

  public void SetColor(int id) => this.BG.enabled = id % 2 != 0;
}
