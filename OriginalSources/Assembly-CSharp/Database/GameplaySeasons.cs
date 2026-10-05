// Decompiled with JetBrains decompiler
// Type: Database.GameplaySeasons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

#nullable disable
namespace Database;

public class GameplaySeasons : ResourceSet<GameplaySeason>
{
  public GameplaySeason NaturalRandomEvents;
  public GameplaySeason DupeRandomEvents;
  public GameplaySeason PrickleCropSeason;
  public GameplaySeason BonusEvents;
  public GameplaySeason MeteorShowers;
  public GameplaySeason TemporalTearMeteorShowers;
  public GameplaySeason SpacedOutStyleStartMeteorShowers;
  public GameplaySeason SpacedOutStyleRocketMeteorShowers;
  public GameplaySeason SpacedOutStyleWarpMeteorShowers;
  public GameplaySeason ClassicStyleStartMeteorShowers;
  public GameplaySeason ClassicStyleWarpMeteorShowers;
  public GameplaySeason TundraMoonletMeteorShowers;
  public GameplaySeason MarshyMoonletMeteorShowers;
  public GameplaySeason NiobiumMoonletMeteorShowers;
  public GameplaySeason WaterMoonletMeteorShowers;
  public GameplaySeason GassyMooteorShowers;
  public GameplaySeason RegolithMoonMeteorShowers;
  public GameplaySeason MiniMetallicSwampyMeteorShowers;
  public GameplaySeason MiniForestFrozenMeteorShowers;
  public GameplaySeason MiniBadlandsMeteorShowers;
  public GameplaySeason MiniFlippedMeteorShowers;
  public GameplaySeason MiniRadioactiveOceanMeteorShowers;
  public GameplaySeason MiniCeresStartShowers;
  public GameplaySeason CeresMeteorShowers;
  public GameplaySeason LargeImpactor;
  public GameplaySeason PrehistoricMeteorShowers;

  public GameplaySeasons(ResourceSet parent)
    : base(nameof (GameplaySeasons), parent)
  {
    this.VanillaSeasons();
    this.Expansion1Seasons();
    this.DLCSeasons();
    this.UnusedSeasons();
  }

  private void VanillaSeasons()
  {
    this.MeteorShowers = this.Add(new MeteorShowerSeason("MeteorShowers", GameplaySeason.Type.World, 14f, false, startActive: true).AddEvent(Db.Get().GameplayEvents.MeteorShowerIronEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerGoldEvent).AddEvent(Db.Get().GameplayEvents.MeteorShowerCopperEvent));
  }

  private void Expansion1Seasons()
  {
    this.RegolithMoonMeteorShowers = this.Add(new MeteorShowerSeason("RegolithMoonMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.MeteorShowerDustEvent).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
    this.TemporalTearMeteorShowers = this.Add(new MeteorShowerSeason("TemporalTearMeteorShowers", GameplaySeason.Type.World, 1f, false, 0.0f, affectedByDifficultySettings: false, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.MeteorShowerFullereneEvent));
    this.GassyMooteorShowers = this.Add(new MeteorShowerSeason("GassyMooteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, affectedByDifficultySettings: false, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.GassyMooteorEvent));
    this.SpacedOutStyleStartMeteorShowers = this.Add((GameplaySeason) new MeteorShowerSeason("SpacedOutStyleStartMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1));
    this.SpacedOutStyleRocketMeteorShowers = this.Add(new MeteorShowerSeason("SpacedOutStyleRocketMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
    this.SpacedOutStyleWarpMeteorShowers = this.Add(new MeteorShowerSeason("SpacedOutStyleWarpMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
    this.ClassicStyleStartMeteorShowers = this.Add(new MeteorShowerSeason("ClassicStyleStartMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIceShower).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower));
    this.ClassicStyleWarpMeteorShowers = this.Add(new MeteorShowerSeason("ClassicStyleWarpMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower));
    this.TundraMoonletMeteorShowers = this.Add((GameplaySeason) new MeteorShowerSeason("TundraMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1));
    this.MarshyMoonletMeteorShowers = this.Add((GameplaySeason) new MeteorShowerSeason("MarshyMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1));
    this.NiobiumMoonletMeteorShowers = this.Add((GameplaySeason) new MeteorShowerSeason("NiobiumMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1));
    this.WaterMoonletMeteorShowers = this.Add((GameplaySeason) new MeteorShowerSeason("WaterMoonletMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1));
    this.MiniMetallicSwampyMeteorShowers = this.Add(new MeteorShowerSeason("MiniMetallicSwampyMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.ClusterBiologicalShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
    this.MiniForestFrozenMeteorShowers = this.Add(new MeteorShowerSeason("MiniForestFrozenMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower));
    this.MiniBadlandsMeteorShowers = this.Add(new MeteorShowerSeason("MiniBadlandsMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.ClusterIceShower));
    this.MiniFlippedMeteorShowers = this.Add((GameplaySeason) new MeteorShowerSeason("MiniFlippedMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1));
    this.MiniRadioactiveOceanMeteorShowers = this.Add(new MeteorShowerSeason("MiniRadioactiveOceanMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1).AddEvent(Db.Get().GameplayEvents.ClusterUraniumShower));
  }

  private void DLCSeasons()
  {
    this.CeresMeteorShowers = this.Add(new MeteorShowerSeason("CeresMeteorShowers", GameplaySeason.Type.World, 20f, false, startActive: true, minCycle: 10f, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.DLC2).AddEvent(Db.Get().GameplayEvents.ClusterIceAndTreesShower));
    this.MiniCeresStartShowers = this.Add(new MeteorShowerSeason("MiniCeresStartShowers", GameplaySeason.Type.World, 20f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.EXPANSION1.Append<string>(DlcManager.DLC2)).AddEvent(Db.Get().GameplayEvents.ClusterOxyliteShower).AddEvent(Db.Get().GameplayEvents.ClusterSnowShower));
    this.LargeImpactor = this.Add(new GameplaySeason("LargeImpactor", GameplaySeason.Type.World, 1f, false, startActive: true, finishAfterNumEvents: 1, requiredDlcIds: DlcManager.DLC4).AddEvent(Db.Get().GameplayEvents.LargeImpactor));
    this.PrehistoricMeteorShowers = this.Add(new MeteorShowerSeason("PrehistoricMeteorShowers", GameplaySeason.Type.World, 50f, false, startActive: true, clusterTravelDuration: 6000f, requiredDlcIds: DlcManager.DLC4).AddEvent(Db.Get().GameplayEvents.ClusterCopperShower).AddEvent(Db.Get().GameplayEvents.ClusterIronShower).AddEvent(Db.Get().GameplayEvents.ClusterGoldShower));
  }

  private void UnusedSeasons()
  {
  }
}
