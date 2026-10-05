// Decompiled with JetBrains decompiler
// Type: BaseSeaFairyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using TUNING;
using UnityEngine;

#nullable disable
public static class BaseSeaFairyConfig
{
  public const string EMOTION_FILE_NAME = "sea_fairy_emotes_kanim";

  public static GameObject BaseSeaFairy(
    string id,
    string name,
    string desc,
    string anim_file,
    string traitId,
    string symbolOverridePrefix = null)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR0 = DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) "sea_fairy_build_kanim");
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 5f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, "sea_fairy_build_kanim", faction: FactionManager.FactionID.Pest, initialTraitID: traitId, NavGridName: "SwimmerNavGrid", navType: NavType.Swim, onDeathDropID: "Nori", onDeathDropCount: 10f, drownVulnerable: false, warningHighTemperature: 318.15f, lethalLowTemperature: 233.15f, lethalHighTemperature: 353.15f);
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = CREATURES.SORTING.CRITTER_ORDER["SeaFairy"];
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.SwimmingCreature);
    component.AddTag(GameTags.Creatures.Swimmer);
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>().canSwim = true;
    placedEntity.AddOrGetDef<FlopMonitor.Def>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = CREATURES.SPACE_REQUIREMENTS.TIER2;
    placedEntity.AddOrGetDef<FishOvercrowdingMonitor.Def>();
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "sea_fairy_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()
    {
      getLandAnim = new Func<FallStates.Instance, string>(BaseSeaFairyConfig.GetLandAnim)
    }).Add((StateMachine.BaseDef) new FlopStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new MoveToLureStates.Def()).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.SeaFairySpecies, symbolOverridePrefix);
    return placedEntity;
  }

  private static string GetLandAnim(FallStates.Instance smi)
  {
    return smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation() ? "idle_loop" : "flop_loop";
  }
}
