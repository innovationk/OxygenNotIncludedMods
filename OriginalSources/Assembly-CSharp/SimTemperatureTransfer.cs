// Decompiled with JetBrains decompiler
// Type: SimTemperatureTransfer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimTemperatureTransfer")]
public class SimTemperatureTransfer : KMonoBehaviour
{
  [MyCmpReq]
  public PrimaryElement pe;
  private const float SIM_FREEZE_SPAWN_ORE_PERCENT = 0.8f;
  public float deltaKJ;
  public Action<SimTemperatureTransfer> onSimRegistered;
  protected int simHandle = -1;
  protected bool forceDataSyncOnRegister;
  [SerializeField]
  protected float surfaceArea = 10f;
  [SerializeField]
  protected float thickness = 0.01f;
  [SerializeField]
  protected float groundTransferScale = 1f / 16f;
  private static Dictionary<int, SimTemperatureTransfer> handleInstanceMap = new Dictionary<int, SimTemperatureTransfer>();
  private ulong cellChangedHandlerID;
  private static readonly Action<object> OnCellChangedDispatcher = (Action<object>) (obj => Unsafe.As<SimTemperatureTransfer>(obj).OnCellChanged());

  public float SurfaceArea
  {
    get => this.surfaceArea;
    set => this.surfaceArea = value;
  }

  public float Thickness
  {
    get => this.thickness;
    set => this.thickness = value;
  }

  public float GroundTransferScale
  {
    get => this.groundTransferScale;
    set => this.groundTransferScale = value;
  }

  public int SimHandle => this.simHandle;

  public static void ClearInstanceMap() => SimTemperatureTransfer.handleInstanceMap.Clear();

  public static void DoOreMeltTransition(int sim_handle)
  {
    SimTemperatureTransfer cmp = (SimTemperatureTransfer) null;
    if (!SimTemperatureTransfer.handleInstanceMap.TryGetValue(sim_handle, out cmp) || (UnityEngine.Object) cmp == (UnityEngine.Object) null || cmp.HasTag(GameTags.Sealed))
      return;
    PrimaryElement pe = cmp.pe;
    Element element = pe.Element;
    bool flag1 = (double) pe.Temperature >= (double) element.highTemp;
    bool flag2 = (double) pe.Temperature <= (double) element.lowTemp;
    if (!(flag1 | flag2) || flag1 && element.highTempTransitionTarget == SimHashes.Unobtanium || flag2 && element.lowTempTransitionTarget == SimHashes.Unobtanium)
      return;
    if ((double) pe.Mass > 0.0)
    {
      int cell = Grid.PosToCell(cmp.transform.GetPosition());
      float mass1 = pe.Mass;
      int diseaseCount = pe.DiseaseCount;
      SimHashes new_element = flag1 ? element.highTempTransitionTarget : element.lowTempTransitionTarget;
      SimHashes hash = flag1 ? element.highTempTransitionOreID : element.lowTempTransitionOreID;
      float num = flag1 ? element.highTempTransitionOreMassConversion : element.lowTempTransitionOreMassConversion;
      if (hash != (SimHashes) 0)
      {
        float mass2 = mass1 * num;
        int disease_count = (int) ((double) diseaseCount * (double) num);
        if ((double) mass2 > 1.0 / 1000.0)
        {
          mass1 -= mass2;
          diseaseCount -= disease_count;
          Element elementByHash = ElementLoader.FindElementByHash(hash);
          if (elementByHash.IsSolid)
          {
            GameObject gameObject = elementByHash.substance.SpawnResource(cmp.transform.GetPosition(), mass2, pe.Temperature, pe.DiseaseIdx, disease_count, true, manual_activation: true);
            elementByHash.substance.ActivateSubstanceGameObject(gameObject, pe.DiseaseIdx, disease_count);
          }
          else
            SimMessages.AddRemoveSubstance(cell, elementByHash.id, CellEventLogger.Instance.OreMelted, mass2, pe.Temperature, pe.DiseaseIdx, disease_count);
        }
      }
      SimMessages.AddRemoveSubstance(cell, new_element, CellEventLogger.Instance.OreMelted, mass1, pe.Temperature, pe.DiseaseIdx, diseaseCount);
    }
    cmp.OnCleanUp();
    Util.KDestroyGameObject(cmp.gameObject);
  }

  protected override void OnPrefabInit()
  {
    this.pe.sttOptimizationHook = this;
    this.pe.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(SimTemperatureTransfer.OnGetTemperature);
    this.pe.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(SimTemperatureTransfer.OnSetTemperature);
    this.pe.onDataChanged += new Action<PrimaryElement>(this.OnDataChanged);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Element element = this.pe.Element;
    this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, SimTemperatureTransfer.OnCellChangedDispatcher, (object) this, "SimTemperatureTransfer.OnSpawn");
    if (!Grid.IsValidCell(Grid.PosToCell((KMonoBehaviour) this)) || this.pe.Element.HasTag(GameTags.Special) || (double) element.specificHeatCapacity == 0.0)
      this.enabled = false;
    this.SimRegister();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.SimRegister();
    if (!Sim.IsValidHandle(this.simHandle))
      return;
    SimTemperatureTransfer.OnSetTemperature(this.pe, this.pe.Temperature);
  }

  protected override void OnCmpDisable()
  {
    if (Sim.IsValidHandle(this.simHandle))
    {
      float temperature = this.pe.Temperature;
      this.pe.InternalTemperature = this.pe.Temperature;
      SimMessages.SetElementChunkData(this.simHandle, temperature, 0.0f);
    }
    base.OnCmpDisable();
  }

  private void OnCellChanged()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (!Grid.IsValidCell(cell))
    {
      this.enabled = false;
    }
    else
    {
      this.SimRegister();
      if (Sim.IsValidHandle(this.simHandle))
        SimMessages.MoveElementChunk(this.simHandle, cell);
      else
        this.forceDataSyncOnRegister = true;
    }
  }

  protected override void OnCleanUp()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
    this.SimUnregister();
    this.OnForcedCleanUp();
  }

  private static unsafe float OnGetTemperature(PrimaryElement primary_element)
  {
    SimTemperatureTransfer optimizationHook = primary_element.sttOptimizationHook;
    float temperature;
    if (Sim.IsValidHandle(optimizationHook.simHandle))
    {
      int handleIndex = Sim.GetHandleIndex(optimizationHook.simHandle);
      temperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
      optimizationHook.deltaKJ = Game.Instance.simData.elementChunks[handleIndex].deltaKJ;
    }
    else
      temperature = primary_element.InternalTemperature;
    return temperature;
  }

  private static unsafe void OnSetTemperature(PrimaryElement primary_element, float temperature)
  {
    if ((double) temperature <= 0.0)
    {
      KCrashReporter.Assert(false, "STT.OnSetTemperature - Tried to set <= 0 degree temperature");
      temperature = 293f;
    }
    primary_element.InternalTemperature = temperature;
    SimTemperatureTransfer optimizationHook = primary_element.sttOptimizationHook;
    if (!Sim.IsValidHandle(optimizationHook.simHandle))
      return;
    float heat_capacity = primary_element.Mass * primary_element.Element.specificHeatCapacity;
    SimMessages.SetElementChunkData(optimizationHook.simHandle, temperature, heat_capacity);
    Game.Instance.simData.elementChunks[Sim.GetHandleIndex(optimizationHook.simHandle)].temperature = temperature;
  }

  private void OnDataChanged(PrimaryElement primary_element)
  {
    if (Sim.IsValidHandle(this.simHandle))
    {
      float heat_capacity = primary_element.Mass * primary_element.Element.specificHeatCapacity;
      SimMessages.SetElementChunkData(this.simHandle, primary_element.Temperature, heat_capacity);
    }
    else
      this.forceDataSyncOnRegister = true;
  }

  protected void SimRegister()
  {
    if (!this.isSpawned || this.simHandle != -1 || !this.enabled || (double) this.pe.Mass <= 0.0 || this.pe.Element.IsTemperatureInsulated)
      return;
    int cell = Grid.PosToCell(this.transform.GetPosition());
    this.simHandle = -2;
    HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle = Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(SimTemperatureTransfer.OnSimRegisteredCallback), (object) this, "SimTemperatureTransfer.SimRegister");
    float num = this.pe.InternalTemperature;
    if ((double) num <= 0.0)
    {
      this.pe.InternalTemperature = 293f;
      num = 293f;
    }
    this.forceDataSyncOnRegister = false;
    int elementId = (int) this.pe.ElementID;
    double mass = (double) this.pe.Mass;
    double temperature = (double) num;
    double surfaceArea = (double) this.surfaceArea;
    double thickness = (double) this.thickness;
    double groundTransferScale = (double) this.groundTransferScale;
    int index = handle.index;
    SimMessages.AddElementChunk(cell, (SimHashes) elementId, (float) mass, (float) temperature, (float) surfaceArea, (float) thickness, (float) groundTransferScale, index);
  }

  protected unsafe void SimUnregister()
  {
    if (this.simHandle == -1 || KMonoBehaviour.isLoadingScene)
      return;
    if (Sim.IsValidHandle(this.simHandle))
    {
      this.pe.InternalTemperature = Game.Instance.simData.elementChunks[Sim.GetHandleIndex(this.simHandle)].temperature;
      SimMessages.RemoveElementChunk(this.simHandle, -1);
      SimTemperatureTransfer.handleInstanceMap.Remove(this.simHandle);
    }
    this.simHandle = -1;
  }

  private static void OnSimRegisteredCallback(int handle, object data)
  {
    ((SimTemperatureTransfer) data).OnSimRegistered(handle);
  }

  private unsafe void OnSimRegistered(int handle)
  {
    if ((UnityEngine.Object) this != (UnityEngine.Object) null && this.simHandle == -2)
    {
      this.simHandle = handle;
      int handleIndex = Sim.GetHandleIndex(handle);
      float temperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
      float internalTemperature = this.pe.InternalTemperature;
      if ((double) temperature <= 0.0)
        KCrashReporter.Assert(false, "Bad temperature");
      SimTemperatureTransfer.handleInstanceMap[this.simHandle] = this;
      if (this.forceDataSyncOnRegister || (double) Mathf.Abs(temperature - internalTemperature) > 0.10000000149011612)
      {
        float heat_capacity = this.pe.Mass * this.pe.Element.specificHeatCapacity;
        SimMessages.SetElementChunkData(this.simHandle, internalTemperature, heat_capacity);
        SimMessages.MoveElementChunk(this.simHandle, Grid.PosToCell((KMonoBehaviour) this));
        Game.Instance.simData.elementChunks[handleIndex].temperature = internalTemperature;
      }
      if (this.onSimRegistered != null)
        this.onSimRegistered(this);
      if (this.enabled)
        return;
      this.OnCmpDisable();
    }
    else
      SimMessages.RemoveElementChunk(handle, -1);
  }
}
