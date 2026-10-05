// Decompiled with JetBrains decompiler
// Type: BasicPlantBarConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BasicPlantBarConfig : IEntityConfig
{
  public const string ID = "BasicPlantBar";
  public const string ANIM = "liceloaf_kanim";
  public static ComplexRecipe recipe;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("BasicPlantBar", (string) STRINGS.ITEMS.FOOD.BASICPLANTBAR.NAME, (string) STRINGS.ITEMS.FOOD.BASICPLANTBAR.DESC, 1f, false, Assets.GetAnim((HashedString) "liceloaf_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.BASICPLANTBAR);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
