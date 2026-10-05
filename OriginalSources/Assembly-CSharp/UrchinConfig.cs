// Decompiled with JetBrains decompiler
// Type: UrchinConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UrchinConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Urchin";

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("Urchin", (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.URCHIN.NAME, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.URCHIN.DESC, 100f, true, Assets.GetAnim((HashedString) "urchin_pod_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.4f, 0.4f, true, additionalTags: new List<Tag>()
    {
      GameTags.PedestalDisplayable,
      GameTags.IndustrialProduct
    });
    looseEntity.GetComponent<KCollider2D>().offset = new Vector2(0.0f, 0.05f);
    looseEntity.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
    DecorProvider decorProvider = looseEntity.AddOrGet<DecorProvider>();
    decorProvider.SetValues(TUNING.DECOR.PENALTY.TIER1);
    decorProvider.overrideName = (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.URCHIN.NAME;
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
