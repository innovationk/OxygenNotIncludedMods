// Decompiled with JetBrains decompiler
// Type: SuperWormPlantConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SuperWormPlantConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SuperWormPlant";
  public static readonly EffectorValues SUPER_DECOR = TUNING.DECOR.BONUS.TIER1;
  public const string SUPER_CROP_ID = "WormSuperFruit";
  public const int CROP_YIELD = 8;
  public const float PLANT_FIBER_PRODUCED_PER_CYCLE = 16f;
  private static StandardCropPlant.AnimSet animSet = new StandardCropPlant.AnimSet()
  {
    grow = "super_grow",
    grow_pst = "super_grow_pst",
    idle_full = "super_idle_full",
    wilt_base = "super_wilt",
    harvest = "super_harvest"
  };

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject go = WormPlantConfig.BaseWormPlant("SuperWormPlant", (string) STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.NAME, (string) STRINGS.CREATURES.SPECIES.SUPERWORMPLANT.DESC, "wormwood_kanim", SuperWormPlantConfig.SUPER_DECOR, "WormSuperFruit");
    go.AddOrGet<SeedProducer>().Configure("WormPlantSeed", SeedProducer.ProductionType.Harvest);
    go.AddOrGet<PlantFiberProducer>().amount = 16f;
    return go;
  }

  public void OnPrefabInit(GameObject prefab)
  {
    TransformingPlant transformingPlant = prefab.AddOrGet<TransformingPlant>();
    transformingPlant.SubscribeToTransformEvent(GameHashes.HarvestComplete);
    transformingPlant.transformPlantId = "WormPlant";
    prefab.GetComponent<KAnimControllerBase>().SetSymbolVisiblity((KAnimHashedString) "flower", false);
    prefab.AddOrGet<StandardCropPlant>().anims = SuperWormPlantConfig.animSet;
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
