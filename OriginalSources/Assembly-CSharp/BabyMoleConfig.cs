// Decompiled with JetBrains decompiler
// Type: BabyMoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyMoleConfig : IEntityConfig
{
  public const string ID = "MoleBaby";

  public GameObject CreatePrefab()
  {
    GameObject mole = MoleConfig.CreateMole("MoleBaby", (string) CREATURES.SPECIES.MOLE.BABY.NAME, (string) CREATURES.SPECIES.MOLE.BABY.DESC, "baby_driller_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(mole, (Tag) "Mole");
    return mole;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => MoleConfig.SetSpawnNavType(inst);
}
