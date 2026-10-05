// Decompiled with JetBrains decompiler
// Type: LaunchConditionManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/LaunchConditionManager")]
public class LaunchConditionManager : KMonoBehaviour, ISim4000ms, ISim1000ms
{
  public HashedString triggerPort;
  public HashedString statusPort;
  private ILaunchableRocket launchable;
  private Dictionary<ProcessCondition, Guid> conditionStatuses = new Dictionary<ProcessCondition, Guid>();

  public List<RocketModule> rocketModules { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.rocketModules = new List<RocketModule>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.launchable = this.GetComponent<ILaunchableRocket>();
    this.FindModules();
    this.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged = (Action<object>) (data => this.FindModules());
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public void Sim1000ms(float dt)
  {
    Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
    if (conditionManager == null)
      return;
    Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(conditionManager.id);
    if (this.gameObject.GetComponent<LogicPorts>().GetInputValue(this.triggerPort) != 1 || spacecraftDestination == null || spacecraftDestination.id == -1)
      return;
    this.Launch(spacecraftDestination);
  }

  public void FindModules()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      RocketModule component = gameObject.GetComponent<RocketModule>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.conditionManager == (UnityEngine.Object) null)
      {
        component.conditionManager = this;
        component.RegisterWithConditionManager();
      }
    }
  }

  public void RegisterRocketModule(RocketModule module)
  {
    if (this.rocketModules.Contains(module))
      return;
    this.rocketModules.Add(module);
  }

  public void UnregisterRocketModule(RocketModule module) => this.rocketModules.Remove(module);

  public List<ProcessCondition> GetLaunchConditionList()
  {
    List<ProcessCondition> conditions = new List<ProcessCondition>();
    foreach (RocketModule rocketModule in this.rocketModules)
    {
      rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep, conditions);
      rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage, conditions);
    }
    return conditions;
  }

  public void Launch(SpaceDestination destination)
  {
    if (destination == null)
      Debug.LogError((object) "Null destination passed to launch");
    if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).state != Spacecraft.MissionState.Grounded || !DebugHandler.InstantBuildMode && (!this.CheckReadyToLaunch() || !this.CheckAbleToFly()))
      return;
    this.launchable.LaunchableGameObject.Trigger(705820818);
    SpacecraftManager.instance.SetSpacecraftDestination(this, destination);
    SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this).BeginMission(destination);
  }

  public bool CheckReadyToLaunch()
  {
    List<ProcessCondition> v;
    using (ProcessCondition.ListPool.Get(out v))
    {
      foreach (RocketModule rocketModule in this.rocketModules)
      {
        rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep, v);
        foreach (ProcessCondition processCondition in v)
        {
          if (processCondition.EvaluateCondition() == ProcessCondition.Status.Failure)
            return false;
        }
        v.Clear();
        rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage, v);
        foreach (ProcessCondition processCondition in v)
        {
          if (processCondition.EvaluateCondition() == ProcessCondition.Status.Failure)
            return false;
        }
        v.Clear();
      }
    }
    return true;
  }

  public bool CheckAbleToFly()
  {
    List<ProcessCondition> v;
    using (ProcessCondition.ListPool.Get(out v))
    {
      foreach (RocketModule rocketModule in this.rocketModules)
      {
        rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight, v);
        foreach (ProcessCondition processCondition in v)
        {
          if (processCondition.EvaluateCondition() == ProcessCondition.Status.Failure)
            return false;
        }
        v.Clear();
      }
    }
    return true;
  }

  private void ClearFlightStatuses()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    foreach (KeyValuePair<ProcessCondition, Guid> conditionStatuse in this.conditionStatuses)
      component.RemoveStatusItem(conditionStatuse.Value);
    this.conditionStatuses.Clear();
  }

  public void Sim4000ms(float dt)
  {
    int num = this.CheckReadyToLaunch() ? 1 : 0;
    LogicPorts component1 = this.gameObject.GetComponent<LogicPorts>();
    if (num != 0)
    {
      Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this);
      if (conditionManager.state == Spacecraft.MissionState.Grounded || conditionManager.state == Spacecraft.MissionState.Launching)
        component1.SendSignal(this.statusPort, 1);
      else
        component1.SendSignal(this.statusPort, 0);
      KSelectable component2 = this.GetComponent<KSelectable>();
      List<ProcessCondition> v;
      using (ProcessCondition.ListPool.Get(out v))
      {
        foreach (RocketModule rocketModule in this.rocketModules)
        {
          rocketModule.PopulateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight, v);
          foreach (ProcessCondition processCondition in v)
          {
            if (processCondition.EvaluateCondition() == ProcessCondition.Status.Failure)
            {
              if (!this.conditionStatuses.ContainsKey(processCondition))
              {
                StatusItem statusItem = processCondition.GetStatusItem(ProcessCondition.Status.Failure);
                this.conditionStatuses[processCondition] = component2.AddStatusItem(statusItem, (object) processCondition);
              }
            }
            else if (this.conditionStatuses.ContainsKey(processCondition))
            {
              component2.RemoveStatusItem(this.conditionStatuses[processCondition]);
              this.conditionStatuses.Remove(processCondition);
            }
          }
          v.Clear();
        }
      }
    }
    else
    {
      this.ClearFlightStatuses();
      component1.SendSignal(this.statusPort, 0);
    }
  }

  public enum ConditionType
  {
    Launch,
    Flight,
  }
}
