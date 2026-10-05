// Decompiled with JetBrains decompiler
// Type: VineFruitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class VineFruitConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "VineFruit";
  public const float KCalPerUnit = 325000f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(VineFruitConfig.ID, (string) STRINGS.ITEMS.FOOD.VINEFRUIT.NAME, (string) STRINGS.ITEMS.FOOD.VINEFRUIT.DESC, 1f, false, Assets.GetAnim((HashedString) "ova_melon_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.VINEFRUIT);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
