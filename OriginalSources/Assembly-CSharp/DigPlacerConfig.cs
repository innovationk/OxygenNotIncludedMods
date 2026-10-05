// Decompiled with JetBrains decompiler
// Type: DigPlacerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class DigPlacerConfig : CommonPlacerConfig, IEntityConfig
{
  public static string ID = "DigPlacer";

  public GameObject CreatePrefab()
  {
    GameObject prefab = this.CreatePrefab(DigPlacerConfig.ID, (string) MISC.PLACERS.DIGPLACER.NAME, Assets.instance.digPlacerAssets.materials[0]);
    Diggable diggable = prefab.AddOrGet<Diggable>();
    diggable.workTime = 5f;
    diggable.synchronizeAnims = false;
    diggable.workAnims = new HashedString[2]
    {
      (HashedString) "place",
      (HashedString) "release"
    };
    diggable.materials = Assets.instance.digPlacerAssets.materials;
    diggable.materialDisplay = prefab.GetComponentInChildren<MeshRenderer>(true);
    prefab.AddOrGet<CancellableDig>();
    return prefab;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }

  [Serializable]
  public class DigPlacerAssets
  {
    public Material[] materials;
  }
}
