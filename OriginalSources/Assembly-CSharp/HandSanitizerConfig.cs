// Decompiled with JetBrains decompiler
// Type: HandSanitizerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class HandSanitizerConfig : IBuildingConfig
{
  public const string ID = "HandSanitizer";
  private const float STORAGE_SIZE = 15f;
  private const float MASS_PER_USE = 0.07f;
  private static readonly int DISEASE_REMOVAL_COUNT = WashBasinConfig.DISEASE_REMOVAL_COUNT * 4;
  private const float WORK_TIME = 1.8f;
  private const SimHashes CONSUMED_ELEMENT = SimHashes.BleachStone;

  public override BuildingDef CreateBuildingDef()
  {
    string[] strArray = new string[1]{ "Metal" };
    float[] construction_mass = new float[1]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
    };
    string[] construction_materials = strArray;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("HandSanitizer", 1, 3, "handsanitizer_kanim", 30, 30f, construction_mass, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_out", NOISE_POLLUTION.NOISY.TIER0);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_in", NOISE_POLLUTION.NOISY.TIER0);
    SoundEventVolumeCache.instance.AddVolume("handsanitizer_kanim", "HandSanitizer_tongue_slurp", NOISE_POLLUTION.NOISY.TIER0);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.AdvancedWashStation);
    HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
    handSanitizer.massConsumedPerUse = 0.07f;
    handSanitizer.consumedElement = SimHashes.BleachStone;
    handSanitizer.diseaseRemovalCount = HandSanitizerConfig.DISEASE_REMOVAL_COUNT;
    handSanitizer.hasMeters = false;
    HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
    work.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_handsanitizer_kanim")
    };
    work.workTime = 1.8f;
    work.trackUses = true;
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    go.AddOrGet<DirectionControl>();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = GameTagExtensions.Create(SimHashes.BleachStone);
    manualDeliveryKg.capacity = 15f;
    manualDeliveryKg.refillMass = 3f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    manualDeliveryKg.operationalRequirement = Operational.State.Functional;
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
