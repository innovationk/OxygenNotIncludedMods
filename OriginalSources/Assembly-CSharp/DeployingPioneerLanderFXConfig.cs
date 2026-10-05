// Decompiled with JetBrains decompiler
// Type: DeployingPioneerLanderFXConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DeployingPioneerLanderFXConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "DeployingPioneerLanderFX";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity("DeployingPioneerLanderFX", "DeployingPioneerLanderFX", false);
    ClusterFXEntity clusterFxEntity = entity.AddOrGet<ClusterFXEntity>();
    clusterFxEntity.kAnimName = "pioneer01_kanim";
    clusterFxEntity.animName = "landing";
    clusterFxEntity.animPlayMode = KAnim.PlayMode.Loop;
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
