// Decompiled with JetBrains decompiler
// Type: IceBellyPoopConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IceBellyPoopConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "IceBellyPoop";

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("IceBellyPoop", (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.DESC, 100f, false, Assets.GetAnim((HashedString) "bammoth_poop_kanim"), "idle3", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, true, additionalTags: new List<Tag>()
    {
      GameTags.PedestalDisplayable
    });
    looseEntity.GetComponent<KCollider2D>().offset = new Vector2(0.0f, 0.05f);
    looseEntity.AddTag(GameTags.IndustrialProduct);
    looseEntity.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
    DecorProvider decorProvider = looseEntity.AddOrGet<DecorProvider>();
    decorProvider.SetValues(TUNING.DECOR.PENALTY.TIER3);
    decorProvider.overrideName = (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ICE_BELLY_POOP.NAME;
    looseEntity.AddOrGet<EntitySplitter>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
