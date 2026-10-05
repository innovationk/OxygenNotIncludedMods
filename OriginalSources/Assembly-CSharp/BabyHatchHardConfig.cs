// Decompiled with JetBrains decompiler
// Type: BabyHatchHardConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyHatchHardConfig : IEntityConfig
{
  public const string ID = "HatchHardBaby";

  public GameObject CreatePrefab()
  {
    GameObject hatch = HatchHardConfig.CreateHatch("HatchHardBaby", (string) CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.NAME, (string) CREATURES.SPECIES.HATCH.VARIANT_HARD.BABY.DESC, "baby_hatch_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(hatch, (Tag) "HatchHard");
    return hatch;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
