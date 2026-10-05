// Decompiled with JetBrains decompiler
// Type: BaseSnailConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaseSnailConfig
{
  private const string IDLE_LOOP = "idle_loop";
  private const string IDLE_LOOP_SAD = "idle_loop_sad";

  public static GameObject CreatePrefab(
    string id,
    string baseTraitId,
    string name,
    string description,
    string animFile,
    bool isBaby,
    string symbolPrefix,
    float warnLowTemp,
    float warnHighTemp,
    float lethalLowTemp,
    float lethalHighTemp,
    string moltPrefabId)
  {
    string id1 = id;
    string name1 = name;
    string desc = description;
    double mass = (double) SnailTuning.MASS;
    EffectorValues tieR0 = TUNING.DECOR.BONUS.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) (isBaby ? animFile : "snail_build_kanim"));
    EffectorValues decor = tieR0;
    float num = (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0);
    EffectorValues noise = new EffectorValues();
    double defaultTemperature = (double) num;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id1, name1, desc, (float) mass, anim, "idle_loop", Grid.SceneLayer.Creatures, 1, 1, decor, noise, defaultTemperature: (float) defaultTemperature);
    placedEntity.AddTag(GameTags.Amphibious);
    placedEntity.AddTag(GameTags.Creatures.Walker);
    BaseSnailConfig.ConfigureTraits(baseTraitId, name, !isBaby);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, false, true, true);
    EntityTemplates.ExtendEntityToBasicCreature(false, placedEntity, animFile, isBaby ? (string) null : "snail_build_kanim", symbolPrefix, initialTraitID: baseTraitId, NavGridName: isBaby ? "WalkerBabyNavGrid" : "WalkerNavGrid1x1NoJump", max_probing_radius: 16 /*0x10*/, moveSpeed: 0.25f, onDeathDropCount: 0.5f, drownVulnerable: false, entombVulnerable: false, warningLowTemperature: warnLowTemp, warningHighTemperature: warnHighTemp, lethalLowTemperature: lethalLowTemp, lethalHighTemperature: lethalHighTemp);
    ChoreTable.Builder choreTable = BaseSnailConfig.CreateChoreTable(isBaby, animFile);
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    placedEntity.AddWeapon(1f, 1f);
    EntityTemplates.AddCreatureBrain(placedEntity, choreTable, GameTags.Creatures.Species.SnailSpecies, symbolPrefix);
    if (!isBaby)
    {
      BaseSnailConfig.UsesCondo(placedEntity);
      BaseSnailConfig.DropsMolt(placedEntity, moltPrefabId);
      MoistureMonitor.Def def = placedEntity.AddOrGetDef<MoistureMonitor.Def>();
      def.lubricant = SimHashes.Mucus;
      def.onDryLandModifier = SnailTuning.MUCUS_PER_CYCLE_DRY_LAND_BONUS / 600f;
      def.lubricantTemperatureKelvin = 311.15f;
      placedEntity.AddOrGetDef<DesiccationMonitor.Def>();
    }
    placedEntity.AddOrGet<Pickupable>().sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Snail"];
    return placedEntity;
  }

  private static void DropsMolt(GameObject prefab, string prefabId)
  {
    MoltDropperMonitor.Def def = prefab.AddOrGetDef<MoltDropperMonitor.Def>();
    def.onGrowDropID = prefabId;
    def.massToDrop = 10f;
    def.isReadyToMolt = new Func<MoltDropperMonitor.Instance, bool>(BaseSnailConfig.IsReadyToMolt);
  }

  public static bool IsReadyToMolt(MoltDropperMonitor.Instance smi)
  {
    return BaseSnailConfig.IsValidTimeToDropMolt(smi) && BaseSnailConfig.IsValidDropCell(smi) && !smi.prefabID.HasTag(GameTags.Creatures.Hungry) && smi.prefabID.HasTag(GameTags.Creatures.Happy);
  }

  public static bool IsValidTimeToDropMolt(MoltDropperMonitor.Instance smi)
  {
    if (smi.spawnedThisCycle)
      return false;
    return (double) smi.timeOfLastDrop <= 0.0 || (double) GameClock.Instance.GetTime() - (double) smi.timeOfLastDrop > 600.0;
  }

  public static void OnSpawn(GameObject inst)
  {
    Navigator component = inst.GetComponent<Navigator>();
    component.transitionDriver.overrideLayers.Add((TransitionDriver.OverrideLayer) new SadSnailTransitionLayer(component));
  }

  private static void UsesCondo(GameObject prefab)
  {
    CritterCondoInteractMontior.Def def = prefab.AddOrGetDef<CritterCondoInteractMontior.Def>();
    def.requireCavity = false;
    def.condoPrefabTag = (Tag) "CritterCondo";
  }

  private static ChoreTable.Builder CreateChoreTable(bool isBaby, string animFile)
  {
    ChoreTable.Builder choreTable = new ChoreTable.Builder();
    KAnimFile anim = Assets.GetAnim((HashedString) animFile);
    choreTable.Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new GrowUpStates.Def(), isBaby).Add((StateMachine.BaseDef) new TrappedStates.Def()).Add((StateMachine.BaseDef) new IncubatingStates.Def(), isBaby).Add((StateMachine.BaseDef) new BaggedStates.Def()).Add((StateMachine.BaseDef) new FallStates.Def()).Add((StateMachine.BaseDef) new StunnedStates.Def()).Add((StateMachine.BaseDef) new DebugGoToStates.Def()).Add((StateMachine.BaseDef) new FleeStates.Def()).Add((StateMachine.BaseDef) new AttackStates.Def(), !isBaby).PushInterruptGroup().Add((StateMachine.BaseDef) new MucusSecretionStates.Def()).Add((StateMachine.BaseDef) new FixedCaptureStates.Def()).Add((StateMachine.BaseDef) new RanchedStates.Def(), !isBaby).Add((StateMachine.BaseDef) new LayEggStates.Def(), !isBaby).Add((StateMachine.BaseDef) new EatStates.Def()).Add((StateMachine.BaseDef) new DrinkMilkStates.Def()
    {
      shouldBeBehindMilkTank = true
    }).Add((StateMachine.BaseDef) new PoopStates.Def(anim, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP, false)).Add((StateMachine.BaseDef) new CallAdultStates.Def(), isBaby).Add((StateMachine.BaseDef) new CritterCondoStates.Def(), !isBaby).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStates.Def()
    {
      customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseSnailConfig.CustomIdleAnim)
    });
    return choreTable;
  }

  private static void ConfigureTraits(string baseTraitId, string name, bool isAdult)
  {
    Trait trait = Db.Get().CreateTrait(baseTraitId, name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SnailTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) SnailTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name));
    if (!isAdult)
      return;
    trait.Add(new AttributeModifier(Db.Get().Amounts.Moisture.deltaAttribute.Id, SnailTuning.DEFAULT_DRYING_RATE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Mucus.deltaAttribute.Id, SnailTuning.MUCUS_PER_CYCLE / 600f, (string) STRINGS.CREATURES.MODIFIERS.MUCUS.BASE_RATE));
  }

  private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
  {
    DesiccationMonitor.Instance smi1 = smi.GetSMI<DesiccationMonitor.Instance>();
    return (HashedString) (smi1 == null || !smi1.IsDesiccating() ? "idle_loop" : "idle_loop_sad");
  }

  public static bool IsValidDropCell(MoltDropperMonitor.Instance smi)
  {
    return Grid.IsValidCell(Grid.PosToCell(smi.transform.GetPosition()));
  }

  public static Diet.Info[] SaltToDirtDiet()
  {
    return new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Salt.CreateTag()
      }, SimHashes.Dirt.CreateTag(), SnailTuning.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL)
    };
  }

  public static Diet.Info[] SulfurToObsidianDiet()
  {
    return new Diet.Info[1]
    {
      new Diet.Info(new HashSet<Tag>()
      {
        SimHashes.Sulfur.CreateTag()
      }, SimHashes.Obsidian.CreateTag(), SnailTuning.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL)
    };
  }
}
