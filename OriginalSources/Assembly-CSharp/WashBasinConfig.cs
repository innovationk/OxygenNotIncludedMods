// Decompiled with JetBrains decompiler
// Type: WashBasinConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class WashBasinConfig : IBuildingConfig
{
  public const string ID = "WashBasin";
  public static readonly int DISEASE_REMOVAL_COUNT = DUPLICANTSTATS.STANDARD.Secretions.DISEASE_PER_PEE + 20000;
  public const float WATER_PER_USE = 5f;
  public const int USES_PER_FLUSH = 40;
  public const float WORK_TIME = 5f;

  public override BuildingDef CreateBuildingDef()
  {
    string[] mineralsOrMetals = TUNING.MATERIALS.RAW_MINERALS_OR_METALS;
    float[] tieR1_1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] construction_materials = mineralsOrMetals;
    EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
    EffectorValues tieR1_2 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = tieR0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WashBasin", 2, 3, "wash_basin_kanim", 30, 30f, tieR1_1, construction_materials, 1600f, BuildLocationRule.OnFloor, tieR1_2, noise);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.SINK);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation);
    HandSanitizer handSanitizer = go.AddOrGet<HandSanitizer>();
    handSanitizer.massConsumedPerUse = 5f;
    handSanitizer.consumedElement = SimHashes.Water;
    handSanitizer.outputElement = SimHashes.DirtyWater;
    handSanitizer.diseaseRemovalCount = WashBasinConfig.DISEASE_REMOVAL_COUNT;
    handSanitizer.maxUses = 40;
    handSanitizer.dumpWhenFull = true;
    go.AddOrGet<DirectionControl>();
    HandSanitizer.Work work = go.AddOrGet<HandSanitizer.Work>();
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_washbasin_kanim")
    };
    work.overrideAnims = kanimFileArray;
    work.workTime = 5f;
    work.trackUses = true;
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = GameTagExtensions.Create(SimHashes.Water);
    manualDeliveryKg.MinimumMass = 5f;
    manualDeliveryKg.capacity = 200f;
    manualDeliveryKg.refillMass = 40f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().prefabInitFn += new KPrefabID.PrefabFn(this.OnInit);
  }

  private void OnInit(GameObject go)
  {
    HandSanitizer.Work component = go.GetComponent<HandSanitizer.Work>();
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_washbasin_kanim")
    };
    component.workerTypeOverrideAnims.Add((Tag) MinionConfig.ID, kanimFileArray);
    component.workerTypeOverrideAnims.Add((Tag) BionicMinionConfig.ID, new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_bionic_interacts_washbasin_kanim")
    });
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
