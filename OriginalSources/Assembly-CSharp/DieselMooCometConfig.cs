// Decompiled with JetBrains decompiler
// Type: DieselMooCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class DieselMooCometConfig : IEntityConfig
{
  public static string ID = "DieselMooComet";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(DieselMooCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.DIESELMOOCOMET.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    GassyMooComet gassyMooComet = entity.AddOrGet<GassyMooComet>();
    gassyMooComet.massRange = new Vector2(100f, 200f);
    gassyMooComet.EXHAUST_ELEMENT = SimHashes.CarbonDioxide;
    gassyMooComet.temperatureRange = new Vector2(296.15f, 318.15f);
    gassyMooComet.entityDamage = 0;
    gassyMooComet.explosionOreCount = new Vector2I(0, 0);
    gassyMooComet.totalTileDamage = 0.0f;
    gassyMooComet.splashRadius = 1;
    gassyMooComet.impactSound = "Meteor_GassyMoo_Impact";
    gassyMooComet.flyingSoundID = 4;
    gassyMooComet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    gassyMooComet.addTiles = 0;
    gassyMooComet.affectedByDifficulty = false;
    gassyMooComet.lootOnDestroyedByMissile = new string[3]
    {
      "Meat",
      "Meat",
      "Meat"
    };
    gassyMooComet.destroyOnExplode = false;
    gassyMooComet.craterPrefabs = new string[1]
    {
      "DieselMoo"
    };
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Creature);
    primaryElement.Temperature = (float) (((double) gassyMooComet.temperatureRange.x + (double) gassyMooComet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_huskymoo_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
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
