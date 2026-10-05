// Decompiled with JetBrains decompiler
// Type: MosquitoLarvaConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class MosquitoLarvaConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "MosquitoBaby";

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject mosquito = MosquitoConfig.CreateMosquito("MosquitoBaby", (string) CREATURES.SPECIES.MOSQUITO.BABY.NAME, (string) CREATURES.SPECIES.MOSQUITO.BABY.DESC, "baby_mosquito_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(mosquito, (Tag) "Mosquito");
    return mosquito;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
