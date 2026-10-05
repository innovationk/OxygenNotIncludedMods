// Decompiled with JetBrains decompiler
// Type: RadiationEmitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RadiationEmitter : SimComponent
{
  public bool radiusProportionalToRads;
  [SerializeField]
  public short emitRadiusX = 4;
  [SerializeField]
  public short emitRadiusY = 4;
  [SerializeField]
  public float emitRads = 10f;
  [SerializeField]
  public float emitRate = 1f;
  [SerializeField]
  public float emitSpeed = 1f;
  [SerializeField]
  public float emitDirection;
  [SerializeField]
  public float emitAngle = 360f;
  [SerializeField]
  public RadiationEmitter.RadiationEmitterType emitType;
  [SerializeField]
  public Vector3 emissionOffset = Vector3.zero;
  private ulong cellChangedHandlerID;
  private static readonly Action<object> RefreshDispatcher = (Action<object>) (obj => Unsafe.As<RadiationEmitter>(obj).Refresh());

  protected override void OnSpawn()
  {
    this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, RadiationEmitter.RefreshDispatcher, (object) this, "RadiationEmitter.OnSpawn");
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
    base.OnCleanUp();
  }

  public void SetEmitting(bool emitting) => this.SetSimActive(emitting);

  public int GetEmissionCell()
  {
    return Grid.PosToCell(this.transform.GetPosition() + this.emissionOffset);
  }

  public void Refresh()
  {
    int emissionCell = this.GetEmissionCell();
    if (this.radiusProportionalToRads)
      this.SetRadiusProportionalToRads();
    SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
  }

  private void SetRadiusProportionalToRads()
  {
    this.emitRadiusX = (short) Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128 /*0x80*/);
    this.emitRadiusY = (short) Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128 /*0x80*/);
  }

  protected override void OnSimActivate()
  {
    int emissionCell = this.GetEmissionCell();
    if (this.radiusProportionalToRads)
      this.SetRadiusProportionalToRads();
    SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
  }

  protected override void OnSimDeactivate()
  {
    SimMessages.ModifyRadiationEmitter(this.simHandle, this.GetEmissionCell(), (short) 0, (short) 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, this.emitType);
  }

  protected override void OnSimRegister(
    HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
  {
    Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
    int emissionCell = this.GetEmissionCell();
    SimMessages.AddRadiationEmitter(cb_handle.index, emissionCell, (short) 0, (short) 0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, this.emitType);
  }

  protected override void OnSimUnregister() => RadiationEmitter.StaticUnregister(this.simHandle);

  private static void StaticUnregister(int sim_handle)
  {
    Debug.Assert(Sim.IsValidHandle(sim_handle));
    SimMessages.RemoveRadiationEmitter(-1, sim_handle);
  }

  private void OnDrawGizmosSelected()
  {
    int emissionCell = this.GetEmissionCell();
    Gizmos.color = Color.green;
    Gizmos.DrawSphere(Grid.CellToPos(emissionCell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
  }

  protected override Action<int> GetStaticUnregister()
  {
    return new Action<int>(RadiationEmitter.StaticUnregister);
  }

  public enum RadiationEmitterType
  {
    Constant,
    Pulsing,
    PulsingAveraged,
    SimplePulse,
    RadialBeams,
    Attractor,
  }
}
