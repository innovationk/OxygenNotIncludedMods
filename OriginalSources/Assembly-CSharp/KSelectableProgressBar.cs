// Decompiled with JetBrains decompiler
// Type: KSelectableProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class KSelectableProgressBar : KSelectable
{
  [MyCmpGet]
  private ProgressBar progressBar;
  private int scaleAmount = 100;

  public override string GetName()
  {
    return $"{this.entityName} {(int) ((double) this.progressBar.PercentFull * (double) this.scaleAmount)}/{this.scaleAmount}";
  }
}
