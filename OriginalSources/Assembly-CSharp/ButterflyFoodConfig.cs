// Decompiled with JetBrains decompiler
// Type: ButterflyFoodConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ButterflyFoodConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "ButterflyFood";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("ButterflyFood", (string) STRINGS.ITEMS.FOOD.BUTTERFLYFOOD.NAME, (string) STRINGS.ITEMS.FOOD.BUTTERFLYFOOD.DESC, 1f, false, Assets.GetAnim((HashedString) "fried_mimillet_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.85f, 0.75f, true), TUNING.FOOD.FOOD_TYPES.BUTTERFLYFOOD);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
