// Decompiled with JetBrains decompiler
// Type: EggIncubatorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class EggIncubatorConfig : IBuildingConfig
{
  public const string ID = "EggIncubator";
  public static readonly List<Storage.StoredItemModifier> IncubatorStorage = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Preserve
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = TUNING.MATERIALS.REFINED_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("EggIncubator", 2, 3, "incubator_kanim", 30, 120f, tieR3, refinedMetals, 1600f, BuildLocationRule.OnFloor, tieR0, noise);
    buildingDef.AudioCategory = "Metal";
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 4f;
    buildingDef.OverheatTemperature = 363.15f;
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    BuildingTemplates.CreateDefaultStorage(go).SetDefaultStoredItemModifiers(EggIncubatorConfig.IncubatorStorage);
    EggIncubator eggIncubator = go.AddOrGet<EggIncubator>();
    eggIncubator.AddDepositTag(GameTags.Egg);
    eggIncubator.SetWorkTime(5f);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
