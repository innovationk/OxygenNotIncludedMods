// Decompiled with JetBrains decompiler
// Type: TemporalTear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class TemporalTear : ClusterGridEntity
{
  [Serialize]
  private bool m_open;
  [Serialize]
  private bool m_hasConsumedCraft;

  public override string Name => Db.Get().SpaceDestinationTypes.Wormhole.typeName;

  public override EntityLayer Layer => EntityLayer.POI;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get
    {
      return new List<ClusterGridEntity.AnimConfig>()
      {
        new ClusterGridEntity.AnimConfig()
        {
          animFile = Assets.GetAnim((HashedString) "temporal_tear_kanim"),
          initialAnim = "closed_loop"
        }
      };
    }
  }

  public override bool IsVisible => true;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Peeked;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    ClusterManager.Instance.GetComponent<ClusterPOIManager>().RegisterTemporalTear(this);
    this.UpdateStatus();
  }

  public void UpdateStatus()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    ClusterMapVisualizer clusterMapVisualizer = (ClusterMapVisualizer) null;
    if ((Object) ClusterMapScreen.Instance != (Object) null)
      clusterMapVisualizer = ClusterMapScreen.Instance.GetEntityVisAnim((ClusterGridEntity) this);
    if (this.IsOpen())
    {
      if ((Object) clusterMapVisualizer != (Object) null)
        clusterMapVisualizer.PlayAnim("open_loop", KAnim.PlayMode.Loop);
      component.RemoveStatusItem(Db.Get().MiscStatusItems.TearClosed);
      component.AddStatusItem(Db.Get().MiscStatusItems.TearOpen);
    }
    else
    {
      if ((Object) clusterMapVisualizer != (Object) null)
        clusterMapVisualizer.PlayAnim("closed_loop", KAnim.PlayMode.Loop);
      component.RemoveStatusItem(Db.Get().MiscStatusItems.TearOpen);
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.TearClosed);
    }
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public void ConsumeCraft(Clustercraft craft)
  {
    if (!this.m_open || !(craft.Location == this.Location) || craft.IsFlightInProgress())
      return;
    for (int idx = 0; idx < Components.MinionIdentities.Count; ++idx)
    {
      MinionIdentity minionIdentity = Components.MinionIdentities[idx];
      if ((Object) minionIdentity != (Object) null && minionIdentity.GetMyWorldId() == craft.ModuleInterface.GetInteriorWorld().id)
        Util.KDestroyGameObject(minionIdentity.gameObject);
    }
    craft.DestroyCraftAndModules();
    this.m_hasConsumedCraft = true;
  }

  public void Open()
  {
    this.m_open = true;
    this.UpdateStatus();
  }

  public bool IsOpen() => this.m_open;

  public bool HasConsumedCraft() => this.m_hasConsumedCraft;
}
