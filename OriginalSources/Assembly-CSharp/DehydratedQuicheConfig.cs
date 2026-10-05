// Decompiled with JetBrains decompiler
// Type: DehydratedQuicheConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DehydratedQuicheConfig : IEntityConfig
{
  public static Tag ID = new Tag("DehydratedQuiche");
  public const float MASS = 1f;
  public const string ANIM_FILE = "dehydrated_food_quiche_kanim";
  public const string INITIAL_ANIM = "idle";

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public GameObject CreatePrefab()
  {
    KAnimFile anim = Assets.GetAnim((HashedString) "dehydrated_food_quiche_kanim");
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(DehydratedQuicheConfig.ID.Name, (string) STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.NAME, (string) STRINGS.ITEMS.FOOD.QUICHE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, element: SimHashes.Polypropylene);
    EntityTemplates.ExtendEntityToDehydratedFoodPackage(looseEntity, TUNING.FOOD.FOOD_TYPES.QUICHE);
    return looseEntity;
  }
}
