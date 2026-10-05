// Decompiled with JetBrains decompiler
// Type: BasicPlantFoodConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BasicPlantFoodConfig : IEntityConfig
{
  public const string ID = "BasicPlantFood";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("BasicPlantFood", (string) STRINGS.ITEMS.FOOD.BASICPLANTFOOD.NAME, (string) STRINGS.ITEMS.FOOD.BASICPLANTFOOD.DESC, 1f, false, Assets.GetAnim((HashedString) "meallicegrain_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.25f, 0.25f, true);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.BASICPLANTFOOD);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
