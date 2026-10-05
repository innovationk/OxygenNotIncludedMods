// Decompiled with JetBrains decompiler
// Type: FartChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FartChore : Chore<FartChore.StatesInstance>
{
  private float mass;
  private SimHashes element_id;
  private byte disease_idx;
  private int disease_count;
  private float overpressureThreshold;

  public FartChore(
    IStateMachineTarget target,
    ChoreType chore_type,
    float mass,
    SimHashes element_id,
    byte disease_idx,
    int disease_count,
    float overpressureThreshold)
    : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: PriorityScreen.PriorityClass.compulsory)
  {
    this.smi = new FartChore.StatesInstance(this, target.gameObject);
    this.mass = mass;
    this.element_id = element_id;
    this.disease_idx = disease_idx;
    this.disease_count = disease_count;
    this.overpressureThreshold = overpressureThreshold;
  }

  private bool CheckIsOverpressure(int cell)
  {
    return (double) Grid.Mass[cell] > (double) this.overpressureThreshold;
  }

  public static void CreateEmission(FartChore.StatesInstance smi) => smi.master.DoFart();

  public void DoFart()
  {
    if ((double) this.mass <= 0.0)
      return;
    Element elementByHash = ElementLoader.FindElementByHash(this.element_id);
    float temperature = this.smi.master.GetComponent<PrimaryElement>().Temperature;
    if (elementByHash.IsGas || elementByHash.IsLiquid)
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      if (this.CheckIsOverpressure(cell))
        return;
      SimMessages.AddRemoveSubstance(cell, this.element_id, CellEventLogger.Instance.ElementConsumerSimUpdate, this.mass, temperature, this.disease_idx, this.disease_count);
    }
    else if (elementByHash.IsSolid)
      elementByHash.substance.SpawnResource(this.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f), this.mass, temperature, this.disease_idx, this.disease_count, forceTemperature: true);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, elementByHash.name, this.gameObject.transform);
  }

  public class StatesInstance : 
    GameStateMachine<FartChore.States, FartChore.StatesInstance, FartChore, object>.GameInstance
  {
    public StatesInstance(FartChore master, GameObject farter)
      : base(master)
    {
      this.sm.farter.Set(farter, this.smi, false);
    }
  }

  public class States : GameStateMachine<FartChore.States, FartChore.StatesInstance, FartChore>
  {
    public StateMachine<FartChore.States, FartChore.StatesInstance, FartChore, object>.TargetParameter farter;
    public GameStateMachine<FartChore.States, FartChore.StatesInstance, FartChore, object>.State finish;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.Target(this.farter);
      this.root.PlayAnim("fart").ScheduleGoTo(10f, (StateMachine.BaseState) this.finish).OnAnimQueueComplete(this.finish);
      this.finish.Enter(new StateMachine<FartChore.States, FartChore.StatesInstance, FartChore, object>.State.Callback(FartChore.CreateEmission)).ReturnSuccess();
    }
  }
}
