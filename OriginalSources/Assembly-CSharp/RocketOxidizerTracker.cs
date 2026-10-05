// Decompiled with JetBrains decompiler
// Type: RocketOxidizerTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RocketOxidizerTracker(int worldID) : WorldTracker(worldID)
{
  public override void UpdateData()
  {
    Clustercraft component = ClusterManager.Instance.GetWorld(this.WorldID).GetComponent<Clustercraft>();
    this.AddPoint((Object) component != (Object) null ? component.ModuleInterface.OxidizerPowerRemaining : 0.0f);
  }

  public override string FormatValueString(float value) => GameUtil.GetFormattedMass(value);
}
