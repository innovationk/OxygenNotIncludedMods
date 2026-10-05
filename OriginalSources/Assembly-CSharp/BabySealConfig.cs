// Decompiled with JetBrains decompiler
// Type: BabySealConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabySealConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SealBaby";

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject seal = SealConfig.CreateSeal("SealBaby", (string) CREATURES.SPECIES.SEAL.BABY.NAME, (string) CREATURES.SPECIES.SEAL.BABY.DESC, "baby_seal_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(seal, (Tag) "Seal");
    return seal;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
