// Decompiled with JetBrains decompiler
// Type: DewDripConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DewDripConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "DewDrip";
  private AttributeModifier decorModifier = new AttributeModifier("Decor", 0.1f, (string) ITEMS.INDUSTRIAL_PRODUCTS.DEWDRIP.NAME, true);

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(DewDripConfig.ID, (string) ITEMS.INDUSTRIAL_PRODUCTS.DEWDRIP.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.DEWDRIP.DESC, 1f, true, Assets.GetAnim((HashedString) "brackorb_kanim"), "object", Grid.SceneLayer.BuildingBack, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.45f, true, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialIngredient
    });
    looseEntity.AddOrGet<EntitySplitter>();
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
