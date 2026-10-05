// Decompiled with JetBrains decompiler
// Type: NuclearWasteCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class NuclearWasteCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "NuclearWasteComet";
  public static float MASS = 1f;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(NuclearWasteCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.NUCLEAR_WASTE.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    comet.massRange = new Vector2(NuclearWasteCometConfig.MASS, NuclearWasteCometConfig.MASS);
    comet.EXHAUST_ELEMENT = SimHashes.Fallout;
    comet.EXHAUST_RATE = NuclearWasteCometConfig.MASS * 0.2f;
    comet.temperatureRange = new Vector2(473.15f, 573.15f);
    comet.entityDamage = 2;
    comet.totalTileDamage = 0.45f;
    comet.splashRadius = 0;
    comet.impactSound = "Meteor_Nuclear_Impact";
    comet.flyingSoundID = 3;
    comet.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    comet.addTiles = 1;
    comet.diseaseIdx = Db.Get().Diseases.GetIndex((HashedString) Db.Get().Diseases.RadiationPoisoning.Id);
    comet.addDiseaseCount = 1000000;
    comet.affectedByDifficulty = false;
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Corium);
    primaryElement.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "nuclear_metldown_comet_fx_kanim")
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
