// Decompiled with JetBrains decompiler
// Type: EffectConfigs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EffectConfigs : IMultiEntityConfig
{
  public static string EffectTemplateId = "EffectTemplateFx";
  public static string EffectTemplateOverrideId = "EffectTemplateOverrideFx";
  public static string AttackSplashId = "AttackSplashFx";
  public static string OreAbsorbId = "OreAbsorbFx";
  public static string PlantDeathId = "PlantDeathFx";
  public static string BuildSplashId = "BuildSplashFx";
  public static string DemolishSplashId = "DemolishSplashFx";
  public static string SquidAttackId = "SquidAttackFx";

  public List<GameObject> CreatePrefabs()
  {
    List<GameObject> prefabs = new List<GameObject>();
    List<EffectConfigs.EffectTemplate> effectTemplateList = new List<EffectConfigs.EffectTemplate>()
    {
      new EffectConfigs.EffectTemplate()
      {
        id = EffectConfigs.EffectTemplateId,
        animFiles = new string[0],
        initialAnim = "",
        initialMode = KAnim.PlayMode.Once,
        destroyOnAnimComplete = false
      },
      new EffectConfigs.EffectTemplate()
      {
        id = EffectConfigs.EffectTemplateOverrideId,
        animFiles = new string[0],
        initialAnim = "",
        initialMode = KAnim.PlayMode.Once,
        destroyOnAnimComplete = false
      },
      new EffectConfigs.EffectTemplate()
      {
        id = EffectConfigs.AttackSplashId,
        animFiles = new string[1]
        {
          "attack_beam_contact_fx_kanim"
        },
        initialAnim = "loop",
        initialMode = KAnim.PlayMode.Loop,
        destroyOnAnimComplete = false
      },
      new EffectConfigs.EffectTemplate()
      {
        id = EffectConfigs.OreAbsorbId,
        animFiles = new string[1]{ "ore_collision_kanim" },
        initialAnim = "idle",
        initialMode = KAnim.PlayMode.Once,
        destroyOnAnimComplete = true
      },
      new EffectConfigs.EffectTemplate()
      {
        id = EffectConfigs.PlantDeathId,
        animFiles = new string[1]{ "plant_death_fx_kanim" },
        initialAnim = "plant_death",
        initialMode = KAnim.PlayMode.Once,
        destroyOnAnimComplete = true
      },
      new EffectConfigs.EffectTemplate()
      {
        id = EffectConfigs.BuildSplashId,
        animFiles = new string[1]
        {
          "sparks_radial_build_kanim"
        },
        initialAnim = "loop",
        initialMode = KAnim.PlayMode.Loop,
        destroyOnAnimComplete = false
      },
      new EffectConfigs.EffectTemplate()
      {
        id = EffectConfigs.DemolishSplashId,
        animFiles = new string[1]
        {
          "poi_demolish_impact_kanim"
        },
        initialAnim = "POI_demolish_impact",
        initialMode = KAnim.PlayMode.Loop,
        destroyOnAnimComplete = false
      }
    };
    if (DlcManager.IsContentSubscribed("DLC5_ID"))
      effectTemplateList.Add(new EffectConfigs.EffectTemplate()
      {
        id = EffectConfigs.SquidAttackId,
        animFiles = new string[1]{ "squid_ink_fx_kanim" },
        initialAnim = "loop",
        initialMode = KAnim.PlayMode.Once,
        destroyOnAnimComplete = true
      });
    foreach (EffectConfigs.EffectTemplate effectTemplate in effectTemplateList)
    {
      GameObject entity = EntityTemplates.CreateEntity(effectTemplate.id, effectTemplate.id, false);
      KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
      kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.Simple;
      kbatchedAnimController.initialAnim = effectTemplate.initialAnim;
      kbatchedAnimController.initialMode = effectTemplate.initialMode;
      kbatchedAnimController.isMovable = true;
      kbatchedAnimController.destroyOnAnimComplete = effectTemplate.destroyOnAnimComplete;
      if (effectTemplate.id == EffectConfigs.EffectTemplateOverrideId)
        SymbolOverrideControllerUtil.AddToPrefab(entity);
      if (effectTemplate.animFiles.Length != 0)
      {
        KAnimFile[] kanimFileArray = new KAnimFile[effectTemplate.animFiles.Length];
        for (int index = 0; index < kanimFileArray.Length; ++index)
          kanimFileArray[index] = Assets.GetAnim((HashedString) effectTemplate.animFiles[index]);
        kbatchedAnimController.AnimFiles = kanimFileArray;
      }
      entity.AddOrGet<LoopingSounds>();
      prefabs.Add(entity);
    }
    return prefabs;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }

  public struct EffectTemplate
  {
    public string id;
    public string[] animFiles;
    public string initialAnim;
    public KAnim.PlayMode initialMode;
    public bool destroyOnAnimComplete;
  }
}
