// Decompiled with JetBrains decompiler
// Type: SelfChargingElectrobankConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SelfChargingElectrobankConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "SelfChargingElectrobank";
  public const float MASS = 20f;
  public const float POWER_DURATION = 90000f;
  public const float SELF_CHARGE_WATTAGE = 60f;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1.Append<string>(DlcManager.DLC3);

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("SelfChargingElectrobank", (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.NAME, (string) STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.DESC, 20f, true, Assets.GetAnim((HashedString) "electrobank_large_uranium_kanim"), "idle1", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.8f, true, element: SimHashes.EnrichedUranium, additionalTags: new List<Tag>()
    {
      GameTags.ChargedPortableBattery,
      GameTags.PedestalDisplayable
    });
    RadiationEmitter radiationEmitter = looseEntity.AddOrGet<RadiationEmitter>();
    radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
    radiationEmitter.radiusProportionalToRads = false;
    radiationEmitter.emitRadiusX = (short) 5;
    radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
    radiationEmitter.emitRads = 120f;
    radiationEmitter.emissionOffset = new Vector3(0.0f, 0.0f, 0.0f);
    if (!Assets.IsTagCountable(GameTags.ChargedPortableBattery))
      Assets.AddCountableTag(GameTags.ChargedPortableBattery);
    looseEntity.GetComponent<KCollider2D>();
    looseEntity.AddTag(GameTags.IndustrialProduct);
    SelfChargingElectrobank chargingElectrobank = looseEntity.AddComponent<SelfChargingElectrobank>();
    chargingElectrobank.rechargeable = false;
    chargingElectrobank.keepEmpty = true;
    chargingElectrobank.radioactivityTuning = radiationEmitter.emitRads;
    looseEntity.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
    looseEntity.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.PENALTY.TIER0);
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
