// Decompiled with JetBrains decompiler
// Type: MorbRoverMakerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MorbRoverMakerConfig : IBuildingConfig
{
  public const string ID = "MorbRoverMaker";
  public const float TUNING_MAX_DESIRED_ROVERS_ALIVE_AT_ONCE = 6f;
  public const int TARGET_AMOUNT_FLOWERS = 10;
  public const float INITIAL_MORB_DEVELOPMENT_PERCENTAGE = 0.5f;
  public static Tag ROVER_PREFAB_ID = (Tag) "MorbRover";
  public static SimHashes ROVER_MATERIAL_TAG = SimHashes.Steel;
  public const float MATERIAL_MASS_PER_ROVER = 300f;
  public const float ROVER_CRAFTING_DURATION = 15f;
  public const float INPUT_MATERIAL_STORAGE_CAPACITY = 1800f;
  public const int MAX_GERMS_TAKEN_PER_PACKAGE = 10000;
  public const long GERMS_PER_ROVER = 9850000;
  public static int GERM_TYPE = (int) Db.Get().Diseases.GetIndex((HashedString) "ZombieSpores");
  public ConduitType GERM_INTAKE_CONDUIT_TYPE = ConduitType.Gas;
  public const float PREDICTED_DURATION_TO_GROW_MORB = 985f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    EffectorValues tieR1 = BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = tieR2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MorbRoverMaker", 5, 4, "gravitas_morb_tank_kanim", 250, 120f, tieR5, refinedMetals, 3200f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Floodable = true;
    buildingDef.Entombable = true;
    buildingDef.ShowInBuildMenu = false;
    buildingDef.Overheatable = false;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.AudioCategory = "Glass";
    buildingDef.AudioSize = "medium";
    buildingDef.UseStructureTemperature = false;
    buildingDef.InputConduitType = this.GERM_INTAKE_CONDUIT_TYPE;
    buildingDef.OutputConduitType = this.GERM_INTAKE_CONDUIT_TYPE;
    buildingDef.UtilityInputOffset = new CellOffset(1, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(2, 3);
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddTag(GameTags.Gravitas);
    go.GetComponent<Deconstructable>().allowDeconstruction = false;
    Prioritizable.AddRef(go);
    PrimaryElement component = go.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Steel);
    component.Temperature = 294.15f;
    Storage storage = go.AddOrGet<Storage>();
    storage.storageFilters = this.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Gas ? new List<Tag>((IEnumerable<Tag>) STORAGEFILTERS.GASES) : new List<Tag>((IEnumerable<Tag>) STORAGEFILTERS.LIQUIDS);
    storage.storageFilters.Add(MorbRoverMakerConfig.ROVER_MATERIAL_TAG.CreateTag());
    storage.allowItemRemoval = false;
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = MorbRoverMakerConfig.ROVER_MATERIAL_TAG.CreateTag();
    manualDeliveryKg.capacity = 1800f;
    manualDeliveryKg.refillMass = 300f;
    manualDeliveryKg.MinimumMass = 300f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
    go.AddOrGet<Operational>();
    go.AddOrGet<Demolishable>().allowDemolition = true;
    go.AddOrGet<MorbRoverMakerWorkable>();
    go.AddOrGet<MorbRoverMakerRevealWorkable>();
    go.AddOrGet<MorbRoverMaker_Capsule>();
    MorbRoverMaker.Def def = go.AddOrGetDef<MorbRoverMaker.Def>();
    def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE = 0.5f;
    def.ROVER_PREFAB_ID = MorbRoverMakerConfig.ROVER_PREFAB_ID;
    def.ROVER_CRAFTING_DURATION = 15f;
    def.ROVER_MATERIAL = MorbRoverMakerConfig.ROVER_MATERIAL_TAG;
    def.METAL_PER_ROVER = 300f;
    def.GERMS_PER_ROVER = 9850000L;
    def.MAX_GERMS_TAKEN_PER_PACKAGE = 10000;
    def.GERM_TYPE = MorbRoverMakerConfig.GERM_TYPE;
    def.GERM_INTAKE_CONDUIT_TYPE = this.GERM_INTAKE_CONDUIT_TYPE;
    go.AddOrGetDef<MorbRoverMakerStorytrait.Def>();
    go.AddOrGetDef<MorbRoverMakerDisplay.Def>();
    go.AddOrGet<LoopingSounds>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    Object.DestroyImmediate((Object) go.GetComponent<RequireInputs>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitConsumer>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitDispenser>());
    Object.DestroyImmediate((Object) go.GetComponent<AutoDisinfectable>());
    Object.DestroyImmediate((Object) go.GetComponent<Disinfectable>());
  }
}
