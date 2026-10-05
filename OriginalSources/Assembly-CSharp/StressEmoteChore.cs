// Decompiled with JetBrains decompiler
// Type: StressEmoteChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class StressEmoteChore : Chore<StressEmoteChore.StatesInstance>
{
  private Func<StatusItem> getStatusItem;

  public StressEmoteChore(
    IStateMachineTarget target,
    ChoreType chore_type,
    HashedString emote_kanim,
    HashedString[] emote_anims,
    KAnim.PlayMode play_mode,
    Func<StatusItem> get_status_item)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.AddPrecondition(ChorePreconditions.instance.IsMoving, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsOffLadder, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.NotInTube, (object) null);
    this.AddPrecondition(ChorePreconditions.instance.IsAwake, (object) null);
    this.getStatusItem = get_status_item;
    this.smi = new StressEmoteChore.StatesInstance(this, target.gameObject, emote_kanim, emote_anims, play_mode);
  }

  protected override StatusItem GetStatusItem()
  {
    return this.getStatusItem == null ? base.GetStatusItem() : this.getStatusItem();
  }

  public override string ToString()
  {
    return this.smi.emoteKAnim.IsValid ? $"StressEmoteChore<{this.smi.emoteKAnim.ToString()}>" : $"StressEmoteChore<{this.smi.emoteAnims[0].ToString()}>";
  }

  public class StatesInstance : 
    GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.GameInstance
  {
    public HashedString[] emoteAnims;
    public HashedString emoteKAnim;
    public KAnim.PlayMode mode = KAnim.PlayMode.Once;

    public StatesInstance(
      StressEmoteChore master,
      GameObject emoter,
      HashedString emote_kanim,
      HashedString[] emote_anims,
      KAnim.PlayMode mode)
      : base(master)
    {
      this.emoteKAnim = emote_kanim;
      this.emoteAnims = emote_anims;
      this.mode = mode;
      this.sm.emoter.Set(emoter, this.smi, false);
    }
  }

  public class States : 
    GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore>
  {
    public StateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.TargetParameter emoter;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.Target(this.emoter);
      this.root.ToggleAnims((Func<StressEmoteChore.StatesInstance, HashedString>) (smi => smi.emoteKAnim)).ToggleThought(Db.Get().Thoughts.Unhappy).PlayAnims((Func<StressEmoteChore.StatesInstance, HashedString[]>) (smi => smi.emoteAnims), (Func<StressEmoteChore.StatesInstance, KAnim.PlayMode>) (smi => smi.mode)).OnAnimQueueComplete((GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.State) null);
    }
  }
}
