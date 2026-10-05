// Decompiled with JetBrains decompiler
// Type: RubberGasketConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RubberGasketConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "RubberGasket";
  public static readonly Tag tag = TagManager.Create("RubberGasket");
  public const float MASS = 1f;
  public const float RECIPE_PRODUCTION_MASS = 50f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("RubberGasket", (string) ITEMS.INDUSTRIAL_PRODUCTS.RUBBER_GASKET.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.RUBBER_GASKET.DESC, 1f, true, Assets.GetAnim((HashedString) "rubber_gasket_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, element: SimHashes.Rubber, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialProduct,
      GameTags.MiscPickupable,
      GameTags.PedestalDisplayable,
      GameTags.BuildingGasket
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
