// Decompiled with JetBrains decompiler
// Type: BaseCometConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class BaseCometConfig
{
  public static GameObject BaseComet(
    string id,
    string name,
    string animName,
    SimHashes primaryElement,
    Vector2 massRange,
    Vector2 temperatureRange,
    string impactSound = "Meteor_Large_Impact",
    int flyingSoundID = 1,
    SimHashes exhaustElement = SimHashes.CarbonDioxide,
    SpawnFXHashes explosionEffect = SpawnFXHashes.None,
    float size = 1f)
  {
    GameObject entity = EntityTemplates.CreateEntity(id, name);
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<LoopingSounds>();
    Comet comet = entity.AddOrGet<Comet>();
    comet.massRange = massRange;
    comet.temperatureRange = temperatureRange;
    comet.explosionTemperatureRange = comet.temperatureRange;
    comet.impactSound = impactSound;
    comet.flyingSoundID = flyingSoundID;
    comet.EXHAUST_ELEMENT = exhaustElement;
    comet.explosionEffectHash = explosionEffect;
    PrimaryElement primaryElement1 = entity.AddOrGet<PrimaryElement>();
    primaryElement1.SetElement(primaryElement);
    primaryElement1.Temperature = (float) (((double) comet.temperatureRange.x + (double) comet.temperatureRange.y) / 2.0);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) animName)
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "fall_loop";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    entity.AddOrGet<KCircleCollider2D>().radius = 0.5f;
    entity.transform.localScale = new Vector3(size, size, 1f);
    entity.AddTag(GameTags.Comet);
    return entity;
  }
}
