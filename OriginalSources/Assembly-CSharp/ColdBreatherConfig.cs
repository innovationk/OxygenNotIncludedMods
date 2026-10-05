// Decompiled with JetBrains decompiler
// Type: ColdBreatherConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ColdBreatherConfig : IEntityConfig
{
  public const string ID = "ColdBreather";
  public static readonly Tag TAG = TagManager.Create("ColdBreather");
  public const float FERTILIZATION_RATE = 0.006666667f;
  public const SimHashes FERTILIZER = SimHashes.Phosphorite;
  public const float TEMP_DELTA = -5f;
  public const float CONSUMPTION_RATE = 1f;
  public const float RADIATION_STRENGTH = 480f;
  public const string SEED_ID = "ColdBreatherSeed";
  public static readonly Tag SEED_TAG = TagManager.Create("ColdBreatherSeed");

  public GameObject CreatePrefab()
  {
    string name1 = (string) STRINGS.CREATURES.SPECIES.COLDBREATHER.NAME;
    string desc1 = (string) STRINGS.CREATURES.SPECIES.COLDBREATHER.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "coldbreather_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = tieR2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("ColdBreather", name1, desc1, 400f, anim1, "grow_seed", Grid.SceneLayer.BuildingFront, 1, 2, decor, noise);
    placedEntity.AddOrGet<ReceptacleMonitor>();
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<WiltCondition>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<Uprootable>();
    placedEntity.AddOrGet<UprootedMonitor>();
    placedEntity.AddOrGet<DrowningMonitor>();
    EntityTemplates.ExtendPlantToFertilizable(placedEntity, new PlantElementAbsorber.ConsumeInfo[1]
    {
      new PlantElementAbsorber.ConsumeInfo()
      {
        tag = SimHashes.Phosphorite.CreateTag(),
        massConsumptionRate = 0.006666667f
      }
    });
    placedEntity.AddOrGet<TemperatureVulnerable>().Configure(213.15f, 183.15f, 368.15f, 463.15f);
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    ColdBreather coldBreather = placedEntity.AddOrGet<ColdBreather>();
    coldBreather.deltaEmitTemperature = -5f;
    coldBreather.emitOffsetCell = new Vector3(0.0f, 1f);
    coldBreather.consumptionRate = 1f;
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    BuildingTemplates.CreateDefaultStorage(placedEntity).showInUI = false;
    ElementConsumer elementConsumer = placedEntity.AddOrGet<ElementConsumer>();
    elementConsumer.storeOnConsume = true;
    elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
    elementConsumer.capacityKG = 2f;
    elementConsumer.consumptionRate = 0.25f;
    elementConsumer.consumptionRadius = (byte) 1;
    elementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f);
    SimTemperatureTransfer component = placedEntity.GetComponent<SimTemperatureTransfer>();
    component.SurfaceArea = 10f;
    component.Thickness = 1f / 1000f;
    if (DlcManager.FeatureRadiationEnabled())
    {
      RadiationEmitter radiationEmitter = placedEntity.AddComponent<RadiationEmitter>();
      radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
      radiationEmitter.radiusProportionalToRads = false;
      radiationEmitter.emitRadiusX = (short) 6;
      radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
      radiationEmitter.emitRads = 480f;
      radiationEmitter.emissionOffset = new Vector3(0.0f, 0.0f, 0.0f);
    }
    GameObject plant = placedEntity;
    IHasDlcRestrictions dlcRestrictions = this as IHasDlcRestrictions;
    string name2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.NAME;
    string desc2 = (string) STRINGS.CREATURES.SPECIES.SEEDS.COLDBREATHER.DESC;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "seed_coldbreather_kanim");
    List<Tag> additionalTags = new List<Tag>();
    additionalTags.Add(GameTags.CropSeed);
    string domesticateddesc = (string) STRINGS.CREATURES.SPECIES.COLDBREATHER.DOMESTICATEDDESC;
    Tag replantGroundTag = new Tag();
    string domesticatedDescription = domesticateddesc;
    EntityTemplates.CreateAndRegisterPreviewForPlant(EntityTemplates.CreateAndRegisterSeedForPlant(plant, dlcRestrictions, SeedProducer.ProductionType.Hidden, "ColdBreatherSeed", name2, desc2, anim2, additionalTags: additionalTags, replantGroundTag: replantGroundTag, sortOrder: 21, domesticatedDescription: domesticatedDescription, width: 0.3f, height: 0.3f), "ColdBreather_preview", Assets.GetAnim((HashedString) "coldbreather_kanim"), "place", 1, 2);
    SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_grow", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("coldbreather_kanim", "ColdBreather_intake", NOISE_POLLUTION.CREATURES.TIER3);
    placedEntity.AddOrGet<EntityCellVisualizer>();
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSink, new CellOffset());
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
