// Decompiled with JetBrains decompiler
// Type: DecorDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
public class DecorDiagnostic : ColonyDiagnostic
{
  public DecorDiagnostic(int worldID)
    : base(worldID, (string) UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.ALL_NAME)
  {
    this.icon = "icon_category_decor";
    this.AddCriterion("CheckDecor", new DiagnosticCriterion((string) UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.CRITERIA.CHECKDECOR, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckDecor)));
  }

  private ColonyDiagnostic.DiagnosticResult CheckDecor()
  {
    List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(this.worldID);
    ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, (string) UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
    if (worldItems.Count == 0)
    {
      diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
      diagnosticResult.Message = this.NO_MINIONS;
    }
    return diagnosticResult;
  }

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS);
    return ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result) ? result : base.Evaluate();
  }
}
