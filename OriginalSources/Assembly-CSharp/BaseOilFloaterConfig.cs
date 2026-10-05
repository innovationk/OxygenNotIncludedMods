// Decompiled with JetBrains decompiler
// Type: BaseOilFloaterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public static class BaseOilFloaterConfig
{
  public const string EMOTION_FILE_NAME = "oilfloater_emotes_kanim";

  public static GameObject BaseOilFloater(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    float warnLowTemp,
    float warnHighTemp,
    float lethalLowTemp,
    float lethalHighTemp,
    bool is_baby,
    string symbolOverridePrefix = null)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR1 = DECOR.BONUS.TIER1;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "oilfloater_build_kanim"));
    EffectorValues decor = tieR1;
    float num = (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0);
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) num;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 50f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise, defaultTemperature: (float) defaultTemperature);
    placedEntity.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Hoverer);
    placedEntity.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "oilfloater_build_kanim", symbolOverridePrefix, FactionManager.FactionID.Pest, traitId, "FloaterNavGrid", NavType.Hover, onDeathDropCount: 2f, entombVulnerable: false, warningLowTemperature: warnLowTemp, warningHighTemperature: warnHighTemp, lethalLowTemperature: lethalLowTemp, lethalHighTemperature: lethalHighTemp);
    placedEntity.AddOrGet<Pickupable>().sortOrder = CREATURES.SORTING.CRITTER_ORDER["Oilfloater"];
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
    placedEntity.AddWeapon(1f, 1f);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    string str1 = "OilFloater_intake_air";
    if (is_baby)
      str1 = "OilFloaterBaby_intake_air";
    KAnimFile anim2 = Assets.GetAnim((HashedString) "oilfloater_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new InhaleStates.Def()
    {
      inhaleSound = str1
    }).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()).Add((StateMachine.BaseDef) new SameSpotPoopStates.Def()).Add((StateMachine.BaseDef) new CallAdultStates.Def(), is_baby).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.OilFloaterSpecies, symbolOverridePrefix);
    string str2 = "OilFloater_move_LP";
    if (is_baby)
      str2 = "OilFloaterBaby_move_LP";
    placedEntity.AddOrGet<OilFloaterMovementSound>().sound = str2;
    return placedEntity;
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    Tag consumed_tag,
    Tag producedTag,
    float caloriesPerKg,
    float producedConversionRate,
    string diseaseId,
    float diseasePerKgProduced,
    float minPoopSizeInKg)
  {
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>() { consumed_tag }, producedTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced)
    });
    CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minConsumedCaloriesBeforePooping = minPoopSizeInKg * caloriesPerKg;
    prefab.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }
}
