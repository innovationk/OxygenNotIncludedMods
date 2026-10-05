// Decompiled with JetBrains decompiler
// Type: CodexEntryGenerator_Creatures
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
public class CodexEntryGenerator_Creatures
{
  public const string CATEGORY_ID = "CREATURES";
  public const string GUIDE_ID = "CREATURES::GUIDE";
  public const string GUIDE_METABOLISM_ID = "CREATURES::GUIDE::METABOLISM";
  public const string GUIDE_MOOD_ID = "CREATURES::GUIDE::MOOD";
  public const string GUIDE_FERTILITY_ID = "CREATURES::GUIDE::FERTILITY";
  public const string GUIDE_DOMESTICATION_ID = "CREATURES::GUIDE::DOMESTICATION";

  public static Dictionary<string, CodexEntry> GenerateEntries()
  {
    Dictionary<string, CodexEntry> results = new Dictionary<string, CodexEntry>();
    List<GameObject> brains = Assets.GetPrefabsWithComponent<CreatureBrain>();
    List<(string, CodexEntry)> critterEntries = new List<(string, CodexEntry)>();
    AddEntry("CREATURES::GUIDE", CodexEntryGenerator_Creatures.GenerateFieldGuideEntry());
    foreach (Tag speciesTag in GameTags.Creatures.Species.AllSpecies_REFLECTION())
      PushCritterEntry(speciesTag);
    PopAndAddAllCritterEntries();
    return results;

    void AddEntry(string entryId, CodexEntry entry, string parentEntryId = "CREATURES")
    {
      if (entry == null)
        return;
      entry.parentId = parentEntryId;
      CodexCache.AddEntry(entryId, entry);
      results.Add(entryId, entry);
    }

    void PushCritterEntry(Tag speciesTag)
    {
      CodexEntry critterEntry = CodexEntryGenerator_Creatures.GenerateCritterEntry(speciesTag, speciesTag.ProperName(), brains);
      if (critterEntry == null)
        return;
      critterEntries.Add((speciesTag.ToString(), critterEntry));
    }

    void PopAndAddAllCritterEntries()
    {
      foreach ((string, CodexEntry) tuple in (IEnumerable<(string, CodexEntry)>) critterEntries.StableSort<(string, CodexEntry), string>((Func<(string, CodexEntry), string>) (pair => UI.StripLinkFormatting(pair.Item2.name))))
        AddEntry(tuple.Item1, tuple.Item2);
    }
  }

  private static CodexEntry GenerateFieldGuideEntry()
  {
    CodexEntry generalInfoEntry = new CodexEntry("CREATURES", new List<ContentContainer>(), (string) CODEX.CRITTERSTATUS.CRITTERSTATUS_TITLE);
    generalInfoEntry.icon = Assets.GetSprite((HashedString) "codex_critter_emotions");
    List<ICodexWidget> subEntryContents = (List<ICodexWidget>) null;
    SubEntry subEntry = (SubEntry) null;
    AddSubEntry("CREATURES::GUIDE::METABOLISM", (string) CODEX.CRITTERSTATUS.METABOLISM.TITLE);
    AddImage(Assets.GetSprite((HashedString) "codex_metabolism"));
    AddBody((string) CODEX.CRITTERSTATUS.METABOLISM.BODY.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.METABOLISM.HUNGRY.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.METABOLISM.HUNGRY.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.METABOLISM.STARVING.TITLE);
    AddBody(string.Format((string) (DlcManager.IsExpansion1Active() ? CODEX.CRITTERSTATUS.METABOLISM.STARVING.CONTAINER1_DLC1 : CODEX.CRITTERSTATUS.METABOLISM.STARVING.CONTAINER1_VANILLA), (object) 10));
    AddSpacer();
    AddSubEntry("CREATURES::GUIDE::MOOD", (string) CODEX.CRITTERSTATUS.MOOD.TITLE);
    AddImage(Assets.GetSprite((HashedString) "codex_mood"));
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.BODY.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.MOOD.HAPPY.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.HAPPY.CONTAINER1);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.HAPPY.SUBTITLE);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.MOOD.HAPPY.HAPPY_METABOLISM);
    AddSubtitle((string) CODEX.CRITTERSTATUS.MOOD.NEUTRAL.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.NEUTRAL.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.MOOD.GLUM.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.GLUM.CONTAINER1);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.GLUM.SUBTITLE);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.MOOD.GLUM.GLUMWILD_METABOLISM);
    AddSubtitle((string) CODEX.CRITTERSTATUS.MOOD.MISERABLE.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.MISERABLE.CONTAINER1);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.MISERABLE.SUBTITLE);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.MOOD.MISERABLE.MISERABLEWILD_METABOLISM);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.MOOD.MISERABLE.MISERABLEWILD_FERTILITY);
    AddSubtitle((string) CODEX.CRITTERSTATUS.MOOD.HOSTILE.TITLE);
    AddBody((string) (DlcManager.IsExpansion1Active() ? CODEX.CRITTERSTATUS.MOOD.HOSTILE.CONTAINER1_DLC1 : CODEX.CRITTERSTATUS.MOOD.HOSTILE.CONTAINER1_VANILLA));
    AddSubtitle((string) CODEX.CRITTERSTATUS.MOOD.CONFINED.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.CONFINED.CONTAINER1);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.CONFINED.SUBTITLE);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.MOOD.CONFINED.CONFINED_FERTILITY);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.MOOD.CONFINED.CONFINED_HAPPINESS);
    AddSubtitle((string) CODEX.CRITTERSTATUS.MOOD.OVERCROWDED.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.OVERCROWDED.CONTAINER1);
    AddBody((string) CODEX.CRITTERSTATUS.MOOD.OVERCROWDED.SUBTITLE);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.MOOD.OVERCROWDED.OVERCROWDED_HAPPY1);
    AddSpacer();
    AddSubEntry("CREATURES::GUIDE::FERTILITY", (string) CODEX.CRITTERSTATUS.FERTILITY.TITLE);
    AddImage(Assets.GetSprite((HashedString) "codex_reproduction"));
    AddBody((string) CODEX.CRITTERSTATUS.FERTILITY.BODY.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.FERTILITY.FERTILITYRATE.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.FERTILITY.FERTILITYRATE.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.FERTILITY.EGGCHANCES.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.FERTILITY.EGGCHANCES.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.FERTILITY.FUTURE_OVERCROWDED.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.FERTILITY.FUTURE_OVERCROWDED.CONTAINER1);
    AddBody((string) CODEX.CRITTERSTATUS.FERTILITY.FUTURE_OVERCROWDED.SUBTITLE);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.FERTILITY.FUTURE_OVERCROWDED.CRAMPED_FERTILITY);
    AddSubtitle((string) CODEX.CRITTERSTATUS.FERTILITY.INCUBATION.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.FERTILITY.INCUBATION.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.FERTILITY.MAXAGE.TITLE);
    AddBody((string) (DlcManager.IsExpansion1Active() ? CODEX.CRITTERSTATUS.FERTILITY.MAXAGE.CONTAINER1_DLC1 : CODEX.CRITTERSTATUS.FERTILITY.MAXAGE.CONTAINER1_VANILLA));
    AddSpacer();
    AddSubEntry("CREATURES::GUIDE::DOMESTICATION", (string) CODEX.CRITTERSTATUS.DOMESTICATION.TITLE);
    AddImage(Assets.GetSprite((HashedString) "codex_domestication"));
    AddBody((string) CODEX.CRITTERSTATUS.DOMESTICATION.BODY.CONTAINER1);
    AddSubtitle((string) CODEX.CRITTERSTATUS.DOMESTICATION.WILD.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.DOMESTICATION.WILD.CONTAINER1);
    AddBody((string) CODEX.CRITTERSTATUS.DOMESTICATION.WILD.SUBTITLE);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.DOMESTICATION.WILD.WILD_METABOLISM);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.DOMESTICATION.WILD.WILD_POOP);
    AddSubtitle((string) CODEX.CRITTERSTATUS.DOMESTICATION.TAME.TITLE);
    AddBody((string) CODEX.CRITTERSTATUS.DOMESTICATION.TAME.CONTAINER1);
    AddBody((string) CODEX.CRITTERSTATUS.DOMESTICATION.TAME.SUBTITLE);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.DOMESTICATION.TAME.TAME_HAPPINESS);
    AddBulletPoint((string) CODEX.CRITTERSTATUS.DOMESTICATION.TAME.TAME_METABOLISM);
    return generalInfoEntry;

    void AddSubEntry(string id, string name)
    {
      subEntryContents = new List<ICodexWidget>();
      subEntryContents.Add((ICodexWidget) new CodexText(name, CodexTextStyle.Title));
      ref CodexEntryGenerator_Creatures.\u003C\u003Ec__DisplayClass7_1 local = ref obj3;
      string id1 = id;
      List<ContentContainer> contentContainers = new List<ContentContainer>();
      contentContainers.Add(new ContentContainer(subEntryContents, ContentContainer.ContentLayout.Vertical));
      string name1 = name;
      SubEntry subEntry = new SubEntry(id1, "CREATURES::GUIDE", contentContainers, name1);
      // ISSUE: reference to a compiler-generated field
      local.subEntry = subEntry;
      generalInfoEntry.subEntries.Add(subEntry);
    }

    void AddImage(Sprite sprite)
    {
      subEntryContents.Add((ICodexWidget) new CodexImage(432, 1, sprite));
    }

    void AddSubtitle(string text)
    {
      AddSpacer();
      subEntryContents.Add((ICodexWidget) new CodexText(text, CodexTextStyle.Subtitle));
    }

    void AddBody(string text) => subEntryContents.Add((ICodexWidget) new CodexText(text));

    void AddSpacer() => subEntryContents.Add((ICodexWidget) new CodexSpacer());

    void AddBulletPoint(string text)
    {
      if (text.StartsWith("    • "))
        text = text.Substring("    • ".Length);
      text = $"<indent=13px>•<indent=21px>{text}</indent></indent>";
      subEntryContents.Add((ICodexWidget) new CodexText(text));
    }
  }

  private static CodexEntry GenerateCritterEntry(
    Tag speciesTag,
    string name,
    List<GameObject> brains)
  {
    CodexEntry critterEntry = (CodexEntry) null;
    List<ContentContainer> contentContainers = new List<ContentContainer>();
    foreach (GameObject brain in brains)
    {
      if (brain.GetDef<BabyMonitor.Def>() == null && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) brain.GetComponent<KPrefabID>()))
      {
        Sprite sprite = (Sprite) null;
        CreatureBrain component = brain.GetComponent<CreatureBrain>();
        if (!(component.species != speciesTag))
        {
          if (critterEntry == null)
          {
            critterEntry = new CodexEntry("CREATURES", contentContainers, name);
            critterEntry.sortString = name;
            contentContainers.Add(new ContentContainer(new List<ICodexWidget>()
            {
              (ICodexWidget) new CodexSpacer(),
              (ICodexWidget) new CodexSpacer()
            }, ContentContainer.ContentLayout.Vertical));
          }
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          string symbolPrefix = component.symbolPrefix;
          Sprite first = Def.GetUISprite((object) brain, symbolPrefix + "ui").first;
          Tag tag = brain.PrefabID();
          GameObject prefab = Assets.TryGetPrefab((Tag) (tag.ToString() + "Baby"));
          if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
            sprite = Def.GetUISprite((object) prefab).first;
          if ((bool) (UnityEngine.Object) sprite)
            CodexEntryGenerator.GenerateImageContainers(new Sprite[2]
            {
              first,
              sprite
            }, contentContainerList, ContentContainer.ContentLayout.Horizontal);
          else
            CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
          CodexEntryGenerator_Creatures.GenerateCreatureDescriptionContainers(brain, contentContainerList);
          tag = component.PrefabID();
          critterEntry.subEntries.Add(new SubEntry(tag.ToString(), speciesTag.ToString(), contentContainerList, component.GetProperName())
          {
            icon = first,
            iconColor = Color.white
          });
        }
      }
    }
    return critterEntry;
  }

  private static void GenerateCreatureDescriptionContainers(
    GameObject creature,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(creature.GetComponent<InfoDescription>().description)
    }, ContentContainer.ContentLayout.Vertical));
    RobotBatteryMonitor.Def def1 = creature.GetDef<RobotBatteryMonitor.Def>();
    if (def1 != null)
    {
      Amount batteryAmount = Db.Get().Amounts.Get(def1.batteryAmountId);
      float num = Db.Get().traits.Get(creature.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers.Find((Predicate<AttributeModifier>) (match => match.AttributeId == batteryAmount.maxAttribute.Id)).Value;
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.INTERNALBATTERY, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.ROBOT_DESCRIPTORS.BATTERY.CAPACITY, (object) num))
      }, ContentContainer.ContentLayout.Vertical));
    }
    if (creature.GetDef<StorageUnloadMonitor.Def>() != null)
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.INTERNALSTORAGE, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.ROBOT_DESCRIPTORS.STORAGE.CAPACITY, (object) creature.GetComponents<Storage>()[1].Capacity()))
      }, ContentContainer.ContentLayout.Vertical));
    List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag((creature.PrefabID().ToString() + "Egg").ToTag());
    if (prefabsWithTag != null && prefabsWithTag.Count > 0)
    {
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.HATCHESFROMEGG, CodexTextStyle.Subtitle)
      }, ContentContainer.ContentLayout.Vertical));
      foreach (GameObject go in prefabsWithTag)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexIndentedLabelWithIcon(go.GetProperName(), CodexTextStyle.Body, Def.GetUISprite((object) go))
        }, ContentContainer.ContentLayout.Horizontal));
    }
    CritterTemperatureMonitor.Def def2 = creature.GetDef<CritterTemperatureMonitor.Def>();
    if (def2 != null)
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.COMFORTRANGE, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.CREATURE_DESCRIPTORS.TEMPERATURE.COMFORT_RANGE, (object) GameUtil.GetFormattedTemperature(def2.temperatureColdUncomfortable), (object) GameUtil.GetFormattedTemperature(def2.temperatureHotUncomfortable))),
        (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.CREATURE_DESCRIPTORS.TEMPERATURE.NON_LETHAL_RANGE, (object) GameUtil.GetFormattedTemperature(def2.temperatureColdDeadly), (object) GameUtil.GetFormattedTemperature(def2.temperatureHotDeadly)))
      }, ContentContainer.ContentLayout.Vertical));
    Modifiers component1 = creature.GetComponent<Modifiers>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      Klei.AI.Attribute maxAttribute = Db.Get().Amounts.Age.maxAttribute;
      float totalValue = AttributeInstance.GetTotalValue(maxAttribute, component1.GetPreModifiers(maxAttribute));
      string text = !Mathf.Approximately(totalValue, 0.0f) ? string.Format((string) CODEX.CREATURE_DESCRIPTORS.MAXAGE, (object) maxAttribute.formatter.GetFormattedValue(totalValue, GameUtil.TimeSlice.None)) : (string) null;
      if (text != null)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexSpacer(),
          (ICodexWidget) new CodexText((string) CODEX.HEADERS.CRITTERMAXAGE, CodexTextStyle.Subtitle),
          (ICodexWidget) new CodexText(text)
        }, ContentContainer.ContentLayout.Vertical));
    }
    OvercrowdingMonitor.Def def3 = creature.GetDef<OvercrowdingMonitor.Def>();
    if (def3 != null && def3.spaceRequiredPerCreature > 0)
    {
      int num = creature.GetDef<FishOvercrowdingMonitor.Def>() != null ? 1 : 0;
      LocString format1 = num != 0 ? CODEX.CREATURE_DESCRIPTORS.OVERCROWDING_AQUATIC : CODEX.CREATURE_DESCRIPTORS.OVERCROWDING;
      LocString format2 = num != 0 ? CODEX.CREATURE_DESCRIPTORS.CONFINED_AQUATIC : CODEX.CREATURE_DESCRIPTORS.CONFINED;
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.CRITTEROVERCROWDING, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText("    • " + string.Format((string) format1, (object) def3.spaceRequiredPerCreature)),
        (ICodexWidget) new CodexText("    • " + string.Format((string) format2, (object) def3.spaceRequiredPerCreature))
      }, ContentContainer.ContentLayout.Vertical));
    }
    string key = (string) null;
    float amount = 0.0f;
    Tag tag = new Tag();
    Butcherable component2 = creature.GetComponent<Butcherable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.drops != null && component2.drops.Count > 0)
    {
      tag.Name = key = component2.drops.Keys.ToList<string>()[0];
      amount = component2.drops[key];
    }
    string text1 = (string) null;
    string text2 = (string) null;
    if (tag.IsValid)
    {
      text1 = TagManager.GetProperName(tag);
      text2 = "\t" + GameUtil.GetFormattedByTag(tag, amount);
    }
    if (!string.IsNullOrEmpty(text1) && !string.IsNullOrEmpty(text2))
    {
      ContentContainer contentContainer1 = new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.CRITTERDROPS, CodexTextStyle.Subtitle)
      }, ContentContainer.ContentLayout.Vertical);
      ContentContainer contentContainer2 = new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexIndentedLabelWithIcon(text1, CodexTextStyle.Body, Def.GetUISprite((object) key)),
        (ICodexWidget) new CodexText(text2)
      }, ContentContainer.ContentLayout.Vertical);
      containers.Add(contentContainer1);
      containers.Add(contentContainer2);
    }
    List<Tag> tagList = new List<Tag>();
    Diet prefabDiet = DietManager.Instance.GetPrefabDiet(creature);
    if (prefabDiet == null)
      return;
    Diet.Info[] infos = prefabDiet.infos;
    if (infos == null || infos.Length == 0)
      return;
    float num1 = 0.0f;
    foreach (AttributeModifier selfModifier in Db.Get().traits.Get(creature.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
    {
      if (selfModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
        num1 = selfModifier.Value;
    }
    CaloriesConsumedElementProducer component3 = creature.GetComponent<CaloriesConsumedElementProducer>();
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (Diet.Info info in infos)
    {
      if (info.consumedTags.Count != 0)
      {
        foreach (Tag consumedTag in info.consumedTags)
        {
          Element elementByHash = ElementLoader.FindElementByHash(ElementLoader.GetElementID(consumedTag));
          if (elementByHash.id != SimHashes.Vacuum && elementByHash.id != SimHashes.Void || !((UnityEngine.Object) Assets.GetPrefab(consumedTag) == (UnityEngine.Object) null))
          {
            int num2 = prefabDiet.IsConsumedTagAbleToBeEatenDirectly(consumedTag) ? 1 : 0;
            float inputAmount = -num1 / info.caloriesPerKg;
            float outputAmount1 = inputAmount * info.producedConversionRate;
            if (num2 != 0)
            {
              if (info.foodType == Diet.Info.FoodType.EatPlantDirectly)
                content.Add((ICodexWidget) new CodexConversionPanel(consumedTag.ProperName(), consumedTag, inputAmount, true, new Func<Tag, float, bool, string>(GameUtil.GetFormattedDirectPlantConsumptionValuePerCycle), info.producedElement, outputAmount1, true, (Func<Tag, float, bool, string>) null, creature));
              else if (info.foodType == Diet.Info.FoodType.EatPlantStorage)
                content.Add((ICodexWidget) new CodexConversionPanel(consumedTag.ProperName(), consumedTag, inputAmount, true, new Func<Tag, float, bool, string>(GameUtil.GetFormattedPlantStorageConsumptionValuePerCycle), info.producedElement, outputAmount1, true, (Func<Tag, float, bool, string>) null, creature));
              else if (info.foodType == Diet.Info.FoodType.EatPrey || info.foodType == Diet.Info.FoodType.EatButcheredPrey)
              {
                float num3 = prefabDiet.AvailableCaloriesInPrey(consumedTag);
                inputAmount = -num1 / num3;
                float outputAmount2 = inputAmount * info.producedConversionRate * num3 / info.caloriesPerKg;
                content.Add((ICodexWidget) new CodexConversionPanel(consumedTag.ProperName(), consumedTag, inputAmount, true, new Func<Tag, float, bool, string>(GameUtil.GetFormattedPreyConsumptionValuePerCycle), info.producedElement, outputAmount2, true, (Func<Tag, float, bool, string>) null, creature));
              }
            }
            else
              content.Add((ICodexWidget) new CodexConversionPanel(consumedTag.ProperName(), consumedTag, inputAmount, true, info.producedElement, outputAmount1, true, creature));
            if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
              content.Add((ICodexWidget) new CodexConversionPanel((string) CODEX.HEADERS.CRITTER_EXTRA_DIET_PRODUCTION, consumedTag, inputAmount, true, component3.producedElement.CreateTag(), (float) ((double) inputAmount * 1000.0 * (double) component3.kgProducedPerKcalConsumed * 2.0), true, creature));
          }
        }
      }
    }
    ContentContainer contents = new ContentContainer(content, ContentContainer.ContentLayout.Vertical);
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexCollapsibleHeader((string) CODEX.HEADERS.DIET, contents)
    }, ContentContainer.ContentLayout.Vertical));
    containers.Add(contents);
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
    CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(creature.PrefabID(), containers);
  }
}
