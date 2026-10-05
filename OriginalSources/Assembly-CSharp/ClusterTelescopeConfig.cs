// Decompiled with JetBrains decompiler
// Type: ClusterTelescopeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class ClusterTelescopeConfig : IBuildingConfig
{
  public const string ID = "ClusterTelescope";
  public const int SCAN_RADIUS = 4;
  public const int VERTICAL_SCAN_OFFSET = 1;
  public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 1), 4, new CellOffset(0, 1), 4, 0);
  public const int DATABANKS_PER_HEX = 3;

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ClusterTelescope", 3, 3, "telescope_low_kanim", 30, 30f, tieR4, rawMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
    ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
    def.clearScanCellRadius = 4;
    def.workableOverrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_telescope_low_kanim")
    };
    def.skyVisibilityInfo = ClusterTelescopeConfig.SKY_VISIBILITY_INFO;
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 1000f;
    storage.showInUI = true;
    go.AddOrGetDef<PoweredController.Def>();
    ManualCodexConversionRegistry.AddConversion((Tag) (string) null, 0.0f, (Tag) "ClusterTelescope", 0.0f, (Tag) DatabankHelper.ID, 3f, (string) STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME, outputCustomFormating: (Func<Tag, float, bool, string>) ((tag, amount, continuous) => string.Format((string) UI.BUILDINGEFFECTS.PER_STARMAP_HEX, (object) GameUtil.GetFormattedInt((float) (int) amount))));
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    ClusterTelescopeConfig.AddVisualizer(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    ClusterTelescopeConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    ClusterTelescopeConfig.AddVisualizer(go);
  }

  private static void AddVisualizer(GameObject prefab)
  {
    SkyVisibilityVisualizer visibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
    visibilityVisualizer.OriginOffset.y = 1;
    visibilityVisualizer.RangeMin = -4;
    visibilityVisualizer.RangeMax = 4;
    visibilityVisualizer.SkipOnModuleInteriors = true;
  }
}
