// Decompiled with JetBrains decompiler
// Type: StarryEyed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;

#nullable disable
[SkipSaveFileSerialization]
public class StarryEyed : StateMachineComponent<StarryEyed.StatesInstance>
{
  private const string STARRY_EYED_EFFECT_NAME = "StarryEyed";

  protected override void OnSpawn() => this.smi.StartSM();

  public class StatesInstance(StarryEyed master) : 
    GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.GameInstance(master)
  {
    public bool IsInSpace()
    {
      WorldContainer myWorld = this.GetMyWorld();
      if (!(bool) (UnityEngine.Object) myWorld)
        return false;
      int parentWorldId = myWorld.ParentWorldId;
      int id = myWorld.id;
      return (bool) (UnityEngine.Object) myWorld.GetComponent<Clustercraft>() && parentWorldId == id;
    }
  }

  public class States : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed>
  {
    public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State idle;
    public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State inSpace;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.root.Enter((StateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State.Callback) (smi =>
      {
        if (!smi.IsInSpace())
          return;
        smi.GoTo((StateMachine.BaseState) this.inSpace);
      }));
      this.idle.EventTransition(GameHashes.MinionMigration, (Func<StarryEyed.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.inSpace, (StateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.Transition.ConditionCallback) (smi => smi.IsInSpace())).Enter((StateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State.Callback) (smi =>
      {
        Effects component = smi.master.gameObject.GetComponent<Effects>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.HasEffect(nameof (StarryEyed)))
          return;
        component.Remove(nameof (StarryEyed));
      }));
      this.inSpace.EventTransition(GameHashes.MinionMigration, (Func<StarryEyed.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.idle, (StateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.Transition.ConditionCallback) (smi => !smi.IsInSpace())).ToggleEffect(nameof (StarryEyed));
    }
  }
}
