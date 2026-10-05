// Decompiled with JetBrains decompiler
// Type: ClusterTelescopeEnclosedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class ClusterTelescopeEnclosedConfig : IBuildingConfig
{
  public const string ID = "ClusterTelescopeEnclosed";
  public const int SCAN_RADIUS = 4;
  public const int VERTICAL_SCAN_OFFSET = 3;
  public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ClusterTelescopeEnclosed", 4, 6, "telescope_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseClusterTelescopeEnclosed.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
    go.AddOrGetDef<PoweredController.Def>();
    ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
    def.clearScanCellRadius = 4;
    def.analyzeClusterRadius = 4;
    def.workableOverrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_telescope_kanim")
    };
    def.skyVisibilityInfo = ClusterTelescopeEnclosedConfig.SKY_VISIBILITY_INFO;
    def.providesOxygen = true;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 1000f;
    storage.showInUI = true;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.capacityKG = 10f;
    conduitConsumer.forceAlwaysSatisfied = true;
    ManualCodexConversionRegistry.AddConversion((Tag) (string) null, 0.0f, (Tag) "ClusterTelescopeEnclosed", 0.0f, (Tag) DatabankHelper.ID, 3f, (string) STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, outputCustomFormating: (Func<Tag, float, bool, string>) ((tag, amount, continuous) => string.Format((string) UI.BUILDINGEFFECTS.PER_STARMAP_HEX, (object) GameUtil.GetFormattedInt((float) (int) amount))));
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    ClusterTelescopeEnclosedConfig.AddVisualizer(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    ClusterTelescopeEnclosedConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    ClusterTelescopeEnclosedConfig.AddVisualizer(go);
  }

  private static void AddVisualizer(GameObject prefab)
  {
    SkyVisibilityVisualizer visibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
    visibilityVisualizer.OriginOffset.y = 3;
    visibilityVisualizer.TwoWideOrgin = true;
    visibilityVisualizer.RangeMin = -4;
    visibilityVisualizer.RangeMax = 5;
    visibilityVisualizer.SkipOnModuleInteriors = true;
  }
}
