// Decompiled with JetBrains decompiler
// Type: PodLander
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class PodLander : StateMachineComponent<PodLander.StatesInstance>, IGameObjectEffectDescriptor
{
  [Serialize]
  private int landOffLocation;
  [Serialize]
  private float flightAnimOffset;
  private float rocketSpeed;
  public float exhaustEmitRate = 2f;
  public float exhaustTemperature = 1000f;
  public SimHashes exhaustElement = SimHashes.CarbonDioxide;
  private GameObject soundSpeakerObject;
  private bool releasingAstronaut;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public void ReleaseAstronaut()
  {
    if (this.releasingAstronaut)
      return;
    this.releasingAstronaut = true;
    MinionStorage component = this.GetComponent<MinionStorage>();
    List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
    for (int index = storedMinionInfo.Count - 1; index >= 0; --index)
    {
      MinionStorage.Info info = storedMinionInfo[index];
      component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(this.smi.master.transform.GetPosition())));
    }
    this.releasingAstronaut = false;
  }

  public List<Descriptor> GetDescriptors(GameObject go) => (List<Descriptor>) null;

  public class StatesInstance(PodLander master) : 
    GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander>
  {
    public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State landing;
    public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State crashed;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.landing;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.landing.PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter((StateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State.Callback) (smi => smi.master.flightAnimOffset = 50f)).Update((Action<PodLander.StatesInstance, float>) ((smi, dt) =>
      {
        float num = 10f;
        smi.master.rocketSpeed = num - Mathf.Clamp(Mathf.Pow(smi.timeinstate / 3.5f, 4f), 0.0f, num - 2f);
        smi.master.flightAnimOffset -= dt * smi.master.rocketSpeed;
        KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
        component.Offset = Vector3.up * smi.master.flightAnimOffset;
        Vector3 positionIncludingOffset = component.PositionIncludingOffset;
        int cell = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
        if (Grid.IsValidCell(cell))
          SimMessages.EmitMass(cell, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, (byte) 0, 0);
        if ((double) component.Offset.y > 0.0)
          return;
        smi.GoTo((StateMachine.BaseState) this.crashed);
      }), UpdateRate.SIM_33ms);
      this.crashed.PlayAnim("grounded").Enter((StateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State.Callback) (smi =>
      {
        smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
        smi.master.rocketSpeed = 0.0f;
        smi.master.ReleaseAstronaut();
      }));
    }
  }
}
