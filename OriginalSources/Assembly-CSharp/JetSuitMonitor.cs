// Decompiled with JetBrains decompiler
// Type: JetSuitMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class JetSuitMonitor : GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance>
{
  public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State off;
  public GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State flying;
  public StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.Target(this.owner);
    this.off.EventTransition(GameHashes.PathAdvanced, this.flying, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStartFlying));
    this.flying.Enter(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StartFlying)).Exit(new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.State.Callback(JetSuitMonitor.StopFlying)).EventTransition(GameHashes.PathAdvanced, this.off, new StateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(JetSuitMonitor.ShouldStopFlying)).Update(new System.Action<JetSuitMonitor.Instance, float>(JetSuitMonitor.Emit));
  }

  public static bool ShouldStartFlying(JetSuitMonitor.Instance smi)
  {
    return (bool) (UnityEngine.Object) smi.navigator && smi.navigator.CurrentNavType == NavType.Hover;
  }

  public static bool ShouldStopFlying(JetSuitMonitor.Instance smi)
  {
    return !(bool) (UnityEngine.Object) smi.navigator || smi.navigator.CurrentNavType != NavType.Hover;
  }

  public static void StartFlying(JetSuitMonitor.Instance smi)
  {
  }

  public static void StopFlying(JetSuitMonitor.Instance smi)
  {
  }

  public static void Emit(JetSuitMonitor.Instance smi, float dt)
  {
    if (!(bool) (UnityEngine.Object) smi.navigator)
      return;
    GameObject gameObject = smi.sm.owner.Get(smi);
    if (!(bool) (UnityEngine.Object) gameObject)
      return;
    Grid.PosToCell(gameObject.transform.GetPosition());
    float num1 = Mathf.Min(0.2f * dt, smi.jet_suit_tank.amount);
    smi.jet_suit_tank.amount -= num1;
    float num2 = num1 * 0.25f;
    if ((double) num2 > 1.4012984643248171E-45)
    {
      Vector3 position1;
      Vector3 position2 = position1 = gameObject.transform.position;
      Vector3 position3 = position1;
      Vector3 vector3 = Vector3.down;
      if ((UnityEngine.Object) smi.helmetController.jet_anim != (UnityEngine.Object) null)
      {
        KBatchedAnimController jetAnim = smi.helmetController.jet_anim;
        bool symbolVisible;
        Matrix4x4 symbolTransform1 = jetAnim.GetSymbolTransform((HashedString) "left_fire", out symbolVisible);
        Matrix4x4 symbolTransform2 = jetAnim.GetSymbolTransform((HashedString) "right_fire", out symbolVisible);
        Vector3 column1 = (Vector3) symbolTransform1.GetColumn(3);
        Vector3 column2 = (Vector3) symbolTransform2.GetColumn(3);
        float f = Quaternion.LookRotation((Vector3) symbolTransform1.GetColumn(2), (Vector3) symbolTransform1.GetColumn(1)).eulerAngles.z * ((float) Math.PI / 180f);
        vector3 = new Vector3(-Mathf.Sin(f), Mathf.Cos(f));
        position2 = column1 + vector3.normalized * 0.6f;
        position3 = column2 + vector3.normalized * 0.6f;
      }
      float mass = num2 / 2f;
      float num3 = 0.5f;
      int cell = Grid.PosToCell(position1);
      CO2Manager.instance.SpawnExhaust(position2, vector3.normalized * num3, cell, mass, 373.15f);
      CO2Manager.instance.SpawnExhaust(position3, vector3.normalized * num3, cell, mass, 373.15f);
    }
    if ((double) smi.jet_suit_tank.amount != 0.0)
      return;
    smi.navigator.AddTag(GameTags.JetSuitOutOfFuel);
    smi.navigator.SetCurrentNavType(NavType.Floor);
  }

  public new class Instance : 
    GameStateMachine<JetSuitMonitor, JetSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public HelmetController helmetController;
    public Navigator navigator;
    public JetSuitTank jet_suit_tank;

    public Instance(IStateMachineTarget master, GameObject owner)
      : base(master)
    {
      this.sm.owner.Set(owner, this.smi, false);
      this.helmetController = master.GetComponent<HelmetController>();
      this.navigator = owner.GetComponent<Navigator>();
      this.jet_suit_tank = master.GetComponent<JetSuitTank>();
    }
  }
}
