// Decompiled with JetBrains decompiler
// Type: DeepFriedMeatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DeepFriedMeatConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "DeepFriedMeat";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedMeat", (string) STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.NAME, (string) STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.DESC, 1f, false, Assets.GetAnim((HashedString) "deepfried_meat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.DEEP_FRIED_MEAT);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
