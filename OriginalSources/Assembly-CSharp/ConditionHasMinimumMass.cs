// Decompiled with JetBrains decompiler
// Type: ConditionHasMinimumMass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class ConditionHasMinimumMass : ProcessCondition
{
  private CommandModule commandModule;

  public ConditionHasMinimumMass(CommandModule command) => this.commandModule = command;

  public override ProcessCondition.Status EvaluateCondition()
  {
    int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
    return spacecraftDestination != null && SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete && (double) spacecraftDestination.AvailableMass >= (double) ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule) ? ProcessCondition.Status.Ready : ProcessCondition.Status.Warning;
  }

  public override string GetStatusMessage(ProcessCondition.Status status)
  {
    int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
    if (spacecraftDestination == null)
      return (string) UI.STARMAP.LAUNCHCHECKLIST.NO_DESTINATION;
    return SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete ? string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.MINIMUM_MASS, (object) GameUtil.GetFormattedMass(spacecraftDestination.AvailableMass, massFormat: GameUtil.MetricMassFormat.Kilogram)) : string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.MINIMUM_MASS, (object) UI.STARMAP.COMPOSITION_UNDISCOVERED_AMOUNT);
  }

  public override string GetStatusTooltip(ProcessCondition.Status status)
  {
    int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
    bool flag = spacecraftDestination != null && SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete;
    string statusTooltip = "";
    if (flag)
    {
      if ((double) spacecraftDestination.AvailableMass <= (double) ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule))
        statusTooltip = $"{statusTooltip}{(string) UI.STARMAP.LAUNCHCHECKLIST.INSUFFICENT_MASS_TOOLTIP}\n\n";
      statusTooltip = $"{statusTooltip}{string.Format((string) UI.STARMAP.LAUNCHCHECKLIST.RESOURCE_MASS_TOOLTIP, (object) spacecraftDestination.GetDestinationType().Name, (object) GameUtil.GetFormattedMass(spacecraftDestination.AvailableMass, massFormat: GameUtil.MetricMassFormat.Kilogram), (object) GameUtil.GetFormattedMass(ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule), massFormat: GameUtil.MetricMassFormat.Kilogram))}\n\n";
    }
    float num = spacecraftDestination != null ? spacecraftDestination.AvailableMass : 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      CargoBay component = gameObject.GetComponent<CargoBay>();
      if ((Object) component != (Object) null)
      {
        if (flag)
        {
          float resourcesPercentage = spacecraftDestination.GetAvailableResourcesPercentage(component.storageType);
          float a = Mathf.Min(component.storage.Capacity(), resourcesPercentage * num);
          num -= a;
          statusTooltip = $"{statusTooltip}{component.gameObject.GetProperName()} {string.Format((string) UI.STARMAP.STORAGESTATS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(Mathf.Min(a, component.storage.Capacity()), massFormat: GameUtil.MetricMassFormat.Kilogram), (object) GameUtil.GetFormattedMass(component.storage.Capacity(), massFormat: GameUtil.MetricMassFormat.Kilogram))}\n";
        }
        else
          statusTooltip = $"{statusTooltip}{component.gameObject.GetProperName()} {string.Format((string) UI.STARMAP.STORAGESTATS.STORAGECAPACITY, (object) GameUtil.GetFormattedMass(0.0f, massFormat: GameUtil.MetricMassFormat.Kilogram), (object) GameUtil.GetFormattedMass(component.storage.Capacity(), massFormat: GameUtil.MetricMassFormat.Kilogram))}\n";
      }
    }
    return statusTooltip;
  }

  public static float CargoCapacity(SpaceDestination destination, CommandModule module)
  {
    if ((Object) module == (Object) null)
      return 0.0f;
    float num = 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(module.GetComponent<AttachableBuilding>()))
    {
      CargoBay component1 = gameObject.GetComponent<CargoBay>();
      if ((Object) component1 != (Object) null && destination.HasElementType(component1.storageType))
      {
        Storage component2 = component1.GetComponent<Storage>();
        num += component2.capacityKg;
      }
    }
    return num;
  }

  public override bool ShowInUI() => true;
}
