// Decompiled with JetBrains decompiler
// Type: PickledMealConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PickledMealConfig : IEntityConfig
{
  public const string ID = "PickledMeal";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    GameObject food = EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("PickledMeal", (string) STRINGS.ITEMS.FOOD.PICKLEDMEAL.NAME, (string) STRINGS.ITEMS.FOOD.PICKLEDMEAL.DESC, 1f, false, Assets.GetAnim((HashedString) "pickledmeal_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true), TUNING.FOOD.FOOD_TYPES.PICKLEDMEAL);
    food.GetComponent<KPrefabID>().AddTag(GameTags.Pickled);
    return food;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
