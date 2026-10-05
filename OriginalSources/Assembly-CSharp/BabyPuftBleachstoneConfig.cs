// Decompiled with JetBrains decompiler
// Type: BabyPuftBleachstoneConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyPuftBleachstoneConfig : IEntityConfig
{
  public const string ID = "PuftBleachstoneBaby";

  public GameObject CreatePrefab()
  {
    GameObject puftBleachstone = PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstoneBaby", (string) CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.NAME, (string) CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.BABY.DESC, "baby_puft_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(puftBleachstone, (Tag) "PuftBleachstone");
    return puftBleachstone;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => BasePuftConfig.OnSpawn(inst);
}
