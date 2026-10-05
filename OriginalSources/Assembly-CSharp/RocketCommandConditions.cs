// Decompiled with JetBrains decompiler
// Type: RocketCommandConditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class RocketCommandConditions : CommandConditions
{
  public ConditionHasAstronaut hasAstronaut;
  public ConditionPilotOnBoard pilotOnBoard;
  public ConditionPassengersOnBoard passengersOnBoard;
  public ConditionNoExtraPassengers noExtraPassengers;
  public ConditionHasAtmoSuit hasSuit;
  public ConditionHasControlStation hasControlStation;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    RocketModule component = this.GetComponent<RocketModule>();
    this.reachable = (ConditionDestinationReachable) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new ConditionDestinationReachable(this.GetComponent<RocketModule>()));
    this.allModulesComplete = (ConditionAllModulesComplete) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new ConditionAllModulesComplete(this.GetComponent<ILaunchableRocket>()));
    if (this.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Spacecraft)
    {
      this.destHasResources = (ConditionHasMinimumMass) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, (ProcessCondition) new ConditionHasMinimumMass(this.GetComponent<CommandModule>()));
      this.hasAstronaut = (ConditionHasAstronaut) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new ConditionHasAstronaut(this.GetComponent<CommandModule>()));
      this.hasSuit = (ConditionHasAtmoSuit) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, (ProcessCondition) new ConditionHasAtmoSuit(this.GetComponent<CommandModule>()));
      this.cargoEmpty = (CargoBayIsEmpty) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, (ProcessCondition) new CargoBayIsEmpty(this.GetComponent<CommandModule>()));
    }
    else if (this.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Clustercraft)
    {
      this.hasEngine = (ConditionHasEngine) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new ConditionHasEngine(this.GetComponent<ILaunchableRocket>()));
      this.hasNosecone = (ConditionHasNosecone) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new ConditionHasNosecone(this.GetComponent<LaunchableRocketCluster>()));
      this.hasControlStation = (ConditionHasControlStation) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new ConditionHasControlStation(this.GetComponent<RocketModuleCluster>()));
      this.pilotOnBoard = (ConditionPilotOnBoard) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, (ProcessCondition) new ConditionPilotOnBoard(this.GetComponent<PassengerRocketModule>()));
      this.passengersOnBoard = (ConditionPassengersOnBoard) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, (ProcessCondition) new ConditionPassengersOnBoard(this.GetComponent<PassengerRocketModule>()));
      this.noExtraPassengers = (ConditionNoExtraPassengers) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, (ProcessCondition) new ConditionNoExtraPassengers(this.GetComponent<PassengerRocketModule>()));
      this.onLaunchPad = (ConditionOnLaunchPad) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new ConditionOnLaunchPad(this.GetComponent<RocketModuleCluster>().CraftInterface));
    }
    int bufferWidth = 1;
    if (DlcManager.FeatureClusterSpaceEnabled())
      bufferWidth = 0;
    this.flightPathIsClear = (ConditionFlightPathIsClear) component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketFlight, (ProcessCondition) new ConditionFlightPathIsClear(this.gameObject, bufferWidth));
  }
}
