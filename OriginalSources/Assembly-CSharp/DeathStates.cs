// Decompiled with JetBrains decompiler
// Type: DeathStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class DeathStates : 
  GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>
{
  private GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State loop;
  public GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State pst;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State loop = this.loop;
    string name = (string) CREATURES.STATUSITEMS.DEAD.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    loop.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).Enter("EnableGravity", (StateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State.Callback) (smi => smi.EnableGravityIfNecessary())).Enter("Play Death Animations", (StateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State.Callback) (smi => smi.PlayDeathAnimations())).OnAnimQueueComplete(this.pst).ScheduleGoTo((Func<DeathStates.Instance, float>) (smi => smi.def.DIE_ANIMATION_EXPIRATION_TIME), (StateMachine.BaseState) this.pst);
    this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete).TriggerOnEnter(GameHashes.Died).Enter("Butcher", (StateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State.Callback) (smi =>
    {
      if (!((UnityEngine.Object) smi.gameObject.GetComponent<Butcherable>() != (UnityEngine.Object) null))
        return;
      smi.GetComponent<Butcherable>().OnButcherComplete();
    })).Enter("Destroy", (StateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State.Callback) (smi =>
    {
      smi.gameObject.AddTag(GameTags.Dead);
      smi.gameObject.DeleteObject();
    })).BehaviourComplete(GameTags.Creatures.Die);
  }

  public class Def : StateMachine.BaseDef
  {
    public float DIE_ANIMATION_EXPIRATION_TIME = 4f;
  }

  public new class Instance : 
    GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.GameInstance
  {
    public Instance(Chore<DeathStates.Instance> chore, DeathStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Die);
    }

    public void EnableGravityIfNecessary()
    {
      if (!this.HasTag(GameTags.Creatures.Flyer) || this.HasTag(GameTags.Stored))
        return;
      GameComps.Gravities.Add(this.smi.gameObject, Vector2.zero, (System.Action<Transform>) (transform => this.smi.DisableGravity()));
    }

    public void DisableGravity()
    {
      if (!GameComps.Gravities.Has((object) this.smi.gameObject))
        return;
      GameComps.Gravities.Remove(this.smi.gameObject);
    }

    public void PlayDeathAnimations()
    {
      if (this.gameObject.HasTag(GameTags.PreventDeadAnimation))
        return;
      KAnimControllerBase component = this.gameObject.GetComponent<KAnimControllerBase>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.Play((HashedString) "Death");
    }
  }
}
