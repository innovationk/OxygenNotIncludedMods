// Decompiled with JetBrains decompiler
// Type: EntityClusterDestinationSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
public class EntityClusterDestinationSelector : ClusterDestinationSelector
{
  [Serialize]
  protected Ref<ClusterGridEntity> m_DestinationEntity = new Ref<ClusterGridEntity>();

  private ClusterGridEntity DestinationEntity
  {
    get
    {
      return this.m_DestinationEntity == null ? (ClusterGridEntity) null : this.m_DestinationEntity.Get();
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Debug.Assert(this.requiredEntityLayer != EntityLayer.None, (object) "EnityClusterDestinationSelector must specify an EntityLayer");
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
  }

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    EntityClusterDestinationSelector component = gameObject.GetComponent<EntityClusterDestinationSelector>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.DestinationEntity != (UnityEngine.Object) null))
      return;
    this.m_DestinationEntity = new Ref<ClusterGridEntity>(component.DestinationEntity);
    this.SetDestination(this.m_DestinationEntity.Get().Location);
  }

  public override ClusterGridEntity GetClusterEntityTarget() => this.DestinationEntity;

  public override AxialI GetDestination()
  {
    return (UnityEngine.Object) this.DestinationEntity != (UnityEngine.Object) null ? this.DestinationEntity.Location : base.GetDestination();
  }

  public override void SetDestination(AxialI location)
  {
    this.m_DestinationEntity.Set(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, this.requiredEntityLayer));
    base.SetDestination(location);
  }
}
