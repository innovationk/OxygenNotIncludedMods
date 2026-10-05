// Decompiled with JetBrains decompiler
// Type: BabyMoleDelicacyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyMoleDelicacyConfig : IEntityConfig
{
  public const string ID = "MoleDelicacyBaby";

  public GameObject CreatePrefab()
  {
    GameObject mole = MoleDelicacyConfig.CreateMole("MoleDelicacyBaby", (string) CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.NAME, (string) CREATURES.SPECIES.MOLE.VARIANT_DELICACY.BABY.DESC, "baby_driller_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(mole, (Tag) "MoleDelicacy");
    return mole;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => MoleConfig.SetSpawnNavType(inst);
}
