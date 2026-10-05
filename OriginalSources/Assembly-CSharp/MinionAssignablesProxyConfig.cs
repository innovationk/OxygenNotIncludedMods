// Decompiled with JetBrains decompiler
// Type: MinionAssignablesProxyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MinionAssignablesProxyConfig : IEntityConfig
{
  public static string ID = "MinionAssignablesProxy";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(MinionAssignablesProxyConfig.ID, MinionAssignablesProxyConfig.ID);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<Ownables>();
    entity.AddOrGet<Equipment>();
    entity.AddOrGet<MinionAssignablesProxy>();
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
