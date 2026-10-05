// Decompiled with JetBrains decompiler
// Type: BabySeaHorseConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabySeaHorseConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SeaHorseBaby";

  public GameObject CreatePrefab()
  {
    GameObject seaHorse = SeaHorseConfig.CreateSeaHorse("SeaHorseBaby", (string) CREATURES.SPECIES.SEAHORSE.BABY.NAME, (string) CREATURES.SPECIES.SEAHORSE.BABY.DESC, "baby_seahorse_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(seaHorse, (Tag) "SeaHorse");
    return seaHorse;
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
