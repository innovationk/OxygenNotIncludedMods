// Decompiled with JetBrains decompiler
// Type: GameTags
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
public class GameTags
{
  public static readonly Tag DeprecatedContent = TagManager.Create(nameof (DeprecatedContent));
  public static readonly Tag Any = TagManager.Create(nameof (Any));
  public static readonly Tag SpawnsInWorld = TagManager.Create(nameof (SpawnsInWorld));
  public static readonly Tag Experimental = TagManager.Create(nameof (Experimental));
  public static readonly Tag Gravitas = TagManager.Create(nameof (Gravitas));
  public static readonly Tag Miscellaneous = TagManager.Create(nameof (Miscellaneous));
  public static readonly Tag Specimen = TagManager.Create(nameof (Specimen));
  public static readonly Tag Seed = TagManager.Create(nameof (Seed));
  public static readonly Tag Dehydrated = TagManager.Create(nameof (Dehydrated));
  public static readonly Tag Rehydrated = TagManager.Create(nameof (Rehydrated));
  public static readonly Tag Edible = TagManager.Create(nameof (Edible));
  public static readonly Tag CookingIngredient = TagManager.Create(nameof (CookingIngredient));
  public static readonly Tag Medicine = TagManager.Create(nameof (Medicine));
  public static readonly Tag MedicalSupplies = TagManager.Create(nameof (MedicalSupplies));
  public static readonly Tag Plant = TagManager.Create(nameof (Plant));
  public static readonly Tag FibrousPlant = TagManager.Create(nameof (FibrousPlant));
  public static readonly Tag PlantBranch = TagManager.Create(nameof (PlantBranch));
  public static readonly Tag GrowingPlant = TagManager.Create(nameof (GrowingPlant));
  public static readonly Tag FullyGrown = TagManager.Create(nameof (FullyGrown));
  public static readonly Tag PlantedOnFloorVessel = TagManager.Create(nameof (PlantedOnFloorVessel));
  public static readonly Tag Pickupable = TagManager.Create(nameof (Pickupable));
  public static readonly Tag Liquifiable = TagManager.Create(nameof (Liquifiable));
  public static readonly Tag IceOre = TagManager.Create(nameof (IceOre));
  public static readonly Tag OxyRock = TagManager.Create(nameof (OxyRock));
  public static readonly Tag Life = TagManager.Create(nameof (Life));
  public static readonly Tag Fertilizer = TagManager.Create(nameof (Fertilizer));
  public static readonly Tag Farmable = TagManager.Create(nameof (Farmable));
  public static readonly Tag Agriculture = TagManager.Create(nameof (Agriculture));
  public static readonly Tag Organics = TagManager.Create(nameof (Organics));
  public static readonly Tag IndustrialProduct = TagManager.Create(nameof (IndustrialProduct));
  public static readonly Tag IndustrialIngredient = TagManager.Create(nameof (IndustrialIngredient));
  public static readonly Tag TechComponents = TagManager.Create(nameof (TechComponents));
  public static readonly Tag Other = TagManager.Create(nameof (Other));
  public static readonly Tag ManufacturedMaterial = TagManager.Create(nameof (ManufacturedMaterial));
  public static readonly Tag Plastic = TagManager.Create(nameof (Plastic));
  public static readonly Tag Steel = TagManager.Create(nameof (Steel));
  public static readonly Tag BuildableAny = TagManager.Create(nameof (BuildableAny));
  public static readonly Tag Decoration = TagManager.Create(nameof (Decoration));
  public static readonly Tag Ornament = TagManager.Create(nameof (Ornament));
  public static readonly Tag OrnamentDisplayer = TagManager.Create("OrnamentDisplay");
  public static readonly Tag Window = TagManager.Create(nameof (Window));
  public static readonly Tag Bunker = TagManager.Create(nameof (Bunker));
  public static readonly Tag Transition = TagManager.Create(nameof (Transition));
  public static readonly Tag Detecting = TagManager.Create(nameof (Detecting));
  public static readonly Tag RareMaterials = TagManager.Create(nameof (RareMaterials));
  public static readonly Tag BuildingFiber = TagManager.Create(nameof (BuildingFiber));
  public static readonly Tag BuildingGasket = TagManager.Create(nameof (BuildingGasket));
  public static readonly Tag Insulator = TagManager.Create(nameof (Insulator));
  public static readonly Tag Plumbable = TagManager.Create(nameof (Plumbable));
  public static readonly Tag BuildingWood = TagManager.Create(nameof (BuildingWood));
  public static readonly Tag PreciousRock = TagManager.Create(nameof (PreciousRock));
  public static readonly Tag Artifact = TagManager.Create(nameof (Artifact));
  public static readonly Tag BionicUpgrade = TagManager.Create(nameof (BionicUpgrade));
  public static readonly Tag BionicBedTime = TagManager.Create(nameof (BionicBedTime));
  public static readonly Tag CharmedArtifact = TagManager.Create(nameof (CharmedArtifact));
  public static readonly Tag TerrestrialArtifact = TagManager.Create(nameof (TerrestrialArtifact));
  public static readonly Tag Keepsake = TagManager.Create(nameof (Keepsake));
  public static readonly Tag MiscPickupable = TagManager.Create(nameof (MiscPickupable));
  public static readonly Tag PlastifiableLiquid = TagManager.Create(nameof (PlastifiableLiquid));
  public static readonly Tag CombustibleGas = TagManager.Create(nameof (CombustibleGas));
  public static readonly Tag CombustibleLiquid = TagManager.Create(nameof (CombustibleLiquid));
  public static readonly Tag CombustibleSolid = TagManager.Create(nameof (CombustibleSolid));
  public static readonly Tag FlyingCritterEdible = TagManager.Create(nameof (FlyingCritterEdible));
  public static readonly Tag Comet = TagManager.Create(nameof (Comet));
  public static readonly Tag DeadReactor = TagManager.Create(nameof (DeadReactor));
  public static readonly Tag Robot = TagManager.Create(nameof (Robot));
  public static readonly Tag StoryTraitResource = TagManager.Create(nameof (StoryTraitResource));
  public static readonly Tag RoomProberBuilding = TagManager.Create(nameof (RoomProberBuilding));
  public static readonly Tag DevBuilding = TagManager.Create(nameof (DevBuilding));
  public static readonly Tag MarkedForMove = TagManager.Create(nameof (MarkedForMove));
  public static readonly Tag HideHealthBar = TagManager.Create(nameof (HideHealthBar));
  public static readonly Tag LongRangeMissile = TagManager.Create(nameof (LongRangeMissile));
  public static readonly Tag LightSource = TagManager.Create(nameof (LightSource));
  public static readonly Tag Incapacitated = TagManager.Create(nameof (Incapacitated));
  public static readonly Tag CaloriesDepleted = TagManager.Create(nameof (CaloriesDepleted));
  public static readonly Tag HitPointsDepleted = TagManager.Create(nameof (HitPointsDepleted));
  public static readonly Tag RadiationSicknessIncapacitation = TagManager.Create("RadiationSickness");
  public static readonly Tag SuffocatingIncapacitated = TagManager.Create(nameof (SuffocatingIncapacitated));
  public static readonly Tag Wilting = TagManager.Create(nameof (Wilting));
  public static readonly Tag Blighted = TagManager.Create(nameof (Blighted));
  public static readonly Tag PreventEmittingDisease = TagManager.Create("EmittingDisease");
  public static readonly Tag Creature = TagManager.Create(nameof (Creature));
  public static readonly Tag OriginalCreature = TagManager.Create(nameof (OriginalCreature));
  public static readonly Tag Hexaped = TagManager.Create(nameof (Hexaped));
  public static readonly Tag HeatBulb = TagManager.Create(nameof (HeatBulb));
  public static readonly Tag Egg = TagManager.Create(nameof (Egg));
  public static readonly Tag IncubatableEgg = TagManager.Create(nameof (IncubatableEgg));
  public static readonly Tag Trapped = TagManager.Create(nameof (Trapped));
  public static readonly Tag BagableCreature = TagManager.Create(nameof (BagableCreature));
  public static readonly Tag SwimmingCreature = TagManager.Create(nameof (SwimmingCreature));
  public static readonly Tag Spawner = TagManager.Create(nameof (Spawner));
  public static readonly Tag FullyIncubated = TagManager.Create(nameof (FullyIncubated));
  public static readonly Tag Amphibious = TagManager.Create(nameof (Amphibious));
  public static readonly Tag LargeCreature = TagManager.Create(nameof (LargeCreature));
  public static readonly Tag MoltShell = TagManager.Create(nameof (MoltShell));
  public static readonly Tag BaseMinion = TagManager.Create(nameof (BaseMinion));
  public static readonly Tag Corpse = TagManager.Create(nameof (Corpse));
  public static readonly Tag Alloy = TagManager.Create(nameof (Alloy));
  public static readonly Tag Metal = TagManager.Create(nameof (Metal));
  public static readonly Tag RefinedMetal = TagManager.Create(nameof (RefinedMetal));
  public static readonly Tag PreciousMetal = TagManager.Create(nameof (PreciousMetal));
  public static readonly Tag StoredMetal = TagManager.Create(nameof (StoredMetal));
  public static readonly Tag Solid = TagManager.Create(nameof (Solid));
  public static readonly Tag Liquid = TagManager.Create(nameof (Liquid));
  public static readonly Tag LiquidSource = TagManager.Create(nameof (LiquidSource));
  public static readonly Tag GasSource = TagManager.Create(nameof (GasSource));
  public static readonly Tag Water = TagManager.Create(nameof (Water));
  public static readonly Tag DirtyWater = TagManager.Create(nameof (DirtyWater));
  public static readonly Tag AnyWater = TagManager.Create(nameof (AnyWater));
  public static readonly Tag LubricatingOil = TagManager.Create(nameof (LubricatingOil));
  public static readonly Tag Algae = TagManager.Create(nameof (Algae));
  public static readonly Tag Void = TagManager.Create(nameof (Void));
  public static readonly Tag Chlorine = TagManager.Create(nameof (Chlorine));
  public static readonly Tag Oxygen = TagManager.Create(nameof (Oxygen));
  public static readonly Tag Hydrogen = TagManager.Create(nameof (Hydrogen));
  public static readonly Tag Methane = TagManager.Create(nameof (Methane));
  public static readonly Tag CarbonDioxide = TagManager.Create(nameof (CarbonDioxide));
  public static readonly Tag Carbon = TagManager.Create(nameof (Carbon));
  public static readonly Tag BuildableRaw = TagManager.Create(nameof (BuildableRaw));
  public static readonly Tag BuildableProcessed = TagManager.Create(nameof (BuildableProcessed));
  public static readonly Tag Phosphorus = TagManager.Create(nameof (Phosphorus));
  public static readonly Tag Phosphorite = TagManager.Create(nameof (Phosphorite));
  public static readonly Tag SlimeMold = TagManager.Create(nameof (SlimeMold));
  public static readonly Tag Filler = TagManager.Create(nameof (Filler));
  public static readonly Tag Item = TagManager.Create(nameof (Item));
  public static readonly Tag Ore = TagManager.Create(nameof (Ore));
  public static readonly Tag GenericOre = TagManager.Create(nameof (GenericOre));
  public static readonly Tag Ingot = TagManager.Create(nameof (Ingot));
  public static readonly Tag Dirt = TagManager.Create(nameof (Dirt));
  public static readonly Tag Filter = TagManager.Create(nameof (Filter));
  public static readonly Tag ConsumableOre = TagManager.Create(nameof (ConsumableOre));
  public static readonly Tag Unstable = TagManager.Create(nameof (Unstable));
  public static readonly Tag Slippery = TagManager.Create(nameof (Slippery));
  public static readonly Tag PreventsSlipping = TagManager.Create(nameof (PreventsSlipping));
  public static readonly Tag Sublimating = TagManager.Create(nameof (Sublimating));
  public static readonly Tag HideFromSpawnTool = TagManager.Create(nameof (HideFromSpawnTool));
  public static readonly Tag HideFromCodex = TagManager.Create(nameof (HideFromCodex));
  public static readonly Tag EmitsLight = TagManager.Create(nameof (EmitsLight));
  public static readonly Tag Special = TagManager.Create(nameof (Special));
  public static readonly Tag Breathable = TagManager.Create(nameof (Breathable));
  public static readonly Tag Unbreathable = TagManager.Create(nameof (Unbreathable));
  public static readonly Tag Gas = TagManager.Create(nameof (Gas));
  public static readonly Tag Crushable = TagManager.Create(nameof (Crushable));
  public static readonly Tag Noncrushable = TagManager.Create(nameof (Noncrushable));
  public static readonly Tag UseSmeltingByproducts = TagManager.Create(nameof (UseSmeltingByproducts));
  public static readonly Tag IronOre = TagManager.Create(nameof (IronOre));
  public static readonly Tag HighEnergyParticle = TagManager.Create(nameof (HighEnergyParticle));
  public static readonly Tag IgnoreMaterialCategory = TagManager.Create(nameof (IgnoreMaterialCategory));
  public static readonly Tag Oxidizer = TagManager.Create(nameof (Oxidizer));
  public static readonly Tag UnrefinedOil = TagManager.Create(nameof (UnrefinedOil));
  public static readonly Tag RiverSource = TagManager.Create(nameof (RiverSource));
  public static readonly Tag RiverSink = TagManager.Create(nameof (RiverSink));
  public static readonly Tag Garbage = TagManager.Create(nameof (Garbage));
  public static readonly Tag OilWell = TagManager.Create(nameof (OilWell));
  public static readonly Tag Glass = TagManager.Create(nameof (Glass));
  public static readonly Tag Door = TagManager.Create(nameof (Door));
  public static readonly Tag Farm = TagManager.Create(nameof (Farm));
  public static readonly Tag StorageLocker = TagManager.Create(nameof (StorageLocker));
  public static readonly Tag LadderBed = TagManager.Create(nameof (LadderBed));
  public static readonly Tag ReefGenerator = TagManager.Create(nameof (ReefGenerator));
  public static readonly Tag UnderwaterVentDrill = TagManager.Create(nameof (UnderwaterVentDrill));
  public static readonly Tag FloorTiles = TagManager.Create(nameof (FloorTiles));
  public static readonly Tag Carpeted = TagManager.Create(nameof (Carpeted));
  public static readonly Tag FarmTiles = TagManager.Create(nameof (FarmTiles));
  public static readonly Tag Ladders = TagManager.Create(nameof (Ladders));
  public static readonly Tag NavTeleporters = TagManager.Create(nameof (NavTeleporters));
  public static readonly Tag Wires = TagManager.Create(nameof (Wires));
  public static readonly Tag Vents = TagManager.Create(nameof (Vents));
  public static readonly Tag Pipes = TagManager.Create(nameof (Pipes));
  public static readonly Tag WireBridges = TagManager.Create(nameof (WireBridges));
  public static readonly Tag TravelTubeBridges = TagManager.Create(nameof (TravelTubeBridges));
  public static readonly Tag Backwall = TagManager.Create(nameof (Backwall));
  public static readonly Tag MISSING_TAG = TagManager.Create(nameof (MISSING_TAG));
  public static readonly Tag PlantRenderer = TagManager.Create(nameof (PlantRenderer));
  public static readonly Tag Usable = TagManager.Create(nameof (Usable));
  public static readonly Tag PedestalDisplayable = TagManager.Create(nameof (PedestalDisplayable));
  public static readonly Tag HasChores = TagManager.Create(nameof (HasChores));
  public static readonly Tag Suit = TagManager.Create(nameof (Suit));
  public static readonly Tag FeetProtection = TagManager.Create(nameof (FeetProtection));
  public static readonly Tag FeetAndWaistProtection = TagManager.Create(nameof (FeetAndWaistProtection));
  public static readonly Tag AirtightSuit = TagManager.Create(nameof (AirtightSuit));
  public static readonly Tag AtmoSuit = TagManager.Create("Atmo_Suit");
  public static readonly Tag OxygenMask = TagManager.Create("Oxygen_Mask");
  public static readonly Tag LeadSuit = TagManager.Create("Lead_Suit");
  public static readonly Tag JetSuit = TagManager.Create("Jet_Suit");
  public static readonly Tag JetSuitOutOfFuel = TagManager.Create(nameof (JetSuitOutOfFuel));
  public static readonly Tag SuitBatteryLow = TagManager.Create(nameof (SuitBatteryLow));
  public static readonly Tag SuitBatteryOut = TagManager.Create(nameof (SuitBatteryOut));
  public static readonly List<Tag> AllSuitTags = new List<Tag>()
  {
    GameTags.Suit,
    GameTags.AtmoSuit,
    GameTags.JetSuit,
    GameTags.LeadSuit
  };
  public static readonly List<Tag> OxygenSuitTags = new List<Tag>()
  {
    GameTags.AtmoSuit,
    GameTags.JetSuit,
    GameTags.LeadSuit
  };
  public static readonly Tag EquippableBalloon = TagManager.Create(nameof (EquippableBalloon));
  public static readonly Tag Clothes = TagManager.Create(nameof (Clothes));
  public static readonly Tag WarmVest = TagManager.Create("Warm_Vest");
  public static readonly Tag FunkyVest = TagManager.Create("Funky_Vest");
  public static readonly List<Tag> AllClothesTags = new List<Tag>()
  {
    GameTags.Clothes,
    GameTags.WarmVest,
    GameTags.FunkyVest
  };
  public static readonly Tag Assigned = TagManager.Create(nameof (Assigned));
  public static readonly Tag Helmet = TagManager.Create(nameof (Helmet));
  public static readonly Tag Equipped = TagManager.Create(nameof (Equipped));
  public static readonly Tag DisposablePortableBattery = TagManager.Create(nameof (DisposablePortableBattery));
  public static readonly Tag ChargedPortableBattery = TagManager.Create(nameof (ChargedPortableBattery));
  public static readonly Tag EmptyPortableBattery = TagManager.Create(nameof (EmptyPortableBattery));
  public static readonly Tag SolidLubricant = TagManager.Create(nameof (SolidLubricant));
  public static readonly Tag Entombed = TagManager.Create(nameof (Entombed));
  public static readonly Tag Uprooted = TagManager.Create(nameof (Uprooted));
  public static readonly Tag Preserved = TagManager.Create(nameof (Preserved));
  public static readonly Tag Compostable = TagManager.Create(nameof (Compostable));
  public static readonly Tag Pickled = TagManager.Create(nameof (Pickled));
  public static readonly Tag UnspicedFood = TagManager.Create(nameof (UnspicedFood));
  public static readonly Tag SpicedFood = TagManager.Create(nameof (SpicedFood));
  public static readonly Tag Dying = TagManager.Create(nameof (Dying));
  public static readonly Tag Dead = TagManager.Create(nameof (Dead));
  public static readonly Tag PreventDeadAnimation = TagManager.Create(nameof (PreventDeadAnimation));
  public static readonly Tag Reachable = TagManager.Create(nameof (Reachable));
  public static readonly Tag PreventChoreInterruption = TagManager.Create(nameof (PreventChoreInterruption));
  public static readonly Tag PerformingWorkRequest = TagManager.Create(nameof (PerformingWorkRequest));
  public static readonly Tag RecoveringBreath = TagManager.Create(nameof (RecoveringBreath));
  public static readonly Tag FeelingCold = TagManager.Create(nameof (FeelingCold));
  public static readonly Tag FeelingWarm = TagManager.Create(nameof (FeelingWarm));
  public static readonly Tag RecoveringWarmnth = TagManager.Create(nameof (RecoveringWarmnth));
  public static readonly Tag RecoveringFromHeat = TagManager.Create(nameof (RecoveringFromHeat));
  public static readonly Tag NoOxygen = TagManager.Create(nameof (NoOxygen));
  public static readonly Tag Idle = TagManager.Create(nameof (Idle));
  public static readonly Tag StationaryIdling = TagManager.Create(nameof (StationaryIdling));
  public static readonly Tag AlwaysConverse = TagManager.Create(nameof (AlwaysConverse));
  public static readonly Tag SuppressConversation = TagManager.Create(nameof (SuppressConversation));
  public static readonly Tag HasDebugDestination = TagManager.Create(nameof (HasDebugDestination));
  public static readonly Tag Shaded = TagManager.Create(nameof (Shaded));
  public static readonly Tag TakingMedicine = TagManager.Create(nameof (TakingMedicine));
  public static readonly Tag Partying = TagManager.Create(nameof (Partying));
  public static readonly Tag MakingMess = TagManager.Create(nameof (MakingMess));
  public static readonly Tag DupeBrain = TagManager.Create(nameof (DupeBrain));
  public static readonly Tag CreatureBrain = TagManager.Create(nameof (CreatureBrain));
  public static readonly Tag Asleep = TagManager.Create(nameof (Asleep));
  public static readonly Tag HoldingBreath = TagManager.Create(nameof (HoldingBreath));
  public static readonly Tag Overjoyed = TagManager.Create(nameof (Overjoyed));
  public static readonly Tag PleasantConversation = TagManager.Create(nameof (PleasantConversation));
  public static readonly Tag CommunalDining = TagManager.Create(nameof (CommunalDining));
  public static readonly Tag WantsToTalk = TagManager.Create(nameof (WantsToTalk));
  public static readonly Tag DoNotInterruptMe = TagManager.Create(nameof (DoNotInterruptMe));
  public static readonly Tag HasSuitTank = TagManager.Create(nameof (HasSuitTank));
  public static readonly Tag HasAirtightSuit = TagManager.Create(nameof (HasAirtightSuit));
  public static readonly Tag NoCreatureIdling = TagManager.Create(nameof (NoCreatureIdling));
  public static readonly Tag UnderConstruction = TagManager.Create(nameof (UnderConstruction));
  public static readonly Tag Operational = TagManager.Create(nameof (Operational));
  public static readonly Tag JetSuitBlocker = TagManager.Create(nameof (JetSuitBlocker));
  public static readonly Tag HasInvalidPorts = TagManager.Create(nameof (HasInvalidPorts));
  public static readonly Tag NotRoomAssignable = TagManager.Create(nameof (NotRoomAssignable));
  public static readonly Tag OneTimeUseLure = TagManager.Create(nameof (OneTimeUseLure));
  public static readonly Tag LureUsed = TagManager.Create(nameof (LureUsed));
  public static readonly Tag TemplateBuilding = TagManager.Create(nameof (TemplateBuilding));
  public static readonly Tag ModularConduitPort = TagManager.Create(nameof (ModularConduitPort));
  public static readonly Tag WarpTech = TagManager.Create(nameof (WarpTech));
  public static readonly Tag HEPPassThrough = TagManager.Create(nameof (HEPPassThrough));
  public static readonly Tag TelephoneRinging = TagManager.Create(nameof (TelephoneRinging));
  public static readonly Tag LongDistanceCall = TagManager.Create(nameof (LongDistanceCall));
  public static readonly Tag Telepad = TagManager.Create(nameof (Telepad));
  public static readonly Tag InTransitTube = TagManager.Create(nameof (InTransitTube));
  public static readonly Tag TrapArmed = TagManager.Create(nameof (TrapArmed));
  public static readonly Tag GeyserFeature = TagManager.Create(nameof (GeyserFeature));
  public static readonly Tag GeyserExhaling = TagManager.Create(nameof (GeyserExhaling));
  public static readonly Tag Rocket = TagManager.Create(nameof (Rocket));
  public static readonly Tag RocketOnGround = TagManager.Create(nameof (RocketOnGround));
  public static readonly Tag RocketNotOnGround = TagManager.Create(nameof (RocketNotOnGround));
  public static readonly Tag RocketInSpace = TagManager.Create(nameof (RocketInSpace));
  public static readonly Tag RocketStranded = TagManager.Create(nameof (RocketStranded));
  public static readonly Tag RailGunPayloadEmptyable = TagManager.Create(nameof (RailGunPayloadEmptyable));
  public static readonly Tag TransferringCargoComplete = TagManager.Create(nameof (TransferringCargoComplete));
  public static readonly Tag NoseRocketModule = TagManager.Create(nameof (NoseRocketModule));
  public static readonly Tag LaunchButtonRocketModule = TagManager.Create(nameof (LaunchButtonRocketModule));
  public static readonly Tag RocketInteriorBuilding = TagManager.Create(nameof (RocketInteriorBuilding));
  public static readonly Tag NotRocketInteriorBuilding = TagManager.Create(nameof (NotRocketInteriorBuilding));
  public static readonly Tag UniquePerWorld = TagManager.Create(nameof (UniquePerWorld));
  public static readonly Tag RocketEnvelopeTile = TagManager.Create(nameof (RocketEnvelopeTile));
  public static readonly Tag NoRocketRefund = TagManager.Create(nameof (NoRocketRefund));
  public static readonly Tag RocketModule = TagManager.Create(nameof (RocketModule));
  public static readonly Tag GantryExtended = TagManager.Create(nameof (GantryExtended));
  public static readonly Tag RocketDrilling = TagManager.Create(nameof (RocketDrilling));
  public static readonly Tag RocketCollectingResources = TagManager.Create(nameof (RocketCollectingResources));
  public static readonly Tag BallisticEntityLanding = TagManager.Create(nameof (BallisticEntityLanding));
  public static readonly Tag BallisticEntityLaunching = TagManager.Create(nameof (BallisticEntityLaunching));
  public static readonly Tag BallisticEntityMoving = TagManager.Create(nameof (BallisticEntityMoving));
  public static readonly Tag ClusterEntityGrounded = TagManager.Create("ClusterEntityGrounded ");
  public static readonly Tag LongRangeMissileMoving = TagManager.Create(nameof (LongRangeMissileMoving));
  public static readonly Tag LongRangeMissileIdle = TagManager.Create(nameof (LongRangeMissileIdle));
  public static readonly Tag LongRangeMissileExploding = TagManager.Create(nameof (LongRangeMissileExploding));
  public static readonly Tag EntityInSpace = TagManager.Create(nameof (EntityInSpace));
  public static readonly Tag Monument = TagManager.Create(nameof (Monument));
  public static readonly Tag Stored = TagManager.Create(nameof (Stored));
  public static readonly Tag StoredPrivate = TagManager.Create(nameof (StoredPrivate));
  public static readonly Tag Sealed = TagManager.Create(nameof (Sealed));
  public static readonly Tag CorrosionProof = TagManager.Create(nameof (CorrosionProof));
  public static readonly Tag PickupableStorage = TagManager.Create(nameof (PickupableStorage));
  public static readonly Tag UnidentifiedSeed = TagManager.Create(nameof (UnidentifiedSeed));
  public static readonly Tag CropSeed = TagManager.Create(nameof (CropSeed));
  public static readonly Tag LargeSeed = TagManager.Create(nameof (LargeSeed));
  public static readonly Tag BackwallSeed = TagManager.Create(nameof (BackwallSeed));
  public static readonly Tag DecorSeed = TagManager.Create(nameof (DecorSeed));
  public static readonly Tag WaterSeed = TagManager.Create(nameof (WaterSeed));
  public static readonly Tag Harvestable = TagManager.Create(nameof (Harvestable));
  public static readonly Tag Hanging = TagManager.Create(nameof (Hanging));
  public static readonly Tag FarmingMaterial = TagManager.Create(nameof (FarmingMaterial));
  public static readonly Tag MutatedSeed = TagManager.Create(nameof (MutatedSeed));
  public static readonly Tag BlockBuildOverPlantFeature = TagManager.Create(nameof (BlockBuildOverPlantFeature));
  public static readonly Tag OverlayInFrontOfConduits = TagManager.Create("OverlayFrontLayer");
  public static readonly Tag OverlayBehindConduits = TagManager.Create("OverlayBackLayer");
  public static readonly Tag MassChunk = TagManager.Create(nameof (MassChunk));
  public static readonly Tag UnitChunk = TagManager.Create(nameof (UnitChunk));
  public static readonly Tag NotConversationTopic = TagManager.Create(nameof (NotConversationTopic));
  public static readonly Tag MinionSelectPreview = TagManager.Create(nameof (MinionSelectPreview));
  public static readonly Tag Empty = TagManager.Create(nameof (Empty));
  public static readonly Tag ExcludeFromTemplate = TagManager.Create(nameof (ExcludeFromTemplate));
  public static readonly Tag SpaceDanger = TagManager.Create(nameof (SpaceDanger));
  public static TagSet SolidElements = new TagSet();
  public static TagSet LiquidElements = new TagSet();
  public static TagSet GasElements = new TagSet();
  public static TagSet CalorieCategories = new TagSet()
  {
    GameTags.Edible
  };
  public static TagSet UnitCategories = new TagSet()
  {
    GameTags.Medicine,
    GameTags.MedicalSupplies,
    GameTags.Seed,
    GameTags.Egg,
    GameTags.Clothes,
    GameTags.IndustrialIngredient,
    GameTags.IndustrialProduct,
    GameTags.TechComponents,
    GameTags.Compostable,
    GameTags.HighEnergyParticle,
    GameTags.StoryTraitResource,
    GameTags.Dehydrated,
    GameTags.ChargedPortableBattery,
    GameTags.BionicUpgrade
  };
  public static TagSet IgnoredMaterialCategories = new TagSet()
  {
    GameTags.Special,
    GameTags.IgnoreMaterialCategory
  };
  public static TagSet MaterialCategories = new TagSet()
  {
    GameTags.Alloy,
    GameTags.Metal,
    GameTags.RefinedMetal,
    GameTags.BuildableRaw,
    GameTags.BuildableProcessed,
    GameTags.Filter,
    GameTags.Liquifiable,
    GameTags.Liquid,
    GameTags.Breathable,
    GameTags.Unbreathable,
    GameTags.ConsumableOre,
    GameTags.Sublimating,
    GameTags.Organics,
    GameTags.Farmable,
    GameTags.Agriculture,
    GameTags.Other,
    GameTags.ManufacturedMaterial,
    GameTags.CookingIngredient,
    GameTags.RareMaterials
  };
  public static TagSet BionicCompatibleBatteries = new TagSet()
  {
    (Tag) "Electrobank",
    GameTags.DisposablePortableBattery,
    GameTags.EmptyPortableBattery
  };
  public static TagSet BionicIncompatibleBatteries = new TagSet()
  {
    (Tag) "SelfChargingElectrobank"
  };
  public static TagSet MaterialBuildingElements = new TagSet()
  {
    GameTags.BuildingFiber,
    GameTags.BuildingWood,
    GameTags.BuildingGasket
  };
  public static TagSet OtherEntityTags = new TagSet()
  {
    GameTags.BagableCreature,
    GameTags.SwimmingCreature,
    GameTags.MiscPickupable
  };
  public static TagSet AllCategories = new TagSet(new TagSet[5]
  {
    GameTags.CalorieCategories,
    GameTags.UnitCategories,
    GameTags.MaterialCategories,
    GameTags.MaterialBuildingElements,
    GameTags.OtherEntityTags
  });
  public static TagSet DisplayAsCalories = new TagSet(GameTags.CalorieCategories);
  public static TagSet DisplayAsUnits = new TagSet(GameTags.UnitCategories);
  public static TagSet DisplayAsInformation = new TagSet();
  public static Tag StartingMetalOre = new Tag(nameof (StartingMetalOre));
  public static Tag StartingRefinedMetal = new Tag(nameof (StartingRefinedMetal));
  public static Tag[] StartingMetalOres;
  public static Tag[] StartingRefinedMetals = (Tag[]) null;
  public static Tag[] BasicWoods = new Tag[3]
  {
    SimHashes.WoodLog.CreateTag(),
    SimHashes.FabricatedWood.CreateTag(),
    SimHashes.PalmWood.CreateTag()
  };
  public static Tag[] BasicMetalOres = new Tag[1]
  {
    SimHashes.IronOre.CreateTag()
  };
  public static Tag[] BasicRefinedMetals = new Tag[1]
  {
    SimHashes.Iron.CreateTag()
  };
  public static TagSet HiddenElementTags = new TagSet()
  {
    GameTags.HideFromCodex,
    GameTags.HideFromSpawnTool,
    GameTags.StartingMetalOre,
    GameTags.StartingRefinedMetal,
    GameTags.UseSmeltingByproducts
  };
  public static Tag[] Fabrics = new Tag[2]
  {
    "BasicFabric".ToTag(),
    (Tag) FeatherFabricConfig.ID
  };

  public static Tag[] Reflection_GetTagsInClass(System.Type classAddress, BindingFlags variableFlags = BindingFlags.Static | BindingFlags.Public)
  {
    List<FieldInfo> all = new List<FieldInfo>((IEnumerable<FieldInfo>) classAddress.GetFields(variableFlags)).FindAll((Predicate<FieldInfo>) (f => f.FieldType == typeof (Tag)));
    Tag[] tagsInClass = new Tag[all.Count];
    for (int index = 0; index < tagsInClass.Length; ++index)
      tagsInClass[index] = (Tag) all[index].Name;
    return tagsInClass;
  }

  public static class Worlds
  {
    public static readonly Tag Ceres = TagManager.Create(nameof (Ceres));
  }

  public abstract class ChoreTypes
  {
    public static readonly Tag Farming = TagManager.Create(nameof (Farming));
    public static readonly Tag Ranching = TagManager.Create(nameof (Ranching));
    public static readonly Tag Research = TagManager.Create(nameof (Research));
    public static readonly Tag Power = TagManager.Create(nameof (Power));
    public static readonly Tag Building = TagManager.Create(nameof (Building));
    public static readonly Tag Cooking = TagManager.Create(nameof (Cooking));
    public static readonly Tag Fabricating = TagManager.Create(nameof (Fabricating));
    public static readonly Tag Wiring = TagManager.Create(nameof (Wiring));
    public static readonly Tag Art = TagManager.Create(nameof (Art));
    public static readonly Tag Digging = TagManager.Create(nameof (Digging));
    public static readonly Tag Doctoring = TagManager.Create(nameof (Doctoring));
    public static readonly Tag Conveyor = TagManager.Create(nameof (Conveyor));
  }

  public static class RotModifierTags
  {
    public static readonly Tag Fresh = TagManager.Create(nameof (Fresh));
    public static readonly Tag Stale = TagManager.Create(nameof (Stale));
    public static readonly Tag DeepFrozen = TagManager.Create(nameof (DeepFrozen));
    public static readonly Tag Refrigerated = TagManager.Create(nameof (Refrigerated));
  }

  public static class Creatures
  {
    public static readonly Tag ReservedByCreature = TagManager.Create(nameof (ReservedByCreature));
    public static readonly Tag PreventGrowAnimation = TagManager.Create(nameof (PreventGrowAnimation));
    public static readonly Tag TrappedInCargoBay = TagManager.Create(nameof (TrappedInCargoBay));
    public static readonly Tag PausedHunger = TagManager.Create(nameof (PausedHunger));
    public static readonly Tag PausedReproduction = TagManager.Create(nameof (PausedReproduction));
    public static readonly Tag Bagged = TagManager.Create(nameof (Bagged));
    public static readonly Tag InIncubator = TagManager.Create(nameof (InIncubator));
    public static readonly Tag Deliverable = TagManager.Create(nameof (Deliverable));
    public static readonly Tag StunnedForCapture = TagManager.Create(nameof (StunnedForCapture));
    public static readonly Tag StunnedBeingEaten = TagManager.Create(nameof (StunnedBeingEaten));
    public static readonly Tag Falling = TagManager.Create(nameof (Falling));
    public static readonly Tag Flopping = TagManager.Create(nameof (Flopping));
    public static readonly Tag WantsToEnterBurrow = TagManager.Create("WantsToBurrow");
    public static readonly Tag Burrowed = TagManager.Create(nameof (Burrowed));
    public static readonly Tag WantsToExitBurrow = TagManager.Create(nameof (WantsToExitBurrow));
    public static readonly Tag WantsToEat = TagManager.Create(nameof (WantsToEat));
    public static readonly Tag WantsToPunchClam = TagManager.Create(nameof (WantsToPunchClam));
    public static readonly Tag SuppressedDiet = TagManager.Create(nameof (SuppressedDiet));
    public static readonly Tag UrgeToPoke = TagManager.Create(nameof (UrgeToPoke));
    public static readonly Tag WantsToStomp = TagManager.Create(nameof (WantsToStomp));
    public static readonly Tag WantsToHarvest = TagManager.Create(nameof (WantsToHarvest));
    public static readonly Tag Behaviour_TryToDrinkMilkFromFeeder = TagManager.Create(nameof (Behaviour_TryToDrinkMilkFromFeeder));
    public static readonly Tag Behaviour_InteractWithCritterCondo = TagManager.Create(nameof (Behaviour_InteractWithCritterCondo));
    public static readonly Tag WantsToGetRanched = TagManager.Create(nameof (WantsToGetRanched));
    public static readonly Tag WantsToGetCaptured = TagManager.Create(nameof (WantsToGetCaptured));
    public static readonly Tag WantsToClimbTree = TagManager.Create(nameof (WantsToClimbTree));
    public static readonly Tag WantsToPlantSeed = TagManager.Create(nameof (WantsToPlantSeed));
    public static readonly Tag WantsToForage = TagManager.Create(nameof (WantsToForage));
    public static readonly Tag WantsToLayEgg = TagManager.Create(nameof (WantsToLayEgg));
    public static readonly Tag WantsToTendEgg = TagManager.Create(nameof (WantsToTendEgg));
    public static readonly Tag WantsAHug = TagManager.Create(nameof (WantsAHug));
    public static readonly Tag WantsConduitConnection = TagManager.Create(nameof (WantsConduitConnection));
    public static readonly Tag WantsToGoHome = TagManager.Create(nameof (WantsToGoHome));
    public static readonly Tag WantsToMakeHome = TagManager.Create(nameof (WantsToMakeHome));
    public static readonly Tag BeeWantsToSleep = TagManager.Create(nameof (BeeWantsToSleep));
    public static readonly Tag WantsToTendCrops = TagManager.Create("WantsToTendPlants");
    public static readonly Tag WantsToStore = TagManager.Create(nameof (WantsToStore));
    public static readonly Tag WantsToBeckon = TagManager.Create(nameof (WantsToBeckon));
    public static readonly Tag Flee = TagManager.Create(nameof (Flee));
    public static readonly Tag Attack = TagManager.Create(nameof (Attack));
    public static readonly Tag Defend = TagManager.Create(nameof (Defend));
    public static readonly Tag ReturnToEgg = TagManager.Create(nameof (ReturnToEgg));
    public static readonly Tag CrabFriend = TagManager.Create(nameof (CrabFriend));
    public static readonly Tag SquidFriend = TagManager.Create(nameof (SquidFriend));
    public static readonly Tag Die = TagManager.Create(nameof (Die));
    public static readonly Tag Poop = TagManager.Create(nameof (Poop));
    public static readonly Tag MoveToLure = TagManager.Create(nameof (MoveToLure));
    public static readonly Tag Drowning = TagManager.Create(nameof (Drowning));
    public static readonly Tag Hungry = TagManager.Create(nameof (Hungry));
    public static readonly Tag Flyer = TagManager.Create(nameof (Flyer));
    public static readonly Tag FishTrapLure = TagManager.Create(nameof (FishTrapLure));
    public static readonly Tag FlyersLure = TagManager.Create("MasterLure");
    public static readonly Tag Walker = TagManager.Create(nameof (Walker));
    public static readonly Tag Hoverer = TagManager.Create(nameof (Hoverer));
    public static readonly Tag Swimmer = TagManager.Create(nameof (Swimmer));
    public static readonly Tag Fertile = TagManager.Create(nameof (Fertile));
    public static readonly Tag Submerged = TagManager.Create(nameof (Submerged));
    public static readonly Tag ExitSubmerged = TagManager.Create(nameof (ExitSubmerged));
    public static readonly Tag WantsToDropElements = TagManager.Create(nameof (WantsToDropElements));
    public static readonly Tag WantsToConsumeAir = TagManager.Create(nameof (WantsToConsumeAir));
    public static readonly Tag OriginallyWild = TagManager.Create(nameof (Wild));
    public static readonly Tag Wild = TagManager.Create(nameof (Wild));
    public static readonly Tag Overcrowded = TagManager.Create(nameof (Overcrowded));
    public static readonly Tag Expecting = TagManager.Create(nameof (Expecting));
    public static readonly Tag Confined = TagManager.Create(nameof (Confined));
    public static readonly Tag Digger = TagManager.Create(nameof (Digger));
    public static readonly Tag Tunnel = TagManager.Create(nameof (Tunnel));
    public static readonly Tag Builder = TagManager.Create(nameof (Builder));
    public static readonly Tag ScalesGrown = TagManager.Create(nameof (ScalesGrown));
    public static readonly Tag CanMolt = TagManager.Create(nameof (CanMolt));
    public static readonly Tag ReadyToMolt = TagManager.Create(nameof (ReadyToMolt));
    public static readonly Tag CantReachEgg = TagManager.Create(nameof (CantReachEgg));
    public static readonly Tag HasNoFoundation = TagManager.Create(nameof (HasNoFoundation));
    public static readonly Tag Cleaning = TagManager.Create(nameof (Cleaning));
    public static readonly Tag Happy = TagManager.Create(nameof (Happy));
    public static readonly Tag Unhappy = TagManager.Create(nameof (Unhappy));
    public static readonly Tag RequiresMilking = TagManager.Create(nameof (RequiresMilking));
    public static readonly Tag TargetedPreyBehaviour = TagManager.Create("TargetedPrey");
    public static readonly Tag WantsToPollinate = TagManager.Create(nameof (WantsToPollinate));
    public static readonly Tag Pollinator = TagManager.Create(nameof (Pollinator));
    public static readonly Tag CritterDrinkable = TagManager.Create(nameof (CritterDrinkable));
    public static readonly Tag Dry = TagManager.Create(nameof (Dry));

    public static class Species
    {
      public static readonly Tag HatchSpecies = TagManager.Create(nameof (HatchSpecies), (string) CREATURES.FAMILY_PLURAL.HATCHSPECIES);
      public static readonly Tag LightBugSpecies = TagManager.Create(nameof (LightBugSpecies), (string) CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES);
      public static readonly Tag OilFloaterSpecies = TagManager.Create(nameof (OilFloaterSpecies), (string) CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES);
      public static readonly Tag DreckoSpecies = TagManager.Create(nameof (DreckoSpecies), (string) CREATURES.FAMILY_PLURAL.DRECKOSPECIES);
      public static readonly Tag GlomSpecies = TagManager.Create(nameof (GlomSpecies), (string) CREATURES.FAMILY_PLURAL.GLOMSPECIES);
      public static readonly Tag PuftSpecies = TagManager.Create(nameof (PuftSpecies), (string) CREATURES.FAMILY_PLURAL.PUFTSPECIES);
      public static readonly Tag MosquitoSpecies = TagManager.Create(nameof (MosquitoSpecies), (string) CREATURES.FAMILY_PLURAL.MOSQUITOSPECIES);
      public static readonly Tag PacuSpecies = TagManager.Create(nameof (PacuSpecies), (string) CREATURES.FAMILY_PLURAL.PACUSPECIES);
      public static readonly Tag MooSpecies = TagManager.Create(nameof (MooSpecies), (string) CREATURES.FAMILY_PLURAL.MOOSPECIES);
      public static readonly Tag MoleSpecies = TagManager.Create(nameof (MoleSpecies), (string) CREATURES.FAMILY_PLURAL.MOLESPECIES);
      public static readonly Tag SquirrelSpecies = TagManager.Create(nameof (SquirrelSpecies), (string) CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES);
      public static readonly Tag CrabSpecies = TagManager.Create(nameof (CrabSpecies), (string) CREATURES.FAMILY_PLURAL.CRABSPECIES);
      public static readonly Tag StaterpillarSpecies = TagManager.Create(nameof (StaterpillarSpecies), (string) CREATURES.FAMILY_PLURAL.STATERPILLARSPECIES);
      public static readonly Tag BeetaSpecies = TagManager.Create(nameof (BeetaSpecies), (string) CREATURES.FAMILY_PLURAL.BEETASPECIES);
      public static readonly Tag DivergentSpecies = TagManager.Create(nameof (DivergentSpecies), (string) CREATURES.FAMILY_PLURAL.DIVERGENTSPECIES);
      public static readonly Tag DeerSpecies = TagManager.Create(nameof (DeerSpecies), (string) CREATURES.FAMILY_PLURAL.DEERSPECIES);
      public static readonly Tag BellySpecies = TagManager.Create(nameof (BellySpecies), (string) CREATURES.FAMILY_PLURAL.BELLYSPECIES);
      public static readonly Tag SealSpecies = TagManager.Create(nameof (SealSpecies), (string) CREATURES.FAMILY_PLURAL.SEALSPECIES);
      public static readonly Tag RaptorSpecies = TagManager.Create(nameof (RaptorSpecies), (string) CREATURES.FAMILY_PLURAL.RAPTORSPECIES);
      public static readonly Tag ChameleonSpecies = TagManager.Create(nameof (ChameleonSpecies), (string) CREATURES.FAMILY_PLURAL.CHAMELEONSPECIES);
      public static readonly Tag PrehistoricPacuSpecies = TagManager.Create(nameof (PrehistoricPacuSpecies), (string) CREATURES.FAMILY_PLURAL.PREHISTORICPACUSPECIES);
      public static readonly Tag StegoSpecies = TagManager.Create(nameof (StegoSpecies), (string) CREATURES.FAMILY_PLURAL.STEGOSPECIES);
      public static readonly Tag ButterflySpecies = TagManager.Create(nameof (ButterflySpecies), (string) CREATURES.FAMILY_PLURAL.BUTTERFLYSPECIES);
      public static readonly Tag ParrotFishSpecies = TagManager.Create(nameof (ParrotFishSpecies), (string) CREATURES.FAMILY_PLURAL.PARROTFISHSPECIES);
      public static readonly Tag SquidSpecies = TagManager.Create(nameof (SquidSpecies), (string) CREATURES.FAMILY_PLURAL.SQUIDSPECIES);
      public static readonly Tag PufferFishSpecies = TagManager.Create(nameof (PufferFishSpecies), (string) CREATURES.FAMILY_PLURAL.PUFFERFISHSPECIES);
      public static readonly Tag SeaFairySpecies = TagManager.Create(nameof (SeaFairySpecies), (string) CREATURES.FAMILY_PLURAL.SEAFAIRYSPECIES);
      public static readonly Tag SeaTurtleSpecies = TagManager.Create(nameof (SeaTurtleSpecies), (string) CREATURES.FAMILY_PLURAL.SEATURTLESPECIES);
      public static readonly Tag SeaHorseSpecies = TagManager.Create(nameof (SeaHorseSpecies), (string) CREATURES.FAMILY_PLURAL.SEAHORSESPECIES);
      public static readonly Tag SnailSpecies = TagManager.Create(nameof (SnailSpecies), (string) CREATURES.FAMILY_PLURAL.SNAILSPECIES);

      public static Tag[] AllSpecies_REFLECTION()
      {
        return GameTags.Reflection_GetTagsInClass(typeof (GameTags.Creatures.Species));
      }
    }

    public static class Behaviours
    {
      public static readonly Tag HarvestHiveBehaviour = TagManager.Create(nameof (HarvestHiveBehaviour));
      public static readonly Tag GrowUpBehaviour = TagManager.Create(nameof (GrowUpBehaviour));
      public static readonly Tag SleepBehaviour = TagManager.Create(nameof (SleepBehaviour));
      public static readonly Tag CallAdultBehaviour = TagManager.Create(nameof (CallAdultBehaviour));
      public static readonly Tag SearchForEggBehaviour = TagManager.Create(nameof (SearchForEggBehaviour));
      public static readonly Tag PlayInterruptAnim = TagManager.Create(nameof (PlayInterruptAnim));
      public static readonly Tag DisableCreature = TagManager.Create(nameof (DisableCreature));
      public static readonly Tag CritterEmoteBehaviour = TagManager.Create(nameof (CritterEmoteBehaviour));
      public static readonly Tag CritterRoarBehaviour = TagManager.Create(nameof (CritterRoarBehaviour));
      public static readonly Tag SecretingMucusBehavior = TagManager.Create(nameof (SecretingMucusBehavior));
    }
  }

  public static class StoragesIds
  {
    public static readonly Tag DefaultStorage = TagManager.Create("Storage");
    public static readonly Tag BionicBatteryStorage = TagManager.Create(nameof (BionicBatteryStorage));
    public static readonly Tag BionicUpgradeStorage = TagManager.Create(nameof (BionicUpgradeStorage));
    public static readonly Tag BionicOxygenTankStorage = TagManager.Create(nameof (BionicOxygenTankStorage));
  }

  public static class Minions
  {
    public static class Models
    {
      public static readonly Tag Standard = TagManager.Create("Minion", (string) DUPLICANTS.MODEL.STANDARD.NAME);
      public static readonly Tag Bionic = TagManager.Create("BionicMinion", (string) DUPLICANTS.MODEL.BIONIC.NAME);
      public static readonly Tag[] AllModels = new Tag[2]
      {
        GameTags.Minions.Models.Standard,
        GameTags.Minions.Models.Bionic
      };

      public static string GetModelTooltipForTag(Tag modelTag)
      {
        return modelTag == GameTags.Minions.Models.Bionic ? (string) DUPLICANTS.MODEL.BIONIC.NAME_TOOLTIP : "";
      }
    }
  }

  public static class CodexCategories
  {
    public static List<Tag> AllTags = new List<Tag>();
    public static Tag CreatureRelocator = GameTags.CodexCategories.AllTags.AddAndReturn(TagManager.Create(nameof (CreatureRelocator)));
    public static Tag FarmBuilding = GameTags.CodexCategories.AllTags.AddAndReturn(nameof (FarmBuilding).ToTag());
    public static Tag BionicBuilding = GameTags.CodexCategories.AllTags.AddAndReturn(nameof (BionicBuilding).ToTag());
    public static Tag Ornament = GameTags.CodexCategories.AllTags.AddAndReturn(nameof (Ornament).ToTag());
    public static Tag Submergible = GameTags.CodexCategories.AllTags.AddAndReturn(nameof (Ornament).ToTag());

    public static string GetCategoryLabelText(Tag tag)
    {
      StringEntry result = (StringEntry) null;
      string str = $"STRINGS.CODEX.CATEGORIES.{tag.ToString().ToUpper()}.NAME";
      return !Strings.TryGet(new StringKey(str), out result) ? ROOMS.CRITERIA.IN_CODE_ERROR.text.Replace("{0}", str) : (string) result;
    }
  }

  public static class Robots
  {
    public static class Models
    {
      public static readonly Tag SweepBot = TagManager.Create(nameof (SweepBot));
      public static readonly Tag ScoutRover = TagManager.Create(nameof (ScoutRover));
      public static readonly Tag MorbRover = TagManager.Create(nameof (MorbRover));
      public static readonly Tag FetchDrone = TagManager.Create(nameof (FetchDrone));
      public static readonly Tag RemoteWorker = TagManager.Create(nameof (RemoteWorker));
    }

    public static class Behaviours
    {
      public static readonly Tag UnloadBehaviour = TagManager.Create(nameof (UnloadBehaviour));
      public static readonly Tag RechargeBehaviour = TagManager.Create(nameof (RechargeBehaviour));
      public static readonly Tag EmoteBehaviour = TagManager.Create(nameof (EmoteBehaviour));
      public static readonly Tag TrappedBehaviour = TagManager.Create(nameof (TrappedBehaviour));
      public static readonly Tag NoElectroBank = TagManager.Create(nameof (NoElectroBank));
      public static readonly Tag HasDoorPermissions = TagManager.Create(nameof (HasDoorPermissions));
    }
  }

  public class Search
  {
    public static readonly Tag Tile = TagManager.Create(nameof (Tile));
    public static readonly Tag Ladder = TagManager.Create(nameof (Ladder));
    public static readonly Tag Powered = TagManager.Create(nameof (Powered));
    public static readonly Tag Rocket = TagManager.Create(nameof (Rocket));
    public static readonly Tag Monument = TagManager.Create(nameof (Monument));
    public static readonly Tag Farming = TagManager.Create(nameof (Farming));
    public static readonly Tag Cooking = TagManager.Create(nameof (Cooking));
  }
}
