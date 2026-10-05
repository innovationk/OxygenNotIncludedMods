// Decompiled with JetBrains decompiler
// Type: WormSuperFoodConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WormSuperFoodConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "WormSuperFood";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormSuperFood", (string) STRINGS.ITEMS.FOOD.WORMSUPERFOOD.NAME, (string) STRINGS.ITEMS.FOOD.WORMSUPERFOOD.DESC, 1f, false, Assets.GetAnim((HashedString) "wormwood_preserved_berries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.6f, true), TUNING.FOOD.FOOD_TYPES.WORMSUPERFOOD);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
