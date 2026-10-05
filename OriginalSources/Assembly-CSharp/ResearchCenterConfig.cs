// Decompiled with JetBrains decompiler
// Type: ResearchCenterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class ResearchCenterConfig : IBuildingConfig
{
  public const float BASE_SECONDS_PER_POINT = 45f;
  public const float MASS_PER_POINT = 50f;
  public const float BASE_MASS_PER_SECOND = 1.11111116f;
  public static readonly Tag INPUT_MATERIAL = GameTags.Dirt;
  public const float CAPACITY = 750f;
  public const string ID = "ResearchCenter";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ResearchCenter", 2, 2, "research_center_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RESEARCH);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 1000f;
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = ResearchCenterConfig.INPUT_MATERIAL;
    manualDeliveryKg.refillMass = 150f;
    manualDeliveryKg.capacity = 750f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
    ResearchCenter researchCenter = go.AddOrGet<ResearchCenter>();
    researchCenter.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_research_center_kanim")
    };
    researchCenter.research_point_type_id = "basic";
    researchCenter.inputMaterial = ResearchCenterConfig.INPUT_MATERIAL;
    researchCenter.mass_per_point = 50f;
    ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
    elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
    {
      new ElementConverter.ConsumedElement(ResearchCenterConfig.INPUT_MATERIAL, 1.11111116f)
    };
    elementConverter.showDescriptors = false;
    go.AddOrGetDef<PoweredController.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
