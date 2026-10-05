// Decompiled with JetBrains decompiler
// Type: BaseMoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BaseMoleConfig
{
  public const string EMOTION_FILE_NAME = "driller_emotes_kanim";
  private static readonly string[] SolidIdleAnims = new string[4]
  {
    "idle1",
    "idle2",
    "idle3",
    "idle4"
  };

  public static GameObject BaseMole(
    string id,
    string name,
    string desc,
    string traitId,
    string anim_file,
    bool is_baby,
    float warningLowTemperature,
    float warningHighTemperature,
    float lethalLowTemperature,
    float lethalHighTemperature,
    string symbolOverridePrefix = null,
    int on_death_drop_count = 10)
  {
    string id1 = id;
    string name1 = name;
    string desc1 = desc;
    EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
    KAnimFile anim1 = Assets.GetAnim((HashedString) (is_baby ? anim_file : "driller_build_kanim"));
    EffectorValues decor = none;
    EffectorValues noise = new EffectorValues();
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc1, 25f, anim1, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, anim_file, is_baby ? (string) null : "driller_build_kanim", symbolOverridePrefix, FactionManager.FactionID.Pest, traitId, "DiggerNavGrid", onDeathDropCount: (float) on_death_drop_count, entombVulnerable: false, warningLowTemperature: warningLowTemperature, warningHighTemperature: warningHighTemperature, lethalLowTemperature: lethalLowTemperature, lethalHighTemperature: lethalHighTemperature);
    if (symbolOverridePrefix != null)
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbolOverridePrefix);
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Mole"];
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<DiggerMonitor.Def>().depthToDig = MoleTuning.DEPTH_TO_HIDE;
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true);
    placedEntity.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker);
    KAnimFile anim2 = Assets.GetAnim((HashedString) "driller_emotes_kanim");
    ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DrowningStates.Def()).Add((StateMachine.BaseDef) new DiggerStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), is_baby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), is_baby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def(), !is_baby).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !is_baby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CreatureSleepStates.Def()).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = is_baby
    }).Add((StateMachine.BaseDef) new NestingPoopState.Def(is_baby ? Tag.Invalid : SimHashes.Regolith.CreateTag())).Add((StateMachine.BaseDef) new PoopStates.Def(anim2, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !is_baby).Add((StateMachine.BaseDef) new CritterEmoteStates.Def(anim2), !is_baby).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def()
    {
      customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseMoleConfig.CustomIdleAnim)
    });
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.MoleSpecies, symbolOverridePrefix);
    return placedEntity;
  }

  public static List<Diet.Info> SimpleOreDiet(
    List<Tag> elementTags,
    float caloriesPerKg,
    float producedConversionRate)
  {
    List<Diet.Info> infoList = new List<Diet.Info>();
    foreach (Tag elementTag in elementTags)
      infoList.Add(new Diet.Info(new HashSet<Tag>()
      {
        elementTag
      }, elementTag, caloriesPerKg, producedConversionRate, produce_solid_tile: true));
    return infoList;
  }

  private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
  {
    if (smi.gameObject.GetComponent<Navigator>().CurrentNavType == NavType.Solid)
    {
      int index = Random.Range(0, BaseMoleConfig.SolidIdleAnims.Length);
      return (HashedString) BaseMoleConfig.SolidIdleAnims[index];
    }
    return smi.gameObject.GetDef<BabyMonitor.Def>() != null && Random.Range(0, 100) >= 90 ? (HashedString) "drill_fail" : (HashedString) "idle_loop";
  }
}
