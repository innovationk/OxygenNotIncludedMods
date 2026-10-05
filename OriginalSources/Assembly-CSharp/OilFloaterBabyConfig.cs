// Decompiled with JetBrains decompiler
// Type: OilFloaterBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class OilFloaterBabyConfig : IEntityConfig
{
  public const string ID = "OilfloaterBaby";

  public GameObject CreatePrefab()
  {
    GameObject oilFloater = OilFloaterConfig.CreateOilFloater("OilfloaterBaby", (string) CREATURES.SPECIES.OILFLOATER.BABY.NAME, (string) CREATURES.SPECIES.OILFLOATER.BABY.DESC, "baby_oilfloater_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(oilFloater, (Tag) "Oilfloater");
    return oilFloater;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
