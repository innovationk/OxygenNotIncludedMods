// Decompiled with JetBrains decompiler
// Type: RailGunConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class RailGunConfig : IBuildingConfig
{
  public const string ID = "RailGun";
  public const string PORT_ID = "HEP_STORAGE";
  public const int RANGE = 20;
  public const float BASE_PARTICLE_COST = 0.0f;
  public const float HEX_PARTICLE_COST = 10f;
  public const float HEP_CAPACITY = 200f;
  public const float TAKEOFF_VELOCITY = 35f;
  public const int MAINTENANCE_AFTER_NUM_PAYLOADS = 6;
  public const int MAINTENANCE_COOLDOWN = 30;
  public const float CAPACITY = 1200f;
  private ConduitPortInfo solidInputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(-1, 0));
  private ConduitPortInfo liquidInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));
  private ConduitPortInfo gasInputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 0));

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("RailGun", 5, 6, "rail_gun_kanim", 250, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = 400f;
    buildingDef.DefaultAnimState = "off";
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(-2, 0);
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.UseHighEnergyParticleInputPort = true;
    buildingDef.HighEnergyParticleInputOffset = new CellOffset(-2, 1);
    buildingDef.LogicInputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.InputPort(RailGun.PORT_ID, new CellOffset(-2, 2), (string) STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.RAILGUN.LOGIC_PORT_INACTIVE)
    };
    buildingDef.LogicOutputPorts = new List<LogicPorts.Port>()
    {
      LogicPorts.Port.OutputPort((HashedString) "HEP_STORAGE", new CellOffset(2, 0), (string) STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE, (string) STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.HEPENGINE.LOGIC_PORT_STORAGE_INACTIVE)
    };
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    return buildingDef;
  }

  private void AttachPorts(GameObject go)
  {
    go.AddComponent<ConduitSecondaryInput>().portInfo = this.liquidInputPort;
    go.AddComponent<ConduitSecondaryInput>().portInfo = this.gasInputPort;
    go.AddComponent<ConduitSecondaryInput>().portInfo = this.solidInputPort;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    RailGun railGun = go.AddOrGet<RailGun>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    go.AddOrGet<LoopingSounds>();
    ClusterDestinationSelector destinationSelector = go.AddOrGet<ClusterDestinationSelector>();
    destinationSelector.changeTargetButtonTooltipString = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CHANGE_DESTINATION_BUTTON_TOOLTIP_RAILGUN;
    destinationSelector.clearTargetButtonTooltipString = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CLEAR_DESTINATION_BUTTON_TOOLTIP_RAILGUN;
    destinationSelector.assignable = true;
    destinationSelector.requireAsteroidDestination = true;
    railGun.liquidPortInfo = this.liquidInputPort;
    railGun.gasPortInfo = this.gasInputPort;
    railGun.solidPortInfo = this.solidInputPort;
    HighEnergyParticleStorage energyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
    energyParticleStorage.capacity = 200f;
    energyParticleStorage.autoStore = true;
    energyParticleStorage.showInUI = false;
    energyParticleStorage.PORT_ID = "HEP_STORAGE";
    energyParticleStorage.showCapacityStatusItem = true;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    List<Tag> tagList = new List<Tag>();
    tagList.AddRange((IEnumerable<Tag>) STORAGEFILTERS.STORAGE_LOCKERS_STANDARD);
    tagList.AddRange((IEnumerable<Tag>) STORAGEFILTERS.GASES);
    tagList.AddRange((IEnumerable<Tag>) STORAGEFILTERS.FOOD);
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
    defaultStorage.showInUI = true;
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    defaultStorage.storageFilters = tagList;
    defaultStorage.allowSettingOnlyFetchMarkedItems = false;
    defaultStorage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    defaultStorage.capacityKg = 1200f;
    go.GetComponent<HighEnergyParticlePort>().requireOperational = false;
    RailGunConfig.AddVisualizer(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    this.AttachPorts(go);
    RailGunConfig.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    this.AttachPorts(go);
    RailGunConfig.AddVisualizer(go);
  }

  private static void AddVisualizer(GameObject prefab)
  {
    SkyVisibilityVisualizer visibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
    visibilityVisualizer.RangeMin = -2;
    visibilityVisualizer.RangeMax = 1;
    visibilityVisualizer.AllOrNothingVisibility = true;
    prefab.GetComponent<KPrefabID>().instantiateFn += (KPrefabID.PrefabFn) (go => go.GetComponent<SkyVisibilityVisualizer>().SkyVisibilityCb = new Func<int, bool>(RailGunConfig.RailGunSkyVisibility));
  }

  private static bool RailGunSkyVisibility(int cell)
  {
    DebugUtil.DevAssert((UnityEngine.Object) ClusterManager.Instance != (UnityEngine.Object) null, "RailGun assumes DLC");
    if (!Grid.IsValidCell(cell) || Grid.WorldIdx[cell] == byte.MaxValue)
      return false;
    int y = (int) ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[cell]).maximumBounds.y;
    for (int index = cell; Grid.CellRow(index) <= y; index = Grid.CellAbove(index))
    {
      if (!Grid.IsValidCell(index) || Grid.Solid[index])
        return false;
    }
    return true;
  }
}
