// Decompiled with JetBrains decompiler
// Type: STRINGS.ROBOTS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace STRINGS;

public class ROBOTS
{
  public static LocString CATEGORY_NAME = (LocString) "Robots";

  public class STATS
  {
    public class INTERNALBATTERY
    {
      public static LocString NAME = (LocString) "Rechargeable Battery";
      public static LocString TOOLTIP = (LocString) "When this bot's battery runs out it must temporarily stop working to go recharge";
    }

    public class INTERNALCHEMICALBATTERY
    {
      public static LocString NAME = (LocString) "Chemical Battery";
      public static LocString TOOLTIP = (LocString) "This bot will shut down permanently when its battery runs out";
    }

    public class INTERNALBIOBATTERY
    {
      public static LocString NAME = (LocString) "Biofuel";
      public static LocString TOOLTIP = (LocString) "This bot will shut down permanently when its biofuel runs out";
    }

    public class INTERNALELECTROBANK
    {
      public static LocString NAME = (LocString) "Power Bank";
      public static LocString TOOLTIP = (LocString) $"When this bot's {UI.PRE_KEYWORD}Power Bank{UI.PST_KEYWORD} runs out, it will stop working until a fully charged one is delivered";
    }
  }

  public class ATTRIBUTES
  {
    public class INTERNALBATTERYDELTA
    {
      public static LocString NAME = (LocString) "Rechargeable Battery Drain";
      public static LocString TOOLTIP = (LocString) "The rate at which battery life is depleted";
    }
  }

  public class STATUSITEMS
  {
    public class CANTREACHSTATION
    {
      public static LocString NAME = (LocString) "Unreachable Dock";
      public static LocString DESC = (LocString) "Obstacles are preventing {0} from heading home";
      public static LocString TOOLTIP = (LocString) "Obstacles are preventing {0} from heading home";
    }

    public class MOVINGTOCHARGESTATION
    {
      public static LocString NAME = (LocString) "Traveling to Dock";
      public static LocString DESC = (LocString) "{0} is on its way home to recharge";
      public static LocString TOOLTIP = (LocString) "{0} is on its way home to recharge";
    }

    public class LOWBATTERY
    {
      public static LocString NAME = (LocString) "Low Battery";
      public static LocString DESC = (LocString) "{0}'s battery is low and needs to recharge";
      public static LocString TOOLTIP = (LocString) "{0}'s battery is low and needs to recharge";
    }

    public class LOWBATTERYNOCHARGE
    {
      public static LocString NAME = (LocString) "Low Battery";
      public static LocString DESC = (LocString) "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";
      public static LocString TOOLTIP = (LocString) "{0}'s battery is low\n\nThe internal battery cannot be recharged and this robot will cease functioning after it is depleted.";
    }

    public class DEADBATTERY
    {
      public static LocString NAME = (LocString) "Shut Down";
      public static LocString DESC = (LocString) "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";
      public static LocString TOOLTIP = (LocString) "RIP {0}\n\n{0}'s battery has been depleted and cannot be recharged";
    }

    public class DEADBATTERYFLYDO
    {
      public static LocString NAME = (LocString) "Shut Down";
      public static LocString DESC = (LocString) "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";
      public static LocString TOOLTIP = (LocString) "{0}'s battery has been depleted\n\n{0} will resume function when a new battery has been delivered";
    }

    public class DUSTBINFULL
    {
      public static LocString NAME = (LocString) "Dust Bin Full";
      public static LocString DESC = (LocString) "{0} must return to its dock to unload";
      public static LocString TOOLTIP = (LocString) "{0} must return to its dock to unload";
    }

    public class WORKING
    {
      public static LocString NAME = (LocString) "Working";
      public static LocString DESC = (LocString) "{0} is working diligently. Great job, {0}!";
      public static LocString TOOLTIP = (LocString) "{0} is working diligently. Great job, {0}!";
    }

    public class UNLOADINGSTORAGE
    {
      public static LocString NAME = (LocString) "Unloading";
      public static LocString DESC = (LocString) "{0} is emptying out its dust bin";
      public static LocString TOOLTIP = (LocString) "{0} is emptying out its dust bin";
    }

    public class CHARGING
    {
      public static LocString NAME = (LocString) "Charging";
      public static LocString DESC = (LocString) "{0} is recharging its battery";
      public static LocString TOOLTIP = (LocString) "{0} is recharging its battery";
    }

    public class REACTPOSITIVE
    {
      public static LocString NAME = (LocString) "Happy Reaction";
      public static LocString DESC = (LocString) "This bot saw something nice!";
      public static LocString TOOLTIP = (LocString) "This bot saw something nice!";
    }

    public class REACTNEGATIVE
    {
      public static LocString NAME = (LocString) "Bothered Reaction";
      public static LocString DESC = (LocString) "This bot saw something upsetting";
      public static LocString TOOLTIP = (LocString) "This bot saw something upsetting";
    }
  }

  public class MODELS
  {
    public class MORB
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Biobot", "STORYTRAITMORBROVER");
      public static LocString DESC = (LocString) "A Pathogen-Fueled Extravehicular Geo-Exploratory Guidebot (model Y), aka \"P.E.G.G.Y.\"\n\nIt can be assigned basic building tasks and digging duties in hazardous environments.";
      public static LocString CODEX_DESC = (LocString) "The pathogen-fueled guidebot is designed to maximize a colony's chances of surviving in hostile environments by meeting three core outcomes:\n\n1. Filtration and removal of toxins from environment;\n2. Safe disposal of filtered toxins through conversion into usable biofuel;\n3. Creation of geo-exploration equipment for colony expansion with minimal colonist endangerment.\n\nThe elements aggregated during this process may result in the unintentional spread of contaminants. Specialized training required for safe handling.";
    }

    public class SCOUT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Rover", "SCOUTROVER");
      public static LocString DESC = (LocString) $"A curious bot that can remotely explore new {(string) UI.CLUSTERMAP.PLANETOID_KEYWORD} locations.";
    }

    public class SWEEPBOT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Sweepy", "SWEEPY");
      public static LocString DESC = (LocString) $"An automated sweeping robot.\n\nSweeps up {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} debris and {UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID")} spills and stores the material back in its {UI.FormatAsLink("Sweepy Dock", "SWEEPBOTSTATION")}.";
    }

    public class FLYDO
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Flydo", "FETCHDRONE");
      public static LocString DESC = (LocString) $"A programmable delivery robot.\n\nPicks up {UI.FormatAsLink("Solid", "ELEMENTS_SOLID")} objects for delivery to selected destinations.";
    }
  }
}
