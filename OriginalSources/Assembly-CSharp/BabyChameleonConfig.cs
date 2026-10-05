// Decompiled with JetBrains decompiler
// Type: BabyChameleonConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyChameleonConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "ChameleonBaby";

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject chameleon = ChameleonConfig.CreateChameleon("ChameleonBaby", (string) CREATURES.SPECIES.CHAMELEON.BABY.NAME, (string) CREATURES.SPECIES.CHAMELEON.BABY.DESC, "baby_chameleo_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(chameleon, (Tag) "Chameleon");
    return chameleon;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
