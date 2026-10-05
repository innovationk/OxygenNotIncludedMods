// Decompiled with JetBrains decompiler
// Type: BabyCrabWoodConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyCrabWoodConfig : IEntityConfig
{
  public const string ID = "CrabWoodBaby";

  public GameObject CreatePrefab()
  {
    GameObject crabWood = CrabWoodConfig.CreateCrabWood("CrabWoodBaby", (string) CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.NAME, (string) CREATURES.SPECIES.CRAB.VARIANT_WOOD.BABY.DESC, "baby_pincher_kanim", true, new string[2]
    {
      "CrabWoodShell",
      "ShellfishMeat"
    }, new float[2]{ 100f, 1.2f });
    EntityTemplates.ExtendEntityToBeingABaby(crabWood, (Tag) "CrabWood", "CrabWoodShell");
    crabWood.AddOrGetDef<BabyMonitor.Def>().onGrowDropUnits = 50f;
    return crabWood;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
