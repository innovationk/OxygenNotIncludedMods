// Decompiled with JetBrains decompiler
// Type: SquidMeatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SquidMeatConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SquidMeat";

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("SquidMeat", (string) STRINGS.ITEMS.FOOD.SQUIDMEAT.NAME, (string) STRINGS.ITEMS.FOOD.SQUIDMEAT.DESC, 1f, false, Assets.GetAnim((HashedString) "squid_meat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.8f, true);
    looseEntity.GetComponent<KBoxCollider2D>().offset = new Vector2(0.0f, 0.1f);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.SQUID_MEAT);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
