// Decompiled with JetBrains decompiler
// Type: GeneticAnalysisStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class GeneticAnalysisStationConfig : IBuildingConfig
{
  public const string ID = "GeneticAnalysisStation";

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = MATERIALS.ALL_METALS;
    EffectorValues tieR3 = NOISE_POLLUTION.NOISY.TIER3;
    EffectorValues none = BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR3;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GeneticAnalysisStation", 7, 2, "genetic_analysisstation_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.EnergyConsumptionWhenActive = 480f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.Deprecated = !DlcManager.FeaturePlantMutationsEnabled();
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanIdentifyMutantSeeds.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGetDef<GeneticAnalysisStation.Def>();
    go.AddOrGet<GeneticAnalysisStationWorkable>().finishedSeedDropOffset = new Vector3(-3f, 1.5f, 0.0f);
    Prioritizable.AddRef(go);
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGetDef<PoweredActiveController.Def>();
    Storage storage = go.AddOrGet<Storage>();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    manualDeliveryKg.RequestedItemTag = GameTags.UnidentifiedSeed;
    manualDeliveryKg.refillMass = 1.1f;
    manualDeliveryKg.MinimumMass = 1f;
    manualDeliveryKg.capacity = 5f;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }

  public override void ConfigurePost(BuildingDef def)
  {
    List<Tag> tagList = new List<Tag>();
    foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.CropSeed))
    {
      if ((Object) go.GetComponent<MutantPlant>() != (Object) null)
        tagList.Add(go.PrefabID());
    }
    def.BuildingComplete.GetComponent<Storage>().storageFilters = tagList;
  }
}
