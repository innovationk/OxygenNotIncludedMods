// Decompiled with JetBrains decompiler
// Type: PowerControlStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class PowerControlStationConfig : IBuildingConfig
{
  public const string ID = "PowerControlStation";
  public static Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;
  public static Tag TINKER_TOOLS = PowerStationToolsConfig.tag;
  public const float MASS_PER_TINKER = 5f;
  public static string ROLE_PERK = "CanPowerTinker";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("PowerControlStation", 2, 4, "electricianworkdesk_kanim", 30, 30f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.POWER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 50f;
    storage.showInUI = true;
    storage.storageFilters = new List<Tag>()
    {
      PowerControlStationConfig.MATERIAL_FOR_TINKER
    };
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    TinkerStation tinkerstation = go.AddOrGet<TinkerStation>();
    tinkerstation.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_electricianworkdesk_kanim")
    };
    tinkerstation.inputMaterial = PowerControlStationConfig.MATERIAL_FOR_TINKER;
    tinkerstation.massPerTinker = 5f;
    tinkerstation.outputPrefab = PowerControlStationConfig.TINKER_TOOLS;
    tinkerstation.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
    tinkerstation.choreType = Db.Get().ChoreTypes.PowerFabricate.IdHash;
    tinkerstation.useFilteredStorage = true;
    tinkerstation.fetchChoreType = Db.Get().ChoreTypes.PowerFetch.IdHash;
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object =>
    {
      TinkerStation component = game_object.GetComponent<TinkerStation>();
      component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
      component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
      component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
      tinkerstation.toolProductionTime = 160f;
    });
  }
}
