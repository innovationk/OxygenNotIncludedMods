// Decompiled with JetBrains decompiler
// Type: RemoteWorkerDockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class RemoteWorkerDockConfig : IBuildingConfig
{
  public static string ID = "RemoteWorkerDock";
  public const float NEW_WORKER_DELAY_SECONDS = 2f;
  public const int WORK_RANGE = 12;
  public const float LUBRICANT_CAPACITY_KG = 50f;
  public const string ON_EMPTY_ANIM = "on_empty";
  public const string ON_FULL_ANIM = "on_full";
  public const string OFF_EMPTY_ANIM = "off_empty";
  public const string OFF_FULL_ANIM = "off_full";
  public const string NEW_WORKER_ANIM = "new_worker";

  public override BuildingDef CreateBuildingDef()
  {
    string id = RemoteWorkerDockConfig.ID;
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] plastics = TUNING.MATERIALS.PLASTICS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR1 = TUNING.BUILDINGS.DECOR.PENALTY.TIER1;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, 1, 2, "remote_work_dock_kanim", 100, 60f, tieR4, plastics, 1600f, BuildLocationRule.OnFloor, tieR1, noise);
    buildingDef.Overheatable = false;
    buildingDef.AudioCategory = "Plastic";
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.UtilityInputOffset = new CellOffset(0, 1);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.SelfHeatKilowattsWhenActive = 2f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AddVisualizer(go);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go) => this.AddVisualizer(go);

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<RemoteWorkerDock>();
    go.AddOrGet<RemoteWorkerDockAnimSM>();
    go.AddOrGet<Operational>();
    go.AddOrGet<UserNameable>();
    go.AddComponent<Storage>().SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.capacityTag = GameTags.LubricatingOil;
    conduitConsumer.capacityKG = 50f;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Liquid;
    conduitDispenser.elementFilter = new SimHashes[1]
    {
      SimHashes.LiquidGunk
    };
    this.AddVisualizer(go);
    go.AddOrGet<RangeVisualizer>();
  }

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  private void AddVisualizer(GameObject prefab)
  {
    RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
    rangeVisualizer.RangeMin.x = -12;
    rangeVisualizer.RangeMin.y = 0;
    rangeVisualizer.RangeMax.x = 12;
    rangeVisualizer.RangeMax.y = 0;
    rangeVisualizer.OriginOffset = new Vector2I();
    rangeVisualizer.BlockingTileVisible = false;
    prefab.GetComponent<KPrefabID>().instantiateFn += (KPrefabID.PrefabFn) (go => go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(RemoteWorkerDockConfig.DockPathBlockingCB));
  }

  public static bool DockPathBlockingCB(int cell)
  {
    int i1 = Grid.CellAbove(cell);
    int i2 = Grid.CellBelow(cell);
    return i1 == Grid.InvalidCell || i2 == Grid.InvalidCell || !Grid.Foundation[i2] && !Grid.Solid[i2] || Grid.Solid[cell] || Grid.Solid[i1];
  }
}
