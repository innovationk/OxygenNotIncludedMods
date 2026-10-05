// Decompiled with JetBrains decompiler
// Type: BionicUpgrade_Skill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class BionicUpgrade_Skill : 
  GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.root;
    this.root.Enter(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.DisableEffect));
  }

  public static void EnableEffect(BionicUpgrade_Skill.Instance smi) => smi.ApplySkill();

  public static void DisableEffect(BionicUpgrade_Skill.Instance smi) => smi.RemoveSkill();

  public class Def : StateMachine.BaseDef
  {
    public string SKILL_ID;
  }

  public new class Instance : 
    GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.GameInstance
  {
    private MinionResume resume;

    public Instance(IStateMachineTarget master, BionicUpgrade_Skill.Def def)
      : base(master, def)
    {
      this.resume = this.GetComponent<MinionResume>();
    }

    public void ApplySkill() => this.resume.GrantSkill(this.def.SKILL_ID);

    public void RemoveSkill() => this.resume.UngrantSkill(this.def.SKILL_ID);
  }
}
