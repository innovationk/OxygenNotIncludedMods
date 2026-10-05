// Decompiled with JetBrains decompiler
// Type: OilFloaterHighTempBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class OilFloaterHighTempBabyConfig : IEntityConfig
{
  public const string ID = "OilfloaterHighTempBaby";

  public GameObject CreatePrefab()
  {
    GameObject oilFloater = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTempBaby", (string) CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.NAME, (string) CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.BABY.DESC, "baby_oilfloater_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(oilFloater, (Tag) "OilfloaterHighTemp");
    return oilFloater;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
