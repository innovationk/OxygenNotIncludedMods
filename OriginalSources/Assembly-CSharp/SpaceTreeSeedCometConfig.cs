// Decompiled with JetBrains decompiler
// Type: SpaceTreeSeedCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class SpaceTreeSeedCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "SpaceTreeSeedComet";

  public string[] GetRequiredDlcIds() => DlcManager.DLC2;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(SpaceTreeSeedCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.SPACETREESEEDCOMET.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    SpaceTreeSeededComet spaceTreeSeededComet = entity.AddOrGet<SpaceTreeSeededComet>();
    spaceTreeSeededComet.massRange = new Vector2(50f, 100f);
    spaceTreeSeededComet.temperatureRange = new Vector2(253.15f, 263.15f);
    spaceTreeSeededComet.explosionTemperatureRange = spaceTreeSeededComet.temperatureRange;
    spaceTreeSeededComet.impactSound = "Meteor_copper_Impact";
    spaceTreeSeededComet.flyingSoundID = 5;
    spaceTreeSeededComet.EXHAUST_ELEMENT = SimHashes.Void;
    spaceTreeSeededComet.explosionEffectHash = SpawnFXHashes.None;
    spaceTreeSeededComet.entityDamage = 0;
    spaceTreeSeededComet.totalTileDamage = 0.0f;
    spaceTreeSeededComet.splashRadius = 1;
    spaceTreeSeededComet.addTiles = 3;
    spaceTreeSeededComet.addTilesMinHeight = 1;
    spaceTreeSeededComet.addTilesMaxHeight = 2;
    spaceTreeSeededComet.lootOnDestroyedByMissile = new string[1]
    {
      "SpaceTreeSeed"
    };
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Snow);
    primaryElement.Temperature = (float) (((double) spaceTreeSeededComet.temperatureRange.x + (double) spaceTreeSeededComet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_bonbon_snow_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
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
