// Decompiled with JetBrains decompiler
// Type: BuildingSubmergable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class BuildingSubmergable : Submergable
{
  public static Operational.Flag notSubmergedFlag = new Operational.Flag("submerged", Operational.Flag.Type.Functional);
  [MyCmpReq]
  private Operational operational;

  private static StatusItem GetSubmergableStatusItem() => Db.Get().BuildingStatusItems.NotSubmerged;

  protected override void OnSpawn()
  {
    this.operational.SetFlag(BuildingSubmergable.notSubmergedFlag, this.isSubmerged);
    this.GetStatusItem = new Func<StatusItem>(BuildingSubmergable.GetSubmergableStatusItem);
    base.OnSpawn();
  }

  protected override void OnSubmergedStateChanged()
  {
    this.operational.SetFlag(BuildingSubmergable.notSubmergedFlag, this.isSubmerged);
    base.OnSubmergedStateChanged();
  }
}
