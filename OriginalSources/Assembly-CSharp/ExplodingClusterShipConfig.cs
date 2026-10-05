// Decompiled with JetBrains decompiler
// Type: ExplodingClusterShipConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ExplodingClusterShipConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "ExplodingClusterShip";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity("ExplodingClusterShip", "ExplodingClusterShip", false);
    ClusterFXEntity clusterFxEntity = entity.AddOrGet<ClusterFXEntity>();
    clusterFxEntity.kAnimName = "rocket_self_destruct_kanim";
    clusterFxEntity.animName = "explode";
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
