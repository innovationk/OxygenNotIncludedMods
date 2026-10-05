// Decompiled with JetBrains decompiler
// Type: SmokedVegetablesConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SmokedVegetablesConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SmokedVegetables";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SmokedVegetables", (string) STRINGS.ITEMS.FOOD.SMOKEDVEGETABLES.NAME, (string) STRINGS.ITEMS.FOOD.SMOKEDVEGETABLES.DESC, 1f, false, Assets.GetAnim((HashedString) "smokedvegetables_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true), TUNING.FOOD.FOOD_TYPES.SMOKED_VEGETABLES);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
