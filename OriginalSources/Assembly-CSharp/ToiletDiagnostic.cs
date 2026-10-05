// Decompiled with JetBrains decompiler
// Type: ToiletDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public class ToiletDiagnostic : ColonyDiagnostic
{
  private const bool INCLUDE_CHILD_WORLDS = true;
  private List<MinionIdentity> minionsWithBladders;
  private List<IUsable> toilets;
  private readonly string NO_MINIONS_WITH_BLADDER;

  public ToiletDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.ALL_NAME)
  {
    this.icon = "icon_action_region_toilet";
    this.tracker = (Tracker) TrackerTool.Instance.GetWorldTracker<WorkingToiletTracker>(worldID);
    this.NO_MINIONS_WITH_BLADDER = (string) (this.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_MINIONS_PLANETOID);
    this.AddCriterion("CheckHasAnyToilets", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKHASANYTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasAnyToilets)));
    this.AddCriterion("CheckEnoughToilets", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKENOUGHTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughToilets)));
    this.AddCriterion("CheckBladders", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKBLADDERS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckBladders)));
  }

  private ColonyDiagnostic.DiagnosticResult CheckHasAnyToilets()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    if (this.minionsWithBladders.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = this.NO_MINIONS_WITH_BLADDER;
    }
    else if (this.toilets.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_TOILETS;
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckEnoughToilets()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    if (this.minionsWithBladders.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = this.NO_MINIONS_WITH_BLADDER;
    }
    else
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
      if ((double) this.tracker.GetDataTimeLength() > 10.0 && (double) this.tracker.GetAverageValue(this.trackerSampleCountSeconds) <= 0.0)
      {
        diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
        diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_WORKING_TOILETS;
      }
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckBladders()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    if (this.minionsWithBladders.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = this.NO_MINIONS_WITH_BLADDER;
    }
    else
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
      WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
      foreach (StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.GenericInstance genericInstance in Components.CriticalBladders.Items)
      {
        int myWorldId = genericInstance.master.gameObject.GetMyWorldId();
        if (myWorldId == this.worldID || world.GetChildWorldIds().Contains(myWorldId))
        {
          diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
          diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.TOILET_URGENT;
          break;
        }
      }
    }
    return diagnosticResult;
  }

  private bool MinionFilter(MinionIdentity minion)
  {
    return minion.modifiers.amounts.Has(Db.Get().Amounts.Bladder);
  }

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS_WITH_BLADDER);
    if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result))
      return result;
    this.RefreshData();
    return base.Evaluate();
  }

  private void RefreshData()
  {
    this.minionsWithBladders = Components.LiveMinionIdentities.GetWorldItems(this.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
    this.toilets = Components.Toilets.GetWorldItems(this.worldID, true);
  }

  public override string GetAverageValueString()
  {
    if (this.minionsWithBladders == null || this.minionsWithBladders.Count == 0)
      this.RefreshData();
    int count = this.toilets.Count;
    for (int index = 0; index < this.toilets.Count; ++index)
    {
      if (!this.toilets[index].IsNullOrDestroyed() && !this.toilets[index].IsUsable())
        --count;
    }
    return $"{count.ToString()}:{this.minionsWithBladders.Count.ToString()}";
  }
}
