// Decompiled with JetBrains decompiler
// Type: PathFinderAbilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public abstract class PathFinderAbilities
{
  protected Navigator navigator;
  protected int prefabInstanceID;

  public PathFinderAbilities(Navigator navigator) => this.navigator = navigator;

  public virtual string KPROFILER_getName() => (string) null;

  public void Refresh()
  {
    this.prefabInstanceID = this.navigator.gameObject.GetComponent<KPrefabID>().InstanceID;
    this.navigator.cachedCell = Grid.PosToCell((KMonoBehaviour) this.navigator);
    this.Refresh(this.navigator);
  }

  protected abstract void Refresh(Navigator navigator);

  public abstract PathFinderAbilities Clone();

  public abstract void RecycleClone();

  public abstract bool TraversePath(
    ref PathFinder.PotentialPath path,
    int from_cell,
    NavType from_nav_type,
    int cost,
    int transition_id,
    bool submerged);

  public virtual int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
  {
    return 0;
  }
}
