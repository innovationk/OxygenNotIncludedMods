// Decompiled with JetBrains decompiler
// Type: BabyPacuCleanerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyPacuCleanerConfig : IEntityConfig
{
  public const string ID = "PacuCleanerBaby";

  public GameObject CreatePrefab()
  {
    GameObject pacu = PacuCleanerConfig.CreatePacu("PacuCleanerBaby", (string) CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.NAME, (string) CREATURES.SPECIES.PACU.VARIANT_CLEANER.BABY.DESC, "baby_pacu_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(pacu, (Tag) "PacuCleaner");
    return pacu;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
