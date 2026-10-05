// Decompiled with JetBrains decompiler
// Type: BaseLightBugConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public static class BaseLightBugConfig
{
  public const string EMOTION_FILE_NAME = "lightbug_emotes_kanim";

  public static GameObject BaseLightBug(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    Color lightColor,
    EffectorValues decor,
    bool is_baby,
    string symbolOverridePrefix = null,
    string onDeathDropID = "",
    float onDeathDropCount = 0.0f)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues effectorValues = decor;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "lightbug_build_kanim"));
    EffectorValues decor1 = effectorValues;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 5f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor1, noise);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "lightbug_build_kanim", symbolOverridePrefix, initialTraitID: traitId, NavGridName: "FlyerNavGrid1x1", navType: NavType.Hover, onDeathDropID: onDeathDropID, onDeathDropCount: onDeathDropCount, warningHighTemperature: 313.15f, lethalLowTemperature: 173.15f, lethalHighTemperature: 373.15f);
    EggConfig.CUSTOM_EGG_OUTPUTS.Add((Tag) (id + "Baby"), new List<Tuple<Tag, float>>()
    {
      new Tuple<Tag, float>(SimHashes.NaturalResin.CreateTag(), 5f)
    });
    placedEntity.AddOrGet<Pickupable>().sortOrder = CREATURES.SORTING.CRITTER_ORDER["LightBug"];
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Flyer);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[2]
    {
      GameTags.Phosphorite,
      GameTags.Creatures.FlyersLure
    };
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    if (DlcManager.FeatureRadiationEnabled())
    {
      RadiationEmitter radiationEmitter = placedEntity.AddOrGet<RadiationEmitter>();
      radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
      radiationEmitter.radiusProportionalToRads = false;
      radiationEmitter.emitRadiusX = (short) 6;
      radiationEmitter.emitRadiusY = radiationEmitter.emitRadiusX;
      radiationEmitter.emitRads = 60f;
      radiationEmitter.emissionOffset = new Vector3(0.0f, 0.0f, 0.0f);
      component.prefabSpawnFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<RadiationEmitter>().SetEmitting(true));
    }
    if (lightColor != Color.black)
    {
      Light2D light2D = placedEntity.AddOrGet<Light2D>();
      light2D.Color = lightColor;
      light2D.overlayColour = LIGHT2D.LIGHTBUG_OVERLAYCOLOR;
      light2D.Range = 5f;
      light2D.Angle = 0.0f;
      light2D.Direction = LIGHT2D.LIGHTBUG_DIRECTION;
      light2D.Offset = LIGHT2D.LIGHTBUG_OFFSET;
      light2D.shape = LightShape.Circle;
      light2D.drawOverlay = true;
      light2D.Lux = 1800;
      light2D.IntensityAnimation = 0.2f;
      placedEntity.AddOrGet<LightSymbolTracker>().targetSymbol = (HashedString) "snapTo_light_locator";
      placedEntity.AddOrGetDef<CreatureLightToggleController.Def>();
    }
    KAnimFile anim2 = Assets.GetAnim((HashedString) "lightbug_emotes_kanim");
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = true
    }).Add((StateMachine.BaseDef) new MoveToLureStates.Def()).Add((StateMachine.BaseDef) new CallAdultStates.Def(), is_baby);
    CritterCondoStates.Def def = new CritterCondoStates.Def();
    def.working_anim = "cc_working_shinebug";
    int num = !is_baby ? 1 : 0;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def, num != 0).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2), !is_baby).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.LightBugSpecies, symbolOverridePrefix);
    placedEntity.AddOrGetDef<CritterCondoInteractMontior.Def>().condoPrefabTag = (Tag) "AirBorneCritterCondo";
    return placedEntity;
  }

  public static GameObject SetupDiet(
    GameObject prefab,
    HashSet<Tag> consumed_tags,
    Tag producedTag,
    float caloriesPerKg)
  {
    Diet diet = new Diet(new Diet.Info[1]
    {
      new Diet.Info(consumed_tags, producedTag, caloriesPerKg)
    });
    prefab.AddOrGetDef<CreatureCalorieMonitor.Def>().diet = diet;
    prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    return prefab;
  }

  public static void SetupLoopingSounds(GameObject inst)
  {
    inst.GetComponent<LoopingSounds>().StartSound(GlobalAssets.GetSound("ShineBug_wings_LP"));
  }
}
