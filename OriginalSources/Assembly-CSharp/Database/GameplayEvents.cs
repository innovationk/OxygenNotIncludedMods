// Decompiled with JetBrains decompiler
// Type: Database.GameplayEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using TUNING;

#nullable disable
namespace Database;

public class GameplayEvents : ResourceSet<GameplayEvent>
{
  public GameplayEvent HatchSpawnEvent;
  public GameplayEvent PartyEvent;
  public GameplayEvent EclipseEvent;
  public GameplayEvent SatelliteCrashEvent;
  public GameplayEvent FoodFightEvent;
  public GameplayEvent PrickleFlowerBlightEvent;
  public GameplayEvent MeteorShowerIronEvent;
  public GameplayEvent MeteorShowerGoldEvent;
  public GameplayEvent MeteorShowerCopperEvent;
  public GameplayEvent MeteorShowerDustEvent;
  public GameplayEvent MeteorShowerFullereneEvent;
  public GameplayEvent GassyMooteorEvent;
  public GameplayEvent ClusterSnowShower;
  public GameplayEvent ClusterIceShower;
  public GameplayEvent ClusterBiologicalShower;
  public GameplayEvent ClusterLightRegolithShower;
  public GameplayEvent ClusterRegolithShower;
  public GameplayEvent ClusterGoldShower;
  public GameplayEvent ClusterCopperShower;
  public GameplayEvent ClusterIronShower;
  public GameplayEvent ClusterUraniumShower;
  public GameplayEvent ClusterOxyliteShower;
  public GameplayEvent ClusterBleachStoneShower;
  public GameplayEvent IridiumShowerEvent;
  public GameplayEvent ClusterIceAndTreesShower;
  public GameplayEvent BonusDream1;
  public GameplayEvent BonusDream2;
  public GameplayEvent BonusDream3;
  public GameplayEvent BonusDream4;
  public GameplayEvent BonusToilet1;
  public GameplayEvent BonusToilet2;
  public GameplayEvent BonusToilet3;
  public GameplayEvent BonusToilet4;
  public GameplayEvent BonusResearch;
  public GameplayEvent BonusDigging1;
  public GameplayEvent BonusStorage;
  public GameplayEvent BonusBuilder;
  public GameplayEvent BonusOxygen;
  public GameplayEvent BonusAlgae;
  public GameplayEvent BonusGenerator;
  public GameplayEvent BonusDoor;
  public GameplayEvent BonusHitTheBooks;
  public GameplayEvent BonusLitWorkspace;
  public GameplayEvent BonusTalker;
  public GameplayEvent CryoFriend;
  public GameplayEvent WarpWorldReveal;
  public GameplayEvent ArtifactReveal;
  public GameplayEvent LargeImpactor;

  public GameplayEvents(ResourceSet parent)
    : base(nameof (GameplayEvents), parent)
  {
    this.HatchSpawnEvent = this.Add((GameplayEvent) new CreatureSpawnEvent());
    this.PartyEvent = this.Add((GameplayEvent) new Klei.AI.PartyEvent());
    this.EclipseEvent = this.Add((GameplayEvent) new Klei.AI.EclipseEvent());
    this.SatelliteCrashEvent = this.Add((GameplayEvent) new Klei.AI.SatelliteCrashEvent());
    this.FoodFightEvent = this.Add((GameplayEvent) new Klei.AI.FoodFightEvent());
    this.BaseGameMeteorEvents();
    this.Expansion1MeteorEvents();
    this.DLCMeteorEvents();
    this.PrickleFlowerBlightEvent = this.Add((GameplayEvent) new PlantBlightEvent(nameof (PrickleFlowerBlightEvent), "PrickleFlower", 3600f, 30f));
    this.CryoFriend = this.Add((GameplayEvent) new SimpleEvent(nameof (CryoFriend), (string) GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.NAME, (string) GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.DESCRIPTION, "cryofriend_kanim", (string) GAMEPLAY_EVENTS.EVENT_TYPES.CRYOFRIEND.BUTTON));
    this.WarpWorldReveal = this.Add((GameplayEvent) new SimpleEvent(nameof (WarpWorldReveal), (string) GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.NAME, (string) GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.DESCRIPTION, "warpworldreveal_kanim", (string) GAMEPLAY_EVENTS.EVENT_TYPES.WARPWORLDREVEAL.BUTTON));
    this.ArtifactReveal = this.Add((GameplayEvent) new SimpleEvent(nameof (ArtifactReveal), (string) GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.NAME, (string) GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.DESCRIPTION, "analyzeartifact_kanim", (string) GAMEPLAY_EVENTS.EVENT_TYPES.ARTIFACT_REVEAL.BUTTON));
  }

  private void BaseGameMeteorEvents()
  {
    MathUtil.MinMax secondsBombardmentOn = new MathUtil.MinMax(50f, 100f);
    this.MeteorShowerGoldEvent = this.Add((GameplayEvent) new MeteorShowerEvent("MeteorShowerGoldEvent", 3000f, 0.4f, new MathUtil.MinMax(800f, 1200f), secondsBombardmentOn).AddMeteor(GoldCometConfig.ID, 2f).AddMeteor(RockCometConfig.ID, 0.5f).AddMeteor(DustCometConfig.ID, 5f));
    secondsBombardmentOn = new MathUtil.MinMax(100f, 400f);
    this.MeteorShowerCopperEvent = this.Add((GameplayEvent) new MeteorShowerEvent("MeteorShowerCopperEvent", 4200f, 5.5f, new MathUtil.MinMax(300f, 1200f), secondsBombardmentOn).AddMeteor(CopperCometConfig.ID, 1f).AddMeteor(RockCometConfig.ID, 1f));
    secondsBombardmentOn = new MathUtil.MinMax(100f, 400f);
    this.MeteorShowerIronEvent = this.Add((GameplayEvent) new MeteorShowerEvent("MeteorShowerIronEvent", 6000f, 1.25f, new MathUtil.MinMax(300f, 1200f), secondsBombardmentOn).AddMeteor(IronCometConfig.ID, 1f).AddMeteor(RockCometConfig.ID, 2f).AddMeteor(DustCometConfig.ID, 5f));
  }

  private void Expansion1MeteorEvents()
  {
    string fullId1 = ClusterMapMeteorShowerConfig.GetFullID("Regolith");
    MathUtil.MinMax secondsBombardmentOn = new MathUtil.MinMax(100f, 400f);
    this.MeteorShowerDustEvent = this.Add((GameplayEvent) new MeteorShowerEvent("MeteorShowerDustEvent", 9000f, 1.25f, new MathUtil.MinMax(300f, 1200f), secondsBombardmentOn, fullId1).AddMeteor(RockCometConfig.ID, 1f).AddMeteor(DustCometConfig.ID, 6f));
    string fullId2 = ClusterMapMeteorShowerConfig.GetFullID("Moo");
    secondsBombardmentOn = new MathUtil.MinMax(15f, 15f);
    this.GassyMooteorEvent = this.Add((GameplayEvent) new MeteorShowerEvent("GassyMooteorEvent", 15f, 3.125f, METEORS.BOMBARDMENT_OFF.NONE, secondsBombardmentOn, fullId2, false).AddMeteor(GassyMooCometConfig.ID, 1f));
    secondsBombardmentOn = new MathUtil.MinMax(80f, 80f);
    this.MeteorShowerFullereneEvent = this.Add((GameplayEvent) new MeteorShowerEvent("MeteorShowerFullereneEvent", 30f, 0.5f, METEORS.BOMBARDMENT_OFF.NONE, secondsBombardmentOn, affectedByDifficulty: false).AddMeteor(FullereneCometConfig.ID, 6f).AddMeteor(DustCometConfig.ID, 1f));
    string fullId3 = ClusterMapMeteorShowerConfig.GetFullID("Snow");
    MathUtil.MinMax unlimited1 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterSnowShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterSnowShower", 600f, 3f, METEORS.BOMBARDMENT_OFF.NONE, unlimited1, fullId3).AddMeteor(SnowballCometConfig.ID, 2f).AddMeteor(LightDustCometConfig.ID, 1f));
    string fullId4 = ClusterMapMeteorShowerConfig.GetFullID("Ice");
    MathUtil.MinMax unlimited2 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterIceShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterIceShower", 300f, 1.4f, METEORS.BOMBARDMENT_OFF.NONE, unlimited2, fullId4).AddMeteor(SnowballCometConfig.ID, 14f).AddMeteor(HardIceCometConfig.ID, 1f));
    string fullId5 = ClusterMapMeteorShowerConfig.GetFullID("Oxylite");
    MathUtil.MinMax unlimited3 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterOxyliteShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterOxyliteShower", 300f, 4f, METEORS.BOMBARDMENT_OFF.NONE, unlimited3, fullId5).AddMeteor(OxyliteCometConfig.ID, 4f).AddMeteor(LightDustCometConfig.ID, 4f));
    string fullId6 = ClusterMapMeteorShowerConfig.GetFullID("BleachStone");
    MathUtil.MinMax unlimited4 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterBleachStoneShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterBleachStoneShower", 300f, 3f, METEORS.BOMBARDMENT_OFF.NONE, unlimited4, fullId6).AddMeteor(BleachStoneCometConfig.ID, 13f).AddMeteor(LightDustCometConfig.ID, 3f));
    string fullId7 = ClusterMapMeteorShowerConfig.GetFullID("Biological");
    MathUtil.MinMax unlimited5 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterBiologicalShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterBiologicalShower", 300f, 3f, METEORS.BOMBARDMENT_OFF.NONE, unlimited5, fullId7).AddMeteor(SlimeCometConfig.ID, 2f).AddMeteor(AlgaeCometConfig.ID, 1f).AddMeteor(PhosphoricCometConfig.ID, 1f));
    string fullId8 = ClusterMapMeteorShowerConfig.GetFullID("LightDust");
    MathUtil.MinMax unlimited6 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterLightRegolithShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterLightRegolithShower", 300f, 4f, METEORS.BOMBARDMENT_OFF.NONE, unlimited6, fullId8).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 1f));
    string fullId9 = ClusterMapMeteorShowerConfig.GetFullID("HeavyDust");
    MathUtil.MinMax unlimited7 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterRegolithShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterRegolithShower", 300f, 3.5f, METEORS.BOMBARDMENT_OFF.NONE, unlimited7, fullId9).AddMeteor(DustCometConfig.ID, 3f).AddMeteor(RockCometConfig.ID, 2f).AddMeteor(LightDustCometConfig.ID, 1f));
    string fullId10 = ClusterMapMeteorShowerConfig.GetFullID("Gold");
    MathUtil.MinMax unlimited8 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterGoldShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterGoldShower", 75f, 1f, METEORS.BOMBARDMENT_OFF.NONE, unlimited8, fullId10).AddMeteor(GoldCometConfig.ID, 4f).AddMeteor(RockCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
    string fullId11 = ClusterMapMeteorShowerConfig.GetFullID("Copper");
    MathUtil.MinMax unlimited9 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterCopperShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterCopperShower", 150f, 2.5f, METEORS.BOMBARDMENT_OFF.NONE, unlimited9, fullId11).AddMeteor(CopperCometConfig.ID, 2f).AddMeteor(RockCometConfig.ID, 1f));
    string fullId12 = ClusterMapMeteorShowerConfig.GetFullID("Iron");
    MathUtil.MinMax unlimited10 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterIronShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterIronShower", 300f, 4.5f, METEORS.BOMBARDMENT_OFF.NONE, unlimited10, fullId12).AddMeteor(IronCometConfig.ID, 4f).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
    string fullId13 = ClusterMapMeteorShowerConfig.GetFullID("Uranium");
    MathUtil.MinMax unlimited11 = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterUraniumShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterUraniumShower", 150f, 4.5f, METEORS.BOMBARDMENT_OFF.NONE, unlimited11, fullId13).AddMeteor(UraniumCometConfig.ID, 2.5f).AddMeteor(DustCometConfig.ID, 1f).AddMeteor(LightDustCometConfig.ID, 2f));
  }

  private void DLCMeteorEvents()
  {
    string fullId = ClusterMapMeteorShowerConfig.GetFullID("IceAndTrees");
    MathUtil.MinMax secondsBombardmentOn = METEORS.BOMBARDMENT_ON.UNLIMITED;
    this.ClusterIceAndTreesShower = this.Add((GameplayEvent) new MeteorShowerEvent("ClusterIceAndTreesShower", 300f, 1.4f, METEORS.BOMBARDMENT_OFF.NONE, secondsBombardmentOn, fullId).AddMeteor(SpaceTreeSeedCometConfig.ID, 1f).AddMeteor(HardIceCometConfig.ID, 2f).AddMeteor(SnowballCometConfig.ID, 22f));
    this.LargeImpactor = this.Add((GameplayEvent) new LargeImpactorEvent("LargeImpactor", DlcManager.DLC4, (string[]) null));
    this.LargeImpactor.AddPrecondition(GameplayEventPreconditions.Instance.Or(GameplayEventPreconditions.Instance.Not(GameplayEventPreconditions.Instance.DifficultySetting(CustomGameSettingConfigs.DemoliorDifficulty, "Off")), GameplayEventPreconditions.Instance.ClusterHasTag("DemoliorImminentImpact")));
    secondsBombardmentOn = new MathUtil.MinMax(80f, 80f);
    this.IridiumShowerEvent = this.Add((GameplayEvent) new MeteorShowerEvent("IridiumShower", 30f, 0.5f, METEORS.BOMBARDMENT_OFF.NONE, secondsBombardmentOn).AddMeteor(IridiumCometConfig.ID, 1f));
  }

  private void BonusEvents()
  {
    GameplayEventMinionFilters instance1 = GameplayEventMinionFilters.Instance;
    GameplayEventPreconditions instance2 = GameplayEventPreconditions.Instance;
    Skills skills = Db.Get().Skills;
    RoomTypes roomTypes = Db.Get().RoomTypes;
    this.BonusDream1 = this.Add(new BonusEvent("BonusDream1").TriggerOnUseBuilding(1, "Bed", "LuxuryBed").SetRoomConstraints(false, roomTypes.Barracks).AddPrecondition(instance2.BuildingExists("Bed", 2)).AddPriorityBoost(instance2.BuildingExists("Bed", 5), 1).AddPriorityBoost(instance2.BuildingExists("LuxuryBed"), 5).TrySpawnEventOnSuccess((HashedString) "BonusDream2"));
    this.BonusDream2 = this.Add(new BonusEvent("BonusDream2", priority: 10).TriggerOnUseBuilding(10, "Bed", "LuxuryBed").AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusDream1)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom))).AddPriorityBoost(instance2.BuildingExists("LuxuryBed"), 5).TrySpawnEventOnSuccess((HashedString) "BonusDream3"));
    this.BonusDream3 = this.Add(new BonusEvent("BonusDream3", priority: 20).TriggerOnUseBuilding(10, "Bed", "LuxuryBed").AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusDream2)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom))).TrySpawnEventOnSuccess((HashedString) "BonusDream4"));
    this.BonusDream4 = this.Add(new BonusEvent("BonusDream4", priority: 30).TriggerOnUseBuilding(10, "LuxuryBed").AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusDream2)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Barracks), instance2.RoomBuilt(roomTypes.Bedroom))));
    this.BonusToilet1 = this.Add(new BonusEvent("BonusToilet1").TriggerOnUseBuilding(1, "Outhouse", "FlushToilet").AddPrecondition(instance2.Or(instance2.BuildingExists("Outhouse", 2), instance2.BuildingExists("FlushToilet"))).AddPrecondition(instance2.Or(instance2.BuildingExists("WashBasin", 2), instance2.BuildingExists("WashSink"))).AddPriorityBoost(instance2.BuildingExists("FlushToilet"), 1).TrySpawnEventOnSuccess((HashedString) "BonusToilet2"));
    this.BonusToilet2 = this.Add(new BonusEvent("BonusToilet2", priority: 10).TriggerOnUseBuilding(5, "FlushToilet").AddPrecondition(instance2.BuildingExists("FlushToilet")).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusToilet1)).AddPriorityBoost(instance2.BuildingExists("FlushToilet", 2), 5).TrySpawnEventOnSuccess((HashedString) "BonusToilet3"));
    this.BonusToilet3 = this.Add(new BonusEvent("BonusToilet3", priority: 20).TriggerOnUseBuilding(5, "FlushToilet").SetRoomConstraints(false, roomTypes.Latrine, roomTypes.PlumbedBathroom).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusToilet2)).AddPrecondition(instance2.Or(instance2.RoomBuilt(roomTypes.Latrine), instance2.RoomBuilt(roomTypes.PlumbedBathroom))).AddPriorityBoost(instance2.BuildingExists("FlushToilet", 2), 10).TrySpawnEventOnSuccess((HashedString) "BonusToilet4"));
    this.BonusToilet4 = this.Add(new BonusEvent("BonusToilet4", priority: 30).TriggerOnUseBuilding(5, "FlushToilet").SetRoomConstraints(false, roomTypes.PlumbedBathroom).AddPrecondition(instance2.PastEventCountAndNotActive(this.BonusToilet3)).AddPrecondition(instance2.RoomBuilt(roomTypes.PlumbedBathroom)));
    this.BonusResearch = this.Add(new BonusEvent("BonusResearch").AddPrecondition(instance2.BuildingExists("ResearchCenter")).AddPrecondition(instance2.ResearchCompleted("FarmingTech")).AddMinionFilter(instance1.HasSkillAptitude(skills.Researching1)));
    this.BonusDigging1 = this.Add(new BonusEvent("BonusDigging1", preSelectMinion: true).TriggerOnWorkableComplete(30, typeof (Diggable)).AddMinionFilter(instance1.Or(instance1.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Dig, 4), instance1.HasSkillAptitude(skills.Mining1))).AddPriorityBoost(instance2.MinionsWithChoreGroupPriorityOrGreater(Db.Get().ChoreGroups.Dig, 1, 4), 1));
    this.BonusStorage = this.Add(new BonusEvent("BonusStorage", preSelectMinion: true).TriggerOnUseBuilding(10, "StorageLocker").AddMinionFilter(instance1.Or(instance1.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Hauling, 4), instance1.HasSkillAptitude(skills.Hauling1))).AddPrecondition(instance2.BuildingExists("StorageLocker")));
    this.BonusBuilder = this.Add(new BonusEvent("BonusBuilder", preSelectMinion: true).TriggerOnNewBuilding(10).AddMinionFilter(instance1.Or(instance1.HasChoreGroupPriorityOrHigher(Db.Get().ChoreGroups.Build, 4), instance1.HasSkillAptitude(skills.Building1))));
    this.BonusOxygen = this.Add(new BonusEvent("BonusOxygen").TriggerOnUseBuilding(1, "MineralDeoxidizer").AddPrecondition(instance2.BuildingExists("MineralDeoxidizer")).AddPrecondition(instance2.Not(instance2.PastEventCount("BonusAlgae"))));
    this.BonusAlgae = this.Add(new BonusEvent("BonusAlgae", "BonusOxygen").TriggerOnUseBuilding(1, "AlgaeHabitat").AddPrecondition(instance2.BuildingExists("AlgaeHabitat")).AddPrecondition(instance2.Not(instance2.PastEventCount("BonusOxygen"))));
    this.BonusGenerator = this.Add(new BonusEvent("BonusGenerator").TriggerOnUseBuilding(1, "ManualGenerator").AddPrecondition(instance2.BuildingExists("ManualGenerator")));
    this.BonusDoor = this.Add(new BonusEvent("BonusDoor").TriggerOnUseBuilding(1, "Door").SetExtraCondition((BonusEvent.ConditionFn) (data => data.building.GetComponent<Door>().RequestedState == Door.ControlState.Locked)).AddPrecondition(instance2.RoomBuilt(roomTypes.Barracks)));
    this.BonusHitTheBooks = this.Add(new BonusEvent("BonusHitTheBooks", preSelectMinion: true).TriggerOnWorkableComplete(1, typeof (ResearchCenter), typeof (NuclearResearchCenterWorkable)).AddPrecondition(instance2.BuildingExists("ResearchCenter")).AddMinionFilter(instance1.HasSkillAptitude(skills.Researching1)));
    this.BonusLitWorkspace = this.Add(new BonusEvent("BonusLitWorkspace").TriggerOnWorkableComplete(1).SetExtraCondition((BonusEvent.ConditionFn) (data => data.workable.currentlyLit)).AddPrecondition(instance2.CycleRestriction(10f)));
    this.BonusTalker = this.Add(new BonusEvent("BonusTalker", preSelectMinion: true).TriggerOnWorkableComplete(3, typeof (SocialGatheringPointWorkable)).SetExtraCondition((BonusEvent.ConditionFn) (data => (data.workable as SocialGatheringPointWorkable).timesConversed > 0)).AddPrecondition(instance2.CycleRestriction(10f)));
  }

  private void VerifyEvents()
  {
    foreach (GameplayEvent resource in this.resources)
    {
      if (resource.animFileName == (HashedString) (string) null)
        DebugUtil.LogWarningArgs((object) ("Gameplay event anim missing: " + resource.Id));
      if (resource is BonusEvent)
        this.VerifyBonusEvent(resource as BonusEvent);
    }
  }

  private void VerifyBonusEvent(BonusEvent e)
  {
    StringEntry result;
    if (!Strings.TryGet($"STRINGS.GAMEPLAY_EVENTS.BONUS.{e.Id.ToUpper()}.NAME", out result))
      DebugUtil.DevLogError($"Event [{e.Id}]: STRINGS.GAMEPLAY_EVENTS.BONUS.{e.Id.ToUpper()} is missing");
    Effect effect = Db.Get().effects.TryGet(e.effect);
    if (effect == null)
    {
      DebugUtil.DevLogError($"Effect {e.effect}[{e.Id}]: Missing from spreadsheet");
    }
    else
    {
      if (!Strings.TryGet($"STRINGS.DUPLICANTS.MODIFIERS.{effect.Id.ToUpper()}.NAME", out result))
        DebugUtil.DevLogError($"Effect {e.effect}[{e.Id}]: STRINGS.DUPLICANTS.MODIFIERS.{effect.Id.ToUpper()}.NAME is missing");
      if (Strings.TryGet($"STRINGS.DUPLICANTS.MODIFIERS.{effect.Id.ToUpper()}.TOOLTIP", out result))
        return;
      DebugUtil.DevLogError($"Effect {e.effect}[{e.Id}]: STRINGS.DUPLICANTS.MODIFIERS.{effect.Id.ToUpper()}.TOOLTIP is missing");
    }
  }
}
