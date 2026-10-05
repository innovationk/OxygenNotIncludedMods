// Decompiled with JetBrains decompiler
// Type: StructureToStructureTemperature
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StructureToStructureTemperature : KMonoBehaviour
{
  [MyCmpGet]
  private Building building;
  private List<int> conductiveCells;
  private HashSet<int> inContactBuildings = new HashSet<int>();
  private bool hasBeenRegister;
  private bool buildingDestroyed;
  private int selfHandle;
  protected static readonly EventSystem.IntraObjectHandler<StructureToStructureTemperature> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<StructureToStructureTemperature>((Action<StructureToStructureTemperature, object>) ((component, data) => component.OnStructureTemperatureRegistered(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<StructureToStructureTemperature>(-1555603773, StructureToStructureTemperature.OnStructureTemperatureRegisteredDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.DefineConductiveCells();
    GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
    this.UnregisterToSIM();
    base.OnCleanUp();
  }

  private void OnStructureTemperatureRegistered(object _sim_handle)
  {
    this.RegisterToSIM(((Boxed<int>) _sim_handle).value);
  }

  private void RegisterToSIM(int sim_handle1)
  {
    string name = this.building.Def.Name;
    HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle = Game.Instance.simComponentCallbackManager.Add((Action<int, object>) ((sim_handle2, callback_data) => this.OnSimRegistered(sim_handle2)), (object) null, "StructureToStructureTemperature.SimRegister");
    SimMessages.RegisterBuildingToBuildingHeatExchange(sim_handle1, handle.index);
  }

  private void OnSimRegistered(int sim_handle)
  {
    if (sim_handle == -1)
      return;
    this.selfHandle = sim_handle;
    this.hasBeenRegister = true;
    if (this.buildingDestroyed)
      this.UnregisterToSIM();
    else
      this.Refresh_InContactBuildings();
  }

  private void UnregisterToSIM()
  {
    if (this.hasBeenRegister)
      SimMessages.RemoveBuildingToBuildingHeatExchange(this.selfHandle);
    this.buildingDestroyed = true;
  }

  private void DefineConductiveCells()
  {
    this.conductiveCells = new List<int>((IEnumerable<int>) this.building.PlacementCells);
    this.conductiveCells.Remove(this.building.GetUtilityInputCell());
    this.conductiveCells.Remove(this.building.GetUtilityOutputCell());
  }

  private void Add(
    StructureToStructureTemperature.InContactBuildingData buildingData)
  {
    if (!this.inContactBuildings.Add(buildingData.buildingInContact))
      return;
    SimMessages.AddBuildingToBuildingHeatExchange(this.selfHandle, buildingData.buildingInContact, buildingData.cellsInContact);
  }

  private void Remove(int building)
  {
    if (!this.inContactBuildings.Contains(building))
      return;
    this.inContactBuildings.Remove(building);
    SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(this.selfHandle, building);
  }

  private void OnAnyBuildingChanged(int _cell, object _data)
  {
    if (!this.hasBeenRegister)
      return;
    StructureToStructureTemperature.BuildingChangedObj buildingChangedObj = (StructureToStructureTemperature.BuildingChangedObj) _data;
    bool flag = false;
    int num = 0;
    for (int index = 0; index < buildingChangedObj.building.PlacementCells.Length; ++index)
    {
      if (this.conductiveCells.Contains(buildingChangedObj.building.PlacementCells[index]))
      {
        flag = true;
        ++num;
      }
    }
    if (!flag)
      return;
    int simHandler = buildingChangedObj.simHandler;
    switch (buildingChangedObj.changeType)
    {
      case StructureToStructureTemperature.BuildingChangeType.Created:
        this.Add(new StructureToStructureTemperature.InContactBuildingData()
        {
          buildingInContact = simHandler,
          cellsInContact = num
        });
        break;
      case StructureToStructureTemperature.BuildingChangeType.Destroyed:
        this.Remove(simHandler);
        break;
    }
  }

  private void Refresh_InContactBuildings()
  {
    foreach (StructureToStructureTemperature.InContactBuildingData inContactBuilding in this.GetAll_InContact_Buildings())
      this.Add(inContactBuilding);
  }

  private List<StructureToStructureTemperature.InContactBuildingData> GetAll_InContact_Buildings()
  {
    Dictionary<Building, int> dictionary = new Dictionary<Building, int>();
    List<StructureToStructureTemperature.InContactBuildingData> contactBuildings = new List<StructureToStructureTemperature.InContactBuildingData>();
    List<GameObject> buildingsInCell = new List<GameObject>();
    foreach (int conductiveCell in this.conductiveCells)
    {
      int cell = conductiveCell;
      buildingsInCell.Clear();
      Action<int> action = (Action<int>) (layer =>
      {
        GameObject gameObject = Grid.Objects[cell, layer];
        if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null) || buildingsInCell.Contains(gameObject))
          return;
        buildingsInCell.Add(gameObject);
      });
      action(1);
      action(26);
      action(27);
      action(31 /*0x1F*/);
      action(32 /*0x20*/);
      action(30);
      action(12);
      action(13);
      action(16 /*0x10*/);
      action(17);
      action(24);
      action(2);
      for (int index = 0; index < buildingsInCell.Count; ++index)
      {
        Building component = (UnityEngine.Object) buildingsInCell[index] == (UnityEngine.Object) null ? (Building) null : buildingsInCell[index].GetComponent<Building>();
        if ((!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.Def.UseStructureTemperature ? 0 : (component.PlacementCellsContainCell(cell) ? 1 : 0)) != 0)
        {
          if (!dictionary.ContainsKey(component))
            dictionary.Add(component, 0);
          dictionary[component]++;
        }
      }
    }
    foreach (Building key in dictionary.Keys)
    {
      HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle((MonoBehaviour) key);
      if (handle != HandleVector<int>.InvalidHandle)
      {
        int simHandleCopy = GameComps.StructureTemperatures.GetPayload(handle).simHandleCopy;
        StructureToStructureTemperature.InContactBuildingData contactBuildingData = new StructureToStructureTemperature.InContactBuildingData()
        {
          buildingInContact = simHandleCopy,
          cellsInContact = dictionary[key]
        };
        contactBuildings.Add(contactBuildingData);
      }
    }
    return contactBuildings;
  }

  public enum BuildingChangeType
  {
    Created,
    Destroyed,
    Moved,
  }

  public struct InContactBuildingData
  {
    public int buildingInContact;
    public int cellsInContact;
  }

  public struct BuildingChangedObj(
    StructureToStructureTemperature.BuildingChangeType _changeType,
    Building _building,
    int sim_handler)
  {
    public StructureToStructureTemperature.BuildingChangeType changeType = _changeType;
    public int simHandler = sim_handler;
    public Building building = _building;
  }
}
