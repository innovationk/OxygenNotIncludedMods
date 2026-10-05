// Decompiled with JetBrains decompiler
// Type: IridiumCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class IridiumCometConfig : IEntityConfig, IHasDlcRestrictions
{
  public static string ID = "IridiumComet";

  public string[] GetRequiredDlcIds() => DlcManager.DLC4;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(IridiumCometConfig.ID, (string) UI.SPACEDESTINATIONS.COMETS.IRIDIUMCOMET.NAME);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    comet.massRange = new Vector2(10f, 100f);
    comet.temperatureRange = new Vector2(473.15f, 548.15f);
    comet.explosionTemperatureRange = comet.temperatureRange;
    comet.explosionOreCount = new Vector2I(2, 4);
    comet.impactSound = "Meteor_copper_Impact";
    comet.flyingSoundID = 1;
    comet.EXHAUST_ELEMENT = SimHashes.CarbonDioxide;
    comet.explosionEffectHash = SpawnFXHashes.MeteorImpactMetal;
    comet.entityDamage = 15;
    comet.totalTileDamage = 0.5f;
    comet.splashRadius = 1;
    PrimaryElement primaryElement = entity.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(SimHashes.Iridium);
    primaryElement.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "meteor_iridium_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
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
