// Decompiled with JetBrains decompiler
// Type: SpaceHeaterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class SpaceHeaterConfig : IBuildingConfig
{
  public const string ID = "SpaceHeater";
  public const float MAX_SELF_HEAT = 32f;
  public const float MAX_EXHAUST_HEAT = 4f;
  public const float MIN_POWER_USAGE = 120f;
  public const float MAX_POWER_USAGE = 240f;
  public static Vector2I MAX_RANGE = new Vector2I(5, 5);
  public static Vector2I MIN_RANGE = new Vector2I(-4, -4);

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SpaceHeater", 2, 2, "spaceheater_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
    buildingDef.ViewMode = OverlayModes.Temperature.ID;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.OverheatTemperature = 398.15f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WarmingStation);
    go.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
    SpaceHeater spaceHeater = go.AddOrGet<SpaceHeater>();
    spaceHeater.targetTemperature = 343.15f;
    spaceHeater.produceHeat = true;
    spaceHeater.maxPower = 240f;
    spaceHeater.minPower = 120f;
    spaceHeater.maxSelfHeatKWs = 32f;
    spaceHeater.maxExhaustedKWs = 4f;
    WarmthProvider.Def def = go.AddOrGetDef<WarmthProvider.Def>();
    def.RangeMax = SpaceHeaterConfig.MAX_RANGE;
    def.RangeMin = SpaceHeaterConfig.MIN_RANGE;
    go.AddOrGetDef<ColdImmunityProvider.Def>().range = new CellOffset[2][]
    {
      new CellOffset[2]
      {
        new CellOffset(-1, 0),
        new CellOffset(2, 0)
      },
      new CellOffset[2]
      {
        new CellOffset(0, 0),
        new CellOffset(1, 0)
      }
    };
    this.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go) => this.AddVisualizer(go);

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    this.AddVisualizer(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGetDef<PoweredActiveController.Def>();
  }

  private void AddVisualizer(GameObject go)
  {
    RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
    rangeVisualizer.RangeMax = SpaceHeaterConfig.MAX_RANGE;
    rangeVisualizer.RangeMin = SpaceHeaterConfig.MIN_RANGE;
    rangeVisualizer.BlockingTileVisible = false;
    go.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, new CellOffset());
  }
}
