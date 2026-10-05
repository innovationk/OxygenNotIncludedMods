// Decompiled with JetBrains decompiler
// Type: PlantFiberConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlantFiberConfig : IEntityConfig
{
  public const string ID = "PlantFiber";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("PlantFiber", (string) ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.DESC, 1f, false, Assets.GetAnim((HashedString) "plant_matter_kanim"), "idle1", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialProduct,
      GameTags.PedestalDisplayable
    });
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddComponent<EntitySizeVisualizer>().TierSetType = OreSizeVisualizerComponents.TiersSetType.PlantFiber;
    EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
