// Decompiled with JetBrains decompiler
// Type: BaseCrabConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public static class BaseCrabConfig
{
  public const string EMOTION_FILE_NAME = "pincher_emotes_kanim";

  public static GameObject BaseCrab(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    bool is_baby,
    string symbolOverridePrefix = null,
    string onDeathDropID = "CrabShell",
    float onDeathDropCount = 1f)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    string anim_file1 = anim_file;
    string traitId1 = traitId;
    int num = is_baby ? 1 : 0;
    string symbolOverridePrefix1 = symbolOverridePrefix;
    string[] onDeathDropsID;
    if (!string.IsNullOrEmpty(onDeathDropID))
      onDeathDropsID = new string[1]{ onDeathDropID };
    else
      onDeathDropsID = (string[]) null;
    float[] onDeathDropsCount = new float[1]
    {
      onDeathDropCount
    };
    return BaseCrabConfig.BaseCrab(id1, name1, desc1, anim_file1, traitId1, num != 0, symbolOverridePrefix1, onDeathDropsID, onDeathDropsCount);
  }

  public static GameObject BaseCrab(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    bool is_baby,
    string symbolOverridePrefix,
    string[] onDeathDropsID,
    float[] onDeathDropsCount)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    int num1 = is_baby ? 1 : 2;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "pincher_build_kanim"));
    int height = num1;
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 100f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, height, decor, noise);
    string str = "WalkerNavGrid1x2";
    if (is_baby)
      str = "WalkerBabyNavGrid";
    EntityTemplates.ExtendEntityToBasicCreature(new EntityTemplates.ExtendEntityToBasicCreatureData()
    {
      isWarmBlooded = false,
      template = placedEntity,
      anim_filename = anim_file,
      build_filename = is_baby ? (string) null : "pincher_build_kanim",
      symbol_override_prefix = symbolOverridePrefix,
      faction = FactionManager.FactionID.Pest,
      initialTraitID = traitId,
      NavGridName = str,
      onDeathDropsID = onDeathDropsID,
      onDeathDropsCount = onDeathDropsCount,
      entombVulnerable = false,
      drownVulnerable = false,
      warningLowTemperature = 273.15f,
      warningHighTemperature = 313.15f,
      lethalLowTemperature = 223.15f,
      lethalHighTemperature = 373.15f
    });
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Crab"];
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    ThreatMonitor.Def def1 = placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    def1.fleethresholdState = Health.HealthState.Dead;
    def1.friendlyCreatureTags = new Tag[1]
    {
      GameTags.Creatures.CrabFriend
    };
    def1.maxSearchDistance = 12;
    def1.offsets = CrabTuning.DEFEND_OFFSETS;
    placedEntity.AddWeapon(2f, 3f);
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
    component.AddTag(GameTags.Creatures.CrabFriend);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "pincher_emotes_kanim");
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new DefendStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = true
    }).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new PunchClamOpenStates.Def(), !is_baby && DlcManager.IsContentSubscribed("DLC5_ID")).Add((StateMachine.BaseDef) new CallAdultStates.Def(), is_baby);
    CritterCondoStates.Def def2 = new CritterCondoStates.Def();
    def2.entersBuilding = false;
    int num2 = !is_baby ? 1 : 0;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def2, num2 != 0).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new CreatureDiseaseCleaner.Def(30f)).Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.CrabSpecies, symbolOverridePrefix);
    CritterCondoInteractMontior.Def def3 = placedEntity.AddOrGetDef<CritterCondoInteractMontior.Def>();
    def3.requireCavity = false;
    def3.condoPrefabTag = (Tag) "UnderwaterCritterCondo";
    if (!is_baby && DlcManager.IsContentSubscribed("DLC5_ID"))
      placedEntity.AddOrGetDef<PunchClamMonitor.Def>();
    placedEntity.AddTag(GameTags.Amphibious);
    return placedEntity;
  }

  public static List<Diet.Info> BasicDiet(
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
        SimHashes.ToxicSand.CreateTag(),
        RotPileConfig.ID.ToTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    };
  }

  public static List<Diet.Info> DietWithSlime(
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
        SimHashes.ToxicSand.CreateTag(),
        RotPileConfig.ID.ToTag(),
        SimHashes.SlimeMold.CreateTag()
      }, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    };
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
