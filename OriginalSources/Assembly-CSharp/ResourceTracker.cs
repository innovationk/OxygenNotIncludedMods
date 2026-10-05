// Decompiled with JetBrains decompiler
// Type: ResourceTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ResourceTracker : WorldTracker
{
  public Tag tag { get; private set; }

  public ResourceTracker(int worldID, Tag materialCategoryTag)
    : base(worldID)
  {
    this.tag = materialCategoryTag;
  }

  public override void UpdateData()
  {
    if ((Object) ClusterManager.Instance.GetWorld(this.WorldID).worldInventory == (Object) null)
      return;
    this.AddPoint(ClusterManager.Instance.GetWorld(this.WorldID).worldInventory.GetAmount(this.tag, false));
  }

  public override string FormatValueString(float value) => GameUtil.GetFormattedMass(value);
}
