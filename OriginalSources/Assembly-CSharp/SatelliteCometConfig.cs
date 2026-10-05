// Decompiled with JetBrains decompiler
// Type: SatelliteCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class SatelliteCometConfig : IEntityConfig
{
  public static string ID = "SatelliteCometComet";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(SatelliteCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.SATELLITE.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    comet.massRange = new Vector2(100f, 200f);
    comet.EXHAUST_ELEMENT = SimHashes.AluminumGas;
    comet.temperatureRange = new Vector2(473.15f, 573.15f);
    comet.entityDamage = 2;
    comet.explosionOreCount = new Vector2I(8, 8);
    comet.totalTileDamage = 2f;
    comet.splashRadius = 1;
    comet.impactSound = "Meteor_Large_Impact";
    comet.flyingSoundID = 1;
    comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    comet.addTiles = 0;
    comet.craterPrefabs = new string[3]
    {
      "PropSurfaceSatellite1",
      PropSurfaceSatellite2Config.ID,
      PropSurfaceSatellite3Config.ID
    };
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Aluminum);
    primaryElement.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_rock_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
    entity.AddTag(GameTags.Comet);
    entity.AddTag(GameTags.DeprecatedContent);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
