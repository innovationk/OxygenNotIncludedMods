// Decompiled with JetBrains decompiler
// Type: AsteroidGridEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;

#nullable disable
public class AsteroidGridEntity : ClusterGridEntity
{
  public static string DEFAULT_ASTEROID_ICON_ANIM = "asteroid_sandstone_start_kanim";
  [MyCmpReq]
  private WorldContainer m_worldContainer;
  [Serialize]
  private string m_name;
  [Serialize]
  private string m_asteroidAnim;

  public override bool ShowName() => true;

  public override string Name => this.m_name;

  public override EntityLayer Layer => EntityLayer.Asteroid;

  public override List<ClusterGridEntity.AnimConfig> AnimConfigs
  {
    get
    {
      List<ClusterGridEntity.AnimConfig> animConfigs = new List<ClusterGridEntity.AnimConfig>();
      ClusterGridEntity.AnimConfig animConfig = new ClusterGridEntity.AnimConfig();
      animConfig.animFile = Assets.GetAnim((HashedString) (this.m_asteroidAnim.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : this.m_asteroidAnim));
      animConfig.initialAnim = "idle_loop";
      animConfigs.Add(animConfig);
      animConfig = new ClusterGridEntity.AnimConfig();
      animConfig.animFile = Assets.GetAnim((HashedString) "orbit_kanim");
      animConfig.initialAnim = "orbit";
      animConfigs.Add(animConfig);
      animConfig = new ClusterGridEntity.AnimConfig();
      animConfig.animFile = Assets.GetAnim((HashedString) "shower_asteroid_current_kanim");
      animConfig.initialAnim = "off";
      animConfig.playMode = KAnim.PlayMode.Once;
      animConfigs.Add(animConfig);
      return animConfigs;
    }
  }

  public override bool IsVisible => true;

  public override ClusterRevealLevel IsVisibleInFOW => ClusterRevealLevel.Peeked;

  public void Init(string name, AxialI location, string asteroidTypeId)
  {
    this.m_name = name;
    this.m_location = location;
    this.m_asteroidAnim = asteroidTypeId;
  }

  protected override void OnSpawn()
  {
    if (!Assets.TryGetAnim((HashedString) this.m_asteroidAnim, out KAnimFile _))
      this.m_asteroidAnim = AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM;
    Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
    Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
    Game.Instance.Subscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
    Game.Instance.Subscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
    if (ClusterGrid.Instance.IsCellVisible(this.m_location))
      SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.m_location, 1);
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
    Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
    Game.Instance.Unsubscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
    Game.Instance.Unsubscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
    base.OnCleanUp();
  }

  public void OnClusterLocationChanged(object data)
  {
    if (this.m_worldContainer.IsDiscovered || !ClusterGrid.Instance.IsCellVisible(this.Location))
      return;
    Clustercraft component = ((ClusterLocationChangedEvent) data).entity.GetComponent<Clustercraft>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !((UnityEngine.Object) component.GetOrbitAsteroid() == (UnityEngine.Object) this))
      return;
    this.m_worldContainer.SetDiscovered(true);
  }

  public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
  {
    base.OnClusterMapIconShown(levelUsed);
    if (levelUsed != ClusterRevealLevel.Visible)
      return;
    this.RefreshMeteorShowerEffect();
  }

  private void OnMeteorShowerEventChanged(object _worldID)
  {
    if (((Boxed<int>) _worldID).value != this.m_worldContainer.id)
      return;
    this.RefreshMeteorShowerEffect();
  }

  public void RefreshMeteorShowerEffect()
  {
    if ((UnityEngine.Object) ClusterMapScreen.Instance == (UnityEngine.Object) null)
      return;
    ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim((ClusterGridEntity) this);
    if ((UnityEngine.Object) entityVisAnim == (UnityEngine.Object) null)
      return;
    KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
    if (!((UnityEngine.Object) animController != (UnityEngine.Object) null))
      return;
    List<GameplayEventInstance> results = new List<GameplayEventInstance>();
    GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(this.m_worldContainer.id, ref results);
    bool flag = false;
    string anim_name = "off";
    foreach (GameplayEventInstance gameplayEventInstance in results)
    {
      if (gameplayEventInstance != null && gameplayEventInstance.smi is MeteorShowerEvent.StatesInstance)
      {
        MeteorShowerEvent.StatesInstance smi = gameplayEventInstance.smi as MeteorShowerEvent.StatesInstance;
        if (smi.IsInsideState((StateMachine.BaseState) smi.sm.running.bombarding))
        {
          flag = true;
          anim_name = "idle_loop";
          break;
        }
      }
    }
    animController.Play((HashedString) anim_name, flag ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once);
  }

  public void OnFogOfWarRevealed(object data = null)
  {
    if (data == null || ((Boxed<AxialI>) data).value != this.m_location || !ClusterGrid.Instance.IsCellVisible(this.Location) || !DlcManager.FeatureClusterSpaceEnabled())
      return;
    WorldDetectedMessage worldDetectedMessage = new WorldDetectedMessage(this.m_worldContainer);
    MusicManager.instance.PlaySong("Stinger_WorldDetected");
    Messenger.Instance.QueueMessage((Message) worldDetectedMessage);
    if (this.m_worldContainer.IsDiscovered)
      return;
    foreach (Clustercraft clustercraft in Components.Clustercrafts)
    {
      if ((UnityEngine.Object) clustercraft.GetOrbitAsteroid() == (UnityEngine.Object) this)
      {
        this.m_worldContainer.SetDiscovered(true);
        break;
      }
    }
  }
}
