// Decompiled with JetBrains decompiler
// Type: FloatingRocketDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

#nullable disable
public class FloatingRocketDiagnostic : ColonyDiagnostic
{
  public FloatingRocketDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.ALL_NAME)
  {
    this.icon = "icon_errand_rocketry";
  }

  public override string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
    Clustercraft component = world.gameObject.GetComponent<Clustercraft>();
    ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS);
    if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result))
      return result;
    result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
    if (world.ParentWorldId == (int) byte.MaxValue || world.ParentWorldId == world.id)
    {
      result.Message = (string) UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_FLIGHT;
      if (component.Destination == component.Location)
      {
        bool flag = false;
        foreach (Ref<RocketModuleCluster> clusterModule in (IEnumerable<Ref<RocketModuleCluster>>) component.ModuleInterface.ClusterModules)
        {
          ResourceHarvestModule.StatesInstance smi = clusterModule.Get().GetSMI<ResourceHarvestModule.StatesInstance>();
          if (smi != null && smi.IsInsideState((StateMachine.BaseState) smi.sm.not_grounded.drilling))
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
          result.Message = (string) UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_UTILITY;
        }
        else
        {
          result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion;
          result.Message = (string) UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.WARNING_NO_DESTINATION;
        }
      }
      else if ((double) component.Speed == 0.0)
      {
        result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
        result.Message = (string) UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.WARNING_NO_SPEED;
      }
    }
    else
      result.Message = (string) UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_LANDED;
    return result;
  }
}
