// Decompiled with JetBrains decompiler
// Type: RotPileConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public class RotPileConfig : IEntityConfig
{
  public static string ID = "RotPile";

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(RotPileConfig.ID, (string) STRINGS.ITEMS.FOOD.ROTPILE.NAME, (string) STRINGS.ITEMS.FOOD.ROTPILE.DESC, 1f, false, Assets.GetAnim((HashedString) "rotfood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true);
    KPrefabID component = looseEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Organics);
    component.AddTag(GameTags.Compostable);
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<OccupyArea>();
    looseEntity.AddOrGet<Modifiers>();
    looseEntity.AddOrGet<RotPile>();
    looseEntity.AddComponent<DecorProvider>().SetValues(TUNING.DECOR.PENALTY.TIER2);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<DecorProvider>().overrideName = (string) STRINGS.ITEMS.FOOD.ROTPILE.NAME;
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
