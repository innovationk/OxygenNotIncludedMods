// Decompiled with JetBrains decompiler
// Type: RocketLaunchConditionVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RocketLaunchConditionVisualizer : KMonoBehaviour
{
  public RocketLaunchConditionVisualizer.RocketModuleVisualizeData[] moduleVisualizeData;
  private LaunchConditionManager launchConditionManager;
  private RocketModuleCluster clusterModule;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (DlcManager.FeatureClusterSpaceEnabled())
      this.clusterModule = this.GetComponent<RocketModuleCluster>();
    else
      this.launchConditionManager = this.GetComponent<LaunchConditionManager>();
    this.UpdateAllModuleData();
    this.Subscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
  }

  private void OnAnyRocketModuleChanged(object obj) => this.UpdateAllModuleData();

  private void UpdateAllModuleData()
  {
    if (this.moduleVisualizeData != null)
      this.moduleVisualizeData = (RocketLaunchConditionVisualizer.RocketModuleVisualizeData[]) null;
    bool flag = (UnityEngine.Object) this.clusterModule != (UnityEngine.Object) null;
    List<Ref<RocketModuleCluster>> refList = (List<Ref<RocketModuleCluster>>) null;
    List<RocketModule> rocketModuleList = (List<RocketModule>) null;
    if (flag)
    {
      refList = new List<Ref<RocketModuleCluster>>((IEnumerable<Ref<RocketModuleCluster>>) this.clusterModule.CraftInterface.ClusterModules);
      this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[refList.Count];
      refList.Sort((Comparison<Ref<RocketModuleCluster>>) ((a, b) => Grid.PosToXY(a.Get().transform.GetPosition()).y.CompareTo(Grid.PosToXY(b.Get().transform.GetPosition()).y)));
    }
    else
    {
      rocketModuleList = new List<RocketModule>((IEnumerable<RocketModule>) this.launchConditionManager.rocketModules);
      rocketModuleList.Sort((Comparison<RocketModule>) ((a, b) => Grid.PosToXY(a.transform.GetPosition()).y.CompareTo(Grid.PosToXY(b.transform.GetPosition()).y)));
      this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[rocketModuleList.Count];
    }
    for (int index = 0; index < this.moduleVisualizeData.Length; ++index)
    {
      RocketModule rocketModule = flag ? (RocketModule) refList[index].Get() : rocketModuleList[index];
      Building component = rocketModule.GetComponent<Building>();
      this.moduleVisualizeData[index] = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData()
      {
        Module = rocketModule,
        RangeMax = Mathf.FloorToInt((float) component.Def.WidthInCells / 2f),
        RangeMin = -Mathf.FloorToInt((float) (component.Def.WidthInCells - 1) / 2f)
      };
    }
  }

  public struct RocketModuleVisualizeData
  {
    public RocketModule Module;
    public Vector2I OriginOffset;
    public int RangeMin;
    public int RangeMax;
  }
}
