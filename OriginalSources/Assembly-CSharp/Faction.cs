// Decompiled with JetBrains decompiler
// Type: Faction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
public class Faction
{
  public HashSet<FactionAlignment> Members = new HashSet<FactionAlignment>();
  public FactionManager.FactionID ID;
  public Dictionary<FactionManager.FactionID, FactionManager.Disposition> Dispositions = new Dictionary<FactionManager.FactionID, FactionManager.Disposition>((IEqualityComparer<FactionManager.FactionID>) new Faction.FactionIDComparer());

  public HashSet<FactionAlignment> HostileTo()
  {
    HashSet<FactionAlignment> factionAlignmentSet = new HashSet<FactionAlignment>();
    foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> disposition in this.Dispositions)
    {
      if (disposition.Value == FactionManager.Disposition.Attack)
        factionAlignmentSet.UnionWith((IEnumerable<FactionAlignment>) FactionManager.Instance.GetFaction(disposition.Key).Members);
    }
    return factionAlignmentSet;
  }

  public Faction(FactionManager.FactionID faction)
  {
    this.ID = faction;
    this.ConfigureAlignments(faction);
  }

  public bool CanAttack { get; private set; }

  public bool CanAssist { get; private set; }

  private void ConfigureAlignments(FactionManager.FactionID faction)
  {
    switch (faction)
    {
      case FactionManager.FactionID.Duplicant:
        this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
        this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
        this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
        break;
      case FactionManager.FactionID.Friendly:
        this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
        this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
        this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
        this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
        break;
      case FactionManager.FactionID.Hostile:
        this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Attack);
        this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
        this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Attack);
        this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
        this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
        break;
      case FactionManager.FactionID.Prey:
        this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
        break;
      case FactionManager.FactionID.Predator:
        this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
        this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
        this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
        this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
        break;
      case FactionManager.FactionID.Pest:
        this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
        this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
        break;
    }
    foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> disposition in this.Dispositions)
    {
      if (disposition.Value == FactionManager.Disposition.Attack)
        this.CanAttack = true;
      if (disposition.Value == FactionManager.Disposition.Assist)
        this.CanAssist = true;
    }
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct FactionIDComparer : IEqualityComparer<FactionManager.FactionID>
  {
    public bool Equals(FactionManager.FactionID x, FactionManager.FactionID y) => x == y;

    public int GetHashCode(FactionManager.FactionID obj) => (int) obj;
  }
}
