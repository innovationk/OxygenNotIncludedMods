// Decompiled with JetBrains decompiler
// Type: BeeMakeHiveStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BeeMakeHiveStates : 
  GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>
{
  public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State findBuildLocation;
  public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State moveToBuildLocation;
  public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State doBuild;
  public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.findBuildLocation;
    this.root.DoNothing();
    this.findBuildLocation.Enter((StateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State.Callback) (smi =>
    {
      this.FindBuildLocation(smi);
      if (smi.targetBuildCell != Grid.InvalidCell)
        smi.GoTo((StateMachine.BaseState) this.moveToBuildLocation);
      else
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    this.moveToBuildLocation.MoveTo((Func<BeeMakeHiveStates.Instance, int>) (smi => smi.targetBuildCell), this.doBuild, this.behaviourcomplete);
    this.doBuild.PlayAnim("hive_grow_pre").EventHandler(GameHashes.AnimQueueComplete, (StateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State.Callback) (smi =>
    {
      if ((UnityEngine.Object) smi.gameObject.GetComponent<Bee>().FindHiveInRoom() == (UnityEngine.Object) null)
      {
        smi.builtHome = true;
        smi.BuildHome();
      }
      smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToMakeHome).Exit((StateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State.Callback) (smi =>
    {
      if (!smi.builtHome)
        return;
      Util.KDestroyGameObject(smi.master.gameObject);
    }));
  }

  private void FindBuildLocation(BeeMakeHiveStates.Instance smi)
  {
    smi.targetBuildCell = Grid.InvalidCell;
    GameObject prefab = Assets.GetPrefab("BeeHive".ToTag());
    BuildingPlacementQuery query = PathFinderQueries.buildingPlacementQuery.Reset(1, prefab);
    smi.GetComponent<Navigator>().RunQuery((PathFinderQuery) query);
    if (query.result_cells.Count <= 0)
      return;
    smi.targetBuildCell = query.result_cells[UnityEngine.Random.Range(0, query.result_cells.Count)];
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.GameInstance
  {
    public int targetBuildCell;
    public bool builtHome;

    public Instance(Chore<BeeMakeHiveStates.Instance> chore, BeeMakeHiveStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToMakeHome);
    }

    public void BuildHome()
    {
      Vector3 pos = Grid.CellToPos(this.targetBuildCell, CellAlignment.Bottom, Grid.SceneLayer.Creatures);
      GameObject go = Util.KInstantiate(Assets.GetPrefab("BeeHive".ToTag()), pos, Quaternion.identity);
      PrimaryElement component = go.GetComponent<PrimaryElement>();
      component.ElementID = SimHashes.Creature;
      component.Temperature = this.gameObject.GetComponent<PrimaryElement>().Temperature;
      go.SetActive(true);
      go.GetSMI<BeeHive.StatesInstance>().SetUpNewHive();
    }
  }
}
