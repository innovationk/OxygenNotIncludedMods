// Decompiled with JetBrains decompiler
// Type: FarmStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TUNING;
using UnityEngine;

#nullable disable
public class FarmStationConfig : IBuildingConfig
{
  public const string ID = "FarmStation";
  public static Tag MATERIAL_FOR_TINKER = GameTags.Fertilizer;
  public static Tag TINKER_TOOLS = FarmStationToolsConfig.tag;
  public const float MASS_PER_TINKER = 5f;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FarmStation", 2, 3, "planttender_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanFarmStation.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FARM);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.FarmStationType);
    go.GetComponent<KPrefabID>().AddTag(GameTags.CodexCategories.FarmBuilding);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.RequestedItemTag = FarmStationConfig.MATERIAL_FOR_TINKER;
    manualDeliveryKg.refillMass = 5f;
    manualDeliveryKg.capacity = 50f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
    TinkerStation tinkerStation = go.AddOrGet<TinkerStation>();
    tinkerStation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_planttender_kanim")
    };
    tinkerStation.inputMaterial = FarmStationConfig.MATERIAL_FOR_TINKER;
    tinkerStation.massPerTinker = 5f;
    tinkerStation.outputPrefab = FarmStationConfig.TINKER_TOOLS;
    tinkerStation.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
    tinkerStation.choreType = Db.Get().ChoreTypes.FarmingFabricate.IdHash;
    tinkerStation.fetchChoreType = Db.Get().ChoreTypes.FarmFetch.IdHash;
    tinkerStation.EffectTitle = (string) UI.BUILDINGEFFECTS.IMPROVED_PLANTS;
    tinkerStation.EffectTooltip = (string) UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_PLANTS;
    tinkerStation.EffectItemString = (string) UI.BUILDINGEFFECTS.IMPROVED_PLANTS_ITEM;
    tinkerStation.EffectItemTooltip = (string) UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_PLANTS_ITEM;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object =>
    {
      TinkerStation component = game_object.GetComponent<TinkerStation>();
      component.AttributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
      component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
      component.toolProductionTime = 15f;
    });
  }
}
