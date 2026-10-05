// Decompiled with JetBrains decompiler
// Type: EquippableBalloon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using TUNING;
using UnityEngine;

#nullable disable
public class EquippableBalloon : StateMachineComponent<EquippableBalloon.StatesInstance>
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    this.ApplyBalloonOverrideToBalloonFx();
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public void SetBalloonOverride(BalloonOverrideSymbol balloonOverride)
  {
    this.smi.facadeAnim = balloonOverride.animFileID;
    this.smi.symbolID = balloonOverride.animFileSymbolID;
    this.ApplyBalloonOverrideToBalloonFx();
  }

  public void ApplyBalloonOverrideToBalloonFx()
  {
    Equippable component = this.GetComponent<Equippable>();
    if (component.IsNullOrDestroyed() || component.assignee.IsNullOrDestroyed())
      return;
    Ownables soleOwner = component.assignee.GetSoleOwner();
    if (soleOwner.IsNullOrDestroyed())
      return;
    BalloonFX.Instance smi = ((Component) soleOwner.GetComponent<MinionAssignablesProxy>().target).GetSMI<BalloonFX.Instance>();
    if (smi.IsNullOrDestroyed())
      return;
    new BalloonOverrideSymbol(this.smi.facadeAnim, this.smi.symbolID).ApplyTo(smi);
  }

  public class StatesInstance(EquippableBalloon master) : 
    GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.GameInstance(master)
  {
    [Serialize]
    public float transitionTime;
    [Serialize]
    public string facadeAnim;
    [Serialize]
    public string symbolID;
  }

  public class States : 
    GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon>
  {
    public GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.State destroy;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.Transition(this.destroy, (StateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.Transition.ConditionCallback) (smi => (double) GameClock.Instance.GetTime() >= (double) smi.transitionTime));
      this.destroy.Enter((StateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.State.Callback) (smi => smi.master.GetComponent<Equippable>().Unassign()));
    }
  }
}
