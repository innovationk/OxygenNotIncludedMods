// Decompiled with JetBrains decompiler
// Type: BabyCrabConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyCrabConfig : IEntityConfig
{
  public const string ID = "CrabBaby";

  public GameObject CreatePrefab()
  {
    GameObject crab = CrabConfig.CreateCrab("CrabBaby", (string) CREATURES.SPECIES.CRAB.BABY.NAME, (string) CREATURES.SPECIES.CRAB.BABY.DESC, "baby_pincher_kanim", true, new string[2]
    {
      "CrabShell",
      "ShellfishMeat"
    }, new float[2]{ 30f, 1.2f });
    EntityTemplates.ExtendEntityToBeingABaby(crab, (Tag) "Crab", "CrabShell");
    crab.AddOrGetDef<BabyMonitor.Def>().onGrowDropUnits = 30f;
    return crab;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
