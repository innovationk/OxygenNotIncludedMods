// Decompiled with JetBrains decompiler
// Type: TUNING.FOOD
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace TUNING;

public class FOOD
{
  public const float EATING_SECONDS_PER_CALORIE = 2E-05f;
  public static float FOOD_CALORIES_PER_CYCLE = -DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE;
  public const int FOOD_AMOUNT_INGREDIENT_ONLY = 0;
  public const float KCAL_SMALL_PORTION = 600000f;
  public const float KCAL_BONUS_COOKING_LOW = 250000f;
  public const float KCAL_BASIC_PORTION = 800000f;
  public const float KCAL_PREPARED_FOOD = 4000000f;
  public const float KCAL_BONUS_COOKING_BASIC = 400000f;
  public const float KCAL_BONUS_COOKING_DEEPFRIED = 1200000f;
  public const float DEFAULT_PRESERVE_TEMPERATURE = 255.15f;
  public const float DEFAULT_ROT_TEMPERATURE = 277.15f;
  public const float HIGH_PRESERVE_TEMPERATURE = 283.15f;
  public const float HIGH_ROT_TEMPERATURE = 308.15f;
  public const float EGG_COOK_TEMPERATURE = 344.15f;
  public const float DEFAULT_MASS = 1f;
  public const float DEFAULT_SPICE_MASS = 1f;
  public const float ROT_TO_ELEMENT_TIME = 600f;
  public const int MUSH_BAR_SPAWN_GERMS = 1000;
  public const float IDEAL_TEMPERATURE_TOLERANCE = 10f;
  public const int FOOD_QUALITY_AWFUL = -1;
  public const int FOOD_QUALITY_TERRIBLE = 0;
  public const int FOOD_QUALITY_MEDIOCRE = 1;
  public const int FOOD_QUALITY_GOOD = 2;
  public const int FOOD_QUALITY_GREAT = 3;
  public const int FOOD_QUALITY_AMAZING = 4;
  public const int FOOD_QUALITY_WONDERFUL = 5;
  public const int FOOD_QUALITY_MORE_WONDERFUL = 6;

  public class SPOIL_TIME
  {
    public const float DEFAULT = 4800f;
    public const float QUICK = 2400f;
    public const float SLOW = 9600f;
    public const float VERYSLOW = 19200f;
  }

  public class FOOD_TYPES
  {
    public static readonly EdiblesManager.FoodInfo FIELDRATION = new EdiblesManager.FoodInfo("FieldRation", 800000f, -1, 255.15f, 277.15f, 19200f, false);
    public static readonly EdiblesManager.FoodInfo MUSHBAR = new EdiblesManager.FoodInfo("MushBar", 800000f, -1, 255.15f, 277.15f, 4800f, true);
    public static readonly EdiblesManager.FoodInfo BASICPLANTFOOD = new EdiblesManager.FoodInfo("BasicPlantFood", 600000f, -1, 255.15f, 277.15f, 4800f, true);
    public static readonly EdiblesManager.FoodInfo VINEFRUIT = new EdiblesManager.FoodInfo(VineFruitConfig.ID, 325000f, 0, 255.15f, 277.15f, 4800f, true, DlcManager.DLC4);
    public static readonly EdiblesManager.FoodInfo BASICFORAGEPLANT = new EdiblesManager.FoodInfo("BasicForagePlant", 800000f, -1, 255.15f, 277.15f, 4800f, false);
    public static readonly EdiblesManager.FoodInfo FORESTFORAGEPLANT = new EdiblesManager.FoodInfo("ForestForagePlant", 6400000f, -1, 255.15f, 277.15f, 4800f, false);
    public static readonly EdiblesManager.FoodInfo SWAMPFORAGEPLANT = new EdiblesManager.FoodInfo("SwampForagePlant", 2400000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.EXPANSION1);
    public static readonly EdiblesManager.FoodInfo ICECAVESFORAGEPLANT = new EdiblesManager.FoodInfo("IceCavesForagePlant", 800000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.DLC2);
    public static readonly EdiblesManager.FoodInfo MUSHROOM = new EdiblesManager.FoodInfo(MushroomConfig.ID, 2400000f, 0, 255.15f, 277.15f, 4800f, true);
    public static readonly EdiblesManager.FoodInfo LETTUCE;
    public static readonly EdiblesManager.FoodInfo RAWEGG;
    public static readonly EdiblesManager.FoodInfo MEAT;
    public static readonly EdiblesManager.FoodInfo PLANTMEAT;
    public static readonly EdiblesManager.FoodInfo PRICKLEFRUIT;
    public static readonly EdiblesManager.FoodInfo SWAMPFRUIT;
    public static readonly EdiblesManager.FoodInfo FISH_MEAT;
    public static readonly EdiblesManager.FoodInfo SHELLFISH_MEAT;
    public static readonly EdiblesManager.FoodInfo JAWBOFILLET;
    public static readonly EdiblesManager.FoodInfo WORMBASICFRUIT;
    public static readonly EdiblesManager.FoodInfo WORMSUPERFRUIT;
    public static readonly EdiblesManager.FoodInfo HARDSKINBERRY;
    public static readonly EdiblesManager.FoodInfo CARROT;
    public static readonly EdiblesManager.FoodInfo PEMMICAN;
    public static readonly EdiblesManager.FoodInfo FRIES_CARROT;
    public static readonly EdiblesManager.FoodInfo BUTTERFLYFOOD;
    public static readonly EdiblesManager.FoodInfo DEEP_FRIED_MEAT;
    public static readonly EdiblesManager.FoodInfo DEEP_FRIED_NOSH;
    public static readonly EdiblesManager.FoodInfo DEEP_FRIED_FISH;
    public static readonly EdiblesManager.FoodInfo DEEP_FRIED_SHELLFISH;
    public static readonly EdiblesManager.FoodInfo GARDENFOODPLANT;
    public static readonly EdiblesManager.FoodInfo GARDENFORAGEPLANT;
    public static readonly EdiblesManager.FoodInfo PICKLEDMEAL;
    public static readonly EdiblesManager.FoodInfo BASICPLANTBAR;
    public static readonly EdiblesManager.FoodInfo FRIEDMUSHBAR;
    public static readonly EdiblesManager.FoodInfo GAMMAMUSH;
    public static readonly EdiblesManager.FoodInfo GRILLED_PRICKLEFRUIT;
    public static readonly EdiblesManager.FoodInfo SWAMP_DELIGHTS;
    public static readonly EdiblesManager.FoodInfo FRIED_MUSHROOM;
    public static readonly EdiblesManager.FoodInfo COOKED_PIKEAPPLE;
    public static readonly EdiblesManager.FoodInfo COLD_WHEAT_BREAD;
    public static readonly EdiblesManager.FoodInfo COOKED_EGG;
    public static readonly EdiblesManager.FoodInfo COOKED_FISH;
    public static readonly EdiblesManager.FoodInfo SMOKED_VEGETABLES;
    public static readonly EdiblesManager.FoodInfo SALTYSTICKSFOOD;
    public static readonly EdiblesManager.FoodInfo EDAMAME;
    public static readonly EdiblesManager.FoodInfo MAKI;
    public static readonly EdiblesManager.FoodInfo NIGIRI;
    public static readonly EdiblesManager.FoodInfo SQUID_MEAT;
    public static readonly EdiblesManager.FoodInfo URCHINMEAT;
    public static readonly EdiblesManager.FoodInfo MUSSELTONGUE;
    public static readonly EdiblesManager.FoodInfo PANCAKES;
    public static readonly EdiblesManager.FoodInfo SMOKED_FISH;
    public static readonly EdiblesManager.FoodInfo COOKED_MEAT;
    public static readonly EdiblesManager.FoodInfo SMOKED_DINOSAURMEAT;
    public static readonly EdiblesManager.FoodInfo WORMBASICFOOD;
    public static readonly EdiblesManager.FoodInfo WORMSUPERFOOD;
    public static readonly EdiblesManager.FoodInfo FRUITCAKE;
    public static readonly EdiblesManager.FoodInfo SALSA;
    public static readonly EdiblesManager.FoodInfo SURF_AND_TURF;
    public static readonly EdiblesManager.FoodInfo MUSHROOM_WRAP;
    public static readonly EdiblesManager.FoodInfo TOFU;
    public static readonly EdiblesManager.FoodInfo CURRY;
    public static readonly EdiblesManager.FoodInfo SPICEBREAD;
    public static readonly EdiblesManager.FoodInfo SPICY_TOFU;
    public static readonly EdiblesManager.FoodInfo QUICHE;
    public static readonly EdiblesManager.FoodInfo BERRY_PIE;
    public static readonly EdiblesManager.FoodInfo BURGER;
    public static readonly EdiblesManager.FoodInfo BEAN;
    public static readonly EdiblesManager.FoodInfo SPICENUT;
    public static readonly EdiblesManager.FoodInfo COLD_WHEAT_SEED;
    public static readonly EdiblesManager.FoodInfo FERNFOOD;
    public static readonly EdiblesManager.FoodInfo BUTTERFLY_SEED;
    public static readonly EdiblesManager.FoodInfo DINOSAURMEAT;
    public static readonly EdiblesManager.FoodInfo NORI;
    public static readonly EdiblesManager.FoodInfo CAVIAR;

    static FOOD_TYPES()
    {
      EdiblesManager.FoodInfo foodInfo1 = new EdiblesManager.FoodInfo("Lettuce", 400000f, 0, 255.15f, 277.15f, 2400f, true);
      List<string> effects1 = new List<string>();
      effects1.Add("SeafoodRadiationResistance");
      string[] expansioN1_1 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.LETTUCE = foodInfo1.AddEffects(effects1, expansioN1_1);
      FOOD.FOOD_TYPES.RAWEGG = new EdiblesManager.FoodInfo("RawEgg", 1600000f, -1, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.MEAT = new EdiblesManager.FoodInfo("Meat", 1600000f, -1, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.PLANTMEAT = new EdiblesManager.FoodInfo("PlantMeat", 1200000f, 1, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1);
      FOOD.FOOD_TYPES.PRICKLEFRUIT = new EdiblesManager.FoodInfo(PrickleFruitConfig.ID, 1600000f, 0, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.SWAMPFRUIT = new EdiblesManager.FoodInfo(SwampFruitConfig.ID, 1840000f, 0, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1);
      EdiblesManager.FoodInfo foodInfo2 = new EdiblesManager.FoodInfo("FishMeat", 1000000f, 2, 255.15f, 277.15f, 2400f, true);
      List<string> effects2 = new List<string>();
      effects2.Add("SeafoodRadiationResistance");
      string[] expansioN1_2 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.FISH_MEAT = foodInfo2.AddEffects(effects2, expansioN1_2);
      EdiblesManager.FoodInfo foodInfo3 = new EdiblesManager.FoodInfo("ShellfishMeat", 1000000f, 2, 255.15f, 277.15f, 2400f, true);
      List<string> effects3 = new List<string>();
      effects3.Add("SeafoodRadiationResistance");
      string[] expansioN1_3 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.SHELLFISH_MEAT = foodInfo3.AddEffects(effects3, expansioN1_3);
      EdiblesManager.FoodInfo foodInfo4 = new EdiblesManager.FoodInfo("PrehistoricPacuFillet", 1000000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC4);
      List<string> effects4 = new List<string>();
      effects4.Add("SeafoodRadiationResistance");
      string[] expansioN1_4 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.JAWBOFILLET = foodInfo4.AddEffects(effects4, expansioN1_4);
      FOOD.FOOD_TYPES.WORMBASICFRUIT = new EdiblesManager.FoodInfo("WormBasicFruit", 800000f, 0, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1);
      FOOD.FOOD_TYPES.WORMSUPERFRUIT = new EdiblesManager.FoodInfo("WormSuperFruit", 250000f, 1, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1);
      FOOD.FOOD_TYPES.HARDSKINBERRY = new EdiblesManager.FoodInfo("HardSkinBerry", 800000f, -1, 255.15f, 277.15f, 9600f, true, DlcManager.DLC2);
      FOOD.FOOD_TYPES.CARROT = new EdiblesManager.FoodInfo(CarrotConfig.ID, 4000000f, 0, 255.15f, 277.15f, 9600f, true, DlcManager.DLC2);
      FOOD.FOOD_TYPES.PEMMICAN = new EdiblesManager.FoodInfo("Pemmican", (float) ((double) FOOD.FOOD_TYPES.HARDSKINBERRY.CaloriesPerUnit * 2.0 + 1000000.0), 2, 255.15f, 277.15f, 19200f, false, DlcManager.DLC2);
      FOOD.FOOD_TYPES.FRIES_CARROT = new EdiblesManager.FoodInfo("FriesCarrot", 5400000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2);
      FOOD.FOOD_TYPES.BUTTERFLYFOOD = new EdiblesManager.FoodInfo("ButterflyFood", 1500000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.DLC4);
      FOOD.FOOD_TYPES.DEEP_FRIED_MEAT = new EdiblesManager.FoodInfo("DeepFriedMeat", 4000000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2);
      FOOD.FOOD_TYPES.DEEP_FRIED_NOSH = new EdiblesManager.FoodInfo("DeepFriedNosh", 5000000f, 3, 255.15f, 277.15f, 4800f, true, DlcManager.DLC2);
      EdiblesManager.FoodInfo foodInfo5 = new EdiblesManager.FoodInfo("DeepFriedFish", 4200000f, 4, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2);
      List<string> effects5 = new List<string>();
      effects5.Add("SeafoodRadiationResistance");
      string[] expansioN1_5 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.DEEP_FRIED_FISH = foodInfo5.AddEffects(effects5, expansioN1_5);
      EdiblesManager.FoodInfo foodInfo6 = new EdiblesManager.FoodInfo("DeepFriedShellfish", 4200000f, 4, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2);
      List<string> effects6 = new List<string>();
      effects6.Add("SeafoodRadiationResistance");
      string[] expansioN1_6 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.DEEP_FRIED_SHELLFISH = foodInfo6.AddEffects(effects6, expansioN1_6);
      FOOD.FOOD_TYPES.GARDENFOODPLANT = new EdiblesManager.FoodInfo("GardenFoodPlantFood", 800000f, -1, 255.15f, 277.15f, 9600f, true, DlcManager.DLC4);
      FOOD.FOOD_TYPES.GARDENFORAGEPLANT = new EdiblesManager.FoodInfo("GardenForagePlant", 800000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.DLC4);
      FOOD.FOOD_TYPES.PICKLEDMEAL = new EdiblesManager.FoodInfo("PickledMeal", 1800000f, -1, 255.15f, 277.15f, 19200f, true);
      FOOD.FOOD_TYPES.BASICPLANTBAR = new EdiblesManager.FoodInfo("BasicPlantBar", 1700000f, 0, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.FRIEDMUSHBAR = new EdiblesManager.FoodInfo("FriedMushBar", 1050000f, 0, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.GAMMAMUSH = new EdiblesManager.FoodInfo("GammaMush", 1050000f, 1, 255.15f, 277.15f, 2400f, true);
      FOOD.FOOD_TYPES.GRILLED_PRICKLEFRUIT = new EdiblesManager.FoodInfo("GrilledPrickleFruit", 2000000f, 1, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.SWAMP_DELIGHTS = new EdiblesManager.FoodInfo("SwampDelights", 2240000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1);
      FOOD.FOOD_TYPES.FRIED_MUSHROOM = new EdiblesManager.FoodInfo("FriedMushroom", 2800000f, 1, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.COOKED_PIKEAPPLE = new EdiblesManager.FoodInfo("CookedPikeapple", 1200000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.DLC2);
      FOOD.FOOD_TYPES.COLD_WHEAT_BREAD = new EdiblesManager.FoodInfo("ColdWheatBread", 1200000f, 2, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.COOKED_EGG = new EdiblesManager.FoodInfo("CookedEgg", 2800000f, 2, 255.15f, 277.15f, 2400f, true);
      EdiblesManager.FoodInfo foodInfo7 = new EdiblesManager.FoodInfo("CookedFish", 1600000f, 3, 255.15f, 277.15f, 2400f, true);
      List<string> effects7 = new List<string>();
      effects7.Add("SeafoodRadiationResistance");
      string[] expansioN1_7 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.COOKED_FISH = foodInfo7.AddEffects(effects7, expansioN1_7);
      FOOD.FOOD_TYPES.SMOKED_VEGETABLES = new EdiblesManager.FoodInfo("SmokedVegetables", 2862500f, 2, 255.15f, 277.15f, 9600f, true, DlcManager.DLC4);
      FOOD.FOOD_TYPES.SALTYSTICKSFOOD = new EdiblesManager.FoodInfo("SaltySticksFood", 600000f, -1, 255.15f, 277.15f, 9600f, true, DlcManager.DLC5).AddEffects(new List<string>()
      {
        "Thirsty"
      });
      FOOD.FOOD_TYPES.EDAMAME = new EdiblesManager.FoodInfo("Edamame", 1450000f, 3, 255.15f, 277.15f, 4800f, true, DlcManager.DLC5);
      EdiblesManager.FoodInfo foodInfo8 = new EdiblesManager.FoodInfo("Maki", 3600000f, 4, 255.15f, 277.15f, 2400f, true, DlcManager.DLC5);
      List<string> effects8 = new List<string>();
      effects8.Add("SeafoodRadiationResistance");
      string[] expansioN1_8 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.MAKI = foodInfo8.AddEffects(effects8, expansioN1_8);
      EdiblesManager.FoodInfo foodInfo9 = new EdiblesManager.FoodInfo("Nigiri", 3600000f, 5, 255.15f, 277.15f, 2400f, true, DlcManager.DLC5);
      List<string> effects9 = new List<string>();
      effects9.Add("SeafoodRadiationResistance");
      string[] expansioN1_9 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.NIGIRI = foodInfo9.AddEffects(effects9, expansioN1_9);
      EdiblesManager.FoodInfo foodInfo10 = new EdiblesManager.FoodInfo("SquidMeat", 800000f, 2, 255.15f, 277.15f, 2400f, true, DlcManager.DLC5);
      List<string> effects10 = new List<string>();
      effects10.Add("SeafoodRadiationResistance");
      string[] expansioN1_10 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.SQUID_MEAT = foodInfo10.AddEffects(effects10, expansioN1_10);
      EdiblesManager.FoodInfo foodInfo11 = new EdiblesManager.FoodInfo(UrchinMeatConfig.ID, 7200000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC5);
      List<string> effects11 = new List<string>();
      effects11.Add("SeafoodRadiationResistance");
      string[] expansioN1_11 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.URCHINMEAT = foodInfo11.AddEffects(effects11, expansioN1_11);
      EdiblesManager.FoodInfo foodInfo12 = new EdiblesManager.FoodInfo(MusselTongueConfig.ID, 2800000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.DLC5);
      List<string> effects12 = new List<string>();
      effects12.Add("SeafoodRadiationResistance");
      string[] expansioN1_12 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.MUSSELTONGUE = foodInfo12.AddEffects(effects12, expansioN1_12);
      FOOD.FOOD_TYPES.PANCAKES = new EdiblesManager.FoodInfo("Pancakes", 3600000f, 3, 255.15f, 277.15f, 4800f, true);
      EdiblesManager.FoodInfo foodInfo13 = new EdiblesManager.FoodInfo("SmokedFish", 2800000f, 3, 255.15f, 277.15f, 19200f, true, DlcManager.DLC4);
      List<string> effects13 = new List<string>();
      effects13.Add("SeafoodRadiationResistance");
      string[] expansioN1_13 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.SMOKED_FISH = foodInfo13.AddEffects(effects13, expansioN1_13);
      FOOD.FOOD_TYPES.COOKED_MEAT = new EdiblesManager.FoodInfo("CookedMeat", 4000000f, 3, 255.15f, 277.15f, 2400f, true);
      FOOD.FOOD_TYPES.SMOKED_DINOSAURMEAT = new EdiblesManager.FoodInfo("SmokedDinosaurMeat", 5000000f, 3, 255.15f, 277.15f, 4800f, true, DlcManager.DLC4);
      FOOD.FOOD_TYPES.WORMBASICFOOD = new EdiblesManager.FoodInfo("WormBasicFood", 1200000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1);
      FOOD.FOOD_TYPES.WORMSUPERFOOD = new EdiblesManager.FoodInfo("WormSuperFood", 2400000f, 3, 255.15f, 277.15f, 19200f, true, DlcManager.EXPANSION1);
      FOOD.FOOD_TYPES.FRUITCAKE = new EdiblesManager.FoodInfo("FruitCake", 4000000f, 3, 255.15f, 277.15f, 19200f, false);
      FOOD.FOOD_TYPES.SALSA = new EdiblesManager.FoodInfo("Salsa", 4400000f, 4, 255.15f, 277.15f, 2400f, true);
      EdiblesManager.FoodInfo foodInfo14 = new EdiblesManager.FoodInfo("SurfAndTurf", 6000000f, 4, 255.15f, 277.15f, 2400f, true);
      List<string> effects14 = new List<string>();
      effects14.Add("SeafoodRadiationResistance");
      string[] expansioN1_14 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.SURF_AND_TURF = foodInfo14.AddEffects(effects14, expansioN1_14);
      EdiblesManager.FoodInfo foodInfo15 = new EdiblesManager.FoodInfo("MushroomWrap", 4800000f, 4, 255.15f, 277.15f, 2400f, true);
      List<string> effects15 = new List<string>();
      effects15.Add("SeafoodRadiationResistance");
      string[] expansioN1_15 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.MUSHROOM_WRAP = foodInfo15.AddEffects(effects15, expansioN1_15);
      FOOD.FOOD_TYPES.TOFU = new EdiblesManager.FoodInfo("Tofu", 3600000f, 2, 255.15f, 277.15f, 2400f, true);
      FOOD.FOOD_TYPES.CURRY = new EdiblesManager.FoodInfo("Curry", 5000000f, 4, 255.15f, 277.15f, 9600f, true).AddEffects(new List<string>()
      {
        "HotStuff",
        "WarmTouchFood"
      });
      FOOD.FOOD_TYPES.SPICEBREAD = new EdiblesManager.FoodInfo("SpiceBread", 4000000f, 5, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.SPICY_TOFU = new EdiblesManager.FoodInfo("SpicyTofu", 4000000f, 5, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>()
      {
        "WarmTouchFood"
      });
      EdiblesManager.FoodInfo foodInfo16 = new EdiblesManager.FoodInfo("Quiche", 6400000f, 5, 255.15f, 277.15f, 2400f, true);
      List<string> effects16 = new List<string>();
      effects16.Add("SeafoodRadiationResistance");
      string[] expansioN1_16 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.QUICHE = foodInfo16.AddEffects(effects16, expansioN1_16);
      FOOD.FOOD_TYPES.BERRY_PIE = new EdiblesManager.FoodInfo("BerryPie", 4200000f, 5, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1);
      EdiblesManager.FoodInfo foodInfo17 = new EdiblesManager.FoodInfo("Burger", 6000000f, 6, 255.15f, 277.15f, 2400f, true).AddEffects(new List<string>()
      {
        "GoodEats"
      });
      List<string> effects17 = new List<string>();
      effects17.Add("SeafoodRadiationResistance");
      string[] expansioN1_17 = DlcManager.EXPANSION1;
      FOOD.FOOD_TYPES.BURGER = foodInfo17.AddEffects(effects17, expansioN1_17);
      FOOD.FOOD_TYPES.BEAN = new EdiblesManager.FoodInfo("BeanPlantSeed", 0.0f, 3, 255.15f, 277.15f, 4800f, true);
      FOOD.FOOD_TYPES.SPICENUT = new EdiblesManager.FoodInfo(SpiceNutConfig.ID, 0.0f, 0, 255.15f, 277.15f, 2400f, true);
      FOOD.FOOD_TYPES.COLD_WHEAT_SEED = new EdiblesManager.FoodInfo("ColdWheatSeed", 0.0f, 0, 283.15f, 308.15f, 9600f, true);
      FOOD.FOOD_TYPES.FERNFOOD = new EdiblesManager.FoodInfo(FernFoodConfig.ID, 0.0f, 2, 255.15f, 277.15f, 9600f, true, DlcManager.DLC4);
      FOOD.FOOD_TYPES.BUTTERFLY_SEED = new EdiblesManager.FoodInfo("ButterflyPlantSeed", 0.0f, 2, 255.15f, 277.15f, 4800f, true, DlcManager.DLC4);
      FOOD.FOOD_TYPES.DINOSAURMEAT = new EdiblesManager.FoodInfo("DinosaurMeat", 0.0f, -1, 255.15f, 277.15f, 2400f, true, DlcManager.DLC4);
      FOOD.FOOD_TYPES.NORI = new EdiblesManager.FoodInfo("Nori", 0.0f, 2, 255.15f, 277.15f, 19200f, true, DlcManager.DLC5);
      FOOD.FOOD_TYPES.CAVIAR = new EdiblesManager.FoodInfo("Caviar", 0.0f, 2, 255.15f, 277.15f, 2400f, false, DlcManager.DLC5);
    }
  }

  public class RECIPES
  {
    public static float SMALL_COOK_TIME = 30f;
    public static float STANDARD_COOK_TIME = 50f;
  }
}
