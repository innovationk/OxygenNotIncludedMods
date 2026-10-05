// Decompiled with JetBrains decompiler
// Type: SnailIronShellConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SnailIronShellConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SnailIronShell";
  public const float MASS = 10f;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("SnailIronShell", (string) ITEMS.INDUSTRIAL_PRODUCTS.SNAIL_IRON_SHELL.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.SNAIL_IRON_SHELL.DESC, 1f, false, Assets.GetAnim((HashedString) "snail_iron_shell_kanim"), "object", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.4f, true, additionalTags: new List<Tag>()
    {
      GameTags.Organics
    });
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<SimpleMassStatusItem>();
    EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
