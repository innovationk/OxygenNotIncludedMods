// Decompiled with JetBrains decompiler
// Type: BabySeaTurtleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabySeaTurtleConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SeaTurtleBaby";

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject seaTurtle = SeaTurtleConfig.CreateSeaTurtle("SeaTurtleBaby", (string) CREATURES.SPECIES.SEATURTLE.BABY.NAME, (string) CREATURES.SPECIES.SEATURTLE.BABY.DESC, "baby_turtle_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(seaTurtle, (Tag) "SeaTurtle").AddOrGetDef<BabyMonitor.Def>().configureAdultOnMaturation = (Action<GameObject>) (go =>
    {
      AmountInstance amountInstance = Db.Get().Amounts.ScaleGrowth.Lookup(go);
      amountInstance.value = amountInstance.GetMax() * SeaTurtleTuning.SCALE_INITIAL_GROWTH_PCT;
    });
    return seaTurtle;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
