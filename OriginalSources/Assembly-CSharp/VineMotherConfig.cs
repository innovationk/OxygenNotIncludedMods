// Decompiled with JetBrains decompiler
// Type: VineMotherConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class VineMotherConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "VineMother";
  public const string SEED_ID = "VineMotherSeed";
  public const int MAX_BRANCH_NETWORK_COUNT = 12;
  public static SimHashes[] ALLOWED_ELEMENTS = new SimHashes[3]
  {
    SimHashes.Oxygen,
    SimHashes.CarbonDioxide,
    SimHashes.ContaminatedOxygen
  };
  public const float IRRIGATION_RATE = 0.15f;
  public const float TEMPERATURE_LETHAL_LOW = 273.15f;
  public const float TEMPERATURE_WARNING_LOW = 298.15f;
  public const float TEMPERATURE_WARNING_HIGH = 318.15f;
  public const float TEMPERATURE_LETHAL_HIGH = 378.15f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.VINEMOTHER.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.VINEMOTHER.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "vine_mother_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("VineMother", name1, desc1, 2f, anim1, "object", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise, defaultTemperature: 308.15f);
    string str = "VineMotherOriginal";
    EntityTemplates.ExtendEntityToBasicPlant(placedEntity, 273.15f, 298.15f, 318.15f, 378.15f, VineMotherConfig.ALLOWED_ELEMENTS, false, can_tinker: false, should_grow_old: false, baseTraitId: str, baseTraitName: (string) STRINGS.CREATURES.SPECIES.VINEMOTHER.NAME);
    WiltCondition component1 = placedEntity.GetComponent<WiltCondition>();
    component1.WiltDelay = 0.0f;
    component1.RecoveryDelay = 0.0f;
    KPrefabID component2 = placedEntity.GetComponent<KPrefabID>();
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component2.PrefabID().ToString());
    placedEntity.AddOrGet<Traits>();
    Db.Get().traits.Get(str);
    placedEntity.GetComponent<Modifiers>().initialTraits.Add(str);
    VineMother.Def def = placedEntity.AddOrGetDef<VineMother.Def>();
    def.BRANCH_PREFAB_NAME = "VineBranch";
    def.MAX_BRANCH_COUNT = 24;
    placedEntity.AddOrGet<HarvestDesignatable>();
    EntityTemplates.ExtendPlantToIrrigated(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = GameTags.Water,
        massConsumptionRate = 0.15f
      }
    });
    GameObject plant = placedEntity;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.VINEMOTHER.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.VINEMOTHER.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_vine_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.VINEMOTHER.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) this, SeedProducer.ProductionType.Hidden, "VineMotherSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 12, domesticatedDescription: domesticatedDescription, collisionShape: EntityTemplates.CollisionShape.RECTANGLE, width: 0.8f, height: 0.6f), "VineMother_preview", Assets.GetAnim((HashedString) "vine_mother_kanim"), "place", 1, 2);
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
