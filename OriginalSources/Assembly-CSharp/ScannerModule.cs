// Decompiled with JetBrains decompiler
// Type: ScannerModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class ScannerModule : 
  GameStateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Enter((StateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.State.Callback) (smi => smi.SetFogOfWarAllowed())).EventHandler(GameHashes.RocketLaunched, (StateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.State.Callback) (smi => smi.Scan())).EventHandler(GameHashes.ClusterLocationChanged, (Func<ScannerModule.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) smi.GetComponent<RocketModuleCluster>().CraftInterface), (StateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.State.Callback) (smi => smi.Scan())).EventHandler(GameHashes.RocketModuleChanged, (Func<ScannerModule.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) smi.GetComponent<RocketModuleCluster>().CraftInterface), (StateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.State.Callback) (smi => smi.SetFogOfWarAllowed())).Exit((StateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.State.Callback) (smi => smi.SetFogOfWarAllowed()));
  }

  public class Def : StateMachine.BaseDef
  {
    public int scanRadius = 1;
  }

  public new class Instance(IStateMachineTarget master, ScannerModule.Def def) : 
    GameStateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.GameInstance(master, def)
  {
    public void Scan()
    {
      Clustercraft component = this.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
      if (component.Status != Clustercraft.CraftStatus.InFlight)
        return;
      ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
      AxialI location = component.Location;
      smi.RevealLocation(location, this.def.scanRadius);
      foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetNotVisibleEntitiesAtAdjacentCell(location))
        smi.RevealLocation(clusterGridEntity.Location);
    }

    public void SetFogOfWarAllowed()
    {
      CraftModuleInterface craftInterface = this.GetComponent<RocketModuleCluster>().CraftInterface;
      if (!craftInterface.HasClusterDestinationSelector())
        return;
      bool flag = false;
      ClusterDestinationSelector destinationSelector = (ClusterDestinationSelector) craftInterface.GetClusterDestinationSelector();
      bool navigateFogOfWar = destinationSelector.canNavigateFogOfWar;
      foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) craftInterface.ClusterModules)
      {
        RocketModuleCluster cmp = clusterModule.Get();
        if ((cmp != null ? cmp.GetSMI<ScannerModule.Instance>() : (ScannerModule.Instance) null) != null)
        {
          flag = true;
          break;
        }
      }
      destinationSelector.canNavigateFogOfWar = flag;
      if (navigateFogOfWar && !flag)
        craftInterface.GetComponent<ClusterTraveler>()?.RevalidatePath();
      craftInterface.GetComponent<Clustercraft>().Trigger(-688990705, (object) null);
    }
  }
}
