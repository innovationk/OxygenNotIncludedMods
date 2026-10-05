// Decompiled with JetBrains decompiler
// Type: BabyBeeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyBeeConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "BeeBaby";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject bee = BeeConfig.CreateBee("BeeBaby", (string) CREATURES.SPECIES.BEE.BABY.NAME, (string) CREATURES.SPECIES.BEE.BABY.DESC, "baby_blarva_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(bee, (Tag) "Bee", force_adult_nav_type: true, adult_threshold: 2f);
    bee.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker);
    return bee;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => BaseBeeConfig.SetupLoopingSounds(inst);
}
