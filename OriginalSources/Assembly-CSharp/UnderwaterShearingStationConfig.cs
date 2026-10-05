// Decompiled with JetBrains decompiler
// Type: UnderwaterShearingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class UnderwaterShearingStationConfig : IBuildingConfig
{
  public const string ID = "UnderwaterShearingStation";

  public override string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues tieR2 = BUILDINGS.DECOR.PENALTY.TIER2;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("UnderwaterShearingStation", 3, 3, "shearing_station_aquatic_kanim", 30, 60f, tieR4, refinedMetals, 1600f, BuildLocationRule.OnBackWall, tieR2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseRanchStation.Id;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    go.AddOrGet<BuildingSubmergable>();
    go.AddComponent<UnderwaterShearingStaion>();
    go.AddOrGet<MultiSkillPerkMissingComplainer>().requiredSkillPerks = new string[2]
    {
      Db.Get().SkillPerks.CanUseRanchStation.Id,
      Db.Get().SkillPerks.CanSwim.Id
    };
    RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
    def.IsCritterEligibleToBeRanchedCb = (Func<GameObject, RanchStation.Instance, bool>) ((creature_go, ranch_station_smi) =>
    {
      if (creature_go.GetSMI<FlopMonitor.Instance>() == null)
        return false;
      IShearable smi = creature_go.GetSMI<IShearable>();
      return smi != null && smi.IsFullyGrown();
    });
    def.RancherInteractAnim = (HashedString) "anim_interacts_shearingstation_aquatic_kanim";
    def.RancherCallingAndWipeBrowAnim = (HashedString) "anim_interacts_rancherstation_aquatic_kanim";
    def.RanchedPreAnim = (HashedString) "shearing_pre";
    def.RanchedLoopAnim = (HashedString) "shearing_loop";
    def.RanchedPstAnim = (HashedString) "shearing_pst";
    def.CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingRanched;
    def.RancherWipesBrowAnim = false;
    def.GetTargetRanchCell = (Func<RanchStation.Instance, int>) (smi =>
    {
      int num = Grid.InvalidCell;
      if (!smi.IsNullOrStopped())
        num = Grid.PosToCell(smi.transform.GetPosition());
      return num;
    });
    def.OnRanchCompleteCb = (Action<GameObject, WorkerBase>) ((creature_go, rancher_wb) =>
    {
      Attributes attributes = rancher_wb.GetAttributes();
      float num = attributes != null ? attributes.Get(Db.Get().Attributes.Ranching.Id).GetTotalValue() : 0.0f;
      RanchableMonitor.Instance smi1 = creature_go.GetSMI<RanchableMonitor.Instance>();
      IShearable smi2 = creature_go.GetSMI<IShearable>();
      if (smi2 != null)
      {
        Tuple<Tag, float> itemDroppedOnShear = smi2.GetItemDroppedOnShear();
        this.StoreShearable(smi1.TargetRanchStation.gameObject, creature_go, itemDroppedOnShear.first, itemDroppedOnShear.second);
        smi2.Shear();
      }
      UnderwaterShearingStaion component = smi1.TargetRanchStation.GetComponent<UnderwaterShearingStaion>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.HideShearableSymbol();
    });
    def.OnRanchWorkBegins = (Action<RanchedStates.Instance, Workable>) ((creature, workable) =>
    {
      UnderwaterShearingStaion component = workable.GetComponent<UnderwaterShearingStaion>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      Tuple<Tag, float> itemDroppedOnShear = creature.gameObject.GetSMI<IShearable>().GetItemDroppedOnShear();
      component.UpdateShearableSymbol(itemDroppedOnShear.first);
    });
  }

  private void StoreShearable(
    GameObject station,
    GameObject critter,
    Tag item_dropped,
    float mass)
  {
    PrimaryElement component1 = critter.GetComponent<PrimaryElement>();
    GameObject go = Util.KInstantiate(Assets.GetPrefab(item_dropped));
    int cell = Grid.CellRight(Grid.PosToCell(critter));
    go.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
    PrimaryElement component2 = go.GetComponent<PrimaryElement>();
    component2.Temperature = component1.Temperature;
    component2.Mass = mass;
    component2.AddDisease(component1.DiseaseIdx, component1.DiseaseCount, "Shearing");
    go.SetActive(true);
    Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, (float) ((double) UnityEngine.Random.value * 2.0 + 2.0));
    if (GameComps.Fallers.Has((object) go))
      GameComps.Fallers.Remove(go);
    GameComps.Fallers.Add(go, initial_velocity);
  }
}
