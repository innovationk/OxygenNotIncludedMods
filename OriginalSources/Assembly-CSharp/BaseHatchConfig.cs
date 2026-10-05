// Decompiled with JetBrains decompiler
// Type: BaseHatchConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public static class BaseHatchConfig
{
  public const string EMOTION_FILE_NAME = "hatch_emotes_kanim";

  public static GameObject BaseHatch(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "hatch_build_kanim"));
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 100f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise);
    string NavGridName = "WalkerNavGrid1x1";
    if (is_baby)
      NavGridName = "WalkerBabyNavGrid";
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "hatch_build_kanim", symbolOverridePrefix, FactionManager.FactionID.Pest, traitId, NavGridName, onDeathDropCount: 2f, entombVulnerable: false, warningHighTemperature: 313.15f, lethalLowTemperature: 228.15f, lethalHighTemperature: 373.15f);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Hatch"];
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGetDef<BurrowMonitor.Def>();
    placedEntity.AddOrGetDef<WorldSpawnableMonitor.Def>().adjustSpawnLocationCb = new Func<int, int>(BaseHatchConfig.AdjustSpawnLocationCB);
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    placedEntity.AddWeapon(1f, 1f);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_idle", NOISE_POLLUTION.CREATURES.TIER2);
    SoundEventVolumeCache.instance.AddVolume("FloorSoundEvent", "Hatch_footstep", NOISE_POLLUTION.CREATURES.TIER1);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_land", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_chew", NOISE_POLLUTION.CREATURES.TIER3);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_hurt", NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_die", NOISE_POLLUTION.CREATURES.TIER5);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_emerge", NOISE_POLLUTION.CREATURES.TIER6);
    SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_hide", NOISE_POLLUTION.CREATURES.TIER6);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    bool condition = !is_baby;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "hatch_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new ExitBurrowStates.Def(), condition).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.Burrowed, true, "idle_mound", (string) STRINGS.CREATURES.STATUSITEMS.BURROWED.NAME, (string) STRINGS.CREATURES.STATUSITEMS.BURROWED.TOOLTIP), condition).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def(), condition).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.WantsToEnterBurrow, false, "hide", (string) STRINGS.CREATURES.STATUSITEMS.BURROWING.NAME, (string) STRINGS.CREATURES.STATUSITEMS.BURROWING.TOOLTIP), condition).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = is_baby
    }).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new CallAdultStates.Def(), is_baby).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2), !is_baby).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.HatchSpecies, symbolOverridePrefix);
    return placedEntity;
  }

  public static List<Diet.Info> BasicRockDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Sand.CreateTag(),
        SimHashes.SandStone.CreateTag(),
        SimHashes.Clay.CreateTag(),
        SimHashes.CrushedRock.CreateTag(),
        SimHashes.Dirt.CreateTag(),
        SimHashes.SedimentaryRock.CreateTag(),
        SimHashes.Shale.CreateTag(),
        SimHashes.SiltStone.CreateTag(),
        SimHashes.Coquina.CreateTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    };
  }

  public static List<Diet.Info> HardRockDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.SedimentaryRock.CreateTag(),
        SimHashes.IgneousRock.CreateTag(),
        SimHashes.Obsidian.CreateTag(),
        SimHashes.Granite.CreateTag(),
        SimHashes.Basalt.CreateTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    };
  }

  public static List<Diet.Info> MetalDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    List<Diet.Info> infoList = new List<Diet.Info>();
    foreach (Tag basicMetalOre in GameTags.BasicMetalOres)
    {
      Tag produced_element = poopTag == GameTags.Metal ? ElementLoader.FindElementByTag(basicMetalOre).refinedMetalTarget.CreateTag() : poopTag;
      infoList.Add(new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
      {
        basicMetalOre
      }), produced_element, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced));
    }
    infoList.Add(new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
    {
      SimHashes.GoldAmalgam.CreateTag()
    }), poopTag == GameTags.Metal ? SimHashes.Gold.CreateTag() : poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced));
    infoList.Add(new Diet.Info(new HashSet<Tag>((IEnumerable<Tag>) new Tag[1]
    {
      SimHashes.Wolframite.CreateTag()
    }), poopTag == GameTags.Metal ? SimHashes.Tungsten.CreateTag() : poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced));
    return infoList;
  }

  public static List<Diet.Info> VeggieDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    return new List<Diet.Info>()
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Dirt.CreateTag(),
        SimHashes.SlimeMold.CreateTag(),
        SimHashes.Algae.CreateTag(),
        SimHashes.Fertilizer.CreateTag(),
        SimHashes.ToxicSand.CreateTag(),
        SimHashes.ToxicMud.CreateTag(),
        SimHashes.Corallium.CreateTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    };
  }

  public static List<Diet.Info> FoodDiet(
    Tag poopTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced)
  {
    List<Diet.Info> infoList = new List<Diet.Info>();
    foreach (EdiblesManager.FoodInfo allLoadedFoodType in EdiblesManager.GetAllLoadedFoodTypes())
    {
      if ((double) allLoadedFoodType.CaloriesPerUnit > 0.0)
        infoList.Add(new Diet.Info(new HashSet<Tag>()
        {
          new Tag(allLoadedFoodType.Id)
        }, poopTag, allLoadedFoodType.CaloriesPerUnit, producedConversionRate, diseaseId, diseasePerKgProduced));
    }
    return infoList;
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    List<Diet.Info> diet_infos,
    float referenceCaloriesPerKg,
    float minPoopSizeInKg)
  {
    Diet diet = new Diet(diet_infos.ToArray());
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minConsumedCaloriesBeforePooping = referenceCaloriesPerKg * minPoopSizeInKg;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }

  private static int AdjustSpawnLocationCB(int cell)
  {
    int num;
    for (; !Grid.Solid[cell]; cell = num)
    {
      num = Grid.CellBelow(cell);
      if (!Grid.IsValidCell(cell))
        break;
    }
    return cell;
  }
}
