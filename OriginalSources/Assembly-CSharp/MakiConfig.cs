// Decompiled with JetBrains decompiler
// Type: MakiConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MakiConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Maki";
  public const string ANIM = "maki_kanim";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Maki", (string) STRINGS.ITEMS.FOOD.MAKI.NAME, (string) STRINGS.ITEMS.FOOD.MAKI.DESC, 1f, false, Assets.GetAnim((HashedString) "maki_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true), TUNING.FOOD.FOOD_TYPES.MAKI);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
