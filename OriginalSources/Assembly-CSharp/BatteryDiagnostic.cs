// Decompiled with JetBrains decompiler
// Type: BatteryDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BatteryDiagnostic : ColonyDiagnostic
{
  public BatteryDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<BatteryTracker>(worldID);
    this.trackerSampleCountSeconds = 4f;
    this.icon = "overlay_power";
    this.AddCriterion("CheckCapacity", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.CRITERIA.CHECKCAPACITY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckCapacity)));
    this.AddCriterion("CheckDead", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.CRITERIA.CHECKDEAD, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckDead)));
  }

  public ColonyDiagnostic.DiagnosticResult CheckCapacity()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    int num1 = 5;
    foreach (ElectricalUtilityNetwork network in (IEnumerable<UtilityNetwork>) Game.Instance.electricalConduitSystem.GetNetworks())
    {
      if (network.allWires != null && network.allWires.Count != 0)
      {
        float num2 = 0.0f;
        int cell = Grid.PosToCell((KMonoBehaviour) network.allWires[0]);
        if ((int) Grid.WorldIdx[cell] == this.worldID)
        {
          ushort circuitId = Game.Instance.circuitManager.GetCircuitID(cell);
          List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitId);
          if (batteriesOnCircuit != null && batteriesOnCircuit.Count != 0)
          {
            foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitId))
            {
              diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
              diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NORMAL;
              num2 += battery.capacity;
            }
            if ((double) num2 < (double) Game.Instance.circuitManager.GetWattsUsedByCircuit(circuitId) * (double) num1)
            {
              diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
              diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.LIMITED_CAPACITY;
              Battery battery = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitId)[0];
              if ((UnityEngine.Object) battery != (UnityEngine.Object) null)
                diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(battery.transform.position, battery.gameObject);
            }
          }
          diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NONE;
        }
      }
    }
    return diagnosticResult;
  }

  public ColonyDiagnostic.DiagnosticResult CheckDead()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    foreach (ElectricalUtilityNetwork network in (IEnumerable<UtilityNetwork>) Game.Instance.electricalConduitSystem.GetNetworks())
    {
      if (network.allWires != null && network.allWires.Count != 0)
      {
        int cell = Grid.PosToCell((KMonoBehaviour) network.allWires[0]);
        if ((int) Grid.WorldIdx[cell] == this.worldID)
        {
          ushort circuitId = Game.Instance.circuitManager.GetCircuitID(cell);
          List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitId);
          if (batteriesOnCircuit != null && batteriesOnCircuit.Count != 0)
          {
            foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitId))
            {
              if (ColonyDiagnosticUtility.PastNewBuildingGracePeriod(battery.transform) && battery.CircuitID != ushort.MaxValue && (double) battery.JoulesAvailable == 0.0)
              {
                diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
                diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.DEAD_BATTERY;
                diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(battery.transform.position, battery.gameObject);
                break;
              }
            }
          }
        }
      }
    }
    return diagnosticResult;
  }

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult result;
    return ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result) ? result : base.Evaluate();
  }
}
