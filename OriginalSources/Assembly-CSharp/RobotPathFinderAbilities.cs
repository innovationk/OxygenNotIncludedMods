// Decompiled with JetBrains decompiler
// Type: RobotPathFinderAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RobotPathFinderAbilities : PathFinderAbilities
{
  public bool canTraverseSubmered;
  private Tag prefabTag;

  public RobotPathFinderAbilities(Navigator navigator)
    : base(navigator)
  {
    this.prefabTag = navigator.GetComponent<KPrefabID>().PrefabTag;
  }

  protected override void Refresh(Navigator navigator)
  {
    if (PathFinder.IsSubmerged(navigator.cachedCell))
      this.canTraverseSubmered = true;
    else
      this.canTraverseSubmered = Db.Get().Attributes.MaxUnderwaterTravelCost.Lookup((Component) navigator) == null;
  }

  public override bool TraversePath(
    ref PathFinder.PotentialPath path,
    int from_cell,
    NavType from_nav_type,
    int cost,
    int transition_id,
    bool submerged)
  {
    return (!submerged || this.canTraverseSubmered) && RobotPathFinderAbilities.IsAccessPermitted(this.prefabTag, path.cell, from_cell, from_nav_type);
  }

  private static bool IsAccessPermitted(
    Tag prefabTag,
    int cell,
    int from_cell,
    NavType from_nav_type)
  {
    int tagId1 = GridRestrictionSerializer.Instance.GetTagId(prefabTag);
    int tagId2 = GridRestrictionSerializer.Instance.GetTagId(GameTags.Robot);
    return Grid.HasPermission(cell, tagId1, tagId2, from_cell, from_nav_type);
  }

  public override PathFinderAbilities Clone()
  {
    RobotPathFinderAbilities pathFinderAbilities = new RobotPathFinderAbilities(this.navigator);
    pathFinderAbilities.prefabInstanceID = this.prefabInstanceID;
    pathFinderAbilities.canTraverseSubmered = this.canTraverseSubmered;
    pathFinderAbilities.prefabTag = this.prefabTag;
    return (PathFinderAbilities) pathFinderAbilities;
  }

  public override void RecycleClone()
  {
  }
}
