// Decompiled with JetBrains decompiler
// Type: GardenForagePlantPlantedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GardenForagePlantPlantedConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "GardenForagePlantPlanted";

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.GARDENFORAGEPLANTPLANTED.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.GARDENFORAGEPLANTPLANTED.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim = Assets.GetAnim((HashedString) "fatplant_kanim");
    EffectorValues decor = tieR1;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("GardenForagePlantPlanted", name, desc, 100f, anim, "idle", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise);
    placedEntity.AddOrGet<SimTemperatureTransfer>();
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<DrowningMonitor>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<Uprootable>();
    placedEntity.AddOrGet<UprootedMonitor>();
    placedEntity.AddOrGet<Harvestable>();
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.AddOrGet<SeedProducer>().Configure("GardenForagePlant", SeedProducer.ProductionType.DigOnly);
    placedEntity.AddOrGet<BasicForagePlantPlanted>();
    placedEntity.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
