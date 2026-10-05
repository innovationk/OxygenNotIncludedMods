// Decompiled with JetBrains decompiler
// Type: BabyDreckoPlasticConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyDreckoPlasticConfig : IEntityConfig
{
  public const string ID = "DreckoPlasticBaby";

  public GameObject CreatePrefab()
  {
    GameObject drecko = DreckoPlasticConfig.CreateDrecko("DreckoPlasticBaby", (string) CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.NAME, (string) CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.BABY.DESC, "baby_drecko_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(drecko, (Tag) "DreckoPlastic");
    return drecko;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
