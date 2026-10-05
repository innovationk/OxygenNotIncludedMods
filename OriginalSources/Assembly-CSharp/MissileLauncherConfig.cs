// Decompiled with JetBrains decompiler
// Type: MissileLauncherConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MissileLauncherConfig : IBuildingConfig
{
  public const string ID = "MissileLauncher";
  public const string CONDUIT_STORAGE = "CondiutStorage";
  public static List<Tag> CosmicBlastShotTypes = new List<Tag>()
  {
    (Tag) "MissileLongRange"
  };

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] allMetals = TUNING.MATERIALS.ALL_METALS;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    EffectorValues noise = tieR5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("MissileLauncher", 3, 5, "missile_launcher_kanim", 250, 30f, tieR4, allMetals, 1600f, BuildLocationRule.OnFloor, none, noise);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = 400f;
    buildingDef.DefaultAnimState = "off";
    buildingDef.RequiresPowerInput = true;
    buildingDef.PowerInputOffset = new CellOffset(-1, 0);
    buildingDef.EnergyConsumptionWhenActive = 240f;
    buildingDef.InputConduitType = ConduitType.Solid;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.MISSILE);
    buildingDef.POIUnlockable = true;
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    this.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go) => this.AddVisualizer(go);

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGetDef<MissileLauncher.Def>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
    Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go);
    defaultStorage.storageID = (Tag) "MissileBasic";
    defaultStorage.showInUI = true;
    defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    defaultStorage.storageFilters = new List<Tag>()
    {
      (Tag) "MissileBasic"
    };
    defaultStorage.allowSettingOnlyFetchMarkedItems = false;
    defaultStorage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    defaultStorage.capacityKg = 300f;
    ManualDeliveryKG manualDeliveryKg1 = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg1.SetStorage(defaultStorage);
    manualDeliveryKg1.RequestedItemTag = (Tag) "MissileBasic";
    manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    manualDeliveryKg1.operationalRequirement = Operational.State.None;
    manualDeliveryKg1.refillMass = 50f;
    manualDeliveryKg1.MinimumMass = 10f;
    manualDeliveryKg1.capacity = defaultStorage.Capacity();
    Storage storage1 = go.AddComponent<Storage>();
    storage1.storageID = (Tag) "MissileLongRange";
    storage1.showInUI = true;
    storage1.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
    storage1.storageFilters = MissileLauncherConfig.CosmicBlastShotTypes;
    storage1.allowSettingOnlyFetchMarkedItems = false;
    storage1.fetchCategory = Storage.FetchCategory.GeneralStorage;
    storage1.capacityKg = 1000f;
    ManualDeliveryKG manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
    manualDeliveryKg2.SetStorage(storage1);
    manualDeliveryKg2.RequestedItemTag = GameTags.LongRangeMissile;
    manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    manualDeliveryKg2.operationalRequirement = Operational.State.None;
    manualDeliveryKg2.refillMass = 1000f;
    manualDeliveryKg2.MinimumMass = 200f;
    manualDeliveryKg2.capacity = storage1.Capacity();
    manualDeliveryKg2.FillToMinimumMass = true;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.storageID = (Tag) "CondiutStorage";
    storage2.capacityKg = 200f;
    storage2.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    storage2.showInUI = false;
    SolidConduitConsumer solidConduitConsumer = go.AddOrGet<SolidConduitConsumer>();
    solidConduitConsumer.alwaysConsume = true;
    solidConduitConsumer.capacityKG = storage2.capacityKg;
    solidConduitConsumer.storage = storage2;
    if (DlcManager.IsContentSubscribed("EXPANSION1_ID"))
    {
      EntityClusterDestinationSelector destinationSelector = go.AddOrGet<EntityClusterDestinationSelector>();
      destinationSelector.assignable = true;
      destinationSelector.sidescreenTitleString = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE_MISSILE_TARGET;
      destinationSelector.changeTargetButtonTooltipString = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CHANGE_DESTINATION_BUTTON_TOOLTIP_MISSILE;
      destinationSelector.clearTargetButtonTooltipString = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CLEAR_DESTINATION_BUTTON_TOOLTIP_MISSILE;
      destinationSelector.requiredEntityLayer = EntityLayer.Meteor;
    }
    this.AddVisualizer(go);
  }

  private void AddVisualizer(GameObject go1)
  {
    RangeVisualizer rangeVisualizer = go1.AddOrGet<RangeVisualizer>();
    rangeVisualizer.OriginOffset = MissileLauncher.Def.LaunchOffset.ToVector2I();
    rangeVisualizer.RangeMin.x = -MissileLauncher.Def.launchRange.x;
    rangeVisualizer.RangeMax.x = MissileLauncher.Def.launchRange.x;
    rangeVisualizer.RangeMin.y = 0;
    rangeVisualizer.RangeMax.y = MissileLauncher.Def.launchRange.y;
    rangeVisualizer.AllowLineOfSightInvalidCells = true;
    go1.GetComponent<KPrefabID>().instantiateFn += (KPrefabID.PrefabFn) (go2 => go2.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(MissileLauncherConfig.IsCellSkyBlocked));
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    SymbolOverrideControllerUtil.AddToPrefab(go);
    go.AddOrGet<TreeFilterable>().dropIncorrectOnFilterChange = false;
    FlatTagFilterable flatTagFilterable = go.AddComponent<FlatTagFilterable>();
    flatTagFilterable.displayOnlyDiscoveredTags = false;
    flatTagFilterable.headerText = (string) STRINGS.BUILDINGS.PREFABS.MISSILELAUNCHER.TARGET_SELECTION_HEADER;
  }

  public static bool IsCellSkyBlocked(int cell)
  {
    if ((UnityEngine.Object) PlayerController.Instance != (UnityEngine.Object) null)
    {
      int cell1 = Grid.InvalidCell;
      BuildTool activeTool1 = PlayerController.Instance.ActiveTool as BuildTool;
      SelectTool activeTool2 = PlayerController.Instance.ActiveTool as SelectTool;
      if ((UnityEngine.Object) activeTool1 != (UnityEngine.Object) null)
        cell1 = activeTool1.GetLastCell;
      else if ((UnityEngine.Object) activeTool2 != (UnityEngine.Object) null)
        cell1 = Grid.PosToCell((KMonoBehaviour) activeTool2.selected);
      if (Grid.IsValidCell(cell) && Grid.IsValidCell(cell1) && (int) Grid.WorldIdx[cell] == (int) Grid.WorldIdx[cell1])
        return Grid.Solid[cell];
    }
    return false;
  }
}
