// Decompiled with JetBrains decompiler
// Type: BellyTuning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TUNING;

#nullable disable
public static class BellyTuning
{
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "IceBellyEgg".ToTag(),
      weight = 1f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "GoldBellyEgg".ToTag(),
      weight = 0.0f
    }
  };
  public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_GOLD = new List<FertilityMonitor.BreedingChance>()
  {
    new FertilityMonitor.BreedingChance()
    {
      egg = "IceBellyEgg".ToTag(),
      weight = 0.02f
    },
    new FertilityMonitor.BreedingChance()
    {
      egg = "GoldBellyEgg".ToTag(),
      weight = 0.98f
    }
  };
  public const float KW_GENERATED_TO_WARM_UP = 1.3f;
  public static float STANDARD_CALORIES_PER_CYCLE = (float) (4.0 * (double) FOOD.FOOD_TYPES.CARROT.CaloriesPerUnit / ((double) CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == CarrotConfig.ID)).cropDuration / 600.0));
  public const float STANDARD_STARVE_CYCLES = 10f;
  public static float STANDARD_STOMACH_SIZE = BellyTuning.STANDARD_CALORIES_PER_CYCLE * 10f;
  public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;
  public const float EGG_MASS = 8f;
  public const int GERMS_EMMITED_PER_KG_POOPED = 1000;
  public static string GERM_ID_EMMITED_ON_POOP = "PollenGerms";
  public static float CALORIES_PER_UNIT_EATEN = FOOD.FOOD_TYPES.CARROT.CaloriesPerUnit;
  public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == CarrotConfig.ID)).cropDuration / 600f;
  public const float CONSUMED_MASS_TO_POOP_MASS_MULTIPLIER = 67.474f;
  public const float MIN_POOP_SIZE_IN_KG = 1f;
}
