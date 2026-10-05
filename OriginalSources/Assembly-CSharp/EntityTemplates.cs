// Decompiled with JetBrains decompiler
// Type: EntityTemplates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class EntityTemplates
{
  private static GameObject selectableEntityTemplate;
  private static GameObject unselectableEntityTemplate;
  private static GameObject baseEntityTemplate;
  private static GameObject placedEntityTemplate;
  private static GameObject baseOreTemplate;

  public static void CreateTemplates()
  {
    EntityTemplates.unselectableEntityTemplate = new GameObject("unselectableEntityTemplate");
    EntityTemplates.unselectableEntityTemplate.SetActive(false);
    EntityTemplates.unselectableEntityTemplate.AddComponent<KPrefabID>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.unselectableEntityTemplate);
    EntityTemplates.selectableEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.unselectableEntityTemplate);
    EntityTemplates.selectableEntityTemplate.name = "selectableEntityTemplate";
    EntityTemplates.selectableEntityTemplate.AddComponent<KSelectable>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.selectableEntityTemplate);
    EntityTemplates.baseEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.selectableEntityTemplate);
    EntityTemplates.baseEntityTemplate.name = "baseEntityTemplate";
    EntityTemplates.baseEntityTemplate.AddComponent<KBatchedAnimController>();
    EntityTemplates.baseEntityTemplate.AddComponent<SaveLoadRoot>();
    EntityTemplates.baseEntityTemplate.AddComponent<StateMachineController>();
    EntityTemplates.baseEntityTemplate.AddComponent<PrimaryElement>();
    EntityTemplates.baseEntityTemplate.AddComponent<InfraredPrimaryElement>();
    EntityTemplates.baseEntityTemplate.AddComponent<SimTemperatureTransfer>();
    EntityTemplates.baseEntityTemplate.AddComponent<InfoDescription>();
    EntityTemplates.baseEntityTemplate.AddComponent<Notifier>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.baseEntityTemplate);
    EntityTemplates.placedEntityTemplate = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseEntityTemplate);
    EntityTemplates.placedEntityTemplate.name = "placedEntityTemplate";
    EntityTemplates.placedEntityTemplate.AddComponent<KBoxCollider2D>();
    EntityTemplates.placedEntityTemplate.AddComponent<OccupyArea>();
    EntityTemplates.placedEntityTemplate.AddComponent<Modifiers>();
    EntityTemplates.placedEntityTemplate.AddComponent<DecorProvider>();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.placedEntityTemplate);
  }

  private static void ConfigEntity(
    GameObject template,
    string id,
    string name,
    bool is_selectable = true)
  {
    template.name = id;
    template.AddOrGet<KPrefabID>().PrefabTag = TagManager.Create(id, name);
    if (!is_selectable)
      return;
    template.AddOrGet<KSelectable>().SetName(name);
  }

  public static GameObject CreateEntity(string id, string name, bool is_selectable = true)
  {
    GameObject entity = !is_selectable ? UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.unselectableEntityTemplate) : UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.selectableEntityTemplate);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) entity);
    EntityTemplates.ConfigEntity(entity, id, name, is_selectable);
    return entity;
  }

  public static GameObject ConfigBasicEntity(
    GameObject template,
    string id,
    string name,
    string desc,
    float mass,
    bool unitMass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    EntityTemplates.ConfigEntity(template, id, name);
    KPrefabID kprefabId = template.AddOrGet<KPrefabID>();
    if (additionalTags != null)
    {
      foreach (Tag additionalTag in additionalTags)
        kprefabId.AddTag(additionalTag);
    }
    KBatchedAnimController kbatchedAnimController = template.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      anim
    };
    kbatchedAnimController.sceneLayer = sceneLayer;
    kbatchedAnimController.initialAnim = initialAnim;
    template.AddOrGet<StateMachineController>();
    PrimaryElement primaryElement = template.AddOrGet<PrimaryElement>();
    primaryElement.ElementID = element;
    primaryElement.Temperature = defaultTemperature;
    if (unitMass)
    {
      primaryElement.MassPerUnit = mass;
      primaryElement.Units = 1f;
      GameTags.DisplayAsUnits.Add(kprefabId.PrefabTag);
    }
    else
      primaryElement.Mass = mass;
    template.AddOrGet<InfoDescription>().description = desc;
    template.AddOrGet<Notifier>();
    return template;
  }

  public static GameObject CreateBasicEntity(
    string id,
    string name,
    string desc,
    float mass,
    bool unitMass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    GameObject basicEntity = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseEntityTemplate);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) basicEntity);
    EntityTemplates.ConfigBasicEntity(basicEntity, id, name, desc, mass, unitMass, anim, initialAnim, sceneLayer, element, additionalTags, defaultTemperature);
    return basicEntity;
  }

  private static GameObject ConfigPlacedEntity(
    GameObject template,
    string id,
    string name,
    string desc,
    float mass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    int width,
    int height,
    EffectorValues decor,
    EffectorValues noise = default (EffectorValues),
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    if ((UnityEngine.Object) anim == (UnityEngine.Object) null)
      Debug.LogErrorFormat("Cant create [{0}] entity without an anim", (object) name);
    EntityTemplates.ConfigBasicEntity(template, id, name, desc, mass, true, anim, initialAnim, sceneLayer, element, additionalTags, defaultTemperature);
    KBoxCollider2D kboxCollider2D = template.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.size = (Vector2) new Vector2f(width, height);
    float num = 0.5f * (float) ((width + 1) % 2);
    kboxCollider2D.offset = (Vector2) new Vector2f(num, (float) height / 2f);
    template.GetComponent<KBatchedAnimController>().Offset = new Vector3(num, 0.0f, 0.0f);
    template.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(width, height));
    DecorProvider decorProvider = template.AddOrGet<DecorProvider>();
    decorProvider.SetValues(decor);
    decorProvider.overrideName = name;
    return template;
  }

  public static GameObject CreatePlacedEntity(
    string id,
    string name,
    string desc,
    float mass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    int width,
    int height,
    EffectorValues decor,
    EffectorValues noise = default (EffectorValues),
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    GameObject placedEntity = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.placedEntityTemplate);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) placedEntity);
    EntityTemplates.ConfigPlacedEntity(placedEntity, id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, noise, element, additionalTags, defaultTemperature);
    return placedEntity;
  }

  public static GameObject CreatePlacedEntity(
    string id,
    string name,
    string desc,
    float mass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    int width,
    int height,
    EffectorValues decor,
    PermittedRotations permittedRotation,
    Orientation orientation = Orientation.Neutral,
    EffectorValues noise = default (EffectorValues),
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null,
    float defaultTemperature = 293f)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, anim, initialAnim, sceneLayer, width, height, decor, noise, element, additionalTags, defaultTemperature);
    if (permittedRotation != PermittedRotations.Unrotatable)
    {
      Rotatable rotatable = placedEntity.AddOrGet<Rotatable>();
      rotatable.SetSize(width, height);
      rotatable.permittedRotations = permittedRotation;
      rotatable.SetOrientation(orientation);
    }
    return placedEntity;
  }

  public static GameObject MakeHangingOffsets(GameObject template, int width, int height)
  {
    KBoxCollider2D component1 = template.GetComponent<KBoxCollider2D>();
    if ((bool) (UnityEngine.Object) component1)
    {
      component1.size = (Vector2) new Vector2f(width, height);
      float a = 0.5f * (float) ((width + 1) % 2);
      component1.offset = (Vector2) new Vector2f(a, (float) ((double) -height / 2.0 + 1.0));
    }
    OccupyArea component2 = template.GetComponent<OccupyArea>();
    if ((bool) (UnityEngine.Object) component2)
      component2.SetCellOffsets(EntityTemplates.GenerateHangingOffsets(width, height));
    return template;
  }

  private GameObject ExtendEntityToBasicPlant(
    GameObject template,
    float temperature_lethal_low = 218.15f,
    float temperature_warning_low = 283.15f,
    float temperature_warning_high = 303.15f,
    float temperature_lethal_high = 398.15f,
    SimHashes[] safe_elements = null,
    bool pressure_sensitive = true,
    float pressure_lethal_low = 0.0f,
    float pressure_warning_low = 0.15f,
    string crop_id = null,
    bool can_drown = true,
    bool can_tinker = true,
    bool require_solid_tile = true,
    bool should_grow_old = true,
    float max_age = 2400f,
    float min_radiation = 0.0f,
    float max_radiation = 2200f,
    string baseTraitId = null,
    string baseTraitName = null)
  {
    return EntityTemplates.ExtendEntityToBasicPlant(template, temperature_lethal_low, temperature_warning_low, temperature_warning_high, temperature_lethal_high, safe_elements, pressure_sensitive, pressure_lethal_low, pressure_warning_low, crop_id, can_drown, can_tinker, require_solid_tile, false, should_grow_old, max_age, min_radiation, max_radiation, baseTraitId, baseTraitName);
  }

  public static GameObject ExtendEntityToBasicPlant(
    GameObject template,
    float temperature_lethal_low = 218.15f,
    float temperature_warning_low = 283.15f,
    float temperature_warning_high = 303.15f,
    float temperature_lethal_high = 398.15f,
    SimHashes[] safe_elements = null,
    bool pressure_sensitive = true,
    float pressure_lethal_low = 0.0f,
    float pressure_warning_low = 0.15f,
    string crop_id = null,
    bool can_drown = true,
    bool can_tinker = true,
    bool require_solid_tile = true,
    bool require_Backwall_Foundation = false,
    bool should_grow_old = true,
    float max_age = 2400f,
    float min_radiation = 0.0f,
    float max_radiation = 2200f,
    string baseTraitId = null,
    string baseTraitName = null)
  {
    Modifiers component1 = template.GetComponent<Modifiers>();
    Trait trait = Db.Get().CreateTrait(baseTraitId, baseTraitName, baseTraitName, (string) null, false, (ChoreGroup[]) null, true, true);
    template.AddTag(GameTags.Plant);
    template.AddOrGet<EntombVulnerable>();
    PressureVulnerable pressureVulnerable = template.AddOrGet<PressureVulnerable>();
    if (pressure_sensitive)
      pressureVulnerable.Configure(pressure_warning_low, pressure_lethal_low, safeAtmospheres: safe_elements);
    else
      pressureVulnerable.Configure(safe_elements);
    template.AddOrGet<WiltCondition>();
    template.AddOrGet<Prioritizable>();
    template.AddOrGet<Uprootable>();
    template.AddOrGet<Effects>();
    template.AddOrGetDef<PollinationVFXMonitor.Def>();
    if (require_solid_tile)
      template.AddOrGet<UprootedMonitor>();
    if (require_Backwall_Foundation)
      EntityTemplates.ExtendPlantEntityToRequireBackwall(template);
    template.AddOrGet<ReceptacleMonitor>();
    template.AddOrGet<Notifier>();
    if (can_drown)
      template.AddOrGet<DrowningMonitor>();
    template.AddOrGet<KAnimControllerBase>().randomiseLoopedOffset = true;
    component1.initialAttributes.Add(Db.Get().PlantAttributes.WiltTempRangeMod.Id);
    template.AddOrGet<TemperatureVulnerable>().Configure(temperature_warning_low, temperature_lethal_low, temperature_warning_high, temperature_lethal_high);
    if (DlcManager.FeaturePlantMutationsEnabled())
    {
      component1.initialAttributes.Add(Db.Get().PlantAttributes.MinRadiationThreshold.Id);
      if ((double) min_radiation != 0.0)
        trait.Add(new AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold.Id, min_radiation, baseTraitName));
      component1.initialAttributes.Add(Db.Get().PlantAttributes.MaxRadiationThreshold.Id);
      trait.Add(new AttributeModifier(Db.Get().PlantAttributes.MaxRadiationThreshold.Id, max_radiation, baseTraitName));
      template.AddOrGetDef<RadiationVulnerable.Def>();
    }
    template.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    KPrefabID component2 = template.GetComponent<KPrefabID>();
    if (crop_id != null)
    {
      GeneratedBuildings.RegisterWithOverlay(OverlayScreen.HarvestableIDs, component2.PrefabID().ToString());
      Crop.CropVal cropval = TUNING.CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == crop_id));
      Debug.Assert(baseTraitId != null && baseTraitName != null, (object) $"Extending {template.name} to a crop plant failed because the base trait wasn't specified.");
      component1.initialAttributes.Add(Db.Get().PlantAttributes.YieldAmount.Id);
      component1.initialAmounts.Add(Db.Get().Amounts.Maturity.Id);
      trait.Add(new AttributeModifier(Db.Get().PlantAttributes.YieldAmount.Id, (float) cropval.numProduced, baseTraitName));
      trait.Add(new AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute.Id, cropval.cropDuration / 600f, baseTraitName));
      if (DlcManager.FeaturePlantMutationsEnabled())
      {
        template.AddOrGet<MutantPlant>().SpeciesID = component2.PrefabTag;
        SymbolOverrideControllerUtil.AddToPrefab(template);
      }
      template.AddOrGet<Crop>().Configure(cropval);
      Growing growing = template.AddOrGet<Growing>();
      growing.shouldGrowOld = should_grow_old;
      growing.maxAge = max_age;
      template.AddOrGet<Harvestable>();
      template.AddOrGet<HarvestDesignatable>();
    }
    if (trait.SelfModifiers != null && trait.SelfModifiers.Count > 0)
    {
      template.AddOrGet<Traits>();
      component1.initialTraits.Add(baseTraitId);
    }
    component2.prefabInitFn += (KPrefabID.PrefabFn) (inst =>
    {
      PressureVulnerable component3 = inst.GetComponent<PressureVulnerable>();
      if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null) || safe_elements == null)
        return;
      foreach (SimHashes safeElement in safe_elements)
        component3.safe_atmospheres.Add(ElementLoader.FindElementByHash(safeElement));
    });
    if (can_tinker)
      Tinkerable.MakeFarmTinkerable(template);
    return template;
  }

  public static void ExtendPlantEntityToRequireBackwall(GameObject plantGameObject)
  {
    UprootedMonitor uprootedMonitor = plantGameObject.AddOrGet<UprootedMonitor>();
    uprootedMonitor.customFoundationCheckFn = new Func<int, bool>(BackwallManager.HasBackwall);
    uprootedMonitor.customScenePartitionerLayerFn = new Func<ScenePartitionerLayer>(EntityTemplates.UprootBackwallScenePartitionerLayer);
    uprootedMonitor.monitorCells = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
  }

  private static ScenePartitionerLayer UprootBackwallScenePartitionerLayer()
  {
    return GameScenePartitioner.Instance.backwallChangedLayer;
  }

  public static GameObject ExtendEntityToWildCreature(
    GameObject prefab,
    int space_required_per_creature)
  {
    return EntityTemplates.ExtendEntityToWildCreature(prefab, space_required_per_creature, true);
  }

  public static GameObject ExtendEntityToWildCreature(
    GameObject prefab,
    int space_required_per_creature,
    bool add_fixed_capturable_monitor)
  {
    prefab.AddOrGetDef<AgeMonitor.Def>();
    prefab.AddOrGetDef<HappinessMonitor.Def>();
    Tag prefabTag = prefab.GetComponent<KPrefabID>().PrefabTag;
    WildnessMonitor.Def def = prefab.AddOrGetDef<WildnessMonitor.Def>();
    def.wildEffect = new Effect("Wild" + prefabTag.Name, (string) STRINGS.CREATURES.MODIFIERS.WILD.NAME, (string) STRINGS.CREATURES.MODIFIERS.WILD.TOOLTIP, 0.0f, true, true, false);
    def.wildEffect.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, 0.008333334f, (string) STRINGS.CREATURES.MODIFIERS.WILD.NAME));
    def.wildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -75f, (string) STRINGS.CREATURES.MODIFIERS.WILD.NAME));
    def.wildEffect.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, -0.75f, (string) STRINGS.CREATURES.MODIFIERS.WILD.NAME, true));
    def.tameEffect = new Effect("Tame" + prefabTag.Name, (string) STRINGS.CREATURES.MODIFIERS.TAME.NAME, (string) STRINGS.CREATURES.MODIFIERS.TAME.TOOLTIP, 0.0f, true, true, false);
    def.tameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, (string) STRINGS.CREATURES.MODIFIERS.TAME.NAME));
    if (space_required_per_creature != 0)
      prefab.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = space_required_per_creature;
    else
      prefab.RemoveDef<OvercrowdingMonitor.Def>();
    if (add_fixed_capturable_monitor)
      prefab.AddOrGetDef<FixedCapturableMonitor.Def>();
    return prefab;
  }

  public static GameObject ExtendEntityToFertileCreature(
    GameObject prefab,
    IHasDlcRestrictions dlcRestrictions,
    string eggId,
    string eggName,
    string eggDesc,
    string eggAnim,
    float eggMass,
    string babyId,
    float fertilityCycles,
    float incubationCycles,
    List<FertilityMonitor.BreedingChance> eggChances,
    int eggSortOrder = -1,
    bool is_ranchable = true,
    bool add_fish_overcrowding_monitor = false,
    float egg_anim_scale = 1f,
    bool deprecated = false)
  {
    return EntityTemplates.ExtendEntityToFertileCreature(prefab, dlcRestrictions, eggId, eggName, eggDesc, eggAnim, eggMass, babyId, fertilityCycles, incubationCycles, eggChances, eggSortOrder, is_ranchable, add_fish_overcrowding_monitor, egg_anim_scale, deprecated, false, eggMass);
  }

  public static GameObject ExtendEntityToFertileCreature(
    GameObject prefab,
    IHasDlcRestrictions dlcRestrictions,
    string eggId,
    string eggName,
    string eggDesc,
    string eggAnim,
    float eggMass,
    string babyId,
    float fertilityCycles,
    float incubationCycles,
    List<FertilityMonitor.BreedingChance> eggChances,
    int eggSortOrder,
    bool is_ranchable,
    bool add_fish_overcrowding_monitor,
    float egg_anim_scale,
    bool deprecated,
    bool preventEggFromDroppingProducts)
  {
    return EntityTemplates.ExtendEntityToFertileCreature(prefab, dlcRestrictions, eggId, eggName, eggDesc, eggAnim, eggMass, babyId, fertilityCycles, incubationCycles, eggChances, eggSortOrder, is_ranchable, add_fish_overcrowding_monitor, egg_anim_scale, deprecated, preventEggFromDroppingProducts, eggMass);
  }

  public static GameObject ExtendEntityToFertileCreature(
    GameObject prefab,
    IHasDlcRestrictions dlcRestrictions,
    string eggId,
    string eggName,
    string eggDesc,
    string eggAnim,
    float eggMass,
    string babyId,
    float fertilityCycles,
    float incubationCycles,
    List<FertilityMonitor.BreedingChance> eggChances,
    int eggSortOrder,
    bool is_ranchable,
    bool add_fish_overcrowding_monitor,
    float egg_anim_scale,
    bool deprecated,
    bool preventEggFromDroppingProducts,
    float eggMassToDrop)
  {
    return EntityTemplates.ExtendEntityToFertileCreature(prefab, dlcRestrictions, eggId, eggName, eggDesc, eggAnim, eggMass, 0.5f, babyId, fertilityCycles, incubationCycles, eggChances, eggSortOrder, is_ranchable, add_fish_overcrowding_monitor, egg_anim_scale, deprecated, preventEggFromDroppingProducts, eggMassToDrop);
  }

  public static GameObject ExtendEntityToFertileCreature(
    GameObject prefab,
    IHasDlcRestrictions dlcRestrictions,
    string eggId,
    string eggName,
    string eggDesc,
    string eggAnim,
    float eggMass,
    float eggShellRatio,
    string babyId,
    float fertilityCycles,
    float incubationCycles,
    List<FertilityMonitor.BreedingChance> eggChances,
    int eggSortOrder,
    bool is_ranchable,
    bool add_fish_overcrowding_monitor,
    float egg_anim_scale,
    bool deprecated,
    bool preventEggFromDroppingProducts,
    float eggMassToDrop,
    bool allowEggCrackerRecipeCreation = true)
  {
    FertilityMonitor.Def def = prefab.AddOrGetDef<FertilityMonitor.Def>();
    def.baseFertileCycles = fertilityCycles;
    DebugUtil.DevAssert(eggSortOrder > -1, "Added a fertile creature without an egg sort order!");
    float base_incubation_rate = (float) (100.0 / (600.0 * (double) incubationCycles));
    string[] requiredDlcsOrNull = DlcRestrictionsUtil.GetRequiredDlcsOrNull(dlcRestrictions);
    string[] forbiddenDlcIdsOrNull = DlcRestrictionsUtil.GetForbiddenDlcIdsOrNull(dlcRestrictions);
    GameObject egg = EggConfig.CreateEgg(eggId, eggName, eggDesc, (Tag) babyId, eggAnim, eggMass, eggSortOrder, base_incubation_rate, requiredDlcsOrNull, forbiddenDlcIdsOrNull, preventEggFromDroppingProducts, eggMassToDrop, eggShellRatio, allowEggCrackerRecipeCreation);
    def.eggPrefab = new Tag(eggId);
    def.initialBreedingWeights = eggChances;
    if ((double) egg_anim_scale != 1.0)
    {
      KBatchedAnimController component = egg.GetComponent<KBatchedAnimController>();
      component.animWidth = egg_anim_scale;
      component.animHeight = egg_anim_scale;
    }
    KPrefabID egg_prefab_id = egg.GetComponent<KPrefabID>();
    SymbolOverrideController prefab1 = SymbolOverrideControllerUtil.AddToPrefab(egg);
    string symbolPrefix = prefab.GetComponent<CreatureBrain>().symbolPrefix;
    if (!string.IsNullOrEmpty(symbolPrefix))
      prefab1.ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) eggAnim), symbolPrefix);
    KPrefabID creature_prefab_id = prefab.GetComponent<KPrefabID>();
    creature_prefab_id.prefabSpawnFn += (KPrefabID.PrefabFn) (inst =>
    {
      DiscoveredResources.Instance.Discover(eggId.ToTag(), DiscoveredResources.GetCategoryForTags(egg_prefab_id.Tags));
      DiscoveredResources.Instance.Discover(babyId.ToTag(), DiscoveredResources.GetCategoryForTags(creature_prefab_id.Tags));
    });
    if (is_ranchable)
      prefab.AddOrGetDef<RanchableMonitor.Def>();
    if (add_fish_overcrowding_monitor)
      egg.AddOrGetDef<FishOvercrowdingMonitor.Def>();
    if (deprecated)
    {
      egg.AddTag(GameTags.DeprecatedContent);
      prefab.AddTag(GameTags.DeprecatedContent);
    }
    return prefab;
  }

  [Obsolete("Mod compatibility: use ExtendEntityToFertileCreature with IHasDlcRestrictions")]
  public static GameObject ExtendEntityToFertileCreature(
    GameObject prefab,
    string eggId,
    string eggName,
    string eggDesc,
    string egg_anim,
    float egg_mass,
    string baby_id,
    float fertility_cycles,
    float incubation_cycles,
    List<FertilityMonitor.BreedingChance> egg_chances,
    string[] dlcIds,
    int eggSortOrder = -1,
    bool is_ranchable = true,
    bool add_fish_overcrowding_monitor = false,
    bool add_fixed_capturable_monitor = true,
    float egg_anim_scale = 1f,
    bool deprecated = false)
  {
    string[] requiredDlcIds;
    string[] forbiddenDlcIds;
    DlcManager.ConvertAvailableToRequireAndForbidden(dlcIds, out requiredDlcIds, out forbiddenDlcIds);
    return EntityTemplates.ExtendEntityToFertileCreature(prefab, (IHasDlcRestrictions) DlcRestrictionsUtil.GetTransientHelperObject(requiredDlcIds, forbiddenDlcIds), eggId, eggName, eggDesc, egg_anim, egg_mass, baby_id, fertility_cycles, incubation_cycles, egg_chances, eggSortOrder, is_ranchable, add_fish_overcrowding_monitor, egg_anim_scale, deprecated);
  }

  [Obsolete("Mod compatibility: use ExtendEntityToFertileCreature with IHasDlcRestrictions")]
  public static GameObject ExtendEntityToFertileCreature(
    GameObject prefab,
    string eggId,
    string eggName,
    string eggDesc,
    string egg_anim,
    float egg_mass,
    string baby_id,
    float fertility_cycles,
    float incubation_cycles,
    List<FertilityMonitor.BreedingChance> egg_chances,
    int eggSortOrder = -1,
    bool is_ranchable = true,
    bool add_fish_overcrowding_monitor = false,
    bool add_fixed_capturable_monitor = true,
    float egg_anim_scale = 1f,
    bool deprecated = false)
  {
    return EntityTemplates.ExtendEntityToFertileCreature(prefab, (IHasDlcRestrictions) null, eggId, eggName, eggDesc, egg_anim, egg_mass, baby_id, fertility_cycles, incubation_cycles, egg_chances, eggSortOrder, is_ranchable, add_fish_overcrowding_monitor, egg_anim_scale, deprecated);
  }

  public static GameObject ExtendEntityToBeingABaby(
    GameObject prefab,
    Tag adult_prefab_id,
    string on_grow_item_drop_id = null,
    bool force_adult_nav_type = false,
    float adult_threshold = 5f)
  {
    prefab.RemoveDef<FertilityMonitor.Def>();
    prefab.AddOrGetDef<BabyMonitor.Def>().adultPrefab = adult_prefab_id;
    prefab.AddOrGetDef<BabyMonitor.Def>().onGrowDropID = on_grow_item_drop_id;
    prefab.AddOrGetDef<BabyMonitor.Def>().forceAdultNavType = force_adult_nav_type;
    prefab.AddOrGetDef<BabyMonitor.Def>().adultThreshold = adult_threshold;
    prefab.AddOrGetDef<IncubatorMonitor.Def>();
    prefab.AddOrGetDef<CreatureSleepMonitor.Def>();
    prefab.AddOrGetDef<CallAdultMonitor.Def>();
    prefab.AddOrGetDef<AgeMonitor.Def>().maxAgePercentOnSpawn = 0.01f;
    prefab.AddOrGet<Pickupable>().sortOrder = Assets.GetPrefab(adult_prefab_id).GetComponent<Pickupable>().sortOrder + 1;
    return prefab;
  }

  public static GameObject ExtendEntityToBasicCreature(
    GameObject template,
    FactionManager.FactionID faction = FactionManager.FactionID.Prey,
    string initialTraitID = null,
    string NavGridName = "WalkerNavGrid1x1",
    NavType navType = NavType.Floor,
    int max_probing_radius = 32 /*0x20*/,
    float moveSpeed = 2f,
    string onDeathDropID = "Meat",
    float onDeathDropCount = 1f,
    bool drownVulnerable = true,
    bool entombVulnerable = true,
    float warningLowTemperature = 283.15f,
    float warningHighTemperature = 293.15f,
    float lethalLowTemperature = 243.15f,
    float lethalHighTemperature = 343.15f)
  {
    return EntityTemplates.ExtendEntityToBasicCreature(false, template, faction, initialTraitID, NavGridName, navType, max_probing_radius, moveSpeed, onDeathDropID, onDeathDropCount, drownVulnerable, entombVulnerable, warningLowTemperature, warningHighTemperature, lethalLowTemperature, lethalHighTemperature);
  }

  public static GameObject ExtendEntityToBasicCreature(
    bool isWarmBlooded,
    GameObject template,
    FactionManager.FactionID faction = FactionManager.FactionID.Prey,
    string initialTraitID = null,
    string NavGridName = "WalkerNavGrid1x1",
    NavType navType = NavType.Floor,
    int max_probing_radius = 32 /*0x20*/,
    float moveSpeed = 2f,
    string onDeathDropID = "Meat",
    float onDeathDropCount = 1f,
    bool drownVulnerable = true,
    bool entombVulnerable = true,
    float warningLowTemperature = 283.15f,
    float warningHighTemperature = 293.15f,
    float lethalLowTemperature = 243.15f,
    float lethalHighTemperature = 343.15f)
  {
    return EntityTemplates.ExtendEntityToBasicCreature(isWarmBlooded, template, (string) null, faction: faction, initialTraitID: initialTraitID, NavGridName: NavGridName, navType: navType, max_probing_radius: max_probing_radius, moveSpeed: moveSpeed, onDeathDropID: onDeathDropID, onDeathDropCount: onDeathDropCount, drownVulnerable: drownVulnerable, entombVulnerable: entombVulnerable, warningLowTemperature: warningLowTemperature, warningHighTemperature: warningHighTemperature, lethalLowTemperature: lethalLowTemperature, lethalHighTemperature: lethalHighTemperature);
  }

  public static GameObject ExtendEntityToBasicCreature(
    bool isWarmBlooded,
    GameObject template,
    string anim_filename,
    string build_filename = null,
    string symbol_override_prefix = null,
    FactionManager.FactionID faction = FactionManager.FactionID.Prey,
    string initialTraitID = null,
    string NavGridName = "WalkerNavGrid1x1",
    NavType navType = NavType.Floor,
    int max_probing_radius = 32 /*0x20*/,
    float moveSpeed = 2f,
    string onDeathDropID = "Meat",
    float onDeathDropCount = 1f,
    bool drownVulnerable = true,
    bool entombVulnerable = true,
    float warningLowTemperature = 283.15f,
    float warningHighTemperature = 293.15f,
    float lethalLowTemperature = 243.15f,
    float lethalHighTemperature = 343.15f)
  {
    EntityTemplates.ExtendEntityToBasicCreatureData data = new EntityTemplates.ExtendEntityToBasicCreatureData();
    data.isWarmBlooded = isWarmBlooded;
    data.template = template;
    data.anim_filename = anim_filename;
    data.build_filename = build_filename;
    data.symbol_override_prefix = symbol_override_prefix;
    data.faction = faction;
    data.initialTraitID = initialTraitID;
    data.NavGridName = NavGridName;
    data.navType = navType;
    data.max_probing_radius = max_probing_radius;
    data.moveSpeed = moveSpeed;
    EntityTemplates.ExtendEntityToBasicCreatureData basicCreatureData = data;
    string[] strArray;
    if (!string.IsNullOrEmpty(onDeathDropID))
      strArray = new string[1]{ onDeathDropID };
    else
      strArray = (string[]) null;
    basicCreatureData.onDeathDropsID = strArray;
    data.onDeathDropsCount = new float[1]
    {
      onDeathDropCount
    };
    data.drownVulnerable = drownVulnerable;
    data.entombVulnerable = entombVulnerable;
    data.warningLowTemperature = warningLowTemperature;
    data.warningHighTemperature = warningHighTemperature;
    data.lethalLowTemperature = lethalLowTemperature;
    data.lethalHighTemperature = lethalHighTemperature;
    return EntityTemplates.ExtendEntityToBasicCreature(data);
  }

  public static GameObject ExtendEntityToBasicCreature(
    EntityTemplates.ExtendEntityToBasicCreatureData data)
  {
    GameObject template = data.template;
    List<KAnimFile> kanimFileList = new List<KAnimFile>();
    KAnimFile anim1 = data.anim_filename != null ? Assets.GetAnim((HashedString) data.anim_filename) : (KAnimFile) null;
    KAnimFile anim2 = data.build_filename != null ? Assets.GetAnim((HashedString) data.build_filename) : (KAnimFile) null;
    kanimFileList.Add(anim2);
    kanimFileList.Add(anim1);
    KBatchedAnimController component = template.GetComponent<KBatchedAnimController>();
    component.isMovable = true;
    if ((UnityEngine.Object) anim2 != (UnityEngine.Object) null)
      component.AnimFiles = kanimFileList.ToArray();
    template.AddOrGet<KPrefabID>().AddTag(GameTags.Creature);
    Modifiers modifiers = template.AddOrGet<Modifiers>();
    if (data.initialTraitID != null)
      modifiers.initialTraits.Add(data.initialTraitID);
    modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
    Pickupable pickupable = template.AddOrGet<Pickupable>();
    int num1 = -1;
    string name = template.PrefabID().Name;
    if (TUNING.CREATURES.SORTING.CRITTER_ORDER.ContainsKey(name))
      num1 = TUNING.CREATURES.SORTING.CRITTER_ORDER[name];
    int num2 = num1;
    pickupable.sortOrder = num2;
    template.AddOrGet<Clearable>().isClearable = false;
    template.AddOrGet<Traits>();
    template.AddOrGet<Health>().isCritter = true;
    template.AddOrGet<CharacterOverlay>();
    template.AddOrGet<RangedAttackable>();
    template.AddOrGet<FactionAlignment>().Alignment = data.faction;
    template.AddOrGet<Prioritizable>();
    template.AddOrGet<Effects>();
    template.AddOrGetDef<CritterEmoteMonitor.Def>();
    template.AddOrGetDef<CreatureDebugGoToMonitor.Def>();
    template.AddOrGetDef<DeathMonitor.Def>();
    template.AddOrGetDef<CreatureThoughtGraph.Def>();
    template.AddOrGetDef<AnimInterruptMonitor.Def>();
    template.AddOrGet<AnimEventHandler>();
    SymbolOverrideController prefab = SymbolOverrideControllerUtil.AddToPrefab(template);
    if (data.symbol_override_prefix != null && (UnityEngine.Object) anim1 != (UnityEngine.Object) null)
      prefab.ApplySymbolOverridesByAffix((UnityEngine.Object) anim2 == (UnityEngine.Object) null ? anim1 : anim2, data.symbol_override_prefix);
    CritterTemperatureMonitor.Def def = template.AddOrGetDef<CritterTemperatureMonitor.Def>();
    def.temperatureHotDeadly = data.lethalHighTemperature;
    def.temperatureHotUncomfortable = data.warningHighTemperature;
    def.temperatureColdDeadly = data.lethalLowTemperature;
    def.temperatureColdUncomfortable = data.warningLowTemperature;
    template.GetComponent<PrimaryElement>().Temperature = def.GetIdealTemperature();
    modifiers.initialAmounts.Add(Db.Get().Amounts.CritterTemperature.Id);
    if (data.isWarmBlooded)
    {
      string properName = template.GetProperName();
      template.UpdateComponentRequirement<SimTemperatureTransfer>(false);
      CreatureSimTemperatureTransfer temperatureTransfer = template.AddOrGet<CreatureSimTemperatureTransfer>();
      temperatureTransfer.temperatureAttributeName = "CritterTemperature";
      temperatureTransfer.SurfaceArea = 17.5f;
      temperatureTransfer.Thickness = 0.025f;
      temperatureTransfer.GroundTransferScale = 0.0f;
      temperatureTransfer.skinThickness = 0.025f;
      temperatureTransfer.skinThicknessAttributeModifierName = properName;
      WarmBlooded warmBlooded = template.AddOrGet<WarmBlooded>();
      warmBlooded.TemperatureAmountName = "CritterTemperature";
      warmBlooded.complexity = WarmBlooded.ComplexityType.SimpleHeatProduction;
      warmBlooded.IdealTemperature = def.GetIdealTemperature();
      warmBlooded.BaseGenerationKW = 10f;
      warmBlooded.BaseTemperatureModifierDescription = properName;
    }
    if (data.drownVulnerable)
      template.AddOrGet<DrowningMonitor>();
    if (data.entombVulnerable)
      template.AddOrGet<EntombVulnerable>();
    EntityTemplates.DeathDropFunction(template, data.onDeathDropsCount, data.onDeathDropsID);
    template.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => EntityTemplates.DeathDropFunction(inst, data.onDeathDropsCount, data.onDeathDropsID));
    Navigator navigator = template.AddOrGet<Navigator>();
    navigator.NavGridName = data.NavGridName;
    navigator.CurrentNavType = data.navType;
    navigator.defaultSpeed = data.moveSpeed;
    navigator.updateProber = true;
    navigator.maxProbeRadiusX = data.max_probing_radius;
    navigator.maxProbeRadiusY = data.max_probing_radius;
    navigator.sceneLayer = Grid.SceneLayer.Creatures;
    template.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "snapto_pivot", false));
    return template;
  }

  public static void AddSecondaryExcretion(
    GameObject template,
    SimHashes element,
    float kgPerKcalConsumed)
  {
    CaloriesConsumedElementProducer consumedElementProducer = template.AddComponent<CaloriesConsumedElementProducer>();
    consumedElementProducer.producedElement = element;
    consumedElementProducer.kgProducedPerKcalConsumed = kgPerKcalConsumed;
  }

  private static void DeathDropFunction(
    GameObject inst,
    float onDeathDropCount,
    string onDeathDropID)
  {
    EntityTemplates.DeathDropFunction(inst, new float[1]
    {
      onDeathDropCount
    }, new string[1]{ onDeathDropID });
  }

  private static void DeathDropFunction(
    GameObject inst,
    float[] onDeathDropCounts,
    string[] onDeathDropIDs)
  {
    if (onDeathDropIDs == null || onDeathDropCounts == null)
      return;
    Dictionary<string, float> drops = new Dictionary<string, float>();
    for (int index = 0; index < onDeathDropIDs.Length; ++index)
    {
      float num = index < onDeathDropCounts.Length ? onDeathDropCounts[index] : onDeathDropCounts[onDeathDropCounts.Length - 1];
      if ((double) num > 0.0 && !string.IsNullOrEmpty(onDeathDropIDs[index]))
        drops[onDeathDropIDs[index]] = num;
    }
    if (drops.Count <= 0)
      return;
    inst.AddOrGet<Butcherable>().SetDrops(drops);
  }

  public static void AddCreatureBrain(
    GameObject prefab,
    ChoreTable.Builder chore_table,
    Tag species,
    string symbol_prefix)
  {
    CreatureBrain creatureBrain = prefab.AddOrGet<CreatureBrain>();
    creatureBrain.species = species;
    creatureBrain.symbolPrefix = symbol_prefix;
    if (chore_table.HasChoreType(typeof (CritterCondoStates.Def)))
      prefab.AddOrGetDef<CritterCondoInteractMontior.Def>();
    DrinkMilkStates.Def def;
    if (chore_table.TryGetChoreDef<DrinkMilkStates.Def>(out def))
      prefab.AddOrGetDef<DrinkMilkMonitor.Def>().drinkCellOffsetGetFn = def.drinkCellOffsetGetFn;
    ChoreConsumer chore_consumer = prefab.AddOrGet<ChoreConsumer>();
    chore_consumer.choreTable = chore_table.CreateTable();
    KPrefabID kprefabId = prefab.AddOrGet<KPrefabID>();
    kprefabId.AddTag(GameTags.CreatureBrain);
    kprefabId.instantiateFn += (KPrefabID.PrefabFn) (go => go.GetComponent<ChoreConsumer>().choreTable = chore_consumer.choreTable);
    kprefabId.prefabSpawnFn += (KPrefabID.PrefabFn) (go => Game.BrainScheduler.PrioritizeBrain((Brain) go.GetComponent<CreatureBrain>()));
  }

  public static Tag GetBaggedCreatureTag(Tag tag) => TagManager.Create("Bagged" + tag.Name);

  public static Tag GetUnbaggedCreatureTag(Tag bagged_tag)
  {
    return TagManager.Create(bagged_tag.Name.Substring(6));
  }

  public static string GetBaggedCreatureID(string name) => "Bagged" + name;

  public static GameObject CreateAndRegisterBaggedCreature(
    GameObject creature,
    bool must_stand_on_top_for_pickup,
    bool allow_mark_for_capture,
    bool use_gun_for_pickup = false)
  {
    KPrefabID creature_prefab_id = creature.GetComponent<KPrefabID>();
    creature_prefab_id.AddTag(GameTags.BagableCreature);
    Baggable baggable = creature.AddOrGet<Baggable>();
    baggable.mustStandOntopOfTrapForPickup = must_stand_on_top_for_pickup;
    baggable.useGunForPickup = use_gun_for_pickup;
    creature.AddOrGet<Capturable>().allowCapture = allow_mark_for_capture;
    if (allow_mark_for_capture)
      creature.AddComponent<Movable>();
    creature_prefab_id.prefabSpawnFn += (KPrefabID.PrefabFn) (inst => DiscoveredResources.Instance.Discover(creature_prefab_id.PrefabTag, DiscoveredResources.GetCategoryForTags(creature_prefab_id.Tags)));
    return creature;
  }

  public static GameObject CreateLooseEntity(
    string id,
    string name,
    string desc,
    float mass,
    bool unitMass,
    KAnimFile anim,
    string initialAnim,
    Grid.SceneLayer sceneLayer,
    EntityTemplates.CollisionShape collisionShape,
    float width = 1f,
    float height = 1f,
    bool isPickupable = false,
    int sortOrder = 0,
    SimHashes element = SimHashes.Creature,
    List<Tag> additionalTags = null)
  {
    GameObject go = EntityTemplates.AddCollision(EntityTemplates.CreateBasicEntity(id, name, desc, mass, unitMass, anim, initialAnim, sceneLayer, element, additionalTags), collisionShape, width, height);
    go.GetComponent<KBatchedAnimController>().isMovable = true;
    go.AddOrGet<Modifiers>();
    if (isPickupable)
    {
      Pickupable pickupable = go.AddOrGet<Pickupable>();
      pickupable.SetWorkTime(5f);
      pickupable.sortOrder = sortOrder;
      go.AddOrGet<Movable>();
    }
    return go;
  }

  public static void CreateBaseOreTemplates()
  {
    EntityTemplates.baseOreTemplate = new GameObject("OreTemplate");
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) EntityTemplates.baseOreTemplate);
    EntityTemplates.baseOreTemplate.SetActive(false);
    EntityTemplates.baseOreTemplate.AddComponent<KPrefabID>();
    EntityTemplates.baseOreTemplate.AddComponent<PrimaryElement>();
    EntityTemplates.baseOreTemplate.AddComponent<InfraredPrimaryElement>();
    EntityTemplates.baseOreTemplate.AddComponent<Pickupable>();
    EntityTemplates.baseOreTemplate.AddComponent<KSelectable>();
    EntityTemplates.baseOreTemplate.AddComponent<SaveLoadRoot>();
    EntityTemplates.baseOreTemplate.AddComponent<StateMachineController>();
    EntityTemplates.baseOreTemplate.AddComponent<Clearable>();
    EntityTemplates.baseOreTemplate.AddComponent<Prioritizable>();
    EntityTemplates.baseOreTemplate.AddComponent<KBatchedAnimController>();
    EntityTemplates.baseOreTemplate.AddComponent<SimTemperatureTransfer>();
    EntityTemplates.baseOreTemplate.AddComponent<Modifiers>();
    EntityTemplates.baseOreTemplate.AddComponent<Movable>();
    EntityTemplates.baseOreTemplate.AddOrGet<OccupyArea>().SetCellOffsets(new CellOffset[1]);
    DecorProvider decorProvider = EntityTemplates.baseOreTemplate.AddOrGet<DecorProvider>();
    decorProvider.baseDecor = -10f;
    decorProvider.baseRadius = 1f;
    EntityTemplates.baseOreTemplate.AddOrGet<ElementChunk>();
  }

  public static void DestroyBaseOreTemplates()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) EntityTemplates.baseOreTemplate);
    EntityTemplates.baseOreTemplate = (GameObject) null;
  }

  public static GameObject CreateOreEntity(
    SimHashes elementID,
    EntityTemplates.CollisionShape shape,
    float width,
    float height,
    List<Tag> additionalTags = null,
    float default_temperature = 293f)
  {
    Element elementByHash = ElementLoader.FindElementByHash(elementID);
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EntityTemplates.baseOreTemplate);
    gameObject.name = elementByHash.name;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) gameObject);
    KPrefabID kprefabId = gameObject.AddOrGet<KPrefabID>();
    kprefabId.PrefabTag = elementByHash.tag;
    kprefabId.InitializeTags();
    if (additionalTags != null)
    {
      foreach (Tag additionalTag in additionalTags)
        kprefabId.AddTag(additionalTag);
    }
    if ((double) elementByHash.lowTemp < 296.14999389648438 && (double) elementByHash.highTemp > 296.14999389648438)
      kprefabId.AddTag(GameTags.PedestalDisplayable);
    PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
    primaryElement.SetElement(elementID);
    primaryElement.Mass = 1f;
    primaryElement.Temperature = default_temperature;
    Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
    pickupable.SetWorkTime(5f);
    pickupable.sortOrder = elementByHash.buildMenuSort;
    gameObject.AddOrGet<KSelectable>().SetName(elementByHash.name);
    KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      elementByHash.substance.anim
    };
    kbatchedAnimController.sceneLayer = Grid.SceneLayer.Front;
    kbatchedAnimController.initialAnim = "idle1";
    kbatchedAnimController.isMovable = true;
    return EntityTemplates.AddCollision(gameObject, shape, width, height);
  }

  public static GameObject CreateSolidOreEntity(SimHashes elementId, List<Tag> additionalTags = null)
  {
    return EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.CIRCLE, 0.5f, 0.5f, additionalTags);
  }

  public static GameObject CreateLiquidOreEntity(SimHashes elementId, List<Tag> additionalTags = null)
  {
    GameObject oreEntity = EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.6f, additionalTags);
    oreEntity.AddOrGet<Dumpable>().SetWorkTime(5f);
    oreEntity.AddOrGet<SubstanceChunk>();
    return oreEntity;
  }

  public static GameObject CreateGasOreEntity(SimHashes elementId, List<Tag> additionalTags = null)
  {
    GameObject oreEntity = EntityTemplates.CreateOreEntity(elementId, EntityTemplates.CollisionShape.RECTANGLE, 0.5f, 0.6f, additionalTags);
    oreEntity.AddOrGet<Dumpable>().SetWorkTime(5f);
    oreEntity.AddOrGet<SubstanceChunk>();
    return oreEntity;
  }

  public static GameObject ExtendEntityToFood(GameObject template, EdiblesManager.FoodInfo foodInfo)
  {
    return EntityTemplates.ExtendEntityToFood(template, foodInfo, true);
  }

  public static GameObject ExtendEntityToFood(
    GameObject template,
    EdiblesManager.FoodInfo foodInfo,
    bool splittable)
  {
    if (splittable)
      template.AddOrGet<EntitySplitter>();
    if (foodInfo.CanRot)
    {
      Rottable.Def def = template.AddOrGetDef<Rottable.Def>();
      def.preserveTemperature = foodInfo.PreserveTemperature;
      def.rotTemperature = foodInfo.RotTemperature;
      def.spoilTime = foodInfo.SpoilTime;
      def.staleTime = foodInfo.StaleTime;
      EntityTemplates.CreateAndRegisterCompostableFromPrefab(template);
    }
    KPrefabID component = template.GetComponent<KPrefabID>();
    component.AddTag(GameTags.PedestalDisplayable);
    if ((double) foodInfo.CaloriesPerUnit > 0.0)
    {
      component.AddTag(GameTags.Edible);
      template.AddOrGet<Edible>().FoodInfo = foodInfo;
      component.instantiateFn += (KPrefabID.PrefabFn) (go => go.GetComponent<Edible>().FoodInfo = foodInfo);
      GameTags.DisplayAsCalories.Add(component.PrefabTag);
    }
    else
    {
      component.AddTag(GameTags.CookingIngredient);
      template.AddOrGet<HasSortOrder>();
    }
    return template;
  }

  public static GameObject ExtendEntityToDehydratedFoodPackage(
    GameObject template,
    EdiblesManager.FoodInfo foodInfo)
  {
    KPrefabID component = template.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Dehydrated);
    component.AddTag(GameTags.PickupableStorage);
    Storage storage = template.AddComponent<Storage>();
    storage.allowItemRemoval = false;
    storage.capacityKg = 1f;
    storage.showInUI = false;
    storage.storageFilters = new List<Tag>()
    {
      (Tag) foodInfo.Id
    };
    DehydratedFoodPackage dehydratedFoodPackage = template.AddOrGet<DehydratedFoodPackage>();
    dehydratedFoodPackage.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_rehydrator_kanim")
    };
    dehydratedFoodPackage.workTime = 5f;
    dehydratedFoodPackage.workLayer = Grid.SceneLayer.Front;
    dehydratedFoodPackage.FoodTag = (Tag) foodInfo.Id;
    return template;
  }

  public static GameObject ExtendEntityToMedicine(GameObject template, MedicineInfo medicineInfo)
  {
    template.AddOrGet<EntitySplitter>();
    KPrefabID component = template.GetComponent<KPrefabID>();
    Debug.Assert(component.PrefabID() == (Tag) medicineInfo.id, (object) "Tried assigning a medicine info to a non-matching prefab!");
    MedicinalPill medicinalPill = template.AddOrGet<MedicinalPill>();
    medicinalPill.info = medicineInfo;
    if (medicineInfo.doctorStationId == null)
    {
      template.AddOrGet<MedicinalPillWorkable>().pill = medicinalPill;
      component.AddTag(GameTags.Medicine);
    }
    else
    {
      component.AddTag(GameTags.MedicalSupplies);
      component.AddTag(medicineInfo.GetSupplyTag());
    }
    return template;
  }

  public static GameObject ExtendPlantToFertilizable(
    GameObject template,
    PlantElementAbsorber.ConsumeInfo[] fertilizers)
  {
    template.GetComponent<Modifiers>().initialAttributes.Add(Db.Get().PlantAttributes.FertilizerUsageMod.Id);
    HashedString idHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
    foreach (PlantElementAbsorber.ConsumeInfo fertilizer in fertilizers)
    {
      ManualDeliveryKG manualDeliveryKg = template.AddComponent<ManualDeliveryKG>();
      manualDeliveryKg.RequestedItemTag = fertilizer.tag;
      manualDeliveryKg.capacity = (float) ((double) fertilizer.massConsumptionRate * 600.0 * 3.0);
      manualDeliveryKg.refillMass = (float) ((double) fertilizer.massConsumptionRate * 600.0 * 0.5);
      manualDeliveryKg.MinimumMass = (float) ((double) fertilizer.massConsumptionRate * 600.0 * 0.5);
      manualDeliveryKg.operationalRequirement = Operational.State.Functional;
      manualDeliveryKg.choreTypeIDHash = idHash;
    }
    KPrefabID component1 = template.GetComponent<KPrefabID>();
    template.AddOrGetDef<FertilizationMonitor.Def>().consumedElements = fertilizers;
    component1.prefabInitFn += (KPrefabID.PrefabFn) (inst =>
    {
      foreach (ManualDeliveryKG component2 in inst.GetComponents<ManualDeliveryKG>())
        component2.Pause(true, "init");
    });
    return template;
  }

  public static GameObject ExtendPlantToIrrigated(
    GameObject template,
    PlantElementAbsorber.ConsumeInfo info)
  {
    return EntityTemplates.ExtendPlantToIrrigated(template, new PlantElementAbsorber.ConsumeInfo[1]
    {
      info
    });
  }

  public static GameObject ExtendPlantToIrrigated(
    GameObject template,
    PlantElementAbsorber.ConsumeInfo[] consume_info)
  {
    template.GetComponent<Modifiers>().initialAttributes.Add(Db.Get().PlantAttributes.FertilizerUsageMod.Id);
    HashedString idHash = Db.Get().ChoreTypes.FarmFetch.IdHash;
    foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in consume_info)
    {
      ManualDeliveryKG manualDeliveryKg = template.AddComponent<ManualDeliveryKG>();
      manualDeliveryKg.RequestedItemTag = consumeInfo.tag;
      manualDeliveryKg.capacity = (float) ((double) consumeInfo.massConsumptionRate * 600.0 * 3.0);
      manualDeliveryKg.refillMass = (float) ((double) consumeInfo.massConsumptionRate * 600.0 * 0.5);
      manualDeliveryKg.MinimumMass = (float) ((double) consumeInfo.massConsumptionRate * 600.0 * 0.5);
      manualDeliveryKg.operationalRequirement = Operational.State.Functional;
      manualDeliveryKg.choreTypeIDHash = idHash;
    }
    IrrigationMonitor.Def def = template.AddOrGetDef<IrrigationMonitor.Def>();
    def.wrongIrrigationTestTag = GameTags.Liquid;
    def.consumedElements = consume_info;
    return template;
  }

  public static GameObject CreateAndRegisterCompostableFromPrefab(GameObject original)
  {
    if ((UnityEngine.Object) original.GetComponent<Compostable>() != (UnityEngine.Object) null)
      return (GameObject) null;
    original.AddComponent<Compostable>().isMarkedForCompost = false;
    KPrefabID component = original.GetComponent<KPrefabID>();
    GameObject target = UnityEngine.Object.Instantiate<GameObject>(original);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
    string tag_string = "Compost" + component.PrefabTag.Name;
    string str = MISC.TAGS.COMPOST_FORMAT.Replace("{Item}", component.PrefabTag.ProperName());
    target.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(tag_string, str);
    target.GetComponent<KPrefabID>().AddTag(GameTags.Compostable);
    target.name = str;
    target.GetComponent<Compostable>().isMarkedForCompost = true;
    target.GetComponent<KSelectable>().SetName(str);
    target.GetComponent<Compostable>().originalPrefab = original;
    target.GetComponent<Compostable>().compostPrefab = target;
    original.GetComponent<Compostable>().originalPrefab = original;
    original.GetComponent<Compostable>().compostPrefab = target;
    Assets.AddPrefab(target.GetComponent<KPrefabID>());
    return target;
  }

  public static GameObject CreateAndRegisterSeedForPlantAsFood(
    GameObject plant,
    IHasDlcRestrictions dlcRestrictions,
    SeedProducer.ProductionType productionType,
    string id,
    string name,
    string desc,
    KAnimFile anim,
    EdiblesManager.FoodInfo foodInfo,
    string initialAnim = "object",
    int numberOfSeeds = 1,
    List<Tag> additionalTags = null,
    SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top,
    Tag replantGroundTag = default (Tag),
    int sortOrder = 0,
    string domesticatedDescription = "",
    EntityTemplates.CollisionShape collisionShape = EntityTemplates.CollisionShape.CIRCLE,
    float width = 0.25f,
    float height = 0.25f,
    Recipe.Ingredient[] recipe_ingredients = null,
    string recipe_description = "",
    bool ignoreDefaultSeedTag = false)
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(id, name, desc, 1f, true, anim, initialAnim, Grid.SceneLayer.Front, collisionShape, width, height, true, SORTORDER.SEEDS + sortOrder);
    looseEntity.AddOrGet<EntitySplitter>();
    if (foodInfo != null)
      EntityTemplates.ExtendEntityToFood(looseEntity, foodInfo);
    if (foodInfo == null || !foodInfo.CanRot)
      EntityTemplates.CreateAndRegisterCompostableFromPrefab(looseEntity);
    PlantableSeed plantableSeed = looseEntity.AddOrGet<PlantableSeed>();
    plantableSeed.PlantID = new Tag(plant.name);
    plantableSeed.replantGroundTag = replantGroundTag;
    plantableSeed.domesticatedDescription = domesticatedDescription;
    plantableSeed.direction = planterDirection;
    KPrefabID component1 = looseEntity.GetComponent<KPrefabID>();
    foreach (Tag additionalTag in additionalTags)
      component1.AddTag(additionalTag);
    component1.requiredDlcIds = DlcRestrictionsUtil.GetRequiredDlcsOrNull(dlcRestrictions);
    component1.forbiddenDlcIds = DlcRestrictionsUtil.GetForbiddenDlcIdsOrNull(dlcRestrictions);
    if (!ignoreDefaultSeedTag)
      component1.AddTag(GameTags.Seed);
    component1.AddTag(GameTags.PedestalDisplayable);
    MutantPlant component2;
    if (plant.TryGetComponent<MutantPlant>(out component2))
    {
      MutantPlant mutantPlant1 = looseEntity.AddOrGet<MutantPlant>();
      MutantPlant mutantPlant2 = looseEntity.GetComponent<Compostable>().compostPrefab.AddOrGet<MutantPlant>();
      mutantPlant1.SpeciesID = component2.SpeciesID;
      Tag speciesId = component2.SpeciesID;
      mutantPlant2.SpeciesID = speciesId;
    }
    Assets.AddPrefab(component1);
    plant.AddOrGet<SeedProducer>().Configure(id, productionType, numberOfSeeds);
    return looseEntity;
  }

  public static GameObject CreateAndRegisterSeedForPlant(
    GameObject plant,
    IHasDlcRestrictions dlcRestrictions,
    SeedProducer.ProductionType productionType,
    string id,
    string name,
    string desc,
    KAnimFile anim,
    string initialAnim = "object",
    int numberOfSeeds = 1,
    List<Tag> additionalTags = null,
    SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top,
    Tag replantGroundTag = default (Tag),
    int sortOrder = 0,
    string domesticatedDescription = "",
    EntityTemplates.CollisionShape collisionShape = EntityTemplates.CollisionShape.CIRCLE,
    float width = 0.25f,
    float height = 0.25f,
    Recipe.Ingredient[] recipe_ingredients = null,
    string recipe_description = "",
    bool ignoreDefaultSeedTag = false)
  {
    return EntityTemplates.CreateAndRegisterSeedForPlantAsFood(plant, dlcRestrictions, productionType, id, name, desc, anim, (EdiblesManager.FoodInfo) null, initialAnim, numberOfSeeds, additionalTags, planterDirection, replantGroundTag, sortOrder, domesticatedDescription, collisionShape, width, height, recipe_ingredients, recipe_description, ignoreDefaultSeedTag);
  }

  [Obsolete("Use version with IHasDlcRestrictions instead")]
  public static GameObject CreateAndRegisterSeedForPlant(
    GameObject plant,
    SeedProducer.ProductionType productionType,
    string id,
    string name,
    string desc,
    KAnimFile anim,
    string initialAnim = "object",
    int numberOfSeeds = 1,
    List<Tag> additionalTags = null,
    SingleEntityReceptacle.ReceptacleDirection planterDirection = SingleEntityReceptacle.ReceptacleDirection.Top,
    Tag replantGroundTag = default (Tag),
    int sortOrder = 0,
    string domesticatedDescription = "",
    EntityTemplates.CollisionShape collisionShape = EntityTemplates.CollisionShape.CIRCLE,
    float width = 0.25f,
    float height = 0.25f,
    Recipe.Ingredient[] recipe_ingredients = null,
    string recipe_description = "",
    bool ignoreDefaultSeedTag = false,
    string[] dlcIds = null)
  {
    return EntityTemplates.CreateAndRegisterSeedForPlant(plant, (IHasDlcRestrictions) DlcRestrictionsUtil.GetTransientHelperObjectFromAllowList(dlcIds), productionType, id, name, desc, anim, initialAnim, numberOfSeeds, additionalTags, planterDirection, replantGroundTag, sortOrder, domesticatedDescription, collisionShape, width, height, recipe_ingredients, recipe_description, ignoreDefaultSeedTag);
  }

  public static GameObject CreateAndRegisterPreview(
    string id,
    KAnimFile anim,
    string initial_anim,
    ObjectLayer object_layer,
    int width,
    int height)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, id, id, 1f, anim, initial_anim, Grid.SceneLayer.Front, width, height, TUNING.BUILDINGS.DECOR.NONE);
    placedEntity.UpdateComponentRequirement<KSelectable>(false);
    placedEntity.UpdateComponentRequirement<SaveLoadRoot>(false);
    placedEntity.AddOrGet<EntityPreview>().objectLayer = object_layer;
    OccupyArea occupyArea = placedEntity.AddOrGet<OccupyArea>();
    occupyArea.objectLayers = new ObjectLayer[1]
    {
      object_layer
    };
    occupyArea.ApplyToCells = false;
    placedEntity.AddOrGet<Storage>();
    Assets.AddPrefab(placedEntity.GetComponent<KPrefabID>());
    return placedEntity;
  }

  public static GameObject CreateAndRegisterPreviewForPlant(
    GameObject seed,
    string id,
    KAnimFile anim,
    string initialAnim,
    int width,
    int height)
  {
    GameObject andRegisterPreview = EntityTemplates.CreateAndRegisterPreview(id, anim, initialAnim, ObjectLayer.Building, width, height);
    seed.GetComponent<PlantableSeed>().PreviewID = TagManager.Create(id);
    return andRegisterPreview;
  }

  public static CellOffset[] GenerateOffsets(int width, int height)
  {
    int num1 = width / 2;
    int startX = num1 - width + 1;
    int num2 = 0;
    int num3 = height - 1;
    int startY = num2;
    int endX = num1;
    int endY = num3;
    return EntityTemplates.GenerateOffsets(startX, startY, endX, endY);
  }

  private static CellOffset[] GenerateOffsets(int startX, int startY, int endX, int endY)
  {
    List<CellOffset> cellOffsetList = new List<CellOffset>();
    for (int index1 = startY; index1 <= endY; ++index1)
    {
      for (int index2 = startX; index2 <= endX; ++index2)
        cellOffsetList.Add(new CellOffset()
        {
          x = index2,
          y = index1
        });
    }
    return cellOffsetList.ToArray();
  }

  public static CellOffset[] GenerateHangingOffsets(int width, int height)
  {
    int num1 = width / 2;
    int startX = num1 - width + 1;
    int num2 = -height + 1;
    int num3 = 0;
    int startY = num2;
    int endX = num1;
    int endY = num3;
    return EntityTemplates.GenerateOffsets(startX, startY, endX, endY);
  }

  public static GameObject AddCollision(
    GameObject template,
    EntityTemplates.CollisionShape shape,
    float width,
    float height)
  {
    switch (shape)
    {
      case EntityTemplates.CollisionShape.RECTANGLE:
        template.AddOrGet<KBoxCollider2D>().size = (Vector2) new Vector2f(width, height);
        break;
      case EntityTemplates.CollisionShape.POLYGONAL:
        template.AddOrGet<PolygonCollider2D>();
        break;
      default:
        template.AddOrGet<KCircleCollider2D>().radius = Mathf.Max(width, height);
        break;
    }
    return template;
  }

  public class ExtendEntityToBasicCreatureData
  {
    public bool isWarmBlooded;
    public GameObject template;
    public string anim_filename;
    public string build_filename;
    public string symbol_override_prefix;
    public FactionManager.FactionID faction = FactionManager.FactionID.Prey;
    public string initialTraitID;
    public string NavGridName = "WalkerNavGrid1x1";
    public NavType navType;
    public int max_probing_radius = 32 /*0x20*/;
    public float moveSpeed = 2f;
    public string[] onDeathDropsID = new string[1]{ "Meat" };
    public float[] onDeathDropsCount = new float[1]{ 1f };
    public bool drownVulnerable = true;
    public bool entombVulnerable = true;
    public float warningLowTemperature = 283.15f;
    public float warningHighTemperature = 293.15f;
    public float lethalLowTemperature = 243.15f;
    public float lethalHighTemperature = 343.15f;
  }

  public enum CollisionShape
  {
    CIRCLE,
    RECTANGLE,
    POLYGONAL,
  }
}
