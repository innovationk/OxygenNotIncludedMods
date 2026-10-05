// Decompiled with JetBrains decompiler
// Type: NigiriConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NigiriConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Nigiri";
  public const string ANIM = "nigiri_kanim";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Nigiri", (string) STRINGS.ITEMS.FOOD.NIGIRI.NAME, (string) STRINGS.ITEMS.FOOD.NIGIRI.DESC, 1f, false, Assets.GetAnim((HashedString) "nigiri_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.8f, true), TUNING.FOOD.FOOD_TYPES.NIGIRI);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
