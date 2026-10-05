// Decompiled with JetBrains decompiler
// Type: Loner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[SkipSaveFileSerialization]
public class Loner : StateMachineComponent<Loner.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  public class StatesInstance(Loner master) : 
    GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.GameInstance(master)
  {
    public bool IsAlone()
    {
      WorldContainer myWorld = this.GetMyWorld();
      if (!(bool) (UnityEngine.Object) myWorld)
        return false;
      int parentWorldId = myWorld.ParentWorldId;
      int id = myWorld.id;
      MinionIdentity component = this.GetComponent<MinionIdentity>();
      foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
      {
        if ((UnityEngine.Object) component != (UnityEngine.Object) liveMinionIdentity)
        {
          int myWorldId = liveMinionIdentity.GetMyWorldId();
          if (id == myWorldId || parentWorldId == myWorldId)
            return false;
        }
      }
      return true;
    }
  }

  public class States : GameStateMachine<Loner.States, Loner.StatesInstance, Loner>
  {
    public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State idle;
    public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State alone;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.Enter((StateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State.Callback) (smi =>
      {
        if (!smi.IsAlone())
          return;
        smi.GoTo((StateMachine.BaseState) this.alone);
      }));
      this.idle.EventTransition(GameHashes.MinionMigration, (Func<Loner.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.alone, (StateMachine<Loner.States, Loner.StatesInstance, Loner, object>.Transition.ConditionCallback) (smi => smi.IsAlone())).EventTransition(GameHashes.MinionDelta, (Func<Loner.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.alone, (StateMachine<Loner.States, Loner.StatesInstance, Loner, object>.Transition.ConditionCallback) (smi => smi.IsAlone()));
      this.alone.EventTransition(GameHashes.MinionMigration, (Func<Loner.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.idle, (StateMachine<Loner.States, Loner.StatesInstance, Loner, object>.Transition.ConditionCallback) (smi => !smi.IsAlone())).EventTransition(GameHashes.MinionDelta, (Func<Loner.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.idle, (StateMachine<Loner.States, Loner.StatesInstance, Loner, object>.Transition.ConditionCallback) (smi => !smi.IsAlone())).ToggleEffect(nameof (Loner));
    }
  }
}
