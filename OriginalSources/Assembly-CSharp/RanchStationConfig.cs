// Decompiled with JetBrains decompiler
// Type: RanchStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class RanchStationConfig : IBuildingConfig
{
  public const string ID = "RanchStation";

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR1 = NOISE_POLLUTION.NOISY.TIER1;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR1;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RanchStation", 2, 3, "rancherstation_kanim", 30, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.ViewMode = OverlayModes.Rooms.ID;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseRanchStation.Id;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
    RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
    def.IsCritterEligibleToBeRanchedCb = (Func<GameObject, RanchStation.Instance, bool>) ((creature_go, ranch_station_smi) => !creature_go.HasTag(GameTags.Creatures.Swimmer) && !creature_go.GetComponent<Effects>().HasEffect("Ranched"));
    def.RancherWipesBrowAnim = true;
    def.OnRanchCompleteCb = (Action<GameObject, WorkerBase>) ((creature_go, rancher_wb) =>
    {
      creature_go.GetSMI<RanchableMonitor.Instance>().TargetRanchStation.GetSMI<RancherChore.RancherChoreStates.Instance>();
      Attributes attributes = rancher_wb.GetAttributes();
      float num1 = (float) (1.0 + (attributes != null ? (double) attributes.Get(Db.Get().Attributes.Ranching.Id).GetTotalValue() : 0.0) * 0.10000000149011612);
      creature_go.GetComponent<Effects>().Add("Ranched", true).timeRemaining *= num1;
      AmountInstance amountInstance = Db.Get().Amounts.HitPoints.Lookup(creature_go);
      if (amountInstance == null)
        return;
      double num2 = (double) amountInstance.ApplyDelta((float) ((double) amountInstance.GetMax() - (double) amountInstance.value + 1.0));
    });
    def.RanchedPreAnim = (HashedString) "grooming_pre";
    def.RanchedLoopAnim = (HashedString) "grooming_loop";
    def.RanchedPstAnim = (HashedString) "grooming_pst";
    def.WorkTime = 12f;
    def.GetTargetRanchCell = (Func<RanchStation.Instance, int>) (smi =>
    {
      int num = Grid.InvalidCell;
      if (!smi.IsNullOrStopped())
        num = Grid.CellRight(Grid.PosToCell(smi.transform.GetPosition()));
      return num;
    });
    RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
    roomTracker.requirement = RoomTracker.Requirement.Required;
    go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanUseRanchStation.Id;
    Prioritizable.AddRef(go);
  }
}
