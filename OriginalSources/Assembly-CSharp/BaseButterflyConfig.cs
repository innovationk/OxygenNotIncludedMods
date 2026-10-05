// Decompiled with JetBrains decompiler
// Type: BaseButterflyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using TUNING;
using UnityEngine;

#nullable disable
public static class BaseButterflyConfig
{
  public const string EMOTION_FILE_NAME = "pollinator_emotes_kanim";

  public static GameObject BaseButterfly(
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
    KAnimFile anim1 = Assets.GetAnim((HashedString) anim_file);
    EffectorValues decor = tieR0;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 5f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, "pollinator_build_kanim", faction: FactionManager.FactionID.Pest, initialTraitID: traitId, NavGridName: "FlyerNavGrid1x1", navType: NavType.Hover, onDeathDropID: "ButterflyPlantSeed", warningHighTemperature: 318.15f, lethalLowTemperature: 233.15f, lethalHighTemperature: 353.15f);
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = CREATURES.SORTING.CRITTER_ORDER["Butterfly"];
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Flyer);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>();
    placedEntity.AddOrGetDef<SubmergedMonitor.Def>();
    placedEntity.AddOrGetDef<PollinateMonitor.Def>().radius = 10;
    placedEntity.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = CREATURES.SPACE_REQUIREMENTS.TIER2;
    placedEntity.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[2]
    {
      GameTags.Algae,
      GameTags.Creatures.FlyersLure
    };
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "pollinator_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def()).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new ApproachBehaviourStates.Def(PollinateMonitor.ID, GameTags.Creatures.WantsToPollinate)
    {
      preAnim = "pollinate_pre",
      loopAnim = "pollinate_loop",
      pstAnim = "pollinate_pst"
    }).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def());
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.ButterflySpecies, symbolOverridePrefix);
    return placedEntity;
  }
}
