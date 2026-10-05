// Decompiled with JetBrains decompiler
// Type: BedDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BedDiagnostic : ColonyDiagnostic
{
  private List<MinionIdentity> minionsWithStamina;
  private const bool INCLUDE_CHILD_WORLDS = true;
  private readonly string NO_MINIONS_WITH_STAMINA;

  public BedDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.ALL_NAME)
  {
    this.icon = "icon_action_region_bedroom";
    this.AddCriterion("CheckEnoughBeds", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.CRITERIA.CHECKENOUGHBEDS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughBeds)));
    this.AddCriterion("CheckReachability", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.CRITERIA.CHECKREACHABILITY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckReachability)));
    this.NO_MINIONS_WITH_STAMINA = (string) (this.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NO_MINIONS_PLANETOID);
  }

  private ColonyDiagnostic.DiagnosticResult CheckEnoughBeds()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
    diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NORMAL;
    if (this.minionsWithStamina.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = this.NO_MINIONS_WITH_STAMINA;
    }
    else if (Components.NormalBeds.GlobalCount < this.minionsWithStamina.Count)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
      diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NOT_ENOUGH_BEDS;
    }
    return diagnosticResult;
  }

  private ColonyDiagnostic.DiagnosticResult CheckReachability()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
    diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NORMAL;
    if (this.minionsWithStamina.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = this.NO_MINIONS_WITH_STAMINA;
    }
    else
    {
      ListPool<Sleepable, BedDiagnostic>.PooledList pooledList = ListPool<Sleepable, BedDiagnostic>.Allocate();
      foreach (Sleepable sleepable in Components.NormalBeds.WorldItemsEnumerate(this.worldID, true))
      {
        if ((UnityEngine.Object) sleepable.assignable != (UnityEngine.Object) null && !sleepable.assignable.IsAssigned())
          pooledList.Add(sleepable);
      }
      foreach (MinionIdentity identity in this.minionsWithStamina)
      {
        Navigator component = identity.GetComponent<Navigator>();
        AssignableSlotInstance slot = identity.assignableProxy.Get().GetComponent<Ownables>().GetSlot(Db.Get().AssignableSlots.Bed);
        if (!slot.IsAssigned() && diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
        {
          Sleepable sleepable1 = (Sleepable) null;
          foreach (Sleepable sleepable2 in (List<Sleepable>) pooledList)
          {
            if (component.CanReach(sleepable2.approachable) && sleepable2.assignable.CanAutoAssignTo((IAssignableIdentity) identity))
            {
              sleepable1 = sleepable2;
              break;
            }
          }
          if ((UnityEngine.Object) sleepable1 != (UnityEngine.Object) null)
          {
            pooledList.Remove(sleepable1);
          }
          else
          {
            diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
            diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.MISSING_ASSIGNMENT;
            diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(identity.gameObject.transform.position, identity.gameObject);
          }
        }
        else if (slot.IsAssigned() && !component.CanReach(Grid.PosToCell((KMonoBehaviour) slot.assignable)))
        {
          diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
          diagnosticResult.Message = (string) UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.CANT_REACH;
          diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(identity.gameObject.transform.position, identity.gameObject);
          break;
        }
      }
      pooledList.Recycle();
    }
    return diagnosticResult;
  }

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS_WITH_STAMINA);
    if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result))
      return result;
    this.RefreshData();
    return base.Evaluate();
  }

  private void RefreshData()
  {
    this.minionsWithStamina = Components.LiveMinionIdentities.GetWorldItems(this.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
  }

  private bool MinionFilter(MinionIdentity minion)
  {
    return minion.modifiers.amounts.Has(Db.Get().Amounts.Stamina);
  }

  public override string GetAverageValueString()
  {
    if (this.minionsWithStamina == null)
      this.RefreshData();
    return $"{Components.NormalBeds.CountWorldItems(this.worldID, true).ToString()}/{this.minionsWithStamina.Count.ToString()}";
  }
}
