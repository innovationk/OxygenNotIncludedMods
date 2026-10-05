// Decompiled with JetBrains decompiler
// Type: ClusterGridEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class ClusterGridEntity : KMonoBehaviour
{
  [Serialize]
  protected AxialI m_location;
  public bool positionDirty;
  [MyCmpGet]
  protected KSelectable m_selectable;
  [MyCmpReq]
  private Transform m_transform;
  public bool isWorldEntity;

  public abstract string Name { get; }

  public abstract EntityLayer Layer { get; }

  public abstract List<ClusterGridEntity.AnimConfig> AnimConfigs { get; }

  public abstract bool IsVisible { get; }

  public virtual bool ShowName() => false;

  public virtual bool ShowProgressBar() => false;

  public virtual float GetProgress() => 0.0f;

  public virtual bool SpaceOutInSameHex() => false;

  public virtual bool KeepRotationWhenSpacingOutInHex() => false;

  public virtual bool ShowPath() => true;

  public virtual void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
  {
  }

  public abstract ClusterRevealLevel IsVisibleInFOW { get; }

  public AxialI Location
  {
    get => this.m_location;
    set
    {
      if (!(value != this.m_location))
        return;
      AxialI location = this.m_location;
      this.m_location = value;
      if (this.gameObject.GetSMI<StateMachine.Instance>() == null)
        this.positionDirty = true;
      this.SendClusterLocationChangedEvent(location, this.m_location);
    }
  }

  protected override void OnSpawn()
  {
    ClusterGrid.Instance.RegisterEntity(this);
    if ((Object) this.m_selectable != (Object) null)
      this.m_selectable.SetName(this.Name);
    if (!this.isWorldEntity)
      this.m_transform.SetLocalPosition(new Vector3(-1f, 0.0f, 0.0f));
    if (!((Object) ClusterMapScreen.Instance != (Object) null))
      return;
    ClusterMapScreen.Instance.Trigger(1980521255, (object) null);
  }

  protected override void OnCleanUp() => ClusterGrid.Instance.UnregisterEntity(this);

  public virtual Sprite GetUISprite()
  {
    if (DlcManager.FeatureClusterSpaceEnabled())
    {
      List<ClusterGridEntity.AnimConfig> animConfigs = this.AnimConfigs;
      if (animConfigs.Count > 0)
        return Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile);
    }
    else
    {
      WorldContainer component = this.GetComponent<WorldContainer>();
      if ((Object) component != (Object) null)
      {
        ProcGen.World worldData = SettingsCache.worlds.GetWorldData(component.worldName);
        return worldData == null ? (Sprite) null : Assets.GetSprite((HashedString) worldData.asteroidIcon);
      }
    }
    return (Sprite) null;
  }

  public void SendClusterLocationChangedEvent(AxialI oldLocation, AxialI newLocation)
  {
    ClusterLocationChangedEvent data = new ClusterLocationChangedEvent()
    {
      entity = this,
      oldLocation = oldLocation,
      newLocation = newLocation
    };
    this.Trigger(-1298331547, (object) data);
    Game.Instance.Trigger(-1298331547, (object) data);
    if (!((Object) this.m_selectable != (Object) null) || !this.m_selectable.IsSelected)
      return;
    DetailsScreen.Instance.Refresh(this.gameObject);
  }

  public virtual void onClustermapVisualizerAnimCreated(
    KBatchedAnimController controller,
    ClusterGridEntity.AnimConfig config)
  {
  }

  public struct AnimConfig
  {
    public KAnimFile animFile;
    public string initialAnim;
    public KAnim.PlayMode playMode;
    public string symbolSwapTarget;
    public string symbolSwapSymbol;
    public Vector3 animOffset;
    public float animPlaySpeedModifier;
    public object additionalInfo;
  }
}
