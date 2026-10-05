// Decompiled with JetBrains decompiler
// Type: DevLifeSupportConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class DevLifeSupportConfig : IBuildingConfig
{
  public const string ID = "DevLifeSupport";
  private const float OXYGEN_GENERATION_RATE = 50.0000038f;
  private const float OXYGEN_TEMPERATURE = 303.15f;
  private const float OXYGEN_MAX_PRESSURE = 1.5f;
  private const float CO2_CONSUMPTION_RATE = 50.0000038f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR3 = BUILDINGS.DECOR.PENALTY.TIER3;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("DevLifeSupport", 1, 1, "dev_life_support_kanim", 30, 30f, tieR5, rawMinerals, 800f, BuildLocationRule.Anywhere, tieR3, noise);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "HollowMetal";
    buildingDef.AudioSize = "large";
    buildingDef.DebugOnly = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddTag(GameTags.DevBuilding);
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
    defaultStorage.showInUI = true;
    defaultStorage.capacityKg = 200f;
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    CellOffset cellOffset = new CellOffset(0, 1);
    ElementEmitter elementEmitter = go.AddOrGet<ElementEmitter>();
    elementEmitter.outputElement = new ElementConverter.OutputElement(50.0000038f, SimHashes.Oxygen, 303.15f, outputElementOffsetx: (float) cellOffset.x, outputElementOffsety: (float) cellOffset.y);
    elementEmitter.emissionFrequency = 1f;
    elementEmitter.maxPressure = 1.5f;
    PassiveElementConsumer passiveElementConsumer = go.AddOrGet<PassiveElementConsumer>();
    passiveElementConsumer.elementToConsume = SimHashes.CarbonDioxide;
    passiveElementConsumer.consumptionRate = 50.0000038f;
    passiveElementConsumer.capacityKG = 50.0000038f;
    passiveElementConsumer.consumptionRadius = (byte) 10;
    passiveElementConsumer.showInStatusPanel = true;
    passiveElementConsumer.sampleCellOffset = new Vector3(0.0f, 0.0f, 0.0f);
    passiveElementConsumer.isRequired = false;
    passiveElementConsumer.storeOnConsume = false;
    passiveElementConsumer.showDescriptor = false;
    passiveElementConsumer.ignoreActiveChanged = true;
    go.AddOrGet<DevLifeSupport>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
