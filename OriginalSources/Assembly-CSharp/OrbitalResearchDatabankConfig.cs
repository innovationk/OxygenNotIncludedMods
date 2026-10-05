// Decompiled with JetBrains decompiler
// Type: OrbitalResearchDatabankConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OrbitalResearchDatabankConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "OrbitalResearchDatabank";
  public static readonly Tag TAG = TagManager.Create("OrbitalResearchDatabank");
  public const float MASS = 1f;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("OrbitalResearchDatabank", (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.NAME, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ORBITAL_RESEARCH_DATABANK.DESC, 1f, true, Assets.GetAnim((HashedString) "floppy_disc_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, additionalTags: new List<Tag>()
    {
      GameTags.TechComponents,
      GameTags.Experimental,
      GameTags.PedestalDisplayable
    });
    looseEntity.AddOrGet<EntitySplitter>().maxStackSize = (float) TUNING.ROCKETRY.DESTINATION_RESEARCH.BASIC;
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    if (!Game.IsDlcActiveForCurrentSave("DLC2_ID") || SaveLoader.Instance.ClusterLayout == null || !SaveLoader.Instance.ClusterLayout.clusterTags.Contains("CeresCluster"))
      return;
    inst.AddOrGet<KBatchedAnimController>().SwapAnims(new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "floppy_disc_ceres_kanim")
    });
  }
}
