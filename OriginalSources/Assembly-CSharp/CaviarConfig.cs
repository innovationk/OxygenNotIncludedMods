// Decompiled with JetBrains decompiler
// Type: CaviarConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CaviarConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "Caviar";
  public static readonly Tag TAG = "Caviar".ToTag();

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("Caviar", (string) ITEMS.FOOD.CAVIAR.NAME, (string) ITEMS.FOOD.CAVIAR.DESC, 1f, false, Assets.GetAnim((HashedString) "caviar_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.32f, 0.32f, true, additionalTags: new List<Tag>()
    {
      GameTags.Other,
      GameTags.PedestalDisplayable
    });
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<SimpleMassStatusItem>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
