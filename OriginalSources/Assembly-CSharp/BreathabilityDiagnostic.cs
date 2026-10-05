// Decompiled with JetBrains decompiler
// Type: BreathabilityDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BreathabilityDiagnostic : ColonyDiagnostic
{
  public BreathabilityDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.ALL_NAME)
  {
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<BreathabilityTracker>(worldID);
    this.trackerSampleCountSeconds = 50f;
    this.icon = "overlay_oxygen";
    this.AddCriterion("CheckSuffocation", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKSUFFOCATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckSuffocation)));
    this.AddCriterion("CheckLowBreathability", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKLOWBREATHABILITY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLowBreathability)));
    this.AddCriterion("CheckBionicOxygen", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKLOWBIONICOXYGEN, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLowBionicOxygen)));
  }

  private ColonyDiagnostic.DiagnosticResult CheckSuffocation()
  {
    List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(this.worldID);
    if (worldItems.Count == 0)
      return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS);
    foreach (Component cmp in worldItems)
    {
      SuffocationMonitor.Instance smi = cmp.GetSMI<SuffocationMonitor.Instance>();
      if (smi != null && smi.IsInsideState((StateMachine.BaseState) smi.sm.noOxygen.suffocating))
        return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, (string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.SUFFOCATING, new Tuple<Vector3, GameObject>(smi.transform.position, smi.gameObject));
    }
    return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL);
  }

  private ColonyDiagnostic.DiagnosticResult CheckLowBreathability()
  {
    return Components.LiveMinionIdentities.GetWorldItems(this.worldID).Count != 0 && (double) this.tracker.GetAverageValue(this.trackerSampleCountSeconds) < 60.0 ? new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, (string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.POOR) : new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL);
  }

  private ColonyDiagnostic.DiagnosticResult CheckLowBionicOxygen()
  {
    List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(this.worldID);
    if (worldItems.Count != 0)
    {
      foreach (MinionIdentity cmp in worldItems)
      {
        if (cmp.HasTag(GameTags.Minions.Models.Bionic))
        {
          BionicOxygenTankMonitor.Instance smi = cmp.GetSMI<BionicOxygenTankMonitor.Instance>();
          if ((double) smi.OxygenPercentage <= 0.0)
            return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, (string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NEAR_OR_EMPTY_BIONIC_TANKS, new Tuple<Vector3, GameObject>(cmp.transform.position, cmp.gameObject));
          if ((double) smi.OxygenPercentage < 0.5)
            return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, (string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.POOR_BIONIC_TANKS, new Tuple<Vector3, GameObject>(cmp.transform.position, cmp.gameObject));
        }
      }
    }
    return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL);
  }

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult result;
    return ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result) ? result : base.Evaluate();
  }
}
