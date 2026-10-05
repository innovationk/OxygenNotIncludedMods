// Decompiled with JetBrains decompiler
// Type: Database.Techs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Database;

public class Techs : ResourceSet<Tech>
{
  private readonly List<List<Tuple<string, float>>> TECH_TIERS;

  public Techs(ResourceSet parent)
    : base(nameof (Techs), parent)
  {
    if (!DlcManager.IsExpansion1Active())
      this.TECH_TIERS = new List<List<Tuple<string, float>>>()
      {
        new List<Tuple<string, float>>(),
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 15f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 20f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 30f),
          new Tuple<string, float>("advanced", 20f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 35f),
          new Tuple<string, float>("advanced", 30f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 40f),
          new Tuple<string, float>("advanced", 50f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 50f),
          new Tuple<string, float>("advanced", 70f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 70f),
          new Tuple<string, float>("advanced", 100f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 70f),
          new Tuple<string, float>("advanced", 100f),
          new Tuple<string, float>("space", 200f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 70f),
          new Tuple<string, float>("advanced", 100f),
          new Tuple<string, float>("space", 400f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 70f),
          new Tuple<string, float>("advanced", 100f),
          new Tuple<string, float>("space", 800f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 70f),
          new Tuple<string, float>("advanced", 100f),
          new Tuple<string, float>("space", 1600f)
        }
      };
    else
      this.TECH_TIERS = new List<List<Tuple<string, float>>>()
      {
        new List<Tuple<string, float>>(),
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 15f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 20f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 30f),
          new Tuple<string, float>("advanced", 20f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 35f),
          new Tuple<string, float>("advanced", 30f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 40f),
          new Tuple<string, float>("advanced", 50f),
          new Tuple<string, float>("orbital", 0.0f),
          new Tuple<string, float>("nuclear", 20f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 50f),
          new Tuple<string, float>("advanced", 70f),
          new Tuple<string, float>("orbital", 30f),
          new Tuple<string, float>("nuclear", 40f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 70f),
          new Tuple<string, float>("advanced", 100f),
          new Tuple<string, float>("orbital", 250f),
          new Tuple<string, float>("nuclear", 370f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 100f),
          new Tuple<string, float>("advanced", 130f),
          new Tuple<string, float>("orbital", 400f),
          new Tuple<string, float>("nuclear", 435f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 100f),
          new Tuple<string, float>("advanced", 130f),
          new Tuple<string, float>("orbital", 600f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 100f),
          new Tuple<string, float>("advanced", 130f),
          new Tuple<string, float>("orbital", 800f)
        },
        new List<Tuple<string, float>>()
        {
          new Tuple<string, float>("basic", 100f),
          new Tuple<string, float>("advanced", 130f),
          new Tuple<string, float>("orbital", 1600f)
        }
      };
  }

  public void Init()
  {
    new Tech("FarmingTech", new List<string>()
    {
      "AlgaeHabitat",
      "PlanterBox",
      "RationBox",
      "Compost"
    }, this).AddSearchTerms((string) SEARCH_TERMS.FARM);
    new Tech("FineDining", new List<string>()
    {
      "CookingStation",
      "EggCracker",
      "DiningTable",
      "FarmTile"
    }, this).AddSearchTerms((string) SEARCH_TERMS.FOOD);
    new Tech("FoodRepurposing", new List<string>()
    {
      "Juicer",
      "SpiceGrinder",
      "MilkPress",
      "Smoker"
    }, this).AddSearchTerms((string) SEARCH_TERMS.FOOD);
    new Tech("FinerDining", new List<string>()
    {
      "GourmetCookingStation",
      "FoodDehydrator",
      "FoodRehydrator",
      "Deepfryer",
      "SushiBar"
    }, this).AddSearchTerms((string) SEARCH_TERMS.FOOD);
    Tech tech1 = new Tech("Agriculture", new List<string>()
    {
      "FarmStation",
      "FertilizerMaker",
      "Refrigerator",
      "HydroponicFarm",
      "ParkSign",
      "RadiationLight",
      "LargeBackwallFarm",
      "WideFarmTile"
    }, this);
    tech1.AddSearchTerms((string) SEARCH_TERMS.FARM);
    tech1.AddSearchTerms((string) SEARCH_TERMS.FRIDGE);
    Tech tech2 = new Tech("Ranching", new List<string>()
    {
      "RanchStation",
      "CreatureDeliveryPoint",
      "ShearingStation",
      "CreatureFeeder",
      "FishFeeder",
      "FishDeliveryPoint",
      "FishPickUp",
      "CritterPickUp",
      "CritterDropOff"
    }, this);
    tech2.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    tech2.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    tech2.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    Tech tech3 = new Tech("AnimalControl", new List<string>()
    {
      "CreatureAirTrap",
      "CreatureGroundTrap",
      "WaterTrap",
      "EggIncubator",
      LogicCritterCountSensorConfig.ID,
      "UnderwaterRanchStation",
      "UnderwaterShearingStation"
    }, this);
    tech3.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    tech3.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    tech3.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    Tech tech4 = new Tech("AnimalComfort", new List<string>()
    {
      "CritterCondo",
      "UnderwaterCritterCondo",
      "AirBorneCritterCondo"
    }, this);
    tech4.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    tech4.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    Tech tech5 = new Tech("DairyOperation", new List<string>()
    {
      "UnderwaterMilkFeeder",
      "MilkFeeder",
      "MilkFatSeparator",
      "MilkingStation",
      "UnderwaterMilkingStation"
    }, this);
    tech5.AddSearchTerms((string) SEARCH_TERMS.CRITTER);
    tech5.AddSearchTerms((string) SEARCH_TERMS.RANCHING);
    new Tech("ImprovedOxygen", new List<string>()
    {
      "Electrolyzer",
      "RustDeoxidizer"
    }, this).AddSearchTerms((string) SEARCH_TERMS.OXYGEN);
    Tech tech6 = new Tech("GasPiping", new List<string>()
    {
      "GasConduit",
      "GasConduitBridge",
      "GasPump",
      "GasVent"
    }, this);
    Tech tech7 = new Tech("ImprovedGasPiping", new List<string>()
    {
      "InsulatedGasConduit",
      LogicPressureSensorGasConfig.ID,
      "GasLogicValve",
      "GasVentHighPressure"
    }, this);
    new Tech("SpaceGas", new List<string>()
    {
      "CO2Engine",
      "ModularLaunchpadPortGas",
      "ModularLaunchpadPortGasUnloader",
      "GasCargoBaySmall"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    Tech tech8 = new Tech("PressureManagement", new List<string>()
    {
      "LiquidValve",
      "GasValve",
      "GasPermeableMembrane",
      "ManualPressureDoor"
    }, this);
    new Tech("DirectedAirStreams", new List<string>()
    {
      "AirFilter",
      "CO2Scrubber",
      "PressureDoor",
      "UnderwaterBreathingStation"
    }, this).AddSearchTerms((string) SEARCH_TERMS.FILTER);
    new Tech("LiquidFiltering", new List<string>()
    {
      "OreScrubber",
      "Desalinator"
    }, this).AddSearchTerms((string) SEARCH_TERMS.FILTER);
    new Tech("MedicineI", new List<string>()
    {
      "Apothecary",
      "LubricationStick"
    }, this).AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    new Tech("MedicineII", new List<string>()
    {
      "DoctorStation",
      "HandSanitizer"
    }, this).AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    new Tech("MedicineIII", new List<string>()
    {
      GasConduitDiseaseSensorConfig.ID,
      LiquidConduitDiseaseSensorConfig.ID,
      LogicDiseaseSensorConfig.ID
    }, this).AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    new Tech("MedicineIV", new List<string>()
    {
      "AdvancedDoctorStation",
      "AdvancedApothecary",
      "HotTub",
      LogicRadiationSensorConfig.ID
    }, this).AddSearchTerms((string) SEARCH_TERMS.MEDICINE);
    Tech tech9 = new Tech("LiquidPiping", new List<string>()
    {
      "LiquidConduit",
      "LiquidConduitBridge",
      "LiquidPump",
      "LiquidVent"
    }, this);
    Tech tech10 = new Tech("ImprovedLiquidPiping", new List<string>()
    {
      "InsulatedLiquidConduit",
      LogicPressureSensorLiquidConfig.ID,
      "LiquidLogicValve",
      "LiquidConduitPreferentialFlow",
      "LiquidConduitOverflow",
      "LiquidReservoir",
      "PlasticGasket"
    }, this);
    Tech tech11 = new Tech("PrecisionPlumbing", new List<string>()
    {
      "EspressoMachine",
      "LiquidFuelTankCluster",
      "MercuryCeilingLight"
    }, this);
    new Tech("SanitationSciences", new List<string>()
    {
      "FlushToilet",
      "WashSink",
      ShowerConfig.ID,
      "MeshTile",
      "GunkEmptier"
    }, this).AddSearchTerms((string) SEARCH_TERMS.TOILET);
    Tech tech12 = new Tech("FlowRedirection", new List<string>()
    {
      "MechanicalSurfboard",
      "LiquidBottler",
      "ModularLaunchpadPortLiquid",
      "ModularLaunchpadPortLiquidUnloader",
      "LiquidCargoBaySmall"
    }, this);
    Tech tech13 = new Tech("LiquidDistribution", new List<string>()
    {
      "BottleEmptierConduitLiquid",
      "RocketInteriorLiquidInput",
      "RocketInteriorLiquidOutput",
      "WallToilet"
    }, this);
    Tech tech14 = new Tech("AdvancedSanitation", new List<string>()
    {
      "DecontaminationShower"
    }, this);
    new Tech("AdvancedFiltration", new List<string>()
    {
      "GasFilter",
      "LiquidFilter",
      "SludgePress",
      "OilChanger"
    }, this).AddSearchTerms((string) SEARCH_TERMS.FILTER);
    Tech tech15 = new Tech("Distillation", new List<string>()
    {
      "AlgaeDistillery",
      "EthanolDistillery",
      "WaterPurifier"
    }, this);
    tech15.AddSearchTerms((string) SEARCH_TERMS.WATER);
    Tech tech16 = new Tech("AdvancedDistillation", new List<string>()
    {
      "ChemicalRefinery"
    }, this);
    tech15.AddSearchTerms((string) SEARCH_TERMS.POWER);
    new Tech("Catalytics", new List<string>()
    {
      "OxyliteRefinery",
      "Chlorinator",
      "SupermaterialRefinery",
      "SUPER_LIQUIDS",
      "SodaFountain",
      "GasCargoBayCluster"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("AdvancedResourceExtraction", new List<string>()
    {
      "NoseconeHarvest"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    Tech tech17 = new Tech("PowerRegulation", new List<string>()
    {
      "BatteryMedium",
      SwitchConfig.ID,
      "WireBridge",
      "SmallElectrobankDischarger"
    }, this);
    tech17.AddSearchTerms((string) SEARCH_TERMS.POWER);
    tech17.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    tech17.AddSearchTerms((string) SEARCH_TERMS.WIRE);
    Tech tech18 = new Tech("AdvancedPowerRegulation", new List<string>()
    {
      "HighWattageWire",
      "WireBridgeHighWattage",
      "HydrogenGenerator",
      LogicPowerRelayConfig.ID,
      "PowerTransformerSmall",
      LogicWattageSensorConfig.ID
    }, this);
    tech18.AddSearchTerms((string) SEARCH_TERMS.POWER);
    tech18.AddSearchTerms((string) SEARCH_TERMS.WIRE);
    tech18.AddSearchTerms((string) SEARCH_TERMS.GENERATOR);
    Tech tech19 = new Tech("PrettyGoodConductors", new List<string>()
    {
      "WireRefined",
      "WireRefinedBridge",
      "WireRefinedHighWattage",
      "WireRefinedBridgeHighWattage",
      "PowerTransformer",
      "LargeElectrobankDischarger"
    }, this);
    tech19.AddSearchTerms((string) SEARCH_TERMS.WIRE);
    tech19.AddSearchTerms((string) SEARCH_TERMS.POWER);
    Tech tech20 = new Tech("RenewableEnergy", new List<string>()
    {
      "SteamTurbine2",
      "SolarPanel",
      "Sauna",
      "SteamEngineCluster",
      "WireRubber",
      "WireRubberBridge"
    }, this);
    tech20.AddSearchTerms((string) SEARCH_TERMS.POWER);
    tech20.AddSearchTerms((string) SEARCH_TERMS.STEAM);
    Tech tech21 = new Tech("Combustion", new List<string>()
    {
      "Generator",
      "WoodGasGenerator",
      "PeatGenerator",
      "ReefGenerator"
    }, this);
    tech21.AddSearchTerms((string) SEARCH_TERMS.POWER);
    tech21.AddSearchTerms((string) SEARCH_TERMS.GENERATOR);
    Tech tech22 = new Tech("ImprovedCombustion", new List<string>()
    {
      "MethaneGenerator",
      "OilRefinery",
      "PetroleumGenerator"
    }, this);
    tech22.AddSearchTerms((string) SEARCH_TERMS.POWER);
    tech22.AddSearchTerms((string) SEARCH_TERMS.GENERATOR);
    Tech tech23 = new Tech("InteriorDecor", new List<string>()
    {
      "FlowerVase",
      "FloorLamp",
      "CeilingLight"
    }, this);
    tech23.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    tech23.AddSearchTerms((string) SEARCH_TERMS.ARTWORK);
    Tech tech24 = new Tech("Artistry", new List<string>()
    {
      "WoodenDoor",
      "FlowerVaseWall",
      "FlowerVaseHanging",
      "CornerMoulding",
      "CrownMoulding",
      "ItemPedestal",
      "SmallSculpture",
      "IceSculpture"
    }, this);
    tech24.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    tech24.AddSearchTerms((string) SEARCH_TERMS.ARTWORK);
    new Tech("Clothing", new List<string>()
    {
      "ClothingFabricator",
      "CarpetTile",
      "ExteriorWall"
    }, this).AddSearchTerms((string) SEARCH_TERMS.TILE);
    Tech tech25 = new Tech("Acoustics", new List<string>()
    {
      "BatterySmart",
      "Phonobox",
      "PowerControlStation",
      "ElectrobankCharger",
      "Electrobank"
    }, this);
    tech25.AddSearchTerms((string) SEARCH_TERMS.POWER);
    tech25.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    Tech tech26 = new Tech("SpacePower", new List<string>()
    {
      "BatteryModule",
      "SolarPanelModule",
      "RocketInteriorPowerPlug"
    }, this);
    tech26.AddSearchTerms((string) SEARCH_TERMS.POWER);
    tech26.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    tech26.AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    Tech tech27 = new Tech("NuclearRefinement", new List<string>()
    {
      "NuclearReactor",
      "UraniumCentrifuge",
      "SelfChargingElectrobank"
    }, this);
    tech27.AddSearchTerms((string) SEARCH_TERMS.POWER);
    tech27.AddSearchTerms((string) SEARCH_TERMS.BATTERY);
    Tech tech28 = new Tech("FineArt", new List<string>()
    {
      "Canvas",
      "Sculpture",
      "Shelf"
    }, this);
    tech28.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    tech28.AddSearchTerms((string) SEARCH_TERMS.ARTWORK);
    Tech tech29 = new Tech("EnvironmentalAppreciation", new List<string>()
    {
      "BeachChair"
    }, this);
    tech29.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    if (DlcManager.IsContentSubscribed("DLC4_ID"))
    {
      tech29.AddSearchTerms((string) SEARCH_TERMS.ARTWORK);
      tech29.AddSearchTerms((string) SEARCH_TERMS.DINOSAUR);
    }
    Tech tech30 = new Tech("Luxury", new List<string>()
    {
      "LuxuryBed",
      "LadderFast",
      "PlasticTile",
      "ClothingAlterationStation",
      "WoodTile",
      "MultiMinionDiningTable"
    }, this);
    tech30.AddSearchTerms((string) SEARCH_TERMS.TILE);
    tech30.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    Tech tech31 = new Tech("RefractiveDecor", new List<string>()
    {
      "CanvasWide",
      "MetalSculpture",
      "WoodSculpture"
    }, this);
    tech31.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    tech31.AddSearchTerms((string) SEARCH_TERMS.ARTWORK);
    Tech tech32 = new Tech("GlassFurnishings", new List<string>()
    {
      "GlassTile",
      "FlowerVaseHangingFancy",
      "SunLamp",
      "GlassExteriorWall",
      "GlassCeilingLight"
    }, this);
    Tech tech33 = new Tech("Screens", new List<string>()
    {
      PixelPackConfig.ID
    }, this);
    Tech tech34 = new Tech("RenaissanceArt", new List<string>()
    {
      "CanvasTall",
      "MarbleSculpture",
      "FossilSculpture",
      "CeilingFossilSculpture"
    }, this);
    tech34.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    tech34.AddSearchTerms((string) SEARCH_TERMS.ARTWORK);
    Tech tech35 = new Tech("Plastics", new List<string>()
    {
      "Polymerizer",
      "OilWellCap"
    }, this);
    Tech tech36 = new Tech("ValveMiniaturization", new List<string>()
    {
      "LiquidMiniPump",
      "GasMiniPump",
      "MiniFridge"
    }, this);
    new Tech("HydrocarbonPropulsion", new List<string>()
    {
      "KeroseneEngineClusterSmall",
      "MissionControlCluster"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("BetterHydroCarbonPropulsion", new List<string>()
    {
      "KeroseneEngineCluster",
      "BiodieselEngineCluster"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("CryoFuelPropulsion", new List<string>()
    {
      "HydrogenEngineCluster",
      "OxidizerTankLiquidCluster"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    Tech tech37 = new Tech("Suits", new List<string>()
    {
      "SuitsOverlay",
      "AtmoSuit",
      "SuitFabricator",
      "SuitMarker",
      "SuitLocker"
    }, this);
    Tech tech38 = new Tech("Jobs", new List<string>()
    {
      "WaterCooler",
      "CraftingTable",
      "DisposableElectrobank_RawMetal",
      "Campfire"
    }, this);
    Tech tech39 = new Tech("AdvancedResearch", new List<string>()
    {
      "BetaResearchPoint",
      "AdvancedResearchCenter",
      "ResetSkillsStation",
      "ClusterTelescope",
      "ExobaseHeadquarters",
      "AdvancedCraftingTable"
    }, this);
    new Tech("SpaceProgram", new List<string>()
    {
      "LaunchPad",
      "HabitatModuleSmall",
      "OrbitalCargoModule",
      RocketControlStationConfig.ID
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("CrashPlan", new List<string>()
    {
      "OrbitalResearchPoint",
      "PioneerModule",
      "OrbitalResearchCenter",
      "DLC1CosmicResearchCenter"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("DurableLifeSupport", new List<string>()
    {
      "NoseconeBasic",
      "HabitatModuleMedium",
      "ArtifactAnalysisStation",
      "ArtifactCargoBay",
      "SpecialCargoBayCluster"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    Tech tech40 = new Tech("NuclearResearch", new List<string>()
    {
      "DeltaResearchPoint",
      "NuclearResearchCenter",
      "ManualHighEnergyParticleSpawner",
      "DisposableElectrobank_UraniumOre"
    }, this);
    Tech tech41 = new Tech("AdvancedNuclearResearch", new List<string>()
    {
      "HighEnergyParticleSpawner",
      "HighEnergyParticleRedirector",
      "HEPBridgeTile"
    }, this);
    Tech tech42 = new Tech("NuclearStorage", new List<string>()
    {
      "HEPBattery"
    }, this);
    new Tech("NuclearPropulsion", new List<string>()
    {
      "HEPEngine"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("NotificationSystems", new List<string>()
    {
      LogicHammerConfig.ID,
      LogicAlarmConfig.ID,
      "Telephone"
    }, this).AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    new Tech("ArtificialFriends", new List<string>()
    {
      "SweepBotStation",
      "ScoutModule",
      "FetchDrone",
      "OverclockedBoosters"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    Tech tech43 = new Tech("BasicRefinement", new List<string>()
    {
      "RockCrusher",
      "Kiln"
    }, this);
    Tech tech44 = new Tech("RefinedObjects", new List<string>()
    {
      "FabricatedWoodMaker",
      "FirePole",
      "ThermalBlock",
      LadderBedConfig.ID
    }, this);
    Tech tech45 = new Tech("Smelting", new List<string>()
    {
      "MetalRefinery",
      "MetalTile",
      "RubberMaker",
      RubberBootsConfig.ID,
      "DrySuit"
    }, this);
    new Tech("HighTempForging", new List<string>()
    {
      "GlassForge",
      "BunkerTile",
      "BunkerDoor",
      "GeoTuner"
    }, this).AddSearchTerms((string) SEARCH_TERMS.GLASS);
    Tech tech46 = new Tech("HighPressureForging", new List<string>()
    {
      "DiamondPress"
    }, this);
    Tech tech47 = new Tech("RadiationProtection", new List<string>()
    {
      "LeadSuit",
      "LeadSuitMarker",
      "LeadSuitLocker",
      LogicHEPSensorConfig.ID
    }, this);
    Tech tech48 = new Tech("TemperatureModulation", new List<string>()
    {
      "InsulatedDoor",
      "LiquidCooledFan",
      "IceCooledFan",
      "IceMachine",
      "IceKettle",
      "InsulationTile",
      "SpaceHeater"
    }, this);
    Tech tech49 = new Tech("HVAC", new List<string>()
    {
      "AirConditioner",
      LogicTemperatureSensorConfig.ID,
      GasConduitTemperatureSensorConfig.ID,
      GasConduitElementSensorConfig.ID,
      "GasConduitRadiant",
      "GasReservoir",
      "GasLimitValve"
    }, this);
    Tech tech50 = new Tech("LiquidTemperature", new List<string>()
    {
      "LiquidConduitRadiant",
      "LiquidConditioner",
      LiquidConduitTemperatureSensorConfig.ID,
      LiquidConduitElementSensorConfig.ID,
      "LiquidHeater",
      "LiquidLimitValve",
      "ContactConductivePipeBridge",
      "RubberTile"
    }, this);
    new Tech("LogicControl", new List<string>()
    {
      "AutomationOverlay",
      LogicSwitchConfig.ID,
      "LogicWire",
      "LogicWireBridge",
      "LogicDuplicantSensor"
    }, this).AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    new Tech("GenericSensors", new List<string>()
    {
      "FloorSwitch",
      LogicElementSensorGasConfig.ID,
      LogicElementSensorLiquidConfig.ID,
      "LogicGateNOT",
      LogicTimeOfDaySensorConfig.ID,
      LogicTimerSensorConfig.ID,
      LogicLightSensorConfig.ID,
      LogicClusterLocationSensorConfig.ID
    }, this).AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    new Tech("LogicCircuits", new List<string>()
    {
      "LogicGateAND",
      "LogicGateOR",
      "LogicGateBUFFER",
      "LogicGateFILTER"
    }, this).AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    new Tech("ParallelAutomation", new List<string>()
    {
      "LogicRibbon",
      "LogicRibbonBridge",
      LogicRibbonWriterConfig.ID,
      LogicRibbonReaderConfig.ID
    }, this).AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    Tech tech51 = new Tech("DupeTrafficControl", new List<string>()
    {
      LogicCounterConfig.ID,
      LogicMemoryConfig.ID,
      "LogicGateXOR",
      "ArcadeMachine",
      "Checkpoint",
      "CosmicResearchCenter"
    }, this);
    tech51.AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    tech51.AddSearchTerms((string) SEARCH_TERMS.RESEARCH);
    tech51.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    new Tech("Multiplexing", new List<string>()
    {
      "LogicGateMultiplexer",
      "LogicGateDemultiplexer"
    }, this).AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    new Tech("SkyDetectors", new List<string>()
    {
      CometDetectorConfig.ID,
      "Telescope",
      "ResearchClusterModule",
      "ClusterTelescopeEnclosed",
      "AstronautTrainingCenter"
    }, this).AddSearchTerms((string) SEARCH_TERMS.RESEARCH);
    Tech tech52 = new Tech("Missiles", new List<string>()
    {
      "MissileFabricator",
      "MissileLauncher"
    }, this);
    new Tech("TravelTubes", new List<string>()
    {
      "TravelTubeEntrance",
      "TravelTube",
      "TravelTubeWallBridge",
      "VerticalWindTunnel"
    }, this).AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    new Tech("SmartStorage", new List<string>()
    {
      "ConveyorOverlay",
      "SolidTransferArm",
      "StorageLockerSmart",
      "ObjectDispenser"
    }, this).AddSearchTerms((string) SEARCH_TERMS.STORAGE);
    Tech tech53 = new Tech("SolidManagement", new List<string>()
    {
      "SolidFilter",
      SolidConduitTemperatureSensorConfig.ID,
      SolidConduitElementSensorConfig.ID,
      SolidConduitDiseaseSensorConfig.ID,
      "StorageTile",
      "CargoBayCluster"
    }, this);
    tech53.AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    tech53.AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    tech53.AddSearchTerms((string) SEARCH_TERMS.STORAGE);
    new Tech("HighVelocityTransport", new List<string>()
    {
      "RailGun",
      "LandingBeacon"
    }, this).AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    Tech tech54 = new Tech("BasicRocketry", new List<string>()
    {
      "CommandModule",
      "SteamEngine",
      "ResearchModule",
      "Gantry"
    }, this);
    tech54.AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    tech54.AddSearchTerms((string) SEARCH_TERMS.RESEARCH);
    tech54.AddSearchTerms((string) SEARCH_TERMS.STEAM);
    new Tech("CargoI", new List<string>() { "CargoBay" }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("CargoII", new List<string>()
    {
      "LiquidCargoBay",
      "GasCargoBay"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("CargoIII", new List<string>()
    {
      "TouristModule",
      "SpecialCargoBay"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("EnginesI", new List<string>()
    {
      "SolidBooster",
      "MissionControl"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("EnginesII", new List<string>()
    {
      "KeroseneEngine",
      "BiodieselEngine",
      "LiquidFuelTank",
      "OxidizerTank"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("EnginesIII", new List<string>()
    {
      "OxidizerTankLiquid",
      "OxidizerTankCluster",
      "HydrogenEngine"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    Tech tech55 = new Tech("Jetpacks", new List<string>()
    {
      "JetSuit",
      "JetSuitMarker",
      "JetSuitLocker",
      "LiquidCargoBayCluster"
    }, this);
    tech55.AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    tech55.AddSearchTerms((string) SEARCH_TERMS.MISSILE);
    new Tech("SolidTransport", new List<string>()
    {
      "SolidConduitInbox",
      "SolidConduit",
      "SolidConduitBridge",
      "SolidVent",
      "SolidCargoBaySmall",
      "ModularLaunchpadPortSolid",
      "ModularLaunchpadPortSolidUnloader",
      "ModularLaunchpadPortBridge"
    }, this).AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    Tech tech56 = new Tech("Monuments", new List<string>()
    {
      "MonumentBottom",
      "MonumentMiddle",
      "MonumentTop"
    }, this);
    tech56.AddSearchTerms((string) SEARCH_TERMS.ARTWORK);
    tech56.AddSearchTerms((string) SEARCH_TERMS.MORALE);
    Tech tech57 = new Tech("SolidSpace", new List<string>()
    {
      "SolidLogicValve",
      "SolidConduitOutbox",
      "SolidLimitValve",
      "RocketInteriorSolidInput",
      "RocketInteriorSolidOutput"
    }, this);
    tech57.AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    tech57.AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    tech57.AddSearchTerms((string) SEARCH_TERMS.TRANSPORT);
    new Tech("RoboticTools", new List<string>()
    {
      "AutoMiner",
      "RailGunPayloadOpener",
      "RoboPilotModule",
      "UnderwaterVentDrill"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROBOT);
    new Tech("PortableGasses", new List<string>()
    {
      "GasBottler",
      "BottleEmptierGas",
      "OxygenMask",
      "OxygenMaskLocker",
      "OxygenMaskMarker",
      "Oxysconce"
    }, this).AddSearchTerms((string) SEARCH_TERMS.OXYGEN);
    new Tech("GasDistribution", new List<string>()
    {
      "BottleEmptierConduitGas",
      "RocketInteriorGasInput",
      "RocketInteriorGasOutput",
      "OxidizerTankCluster"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    this.InitBaseGameOnly();
    this.InitExpansion1();
  }

  private void InitBaseGameOnly()
  {
    if (DlcManager.IsExpansion1Active() || !DlcManager.IsContentSubscribed("DLC3_ID"))
      return;
    new Tech("DataScienceBaseGame", new List<string>()
    {
      "DataMiner",
      RemoteWorkerDockConfig.ID,
      RemoteWorkTerminalConfig.ID,
      "RoboPilotCommandModule"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROBOT);
  }

  private void InitExpansion1()
  {
    if (!DlcManager.IsExpansion1Active())
      return;
    this.Get("HighTempForging").AddUnlockedItemIDs("Gantry");
    new Tech("Bioengineering", new List<string>()
    {
      "GeneticAnalysisStation"
    }, this).AddSearchTerms((string) SEARCH_TERMS.RESEARCH);
    new Tech("SpaceCombustion", new List<string>()
    {
      "SugarEngine",
      "SmallOxidizerTank"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("HighVelocityDestruction", new List<string>()
    {
      "NoseconeHarvest"
    }, this).AddSearchTerms((string) SEARCH_TERMS.ROCKET);
    new Tech("AdvancedScanners", new List<string>()
    {
      "ScannerModule",
      "LogicInterasteroidSender",
      "LogicInterasteroidReceiver"
    }, this).AddSearchTerms((string) SEARCH_TERMS.AUTOMATION);
    if (!DlcManager.IsContentSubscribed("DLC3_ID"))
      return;
    Tech tech = new Tech("DataScience", new List<string>()
    {
      "DataMiner",
      RemoteWorkerDockConfig.ID,
      RemoteWorkTerminalConfig.ID
    }, this);
  }

  public void PostProcess()
  {
    foreach (Tech resource in this.resources)
    {
      List<TechItem> techItemList = new List<TechItem>();
      foreach (string unlockedItemId in resource.unlockedItemIDs)
      {
        TechItem techItem = Db.Get().TechItems.TryGet(unlockedItemId);
        if (techItem != null)
          techItemList.Add(techItem);
      }
      resource.unlockedItems = techItemList;
    }
  }

  public void Load(TextAsset tree_file)
  {
    ResourceTreeLoader<ResourceTreeNode> resourceTreeLoader = new ResourceTreeLoader<ResourceTreeNode>(tree_file);
    List<TechTreeTitle> techTreeTitleList = new List<TechTreeTitle>();
    for (int idx = 0; idx < Db.Get().TechTreeTitles.Count; ++idx)
      techTreeTitleList.Add(Db.Get().TechTreeTitles[idx]);
    techTreeTitleList.Sort((Comparison<TechTreeTitle>) ((a, b) => a.center.y.CompareTo(b.center.y)));
    foreach (ResourceTreeNode node in (ResourceLoader<ResourceTreeNode>) resourceTreeLoader)
    {
      if (!string.Equals(node.Id.Substring(0, 1), "_"))
      {
        Tech tech1 = this.TryGet(node.Id);
        if (tech1 != null)
        {
          string categoryID1 = "";
          for (int index = 0; index < techTreeTitleList.Count; ++index)
          {
            if ((double) techTreeTitleList[index].center.y >= (double) node.center.y)
            {
              categoryID1 = techTreeTitleList[index].Id;
              break;
            }
          }
          tech1.SetNode(node, categoryID1);
          foreach (ResourceTreeNode reference in node.references)
          {
            Tech tech2 = this.TryGet(reference.Id);
            if (tech2 != null)
            {
              string categoryID2 = "";
              for (int index = 0; index < techTreeTitleList.Count; ++index)
              {
                if ((double) techTreeTitleList[index].center.y >= (double) node.center.y)
                {
                  categoryID2 = techTreeTitleList[index].Id;
                  break;
                }
              }
              tech2.SetNode(reference, categoryID2);
              tech2.requiredTech.Add(tech1);
              tech1.unlockedTech.Add(tech2);
            }
          }
        }
      }
    }
    foreach (Tech resource in this.resources)
    {
      resource.tier = Techs.GetTier(resource);
      foreach (Tuple<string, float> tuple in this.TECH_TIERS[resource.tier])
      {
        if (!resource.costsByResearchTypeID.ContainsKey(tuple.first))
          resource.costsByResearchTypeID.Add(tuple.first, tuple.second);
      }
    }
    for (int idx = this.Count - 1; idx >= 0; --idx)
    {
      if (!((Tech) this.GetResource(idx)).FoundNode)
        this.Remove(this.GetResource(idx));
    }
  }

  public static int GetTier(Tech tech)
  {
    if (tech == null)
      return 0;
    int val1 = 0;
    foreach (Tech tech1 in tech.requiredTech)
      val1 = Math.Max(val1, Techs.GetTier(tech1));
    return val1 + 1;
  }

  private void AddPrerequisite(Tech tech, string prerequisite_name)
  {
    Tech tech1 = this.TryGet(prerequisite_name);
    if (tech1 == null)
      return;
    tech.requiredTech.Add(tech1);
    tech1.unlockedTech.Add(tech);
  }

  public Tech TryGetTechForTechItem(string itemId)
  {
    for (int idx = 0; idx < this.Count; ++idx)
    {
      Tech resource = (Tech) this.GetResource(idx);
      if (resource.unlockedItemIDs.Find((Predicate<string>) (candidateItemId => candidateItemId == itemId)) != null)
        return resource;
    }
    return (Tech) null;
  }

  public bool IsTechItemComplete(string id)
  {
    foreach (Tech resource in this.resources)
    {
      foreach (Resource unlockedItem in resource.unlockedItems)
      {
        if (unlockedItem.Id == id)
          return resource.IsComplete();
      }
    }
    return true;
  }
}
