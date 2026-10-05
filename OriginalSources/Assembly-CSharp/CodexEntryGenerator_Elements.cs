// Decompiled with JetBrains decompiler
// Type: CodexEntryGenerator_Elements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CodexEntryGenerator_Elements
{
  public static string ELEMENTS_ID = CodexCache.FormatLinkID("ELEMENTS");
  public static string ELEMENTS_SOLIDS_ID = CodexCache.FormatLinkID("ELEMENTS_SOLID");
  public static string ELEMENTS_LIQUIDS_ID = CodexCache.FormatLinkID("ELEMENTS_LIQUID");
  public static string ELEMENTS_GASES_ID = CodexCache.FormatLinkID("ELEMENTS_GAS");
  public static string ELEMENTS_OTHER_ID = CodexCache.FormatLinkID("ELEMENTS_OTHER");
  public static string ELEMENT_TYPES = CodexCache.FormatLinkID("ELEMENTTYPES");
  private static CodexEntryGenerator_Elements.ElementEntryContext contextInstance;

  private static Tag WaterTag => ElementLoader.FindElementByHash(SimHashes.Water).tag;

  private static Tag DirtyWaterTag => ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;

  public static Dictionary<string, CodexEntry> GenerateEntries()
  {
    Dictionary<string, CodexEntry> entriesElements = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries1 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries2 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries3 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries4 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries5 = new Dictionary<string, CodexEntry>();
    AddCategoryEntry(CodexEntryGenerator_Elements.ELEMENTS_SOLIDS_ID, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSSOLID, Assets.GetSprite((HashedString) "ui_elements-solid"), entries1);
    AddCategoryEntry(CodexEntryGenerator_Elements.ELEMENTS_LIQUIDS_ID, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID, Assets.GetSprite((HashedString) "ui_elements-liquids"), entries2);
    AddCategoryEntry(CodexEntryGenerator_Elements.ELEMENTS_GASES_ID, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSGAS, Assets.GetSprite((HashedString) "ui_elements-gases"), entries3);
    AddCategoryEntry(CodexEntryGenerator_Elements.ELEMENTS_OTHER_ID, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSOTHER, Assets.GetSprite((HashedString) "ui_elements-other"), entries4);
    AddCategoryEntry(CodexEntryGenerator_Elements.ELEMENT_TYPES, (string) UI.CODEX.CATEGORYNAMES.ELEMENTTYPES, Assets.GetSprite((HashedString) "ui_element_poperties"), entries5);
    foreach (Element element in ElementLoader.elements)
    {
      if (!element.disabled)
      {
        bool flag = false;
        foreach (Tag oreTag in element.oreTags)
        {
          if (oreTag == GameTags.HideFromCodex)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          Tuple<Sprite, Color> tuple = Def.GetUISprite((object) element);
          if ((UnityEngine.Object) tuple.first == (UnityEngine.Object) null)
          {
            if (element.id == SimHashes.Void)
              tuple = new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_elements-void"), Color.white);
            else if (element.id == SimHashes.Vacuum)
              tuple = new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_elements-vacuum"), Color.white);
          }
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(element.name, contentContainerList);
          CodexEntryGenerator.GenerateImageContainers(new Tuple<Sprite, Color>[1]
          {
            tuple
          }, contentContainerList, ContentContainer.ContentLayout.Horizontal);
          CodexEntryGenerator_Elements.GenerateElementDescriptionContainers(element, contentContainerList);
          string category;
          Dictionary<string, CodexEntry> dictionary;
          if (element.IsSolid)
          {
            category = CodexEntryGenerator_Elements.ELEMENTS_SOLIDS_ID;
            dictionary = entries1;
          }
          else if (element.IsLiquid)
          {
            category = CodexEntryGenerator_Elements.ELEMENTS_LIQUIDS_ID;
            dictionary = entries2;
          }
          else if (element.IsGas)
          {
            category = CodexEntryGenerator_Elements.ELEMENTS_GASES_ID;
            dictionary = entries3;
          }
          else
          {
            category = CodexEntryGenerator_Elements.ELEMENTS_OTHER_ID;
            dictionary = entries4;
          }
          string str = element.id.ToString();
          CodexEntry entry = new CodexEntry(category, contentContainerList, element.name);
          entry.parentId = category;
          entry.icon = tuple.first;
          entry.iconColor = tuple.second;
          CodexCache.AddEntry(str, entry);
          dictionary.Add(str, entry);
        }
      }
    }
    string str1 = "IceBellyPoop";
    GameObject prefab = Assets.TryGetPrefab((Tag) str1);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      string elementsSolidsId = CodexEntryGenerator_Elements.ELEMENTS_SOLIDS_ID;
      Dictionary<string, CodexEntry> dictionary = entries1;
      KPrefabID component1 = prefab.GetComponent<KPrefabID>();
      InfoDescription component2 = prefab.GetComponent<InfoDescription>();
      string properName = prefab.GetProperName();
      string description = component2.description;
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) prefab);
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      CodexEntryGenerator.GenerateTitleContainers(properName, contentContainerList);
      CodexEntryGenerator.GenerateImageContainers(new Tuple<Sprite, Color>[1]
      {
        uiSprite
      }, contentContainerList, ContentContainer.ContentLayout.Horizontal);
      CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(component1.PrefabTag, contentContainerList);
      contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText(description),
        (ICodexWidget) new CodexSpacer()
      }, ContentContainer.ContentLayout.Vertical));
      CodexEntry entry = new CodexEntry(elementsSolidsId, contentContainerList, properName);
      entry.parentId = elementsSolidsId;
      entry.icon = uiSprite.first;
      entry.iconColor = uiSprite.second;
      CodexCache.AddEntry(str1, entry);
      dictionary.Add(str1, entry);
    }
    CodexEntryGenerator.PopulateCategoryEntries(entriesElements);
    return entriesElements;

    void AddCategoryEntry(
      string categoryId,
      string name,
      Sprite icon,
      Dictionary<string, CodexEntry> entries)
    {
      CodexEntry categoryEntry = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(categoryId, name, entries, icon);
      categoryEntry.parentId = CodexEntryGenerator_Elements.ELEMENTS_ID;
      categoryEntry.category = CodexEntryGenerator_Elements.ELEMENTS_ID;
      entriesElements.Add(categoryId, categoryEntry);
    }
  }

  public static void GenerateElementDescriptionContainers(
    Element element,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content1 = new List<ICodexWidget>();
    List<ICodexWidget> content2 = new List<ICodexWidget>();
    if (element.sublimateId != (SimHashes) 0 || element.HasTag(GameTags.Sublimating))
      content1.Add((ICodexWidget) new CodexTemperatureTransitionPanel(element, (double) element.offGasPercentage != 0.0 ? CodexTemperatureTransitionPanel.TransitionType.OFFGASS : CodexTemperatureTransitionPanel.TransitionType.SUBLIMATE));
    if (element.highTempTransition != null)
      content1.Add((ICodexWidget) new CodexTemperatureTransitionPanel(element, CodexTemperatureTransitionPanel.TransitionType.HEAT));
    if (element.lowTempTransition != null)
      content1.Add((ICodexWidget) new CodexTemperatureTransitionPanel(element, CodexTemperatureTransitionPanel.TransitionType.COOL));
    foreach (Element element1 in ElementLoader.elements)
    {
      if (!element1.disabled)
      {
        if (element1.highTempTransition == element || ElementLoader.FindElementByHash(element1.highTempTransitionOreID) == element)
          content2.Add((ICodexWidget) new CodexTemperatureTransitionPanel(element1, CodexTemperatureTransitionPanel.TransitionType.HEAT));
        if (element1.lowTempTransition == element || ElementLoader.FindElementByHash(element1.lowTempTransitionOreID) == element)
          content2.Add((ICodexWidget) new CodexTemperatureTransitionPanel(element1, CodexTemperatureTransitionPanel.TransitionType.COOL));
        if (element1.sublimateId == element.id || element1.HasTag(GameTags.Sublimating))
        {
          bool flag = element1.sublimateId == element.id;
          if (element1.sublimateId != element.id)
          {
            GameObject prefab = Assets.GetPrefab(element1.id.CreateTag());
            if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
            {
              Sublimates component = prefab.GetComponent<Sublimates>();
              flag = (UnityEngine.Object) component != (UnityEngine.Object) null && component.info.sublimatedElement == element.id;
            }
          }
          if (flag)
            content2.Add((ICodexWidget) new CodexTemperatureTransitionPanel(element1, (double) element1.offGasPercentage != 0.0 ? CodexTemperatureTransitionPanel.TransitionType.OFFGASS : CodexTemperatureTransitionPanel.TransitionType.SUBLIMATE));
        }
      }
    }
    if (content1.Count > 0)
    {
      ContentContainer contents = new ContentContainer(content1, ContentContainer.ContentLayout.Vertical);
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexCollapsibleHeader((string) CODEX.HEADERS.ELEMENTTRANSITIONSTO, contents)
      }, ContentContainer.ContentLayout.Vertical));
      containers.Add(contents);
    }
    if (content2.Count > 0)
    {
      ContentContainer contents = new ContentContainer(content2, ContentContainer.ContentLayout.Vertical);
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexCollapsibleHeader((string) CODEX.HEADERS.ELEMENTTRANSITIONSFROM, contents)
      }, ContentContainer.ContentLayout.Vertical));
      containers.Add(contents);
    }
    CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(element.tag, containers);
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText(element.FullDescription()),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  public static void GenerateMadeAndUsedContainers(Tag tag, List<ContentContainer> containers)
  {
    List<ICodexWidget> used = new List<ICodexWidget>();
    List<ICodexWidget> made = new List<ICodexWidget>();
    foreach (ComplexRecipe recipe in ComplexRecipeManager.Get().recipes)
    {
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) recipe) && !recipe.IsAnyProductDeprecated())
      {
        if (((IEnumerable<ComplexRecipe.RecipeElement>) recipe.ingredients).Any<ComplexRecipe.RecipeElement>((Func<ComplexRecipe.RecipeElement, bool>) (i => i.material == tag)))
          used.Add((ICodexWidget) new CodexRecipePanel(recipe));
        if (((IEnumerable<ComplexRecipe.RecipeElement>) recipe.results).Any<ComplexRecipe.RecipeElement>((Func<ComplexRecipe.RecipeElement, bool>) (i => i.material == tag)))
          made.Add((ICodexWidget) new CodexRecipePanel(recipe, true));
      }
    }
    List<CodexEntryGenerator_Elements.ConversionEntry> conversionEntryList1;
    if (CodexEntryGenerator_Elements.GetElementEntryContext().usedMap.map.TryGetValue(tag, out conversionEntryList1))
    {
      foreach (CodexEntryGenerator_Elements.ConversionEntry conversionEntry in conversionEntryList1)
        used.Add((ICodexWidget) new CodexConversionPanel(conversionEntry.title, conversionEntry.inSet.ToArray<ElementUsage>(), conversionEntry.outSet.ToArray<ElementUsage>(), conversionEntry.prefab, conversionEntry.aidIcon1));
    }
    List<CodexEntryGenerator_Elements.ConversionEntry> conversionEntryList2;
    if (CodexEntryGenerator_Elements.GetElementEntryContext().madeMap.map.TryGetValue(tag, out conversionEntryList2))
    {
      foreach (CodexEntryGenerator_Elements.ConversionEntry conversionEntry in conversionEntryList2)
        made.Add((ICodexWidget) new CodexConversionPanel(conversionEntry.title, conversionEntry.inSet.ToArray<ElementUsage>(), conversionEntry.outSet.ToArray<ElementUsage>(), conversionEntry.prefab, conversionEntry.aidIcon1));
    }
    ManualCodexConversionRegistry.GetConversionsForGivenConverter(tag)?.ForEach((Action<ManualCodexConversionRegistry.ManualConversionEntry>) (ce =>
    {
      List<ICodexWidget> codexWidgetList = used;
      string headerDescription = ce.headerDescription;
      ElementUsage[] ins;
      if (ce.input == null)
        ins = (ElementUsage[]) null;
      else
        ins = new ElementUsage[1]
        {
          new ElementUsage(ce.input.first, ce.input.second, false, ce.inputCustomFormating)
        };
      ElementUsage[] outs;
      if (ce.output == null)
        outs = (ElementUsage[]) null;
      else
        outs = new ElementUsage[1]
        {
          new ElementUsage(ce.output.first, ce.output.second, false, ce.outputCustomFormating)
        };
      GameObject prefab = Assets.GetPrefab(tag);
      CodexConversionPanel codexConversionPanel = new CodexConversionPanel(headerDescription, ins, outs, prefab);
      codexWidgetList.Add((ICodexWidget) codexConversionPanel);
    }));
    ManualCodexConversionRegistry.GetProducersForGivenOutput(tag)?.ForEach((Action<ManualCodexConversionRegistry.ManualConversionEntry>) (ce =>
    {
      List<ICodexWidget> codexWidgetList = made;
      string headerDescription = ce.headerDescription;
      ElementUsage[] ins;
      if (ce.input == null)
        ins = (ElementUsage[]) null;
      else
        ins = new ElementUsage[1]
        {
          new ElementUsage(ce.input.first, ce.input.second, false, ce.inputCustomFormating)
        };
      ElementUsage[] outs;
      if (ce.output == null)
        outs = (ElementUsage[]) null;
      else
        outs = new ElementUsage[1]
        {
          new ElementUsage(ce.output.first, ce.output.second, false, ce.outputCustomFormating)
        };
      GameObject prefab = Assets.GetPrefab(ce.converter.first);
      CodexConversionPanel codexConversionPanel = new CodexConversionPanel(headerDescription, ins, outs, prefab);
      codexWidgetList.Add((ICodexWidget) codexConversionPanel);
    }));
    ManualCodexConversionRegistry.GetConsumersForGivenInput(tag)?.ForEach((Action<ManualCodexConversionRegistry.ManualConversionEntry>) (ce =>
    {
      List<ICodexWidget> codexWidgetList = used;
      string headerDescription = ce.headerDescription;
      ElementUsage[] ins;
      if (ce.input == null)
        ins = (ElementUsage[]) null;
      else
        ins = new ElementUsage[1]
        {
          new ElementUsage(ce.input.first, ce.input.second, false, ce.inputCustomFormating)
        };
      ElementUsage[] outs;
      if (ce.output == null)
        outs = (ElementUsage[]) null;
      else
        outs = new ElementUsage[1]
        {
          new ElementUsage(ce.output.first, ce.output.second, false, ce.outputCustomFormating)
        };
      GameObject prefab = Assets.GetPrefab(ce.converter.first);
      CodexConversionPanel codexConversionPanel = new CodexConversionPanel(headerDescription, ins, outs, prefab);
      codexWidgetList.Add((ICodexWidget) codexConversionPanel);
    }));
    ContentContainer contents1 = new ContentContainer(used, ContentContainer.ContentLayout.Vertical);
    ContentContainer contents2 = new ContentContainer(made, ContentContainer.ContentLayout.Vertical);
    if (used.Count > 0)
    {
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexCollapsibleHeader((string) CODEX.HEADERS.ELEMENTCONSUMEDBY, contents1)
      }, ContentContainer.ContentLayout.Vertical));
      containers.Add(contents1);
    }
    if (made.Count <= 0)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexCollapsibleHeader((string) CODEX.HEADERS.ELEMENTPRODUCEDBY, contents2)
    }, ContentContainer.ContentLayout.Vertical));
    containers.Add(contents2);
  }

  private static void AddPlantFiberInfo(
    ref HashSet<ElementUsage> inSet,
    CodexEntryGenerator_Elements.CodexElementMap usedMap,
    CodexEntryGenerator_Elements.CodexElementMap madeMap,
    GameObject prefabOfProducer,
    GameObject prefabForPlayerFacing,
    Crop crop,
    Func<Tag, float, bool, string> customFormatting = null)
  {
    PlantFiberProducer component;
    if (!prefabOfProducer.TryGetComponent<PlantFiberProducer>(out component))
      return;
    CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
    ce.title = prefabForPlayerFacing.GetProperName();
    ce.prefab = prefabForPlayerFacing;
    ce.inSet = inSet;
    ce.outSet.Add(new ElementUsage((Tag) "PlantFiber", component.amount / crop.cropVal.cropDuration, true, customFormatting));
    ce.aidIcon1 = new CodexConversionPanel.IconSettings()
    {
      spriteName = "skillbadge_role_farming3",
      tooltip = (string) CODEX.MISC.TIP_ICON.FARMING3_SKILL.TOOLTIP,
      onClickActions = (System.Action) (() => ManagementMenu.Instance.OpenSkills((MinionIdentity) null))
    };
    usedMap.Add(prefabForPlayerFacing.PrefabID(), ce);
    madeMap.Add((Tag) "PlantFiber", ce);
  }

  private static void CheckPrefab(
    GameObject prefab,
    CodexEntryGenerator_Elements.CodexElementMap usedMap,
    CodexEntryGenerator_Elements.CodexElementMap madeMap)
  {
    HashSet<ElementUsage> inSet = new HashSet<ElementUsage>();
    HashSet<ElementUsage> outSet = new HashSet<ElementUsage>();
    List<ElementConverter> outputOnlyConverters = new List<ElementConverter>();
    List<ElementConverter> withInputsConverters = new List<ElementConverter>();
    List<ElementConverter> categoryConverters = new List<ElementConverter>();
    CodexEntryGenerator_Elements.PartitionElementConverters(prefab, outputOnlyConverters, withInputsConverters, categoryConverters);
    CodexEntryGenerator_Elements.CollectSharedConversionIO(prefab, inSet, outSet, outputOnlyConverters);
    CodexEntryGenerator_Elements.RegisterConversionEntries(prefab, inSet, outSet, usedMap, madeMap, withInputsConverters);
    CodexEntryGenerator_Elements.AddIndependentConversionEntries(prefab, inSet, usedMap, madeMap, categoryConverters);
  }

  private static void PartitionElementConverters(
    GameObject prefab,
    List<ElementConverter> outputOnlyConverters,
    List<ElementConverter> withInputsConverters,
    List<ElementConverter> categoryConverters)
  {
    foreach (ElementConverter elementConverter in (IEnumerable<ElementConverter>) prefab.GetComponents<ElementConverter>() ?? Enumerable.Empty<ElementConverter>())
    {
      if (elementConverter.inputIsCategory)
        categoryConverters.Add(elementConverter);
      else if (elementConverter.consumedElements != null && elementConverter.consumedElements.Length != 0)
        withInputsConverters.Add(elementConverter);
      else
        outputOnlyConverters.Add(elementConverter);
    }
  }

  private static void VerifyClaimedByproduct(
    GameObject prefab,
    List<ElementConverter> withInputsConverters,
    List<ElementConverter> categoryConverters)
  {
    IConverterByproduct implementingInterface = prefab.GetDefImplementingInterface<IConverterByproduct>();
    if (implementingInterface == null || (double) implementingInterface.ByproductRate <= 0.0)
      return;
    bool test = false;
    ElementUsage usage;
    foreach (ElementConverter withInputsConverter in withInputsConverters)
    {
      if (CodexEntryGenerator_Elements.TryGetByproductUsage(implementingInterface, withInputsConverter, out usage))
      {
        test = true;
        break;
      }
    }
    if (!test)
    {
      foreach (ElementConverter categoryConverter in categoryConverters)
      {
        if (CodexEntryGenerator_Elements.TryGetByproductUsage(implementingInterface, categoryConverter, out usage))
        {
          test = true;
          break;
        }
      }
    }
    DebugUtil.DevAssert(test, "IConverterByproduct has no associated ElementConverter");
  }

  private static void RegisterConversionEntries(
    GameObject prefab,
    HashSet<ElementUsage> inSet,
    HashSet<ElementUsage> outSet,
    CodexEntryGenerator_Elements.CodexElementMap usedMap,
    CodexEntryGenerator_Elements.CodexElementMap madeMap,
    List<ElementConverter> withInputsConverters)
  {
    IrrigationMonitor.Def def = prefab.GetDef<IrrigationMonitor.Def>();
    IConverterByproduct implementingInterface = prefab.GetDefImplementingInterface<IConverterByproduct>();
    if (withInputsConverters.Count == 0)
    {
      CodexEntryGenerator_Elements.RegisterIrrigationOrSingleEntry(prefab, inSet, outSet, usedMap, madeMap, def);
    }
    else
    {
      foreach (ElementConverter withInputsConverter in withInputsConverters)
      {
        HashSet<ElementUsage> inSet1 = new HashSet<ElementUsage>((IEnumerable<ElementUsage>) inSet);
        foreach (ElementConverter.ConsumedElement consumedElement in withInputsConverter.consumedElements)
          inSet1.Add(new ElementUsage(consumedElement.Tag, consumedElement.MassConsumptionRate, true));
        HashSet<ElementUsage> outSet1 = new HashSet<ElementUsage>((IEnumerable<ElementUsage>) outSet);
        foreach (ElementConverter.OutputElement outputElement in (IEnumerable<ElementConverter.OutputElement>) withInputsConverter.outputElements ?? Enumerable.Empty<ElementConverter.OutputElement>())
        {
          Tag tag = ElementLoader.FindElementByHash(outputElement.elementHash).tag;
          outSet1.Add(new ElementUsage(tag, outputElement.massGenerationRate, true));
        }
        ElementUsage usage;
        if (CodexEntryGenerator_Elements.TryGetByproductUsage(implementingInterface, withInputsConverter, out usage))
          outSet1.Add(usage);
        CodexEntryGenerator_Elements.RegisterIrrigationOrSingleEntry(prefab, inSet1, outSet1, usedMap, madeMap, def);
      }
    }
  }

  private static void RegisterIrrigationOrSingleEntry(
    GameObject prefab,
    HashSet<ElementUsage> inSet,
    HashSet<ElementUsage> outSet,
    CodexEntryGenerator_Elements.CodexElementMap usedMap,
    CodexEntryGenerator_Elements.CodexElementMap madeMap,
    IrrigationMonitor.Def irrigation)
  {
    if (irrigation != null)
    {
      foreach (PlantElementAbsorber.ConsumeInfo consumedElement in irrigation.consumedElements)
        CodexEntryGenerator_Elements.RegisterSingleEntry(prefab, new HashSet<ElementUsage>((IEnumerable<ElementUsage>) inSet)
        {
          new ElementUsage(consumedElement.tag, consumedElement.massConsumptionRate, true)
        }, outSet, usedMap, madeMap);
    }
    else
      CodexEntryGenerator_Elements.RegisterSingleEntry(prefab, inSet, outSet, usedMap, madeMap);
  }

  private static void RegisterSingleEntry(
    GameObject prefab,
    HashSet<ElementUsage> inSet,
    HashSet<ElementUsage> outSet,
    CodexEntryGenerator_Elements.CodexElementMap usedMap,
    CodexEntryGenerator_Elements.CodexElementMap madeMap)
  {
    CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
    ce.title = prefab.GetProperName();
    ce.prefab = prefab;
    ce.inSet = inSet;
    ce.outSet = outSet;
    if (inSet.Count > 0 && outSet.Count > 0)
      usedMap.Add(prefab.PrefabID(), ce);
    foreach (ElementUsage elementUsage in inSet)
      usedMap.Add(elementUsage.tag, ce);
    foreach (ElementUsage elementUsage in outSet)
      madeMap.Add(elementUsage.tag, ce);
    Crop component = prefab.GetComponent<Crop>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || prefab.GetComponent<IPlantConsumeEntities>() != null)
      return;
    CodexEntryGenerator_Elements.AddPlantFiberInfo(ref inSet, usedMap, madeMap, prefab, prefab, component);
  }

  private static bool TryGetByproductUsage(
    IConverterByproduct byproduct,
    ElementConverter conv,
    out ElementUsage usage)
  {
    usage = (ElementUsage) null;
    if (byproduct == null || (double) byproduct.ByproductRate <= 0.0)
      return false;
    foreach (ElementConverter.ConsumedElement consumedElement in (IEnumerable<ElementConverter.ConsumedElement>) conv.consumedElements ?? Enumerable.Empty<ElementConverter.ConsumedElement>())
    {
      if (consumedElement.Tag == byproduct.ByproductAssociatedInputTag)
      {
        usage = new ElementUsage(byproduct.ByproductTag, byproduct.ByproductRate, byproduct.ByproductIsContinuous);
        return true;
      }
    }
    return false;
  }

  private static void CollectSharedConversionIO(
    GameObject prefab,
    HashSet<ElementUsage> inSet,
    HashSet<ElementUsage> outSet,
    List<ElementConverter> outputOnlyConverters)
  {
    EnergyGenerator component1 = prefab.GetComponent<EnergyGenerator>();
    if ((bool) (UnityEngine.Object) component1)
    {
      foreach (EnergyGenerator.InputItem inputItem in (IEnumerable<EnergyGenerator.InputItem>) component1.formula.inputs ?? Enumerable.Empty<EnergyGenerator.InputItem>())
        inSet.Add(new ElementUsage(inputItem.tag, inputItem.consumptionRate, true));
      foreach (EnergyGenerator.OutputItem outputItem in (IEnumerable<EnergyGenerator.OutputItem>) component1.formula.outputs ?? Enumerable.Empty<EnergyGenerator.OutputItem>())
      {
        Tag tag = ElementLoader.FindElementByHash(outputItem.element).tag;
        outSet.Add(new ElementUsage(tag, outputItem.creationRate, true));
      }
    }
    foreach (ElementConverter outputOnlyConverter in outputOnlyConverters)
    {
      foreach (ElementConverter.OutputElement outputElement in (IEnumerable<ElementConverter.OutputElement>) outputOnlyConverter.outputElements ?? Enumerable.Empty<ElementConverter.OutputElement>())
      {
        Tag tag = ElementLoader.FindElementByHash(outputElement.elementHash).tag;
        outSet.Add(new ElementUsage(tag, outputElement.massGenerationRate, true));
      }
    }
    foreach (ElementConsumer elementConsumer in (IEnumerable<ElementConsumer>) prefab.GetComponents<ElementConsumer>() ?? Enumerable.Empty<ElementConsumer>())
    {
      if (!elementConsumer.storeOnConsume)
      {
        Tag tag = ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag;
        inSet.Add(new ElementUsage(tag, elementConsumer.consumptionRate, true));
      }
    }
    FertilizationMonitor.Def def = prefab.GetDef<FertilizationMonitor.Def>();
    if (def != null)
    {
      foreach (PlantElementAbsorber.ConsumeInfo consumedElement in def.consumedElements)
        inSet.Add(new ElementUsage(consumedElement.tag, consumedElement.massConsumptionRate, true));
    }
    Crop component2 = prefab.GetComponent<Crop>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && prefab.GetComponent<IPlantConsumeEntities>() == null)
      outSet.Add(new ElementUsage((Tag) component2.cropId, (float) component2.cropVal.numProduced / component2.cropVal.cropDuration, true));
    FlushToilet component3 = prefab.GetComponent<FlushToilet>();
    if ((bool) (UnityEngine.Object) component3)
    {
      inSet.Add(new ElementUsage(CodexEntryGenerator_Elements.WaterTag, component3.massConsumedPerUse, false));
      outSet.Add(new ElementUsage(CodexEntryGenerator_Elements.DirtyWaterTag, component3.massEmittedPerUse, false));
    }
    HandSanitizer component4 = prefab.GetComponent<HandSanitizer>();
    if (!(bool) (UnityEngine.Object) component4)
      return;
    Tag tag1 = ElementLoader.FindElementByHash(component4.consumedElement).tag;
    inSet.Add(new ElementUsage(tag1, component4.massConsumedPerUse, false));
    if (component4.outputElement == SimHashes.Vacuum)
      return;
    Tag tag2 = ElementLoader.FindElementByHash(component4.outputElement).tag;
    outSet.Add(new ElementUsage(tag2, component4.massConsumedPerUse, false));
  }

  private static void AddIndependentConversionEntries(
    GameObject prefab,
    HashSet<ElementUsage> inSet,
    CodexEntryGenerator_Elements.CodexElementMap usedMap,
    CodexEntryGenerator_Elements.CodexElementMap madeMap,
    List<ElementConverter> categoryConverters)
  {
    Crop component1 = prefab.GetComponent<Crop>();
    IPlantConsumeEntities component2 = prefab.GetComponent<IPlantConsumeEntities>();
    foreach (ElementConverter categoryConverter in categoryConverters)
    {
      List<CodexEntryGenerator_Elements.ConversionEntry> conversionEntryList = new List<CodexEntryGenerator_Elements.ConversionEntry>();
      foreach (ElementConverter.ConsumedElement consumedElement in (IEnumerable<ElementConverter.ConsumedElement>) categoryConverter.consumedElements ?? Enumerable.Empty<ElementConverter.ConsumedElement>())
      {
        ElementConverter.ConsumedElement c = consumedElement;
        foreach (Element element in ElementLoader.FindElements((Func<Element, bool>) (e => e.HasTag(c.Tag))))
          conversionEntryList.Add(new CodexEntryGenerator_Elements.ConversionEntry()
          {
            title = prefab.GetProperName(),
            prefab = prefab,
            inSet = {
              new ElementUsage(element.tag, c.MassConsumptionRate, true)
            }
          });
      }
      foreach (ElementConverter.OutputElement outputElement in (IEnumerable<ElementConverter.OutputElement>) categoryConverter.outputElements ?? Enumerable.Empty<ElementConverter.OutputElement>())
      {
        ElementUsage elementUsage = new ElementUsage(ElementLoader.FindElementByHash(outputElement.elementHash).tag, outputElement.massGenerationRate, true);
        foreach (CodexEntryGenerator_Elements.ConversionEntry conversionEntry in conversionEntryList)
          conversionEntry.outSet.Add(elementUsage);
      }
      ElementUsage usage;
      if (CodexEntryGenerator_Elements.TryGetByproductUsage(prefab.GetDefImplementingInterface<IConverterByproduct>(), categoryConverter, out usage))
      {
        foreach (CodexEntryGenerator_Elements.ConversionEntry conversionEntry in conversionEntryList)
          conversionEntry.outSet.Add(usage);
      }
      foreach (CodexEntryGenerator_Elements.ConversionEntry ce in conversionEntryList)
      {
        if (ce.inSet.Count > 0 && ce.outSet.Count > 0)
          usedMap.Add(prefab.PrefabID(), ce);
        foreach (ElementUsage elementUsage in ce.inSet)
          usedMap.Add(elementUsage.tag, ce);
        foreach (ElementUsage elementUsage in ce.outSet)
          madeMap.Add(elementUsage.tag, ce);
      }
    }
    IPlantBranchGrower implementingInterface = prefab.GetDefImplementingInterface<IPlantBranchGrower>();
    if (implementingInterface != null)
    {
      GameObject prefab1 = Assets.GetPrefab((Tag) implementingInterface.GetPlantBranchPrefabName());
      if ((UnityEngine.Object) prefab1 != (UnityEngine.Object) null)
      {
        Crop component3 = prefab1.GetComponent<Crop>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && ((UnityEngine.Object) component1 == (UnityEngine.Object) null || component3.cropId != component1.cropId || component3.cropVal.numProduced != component1.cropVal.numProduced))
        {
          CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
          ce.title = prefab1.GetProperName();
          ce.prefab = prefab;
          usedMap.Add(prefab.PrefabID(), ce);
          ce.inSet = new HashSet<ElementUsage>();
          IrrigationMonitor.Def def1 = prefab.GetDef<IrrigationMonitor.Def>();
          if (def1 != null)
          {
            foreach (PlantElementAbsorber.ConsumeInfo consumedElement in def1.consumedElements)
              ce.inSet.Add(new ElementUsage(consumedElement.tag, consumedElement.massConsumptionRate, true));
          }
          FertilizationMonitor.Def def2 = prefab.GetDef<FertilizationMonitor.Def>();
          if (def2 != null)
          {
            foreach (PlantElementAbsorber.ConsumeInfo consumedElement in def2.consumedElements)
              ce.inSet.Add(new ElementUsage(consumedElement.tag, consumedElement.massConsumptionRate, true));
          }
          ce.outSet = new HashSet<ElementUsage>();
          int branchCount = implementingInterface.GetMaxBranchCount();
          ce.outSet.Add(new ElementUsage((Tag) component3.cropId, (float) component3.cropVal.numProduced / component3.cropVal.cropDuration, true, (Func<Tag, float, bool, string>) ((t, a, b) => GameUtil.GetFormattedBranchGrowerPlantProductionValuePerCycle(t, a, branchCount))));
          madeMap.Add((Tag) component3.cropId, ce);
          CodexEntryGenerator_Elements.AddPlantFiberInfo(ref inSet, usedMap, madeMap, prefab1, prefab, component3, (Func<Tag, float, bool, string>) ((t, a, b) => GameUtil.GetFormattedBranchGrowerPlantPlantFiberProductionValuePerCycle(t, a, branchCount)));
        }
      }
    }
    if (component2 != null)
    {
      List<KPrefabID> prefabsOfPossiblePrey = component2.GetPrefabsOfPossiblePrey();
      List<string> stringList = new List<string>();
      foreach (KPrefabID kprefabId in prefabsOfPossiblePrey)
      {
        CreatureBrain component4 = kprefabId.GetComponent<CreatureBrain>();
        Tag tag = (UnityEngine.Object) component4 == (UnityEngine.Object) null ? kprefabId.PrefabID() : component4.species;
        string str = tag.ProperName();
        if (!stringList.Contains(str))
        {
          CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
          ce.title = $"{component2.GetConsumableEntitiesCategoryName()}: {str}";
          ce.prefab = prefab;
          ce.inSet.Add(new ElementUsage(tag, (UnityEngine.Object) component1 == (UnityEngine.Object) null ? 1f : 1f / component1.cropVal.cropDuration, (UnityEngine.Object) component1 != (UnityEngine.Object) null, (Func<Tag, float, bool, string>) ((t, amount, c) => GameUtil.GetFormattedUnits(amount, c ? GameUtil.TimeSlice.PerCycle : GameUtil.TimeSlice.None))));
          if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          {
            ce.outSet.Add(new ElementUsage((Tag) component1.cropId, (float) component1.cropVal.numProduced / component1.cropVal.cropDuration, true));
            madeMap.Add((Tag) component1.cropId, ce);
          }
          usedMap.Add(prefab.PrefabID(), ce);
          stringList.Add(str);
        }
      }
    }
    ScaleGrowthMonitor.Def def3 = prefab.GetDef<ScaleGrowthMonitor.Def>();
    if (def3 != null)
    {
      CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
      GameObject prefab2 = Assets.GetPrefab((Tag) (prefab.GetComponent<KPrefabID>().HasTag(GameTags.SwimmingCreature) ? "UnderwaterShearingStation" : "ShearingStation"));
      ce.title = prefab2.GetProperName();
      ce.prefab = prefab2;
      ce.inSet = new HashSet<ElementUsage>();
      ce.inSet.Add(new ElementUsage(prefab.PrefabID(), 1f, false));
      usedMap.Add(prefab.PrefabID(), ce);
      usedMap.Add(prefab2.PrefabID(), ce);
      ce.outSet = new HashSet<ElementUsage>();
      ce.outSet.Add(new ElementUsage(def3.itemDroppedOnShear, def3.dropMass, false));
      madeMap.Add(def3.itemDroppedOnShear, ce);
    }
    WellFedShearable.Def def4 = prefab.GetDef<WellFedShearable.Def>();
    if (def4 != null)
    {
      CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
      GameObject prefab3 = Assets.GetPrefab((Tag) (prefab.GetComponent<KPrefabID>().HasTag(GameTags.SwimmingCreature) ? "UnderwaterShearingStation" : "ShearingStation"));
      ce.title = prefab3.GetProperName();
      ce.prefab = prefab3;
      ce.inSet = new HashSet<ElementUsage>();
      ce.inSet.Add(new ElementUsage(prefab.PrefabID(), 1f, false));
      usedMap.Add(prefab.PrefabID(), ce);
      usedMap.Add(prefab3.PrefabID(), ce);
      ce.outSet = new HashSet<ElementUsage>();
      ce.outSet.Add(new ElementUsage(def4.itemDroppedOnShear, def4.dropMass, false));
      madeMap.Add(def4.itemDroppedOnShear, ce);
    }
    FertilityShearable.Def def5 = prefab.GetDef<FertilityShearable.Def>();
    if (def5 != null)
    {
      CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
      GameObject prefab4 = Assets.GetPrefab((Tag) "UnderwaterMilkingStation");
      ce.title = prefab4.GetProperName();
      ce.prefab = prefab4;
      ce.inSet = new HashSet<ElementUsage>()
      {
        new ElementUsage(prefab.PrefabID(), 1f, false)
      };
      usedMap.Add(prefab.PrefabID(), ce);
      usedMap.Add(prefab4.PrefabID(), ce);
      Tag tag = def5.milkElement.CreateTag();
      ce.outSet = new HashSet<ElementUsage>()
      {
        new ElementUsage(tag, def5.dropMass, false)
      };
      madeMap.Add(tag, ce);
    }
    MilkProductionMonitor.Def def6 = prefab.GetDef<MilkProductionMonitor.Def>();
    if (def6 != null)
    {
      string str = prefab.GetComponent<KPrefabID>().HasTag(GameTags.SwimmingCreature) ? "UnderwaterMilkingStation" : "MilkingStation";
      CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
      GameObject prefab5 = Assets.GetPrefab((Tag) str);
      ce.title = prefab5.GetProperName();
      ce.prefab = prefab5;
      ce.inSet = new HashSet<ElementUsage>();
      ce.inSet.Add(new ElementUsage(prefab.PrefabID(), 1f, false));
      usedMap.Add(prefab.PrefabID(), ce);
      usedMap.Add(prefab5.PrefabID(), ce);
      ce.outSet = new HashSet<ElementUsage>();
      ce.outSet.Add(new ElementUsage(def6.element.CreateTag(), def6.Capacity, false));
      madeMap.Add(def6.element.CreateTag(), ce);
    }
    MoistureMonitor.Def def7 = prefab.GetDef<MoistureMonitor.Def>();
    if (def7 != null)
    {
      string title = CODEX.HEADERS.SECRETED.Replace("{Creature}", prefab.GetProperName());
      CodexEntryGenerator_Elements.ConversionEntry ce = CodexEntryGenerator_Elements.SimpleConversionBase(usedMap, prefab, title);
      string id = Db.Get().Amounts.Mucus.deltaAttribute.Id;
      float num = 0.0f;
      foreach (AttributeModifier selfModifier in Db.Get().traits.Get(prefab.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
      {
        if (selfModifier.AttributeId == id)
        {
          num = selfModifier.Value;
          break;
        }
      }
      float amount1 = num + def7.GetMaxModification();
      ElementUsage elementUsage = new ElementUsage(def7.lubricant.CreateTag(), amount1, true)
      {
        customFormating = (Func<Tag, float, bool, string>) ((tag, amount, continous) => string.Format((string) CODEX.FORMAT_STRINGS.SECRETED, (object) GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.PerCycle)))
      };
      ce.outSet.Add(elementUsage);
      madeMap.Add(def7.lubricant.CreateTag(), ce);
    }
    MoltDropperMonitor.Def def8 = prefab.GetDef<MoltDropperMonitor.Def>();
    if (def8 != null)
    {
      CodexEntryGenerator_Elements.ConversionEntry ce = CodexEntryGenerator_Elements.SimpleConversionBase(usedMap, prefab, CODEX.HEADERS.MOLTED.Replace("{Creature}", prefab.GetProperName()));
      ElementUsage elementUsage = new ElementUsage((Tag) def8.onGrowDropID, def8.massToDrop / 600f, true)
      {
        customFormating = (Func<Tag, float, bool, string>) ((tag, amount, continous) => CODEX.FORMAT_STRINGS.MOLTED.Replace("{Amount}", GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.PerCycle)))
      };
      ce.outSet.Add(elementUsage);
      madeMap.Add((Tag) def8.onGrowDropID, ce);
    }
    Butcherable component5 = prefab.GetComponent<Butcherable>();
    if (!((UnityEngine.Object) component5 != (UnityEngine.Object) null))
      return;
    CodexEntryGenerator_Elements.ConversionEntry ce1 = new CodexEntryGenerator_Elements.ConversionEntry();
    ce1.title = prefab.GetProperName();
    ce1.prefab = prefab;
    usedMap.Add(prefab.PrefabID(), ce1);
    ce1.outSet = new HashSet<ElementUsage>();
    Dictionary<string, float> dictionary = new Dictionary<string, float>();
    foreach (KeyValuePair<string, float> drop in component5.drops)
    {
      float num;
      dictionary.TryGetValue(drop.Key, out num);
      dictionary[drop.Key] = num + Assets.GetPrefab((Tag) drop.Key).GetComponent<PrimaryElement>().Mass * drop.Value;
    }
    foreach (KeyValuePair<string, float> keyValuePair in dictionary)
    {
      string str;
      float num;
      keyValuePair.Deconstruct(ref str, ref num);
      string t = str;
      float amount = num;
      ce1.outSet.Add(new ElementUsage((Tag) t, amount, false));
      madeMap.Add((Tag) t, ce1);
    }
  }

  private static void AddDietConversions(
    GameObject prefab,
    CodexEntryGenerator_Elements.CodexElementMap usedMap,
    CodexEntryGenerator_Elements.CodexElementMap madeMap)
  {
    Diet diet = (Diet) null;
    CreatureCalorieMonitor.Def def1 = prefab.GetDef<CreatureCalorieMonitor.Def>();
    if (def1 != null)
    {
      diet = def1.diet;
    }
    else
    {
      BeehiveCalorieMonitor.Def def2 = prefab.GetDef<BeehiveCalorieMonitor.Def>();
      if (def2 != null)
        diet = def2.diet;
    }
    if (diet == null)
      return;
    float num1 = 0.0f;
    foreach (AttributeModifier selfModifier in Db.Get().traits.Get(prefab.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
    {
      if (selfModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
        num1 = selfModifier.Value;
    }
    foreach (Diet.Info info in diet.infos)
    {
      foreach (Tag consumedTag in info.consumedTags)
      {
        float amount1 = -num1 / info.caloriesPerKg;
        float amount2 = amount1 * info.producedConversionRate;
        int num2 = diet.IsConsumedTagAbleToBeEatenDirectly(consumedTag) ? 1 : 0;
        ElementUsage elementUsage = (ElementUsage) null;
        if (num2 != 0)
        {
          if (info.foodType == Diet.Info.FoodType.EatPlantDirectly)
            elementUsage = new ElementUsage(consumedTag, amount1, true, new Func<Tag, float, bool, string>(GameUtil.GetFormattedDirectPlantConsumptionValuePerCycle));
          else if (info.foodType == Diet.Info.FoodType.EatPlantStorage)
            elementUsage = new ElementUsage(consumedTag, amount1, true, new Func<Tag, float, bool, string>(GameUtil.GetFormattedPlantStorageConsumptionValuePerCycle));
          else if (info.foodType == Diet.Info.FoodType.EatPrey || info.foodType == Diet.Info.FoodType.EatButcheredPrey)
          {
            float num3 = diet.AvailableCaloriesInPrey(consumedTag);
            float amount3 = -num1 / num3;
            amount2 = amount3 * info.producedConversionRate * num3 / info.caloriesPerKg;
            elementUsage = new ElementUsage(consumedTag, amount3, true, new Func<Tag, float, bool, string>(GameUtil.GetFormattedPreyConsumptionValuePerCycle));
          }
        }
        else
          elementUsage = new ElementUsage(consumedTag, amount1, true);
        CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry();
        ce.title = prefab.GetProperName();
        ce.prefab = prefab;
        ce.inSet.Add(elementUsage);
        ce.outSet.Add(new ElementUsage(info.producedElement, amount2, true));
        usedMap.Add(consumedTag, ce);
        madeMap.Add(info.producedElement, ce);
      }
    }
  }

  public static CodexEntryGenerator_Elements.ElementEntryContext GetElementEntryContext()
  {
    if (CodexEntryGenerator_Elements.contextInstance != null)
      return CodexEntryGenerator_Elements.contextInstance;
    CodexEntryGenerator_Elements.CodexElementMap usedMap = new CodexEntryGenerator_Elements.CodexElementMap();
    CodexEntryGenerator_Elements.CodexElementMap madeMap = new CodexEntryGenerator_Elements.CodexElementMap();
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
        if ((UnityEngine.Object) buildingDef == (UnityEngine.Object) null)
          Debug.LogError((object) $"Building def for id {keyValuePair.Key} is null");
        if (!buildingDef.Deprecated && !buildingDef.BuildingComplete.HasTag(GameTags.DevBuilding))
          CodexEntryGenerator_Elements.CheckPrefab(buildingDef.BuildingComplete, usedMap, madeMap);
      }
    }
    HashSet<GameObject> gameObjectSet = new HashSet<GameObject>((IEnumerable<GameObject>) Assets.GetPrefabsWithComponent<Harvestable>());
    foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<WiltCondition>())
      gameObjectSet.Add(gameObject);
    foreach (GameObject gameObject in gameObjectSet)
    {
      if (!gameObject.HasTag(GameTags.HideFromCodex))
        CodexEntryGenerator_Elements.CheckPrefab(gameObject, usedMap, madeMap);
    }
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<CreatureBrain>();
    foreach (GameObject gameObject in prefabsWithComponent)
    {
      if (gameObject.GetDef<BabyMonitor.Def>() == null)
        CodexEntryGenerator_Elements.CheckPrefab(gameObject, usedMap, madeMap);
    }
    foreach (GameObject gameObject in prefabsWithComponent)
    {
      if (gameObject.GetDef<BabyMonitor.Def>() == null)
        CodexEntryGenerator_Elements.AddDietConversions(gameObject, usedMap, madeMap);
    }
    CodexEntryGenerator_Elements.contextInstance = new CodexEntryGenerator_Elements.ElementEntryContext()
    {
      usedMap = usedMap,
      madeMap = madeMap
    };
    return CodexEntryGenerator_Elements.contextInstance;
  }

  private static CodexEntryGenerator_Elements.ConversionEntry SimpleConversionBase(
    CodexEntryGenerator_Elements.CodexElementMap usedMap,
    GameObject prefab,
    string title = null)
  {
    CodexEntryGenerator_Elements.ConversionEntry ce = new CodexEntryGenerator_Elements.ConversionEntry()
    {
      title = title == null ? prefab.GetProperName() : title,
      prefab = prefab,
      inSet = new HashSet<ElementUsage>()
    };
    usedMap.Add(prefab.PrefabID(), ce);
    return ce;
  }

  public class ConversionEntry
  {
    public string title;
    public GameObject prefab;
    public HashSet<ElementUsage> inSet = new HashSet<ElementUsage>();
    public HashSet<ElementUsage> outSet = new HashSet<ElementUsage>();
    public CodexConversionPanel.IconSettings aidIcon1;
  }

  public class CodexElementMap
  {
    public Dictionary<Tag, List<CodexEntryGenerator_Elements.ConversionEntry>> map = new Dictionary<Tag, List<CodexEntryGenerator_Elements.ConversionEntry>>();

    public void Add(Tag t, CodexEntryGenerator_Elements.ConversionEntry ce)
    {
      List<CodexEntryGenerator_Elements.ConversionEntry> conversionEntryList;
      if (this.map.TryGetValue(t, out conversionEntryList))
        conversionEntryList.Add(ce);
      else
        this.map[t] = new List<CodexEntryGenerator_Elements.ConversionEntry>()
        {
          ce
        };
    }
  }

  public class ElementEntryContext
  {
    public CodexEntryGenerator_Elements.CodexElementMap madeMap = new CodexEntryGenerator_Elements.CodexElementMap();
    public CodexEntryGenerator_Elements.CodexElementMap usedMap = new CodexEntryGenerator_Elements.CodexElementMap();
  }
}
