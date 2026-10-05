// Decompiled with JetBrains decompiler
// Type: BaseMooConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BaseMooConfig
{
  public const string EMOTION_FILE_NAME = "gassy_moo_emotes_kanim";

  public static GameObject BaseMoo(
    string id,
    string name,
    string desc,
    string traitId,
    string anim_file,
    List<BeckoningMonitor.SongChance> initialSongChances,
    bool is_baby,
    string symbol_override_prefix)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "gassy_moo_build_kanim"));
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 50f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 2, 2, decor, noise);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "gassy_moo_build_kanim", symbol_override_prefix, initialTraitID: traitId, NavGridName: "FlyerNavGrid2x2", navType: NavType.Hover, onDeathDropCount: 10f, warningLowTemperature: 223.15f, warningHighTemperature: 323.15f, lethalLowTemperature: 73.1499939f, lethalHighTemperature: 473.15f);
    if (!string.IsNullOrEmpty(symbol_override_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_override_prefix);
    if (!is_baby)
    {
      KBoxCollider2D kboxCollider2D = placedEntity.AddOrGet<KBoxCollider2D>();
      kboxCollider2D.offset = (Vector2) new Vector2f(0.0f, kboxCollider2D.offset.y);
    }
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Moo"];
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Flyer);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    BeckoningMonitor.Def def1 = placedEntity.AddOrGetDef<BeckoningMonitor.Def>();
    def1.initialSongWeights = initialSongChances;
    def1.caloriesPerCycle = MooTuning.WELLFED_CALORIES_PER_CYCLE;
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[2]
    {
      SimHashes.BleachStone.CreateTag(),
      GameTags.Creatures.FlyersLure
    };
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    placedEntity.AddOrGetDef<RanchableMonitor.Def>();
    placedEntity.AddOrGetDef<FixedCapturableMonitor.Def>();
    MilkProductionMonitor.Def def2 = placedEntity.AddOrGetDef<MilkProductionMonitor.Def>();
    def2.CaloriesPerCycle = MooTuning.WELLFED_CALORIES_PER_CYCLE;
    def2.Capacity = MooTuning.MILK_CAPACITY;
    KAnimFile anim2 = Assets.GetAnim((HashedString) "gassy_moo_emotes_kanim");
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new BeckonFromSpaceStates.Def()).Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def()
    {
      WaitCellOffset = 2
    }).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = false,
      drinkCellOffsetGetFn = new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_GassyMoo)
    }).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_GAS.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP, false)).Add((StateMachine.BaseDef) new MoveToLureStates.Def());
    CritterCondoStates.Def def3 = new CritterCondoStates.Def();
    def3.working_anim = "cc_working_moo";
    int num = !is_baby ? 1 : 0;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def3, num != 0).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2), !is_baby).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def()
    {
      customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseMooConfig.CustomIdleAnim)
    });
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.MooSpecies, symbol_override_prefix);
    placedEntity.AddOrGetDef<CritterCondoInteractMontior.Def>().condoPrefabTag = (Tag) "AirBorneCritterCondo";
    return placedEntity;
  }

  public static void SetupBaseDiet(GameObject prefab, Tag producedTag)
  {
    Diet diet = BaseMooConfig.ExpandDiet(BaseMooConfig.ExpandDiet((Diet) null, prefab, "GasGrass".ToTag(), producedTag, MooTuning.CALORIES_PER_DAY_OF_PLANT_EATEN, MooTuning.KG_POOP_PER_DAY_OF_PLANT, Diet.Info.FoodType.EatPlantDirectly, MooTuning.MIN_POOP_SIZE_IN_KG), prefab, "PlantFiber".ToTag(), producedTag, MooTuning.CALORIES_PER_DAY_OF_SOLID_EATEN, MooTuning.POOP_KG_COVERSION_RATE_FOR_SOLID_DIET, Diet.Info.FoodType.EatSolid, MooTuning.MIN_POOP_SIZE_IN_KG);
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minConsumedCaloriesBeforePooping = MooTuning.MIN_POOP_SIZE_IN_CALORIES;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
  }

  public static Diet ExpandDiet(
    Diet diet,
    GameObject prefab,
    Tag consumed_tag,
    Tag producedTag,
    float caloriesPerKg,
    float producedConversionRate,
    Diet.Info.FoodType foodType,
    float minPoopSizeInKg)
  {
    HashSet<Tag> consumed_tags = new HashSet<Tag>();
    consumed_tags.Add(consumed_tag);
    Diet.Info[] infoArray = diet != null ? new Diet.Info[diet.infos.Length + 1] : new Diet.Info[1];
    if (diet != null)
    {
      for (int index = 0; index < diet.infos.Length; ++index)
        infoArray[index] = diet.infos[index];
    }
    infoArray[infoArray.Length - 1] = new Diet.Info(consumed_tags, producedTag, caloriesPerKg, producedConversionRate, food_type: foodType);
    return new Diet(infoArray);
  }

  private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
  {
    CreatureCalorieMonitor.Instance smi1 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
    return (HashedString) (smi1 == null || !smi1.stomach.IsReadyToPoop() ? "idle_loop" : "idle_loop_full");
  }

  public static void OnSpawn(GameObject inst)
  {
    Navigator component = inst.GetComponent<Navigator>();
    component.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new FullPuftTransitionLayer(component));
  }
}
