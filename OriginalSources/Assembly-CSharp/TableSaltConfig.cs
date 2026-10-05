// Decompiled with JetBrains decompiler
// Type: TableSaltConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class TableSaltConfig : IEntityConfig
{
  public static string ID = "TableSalt";
  public static readonly Tag TAG = TableSaltConfig.ID.ToTag();

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(TableSaltConfig.ID, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC, 1f, false, Assets.GetAnim((HashedString) "seed_saltPlant_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + TableSaltTuning.SORTORDER, SimHashes.Salt, new List<Tag>()
    {
      GameTags.Other
    });
    looseEntity.AddOrGet<EntitySplitter>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
