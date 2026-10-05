// Decompiled with JetBrains decompiler
// Type: STRINGS.ITEMS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace STRINGS;

public class ITEMS
{
  public class PILLS
  {
    public class PLACEBO
    {
      public static LocString NAME = (LocString) "Placebo";
      public static LocString DESC = (LocString) $"A general, all-purpose {UI.FormatAsLink("Medicine", "MEDICINE")}.\n\nThe less one knows about it, the better it works.";
      public static LocString RECIPEDESC = (LocString) $"All-purpose {UI.FormatAsLink("Medicine", "MEDICINE")}.";
    }

    public class BASICBOOSTER
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Vitamin Chews", nameof (BASICBOOSTER));
      public static LocString DESC = (LocString) "Minorly reduces the chance of becoming sick.";
      public static LocString RECIPEDESC = (LocString) $"A supplement that minorly reduces the chance of contracting a {UI.PRE_KEYWORD}Germ{UI.PST_KEYWORD}-based {UI.FormatAsLink("Disease", "DISEASE")}.\n\nMust be taken daily.";
    }

    public class INTERMEDIATEBOOSTER
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Immuno Booster", nameof (INTERMEDIATEBOOSTER));
      public static LocString DESC = (LocString) "Significantly reduces the chance of becoming sick.";
      public static LocString RECIPEDESC = (LocString) $"A supplement that significantly reduces the chance of contracting a {UI.PRE_KEYWORD}Germ{UI.PST_KEYWORD}-based {UI.FormatAsLink("Disease", "DISEASE")}.\n\nMust be taken daily.";
    }

    public class ANTIHISTAMINE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Allergy Medication", nameof (ANTIHISTAMINE));
      public static LocString DESC = (LocString) "Suppresses and prevents allergic reactions.";
      public static LocString RECIPEDESC = (LocString) $"A strong antihistamine Duplicants can take to halt an allergic reaction. {(string) ITEMS.PILLS.ANTIHISTAMINE.NAME} will also prevent further reactions from occurring for a short time after ingestion.";
    }

    public class BASICCURE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Curative Tablet", nameof (BASICCURE));
      public static LocString DESC = (LocString) "A simple, easy-to-take remedy for minor germ-based diseases.";
      public static LocString RECIPEDESC = (LocString) $"Duplicants can take this to cure themselves of minor {UI.PRE_KEYWORD}Germ{UI.PST_KEYWORD}-based {UI.FormatAsLink("Diseases", "DISEASE")}.\n\nCurative Tablets are very effective against {(string) DUPLICANTS.DISEASES.FOODPOISONING.NAME}.";
    }

    public class INTERMEDIATECURE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Medical Pack", nameof (INTERMEDIATECURE));
      public static LocString DESC = (LocString) "A doctor-administered cure for moderate ailments.";
      public static LocString RECIPEDESC = (LocString) $"A doctor-administered cure for moderate {UI.FormatAsLink("Diseases", "DISEASE")}. {(string) ITEMS.PILLS.INTERMEDIATECURE.NAME}s are very effective against {UI.FormatAsLink("Slimelung", "SLIMESICKNESS")}.\n\nMust be administered by a Duplicant with the {(string) DUPLICANTS.ROLES.MEDIC.NAME} Skill.";
    }

    public class ADVANCEDCURE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Serum Vial", nameof (ADVANCEDCURE));
      public static LocString DESC = (LocString) "A doctor-administered cure for severe ailments.";
      public static LocString RECIPEDESC = (LocString) $"An extremely powerful medication created to treat severe {UI.FormatAsLink("Diseases", "DISEASE")}. {(string) ITEMS.PILLS.ADVANCEDCURE.NAME} is very effective against {UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES")}.\n\nMust be administered by a Duplicant with the {(string) DUPLICANTS.ROLES.SENIOR_MEDIC.NAME} Skill.";
    }

    public class BASICRADPILL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Basic Rad Pill", nameof (BASICRADPILL));
      public static LocString DESC = (LocString) "Increases a Duplicant's natural radiation absorption rate.";
      public static LocString RECIPEDESC = (LocString) "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
    }

    public class INTERMEDIATERADPILL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Intermediate Rad Pill", nameof (INTERMEDIATERADPILL));
      public static LocString DESC = (LocString) "Increases a Duplicant's natural radiation absorption rate.";
      public static LocString RECIPEDESC = (LocString) "A supplement that speeds up the rate at which a Duplicant body absorbs radiation, allowing them to manage increased radiation exposure.\n\nMust be taken daily.";
    }
  }

  public class LUBRICATIONSTICK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Gear Balm", nameof (LUBRICATIONSTICK));
    public static LocString SUBHEADER = (LocString) "Mechanical Lubricant";
    public static LocString DESC = (LocString) $"Provides a small amount of lubricating {UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL")}.\n\nCan be produced at the {(string) BUILDINGS.PREFABS.APOTHECARY.NAME}.";
    public static LocString RECIPEDESC = (LocString) "A self-administered mechanical lubricant for Duplicants with bionic parts.";
  }

  public class TALLOWLUBRICATIONSTICK
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Tallow Gear Balm", nameof (TALLOWLUBRICATIONSTICK));
    public static LocString SUBHEADER = (LocString) "Mechanical Lubricant";
    public static LocString DESC = (LocString) $"Provides a small amount of extra-silky lubricating {UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL")}.\n\nCan be produced at the {(string) BUILDINGS.PREFABS.APOTHECARY.NAME}.";
    public static LocString RECIPEDESC = (LocString) "An advanced self-administered mechanical lubricant for Duplicants with bionic parts.";
  }

  public class BIONIC_BOOSTERS
  {
    public static LocString FABRICATION_SOURCE = (LocString) "This booster can be manufactured at the {0}.";

    public class BOOSTER_DIG1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Digging Booster", nameof (BOOSTER_DIG1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant the skill required to dig hard things.";
    }

    public class BOOSTER_DIG2
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Extreme Digging Booster", nameof (BOOSTER_DIG2));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant the digging skill required to get through anything.";
    }

    public class BOOSTER_CONSTRUCT1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Construction Booster", nameof (BOOSTER_CONSTRUCT1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant the ability to build fast, and demolish buildings that others cannot.";
    }

    public class BOOSTER_FARM1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Crop Tending Booster", nameof (BOOSTER_FARM1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant unparalleled farming and botanical analysis skills.";
    }

    public class BOOSTER_RANCH1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Ranching Booster", nameof (BOOSTER_RANCH1));
      public static LocString DESC = (LocString) $"Grants a Bionic Duplicant the skills required to care for {UI.FormatAsLink("Critters", "CREATURES")} in every way.";
    }

    public class BOOSTER_COOK1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Grilling Booster", nameof (BOOSTER_COOK1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant deliciously professional culinary skills.";
    }

    public class BOOSTER_ART1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Masterworks Art Booster", nameof (BOOSTER_ART1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant flawless decorating skills.";
    }

    public class BOOSTER_RESEARCH1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Researching Booster", nameof (BOOSTER_RESEARCH1));
      public static LocString DESC = (LocString) $"Grants a Bionic Duplicant the expertise required to study {UI.FormatAsLink("geysers", "GEYSERS")} and other advanced topics.";
    }

    public class BOOSTER_RESEARCH2
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Astronomy Booster", nameof (BOOSTER_RESEARCH2));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant a keen grasp of science and usage of space-research buildings.";
    }

    public class BOOSTER_RESEARCH3
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Applied Sciences Booster", nameof (BOOSTER_RESEARCH3));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant a deeply pragmatic approach to scientific research.";
    }

    public class BOOSTER_PILOT1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Piloting Booster", nameof (BOOSTER_PILOT1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant the expertise required to explore the skies in person.";
    }

    public class BOOSTER_PILOTVANILLA1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Rocketry Booster", nameof (BOOSTER_PILOTVANILLA1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant the expertise required to command a rocket.";
    }

    public class BOOSTER_SUITS1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Suit Training Booster", nameof (BOOSTER_SUITS1));
      public static LocString DESC = (LocString) $"Enables a Bionic Duplicant to maximize durability of equipped {UI.FormatAsLink("Exosuits", "EQUIPMENT")} and maintain their runspeed.";
    }

    public class BOOSTER_CARRY1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Strength Booster", nameof (BOOSTER_CARRY1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant increased carrying capacity and athletic prowess.";
    }

    public class BOOSTER_OP1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Electrical Engineering Booster", nameof (BOOSTER_OP1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant the skills requried to tinker and solder to their heart's content.";
    }

    public class BOOSTER_OP2
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mechatronics Engineering Booster", nameof (BOOSTER_OP2));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant complete mastery of engineering skills.";
    }

    public class BOOSTER_MEDICINE1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Advanced Medical Booster", nameof (BOOSTER_MEDICINE1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant the ability to perform all doctoring errands.";
    }

    public class BOOSTER_TIDY1
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Tidying Booster", nameof (BOOSTER_TIDY1));
      public static LocString DESC = (LocString) "Grants a Bionic Duplicant the full range of tidying skills, including blasting unwanted meteors out of the sky.";
    }

    public class BOOSTER_MOD_SWIM
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Swim Hack Booster", nameof (BOOSTER_MOD_SWIM));
      public static LocString DESC = (LocString) "Overrides a Bionic Duplicant's safety protocols to enable swimming.\n\n<b>WARNING:</b>\nUsage stresses bionic systems and does not protect from liquid exposure.";
    }
  }

  public class FOOD
  {
    public static LocString COMPOST = (LocString) "Compost";

    public class FOODSPLAT
    {
      public static LocString NAME = (LocString) "Food Splatter";
      public static LocString DESC = (LocString) "Food smeared on the wall from a recent Food Fight";
    }

    public class BURGER
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Frost Burger", nameof (BURGER));
      public static LocString DESC = (LocString) $"{UI.FormatAsLink("Meat", "MEAT")} and {UI.FormatAsLink("Lettuce", "LETTUCE")} on a chilled {UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD")}.\n\nIt's the only burger best served cold.";
      public static LocString RECIPEDESC = (LocString) $"{UI.FormatAsLink("Meat", "MEAT")} and {UI.FormatAsLink("Lettuce", "LETTUCE")} on a chilled {UI.FormatAsLink("Frost Bun", "COLDWHEATBREAD")}.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Frost Burger", nameof (BURGER));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Frost Burger", nameof (BURGER))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class FIELDRATION
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Nutrient Bar", nameof (FIELDRATION));
      public static LocString DESC = (LocString) "A nourishing nutrient paste, sandwiched between thin wafer layers.";
    }

    public class MUSHBAR
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mush Bar", nameof (MUSHBAR));
      public static LocString DESC = (LocString) "An edible, putrefied mudslop.\n\nMush Bars are preferable to starvation, but only just barely.";
      public static LocString RECIPEDESC = (LocString) $"An edible, putrefied mudslop.\n\n{(string) ITEMS.FOOD.MUSHBAR.NAME}s are preferable to starvation, but only just barely.";
    }

    public class MUSHROOMWRAP
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mushroom Wrap", nameof (MUSHROOMWRAP));
      public static LocString DESC = (LocString) $"Flavorful {UI.FormatAsLink("Mushrooms", "MUSHROOM")} wrapped in {UI.FormatAsLink("Lettuce", "LETTUCE")}.\n\nIt has an earthy flavor punctuated by a refreshing crunch.";
      public static LocString RECIPEDESC = (LocString) $"Flavorful {UI.FormatAsLink("Mushrooms", "MUSHROOM")} wrapped in {UI.FormatAsLink("Lettuce", "LETTUCE")}.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Mushroom Wrap", nameof (MUSHROOMWRAP));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Mushroom Wrap", nameof (MUSHROOMWRAP))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class MICROWAVEDLETTUCE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Microwaved Lettuce", nameof (MICROWAVEDLETTUCE));
      public static LocString DESC = (LocString) $"{UI.FormatAsLink("Lettuce", "LETTUCE")} scrumptiously wilted in the {(string) BUILDINGS.PREFABS.GAMMARAYOVEN.NAME}.";
      public static LocString RECIPEDESC = (LocString) $"{UI.FormatAsLink("Lettuce", "LETTUCE")} scrumptiously wilted in the {(string) BUILDINGS.PREFABS.GAMMARAYOVEN.NAME}.";
    }

    public class GAMMAMUSH
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Gamma Mush", nameof (GAMMAMUSH));
      public static LocString DESC = (LocString) "A disturbingly delicious mixture of irradiated dirt and water.";
      public static LocString RECIPEDESC = (LocString) $"{UI.FormatAsLink("Mush Fry", "FRIEDMUSHBAR")} reheated in a {(string) BUILDINGS.PREFABS.GAMMARAYOVEN.NAME}.";
    }

    public class FRUITCAKE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Berry Sludge", nameof (FRUITCAKE));
      public static LocString DESC = (LocString) $"A mashed up {UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT")} sludge with an exceptionally long shelf life.\n\nIts aggressive, overbearing sweetness can leave the tongue feeling temporarily numb.";
      public static LocString RECIPEDESC = (LocString) $"A mashed up {UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT")} sludge with an exceptionally long shelf life.";
    }

    public class POPCORN
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Popcorn", nameof (POPCORN));
      public static LocString DESC = (LocString) $"{UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED")} popped in a {(string) BUILDINGS.PREFABS.GAMMARAYOVEN.NAME}.\n\nCompletely devoid of any fancy flavorings.";
      public static LocString RECIPEDESC = (LocString) $"Gamma-radiated {UI.FormatAsLink("Sleet Wheat Grain", "COLDWHEATSEED")}.";
    }

    public class SUSHI
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Sushi", nameof (SUSHI));
      public static LocString DESC = (LocString) $"Raw {UI.FormatAsLink("Fish Fillet", "FISHMEAT")} wrapped with fresh {UI.FormatAsLink("Lettuce", "LETTUCE")}.\n\nWhile the salt of the lettuce may initially overpower the flavor, a keen palate can discern the subtle sweetness of the fillet beneath.";
      public static LocString RECIPEDESC = (LocString) $"Raw {UI.FormatAsLink("Fish Fillet", "FISHMEAT")} wrapped with fresh {UI.FormatAsLink("Lettuce", "LETTUCE")}.";
    }

    public class HATCHEGG
    {
      public static LocString NAME = CREATURES.SPECIES.HATCH.EGG_NAME;
      public static LocString DESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Hatch", "HATCH")}.\n\nIf incubated, it will hatch into a {UI.FormatAsLink("Hatchling", "HATCH")}.";
      public static LocString RECIPEDESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Hatch", "HATCH")}.";
    }

    public class DRECKOEGG
    {
      public static LocString NAME = CREATURES.SPECIES.DRECKO.EGG_NAME;
      public static LocString DESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Drecko", "DRECKO")}.\n\nIf incubated, it will hatch into a new {UI.FormatAsLink("Drecklet", "DRECKO")}.";
      public static LocString RECIPEDESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Drecko", "DRECKO")}.";
    }

    public class LIGHTBUGEGG
    {
      public static LocString NAME = CREATURES.SPECIES.LIGHTBUG.EGG_NAME;
      public static LocString DESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Shine Bug", "LIGHTBUG")}.\n\nIf incubated, it will hatch into a {UI.FormatAsLink("Shine Nymph", "LIGHTBUG")}.";
      public static LocString RECIPEDESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Shine Bug", "LIGHTBUG")}.";
    }

    public class LETTUCE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Lettuce", nameof (LETTUCE));
      public static LocString DESC = (LocString) $"Crunchy, slightly salty leaves from a {UI.FormatAsLink("Waterweed", "SEALETTUCE")} plant.";
      public static LocString RECIPEDESC = (LocString) $"Edible roughage from a {UI.FormatAsLink("Waterweed", "SEALETTUCE")}.";
    }

    public class PASTA
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pasta", nameof (PASTA));
      public static LocString DESC = (LocString) "pasta made from egg and wheat";
      public static LocString RECIPEDESC = (LocString) "pasta made from egg and wheat";
    }

    public class PANCAKES
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Soufflé Pancakes", nameof (PANCAKES));
      public static LocString DESC = (LocString) $"Sweet discs made from {UI.FormatAsLink("Raw Egg", "RAWEGG")} and {(string) ITEMS.FOOD.COLDWHEATSEED.NAME}.\n\nThey're so thick!";
      public static LocString RECIPEDESC = (LocString) $"Sweet discs made from {UI.FormatAsLink("Raw Egg", "RAWEGG")} and {(string) ITEMS.FOOD.COLDWHEATSEED.NAME}.";
    }

    public class OILFLOATEREGG
    {
      public static LocString NAME = CREATURES.SPECIES.OILFLOATER.EGG_NAME;
      public static LocString DESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Slickster", "OILFLOATER")}.\n\nIf incubated, it will hatch into a {UI.FormatAsLink("Slickster Larva", "OILFLOATER")}.";
      public static LocString RECIPEDESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Slickster", "OILFLOATER")}.";
    }

    public class PUFTEGG
    {
      public static LocString NAME = CREATURES.SPECIES.PUFT.EGG_NAME;
      public static LocString DESC = (LocString) $"An egg laid by a {UI.FormatAsLink("Puft", "PUFT")}.\n\nIf incubated, it will hatch into a {UI.FormatAsLink("Puftlet", "PUFT")}.";
      public static LocString RECIPEDESC = (LocString) $"An egg laid by a {(string) CREATURES.SPECIES.PUFT.NAME}.";
    }

    public class PREHISTORICPACUFILLET
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Jawbo Fillet", nameof (PREHISTORICPACUFILLET));
      public static LocString DESC = (LocString) $"An uncooked fillet from a very dead {(string) CREATURES.SPECIES.PREHISTORICPACU.NAME}. It has a silky texture.";
    }

    public class FISHMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Fish Fillet", nameof (FISHMEAT));
      public static LocString DESC = (LocString) "An uncooked fillet from a very dead aquatic critter. Yum!";
    }

    public class FISHFOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pacu Treat", "PACUTREAT");
      public static LocString DESC = (LocString) $"A gourmet loaf of ground seeds and {UI.FormatAsLink("Water", "WATER")}, formulated to delight {UI.FormatAsLink("Pacus", "PACUSPECIES")}.";
      public static LocString RECIPEDESC = (LocString) $"A gourmet loaf of ground seeds, specially formulated for {UI.FormatAsLink("Pacus", "PACUSPECIES")}.";
    }

    public class MEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Meat", nameof (MEAT));
      public static LocString DESC = (LocString) "Uncooked meat from a very dead critter. Yum!";
    }

    public class DINOSAURMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Tough Meat", nameof (DINOSAURMEAT));
      public static LocString DESC = (LocString) $"Uncooked meat from a very dead critter.\n\nIt's inedible until cooked in the {(string) BUILDINGS.PREFABS.SMOKER.NAME}.";
    }

    public class SMOKEDDINOSAURMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Tender Brisket", nameof (SMOKEDDINOSAURMEAT));
      public static LocString DESC = (LocString) "A cooked stack of tough meat that's been marinated and slow-smoked to tender perfection.";
      public static LocString RECIPEDESC = (LocString) "A stack of tender, slow-smoked meat.";
    }

    public class SMOKEDFISH
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Smoked Fish", nameof (SMOKEDFISH));
      public static LocString DESC = (LocString) "A buttery smoked fish fillet.\n\nIt flakes nicely when pulled apart with a fork.";
      public static LocString RECIPEDESC = (LocString) "A buttery smoked fish fillet.";
    }

    public class SMOKEDVEGETABLES
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Veggie Poppers", nameof (SMOKEDVEGETABLES));
      public static LocString DESC = (LocString) "Crisp vegetables stuffed with herbs and smoked for hours.";
      public static LocString RECIPEDESC = (LocString) "Crisp vegetables stuffed with herbs.";
    }

    public class PLANTMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Plant Meat", nameof (PLANTMEAT));
      public static LocString DESC = (LocString) "Planty plant meat from a plant. How nice!";
    }

    public class SHELLFISHMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Raw Shellfish", nameof (SHELLFISHMEAT));
      public static LocString DESC = (LocString) $"An uncooked chunk of very dead {(string) CREATURES.SPECIES.CRAB.VARIANT_FRESH_WATER.NAME}. Yum!";
    }

    public class MUSHROOM
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mushroom", nameof (MUSHROOM));
      public static LocString DESC = (LocString) "An edible, flavorless fungus that grew in the dark.";
    }

    public class COOKEDFISH
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Cooked Seafood", nameof (COOKEDFISH));
      public static LocString DESC = (LocString) "A cooked piece of freshly caught aquatic critter.\n\nUnsurprisingly, it tastes a bit fishy.";
      public static LocString RECIPEDESC = (LocString) "A cooked piece of freshly caught aquatic critter.";
    }

    public class COOKEDMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Barbeque", nameof (COOKEDMEAT));
      public static LocString DESC = (LocString) "The cooked meat of a defeated critter.\n\nIt has a delightful smoky aftertaste.";
      public static LocString RECIPEDESC = (LocString) "The cooked meat of a defeated critter.";
    }

    public class FRIESCARROT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Squash Fries", nameof (FRIESCARROT));
      public static LocString DESC = (LocString) "Irresistibly crunchy.\n\nBest eaten hot.";
      public static LocString RECIPEDESC = (LocString) $"Crunchy sticks of {UI.FormatAsLink("Plume Squash", "CARROT")} deep-fried in {UI.FormatAsLink("Tallow", "TALLOW")}.";
    }

    public class DEEPFRIEDFISH
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Fish Taco", nameof (DEEPFRIEDFISH));
      public static LocString DESC = (LocString) "Deep-fried fish cradled in a crunchy fin.";
      public static LocString RECIPEDESC = (LocString) $"{UI.FormatAsLink("Fish Fillet", "FISHMEAT")} lightly battered and deep-fried in {UI.FormatAsLink("Tallow", "TALLOW")}.";
    }

    public class DEEPFRIEDSHELLFISH
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Shellfish Tempura", nameof (DEEPFRIEDSHELLFISH));
      public static LocString DESC = (LocString) "A crispy deep-fried critter claw.";
      public static LocString RECIPEDESC = (LocString) $"A tender chunk of battered {UI.FormatAsLink("Raw Shellfish", "SHELLFISHMEAT")} deep-fried in {UI.FormatAsLink("Tallow", "TALLOW")}.";
    }

    public class DEEPFRIEDMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Deep Fried Steak", nameof (DEEPFRIEDMEAT));
      public static LocString DESC = (LocString) "A juicy slab of meat with a crunchy deep-fried upper layer.";
      public static LocString RECIPEDESC = (LocString) $"A juicy slab of {UI.FormatAsLink("Raw Meat", "MEAT")} deep-fried in {UI.FormatAsLink("Tallow", "TALLOW")}.";
    }

    public class DEEPFRIEDNOSH
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Nosh Noms", nameof (DEEPFRIEDNOSH));
      public static LocString DESC = (LocString) "A snackable handful of crunchy beans.";
      public static LocString RECIPEDESC = (LocString) $"A crunchy stack of {UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED")} deep-fried in {UI.FormatAsLink("Tallow", "TALLOW")}.";
    }

    public class PICKLEDMEAL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pickled Meal", nameof (PICKLEDMEAL));
      public static LocString DESC = (LocString) "Meal Lice preserved in vinegar.\n\nIt's a rarely acquired taste.";
      public static LocString RECIPEDESC = (LocString) ((string) ITEMS.FOOD.BASICPLANTFOOD.NAME + " regrettably preserved in vinegar.");
    }

    public class FRIEDMUSHBAR
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mush Fry", nameof (FRIEDMUSHBAR));
      public static LocString DESC = (LocString) "Pan-fried, solidified mudslop.\n\nThe inside is almost completely uncooked, despite the crunch on the outside.";
      public static LocString RECIPEDESC = (LocString) "Pan-fried, solidified mudslop.";
    }

    public class RAWEGG
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Raw Egg", nameof (RAWEGG));
      public static LocString DESC = (LocString) $"A raw Egg that has been cracked open for use in {UI.FormatAsLink("Food", nameof (FOOD))} preparation.\n\nIt will never hatch.";
      public static LocString RECIPEDESC = (LocString) $"A raw egg that has been cracked open for use in {UI.FormatAsLink("Food", nameof (FOOD))} preparation.";
    }

    public class COOKEDEGG
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Omelette", nameof (COOKEDEGG));
      public static LocString DESC = (LocString) "Fluffed and folded Egg innards.\n\nIt turns out you do, in fact, have to break a few eggs to make it.";
      public static LocString RECIPEDESC = (LocString) "Fluffed and folded egg innards.";
    }

    public class FRIEDMUSHROOM
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Fried Mushroom", nameof (FRIEDMUSHROOM));
      public static LocString DESC = (LocString) $"A pan-fried dish made with a fruiting {UI.FormatAsLink("Dusk Cap", "MUSHROOM")}.\n\nIt has a thick, savory flavor with subtle earthy undertones.";
      public static LocString RECIPEDESC = (LocString) $"A pan-fried dish made with a fruiting {UI.FormatAsLink("Dusk Cap", "MUSHROOM")}.";
    }

    public class COOKEDPIKEAPPLE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pikeapple Skewer", nameof (COOKEDPIKEAPPLE));
      public static LocString DESC = (LocString) $"Grilling a {UI.FormatAsLink("Pikeapple", "HARDSKINBERRY")} softens its spikes, making it slighly less awkward to eat.\n\nIt does not diminish the smell.";
      public static LocString RECIPEDESC = (LocString) $"A grilled dish made with a fruiting {UI.FormatAsLink("Pikeapple", "HARDSKINBERRY")}.";
    }

    public class PRICKLEFRUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Bristle Berry", nameof (PRICKLEFRUIT));
      public static LocString DESC = (LocString) "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";
    }

    public class GRILLEDPRICKLEFRUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Gristle Berry", nameof (GRILLEDPRICKLEFRUIT));
      public static LocString DESC = (LocString) $"The grilled bud of a {UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT")}.\n\nHeat unlocked an exquisite taste in the fruit, though the burnt spines leave something to be desired.";
      public static LocString RECIPEDESC = (LocString) $"The grilled bud of a {UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT")}.";
    }

    public class SWAMPFRUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Bog Jelly", nameof (SWAMPFRUIT));
      public static LocString DESC = (LocString) "A fruit with an outer film that contains chewy gelatinous cubes.";
    }

    public class SWAMPDELIGHTS
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Swampy Delights", nameof (SWAMPDELIGHTS));
      public static LocString DESC = (LocString) $"Dried gelatinous cubes from a {UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT")}.\n\nEach cube has a wonderfully chewy texture and is lightly coated in a delicate powder.";
      public static LocString RECIPEDESC = (LocString) $"Dried gelatinous cubes from a {UI.FormatAsLink("Bog Jelly", "SWAMPFRUIT")}.";
    }

    public class WORMBASICFRUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Spindly Grubfruit", nameof (WORMBASICFRUIT));
      public static LocString DESC = (LocString) $"A {UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT")} that failed to develop properly.\n\nIt is nonetheless edible, and vaguely tasty.";
    }

    public class WORMBASICFOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Roast Grubfruit Nut", nameof (WORMBASICFOOD));
      public static LocString DESC = (LocString) $"Slow roasted {UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT")}.\n\nIt has a smoky aroma and tastes of coziness.";
      public static LocString RECIPEDESC = (LocString) $"Slow roasted {UI.FormatAsLink("Spindly Grubfruit", "WORMBASICFRUIT")}.";
    }

    public class WORMSUPERFRUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Grubfruit", nameof (WORMSUPERFRUIT));
      public static LocString DESC = (LocString) "A plump, healthy fruit with a honey-like taste.";
    }

    public class WORMSUPERFOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Grubfruit Preserve", nameof (WORMSUPERFOOD));
      public static LocString DESC = (LocString) $"A long lasting {UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT")} jam preserved in {UI.FormatAsLink("Sucrose", "SUCROSE")}.\n\nThe thick, goopy jam retains the shape of the jar when poured out, but the sweet taste can't be matched.";
      public static LocString RECIPEDESC = (LocString) $"A long lasting {UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT")} jam preserved in {UI.FormatAsLink("Sucrose", "SUCROSE")}.";
    }

    public class VINEFRUITJAM
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("", nameof (VINEFRUITJAM));
      public static LocString DESC = (LocString) "";
      public static LocString RECIPEDESC = (LocString) "";
    }

    public class BERRYPIE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mixed Berry Pie", nameof (BERRYPIE));
      public static LocString DESC = (LocString) $"A pie made primarily of {UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT")} and {UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT")}.\n\nThe mixture of berries creates a fragrant, colorful filling that packs a sweet punch.";
      public static LocString RECIPEDESC = (LocString) $"A pie made primarily of {UI.FormatAsLink("Grubfruit", "WORMSUPERFRUIT")} and {UI.FormatAsLink("Gristle Berries", "PRICKLEFRUIT")}.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Berry Pie", nameof (BERRYPIE));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Mixed Berry Pie", nameof (BERRYPIE))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class COLDWHEATBREAD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Frost Bun", nameof (COLDWHEATBREAD));
      public static LocString DESC = (LocString) $"A simple bun baked from {(string) ITEMS.FOOD.COLDWHEATSEED.NAME}.\n\nEach bite leaves a mild cooling sensation in one's mouth, even when the bun itself is warm.";
      public static LocString RECIPEDESC = (LocString) $"A simple bun baked from {(string) ITEMS.FOOD.COLDWHEATSEED.NAME}.";
    }

    public class BEAN
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Nosh Bean", nameof (BEAN));
      public static LocString DESC = (LocString) $"The crisp bean of a {UI.FormatAsLink("Nosh Sprout", "BEAN_PLANT")}.\n\nEach bite tastes refreshingly natural and wholesome.";
    }

    public class SPICENUT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pincha Peppernut", nameof (SPICENUT));
      public static LocString DESC = (LocString) $"The flavorful nut of a {UI.FormatAsLink("Pincha Pepperplant", "SPICE_VINE")}.\n\nThe bitter outer rind hides a rich, peppery core that is useful in cooking.";
    }

    public class VINEFRUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Ovagro Fig", nameof (VINEFRUIT));
      public static LocString DESC = (LocString) $"The fruit from an {UI.FormatAsLink("Ovagro Vine", "VINEMOTHER")}.\n\nIt's fun to squeeze as many as possible into a single mouthful.";
    }

    public class URCHINMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Uni", nameof (URCHINMEAT));
      public static LocString DESC = (LocString) $"The edible flesh of a {UI.FormatAsLink("Pinpoki", "URCHIN")}.\n\nA rare delicacy whose safe preparation involves destroying the valuable outer spikes.";
      public static LocString RECIPEDESC = (LocString) $"The edible flesh of a {UI.FormatAsLink("Pinpoki", "URCHIN")} prepared at the {UI.FormatAsLink("Sushi Bar", "SUSHIBAR")}.";
    }

    public class SPICEBREAD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pepper Bread", nameof (SPICEBREAD));
      public static LocString DESC = (LocString) $"A loaf of bread, lightly spiced with {UI.FormatAsLink("Pincha Peppernut", "SPICENUT")} for a mild bite.\n\nThere's a simple joy to be had in pulling it apart in one's fingers.";
      public static LocString RECIPEDESC = (LocString) $"A loaf of bread, lightly spiced with {UI.FormatAsLink("Pincha Peppernut", "SPICENUT")} for a mild bite.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Pepper Bread", nameof (SPICEBREAD));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Pepper Bread", nameof (SPICEBREAD))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class SURFANDTURF
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Surf'n'Turf", nameof (SURFANDTURF));
      public static LocString DESC = (LocString) $"A bit of {UI.FormatAsLink("Meat", "MEAT")} from the land and {UI.FormatAsLink("Cooked Seafood", "COOKEDFISH")} from the sea.\n\nIt's hearty and satisfying.";
      public static LocString RECIPEDESC = (LocString) $"A bit of {UI.FormatAsLink("Meat", "MEAT")} from the land and {UI.FormatAsLink("Cooked Seafood", "COOKEDFISH")} from the sea.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Surf'n'Turf", nameof (SURFANDTURF));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Surf'n'Turf", nameof (SURFANDTURF))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class TOFU
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Tofu", nameof (TOFU));
      public static LocString DESC = (LocString) $"A bland curd made from {UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED")}.\n\nIt has an unusual but pleasant consistency.";
      public static LocString RECIPEDESC = (LocString) $"A bland curd made from {UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED")}.";
    }

    public class SPICYTOFU
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Spicy Tofu", nameof (SPICYTOFU));
      public static LocString DESC = (LocString) $"{(string) ITEMS.FOOD.TOFU.NAME} marinated in a flavorful {UI.FormatAsLink("Pincha Peppernut", "SPICENUT")} sauce.\n\nIt packs a delightful punch.";
      public static LocString RECIPEDESC = (LocString) $"{(string) ITEMS.FOOD.TOFU.NAME} marinated in a flavorful {UI.FormatAsLink("Pincha Peppernut", "SPICENUT")} sauce.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Spicy Tofu", nameof (SPICYTOFU));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Spicy Tofu", nameof (SPICYTOFU))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class CURRY
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Curried Beans", nameof (CURRY));
      public static LocString DESC = (LocString) $"Chewy {UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED")} simmered with chunks of {(string) ITEMS.INGREDIENTS.GINGER.NAME}.\n\nIt's so spicy!";
      public static LocString RECIPEDESC = (LocString) $"Chewy {UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED")} simmered with chunks of {(string) ITEMS.INGREDIENTS.GINGER.NAME}.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Curried Beans", nameof (CURRY));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Curried Beans", nameof (CURRY))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class SALSA
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Stuffed Berry", nameof (SALSA));
      public static LocString DESC = (LocString) $"A baked {UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT")} stuffed with delectable spices and vibrantly flavored.";
      public static LocString RECIPEDESC = (LocString) $"A baked {UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT")} stuffed with delectable spices and vibrantly flavored.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Stuffed Berry", nameof (SALSA));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Stuffed Berry", nameof (SALSA))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class HARDSKINBERRY
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pikeapple", nameof (HARDSKINBERRY));
      public static LocString DESC = (LocString) "An edible fruit encased in a thorny husk.";
    }

    public class CARROT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Plume Squash", nameof (CARROT));
      public static LocString DESC = (LocString) "An edible tuber with an earthy, elegant flavor.";
    }

    public class FERNFOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Megafrond Grain", nameof (FERNFOOD));
      public static LocString DESC = (LocString) $"An ancient grain that can be processed into {UI.FormatAsLink("Food", nameof (FOOD))}.";
    }

    public class PEMMICAN
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pemmican", nameof (PEMMICAN));
      public static LocString DESC = (LocString) $"{UI.FormatAsLink("Meat", "MEAT")} and {UI.FormatAsLink("Tallow", "TALLOW")} pounded into a calorie-dense brick with an exceptionally long shelf life.\n\nSurvival never tasted so good.";
      public static LocString RECIPEDESC = (LocString) $"{UI.FormatAsLink("Meat", "MEAT")} and {UI.FormatAsLink("Tallow", "TALLOW")} pounded into a nutrient-dense brick with an exceptionally long shelf life.";
    }

    public class BASICPLANTFOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Meal Lice", nameof (BASICPLANTFOOD));
      public static LocString DESC = (LocString) "A flavorless grain that almost never wiggles on its own.";
    }

    public class BASICPLANTBAR
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Liceloaf", nameof (BASICPLANTBAR));
      public static LocString DESC = (LocString) (UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.");
      public static LocString RECIPEDESC = (LocString) (UI.FormatAsLink("Meal Lice", "BASICPLANTFOOD") + " compacted into a dense, immobile loaf.");
    }

    public class BASICFORAGEPLANT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Muckroot", nameof (BASICFORAGEPLANT));
      public static LocString DESC = (LocString) $"A seedless fruit with an upsettingly bland aftertaste.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a {(string) ITEMS.FOOD.BASICFORAGEPLANT.NAME}.";
    }

    public class FORESTFORAGEPLANT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Hexalent Fruit", nameof (FORESTFORAGEPLANT));
      public static LocString DESC = (LocString) "A seedless fruit with an unusual rubbery texture.\n\nIt cannot be replanted.\n\nHexalent fruit is much more calorie dense than Muckroot fruit.";
    }

    public class SWAMPFORAGEPLANT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Swamp Chard Heart", nameof (SWAMPFORAGEPLANT));
      public static LocString DESC = (LocString) "A seedless plant with a squishy, juicy center and an awful smell.\n\nIt cannot be replanted.";
    }

    public class ICECAVESFORAGEPLANT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Sherberry", nameof (ICECAVESFORAGEPLANT));
      public static LocString DESC = (LocString) "A cold seedless fruit that triggers mild brain freeze.\n\nIt cannot be replanted.";
    }

    public class AQUATICFORAGEPLANT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mussel Tongue", nameof (AQUATICFORAGEPLANT));
      public static LocString DESC = (LocString) $"A seedless plant with a gummy texture.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a {(string) ITEMS.FOOD.AQUATICFORAGEPLANT.NAME}.";
    }

    public class ROTPILE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Rot Pile", "COMPOST");
      public static LocString DESC = (LocString) $"An inedible glop of former foodstuff.\n\n{(string) ITEMS.FOOD.ROTPILE.NAME}s break down into {UI.FormatAsLink("Polluted Dirt", "TOXICSAND")} over time.";
    }

    public class COLDWHEATSEED
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Sleet Wheat Grain", nameof (COLDWHEATSEED));
      public static LocString DESC = (LocString) "An edible grain that leaves a cool taste on the tongue.";
    }

    public class BEANPLANTSEED
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Nosh Bean", nameof (BEANPLANTSEED));
      public static LocString DESC = (LocString) "An inedible bean that can be processed into delicious foods.";
    }

    public class QUICHE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mushroom Quiche", nameof (QUICHE));
      public static LocString DESC = (LocString) $"{UI.FormatAsLink("Omelette", "COOKEDEGG")}, {UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM")} and {UI.FormatAsLink("Lettuce", "LETTUCE")} piled onto a yummy crust.\n\nSomehow, it's both soggy <i>and</i> crispy.";
      public static LocString RECIPEDESC = (LocString) $"{UI.FormatAsLink("Omelette", "COOKEDEGG")}, {UI.FormatAsLink("Fried Mushroom", "FRIEDMUSHROOM")} and {UI.FormatAsLink("Lettuce", "LETTUCE")} piled onto a yummy crust.";

      public class DEHYDRATED
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Dried Mushroom Quiche", nameof (QUICHE));
        public static LocString DESC = (LocString) $"A dehydrated {UI.FormatAsLink("Mushroom Quiche", nameof (QUICHE))} ration. It must be rehydrated in order to be considered {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDry rations have no expiry date.";
      }
    }

    public class GARDENFOODPLANTFOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Sweatcorn", nameof (GARDENFOODPLANTFOOD));
      public static LocString DESC = (LocString) $"The sugary vegetable produced by {UI.FormatAsLink("Sweatcorn Stalks", "GARDENFOODPLANT")}.\n\nIt tastes a lot better deep-fried.";
    }

    public class GARDENFORAGEPLANT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Snac Fruit", nameof (GARDENFORAGEPLANT));
      public static LocString DESC = (LocString) $"A seedless fruit that loses its flavor long before it is fully chewed.\n\nIt cannot be replanted.\n\nDigging up Buried Objects may uncover a {(string) ITEMS.FOOD.GARDENFORAGEPLANT.NAME}.";
    }

    public class SALTYSTICKSFOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Salty Sticks", nameof (SALTYSTICKSFOOD));
      public static LocString DESC = (LocString) $"An extremely salty snack harvested from a {UI.FormatAsLink("Sodicane", "SALTYSTICKSPLANT")}.\n\nVisiting a {UI.FormatAsLink("Water Cooler", "WATERCOOLER")} will counteract the {UI.FormatAsLink("Thirst", "IDFOODEFFECTS")} caused by eating Salty Sticks.";
    }

    public class BUTTERFLYPLANTSEED
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mimillet", nameof (BUTTERFLYPLANTSEED));
      public static LocString DESC = (LocString) $"An inedible seed from a {UI.FormatAsLink("Mimika Bud", "BUTTERFLYPLANT")}.\n\nIt can be sown to cultivate more plants, or processed into {UI.FormatAsLink("Food", nameof (FOOD))}.\n\nDigging up Buried Objects may uncover a Mimillet Seed.";
      public static LocString RECIPEDESC = (LocString) $"An inedible {(string) CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.NAME} seed.";
    }

    public class BUTTERFLYFOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Toasted Mimillet", nameof (BUTTERFLYFOOD));
      public static LocString DESC = (LocString) $"A lightly toasted {(string) CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.NAME}.\n\nIt makes the tummy feel a bit fluttery.";
      public static LocString RECIPEDESC = (LocString) $"A lightly toasted {(string) CREATURES.SPECIES.SEEDS.BUTTERFLYPLANTSEED.NAME}.";
    }

    public class EDAMAME
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Edamame", nameof (EDAMAME));
      public static LocString DESC = (LocString) $"Raw {UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED")} sprinkled with {UI.FormatAsLink("Sodicane", "SALTYSTICKSPLANT")} shavings.\n\nSimple and satisfying.";
      public static LocString RECIPEDESC = (LocString) $"Raw {UI.FormatAsLink("Nosh Beans", "BEANPLANTSEED")} with {UI.FormatAsLink("Sodicane", "SALTYSTICKSPLANT")}.";
    }

    public class MAKI
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Sushi Roll", nameof (MAKI));
      public static LocString DESC = (LocString) $"Raw {UI.FormatAsLink("Fish Fillet", "FISHMEAT")} paired with {UI.FormatAsLink("Liceloaf", "BASICPLANTBAR")} and wrapped in {UI.FormatAsLink("Nori", "NORI")}.\n\nThe salty, crispy wrap balances perfectly with the juicy proteins inside.";
      public static LocString RECIPEDESC = (LocString) $"Raw {UI.FormatAsLink("Fish Fillet", "FISHMEAT")} paired with {UI.FormatAsLink("Liceloaf", "BASICPLANTBAR")} and {UI.FormatAsLink("Nori", "NORI")}.";
    }

    public class NIGIRI
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Nigiri", nameof (NIGIRI));
      public static LocString DESC = (LocString) $"Tenderized {UI.FormatAsLink("Calamari", "SQUIDMEAT")} and {UI.FormatAsLink("Liceloaf", "BASICPLANTBAR")} wrapped in {UI.FormatAsLink("Nori", "NORI")}.\n\nExpert preparation highlights the meat's delicate sweetness and soft, chewy texture.";
      public static LocString RECIPEDESC = (LocString) $"Tenderized {UI.FormatAsLink("Calamari", "SQUIDMEAT")} with {UI.FormatAsLink("Liceloaf", "BASICPLANTBAR")} and {UI.FormatAsLink("Nori", "NORI")}.";
    }

    public class SQUIDMEAT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Calamari", nameof (SQUIDMEAT));
      public static LocString DESC = (LocString) $"Uncooked meat from a very dead {(string) CREATURES.SPECIES.SQUID.NAME}.\n\nIt's a little slippery.";
      public static LocString RECIPEDESC = (LocString) $"Uncooked meat from a very dead {(string) CREATURES.SPECIES.SQUID.NAME}.";
    }

    public class NORI
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Nori", nameof (NORI));
      public static LocString DESC = (LocString) $"A bundle of slightly salty dried greenery from a {UI.FormatAsLink("Tower Kelp", "SEATREE")} or {UI.FormatAsLink("Kelpole", "SEAFAIRY")}.\n\nIt's most commonly used as an ingredient in higher-tier dishes.";
    }

    public class CAVIAR
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Caviar", nameof (CAVIAR));
      public static LocString DESC = (LocString) $"Shelf-stable eggs from an aquatic critter.\n\nIt can be added to {UI.FormatAsLink("Food", nameof (FOOD))} at a {UI.FormatAsLink("Mess Table", "DININGTABLE")} to boost {UI.FormatAsLink("Morale", "MORALE")}.\n\nDuplicants will automatically use Caviar while sitting at a {(string) BUILDINGS.PREFABS.DININGTABLE.NAME} during mealtime.";
    }

    public class MUSSELTONGUE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Mussel Tongue", nameof (MUSSELTONGUE));
      public static LocString DESC = (LocString) $"Edible tongue of a {UI.FormatAsLink("Mussel Sprout", "MUSSELSPROUT")}.\n\nIt's bitter and chewy, but better than starvation.";
      public static LocString RECIPEDESC = (LocString) $"Uncooked meat from a {UI.FormatAsLink("Mussel Sprout", "MUSSELSPROUT")}.";
    }
  }

  public class INGREDIENTS
  {
    public class SWAMPLILYFLOWER
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Balm Lily Flower", nameof (SWAMPLILYFLOWER));
      public static LocString DESC = (LocString) "A medicinal flower that soothes most minor maladies.\n\nIt is exceptionally fragrant.";
    }

    public class GINGER
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Tonic Root", "GINGERCONFIG");
      public static LocString DESC = (LocString) "A chewy, fibrous rhizome with a fiery aftertaste.";
    }

    public class KELP
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Seakomb Leaf", nameof (KELP));
      public static LocString DESC = (LocString) $"The leaf of a {UI.FormatAsLink("Seakomb", "KELPPLANT")}.\n\nIt can be processed into {UI.FormatAsLink("Phyto Oil", "PHYTOOIL")} or used as an ingredient in {UI.FormatAsLink("Allergy Medication", "ANTIHISTAMINE")}.";
    }
  }

  public class INDUSTRIAL_PRODUCTS
  {
    public class ELECTROBANK_URANIUM_ORE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Uranium Ore Power Bank", nameof (ELECTROBANK_URANIUM_ORE));
      public static LocString DESC = (LocString) $"A disposable {UI.FormatAsLink("Power Bank", "ELECTROBANK")} made with {UI.FormatAsLink("Uranium Ore", "URANIUMORE")}.\n\nIt can power buildings via {UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER")} or {UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER")}.\n\nDuplicants can produce new {UI.FormatAsLink("Uranium Ore Power Banks", "ELECTROBANK")} at the {UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE")}.\n\nMust be kept dry.";
    }

    public class ELECTROBANK_METAL_ORE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Metal Power Bank", nameof (ELECTROBANK_METAL_ORE));
      public static LocString DESC = (LocString) $"A disposable {UI.FormatAsLink("Power Bank", "ELECTROBANK")} made with {UI.FormatAsLink("Metal Ore", "METAL")}.\n\nIt can power buildings via {UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER")} or {UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER")}.\n\nDuplicants can produce new {UI.FormatAsLink("Metal Power Banks", "ELECTROBANK")} at the {UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE")}.\n\nMust be kept dry.";
    }

    public class ELECTROBANK_SELFCHARGING
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Atomic Power Bank", nameof (ELECTROBANK_SELFCHARGING));
      public static LocString DESC = (LocString) $"A self-charging {UI.FormatAsLink("Power Bank", "ELECTROBANK")} made with {(string) ELEMENTS.ENRICHEDURANIUM.NAME}.\n\nIt can power buildings via {UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER")} or {UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER")}.\n\nIts low {UI.FormatAsLink("wattage", "POWER")} and high {UI.FormatAsLink("Radioactivity", "RADIATION")} make it unsuitable for Bionic Duplicant use.";
    }

    public class ELECTROBANK
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Eco Power Bank", nameof (ELECTROBANK));
      public static LocString DESC = (LocString) $"A rechargeable {UI.FormatAsLink("Power Bank", nameof (ELECTROBANK))}.\n\nIt can power buildings via {UI.FormatAsLink("Large Dischargers", "LARGEELECTROBANKDISCHARGER")} or {UI.FormatAsLink("Compact Dischargers", "SMALLELECTROBANKDISCHARGER")}.\n\nDuplicants can produce new {UI.FormatAsLink("Eco Power Banks", nameof (ELECTROBANK))} at the {UI.FormatAsLink("Soldering Station", "ADVANCEDCRAFTINGTABLE")}.\n\nMust be kept dry.";
    }

    public class ELECTROBANK_EMPTY
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Empty Eco Power Bank", "ELECTROBANK");
      public static LocString DESC = (LocString) $"A depleted {UI.FormatAsLink("Power Bank", "ELECTROBANK")}.\n\nIt must be recharged at a {UI.FormatAsLink("Power Bank Charger", "ELECTROBANKCHARGER")} before it can be reused.";
    }

    public class ELECTROBANK_GARBAGE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Power Bank Scrap", "ELECTROBANK");
      public static LocString DESC = (LocString) $"A {UI.FormatAsLink("Power Bank", "ELECTROBANK")} that has reached the end of its lifetime.\n\nIt can be salvaged for {UI.FormatAsLink("Abyssalite", "KATAIRITE")} at the {UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER")}.";
    }

    public class FUEL_BRICK
    {
      public static LocString NAME = (LocString) "Fuel Brick";
      public static LocString DESC = (LocString) $"A densely compressed brick of combustible material.\n\nIt can be burned to produce a one-time burst of {UI.FormatAsLink("Power", "POWER")}.";
    }

    public class BASIC_FABRIC
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Reed Fiber", nameof (BASIC_FABRIC));
      public static LocString DESC = (LocString) $"A ball of raw cellulose used in the production of {UI.FormatAsLink("Clothing", "EQUIPMENT")} and textiles.";
    }

    public class PLANT_FIBER
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Plant Husk", nameof (PLANT_FIBER));
      public static LocString DESC = (LocString) $"A bundle of dried plant matter.\n\nIt can be eaten by {(string) CREATURES.FAMILY_PLURAL.MOOSPECIES}, or processed into {UI.FormatAsLink("fuel", "POWER")} or {UI.FormatAsLink("building materials", "BUILDINGMATERIALCLASSES")}.";
    }

    public class PLANKTON_CORAL_PRODUCT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Phosphorite", "PHOSPHORITE");
      public static LocString DESC = (LocString) $"A chunk of hardened mineral.\n\nIt can be eaten by {(string) CREATURES.FAMILY_PLURAL.PARROTFISHSPECIES}, or processed into various useful items.";
    }

    public class FEATHER_FABRIC
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Feather Fiber", nameof (FEATHER_FABRIC));
      public static LocString DESC = (LocString) $"A stalk of raw keratin used in the production of {UI.FormatAsLink("Clothing", "EQUIPMENT")} and textiles.";
    }

    public class DEWDRIP
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Dewdrip", nameof (DEWDRIP));
      public static LocString DESC = (LocString) $"A crystallized blob of {UI.FormatAsLink("Brackene", "MILK")} from the {UI.FormatAsLink("Dew Dripper", "DEWDRIPPERPLANT")}.";
    }

    public class TRAP_PARTS
    {
      public static LocString NAME = (LocString) "Trap Components";
      public static LocString DESC = (LocString) $"These components can be assembled into a {(string) BUILDINGS.PREFABS.CREATURETRAP.NAME} and used to catch {UI.FormatAsLink("Critters", "CREATURES")}.";
    }

    public class POWER_STATION_TOOLS
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Microchip", nameof (POWER_STATION_TOOLS));
      public static LocString DESC = (LocString) $"A specialized {(string) ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME} created by a professional engineer.\n\nTunes up {UI.FormatAsLink("Generators", "REQUIREMENTCLASSGENERATORTYPE")} to increase their {UI.FormatAsLink("Power", "POWER")} output.\n\nAlso used in the production of {UI.FormatAsLink("Boosters", "BOOSTER")} for Bionic Duplicants.";
      public static LocString TINKER_REQUIREMENT_NAME = (LocString) ("Skill: " + (string) DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME);
      public static LocString TINKER_REQUIREMENT_TOOLTIP = (LocString) $"Can only be used by a Duplicant with {(string) DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME} to apply a {UI.PRE_KEYWORD}Tune Up{UI.PST_KEYWORD}.";
      public static LocString TINKER_EFFECT_NAME = (LocString) "Engie's Tune-Up: {0} {1}";
      public static LocString TINKER_EFFECT_TOOLTIP = (LocString) $"Can be used to {UI.PRE_KEYWORD}Tune Up{UI.PST_KEYWORD} a generator, increasing its {{0}} by <b>{{1}}</b>.";
      public static LocString RECIPE_DESCRIPTION = (LocString) $"Make {(string) ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME} from {{0}}";
    }

    public class FARM_STATION_TOOLS
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Micronutrient Fertilizer", nameof (FARM_STATION_TOOLS));
      public static LocString DESC = (LocString) $"Specialized {UI.FormatAsLink("Fertilizer", "FERTILIZER")} mixed by a Duplicant with the {(string) DUPLICANTS.ROLES.FARMER.NAME} Skill.\n\nIncreases the {UI.PRE_KEYWORD}Growth Rate{UI.PST_KEYWORD} of one {UI.FormatAsLink("Plant", "PLANTS")}.";
    }

    public class PLASTIC_GASKET
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Plastic Gasket", nameof (PLASTIC_GASKET));
      public static LocString DESC = (LocString) $"A flexible {UI.FormatAsLink("Plastic", "POLYPROPYLENE")} seal, made at the {UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE")}.\n\nPrevents unwanted {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} leakage.";
    }

    public class RUBBER_GASKET
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Rubber Gasket", nameof (RUBBER_GASKET));
      public static LocString DESC = (LocString) $"A flexible {UI.FormatAsLink("Rubber", "RUBBER")} seal, made at the {UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE")}.\n\nPrevents unwanted {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} leakage.";
    }

    public class MACHINE_PARTS
    {
      public static LocString NAME = (LocString) "Custom Parts";
      public static LocString DESC = (LocString) $"Specialized Parts crafted by a professional engineer.\n\n{UI.PRE_KEYWORD}Jerry Rig{UI.PST_KEYWORD} machine buildings to increase their efficiency.";
      public static LocString TINKER_REQUIREMENT_NAME = (LocString) ("Job: " + (string) DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME);
      public static LocString TINKER_REQUIREMENT_TOOLTIP = (LocString) $"Can only be used by a Duplicant with {(string) DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME} to apply a {UI.PRE_KEYWORD}Jerry Rig{UI.PST_KEYWORD}.";
      public static LocString TINKER_EFFECT_NAME = (LocString) "Engineer's Jerry Rig: {0} {1}";
      public static LocString TINKER_EFFECT_TOOLTIP = (LocString) $"Can be used to {UI.PRE_KEYWORD}Jerry Rig{UI.PST_KEYWORD} upgrades to a machine building, increasing its {{0}} by <b>{{1}}</b>.";
    }

    public class RESEARCH_DATABANK
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Data Bank", "DATABANK");
      public static LocString NAME_PLURAL = (LocString) UI.FormatAsLink("Data Banks", "DATABANK");
      public static LocString DESC = (LocString) $"Raw data that can be processed into {UI.FormatAsLink("Interstellar Research", "RESEARCH")} points.";
    }

    public class ORBITAL_RESEARCH_DATABANK
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Data Bank", "DATABANK");
      public static LocString NAME_PLURAL = (LocString) UI.FormatAsLink("Data Banks", "DATABANK");
      public static LocString DESC = (LocString) $"Raw Data that can be processed into {UI.FormatAsLink("Data Analysis Research", "RESEARCHDLC1")} points.";
      public static LocString RECIPE_DESC = (LocString) $"Data Banks of raw data generated from exploring, either by exploring new areas with Duplicants, or by using an {UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER")}.\n\nUsed by the {UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER")} to conduct research.";
    }

    public class EGG_SHELL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Egg Shell", nameof (EGG_SHELL));
      public static LocString DESC = (LocString) $"Can be crushed to produce {UI.FormatAsLink("Lime", "LIME")}.";
    }

    public class GOLD_BELLY_CROWN
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Regal Bammoth Crest", nameof (GOLD_BELLY_CROWN));
      public static LocString DESC = (LocString) $"Can be crushed to produce {(string) ELEMENTS.GOLDAMALGAM.NAME}.";
    }

    public class CRAB_SHELL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pokeshell Molt", nameof (CRAB_SHELL));
      public static LocString DESC = (LocString) $"Can be crushed to produce {UI.FormatAsLink("Lime", "LIME")}.";

      public class VARIANT_WOOD
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Oakshell Molt", "CRABWOODSHELL");
        public static LocString DESC = (LocString) $"Can be crushed to produce {UI.FormatAsLink("Wood", "WOOD")}.";
      }
    }

    public class BABY_CRAB_SHELL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Small Pokeshell Molt", "CRAB_SHELL");
      public static LocString DESC = (LocString) $"Can be crushed to produce {UI.FormatAsLink("Lime", "LIME")}.";

      public class VARIANT_WOOD
      {
        public static LocString NAME = (LocString) UI.FormatAsLink("Small Oakshell Molt", "CRABWOODSHELL");
        public static LocString DESC = (LocString) $"Can be crushed to produce {UI.FormatAsLink("Wood", "WOOD")}.";
      }
    }

    public class SNAIL_SHELL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Slogo Molt", "SNAILSHELL");
      public static LocString DESC = (LocString) $"Can be crushed to produce {UI.FormatAsLink("Lime", "LIME")}.";
    }

    public class SNAIL_IRON_SHELL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Gildgo Molt", "SNAILIRONSHELL");
      public static LocString DESC = (LocString) $"Can be crushed to produce {UI.FormatAsLink("Gold Amalgam", "GOLDAMALGAM")}.";
    }

    public class WOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Wood", nameof (WOOD));
      public static LocString DESC = (LocString) $"Natural resource harvested from certain {UI.FormatAsLink("Critters", "CREATURES")} and {UI.FormatAsLink("Plants", "PLANTS")}.\n\nUsed in construction or {UI.FormatAsLink("Heat", "HEAT")} production.";
    }

    public class GENE_SHUFFLER_RECHARGE
    {
      public static LocString NAME = (LocString) "Vacillator Recharge";
      public static LocString DESC = (LocString) $"Replenishes one charge to a depleted {(string) BUILDINGS.PREFABS.GENESHUFFLER.NAME}.";
    }

    public class TABLE_SALT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Table Salt", "IDTABLESALT");
      public static LocString DESC = (LocString) $"A seasoning that Duplicants can add to their {UI.FormatAsLink("Food", "FOOD")} to boost {UI.FormatAsLink("Morale", "MORALE")}.\n\nDuplicants will automatically use Table Salt while sitting at a {(string) BUILDINGS.PREFABS.DININGTABLE.NAME} during mealtime.\n\n<i>Only the finest grains are chosen.</i>";
    }

    public class REFINED_SUGAR
    {
      public static LocString NAME = (LocString) "Refined Sugar";
      public static LocString DESC = (LocString) $"A seasoning that Duplicants can add to their {UI.FormatAsLink("Food", "FOOD")} to boost {UI.FormatAsLink("Morale", "MORALE")}.\n\nDuplicants will automatically use Refined Sugar while sitting at a {(string) BUILDINGS.PREFABS.DININGTABLE.NAME} during mealtime.\n\n<i>Only the finest grains are chosen.</i>";
    }

    public class URCHIN
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pinpoki", nameof (URCHIN));
      public static LocString DESC = (LocString) $"Pinpoki are aquatic organisms grown in {UI.FormatAsLink("Pinpokets", "URCHINPLANT")}.\n\nThey can be processed into {UI.FormatAsLink("Diamond", "DIAMOND")} or {UI.FormatAsLink("Uni", "URCHINMEAT")}, or used as {UI.FormatAsLink("Decor", "DECOR")}.";
    }

    public class ICE_BELLY_POOP
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Bammoth Patty", nameof (ICE_BELLY_POOP));
      public static LocString DESC = (LocString) $"A little treat left behind by a very large critter.\n\nIt can be crushed to extract {UI.FormatAsLink("Phosphorite", "PHOSPHORITE")} and {UI.FormatAsLink("Clay", "CLAY")}.";
    }
  }

  public class CARGO_CAPSULE
  {
    public static LocString NAME = (LocString) "Care Package";
    public static LocString DESC = (LocString) "A delivery system for recently printed resources.\n\nIt will dematerialize shortly.";
  }

  public class RAILGUNPAYLOAD
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Interplanetary Payload", nameof (RAILGUNPAYLOAD));
    public static LocString DESC = (LocString) $"Contains resources packed for interstellar shipping.\n\nCan be launched by a {(string) BUILDINGS.PREFABS.RAILGUN.NAME} or unpacked with a {(string) BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME}.";
  }

  public class MISSILE_BASIC
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Blastshot", nameof (MISSILE_BASIC));
    public static LocString DESC = (LocString) $"An explosive projectile designed to defend against meteor showers.\n\nMust be launched by a {UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER")}.";
  }

  public class MISSILE_LONGRANGE_VANILLADLC4
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Intracosmic Blastshot", nameof (MISSILE_LONGRANGE_VANILLADLC4));
    public static LocString DESC = (LocString) $"A long-range explosive projectile that defends against distant space objects.\n\nMust be launched by {UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER")}.";
  }

  public class MISSILE_LONGRANGE
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Intracosmic Blastshot", nameof (MISSILE_LONGRANGE));
    public static LocString DESC = (LocString) $"A long-range explosive projectile that defends against distant space objects.\n\nMust be launched by {UI.FormatAsLink("Meteor Blaster", "MISSILELAUNCHER")}.";
  }

  public class DEBRISPAYLOAD
  {
    public static LocString NAME = (LocString) "Rocket Debris";
    public static LocString DESC = (LocString) "Whatever is left over from a Rocket Self-Destruct can be recovered once it has crash-landed.";
  }

  public class RADIATION
  {
    public class HIGHENERGYPARITCLE
    {
      public static LocString NAME = (LocString) "Radbolts";
      public static LocString DESC = (LocString) $"A concentrated field of {UI.FormatAsKeyWord("Radbolts")} that can be largely redirected using a {UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR")}.";
    }
  }

  public class DREAMJOURNAL
  {
    public static LocString NAME = (LocString) UI.FormatAsLink("Dream Journal", "STORYTRAITMEGABRAINTANK");
    public static LocString DESC = (LocString) $"A hand-scrawled account of {UI.FormatAsLink("Pajama", "SLEEP_CLINIC_PAJAMAS")}-induced dreams.\n\nCan be analyzed using a {UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK")}.";
  }

  public class DEHYDRATEDFOODPACKAGE
  {
    public static LocString NAME = (LocString) "Dry Ration";
    public static LocString DESC = (LocString) "A package of non-perishable dehydrated food.\n\nIt requires no refrigeration, but must be rehydrated before consumption.";
    public static LocString CONSUMED = (LocString) "Ate Rehydrated Food";
    public static LocString CONTENTS = (LocString) "Dried {0}";
  }

  public class SPICES
  {
    public class MACHINERY_SPICE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Machinist Spice", nameof (MACHINERY_SPICE));
      public static LocString DESC = (LocString) "Improves operating skills when ingested.";
    }

    public class PILOTING_SPICE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Rocketeer Spice", nameof (PILOTING_SPICE));
      public static LocString DESC = (LocString) "Provides a boost to piloting abilities.";
    }

    public class PRESERVING_SPICE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Freshener Spice", nameof (PRESERVING_SPICE));
      public static LocString DESC = (LocString) "Slows the decomposition of perishable foods.";
    }

    public class STRENGTH_SPICE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Brawny Spice", nameof (STRENGTH_SPICE));
      public static LocString DESC = (LocString) "Strengthens even the weakest of muscles.";
    }
  }
}
