// Decompiled with JetBrains decompiler
// Type: NuclearWaste
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

#nullable disable
public class NuclearWaste : 
  GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>
{
  private const float lifetime = 600f;
  public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State idle;
  public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State decayed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.idle;
    this.idle.PlayAnim((Func<NuclearWaste.Instance, string>) (smi => smi.GetAnimToPlay())).Update((System.Action<NuclearWaste.Instance, float>) ((smi, dt) =>
    {
      smi.timeAlive += dt;
      string animToPlay = smi.GetAnimToPlay();
      if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
        smi.Play(animToPlay);
      if ((double) smi.timeAlive < 600.0)
        return;
      smi.GoTo((StateMachine.BaseState) this.decayed);
    }), UpdateRate.SIM_4000ms).EventHandler(GameHashes.Absorb, (GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.GameEvent.Callback) ((smi, otherObject) =>
    {
      Pickupable cmp = (Pickupable) otherObject;
      float timeAlive = cmp.GetSMI<NuclearWaste.Instance>().timeAlive;
      float mass1 = cmp.PrimaryElement.Mass;
      float mass2 = smi.master.GetComponent<PrimaryElement>().Mass;
      float num = (float) (((double) mass2 - (double) mass1) * (double) smi.timeAlive + (double) mass1 * (double) timeAlive) / mass2;
      smi.timeAlive = num;
      string animToPlay = smi.GetAnimToPlay();
      if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
        smi.Play(animToPlay);
      if ((double) smi.timeAlive < 600.0)
        return;
      smi.GoTo((StateMachine.BaseState) this.decayed);
    }));
    this.decayed.Enter((StateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State.Callback) (smi =>
    {
      smi.GetComponent<Dumpable>().Dump();
      Util.KDestroyGameObject(smi.master.gameObject);
    }));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, NuclearWaste.Def def) : 
    GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.GameInstance(master, def)
  {
    [Serialize]
    public float timeAlive;
    private float percentageRemaining;

    public string GetAnimToPlay()
    {
      this.percentageRemaining = (float) (1.0 - (double) this.smi.timeAlive / 600.0);
      if ((double) this.percentageRemaining <= 0.33000001311302185)
        return "idle1";
      return (double) this.percentageRemaining <= 0.6600000262260437 ? "idle2" : "idle3";
    }
  }
}
