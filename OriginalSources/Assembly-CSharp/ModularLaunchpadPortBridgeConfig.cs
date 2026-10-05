// Decompiled with JetBrains decompiler
// Type: ModularLaunchpadPortBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class ModularLaunchpadPortBridgeConfig : IBuildingConfig
{
  public const string ID = "ModularLaunchpadPortBridge";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ModularLaunchpadPortBridge", 1, 2, "rocket_loader_extension_kanim", 1000, 60f, tieR3, refinedMetals, 9999f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.DefaultAnimState = "idle";
    buildingDef.UseStructureTemperature = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "medium";
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    component.AddTag(GameTags.ModularConduitPort);
    component.AddTag(GameTags.NotRocketInteriorBuilding);
    component.AddTag(BaseModularLaunchpadPortConfig.LinkTag);
    ChainedBuilding.Def def = go.AddOrGetDef<ChainedBuilding.Def>();
    def.headBuildingTag = "LaunchPad".ToTag();
    def.linkBuildingTag = BaseModularLaunchpadPortConfig.LinkTag;
    def.objectLayer = ObjectLayer.Building;
    go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[1]
    {
      new CellOffset(0, 1)
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
