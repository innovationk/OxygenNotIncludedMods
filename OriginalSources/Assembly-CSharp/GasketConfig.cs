// Decompiled with JetBrains decompiler
// Type: GasketConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GasketConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "PlasticGasket";
  public static readonly Tag tag = TagManager.Create("PlasticGasket");
  public const float MASS = 1f;
  public const float RECIPE_PRODUCTION_MASS = 50f;

  public string[] GetRequiredDlcIds() => (string[]) null;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("PlasticGasket", (string) ITEMS.INDUSTRIAL_PRODUCTS.PLASTIC_GASKET.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.PLASTIC_GASKET.DESC, 1f, true, Assets.GetAnim((HashedString) "gasket_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, element: SimHashes.Polypropylene, additionalTags: new List<Tag>()
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
