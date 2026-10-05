// Decompiled with JetBrains decompiler
// Type: Database.ColonyAchievements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using STRINGS;
using System;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class ColonyAchievements : ResourceSet<ColonyAchievement>
{
  public ColonyAchievement Thriving;
  public ColonyAchievement ReachedDistantPlanet;
  public ColonyAchievement CollectedArtifacts;
  public ColonyAchievement Survived100Cycles;
  public ColonyAchievement ReachedSpace;
  public ColonyAchievement CompleteSkillBranch;
  public ColonyAchievement CompleteResearchTree;
  public ColonyAchievement Clothe8Dupes;
  public ColonyAchievement Build4NatureReserves;
  public ColonyAchievement Minimum20LivingDupes;
  public ColonyAchievement TameAGassyMoo;
  public ColonyAchievement CoolBuildingTo6K;
  public ColonyAchievement EatkCalFromMeatByCycle100;
  public ColonyAchievement NoFarmTilesAndKCal;
  public ColonyAchievement Generate240000kJClean;
  public ColonyAchievement BuildOutsideStartBiome;
  public ColonyAchievement Travel10000InTubes;
  public ColonyAchievement VarietyOfRooms;
  public ColonyAchievement TameAllBasicCritters;
  public ColonyAchievement SurviveOneYear;
  public ColonyAchievement ExploreOilBiome;
  public ColonyAchievement EatCookedFood;
  public ColonyAchievement BasicPumping;
  public ColonyAchievement BasicComforts;
  public ColonyAchievement PlumbedWashrooms;
  public ColonyAchievement AutomateABuilding;
  public ColonyAchievement MasterpiecePainting;
  public ColonyAchievement InspectPOI;
  public ColonyAchievement HatchACritter;
  public ColonyAchievement CuredDisease;
  public ColonyAchievement GeneratorTuneup;
  public ColonyAchievement ClearFOW;
  public ColonyAchievement HatchRefinement;
  public ColonyAchievement BunkerDoorDefense;
  public ColonyAchievement IdleDuplicants;
  public ColonyAchievement ExosuitCycles;
  public ColonyAchievement FirstTeleport;
  public ColonyAchievement SoftLaunch;
  public ColonyAchievement GMOOK;
  public ColonyAchievement MineTheGap;
  public ColonyAchievement LandedOnAllWorlds;
  public ColonyAchievement RadicalTrip;
  public ColonyAchievement SweeterThanHoney;
  public ColonyAchievement SurviveInARocket;
  public ColonyAchievement RunAReactor;
  public ColonyAchievement ActivateGeothermalPlant;
  public ColonyAchievement EfficientData;
  public ColonyAchievement AllTheCircuits;
  public ColonyAchievement AsteroidDestroyed;
  public ColonyAchievement AsteroidSurvived;
  public ColonyAchievement MinnowRecruited;

  public ColonyAchievements(ResourceSet parent)
    : base(nameof (ColonyAchievements), parent)
  {
    string name1 = (string) COLONY_ACHIEVEMENTS.THRIVING.NAME;
    string description1 = (string) COLONY_ACHIEVEMENTS.THRIVING.DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist1 = new List<ColonyAchievementRequirement>();
    requirementChecklist1.Add((ColonyAchievementRequirement) new CycleNumber(200));
    requirementChecklist1.Add((ColonyAchievementRequirement) new MinimumMorale());
    requirementChecklist1.Add((ColonyAchievementRequirement) new NumberOfDupes(12));
    requirementChecklist1.Add((ColonyAchievementRequirement) new MonumentBuilt());
    string messageTitle1 = (string) COLONY_ACHIEVEMENTS.THRIVING.MESSAGE_TITLE;
    string messageBody1 = (string) COLONY_ACHIEVEMENTS.THRIVING.MESSAGE_BODY;
    Action<KMonoBehaviour> VictorySequence1 = new Action<KMonoBehaviour>(ThrivingSequence.Start);
    EventReference nisGenericSnapshot1 = AudioMixerSnapshots.Get().VictoryNISGenericSnapshot;
    this.Thriving = this.Add(new ColonyAchievement(nameof (Thriving), "WINCONDITION_STAY", name1, description1, true, requirementChecklist1, messageTitle1, messageBody1, "victoryShorts/Stay", "victoryLoops/Stay_loop", VictorySequence1, nisGenericSnapshot1, "home_sweet_home"));
    ColonyAchievement colonyAchievement1;
    if (!DlcManager.IsExpansion1Active())
    {
      string name2 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.NAME;
      string description2 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist2 = new List<ColonyAchievementRequirement>();
      requirementChecklist2.Add((ColonyAchievementRequirement) new Database.ReachedSpace(Db.Get().SpaceDestinationTypes.Wormhole));
      string messageTitle2 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.MESSAGE_TITLE;
      string messageBody2 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.MESSAGE_BODY;
      Action<KMonoBehaviour> VictorySequence2 = new Action<KMonoBehaviour>(ReachedDistantPlanetSequence.Start);
      EventReference nisRocketSnapshot = AudioMixerSnapshots.Get().VictoryNISRocketSnapshot;
      colonyAchievement1 = this.Add(new ColonyAchievement(nameof (ReachedDistantPlanet), "WINCONDITION_LEAVE", name2, description2, true, requirementChecklist2, messageTitle2, messageBody2, "victoryShorts/Leave", "victoryLoops/Leave_loop", VictorySequence2, nisRocketSnapshot, "rocket"));
    }
    else
    {
      string name3 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.NAME;
      string description3 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist3 = new List<ColonyAchievementRequirement>();
      requirementChecklist3.Add((ColonyAchievementRequirement) new EstablishColonies());
      requirementChecklist3.Add((ColonyAchievementRequirement) new OpenTemporalTear());
      requirementChecklist3.Add((ColonyAchievementRequirement) new SentCraftIntoTemporalTear());
      string messageTitleDlC1 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.MESSAGE_TITLE_DLC1;
      string messageBodyDlC1 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.MESSAGE_BODY_DLC1;
      Action<KMonoBehaviour> VictorySequence3 = new Action<KMonoBehaviour>(EnterTemporalTearSequence.Start);
      EventReference nisRocketSnapshot = AudioMixerSnapshots.Get().VictoryNISRocketSnapshot;
      colonyAchievement1 = this.Add(new ColonyAchievement(nameof (ReachedDistantPlanet), "WINCONDITION_LEAVE", name3, description3, true, requirementChecklist3, messageTitleDlC1, messageBodyDlC1, "victoryShorts/Leave", "victoryLoops/Leave_loop", VictorySequence3, nisRocketSnapshot, "rocket"));
    }
    this.ReachedDistantPlanet = colonyAchievement1;
    if (DlcManager.IsExpansion1Active())
    {
      string name4 = (string) COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.NAME;
      string description4 = (string) COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist4 = new List<ColonyAchievementRequirement>();
      requirementChecklist4.Add((ColonyAchievementRequirement) new Database.CollectedArtifacts());
      requirementChecklist4.Add((ColonyAchievementRequirement) new CollectedSpaceArtifacts());
      string messageTitle3 = (string) COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.MESSAGE_TITLE;
      string messageBody3 = (string) COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.MESSAGE_BODY;
      Action<KMonoBehaviour> VictorySequence4 = new Action<KMonoBehaviour>(ArtifactSequence.Start);
      EventReference nisGenericSnapshot2 = AudioMixerSnapshots.Get().VictoryNISGenericSnapshot;
      string[] expansioN1 = DlcManager.EXPANSION1;
      this.CollectedArtifacts = new ColonyAchievement(nameof (CollectedArtifacts), "WINCONDITION_ARTIFACTS", name4, description4, true, requirementChecklist4, messageTitle3, messageBody3, "victoryShorts/Artifact", "victoryLoops/Artifact_loop", VictorySequence4, nisGenericSnapshot2, "cosmic_archaeology", expansioN1, dlcIdFrom: "EXPANSION1_ID");
      this.Add(this.CollectedArtifacts);
    }
    if (DlcManager.IsContentSubscribed("DLC2_ID"))
    {
      string name5 = (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NAME;
      string description5 = (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist5 = new List<ColonyAchievementRequirement>();
      requirementChecklist5.Add((ColonyAchievementRequirement) new DiscoverGeothermalFacility());
      requirementChecklist5.Add((ColonyAchievementRequirement) new RepairGeothermalController());
      requirementChecklist5.Add((ColonyAchievementRequirement) new UseGeothermalPlant());
      requirementChecklist5.Add((ColonyAchievementRequirement) new ClearBlockedGeothermalVent());
      string messageTitle4 = (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.MESSAGE_TITLE;
      string messageBody4 = (string) COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.MESSAGE_BODY;
      Action<KMonoBehaviour> VictorySequence5 = new Action<KMonoBehaviour>(GeothermalVictorySequence.Start);
      EventReference nisGenericSnapshot3 = AudioMixerSnapshots.Get().VictoryNISGenericSnapshot;
      string[] dlC2 = DlcManager.DLC2;
      this.ActivateGeothermalPlant = this.Add(new ColonyAchievement("ActivatedGeothermalPlant", "WINCONDITION_GEOPLANT", name5, description5, true, requirementChecklist5, messageTitle4, messageBody4, "victoryShorts/Geothermal", "victoryLoops/Geothermal_loop", VictorySequence5, nisGenericSnapshot3, "geothermalplant", dlC2, dlcIdFrom: "DLC2_ID", clusterTag: "GeothermalImperative"));
    }
    string surviveHundredCycles = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_HUNDRED_CYCLES;
    string cyclesDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_HUNDRED_CYCLES_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist6 = new List<ColonyAchievementRequirement>();
    requirementChecklist6.Add((ColonyAchievementRequirement) new CycleNumber());
    EventReference victorySnapshot1 = new EventReference();
    this.Survived100Cycles = this.Add(new ColonyAchievement(nameof (Survived100Cycles), "SURVIVE_HUNDRED_CYCLES", surviveHundredCycles, cyclesDescription, false, requirementChecklist6, victorySnapshot: victorySnapshot1, icon: "Turn_of_the_Century"));
    ColonyAchievement colonyAchievement2;
    if (!DlcManager.IsExpansion1Active())
    {
      string spaceAnyDestination = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION;
      string destinationDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist7 = new List<ColonyAchievementRequirement>();
      requirementChecklist7.Add((ColonyAchievementRequirement) new Database.ReachedSpace());
      EventReference victorySnapshot2 = new EventReference();
      colonyAchievement2 = this.Add(new ColonyAchievement(nameof (ReachedSpace), "REACH_SPACE_ANY_DESTINATION", spaceAnyDestination, destinationDescription, false, requirementChecklist7, victorySnapshot: victorySnapshot2, icon: "space_race"));
    }
    else
    {
      string spaceAnyDestination = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION;
      string destinationDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist8 = new List<ColonyAchievementRequirement>();
      requirementChecklist8.Add((ColonyAchievementRequirement) new LaunchedCraft());
      EventReference victorySnapshot3 = new EventReference();
      colonyAchievement2 = this.Add(new ColonyAchievement(nameof (ReachedSpace), "REACH_SPACE_ANY_DESTINATION", spaceAnyDestination, destinationDescription, false, requirementChecklist8, victorySnapshot: victorySnapshot3, icon: "space_race"));
    }
    this.ReachedSpace = colonyAchievement2;
    string completedSkillBranch = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_SKILL_BRANCH;
    string branchDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_SKILL_BRANCH_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist9 = new List<ColonyAchievementRequirement>();
    requirementChecklist9.Add((ColonyAchievementRequirement) new SkillBranchComplete(Db.Get().Skills.GetTerminalSkills()));
    EventReference victorySnapshot4 = new EventReference();
    this.CompleteSkillBranch = this.Add(new ColonyAchievement(nameof (CompleteSkillBranch), "COMPLETED_SKILL_BRANCH", completedSkillBranch, branchDescription, false, requirementChecklist9, victorySnapshot: victorySnapshot4, icon: nameof (CompleteSkillBranch)));
    string completedResearch = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_RESEARCH;
    string researchDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_RESEARCH_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist10 = new List<ColonyAchievementRequirement>();
    requirementChecklist10.Add((ColonyAchievementRequirement) new ResearchComplete());
    EventReference victorySnapshot5 = new EventReference();
    this.CompleteResearchTree = this.Add(new ColonyAchievement(nameof (CompleteResearchTree), "COMPLETED_RESEARCH", completedResearch, researchDescription, false, requirementChecklist10, victorySnapshot: victorySnapshot5, icon: "honorary_doctorate"));
    string equipNDupes = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EQUIP_N_DUPES;
    string description6 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EQUIP_N_DUPES_DESCRIPTION, (object) 8);
    List<ColonyAchievementRequirement> requirementChecklist11 = new List<ColonyAchievementRequirement>();
    requirementChecklist11.Add((ColonyAchievementRequirement) new EquipNDupes(Db.Get().AssignableSlots.Outfit, 8));
    EventReference victorySnapshot6 = new EventReference();
    this.Clothe8Dupes = this.Add(new ColonyAchievement(nameof (Clothe8Dupes), "EQUIP_EIGHT_DUPES", equipNDupes, description6, false, requirementChecklist11, victorySnapshot: victorySnapshot6, icon: "and_nowhere_to_go"));
    string tameBasicCritters = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_BASIC_CRITTERS;
    string crittersDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_BASIC_CRITTERS_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist12 = new List<ColonyAchievementRequirement>();
    requirementChecklist12.Add((ColonyAchievementRequirement) new CritterTypesWithTraits(new List<Tag>()
    {
      (Tag) "Drecko",
      (Tag) "Hatch",
      (Tag) "LightBug",
      (Tag) "Mole",
      (Tag) "Oilfloater",
      (Tag) "Pacu",
      (Tag) "Puft",
      (Tag) "Moo",
      (Tag) "Crab",
      (Tag) "Squirrel"
    }));
    EventReference victorySnapshot7 = new EventReference();
    this.TameAllBasicCritters = this.Add(new ColonyAchievement(nameof (TameAllBasicCritters), "TAME_BASIC_CRITTERS", tameBasicCritters, crittersDescription, false, requirementChecklist12, victorySnapshot: victorySnapshot7, icon: "Animal_friends"));
    string buildNatureReserves = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUILD_NATURE_RESERVES;
    string description7 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUILD_NATURE_RESERVES_DESCRIPTION, (object) Db.Get().RoomTypes.NatureReserve.Name, (object) 4);
    List<ColonyAchievementRequirement> requirementChecklist13 = new List<ColonyAchievementRequirement>();
    requirementChecklist13.Add((ColonyAchievementRequirement) new BuildNRoomTypes(Db.Get().RoomTypes.NatureReserve, 4));
    EventReference victorySnapshot8 = new EventReference();
    this.Build4NatureReserves = this.Add(new ColonyAchievement(nameof (Build4NatureReserves), "BUILD_NATURE_RESERVES", buildNatureReserves, description7, false, requirementChecklist13, victorySnapshot: victorySnapshot8, icon: "Some_Reservations"));
    string twentyDupes = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TWENTY_DUPES;
    string dupesDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TWENTY_DUPES_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist14 = new List<ColonyAchievementRequirement>();
    requirementChecklist14.Add((ColonyAchievementRequirement) new NumberOfDupes(20));
    EventReference victorySnapshot9 = new EventReference();
    this.Minimum20LivingDupes = this.Add(new ColonyAchievement(nameof (Minimum20LivingDupes), "TWENTY_DUPES", twentyDupes, dupesDescription, false, requirementChecklist14, victorySnapshot: victorySnapshot9, icon: "no_place_like_clone"));
    string tameGassymoo = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_GASSYMOO;
    string gassymooDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_GASSYMOO_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist15 = new List<ColonyAchievementRequirement>();
    requirementChecklist15.Add((ColonyAchievementRequirement) new CritterTypesWithTraits(new List<Tag>()
    {
      (Tag) "Moo",
      (Tag) "DieselMoo"
    }, false));
    EventReference victorySnapshot10 = new EventReference();
    this.TameAGassyMoo = this.Add(new ColonyAchievement(nameof (TameAGassyMoo), "TAME_GASSYMOO", tameGassymoo, gassymooDescription, false, requirementChecklist15, victorySnapshot: victorySnapshot10, icon: "moovin_on_up"));
    string sixkelvinBuilding = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SIXKELVIN_BUILDING;
    string buildingDescription1 = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SIXKELVIN_BUILDING_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist16 = new List<ColonyAchievementRequirement>();
    requirementChecklist16.Add((ColonyAchievementRequirement) new CoolBuildingToXKelvin(6));
    EventReference victorySnapshot11 = new EventReference();
    this.CoolBuildingTo6K = this.Add(new ColonyAchievement(nameof (CoolBuildingTo6K), "SIXKELVIN_BUILDING", sixkelvinBuilding, buildingDescription1, false, requirementChecklist16, victorySnapshot: victorySnapshot11, icon: "not_0k"));
    string eatMeat = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EAT_MEAT;
    string eatMeatDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EAT_MEAT_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist17 = new List<ColonyAchievementRequirement>();
    requirementChecklist17.Add((ColonyAchievementRequirement) new BeforeCycleNumber());
    requirementChecklist17.Add((ColonyAchievementRequirement) new EatXCaloriesFromY(400000, new List<string>()
    {
      TUNING.FOOD.FOOD_TYPES.MEAT.Id,
      TUNING.FOOD.FOOD_TYPES.DEEP_FRIED_MEAT.Id,
      TUNING.FOOD.FOOD_TYPES.PEMMICAN.Id,
      TUNING.FOOD.FOOD_TYPES.FISH_MEAT.Id,
      TUNING.FOOD.FOOD_TYPES.COOKED_FISH.Id,
      TUNING.FOOD.FOOD_TYPES.DEEP_FRIED_FISH.Id,
      TUNING.FOOD.FOOD_TYPES.SHELLFISH_MEAT.Id,
      TUNING.FOOD.FOOD_TYPES.DEEP_FRIED_SHELLFISH.Id,
      TUNING.FOOD.FOOD_TYPES.COOKED_MEAT.Id,
      TUNING.FOOD.FOOD_TYPES.SURF_AND_TURF.Id,
      TUNING.FOOD.FOOD_TYPES.BURGER.Id,
      TUNING.FOOD.FOOD_TYPES.JAWBOFILLET.Id,
      TUNING.FOOD.FOOD_TYPES.SMOKED_FISH.Id,
      TUNING.FOOD.FOOD_TYPES.SMOKED_DINOSAURMEAT.Id,
      TUNING.FOOD.FOOD_TYPES.SQUID_MEAT.Id,
      TUNING.FOOD.FOOD_TYPES.URCHINMEAT.Id,
      TUNING.FOOD.FOOD_TYPES.NIGIRI.Id,
      TUNING.FOOD.FOOD_TYPES.MAKI.Id
    }));
    EventReference victorySnapshot12 = new EventReference();
    this.EatkCalFromMeatByCycle100 = this.Add(new ColonyAchievement(nameof (EatkCalFromMeatByCycle100), "EAT_MEAT", eatMeat, eatMeatDescription, false, requirementChecklist17, victorySnapshot: victorySnapshot12, icon: "Carnivore"));
    string noPlanterbox = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.NO_PLANTERBOX;
    string planterboxDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.NO_PLANTERBOX_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist18 = new List<ColonyAchievementRequirement>();
    requirementChecklist18.Add((ColonyAchievementRequirement) new NoFarmables());
    requirementChecklist18.Add((ColonyAchievementRequirement) new EatXCalories(400000));
    EventReference victorySnapshot13 = new EventReference();
    this.NoFarmTilesAndKCal = this.Add(new ColonyAchievement(nameof (NoFarmTilesAndKCal), "NO_PLANTERBOX", noPlanterbox, planterboxDescription, false, requirementChecklist18, victorySnapshot: victorySnapshot13, icon: "Locavore"));
    string cleanEnergy = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAN_ENERGY;
    string energyDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAN_ENERGY_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist19 = new List<ColonyAchievementRequirement>();
    requirementChecklist19.Add((ColonyAchievementRequirement) new ProduceXEngeryWithoutUsingYList(240000f, new List<Tag>()
    {
      (Tag) "MethaneGenerator",
      (Tag) "PetroleumGenerator",
      (Tag) "WoodGasGenerator",
      (Tag) "Generator",
      (Tag) "PeatGenerator"
    }));
    EventReference victorySnapshot14 = new EventReference();
    this.Generate240000kJClean = this.Add(new ColonyAchievement(nameof (Generate240000kJClean), "CLEAN_ENERGY", cleanEnergy, energyDescription, false, requirementChecklist19, victorySnapshot: victorySnapshot14, icon: "sustainably_sustaining"));
    string buildOutsideBiome = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUILD_OUTSIDE_BIOME;
    string biomeDescription1 = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUILD_OUTSIDE_BIOME_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist20 = new List<ColonyAchievementRequirement>();
    requirementChecklist20.Add((ColonyAchievementRequirement) new Database.BuildOutsideStartBiome());
    EventReference victorySnapshot15 = new EventReference();
    this.BuildOutsideStartBiome = this.Add(new ColonyAchievement(nameof (BuildOutsideStartBiome), "BUILD_OUTSIDE_BIOME", buildOutsideBiome, biomeDescription1, false, requirementChecklist20, victorySnapshot: victorySnapshot15, icon: "build_outside"));
    string tubeTravelDistance = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TUBE_TRAVEL_DISTANCE;
    string distanceDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TUBE_TRAVEL_DISTANCE_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist21 = new List<ColonyAchievementRequirement>();
    requirementChecklist21.Add((ColonyAchievementRequirement) new TravelXUsingTransitTubes(NavType.Tube, 10000));
    EventReference victorySnapshot16 = new EventReference();
    this.Travel10000InTubes = this.Add(new ColonyAchievement(nameof (Travel10000InTubes), "TUBE_TRAVEL_DISTANCE", tubeTravelDistance, distanceDescription, false, requirementChecklist21, victorySnapshot: victorySnapshot16, icon: "Totally-Tubular"));
    string varietyOfRooms = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.VARIETY_OF_ROOMS;
    string roomsDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.VARIETY_OF_ROOMS_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist22 = new List<ColonyAchievementRequirement>();
    requirementChecklist22.Add((ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.NatureReserve));
    requirementChecklist22.Add((ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.Hospital));
    requirementChecklist22.Add((ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.RecRoom));
    requirementChecklist22.Add((ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.GreatHall));
    requirementChecklist22.Add((ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.Bedroom));
    requirementChecklist22.Add((ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.PlumbedBathroom));
    requirementChecklist22.Add((ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.Farm));
    requirementChecklist22.Add((ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.CreaturePen));
    EventReference victorySnapshot17 = new EventReference();
    this.VarietyOfRooms = this.Add(new ColonyAchievement(nameof (VarietyOfRooms), "VARIETY_OF_ROOMS", varietyOfRooms, roomsDescription, false, requirementChecklist22, victorySnapshot: victorySnapshot17, icon: "Get-a-Room"));
    string surviveOneYear = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_ONE_YEAR;
    string oneYearDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_ONE_YEAR_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist23 = new List<ColonyAchievementRequirement>();
    requirementChecklist23.Add((ColonyAchievementRequirement) new FractionalCycleNumber(365.25f));
    EventReference victorySnapshot18 = new EventReference();
    this.SurviveOneYear = this.Add(new ColonyAchievement(nameof (SurviveOneYear), "SURVIVE_ONE_YEAR", surviveOneYear, oneYearDescription, false, requirementChecklist23, victorySnapshot: victorySnapshot18, icon: "One_year"));
    string exploreOilBiome = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXPLORE_OIL_BIOME;
    string biomeDescription2 = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXPLORE_OIL_BIOME_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist24 = new List<ColonyAchievementRequirement>();
    requirementChecklist24.Add((ColonyAchievementRequirement) new ExploreOilFieldSubZone());
    EventReference victorySnapshot19 = new EventReference();
    this.ExploreOilBiome = this.Add(new ColonyAchievement(nameof (ExploreOilBiome), "EXPLORE_OIL_BIOME", exploreOilBiome, biomeDescription2, false, requirementChecklist24, victorySnapshot: victorySnapshot19, icon: "enter_oil_biome"));
    string cookedFood = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COOKED_FOOD;
    string cookedFoodDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COOKED_FOOD_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist25 = new List<ColonyAchievementRequirement>();
    requirementChecklist25.Add((ColonyAchievementRequirement) new EatXKCalProducedByY(1, new List<Tag>()
    {
      (Tag) "GourmetCookingStation",
      (Tag) "CookingStation",
      (Tag) "Deepfryer",
      (Tag) "Smoker",
      (Tag) "SushiBar"
    }));
    EventReference victorySnapshot20 = new EventReference();
    this.EatCookedFood = this.Add(new ColonyAchievement(nameof (EatCookedFood), "COOKED_FOOD", cookedFood, cookedFoodDescription, false, requirementChecklist25, victorySnapshot: victorySnapshot20, icon: "its_not_raw"));
    string basicPumping = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_PUMPING;
    string pumpingDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_PUMPING_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist26 = new List<ColonyAchievementRequirement>();
    requirementChecklist26.Add((ColonyAchievementRequirement) new VentXKG(SimHashes.Oxygen, 1000f));
    EventReference victorySnapshot21 = new EventReference();
    this.BasicPumping = this.Add(new ColonyAchievement(nameof (BasicPumping), "BASIC_PUMPING", basicPumping, pumpingDescription, false, requirementChecklist26, victorySnapshot: victorySnapshot21, icon: nameof (BasicPumping)));
    string basicComforts = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_COMFORTS;
    string comfortsDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_COMFORTS_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist27 = new List<ColonyAchievementRequirement>();
    requirementChecklist27.Add((ColonyAchievementRequirement) new AtLeastOneBuildingForEachDupe(new List<Tag>()
    {
      (Tag) "FlushToilet",
      (Tag) "Outhouse"
    }));
    requirementChecklist27.Add((ColonyAchievementRequirement) new AtLeastOneBuildingForEachDupe(new List<Tag>()
    {
      (Tag) "Bed",
      (Tag) "LuxuryBed"
    }));
    EventReference victorySnapshot22 = new EventReference();
    this.BasicComforts = this.Add(new ColonyAchievement(nameof (BasicComforts), "BASIC_COMFORTS", basicComforts, comfortsDescription, false, requirementChecklist27, victorySnapshot: victorySnapshot22, icon: "1bed_1toilet"));
    string plumbedWashrooms = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.PLUMBED_WASHROOMS;
    string washroomsDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.PLUMBED_WASHROOMS_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist28 = new List<ColonyAchievementRequirement>();
    requirementChecklist28.Add((ColonyAchievementRequirement) new UpgradeAllBasicBuildings((Tag) "Outhouse", (Tag) "FlushToilet"));
    requirementChecklist28.Add((ColonyAchievementRequirement) new UpgradeAllBasicBuildings((Tag) "WashBasin", (Tag) "WashSink"));
    EventReference victorySnapshot23 = new EventReference();
    this.PlumbedWashrooms = this.Add(new ColonyAchievement(nameof (PlumbedWashrooms), "PLUMBED_WASHROOMS", plumbedWashrooms, washroomsDescription, false, requirementChecklist28, victorySnapshot: victorySnapshot23, icon: "royal_flush"));
    string automateABuilding = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.AUTOMATE_A_BUILDING;
    string buildingDescription2 = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.AUTOMATE_A_BUILDING_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist29 = new List<ColonyAchievementRequirement>();
    requirementChecklist29.Add((ColonyAchievementRequirement) new Database.AutomateABuilding());
    EventReference victorySnapshot24 = new EventReference();
    this.AutomateABuilding = this.Add(new ColonyAchievement(nameof (AutomateABuilding), "AUTOMATE_A_BUILDING", automateABuilding, buildingDescription2, false, requirementChecklist29, victorySnapshot: victorySnapshot24, icon: "red_light_green_light"));
    string masterpiecePainting = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MASTERPIECE_PAINTING;
    string paintingDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MASTERPIECE_PAINTING_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist30 = new List<ColonyAchievementRequirement>();
    requirementChecklist30.Add((ColonyAchievementRequirement) new CreateMasterPainting());
    EventReference victorySnapshot25 = new EventReference();
    this.MasterpiecePainting = this.Add(new ColonyAchievement(nameof (MasterpiecePainting), "MASTERPIECE_PAINTING", masterpiecePainting, paintingDescription, false, requirementChecklist30, victorySnapshot: victorySnapshot25, icon: "art_underground"));
    string inspectPoi = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.INSPECT_POI;
    string inspectPoiDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.INSPECT_POI_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist31 = new List<ColonyAchievementRequirement>();
    requirementChecklist31.Add((ColonyAchievementRequirement) new ActivateLorePOI());
    EventReference victorySnapshot26 = new EventReference();
    this.InspectPOI = this.Add(new ColonyAchievement(nameof (InspectPOI), "INSPECT_POI", inspectPoi, inspectPoiDescription, false, requirementChecklist31, victorySnapshot: victorySnapshot26, icon: "ghosts_of_gravitas"));
    string hatchACritter = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_A_CRITTER;
    string critterDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_A_CRITTER_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist32 = new List<ColonyAchievementRequirement>();
    requirementChecklist32.Add((ColonyAchievementRequirement) new CritterTypeExists(new List<Tag>()
    {
      (Tag) "DreckoPlasticBaby",
      (Tag) "HatchHardBaby",
      (Tag) "HatchMetalBaby",
      (Tag) "HatchVeggieBaby",
      (Tag) "LightBugBlackBaby",
      (Tag) "LightBugBlueBaby",
      (Tag) "LightBugCrystalBaby",
      (Tag) "LightBugOrangeBaby",
      (Tag) "LightBugPinkBaby",
      (Tag) "LightBugPurpleBaby",
      (Tag) "OilfloaterDecorBaby",
      (Tag) "OilfloaterHighTempBaby",
      (Tag) "PacuCleanerBaby",
      (Tag) "PacuTropicalBaby",
      (Tag) "PuftBleachstoneBaby",
      (Tag) "PuftOxyliteBaby",
      (Tag) "SquirrelHugBaby",
      (Tag) "CrabWoodBaby",
      (Tag) "CrabFreshWaterBaby",
      (Tag) "MoleDelicacyBaby",
      (Tag) "GlassDeerBaby",
      (Tag) "AlgaeStegoBaby",
      (Tag) "SnailIronBaby"
    }));
    EventReference victorySnapshot27 = new EventReference();
    this.HatchACritter = this.Add(new ColonyAchievement(nameof (HatchACritter), "HATCH_A_CRITTER", hatchACritter, critterDescription, false, requirementChecklist32, victorySnapshot: victorySnapshot27, icon: "good_egg"));
    string curedDisease = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CURED_DISEASE;
    string diseaseDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CURED_DISEASE_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist33 = new List<ColonyAchievementRequirement>();
    requirementChecklist33.Add((ColonyAchievementRequirement) new CureDisease());
    EventReference victorySnapshot28 = new EventReference();
    this.CuredDisease = this.Add(new ColonyAchievement(nameof (CuredDisease), "CURED_DISEASE", curedDisease, diseaseDescription, false, requirementChecklist33, victorySnapshot: victorySnapshot28, icon: "medic"));
    string generatorTuneup = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GENERATOR_TUNEUP;
    string description8 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GENERATOR_TUNEUP_DESCRIPTION, (object) 100);
    List<ColonyAchievementRequirement> requirementChecklist34 = new List<ColonyAchievementRequirement>();
    requirementChecklist34.Add((ColonyAchievementRequirement) new TuneUpGenerator(100f));
    EventReference victorySnapshot29 = new EventReference();
    this.GeneratorTuneup = this.Add(new ColonyAchievement(nameof (GeneratorTuneup), "GENERATOR_TUNEUP", generatorTuneup, description8, false, requirementChecklist34, victorySnapshot: victorySnapshot29, icon: "tune_up_for_what"));
    string clearFow = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAR_FOW;
    string clearFowDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAR_FOW_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist35 = new List<ColonyAchievementRequirement>();
    requirementChecklist35.Add((ColonyAchievementRequirement) new RevealAsteriod(0.8f));
    EventReference victorySnapshot30 = new EventReference();
    this.ClearFOW = this.Add(new ColonyAchievement(nameof (ClearFOW), "CLEAR_FOW", clearFow, clearFowDescription, false, requirementChecklist35, victorySnapshot: victorySnapshot30, icon: "pulling_back_the_veil"));
    string hatchRefinement = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_REFINEMENT;
    string description9 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_REFINEMENT_DESCRIPTION, (object) GameUtil.GetFormattedMass(10000f, massFormat: GameUtil.MetricMassFormat.Tonne));
    List<ColonyAchievementRequirement> requirementChecklist36 = new List<ColonyAchievementRequirement>();
    requirementChecklist36.Add((ColonyAchievementRequirement) new CreaturePoopKGProduction((Tag) "HatchMetal", 10000f));
    EventReference victorySnapshot31 = new EventReference();
    this.HatchRefinement = this.Add(new ColonyAchievement(nameof (HatchRefinement), "HATCH_REFINEMENT", hatchRefinement, description9, false, requirementChecklist36, victorySnapshot: victorySnapshot31, icon: "down_the_hatch"));
    string bunkerDoorDefense = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUNKER_DOOR_DEFENSE;
    string defenseDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUNKER_DOOR_DEFENSE_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist37 = new List<ColonyAchievementRequirement>();
    requirementChecklist37.Add((ColonyAchievementRequirement) new BlockedCometWithBunkerDoor());
    EventReference victorySnapshot32 = new EventReference();
    this.BunkerDoorDefense = this.Add(new ColonyAchievement(nameof (BunkerDoorDefense), "BUNKER_DOOR_DEFENSE", bunkerDoorDefense, defenseDescription, false, requirementChecklist37, victorySnapshot: victorySnapshot32, icon: "Immovable_Object"));
    string idleDuplicants = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.IDLE_DUPLICANTS;
    string duplicantsDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.IDLE_DUPLICANTS_DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist38 = new List<ColonyAchievementRequirement>();
    requirementChecklist38.Add((ColonyAchievementRequirement) new DupesVsSolidTransferArmFetch(1f, 5));
    EventReference victorySnapshot33 = new EventReference();
    this.IdleDuplicants = this.Add(new ColonyAchievement(nameof (IdleDuplicants), "IDLE_DUPLICANTS", idleDuplicants, duplicantsDescription, false, requirementChecklist38, victorySnapshot: victorySnapshot33, icon: "easy_livin"));
    string exosuitCycles = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXOSUIT_CYCLES;
    string description10 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXOSUIT_CYCLES_DESCRIPTION, (object) 10);
    List<ColonyAchievementRequirement> requirementChecklist39 = new List<ColonyAchievementRequirement>();
    requirementChecklist39.Add((ColonyAchievementRequirement) new DupesCompleteChoreInExoSuitForCycles(10));
    EventReference victorySnapshot34 = new EventReference();
    this.ExosuitCycles = this.Add(new ColonyAchievement(nameof (ExosuitCycles), "EXOSUIT_CYCLES", exosuitCycles, description10, false, requirementChecklist39, victorySnapshot: victorySnapshot34, icon: "job_suitability"));
    if (DlcManager.IsExpansion1Active())
    {
      string firstTeleport = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.FIRST_TELEPORT;
      string teleportDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.FIRST_TELEPORT_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist40 = new List<ColonyAchievementRequirement>();
      requirementChecklist40.Add((ColonyAchievementRequirement) new TeleportDuplicant());
      requirementChecklist40.Add((ColonyAchievementRequirement) new DefrostDuplicant());
      string[] expansioN1_1 = DlcManager.EXPANSION1;
      EventReference victorySnapshot35 = new EventReference();
      string[] requiredDlcIds1 = expansioN1_1;
      this.FirstTeleport = this.Add(new ColonyAchievement(nameof (FirstTeleport), "FIRST_TELEPORT", firstTeleport, teleportDescription, false, requirementChecklist40, victorySnapshot: victorySnapshot35, icon: "first_teleport_of_call", requiredDlcIds: requiredDlcIds1, dlcIdFrom: "EXPANSION1_ID"));
      string softLaunch = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SOFT_LAUNCH;
      string launchDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SOFT_LAUNCH_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist41 = new List<ColonyAchievementRequirement>();
      requirementChecklist41.Add((ColonyAchievementRequirement) new BuildALaunchPad());
      string[] expansioN1_2 = DlcManager.EXPANSION1;
      EventReference victorySnapshot36 = new EventReference();
      string[] requiredDlcIds2 = expansioN1_2;
      this.SoftLaunch = this.Add(new ColonyAchievement(nameof (SoftLaunch), "SOFT_LAUNCH", softLaunch, launchDescription, false, requirementChecklist41, victorySnapshot: victorySnapshot36, icon: "soft_launch", requiredDlcIds: requiredDlcIds2, dlcIdFrom: "EXPANSION1_ID"));
      string gmoOk = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GMO_OK;
      string gmoOkDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GMO_OK_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist42 = new List<ColonyAchievementRequirement>();
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed(BasicFabricMaterialPlantConfig.ID));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("BasicSingleHarvestPlant"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("GasGrass"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("MushroomPlant"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("PrickleFlower"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("SaltPlant"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed(SeaLettuceConfig.ID));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("SpiceVine"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("SwampHarvestPlant"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed(SwampLilyConfig.ID));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("WormPlant"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("ColdWheat"));
      requirementChecklist42.Add((ColonyAchievementRequirement) new AnalyzeSeed("BeanPlant"));
      string[] expansioN1_3 = DlcManager.EXPANSION1;
      EventReference victorySnapshot37 = new EventReference();
      string[] requiredDlcIds3 = expansioN1_3;
      this.GMOOK = this.Add(new ColonyAchievement(nameof (GMOOK), "GMO_OK", gmoOk, gmoOkDescription, false, requirementChecklist42, victorySnapshot: victorySnapshot37, icon: "gmo_ok", requiredDlcIds: requiredDlcIds3, dlcIdFrom: "EXPANSION1_ID"));
      string mineTheGap = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MINE_THE_GAP;
      string theGapDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MINE_THE_GAP_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist43 = new List<ColonyAchievementRequirement>();
      requirementChecklist43.Add((ColonyAchievementRequirement) new HarvestAmountFromSpacePOI(1000000f));
      string[] expansioN1_4 = DlcManager.EXPANSION1;
      EventReference victorySnapshot38 = new EventReference();
      string[] requiredDlcIds4 = expansioN1_4;
      this.MineTheGap = this.Add(new ColonyAchievement(nameof (MineTheGap), "MINE_THE_GAP", mineTheGap, theGapDescription, false, requirementChecklist43, victorySnapshot: victorySnapshot38, icon: "mine_the_gap", requiredDlcIds: requiredDlcIds4, dlcIdFrom: "EXPANSION1_ID"));
      string landOnAllWorlds = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.LAND_ON_ALL_WORLDS;
      string worldsDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.LAND_ON_ALL_WORLDS_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist44 = new List<ColonyAchievementRequirement>();
      requirementChecklist44.Add((ColonyAchievementRequirement) new LandOnAllWorlds());
      string[] expansioN1_5 = DlcManager.EXPANSION1;
      EventReference victorySnapshot39 = new EventReference();
      string[] requiredDlcIds5 = expansioN1_5;
      this.LandedOnAllWorlds = this.Add(new ColonyAchievement(nameof (LandedOnAllWorlds), "LANDED_ON_ALL_WORLDS", landOnAllWorlds, worldsDescription, false, requirementChecklist44, victorySnapshot: victorySnapshot39, icon: "land_on_all_worlds", requiredDlcIds: requiredDlcIds5, dlcIdFrom: "EXPANSION1_ID"));
      string radicalTrip = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.RADICAL_TRIP;
      string description11 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.RADICAL_TRIP_DESCRIPTION, (object) 10);
      List<ColonyAchievementRequirement> requirementChecklist45 = new List<ColonyAchievementRequirement>();
      requirementChecklist45.Add((ColonyAchievementRequirement) new RadBoltTravelDistance(10000));
      string[] expansioN1_6 = DlcManager.EXPANSION1;
      EventReference victorySnapshot40 = new EventReference();
      string[] requiredDlcIds6 = expansioN1_6;
      this.RadicalTrip = this.Add(new ColonyAchievement(nameof (RadicalTrip), "RADICAL_TRIP", radicalTrip, description11, false, requirementChecklist45, victorySnapshot: victorySnapshot40, icon: "radical_trip", requiredDlcIds: requiredDlcIds6, dlcIdFrom: "EXPANSION1_ID"));
      string sweeterThanHoney = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SWEETER_THAN_HONEY;
      string honeyDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SWEETER_THAN_HONEY_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist46 = new List<ColonyAchievementRequirement>();
      requirementChecklist46.Add((ColonyAchievementRequirement) new HarvestAHiveWithoutBeingStung());
      string[] expansioN1_7 = DlcManager.EXPANSION1;
      EventReference victorySnapshot41 = new EventReference();
      string[] requiredDlcIds7 = expansioN1_7;
      this.SweeterThanHoney = this.Add(new ColonyAchievement(nameof (SweeterThanHoney), "SWEETER_THAN_HONEY", sweeterThanHoney, honeyDescription, false, requirementChecklist46, victorySnapshot: victorySnapshot41, icon: "sweeter_than_honey", requiredDlcIds: requiredDlcIds7, dlcIdFrom: "EXPANSION1_ID"));
      string surviveInARocket = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_IN_A_ROCKET;
      string description12 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_IN_A_ROCKET_DESCRIPTION, (object) 10, (object) 25);
      List<ColonyAchievementRequirement> requirementChecklist47 = new List<ColonyAchievementRequirement>();
      requirementChecklist47.Add((ColonyAchievementRequirement) new SurviveARocketWithMinimumMorale(25f, 10));
      string[] expansioN1_8 = DlcManager.EXPANSION1;
      EventReference victorySnapshot42 = new EventReference();
      string[] requiredDlcIds8 = expansioN1_8;
      this.SurviveInARocket = this.Add(new ColonyAchievement(nameof (SurviveInARocket), "SURVIVE_IN_A_ROCKET", surviveInARocket, description12, false, requirementChecklist47, victorySnapshot: victorySnapshot42, icon: "survive_a_rocket", requiredDlcIds: requiredDlcIds8, dlcIdFrom: "EXPANSION1_ID"));
      string reactorUsage = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACTOR_USAGE;
      string description13 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACTOR_USAGE_DESCRIPTION, (object) 5);
      List<ColonyAchievementRequirement> requirementChecklist48 = new List<ColonyAchievementRequirement>();
      requirementChecklist48.Add((ColonyAchievementRequirement) new RunReactorForXDays(5));
      string[] expansioN1_9 = DlcManager.EXPANSION1;
      EventReference victorySnapshot43 = new EventReference();
      string[] requiredDlcIds9 = expansioN1_9;
      this.RunAReactor = this.Add(new ColonyAchievement(nameof (RunAReactor), "REACTOR_USAGE", reactorUsage, description13, false, requirementChecklist48, victorySnapshot: victorySnapshot43, icon: "thats_rad", requiredDlcIds: requiredDlcIds9, dlcIdFrom: "EXPANSION1_ID"));
    }
    if (DlcManager.IsContentSubscribed("DLC3_ID"))
    {
      string dataDriven = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.DATA_DRIVEN;
      string drivenDescription = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.DATA_DRIVEN_DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist49 = new List<ColonyAchievementRequirement>();
      requirementChecklist49.Add((ColonyAchievementRequirement) new EfficientDataMiningCheck());
      string[] dlC3_1 = DlcManager.DLC3;
      EventReference victorySnapshot44 = new EventReference();
      string[] requiredDlcIds10 = dlC3_1;
      this.EfficientData = this.Add(new ColonyAchievement(nameof (EfficientData), "EFFICIENT_DATAMINING", dataDriven, drivenDescription, false, requirementChecklist49, victorySnapshot: victorySnapshot44, icon: "efficient_data_mining", requiredDlcIds: requiredDlcIds10, dlcIdFrom: "DLC3_ID"));
      string mvb = (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MVB;
      string description14 = string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MVB_DESCRIPTION, (object) 8);
      List<ColonyAchievementRequirement> requirementChecklist50 = new List<ColonyAchievementRequirement>();
      requirementChecklist50.Add((ColonyAchievementRequirement) new AllTheCircuitsCompleteCheck());
      string[] dlC3_2 = DlcManager.DLC3;
      EventReference victorySnapshot45 = new EventReference();
      string[] requiredDlcIds11 = dlC3_2;
      this.AllTheCircuits = this.Add(new ColonyAchievement(nameof (AllTheCircuits), "ALL_THE_CIRCUITS", mvb, description14, false, requirementChecklist50, victorySnapshot: victorySnapshot45, icon: "all_the_circuits", requiredDlcIds: requiredDlcIds11, dlcIdFrom: "DLC3_ID"));
    }
    if (DlcManager.IsContentSubscribed("DLC4_ID"))
    {
      string name6 = (string) COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.NAME;
      string description15 = (string) COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist51 = new List<ColonyAchievementRequirement>();
      requirementChecklist51.Add((ColonyAchievementRequirement) new DefeatPrehistoricAsteroid());
      string messageTitle5 = (string) COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.MESSAGE_TITLE;
      string messageBody5 = (string) COLONY_ACHIEVEMENTS.ASTEROID_DESTROYED.MESSAGE_BODY;
      EventReference nisGenericSnapshot4 = AudioMixerSnapshots.Get().VictoryNISGenericSnapshot;
      string[] dlC4_1 = DlcManager.DLC4;
      this.AsteroidDestroyed = this.Add(new ColonyAchievement(nameof (AsteroidDestroyed), "ASTEROID_DESTROYED", name6, description15, true, requirementChecklist51, messageTitle5, messageBody5, "DLC4/LargeImpactorDefeatedVideo", "DLC4/LargeImpactorSpacePOIVideo", (Action<KMonoBehaviour>) (a => LargeImpactorDestroyedSequence.Start()), nisGenericSnapshot4, "blast_line_of_defense", dlC4_1, dlcIdFrom: "DLC4_ID", clusterTag: "DemoliorImperative"));
      string name7 = (string) COLONY_ACHIEVEMENTS.ASTEROID_SURVIVED.NAME;
      string description16 = (string) COLONY_ACHIEVEMENTS.ASTEROID_SURVIVED.DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist52 = new List<ColonyAchievementRequirement>();
      requirementChecklist52.Add((ColonyAchievementRequirement) new SurvivedPrehistoricAsteroidImpact(100));
      requirementChecklist52.Add((ColonyAchievementRequirement) new NoDuplicantsCanDie());
      string[] dlC4_2 = DlcManager.DLC4;
      EventReference victorySnapshot46 = new EventReference();
      string[] requiredDlcIds = dlC4_2;
      this.AsteroidSurvived = this.Add(new ColonyAchievement(nameof (AsteroidSurvived), "ASTEROID_SURVIVED", name7, description16, false, requirementChecklist52, victorySnapshot: victorySnapshot46, icon: "life_found_a_way", requiredDlcIds: requiredDlcIds, dlcIdFrom: "DLC4_ID", clusterTag: "DemoliorSurivedAchievement"));
    }
    if (!DlcManager.IsContentSubscribed("DLC5_ID"))
      return;
    string name8 = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.NAME;
    string description17 = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.DESCRIPTION;
    List<ColonyAchievementRequirement> requirementChecklist53 = new List<ColonyAchievementRequirement>();
    requirementChecklist53.Add((ColonyAchievementRequirement) new Database.MinnowRecruited(MinnowImperativePOIStates.MinnowPOIIdentity.POI_A));
    requirementChecklist53.Add((ColonyAchievementRequirement) new Database.MinnowRecruited(MinnowImperativePOIStates.MinnowPOIIdentity.POI_B));
    requirementChecklist53.Add((ColonyAchievementRequirement) new Database.MinnowRecruited(MinnowImperativePOIStates.MinnowPOIIdentity.POI_C));
    string messageTitle6 = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.MESSAGE_TITLE;
    string messageBody6 = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.MESSAGE_BODY;
    Action<KMonoBehaviour> VictorySequence6 = new Action<KMonoBehaviour>(FindingMinnowCompleteSequence.Start);
    EventReference nisGenericSnapshot5 = AudioMixerSnapshots.Get().VictoryNISGenericSnapshot;
    string[] dlC5 = DlcManager.DLC5;
    this.MinnowRecruited = this.Add(new ColonyAchievement(nameof (MinnowRecruited), "MINNOW_RECRUITED", name8, description17, true, requirementChecklist53, messageTitle6, messageBody6, "victoryShorts/Stay", "DLC5/aquatic_loop", VictorySequence6, nisGenericSnapshot5, "finding_minnow", dlC5, dlcIdFrom: "DLC5_ID", clusterTag: "MinnowRecruitedAchievement"));
  }
}
