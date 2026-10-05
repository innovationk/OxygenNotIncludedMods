// Decompiled with JetBrains decompiler
// Type: IceCavesForagePlantPlantedConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IceCavesForagePlantPlantedConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "IceCavesForagePlantPlanted";

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.CREATURES.SPECIES.ICECAVESFORAGEPLANTPLANTED.NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.ICECAVESFORAGEPLANTPLANTED.DESC;
    EffectorValues tieR1 = TUNING.DECOR.BONUS.TIER1;
    KAnimFile anim = Assets.GetAnim((HashedString) "frozenberries_kanim");
    EffectorValues decor = tieR1;
    List<Tag> tagList = new List<Tag>() { GameTags.Hanging };
    EffectorValues noise = new EffectorValues();
    List<Tag> additionalTags = tagList;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("IceCavesForagePlantPlanted", name, desc, 100f, anim, "idle", Grid.SceneLayer.BuildingBack, 1, 2, decor, noise, additionalTags: additionalTags, defaultTemperature: 253.15f);
    EntityTemplates.MakeHangingOffsets(placedEntity, 1, 2);
    placedEntity.AddOrGet<SimTemperatureTransfer>();
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    placedEntity.AddOrGet<EntombVulnerable>();
    placedEntity.AddOrGet<DrowningMonitor>();
    placedEntity.AddOrGet<Prioritizable>();
    placedEntity.AddOrGet<Uprootable>();
    placedEntity.AddOrGet<UprootedMonitor>().monitorCells = new CellOffset[1]
    {
      new CellOffset(0, 1)
    };
    placedEntity.AddOrGet<Harvestable>();
    placedEntity.AddOrGet<HarvestDesignatable>();
    placedEntity.AddOrGet<SeedProducer>().Configure("IceCavesForagePlant", SeedProducer.ProductionType.DigOnly, 2);
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
