// Decompiled with JetBrains decompiler
// Type: StructureTemperatureComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class StructureTemperatureComponents : 
  KGameObjectSplitComponentManager<StructureTemperatureHeader, StructureTemperaturePayload>
{
  private const float MAX_PRESSURE = 1.5f;
  private static Dictionary<int, HandleVector<int>.Handle> handleInstanceMap = new Dictionary<int, HandleVector<int>.Handle>();
  private StatusItem operatingEnergyStatusItem;

  public HandleVector<int>.Handle Add(GameObject go)
  {
    StructureTemperaturePayload payload = new StructureTemperaturePayload(go);
    return this.Add(go, new StructureTemperatureHeader()
    {
      dirty = false,
      simHandle = -1,
      isActiveBuilding = false
    }, ref payload);
  }

  public static void ClearInstanceMap() => StructureTemperatureComponents.handleInstanceMap.Clear();

  protected override void OnPrefabInit(HandleVector<int>.Handle handle)
  {
    this.InitializeStatusItem();
    base.OnPrefabInit(handle);
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    this.GetData(handle, out header, out payload);
    payload.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(StructureTemperatureComponents.OnGetTemperature);
    payload.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(StructureTemperatureComponents.OnSetTemperature);
    header.isActiveBuilding = (double) payload.building.Def.SelfHeatKilowattsWhenActive != 0.0 || (double) payload.ExhaustKilowatts != 0.0;
    this.SetHeader(handle, header);
  }

  private void InitializeStatusItem()
  {
    if (this.operatingEnergyStatusItem != null)
      return;
    this.operatingEnergyStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.operatingEnergyStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, ev_data) =>
    {
      int key = (int) ev_data;
      StructureTemperaturePayload payload = this.GetPayload(StructureTemperatureComponents.handleInstanceMap[key]);
      if (str != (string) BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP)
      {
        try
        {
          str = string.Format(str, (object) GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f));
        }
        catch (Exception ex)
        {
          Debug.LogWarning((object) ex);
          Debug.LogWarning((object) BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP);
          Debug.LogWarning((object) str);
        }
      }
      else
      {
        string str1 = "";
        foreach (StructureTemperaturePayload.EnergySource energySource in payload.energySourcesKW)
          str1 += string.Format((string) BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, (object) energySource.source, (object) GameUtil.GetFormattedHeatEnergy(energySource.value * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
        str = string.Format(str, (object) GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S), (object) str1);
      }
      return str;
    });
  }

  protected override void OnSpawn(HandleVector<int>.Handle handle)
  {
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    this.GetData(handle, out header, out payload);
    if ((UnityEngine.Object) payload.operational != (UnityEngine.Object) null && header.isActiveBuilding)
      payload.primaryElement.Subscribe(824508782, (Action<object>) (ev_data => StructureTemperatureComponents.OnActiveChanged(handle)));
    payload.maxTemperature = (UnityEngine.Object) payload.overheatable != (UnityEngine.Object) null ? payload.overheatable.OverheatTemperature : 10000f;
    if ((double) payload.maxTemperature <= 0.0)
      Debug.LogError((object) "invalid max temperature");
    this.SetPayload(handle, ref payload);
    this.SimRegister(handle, ref header, ref payload);
  }

  private static void OnActiveChanged(HandleVector<int>.Handle handle)
  {
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    GameComps.StructureTemperatures.GetData(handle, out header, out payload);
    payload.primaryElement.InternalTemperature = payload.Temperature;
    header.dirty = true;
    GameComps.StructureTemperatures.SetHeader(handle, header);
  }

  protected override void OnCleanUp(HandleVector<int>.Handle handle)
  {
    this.SimUnregister(handle);
    base.OnCleanUp(handle);
  }

  public override void Sim200ms(float dt)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    List<StructureTemperatureHeader> headers;
    List<StructureTemperaturePayload> payloads;
    this.GetDataLists(out headers, out payloads);
    StructureTemperaturePayload payload = new StructureTemperaturePayload();
    for (int index = 0; index != headers.Count; ++index)
    {
      StructureTemperatureHeader temperatureHeader = headers[index];
      if (Sim.IsValidHandle(temperatureHeader.simHandle))
      {
        int num4 = temperatureHeader.dirty ? 1 : (temperatureHeader.isActiveBuilding ? 1 : 0);
        if (num4 != 0)
          payload = payloads[index];
        if (temperatureHeader.dirty)
        {
          StructureTemperatureComponents.UpdateSimState(ref payload);
          if ((double) payload.pendingEnergyModifications != 0.0)
          {
            SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, payload.pendingEnergyModifications, 0.0f, 10000f);
            payload.pendingEnergyModifications = 0.0f;
          }
          temperatureHeader.dirty = false;
          headers[index] = temperatureHeader;
        }
        if (temperatureHeader.isActiveBuilding)
        {
          if ((UnityEngine.Object) payload.operational == (UnityEngine.Object) null || payload.operational.IsActive)
          {
            ++num1;
            if (!payload.isActiveStatusItemSet)
            {
              ++num3;
              payload.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, this.operatingEnergyStatusItem, (object) payload.simHandleCopy);
              payload.isActiveStatusItemSet = true;
            }
            payload.energySourcesKW = this.AccumulateProducedEnergyKW(payload.energySourcesKW, payload.OperatingKilowatts, (string) BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING);
            if ((double) payload.ExhaustKilowatts != 0.0)
            {
              ++num2;
              StructureTemperatureComponents.ExhaustHeat(payload.GetExtents(), payload.ExhaustKilowatts, payload.maxTemperature, dt);
              payload.energySourcesKW = this.AccumulateProducedEnergyKW(payload.energySourcesKW, payload.ExhaustKilowatts, (string) BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING);
            }
          }
          else if (payload.isActiveStatusItemSet)
          {
            ++num3;
            payload.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, (StatusItem) null);
            payload.isActiveStatusItemSet = false;
          }
        }
        if (num4 != 0)
          payloads[index] = payload;
      }
    }
  }

  public static void ExhaustHeat(Extents extents, float kw, float maxTemperature, float dt)
  {
    int num1 = extents.width * extents.height;
    float num2 = kw * dt / (float) num1;
    for (int index1 = 0; index1 < extents.height; ++index1)
    {
      int num3 = extents.y + index1;
      for (int index2 = 0; index2 < extents.width; ++index2)
      {
        int num4 = extents.x + index2;
        int num5 = num3 * Grid.WidthInCells + num4;
        float num6 = Mathf.Min(Grid.Mass[num5], 1.5f) / 1.5f;
        float kilojoules = num2 * num6;
        SimMessages.ModifyEnergy(num5, kilojoules, maxTemperature, SimMessages.EnergySourceID.StructureTemperature);
      }
    }
  }

  private static void UpdateSimState(ref StructureTemperaturePayload payload)
  {
    DebugUtil.Assert(Sim.IsValidHandle(payload.simHandleCopy));
    float internalTemperature = payload.primaryElement.InternalTemperature;
    BuildingDef def = payload.building.Def;
    float mass = def.MassForTemperatureModification;
    float operatingKilowatts = payload.OperatingKilowatts;
    float overheat_temperature = (UnityEngine.Object) payload.overheatable != (UnityEngine.Object) null ? payload.overheatable.OverheatTemperature : 10000f;
    if (!payload.enabled || payload.bypass)
      mass = 0.0f;
    Extents extents = payload.GetExtents();
    ushort idx = payload.primaryElement.Element.idx;
    if ((UnityEngine.Object) payload.heatEffect != (UnityEngine.Object) null)
    {
      float num = (UnityEngine.Object) payload.operational == (UnityEngine.Object) null || payload.operational.IsActive ? payload.ExhaustKilowatts : 0.0f;
      payload.heatEffect.SetHeatBeingProducedValue(Mathf.Clamp(operatingKilowatts + num, 0.0f, float.MaxValue));
    }
    SimMessages.ModifyBuildingHeatExchange(payload.simHandleCopy, extents, mass, internalTemperature, def.ThermalConductivity, overheat_temperature, operatingKilowatts, idx);
  }

  private static unsafe float OnGetTemperature(PrimaryElement primary_element)
  {
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
    StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
    float temperature;
    if (Sim.IsValidHandle(payload.simHandleCopy) && payload.enabled)
    {
      if (!payload.bypass)
      {
        temperature = Game.Instance.simData.buildingTemperatures[Sim.GetHandleIndex(payload.simHandleCopy)].temperature;
      }
      else
      {
        int cell = Grid.PosToCell(payload.primaryElement.transform.GetPosition());
        temperature = Grid.Temperature[cell];
      }
    }
    else
      temperature = payload.primaryElement.InternalTemperature;
    return temperature;
  }

  private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
  {
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    GameComps.StructureTemperatures.GetData(handle, out header, out payload);
    payload.primaryElement.InternalTemperature = temperature;
    header.dirty = true;
    GameComps.StructureTemperatures.SetHeader(handle, header);
    if (header.isActiveBuilding || !Sim.IsValidHandle(payload.simHandleCopy))
      return;
    StructureTemperatureComponents.UpdateSimState(ref payload);
    if ((double) payload.pendingEnergyModifications == 0.0)
      return;
    SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, payload.pendingEnergyModifications, 0.0f, 10000f);
    payload.pendingEnergyModifications = 0.0f;
    GameComps.StructureTemperatures.SetPayload(handle, ref payload);
  }

  public void ProduceEnergy(
    HandleVector<int>.Handle handle,
    float delta_kilojoules,
    string source,
    float display_dt)
  {
    StructureTemperaturePayload payload = this.GetPayload(handle);
    if (Sim.IsValidHandle(payload.simHandleCopy))
    {
      SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, delta_kilojoules, 0.0f, 10000f);
    }
    else
    {
      payload.pendingEnergyModifications += delta_kilojoules;
      StructureTemperatureHeader header = this.GetHeader(handle) with
      {
        dirty = true
      };
      this.SetHeader(handle, header);
    }
    payload.energySourcesKW = this.AccumulateProducedEnergyKW(payload.energySourcesKW, delta_kilojoules / display_dt, source);
    this.SetPayload(handle, ref payload);
  }

  private List<StructureTemperaturePayload.EnergySource> AccumulateProducedEnergyKW(
    List<StructureTemperaturePayload.EnergySource> sources,
    float kw,
    string source)
  {
    if (sources == null)
      sources = new List<StructureTemperaturePayload.EnergySource>();
    bool flag = false;
    for (int index = 0; index < sources.Count; ++index)
    {
      if (sources[index].source == source)
      {
        sources[index].Accumulate(kw);
        flag = true;
        break;
      }
    }
    if (!flag)
      sources.Add(new StructureTemperaturePayload.EnergySource(kw, source));
    return sources;
  }

  public static void DoStateTransition(int sim_handle)
  {
    HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
    if (!StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
      return;
    StructureTemperatureComponents.DoMelt(GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement);
  }

  public static void DoMelt(PrimaryElement primary_element)
  {
    Element element = primary_element.Element;
    if (element.highTempTransitionTarget == SimHashes.Unobtanium)
      return;
    SimMessages.AddRemoveSubstance(Grid.PosToCell(primary_element.transform.GetPosition()), element.highTempTransitionTarget, CellEventLogger.Instance.OreMelted, primary_element.Mass, primary_element.Element.highTemp, primary_element.DiseaseIdx, primary_element.DiseaseCount);
    Building.CreateBuildingMeltedNotification(primary_element.gameObject);
    Util.KDestroyGameObject(primary_element.gameObject);
  }

  public static void DoOverheat(int sim_handle)
  {
    HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
    if (!StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
      return;
    GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(1832602615);
  }

  public static void DoNoLongerOverheated(int sim_handle)
  {
    HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
    if (!StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
      return;
    GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(171119937);
  }

  public bool IsEnabled(HandleVector<int>.Handle handle) => this.GetPayload(handle).enabled;

  private void Enable(HandleVector<int>.Handle handle, bool isEnabled)
  {
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    this.GetData(handle, out header, out payload);
    header.dirty = true;
    payload.enabled = isEnabled;
    this.SetData(handle, header, ref payload);
  }

  public void Enable(HandleVector<int>.Handle handle) => this.Enable(handle, true);

  public void Disable(HandleVector<int>.Handle handle) => this.Enable(handle, false);

  public bool IsBypassed(HandleVector<int>.Handle handle) => this.GetPayload(handle).bypass;

  private void Bypass(HandleVector<int>.Handle handle, bool bypass)
  {
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    this.GetData(handle, out header, out payload);
    header.dirty = true;
    payload.bypass = bypass;
    this.SetData(handle, header, ref payload);
  }

  public void Bypass(HandleVector<int>.Handle handle) => this.Bypass(handle, true);

  public void UnBypass(HandleVector<int>.Handle handle) => this.Bypass(handle, false);

  protected void SimRegister(
    HandleVector<int>.Handle handle,
    ref StructureTemperatureHeader header,
    ref StructureTemperaturePayload payload)
  {
    if (payload.simHandleCopy != -1)
      return;
    PrimaryElement primaryElement = payload.primaryElement;
    if ((double) primaryElement.Mass <= 0.0)
      return;
    if (primaryElement.Element == null)
      primaryElement.ElementID = ElementLoader.GetElementID(GameTags.IronOre);
    if (primaryElement.Element.IsTemperatureInsulated)
      return;
    payload.simHandleCopy = -2;
    string dbg_name = primaryElement.name;
    HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle1 = Game.Instance.simComponentCallbackManager.Add((Action<int, object>) ((sim_handle, callback_data) => StructureTemperatureComponents.OnSimRegistered(handle, sim_handle, dbg_name)), (object) null, "StructureTemperature.SimRegister");
    BuildingDef def = primaryElement.GetComponent<Building>().Def;
    float internalTemperature = primaryElement.InternalTemperature;
    float temperatureModification = def.MassForTemperatureModification;
    float operatingKilowatts = payload.OperatingKilowatts;
    Extents extents = payload.GetExtents();
    ushort idx = primaryElement.Element.idx;
    double mass = (double) temperatureModification;
    double temperature = (double) internalTemperature;
    double thermalConductivity = (double) def.ThermalConductivity;
    double operating_kw = (double) operatingKilowatts;
    int elem_idx = (int) idx;
    int index = handle1.index;
    SimMessages.AddBuildingHeatExchange(extents, (float) mass, (float) temperature, (float) thermalConductivity, (float) operating_kw, (ushort) elem_idx, index);
    header.simHandle = payload.simHandleCopy;
    this.SetData(handle, header, ref payload);
  }

  private static void OnSimRegistered(
    HandleVector<int>.Handle handle,
    int sim_handle,
    string dbg_name)
  {
    if (!GameComps.StructureTemperatures.IsValid(handle) || !GameComps.StructureTemperatures.IsVersionValid(handle))
      return;
    StructureTemperatureHeader header;
    StructureTemperaturePayload payload;
    GameComps.StructureTemperatures.GetData(handle, out header, out payload);
    if (payload.simHandleCopy == -2)
    {
      StructureTemperatureComponents.handleInstanceMap[sim_handle] = handle;
      header.simHandle = sim_handle;
      payload.simHandleCopy = sim_handle;
      GameComps.StructureTemperatures.SetData(handle, header, ref payload);
      payload.primaryElement.BoxingTrigger<int>(-1555603773, sim_handle);
      GameScenePartitioner.Instance.TriggerEvent(Grid.PosToCell(payload.building.transform.GetPosition()), GameScenePartitioner.Instance.contactConductiveLayer, (object) new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Created, payload.building, sim_handle));
    }
    else
      SimMessages.RemoveBuildingHeatExchange(sim_handle);
  }

  protected unsafe void SimUnregister(HandleVector<int>.Handle handle)
  {
    if (!GameComps.StructureTemperatures.IsVersionValid(handle))
    {
      KCrashReporter.Assert(false, "Handle version mismatch in StructureTemperature.SimUnregister");
    }
    else
    {
      if (KMonoBehaviour.isLoadingScene)
        return;
      StructureTemperatureHeader header;
      StructureTemperaturePayload payload;
      GameComps.StructureTemperatures.GetData(handle, out header, out payload);
      if (payload.simHandleCopy == -1)
        return;
      GameScenePartitioner.Instance.TriggerEvent(Grid.PosToCell((KMonoBehaviour) payload.building), GameScenePartitioner.Instance.contactConductiveLayer, (object) new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Destroyed, payload.building, payload.simHandleCopy));
      if (Sim.IsValidHandle(payload.simHandleCopy))
      {
        int handleIndex = Sim.GetHandleIndex(payload.simHandleCopy);
        payload.primaryElement.InternalTemperature = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
        SimMessages.RemoveBuildingHeatExchange(payload.simHandleCopy);
        StructureTemperatureComponents.handleInstanceMap.Remove(payload.simHandleCopy);
      }
      payload.simHandleCopy = -1;
      header.simHandle = -1;
      this.SetData(handle, header, ref payload);
    }
  }
}
