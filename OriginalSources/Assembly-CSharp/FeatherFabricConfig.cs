// Decompiled with JetBrains decompiler
// Type: FeatherFabricConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class FeatherFabricConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "FeatherFabric";
  private AttributeModifier decorModifier = new AttributeModifier("Decor", 0.1f, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.BASIC_FABRIC.NAME, true);

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(FeatherFabricConfig.ID, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.FEATHER_FABRIC.NAME, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.FEATHER_FABRIC.DESC, 1f, true, Assets.GetAnim((HashedString) "feather_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, SORTORDER.BUILDINGELEMENTS + BasicFabricTuning.SORTORDER, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialIngredient,
      GameTags.BuildingFiber,
      GameTags.PedestalDisplayable
    });
    looseEntity.AddOrGet<EntitySplitter>();
    KBoxCollider2D kboxCollider2D = looseEntity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.offset = (Vector2) new Vector2f(0.0f, 0.3f);
    kboxCollider2D.size = (Vector2) new Vector2f(0.8f, 0.8f);
    looseEntity.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(this.decorModifier);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
