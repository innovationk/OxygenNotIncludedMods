// Decompiled with JetBrains decompiler
// Type: SeaTreeRootConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class SeaTreeRootConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SeaTree";
  public const string SEED_ID = "SeaTreeSeed";
  public const int MAX_BRANCH_COUNT = 8;
  public const float FERTILIZATION_KG_PER_CYCLE = 40f;
  public const float IRRIGATION_KG_PER_CYCLE = 30f;
  public const float FERTILIZATION_RATE = 0.06666667f;
  public const float IRRIGATION_RATE = 0.05f;
  public const float TEMPERATURE_LETHAL_LOW = 248.15f;
  public const float TEMPERATURE_WARNING_LOW = 295.15f;
  public const float TEMPERATURE_WARNING_HIGH = 310.15f;
  public const float TEMPERATURE_LETHAL_HIGH = 398.15f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.SEATREE.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.SEATREE.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "sea_fairy_plant_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("SeaTree", name1, desc1, 2f, anim1, "grow", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, defaultTemperature: 302.65f);
    string str = "SeaTreeOriginal";
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 248.15f, 295.15f, 310.15f, safe_elements: PLANTS.SAFE_ELEMENTS.AllWaters, pressure_sensitive: false, can_drown: false, can_tinker: false, should_grow_old: false, baseTraitId: str, baseTraitName: (string) STRINGS.CREATURES.SPECIES.SEATREE.NAME);
    placedEntity.AddOrGet<PressureVulnerable>().allCellsMustBeSafe = true;
    WiltCondition component1 = placedEntity.GetComponent<WiltCondition>();
    component1.WiltDelay = 0.0f;
    component1.RecoveryDelay = 0.0f;
    KPrefabID component2 = placedEntity.GetComponent<KPrefabID>();
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component2.PrefabID().ToString());
    placedEntity.AddOrGet<Traits>();
    Db.Get().traits.Get(str);
    placedEntity.GetComponent<Modifiers>().initialTraits.Add(str);
    SeaTreeRoot.Def def = placedEntity.AddOrGetDef<SeaTreeRoot.Def>();
    def.BRANCH_PREFAB_NAME = "SeaTreeBranch";
    def.MAX_BRANCH_COUNT = 8;
    placedEntity.AddOrGet<HarvestDesignatable>();
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.DirtyWater.CreateTag(),
        massConsumptionRate = 0.05f
      }
    });
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.ToxicSand.CreateTag(),
        massConsumptionRate = 0.06666667f
      }
    });
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SEATREE.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.SEATREE.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_sea_plant_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.WaterSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.SEATREE.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "SeaTreeSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 12, domesticatedDescription: domesticatedDescription, collisionShape: EntityTemplates.CollisionShape.RECTANGLE, width: 0.8f, height: 0.6f), "SeaTree_preview", Assets.GetAnim((HashedString) "sea_fairy_plant_kanim"), "place", 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
