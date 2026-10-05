// Decompiled with JetBrains decompiler
// Type: RawEggConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RawEggConfig : IEntityConfig
{
  public const string ID = "RawEgg";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("RawEgg", (string) STRINGS.ITEMS.FOOD.RAWEGG.NAME, (string) STRINGS.ITEMS.FOOD.RAWEGG.DESC, 1f, false, Assets.GetAnim((HashedString) "rawegg_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true);
    EntityTemplates.ExtendEntityToFood(looseEntity, TUNING.FOOD.FOOD_TYPES.RAWEGG);
    TemperatureCookable temperatureCookable = looseEntity.AddOrGet<TemperatureCookable>();
    temperatureCookable.cookTemperature = 344.15f;
    temperatureCookable.cookedID = "CookedEgg";
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
