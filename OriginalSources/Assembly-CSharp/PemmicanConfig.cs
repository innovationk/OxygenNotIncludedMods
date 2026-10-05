// Decompiled with JetBrains decompiler
// Type: PemmicanConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PemmicanConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Pemmican";
  public const string ANIM = "pemmican_kanim";
  public static ComplexRecipe recipe;

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("Pemmican", (string) STRINGS.ITEMS.FOOD.PEMMICAN.NAME, (string) STRINGS.ITEMS.FOOD.PEMMICAN.DESC, 1f, false, Assets.GetAnim((HashedString) "pemmican_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true), TUNING.FOOD.FOOD_TYPES.PEMMICAN);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
