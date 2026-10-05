// Decompiled with JetBrains decompiler
// Type: GingerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GingerConfig : IEntityConfig
{
  public static string ID = nameof (GingerConfig);
  public static int SORTORDER = 1;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(GingerConfig.ID, (string) STRINGS.ITEMS.INGREDIENTS.GINGER.NAME, (string) STRINGS.ITEMS.INGREDIENTS.GINGER.DESC, 1f, true, Assets.GetAnim((HashedString) "ginger_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.45f, 0.4f, true, TUNING.SORTORDER.BUILDINGELEMENTS + GingerConfig.SORTORDER, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialIngredient
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
