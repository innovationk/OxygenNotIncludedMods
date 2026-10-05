// Decompiled with JetBrains decompiler
// Type: HighEnergyParticleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class HighEnergyParticleConfig : IEntityConfig, IHasDlcRestrictions
{
  public const int PARTICLE_SPEED = 8;
  public const float PARTICLE_COLLISION_SIZE = 0.2f;
  public const float PER_CELL_FALLOFF = 0.1f;
  public const float FALLOUT_RATIO = 0.5f;
  public const int MAX_PAYLOAD = 500;
  public const int EXPLOSION_FALLOUT_TEMPERATURE = 5000;
  public const float EXPLOSION_FALLOUT_MASS_PER_PARTICLE = 0.001f;
  public const float EXPLOSION_EMIT_DURRATION = 1f;
  public const short EXPLOSION_EMIT_RADIUS = 6;
  public const string ID = "HighEnergyParticle";

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject basicEntity = EntityTemplates.CreateBasicEntity("HighEnergyParticle", (string) ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, (string) ITEMS.RADIATION.HIGHENERGYPARITCLE.DESC, 1f, false, Assets.GetAnim((HashedString) "spark_radial_high_energy_particles_kanim"), "travel_pre", Grid.SceneLayer.FXFront2);
    EntityTemplates.AddCollision(basicEntity, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f);
    basicEntity.AddOrGet<LoopingSounds>();
    RadiationEmitter radiationEmitter = basicEntity.AddOrGet<RadiationEmitter>();
    radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
    radiationEmitter.radiusProportionalToRads = false;
    radiationEmitter.emitRadiusX = (short) 3;
    radiationEmitter.emitRadiusY = (short) 3;
    radiationEmitter.emitRads = (float) (0.40000000596046448 * ((double) radiationEmitter.emitRadiusX / 6.0));
    basicEntity.AddComponent<HighEnergyParticle>().speed = 8f;
    return basicEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
