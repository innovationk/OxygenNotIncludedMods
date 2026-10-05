// Decompiled with JetBrains decompiler
// Type: BabyWormConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyWormConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "DivergentWormBaby";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject worm = DivergentWormConfig.CreateWorm("DivergentWormBaby", (string) CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.NAME, (string) CREATURES.SPECIES.DIVERGENT.VARIANT_WORM.BABY.DESC, "baby_worm_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(worm, (Tag) "DivergentWorm");
    return worm;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
