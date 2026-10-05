// Decompiled with JetBrains decompiler
// Type: BabyPuftAlphaConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class BabyPuftAlphaConfig : IEntityConfig
{
  public const string ID = "PuftAlphaBaby";

  public GameObject CreatePrefab()
  {
    GameObject puftAlpha = PuftAlphaConfig.CreatePuftAlpha("PuftAlphaBaby", (string) CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.NAME, (string) CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.DESC, "baby_puft_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(puftAlpha, (Tag) "PuftAlpha");
    return puftAlpha;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => BasePuftConfig.OnSpawn(inst);
}
