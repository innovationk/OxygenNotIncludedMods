// Decompiled with JetBrains decompiler
// Type: BeOfflineChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class BeOfflineChore : Chore<BeOfflineChore.StatesInstance>
{
  public const string EFFECT_NAME = "BionicOffline";

  public static string GetPowerDownAnimPre(BeOfflineChore.StatesInstance smi)
  {
    switch (smi.gameObject.GetComponent<Navigator>().CurrentNavType)
    {
      case NavType.Ladder:
      case NavType.Pole:
        return "ladder_power_down";
      default:
        return "power_down";
    }
  }

  public static string GetPowerDownAnimLoop(BeOfflineChore.StatesInstance smi)
  {
    switch (smi.gameObject.GetComponent<Navigator>().CurrentNavType)
    {
      case NavType.Ladder:
      case NavType.Pole:
        return "ladder_power_down_idle";
      default:
        return "power_down_idle";
    }
  }

  public BeOfflineChore(IStateMachineTarget master)
    : base(Db.Get().ChoreTypes.BeOffline, master, master.GetComponent<ChoreProvider>(), master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new BeOfflineChore.StatesInstance(this);
    this.AddPrecondition(ChorePreconditions.instance.NotInTube, (object) null);
  }

  public class StatesInstance(BeOfflineChore master) : 
    GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore>
  {
    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.ToggleAnims("anim_bionic_kanim").ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicOfflineIncapacitated, (Func<BeOfflineChore.StatesInstance, object>) (smi => (object) smi.master.gameObject.GetSMI<BionicBatteryMonitor.Instance>())).ToggleEffect("BionicOffline").PlayAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimPre)).QueueAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimLoop), true);
    }
  }
}
