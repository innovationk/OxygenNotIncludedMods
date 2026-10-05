// Decompiled with JetBrains decompiler
// Type: PlanStamp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/PlanStamp")]
public class PlanStamp : KMonoBehaviour
{
  public PlanStamp.StampArt stampSprites;
  [SerializeField]
  private Image StampImage;
  [SerializeField]
  private Text StampText;

  public void SetStamp(Sprite sprite, string Text)
  {
    this.StampImage.sprite = sprite;
    this.StampText.text = Text.ToUpper();
  }

  [Serializable]
  public struct StampArt
  {
    public Sprite UnderConstruction;
    public Sprite NeedsResearch;
    public Sprite SelectResource;
    public Sprite NeedsRepair;
    public Sprite NeedsPower;
    public Sprite NeedsResource;
    public Sprite NeedsGasPipe;
    public Sprite NeedsLiquidPipe;
  }
}
