// Decompiled with JetBrains decompiler
// Type: WaterCoolerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class WaterCoolerConfig : IBuildingConfig
{
  public const string ID = "WaterCooler";
  public static Tuple<Tag, string>[] BEVERAGE_CHOICE_OPTIONS = new Tuple<Tag, string>[3]
  {
    new Tuple<Tag, string>(SimHashes.Water.CreateTag(), ""),
    new Tuple<Tag, string>(SimHashes.Milk.CreateTag(), "DuplicantGotMilk"),
    new Tuple<Tag, string>(SimHashes.Ink.CreateTag(), "DuplicantDrankInk")
  };
  public const string MilkEffectID = "DuplicantGotMilk";
  public const string InkEffectID = "DuplicantDrankInk";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = TUNING.MATERIALS.RAW_MINERALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.BONUS.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WaterCooler", 2, 2, "watercooler_kanim", 30, 10f, tieR4, rawMinerals, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.WATER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding);
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.capacityKg = 10f;
    storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Insulate
    });
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage(storage);
    manualDeliveryKg.capacity = 10f;
    manualDeliveryKg.refillMass = 9f;
    manualDeliveryKg.MinimumMass = 1f;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    HeatImmunityProvider.Def def1 = go.AddOrGetDef<HeatImmunityProvider.Def>();
    def1.range = new CellOffset[2][]
    {
      new CellOffset[1]{ new CellOffset(1, 0) },
      new CellOffset[1]{ new CellOffset(0, 0) }
    };
    def1.overrideFileName = new Func<GameObject, string>(this.GetImmunityProviderAnimFileName);
    def1.overrideAnims = new string[3]
    {
      "working_pre",
      "working_loop",
      "working_pst"
    };
    def1.specialRequirements = new Func<GameObject, bool>(this.RefreshFromHeatCondition);
    HeatImmunityProvider.Def def2 = def1;
    def2.onEffectApplied = def2.onEffectApplied + new Action<GameObject, HeatImmunityProvider.Instance>(this.OnHeatImmunityEffectApplied);
    go.AddOrGet<WaterCooler>();
    WaterCooler.OnDuplicantDrank += new Action<GameObject, GameObject>(this.ApplyImmunityEffectWhenDrankRecreationally);
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
    roomTracker.requirement = RoomTracker.Requirement.Recommended;
    go.AddOrGetDef<RocketUsageRestriction.Def>();
  }

  private string GetImmunityProviderAnimFileName(GameObject theEntitySeekingImmunity)
  {
    if ((UnityEngine.Object) theEntitySeekingImmunity == (UnityEngine.Object) null)
      return "anim_interacts_watercooler_kanim";
    MinionIdentity component = theEntitySeekingImmunity.GetComponent<MinionIdentity>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.model == BionicMinionConfig.MODEL ? "anim_bionic_interacts_watercooler_kanim" : "anim_interacts_watercooler_kanim";
  }

  private void ApplyImmunityEffectWhenDrankRecreationally(
    GameObject duplicant,
    GameObject waterCoolerInstance)
  {
    waterCoolerInstance.GetSMI<HeatImmunityProvider.Instance>()?.ApplyImmunityEffect(duplicant, false);
  }

  private void OnHeatImmunityEffectApplied(GameObject duplicant, HeatImmunityProvider.Instance smi)
  {
    smi.GetSMI<WaterCooler.StatesInstance>().Drink(duplicant, false);
  }

  private bool RefreshFromHeatCondition(GameObject go_instance)
  {
    WaterCooler.StatesInstance smi = go_instance.GetSMI<WaterCooler.StatesInstance>();
    return smi != null && smi.IsInsideState((StateMachine.BaseState) smi.sm.dispensing);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
