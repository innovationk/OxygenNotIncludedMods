// Decompiled with JetBrains decompiler
// Type: BabyDreckoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyDreckoConfig : IEntityConfig
{
  public const string ID = "DreckoBaby";

  public GameObject CreatePrefab()
  {
    GameObject drecko = DreckoConfig.CreateDrecko("DreckoBaby", (string) CREATURES.SPECIES.DRECKO.BABY.NAME, (string) CREATURES.SPECIES.DRECKO.BABY.DESC, "baby_drecko_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(drecko, (Tag) "Drecko").AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = (Action<GameObject>) (go =>
    {
      AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
      amountInstance.value = amountInstance.GetMax() * DreckoConfig.SCALE_INITIAL_GROWTH_PCT;
    });
    return drecko;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
