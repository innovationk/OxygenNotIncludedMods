// Decompiled with JetBrains decompiler
// Type: LightBugBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
[EntityConfigOrder(3)]
public class LightBugBabyConfig : IEntityConfig
{
  public const string ID = "LightBugBaby";

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugConfig.CreateLightBug("LightBugBaby", (string) CREATURES.SPECIES.LIGHTBUG.BABY.NAME, (string) CREATURES.SPECIES.LIGHTBUG.BABY.DESC, "baby_lightbug_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(lightBug, (Tag) "LightBug");
    return lightBug;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
