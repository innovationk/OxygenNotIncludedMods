// Decompiled with JetBrains decompiler
// Type: BabyStaterpillarGasConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyStaterpillarGasConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "StaterpillarGasBaby";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject staterpillarGas = StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGasBaby", (string) CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.NAME, (string) CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.BABY.DESC, "baby_caterpillar_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(staterpillarGas, (Tag) "StaterpillarGas");
    return staterpillarGas;
  }

  public void OnPrefabInit(GameObject prefab)
  {
    prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "electric_bolt_c_bloom", false);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
