// Decompiled with JetBrains decompiler
// Type: SmokedDinosaurMeatConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SmokedDinosaurMeatConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SmokedDinosaurMeat";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SmokedDinosaurMeat", (string) STRINGS.ITEMS.FOOD.SMOKEDDINOSAURMEAT.NAME, (string) STRINGS.ITEMS.FOOD.SMOKEDDINOSAURMEAT.DESC, 1f, false, Assets.GetAnim((HashedString) "dinobrisket_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.SMOKED_DINOSAURMEAT);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
