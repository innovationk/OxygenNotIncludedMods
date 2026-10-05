// Decompiled with JetBrains decompiler
// Type: CrabShellConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CrabShellConfig : IEntityConfig
{
  public const string ID = "CrabShell";
  public static readonly Tag TAG = TagManager.Create("CrabShell");
  public const float ADULT_MASS = 60f;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("CrabShell", (string) ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.CRAB_SHELL.DESC, 1f, false, Assets.GetAnim((HashedString) "crabshell_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, additionalTags: new List<Tag>()
    {
      GameTags.Organics,
      GameTags.MoltShell
    });
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddOrGet<SimpleMassStatusItem>();
    looseEntity.AddComponent<EntitySizeVisualizer>().TierSetType = OreSizeVisualizerComponents.TiersSetType.PokeShells;
    EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst1)
  {
    inst1.GetComponent<Compostable>().OnDeserializeCb = (Action<KMonoBehaviour>) (inst2 =>
    {
      if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 36))
        return;
      inst2.GetComponent<PrimaryElement>();
      PrimaryElement component1 = inst2.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        component1.MassPerUnit = 1f;
        component1.Mass = component1.Units * 60f;
      }
      KPrefabID component2 = inst2.GetComponent<KPrefabID>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      component2.RemoveTag(GameTags.IndustrialIngredient);
    });
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
