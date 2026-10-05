// Decompiled with JetBrains decompiler
// Type: WorkingToiletTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class WorkingToiletTracker(int worldID) : WorldTracker(worldID)
{
  public override void UpdateData()
  {
    int num = 0;
    foreach (IUsable usable in Components.Toilets.WorldItemsEnumerate(this.WorldID, true))
    {
      if (usable.IsUsable())
        ++num;
    }
    this.AddPoint((float) num);
  }

  public override string FormatValueString(float value) => value.ToString();
}
