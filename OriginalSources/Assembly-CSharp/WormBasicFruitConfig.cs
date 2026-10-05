// Decompiled with JetBrains decompiler
// Type: WormBasicFruitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WormBasicFruitConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "WormBasicFruit";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("WormBasicFruit", (string) STRINGS.ITEMS.FOOD.WORMBASICFRUIT.NAME, (string) STRINGS.ITEMS.FOOD.WORMBASICFRUIT.DESC, 1f, false, Assets.GetAnim((HashedString) "wormwood_basic_fruit_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.7f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.WORMBASICFRUIT);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
