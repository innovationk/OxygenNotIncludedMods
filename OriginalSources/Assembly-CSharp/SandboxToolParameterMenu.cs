// Decompiled with JetBrains decompiler
// Type: SandboxToolParameterMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class SandboxToolParameterMenu : KScreen
{
  public static SandboxToolParameterMenu instance;
  public SandboxSettings settings;
  [SerializeField]
  private GameObject sliderPropertyPrefab;
  [SerializeField]
  private GameObject selectorPropertyPrefab;
  private List<GameObject> inputFields = new List<GameObject>();
  public SandboxToolParameterMenu.SelectorValue elementSelector;
  public SandboxToolParameterMenu.SliderValue brushRadiusSlider = new SandboxToolParameterMenu.SliderValue(1f, 10f, "dash", "circle_hard", "", (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_SIZE.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_SIZE.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.BrushSize", Mathf.Clamp(Mathf.RoundToInt(value), 1, 50))));
  public SandboxToolParameterMenu.SliderValue noiseScaleSlider = new SandboxToolParameterMenu.SliderValue(0.0f, 1f, "little", "lots", "", (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.NoiseScale", value)), 2);
  public SandboxToolParameterMenu.SliderValue noiseDensitySlider = new SandboxToolParameterMenu.SliderValue(1f, 20f, "little", "lots", "", (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_SCALE.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.BRUSH_NOISE_DENSITY.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.NoiseDensity", value)), 2);
  public SandboxToolParameterMenu.SliderValue massSlider = new SandboxToolParameterMenu.SliderValue(0.1f, 1000f, "action_pacify", "status_item_plant_solid", (string) STRINGS.UI.UNITSUFFIXES.MASS.KILOGRAM, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.MASS.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.MASS.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.Mass", Mathf.Clamp(value, 1f / 1000f, 9999f))), 2);
  public SandboxToolParameterMenu.SliderValue temperatureSlider = new SandboxToolParameterMenu.SliderValue(150f, 500f, "cold", "hot", GameUtil.GetTemperatureUnitSuffix(), (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.Temperature", Mathf.Clamp(GameUtil.GetTemperatureConvertedToKelvin(value), 1f, 9999f))));
  public SandboxToolParameterMenu.SelectorValue targetLayerSelector;
  public SandboxToolParameterMenu.SliderValue temperatureAdditiveSlider = new SandboxToolParameterMenu.SliderValue(-15f, 15f, "cold", "hot", GameUtil.GetTemperatureUnitSuffix(), (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE_ADDITIVE.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.TEMPERATURE_ADDITIVE.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.TemperatureAdditive", GameUtil.GetTemperatureConvertedToKelvin(value))));
  public SandboxToolParameterMenu.SliderValue stressAdditiveSlider = new SandboxToolParameterMenu.SliderValue(-10f, 10f, "little", "lots", (string) STRINGS.UI.UNITSUFFIXES.PERCENT, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.STRESS_ADDITIVE.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.STRESS_ADDITIVE.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.StressAdditive", value)));
  public SandboxToolParameterMenu.SliderValue moraleSlider = new SandboxToolParameterMenu.SliderValue(-25f, 25f, "little", "lots", (string) STRINGS.UI.UNITSUFFIXES.UNITS, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.MORALE.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.MORALE.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetIntSetting("SandbosTools.MoraleAdjustment", Mathf.RoundToInt(value))));
  public SandboxToolParameterMenu.SelectorValue diseaseSelector;
  public SandboxToolParameterMenu.SliderValue diseaseCountSlider = new SandboxToolParameterMenu.SliderValue(0.0f, 10000f, "status_item_barren", "germ", (string) STRINGS.UI.UNITSUFFIXES.DISEASE.UNITS, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.DISEASE_COUNT.TOOLTIP, (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.DISEASE_COUNT.NAME, (Action<float>) (value => SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.DiseaseCount", Mathf.RoundToInt(value))));
  public SandboxToolParameterMenu.SelectorValue entitySelector;
  public SandboxToolParameterMenu.SelectorValue storySelector;

  public static void DestroyInstance()
  {
    SandboxToolParameterMenu.instance = (SandboxToolParameterMenu) null;
  }

  public override float GetSortKey() => 50f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ConfigureSettings();
    this.activateOnSpawn = true;
    this.ConsumeMouseScroll = true;
  }

  private void ConfigureSettings()
  {
    this.massSlider.clampValueLow = 1f / 1000f;
    this.massSlider.clampValueHigh = 10000f;
    this.temperatureAdditiveSlider.clampValueLow = -9999f;
    this.temperatureAdditiveSlider.clampValueHigh = 9999f;
    this.temperatureSlider.clampValueLow = -458f;
    this.temperatureSlider.clampValueHigh = 9999f;
    this.brushRadiusSlider.clampValueLow = 1f;
    this.brushRadiusSlider.clampValueHigh = 50f;
    this.diseaseCountSlider.clampValueHigh = 1000000f;
    this.diseaseCountSlider.slideMaxValue = 1000000f;
    this.settings = new SandboxSettings();
    this.settings.OnChangeElement += (Action<bool>) (forceElementDefaults =>
    {
      int index = this.settings.GetIntSetting("SandboxTools.SelectedElement");
      if (index >= ElementLoader.elements.Count)
        index = 0;
      Element element = ElementLoader.elements[index];
      this.elementSelector.button.GetComponentInChildren<LocText>().text = $"{element.name} ({element.GetStateString()})";
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) element);
      this.elementSelector.button.GetComponentsInChildren<Image>()[1].sprite = uiSprite.first;
      this.elementSelector.button.GetComponentsInChildren<Image>()[1].color = uiSprite.second;
      this.SetAbsoluteTemperatureSliderRange(element);
      this.massSlider.SetRange(0.1f, Mathf.Min(element.maxMass * 2f, this.massSlider.clampValueHigh), false);
      if (!forceElementDefaults)
        return;
      this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(element.defaultValues.temperature, true));
      this.massSlider.SetValue(element.defaultValues.mass);
    });
    this.settings.OnChangeTargetLayer += (System.Action) (() =>
    {
      string str = (string) (this.settings.GetIntSetting("SandboxTools.SelectedTargetLayer") == 1 ? STRINGS.UI.SANDBOXTOOLS.SETTINGS.TARGET_LAYER.BACKGROUND : STRINGS.UI.SANDBOXTOOLS.SETTINGS.TARGET_LAYER.FOREGROUND);
      this.targetLayerSelector.button.GetComponentInChildren<LocText>().text = str;
    });
    this.settings.OnChangeMass += (System.Action) (() => this.massSlider.SetValue(this.settings.GetFloatSetting("SandboxTools.Mass"), false));
    this.settings.OnChangeDisease += (System.Action) (() =>
    {
      Klei.AI.Disease disease = Db.Get().Diseases.TryGet(SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedDisease")) ?? Db.Get().Diseases.Get("FoodPoisoning");
      this.diseaseSelector.button.GetComponentInChildren<LocText>().text = disease.Name;
      this.diseaseSelector.button.GetComponentsInChildren<Image>()[1].sprite = Assets.GetSprite((HashedString) "germ");
      this.diseaseCountSlider.SetRange(0.0f, 1000000f, false);
    });
    this.settings.OnChangeDiseaseCount += (System.Action) (() => this.diseaseCountSlider.SetValue((float) this.settings.GetIntSetting("SandboxTools.DiseaseCount"), false));
    this.settings.OnChangeStory += (System.Action) (() =>
    {
      string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedStory");
      Story story = Db.Get().Stories.TryGet(stringSetting);
      if (story == null)
      {
        this.settings.ForceDefaultStringSetting("SandboxTools.SelectedStory");
      }
      else
      {
        this.storySelector.button.GetComponentInChildren<LocText>().text = (string) Strings.Get(story.StoryTrait.name);
        this.storySelector.button.GetComponentsInChildren<Image>()[1].sprite = Assets.GetSprite((HashedString) story.StoryTrait.icon);
      }
    });
    this.settings.OnChangeEntity += (System.Action) (() =>
    {
      string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
      GameObject prefab = Assets.TryGetPrefab((Tag) stringSetting);
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) prefab.GetComponent<KPrefabID>()) || prefab.HasTag(GameTags.HideFromSpawnTool))
      {
        this.settings.ForceDefaultStringSetting("SandboxTools.SelectedEntity");
      }
      else
      {
        this.entitySelector.button.GetComponentInChildren<LocText>().text = prefab.GetProperName();
        Tuple<Sprite, Color> tuple = !prefab.HasTag(GameTags.BaseMinion) ? Def.GetUISprite((object) stringSetting) : new Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel(prefab.PrefabID()), Color.white);
        if (tuple == null)
          return;
        this.entitySelector.button.GetComponentsInChildren<Image>()[1].sprite = tuple.first;
        this.entitySelector.button.GetComponentsInChildren<Image>()[1].color = tuple.second;
      }
    });
    this.settings.OnChangeBrushSize += (System.Action) (() =>
    {
      if (!(PlayerController.Instance.ActiveTool is BrushTool))
        return;
      (PlayerController.Instance.ActiveTool as BrushTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
    });
    this.settings.OnChangeNoiseScale += (System.Action) (() =>
    {
      if (!(PlayerController.Instance.ActiveTool is SandboxSprinkleTool))
        return;
      (PlayerController.Instance.ActiveTool as SandboxSprinkleTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
    });
    this.settings.OnChangeNoiseDensity += (System.Action) (() =>
    {
      if (!(PlayerController.Instance.ActiveTool is SandboxSprinkleTool))
        return;
      (PlayerController.Instance.ActiveTool as SandboxSprinkleTool).SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
    });
    this.settings.OnChangeTemperature += (System.Action) (() => this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.Temperature")), false));
    this.settings.OnChangeAdditiveTemperature += (System.Action) (() => this.temperatureAdditiveSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.TemperatureAdditive"), true), false));
    Game.Instance.Subscribe(999382396, new Action<object>(this.OnTemperatureUnitChanged));
    this.settings.OnChangeAdditiveStress += (System.Action) (() => this.stressAdditiveSlider.SetValue(this.settings.GetFloatSetting("SandbosTools.StressAdditive"), false));
    this.settings.OnChangeMoraleAdjustment += (System.Action) (() => this.moraleSlider.SetValue((float) this.settings.GetIntSetting("SandbosTools.MoraleAdjustment"), false));
  }

  public void DisableParameters()
  {
    this.elementSelector.row.SetActive(false);
    this.targetLayerSelector.row.SetActive(false);
    this.entitySelector.row.SetActive(false);
    this.brushRadiusSlider.row.SetActive(false);
    this.noiseScaleSlider.row.SetActive(false);
    this.noiseDensitySlider.row.SetActive(false);
    this.massSlider.row.SetActive(false);
    this.temperatureAdditiveSlider.row.SetActive(false);
    this.temperatureSlider.row.SetActive(false);
    this.diseaseCountSlider.row.SetActive(false);
    this.diseaseSelector.row.SetActive(false);
    this.stressAdditiveSlider.row.SetActive(false);
    this.moraleSlider.row.SetActive(false);
    this.storySelector.row.SetActive(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.ConfigureElementSelector();
    this.ConfigureDiseaseSelector();
    this.ConfigureEntitySelector();
    this.ConfigureStoryTraitSelector();
    this.ConfigureTargetLayerSelector();
    this.SpawnSelector(this.entitySelector);
    this.SpawnSelector(this.elementSelector);
    this.SpawnSelector(this.storySelector);
    this.SpawnSlider(this.brushRadiusSlider);
    this.SpawnSlider(this.noiseScaleSlider);
    this.SpawnSlider(this.noiseDensitySlider);
    this.SpawnSlider(this.massSlider);
    this.SpawnSlider(this.temperatureSlider);
    this.SpawnSelector(this.targetLayerSelector);
    this.SpawnSlider(this.temperatureAdditiveSlider);
    this.SpawnSlider(this.stressAdditiveSlider);
    this.SpawnSelector(this.diseaseSelector);
    this.SpawnSlider(this.diseaseCountSlider);
    this.SpawnSlider(this.moraleSlider);
    if (!((UnityEngine.Object) SandboxToolParameterMenu.instance == (UnityEngine.Object) null))
      return;
    SandboxToolParameterMenu.instance = this;
    this.gameObject.SetActive(false);
    this.settings.RestorePrefs();
  }

  private void ConfigureElementSelector()
  {
    Func<object, bool> condition1 = (Func<object, bool>) (element => (element as Element).IsSolid);
    Func<object, bool> condition2 = (Func<object, bool>) (element => (element as Element).IsLiquid);
    Func<object, bool> condition3 = (Func<object, bool>) (element => (element as Element).IsGas);
    List<Element> commonElements = new List<Element>();
    Func<object, bool> condition4 = (Func<object, bool>) (element => commonElements.Contains(element as Element));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Oxygen));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Water));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Vacuum));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Dirt));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.SandStone));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Cuprite));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Steel));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Algae));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.CrudeOil));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.CarbonDioxide));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Sand));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.SlimeMold));
    commonElements.Insert(0, ElementLoader.FindElementByHash(SimHashes.Granite));
    List<Element> elementList = new List<Element>();
    foreach (Element element in ElementLoader.elements)
    {
      if (!element.disabled)
      {
        bool flag = false;
        foreach (Tag oreTag in element.oreTags)
        {
          if (oreTag == GameTags.HideFromSpawnTool)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          elementList.Add(element);
      }
    }
    elementList.Sort((Comparison<Element>) ((a, b) => a.name.CompareTo(b.name)));
    this.elementSelector = new SandboxToolParameterMenu.SelectorValue((object[]) elementList.ToArray(), (Action<object>) (element => this.settings.SetIntSetting("SandboxTools.SelectedElement", (int) ((Element) element).idx)), (Func<object, string>) (element => $"{(element as Element).name} ({(element as Element).GetStateString()})"), (Func<string, object, bool>) ((filterString, option) => ((option as Element).name.ToUpper() + (option as Element).GetStateString().ToUpper()).Contains(filterString.ToUpper())), (Func<object, Tuple<Sprite, Color>>) (element => Def.GetUISprite((object) (element as Element))), (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.ELEMENT.NAME, new SandboxToolParameterMenu.SelectorValue.SearchFilter[4]
    {
      new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.COMMON, condition4),
      new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.SOLID, condition1, icon: Def.GetUISprite((object) ElementLoader.FindElementByHash(SimHashes.SandStone))),
      new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.LIQUID, condition2, icon: Def.GetUISprite((object) ElementLoader.FindElementByHash(SimHashes.Water))),
      new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.GAS, condition3, icon: Def.GetUISprite((object) ElementLoader.FindElementByHash(SimHashes.Oxygen)))
    });
  }

  private void ConfigureEntitySelector()
  {
    List<SandboxToolParameterMenu.SelectorValue.SearchFilter> searchFilterList = new List<SandboxToolParameterMenu.SelectorValue.SearchFilter>();
    searchFilterList.Add(new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.FOOD, (Func<object, bool>) (entity =>
    {
      KPrefabID kprefabId = entity as KPrefabID;
      string idString = kprefabId.PrefabID().ToString();
      return !kprefabId.HasTag(GameTags.Egg) && EdiblesManager.GetAllFoodTypes().Find((Predicate<EdiblesManager.FoodInfo>) (match => match.Id == idString)) != null || kprefabId.HasTag(GameTags.Dehydrated);
    }), icon: Def.GetUISprite((object) Assets.GetPrefab((Tag) "MushBar"))));
    searchFilterList.Add(new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.COMETS, (Func<object, bool>) (entity =>
    {
      KPrefabID restrictions = entity as KPrefabID;
      return restrictions.HasTag(GameTags.Comet) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions);
    }), icon: Def.GetUISprite((object) Assets.GetPrefab((Tag) CopperCometConfig.ID))));
    searchFilterList.Add(new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SPECIAL, (Func<object, bool>) (entity =>
    {
      KPrefabID restrictions = entity as KPrefabID;
      return (entity as KPrefabID).HasTag(GameTags.BaseMinion) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions);
    }), icon: new Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel(GameTags.Minions.Models.Standard), Color.white)));
    if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
    {
      SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.BIONICUPGRADES, (Func<object, bool>) (entity =>
      {
        KPrefabID restrictions = entity as KPrefabID;
        return restrictions.HasTag(GameTags.BionicUpgrade) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions);
      }), icon: new Tuple<Sprite, Color>(Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim((HashedString) "upgrade_disc_kanim")), Color.white));
      searchFilterList.Add(searchFilter);
    }
    SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter1 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.CREATURE, (Func<object, bool>) (entity => false), icon: Def.GetUISprite((object) Assets.GetPrefab((Tag) "Hatch")));
    searchFilterList.Add(parentFilter1);
    List<Tag> tagList = new List<Tag>();
    foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.CreatureBrain))
    {
      CreatureBrain brain = gameObject.GetComponent<CreatureBrain>();
      KPrefabID component1 = brain.GetComponent<KPrefabID>();
      if (!component1.HasTag(GameTags.HideFromSpawnTool) && !tagList.Contains(brain.species) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) component1))
      {
        Tuple<Sprite, Color> icon = (Tuple<Sprite, Color>) null;
        CodexEntry codexEntry;
        if (CodexCache.entries.TryGetValue(brain.species.ToString().ToUpper(), out codexEntry))
          icon = new Tuple<Sprite, Color>(codexEntry.icon, codexEntry.iconColor);
        tagList.Add(brain.species);
        SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + brain.species.ToString().ToUpper()), (Func<object, bool>) (entity =>
        {
          CreatureBrain component2 = Assets.GetPrefab((entity as KPrefabID).PrefabID()).GetComponent<CreatureBrain>();
          return (entity as KPrefabID).HasTag(GameTags.CreatureBrain) && component2.species == brain.species;
        }), parentFilter1, icon);
        searchFilterList.Add(searchFilter);
      }
    }
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter1 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.CREATURE_EGG, (Func<object, bool>) (entity => (entity as KPrefabID).HasTag(GameTags.Egg)), parentFilter1, Def.GetUISprite((object) Assets.GetPrefab((Tag) "HatchEgg")));
    searchFilterList.Add(searchFilter1);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter2 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.EQUIPMENT, (Func<object, bool>) (entity =>
    {
      if ((UnityEngine.Object) (entity as KPrefabID).gameObject == (UnityEngine.Object) null)
        return false;
      GameObject gameObject = (entity as KPrefabID).gameObject;
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<Equippable>() != (UnityEngine.Object) null;
    }), icon: Def.GetUISprite((object) Assets.GetPrefab((Tag) "Funky_Vest")));
    searchFilterList.Add(searchFilter2);
    SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter2 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.PLANTS, (Func<object, bool>) (entity =>
    {
      KPrefabID restrictions = entity as KPrefabID;
      if ((UnityEngine.Object) restrictions == (UnityEngine.Object) null || (UnityEngine.Object) restrictions.gameObject == (UnityEngine.Object) null || !((UnityEngine.Object) restrictions != (UnityEngine.Object) null) || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
        return false;
      return (UnityEngine.Object) restrictions.GetComponent<Harvestable>() != (UnityEngine.Object) null || (UnityEngine.Object) restrictions.GetComponent<WiltCondition>() != (UnityEngine.Object) null;
    }), icon: Def.GetUISprite((object) Assets.GetPrefab((Tag) "PrickleFlower")));
    searchFilterList.Add(parentFilter2);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter3 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.SEEDS, (Func<object, bool>) (entity =>
    {
      if ((UnityEngine.Object) (entity as KPrefabID).gameObject == (UnityEngine.Object) null)
        return false;
      GameObject gameObject = (entity as KPrefabID).gameObject;
      return (UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<PlantableSeed>() != (UnityEngine.Object) null;
    }), parentFilter2, Def.GetUISprite((object) Assets.GetPrefab((Tag) "PrickleFlowerSeed")));
    searchFilterList.Add(searchFilter3);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter4 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.INDUSTRIAL_PRODUCTS, (Func<object, bool>) (entity =>
    {
      KPrefabID restrictions = entity as KPrefabID;
      if ((UnityEngine.Object) restrictions == (UnityEngine.Object) null || (UnityEngine.Object) restrictions.gameObject == (UnityEngine.Object) null || restrictions.HasTag(GameTags.DeprecatedContent) || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
        return false;
      return restrictions.HasTag(GameTags.IndustrialIngredient) || restrictions.HasTag(GameTags.TechComponents) || restrictions.HasTag(GameTags.IndustrialProduct) || restrictions.HasTag(GameTags.Medicine) || restrictions.HasTag(GameTags.MedicalSupplies) || restrictions.HasTag(GameTags.ChargedPortableBattery) || restrictions.HasTag(TableSaltConfig.TAG) || restrictions.HasTag(GameTags.MoltShell);
    }), icon: Def.GetUISprite((object) Assets.GetPrefab((Tag) "BasicCure")));
    searchFilterList.Add(searchFilter4);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter5 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.ORE_CHUNKS, (Func<object, bool>) (entity =>
    {
      Element elementByTag = ElementLoader.FindElementByTag((entity as KPrefabID).PrefabTag);
      return elementByTag != null && elementByTag.IsSolid;
    }), icon: new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_elements-solid"), Color.white));
    searchFilterList.Add(searchFilter5);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter6 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.BOTTLES, (Func<object, bool>) (entity =>
    {
      Element elementByTag = ElementLoader.FindElementByTag((entity as KPrefabID).PrefabTag);
      return elementByTag != null && elementByTag.IsLiquid;
    }), icon: new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_elements-liquids"), Color.white));
    searchFilterList.Add(searchFilter6);
    SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter7 = new SandboxToolParameterMenu.SelectorValue.SearchFilter((string) STRINGS.UI.SANDBOXTOOLS.FILTERS.ENTITIES.CANISTERS, (Func<object, bool>) (entity =>
    {
      Element elementByTag = ElementLoader.FindElementByTag((entity as KPrefabID).PrefabTag);
      return elementByTag != null && elementByTag.IsGas;
    }), icon: new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_elements-gases"), Color.white));
    searchFilterList.Add(searchFilter7);
    List<KPrefabID> kprefabIdList = new List<KPrefabID>();
    foreach (KPrefabID prefab in Assets.Prefabs)
    {
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) prefab) && !prefab.HasTag(GameTags.HideFromSpawnTool))
      {
        foreach (SandboxToolParameterMenu.SelectorValue.SearchFilter searchFilter8 in searchFilterList)
        {
          if (searchFilter8.condition((object) prefab))
          {
            kprefabIdList.Add(prefab);
            break;
          }
        }
      }
    }
    this.entitySelector = new SandboxToolParameterMenu.SelectorValue((object[]) kprefabIdList.ToArray(), (Action<object>) (entity => this.settings.SetStringSetting("SandboxTools.SelectedEntity", (entity as KPrefabID).PrefabID().Name)), (Func<object, string>) (entity => (entity as KPrefabID).GetProperName()), (Func<string, object, bool>) null, (Func<object, Tuple<Sprite, Color>>) (entity =>
    {
      GameObject prefab = Assets.GetPrefab((entity as KPrefabID).PrefabTag);
      KPrefabID component3 = prefab.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        if (component3.HasTag(GameTags.BaseMinion))
          return new Tuple<Sprite, Color>(BaseMinionConfig.GetSpriteForMinionModel((entity as KPrefabID).PrefabID()), Color.white);
        KBatchedAnimController component4 = prefab.GetComponent<KBatchedAnimController>();
        if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && component4.AnimFiles.Length != 0 && (UnityEngine.Object) component4.AnimFiles[0] != (UnityEngine.Object) null)
          return Def.GetUISprite((object) prefab);
      }
      return (Tuple<Sprite, Color>) null;
    }), (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.SPAWN_ENTITY.NAME, searchFilterList.ToArray());
  }

  private void ConfigureStoryTraitSelector()
  {
    this.storySelector = new SandboxToolParameterMenu.SelectorValue((object[]) Db.Get().Stories.resources.ToArray(), (Action<object>) (story => this.settings.SetStringSetting("SandboxTools.SelectedStory", ((Resource) story).Id)), (Func<object, string>) (story => (string) Strings.Get((story as Story).StoryTrait.name)), (Func<string, object, bool>) null, (Func<object, Tuple<Sprite, Color>>) (story => new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) ((Story) story).StoryTrait.icon), Color.white)), (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.SPAWN_STORY_TRAIT.NAME);
  }

  private void ConfigureTargetLayerSelector()
  {
    this.targetLayerSelector = new SandboxToolParameterMenu.SelectorValue(new object[2]
    {
      (object) 0,
      (object) 1
    }, (Action<object>) (layer => this.settings.SetIntSetting("SandboxTools.SelectedTargetLayer", (int) layer)), (Func<object, string>) (layer => (string) ((int) layer == 1 ? STRINGS.UI.SANDBOXTOOLS.SETTINGS.TARGET_LAYER.BACKGROUND : STRINGS.UI.SANDBOXTOOLS.SETTINGS.TARGET_LAYER.FOREGROUND)), (Func<string, object, bool>) null, (Func<object, Tuple<Sprite, Color>>) (layer => new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "dash"), Color.white)), (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.TARGET_LAYER.NAME);
  }

  private void ConfigureDiseaseSelector()
  {
    this.diseaseSelector = new SandboxToolParameterMenu.SelectorValue((object[]) Db.Get().Diseases.resources.ToArray(), (Action<object>) (disease => this.settings.SetStringSetting("SandboxTools.SelectedDisease", ((Resource) disease).Id)), (Func<object, string>) (disease => (disease as Klei.AI.Disease).Name), (Func<string, object, bool>) null, (Func<object, Tuple<Sprite, Color>>) (disease => new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "germ"), (Color) GlobalAssets.Instance.colorSet.GetColorByName((disease as Klei.AI.Disease).overlayColourName))), (string) STRINGS.UI.SANDBOXTOOLS.SETTINGS.DISEASE.NAME);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!((UnityEngine.Object) PlayerController.Instance.ActiveTool != (UnityEngine.Object) null) || !((UnityEngine.Object) SandboxToolParameterMenu.instance != (UnityEngine.Object) null))
      return;
    this.RefreshDisplay();
  }

  public void RefreshDisplay()
  {
    this.brushRadiusSlider.row.SetActive(PlayerController.Instance.ActiveTool is BrushTool);
    if (PlayerController.Instance.ActiveTool is BrushTool)
      this.brushRadiusSlider.SetValue((float) this.settings.GetIntSetting("SandboxTools.BrushSize"));
    this.massSlider.SetValue(this.settings.GetFloatSetting("SandboxTools.Mass"));
    this.stressAdditiveSlider.SetValue(this.settings.GetFloatSetting("SandbosTools.StressAdditive"));
    this.RefreshTemperatureUnitDisplays();
    this.temperatureSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.Temperature"), true));
    this.temperatureAdditiveSlider.SetValue(GameUtil.GetConvertedTemperature(this.settings.GetFloatSetting("SandbosTools.TemperatureAdditive"), true));
    this.diseaseCountSlider.SetValue((float) this.settings.GetIntSetting("SandboxTools.DiseaseCount"));
    this.moraleSlider.SetValue((float) this.settings.GetIntSetting("SandbosTools.MoraleAdjustment"));
  }

  private void OnTemperatureUnitChanged(object _)
  {
    int index = this.settings.GetIntSetting("SandboxTools.SelectedElement");
    if (index >= ElementLoader.elements.Count)
      index = 0;
    this.SetAbsoluteTemperatureSliderRange(ElementLoader.elements[index]);
    this.temperatureAdditiveSlider.SetValue(5f);
  }

  private void SetAbsoluteTemperatureSliderRange(Element element)
  {
    float temperature1 = Mathf.Max(element.lowTemp - 10f, 1f);
    float temperature2;
    if (element.IsGas)
      temperature2 = Mathf.Min(9999f, element.highTemp + 10f, element.defaultValues.temperature + 100f);
    else
      temperature2 = Mathf.Min(9999f, element.highTemp + 10f);
    this.temperatureSlider.SetRange(GameUtil.GetConvertedTemperature(temperature1, true), GameUtil.GetConvertedTemperature(temperature2, true), false);
  }

  private void RefreshTemperatureUnitDisplays()
  {
    this.temperatureSlider.unitString = GameUtil.GetTemperatureUnitSuffix();
    this.temperatureSlider.row.GetComponent<HierarchyReferences>().GetReference<LocText>("UnitLabel").text = this.temperatureSlider.unitString;
    this.temperatureAdditiveSlider.unitString = GameUtil.GetTemperatureUnitSuffix();
    this.temperatureAdditiveSlider.row.GetComponent<HierarchyReferences>().GetReference<LocText>("UnitLabel").text = this.temperatureSlider.unitString;
  }

  private GameObject SpawnSelector(SandboxToolParameterMenu.SelectorValue selector)
  {
    GameObject gameObject1 = Util.KInstantiateUI(this.selectorPropertyPrefab, this.gameObject, true);
    HierarchyReferences component = gameObject1.GetComponent<HierarchyReferences>();
    GameObject panel = component.GetReference("ScrollPanel").gameObject;
    GameObject gameObject2 = component.GetReference("Content").gameObject;
    InputField filterInputField = component.GetReference<InputField>("Filter");
    component.GetReference<LocText>("Label").SetText(selector.labelText);
    Game.Instance.Subscribe(1174281782, (Action<object>) (data =>
    {
      if (!panel.activeSelf)
        return;
      panel.SetActive(false);
    }));
    KButton reference = component.GetReference<KButton>("Button");
    reference.onClick += (System.Action) (() =>
    {
      panel.SetActive(!panel.activeSelf);
      if (!panel.activeSelf)
        return;
      panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
      filterInputField.ActivateInputField();
      filterInputField.onValueChanged.Invoke(filterInputField.text);
    });
    GameObject gameObject3 = component.GetReference("optionPrefab").gameObject;
    selector.row = gameObject1;
    selector.optionButtons = new List<KeyValuePair<object, GameObject>>();
    GameObject clearFilterButton = Util.KInstantiateUI(gameObject3, gameObject2);
    clearFilterButton.GetComponentInChildren<LocText>().text = (string) STRINGS.UI.SANDBOXTOOLS.FILTERS.BACK;
    clearFilterButton.GetComponentsInChildren<Image>()[1].enabled = false;
    clearFilterButton.GetComponent<KButton>().onClick += (System.Action) (() =>
    {
      selector.currentFilter = (SandboxToolParameterMenu.SelectorValue.SearchFilter) null;
      selector.optionButtons.ForEach((Action<KeyValuePair<object, GameObject>>) (test =>
      {
        if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter)
          test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null);
        else
          test.Value.SetActive(false);
      }));
      clearFilterButton.SetActive(false);
      panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
      filterInputField.text = "";
      filterInputField.onValueChanged.Invoke(filterInputField.text);
    });
    if (selector.filters != null)
    {
      foreach (SandboxToolParameterMenu.SelectorValue.SearchFilter filter1 in selector.filters)
      {
        SandboxToolParameterMenu.SelectorValue.SearchFilter filter = filter1;
        GameObject gameObject4 = Util.KInstantiateUI(gameObject3, gameObject2);
        gameObject4.SetActive(filter.parentFilter == null);
        gameObject4.GetComponentInChildren<LocText>().text = filter.Name;
        if (filter.icon != null)
        {
          gameObject4.GetComponentsInChildren<Image>()[1].sprite = filter.icon.first;
          gameObject4.GetComponentsInChildren<Image>()[1].color = filter.icon.second;
        }
        gameObject4.GetComponent<KButton>().onClick += (System.Action) (() =>
        {
          selector.currentFilter = filter;
          clearFilterButton.SetActive(true);
          selector.optionButtons.ForEach((Action<KeyValuePair<object, GameObject>>) (test =>
          {
            if (!(test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter))
              test.Value.SetActive(selector.runCurrentFilter(test.Key));
            else if ((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null)
              test.Value.SetActive(false);
            else
              test.Value.SetActive((test.Key as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == filter);
          }));
          panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
        });
        selector.optionButtons.Add(new KeyValuePair<object, GameObject>((object) filter, gameObject4));
      }
    }
    foreach (object option1 in selector.options)
    {
      object option = option1;
      GameObject gameObject5 = Util.KInstantiateUI(gameObject3, gameObject2, true);
      gameObject5.GetComponentInChildren<LocText>().text = selector.getOptionName(option);
      gameObject5.GetComponent<KButton>().onClick += (System.Action) (() =>
      {
        selector.onValueChanged(option);
        panel.SetActive(false);
      });
      Tuple<Sprite, Color> tuple = selector.getOptionSprite(option);
      gameObject5.GetComponentsInChildren<Image>()[1].sprite = tuple.first;
      gameObject5.GetComponentsInChildren<Image>()[1].color = tuple.second;
      selector.optionButtons.Add(new KeyValuePair<object, GameObject>(option, gameObject5));
      if (option is SandboxToolParameterMenu.SelectorValue.SearchFilter)
        gameObject5.SetActive((option as SandboxToolParameterMenu.SelectorValue.SearchFilter).parentFilter == null);
      else
        gameObject5.SetActive(false);
    }
    selector.button = reference;
    filterInputField.onValueChanged.AddListener((UnityAction<string>) (filterString =>
    {
      if (!clearFilterButton.activeSelf && !string.IsNullOrEmpty(filterString))
        clearFilterButton.SetActive(true);
      List<KeyValuePair<object, GameObject>> keyValuePairList = new List<KeyValuePair<object, GameObject>>();
      bool flag = selector.optionButtons.Find((Predicate<KeyValuePair<object, GameObject>>) (match => match.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter)).Key != null;
      if (string.IsNullOrEmpty(filterString))
      {
        if (!flag)
          selector.optionButtons.ForEach((Action<KeyValuePair<object, GameObject>>) (test => test.Value.SetActive(true)));
        else
          selector.optionButtons.ForEach((Action<KeyValuePair<object, GameObject>>) (test =>
          {
            if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter && ((SandboxToolParameterMenu.SelectorValue.SearchFilter) test.Key).parentFilter == null)
              test.Value.SetActive(true);
            else
              test.Value.SetActive(false);
          }));
      }
      else
        selector.optionButtons.ForEach((Action<KeyValuePair<object, GameObject>>) (test =>
        {
          if (test.Key is SandboxToolParameterMenu.SelectorValue.SearchFilter)
            test.Value.SetActive(((SandboxToolParameterMenu.SelectorValue.SearchFilter) test.Key).Name.ToUpper().Contains(filterString.ToUpper()));
          else
            test.Value.SetActive(selector.getOptionName(test.Key).ToUpper().Contains(filterString.ToUpper()));
        }));
      if (selector.filterOptionFunction != null)
      {
        foreach (object option2 in selector.options)
        {
          object option = option2;
          foreach (KeyValuePair<object, GameObject> keyValuePair in selector.optionButtons.FindAll((Predicate<KeyValuePair<object, GameObject>>) (match => match.Key == option)))
          {
            if (string.IsNullOrEmpty(filterString))
              keyValuePair.Value.SetActive(false);
            else
              keyValuePair.Value.SetActive(selector.filterOptionFunction(filterString, option));
          }
        }
      }
      panel.GetComponent<KScrollRect>().verticalNormalizedPosition = 1f;
    }));
    this.inputFields.Add(filterInputField.gameObject);
    panel.SetActive(false);
    return gameObject1;
  }

  private GameObject SpawnSlider(SandboxToolParameterMenu.SliderValue value)
  {
    GameObject gameObject = Util.KInstantiateUI(this.sliderPropertyPrefab, this.gameObject, true);
    HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("BottomIcon").sprite = Assets.GetSprite((HashedString) value.bottomSprite);
    component.GetReference<Image>("TopIcon").sprite = Assets.GetSprite((HashedString) value.topSprite);
    component.GetReference<LocText>("Label").SetText(value.labelText);
    KSlider slider = component.GetReference<KSlider>("Slider");
    KNumberInputField inputField = component.GetReference<KNumberInputField>("InputField");
    gameObject.GetComponent<ToolTip>().SetSimpleTooltip(value.tooltip);
    slider.minValue = value.slideMinValue;
    slider.maxValue = value.slideMaxValue;
    inputField.minValue = value.clampValueLow;
    inputField.maxValue = value.clampValueHigh;
    this.inputFields.Add(inputField.gameObject);
    value.slider = slider;
    inputField.decimalPlaces = value.roundToDecimalPlaces;
    value.inputField = inputField;
    value.row = gameObject;
    slider.onReleaseHandle += (System.Action) (() =>
    {
      slider.value = Mathf.Round(slider.value * Mathf.Pow(10f, (float) value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float) value.roundToDecimalPlaces);
      inputField.currentValue = Mathf.Round(slider.value * Mathf.Pow(10f, (float) value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float) value.roundToDecimalPlaces);
      inputField.SetDisplayValue(inputField.currentValue.ToString());
      if (value.onValueChanged == null)
        return;
      value.onValueChanged(slider.value);
    });
    slider.onDrag += (System.Action) (() =>
    {
      float num = Mathf.Round(slider.value * Mathf.Pow(10f, (float) value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float) value.roundToDecimalPlaces);
      slider.value = num;
      inputField.currentValue = num;
      inputField.SetDisplayValue(inputField.currentValue.ToString());
      if (value.onValueChanged == null)
        return;
      value.onValueChanged(slider.value);
    });
    slider.onMove += (System.Action) (() =>
    {
      float num = Mathf.Round(slider.value * Mathf.Pow(10f, (float) value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float) value.roundToDecimalPlaces);
      slider.value = num;
      inputField.currentValue = num;
      inputField.SetDisplayValue(inputField.currentValue.ToString());
      if (value.onValueChanged == null)
        return;
      value.onValueChanged(slider.value);
    });
    inputField.onEndEdit += (System.Action) (() =>
    {
      float num = Mathf.Round(inputField.currentValue * Mathf.Pow(10f, (float) value.roundToDecimalPlaces)) / Mathf.Pow(10f, (float) value.roundToDecimalPlaces);
      inputField.SetDisplayValue(num.ToString());
      slider.value = num;
      if (value.onValueChanged == null)
        return;
      value.onValueChanged(num);
    });
    component.GetReference<LocText>("UnitLabel").text = value.unitString;
    return gameObject;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.CheckBlockedInput())
    {
      if (e.Consumed)
        return;
      e.Consumed = true;
    }
    else
      base.OnKeyDown(e);
  }

  private bool CheckBlockedInput()
  {
    bool flag = false;
    if ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current != (UnityEngine.Object) null)
    {
      GameObject selectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
      if ((UnityEngine.Object) selectedGameObject != (UnityEngine.Object) null)
      {
        foreach (GameObject inputField in this.inputFields)
        {
          if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) inputField.gameObject)
          {
            flag = true;
            break;
          }
        }
      }
    }
    return flag;
  }

  public class SelectorValue
  {
    public GameObject row;
    public List<KeyValuePair<object, GameObject>> optionButtons;
    public KButton button;
    public object[] options;
    public Action<object> onValueChanged;
    public Func<object, string> getOptionName;
    public Func<string, object, bool> filterOptionFunction;
    public Func<object, Tuple<Sprite, Color>> getOptionSprite;
    public SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters;
    public List<SandboxToolParameterMenu.SelectorValue.SearchFilter> activeFilters = new List<SandboxToolParameterMenu.SelectorValue.SearchFilter>();
    public SandboxToolParameterMenu.SelectorValue.SearchFilter currentFilter;
    public string labelText;

    public SelectorValue(
      object[] options,
      Action<object> onValueChanged,
      Func<object, string> getOptionName,
      Func<string, object, bool> filterOptionFunction,
      Func<object, Tuple<Sprite, Color>> getOptionSprite,
      string labelText,
      SandboxToolParameterMenu.SelectorValue.SearchFilter[] filters = null)
    {
      this.options = options;
      this.onValueChanged = onValueChanged;
      this.getOptionName = getOptionName;
      this.filterOptionFunction = filterOptionFunction;
      this.getOptionSprite = getOptionSprite;
      this.filters = filters;
      this.labelText = labelText;
    }

    public bool runCurrentFilter(object obj)
    {
      return this.currentFilter == null || this.currentFilter.condition(obj);
    }

    public class SearchFilter
    {
      public string Name;
      public Func<object, bool> condition;
      public SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter;
      public Tuple<Sprite, Color> icon;

      public SearchFilter(
        string Name,
        Func<object, bool> condition,
        SandboxToolParameterMenu.SelectorValue.SearchFilter parentFilter = null,
        Tuple<Sprite, Color> icon = null)
      {
        this.Name = Name;
        this.condition = condition;
        this.parentFilter = parentFilter;
        this.icon = icon;
      }
    }
  }

  public class SliderValue
  {
    public GameObject row;
    public string bottomSprite;
    public string topSprite;
    public float slideMinValue;
    public float slideMaxValue;
    public float clampValueLow;
    public float clampValueHigh;
    public string unitString;
    public Action<float> onValueChanged;
    public string tooltip;
    public int roundToDecimalPlaces;
    public string labelText;
    public KSlider slider;
    public KNumberInputField inputField;

    public SliderValue(
      float slideMinValue,
      float slideMaxValue,
      string bottomSprite,
      string topSprite,
      string unitString,
      string tooltip,
      string labelText,
      Action<float> onValueChanged,
      int decimalPlaces = 0)
    {
      this.slideMinValue = slideMinValue;
      this.slideMaxValue = slideMaxValue;
      this.bottomSprite = bottomSprite;
      this.topSprite = topSprite;
      this.unitString = unitString;
      this.onValueChanged = onValueChanged;
      this.tooltip = tooltip;
      this.roundToDecimalPlaces = decimalPlaces;
      this.labelText = labelText;
      this.clampValueLow = slideMinValue;
      this.clampValueHigh = slideMaxValue;
    }

    public void SetRange(float min, float max, bool resetCurrentValue = true)
    {
      this.slideMinValue = min;
      this.slideMaxValue = max;
      this.slider.minValue = this.slideMinValue;
      this.slider.maxValue = this.slideMaxValue;
      this.inputField.currentValue = this.slideMinValue + (float) (((double) this.slideMaxValue - (double) this.slideMinValue) / 2.0);
      this.inputField.SetDisplayValue(this.inputField.currentValue.ToString());
      if (!resetCurrentValue)
        return;
      this.slider.value = this.slideMinValue + (float) (((double) this.slideMaxValue - (double) this.slideMinValue) / 2.0);
      this.onValueChanged(this.slideMinValue + (float) (((double) this.slideMaxValue - (double) this.slideMinValue) / 2.0));
    }

    public void SetValue(float value, bool runOnValueChanged = true)
    {
      value = Mathf.Clamp(value, this.clampValueLow, this.clampValueHigh);
      this.slider.value = value;
      this.inputField.currentValue = value;
      if (runOnValueChanged)
        this.onValueChanged(value);
      this.RefreshDisplay();
    }

    public void RefreshDisplay()
    {
      this.inputField.SetDisplayValue((this.roundToDecimalPlaces == 0 ? (float) Mathf.RoundToInt(this.inputField.currentValue) : this.inputField.currentValue).ToString());
    }
  }
}
