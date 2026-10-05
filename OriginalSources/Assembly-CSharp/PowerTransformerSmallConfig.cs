// Decompiled with JetBrains decompiler
// Type: PowerTransformerSmallConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class PowerTransformerSmallConfig : IBuildingConfig
{
  public const string ID = "PowerTransformerSmall";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMetals = MATERIALS.RAW_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues tieR1 = BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("PowerTransformerSmall", 2, 2, "transformer_small_kanim", 30, 30f, tieR3, rawMetals, 800f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.RequiresPowerOutput = true;
    buildingDef.PowerInputOffset = new CellOffset(0, 1);
    buildingDef.PowerOutputOffset = new CellOffset(1, 0);
    buildingDef.ElectricalArrowOffset = new CellOffset(1, 0);
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.Entombable = true;
    buildingDef.GeneratorWattageRating = 1000f;
    buildingDef.GeneratorBaseCapacity = 1000f;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
    go.AddComponent<RequireInputs>();
    BuildingDef def = go.GetComponent<Building>().Def;
    Battery battery = go.AddOrGet<Battery>();
    battery.powerSortOrder = 1000;
    battery.capacity = def.GeneratorWattageRating;
    battery.chargeWattage = def.GeneratorWattageRating;
    go.AddComponent<PowerTransformer>().powerDistributionOrder = 9;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<EnergyConsumer>());
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
