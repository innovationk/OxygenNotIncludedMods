// Decompiled with JetBrains decompiler
// Type: CodexEntryGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei;
using Klei.AI;
using ProcGen;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

#nullable disable
public static class CodexEntryGenerator
{
  private static string categoryPrefx = "BUILD_CATEGORY_";
  public static readonly string FOOD_CATEGORY_ID = CodexCache.FormatLinkID("FOOD");
  public static readonly string FOOD_EFFECTS_ENTRY_ID = CodexCache.FormatLinkID("id_food_effects");
  public static readonly string TABLE_SALT_ENTRY_ID = CodexCache.FormatLinkID("id_table_salt");
  public static readonly string MINION_MODIFIERS_CATEGORY_ID = CodexCache.FormatLinkID("MINION_MODIFIERS");
  public static Tag[] HiddenRoomConstrainTags = new Tag[13]
  {
    RoomConstraints.ConstraintTags.Refrigerator,
    RoomConstraints.ConstraintTags.FarmStationType,
    RoomConstraints.ConstraintTags.LuxuryBedType,
    RoomConstraints.ConstraintTags.MassageTable,
    RoomConstraints.ConstraintTags.MessTable,
    RoomConstraints.ConstraintTags.NatureReserve,
    RoomConstraints.ConstraintTags.Park,
    RoomConstraints.ConstraintTags.SpiceStation,
    RoomConstraints.ConstraintTags.DeStressingBuilding,
    RoomConstraints.ConstraintTags.MachineShopType,
    RoomConstraints.ConstraintTags.LightDutyGeneratorType,
    RoomConstraints.ConstraintTags.HeavyDutyGeneratorType,
    RoomConstraints.ConstraintTags.BionicUpkeepType
  };
  public static Dictionary<Tag, Tag> RoomConstrainTagIcons = new Dictionary<Tag, Tag>()
  {
    [RoomConstraints.ConstraintTags.IndustrialMachinery] = (Tag) "ManualGenerator",
    [RoomConstraints.ConstraintTags.RecBuilding] = (Tag) "ArcadeMachine",
    [RoomConstraints.ConstraintTags.Clinic] = (Tag) "MedicalCot",
    [RoomConstraints.ConstraintTags.WashStation] = (Tag) "WashBasin",
    [RoomConstraints.ConstraintTags.AdvancedWashStation] = (Tag) ShowerConfig.ID,
    [RoomConstraints.ConstraintTags.ToiletType] = (Tag) "Outhouse",
    [RoomConstraints.ConstraintTags.FlushToiletType] = (Tag) "FlushToilet",
    [RoomConstraints.ConstraintTags.ScienceBuilding] = (Tag) "ResearchCenter",
    [GameTags.Decoration] = (Tag) "Canvas",
    [GameTags.Ornament] = (Tag) "artifact_RubiksCube",
    [RoomConstraints.ConstraintTags.RanchStationType] = (Tag) "RanchStation",
    [RoomConstraints.ConstraintTags.BedType] = (Tag) "Bed",
    [RoomConstraints.ConstraintTags.GeneratorType] = (Tag) "Generator",
    [RoomConstraints.ConstraintTags.RocketInterior] = (Tag) RocketControlStationConfig.ID,
    [RoomConstraints.ConstraintTags.CookTop] = (Tag) "CookingStation",
    [RoomConstraints.ConstraintTags.WarmingStation] = (Tag) "SpaceHeater",
    [RoomConstraints.ConstraintTags.PowerBuilding] = (Tag) "Battery",
    [RoomConstraints.ConstraintTags.DiningTableType] = (Tag) "DiningTable",
    [RoomConstraints.ConstraintTags.Submergible] = (Tag) "UnderwaterCritterCondo",
    [RoomConstraints.ConstraintTags.KitchenRefrigerator] = (Tag) "Refrigerator"
  };
  public static Dictionary<Tag, Tag> BuildingsCategoriesTagIcons = new Dictionary<Tag, Tag>()
  {
    [GameTags.CodexCategories.CreatureRelocator] = (Tag) "CreatureDeliveryPoint",
    [GameTags.CodexCategories.FarmBuilding] = (Tag) "FarmTile",
    [GameTags.CodexCategories.BionicBuilding] = (Tag) "OilChanger"
  };

  public static Dictionary<string, CodexEntry> GenerateBuildingEntries()
  {
    Dictionary<string, CodexEntry> categoryEntries = new Dictionary<string, CodexEntry>();
    foreach (PlanScreen.PlanInfo category in TUNING.BUILDINGS.PLANORDER)
      CodexEntryGenerator.GenerateEntriesForBuildingsInCategory(category, CodexEntryGenerator.categoryPrefx, ref categoryEntries);
    if (DlcManager.FeatureClusterSpaceEnabled())
      CodexEntryGenerator.GenerateDLC1RocketryEntries();
    CodexEntryGenerator.GenerateBuildingCategoriesEntry(CodexEntryGenerator.categoryPrefx, ref categoryEntries);
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntries);
    return categoryEntries;
  }

  private static void GenerateEntriesForBuildingsInCategory(
    PlanScreen.PlanInfo category,
    string categoryPrefx,
    ref Dictionary<string, CodexEntry> categoryEntries)
  {
    string key = HashCache.Get().Get(category.category);
    string str = CodexCache.FormatLinkID(categoryPrefx + key);
    Dictionary<string, CodexEntry> entries = new Dictionary<string, CodexEntry>();
    foreach (KeyValuePair<string, string> keyValuePair in category.buildingAndSubcategoryData)
    {
      BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) buildingDef))
      {
        CodexEntry codexEntry = CodexEntryGenerator.GenerateSingleBuildingEntry(buildingDef, str);
        if (buildingDef.ExtendCodexEntry != null)
          codexEntry = buildingDef.ExtendCodexEntry(codexEntry);
        if (codexEntry != null)
          entries.Add(codexEntry.id, codexEntry);
      }
    }
    if (entries.Count == 0)
      return;
    CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID(str), (string) Strings.Get($"STRINGS.UI.BUILDCATEGORIES.{key.ToUpper()}.NAME"), entries);
    categoryEntry.parentId = "BUILDINGS";
    categoryEntry.category = "BUILDINGS";
    categoryEntry.icon = Assets.GetSprite((HashedString) PlanScreen.IconNameMap[(HashedString) key]);
    categoryEntries.Add(str, (CodexEntry) categoryEntry);
  }

  private static void GenerateBuildingCategoriesEntry(
    string categoryPrefix,
    ref Dictionary<string, CodexEntry> categoryEntries)
  {
    string str1 = "CATEGORY";
    string str2 = CodexCache.FormatLinkID(CodexEntryGenerator.categoryPrefx + str1);
    Dictionary<string, CodexEntry> classCategoryEntry = CodexEntryGenerator.GenerateBuildingRequirementClassCategoryEntry(str2);
    Dictionary<string, CodexEntry> categoryGroupEntry = CodexEntryGenerator.GenerateBuildingCategoryGroupEntry(str2);
    Dictionary<string, CodexEntry> entries = new Dictionary<string, CodexEntry>((IDictionary<string, CodexEntry>) classCategoryEntry);
    foreach (string key in categoryGroupEntry.Keys)
      entries.Add(key, categoryGroupEntry[key]);
    CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID(str2), (string) CODEX.ROOM_REQUIREMENT_CLASS.NAME, entries);
    categoryEntry.parentId = "BUILDINGS";
    categoryEntry.category = "BUILDINGS";
    categoryEntry.icon = Assets.GetSprite((HashedString) "icon_categories_placeholder");
    categoryEntries.Add(str2, (CodexEntry) categoryEntry);
  }

  private static Dictionary<string, CodexEntry> GenerateBuildingRequirementClassCategoryEntry(
    string categoryParentName)
  {
    string id_prefix = "REQUIREMENTCLASS";
    Dictionary<string, CodexEntry> classCategoryEntry = new Dictionary<string, CodexEntry>();
    foreach (Tag allTag in RoomConstraints.ConstraintTags.AllTags)
    {
      if (!((IEnumerable<Tag>) CodexEntryGenerator.HiddenRoomConstrainTags).Contains<Tag>(allTag) && (DlcManager.FeatureClusterSpaceEnabled() || !(allTag == RoomConstraints.ConstraintTags.RocketInterior)) && (!(allTag == RoomConstraints.ConstraintTags.BionicUpkeepType) || Game.IsDlcActiveForCurrentSave("DLC3_ID")))
      {
        CodexEntry requirementClass = CodexEntryGenerator.GenerateEntryForSpecificBuildingRequirementClass(allTag, categoryParentName, id_prefix);
        classCategoryEntry.Add(requirementClass.id, requirementClass);
      }
    }
    return classCategoryEntry;
  }

  private static Dictionary<string, CodexEntry> GenerateBuildingCategoryGroupEntry(
    string parentCategory)
  {
    string id_prefix = "GROUP";
    Dictionary<string, CodexEntry> categoryGroupEntry = new Dictionary<string, CodexEntry>();
    foreach (Tag allTag in GameTags.CodexCategories.AllTags)
    {
      if (!((IEnumerable<Tag>) CodexEntryGenerator.HiddenRoomConstrainTags).Contains<Tag>(allTag))
      {
        CodexEntry buildingCategoryGroup = CodexEntryGenerator.GenerateEntryForSpecificBuildingCategoryGroup(allTag, parentCategory, id_prefix);
        if (buildingCategoryGroup != null)
          categoryGroupEntry.Add(buildingCategoryGroup.id, buildingCategoryGroup);
      }
    }
    return categoryGroupEntry;
  }

  private static CodexEntry GenerateEntryForSpecificBuildingRequirementClass(
    Tag requirementClassTag,
    string category_parentName,
    string id_prefix)
  {
    string str = "STRINGS.CODEX.ROOM_REQUIREMENT_CLASS." + requirementClassTag.ToString().ToUpper();
    List<ContentContainer> contentContainers = new List<ContentContainer>();
    contentContainers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get(str + ".TITLE"), CodexTextStyle.Title),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    List<ICodexWidget> content1 = new List<ICodexWidget>();
    content1.Add((ICodexWidget) new CodexText((string) Strings.Get(str + ".DESCRIPTION")));
    content1.Add((ICodexWidget) new CodexSpacer());
    content1.Reverse();
    contentContainers.Add(new ContentContainer(content1, ContentContainer.ContentLayout.Vertical));
    List<ICodexWidget> content2 = new List<ICodexWidget>();
    List<ICodexWidget> content3 = new List<ICodexWidget>();
    List<string> stringList = new List<string>();
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
        if (!buildingDef.DebugOnly && !buildingDef.Deprecated)
        {
          KPrefabID component = buildingDef.BuildingComplete.GetComponent<KPrefabID>();
          if (!stringList.Contains(component.name))
          {
            stringList.Add(component.name);
            if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Tags.Contains(requirementClassTag))
            {
              ICodexWidget codexWidget = (ICodexWidget) new CodexIndentedLabelWithIcon("    • " + (string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingDef.PrefabID.ToUpper()}.NAME"), CodexTextStyle.Body, Def.GetUISprite((object) component.gameObject));
              content3.Add(codexWidget);
            }
          }
        }
      }
    }
    foreach (GameObject go in Assets.GetPrefabsWithTag(requirementClassTag))
    {
      KPrefabID component = go.GetComponent<KPrefabID>();
      if (!stringList.Contains(component.name) && !go.HasTag(GameTags.HideFromCodex) && (UnityEngine.Object) component != (UnityEngine.Object) null && component.Tags.Contains(requirementClassTag))
      {
        ICodexWidget codexWidget = (ICodexWidget) new CodexIndentedLabelWithIcon("    • " + go.GetProperName(), CodexTextStyle.Body, Def.GetUISprite((object) component.gameObject));
        content3.Add(codexWidget);
        stringList.Add(go.name);
      }
    }
    if (content3.Count > 0)
    {
      ContentContainer contents = new ContentContainer(content3, ContentContainer.ContentLayout.Vertical);
      CodexCollapsibleHeader collapsibleHeader = new CodexCollapsibleHeader((string) CODEX.ROOM_REQUIREMENT_CLASS.SHARED.BUILDINGS_LIST_TITLE, contents);
      content2.Add((ICodexWidget) collapsibleHeader);
      content2.Add((ICodexWidget) new CodexSpacer());
      content2.Add((ICodexWidget) new CodexSpacer());
      content2.Reverse();
      contentContainers.Add(new ContentContainer(content2, ContentContainer.ContentLayout.Vertical));
      contentContainers.Add(contents);
    }
    StringEntry result1;
    if (Strings.TryGet(new StringKey(str + ".ROOMSREQUIRING"), out result1))
    {
      List<ICodexWidget> content4 = new List<ICodexWidget>();
      List<ICodexWidget> content5 = new List<ICodexWidget>();
      foreach (string text in result1.String.Split('\n', StringSplitOptions.None))
      {
        ICodexWidget codexWidget = (ICodexWidget) new CodexText(text);
        content5.Add(codexWidget);
      }
      CodexText codexText = new CodexText(result1.String);
      ContentContainer contents = new ContentContainer(content5, ContentContainer.ContentLayout.Vertical);
      CodexCollapsibleHeader collapsibleHeader = new CodexCollapsibleHeader((string) CODEX.ROOM_REQUIREMENT_CLASS.SHARED.ROOMS_REQUIRED_LIST_TITLE, contents);
      content4.Add((ICodexWidget) collapsibleHeader);
      content4.Add((ICodexWidget) new CodexSpacer());
      content4.Add((ICodexWidget) new CodexSpacer());
      content4.Reverse();
      contentContainers.Add(new ContentContainer(content4, ContentContainer.ContentLayout.Vertical));
      contentContainers.Add(contents);
    }
    StringEntry result2;
    if (Strings.TryGet(new StringKey(str + ".CONFLICTINGROOMS"), out result2))
    {
      List<ICodexWidget> content6 = new List<ICodexWidget>();
      List<ICodexWidget> content7 = new List<ICodexWidget>();
      foreach (string text in result2.String.Split('\n', StringSplitOptions.None))
      {
        ICodexWidget codexWidget = (ICodexWidget) new CodexText(text);
        content7.Add(codexWidget);
      }
      ContentContainer contents = new ContentContainer(content7, ContentContainer.ContentLayout.Vertical);
      CodexCollapsibleHeader collapsibleHeader = new CodexCollapsibleHeader((string) CODEX.ROOM_REQUIREMENT_CLASS.SHARED.ROOMS_CONFLICT_LIST_TITLE, contents);
      content6.Add((ICodexWidget) collapsibleHeader);
      content6.Add((ICodexWidget) new CodexSpacer());
      content6.Add((ICodexWidget) new CodexSpacer());
      content6.Reverse();
      contentContainers.Add(new ContentContainer(content6, ContentContainer.ContentLayout.Vertical));
      contentContainers.Add(contents);
    }
    contentContainers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get(str + ".FLAVOUR"))
    }, ContentContainer.ContentLayout.Vertical));
    CodexEntry entry = new CodexEntry(category_parentName, contentContainers, RoomConstraints.ConstraintTags.GetRoomConstraintLabelText(requirementClassTag));
    Tag tag;
    entry.icon = CodexEntryGenerator.RoomConstrainTagIcons.TryGetValue(requirementClassTag, out tag) ? Def.GetUISprite((object) tag).first : (Sprite) null;
    entry.parentId = category_parentName;
    CodexCache.AddEntry(CodexCache.FormatLinkID(id_prefix + requirementClassTag.ToString()), entry);
    return entry;
  }

  private static CodexEntry GenerateEntryForSpecificBuildingCategoryGroup(
    Tag categoryGroupTag,
    string category_parentName,
    string id_prefix)
  {
    string str = "STRINGS.CODEX.CATEGORIES." + categoryGroupTag.ToString().ToUpper();
    List<ContentContainer> contentContainers = new List<ContentContainer>();
    contentContainers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get(str + ".TITLE"), CodexTextStyle.Title),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    List<ICodexWidget> content1 = new List<ICodexWidget>();
    content1.Add((ICodexWidget) new CodexText((string) Strings.Get(str + ".DESCRIPTION")));
    content1.Add((ICodexWidget) new CodexSpacer());
    content1.Reverse();
    contentContainers.Add(new ContentContainer(content1, ContentContainer.ContentLayout.Vertical));
    List<ICodexWidget> content2 = new List<ICodexWidget>();
    List<ICodexWidget> content3 = new List<ICodexWidget>();
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
        if (!buildingDef.DebugOnly && !buildingDef.Deprecated)
        {
          KPrefabID component = buildingDef.BuildingComplete.GetComponent<KPrefabID>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Tags.Contains(categoryGroupTag))
          {
            ICodexWidget codexWidget = (ICodexWidget) new CodexIndentedLabelWithIcon("    • " + (string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingDef.PrefabID.ToUpper()}.NAME"), CodexTextStyle.Body, Def.GetUISprite((object) component.gameObject));
            content3.Add(codexWidget);
          }
        }
      }
    }
    if (content3.Count == 0)
      return (CodexEntry) null;
    ContentContainer contents = new ContentContainer(content3, ContentContainer.ContentLayout.Vertical);
    CodexCollapsibleHeader collapsibleHeader = new CodexCollapsibleHeader((string) CODEX.CATEGORIES.SHARED.BUILDINGS_LIST_TITLE, contents);
    content2.Add((ICodexWidget) collapsibleHeader);
    content2.Add((ICodexWidget) new CodexSpacer());
    content2.Add((ICodexWidget) new CodexSpacer());
    content2.Reverse();
    contentContainers.Add(new ContentContainer(content2, ContentContainer.ContentLayout.Vertical));
    contentContainers.Add(contents);
    contentContainers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get(str + ".FLAVOUR"))
    }, ContentContainer.ContentLayout.Vertical));
    CodexEntry entry = new CodexEntry(category_parentName, contentContainers, GameTags.CodexCategories.GetCategoryLabelText(categoryGroupTag));
    Tag tag;
    entry.icon = CodexEntryGenerator.BuildingsCategoriesTagIcons.TryGetValue(categoryGroupTag, out tag) ? Def.GetUISprite((object) tag).first : (Sprite) null;
    entry.parentId = category_parentName;
    CodexCache.AddEntry(CodexCache.FormatLinkID(id_prefix + categoryGroupTag.ToString()), entry);
    return entry;
  }

  private static CodexEntry GenerateSingleBuildingEntry(BuildingDef def, string categoryEntryID)
  {
    if (def.DebugOnly || def.Deprecated)
      return (CodexEntry) null;
    List<ContentContainer> contentContainerList = new List<ContentContainer>();
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexText(def.Name, CodexTextStyle.Title));
    Tech techForTechItem = Db.Get().Techs.TryGetTechForTechItem(def.PrefabID);
    if (techForTechItem != null)
      content.Add((ICodexWidget) new CodexLabelWithIcon(techForTechItem.Name, CodexTextStyle.Body, new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "research_type_alpha_icon"), Color.white)));
    content.Add((ICodexWidget) new CodexDividerLine());
    contentContainerList.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
    CodexEntryGenerator.GenerateImageContainers(def.GetUISprite(), contentContainerList);
    CodexEntryGenerator.GenerateBuildingDescriptionContainers(def, contentContainerList);
    CodexEntryGenerator.GenerateFabricatorContainers(def.BuildingComplete, contentContainerList);
    CodexEntryGenerator.GenerateReceptacleContainers(def.BuildingComplete, contentContainerList);
    CodexEntryGenerator.GenerateConfigurableConsumerContainers(def.BuildingComplete, contentContainerList);
    CodexEntryGenerator.GenerateManualConversionEntries(def.BuildingComplete.PrefabID(), contentContainerList);
    CodexEntry entry = new CodexEntry(categoryEntryID, contentContainerList, (string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{def.PrefabID.ToUpper()}.NAME"));
    entry.icon = def.GetUISprite();
    entry.parentId = categoryEntryID;
    CodexCache.AddEntry(def.PrefabID, entry);
    return entry;
  }

  private static void GenerateDLC1RocketryEntries()
  {
    PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER.Find((Predicate<PlanScreen.PlanInfo>) (match => match.category == new HashedString("Rocketry")));
    foreach (string prefab_id in SelectModuleSideScreen.moduleButtonSortOrder)
    {
      string str = HashCache.Get().Get(planInfo.category);
      string categoryEntryID = CodexCache.FormatLinkID(CodexEntryGenerator.categoryPrefx + str);
      BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
      if (!((UnityEngine.Object) buildingDef == (UnityEngine.Object) null))
      {
        CodexEntry singleBuildingEntry = CodexEntryGenerator.GenerateSingleBuildingEntry(buildingDef, categoryEntryID);
        List<ICodexWidget> content = new List<ICodexWidget>();
        content.Add((ICodexWidget) new CodexSpacer());
        content.Add((ICodexWidget) new CodexText((string) UI.CLUSTERMAP.ROCKETS.MODULE_STATS.NAME_HEADER, CodexTextStyle.Subtitle));
        content.Add((ICodexWidget) new CodexSpacer());
        content.Add((ICodexWidget) new CodexText((string) UI.CLUSTERMAP.ROCKETS.SPEED.TOOLTIP));
        RocketModuleCluster component1 = buildingDef.BuildingComplete.GetComponent<RocketModuleCluster>();
        float burden = component1.performanceStats.Burden;
        float enginePower = component1.performanceStats.EnginePower;
        RocketEngineCluster component2 = buildingDef.BuildingComplete.GetComponent<RocketEngineCluster>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          content.Add((ICodexWidget) new CodexText($"    • {(string) UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME_MAX_SUPPORTED}{component2.maxHeight.ToString()}"));
        content.Add((ICodexWidget) new CodexText($"    • {(string) UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME_RAW}{buildingDef.HeightInCells.ToString()}"));
        if ((double) burden != 0.0)
          content.Add((ICodexWidget) new CodexText($"    • {(string) UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.NAME}{burden.ToString()}"));
        if ((double) enginePower != 0.0)
          content.Add((ICodexWidget) new CodexText($"    • {(string) UI.CLUSTERMAP.ROCKETS.POWER_MODULE.NAME}{enginePower.ToString()}"));
        ContentContainer container = new ContentContainer(content, ContentContainer.ContentLayout.Vertical);
        singleBuildingEntry.AddContentContainer(container);
      }
    }
  }

  public static void GeneratePageNotFound()
  {
    CodexCache.AddEntry("PageNotFound", new CodexEntry("ROOT", new List<ContentContainer>()
    {
      new ContentContainer()
      {
        content = {
          (ICodexWidget) new CodexText((string) CODEX.PAGENOTFOUND.TITLE, CodexTextStyle.Title),
          (ICodexWidget) new CodexText((string) CODEX.PAGENOTFOUND.SUBTITLE, CodexTextStyle.Subtitle),
          (ICodexWidget) new CodexDividerLine(),
          (ICodexWidget) new CodexImage(312, 312, Assets.GetSprite((HashedString) "outhouseMessage"))
        }
      }
    }, (string) CODEX.PAGENOTFOUND.TITLE)
    {
      searchOnly = true
    });
  }

  public static Dictionary<string, CodexEntry> GenerateRoomsEntries()
  {
    Dictionary<string, CodexEntry> result = new Dictionary<string, CodexEntry>();
    RoomTypes roomTypesData = Db.Get().RoomTypes;
    string parentCategoryName = "ROOMS";
    Action<RoomTypeCategory> action = (Action<RoomTypeCategory>) (roomCategory =>
    {
      bool flag = false;
      CodexEntry entry = new CodexEntry(parentCategoryName, new List<ContentContainer>(), roomCategory.Name);
      for (int idx = 0; idx < roomTypesData.Count; ++idx)
      {
        RoomType roomType = roomTypesData[idx];
        if (roomType.category.Id == roomCategory.Id)
        {
          if (!flag)
          {
            flag = true;
            entry.parentId = parentCategoryName;
            entry.name = roomCategory.Name;
            CodexCache.AddEntry(parentCategoryName + roomCategory.Id, entry);
            result.Add(parentCategoryName + roomType.category.Id, entry);
            ContentContainer container = new ContentContainer(new List<ICodexWidget>()
            {
              (ICodexWidget) new CodexImage(312, 312, Assets.GetSprite((HashedString) roomCategory.icon))
            }, ContentContainer.ContentLayout.Vertical);
            entry.AddContentContainer(container);
          }
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(roomType.Name, contentContainerList);
          CodexEntryGenerator.GenerateRoomTypeDescriptionContainers(roomType, contentContainerList);
          CodexEntryGenerator.GenerateRoomTypeDetailsContainers(roomType, contentContainerList);
          entry.subEntries.Add(new SubEntry(roomType.Id, parentCategoryName + roomType.category.Id, contentContainerList, roomType.Name)
          {
            icon = Assets.GetSprite((HashedString) roomCategory.icon),
            iconColor = Color.white
          });
        }
      }
    });
    action(Db.Get().RoomTypeCategories.Agricultural);
    action(Db.Get().RoomTypeCategories.Bathroom);
    if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
      action(Db.Get().RoomTypeCategories.Bionic);
    action(Db.Get().RoomTypeCategories.Food);
    action(Db.Get().RoomTypeCategories.Hospital);
    action(Db.Get().RoomTypeCategories.Industrial);
    action(Db.Get().RoomTypeCategories.Park);
    action(Db.Get().RoomTypeCategories.Recreation);
    action(Db.Get().RoomTypeCategories.Sleep);
    action(Db.Get().RoomTypeCategories.Science);
    return result;
  }

  public static Dictionary<string, CodexEntry> GeneratePlantEntries()
  {
    Dictionary<string, CodexEntry> plantEntries = new Dictionary<string, CodexEntry>();
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Harvestable>();
    prefabsWithComponent.AddRange((IEnumerable<GameObject>) Assets.GetPrefabsWithComponent<WiltCondition>());
    foreach (GameObject gameObject in prefabsWithComponent)
    {
      KPrefabID component1 = gameObject.GetComponent<KPrefabID>();
      SeedProducer component2 = gameObject.GetComponent<SeedProducer>();
      KPrefabID component3 = (UnityEngine.Object) component2 == (UnityEngine.Object) null ? (KPrefabID) null : Assets.GetPrefab((Tag) component2.seedInfo.seedId).GetComponent<KPrefabID>();
      int num;
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
        num = component3.HasAnyTags(new List<Tag>()
        {
          GameTags.Seed,
          GameTags.CropSeed
        }) ? 1 : 0;
      else
        num = 0;
      bool flag = num != 0;
      Dictionary<string, CodexEntry> dictionary1 = plantEntries;
      Tag tag = component1.PrefabID();
      string key1 = tag.ToString();
      if (!dictionary1.ContainsKey(key1) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) component1) && !component1.HasTag(GameTags.DeprecatedContent) && !component1.HasTag(GameTags.HideFromCodex))
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        Sprite first = Def.GetUISprite((object) gameObject).first;
        CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
        CodexEntryGenerator.GeneratePlantDescriptionContainers(gameObject, contentContainerList);
        CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(gameObject.PrefabID(), contentContainerList);
        CodexEntry entry = new CodexEntry("PLANTS", contentContainerList, gameObject.GetProperName());
        if (flag)
        {
          List<ContentContainer> containers = new List<ContentContainer>();
          List<ICodexWidget> content = new List<ICodexWidget>();
          InfoDescription component4 = component3.GetComponent<InfoDescription>();
          CodexText codexText1 = new CodexText((string) CODEX.HEADERS.GROWNFROMSEED, CodexTextStyle.Subtitle);
          CodexText codexText2 = new CodexText(component3.GetProperName(), CodexTextStyle.Title);
          CodexText codexText3 = new CodexText(component4.description);
          content.Add((ICodexWidget) new CodexLargeSpacer());
          content.Add((ICodexWidget) codexText1);
          content.Add((ICodexWidget) new CodexDividerLine());
          content.Add((ICodexWidget) codexText2);
          content.Add((ICodexWidget) codexText3);
          ContentContainer contentContainer = new ContentContainer(content, ContentContainer.ContentLayout.Vertical);
          containers.Add(contentContainer);
          CodexEntryGenerator.GenerateImageContainers(Def.GetUISprite((object) component2.seedInfo.seedId).first, containers);
          tag = component3.PrefabID();
          string id1 = tag.ToString();
          if (CodexCache.FindSubEntry(id1) != null)
            id1 = $"{component1.ToString()}_{id1}";
          CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(component3.PrefabID(), containers);
          string id2 = id1;
          tag = gameObject.PrefabID();
          string parentEntryID = tag.ToString();
          List<ContentContainer> contentContainers = containers;
          string properName = component3.GetProperName();
          SubEntry subEntry = new SubEntry(id2, parentEntryID, contentContainers, properName);
          entry.subEntries.Add(subEntry);
        }
        entry.parentId = "PLANTS";
        entry.icon = first;
        tag = gameObject.PrefabID();
        CodexCache.AddEntry(tag.ToString(), entry);
        Dictionary<string, CodexEntry> dictionary2 = plantEntries;
        tag = gameObject.PrefabID();
        string key2 = tag.ToString();
        CodexEntry codexEntry = entry;
        dictionary2.Add(key2, codexEntry);
      }
    }
    return plantEntries;
  }

  public static Dictionary<string, CodexEntry> GenerateFoodEntries()
  {
    Dictionary<string, CodexEntry> foodEntries = new Dictionary<string, CodexEntry>();
    foreach (EdiblesManager.FoodInfo allFoodType in EdiblesManager.GetAllFoodTypes())
    {
      GameObject prefab = Assets.GetPrefab((Tag) allFoodType.Id);
      DebugUtil.DevAssert((UnityEngine.Object) prefab != (UnityEngine.Object) null, "Food prefab is null: " + allFoodType.Id);
      if (!((UnityEngine.Object) prefab == (UnityEngine.Object) null))
      {
        KPrefabID component = prefab.GetComponent<KPrefabID>();
        if (!component.HasTag(GameTags.DeprecatedContent) && !component.HasTag(GameTags.IncubatableEgg) && !component.HasTag(GameTags.Seed) && !component.HasTag(GameTags.CropSeed))
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(allFoodType.Name, contentContainerList);
          Sprite first = Def.GetUISprite((object) allFoodType.ConsumableId).first;
          CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
          CodexEntryGenerator.GenerateFoodDescriptionContainers(allFoodType, contentContainerList);
          CodexEntryGenerator.GenerateRecipeContainers(allFoodType.ConsumableId.ToTag(), contentContainerList);
          CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(allFoodType.ConsumableId.ToTag(), contentContainerList);
          CodexEntry entry = new CodexEntry(CodexEntryGenerator.FOOD_CATEGORY_ID, contentContainerList, allFoodType.Name);
          entry.icon = first;
          entry.parentId = CodexEntryGenerator.FOOD_CATEGORY_ID;
          CodexCache.AddEntry(allFoodType.Id, entry);
          foodEntries.Add(allFoodType.Id, entry);
        }
      }
    }
    CodexEntry foodEffectEntry = CodexEntryGenerator.GenerateFoodEffectEntry();
    CodexCache.AddEntry(CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID, foodEffectEntry);
    foodEntries.Add(CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID, foodEffectEntry);
    CodexEntry tabelSaltEntry = CodexEntryGenerator.GenerateTabelSaltEntry();
    CodexCache.AddEntry(CodexEntryGenerator.TABLE_SALT_ENTRY_ID, tabelSaltEntry);
    foodEntries.Add(CodexEntryGenerator.TABLE_SALT_ENTRY_ID, tabelSaltEntry);
    return foodEntries;
  }

  private static CodexEntry GenerateFoodEffectEntry()
  {
    List<ICodexWidget> content1 = new List<ICodexWidget>();
    string foodCategoryId = CodexEntryGenerator.FOOD_CATEGORY_ID;
    List<ContentContainer> contentContainers1 = new List<ContentContainer>();
    contentContainers1.Add(new ContentContainer(content1, ContentContainer.ContentLayout.Vertical));
    string foodeffects = (string) CODEX.HEADERS.FOODEFFECTS;
    CodexEntry foodEffectEntry = new CodexEntry(foodCategoryId, contentContainers1, foodeffects);
    foodEffectEntry.parentId = CodexEntryGenerator.FOOD_CATEGORY_ID;
    foodEffectEntry.icon = Assets.GetSprite((HashedString) "icon_category_food");
    Dictionary<string, List<EdiblesManager.FoodInfo>> dictionary = new Dictionary<string, List<EdiblesManager.FoodInfo>>();
    foreach (EdiblesManager.FoodInfo allFoodType in EdiblesManager.GetAllFoodTypes())
    {
      foreach (string effect in allFoodType.Effects)
      {
        List<EdiblesManager.FoodInfo> foodInfoList;
        if (!dictionary.TryGetValue(effect, out foodInfoList))
        {
          foodInfoList = new List<EdiblesManager.FoodInfo>();
          dictionary[effect] = foodInfoList;
        }
        foodInfoList.Add(allFoodType);
      }
    }
    foreach (KeyValuePair<string, List<EdiblesManager.FoodInfo>> keyValuePair in dictionary)
    {
      string str1;
      List<EdiblesManager.FoodInfo> foodInfoList1;
      keyValuePair.Deconstruct(ref str1, ref foodInfoList1);
      string id1 = str1;
      List<EdiblesManager.FoodInfo> foodInfoList2 = foodInfoList1;
      Effect effect = Db.Get().effects.Get(id1);
      string id2 = $"{CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID}::{id1.ToUpper()}";
      string text1 = (string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{id1.ToUpper()}.NAME");
      string text2 = (string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{id1.ToUpper()}.DESCRIPTION");
      List<ICodexWidget> content2 = new List<ICodexWidget>();
      content2.Add((ICodexWidget) new CodexText(text1, CodexTextStyle.Title));
      string foodEffectsEntryId = CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID;
      List<ContentContainer> contentContainers2 = new List<ContentContainer>();
      contentContainers2.Add(new ContentContainer(content2, ContentContainer.ContentLayout.Vertical));
      string name = text1;
      SubEntry subEntry = new SubEntry(id2, foodEffectsEntryId, contentContainers2, name);
      foodEffectEntry.subEntries.Add(subEntry);
      content2.Add((ICodexWidget) new CodexText(text2));
      foreach (AttributeModifier selfModifier in effect.SelfModifiers)
      {
        string str2 = (string) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{selfModifier.AttributeId.ToUpper()}.NAME");
        string tooltip = (string) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{selfModifier.AttributeId.ToUpper()}.DESC");
        content2.Add((ICodexWidget) new CodexTextWithTooltip($"    • {str2}: {selfModifier.GetFormattedString()}", tooltip));
      }
      content2.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.FOODSWITHEFFECT + ": "));
      foreach (EdiblesManager.FoodInfo foodInfo in foodInfoList2)
        content2.Add((ICodexWidget) new CodexTextWithTooltip("    • " + foodInfo.Name, foodInfo.Description));
      content2.Add((ICodexWidget) new CodexSpacer());
    }
    return foodEffectEntry;
  }

  public static Dictionary<string, CodexEntry> GenerateDuplicantEntries()
  {
    string str = "DUPLICANTS";
    CodexEntry entry = new CodexEntry("DUPLICANTSCATEGORY", new List<ContentContainer>(), (string) CODEX.DUPLICANT.SPECIES_TITLE);
    entry.icon = Assets.GetSprite((HashedString) "codexIconDupes");
    entry.parentId = "DUPLICANTSCATEGORY";
    CodexCache.AddEntry(str, entry);
    CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry("ROLES", (string) UI.CODEX.CATEGORYNAMES.ROLES, CodexEntryGenerator.GenerateRoleEntries(), Assets.GetSprite((HashedString) "codexIconSkills"));
    categoryEntry.parentId = entry.parentId;
    Dictionary<string, CodexEntry> duplicantEntries = new Dictionary<string, CodexEntry>();
    duplicantEntries.Add(str, entry);
    duplicantEntries.Add("ROLES", (CodexEntry) categoryEntry);
    CodexEntryGenerator.PopulateCategoryEntries(new List<CategoryEntry>()
    {
      categoryEntry
    });
    List<ContentContainer> contentContainers1 = new List<ContentContainer>()
    {
      new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText((string) CODEX.STANDARD.TITLE, CodexTextStyle.Title),
        (ICodexWidget) new CodexText((string) CODEX.STANDARD.SUBTITLE, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexDividerLine()
      }, ContentContainer.ContentLayout.Vertical),
      new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText((string) CODEX.STANDARD.HEADER_1, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText(CODEX.STANDARD.PARAGRAPH_1.Replace("{time}", GameUtil.GetFormattedCycles(100f / DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND)).Replace("{O2gperSec}", GameUtil.GetFormattedMass(DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND, GameUtil.TimeSlice.PerSecond)).Replace("{CO2gperSec}", GameUtil.GetFormattedMass(DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_TO_CO2_CONVERSION, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram)).Replace("{caloriesrequired}", GameUtil.GetFormattedCalories(DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE * -1f))),
        (ICodexWidget) new CodexText((string) CODEX.STANDARD.HEADER_2, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText((string) CODEX.STANDARD.PARAGRAPH_2)
      }, ContentContainer.ContentLayout.Vertical)
    };
    SubEntry subEntry1 = new SubEntry("DuplicantStandard", str, contentContainers1, (string) CODEX.STANDARD.TITLE);
    CodexCache.FindEntry(str).subEntries.Add(subEntry1);
    List<ContentContainer> contentContainers2 = new List<ContentContainer>()
    {
      new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText((string) CODEX.BIONIC.TITLE, CodexTextStyle.Title),
        (ICodexWidget) new CodexText((string) CODEX.BIONIC.SUBTITLE, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexDividerLine()
      }, ContentContainer.ContentLayout.Vertical),
      new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText((string) CODEX.BIONIC.HEADER_1, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText(CODEX.BIONIC.PARAGRAPH_1.Replace("{time}", GameUtil.GetFormattedCycles(GunkMonitor.GUNK_CAPACITY / 0.0333333351f)).Replace("{number}", GameUtil.GetFormattedCycles(BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG / DUPLICANTSTATS.BIONICS.BaseStats.OXYGEN_USED_PER_SECOND))),
        (ICodexWidget) new CodexText((string) CODEX.BIONIC.HEADER_2, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText((string) CODEX.BIONIC.PARAGRAPH_2)
      }, ContentContainer.ContentLayout.Vertical)
    };
    SubEntry subEntry2 = new SubEntry("DuplicantBionic", str, contentContainers2, (string) CODEX.BIONIC.TITLE);
    CodexCache.FindEntry(str).subEntries.Add(subEntry2);
    return duplicantEntries;
  }

  private static CodexEntry GenerateTabelSaltEntry()
  {
    LocString name = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME;
    LocString desc = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC;
    Sprite sprite = Assets.GetSprite((HashedString) "ui_food_table_salt");
    List<ContentContainer> contentContainerList = new List<ContentContainer>();
    CodexEntryGenerator.GenerateImageContainers(sprite, contentContainerList);
    contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) name, CodexTextStyle.Title),
      (ICodexWidget) new CodexText((string) desc)
    }, ContentContainer.ContentLayout.Vertical));
    return new CodexEntry(CodexEntryGenerator.FOOD_CATEGORY_ID, contentContainerList, (string) name)
    {
      parentId = CodexEntryGenerator.FOOD_CATEGORY_ID,
      icon = sprite
    };
  }

  public static Dictionary<string, CodexEntry> GenerateMinionModifierEntries()
  {
    Dictionary<string, CodexEntry> minionModifierEntries = new Dictionary<string, CodexEntry>();
    foreach (Effect resource in Db.Get().effects.resources)
    {
      if (resource.triggerFloatingText || !resource.showInUI)
      {
        string id = resource.Id;
        string str1 = "AVOID_COLLISIONS_" + id;
        StringEntry result1;
        StringEntry result2;
        if (Strings.TryGet($"STRINGS.DUPLICANTS.MODIFIERS.{id.ToUpper()}.NAME", out result1) && (Strings.TryGet($"STRINGS.DUPLICANTS.MODIFIERS.{id.ToUpper()}.DESCRIPTION", out result2) || Strings.TryGet($"STRINGS.DUPLICANTS.MODIFIERS.{id.ToUpper()}.TOOLTIP", out result2)))
        {
          string str2 = result1.String;
          string str3 = result2.String;
          List<ContentContainer> contentContainers = new List<ContentContainer>();
          ContentContainer contentContainer = new ContentContainer();
          List<ICodexWidget> content = contentContainer.content;
          content.Add((ICodexWidget) new CodexText(resource.Name, CodexTextStyle.Title));
          content.Add((ICodexWidget) new CodexText(resource.description));
          foreach (AttributeModifier selfModifier in resource.SelfModifiers)
          {
            string str4 = (string) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{selfModifier.AttributeId.ToUpper()}.NAME");
            string tooltip = (string) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{selfModifier.AttributeId.ToUpper()}.DESC");
            content.Add((ICodexWidget) new CodexTextWithTooltip($"    • {str4}: {selfModifier.GetFormattedString()}", tooltip));
          }
          content.Add((ICodexWidget) new CodexSpacer());
          contentContainers.Add(contentContainer);
          CodexEntry entry = new CodexEntry(CodexEntryGenerator.MINION_MODIFIERS_CATEGORY_ID, contentContainers, resource.Name);
          entry.icon = Assets.GetSprite((HashedString) resource.customIcon);
          entry.parentId = CodexEntryGenerator.MINION_MODIFIERS_CATEGORY_ID;
          CodexCache.AddEntry(str1, entry);
          minionModifierEntries.Add(str1, entry);
        }
      }
    }
    return minionModifierEntries;
  }

  public static Dictionary<string, CodexEntry> GenerateTechEntries()
  {
    Dictionary<string, CodexEntry> techEntries = new Dictionary<string, CodexEntry>();
    foreach (Tech resource in Db.Get().Techs.resources)
    {
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
      CodexEntryGenerator.GenerateTechDescriptionContainers(resource, contentContainerList);
      CodexEntryGenerator.GeneratePrerequisiteTechContainers(resource, contentContainerList);
      CodexEntryGenerator.GenerateUnlockContainers(resource, contentContainerList);
      CodexEntry entry = new CodexEntry("TECH", contentContainerList, resource.Name);
      TechItem unlockedItem = resource.unlockedItems.Count != 0 ? resource.unlockedItems[0] : (TechItem) null;
      entry.icon = unlockedItem == null ? (Sprite) null : unlockedItem.getUISprite("ui", false);
      entry.parentId = "TECH";
      CodexCache.AddEntry(resource.Id, entry);
      techEntries.Add(resource.Id, entry);
    }
    return techEntries;
  }

  public static Dictionary<string, CodexEntry> GenerateRoleEntries()
  {
    Dictionary<string, CodexEntry> roleEntries = new Dictionary<string, CodexEntry>();
    foreach (Skill resource in Db.Get().Skills.resources)
    {
      if (!resource.deprecated && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) resource))
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        Sprite sprite = Assets.GetSprite((HashedString) resource.hat);
        CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
        CodexEntryGenerator.GenerateImageContainers(sprite, contentContainerList);
        CodexEntryGenerator.GenerateGenericDescriptionContainers(resource.description, contentContainerList);
        CodexEntryGenerator.GenerateSkillRequirementsAndPerksContainers(resource, contentContainerList);
        CodexEntryGenerator.GenerateRelatedSkillContainers(resource, contentContainerList);
        CodexEntry entry = new CodexEntry("ROLES", contentContainerList, resource.Name);
        entry.parentId = "ROLES";
        entry.icon = sprite;
        CodexCache.AddEntry(resource.Id, entry);
        roleEntries.Add(resource.Id, entry);
      }
    }
    return roleEntries;
  }

  public static Dictionary<string, CodexEntry> GenerateGeyserEntries()
  {
    Dictionary<string, CodexEntry> geyserEntries = new Dictionary<string, CodexEntry>();
    foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.GeyserFeature))
    {
      KPrefabID component = go.GetComponent<KPrefabID>();
      if (!component.HasTag(GameTags.DeprecatedContent) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) component))
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(go.GetProperName(), contentContainerList);
        Sprite first = Def.GetUISprite((object) go).first;
        CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
        List<ICodexWidget> content = new List<ICodexWidget>();
        string str1 = go.PrefabID().ToString();
        string str2 = str1.ToUpper();
        if (str2.StartsWith("GEYSERGENERIC_"))
          str2 = str2.Substring("GEYSERGENERIC_".Length);
        content.Add((ICodexWidget) new CodexText((string) Strings.Get($"STRINGS.CREATURES.SPECIES.GEYSER.{str2}.DESC")));
        content.Add((ICodexWidget) new CodexText((string) UI.CODEX.GEYSERS.DESC));
        contentContainerList.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
        CodexEntry entry = new CodexEntry("GEYSERS", contentContainerList, go.GetProperName());
        entry.icon = first;
        entry.parentId = "GEYSERS";
        entry.id = str1;
        CodexCache.AddEntry(entry.id, entry);
        geyserEntries.Add(entry.id, entry);
      }
    }
    return geyserEntries;
  }

  public static Dictionary<string, CodexEntry> GenerateEquipmentEntries()
  {
    Dictionary<string, CodexEntry> equipmentEntries = new Dictionary<string, CodexEntry>();
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Equippable>();
    if (prefabsWithComponent != null)
    {
      foreach (GameObject go in prefabsWithComponent)
      {
        if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) go.GetComponent<KPrefabID>()))
        {
          bool flag = false;
          Equippable component = go.GetComponent<Equippable>();
          if (component.def.AdditionalTags != null)
          {
            foreach (Tag additionalTag in component.def.AdditionalTags)
            {
              if (additionalTag == GameTags.DeprecatedContent)
              {
                flag = true;
                break;
              }
            }
          }
          if (!flag && !component.hideInCodex)
          {
            List<ContentContainer> contentContainerList = new List<ContentContainer>();
            CodexEntryGenerator.GenerateTitleContainers(go.GetProperName(), contentContainerList);
            Sprite first = Def.GetUISprite((object) go).first;
            CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
            List<ICodexWidget> content = new List<ICodexWidget>();
            Tag tag = go.PrefabID();
            string str1 = tag.ToString();
            if (component.def.Id == "SleepClinicPajamas")
            {
              content.Add((ICodexWidget) new CodexText((string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{str1.ToUpper()}.DESC")));
              content.Add((ICodexWidget) new CodexText((string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{str1.ToUpper()}.EFFECT")));
            }
            else
              content.Add((ICodexWidget) new CodexText((string) Strings.Get($"STRINGS.EQUIPMENT.PREFABS.{str1.ToUpper()}.RECIPE_DESC")));
            if (component.def.AttributeModifiers.Count > 0 || component.def.additionalDescriptors.Count > 0)
            {
              content.Add((ICodexWidget) new CodexSpacer());
              content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.EQUIPMENTEFFECTS, CodexTextStyle.Subtitle));
            }
            foreach (AttributeModifier attributeModifier in component.def.AttributeModifiers)
              content.Add((ICodexWidget) new CodexTextWithTooltip("    • " + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) attributeModifier.GetName(), (object) attributeModifier.GetFormattedString()), Db.Get().Attributes.Get(attributeModifier.AttributeId).Description));
            foreach (Descriptor additionalDescriptor in component.def.additionalDescriptors)
              content.Add((ICodexWidget) new CodexTextWithTooltip("    • " + additionalDescriptor.text, additionalDescriptor.tooltipText));
            contentContainerList.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
            CodexEntry entry = new CodexEntry("EQUIPMENT", contentContainerList, go.GetProperName());
            entry.icon = first;
            entry.parentId = "EQUIPMENT";
            CodexEntry codexEntry = entry;
            tag = go.PrefabID();
            string str2 = tag.ToString();
            codexEntry.id = str2;
            CodexCache.AddEntry(entry.id, entry);
            equipmentEntries.Add(entry.id, entry);
          }
        }
      }
    }
    return equipmentEntries;
  }

  public static void GenerateElectrobankEntries()
  {
    CodexEntry entry = new CodexEntry("ROOT", new List<ContentContainer>(), (string) CODEX.ELECTROBANK.TITLE);
    entry.id = "ELECTROBANKS";
    entry.icon = Assets.GetSprite((HashedString) "upgrade_disc");
    entry.parentId = "EQUIPMENT";
    CodexCache.AddEntry(entry.id, entry);
    foreach (GameObject go in Assets.GetPrefabsWithComponent<Electrobank>())
    {
      if (!go.HasTag(GameTags.DeprecatedContent))
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(go.GetProperName(), contentContainerList);
        Sprite first = Def.GetUISprite((object) go).first;
        CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
        contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexText(go.GetComponent<InfoDescription>().description),
          (ICodexWidget) new CodexSpacer()
        }, ContentContainer.ContentLayout.Vertical));
        string id = UI.ExtractLinkID(go.GetProperName());
        if (CodexCache.FindEntry("ELECTROBANKS").subEntries.Find((Predicate<SubEntry>) (x => x.id == id)) == null)
          CodexCache.FindEntry("ELECTROBANKS").subEntries.Add(new SubEntry(id, "ELECTROBANKS", contentContainerList, go.GetProperName())
          {
            icon = first
          });
      }
    }
  }

  public static void GenerateBionicUpgradeEntries()
  {
    CodexEntry entry = new CodexEntry("ROOT", new List<ContentContainer>(), (string) CODEX.BIONICBOOSTER.TITLE);
    entry.id = "BOOSTER";
    entry.icon = Assets.GetSprite((HashedString) "upgrade_disc");
    entry.parentId = "EQUIPMENT";
    CodexCache.AddEntry(entry.id, entry);
    foreach (GameObject go1 in Assets.GetPrefabsWithComponent<BionicUpgradeComponent>())
    {
      BionicUpgradeComponent component = go1.GetComponent<BionicUpgradeComponent>();
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      CodexEntryGenerator.GenerateTitleContainers(go1.GetProperName(), contentContainerList);
      Sprite first = Def.GetUISprite((object) go1).first;
      CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
      List<ICodexWidget> content = new List<ICodexWidget>();
      GameObject go2 = go1;
      foreach (Descriptor descriptor in component.GetDescriptors(go2))
        content.Add((ICodexWidget) new CodexText(descriptor.text));
      content.Add((ICodexWidget) new CodexSpacer());
      contentContainerList.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
      string id = UI.ExtractLinkID(go1.GetProperName());
      if (CodexCache.FindEntry("BOOSTER").subEntries.Find((Predicate<SubEntry>) (x => x.id == id)) == null)
        CodexCache.FindEntry("BOOSTER").subEntries.Add(new SubEntry(id, "BOOSTER", contentContainerList, go1.GetProperName())
        {
          icon = first
        });
    }
  }

  public static Dictionary<string, CodexEntry> GenerateBiomeEntries()
  {
    Dictionary<string, CodexEntry> biomeEntries = new Dictionary<string, CodexEntry>();
    ListPool<YamlIO.Error, WorldGen>.PooledList world_gen_errors = ListPool<YamlIO.Error, WorldGen>.Allocate();
    string str1 = Application.streamingAssetsPath + "/worldgen/worlds/";
    string str2 = Application.streamingAssetsPath + "/worldgen/biomes/";
    string str3 = Application.streamingAssetsPath + "/worldgen/subworlds/";
    WorldGen.LoadSettings();
    Dictionary<string, List<WeightedSubworldName>> dictionary = new Dictionary<string, List<WeightedSubworldName>>();
    foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
    {
      ClusterLayout clusterLayout = keyValuePair.Value;
      string filePath = clusterLayout.filePath;
      foreach (WorldPlacement worldPlacement in clusterLayout.worldPlacements)
      {
        foreach (WeightedSubworldName subworldFile in SettingsCache.worlds.GetWorldData(worldPlacement.world).subworldFiles)
        {
          string str4 = subworldFile.name.Substring(subworldFile.name.LastIndexOf("/"));
          string str5 = subworldFile.name.Substring(0, subworldFile.name.Length - str4.Length);
          string key = str5.Substring(str5.LastIndexOf("/") + 1);
          if (!(key == "subworlds"))
          {
            if (!dictionary.ContainsKey(key))
              dictionary.Add(key, new List<WeightedSubworldName>()
              {
                subworldFile
              });
            else
              dictionary[key].Add(subworldFile);
          }
        }
      }
    }
    foreach (KeyValuePair<string, List<WeightedSubworldName>> keyValuePair1 in dictionary)
    {
      string str6 = CodexCache.FormatLinkID(keyValuePair1.Key);
      Tuple<Sprite, Color> tuple = (Tuple<Sprite, Color>) null;
      string name1 = (string) Strings.Get($"STRINGS.SUBWORLDS.{str6.ToUpper()}.NAME");
      if (name1.Contains("MISSING"))
        name1 = str6 + " (missing string key)";
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      CodexEntryGenerator.GenerateTitleContainers(name1, contentContainerList);
      string name2 = $"biomeIcon{char.ToUpper(str6[0]).ToString()}{str6.Substring(1).ToLower()}";
      Sprite sprite = Assets.GetSprite((HashedString) name2);
      if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
        tuple = new Tuple<Sprite, Color>(sprite, Color.white);
      else
        Debug.LogWarning((object) ("Missing codex biome icon: " + name2));
      string str7 = (string) Strings.Get($"STRINGS.SUBWORLDS.{str6.ToUpper()}.DESC");
      string str8 = (string) Strings.Get($"STRINGS.SUBWORLDS.{str6.ToUpper()}.UTILITY");
      ContentContainer contentContainer1 = new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText(string.IsNullOrEmpty(str7) ? "Basic description of the biome." : str7),
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText(string.IsNullOrEmpty(str8) ? "Description of the biomes utility." : str8),
        (ICodexWidget) new CodexSpacer()
      }, ContentContainer.ContentLayout.Vertical);
      contentContainerList.Add(contentContainer1);
      Dictionary<string, float> source = new Dictionary<string, float>();
      ContentContainer contentContainer2 = new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) UI.CODEX.SUBWORLDS.ELEMENTS, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexSpacer()
      }, ContentContainer.ContentLayout.Vertical);
      contentContainerList.Add(contentContainer2);
      ContentContainer contentContainer3 = new ContentContainer();
      contentContainer3.contentLayout = ContentContainer.ContentLayout.Vertical;
      contentContainer3.content = new List<ICodexWidget>();
      contentContainerList.Add(contentContainer3);
      foreach (WeightedSubworldName weightedSubworldName in keyValuePair1.Value)
      {
        SubWorld subworld = SettingsCache.subworlds[weightedSubworldName.name];
        foreach (WeightedBiome biome in SettingsCache.subworlds[weightedSubworldName.name].biomes)
        {
          foreach (ElementGradient elementGradient in (List<ElementGradient>) SettingsCache.biomes.BiomeBackgroundElementBandConfigurations[biome.name])
          {
            if (source.ContainsKey(elementGradient.content))
            {
              source[elementGradient.content] = source[elementGradient.content] + elementGradient.bandSize;
            }
            else
            {
              if (ElementLoader.FindElementByName(elementGradient.content) == null)
                Debug.LogError((object) $"Biome {biome.name} contains non-existent element {elementGradient.content}");
              source.Add(elementGradient.content, elementGradient.bandSize);
            }
          }
        }
        foreach (Feature feature in subworld.features)
        {
          foreach (KeyValuePair<string, ElementChoiceGroup<WeightedSimHash>> elementChoiceGroup in SettingsCache.GetCachedFeature(feature.type).ElementChoiceGroups)
          {
            foreach (WeightedSimHash choice in elementChoiceGroup.Value.choices)
            {
              if (source.ContainsKey(choice.element))
                source[choice.element] = source[choice.element] + 1f;
              else
                source.Add(choice.element, 1f);
            }
          }
        }
      }
      foreach (KeyValuePair<string, float> keyValuePair2 in (IEnumerable<KeyValuePair<string, float>>) source.OrderBy<KeyValuePair<string, float>, float>((Func<KeyValuePair<string, float>, float>) (pair => pair.Value)))
      {
        Element elementByName = ElementLoader.FindElementByName(keyValuePair2.Key);
        if (tuple == null)
          tuple = Def.GetUISprite((object) elementByName.substance);
        contentContainer3.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon(elementByName.name, CodexTextStyle.Body, Def.GetUISprite((object) elementByName.substance)));
      }
      List<Tag> tagList = new List<Tag>();
      ContentContainer contentContainer4 = new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) UI.CODEX.SUBWORLDS.PLANTS, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexSpacer()
      }, ContentContainer.ContentLayout.Vertical);
      contentContainerList.Add(contentContainer4);
      ContentContainer contentContainer5 = new ContentContainer();
      contentContainer5.contentLayout = ContentContainer.ContentLayout.Vertical;
      contentContainer5.content = new List<ICodexWidget>();
      contentContainerList.Add(contentContainer5);
      foreach (WeightedSubworldName weightedSubworldName in keyValuePair1.Value)
      {
        foreach (WeightedBiome biome in SettingsCache.subworlds[weightedSubworldName.name].biomes)
        {
          if (biome.tags != null)
          {
            foreach (string tag in biome.tags)
            {
              if (!tagList.Contains((Tag) tag))
              {
                GameObject prefab = Assets.TryGetPrefab((Tag) tag);
                if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && ((UnityEngine.Object) prefab.GetComponent<Harvestable>() != (UnityEngine.Object) null || (UnityEngine.Object) prefab.GetComponent<SeedProducer>() != (UnityEngine.Object) null))
                {
                  tagList.Add((Tag) tag);
                  contentContainer5.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon(prefab.GetProperName(), CodexTextStyle.Body, Def.GetUISprite((object) prefab)));
                }
              }
            }
          }
        }
        foreach (Feature feature in SettingsCache.subworlds[weightedSubworldName.name].features)
        {
          foreach (MobReference internalMob in SettingsCache.GetCachedFeature(feature.type).internalMobs)
          {
            Tag tag = internalMob.type.ToTag();
            if (!tagList.Contains(tag))
            {
              GameObject prefab = Assets.TryGetPrefab(tag);
              if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && ((UnityEngine.Object) prefab.GetComponent<Harvestable>() != (UnityEngine.Object) null || (UnityEngine.Object) prefab.GetComponent<SeedProducer>() != (UnityEngine.Object) null))
              {
                tagList.Add(tag);
                contentContainer5.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon(prefab.GetProperName(), CodexTextStyle.Body, Def.GetUISprite((object) prefab)));
              }
            }
          }
        }
      }
      if (tagList.Count == 0)
        contentContainer5.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon((string) UI.CODEX.SUBWORLDS.NONE, CodexTextStyle.Body, new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "inspectorUI_cannot_build"), Color.red)));
      ContentContainer contentContainer6 = new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) UI.CODEX.SUBWORLDS.CRITTERS, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexSpacer()
      }, ContentContainer.ContentLayout.Vertical);
      contentContainerList.Add(contentContainer6);
      DictionaryPool<Tag, GameObject, CodexEntry>.PooledDictionary critterTypes = DictionaryPool<Tag, GameObject, CodexEntry>.Allocate();
      CodexEntryGenerator.CollectCritterTypes((IReadOnlyList<WeightedSubworldName>) keyValuePair1.Value, (Dictionary<Tag, GameObject>) critterTypes);
      ContentContainer contentContainer7 = new ContentContainer();
      contentContainer7.contentLayout = ContentContainer.ContentLayout.Vertical;
      contentContainer7.content = new List<ICodexWidget>();
      contentContainerList.Add(contentContainer7);
      if (critterTypes.Count == 0)
      {
        contentContainer7.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon((string) UI.CODEX.SUBWORLDS.NONE, CodexTextStyle.Body, new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "inspectorUI_cannot_build"), Color.red)));
      }
      else
      {
        foreach (KeyValuePair<Tag, GameObject> keyValuePair3 in (Dictionary<Tag, GameObject>) critterTypes)
          contentContainer7.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon(keyValuePair3.Value.GetProperName(), CodexTextStyle.Body, Def.GetUISprite((object) keyValuePair3.Value)));
      }
      critterTypes.Recycle();
      string str9 = "BIOME" + str6;
      CodexEntry entry = new CodexEntry("BIOMES", contentContainerList, str9);
      entry.name = name1;
      entry.parentId = "BIOMES";
      entry.icon = tuple.first;
      entry.iconColor = tuple.second;
      CodexCache.AddEntry(str9, entry);
      biomeEntries.Add(str9, entry);
    }
    if (Application.isPlaying)
    {
      Global.Instance.modManager.HandleErrors((List<YamlIO.Error>) world_gen_errors);
    }
    else
    {
      foreach (YamlIO.Error error in (List<YamlIO.Error>) world_gen_errors)
        YamlIO.LogError(error, false);
    }
    world_gen_errors.Recycle();
    return biomeEntries;
  }

  private static void CollectCritterTypes(
    IReadOnlyList<WeightedSubworldName> subworlds,
    Dictionary<Tag, GameObject> critterTypes)
  {
    for (int index = 0; index != subworlds.Count; ++index)
    {
      WeightedSubworldName subworld = subworlds[index];
      foreach (WeightedBiome biome in SettingsCache.subworlds[subworld.name].biomes)
      {
        if (biome.tags != null)
        {
          foreach (string tag in biome.tags)
            TryAddCritterType((Tag) tag, critterTypes);
        }
      }
      foreach (Feature feature in SettingsCache.subworlds[subworld.name].features)
      {
        foreach (MobReference internalMob in SettingsCache.GetCachedFeature(feature.type).internalMobs)
          TryAddCritterType(internalMob.type.ToTag(), critterTypes);
      }
    }

    static void TryAddCritterType(Tag tag, Dictionary<Tag, GameObject> _critterTypes)
    {
      if (_critterTypes.ContainsKey(tag))
        return;
      GameObject prefab = Assets.TryGetPrefab(tag);
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null || !prefab.HasTag(GameTags.Creature))
        return;
      _critterTypes.Add(tag, prefab);
    }
  }

  public static void CollectCritterTypes(Dictionary<Tag, GameObject> critterTypes)
  {
    foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
    {
      foreach (WorldPlacement worldPlacement in keyValuePair.Value.worldPlacements)
        CodexEntryGenerator.CollectCritterTypes((IReadOnlyList<WeightedSubworldName>) SettingsCache.worlds.GetWorldData(worldPlacement.world).subworldFiles, critterTypes);
    }
  }

  public static Dictionary<string, CodexEntry> GenerateConstructionMaterialEntries()
  {
    Dictionary<string, CodexEntry> constructionMaterialEntries = new Dictionary<string, CodexEntry>();
    Dictionary<Tag, List<BuildingDef>> dictionary = new Dictionary<Tag, List<BuildingDef>>();
    foreach (BuildingDef buildingDef in Assets.BuildingDefs)
    {
      if (!buildingDef.Deprecated && !buildingDef.DebugOnly && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) buildingDef) && (buildingDef.ShowInBuildMenu || buildingDef.BuildingComplete.HasTag(GameTags.RocketModule)))
      {
        foreach (string str in buildingDef.MaterialCategory)
        {
          foreach (string name in str.Split('&', StringSplitOptions.None))
          {
            Tag key = new Tag(name);
            if (!dictionary.ContainsKey(key))
              dictionary.Add(key, new List<BuildingDef>());
            dictionary[key].Add(buildingDef);
          }
        }
      }
    }
    foreach (Tag key in dictionary.Keys)
    {
      if (ElementLoader.GetElement(key) == null)
      {
        string str = key.ToString();
        string name = (string) Strings.Get("STRINGS.MISC.TAGS." + str.ToUpper());
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(name, contentContainerList);
        contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexSpacer(),
          (ICodexWidget) new CodexText((string) Strings.Get($"STRINGS.MISC.TAGS.{str.ToUpper()}_DESC")),
          (ICodexWidget) new CodexSpacer()
        }, ContentContainer.ContentLayout.Vertical));
        List<ICodexWidget> content1 = new List<ICodexWidget>();
        List<Tag> validMaterials = MaterialSelector.GetValidMaterials(key, true);
        foreach (Tag tag in validMaterials)
          content1.Add((ICodexWidget) new CodexIndentedLabelWithIcon(tag.ProperName(), CodexTextStyle.Body, Def.GetUISprite((object) tag)));
        contentContainerList.Add(new ContentContainer(content1, ContentContainer.ContentLayout.GridTwoColumn));
        contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexSpacer(),
          (ICodexWidget) new CodexText((string) CODEX.HEADERS.MATERIALUSEDTOCONSTRUCT, CodexTextStyle.Title),
          (ICodexWidget) new CodexDividerLine()
        }, ContentContainer.ContentLayout.Vertical));
        List<ICodexWidget> content2 = new List<ICodexWidget>();
        foreach (BuildingDef buildingDef in dictionary[key])
          content2.Add((ICodexWidget) new CodexIndentedLabelWithIcon(buildingDef.Name, CodexTextStyle.Body, Def.GetUISprite((object) buildingDef.Tag)));
        contentContainerList.Add(new ContentContainer(content2, ContentContainer.ContentLayout.GridTwoColumn));
        CodexEntry entry = new CodexEntry("BUILDINGMATERIALCLASSES", contentContainerList, name);
        entry.parentId = entry.category;
        entry.icon = Assets.GetSprite((HashedString) ("ui_" + key.Name.ToLower())) ?? (validMaterials.Count != 0 ? Def.GetUISprite((object) validMaterials[0]).first : (Sprite) null) ?? Assets.GetSprite((HashedString) "ui_elements_classes");
        if (key == GameTags.BuildableAny)
          entry.icon = Assets.GetSprite((HashedString) "ui_elements_classes");
        CodexCache.AddEntry(CodexCache.FormatLinkID(str), entry);
        constructionMaterialEntries.Add(str, entry);
      }
    }
    return constructionMaterialEntries;
  }

  public static Dictionary<string, CodexEntry> GenerateDiseaseEntries()
  {
    Dictionary<string, CodexEntry> diseaseEntries = new Dictionary<string, CodexEntry>();
    foreach (Klei.AI.Disease resource in Db.Get().Diseases.resources)
    {
      if (!resource.Disabled)
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
        CodexEntryGenerator.GenerateDiseaseDescriptionContainers(resource, contentContainerList);
        CodexEntry entry = new CodexEntry("DISEASE", contentContainerList, resource.Name);
        entry.parentId = "DISEASE";
        diseaseEntries.Add(resource.Id, entry);
        entry.icon = Assets.GetSprite((HashedString) "overlay_disease");
        CodexCache.AddEntry(resource.Id, entry);
      }
    }
    return diseaseEntries;
  }

  public static CategoryEntry GenerateCategoryEntry(
    string id,
    string name,
    Dictionary<string, CodexEntry> entries,
    Sprite icon = null,
    bool largeFormat = true,
    bool sort = true,
    string overrideHeader = null)
  {
    List<ContentContainer> contentContainerList = new List<ContentContainer>();
    CodexEntryGenerator.GenerateTitleContainers(overrideHeader == null ? name : overrideHeader, contentContainerList);
    List<CodexEntry> entriesInCategory = new List<CodexEntry>();
    foreach (KeyValuePair<string, CodexEntry> entry in entries)
    {
      entriesInCategory.Add(entry.Value);
      if ((UnityEngine.Object) icon == (UnityEngine.Object) null)
        icon = entry.Value.icon;
    }
    CategoryEntry entry1 = new CategoryEntry("Root", contentContainerList, name, entriesInCategory, largeFormat, sort);
    entry1.icon = icon;
    CodexCache.AddEntry(id, (CodexEntry) entry1);
    return entry1;
  }

  public static Dictionary<string, CodexEntry> GenerateTutorialNotificationEntries()
  {
    Dictionary<string, CodexEntry> notificationEntries = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries1 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries2 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries3 = new Dictionary<string, CodexEntry>();
    for (int tm = 0; tm < 24; ++tm)
    {
      TutorialMessage restrictions = (TutorialMessage) Tutorial.Instance.TutorialMessage((Tutorial.TutorialMessages) tm, false);
      if (restrictions != null && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
      {
        if (!string.IsNullOrEmpty(restrictions.videoClipId))
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(restrictions.GetTitle(), contentContainerList);
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexVideo()
            {
              videoName = restrictions.videoClipId,
              overlayName = restrictions.videoOverlayName,
              overlayTexts = new List<string>()
              {
                restrictions.videoTitleText,
                (string) VIDEOS.TUTORIAL_HEADER
              }
            }
          }, ContentContainer.ContentLayout.Vertical));
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexText(restrictions.GetMessageBody(), id: restrictions.GetTitle())
          }, ContentContainer.ContentLayout.Vertical));
          CodexEntry entry = new CodexEntry("VIDEOTUTORIALS", contentContainerList, UI.FormatAsLink(restrictions.GetTitle(), "videos_" + tm.ToString()));
          entry.parentId = "VIDEOTUTORIALS";
          entry.icon = Assets.GetSprite((HashedString) "codexVideo");
          CodexCache.AddEntry("videos_" + tm.ToString(), entry);
          entries1.Add(entry.id, entry);
        }
        else
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(restrictions.GetTitle(), contentContainerList);
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexText(restrictions.GetMessageBody(), id: restrictions.GetTitle())
          }, ContentContainer.ContentLayout.Vertical));
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexSpacer(),
            (ICodexWidget) new CodexSpacer()
          }, ContentContainer.ContentLayout.Vertical));
          CodexEntry entry = new CodexEntry("MISCELLANEOUSTIPS", contentContainerList, restrictions.GetTitle());
          entry.parentId = "MISCELLANEOUSTIPS";
          CodexCache.AddEntry("MISCELLANEOUSTIPS_" + tm.ToString(), entry);
          entries2.Add(entry.id, entry);
        }
      }
    }
    CategoryEntry categoryEntry1 = CodexEntryGenerator.GenerateCategoryEntry("VIDEOTUTORIALS", (string) Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.VIDEOTUTORIALS"), entries1, Assets.GetSprite((HashedString) "codexVideo"), sort: false, overrideHeader: (string) Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.VIDEOTUTORIALS"));
    CategoryEntry categoryEntry2 = CodexEntryGenerator.GenerateCategoryEntry("SYSTEMSTUTORIALS", (string) Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.SYSTEMSTUTORIALS"), entries3, Assets.GetSprite((HashedString) "ui_systems"), sort: false, overrideHeader: (string) Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.SYSTEMSTUTORIALS"));
    CategoryEntry categoryEntry3 = CodexEntryGenerator.GenerateCategoryEntry("MISCELLANEOUSTIPS", (string) Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.MISCELLANEOUSTIPS"), entries2, Assets.GetSprite((HashedString) "ui_tips"), false, false, (string) Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.MISCELLANEOUSTIPS"));
    categoryEntry1.parentId = "LESSONS";
    categoryEntry2.parentId = "LESSONS";
    categoryEntry3.parentId = "LESSONS";
    notificationEntries.Add("VIDEOTUTORIALS", (CodexEntry) categoryEntry1);
    notificationEntries.Add("SYSTEMSTUTORIALS", (CodexEntry) categoryEntry2);
    notificationEntries.Add("MISCELLANEOUSTIPS", (CodexEntry) categoryEntry3);
    foreach (KeyValuePair<string, CodexEntry> keyValuePair in notificationEntries)
      CodexCache.categoriesForPostYAMLPopulation.Add(keyValuePair.Value as CategoryEntry);
    return notificationEntries;
  }

  public static void PopulateCategoryEntries(Dictionary<string, CodexEntry> categoryEntries)
  {
    List<CategoryEntry> categoryEntries1 = new List<CategoryEntry>();
    foreach (KeyValuePair<string, CodexEntry> categoryEntry in categoryEntries)
      categoryEntries1.Add(categoryEntry.Value as CategoryEntry);
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntries1);
  }

  public static void PopulateCategoryEntries(
    List<CategoryEntry> categoryEntries,
    Comparison<CodexEntry> comparison = null)
  {
    foreach (CategoryEntry categoryEntry in categoryEntries)
    {
      List<ContentContainer> contentContainers = categoryEntry.contentContainers;
      List<CodexEntry> codexEntryList = new List<CodexEntry>();
      foreach (CodexEntry codexEntry in categoryEntry.entriesInCategory)
        codexEntryList.Add(codexEntry);
      if (categoryEntry.sort)
      {
        if (comparison == null)
          codexEntryList.Sort((Comparison<CodexEntry>) ((a, b) => UI.StripLinkFormatting(a.name).CompareTo(UI.StripLinkFormatting(b.name))));
        else
          codexEntryList.Sort(comparison);
      }
      if (categoryEntry.largeFormat)
      {
        ContentContainer contentContainer1 = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Grid);
        foreach (CodexEntry codexEntry in codexEntryList)
          contentContainer1.content.Add((ICodexWidget) new CodexLabelWithLargeIcon(codexEntry.name, CodexTextStyle.BodyWhite, new Tuple<Sprite, Color>((UnityEngine.Object) codexEntry.icon != (UnityEngine.Object) null ? codexEntry.icon : Assets.GetSprite((HashedString) "unknown"), codexEntry.iconColor), codexEntry.id));
        if (categoryEntry.showBeforeGeneratedCategoryLinks)
        {
          contentContainers.Add(contentContainer1);
        }
        else
        {
          ContentContainer contentContainer2 = contentContainers[contentContainers.Count - 1];
          contentContainers.RemoveAt(contentContainers.Count - 1);
          contentContainers.Insert(0, contentContainer2);
          contentContainers.Insert(1, contentContainer1);
          contentContainers.Insert(2, new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexSpacer()
          }, ContentContainer.ContentLayout.Vertical));
        }
      }
      else
      {
        ContentContainer contentContainer3 = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Vertical);
        foreach (CodexEntry codexEntry in codexEntryList)
        {
          if ((UnityEngine.Object) codexEntry.icon == (UnityEngine.Object) null)
            contentContainer3.content.Add((ICodexWidget) new CodexText(UI.FormatAsLink(Util.StripTextFormatting(codexEntry.name), codexEntry.id)));
          else
            contentContainer3.content.Add((ICodexWidget) new CodexLabelWithIcon(UI.FormatAsLink(Util.StripTextFormatting(codexEntry.name), codexEntry.id), CodexTextStyle.Body, new Tuple<Sprite, Color>(codexEntry.icon, codexEntry.iconColor), 64 /*0x40*/, 48 /*0x30*/));
        }
        if (categoryEntry.showBeforeGeneratedCategoryLinks)
        {
          contentContainers.Add(contentContainer3);
        }
        else
        {
          ContentContainer contentContainer4 = contentContainers[contentContainers.Count - 1];
          contentContainers.RemoveAt(contentContainers.Count - 1);
          contentContainers.Insert(0, contentContainer4);
          contentContainers.Insert(1, contentContainer3);
        }
      }
    }
  }

  public static void GenerateTitleContainers(string name, List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(name, CodexTextStyle.Title),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GeneratePrerequisiteTechContainers(
    Tech tech,
    List<ContentContainer> containers)
  {
    if (tech.requiredTech == null || tech.requiredTech.Count == 0)
      return;
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.PREREQUISITE_TECH, CodexTextStyle.Subtitle));
    content.Add((ICodexWidget) new CodexDividerLine());
    content.Add((ICodexWidget) new CodexSpacer());
    foreach (Tech tech1 in tech.requiredTech)
      content.Add((ICodexWidget) new CodexText(tech1.Name));
    content.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateSkillRequirementsAndPerksContainers(
    Skill skill,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    string text1 = (string) CODEX.HEADERS.ROLE_PERKS;
    string text2 = (string) CODEX.HEADERS.ROLE_PERKS_DESC;
    if (DlcManager.DlcListContains(skill.GetRequiredDlcIds(), "DLC3_ID"))
    {
      text1 = (string) CODEX.HEADERS.ROLE_PERKS_BIONIC;
      text2 = (string) CODEX.HEADERS.ROLE_PERKS_BIONIC_DESC;
    }
    CodexText codexText1 = new CodexText(text1, CodexTextStyle.Subtitle);
    CodexText codexText2 = new CodexText(text2);
    content.Add((ICodexWidget) codexText1);
    content.Add((ICodexWidget) new CodexDividerLine());
    content.Add((ICodexWidget) codexText2);
    content.Add((ICodexWidget) new CodexSpacer());
    foreach (SkillPerk perk in skill.perks)
    {
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) perk))
      {
        CodexText codexText3 = new CodexText(SkillPerk.GetDescription(perk.Id));
        content.Add((ICodexWidget) codexText3);
      }
    }
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
    content.Add((ICodexWidget) new CodexSpacer());
  }

  private static void GenerateRelatedSkillContainers(Skill skill, List<ContentContainer> containers)
  {
    bool flag1 = false;
    List<ICodexWidget> content1 = new List<ICodexWidget>();
    content1.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.PREREQUISITE_ROLES, CodexTextStyle.Subtitle));
    content1.Add((ICodexWidget) new CodexDividerLine());
    content1.Add((ICodexWidget) new CodexSpacer());
    foreach (string priorSkill in skill.priorSkills)
    {
      CodexText codexText = new CodexText(Db.Get().Skills.Get(priorSkill).Name);
      content1.Add((ICodexWidget) codexText);
      flag1 = true;
    }
    if (flag1)
    {
      content1.Add((ICodexWidget) new CodexSpacer());
      containers.Add(new ContentContainer(content1, ContentContainer.ContentLayout.Vertical));
    }
    bool flag2 = false;
    List<ICodexWidget> content2 = new List<ICodexWidget>();
    string text1 = (string) CODEX.HEADERS.UNLOCK_ROLES;
    string text2 = (string) CODEX.HEADERS.UNLOCK_ROLES_DESC;
    if (DlcManager.DlcListContains(skill.GetRequiredDlcIds(), "DLC3_ID"))
    {
      text1 = (string) CODEX.HEADERS.UNLOCK_ROLES_BIONIC;
      text2 = (string) CODEX.HEADERS.UNLOCK_ROLES_BIONIC_DESC;
    }
    CodexText codexText1 = new CodexText(text1, CodexTextStyle.Subtitle);
    CodexText codexText2 = new CodexText(text2);
    content2.Add((ICodexWidget) codexText1);
    content2.Add((ICodexWidget) new CodexDividerLine());
    content2.Add((ICodexWidget) codexText2);
    content2.Add((ICodexWidget) new CodexSpacer());
    foreach (Skill resource in Db.Get().Skills.resources)
    {
      if (!resource.deprecated)
      {
        foreach (string priorSkill in resource.priorSkills)
        {
          if (priorSkill == skill.Id)
          {
            CodexText codexText3 = new CodexText(resource.Name);
            content2.Add((ICodexWidget) codexText3);
            flag2 = true;
          }
        }
      }
    }
    if (!flag2)
      return;
    content2.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content2, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateUnlockContainers(Tech tech, List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.TECH_UNLOCKS, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine(),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (TechItem unlockedItem in tech.unlockedItems)
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexImage(64 /*0x40*/, 64 /*0x40*/, unlockedItem.getUISprite("ui", false)),
        (ICodexWidget) new CodexText(unlockedItem.Name)
      }, ContentContainer.ContentLayout.Horizontal));
  }

  private static void GenerateRecipeContainers(Tag prefabID, List<ContentContainer> containers)
  {
    Recipe recipe1 = (Recipe) null;
    foreach (Recipe recipe2 in RecipeManager.Get().recipes)
    {
      if (recipe2.Result == prefabID)
      {
        recipe1 = recipe2;
        break;
      }
    }
    if (recipe1 == null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.RECIPE, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    Func<Recipe, List<ContentContainer>> func = (Func<Recipe, List<ContentContainer>>) (rec =>
    {
      List<ContentContainer> recipeContainers = new List<ContentContainer>();
      foreach (Recipe.Ingredient ingredient in rec.Ingredients)
      {
        GameObject prefab = Assets.GetPrefab(ingredient.tag);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
          recipeContainers.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexImage(64 /*0x40*/, 64 /*0x40*/, Def.GetUISprite((object) prefab)),
            (ICodexWidget) new CodexText(GameUtil.SafeStringFormat((string) UI.CODEX.RECIPE_ITEM, (object) Assets.GetPrefab(ingredient.tag).GetProperName(), (object) ingredient.amount, ElementLoader.GetElement(ingredient.tag) == null ? (object) "" : (object) UI.UNITSUFFIXES.MASS.KILOGRAM.text))
          }, ContentContainer.ContentLayout.Horizontal));
      }
      return recipeContainers;
    });
    containers.AddRange((IEnumerable<ContentContainer>) func(recipe1));
    GameObject prefab1 = recipe1.fabricators == null ? (GameObject) null : Assets.GetPrefab((Tag) recipe1.fabricators[0]);
    if (!((UnityEngine.Object) prefab1 != (UnityEngine.Object) null))
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) UI.CODEX.RECIPE_FABRICATOR_HEADER, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexImage(64 /*0x40*/, 64 /*0x40*/, Def.GetUISpriteFromMultiObjectAnim(prefab1.GetComponent<KBatchedAnimController>().AnimFiles[0])),
      (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.RECIPE_FABRICATOR, (object) recipe1.FabricationTime, (object) prefab1.GetProperName()))
    }, ContentContainer.ContentLayout.Horizontal));
  }

  private static void GenerateRoomTypeDetailsContainers(
    RoomType roomType,
    List<ContentContainer> containers)
  {
    ContentContainer contentContainer1 = new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) UI.CODEX.DETAILS, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical);
    containers.Add(contentContainer1);
    List<ICodexWidget> content = new List<ICodexWidget>();
    if (!string.IsNullOrEmpty(roomType.effect))
    {
      string roomEffectsString = roomType.GetRoomEffectsString();
      content.Add((ICodexWidget) new CodexText(roomEffectsString));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    if ((roomType.primary_constraint != null ? 1 : (roomType.additional_constraints != null ? 1 : 0)) != 0)
    {
      content.Add((ICodexWidget) new CodexText((string) ROOMS.CRITERIA.HEADER));
      string text = "";
      if (roomType.primary_constraint != null)
        text = $"{text}    • {roomType.primary_constraint.name}";
      if (roomType.additional_constraints != null)
      {
        for (int index = 0; index < roomType.additional_constraints.Length; ++index)
          text = $"{text}\n    • {roomType.additional_constraints[index].name}";
      }
      content.Add((ICodexWidget) new CodexText(text));
    }
    ContentContainer contentContainer2 = new ContentContainer(content, ContentContainer.ContentLayout.Vertical);
    containers.Add(contentContainer2);
  }

  private static void GenerateRoomTypeDescriptionContainers(
    RoomType roomType,
    List<ContentContainer> containers)
  {
    ContentContainer contentContainer = new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(roomType.description),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical);
    containers.Add(contentContainer);
  }

  private static void GeneratePlantDescriptionContainers(
    GameObject plant,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexSpacer());
    content.Add((ICodexWidget) new CodexText((string) UI.CODEX.DETAILS, CodexTextStyle.Subtitle));
    content.Add((ICodexWidget) new CodexDividerLine());
    InfoDescription component = Assets.GetPrefab(plant.PrefabID()).GetComponent<InfoDescription>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      content.Add((ICodexWidget) new CodexText(component.description));
    string str1 = "";
    List<Descriptor> requirementDescriptors = GameUtil.GetPlantRequirementDescriptors(plant);
    if (requirementDescriptors.Count > 0)
    {
      string text = str1 + requirementDescriptors[0].text;
      for (int index = 1; index < requirementDescriptors.Count; ++index)
        text = $"{text}\n    • {requirementDescriptors[index].text}";
      content.Add((ICodexWidget) new CodexText(text));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    string str2 = "";
    List<Descriptor> effectDescriptors = GameUtil.GetPlantEffectDescriptors(plant);
    if (effectDescriptors.Count > 0)
    {
      string text = str2 + effectDescriptors[0].text;
      for (int index = 1; index < effectDescriptors.Count; ++index)
        text = $"{text}\n    • {effectDescriptors[index].text}";
      CodexText codexText = new CodexText(text);
      content.Add((ICodexWidget) codexText);
      content.Add((ICodexWidget) new CodexSpacer());
    }
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static ICodexWidget GetIconWidget(object entity)
  {
    return (ICodexWidget) new CodexImage(32 /*0x20*/, 32 /*0x20*/, Def.GetUISprite(entity));
  }

  public static void GenerateManualConversionEntries(Tag tag, List<ContentContainer> containers)
  {
    CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(tag, containers);
  }

  private static void GenerateDiseaseDescriptionContainers(
    Klei.AI.Disease disease,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexSpacer());
    StringEntry result = (StringEntry) null;
    if (Strings.TryGet($"STRINGS.DUPLICANTS.DISEASES.{disease.Id.ToUpper()}.DESC", out result))
    {
      content.Add((ICodexWidget) new CodexText(result.String));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    foreach (Descriptor quantitativeDescriptor in disease.GetQuantitativeDescriptors())
      content.Add((ICodexWidget) new CodexText(quantitativeDescriptor.text));
    content.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateFoodDescriptionContainers(
    EdiblesManager.FoodInfo food,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> codexWidgetList1 = new List<ICodexWidget>();
    codexWidgetList1.Add((ICodexWidget) new CodexText(food.Description));
    codexWidgetList1.Add((ICodexWidget) new CodexSpacer());
    codexWidgetList1.Add((ICodexWidget) new CodexText(GameUtil.SafeStringFormat((string) UI.CODEX.FOOD.QUALITY, (object) GameUtil.GetFormattedFoodQuality(food.Quality))));
    codexWidgetList1.Add((ICodexWidget) new CodexText(GameUtil.SafeStringFormat((string) UI.CODEX.FOOD.CALORIES, (object) GameUtil.GetFormattedCalories(food.CaloriesPerUnit))));
    codexWidgetList1.Add((ICodexWidget) new CodexSpacer());
    List<ICodexWidget> codexWidgetList2 = codexWidgetList1;
    string text1;
    if (!food.CanRot)
      text1 = UI.CODEX.FOOD.NON_PERISHABLE.ToString();
    else
      text1 = GameUtil.SafeStringFormat((string) UI.CODEX.FOOD.SPOILPROPERTIES, (object) GameUtil.GetFormattedTemperature(food.RotTemperature), (object) GameUtil.GetFormattedTemperature(food.PreserveTemperature), (object) GameUtil.GetFormattedCycles(food.SpoilTime));
    CodexText codexText = new CodexText(text1);
    codexWidgetList2.Add((ICodexWidget) codexText);
    codexWidgetList1.Add((ICodexWidget) new CodexSpacer());
    List<ICodexWidget> content = codexWidgetList1;
    if (food.Effects.Count > 0)
    {
      content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.FOODEFFECTS + ":"));
      foreach (string effect1 in food.Effects)
      {
        Effect effect2 = Db.Get().effects.Get(effect1);
        string text2 = (string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{effect1.ToUpper()}.NAME");
        string str1 = (string) Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{effect1.ToUpper()}.DESCRIPTION");
        string str2 = "";
        foreach (AttributeModifier selfModifier in effect2.SelfModifiers)
          str2 = $"{str2}\n    • {(string) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{selfModifier.AttributeId.ToUpper()}.NAME")}: {selfModifier.GetFormattedString()}";
        string tooltip = str1 + str2;
        string str3 = UI.FormatAsLink(text2, $"{CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID}::{effect1.ToUpper()}");
        content.Add((ICodexWidget) new CodexTextWithTooltip("    • " + str3, tooltip));
      }
      content.Add((ICodexWidget) new CodexSpacer());
    }
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateTechDescriptionContainers(
    Tech tech,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get($"STRINGS.RESEARCH.TECHS.{tech.Id.ToUpper()}.DESC")),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateGenericDescriptionContainers(
    string description,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(description),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void AddDescriptorWidgets(
    List<ICodexWidget> widgets,
    string title,
    List<Descriptor> descriptors)
  {
    if (descriptors == null || descriptors.Count == 0)
      return;
    widgets.Add((ICodexWidget) new CodexText(title, CodexTextStyle.Subtitle));
    foreach (Descriptor descriptor in descriptors)
      widgets.Add((ICodexWidget) new CodexTextWithTooltip("    " + descriptor.text, descriptor.tooltipText));
    widgets.Add((ICodexWidget) new CodexSpacer());
  }

  private static void AddRequirementDescriptors(
    List<ICodexWidget> widgets,
    List<Descriptor> nonConverterReqs,
    List<(ElementConverter converter, List<Descriptor> descriptors)> converterDescCache,
    bool hasConverterReqs)
  {
    if ((nonConverterReqs.Count > 0 ? 1 : (converterDescCache.Count > 0 ? 1 : 0)) == 0)
      return;
    widgets.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.BUILDINGREQUIREMENTS, CodexTextStyle.Subtitle));
    foreach (Descriptor nonConverterReq in nonConverterReqs)
      widgets.Add((ICodexWidget) new CodexTextWithTooltip("    " + nonConverterReq.text, nonConverterReq.tooltipText));
    if (hasConverterReqs)
      widgets.Add((ICodexWidget) new CodexText("    " + (string) UI.BUILDINGEFFECTS.OPERATIONINPUTS));
    foreach ((ElementConverter converter, List<Descriptor> descriptors) tuple in converterDescCache)
    {
      foreach (Descriptor descriptor in tuple.descriptors)
      {
        if (descriptor.type == Descriptor.DescriptorType.Requirement)
          widgets.Add((ICodexWidget) new CodexTextWithTooltip("        • " + descriptor.text, descriptor.tooltipText));
      }
    }
    widgets.Add((ICodexWidget) new CodexSpacer());
  }

  private static void AddConstructionPropertyDescriptors(
    List<ICodexWidget> widgets,
    BuildingDef def)
  {
    if (def.MaterialCategory.Length != def.Mass.Length)
      Debug.LogWarningFormat("{0} Required Materials({1}) and Masses({2}) mismatch!", (object) def.name, (object) string.Join(", ", def.MaterialCategory), (object) string.Join<float>(", ", (IEnumerable<float>) def.Mass));
    if (def.MaterialCategory.Length + def.Mass.Length == 0)
      return;
    widgets.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.BUILDINGCONSTRUCTIONPROPS, CodexTextStyle.Subtitle));
    widgets.Add((ICodexWidget) new CodexText("    " + string.Format((string) CODEX.FORMAT_STRINGS.BUILDING_SIZE, (object) def.WidthInCells, (object) def.HeightInCells)));
    widgets.Add((ICodexWidget) new CodexText("    " + string.Format((string) CODEX.FORMAT_STRINGS.CONSTRUCTION_TIME, (object) def.ConstructionTime)));
    List<string> values = new List<string>();
    for (int index = 0; index < Math.Min(def.MaterialCategory.Length, def.Mass.Length); ++index)
      values.Add(string.Format((string) CODEX.FORMAT_STRINGS.MATERIAL_MASS, (object) TUNING.MATERIALS.GetMaterialString(def.MaterialCategory[index]), (object) GameUtil.GetFormattedMass(def.Mass[index])));
    widgets.Add((ICodexWidget) new CodexText($"    {(string) CODEX.HEADERS.BUILDINGCONSTRUCTIONMATERIALS}{string.Join(", ", (IEnumerable<string>) values)}"));
    widgets.Add((ICodexWidget) new CodexSpacer());
  }

  private static void AddEffectDescriptors(
    List<ICodexWidget> widgets,
    List<Descriptor> nonConverterEffects,
    List<(ElementConverter converter, List<Descriptor> descriptors)> converterDescCache)
  {
    if ((nonConverterEffects.Count > 0 ? 1 : (converterDescCache.Count > 0 ? 1 : 0)) == 0)
      return;
    widgets.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.BUILDINGEFFECTS, CodexTextStyle.Subtitle));
    foreach (Descriptor nonConverterEffect in nonConverterEffects)
      widgets.Add((ICodexWidget) new CodexTextWithTooltip("    " + nonConverterEffect.text, nonConverterEffect.tooltipText));
    foreach ((ElementConverter converter, List<Descriptor> descriptors) in converterDescCache)
    {
      string str = converter.consumedElements[0].Name;
      for (int index = 1; index < converter.consumedElements.Length; ++index)
        str = $"{str}, {converter.consumedElements[index].Name}";
      widgets.Add((ICodexWidget) new CodexText($"    {str}:"));
      foreach (Descriptor descriptor in descriptors)
      {
        if (descriptor.type != Descriptor.DescriptorType.Requirement)
          widgets.Add((ICodexWidget) new CodexTextWithTooltip("        " + descriptor.text, descriptor.tooltipText));
      }
    }
    widgets.Add((ICodexWidget) new CodexSpacer());
  }

  private static void GenerateBuildingDescriptionContainers(
    BuildingDef def,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> codexWidgetList = new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{def.PrefabID.ToUpper()}.EFFECT")),
      (ICodexWidget) new CodexSpacer()
    };
    ListPool<(ElementConverter, List<Descriptor>), BuildingDef>.PooledList converterDescCache = ListPool<(ElementConverter, List<Descriptor>), BuildingDef>.Allocate();
    ListPool<Descriptor, BuildingDef>.PooledList nonConverterReqs = ListPool<Descriptor, BuildingDef>.Allocate();
    ListPool<Descriptor, BuildingDef>.PooledList nonConverterEffects = ListPool<Descriptor, BuildingDef>.Allocate();
    bool hasConverterReqs;
    GameUtil.PartitionBuildingDescriptors(def.BuildingComplete, false, out List<Descriptor> _, (List<(ElementConverter, List<Descriptor>)>) converterDescCache, (List<Descriptor>) nonConverterReqs, (List<Descriptor>) nonConverterEffects, out hasConverterReqs);
    CodexEntryGenerator.AddRequirementDescriptors(codexWidgetList, (List<Descriptor>) nonConverterReqs, (List<(ElementConverter, List<Descriptor>)>) converterDescCache, hasConverterReqs);
    nonConverterReqs.Recycle();
    CodexEntryGenerator.AddConstructionPropertyDescriptors(codexWidgetList, def);
    CodexEntryGenerator.AddEffectDescriptors(codexWidgetList, (List<Descriptor>) nonConverterEffects, (List<(ElementConverter, List<Descriptor>)>) converterDescCache);
    nonConverterEffects.Recycle();
    converterDescCache.Recycle();
    string[] roomClassForObject = CodexEntryGenerator.GetRoomClassForObject(def.BuildingComplete);
    string[] categoriesForObject = CodexEntryGenerator.GetCategoriesForObject(def.BuildingComplete);
    bool flag = roomClassForObject != null || categoriesForObject != null;
    if (flag)
      codexWidgetList.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.BUILDINGTYPE, CodexTextStyle.Subtitle));
    if (roomClassForObject != null)
    {
      foreach (string str in roomClassForObject)
        codexWidgetList.Add((ICodexWidget) new CodexText("    " + str));
    }
    if (categoriesForObject != null)
    {
      foreach (string str in categoriesForObject)
        codexWidgetList.Add((ICodexWidget) new CodexText("    " + str));
    }
    if (flag)
      codexWidgetList.Add((ICodexWidget) new CodexSpacer());
    codexWidgetList.Add((ICodexWidget) new CodexText($"<i>{(string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{def.PrefabID.ToUpper()}.DESC")}</i>"));
    containers.Add(new ContentContainer(codexWidgetList, ContentContainer.ContentLayout.Vertical));
  }

  public static string[] GetCategoriesForObject(GameObject obj)
  {
    List<string> stringList = new List<string>();
    KPrefabID component = obj.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      foreach (Tag tag in component.Tags)
      {
        if (GameTags.CodexCategories.AllTags.Contains(tag))
          stringList.Add(GameTags.CodexCategories.GetCategoryLabelText(tag));
      }
    }
    return stringList.Count <= 0 ? (string[]) null : stringList.ToArray();
  }

  public static string[] GetRoomClassForObject(GameObject obj)
  {
    List<string> stringList = new List<string>();
    KPrefabID component = obj.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      foreach (Tag tag in component.Tags)
      {
        if (RoomConstraints.ConstraintTags.AllTags.Contains(tag))
          stringList.Add(RoomConstraints.ConstraintTags.GetRoomConstraintLabelText(tag));
      }
    }
    return stringList.Count <= 0 ? (string[]) null : stringList.ToArray();
  }

  public static void GenerateImageContainers(
    Sprite[] sprites,
    List<ContentContainer> containers,
    ContentContainer.ContentLayout layout)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (Sprite sprite in sprites)
    {
      if (!((UnityEngine.Object) sprite == (UnityEngine.Object) null))
      {
        CodexImage codexImage = new CodexImage(128 /*0x80*/, 128 /*0x80*/, sprite);
        content.Add((ICodexWidget) codexImage);
      }
    }
    containers.Add(new ContentContainer(content, layout));
  }

  public static void GenerateImageContainers(
    Tuple<Sprite, Color>[] sprites,
    List<ContentContainer> containers,
    ContentContainer.ContentLayout layout)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (Tuple<Sprite, Color> sprite in sprites)
    {
      if (sprite != null)
      {
        CodexImage codexImage = new CodexImage(128 /*0x80*/, 128 /*0x80*/, sprite);
        content.Add((ICodexWidget) codexImage);
      }
    }
    containers.Add(new ContentContainer(content, layout));
  }

  public static void GenerateImageContainers(Sprite sprite, List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexImage(128 /*0x80*/, 128 /*0x80*/, sprite)
    }, ContentContainer.ContentLayout.Vertical));
  }

  public static void CreateUnlockablesContentContainer(SubEntry subentry)
  {
    subentry.lockedContentContainer = new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.SECTION_UNLOCKABLES, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical)
    {
      showBeforeGeneratedContent = false
    };
  }

  private static void GenerateFabricatorContainers(
    GameObject entity,
    List<ContentContainer> containers)
  {
    ComplexFabricator component = entity.GetComponent<ComplexFabricator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CODEX.HEADERS.FABRICATIONS"), CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (ComplexRecipe recipe in component.GetRecipes())
      content.Add((ICodexWidget) new CodexRecipePanel(recipe));
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateConfigurableConsumerContainers(
    GameObject buildingComplete,
    List<ContentContainer> containers)
  {
    IConfigurableConsumer component = buildingComplete.GetComponent<IConfigurableConsumer>();
    if (component == null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CODEX.HEADERS.FABRICATIONS"), CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (IConfigurableConsumerOption settingOption in component.GetSettingOptions())
      content.Add((ICodexWidget) new CodexConfigurableConsumerRecipePanel(settingOption));
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateReceptacleContainers(
    GameObject entity,
    List<ContentContainer> containers)
  {
    SingleEntityReceptacle plot = entity.GetComponent<SingleEntityReceptacle>();
    if ((UnityEngine.Object) plot == (UnityEngine.Object) null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CODEX.HEADERS.RECEPTACLE"), CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (Tag depositObjectTag in (IEnumerable<Tag>) plot.possibleDepositObjectTags)
    {
      List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(depositObjectTag);
      if ((UnityEngine.Object) plot.rotatable == (UnityEngine.Object) null)
        prefabsWithTag.RemoveAll((Predicate<GameObject>) (go =>
        {
          IReceptacleDirection component = go.GetComponent<IReceptacleDirection>();
          return component != null && component.Direction != plot.Direction;
        }));
      foreach (GameObject go in prefabsWithTag)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexImage(64 /*0x40*/, 64 /*0x40*/, Def.GetUISprite((object) go).first),
          (ICodexWidget) new CodexText(go.GetProperName())
        }, ContentContainer.ContentLayout.Horizontal));
    }
  }
}
