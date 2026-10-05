// Decompiled with JetBrains decompiler
// Type: CreaturePathFinderAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CreaturePathFinderAbilities(Navigator navigator) : PathFinderAbilities(navigator)
{
  public bool canTraverseSubmered;

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
    return !submerged || this.canTraverseSubmered;
  }

  public override PathFinderAbilities Clone()
  {
    CreaturePathFinderAbilities pathFinderAbilities = new CreaturePathFinderAbilities(this.navigator);
    pathFinderAbilities.prefabInstanceID = this.prefabInstanceID;
    pathFinderAbilities.canTraverseSubmered = this.canTraverseSubmered;
    return (PathFinderAbilities) pathFinderAbilities;
  }

  public override void RecycleClone()
  {
  }
}
