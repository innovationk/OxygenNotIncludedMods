// Decompiled with JetBrains decompiler
// Type: BaseBeeHiveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[EntityConfigOrder(5)]
public class BaseBeeHiveConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "BeeHive";
  public const string BASE_TRAIT_ID = "BeeHiveBaseTrait";
  private const int WIDTH = 2;
  private const int HEIGHT = 3;

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("BeeHive", (string) STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME, (string) STRINGS.BUILDINGS.PREFABS.BEEHIVE.DESC, 100f, Assets.GetAnim((HashedString) "beehive_kanim"), "grow_pre", Grid.SceneLayer.Creatures, 2, 3, TUNING.BUILDINGS.DECOR.BONUS.TIER0, NOISE_POLLUTION.NOISY.TIER0, defaultTemperature: TUNING.CREATURES.TEMPERATURE.FREEZING_3);
    placedEntity.GetComponent<InfoDescription>().effect = (string) STRINGS.BUILDINGS.PREFABS.BEEHIVE.EFFECT;
    KPrefabID kprefabId = placedEntity.AddOrGet<KPrefabID>();
    kprefabId.AddTag(GameTags.Experimental);
    kprefabId.AddTag(GameTags.Creature);
    if (Sim.IsRadiationEnabled())
    {
      placedEntity.AddOrGet<Storage>().storageFXOffset = new Vector3(1f, 1f, 0.0f);
      BeeHive.Def def1 = placedEntity.AddOrGetDef<BeeHive.Def>();
      def1.beePrefabID = "Bee";
      def1.larvaPrefabID = "BeeBaby";
      KAnimFile[] kanimFileArray = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_interacts_beehive_kanim")
      };
      HiveWorkableEmpty hiveWorkableEmpty = placedEntity.AddOrGet<HiveWorkableEmpty>();
      hiveWorkableEmpty.workTime = 15f;
      hiveWorkableEmpty.overrideAnims = kanimFileArray;
      hiveWorkableEmpty.workLayer = Grid.SceneLayer.Front;
      RadiationEmitter radiationEmitter = placedEntity.AddComponent<RadiationEmitter>();
      radiationEmitter.emitRadiusX = (short) 7;
      radiationEmitter.emitRadiusY = (short) 6;
      radiationEmitter.emitRate = 3f;
      radiationEmitter.emitRads = 0.0f;
      radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Pulsing;
      radiationEmitter.emissionOffset = new Vector3(0.5f, 1f, 0.0f);
      kprefabId.prefabSpawnFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<RadiationEmitter>().SetEmitting(true));
      placedEntity.AddOrGet<Traits>();
      placedEntity.AddOrGet<Health>();
      placedEntity.AddOrGet<CharacterOverlay>();
      placedEntity.AddOrGet<RangedAttackable>();
      FactionAlignment factionAlignment = placedEntity.AddOrGet<FactionAlignment>();
      factionAlignment.Alignment = FactionManager.FactionID.Hostile;
      factionAlignment.updatePrioritizable = false;
      placedEntity.AddOrGet<Prioritizable>();
      Prioritizable.AddRef(placedEntity);
      placedEntity.AddOrGet<Effects>();
      placedEntity.AddOrGet<TemperatureVulnerable>().Configure(TUNING.CREATURES.TEMPERATURE.FREEZING_9, TUNING.CREATURES.TEMPERATURE.FREEZING_10, TUNING.CREATURES.TEMPERATURE.FREEZING_1, TUNING.CREATURES.TEMPERATURE.FREEZING);
      placedEntity.AddOrGet<DrowningMonitor>().canDrownToDeath = false;
      placedEntity.AddOrGet<EntombVulnerable>();
      placedEntity.AddOrGetDef<DeathMonitor.Def>();
      placedEntity.AddOrGetDef<AnimInterruptMonitor.Def>();
      placedEntity.AddOrGetDef<HiveGrowthMonitor.Def>();
      placedEntity.AddOrGet<FoundationMonitor>().monitorCells = new CellOffset[2]
      {
        new CellOffset(0, -1),
        new CellOffset(1, -1)
      };
      placedEntity.AddOrGetDef<HiveEatingMonitor.Def>().consumedOre = BeeHiveTuning.CONSUMED_ORE;
      HiveHarvestMonitor.Def def2 = placedEntity.AddOrGetDef<HiveHarvestMonitor.Def>();
      def2.producedOre = BeeHiveTuning.PRODUCED_ORE;
      def2.harvestThreshold = 10f;
      Diet diet = new Diet(new Diet.Info[1]
      {
        new Diet.Info(new HashSet<Tag>()
        {
          BeeHiveTuning.CONSUMED_ORE
        }, BeeHiveTuning.PRODUCED_ORE, BeeHiveTuning.CALORIES_PER_KG_OF_ORE, BeeHiveTuning.POOP_CONVERSTION_RATE)
      });
      placedEntity.AddOrGetDef<BeehiveCalorieMonitor.Def>().diet = diet;
      Trait trait = Db.Get().CreateTrait("BeeHiveBaseTrait", (string) STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME, (string) STRINGS.BUILDINGS.PREFABS.BEEHIVE.DESC, (string) null, false, (ChoreGroup[]) null, true, true);
      trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, BeeHiveTuning.STANDARD_STOMACH_SIZE, (string) STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME));
      trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME));
      trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, (string) STRINGS.BUILDINGS.PREFABS.BEEHIVE.NAME));
      Modifiers modifiers = placedEntity.AddOrGet<Modifiers>();
      modifiers.initialTraits.Add("BeeHiveBaseTrait");
      modifiers.initialAmounts.Add(Db.Get().Amounts.HitPoints.Id);
      modifiers.initialAttributes.Add(Db.Get().CritterAttributes.Metabolism.Id);
      ChoreTable.Builder chore_table = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def()).Add((StateMachine.BaseDef) new AnimInterruptStates.Def()).Add((StateMachine.BaseDef) new DisabledCreatureStates.Def("inactive")).PushInterruptGroup().Add((StateMachine.BaseDef) new HiveGrowingStates.Def()).Add((StateMachine.BaseDef) new HiveHarvestStates.Def()).Add((StateMachine.BaseDef) new HiveEatingStates.Def(BeeHiveTuning.CONSUMED_ORE)).PopInterruptGroup().Add((StateMachine.BaseDef) new IdleStandStillStates.Def());
      EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.BeetaSpecies, (string) null);
    }
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
