// Decompiled with JetBrains decompiler
// Type: IdleStandStillStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
public class IdleStandStillStates : 
  GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>
{
  private GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State loop;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State root = this.root;
    string name = (string) CREATURES.STATUSITEMS.IDLE.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    root.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).ToggleTag(GameTags.Idle);
    this.loop.Enter(new StateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State.Callback(this.PlayIdle));
  }

  public void PlayIdle(IdleStandStillStates.Instance smi)
  {
    KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
    if (smi.def.customIdleAnim != null)
    {
      HashedString invalid = HashedString.Invalid;
      HashedString anim_name = smi.def.customIdleAnim(smi, ref invalid);
      if (anim_name != HashedString.Invalid)
      {
        if (invalid != HashedString.Invalid)
          component.Play(invalid);
        component.Queue(anim_name, KAnim.PlayMode.Loop);
        return;
      }
    }
    component.Play((HashedString) "idle", KAnim.PlayMode.Loop);
  }

  public class Def : StateMachine.BaseDef
  {
    public IdleStandStillStates.Def.IdleAnimCallback customIdleAnim;

    public delegate HashedString IdleAnimCallback(
      IdleStandStillStates.Instance smi,
      ref HashedString pre_anim);
  }

  public new class Instance(
    Chore<IdleStandStillStates.Instance> chore,
    IdleStandStillStates.Def def) : 
    GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.GameInstance((IStateMachineTarget) chore, def)
  {
  }
}
