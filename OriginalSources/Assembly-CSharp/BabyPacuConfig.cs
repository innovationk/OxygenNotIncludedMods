// Decompiled with JetBrains decompiler
// Type: BabyPacuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyPacuConfig : IEntityConfig
{
  public const string ID = "PacuBaby";

  public GameObject CreatePrefab()
  {
    GameObject pacu = PacuConfig.CreatePacu("PacuBaby", (string) CREATURES.SPECIES.PACU.BABY.NAME, (string) CREATURES.SPECIES.PACU.BABY.DESC, "baby_pacu_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(pacu, (Tag) "Pacu");
    return pacu;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
