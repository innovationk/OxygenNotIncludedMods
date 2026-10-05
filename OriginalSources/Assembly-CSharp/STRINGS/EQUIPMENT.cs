// Decompiled with JetBrains decompiler
// Type: STRINGS.EQUIPMENT
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace STRINGS;

public class EQUIPMENT
{
  public class PREFABS
  {
    public class OXYGEN_MASK
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Oxygen Mask", nameof (OXYGEN_MASK));
      public static LocString DESC = (LocString) "Ensures my Duplicants can breathe easy... for a little while, anyways.";
      public static LocString EFFECT = (LocString) $"Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.\n\nMust be refilled with oxygen at an {UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER")} when depleted.";
      public static LocString RECIPE_DESC = (LocString) "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in toxic and low breathability environments.";
      public static LocString GENERICNAME = (LocString) "Suit";
      public static LocString WORN_NAME = (LocString) UI.FormatAsLink("Worn Oxygen Mask", nameof (OXYGEN_MASK));
      public static LocString WORN_DESC = (LocString) $"A worn out {UI.FormatAsLink("Oxygen Mask", nameof (OXYGEN_MASK))}.\n\nMasks can be repaired at a {UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE")}.";
      public static LocString REPAIR_WORN_RECIPE_NAME = (LocString) ("Repair " + (string) EQUIPMENT.PREFABS.OXYGEN_MASK.NAME);
      public static LocString REPAIR_WORN_DESC = (LocString) $"Restore an {UI.FormatAsLink("Oxygen Mask", nameof (OXYGEN_MASK))} to working order.";
    }

    public class ATMO_SUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Atmo Suit", nameof (ATMO_SUIT));
      public static LocString DESC = (LocString) "Ensures my Duplicants can breathe easy, anytime, anywhere.";
      public static LocString EFFECT = (LocString) $"Supplies Duplicants with {UI.FormatAsLink("Oxygen", "OXYGEN")} in toxic and low breathability environments, and protects against extreme temperatures.\n\nMust be refilled with oxygen at an {UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER")} when depleted.";
      public static LocString RECIPE_DESC = (LocString) $"Supplies Duplicants with {UI.FormatAsLink("Oxygen", "OXYGEN")} in toxic and low breathability environments.";
      public static LocString GENERICNAME = (LocString) "Suit";
      public static LocString WORN_NAME = (LocString) UI.FormatAsLink("Worn Atmo Suit", nameof (ATMO_SUIT));
      public static LocString WORN_DESC = (LocString) $"A worn out {UI.FormatAsLink("Atmo Suit", nameof (ATMO_SUIT))}.\n\nSuits can be repaired at an {UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR")}.";
      public static LocString REPAIR_WORN_RECIPE_NAME = (LocString) ("Repair " + (string) EQUIPMENT.PREFABS.ATMO_SUIT.NAME);
      public static LocString REPAIR_WORN_DESC = (LocString) $"Restore a {UI.FormatAsLink("Worn Atmo Suit", nameof (ATMO_SUIT))} to working order.";
    }

    public class ATMO_SUIT_SET
    {
      public class PUFT
      {
        public static LocString NAME = (LocString) "Puft Atmo Suit";
        public static LocString DESC = (LocString) "Critter-forward protective gear for the intrepid explorer!\nReleased for Klei Fest 2023.";
      }
    }

    public class HOLIDAY_2023_CRATE
    {
      public static LocString NAME = (LocString) "Holiday Gift Crate";
      public static LocString DESC = (LocString) "An unaddressed package has been discovered near the Printing Pod. It exudes seasonal cheer, and trace amounts of Neutronium have been detected.";
    }

    public class ATMO_SUIT_HELMET
    {
      public static LocString NAME = (LocString) "Default Atmo Helmet";
      public static LocString DESC = (LocString) "Default helmet for atmo suits.";

      public class FACADES
      {
        public class SPARKLE_RED
        {
          public static LocString NAME = (LocString) "Red Glitter Atmo Helmet";
          public static LocString DESC = (LocString) "Protective gear at its sparkliest.";
        }

        public class SPARKLE_GREEN
        {
          public static LocString NAME = (LocString) "Green Glitter Atmo Helmet";
          public static LocString DESC = (LocString) "Protective gear at its sparkliest.";
        }

        public class SPARKLE_BLUE
        {
          public static LocString NAME = (LocString) "Blue Glitter Atmo Helmet";
          public static LocString DESC = (LocString) "Protective gear at its sparkliest.";
        }

        public class SPARKLE_PURPLE
        {
          public static LocString NAME = (LocString) "Violet Glitter Atmo Helmet";
          public static LocString DESC = (LocString) "Protective gear at its sparkliest.";
        }

        public class LIMONE
        {
          public static LocString NAME = (LocString) "Citrus Atmo Helmet";
          public static LocString DESC = (LocString) "Fresh, fruity and full of breathable air.";
        }

        public class PUFT
        {
          public static LocString NAME = (LocString) "Puft Atmo Helmet";
          public static LocString DESC = (LocString) "Convincing enough to fool most Pufts and even a few Duplicants.\nReleased for Klei Fest 2023.";
        }

        public class CLUBSHIRT_PURPLE
        {
          public static LocString NAME = (LocString) "Eggplant Atmo Helmet";
          public static LocString DESC = (LocString) "It is neither an egg, nor a plant. But it <i>is</i> a functional helmet.";
        }

        public class TRIANGLES_TURQ
        {
          public static LocString NAME = (LocString) "Confetti Atmo Helmet";
          public static LocString DESC = (LocString) "Doubles as a party hat.";
        }

        public class CUMMERBUND_RED
        {
          public static LocString NAME = (LocString) "Blastoff Atmo Helmet";
          public static LocString DESC = (LocString) "Red means go!";
        }

        public class WORKOUT_LAVENDER
        {
          public static LocString NAME = (LocString) "Pink Punch Atmo Helmet";
          public static LocString DESC = (LocString) "Unapologetically ostentatious.";
        }

        public class CANTALOUPE
        {
          public static LocString NAME = (LocString) "Rocketmelon Atmo Helmet";
          public static LocString DESC = (LocString) "A melon for your melon.";
        }

        public class MONDRIAN_BLUE_RED_YELLOW
        {
          public static LocString NAME = (LocString) "Cubist Atmo Helmet";
          public static LocString DESC = (LocString) "Abstract geometrics are both hip <i>and</i> square.";
        }

        public class OVERALLS_RED
        {
          public static LocString NAME = (LocString) "Spiffy Atmo Helmet";
          public static LocString DESC = (LocString) "The twin antennae serve as an early warning system for low ceilings.";
        }
      }
    }

    public class ATMO_SUIT_BODY
    {
      public static LocString NAME = (LocString) "Default Atmo Uniform";
      public static LocString DESC = (LocString) "Default top and bottom of an atmo suit.";

      public class FACADES
      {
        public class SPARKLE_RED
        {
          public static LocString NAME = (LocString) "Red Glitter Atmo Suit";
          public static LocString DESC = (LocString) "Protects the wearer from hostile environments <i>and</i> drab fashion.";
        }

        public class SPARKLE_GREEN
        {
          public static LocString NAME = (LocString) "Green Glitter Atmo Suit";
          public static LocString DESC = (LocString) "Protects the wearer from hostile environments <i>and</i> drab fashion.";
        }

        public class SPARKLE_BLUE
        {
          public static LocString NAME = (LocString) "Blue Glitter Atmo Suit";
          public static LocString DESC = (LocString) "Protects the wearer from hostile environments <i>and</i> drab fashion.";
        }

        public class SPARKLE_LAVENDER
        {
          public static LocString NAME = (LocString) "Violet Glitter Atmo Suit";
          public static LocString DESC = (LocString) "Protects the wearer from hostile environments <i>and</i> drab fashion.";
        }

        public class LIMONE
        {
          public static LocString NAME = (LocString) "Citrus Atmo Suit";
          public static LocString DESC = (LocString) "Perfect for summery, atmospheric excursions.";
        }

        public class PUFT
        {
          public static LocString NAME = (LocString) "Puft Atmo Suit";
          public static LocString DESC = (LocString) "Warning: prolonged wear may result in feelings of Puft-up pride.\nReleased for Klei Fest 2023.";
        }

        public class BASIC_PURPLE
        {
          public static LocString NAME = (LocString) "Crisp Eggplant Atmo Suit";
          public static LocString DESC = (LocString) "It really emphasizes wide shoulders.";
        }

        public class PRINT_TRIANGLES_TURQ
        {
          public static LocString NAME = (LocString) "Confetti Atmo Suit";
          public static LocString DESC = (LocString) "It puts the \"fun\" in \"perfunctory nods to personnel individuality\"!";
        }

        public class BASIC_NEON_PINK
        {
          public static LocString NAME = (LocString) "Crisp Neon Pink Atmo Suit";
          public static LocString DESC = (LocString) "The neck is a little snug.";
        }

        public class MULTI_RED_BLACK
        {
          public static LocString NAME = (LocString) "Red-bellied Atmo Suit";
          public static LocString DESC = (LocString) "It really highlights the midsection.";
        }

        public class CANTALOUPE
        {
          public static LocString NAME = (LocString) "Rocketmelon Atmo Suit";
          public static LocString DESC = (LocString) "It starts to smell ripe pretty quickly.";
        }

        public class MULTI_BLUE_GREY_BLACK
        {
          public static LocString NAME = (LocString) "Swagger Atmo Suit";
          public static LocString DESC = (LocString) "Engineered to resemble stonewashed denim and black leather.";
        }

        public class MULTI_BLUE_YELLOW_RED
        {
          public static LocString NAME = (LocString) "Fundamental Stripe Atmo Suit";
          public static LocString DESC = (LocString) "Designed by the Primary Colors Appreciation Society.";
        }
      }
    }

    public class ATMO_SUIT_GLOVES
    {
      public static LocString NAME = (LocString) "Default Atmo Gloves";
      public static LocString DESC = (LocString) "Default atmo suit gloves.";

      public class FACADES
      {
        public class SPARKLE_RED
        {
          public static LocString NAME = (LocString) "Red Glitter Atmo Gloves";
          public static LocString DESC = (LocString) "Sparkly red gloves for hostile environments.";
        }

        public class SPARKLE_GREEN
        {
          public static LocString NAME = (LocString) "Green Glitter Atmo Gloves";
          public static LocString DESC = (LocString) "Sparkly green gloves for hostile environments.";
        }

        public class SPARKLE_BLUE
        {
          public static LocString NAME = (LocString) "Blue Glitter Atmo Gloves";
          public static LocString DESC = (LocString) "Sparkly blue gloves for hostile environments.";
        }

        public class SPARKLE_LAVENDER
        {
          public static LocString NAME = (LocString) "Violet Glitter Atmo Gloves";
          public static LocString DESC = (LocString) "Sparkly violet gloves for hostile environments.";
        }

        public class LIMONE
        {
          public static LocString NAME = (LocString) "Citrus Atmo Gloves";
          public static LocString DESC = (LocString) "Lime-inspired gloves brighten up hostile environments.";
        }

        public class PUFT
        {
          public static LocString NAME = (LocString) "Puft Atmo Gloves";
          public static LocString DESC = (LocString) "A little Puft-love for delicate extremities.\nReleased for Klei Fest 2023.";
        }

        public class GOLD
        {
          public static LocString NAME = (LocString) "Gold Atmo Gloves";
          public static LocString DESC = (LocString) "A golden touch! Without all the Midas-type baggage.";
        }

        public class PURPLE
        {
          public static LocString NAME = (LocString) "Eggplant Atmo Gloves";
          public static LocString DESC = (LocString) "Fab purple gloves for hostile environments.";
        }

        public class WHITE
        {
          public static LocString NAME = (LocString) "White Atmo Gloves";
          public static LocString DESC = (LocString) "For the Duplicant who never gets their hands dirty.";
        }

        public class STRIPES_LAVENDER
        {
          public static LocString NAME = (LocString) "Wildberry Atmo Gloves";
          public static LocString DESC = (LocString) "Functional finger-protectors with fruity flair.";
        }

        public class CANTALOUPE
        {
          public static LocString NAME = (LocString) "Rocketmelon Atmo Gloves";
          public static LocString DESC = (LocString) "It takes eighteen melon rinds to make a single glove.";
        }

        public class BROWN
        {
          public static LocString NAME = (LocString) "Leather Atmo Gloves";
          public static LocString DESC = (LocString) "They creak rather loudly during the break-in period.";
        }
      }
    }

    public class ATMO_SUIT_BELT
    {
      public static LocString NAME = (LocString) "Default Atmo Belt";
      public static LocString DESC = (LocString) "Default belt for atmo suits.";

      public class FACADES
      {
        public class SPARKLE_RED
        {
          public static LocString NAME = (LocString) "Red Glitter Atmo Belt";
          public static LocString DESC = (LocString) "It's red! It's shiny! It keeps atmo suit pants on!";
        }

        public class SPARKLE_GREEN
        {
          public static LocString NAME = (LocString) "Green Glitter Atmo Belt";
          public static LocString DESC = (LocString) "It's green! It's shiny! It keeps atmo suit pants on!";
        }

        public class SPARKLE_BLUE
        {
          public static LocString NAME = (LocString) "Blue Glitter Atmo Belt";
          public static LocString DESC = (LocString) "It's blue! It's shiny! It keeps atmo suit pants on!";
        }

        public class SPARKLE_LAVENDER
        {
          public static LocString NAME = (LocString) "Violet Glitter Atmo Belt";
          public static LocString DESC = (LocString) "It's violet! It's shiny! It keeps atmo suit pants on!";
        }

        public class LIMONE
        {
          public static LocString NAME = (LocString) "Citrus Atmo Belt";
          public static LocString DESC = (LocString) "This lime-hued belt really pulls an atmo suit together.";
        }

        public class PUFT
        {
          public static LocString NAME = (LocString) "Puft Atmo Belt";
          public static LocString DESC = (LocString) "If critters wore belts...\nReleased for Klei Fest 2023.";
        }

        public class TWOTONE_PURPLE
        {
          public static LocString NAME = (LocString) "Eggplant Atmo Belt";
          public static LocString DESC = (LocString) "In the more pretentious space-fashion circles, it's known as \"aubergine.\"";
        }

        public class BASIC_GOLD
        {
          public static LocString NAME = (LocString) "Gold Atmo Belt";
          public static LocString DESC = (LocString) "Better to be overdressed than underdressed.";
        }

        public class BASIC_GREY
        {
          public static LocString NAME = (LocString) "Slate Atmo Belt";
          public static LocString DESC = (LocString) "Slick and understated space style.";
        }

        public class BASIC_NEON_PINK
        {
          public static LocString NAME = (LocString) "Neon Pink Atmo Belt";
          public static LocString DESC = (LocString) "Visible from several planetoids away.";
        }

        public class CANTALOUPE
        {
          public static LocString NAME = (LocString) "Rocketmelon Atmo Belt";
          public static LocString DESC = (LocString) "A tribute to the <i>cucumis melo cantalupensis</i>.";
        }

        public class TWOTONE_BROWN
        {
          public static LocString NAME = (LocString) "Leather Atmo Belt";
          public static LocString DESC = (LocString) "Crafted from the tanned hide of a thick-skinned critter.";
        }
      }
    }

    public class ATMO_SUIT_SHOES
    {
      public static LocString NAME = (LocString) "Default Atmo Boots";
      public static LocString DESC = (LocString) "Default footwear for atmo suits.";

      public class FACADES
      {
        public class LIMONE
        {
          public static LocString NAME = (LocString) "Citrus Atmo Boots";
          public static LocString DESC = (LocString) "Cheery boots for stomping around in hostile environments.";
        }

        public class PUFT
        {
          public static LocString NAME = (LocString) "Puft Atmo Boots";
          public static LocString DESC = (LocString) "These boots were made for puft-ing.\nReleased for Klei Fest 2023.";
        }

        public class SPARKLE_BLACK
        {
          public static LocString NAME = (LocString) "Black Glitter Atmo Boots";
          public static LocString DESC = (LocString) "A timeless color, with a little pizzazz.";
        }

        public class BASIC_BLACK
        {
          public static LocString NAME = (LocString) "Stealth Atmo Boots";
          public static LocString DESC = (LocString) "They attract no attention at all.";
        }

        public class BASIC_PURPLE
        {
          public static LocString NAME = (LocString) "Eggplant Atmo Boots";
          public static LocString DESC = (LocString) "Purple boots for stomping around in hostile environments.";
        }

        public class BASIC_LAVENDER
        {
          public static LocString NAME = (LocString) "Lavender Atmo Boots";
          public static LocString DESC = (LocString) "Soothing space booties for tired feet.";
        }

        public class CANTALOUPE
        {
          public static LocString NAME = (LocString) "Rocketmelon Atmo Boots";
          public static LocString DESC = (LocString) "Keeps feet safe (and juicy) in hostile environments.";
        }
      }
    }

    public class JET_SUIT_SHOES
    {
      public static LocString NAME = (LocString) "Default Jet Boots";
      public static LocString DESC = (LocString) "Default footwear for jet suits.";
    }

    public class JET_SUIT_HELMET
    {
      public static LocString NAME = (LocString) "Default Jet Helmet";
      public static LocString DESC = (LocString) "Default helmet for jet suits.";
    }

    public class JET_SUIT_BODY
    {
      public static LocString NAME = (LocString) "Default Jet Uniform";
      public static LocString DESC = (LocString) "Default top and bottom of a jet suit.";
    }

    public class JET_SUIT_GLOVES
    {
      public static LocString NAME = (LocString) "Default Jet Gloves";
      public static LocString DESC = (LocString) "Default gloves for jet suits.";
    }

    public class AQUA_SUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Aqua Suit", nameof (AQUA_SUIT));
      public static LocString DESC = (LocString) "Because breathing underwater is better than... not.";
      public static LocString EFFECT = (LocString) $"Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.\n\nMust be refilled with {UI.FormatAsLink("Oxygen", "OXYGEN")} at an {UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER")} when depleted.";
      public static LocString RECIPE_DESC = (LocString) "Supplies Duplicants with <style=\"oxygen\">Oxygen</style> in underwater environments.";
      public static LocString WORN_NAME = (LocString) UI.FormatAsLink("Worn Lead Suit", nameof (AQUA_SUIT));
      public static LocString WORN_DESC = (LocString) $"A worn out {UI.FormatAsLink("Aqua Suit", nameof (AQUA_SUIT))}.\n\nSuits can be repaired at a {UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE")}.";
    }

    public class TEMPERATURE_SUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Thermo Suit", nameof (TEMPERATURE_SUIT));
      public static LocString DESC = (LocString) "Keeps my Duplicants cool in case things heat up.";
      public static LocString EFFECT = (LocString) "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.\n\nMust be powered at a Thermo Suit Dock when depleted.";
      public static LocString RECIPE_DESC = (LocString) "Provides insulation in regions with extreme <style=\"heat\">Temperatures</style>.";
      public static LocString WORN_NAME = (LocString) UI.FormatAsLink("Worn Lead Suit", nameof (TEMPERATURE_SUIT));
      public static LocString WORN_DESC = (LocString) $"A worn out {UI.FormatAsLink("Thermo Suit", nameof (TEMPERATURE_SUIT))}.\n\nSuits can be repaired at a {UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE")}.";
    }

    public class JET_SUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Jet Suit", nameof (JET_SUIT));
      public static LocString DESC = (LocString) "Allows my Duplicants to take to the skies, for a time.";
      public static LocString EFFECT = (LocString) $"Supplies Duplicants with {UI.FormatAsLink("Oxygen", "OXYGEN")} in toxic and low breathability environments.\n\nMust be refilled with {UI.FormatAsLink("Oxygen", "OXYGEN")} and {(string) MISC.TAGS.COMBUSTIBLELIQUID} at a {UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER")} when depleted.";
      public static LocString RECIPE_DESC = (LocString) $"Supplies Duplicants with {UI.FormatAsLink("Oxygen", "OXYGEN")} in toxic and low breathability environments.\n\nAllows Duplicant flight.";
      public static LocString GENERICNAME = (LocString) "Jet Suit";
      public static LocString TANK_EFFECT_NAME = (LocString) "Fuel Tank";
      public static LocString WORN_NAME = (LocString) UI.FormatAsLink("Worn Jet Suit", nameof (JET_SUIT));
      public static LocString WORN_DESC = (LocString) $"A worn out {UI.FormatAsLink("Jet Suit", nameof (JET_SUIT))}.\n\nSuits can be repaired at an {UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR")}.";
      public static LocString REPAIR_WORN_RECIPE_NAME = (LocString) ("Repair " + (string) EQUIPMENT.PREFABS.JET_SUIT.NAME);
      public static LocString REPAIR_WORN_DESC = (LocString) $"Restore a {UI.FormatAsLink("Worn Jet Suit", nameof (JET_SUIT))} to working order.";
    }

    public class LEAD_SUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Lead Suit", nameof (LEAD_SUIT));
      public static LocString DESC = (LocString) "Because exposure to radiation doesn't grant Duplicants superpowers.";
      public static LocString EFFECT = (LocString) $"Supplies Duplicants with {UI.FormatAsLink("Oxygen", "OXYGEN")} and protection in areas with {UI.FormatAsLink("Radiation", "RADIATION")}.\n\nMust be refilled with {UI.FormatAsLink("Oxygen", "OXYGEN")} at a {UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER")} when depleted.";
      public static LocString RECIPE_DESC = (LocString) $"Supplies Duplicants with {UI.FormatAsLink("Oxygen", "OXYGEN")} in toxic and low breathability environments.\n\nProtects Duplicants from {UI.FormatAsLink("Radiation", "RADIATION")}.";
      public static LocString GENERICNAME = (LocString) "Lead Suit";
      public static LocString BATTERY_EFFECT_NAME = (LocString) "Suit Battery";
      public static LocString SUIT_OUT_OF_BATTERIES = (LocString) "Suit Batteries Empty";
      public static LocString WORN_NAME = (LocString) UI.FormatAsLink("Worn Lead Suit", nameof (LEAD_SUIT));
      public static LocString WORN_DESC = (LocString) $"A worn out {UI.FormatAsLink("Lead Suit", nameof (LEAD_SUIT))}.\n\nSuits can be repaired at an {UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR")}.";
      public static LocString REPAIR_WORN_RECIPE_NAME = (LocString) ("Repair " + (string) EQUIPMENT.PREFABS.LEAD_SUIT.NAME);
      public static LocString REPAIR_WORN_DESC = (LocString) $"Restore a {UI.FormatAsLink("Worn Lead Suit", nameof (LEAD_SUIT))} to working order.";
    }

    public class RUBBERBOOTS
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Rubber Boots", nameof (RUBBERBOOTS));
      public static LocString GENERICNAME = (LocString) "Boots";
      public static LocString DESC = (LocString) "Grippy waterproof boots make chores much more manageable.";
      public static LocString EFFECT = (LocString) $"Protects the wearer from {UI.PRE_KEYWORD}Soggy Feet{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Slippery{UI.PST_KEYWORD} floors.";
      public static LocString EFFECT_DLC3 = (LocString) $"Protects the wearer from {UI.PRE_KEYWORD}Soggy Feet{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Slippery{UI.PST_KEYWORD} floors.\n\nEnables Bionic Duplicants to cross small puddles without {UI.PRE_KEYWORD}Liquid Exposure{UI.PST_KEYWORD} damage.";
      public static LocString RECIPE_DESC = EQUIPMENT.PREFABS.RUBBERBOOTS.EFFECT;
      public static LocString RECIPE_DESC_DLC3 = EQUIPMENT.PREFABS.RUBBERBOOTS.EFFECT_DLC3;
      public static LocString WORN_NAME = (LocString) UI.FormatAsLink("Worn Rubber Boots", nameof (RUBBERBOOTS));
      public static LocString WORN_DESC = (LocString) $"A worn out pair of {UI.FormatAsLink("Rubber Boots", nameof (RUBBERBOOTS))}.\nBoots can be repaired at a {UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE")}.";
      public static LocString REPAIR_WORN_RECIPE_NAME = (LocString) $"Repair {UI.FormatAsLink("Rubber Boots", nameof (RUBBERBOOTS))}.";
      public static LocString REPAIR_WORN_DESC = (LocString) $"Restore a {UI.FormatAsLink("Rubber Boots", nameof (RUBBERBOOTS))} to working order.";
    }

    public class COOL_VEST
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Cool Vest", nameof (COOL_VEST));
      public static LocString GENERICNAME = (LocString) "Clothing";
      public static LocString DESC = (LocString) "Don't sweat it!";
      public static LocString EFFECT = (LocString) "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation.";
      public static LocString RECIPE_DESC = (LocString) "Protects the wearer from <style=\"heat\">Heat</style> by decreasing insulation";
    }

    public class WARM_VEST
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Warm Coat", nameof (WARM_VEST));
      public static LocString GENERICNAME = (LocString) "Clothing";
      public static LocString DESC = (LocString) "Happiness is a warm Duplicant.";
      public static LocString EFFECT = (LocString) "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation.";
      public static LocString RECIPE_DESC = (LocString) "Protects the wearer from <style=\"heat\">Cold</style> by increasing insulation";
    }

    public class FUNKY_VEST
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Snazzy Suit", nameof (FUNKY_VEST));
      public static LocString GENERICNAME = (LocString) "Clothing";
      public static LocString DESC = (LocString) "This transforms my Duplicant into a walking beacon of charm and style.";
      public static LocString EFFECT = (LocString) $"Increases Decor in a small area effect around the wearer. Can be upgraded to {UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING")} at the {UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION")}.";
      public static LocString RECIPE_DESC = (LocString) $"Increases Decor in a small area effect around the wearer. Can be upgraded to {UI.FormatAsLink("Primo Garb", "CUSTOMCLOTHING")} at the {UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION")}";
    }

    public class DRYSUIT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Swimwear", "DrySuit");
      public static LocString GENERICNAME = (LocString) "Clothing";
      public static LocString DESC = (LocString) "Keeps Duplicants comfy and dry while swimming.";
      public static LocString EFFECT = (LocString) $"Protects the wearer from {UI.PRE_KEYWORD}Soggy Feet{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Sopping Wet{UI.PST_KEYWORD} effects.";
      public static LocString RECIPE_DESC = (LocString) $"Protects the wearer from  {UI.PRE_KEYWORD}Soggy Feet{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Sopping Wet{UI.PST_KEYWORD} effects.";
      public static LocString RECIPE_DESC_DLC3 = (LocString) $"Protects the wearer from  {UI.PRE_KEYWORD}Soggy Feet{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Sopping Wet{UI.PST_KEYWORD} effects.\n\nEnables Bionic Duplicants to cross waist-deep pools without {UI.PRE_KEYWORD}Liquid Exposure{UI.PST_KEYWORD} damage.";
    }

    public class CUSTOMCLOTHING
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Primo Garb", nameof (CUSTOMCLOTHING));
      public static LocString GENERICNAME = (LocString) "Clothing";
      public static LocString DESC = (LocString) "This transforms my Duplicant into a colony-inspiring fashion icon.";
      public static LocString EFFECT = (LocString) "Increases Decor in a small area effect around the wearer.";
      public static LocString RECIPE_DESC = (LocString) "Increases Decor in a small area effect around the wearer";

      public class FACADES
      {
        public static LocString CLUBSHIRT = (LocString) UI.FormatAsLink("Purple Polyester Suit", nameof (CUSTOMCLOTHING));
        public static LocString CUMMERBUND = (LocString) UI.FormatAsLink("Classic Cummerbund", nameof (CUSTOMCLOTHING));
        public static LocString DECOR_02 = (LocString) UI.FormatAsLink("Snazzier Red Suit", nameof (CUSTOMCLOTHING));
        public static LocString DECOR_03 = (LocString) UI.FormatAsLink("Snazzier Blue Suit", nameof (CUSTOMCLOTHING));
        public static LocString DECOR_04 = (LocString) UI.FormatAsLink("Snazzier Green Suit", nameof (CUSTOMCLOTHING));
        public static LocString DECOR_05 = (LocString) UI.FormatAsLink("Snazzier Violet Suit", nameof (CUSTOMCLOTHING));
        public static LocString GAUDYSWEATER = (LocString) UI.FormatAsLink("Pompom Knit Suit", nameof (CUSTOMCLOTHING));
        public static LocString LIMONE = (LocString) UI.FormatAsLink("Citrus Spandex Suit", nameof (CUSTOMCLOTHING));
        public static LocString MONDRIAN = (LocString) UI.FormatAsLink("Cubist Knit Suit", nameof (CUSTOMCLOTHING));
        public static LocString OVERALLS = (LocString) UI.FormatAsLink("Spiffy Overalls", nameof (CUSTOMCLOTHING));
        public static LocString TRIANGLES = (LocString) UI.FormatAsLink("Confetti Suit", nameof (CUSTOMCLOTHING));
        public static LocString WORKOUT = (LocString) UI.FormatAsLink("Pink Unitard", nameof (CUSTOMCLOTHING));
      }
    }

    public class CLOTHING_GLOVES
    {
      public static LocString NAME = (LocString) "Default Gloves";
      public static LocString DESC = (LocString) "The default gloves.";

      public class FACADES
      {
        public class STANDARD_GOLD
        {
          public static LocString NAME = (LocString) "Standard Gloves";
          public static LocString DESC = (LocString) "Standard-issue gloves for colony workers.";
        }

        public class BASIC_BLUE_MIDDLE
        {
          public static LocString NAME = (LocString) "Basic Aqua Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of aqua-blue gloves that go with everything.";
        }

        public class BASIC_YELLOW
        {
          public static LocString NAME = (LocString) "Basic Yellow Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of yellow gloves that go with everything.";
        }

        public class BASIC_BLACK
        {
          public static LocString NAME = (LocString) "Basic Black Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of black gloves that go with everything.";
        }

        public class BASIC_PINK_ORCHID
        {
          public static LocString NAME = (LocString) "Basic Bubblegum Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of bubblegum-pink gloves that go with everything.";
        }

        public class BASIC_GREEN
        {
          public static LocString NAME = (LocString) "Basic Green Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of green gloves that go with everything.";
        }

        public class BASIC_ORANGE
        {
          public static LocString NAME = (LocString) "Basic Orange Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of orange gloves that go with everything.";
        }

        public class BASIC_PURPLE
        {
          public static LocString NAME = (LocString) "Basic Purple Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of purple gloves that go with everything.";
        }

        public class BASIC_RED
        {
          public static LocString NAME = (LocString) "Basic Red Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of red gloves that go with everything.";
        }

        public class BASIC_WHITE
        {
          public static LocString NAME = (LocString) "Basic White Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of white gloves that go with everything.";
        }

        public class GLOVES_ATHLETIC_DEEPRED
        {
          public static LocString NAME = (LocString) "Team Captain Sports Gloves";
          public static LocString DESC = (LocString) "Red-striped gloves for winning at any activity.";
        }

        public class GLOVES_ATHLETIC_SATSUMA
        {
          public static LocString NAME = (LocString) "Superfan Sports Gloves";
          public static LocString DESC = (LocString) "Orange-striped gloves for enthusiastic athletes.";
        }

        public class GLOVES_ATHLETIC_LEMON
        {
          public static LocString NAME = (LocString) "Hype Sports Gloves";
          public static LocString DESC = (LocString) "Yellow-striped gloves for athletes who seek to raise the bar.";
        }

        public class GLOVES_ATHLETIC_KELLYGREEN
        {
          public static LocString NAME = (LocString) "Go Team Sports Gloves";
          public static LocString DESC = (LocString) "Green-striped gloves for the perenially good sport.";
        }

        public class GLOVES_ATHLETIC_COBALT
        {
          public static LocString NAME = (LocString) "True Blue Sports Gloves";
          public static LocString DESC = (LocString) "Blue-striped gloves perfect for shaking hands after the game.";
        }

        public class GLOVES_ATHLETIC_FLAMINGO
        {
          public static LocString NAME = (LocString) "Pep Rally Sports Gloves";
          public static LocString DESC = (LocString) "Pink-striped glove designed to withstand countless high-fives.";
        }

        public class GLOVES_ATHLETIC_CHARCOAL
        {
          public static LocString NAME = (LocString) "Underdog Sports Gloves";
          public static LocString DESC = (LocString) "The muted stripe minimizes distractions so its wearer can focus on trying very, very hard.";
        }

        public class CUFFLESS_BLUEBERRY
        {
          public static LocString NAME = (LocString) "Blueberry Glovelets";
          public static LocString DESC = (LocString) "Wrist coverage is <i>so</i> overrated.";
        }

        public class CUFFLESS_GRAPE
        {
          public static LocString NAME = (LocString) "Grape Glovelets";
          public static LocString DESC = (LocString) "Wrist coverage is <i>so</i> overrated.";
        }

        public class CUFFLESS_LEMON
        {
          public static LocString NAME = (LocString) "Lemon Glovelets";
          public static LocString DESC = (LocString) "Wrist coverage is <i>so</i> overrated.";
        }

        public class CUFFLESS_LIME
        {
          public static LocString NAME = (LocString) "Lime Glovelets";
          public static LocString DESC = (LocString) "Wrist coverage is <i>so</i> overrated.";
        }

        public class CUFFLESS_SATSUMA
        {
          public static LocString NAME = (LocString) "Satsuma Glovelets";
          public static LocString DESC = (LocString) "Wrist coverage is <i>so</i> overrated.";
        }

        public class CUFFLESS_STRAWBERRY
        {
          public static LocString NAME = (LocString) "Strawberry Glovelets";
          public static LocString DESC = (LocString) "Wrist coverage is <i>so</i> overrated.";
        }

        public class CUFFLESS_WATERMELON
        {
          public static LocString NAME = (LocString) "Watermelon Glovelets";
          public static LocString DESC = (LocString) "Wrist coverage is <i>so</i> overrated.";
        }

        public class CIRCUIT_GREEN
        {
          public static LocString NAME = (LocString) "LED Gloves";
          public static LocString DESC = (LocString) "Great for gesticulating at parties.";
        }

        public class ATHLETE
        {
          public static LocString NAME = (LocString) "Racing Gloves";
          public static LocString DESC = (LocString) "Crafted for high-speed handshakes.";
        }

        public class BASIC_BROWN_KHAKI
        {
          public static LocString NAME = (LocString) "Basic Khaki Gloves";
          public static LocString DESC = (LocString) "They don't show dirt.";
        }

        public class BASIC_BLUEGREY
        {
          public static LocString NAME = (LocString) "Basic Gunmetal Gloves";
          public static LocString DESC = (LocString) "A tough name for soft gloves.";
        }

        public class CUFFLESS_BLACK
        {
          public static LocString NAME = (LocString) "Stealth Glovelets";
          public static LocString DESC = (LocString) "It's easy to forget they're even on.";
        }

        public class DENIM_BLUE
        {
          public static LocString NAME = (LocString) "Denim Gloves";
          public static LocString DESC = (LocString) "They're not great for dexterity.";
        }

        public class BASIC_GREY
        {
          public static LocString NAME = (LocString) "Basic Gray Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of gray gloves that go with everything.";
        }

        public class BASIC_PINKSALMON
        {
          public static LocString NAME = (LocString) "Basic Coral Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of bright pink gloves that go with everything.";
        }

        public class BASIC_TAN
        {
          public static LocString NAME = (LocString) "Basic Tan Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of tan gloves that go with everything.";
        }

        public class BALLERINA_PINK
        {
          public static LocString NAME = (LocString) "Ballet Gloves";
          public static LocString DESC = (LocString) "Wrist ruffles highlight the poetic movements of the phalanges.";
        }

        public class FORMAL_WHITE
        {
          public static LocString NAME = (LocString) "White Silk Gloves";
          public static LocString DESC = (LocString) "They're as soft as...well, silk.";
        }

        public class LONG_WHITE
        {
          public static LocString NAME = (LocString) "White Evening Gloves";
          public static LocString DESC = (LocString) "Super-long gloves for super-formal occasions.";
        }

        public class TWOTONE_CREAM_CHARCOAL
        {
          public static LocString NAME = (LocString) "Contrast Cuff Gloves";
          public static LocString DESC = (LocString) "For elegance so understated, it may go completely unnoticed.";
        }

        public class SOCKSUIT_BEIGE
        {
          public static LocString NAME = (LocString) "Vintage Handsock";
          public static LocString DESC = (LocString) "Designed by someone with cold hands and an excess of old socks.";
        }

        public class BASIC_SLATE
        {
          public static LocString NAME = (LocString) "Basic Slate Gloves";
          public static LocString DESC = (LocString) "A good, solid pair of slate gloves that go with everything.";
        }

        public class KNIT_GOLD
        {
          public static LocString NAME = (LocString) "Gold Knit Gloves";
          public static LocString DESC = (LocString) "Produces a pleasantly muffled \"whump\" when high-fiving.";
        }

        public class KNIT_MAGENTA
        {
          public static LocString NAME = (LocString) "Magenta Knit Gloves";
          public static LocString DESC = (LocString) "Produces a pleasantly muffled \"whump\" when high-fiving.";
        }

        public class SPARKLE_WHITE
        {
          public static LocString NAME = (LocString) "White Glitter Gloves";
          public static LocString DESC = (LocString) "Each sequin was attached using sealant borrowed from the rocketry department.";
        }

        public class GINCH_PINK_SALTROCK
        {
          public static LocString NAME = (LocString) "Frilly Saltrock Gloves";
          public static LocString DESC = (LocString) "Thick, soft pink gloves with added flounce.";
        }

        public class GINCH_PURPLE_DUSKY
        {
          public static LocString NAME = (LocString) "Frilly Dusk Gloves";
          public static LocString DESC = (LocString) "Thick, soft purple gloves with added flounce.";
        }

        public class GINCH_BLUE_BASIN
        {
          public static LocString NAME = (LocString) "Frilly Basin Gloves";
          public static LocString DESC = (LocString) "Thick, soft blue gloves with added flounce.";
        }

        public class GINCH_TEAL_BALMY
        {
          public static LocString NAME = (LocString) "Frilly Balm Gloves";
          public static LocString DESC = (LocString) "The soft teal fabric soothes hard-working hands.";
        }

        public class GINCH_GREEN_LIME
        {
          public static LocString NAME = (LocString) "Frilly Leach Gloves";
          public static LocString DESC = (LocString) "Thick, soft green gloves with added flounce.";
        }

        public class GINCH_YELLOW_YELLOWCAKE
        {
          public static LocString NAME = (LocString) "Frilly Yellowcake Gloves";
          public static LocString DESC = (LocString) "Thick, soft yellow gloves with added flounce.";
        }

        public class GINCH_ORANGE_ATOMIC
        {
          public static LocString NAME = (LocString) "Frilly Atomic Gloves";
          public static LocString DESC = (LocString) "Thick, bright orange gloves with added flounce.";
        }

        public class GINCH_RED_MAGMA
        {
          public static LocString NAME = (LocString) "Frilly Magma Gloves";
          public static LocString DESC = (LocString) "Thick, soft red gloves with added flounce.";
        }

        public class GINCH_GREY_GREY
        {
          public static LocString NAME = (LocString) "Frilly Slate Gloves";
          public static LocString DESC = (LocString) "Thick, soft gray gloves with added flounce.";
        }

        public class GINCH_GREY_CHARCOAL
        {
          public static LocString NAME = (LocString) "Frilly Charcoal Gloves";
          public static LocString DESC = (LocString) "Thick, soft dark gray gloves with added flounce.";
        }
      }
    }

    public class CLOTHING_TOPS
    {
      public static LocString NAME = (LocString) "Default Top";
      public static LocString DESC = (LocString) "The default shirt.";

      public class FACADES
      {
        public class STANDARD_YELLOW_TOP
        {
          public static LocString NAME = (LocString) "Yellow Uniform Shirt";
          public static LocString DESC = (LocString) "A standard-issue uniform shirt in flax yellow.";
        }

        public class STANDARD_GREEN_TOP
        {
          public static LocString NAME = (LocString) "Green Uniform Shirt";
          public static LocString DESC = (LocString) "A standard-issue uniform shirt in swampy green.";
        }

        public class STANDARD_RED_TOP
        {
          public static LocString NAME = (LocString) "Red Uniform Shirt";
          public static LocString DESC = (LocString) "A standard-issue uniform shirt in carmine red.";
        }

        public class STANDARD_BLUE_TOP
        {
          public static LocString NAME = (LocString) "Blue Uniform Shirt";
          public static LocString DESC = (LocString) "A standard-issue uniform shirt in a standard-issue blue hue.";
        }

        public class BASIC_BLUE_MIDDLE
        {
          public static LocString NAME = (LocString) "Basic Aqua Shirt";
          public static LocString DESC = (LocString) "A nice aqua-blue shirt that goes with everything.";
        }

        public class BASIC_BLACK
        {
          public static LocString NAME = (LocString) "Basic Black Shirt";
          public static LocString DESC = (LocString) "A nice black shirt that goes with everything.";
        }

        public class BASIC_PINK_ORCHID
        {
          public static LocString NAME = (LocString) "Basic Bubblegum Shirt";
          public static LocString DESC = (LocString) "A nice bubblegum-pink shirt that goes with everything.";
        }

        public class BASIC_GREEN
        {
          public static LocString NAME = (LocString) "Basic Green Shirt";
          public static LocString DESC = (LocString) "A nice green shirt that goes with everything.";
        }

        public class BASIC_ORANGE
        {
          public static LocString NAME = (LocString) "Basic Orange Shirt";
          public static LocString DESC = (LocString) "A nice orange shirt that goes with everything.";
        }

        public class BASIC_PURPLE
        {
          public static LocString NAME = (LocString) "Basic Purple Shirt";
          public static LocString DESC = (LocString) "A nice purple shirt that goes with everything.";
        }

        public class BASIC_RED_BURNT
        {
          public static LocString NAME = (LocString) "Basic Red Shirt";
          public static LocString DESC = (LocString) "A nice red shirt that goes with everything.";
        }

        public class BASIC_WHITE
        {
          public static LocString NAME = (LocString) "Basic White Shirt";
          public static LocString DESC = (LocString) "A nice white shirt that goes with everything.";
        }

        public class BASIC_YELLOW
        {
          public static LocString NAME = (LocString) "Basic Yellow Shirt";
          public static LocString DESC = (LocString) "A nice yellow shirt that goes with everything.";
        }

        public class RAGLANTOP_DEEPRED
        {
          public static LocString NAME = (LocString) "Team Captain T-shirt";
          public static LocString DESC = (LocString) "A slightly sweat-stained tee for natural leaders.";
        }

        public class RAGLANTOP_COBALT
        {
          public static LocString NAME = (LocString) "True Blue T-shirt";
          public static LocString DESC = (LocString) "A slightly sweat-stained tee for the real team players.";
        }

        public class RAGLANTOP_FLAMINGO
        {
          public static LocString NAME = (LocString) "Pep Rally T-shirt";
          public static LocString DESC = (LocString) "A slightly sweat-stained tee to boost team spirits.";
        }

        public class RAGLANTOP_KELLYGREEN
        {
          public static LocString NAME = (LocString) "Go Team T-shirt";
          public static LocString DESC = (LocString) "A slightly sweat-stained tee for cheering from the sidelines.";
        }

        public class RAGLANTOP_CHARCOAL
        {
          public static LocString NAME = (LocString) "Underdog T-shirt";
          public static LocString DESC = (LocString) "For those who don't win a lot.";
        }

        public class RAGLANTOP_LEMON
        {
          public static LocString NAME = (LocString) "Hype T-shirt";
          public static LocString DESC = (LocString) "A slightly sweat-stained tee to wear when talking a big game.";
        }

        public class RAGLANTOP_SATSUMA
        {
          public static LocString NAME = (LocString) "Superfan T-shirt";
          public static LocString DESC = (LocString) "A slightly sweat-stained tee for the long-time supporter.";
        }

        public class JELLYPUFFJACKET_BLUEBERRY
        {
          public static LocString NAME = (LocString) "Blueberry Jelly Jacket";
          public static LocString DESC = (LocString) "It's best to keep jelly-filled puffer jackets away from sharp corners.";
        }

        public class JELLYPUFFJACKET_GRAPE
        {
          public static LocString NAME = (LocString) "Grape Jelly Jacket";
          public static LocString DESC = (LocString) "It's best to keep jelly-filled puffer jackets away from sharp corners.";
        }

        public class JELLYPUFFJACKET_LEMON
        {
          public static LocString NAME = (LocString) "Lemon Jelly Jacket";
          public static LocString DESC = (LocString) "It's best to keep jelly-filled puffer jackets away from sharp corners.";
        }

        public class JELLYPUFFJACKET_LIME
        {
          public static LocString NAME = (LocString) "Lime Jelly Jacket";
          public static LocString DESC = (LocString) "It's best to keep jelly-filled puffer jackets away from sharp corners.";
        }

        public class JELLYPUFFJACKET_SATSUMA
        {
          public static LocString NAME = (LocString) "Satsuma Jelly Jacket";
          public static LocString DESC = (LocString) "It's best to keep jelly-filled puffer jackets away from sharp corners.";
        }

        public class JELLYPUFFJACKET_STRAWBERRY
        {
          public static LocString NAME = (LocString) "Strawberry Jelly Jacket";
          public static LocString DESC = (LocString) "It's best to keep jelly-filled puffer jackets away from sharp corners.";
        }

        public class JELLYPUFFJACKET_WATERMELON
        {
          public static LocString NAME = (LocString) "Watermelon Jelly Jacket";
          public static LocString DESC = (LocString) "It's best to keep jelly-filled puffer jackets away from sharp corners.";
        }

        public class CIRCUIT_GREEN
        {
          public static LocString NAME = (LocString) "LED Jacket";
          public static LocString DESC = (LocString) "For dancing in the dark.";
        }

        public class TSHIRT_WHITE
        {
          public static LocString NAME = (LocString) "Classic White Tee";
          public static LocString DESC = (LocString) "It's practically begging for a big Bog Jelly stain down the front.";
        }

        public class TSHIRT_MAGENTA
        {
          public static LocString NAME = (LocString) "Classic Magenta Tee";
          public static LocString DESC = (LocString) "It will never chafe against delicate inner-elbow skin.";
        }

        public class ATHLETE
        {
          public static LocString NAME = (LocString) "Racing Jacket";
          public static LocString DESC = (LocString) "The epitome of fast fashion.";
        }

        public class DENIM_BLUE
        {
          public static LocString NAME = (LocString) "Denim Jacket";
          public static LocString DESC = (LocString) "The top half of a Canadian tuxedo.";
        }

        public class GONCH_STRAWBERRY
        {
          public static LocString NAME = (LocString) "Executive Undershirt";
          public static LocString DESC = (LocString) "The breathable base layer every power suit needs.";
        }

        public class GONCH_SATSUMA
        {
          public static LocString NAME = (LocString) "Underling Undershirt";
          public static LocString DESC = (LocString) "Extra-absorbent fabric in the underarms to mop up nervous sweat.";
        }

        public class GONCH_LEMON
        {
          public static LocString NAME = (LocString) "Groupthink Undershirt";
          public static LocString DESC = (LocString) "Because the most popular choice is always the right choice.";
        }

        public class GONCH_LIME
        {
          public static LocString NAME = (LocString) "Stakeholder Undershirt";
          public static LocString DESC = (LocString) "Soft against the skin, for those who have skin in the game.";
        }

        public class GONCH_BLUEBERRY
        {
          public static LocString NAME = (LocString) "Admin Undershirt";
          public static LocString DESC = (LocString) "Criminally underappreciated.";
        }

        public class GONCH_GRAPE
        {
          public static LocString NAME = (LocString) "Buzzword Undershirt";
          public static LocString DESC = (LocString) "A value-added vest for touching base and thinking outside the box using best practices ASAP.";
        }

        public class GONCH_WATERMELON
        {
          public static LocString NAME = (LocString) "Synergy Undershirt";
          public static LocString DESC = (LocString) "Asking for it by name often triggers dramatic eye-rolls from bystanders.";
        }

        public class NERD_BROWN
        {
          public static LocString NAME = (LocString) "Research Shirt";
          public static LocString DESC = (LocString) "Comes with a thoughtfully chewed-up ballpoint pen.";
        }

        public class GI_WHITE
        {
          public static LocString NAME = (LocString) "Rebel Gi Jacket";
          public static LocString DESC = (LocString) "The contrasting trim hides stains from messy post-sparring snacks.";
        }

        public class JACKET_SMOKING_BURGUNDY
        {
          public static LocString NAME = (LocString) "Donor Jacket";
          public static LocString DESC = (LocString) "Crafted from the softest, most philanthropic fibers.";
        }

        public class MECHANIC
        {
          public static LocString NAME = (LocString) "Engineer Jacket";
          public static LocString DESC = (LocString) "Designed to withstand the rigors of applied science.";
        }

        public class VELOUR_BLACK
        {
          public static LocString NAME = (LocString) "PhD Velour Jacket";
          public static LocString DESC = (LocString) "A formal jacket for those who are \"not that kind of doctor.\"";
        }

        public class VELOUR_BLUE
        {
          public static LocString NAME = (LocString) "Shortwave Velour Jacket";
          public static LocString DESC = (LocString) "A luxe, pettable jacket paired with a clip-on tie.";
        }

        public class VELOUR_PINK
        {
          public static LocString NAME = (LocString) "Gamma Velour Jacket";
          public static LocString DESC = (LocString) "Some scientists are less shy than others.";
        }

        public class WAISTCOAT_PINSTRIPE_SLATE
        {
          public static LocString NAME = (LocString) "Nobel Pinstripe Waistcoat";
          public static LocString DESC = (LocString) "One must dress for the prize that one wishes to win.";
        }

        public class WATER
        {
          public static LocString NAME = (LocString) "HVAC Khaki Shirt";
          public static LocString DESC = (LocString) "Designed to regulate temperature and humidity.";
        }

        public class TWEED_PINK_ORCHID
        {
          public static LocString NAME = (LocString) "Power Brunch Blazer";
          public static LocString DESC = (LocString) "Winners never quit, quitters never win.";
        }

        public class DRESS_SLEEVELESS_BOW_BW
        {
          public static LocString NAME = (LocString) "PhD Dress";
          public static LocString DESC = (LocString) "Ready for a post-thesis-defense party.";
        }

        public class BODYSUIT_BALLERINA_PINK
        {
          public static LocString NAME = (LocString) "Ballet Leotard";
          public static LocString DESC = (LocString) "Lab-crafted fabric with a level of stretchiness that defies the laws of physics.";
        }

        public class SOCKSUIT_BEIGE
        {
          public static LocString NAME = (LocString) "Vintage Sockshirt";
          public static LocString DESC = (LocString) "Like a sock for the torso. With sleeves.";
        }

        public class X_SPORCHID
        {
          public static LocString NAME = (LocString) "Sporefest Sweater";
          public static LocString DESC = (LocString) "This soft knit can be worn anytime, not just during Zombie Spore season.";
        }

        public class X1_PINCHAPEPPERNUTBELLS
        {
          public static LocString NAME = (LocString) "Pinchabell Jacket";
          public static LocString DESC = (LocString) "The peppernuts jingle just loudly enough to be distracting.";
        }

        public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
        {
          public static LocString NAME = (LocString) "Pom Bug Sweater";
          public static LocString DESC = (LocString) "No Shine Bugs were harmed in the making of this sweater.";
        }

        public class SNOWFLAKE_BLUE
        {
          public static LocString NAME = (LocString) "Crystal-Iced Sweater";
          public static LocString DESC = (LocString) "Tiny imperfections in the front pattern ensure that no two are truly identical.";
        }

        public class PJ_CLOVERS_GLITCH_KELLY
        {
          public static LocString NAME = (LocString) "Lucky Jammies";
          public static LocString DESC = (LocString) "Even the most brilliant minds need a little extra luck sometimes.";
        }

        public class PJ_HEARTS_CHILLI_STRAWBERRY
        {
          public static LocString NAME = (LocString) "Sweetheart Jammies";
          public static LocString DESC = (LocString) "Plush chenille fabric and a drool-absorbent collar? This sleepsuit really <i>is</i> \"The One.\"";
        }

        public class BUILDER
        {
          public static LocString NAME = (LocString) "Hi-Vis Jacket";
          public static LocString DESC = (LocString) "Unmissable style for the safety-minded.";
        }

        public class FLORAL_PINK
        {
          public static LocString NAME = (LocString) "Downtime Shirt";
          public static LocString DESC = (LocString) "For maxing and relaxing when errands are too taxing.";
        }

        public class GINCH_PINK_SALTROCK
        {
          public static LocString NAME = (LocString) "Frilly Saltrock Undershirt";
          public static LocString DESC = (LocString) "A seamless pink undershirt with laser-cut ruffles.";
        }

        public class GINCH_PURPLE_DUSKY
        {
          public static LocString NAME = (LocString) "Frilly Dusk Undershirt";
          public static LocString DESC = (LocString) "A seamless purple undershirt with laser-cut ruffles.";
        }

        public class GINCH_BLUE_BASIN
        {
          public static LocString NAME = (LocString) "Frilly Basin Undershirt";
          public static LocString DESC = (LocString) "A seamless blue undershirt with laser-cut ruffles.";
        }

        public class GINCH_TEAL_BALMY
        {
          public static LocString NAME = (LocString) "Frilly Balm Undershirt";
          public static LocString DESC = (LocString) "A seamless teal undershirt with laser-cut ruffles.";
        }

        public class GINCH_GREEN_LIME
        {
          public static LocString NAME = (LocString) "Frilly Leach Undershirt";
          public static LocString DESC = (LocString) "A seamless green undershirt with laser-cut ruffles.";
        }

        public class GINCH_YELLOW_YELLOWCAKE
        {
          public static LocString NAME = (LocString) "Frilly Yellowcake Undershirt";
          public static LocString DESC = (LocString) "A seamless yellow undershirt with laser-cut ruffles.";
        }

        public class GINCH_ORANGE_ATOMIC
        {
          public static LocString NAME = (LocString) "Frilly Atomic Undershirt";
          public static LocString DESC = (LocString) "A seamless orange undershirt with laser-cut ruffles.";
        }

        public class GINCH_RED_MAGMA
        {
          public static LocString NAME = (LocString) "Frilly Magma Undershirt";
          public static LocString DESC = (LocString) "A seamless red undershirt with laser-cut ruffles.";
        }

        public class GINCH_GREY_GREY
        {
          public static LocString NAME = (LocString) "Frilly Slate Undershirt";
          public static LocString DESC = (LocString) "A seamless grey undershirt with laser-cut ruffles.";
        }

        public class GINCH_GREY_CHARCOAL
        {
          public static LocString NAME = (LocString) "Frilly Charcoal Undershirt";
          public static LocString DESC = (LocString) "A seamless dark gray undershirt with laser-cut ruffles.";
        }

        public class KNIT_POLKADOT_TURQ
        {
          public static LocString NAME = (LocString) "Polka Dot Track Jacket";
          public static LocString DESC = (LocString) "The dots are infused with odor-neutralizing enzymes!";
        }

        public class FLASHY
        {
          public static LocString NAME = (LocString) "Superstar Jacket";
          public static LocString DESC = (LocString) "Some of us were not made to be subtle.";
        }
      }
    }

    public class CLOTHING_BOTTOMS
    {
      public static LocString NAME = (LocString) "Default Bottom";
      public static LocString DESC = (LocString) "The default bottoms.";

      public class FACADES
      {
        public class BASIC_BLUE_MIDDLE
        {
          public static LocString NAME = (LocString) "Basic Aqua Pants";
          public static LocString DESC = (LocString) "A clean pair of aqua-blue pants that go with everything.";
        }

        public class BASIC_PINK_ORCHID
        {
          public static LocString NAME = (LocString) "Basic Bubblegum Pants";
          public static LocString DESC = (LocString) "A clean pair of bubblegum-pink pants that go with everything.";
        }

        public class BASIC_GREEN
        {
          public static LocString NAME = (LocString) "Basic Green Pants";
          public static LocString DESC = (LocString) "A clean pair of green pants that go with everything.";
        }

        public class BASIC_ORANGE
        {
          public static LocString NAME = (LocString) "Basic Orange Pants";
          public static LocString DESC = (LocString) "A clean pair of orange pants that go with everything.";
        }

        public class BASIC_PURPLE
        {
          public static LocString NAME = (LocString) "Basic Purple Pants";
          public static LocString DESC = (LocString) "A clean pair of purple pants that go with everything.";
        }

        public class BASIC_RED
        {
          public static LocString NAME = (LocString) "Basic Red Pants";
          public static LocString DESC = (LocString) "A clean pair of red pants that go with everything.";
        }

        public class BASIC_WHITE
        {
          public static LocString NAME = (LocString) "Basic White Pants";
          public static LocString DESC = (LocString) "A clean pair of white pants that go with everything.";
        }

        public class BASIC_YELLOW
        {
          public static LocString NAME = (LocString) "Basic Yellow Pants";
          public static LocString DESC = (LocString) "A clean pair of yellow pants that go with everything.";
        }

        public class BASIC_BLACK
        {
          public static LocString NAME = (LocString) "Basic Black Pants";
          public static LocString DESC = (LocString) "A clean pair of black pants that go with everything.";
        }

        public class SHORTS_BASIC_DEEPRED
        {
          public static LocString NAME = (LocString) "Team Captain Shorts";
          public static LocString DESC = (LocString) "A fresh pair of shorts for natural leaders.";
        }

        public class SHORTS_BASIC_SATSUMA
        {
          public static LocString NAME = (LocString) "Superfan Shorts";
          public static LocString DESC = (LocString) "A fresh pair of shorts for long-time supporters of...shorts.";
        }

        public class SHORTS_BASIC_YELLOWCAKE
        {
          public static LocString NAME = (LocString) "Yellowcake Shorts";
          public static LocString DESC = (LocString) "A fresh pair of uranium-powder-colored shorts that are definitely not radioactive. Probably.";
        }

        public class SHORTS_BASIC_KELLYGREEN
        {
          public static LocString NAME = (LocString) "Go Team Shorts";
          public static LocString DESC = (LocString) "A fresh pair of shorts for cheering from the sidelines.";
        }

        public class SHORTS_BASIC_BLUE_COBALT
        {
          public static LocString NAME = (LocString) "True Blue Shorts";
          public static LocString DESC = (LocString) "A fresh pair of shorts for the real team players.";
        }

        public class SHORTS_BASIC_PINK_FLAMINGO
        {
          public static LocString NAME = (LocString) "Pep Rally Shorts";
          public static LocString DESC = (LocString) "The peppiest pair of shorts this side of the asteroid.";
        }

        public class SHORTS_BASIC_CHARCOAL
        {
          public static LocString NAME = (LocString) "Underdog Shorts";
          public static LocString DESC = (LocString) "A fresh pair of shorts. They're cleaner than they look.";
        }

        public class CIRCUIT_GREEN
        {
          public static LocString NAME = (LocString) "LED Pants";
          public static LocString DESC = (LocString) "These legs are lit.";
        }

        public class ATHLETE
        {
          public static LocString NAME = (LocString) "Racing Pants";
          public static LocString DESC = (LocString) "Fast, furious fashion.";
        }

        public class BASIC_LIGHTBROWN
        {
          public static LocString NAME = (LocString) "Basic Khaki Pants";
          public static LocString DESC = (LocString) "Transition effortlessly from subterranean day to subterranean night.";
        }

        public class BASIC_REDORANGE
        {
          public static LocString NAME = (LocString) "Basic Crimson Pants";
          public static LocString DESC = (LocString) "Like red pants, but slightly fancier-sounding.";
        }

        public class GONCH_STRAWBERRY
        {
          public static LocString NAME = (LocString) "Executive Briefs";
          public static LocString DESC = (LocString) "Bossy (under)pants.";
        }

        public class GONCH_SATSUMA
        {
          public static LocString NAME = (LocString) "Underling Briefs";
          public static LocString DESC = (LocString) "The seams are already unraveling.";
        }

        public class GONCH_LEMON
        {
          public static LocString NAME = (LocString) "Groupthink Briefs";
          public static LocString DESC = (LocString) "All the cool people are wearing them.";
        }

        public class GONCH_LIME
        {
          public static LocString NAME = (LocString) "Stakeholder Briefs";
          public static LocString DESC = (LocString) "They're really invested in keeping the wearer comfortable.";
        }

        public class GONCH_BLUEBERRY
        {
          public static LocString NAME = (LocString) "Admin Briefs";
          public static LocString DESC = (LocString) "The workhorse of the underwear world.";
        }

        public class GONCH_GRAPE
        {
          public static LocString NAME = (LocString) "Buzzword Briefs";
          public static LocString DESC = (LocString) "Underwear that works hard, plays hard, and gives 110% to maximize the \"bottom\" line.";
        }

        public class GONCH_WATERMELON
        {
          public static LocString NAME = (LocString) "Synergy Briefs";
          public static LocString DESC = (LocString) "Teamwork makes the dream work.";
        }

        public class DENIM_BLUE
        {
          public static LocString NAME = (LocString) "Jeans";
          public static LocString DESC = (LocString) "The bottom half of a Canadian tuxedo.";
        }

        public class GI_WHITE
        {
          public static LocString NAME = (LocString) "White Capris";
          public static LocString DESC = (LocString) "The cropped length is ideal for wading through flooded hallways.";
        }

        public class NERD_BROWN
        {
          public static LocString NAME = (LocString) "Research Pants";
          public static LocString DESC = (LocString) "The pockets are full of illegible notes that didn't quite survive the wash.";
        }

        public class SKIRT_BASIC_BLUE_MIDDLE
        {
          public static LocString NAME = (LocString) "Aqua Rayon Skirt";
          public static LocString DESC = (LocString) "The tag says \"Dry Clean Only.\" There are no dry cleaners in space.";
        }

        public class SKIRT_BASIC_PURPLE
        {
          public static LocString NAME = (LocString) "Purple Rayon Skirt";
          public static LocString DESC = (LocString) "It's not the most breathable fabric, but it <i>is</i> a lovely shade of purple.";
        }

        public class SKIRT_BASIC_GREEN
        {
          public static LocString NAME = (LocString) "Olive Rayon Skirt";
          public static LocString DESC = (LocString) "Designed not to get snagged on ladders.";
        }

        public class SKIRT_BASIC_ORANGE
        {
          public static LocString NAME = (LocString) "Apricot Rayon Skirt";
          public static LocString DESC = (LocString) "Ready for spontaneous workplace twirling.";
        }

        public class SKIRT_BASIC_PINK_ORCHID
        {
          public static LocString NAME = (LocString) "Bubblegum Rayon Skirt";
          public static LocString DESC = (LocString) "The bubblegum scent lasts 100 washes!";
        }

        public class SKIRT_BASIC_RED
        {
          public static LocString NAME = (LocString) "Garnet Rayon Skirt";
          public static LocString DESC = (LocString) "It's business time.";
        }

        public class SKIRT_BASIC_YELLOW
        {
          public static LocString NAME = (LocString) "Yellow Rayon Skirt";
          public static LocString DESC = (LocString) "A formerly white skirt that has not aged well.";
        }

        public class SKIRT_BASIC_POLKADOT
        {
          public static LocString NAME = (LocString) "Polka Dot Skirt";
          public static LocString DESC = (LocString) "Polka dots are a way to infinity.";
        }

        public class SKIRT_BASIC_WATERMELON
        {
          public static LocString NAME = (LocString) "Picnic Skirt";
          public static LocString DESC = (LocString) "The seeds are spittable, but will bear no fruit.";
        }

        public class SKIRT_DENIM_BLUE
        {
          public static LocString NAME = (LocString) "Denim Tux Skirt";
          public static LocString DESC = (LocString) "Designed for the casual red carpet.";
        }

        public class SKIRT_LEOPARD_PRINT_BLUE_PINK
        {
          public static LocString NAME = (LocString) "Disco Leopard Skirt";
          public static LocString DESC = (LocString) "A faux-fur party staple.";
        }

        public class SKIRT_SPARKLE_BLUE
        {
          public static LocString NAME = (LocString) "Blue Tinsel Skirt";
          public static LocString DESC = (LocString) "The tinsel is scratchy, but look how shiny!";
        }

        public class BASIC_ORANGE_SATSUMA
        {
          public static LocString NAME = (LocString) "Hi-Vis Pants";
          public static LocString DESC = (LocString) "They make the wearer feel truly seen.";
        }

        public class PINSTRIPE_SLATE
        {
          public static LocString NAME = (LocString) "Nobel Pinstripe Trousers";
          public static LocString DESC = (LocString) "There's a waterproof pocket to keep acceptance speeches smudge-free.";
        }

        public class VELOUR_BLACK
        {
          public static LocString NAME = (LocString) "Black Velour Trousers";
          public static LocString DESC = (LocString) "Fuzzy, formal and finely cut.";
        }

        public class VELOUR_BLUE
        {
          public static LocString NAME = (LocString) "Shortwave Velour Pants";
          public static LocString DESC = (LocString) "Formal wear with a sensory side.";
        }

        public class VELOUR_PINK
        {
          public static LocString NAME = (LocString) "Gamma Velour Pants";
          public static LocString DESC = (LocString) "They're stretchy <i>and</i> flame retardant.";
        }

        public class SKIRT_BALLERINA_PINK
        {
          public static LocString NAME = (LocString) "Ballet Tutu";
          public static LocString DESC = (LocString) "A tulle skirt spun and assembled by an army of patent-pending nanobots.";
        }

        public class SKIRT_TWEED_PINK_ORCHID
        {
          public static LocString NAME = (LocString) "Power Brunch Skirt";
          public static LocString DESC = (LocString) "It has pockets!";
        }

        public class GINCH_PINK_GLUON
        {
          public static LocString NAME = (LocString) "Gluon Shorties";
          public static LocString DESC = (LocString) "Comfy pink short-shorts with a ruffled hem.";
        }

        public class GINCH_PURPLE_CORTEX
        {
          public static LocString NAME = (LocString) "Cortex Shorties";
          public static LocString DESC = (LocString) "Comfy purple short-shorts with a ruffled hem.";
        }

        public class GINCH_BLUE_FROSTY
        {
          public static LocString NAME = (LocString) "Frosty Shorties";
          public static LocString DESC = (LocString) "Icy blue short-shorts with a ruffled hem.";
        }

        public class GINCH_TEAL_LOCUS
        {
          public static LocString NAME = (LocString) "Locus Shorties";
          public static LocString DESC = (LocString) "Comfy teal short-shorts with a ruffled hem.";
        }

        public class GINCH_GREEN_GOOP
        {
          public static LocString NAME = (LocString) "Goop Shorties";
          public static LocString DESC = (LocString) "Short-shorts with a ruffled hem and one pocket full of melted snacks.";
        }

        public class GINCH_YELLOW_BILE
        {
          public static LocString NAME = (LocString) "Bile Shorties";
          public static LocString DESC = (LocString) "Ruffled short-shorts in a stomach-turning shade of yellow.";
        }

        public class GINCH_ORANGE_NYBBLE
        {
          public static LocString NAME = (LocString) "Nybble Shorties";
          public static LocString DESC = (LocString) "Comfy orange ruffled short-shorts for computer scientists.";
        }

        public class GINCH_RED_IRONBOW
        {
          public static LocString NAME = (LocString) "Ironbow Shorties";
          public static LocString DESC = (LocString) "Comfy red short-shorts with a ruffled hem.";
        }

        public class GINCH_GREY_PHLEGM
        {
          public static LocString NAME = (LocString) "Phlegmy Shorties";
          public static LocString DESC = (LocString) "Ruffled short-shorts in a rather sticky shade of light gray.";
        }

        public class GINCH_GREY_OBELUS
        {
          public static LocString NAME = (LocString) "Obelus Shorties";
          public static LocString DESC = (LocString) "Comfy gray short-shorts with a ruffled hem.";
        }

        public class KNIT_POLKADOT_TURQ
        {
          public static LocString NAME = (LocString) "Polka Dot Track Pants";
          public static LocString DESC = (LocString) "For clowning around during mandatory physical fitness week.";
        }

        public class GI_BELT_WHITE_BLACK
        {
          public static LocString NAME = (LocString) "Rebel Gi Pants";
          public static LocString DESC = (LocString) "Relaxed-fit pants designed for roundhouse kicks.";
        }

        public class BELT_KHAKI_TAN
        {
          public static LocString NAME = (LocString) "HVAC Khaki Pants";
          public static LocString DESC = (LocString) "Rip-resistant fabric makes crawling through ducts a breeze.";
        }
      }
    }

    public class CLOTHING_SHOES
    {
      public static LocString NAME = (LocString) "Default Footwear";
      public static LocString DESC = (LocString) "The default style of footwear.";

      public class FACADES
      {
        public class BASIC_BLUE_MIDDLE
        {
          public static LocString NAME = (LocString) "Basic Aqua Shoes";
          public static LocString DESC = (LocString) "A fresh pair of aqua-blue shoes that go with everything.";
        }

        public class BASIC_PINK_ORCHID
        {
          public static LocString NAME = (LocString) "Basic Bubblegum Shoes";
          public static LocString DESC = (LocString) "A fresh pair of bubblegum-pink shoes that go with everything.";
        }

        public class BASIC_GREEN
        {
          public static LocString NAME = (LocString) "Basic Green Shoes";
          public static LocString DESC = (LocString) "A fresh pair of green shoes that go with everything.";
        }

        public class BASIC_ORANGE
        {
          public static LocString NAME = (LocString) "Basic Orange Shoes";
          public static LocString DESC = (LocString) "A fresh pair of orange shoes that go with everything.";
        }

        public class BASIC_PURPLE
        {
          public static LocString NAME = (LocString) "Basic Purple Shoes";
          public static LocString DESC = (LocString) "A fresh pair of purple shoes that go with everything.";
        }

        public class BASIC_RED
        {
          public static LocString NAME = (LocString) "Basic Red Shoes";
          public static LocString DESC = (LocString) "A fresh pair of red shoes that go with everything.";
        }

        public class BASIC_WHITE
        {
          public static LocString NAME = (LocString) "Basic White Shoes";
          public static LocString DESC = (LocString) "A fresh pair of white shoes that go with everything.";
        }

        public class BASIC_YELLOW
        {
          public static LocString NAME = (LocString) "Basic Yellow Shoes";
          public static LocString DESC = (LocString) "A fresh pair of yellow shoes that go with everything.";
        }

        public class BASIC_BLACK
        {
          public static LocString NAME = (LocString) "Basic Black Shoes";
          public static LocString DESC = (LocString) "A fresh pair of black shoes that go with everything.";
        }

        public class BASIC_BLUEGREY
        {
          public static LocString NAME = (LocString) "Basic Gunmetal Shoes";
          public static LocString DESC = (LocString) "A fresh pair of pastel shoes that go with everything.";
        }

        public class BASIC_TAN
        {
          public static LocString NAME = (LocString) "Basic Tan Shoes";
          public static LocString DESC = (LocString) "They're remarkably unremarkable.";
        }

        public class SOCKS_ATHLETIC_DEEPRED
        {
          public static LocString NAME = (LocString) "Team Captain Gym Socks";
          public static LocString DESC = (LocString) "Breathable socks with sporty red stripes.";
        }

        public class SOCKS_ATHLETIC_SATSUMA
        {
          public static LocString NAME = (LocString) "Superfan Gym Socks";
          public static LocString DESC = (LocString) "Breathable socks with sporty orange stripes.";
        }

        public class SOCKS_ATHLETIC_LEMON
        {
          public static LocString NAME = (LocString) "Hype Gym Socks";
          public static LocString DESC = (LocString) "Breathable socks with sporty yellow stripes.";
        }

        public class SOCKS_ATHLETIC_KELLYGREEN
        {
          public static LocString NAME = (LocString) "Go Team Gym Socks";
          public static LocString DESC = (LocString) "Breathable socks with sporty green stripes.";
        }

        public class SOCKS_ATHLETIC_COBALT
        {
          public static LocString NAME = (LocString) "True Blue Gym Socks";
          public static LocString DESC = (LocString) "Breathable socks with sporty blue stripes.";
        }

        public class SOCKS_ATHLETIC_FLAMINGO
        {
          public static LocString NAME = (LocString) "Pep Rally Gym Socks";
          public static LocString DESC = (LocString) "Breathable socks with sporty pink stripes.";
        }

        public class SOCKS_ATHLETIC_CHARCOAL
        {
          public static LocString NAME = (LocString) "Underdog Gym Socks";
          public static LocString DESC = (LocString) "Breathable socks that do nothing whatsoever to eliminate foot odor.";
        }

        public class BASIC_GREY
        {
          public static LocString NAME = (LocString) "Basic Gray Shoes";
          public static LocString DESC = (LocString) "A fresh pair of gray shoes that go with everything.";
        }

        public class DENIM_BLUE
        {
          public static LocString NAME = (LocString) "Denim Shoes";
          public static LocString DESC = (LocString) "Not technically essential for a Canadian tuxedo, but why not?";
        }

        public class LEGWARMERS_STRAWBERRY
        {
          public static LocString NAME = (LocString) "Slouchy Strawberry Socks";
          public static LocString DESC = (LocString) "Freckly knitted socks that don't stay up.";
        }

        public class LEGWARMERS_SATSUMA
        {
          public static LocString NAME = (LocString) "Slouchy Satsuma Socks";
          public static LocString DESC = (LocString) "Sweet knitted socks for spontaneous dance segments.";
        }

        public class LEGWARMERS_LEMON
        {
          public static LocString NAME = (LocString) "Slouchy Lemon Socks";
          public static LocString DESC = (LocString) "Zesty knitted socks that don't stay up.";
        }

        public class LEGWARMERS_LIME
        {
          public static LocString NAME = (LocString) "Slouchy Lime Socks";
          public static LocString DESC = (LocString) "Juicy knitted socks that don't stay up.";
        }

        public class LEGWARMERS_BLUEBERRY
        {
          public static LocString NAME = (LocString) "Slouchy Blueberry Socks";
          public static LocString DESC = (LocString) "Knitted socks with a fun bobble-stitch texture.";
        }

        public class LEGWARMERS_GRAPE
        {
          public static LocString NAME = (LocString) "Slouchy Grape Socks";
          public static LocString DESC = (LocString) "These fabulous knitted socks that don't stay up are really raisin the bar.";
        }

        public class LEGWARMERS_WATERMELON
        {
          public static LocString NAME = (LocString) "Slouchy Watermelon Socks";
          public static LocString DESC = (LocString) "Summery knitted socks that don't stay up.";
        }

        public class BALLERINA_PINK
        {
          public static LocString NAME = (LocString) "Ballet Shoes";
          public static LocString DESC = (LocString) "There's no \"pointe\" in aiming for anything less than perfection.";
        }

        public class MARYJANE_SOCKS_BW
        {
          public static LocString NAME = (LocString) "Frilly Sock Shoes";
          public static LocString DESC = (LocString) "They add a little <i>je ne sais quoi</i> to everyday lab wear.";
        }

        public class CLASSICFLATS_CREAM_CHARCOAL
        {
          public static LocString NAME = (LocString) "Dressy Shoes";
          public static LocString DESC = (LocString) "An enduring style, for enduring endless small talk.";
        }

        public class VELOUR_BLUE
        {
          public static LocString NAME = (LocString) "Shortwave Velour Shoes";
          public static LocString DESC = (LocString) "Not the easiest to keep clean.";
        }

        public class VELOUR_PINK
        {
          public static LocString NAME = (LocString) "Gamma Velour Shoes";
          public static LocString DESC = (LocString) "Finally, a pair of work-appropriate fuzzy shoes.";
        }

        public class VELOUR_BLACK
        {
          public static LocString NAME = (LocString) "Black Velour Shoes";
          public static LocString DESC = (LocString) "Matching velour lining gently tickles feet with every step.";
        }

        public class FLASHY
        {
          public static LocString NAME = (LocString) "Superstar Shoes";
          public static LocString DESC = (LocString) "Why walk when you can <i>moon</i>walk?";
        }

        public class GINCH_PINK_SALTROCK
        {
          public static LocString NAME = (LocString) "Frilly Saltrock Socks";
          public static LocString DESC = (LocString) "Thick, soft pink socks with extra flounce.";
        }

        public class GINCH_PURPLE_DUSKY
        {
          public static LocString NAME = (LocString) "Frilly Dusk Socks";
          public static LocString DESC = (LocString) "Thick, soft purple socks with extra flounce.";
        }

        public class GINCH_BLUE_BASIN
        {
          public static LocString NAME = (LocString) "Frilly Basin Socks";
          public static LocString DESC = (LocString) "Thick, soft blue socks with extra flounce.";
        }

        public class GINCH_TEAL_BALMY
        {
          public static LocString NAME = (LocString) "Frilly Balm Socks";
          public static LocString DESC = (LocString) "Thick, soothing teal socks with extra flounce.";
        }

        public class GINCH_GREEN_LIME
        {
          public static LocString NAME = (LocString) "Frilly Leach Socks";
          public static LocString DESC = (LocString) "Thick, soft green socks with extra flounce.";
        }

        public class GINCH_YELLOW_YELLOWCAKE
        {
          public static LocString NAME = (LocString) "Frilly Yellowcake Socks";
          public static LocString DESC = (LocString) "Dangerously soft yellow socks with extra flounce.";
        }

        public class GINCH_ORANGE_ATOMIC
        {
          public static LocString NAME = (LocString) "Frilly Atomic Socks";
          public static LocString DESC = (LocString) "Thick, soft orange socks with extra flounce.";
        }

        public class GINCH_RED_MAGMA
        {
          public static LocString NAME = (LocString) "Frilly Magma Socks";
          public static LocString DESC = (LocString) "Thick, toasty red socks with extra flounce.";
        }

        public class GINCH_GREY_GREY
        {
          public static LocString NAME = (LocString) "Frilly Slate Socks";
          public static LocString DESC = (LocString) "Thick, soft gray socks with extra flounce.";
        }

        public class GINCH_GREY_CHARCOAL
        {
          public static LocString NAME = (LocString) "Frilly Charcoal Socks";
          public static LocString DESC = (LocString) "Thick, soft dark gray socks with extra flounce.";
        }
      }
    }

    public class CLOTHING_HATS
    {
      public static LocString NAME = (LocString) "Default Headgear";
      public static LocString DESC = (LocString) "<DESC>";

      public class FACADES
      {
      }
    }

    public class CLOTHING_ACCESORIES
    {
      public static LocString NAME = (LocString) "Default Accessory";
      public static LocString DESC = (LocString) "<DESC>";

      public class FACADES
      {
      }
    }

    public class OXYGEN_TANK
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Oxygen Tank", nameof (OXYGEN_TANK));
      public static LocString GENERICNAME = (LocString) "Equipment";
      public static LocString DESC = (LocString) "It's like a to-go bag for your lungs.";
      public static LocString EFFECT = (LocString) "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>.";
      public static LocString RECIPE_DESC = (LocString) "Allows Duplicants to breathe in hazardous environments.\n\nDoes not work when submerged in <style=\"liquid\">Liquid</style>";
    }

    public class OXYGEN_TANK_UNDERWATER
    {
      public static LocString NAME = (LocString) "Oxygen Rebreather";
      public static LocString GENERICNAME = (LocString) "Equipment";
      public static LocString DESC = (LocString) "";
      public static LocString EFFECT = (LocString) "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid.";
      public static LocString RECIPE_DESC = (LocString) "Allows Duplicants to breathe while submerged in <style=\"liquid\">Liquid</style>.\n\nDoes not work outside of liquid";
    }

    public class EQUIPPABLEBALLOON
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Balloon Friend", nameof (EQUIPPABLEBALLOON));
      public static LocString DESC = (LocString) "A floating friend to reassure my Duplicants they are so very, very clever.";
      public static LocString EFFECT = (LocString) $"Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist {UI.FormatAsLink("Overjoyed", "MORALE")} response.";
      public static LocString RECIPE_DESC = (LocString) $"Gives Duplicants a boost in brain function.\n\nSupplied by Duplicants with the Balloon Artist {UI.FormatAsLink("Overjoyed", "MORALE")} response";
      public static LocString GENERICNAME = (LocString) "Balloon Friend";

      public class FACADES
      {
        public class DEFAULT_BALLOON
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Balloon Friend", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A floating friend to reassure my Duplicants that they are so very, very clever.";
        }

        public class BALLOON_FIREENGINE_LONG_SPARKLES
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Magma Glitter", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "They float <i>and</i> sparkle!";
        }

        public class BALLOON_YELLOW_LONG_SPARKLES
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Lavatory Glitter", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "Sparkly balloons in an all-too-familiar hue.";
        }

        public class BALLOON_BLUE_LONG_SPARKLES
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Wheezewort Glitter", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "They float <i>and</i> sparkle!";
        }

        public class BALLOON_GREEN_LONG_SPARKLES
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Mush Bar Glitter", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "They float <i>and</i> sparkle!";
        }

        public class BALLOON_PINK_LONG_SPARKLES
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Petal Glitter", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "They float <i>and</i> sparkle!";
        }

        public class BALLOON_PURPLE_LONG_SPARKLES
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Dusky Glitter", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "They float <i>and</i> sparkle!";
        }

        public class BALLOON_BABY_PACU_EGG
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Floatie Fish", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "They do not taste as good as the real thing.";
        }

        public class BALLOON_BABY_GLOSSY_DRECKO_EGG
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Glossy Glee", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A happy little trio of inflatable critters.";
        }

        public class BALLOON_BABY_HATCH_EGG
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Helium Hatches", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A happy little trio of inflatable critters.";
        }

        public class BALLOON_BABY_POKESHELL_EGG
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Peppy Pokeshells", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A happy little trio of inflatable critters.";
        }

        public class BALLOON_BABY_PUFT_EGG
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Puffed-Up Pufts", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A happy little trio of inflatable critters.";
        }

        public class BALLOON_BABY_SHOVOLE_EGG
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Voley Voley Voles", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A happy little trio of inflatable critters.";
        }

        public class BALLOON_BABY_PIP_EGG
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Pip Pip Hooray", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A happy little trio of inflatable critters.";
        }

        public class CANDY_BLUEBERRY
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Candied Blueberry", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A juicy bunch of blueberry-scented balloons.";
        }

        public class CANDY_GRAPE
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Candied Grape", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A juicy bunch of grape-scented balloons.";
        }

        public class CANDY_LEMON
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Candied Lemon", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A juicy lemon-scented bunch of balloons.";
        }

        public class CANDY_LIME
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Candied Lime", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A juicy lime-scented bunch of balloons.";
        }

        public class CANDY_ORANGE
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Candied Satsuma", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A juicy satsuma-scented bunch of balloons.";
        }

        public class CANDY_STRAWBERRY
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Candied Strawberry", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A juicy strawberry-scented bunch of balloons.";
        }

        public class CANDY_WATERMELON
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Candied Watermelon", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "A juicy watermelon-scented bunch of balloons.";
        }

        public class HAND_GOLD
        {
          public static LocString NAME = (LocString) UI.FormatAsLink("Gold Fingers", nameof (EQUIPPABLEBALLOON));
          public static LocString DESC = (LocString) "Inflatable gestures of encouragement.";
        }
      }
    }

    public class SLEEPCLINICPAJAMAS
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");
      public static LocString GENERICNAME = (LocString) "Clothing";
      public static LocString DESC = (LocString) "A soft, fleecy ticket to dreamland.";
      public static LocString EFFECT = (LocString) $"Helps Duplicants fall asleep by reducing {UI.FormatAsLink("Stamina", "HEALTH")}.\n\nEnables the wearer to dream and produce Dream Journals.";
      public static LocString DESTROY_TOAST = (LocString) "Ripped Pajamas";
    }
  }
}
