// Decompiled with JetBrains decompiler
// Type: ScoutModuleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ScoutModuleConfig : IBuildingConfig
{
  public const string ID = "ScoutModule";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] hollowTieR1 = TUNING.BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ScoutModule", 3, 3, "rocket_scout_cargo_module_kanim", 1000, 30f, hollowTieR1, rawMetals, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.DefaultAnimState = "deployed";
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    Storage storage = go.AddComponent<Storage>();
    storage.showInUI = true;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    BuildingInternalConstructor.Def def1 = go.AddOrGetDef<BuildingInternalConstructor.Def>();
    def1.constructionMass = 500f;
    def1.outputIDs = new List<string>()
    {
      "ScoutLander",
      "ScoutRover"
    };
    def1.spawnIntoStorage = true;
    def1.storage = (DefComponent<Storage>) storage;
    def1.constructionSymbol = "under_construction";
    go.AddOrGet<BuildingInternalConstructorWorkable>().SetWorkTime(30f);
    JettisonableCargoModule.Def def2 = go.AddOrGetDef<JettisonableCargoModule.Def>();
    def2.landerPrefabID = "ScoutLander".ToTag();
    def2.landerContainer = (DefComponent<Storage>) storage;
    def2.clusterMapFXPrefabID = "DeployingScoutLanderFXConfig";
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Prioritizable.AddRef(go);
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, TUNING.ROCKETRY.BURDEN.MODERATE);
  }
}
