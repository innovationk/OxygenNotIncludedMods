// Decompiled with JetBrains decompiler
// Type: BabyPrehistoricPacuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class BabyPrehistoricPacuConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "PrehistoricPacuBaby";

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject prehistoricPacu = PrehistoricPacuConfig.CreatePrehistoricPacu("PrehistoricPacuBaby", (string) CREATURES.SPECIES.PREHISTORICPACU.BABY.NAME, (string) CREATURES.SPECIES.PREHISTORICPACU.BABY.DESC, "baby_paculacanth_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(prehistoricPacu, (Tag) "PrehistoricPacu");
    return prehistoricPacu;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
