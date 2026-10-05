// Decompiled with JetBrains decompiler
// Type: TrappedDuplicantDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TrappedDuplicantDiagnostic : ColonyDiagnostic
{
  public TrappedDuplicantDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.ALL_NAME)
  {
    this.icon = "overlay_power";
    this.AddCriterion("CheckTrapped", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.CRITERIA.CHECKTRAPPED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckTrapped)));
  }

  public ColonyDiagnostic.DiagnosticResult CheckTrapped()
  {
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    bool flag1 = false;
    foreach (MinionIdentity worldItem1 in Components.LiveMinionIdentities.GetWorldItems(this.worldID))
    {
      if (!flag1)
      {
        if (!ClusterManager.Instance.GetWorld(this.worldID).IsModuleInterior && this.CheckMinionBasicallyIdle(worldItem1))
        {
          Navigator component = worldItem1.GetComponent<Navigator>();
          bool flag2 = true;
          foreach (MinionIdentity worldItem2 in Components.LiveMinionIdentities.GetWorldItems(this.worldID))
          {
            if (!((UnityEngine.Object) worldItem1 == (UnityEngine.Object) worldItem2) && !this.CheckMinionBasicallyIdle(worldItem2) && component.CanReach(worldItem2.GetComponent<IApproachable>()))
            {
              flag2 = false;
              break;
            }
          }
          List<Telepad> worldItems1 = Components.Telepads.GetWorldItems(component.GetMyWorld().id);
          if (worldItems1 != null && worldItems1.Count > 0)
            flag2 = flag2 && !component.CanReach(worldItems1[0].GetComponent<IApproachable>());
          List<WarpReceiver> worldItems2 = Components.WarpReceivers.GetWorldItems(component.GetMyWorld().id);
          if (worldItems2 != null && worldItems2.Count > 0)
          {
            foreach (WarpReceiver warpReceiver in worldItems2)
              flag2 = flag2 && !component.CanReach(worldItems2[0].GetComponent<IApproachable>());
          }
          foreach (Sleepable sleepable in Components.NormalBeds.WorldItemsEnumerate(component.GetMyWorldId(), true))
          {
            Assignable assignable = sleepable.assignable;
            if ((UnityEngine.Object) assignable != (UnityEngine.Object) null && assignable.IsAssignedTo((IAssignableIdentity) worldItem1))
              flag2 = flag2 && !component.CanReach(sleepable.approachable);
          }
          if (flag2)
            diagnosticResult.clickThroughTarget = new Tuple<Vector3, GameObject>(worldItem1.transform.position, worldItem1.gameObject);
          flag1 |= flag2;
        }
      }
      else
        break;
    }
    diagnosticResult.opinion = flag1 ? ColonyDiagnostic.DiagnosticResult.Opinion.Bad : ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
    diagnosticResult.Message = (string) (flag1 ? UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.STUCK : UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.NORMAL);
    return diagnosticResult;
  }

  private bool CheckMinionBasicallyIdle(MinionIdentity minion)
  {
    KPrefabID component = minion.GetComponent<KPrefabID>();
    return component.HasTag(GameTags.Idle) || component.HasTag(GameTags.RecoveringBreath) || component.HasTag(GameTags.MakingMess);
  }
}
