// Decompiled with JetBrains decompiler
// Type: MeterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MeterConfig : IEntityConfig
{
  public static readonly string ID = "Meter";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(MeterConfig.ID, MeterConfig.ID, false);
    entity.AddOrGet<KBatchedAnimController>();
    entity.AddOrGet<KBatchedAnimTracker>();
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
