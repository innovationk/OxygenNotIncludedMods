// Decompiled with JetBrains decompiler
// Type: BabyCrabFreshWaterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyCrabFreshWaterConfig : IEntityConfig
{
  public const string ID = "CrabFreshWaterBaby";

  public GameObject CreatePrefab()
  {
    GameObject crabFreshWater = CrabFreshWaterConfig.CreateCrabFreshWater("CrabFreshWaterBaby", (string) CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.NAME, (string) CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.BABY.DESC, "baby_pincher_kanim", true, "ShellfishMeat", 4);
    EntityTemplates.ExtendEntityToBeingABaby(crabFreshWater, (Tag) "CrabFreshWater");
    return crabFreshWater;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
