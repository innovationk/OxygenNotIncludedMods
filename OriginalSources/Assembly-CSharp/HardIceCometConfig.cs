// Decompiled with JetBrains decompiler
// Type: HardIceCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class HardIceCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static readonly string ID = "HardIceComet";
  private const SimHashes element = SimHashes.CrushedIce;
  private const int ADDED_CELLS = 6;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(HardIceCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.HARDICECOMET.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    float mass = ElementLoader.FindElementByHash(SimHashes.CrushedIce).defaultValues.mass;
    comet.massRange = new Vector2((float) ((double) mass * 0.800000011920929 * 6.0), (float) ((double) mass * 1.2000000476837158 * 6.0));
    comet.temperatureRange = new Vector2(173.15f, 248.15f);
    comet.explosionTemperatureRange = comet.temperatureRange;
    comet.addTiles = 6;
    comet.addTilesMinHeight = 2;
    comet.addTilesMaxHeight = 8;
    comet.entityDamage = 0;
    comet.totalTileDamage = 0.0f;
    comet.splashRadius = 1;
    comet.impactSound = "Meteor_ice_Impact";
    comet.flyingSoundID = 6;
    comet.explosionEffectHash = SpawnFXHashes.MeteorImpactIce;
    comet.EXHAUST_ELEMENT = SimHashes.Oxygen;
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.CrushedIce);
    primaryElement.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_ice_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.AddTag(GameTags.Comet);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
