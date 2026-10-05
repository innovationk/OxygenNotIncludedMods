// Decompiled with JetBrains decompiler
// Type: LettuceConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LettuceConfig : IEntityConfig
{
  public const string ID = "Lettuce";
  public const float MASS_PER_UNIT = 1f;
  public const float KCAL_PER_UNIT = 400000f;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Lettuce", (string) STRINGS.ITEMS.FOOD.LETTUCE.NAME, (string) STRINGS.ITEMS.FOOD.LETTUCE.DESC, 1f, false, Assets.GetAnim((HashedString) "sea_lettuce_leaves_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.LETTUCE);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
