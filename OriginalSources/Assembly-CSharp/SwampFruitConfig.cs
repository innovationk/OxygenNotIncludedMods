// Decompiled with JetBrains decompiler
// Type: SwampFruitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SwampFruitConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "SwampFruit";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(SwampFruitConfig.ID, (string) STRINGS.ITEMS.FOOD.SWAMPFRUIT.NAME, (string) STRINGS.ITEMS.FOOD.SWAMPFRUIT.DESC, 1f, false, Assets.GetAnim((HashedString) "swampcrop_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, height: 0.72f, isPickupable: true), TUNING.FOOD.FOOD_TYPES.SWAMPFRUIT);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
