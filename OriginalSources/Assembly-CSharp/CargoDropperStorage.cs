// Decompiled with JetBrains decompiler
// Type: CargoDropperStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CargoDropperStorage : 
  GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.EventHandler(GameHashes.JettisonCargo, (GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>.GameEvent.Callback) ((smi, data) => smi.JettisonCargo(data)));
  }

  public class Def : StateMachine.BaseDef
  {
    public Vector3 dropOffset;
  }

  public class StatesInstance(IStateMachineTarget master, CargoDropperStorage.Def def) : 
    GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>.GameInstance(master, def)
  {
    public void JettisonCargo(object data)
    {
      Vector3 position1 = this.master.transform.GetPosition() + this.def.dropOffset;
      Storage component1 = this.GetComponent<Storage>();
      if (!((Object) component1 != (Object) null))
        return;
      GameObject first = component1.FindFirst((Tag) "ScoutRover");
      if ((Object) first != (Object) null)
      {
        component1.Drop(first, true);
        Vector3 position2 = this.master.transform.GetPosition() with
        {
          z = Grid.GetLayerZ(Grid.SceneLayer.Creatures)
        };
        first.transform.SetPosition(position2);
        ChoreProvider component2 = first.GetComponent<ChoreProvider>();
        if ((Object) component2 != (Object) null)
        {
          KBatchedAnimController component3 = first.GetComponent<KBatchedAnimController>();
          if ((Object) component3 != (Object) null)
            component3.Play((HashedString) "enter");
          EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) component2, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) (string) null, new HashedString[1]
          {
            (HashedString) "enter"
          }, KAnim.PlayMode.Once);
        }
        first.GetMyWorld().SetRoverLanded();
      }
      component1.DropAll(position1);
    }
  }
}
