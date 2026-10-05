// Decompiled with JetBrains decompiler
// Type: PancakesConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PancakesConfig : IEntityConfig
{
  public const string ID = "Pancakes";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pancakes", (string) STRINGS.ITEMS.FOOD.PANCAKES.NAME, (string) STRINGS.ITEMS.FOOD.PANCAKES.DESC, 1f, false, Assets.GetAnim((HashedString) "stackedpancakes_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true), TUNING.FOOD.FOOD_TYPES.PANCAKES);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
