// Decompiled with JetBrains decompiler
// Type: ArtifactHarvestModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class ArtifactHarvestModule : 
  GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>
{
  public StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.BoolParameter canHarvest;
  public StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.TargetParameter entityTarget;
  public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State grounded;
  public ArtifactHarvestModule.NotGroundedStates not_grounded;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.grounded;
    this.root.Enter((StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State.Callback) (smi => smi.CheckIfCanHarvest()));
    this.grounded.TagTransition(GameTags.RocketNotOnGround, (GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State) this.not_grounded);
    this.not_grounded.DefaultState(this.not_grounded.not_harvesting).EventHandler(GameHashes.ClusterLocationChanged, (Func<ArtifactHarvestModule.StatesInstance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), new GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.GameEvent.Callback(ArtifactHarvestModule.OnAnythingChangingLocationsInSpace)).EventHandler(GameHashes.OnStorageChange, (StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State.Callback) (smi => smi.CheckIfCanHarvest())).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
    this.not_grounded.not_harvesting.PlayAnim("loaded").ParamTransition<bool>((StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.Parameter<bool>) this.canHarvest, this.not_grounded.harvesting, GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.IsTrue);
    this.not_grounded.harvesting.PlayAnim("deploying").Update((System.Action<ArtifactHarvestModule.StatesInstance, float>) ((smi, dt) => smi.HarvestFromHexCell(dt)), UpdateRate.SIM_4000ms).ParamTransition<bool>((StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.Parameter<bool>) this.canHarvest, this.not_grounded.not_harvesting, GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.IsFalse);
  }

  private static void OnAnythingChangingLocationsInSpace(
    ArtifactHarvestModule.StatesInstance smi,
    object obj)
  {
    if (obj == null || !((UnityEngine.Object) ((ClusterLocationChangedEvent) obj).entity == (UnityEngine.Object) smi.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>()))
      return;
    smi.CheckIfCanHarvest();
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class NotGroundedStates : 
    GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State
  {
    public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State not_harvesting;
    public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State harvesting;
  }

  public class StatesInstance(IStateMachineTarget master, ArtifactHarvestModule.Def def) : 
    GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.GameInstance(master, def)
  {
    [MyCmpReq]
    private Storage storage;
    [MyCmpReq]
    private SingleEntityReceptacle receptacle;

    public void HarvestFromHexCell(float dt)
    {
      Clustercraft component = this.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
      StarmapHexCellInventory hexCellInventory = ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location);
      StarmapHexCellInventory.SerializedItem serializedItem = hexCellInventory.Items.Find((Predicate<StarmapHexCellInventory.SerializedItem>) (item => item.IsEntity && Assets.GetPrefab(item.ID).HasTag(GameTags.Artifact)));
      if (serializedItem == null)
        return;
      PrimaryElement andSpawnItem = hexCellInventory.ExtractAndSpawnItem(serializedItem.ID);
      this.receptacle.ForceDeposit(andSpawnItem.gameObject);
      this.storage.Store(andSpawnItem.gameObject);
    }

    public bool CheckIfCanHarvest()
    {
      Clustercraft component = this.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return false;
      if ((UnityEngine.Object) this.receptacle.Occupant != (UnityEngine.Object) null)
      {
        this.sm.canHarvest.Set(false, this);
        return false;
      }
      ClusterGridEntity atCurrentLocation = component.GetPOIAtCurrentLocation();
      if (ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location).Items.Find((Predicate<StarmapHexCellInventory.SerializedItem>) (item => item.IsEntity && Assets.GetPrefab(item.ID).HasTag(GameTags.Artifact))) != null)
      {
        this.sm.canHarvest.Set(true, this);
        return true;
      }
      if ((UnityEngine.Object) atCurrentLocation != (UnityEngine.Object) null && ((bool) (UnityEngine.Object) atCurrentLocation.GetComponent<ArtifactPOIClusterGridEntity>() || (bool) (UnityEngine.Object) atCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>()))
      {
        ArtifactPOIStates.Instance smi = atCurrentLocation.GetSMI<ArtifactPOIStates.Instance>();
        if (smi != null && smi.HasArtifactAvailableInHexCell())
        {
          this.sm.canHarvest.Set(true, this);
          return true;
        }
      }
      this.sm.canHarvest.Set(false, this);
      return false;
    }
  }
}
