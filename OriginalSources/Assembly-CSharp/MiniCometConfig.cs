// Decompiled with JetBrains decompiler
// Type: MiniCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class MiniCometConfig : IEntityConfig
{
  public static readonly string ID = "MiniComet";
  private const SimHashes element = SimHashes.Regolith;
  private const int ADDED_CELLS = 6;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(MiniCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.MINICOMET.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    MiniComet miniComet = entity.AddOrGet<MiniComet>();
    Sim.PhysicsData defaultValues = ElementLoader.FindElementByHash(SimHashes.Regolith).defaultValues;
    miniComet.impactSound = "MeteorDamage_Rock";
    miniComet.flyingSoundID = 2;
    miniComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    entity.AddOrGet<PrimaryElement>();
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_sand_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.AddTag(GameTags.Comet);
    entity.AddTag(GameTags.HideFromSpawnTool);
    entity.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
