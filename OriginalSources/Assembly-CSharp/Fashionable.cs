// Decompiled with JetBrains decompiler
// Type: Fashionable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class Fashionable : StateMachineComponent<Fashionable.StatesInstance>
{
  protected override void OnSpawn() => this.smi.StartSM();

  protected bool IsUncomfortable()
  {
    ClothingWearer component = this.GetComponent<ClothingWearer>();
    return (Object) component != (Object) null && component.currentClothing.decorMod <= 0;
  }

  public class StatesInstance(Fashionable master) : 
    GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.GameInstance(master)
  {
  }

  public class States : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable>
  {
    public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State satisfied;
    public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.EventHandler(GameHashes.EquippedItemEquipper, (StateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State.Callback) (smi =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      })).EventHandler(GameHashes.UnequippedItemEquipper, (StateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State.Callback) (smi =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }));
      this.suffering.AddEffect("UnfashionableClothing").ToggleExpression(Db.Get().Expressions.Uncomfortable);
      this.satisfied.DoNothing();
    }
  }
}
