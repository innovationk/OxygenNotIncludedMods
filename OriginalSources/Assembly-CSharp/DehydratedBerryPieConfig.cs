// Decompiled with JetBrains decompiler
// Type: DehydratedBerryPieConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DehydratedBerryPieConfig : IEntityConfig, IHasDlcRestrictions
{
  public static Tag ID = new Tag("DehydratedBerryPie");
  public const float MASS = 1f;
  public const string ANIM_FILE = "dehydrated_food_berry_pie_kanim";
  public const string INITIAL_ANIM = "idle";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public GameObject CreatePrefab()
  {
    KAnimFile anim = Assets.GetAnim((HashedString) "dehydrated_food_berry_pie_kanim");
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(DehydratedBerryPieConfig.ID.Name, (string) STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.NAME, (string) STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, element: SimHashes.Polypropylene);
    EntityTemplates.ExtendEntityToDehydratedFoodPackage(looseEntity, TUNING.FOOD.FOOD_TYPES.BERRY_PIE);
    return looseEntity;
  }
}
