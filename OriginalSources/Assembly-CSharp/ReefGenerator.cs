// Decompiled with JetBrains decompiler
// Type: ReefGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReefGenerator : 
  GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>
{
  private const string OFF_ANIM = "off";
  private readonly ReefGenerator.InoperationalState inoperational;
  private readonly ReefGenerator.OperationalState operational;
  private readonly StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.TargetParameter geyserTarget;
  private static readonly Operational.Flag reefGeyserEmittingFlag = new Operational.Flag("reefGeyserEmitting", Operational.Flag.Type.Requirement);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.inoperational;
    this.inoperational.Enter(new StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State.Callback(ReefGenerator.DisableWattage)).EnterTransition(this.inoperational.withoutGeyser, (StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.Transition.ConditionCallback) (smi => this.geyserTarget.IsNull(smi))).EnterTransition(this.inoperational.withGeyser, (StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.Transition.ConditionCallback) (smi => !this.geyserTarget.IsNull(smi)));
    this.inoperational.withoutGeyser.PlayAnim("off").Update((System.Action<ReefGenerator.Instance, float>) ((smi, _) => smi.MonitorForLinkableGeyser())).UpdateTransition(this.inoperational.withGeyser, (Func<ReefGenerator.Instance, float, bool>) ((smi, _) => !this.geyserTarget.IsNull(smi)));
    this.inoperational.withGeyser.Enter(new StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State.Callback(ReefGenerator.LinkToGeyser)).Exit(new StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State.Callback(ReefGenerator.UnlinkFromGeyser)).EnterTransition(this.operational.inhale, (StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.Transition.ConditionCallback) (smi => smi.IsOperationalIgnoringGeyser())).ParamTransition<GameObject>((StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.Parameter<GameObject>) this.geyserTarget, this.inoperational.withoutGeyser, GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.IsNull).EventTransition(GameHashes.OperationalFlagChanged, this.operational.inhale, (StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.Transition.ConditionCallback) (smi => smi.IsOperationalIgnoringGeyser()));
    this.operational.Enter(new StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State.Callback(ReefGenerator.LinkToGeyser)).Exit(new StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State.Callback(ReefGenerator.UnlinkFromGeyser)).ParamTransition<GameObject>((StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.Parameter<GameObject>) this.geyserTarget, this.inoperational.withoutGeyser, GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.IsNull).Target(this.masterTarget).EventTransition(GameHashes.OperationalFlagChanged, (GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State) this.inoperational, (StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.Transition.ConditionCallback) (smi => !smi.IsOperationalIgnoringGeyser()));
    this.operational.inhale.Target(this.geyserTarget).TagTransition(GameTags.GeyserExhaling, this.operational.exhale).Target(this.masterTarget).PlayAnim("off").Enter(new StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State.Callback(ReefGenerator.DisableWattage)).ToggleStatusItem(Db.Get().BuildingStatusItems.ReefGeneratorIdle);
    this.operational.exhale.Target(this.geyserTarget).TagTransition(GameTags.GeyserExhaling, this.operational.inhale, true).Target(this.masterTarget).Enter(new StateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State.Callback(ReefGenerator.EnableWattage)).ToggleStatusItem(Db.Get().BuildingStatusItems.Wattage, (Func<ReefGenerator.Instance, object>) (smi => (object) smi.GeneratorPower));
  }

  private static void EnableWattage(ReefGenerator.Instance smi) => smi.SetWattageState(true);

  private static void DisableWattage(ReefGenerator.Instance smi) => smi.SetWattageState(false);

  private static void LinkToGeyser(ReefGenerator.Instance smi) => smi.LinkToGeyser();

  private static void UnlinkFromGeyser(ReefGenerator.Instance smi) => smi.UnlinkFromGeyser();

  public class Def : StateMachine.BaseDef
  {
  }

  private class OperationalState : 
    GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State
  {
    public GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State inhale;
    public GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State exhale;
  }

  private class InoperationalState : 
    GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State
  {
    public GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State withoutGeyser;
    public GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.State withGeyser;
  }

  public new class Instance : 
    GameStateMachine<ReefGenerator, ReefGenerator.Instance, IStateMachineTarget, ReefGenerator.Def>.GameInstance
  {
    private KAnimLink animLink;
    private KAnimControllerBase geyserController;
    private readonly Operational operational;
    private readonly KAnimControllerBase myController;
    private const string INOPERABLE_INSERT = "_inoperable";
    private static readonly string[] PHASE_PREFIXES = new string[2]
    {
      "inhale",
      "exhale"
    };

    public ReefGeneratorPower GeneratorPower { get; private set; }

    public Instance(IStateMachineTarget master, ReefGenerator.Def def)
      : base(master, def)
    {
      this.operational = master.GetComponent<Operational>();
      this.myController = master.GetComponent<KAnimControllerBase>();
      this.GeneratorPower = master.GetComponent<ReefGeneratorPower>();
    }

    public void MonitorForLinkableGeyser()
    {
      if (!this.sm.geyserTarget.IsNull(this.smi))
        return;
      int cell = Grid.PosToCell(this.master.transform.GetPosition());
      GameObject gameObject = Grid.Objects[cell, 1];
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return;
      this.sm.geyserTarget.Set(gameObject, this.smi, false);
    }

    public bool IsOperationalIgnoringGeyser()
    {
      if (this.sm.geyserTarget.IsNull(this.smi))
        return false;
      foreach (KeyValuePair<Operational.Flag, bool> flag in this.operational.Flags)
      {
        if (flag.Key != ReefGenerator.reefGeyserEmittingFlag && !flag.Value)
          return false;
      }
      return true;
    }

    public void SetWattageState(bool active)
    {
      this.operational.SetFlag(ReefGenerator.reefGeyserEmittingFlag, active);
    }

    public void LinkToGeyser()
    {
      GameObject gameObject = this.sm.geyserTarget.Get(this.smi);
      if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        return;
      this.geyserController = gameObject.GetComponent<KAnimControllerBase>();
      if ((UnityEngine.Object) this.geyserController == (UnityEngine.Object) null)
        return;
      this.animLink = new KAnimLink(this.geyserController, this.myController);
      KAnimSynchronizer synchronizer = this.geyserController.GetSynchronizer();
      synchronizer.Add(this.myController, new KAnimSynchronizer.TranslateAnimName(this.TranslateGeyserAnim));
      synchronizer.Sync(this.myController);
      synchronizer.IdleAnim = "off";
    }

    public void UnlinkFromGeyser()
    {
      if (this.animLink != null)
      {
        this.animLink.Unregister();
        this.animLink = (KAnimLink) null;
      }
      if ((UnityEngine.Object) this.geyserController != (UnityEngine.Object) null)
        this.geyserController.GetSynchronizer().Remove(this.myController);
      this.geyserController = (KAnimControllerBase) null;
    }

    private string TranslateGeyserAnim(string masterAnimName)
    {
      if (this.IsOperationalIgnoringGeyser())
        return masterAnimName;
      foreach (string str1 in ReefGenerator.Instance.PHASE_PREFIXES)
      {
        if (masterAnimName.StartsWith(str1))
        {
          string str2 = str1;
          string str3 = masterAnimName;
          int length = str1.Length;
          string str4 = str3.Substring(length, str3.Length - length);
          return $"{str2}_inoperable{str4}";
        }
      }
      return masterAnimName;
    }
  }
}
