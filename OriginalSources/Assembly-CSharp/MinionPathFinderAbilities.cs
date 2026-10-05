// Decompiled with JetBrains decompiler
// Type: MinionPathFinderAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine.Pool;

#nullable disable
public class MinionPathFinderAbilities : PathFinderAbilities
{
  private int proxyID;
  private int accessControlDefaultKey;
  private bool out_of_fuel;
  private bool idleNavMaskEnabled;
  private bool hasSwimSkill;
  private static ObjectPool<MinionPathFinderAbilities> pool = new ObjectPool<MinionPathFinderAbilities>((Func<MinionPathFinderAbilities>) (() => new MinionPathFinderAbilities()), actionOnRelease: (Action<MinionPathFinderAbilities>) (obj =>
  {
    obj.navigator = (Navigator) null;
    obj.prefabInstanceID = -1;
  }), collectionCheck: false, defaultCapacity: 4, maxSize: 8);

  public MinionPathFinderAbilities(Navigator navigator)
    : base(navigator)
  {
  }

  private MinionPathFinderAbilities()
    : base((Navigator) null)
  {
  }

  protected override void Refresh(Navigator navigator)
  {
    MinionAssignablesProxy assignablesProxy = navigator.GetComponent<MinionIdentity>().assignableProxy.Get();
    this.proxyID = assignablesProxy.GetComponent<KPrefabID>().InstanceID;
    this.accessControlDefaultKey = GridRestrictionSerializer.Instance.GetTagId(assignablesProxy.GetMinionModel());
    this.out_of_fuel = navigator.HasTag(GameTags.JetSuitOutOfFuel);
    this.hasSwimSkill = navigator.GetComponent<MinionResume>().HasPerk(Db.Get().SkillPerks.CanSwim);
  }

  public void SetIdleNavMaskEnabled(bool enabled) => this.idleNavMaskEnabled = enabled;

  private static bool IsAccessPermitted(
    int proxyID,
    int proxyTag,
    int cell,
    int from_cell,
    NavType from_nav_type)
  {
    return Grid.HasPermission(cell, proxyID, proxyTag, from_cell, from_nav_type);
  }

  public override int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
  {
    bool flag1 = path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasLeadSuit);
    bool flag2 = link.endNavType == NavType.Swim;
    if (!this.hasSwimSkill)
      return !flag1 ? (int) link.cost * 2 : 0;
    if (flag1 & flag2)
      return (int) link.cost * 50;
    if (!flag1 && !flag2)
      return (int) link.cost * 2;
    return !flag1 & flag2 && PathFinder.IsSubmerged(link.link) ? (int) link.cost / 2 : 0;
  }

  public override bool TraversePath(
    ref PathFinder.PotentialPath path,
    int from_cell,
    NavType from_nav_type,
    int cost,
    int transition_id,
    bool submerged)
  {
    if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, this.accessControlDefaultKey, path.cell, from_cell, from_nav_type))
      return false;
    foreach (CellOffset voidOffset in this.navigator.NavGrid.transitions[transition_id].voidOffsets)
    {
      if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, this.accessControlDefaultKey, Grid.OffsetCell(from_cell, voidOffset), from_cell, from_nav_type))
        return false;
    }
    if (path.navType == NavType.Tube && from_nav_type == NavType.Floor && !Grid.HasUsableTubeEntrance(from_cell, this.prefabInstanceID) || !this.hasSwimSkill && (path.navType == NavType.Swim || from_nav_type == NavType.Swim) || path.navType == NavType.Hover && (this.out_of_fuel || !path.HasFlag(PathFinder.PotentialPath.Flags.HasJetPack)))
      return false;
    Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags) 0;
    PathFinder.PotentialPath.Flags pathFlags = PathFinder.PotentialPath.Flags.None;
    bool flag1 = path.HasFlag(PathFinder.PotentialPath.Flags.PerformSuitChecks) && Grid.TryGetSuitMarkerFlags(from_cell, out flags, out pathFlags) && (flags & Grid.SuitMarker.Flags.Operational) != 0;
    bool flag2 = SuitMarker.DoesTraversalDirectionRequireSuit(from_cell, path.cell, flags);
    bool flag3 = path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
    if (flag1)
    {
      bool flag4 = path.HasFlag(pathFlags);
      if (flag2)
      {
        if (!flag3 && !Grid.HasSuit(from_cell, this.prefabInstanceID))
          return false;
      }
      else if (flag3 && (flags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) != (Grid.SuitMarker.Flags) 0 && (!flag4 || !Grid.HasEmptyLocker(from_cell, this.prefabInstanceID)))
        return false;
    }
    if (this.idleNavMaskEnabled && (Grid.PreventIdleTraversal[path.cell] || Grid.PreventIdleTraversal[from_cell]))
      return false;
    if (flag1)
    {
      if (flag2)
      {
        if (!flag3)
          path.SetFlags(pathFlags);
      }
      else
        path.ClearFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
    }
    return true;
  }

  public override PathFinderAbilities Clone()
  {
    MinionPathFinderAbilities pathFinderAbilities = MinionPathFinderAbilities.pool.Get();
    pathFinderAbilities.navigator = this.navigator;
    pathFinderAbilities.prefabInstanceID = this.prefabInstanceID;
    pathFinderAbilities.proxyID = this.proxyID;
    pathFinderAbilities.accessControlDefaultKey = this.accessControlDefaultKey;
    pathFinderAbilities.out_of_fuel = this.out_of_fuel;
    pathFinderAbilities.idleNavMaskEnabled = this.idleNavMaskEnabled;
    pathFinderAbilities.hasSwimSkill = this.hasSwimSkill;
    return (PathFinderAbilities) pathFinderAbilities;
  }

  public override void RecycleClone() => MinionPathFinderAbilities.pool.Release(this);
}
