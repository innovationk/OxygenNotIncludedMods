// Decompiled with JetBrains decompiler
// Type: BabyPuftOxyliteConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyPuftOxyliteConfig : IEntityConfig
{
  public const string ID = "PuftOxyliteBaby";

  public GameObject CreatePrefab()
  {
    GameObject puftOxylite = PuftOxyliteConfig.CreatePuftOxylite("PuftOxyliteBaby", (string) CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.NAME, (string) CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.DESC, "baby_puft_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(puftOxylite, (Tag) "PuftOxylite");
    return puftOxylite;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
