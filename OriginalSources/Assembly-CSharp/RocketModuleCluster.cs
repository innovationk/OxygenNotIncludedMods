// Decompiled with JetBrains decompiler
// Type: RocketModuleCluster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class RocketModuleCluster : RocketModule
{
  public RocketModulePerformance performanceStats;
  private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnNewConstructionDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>((Action<RocketModuleCluster, object>) ((component, data) => component.OnNewConstruction(data)));
  private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLaunchConditionChangedDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>((Action<RocketModuleCluster, object>) ((component, data) => component.OnLaunchConditionChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLandDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>((Action<RocketModuleCluster, object>) ((component, data) => component.OnLand(data)));
  private CraftModuleInterface _craftInterface;

  public CraftModuleInterface CraftInterface
  {
    get => this._craftInterface;
    set
    {
      this._craftInterface = value;
      if (!((UnityEngine.Object) this._craftInterface != (UnityEngine.Object) null))
        return;
      this.name = $"{this.name}: {this.GetParentRocketName()}";
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<RocketModuleCluster>(2121280625, RocketModuleCluster.OnNewConstructionDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.CraftInterface == (UnityEngine.Object) null && DlcManager.FeatureClusterSpaceEnabled())
      this.RegisterWithCraftModuleInterface();
    if (!((UnityEngine.Object) this.GetComponent<RocketEngine>() == (UnityEngine.Object) null) || !((UnityEngine.Object) this.GetComponent<RocketEngineCluster>() == (UnityEngine.Object) null) || !((UnityEngine.Object) this.GetComponent<BuildingUnderConstruction>() == (UnityEngine.Object) null))
      return;
    this.Subscribe<RocketModuleCluster>(1655598572, RocketModuleCluster.OnLaunchConditionChangedDelegate);
    this.Subscribe<RocketModuleCluster>(-887025858, RocketModuleCluster.OnLandDelegate);
  }

  protected void OnNewConstruction(object data)
  {
    Constructable constructable = (Constructable) data;
    if ((UnityEngine.Object) constructable == (UnityEngine.Object) null)
      return;
    RocketModuleCluster component = constructable.GetComponent<RocketModuleCluster>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || !((UnityEngine.Object) component.CraftInterface != (UnityEngine.Object) null))
      return;
    component.CraftInterface.AddModule(this);
  }

  private void RegisterWithCraftModuleInterface()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) this.gameObject))
      {
        RocketModuleCluster component = gameObject.GetComponent<RocketModuleCluster>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          component.CraftInterface.AddModule(this);
          break;
        }
      }
    }
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.CraftInterface.RemoveModule(this);
  }

  public override LaunchConditionManager FindLaunchConditionManager()
  {
    return this.CraftInterface.FindLaunchConditionManager();
  }

  public override string GetParentRocketName()
  {
    return (UnityEngine.Object) this.CraftInterface != (UnityEngine.Object) null ? this.CraftInterface.GetComponent<Clustercraft>().Name : this.parentRocketName;
  }

  private void OnLaunchConditionChanged(object data) => this.UpdateAnimations();

  private void OnLand(object data) => this.UpdateAnimations();

  protected void UpdateAnimations()
  {
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    Clustercraft component2 = (UnityEngine.Object) this.CraftInterface == (UnityEngine.Object) null ? (Clustercraft) null : this.CraftInterface.GetComponent<Clustercraft>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.Status == Clustercraft.CraftStatus.Launching && component1.HasAnimation((HashedString) "launch"))
    {
      component1.ClearQueue();
      if (component1.HasAnimation((HashedString) "launch_pre"))
        component1.Play((HashedString) "launch_pre");
      component1.Queue((HashedString) "launch", KAnim.PlayMode.Loop);
    }
    else if ((UnityEngine.Object) this.CraftInterface != (UnityEngine.Object) null && this.CraftInterface.CheckPreppedForLaunch())
    {
      component1.initialAnim = "ready_to_launch";
      component1.Play((HashedString) "pre_ready_to_launch");
      component1.Queue((HashedString) "ready_to_launch", KAnim.PlayMode.Loop);
    }
    else
    {
      component1.initialAnim = "grounded";
      component1.Play((HashedString) "pst_ready_to_launch");
      component1.Queue((HashedString) "grounded", KAnim.PlayMode.Loop);
    }
  }
}
