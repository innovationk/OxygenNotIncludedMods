// Decompiled with JetBrains decompiler
// Type: LightBugPinkBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(4)]
public class LightBugPinkBabyConfig : IEntityConfig
{
  public const string ID = "LightBugPinkBaby";

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugPinkConfig.CreateLightBug("LightBugPinkBaby", (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.NAME, (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.DESC, "baby_lightbug_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(lightBug, (Tag) "LightBugPink");
    return lightBug;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
