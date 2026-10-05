// Decompiled with JetBrains decompiler
// Type: ClusterDestinationSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;

#nullable disable
public class ClusterDestinationSelector : KMonoBehaviour
{
  [Serialize]
  protected AxialI m_destination;
  public bool assignable;
  public bool requireAsteroidDestination;
  [Serialize]
  public bool canNavigateFogOfWar;
  public bool dodgesHiddenAsteroids;
  public bool requireLaunchPadOnAsteroidDestination;
  public bool shouldPointTowardsPath;
  public string sidescreenTitleString = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE;
  public string changeTargetButtonTooltipString = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CHANGE_DESTINATION_BUTTON_TOOLTIP;
  public string clearTargetButtonTooltipString = (string) UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.CLEAR_DESTINATION_BUTTON_TOOLTIP;
  public EntityLayer requiredEntityLayer = EntityLayer.None;
  private EventSystem.IntraObjectHandler<ClusterDestinationSelector> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<ClusterDestinationSelector>((Action<ClusterDestinationSelector, object>) ((cmp, data) => cmp.OnClusterLocationChanged(data)));

  protected override void OnPrefabInit()
  {
    this.Subscribe<ClusterDestinationSelector>(-1298331547, this.OnClusterLocationChangedDelegate);
  }

  protected virtual void OnClusterLocationChanged(object data)
  {
    if (!(((ClusterLocationChangedEvent) data).newLocation == this.m_destination))
      return;
    this.Trigger(1796608350, data);
  }

  public int GetDestinationWorld() => ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination);

  public virtual AxialI GetDestination() => this.m_destination;

  public virtual ClusterGridEntity GetClusterEntityTarget() => (ClusterGridEntity) null;

  public virtual void SetDestination(AxialI location)
  {
    if (this.requireAsteroidDestination)
      Debug.Assert(ClusterUtil.GetAsteroidWorldIdAtLocation(location) != -1, (object) $"Cannot SetDestination to {location} as there is no world there");
    this.m_destination = location;
    this.BoxingTrigger<AxialI>(543433792, location);
  }

  public bool HasAsteroidDestination()
  {
    return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination) != -1;
  }

  public virtual bool IsAtDestination() => this.GetMyWorldLocation() == this.m_destination;
}
