// Decompiled with JetBrains decompiler
// Type: GantryConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GantryConfig : IBuildingConfig
{
  public const string ID = "Gantry";
  private static readonly CellOffset[] SOLID_OFFSETS = new CellOffset[2]
  {
    new CellOffset(-2, 1),
    new CellOffset(-1, 1)
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Gantry", 6, 2, "gantry_kanim", 30, 30f, tieR3, construction_materials, 3200f, BuildLocationRule.Anywhere, none2, noise, 1f);
    buildingDef.ObjectLayer = ObjectLayer.Gantry;
    buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    buildingDef.Entombable = true;
    buildingDef.IsFoundation = false;
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(-2, 0);
    buildingDef.EnergyConsumptionWhenActive = 1200f;
    buildingDef.ExhaustKilowattsWhenActive = 1f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.AudioCategory = "Metal";
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(Gantry.PORT_ID, new CellOffset(-1, 1), (string) STRINGS.BUILDINGS.PREFABS.GANTRY.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.GANTRY.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.GANTRY.LOGIC_PORT_INACTIVE)
    };
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.GetComponent<KPrefabID>().prefabSpawnFn += new KPrefabID.PrefabFn(this.OnSpawn);
  }

  private void OnSpawn(GameObject instance)
  {
    instance.GetComponent<KBatchedAnimController>().SetFGLayer(Grid.SceneLayer.TileMain);
    foreach (KBatchedAnimController kbatchedAnimController in instance.GetComponentsInChildrenOnly<KBatchedAnimController>())
    {
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.LiquidVisibilityLayer, false);
      kbatchedAnimController.SetBlendValue(KBatchedAnimInstanceData.BlendActiveOptions.WaterProof, true);
    }
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<Gantry>();
    go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = GantryConfig.SOLID_OFFSETS;
    FakeFloorAdder fakeFloorAdder = go.AddOrGet<FakeFloorAdder>();
    fakeFloorAdder.floorOffsets = new CellOffset[4]
    {
      new CellOffset(0, 1),
      new CellOffset(1, 1),
      new CellOffset(2, 1),
      new CellOffset(3, 1)
    };
    fakeFloorAdder.initiallyActive = false;
    Object.DestroyImmediate((Object) go.GetComponent<LogicOperationalController>());
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = GantryConfig.SOLID_OFFSETS;
  }
}
