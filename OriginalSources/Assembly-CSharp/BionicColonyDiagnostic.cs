// Decompiled with JetBrains decompiler
// Type: BionicColonyDiagnostic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public abstract class BionicColonyDiagnostic : ColonyDiagnostic
{
  protected const bool INCLUDE_CHILD_WORLDS = true;
  protected List<MinionIdentity> bionics;
  protected bool ignoreInIdleRockets = true;

  public override string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public BionicColonyDiagnostic(int worldID, string name)
    : base(worldID, name)
  {
    this.RefreshData();
  }

  protected void RefreshData()
  {
    Components.Cmps<MinionIdentity> cmps;
    if (Components.LiveMinionIdentitiesByModel.TryGetValue(BionicMinionConfig.MODEL, out cmps))
      this.bionics = cmps.GetWorldItems(this.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
    else
      this.bionics = new List<MinionIdentity>();
  }

  protected virtual bool MinionFilter(MinionIdentity minion) => true;

  public override ColonyDiagnostic.DiagnosticResult Evaluate()
  {
    ColonyDiagnostic.DiagnosticResult result;
    if (this.ignoreInIdleRockets && ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(this.worldID, out result))
      return result;
    this.RefreshData();
    result = base.Evaluate();
    if (result.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
      result.Message = this.GetDefaultResultMessage();
    return result;
  }

  protected abstract string GetDefaultResultMessage();
}
