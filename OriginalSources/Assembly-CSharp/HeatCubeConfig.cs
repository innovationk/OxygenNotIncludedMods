// Decompiled with JetBrains decompiler
// Type: HeatCubeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class HeatCubeConfig : IEntityConfig
{
  public const string ID = "HeatCube";

  public GameObject CreatePrefab()
  {
    return EntityTemplates.CreateLooseEntity("HeatCube", "Heat Cube", "A cube that holds heat.", 1000f, true, Assets.GetAnim((HashedString) "copper_kanim"), "idle_tallstone", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, isPickupable: true, sortOrder: SORTORDER.BUILDINGELEMENTS, element: SimHashes.Diamond, additionalTags: new List<Tag>()
    {
      GameTags.MiscPickupable,
      GameTags.IndustrialIngredient
    });
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
