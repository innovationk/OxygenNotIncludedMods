// Decompiled with JetBrains decompiler
// Type: BabyStaterpillarConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyStaterpillarConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "StaterpillarBaby";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject staterpillar = StaterpillarConfig.CreateStaterpillar("StaterpillarBaby", (string) CREATURES.SPECIES.STATERPILLAR.BABY.NAME, (string) CREATURES.SPECIES.STATERPILLAR.BABY.DESC, "baby_caterpillar_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(staterpillar, (Tag) "Staterpillar");
    return staterpillar;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
