// Decompiled with JetBrains decompiler
// Type: BasePufferFish
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BasePufferFish
{
  private const string EMOTION_FILE_NAME = "blowfish_emotes_kanim";

  public static GameObject CreatePrefab(
    string id,
    string base_trait_id,
    string name,
    string description,
    string anim_file,
    bool is_baby,
    string symbol_prefix,
    float warnLowTemp,
    float warnHighTemp,
    float lethalLowTemp,
    float lethalHighTemp)
  {
    string id1 = id;
    string name1 = name;
    string desc = description;
    double mass = (double) PufferFishTuning.MASS;
    EffectorValues decor1 = PufferFishTuning.DECOR;
    KAnimFile anim1 = Assets.GetAnim((HashedString) anim_file);
    EffectorValues decor2 = decor1;
    float num = (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0);
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) num;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc, (float) mass, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor2, noise, defaultTemperature: (float) defaultTemperature);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.SwimmingCreature);
    component.AddTag(GameTags.Creatures.Swimmer);
    Trait trait = Db.Get().CreateTrait(base_trait_id, name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 2000000f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -666.6667f, (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, PufferFishTuning.HITPOINTS, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, PufferFishTuning.LIFESPAN, name));
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, false, true, true);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "blowfish_build_kanim", symbol_prefix, initialTraitID: base_trait_id, NavGridName: "SwimmerNavGrid", navType: NavType.Swim, onDeathDropID: "FishMeat", drownVulnerable: false, entombVulnerable: false, warningLowTemperature: warnLowTemp, warningHighTemperature: warnHighTemp, lethalLowTemperature: lethalLowTemp, lethalHighTemperature: lethalHighTemp);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "blowfish_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()
    {
      getLandAnim = new Func<FallStates.Instance, string>(BasePufferFish.GetLandAnim)
    }).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FlopStates.Def()
    {
      frameToFlopStart = (is_baby ? 19 : 10),
      frameToFlopEnd = (is_baby ? 33 : 32 /*0x20*/)
    }).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()).Add((StateMachine.BaseDef) new VentBubbleStates.Def()
    {
      element = SimHashes.Oxygen,
      dupebreathingAnimFiles = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_interact_blowfish_kanim")
      },
      dupebreathingAnims = new HashedString[2]
      {
        (HashedString) "blowfish_breath_pre",
        (HashedString) "blowfish_breathe_loop"
      },
      dupebreathingPst = new HashedString[1]
      {
        (HashedString) "blowfish_breath_pst"
      }
    }, (!is_baby ? 1 : 0) != 0).Add((StateMachine.BaseDef) new MoveToLureStates.Def()).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
    placedEntity.AddOrGetDef<FlopMonitor.Def>();
    placedEntity.AddOrGetDef<FishOvercrowdingMonitor.Def>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.PufferFishSpecies, symbol_prefix);
    CritterCondoInteractMontior.Def def1 = placedEntity.AddOrGetDef<CritterCondoInteractMontior.Def>();
    def1.requireCavity = false;
    def1.condoPrefabTag = (Tag) "UnderwaterCritterCondo";
    HashSet<Tag> tagSet = new HashSet<Tag>();
    HashSet<Tag> consumed_tags1 = new HashSet<Tag>();
    consumed_tags1.Add((Tag) "Lettuce");
    HashSet<Tag> consumed_tags2 = new HashSet<Tag>();
    consumed_tags2.Add((Tag) SeaLettuceConfig.ID);
    string[] eat_anims = new string[3]
    {
      "eat_pre",
      "eat_loop",
      "idle_loop"
    };
    Diet diet = new Diet(new List<Diet.Info>()
    {
      new Diet.Info(consumed_tags1, PufferFishTuning.POOP_ELEMENT, 400000f, 15f, eat_anims: eat_anims),
      new Diet.Info(consumed_tags2, PufferFishTuning.POOP_ELEMENT, 400000f, 15f, food_type: Diet.Info.FoodType.EatPlantDirectly, eat_anims: eat_anims)
    }.ToArray());
    CreatureCalorieMonitor.Def def2 = placedEntity.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def2.diet = diet;
    def2.minConsumedCaloriesBeforePooping = 200000f;
    def2.minimumTimeBeforePooping = 0.0f;
    placedEntity.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    Storage storage = placedEntity.AddComponent<Storage>();
    storage.capacityKg = PufferFishTuning.OXYGEN_STORAGE_CAPACITY;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    placedEntity.AddOrGet<UnderwaterBreathingLocation>().allowLandUse = false;
    placedEntity.AddOrGet<UnderwaterBreathingLocationWorkable>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
    if (!string.IsNullOrEmpty(symbol_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_prefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["PufferFish"];
    return placedEntity;
  }

  private static string GetLandAnim(FallStates.Instance smi)
  {
    return smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation() ? "idle_loop" : "flop_loop";
  }
}
