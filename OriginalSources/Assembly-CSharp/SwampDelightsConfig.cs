// Decompiled with JetBrains decompiler
// Type: SwampDelightsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SwampDelightsConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SwampDelights";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampDelights", (string) STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.NAME, (string) STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.DESC, 1f, false, Assets.GetAnim((HashedString) "swamp_delights_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true), TUNING.FOOD.FOOD_TYPES.SWAMP_DELIGHTS);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
