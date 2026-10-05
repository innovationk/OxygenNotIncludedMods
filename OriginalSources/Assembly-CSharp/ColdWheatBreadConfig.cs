// Decompiled with JetBrains decompiler
// Type: ColdWheatBreadConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ColdWheatBreadConfig : IEntityConfig
{
  public const string ID = "ColdWheatBread";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ColdWheatBread", (string) STRINGS.ITEMS.FOOD.COLDWHEATBREAD.NAME, (string) STRINGS.ITEMS.FOOD.COLDWHEATBREAD.DESC, 1f, false, Assets.GetAnim((HashedString) "frostbread_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.COLD_WHEAT_BREAD);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
