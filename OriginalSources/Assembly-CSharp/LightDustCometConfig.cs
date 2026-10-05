// Decompiled with JetBrains decompiler
// Type: LightDustCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class LightDustCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "LightDustComet";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(LightDustCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.LIGHTDUSTCOMET.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    comet.massRange = new Vector2(10f, 14f);
    comet.temperatureRange = new Vector2(223.15f, 253.15f);
    comet.explosionTemperatureRange = comet.temperatureRange;
    comet.explosionOreCount = new Vector2I(1, 2);
    comet.explosionSpeedRange = new Vector2(4f, 7f);
    comet.entityDamage = 0;
    comet.totalTileDamage = 0.0f;
    comet.splashRadius = 0;
    comet.impactSound = "Meteor_dust_light_Impact";
    comet.flyingSoundID = 0;
    comet.explosionEffectHash = SpawnFXHashes.MeteorImpactLightDust;
    comet.EXHAUST_ELEMENT = SimHashes.Void;
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Regolith);
    primaryElement.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_dust_kanim")
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
