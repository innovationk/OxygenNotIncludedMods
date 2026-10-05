// Decompiled with JetBrains decompiler
// Type: EmptyElectrobankConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EmptyElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "EmptyElectrobank";
  public const float MASS = 20f;

  public string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("EmptyElectrobank", (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.NAME, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_EMPTY.DESC, 20f, true, Assets.GetAnim((HashedString) "electrobank_large_depleted_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, element: SimHashes.Katairite, additionalTags: new List<Tag>()
    {
      GameTags.EmptyPortableBattery,
      GameTags.PedestalDisplayable
    });
    if (!Assets.IsTagCountable(GameTags.EmptyPortableBattery))
      Assets.AddCountableTag(GameTags.EmptyPortableBattery);
    looseEntity.GetComponent<KCollider2D>();
    looseEntity.AddTag(GameTags.IndustrialProduct);
    looseEntity.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
    looseEntity.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.PENALTY.TIER0);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
