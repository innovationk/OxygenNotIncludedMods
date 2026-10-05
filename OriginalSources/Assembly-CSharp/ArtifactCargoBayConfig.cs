// Decompiled with JetBrains decompiler
// Type: ArtifactCargoBayConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ArtifactCargoBayConfig : IBuildingConfig
{
  public const string ID = "ArtifactCargoBay";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] hollowTieR1 = TUNING.BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ArtifactCargoBay", 3, 1, "artifact_transport_module_kanim", 1000, 60f, hollowTieR1, refinedMetals, 9999f, BuildLocationRule.Anywhere, none, noise);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.Invincible = true;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    buildingDef.Cancellable = false;
    buildingDef.ShowInBuildMenu = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 1), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, (string) null, TUNING.ROCKETRY.BURDEN.MINOR_PLUS);
    go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>((IEnumerable<Storage.StoredItemModifier>) new Storage.StoredItemModifier[2]
    {
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Preserve
    }));
    Prioritizable.AddRef(go);
    ArtifactModule artifactModule = go.AddOrGet<ArtifactModule>();
    artifactModule.AddDepositTag(GameTags.Artifact);
    artifactModule.occupyingObjectRelativePosition = new Vector3(0.0f, 0.5f, -1f);
    go.AddOrGet<DecorProvider>();
    go.AddOrGet<ItemPedestal>();
    go.AddOrGetDef<ArtifactHarvestModule.Def>();
  }
}
