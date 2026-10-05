// Decompiled with JetBrains decompiler
// Type: AsteroidConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AsteroidConfig : IEntityConfig
{
  public const string ID = "Asteroid";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity("Asteroid", "Asteroid");
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<WorldInventory>();
    entity.AddOrGet<WorldContainer>();
    entity.AddOrGet<AsteroidGridEntity>();
    entity.AddOrGet<OrbitalMechanics>();
    entity.AddOrGetDef<GameplaySeasonManager.Def>();
    entity.AddOrGetDef<AlertStateManager.Def>();
    return entity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
