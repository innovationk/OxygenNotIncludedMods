// Decompiled with JetBrains decompiler
// Type: ConditionRobotPilotReady
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class ConditionRobotPilotReady : ProcessCondition
{
  private LaunchableRocketRegisterType craftRegisterType;
  private RoboPilotModule module;
  private CraftModuleInterface craftInterface;

  public ConditionRobotPilotReady(RoboPilotModule module)
  {
    this.module = module;
    this.craftRegisterType = module.GetComponent<ILaunchableRocket>().registerType;
    if (this.craftRegisterType != LaunchableRocketRegisterType.Clustercraft)
      return;
    this.craftInterface = module.GetComponent<RocketModuleCluster>().CraftInterface;
  }

  public override ProcessCondition.Status EvaluateCondition()
  {
    ProcessCondition.Status condition = ProcessCondition.Status.Failure;
    switch (this.craftRegisterType)
    {
      case LaunchableRocketRegisterType.Spacecraft:
        int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
        SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
        condition = spacecraftDestination != null ? (!this.module.HasResourcesToMove(spacecraftDestination.OneBasedDistance * 2) ? ProcessCondition.Status.Failure : ProcessCondition.Status.Ready) : ProcessCondition.Status.Failure;
        break;
      case LaunchableRocketRegisterType.Clustercraft:
        if (this.HasDestination())
        {
          Clustercraft component1 = this.craftInterface.GetComponent<Clustercraft>();
          ClusterTraveler component2 = this.craftInterface.GetComponent<ClusterTraveler>();
          if ((Object) component1 == (Object) null || (Object) component2 == (Object) null || component2.CurrentPath == null)
            return ProcessCondition.Status.Failure;
          int distance = component2.RemainingTravelNodes();
          int num = this.module.HasResourcesToMove(distance * 2) ? 1 : 0;
          bool move = this.module.HasResourcesToMove(distance);
          if (num != 0)
          {
            condition = ProcessCondition.Status.Ready;
            break;
          }
          if (move || this.RocketHasDupeControlStation())
          {
            condition = ProcessCondition.Status.Warning;
            break;
          }
          break;
        }
        break;
    }
    return condition;
  }

  private bool HasDestination()
  {
    if (this.craftRegisterType == LaunchableRocketRegisterType.Clustercraft)
      return !this.module.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<RocketClusterDestinationSelector>().IsAtDestination();
    if (this.craftRegisterType != LaunchableRocketRegisterType.Spacecraft)
      return false;
    int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
    return SpacecraftManager.instance.GetSpacecraftDestination(id) != null;
  }

  private bool RocketHasDupeControlStation()
  {
    if ((Object) this.craftInterface != (Object) null)
    {
      PassengerRocketModule passengerModule = this.craftInterface.GetPassengerModule();
      if ((Object) passengerModule != (Object) null)
        return passengerModule.CheckPilotBoarded();
    }
    return false;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    switch (status)
    {
      case ProcessCondition.Status.Warning:
        return this.RocketHasDupeControlStation() ? (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.WARNING_NO_DATA_BANKS_HUMAN_PILOT : (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.WARNING;
      case ProcessCondition.Status.Ready:
        return (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.READY;
      default:
        return (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.FAILURE;
    }
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    switch (this.craftRegisterType)
    {
      case LaunchableRocketRegisterType.Spacecraft:
        int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
        SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
        switch (status)
        {
          case ProcessCondition.Status.Warning:
            if (spacecraftDestination != null)
            {
              int num = spacecraftDestination.OneBasedDistance * 2;
              return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING, (object) this.module.GetDataBanksStored(), (object) num);
            }
            break;
          case ProcessCondition.Status.Ready:
            int num1 = spacecraftDestination.OneBasedDistance * 2;
            return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY, (object) this.module.GetDataBanksStored(), (object) num1);
          default:
            if (spacecraftDestination != null)
              return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE, (object) (spacecraftDestination.OneBasedDistance * 2), (object) this.module.GetDataBanksStored());
            return this.module.IsFull() ? string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION, (object) this.module.GetDataBanksStored()) : (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE_NO_DESTINATION;
        }
        break;
      case LaunchableRocketRegisterType.Clustercraft:
        ClusterTraveler component = this.craftInterface.GetComponent<ClusterTraveler>();
        switch (status)
        {
          case ProcessCondition.Status.Warning:
            if (this.RocketHasDupeControlStation())
              return (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING_NO_DATA_BANKS_HUMAN_PILOT;
            if ((Object) component == (Object) null || component.CurrentPath == null)
              return (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE_NO_DESTINATION;
            int num2 = component.RemainingTravelNodes() * 2 * this.module.dataBankConsumption;
            return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING, (object) this.module.GetDataBanksStored(), (object) num2);
          case ProcessCondition.Status.Ready:
            if (this.craftInterface.GetClusterDestinationSelector().IsAtDestination())
              return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION, (object) this.module.GetDataBanksStored());
            int num3 = component.RemainingTravelNodes() * 2 * this.module.dataBankConsumption;
            return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY, (object) this.module.GetDataBanksStored(), (object) num3);
          default:
            if (!this.HasDestination() || (Object) component == (Object) null || component.CurrentPath == null)
              return this.module.IsFull() ? string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION, (object) this.module.GetDataBanksStored()) : (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE_NO_DESTINATION;
            int num4 = component.RemainingTravelNodes();
            return string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE, (object) (num4 * this.module.dataBankConsumption), (object) this.module.GetDataBanksStored());
        }
    }
    DebugUtil.DevAssert(false, $"Rocket type {this.craftRegisterType.ToString()} does not have a status tooltip for {status.ToString()}");
    return (string) UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE_NO_DESTINATION;
  }

  public override bool ShowInUI() => true;
}
