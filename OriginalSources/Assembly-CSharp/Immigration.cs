// Decompiled with JetBrains decompiler
// Type: Immigration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Immigration")]
public class Immigration : KMonoBehaviour, ISaveLoadable, ISim200ms, IPersonalPriorityManager
{
  public float[] spawnInterval;
  public int[] spawnTable;
  [Serialize]
  private Dictionary<HashedString, int> defaultPersonalPriorities = new Dictionary<HashedString, int>();
  [Serialize]
  public float timeBeforeSpawn = float.PositiveInfinity;
  [Serialize]
  private bool bImmigrantAvailable;
  [Serialize]
  private int spawnIdx;
  private List<CarePackageInfo> carePackages;
  private Dictionary<string, List<CarePackageInfo>> carePackagesByDlc;
  public static Immigration Instance;
  private const int CYCLE_THRESHOLD_A = 6;
  private const int CYCLE_THRESHOLD_B = 12;
  private const int CYCLE_THRESHOLD_C = 24;
  private const int CYCLE_THRESHOLD_D = 48 /*0x30*/;
  private const int CYCLE_THRESHOLD_E = 100;
  private const int CYCLE_THRESHOLD_UNLOCK_EVERYTHING = 500;
  public const string FACADE_SELECT_RANDOM = "SELECTRANDOM";

  public static void DestroyInstance() => Immigration.Instance = (Immigration) null;

  protected override void OnPrefabInit()
  {
    this.bImmigrantAvailable = false;
    Immigration.Instance = this;
    this.timeBeforeSpawn = this.spawnInterval[Math.Min(this.spawnIdx, this.spawnInterval.Length - 1)];
    this.SetupDLCCarePackages();
    this.ResetPersonalPriorities();
    this.ConfigureCarePackages();
  }

  private void SetupDLCCarePackages()
  {
    this.carePackagesByDlc = new Dictionary<string, List<CarePackageInfo>>()
    {
      {
        "DLC2_ID",
        new List<CarePackageInfo>()
        {
          new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cinnabar).tag.ToString(), 2000f, (Func<bool>) (() => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cinnabar).tag))),
          new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.WoodLog).tag.ToString(), 200f, (Func<bool>) (() => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.WoodLog).tag))),
          new CarePackageInfo("WoodDeerBaby", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
          new CarePackageInfo("SealBaby", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("IceBellyEgg", 1f, (Func<bool>) (() => Immigration.CycleCondition(100))),
          new CarePackageInfo("Pemmican", 3f, (Func<bool>) null),
          new CarePackageInfo("FriesCarrot", 3f, (Func<bool>) (() => Immigration.CycleCondition(24))),
          new CarePackageInfo("IceFlowerSeed", 3f, (Func<bool>) null),
          new CarePackageInfo("BlueGrassSeed", 1f, (Func<bool>) null),
          new CarePackageInfo("CarrotPlantSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
          new CarePackageInfo("SpaceTreeSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
          new CarePackageInfo("HardSkinBerryPlantSeed", 3f, (Func<bool>) null)
        }
      },
      {
        "DLC3_ID",
        new List<CarePackageInfo>()
        {
          new CarePackageInfo("DisposableElectrobank_RawMetal", 3f, (Func<bool>) (() => Immigration.CycleCondition(12)))
        }
      },
      {
        "DLC4_ID",
        new List<CarePackageInfo>()
        {
          new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Peat).tag.ToString(), 3000f, (Func<bool>) null),
          new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.NickelOre).tag.ToString(), 2000f, (Func<bool>) (() => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.NickelOre).tag))),
          new CarePackageInfo("GardenFoodPlantSeed", 1f, (Func<bool>) null),
          new CarePackageInfo("GardenDecorPlantSeed", 1f, (Func<bool>) null),
          new CarePackageInfo("ButterflyPlantSeed", 1f, (Func<bool>) null),
          new CarePackageInfo("DinofernSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("DewDripperPlantSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("KelpPlantSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("FlyTrapPlantSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("VineMotherSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("GardenForagePlant", 3f, (Func<bool>) null),
          new CarePackageInfo(VineFruitConfig.ID, 6f, (Func<bool>) null),
          new CarePackageInfo("SmokedDinosaurMeat", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("StegoBaby", 1f, (Func<bool>) null),
          new CarePackageInfo("ChameleonEgg", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("MosquitoEgg", 3f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("PrehistoricPacuEgg", 1f, (Func<bool>) (() => Immigration.CycleCondition(100))),
          new CarePackageInfo("RaptorEgg", 1f, (Func<bool>) (() => Immigration.CycleCondition(100)))
        }
      },
      {
        "DLC5_ID",
        new List<CarePackageInfo>()
        {
          new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Pearl).tag.ToString(), 300f, (Func<bool>) null),
          new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.ZincOre).tag.ToString(), 2000f, (Func<bool>) (() => Immigration.CycleCondition(12))),
          new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Corallium).tag.ToString(), 4000f, (Func<bool>) null),
          new CarePackageInfo("ParrotFish", 5f, (Func<bool>) (() => Immigration.CycleCondition(24))),
          new CarePackageInfo("PufferFish", 3f, (Func<bool>) (() => Immigration.CycleCondition(24))),
          new CarePackageInfo("SeaTurtleBaby", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("SquidEgg", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo(RubberBootsConfig.ID, 1f, (Func<bool>) null),
          new CarePackageInfo("DrySuit", 1f, (Func<bool>) null),
          new CarePackageInfo("RubberGasket", 1f, (Func<bool>) null),
          new CarePackageInfo("Lettuce", 3f, (Func<bool>) (() => Immigration.CycleCondition(24))),
          new CarePackageInfo("SeaLettuceSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("OxyCoralSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("ClamSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("UrchinPlantSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo("BulbloomSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
          new CarePackageInfo(DewPalmConfig.SEED_ID, 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/)))
        }
      }
    };
    foreach (KeyValuePair<Tag, BionicUpgradeComponentConfig.BionicUpgradeData> keyValuePair in BionicUpgradeComponentConfig.UpgradesData)
    {
      if (keyValuePair.Value.isCarePackage)
        this.carePackagesByDlc["DLC3_ID"].Add(new CarePackageInfo(keyValuePair.Key.Name, 1f, (Func<bool>) (() => Immigration.HasMinionModelCondition(BionicMinionConfig.MODEL))));
    }
  }

  private void ConfigureCarePackages()
  {
    if (DlcManager.FeatureClusterSpaceEnabled())
      this.ConfigureMultiWorldCarePackages();
    else
      this.ConfigureBaseGameCarePackages();
    foreach (string dlcId in SaveLoader.Instance.GameInfo.dlcIds)
    {
      if (this.carePackagesByDlc.ContainsKey(dlcId))
        this.carePackages.AddRange((IEnumerable<CarePackageInfo>) this.carePackagesByDlc[dlcId]);
    }
  }

  private void ConfigureBaseGameCarePackages()
  {
    this.carePackages = new List<CarePackageInfo>()
    {
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SandStone).tag.ToString(), 1000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Dirt).tag.ToString(), 500f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Algae).tag.ToString(), 500f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag.ToString(), 100f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Water).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Sand).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Carbon).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Fertilizer).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ice).tag.ToString(), 4000f, (Func<bool>) (() => Immigration.CycleCondition(12))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Brine).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SaltWater).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Rust).tag.ToString(), 1000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag.ToString(), 2000f, (Func<bool>) (() => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag.ToString(), 2000f, (Func<bool>) (() => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Copper).tag.ToString(), 400f, (Func<bool>) (() => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Copper).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Iron).tag.ToString(), 400f, (Func<bool>) (() => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Iron).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Lime).tag.ToString(), 150f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Lime).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag.ToString(), 500f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Glass).tag.ToString(), 200f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Glass).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Steel).tag.ToString(), 100f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Steel).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag.ToString(), 100f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag.ToString(), 100f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag))),
      new CarePackageInfo("PrickleGrassSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("LeafyPlantSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("CactusPlantSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("MushroomSeed", 1f, (Func<bool>) null),
      new CarePackageInfo("PrickleFlowerSeed", 2f, (Func<bool>) null),
      new CarePackageInfo("OxyfernSeed", 1f, (Func<bool>) null),
      new CarePackageInfo("ForestTreeSeed", 1f, (Func<bool>) null),
      new CarePackageInfo(BasicFabricMaterialPlantConfig.SEED_ID, 3f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("SwampLilySeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("ColdBreatherSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("SpiceVineSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("SaltPlantSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("BasicSingleHarvestPlantSeed", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("FieldRation", 5f, (Func<bool>) null),
      new CarePackageInfo("BasicForagePlant", 6f, (Func<bool>) null),
      new CarePackageInfo("CookedEgg", 3f, (Func<bool>) (() => Immigration.CycleCondition(6))),
      new CarePackageInfo(PrickleFruitConfig.ID, 3f, (Func<bool>) (() => Immigration.CycleCondition(12))),
      new CarePackageInfo("FriedMushroom", 3f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("CookedMeat", 3f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
      new CarePackageInfo("SpicyTofu", 3f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
      new CarePackageInfo("LightBugBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("HatchBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("PuftBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("SquirrelBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("CrabBaby", 1f, (Func<bool>) null),
      new CarePackageInfo("DreckoBaby", 1f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("Pacu", 8f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("MoleBaby", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
      new CarePackageInfo("OilfloaterBaby", 1f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
      new CarePackageInfo("LightBugEgg", 3f, (Func<bool>) null),
      new CarePackageInfo("HatchEgg", 3f, (Func<bool>) null),
      new CarePackageInfo("PuftEgg", 3f, (Func<bool>) null),
      new CarePackageInfo("OilfloaterEgg", 3f, (Func<bool>) (() => Immigration.CycleCondition(12))),
      new CarePackageInfo("MoleEgg", 3f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("DreckoEgg", 3f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("SquirrelEgg", 2f, (Func<bool>) null),
      new CarePackageInfo("BasicCure", 3f, (Func<bool>) null),
      new CarePackageInfo("CustomClothing", 1f, (Func<bool>) null, "SELECTRANDOM"),
      new CarePackageInfo("Funky_Vest", 1f, (Func<bool>) null)
    };
  }

  private void ConfigureMultiWorldCarePackages()
  {
    this.carePackages = new List<CarePackageInfo>()
    {
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SandStone).tag.ToString(), 1000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Dirt).tag.ToString(), 500f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Algae).tag.ToString(), 500f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag.ToString(), 100f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Water).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Sand).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Carbon).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Fertilizer).tag.ToString(), 3000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ice).tag.ToString(), 4000f, (Func<bool>) (() => Immigration.CycleCondition(12))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Brine).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.SaltWater).tag.ToString(), 2000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Rust).tag.ToString(), 1000f, (Func<bool>) null),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag.ToString(), 2000f, (Func<bool>) (() => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Cuprite).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag.ToString(), 2000f, (Func<bool>) (() => Immigration.CycleCondition(12) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.GoldAmalgam).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Copper).tag.ToString(), 400f, (Func<bool>) (() => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Copper).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Iron).tag.ToString(), 400f, (Func<bool>) (() => Immigration.CycleCondition(24) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Iron).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Lime).tag.ToString(), 150f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Lime).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag.ToString(), 500f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Polypropylene).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Glass).tag.ToString(), 200f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Glass).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Steel).tag.ToString(), 100f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Steel).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag.ToString(), 100f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.Ethanol).tag))),
      new CarePackageInfo(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag.ToString(), 100f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/) && Immigration.DiscoveredCondition(ElementLoader.FindElementByHash(SimHashes.AluminumOre).tag))),
      new CarePackageInfo("PrickleGrassSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("LeafyPlantSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("CactusPlantSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("WineCupsSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("CylindricaSeed", 3f, (Func<bool>) null),
      new CarePackageInfo("MushroomSeed", 1f, (Func<bool>) null),
      new CarePackageInfo("PrickleFlowerSeed", 2f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "PrickleFlowerSeed") || Immigration.CycleCondition(500))),
      new CarePackageInfo("OxyfernSeed", 1f, (Func<bool>) null),
      new CarePackageInfo("BasicSingleHarvestPlantSeed", 1f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "BasicSingleHarvestPlantSeed") || Immigration.CycleCondition(500))),
      new CarePackageInfo("ForestTreeSeed", 1f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "ForestTreeSeed") || Immigration.CycleCondition(500))),
      new CarePackageInfo(BasicFabricMaterialPlantConfig.SEED_ID, 3f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) BasicFabricMaterialPlantConfig.SEED_ID) || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("SwampLilySeed", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "SwampLilySeed") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("ColdBreatherSeed", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "ColdBreatherSeed") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("SpiceVineSeed", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "SpiceVineSeed") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("WormPlantSeed", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "WormPlantSeed") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("SaltPlantSeed", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "SaltPlantSeed") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("FieldRation", 5f, (Func<bool>) null),
      new CarePackageInfo("BasicForagePlant", 6f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "BasicForagePlant"))),
      new CarePackageInfo("ForestForagePlant", 2f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "ForestForagePlant"))),
      new CarePackageInfo("SwampForagePlant", 2f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "SwampForagePlant"))),
      new CarePackageInfo("CookedEgg", 3f, (Func<bool>) (() => Immigration.CycleCondition(6))),
      new CarePackageInfo(PrickleFruitConfig.ID, 3f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(12))
          return false;
        return Immigration.DiscoveredCondition((Tag) PrickleFruitConfig.ID) || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("FriedMushroom", 3f, (Func<bool>) (() => Immigration.CycleCondition(24))),
      new CarePackageInfo("CookedMeat", 3f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
      new CarePackageInfo("SpicyTofu", 3f, (Func<bool>) (() => Immigration.CycleCondition(48 /*0x30*/))),
      new CarePackageInfo("WormSuperFood", 2f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "WormPlantSeed") || Immigration.CycleCondition(500))),
      new CarePackageInfo("LightBugBaby", 1f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "LightBugEgg") || Immigration.CycleCondition(500))),
      new CarePackageInfo("HatchBaby", 1f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "HatchEgg") || Immigration.CycleCondition(500))),
      new CarePackageInfo("PuftBaby", 1f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "PuftEgg") || Immigration.CycleCondition(500))),
      new CarePackageInfo("SquirrelBaby", 1f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "SquirrelEgg") || Immigration.CycleCondition(24) || Immigration.CycleCondition(500))),
      new CarePackageInfo("CrabBaby", 1f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "CrabEgg") || Immigration.CycleCondition(500))),
      new CarePackageInfo("DreckoBaby", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "DreckoEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("Pacu", 8f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "PacuEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("MoleBaby", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(48 /*0x30*/))
          return false;
        return Immigration.DiscoveredCondition((Tag) "MoleEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("OilfloaterBaby", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(48 /*0x30*/))
          return false;
        return Immigration.DiscoveredCondition((Tag) "OilfloaterEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("DivergentBeetleBaby", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(48 /*0x30*/))
          return false;
        return Immigration.DiscoveredCondition((Tag) "DivergentBeetleEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("StaterpillarBaby", 1f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(48 /*0x30*/))
          return false;
        return Immigration.DiscoveredCondition((Tag) "StaterpillarEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("LightBugEgg", 3f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "LightBugEgg") || Immigration.CycleCondition(500))),
      new CarePackageInfo("HatchEgg", 3f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "HatchEgg") || Immigration.CycleCondition(500))),
      new CarePackageInfo("PuftEgg", 3f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "PuftEgg") || Immigration.CycleCondition(500))),
      new CarePackageInfo("OilfloaterEgg", 3f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(12))
          return false;
        return Immigration.DiscoveredCondition((Tag) "OilfloaterEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("MoleEgg", 3f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "MoleEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("DreckoEgg", 3f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(24))
          return false;
        return Immigration.DiscoveredCondition((Tag) "DreckoEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("SquirrelEgg", 2f, (Func<bool>) (() => Immigration.DiscoveredCondition((Tag) "SquirrelEgg") || Immigration.CycleCondition(24) || Immigration.CycleCondition(500))),
      new CarePackageInfo("DivergentBeetleEgg", 2f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(48 /*0x30*/))
          return false;
        return Immigration.DiscoveredCondition((Tag) "DivergentBeetleEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("StaterpillarEgg", 2f, (Func<bool>) (() =>
      {
        if (!Immigration.CycleCondition(48 /*0x30*/))
          return false;
        return Immigration.DiscoveredCondition((Tag) "StaterpillarEgg") || Immigration.CycleCondition(500);
      })),
      new CarePackageInfo("BasicCure", 3f, (Func<bool>) null),
      new CarePackageInfo("CustomClothing", 1f, (Func<bool>) null, "SELECTRANDOM"),
      new CarePackageInfo("Funky_Vest", 1f, (Func<bool>) null)
    };
  }

  private static bool CycleCondition(int cycle) => GameClock.Instance.GetCycle() >= cycle;

  private static bool DiscoveredCondition(Tag tag)
  {
    return DiscoveredResources.Instance.IsDiscovered(tag);
  }

  private static bool HasMinionModelCondition(Tag model)
  {
    Components.Cmps<MinionIdentity> cmps;
    return Components.LiveMinionIdentitiesByModel.TryGetValue(model, out cmps) && cmps.Count > 0;
  }

  public bool ImmigrantsAvailable => this.bImmigrantAvailable;

  public int EndImmigration()
  {
    this.bImmigrantAvailable = false;
    ++this.spawnIdx;
    int index = Math.Min(this.spawnIdx, this.spawnInterval.Length - 1);
    this.timeBeforeSpawn = this.spawnInterval[index];
    return this.spawnTable[index];
  }

  public float GetTimeRemaining() => this.timeBeforeSpawn;

  public float GetTotalWaitTime()
  {
    return this.spawnInterval[Math.Min(this.spawnIdx, this.spawnInterval.Length - 1)];
  }

  public void Sim200ms(float dt)
  {
    if (this.IsHalted() || this.bImmigrantAvailable)
      return;
    this.timeBeforeSpawn -= dt;
    this.timeBeforeSpawn = Math.Max(this.timeBeforeSpawn, 0.0f);
    if ((double) this.timeBeforeSpawn > 0.0)
      return;
    this.bImmigrantAvailable = true;
  }

  private bool IsHalted()
  {
    foreach (Component component1 in Components.Telepads.Items)
    {
      Operational component2 = component1.GetComponent<Operational>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.IsOperational)
        return false;
    }
    return true;
  }

  public int GetPersonalPriority(ChoreGroup group)
  {
    int personalPriority;
    if (!this.defaultPersonalPriorities.TryGetValue(group.IdHash, out personalPriority))
      personalPriority = 3;
    return personalPriority;
  }

  public CarePackageInfo RandomCarePackage()
  {
    List<CarePackageInfo> carePackageInfoList = new List<CarePackageInfo>();
    foreach (CarePackageInfo carePackage in this.carePackages)
    {
      if (carePackage.requirement == null || carePackage.requirement())
        carePackageInfoList.Add(carePackage);
    }
    return carePackageInfoList[UnityEngine.Random.Range(0, carePackageInfoList.Count)];
  }

  public void SetPersonalPriority(ChoreGroup group, int value)
  {
    this.defaultPersonalPriorities[group.IdHash] = value;
  }

  public int GetAssociatedSkillLevel(ChoreGroup group) => 0;

  public void ApplyDefaultPersonalPriorities(GameObject minion)
  {
    IPersonalPriorityManager instance = (IPersonalPriorityManager) Immigration.Instance;
    IPersonalPriorityManager component = (IPersonalPriorityManager) minion.GetComponent<ChoreConsumer>();
    foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
    {
      int personalPriority = instance.GetPersonalPriority(resource);
      component.SetPersonalPriority(resource, personalPriority);
    }
  }

  public void ResetPersonalPriorities()
  {
    bool personalPriorities = Game.Instance.advancedPersonalPriorities;
    foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
      this.defaultPersonalPriorities[resource.IdHash] = personalPriorities ? resource.DefaultPersonalPriority : 3;
  }

  public bool IsChoreGroupDisabled(ChoreGroup g) => false;
}
