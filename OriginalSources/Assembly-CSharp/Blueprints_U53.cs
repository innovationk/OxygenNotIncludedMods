// Decompiled with JetBrains decompiler
// Type: Blueprints_U53
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;

#nullable disable
public class Blueprints_U53 : BlueprintProvider
{
  public override void SetupBlueprints()
  {
    this.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_hatch", "elegantbed_hatch_kanim");
    this.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_pipsqueak", "elegantbed_pipsqueak_kanim");
  }
}
