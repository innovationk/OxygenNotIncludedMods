// Decompiled with JetBrains decompiler
// Type: DiseaseEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseEmitter")]
public class DiseaseEmitter : KMonoBehaviour
{
  [Serialize]
  public float emitRate = 1f;
  [Serialize]
  public byte emitRange;
  [Serialize]
  public int emitCount;
  [Serialize]
  public byte[] emitDiseases;
  public int[] simHandles;
  [Serialize]
  protected bool enableEmitter = true;
  private ulong cellChangedHandlerID;
  private static readonly Action<object> OnCellChangedDispatcher = (Action<object>) (obj => Unsafe.As<DiseaseEmitter>(obj).OnCellChanged());

  public float EmitRate => this.emitRate;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.emitDiseases != null)
    {
      this.simHandles = new int[this.emitDiseases.Length];
      for (int index = 0; index < this.simHandles.Length; ++index)
        this.simHandles[index] = -1;
    }
    this.SimRegister();
  }

  protected override void OnCleanUp()
  {
    this.SimUnregister();
    base.OnCleanUp();
  }

  public void SetEnable(bool enable)
  {
    if (this.enableEmitter == enable)
      return;
    this.enableEmitter = enable;
    if (this.enableEmitter)
      this.SimRegister();
    else
      this.SimUnregister();
  }

  private void SimModifyDiseaseEmitter(int emitterIndex, int cell)
  {
    SimMessages.ModifyDiseaseEmitter(this.simHandles[emitterIndex], cell, this.emitRange, this.emitDiseases[emitterIndex], this.emitRate, this.emitCount);
  }

  protected void OnCellChanged()
  {
    DebugUtil.DevAssert(this.simHandles != null, "DiseaseEmitter received cell change notification but has not been Spawned?!");
    if (this.simHandles == null || !this.enableEmitter)
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if (!Grid.IsValidCell(cell))
      return;
    for (int emitterIndex = 0; emitterIndex < this.emitDiseases.Length; ++emitterIndex)
    {
      if (Sim.IsValidHandle(this.simHandles[emitterIndex]))
        this.SimModifyDiseaseEmitter(emitterIndex, cell);
    }
  }

  private void SimRegister()
  {
    DebugUtil.DevAssert(this.simHandles != null, "DiseaseEmitter.SimRegister invoked but has not been Spawned?!");
    if (this.simHandles == null || !this.enableEmitter)
      return;
    if (this.cellChangedHandlerID != 0UL)
      this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, DiseaseEmitter.OnCellChangedDispatcher, (object) this);
    for (int index = 0; index < this.simHandles.Length; ++index)
    {
      if (this.simHandles[index] == -1)
      {
        this.simHandles[index] = -2;
        SimMessages.AddDiseaseEmitter(Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(DiseaseEmitter.OnSimRegisteredCallback), (object) new DiseaseEmitter.EmitterRegistration()
        {
          emitter = this,
          emitterIndex = index
        }, nameof (DiseaseEmitter)).index);
      }
    }
  }

  private void SimUnregister()
  {
    DebugUtil.DevAssert(this.simHandles != null, "DiseaseEmitter.SimUnregister invoked but has not been Spawned?!");
    if (this.simHandles == null)
      return;
    for (int index = 0; index < this.simHandles.Length; ++index)
    {
      if (Sim.IsValidHandle(this.simHandles[index]))
        SimMessages.RemoveDiseaseEmitter(-1, this.simHandles[index]);
      this.simHandles[index] = -1;
    }
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
  }

  private static void OnSimRegisteredCallback(int handle, object data)
  {
    DiseaseEmitter.EmitterRegistration emitterRegistration = (DiseaseEmitter.EmitterRegistration) data;
    emitterRegistration.emitter.OnSimRegistered(handle, emitterRegistration.emitterIndex);
  }

  private void OnSimRegistered(int handle, int emitterIndex)
  {
    if (this.IsNullOrDestroyed())
    {
      SimMessages.RemoveDiseaseEmitter(-1, handle);
    }
    else
    {
      this.simHandles[emitterIndex] = handle;
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      DebugUtil.DevAssert(Grid.IsValidCell(cell), "Failed to initialize DiseaseEmitter because it is on an invalid cell");
      if (!Grid.IsValidCell(cell))
        return;
      this.SimModifyDiseaseEmitter(emitterIndex, cell);
    }
  }

  public void SetDiseases(List<Klei.AI.Disease> diseases)
  {
    this.emitDiseases = new byte[diseases.Count];
    for (int index = 0; index < diseases.Count; ++index)
      this.emitDiseases[index] = Db.Get().Diseases.GetIndex(diseases[index].id);
  }

  private struct EmitterRegistration
  {
    public DiseaseEmitter emitter;
    public int emitterIndex;
  }
}
