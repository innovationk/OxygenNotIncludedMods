// Decompiled with JetBrains decompiler
// Type: TUNING.CREATURES
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace TUNING;

public class CREATURES
{
  public const float WILD_GROWTH_RATE_MODIFIER = 0.25f;
  public const int DEFAULT_PROBING_RADIUS = 32 /*0x20*/;
  public const float CREATURES_BASE_GENERATION_KILOWATTS = 10f;
  public const float FERTILITY_TIME_BY_LIFESPAN = 0.6f;
  public const float INCUBATION_TIME_BY_LIFESPAN = 0.2f;
  public const float INCUBATOR_INCUBATION_MULTIPLIER = 4f;
  public const float WILD_CALORIE_BURN_RATIO = 0.25f;
  public const float HUG_INCUBATION_MULTIPLIER = 1f;
  public const float VIABILITY_LOSS_RATE = -0.0166666675f;
  public const float STATERPILLAR_POWER_CHARGE_LOSS_RATE = -0.055555556f;
  public const float HUNT_FAILED_DURATION = 45f;
  public const float EVADED_HUNT_DURATION = 10f;

  public class HITPOINTS
  {
    public const float TIER0 = 5f;
    public const float TIER1 = 25f;
    public const float TIER2 = 50f;
    public const float TIER3 = 100f;
    public const float TIER4 = 150f;
    public const float TIER5 = 200f;
    public const float TIER6 = 400f;
  }

  public class MASS_KG
  {
    public const float TIER0 = 5f;
    public const float TIER1 = 25f;
    public const float TIER2 = 50f;
    public const float TIER3 = 100f;
    public const float TIER4 = 200f;
    public const float TIER5 = 400f;
  }

  public class TEMPERATURE
  {
    public const float SKIN_THICKNESS = 0.025f;
    public const float SURFACE_AREA = 17.5f;
    public const float GROUND_TRANSFER_SCALE = 0.0f;
    public static float FREEZING_10 = 173f;
    public static float FREEZING_9 = 183f;
    public static float FREEZING_3 = 243f;
    public static float FREEZING_2 = 253f;
    public static float FREEZING_1 = 263f;
    public static float FREEZING = 273f;
    public static float COOL = 283f;
    public static float MODERATE = 293f;
    public static float HOT = 303f;
    public static float HOT_1 = 313f;
    public static float HOT_2 = 323f;
    public static float HOT_3 = 333f;
    public static float HOT_7 = 373f;
  }

  public class LIFESPAN
  {
    public const float TIER0 = 5f;
    public const float TIER1 = 25f;
    public const float TIER2 = 75f;
    public const float TIER3 = 100f;
    public const float TIER4 = 150f;
    public const float TIER5 = 200f;
    public const float TIER6 = 400f;
  }

  public class CONVERSION_EFFICIENCY
  {
    public static float BAD_2 = 0.1f;
    public static float BAD_1 = 0.25f;
    public static float NORMAL = 0.5f;
    public static float GOOD_0 = 0.6666667f;
    public static float GOOD_1 = 0.75f;
    public static float GOOD_2 = 0.95f;
    public static float GOOD_3 = 1f;
  }

  public class SPACE_REQUIREMENTS
  {
    public static int TIER1 = 4;
    public static int TIER2 = 8;
    public static int TIER3 = 12;
    public static int TIER4 = 16 /*0x10*/;
  }

  public class EGG_CHANCE_MODIFIERS
  {
    public static List<System.Action> MODIFIER_CREATORS;

    private static System.Action CreateDietaryModifier(
      string id,
      Tag eggTag,
      HashSet<Tag> foodTags,
      float modifierPerCal)
    {
      return (System.Action) (() =>
      {
        string name = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.DIET.NAME;
        string desc = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.DIET.DESC;
        Db.Get().CreateFertilityModifier(id, eggTag, name, desc, (Func<string, string>) (descStr =>
        {
          string str = string.Join(", ", foodTags.Select<Tag, string>((Func<Tag, string>) (t => t.ProperName())).ToArray<string>());
          descStr = string.Format(descStr, (object) str);
          return descStr;
        }), (FertilityModifier.FertilityModFn) ((inst, eggType) => inst.gameObject.Subscribe(-2038961714, (Action<object>) (data =>
        {
          CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>) data).value;
          if (!foodTags.Contains(caloriesConsumedEvent.tag))
            return;
          inst.AddBreedingChance(eggType, caloriesConsumedEvent.calories * modifierPerCal);
        }))));
      });
    }

    private static System.Action CreateDietaryModifier(
      string id,
      Tag eggTag,
      Tag foodTag,
      float modifierPerCal)
    {
      string id1 = id;
      Tag eggTag1 = eggTag;
      HashSet<Tag> foodTags = new HashSet<Tag>();
      foodTags.Add(foodTag);
      double modifierPerCal1 = (double) modifierPerCal;
      return CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier(id1, eggTag1, foodTags, (float) modifierPerCal1);
    }

    private static System.Action CreateNearbyCreatureModifier(
      string id,
      Tag eggTag,
      Tag nearbyCreatureBaby,
      Tag nearbyCreatureAdult,
      float modifierPerSecond,
      bool alsoInvert)
    {
      return (System.Action) (() =>
      {
        string name = (string) ((double) modifierPerSecond < 0.0 ? STRINGS.CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.NAME : STRINGS.CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.NAME);
        string description = (string) ((double) modifierPerSecond < 0.0 ? STRINGS.CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.DESC : STRINGS.CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.DESC);
        Db.Get().CreateFertilityModifier(id, eggTag, name, description, (Func<string, string>) (descStr => string.Format(descStr, (object) nearbyCreatureAdult.ProperName())), (FertilityModifier.FertilityModFn) ((inst, eggType) =>
        {
          NearbyCreatureMonitor.Instance instance = inst.gameObject.GetSMI<NearbyCreatureMonitor.Instance>();
          if (instance == null)
          {
            instance = new NearbyCreatureMonitor.Instance(inst.master);
            instance.StartSM();
          }
          instance.OnUpdateNearbyCreatures += (Action<float, List<KPrefabID>, List<KPrefabID>>) ((dt, creatures, eggs) =>
          {
            bool flag = false;
            foreach (KPrefabID creature in creatures)
            {
              if (creature.PrefabTag == nearbyCreatureBaby || creature.PrefabTag == nearbyCreatureAdult)
              {
                flag = true;
                break;
              }
            }
            if (flag)
            {
              inst.AddBreedingChance(eggType, dt * modifierPerSecond);
            }
            else
            {
              if (!alsoInvert)
                return;
              inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
            }
          });
        }));
      });
    }

    private static System.Action CreateElementCreatureModifier(
      string id,
      Tag eggTag,
      Tag element,
      float modifierPerSecond,
      bool alsoInvert,
      bool checkSubstantialLiquid,
      string tooltipOverride = null)
    {
      return (System.Action) (() =>
      {
        string name = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.NAME;
        string desc = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.DESC;
        Db.Get().CreateFertilityModifier(id, eggTag, name, desc, (Func<string, string>) (descStr => tooltipOverride == null ? string.Format(descStr, (object) ElementLoader.GetElement(element).name) : tooltipOverride), (FertilityModifier.FertilityModFn) ((inst, eggType) =>
        {
          CritterElementMonitor.Instance instance = inst.gameObject.GetSMI<CritterElementMonitor.Instance>();
          if (instance == null)
          {
            instance = new CritterElementMonitor.Instance(inst.master);
            instance.StartSM();
          }
          instance.OnUpdateEggChances += (Action<float>) (dt =>
          {
            int cell = Grid.PosToCell((StateMachine.Instance) inst);
            if (!Grid.IsValidCell(cell))
              return;
            if (Grid.Element[cell].HasTag(element) && (!checkSubstantialLiquid || Grid.IsSubstantialLiquid(cell)))
            {
              inst.AddBreedingChance(eggType, dt * modifierPerSecond);
            }
            else
            {
              if (!alsoInvert)
                return;
              inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
            }
          });
        }));
      });
    }

    private static System.Action CreateCropTendedModifier(
      string id,
      Tag eggTag,
      HashSet<Tag> cropTags,
      float modifierPerEvent)
    {
      return (System.Action) (() =>
      {
        string name = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.CROPTENDING.NAME;
        string desc = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.CROPTENDING.DESC;
        Db.Get().CreateFertilityModifier(id, eggTag, name, desc, (Func<string, string>) (descStr =>
        {
          string str = string.Join(", ", cropTags.Select<Tag, string>((Func<Tag, string>) (t => t.ProperName())).ToArray<string>());
          descStr = string.Format(descStr, (object) str);
          return descStr;
        }), (FertilityModifier.FertilityModFn) ((inst, eggType) => inst.gameObject.Subscribe(90606262, (Action<object>) (data =>
        {
          if (!cropTags.Contains(((CropTendingStates.CropTendingEventData) data).cropId))
            return;
          inst.AddBreedingChance(eggType, modifierPerEvent);
        }))));
      });
    }

    private static System.Action CreateTemperatureModifier(
      string id,
      Tag eggTag,
      float minTemp,
      float maxTemp,
      float modifierPerSecond,
      bool alsoInvert)
    {
      return (System.Action) (() =>
      {
        string name = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.NAME;
        Db.Get().CreateFertilityModifier(id, eggTag, name, (string) null, (Func<string, string>) (src => string.Format((string) STRINGS.CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.DESC, (object) GameUtil.GetFormattedTemperature(minTemp), (object) GameUtil.GetFormattedTemperature(maxTemp))), (FertilityModifier.FertilityModFn) ((inst, eggType) =>
        {
          CritterTemperatureMonitor.Instance smi = inst.gameObject.GetSMI<CritterTemperatureMonitor.Instance>();
          if (smi != null)
            smi.OnUpdate_GetTemperatureInternal += (Action<float, float>) ((dt, newTemp) =>
            {
              if ((double) newTemp > (double) minTemp && (double) newTemp < (double) maxTemp)
              {
                inst.AddBreedingChance(eggType, dt * modifierPerSecond);
              }
              else
              {
                if (!alsoInvert)
                  return;
                inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
              }
            });
          else
            DebugUtil.LogErrorArgs((object) "Ack! Trying to add temperature modifier", (object) id, (object) "to", (object) inst.master.name, (object) "but it doesn't have a CritterTemperatureMonitor.Instance");
        }));
      });
    }

    private static System.Action CreateDecorModifier(
      string id,
      Tag eggTag,
      float minDecor,
      float modifierPerSecond,
      bool alsoInvert)
    {
      return (System.Action) (() =>
      {
        string name = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.DECOR.NAME;
        Db.Get().CreateFertilityModifier(id, eggTag, name, (string) null, (Func<string, string>) (src => string.Format((string) STRINGS.CREATURES.FERTILITY_MODIFIERS.DECOR.DESC, (object) GameUtil.GetFormattedDecor(minDecor))), (FertilityModifier.FertilityModFn) ((inst, eggType) =>
        {
          CreatureDecorMonitor.Instance smi = inst.gameObject.GetSMI<CreatureDecorMonitor.Instance>();
          if (smi != null)
          {
            smi.OnHighDecorUpdate += (Action<float>) (dt => inst.AddBreedingChance(eggType, dt * modifierPerSecond));
            if (!alsoInvert)
              return;
            smi.OnLowDecorUpdate += (Action<float>) (dt => inst.AddBreedingChance(eggType, dt * -modifierPerSecond));
          }
          else
            DebugUtil.LogErrorArgs((object) "Ack! Trying to add decor modifier", (object) id, (object) "to", (object) inst.master.name, (object) "but it doesn't have a CreatureDecorMonitor.Instance");
        }));
      });
    }

    static EGG_CHANCE_MODIFIERS()
    {
      List<System.Action> actionList = new List<System.Action>();
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchHard", "HatchHardEgg".ToTag(), SimHashes.SedimentaryRock.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchVeggie", "HatchVeggieEgg".ToTag(), SimHashes.Dirt.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchMetal", "HatchMetalEgg".ToTag(), HatchMetalConfig.METAL_ORE_TAGS, 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaBalance", "PuftAlphaEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), -0.00025f, true));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyOxylite", "PuftOxyliteEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyBleachstone", "PuftBleachstoneEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterHighTemp", "OilfloaterHighTempEgg".ToTag(), 373.15f, 523.15f, 8.333333E-05f, false));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterDecor", "OilfloaterDecorEgg".ToTag(), 293.15f, 333.15f, 8.333333E-05f, false));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugOrange", "LightBugOrangeEgg".ToTag(), "GrilledPrickleFruit".ToTag(), 1f / 800f));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPurple", "LightBugPurpleEgg".ToTag(), "FriedMushroom".ToTag(), 1f / 800f));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPink", "LightBugPinkEgg".ToTag(), "SpiceBread".ToTag(), 1f / 800f));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlue", "LightBugBlueEgg".ToTag(), "Salsa".ToTag(), 1f / 800f));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlack", "LightBugBlackEgg".ToTag(), SimHashes.Phosphorus.CreateTag(), 1f / 800f));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugCrystal", "LightBugCrystalEgg".ToTag(), "CookedMeat".ToTag(), 1f / 800f));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuTropical", "PacuTropicalEgg".ToTag(), 308.15f, 353.15f, 8.333333E-05f, false));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuCleaner", "PacuCleanerEgg".ToTag(), 243.15f, 278.15f, 8.333333E-05f, false));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("DreckoPlastic", "DreckoPlasticEgg".ToTag(), "BasicSingleHarvestPlant".ToTag(), 0.025f / DreckoTuning.STANDARD_CALORIES_PER_CYCLE));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("SquirrelHug", "SquirrelHugEgg".ToTag(), BasicFabricMaterialPlantConfig.ID.ToTag(), 0.025f / SquirrelTuning.STANDARD_CALORIES_PER_CYCLE));
      Tag tag = "DivergentWormEgg".ToTag();
      HashSet<Tag> cropTags = new HashSet<Tag>();
      cropTags.Add("WormPlant".ToTag());
      cropTags.Add("SuperWormPlant".ToTag());
      double modifierPerEvent = 0.05000000074505806 / (double) DivergentTuning.TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION;
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateCropTendedModifier("DivergentWorm", tag, cropTags, (float) modifierPerEvent));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeLumber", "CrabWoodEgg".ToTag(), SimHashes.Ethanol.CreateTag(), 0.00025f, true, true));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeFreshWater", "CrabFreshWaterEgg".ToTag(), SimHashes.Water.CreateTag(), 0.00025f, true, true));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("MoleDelicacy", "MoleDelicacyEgg".ToTag(), MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MIN, MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MAX, 8.333333E-05f, false));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarGas", "StaterpillarGasEgg".ToTag(), GameTags.Unbreathable, 0.00025f, true, false, (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.UNBREATHABLE));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarLiquid", "StaterpillarLiquidEgg".ToTag(), GameTags.Liquid, 0.00025f, true, false, (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.LIQUID));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("BellyGold", "GoldBellyEgg".ToTag(), "FriesCarrot".ToTag(), 0.05f / BellyTuning.STANDARD_CALORIES_PER_CYCLE));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDecorModifier("GlassDeerDecor", "GlassDeerEgg".ToTag(), 100f, 8.333333E-05f, true));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("AlgaeStego", "AlgaeStegoEgg".ToTag(), SimHashes.CarbonDioxide.CreateTag(), 0.00025f, true, false));
      actionList.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("SnailHighTemp", "SnailIronEgg".ToTag(), 333.15f, 413.15f, 0.00025f, false));
      CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS = actionList;
    }
  }

  public class MOO_SONG_MODIFIERS
  {
    public static List<System.Action> MODIFIER_CREATORS = new List<System.Action>()
    {
      CREATURES.MOO_SONG_MODIFIERS.CreateDietaryModifier("GassyMoo", (Tag) GassyMooCometConfig.ID, (Tag) "GasGrass", 0.05f / MooTuning.STANDARD_CALORIES_PER_CYCLE),
      CREATURES.MOO_SONG_MODIFIERS.CreateDietaryModifier("DieselMoo", (Tag) DieselMooCometConfig.ID, (Tag) "PlantFiber", 0.05f / MooTuning.STANDARD_CALORIES_PER_CYCLE)
    };

    private static System.Action CreateDietaryModifier(
      string id,
      Tag meteorTag,
      HashSet<Tag> foodTags,
      float modifierPerCal)
    {
      return (System.Action) (() =>
      {
        string name = (string) STRINGS.CREATURES.MOO_SONG_MODIFIERS.DIET.NAME;
        string desc = (string) STRINGS.CREATURES.MOO_SONG_MODIFIERS.DIET.DESC;
        Db.Get().CreateMooSongModifier(id, meteorTag, name, desc, (Func<string, string>) (descStr =>
        {
          string str = string.Join(", ", foodTags.Select<Tag, string>((Func<Tag, string>) (t => t.ProperName())).ToArray<string>());
          descStr = string.Format(descStr, (object) str);
          return descStr;
        }), (MooSongModifier.MooSongModFn) ((inst, meteorID) => inst.gameObject.Subscribe(-2038961714, (Action<object>) (data =>
        {
          CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>) data).value;
          if (!foodTags.Contains(caloriesConsumedEvent.tag))
            return;
          inst.AddSongChance(meteorID, caloriesConsumedEvent.calories * modifierPerCal);
        }))));
      });
    }

    private static System.Action CreateDietaryModifier(
      string id,
      Tag meteorTag,
      Tag foodTag,
      float modifierPerCal)
    {
      string id1 = id;
      Tag meteorTag1 = meteorTag;
      HashSet<Tag> foodTags = new HashSet<Tag>();
      foodTags.Add(foodTag);
      double modifierPerCal1 = (double) modifierPerCal;
      return CREATURES.MOO_SONG_MODIFIERS.CreateDietaryModifier(id1, meteorTag1, foodTags, (float) modifierPerCal1);
    }
  }

  public class SORTING
  {
    public static Dictionary<string, int> CRITTER_ORDER = new Dictionary<string, int>()
    {
      {
        "Hatch",
        10
      },
      {
        "Puft",
        20
      },
      {
        "Drecko",
        30
      },
      {
        "Squirrel",
        40
      },
      {
        "Pacu",
        50
      },
      {
        "Oilfloater",
        60
      },
      {
        "LightBug",
        70
      },
      {
        "Crab",
        80 /*0x50*/
      },
      {
        "DivergentBeetle",
        90
      },
      {
        "Staterpillar",
        100
      },
      {
        "Mole",
        110
      },
      {
        "Bee",
        120
      },
      {
        "Moo",
        130
      },
      {
        "Glom",
        140
      },
      {
        "WoodDeer",
        150
      },
      {
        "Seal",
        160 /*0xA0*/
      },
      {
        "IceBelly",
        170
      },
      {
        "Stego",
        180
      },
      {
        "Butterfly",
        190
      },
      {
        "Mosquito",
        200
      },
      {
        "Chameleon",
        210
      },
      {
        "PrehistoricPacu",
        220
      },
      {
        "ParrotFish",
        230
      },
      {
        "Squid",
        240 /*0xF0*/
      },
      {
        "PufferFish",
        250
      },
      {
        "SeaFairy",
        260
      },
      {
        "SeaTurtle",
        270
      },
      {
        "SeaHorse",
        280
      },
      {
        "Snail",
        290
      }
    };
  }
}
