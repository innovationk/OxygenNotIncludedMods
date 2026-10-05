// Decompiled with JetBrains decompiler
// Type: STRINGS.UI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace STRINGS;

public class UI
{
  public static string PRE_KEYWORD = "<style=\"KKeyword\">";
  public static string PST_KEYWORD = "</style>";
  public static string PRE_POS_MODIFIER = "<b>";
  public static string PST_POS_MODIFIER = "</b>";
  public static string PRE_NEG_MODIFIER = "<b>";
  public static string PST_NEG_MODIFIER = "</b>";
  public static string PRE_RATE_NEGATIVE = "<style=\"consumed\">";
  public static string PRE_RATE_POSITIVE = "<style=\"produced\">";
  public static string PST_RATE = "</style>";
  public static string CODEXLINK = "REQUIREMENTCLASS";
  public static string PRE_AUTOMATION_ACTIVE = "<b><style=\"logic_on\">";
  public static string PRE_AUTOMATION_STANDBY = "<b><style=\"logic_off\">";
  public static string PST_AUTOMATION = "</style></b>";
  public static string YELLOW_PREFIX = "<color=#ffff00ff>";
  public static string COLOR_SUFFIX = "</color>";
  public static string HORIZONTAL_RULE = "------------------";
  public static string HORIZONTAL_BR_RULE = $"\n{UI.HORIZONTAL_RULE}\n";
  public static LocString POS_INFINITY = (LocString) "Infinity";
  public static LocString NEG_INFINITY = (LocString) "-Infinity";
  public static LocString PROCEED_BUTTON = (LocString) "PROCEED";
  public static LocString COPY_BUILDING = (LocString) "Copy";
  public static LocString COPY_BUILDING_TOOLTIP = (LocString) "Create new build orders using the most recent building selection as a template. {Hotkey}";
  public static LocString NAME_WITH_UNITS = (LocString) "{0} x {1}";
  public static LocString NA = (LocString) "N/A";
  public static LocString POSITIVE_FORMAT = (LocString) "+{0}";
  public static LocString NEGATIVE_FORMAT = (LocString) "-{0}";
  public static LocString FILTER = (LocString) "Filter";
  public static LocString SPEED_SLOW = (LocString) "SLOW";
  public static LocString SPEED_MEDIUM = (LocString) "MEDIUM";
  public static LocString SPEED_FAST = (LocString) "FAST";
  public static LocString RED_ALERT = (LocString) "RED ALERT";
  public static LocString JOBS = (LocString) "PRIORITIES";
  public static LocString CONSUMABLES = (LocString) nameof (CONSUMABLES);
  public static LocString VITALS = (LocString) nameof (VITALS);
  public static LocString RESEARCH = (LocString) nameof (RESEARCH);
  public static LocString ROLES = (LocString) "JOB ASSIGNMENTS";
  public static LocString RESEARCHPOINTS = (LocString) "Research points";
  public static LocString SCHEDULE = (LocString) nameof (SCHEDULE);
  public static LocString REPORT = (LocString) "REPORTS";
  public static LocString SKILLS = (LocString) nameof (SKILLS);
  public static LocString OVERLAYSTITLE = (LocString) "OVERLAYS";
  public static LocString ALERTS = (LocString) nameof (ALERTS);
  public static LocString MESSAGES = (LocString) nameof (MESSAGES);
  public static LocString ACTIONS = (LocString) nameof (ACTIONS);
  public static LocString QUEUE = (LocString) "Queue";
  public static LocString BASECOUNT = (LocString) "Base {0}";
  public static LocString CHARACTERCONTAINER_SKILLS_TITLE = (LocString) "ATTRIBUTES";
  public static LocString CHARACTERCONTAINER_TRAITS_TITLE = (LocString) "TRAITS";
  public static LocString CHARACTERCONTAINER_TRAITS_TITLE_BIONIC = (LocString) "BIONIC SYSTEMS";
  public static LocString CHARACTERCONTAINER_APTITUDES_TITLE = (LocString) "INTERESTS";
  public static LocString CHARACTERCONTAINER_APTITUDES_TITLE_TOOLTIP = (LocString) "A Duplicant's starting Attributes are determined by their Interests\n\nLearning Skills related to their Interests will give Duplicants a Morale boost";
  public static LocString CHARACTERCONTAINER_EXPECTATIONS_TITLE = (LocString) "ADDITIONAL INFORMATION";
  public static LocString CHARACTERCONTAINER_SKILL_VALUE = (LocString) " {0} {1}";
  public static LocString CHARACTERCONTAINER_NEED = (LocString) "{0}: {1}";
  public static LocString CHARACTERCONTAINER_STRESSTRAIT = (LocString) "Stress Reaction: {0}";
  public static LocString CHARACTERCONTAINER_JOYTRAIT = (LocString) "Overjoyed Response: {0}";
  public static LocString CHARACTERCONTAINER_CONGENITALTRAIT = (LocString) "Genetic Trait: {0}";
  public static LocString CHARACTERCONTAINER_NOARCHETYPESELECTED = (LocString) "Random";
  public static LocString CHARACTERCONTAINER_ARCHETYPESELECT_TOOLTIP = (LocString) "Change the type of Duplicant the reroll button will produce";
  public static LocString CAREPACKAGECONTAINER_INFORMATION_TITLE = (LocString) "CARE PACKAGE";
  public static LocString CHARACTERCONTAINER_CONFIRM_OUTFIT_SELECTION_TOOLTIP = (LocString) "Click to confirm this outfit selection.";
  public static LocString CHARACTERCONTAINER_ALL_MODELS = (LocString) "Any";
  public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED = (LocString) "Increased <b>{0}</b>";
  public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED = (LocString) "Decreased <b>{0}</b>";
  public static LocString CHARACTERCONTAINER_FILTER_STANDARD = (LocString) "Check box to allow standard Duplicants";
  public static LocString CHARACTERCONTAINER_FILTER_BIONIC = (LocString) "Check box to allow Bionic Duplicants";
  public static LocString CHARACTERCONTAINER_NO_OUTFIT = (LocString) "Default Outfit";
  public static LocString CHARACTERCONTAINER_NEXT_OUTFIT = (LocString) "Next Outfit";
  public static LocString CHARACTERCONTAINER_PREV_OUTFIT = (LocString) "Previous Outfit";
  public static LocString CHARACTERCONTAINER_EXPAND_OUTFIT_SELECTOR_BUTTON = (LocString) $"<b>Current Outfit:</b> {{0}}\n\nClick to toggle between different outfits owned by this colony\n\nVisit the {UI.PRE_KEYWORD}Supply Closet{UI.PST_KEYWORD} to create new outfits";
  public static LocString PRODUCTINFO_SELECTMATERIAL = (LocString) "Select {0}:";
  public static LocString PRODUCTINFO_RESEARCHREQUIRED = (LocString) "Research required...";
  public static LocString PRODUCTINFO_REQUIRESRESEARCHDESC = (LocString) "Requires research: {0}";
  public static LocString PRODUCTINFO_APPLICABLERESOURCES = (LocString) "Required resources:";
  public static LocString PRODUCTINFO_MISSINGRESOURCES_TITLE = (LocString) "Requires {0}: {1}";
  public static LocString PRODUCTINFO_INSUFFICIENTRESOURCES_TITLE = (LocString) "Insufficient {0} available";
  public static LocString PRODUCTINFO_MISSINGRESOURCES_HOVER = (LocString) "Missing resources";
  public static LocString PRODUCTINFO_MISSINGRESOURCES_DESC = (LocString) "{0} has yet to be discovered";
  public static LocString PRODUCTINFO_UNIQUE_PER_WORLD = (LocString) ("Limit one per " + (string) UI.CLUSTERMAP.PLANETOID_KEYWORD);
  public static LocString PRODUCTINFO_ROCKET_INTERIOR = (LocString) "Rocket interior only";
  public static LocString PRODUCTINFO_ROCKET_NOT_INTERIOR = (LocString) "Cannot build inside rocket";
  public static LocString BUILDTOOL_ROTATE = (LocString) "Rotate this building";
  public static LocString BUILDTOOL_ROTATE_CURRENT_DEGREES = (LocString) "Currently rotated {Degrees} degrees";
  public static LocString BUILDTOOL_ROTATE_CURRENT_LEFT = (LocString) "Currently facing left";
  public static LocString BUILDTOOL_ROTATE_CURRENT_RIGHT = (LocString) "Currently facing right";
  public static LocString BUILDTOOL_ROTATE_CURRENT_UP = (LocString) "Currently facing up";
  public static LocString BUILDTOOL_ROTATE_CURRENT_DOWN = (LocString) "Currently facing down";
  public static LocString BUILDTOOL_ROTATE_CURRENT_UPRIGHT = (LocString) "Currently upright";
  public static LocString BUILDTOOL_ROTATE_CURRENT_ON_SIDE = (LocString) "Currently on its side";
  public static LocString BUILDTOOL_CANT_ROTATE = (LocString) "This building cannot be rotated";
  public static LocString EQUIPMENTTAB_OWNED = (LocString) "Owned Items";
  public static LocString EQUIPMENTTAB_HELD = (LocString) "Held Items";
  public static LocString EQUIPMENTTAB_ROOM = (LocString) "Assigned Rooms";
  public static LocString JOBSCREEN_PRIORITY = (LocString) "Priority";
  public static LocString JOBSCREEN_HIGH = (LocString) "High";
  public static LocString JOBSCREEN_LOW = (LocString) "Low";
  public static LocString JOBSCREEN_EVERYONE = (LocString) "Everyone";
  public static LocString JOBSCREEN_DEFAULT = (LocString) "New Duplicants";
  public static LocString BUILD_REQUIRES_SKILL = (LocString) "Skill: {Skill}";
  public static LocString BUILD_REQUIRES_SKILL_TOOLTIP = (LocString) "At least one Duplicant must have the {Skill} Skill to construct this building";
  public static LocString OPERATION_REQUIRES_SKILL = (LocString) "Skilled Operator: {Skill}";
  public static LocString OPERATION_REQUIRES_SKILL_TOOLTIP = (LocString) "Only a Duplicant with the {Skill} Skill can operate this building";
  public static LocString VITALSSCREEN_NAME = (LocString) "Name";
  public static LocString VITALSSCREEN_STRESS = (LocString) "Stress";
  public static LocString VITALSSCREEN_HEALTH = (LocString) "Health";
  public static LocString VITALSSCREEN_SICKNESS = (LocString) "Disease";
  public static LocString VITALSSCREEN_POWERBANKS = (LocString) "Power";
  public static LocString VITALSSCREEN_CALORIES = (LocString) "Fullness";
  public static LocString VITALSSCREEN_RATIONS = (LocString) "Calories / Cycle";
  public static LocString VITALSSCREEN_EATENTODAY = (LocString) "Eaten Today";
  public static LocString VITALSSCREEN_RATIONS_TOOLTIP = (LocString) "Set how many calories this Duplicant may consume daily";
  public static LocString VITALSSCREEN_EATENTODAY_TOOLTIP = (LocString) "The amount of food this Duplicant has eaten this cycle";
  public static LocString VITALSSCREEN_UNTIL_FULL = (LocString) "Until Full";
  public static LocString RESEARCHSCREEN_UNLOCKSTOOLTIP = (LocString) "Unlocks: {0}";
  public static LocString RESEARCHSCREEN_FILTER = (LocString) "Search Tech";
  public static LocString ATTRIBUTELEVEL = (LocString) "Expertise: Level {0} {1}";
  public static LocString ATTRIBUTELEVEL_SHORT = (LocString) "Level {0} {1}";
  public static LocString NEUTRONIUMMASS = (LocString) "Immeasurable";
  public static LocString CALCULATING = (LocString) "Calculating...";
  public static LocString FORMATDAY = (LocString) "{0:F1} cycles";
  public static LocString FORMATSECONDS = (LocString) "{0}s";
  public static LocString DELIVERED = (LocString) "Delivered: {0} {1}";
  public static LocString PICKEDUP = (LocString) "Picked Up: {0} {1}";
  public static LocString COPIED_SETTINGS = (LocString) "Settings Applied";
  public static LocString WELCOMEMESSAGETITLE = (LocString) "- ALERT -";
  public static LocString WELCOMEMESSAGEBODY = (LocString) "I've awoken at the target location, but colonization efforts have already hit a hitch. I was supposed to land on the planet's surface, but became trapped many miles underground instead.\n\nAlthough the conditions are not ideal, it's imperative that I establish a colony here and begin mounting efforts to escape.";
  public static LocString WELCOMEMESSAGEBODY_SPACEDOUT = (LocString) "The asteroid we call home has collided with an anomalous planet, decimating our colony. Rebuilding it is of the utmost importance.\n\nI've detected a new cluster of material-rich planetoids in nearby space. If I can guide the Duplicants through the perils of space travel, we could build a colony even bigger and better than before.";
  public static LocString WELCOMEMESSAGEBODY_KF23 = (LocString) "This asteroid is oddly tilted, as though a powerful external force once knocked it off its axis.\n\nI'll need to recalibrate my approach to colony-building in order to make the most of this unusual distribution of resources.";
  public static LocString WELCOMEMESSAGEBODY_DLC2_CERES = (LocString) "The ambient temperatures of this planet are inhospitably low.\n\nI've detected the ruins of a scientifically advanced settlement buried deep beneath our landing site.\n\nIf my Duplicants can survive the journey into this frosty planet's core, we could use this newfound technology to build a colony like no other.";
  public static LocString WELCOMEMESSAGEBODY_DLC4_PREHISTORIC = (LocString) "My collision monitoring system has detected an imminent threat to our survival: a huge impactor asteroid is hurtling directly at this planet.\n\nWe must make our way to the surface and mount a defense system in time to destroy the incoming asteroid before it destroys us.";
  public static LocString WELCOMEMESSAGEBODY_DLC4_PREHISTORIC_SHATTERED = (LocString) "Impactor asteroid collision in 10 cycles!\n\nMy scans indicate that the impact will trigger the eruption of all geysers that surround our landing site. There are...so many.\n\nInitiate survival procedures immediately.";
  public static LocString WELCOMEMESSAGEBODY_DLC5_AQUATIC = (LocString) "This asteroid has suffered extensive flooding. My scans show traces of a previous party of explorers, but... only one remains.\n\nIt's imperative that I find and recruit the survivor to my colony. Their adaptations may well be the key to plumbing the depths of this waterlogged world.";
  public static LocString WELCOMEMESSAGEBEGIN = (LocString) "BEGIN";
  public static LocString VIEWDUPLICANTS = (LocString) "Choose a Blueprint";
  public static LocString DUPLICANTPRINTING = (LocString) "Duplicant Printing";
  public static LocString ASSIGNDUPLICANT = (LocString) "Assign Duplicant";
  public static LocString CRAFT = (LocString) "ADD TO QUEUE";
  public static LocString CLEAR_COMPLETED = (LocString) "CLEAR COMPLETED ORDERS";
  public static LocString CRAFT_CONTINUOUS = (LocString) "CONTINUOUS";
  public static LocString INCUBATE_CONTINUOUS_TOOLTIP = (LocString) "When checked, this building will continuously incubate eggs of the selected type";
  public static LocString PLACEINRECEPTACLE = (LocString) "Plant";
  public static LocString REMOVEFROMRECEPTACLE = (LocString) "Uproot";
  public static LocString CANCELPLACEINRECEPTACLE = (LocString) "Cancel";
  public static LocString CANCELREMOVALFROMRECEPTACLE = (LocString) "Cancel";
  public static LocString CHANGEPERSECOND = (LocString) "Change per second: {0}";
  public static LocString CHANGEPERCYCLE = (LocString) "Total change per cycle: {0}";
  public static LocString CHANGEPERCYCLE_FRESH = (LocString) "Total change in freshness per cycle: {0}";
  public static LocString MODIFIER_ITEM_TEMPLATE = (LocString) "    • {0}: {1}";
  public static LocString LISTENTRYSTRING = (LocString) "     {0}\n";
  public static LocString LISTENTRYSTRINGNOLINEBREAK = (LocString) "     {0}";

  public static string FormatAsBuildMenuTab(string text) => $"<b>{text}</b>";

  public static string FormatAsBuildMenuTab(string text, string hotkey)
  {
    return $"<b>{text}</b> {UI.FormatAsHotkey(hotkey)}";
  }

  public static string FormatAsBuildMenuTab(string text, Action a)
  {
    return $"<b>{text}</b> {UI.FormatAsHotKey(a)}";
  }

  public static string FormatAsOverlay(string text) => $"<b>{text}</b>";

  public static string FormatAsOverlay(string text, string hotkey)
  {
    return $"<b>{text}</b> {UI.FormatAsHotkey(hotkey)}";
  }

  public static string FormatAsOverlay(string text, Action a)
  {
    return $"<b>{text}</b> {UI.FormatAsHotKey(a)}";
  }

  public static string FormatAsManagementMenu(string text) => $"<b>{text}</b>";

  public static string FormatAsManagementMenu(string text, string hotkey)
  {
    return $"<b>{text}</b> {UI.FormatAsHotkey(hotkey)}";
  }

  public static string FormatAsManagementMenu(string text, Action a)
  {
    return $"<b>{text}</b> {UI.FormatAsHotKey(a)}";
  }

  public static string FormatAsKeyWord(string text) => UI.PRE_KEYWORD + text + UI.PST_KEYWORD;

  public static string FormatAsHotkey(string text) => $"<b><color=#F44A4A>{text}</b></color>";

  public static string FormatAsHotKey(Action a) => $"{{Hotkey/{a.ToString()}}}";

  public static string FormatAsTool(string text, string hotkey)
  {
    return $"<b>{text}</b> {UI.FormatAsHotkey(hotkey)}";
  }

  public static string FormatAsTool(string text, Action a)
  {
    return $"<b>{text}</b> {UI.FormatAsHotKey(a)}";
  }

  public static string FormatAsLink(string text, string linkID)
  {
    text = UI.StripLinkFormatting(text);
    linkID = CodexCache.FormatLinkID(linkID);
    return $"<link=\"{linkID}\">{text}</link>";
  }

  public static string FormatAsPositiveModifier(string text)
  {
    return UI.PRE_POS_MODIFIER + text + UI.PST_POS_MODIFIER;
  }

  public static string FormatAsNegativeModifier(string text)
  {
    return UI.PRE_NEG_MODIFIER + text + UI.PST_NEG_MODIFIER;
  }

  public static string FormatAsPositiveRate(string text)
  {
    return UI.PRE_RATE_POSITIVE + text + UI.PST_RATE;
  }

  public static string FormatAsNegativeRate(string text)
  {
    return UI.PRE_RATE_NEGATIVE + text + UI.PST_RATE;
  }

  public static string CLICK(UI.ClickType c) => $"(ClickType/{c.ToString()})";

  public static string FormatAsAutomationState(string text, UI.AutomationState state)
  {
    return state == UI.AutomationState.Active ? UI.PRE_AUTOMATION_ACTIVE + text + UI.PST_AUTOMATION : UI.PRE_AUTOMATION_STANDBY + text + UI.PST_AUTOMATION;
  }

  public static string FormatAsCaps(string text) => text.ToUpper();

  public static string ExtractLinkID(string text)
  {
    string linkId = text;
    int num1 = linkId.IndexOf("<link=");
    if (num1 != -1)
    {
      int startIndex = num1 + 7;
      int num2 = linkId.IndexOf(">") - 1;
      linkId = text.Substring(startIndex, num2 - startIndex);
    }
    return linkId;
  }

  public static string StripTagFormatting(string text, string tag)
  {
    string str1 = text;
    try
    {
      string str2 = $"<{tag}=";
      string str3 = $"</{tag}>";
      int length = str3.Length;
      while (str1.Contains(str2))
      {
        int startIndex1 = str1.IndexOf(str3);
        if (startIndex1 > -1)
          str1 = str1.Remove(startIndex1, length);
        else
          Debug.LogWarningFormat("String has no closing {0} tag: {1}", (object) tag, (object) text);
        int startIndex2 = str1.IndexOf(str2);
        if (startIndex2 != -1)
        {
          int num = str1.IndexOf("\">", startIndex2);
          if (num != -1)
          {
            str1 = str1.Remove(startIndex2, num - startIndex2 + 2);
          }
          else
          {
            str1 = str1.Remove(startIndex2, str2.Length);
            Debug.LogWarningFormat("String has no open {0} closure: {1}", (object) tag, (object) text);
          }
        }
        else
          Debug.LogWarningFormat("String has no open {0} tag: {1}", (object) tag, (object) text);
      }
    }
    catch
    {
      Debug.LogFormat("STRIP TAG FORMATTING FOR {0} FAILED ON: {1}", (object) tag, (object) text);
      str1 = text;
    }
    return str1;
  }

  public static string StripLinkFormatting(string text)
  {
    return UI.StripTagFormatting(UI.StripTagFormatting(text, "link"), "LINK");
  }

  public static string StripStyleFormatting(string text)
  {
    return UI.StripTagFormatting(UI.StripTagFormatting(text, "style"), "STYLE");
  }

  public static class PLATFORMS
  {
    public static LocString UNKNOWN = (LocString) "Your game client";
    public static LocString STEAM = (LocString) "Steam";
    public static LocString EPIC = (LocString) "Epic Games Store";
    public static LocString WEGAME = (LocString) "Wegame";
  }

  private enum KeywordType
  {
    Hotkey,
    BuildMenu,
    Attribute,
    Generic,
  }

  public enum ClickType
  {
    Click,
    Clicked,
    Clicking,
    Clickable,
    Clicks,
    click,
    clicked,
    clicking,
    clickable,
    clicks,
    CLICK,
    CLICKED,
    CLICKING,
    CLICKABLE,
    CLICKS,
  }

  public enum AutomationState
  {
    Active,
    Standby,
  }

  public class UNKNOWN_DLC
  {
    public static LocString NAME = (LocString) "Unknown DLC";
    public static LocString NAME_ITAL = (LocString) $"<i>{(string) UI.UNKNOWN_DLC.NAME}</i>";
  }

  public class VANILLA
  {
    public static LocString NAME = (LocString) "Base Game";
    public static LocString NAME_ITAL = (LocString) $"<i>{(string) UI.VANILLA.NAME}</i>";
  }

  public class EXPANSION1
  {
    public static LocString NAME = (LocString) "Spaced Out!";
    public static LocString NAME_ITAL = (LocString) $"<i>{(string) UI.EXPANSION1.NAME}</i>";
  }

  public class DLC2
  {
    public static LocString NAME = (LocString) "The Frosty Planet Pack";
    public static LocString NAME_ITAL = (LocString) $"<i>{(string) UI.DLC2.NAME}</i>";
    public static LocString MIXING_TOOLTIP = (LocString) "<b><i>The Frosty Planet Pack</i></b> features frozen biomes and elements useful in thermal regulation";
  }

  public class DLC3
  {
    public static LocString NAME = (LocString) "The Bionic Booster Pack";
    public static LocString NAME_ITAL = (LocString) $"<i>{(string) UI.DLC3.NAME}</i>";
    public static LocString MIXING_TOOLTIP = (LocString) ((string) UI.DLC3.NAME_ITAL + " features portable power storage, bionic Duplicants, and remote building operation");
  }

  public class DLC4
  {
    public static LocString NAME = (LocString) "The Prehistoric Planet Pack";
    public static LocString NAME_ITAL = (LocString) $"<i>{(string) UI.DLC4.NAME}</i>";
    public static LocString MIXING_TOOLTIP = (LocString) ((string) UI.DLC4.NAME_ITAL + " features carnivorous flora and fauna, biodiesel, and a focus on time-sensitive surface defense");
  }

  public class DLC5
  {
    public static LocString NAME = (LocString) "The Aquatic Planet Pack";
    public static LocString NAME_ITAL = (LocString) $"<i>{(string) UI.DLC5.NAME}</i>";
    public static LocString MIXING_TOOLTIP = (LocString) ((string) UI.DLC5.NAME_ITAL + " features underwater flora and fauna, diving deep for resources, and a splashy new survivor");
  }

  public class COSMETIC1
  {
    public static LocString NAME = (LocString) "Neutronium Cosmetics Pack";
    public static LocString NAME_ITAL = (LocString) $"<i>{(string) UI.COSMETIC1.NAME}</i>";
    public static LocString MIXING_TOOLTIP = (LocString) ((string) UI.COSMETIC1.NAME_ITAL + " features cosmetic blueprints fit for royalty");
  }

  public class DIAGNOSTICS_SCREEN
  {
    public static LocString TITLE = (LocString) "Diagnostics";
    public static LocString DIAGNOSTIC = (LocString) "Diagnostic";
    public static LocString TOTAL = (LocString) "Total";
    public static LocString RESERVED = (LocString) "Reserved";
    public static LocString STATUS = (LocString) "Status";
    public static LocString SEARCH = (LocString) "Search";
    public static LocString CRITERIA_HEADER_TOOLTIP = (LocString) "Expand or collapse diagnostic criteria panel";
    public static LocString SEE_ALL = (LocString) "+ See All ({0})";
    public static LocString CRITERIA_TOOLTIP = (LocString) "Toggle the <b>{0}</b> diagnostics evaluation of the <b>{1}</b> criteria";
    public static LocString CRITERIA_ENABLED_COUNT = (LocString) "{0}/{1} criteria enabled";

    public class CLICK_TOGGLE_MESSAGE
    {
      public static LocString ALWAYS = (LocString) (UI.CLICK(UI.ClickType.Click) + " to pin this diagnostic to the sidebar - Current State: <b>Visible On Alert Only</b>");
      public static LocString ALERT_ONLY = (LocString) (UI.CLICK(UI.ClickType.Click) + " to subscribe to this diagnostic - Current State: <b>Never Visible</b>");
      public static LocString NEVER = (LocString) (UI.CLICK(UI.ClickType.Click) + " to mute this diagnostic on the sidebar - Current State: <b>Always Visible</b>");
      public static LocString TUTORIAL_DISABLED = (LocString) (UI.CLICK(UI.ClickType.Click) + " to enable this diagnostic -  Current State: <b>Temporarily disabled</b>");
    }
  }

  public class TEMPORARY_ACTIONS
  {
    public class CAMERA_RETURN
    {
      public static LocString NAME = (LocString) "Camera: Return";
      public static LocString TOOLTIP = (LocString) "Return camera to its previous position";
    }
  }

  public class WORLD_SELECTOR_SCREEN
  {
    public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;
  }

  public class COLONY_DIAGNOSTICS
  {
    public static LocString NO_MINIONS_PLANETOID = (LocString) "    • There are no Duplicants on this planetoid";
    public static LocString NO_MINIONS_ROCKET = (LocString) "    • There are no Duplicants aboard this rocket";
    public static LocString ROCKET = (LocString) "rocket";
    public static LocString NO_MINIONS_REQUESTED = (LocString) "    • Crew must be requested to update this diagnostic";
    public static LocString NO_DATA = (LocString) "    • Not enough data for evaluation";
    public static LocString NO_DATA_SHORT = (LocString) "    • No data";
    public static LocString MUTE_TUTORIAL = (LocString) "Diagnostic can be muted in the <b><color=#E5B000>See All</color></b> panel";
    public static LocString GENERIC_STATUS_NORMAL = (LocString) "All values normal";
    public static LocString PLACEHOLDER_CRITERIA_NAME = (LocString) "";
    public static LocString GENERIC_CRITERIA_PASS = (LocString) "Criteria met";
    public static LocString GENERIC_CRITERIA_FAIL = (LocString) "Criteria not met";

    public class GENERIC_CRITERIA
    {
      public static LocString CHECKWORLDHASMINIONS = (LocString) "Check world has Duplicants";
    }

    public class IDLEDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Idleness";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Idleness</b>";
      public static LocString NORMAL = (LocString) "    • All Duplicants currently have tasks";
      public static LocString IDLE = (LocString) "    • One or more Duplicants are idle";

      public static class CRITERIA
      {
        public static LocString CHECKIDLE = (LocString) "Check idle";
        public static LocString CHECKIDLESEVERE = (LocString) "Use high severity idle warning";
      }
    }

    public class CHOREGROUPDIAGNOSTIC
    {
      public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

      public static class CRITERIA
      {
      }
    }

    public class ALLCHORESDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Errands";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Errands</b>";
      public static LocString NORMAL = (LocString) "    • {0} errands pending or in progress";

      public static class CRITERIA
      {
      }
    }

    public class WORKTIMEDIAGNOSTIC
    {
      public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

      public static class CRITERIA
      {
      }
    }

    public class ALLWORKTIMEDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Work Time";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Work Time</b>";
      public static LocString NORMAL = (LocString) "    • {0} of Duplicant time spent working";

      public static class CRITERIA
      {
      }
    }

    public class TRAVEL_TIME
    {
      public static LocString ALL_NAME = (LocString) "Travel Time";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Travel Time</b>";
      public static LocString NORMAL = (LocString) "    • {0} of Duplicant time spent traveling between errands";

      public static class CRITERIA
      {
      }
    }

    public class TRAPPEDDUPLICANTDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Trapped";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Trapped</b>";
      public static LocString NORMAL = (LocString) "    • No Duplicants are trapped";
      public static LocString STUCK = (LocString) "    • One or more Duplicants are trapped";

      public static class CRITERIA
      {
        public static LocString CHECKTRAPPED = (LocString) "Check Trapped";
      }
    }

    public class FLOODEDDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Flooded";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Flooded</b>";
      public static LocString NORMAL = (LocString) "    • No buildings are flooded";
      public static LocString BUILDING_FLOODED = (LocString) "    • One or more buildings are flooded";

      public static class CRITERIA
      {
        public static LocString CHECKFLOODED = (LocString) "Check Flooded";
      }
    }

    public class BREATHABILITYDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Breathability";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Breathability</b>";
      public static LocString NORMAL = (LocString) "    • Oxygen levels are satisfactory";
      public static LocString POOR = (LocString) "    • Oxygen is becoming scarce or low pressure";
      public static LocString SUFFOCATING = (LocString) "    • One or more Duplicants are suffocating";
      public static LocString POOR_BIONIC_TANKS = (LocString) "    • Bionic oxygen tanks are low";
      public static LocString NEAR_OR_EMPTY_BIONIC_TANKS = (LocString) "    • Bionic oxygen tanks are critically low";

      public static class CRITERIA
      {
        public static LocString CHECKSUFFOCATION = (LocString) "Check suffocation";
        public static LocString CHECKLOWBREATHABILITY = (LocString) "Check low breathability";
        public static LocString CHECKLOWBIONICOXYGEN = (LocString) "Check low Bionic Duplicant oxygen tanks";
      }
    }

    public class STRESSDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Max Stress";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Max Stress</b>";
      public static LocString HIGH_STRESS = (LocString) "    • One or more Duplicants is suffering high stress";
      public static LocString NORMAL = (LocString) "    • Duplicants have acceptable stress levels";

      public static class CRITERIA
      {
        public static LocString CHECKSTRESSED = (LocString) "Check stressed";
      }
    }

    public class DECORDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Decor";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Decor</b>";
      public static LocString LOW = (LocString) "    • Decor levels are low";
      public static LocString NORMAL = (LocString) "    • Decor levels are satisfactory";

      public static class CRITERIA
      {
        public static LocString CHECKDECOR = (LocString) "Check decor";
      }
    }

    public class TOILETDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Toilets";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Toilets</b>";
      public static LocString NO_TOILETS = (LocString) "    • Colony has no toilets";
      public static LocString NO_WORKING_TOILETS = (LocString) "    • Colony has no working toilets";
      public static LocString TOILET_URGENT = (LocString) "    • Duplicants urgently need to use a toilet";
      public static LocString FEW_TOILETS = (LocString) "    • Toilet-to-Duplicant ratio is low";
      public static LocString INOPERATIONAL = (LocString) "    • One or more toilets are out of order";
      public static LocString NORMAL = (LocString) "    • Colony has adequate working toilets";
      public static LocString NO_MINIONS_PLANETOID = (LocString) "    • There are no Duplicants with a bladder on this planetoid";
      public static LocString NO_MINIONS_ROCKET = (LocString) "    • There are no Duplicants with a bladder aboard this rocket";

      public static class CRITERIA
      {
        public static LocString CHECKHASANYTOILETS = (LocString) "Check has any toilets";
        public static LocString CHECKENOUGHTOILETS = (LocString) "Check enough toilets";
        public static LocString CHECKBLADDERS = (LocString) "Check Duplicants really need to use the toilet";
      }
    }

    public class BEDDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Beds";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Beds</b>";
      public static LocString NORMAL = (LocString) "    • Colony has adequate bedding";
      public static LocString NOT_ENOUGH_BEDS = (LocString) "    • One or more Duplicants are missing a bed";
      public static LocString MISSING_ASSIGNMENT = (LocString) "    • One or more Duplicants don't have an assigned bed";
      public static LocString CANT_REACH = (LocString) "    • One or more Duplicants can't reach their bed";
      public static LocString NO_MINIONS_PLANETOID = (LocString) "    • There are no Duplicants on this planetoid who need sleep";
      public static LocString NO_MINIONS_ROCKET = (LocString) "    • There are no Duplicants aboard this rocket who need sleep";

      public static class CRITERIA
      {
        public static LocString CHECKENOUGHBEDS = (LocString) "Check enough beds";
        public static LocString CHECKREACHABILITY = (LocString) "Check beds are reachable";
      }
    }

    public class FOODDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Food";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Food</b>";
      public static LocString NORMAL = (LocString) "    • Food supply is currently adequate";
      public static LocString LOW_CALORIES = (LocString) "    • Food-to-Duplicant ratio is low";
      public static LocString HUNGRY = (LocString) "    • One or more Duplicants are very hungry";
      public static LocString NO_FOOD = (LocString) "    • Duplicants have no food";

      public class CRITERIA_HAS_FOOD
      {
        public static LocString PASS = (LocString) "    • Duplicants have food";
        public static LocString FAIL = (LocString) "    • Duplicants have no food";
      }

      public static class CRITERIA
      {
        public static LocString CHECKENOUGHFOOD = (LocString) "Check enough food";
        public static LocString CHECKSTARVATION = (LocString) "Check starvation";
      }
    }

    public class FARMDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Crops";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Crops</b>";
      public static LocString NORMAL = (LocString) "    • Crops are being grown in sufficient quantity";
      public static LocString NONE = (LocString) "    • No farm plots";
      public static LocString NONE_PLANTED = (LocString) "    • No crops planted";
      public static LocString WILTING = (LocString) "    • One or more crops are wilting";
      public static LocString INOPERATIONAL = (LocString) "    • One or more farm plots are inoperable";

      public static class CRITERIA
      {
        public static LocString CHECKHASFARMS = (LocString) "Check colony has farms";
        public static LocString CHECKPLANTED = (LocString) "Check farms are planted";
        public static LocString CHECKWILTING = (LocString) "Check crops wilting";
        public static LocString CHECKOPERATIONAL = (LocString) "Check farm plots operational";
      }
    }

    public class POWERUSEDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Power use";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Power use</b>";
      public static LocString NORMAL = (LocString) "    • Power supply is satisfactory";
      public static LocString OVERLOADED = (LocString) "    • One or more power grids are damaged";
      public static LocString SIGNIFICANT_POWER_CHANGE_DETECTED = (LocString) "Significant power use change detected. (Average:{0}, Current:{1})";
      public static LocString CIRCUIT_OVER_CAPACITY = (LocString) "Circuit overloaded {0}/{1}";

      public static class CRITERIA
      {
        public static LocString CHECKOVERWATTAGE = (LocString) "Check circuit overloaded";
        public static LocString CHECKPOWERUSECHANGE = (LocString) "Check power use change";
      }
    }

    public class HEATDIAGNOSTIC
    {
      public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.ALL_NAME;

      public static class CRITERIA
      {
        public static LocString CHECKHEAT = (LocString) "Check heat";
      }
    }

    public class BATTERYDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Battery";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Battery</b>";
      public static LocString NORMAL = (LocString) "    • All batteries functional";
      public static LocString NONE = (LocString) "    • No batteries are connected to a power grid";
      public static LocString DEAD_BATTERY = (LocString) "    • One or more batteries have died";
      public static LocString LIMITED_CAPACITY = (LocString) "    • Low battery capacity relative to power use";

      public class CRITERIA_CHECK_CAPACITY
      {
        public static LocString PASS = (LocString) "";
        public static LocString FAIL = (LocString) "";
      }

      public static class CRITERIA
      {
        public static LocString CHECKCAPACITY = (LocString) "Check capacity";
        public static LocString CHECKDEAD = (LocString) "Check dead";
      }
    }

    public class RADIATIONDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Radiation";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Radiation</b>";
      public static LocString NORMAL = (LocString) "    • No Radiation concerns";
      public static LocString AVERAGE_RADS = (LocString) "Avg. {0}";

      public class CRITERIA_RADIATION_SICKNESS
      {
        public static LocString PASS = (LocString) "    • No current cases of radiation sickness";
        public static LocString FAIL = (LocString) "    • One or more Duplicants have radiation sickness";
      }

      public class CRITERIA_RADIATION_EXPOSURE
      {
        public static LocString PASS = (LocString) "    • Safe exposure levels";
        public static LocString FAIL_CONCERN = (LocString) "    • Exposure levels are above safe limits for one or more Duplicants";
        public static LocString FAIL_WARNING = (LocString) "    • One or more Duplicants are being exposed to extreme levels of radiation";
      }

      public static class CRITERIA
      {
        public static LocString CHECKSICK = (LocString) "Check sick";
        public static LocString CHECKEXPOSED = (LocString) "Check exposed";
      }
    }

    public class METEORDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Meteor Showers";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Meteor Showers</b>";
      public static LocString NORMAL = (LocString) "    • No meteor showers in progress";
      public static LocString SHOWER_UNDERWAY = (LocString) "    • Meteor bombardment underway! {0} remaining";

      public static class CRITERIA
      {
        public static LocString CHECKUNDERWAY = (LocString) "Check meteor bombardment";
      }
    }

    public class ENTOMBEDDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Entombed";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Entombed</b>";
      public static LocString NORMAL = (LocString) "    • No buildings are entombed";
      public static LocString BUILDING_ENTOMBED = (LocString) "    • One or more buildings are entombed";

      public static class CRITERIA
      {
        public static LocString CHECKENTOMBED = (LocString) "Check entombed";
      }
    }

    public class ROCKETFUELDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Rocket Fuel";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Rocket Fuel</b>";
      public static LocString NORMAL = (LocString) "    • This rocket has sufficient fuel";
      public static LocString WARNING = (LocString) "    • This rocket has no fuel";

      public static class CRITERIA
      {
      }
    }

    public class ROCKETOXIDIZERDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Rocket Oxidizer";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Rocket Oxidizer</b>";
      public static LocString NORMAL = (LocString) "    • This rocket has sufficient oxidizer";
      public static LocString WARNING = (LocString) "    • This rocket has insufficient oxidizer";

      public static class CRITERIA
      {
      }
    }

    public class REACTORDIAGNOSTIC
    {
      public static LocString ALL_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;
      public static LocString TOOLTIP_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;
      public static LocString NORMAL = (LocString) "    • Safe";
      public static LocString CRITERIA_TEMPERATURE_WARNING = (LocString) "    • Temperature dangerously high";
      public static LocString CRITERIA_COOLANT_WARNING = (LocString) "    • Coolant tank low";

      public static class CRITERIA
      {
        public static LocString CHECKTEMPERATURE = (LocString) "Check temperature";
        public static LocString CHECKCOOLANT = (LocString) "Check coolant";
      }
    }

    public class FLOATINGROCKETDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Flight Status";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Flight Status</b>";
      public static LocString NORMAL_FLIGHT = (LocString) "    • This rocket is in flight towards its destination";
      public static LocString NORMAL_UTILITY = (LocString) "    • This rocket is performing a task at its destination";
      public static LocString NORMAL_LANDED = (LocString) $"    • This rocket is currently landed on a {UI.PRE_KEYWORD}Rocket Platform{UI.PST_KEYWORD}";
      public static LocString WARNING_NO_DESTINATION = (LocString) "    • This rocket is suspended in space with no set destination";
      public static LocString WARNING_NO_SPEED = (LocString) "    • This rocket's flight has been halted";

      public static class CRITERIA
      {
      }
    }

    public class ROCKETINORBITDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Rockets in Orbit";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Rockets in Orbit</b>";
      public static LocString NORMAL_ONE_IN_ORBIT = (LocString) "    • {0} is in orbit waiting to land";
      public static LocString NORMAL_IN_ORBIT = (LocString) "    • There are {0} rockets in orbit waiting to land";
      public static LocString WARNING_ONE_ROCKETS_STRANDED = (LocString) $"    • No {(string) BUILDINGS.PREFABS.LAUNCHPAD.NAME} present. {{0}} stranded";
      public static LocString WARNING_ROCKETS_STRANDED = (LocString) $"    • No {(string) BUILDINGS.PREFABS.LAUNCHPAD.NAME} present. {{0}} rockets stranded";
      public static LocString NORMAL_NO_ROCKETS = (LocString) "    • No rockets waiting to land";

      public static class CRITERIA
      {
        public static LocString CHECKORBIT = (LocString) "Check Orbiting Rockets";
      }
    }

    public class BIONICBATTERYDIAGNOSTIC
    {
      public static LocString ALL_NAME = (LocString) "Bionic Power";
      public static LocString TOOLTIP_NAME = (LocString) "<b>Bionic Power</b>";

      public class CRITERIA_BATTERIES
      {
        public static LocString PASS = (LocString) $"    • {UI.FormatAsLink("Power Bank", "ELECTROBANK")} supply is currently adequate";
        public static LocString NO_POWERBANKS = (LocString) $"    • Colony has no {UI.FormatAsLink("Power Banks", "ELECTROBANK")}\n\nBionic Duplicants are at risk of becoming powerless";
        public static LocString LOW_POWERBANKS = (LocString) $"    • {UI.FormatAsLink("Power Bank", "ELECTROBANK")} reserves are low:\n\n<indent=20px>    • {{0}} are currently available</indent>\n<indent=20px>    • {{1}} are being consumed per cycle</indent>";
      }

      public class CRITERIA_POWERLEVEL
      {
        public static LocString CRITICAL_MODE = (LocString) ("    • One or more Duplicants are in desperate need of " + UI.FormatAsLink("Power Banks", "ELECTROBANK"));
        public static LocString POWERLESS = (LocString) ("    • One or more Duplicants are incapacitated and in desperate need of " + UI.FormatAsLink("Power Banks", "ELECTROBANK"));
      }

      public static class CRITERIA
      {
        public static LocString CHECKENOUGHBATTERIES = (LocString) "Check enough power banks";
        public static LocString CHECKPOWERLEVEL = (LocString) "Check critical power level";
      }
    }
  }

  public class SELFCHARGINGBATTERYDIAGNOSTIC
  {
    public static LocString ALL_NAME = ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.NAME;
    public static LocString TOOLTIP_NAME = ITEMS.INDUSTRIAL_PRODUCTS.ELECTROBANK_SELFCHARGING.NAME;
    public static LocString NORMAL = (LocString) "    • Safe";
    public static LocString CRITERIA_BATTERYLIFE_WARNING = (LocString) "    • End-of-lifespan explosion imminent";

    public static class CRITERIA
    {
      public static LocString CHECKSELFCHARGINGBATTERYLIFE = (LocString) "Check lifespan";
    }
  }

  public class TRACKERS
  {
    public static LocString BREATHABILITY = (LocString) "Breathability";
    public static LocString FOOD = (LocString) "Food";
    public static LocString STRESS = (LocString) "Max Stress";
    public static LocString IDLE = (LocString) "Idle Duplicants";
  }

  public class CONTROLS
  {
    public static LocString PRESS = (LocString) "Press";
    public static LocString PRESSLOWER = (LocString) "press";
    public static LocString PRESSUPPER = (LocString) nameof (PRESS);
    public static LocString PRESSING = (LocString) "Pressing";
    public static LocString PRESSINGLOWER = (LocString) "pressing";
    public static LocString PRESSINGUPPER = (LocString) nameof (PRESSING);
    public static LocString PRESSED = (LocString) "Pressed";
    public static LocString PRESSEDLOWER = (LocString) "pressed";
    public static LocString PRESSEDUPPER = (LocString) nameof (PRESSED);
    public static LocString PRESSES = (LocString) "Presses";
    public static LocString PRESSESLOWER = (LocString) "presses";
    public static LocString PRESSESUPPER = (LocString) nameof (PRESSES);
    public static LocString PRESSABLE = (LocString) "Pressable";
    public static LocString PRESSABLELOWER = (LocString) "pressable";
    public static LocString PRESSABLEUPPER = (LocString) nameof (PRESSABLE);
    public static LocString CLICK = (LocString) "Click";
    public static LocString CLICKLOWER = (LocString) "click";
    public static LocString CLICKUPPER = (LocString) nameof (CLICK);
    public static LocString CLICKING = (LocString) "Clicking";
    public static LocString CLICKINGLOWER = (LocString) "clicking";
    public static LocString CLICKINGUPPER = (LocString) nameof (CLICKING);
    public static LocString CLICKED = (LocString) "Clicked";
    public static LocString CLICKEDLOWER = (LocString) "clicked";
    public static LocString CLICKEDUPPER = (LocString) nameof (CLICKED);
    public static LocString CLICKS = (LocString) "Clicks";
    public static LocString CLICKSLOWER = (LocString) "clicks";
    public static LocString CLICKSUPPER = (LocString) nameof (CLICKS);
    public static LocString CLICKABLE = (LocString) "Clickable";
    public static LocString CLICKABLELOWER = (LocString) "clickable";
    public static LocString CLICKABLEUPPER = (LocString) nameof (CLICKABLE);
  }

  public class MATH_PICTURES
  {
    public class AXIS_LABELS
    {
      public static LocString CYCLES = (LocString) "Cycles";
    }
  }

  public class SPACEDESTINATIONS
  {
    public class WORMHOLE
    {
      public static LocString NAME = (LocString) "Temporal Tear";
      public static LocString DESCRIPTION = (LocString) "The source of our misfortune, though it may also be our shot at freedom. Traces of Neutronium are detectable in my readings.";
    }

    public class RESEARCHDESTINATION
    {
      public static LocString NAME = (LocString) "Alluring Anomaly";
      public static LocString DESCRIPTION = (LocString) "Our researchers would have a field day with this if they could only get close enough.";
    }

    public class DEBRIS
    {
      public class SATELLITE
      {
        public static LocString NAME = (LocString) "Satellite";
        public static LocString DESCRIPTION = (LocString) "An artificial construct that has escaped its orbit. It no longer appears to be monitored.";
      }
    }

    public class NONE
    {
      public static LocString NAME = (LocString) "Unselected";
    }

    public class ORBIT
    {
      public static LocString NAME_FMT = (LocString) "Orbiting {Name}";
    }

    public class EMPTY_SPACE
    {
      public static LocString NAME = (LocString) "Empty Space";
    }

    public class FOG_OF_WAR_SPACE
    {
      public static LocString NAME = (LocString) "Unexplored Space";
    }

    public class ARTIFACT_POI
    {
      public class GRAVITASSPACESTATION1
      {
        public static LocString NAME = (LocString) "Destroyed Satellite";
        public static LocString DESC = (LocString) $"The remnants of a bygone era, lost in time.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class GRAVITASSPACESTATION2
      {
        public static LocString NAME = (LocString) "Demolished Rocket";
        public static LocString DESC = (LocString) $"A defunct rocket from a corporation that vanished long ago.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class GRAVITASSPACESTATION3
      {
        public static LocString NAME = (LocString) "Ruined Rocket";
        public static LocString DESC = (LocString) $"The ruins of a rocket that stopped functioning ages ago.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class GRAVITASSPACESTATION4
      {
        public static LocString NAME = (LocString) "Retired Planetary Excursion Module";
        public static LocString DESC = (LocString) $"A rocket part from a society that has been wiped out.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class GRAVITASSPACESTATION5
      {
        public static LocString NAME = (LocString) "Destroyed Satellite";
        public static LocString DESC = (LocString) $"A destroyed Gravitas satellite.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class GRAVITASSPACESTATION6
      {
        public static LocString NAME = (LocString) "Annihilated Satellite";
        public static LocString DESC = (LocString) $"The remains of a satellite made some time in the past.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class GRAVITASSPACESTATION7
      {
        public static LocString NAME = (LocString) "Wrecked Space Shuttle";
        public static LocString DESC = (LocString) $"A defunct space shuttle that floats through space unattended.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class GRAVITASSPACESTATION8
      {
        public static LocString NAME = (LocString) "Obsolete Space Station Module";
        public static LocString DESC = (LocString) $"The module from a space station that ceased to exist ages ago.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class RUSSELLSTEAPOT
      {
        public static LocString NAME = (LocString) "Russell's Teapot";
        public static LocString DESC = (LocString) $"Has never been disproven to not exist.\n\n{UI.FormatAsLink("Gathering", "EXOBASESDLC1")} space junk requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }
    }

    public class HARVESTABLE_POI
    {
      public static LocString POI_PRODUCTION = (LocString) "{0}";
      public static LocString POI_PRODUCTION_TOOLTIP = (LocString) "{0}";

      public class CARBONASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Carbon Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid containing {UI.FormatAsLink("Refined Carbon", "REFINEDCARBON")} and {UI.FormatAsLink("Coal", "CARBON")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class METALLICASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Metallic Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Iron", "IRON")}, {UI.FormatAsLink("Copper", "COPPER")}, {UI.FormatAsLink("Lead", "LEAD")}, and {UI.FormatAsLink("Obsidian", "OBSIDIAN")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class SATELLITEFIELD
      {
        public static LocString NAME = (LocString) "Space Debris";
        public static LocString DESC = (LocString) $"Space junk from a forgotten age.\n\n{UI.FormatAsLink("Collecting artifacts", "EXOBASESDLC1")} requires a rocket equipped with an {(string) BUILDINGS.PREFABS.ARTIFACTCARGOBAY.NAME}.";
      }

      public class ROCKYASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Rocky Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Granite", "GRANITE")}, {UI.FormatAsLink("Abyssalite", "KATAIRITE")}, {UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK")}, and {UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class INTERSTELLARICEFIELD
      {
        public static LocString NAME = (LocString) "Ice Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Ice", "ICE")}, {UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE")} and {UI.FormatAsLink("Oxygen", "OXYGEN")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class ORGANICMASSFIELD
      {
        public static LocString NAME = (LocString) "Organic Mass Field";
        public static LocString DESC = (LocString) $"A mass of harvestable resources containing {UI.FormatAsLink("Algae", "ALGAE")}, {UI.FormatAsLink("Slime", "SLIMEMOLD")}, {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} and {UI.FormatAsLink("Dirt", "DIRT")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class ICEASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Exploded Ice Giant";
        public static LocString DESC = (LocString) $"A cloud of planetary remains containing {UI.FormatAsLink("Ice", "ICE")}, {UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE")}, {UI.FormatAsLink("Oxygen", "OXYGEN")} and {UI.FormatAsLink("Natural Gas", "METHANE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class GASGIANTCLOUD
      {
        public static LocString NAME = (LocString) "Exploded Gas Giant";
        public static LocString DESC = (LocString) $"The harvestable remains of a planet containing {UI.FormatAsLink("Hydrogen", "HYDROGEN")} in {UI.FormatAsLink("gas", "ELEMENTS_GAS")} form, and {UI.FormatAsLink("Methane", "SOLIDMETHANE")} in {UI.FormatAsLink("solid", "ELEMENTS_SOLID")} and {UI.FormatAsLink("liquid", "ELEMENTS_LIQUID")} form.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class CHLORINECLOUD
      {
        public static LocString NAME = (LocString) "Chlorine Cloud";
        public static LocString DESC = (LocString) $"A cloud of harvestable debris containing {UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS")} and {UI.FormatAsLink("Bleach Stone", "BLEACHSTONE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class GILDEDASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Gilded Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Gold", "GOLD")}, {UI.FormatAsLink("Fullerene", "FULLERENE")}, {UI.FormatAsLink("Regolith", "REGOLITH")} and more.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class GLIMMERINGASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Glimmering Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Tungsten", "TUNGSTEN")}, {UI.FormatAsLink("Wolframite", "WOLFRAMITE")} and more.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class HELIUMCLOUD
      {
        public static LocString NAME = (LocString) "Helium Cloud";
        public static LocString DESC = (LocString) $"A cloud of resources containing {UI.FormatAsLink("Water", "WATER")} and {UI.FormatAsLink("Hydrogen Gas", "HYDROGEN")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class OILYASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Oily Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Solid Methane", "SOLIDMETHANE")}, {UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE")} and {UI.FormatAsLink("Crude Oil", "CRUDEOIL")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class OXIDIZEDASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Oxidized Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE")} and {UI.FormatAsLink("Rust", "RUST")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class SALTYASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Salty Asteroid Field";
        public static LocString DESC = (LocString) $"A field of harvestable resources containing {UI.FormatAsLink("Salt Water", "SALTWATER")},{UI.FormatAsLink("Brine", "BRINE")} and {UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class FROZENOREFIELD
      {
        public static LocString NAME = (LocString) "Frozen Ore Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Polluted Ice", "DIRTYICE")}, {UI.FormatAsLink("Ice", "ICE")}, {UI.FormatAsLink("Snow", "SNOW")} and {UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class FORESTYOREFIELD
      {
        public static LocString NAME = (LocString) "Forested Ore Field";
        public static LocString DESC = (LocString) $"A field of harvestable resources containing {UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE")}, {UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK")} and {UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class SWAMPYOREFIELD
      {
        public static LocString NAME = (LocString) "Swampy Ore Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Mud", "MUD")}, {UI.FormatAsLink("Polluted Dirt", "TOXICSAND")} and {UI.FormatAsLink("Cobalt Ore", "COBALTITE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class SANDYOREFIELD
      {
        public static LocString NAME = (LocString) "Sandy Ore Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Sandstone", "SANDSTONE")}, {UI.FormatAsLink("Algae", "ALGAE")}, {UI.FormatAsLink("Copper Ore", "CUPRITE")} and {UI.FormatAsLink("Sand", "SAND")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class RADIOACTIVEGASCLOUD
      {
        public static LocString NAME = (LocString) "Radioactive Gas Cloud";
        public static LocString DESC = (LocString) $"A cloud of resources containing {UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS")}, {UI.FormatAsLink("Uranium Ore", "URANIUMORE")} and {UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class RADIOACTIVEASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Radioactive Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Bleach Stone", "BLEACHSTONE")}, {UI.FormatAsLink("Rust", "RUST")}, {UI.FormatAsLink("Uranium Ore", "URANIUMORE")} and {UI.FormatAsLink("Sulfur", "SULFUR")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class OXYGENRICHASTEROIDFIELD
      {
        public static LocString NAME = (LocString) "Oxygen Rich Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Ice", "ICE")}, {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} and {UI.FormatAsLink("Water", "WATER")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class INTERSTELLAROCEAN
      {
        public static LocString NAME = (LocString) "Interstellar Ocean";
        public static LocString DESC = (LocString) $"An interplanetary body that consists of {UI.FormatAsLink("Salt Water", "SALTWATER")}, {UI.FormatAsLink("Brine", "BRINE")}, {UI.FormatAsLink("Salt", "SALT")} and {UI.FormatAsLink("Ice", "ICE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC2CERESFIELD
      {
        public static LocString NAME = (LocString) "Frozen Cinnabar Asteroid Field";
        public static LocString DESC = (LocString) $"The harvestable remains of a planet containing {UI.FormatAsLink("Cinnabar", "Cinnabar")}, {UI.FormatAsLink("Ice", "ICE")} and {UI.FormatAsLink("Mercury", "MERCURY")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC2CERESOREFIELD
      {
        public static LocString NAME = (LocString) "Frozen Mercury Asteroid Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Cinnabar", "Cinnabar")}, {UI.FormatAsLink("Ice", "ICE")} and {UI.FormatAsLink("Mercury", "MERCURY")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC4PREHISTORICOREFIELD
      {
        public static LocString NAME = (LocString) "Amber Field";
        public static LocString DESC = (LocString) $"The harvestable remains of a planet containing {UI.FormatAsLink("Nickel Ore", "NICKELORE")}, {UI.FormatAsLink("Peat", "PEAT")}, {UI.FormatAsLink("Amber", "AMBER")}, and {UI.FormatAsLink("Shale", "SHALE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC4PREHISTORICMIXINGFIELD
      {
        public static LocString NAME = (LocString) "Conductive Ore Field";
        public static LocString DESC = (LocString) $"An asteroid field containing {UI.FormatAsLink("Nickel Ore", "NICKELORE")}, {UI.FormatAsLink("Peat", "PEAT")}, {UI.FormatAsLink("Amber", "AMBER")}, and {UI.FormatAsLink("Shale", "SHALE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC4IMPACTORDEBRISFIELD1
      {
        public static LocString NAME = (LocString) "Demolior Debris";
        public static LocString DESC = (LocString) $"The solid harvestable remains of Demolior, containing {UI.FormatAsLink("Iridium", "IRIDIUM")}, {UI.FormatAsLink("Mafic Rock", "MAFICROCK")}, {UI.FormatAsLink("Gold", "GOLD")}, and {UI.FormatAsLink("Granite", "GRANITE")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC4IMPACTORDEBRISFIELD2
      {
        public static LocString NAME = (LocString) "Liquid Demolior Debris";
        public static LocString DESC = (LocString) $"The harvestable liquid remains of Demolior, containing {UI.FormatAsLink("Isosap", "ISORESIN")}, {UI.FormatAsLink("Petroleum", "PETROLEUM")}, and {UI.FormatAsLink("Liquid Sulfur", "LIQUIDSULFUR")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC4IMPACTORDEBRISFIELD3
      {
        public static LocString NAME = (LocString) "Molten Demolior Debris";
        public static LocString DESC = (LocString) $"The harvestable molten remains of Demolior, containing {UI.FormatAsLink("Molten Iridium", "MOLTENIRIDIUM")}, {UI.FormatAsLink("Magma", "MAGMA")}, {UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN")}, and {UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC5AQUATICMIXINGFIELD
      {
        public static LocString NAME = (LocString) "Zinc Ore Field";
        public static LocString DESC = (LocString) $"The harvestable remains of a planet containing {UI.FormatAsLink("Zinc Ore", "ZINCORE")}, {UI.FormatAsLink("Silt Stone", "SILTSTONE")}, {UI.FormatAsLink("Coquina", "COQUINA")}, {UI.FormatAsLink("Corallium", "CORALLIUM")}, and {UI.FormatAsLink("Basalt", "BASALT")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }

      public class DLC5AQUATICOREFIELD
      {
        public static LocString NAME = (LocString) "Zinc Ore Field";
        public static LocString DESC = (LocString) $"The harvestable remains of a planet containing {UI.FormatAsLink("Zinc Ore", "ZINCORE")}, {UI.FormatAsLink("Siltstone", "SILTSTONE")}, {UI.FormatAsLink("Coquina", "COQUINA")}, {UI.FormatAsLink("Corallium", "CORALLIUM")}, and {UI.FormatAsLink("Basalt", "BASALT")}.\n\n{UI.FormatAsLink("Space mining", "EXOBASESDLC1")} requires a rocket equipped with a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} and a cargo module.";
      }
    }

    public class GRAVITAS_SPACE_POI
    {
      public static LocString STATION = (LocString) "Destroyed Gravitas Space Station";
    }

    public class TELESCOPE_TARGET
    {
      public static LocString NAME = (LocString) "Telescope Target";
    }

    public class ASTEROIDS
    {
      public class ROCKYASTEROID
      {
        public static LocString NAME = (LocString) "Rocky Asteroid";
        public static LocString DESCRIPTION = (LocString) "A minor mineral planet. Unlike a comet, it does not possess a tail.";
      }

      public class METALLICASTEROID
      {
        public static LocString NAME = (LocString) "Metallic Asteroid";
        public static LocString DESCRIPTION = (LocString) "A shimmering conglomerate of various metals.";
      }

      public class CARBONACEOUSASTEROID
      {
        public static LocString NAME = (LocString) "Carbon Asteroid";
        public static LocString DESCRIPTION = (LocString) "A common asteroid containing several useful resources.";
      }

      public class OILYASTEROID
      {
        public static LocString NAME = (LocString) "Oily Asteroid";
        public static LocString DESCRIPTION = (LocString) "A viscous asteroid that is only loosely held together. Contains fossil fuel resources.";
      }

      public class GOLDASTEROID
      {
        public static LocString NAME = (LocString) "Gilded Asteroid";
        public static LocString DESCRIPTION = (LocString) "A rich asteroid with thin gold coating and veins of gold deposits throughout.";
      }
    }

    public class CLUSTERMAPMETEORSHOWERS
    {
      public class UNIDENTIFIED
      {
        public static LocString NAME = (LocString) "Unidentified Object";
        public static LocString DESCRIPTION = (LocString) $"A cosmic anomaly is traveling through the galaxy.\n\nIts origins and purpose are currently unknown, though a {(string) BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME} could change that.";
      }

      public class SLIME
      {
        public static LocString NAME = (LocString) "Slimy Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A shower of slimy, biodynamic meteors on a collision course with the surface of an asteroid.";
      }

      public class SNOW
      {
        public static LocString NAME = (LocString) "Blizzard Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A shower of cold, cold meteors on a collision course with the surface of an asteroid.";
      }

      public class ICE
      {
        public static LocString NAME = (LocString) "Ice Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A hailstorm of icy space rocks on a collision course with the surface of an asteroid.";
      }

      public class ICEANDTREES
      {
        public static LocString NAME = (LocString) "Icy Nectar Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A hailstorm of sweet, icy space rocks on a collision course with the surface of an asteroid";
      }

      public class COPPER
      {
        public static LocString NAME = (LocString) "Copper Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A shower of metallic meteors on a collision course with the surface of an asteroid.";
      }

      public class IRON
      {
        public static LocString NAME = (LocString) "Iron Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A shower of metallic space rocks on a collision course with the surface of an asteroid.";
      }

      public class GOLD
      {
        public static LocString NAME = (LocString) "Gold Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A shower of shiny metallic space rocks on a collision course with the surface of an asteroid.";
      }

      public class URANIUM
      {
        public static LocString NAME = (LocString) "Uranium Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A toxic shower of radioactive meteors on a collision course with the surface of an asteroid.";
      }

      public class LIGHTDUST
      {
        public static LocString NAME = (LocString) "Dust Fluff Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A cloud-like shower of dust fluff meteors heading towards the surface of an asteroid.";
      }

      public class HEAVYDUST
      {
        public static LocString NAME = (LocString) "Dense Dust Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A dark cloud of heavy dust meteors heading towards the surface of an asteroid.";
      }

      public class REGOLITH
      {
        public static LocString NAME = (LocString) "Regolith Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A shower of rocky meteors on a collision course with the surface of an asteroid.";
      }

      public class OXYLITE
      {
        public static LocString NAME = (LocString) "Oxylite Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A shower of rocky, oxygen-rich meteors on a collision course with the surface of an asteroid.";
      }

      public class BLEACHSTONE
      {
        public static LocString NAME = (LocString) "Bleach Stone Meteor Shower";
        public static LocString DESCRIPTION = (LocString) "A shower of bleach stone meteors on a collision course with the surface of an asteroid.";
      }

      public class MOO
      {
        public static LocString NAME = (LocString) "Gassy Mooteor Shower";
        public static LocString DESCRIPTION = (LocString) "A herd of methane-infused meteors that cause a bit of a stink, but do no actual damage.";
      }
    }

    public class CLUSTERMAPMETEORS
    {
      public class COPPER
      {
        public static LocString NAME = (LocString) "Copper Meteor";
        public static LocString DESCRIPTION = (LocString) "A shower of metallic meteors on a collision course with the surface of an asteroid.";
      }

      public class IRON
      {
        public static LocString NAME = (LocString) "Iron Meteor";
        public static LocString DESCRIPTION = (LocString) "A shower of metallic space rocks on a collision course with the surface of an asteroid.";
      }

      public class LARGEIMACTOR
      {
        public static LocString NAME = (LocString) "Demolior";
        public static LocString DESCRIPTION = (LocString) "An ancient impactor asteroid on a collision course with the surface of this world.";
      }
    }

    public class COMETS
    {
      public class ROCKCOMET
      {
        public static LocString NAME = (LocString) "Rock Meteor";
      }

      public class DUSTCOMET
      {
        public static LocString NAME = (LocString) "Dust Meteor";
      }

      public class IRONCOMET
      {
        public static LocString NAME = (LocString) "Iron Meteor";
      }

      public class COPPERCOMET
      {
        public static LocString NAME = (LocString) "Copper Meteor";
      }

      public class GOLDCOMET
      {
        public static LocString NAME = (LocString) "Gold Meteor";
      }

      public class FULLERENECOMET
      {
        public static LocString NAME = (LocString) "Fullerene Meteor";
      }

      public class URANIUMORECOMET
      {
        public static LocString NAME = (LocString) "Uranium Meteor";
      }

      public class NUCLEAR_WASTE
      {
        public static LocString NAME = (LocString) "Radioactive Meteor";
      }

      public class SATELLITE
      {
        public static LocString NAME = (LocString) "Defunct Satellite";
      }

      public class FOODCOMET
      {
        public static LocString NAME = (LocString) "Snack Bomb";
      }

      public class GASSYMOOCOMET
      {
        public static LocString NAME = (LocString) "Gassy Mooteor";
      }

      public class DIESELMOOCOMET
      {
        public static LocString NAME = (LocString) "Husky Mooteor";
      }

      public class SLIMECOMET
      {
        public static LocString NAME = (LocString) "Slime Meteor";
      }

      public class SNOWBALLCOMET
      {
        public static LocString NAME = (LocString) "Snow Meteor";
      }

      public class SPACETREESEEDCOMET
      {
        public static LocString NAME = (LocString) "Bonbon Meteor";
      }

      public class IRIDIUMCOMET
      {
        public static LocString NAME = (LocString) "Iridium Meteor";
      }

      public class HARDICECOMET
      {
        public static LocString NAME = (LocString) "Ice Meteor";
      }

      public class LIGHTDUSTCOMET
      {
        public static LocString NAME = (LocString) "Dust Fluff Meteor";
      }

      public class ALGAECOMET
      {
        public static LocString NAME = (LocString) "Algae Meteor";
      }

      public class PHOSPHORICCOMET
      {
        public static LocString NAME = (LocString) "Phosphoric Meteor";
      }

      public class OXYLITECOMET
      {
        public static LocString NAME = (LocString) "Oxylite Meteor";
      }

      public class BLEACHSTONECOMET
      {
        public static LocString NAME = (LocString) "Bleach Stone Meteor";
      }

      public class MINICOMET
      {
        public static LocString NAME = (LocString) "Debris Projectile";
      }
    }

    public class DWARFPLANETS
    {
      public class ICYDWARF
      {
        public static LocString NAME = (LocString) "Interstellar Ice";
        public static LocString DESCRIPTION = (LocString) "A terrestrial destination, frozen completely solid.";
      }

      public class ORGANICDWARF
      {
        public static LocString NAME = (LocString) "Organic Mass";
        public static LocString DESCRIPTION = (LocString) "A mass of organic material similar to the ooze used to print Duplicants. This sample is heavily degraded.";
      }

      public class DUSTYDWARF
      {
        public static LocString NAME = (LocString) "Dusty Dwarf";
        public static LocString DESCRIPTION = (LocString) "A loosely held together composite of minerals.";
      }

      public class SALTDWARF
      {
        public static LocString NAME = (LocString) "Salty Dwarf";
        public static LocString DESCRIPTION = (LocString) "A dwarf planet with unusually high sodium concentrations.";
      }

      public class REDDWARF
      {
        public static LocString NAME = (LocString) "Red Dwarf";
        public static LocString DESCRIPTION = (LocString) "An M-class star orbited by clusters of extractable aluminum and methane.";
      }
    }

    public class PLANETS
    {
      public class TERRAPLANET
      {
        public static LocString NAME = (LocString) "Terrestrial Planet";
        public static LocString DESCRIPTION = (LocString) "A planet with a walkable surface, though it does not possess the resources to sustain long-term life.";
      }

      public class VOLCANOPLANET
      {
        public static LocString NAME = (LocString) "Volcanic Planet";
        public static LocString DESCRIPTION = (LocString) "A large terrestrial object composed mainly of molten rock.";
      }

      public class SHATTEREDPLANET
      {
        public static LocString NAME = (LocString) "Shattered Planet";
        public static LocString DESCRIPTION = (LocString) "A once-habitable planet that has sustained massive damage.\n\nA powerful containment field prevents our rockets from traveling to its surface.";
      }

      public class RUSTPLANET
      {
        public static LocString NAME = (LocString) "Oxidized Asteroid";
        public static LocString DESCRIPTION = (LocString) "A small planet covered in large swathes of brown rust.";
      }

      public class FORESTPLANET
      {
        public static LocString NAME = (LocString) "Living Planet";
        public static LocString DESCRIPTION = (LocString) "A small green planet displaying several markers of primitive life.";
      }

      public class SHINYPLANET
      {
        public static LocString NAME = (LocString) "Glimmering Planet";
        public static LocString DESCRIPTION = (LocString) "A planet composed of rare, shimmering minerals. From the distance, it looks like gem in the sky.";
      }

      public class CHLORINEPLANET
      {
        public static LocString NAME = (LocString) "Chlorine Planet";
        public static LocString DESCRIPTION = (LocString) "A noxious planet permeated by unbreathable chlorine.";
      }

      public class SALTDESERTPLANET
      {
        public static LocString NAME = (LocString) "Arid Planet";
        public static LocString DESCRIPTION = (LocString) "A sweltering, desert-like planet covered in surface salt deposits.";
      }

      public class DLC2CERESSPACEDESTINATION
      {
        public static LocString NAME = (LocString) "Ceres";
        public static LocString DESCRIPTION = (LocString) "A frozen planet peppered with cinnabar deposits.";
      }

      public class DLC4PREHISTORICSPACEDESTINATION
      {
        public static LocString NAME = (LocString) "Prehistoric Ore Field";
        public static LocString DESCRIPTION = (LocString) "A destination with extractable resources from another era.";
      }

      public class DLC4PREHISTORICDEMOLIORSPACEDESTINATION
      {
        public static LocString NAME = (LocString) "Demolior Debris";
        public static LocString DESCRIPTION = (LocString) "The remains of an obliterated asteroid containing a renewable source of iridium.";
      }

      public class DLC4PREHISTORICDEMOLIORSPACEDESTINATION2
      {
        public static LocString NAME = (LocString) "Liquid Demolior Debris";
        public static LocString DESCRIPTION = (LocString) "The liquid remains of an obliterated asteroid containing a renewable source of isosap.";
      }

      public class DLC4PREHISTORICDEMOLIORSPACEDESTINATION3
      {
        public static LocString NAME = (LocString) "Molten Demolior Debris";
        public static LocString DESCRIPTION = (LocString) "The hot metallic remains of an obliterated asteroid containing a renewable source of iridium.";
      }

      public class DLC5AQUATICSPACEDESCRIPTION
      {
        public static LocString NAME = (LocString) "Extinct Ocean";
        public static LocString DESCRIPTION = (LocString) "The remains of an aquatic world primarily composed of harvestable corallium, coquina, and zinc ore.";
      }
    }

    public class GIANTS
    {
      public class GASGIANT
      {
        public static LocString NAME = (LocString) "Gas Giant";
        public static LocString DESCRIPTION = (LocString) $"A massive volume of {UI.FormatAsLink("Hydrogen Gas", "HYDROGEN")} formed around a small solid center.";
      }

      public class ICEGIANT
      {
        public static LocString NAME = (LocString) "Ice Giant";
        public static LocString DESCRIPTION = (LocString) $"A massive volume of frozen material, primarily composed of {UI.FormatAsLink("Ice", "ICE")}.";
      }

      public class HYDROGENGIANT
      {
        public static LocString NAME = (LocString) "Helium Giant";
        public static LocString DESCRIPTION = (LocString) $"A massive volume of {UI.FormatAsLink("Helium", "HELIUM")} formed around a small solid center.";
      }
    }
  }

  public class SPACEARTIFACTS
  {
    public class ARTIFACTTIERS
    {
      public static LocString TIER_NONE = (LocString) "Nothing";
      public static LocString TIER0 = (LocString) "Rarity 0";
      public static LocString TIER1 = (LocString) "Rarity 1";
      public static LocString TIER2 = (LocString) "Rarity 2";
      public static LocString TIER3 = (LocString) "Rarity 3";
      public static LocString TIER4 = (LocString) "Rarity 4";
      public static LocString TIER5 = (LocString) "Rarity 5";
    }

    public class PACUPERCOLATOR
    {
      public static LocString NAME = (LocString) "Percolator";
      public static LocString DESCRIPTION = (LocString) "Don't drink from it! There was a pacu... IN the percolator!";
      public static LocString ARTIFACT = (LocString) "A coffee percolator with the remnants of a blend of coffee that was a personal favorite of Dr. Hassan Aydem.\n\nHe would specifically reserve the consumption of this particular blend for when he was reviewing research papers on Sunday afternoons.";
    }

    public class ROBOTARM
    {
      public static LocString NAME = (LocString) "Robot Arm";
      public static LocString DESCRIPTION = (LocString) "It's not functional. Just cool.";
      public static LocString ARTIFACT = (LocString) "A commercially available robot arm that has had a significant amount of modifications made to it.\n\nThe initials B.A. appear on one of the fingers.";
    }

    public class HATCHFOSSIL
    {
      public static LocString NAME = (LocString) "Pristine Fossil";
      public static LocString DESCRIPTION = (LocString) "The preserved bones of an early species of Hatch.";
      public static LocString ARTIFACT = (LocString) "The preservation of this skeleton occurred artificially using a technique called the \"The Ali Method\".\n\nIt should be noted that this fossilization technique was pioneered by one Dr. Ashkan Seyed Ali, an employee of Gravitas.";
    }

    public class MODERNART
    {
      public static LocString NAME = (LocString) "Modern Art";
      public static LocString DESCRIPTION = (LocString) "I don't get it.";
      public static LocString ARTIFACT = (LocString) "A sculpture of the Neoplastism movement of Modern Art.\n\nGravitas records show that this piece was once used in a presentation called 'Form and Function in Corporate Aesthetic'.";
    }

    public class EGGROCK
    {
      public static LocString NAME = (LocString) "Egg-Shaped Rock";
      public static LocString DESCRIPTION = (LocString) "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.";
      public static LocString ARTIFACT = (LocString) "The words \"Happy Farters Day Dad. Love Macy\" appear on the bottom of this rock, written in a childlish scrawl.";
    }

    public class RAINBOWEGGROCK
    {
      public static LocString NAME = (LocString) "Egg-Shaped Rock";
      public static LocString DESCRIPTION = (LocString) "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.\n\nThis one is rainbow colored.";
      public static LocString ARTIFACT = (LocString) "The words \"Happy Father's Day, Dad. Love you!\" appear on the bottom of this rock, written in very neat handwriting. The words are surrounded by four hearts drawn in what appears to be a pink gel pen.";
    }

    public class OKAYXRAY
    {
      public static LocString NAME = (LocString) "Old X-Ray";
      public static LocString DESCRIPTION = (LocString) "Ew, weird. It has five fingers!";
      public static LocString ARTIFACT = (LocString) "The description on this X-ray indicates that it was taken in the Gravitas Medical Facility.\n\nMost likely this X-ray was performed while investigating an injury that occurred within the facility.";
    }

    public class SHIELDGENERATOR
    {
      public static LocString NAME = (LocString) "Shield Generator";
      public static LocString DESCRIPTION = (LocString) "A mechanical prototype capable of producing a small section of shielding.";
      public static LocString ARTIFACT = (LocString) "The energy field produced by this shield generator completely ignores those light behaviors which are wave-like and focuses instead on its particle behaviors.\n\nThis seemingly paradoxical state is possible when light is slowed down to the point at which it stops entirely.";
    }

    public class TEAPOT
    {
      public static LocString NAME = (LocString) "Encrusted Teapot";
      public static LocString DESCRIPTION = (LocString) "A teapot from the depths of space, coated in a thick layer of Neutronium.";
      public static LocString ARTIFACT = (LocString) "The amount of Neutronium present in this teapot suggests that it has crossed the threshold of the spacetime continuum on countless occasions, floating through many multiple universes over a plethora of times and spaces.\n\nThough there are, theoretically, an infinite amount of outcomes to any one event over many multi-verses, the homogeneity of the still relatively young multiverse suggests that this is then not the only teapot which has crossed into multiple universes. Despite the infinite possible outcomes of infinite multiverses it appears one high probability constant is that there is, or once was, a teapot floating somewhere in space within every universe.";
    }

    public class DNAMODEL
    {
      public static LocString NAME = (LocString) "Double Helix Model";
      public static LocString DESCRIPTION = (LocString) "An educational model of genetic information.";
      public static LocString ARTIFACT = (LocString) "A physical representation of the building blocks of life.\n\nThis one contains trace amounts of a Genetic Ooze prototype that was once used by Gravitas.";
    }

    public class SANDSTONE
    {
      public static LocString NAME = (LocString) "Sandstone";
      public static LocString DESCRIPTION = (LocString) "A beautiful rock composed of multiple layers of sediment.";
      public static LocString ARTIFACT = (LocString) "This sample of sandstone appears to have been processed by the Gravitas Mining Gun that was made available to the general public.\n\nNote: The Gravitas public Mining Gun model is different than ones used by Duplicants in its larger size, and extra precautionary features added in order to be compliant with national safety standards.";
    }

    public class MAGMALAMP
    {
      public static LocString NAME = (LocString) "Magma Lamp";
      public static LocString DESCRIPTION = (LocString) "The sequel to \"Lava Lamp\".";
      public static LocString ARTIFACT = (LocString) "Molten lava and obsidian combined in a way that allows the lava to maintain just enough heat to remain in liquid form.\n\nPlans of this lamp found in the Gravitas archives have been attributed to one Robin Nisbet, PhD.";
    }

    public class OBELISK
    {
      public static LocString NAME = (LocString) "Small Obelisk";
      public static LocString DESCRIPTION = (LocString) "A rectangular stone piece.\n\nIts function is unclear.";
      public static LocString ARTIFACT = (LocString) "On close inspection this rectangle is actually a stone box built with a covert, almost seamless, lid, housing a tiny key.\n\nIt is still unclear what the key unlocks.";
    }

    public class RUBIKSCUBE
    {
      public static LocString NAME = (LocString) "Rubik's Cube";
      public static LocString DESCRIPTION = (LocString) "This mystery of the universe has already been solved.";
      public static LocString ARTIFACT = (LocString) "A well-used, competition-compliant version of the popular puzzle cube.\n\nIt's worth noting that Dr. Dylan 'Nails' Winslow was once a regional Rubik's Cube champion.";
    }

    public class OFFICEMUG
    {
      public static LocString NAME = (LocString) "Office Mug";
      public static LocString DESCRIPTION = (LocString) "An intermediary place to store espresso before you move it to your mouth.";
      public static LocString ARTIFACT = (LocString) "An office mug with the Gravitas logo on it. Though their office mugs were all emblazoned with the same logo, Gravitas colored their mugs differently to distinguish between their various departments.\n\nThis one is from the AI department.";
    }

    public class AMELIASWATCH
    {
      public static LocString NAME = (LocString) "Wrist Watch";
      public static LocString DESCRIPTION = (LocString) "It was discovered in a package labeled \"To be entrusted to Dr. Walker\".";
      public static LocString ARTIFACT = (LocString) "This watch once belonged to pioneering aviator Amelia Earhart and travelled to space via astronaut Dr. Shannon Walker.\n\nHow it came to be floating in space is a matter of speculation, but perhaps the adventurous spirit of its original stewards became infused within the fabric of this timepiece and compelled the universe to launch it into the great unknown.";
    }

    public class MOONMOONMOON
    {
      public static LocString NAME = (LocString) "Moonmoonmoon";
      public static LocString DESCRIPTION = (LocString) "A moon's moon's moon. It's very small.";
      public static LocString ARTIFACT = (LocString) "In contrast to most moons, this object's glowing properties do not come from reflecting an external source of light, but rather from an internal glow of mysterious origin.\n\nThe glow of this object also grants an extraordinary amount of Decor bonus to nearby Duplicants, almost as if it was designed that way.";
    }

    public class BIOLUMINESCENTROCK
    {
      public static LocString NAME = (LocString) "Bioluminescent Rock";
      public static LocString DESCRIPTION = (LocString) "A thriving colony of tiny, microscopic organisms is responsible for giving it its bluish glow.";
      public static LocString ARTIFACT = (LocString) "The microscopic organisms within this rock are of a unique variety whose genetic code shows many tell-tale signs of being genetically engineered within a lab.\n\nFurther analysis reveals they share 99.999% of their genetic code with Shine Bugs.";
    }

    public class PLASMALAMP
    {
      public static LocString NAME = (LocString) "Plasma Lamp";
      public static LocString DESCRIPTION = (LocString) "No space colony is complete without one.";
      public static LocString ARTIFACT = (LocString) "The bottom of this lamp contains the words 'Property of the Atmospheric Sciences Department'.\n\nIt's worth noting that the Gravitas Atmospheric Sciences Department once simulated an experiment testing the feasibility of survival in an environment filled with noble gases, similar to the ones contained within this device.";
    }

    public class MOLDAVITE
    {
      public static LocString NAME = (LocString) "Moldavite";
      public static LocString DESCRIPTION = (LocString) "A unique green stone formed from the impact of a meteorite.";
      public static LocString ARTIFACT = (LocString) "This extremely rare, museum grade moldavite once sat on the desk of Dr. Ren Sato, but it was stolen by some unknown person.\n\nDr. Sato suspected the perpetrator was none other than Director Stern, but was never able to confirm this theory.";
    }

    public class BRICKPHONE
    {
      public static LocString NAME = (LocString) "Strange Brick";
      public static LocString DESCRIPTION = (LocString) "It still works.";
      public static LocString ARTIFACT = (LocString) "This cordless phone once held a direct line to an unknown location in which strange distant voices can be heard but not understood, nor interacted with.\n\nThough Gravitas spent a lot of money and years of study dedicated to discovering its secret, the mystery was never solved.";
    }

    public class SOLARSYSTEM
    {
      public static LocString NAME = (LocString) "Self-Contained System";
      public static LocString DESCRIPTION = (LocString) "A marvel of the cosmos, inside this display is an entirely self-contained solar system.";
      public static LocString ARTIFACT = (LocString) "This marvel of a device was built using parts from an old Tornado-in-a-Box science fair project.\n\nVery faint, faded letters are still visible on the display bottom that read 'Camille P. Grade 5'.";
    }

    public class SINK
    {
      public static LocString NAME = (LocString) "Sink";
      public static LocString DESCRIPTION = (LocString) "No collection is complete without it.";
      public static LocString ARTIFACT = (LocString) "A small trace of encrusted soap on this sink strongly suggests it was installed in a personal bathroom, rather than a public one which would have used a soap dispenser.\n\nThe soap sliver is light blue and contains a manufactured blueberry fragrance.";
    }

    public class ROCKTORNADO
    {
      public static LocString NAME = (LocString) "Tornado Rock";
      public static LocString DESCRIPTION = (LocString) "It's unclear how it formed, although I'm glad it did.";
      public static LocString ARTIFACT = (LocString) "Speculations about the origin of this rock include a paper written by one Harold P. Moreson, Ph.D. in which he theorized it could be a rare form of hollow geode which failed to form any crystals inside.\n\nThis paper appears in the Gravitas archives, and in all probability, was one of the factors in the hiring of Moreson into the Geology department of the company.";
    }

    public class BLENDER
    {
      public static LocString NAME = (LocString) "Blender";
      public static LocString DESCRIPTION = (LocString) "Equipment used to conduct experiments answering the age-old question, \"Could that blend\"?";
      public static LocString ARTIFACT = (LocString) "Trace amounts of edible foodstuffs present in this blender indicate that it was probably used to emulsify the ingredients of a mush bar.\n\nIt is also very likely that it was employed at least once in the production of a peanut butter and banana smoothie.";
    }

    public class SAXOPHONE
    {
      public static LocString NAME = (LocString) "Mangled Saxophone";
      public static LocString DESCRIPTION = (LocString) "The name \"Pesquet\" is barely legible on the inside.";
      public static LocString ARTIFACT = (LocString) "Though it is often remarked that \"in space, no one can hear you scream\", Thomas Pesquet proved the same cannot be said for the smooth jazzy sounds of a saxophone.\n\nAlthough this instrument once belonged to the eminent French Astronaut its current bumped and bent shape suggests it has seen many adventures beyond that of just being used to perform an out-of-this-world saxophone solo.";
    }

    public class STETHOSCOPE
    {
      public static LocString NAME = (LocString) "Stethoscope";
      public static LocString DESCRIPTION = (LocString) "Listens to Duplicant heartbeats, or gurgly tummies.";
      public static LocString ARTIFACT = (LocString) "The size and shape of this stethescope suggests it was not intended to be used by neither a human-sized nor a Duplicant-sized person but something half-way in between the two beings.";
    }

    public class VHS
    {
      public static LocString NAME = (LocString) "Archaic Tech";
      public static LocString DESCRIPTION = (LocString) "Be kind when you handle it. It's very fragile.";
      public static LocString ARTIFACT = (LocString) "The label on this VHS tape reads \"Jackie and Olivia's House Warming Party\".\n\nUnfortunately, a device with which to play this recording no longer exists in this universe.";
    }

    public class REACTORMODEL
    {
      public static LocString NAME = (LocString) "Model Nuclear Power Plant";
      public static LocString DESCRIPTION = (LocString) "It's pronounced nu-clear.";
      public static LocString ARTIFACT = (LocString) "Though this Nuclear Power Plant was never built, this model exists as an artifact to a time early in the life of Gravitas when it was researching all alternatives to solving the global energy problem.\n\nUltimately, the idea of building a Nuclear Power Plant was abandoned in favor of the \"much safer\" alternative of developing the Temporal Bow.";
    }

    public class MOODRING
    {
      public static LocString NAME = (LocString) "Radiation Mood Ring";
      public static LocString DESCRIPTION = (LocString) "How radioactive are you feeling?";
      public static LocString ARTIFACT = (LocString) "A wholly unique ring not found anywhere outside of the Gravitas Laboratory.\n\nThough it can't be determined for sure who worked on this extraordinary curiousity it's worth noting that, for his Ph.D. thesis, Dr. Travaldo Farrington wrote a paper entitled \"Novelty Uses for Radiochromatic Dyes\".";
    }

    public class ORACLE
    {
      public static LocString NAME = (LocString) "Useless Machine";
      public static LocString DESCRIPTION = (LocString) "What does it do?";
      public static LocString ARTIFACT = (LocString) "All of the parts for this contraption are recycled from projects abandoned by the Robotics department.\n\nThe design is very close to one published in an amateur DIY magazine that once sat in the lobby of the 'Employees Only' area of Gravitas' facilities.";
    }

    public class GRUBSTATUE
    {
      public static LocString NAME = (LocString) "Grubgrub Statue";
      public static LocString DESCRIPTION = (LocString) "A moving tribute to a tiny plant hugger.";
      public static LocString ARTIFACT = (LocString) "It's very likely this statue was placed in a hidden, secluded place in the Gravitas laboratory since the creation of Grubgrubs was a closely held secret that the general public was not privy to.\n\nThis is a shame since the artistic quality of this statue is really quite accomplished.";
    }

    public class HONEYJAR
    {
      public static LocString NAME = (LocString) "Honey Jar";
      public static LocString DESCRIPTION = (LocString) "Sweet golden liquid with just a touch of uranium.";
      public static LocString ARTIFACT = (LocString) "Records from the Genetics and Biology Lab of the Gravitas facility show that several early iterations of a radioactive Bee would continue to produce honey and that this honey was once accidentally stored in the employee kitchen which resulted in several incidents of minor radiation poisoning when it was erroneously labled as a sweetener for tea.\n\nEmployees who used this product reported that it was the \"sweetest honey they'd ever tasted\" and expressed no regret at the mix-up.";
    }

    public class PLASTICFLOWERS
    {
      public static LocString NAME = (LocString) "Plastic Flowers";
      public static LocString DESCRIPTION = (LocString) "Maintenance-free blooms that will outlast us all.";
      public static LocString ARTIFACT = (LocString) "Manufactured and sold by a home staging company hired by Gravitas to \"make Space feel more like home.\"\n\nThis bouquet is designed to smell like freshly baked cookies.";
    }

    public class FOUNTAINPEN
    {
      public static LocString NAME = (LocString) "Fountain Pen";
      public static LocString DESCRIPTION = (LocString) "It cuts through red tape better than a sword ever could.";
      public static LocString ARTIFACT = (LocString) "The handcrafted gold nib features a triangular logo with the letters V and I inside.\n\nIts owner was too proud to report it stolen, and would be shocked to learn of its whereabouts.";
    }
  }

  public class KEEPSAKES
  {
    public class CRITTER_MANIPULATOR
    {
      public static LocString NAME = (LocString) "Ceramic Morb";
      public static LocString DESCRIPTION = (LocString) "A pottery project produced in an HR-mandated art therapy class.\n\nIt's glazed with a substance that once landed a curious lab technician in the ER.";
    }

    public class MEGA_BRAIN
    {
      public static LocString NAME = (LocString) "Model Plane";
      public static LocString DESCRIPTION = (LocString) "A treasured souvenir that was once a common accompaniment to children's meals during commercial flights. There's a hole in the bottom from when Dr. Holland had it mounted on a stand.";
    }

    public class LONELY_MINION
    {
      public static LocString NAME = (LocString) "Rusty Toolbox";
      public static LocString DESCRIPTION = (LocString) "On the inside of the lid, someone used a screwdriver to carve a drawing of a group of smiling Duplicants gathered around a massive crater.";
    }

    public class FOSSIL_HUNT
    {
      public static LocString NAME = (LocString) "Critter Collar";
      public static LocString DESCRIPTION = (LocString) "The tag reads \"Molly\".\n\nOn the reverse is \"Designed by B363\" stamped above what appears to be an unusually shaped pawprint.";
    }

    public class MORB_ROVER_MAKER
    {
      public static LocString NAME = (LocString) "Toy Bot";
      public static LocString DESCRIPTION = (LocString) "A custom-made robot programmed to deliver puns in a variety of celebrity voices.\n\nIt is also a paper shredder.";
    }

    public class GEOTHERMAL_PLANT
    {
      public static LocString NAME = (LocString) "Shiny Coprolite";
      public static LocString DESCRIPTION = (LocString) "A spectacular sample of organic material fossilized into lead.\n\nSome things really <i>do</i> get better with age.";
    }

    public class VIEWMASTER
    {
      public static LocString NAME = (LocString) "Stereoscope";
      public static LocString DESCRIPTION = (LocString) "A tool used to gaze into frozen moments of time.\n\nOne of the images is of a child standing in a field, waving a huge piece of blackened titanium.";
    }

    public class HIJACK_HEADQUARTERS
    {
      public static LocString NAME = (LocString) "Hair Dryer";
      public static LocString DESCRIPTION = (LocString) "A vintage follicle styling tool.\n\nThe handle bears a small engraving that reads: \"To N.T., from N.T.\"";
    }
  }

  public class SANDBOXTOOLS
  {
    public class SETTINGS
    {
      public class INSTANT_BUILD
      {
        public static LocString NAME = (LocString) "Instant build mode ON";
        public static LocString TOOLTIP = (LocString) "Toggle between placing construction plans and fully built buildings";
      }

      public class BRUSH_SIZE
      {
        public static LocString NAME = (LocString) "Size";
        public static LocString TOOLTIP = (LocString) "Adjust brush size";
      }

      public class BRUSH_NOISE_SCALE
      {
        public static LocString NAME = (LocString) "Noise A";
        public static LocString TOOLTIP = (LocString) "Adjust brush noisiness A";
      }

      public class BRUSH_NOISE_DENSITY
      {
        public static LocString NAME = (LocString) "Noise B";
        public static LocString TOOLTIP = (LocString) "Adjust brush noisiness B";
      }

      public class TEMPERATURE
      {
        public static LocString NAME = (LocString) "Temperature";
        public static LocString TOOLTIP = (LocString) "Adjust absolute temperature";
      }

      public class TEMPERATURE_ADDITIVE
      {
        public static LocString NAME = (LocString) "Temperature";
        public static LocString TOOLTIP = (LocString) "Adjust additive temperature";
      }

      public class RADIATION
      {
        public static LocString NAME = (LocString) "Absolute radiation";
        public static LocString TOOLTIP = (LocString) "Adjust absolute radiation";
      }

      public class RADIATION_ADDITIVE
      {
        public static LocString NAME = (LocString) "Additive radiation";
        public static LocString TOOLTIP = (LocString) "Adjust additive radiation";
      }

      public class STRESS_ADDITIVE
      {
        public static LocString NAME = (LocString) "Reduce Stress";
        public static LocString TOOLTIP = (LocString) "Adjust stress reduction";
      }

      public class MORALE
      {
        public static LocString NAME = (LocString) "Adjust Morale";
        public static LocString TOOLTIP = (LocString) "Bonus Morale adjustment";
      }

      public class MASS
      {
        public static LocString NAME = (LocString) "Mass";
        public static LocString TOOLTIP = (LocString) "Adjust mass";
      }

      public class DISEASE
      {
        public static LocString NAME = (LocString) "Germ";
        public static LocString TOOLTIP = (LocString) "Adjust type of germ";
      }

      public class DISEASE_COUNT
      {
        public static LocString NAME = (LocString) "Germs";
        public static LocString TOOLTIP = (LocString) "Adjust germ count";
      }

      public class BRUSH
      {
        public static LocString NAME = (LocString) "Brush";
        public static LocString TOOLTIP = (LocString) "Paint elements into the world simulation {Hotkey}";
      }

      public class ELEMENT
      {
        public static LocString NAME = (LocString) "Element";
        public static LocString TOOLTIP = (LocString) "Adjust type of element";
      }

      public class TARGET_LAYER
      {
        public static LocString NAME = (LocString) "Target Layer";
        public static LocString TOOLTIP = (LocString) "Adjust which layer the brush tools will affect\n\nL- Backwall\nR- Far backwall";
        public static LocString FOREGROUND = (LocString) "Foreground";
        public static LocString BACKGROUND = (LocString) "Background";
      }

      public class SPRINKLE
      {
        public static LocString NAME = (LocString) "Sprinkle";
        public static LocString TOOLTIP = (LocString) "Paint elements into the simulation using noise {Hotkey}";
      }

      public class FLOOD
      {
        public static LocString NAME = (LocString) "Fill";
        public static LocString TOOLTIP = (LocString) "Fill a section of the simulation with the chosen element {Hotkey}";
      }

      public class SAMPLE
      {
        public static LocString NAME = (LocString) "Sample";
        public static LocString TOOLTIP = (LocString) "Copy the settings from a cell to use with brush tools {Hotkey}";
      }

      public class HEATGUN
      {
        public static LocString NAME = (LocString) "Heat Gun";
        public static LocString TOOLTIP = (LocString) "Inject thermal energy into the simulation {Hotkey}";
      }

      public class RADSTOOL
      {
        public static LocString NAME = (LocString) "Radiation Tool";
        public static LocString TOOLTIP = (LocString) "Inject or remove radiation from the simulation {Hotkey}";
      }

      public class SPAWNER
      {
        public static LocString NAME = (LocString) "Spawner";
        public static LocString TOOLTIP = (LocString) "Spawn critters, food, equipment, and other entities {Hotkey}";
      }

      public class STRESS
      {
        public static LocString NAME = (LocString) "Stress";
        public static LocString TOOLTIP = (LocString) "Manage Duplicants' stress levels {Hotkey}";
      }

      public class CLEAR_FLOOR
      {
        public static LocString NAME = (LocString) "Clear Debris";
        public static LocString TOOLTIP = (LocString) "Delete loose items cluttering the floor {Hotkey}";
      }

      public class DESTROY
      {
        public static LocString NAME = (LocString) "Destroy";
        public static LocString TOOLTIP = (LocString) "Delete everything in the selected cell(s) {Hotkey}";
      }

      public class SPAWN_ENTITY
      {
        public static LocString NAME = (LocString) "Spawn";
      }

      public class FOW
      {
        public static LocString NAME = (LocString) "Reveal";
        public static LocString TOOLTIP = (LocString) "Dispel the Fog of War shrouding the map {Hotkey}";
      }

      public class CRITTER
      {
        public static LocString NAME = (LocString) "Critter Removal";
        public static LocString TOOLTIP = (LocString) "Remove critters! {Hotkey}";
      }

      public class SPAWN_STORY_TRAIT
      {
        public static LocString NAME = (LocString) "Story Traits";
        public static LocString TOOLTIP = (LocString) "Spawn story traits {Hotkey}";
      }
    }

    public class FILTERS
    {
      public static LocString BACK = (LocString) "Back";
      public static LocString COMMON = (LocString) "Common Substances";
      public static LocString SOLID = (LocString) "Solids";
      public static LocString LIQUID = (LocString) "Liquids";
      public static LocString GAS = (LocString) "Gases";

      public class ENTITIES
      {
        public static LocString BIONICUPGRADES = (LocString) "Boosters";
        public static LocString SPECIAL = (LocString) "Special";
        public static LocString GRAVITAS = (LocString) "Gravitas";
        public static LocString PLANTS = (LocString) "Plants";
        public static LocString SEEDS = (LocString) "Seeds";
        public static LocString CREATURE = (LocString) "Critters";
        public static LocString CREATURE_EGG = (LocString) "Eggs";
        public static LocString FOOD = (LocString) "Foods";
        public static LocString EQUIPMENT = (LocString) "Equipment";
        public static LocString GEYSERS = (LocString) "Geysers";
        public static LocString EXPERIMENTS = (LocString) "Experimental";
        public static LocString INDUSTRIAL_PRODUCTS = (LocString) "Industrial";
        public static LocString COMETS = (LocString) "Meteors";
        public static LocString ARTIFACTS = (LocString) "Artifacts";
        public static LocString STORYTRAITS = (LocString) "Story Traits";
        public static LocString ORE_CHUNKS = (LocString) "Solid Materials";
        public static LocString BOTTLES = (LocString) "Bottled Liquids";
        public static LocString CANISTERS = (LocString) "Gas Canisters";
      }
    }

    public class CLEARFLOOR
    {
      public static LocString DELETED = (LocString) "Deleted";
    }
  }

  public class RETIRED_COLONY_INFO_SCREEN
  {
    public static LocString SECONDS = (LocString) "Seconds";
    public static LocString CYCLES = (LocString) "Cycles";
    public static LocString CYCLE_COUNT = (LocString) "Cycle Count: {0}";
    public static LocString DUPLICANT_AGE = (LocString) "Age: {0} cycles";
    public static LocString SKILL_LEVEL = (LocString) "Skill Level: {0}";
    public static LocString BUILDING_COUNT = (LocString) "Count: {0}";
    public static LocString PREVIEW_UNAVAILABLE = (LocString) "Preview\nUnavailable";
    public static LocString TIMELAPSE_UNAVAILABLE = (LocString) "Timelapse\nUnavailable";
    public static LocString SEARCH = (LocString) "SEARCH...";

    public class BUTTONS
    {
      public static LocString RETURN_TO_GAME = (LocString) "RETURN TO GAME";
      public static LocString VIEW_OTHER_COLONIES = (LocString) "BACK";
      public static LocString QUIT_TO_MENU = (LocString) "QUIT TO MAIN MENU";
      public static LocString CLOSE = (LocString) nameof (CLOSE);
    }

    public class TITLES
    {
      public static LocString EXPLORER_HEADER = (LocString) "COLONIES";
      public static LocString RETIRED_COLONIES = (LocString) "Colony Summaries";
      public static LocString COLONY_STATISTICS = (LocString) "Colony Statistics";
      public static LocString DUPLICANTS = (LocString) "Duplicants";
      public static LocString BUILDINGS = (LocString) "Buildings";
      public static LocString CHEEVOS = (LocString) "Colony Achievements";
      public static LocString ACHIEVEMENT_HEADER = (LocString) "ACHIEVEMENTS";
      public static LocString TIMELAPSE = (LocString) "Timelapse";
    }

    public class STATS
    {
      public static LocString OXYGEN_CREATED = (LocString) "Total Oxygen Produced";
      public static LocString OXYGEN_CONSUMED = (LocString) "Total Oxygen Consumed";
      public static LocString POWER_CREATED = (LocString) "Average Power Produced";
      public static LocString POWER_WASTED = (LocString) "Average Power Wasted";
      public static LocString TRAVEL_TIME = (LocString) "Total Travel Time";
      public static LocString WORK_TIME = (LocString) "Total Work Time";
      public static LocString AVERAGE_TRAVEL_TIME = (LocString) "Average Travel Time";
      public static LocString AVERAGE_WORK_TIME = (LocString) "Average Work Time";
      public static LocString CALORIES_CREATED = (LocString) "Calorie Generation";
      public static LocString CALORIES_CONSUMED = (LocString) "Calorie Consumption";
      public static LocString LIVE_DUPLICANTS = (LocString) "Duplicants";
      public static LocString AVERAGE_STRESS_CREATED = (LocString) "Average Stress Created";
      public static LocString AVERAGE_STRESS_REMOVED = (LocString) "Average Stress Removed";
      public static LocString NUMBER_DOMESTICATED_CRITTERS = (LocString) "Domesticated Critters";
      public static LocString NUMBER_WILD_CRITTERS = (LocString) "Wild Critters";
      public static LocString AVERAGE_GERMS = (LocString) "Average Germs";
      public static LocString ROCKET_MISSIONS = (LocString) "Rocket Missions Underway";
    }
  }

  public class DROPDOWN
  {
    public static LocString NONE = (LocString) "Unassigned";
  }

  public class FRONTEND
  {
    public static LocString GAME_VERSION = (LocString) "Game Version: ";
    public static LocString LOADING = (LocString) "Loading...";
    public static LocString DONE_BUTTON = (LocString) "DONE";

    public class DEMO_OVER_SCREEN
    {
      public static LocString TITLE = (LocString) "Thanks for playing!";
      public static LocString BODY = (LocString) "Thank you for playing the demo for Oxygen Not Included!\n\nThis game is still in development.\n\nGo to kleigames.com/o2 or ask one of us if you'd like more information.";
      public static LocString BUTTON_EXIT_TO_MENU = (LocString) "EXIT TO MENU";
    }

    public class CUSTOMGAMESETTINGSSCREEN
    {
      public class SETTINGS
      {
        public class SANDBOXMODE
        {
          public static LocString NAME = (LocString) "Sandbox Mode";
          public static LocString TOOLTIP = (LocString) "Manipulate and customize the simulation with tools that ignore regular game constraints";

          public static class LEVELS
          {
            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "Unchecked: Sandbox Mode is turned off (Default)";
            }

            public static class ENABLED
            {
              public static LocString NAME = (LocString) "Enabled";
              public static LocString TOOLTIP = (LocString) "Checked: Sandbox Mode is turned on";
            }
          }
        }

        public class FASTWORKERSMODE
        {
          public static LocString NAME = (LocString) "Fast Workers Mode";
          public static LocString TOOLTIP = (LocString) "Duplicants will finish most work immediately and require little sleep";

          public static class LEVELS
          {
            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "Unchecked: Fast Workers Mode is turned off (Default)";
            }

            public static class ENABLED
            {
              public static LocString NAME = (LocString) "Enabled";
              public static LocString TOOLTIP = (LocString) "Checked: Fast Workers Mode is turned on";
            }
          }
        }

        public class EXPANSION1ACTIVE
        {
          public static LocString NAME = (LocString) ((string) UI.EXPANSION1.NAME_ITAL + " Content Enabled");
          public static LocString TOOLTIP = (LocString) $"If checked, content from the {(string) UI.EXPANSION1.NAME_ITAL} Expansion will be available";

          public static class LEVELS
          {
            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) $"Unchecked: {(string) UI.EXPANSION1.NAME_ITAL} Content is turned off (Default)";
            }

            public static class ENABLED
            {
              public static LocString NAME = (LocString) "Enabled";
              public static LocString TOOLTIP = (LocString) $"Checked: {(string) UI.EXPANSION1.NAME_ITAL} Content is turned on";
            }
          }
        }

        public class SAVETOCLOUD
        {
          public static LocString NAME = (LocString) "Save To Cloud";
          public static LocString TOOLTIP = (LocString) "This colony will be created in the cloud saves folder, and synced by the game platform.";
          public static LocString TOOLTIP_LOCAL = (LocString) "This colony will be created in the local saves folder. It will not be a cloud save and will not be synced by the game platform.";
          public static LocString TOOLTIP_EXTRA = (LocString) "This can be changed later with the colony management options in the load screen, from the main menu.";

          public static class LEVELS
          {
            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "Unchecked: This colony will be a local save";
            }

            public static class ENABLED
            {
              public static LocString NAME = (LocString) "Enabled";
              public static LocString TOOLTIP = (LocString) "Checked: This colony will be a cloud save (Default)";
            }
          }
        }

        public class CAREPACKAGES
        {
          public static LocString NAME = (LocString) "Care Packages";
          public static LocString TOOLTIP = (LocString) "Affects what resources can be printed from the Printing Pod";

          public static class LEVELS
          {
            public static class NORMAL
            {
              public static LocString NAME = (LocString) "All";
              public static LocString TOOLTIP = (LocString) "Checked: The Printing Pod will offer both Duplicant blueprints and care packages (Default)";
            }

            public static class DUPLICANTS_ONLY
            {
              public static LocString NAME = (LocString) "Duplicants Only";
              public static LocString TOOLTIP = (LocString) "Unchecked: The Printing Pod will only offer Duplicant blueprints";
            }
          }
        }

        public class IMMUNESYSTEM
        {
          public static LocString NAME = (LocString) "Disease";
          public static LocString TOOLTIP = (LocString) "Affects Duplicants' chances of contracting a disease after germ exposure";

          public static class LEVELS
          {
            public static class COMPROMISED
            {
              public static LocString NAME = (LocString) "Outbreak Prone";
              public static LocString TOOLTIP = (LocString) "The whole colony will be ravaged by plague if a Duplicant so much as sneezes funny";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Outbreak Prone (Highest Difficulty)";
            }

            public static class WEAK
            {
              public static LocString NAME = (LocString) "Germ Susceptible";
              public static LocString TOOLTIP = (LocString) "These Duplicants have an increased chance of contracting diseases from germ exposure";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Germ Susceptibility (Difficulty Up)";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) "Default disease chance";
            }

            public static class STRONG
            {
              public static LocString NAME = (LocString) "Germ Resistant";
              public static LocString TOOLTIP = (LocString) "These Duplicants have a decreased chance of contracting diseases from germ exposure";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Germ Resistance (Difficulty Down)";
            }

            public static class INVINCIBLE
            {
              public static LocString NAME = (LocString) "Total Immunity";
              public static LocString TOOLTIP = (LocString) "Like diplomatic immunity, but without the diplomacy. These Duplicants will never get sick";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Total Immunity (No Disease)";
            }
          }
        }

        public class MORALE
        {
          public static LocString NAME = (LocString) "Morale";
          public static LocString TOOLTIP = (LocString) "Adjusts the minimum morale Duplicants must maintain to avoid gaining stress";

          public static class LEVELS
          {
            public static class VERYHARD
            {
              public static LocString NAME = (LocString) "Draconian";
              public static LocString TOOLTIP = (LocString) "The finest of the finest can barely keep up with these Duplicants' stringent demands";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Draconian (Highest Difficulty)";
            }

            public static class HARD
            {
              public static LocString NAME = (LocString) "A Bit Persnickety";
              public static LocString TOOLTIP = (LocString) "Duplicants require higher morale than usual to fend off stress";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "A Bit Persnickety (Difficulty Up)";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) "Default morale needs";
            }

            public static class EASY
            {
              public static LocString NAME = (LocString) "Chill";
              public static LocString TOOLTIP = (LocString) "Duplicants require lower morale than usual to fend off stress";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Chill (Difficulty Down)";
            }

            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Totally Blasé";
              public static LocString TOOLTIP = (LocString) "These Duplicants have zero standards and will never gain stress, regardless of their morale";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Totally Blasé (No Morale)";
            }
          }
        }

        public class CALORIE_BURN
        {
          public static LocString NAME = (LocString) "Hunger";
          public static LocString TOOLTIP = (LocString) "Affects how quickly Duplicants burn calories and become hungry";

          public static class LEVELS
          {
            public static class VERYHARD
            {
              public static LocString NAME = (LocString) "Ravenous";
              public static LocString TOOLTIP = (LocString) "Your Duplicants are on a see-food diet... They see food and they eat it";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Ravenous (Highest Difficulty)";
            }

            public static class HARD
            {
              public static LocString NAME = (LocString) "Rumbly Tummies";
              public static LocString TOOLTIP = (LocString) "Duplicants burn calories quickly and require more feeding than usual";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Rumbly Tummies (Difficulty Up)";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) "Default calorie burn rate";
            }

            public static class EASY
            {
              public static LocString NAME = (LocString) "Fasting";
              public static LocString TOOLTIP = (LocString) "Duplicants burn calories slowly and get by with fewer meals";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Fasting (Difficulty Down)";
            }

            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Tummyless";
              public static LocString TOOLTIP = (LocString) "These Duplicants were printed without tummies and need no food at all";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Tummyless (No Hunger)";
            }
          }
        }

        public class BIONICPOWERUSE
        {
          public static LocString NAME = (LocString) "Bionic Wattage";
          public static LocString TOOLTIP = (LocString) "Adjusts Bionic Duplicants' baseline power consumption";

          public static class LEVELS
          {
            public static class VERYHARD
            {
              public static LocString NAME = (LocString) "Energy Vampire";
              public static LocString TOOLTIP = (LocString) "These Bionic Duplicants drain batteries like it's their only job";
            }

            public static class HARD
            {
              public static LocString NAME = (LocString) "Power Hungry";
              public static LocString TOOLTIP = (LocString) "These Duplicants have an increased appetite for power";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) "Default wattage";
            }

            public static class EASY
            {
              public static LocString NAME = (LocString) "Energy Efficient";
              public static LocString TOOLTIP = (LocString) "These Duplicants consume less power than usual";
            }

            public static class VERYEASY
            {
              public static LocString NAME = (LocString) "Analog";
              public static LocString TOOLTIP = (LocString) "These Bionic Duplicants run on old-school enthusiasm, and barely consume power at all";
            }
          }
        }

        public class DEMOLIORDIFFICULTY
        {
          public static LocString NAME = (LocString) "Demolior Impact";
          public static LocString TOOLTIP = (LocString) "Adjusts how soon the Demolior asteroid collides with <i>The Prehistoric Planet Pack</i> asteroid";

          public static class LEVELS
          {
            public static class VERYHARD
            {
              public static LocString NAME = (LocString) "Imminent Extinction";
              public static LocString TOOLTIP = (LocString) $"It'll all be over soon\n\nOnly {100f.ToString()} cycles until collision";
            }

            public static class HARD
            {
              public static LocString NAME = (LocString) "Early Arrival";
              public static LocString TOOLTIP = (LocString) $"Demolior impacts sooner than usual\n\n{150f.ToString()} cycles until collision";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) $"Demolior impacts in {200f.ToString()} cycles";
            }

            public static class EASY
            {
              public static LocString NAME = (LocString) "Slightly Delayed";
              public static LocString TOOLTIP = (LocString) $"Demolior impacts later than usual\n\n{300f.ToString()} cycles until collision";
            }

            public static class VERYEASY
            {
              public static LocString NAME = (LocString) "Far-Off Forecast";
              public static LocString TOOLTIP = (LocString) "Duplicants could probably build a whole new asteroid by the time Demolior impacts this one\n\n500 cycles until collision";
            }

            public static class OFF
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "Demolior does not exist in this universe and the achievement cannot be earned";
            }
          }
        }

        public class WORLD_CHOICE
        {
          public static LocString NAME = (LocString) "World";
          public static LocString TOOLTIP = (LocString) "New worlds added by mods can be selected here";
        }

        public class CLUSTER_CHOICE
        {
          public static LocString NAME = (LocString) "Asteroid Belt";
          public static LocString TOOLTIP = (LocString) "New asteroid belts added by mods can be selected here";
        }

        public class STORY_TRAIT_COUNT
        {
          public static LocString NAME = (LocString) "Story Traits";
          public static LocString TOOLTIP = (LocString) "Determines the number of story traits spawned";

          public static class LEVELS
          {
            public static class NONE
            {
              public static LocString NAME = (LocString) "Zilch";
              public static LocString TOOLTIP = (LocString) "Zero story traits. Zip. Nada. None";
            }

            public static class FEW
            {
              public static LocString NAME = (LocString) "Stingy";
              public static LocString TOOLTIP = (LocString) "Not zero, but not a lot";
            }

            public static class LOTS
            {
              public static LocString NAME = (LocString) "Oodles";
              public static LocString TOOLTIP = (LocString) "Plenty of story traits to go around";
            }
          }
        }

        public class DURABILITY
        {
          public static LocString NAME = (LocString) "Durability";
          public static LocString TOOLTIP = (LocString) "Affects how quickly equippable suits wear out";

          public static class LEVELS
          {
            public static class INDESTRUCTIBLE
            {
              public static LocString NAME = (LocString) "Indestructible";
              public static LocString TOOLTIP = (LocString) "Duplicants have perfected clothes manufacturing and are able to make suits that last forever";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Indestructible Suits (No Durability)";
            }

            public static class REINFORCED
            {
              public static LocString NAME = (LocString) "Reinforced";
              public static LocString TOOLTIP = (LocString) "Suits are more durable than usual";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Reinforced Suits (Difficulty Down)";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) "Default suit durability";
            }

            public static class FLIMSY
            {
              public static LocString NAME = (LocString) "Flimsy";
              public static LocString TOOLTIP = (LocString) "Suits wear out faster than usual";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Flimsy Suits (Difficulty Up)";
            }

            public static class THREADBARE
            {
              public static LocString NAME = (LocString) "Threadbare";
              public static LocString TOOLTIP = (LocString) "These Duplicants are no tailors - suits wear out much faster than usual";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Threadbare Suits (Highest Difficulty)";
            }
          }
        }

        public class RADIATION
        {
          public static LocString NAME = (LocString) "Radiation";
          public static LocString TOOLTIP = (LocString) "Affects how susceptible Duplicants are to radiation sickness";

          public static class LEVELS
          {
            public static class HARDEST
            {
              public static LocString NAME = (LocString) "Critical Mass";
              public static LocString TOOLTIP = (LocString) "Duplicants feel ill at the merest mention of radiation...and may never truly recover";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Super Radiation (Highest Difficulty)";
            }

            public static class HARDER
            {
              public static LocString NAME = (LocString) "Toxic Positivity";
              public static LocString TOOLTIP = (LocString) "Duplicants are more sensitive to radiation exposure than usual";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Radiation Vulnerable (Difficulty Up)";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) "Default radiation settings";
            }

            public static class EASIER
            {
              public static LocString NAME = (LocString) "Healthy Glow";
              public static LocString TOOLTIP = (LocString) "Duplicants are more resistant to radiation exposure than usual";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Radiation Shielded (Difficulty Down)";
            }

            public static class EASIEST
            {
              public static LocString NAME = (LocString) "Nuke-Proof";
              public static LocString TOOLTIP = (LocString) "Duplicants could bathe in radioactive waste and not even notice";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Radiation Protection (Lowest Difficulty)";
            }
          }
        }

        public class STRESS
        {
          public static LocString NAME = (LocString) "Stress";
          public static LocString TOOLTIP = (LocString) "Affects how quickly Duplicant stress rises";

          public static class LEVELS
          {
            public static class INDOMITABLE
            {
              public static LocString NAME = (LocString) "Cloud Nine";
              public static LocString TOOLTIP = (LocString) "A strong emotional support system makes these Duplicants impervious to all stress";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Cloud Nine (No Stress)";
            }

            public static class OPTIMISTIC
            {
              public static LocString NAME = (LocString) "Chipper";
              public static LocString TOOLTIP = (LocString) "Duplicants gain stress slower than usual";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Chipper (Difficulty Down)";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) "Default stress change rate";
            }

            public static class PESSIMISTIC
            {
              public static LocString NAME = (LocString) "Glum";
              public static LocString TOOLTIP = (LocString) "Duplicants gain stress more quickly than usual";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Glum (Difficulty Up)";
            }

            public static class DOOMED
            {
              public static LocString NAME = (LocString) "Frankly Depressing";
              public static LocString TOOLTIP = (LocString) "These Duplicants were never taught coping mechanisms... they're devastated by stress as a result";
              public static LocString ATTRIBUTE_MODIFIER_NAME = (LocString) "Frankly Depressing (Highest Difficulty)";
            }
          }
        }

        public class STRESS_BREAKS
        {
          public static LocString NAME = (LocString) "Stress Reactions";
          public static LocString TOOLTIP = (LocString) "Determines whether Duplicants wreak havoc on the colony when they reach maximum stress";

          public static class LEVELS
          {
            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Enabled";
              public static LocString TOOLTIP = (LocString) "Checked: Duplicants will wreak havoc when they reach 100% stress (Default)";
            }

            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "Unchecked: Duplicants will not wreak havoc at maximum stress";
            }
          }
        }

        public class WORLDGEN_SEED
        {
          public static LocString NAME = (LocString) "Worldgen Seed";
          public static LocString TOOLTIP = (LocString) "This number chooses the procedural parameters that create your unique map\n\nWorldgen seeds can be copied and pasted so others can play a replica of your world configuration";
          public static LocString FIXEDSEED = (LocString) "This is a predetermined seed, and cannot be changed";
        }

        public class TELEPORTERS
        {
          public static LocString NAME = (LocString) "Teleporters";
          public static LocString TOOLTIP = (LocString) "Determines whether teleporters will be spawned during Worldgen";

          public static class LEVELS
          {
            public static class ENABLED
            {
              public static LocString NAME = (LocString) "Enabled";
              public static LocString TOOLTIP = (LocString) "Checked: Teleporters will spawn during Worldgen (Default)";
            }

            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "Unchecked: No Teleporters will spawn during Worldgen";
            }
          }
        }

        public class METEORSHOWERS
        {
          public static LocString NAME = (LocString) "Meteor Showers";
          public static LocString TOOLTIP = (LocString) "Adjusts the intensity of incoming space rocks";

          public static class LEVELS
          {
            public static class CLEAR_SKIES
            {
              public static LocString NAME = (LocString) "Clear Skies";
              public static LocString TOOLTIP = (LocString) "No meteor damage, no worries";
            }

            public static class INFREQUENT
            {
              public static LocString NAME = (LocString) "Spring Showers";
              public static LocString TOOLTIP = (LocString) "Meteor showers are less frequent and less intense than usual";
            }

            public static class DEFAULT
            {
              public static LocString NAME = (LocString) "Default";
              public static LocString TOOLTIP = (LocString) "Default meteor shower frequency and intensity";
            }

            public static class INTENSE
            {
              public static LocString NAME = (LocString) "Cosmic Storm";
              public static LocString TOOLTIP = (LocString) "Meteor showers are more frequent and more intense than usual";
            }

            public static class DOOMED
            {
              public static LocString NAME = (LocString) "Doomsday";
              public static LocString TOOLTIP = (LocString) "An onslaught of apocalyptic hailstorms that feels almost personal";
            }
          }
        }

        public class DLC_MIXING
        {
          public static class LEVELS
          {
            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "Content from this DLC is currently <b>disabled</b>";
            }

            public static class ENABLED
            {
              public static LocString NAME = (LocString) "Enabled";
              public static LocString TOOLTIP = (LocString) "Content from this DLC is currently <b>enabled</b>\n\nThis includes Care Packages, buildings, and space POIs";
            }
          }
        }

        public class SUBWORLD_MIXING
        {
          public static class LEVELS
          {
            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "This biome will not be mixed into any world";
              public static LocString TOOLTIP_BASEGAME = (LocString) "This biome will not be mixed in";
            }

            public static class TRY_MIXING
            {
              public static LocString NAME = (LocString) "Likely";
              public static LocString TOOLTIP = (LocString) "This biome is very likely to be mixed into a world";
              public static LocString TOOLTIP_BASEGAME = (LocString) "This biome is very likely to be mixed in";
            }

            public static class GUARANTEE_MIXING
            {
              public static LocString NAME = (LocString) "Guaranteed";
              public static LocString TOOLTIP = (LocString) "This biome will be mixed into a world, even if it causes a worldgen failure";
              public static LocString TOOLTIP_BASEGAME = (LocString) "This biome will be mixed in, even if it causes a worldgen failure";
            }
          }
        }

        public class WORLD_MIXING
        {
          public static class LEVELS
          {
            public static class DISABLED
            {
              public static LocString NAME = (LocString) "Disabled";
              public static LocString TOOLTIP = (LocString) "This asteroid will not be mixed in";
            }

            public static class TRY_MIXING
            {
              public static LocString NAME = (LocString) "Likely";
              public static LocString TOOLTIP = (LocString) "This asteroid is very likely to be mixed in";
            }

            public static class GUARANTEE_MIXING
            {
              public static LocString NAME = (LocString) "Guaranteed";
              public static LocString TOOLTIP = (LocString) "This asteroid will be mixed in, even if it causes worldgen failure";
            }
          }
        }
      }
    }

    public class MAINMENU
    {
      public static LocString STARTDEMO = (LocString) "START DEMO";
      public static LocString NEWGAME = (LocString) "NEW GAME";
      public static LocString RESUMEGAME = (LocString) "RESUME GAME";
      public static LocString LOADGAME = (LocString) "LOAD GAME";
      public static LocString RETIREDCOLONIES = (LocString) "COLONY SUMMARIES";
      public static LocString KLEIINVENTORY = (LocString) "KLEI INVENTORY";
      public static LocString LOCKERMENU = (LocString) "SUPPLY CLOSET";
      public static LocString SCENARIOS = (LocString) nameof (SCENARIOS);
      public static LocString TRANSLATIONS = (LocString) nameof (TRANSLATIONS);
      public static LocString OPTIONS = (LocString) nameof (OPTIONS);
      public static LocString QUITTODESKTOP = (LocString) nameof (QUIT);
      public static LocString RESTARTCONFIRM = (LocString) "Should I really quit?\nAll unsaved progress will be lost.";
      public static LocString QUITCONFIRM = (LocString) "Should I quit to the main menu?\nAll unsaved progress will be lost.";
      public static LocString RETIRECONFIRM = (LocString) "Should I surrender under the soul-crushing weight of this universe's entropy and retire my colony?";
      public static LocString DESKTOPQUITCONFIRM = (LocString) "Should I really quit?\nAll unsaved progress will be lost.";
      public static LocString RESUMEBUTTON_BASENAME = (LocString) "{0}: Cycle {1}";
      public static LocString QUIT = (LocString) "QUIT WITHOUT SAVING";
      public static LocString SAVEANDQUITTITLE = (LocString) "SAVE AND QUIT";
      public static LocString SAVEANDQUITDESKTOP = (LocString) "SAVE AND QUIT";

      public class DLC
      {
        public static LocString ACTIVATE_EXPANSION1 = (LocString) "ENABLE DLC";
        public static LocString ACTIVATE_EXPANSION1_TOOLTIP = (LocString) "<b>This DLC is disabled</b>\n\n<color=#ffff00ff><b>Click to enable the <i>Spaced Out!</i> DLC</b></color>";
        public static LocString ACTIVATE_EXPANSION1_DESC = (LocString) "The game will need to restart in order to enable <i>Spaced Out!</i>";
        public static LocString ACTIVATE_EXPANSION1_RAIL_DESC = (LocString) "<i>Spaced Out!</i> will be enabled the next time you launch the game. The game will now close.";
        public static LocString DEACTIVATE_EXPANSION1 = (LocString) "DISABLE DLC";
        public static LocString DEACTIVATE_EXPANSION1_TOOLTIP = (LocString) "<b>This DLC is enabled</b>\n\n<color=#ffff00ff><b>Click to disable the <i>Spaced Out!</i> DLC</b></color>";
        public static LocString DEACTIVATE_EXPANSION1_DESC = (LocString) "The game will need to restart in order to enable the <i>Oxygen Not Included</i> base game.";
        public static LocString DEACTIVATE_EXPANSION1_RAIL_DESC = (LocString) "<i>Spaced Out!</i> will be disabled the next time you launch the game. The game will now close.";
        public static LocString AD_DLC1 = (LocString) "Spaced Out! DLC";
        public static LocString CONTENT_INSTALLED_LABEL = (LocString) "Installed";
        public static LocString CONTENT_ACTIVE_TOOLTIP = (LocString) "<b>This DLC is enabled</b>\n\nFind it in the destination selection screen when starting a new game, or in the Load Game screen for existing DLC-enabled saves";
        public static LocString COSMETIC_CONTENT_ACTIVE_TOOLTIP = (LocString) "Filter cosmetic blueprints by DLC using the dropdown menu in the All screen";
        public static LocString CONTENT_OWNED_NOTINSTALLED_LABEL = (LocString) "";
        public static LocString CONTENT_OWNED_NOTINSTALLED_TOOLTIP = (LocString) "This DLC is owned but not currently installed";
        public static LocString CONTENT_NOTOWNED_PURCHASE_LABEL = (LocString) "Available Now";
        public static LocString CONTENT_NOTOWNED_PURCHASE_TOOLTIP = (LocString) "<color=#ffff00ff><b>Click to view this DLC in the store</b></color>";
        public static LocString CONTENT_NOTOWNED_WISHLIST_LABEL = (LocString) "Wishlist Now";
        public static LocString CONTENT_NOTOWNED_WISHLIST_TOOLTIP = (LocString) "<color=#ffff00ff><b>Click to view this DLC in the store</b></color>";
      }
    }

    public class DEVTOOLS
    {
      public static LocString TITLE = (LocString) "About Dev Tools";
      public static LocString WARNING = (LocString) "DANGER!!\n\nDev Tools are intended for developer use only. Using them may result in your save becoming unplayable, unstable, or severely damaged.\n\nThese tools are completely unsupported and may contain bugs. Are you sure you want to continue?";
      public static LocString DONTSHOW = (LocString) "Do not show this message again";
      public static LocString BUTTON = (LocString) "Show Dev Tools";
    }

    public class NEWGAMESETTINGS
    {
      public static LocString HEADER = (LocString) "GAME SETTINGS";

      public class BUTTONS
      {
        public static LocString STANDARDGAME = (LocString) "Standard Game";
        public static LocString CUSTOMGAME = (LocString) "Custom Game";
        public static LocString CANCEL = (LocString) "Cancel";
        public static LocString STARTGAME = (LocString) "Start Game";
      }
    }

    public class COLONYDESTINATIONSCREEN
    {
      public static LocString TITLE = (LocString) "CHOOSE A DESTINATION";
      public static LocString GENTLE_ZONE = (LocString) "Habitable Zone";
      public static LocString DETAILS = (LocString) "Destination Details";
      public static LocString START_SITE = (LocString) "Immediate Surroundings";
      public static LocString COORDINATE = (LocString) "Coordinates:";
      public static LocString CANCEL = (LocString) "Back";
      public static LocString CUSTOMIZE = (LocString) "Game Settings";
      public static LocString START_GAME = (LocString) "Start Game";
      public static LocString SHUFFLE = (LocString) "Shuffle";
      public static LocString SHUFFLETOOLTIP = (LocString) "Reroll World Seed\n\nThis will shuffle the layout of your world and the geographical traits listed below";
      public static LocString SHUFFLETOOLTIP_DISABLED = (LocString) "This world's seed is predetermined. It cannot be changed";
      public static LocString HEADER_ASTEROID_STARTING = (LocString) "Starting Asteroid";
      public static LocString HEADER_ASTEROID_NEARBY = (LocString) "Nearby Asteroids";
      public static LocString HEADER_ASTEROID_DISTANT = (LocString) "Distant Asteroids";
      public static LocString TRAITS_HEADER = (LocString) "World Traits";
      public static LocString STORY_TRAITS_HEADER = (LocString) "Story Traits";
      public static LocString MIXING_SETTINGS_HEADER = (LocString) "Scramble DLCs";
      public static LocString MIXING_DLC_HEADER = (LocString) "DLC Content";
      public static LocString MIXING_WORLDMIXING_HEADER = (LocString) "Asteroid Remix";
      public static LocString MIXING_SUBWORLDMIXING_HEADER = (LocString) "Biome Remix";
      public static LocString MIXING_NO_OPTIONS = (LocString) "No additional content currently available for remixing. Don't worry, there's plenty already baked in.";
      public static LocString MIXING_WARNING = (LocString) "Choose additional content to remix into the game. Scrambling realities may cause cosmic collapse.";
      public static LocString MIXING_TOOLTIP_DLC_MIXING = (LocString) "DLC content includes buildings, Care Packages, space POIs, critters, etc\n\nEnabling DLC content allows asteroid and biome remixes from that DLC to be customized in the sections below";
      public static LocString MIXING_TOOLTIP_ASTEROID_MIXING = (LocString) "Asteroid remixing modifies which asteroids appear on the Starmap\n\nRemixed asteroids will retain key features of the outer asteroids that they replace";
      public static LocString MIXING_TOOLTIP_BIOME_MIXING = (LocString) "Biome remixing modifies which biomes will be included across multiple asteroids";
      public static LocString MIXING_TOOLTIP_TOO_MANY_GUARENTEED_ASTEROID_MIXINGS = (LocString) ((string) UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_ASTEROID_MIXING + "\n\nMaximum of {1} guaranteed asteroid remixes allowed\n\nTotal currently selected: {0}");
      public static LocString MIXING_TOOLTIP_TOO_MANY_GUARENTEED_BIOME_MIXINGS = (LocString) ((string) UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_BIOME_MIXING + "\n\nMaximum of {1} guaranteed biome remixes allowed\n\nTotal currently selected: {0}");
      public static LocString MIXING_TOOLTIP_LOCKED_START_NOT_SUPPORTED = (LocString) "This destination does not support changing this setting";
      public static LocString MIXING_TOOLTIP_LOCKED_REQUIRE_DLC_NOT_ENABLED = (LocString) "This setting requires the following content to be enabled:\n{0}";
      public static LocString MIXING_TOOLTIP_DLC_CONTENT = (LocString) "This content is from {0}";
      public static LocString MIXING_TOOLTIP_MODDED_SETTING = (LocString) "<i><color=#d6d6d6>This setting was added by a mod</color></i>";
      public static LocString MIXING_TOOLTIP_CANNOT_START = (LocString) "Cannot start a new game with current asteroid and biome remix configuration";
      public static LocString NO_TRAITS = (LocString) "No Traits";
      public static LocString SINGLE_TRAIT = (LocString) "1 Trait";
      public static LocString TRAIT_COUNT = (LocString) "{0} Traits";
      public static LocString TRAIT_COUNT_TOOLTIP = (LocString) "Customize selection in the Story Traits tab";
      public static LocString TOO_MANY_TRAITS_WARNING = (LocString) $"{UI.YELLOW_PREFIX}Unstable!{UI.COLOR_SUFFIX}";
      public static LocString TOO_MANY_TRAITS_WARNING_TOOLTIP = (LocString) $"{UI.YELLOW_PREFIX}Squeezing this many story traits into this asteroid may cause worldgen to fail\n\nConsider lowering the number of story traits or changing the selected asteroid{UI.COLOR_SUFFIX}";
      public static LocString SHUFFLE_STORY_TRAITS_TOOLTIP = (LocString) "Randomize Story Traits\n\nThis will select a comfortable number of story traits for the starting asteroid";
      public static LocString SELECTED_CLUSTER_TRAITS_HEADER = (LocString) "Target Details";
    }

    public class MODESELECTSCREEN
    {
      public static LocString HEADER = (LocString) "GAME MODE";
      public static LocString BLANK_DESC = (LocString) "Select a playstyle...";
      public static LocString SURVIVAL_TITLE = (LocString) "SURVIVAL";
      public static LocString SURVIVAL_DESC = (LocString) "Stay on your toes and one step ahead of this unforgiving world. One slip up could bring your colony crashing down.";
      public static LocString NOSWEAT_TITLE = (LocString) "NO SWEAT";
      public static LocString NOSWEAT_DESC = (LocString) "When disaster strikes (and it inevitably will), take a deep breath and stay calm. You have ample time to find a solution.";
      public static LocString ACTIVE_CONTENT_HEADER = (LocString) "ACTIVE CONTENT";
    }

    public class CLUSTERCATEGORYSELECTSCREEN
    {
      public static LocString HEADER = (LocString) "ASTEROID STYLE";
      public static LocString BLANK_DESC = (LocString) "Select an asteroid style...";
      public static LocString VANILLA_TITLE = (LocString) "Standard";
      public static LocString VANILLA_DESC = (LocString) "Scenarios designed for classic gameplay.";
      public static LocString CLASSIC_TITLE = (LocString) "Classic";
      public static LocString CLASSIC_DESC = (LocString) "Scenarios similar to the <b>classic Oxygen Not Included</b> experience. Large starting asteroids with many resources.\nLess emphasis on space travel.";
      public static LocString SPACEDOUT_TITLE = (LocString) "Spaced Out!";
      public static LocString SPACEDOUT_DESC = (LocString) "Scenarios designed for the <b>Spaced Out! DLC</b>.\nSmaller starting asteroids with resources distributed across the Starmap. More emphasis on space travel.";
      public static LocString EVENT_TITLE = (LocString) "The Lab";
      public static LocString EVENT_DESC = (LocString) "Alternative gameplay experiences, including experimental scenarios designed for special events.";
    }

    public class PATCHNOTESSCREEN
    {
      public static LocString HEADER = (LocString) "IMPORTANT UPDATE NOTES";
      public static LocString OK_BUTTON = (LocString) "OK";
      public static LocString FULLPATCHNOTES_TOOLTIP = (LocString) "View the full patch notes online";
    }

    public class LOADSCREEN
    {
      public static LocString TITLE = (LocString) "LOAD GAME";
      public static LocString TITLE_INSPECT = (LocString) "LOAD GAME";
      public static LocString DELETEBUTTON = (LocString) "DELETE";
      public static LocString BACKBUTTON = (LocString) "< BACK";
      public static LocString CONFIRMDELETE = (LocString) "Are you sure you want to delete {0}?\nYou cannot undo this action.";
      public static LocString SAVEDETAILS = (LocString) "<b>File:</b> {0}\n\n<b>Save Date:</b>\n{1}\n\n<b>Base Name:</b> {2}\n<b>Duplicants Alive:</b> {3}\n<b>Cycle(s) Survived:</b> {4}";
      public static LocString AUTOSAVEWARNING = (LocString) "<color=#F44A47FF>Autosave: This file will get deleted as new autosaves are created</color>";
      public static LocString CORRUPTEDSAVE = (LocString) "<b><color=#F44A47FF>Could not load file {0}. Its data may be corrupted.</color></b>";
      public static LocString SAVE_TOO_NEW = (LocString) "<b><color=#F44A47FF>Could not load file {0}. File is using build {1}, v{2}. This build is {3}, v{4}.</color></b>";
      public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION = (LocString) "This save file was created with a different DLC configuration\n\nTo load this file:";
      public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_ENABLE = (LocString) "    • Activate {0}";
      public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_DISABLE = (LocString) "    • Deactivate {0}";
      public static LocString TOOLTIP_SAVE_USES_DLC = (LocString) "{0} save";
      public static LocString UNSUPPORTED_SAVE_VERSION = (LocString) "<b><color=#F44A47FF>This save file is from a previous version of the game and is no longer supported.</color></b>";
      public static LocString MORE_INFO = (LocString) "More Info";
      public static LocString NEWEST_SAVE = (LocString) "NEWEST";
      public static LocString BASE_NAME = (LocString) "Base Name";
      public static LocString CYCLES_SURVIVED = (LocString) "Cycles Survived";
      public static LocString DUPLICANTS_ALIVE = (LocString) "Duplicants Alive";
      public static LocString WORLD_NAME = (LocString) "Asteroid Type";
      public static LocString NO_FILE_SELECTED = (LocString) "No file selected";
      public static LocString COLONY_INFO_FMT = (LocString) "{0}: {1}";
      public static LocString LOAD_MORE_COLONIES_BUTTON = (LocString) "Load more...";
      public static LocString VANILLA_RESTART = (LocString) $"Loading this colony will require restarting the game with {(string) UI.EXPANSION1.NAME_ITAL} content disabled";
      public static LocString EXPANSION1_RESTART = (LocString) $"Loading this colony will require restarting the game with {(string) UI.EXPANSION1.NAME_ITAL} content enabled";
      public static LocString UNSUPPORTED_VANILLA_TEMP = (LocString) $"<b><color=#F44A47FF>This save file is from the base version of the game and currently cannot be loaded while {(string) UI.EXPANSION1.NAME_ITAL} is installed.</color></b>";
      public static LocString CONTENT = (LocString) "Content";
      public static LocString VANILLA_CONTENT = (LocString) "Vanilla FIXME";
      public static LocString EXPANSION1_CONTENT = (LocString) ((string) UI.EXPANSION1.NAME_ITAL + " Expansion FIXME");
      public static LocString SAVE_INFO = (LocString) "{0} saves  {1} autosaves  {2}";
      public static LocString COLONIES_TITLE = (LocString) "Colony View";
      public static LocString COLONY_TITLE = (LocString) "Viewing colony '{0}'";
      public static LocString COLONY_FILE_SIZE = (LocString) "Size: {0}";
      public static LocString COLONY_FILE_NAME = (LocString) "File: '{0}'";
      public static LocString NO_PREVIEW = (LocString) "NO PREVIEW";
      public static LocString LOCAL_SAVE = (LocString) "local";
      public static LocString CLOUD_SAVE = (LocString) "cloud";
      public static LocString CONVERT_COLONY = (LocString) "CONVERT COLONY";
      public static LocString CONVERT_ALL_COLONIES = (LocString) "CONVERT ALL";
      public static LocString CONVERT_ALL_WARNING = (LocString) $"{UI.PRE_KEYWORD}\nWarning:{UI.PST_KEYWORD} Converting all colonies may take some time.";
      public static LocString SAVE_INFO_DIALOG_TITLE = (LocString) "SAVE INFORMATION";
      public static LocString SAVE_INFO_DIALOG_TEXT = (LocString) "Access your save files using the options below.";
      public static LocString SAVE_INFO_DIALOG_TOOLTIP = (LocString) "Access your save file locations from here.";
      public static LocString CONVERT_ERROR_TITLE = (LocString) "SAVE CONVERSION UNSUCCESSFUL";
      public static LocString CONVERT_ERROR = (LocString) $"Converting the colony {UI.PRE_KEYWORD}{{Colony}}{UI.PST_KEYWORD} was unsuccessful!\nThe error was:\n\n<b>{{Error}}</b>\n\nPlease try again, or post a bug in the forums if this problem keeps happening.";
      public static LocString CONVERT_TO_CLOUD = (LocString) "CONVERT TO CLOUD SAVES";
      public static LocString CONVERT_TO_LOCAL = (LocString) "CONVERT TO LOCAL SAVES";
      public static LocString CONVERT_COLONY_TO_CLOUD = (LocString) "Convert colony to use cloud saves";
      public static LocString CONVERT_COLONY_TO_LOCAL = (LocString) "Convert to colony to use local saves";
      public static LocString CONVERT_ALL_TO_CLOUD = (LocString) "Convert <b>all</b> colonies below to use cloud saves";
      public static LocString CONVERT_ALL_TO_LOCAL = (LocString) "Convert <b>all</b> colonies below to use local saves";
      public static LocString CONVERT_ALL_TO_CLOUD_SUCCESS = (LocString) $"{UI.PRE_KEYWORD}SUCCESS!{UI.PST_KEYWORD}\nAll existing colonies have been converted into {UI.PRE_KEYWORD}cloud{UI.PST_KEYWORD} saves.\nNew colonies will use {UI.PRE_KEYWORD}cloud{UI.PST_KEYWORD} saves by default.\n\n{{Client}} may take longer than usual to sync the next time you exit the game as a result of this change.";
      public static LocString CONVERT_ALL_TO_LOCAL_SUCCESS = (LocString) $"{UI.PRE_KEYWORD}SUCCESS!{UI.PST_KEYWORD}\nAll existing colonies have been converted into {UI.PRE_KEYWORD}local{UI.PST_KEYWORD} saves.\nNew colonies will use {UI.PRE_KEYWORD}local{UI.PST_KEYWORD} saves by default.\n\n{{Client}} may take longer than usual to sync the next time you exit the game as a result of this change.";
      public static LocString CONVERT_TO_CLOUD_DETAILS = (LocString) "Converting a colony to use cloud saves will move all of the save files for that colony into the cloud saves folder.\n\nThis allows your game platform to sync this colony to the cloud for your account, so it can be played on multiple machines.";
      public static LocString CONVERT_TO_LOCAL_DETAILS = (LocString) $"Converting a colony to NOT use cloud saves will move all of the save files for that colony into the local saves folder.\n\n{UI.PRE_KEYWORD}These save files will no longer be synced to the cloud.{UI.PST_KEYWORD}";
      public static LocString OPEN_SAVE_FOLDER = (LocString) "LOCAL SAVES";
      public static LocString OPEN_CLOUDSAVE_FOLDER = (LocString) "CLOUD SAVES";
      public static LocString MIGRATE_TITLE = (LocString) "SAVE FILE MIGRATION";
      public static LocString MIGRATE_SAVE_FILES = (LocString) "MIGRATE SAVE FILES";
      public static LocString MIGRATE_COUNT = (LocString) $"\nFound {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} saves and {UI.PRE_KEYWORD}{{1}}{UI.PST_KEYWORD} autosaves that require migration.";
      public static LocString MIGRATE_RESULT = (LocString) $"{UI.PRE_KEYWORD}SUCCESS!{UI.PST_KEYWORD}\nMigration moved {UI.PRE_KEYWORD}{{0}}/{{1}}{UI.PST_KEYWORD} saves and {UI.PRE_KEYWORD}{{2}}/{{3}}{UI.PST_KEYWORD} autosaves{UI.PST_KEYWORD}.";
      public static LocString MIGRATE_RESULT_FAILURES = (LocString) $"{UI.PRE_KEYWORD}<b>WARNING:</b> Not all saves could be migrated.{UI.PST_KEYWORD}\nMigration moved {UI.PRE_KEYWORD}{{0}}/{{1}}{UI.PST_KEYWORD} saves and {UI.PRE_KEYWORD}{{2}}/{{3}}{UI.PST_KEYWORD} autosaves.\n\nThe file {UI.PRE_KEYWORD}{{ErrorColony}}{UI.PST_KEYWORD} encountered this error:\n\n<b>{{ErrorMessage}}</b>";
      public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE = (LocString) "MIGRATION INCOMPLETE";
      public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_PRE = (LocString) "<b>The game was unable to move all save files to their new location.\nTo fix this, please:</b>\n\n";
      public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1 = (LocString) "    1. Try temporarily disabling virus scanners and malware\n         protection programs.";
      public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2 = (LocString) "    2. Turn off file sync services such as OneDrive and DropBox.";
      public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3 = (LocString) "    3. Restart the game to retry file migration.";
      public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_POST = (LocString) "\n<b>If this still doesn't solve the problem, please post a bug in the forums and we will attempt to assist with your issue.</b>";
      public static LocString MIGRATE_INFO = (LocString) $"We've changed how save files are organized!\nPlease {UI.CLICK(UI.ClickType.click)} the button below to automatically update your save file storage.";
      public static LocString MIGRATE_DONE = (LocString) "CONTINUE";
      public static LocString MIGRATE_FAILURES_FORUM_BUTTON = (LocString) "VISIT FORUMS";
      public static LocString MIGRATE_FAILURES_DONE = (LocString) "MORE INFO";
      public static LocString CLOUD_TUTORIAL_BOUNCER = (LocString) "Upload Saves to Cloud";
    }

    public class SAVESCREEN
    {
      public static LocString TITLE = (LocString) "SAVE SLOTS";
      public static LocString NEWSAVEBUTTON = (LocString) "New Save";
      public static LocString OVERWRITEMESSAGE = (LocString) "Are you sure you want to overwrite {0}?";
      public static LocString SAVENAMETITLE = (LocString) "SAVE NAME";
      public static LocString CONFIRMNAME = (LocString) "Confirm";
      public static LocString CANCELNAME = (LocString) "Cancel";
      public static LocString IO_ERROR = (LocString) "An error occurred trying to save your game. Please ensure there is sufficient disk space.\n\n{0}";
      public static LocString REPORT_BUG = (LocString) "Report Bug";
      public static LocString SAVE_COMPLETE_MESSAGE = (LocString) "Save Complete";
    }

    public class RAILFORCEQUIT
    {
      public static LocString SAVE_EXIT = (LocString) "Play time has expired and the game is exiting. Would you like to overwrite {0}?";
      public static LocString WARN_EXIT = (LocString) "Play time has expired and the game will now exit.";
      public static LocString DLC_NOT_PURCHASED = (LocString) "The <i>Spaced Out!</i> DLC has not yet been purchased in the WeGame store. Purchase <i>Spaced Out!</i> to support <i>Oxygen Not Included</i> and enjoy the new content!";
    }

    public class MOD_ERRORS
    {
      public static LocString TITLE = (LocString) "MOD ERRORS";
      public static LocString DETAILS = (LocString) nameof (DETAILS);
      public static LocString CLOSE = (LocString) nameof (CLOSE);
    }

    public class MODS
    {
      public static LocString TITLE = (LocString) nameof (MODS);
      public static LocString TITLE_SAFE_MODE = (LocString) "MODS - SAFE MODE";
      public static LocString MANAGE = (LocString) "Subscription";
      public static LocString MANAGE_LOCAL = (LocString) "Browse";
      public static LocString WORKSHOP = (LocString) "STEAM WORKSHOP";
      public static LocString ENABLE_ALL = (LocString) "ENABLE ALL";
      public static LocString DISABLE_ALL = (LocString) "DISABLE ALL";
      public static LocString DRAG_TO_REORDER = (LocString) "Drag to reorder";
      public static LocString REQUIRES_RESTART = (LocString) "Mod changes require restart";
      public static LocString FAILED_TO_LOAD = (LocString) "A mod failed to load and is being disabled:\n\n{0}: {1}\n\n{2}";
      public static LocString DB_CORRUPT = (LocString) "An error occurred trying to load the Mod Database.\n\n{0}";
      public static LocString NO_DESCRIPTION = (LocString) "No description found in mod.yaml";
      public static LocString INSTALLED_VERSION_NEWER_THAN_LOADED_VERSION = (LocString) "Reinstall pending";

      public class CONTENT_FAILURE
      {
        public static LocString DISABLED_CONTENT = (LocString) " - <b>Incompatible DLC configuration</b>";
        public static LocString DISABLED_CONTENT_TOOLTIP = (LocString) "The current configuration of enabled/disabled DLCs does not match this mod's specifications.";
        public static LocString DISABLED_CONTENT_TOOLTIP_REQUIRED = (LocString) "<b>Required DLCs:</b>";
        public static LocString DISABLED_CONTENT_TOOLTIP_FORBIDDEN_DLC = (LocString) "<b>Forbidden DLCs:</b>";
        public static LocString NO_CONTENT = (LocString) " - <b>No compatible mod found</b>";
        public static LocString NO_CONTENT_TOOLTIP = (LocString) "No content was found to load.";
        public static LocString OLD_API = (LocString) " - <b>Mod out-of-date</b>";
        public static LocString OLD_API_TOOLTIP = (LocString) "This mod is outdated.";
      }

      public class TOOLTIPS
      {
        public static LocString ENABLED = (LocString) "Enabled";
        public static LocString DISABLED = (LocString) "Disabled";
        public static LocString MANAGE_STEAM_SUBSCRIPTION = (LocString) "Manage Steam Subscription";
        public static LocString MANAGE_RAIL_SUBSCRIPTION = (LocString) "Manage Subscription";
        public static LocString MANAGE_LOCAL_MOD = (LocString) "Manage Local Mod";
      }

      public class RAILMODUPLOAD
      {
        public static LocString TITLE = (LocString) "Upload Mod";
        public static LocString NAME = (LocString) "Mod Name";
        public static LocString DESCRIPTION = (LocString) "Mod Description";
        public static LocString VERSION = (LocString) "Version Number";
        public static LocString PREVIEW_IMAGE = (LocString) "Preview Image Path";
        public static LocString CONTENT_FOLDER = (LocString) "Content Folder Path";
        public static LocString SHARE_TYPE = (LocString) "Share Type";
        public static LocString SUBMIT = (LocString) "Submit";
        public static LocString SUBMIT_READY = (LocString) "This mod is ready to submit";
        public static LocString SUBMIT_NOT_READY = (LocString) "The mod cannot be submitted. Check that all fields are properly entered and that the paths are valid.";

        public static class MOD_SHARE_TYPE
        {
          public static LocString PRIVATE = (LocString) "Private";
          public static LocString TOOLTIP_PRIVATE = (LocString) "This mod will only be visible to its creator";
          public static LocString FRIEND = (LocString) "Friend";
          public static LocString TOOLTIP_FRIEND = (LocString) "Friend";
          public static LocString PUBLIC = (LocString) "Public";
          public static LocString TOOLTIP_PUBLIC = (LocString) "This mod will be available to all players after publishing. It may be subject to review before being allowed to be published.";
        }

        public static class MOD_UPLOAD_RESULT
        {
          public static LocString SUCCESS = (LocString) "Mod upload succeeded.";
          public static LocString FAILURE = (LocString) "Mod upload failed.";
        }
      }
    }

    public class MOD_EVENTS
    {
      public static LocString REQUIRED = (LocString) nameof (REQUIRED);
      public static LocString NOT_FOUND = (LocString) "NOT FOUND";
      public static LocString INSTALL_INFO_INACCESSIBLE = (LocString) "INACCESSIBLE";
      public static LocString OUT_OF_ORDER = (LocString) "ORDERING CHANGED";
      public static LocString ACTIVE_DURING_CRASH = (LocString) "ACTIVE DURING CRASH";
      public static LocString EXPECTED_ENABLED = (LocString) "NEWLY DISABLED";
      public static LocString EXPECTED_DISABLED = (LocString) "NEWLY ENABLED";
      public static LocString VERSION_UPDATE = (LocString) "VERSION UPDATE";
      public static LocString AVAILABLE_CONTENT_CHANGED = (LocString) "CONTENT CHANGED";
      public static LocString INSTALL_FAILED = (LocString) "INSTALL FAILED";
      public static LocString DOWNLOAD_FAILED = (LocString) "STEAM DOWNLOAD FAILED";
      public static LocString INSTALLED = (LocString) nameof (INSTALLED);
      public static LocString UNINSTALLED = (LocString) nameof (UNINSTALLED);
      public static LocString CANNOT_INSTALL = (LocString) "INSTALL FAILED";
      public static LocString REQUIRES_RESTART = (LocString) "RESTART REQUIRED";
      public static LocString BAD_WORLD_GEN = (LocString) "LOAD FAILED";
      public static LocString DEACTIVATED = (LocString) nameof (DEACTIVATED);
      public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = (LocString) nameof (DEACTIVATED);

      public class TOOLTIPS
      {
        public static LocString REQUIRED = (LocString) "The current save game couldn't load this mod. Unexpected things may happen!";
        public static LocString NOT_FOUND = (LocString) "This mod isn't installed";
        public static LocString INSTALL_INFO_INACCESSIBLE = (LocString) "Mod files are inaccessible";
        public static LocString OUT_OF_ORDER = (LocString) "Active mod has changed order with respect to some other active mod";
        public static LocString ACTIVE_DURING_CRASH = (LocString) "Mod was active during a crash and may be the cause";
        public static LocString EXPECTED_ENABLED = (LocString) "This mod needs to be enabled";
        public static LocString EXPECTED_DISABLED = (LocString) "This mod needs to be disabled";
        public static LocString VERSION_UPDATE = (LocString) "New version detected";
        public static LocString AVAILABLE_CONTENT_CHANGED = (LocString) "Content added or removed";
        public static LocString INSTALL_FAILED = (LocString) "Installation failed";
        public static LocString DOWNLOAD_FAILED = (LocString) "Steam failed to download the mod";
        public static LocString INSTALLED = (LocString) "Installation succeeded";
        public static LocString UNINSTALLED = (LocString) "Uninstalled";
        public static LocString CANNOT_INSTALL = (LocString) "Encountered an error while reading/writing to disk";
        public static LocString BAD_WORLD_GEN = (LocString) "Encountered an error while loading file";
        public static LocString DEACTIVATED = (LocString) "Deactivated due to errors";
        public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = (LocString) ("Deactivated due to Early Access for " + (string) UI.EXPANSION1.NAME_ITAL);
      }
    }

    public class MOD_DIALOGS
    {
      public static LocString ADDITIONAL_MOD_EVENTS = (LocString) "(...additional entries omitted)";

      public class INSTALL_INFO_INACCESSIBLE
      {
        public static LocString TITLE = (LocString) "STEAM CONTENT ERROR";
        public static LocString MESSAGE = (LocString) "Failed to access local Steam files for mod {0}.\nTry restarting Oxygen not Included.\nIf that doesn't work, try re-subscribing to the mod via Steam.";
      }

      public class STEAM_SUBSCRIBED
      {
        public static LocString TITLE = (LocString) "STEAM MOD SUBSCRIBED";
        public static LocString MESSAGE = (LocString) "Subscribed to Steam mod: {0}";
      }

      public class STEAM_UPDATED
      {
        public static LocString TITLE = (LocString) "STEAM MOD UPDATE";
        public static LocString MESSAGE = (LocString) "Updating version of Steam mod: {0}";
      }

      public class STEAM_UNSUBSCRIBED
      {
        public static LocString TITLE = (LocString) "STEAM MOD UNSUBSCRIBED";
        public static LocString MESSAGE = (LocString) "Unsubscribed from Steam mod: {0}";
      }

      public class STEAM_REFRESH
      {
        public static LocString TITLE = (LocString) "STEAM MODS REFRESHED";
        public static LocString MESSAGE = (LocString) "Refreshed Steam mods:\n{0}";
      }

      public class ALL_MODS_DISABLED_EARLY_ACCESS
      {
        public static LocString TITLE = (LocString) "ALL MODS DISABLED";
        public static LocString MESSAGE = (LocString) $"Mod support is temporarily suspended for the initial launch of {(string) UI.EXPANSION1.NAME_ITAL} into Early Access:\n{{0}}";
      }

      public class LOAD_FAILURE
      {
        public static LocString TITLE = (LocString) "LOAD FAILURE";
        public static LocString MESSAGE = (LocString) "Failed to load one or more mods:\n{0}\nThey will be re-installed when the game is restarted.\nGame may be unstable until restarted.";
      }

      public class SAVE_GAME_MODS_DIFFER
      {
        public static LocString TITLE = (LocString) "MOD DIFFERENCES";
        public static LocString MESSAGE = (LocString) "Save game mods differ from currently active mods:\n{0}";
      }

      public class MOD_ERRORS_ON_BOOT
      {
        public static LocString TITLE = (LocString) "MOD ERRORS";
        public static LocString MESSAGE = (LocString) "An error occurred during start-up with mods active.\nAll mods have been disabled to ensure a clean restart.\n{0}";
        public static LocString DEV_MESSAGE = (LocString) "An error occurred during start-up with mods active.\n{0}\nDisable all mods and restart, or continue in an unstable state?";
      }

      public class MODS_SCREEN_CHANGES
      {
        public static LocString TITLE = (LocString) "MODS CHANGED";
        public static LocString MESSAGE = (LocString) "{0}\nRestart required to reload mods.\nGame may be unstable until restarted.";
      }

      public class MOD_EVENTS
      {
        public static LocString TITLE = (LocString) "MOD EVENTS";
        public static LocString MESSAGE = (LocString) "{0}";
        public static LocString DEV_MESSAGE = (LocString) "{0}\nCheck Player.log for details.";
      }

      public class RESTART
      {
        public static LocString OK = (LocString) nameof (RESTART);
        public static LocString CANCEL = (LocString) "CONTINUE";
        public static LocString MESSAGE = (LocString) "{0}\nRestart required.";
        public static LocString DEV_MESSAGE = (LocString) "{0}\nRestart required.\nGame may be unstable until restarted.";
      }

      public class SAFE_MODE
      {
        public static LocString TITLE = (LocString) "MOD SAFE MODE";
        public static LocString MESSAGE = (LocString) "One or more mods caused the game to crash when launched. Mods will not be loaded until the game is restarted.\n\nPlease disable some mods and restart to see if that fixes the issue.";
      }
    }

    public class PAUSE_SCREEN
    {
      public static LocString TITLE = (LocString) "PAUSED";
      public static LocString RESUME = (LocString) "Resume";
      public static LocString LOGBOOK = (LocString) "Logbook";
      public static LocString OPTIONS = (LocString) "Options";
      public static LocString SAVE = (LocString) "Save";
      public static LocString ALREADY_SAVED = (LocString) "<i><color=#CAC8C8>Already Saved</color></i>";
      public static LocString SAVEAS = (LocString) "Save As";
      public static LocString COLONY_SUMMARY = (LocString) "Colony Summary";
      public static LocString LOCKERMENU = (LocString) "Supply Closet";
      public static LocString LOAD = (LocString) "Load";
      public static LocString QUIT = (LocString) "Main Menu";
      public static LocString DESKTOPQUIT = (LocString) "Quit to Desktop";
      public static LocString WORLD_SEED = (LocString) "Coordinates: {0}";
      public static LocString WORLD_SEED_TOOLTIP = (LocString) "Share coordinates with a friend and they can start a colony on an identical asteroid!\n\n{0} - The asteroid\n\n{1} - The world seed\n\n{2} - Difficulty and Custom settings\n\n{3} - Story Trait settings\n\n{4} - Scramble DLC settings";
      public static LocString WORLD_SEED_COPY_TOOLTIP = (LocString) "Copy Coordinates to clipboard\n\nShare coordinates with a friend and they can start a colony on an identical asteroid!";
      public static LocString MANAGEMENT_BUTTON = (LocString) "Pause Menu";

      public class ADD_DLC_MENU
      {
        public static LocString ENABLE_QUESTION = (LocString) "Enable DLC content on this save?\n\nThis will create a new copy of the save game. It will no longer be possible to load this copy without the DLC enabled.";
        public static LocString CONFIRM = (LocString) nameof (CONFIRM);
        public static LocString DLC_ENABLED_TOOLTIP = (LocString) "This save has content from <b>{0}</b> DLC enabled";
        public static LocString DLC_DISABLED_TOOLTIP = (LocString) "This save does not currently have content from <b>{0}</b> DLC enabled \n\n<b>Click to enable it</b>";
        public static LocString DLC_DISABLED_NOT_EDITABLE_TOOLTIP = (LocString) "This save does not have content from the <b>{0}</b> DLC enabled";
      }
    }

    public class OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "OPTIONS";
      public static LocString GRAPHICS = (LocString) "Graphics";
      public static LocString AUDIO = (LocString) "Audio";
      public static LocString GAME = (LocString) "Game";
      public static LocString CONTROLS = (LocString) "Controls";
      public static LocString UNITS = (LocString) "Temperature Units";
      public static LocString METRICS = (LocString) "Data Communication";
      public static LocString LANGUAGE = (LocString) "Change Language";
      public static LocString WORLD_GEN = (LocString) "World Generation Key";
      public static LocString RESET_TUTORIAL = (LocString) "Reset Tutorial Messages";
      public static LocString RESET_TUTORIAL_WARNING = (LocString) "All tutorial messages will be reset, and\nwill show up again the next time you play the game.";
      public static LocString FEEDBACK = (LocString) "Feedback";
      public static LocString CREDITS = (LocString) "Credits";
      public static LocString BACK = (LocString) "Done";
      public static LocString UNLOCK_SANDBOX = (LocString) "Unlock Sandbox Mode";
      public static LocString MODS = (LocString) nameof (MODS);
      public static LocString SAVE_OPTIONS = (LocString) "Save Options";

      public class TOGGLE_SANDBOX_SCREEN
      {
        public static LocString UNLOCK_SANDBOX_WARNING = (LocString) "Sandbox Mode will be enabled for this save file";
        public static LocString CONFIRM = (LocString) "Enable Sandbox Mode";
        public static LocString CANCEL = (LocString) "Cancel";
        public static LocString CONFIRM_SAVE_BACKUP = (LocString) "Enable Sandbox Mode, but save a backup first";
        public static LocString BACKUP_SAVE_GAME_APPEND = (LocString) " (BACKUP)";
      }
    }

    public class INPUT_BINDINGS_SCREEN
    {
      public static LocString TITLE = (LocString) "CUSTOMIZE KEYS";
      public static LocString RESET = (LocString) "Reset";
      public static LocString APPLY = (LocString) "Done";
      public static LocString DUPLICATE = (LocString) "{0} was already bound to {1} and is now unbound.";
      public static LocString UNBOUND_ACTION = (LocString) "{0} is unbound. Are you sure you want to continue?";
      public static LocString MULTIPLE_UNBOUND_ACTIONS = (LocString) "You have multiple unbound actions, this may result in difficulty playing the game. Are you sure you want to continue?";
      public static LocString WAITING_FOR_INPUT = (LocString) "???";
    }

    public class TRANSLATIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "TRANSLATIONS";
      public static LocString UNINSTALL = (LocString) "Uninstall";
      public static LocString PREINSTALLED_HEADER = (LocString) "Preinstalled Language Packs";
      public static LocString UGC_HEADER = (LocString) "Subscribed Workshop Language Packs";
      public static LocString UGC_MOD_TITLE_FORMAT = (LocString) "{0} (workshop)";
      public static LocString ARE_YOU_SURE = (LocString) "Are you sure you want to uninstall this language pack?";
      public static LocString PLEASE_REBOOT = (LocString) "Please restart your game for these changes to take effect.";
      public static LocString NO_PACKS = (LocString) "Steam Workshop";
      public static LocString DOWNLOAD = (LocString) "Start Download";
      public static LocString INSTALL = (LocString) "Install";
      public static LocString INSTALLED = (LocString) "Installed";
      public static LocString NO_STEAM = (LocString) "Unable to retrieve language list from Steam";
      public static LocString RESTART = (LocString) nameof (RESTART);
      public static LocString CANCEL = (LocString) nameof (CANCEL);
      public static LocString MISSING_LANGUAGE_PACK = (LocString) "Selected language pack ({0}) not found.\nReverting to default language.";
      public static LocString UNKNOWN = (LocString) "Unknown";

      public class PREINSTALLED_LANGUAGES
      {
        public static LocString EN = (LocString) "English (Klei)";
        public static LocString ZH_KLEI = (LocString) "Chinese (Klei)";
        public static LocString KO_KLEI = (LocString) "Korean (Klei)";
        public static LocString RU_KLEI = (LocString) "Russian (Klei)";
      }
    }

    public class SCENARIOS_MENU
    {
      public static LocString TITLE = (LocString) "Scenarios";
      public static LocString UNSUBSCRIBE = (LocString) "Unsubscribe";
      public static LocString UNSUBSCRIBE_CONFIRM = (LocString) "Are you sure you want to unsubscribe from this scenario?";
      public static LocString LOAD_SCENARIO_CONFIRM = (LocString) "Load the \"{SCENARIO_NAME}\" scenario?";
      public static LocString LOAD_CONFIRM_TITLE = (LocString) "LOAD";
      public static LocString SCENARIO_NAME = (LocString) "Name:";
      public static LocString SCENARIO_DESCRIPTION = (LocString) "Description";
      public static LocString BUTTON_DONE = (LocString) "Done";
      public static LocString BUTTON_LOAD = (LocString) "Load";
      public static LocString BUTTON_WORKSHOP = (LocString) "Steam Workshop";
      public static LocString NO_SCENARIOS_AVAILABLE = (LocString) "No scenarios available.\n\nSubscribe to some in the Steam Workshop.";
    }

    public class AUDIO_OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "AUDIO OPTIONS";
      public static LocString HEADER_VOLUME = (LocString) "VOLUME";
      public static LocString HEADER_SETTINGS = (LocString) "SETTINGS";
      public static LocString DONE_BUTTON = (LocString) "Done";
      public static LocString MUSIC_EVERY_CYCLE = (LocString) "Play background music each morning";
      public static LocString MUSIC_EVERY_CYCLE_TOOLTIP = (LocString) "If enabled, background music will play every cycle instead of every few cycles";
      public static LocString AUTOMATION_SOUNDS_ALWAYS = (LocString) "Always play automation sounds";
      public static LocString AUTOMATION_SOUNDS_ALWAYS_TOOLTIP = (LocString) ("If enabled, automation sound effects will play even when outside of the " + UI.FormatAsOverlay("Automation Overlay"));
      public static LocString MUTE_ON_FOCUS_LOST = (LocString) "Mute when unfocused";
      public static LocString MUTE_ON_FOCUS_LOST_TOOLTIP = (LocString) "If enabled, the game will be muted while minimized or if the application loses focus";
      public static LocString AUDIO_BUS_MASTER = (LocString) "Master";
      public static LocString AUDIO_BUS_SFX = (LocString) "SFX";
      public static LocString AUDIO_BUS_MUSIC = (LocString) "Music";
      public static LocString AUDIO_BUS_AMBIENCE = (LocString) "Ambience";
      public static LocString AUDIO_BUS_UI = (LocString) nameof (UI);
    }

    public class GAME_OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "GAME OPTIONS";
      public static LocString GENERAL_GAME_OPTIONS = (LocString) "GENERAL";
      public static LocString DISABLED_WARNING = (LocString) "More options available in-game";
      public static LocString DEFAULT_TO_CLOUD_SAVES = (LocString) "Default to cloud saves";
      public static LocString DEFAULT_TO_CLOUD_SAVES_TOOLTIP = (LocString) "When a new colony is created, this controls whether it will be saved into the cloud saves folder for syncing or not.";
      public static LocString RESET_TUTORIAL_DESCRIPTION = (LocString) "Mark all tutorial messages \"unread\"";
      public static LocString SANDBOX_DESCRIPTION = (LocString) "Enable sandbox tools";
      public static LocString CONTROLS_DESCRIPTION = (LocString) "Change key bindings";
      public static LocString TEMPERATURE_UNITS = (LocString) "TEMPERATURE UNITS";
      public static LocString SAVE_OPTIONS = (LocString) "SAVE";
      public static LocString CAMERA_SPEED_LABEL = (LocString) "Camera Pan Speed: {0}%";
    }

    public class METRIC_OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "DATA COMMUNICATION";
      public static LocString HEADER_METRICS = (LocString) "USER DATA";
      public static LocString USER_ACCOUNT_LINK = (LocString) "View your Klei account online";
    }

    public class COLONY_SAVE_OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "COLONY SAVE OPTIONS";
      public static LocString DESCRIPTION = (LocString) "Note: These values are configured per save file";
      public static LocString AUTOSAVE_FREQUENCY = (LocString) "Autosave frequency:";
      public static LocString AUTOSAVE_FREQUENCY_DESCRIPTION = (LocString) "Every: {0} cycle(s)";
      public static LocString AUTOSAVE_NEVER = (LocString) "Never";
      public static LocString TIMELAPSE_RESOLUTION = (LocString) "Timelapse resolution:";
      public static LocString TIMELAPSE_RESOLUTION_DESCRIPTION = (LocString) "{0}x{1}";
      public static LocString TIMELAPSE_DISABLED_DESCRIPTION = (LocString) "Disabled";
    }

    public class FEEDBACK_SCREEN
    {
      public static LocString TITLE = (LocString) "FEEDBACK";
      public static LocString HEADER = (LocString) "We would love to hear from you!";
      public static LocString DESCRIPTION = (LocString) "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file. The buttons to the right will help you find those files on your local drive.\n\nThank you for being part of the Oxygen Not Included community!";
      public static LocString ALT_DESCRIPTION = (LocString) "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file.\n\nThank you for being part of the Oxygen Not Included community!";
      public static LocString BUG_FORUMS_BUTTON = (LocString) "Report a Bug";
      public static LocString SUGGESTION_FORUMS_BUTTON = (LocString) "Suggestions Forum";
      public static LocString LOGS_DIRECTORY_BUTTON = (LocString) "Browse Log Files";
      public static LocString SAVE_FILES_DIRECTORY_BUTTON = (LocString) "Browse Save Files";
    }

    public class WORLD_GEN_OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "WORLD GENERATION OPTIONS";
      public static LocString USE_SEED = (LocString) "Set Worldgen Seed";
      public static LocString DONE_BUTTON = (LocString) "Done";
      public static LocString RANDOM_BUTTON = (LocString) "Randomize";
      public static LocString RANDOM_BUTTON_TOOLTIP = (LocString) "Randomize a new worldgen seed";
      public static LocString TOOLTIP = (LocString) "This will override the current worldgen seed";
    }

    public class METRICS_OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "DATA COMMUNICATION OPTIONS";
      public static LocString ENABLE_BUTTON = (LocString) "Enable Data Communication";
      public static LocString DESCRIPTION = (LocString) "Collecting user data helps us improve the game.\n\nPlayers who opt out of data communication will no longer send us crash reports and play data.\n\nThey will also be unable to receive new item unlocks from our servers, though existing unlocked items will continue to function.\n\nFor more details on our privacy policy and how we use the data we collect, please visit our <color=#ECA6C9><u><b>privacy center</b></u></color>.";
      public static LocString DONE_BUTTON = (LocString) "Done";
      public static LocString RESTART_BUTTON = (LocString) "Restart Game";
      public static LocString TOOLTIP = (LocString) "Uncheck to disable data communication";
      public static LocString RESTART_WARNING = (LocString) "A game restart is required to apply settings.";
    }

    public class UNIT_OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "TEMPERATURE UNITS";
      public static LocString CELSIUS = (LocString) "Celsius";
      public static LocString CELSIUS_TOOLTIP = (LocString) "Change temperature unit to Celsius (°C)";
      public static LocString KELVIN = (LocString) "Kelvin";
      public static LocString KELVIN_TOOLTIP = (LocString) "Change temperature unit to Kelvin (K)";
      public static LocString FAHRENHEIT = (LocString) "Fahrenheit";
      public static LocString FAHRENHEIT_TOOLTIP = (LocString) "Change temperature unit to Fahrenheit (°F)";
    }

    public class GRAPHICS_OPTIONS_SCREEN
    {
      public static LocString TITLE = (LocString) "GRAPHICS OPTIONS";
      public static LocString FULLSCREEN = (LocString) "Fullscreen";
      public static LocString RESOLUTION = (LocString) "Resolution:";
      public static LocString LOWRES = (LocString) "Low Resolution Textures";
      public static LocString APPLYBUTTON = (LocString) "Apply";
      public static LocString REVERTBUTTON = (LocString) "Revert";
      public static LocString DONE_BUTTON = (LocString) "Done";
      public static LocString UI_SCALE = (LocString) "UI Scale";
      public static LocString HEADER_DISPLAY = (LocString) "DISPLAY";
      public static LocString HEADER_UI = (LocString) "INTERFACE";
      public static LocString COLORMODE = (LocString) "Color Mode:";
      public static LocString COLOR_MODE_DEFAULT = (LocString) "Default";
      public static LocString COLOR_MODE_PROTANOPIA = (LocString) "Protanopia";
      public static LocString COLOR_MODE_DEUTERANOPIA = (LocString) "Deuteranopia";
      public static LocString COLOR_MODE_TRITANOPIA = (LocString) "Tritanopia";
      public static LocString ACCEPT_CHANGES = (LocString) "Accept Changes?";
      public static LocString ACCEPT_CHANGES_STRING_COLOR = (LocString) "Interface changes will be visible immediately, but applying color changes to in-game text will require a restart.\n\nAccept Changes?";
      public static LocString COLORBLIND_FEEDBACK = (LocString) "Color blindness options are currently in progress.\n\nIf you would benefit from an alternative color mode or have had difficulties with any of the default colors, please visit the forums and let us know about your experiences.\n\nYour feedback is extremely helpful to us!";
      public static LocString COLORBLIND_FEEDBACK_BUTTON = (LocString) "Provide Feedback";
    }

    public class WORLDGENSCREEN
    {
      public static LocString TITLE = (LocString) "NEW GAME";
      public static LocString GENERATINGWORLD = (LocString) "GENERATING WORLD";
      public static LocString SELECTSIZEPROMPT = (LocString) "A new world is about to be created. Please select its size.";
      public static LocString LOADINGGAME = (LocString) "LOADING WORLD...";

      public class SIZES
      {
        public static LocString TINY = (LocString) "Tiny";
        public static LocString SMALL = (LocString) "Small";
        public static LocString STANDARD = (LocString) "Standard";
        public static LocString LARGE = (LocString) "Big";
        public static LocString HUGE = (LocString) "Colossal";
      }
    }

    public class MINSPECSCREEN
    {
      public static LocString TITLE = (LocString) "WARNING!";
      public static LocString SIMFAILEDTOLOAD = (LocString) "A problem occurred loading Oxygen Not Included. This is usually caused by the Visual Studio C++ 2015 runtime being improperly installed on the system. Please exit the game, run Windows Update, and try re-launching Oxygen Not Included.";
      public static LocString BODY = (LocString) "We've detected that this computer does not meet the minimum requirements to run Oxygen Not Included. While you may continue with your current specs, the game might not run smoothly for you.\n\nPlease be aware that your experience may suffer as a result.";
      public static LocString OKBUTTON = (LocString) "Okay, thanks!";
      public static LocString QUITBUTTON = (LocString) "Quit";
    }

    public class SUPPORTWARNINGS
    {
      public static LocString AUDIO_DRIVERS = (LocString) "A problem occurred initializing your audio device.\nSorry about that!\n\nThis is usually caused by outdated audio drivers.\n\nPlease visit your audio device manufacturer's website to download the latest drivers.";
      public static LocString AUDIO_DRIVERS_MORE_INFO = (LocString) "More Info";
      public static LocString DUPLICATE_KEY_BINDINGS = (LocString) "<b>Duplicate key bindings were detected.\nThis may be because your custom key bindings conflicted with a new feature's default key.\nPlease visit the controls screen to ensure your key bindings are set how you like them.</b>\n{0}";
      public static LocString SAVE_DIRECTORY_READ_ONLY = (LocString) "A problem occurred while accessing your save directory.\nThis may be because your directory is set to read-only.\n\nPlease ensure your save directory is readable as well as writable and re-launch the game.\n{0}";
      public static LocString SAVE_DIRECTORY_INSUFFICIENT_SPACE = (LocString) "There is insufficient disk space to write to your save directory.\n\nPlease free at least 15 MB to give your saves some room to breathe.\n{0}";
      public static LocString WORLD_GEN_FILES = (LocString) "A problem occurred while accessing certain game files that will prevent starting new games.\n\nPlease ensure that the directory and files are readable as well as writable and re-launch the game:\n\n{0}";
      public static LocString WORLD_GEN_FAILURE = (LocString) "A problem occurred while generating a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with a different seed.";
      public static LocString WORLD_GEN_FAILURE_MIXING = (LocString) "A problem occurred while trying to mix a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with different remix settings or a different seed.";
      public static LocString WORLD_GEN_FAILURE_STORY = (LocString) "A problem occurred while generating a world from this seed:\n{0}.\n\nNot all story traits were able to be placed. Please try again with a different seed or fewer story traits.";
      public static LocString PLAYER_PREFS_CORRUPTED = (LocString) "A problem occurred while loading your game options.\nThey have been reset to their default settings.\n\n";
      public static LocString IO_UNAUTHORIZED = (LocString) "An Unauthorized Access Error occurred when trying to write to disk.\n\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";
      public static LocString IO_UNAUTHORIZED_ONEDRIVE = (LocString) "An Unauthorized Access Error occurred when trying to write to disk.\n\nOneDrive may be interfering with the game.\n\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";
      public static LocString IO_SUFFICIENT_SPACE = (LocString) "An Insufficient Space Error occurred when trying to write to disk. \n\nPlease free up some space.\n{0}";
      public static LocString IO_UNKNOWN = (LocString) "An unknown error occurred when trying to write or access a file.\n{0}";
      public static LocString MORE_INFO_BUTTON = (LocString) "More Info";
    }

    public class SAVEUPGRADEWARNINGS
    {
      public static LocString SUDDENMORALEHELPER_TITLE = (LocString) "MORALE CHANGES";
      public static LocString SUDDENMORALEHELPER = (LocString) "Welcome to the Expressive Upgrade! This update introduces a new Morale system that replaces Food and Decor Expectations that were found in previous versions of the game.\n\nThe game you are trying to load was created before this system was introduced, and will need to be updated. You may either:\n\n\n1) Enable the new Morale system in this save, removing Food and Decor Expectations. It's possible that when you load your save your old colony won't meet your Duplicants' new Morale needs, so they'll receive a 5 cycle Morale boost to give you time to adjust.\n\n2) Disable Morale in this save. The new Morale mechanics will still be visible, but won't affect your Duplicants' stress. Food and Decor expectations will no longer exist in this save.";
      public static LocString SUDDENMORALEHELPER_BUFF = (LocString) "1) Bring on Morale!";
      public static LocString SUDDENMORALEHELPER_DISABLE = (LocString) "2) Disable Morale";
      public static LocString NEWAUTOMATIONWARNING_TITLE = (LocString) "AUTOMATION CHANGES";
      public static LocString NEWAUTOMATIONWARNING = (LocString) $"The following buildings have acquired new automation ports!\n\nTake a moment to check whether these buildings in your colony are now unintentionally connected to existing {(string) BUILDINGS.PREFABS.LOGICWIRE.NAME}s.";
      public static LocString MERGEDOWNCHANGES_TITLE = (LocString) "BREATH OF FRESH AIR UPDATE CHANGES";
      public static LocString MERGEDOWNCHANGES = (LocString) "Oxygen Not Included has had a <b>major update</b> since this save file was created! In addition to the <b>multitude of bug fixes and quality-of-life features</b>, please pay attention to these changes which may affect your existing colony:";
      public static LocString MERGEDOWNCHANGES_FOOD = (LocString) "•<indent=20px>Fridges are more effective for early-game food storage</indent>\n•<indent=20px><b>Both</b> freezing temperatures and a sterile gas are needed for <b>total food preservation</b>.</indent>";
      public static LocString MERGEDOWNCHANGES_AIRFILTER = (LocString) $"•<indent=20px>{(string) BUILDINGS.PREFABS.AIRFILTER.NAME} now requires <b>5w Power</b>.</indent>\n•<indent=20px>Duplicants will get <b>Stinging Eyes</b> from gases such as chlorine and hydrogen.</indent>";
      public static LocString MERGEDOWNCHANGES_SIMULATION = (LocString) $"•<indent=20px>Many <b>simulation bugs</b> have been fixed.</indent>\n•<indent=20px>This may <b>change the effectiveness</b> of certain contraptions and {(string) BUILDINGS.PREFABS.STEAMTURBINE2.NAME} setups.</indent>";
      public static LocString MERGEDOWNCHANGES_BUILDINGS = (LocString) $"•<indent=20px>The <b>{(string) BUILDINGS.PREFABS.OXYGENMASKSTATION.NAME}</b> has been added to aid early-game exploration.</indent>\n•<indent=20px>Use the new <b>Meter Valves</b> for precise control of resources in pipes.</indent>";
      public static LocString SPACESCANNERANDTELESCOPECHANGES_TITLE = (LocString) "JUNE 2023 QoL UPDATE CHANGES";
      public static LocString SPACESCANNERANDTELESCOPECHANGES_SUMMARY = (LocString) "There have been significant changes to <b>Space Scanners</b> and <b>Telescopes</b> since this save file was created!\n\nMeteor showers have been disabled for 20 cycles to provide time to adapt.";
      public static LocString SPACESCANNERANDTELESCOPECHANGES_WARNING = (LocString) "Please note these changes which may affect your existing colony:\n\n";
      public static LocString SPACESCANNERANDTELESCOPECHANGES_SPACESCANNERS = (LocString) "•<indent=20px>Automation is synced between all Space Scanners targeting the same object.</indent>\n•<indent=20px>Network quality based on the total percentage of sky covered.</indent>\n•<indent=20px>Industrial machinery no longer impacts network quality.</indent>";
      public static LocString SPACESCANNERANDTELESCOPECHANGES_TELESCOPES = (LocString) "•<indent=20px>Telescopes have a symmetrical scanning range.</indent>\n•<indent=20px>Obstructions block visibility from the blocked tile out toward the outer edge of scanning range.</indent>";
      public static LocString U50_CHANGES_TITLE = (LocString) "IMPORTANT CHANGES";
      public static LocString U50_CHANGES_SUMMARY = (LocString) "There have been significant changes to critters since this save file was created! Please check on your ranches.";
      public static LocString U50_CHANGES_MOOD = (LocString) "•<indent=20px>Critter moods have been expanded to include miserable and satisfied states: Miserable stops reproduction. Satisfied gives full metabolism and default reproduction.</indent>";
      public static LocString U50_CHANGES_PACU = (LocString) "•<indent=20px>Pacus have received a number of bug fixes and changes affecting their reproduction: Now correctly Confined when flopping or in less than 8 tiles of liquid. Easier to feed due to a rebalanced diet.</indent>";
      public static LocString U50_CHANGES_SUITCHECKPOINTS = (LocString) $"•<indent=20px>Suit checkpoints now have an automation port to disable them so Duplicants can pass through. Some checkpoints may now be unintentionally connected to existing {(string) BUILDINGS.PREFABS.LOGICWIRE.NAME}s.";
      public static LocString U50_CHANGES_METER_VALVES = (LocString) "•<indent=20px>Meter valves no longer continuously reset when receiving a green signal.</indent>";
    }
  }

  public class SANDBOX_TOGGLE
  {
    public static LocString TOOLTIP_LOCKED = (LocString) "<b>Sandbox Mode</b> must be unlocked in the options menu before it can be used. {Hotkey}";
    public static LocString TOOLTIP_UNLOCKED = (LocString) "Toggle <b>Sandbox Mode</b> {Hotkey}";
  }

  public class SKILLS_SCREEN
  {
    public static LocString CURRENT_MORALE = (LocString) "Current Morale: {0}\nMorale Need: {1}";
    public static LocString SORT_BY_DUPLICANT = (LocString) "Duplicants";
    public static LocString SORT_BY_MORALE = (LocString) "Morale";
    public static LocString SORT_BY_EXPERIENCE = (LocString) "Skill Points";
    public static LocString SORT_BY_SKILL_AVAILABLE = (LocString) "Skill Points";
    public static LocString SORT_BY_HAT = (LocString) "Hat";
    public static LocString SELECT_HAT = (LocString) "<b>SELECT HAT</b>";
    public static LocString POINTS_AVAILABLE = (LocString) "<b>SKILL POINTS AVAILABLE</b>";
    public static LocString MORALE = (LocString) "<b>Morale</b>";
    public static LocString MORALE_EXPECTATION = (LocString) "<b>Morale Need</b>";
    public static LocString EXPERIENCE = (LocString) "EXPERIENCE TO NEXT LEVEL";
    public static LocString EXPERIENCE_TOOLTIP = (LocString) "{0}exp to next Skill Point";
    public static LocString NOT_AVAILABLE = (LocString) "Not available";
    public static LocString ASSIGNED_BOOSTERS_HEADER = (LocString) "{0}'s Boosters";
    public static LocString ASSIGNED_BOOSTERS_COUNT_LABEL = (LocString) "{0}/{1} boosters assigned";
    public static LocString AVAILABLE_BOOSTERS_LABEL = (LocString) "{0} available";
    public static LocString ASSIGNED_BOOSTERS_LABEL = (LocString) "{0} assigned";
    public static LocString BIONIC_UPGRADE_SLOT_LOCKED = (LocString) $"This booster slot is unavailable\n\nBooster slots can be unlocked using {UI.PRE_KEYWORD}Skill Points{UI.PST_KEYWORD}";
    public static LocString BIONIC_UPGRADE_SLOT_AVAILABLE = (LocString) ("No booster installed\n\nAssign a booster from the menu below or craft new boosters at the " + UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"));
    public static LocString BIONIC_UPGRADE_SLOT_UNASSIGN = (LocString) (UI.CLICK(UI.ClickType.click) + " to unassign");

    public class ASSIGNMENT_REQUIREMENTS
    {
      public static LocString EXPECTATION_TARGET_SKILL = (LocString) "Current Morale: {0}\nSkill Morale Needs: {1}";
      public static LocString EXPECTATION_ALERT_TARGET_SKILL = (LocString) "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";
      public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = (LocString) "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

      public class SKILLGROUP_ENABLED
      {
        public static LocString NAME = (LocString) "Can perform {0}";
        public static LocString DESCRIPTION = (LocString) "Capable of performing <b>{0}</b> skills";
      }

      public class MASTERY
      {
        public static LocString CAN_MASTER = (LocString) "{0} <b>can learn</b> {1}";
        public static LocString HAS_MASTERED = (LocString) "{0} has <b>already learned</b> {1}";
        public static LocString CANNOT_MASTER = (LocString) "{0} <b>cannot learn</b> {1}";
        public static LocString STRESS_WARNING_MESSAGE = (LocString) $"Learning {{0}} will put {{1}} into a {UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD} deficit and cause unnecessary {UI.PRE_KEYWORD}Stress{UI.PST_KEYWORD}!";
        public static LocString REQUIRES_MORE_SKILL_POINTS = (LocString) $"    • Not enough {UI.PRE_KEYWORD}Skill Points{UI.PST_KEYWORD}";
        public static LocString REQUIRES_PREVIOUS_SKILLS = (LocString) $"    • Missing prerequisite {UI.PRE_KEYWORD}Skill{UI.PST_KEYWORD}";
        public static LocString PREVENTED_BY_TRAIT = (LocString) $"    • This Duplicant possesses the {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} Trait and cannot learn this Skill";
        public static LocString SKILL_APTITUDE = (LocString) $"{{0}} is interested in {{1}} and will receive a {UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD} bonus for learning it!";
        public static LocString SKILL_GRANTED = (LocString) $"{{0}} has been granted {{1}} by a Trait, but does not have increased {UI.FormatAsKeyWord("Morale Requirements")} from learning it";
      }
    }
  }

  public class KLEI_INVENTORY_SCREEN
  {
    public static LocString OPEN_INVENTORY_BUTTON = (LocString) "Open Klei Inventory";
    public static LocString ITEM_FACADE_FOR = (LocString) "This blueprint works with any {ConfigProperName}.";
    public static LocString ARTABLE_ITEM_FACADE_FOR = (LocString) "This blueprint works with any {ConfigProperName} of {ArtableQuality} quality.";
    public static LocString CLOTHING_ITEM_FACADE_FOR = (LocString) "This blueprint can be worn by any Duplicant.";
    public static LocString BALLOON_ARTIST_FACADE_FOR = (LocString) "This blueprint can be used by any Balloon Artist.";
    public static LocString MONUMENT_PART_FACADE_FOR = (LocString) "This blueprint can be used in any Great Monument.";
    public static LocString COLLECTION = (LocString) "Part of {Collection} collection.";
    public static LocString COLLECTION_THE = (LocString) "Part of the {Collection} collection.";
    public static LocString COLLECTION_COMING_SOON = (LocString) "Part of {Collection} collection. Coming soon!";
    public static LocString COLLECTION_COMING_SOON_THE = (LocString) "Part of the {Collection} collection. Coming soon!";
    public static LocString ITEM_RARITY_DETAILS = (LocString) "{RarityName} quality.";
    public static LocString ITEM_PLAYER_OWNED_AMOUNT = (LocString) "My colony has {OwnedCount} of these blueprints.";
    public static LocString ITEM_PLAYER_OWN_NONE = (LocString) "My colony doesn't have any of these yet.";
    public static LocString ITEM_PLAYER_OWNED_AMOUNT_ICON = (LocString) "x{OwnedCount}";
    public static LocString ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE = (LocString) "This blueprint is part of my colony's permanent collection.";
    public static LocString ITEM_DLC_REQUIRED = (LocString) "This blueprint is designed for the <i>Spaced Out!</i> DLC.";
    public static LocString ITEM_UNKNOWN_NAME = (LocString) "Uh oh!";
    public static LocString ITEM_UNKNOWN_DESCRIPTION = (LocString) "Hmm. Looks like this blueprint is missing from the supply closet. Perhaps due to a temporal anomaly...";
    public static LocString SEARCH_PLACEHOLDER = (LocString) "Search";
    public static LocString CLEAR_SEARCH_BUTTON_TOOLTIP = (LocString) "Clear search";
    public static LocString TOOLTIP_VIEW_ALL_ITEMS = (LocString) $"Filter: Showing all items\n\n{UI.CLICK(UI.ClickType.Click)} to toggle";
    public static LocString TOOLTIP_VIEW_OWNED_ONLY = (LocString) $"Filter: Showing owned items only\n\n{UI.CLICK(UI.ClickType.Click)} to toggle";
    public static LocString TOOLTIP_VIEW_DOUBLES_ONLY = (LocString) $"Filter: Showing multiples owned only\n\n{UI.CLICK(UI.ClickType.Click)} to toggle";
    public static LocString TOOLTIP_DLC_FILTER = (LocString) "{0}\n\nClick to filter displayed content by DLC Pack";
    public static LocString TOOLTIP_DLC_FILTER_ALL = (LocString) "<b>Showing all content</b>";

    public static class BARTERING
    {
      public static LocString TOOLTIP_ACTION_INVALID_OFFLINE = (LocString) "Currently unavailable";
      public static LocString BUY = (LocString) "PRINT";
      public static LocString TOOLTIP_BUY_ACTIVE = (LocString) "This item requires {0} spools of Filament to print";
      public static LocString TOOLTIP_UNBUYABLE = (LocString) "This item is unprintable";
      public static LocString TOOLTIP_UNBUYABLE_BETA = (LocString) "This item may be printable after the public testing period";
      public static LocString TOOLTIP_UNBUYABLE_ALREADY_OWNED = (LocString) "My colony already owns one of these blueprints";
      public static LocString TOOLTIP_BUY_CANT_AFFORD = (LocString) "Filament supply is too low";
      public static LocString SELL = (LocString) "RECYCLE";
      public static LocString TOOLTIP_SELL_ACTIVE = (LocString) "Recycle this blueprint for {0} spools of Filament";
      public static LocString TOOLTIP_UNSELLABLE = (LocString) "This item is non-recyclable";
      public static LocString TOOLTIP_NONE_TO_SELL = (LocString) "My colony does not own any of these blueprints";
      public static LocString CANCEL = (LocString) nameof (CANCEL);
      public static LocString CONFIRM_RECYCLE_HEADER = (LocString) "RECYCLE INTO FILAMENT?";
      public static LocString CONFIRM_PRINT_HEADER = (LocString) "PRINT ITEM?";
      public static LocString OFFLINE_LABEL = (LocString) "Not connected to Klei server";
      public static LocString LOADING = (LocString) "Connecting to server...";
      public static LocString TRANSACTION_ERROR = (LocString) "Whoops! Something's gone wrong.";
      public static LocString ACTION_DESCRIPTION_RECYCLE = (LocString) "Recycling this blueprint will recover Filament that my colony can use to print other items.\n\nOne copy of this blueprint will be removed from my colony's supply closet.";
      public static LocString ACTION_DESCRIPTION_PRINT = (LocString) "Producing this blueprint requires Filament from my colony's supply.\n\nOne copy of this blueprint will be extruded at a time.";
      public static LocString WALLET_TOOLTIP = (LocString) "{0} spool of Filament available";
      public static LocString WALLET_PLURAL_TOOLTIP = (LocString) "{0} spools of Filament available";
      public static LocString TRANSACTION_COMPLETE_HEADER = (LocString) "SUCCESS!";
      public static LocString TRANSACTION_INCOMPLETE_HEADER = (LocString) "ERROR";
      public static LocString PURCHASE_SUCCESS = (LocString) "One copy of this blueprint has been added to my colony's supply closet.";
      public static LocString SELL_SUCCESS = (LocString) "The Filament recovered from recycling this item can now be used to print other items.";
    }

    public static class CATEGORIES
    {
      public static LocString EQUIPMENT = (LocString) "Equipment";
      public static LocString DUPE_TOPS = (LocString) "Tops & Onesies";
      public static LocString DUPE_BOTTOMS = (LocString) "Bottoms";
      public static LocString DUPE_GLOVES = (LocString) "Gloves";
      public static LocString DUPE_SHOES = (LocString) "Footwear";
      public static LocString DUPE_HATS = (LocString) "Headgear";
      public static LocString DUPE_ACCESSORIES = (LocString) "Accessories";
      public static LocString ATMO_SUIT_HELMET = (LocString) "Atmo Helmets";
      public static LocString ATMO_SUIT_BODY = (LocString) "Atmo Suits";
      public static LocString ATMO_SUIT_GLOVES = (LocString) "Atmo Gloves";
      public static LocString ATMO_SUIT_BELT = (LocString) "Atmo Belts";
      public static LocString ATMO_SUIT_SHOES = (LocString) "Atmo Boots";
      public static LocString PRIMOGARB = (LocString) "Primo Garb";
      public static LocString ATMOSUITS = (LocString) "Atmo Suits";
      public static LocString JET_SUIT_BODY = (LocString) "Jet Suits";
      public static LocString JET_SUIT_HELMET = (LocString) "Jet Helmets";
      public static LocString JET_SUIT_GLOVES = (LocString) "Jet Gloves";
      public static LocString JET_SUIT_SHOES = (LocString) "Jet Boots";
      public static LocString BUILDINGS = (LocString) "Buildings";
      public static LocString CRITTERS = (LocString) "Critters";
      public static LocString SWEEPYS = (LocString) "Sweepys";
      public static LocString DUPLICANTS = (LocString) "Duplicants";
      public static LocString ARTWORKS = (LocString) "Artwork";
      public static LocString JOY_RESPONSE = (LocString) "Overjoyed Responses";

      public static class JOY_RESPONSES
      {
        public static LocString BALLOON_ARTIST = (LocString) "Balloon Artist";
      }
    }

    public static class TOP_LEVEL_CATEGORIES
    {
      public static LocString UNRELEASED = (LocString) "DEBUG UNRELEASED";
      public static LocString CLOTHING_TOPS = (LocString) "Tops & Onesies";
      public static LocString CLOTHING_BOTTOMS = (LocString) "Bottoms";
      public static LocString CLOTHING_GLOVES = (LocString) "Gloves";
      public static LocString CLOTHING_SHOES = (LocString) "Footwear";
      public static LocString ATMOSUITS = (LocString) "Atmo Suits";
      public static LocString JETSUITS = (LocString) "Jet Suits";
      public static LocString BUILDINGS = (LocString) "Buildings";
      public static LocString WALLPAPERS = (LocString) "Wallpapers";
      public static LocString ARTWORK = (LocString) "Artwork";
      public static LocString JOY_RESPONSES = (LocString) "Joy Responses";
    }

    public static class SUBCATEGORIES
    {
      public static LocString UNRELEASED = (LocString) "DEBUG UNRELEASED";
      public static LocString UNCATEGORIZED = (LocString) "BUG: UNCATEGORIZED";
      public static LocString YAML = (LocString) nameof (YAML);
      public static LocString DEFAULT = (LocString) "Default";
      public static LocString JOY_BALLOON = (LocString) "Balloons";
      public static LocString JOY_STICKER = (LocString) "Stickers";
      public static LocString PRIMO_GARB = (LocString) "Primo Garb";
      public static LocString CLOTHING_TOPS_BASIC = (LocString) "Standard Shirts";
      public static LocString CLOTHING_TOPS_TSHIRT = (LocString) "Tees";
      public static LocString CLOTHING_TOPS_FANCY = (LocString) "Specialty Tops";
      public static LocString CLOTHING_TOPS_JACKET = (LocString) "Jackets";
      public static LocString CLOTHING_TOPS_UNDERSHIRT = (LocString) "Undershirts";
      public static LocString CLOTHING_TOPS_DRESS = (LocString) "Dresses and Onesies";
      public static LocString CLOTHING_BOTTOMS_BASIC = (LocString) "Standard Pants";
      public static LocString CLOTHING_BOTTOMS_FANCY = (LocString) "Fancy Pants";
      public static LocString CLOTHING_BOTTOMS_SHORTS = (LocString) "Shorts";
      public static LocString CLOTHING_BOTTOMS_SKIRTS = (LocString) "Skirts";
      public static LocString CLOTHING_BOTTOMS_UNDERWEAR = (LocString) "Underwear";
      public static LocString CLOTHING_GLOVES_BASIC = (LocString) "Standard Gloves";
      public static LocString CLOTHING_GLOVES_FORMAL = (LocString) "Fancy Gloves";
      public static LocString CLOTHING_GLOVES_SHORT = (LocString) "Short Gloves";
      public static LocString CLOTHING_GLOVES_PRINTS = (LocString) "Specialty Gloves";
      public static LocString CLOTHING_SHOES_BASIC = (LocString) "Standard Shoes";
      public static LocString CLOTHING_SHOE_SOCKS = (LocString) "Socks";
      public static LocString CLOTHING_SHOES_FANCY = (LocString) "Fancy Shoes";
      public static LocString ATMOSUIT_HELMETS_BASIC = (LocString) "Atmo Helmets";
      public static LocString ATMOSUIT_HELMETS_FANCY = (LocString) "Fancy Atmo Helmets";
      public static LocString ATMOSUIT_BODIES_BASIC = (LocString) "Atmo Suits";
      public static LocString ATMOSUIT_BODIES_FANCY = (LocString) "Fancy Atmo Suits";
      public static LocString ATMOSUIT_GLOVES_BASIC = (LocString) "Atmo Gloves";
      public static LocString ATMOSUIT_GLOVES_FANCY = (LocString) "Fancy Atmo Gloves";
      public static LocString ATMOSUIT_BELTS_BASIC = (LocString) "Atmo Belts";
      public static LocString ATMOSUIT_BELTS_FANCY = (LocString) "Fancy Atmo Belts";
      public static LocString ATMOSUIT_SHOES_BASIC = (LocString) "Atmo Boots";
      public static LocString ATMOSUIT_SHOES_FANCY = (LocString) "Fancy Atmo Boots";
      public static LocString JETSUIT_HELMETS_BASIC = (LocString) "Jet Helmets";
      public static LocString JETSUIT_BODIES_BASIC = (LocString) "Jet Suits";
      public static LocString JETSUIT_GLOVES_BASIC = (LocString) "Jet Gloves";
      public static LocString JETSUIT_SHOES_BASIC = (LocString) "Jet Boots";
      public static LocString BUILDING_WALLPAPER_BASIC = (LocString) "Solid Wallpapers";
      public static LocString BUILDING_WALLPAPER_FANCY = (LocString) "Geometric Wallpapers";
      public static LocString BUILDING_WALLPAPER_PRINTS = (LocString) "Patterned Wallpapers";
      public static LocString BUILDING_CANVAS_STANDARD = (LocString) "Standard Canvas";
      public static LocString BUILDING_CANVAS_PORTRAIT = (LocString) "Portrait Canvas";
      public static LocString BUILDING_CANVAS_LANDSCAPE = (LocString) "Landscape Canvas";
      public static LocString BUILDING_SCULPTURE = (LocString) "Sculptures";
      public static LocString MONUMENT_BOTTOM = (LocString) "Monument Base";
      public static LocString MONUMENT_MIDDLE = (LocString) "Monument Midsection";
      public static LocString MONUMENT_TOP = (LocString) "Monument Top";
      public static LocString BUILDINGS_FLOWER_VASE = (LocString) "Pots and Planters";
      public static LocString BUILDINGS_BED_COT = (LocString) "Cots";
      public static LocString BUILDINGS_BED_LUXURY = (LocString) "Comfy Beds";
      public static LocString BUILDING_CEILING_LIGHT = (LocString) "Lights";
      public static LocString BUILDINGS_STORAGE = (LocString) "Storage";
      public static LocString BUILDINGS_INDUSTRIAL = (LocString) "Industrial";
      public static LocString BUILDINGS_FOOD = (LocString) "Cooking";
      public static LocString BUILDINGS_WASHROOM = (LocString) "Sanitation";
      public static LocString BUILDINGS_RANCHING = (LocString) "Agricultural";
      public static LocString BUILDINGS_RECREATION = (LocString) "Recreation and Decor";
      public static LocString BUILDINGS_PRINTING_POD = (LocString) "Printing Pods";
      public static LocString BUILDINGS_ELECTIC_WIRES = (LocString) "Electrical";
      public static LocString BUILDINGS_AUTOMATION = (LocString) "Automation";
      public static LocString BUILDINGS_RESEARCH = (LocString) "Research";
    }

    public static class COLUMN_HEADERS
    {
      public static LocString CATEGORY_HEADER = (LocString) "BLUEPRINTS";
      public static LocString ITEMS_HEADER = (LocString) "Items";
      public static LocString DETAILS_HEADER = (LocString) "Details";
    }
  }

  public class ITEM_DROP_SCREEN
  {
    public static LocString THANKS_FOR_PLAYING = (LocString) "New blueprints unlocked!";
    public static LocString WEB_REWARDS_AVAILABLE = (LocString) "Rewards available online!";
    public static LocString NOTHING_AVAILABLE = (LocString) "All available blueprints claimed";
    public static LocString OPEN_URL_BUTTON = (LocString) "CLAIM";
    public static LocString PRINT_ITEM_BUTTON = (LocString) "PRINT";
    public static LocString DISMISS_BUTTON = (LocString) "DISMISS";
    public static LocString ERROR_CANNOTLOADITEM = (LocString) "Whoops! Something's gone wrong.";
    public static LocString UNOPENED_ITEM_COUNT = (LocString) "{0} unclaimed blueprints";
    public static LocString UNOPENED_ITEM = (LocString) "{0} unclaimed blueprint";

    public static class IN_GAME_BUTTON
    {
      public static LocString TOOLTIP_ITEMS_AVAILABLE = (LocString) "Unlock new blueprints";
      public static LocString TOOLTIP_ERROR_NO_ITEMS = (LocString) "No new blueprints to unlock";
    }
  }

  public class OUTFIT_BROWSER_SCREEN
  {
    public static LocString BUTTON_ADD_OUTFIT = (LocString) "New Outfit";
    public static LocString BUTTON_PICK_OUTFIT = (LocString) "Assign Outfit";
    public static LocString TOOLTIP_PICK_OUTFIT_ERROR_LOCKED = (LocString) "Cannot assign this outfit to {MinionName} because my colony doesn't have all of these blueprints yet";
    public static LocString BUTTON_EDIT_OUTFIT = (LocString) "Restyle Outfit";
    public static LocString BUTTON_COPY_OUTFIT = (LocString) "Copy Outfit";
    public static LocString TOOLTIP_DELETE_OUTFIT = (LocString) "Delete Outfit";
    public static LocString TOOLTIP_DELETE_OUTFIT_ERROR_READONLY = (LocString) "This outfit cannot be deleted";
    public static LocString TOOLTIP_RENAME_OUTFIT = (LocString) "Rename Outfit";
    public static LocString TOOLTIP_RENAME_OUTFIT_ERROR_READONLY = (LocString) "This outfit cannot be renamed";
    public static LocString TOOLTIP_FILTER_BY_CLOTHING = (LocString) "View my Clothing Outfits";
    public static LocString TOOLTIP_FILTER_BY_ATMO_SUITS = (LocString) "View my Atmo Suit Outfits";
    public static LocString TOOLTIP_FILTER_BY_JET_SUITS = (LocString) "View my Jet Suit Outfits";

    public static class COLUMN_HEADERS
    {
      public static LocString GALLERY_HEADER = (LocString) "OUTFITS";
      public static LocString MINION_GALLERY_HEADER = (LocString) "WARDROBE";
      public static LocString DETAILS_HEADER = (LocString) "Preview";
    }

    public class DELETE_WARNING_POPUP
    {
      public static LocString HEADER = (LocString) "Delete \"{OutfitName}\"?";
      public static LocString BODY = (LocString) "Are you sure you want to delete \"{OutfitName}\"?\n\nAny Duplicants assigned to wear this outfit on spawn will be printed wearing their default outfit instead. Existing Duplicants in saved games won't be affected.\n\nThis <b>cannot</b> be undone.";
      public static LocString BUTTON_YES_DELETE = (LocString) "Yes, delete outfit";
      public static LocString BUTTON_DONT_DELETE = (LocString) "Cancel";
    }

    public class RENAME_POPUP
    {
      public static LocString HEADER = (LocString) "RENAME OUTFIT";
    }
  }

  public class LOCKER_MENU
  {
    public static LocString TITLE = (LocString) "SUPPLY CLOSET";
    public static LocString BUTTON_INVENTORY = (LocString) "All";
    public static LocString BUTTON_INVENTORY_DESCRIPTION = (LocString) "View all of my colony's blueprints";
    public static LocString BUTTON_DUPLICANTS = (LocString) "Duplicants";
    public static LocString BUTTON_DUPLICANTS_DESCRIPTION = (LocString) "Manage individual Duplicants' outfits";
    public static LocString BUTTON_OUTFITS = (LocString) "Wardrobe";
    public static LocString BUTTON_OUTFITS_DESCRIPTION = (LocString) "Manage my colony's collection of outfits";
    public static LocString DEFAULT_DESCRIPTION = (LocString) "Select a screen";
    public static LocString BUTTON_CLAIM = (LocString) "Claim Blueprints";
    public static LocString BUTTON_CLAIM_DESCRIPTION = (LocString) "Claim any available blueprints";
    public static LocString BUTTON_CLAIM_NONE_DESCRIPTION = (LocString) "All available blueprints claimed";
    public static LocString UNOPENED_ITEMS_TOOLTIP = (LocString) "New blueprints available";
    public static LocString UNOPENED_ITEMS_NONE_TOOLTIP = (LocString) "All available blueprints claimed";
    public static LocString OFFLINE_ICON_TOOLTIP = (LocString) "Not connected to Klei server";
    public static LocString OFFLINE_ICON_TOOLTIP_DATA_COLLECTIONS = (LocString) "Not connected to Klei server\n\nDisabled by privacy settings in Data Communication";
  }

  public class LOCKER_NAVIGATOR
  {
    public static LocString BUTTON_BACK = (LocString) "BACK";
    public static LocString BUTTON_CLOSE = (LocString) "CLOSE";

    public class DATA_COLLECTION_WARNING_POPUP
    {
      public static LocString HEADER = (LocString) "Data Communication is Disabled";
      public static LocString BODY = (LocString) "Data Communication must be enabled in order to access newly unlocked items. This setting can be found in the Options menu.\n\nExisting item unlocks can still be used while Data Communication is disabled.";
      public static LocString BUTTON_OK = (LocString) "Continue";
      public static LocString BUTTON_OPEN_SETTINGS = (LocString) "Options Menu";
    }
  }

  public class JOY_RESPONSE_DESIGNER_SCREEN
  {
    public static LocString CATEGORY_HEADER = (LocString) "OVERJOYED RESPONSES";
    public static LocString BUTTON_APPLY_TO_MINION = (LocString) "Assign to {MinionName}";
    public static LocString TOOLTIP_NO_FACADES_FOR_JOY_TRAIT = (LocString) "There aren't any blueprints for {JoyResponseType} Duplicants yet";
    public static LocString TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED = (LocString) "This Overjoyed Response blueprint cannot be assigned because my colony doesn't own it yet";

    public class CHANGES_NOT_SAVED_WARNING_POPUP
    {
      public static LocString HEADER = (LocString) "Discard changes to {MinionName}'s Overjoyed Response?";
    }
  }

  public class OUTFIT_DESIGNER_SCREEN
  {
    public static LocString CATEGORY_HEADER = (LocString) "CLOTHING";

    public class MINION_INSTANCE
    {
      public static LocString BUTTON_APPLY_TO_MINION = (LocString) "Assign to {MinionName}";
      public static LocString BUTTON_APPLY_TO_TEMPLATE = (LocString) "Apply to Template";

      public class APPLY_TEMPLATE_POPUP
      {
        public static LocString HEADER = (LocString) "SAVE AS TEMPLATE";
        public static LocString DESC_SAVE_EXISTING = (LocString) "\"{OutfitName}\" will be updated and applied to {MinionName} on save.";
        public static LocString DESC_SAVE_NEW = (LocString) "A new outfit named \"{OutfitName}\" will be created and assigned to {MinionName} on save.";
        public static LocString BUTTON_SAVE_EXISTING = (LocString) "Update Outfit";
        public static LocString BUTTON_SAVE_NEW = (LocString) "Save New Outfit";
      }
    }

    public class OUTFIT_TEMPLATE
    {
      public static LocString BUTTON_SAVE = (LocString) "Save Template";
      public static LocString BUTTON_COPY = (LocString) "Save a Copy";
      public static LocString TOOLTIP_SAVE_ERROR_LOCKED = (LocString) "Cannot save this outfit because my colony doesn't own all of its blueprints yet";
      public static LocString TOOLTIP_SAVE_ERROR_READONLY = (LocString) "This wardrobe staple cannot be altered\n\nMake a copy to save your changes";
    }

    public class CHANGES_NOT_SAVED_WARNING_POPUP
    {
      public static LocString HEADER = (LocString) "Discard changes to \"{OutfitName}\"?";
      public static LocString BODY = (LocString) "There are unsaved changes which will be lost if you exit now.\n\nAre you sure you want to discard your changes?";
      public static LocString BUTTON_DISCARD = (LocString) "Yes, discard changes";
      public static LocString BUTTON_RETURN = (LocString) "Cancel";
    }

    public class COPY_POPUP
    {
      public static LocString HEADER = (LocString) "RENAME COPY";
    }
  }

  public class OUTFIT_NAME
  {
    public static LocString NEW = (LocString) "Custom Outfit";
    public static LocString COPY_OF = (LocString) "Copy of {OutfitName}";
    public static LocString RESOLVE_CONFLICT = (LocString) "{OutfitName} ({ConflictNumber})";
    public static LocString ERROR_NAME_EXISTS = (LocString) "There's already an outfit named \"{OutfitName}\"";
    public static LocString MINIONS_OUTFIT = (LocString) "{MinionName}'s Current Outfit";
    public static LocString NONE = (LocString) "Default Outfit";
    public static LocString NONE_JOY_RESPONSE = (LocString) "Default Overjoyed Response";
    public static LocString NONE_ATMO_SUIT = (LocString) "Default Atmo Suit";
    public static LocString NONE_JET_SUIT = (LocString) "Default Jet Suit";
  }

  public class OUTFIT_DESCRIPTION
  {
    public static LocString CONTAINS_NON_OWNED_ITEMS = (LocString) "This outfit can only be worn once my colony has access to all of its blueprints.";
    public static LocString NO_JOY_RESPONSE_NAME = (LocString) "Default Overjoyed Response";
    public static LocString NO_JOY_RESPONSE_DESC = (LocString) "Default response to an overjoyed state.";
  }

  public class MINION_BROWSER_SCREEN
  {
    public static LocString CATEGORY_HEADER = (LocString) "DUPLICANTS";
    public static LocString BUTTON_CHANGE_OUTFIT = (LocString) "Open Wardrobe";
    public static LocString BUTTON_EDIT_OUTFIT_ITEMS = (LocString) "Restyle Outfit";
    public static LocString BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS = (LocString) "Restyle Atmo Suit";
    public static LocString BUTTON_EDIT_JET_SUIT_OUTFIT_ITEMS = (LocString) "Restyle Jet Suit";
    public static LocString BUTTON_EDIT_JOY_RESPONSE = (LocString) "Restyle Overjoyed Response";
    public static LocString OUTFIT_TYPE_CLOTHING = (LocString) "CLOTHING";
    public static LocString OUTFIT_TYPE_JOY_RESPONSE = (LocString) "OVERJOYED RESPONSE";
    public static LocString OUTFIT_TYPE_ATMOSUIT = (LocString) "ATMO SUIT";
    public static LocString OUTFIT_TYPE_JETSUIT = (LocString) "JET SUIT";
    public static LocString TOOLTIP_FROM_DLC = (LocString) "This Duplicant is part of {0} DLC";
    public static LocString TOOLTIP_CYCLE_PREVIOUS_OUTFIT_TYPE = (LocString) "Previous Restylable\n\nClick to restyle this Duplicant's other outfit types and Overjoyed response";
    public static LocString TOOLTIP_CYCLE_NEXT_OUTFIT_TYPE = (LocString) "Next Restylable\n\nClick to restyle this Duplicant's other outfit types and Overjoyed response";
  }

  public class PERMIT_RARITY
  {
    public static readonly LocString UNKNOWN = (LocString) "Unknown";
    public static readonly LocString UNIVERSAL = (LocString) "Universal";
    public static readonly LocString LOYALTY = (LocString) "<color=#FFB037>Loyalty</color>";
    public static readonly LocString COMMON = (LocString) "<color=#97B2B9>Common</color>";
    public static readonly LocString DECENT = (LocString) "<color=#81EBDE>Decent</color>";
    public static readonly LocString NIFTY = (LocString) "<color=#71E379>Nifty</color>";
    public static readonly LocString SPLENDID = (LocString) "<color=#FF6DE7>Splendid</color>";
  }

  public class OUTFITS
  {
    public class STANDARD_YELLOW
    {
      public static LocString NAME = (LocString) "Standard Yellow Uniform";
    }

    public class STANDARD_RED
    {
      public static LocString NAME = (LocString) "Standard Red Uniform";
    }

    public class STANDARD_GREEN
    {
      public static LocString NAME = (LocString) "Standard Green Uniform";
    }

    public class STANDARD_BLUE
    {
      public static LocString NAME = (LocString) "Standard Blue Uniform";
    }

    public class BASIC_BLACK
    {
      public static LocString NAME = (LocString) "Basic Black Outfit";
    }

    public class BASIC_WHITE
    {
      public static LocString NAME = (LocString) "Basic White Outfit";
    }

    public class BASIC_RED
    {
      public static LocString NAME = (LocString) "Basic Red Outfit";
    }

    public class BASIC_ORANGE
    {
      public static LocString NAME = (LocString) "Basic Orange Outfit";
    }

    public class BASIC_YELLOW
    {
      public static LocString NAME = (LocString) "Basic Yellow Outfit";
    }

    public class BASIC_GREEN
    {
      public static LocString NAME = (LocString) "Basic Green Outfit";
    }

    public class BASIC_AQUA
    {
      public static LocString NAME = (LocString) "Basic Aqua Outfit";
    }

    public class BASIC_PURPLE
    {
      public static LocString NAME = (LocString) "Basic Purple Outfit";
    }

    public class BASIC_PINK_ORCHID
    {
      public static LocString NAME = (LocString) "Basic Bubblegum Outfit";
    }

    public class BASIC_DEEPRED
    {
      public static LocString NAME = (LocString) "Team Captain Outfit";
    }

    public class BASIC_BLUE_COBALT
    {
      public static LocString NAME = (LocString) "True Blue Outfit";
    }

    public class BASIC_PINK_FLAMINGO
    {
      public static LocString NAME = (LocString) "Pep Rally Outfit";
    }

    public class BASIC_GREEN_KELLY
    {
      public static LocString NAME = (LocString) "Go Team! Outfit";
    }

    public class BASIC_GREY_CHARCOAL
    {
      public static LocString NAME = (LocString) "Underdog Outfit";
    }

    public class BASIC_LEMON
    {
      public static LocString NAME = (LocString) "Team Hype Outfit";
    }

    public class BASIC_SATSUMA
    {
      public static LocString NAME = (LocString) "Superfan Outfit";
    }

    public class JELLYPUFF_BLUEBERRY
    {
      public static LocString NAME = (LocString) "Blueberry Jelly Outfit";
    }

    public class JELLYPUFF_GRAPE
    {
      public static LocString NAME = (LocString) "Grape Jelly Outfit";
    }

    public class JELLYPUFF_LEMON
    {
      public static LocString NAME = (LocString) "Lemon Jelly Outfit";
    }

    public class JELLYPUFF_LIME
    {
      public static LocString NAME = (LocString) "Lime Jelly Outfit";
    }

    public class JELLYPUFF_SATSUMA
    {
      public static LocString NAME = (LocString) "Satsuma Jelly Outfit";
    }

    public class JELLYPUFF_STRAWBERRY
    {
      public static LocString NAME = (LocString) "Strawberry Jelly Outfit";
    }

    public class JELLYPUFF_WATERMELON
    {
      public static LocString NAME = (LocString) "Watermelon Jelly Outfit";
    }

    public class ATHLETE
    {
      public static LocString NAME = (LocString) "Racing Outfit";
    }

    public class CIRCUIT
    {
      public static LocString NAME = (LocString) "LED Party Outfit";
    }

    public class ATMOSUIT_LIMONE
    {
      public static LocString NAME = (LocString) "Citrus Atmo Outfit";
    }

    public class ATMOSUIT_SPARKLE_RED
    {
      public static LocString NAME = (LocString) "Red Glitter Atmo Outfit";
    }

    public class ATMOSUIT_SPARKLE_BLUE
    {
      public static LocString NAME = (LocString) "Blue Glitter Atmo Outfit";
    }

    public class ATMOSUIT_SPARKLE_GREEN
    {
      public static LocString NAME = (LocString) "Green Glitter Atmo Outfit";
    }

    public class ATMOSUIT_SPARKLE_LAVENDER
    {
      public static LocString NAME = (LocString) "Violet Glitter Atmo Outfit";
    }

    public class ATMOSUIT_PUFT
    {
      public static LocString NAME = (LocString) "Puft Atmo Outfit";
    }

    public class ATMOSUIT_CONFETTI
    {
      public static LocString NAME = (LocString) "Confetti Atmo Outfit";
    }

    public class ATMOSUIT_BASIC_PURPLE
    {
      public static LocString NAME = (LocString) "Eggplant Atmo Outfit";
    }

    public class ATMOSUIT_PINK_PURPLE
    {
      public static LocString NAME = (LocString) "Pink Punch Atmo Outfit";
    }

    public class ATMOSUIT_RED_GREY
    {
      public static LocString NAME = (LocString) "Blastoff Atmo Outfit";
    }

    public class CANUXTUX
    {
      public static LocString NAME = (LocString) "Canadian Tuxedo Outfit";
    }

    public class GONCHIES_STRAWBERRY
    {
      public static LocString NAME = (LocString) "Executive Undies Outfit";
    }

    public class GONCHIES_SATSUMA
    {
      public static LocString NAME = (LocString) "Underling Undies Outfit";
    }

    public class GONCHIES_LEMON
    {
      public static LocString NAME = (LocString) "Groupthink Undies Outfit";
    }

    public class GONCHIES_LIME
    {
      public static LocString NAME = (LocString) "Stakeholder Undies Outfit";
    }

    public class GONCHIES_BLUEBERRY
    {
      public static LocString NAME = (LocString) "Admin Undies Outfit";
    }

    public class GONCHIES_GRAPE
    {
      public static LocString NAME = (LocString) "Buzzword Undies Outfit";
    }

    public class GONCHIES_WATERMELON
    {
      public static LocString NAME = (LocString) "Synergy Undies Outfit";
    }

    public class NERD
    {
      public static LocString NAME = (LocString) "Research Outfit";
    }

    public class REBELGI
    {
      public static LocString NAME = (LocString) "Rebel Gi Outfit";
    }

    public class DONOR
    {
      public static LocString NAME = (LocString) "Donor Outfit";
    }

    public class MECHANIC
    {
      public static LocString NAME = (LocString) "Engineer Coveralls";
    }

    public class VELOUR_BLACK
    {
      public static LocString NAME = (LocString) "PhD Velour Outfit";
    }

    public class SLEEVELESS_BOW_BW
    {
      public static LocString NAME = (LocString) "PhD Dress Outfit";
    }

    public class VELOUR_BLUE
    {
      public static LocString NAME = (LocString) "Shortwave Velour Outfit";
    }

    public class VELOUR_PINK
    {
      public static LocString NAME = (LocString) "Gamma Velour Outfit";
    }

    public class WATER
    {
      public static LocString NAME = (LocString) "HVAC Coveralls";
    }

    public class WAISTCOAT_PINSTRIPE_SLATE
    {
      public static LocString NAME = (LocString) "Nobel Pinstripe Outfit";
    }

    public class TWEED_PINK_ORCHID
    {
      public static LocString NAME = (LocString) "Power Brunch Outfit";
    }

    public class BALLET
    {
      public static LocString NAME = (LocString) "Ballet Outfit";
    }

    public class ATMOSUIT_CANTALOUPE
    {
      public static LocString NAME = (LocString) "Rocketmelon Atmo Outfit";
    }

    public class PAJAMAS_SNOW
    {
      public static LocString NAME = (LocString) "Crystal-Iced Jammies";
    }

    public class X_SPORCHID
    {
      public static LocString NAME = (LocString) "Sporefest Outfit";
    }

    public class X1_PINCHAPEPPERNUTBELLS
    {
      public static LocString NAME = (LocString) "Pinchabell Outfit";
    }

    public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
    {
      public static LocString NAME = (LocString) "Pom Bug Outfit";
    }

    public class SNOWFLAKE_BLUE
    {
      public static LocString NAME = (LocString) "Crystal-Iced Outfit";
    }

    public class POLKADOT_TRACKSUIT
    {
      public static LocString NAME = (LocString) "Polka Dot Tracksuit";
    }

    public class SUPERSTAR
    {
      public static LocString NAME = (LocString) "Superstar Outfit";
    }

    public class ATMOSUIT_SPIFFY
    {
      public static LocString NAME = (LocString) "Spiffy Atmo Outfit";
    }

    public class ATMOSUIT_CUBIST
    {
      public static LocString NAME = (LocString) "Cubist Atmo Outfit";
    }

    public class LUCKY
    {
      public static LocString NAME = (LocString) "Lucky Jammies Outfit";
    }

    public class SWEETHEART
    {
      public static LocString NAME = (LocString) "Sweetheart Jammies Outfit";
    }

    public class GINCH_GLUON
    {
      public static LocString NAME = (LocString) "Frilly Saltrock Outfit";
    }

    public class GINCH_CORTEX
    {
      public static LocString NAME = (LocString) "Dusk Undies Outfit";
    }

    public class GINCH_FROSTY
    {
      public static LocString NAME = (LocString) "Frostbasin Undies Outfit";
    }

    public class GINCH_LOCUS
    {
      public static LocString NAME = (LocString) "Balmy Undies Outfit";
    }

    public class GINCH_GOOP
    {
      public static LocString NAME = (LocString) "Leachy Undies Outfit";
    }

    public class GINCH_BILE
    {
      public static LocString NAME = (LocString) "Yellowcake Undies Outfit";
    }

    public class GINCH_NYBBLE
    {
      public static LocString NAME = (LocString) "Atomic Undies Outfit";
    }

    public class GINCH_IRONBOW
    {
      public static LocString NAME = (LocString) "Magma Undies Outfit";
    }

    public class GINCH_PHLEGM
    {
      public static LocString NAME = (LocString) "Slate Undies Outfit";
    }

    public class GINCH_OBELUS
    {
      public static LocString NAME = (LocString) "Charcoal Undies Outfit";
    }

    public class HIVIS
    {
      public static LocString NAME = (LocString) "Hi-Vis Outfit";
    }

    public class DOWNTIME
    {
      public static LocString NAME = (LocString) "Downtime Outfit";
    }

    public class FLANNEL_RED
    {
      public static LocString NAME = (LocString) "Classic Flannel Outfit";
    }

    public class FLANNEL_ORANGE
    {
      public static LocString NAME = (LocString) "Cadmium Flannel Outfit";
    }

    public class FLANNEL_YELLOW
    {
      public static LocString NAME = (LocString) "Flax Flannel Outfit";
    }

    public class FLANNEL_GREEN
    {
      public static LocString NAME = (LocString) "Swampy Flannel Outfit";
    }

    public class FLANNEL_BLUE_MIDDLE
    {
      public static LocString NAME = (LocString) "Scrub Flannel Outfit";
    }

    public class FLANNEL_PURPLE
    {
      public static LocString NAME = (LocString) "Fusion Flannel Outfit";
    }

    public class FLANNEL_PINK_ORCHID
    {
      public static LocString NAME = (LocString) "Flare Flannel Outfit";
    }

    public class FLANNEL_WHITE
    {
      public static LocString NAME = (LocString) "White Flannel Outfit";
    }

    public class FLANNEL_BLACK
    {
      public static LocString NAME = (LocString) "Monochrome Flannel Outfit";
    }
  }

  public class ROLES_SCREEN
  {
    public static LocString MANAGEMENT_BUTTON = (LocString) "JOBS";
    public static LocString ROLE_PROGRESS = (LocString) "<b>Job Experience: {0}/{1}</b>\nDuplicants can become eligible for specialized jobs by maxing their current job experience";
    public static LocString NO_JOB_STATION_WARNING = (LocString) $"Build a {UI.PRE_KEYWORD}Printing Pod{UI.PST_KEYWORD} to unlock this menu\n\nThe {UI.PRE_KEYWORD}Printing Pod{UI.PST_KEYWORD} can be found in the {UI.FormatAsBuildMenuTab("Base Tab", Action.Plan1)} of the Build Menu";
    public static LocString AUTO_PRIORITIZE = (LocString) "Auto-Prioritize:";
    public static LocString AUTO_PRIORITIZE_ENABLED = (LocString) "Duplicant priorities are automatically reconfigured when they are assigned a new job";
    public static LocString AUTO_PRIORITIZE_DISABLED = (LocString) "Duplicant priorities can only be changed manually";
    public static LocString EXPECTATION_ALERT_EXPECTATION = (LocString) "Current Morale: {0}\nJob Morale Needs: {1}";
    public static LocString EXPECTATION_ALERT_JOB = (LocString) "Current Morale: {0}\n{2} Minimum Morale: {1}";
    public static LocString EXPECTATION_ALERT_TARGET_JOB = (LocString) "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";
    public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = (LocString) "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";
    public static LocString EXPECTATION_ALERT_DESC_JOB = (LocString) "This Duplicant's Morale is too low to handle the assigned job, which will cause them Stress over time.";
    public static LocString EXPECTATION_ALERT_DESC_TARGET_JOB = (LocString) "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";
    public static LocString HIGHEST_EXPECTATIONS_TIER = (LocString) "<b>Highest Expectations</b>";
    public static LocString ADDED_EXPECTATIONS_AMOUNT = (LocString) " (+{0} Expectation)";

    public class WIDGET
    {
      public static LocString NUMBER_OF_MASTERS_TOOLTIP = (LocString) "<b>Duplicants who have mastered this job:</b>{0}";
      public static LocString NO_MASTERS_TOOLTIP = (LocString) "<b>No Duplicants have mastered this job</b>";
    }

    public class TIER_NAMES
    {
      public static LocString ZERO = (LocString) "Tier 0";
      public static LocString ONE = (LocString) "Tier 1";
      public static LocString TWO = (LocString) "Tier 2";
      public static LocString THREE = (LocString) "Tier 3";
      public static LocString FOUR = (LocString) "Tier 4";
      public static LocString FIVE = (LocString) "Tier 5";
      public static LocString SIX = (LocString) "Tier 6";
      public static LocString SEVEN = (LocString) "Tier 7";
      public static LocString EIGHT = (LocString) "Tier 8";
      public static LocString NINE = (LocString) "Tier 9";
    }

    public class SLOTS
    {
      public static LocString UNASSIGNED = (LocString) "Vacant Position";
      public static LocString UNASSIGNED_TOOLTIP = (LocString) (UI.CLICK(UI.ClickType.Click) + " to assign a Duplicant to this job opening");
      public static LocString NOSLOTS = (LocString) "No slots available";
      public static LocString NO_ELIGIBLE_DUPLICANTS = (LocString) "No Duplicants meet the requirements for this job";
      public static LocString ASSIGNMENT_PENDING = (LocString) "(Pending)";
      public static LocString PICK_JOB = (LocString) "No Job";
      public static LocString PICK_DUPLICANT = (LocString) "None";
    }

    public class DROPDOWN
    {
      public static LocString NAME_AND_ROLE = (LocString) "{0} <color=#F44A47FF>({1})</color>";
      public static LocString ALREADY_ROLE = (LocString) "(Currently {0})";
    }

    public class SIDEBAR
    {
      public static LocString ASSIGNED_DUPLICANTS = (LocString) "Assigned Duplicants";
      public static LocString UNASSIGNED_DUPLICANTS = (LocString) "Unassigned Duplicants";
      public static LocString UNASSIGN = (LocString) "Unassign job";
    }

    public class PRIORITY
    {
      public static LocString TITLE = (LocString) "Job Priorities";
      public static LocString DESCRIPTION = (LocString) "{0}s prioritize these work errands: ";
      public static LocString NO_EFFECT = (LocString) "This job does not affect errand prioritization";
    }

    public class RESUME
    {
      public static LocString TITLE = (LocString) "Qualifications";
      public static LocString PREVIOUS_ROLES = (LocString) "PREVIOUS DUTIES";
      public static LocString UNASSIGNED = (LocString) "Unassigned";
      public static LocString NO_SELECTION = (LocString) "No Duplicant selected";
    }

    public class PERKS
    {
      public static LocString TITLE_BASICTRAINING = (LocString) "Basic Job Training";
      public static LocString TITLE_MORETRAINING = (LocString) "Additional Job Training";
      public static LocString NO_PERKS = (LocString) "This job comes with no training";
      public static LocString ATTRIBUTE_EFFECT_FMT = (LocString) $"<b>{{0}}</b> {UI.PRE_KEYWORD}{{1}}{UI.PST_KEYWORD}";
      public static LocString IMMUNITY = (LocString) "Immunity to <b>{0}</b>";

      public class CAN_DIG_VERY_FIRM
      {
        public static LocString DESCRIPTION = (LocString) (UI.FormatAsLink((string) ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + " Material", "HARDNESS") + " Mining");
      }

      public class CAN_DIG_NEARLY_IMPENETRABLE
      {
        public static LocString DESCRIPTION = (LocString) (UI.FormatAsLink("Abyssalite", "KATAIRITE") + " Mining");
      }

      public class CAN_DIG_SUPER_SUPER_HARD
      {
        public static LocString DESCRIPTION = (LocString) $"{UI.FormatAsLink("Diamond", "DIAMOND")} and {UI.FormatAsLink("Obsidian", "OBSIDIAN")} Mining";
      }

      public class CAN_DIG_RADIOACTIVE_MATERIALS
      {
        public static LocString DESCRIPTION = (LocString) (UI.FormatAsLink("Corium", "CORIUM") + " Mining");
      }

      public class CAN_DIG_UNOBTANIUM
      {
        public static LocString DESCRIPTION = (LocString) (UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " Mining");
      }

      public class CAN_SWIM
      {
        public static LocString DESCRIPTION = (LocString) $"{UI.PRE_KEYWORD}Novice{UI.PST_KEYWORD} swimming ability";
      }

      public class INCREASE_SWIMMER_STAMINA_IN_LIQUID
      {
        public static LocString DESCRIPTION = (LocString) $"Reduced {UI.PRE_KEYWORD}Stamina{UI.PST_KEYWORD} consumption when swimming";
      }

      public class INCREASE_SWIMMER_ATHLETICS_IN_LIQUID
      {
        public static LocString DESCRIPTION = (LocString) "Increased swimming speed";
      }

      public class REDUCE_SALTWATER_SWIMMER_EYE_IRRITATION
      {
        public static LocString DESCRIPTION = (LocString) $"Increased resistance to {UI.PRE_KEYWORD}Eye Irritation{UI.PST_KEYWORD} caused by {UI.PRE_KEYWORD}Salt Water{UI.PST_KEYWORD}";
      }

      public class IMPROVED_LIQUID_TEMPERATURE_TOLERANCE
      {
        public static LocString DESCRIPTION = (LocString) $"Increased resistance to {UI.PRE_KEYWORD}Chilly Surroundings{UI.PST_KEYWORD} when in liquid";
      }

      public class CAN_ART
      {
        public static LocString DESCRIPTION = (LocString) $"Can produce artwork using:\n<indent=30px>• {(string) BUILDINGS.PREFABS.CANVAS.NAME}\n• {(string) BUILDINGS.PREFABS.SMALLSCULPTURE.NAME}\n• {(string) BUILDINGS.PREFABS.SCULPTURE.NAME}\n• {(string) BUILDINGS.PREFABS.ICESCULPTURE.NAME}\n• {(string) BUILDINGS.PREFABS.METALSCULPTURE.NAME}\n• {(string) BUILDINGS.PREFABS.WOODSCULPTURE.NAME}</indent>";
      }

      public class CAN_ART_UGLY
      {
        public static LocString DESCRIPTION = (LocString) $"{UI.PRE_KEYWORD}Crude{UI.PST_KEYWORD} artwork quality";
      }

      public class CAN_ART_OKAY
      {
        public static LocString DESCRIPTION = (LocString) $"{UI.PRE_KEYWORD}Mediocre{UI.PST_KEYWORD} artwork quality";
      }

      public class CAN_ART_GREAT
      {
        public static LocString DESCRIPTION = (LocString) $"{UI.PRE_KEYWORD}Master{UI.PST_KEYWORD} artwork quality";
      }

      public class CAN_FARM_TINKER
      {
        public static LocString DESCRIPTION = (LocString) UI.FormatAsLink("Crop Tending", "PLANTS");
      }

      public class CAN_IDENTIFY_MUTANT_SEEDS
      {
        public static LocString DESCRIPTION = (LocString) $"Can identify {UI.PRE_KEYWORD}Mutant Seeds{UI.PST_KEYWORD} at the {(string) BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME}";
      }

      public class CAN_FARM_STATION
      {
        public static LocString DESCRIPTION = (LocString) $"Can craft {UI.PRE_KEYWORD}Micronutrient Fertilizer{UI.PST_KEYWORD} at the {(string) BUILDINGS.PREFABS.FARMSTATION.NAME}";
      }

      public class CAN_SALVAGE_PLANT_FIBER
      {
        public static LocString DESCRIPTION = (LocString) ("Can salvage " + (string) ITEMS.INDUSTRIAL_PRODUCTS.PLANT_FIBER.NAME);
      }

      public class CAN_WRANGLE_CREATURES
      {
        public static LocString DESCRIPTION = (LocString) "Critter Wrangling";
      }

      public class CAN_USE_BUILDING
      {
        public static LocString DESCRIPTION = (LocString) "{0} Usage";
      }

      public class CAN_USE_RANCH_STATION
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.RANCHSTATION.NAME + " Usage");
      }

      public class CAN_USE_MILKING_STATION
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.MILKINGSTATION.NAME + " Usage");
      }

      public class CAN_POWER_TINKER
      {
        public static LocString DESCRIPTION = (LocString) $"{UI.FormatAsLink("Generator", "POWER")} Tuning and {UI.FormatAsLink("Microchip", "POWER_STATION_TOOLS")} Crafting";
      }

      public class CAN_ELECTRIC_GRILL
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.COOKINGSTATION.NAME + " Usage");
      }

      public class CAN_GAS_RANGE
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.GOURMETCOOKINGSTATION.NAME + " Usage");
      }

      public class CAN_DEEP_FRYER
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.DEEPFRYER.NAME + " Usage");
      }

      public class CAN_SUSHI_BAR
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.SUSHIBAR.NAME + " Usage");
      }

      public class CAN_SPICE_GRINDER
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.SPICEGRINDER.NAME + " Usage");
      }

      public class CAN_MAKE_MISSILES
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.MISSILEFABRICATOR.NAME + " Usage");
      }

      public class CAN_CRAFT_ELECTRONICS
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME + " Usage");
      }

      public class ADVANCED_RESEARCH
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME + " Usage");
      }

      public class INTERSTELLAR_RESEARCH
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME + " Usage");
      }

      public class NUCLEAR_RESEARCH
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME + " Usage");
      }

      public class ORBITAL_RESEARCH
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME + " Usage");
      }

      public class GEYSER_TUNING
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.GEOTUNER.NAME + " Usage");
      }

      public class CHEMISTRY
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.CHEMICALREFINERY.NAME + " Usage");
      }

      public class CAN_CLOTHING_ALTERATION
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.CLOTHINGALTERATIONSTATION.NAME + " Usage");
      }

      public class CAN_STUDY_WORLD_OBJECTS
      {
        public static LocString DESCRIPTION = (LocString) "Geographical Analysis";
      }

      public class CAN_STUDY_ARTIFACTS
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME + " Usage");
      }

      public class CAN_USE_CLUSTER_TELESCOPE
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " Usage");
      }

      public class CAN_CLUSTERTELESCOPEENCLOSED
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME + " Usage");
      }

      public class EXOSUIT_EXPERTISE
      {
        public static LocString DESCRIPTION = (LocString) (UI.FormatAsLink("Exosuit", "EQUIPMENT") + " Penalty Reduction");
      }

      public class EXOSUIT_DURABILITY
      {
        public static LocString DESCRIPTION = (LocString) $"Slows {UI.FormatAsLink("Exosuit", "EQUIPMENT")} Durability Damage";
      }

      public class CONVEYOR_BUILD
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " Construction");
      }

      public class CAN_DO_PLUMBING
      {
        public static LocString DESCRIPTION = (LocString) "Pipe Emptying";
      }

      public class CAN_USE_ROCKETS
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.COMMANDMODULE.NAME + " Usage");
      }

      public class CAN_DO_ASTRONAUT_TRAINING
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.ASTRONAUTTRAININGCENTER.NAME + " Usage");
      }

      public class CAN_MISSION_CONTROL
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.MISSIONCONTROL.NAME + " Usage");
      }

      public class CAN_PILOT_ROCKET
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " Usage");
      }

      public class CAN_COMPOUND
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.APOTHECARY.NAME + " Usage");
      }

      public class CAN_DOCTOR
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.DOCTORSTATION.NAME + " Usage");
      }

      public class CAN_ADVANCED_MEDICINE
      {
        public static LocString DESCRIPTION = (LocString) ((string) BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME + " Usage");
      }

      public class CAN_DEMOLISH
      {
        public static LocString DESCRIPTION = (LocString) "Demolish Gravitas Buildings";
      }

      public class EXTRA_BIONIC_BATTERIES
      {
        public static LocString DESCRIPTION = (LocString) ("Extra " + UI.FormatAsLink("Power Banks", "ELECTROBANK"));
      }

      public class REDUCED_GUNK_PRODUCTION
      {
        public static LocString DESCRIPTION = (LocString) $"10% Slower {UI.FormatAsLink("Gunk", "LIQUIDGUNK")} Buildup";
      }

      public class EFFICIENT_BIONIC_GEARS
      {
        public static LocString DESCRIPTION = (LocString) "50% Grinding Gears penalty reduction";
      }

      public class CAN_FARM_CLAMS
      {
        public static LocString DESCRIPTION = (LocString) (UI.FormatAsLink("Clampum", "CLAM") + " farming");
      }
    }

    public class ASSIGNMENT_REQUIREMENTS
    {
      public static LocString TITLE = (LocString) "Qualifications";
      public static LocString NONE = (LocString) "This position has no qualification requirements";
      public static LocString ALREADY_IS_ROLE = (LocString) "{0} <b>is already</b> assigned to the {1} position";
      public static LocString ALREADY_IS_JOBLESS = (LocString) "{0} <b>is already</b> unemployed";
      public static LocString MASTERED = (LocString) "{0} has mastered the {1} position";
      public static LocString WILL_BE_UNASSIGNED = (LocString) "Note: Assigning {0} to {1} will <color=#F44A47FF>unassign</color> them from {2}";
      public static LocString RELEVANT_ATTRIBUTES = (LocString) "Relevant skills:";
      public static LocString APTITUDES = (LocString) "Interests";
      public static LocString RELEVANT_APTITUDES = (LocString) "Relevant Interests:";
      public static LocString NO_APTITUDE = (LocString) "None";

      public class ELIGIBILITY
      {
        public static LocString ELIGIBLE = (LocString) "{0} is qualified for the {1} position";
        public static LocString INELIGIBLE = (LocString) "{0} is <color=#F44A47FF>not qualified</color> for the {1} position";
      }

      public class UNEMPLOYED
      {
        public static LocString NAME = (LocString) "Unassigned";
        public static LocString DESCRIPTION = (LocString) "Duplicant must not already have a job assignment";
      }

      public class HAS_COLONY_LEADER
      {
        public static LocString NAME = (LocString) "Has colony leader";
        public static LocString DESCRIPTION = (LocString) "A colony leader must be assigned";
      }

      public class HAS_ATTRIBUTE_DIGGING_BASIC
      {
        public static LocString NAME = (LocString) "Basic Digging";
        public static LocString DESCRIPTION = (LocString) "Must have at least {0} digging skill";
      }

      public class HAS_ATTRIBUTE_COOKING_BASIC
      {
        public static LocString NAME = (LocString) "Basic Cooking";
        public static LocString DESCRIPTION = (LocString) "Must have at least {0} cooking skill";
      }

      public class HAS_ATTRIBUTE_LEARNING_BASIC
      {
        public static LocString NAME = (LocString) "Basic Learning";
        public static LocString DESCRIPTION = (LocString) "Must have at least {0} learning skill";
      }

      public class HAS_ATTRIBUTE_LEARNING_MEDIUM
      {
        public static LocString NAME = (LocString) "Medium Learning";
        public static LocString DESCRIPTION = (LocString) "Must have at least {0} learning skill";
      }

      public class HAS_EXPERIENCE
      {
        public static LocString NAME = (LocString) "{0} Experience";
        public static LocString DESCRIPTION = (LocString) "Mastery of the <b>{0}</b> job";
      }

      public class HAS_COMPLETED_ANY_OTHER_ROLE
      {
        public static LocString NAME = (LocString) "General Experience";
        public static LocString DESCRIPTION = (LocString) "Mastery of <b>at least one</b> job";
      }

      public class CHOREGROUP_ENABLED
      {
        public static LocString NAME = (LocString) "Can perform {0}";
        public static LocString DESCRIPTION = (LocString) "Capable of performing <b>{0}</b> jobs";
      }
    }

    public class EXPECTATIONS
    {
      public static LocString TITLE = (LocString) "Special Provisions Request";
      public static LocString NO_EXPECTATIONS = (LocString) "No additional provisions are required to perform this job";

      public class PRIVATE_ROOM
      {
        public static LocString NAME = (LocString) "Private Bedroom";
        public static LocString DESCRIPTION = (LocString) "Duplicants in this job would appreciate their own place to unwind";
      }

      public class FOOD_QUALITY
      {
        public class MINOR
        {
          public static LocString NAME = (LocString) "Standard Food";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier desire food that meets basic living standards";
        }

        public class MEDIUM
        {
          public static LocString NAME = (LocString) "Good Food";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier desire decent food for their efforts";
        }

        public class HIGH
        {
          public static LocString NAME = (LocString) "Great Food";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier desire better than average food";
        }

        public class VERY_HIGH
        {
          public static LocString NAME = (LocString) "Superb Food";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier have a refined taste for food";
        }

        public class EXCEPTIONAL
        {
          public static LocString NAME = (LocString) "Ambrosial Food";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier expect only the best cuisine";
        }
      }

      public class DECOR
      {
        public class MINOR
        {
          public static LocString NAME = (LocString) "Minor Decor";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier desire slightly improved colony decor";
        }

        public class MEDIUM
        {
          public static LocString NAME = (LocString) "Medium Decor";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier desire reasonably improved colony decor";
        }

        public class HIGH
        {
          public static LocString NAME = (LocString) "High Decor";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier desire a decent increase in colony decor";
        }

        public class VERY_HIGH
        {
          public static LocString NAME = (LocString) "Superb Decor";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier desire majorly improved colony decor";
        }

        public class UNREASONABLE
        {
          public static LocString NAME = (LocString) "Decadent Decor";
          public static LocString DESCRIPTION = (LocString) "Duplicants employed in this Tier desire unrealistically luxurious improvements to decor";
        }
      }

      public class QUALITYOFLIFE
      {
        public class TIER0
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 0";
        }

        public class TIER1
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 1";
        }

        public class TIER2
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 2";
        }

        public class TIER3
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 3";
        }

        public class TIER4
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 4";
        }

        public class TIER5
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 5";
        }

        public class TIER6
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 6";
        }

        public class TIER7
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 7";
        }

        public class TIER8
        {
          public static LocString NAME = (LocString) "Morale Requirements";
          public static LocString DESCRIPTION = (LocString) "Tier 8";
        }
      }
    }
  }

  public class GAMEPLAY_EVENT_INFO_SCREEN
  {
    public static LocString WHERE = (LocString) "WHERE: {0}";
    public static LocString WHEN = (LocString) "WHEN: {0}";
  }

  public class DEBUG_TOOLS
  {
    public static LocString ENTER_TEXT = (LocString) "";
    public static LocString DEBUG_ACTIVE = (LocString) "Debug tools active";
    public static LocString INVALID_LOCATION = (LocString) "Invalid Location";

    public class PAINT_ELEMENTS_SCREEN
    {
      public static LocString TITLE = (LocString) "CELL PAINTER";
      public static LocString ELEMENT = (LocString) "Element";
      public static LocString MASS_KG = (LocString) "Mass (kg)";
      public static LocString TEMPERATURE_KELVIN = (LocString) "Temperature (K)";
      public static LocString DISEASE = (LocString) "Disease";
      public static LocString DISEASE_COUNT = (LocString) "Disease Count";
      public static LocString BUILDINGS = (LocString) "Buildings:";
      public static LocString CELLS = (LocString) "Cells:";
      public static LocString ADD_FOW_MASK = (LocString) "Prevent FoW Reveal";
      public static LocString REMOVE_FOW_MASK = (LocString) "Allow FoW Reveal";
      public static LocString PAINT = (LocString) "Paint";
      public static LocString SAMPLE = (LocString) "Sample";
      public static LocString STORE = (LocString) "Store";
      public static LocString FILL = (LocString) "Fill";
      public static LocString SPAWN_ALL = (LocString) "Spawn All (Slow)";
    }

    public class SAVE_BASE_TEMPLATE
    {
      public static LocString TITLE = (LocString) "Base and World Tools";
      public static LocString SAVE_TITLE = (LocString) "Save Selection";
      public static LocString CLEAR_BUTTON = (LocString) "Clear Floor";
      public static LocString DESTROY_BUTTON = (LocString) "Destroy";
      public static LocString DECONSTRUCT_BUTTON = (LocString) "Deconstruct";
      public static LocString CLEAR_SELECTION_BUTTON = (LocString) "Clear Selection";
      public static LocString DEFAULT_SAVE_NAME = (LocString) "TemplateSaveName";
      public static LocString MORE = (LocString) "More";
      public static LocString BASE_GAME_FOLDER_NAME = (LocString) "Base Game";
      public static LocString LORE_OVERRIDE_WARNING = (LocString) "The new template has fewer lore overrides ({newCount}) than the existing one ({oldCount}). Save anyway?";

      public class SELECTION_INFO_PANEL
      {
        public static LocString TOTAL_MASS = (LocString) "Total mass: {0}";
        public static LocString AVERAGE_MASS = (LocString) "Average cell mass: {0}";
        public static LocString AVERAGE_TEMPERATURE = (LocString) "Average temperature: {0}";
        public static LocString TOTAL_JOULES = (LocString) "Total joules: {0}";
        public static LocString JOULES_PER_KILOGRAM = (LocString) "Joules per kilogram: {0}";
        public static LocString TOTAL_RADS = (LocString) "Total rads: {0}";
        public static LocString AVERAGE_RADS = (LocString) "Average rads: {0}";
        public static LocString AVERAGE_GERMS = (LocString) "Average germs: {0}";
        public static LocString TOTAL_GERMS = (LocString) "Total germs: {0}";
      }
    }
  }

  public class WORLDGEN
  {
    public static LocString NOHEADERS = (LocString) "";
    public static LocString COMPLETE = (LocString) "Success! Space adventure awaits.";
    public static LocString FAILED = (LocString) "Goodness, has this ever gone terribly wrong!";
    public static LocString RESTARTING = (LocString) "Rebooting...";
    public static LocString LOADING = (LocString) "Loading world...";
    public static LocString GENERATINGWORLD = (LocString) "The Galaxy Synthesizer";
    public static LocString CHOOSEWORLDSIZE = (LocString) "Select the magnitude of your new galaxy.";
    public static LocString USING_PLAYER_SEED = (LocString) "Using selected worldgen seed: {0}";
    public static LocString CLEARINGLEVEL = (LocString) "Staring into the void...";
    public static LocString GENERATESOLARSYSTEM = (LocString) "Catalyzing Big Bang...";
    public static LocString GENERATESOLARSYSTEM1 = (LocString) "Catalyzing Big Bang...";
    public static LocString GENERATESOLARSYSTEM2 = (LocString) "Catalyzing Big Bang...";
    public static LocString GENERATESOLARSYSTEM3 = (LocString) "Catalyzing Big Bang...";
    public static LocString GENERATESOLARSYSTEM4 = (LocString) "Catalyzing Big Bang...";
    public static LocString GENERATESOLARSYSTEM5 = (LocString) "Catalyzing Big Bang...";
    public static LocString GENERATESOLARSYSTEM6 = (LocString) "Approaching event horizon...";
    public static LocString GENERATESOLARSYSTEM7 = (LocString) "Approaching event horizon...";
    public static LocString GENERATESOLARSYSTEM8 = (LocString) "Approaching event horizon...";
    public static LocString GENERATESOLARSYSTEM9 = (LocString) "Approaching event horizon...";
    public static LocString SETUPNOISE = (LocString) "BANG!";
    public static LocString BUILDNOISESOURCE = (LocString) "Sorting quadrillions of atoms...";
    public static LocString BUILDNOISESOURCE1 = (LocString) "Sorting quadrillions of atoms...";
    public static LocString BUILDNOISESOURCE2 = (LocString) "Sorting quadrillions of atoms...";
    public static LocString BUILDNOISESOURCE3 = (LocString) "Ironing the fabric of creation...";
    public static LocString BUILDNOISESOURCE4 = (LocString) "Ironing the fabric of creation...";
    public static LocString BUILDNOISESOURCE5 = (LocString) "Ironing the fabric of creation...";
    public static LocString BUILDNOISESOURCE6 = (LocString) "Taking hot meteor shower...";
    public static LocString BUILDNOISESOURCE7 = (LocString) "Tightening asteroid belts...";
    public static LocString BUILDNOISESOURCE8 = (LocString) "Tightening asteroid belts...";
    public static LocString BUILDNOISESOURCE9 = (LocString) "Tightening asteroid belts...";
    public static LocString GENERATENOISE = (LocString) "Baking igneous rock...";
    public static LocString GENERATENOISE1 = (LocString) "Multilayering sediment...";
    public static LocString GENERATENOISE2 = (LocString) "Multilayering sediment...";
    public static LocString GENERATENOISE3 = (LocString) "Multilayering sediment...";
    public static LocString GENERATENOISE4 = (LocString) "Superheating gases...";
    public static LocString GENERATENOISE5 = (LocString) "Superheating gases...";
    public static LocString GENERATENOISE6 = (LocString) "Superheating gases...";
    public static LocString GENERATENOISE7 = (LocString) "Vacuuming out vacuums...";
    public static LocString GENERATENOISE8 = (LocString) "Vacuuming out vacuums...";
    public static LocString GENERATENOISE9 = (LocString) "Vacuuming out vacuums...";
    public static LocString NORMALISENOISE = (LocString) "Interpolating suffocating gas...";
    public static LocString WORLDLAYOUT = (LocString) "Freezing ice formations...";
    public static LocString WORLDLAYOUT1 = (LocString) "Freezing ice formations...";
    public static LocString WORLDLAYOUT2 = (LocString) "Freezing ice formations...";
    public static LocString WORLDLAYOUT3 = (LocString) "Freezing ice formations...";
    public static LocString WORLDLAYOUT4 = (LocString) "Melting magma...";
    public static LocString WORLDLAYOUT5 = (LocString) "Melting magma...";
    public static LocString WORLDLAYOUT6 = (LocString) "Melting magma...";
    public static LocString WORLDLAYOUT7 = (LocString) "Sprinkling sand...";
    public static LocString WORLDLAYOUT8 = (LocString) "Sprinkling sand...";
    public static LocString WORLDLAYOUT9 = (LocString) "Sprinkling sand...";
    public static LocString WORLDLAYOUT10 = (LocString) "Sprinkling sand...";
    public static LocString COMPLETELAYOUT = (LocString) "Cooling glass...";
    public static LocString COMPLETELAYOUT1 = (LocString) "Cooling glass...";
    public static LocString COMPLETELAYOUT2 = (LocString) "Cooling glass...";
    public static LocString COMPLETELAYOUT3 = (LocString) "Cooling glass...";
    public static LocString COMPLETELAYOUT4 = (LocString) "Digging holes...";
    public static LocString COMPLETELAYOUT5 = (LocString) "Digging holes...";
    public static LocString COMPLETELAYOUT6 = (LocString) "Digging holes...";
    public static LocString COMPLETELAYOUT7 = (LocString) "Adding buckets of dirt...";
    public static LocString COMPLETELAYOUT8 = (LocString) "Adding buckets of dirt...";
    public static LocString COMPLETELAYOUT9 = (LocString) "Adding buckets of dirt...";
    public static LocString COMPLETELAYOUT10 = (LocString) "Adding buckets of dirt...";
    public static LocString PROCESSRIVERS = (LocString) "Pouring rivers...";
    public static LocString CONVERTTERRAINCELLSTOEDGES = (LocString) "Hardening diamonds...";
    public static LocString PROCESSING = (LocString) "Embedding metals...";
    public static LocString PROCESSING1 = (LocString) "Embedding metals...";
    public static LocString PROCESSING2 = (LocString) "Embedding metals...";
    public static LocString PROCESSING3 = (LocString) "Burying precious ore...";
    public static LocString PROCESSING4 = (LocString) "Burying precious ore...";
    public static LocString PROCESSING5 = (LocString) "Burying precious ore...";
    public static LocString PROCESSING6 = (LocString) "Burying precious ore...";
    public static LocString PROCESSING7 = (LocString) "Excavating tunnels...";
    public static LocString PROCESSING8 = (LocString) "Excavating tunnels...";
    public static LocString PROCESSING9 = (LocString) "Excavating tunnels...";
    public static LocString BORDERS = (LocString) "Just adding water...";
    public static LocString BORDERS1 = (LocString) "Just adding water...";
    public static LocString BORDERS2 = (LocString) "Staring at the void...";
    public static LocString BORDERS3 = (LocString) "Staring at the void...";
    public static LocString BORDERS4 = (LocString) "Staring at the void...";
    public static LocString BORDERS5 = (LocString) "Avoiding awkward eye contact with the void...";
    public static LocString BORDERS6 = (LocString) "Avoiding awkward eye contact with the void...";
    public static LocString BORDERS7 = (LocString) "Avoiding awkward eye contact with the void...";
    public static LocString BORDERS8 = (LocString) "Avoiding awkward eye contact with the void...";
    public static LocString BORDERS9 = (LocString) "Avoiding awkward eye contact with the void...";
    public static LocString DRAWWORLDBORDER = (LocString) "Establishing personal boundaries...";
    public static LocString PLACINGTEMPLATES = (LocString) "Generating interest...";
    public static LocString SETTLESIM = (LocString) "Infusing oxygen...";
    public static LocString SETTLESIM1 = (LocString) "Infusing oxygen...";
    public static LocString SETTLESIM2 = (LocString) "Too much oxygen. Removing...";
    public static LocString SETTLESIM3 = (LocString) "Too much oxygen. Removing...";
    public static LocString SETTLESIM4 = (LocString) "Ideal oxygen levels achieved...";
    public static LocString SETTLESIM5 = (LocString) "Ideal oxygen levels achieved...";
    public static LocString SETTLESIM6 = (LocString) "Planting space flora...";
    public static LocString SETTLESIM7 = (LocString) "Planting space flora...";
    public static LocString SETTLESIM8 = (LocString) "Releasing wildlife...";
    public static LocString SETTLESIM9 = (LocString) "Releasing wildlife...";
    public static LocString ANALYZINGWORLD = (LocString) "Shuffling DNA Blueprints...";
    public static LocString ANALYZINGWORLDCOMPLETE = (LocString) "Tidying up for the Duplicants...";
    public static LocString PLACINGCREATURES = (LocString) "Building the suspense...";
  }

  public class TOOLTIPS
  {
    public static LocString MANAGEMENTMENU_JOBS = (LocString) $"Manage my Duplicant Priorities {{Hotkey}}\n\n{UI.PRE_KEYWORD}Duplicant Priorities{UI.PST_KEYWORD} are calculated <i>before</i> the {UI.PRE_KEYWORD}Building Priorities{UI.PST_KEYWORD} set by the {UI.FormatAsTool("Priority Tool", Action.Prioritize)}";
    public static LocString MANAGEMENTMENU_CONSUMABLES = (LocString) "Manage my Duplicants' diets and medications {Hotkey}";
    public static LocString MANAGEMENTMENU_VITALS = (LocString) "View my Duplicants' vitals {Hotkey}";
    public static LocString MANAGEMENTMENU_RESEARCH = (LocString) "View the Research Tree {Hotkey}";
    public static LocString MANAGEMENTMENU_RESEARCH_NO_RESEARCH = (LocString) "No active research projects";
    public static LocString MANAGEMENTMENU_RESEARCH_CARD_NAME = (LocString) "Currently researching: {0}";
    public static LocString MANAGEMENTMENU_RESEARCH_ITEM_LINE = (LocString) "• {0}";
    public static LocString MANAGEMENTMENU_REQUIRES_RESEARCH = (LocString) $"Build a Research Station to unlock this menu\n\nThe {(string) BUILDINGS.PREFABS.RESEARCHCENTER.NAME} can be found in the {UI.FormatAsBuildMenuTab("Stations Tab", Action.Plan10)} of the Build Menu";
    public static LocString MANAGEMENTMENU_DAILYREPORT = (LocString) "View each cycle's Colony Report {Hotkey}";
    public static LocString MANAGEMENTMENU_CODEX = (LocString) "Browse entries in my Database {Hotkey}";
    public static LocString MANAGEMENTMENU_SCHEDULE = (LocString) "Adjust the colony's time usage {Hotkey}";
    public static LocString MANAGEMENTMENU_STARMAP = (LocString) "Manage astronaut rocket missions {Hotkey}";
    public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE = (LocString) $"Build a Telescope to unlock this menu\n\nThe {(string) BUILDINGS.PREFABS.TELESCOPE.NAME} can be found in the {UI.FormatAsBuildMenuTab("Stations Tab", Action.Plan10)} of the Build Menu";
    public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE_CLUSTER = (LocString) $"Build a Telescope to unlock this menu\n\nThe {(string) BUILDINGS.PREFABS.TELESCOPE.NAME} can be found in the {UI.FormatAsBuildMenuTab("Rocketry Tab", Action.Plan14)} of the Build Menu";
    public static LocString MANAGEMENTMENU_SKILLS = (LocString) "Manage Duplicants' Skill assignments {Hotkey}";
    public static LocString MANAGEMENTMENU_REQUIRES_SKILL_STATION = (LocString) $"Build a Printing Pod to unlock this menu\n\nThe {(string) BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME} can be found in the {UI.FormatAsBuildMenuTab("Base Tab", Action.Plan1)} of the Build Menu";
    public static LocString MANAGEMENTMENU_PAUSEMENU = (LocString) "Open the game menu {Hotkey}";
    public static LocString MANAGEMENTMENU_RESOURCES = (LocString) "Open the resource management screen {Hotkey}";
    public static LocString OPEN_CODEX_ENTRY = (LocString) "View full entry in database";
    public static LocString NO_CODEX_ENTRY = (LocString) "No database entry available";
    public static LocString OPEN_RESOURCE_INFO = (LocString) "{0} of {1} available for the Duplicants on this asteroid to use\n\nClick to open Resources menu";
    public static LocString CHANGE_OUTFIT = (LocString) "Change this Duplicant's outfit";
    public static LocString CHANGE_MATERIAL = (LocString) "Change this building's construction material";
    public static LocString METERSCREEN_AVGSTRESS = (LocString) "Highest Stress: {0}";
    public static LocString METERSCREEN_MEALHISTORY = (LocString) "Calories Available: {0}\n\nDuplicants consume a minimum of {1} calories each per cycle";
    public static LocString METERSCREEN_ELECTROBANK_JOULES = (LocString) "Joules Available: {0}\n\nBionic Duplicants use a minimum of {1} each per cycle\n\nPower Banks Available: {2}\n";
    public static LocString METERSCREEN_POPULATION = (LocString) "Population: {0}";
    public static LocString METERSCREEN_POPULATION_CLUSTER = (LocString) $"{UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} Population: {{1}}\nTotal Population: {{2}}";
    public static LocString METERSCREEN_SICK_DUPES = (LocString) "Sick Duplicants: {0}";
    public static LocString METERSCREEN_INVALID_FOOD_TYPE = (LocString) "Invalid Food Type: {0}";
    public static LocString METERSCREEN_INVALID_ELECTROBANK_TYPE = (LocString) "Invalid Power Bank Type: {0}";
    public static LocString PLAYBUTTON = (LocString) "Start";
    public static LocString PAUSEBUTTON = (LocString) "Pause";
    public static LocString PAUSE = (LocString) "Pause {Hotkey}";
    public static LocString UNPAUSE = (LocString) "Unpause {Hotkey}";
    public static LocString SPEEDBUTTON_SLOW = (LocString) "Slow speed {Hotkey}";
    public static LocString SPEEDBUTTON_MEDIUM = (LocString) "Medium speed {Hotkey}";
    public static LocString SPEEDBUTTON_FAST = (LocString) "Fast speed {Hotkey}";
    public static LocString RED_ALERT_TITLE = (LocString) "Toggle Red Alert";
    public static LocString RED_ALERT_CONTENT = (LocString) "Duplicants will work, ignoring schedules and their basic needs\n\nUse in case of emergency";
    public static LocString DISINFECTBUTTON = (LocString) "Disinfect buildings {Hotkey}";
    public static LocString MOPBUTTON = (LocString) "Mop liquid spills {Hotkey}";
    public static LocString DIGBUTTON = (LocString) "Set dig errands {Hotkey}";
    public static LocString CANCELBUTTON = (LocString) "Cancel errands {Hotkey}";
    public static LocString DECONSTRUCTBUTTON = (LocString) "Demolish buildings {Hotkey}";
    public static LocString ATTACKBUTTON = (LocString) "Attack poor, wild critters {Hotkey}";
    public static LocString CAPTUREBUTTON = (LocString) "Capture critters {Hotkey}";
    public static LocString CLEARBUTTON = (LocString) "Move debris into storage {Hotkey}";
    public static LocString HARVESTBUTTON = (LocString) "Harvest plants {Hotkey}";
    public static LocString PRIORITIZEMAINBUTTON = (LocString) "";
    public static LocString PRIORITIZEBUTTON = (LocString) $"Set Building Priority {{Hotkey}}\n\nDuplicant Priorities{UI.PST_KEYWORD} {UI.FormatAsHotKey(Action.ManagePriorities)} are calculated <i>before</i> the {UI.PRE_KEYWORD}Building Priorities{UI.PST_KEYWORD} set by this tool";
    public static LocString CLEANUPMAINBUTTON = (LocString) "Mop and sweep messy floors {Hotkey}";
    public static LocString CANCELDECONSTRUCTIONBUTTON = (LocString) "Cancel queued orders or deconstruct existing buildings {Hotkey}";
    public static LocString HELP_ROTATE_KEY = (LocString) $"Press {UI.FormatAsHotKey(Action.RotateBuilding)} to Rotate";
    public static LocString HELP_BUILDLOCATION_INVALID_CELL = (LocString) "Invalid Cell";
    public static LocString HELP_BUILDLOCATION_MISSING_TELEPAD = (LocString) $"World has no {(string) BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME} or {(string) BUILDINGS.PREFABS.EXOBASEHEADQUARTERS.NAME}";
    public static LocString HELP_BUILDLOCATION_FLOOR = (LocString) "Must be built on solid ground";
    public static LocString HELP_BUILDLOCATION_WALL = (LocString) "Must be built against a wall";
    public static LocString HELP_BUILDLOCATION_BACK_WALL_REQUIRED = (LocString) "Must be built against a back wall";
    public static LocString HELP_BUILDLOCATION_FLOOR_OR_ATTACHPOINT = (LocString) "Must be built on solid ground or overlapping an {0}";
    public static LocString HELP_BUILDLOCATION_OCCUPIED = (LocString) "Must be built in unoccupied space";
    public static LocString HELP_BUILDLOCATION_BURIEDOBJECT = (LocString) "Cannot be built over buried objects";
    public static LocString HELP_BUILDLOCATION_CEILING = (LocString) "Must be built on the ceiling";
    public static LocString HELP_BUILDLOCATION_INSIDEGROUND = (LocString) "Must be built in the ground";
    public static LocString HELP_BUILDLOCATION_ATTACHPOINT = (LocString) "Must be built overlapping a {0}";
    public static LocString HELP_BUILDLOCATION_SPACE = (LocString) "Must be built on the surface in space";
    public static LocString HELP_BUILDLOCATION_CORNER = (LocString) "Must be built in a corner";
    public static LocString HELP_BUILDLOCATION_CORNER_FLOOR = (LocString) "Must be built in a corner on the ground";
    public static LocString HELP_BUILDLOCATION_BELOWROCKETCEILING = (LocString) "Must be placed further from the edge of space";
    public static LocString HELP_BUILDLOCATION_ONROCKETENVELOPE = (LocString) "Must be built on the interior wall of a rocket";
    public static LocString HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN = (LocString) "Obstructed by a building";
    public static LocString HELP_BUILDLOCATION_NOT_IN_TILES = (LocString) "Cannot be built inside tile";
    public static LocString HELP_BUILDLOCATION_GASPORTS_OVERLAP = (LocString) "Gas ports cannot overlap";
    public static LocString HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP = (LocString) "Liquid ports cannot overlap";
    public static LocString HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP = (LocString) "Solid ports cannot overlap";
    public static LocString HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED = (LocString) "Automation ports cannot overlap";
    public static LocString HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP = (LocString) "Power connectors cannot overlap";
    public static LocString HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE = (LocString) "Heavi-Watt connectors cannot be built inside tile";
    public static LocString HELP_BUILDLOCATION_WIRE_OBSTRUCTION = (LocString) "Obstructed by Heavi-Watt Wire";
    public static LocString HELP_BUILDLOCATION_BACK_WALL = (LocString) "Obstructed by back wall";
    public static LocString HELP_TUBELOCATION_NO_UTURNS = (LocString) "Can't U-Turn";
    public static LocString HELP_TUBELOCATION_STRAIGHT_BRIDGES = (LocString) "Can't Turn Here";
    public static LocString HELP_REQUIRES_ROOM = (LocString) $"Must be in a {UI.PRE_KEYWORD}Room{UI.PST_KEYWORD}";
    public static LocString OXYGENOVERLAYSTRING = (LocString) "Displays ambient oxygen density {Hotkey}";
    public static LocString POWEROVERLAYSTRING = (LocString) "Displays power grid components {Hotkey}";
    public static LocString TEMPERATUREOVERLAYSTRING = (LocString) "Displays ambient temperature {Hotkey}";
    public static LocString HEATFLOWOVERLAYSTRING = (LocString) "Displays comfortable temperatures for Duplicants {Hotkey}";
    public static LocString SUITOVERLAYSTRING = (LocString) "Displays Exosuits and related buildings {Hotkey}";
    public static LocString LOGICOVERLAYSTRING = (LocString) "Displays automation grid components {Hotkey}";
    public static LocString ROOMSOVERLAYSTRING = (LocString) "Displays special purpose rooms and bonuses {Hotkey}";
    public static LocString JOULESOVERLAYSTRING = (LocString) "Displays the thermal energy in each cell";
    public static LocString LIGHTSOVERLAYSTRING = (LocString) "Displays the visibility radius of light sources {Hotkey}";
    public static LocString LIQUIDVENTOVERLAYSTRING = (LocString) "Displays liquid pipe system components {Hotkey}";
    public static LocString GASVENTOVERLAYSTRING = (LocString) "Displays gas pipe system components {Hotkey}";
    public static LocString DECOROVERLAYSTRING = (LocString) "Displays areas with Morale-boosting decor values {Hotkey}";
    public static LocString PRIORITIESOVERLAYSTRING = (LocString) "Displays work priority values {Hotkey}";
    public static LocString DISEASEOVERLAYSTRING = (LocString) "Displays areas of disease risk {Hotkey}";
    public static LocString NOISE_POLLUTION_OVERLAY_STRING = (LocString) "Displays ambient noise levels {Hotkey}";
    public static LocString CROPS_OVERLAY_STRING = (LocString) "Displays plant growth progress {Hotkey}";
    public static LocString CONVEYOR_OVERLAY_STRING = (LocString) "Displays conveyor transport components {Hotkey}";
    public static LocString TILEMODE_OVERLAY_STRING = (LocString) "Displays material information {Hotkey}";
    public static LocString REACHABILITYOVERLAYSTRING = (LocString) "Displays areas accessible by Duplicants";
    public static LocString RADIATIONOVERLAYSTRING = (LocString) "Displays radiation levels {Hotkey}";
    public static LocString ENERGYREQUIRED = (LocString) (UI.FormatAsLink("Power", "POWER") + " Required");
    public static LocString ENERGYGENERATED = (LocString) (UI.FormatAsLink("Power", "POWER") + " Produced");
    public static LocString INFOPANEL = (LocString) "The Info Panel contains an overview of the basic information about my Duplicant";
    public static LocString VITALSPANEL = (LocString) "The Vitals Panel monitors the status and well being of my Duplicant";
    public static LocString STRESSPANEL = (LocString) "The Stress Panel offers a detailed look at what is affecting my Duplicant psychologically";
    public static LocString STATSPANEL = (LocString) "The Stats Panel gives me an overview of my Duplicant's individual stats";
    public static LocString ITEMSPANEL = (LocString) "The Items Panel displays everything this Duplicant is in possession of";
    public static LocString STRESSDESCRIPTION = (LocString) $"Accommodate my Duplicant's needs to manage their {UI.FormatAsLink("Stress", "STRESS")}.\n\nLow {UI.FormatAsLink("Stress", "STRESS")} can provide a productivity boost, while high {UI.FormatAsLink("Stress", "STRESS")} can impair production or even lead to a nervous breakdown.";
    public static LocString ALERTSTOOLTIP = (LocString) "Alerts provide important information about what's happening in the colony right now";
    public static LocString MESSAGESTOOLTIP = (LocString) "Messages are events that have happened and tips to help me manage my colony";
    public static LocString NEXTMESSAGESTOOLTIP = (LocString) "Next message";
    public static LocString CLOSETOOLTIP = (LocString) "Close";
    public static LocString DISMISSMESSAGE = (LocString) "Dismiss message";
    public static LocString RECIPE_QUEUE = (LocString) "Queue {0} for continuous fabrication";
    public static LocString RED_ALERT_BUTTON_ON = (LocString) "Enable Red Alert";
    public static LocString RED_ALERT_BUTTON_OFF = (LocString) "Disable Red Alert";
    public static LocString JOBSSCREEN_PRIORITY = (LocString) "High priority tasks are always performed before low priority tasks.\n\nHowever, a busy Duplicant will continue to work on their current work errand until it's complete, even if a more important errand becomes available.";
    public static LocString JOBSSCREEN_ATTRIBUTES = (LocString) "The following attributes affect a Duplicant's efficiency at this errand:";
    public static LocString JOBSSCREEN_CANNOTPERFORMTASK = (LocString) "{0} cannot perform this errand.";
    public static LocString JOBSSCREEN_RELEVANT_ATTRIBUTES = (LocString) "Relevant Attributes:";
    public static LocString SORTCOLUMN = (LocString) (UI.CLICK(UI.ClickType.Click) + " to sort");
    public static LocString NOMATERIAL = (LocString) "Not enough materials";
    public static LocString SELECTAMATERIAL = (LocString) "There are insufficient materials to construct this building";
    public static LocString EDITNAME = (LocString) "Give this Duplicant a new name";
    public static LocString RANDOMIZENAME = (LocString) "Randomize this Duplicant's name";
    public static LocString EDITNAMEGENERIC = (LocString) "Rename {0}";
    public static LocString EDITNAMEROCKET = (LocString) "Rename this rocket";
    public static LocString BASE_VALUE = (LocString) "Base Value";
    public static LocString MATIERIAL_MOD = (LocString) "Made out of {0}";
    public static LocString VITALS_CHECKBOX_TEMPERATURE = (LocString) $"This plant's internal {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} is <b>{{temperature}}</b>";
    public static LocString VITALS_CHECKBOX_PRESSURE = (LocString) $"The current {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD} pressure is <b>{{pressure}}</b>";
    public static LocString VITALS_CHECKBOX_ATMOSPHERE = (LocString) "This plant is immersed in {element}";
    public static LocString VITALS_CHECKBOX_ILLUMINATION_DARK = (LocString) "This plant is currently in the dark";
    public static LocString VITALS_CHECKBOX_ILLUMINATION_LIGHT = (LocString) "This plant is currently lit";
    public static LocString VITALS_CHECKBOX_SPACETREE_ILLUMINATION_DARK = (LocString) $"This plant must be lit in order to produce {UI.PRE_KEYWORD}Nectar{UI.PST_KEYWORD}";
    public static LocString VITALS_CHECKBOX_SPACETREE_ILLUMINATION_LIGHT = (LocString) $"This plant is currently lit, and will produce {UI.PRE_KEYWORD}Nectar{UI.PST_KEYWORD} when fully grown";
    public static LocString VITALS_CHECKBOX_FERTILIZER = (LocString) $"<b>{{mass}}</b> of {UI.PRE_KEYWORD}Fertilizer{UI.PST_KEYWORD} is currently available";
    public static LocString VITALS_CHECKBOX_IRRIGATION = (LocString) $"<b>{{mass}}</b> of {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD} is currently available";
    public static LocString VITALS_CHECKBOX_SUBMERGED_TRUE = (LocString) $"This plant is fully submerged in {UI.PRE_KEYWORD}Liquid{UI.PRE_KEYWORD}";
    public static LocString VITALS_CHECKBOX_SUBMERGED_FALSE = (LocString) $"This plant must be submerged in {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD}";
    public static LocString VITALS_CHECKBOX_DROWNING_TRUE = (LocString) "This plant is not drowning";
    public static LocString VITALS_CHECKBOX_DROWNING_FALSE = (LocString) $"This plant is drowning in {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD}";
    public static LocString VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL = (LocString) "This plant is housed in an operational farm plot";
    public static LocString VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL = (LocString) "This plant is not housed in an operational farm plot";
    public static LocString VITALS_CHECKBOX_RADIATION = (LocString) $"This plant is sitting in <b>{{rads}}</b> of ambient {UI.PRE_KEYWORD}Radiation{UI.PST_KEYWORD}. It needs between {{minRads}} and {{maxRads}} to grow";
    public static LocString VITALS_CHECKBOX_RADIATION_NO_MIN = (LocString) $"This plant is sitting in <b>{{rads}}</b> of ambient {UI.PRE_KEYWORD}Radiation{UI.PST_KEYWORD}. It needs less than {{maxRads}} to grow";
    public static LocString VITALS_CHECKBOX_ENTITY_CONSUMER_REQUIREMENTS = (LocString) $"This plant must consume {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} in order to grow";
    public static LocString VITALS_CHECKBOX_ENTITY_CONSUMER_SATISFIED = (LocString) $"{UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} consumed";
    public static LocString VITALS_CHECKBOX_ENTITY_CONSUMER_UNSATISFIED = (LocString) "Awaiting prey";
    public static LocString VITALS_CHECKBOX_POLLINATED = (LocString) $"This plant was recently pollinated by a {UI.PRE_KEYWORD}Critter{UI.PST_KEYWORD} ";
    public static LocString VITALS_CHECKBOX_UNPOLLINATED = (LocString) $"This plant must be pollinated by a {UI.PRE_KEYWORD}Critter{UI.PST_KEYWORD}{{0}}";
    public static LocString VITALS_CHECKBOX_GERM_PRESENCE = (LocString) "This plant is currently exposed to {0}\n\nThis is essential for growth";
    public static LocString VITALS_CHECKBOX_GERM_ABSENT = (LocString) "This plant requires exposure to {0}";
  }

  public class CLUSTERMAP
  {
    public static LocString PLANETOID = (LocString) "Planetoid";
    public static LocString PLANETOID_KEYWORD = (LocString) (UI.PRE_KEYWORD + (string) UI.CLUSTERMAP.PLANETOID + UI.PST_KEYWORD);
    public static LocString TITLE = (LocString) "STARMAP";
    public static LocString LANDING_SITES = (LocString) "LANDING SITES";
    public static LocString DESTINATION = (LocString) nameof (DESTINATION);
    public static LocString OCCUPANTS = (LocString) "CREW";
    public static LocString ELEMENTS = (LocString) nameof (ELEMENTS);
    public static LocString UNKNOWN_DESTINATION = (LocString) "Unknown";
    public static LocString TILES = (LocString) "Starmap Hexes";
    public static LocString TILES_PER_CYCLE = (LocString) "Starmap hexes per cycle";
    public static LocString CHANGE_DESTINATION = (LocString) (UI.CLICK(UI.ClickType.Click) + " to change destination");
    public static LocString SELECT_DESTINATION = (LocString) "Select a new destination on the map";
    public static LocString TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR = (LocString) $"Cannot travel to this hex until it has been analyzed\n\nSpace can be analyzed with a {(string) BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME} or {(string) BUILDINGS.PREFABS.SCANNERMODULE.NAME}";
    public static LocString TOOLTIP_INVALID_DESTINATION_NO_PATH = (LocString) $"There is no navigable rocket path to this {(string) UI.CLUSTERMAP.PLANETOID_KEYWORD}\n\nSpace can be analyzed with a {(string) BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME} or {(string) BUILDINGS.PREFABS.SCANNERMODULE.NAME} to clear the way";
    public static LocString TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD = (LocString) $"There is no {(string) BUILDINGS.PREFABS.LAUNCHPAD.NAME} on this {(string) UI.CLUSTERMAP.PLANETOID_KEYWORD} for a rocket to land on\n\nSend a {(string) BUILDINGS.PREFABS.PIONEERMODULE.NAME} or {(string) BUILDINGS.PREFABS.SCOUTMODULE.NAME} to its orbit to deploy a scout and make first contact";
    public static LocString TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID = (LocString) $"Must select a {(string) UI.CLUSTERMAP.PLANETOID_KEYWORD} destination";
    public static LocString TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE = (LocString) $"This destination is further away than this rocket is currently equipped to go\n\n    • Current Range: {{0}} Hexes\n    • Potential {(string) DUPLICANTS.ATTRIBUTES.FUELRANGEPERKILOGRAM.NAME}: {{1}} Hexes";
    public static LocString TOOLTIP_INVALID_METEOR_TARGET = (LocString) "This destination does not have an impactor asteroid to target";
    public static LocString TOOLTIP_HIDDEN_HEX = (LocString) "???";
    public static LocString TOOLTIP_PEEKED_HEX_WITH_OBJECT = (LocString) "UNKNOWN OBJECT DETECTED!";
    public static LocString TOOLTIP_EMPTY_HEX = (LocString) "EMPTY SPACE";
    public static LocString TOOLTIP_PATH_LENGTH = (LocString) "Trip Distance: {0}/{1}";
    public static LocString TOOLTIP_PATH_LENGTH_RETURN = (LocString) "Trip Distance: {0}/{1} (Return Trip)";

    public class STATUS
    {
      public static LocString NORMAL = (LocString) "Normal";

      public class ROCKET
      {
        public static LocString GROUNDED = (LocString) "Normal";
        public static LocString TRAVELING = (LocString) "Traveling";
        public static LocString STRANDED = (LocString) "Stranded";
        public static LocString IDLE = (LocString) "Idle";
      }
    }

    public class ASTEROIDS
    {
      public class ELEMENT_AMOUNTS
      {
        public static LocString LOTS = (LocString) "Plentiful";
        public static LocString SOME = (LocString) "Significant amount";
        public static LocString LITTLE = (LocString) "Small amount";
        public static LocString VERY_LITTLE = (LocString) "Trace amount";
      }

      public class SURFACE_CONDITIONS
      {
        public static LocString LIGHT = (LocString) "Peak Light";
        public static LocString RADIATION = (LocString) "Cosmic Radiation";
      }
    }

    public class POI
    {
      public static LocString TITLE = (LocString) "POINT OF INTEREST";
      public static LocString MASS_REMAINING = (LocString) "<b>Total Mass Remaining</b>";
      public static LocString ROCKETS_AT_THIS_LOCATION = (LocString) "<b>Rockets at this location</b>";
      public static LocString ARTIFACTS = (LocString) "Artifact";
      public static LocString ARTIFACTS_AVAILABLE = (LocString) "Available";
      public static LocString ARTIFACTS_DEPLETED = (LocString) "Collected\nRecharge: {0}";
    }

    public class HEXCELL_INVENTORY
    {
      public static LocString NAME = (LocString) "Harvestable Resources";
      public static LocString DESC = (LocString) $"Free-floating resources that can be gathered by a rocket equipped with a cargo module.\n\nAdditional resources may become available by {UI.FormatAsLink("mining", "EXOBASESDLC1")} other space POIs on the same hex cell.";

      public class UI_PANEL
      {
        public static LocString TITLE = (LocString) "CONTENTS";
      }
    }

    public class ROCKETS
    {
      public class SPEED
      {
        public static LocString NAME = (LocString) "Rocket Speed: ";
        public static LocString TOOLTIP = (LocString) "<b>Rocket speed</b> is calculated by dividing <b>engine power</b> by <b>burden</b>";
        public static LocString PILOT_SPEED_MODIFIER = (LocString) $"\n\nRockets operating on autopilot will have a reduced speed\n\n<b>Rocket speed</b> can be increased by a Duplicant pilot's <b>Skill</b> and by a {UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD}";
        public static LocString UNPILOTED_SPEED_TOOLTIP = (LocString) "Rocket is operating on autopilot: -{speed_boost} speed";
        public static LocString SUPERPILOTED_SPEED_TOOLTIP = (LocString) "Multi-Piloted: +{speed_boost} speed boost";
        public static LocString DUPEPILOT_SPEED_TOOLTIP = (LocString) "Current Duplicant pilot <b>Skill</b>: +{speed_boost} speed boost";
        public static LocString ROBO_PILOT_ONLY_SPEED_TOOLTIP = (LocString) $"Piloted by a {UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD}: +0% speed boost";
        public static LocString DEAD_ROBO_PILOT_ONLY_SPEED_TOOLTIP = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} has no {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD}. This rocket is stranded";
      }

      public class FUEL_REMAINING
      {
        public static LocString NAME = (LocString) "Fuel Remaining: ";
        public static LocString TOOLTIP = (LocString) "This rocket has {0} fuel in its tank";
      }

      public class OXIDIZER_REMAINING
      {
        public static LocString NAME = (LocString) "Oxidizer Power Remaining: ";
        public static LocString TOOLTIP = (LocString) "This rocket has enough oxidizer in its tank for {0} of fuel";
      }

      public class RANGE
      {
        public static LocString NAME = (LocString) "Range Remaining: ";
        public static LocString TOOLTIP = (LocString) "<b>Range remaining</b> is calculated by dividing the lesser of <b>fuel remaining</b> and <b>oxidizer power remaining</b> by <b>fuel consumed per hex</b>";
        public static LocString ROBO_PILOTED_TOOLTIP = (LocString) $"\nRockets piloted by a {UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} can travel one hex per {{0}} {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD}\n    • {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD} Remaining: {{1}}";
      }

      public class FUEL_PER_HEX
      {
        public static LocString NAME = (LocString) "Fuel consumed per Hex: {0}";
        public static LocString TOOLTIP = (LocString) "This rocket can travel one hex per {0} of fuel";
      }

      public class BURDEN_TOTAL
      {
        public static LocString NAME = (LocString) "Rocket burden: ";
        public static LocString TOOLTIP = (LocString) "The combined burden of all the modules in this rocket";
      }

      public class BURDEN_MODULE
      {
        public static LocString NAME = (LocString) "Module Burden: ";
        public static LocString TOOLTIP = (LocString) ("The selected module adds {0} to the rocket's total " + (string) DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME);
      }

      public class POWER_TOTAL
      {
        public static LocString NAME = (LocString) "Rocket engine power: ";
        public static LocString TOOLTIP = (LocString) "The total engine power added by all the modules in this rocket";
      }

      public class POWER_MODULE
      {
        public static LocString NAME = (LocString) "Module Engine Power: ";
        public static LocString TOOLTIP = (LocString) ("The selected module adds {0} to the rocket's total " + (string) DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME);
      }

      public class MODULE_STATS
      {
        public static LocString NAME = (LocString) "Module Stats: ";
        public static LocString NAME_HEADER = (LocString) "Module Stats";
        public static LocString TOOLTIP = (LocString) "Properties of the selected module";
      }

      public class MAX_MODULES
      {
        public static LocString NAME = (LocString) "Max Modules: ";
        public static LocString TOOLTIP = (LocString) "The {0} can support {1} rocket modules, plus itself";
      }

      public class MAX_HEIGHT
      {
        public static LocString NAME = (LocString) "Height: {0}/{1}";
        public static LocString NAME_RAW = (LocString) "Height: ";
        public static LocString NAME_MAX_SUPPORTED = (LocString) "Maximum supported rocket height: ";
        public static LocString TOOLTIP = (LocString) "The {0} can support a total rocket height {1}";
      }

      public class ARTIFACT_MODULE
      {
        public static LocString EMPTY = (LocString) "Empty";
      }
    }
  }

  public class STARMAP
  {
    public static LocString TITLE = (LocString) nameof (STARMAP);
    public static LocString MANAGEMENT_BUTTON = (LocString) nameof (STARMAP);
    public static LocString SUBROW = (LocString) "•  {0}";
    public static LocString UNKNOWN_DESTINATION = (LocString) "Destination Unknown";
    public static LocString ANALYSIS_AMOUNT = (LocString) "Analysis {0} Complete";
    public static LocString ANALYSIS_COMPLETE = (LocString) "ANALYSIS COMPLETE";
    public static LocString NO_ANALYZABLE_DESTINATION_SELECTED = (LocString) "No destination selected";
    public static LocString UNKNOWN_TYPE = (LocString) "Type Unknown";
    public static LocString DISTANCE = (LocString) "{0} km";
    public static LocString MODULE_MASS = (LocString) "+ {0} t";
    public static LocString MODULE_STORAGE = (LocString) "{0} / {1}";
    public static LocString ANALYSIS_DESCRIPTION = (LocString) "Use a Telescope to analyze space destinations.\n\nCompleting analysis on an object will unlock rocket missions to that destination.";
    public static LocString RESEARCH_DESCRIPTION = (LocString) "Gather Interstellar Research Data using Research Modules.";
    public static LocString ROCKET_RENAME_BUTTON_TOOLTIP = (LocString) "Rename this rocket";
    public static LocString NO_ROCKETS_HELP_TEXT = (LocString) "Rockets allow you to visit nearby celestial bodies.\n\nEach rocket must have a Command Module, an Engine, and Fuel.\n\nYou can also carry other modules that allow you to gather specific resources from the places you visit.\n\nRemember the more weight a rocket has, the more limited it'll be on the distance it can travel. You can add more fuel to fix that, but fuel will add weight as well.";
    public static LocString CONTAINER_REQUIRED = (LocString) "{0} installation required to retrieve material";
    public static LocString CAN_CARRY_ELEMENT = (LocString) "Gathered by: {1}";
    public static LocString CANT_CARRY_ELEMENT = (LocString) "{0} installation required to retrieve material";
    public static LocString STATUS = (LocString) "SELECTED";
    public static LocString DISTANCE_OVERLAY = (LocString) "TOO FAR FOR THIS ROCKET";
    public static LocString COMPOSITION_UNDISCOVERED = (LocString) "?????????";
    public static LocString COMPOSITION_UNDISCOVERED_TOOLTIP = (LocString) "Further research required to identify resource\n\nSend a Research Module to this destination for more information";
    public static LocString COMPOSITION_UNDISCOVERED_AMOUNT = (LocString) "???";
    public static LocString COMPOSITION_SMALL_AMOUNT = (LocString) "Trace Amount";
    public static LocString CURRENT_MASS = (LocString) "Current Mass";
    public static LocString CURRENT_MASS_TOOLTIP = (LocString) "Warning: Missions to this destination will not return a full cargo load to avoid depleting the destination for future explorations\n\nDestination: {0} Resources Available\nRocket Capacity: {1}";
    public static LocString MAXIMUM_MASS = (LocString) "Maximum Mass";
    public static LocString MINIMUM_MASS = (LocString) "Minimum Mass";
    public static LocString MINIMUM_MASS_TOOLTIP = (LocString) "This destination must retain at least this much mass in order to prevent depletion and allow the future regeneration of resources.\n\nDuplicants will always maintain a destination's minimum mass requirements, potentially returning with less cargo than their rocket can hold";
    public static LocString REPLENISH_RATE = (LocString) "Replenished/Cycle:";
    public static LocString REPLENISH_RATE_TOOLTIP = (LocString) "The rate at which this destination regenerates resources";
    public static LocString ROCKETLIST = (LocString) "Rocket Hangar";
    public static LocString NO_ROCKETS_TITLE = (LocString) "NO ROCKETS";
    public static LocString ROCKET_COUNT = (LocString) "ROCKETS: {0}";
    public static LocString LAUNCH_MISSION = (LocString) "LAUNCH MISSION";
    public static LocString CANT_LAUNCH_MISSION = (LocString) "CANNOT LAUNCH";
    public static LocString LAUNCH_ROCKET = (LocString) "Launch Rocket";
    public static LocString LAND_ROCKET = (LocString) "Land Rocket";
    public static LocString SEE_ROCKETS_LIST = (LocString) "See Rockets List";
    public static LocString DEFAULT_NAME = (LocString) "Rocket";
    public static LocString ANALYZE_DESTINATION = (LocString) "ANALYZE OBJECT";
    public static LocString SUSPEND_DESTINATION_ANALYSIS = (LocString) "PAUSE ANALYSIS";
    public static LocString DESTINATIONTITLE = (LocString) "Destination Status";

    public class DESTINATIONSTUDY
    {
      public static LocString UPPERATMO = (LocString) "Study upper atmosphere";
      public static LocString LOWERATMO = (LocString) "Study lower atmosphere";
      public static LocString MAGNETICFIELD = (LocString) "Study magnetic field";
      public static LocString SURFACE = (LocString) "Study surface";
      public static LocString SUBSURFACE = (LocString) "Study subsurface";
    }

    public class COMPONENT
    {
      public static LocString FUEL_TANK = (LocString) "Fuel Tank";
      public static LocString ROCKET_ENGINE = (LocString) "Rocket Engine";
      public static LocString CARGO_BAY = (LocString) "Cargo Bay";
      public static LocString OXIDIZER_TANK = (LocString) "Oxidizer Tank";
    }

    public class MISSION_STATUS
    {
      public static LocString GROUNDED = (LocString) "Grounded";
      public static LocString LAUNCHING = (LocString) "Launching";
      public static LocString WAITING_TO_LAND = (LocString) "Waiting To Land";
      public static LocString LANDING = (LocString) "Landing";
      public static LocString UNDERWAY = (LocString) "Underway";
      public static LocString UNDERWAY_BOOSTED = (LocString) "Underway <color=#5FDB37FF>(Boosted)</color>";
      public static LocString DESTROYED = (LocString) "Destroyed";
      public static LocString GO = (LocString) "ALL SYSTEMS GO";
    }

    public class LISTTITLES
    {
      public static LocString MISSIONSTATUS = (LocString) "Mission Status";
      public static LocString LAUNCHCHECKLIST = (LocString) "Launch Checklist";
      public static LocString MAXRANGE = (LocString) "Max Range";
      public static LocString MASS = (LocString) "Mass";
      public static LocString STORAGE = (LocString) "Storage";
      public static LocString FUEL = (LocString) "Fuel";
      public static LocString OXIDIZER = (LocString) "Oxidizer";
      public static LocString PASSENGERS = (LocString) "Passengers";
      public static LocString RESEARCH = (LocString) "Research";
      public static LocString ARTIFACTS = (LocString) "Artifacts";
      public static LocString ANALYSIS = (LocString) "Analysis";
      public static LocString WORLDCOMPOSITION = (LocString) "World Composition";
      public static LocString RESOURCES = (LocString) "Resources";
      public static LocString MODULES = (LocString) "Modules";
      public static LocString TYPE = (LocString) "Type";
      public static LocString DISTANCE = (LocString) "Distance";
      public static LocString DESTINATION_MASS = (LocString) "World Mass Available";
      public static LocString STORAGECAPACITY = (LocString) "Storage Capacity";
    }

    public class ROCKETWEIGHT
    {
      public static LocString MASS = (LocString) "Mass: ";
      public static LocString MASSPENALTY = (LocString) "Mass Penalty: ";
      public static LocString CURRENTMASS = (LocString) "Current Rocket Mass: ";
      public static LocString CURRENTMASSPENALTY = (LocString) "Current Weight Penalty: ";
    }

    public class DESTINATIONSELECTION
    {
      public static LocString REACHABLE = (LocString) "Destination set";
      public static LocString UNREACHABLE = (LocString) "Destination set";
      public static LocString NOTSELECTED = (LocString) "Destination set";
    }

    public class DESTINATIONSELECTION_TOOLTIP
    {
      public static LocString REACHABLE = (LocString) "Viable destination selected, ready for launch";
      public static LocString UNREACHABLE = (LocString) "The selected destination is beyond rocket reach";
      public static LocString NOTSELECTED = (LocString) "Select the rocket's Command Module to set a destination";
    }

    public class HASFOOD
    {
      public static LocString NAME = (LocString) "Food Loaded";
      public static LocString TOOLTIP = (LocString) "Sufficient food stores have been loaded, ready for launch";
    }

    public class HASSUIT
    {
      public static LocString NAME = (LocString) ("Has " + (string) EQUIPMENT.PREFABS.ATMO_SUIT.NAME);
      public static LocString TOOLTIP = (LocString) $"An {(string) EQUIPMENT.PREFABS.ATMO_SUIT.NAME} has been loaded";
    }

    public class NOSUIT
    {
      public static LocString NAME = (LocString) ("Missing " + (string) EQUIPMENT.PREFABS.ATMO_SUIT.NAME);
      public static LocString TOOLTIP = (LocString) $"Rocket cannot launch without an {(string) EQUIPMENT.PREFABS.ATMO_SUIT.NAME} loaded";
    }

    public class NOFOOD
    {
      public static LocString NAME = (LocString) "Insufficient Food";
      public static LocString TOOLTIP = (LocString) "Rocket cannot launch without adequate food stores for passengers";
    }

    public class CARGOEMPTY
    {
      public static LocString NAME = (LocString) "Emptied Cargo Bay";
      public static LocString TOOLTIP = (LocString) "Cargo Bays must be emptied of all materials before launch";
    }

    public class LAUNCHCHECKLIST
    {
      public static LocString ASTRONAUT_TITLE = (LocString) "Astronaut";
      public static LocString HASASTRONAUT = (LocString) "Astronaut ready for liftoff";
      public static LocString ASTRONAUGHT = (LocString) "No Astronaut assigned";
      public static LocString INSTALLED = (LocString) "Installed";
      public static LocString INSTALLED_TOOLTIP = (LocString) "A suitable {0} has been installed";
      public static LocString REQUIRED = (LocString) "Required";
      public static LocString REQUIRED_TOOLTIP = (LocString) "A {0} must be installed before launch";
      public static LocString MISSING_TOOLTIP = (LocString) "No {0} installed\n\nThis rocket cannot launch without a completed {0}";
      public static LocString NO_DESTINATION = (LocString) "No destination selected";
      public static LocString MINIMUM_MASS = (LocString) "Resources available {0}";
      public static LocString RESOURCE_MASS_TOOLTIP = (LocString) "{0} has {1} resources available\nThis rocket has capacity for {2}";
      public static LocString INSUFFICENT_MASS_TOOLTIP = (LocString) "Launching to this destination will not return a full cargo load";

      public class CONSTRUCTION_COMPLETE
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "No active construction";
          public static LocString FAILURE = (LocString) "No active construction";
          public static LocString WARNING = (LocString) "No active construction";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "Construction of all modules is complete";
          public static LocString FAILURE = (LocString) "In-progress module construction is preventing takeoff";
          public static LocString WARNING = (LocString) "Construction warning";
        }
      }

      public class PILOT_BOARDED
      {
        public static LocString READY = (LocString) "Pilot boarded";
        public static LocString FAILURE = (LocString) "Pilot boarded";
        public static LocString WARNING = (LocString) "Pilot boarded";
        public static LocString ROBO_PILOT_WARNING = (LocString) "Copilot boarded";

        public class TOOLTIP
        {
          public static LocString READY = (LocString) $"A Duplicant with the {(string) DUPLICANTS.ROLES.ROCKETPILOT.NAME} skill is currently onboard";
          public static LocString FAILURE = (LocString) $"At least one crew member aboard the rocket must possess the {(string) DUPLICANTS.ROLES.ROCKETPILOT.NAME} skill to launch\n\nQualified Duplicants must be assigned to the rocket crew, and have access to the module's hatch";
          public static LocString WARNING = (LocString) "Pilot warning";
          public static LocString ROBO_PILOT_WARNING = (LocString) $"This rocket is being piloted by a {UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD}\n\nThere are no Duplicants with the {(string) DUPLICANTS.ROLES.ROCKETPILOT.NAME} skill currently onboard\n\nQualified Duplicants must be assigned to the rocket crew, and have access to the module's hatch";
        }
      }

      public class CREW_BOARDED
      {
        public static LocString READY = (LocString) "All crew boarded";
        public static LocString FAILURE = (LocString) "All crew boarded";
        public static LocString WARNING = (LocString) "All crew boarded";

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "All Duplicants assigned to the rocket crew are boarded and ready for launch\n\n    • {0}/{1} Boarded";
          public static LocString FAILURE = (LocString) "No crew members have boarded this rocket\n\nDuplicants must be assigned to the rocket crew and have access to the module's hatch to board\n\n    • {0}/{1} Boarded";
          public static LocString WARNING = (LocString) "Some Duplicants assigned to this rocket crew have not yet boarded\n    • {0}/{1} Boarded";
          public static LocString NONE = (LocString) "There are no Duplicants assigned to this rocket crew\n    • {0}/{1} Boarded";
        }
      }

      public class NO_EXTRA_PASSENGERS
      {
        public static LocString READY = (LocString) "Non-crew exited";
        public static LocString FAILURE = (LocString) "Non-crew exited";
        public static LocString WARNING = (LocString) "Non-crew exited";

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "All non-crew Duplicants have disembarked";
          public static LocString FAILURE = (LocString) "Non-crew Duplicants must exit the rocket before launch";
          public static LocString WARNING = (LocString) "Non-crew warning";
        }
      }

      public class FLIGHT_PATH_CLEAR
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Clear launch path";
          public static LocString FAILURE = (LocString) "Clear launch path";
          public static LocString WARNING = (LocString) "Clear launch path";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "The rocket's launch path is clear for takeoff";
          public static LocString FAILURE = (LocString) "This rocket does not have a clear line of sight to space, preventing launch\n\nThe rocket's launch path can be cleared by excavating undug tiles and deconstructing any buildings above the rocket";
          public static LocString WARNING = (LocString) "";
        }
      }

      public class HAS_FUEL_TANK
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Fuel Tank";
          public static LocString FAILURE = (LocString) "Fuel Tank";
          public static LocString WARNING = (LocString) "Fuel Tank";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "A fuel tank has been installed";
          public static LocString FAILURE = (LocString) "No fuel tank installed\n\nThis rocket cannot launch without a completed fuel tank";
          public static LocString WARNING = (LocString) "Fuel tank warning";
        }
      }

      public class HAS_ENGINE
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Engine";
          public static LocString FAILURE = (LocString) "Engine";
          public static LocString WARNING = (LocString) "Engine";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "A suitable engine has been installed";
          public static LocString FAILURE = (LocString) "No engine installed\n\nThis rocket cannot launch without a completed engine";
          public static LocString WARNING = (LocString) "Engine warning";
        }
      }

      public class HAS_NOSECONE
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Nosecone";
          public static LocString FAILURE = (LocString) "Nosecone";
          public static LocString WARNING = (LocString) "Nosecone";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "A suitable nosecone has been installed";
          public static LocString FAILURE = (LocString) "No nosecone installed\n\nThis rocket cannot launch without a completed nosecone";
          public static LocString WARNING = (LocString) "Nosecone warning";
        }
      }

      public class HAS_CARGO_BAY_FOR_NOSECONE_HARVEST
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Drillcone Cargo Bay";
          public static LocString FAILURE = (LocString) "Drillcone Cargo Bay";
          public static LocString WARNING = (LocString) "Drillcone Cargo Bay";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "A suitable cargo bay has been installed";
          public static LocString FAILURE = (LocString) $"No cargo bay installed\n\nThis rocket has a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} installed but nowhere to store the materials";
          public static LocString WARNING = (LocString) $"No cargo bay installed\n\nThis rocket has a {UI.FormatAsLink("Drillcone", "NOSECONEHARVEST")} installed but nowhere to store the materials";
        }
      }

      public class HAS_CONTROLSTATION
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Control Station";
          public static LocString FAILURE = (LocString) "Control Station";
          public static LocString WARNING = (LocString) "Control Station";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "The control station is installed and waiting for the pilot";
          public static LocString FAILURE = (LocString) "No control station\n\nA new Rocket Control Station must be installed inside the rocket";
          public static LocString WARNING = (LocString) "Control Station warning";
          public static LocString WARNING_ROBO_PILOT = (LocString) "No control station\n\nThis rocket is being piloted by a Robo-Pilot Module";
        }
      }

      public class LOADING_COMPLETE
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Cargo Loading Complete";
          public static LocString FAILURE = (LocString) "";
          public static LocString WARNING = (LocString) "Cargo Loading Complete";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "All possible loading and unloading has been completed";
          public static LocString FAILURE = (LocString) "";
          public static LocString WARNING = (LocString) $"The {(string) BUILDINGS.PREFABS.LAUNCHPAD.NAME} could still transfer cargo to or from this rocket";
        }
      }

      public class CARGO_TRANSFER_COMPLETE
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Cargo Transfer Complete";
          public static LocString FAILURE = (LocString) "";
          public static LocString WARNING = (LocString) "Cargo Transfer Complete";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "All possible loading and unloading has been completed";
          public static LocString FAILURE = (LocString) "";
          public static LocString WARNING = (LocString) $"The {(string) BUILDINGS.PREFABS.LAUNCHPAD.NAME} could still transfer cargo to or from this rocket";
        }
      }

      public class INTERNAL_CONSTRUCTION_COMPLETE
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Landers Ready";
          public static LocString FAILURE = (LocString) "Landers Ready";
          public static LocString WARNING = (LocString) "";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "All requested landers have been built and are ready for deployment";
          public static LocString FAILURE = (LocString) "Additional landers must be constructed to fulfill the lander requests of this rocket";
          public static LocString WARNING = (LocString) "";
        }
      }

      public class MAX_MODULES
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Module limit";
          public static LocString FAILURE = (LocString) "Module limit";
          public static LocString WARNING = (LocString) "Module limit";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "The rocket's engine can support the number of installed rocket modules";
          public static LocString FAILURE = (LocString) "The number of installed modules exceeds the engine's module limit\n\nExcess modules must be removed";
          public static LocString WARNING = (LocString) "Module limit warning";
        }
      }

      public class HAS_RESOURCE
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "{0} {1} supplied";
          public static LocString FAILURE = (LocString) "{0} missing {1}";
          public static LocString WARNING = (LocString) "{0} missing {1}";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "{0} {1} supplied";
          public static LocString FAILURE = (LocString) "{0} has less than {1} {2}";
          public static LocString WARNING = (LocString) "{0} has less than {1} {2}";
        }
      }

      public class MAX_HEIGHT
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Height limit";
          public static LocString FAILURE = (LocString) "Height limit";
          public static LocString WARNING = (LocString) "Height limit";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "The rocket's engine can support the height of the rocket";
          public static LocString FAILURE = (LocString) "The height of the rocket exceeds the engine's limit\n\nExcess modules must be removed";
          public static LocString WARNING = (LocString) "Height limit warning";
        }
      }

      public class PROPERLY_FUELED
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Fueled";
          public static LocString FAILURE = (LocString) "Fueled";
          public static LocString WARNING = (LocString) "Fueled";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "The rocket is sufficiently fueled for a roundtrip to its destination and back";
          public static LocString READY_NO_DESTINATION = (LocString) "This rocket's fuel tanks have been filled to capacity, but it has no destination";
          public static LocString FAILURE = (LocString) "This rocket does not have enough fuel to reach its destination\n\nIf the tanks are full, a different Fuel Tank Module may be required";
          public static LocString WARNING = (LocString) "The rocket has enough fuel for a one-way trip to its destination, but will not be able to make it back";
        }
      }

      public class SUFFICIENT_OXIDIZER
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Sufficient Oxidizer";
          public static LocString FAILURE = (LocString) "Sufficient Oxidizer";
          public static LocString WARNING = (LocString) "Warning: Limited oxidizer";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "This rocket has sufficient oxidizer for a roundtrip to its destination and back";
          public static LocString FAILURE = (LocString) "This rocket does not have enough oxidizer to reach its destination\n\nIf the oxidizer tanks are full, a different Oxidizer Tank Module may be required";
          public static LocString WARNING = (LocString) "The rocket has enough oxidizer for a one-way trip to its destination, but will not be able to make it back";
        }
      }

      public class ON_LAUNCHPAD
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "On a launch pad";
          public static LocString FAILURE = (LocString) "Not on a launch pad";
          public static LocString WARNING = (LocString) "No launch pad";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) "On a launch pad";
          public static LocString FAILURE = (LocString) "Not on a launch pad";
          public static LocString WARNING = (LocString) "No launch pad";
        }
      }

      public class ROBOT_PILOT_DATA_REQUIREMENTS
      {
        public class STATUS
        {
          public static LocString WARNING_NO_DATA_BANKS_HUMAN_PILOT = (LocString) "Robo-Pilot programmed";
          public static LocString READY = (LocString) "Robo-Pilot programmed";
          public static LocString FAILURE = (LocString) "Robo-Pilot programmed";
          public static LocString WARNING = (LocString) "Robo-Pilot programmed";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} has sufficient {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD} for a roundtrip to its destination and back\n    • {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD} stored: {{0}}/{{1}}";
          public static LocString READY_NO_DESTINATION = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} has sufficient {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD}, but no destination has been set\n    • {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD} stored: {{0}}";
          public static LocString FAILURE_NO_DESTINATION = (LocString) "No destination has been set";
          public static LocString FAILURE = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} requires at least {{0}} {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD} to reach its destination\n    • {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD} stored: {{1}}";
          public static LocString WARNING = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} has insufficient {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD} for a roundtrip to its destination and back\n    • {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD}stored: {{0}}/{{1}}";
          public static LocString WARNING_NO_DATA_BANKS_HUMAN_PILOT = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} cannot function without {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD}\n\nThis rocket is currently being operated by a Duplicant who possesses the {(string) DUPLICANTS.ROLES.ROCKETPILOT.NAME} skill";
        }
      }

      public class ROBOT_PILOT_POWER_SOUCRE
      {
        public class STATUS
        {
          public static LocString READY = (LocString) "Robo-Pilot has power";
          public static LocString WARNING = (LocString) "Robo-Pilot has power";
          public static LocString FAILURE = (LocString) "Robo-Pilot has power";
        }

        public class TOOLTIP
        {
          public static LocString READY = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} has a {UI.PRE_KEYWORD}Power{UI.PST_KEYWORD} source";
          public static LocString WARNING = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} has insufficient {UI.PRE_KEYWORD}Power{UI.PST_KEYWORD} for a round-trip to its destination";
          public static LocString FAILURE = (LocString) $"{UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} requires a {UI.PRE_KEYWORD}Power{UI.PST_KEYWORD} source for launch";
        }
      }
    }

    public class FULLTANK
    {
      public static LocString NAME = (LocString) "Fuel Tank full";
      public static LocString TOOLTIP = (LocString) "Tank is full, ready for launch";
    }

    public class EMPTYTANK
    {
      public static LocString NAME = (LocString) "Fuel Tank not full";
      public static LocString TOOLTIP = (LocString) "Fuel tank must be filled before launch";
    }

    public class FULLOXIDIZERTANK
    {
      public static LocString NAME = (LocString) "Oxidizer Tank full";
      public static LocString TOOLTIP = (LocString) "Tank is full, ready for launch";
    }

    public class EMPTYOXIDIZERTANK
    {
      public static LocString NAME = (LocString) "Oxidizer Tank not full";
      public static LocString TOOLTIP = (LocString) "Oxidizer tank must be filled before launch";
    }

    public class ROCKETSTATUS
    {
      public static LocString STATUS_TITLE = (LocString) "Rocket Status";
      public static LocString NONE = (LocString) nameof (NONE);
      public static LocString SELECTED = (LocString) nameof (SELECTED);
      public static LocString LOCKEDIN = (LocString) "LOCKED IN";
      public static LocString NODESTINATION = (LocString) "No destination selected";
      public static LocString DESTINATIONVALUE = (LocString) "None";
      public static LocString NOPASSENGERS = (LocString) "No passengers";
      public static LocString STATUS = (LocString) "Status";
      public static LocString TOTAL = (LocString) "Total";
      public static LocString WEIGHTPENALTY = (LocString) "Weight Penalty";
      public static LocString TIMEREMAINING = (LocString) "Time Remaining";
      public static LocString BOOSTED_TIME_MODIFIER = (LocString) "Less Than ";
    }

    public class ROCKETSTATS
    {
      public static LocString TOTAL_OXIDIZABLE_FUEL = (LocString) "Total oxidizable fuel";
      public static LocString TOTAL_OXIDIZER = (LocString) "Total oxidizer";
      public static LocString TOTAL_FUEL = (LocString) "Total fuel";
      public static LocString NO_ENGINE = (LocString) "NO ENGINE";
      public static LocString ENGINE_EFFICIENCY = (LocString) "Main engine efficiency";
      public static LocString OXIDIZER_EFFICIENCY = (LocString) "Average oxidizer efficiency";
      public static LocString SOLID_BOOSTER = (LocString) "Solid boosters";
      public static LocString ROBO_PILOT_RANGE = (LocString) "Robo-Pilot Range";
      public static LocString ROBO_PILOT_EFFICIENCY = (LocString) $"Robo-Pilot can travel {{0}} per {UI.PRE_KEYWORD}Data Bank{UI.PST_KEYWORD}";
      public static LocString TOTAL_THRUST = (LocString) "Total thrust";
      public static LocString TOTAL_RANGE = (LocString) "Total range";
      public static LocString DRY_MASS = (LocString) "Dry mass";
      public static LocString WET_MASS = (LocString) "Wet mass";
    }

    public class STORAGESTATS
    {
      public static LocString STORAGECAPACITY = (LocString) "{0} / {1}";
    }
  }

  public class RESEARCHSCREEN
  {
    public static LocString SEARCH_RESULTS_CATEGORY = (LocString) "Search Results";

    public class FILTER_BUTTONS
    {
      public static LocString HEADER = (LocString) "Preset Filters";
      public static LocString ALL = (LocString) "All";
      public static LocString AVAILABLE = (LocString) "Next";
      public static LocString COMPLETED = (LocString) "Completed";
      public static LocString OXYGEN = (LocString) "Oxygen";
      public static LocString FOOD = (LocString) "Food";
      public static LocString WATER = (LocString) "Water";
      public static LocString POWER = (LocString) "Power";
      public static LocString MORALE = (LocString) "Morale";
      public static LocString RANCHING = (LocString) "Ranching";
      public static LocString FILTER = (LocString) "Filter";
      public static LocString TILE = (LocString) "Tile";
      public static LocString TRANSPORT = (LocString) "Transport";
      public static LocString AUTOMATION = (LocString) "Automation";
      public static LocString MEDICINE = (LocString) "Medicine";
      public static LocString ROCKET = (LocString) "Rocket";
      public static LocString RADIATION = (LocString) "Radiation";
    }
  }

  public class CODEX
  {
    public static LocString SEARCH_HEADER = (LocString) "Search Database";
    public static LocString BACK_BUTTON = (LocString) "Back ({0})";
    public static LocString TIPS = (LocString) "Tips";
    public static LocString GAME_SYSTEMS = (LocString) "Systems";
    public static LocString DETAILS = (LocString) "Details";
    public static LocString RECIPE_ITEM = (LocString) "{0} x {1}{2}";
    public static LocString RECIPE_FABRICATOR = (LocString) "{1} ({0} seconds)";
    public static LocString RECIPE_FABRICATOR_HEADER = (LocString) "Produced by";
    public static LocString BACK_BUTTON_TOOLTIP = (LocString) (UI.CLICK(UI.ClickType.Click) + " to go back:\n{0}");
    public static LocString BACK_BUTTON_NO_HISTORY_TOOLTIP = (LocString) (UI.CLICK(UI.ClickType.Click) + " to go back:\nN/A");
    public static LocString FORWARD_BUTTON_TOOLTIP = (LocString) (UI.CLICK(UI.ClickType.Click) + " to go forward:\n{0}");
    public static LocString FORWARD_BUTTON_NO_HISTORY_TOOLTIP = (LocString) (UI.CLICK(UI.ClickType.Click) + " to go forward:\nN/A");
    public static LocString TITLE = (LocString) "DATABASE";
    public static LocString MANAGEMENT_BUTTON = (LocString) "DATABASE";

    public class CODEX_DISCOVERED_MESSAGE
    {
      public static LocString TITLE = (LocString) "New Log Entry";
      public static LocString BODY = (LocString) "I've added a new entry to my log: {codex}\n";
    }

    public class SUBWORLDS
    {
      public static LocString ELEMENTS = (LocString) "Elements";
      public static LocString PLANTS = (LocString) "Plants";
      public static LocString CRITTERS = (LocString) "Critters";
      public static LocString NONE = (LocString) "None";
    }

    public class GEYSERS
    {
      public static LocString DESC = (LocString) "Geysers and Fumaroles emit elements at variable intervals. They provide a sustainable source of material or kinetic energy, albeit typically in low volumes.\n\nThe variable factors of a geyser are:\n\n    • Emission element \n    • Emission temperature \n    • Emission mass \n    • Cycle length \n    • Dormancy duration \n    • Disease emitted";
    }

    public class EQUIPMENT
    {
      public static LocString DESC = (LocString) "Equipment description";
    }

    public class FOOD
    {
      public static LocString QUALITY = (LocString) "Quality: {0}";
      public static LocString CALORIES = (LocString) "Calories: {0}";
      public static LocString SPOILPROPERTIES = (LocString) "Refrigeration temperature: {0}\nDeep Freeze temperature: {1}\nSpoil time: {2}";
      public static LocString NON_PERISHABLE = (LocString) "Spoil time: Never";
    }

    public class CATEGORYNAMES
    {
      public static LocString ROOT = (LocString) UI.FormatAsLink("Index", "HOME");
      public static LocString PLANTS = (LocString) UI.FormatAsLink("Plants", nameof (PLANTS));
      public static LocString CREATURES = (LocString) UI.FormatAsLink("Critters", nameof (CREATURES));
      public static LocString DUPLICANTS = (LocString) UI.FormatAsLink("Duplicants", nameof (DUPLICANTS));
      public static LocString EMAILS = (LocString) UI.FormatAsLink("E-mail", nameof (EMAILS));
      public static LocString JOURNALS = (LocString) UI.FormatAsLink("Journals", nameof (JOURNALS));
      public static LocString MYLOG = (LocString) UI.FormatAsLink("My Log", nameof (MYLOG));
      public static LocString INVESTIGATIONS = (LocString) UI.FormatAsLink("Investigations", "Investigations");
      public static LocString RESEARCHNOTES = (LocString) UI.FormatAsLink("Research Notes", nameof (RESEARCHNOTES));
      public static LocString NOTICES = (LocString) UI.FormatAsLink("Notices", nameof (NOTICES));
      public static LocString FOOD = (LocString) UI.FormatAsLink("Food", nameof (FOOD));
      public static LocString MINION_MODIFIERS = (LocString) UI.FormatAsLink("Duplicant Effects (EDITOR ONLY)", nameof (MINION_MODIFIERS));
      public static LocString BUILDINGS = (LocString) UI.FormatAsLink("Buildings", nameof (BUILDINGS));
      public static LocString ROOMS = (LocString) UI.FormatAsLink("Rooms", nameof (ROOMS));
      public static LocString TECH = (LocString) UI.FormatAsLink("Research", nameof (TECH));
      public static LocString TIPS = (LocString) UI.FormatAsLink("Tutorials", "LESSONS");
      public static LocString TUTORIALS = (LocString) UI.FormatAsLink("Tutorials", "LESSONS");
      public static LocString VIDEOTUTORIALS = (LocString) UI.FormatAsLink("Videos", nameof (VIDEOTUTORIALS));
      public static LocString SYSTEMSTUTORIALS = (LocString) UI.FormatAsLink("Systems", nameof (SYSTEMSTUTORIALS));
      public static LocString EQUIPMENT = (LocString) UI.FormatAsLink("Equipment", nameof (EQUIPMENT));
      public static LocString BIOMES = (LocString) UI.FormatAsLink("Biomes", nameof (BIOMES));
      public static LocString STORYTRAITS = (LocString) UI.FormatAsLink("Story Traits", nameof (STORYTRAITS));
      public static LocString MISCELLANEOUSTIPS = (LocString) UI.FormatAsLink("Tips", nameof (MISCELLANEOUSTIPS));
      public static LocString MISCELLANEOUSITEMS = (LocString) UI.FormatAsLink("Items", nameof (MISCELLANEOUSITEMS));
      public static LocString ORNAMENTS = (LocString) UI.FormatAsLink("Ornaments", nameof (ORNAMENTS));
      public static LocString ELEMENTS = (LocString) UI.FormatAsLink("Elements", nameof (ELEMENTS));
      public static LocString ELEMENTSSOLID = (LocString) UI.FormatAsLink("Solids", "ELEMENTS_SOLID");
      public static LocString ELEMENTSGAS = (LocString) UI.FormatAsLink("Gases", "ELEMENTS_GAS");
      public static LocString ELEMENTSLIQUID = (LocString) UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID");
      public static LocString ELEMENTSOTHER = (LocString) UI.FormatAsLink("Other", "ELEMENTS_OTHER");
      public static LocString ELEMENTTYPES = (LocString) UI.FormatAsLink("Element Properties", "ELEMENT_TYPES");
      public static LocString BUILDINGMATERIALCLASSES = (LocString) UI.FormatAsLink("Building Materials", "BUILDING_MATERIAL_CLASSES");
      public static LocString INDUSTRIALINGREDIENTS = (LocString) UI.FormatAsLink("Industrial Ingredients", nameof (INDUSTRIALINGREDIENTS));
      public static LocString TECHCOMPONENTS = (LocString) UI.FormatAsLink("Tech Components", nameof (TECHCOMPONENTS));
      public static LocString DUPLICANTSCATEGORY = (LocString) UI.FormatAsLink("Duplicants", nameof (DUPLICANTS));
      public static LocString MEDICINES = (LocString) UI.FormatAsLink("Medicines", nameof (MEDICINES));
      public static LocString GEYSERS = (LocString) UI.FormatAsLink("Geysers", nameof (GEYSERS));
      public static LocString SYSTEMS = (LocString) UI.FormatAsLink("Systems", nameof (SYSTEMS));
      public static LocString ROLES = (LocString) UI.FormatAsLink("Duplicant Skills", nameof (ROLES));
      public static LocString DISEASE = (LocString) UI.FormatAsLink("Disease", nameof (DISEASE));
      public static LocString SICKNESS = (LocString) UI.FormatAsLink("Sickness", nameof (SICKNESS));
      public static LocString MEDIA = (LocString) UI.FormatAsLink("Media", nameof (MEDIA));
    }
  }

  public class DEVELOPMENTBUILDS
  {
    public static LocString WATERMARK = (LocString) "BUILD: {0}";
    public static LocString TESTING_WATERMARK = (LocString) "TESTING BUILD: {0}";
    public static LocString TESTING_TOOLTIP = (LocString) $"This game is currently running a Test version.\n\n{UI.CLICK(UI.ClickType.Click)} for more info.";
    public static LocString TESTING_MESSAGE_TITLE = (LocString) "TESTING BUILD";
    public static LocString TESTING_MESSAGE = (LocString) "This game is running a Test version of Oxygen Not Included. This means that some features may be in development or buggier than normal, and require more testing before they can be moved into the Release build.\n\nIf you encounter any bugs or strange behavior, please add a report to the bug forums. We appreciate it!";
    public static LocString TESTING_MORE_INFO = (LocString) "BUG FORUMS";
    public static LocString FULL_PATCH_NOTES = (LocString) "Full Patch Notes";
    public static LocString PREVIOUS_VERSION = (LocString) "Previous Version";

    public class ALPHA
    {
      public class MESSAGES
      {
        public static LocString FORUMBUTTON = (LocString) "FORUMS";
        public static LocString MAILINGLIST = (LocString) "MAILING LIST";
        public static LocString PATCHNOTES = (LocString) "PATCH NOTES";
        public static LocString FEEDBACK = (LocString) nameof (FEEDBACK);
      }

      public class LOADING
      {
        public static LocString TITLE = (LocString) "<b>Welcome to Oxygen Not Included!</b>";
        public static LocString BODY = (LocString) "This game is in the early stages of development which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\nDuring this time Oxygen Not Included will be receiving regular updates to fix bugs, add features, and introduce additional content, so if you encounter issues or just have suggestions to share, please let us know on our forums: <u>http://forums.kleientertainment.com</u>\n\nA special thanks to those who joined us during our time in Alpha. We value your feedback and thank you for joining us in the development process. We couldn't do this without you.\n\nEnjoy your time in deep space!\n\n- Klei";
        public static LocString BODY_NOLINKS = (LocString) "This DLC is currently in active development, which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\n During this time Spaced Out! will be receiving regular updates to fix bugs, add features, and introduce additional content.\n\n We've got lots of content old and new to add to this DLC before it's ready, and we're happy to have you along with us. Enjoy your time in deep space!\n\n - The Team at Klei";
        public static LocString FORUMBUTTON = (LocString) "Visit Forums";
      }

      public class HEALTHY_MESSAGE
      {
        public static LocString CONTINUEBUTTON = (LocString) "Thanks!";
      }
    }

    public class PREVIOUS_UPDATE
    {
      public static LocString TITLE = (LocString) "<b>Welcome to Oxygen Not Included</b>";
      public static LocString BODY = (LocString) "Whoops!\n\nYou're about to opt in to the <b>Previous Update branch</b>. That means opting out of all new features, fixes and content from the live branch.\n\nThis branch is temporary. It will be replaced when the next update is released. It's also completely unsupported: please don't report bugs or issues you find here.\n\nAre you sure you want to opt in?";
      public static LocString CONTINUEBUTTON = (LocString) "Play Old Version";
      public static LocString FORUMBUTTON = (LocString) "More Information";
      public static LocString QUITBUTTON = (LocString) "Quit";
    }

    public class DLC_BETA
    {
      public static LocString TITLE = (LocString) "<b>Welcome to Oxygen Not Included</b>";
      public static LocString BODY = (LocString) "You're about to opt in to the beta for <b>The Aquatic Planet Pack</b> DLC.\nThis free beta is a work in progress, and will be discontinued before the paid DLC is released. \n\nAre you sure you want to opt in?";
      public static LocString CONTINUEBUTTON = (LocString) "Play Beta";
      public static LocString FORUMBUTTON = (LocString) "More Information";
      public static LocString QUITBUTTON = (LocString) "Quit";
    }

    public class UPDATES
    {
      public static LocString UPDATES_HEADER = (LocString) "NEXT UPGRADE LIVE IN";
      public static LocString NOW = (LocString) "Less than a day";
      public static LocString TWENTY_FOUR_HOURS = (LocString) "Less than a day";
      public static LocString FINAL_WEEK = (LocString) "{0} days";
      public static LocString BIGGER_TIMES = (LocString) "{1} weeks {0} days";
    }
  }

  public class UNITSUFFIXES
  {
    public static LocString SECOND = (LocString) " s";
    public static LocString PERSECOND = (LocString) "/s";
    public static LocString PERCYCLE = (LocString) "/cycle";
    public static LocString UNIT = (LocString) " unit";
    public static LocString UNITS = (LocString) " units";
    public static LocString PERCENT = (LocString) "%";
    public static LocString DEGREES = (LocString) " degrees";
    public static LocString CRITTERS = (LocString) " critters";
    public static LocString GROWTH = (LocString) "growth";
    public static LocString SECONDS = (LocString) "Seconds";
    public static LocString DUPLICANTS = (LocString) "Duplicants";
    public static LocString GERMS = (LocString) "Germs";
    public static LocString ROCKET_MISSIONS = (LocString) "Missions";
    public static LocString TILES = (LocString) "Tiles";
    public static LocString TILES_STARMAP = (LocString) "Hexes";

    public class MASS
    {
      public static LocString TONNE = (LocString) " t";
      public static LocString KILOGRAM = (LocString) " kg";
      public static LocString GRAM = (LocString) " g";
      public static LocString MILLIGRAM = (LocString) " mg";
      public static LocString MICROGRAM = (LocString) " mcg";
      public static LocString POUND = (LocString) " lb";
      public static LocString DRACHMA = (LocString) " dr";
      public static LocString GRAIN = (LocString) " gr";
    }

    public class TEMPERATURE
    {
      public static LocString CELSIUS = (LocString) $" {'º'.ToString()}C";
      public static LocString FAHRENHEIT = (LocString) $" {'º'.ToString()}F";
      public static LocString KELVIN = (LocString) " K";
    }

    public class CALORIES
    {
      public static LocString CALORIE = (LocString) " cal";
      public static LocString KILOCALORIE = (LocString) " kcal";
    }

    public class ELECTRICAL
    {
      public static LocString JOULE = (LocString) " J";
      public static LocString KILOJOULE = (LocString) " kJ";
      public static LocString MEGAJOULE = (LocString) " MJ";
      public static LocString WATT = (LocString) " W";
      public static LocString KILOWATT = (LocString) " kW";
    }

    public class HEAT
    {
      public static LocString DTU = (LocString) " DTU";
      public static LocString KDTU = (LocString) " kDTU";
      public static LocString DTU_S = (LocString) " DTU/s";
      public static LocString KDTU_S = (LocString) " kDTU/s";
    }

    public class DISTANCE
    {
      public static LocString METER = (LocString) " m";
      public static LocString KILOMETER = (LocString) " km";
    }

    public class DISEASE
    {
      public static LocString UNITS = (LocString) " germs";
    }

    public class NOISE
    {
      public static LocString UNITS = (LocString) " dB";
    }

    public class INFORMATION
    {
      public static LocString BYTE = (LocString) "B";
      public static LocString KILOBYTE = (LocString) "kB";
      public static LocString MEGABYTE = (LocString) "MB";
      public static LocString GIGABYTE = (LocString) "GB";
      public static LocString TERABYTE = (LocString) "TB";
    }

    public class LIGHT
    {
      public static LocString LUX = (LocString) " lux";
    }

    public class RADIATION
    {
      public static LocString RADS = (LocString) " rads";
    }

    public class HIGHENERGYPARTICLES
    {
      public static LocString PARTRICLE = (LocString) " Radbolt";
      public static LocString PARTRICLES = (LocString) " Radbolts";
    }
  }

  public class OVERLAYS
  {
    public class TILEMODE
    {
      public static LocString NAME = (LocString) "MATERIALS OVERLAY";
      public static LocString BUTTON = (LocString) "Materials Overlay";
    }

    public class OXYGEN
    {
      public static LocString NAME = (LocString) "OXYGEN OVERLAY";
      public static LocString BUTTON = (LocString) "Oxygen Overlay";
      public static LocString LEGEND1 = (LocString) "Very Breathable";
      public static LocString LEGEND2 = (LocString) "Breathable";
      public static LocString LEGEND3 = (LocString) "Barely Breathable";
      public static LocString LEGEND4 = (LocString) "Unbreathable";
      public static LocString LEGEND5 = (LocString) "Barely Breathable";
      public static LocString LEGEND6 = (LocString) "Unbreathable";

      public class TOOLTIPS
      {
        public static LocString LEGEND1 = (LocString) $"<b>Very Breathable</b>\nHigh {UI.PRE_KEYWORD}Oxygen{UI.PST_KEYWORD} concentrations";
        public static LocString LEGEND2 = (LocString) $"<b>Breathable</b>\nSufficient {UI.PRE_KEYWORD}Oxygen{UI.PST_KEYWORD} concentrations";
        public static LocString LEGEND3 = (LocString) $"<b>Barely Breathable</b>\nLow {UI.PRE_KEYWORD}Oxygen{UI.PST_KEYWORD} concentrations";
        public static LocString LEGEND4 = (LocString) $"<b>Unbreathable</b>\nExtremely low or absent {UI.PRE_KEYWORD}Oxygen{UI.PST_KEYWORD} concentrations\n\nDuplicants will suffocate if trapped in these areas";
        public static LocString LEGEND5 = (LocString) "<b>Slightly Toxic</b>\nHarmful gas concentration";
        public static LocString LEGEND6 = (LocString) "<b>Very Toxic</b>\nLethal gas concentration";
      }
    }

    public class ELECTRICAL
    {
      public static LocString NAME = (LocString) "POWER OVERLAY";
      public static LocString BUTTON = (LocString) "Power Overlay";
      public static LocString LEGEND1 = (LocString) "<b>BUILDING POWER</b>";
      public static LocString LEGEND2 = (LocString) "Consumer";
      public static LocString LEGEND3 = (LocString) "Producer";
      public static LocString LEGEND4 = (LocString) "<b>CIRCUIT POWER HEALTH</b>";
      public static LocString LEGEND5 = (LocString) "Inactive";
      public static LocString LEGEND6 = (LocString) "Safe";
      public static LocString LEGEND7 = (LocString) "Strained";
      public static LocString LEGEND8 = (LocString) "Overloaded";
      public static LocString DIAGRAM_HEADER = (LocString) "Energy from the <b>Left Outlet</b> is used by the <b>Right Outlet</b>";
      public static LocString LEGEND_SWITCH = (LocString) "Switch";

      public class TOOLTIPS
      {
        public static LocString LEGEND1 = (LocString) ("Displays whether buildings use or generate " + UI.FormatAsLink("Power", "POWER"));
        public static LocString LEGEND2 = (LocString) "<b>Consumer</b>\nThese buildings draw power from a circuit";
        public static LocString LEGEND3 = (LocString) "<b>Producer</b>\nThese buildings generate power for a circuit";
        public static LocString LEGEND4 = (LocString) "Displays the health of wire systems";
        public static LocString LEGEND5 = (LocString) "<b>Inactive</b>\nThere is no power activity on these circuits";
        public static LocString LEGEND6 = (LocString) "<b>Safe</b>\nThese circuits are not in danger of overloading";
        public static LocString LEGEND7 = (LocString) "<b>Strained</b>\nThese circuits are close to consuming more power than their wires support";
        public static LocString LEGEND8 = (LocString) "<b>Overloaded</b>\nThese circuits are consuming more power than their wires support";
        public static LocString LEGEND_SWITCH = (LocString) "<b>Switch</b>\nActivates or deactivates connected circuits";
      }
    }

    public class TEMPERATURE
    {
      public static LocString NAME = (LocString) "TEMPERATURE OVERLAY";
      public static LocString BUTTON = (LocString) "Temperature Overlay";
      public static LocString EXTREMECOLD = (LocString) "Absolute Zero";
      public static LocString VERYCOLD = (LocString) "Cold";
      public static LocString COLD = (LocString) "Chilled";
      public static LocString TEMPERATE = (LocString) "Temperate";
      public static LocString HOT = (LocString) "Warm";
      public static LocString VERYHOT = (LocString) "Hot";
      public static LocString EXTREMEHOT = (LocString) "Scorching";
      public static LocString MAXHOT = (LocString) "Molten";
      public static LocString HEATSOURCES = (LocString) "Heat Source";
      public static LocString HEATSINK = (LocString) "Heat Sink";
      public static LocString DEFAULT_TEMPERATURE_BUTTON = (LocString) "Default";

      public class TOOLTIPS
      {
        public static LocString TEMPERATURE = (LocString) "Temperatures reaching {0}";
        public static LocString HEATSOURCES = (LocString) "Elements displaying this symbol can produce heat";
        public static LocString HEATSINK = (LocString) "Elements displaying this symbol can absorb heat";
      }
    }

    public class STATECHANGE
    {
      public static LocString LOWPOINT = (LocString) "Low energy state change";
      public static LocString STABLE = (LocString) "Stable";
      public static LocString HIGHPOINT = (LocString) "High energy state change";

      public class TOOLTIPS
      {
        public static LocString LOWPOINT = (LocString) "Nearing a low energy state change";
        public static LocString STABLE = (LocString) "Not near any state changes";
        public static LocString HIGHPOINT = (LocString) "Nearing high energy state change";
      }
    }

    public class HEATFLOW
    {
      public static LocString NAME = (LocString) "THERMAL TOLERANCE OVERLAY";
      public static LocString HOVERTITLE = (LocString) "THERMAL TOLERANCE";
      public static LocString BUTTON = (LocString) "Thermal Tolerance Overlay";
      public static LocString COOLING = (LocString) "Body Heat Loss";
      public static LocString NEUTRAL = (LocString) "Comfort Zone";
      public static LocString HEATING = (LocString) "Body Heat Retention";
      public static LocString COOLING_DUPE = (LocString) "Body Heat Loss {0}\n\nUncomfortably chilly surroundings";
      public static LocString NEUTRAL_DUPE = (LocString) "Comfort Zone {0}";
      public static LocString HEATING_DUPE = (LocString) "Body Heat Loss {0}\n\nUncomfortably toasty surroundings";

      public class TOOLTIPS
      {
        public static LocString COOLING = (LocString) "<b>Body Heat Loss</b>\nUncomfortably cold\n\nDuplicants lose more heat in chilly surroundings than they can absorb\n    • Warm Coats help Duplicants retain body heat";
        public static LocString NEUTRAL = (LocString) "<b>Comfort Zone</b>\nComfortable area\n\nDuplicants can regulate their internal temperatures in these areas";
        public static LocString HEATING = (LocString) "<b>Body Heat Retention</b>\nUncomfortably warm\n\nDuplicants absorb more heat in toasty surroundings than they can release";
      }
    }

    public class RELATIVETEMPERATURE
    {
      public static LocString NAME = (LocString) "RELATIVE TEMPERATURE";
      public static LocString HOVERTITLE = (LocString) "RELATIVE TEMPERATURE";
      public static LocString BUTTON = (LocString) "Relative Temperature Overlay";
    }

    public class ROOMS
    {
      public static LocString NAME = (LocString) "ROOM OVERLAY";
      public static LocString BUTTON = (LocString) "Room Overlay";
      public static LocString ROOM = (LocString) "Room {0}";
      public static LocString HOVERTITLE = (LocString) nameof (ROOMS);

      public static class NOROOM
      {
        public static LocString HEADER = (LocString) "No Room";
        public static LocString DESC = (LocString) "Enclose this space with walls and doors to make a room";
        public static LocString TOO_BIG = (LocString) "<color=#F44A47FF>    • Size: {0} Tiles\n    • Maximum room size: {1} Tiles</color>";
      }

      public static class POND
      {
        public static LocString HEADER = (LocString) "<b>Pool Size</b>";
        public static LocString SIZE = (LocString) "    • Pool Size: {0} Tiles";
        public static LocString CRITTER_COUNT = (LocString) "    • Aquatic Critters: {0}";
        public static LocString NOFISH = (LocString) "    • No aquatic critters found";
      }

      public class TOOLTIPS
      {
        public static LocString ROOM = (LocString) "Completed Duplicant bedrooms";
        public static LocString NOROOMS = (LocString) "Duplicants have nowhere to sleep";
      }
    }

    public class JOULES
    {
      public static LocString NAME = (LocString) nameof (JOULES);
      public static LocString HOVERTITLE = (LocString) nameof (JOULES);
      public static LocString BUTTON = (LocString) "Joules Overlay";
    }

    public class LIGHTING
    {
      public static LocString NAME = (LocString) "LIGHT OVERLAY";
      public static LocString BUTTON = (LocString) "Light Overlay";
      public static LocString LITAREA = (LocString) "Lit Area";
      public static LocString DARK = (LocString) "Unlit Area";
      public static LocString HOVERTITLE = (LocString) "LIGHT";
      public static LocString DESC = (LocString) "{0} Lux";

      public class RANGES
      {
        public static LocString NO_LIGHT = (LocString) "Pitch Black";
        public static LocString VERY_LOW_LIGHT = (LocString) "Very Dim";
        public static LocString LOW_LIGHT = (LocString) "Dim";
        public static LocString MEDIUM_LIGHT = (LocString) "Well Lit";
        public static LocString HIGH_LIGHT = (LocString) "Bright";
        public static LocString VERY_HIGH_LIGHT = (LocString) "Brilliant";
        public static LocString MAX_LIGHT = (LocString) "Blinding";
      }

      public class TOOLTIPS
      {
        public static LocString NAME = (LocString) "LIGHT OVERLAY";
        public static LocString LITAREA = (LocString) $"<b>Lit Area</b>\nWorking in well-lit areas improves Duplicant {UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD}";
        public static LocString DARK = (LocString) "<b>Unlit Area</b>\nWorking in the dark has no effect on Duplicants";
      }
    }

    public class CROP
    {
      public static LocString NAME = (LocString) "FARMING OVERLAY";
      public static LocString BUTTON = (LocString) "Farming Overlay";
      public static LocString GROWTH_HALTED = (LocString) "Halted Growth";
      public static LocString GROWING = (LocString) "Growing";
      public static LocString FULLY_GROWN = (LocString) "Fully Grown";

      public class TOOLTIPS
      {
        public static LocString GROWTH_HALTED = (LocString) "<b>Halted Growth</b>\nSubstandard conditions prevent these plants from growing";
        public static LocString GROWING = (LocString) "<b>Growing</b>\nThese plants are thriving in their current conditions";
        public static LocString FULLY_GROWN = (LocString) $"<b>Fully Grown</b>\nThese plants have reached maturation\n\nSelect the {UI.FormatAsTool("Harvest Tool", Action.Harvest)} to batch harvest";
      }
    }

    public class LIQUIDPLUMBING
    {
      public static LocString NAME = (LocString) "PLUMBING OVERLAY";
      public static LocString BUTTON = (LocString) "Plumbing Overlay";
      public static LocString CONSUMER = (LocString) "Output Pipe";
      public static LocString FILTERED = (LocString) "Filtered Output Pipe";
      public static LocString PRODUCER = (LocString) "Building Intake";
      public static LocString CONNECTED = (LocString) "Connected";
      public static LocString DISCONNECTED = (LocString) "Disconnected";
      public static LocString NETWORK = (LocString) "Liquid Network {0}";
      public static LocString DIAGRAM_BEFORE_ARROW = (LocString) "Liquid flows from <b>Output Pipe</b>";
      public static LocString DIAGRAM_AFTER_ARROW = (LocString) "<b>Building Intake</b>";

      public class TOOLTIPS
      {
        public static LocString CONNECTED = (LocString) ("Connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT"));
        public static LocString DISCONNECTED = (LocString) ("Not connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT"));
        public static LocString CONSUMER = (LocString) ("<b>Output Pipe</b>\nOutputs send liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING"));
        public static LocString FILTERED = (LocString) ("<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING"));
        public static LocString PRODUCER = (LocString) ("<b>Building Intake</b>\nIntakes send liquid into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "LIQUIDPIPING"));
        public static LocString NETWORK = (LocString) "Liquid network {0}";
      }
    }

    public class GASPLUMBING
    {
      public static LocString NAME = (LocString) "VENTILATION OVERLAY";
      public static LocString BUTTON = (LocString) "Ventilation Overlay";
      public static LocString CONSUMER = (LocString) "Output Pipe";
      public static LocString FILTERED = (LocString) "Filtered Output Pipe";
      public static LocString PRODUCER = (LocString) "Building Intake";
      public static LocString CONNECTED = (LocString) "Connected";
      public static LocString DISCONNECTED = (LocString) "Disconnected";
      public static LocString NETWORK = (LocString) "Gas Network {0}";
      public static LocString DIAGRAM_BEFORE_ARROW = (LocString) "Gas flows from <b>Output Pipe</b>";
      public static LocString DIAGRAM_AFTER_ARROW = (LocString) "<b>Building Intake</b>";

      public class TOOLTIPS
      {
        public static LocString CONNECTED = (LocString) ("Connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING"));
        public static LocString DISCONNECTED = (LocString) ("Not connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING"));
        public static LocString CONSUMER = (LocString) $"<b>Output Pipe</b>\nOutputs send {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD} into {UI.PRE_KEYWORD}Pipes{UI.PST_KEYWORD}\n\nMust be on the same network as at least one {UI.FormatAsLink("Intake", "GASPIPING")}";
        public static LocString FILTERED = (LocString) $"<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD} into {UI.PRE_KEYWORD}Pipes{UI.PST_KEYWORD}\n\nMust be on the same network as at least one {UI.FormatAsLink("Intake", "GASPIPING")}";
        public static LocString PRODUCER = (LocString) ("<b>Building Intake</b>\nIntakes send gas into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "GASPIPING"));
        public static LocString NETWORK = (LocString) "Gas network {0}";
      }
    }

    public class SUIT
    {
      public static LocString NAME = (LocString) "EXOSUIT OVERLAY";
      public static LocString BUTTON = (LocString) "Exosuit Overlay";
      public static LocString SUIT_ICON = (LocString) "Exosuit";
      public static LocString SUIT_ICON_TOOLTIP = (LocString) "<b>Exosuit</b>\nHighlights the current location of equippable exosuits";
    }

    public class LOGIC
    {
      public static LocString NAME = (LocString) "AUTOMATION OVERLAY";
      public static LocString BUTTON = (LocString) "Automation Overlay";
      public static LocString INPUT = (LocString) "Input Port";
      public static LocString OUTPUT = (LocString) "Output Port";
      public static LocString RIBBON_INPUT = (LocString) "Ribbon Input Port";
      public static LocString RIBBON_OUTPUT = (LocString) "Ribbon Output Port";
      public static LocString RESET_UPDATE = (LocString) "Reset Port";
      public static LocString CONTROL_INPUT = (LocString) "Control Port";
      public static LocString CIRCUIT_STATUS_HEADER = (LocString) "GRID STATUS";
      public static LocString ONE = (LocString) "Green";
      public static LocString ZERO = (LocString) "Red";
      public static LocString DISCONNECTED = (LocString) nameof (DISCONNECTED);

      public abstract class TOOLTIPS
      {
        public static LocString INPUT = (LocString) "<b>Input Port</b>\nReceives a signal from an automation grid";
        public static LocString OUTPUT = (LocString) "<b>Output Port</b>\nSends a signal out to an automation grid";
        public static LocString RIBBON_INPUT = (LocString) "<b>Ribbon Input Port</b>\nReceives a 4-bit signal from an automation grid";
        public static LocString RIBBON_OUTPUT = (LocString) "<b>Ribbon Output Port</b>\nSends a 4-bit signal out to an automation grid";
        public static LocString RESET_UPDATE = (LocString) $"<b>Reset Port</b>\nReset a {(string) BUILDINGS.PREFABS.LOGICMEMORY.NAME}'s internal Memory to {UI.FormatAsAutomationState("Red", UI.AutomationState.Standby)}";
        public static LocString CONTROL_INPUT = (LocString) $"<b>Control Port</b>\nControl the signal selection of a {(string) BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.NAME} or {(string) BUILDINGS.PREFABS.LOGICGATEDEMULTIPLEXER.NAME}";
        public static LocString ONE = (LocString) ("<b>Green</b>\nThis port is currently " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active));
        public static LocString ZERO = (LocString) ("<b>Red</b>\nThis port is currently " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby));
        public static LocString DISCONNECTED = (LocString) "<b>Disconnected</b>\nThis port is not connected to an automation grid";
      }
    }

    public class CONVEYOR
    {
      public static LocString NAME = (LocString) "CONVEYOR OVERLAY";
      public static LocString BUTTON = (LocString) "Conveyor Overlay";
      public static LocString OUTPUT = (LocString) "Loader";
      public static LocString INPUT = (LocString) "Receptacle";

      public abstract class TOOLTIPS
      {
        public static LocString OUTPUT = (LocString) $"<b>Loader</b>\nLoads material onto a {UI.PRE_KEYWORD}Conveyor Rail{UI.PST_KEYWORD} for transport to Receptacles";
        public static LocString INPUT = (LocString) $"<b>Receptacle</b>\nReceives material from a {UI.PRE_KEYWORD}Conveyor Rail{UI.PST_KEYWORD} and stores it for Duplicant use";
      }
    }

    public class DECOR
    {
      public static LocString NAME = (LocString) "DECOR OVERLAY";
      public static LocString BUTTON = (LocString) "Decor Overlay";
      public static LocString TOTAL = (LocString) "Total Decor: ";
      public static LocString ENTRY = (LocString) "{0} {1} {2}";
      public static LocString COUNT = (LocString) "({0})";
      public static LocString VALUE = (LocString) "{0}{1}";
      public static LocString VALUE_ZERO = (LocString) "{0}{1}";
      public static LocString HEADER_POSITIVE = (LocString) "Positive Value:";
      public static LocString HEADER_NEGATIVE = (LocString) "Negative Value:";
      public static LocString LOWDECOR = (LocString) "Negative Decor";
      public static LocString HIGHDECOR = (LocString) "Positive Decor";
      public static LocString CLUTTER = (LocString) "Debris";
      public static LocString LIGHTING = (LocString) "Lighting";
      public static LocString CLOTHING = (LocString) "{0}'s Outfit";
      public static LocString CLOTHING_TRAIT_DECORUP = (LocString) "{0}'s Outfit (Innately Stylish)";
      public static LocString CLOTHING_TRAIT_DECORDOWN = (LocString) "{0}'s Outfit (Shabby Dresser)";
      public static LocString HOVERTITLE = (LocString) nameof (DECOR);
      public static LocString MAXIMUM_DECOR = (LocString) "{0}{1} (Maximum Decor)";

      public class TOOLTIPS
      {
        public static LocString LOWDECOR = (LocString) $"<b>Negative Decor</b>\nArea with insufficient {UI.PRE_KEYWORD}Decor{UI.PST_KEYWORD} values\n* Resources on the floor are considered \"debris\" and will decrease decor";
        public static LocString HIGHDECOR = (LocString) $"<b>Positive Decor</b>\nArea with sufficient {UI.PRE_KEYWORD}Decor{UI.PST_KEYWORD} values\n* Lighting and aesthetically pleasing buildings increase decor";
      }
    }

    public class PRIORITIES
    {
      public static LocString NAME = (LocString) "PRIORITY OVERLAY";
      public static LocString BUTTON = (LocString) "Priority Overlay";
      public static LocString ONE = (LocString) "1 (Low Urgency)";
      public static LocString ONE_TOOLTIP = (LocString) "Priority 1";
      public static LocString TWO = (LocString) "2";
      public static LocString TWO_TOOLTIP = (LocString) "Priority 2";
      public static LocString THREE = (LocString) "3";
      public static LocString THREE_TOOLTIP = (LocString) "Priority 3";
      public static LocString FOUR = (LocString) "4";
      public static LocString FOUR_TOOLTIP = (LocString) "Priority 4";
      public static LocString FIVE = (LocString) "5";
      public static LocString FIVE_TOOLTIP = (LocString) "Priority 5";
      public static LocString SIX = (LocString) "6";
      public static LocString SIX_TOOLTIP = (LocString) "Priority 6";
      public static LocString SEVEN = (LocString) "7";
      public static LocString SEVEN_TOOLTIP = (LocString) "Priority 7";
      public static LocString EIGHT = (LocString) "8";
      public static LocString EIGHT_TOOLTIP = (LocString) "Priority 8";
      public static LocString NINE = (LocString) "9 (High Urgency)";
      public static LocString NINE_TOOLTIP = (LocString) "Priority 9";
    }

    public class DISEASE
    {
      public static LocString NAME = (LocString) "GERM OVERLAY";
      public static LocString BUTTON = (LocString) "Germ Overlay";
      public static LocString HOVERTITLE = (LocString) "Germ";
      public static LocString INFECTION_SOURCE = (LocString) "Germ Source";
      public static LocString INFECTION_SOURCE_TOOLTIP = (LocString) "<b>Germ Source</b>\nAreas where germs are produced\n•  Placing Wash Basins or Hand Sanitizers near these areas may prevent disease spread";
      public static LocString NO_DISEASE = (LocString) "Zero surface germs";
      public static LocString DISEASE_NAME_FORMAT = (LocString) "{0}<color=#{1}></color>";
      public static LocString DISEASE_NAME_FORMAT_NO_COLOR = (LocString) "{0}";
      public static LocString DISEASE_FORMAT = (LocString) "{1} [{0}]<color=#{2}></color>";
      public static LocString DISEASE_FORMAT_NO_COLOR = (LocString) "{1} [{0}]";
      public static LocString CONTAINER_FORMAT = (LocString) "\n    {0}: {1}";

      public class DISINFECT_THRESHOLD_DIAGRAM
      {
        public static LocString UNITS = (LocString) "Germs";
        public static LocString MIN_LABEL = (LocString) "0";
        public static LocString MAX_LABEL = (LocString) "1m";
        public static LocString THRESHOLD_PREFIX = (LocString) "Disinfect At:";
        public static LocString TOOLTIP = (LocString) "Automatically disinfect any building with more than {NumberOfGerms} germs.";
        public static LocString TOOLTIP_DISABLED = (LocString) "Automatic building disinfection disabled.";
      }
    }

    public class CROPS
    {
      public static LocString NAME = (LocString) "FARMING OVERLAY";
      public static LocString BUTTON = (LocString) "Farming Overlay";
    }

    public class POWER
    {
      public static LocString WATTS_GENERATED = (LocString) "Watts Generated";
      public static LocString WATTS_CONSUMED = (LocString) "Watts Consumed";
    }

    public class RADIATION
    {
      public static LocString NAME = (LocString) nameof (RADIATION);
      public static LocString BUTTON = (LocString) "Radiation Overlay";
      public static LocString DESC = (LocString) "{rads} per cycle ({description})";
      public static LocString SHIELDING_DESC = (LocString) "Radiation Blocking: {radiationAbsorptionFactor}";
      public static LocString HOVERTITLE = (LocString) nameof (RADIATION);

      public class RANGES
      {
        public static LocString NONE = (LocString) "Completely Safe";
        public static LocString VERY_LOW = (LocString) "Mostly Safe";
        public static LocString LOW = (LocString) "Barely Safe";
        public static LocString MEDIUM = (LocString) "Slight Hazard";
        public static LocString HIGH = (LocString) "Significant Hazard";
        public static LocString VERY_HIGH = (LocString) "Extreme Hazard";
        public static LocString MAX = (LocString) "Maximum Hazard";
        public static LocString INPUTPORT = (LocString) "Radbolt Input Port";
        public static LocString OUTPUTPORT = (LocString) "Radbolt Output Port";
      }

      public class TOOLTIPS
      {
        public static LocString NONE = (LocString) "Completely Safe";
        public static LocString VERY_LOW = (LocString) "Mostly Safe";
        public static LocString LOW = (LocString) "Barely Safe";
        public static LocString MEDIUM = (LocString) "Slight Hazard";
        public static LocString HIGH = (LocString) "Significant Hazard";
        public static LocString VERY_HIGH = (LocString) "Extreme Hazard";
        public static LocString MAX = (LocString) "Maximum Hazard";
        public static LocString INPUTPORT = (LocString) "Radbolt Input Port";
        public static LocString OUTPUTPORT = (LocString) "Radbolt Output Port";
      }
    }
  }

  public class TABLESCREENS
  {
    public static LocString DUPLICANT_PROPERNAME = (LocString) "<b>{0}</b>";
    public static LocString SELECT_DUPLICANT_BUTTON = (LocString) (UI.CLICK(UI.ClickType.Click) + " to select <b>{0}</b>");
    public static LocString GOTO_DUPLICANT_BUTTON = (LocString) $"Double-{UI.CLICK(UI.ClickType.click)} to go to <b>{{0}}</b>";
    public static LocString COLUMN_SORT_BY_NAME = (LocString) "Sort by <b>Name</b>";
    public static LocString COLUMN_SORT_BY_STRESS = (LocString) "Sort by <b>Stress</b>";
    public static LocString COLUMN_SORT_BY_HITPOINTS = (LocString) "Sort by <b>Health</b>";
    public static LocString COLUMN_SORT_BY_SICKNESSES = (LocString) "Sort by <b>Disease</b>";
    public static LocString COLUMN_SORT_BY_FULLNESS = (LocString) "Sort by <b>Fullness</b>";
    public static LocString COLUMN_SORT_BY_EATEN_TODAY = (LocString) "Sort by number of <b>Calories</b> consumed today";
    public static LocString COLUMN_SORT_BY_EXPECTATIONS = (LocString) "Sort by <b>Morale</b>";
    public static LocString COLUMN_SORT_BY_POWERBANKS = (LocString) "Sort by <b>Power Banks</b>";
    public static LocString NA = (LocString) "N/A";
    public static LocString INFORMATION_NOT_AVAILABLE_TOOLTIP = (LocString) "Information is not available because {1} is in {0}";
    public static LocString NOBODY_HERE = (LocString) "Nobody here...";
  }

  public class CONSUMABLESSCREEN
  {
    public static LocString TITLE = (LocString) "CONSUMABLES";
    public static LocString TOOLTIP_TOGGLE_ALL = (LocString) "Toggle <b>all</b> food permissions <b>colonywide</b>";
    public static LocString TOOLTIP_TOGGLE_COLUMN = (LocString) "Toggle colonywide <b>{0}</b> permission";
    public static LocString TOOLTIP_TOGGLE_ROW = (LocString) "Toggle <b>all consumable permissions</b> for <b>{0}</b>";
    public static LocString NEW_MINIONS_TOOLTIP_TOGGLE_ROW = (LocString) "Toggle <b>all consumable permissions</b> for <b>New Duplicants</b>";
    public static LocString NEW_MINIONS_FOOD_PERMISSION_ON = (LocString) $"<b>New Duplicants</b> are <b>allowed</b> to eat \n{UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD}</b> by default";
    public static LocString NEW_MINIONS_FOOD_PERMISSION_OFF = (LocString) $"<b>New Duplicants</b> are <b>not allowed</b> to eat \n{UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} by default";
    public static LocString FOOD_PERMISSION_ON = (LocString) $"<b>{{0}}</b> is <b>allowed</b> to eat {UI.PRE_KEYWORD}{{1}}{UI.PST_KEYWORD}";
    public static LocString FOOD_PERMISSION_OFF = (LocString) $"<b>{{0}}</b> is <b>not allowed</b> to eat {UI.PRE_KEYWORD}{{1}}{UI.PST_KEYWORD}";
    public static LocString FOOD_CANT_CONSUME = (LocString) $"<b>{{0}}</b> <b>physically cannot</b> eat\n{UI.PRE_KEYWORD}{{1}}{UI.PST_KEYWORD}";
    public static LocString FOOD_REFUSE = (LocString) $"<b>{{0}}</b> <b>refuses</b> to eat\n{UI.PRE_KEYWORD}{{1}}{UI.PST_KEYWORD}";
    public static LocString FOOD_AVAILABLE = (LocString) "Available: {0}";
    public static LocString FOOD_MORALE = (LocString) $"{UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD}: {{0}}";
    public static LocString FOOD_QUALITY = (LocString) $"{UI.PRE_KEYWORD}Quality{UI.PST_KEYWORD}: {{0}}";
    public static LocString FOOD_QUALITY_VS_EXPECTATION = (LocString) $"\nThis food will give {UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD} <b>{{0}}</b> if {{1}} eats it";
    public static LocString CANNOT_ADJUST_PERMISSIONS = (LocString) "Cannot adjust consumable permissions because they're in {0}";
  }

  public class JOBSSCREEN
  {
    public static LocString TITLE = (LocString) "MANAGE DUPLICANT PRIORITIES";
    public static LocString TOOLTIP_TOGGLE_ALL = (LocString) "Set priority of all Errand Types colonywide";
    public static LocString HEADER_TOOLTIP = (LocString) $"<size=16>{{Job}} Errand Type</size>\n\n{{Details}}\n\nDuplicants will first choose what {UI.PRE_KEYWORD}Errand Type{UI.PST_KEYWORD} to perform based on {UI.PRE_KEYWORD}Duplicant Priorities{UI.PST_KEYWORD},\nthen they will choose individual tasks within that type using {UI.PRE_KEYWORD}Building Priorities{UI.PST_KEYWORD} set by the {UI.FormatAsLink("Priority Tool", "PRIORITIES")} {UI.FormatAsHotKey(Action.ManagePriorities)}";
    public static LocString HEADER_DETAILS_TOOLTIP = (LocString) "{Description}\n\nAffected errands: {ChoreList}";
    public static LocString HEADER_CHANGE_TOOLTIP = (LocString) $"Set the priority for the {UI.PRE_KEYWORD}{{Job}}{UI.PST_KEYWORD} Errand Type colonywide\n";
    public static LocString NEW_MINION_ITEM_TOOLTIP = (LocString) $"The {UI.PRE_KEYWORD}{{Job}}{UI.PST_KEYWORD} Errand Type is automatically a {{Priority}} {UI.PRE_KEYWORD}Priority{UI.PST_KEYWORD} for <b>Arriving Duplicants</b>";
    public static LocString ITEM_TOOLTIP = (LocString) $"{UI.PRE_KEYWORD}{{Job}}{UI.PST_KEYWORD} Priority for {{Name}}:\n<b>{{Priority}} Priority ({{PriorityValue}})</b>";
    public static LocString MINION_SKILL_TOOLTIP = (LocString) $"{{Name}}'s {UI.PRE_KEYWORD}{{Attribute}}{UI.PST_KEYWORD} Skill: ";
    public static LocString TRAIT_DISABLED = (LocString) $"{{Name}} possesses the {UI.PRE_KEYWORD}{{Trait}}{UI.PST_KEYWORD} trait and <b>cannot</b> do {UI.PRE_KEYWORD}{{Job}}{UI.PST_KEYWORD} Errands";
    public static LocString INCREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = (LocString) $"Prioritize {UI.PRE_KEYWORD}All Errands{UI.PST_KEYWORD} for <b>New Duplicants</b>";
    public static LocString DECREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = (LocString) $"Deprioritize {UI.PRE_KEYWORD}All Errands{UI.PST_KEYWORD} for <b>New Duplicants</b>";
    public static LocString INCREASE_ROW_PRIORITY_MINION_TOOLTIP = (LocString) $"Prioritize {UI.PRE_KEYWORD}All Errands{UI.PST_KEYWORD} for <b>{{Name}}</b>";
    public static LocString DECREASE_ROW_PRIORITY_MINION_TOOLTIP = (LocString) $"Deprioritize {UI.PRE_KEYWORD}All Errands{UI.PST_KEYWORD} for <b>{{Name}}</b>";
    public static LocString INCREASE_PRIORITY_TUTORIAL = (LocString) "{Hotkey} Increase Priority";
    public static LocString DECREASE_PRIORITY_TUTORIAL = (LocString) "{Hotkey} Decrease Priority";
    public static LocString CANNOT_ADJUST_PRIORITY = (LocString) $"Priorities for {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} cannot be adjusted currently because they're in {{1}}";
    public static LocString SORT_TOOLTIP = (LocString) $"Sort by the {UI.PRE_KEYWORD}{{Job}}{UI.PST_KEYWORD} Errand Type";
    public static LocString DISABLED_TOOLTIP = (LocString) $"{{Name}} may not perform {UI.PRE_KEYWORD}{{Job}}{UI.PST_KEYWORD} Errands";
    public static LocString OPTIONS = (LocString) "Options";
    public static LocString TOGGLE_ADVANCED_MODE = (LocString) "Enable Proximity";
    public static LocString TOGGLE_ADVANCED_MODE_TOOLTIP = (LocString) "<b>Errand Proximity Settings</b>\n\nEnabling Proximity settings tells my Duplicants to always choose the closest, most urgent errand to perform.\n\nWhen disabled, Duplicants will choose between two high priority errands based on a hidden priority hierarchy instead.\n\nEnabling Proximity helps cut down on travel time in areas with lots of high priority errands, and is useful for large colonies.";
    public static LocString RESET_SETTINGS = (LocString) "Reset Priorities";
    public static LocString RESET_SETTINGS_TOOLTIP = (LocString) "<b>Reset Priorities</b>\n\nReturns all priorities to their default values.\n\nProximity Enabled: Priorities will be adjusted high-to-low.\n\nProximity Disabled: All priorities will be reset to neutral.";

    public class PRIORITY
    {
      public static LocString VERYHIGH = (LocString) "Very High";
      public static LocString HIGH = (LocString) "High";
      public static LocString STANDARD = (LocString) "Standard";
      public static LocString LOW = (LocString) "Low";
      public static LocString VERYLOW = (LocString) "Very Low";
      public static LocString DISABLED = (LocString) "Disallowed";
    }

    public class PRIORITY_CLASS
    {
      public static LocString IDLE = (LocString) "Idle";
      public static LocString BASIC = (LocString) "Normal";
      public static LocString HIGH = (LocString) "Urgent";
      public static LocString PERSONAL_NEEDS = (LocString) "Personal Needs";
      public static LocString EMERGENCY = (LocString) "Emergency";
      public static LocString COMPULSORY = (LocString) "Involuntary";
    }
  }

  public class VITALSSCREEN
  {
    public static LocString HEALTH = (LocString) "Health";
    public static LocString SICKNESS = (LocString) "Diseases";
    public static LocString NO_SICKNESSES = (LocString) "No diseases";
    public static LocString MULTIPLE_SICKNESSES = (LocString) "Multiple diseases ({0})";
    public static LocString SICKNESS_REMAINING = (LocString) "{0}\n({1})";
    public static LocString STRESS = (LocString) "Stress";
    public static LocString EXPECTATIONS = (LocString) "Expectations";
    public static LocString CALORIES = (LocString) "Fullness";
    public static LocString EATEN_TODAY = (LocString) "Eaten Today";
    public static LocString EATEN_TODAY_TOOLTIP = (LocString) "Consumed {0} of food this cycle";
    public static LocString ATMOSPHERE_CONDITION = (LocString) "Atmosphere:";
    public static LocString SUBMERSION = (LocString) "Liquid Level";
    public static LocString NOT_DROWNING = (LocString) "Liquid Level";
    public static LocString FOOD_EXPECTATIONS = (LocString) "Food Expectation";
    public static LocString FOOD_EXPECTATIONS_TOOLTIP = (LocString) "This Duplicant desires food that is {0} quality or better";
    public static LocString DECOR_EXPECTATIONS = (LocString) "Decor Expectation";
    public static LocString DECOR_EXPECTATIONS_TOOLTIP = (LocString) "This Duplicant desires decor that is {0} or higher";
    public static LocString QUALITYOFLIFE_EXPECTATIONS = (LocString) "Morale";
    public static LocString QUALITYOFLIFE_EXPECTATIONS_TOOLTIP = (LocString) $"This Duplicant requires {UI.FormatAsLink("{0} Morale", "MORALE")}.\n\nCurrent Morale:";
    public static LocString POLLINATION = (LocString) "Pollination";
    public static LocString GERMPRESENCE = (LocString) "Germ Exposure";

    public class CONDITIONS_GROWING
    {
      public class WILD
      {
        public static LocString BASE = (LocString) "<b>Wild Growth\n[Life Cycle: {0}]</b>";
        public static LocString TOOLTIP = (LocString) "This plant will take {0} to grow in the wild";
      }

      public class DOMESTIC
      {
        public static LocString BASE = (LocString) "<b>Domestic Growth\n[Life Cycle: {0}]</b>";
        public static LocString TOOLTIP = (LocString) "This plant will take {0} to grow domestically";
      }

      public class ADDITIONAL_DOMESTIC
      {
        public static LocString BASE = (LocString) "<b>Additional Domestic Growth\n[Life Cycle: {0}]</b>";
        public static LocString TOOLTIP = (LocString) "This plant will take {0} to grow domestically";
      }

      public class WILD_DECOR
      {
        public static LocString BASE = (LocString) "<b>Wild Growth</b>";
        public static LocString TOOLTIP = (LocString) "This plant must have these requirements met to grow in the wild";
      }

      public class WILD_INSTANT
      {
        public static LocString BASE = (LocString) "<b>Wild Growth\n[{0}% Throughput]</b>";
        public static LocString TOOLTIP = (LocString) "This plant must have these requirements met to grow in the wild";
      }

      public class ADDITIONAL_DOMESTIC_INSTANT
      {
        public static LocString BASE = (LocString) "<b>Domestic Growth\n[{0}% Throughput]</b>";
        public static LocString TOOLTIP = (LocString) "This plant must have these requirements met to grow domestically";
      }
    }
  }

  public class SCHEDULESCREEN
  {
    public static LocString SCHEDULE_EDITOR = (LocString) "SCHEDULE EDITOR";
    public static LocString SCHEDULE_NAME_DEFAULT = (LocString) "Default Standard Schedule";
    public static LocString SCHEDULE_NAME_NEW = (LocString) "New Schedule";
    public static LocString SCHEDULE_NAME_FORMAT = (LocString) "Schedule {0}";
    public static LocString SCHEDULE_NAME_DEFAULT_BIONIC = (LocString) "Default Bionic Schedule";
    public static LocString SCHEDULE_NAME_COPY_PREFIX = (LocString) "Copy of ";
    public static LocString SCHEDULE_DROPDOWN_ASSIGNED = (LocString) "{0} (Assigned)";
    public static LocString SCHEDULE_DROPDOWN_BLANK = (LocString) "<i>Move Duplicant...</i>";
    public static LocString SCHEDULE_DOWNTIME_MORALE = (LocString) "Duplicants will receive {0} Morale from the scheduled Downtime shifts";
    public static LocString RENAME_BUTTON_TOOLTIP = (LocString) "Rename custom schedule";
    public static LocString ALARM_BUTTON_ON_TOOLTIP = (LocString) $"Toggle Notifications\n\nSounds and notifications will play when shifts change for this schedule.\n\nENABLED\n{UI.CLICK(UI.ClickType.Click)} to disable";
    public static LocString ALARM_BUTTON_OFF_TOOLTIP = (LocString) $"Toggle Notifications\n\nNo sounds or notifications will play for this schedule.\n\nDISABLED\n{UI.CLICK(UI.ClickType.Click)} to enable";
    public static LocString DELETE_BUTTON_TOOLTIP = (LocString) "Delete Schedule";
    public static LocString PAINT_TOOLS = (LocString) "Paint Tools:";
    public static LocString ADD_SCHEDULE = (LocString) "Add New Schedule";
    public static LocString POO = (LocString) "dar";
    public static LocString DOWNTIME_MORALE = (LocString) "Downtime Morale: {0}";
    public static LocString ALARM_TITLE_ENABLED = (LocString) "Alarm On";
    public static LocString ALARM_TITLE_DISABLED = (LocString) "Alarm Off";
    public static LocString SETTINGS = (LocString) "Settings";
    public static LocString ALARM_BUTTON = (LocString) "Shift Alarms";
    public static LocString RESET_SETTINGS = (LocString) "Reset Shifts";
    public static LocString RESET_SETTINGS_TOOLTIP = (LocString) "Restore this schedule to default shifts";
    public static LocString DELETE_SCHEDULE = (LocString) "Delete Schedule";
    public static LocString DELETE_SCHEDULE_TOOLTIP = (LocString) "Remove this schedule and unassign all Duplicants from it";
    public static LocString DUPLICANT_NIGHTOWL_TOOLTIP = (LocString) $"{(string) DUPLICANTS.TRAITS.NIGHTOWL.NAME}\n• All {UI.PRE_KEYWORD}Attributes{UI.PST_KEYWORD} <b>+3</b> at night";
    public static LocString DUPLICANT_EARLYBIRD_TOOLTIP = (LocString) $"{(string) DUPLICANTS.TRAITS.EARLYBIRD.NAME}\n• All {UI.PRE_KEYWORD}Attributes{UI.PST_KEYWORD} <b>+2</b> in the morning";
    public static LocString SHIFT_SCHEDULE_LEFT_TOOLTIP = (LocString) "Shift all schedule blocks left";
    public static LocString SHIFT_SCHEDULE_RIGHT_TOOLTIP = (LocString) "Shift all schedule blocks right";
    public static LocString SHIFT_SCHEDULE_UP_TOOLTIP = (LocString) "Swap this row with the one above it";
    public static LocString SHIFT_SCHEDULE_DOWN_TOOLTIP = (LocString) "Swap this row with the one below it";
    public static LocString DUPLICATE_SCHEDULE_TIMETABLE = (LocString) "Duplicate this row";
    public static LocString DELETE_SCHEDULE_TIMETABLE = (LocString) "Delete this row\n\nSchedules must have two or more rows in order for one row to be deleted";
    public static LocString DUPLICATE_SCHEDULE = (LocString) "Duplicate this schedule";
  }

  public class COLONYLOSTSCREEN
  {
    public static LocString COLONYLOST = (LocString) "COLONY LOST";
    public static LocString COLONYLOSTDESCRIPTION = (LocString) "All Duplicants are dead or incapacitated.";
    public static LocString RESTARTPROMPT = (LocString) "Press <color=#F44A47><b>[ESC]</b></color> to return to a previous colony, or begin a new one.";
    public static LocString DISMISSBUTTON = (LocString) "DISMISS";
    public static LocString QUITBUTTON = (LocString) "MAIN MENU";
  }

  public class VICTORYSCREEN
  {
    public static LocString HEADER = (LocString) "SUCCESS: IMPERATIVE ACHIEVED!";
    public static LocString DESCRIPTION = (LocString) "I have fulfilled the conditions of one of my Hardwired Imperatives";
    public static LocString RESTARTPROMPT = (LocString) "Press <color=#F44A47><b>[ESC]</b></color> to retire the colony and begin anew.";
    public static LocString DISMISSBUTTON = (LocString) "DISMISS";
    public static LocString RETIREBUTTON = (LocString) "RETIRE COLONY";
  }

  public class GENESHUFFLERMESSAGE
  {
    public static LocString HEADER = (LocString) "NEURAL VACILLATION COMPLETE";
    public static LocString BODY_SUCCESS = (LocString) "Whew! <b>{0}'s</b> brain is still vibrating, but they've never felt better!\n\n<b>{0}</b> acquired the <b>{1}</b> trait.\n\n<b>{1}:</b>\n{2}";
    public static LocString BODY_FAILURE = (LocString) "The machine attempted to alter this Duplicant, but there's no improving on perfection.\n\n<b>{0}</b> already has all positive traits!";
    public static LocString DISMISSBUTTON = (LocString) "DISMISS";
  }

  public class PRINTERCEPTORSCREEN
  {
    public static LocString HEADER = (LocString) "PRINTABLES MENU";
    public static LocString SELECT_ENTITY = (LocString) "View";
    public static LocString SELECT_ENTITY_TOOLTIP = (LocString) "Click for more information about this printable";
    public static LocString PRINT = (LocString) "Print";
    public static LocString PRINT_TOOLTIP = (LocString) $"Click to print selected item\n\nThe number of {UI.PRE_KEYWORD}Data Banks{UI.PST_KEYWORD} required to print additional copies of this item will increase by {{0}} each time";
    public static LocString PRINT_TOOLTIP_DISABLED = (LocString) "Not enough stored materials to complete this printing job\n\nCheck building inventory";
    public static LocString DATABANKS_AVAILABLE = (LocString) "Data Banks Available: {0}";
    public static LocString DATABANKS_COST = (LocString) "Data Banks Required: {0}";
  }

  public class CRASHSCREEN
  {
    public static LocString TITLE = (LocString) "\"Whoops! We're sorry, but it seems your game has encountered an error. It's okay though - these errors are how we find and fix problems to make our game more fun for everyone. If you use the box below to submit a crash report to us, we can use this information to get the issue sorted out.\"";
    public static LocString TITLE_MODS = (LocString) "\"Oops-a-daisy! We're sorry, but it seems your game has encountered an error. If you uncheck all of the mods below, we will be able to help the next time this happens. Any mods that could be related to this error have already been unchecked.\"";
    public static LocString HEADER = (LocString) "OPTIONAL CRASH DESCRIPTION";
    public static LocString HEADER_MODS = (LocString) "ACTIVE MODS";
    public static LocString BODY = (LocString) "Help! A black hole ate my game!";
    public static LocString THANKYOU = (LocString) "Thank you!\n\nYou're making our game better, one crash at a time.";
    public static LocString UPLOAD_FAILED = (LocString) "There was an issue in reporting this crash.\n\nPlease submit a bug report at:\n<u>https://forums.kleientertainment.com/klei-bug-tracker/oni/</u>";
    public static LocString UPLOADINFO = (LocString) "UPLOAD ADDITIONAL INFO ({0})";
    public static LocString REPORTBUTTON = (LocString) "REPORT CRASH";
    public static LocString REPORTING = (LocString) "REPORTING, PLEASE WAIT...";
    public static LocString CONTINUEBUTTON = (LocString) "CONTINUE GAME";
    public static LocString MOREINFOBUTTON = (LocString) "MORE INFO";
    public static LocString COPYTOCLIPBOARDBUTTON = (LocString) "COPY TO CLIPBOARD";
    public static LocString QUITBUTTON = (LocString) "QUIT TO DESKTOP";
    public static LocString SAVEFAILED = (LocString) "Save Failed: {0}";
    public static LocString LOADFAILED = (LocString) "Load Failed: {0}\nSave Version: {1}\nExpected: {2}";
    public static LocString REPORTEDERROR_SUCCESS = (LocString) "Thank you for reporting this error.";
    public static LocString REPORTEDERROR_FAILURE_TOO_LARGE = (LocString) "Unable to report error. Save file is too large. Please contact us using the bug tracker.";
    public static LocString REPORTEDERROR_FAILURE = (LocString) "Unable to report error. Please contact us using the bug tracker.";
    public static LocString UPLOADINPROGRESS = (LocString) "Submitting {0}";
  }

  public class DEMOOVERSCREEN
  {
    public static LocString TIMEREMAINING = (LocString) "Demo time remaining:";
    public static LocString TIMERTOOLTIP = (LocString) "Demo time remaining";
    public static LocString TIMERINACTIVE = (LocString) "Timer inactive";
    public static LocString DEMOOVER = (LocString) "END OF DEMO";
    public static LocString DESCRIPTION = (LocString) "Thank you for playing <color=#F44A47>Oxygen Not Included</color>!";
    public static LocString DESCRIPTION_2 = (LocString) "";
    public static LocString QUITBUTTON = (LocString) "RESET";
  }

  public class CREDITSSCREEN
  {
    public static LocString TITLE = (LocString) "CREDITS";
    public static LocString CLOSEBUTTON = (LocString) "CLOSE";

    public class THIRD_PARTY
    {
      public static LocString FMOD = (LocString) "FMOD Sound System\nCopyright Firelight Technologies";
      public static LocString HARMONY = (LocString) "Harmony by Andreas Pardeike";
    }
  }

  public class ALLRESOURCESSCREEN
  {
    public static LocString RESOURCES_TITLE = (LocString) nameof (RESOURCES);
    public static LocString RESOURCES = (LocString) "Resources";
    public static LocString SEARCH = (LocString) "Search";
    public static LocString NAME = (LocString) "Resource";
    public static LocString TOTAL = (LocString) "Total";
    public static LocString AVAILABLE = (LocString) "Available";
    public static LocString RESERVED = (LocString) "Reserved";
    public static LocString SEARCH_PLACEHODLER = (LocString) "Enter text...";
    public static LocString FIRST_FRAME_NO_DATA = (LocString) "...";
    public static LocString PIN_TOOLTIP = (LocString) "Check to pin resource to side panel";
    public static LocString UNPIN_TOOLTIP = (LocString) "Unpin resource";
  }

  public class PRIORITYSCREEN
  {
    public static LocString BASIC = (LocString) "Set the order in which specific pending errands should be done\n\n1: Least Urgent\n9: Most Urgent";
    public static LocString HIGH = (LocString) "";
    public static LocString TOP_PRIORITY = (LocString) "Top Priority\n\nThis priority will override all other priorities and set the colony on Yellow Alert until the errand is completed";
    public static LocString HIGH_TOGGLE = (LocString) "";
    public static LocString OPEN_JOBS_SCREEN = (LocString) $"{UI.CLICK(UI.ClickType.Click)} to open the Priorities Screen\n\nDuplicants will first decide what to work on based on their {UI.PRE_KEYWORD}Duplicant Priorities{UI.PST_KEYWORD}, and then decide where to work based on {UI.PRE_KEYWORD}Building Priorities{UI.PST_KEYWORD}";
    public static LocString DIAGRAM = (LocString) $"Duplicants will first choose what {UI.PRE_KEYWORD}Errand Type{UI.PST_KEYWORD} to perform using their {UI.PRE_KEYWORD}Duplicant Priorities{UI.PST_KEYWORD} {UI.FormatAsHotKey(Action.ManagePriorities)}\n\nThey will then choose one {UI.PRE_KEYWORD}Errand{UI.PST_KEYWORD} from within that type using the {UI.PRE_KEYWORD}Building Priorities{UI.PST_KEYWORD} set by this tool";
    public static LocString DIAGRAM_TITLE = (LocString) "BUILDING PRIORITY";
  }

  public class RESOURCESCREEN
  {
    public static LocString HEADER = (LocString) "RESOURCES";
    public static LocString CATEGORY_TOOLTIP = (LocString) $"Counts all unallocated resources within reach\n\n{UI.CLICK(UI.ClickType.Click)} to expand";
    public static LocString AVAILABLE_TOOLTIP = (LocString) "Available: <b>{0}</b>\n({1} of {2} allocated to pending errands)";
    public static LocString TREND_TOOLTIP = (LocString) "The available amount of this resource has {0} {1} in the last cycle";
    public static LocString TREND_TOOLTIP_NO_CHANGE = (LocString) "The available amount of this resource has NOT CHANGED in the last cycle";
    public static LocString FLAT_STR = (LocString) "<b>NOT CHANGED</b>";
    public static LocString INCREASING_STR = (LocString) $"<color={Constants.POSITIVE_COLOR_STR}>INCREASED</color>";
    public static LocString DECREASING_STR = (LocString) $"<color={Constants.NEGATIVE_COLOR_STR}>DECREASED</color>";
    public static LocString CLEAR_NEW_RESOURCES = (LocString) "Clear New";
    public static LocString CLEAR_ALL = (LocString) "Unpin all resources";
    public static LocString SEE_ALL = (LocString) "+ See All ({0})";
    public static LocString NEW_TAG = (LocString) "NEW";
  }

  public class CONFIRMDIALOG
  {
    public static LocString OK = (LocString) nameof (OK);
    public static LocString CANCEL = (LocString) nameof (CANCEL);
    public static LocString DIALOG_HEADER = (LocString) "MESSAGE";
  }

  public class FACADE_SELECTION_PANEL
  {
    public static LocString HEADER = (LocString) "Select Blueprint";
    public static LocString STORE_BUTTON_TOOLTIP = (LocString) "See more Blueprints in the Supply Closet";
  }

  public class FILE_NAME_DIALOG
  {
    public static LocString ENTER_TEXT = (LocString) "Enter Text...";
  }

  public class MINION_IDENTITY_SORT
  {
    public static LocString TITLE = (LocString) "Sort By";
    public static LocString NAME = (LocString) "Duplicant";
    public static LocString ROLE = (LocString) "Role";
    public static LocString PERMISSION = (LocString) "Permission";
  }

  public class UISIDESCREENS
  {
    public class TABS
    {
      public static LocString HEADER = (LocString) "Options";
      public static LocString CONFIGURATION = (LocString) "Config";
      public static LocString MATERIAL = (LocString) "Material";
      public static LocString SKIN = (LocString) "Blueprint";
    }

    public class BLUEPRINT_TAB
    {
      public static LocString EDIT_OUTFIT_BUTTON = (LocString) "Restyle";
      public static LocString SUBCATEGORY_OUTFIT = (LocString) "Clothing";
      public static LocString SUBCATEGORY_ATMOSUIT = (LocString) "Atmo Suit";
      public static LocString SUBCATEGORY_JOYRESPONSE = (LocString) "Overjoyed";
      public static LocString SUBCATEGORY_JETSUIT = (LocString) "Jet Suit";
    }

    public class NOCONFIG
    {
      public static LocString TITLE = (LocString) "No configuration";
      public static LocString LABEL = (LocString) "There is no configuration available for this object.";
    }

    public class ARTABLESELECTIONSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Style Selection";
      public static LocString BUTTON = (LocString) "Redecorate";
      public static LocString BUTTON_TOOLTIP = (LocString) "Clears current artwork\n\nCreates errand for a skilled Duplicant to create selected style";
      public static LocString CLEAR_BUTTON_TOOLTIP = (LocString) "Clears current artwork\n\nAllows a skilled Duplicant to create artwork of their choice";
    }

    public class ARTIFACTANALYSISSIDESCREEN
    {
      public static LocString NO_ARTIFACTS_DISCOVERED = (LocString) "No artifacts analyzed";
      public static LocString NO_ARTIFACTS_DISCOVERED_TOOLTIP = (LocString) "Analyzing artifacts requires a Duplicant with the Masterworks skill";
    }

    public class BUTTONMENUSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Building Menu";
      public static LocString ALLOW_INTERNAL_CONSTRUCTOR = (LocString) "Enable Auto-Delivery";
      public static LocString ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = (LocString) $"Order Duplicants to deliver {{0}}{UI.FormatAsLink("s", "NONE")} to this building automatically when they need replacing";
      public static LocString DISALLOW_INTERNAL_CONSTRUCTOR = (LocString) "Cancel Auto-Delivery";
      public static LocString DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = (LocString) "Cancel automatic {0} deliveries to this building";
    }

    public class LOREBEARERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Lore Unlock";
      public static LocString BUTTON = (LocString) "Read Files";
    }

    public class CONFIGURECONSUMERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Configure Building";
      public static LocString SELECTION_DESCRIPTION_HEADER = (LocString) "Description";
    }

    public class TREEFILTERABLESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Element Filter";
      public static LocString TITLE_CRITTER = (LocString) "Critter Filter";
      public static LocString ALLBUTTON = (LocString) "All Standard";
      public static LocString ALLBUTTONTOOLTIP = (LocString) "Allow storage of all standard resources preferred by this building\n\nNon-standard resources must be selected manually\n\nNon-standard resources include:\n    • Clothing\n    • Critter Eggs\n    • Sublimators";
      public static LocString ALLBUTTON_EDIBLES = (LocString) "All Edibles";
      public static LocString ALLBUTTON_EDIBLES_TOOLTIP = (LocString) "Allow storage of all edible resources";
      public static LocString ALLBUTTON_CRITTERS = (LocString) "All Critters";
      public static LocString ALLBUTTON_CRITTERS_TOOLTIP = (LocString) $"Allow storage of all eligible {UI.PRE_KEYWORD}Critters{UI.PST_KEYWORD}";
      public static LocString SPECIAL_RESOURCES = (LocString) "Non-Standard";
      public static LocString SPECIAL_RESOURCES_TOOLTIP = (LocString) "These objects may not be ideally suited to storage";
      public static LocString CATEGORYBUTTONTOOLTIP = (LocString) "Allow storage of anything in the {0} resource category";
      public static LocString MATERIALBUTTONTOOLTIP = (LocString) "Add or remove this material from storage";
      public static LocString ONLYALLOWTRANSPORTITEMSBUTTON = (LocString) "Sweep Only";
      public static LocString ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP = (LocString) "Only store objects marked Sweep <color=#F44A47><b>[K]</b></color> in this container";
      public static LocString ONLYALLOWSPICEDITEMSBUTTON = (LocString) "Seasoned Food Only";
      public static LocString ONLYALLOWSPICEDITEMSBUTTONTOOLTIP = (LocString) $"Only store foods that have been seasoned at the {UI.PRE_KEYWORD}Spice Grinder{UI.PST_KEYWORD}";
      public static LocString SEARCH_PLACEHOLDER = (LocString) "Search";
    }

    public class TELESCOPESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Telescope Configuration";
      public static LocString NO_SELECTED_ANALYSIS_TARGET = (LocString) $"No analysis focus selected\nOpen the {UI.FormatAsManagementMenu("Starmap", Action.ManageStarmap)} to select a focus";
      public static LocString ANALYSIS_TARGET_SELECTED = (LocString) "Object focus selected\nAnalysis underway";
      public static LocString OPENSTARMAPBUTTON = (LocString) "OPEN STARMAP";
      public static LocString ANALYSIS_TARGET_HEADER = (LocString) "Object Analysis";
    }

    public class CLUSTERTELESCOPESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Telescope Configuration";
      public static LocString CHECKBOX_METEORS = (LocString) "Allow meteor shower identification";
      public static LocString CHECKBOX_TOOLTIP_METEORS = (LocString) $"Prioritizes unidentified meteors that come within range in a previously revealed location\n\nWill interrupt a Duplicant working on revealing a new {UI.PRE_KEYWORD}Starmap{UI.PST_KEYWORD} location";
    }

    public class TEMPORALTEARSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Temporal Tear";
      public static LocString BUTTON_OPEN = (LocString) "Enter Tear";
      public static LocString BUTTON_CLOSED = (LocString) "Tear Closed";
      public static LocString BUTTON_LABEL = (LocString) "Enter Temporal Tear";
      public static LocString CONFIRM_POPUP_MESSAGE = (LocString) "Are you sure you want to fire this?";
      public static LocString CONFIRM_POPUP_CONFIRM = (LocString) "Yes, I'm ready for a meteor shower.";
      public static LocString CONFIRM_POPUP_CANCEL = (LocString) "No, I need more time to prepare.";
      public static LocString CONFIRM_POPUP_TITLE = (LocString) "Temporal Tear Opener";
    }

    public class RAILGUNSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Launcher Configuration";
      public static LocString NO_SELECTED_LAUNCH_TARGET = (LocString) $"No destination selected\nOpen the {UI.FormatAsManagementMenu("Starmap", Action.ManageStarmap)} to set a course";
      public static LocString LAUNCH_TARGET_SELECTED = (LocString) "Launcher destination {0} set";
      public static LocString OPENSTARMAPBUTTON = (LocString) "OPEN STARMAP";
      public static LocString LAUNCH_RESOURCES_HEADER = (LocString) "Launch Resources:";
      public static LocString MINIMUM_PAYLOAD_MASS = (LocString) "Minimum launch mass:";
    }

    public class CLUSTERWORLDSIDESCREEN
    {
      public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;
      public static LocString VIEW_WORLD = (LocString) ("Oversee " + (string) UI.CLUSTERMAP.PLANETOID);
      public static LocString VIEW_WORLD_DISABLE_TOOLTIP = (LocString) ("Cannot view " + (string) UI.CLUSTERMAP.PLANETOID);
      public static LocString VIEW_WORLD_TOOLTIP = (LocString) $"View this {(string) UI.CLUSTERMAP.PLANETOID}'s surface";
    }

    public class ROCKETVIEWINTERIORSECTION
    {
      public class BUTTONVIEWINTERIOR
      {
        public static LocString TITLE = (LocString) "Rocket Interior";
        public static LocString LABEL = (LocString) "View Interior";
        public static LocString DESC = (LocString) "What's goin' on in there?";
        public static LocString INVALID = (LocString) $"This rocket does not have a {UI.PRE_KEYWORD}Spacefarer{UI.PST_KEYWORD} module";
      }

      public class BUTTONVIEWEXTERIOR
      {
        public static LocString LABEL = (LocString) "View Exterior";
        public static LocString DESC = (LocString) "Switch to external world view";
        public static LocString INVALID = (LocString) "Not available while rocket is in flight";
      }
    }

    public class ROCKETMODULESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Module";
      public static LocString CHANGEMODULEPANEL = (LocString) "Add or Change Module";
      public static LocString ENGINE_MAX_HEIGHT = (LocString) "This engine allows a <b>Maximum Rocket Height</b> of {0}";

      public class MODULESTATCHANGE
      {
        public static LocString TITLE = (LocString) "Rocket stats on construction:";
        public static LocString BURDEN = (LocString) $"    • {(string) DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME}: {{0}} ({{1}})";
        public static LocString RANGE = (LocString) $"    • Potential {(string) DUPLICANTS.ATTRIBUTES.FUELRANGEPERKILOGRAM.NAME}: {{0}}/1{(string) UI.UNITSUFFIXES.MASS.KILOGRAM} Fuel ({{1}})";
        public static LocString SPEED = (LocString) "    • Speed: {0} ({1})";
        public static LocString ENGINEPOWER = (LocString) $"    • {(string) DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME}: {{0}} ({{1}})";
        public static LocString HEIGHT = (LocString) $"    • {(string) DUPLICANTS.ATTRIBUTES.HEIGHT.NAME}: {{0}}/{{2}} ({{1}})";
        public static LocString HEIGHT_NOMAX = (LocString) $"    • {(string) DUPLICANTS.ATTRIBUTES.HEIGHT.NAME}: {{0}} ({{1}})";
        public static LocString POSITIVEDELTA = (LocString) UI.FormatAsPositiveModifier("{0}");
        public static LocString NEGATIVEDELTA = (LocString) UI.FormatAsNegativeModifier("{0}");
      }

      public class BUTTONSWAPMODULEUP
      {
        public static LocString DESC = (LocString) "Swap this rocket module with the one above";
        public static LocString INVALID = (LocString) "No module above may be swapped.\n\n    • A module above may be unable to have modules placed above it.\n    • A module above may be unable to fit into the space below it.\n    • This module may be unable to fit into the space above it.";
      }

      public class BUTTONSWAPMODULEDOWN
      {
        public static LocString DESC = (LocString) "Swap this rocket module with the one below";
        public static LocString INVALID = (LocString) "No module below may be swapped.\n\n    • A module below may be unable to have modules placed below it.\n    • A module below may be unable to fit into the space above it.\n    • This module may be unable to fit into the space below it.";
      }

      public class BUTTONCHANGEMODULE
      {
        public static LocString LABEL = (LocString) "Change Module";
        public static LocString DESC = (LocString) "Swap this module for a different module";
        public static LocString INVALID = (LocString) "This module cannot be changed to a different type";
      }

      public class BUTTONREMOVEMODULE
      {
        public static LocString LABEL = (LocString) "Deconstruct";
        public static LocString LABEL_CANCEL = (LocString) "Cancel Deconstruct";
        public static LocString DESC = (LocString) "Remove this module";
        public static LocString DESC_CANCEL = (LocString) "Cancel the order for deconstructing this module";
        public static LocString INVALID = (LocString) "This module cannot be removed";
      }

      public class ADDMODULE
      {
        public static LocString LABEL = (LocString) "Add Module";
        public static LocString DESC = (LocString) "Add a new module above this one";
        public static LocString INVALID = (LocString) "Modules cannot be added above this module, or there is no room above to add a module";
      }
    }

    public class CLUSTERLOCATIONFILTERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Location Filter";
      public static LocString HEADER = (LocString) "Send Green signal at locations";
      public static LocString EMPTY_SPACE_ROW = (LocString) "In Space";
    }

    public class DISPENSERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Dispenser";
      public static LocString BUTTON_CANCEL = (LocString) "Cancel order";
      public static LocString BUTTON_DISPENSE = (LocString) "Dispense item";
    }

    public class ROCKETRESTRICTIONSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Rocket Restrictions";
      public static LocString BUILDING_RESTRICTIONS_LABEL = (LocString) "Interior Building Restrictions";
      public static LocString NONE_RESTRICTION_BUTTON = (LocString) "None";
      public static LocString NONE_RESTRICTION_BUTTON_TOOLTIP = (LocString) "There are no restrictions on buildings inside this rocket";
      public static LocString GROUNDED_RESTRICTION_BUTTON = (LocString) "Grounded";
      public static LocString GROUNDED_RESTRICTION_BUTTON_TOOLTIP = (LocString) "Buildings with their access restricted cannot be operated while grounded, though they can still be filled";
      public static LocString AUTOMATION = (LocString) "Automation Controlled";
      public static LocString AUTOMATION_TOOLTIP = (LocString) "Building restrictions are managed by automation\n\nBuildings with their access restricted cannot be operated, though they can still be filled";
    }

    public class HABITATMODULESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Spacefarer Module";
      public static LocString VIEW_BUTTON = (LocString) "View Interior";
      public static LocString VIEW_BUTTON_TOOLTIP = (LocString) "What's goin' on in there?";
    }

    public class HARVESTMODULESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Mining Resources";
      public static LocString MINING_IN_PROGRESS = (LocString) "Drilling...";
      public static LocString MINING_STOPPED = (LocString) "Not drilling";
      public static LocString ENABLE = (LocString) "Enable Drill";
      public static LocString DISABLE = (LocString) "Disable Drill";
    }

    public class CARGOMODULESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Harvesting Resources";
      public static LocString GATHERING_IN_PROGRESS = (LocString) "Gathering...";
      public static LocString GATHERING_FULL = (LocString) "Storage Full";
      public static LocString GATHERING_STOPPED = (LocString) "Not gathering";
      public static LocString ENABLE = (LocString) "Enable Gather";
      public static LocString DISABLE = (LocString) "Disable Gather";
    }

    public class SELECTMODULESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Select Module";
      public static LocString BUILDBUTTON = (LocString) "Build";

      public class CONSTRAINTS
      {
        public class RESEARCHED
        {
          public static LocString COMPLETE = (LocString) "Research Completed";
          public static LocString FAILED = (LocString) "Research Incomplete";
        }

        public class MATERIALS_AVAILABLE
        {
          public static LocString COMPLETE = (LocString) "Materials available";
          public static LocString FAILED = (LocString) "• Materials unavailable";
        }

        public class ONE_COMMAND_PER_ROCKET
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Command module already installed";
        }

        public class ONE_ENGINE_PER_ROCKET
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Engine module already installed";
        }

        public class ENGINE_AT_BOTTOM
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Must install at bottom of rocket";
        }

        public class TOP_ONLY
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Must install at top of rocket";
        }

        public class SPACE_AVAILABLE
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Space above rocket blocked";
        }

        public class PASSENGER_MODULE_AVAILABLE
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Max number of passenger modules installed";
        }

        public class MAX_MODULES
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Max module limit of engine reached";
        }

        public class MAX_HEIGHT
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Engine's height limit reached or exceeded";
          public static LocString FAILED_NO_ENGINE = (LocString) "• Rocket requires space for an engine";
        }

        public class ONE_ROBOPILOT_PER_ROCKET
        {
          public static LocString COMPLETE = (LocString) "";
          public static LocString FAILED = (LocString) "• Robo-Pilot module already installed";
        }
      }
    }

    public class FILTERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Filter Outputs";
      public static LocString NO_SELECTION = (LocString) "None";
      public static LocString OUTPUTELEMENTHEADER = (LocString) "Output 1";
      public static LocString SELECTELEMENTHEADER = (LocString) "Output 2";
      public static LocString OUTPUTRED = (LocString) "Output Red";
      public static LocString OUTPUTGREEN = (LocString) "Output Green";
      public static LocString NOELEMENTSELECTED = (LocString) "No element selected";
      public static LocString DRIEDFOOD = (LocString) "Dried Food";

      public static class UNFILTEREDELEMENTS
      {
        public static LocString GAS = (LocString) "Gas Output:\nAll";
        public static LocString LIQUID = (LocString) "Liquid Output:\nAll";
        public static LocString SOLID = (LocString) "Solid Output:\nAll";
      }

      public static class FILTEREDELEMENT
      {
        public static LocString GAS = (LocString) "Filtered Gas Output:\n{0}";
        public static LocString LIQUID = (LocString) "Filtered Liquid Output:\n{0}";
        public static LocString SOLID = (LocString) "Filtered Solid Output:\n{0}";
      }
    }

    public class SINGLEITEMSELECTIONSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Element Filter";
      public static LocString LIST_TITLE = (LocString) "Options";
      public static LocString NO_SELECTION = (LocString) "None";

      public class CURRENT_ITEM_SELECTED_SECTION
      {
        public static LocString TITLE = (LocString) "Current Selection";
        public static LocString NO_ITEM_TITLE = (LocString) "No Item Selected";
        public static LocString NO_ITEM_MESSAGE = (LocString) "Select an item for storage below.";
      }
    }

    public class FEWOPTIONSELECTIONSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Options";
    }

    public class MISSILESELECTIONSIDESCREEN
    {
      public static LocString TITLE = BUILDINGS.PREFABS.MISSILELAUNCHER.NAME;
      public static LocString HEADER = (LocString) "Projectile Selection";

      public class VANILLALARGEIMPACTOR
      {
        public static LocString HEALTH_BAR_TITLE = (LocString) "Health";
        public static LocString HEALTH_BAR_TOOLTIP = (LocString) "Demolior health: {0} / {1}";
        public static LocString TIME_UNTIL_COLLISION_TITLE = (LocString) "Time Until Impact";
        public static LocString TIME_UNTIL_COLLISION_TOOLTIP = (LocString) "{0} cycles remaining until impact";
      }
    }

    public class LOGICBROADCASTCHANNELSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Channel Selector";
      public static LocString HEADER = (LocString) "Channel Selector";
      public static LocString IN_RANGE = (LocString) "In Range";
      public static LocString OUT_OF_RANGE = (LocString) "Out of Range";
      public static LocString NO_SENDERS = (LocString) "No Channels Available";
      public static LocString NO_SENDERS_DESC = (LocString) $"Build a {(string) BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.NAME} to transmit a signal.";
    }

    public class CONDITIONLISTSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Condition List";
    }

    public class FABRICATORSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Production Orders";
      public static LocString SUBTITLE = (LocString) "Recipes";
      public static LocString NORECIPEDISCOVERED = (LocString) "No discovered recipes";
      public static LocString NORECIPEDISCOVERED_BODY = (LocString) "Discover new ingredients or research new technology to unlock some recipes.";
      public static LocString UNDISCOVERED_RECIPES = (LocString) "The following recipes are not yet discovered.\nI must discover new ingredients or research new technology to unlock them:";
      public static LocString NORECIPESELECTED = (LocString) "No recipe selected";
      public static LocString SELECTRECIPE = (LocString) "Select a recipe to fabricate.";
      public static LocString COST = (LocString) "<b>Ingredients:</b>\n";
      public static LocString RESULTREQUIREMENTS = (LocString) "<b>Requirements:</b>";
      public static LocString RESULTEFFECTS = (LocString) "<b>Effects:</b>";
      public static LocString KG = (LocString) "- {0}: {1}\n";
      public static LocString INFORMATION = (LocString) nameof (INFORMATION);
      public static LocString CANCEL = (LocString) "Cancel";
      public static LocString RECIPE_REQUIREMENT = (LocString) "{0}: {1}";
      public static LocString RECIPE_AVAILABLE = (LocString) "Available: {0}";
      public static LocString RECIPEPRODUCT = (LocString) "{0}: {1}";
      public static LocString UNITS_AND_CALS = (LocString) "{0} [{1}]";
      public static LocString CALS = (LocString) "{0}";
      public static LocString QUEUED_MISSING_INGREDIENTS_TOOLTIP = (LocString) "Missing {0} of {1}\n";
      public static LocString CURRENT_ORDER = (LocString) "Current order: {0}";
      public static LocString NEXT_ORDER = (LocString) "Next order: {0}";
      public static LocString NO_WORKABLE_ORDER = (LocString) "No workable order";
      public static LocString RECIPE_DETAILS = (LocString) "Recipe Details";
      public static LocString RECIPE_QUEUE = (LocString) "Order Production Quantity: ";
      public static LocString RECIPE_QUEUE_CLICK_DESCRIPTION = (LocString) $"<b>{UI.CLICK(UI.ClickType.Click)} to select the next queued variant of this recipe</b>";
      public static LocString RECIPE_FOREVER = (LocString) "Forever";
      public static LocString RECIPE_NONE = (LocString) "No Orders Queued";
      public static LocString CHANGE_RECIPE_ARROW_LABEL = (LocString) "Change recipe";
      public static LocString RECIPE_RESEARCH_REQUIRED = (LocString) "Research Required";
      public static LocString RECIPE_UNDISCOVERED_INGREDIENTS = (LocString) "Undiscovered ingredients";
      public static LocString INGREDIENT_CATEGORY = (LocString) "Ingredient #{0}";
      public static LocString ADDITIONAL_REQUIREMENTS = (LocString) "Additional Requirements";
      public static LocString ADDITIONAL_REQUIREMENTS_TOOLTIP = (LocString) $"This recipe requires {(string) ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME} to be collected by the building's input port\n\nOpen the {UI.FormatAsBuildMenuTab("Automation Overlay", Action.Overlay13)} to view building ports";
      public static LocString NO_DISCOVERED_INGREDIENTS = (LocString) "No ingredients discovered";
      public static LocString UNDISCOVERED_INGREDIENTS_IN_CATEGORY = (LocString) "Some ingredient options have not been discovered yet:\n\n{0}";
      public static LocString ALL_INGREDIENTS_IN_CATEGORY_DISOVERED = (LocString) "All ingredient options in this category have been discovered.";
      public static LocString INGREDIENTS = (LocString) "<b>Ingredients:</b>";
      public static LocString RECIPE_EFFECTS = (LocString) "<b>Effects:</b>";
      public static LocString RECIPE_EFFECTS_HEADER = (LocString) "Effects";
      public static LocString ALLOW_MUTANT_SEED_INGREDIENTS = (LocString) "Building accepts mutant seeds";
      public static LocString ALLOW_MUTANT_SEED_INGREDIENTS_TOOLTIP = (LocString) "Toggle whether Duplicants will deliver mutant seed species to this building as recipe ingredients";
      public static LocString RECIPE_RADBOLTS_REQUIRED = (LocString) ((string) ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME + ": {0}");

      public class TOOLTIPS
      {
        public static LocString RECIPE_WORKTIME = (LocString) "This recipe takes {0} to complete";
        public static LocString RECIPERQUIREMENT_SUFFICIENT = (LocString) "This recipe consumes {1} of an available {2} of {0}";
        public static LocString RECIPERQUIREMENT_INSUFFICIENT = (LocString) "This recipe requires {1} {0}\nAvailable: {2}";
        public static LocString RECIPEPRODUCT = (LocString) "This recipe produces {1} {0}";
        public static LocString ADDITIONAL_INGREDIENT_OPTIONS_MESSAGE = (LocString) $"{UIConstants.ColorPrefixYellow}Alternative ingredient options are available.{UIConstants.ColorSuffix}";
      }

      public class EFFECTS
      {
        public static LocString OXYGEN_TANK = (LocString) ((string) EQUIPMENT.PREFABS.OXYGEN_TANK.NAME + " ({0})");
        public static LocString OXYGEN_TANK_UNDERWATER = (LocString) ((string) EQUIPMENT.PREFABS.OXYGEN_TANK_UNDERWATER.NAME + " ({0})");
        public static LocString JETSUIT_TANK = (LocString) ((string) EQUIPMENT.PREFABS.JET_SUIT.TANK_EFFECT_NAME + " ({0})");
        public static LocString LEADSUIT_BATTERY = (LocString) ((string) EQUIPMENT.PREFABS.LEAD_SUIT.BATTERY_EFFECT_NAME + " ({0})");
        public static LocString COOL_VEST = (LocString) ((string) EQUIPMENT.PREFABS.COOL_VEST.NAME + " ({0})");
        public static LocString WARM_VEST = (LocString) ((string) EQUIPMENT.PREFABS.WARM_VEST.NAME + " ({0})");
        public static LocString FUNKY_VEST = (LocString) ((string) EQUIPMENT.PREFABS.FUNKY_VEST.NAME + " ({0})");
        public static LocString RESEARCHPOINT = (LocString) "{0}: +1";
      }

      public class RECIPE_CATEGORIES
      {
        public static LocString ATMO_SUIT_FACADES = (LocString) "Atmo Suit Styles";
        public static LocString JET_SUIT_FACADES = (LocString) "Jet Suit Styles";
        public static LocString LEAD_SUIT_FACADES = (LocString) "Lead Suit Styles";
        public static LocString PRIMO_GARB_FACADES = (LocString) "Primo Garb Styles";
      }
    }

    public class ASSIGNMENTGROUPCONTROLLER
    {
      public static LocString TITLE = (LocString) "Duplicant Assignment";
      public static LocString PILOT = (LocString) "Pilot";
      public static LocString OFFWORLD = (LocString) "Offworld";

      public class TOOLTIPS
      {
        public static LocString DIFFERENT_WORLD = (LocString) ("This Duplicant is on a different " + (string) UI.CLUSTERMAP.PLANETOID);
        public static LocString ASSIGN = (LocString) "<b>Add</b> this Duplicant to rocket crew";
        public static LocString UNASSIGN = (LocString) "<b>Remove</b> this Duplicant from rocket crew";
      }
    }

    public class LAUNCHPADSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Rocket Platform";
      public static LocString WAITING_TO_LAND_PANEL = (LocString) "Waiting to land";
      public static LocString NO_ROCKETS_WAITING = (LocString) "No rockets in orbit";
      public static LocString IN_ORBIT_ABOVE_PANEL = (LocString) "Rockets in orbit";
      public static LocString NEW_ROCKET_BUTTON = (LocString) "NEW ROCKET";
      public static LocString LAND_BUTTON = (LocString) "LAND HERE";
      public static LocString CANCEL_LAND_BUTTON = (LocString) "CANCEL";
      public static LocString LAUNCH_BUTTON = (LocString) "BEGIN LAUNCH SEQUENCE";
      public static LocString LAUNCH_BUTTON_DEBUG = (LocString) "BEGIN LAUNCH SEQUENCE (DEBUG ENABLED)";
      public static LocString LAUNCH_BUTTON_TOOLTIP = (LocString) "Blast off!";
      public static LocString LAUNCH_BUTTON_NOT_READY_TOOLTIP = (LocString) "This rocket is <b>not</b> ready to launch\n\n<b>Review the Launch Checklist in the status panel for more detail</b>";
      public static LocString LAUNCH_WARNINGS_BUTTON = (LocString) "ACKNOWLEDGE WARNINGS";
      public static LocString LAUNCH_WARNINGS_BUTTON_TOOLTIP = (LocString) $"Some items in the Launch Checklist require attention\n\n<b>{UI.CLICK(UI.ClickType.Click)} to ignore warnings and proceed with launch</b>";
      public static LocString LAUNCH_REQUESTED_BUTTON = (LocString) "CANCEL LAUNCH";
      public static LocString LAUNCH_REQUESTED_BUTTON_TOOLTIP = (LocString) $"This rocket will take off as soon as a Duplicant takes the controls\n\n<b>{UI.CLICK(UI.ClickType.Click)} to cancel launch</b>";
      public static LocString LAUNCH_AUTOMATION_CONTROLLED = (LocString) "AUTOMATION CONTROLLED";
      public static LocString LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP = (LocString) $"This {(string) BUILDINGS.PREFABS.LAUNCHPAD.NAME}'s launch operation is controlled by automation signals";

      public class STATUS
      {
        public static LocString STILL_PREPPING = (LocString) "Launch Checklist Incomplete";
        public static LocString READY_FOR_LAUNCH = (LocString) "Ready to Launch";
        public static LocString LOADING_CREW = (LocString) "Loading crew...";
        public static LocString UNLOADING_PASSENGERS = (LocString) "Unloading non-crew...";
        public static LocString WAITING_FOR_PILOT = (LocString) "Pilot requested at control station...";
        public static LocString COUNTING_DOWN = (LocString) "5... 4... 3... 2... 1...";
        public static LocString TAKING_OFF = (LocString) "Liftoff!!";
      }
    }

    public class AUTOPLUMBERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Automatic Building Configuration";

      public class BUTTONS
      {
        public class POWER
        {
          public static LocString TOOLTIP = (LocString) "Add Dev Generator and Electrical Wires";
        }

        public class PIPES
        {
          public static LocString TOOLTIP = (LocString) "Add Dev Pumps and Pipes";
        }

        public class SOLIDS
        {
          public static LocString TOOLTIP = (LocString) "Spawn solid resources for a relevant recipe or conversions";
        }

        public class MINION
        {
          public static LocString TOOLTIP = (LocString) "Spawn a Duplicant in front of the building";
        }

        public class FACADE
        {
          public static LocString TOOLTIP = (LocString) "Toggle the building blueprint";
        }
      }
    }

    public class SELFDESTRUCTSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Self Destruct";
      public static LocString MESSAGE_TEXT = (LocString) "EMERGENCY PROCEDURES";
      public static LocString BUTTON_TEXT = (LocString) "ABANDON SHIP!";
      public static LocString BUTTON_TEXT_CONFIRM = (LocString) "CONFIRM ABANDON SHIP";
      public static LocString BUTTON_TOOLTIP = (LocString) "This rocket is equipped with an emergency escape system.\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";
      public static LocString BUTTON_TOOLTIP_CONFIRM = (LocString) "<b>This will eject any passengers and destroy the rocket.<b>\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";
    }

    public class GENESHUFFLERSIDESREEN
    {
      public static LocString TITLE = (LocString) "Neural Vacillator";
      public static LocString COMPLETE = (LocString) "Something feels different.";
      public static LocString UNDERWAY = (LocString) "Neural Vacillation in progress.";
      public static LocString CONSUMED = (LocString) "There are no charges left in this Vacillator.";
      public static LocString CONSUMED_WAITING = (LocString) "Recharge requested, awaiting delivery by Duplicant.";
      public static LocString BUTTON = (LocString) "Complete Neural Process";
      public static LocString BUTTON_RECHARGE = (LocString) "Recharge";
      public static LocString BUTTON_RECHARGE_CANCEL = (LocString) "Cancel Recharge";
    }

    public class MINIONTODOSIDESCREEN
    {
      public static LocString NAME = (LocString) "Errands";
      public static LocString TOOLTIP = (LocString) "<b>Errands</b>\nView current and upcoming errands";
      public static LocString CURRENT_TITLE = (LocString) "Current Errand";
      public static LocString LIST_TITLE = (LocString) "Upcoming Errands";
      public static LocString CURRENT_SCHEDULE_BLOCK = (LocString) "CURRENT SHIFT: {0}";
      public static LocString CHORE_TARGET = (LocString) "{Target}";
      public static LocString CHORE_TARGET_AND_GROUP = (LocString) "{Target} -- {Groups}";
      public static LocString SELF_LABEL = (LocString) "Self";
      public static LocString TRUNCATED_CHORES = (LocString) "{0} more";
      public static LocString TOOLTIP_IDLE = (LocString) $"{{IdleDescription}}\n\nDuplicants will only <b>{{Errand}}</b> when there is nothing else for them to do\n\nTotal {UI.PRE_KEYWORD}Priority{UI.PST_KEYWORD}: {{TotalPriority}}\n    • {(string) UI.JOBSSCREEN.PRIORITY_CLASS.IDLE}: {{ClassPriority}}\n    • All {{BestGroup}} Errands: {{TypePriority}}";
      public static LocString TOOLTIP_NORMAL = (LocString) $"{{Description}}\n\nErrand Type: {{Groups}}\n\nTotal {UI.PRE_KEYWORD}Priority{UI.PST_KEYWORD}: {{TotalPriority}}\n    • {{Name}}'s {{BestGroup}} Priority: {{PersonalPriorityValue}} ({{PersonalPriority}})\n    • This {{Building}}'s Priority: {{BuildingPriority}}\n    • All {{BestGroup}} Errands: {{TypePriority}}";
      public static LocString TOOLTIP_PERSONAL = (LocString) $"{{Description}}\n\n<b>{{Errand}}</b> is a {(string) UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS} errand and so will be performed before all Regular errands\n\nTotal {UI.PRE_KEYWORD}Priority{UI.PST_KEYWORD}: {{TotalPriority}}\n    • {(string) UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS}: {{ClassPriority}}\n    • All {{BestGroup}} Errands: {{TypePriority}}";
      public static LocString TOOLTIP_EMERGENCY = (LocString) $"{{Description}}\n\n<b>{{Errand}}</b> is an {(string) UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY} errand and so will be performed before all Regular and Personal errands\n\nTotal {UI.PRE_KEYWORD}Priority{UI.PST_KEYWORD}: {{TotalPriority}}\n    • {(string) UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY} : {{ClassPriority}}\n    • This {{Building}}'s Priority: {{BuildingPriority}}\n    • All {{BestGroup}} Errands: {{TypePriority}}";
      public static LocString TOOLTIP_COMPULSORY = (LocString) $"{{Description}}\n\n<b>{{Errand}}</b> is a {(string) UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY} action and so will occur immediately\n\nTotal {UI.PRE_KEYWORD}Priority{UI.PST_KEYWORD}: {{TotalPriority}}\n    • {(string) UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY}: {{ClassPriority}}\n    • All {{BestGroup}} Errands: {{TypePriority}}";
      public static LocString TOOLTIP_DESC_ACTIVE = (LocString) "{Name}'s Current Errand: <b>{Errand}</b>";
      public static LocString TOOLTIP_DESC_INACTIVE = (LocString) "{Name} could work on <b>{Errand}</b>, but it's not their top priority right now";
      public static LocString TOOLTIP_IDLEDESC_ACTIVE = (LocString) "{Name} is currently <b>Idle</b>";
      public static LocString TOOLTIP_IDLEDESC_INACTIVE = (LocString) "{Name} could become <b>Idle</b> when all other errands are canceled or completed";
      public static LocString TOOLTIP_NA = (LocString) "--";
      public static LocString CHORE_GROUP_SEPARATOR = (LocString) " or ";
    }

    public class MODULEFLIGHTUTILITYSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Deployables";
      public static LocString SELECT_TARGET_BUTTON = (LocString) "Select Target";
      public static LocString SELECT_TARGET_BUTTON_TOOLTIP = (LocString) "Select this module's target on the starmap";
      public static LocString DEPLOY_BUTTON = (LocString) "Deploy";
      public static LocString DEPLOY_BUTTON_TOOLTIP = (LocString) $"Send this module's contents to the surface of the currently orbited {(string) UI.CLUSTERMAP.PLANETOID_KEYWORD}\n\nA specific deploy location may need to be chosen for certain modules";
      public static LocString REPEAT_BUTTON_TOOLTIP = (LocString) "Automatically deploy this module's contents when a destination orbit is reached";
      public static LocString SELECT_DUPLICANT = (LocString) "Select Duplicant";
      public static LocString PILOT_FMT = (LocString) "{0} - Pilot";
      public static LocString FIRE_BUTTON = (LocString) "Fire";
      public static LocString FIRE_BUTTON_TOOLTIP = (LocString) "Fire this module's contents at the selected target";
      public static LocString CLEAR_TARGET_BUTTON_TOOLTIP = (LocString) "Cancel target selection";
    }

    public class HIGHENERGYPARTICLEDIRECTIONSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Emitting Particle Direction";
      public static LocString SELECTED_DIRECTION = (LocString) "Selected direction: {0}";
      public static LocString DIRECTION_N = (LocString) "N";
      public static LocString DIRECTION_NE = (LocString) "NE";
      public static LocString DIRECTION_E = (LocString) "E";
      public static LocString DIRECTION_SE = (LocString) "SE";
      public static LocString DIRECTION_S = (LocString) "S";
      public static LocString DIRECTION_SW = (LocString) "SW";
      public static LocString DIRECTION_W = (LocString) "W";
      public static LocString DIRECTION_NW = (LocString) "NW";
    }

    public class MONUMENTSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Great Monument";
      public static LocString FLIP_FACING_BUTTON = (LocString) (UI.CLICK(UI.ClickType.CLICK) + " TO ROTATE");
    }

    public class PLANTERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "{0} Seeds";
      public static LocString INFORMATION = (LocString) nameof (INFORMATION);
      public static LocString AWAITINGREQUEST = (LocString) "PLANT: {0}";
      public static LocString AWAITINGDELIVERY = (LocString) "AWAITING DELIVERY: {0}";
      public static LocString AWAITINGREMOVAL = (LocString) "AWAITING DIGGING UP: {0}";
      public static LocString ENTITYDEPOSITED = (LocString) "PLANTED: {0}";
      public static LocString MUTATIONS_HEADER = (LocString) "Mutations";
      public static LocString DEPOSIT = (LocString) "Plant";
      public static LocString CANCELDEPOSIT = (LocString) "Cancel";
      public static LocString REMOVE = (LocString) "Uproot";
      public static LocString CANCELREMOVAL = (LocString) "Cancel";
      public static LocString SELECT_TITLE = (LocString) "SELECT";
      public static LocString SELECT_DESC = (LocString) "Select a seed to plant.";
      public static LocString LIFECYCLE = (LocString) "<b>Life Cycle</b>:";
      public static LocString PLANTREQUIREMENTS = (LocString) "<b>Growth Requirements</b>:";
      public static LocString PLANTEFFECTS = (LocString) "<b>Effects</b>:";
      public static LocString NUMBEROFHARVESTS = (LocString) "Harvests: {0}";
      public static LocString YIELD = (LocString) "{0}: {1} ";
      public static LocString YIELD_NONFOOD = (LocString) "{0}: {1} ";
      public static LocString YIELD_SINGLE = (LocString) "{0}";
      public static LocString YIELDPERHARVEST = (LocString) "{0} {1} per harvest";
      public static LocString TOTALHARVESTCALORIESWITHPERUNIT = (LocString) "{0} [{1} / unit]";
      public static LocString TOTALHARVESTCALORIES = (LocString) "{0}";
      public static LocString BONUS_SEEDS = (LocString) $"Base {UI.FormatAsLink("Seed", "PLANTS")} Harvest Chance: {{0}}";
      public static LocString YIELD_SEED = (LocString) "{1} {0}";
      public static LocString YIELD_SEED_SINGLE = (LocString) "{0}";
      public static LocString YIELD_SEED_FINAL_HARVEST = (LocString) "{1} {0} - Final harvest only";
      public static LocString YIELD_SEED_SINGLE_FINAL_HARVEST = (LocString) "{0} - Final harvest only";
      public static LocString ROTATION_NEED_FLOOR = (LocString) "<b>Requires upward plot orientation.</b>";
      public static LocString ROTATION_NEED_WALL = (LocString) "<b>Requires sideways plot orientation.</b>";
      public static LocString ROTATION_NEED_CEILING = (LocString) "<b>Requires downward plot orientation.</b>";
      public static LocString NO_SPECIES_SELECTED = (LocString) "Select a seed species above...";
      public static LocString DISEASE_DROPPER_BURST = (LocString) "{Disease} Burst: {DiseaseAmount}";
      public static LocString DISEASE_DROPPER_CONSTANT = (LocString) "{Disease}: {DiseaseAmount}";
      public static LocString DISEASE_ON_HARVEST = (LocString) "{Disease} on crop: {DiseaseAmount}";
      public static LocString AUTO_SELF_HARVEST = (LocString) "Self-Harvest On Grown";

      public class TOOLTIPS
      {
        public static LocString PLANTLIFECYCLE = (LocString) "Duration and number of harvests produced by this plant in a lifetime";
        public static LocString PLANTREQUIREMENTS = (LocString) "Minimum conditions for basic plant growth";
        public static LocString PLANTEFFECTS = (LocString) "Additional attributes of this plant";
        public static LocString YIELD = (LocString) $"{UI.FormatAsLink("{2}", "KCAL")} produced [{UI.FormatAsLink("{1}", "KCAL")} / unit]";
        public static LocString YIELD_NONFOOD = (LocString) "{0} produced per harvest";
        public static LocString NUMBEROFHARVESTS = (LocString) "This plant can mature {0} times before the end of its life cycle";
        public static LocString YIELD_SEED = (LocString) "Sow to grow more of this plant";
        public static LocString YIELD_SEED_FINAL_HARVEST = (LocString) "{0}\n\nProduced in the final harvest of the plant's life cycle";
        public static LocString BONUS_SEEDS = (LocString) "This plant has a {0} chance to produce new seeds when harvested";
        public static LocString DISEASE_DROPPER_BURST = (LocString) "At certain points in this plant's lifecycle, it will emit a burst of {DiseaseAmount} {Disease}.";
        public static LocString DISEASE_DROPPER_CONSTANT = (LocString) "This plant emits {DiseaseAmount} {Disease} while it is alive.";
        public static LocString DISEASE_ON_HARVEST = (LocString) "The {Crop} produced by this plant will have {DiseaseAmount} {Disease} on it.";
        public static LocString AUTO_SELF_HARVEST = (LocString) "This plant will instantly drop its crop and begin regrowing when it is matured.";
        public static LocString PLANT_TOGGLE_TOOLTIP = (LocString) "{0}\n\n{1}\n\n<b>{2}</b> seeds available";
      }
    }

    public class EGGINCUBATOR
    {
      public static LocString TITLE = (LocString) "Critter Eggs";
      public static LocString AWAITINGREQUEST = (LocString) "INCUBATE: {0}";
      public static LocString AWAITINGDELIVERY = (LocString) "AWAITING DELIVERY: {0}";
      public static LocString AWAITINGREMOVAL = (LocString) "AWAITING REMOVAL: {0}";
      public static LocString ENTITYDEPOSITED = (LocString) "INCUBATING: {0}";
      public static LocString DEPOSIT = (LocString) "Incubate";
      public static LocString CANCELDEPOSIT = (LocString) "Cancel";
      public static LocString REMOVE = (LocString) "Remove";
      public static LocString CANCELREMOVAL = (LocString) "Cancel";
      public static LocString SELECT_TITLE = (LocString) "SELECT";
      public static LocString SELECT_DESC = (LocString) "Select an egg to incubate.";
    }

    public class BASICRECEPTACLE
    {
      public static LocString TITLE = (LocString) "Displayed Object";
      public static LocString AWAITINGREQUEST = (LocString) "SELECT: {0}";
      public static LocString AWAITINGDELIVERY = (LocString) "AWAITING DELIVERY: {0}";
      public static LocString AWAITINGREMOVAL = (LocString) "AWAITING REMOVAL: {0}";
      public static LocString ENTITYDEPOSITED = (LocString) "DISPLAYING: {0}";
      public static LocString DEPOSIT = (LocString) "Select";
      public static LocString CANCELDEPOSIT = (LocString) "Cancel";
      public static LocString REMOVE = (LocString) "Remove";
      public static LocString CANCELREMOVAL = (LocString) "Cancel";
      public static LocString SELECT_TITLE = (LocString) "SELECT OBJECT";
      public static LocString SELECT_DESC = (LocString) "Select an object to display here.";
    }

    public class SPECIALCARGOBAYCLUSTER
    {
      public static LocString TITLE = (LocString) "Target Critter";
      public static LocString AWAITINGREQUEST = (LocString) "SELECT: {0}";
      public static LocString AWAITINGDELIVERY = (LocString) "AWAITING DELIVERY: {0}";
      public static LocString AWAITINGREMOVAL = (LocString) "AWAITING REMOVAL: {0}";
      public static LocString ENTITYDEPOSITED = (LocString) "CONTENTS: {0}";
      public static LocString DEPOSIT = (LocString) "Select";
      public static LocString CANCELDEPOSIT = (LocString) "Cancel";
      public static LocString REMOVE = (LocString) "Remove";
      public static LocString CANCELREMOVAL = (LocString) "Cancel";
      public static LocString SELECT_TITLE = (LocString) "SELECT CRITTER";
      public static LocString SELECT_DESC = (LocString) "Select a critter to store in this module.";
    }

    public class LURE
    {
      public static LocString TITLE = (LocString) "Select Bait";
      public static LocString INFORMATION = (LocString) nameof (INFORMATION);
      public static LocString AWAITINGREQUEST = (LocString) "PLANT: {0}";
      public static LocString AWAITINGDELIVERY = (LocString) "AWAITING DELIVERY: {0}";
      public static LocString AWAITINGREMOVAL = (LocString) "AWAITING DIGGING UP: {0}";
      public static LocString ENTITYDEPOSITED = (LocString) "PLANTED: {0}";
      public static LocString ATTRACTS = (LocString) "Attract {1}s";
    }

    public class ROLESTATION
    {
      public static LocString TITLE = (LocString) "Duplicant Skills";
      public static LocString OPENROLESBUTTON = (LocString) "SKILLS";
    }

    public class RESEARCHSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Select Research";
      public static LocString CURRENTLYRESEARCHING = (LocString) "Currently Researching";
      public static LocString NOSELECTEDRESEARCH = (LocString) "No Research selected";
      public static LocString OPENRESEARCHBUTTON = (LocString) "RESEARCH";
    }

    public class REFINERYSIDESCREEN
    {
      public static LocString RECIPE_FROM_TO = (LocString) "{0} to {1}";
      public static LocString RECIPE_WITH = (LocString) "{1} ({0})";
      public static LocString RECIPE_FROM_TO_WITH_NEWLINES = (LocString) "{0}\nto\n{1}";
      public static LocString RECIPE_FROM_TO_COMPOSITE = (LocString) "{0} to {1} and {2}";
      public static LocString RECIPE_FROM_TO_HEP = (LocString) $"{{0}} to {UI.FormatAsLink("Radbolts", "RADIATION")} and {{1}}";
      public static LocString RECIPE_SIMPLE_INCLUDE_AMOUNTS = (LocString) "{0} {1}";
      public static LocString RECIPE_FROM_TO_INCLUDE_AMOUNTS = (LocString) "{2} {0} to {3} {1}";
      public static LocString RECIPE_WITH_INCLUDE_AMOUNTS = (LocString) "{3} {1} ({2} {0})";
      public static LocString RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS = (LocString) "{3} {0} to {4} {1} and {5} {2}";
      public static LocString RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS = (LocString) $"{{2}} {{0}} to {{3}} {UI.FormatAsLink("Radbolts", "RADIATION")} and {{4}} {{1}}";
    }

    public class SEALEDDOORSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Sealed Door";
      public static LocString LABEL = (LocString) "This door requires a sample to unlock.";
      public static LocString BUTTON = (LocString) "SUBMIT BIOSCAN";
      public static LocString AWAITINGBUTTON = (LocString) "AWAITING BIOSCAN";
    }

    public class ENCRYPTEDLORESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Encrypted File";
      public static LocString LABEL = (LocString) "This computer contains encrypted files.";
      public static LocString BUTTON = (LocString) "ATTEMPT DECRYPTION";
      public static LocString AWAITINGBUTTON = (LocString) "AWAITING DECRYPTION";
    }

    public class ACCESS_CONTROL_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Door Access Control";
      public static LocString DOOR_DEFAULT = (LocString) "Default";
      public static LocString MINION_ACCESS = (LocString) "Access Permissions";
      public static LocString MINION_SELECT_TOOLTIP = (LocString) $"{UI.CLICK(UI.ClickType.Click)} to toggle between default and custom {UI.PRE_KEYWORD}Access Permissions{UI.PST_KEYWORD} for this Duplicant\n\n{UI.PRE_KEYWORD}Access Permissions{UI.PST_KEYWORD} apply only when door is set to auto";
      public static LocString GO_LEFT_ENABLED = (LocString) $"Passing Left through this door is permitted\n\n{UI.CLICK(UI.ClickType.Click)} to revoke permission";
      public static LocString GO_LEFT_DISABLED = (LocString) $"Passing Left through this door is not permitted\n\n{UI.CLICK(UI.ClickType.Click)} to grant permission";
      public static LocString GO_RIGHT_ENABLED = (LocString) $"Passing Right through this door is permitted\n\n{UI.CLICK(UI.ClickType.Click)} to revoke permission";
      public static LocString GO_RIGHT_DISABLED = (LocString) $"Passing Right through this door is not permitted\n\n{UI.CLICK(UI.ClickType.Click)} to grant permission";
      public static LocString GO_UP_ENABLED = (LocString) $"Passing Up through this door is permitted\n\n{UI.CLICK(UI.ClickType.Click)} to revoke permission";
      public static LocString GO_UP_DISABLED = (LocString) $"Passing Up through this door is not permitted\n\n{UI.CLICK(UI.ClickType.Click)} to grant permission";
      public static LocString GO_DOWN_ENABLED = (LocString) $"Passing Down through this door is permitted\n\n{UI.CLICK(UI.ClickType.Click)} to revoke permission";
      public static LocString GO_DOWN_DISABLED = (LocString) $"Passing Down through this door is not permitted\n\n{UI.CLICK(UI.ClickType.Click)} to grant permission";
      public static LocString SET_TO_DEFAULT = (LocString) (UI.CLICK(UI.ClickType.Click) + " to clear custom permissions");
      public static LocString SET_TO_CUSTOM = (LocString) (UI.CLICK(UI.ClickType.Click) + " to assign custom permissions");
      public static LocString USING_DEFAULT = (LocString) "Default Access";
      public static LocString USING_CUSTOM = (LocString) "Custom Access";
      public static LocString EMPTY_CATEGORY = (LocString) "None available";
      public static LocString CATEGORY_HEADER_TOOLTIP = (LocString) $"Use the arrows to set default {UI.PRE_KEYWORD}Access Permissions{UI.PST_KEYWORD} for all entities in this category\n\n{UI.CLICK(UI.ClickType.Click)} to collapse or expand this category";
    }

    public class OWNABLESSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Equipment and Amenities";
      public static LocString NO_ITEM_ASSIGNED = (LocString) "Assign";
      public static LocString NO_ITEM_FOUND = (LocString) "None found";
      public static LocString NO_APPLICABLE = (LocString) "{0}: Ineligible";

      public static class TOOLTIPS
      {
        public static LocString NO_APPLICABLE = (LocString) $"This Duplicant cannot be assigned {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD}";
        public static LocString NO_ITEM_ASSIGNED = (LocString) $"Click to view and assign existing {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} to this Duplicant";
        public static LocString ITEM_ASSIGNED_GENERIC = (LocString) "This Duplicant has {0} assigned to them";
        public static LocString ITEM_ASSIGNED = (LocString) "{0}\n\n{1}";
      }

      public class CATEGORIES
      {
        public static LocString SUITS = (LocString) "Suits";
        public static LocString AMENITIES = (LocString) "Amenities";
      }
    }

    public class OWNABLESSECONDSIDESCREEN
    {
      public static LocString TITLE = (LocString) "{0}";
      public static LocString NONE_ROW_LABEL = (LocString) "Clear";
      public static LocString NONE_ROW_TOOLTIP = (LocString) "Click to remove any item currently assigned to the selected slot";
      public static LocString ASSIGNED_TO_OTHER_STATUS = (LocString) "Assigned to: {0}";
      public static LocString ASSIGNED_TO_SELF_STATUS = (LocString) "Assigned";
      public static LocString NOT_ASSIGNED = (LocString) "Unassigned";
    }

    public class ASSIGNABLESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Assign {0}";
      public static LocString ASSIGNED = (LocString) "Assigned";
      public static LocString UNASSIGNED = (LocString) "-";
      public static LocString DISABLED = (LocString) "Ineligible";
      public static LocString SORT_BY_DUPLICANT = (LocString) "Duplicant";
      public static LocString SORT_BY_ASSIGNMENT = (LocString) "Assignment";
      public static LocString ASSIGN_TO_TOOLTIP = (LocString) "Assign to {0}";
      public static LocString UNASSIGN_TOOLTIP = (LocString) "Assigned to {0}";
      public static LocString DISABLED_TOOLTIP = (LocString) "{0} is ineligible for this skill assignment";
      public static LocString PUBLIC = (LocString) "Public";
    }

    public class COMETDETECTORSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Space Scanner";
      public static LocString HEADER = (LocString) "Sends automation signal when selected object is detected";
      public static LocString ASSIGNED = (LocString) "Assigned";
      public static LocString UNASSIGNED = (LocString) "-";
      public static LocString DISABLED = (LocString) "Ineligible";
      public static LocString SORT_BY_DUPLICANT = (LocString) "Duplicant";
      public static LocString SORT_BY_ASSIGNMENT = (LocString) "Assignment";
      public static LocString ASSIGN_TO_TOOLTIP = (LocString) "Scanning for {0}";
      public static LocString UNASSIGN_TOOLTIP = (LocString) "Scanning for {0}";
      public static LocString NOTHING = (LocString) "Nothing";
      public static LocString COMETS = (LocString) "Meteor Showers";
      public static LocString ROCKETS = (LocString) "Rocket Landing Ping";
      public static LocString DUPEMADE = (LocString) "Interplanetary Payloads";
    }

    public class GEOTUNERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Select Geyser";
      public static LocString DESCRIPTION = (LocString) "Select an analyzed geyser to transmit amplification data to.";
      public static LocString NOTHING = (LocString) "No geyser selected";
      public static LocString UNSTUDIED_TOOLTIP = (LocString) "This geyser must be analyzed before it can be selected\n\nDouble-click to view this geyser";
      public static LocString STUDIED_TOOLTIP = (LocString) $"Increase this geyser's {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} and output";
      public static LocString GEOTUNER_LIMIT_TOOLTIP = (LocString) $"This geyser cannot be targeted by more {UI.PRE_KEYWORD}Geotuners{UI.PST_KEYWORD}";
      public static LocString STUDIED_TOOLTIP_MATERIAL = (LocString) "Required resource: {MATERIAL}";
      public static LocString STUDIED_TOOLTIP_POTENTIAL_OUTPUT = (LocString) "Potential Output {POTENTIAL_OUTPUT}";
      public static LocString STUDIED_TOOLTIP_BASE_TEMP = (LocString) "Base {BASE}";
      public static LocString STUDIED_TOOLTIP_VISIT_GEYSER = (LocString) "Double-click to view this geyser";
      public static LocString STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE = (LocString) "Geotuned ";
      public static LocString STUDIED_TOOLTIP_NUMBER_HOVERED = (LocString) "This geyser is targeted by {0} Geotuners";
    }

    public class REMOTE_WORK_TERMINAL_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Dock Assignment";
      public static LocString DESCRIPTION = (LocString) "Select a remote worker dock for this controller to target.";
      public static LocString NOTHING_SELECTED = (LocString) "None";
      public static LocString DOCK_TOOLTIP = (LocString) "Click to assign this dock to this controller\n\nDouble-click to view this dock";
    }

    public class COMMAND_MODULE_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Launch Conditions";
      public static LocString DESTINATION_BUTTON = (LocString) "Show Starmap";
      public static LocString DESTINATION_BUTTON_EXPANSION = (LocString) "Show Starmap";
    }

    public class CLUSTERDESTINATIONSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Destination";
      public static LocString DESTINATION_LABEL = (LocString) "Destination: {0}";
      public static LocString DESTINATION_ROUNTRIP_LABEL = (LocString) "Pending Roundtrip to: {0}";
      public static LocString DESTINATION_LABEL_SELECTING = (LocString) "Selecting new destination...";
      public static LocString DESTINATION_LABEL_INVALID = (LocString) "None selected";
      public static LocString CHANGE_DESTINATION_BUTTON = (LocString) "Select Destination";
      public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP = (LocString) "Select a new destination for this rocket";
      public static LocString CHANGE_DESTINATION_BUTTON_SELECTING_TOOLTIP = (LocString) $"Select a destination by clicking on the desired {UI.FormatAsManagementMenu("Starmap", Action.ManageStarmap)} hex";
      public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP_MISSILE = (LocString) "Select a new target for this projectile launcher";
      public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP_RAILGUN = (LocString) "Select a new target for this payload launcher";
      public static LocString CLEAR_DESTINATION_BUTTON = (LocString) "Clear";
      public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP = (LocString) "Clear this rocket's selected destination";
      public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP_MISSILE = (LocString) "Clear this projectile launcher's selected target";
      public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP_RAILGUN = (LocString) "Clear this payload launcher's selected target";
      public static LocString LANDING_PLATFORM_LABEL = (LocString) "Landing Site: {0}";
      public static LocString FIRSTAVAILABLE = (LocString) ("Any " + (string) BUILDINGS.PREFABS.LAUNCHPAD.NAME);
      public static LocString DROPDOWN_TOOLTIP_VALID_SITE = (LocString) "Land at {0} when the site is clear";
      public static LocString DROPDOWN_TOOLTIP_FIRST_AVAILABLE = (LocString) "Select the first available landing site";
      public static LocString DROPDOWN_TOOLTIP_TOO_SHORT = (LocString) "This rocket's height exceeds the space available in this landing site";
      public static LocString DROPDOWN_TOOLTIP_PATH_OBSTRUCTED = (LocString) "Landing path obstructed";
      public static LocString DROPDOWN_TOOLTIP_SITE_OBSTRUCTED = (LocString) "Landing position on the platform is obstructed";
      public static LocString DROPDOWN_TOOLTIP_PAD_DISABLED = (LocString) ((string) BUILDINGS.PREFABS.LAUNCHPAD.NAME + " is disabled");
      public static LocString ROUNDTRIP_LABEL_ONE_WAY = (LocString) "Flight Plan: One Way";
      public static LocString ROUNDTRIP_LABEL_ROUNDTRIP = (LocString) "Flight Plan: Round Trip";
      public static LocString ROUNDTRIP_BUTTON_ROUNDTRIP = (LocString) "Set Round Trip";
      public static LocString ROUNDTRIP_BUTTON_ONE_WAY = (LocString) "Set One Way Trip";
      public static LocString ROUNDTRIP_BUTTON_TOOLTIP_ROUNDTRIP = (LocString) "Set rocket to travel to its destination and back";
      public static LocString ROUNDTRIP_BUTTON_TOOLTIP_ONE_WAY = (LocString) "Set rocket to travel to its destination and remain there";
      public static LocString TITLE_MISSILE_TARGET = (LocString) "Long Range Target";
      public static LocString NONEAVAILABLE = (LocString) "No landing site";
      public static LocString NO_TALL_SITES_AVAILABLE = (LocString) "No landing sites fit the height of this rocket";

      public class ASSIGNMENTSTATUS
      {
        public static LocString LOCAL = (LocString) "Current";
        public static LocString DESTINATION = (LocString) "Destination";
      }
    }

    public class EQUIPPABLESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Equip {0}";
      public static LocString ASSIGNEDTO = (LocString) "Assigned to: {Assignee}";
      public static LocString UNASSIGNED = (LocString) "Unassigned";
      public static LocString GENERAL_CURRENTASSIGNED = (LocString) "(Owner)";
    }

    public class EQUIPPABLE_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Assign To Duplicant";
      public static LocString CURRENTLY_EQUIPPED = (LocString) "Currently Equipped:\n{0}";
      public static LocString NONE_EQUIPPED = (LocString) "None";
      public static LocString EQUIP_BUTTON = (LocString) "Equip";
      public static LocString DROP_BUTTON = (LocString) "Drop";
      public static LocString SWAP_BUTTON = (LocString) "Swap";
    }

    public class TELEPADSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Printables";
      public static LocString NEXTPRODUCTION = (LocString) "Next Print: {0}";
      public static LocString GAMEOVER = (LocString) "Colony Lost";
      public static LocString VICTORY_CONDITIONS = (LocString) "Hardwired Imperatives";
      public static LocString SUMMARY_TITLE = (LocString) "Management";
      public static LocString SUMMARY_BUTTON = (LocString) "Colony Summary";
      public static LocString SKILLS_TITLE = (LocString) "Skills";
      public static LocString SKILLS_BUTTON = (LocString) "Duplicant Skills";
      public static LocString RESEARCH_BUTTON = (LocString) "Research";
    }

    public class VALVESIDESCREEN
    {
      public static LocString TITLE = (LocString) "Flow Control";
    }

    public class BIONIC_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Boosters";
      public static LocString UPGRADE_SLOT_LOCKED = (LocString) "N/A";
      public static LocString UPGRADE_SLOT_EMPTY = (LocString) "Empty";
      public static LocString UPGRADE_SLOT_ASSIGNED = (LocString) "Assigned";
      public static LocString UPGRADE_SLOT_INSTALLED = (LocString) "Installed";
      public static LocString CURRENT_WATTAGE_LABEL = (LocString) "Current Wattage: <b>{0}</b>";
      public static LocString CURRENT_WATTAGE_LABEL_BATTERY_SAVE_MODE = (LocString) "Current Wattage: <color=#0303fc><b>{0}</b> {1}</color>";
      public static LocString CURRENT_WATTAGE_LABEL_OFFLINE = (LocString) "Current Wattage: <color=#GG2222>Offline {0}</color>";
      public const string OFFLINE_MODE_COLOR = "<color=#GG2222>";
      public const string BATTERY_SAVE_MODE_COLOR = "<color=#0303fc>";
      public const string COLOR_END = "</color>";

      public class TOOLTIP
      {
        public static LocString CURRENT_WATTAGE = (LocString) "Wattage is the amount of energy that this Duplicant's bionic parts consume per second\n\nInstalled boosters consume wattage while active";
        public static LocString SLOT_LOCKED = (LocString) $"This booster slot is unavailable\n\nBooster slots can be unlocked using {UI.PRE_KEYWORD}Skill Points{UI.PST_KEYWORD}";
        public static LocString SLOT_EMPTY = (LocString) "No booster installed\n\nClick to view available boosters";
        public static LocString SLOT_ASSIGNED = (LocString) $"This {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} will be installed when it is within this Duplicant's reach";
        public static LocString SLOT_INSTALLED = (LocString) "{0}";
      }

      public class BOOSTER_ASSIGNMENT
      {
        public static LocString NOT_ALREADY_ASSIGNED = (LocString) "{0} does not currently have this type of booster assigned";
        public static LocString ALREADY_ASSIGNED = (LocString) "{0} currently has <b>{1} of this type</b> of booster assigned";
        public static LocString AVAILABLE_SLOTS = (LocString) "{0} has <b>{1}/{2}</b> booster slots assigned";
        public static LocString NO_AVAILABLE_SLOTS = (LocString) $"{UI.YELLOW_PREFIX}All of {{0}}'s booster slots are currently assigned: <b>{{1}}/{{2}}</b>{UI.COLOR_SUFFIX}";
        public static LocString HEADER_PERKS = (LocString) "<b>Enables:</b>";
        public static LocString HEADER_ATTRIBUTES = (LocString) "<b>Modifiers:</b>";
      }
    }

    public class LIMIT_VALVE_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Meter Control";
      public static LocString AMOUNT = (LocString) "Amount: {0}";
      public static LocString LIMIT = (LocString) "Limit:";
      public static LocString RESET_BUTTON = (LocString) "Reset Amount";
      public static LocString SLIDER_TOOLTIP_UNITS = (LocString) "The amount of Units or Mass passing through the sensor.";
    }

    public class NUCLEAR_REACTOR_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Reaction Mass Target";
      public static LocString TOOLTIP = (LocString) $"Duplicants will attempt to keep the reactor supplied with {UI.PRE_KEYWORD}{{0}}{{1}}{UI.PST_KEYWORD} of {UI.PRE_KEYWORD}{{2}}{UI.PST_KEYWORD}";
    }

    public class MANUALGENERATORSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Battery Recharge Threshold";
      public static LocString CURRENT_THRESHOLD = (LocString) "Current Threshold: {0}%";
      public static LocString TOOLTIP = (LocString) $"Duplicants will be requested to operate this generator when the total charge of the connected {UI.PRE_KEYWORD}Batteries{UI.PST_KEYWORD} falls below <b>{{0}}%</b>";
    }

    public class PRINTERCEPTORSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Inventory";
      public static LocString BUTTON_PRINT = (LocString) "Choose a Blueprint";
      public static LocString BUTTON_INTERCEPTOR = (LocString) "Intercept Charge";
      public static LocString INTERCEPT_METER = (LocString) "{0}/{1} Stored Charges";
      public static LocString INTERCEPT_TOOLTIP = (LocString) ("Click to intercept a charge from the " + (string) BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME);
      public static LocString INTERCEPT_TOOLTIP_DISABLED = (LocString) $"There is no {(string) BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME} charge to intercept";
      public static LocString INTERCEPT_TOOLTIP_DISABLED_TOO_FULL = (LocString) "This building is fully charged\n\nStored charges must be consumed before additional charges can be intercepted";
      public static LocString PRINT_TOOLTIP = (LocString) $"Click to view printable {UI.PRE_KEYWORD}Eggs{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Seeds{UI.PST_KEYWORD}";
      public static LocString PRINT_TOOLTIP_DISABLED = (LocString) "Insufficient stored materials";
      public static LocString DATABANK_COUNT = (LocString) "{0} Stored Data Banks";
      public static LocString LOCKED_LABEL = (LocString) "Access Code Required";
      public static LocString ACTIVATE_TOILET_BUTTON = (LocString) "Unclog";
      public static LocString ACTIVATE_TOILET_BUTTON_TOOLTIP = (LocString) "Send a Duplicant to plunge the depths of this building";
      public static LocString ACTIVATE_TOILET_BUTTON_CANCEL = (LocString) "Cancel Unclog";
      public static LocString ACTIVATE_TOILET_BUTTON_CANCEL_TOOLTIP = (LocString) "Cancel plunging this toilet";
    }

    public class SPACEHEATERSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Power Consumption";
      public static LocString CURRENT_THRESHOLD = (LocString) "Current Power Consumption: {0}";
      public static LocString TOOLTIP = (LocString) "Adjust power consumption to determine how much heat is produced\n\nCurrent heat production: <b>{0}</b>";
    }

    public class MANUALDELIVERYGENERATORSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Fuel Request Threshold";
      public static LocString CURRENT_THRESHOLD = (LocString) "Current Threshold: {0}%";
      public static LocString TOOLTIP = (LocString) $"Duplicants will be requested to deliver {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} when the total charge of the connected {UI.PRE_KEYWORD}Batteries{UI.PST_KEYWORD} falls below <b>{{1}}%</b>";
    }

    public class TIME_OF_DAY_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Time-of-Day Sensor";
      public static LocString TOOLTIP = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} after the selected Turn On time, and a {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} after the selected Turn Off time";
      public static LocString START = (LocString) "Turn On";
      public static LocString STOP = (LocString) "Turn Off";
    }

    public class CRITTER_COUNT_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Critter Count Sensor";
      public static LocString TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if there are more than <b>{{0}}</b> {UI.PRE_KEYWORD}Critters{UI.PST_KEYWORD} or {UI.PRE_KEYWORD}Eggs{UI.PST_KEYWORD} in the room";
      public static LocString TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if there are fewer than <b>{{0}}</b> {UI.PRE_KEYWORD}Critters{UI.PST_KEYWORD} or {UI.PRE_KEYWORD}Eggs{UI.PST_KEYWORD} in the room";
      public static LocString START = (LocString) "Turn On";
      public static LocString STOP = (LocString) "Turn Off";
      public static LocString VALUE_NAME = (LocString) "Count";
    }

    public class OIL_WELL_CAP_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Backpressure Release Threshold";
      public static LocString TOOLTIP = (LocString) "Duplicants will be requested to release backpressure buildup when it exceeds <b>{0}%</b>";
    }

    public class MODULAR_CONDUIT_PORT_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Pump Control";
      public static LocString LABEL_UNLOAD = (LocString) "Unload Only";
      public static LocString LABEL_BOTH = (LocString) "Load/Unload";
      public static LocString LABEL_LOAD = (LocString) "Load Only";
      public static readonly List<LocString> LABELS = new List<LocString>()
      {
        UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_UNLOAD,
        UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_BOTH,
        UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_LOAD
      };
      public static LocString TOOLTIP_UNLOAD = (LocString) "This pump will attempt to <b>Unload</b> cargo from the landed rocket, but not attempt to load new cargo";
      public static LocString TOOLTIP_BOTH = (LocString) "This pump will both <b>Load</b> and <b>Unload</b> cargo from the landed rocket";
      public static LocString TOOLTIP_LOAD = (LocString) "This pump will attempt to <b>Load</b> cargo onto the landed rocket, but will not unload it";
      public static readonly List<LocString> TOOLTIPS = new List<LocString>()
      {
        UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_UNLOAD,
        UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_BOTH,
        UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_LOAD
      };
      public static LocString DESCRIPTION = (LocString) "";
    }

    public class LOGIC_BUFFER_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Buffer Time";
      public static LocString TOOLTIP = (LocString) $"Will continue to send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} for <b>{{0}} seconds</b> after receiving a {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)}";
    }

    public class LOGIC_FILTER_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Filter Time";
      public static LocString TOOLTIP = (LocString) $"Will only send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if it receives {UI.FormatAsAutomationState("Green", UI.AutomationState.Active)} for longer than <b>{{0}} seconds</b>";
    }

    public class TIME_RANGE_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Time Schedule";
      public static LocString ON = (LocString) "Activation Time";
      public static LocString ON_TOOLTIP = (LocString) $"Activation time determines the time of day this sensor should begin sending a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)}\n\nThis sensor sends a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} {{0}} through the day";
      public static LocString DURATION = (LocString) "Active Duration";
      public static LocString DURATION_TOOLTIP = (LocString) $"Active duration determines what percentage of the day this sensor will spend sending a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)}\n\nThis sensor will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} for {{0}} of the day";
    }

    public class TIMER_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Timer";
      public static LocString ON = (LocString) "Green Duration";
      public static LocString GREEN_DURATION_TOOLTIP = (LocString) $"Green duration determines the amount of time this sensor should send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)}\n\nThis sensor sends a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} for {{0}}";
      public static LocString OFF = (LocString) "Red Duration";
      public static LocString RED_DURATION_TOOLTIP = (LocString) $"Red duration determines the amount of time this sensor should send a {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)}\n\nThis sensor will send a {UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby)} for {{0}}";
      public static LocString CURRENT_TIME = (LocString) "{0}/{1}";
      public static LocString MODE_LABEL_SECONDS = (LocString) "Mode: Seconds";
      public static LocString MODE_LABEL_CYCLES = (LocString) "Mode: Cycles";
      public static LocString RESET_BUTTON = (LocString) "Reset Timer";
      public static LocString DISABLED = (LocString) "Timer Disabled";
    }

    public class COUNTER_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Counter";
      public static LocString RESET_BUTTON = (LocString) "Reset Counter";
      public static LocString DESCRIPTION = (LocString) $"Send {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} when count is reached:";
      public static LocString INCREMENT_MODE = (LocString) "Mode: Increment";
      public static LocString DECREMENT_MODE = (LocString) "Mode: Decrement";
      public static LocString ADVANCED_MODE = (LocString) "Advanced Mode";
      public static LocString CURRENT_COUNT_SIMPLE = (LocString) "{0} of ";
      public static LocString CURRENT_COUNT_ADVANCED = (LocString) "{0} % ";

      public class TOOLTIPS
      {
        public static LocString ADVANCED_MODE = (LocString) $"In Advanced Mode, the {(string) BUILDINGS.PREFABS.LOGICCOUNTER.NAME} will count from <b>0</b> rather than <b>1</b>. It will reset when the max is reached, and send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} as a brief pulse rather than continuously.";
      }
    }

    public class PILOT_AND_CREW_SIDESCREEN
    {
      public static LocString TITLE = (LocString) "Crew";
      public static LocString INFO_LABEL = (LocString) "Assigned Pilot: {0}";
      public static LocString INFO_LABEL_ROBOT_ONLY = (LocString) "Assigned Pilot: Robo-Pilot";
      public static LocString NO_ASSIGNED_NAME = (LocString) "None";
      public static LocString EDIT_CREW_BUTTON_LABEL = (LocString) "Select Crew";
      public static LocString EDIT_CREW_BUTTON_TOOLTIP = (LocString) "Assign Duplicants to crew this rocket";
      public static LocString EDIT_CREW_BUTTON_DISABLED_TOOLTIP = (LocString) $"Rockets must include a {UI.PRE_KEYWORD}Spacefarer{UI.PST_KEYWORD} module in order for Duplicant crew to be assigned\n\nRockets with a {UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} module can launch without a Duplicant crew";
    }

    public class SUMMON_CREW_SIDESCREEN
    {
      public static LocString TITLE = (LocString) "Access Permissions";
      public static LocString INFO_LABEL_NO_CREW_NEEDED = (LocString) "No crew required";
      public static LocString INFO_LABEL_NO_CREW_FOUND = (LocString) "No crew assigned";
      public static LocString INFO_LABEL_PUBLIC_ACCESS = (LocString) "Access: Public";
      public static LocString INFO_LABEL_AWAITING_CREW = (LocString) "Crew Boarded: {0} of {1}";
      public static LocString INFO_LABEL_CREW_READY = (LocString) "All crew members have boarded";
      public static LocString INFO_LABEL_TOOLTIP_NO_CREW_NEEDED = (LocString) $"This rocket is piloted by a {UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} module and does not require a Duplicant crew";
      public static LocString INFO_LABEL_TOOLTIP_NO_CREW_FOUND = (LocString) $"This rocket must be piloted by a Duplicant with the {UI.PRE_KEYWORD}Rocket Piloting{UI.PST_KEYWORD} skill\n\nRockets with a {UI.PRE_KEYWORD}Robo-Pilot{UI.PST_KEYWORD} module do not require a Duplicant pilot";
      public static LocString INFO_LABEL_TOOLTIP_PUBLIC_ACCESS = (LocString) "All Duplicants can access this rocket regardless of crew assignment\n\nRockets cannot launch in this mode";
      public static LocString INFO_LABEL_TOOLTIP_AWAITING_CREW = (LocString) "Crew boarding in progress\n\nNon-crew will disembark and can no longer board this rocket\n\nRockets cannot launch until all crew members have boarded";
      public static LocString INFO_LABEL_TOOLTIP_CREW_READY = (LocString) $"All crew members have boarded and are ready for this rocket to launch\n\nPress the {UI.PRE_KEYWORD}Begin Launch Sequence{UI.PST_KEYWORD} button to launch this rocket";
      public static LocString SUMMON_CREW_BUTTON_LABEL = (LocString) "Summon Crew";
      public static LocString SUMMON_CREW_BUTTON_TOOLTIP = (LocString) "Summon crew members to board this rocket in preparation for launch\n\nNon-crew will disembark";
      public static LocString CANCEL_BUTTON_LABEL = (LocString) "Set to Public Access";
      public static LocString CANCEL_BUTTON_TOOLTIP = (LocString) "Enable all Duplicants to access this rocket regardless of crew assignment\n\nRockets cannot launch in this mode";
    }

    public class TIMEDSWITCHSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Time Schedule";
      public static LocString ONTIME = (LocString) "On Time:";
      public static LocString OFFTIME = (LocString) "Off Time:";
      public static LocString TIMETODEACTIVATE = (LocString) "Time until deactivation: {0}";
      public static LocString TIMETOACTIVATE = (LocString) "Time until activation: {0}";
      public static LocString WARNING = (LocString) $"Switch must be connected to a {UI.FormatAsLink("Power", "POWER")} grid";
      public static LocString CURRENTSTATE = (LocString) "Current State:";
      public static LocString ON = (LocString) "On";
      public static LocString OFF = (LocString) "Off";
    }

    public class CAPTURE_POINT_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Stable Management";
      public static LocString AUTOWRANGLE = (LocString) "Auto-Wrangle Surplus";
      public static LocString AUTOWRANGLE_TOOLTIP = (LocString) $"A Duplicant will automatically wrangle any critters that exceed the population limit or that do not belong in this stable\n\nDuplicants must possess the {UI.PRE_KEYWORD}Critter Ranching{UI.PST_KEYWORD} skill in order to wrangle critters";
      public static LocString LIMIT_TOOLTIP = (LocString) "Critters exceeding this population limit will automatically be wrangled:";
      public static LocString UNITS_SUFFIX = (LocString) " Critters";
    }

    public class TEMPERATURESWITCHSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Temperature Threshold";
      public static LocString CURRENT_TEMPERATURE = (LocString) "Current Temperature:\n{0}";
      public static LocString ACTIVATE_IF = (LocString) $"Send {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if:";
      public static LocString COLDER_BUTTON = (LocString) "Below";
      public static LocString WARMER_BUTTON = (LocString) "Above";
    }

    public class BRIGHTNESSSWITCHSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Brightness Threshold";
      public static LocString CURRENT_TEMPERATURE = (LocString) "Current Brightness:\n{0}";
      public static LocString ACTIVATE_IF = (LocString) $"Send {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if:";
      public static LocString COLDER_BUTTON = (LocString) "Below";
      public static LocString WARMER_BUTTON = (LocString) "Above";
    }

    public class RADIATIONSWITCHSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Radiation Threshold";
      public static LocString CURRENT_TEMPERATURE = (LocString) "Current Radiation:\n{0}/cycle";
      public static LocString ACTIVATE_IF = (LocString) $"Send {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if:";
      public static LocString COLDER_BUTTON = (LocString) "Below";
      public static LocString WARMER_BUTTON = (LocString) "Above";
    }

    public class WATTAGESWITCHSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Wattage Threshold";
      public static LocString CURRENT_TEMPERATURE = (LocString) "Current Wattage:\n{0}";
      public static LocString ACTIVATE_IF = (LocString) $"Send {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if:";
      public static LocString COLDER_BUTTON = (LocString) "Below";
      public static LocString WARMER_BUTTON = (LocString) "Above";
    }

    public class HEPSWITCHSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Radbolt Threshold";
    }

    public class THRESHOLD_SWITCH_SIDESCREEN
    {
      public static LocString TITLE = (LocString) "Pressure";
      public static LocString THRESHOLD_SUBTITLE = (LocString) "Threshold:";
      public static LocString CURRENT_VALUE = (LocString) "Current {0}:\n{1}";
      public static LocString ACTIVATE_IF = (LocString) $"Send {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if:";
      public static LocString ABOVE_BUTTON = (LocString) "Above";
      public static LocString BELOW_BUTTON = (LocString) "Below";
      public static LocString STATUS_ACTIVE = (LocString) "Switch Active";
      public static LocString STATUS_INACTIVE = (LocString) "Switch Inactive";
      public static LocString PRESSURE = (LocString) "Ambient Pressure";
      public static LocString PRESSURE_TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the {UI.PRE_KEYWORD}Pressure{UI.PST_KEYWORD} is above <b>{{0}}</b>";
      public static LocString PRESSURE_TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the {UI.PRE_KEYWORD}Pressure{UI.PST_KEYWORD} is below <b>{{0}}</b>";
      public static LocString TEMPERATURE = (LocString) "Ambient Temperature";
      public static LocString TEMPERATURE_TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the ambient {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} is above <b>{{0}}</b>";
      public static LocString TEMPERATURE_TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the ambient {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} is below <b>{{0}}</b>";
      public static LocString CONTENT_TEMPERATURE = (LocString) "Internal Temperature";
      public static LocString CONTENT_TEMPERATURE_TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of its contents is above <b>{{0}}</b>";
      public static LocString CONTENT_TEMPERATURE_TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of its contents is below <b>{{0}}</b>";
      public static LocString BRIGHTNESS = (LocString) "Ambient Brightness";
      public static LocString BRIGHTNESS_TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the ambient {UI.PRE_KEYWORD}Brightness{UI.PST_KEYWORD} is above <b>{{0}}</b>";
      public static LocString BRIGHTNESS_TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the ambient {UI.PRE_KEYWORD}Brightness{UI.PST_KEYWORD} is below <b>{{0}}</b>";
      public static LocString WATTAGE = (LocString) "Wattage Reading";
      public static LocString WATTAGE_TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the {UI.PRE_KEYWORD}Wattage{UI.PST_KEYWORD} consumed is above <b>{{0}}</b>";
      public static LocString WATTAGE_TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the {UI.PRE_KEYWORD}Wattage{UI.PST_KEYWORD} consumed is below <b>{{0}}</b>";
      public static LocString DISEASE_TITLE = (LocString) "Germ Threshold";
      public static LocString DISEASE = (LocString) "Ambient Germs";
      public static LocString DISEASE_TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the number of {UI.PRE_KEYWORD}Germs{UI.PST_KEYWORD} is above <b>{{0}}</b>";
      public static LocString DISEASE_TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the number of {UI.PRE_KEYWORD}Germs{UI.PST_KEYWORD} is below <b>{{0}}</b>";
      public static LocString DISEASE_UNITS = (LocString) "";
      public static LocString CONTENT_DISEASE = (LocString) "Germ Count";
      public static LocString RADIATION = (LocString) "Ambient Radiation";
      public static LocString RADIATION_TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the ambient {UI.PRE_KEYWORD}Radiation{UI.PST_KEYWORD} is above <b>{{0}}</b>";
      public static LocString RADIATION_TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the ambient {UI.PRE_KEYWORD}Radiation{UI.PST_KEYWORD} is below <b>{{0}}</b>";
      public static LocString HEPS = (LocString) "Radbolt Reading";
      public static LocString HEPS_TOOLTIP_ABOVE = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the {UI.PRE_KEYWORD}Radbolts{UI.PST_KEYWORD} is above <b>{{0}}</b>";
      public static LocString HEPS_TOOLTIP_BELOW = (LocString) $"Will send a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)} if the {UI.PRE_KEYWORD}Radbolts{UI.PST_KEYWORD} is below <b>{{0}}</b>";
    }

    public class CAPACITY_CONTROL_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Storage Capacity Control";
      public static LocString MAX_LABEL = (LocString) "Max:";
    }

    public class DOOR_TOGGLE_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "DOOR SETTINGS";
      public static LocString OPEN = (LocString) "Door is open to all permitted entities, as well as liquids and gases.\n";
      public static LocString AUTO = (LocString) "Door will auto-open according to access permissions.\n";
      public static LocString CLOSE = (LocString) "Door is locked. It must be unlocked before use.\n";
      public static LocString PENDING_FORMAT = (LocString) "{0} {1}";
      public static LocString OPEN_PENDING = (LocString) "Awaiting Duplicant to open door.";
      public static LocString AUTO_PENDING = (LocString) "Awaiting Duplicant to automate door.";
      public static LocString CLOSE_PENDING = (LocString) "Awaiting Duplicant to lock door.";
      public static LocString ACCESS_FORMAT = (LocString) "{0}\n\n{1}";
      public static LocString ACCESS_OFFLINE = (LocString) $"Emergency Access Permissions:\nAll Duplicants are permitted to use this door until {UI.FormatAsLink("Power", "POWER")} is restored.";
      public static LocString POI_INTERNAL = (LocString) "This door cannot be manually controlled.";
    }

    public class ACTIVATION_RANGE_SIDE_SCREEN
    {
      public static LocString NAME = (LocString) "Breaktime Policy";
      public static LocString ACTIVATE = (LocString) "Break starts at:";
      public static LocString DEACTIVATE = (LocString) "Break ends at:";
    }

    public class CAPACITY_SIDE_SCREEN
    {
      public static LocString TOOLTIP = (LocString) "Adjust the maximum amount that can be stored here";
    }

    public class SUIT_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Dock Inventory";
      public static LocString CONFIGURATION_REQUIRED = (LocString) "Configuration Required:";
      public static LocString CONFIG_REQUEST_SUIT = (LocString) "Deliver Suit";
      public static LocString CONFIG_REQUEST_SUIT_TOOLTIP = (LocString) "Duplicants will immediately deliver and dock the nearest unequipped suit";
      public static LocString CONFIG_NO_SUIT = (LocString) "Leave Empty";
      public static LocString CONFIG_NO_SUIT_TOOLTIP = (LocString) "The next suited Duplicant to pass by will unequip their suit and dock it here";
      public static LocString CONFIG_CANCEL_REQUEST = (LocString) "Cancel Request";
      public static LocString CONFIG_CANCEL_REQUEST_TOOLTIP = (LocString) "Cancel this suit delivery";
      public static LocString CONFIG_DROP_SUIT = (LocString) "Undock Suit";
      public static LocString CONFIG_DROP_SUIT_TOOLTIP = (LocString) "Disconnect this suit, dropping it on the ground";
      public static LocString CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP = (LocString) "There is no suit in this building to undock";
    }

    public class AUTOMATABLE_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Automatable Storage";
      public static LocString ALLOWMANUALBUTTON = (LocString) "Allow Manual Use";
      public static LocString ALLOWMANUALBUTTONTOOLTIP = (LocString) "Allow Duplicants to manually manage these storage materials";
    }

    public class TURBOMODE_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Power Settings";
      public static LocString LABEL = (LocString) "TURBO MODE";
      public static LocString TOGGLE_TOOLTIP = (LocString) "Toggle Turbo Mode";
    }

    public class STUDYABLE_SIDE_SCREEN
    {
      public static LocString TITLE = (LocString) "Analyze Natural Feature";
      public static LocString STUDIED_STATUS = (LocString) "Researchers have completed their analysis and compiled their findings.";
      public static LocString STUDIED_BUTTON = (LocString) "ANALYSIS COMPLETE";
      public static LocString SEND_STATUS = (LocString) "Send a researcher to gather data here.\n\nAnalyzing a feature takes time, but yields useful results.";
      public static LocString SEND_BUTTON = (LocString) "ANALYZE";
      public static LocString PENDING_STATUS = (LocString) "A researcher is in the process of studying this feature.";
      public static LocString PENDING_BUTTON = (LocString) "CANCEL ANALYSIS";
    }

    public class MEDICALCOTSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Severity Requirement";
      public static LocString TOOLTIP = (LocString) $"A Duplicant may not use this cot until their {UI.PRE_KEYWORD}Health{UI.PST_KEYWORD} falls below <b>{{0}}%</b>";
    }

    public class WARPPORTALSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Teleporter";
      public static LocString IDLE = (LocString) "Teleporter online.\nPlease select a passenger:";
      public static LocString WAITING = (LocString) "Ready to transmit passenger.";
      public static LocString COMPLETE = (LocString) "Passenger transmitted!";
      public static LocString UNDERWAY = (LocString) "Transmitting passenger...";
      public static LocString CONSUMED = (LocString) "Teleporter recharging:";
      public static LocString BUTTON = (LocString) "Teleport!";
      public static LocString CANCELBUTTON = (LocString) "Cancel";
    }

    public class RADBOLTTHRESHOLDSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Radbolt Threshold";
      public static LocString CURRENT_THRESHOLD = (LocString) "Current Threshold: {0}%";
      public static LocString TOOLTIP = (LocString) $"Releases a {UI.PRE_KEYWORD}Radbolt{UI.PST_KEYWORD} when stored Radbolts exceed <b>{{0}}</b>";
      public static LocString PROGRESS_BAR_LABEL = (LocString) "Radbolt Generation";
      public static LocString PROGRESS_BAR_TOOLTIP = (LocString) $"The building will emit a {UI.PRE_KEYWORD}Radbolt{UI.PST_KEYWORD} in the chosen direction when fully charged";
    }

    public class LOGICBITSELECTORSIDESCREEN
    {
      public static LocString RIBBON_READER_TITLE = (LocString) "Ribbon Reader";
      public static LocString RIBBON_READER_DESCRIPTION = (LocString) "Selected <b>Bit's Signal</b> will be read by the <b>Output Port</b>";
      public static LocString RIBBON_WRITER_TITLE = (LocString) "Ribbon Writer";
      public static LocString RIBBON_WRITER_DESCRIPTION = (LocString) "Received <b>Signal</b> will be written to selected <b>Bit</b>";
      public static LocString BIT = (LocString) "Bit {0}";
      public static LocString STATE_ACTIVE = (LocString) "Green";
      public static LocString STATE_INACTIVE = (LocString) "Red";
    }

    public class LOGICALARMSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Notification Designer";
      public static LocString DESCRIPTION = (LocString) $"Notification will be sent upon receiving a {UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active)}\n\nMaking modifications will clear any existing notifications being sent by this building.";
      public static LocString NAME = (LocString) "<b>Name:</b>";
      public static LocString NAME_DEFAULT = (LocString) "Notification";
      public static LocString TOOLTIP = (LocString) "<b>Tooltip:</b>";
      public static LocString TOOLTIP_DEFAULT = (LocString) "Tooltip";
      public static LocString TYPE = (LocString) "<b>Type:</b>";
      public static LocString PAUSE = (LocString) "<b>Pause:</b>";
      public static LocString ZOOM = (LocString) "<b>Zoom:</b>";

      public class TOOLTIPS
      {
        public static LocString NAME = (LocString) "Select notification text";
        public static LocString TOOLTIP = (LocString) "Select notification hover text";
        public static LocString TYPE = (LocString) "Select the visual and aural style of the notification";
        public static LocString PAUSE = (LocString) "Time will pause upon notification when checked";
        public static LocString ZOOM = (LocString) "The view will zoom to this building upon notification when checked";
        public static LocString BAD = (LocString) "\"Boing boing!\"";
        public static LocString NEUTRAL = (LocString) "\"Pop!\"";
        public static LocString DUPLICANT_THREATENING = (LocString) "AHH!";
      }
    }

    public class GENETICANALYSISSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Genetic Analysis";
      public static LocString NONE_DISCOVERED = (LocString) "No mutant seeds have been found.";
      public static LocString SELECT_SEEDS = (LocString) "Select which seed types to analyze:";
      public static LocString SEED_NO_MUTANTS = (LocString) "</i>No mutants found</i>";
      public static LocString SEED_FORBIDDEN = (LocString) "</i>Won't analyze</i>";
      public static LocString SEED_ALLOWED = (LocString) "</i>Will analyze</i>";
    }

    public class RELATEDENTITIESSIDESCREEN
    {
      public static LocString TITLE = (LocString) "Related Objects";
    }
  }

  public class USERMENUACTIONS
  {
    public class TINKER
    {
      public static LocString ALLOW = (LocString) "Allow Tinker";
      public static LocString DISALLOW = (LocString) "Disallow Tinker";
      public static LocString TOOLTIP_DISALLOW = (LocString) "Disallow {0} on this {1}";
      public static LocString TOOLTIP_ALLOW = (LocString) "Allow  {0} on this {1}";
    }

    public class TRANSITTUBEWAX
    {
      public static LocString NAME = (LocString) "Enable Smooth Ride";
      public static LocString TOOLTIP = (LocString) $"Enables the use of {(string) ELEMENTS.MILKFAT.NAME} to boost travel speed";
    }

    public class CANCELTRANSITTUBEWAX
    {
      public static LocString NAME = (LocString) "Disable Smooth Ride";
      public static LocString TOOLTIP = (LocString) ("Disables travel speed boost and refunds stored " + (string) ELEMENTS.MILKFAT.NAME);
    }

    public class CLEANTOILET
    {
      public static LocString NAME = (LocString) "Clean Toilet";
      public static LocString TOOLTIP = (LocString) "Empty waste from this toilet";
    }

    public class CANCELCLEANTOILET
    {
      public static LocString NAME = (LocString) "Cancel Clean";
      public static LocString TOOLTIP = (LocString) "Cancel this cleaning order";
    }

    public class EMPTYBEEHIVE
    {
      public static LocString NAME = (LocString) "Enable Autoharvest";
      public static LocString TOOLTIP = (LocString) "Automatically harvest this hive when full";
    }

    public class CANCELEMPTYBEEHIVE
    {
      public static LocString NAME = (LocString) "Disable Autoharvest";
      public static LocString TOOLTIP = (LocString) "Do not automatically harvest this hive";
    }

    public class EMPTYDESALINATOR
    {
      public static LocString NAME = (LocString) "Empty Desalinator";
      public static LocString TOOLTIP = (LocString) "Empty salt from this desalinator";
    }

    public class CHANGE_ROOM
    {
      public static LocString REQUEST_OUTFIT = (LocString) "Request Outfit";
      public static LocString REQUEST_OUTFIT_TOOLTIP = (LocString) "Request outfit to be delivered to this change room";
      public static LocString CANCEL_REQUEST = (LocString) "Cancel Request";
      public static LocString CANCEL_REQUEST_TOOLTIP = (LocString) "Cancel outfit request";
      public static LocString DROP_OUTFIT = (LocString) "Drop Outfit";
      public static LocString DROP_OUTFIT_TOOLTIP = (LocString) "Drop outfit on floor";
    }

    public class DUMP
    {
      public static LocString NAME = (LocString) "Empty";
      public static LocString TOOLTIP = (LocString) "Dump bottle contents onto the floor";
      public static LocString NAME_OFF = (LocString) "Cancel Empty";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this empty order";
    }

    public class TAGFILTER
    {
      public static LocString NAME = (LocString) "Filter Settings";
      public static LocString TOOLTIP = (LocString) "Assign materials to storage";
    }

    public class CANCELCONSTRUCTION
    {
      public static LocString NAME = (LocString) "Cancel Build";
      public static LocString TOOLTIP = (LocString) "Cancel this build order";
    }

    public class DIG
    {
      public static LocString NAME = (LocString) "Dig";
      public static LocString TOOLTIP = (LocString) "Dig out this cell";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this dig order";
    }

    public class CANCELMOP
    {
      public static LocString NAME = (LocString) "Cancel Mop";
      public static LocString TOOLTIP = (LocString) "Cancel this mop order";
    }

    public class CANCELDIG
    {
      public static LocString NAME = (LocString) "Cancel Dig";
      public static LocString TOOLTIP = (LocString) "Cancel this dig order";
    }

    public class UPROOT
    {
      public static LocString NAME = (LocString) "Uproot";
      public static LocString TOOLTIP = (LocString) "Convert this plant into a seed";
    }

    public class CANCELUPROOT
    {
      public static LocString NAME = (LocString) "Cancel Uproot";
      public static LocString TOOLTIP = (LocString) "Cancel this uproot order";
    }

    public class HARVEST_WHEN_READY
    {
      public static LocString NAME = (LocString) "Enable Autoharvest";
      public static LocString TOOLTIP = (LocString) "Automatically harvest this plant when it matures";
    }

    public class CANCEL_HARVEST_WHEN_READY
    {
      public static LocString NAME = (LocString) "Disable Autoharvest";
      public static LocString TOOLTIP = (LocString) "Do not automatically harvest this plant";
    }

    public class HARVEST
    {
      public static LocString NAME = (LocString) "Harvest";
      public static LocString TOOLTIP = (LocString) "Harvest materials from this plant";
      public static LocString TOOLTIP_DISABLED = (LocString) "This plant has nothing to harvest";
    }

    public class CANCELHARVEST
    {
      public static LocString NAME = (LocString) "Cancel Harvest";
      public static LocString TOOLTIP = (LocString) "Cancel this harvest order";
    }

    public class ATTACK
    {
      public static LocString NAME = (LocString) "Attack";
      public static LocString TOOLTIP = (LocString) "Attack this critter";
    }

    public class CANCELATTACK
    {
      public static LocString NAME = (LocString) "Cancel Attack";
      public static LocString TOOLTIP = (LocString) "Cancel this attack order";
    }

    public class CAPTURE
    {
      public static LocString NAME = (LocString) "Wrangle";
      public static LocString TOOLTIP = (LocString) "Capture this critter alive";
    }

    public class CANCELCAPTURE
    {
      public static LocString NAME = (LocString) "Cancel Wrangle";
      public static LocString TOOLTIP = (LocString) "Cancel this wrangle order";
    }

    public class RELEASEELEMENT
    {
      public static LocString NAME = (LocString) "Empty Building";
      public static LocString TOOLTIP = (LocString) "Refund all resources currently in use by this building";
    }

    public class DECONSTRUCT
    {
      public static LocString NAME = (LocString) "Deconstruct";
      public static LocString TOOLTIP = (LocString) "Deconstruct this building and refund all resources";
      public static LocString NAME_OFF = (LocString) "Cancel Deconstruct";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this deconstruct order";
    }

    public class DEMOLISH
    {
      public static LocString NAME = (LocString) "Demolish";
      public static LocString TOOLTIP = (LocString) "Demolish this building";
      public static LocString NAME_OFF = (LocString) "Cancel Demolition";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this demolition order";
    }

    public class ROCKETUSAGERESTRICTION
    {
      public static LocString NAME_UNCONTROLLED = (LocString) "Uncontrolled";
      public static LocString TOOLTIP_UNCONTROLLED = (LocString) ("Do not allow this building to be controlled by a " + (string) BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME);
      public static LocString NAME_CONTROLLED = (LocString) "Controlled";
      public static LocString TOOLTIP_CONTROLLED = (LocString) ("Allow this building's operation to be controlled by a " + (string) BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME);
    }

    public class MANUAL_DELIVERY
    {
      public static LocString NAME = (LocString) "Disable Delivery";
      public static LocString TOOLTIP = (LocString) "Do not deliver materials to this building";
      public static LocString NAME_OFF = (LocString) "Enable Delivery";
      public static LocString TOOLTIP_OFF = (LocString) "Deliver materials to this building";
    }

    public class SELECTRESEARCH
    {
      public static LocString NAME = (LocString) "Select Research";
      public static LocString TOOLTIP = (LocString) ("Choose a technology from the " + UI.FormatAsManagementMenu("Research Tree", Action.ManageResearch));
    }

    public class RECONSTRUCT
    {
      public static LocString REQUEST_RECONSTRUCT = (LocString) "Order Rebuild";
      public static LocString REQUEST_RECONSTRUCT_TOOLTIP = (LocString) "Deconstruct this building and rebuild it using the selected material";
      public static LocString CANCEL_RECONSTRUCT = (LocString) "Cancel Rebuild Order";
      public static LocString CANCEL_RECONSTRUCT_TOOLTIP = (LocString) "Cancel deconstruction and rebuilding of this building";
    }

    public class RELOCATE
    {
      public static LocString NAME = (LocString) "Relocate";
      public static LocString TOOLTIP = (LocString) "Move this building to a new location\n\nCosts no additional resources";
      public static LocString NAME_OFF = (LocString) "Cancel Relocation";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this relocation order";
    }

    public class ENABLEBUILDING
    {
      public static LocString NAME = (LocString) "Disable Building";
      public static LocString TOOLTIP = (LocString) "Halt the use of this building {Hotkey}\n\nDisabled buildings consume no energy or resources";
      public static LocString NAME_OFF = (LocString) "Enable Building";
      public static LocString TOOLTIP_OFF = (LocString) "Resume the use of this building {Hotkey}";
    }

    public class READLORE
    {
      public static LocString NAME = (LocString) "Inspect";
      public static LocString ALREADYINSPECTED = (LocString) "Already inspected";
      public static LocString TOOLTIP = (LocString) "Recover files from this structure";
      public static LocString TOOLTIP_ALREADYINSPECTED = (LocString) "This structure has already been inspected";
      public static LocString GOTODATABASE = (LocString) "View Entry";
      public static LocString SEARCH_DISPLAY = (LocString) "The display is still functional. I copy its message into my database.\n\nNew Database Entry discovered.";
      public static LocString SEARCH_ELLIESDESK = (LocString) "All I find on the machine is a curt e-mail from a disgruntled employee.\n\nNew Database Entry discovered.";
      public static LocString SEARCH_POD = (LocString) "I search my incoming message history and find a single entry. I move the odd message into my database.\n\nNew Database Entry discovered.";
      public static LocString ALREADY_SEARCHED = (LocString) "I already took everything of interest from this. I can check the Database to re-read what I found.";
      public static LocString SEARCH_CABINET = (LocString) "One intact document remains - an old yellowing newspaper clipping. It won't be of much use, but I add it to my database nonetheless.\n\nNew Database Entry discovered.";
      public static LocString SEARCH_STERNSDESK = (LocString) "There's an old magazine article from a publication called the \"Nucleoid\" tucked in the top drawer. I add it to my database.\n\nNew Database Entry discovered.";
      public static LocString ALREADY_SEARCHED_STERNSDESK = (LocString) "The desk is eerily empty inside.";
      public static LocString SEARCH_TELEPORTER_SENDER = (LocString) "While scanning the antiquated computer code of this machine I uncovered some research notes. I add them to my database.\n\nNew Database Entry discovered.";
      public static LocString SEARCH_TELEPORTER_RECEIVER = (LocString) "Incongruously placed research notes are hidden within the operating instructions of this device. I add them to my database.\n\nNew Database Entry discovered.";
      public static LocString SEARCH_CRYO_TANK = (LocString) "There are some safety instructions included in the operating instructions of this Cryotank. I add them to my database.\n\nNew Database Entry discovered.";
      public static LocString SEARCH_PROPGRAVITASCREATUREPOSTER = (LocString) "There's a handwritten note taped to the back of this poster. I add it to my database.\n\nNew Database Entry discovered.";
      public static LocString SEARCH_DLC5MINIFRIDGE = (LocString) "There's a note taped to an empty jar on the top shelf. I add it to my database.\n\nNew Database Entry discovered.";

      public class SEARCH_COMPUTER_PODIUM
      {
        public static LocString SEARCH1 = (LocString) "I search through the computer's database and find an unredacted e-mail.\n\nNew Database Entry unlocked.";
      }

      public class SEARCH_COMPUTER_SUCCESS
      {
        public static LocString SEARCH1 = (LocString) "After searching through the computer's database, I managed to piece together some files that piqued my interest.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH2 = (LocString) "Searching through the computer, I find some recoverable files that are still readable.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH3 = (LocString) "The computer looks pristine on the outside, but is corrupted internally. Still, I managed to find one uncorrupted file, and have added it to my database.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH4 = (LocString) "The computer was wiped almost completely clean, except for one file hidden in the recycle bin.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH5 = (LocString) "I search the computer, storing what useful data I can find in my own memory.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH6 = (LocString) "This computer is broken and requires some finessing to get working. Still, I recover a handful of interesting files.\n\nNew Database Entry unlocked.";
      }

      public class SEARCH_COMPUTER_FAIL
      {
        public static LocString SEARCH1 = (LocString) "Unfortunately, the computer's hard drive is irreparably corrupted.";
        public static LocString SEARCH2 = (LocString) "The computer was wiped clean before I got here. There is nothing to recover.";
        public static LocString SEARCH3 = (LocString) "Some intact files are available on the computer, but nothing I haven't already discovered elsewhere. I find nothing else.";
        public static LocString SEARCH4 = (LocString) "The computer has nothing of import.";
        public static LocString SEARCH5 = (LocString) "Someone's left a solitaire game up. There's nothing else of interest on the computer.\n\nAlso, it looks as though they were about to lose.";
        public static LocString SEARCH6 = (LocString) "The background on this computer depicts two kittens hugging in a field of daisies. There is nothing else of import to be found.";
        public static LocString SEARCH7 = (LocString) "The user alphabetized the shortcuts on their desktop. There is nothing else of import to be found.";
        public static LocString SEARCH8 = (LocString) "The background is a picture of a golden retriever in a science lab. It looks very confused. There is nothing else of import to be found.";
        public static LocString SEARCH9 = (LocString) "This user never changed their default background. There is nothing else of import to be found. How dull.";
      }

      public class SEARCH_TECHNOLOGY_SUCCESS
      {
        public static LocString SEARCH1 = (LocString) "I scour the internal systems and find something of interest.\n\nNew Database Entry discovered.";
        public static LocString SEARCH2 = (LocString) "I see if I can salvage anything from the electronics. I add what I find to my database.\n\nNew Database Entry discovered.";
        public static LocString SEARCH3 = (LocString) "I look for anything of interest within the abandoned machinery and add what I find to my database.\n\nNew Database Entry discovered.";
      }

      public class SEARCH_OBJECT_SUCCESS
      {
        public static LocString SEARCH1 = (LocString) "I look around and recover an old file.\n\nNew Database Entry discovered.";
        public static LocString SEARCH2 = (LocString) "There's a three-ringed binder inside. I scan the surviving documents.\n\nNew Database Entry discovered.";
        public static LocString SEARCH3 = (LocString) "A discarded journal inside remains mostly intact. I scan the pages of use.\n\nNew Database Entry discovered.";
        public static LocString SEARCH4 = (LocString) "A single page of a long printout remains legible. I scan it and add it to my database.\n\nNew Database Entry discovered.";
        public static LocString SEARCH5 = (LocString) "A few loose papers can be found inside. I scan the ones that look interesting.\n\nNew Database Entry discovered.";
        public static LocString SEARCH6 = (LocString) "I find a memory stick inside and copy its data into my database.\n\nNew Database Entry discovered.";
      }

      public class SEARCH_OBJECT_FAIL
      {
        public static LocString SEARCH1 = (LocString) "I look around but find nothing of interest.";
      }

      public class SEARCH_FLATOBJECT_SUCCESS
      {
        public static LocString SEARCH1 = (LocString) "There's a recording device taped to the back.\n\nNew Database Entry discovered.";
        public static LocString SEARCH2 = (LocString) "I find a small plastic disc and download its contents to my database.\n\nNew Database Entry discovered.";
        public static LocString SEARCH3 = (LocString) "A bundle of papers was tucked out of sight. I scan the contents and add them to my database.\n\nNew Database Entry discovered.";
      }

      public class SEARCH_FLATOBJECT_FAIL
      {
        public static LocString SEARCH1 = (LocString) "There is nothing to find here. I suppose some things really are exactly what they seem to be.";
      }

      public class SEARCH_SPACEPOI_SUCCESS
      {
        public static LocString SEARCH1 = (LocString) "A quick analysis of the hardware of this debris has uncovered some searchable files within.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH2 = (LocString) "There's an archaic interface I can interact with on this device.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH3 = (LocString) "While investigating the software of this wreckage, a compelling file comes to my attention.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH4 = (LocString) "Not much remains of the software that once ran this spacecraft except for one file that piques my interest.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH5 = (LocString) "I find some noteworthy data hidden amongst the system files of this space junk.\n\nNew Database Entry unlocked.";
        public static LocString SEARCH6 = (LocString) "Despite being subjected to years of degradation, there are still recoverable files in this machinery.\n\nNew Database Entry unlocked.";
      }

      public class SEARCH_SPACEPOI_FAIL
      {
        public static LocString SEARCH1 = (LocString) "There's nothing of interest left in this old space junk.";
        public static LocString SEARCH2 = (LocString) "I've salvaged everything I can from this vehicle.";
        public static LocString SEARCH3 = (LocString) "Years of neglect and radioactive decay have destroyed all the useful data from this derelict spacecraft.";
      }

      public class SEARCH_DISPLAY_FAIL
      {
        public static LocString SEARCH1 = (LocString) "The display is frozen. Whatever information it once contained is long gone.";
      }
    }

    public class OPENPOI
    {
      public static LocString NAME = (LocString) "Rummage";
      public static LocString TOOLTIP = (LocString) "Scrounge for usable materials";
      public static LocString NAME_OFF = (LocString) "Cancel Rummage";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this rummage order";
      public static LocString ALREADY_RUMMAGED = (LocString) "Already Rummaged";
      public static LocString TOOLTIP_ALREADYRUMMAGED = (LocString) "There are no usable materials left to find";
    }

    public class OPEN_TECHUNLOCKS
    {
      public static LocString NAME = (LocString) "Unlock Portal";
      public static LocString TOOLTIP = (LocString) "Retrieve data stored in this building";
      public static LocString NAME_OFF = (LocString) "Cancel Unlock Portal";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this portal access order";
      public static LocString ALREADY_RUMMAGED = (LocString) "Already Unlocked";
      public static LocString TOOLTIP_ALREADYRUMMAGED = (LocString) "All data has been accessed and recorded";
    }

    public class EMPTYSTORAGE
    {
      public static LocString NAME = (LocString) "Empty Storage";
      public static LocString TOOLTIP = (LocString) "Eject all resources from this container";
      public static LocString NAME_OFF = (LocString) "Cancel Empty";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this empty order";
    }

    public class CLOSESTORAGE
    {
      public static LocString NAME = (LocString) "Close Storage";
      public static LocString TOOLTIP = (LocString) "Prevent this container from receiving resources for storage";
      public static LocString NAME_OFF = (LocString) "Cancel Close";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this close order";
    }

    public class COPY_BUILDING_SETTINGS
    {
      public static LocString NAME = (LocString) "Copy Settings";
      public static LocString TOOLTIP = (LocString) "Apply the settings and priorities of this building to other buildings of the same type {Hotkey}";
    }

    public class CLEAR
    {
      public static LocString NAME = (LocString) "Sweep";
      public static LocString TOOLTIP = (LocString) "Put this object away in the nearest storage container";
      public static LocString NAME_OFF = (LocString) "Cancel Sweeping";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this sweep order";
    }

    public class COMPOST
    {
      public static LocString NAME = (LocString) "Compost";
      public static LocString TOOLTIP = (LocString) "Mark this object for compost";
      public static LocString NAME_OFF = (LocString) "Cancel Compost";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this compost order";
    }

    public class PICKUPABLEMOVE
    {
      public static LocString NAME = (LocString) "Relocate To";
      public static LocString TOOLTIP = (LocString) "Relocate this object to a specific location";
      public static LocString NAME_OFF = (LocString) "Cancel Relocate";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel order to relocate this object";
    }

    public class UNEQUIP
    {
      public static LocString NAME = (LocString) "Unequip {0}";
      public static LocString TOOLTIP = (LocString) "Take off and drop this equipment";
    }

    public class QUARANTINE
    {
      public static LocString NAME = (LocString) "Quarantine";
      public static LocString TOOLTIP = (LocString) "Isolate this Duplicant\nThe Duplicant will return to their assigned Cot";
      public static LocString TOOLTIP_DISABLED = (LocString) "No quarantine zone assigned";
      public static LocString NAME_OFF = (LocString) "Cancel Quarantine";
      public static LocString TOOLTIP_OFF = (LocString) "Cancel this quarantine order";
    }

    public class DRAWPATHS
    {
      public static LocString NAME = (LocString) "Show Navigation";
      public static LocString TOOLTIP = (LocString) "Show all areas within this Duplicant's reach";
      public static LocString NAME_OFF = (LocString) "Hide Navigation";
      public static LocString TOOLTIP_OFF = (LocString) "Hide areas within this Duplicant's reach";
    }

    public class MOVETOLOCATION
    {
      public static LocString NAME = (LocString) "Move To";
      public static LocString TOOLTIP = (LocString) "Move this Duplicant to a specific location";
    }

    public class FOLLOWCAM
    {
      public static LocString NAME = (LocString) "Follow Cam";
      public static LocString TOOLTIP = (LocString) "Track this Duplicant with the camera";
    }

    public class WORKABLE_DIRECTION_BOTH
    {
      public static LocString NAME = (LocString) "Set Direction: Both";
      public static LocString TOOLTIP = (LocString) "Select to make Duplicants wash when passing by in either direction";
    }

    public class WORKABLE_DIRECTION_LEFT
    {
      public static LocString NAME = (LocString) "Set Direction: Left";
      public static LocString TOOLTIP = (LocString) "Select to make Duplicants wash when passing by from right to left";
    }

    public class WORKABLE_DIRECTION_RIGHT
    {
      public static LocString NAME = (LocString) "Set Direction: Right";
      public static LocString TOOLTIP = (LocString) "Select to make Duplicants wash when passing by from left to right";
    }

    public class MANUAL_PUMP_DELIVERY
    {
      public static class ALLOWED
      {
        public static LocString NAME = (LocString) "Enable Auto-Bottle";
        public static LocString TOOLTIP = (LocString) "If enabled, Duplicants will deliver bottled liquids to this building directly from these sources:\n";
        public static LocString ITEM = (LocString) "\n{0}";
      }

      public static class DENIED
      {
        public static LocString NAME = (LocString) "Disable Auto-Bottle";
        public static LocString TOOLTIP = (LocString) "If disabled, Duplicants will no longer deliver bottled liquids directly from Pitcher Pumps";
      }

      public static class ALLOWED_GAS
      {
        public static LocString NAME = (LocString) "Enable Auto-Bottle";
        public static LocString TOOLTIP = (LocString) "If enabled, Duplicants will deliver gas canisters to this building directly from Canister Fillers";
      }

      public static class DENIED_GAS
      {
        public static LocString NAME = (LocString) "Disable Auto-Bottle";
        public static LocString TOOLTIP = (LocString) "If disabled, Duplicants will no longer deliver gas canisters directly from Canister Fillers";
      }
    }

    public class SUIT_MARKER_TRAVERSAL
    {
      public static class ONLY_WHEN_ROOM_AVAILABLE
      {
        public static LocString NAME = (LocString) "Clearance: Vacancy";
        public static LocString TOOLTIP = (LocString) "Suited Duplicants may only pass if there is an available dock to store their suit";
      }

      public static class ALWAYS
      {
        public static LocString NAME = (LocString) "Clearance: Always";
        public static LocString TOOLTIP = (LocString) "Suited Duplicants may pass even if there is no room to store their suits\n\nWhen all available docks are full, Duplicants will unequip their suits and drop them on the floor";
      }
    }

    public class ACTIVATEBUILDING
    {
      public static LocString ACTIVATE = (LocString) "Activate";
      public static LocString TOOLTIP_ACTIVATE = (LocString) "Request a Duplicant to activate this building";
      public static LocString TOOLTIP_ACTIVATED = (LocString) "This building has already been activated";
      public static LocString ACTIVATE_CANCEL = (LocString) "Cancel Activation";
      public static LocString ACTIVATED = (LocString) "Activated";
      public static LocString TOOLTIP_CANCEL = (LocString) "Cancel activation of this building";
    }

    public class ACCEPT_MUTANT_SEEDS
    {
      public static LocString ACCEPT = (LocString) "Allow Mutants";
      public static LocString REJECT = (LocString) "Forbid Mutants";
      public static LocString TOOLTIP = (LocString) $"Toggle whether or not this building will accept {UI.PRE_KEYWORD}Mutant Seeds{UI.PST_KEYWORD} for recipes that could use them";
      public static LocString FISH_FEEDER_TOOLTIP = (LocString) $"Toggle whether or not this feeder will accept {UI.PRE_KEYWORD}Mutant Seeds{UI.PST_KEYWORD} for critters who eat them";
    }

    public class CARVE
    {
      public static LocString NAME = (LocString) "Carve";
      public static LocString TOOLTIP = (LocString) "Carve this rock to enhance its positive effects";
    }

    public class CANCELCARVE
    {
      public static LocString NAME = (LocString) "Cancel Carve";
      public static LocString TOOLTIP = (LocString) "Cancel order to carve this rock";
    }
  }

  public class BUILDCATEGORIES
  {
    public static class BASE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Base", "BUILDCATEGORYBASE");
      public static LocString TOOLTIP = (LocString) "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
    }

    public static class CONVEYANCE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Shipping", "BUILDCATEGORYCONVEYANCE");
      public static LocString TOOLTIP = (LocString) "Transport ore and solid materials around my base. {Hotkey}";
    }

    public static class OXYGEN
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Oxygen", "BUILDCATEGORYOXYGEN");
      public static LocString TOOLTIP = (LocString) "Everything I need to keep the colony breathing. {Hotkey}";
    }

    public static class POWER
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Power", "BUILDCATEGORYPOWER");
      public static LocString TOOLTIP = (LocString) "Need to power the colony? Here's how to do it! {Hotkey}";
    }

    public static class FOOD
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Food", "BUILDCATEGORYFOOD");
      public static LocString TOOLTIP = (LocString) "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
    }

    public static class UTILITIES
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Utilities", "BUILDCATEGORYUTILITIES");
      public static LocString TOOLTIP = (LocString) "Heat up and cool down. {Hotkey}";
    }

    public static class PLUMBING
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Plumbing", "BUILDCATEGORYPLUMBING");
      public static LocString TOOLTIP = (LocString) "Get the colony's water running and its sewage flowing. {Hotkey}";
    }

    public static class HVAC
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Ventilation", "BUILDCATEGORYHVAC");
      public static LocString TOOLTIP = (LocString) "Control the flow of gas in the base. {Hotkey}";
    }

    public static class REFINING
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Refinement", "BUILDCATEGORYREFINING");
      public static LocString TOOLTIP = (LocString) "Use the resources I want, filter the ones I don't. {Hotkey}";
    }

    public static class ROCKETRY
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Rocketry", "BUILDCATEGORYROCKETRY");
      public static LocString TOOLTIP = (LocString) "With rockets, the sky's no longer the limit! {Hotkey}";
    }

    public static class MEDICAL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Medicine", "BUILDCATEGORYMEDICAL");
      public static LocString TOOLTIP = (LocString) "A cure for everything but the common cold. {Hotkey}";
    }

    public static class FURNITURE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Furniture", "BUILDCATEGORYFURNITURE");
      public static LocString TOOLTIP = (LocString) "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
    }

    public static class EQUIPMENT
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Stations", "BUILDCATEGORYEQUIPMENT");
      public static LocString TOOLTIP = (LocString) "Unlock new technologies through the power of science! {Hotkey}";
    }

    public static class MISC
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Decor", "BUILDCATEGORYMISC");
      public static LocString TOOLTIP = (LocString) "Spruce up my colony with some lovely interior decorating. {Hotkey}";
    }

    public static class AUTOMATION
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Automation", "BUILDCATEGORYAUTOMATION");
      public static LocString TOOLTIP = (LocString) "Automate my base with a wide range of sensors. {Hotkey}";
    }

    public static class HEP
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Radiation", "BUILDCATEGORYHEP");
      public static LocString TOOLTIP = (LocString) "Here's where things get rad. {Hotkey}";
    }
  }

  public class NEWBUILDCATEGORIES
  {
    public static class BASE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Base", "BUILD_CATEGORY_BASE");
      public static LocString TOOLTIP = (LocString) "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
    }

    public static class INFRASTRUCTURE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Utilities", "BUILD_CATEGORY_INFRASTRUCTURE");
      public static LocString TOOLTIP = (LocString) "Power, plumbing, and ventilation can all be found here. {Hotkey}";
    }

    public static class FOODANDAGRICULTURE
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Food", "BUILD_CATEGORY_FOODANDAGRICULTURE");
      public static LocString TOOLTIP = (LocString) "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
    }

    public static class LOGISTICS
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Logistics", "BUILD_CATEGORY_LOGISTICS");
      public static LocString TOOLTIP = (LocString) "Devices for base automation and material transport. {Hotkey}";
    }

    public static class HEALTHANDHAPPINESS
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Accommodation", "BUILD_CATEGORY_HEALTHANDHAPPINESS");
      public static LocString TOOLTIP = (LocString) "Everything a Duplicant needs to stay happy, healthy, and fulfilled. {Hotkey}";
    }

    public static class INDUSTRIAL
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Industrials", "BUILD_CATEGORY_INDUSTRIAL");
      public static LocString TOOLTIP = (LocString) "Machinery for oxygen production, heat management, and material refinement. {Hotkey}";
    }

    public static class LADDERS
    {
      public static LocString NAME = (LocString) "Ladders";
      public static LocString BUILDMENUTITLE = (LocString) "Ladders";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class TILES
    {
      public static LocString NAME = (LocString) "Tiles and Drywall";
      public static LocString BUILDMENUTITLE = (LocString) "Tiles and Drywall";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class PRINTINGPODS
    {
      public static LocString NAME = (LocString) "Printing Pods";
      public static LocString BUILDMENUTITLE = (LocString) "Printing Pods";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class DOORS
    {
      public static LocString NAME = (LocString) "Doors";
      public static LocString BUILDMENUTITLE = (LocString) "Doors";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class STORAGE
    {
      public static LocString NAME = (LocString) "Storage";
      public static LocString BUILDMENUTITLE = (LocString) "Storage";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class TRANSPORT
    {
      public static LocString NAME = (LocString) "Transit Tubes";
      public static LocString BUILDMENUTITLE = (LocString) "Transit Tubes";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class OPERATIONS
    {
      public static LocString NAME = (LocString) "Operations";
      public static LocString BUILDMENUTITLE = (LocString) "Operations";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class PRODUCERS
    {
      public static LocString NAME = (LocString) "Production";
      public static LocString BUILDMENUTITLE = (LocString) "Production";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class SCRUBBERS
    {
      public static LocString NAME = (LocString) "Purification";
      public static LocString BUILDMENUTITLE = (LocString) "Purification";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class DISTRIBUTORS
    {
      public static LocString NAME = (LocString) "Distribution";
      public static LocString BUILDMENUTITLE = (LocString) "Distribution";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class BATTERIES
    {
      public static LocString NAME = (LocString) "Batteries";
      public static LocString BUILDMENUTITLE = (LocString) "Batteries";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class SWITCHES
    {
      public static LocString NAME = (LocString) "Switches";
      public static LocString BUILDMENUTITLE = (LocString) "Switches";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class COOKING
    {
      public static LocString NAME = (LocString) "Cooking";
      public static LocString BUILDMENUTITLE = (LocString) "Cooking";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class FARMING
    {
      public static LocString NAME = (LocString) "Farming";
      public static LocString BUILDMENUTITLE = (LocString) "Farming";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class RANCHING
    {
      public static LocString NAME = (LocString) "Ranching";
      public static LocString BUILDMENUTITLE = (LocString) "Ranching";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class WASHROOM
    {
      public static LocString NAME = (LocString) "Washroom";
      public static LocString BUILDMENUTITLE = (LocString) "Washroom";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class VALVES
    {
      public static LocString NAME = (LocString) "Valves";
      public static LocString BUILDMENUTITLE = (LocString) "Valves";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class PUMPS
    {
      public static LocString NAME = (LocString) "Pumps";
      public static LocString BUILDMENUTITLE = (LocString) "Pumps";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class SENSORS
    {
      public static LocString NAME = (LocString) "Sensors";
      public static LocString BUILDMENUTITLE = (LocString) "Sensors";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class PORTS
    {
      public static LocString NAME = (LocString) "Ports";
      public static LocString BUILDMENUTITLE = (LocString) "Ports";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class MATERIALS
    {
      public static LocString NAME = (LocString) "Materials";
      public static LocString BUILDMENUTITLE = (LocString) "Materials";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class OIL
    {
      public static LocString NAME = (LocString) "Oil";
      public static LocString BUILDMENUTITLE = (LocString) "Oil";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class ADVANCED
    {
      public static LocString NAME = (LocString) "Advanced";
      public static LocString BUILDMENUTITLE = (LocString) "Advanced";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class ORGANIC
    {
      public static LocString NAME = (LocString) "Organic";
      public static LocString BUILDMENUTITLE = (LocString) "Organic";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class BEDS
    {
      public static LocString NAME = (LocString) "Beds";
      public static LocString BUILDMENUTITLE = (LocString) "Beds";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class LIGHTS
    {
      public static LocString NAME = (LocString) "Lights";
      public static LocString BUILDMENUTITLE = (LocString) "Lights";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class DINING
    {
      public static LocString NAME = (LocString) "Dining";
      public static LocString BUILDMENUTITLE = (LocString) "Dining";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class MANUFACTURING
    {
      public static LocString NAME = (LocString) "Manufacturing";
      public static LocString BUILDMENUTITLE = (LocString) "Manufacturing";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class TEMPERATURE
    {
      public static LocString NAME = (LocString) "Temperature";
      public static LocString BUILDMENUTITLE = (LocString) "Temperature";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class RESEARCH
    {
      public static LocString NAME = (LocString) "Research";
      public static LocString BUILDMENUTITLE = (LocString) "Research";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class GENERATORS
    {
      public static LocString NAME = (LocString) "Generators";
      public static LocString BUILDMENUTITLE = (LocString) "Generators";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class WIRES
    {
      public static LocString NAME = (LocString) "Wires";
      public static LocString BUILDMENUTITLE = (LocString) "Wires";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class ELECTROBANKBUILDINGS
    {
      public static LocString NAME = (LocString) "Converters";
      public static LocString BUILDMENUTITLE = (LocString) "Converters";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class LOGICGATES
    {
      public static LocString NAME = (LocString) "Gates";
      public static LocString BUILDMENUTITLE = (LocString) "Gates";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class TRANSMISSIONS
    {
      public static LocString NAME = (LocString) "Transmissions";
      public static LocString BUILDMENUTITLE = (LocString) "Transmissions";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class LOGICMANAGER
    {
      public static LocString NAME = (LocString) "Monitoring";
      public static LocString BUILDMENUTITLE = (LocString) "Monitoring";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class LOGICAUDIO
    {
      public static LocString NAME = (LocString) "Ambience";
      public static LocString BUILDMENUTITLE = (LocString) "Ambience";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class CONVEYANCESTRUCTURES
    {
      public static LocString NAME = (LocString) "Structural";
      public static LocString BUILDMENUTITLE = (LocString) "Structural";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class BUILDMENUPORTS
    {
      public static LocString NAME = (LocString) "Ports";
      public static LocString BUILDMENUTITLE = (LocString) "Ports";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class POWERCONTROL
    {
      public static LocString NAME = (LocString) "Power\nRegulation";
      public static LocString BUILDMENUTITLE = (LocString) "Power Regulation";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class PLUMBINGSTRUCTURES
    {
      public static LocString NAME = (LocString) "Plumbing";
      public static LocString BUILDMENUTITLE = (LocString) "Plumbing";
      public static LocString TOOLTIP = (LocString) "Get the colony's water running and its sewage flowing. {Hotkey}";
    }

    public static class PIPES
    {
      public static LocString NAME = (LocString) "Pipes";
      public static LocString BUILDMENUTITLE = (LocString) "Pipes";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class VENTILATIONSTRUCTURES
    {
      public static LocString NAME = (LocString) "Ventilation";
      public static LocString BUILDMENUTITLE = (LocString) "Ventilation";
      public static LocString TOOLTIP = (LocString) "Control the flow of gas in your base. {Hotkey}";
    }

    public static class CONVEYANCE
    {
      public static LocString NAME = (LocString) "Ore\nTransport";
      public static LocString BUILDMENUTITLE = (LocString) "Ore Transport";
      public static LocString TOOLTIP = (LocString) "Transport ore and solid materials around my base. {Hotkey}";
    }

    public static class HYGIENE
    {
      public static LocString NAME = (LocString) "Hygiene";
      public static LocString BUILDMENUTITLE = (LocString) "Hygiene";
      public static LocString TOOLTIP = (LocString) "Keeps my Duplicants clean.";
    }

    public static class MEDICAL
    {
      public static LocString NAME = (LocString) "Medical";
      public static LocString BUILDMENUTITLE = (LocString) "Medical";
      public static LocString TOOLTIP = (LocString) "A cure for everything but the common cold. {Hotkey}";
    }

    public static class WELLNESS
    {
      public static LocString NAME = (LocString) "Wellness";
      public static LocString BUILDMENUTITLE = (LocString) "Wellness";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class RECREATION
    {
      public static LocString NAME = (LocString) "Recreation";
      public static LocString BUILDMENUTITLE = (LocString) "Recreation";
      public static LocString TOOLTIP = (LocString) "Everything needed to reduce stress and increase fun.";
    }

    public static class FURNITURE
    {
      public static LocString NAME = (LocString) "Furniture";
      public static LocString BUILDMENUTITLE = (LocString) "Furniture";
      public static LocString TOOLTIP = (LocString) "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
    }

    public static class DECOR
    {
      public static LocString NAME = (LocString) "Decor";
      public static LocString BUILDMENUTITLE = (LocString) "Decor";
      public static LocString TOOLTIP = (LocString) "Spruce up your colony with some lovely interior decorating. {Hotkey}";
    }

    public static class OXYGEN
    {
      public static LocString NAME = (LocString) "Oxygen";
      public static LocString BUILDMENUTITLE = (LocString) "Oxygen";
      public static LocString TOOLTIP = (LocString) "Everything I need to keep my colony breathing. {Hotkey}";
    }

    public static class UTILITIES
    {
      public static LocString NAME = (LocString) "Temperature";
      public static LocString BUILDMENUTITLE = (LocString) "Temperature";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class REFINING
    {
      public static LocString NAME = (LocString) "Refinement";
      public static LocString BUILDMENUTITLE = (LocString) "Refinement";
      public static LocString TOOLTIP = (LocString) "Use the resources you want, filter the ones you don't. {Hotkey}";
    }

    public static class EQUIPMENT
    {
      public static LocString NAME = (LocString) "Equipment";
      public static LocString BUILDMENUTITLE = (LocString) "Equipment";
      public static LocString TOOLTIP = (LocString) "Unlock new technologies through the power of science! {Hotkey}";
    }

    public static class ARCHAEOLOGY
    {
      public static LocString NAME = (LocString) "Archaeology";
      public static LocString BUILDMENUTITLE = (LocString) "Archaeology";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class METEORDEFENSE
    {
      public static LocString NAME = (LocString) "Meteor Defense";
      public static LocString BUILDMENUTITLE = (LocString) "Meteor Defense";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class INDUSTRIALSTATION
    {
      public static LocString NAME = (LocString) "Industrial";
      public static LocString BUILDMENUTITLE = (LocString) "Industrial";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class TELESCOPES
    {
      public static LocString NAME = (LocString) "Telescopes";
      public static LocString BUILDMENUTITLE = (LocString) "Telescopes";
      public static LocString TOOLTIP = (LocString) "Unlock new technologies through the power of science! {Hotkey}";
    }

    public static class MISSILES
    {
      public static LocString NAME = (LocString) "Meteor Defense";
      public static LocString BUILDMENUTITLE = (LocString) "Meteor Defense";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class FITTINGS
    {
      public static LocString NAME = (LocString) "Fittings";
      public static LocString BUILDMENUTITLE = (LocString) "Fittings";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class SANITATION
    {
      public static LocString NAME = (LocString) "Sanitation";
      public static LocString BUILDMENUTITLE = (LocString) "Sanitation";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class AUTOMATED
    {
      public static LocString NAME = (LocString) "Automated";
      public static LocString BUILDMENUTITLE = (LocString) "Automated";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class ROCKETSTRUCTURES
    {
      public static LocString NAME = (LocString) "Structural";
      public static LocString BUILDMENUTITLE = (LocString) "Structural";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class ROCKETNAV
    {
      public static LocString NAME = (LocString) "Navigation";
      public static LocString BUILDMENUTITLE = (LocString) "Navigation";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class CONDUITSENSORS
    {
      public static LocString NAME = (LocString) "Pipe Sensors";
      public static LocString BUILDMENUTITLE = (LocString) "Pipe Sensors";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class ROCKETRY
    {
      public static LocString NAME = (LocString) "Rocketry";
      public static LocString BUILDMENUTITLE = (LocString) "Rocketry";
      public static LocString TOOLTIP = (LocString) "Rocketry {Hotkey}";
    }

    public static class ENGINES
    {
      public static LocString NAME = (LocString) "Engines";
      public static LocString BUILDMENUTITLE = (LocString) "Engines";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class TANKS
    {
      public static LocString NAME = (LocString) "Tanks";
      public static LocString BUILDMENUTITLE = (LocString) "Tanks";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class CARGO
    {
      public static LocString NAME = (LocString) "Cargo";
      public static LocString BUILDMENUTITLE = (LocString) "Cargo";
      public static LocString TOOLTIP = (LocString) "";
    }

    public static class MODULE
    {
      public static LocString NAME = (LocString) "Modules";
      public static LocString BUILDMENUTITLE = (LocString) "Modules";
      public static LocString TOOLTIP = (LocString) "";
    }
  }

  public class TOOLS
  {
    public static LocString TOOL_AREA_FMT = (LocString) "{0} x {1}\n{2} tiles";
    public static LocString TOOL_LENGTH_FMT = (LocString) "{0}";
    public static LocString FILTER_HOVERCARD_HEADER = (LocString) "   <style=\"hovercard_element\">({0})</style>";
    public static LocString CAPITALS = (LocString) "<uppercase>{0}</uppercase>";

    public class SANDBOX
    {
      public class SANDBOX_TOGGLE
      {
        public static LocString NAME = (LocString) nameof (SANDBOX);
      }

      public class BRUSH
      {
        public static LocString NAME = (LocString) "Brush";
        public static LocString HOVERACTION = (LocString) "PAINT SIM";
      }

      public class SPRINKLE
      {
        public static LocString NAME = (LocString) "Sprinkle";
        public static LocString HOVERACTION = (LocString) "SPRINKLE SIM";
      }

      public class FLOOD
      {
        public static LocString NAME = (LocString) "Fill";
        public static LocString HOVERACTION = (LocString) "PAINT SECTION";
      }

      public class MARQUEE
      {
        public static LocString NAME = (LocString) "Marquee";
      }

      public class SAMPLE
      {
        public static LocString NAME = (LocString) "Sample";
        public static LocString HOVERACTION = (LocString) "COPY SELECTION";
      }

      public class HEATGUN
      {
        public static LocString NAME = (LocString) "Heat Gun";
        public static LocString HOVERACTION = (LocString) "PAINT HEAT";
      }

      public class RADSTOOL
      {
        public static LocString NAME = (LocString) "Radiation Tool";
        public static LocString HOVERACTION = (LocString) "PAINT RADS";
      }

      public class STRESSTOOL
      {
        public static LocString NAME = (LocString) "Happy Tool";
        public static LocString HOVERACTION = (LocString) "PAINT CALM";
      }

      public class SPAWNER
      {
        public static LocString NAME = (LocString) "Spawner";
        public static LocString HOVERACTION = (LocString) "SPAWN";
      }

      public class CLEAR_FLOOR
      {
        public static LocString NAME = (LocString) "Clear Floor";
        public static LocString HOVERACTION = (LocString) "DELETE DEBRIS";
      }

      public class DESTROY
      {
        public static LocString NAME = (LocString) "Destroy";
        public static LocString HOVERACTION = (LocString) "DELETE";
      }

      public class SPAWN_ENTITY
      {
        public static LocString NAME = (LocString) "Spawn";
      }

      public class FOW
      {
        public static LocString NAME = (LocString) "Reveal";
        public static LocString HOVERACTION = (LocString) "DE-FOG";
      }

      public class CRITTER
      {
        public static LocString NAME = (LocString) "Critter Removal";
        public static LocString HOVERACTION = (LocString) "DELETE CRITTERS";
      }

      public class SPAWN_STORY_TRAIT
      {
        public static LocString NAME = (LocString) "Story Trait";
        public static LocString HOVERACTION = (LocString) "PLACE";
        public static LocString ERROR_ALREADY_EXISTS = (LocString) "{StoryName} already exists in this save";
        public static LocString ERROR_INVALID_LOCATION = (LocString) "Invalid location";
        public static LocString ERROR_DUPE_HAZARD = (LocString) "One or more Duplicants are in the way";
        public static LocString ERROR_ROBOT_HAZARD = (LocString) "One or more robots are in the way";
        public static LocString ERROR_CREATURE_HAZARD = (LocString) "One or more critters are in the way";
        public static LocString ERROR_BUILDING_HAZARD = (LocString) "One or more buildings are in the way";
      }
    }

    public class GENERIC
    {
      public static LocString BACK = (LocString) "Back";
      public static LocString UNKNOWN = (LocString) nameof (UNKNOWN);
      public static LocString BUILDING_HOVER_NAME_FMT = (LocString) "{Name}    <style=\"hovercard_element\">({Element})</style>";
      public static LocString LOGIC_INPUT_HOVER_FMT = (LocString) "{Port}    <style=\"hovercard_element\">({Name})</style>";
      public static LocString LOGIC_OUTPUT_HOVER_FMT = (LocString) "{Port}    <style=\"hovercard_element\">({Name})</style>";
      public static LocString LOGIC_MULTI_INPUT_HOVER_FMT = (LocString) "{Port}    <style=\"hovercard_element\">({Name})</style>";
      public static LocString LOGIC_MULTI_OUTPUT_HOVER_FMT = (LocString) "{Port}    <style=\"hovercard_element\">({Name})</style>";
      public static LocString NATURAL_BACKWALL_LABEL = (LocString) "BACKWALL";
      public static LocString NATURAL_BACKWALL_LABEL_TITLECASE = (LocString) "Backwall";
      public static LocString BUBBLE_LABEL = (LocString) "Bubble";
    }

    public class ATTACK
    {
      public static LocString NAME = (LocString) "Attack";
      public static LocString TOOLNAME = (LocString) "Attack tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class CAPTURE
    {
      public static LocString NAME = (LocString) "Wrangle";
      public static LocString TOOLNAME = (LocString) "Wrangle tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
      public static LocString NOT_CAPTURABLE = (LocString) "Cannot Wrangle";
    }

    public class BUILD
    {
      public static LocString NAME = (LocString) "Build {0}";
      public static LocString TOOLNAME = (LocString) "Build tool";
      public static LocString TOOLACTION = (LocString) (UI.CLICK(UI.ClickType.CLICK) + " TO BUILD");
      public static LocString TOOLACTION_DRAG = (LocString) "DRAG";
    }

    public class PLACE
    {
      public static LocString NAME = (LocString) "Place {0}";
      public static LocString TOOLNAME = (LocString) "Place tool";
      public static LocString TOOLACTION = (LocString) (UI.CLICK(UI.ClickType.CLICK) + " TO PLACE");

      public class REASONS
      {
        public static LocString CAN_OCCUPY_AREA = (LocString) "Location blocked";
        public static LocString ON_FOUNDATION = (LocString) "Must place on the ground";
        public static LocString VISIBLE_TO_SPACE = (LocString) "Must have a clear path to space";
        public static LocString RESTRICT_TO_WORLD = (LocString) ("Incorrect " + (string) UI.CLUSTERMAP.PLANETOID);
      }
    }

    public class MOVETOLOCATION
    {
      public static LocString NAME = (LocString) "Relocate";
      public static LocString TOOLNAME = (LocString) "Relocate Tool";
      public static LocString TOOLACTION = (LocString) (UI.CLICK(UI.ClickType.CLICK) ?? "");
      public static LocString UNREACHABLE = (LocString) nameof (UNREACHABLE);
    }

    public class COPYSETTINGS
    {
      public static LocString NAME = (LocString) "Paste Settings";
      public static LocString TOOLNAME = (LocString) "Paste Settings Tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class DIG
    {
      public static LocString NAME = (LocString) "Dig";
      public static LocString TOOLNAME = (LocString) "Dig tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class DISINFECT
    {
      public static LocString NAME = (LocString) "Disinfect";
      public static LocString TOOLNAME = (LocString) "Disinfect tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class DISCONNECT
    {
      public static LocString NAME = (LocString) "Disconnect";
      public static LocString TOOLTIP = (LocString) "Sever conduits and connectors {Hotkey}";
      public static LocString TOOLNAME = (LocString) "Disconnect tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class CANCEL
    {
      public static LocString NAME = (LocString) "Cancel";
      public static LocString TOOLNAME = (LocString) "Cancel tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class DECONSTRUCT
    {
      public static LocString NAME = (LocString) "Deconstruct";
      public static LocString TOOLNAME = (LocString) "Deconstruct tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class CLEANUPCATEGORY
    {
      public static LocString NAME = (LocString) "Clean";
      public static LocString TOOLNAME = (LocString) "Clean Up tools";
    }

    public class PRIORITIESCATEGORY
    {
      public static LocString NAME = (LocString) "Priority";
    }

    public class MARKFORSTORAGE
    {
      public static LocString NAME = (LocString) "Sweep";
      public static LocString TOOLNAME = (LocString) "Sweep tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class MOP
    {
      public static LocString NAME = (LocString) "Mop";
      public static LocString TOOLNAME = (LocString) "Mop tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
      public static LocString TOO_MUCH_LIQUID = (LocString) "Too Much Liquid";
      public static LocString NOT_ON_FLOOR = (LocString) "Not On Floor";
    }

    public class HARVEST
    {
      public static LocString NAME = (LocString) "Harvest";
      public static LocString TOOLNAME = (LocString) "Harvest tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class PRIORITIZE
    {
      public static LocString NAME = (LocString) "Priority";
      public static LocString TOOLNAME = (LocString) "Priority tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
      public static LocString SPECIFIC_PRIORITY = (LocString) "Set Priority: {0}";
    }

    public class EMPTY_PIPE
    {
      public static LocString NAME = (LocString) "Empty Pipe";
      public static LocString TOOLTIP = (LocString) "Extract pipe contents {Hotkey}";
      public static LocString TOOLNAME = (LocString) "Empty Pipe tool";
      public static LocString TOOLACTION = (LocString) "DRAG";
    }

    public class FILTERSCREEN
    {
      public static LocString OPTIONS = (LocString) "Tool Filter";
    }

    public class FILTERLAYERS
    {
      public class BUILDINGS
      {
        public static LocString NAME = (LocString) "Buildings";
        public static LocString TOOLTIP = (LocString) "All buildings";
      }

      public class TILES
      {
        public static LocString NAME = (LocString) "Tiles";
        public static LocString TOOLTIP = (LocString) "Tiles only";
      }

      public class WIRES
      {
        public static LocString NAME = (LocString) "Power Wires";
        public static LocString TOOLTIP = (LocString) "Power wires only";
      }

      public class SOLIDCONDUITS
      {
        public static LocString NAME = (LocString) "Conveyor Rails";
        public static LocString TOOLTIP = (LocString) "Conveyor rails only";
      }

      public class DIGPLACER
      {
        public static LocString NAME = (LocString) "Dig Orders";
        public static LocString TOOLTIP = (LocString) "Dig orders only";
      }

      public class CLEANANDCLEAR
      {
        public static LocString NAME = (LocString) "Sweep & Mop Orders";
        public static LocString TOOLTIP = (LocString) "Sweep and mop orders only";
      }

      public class HARVEST_WHEN_READY
      {
        public static LocString NAME = (LocString) "Enable Harvest";
        public static LocString TOOLTIP = (LocString) "Enable harvest on selected plants";
      }

      public class DO_NOT_HARVEST
      {
        public static LocString NAME = (LocString) "Disable Harvest";
        public static LocString TOOLTIP = (LocString) "Disable harvest on selected plants";
      }

      public class ATTACK
      {
        public static LocString NAME = (LocString) "Attack";
        public static LocString TOOLTIP = (LocString) "";
      }

      public class LOGIC
      {
        public static LocString NAME = (LocString) "Automation";
        public static LocString TOOLTIP = (LocString) "Automation buildings only";
      }

      public class BACKWALL
      {
        public static LocString NAME = (LocString) "Background Buildings";
        public static LocString TOOLTIP = (LocString) "Background buildings only";
      }

      public class NATURALBACKWALL
      {
        public static LocString NAME = (LocString) "Natural Backwall";
        public static LocString TOOLTIP = (LocString) "Natural backwall tiles only";
      }

      public class UPROOTPLANTS
      {
        public static LocString NAME = (LocString) "Plants";
        public static LocString TOOLTIP = (LocString) "Uproot plants only";
      }

      public class LIQUIDPIPES
      {
        public static LocString NAME = (LocString) "Liquid Pipes";
        public static LocString TOOLTIP = (LocString) "Liquid pipes only";
      }

      public class GASPIPES
      {
        public static LocString NAME = (LocString) "Gas Pipes";
        public static LocString TOOLTIP = (LocString) "Gas pipes only";
      }

      public class ALL
      {
        public static LocString NAME = (LocString) "All";
        public static LocString TOOLTIP = (LocString) "Target all";
      }

      public class ALL_OVERLAY
      {
        public static LocString NAME = (LocString) "All";
        public static LocString TOOLTIP = (LocString) "Show all";
      }

      public class METAL
      {
        public static LocString NAME = (LocString) "Metal";
        public static LocString TOOLTIP = (LocString) "Show only metals";
      }

      public class BUILDABLE
      {
        public static LocString NAME = (LocString) "Mineral";
        public static LocString TOOLTIP = (LocString) "Show only minerals";
      }

      public class FILTER
      {
        public static LocString NAME = (LocString) "Filtration Medium";
        public static LocString TOOLTIP = (LocString) "Show only filtration mediums";
      }

      public class CONSUMABLEORE
      {
        public static LocString NAME = (LocString) "Consumable Ore";
        public static LocString TOOLTIP = (LocString) "Show only consumable ore";
      }

      public class ORGANICS
      {
        public static LocString NAME = (LocString) "Organic";
        public static LocString TOOLTIP = (LocString) "Show only organic materials";
      }

      public class FARMABLE
      {
        public static LocString NAME = (LocString) "Cultivable Soil";
        public static LocString TOOLTIP = (LocString) "Show only cultivable soil";
      }

      public class LIQUIFIABLE
      {
        public static LocString NAME = (LocString) "Liquefiable";
        public static LocString TOOLTIP = (LocString) "Show only liquefiable elements";
      }

      public class GAS
      {
        public static LocString NAME = (LocString) "Gas";
        public static LocString TOOLTIP = (LocString) "Show only gases";
      }

      public class LIQUID
      {
        public static LocString NAME = (LocString) "Liquid";
        public static LocString TOOLTIP = (LocString) "Show only liquids";
      }

      public class MISC
      {
        public static LocString NAME = (LocString) "Miscellaneous";
        public static LocString TOOLTIP = (LocString) "Show only miscellaneous elements";
      }

      public class ABSOLUTETEMPERATURE
      {
        public static LocString NAME = (LocString) "Absolute Temperature";
        public static LocString TOOLTIP = (LocString) "<b>Absolute Temperature</b>\nView the default temperature ranges and categories relative to absolute zero";
      }

      public class RELATIVETEMPERATURE
      {
        public static LocString NAME = (LocString) "Relative Temperature";
        public static LocString TOOLTIP = (LocString) "<b>Relative Temperature</b>\nCustomize visual map to identify temperatures relative to a selected midpoint\n\nDrag the slider to adjust the relative temperature range";
      }

      public class HEATFLOW
      {
        public static LocString NAME = (LocString) "Thermal Tolerance";
        public static LocString TOOLTIP = (LocString) "<b>Thermal Tolerance</b>\nView the impact of ambient temperatures on living beings";
      }

      public class STATECHANGE
      {
        public static LocString NAME = (LocString) "State Change";
        public static LocString TOOLTIP = (LocString) "<b>State Change</b>\nView the impact of ambient temperatures on element states";
      }

      public class BREATHABLE
      {
        public static LocString NAME = (LocString) "Breathable Gas";
        public static LocString TOOLTIP = (LocString) "Show only breathable gases";
      }

      public class UNBREATHABLE
      {
        public static LocString NAME = (LocString) "Unbreathable Gas";
        public static LocString TOOLTIP = (LocString) "Show only unbreathable gases";
      }

      public class AGRICULTURE
      {
        public static LocString NAME = (LocString) "Agriculture";
        public static LocString TOOLTIP = (LocString) "";
      }

      public class ADAPTIVETEMPERATURE
      {
        public static LocString NAME = (LocString) "Adapt. Temperature";
        public static LocString TOOLTIP = (LocString) "";
      }

      public class CONSTRUCTION
      {
        public static LocString NAME = (LocString) "Construction";
        public static LocString TOOLTIP = (LocString) $"Target {UI.PRE_KEYWORD}Construction{UI.PST_KEYWORD} errands only";
      }

      public class DIG
      {
        public static LocString NAME = (LocString) "Digging";
        public static LocString TOOLTIP = (LocString) $"Target {UI.PRE_KEYWORD}Digging{UI.PST_KEYWORD} errands only";
      }

      public class CLEAN
      {
        public static LocString NAME = (LocString) "Cleaning";
        public static LocString TOOLTIP = (LocString) "Target cleaning errands only";
      }

      public class OPERATE
      {
        public static LocString NAME = (LocString) "Duties";
        public static LocString TOOLTIP = (LocString) "Target general duties only";
      }
    }
  }

  public class DETAILTABS
  {
    public class TEXTICONDATA
    {
      public class REFRIGERATED
      {
        public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_refrigerated\">";
        public static LocString NAME = (LocString) "Refrigerated";
        public static LocString TOOLTIP = (LocString) $"Ideal {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} storage is slowing this food's {UI.PRE_KEYWORD}Decay Rate{UI.PST_KEYWORD}\n\nCold environments can also mimic refrigeration";
      }

      public class DEEPFROZEN
      {
        public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_frozen\">";
        public static LocString NAME = (LocString) "Deep Freeze";
        public static LocString TOOLTIP = (LocString) $"Extremely low {UI.PRE_KEYWORD}Temperatures{UI.PST_KEYWORD} are greatly prolonging the shelf-life of this food\n\nCold environments may also result in frozen food";
      }

      public class FRESH
      {
        public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_fresh\">";
        public static LocString NAME = (LocString) "Fresh";
        public static LocString TOOLTIP = (LocString) "This food is fresh";
      }

      public class STALE
      {
        public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_stale\">";
        public static LocString NAME = (LocString) "Stale";
        public static LocString TOOLTIP = (LocString) "This food is still edible, but will soon expire";
      }

      public class SPICEDFOOD
      {
        public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_spiced\">";
        public static LocString NAME = (LocString) "Seasoned";
        public static LocString TOOLTIP = (LocString) ("This food has been improved with spice from the " + (string) BUILDINGS.PREFABS.SPICEGRINDER.NAME);
      }

      public class GERMS
      {
        public static readonly string ICON = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_germs\">";
        public static LocString NAME = (LocString) "{0} Germs";
        public static LocString TOOLTIP = (LocString) "Contaminated with {0} germs";
      }
    }

    public class STATS
    {
      public static LocString NAME = (LocString) "Skills";
      public static LocString TOOLTIP = (LocString) "<b>Skills</b>\nView this Duplicant's resume and attributes";
      public static LocString GROUPNAME_ATTRIBUTES = (LocString) "ATTRIBUTES";
      public static LocString GROUPNAME_STRESS = (LocString) "TODAY'S STRESS";
      public static LocString GROUPNAME_EXPECTATIONS = (LocString) "EXPECTATIONS";
      public static LocString GROUPNAME_TRAITS = (LocString) "TRAITS";
    }

    public class SIMPLEINFO
    {
      public static LocString NAME = (LocString) "Status";
      public static LocString TOOLTIP = (LocString) "<b>Status</b>\nView current status";
      public static LocString GROUPNAME_STATUS = (LocString) "STATUS";
      public static LocString GROUPNAME_DESCRIPTION = (LocString) "INFORMATION";
      public static LocString GROUPNAME_CONDITION = (LocString) "CONDITION";
      public static LocString GROUPNAME_REQUIREMENTS = (LocString) "REQUIREMENTS";
      public static LocString GROUPNAME_EFFECTS = (LocString) "EFFECTS";
      public static LocString GROUPNAME_RESEARCH = (LocString) "RESEARCH";
      public static LocString GROUPNAME_LORE = (LocString) "RECOVERED FILES";
      public static LocString GROUPNAME_FERTILITY = (LocString) "EGG CHANCES";
      public static LocString GROUPNAME_MOO_FERTILITY = (LocString) "SONG CHANCES";
      public static LocString GROUPNAME_ROCKET = (LocString) "ROCKETRY";
      public static LocString GROUPNAME_CARGOBAY = (LocString) "CARGO BAYS";
      public static LocString GROUPNAME_ELEMENTS = (LocString) "RESOURCES";
      public static LocString GROUPNAME_LIFE = (LocString) "LIFEFORMS";
      public static LocString GROUPNAME_BIOMES = (LocString) "BIOMES";
      public static LocString GROUPNAME_GEYSERS = (LocString) "GEYSERS";
      public static LocString GROUPNAME_METEORSHOWERS = (LocString) "METEOR SHOWERS";
      public static LocString GROUPNAME_WORLDTRAITS = (LocString) "WORLD TRAITS";
      public static LocString GROUPNAME_CLUSTER_POI = (LocString) "POINT OF INTEREST";
      public static LocString GROUPNAME_MOVABLE = (LocString) "MOVING";
      public static LocString NO_METEORSHOWERS = (LocString) "No meteor showers forecasted";
      public static LocString NO_GEYSERS = (LocString) "No geysers detected";
      public static LocString UNKNOWN_GEYSERS = (LocString) "Unknown Geysers ({num})";
    }

    public class DETAILS
    {
      public static LocString NAME = (LocString) "Properties";
      public static LocString MINION_NAME = (LocString) "About";
      public static LocString TOOLTIP = (LocString) "<b>Properties</b>\nView elements, temperature, germs and more";
      public static LocString MINION_TOOLTIP = (LocString) "More information";
      public static LocString GROUPNAME_DETAILS = (LocString) nameof (DETAILS);
      public static LocString GROUPNAME_CONTENTS = (LocString) "CONTENTS";
      public static LocString GROUPNAME_MINION_CONTENTS = (LocString) "CARRIED ITEMS";
      public static LocString STORAGE_EMPTY = (LocString) "None";
      public static LocString CONTENTS_MASS = (LocString) "{0}: {1}";
      public static LocString CONTENTS_TEMPERATURE = (LocString) "{0} at {1}";
      public static LocString CONTENTS_ROTTABLE = (LocString) "\n • {0}";
      public static LocString CONTENTS_DISEASED = (LocString) "\n • {0}";
      public static LocString NET_STRESS = (LocString) "<b>Today's Net Stress: {0}%</b>";

      public class RADIATIONABSORPTIONFACTOR
      {
        public static LocString NAME = (LocString) "Radiation Blocking: {0}";
        public static LocString TOOLTIP = (LocString) "This object will block approximately {0} of radiation.";
      }
    }

    public class PERSONALITY
    {
      public static LocString NAME = (LocString) "Bio";
      public static LocString TOOLTIP = (LocString) "<b>Bio</b>\nView this Duplicant's personality, skills, traits and amenities";
      public static LocString GROUPNAME_BIO = (LocString) "ABOUT";
      public static LocString GROUPNAME_RESUME = (LocString) "{0}'S RESUME";

      public class RESUME
      {
        public static LocString MASTERED_SKILLS = (LocString) "<b><size=13>Learned Skills:</size></b>";
        public static LocString MASTERED_SKILLS_TOOLTIP = (LocString) $"All {UI.PRE_KEYWORD}Traits{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Morale Needs{UI.PST_KEYWORD} become permanent once a Duplicant has learned a new {UI.PRE_KEYWORD}Skill{UI.PST_KEYWORD}\n\n{(string) BUILDINGS.PREFABS.RESETSKILLSSTATION.NAME}s can be built from the {UI.FormatAsBuildMenuTab("Stations Tab", Action.Plan10)} to completely reset a Duplicant's learned {UI.PRE_KEYWORD}Skills{UI.PST_KEYWORD}, refunding all {UI.PRE_KEYWORD}Skill Points{UI.PST_KEYWORD}";
        public static LocString JOBTRAINING_TOOLTIP = (LocString) $"{{0}} learned this {UI.PRE_KEYWORD}Skill{UI.PST_KEYWORD} while working as a {{1}}";

        public class APTITUDES
        {
          public static LocString NAME = (LocString) "<b><size=13>Personal Interests:</size></b>";
          public static LocString TOOLTIP = (LocString) "{0} enjoys these types of work";
        }

        public class PERKS
        {
          public static LocString NAME = (LocString) "<b><size=13>Skill Training:</size></b>";
          public static LocString TOOLTIP = (LocString) "These are permanent skills {0} gained from learned skills";
        }

        public class CURRENT_ROLE
        {
          public static LocString NAME = (LocString) "<size=13><b>Current Job:</b> {0}</size>";
          public static LocString TOOLTIP = (LocString) "{0} is currently working as a {1}";
          public static LocString NOJOB_TOOLTIP = (LocString) "This {0} is... \"between jobs\" at present";
        }

        public class NO_MASTERED_SKILLS
        {
          public static LocString NAME = (LocString) "None";
          public static LocString TOOLTIP = (LocString) $"{{0}} has not learned any {UI.PRE_KEYWORD}Skills{UI.PST_KEYWORD} yet";
        }
      }

      public class EQUIPMENT
      {
        public static LocString GROUPNAME_ROOMS = (LocString) "AMENITIES";
        public static LocString GROUPNAME_OWNABLE = (LocString) nameof (EQUIPMENT);
        public static LocString NO_ASSIGNABLES = (LocString) "None";
        public static LocString NO_ASSIGNABLES_TOOLTIP = (LocString) "{0} has not been assigned any buildings of their own";
        public static LocString UNASSIGNED = (LocString) "Unassigned";
        public static LocString UNASSIGNED_TOOLTIP = (LocString) "This Duplicant has not been assigned a {0}";
        public static LocString ASSIGNED_TOOLTIP = (LocString) "{2} has been assigned a {0}\n\nEffects: {1}";
        public static LocString NOEQUIPMENT = (LocString) "None";
        public static LocString NOEQUIPMENT_TOOLTIP = (LocString) "{0}'s wearing their Printday Suit and nothing more";
      }
    }

    public class ENERGYCONSUMER
    {
      public static LocString NAME = (LocString) "Energy";
      public static LocString TOOLTIP = (LocString) "View how much power this building consumes";
    }

    public class ENERGYWIRE
    {
      public static LocString NAME = (LocString) "Energy";
      public static LocString TOOLTIP = (LocString) "View this wire's network";
    }

    public class ENERGYGENERATOR
    {
      public static LocString NAME = (LocString) "Energy";
      public static LocString TOOLTIP = (LocString) "<b>Energy</b>\nMonitor the power this building is generating";
      public static LocString CIRCUITOVERVIEW = (LocString) "CIRCUIT OVERVIEW";
      public static LocString GENERATORS = (LocString) "POWER GENERATORS";
      public static LocString CONSUMERS = (LocString) "POWER CONSUMERS";
      public static LocString BATTERIES = (LocString) nameof (BATTERIES);
      public static LocString DISCONNECTED = (LocString) "Not connected to an electrical circuit";
      public static LocString NOGENERATORS = (LocString) "No generators on this circuit";
      public static LocString NOCONSUMERS = (LocString) "No consumers on this circuit";
      public static LocString NOBATTERIES = (LocString) "No batteries on this circuit";
      public static LocString AVAILABLE_JOULES = (LocString) (UI.FormatAsLink("Power", "POWER") + " stored: {0}");
      public static LocString AVAILABLE_JOULES_TOOLTIP = (LocString) "Amount of power stored in batteries";
      public static LocString WATTAGE_GENERATED = (LocString) (UI.FormatAsLink("Power", "POWER") + " produced: {0}");
      public static LocString WATTAGE_GENERATED_TOOLTIP = (LocString) "The total amount of power generated by this circuit";
      public static LocString WATTAGE_CONSUMED = (LocString) (UI.FormatAsLink("Power", "POWER") + " consumed: {0}");
      public static LocString WATTAGE_CONSUMED_TOOLTIP = (LocString) "The total amount of power used by this circuit";
      public static LocString POTENTIAL_WATTAGE_CONSUMED = (LocString) "Potential power consumed: {0}";
      public static LocString POTENTIAL_WATTAGE_CONSUMED_TOOLTIP = (LocString) "The total amount of power that can be used by this circuit if all connected buildings are active";
      public static LocString MAX_SAFE_WATTAGE = (LocString) "Maximum safe wattage: {0}";
      public static LocString MAX_SAFE_WATTAGE_TOOLTIP = (LocString) "Exceeding this value will overload the circuit and can result in damage to wiring and buildings";
    }

    public class DISEASE
    {
      public static LocString NAME = (LocString) "Germs";
      public static LocString TOOLTIP = (LocString) "<b>Germs</b>\nView germ resistance and risk of contagion";
      public static LocString DISEASE_SOURCE = (LocString) "DISEASE SOURCE";
      public static LocString IMMUNE_SYSTEM = (LocString) "GERM HOST";
      public static LocString CONTRACTION_RATES = (LocString) "CONTRACTION RATES";
      public static LocString CURRENT_GERMS = (LocString) "SURFACE GERMS";
      public static LocString NO_CURRENT_GERMS = (LocString) "SURFACE GERMS";
      public static LocString GERMS_INFO = (LocString) "GERM LIFE CYCLE";
      public static LocString INFECTION_INFO = (LocString) "INFECTION DETAILS";
      public static LocString DISEASE_INFO_POPUP_HEADER = (LocString) "DISEASE INFO: {0}";
      public static LocString DISEASE_INFO_POPUP_BUTTON = (LocString) "FULL INFO";
      public static LocString DISEASE_INFO_POPUP_TOOLTIP = (LocString) "View detailed germ and infection info for {0}";

      public class DETAILS
      {
        public static LocString NODISEASE = (LocString) "No surface germs";
        public static LocString NODISEASE_TOOLTIP = (LocString) "There are no germs present on this object";
        public static LocString DISEASE_AMOUNT = (LocString) "{0}: {1}";
        public static LocString DISEASE_AMOUNT_TOOLTIP = (LocString) "{0} are present on the surface of the selected object";
        public static LocString DEATH_FORMAT = (LocString) "{0} dead/cycle";
        public static LocString DEATH_FORMAT_TOOLTIP = (LocString) "Germ count is being reduced by {0}/cycle";
        public static LocString GROWTH_FORMAT = (LocString) "{0} spawned/cycle";
        public static LocString GROWTH_FORMAT_TOOLTIP = (LocString) "Germ count is being increased by {0}/cycle";
        public static LocString NEUTRAL_FORMAT = (LocString) "No change";
        public static LocString NEUTRAL_FORMAT_TOOLTIP = (LocString) "Germ count is static";

        public class GROWTH_FACTORS
        {
          public static LocString TITLE = (LocString) "\nGrowth factors:";
          public static LocString TOOLTIP = (LocString) "These conditions are contributing to the multiplication of germs";
          public static LocString RATE_OF_CHANGE = (LocString) "Change rate: {0}";
          public static LocString RATE_OF_CHANGE_TOOLTIP = (LocString) "Germ count is fluctuating at a rate of {0}";
          public static LocString HALF_LIFE_NEG = (LocString) "Half life: {0}";
          public static LocString HALF_LIFE_NEG_TOOLTIP = (LocString) "In {0} the germ count on this object will be halved";
          public static LocString HALF_LIFE_POS = (LocString) "Doubling time: {0}";
          public static LocString HALF_LIFE_POS_TOOLTIP = (LocString) "In {0} the germ count on this object will be doubled";
          public static LocString HALF_LIFE_NEUTRAL = (LocString) "Static";
          public static LocString HALF_LIFE_NEUTRAL_TOOLTIP = (LocString) "The germ count is neither increasing nor decreasing";

          public class SUBSTRATE
          {
            public static LocString GROW = (LocString) "    • Growing on {0}: {1}";
            public static LocString GROW_TOOLTIP = (LocString) "Contact with this substance is causing germs to multiply";
            public static LocString NEUTRAL = (LocString) "    • No change on {0}";
            public static LocString NEUTRAL_TOOLTIP = (LocString) "Contact with this substance has no effect on germ count";
            public static LocString DIE = (LocString) "    • Dying on {0}: {1}";
            public static LocString DIE_TOOLTIP = (LocString) "Contact with this substance is causing germs to die off";
          }

          public class ENVIRONMENT
          {
            public static LocString TITLE = (LocString) "    • Surrounded by {0}: {1}";
            public static LocString GROW_TOOLTIP = (LocString) "This atmosphere is causing germs to multiply";
            public static LocString DIE_TOOLTIP = (LocString) "This atmosphere is causing germs to die off";
          }

          public class TEMPERATURE
          {
            public static LocString TITLE = (LocString) "    • Current temperature {0}: {1}";
            public static LocString GROW_TOOLTIP = (LocString) "This temperature is allowing germs to multiply";
            public static LocString DIE_TOOLTIP = (LocString) "This temperature is causing germs to die off";
          }

          public class PRESSURE
          {
            public static LocString TITLE = (LocString) "    • Current pressure {0}: {1}";
            public static LocString GROW_TOOLTIP = (LocString) "Atmospheric pressure is causing germs to multiply";
            public static LocString DIE_TOOLTIP = (LocString) "Atmospheric pressure is causing germs to die off";
          }

          public class RADIATION
          {
            public static LocString TITLE = (LocString) "    • Exposed to {0} Rads: {1}";
            public static LocString DIE_TOOLTIP = (LocString) "Radiation exposure is causing germs to die off";
          }

          public class DYING_OFF
          {
            public static LocString TITLE = (LocString) "    • <b>Dying off: {0}</b>";
            public static LocString TOOLTIP = (LocString) $"Low germ count in this area is causing germs to die rapidly\n\nFewer than {{0}} are on this {{1}} of material.\n({{2}} germs/{(string) UI.UNITSUFFIXES.MASS.KILOGRAM})";
          }

          public class OVERPOPULATED
          {
            public static LocString TITLE = (LocString) "    • <b>Overpopulated: {0}</b>";
            public static LocString TOOLTIP = (LocString) $"Too many germs are present in this area, resulting in rapid die-off until the population stabilizes\n\nA maximum of {{0}} can be on this {{1}} of material.\n({{2}} germs/{(string) UI.UNITSUFFIXES.MASS.KILOGRAM})";
          }
        }
      }
    }

    public class NEEDS
    {
      public static LocString NAME = (LocString) "Stress";
      public static LocString TOOLTIP = (LocString) "View this Duplicant's psychological status";
      public static LocString CURRENT_STRESS_LEVEL = (LocString) $"Current {UI.FormatAsLink("Stress", "STRESS")} Level: {{0}}";
      public static LocString OVERVIEW = (LocString) "Overview";
      public static LocString STRESS_CREATORS = (LocString) (UI.FormatAsLink("Stress", "STRESS") + " Creators");
      public static LocString STRESS_RELIEVERS = (LocString) (UI.FormatAsLink("Stress", "STRESS") + " Relievers");
      public static LocString CURRENT_NEED_LEVEL = (LocString) "Current Level: {0}";
      public static LocString NEXT_NEED_LEVEL = (LocString) "Next Level: {0}";
    }

    public class EGG_CHANCES
    {
      public static LocString CHANCE_FORMAT = (LocString) "{0}: {1}";
      public static LocString CHANCE_FORMAT_TOOLTIP = (LocString) "This critter has a {1} chance of laying {0}s.\n\nThis probability increases when the creature:\n{2}";
      public static LocString CHANCE_MOD_FORMAT = (LocString) "    • {0}\n";
      public static LocString CHANCE_FORMAT_TOOLTIP_NOMOD = (LocString) "This critter has a {1} chance of laying {0}s.";
    }

    public class MOO_SONG_CHANCES
    {
      public static LocString CHANCE_FORMAT = (LocString) "{0}: {1}";
      public static LocString CHANCE_FORMAT_TOOLTIP = (LocString) "This critter has a {1} chance of calling {0}s\n\nThis probability increases when the creature:\n{2}";
      public static LocString CHANCE_MOD_FORMAT = (LocString) "    • {0}\n";
      public static LocString CHANCE_FORMAT_TOOLTIP_NOMOD = (LocString) "This critter has a {1} chance of calling {0}s";
    }

    public class BUILDING_CHORES
    {
      public static LocString NAME = (LocString) "Errands";
      public static LocString TOOLTIP = (LocString) "<b>Errands</b>\nView available errands and current queue";
      public static LocString CHORE_TYPE_TOOLTIP = (LocString) "Errand Type: {0}";
      public static LocString AVAILABLE_CHORES = (LocString) "AVAILABLE ERRANDS";
      public static LocString DUPE_TOOLTIP_FAILED = (LocString) "{Name} cannot currently {Errand}\n\nReason:\n{FailedPrecondition}";
      public static LocString DUPE_TOOLTIP_SUCCEEDED = (LocString) "{Description}\n\n{Errand}'s Type: {Groups}\n\n{Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n{Building} Priority: {BuildingPriority}\nAll {BestGroup} Errands: {TypePriority}\n\nTotal Priority: {TotalPriority}";
      public static LocString DUPE_TOOLTIP_DESC_ACTIVE = (LocString) "{Name} is currently busy: \"{Errand}\"";
      public static LocString DUPE_TOOLTIP_DESC_INACTIVE = (LocString) "\"{Errand}\" is #{Rank} on {Name}'s To Do list, after they finish their current errand";
    }

    public class PROCESS_CONDITIONS
    {
      public static LocString NAME = (LocString) "LAUNCH CHECKLIST";
      public static LocString ROCKETPREP = (LocString) "Rocket Construction";
      public static LocString ROCKETPREP_TOOLTIP = (LocString) "It is recommended that all boxes on the Rocket Construction checklist be ticked before launching";
      public static LocString ROCKETSTORAGE = (LocString) "Cargo Manifest";
      public static LocString ROCKETSTORAGE_TOOLTIP = (LocString) "It is recommended that all boxes on the Cargo Manifest checklist be ticked before launching";
      public static LocString ROCKETFLIGHT = (LocString) "Flight Route";
      public static LocString ROCKETFLIGHT_TOOLTIP = (LocString) "A rocket requires a clear path to a set destination to conduct a mission";
      public static LocString ROCKETBOARD = (LocString) "Crew Manifest";
      public static LocString ROCKETBOARD_TOOLTIP = (LocString) "It is recommended that all boxes on the Crew Manifest checklist be ticked before launching";
      public static LocString ALL = (LocString) "Requirements";
      public static LocString ALL_TOOLTIP = (LocString) "These conditions must be fulfilled in order to launch a rocket mission";
    }

    public class COSMETICS
    {
      public static LocString NAME = (LocString) "Blueprint";
      public static LocString TOOLTIP = (LocString) "<b>Blueprint</b>\nView and change assigned blueprints";
    }

    public class MATERIAL
    {
      public static LocString NAME = (LocString) "Material";
      public static LocString TOOLTIP = (LocString) "<b>Material</b>\nView and change this building's construction material";
      public static LocString SUB_HEADER_CURRENT_MATERIAL = (LocString) "CURRENT MATERIAL";
      public static LocString BUTTON_CHANGE_MATERIAL = (LocString) "Change Material";
    }

    public class CONFIGURATION
    {
      public static LocString NAME = (LocString) "Config";
      public static LocString TOOLTIP = (LocString) "<b>Config</b>\nView and change filters, recipes, production orders and more";
      public static LocString TOOLTIP_DUPLICANT = (LocString) "<b>Config</b>\nView and change assigned equipment and amenities";
    }
  }

  public class BUILDMENU
  {
    public static LocString GRID_VIEW_TOGGLE_TOOLTIP = (LocString) "Toggle Grid View";
    public static LocString LIST_VIEW_TOGGLE_TOOLTIP = (LocString) "Toggle List View";
    public static LocString NO_SEARCH_RESULTS = (LocString) "NO RESULTS FOUND";
    public static LocString SEARCH_RESULTS_HEADER = (LocString) "SEARCH RESULTS";
    public static LocString SEARCH_TEXT_PLACEHOLDER = (LocString) "Search all buildings...";
    public static LocString SEARCH_TOOLTIP = (LocString) "Search all buildings {Hotkey}";
    public static LocString CLEAR_SEARCH_TOOLTIP = (LocString) "Clear search";
  }

  public class BUILDINGEFFECTS
  {
    public static LocString OPERATIONREQUIREMENTS = (LocString) "<b>Requirements:</b>";
    public static LocString OPERATIONINPUTS = (LocString) "Inputs:";
    public static LocString REQUIRESPOWER = (LocString) (UI.FormatAsLink("Power", "POWER") + ": {0}");
    public static LocString REQUIRESELEMENT = (LocString) "Supply of {0}";
    public static LocString REQUIRESLIQUIDINPUT = (LocString) UI.FormatAsLink("Liquid Intake Pipe", "LIQUIDPIPING");
    public static LocString REQUIRESLIQUIDOUTPUT = (LocString) UI.FormatAsLink("Liquid Output Pipe", "LIQUIDPIPING");
    public static LocString REQUIRESLIQUIDOUTPUTS = (LocString) ("Two " + UI.FormatAsLink("Liquid Output Pipes", "LIQUIDPIPING"));
    public static LocString REQUIRESGASINPUT = (LocString) UI.FormatAsLink("Gas Intake Pipe", "GASPIPING");
    public static LocString REQUIRESGASOUTPUT = (LocString) UI.FormatAsLink("Gas Output Pipe", "GASPIPING");
    public static LocString REQUIRESGASOUTPUTS = (LocString) ("Two " + UI.FormatAsLink("Gas Output Pipes", "GASPIPING"));
    public static LocString REQUIRESMANUALOPERATION = (LocString) "Duplicant operation";
    public static LocString REQUIRESSKILLEDOPERATION = (LocString) "Skilled Duplicant operation";
    public static LocString REQUIRESSKILLEDOPERATION_DLC3 = (LocString) "Skilled Duplicant operation";
    public static LocString REQUIRESCREATIVITY = (LocString) ("Duplicant " + UI.FormatAsLink("Creativity", "ARTING1"));
    public static LocString REQUIRESPOWERGENERATOR = (LocString) (UI.FormatAsLink("Power", "POWER") + " generator");
    public static LocString REQUIRESSEED = (LocString) ("1 Unplanted " + UI.FormatAsLink("Seed", "PLANTS"));
    public static LocString PREFERS_ROOM = (LocString) "Preferred Room: {0}";
    public static LocString REQUIRESROOM = (LocString) "Dedicated Room: {0}";
    public static LocString ALLOWS_FERTILIZER = (LocString) ("Plant " + UI.FormatAsLink("Fertilization", "WILTCONDITIONS"));
    public static LocString ALLOWS_IRRIGATION = (LocString) ("Plant " + UI.FormatAsLink("Liquid", "WILTCONDITIONS"));
    public static LocString ASSIGNEDDUPLICANT = (LocString) "Duplicant assignment";
    public static LocString CONSUMESANYELEMENT = (LocString) "Any Element";
    public static LocString ENABLESDOMESTICGROWTH = (LocString) ("Enables " + UI.FormatAsLink("Plant Domestication", "PLANTS"));
    public static LocString TRANSFORMER_INPUT_WIRE = (LocString) ("Input " + UI.FormatAsLink("Power Wire", "WIRE"));
    public static LocString TRANSFORMER_OUTPUT_WIRE = (LocString) $"Output {UI.FormatAsLink("Power Wire", "WIRE")} (Limited to {{0}})";
    public static LocString OPERATIONEFFECTS = (LocString) "<b>Effects:</b>";
    public static LocString BATTERYCAPACITY = (LocString) (UI.FormatAsLink("Power", "POWER") + " capacity: {0}");
    public static LocString BATTERYLEAK = (LocString) (UI.FormatAsLink("Power", "POWER") + " leak: {0}");
    public static LocString STORAGECAPACITY = (LocString) "Storage capacity: {0}";
    public static LocString ELEMENTEMITTED_INPUTTEMP = (LocString) "{0}: {1}";
    public static LocString ELEMENTEMITTED_ENTITYTEMP = (LocString) "{0}: {1}";
    public static LocString ELEMENTEMITTED_MINORENTITYTEMP = (LocString) "{0}: {1}";
    public static LocString ELEMENTEMITTED_MINTEMP = (LocString) "{0}: {1}";
    public static LocString ELEMENTEMITTED_FIXEDTEMP = (LocString) "{0}: {1}";
    public static LocString ELEMENTCONSUMED = (LocString) "{0}: {1}";
    public static LocString ELEMENTEMITTED_TOILET = (LocString) "{0}: {1} per use";
    public static LocString ELEMENTEMITTEDPERUSE = (LocString) "{0}: {1} per use";
    public static LocString DISEASEEMITTEDPERUSE = (LocString) "{0}: {1} per use";
    public static LocString DISEASECONSUMEDPERUSE = (LocString) "All Diseases: -{0} per use";
    public static LocString ELEMENTCONSUMEDPERUSE = (LocString) "{0}: {1} per use";
    public static LocString ENERGYCONSUMED = (LocString) (UI.FormatAsLink("Power", "POWER") + " consumed: {0}");
    public static LocString ENERGYGENERATED = (LocString) (UI.FormatAsLink("Power", "POWER") + ": +{0}");
    public static LocString HEATGENERATED = (LocString) (UI.FormatAsLink("Heat", "HEAT") + ": +{0}/s");
    public static LocString HEATCONSUMED = (LocString) (UI.FormatAsLink("Heat", "HEAT") + ": -{0}/s");
    public static LocString HEATER_TARGETTEMPERATURE = (LocString) $"Target {UI.FormatAsLink("Temperature", "HEAT")}: {{0}}";
    public static LocString HEATGENERATED_AIRCONDITIONER = (LocString) (UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)");
    public static LocString HEATGENERATED_LIQUIDCONDITIONER = (LocString) (UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)");
    public static LocString FABRICATES = (LocString) "Fabricates";
    public static LocString FABRICATEDITEM = (LocString) "{1}";
    public static LocString PROCESSES = (LocString) "Refines:";
    public static LocString PROCESSEDITEM = (LocString) "{1} {0}";
    public static LocString PLANTERBOX_PENTALTY = (LocString) "Planter box penalty";
    public static LocString DECORPROVIDED = (LocString) (UI.FormatAsLink("Decor", "DECOR") + ": {1} (Radius: {2} tiles)");
    public static LocString OVERHEAT_TEMP = (LocString) $"Overheat {UI.FormatAsLink("Temperature", "HEAT")}: {{0}}";
    public static LocString MINIMUM_TEMP = (LocString) $"Freeze {UI.FormatAsLink("Temperature", "HEAT")}: {{0}}";
    public static LocString OVER_PRESSURE_MASS = (LocString) "Overpressure: {0}";
    public static LocString REFILLOXYGENTANK = (LocString) ("Refills Exosuit " + (string) EQUIPMENT.PREFABS.OXYGEN_TANK.NAME);
    public static LocString DUPLICANTMOVEMENTBOOST = (LocString) "Runspeed: {0}";
    public static LocString ELECTROBANKS = (LocString) (UI.FormatAsLink("Charge", "POWER") + ": {0}");
    public static LocString STRESSREDUCEDPERMINUTE = (LocString) (UI.FormatAsLink("Stress", "STRESS") + ": {0} per minute");
    public static LocString REMOVESEFFECTSUBTITLE = (LocString) "Cures";
    public static LocString REMOVEDEFFECT = (LocString) "{0}";
    public static LocString ADDED_EFFECT = (LocString) "Added Effect: {0}";
    public static LocString GASCOOLING = (LocString) (UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}");
    public static LocString LIQUIDCOOLING = (LocString) (UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}");
    public static LocString MAX_WATTAGE = (LocString) $"Max {UI.FormatAsLink("Power", "POWER")}: {{0}}";
    public static LocString MAX_BITS = (LocString) (UI.FormatAsLink("Bit", "LOGIC") + " Depth: {0}");
    public static LocString RESEARCH_MATERIALS = (LocString) $"{{0}}: {{1}} per {UI.FormatAsLink("Research", "RESEARCH")} point";
    public static LocString PRODUCES_RESEARCH_POINTS = (LocString) "{0}";
    public static LocString HIT_POINTS_PER_CYCLE = (LocString) (UI.FormatAsLink("Health", "Health") + " per cycle: {0}");
    public static LocString KCAL_PER_CYCLE = (LocString) (UI.FormatAsLink("KCal", "FOOD") + " per cycle: {0}");
    public static LocString PER_STARMAP_HEX = (LocString) "{0} per Starmap hex";
    public static LocString REMOVES_DISEASE = (LocString) "Kills germs";
    public static LocString DOCTORING = (LocString) "Doctoring";
    public static LocString RECREATION = (LocString) "Recreation";
    public static LocString COOLANT = (LocString) "Coolant: {1} {0}";
    public static LocString REFINEMENT_ENERGY = (LocString) "Heat: {0}";
    public static LocString IMPROVED_BUILDINGS = (LocString) "Improved Buildings";
    public static LocString IMPROVED_PLANTS = (LocString) "Improved Plants";
    public static LocString IMPROVED_BUILDINGS_ITEM = (LocString) "{0}";
    public static LocString IMPROVED_PLANTS_ITEM = (LocString) "{0}";
    public static LocString GEYSER_PRODUCTION = (LocString) "{0}: {1} at {2}";
    public static LocString GEYSER_DISEASE = (LocString) "Germs: {0}";
    public static LocString GEYSER_PERIOD = (LocString) "Eruption Period: {0} every {1}";
    public static LocString GEYSER_YEAR_UNSTUDIED = (LocString) "Active Period: (Requires Analysis)";
    public static LocString GEYSER_YEAR_PERIOD = (LocString) "Active Period: {0} every {1}";
    public static LocString GEYSER_YEAR_NEXT_ACTIVE = (LocString) "Next Activity: {0}";
    public static LocString GEYSER_YEAR_NEXT_DORMANT = (LocString) "Next Dormancy: {0}";
    public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = (LocString) "Average Output: (Requires Analysis)";
    public static LocString GEYSER_YEAR_AVR_OUTPUT = (LocString) "Average Output: {0}";
    public static LocString CAPTURE_METHOD_WRANGLE = (LocString) "Capture Method: Wrangling";
    public static LocString CAPTURE_METHOD_FLYING_TRAP = (LocString) "Capture Method: Airborne Critter Trap";
    public static LocString CAPTURE_METHOD_LAND_TRAP = (LocString) "Capture Method: Critter Trap";
    public static LocString CAPTURE_METHOD_FISH_TRAP = (LocString) "Capture Method: Fish Trap";
    public static LocString DIET_HEADER = (LocString) "Digestion:";
    public static LocString DIET_CONSUMED = (LocString) "    • Diet: {Foodlist}";
    public static LocString DIET_STORED = (LocString) "    • Stores: {Foodlist}";
    public static LocString DIET_CONSUMED_ITEM = (LocString) "{Food}: {Amount}";
    public static LocString DIET_PRODUCED = (LocString) "    • Excretion: {Items}";
    public static LocString DIET_PRODUCED_ITEM = (LocString) "{Item}: {Percent} of consumed mass";
    public static LocString DIET_PRODUCED_ITEM_FROM_PLANT = (LocString) "{Item}: {Amount} when properly fed";
    public static LocString DIET_ADDITIONAL_PRODUCED = (LocString) "Secondary Excretion: {Items}";
    public static LocString SCALE_GROWTH = (LocString) "Shearable {Item}: {Amount} per {Time}";
    public static LocString SCALE_GROWTH_ATMO = (LocString) "Shearable {Item}: {Amount} per {Time} ({Atmosphere})";
    public static LocString SCALE_GROWTH_TEMP = (LocString) "Shearable {Item}: {Amount} per {Time} ({TempMin} - {TempMax})";
    public static LocString SCALE_GROWTH_FERTILITY = (LocString) $"Milkable {{Item}}: {{Amount}} when {UI.PRE_KEYWORD}Reproduction{UI.PST_KEYWORD} reaches {{Percent}}";
    public static LocString MUCUS_SECRETION = (LocString) "Secretes {Item}: {Rate}";
    public static LocString MOLT_DROP = (LocString) "Drops {Item}: {Rate} when happy";
    public static LocString ACCESS_CONTROL = (LocString) "Duplicant Access Permissions";
    public static LocString MOISTURE = (LocString) "Moisture Loss: {Percent}";
    public static LocString ROCKETRESTRICTION_HEADER = (LocString) "Restriction Control:";
    public static LocString ROCKETRESTRICTION_BUILDINGS = (LocString) "    • Buildings: {buildinglist}";
    public static LocString UNSTABLEENTOMBDEFENSEREADY = (LocString) "Entomb Defense: Ready";
    public static LocString UNSTABLEENTOMBDEFENSETHREATENED = (LocString) "Entomb Defense: Threatened";
    public static LocString UNSTABLEENTOMBDEFENSEREACTING = (LocString) "Entomb Defense: Reacting";
    public static LocString UNSTABLEENTOMBDEFENSEOFF = (LocString) "Entomb Defense: Off";
    public static LocString ITEM_TEMPERATURE_ADJUST = (LocString) $"Stored {UI.FormatAsLink("Temperature", "HEAT")}: {{0}}";
    public static LocString NOISE_CREATED = (LocString) (UI.FormatAsLink("Noise", "SOUND") + ": {0} dB (Radius: {1} tiles)");
    public static LocString MESS_TABLE_SALT = (LocString) "Table Salt: +{0}";
    public static LocString MESS_CAVIAR = (LocString) (UI.FormatAsLink("Caviar", "CAVIAR") + ": +{0} Morale, -{1}% Stress");
    public static LocString COMMUNAL_DINING = (LocString) "Communal Dining: +{0}";
    public static LocString ACTIVE_PARTICLE_CONSUMPTION = (LocString) "Radbolts: {Rate}";
    public static LocString UNDERWATER_DRILL_DIAMOND_CONSUMPTION = (LocString) (UI.FormatAsLink("Diamond", "DIAMOND") + ": {Rate}");
    public static LocString PARTICLE_PORT_INPUT = (LocString) "Radbolt Input Port";
    public static LocString PARTICLE_PORT_OUTPUT = (LocString) "Radbolt Output Port";
    public static LocString IN_ORBIT_REQUIRED = (LocString) "Active In Space";
    public static LocString KETTLE_MELT_RATE = (LocString) "Melting Rate: {0}";
    public static LocString UNDERWATERVENT_SHEARING = (LocString) (UI.FormatAsLink("Sulfur", "SULFUR") + " Build-up: {0}");
    public static LocString FOOD_DEHYDRATOR_WATER_OUTPUT = (LocString) "Wet Floor";
    public static LocString PRODUCESDATABANKS = (LocString) (UI.FormatAsLink(DatabankHelper.NAME_PLURAL, "Databank") + ": {0}");
    public static LocString PRODUCESMEDICINE = (LocString) "Medicine";

    public class TOOLTIPS
    {
      public static LocString OPERATIONREQUIREMENTS = (LocString) "All requirements must be met in order for this building to operate";
      public static LocString REQUIRESPOWER = (LocString) $"Must be connected to a power grid with at least {UI.FormatAsNegativeRate("{0}")} of available {UI.PRE_KEYWORD}Power{UI.PST_KEYWORD}";
      public static LocString REQUIRESELEMENT = (LocString) $"Must receive deliveries of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} to function";
      public static LocString REQUIRESLIQUIDINPUT = (LocString) $"Must receive {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD} from a {(string) BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME} system";
      public static LocString REQUIRESLIQUIDOUTPUT = (LocString) $"Must expel {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD} through a {(string) BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME} system";
      public static LocString REQUIRESLIQUIDOUTPUTS = (LocString) $"Must expel {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD} through a {(string) BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME} system";
      public static LocString REQUIRESGASINPUT = (LocString) $"Must receive {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD} from a {(string) BUILDINGS.PREFABS.GASCONDUIT.NAME} system";
      public static LocString REQUIRESGASOUTPUT = (LocString) $"Must expel {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD} through a {(string) BUILDINGS.PREFABS.GASCONDUIT.NAME} system";
      public static LocString REQUIRESGASOUTPUTS = (LocString) $"Must expel {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD} through a {(string) BUILDINGS.PREFABS.GASCONDUIT.NAME} system";
      public static LocString REQUIRESMANUALOPERATION = (LocString) "A Duplicant must be present to run this building";
      public static LocString REQUIRESSKILLEDOPERATION = (LocString) "Only a Duplicant with the {Skill} skill can use this building";
      public static LocString REQUIRESSKILLEDOPERATION_DLC3 = (LocString) "Only a Duplicant with the {Skill} skill or {Booster} can use this building";
      public static LocString REQUIRESCREATIVITY = (LocString) $"An expressive Duplicant must work on this object to create {UI.PRE_KEYWORD}Art{UI.PST_KEYWORD}";
      public static LocString REQUIRESPOWERGENERATOR = (LocString) $"Must be connected to a {UI.PRE_KEYWORD}Power{UI.PST_KEYWORD} producing generator to function";
      public static LocString REQUIRESSEED = (LocString) $"Must receive a plant {UI.PRE_KEYWORD}Seed{UI.PST_KEYWORD}";
      public static LocString PREFERS_ROOM = (LocString) $"This building gains additional effects or functionality when built inside a {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD}";
      public static LocString REQUIRESROOM = (LocString) $"Must be built within a dedicated {UI.PRE_KEYWORD}Room{UI.PST_KEYWORD}\n\n{UI.PRE_KEYWORD}Room{UI.PST_KEYWORD} will become a {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} after construction";
      public static LocString ALLOWS_FERTILIZER = (LocString) $"Allows {UI.PRE_KEYWORD}Fertilizer{UI.PST_KEYWORD} to be delivered to plants";
      public static LocString ALLOWS_IRRIGATION = (LocString) $"Allows {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD} to be delivered to plants";
      public static LocString ALLOWS_IRRIGATION_PIPE = (LocString) $"Allows irrigation {UI.PRE_KEYWORD}Pipe{UI.PST_KEYWORD} connection";
      public static LocString ASSIGNEDDUPLICANT = (LocString) "This amenity may only be used by the Duplicant it is assigned to";
      public static LocString BUILDINGROOMREQUIREMENTCLASS = (LocString) $"This category of building may be required or prohibited in certain {UI.PRE_KEYWORD}Rooms{UI.PST_KEYWORD}";
      public static LocString OPERATIONEFFECTS = (LocString) "The building will produce these effects when its requirements are met";
      public static LocString BATTERYCAPACITY = (LocString) $"Can hold <b>{{0}}</b> of {UI.PRE_KEYWORD}Power{UI.PST_KEYWORD} when connected to a {UI.PRE_KEYWORD}Generator{UI.PST_KEYWORD}";
      public static LocString BATTERYLEAK = (LocString) $"{UI.FormatAsNegativeRate("{0}")} of this battery's charge will be lost as {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD}";
      public static LocString STORAGECAPACITY = (LocString) "Holds up to <b>{0}</b> of material";
      public static LocString ELEMENTEMITTED_INPUTTEMP = (LocString) $"Produces {UI.FormatAsPositiveRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} when in use\n\nIt will be the combined {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of the input materials.";
      public static LocString ELEMENTEMITTED_ENTITYTEMP = (LocString) $"Produces {UI.FormatAsPositiveRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} when in use\n\nIt will be the {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of the building at the time of production";
      public static LocString ELEMENTEMITTED_MINORENTITYTEMP = (LocString) $"Produces {UI.FormatAsPositiveRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} when in use\n\nIt will be at least <b>{{2}}</b>, or hotter if the building is hotter.";
      public static LocString ELEMENTEMITTED_MINTEMP = (LocString) $"Produces {UI.FormatAsPositiveRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} when in use\n\nIt will be at least <b>{{2}}</b>, or hotter if the input materials are hotter.";
      public static LocString ELEMENTEMITTED_FIXEDTEMP = (LocString) $"Produces {UI.FormatAsPositiveRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} when in use\n\nIt will be produced at <b>{{2}}</b>.";
      public static LocString ELEMENTCONSUMED = (LocString) $"Consumes {UI.FormatAsNegativeRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} when in use";
      public static LocString ELEMENTEMITTED_TOILET = (LocString) $"Produces {UI.FormatAsPositiveRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} per use\n\nDuplicant waste is emitted at <b>{{2}}</b>.";
      public static LocString ELEMENTEMITTEDPERUSE = (LocString) $"Produces {UI.FormatAsPositiveRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} per use\n\nIt will be the {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of the input materials.";
      public static LocString DISEASEEMITTEDPERUSE = (LocString) $"Produces {UI.FormatAsPositiveRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} per use";
      public static LocString DISEASECONSUMEDPERUSE = (LocString) $"Removes {UI.FormatAsNegativeRate("{0}")} per use";
      public static LocString ELEMENTCONSUMEDPERUSE = (LocString) $"Consumes {UI.FormatAsNegativeRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} per use";
      public static LocString ENERGYCONSUMED = (LocString) $"Draws {UI.FormatAsNegativeRate("{0}")} from the {UI.PRE_KEYWORD}Power Grid{UI.PST_KEYWORD} it's connected to";
      public static LocString ENERGYGENERATED = (LocString) $"Produces {UI.FormatAsPositiveRate("{0}")} for the {UI.PRE_KEYWORD}Power Grid{UI.PST_KEYWORD} it's connected to";
      public static LocString ENABLESDOMESTICGROWTH = (LocString) $"Accelerates {UI.PRE_KEYWORD}Plant{UI.PST_KEYWORD} growth and maturation";
      public static LocString HEATGENERATED = (LocString) $"Generates {UI.FormatAsPositiveRate("{0}")} per second\n\nSum {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} change is affected by the material attributes of the heated substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity";
      public static LocString HEATCONSUMED = (LocString) $"Dissipates {UI.FormatAsNegativeRate("{0}")} per second\n\nSum {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} change can be affected by the material attributes of the cooled substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity";
      public static LocString HEATER_TARGETTEMPERATURE = (LocString) $"Stops heating when the surrounding average {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} is above <b>{{0}}</b>";
      public static LocString FABRICATES = (LocString) "Fabrication is the production of items and equipment";
      public static LocString PROCESSES = (LocString) "Processes raw materials into refined materials";
      public static LocString PROCESSEDITEM = (LocString) $"Refining this material produces {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD}";
      public static LocString PLANTERBOX_PENTALTY = (LocString) "Plants grow more slowly when contained within boxes";
      public static LocString DECORPROVIDED = (LocString) $"Improves {UI.PRE_KEYWORD}Decor{UI.PST_KEYWORD} values by {UI.FormatAsPositiveModifier("<b>{0}</b>")} in a <b>{{1}}</b> tile radius";
      public static LocString DECORDECREASED = (LocString) $"Decreases {UI.PRE_KEYWORD}Decor{UI.PST_KEYWORD} values by {UI.FormatAsNegativeModifier("<b>{0}</b>")} in a <b>{{1}}</b> tile radius";
      public static LocString OVERHEAT_TEMP = (LocString) "Begins overheating at <b>{0}</b>";
      public static LocString MINIMUM_TEMP = (LocString) "Ceases to function when temperatures fall below <b>{0}</b>";
      public static LocString OVER_PRESSURE_MASS = (LocString) "Ceases to function when the surrounding mass is above <b>{0}</b>";
      public static LocString REFILLOXYGENTANK = (LocString) $"Refills {UI.PRE_KEYWORD}Exosuit{UI.PST_KEYWORD} Oxygen tanks with {UI.PRE_KEYWORD}Oxygen{UI.PST_KEYWORD} for reuse";
      public static LocString DUPLICANTMOVEMENTBOOST = (LocString) "Duplicants walk <b>{0}</b> faster on this tile";
      public static LocString ELECTROBANKS = (LocString) $"Power Banks store {{0}} of {UI.PRE_KEYWORD}Power{UI.PST_KEYWORD}\n\nThey can be discharged by circuits, buildings and Bionic Duplicants";
      public static LocString STRESSREDUCEDPERMINUTE = (LocString) $"Removes <b>{{0}}</b> of Duplicants' {UI.PRE_KEYWORD}Stress{UI.PST_KEYWORD} for every uninterrupted minute of use";
      public static LocString REMOVESEFFECTSUBTITLE = (LocString) "Use of this building will remove the listed effects";
      public static LocString REMOVEDEFFECT = (LocString) "{0}";
      public static LocString ADDED_EFFECT = (LocString) "Effect being applied:\n\n{0}\n{1}";
      public static LocString GASCOOLING = (LocString) $"Reduces the {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of piped {UI.PRE_KEYWORD}Gases{UI.PST_KEYWORD} by <b>{{0}}</b>";
      public static LocString LIQUIDCOOLING = (LocString) $"Reduces the {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of piped {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD} by <b>{{0}}</b>";
      public static LocString MAX_WATTAGE = (LocString) $"Drawing more than the maximum allowed {UI.PRE_KEYWORD}Watts{UI.PST_KEYWORD} can result in damage to the circuit";
      public static LocString MAX_BITS = (LocString) $"Sending an {UI.PRE_KEYWORD}Automation Signal{UI.PST_KEYWORD} with a higher {UI.PRE_KEYWORD}Bit Depth{UI.PST_KEYWORD} than the connected {UI.PRE_KEYWORD}Logic Wire{UI.PST_KEYWORD} can result in damage to the circuit";
      public static LocString RESEARCH_MATERIALS = (LocString) $"This research station consumes {UI.FormatAsNegativeRate("{1}")} of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} for each {UI.PRE_KEYWORD}Research Point{UI.PST_KEYWORD} produced";
      public static LocString PRODUCES_RESEARCH_POINTS = (LocString) $"Produces {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} research";
      public static LocString REMOVES_DISEASE = (LocString) $"The cooking process kills all {UI.PRE_KEYWORD}Germs{UI.PST_KEYWORD} present in the ingredients, removing the {UI.PRE_KEYWORD}Disease{UI.PST_KEYWORD} risk when eating the product";
      public static LocString DOCTORING = (LocString) $"Doctoring increases existing health benefits and can allow the treatment of otherwise stubborn {UI.PRE_KEYWORD}Diseases{UI.PST_KEYWORD}";
      public static LocString RECREATION = (LocString) $"Improves Duplicant {UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD} during scheduled {UI.PRE_KEYWORD}Downtime{UI.PST_KEYWORD}";
      public static LocString HEATGENERATED_AIRCONDITIONER = (LocString) $"Generates {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD} based on the {UI.PRE_KEYWORD}Volume{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Specific Heat Capacity{UI.PST_KEYWORD} of the pumped {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD}\n\nCooling 1{(string) UI.UNITSUFFIXES.MASS.KILOGRAM} of {(string) ELEMENTS.OXYGEN.NAME} the entire <b>{{1}}</b> will output <b>{{0}}</b>";
      public static LocString HEATGENERATED_LIQUIDCONDITIONER = (LocString) $"Generates {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD} based on the {UI.PRE_KEYWORD}Volume{UI.PST_KEYWORD} and {UI.PRE_KEYWORD}Specific Heat Capacity{UI.PST_KEYWORD} of the pumped {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD}\n\nCooling 10{(string) UI.UNITSUFFIXES.MASS.KILOGRAM} of {(string) ELEMENTS.WATER.NAME} the entire <b>{{1}}</b> will output <b>{{0}}</b>";
      public static LocString MOVEMENT_BONUS = (LocString) "Increases the Runspeed of Duplicants";
      public static LocString COOLANT = (LocString) $"<b>{{1}}</b> of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} coolant is required to cool off an item produced by this building\n\nCoolant {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} increase is variable and dictated by the amount of energy needed to cool the produced item";
      public static LocString REFINEMENT_ENERGY_HAS_COOLANT = (LocString) $"{UI.FormatAsPositiveRate("{0}")} of {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD} will be produced to cool off the fabricated item\n\nThis will raise the {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of the contained {UI.PRE_KEYWORD}{{1}}{UI.PST_KEYWORD} by {UI.FormatAsPositiveModifier("{2}")}, and heat the containing building";
      public static LocString REFINEMENT_ENERGY_NO_COOLANT = (LocString) $"{UI.FormatAsPositiveRate("{0}")} of {UI.PRE_KEYWORD}Heat{UI.PST_KEYWORD} will be produced to cool off the fabricated item\n\nIf {UI.PRE_KEYWORD}{{1}}{UI.PST_KEYWORD} is used for coolant, its {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} will be raised by {UI.FormatAsPositiveModifier("{2}")}, and will heat the containing building";
      public static LocString IMPROVED_BUILDINGS = (LocString) $"{UI.PRE_KEYWORD}Tune Ups{UI.PST_KEYWORD} will improve these buildings:";
      public static LocString IMPROVED_BUILDINGS_ITEM = (LocString) "{0}";
      public static LocString IMPROVED_PLANTS = (LocString) $"{UI.PRE_KEYWORD}Crop Tending{UI.PST_KEYWORD} will improve growth times for these plants:";
      public static LocString IMPROVED_PLANTS_ITEM = (LocString) "{0}";
      public static LocString GEYSER_PRODUCTION = (LocString) $"While erupting, this geyser will produce {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} at a rate of {UI.FormatAsPositiveRate("{1}")}, and at a {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of <b>{{2}}</b>";
      public static LocString GEYSER_PRODUCTION_GEOTUNED = (LocString) $"While erupting, this geyser will produce {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} at a rate of {UI.FormatAsPositiveRate("{1}")}, and at a {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of <b>{{2}}</b>";
      public static LocString GEYSER_PRODUCTION_GEOTUNED_COUNT = (LocString) "<b>{0}</b> of <b>{1}</b> Geotuners targeting this geyser are amplifying it";
      public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL = (LocString) "Total geotuning: {0} {1}";
      public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL_ROW_TITLE = (LocString) "Geotuned ";
      public static LocString GEYSER_DISEASE = (LocString) $"{UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} germs are present in the output of this geyser";
      public static LocString GEYSER_PERIOD = (LocString) "This geyser will produce for <b>{0}</b> of every <b>{1}</b>";
      public static LocString GEYSER_YEAR_UNSTUDIED = (LocString) "A researcher must analyze this geyser to determine its geoactive period";
      public static LocString GEYSER_YEAR_PERIOD = (LocString) "This geyser will be active for <b>{0}</b> out of every <b>{1}</b>\n\nIt will be dormant the rest of the time";
      public static LocString GEYSER_YEAR_NEXT_ACTIVE = (LocString) "This geyser will become active in <b>{0}</b>";
      public static LocString GEYSER_YEAR_NEXT_DORMANT = (LocString) "This geyser will become dormant in <b>{0}</b>";
      public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = (LocString) "A researcher must analyze this geyser to determine its average output rate";
      public static LocString GEYSER_YEAR_AVR_OUTPUT = (LocString) "This geyser emits an average of {average} of {element} during its lifetime\n\nThis includes its dormant period";
      public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE = (LocString) "Total Geotuning ";
      public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW = (LocString) "Geotuned ";
      public static LocString CAPTURE_METHOD_WRANGLE = (LocString) $"This critter can be captured\n\nMark critters for capture using the {UI.FormatAsTool("Wrangle Tool", Action.Capture)}\n\nDuplicants must possess the {UI.PRE_KEYWORD}Critter Ranching{UI.PST_KEYWORD} skill in order to wrangle critters";
      public static LocString CAPTURE_METHOD_FLYING_TRAP = (LocString) ("This critter can be captured and moved using an " + (string) BUILDINGS.PREFABS.CREATUREAIRTRAP.NAME);
      public static LocString CAPTURE_METHOD_TRAP = (LocString) ("This critter can be captured and moved using a " + (string) BUILDINGS.PREFABS.CREATURETRAP.NAME);
      public static LocString CAPTURE_METHOD_FISH_TRAP = (LocString) ("This critter can be captured and moved using a " + (string) BUILDINGS.PREFABS.FISHTRAP.NAME);
      public static LocString NOISE_POLLUTION_INCREASE = (LocString) "Produces noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";
      public static LocString NOISE_POLLUTION_DECREASE = (LocString) "Dampens noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";
      public static LocString ITEM_TEMPERATURE_ADJUST = (LocString) $"Stored items will reach a {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} of <b>{{0}}</b> over time";
      public static LocString DIET_HEADER = (LocString) "Creatures will eat and digest only specific materials";
      public static LocString DIET_CONSUMED = (LocString) "This critter can typically consume these materials at the following rates:\n\n{Foodlist}";
      public static LocString DIET_PRODUCED = (LocString) "This critter will \"produce\" the following materials:\n\n{Items}";
      public static LocString DIET_ADDITIONAL_PRODUCED = (LocString) "This critter gets bloated after eating and will produce {Items}";
      public static LocString ROCKETRESTRICTION_HEADER = (LocString) "Controls whether a building is operational within a rocket interior";
      public static LocString ROCKETRESTRICTION_BUILDINGS = (LocString) "This station controls the operational status of the following buildings:\n\n{buildinglist}";
      public static LocString UNSTABLEENTOMBDEFENSEREADY = (LocString) $"This plant is ready to shake off {UI.PRE_KEYWORD}Unstable{UI.PST_KEYWORD} elements that threaten to entomb it";
      public static LocString UNSTABLEENTOMBDEFENSETHREATENED = (LocString) $"This plant is preparing to shake off {UI.PRE_KEYWORD}Unstable{UI.PST_KEYWORD} elements that are entombing it";
      public static LocString UNSTABLEENTOMBDEFENSEREACTING = (LocString) $"This plant is currently unentombing itself from {UI.PRE_KEYWORD}Unstable{UI.PST_KEYWORD} elements";
      public static LocString UNSTABLEENTOMBDEFENSEOFF = (LocString) $"This plant's ability to unentomb itself from {UI.PRE_KEYWORD}Unstable{UI.PST_KEYWORD} elements is currently disabled";
      public static LocString BRANCH_GROWER_PLANT_POTENTIAL_OUTPUT = (LocString) "{0} to {1}";
      public static LocString EDIBLE_PLANT_INTERNAL_STORAGE = (LocString) "{0} of stored {1}";
      public static LocString SCALE_GROWTH = (LocString) $"This critter can be sheared every <b>{{Time}}</b> to produce {UI.FormatAsPositiveModifier("{Amount}")} of {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD}";
      public static LocString SCALE_GROWTH_ATMO = (LocString) $"This critter can be sheared every <b>{{Time}}</b> to produce {UI.FormatAsPositiveRate("{Amount}")} of {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD}\n\nIt must be kept in {UI.PRE_KEYWORD}{{Atmosphere}}{UI.PST_KEYWORD}-rich environments to regrow sheared {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD}";
      public static LocString SCALE_GROWTH_TEMP = (LocString) $"This critter can be sheared every <b>{{Time}}</b> to produce {UI.FormatAsPositiveRate("{Amount}")} of {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD}\n\nIt must eat food between {{TempMin}} - {{TempMax}} to regrow sheared {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD}";
      public static LocString SCALE_GROWTH_FED = (LocString) $"This critter can be sheared every <b>{{Time}}</b> to produce {UI.FormatAsPositiveModifier("{Amount}")} of {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD}\n\nIt must be well fed to grow shearable {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD}";
      public static LocString SCALE_GROWTH_FERTILE = (LocString) $"This critter can be milked to produce {UI.FormatAsPositiveModifier("{Amount}")} of {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD} once tamed\n\n{UI.PRE_KEYWORD}Reproduction{UI.PST_KEYWORD} must be 75% or higher before milking\n\nElderly critters cannot be milked";
      public static LocString MUCUS_SECRETION = (LocString) $"This critter secretes {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD} at a rate of {UI.FormatAsPositiveRate("{Rate}")}";
      public static LocString MOLT_DROP = (LocString) $"This critter drops {UI.PRE_KEYWORD}{{Item}}{UI.PST_KEYWORD} at a rate of {UI.FormatAsPositiveRate("{Rate}")} when it is happy\n\nIt must be in a good {UI.PRE_KEYWORD}Mood{UI.PST_KEYWORD} in order to molt";
      public static LocString MESS_TABLE_SALT = (LocString) $"Duplicants gain {UI.FormatAsPositiveModifier("+{0}")} {UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD} when using {UI.PRE_KEYWORD}Table Salt{UI.PST_KEYWORD} with their food at a {(string) ROOMS.CRITERIA.DININGTABLETYPE.NAME}";
      public static LocString MESS_CAVIAR = (LocString) $"Duplicants gain {UI.FormatAsPositiveModifier("+{0}")} {UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD} and {UI.FormatAsNegativeModifier("-{1}%")} {UI.PRE_KEYWORD}Stress{UI.PST_KEYWORD} when topping their food with {UI.PRE_KEYWORD}Caviar{UI.PST_KEYWORD} at a {(string) ROOMS.CRITERIA.DININGTABLETYPE.NAME}";
      public static LocString COMMUNAL_DINING = (LocString) $"Duplicants gain {UI.FormatAsPositiveModifier("+{0}")} {UI.PRE_KEYWORD}Morale{UI.PST_KEYWORD} when dining with friends at a {(string) BUILDINGS.PREFABS.MULTIMINIONDININGTABLE.NAME}";
      public static LocString ACCESS_CONTROL = (LocString) "Settings to allow or restrict Duplicants from passing through the door.";
      public static LocString TRANSFORMER_INPUT_WIRE = (LocString) $"Connect a {UI.PRE_KEYWORD}Wire{UI.PST_KEYWORD} to the large {UI.PRE_KEYWORD}Input{UI.PST_KEYWORD} with any amount of {UI.PRE_KEYWORD}Watts{UI.PST_KEYWORD}.";
      public static LocString TRANSFORMER_OUTPUT_WIRE = (LocString) $"The {UI.PRE_KEYWORD}Power{UI.PST_KEYWORD} provided by the the small {UI.PRE_KEYWORD}Output{UI.PST_KEYWORD} will be limited to {{0}}.";
      public static LocString FABRICATOR_INGREDIENTS = (LocString) "Ingredients:\n{0}";
      public static LocString ACTIVE_PARTICLE_CONSUMPTION = (LocString) $"This building requires {UI.PRE_KEYWORD}Radbolts{UI.PST_KEYWORD} to function, consuming them at a rate of {{Rate}} while in use";
      public static LocString UNDERWATER_DRILL_DIAMOND_CONSUMPTION = (LocString) $"This building requires {UI.PRE_KEYWORD}Diamond{UI.PST_KEYWORD} to operate, consuming it at a rate of {{Rate}} while in use";
      public static LocString PARTICLE_PORT_INPUT = (LocString) $"A Radbolt Port on this building allows it to receive {UI.PRE_KEYWORD}Radbolts{UI.PST_KEYWORD}";
      public static LocString PARTICLE_PORT_OUTPUT = (LocString) $"This building has a configurable Radbolt Port for {UI.PRE_KEYWORD}Radbolt{UI.PST_KEYWORD} emission";
      public static LocString IN_ORBIT_REQUIRED = (LocString) "This building is only operational while its parent rocket is in flight";
      public static LocString FOOD_DEHYDRATOR_WATER_OUTPUT = (LocString) $"This building dumps {UI.PRE_KEYWORD}Water{UI.PST_KEYWORD} on the floor while in use";
      public static LocString KETTLE_MELT_RATE = (LocString) $"This building melts {{0}} of {UI.PRE_KEYWORD}Ice{UI.PST_KEYWORD} into {{0}} of cold ({{1}}) {UI.PRE_KEYWORD}Water{UI.PST_KEYWORD}\n\n{UI.PRE_KEYWORD}Wood{UI.PST_KEYWORD} consumption varies depending on the initial temperature of the {UI.PRE_KEYWORD}Ice{UI.PST_KEYWORD}";
      public static LocString PRODUCESDATABANKS = (LocString) $"This building produces {{0}} {UI.FormatAsLink(DatabankHelper.NAME_PLURAL, "Databank")} each time a Starmap hex is revealed";
      public static LocString PRODUCESMEDICINE = (LocString) "This building produces medical supplies";
      public static LocString UNDERWATERVENT_SHEARING = (LocString) ("This vent produces {0} which can be harvested using a " + UI.FormatAsLink("Marine Drill", "UNDERWATERVENTDRILL"));
    }
  }

  public class LOGIC_PORTS
  {
    public static LocString INPUT_PORTS = (LocString) UI.FormatAsLink("Auto Inputs", "LOGIC");
    public static LocString INPUT_PORTS_TOOLTIP = (LocString) "Input ports change a state on this building when a signal is received";
    public static LocString OUTPUT_PORTS = (LocString) UI.FormatAsLink("Auto Outputs", "LOGIC");
    public static LocString OUTPUT_PORTS_TOOLTIP = (LocString) "Output ports send a signal when this building changes state";
    public static LocString INPUT_PORT_TOOLTIP = (LocString) "Input Behavior:\n• {0}\n• {1}";
    public static LocString OUTPUT_PORT_TOOLTIP = (LocString) "Output Behavior:\n• {0}\n• {1}";
    public static LocString CONTROL_OPERATIONAL = (LocString) "Enable/Disable";
    public static LocString CONTROL_OPERATIONAL_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable building");
    public static LocString CONTROL_OPERATIONAL_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable building");
    public static LocString PORT_INPUT_DEFAULT_NAME = (LocString) "INPUT";
    public static LocString PORT_OUTPUT_DEFAULT_NAME = (LocString) "OUTPUT";
    public static LocString GATE_MULTI_INPUT_ONE_NAME = (LocString) "INPUT A";
    public static LocString GATE_MULTI_INPUT_ONE_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_MULTI_INPUT_ONE_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_MULTI_INPUT_TWO_NAME = (LocString) "INPUT B";
    public static LocString GATE_MULTI_INPUT_TWO_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_MULTI_INPUT_TWO_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_MULTI_INPUT_THREE_NAME = (LocString) "INPUT C";
    public static LocString GATE_MULTI_INPUT_THREE_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_MULTI_INPUT_THREE_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_MULTI_INPUT_FOUR_NAME = (LocString) "INPUT D";
    public static LocString GATE_MULTI_INPUT_FOUR_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_MULTI_INPUT_FOUR_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_SINGLE_INPUT_ONE_NAME = (LocString) "INPUT";
    public static LocString GATE_SINGLE_INPUT_ONE_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_SINGLE_INPUT_ONE_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_MULTI_OUTPUT_ONE_NAME = (LocString) "OUTPUT A";
    public static LocString GATE_MULTI_OUTPUT_ONE_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_MULTI_OUTPUT_ONE_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_MULTI_OUTPUT_TWO_NAME = (LocString) "OUTPUT B";
    public static LocString GATE_MULTI_OUTPUT_TWO_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_MULTI_OUTPUT_TWO_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_MULTI_OUTPUT_THREE_NAME = (LocString) "OUTPUT C";
    public static LocString GATE_MULTI_OUTPUT_THREE_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_MULTI_OUTPUT_THREE_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_MULTI_OUTPUT_FOUR_NAME = (LocString) "OUTPUT D";
    public static LocString GATE_MULTI_OUTPUT_FOUR_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_MULTI_OUTPUT_FOUR_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_SINGLE_OUTPUT_ONE_NAME = (LocString) "OUTPUT";
    public static LocString GATE_SINGLE_OUTPUT_ONE_ACTIVE = (LocString) "Green Signal";
    public static LocString GATE_SINGLE_OUTPUT_ONE_INACTIVE = (LocString) "Red Signal";
    public static LocString GATE_MULTIPLEXER_CONTROL_ONE_NAME = (LocString) "CONTROL A";
    public static LocString GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position");
    public static LocString GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position");
    public static LocString GATE_MULTIPLEXER_CONTROL_TWO_NAME = (LocString) "CONTROL B";
    public static LocString GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE = (LocString) (UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position");
    public static LocString GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE = (LocString) (UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position");
  }

  public class GAMEOBJECTEFFECTS
  {
    public static LocString CALORIES = (LocString) "+{0}";
    public static LocString FOOD_QUALITY = (LocString) "Quality: {0}";
    public static LocString FOOD_MORALE = (LocString) "Morale: {0}";
    public static LocString FORGAVEATTACKER = (LocString) "Forgiveness";
    public static LocString COLDBREATHER = (LocString) UI.FormatAsLink("Cooling Effect", "HEAT");
    public static LocString LIFECYCLETITLE = (LocString) "Growth:";
    public static LocString GROWTHTIME_SIMPLE = (LocString) "Life Cycle: {0}";
    public static LocString GROWTHTIME_REGROWTH = (LocString) "Domestic growth: {0} / {1}";
    public static LocString GROWTHTIME = (LocString) "Growth: {0}";
    public static LocString INITIALGROWTHTIME = (LocString) "Initial Growth: {0}";
    public static LocString REGROWTHTIME = (LocString) "Regrowth: {0}";
    public static LocString REQUIRES_LIGHT = (LocString) (UI.FormatAsLink("Light", nameof (LIGHT)) + ": {Lux}");
    public static LocString REQUIRES_DARKNESS = (LocString) UI.FormatAsLink("Darkness", nameof (LIGHT));
    public static LocString REQUIRESFERTILIZER = (LocString) "{0}: {1}";
    public static LocString IDEAL_FERTILIZER = (LocString) "{0}: {1}";
    public static LocString EQUIPMENT_MODS = (LocString) "{Attribute} {Value}";
    public static LocString ROTTEN = (LocString) "Rotten";
    public static LocString REQUIRES_ATMOSPHERE = (LocString) "Atmosphere: {0}";
    public static LocString REQUIRES_PRESSURE = (LocString) "Air Pressure: {0} minimum";
    public static LocString IDEAL_PRESSURE = (LocString) "Air Pressure: {0}";
    public static LocString REQUIRES_TEMPERATURE = (LocString) (UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}");
    public static LocString IDEAL_TEMPERATURE = (LocString) (UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}");
    public static LocString REQUIRES_SUBMERSION = (LocString) (UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Submersion");
    public static LocString FOOD_EFFECTS = (LocString) "Effects:";
    public static LocString EMITS_LIGHT = (LocString) (UI.FormatAsLink("Light Range", nameof (LIGHT)) + ": {0} tiles");
    public static LocString EMITS_LIGHT_LUX = (LocString) (UI.FormatAsLink("Brightness", nameof (LIGHT)) + ": {0} Lux");
    public static LocString AMBIENT_RADIATION = (LocString) "Ambient Radiation";
    public static LocString AMBIENT_RADIATION_FMT = (LocString) "{minRads} - {maxRads}";
    public static LocString AMBIENT_NO_MIN_RADIATION_FMT = (LocString) "Less than {maxRads}";
    public static LocString REQUIRES_NO_MIN_RADIATION = (LocString) $"Maximum {UI.FormatAsLink("Radiation", "RADIATION")}: {{MaxRads}}";
    public static LocString REQUIRES_RADIATION = (LocString) (UI.FormatAsLink("Radiation", "RADIATION") + ": {MinRads} to {MaxRads}");
    public static LocString MUTANT_STERILE = (LocString) ("Doesn't Drop " + UI.FormatAsLink("Seeds", "PLANTS"));
    public static LocString REQUIREMETS_OR = (LocString) " or ";
    public static LocString DARKNESS = (LocString) "Darkness";
    public static LocString LIGHT = (LocString) "Light";
    public static LocString SEED_PRODUCTION_DIG_ONLY = (LocString) ("Consumes 1 " + UI.FormatAsLink("Seed", "PLANTS"));
    public static LocString SEED_PRODUCTION_HARVEST = (LocString) ("Harvest yields " + UI.FormatAsLink("Seeds", "PLANTS"));
    public static LocString SEED_PRODUCTION_FINAL_HARVEST = (LocString) ("Final harvest yields " + UI.FormatAsLink("Seeds", "PLANTS"));
    public static LocString SEED_PRODUCTION_FRUIT = (LocString) ("Fruit produces " + UI.FormatAsLink("Seeds", "PLANTS"));
    public static LocString SEED_REQUIREMENT_CEILING = (LocString) "Plot Orientation: Downward";
    public static LocString SEED_REQUIREMENT_WALL = (LocString) "Plot Orientation: Sideways";
    public static LocString REQUIRES_RECEPTACLE = (LocString) "Farm Plot";
    public static LocString PLANT_MARK_FOR_HARVEST = (LocString) "Autoharvest Enabled";
    public static LocString PLANT_DO_NOT_HARVEST = (LocString) "Autoharvest Disabled";
    public static LocString REQUIRES_POLLINATION = (LocString) "Pollination";

    public class INSULATED
    {
      public static LocString NAME = (LocString) "Insulated";
      public static LocString TOOLTIP = (LocString) "Proper insulation drastically reduces thermal conductivity";
    }

    public class TOOLTIPS
    {
      public static LocString CALORIES = (LocString) "+{0}";
      public static LocString FOOD_QUALITY = (LocString) "Quality: {0}";
      public static LocString FOOD_MORALE = (LocString) "Morale: {0}";
      public static LocString COLDBREATHER = (LocString) "Lowers ambient air temperature";
      public static LocString GROWTHTIME_SIMPLE = (LocString) "This plant takes <b>{0}</b> to grow";
      public static LocString GROWTHTIME_REGROWTH = (LocString) "This plant initially takes <b>{0}</b> to grow, but only <b>{1}</b> to mature after first harvest";
      public static LocString GROWTHTIME = (LocString) "This plant takes <b>{0}</b> to grow";
      public static LocString INITIALGROWTHTIME = (LocString) "This plant takes <b>{0}</b> to mature again once replanted";
      public static LocString REGROWTHTIME = (LocString) "This plant takes <b>{0}</b> to mature again once harvested";
      public static LocString EQUIPMENT_MODS = (LocString) "{Attribute} {Value}";
      public static LocString REQUIRESFERTILIZER = (LocString) $"This plant requires <b>{{1}}</b> {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} for basic growth";
      public static LocString IDEAL_FERTILIZER = (LocString) $"This plant requires <b>{{1}}</b> of {UI.PRE_KEYWORD}{{0}}{UI.PST_KEYWORD} for basic growth";
      public static LocString REQUIRES_LIGHT = (LocString) $"This plant requires a {UI.PRE_KEYWORD}Light{UI.PST_KEYWORD} source bathing it in at least {{Lux}}";
      public static LocString REQUIRES_DARKNESS = (LocString) "This plant requires complete darkness";
      public static LocString REQUIRES_ATMOSPHERE = (LocString) "This plant must be submerged in one of the following gases: {0}";
      public static LocString REQUIRES_ATMOSPHERE_LIQUID = (LocString) "This plant must be submerged in one of the following liquids: {0}";
      public static LocString REQUIRES_ATMOSPHERE_MIXED = (LocString) "This plant must be submerged in one of the following gases or liquids: {0}";
      public static LocString REQUIRES_PRESSURE = (LocString) $"Ambient {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD} pressure must be at least <b>{{0}}</b> for basic growth";
      public static LocString IDEAL_PRESSURE = (LocString) $"This plant requires {UI.PRE_KEYWORD}Gas{UI.PST_KEYWORD} pressures above <b>{{0}}</b> for basic growth";
      public static LocString REQUIRES_TEMPERATURE = (LocString) $"Internal {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} must be between <b>{{0}}</b> and <b>{{1}}</b> for basic growth";
      public static LocString IDEAL_TEMPERATURE = (LocString) $"This plant requires internal {UI.PRE_KEYWORD}Temperature{UI.PST_KEYWORD} between <b>{{0}}</b> and <b>{{1}}</b> for basic growth";
      public static LocString REQUIRES_SUBMERSION = (LocString) $"This plant must be fully submerged in {UI.PRE_KEYWORD}Liquid{UI.PST_KEYWORD} for basic growth";
      public static LocString FOOD_EFFECTS = (LocString) "Duplicants will gain the following effects from eating this food: {0}";
      public static LocString REQUIRES_RECEPTACLE = (LocString) $"This plant must be housed in a {UI.FormatAsLink("Planter Box", "PLANTERBOX")}, {UI.FormatAsLink("Farm Tile", "FARMTILE")}, or {UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM")} farm to grow domestically";
      public static LocString EMITS_LIGHT = (LocString) $"Emits {UI.PRE_KEYWORD}Light{UI.PST_KEYWORD}\n\nDuplicants can operate buildings more quickly when they're well lit";
      public static LocString EMITS_LIGHT_LUX = (LocString) $"Emits {UI.PRE_KEYWORD}Light{UI.PST_KEYWORD}\n\nDuplicants can operate buildings more quickly when they're well lit";
      public static LocString METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP = (LocString) "Distribution of meteor types in this shower";
      public static LocString SEED_PRODUCTION_DIG_ONLY = (LocString) $"May be replanted, but will produce no further {UI.PRE_KEYWORD}Seeds{UI.PST_KEYWORD}";
      public static LocString SEED_PRODUCTION_HARVEST = (LocString) $"Harvesting this plant will yield new {UI.PRE_KEYWORD}Seeds{UI.PST_KEYWORD}";
      public static LocString SEED_PRODUCTION_FINAL_HARVEST = (LocString) $"Yields new {UI.PRE_KEYWORD}Seeds{UI.PST_KEYWORD} on the final harvest of its life cycle";
      public static LocString SEED_PRODUCTION_FRUIT = (LocString) $"Consuming this plant's fruit will yield new {UI.PRE_KEYWORD}Seeds{UI.PST_KEYWORD}";
      public static LocString SEED_REQUIREMENT_CEILING = (LocString) $"This seed must be planted in a downward facing plot\n\nPress {UI.FormatAsKeyWord("[O]")} while building farm plots to rotate them";
      public static LocString SEED_REQUIREMENT_WALL = (LocString) $"This seed must be planted in a side facing plot\n\nPress {UI.FormatAsKeyWord("[O]")} while building farm plots to rotate them";
      public static LocString REQUIRES_NO_MIN_RADIATION = (LocString) ("This plant will stop growing if exposed to more than {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION"));
      public static LocString REQUIRES_RADIATION = (LocString) ("This plant will only grow if it has between {MinRads} and {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION"));
      public static LocString MUTANT_SEED_TOOLTIP = (LocString) "\n\nGrowing near its maximum radiation increases the chance of mutant seeds being produced";
      public static LocString MUTANT_STERILE = (LocString) "This plant will not produce seeds of its own due to changes to its DNA";
      public static LocString REQUIRES_POLLINATION = (LocString) $"This plant must be tended by a {UI.PRE_KEYWORD}Critter{UI.PST_KEYWORD} to grow";
    }

    public class DAMAGE_POPS
    {
      public static LocString OVERHEAT = (LocString) "Overheat Damage";
      public static LocString CORROSIVE_ELEMENT = (LocString) "Corrosive Element Damage";
      public static LocString WRONG_ELEMENT = (LocString) "Wrong Element Damage";
      public static LocString CIRCUIT_OVERLOADED = (LocString) "Overload Damage";
      public static LocString LOGIC_CIRCUIT_OVERLOADED = (LocString) "Signal Overload Damage";
      public static LocString LIQUID_PRESSURE = (LocString) "Pressure Damage";
      public static LocString MINION_DESTRUCTION = (LocString) "Tantrum Damage";
      public static LocString CONDUIT_CONTENTS_FROZE = (LocString) "Cold Damage";
      public static LocString CONDUIT_CONTENTS_BOILED = (LocString) "Heat Damage";
      public static LocString MICROMETEORITE = (LocString) "Micrometeorite Damage";
      public static LocString COMET = (LocString) "Meteor Damage";
      public static LocString ROCKET = (LocString) "Rocket Thruster Damage";
      public static LocString POWER_BANK_WATER_DAMAGE = (LocString) "Water Damage";
    }
  }

  public class ASTEROIDCLOCK
  {
    public static LocString CYCLE = (LocString) "Cycle";
    public static LocString CYCLES_OLD = (LocString) "This Colony is {0} Cycle(s) Old";
    public static LocString TIME_PLAYED = (LocString) "Time Played: {0} hours";
    public static LocString SCHEDULE_BUTTON_TOOLTIP = (LocString) "Manage Schedule";
    public static LocString MILESTONE_TITLE = (LocString) "Approaching Milestone";
    public static LocString MILESTONE_DESCRIPTION = (LocString) "This colony is about to hit Cycle {0}!";
  }

  public class ENDOFDAYREPORT
  {
    public static LocString REPORT_TITLE = (LocString) "DAILY REPORTS";
    public static LocString DAY_TITLE = (LocString) "Cycle {0}";
    public static LocString DAY_TITLE_TODAY = (LocString) "Cycle {0} - Today";
    public static LocString DAY_TITLE_YESTERDAY = (LocString) "Cycle {0} - Yesterday";
    public static LocString NOTIFICATION_TITLE = (LocString) "Cycle {0} report ready";
    public static LocString NOTIFICATION_TOOLTIP = (LocString) "The daily report for Cycle {0} is ready to view";
    public static LocString NEXT = (LocString) "Next";
    public static LocString PREV = (LocString) "Prev";
    public static LocString ADDED = (LocString) "Added";
    public static LocString REMOVED = (LocString) "Removed";
    public static LocString NET = (LocString) "Net";
    public static LocString DUPLICANT_DETAILS_HEADER = (LocString) "Duplicant Details:";
    public static LocString TIME_DETAILS_HEADER = (LocString) "Total Time Details:";
    public static LocString BASE_DETAILS_HEADER = (LocString) "Base Details:";
    public static LocString AVERAGE_TIME_DETAILS_HEADER = (LocString) "Average Time Details:";
    public static LocString MY_COLONY = (LocString) "my colony";
    public static LocString NONE = (LocString) "None";

    public class OXYGEN_CREATED
    {
      public static LocString NAME = (LocString) (UI.FormatAsLink("Oxygen", "OXYGEN") + " Generation:");
      public static LocString POSITIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Oxygen", "OXYGEN")} was produced by {{1}} over the course of the day";
      public static LocString NEGATIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Oxygen", "OXYGEN")} was consumed by {{1}} over the course of the day";
    }

    public class CALORIES_CREATED
    {
      public static LocString NAME = (LocString) "Calorie Generation:";
      public static LocString POSITIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Food", "FOOD")} was produced by {{1}} over the course of the day";
      public static LocString NEGATIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Food", "FOOD")} was consumed by {{1}} over the course of the day";
    }

    public class NUMBER_OF_DOMESTICATED_CRITTERS
    {
      public static LocString NAME = (LocString) "Domesticated Critters:";
      public static LocString POSITIVE_TOOLTIP = (LocString) "{0} domestic critters live in {1}";
      public static LocString NEGATIVE_TOOLTIP = (LocString) "{0} domestic critters live in {1}";
    }

    public class NUMBER_OF_WILD_CRITTERS
    {
      public static LocString NAME = (LocString) "Wild Critters:";
      public static LocString POSITIVE_TOOLTIP = (LocString) "{0} wild critters live in {1}";
      public static LocString NEGATIVE_TOOLTIP = (LocString) "{0} wild critters live in {1}";
    }

    public class ROCKETS_IN_FLIGHT
    {
      public static LocString NAME = (LocString) "Rocket Missions Underway:";
      public static LocString POSITIVE_TOOLTIP = (LocString) "{0} rockets are currently flying missions for {1}";
      public static LocString NEGATIVE_TOOLTIP = (LocString) "{0} rockets are currently flying missions for {1}";
    }

    public class STRESS_DELTA
    {
      public static LocString NAME = (LocString) (UI.FormatAsLink("Stress", "STRESS") + " Change:");
      public static LocString POSITIVE_TOOLTIP = (LocString) (UI.FormatAsLink("Stress", "STRESS") + " increased by a total of {0} for {1}");
      public static LocString NEGATIVE_TOOLTIP = (LocString) (UI.FormatAsLink("Stress", "STRESS") + " decreased by a total of {0} for {1}");
    }

    public class TRAVELTIMEWARNING
    {
      public static LocString WARNING_TITLE = (LocString) "Long Commutes";
      public static LocString WARNING_MESSAGE = (LocString) "My Duplicants are spending a significant amount of time traveling between their errands (> {0})";
    }

    public class TRAVEL_TIME
    {
      public static LocString NAME = (LocString) "Travel Time:";
      public static LocString POSITIVE_TOOLTIP = (LocString) "On average, {1} spent {0} of their time traveling between tasks";
    }

    public class WORK_TIME
    {
      public static LocString NAME = (LocString) "Work Time:";
      public static LocString POSITIVE_TOOLTIP = (LocString) "On average, {0} of {1}'s time was spent working";
    }

    public class IDLE_TIME
    {
      public static LocString NAME = (LocString) "Idle Time:";
      public static LocString POSITIVE_TOOLTIP = (LocString) "On average, {0} of {1}'s time was spent idling";
    }

    public class PERSONAL_TIME
    {
      public static LocString NAME = (LocString) "Personal Time:";
      public static LocString POSITIVE_TOOLTIP = (LocString) "On average, {0} of {1}'s time was spent tending to personal needs";
    }

    public class ENERGY_USAGE
    {
      public static LocString NAME = (LocString) (UI.FormatAsLink("Power", "POWER") + " Usage:");
      public static LocString POSITIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Power", "POWER")} was created by {{1}} over the course of the day";
      public static LocString NEGATIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Power", "POWER")} was consumed by {{1}} over the course of the day";
    }

    public class ENERGY_WASTED
    {
      public static LocString NAME = (LocString) (UI.FormatAsLink("Power", "POWER") + " Wasted:");
      public static LocString NEGATIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Power", "POWER")} was lost today due to battery runoff and overproduction in {{1}}";
    }

    public class LEVEL_UP
    {
      public static LocString NAME = (LocString) "Attribute Increases:";
      public static LocString TOOLTIP = (LocString) "Today {1} gained a total of {0} attribute levels";
    }

    public class TOILET_INCIDENT
    {
      public static LocString NAME = (LocString) "Restroom Accidents:";
      public static LocString TOOLTIP = (LocString) "{0} Duplicants couldn't quite reach the toilet in time today";
    }

    public class DISEASE_ADDED
    {
      public static LocString NAME = (LocString) (UI.FormatAsLink("Diseases", "DISEASE") + " Contracted:");
      public static LocString POSITIVE_TOOLTIP = (LocString) $"{{0}} {UI.FormatAsLink("Disease", "DISEASE")} were contracted by {{1}}";
      public static LocString NEGATIVE_TOOLTIP = (LocString) $"{{0}} {UI.FormatAsLink("Disease", "DISEASE")} were cured by {{1}}";
    }

    public class CONTAMINATED_OXYGEN_FLATULENCE
    {
      public static LocString NAME = (LocString) (UI.FormatAsLink("Flatulence", "CONTAMINATEDOXYGEN") + " Generation:");
      public static LocString POSITIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} was generated by {{1}} over the course of the day";
      public static LocString NEGATIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} was consumed by {{1}} over the course of the day";
    }

    public class CONTAMINATED_OXYGEN_TOILET
    {
      public static LocString NAME = (LocString) UI.FormatAsLink("Toilet Emissions: ", "CONTAMINATEDOXYGEN");
      public static LocString POSITIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} was generated by {{1}} over the course of the day";
      public static LocString NEGATIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} was consumed by {{1}} over the course of the day";
    }

    public class CONTAMINATED_OXYGEN_SUBLIMATION
    {
      public static LocString NAME = (LocString) (UI.FormatAsLink("Sublimation", "CONTAMINATEDOXYGEN") + ":");
      public static LocString POSITIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} was generated by {{1}} over the course of the day";
      public static LocString NEGATIVE_TOOLTIP = (LocString) $"{{0}} of {UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN")} was consumed by {{1}} over the course of the day";
    }

    public class DISEASE_STATUS
    {
      public static LocString NAME = (LocString) "Disease Status:";
      public static LocString TOOLTIP = (LocString) "There are {0} covering {1}";
    }

    public class CHORE_STATUS
    {
      public static LocString NAME = (LocString) "Errands:";
      public static LocString POSITIVE_TOOLTIP = (LocString) "{0} errands are queued for {1}";
      public static LocString NEGATIVE_TOOLTIP = (LocString) "{0} errands were completed over the course of the day by {1}";
    }

    public class NOTES
    {
      public static LocString NOTE_ENTRY_LINE_ITEM = (LocString) "{0}\n{1}: {2}";
      public static LocString BUTCHERED = (LocString) "Butchered for {0}";
      public static LocString BUTCHERED_CONTEXT = (LocString) "Butchered";
      public static LocString CRAFTED = (LocString) "Crafted a {0}";
      public static LocString CRAFTED_USED = (LocString) "{0} used as ingredient";
      public static LocString CRAFTED_CONTEXT = (LocString) "Crafted";
      public static LocString HARVESTED = (LocString) "Harvested {0}";
      public static LocString HARVESTED_CONTEXT = (LocString) "Harvested";
      public static LocString EATEN = (LocString) "{0} eaten";
      public static LocString ROTTED = (LocString) "Rotten {0}";
      public static LocString ROTTED_CONTEXT = (LocString) "Rotted";
      public static LocString GERMS = (LocString) "On {0}";
      public static LocString TIME_SPENT = (LocString) "{0}";
      public static LocString WORK_TIME = (LocString) "{0}";
      public static LocString PERSONAL_TIME = (LocString) "{0}";
      public static LocString FOODFIGHT_CONTEXT = (LocString) "{0} ingested in food fight";
    }
  }

  public static class SCHEDULEBLOCKTYPES
  {
    public static class EAT
    {
      public static LocString NAME = (LocString) "Mealtime";
      public static LocString DESCRIPTION = (LocString) "EAT:\nDuring Mealtime Duplicants will head to their assigned mess halls and eat.";
    }

    public static class SLEEP
    {
      public static LocString NAME = (LocString) "Sleep";
      public static LocString DESCRIPTION = (LocString) "SLEEP:\nWhen it's time to sleep, Duplicants will head to their assigned rooms and rest.";
    }

    public static class WORK
    {
      public static LocString NAME = (LocString) "Work";
      public static LocString DESCRIPTION = (LocString) "WORK:\nDuring Work hours Duplicants will perform any pending errands in the colony.";
    }

    public static class RECREATION
    {
      public static LocString NAME = (LocString) "Recreation";
      public static LocString DESCRIPTION = (LocString) $"HAMMER TIME:\nDuring Hammer Time, Duplicants will relieve their {UI.FormatAsLink("Stress", "STRESS")} through dance. Please be aware that no matter how hard my Duplicants try, they will absolutely not be able to touch this.";
    }

    public static class HYGIENE
    {
      public static LocString NAME = (LocString) "Hygiene";
      public static LocString DESCRIPTION = (LocString) $"HYGIENE:\nDuring {UI.FormatAsLink("Hygiene", nameof (HYGIENE))} hours Duplicants will head to their assigned washrooms to get cleaned up.";
    }
  }

  public static class SCHEDULEGROUPS
  {
    public static LocString TOOLTIP_FORMAT = (LocString) "{0}\n\n{1}";
    public static LocString MISSINGBLOCKS = (LocString) "Warning: Scheduling Issues ({0})";
    public static LocString NOTIME = (LocString) "No {0} shifts allotted";

    public static class HYGENE
    {
      public static LocString NAME = (LocString) "Bathtime";
      public static LocString DESCRIPTION = (LocString) "During Bathtime shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands.\n\nOnce they're all caught up on personal hygiene, Duplicants will head back to work.";
      public static LocString NOTIFICATION_TOOLTIP = (LocString) $"During {UI.PRE_KEYWORD}Bathtime{UI.PST_KEYWORD} shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands.";
    }

    public static class WORKTIME
    {
      public static LocString NAME = (LocString) "Work";
      public static LocString DESCRIPTION = (LocString) "During Work shifts my Duplicants must perform the errands I have placed for them throughout the colony.\n\nIt's important when scheduling to maintain a good work-life balance for my Duplicants to maintain their health and prevent Morale loss.";
      public static LocString NOTIFICATION_TOOLTIP = (LocString) $"During {UI.PRE_KEYWORD}Work{UI.PST_KEYWORD} shifts my Duplicants must perform the errands I've placed for them throughout the colony.";
    }

    public static class RECREATION
    {
      public static LocString NAME = (LocString) "Downtime";
      public static LocString DESCRIPTION = (LocString) "During Downtime my Duplicants they may do as they please.\n\nThis may include personal matters like bathroom visits or snacking, or they may choose to engage in leisure activities like socializing with friends.\n\nDowntime increases Duplicant Morale.";
      public static LocString NOTIFICATION_TOOLTIP = (LocString) $"During {UI.PRE_KEYWORD}Downtime{UI.PST_KEYWORD} shifts my Duplicants they may do as they please.";
    }

    public static class SLEEP
    {
      public static LocString NAME = (LocString) "Bedtime";
      public static LocString DESCRIPTION = (LocString) "My Duplicants use Bedtime shifts to rest up after a hard day's work.\n\nScheduling too few bedtime shifts may prevent my Duplicants from regaining enough Stamina to make it through the following day.";
      public static LocString NOTIFICATION_TOOLTIP = (LocString) $"My Duplicants use {UI.PRE_KEYWORD}Bedtime{UI.PST_KEYWORD} shifts to rest up after a hard day's work.";
    }
  }

  public class ELEMENTAL
  {
    public class AGE
    {
      public static LocString NAME = (LocString) "Age: {0}";
      public static LocString TOOLTIP = (LocString) "The selected object is {0} cycles old";
      public static LocString UNKNOWN = (LocString) "Unknown";
      public static LocString UNKNOWN_TOOLTIP = (LocString) "The age of the selected object is unknown";
    }

    public class UPTIME
    {
      public static LocString NAME = (LocString) "Uptime:\n{0}{1}: {2}\n{0}{3}: {4}\n{0}{5}: {6}";
      public static LocString THIS_CYCLE = (LocString) "This Cycle";
      public static LocString LAST_CYCLE = (LocString) "Last Cycle";
      public static LocString LAST_X_CYCLES = (LocString) "Last {0} Cycles";
    }

    public class PRIMARYELEMENT
    {
      public static LocString NAME = (LocString) "Primary Element: {0}";
      public static LocString TOOLTIP = (LocString) "The selected object is primarily composed of {0}";
    }

    public class UNITS
    {
      public static LocString NAME = (LocString) "Stack Units: {0}";
      public static LocString TOOLTIP = (LocString) "This stack contains {0} units of {1}";
    }

    public class MASS
    {
      public static LocString NAME = (LocString) "Mass: {0}";
      public static LocString TOOLTIP = (LocString) "The selected object has a mass of {0}";
    }

    public class TEMPERATURE
    {
      public static LocString NAME = (LocString) "Temperature: {0}";
      public static LocString TOOLTIP = (LocString) "The selected object's current temperature is {0}";
    }

    public class DISEASE
    {
      public static LocString NAME = (LocString) "Disease: {0}";
      public static LocString TOOLTIP = (LocString) "There are {0} on the selected object";
    }

    public class SHC
    {
      public static LocString NAME = (LocString) "Specific Heat Capacity: {0}";
      public static LocString TOOLTIP = (LocString) "{SPECIFIC_HEAT_CAPACITY} is required to heat 1 g of the selected object by 1 {TEMPERATURE_UNIT}";
    }

    public class THERMALCONDUCTIVITY
    {
      public static LocString NAME = (LocString) "Thermal Conductivity: {0}";
      public static LocString TOOLTIP = (LocString) "This object can conduct heat to other materials at a rate of {THERMAL_CONDUCTIVITY} W for each degree {TEMPERATURE_UNIT} difference\n\nBetween two objects, the rate of heat transfer will be determined by the object with the lowest Thermal Conductivity";

      public class ADJECTIVES
      {
        public static LocString VALUE_WITH_ADJECTIVE = (LocString) "{0} ({1})";
        public static LocString VERY_LOW_CONDUCTIVITY = (LocString) "Highly Insulating";
        public static LocString LOW_CONDUCTIVITY = (LocString) "Insulating";
        public static LocString MEDIUM_CONDUCTIVITY = (LocString) "Conductive";
        public static LocString HIGH_CONDUCTIVITY = (LocString) "Highly Conductive";
        public static LocString VERY_HIGH_CONDUCTIVITY = (LocString) "Extremely Conductive";
      }
    }

    public class CONDUCTIVITYBARRIER
    {
      public static LocString NAME = (LocString) "Insulation Thickness: {0}";
      public static LocString TOOLTIP = (LocString) "Thick insulation reduces an object's Thermal Conductivity";
    }

    public class VAPOURIZATIONPOINT
    {
      public static LocString NAME = (LocString) "Vaporization Point: {0}";
      public static LocString TOOLTIP = (LocString) "The selected object will evaporate into a gas at {0}";
    }

    public class MELTINGPOINT
    {
      public static LocString NAME = (LocString) "Melting Point: {0}";
      public static LocString TOOLTIP = (LocString) "The selected object will melt into a liquid at {0}";
    }

    public class OVERHEATPOINT
    {
      public static LocString NAME = (LocString) "Overheat Modifier: {0}";
      public static LocString TOOLTIP = (LocString) "This building will overheat and take damage if its temperature reaches {0}\n\nBuilding with better building materials can increase overheat temperature";
    }

    public class FREEZEPOINT
    {
      public static LocString NAME = (LocString) "Freeze Point: {0}";
      public static LocString TOOLTIP = (LocString) "The selected object will cool into a solid at {0}";
    }

    public class DEWPOINT
    {
      public static LocString NAME = (LocString) "Condensation Point: {0}";
      public static LocString TOOLTIP = (LocString) "The selected object will condense into a liquid at {0}";
    }
  }

  public class IMMIGRANTSCREEN
  {
    public static LocString IMMIGRANTSCREENTITLE = (LocString) "Select a Blueprint";
    public static LocString PROCEEDBUTTON = (LocString) "Print";
    public static LocString CANCELBUTTON = (LocString) "Cancel";
    public static LocString REJECTALL = (LocString) "Reject All";
    public static LocString EMBARK = (LocString) nameof (EMBARK);
    public static LocString SELECTDUPLICANTS = (LocString) "Select {0} Duplicants";
    public static LocString SELECTYOURCREW = (LocString) "CHOOSE THREE DUPLICANTS TO BEGIN";
    public static LocString SHUFFLE = (LocString) "REROLL";
    public static LocString SHUFFLETOOLTIP = (LocString) "Reroll for a different Duplicant";
    public static LocString BACK = (LocString) nameof (BACK);
    public static LocString CONFIRMATIONTITLE = (LocString) "Reject All Printables?";
    public static LocString CONFIRMATIONBODY = (LocString) "The Printing Pod will need time to recharge if I reject these Printables.";
    public static LocString NAME_YOUR_COLONY = (LocString) "NAME THE COLONY";
    public static LocString CARE_PACKAGE_ELEMENT_QUANTITY = (LocString) "{0} of {1}";
    public static LocString CARE_PACKAGE_ELEMENT_COUNT = (LocString) "{0} x {1}";
    public static LocString CARE_PACKAGE_ELEMENT_COUNT_ONLY = (LocString) "x {0}";
    public static LocString CARE_PACKAGE_CURRENT_AMOUNT = (LocString) "Available: {0}";
    public static LocString DUPLICATE_COLONY_NAME = (LocString) "A colony named \"{0}\" already exists";
  }

  public class METERS
  {
    public class HEALTH
    {
      public static LocString TOOLTIP = (LocString) "Health";
    }

    public class BREATH
    {
      public static LocString TOOLTIP = (LocString) "Oxygen";
    }

    public class FUEL
    {
      public static LocString TOOLTIP = (LocString) "Fuel";
    }

    public class BATTERY
    {
      public static LocString TOOLTIP = (LocString) "Battery Charge";
    }
  }
}
