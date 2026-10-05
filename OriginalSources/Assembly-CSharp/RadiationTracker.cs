// Decompiled with JetBrains decompiler
// Type: RadiationTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

#nullable disable
public class RadiationTracker(int worldID) : WorldTracker(worldID)
{
  public override void UpdateData()
  {
    float num = 0.0f;
    List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(this.WorldID);
    if (worldItems.Count == 0)
    {
      this.AddPoint(0.0f);
    }
    else
    {
      foreach (MinionIdentity cmp in worldItems)
        num += cmp.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value;
      this.AddPoint(num / (float) worldItems.Count);
    }
  }

  public override string FormatValueString(float value) => GameUtil.GetFormattedRads(value);
}
