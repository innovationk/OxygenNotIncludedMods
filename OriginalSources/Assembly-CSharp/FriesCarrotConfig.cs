// Decompiled with JetBrains decompiler
// Type: FriesCarrotConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FriesCarrotConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "FriesCarrot";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriesCarrot", (string) STRINGS.ITEMS.FOOD.FRIESCARROT.NAME, (string) STRINGS.ITEMS.FOOD.FRIESCARROT.DESC, 1f, false, Assets.GetAnim((HashedString) "rootfries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true), TUNING.FOOD.FOOD_TYPES.FRIES_CARROT);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
