// Decompiled with JetBrains decompiler
// Type: ShearingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
public class ShearingStationConfig : IBuildingConfig
{
  public const string ID = "ShearingStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMetals = MATERIALS.RAW_METALS;
    EffectorValues none1 = NOISE_POLLUTION.NONE;
    EffectorValues none2 = BUILDINGS.DECOR.NONE;
    EffectorValues noise = none1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ShearingStation", 3, 3, "shearing_station_kanim", 100, 10f, tieR4, rawMetals, 1600f, BuildLocationRule.OnFloor, none2, noise);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
    buildingDef.Floodable = true;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.DefaultAnimState = "on";
    buildingDef.ShowInBuildMenu = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
    def.IsCritterEligibleToBeRanchedCb = (Func<GameObject, RanchStation.Instance, bool>) ((creature_go, ranch_station_smi) =>
    {
      IShearable smi = creature_go.GetSMI<IShearable>();
      return smi != null && smi.IsFullyGrown();
    });
    def.OnRanchCompleteCb = (Action<GameObject, WorkerBase>) ((creature_go, rancher_wb) =>
    {
      IShearable smi = creature_go.GetSMI<IShearable>();
      Tuple<Tag, float> itemDroppedOnShear = smi.GetItemDroppedOnShear();
      this.DropShearable(rancher_wb.gameObject, creature_go, itemDroppedOnShear.first, itemDroppedOnShear.second);
      smi.Shear();
    });
    def.RancherInteractAnim = (HashedString) "anim_interacts_shearingstation_kanim";
    def.WorkTime = 12f;
    def.RanchedPreAnim = (HashedString) "shearing_pre";
    def.RanchedLoopAnim = (HashedString) "shearing_loop";
    def.RanchedPstAnim = (HashedString) "shearing_pst";
    def.RancherWipesBrowAnim = false;
    go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanUseRanchStation.Id;
    Prioritizable.AddRef(go);
  }

  private void DropShearable(GameObject go, GameObject critter, Tag item_dropped, float mass)
  {
    PrimaryElement component1 = critter.GetComponent<PrimaryElement>();
    GameObject go1 = Util.KInstantiate(Assets.GetPrefab(item_dropped));
    int cell = Grid.CellLeft(Grid.PosToCell(go));
    go1.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
    PrimaryElement component2 = go1.GetComponent<PrimaryElement>();
    component2.Temperature = component1.Temperature;
    component2.Mass = mass;
    component2.AddDisease(component1.DiseaseIdx, component1.DiseaseCount, "Shearing");
    go1.SetActive(true);
    Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, (float) ((double) UnityEngine.Random.value * 2.0 + 2.0));
    if (GameComps.Fallers.Has((object) go1))
      GameComps.Fallers.Remove(go1);
    GameComps.Fallers.Add(go1, initial_velocity);
  }
}
