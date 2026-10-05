// Decompiled with JetBrains decompiler
// Type: LimitValveTuning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class LimitValveTuning
{
  public const float MAX_LIMIT = 500f;
  public const float DEFAULT_LIMIT = 100f;

  public static NonLinearSlider.Range[] GetDefaultSlider()
  {
    return new NonLinearSlider.Range[2]
    {
      new NonLinearSlider.Range(70f, 100f),
      new NonLinearSlider.Range(30f, 500f)
    };
  }
}
