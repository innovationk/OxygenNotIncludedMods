// Decompiled with JetBrains decompiler
// Type: MusselSproutConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MusselSproutConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "MusselSprout";
  public static string BASE_TRAIT_ID = MusselSproutConfig.ID + "_BaseTrait";
  private const string HARVEST_ANIM = "harvest";

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(MusselSproutConfig.ID, (string) STRINGS.CREATURES.SPECIES.MUSSELSPROUT.NAME, (string) STRINGS.CREATURES.SPECIES.MUSSELSPROUT.DESC, 50f, Assets.GetAnim((HashedString) "mussel_sprout_kanim"), "idle", Grid.SceneLayer.BuildingBack, 1, 1, TUNING.DECOR.PENALTY.TIER0);
    placedEntity.AddOrGet<SimTemperatureTransfer>();
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<DelayedUprootable>().deathAnimation = (HashedString) "harvest";
    placedEntity.AddOrGet<UprootedMonitor>();
    placedEntity.AddOrGet<Harvestable>();
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.AddOrGet<SeedProducer>().Configure(MusselTongueConfig.ID, SeedProducer.ProductionType.DigOnly);
    placedEntity.AddOrGet<BasicForagePlantPlanted>().Pre_Death_Anim = "harvest";
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    return placedEntity;
  }

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    inst.GetComponent<KBatchedAnimController>().animScale *= 0.75f;
  }
}
