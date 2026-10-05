// Decompiled with JetBrains decompiler
// Type: SaltySticksFoodConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SaltySticksFoodConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SaltySticksFood";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("SaltySticksFood", (string) STRINGS.ITEMS.FOOD.SALTYSTICKSFOOD.NAME, (string) STRINGS.ITEMS.FOOD.SALTYSTICKSFOOD.DESC, 1f, false, Assets.GetAnim((HashedString) "saltysticksfood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.37f, true);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.SALTYSTICKSFOOD);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;
}
