// Decompiled with JetBrains decompiler
// Type: MusselTongueConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MusselTongueConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "MusselTongue";

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(MusselTongueConfig.ID, (string) STRINGS.ITEMS.FOOD.MUSSELTONGUE.NAME, (string) STRINGS.ITEMS.FOOD.MUSSELTONGUE.DESC, 1f, false, Assets.GetAnim((HashedString) "musseltongue_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.3f, true), TUNING.FOOD.FOOD_TYPES.MUSSELTONGUE);
  }

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
