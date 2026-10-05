// Decompiled with JetBrains decompiler
// Type: BabyIceBellyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyIceBellyConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "IceBellyBaby";

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject iceBelly = IceBellyConfig.CreateIceBelly("IceBellyBaby", (string) CREATURES.SPECIES.ICEBELLY.BABY.NAME, (string) CREATURES.SPECIES.ICEBELLY.BABY.DESC, "baby_icebelly_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(iceBelly, (Tag) "IceBelly").AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = (Action<GameObject>) (go =>
    {
      AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
      amountInstance.value = amountInstance.GetMax() * IceBellyConfig.SCALE_INITIAL_GROWTH_PCT;
    });
    return iceBelly;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
