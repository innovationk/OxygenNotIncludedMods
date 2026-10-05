// Decompiled with JetBrains decompiler
// Type: BabySquidConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabySquidConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SquidBaby";

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject squid = SquidConfig.CreateSquid("SquidBaby", (string) CREATURES.SPECIES.SQUID.BABY.NAME, (string) CREATURES.SPECIES.SQUID.BABY.DESC, "baby_squid_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(squid, (Tag) "Squid");
    return squid;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
