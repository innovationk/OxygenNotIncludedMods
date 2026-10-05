// Decompiled with JetBrains decompiler
// Type: OuthouseConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class OuthouseConfig : IBuildingConfig
{
  public const string ID = "Outhouse";
  private const int USES_PER_REFILL = 15;
  private const float DIRT_PER_REFILL = 200f;
  private const float DIRT_PER_USE = 13f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMineralsOrWood = TUNING.MATERIALS.RAW_MINERALS_OR_WOOD;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR4 = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("Outhouse", 2, 3, "outhouse_kanim", 30, 30f, tieR3, rawMineralsOrWood, 800f, BuildLocationRule.OnFloor, tieR4, noise);
    buildingDef.Overheatable = false;
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TOILET);
    SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_open", NOISE_POLLUTION.NOISY.TIER1);
    SoundEventVolumeCache.instance.AddVolume("outhouse_kanim", "Latrine_door_close", NOISE_POLLUTION.NOISY.TIER1);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    KPrefabID component = go.GetComponent<KPrefabID>();
    go.AddOrGet<LoopingSounds>();
    component.AddTag(RoomConstraints.ConstraintTags.ToiletType);
    Toilet toilet = go.AddOrGet<Toilet>();
    toilet.maxFlushes = 15;
    toilet.dirtUsedPerFlush = 13f;
    toilet.solidWastePerUse = new Toilet.SpawnInfo(SimHashes.ToxicSand, DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE, 0.0f);
    toilet.solidWasteTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
    toilet.diseaseId = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;
    toilet.diseasePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE;
    toilet.diseaseOnDupePerFlush = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE;
    go.AddOrGet<ToiletWorkableUse>().workLayer = Grid.SceneLayer.BuildingFront;
    ToiletWorkableClean toiletWorkableClean = go.AddOrGet<ToiletWorkableClean>();
    toiletWorkableClean.workTime = 90f;
    toiletWorkableClean.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_outhouse_kanim")
    };
    toiletWorkableClean.workLayer = Grid.SceneLayer.BuildingFront;
    Prioritizable.AddRef(go);
    toiletWorkableClean.SetIsCloggedByGunk(false);
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage.showInUI = true;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = new Tag("Dirt");
    manualDeliveryKg.capacity = 200f;
    manualDeliveryKg.refillMass = 0.01f;
    manualDeliveryKg.MinimumMass = 200f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    manualDeliveryKg.operationalRequirement = Operational.State.Functional;
    manualDeliveryKg.FillToCapacity = true;
    Ownable ownable = go.AddOrGet<Ownable>();
    ownable.slotID = Db.Get().AssignableSlots.Toilet.Id;
    ownable.canBePublic = true;
    go.AddOrGetDef<RocketUsageRestriction.Def>();
    component.prefabInitFn += new KPrefabID.PrefabFn(this.OnInit);
  }

  private void OnInit(GameObject go)
  {
    ToiletWorkableUse component = go.GetComponent<ToiletWorkableUse>();
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_outhouse_kanim")
    };
    component.workerTypeOverrideAnims.Add((Tag) MinionConfig.ID, kanimFileArray);
    component.workerTypeOverrideAnims.Add((Tag) BionicMinionConfig.ID, new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_bionic_interacts_outhouse_kanim")
    });
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
