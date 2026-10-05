// Decompiled with JetBrains decompiler
// Type: BaseDreckoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
public static class BaseDreckoConfig
{
  public const string EMOTION_FILE_NAME = "drecko_emotes_kanim";

  public static GameObject BaseDrecko(
    string id,
    string name,
    string desc,
    string anim_file,
    string trait_id,
    bool is_baby,
    string symbol_override_prefix,
    float warnLowTemp,
    float warnHighTemp,
    float lethalLowTemp,
    float lethalHighTemp)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "drecko_build_kanim"));
    EffectorValues decor = tieR0;
    float num = (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0);
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) num;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 200f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise, defaultTemperature: (float) defaultTemperature);
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    string NavGridName = "DreckoNavGrid";
    if (is_baby)
      NavGridName = "DreckoBabyNavGrid";
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "drecko_build_kanim", symbol_override_prefix, FactionManager.FactionID.Pest, trait_id, NavGridName, moveSpeed: 1f, onDeathDropCount: 2f, entombVulnerable: false, warningLowTemperature: warnLowTemp, warningHighTemperature: warnHighTemp, lethalLowTemperature: lethalLowTemp, lethalHighTemperature: lethalHighTemp);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Drecko"];
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    placedEntity.AddWeapon(1f, 1f);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "drecko_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def(), !is_baby).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = is_baby
    }).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new CallAdultStates.Def(), is_baby).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2)).PopInterruptGroup().Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new IdleStates.Def()
    {
      customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseDreckoConfig.CustomIdleAnim)
    });
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.DreckoSpecies, symbol_override_prefix);
    return placedEntity;
  }

  private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
  {
    CellOffset offset = new CellOffset(0, -1);
    bool facing = smi.GetComponent<Facing>().GetFacing();
    switch (smi.GetComponent<Navigator>().CurrentNavType)
    {
      case NavType.Floor:
        offset = facing ? new CellOffset(1, -1) : new CellOffset(-1, -1);
        break;
      case NavType.Ceiling:
        offset = facing ? new CellOffset(1, 1) : new CellOffset(-1, 1);
        break;
    }
    HashedString hashedString = (HashedString) "idle_loop";
    int num = Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) smi), offset);
    if (Grid.IsValidCell(num) && !Grid.Solid[num])
    {
      pre_anim = (HashedString) "idle_loop_hang_pre";
      hashedString = (HashedString) "idle_loop_hang";
    }
    return hashedString;
  }
}
