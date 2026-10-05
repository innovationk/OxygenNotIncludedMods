// Decompiled with JetBrains decompiler
// Type: BabySnailConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabySnailConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SnailBaby";

  public GameObject CreatePrefab()
  {
    GameObject snail = SnailConfig.CreateSnail("SnailBaby", (string) CREATURES.SPECIES.SNAIL.BABY.NAME, (string) CREATURES.SPECIES.SNAIL.BABY.DESC, "baby_snail_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(snail, (Tag) "Snail", force_adult_nav_type: true);
    return snail;
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
