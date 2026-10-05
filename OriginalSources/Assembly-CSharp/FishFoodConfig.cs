// Decompiled with JetBrains decompiler
// Type: FishFoodConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FishFoodConfig : IEntityConfig
{
  public const string ID = "FishFood";
  public const string ANIM = "fishfood_kanim";
  public const string EFFECT_ID = "AteWellPreparedFishFood";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("FishFood", (string) ITEMS.FOOD.FISHFOOD.NAME, (string) ITEMS.FOOD.FISHFOOD.DESC, 1f, true, Assets.GetAnim((HashedString) "fishfood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, additionalTags: new List<Tag>()
    {
      GameTags.Other
    });
    looseEntity.AddOrGet<EntitySplitter>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
