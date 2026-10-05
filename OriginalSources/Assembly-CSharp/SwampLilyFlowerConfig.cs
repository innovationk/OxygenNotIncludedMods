// Decompiled with JetBrains decompiler
// Type: SwampLilyFlowerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SwampLilyFlowerConfig : IEntityConfig
{
  public static float SEEDS_PER_FRUIT = 1f;
  public static string ID = "SwampLilyFlower";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(SwampLilyFlowerConfig.ID, (string) ITEMS.INGREDIENTS.SWAMPLILYFLOWER.NAME, (string) ITEMS.INGREDIENTS.SWAMPLILYFLOWER.DESC, 1f, false, Assets.GetAnim((HashedString) "swamplilyflower_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialIngredient,
      GameTags.PedestalDisplayable
    });
    EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
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
