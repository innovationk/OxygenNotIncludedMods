// Decompiled with JetBrains decompiler
// Type: PowerStationToolsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PowerStationToolsConfig : IEntityConfig
{
  public const string ID = "PowerStationTools";
  public static readonly Tag tag = TagManager.Create("PowerStationTools");
  public const float MASS = 5f;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("PowerStationTools", (string) ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim((HashedString) "kit_electrician_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, additionalTags: new List<Tag>()
    {
      GameTags.IndustrialProduct,
      GameTags.MiscPickupable,
      GameTags.PedestalDisplayable
    });
    looseEntity.AddOrGet<EntitySplitter>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    PrimaryElement component = inst.GetComponent<PrimaryElement>();
    if ((double) component.MassPerUnit <= 1.0 || (double) Math.Abs(component.Units - Mathf.Floor(component.Units)) <= (double) Mathf.Epsilon)
      return;
    float num = Mathf.Ceil(component.Mass / component.MassPerUnit) * component.MassPerUnit;
    component.Mass = num;
  }
}
