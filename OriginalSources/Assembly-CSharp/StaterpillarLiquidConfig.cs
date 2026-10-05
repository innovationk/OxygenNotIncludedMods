// Decompiled with JetBrains decompiler
// Type: StaterpillarLiquidConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class StaterpillarLiquidConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "StaterpillarLiquid";
  public const string BASE_TRAIT_ID = "StaterpillarLiquidBaseTrait";
  public const string EGG_ID = "StaterpillarLiquidEgg";
  public const int EGG_SORT_ORDER = 2;
  private static float KG_ORE_EATEN_PER_CYCLE = 30f;
  private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarLiquidConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float STORAGE_CAPACITY = 1000f;
  private static float COOLDOWN_MIN = 20f;
  private static float COOLDOWN_MAX = 40f;
  private static float CONSUMPTION_RATE = 10f;
  private static float INHALE_TIME = 6f;

  public static GameObject CreateStaterpillarLiquid(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    InhaleStates.Def inhaleDef = new InhaleStates.Def()
    {
      behaviourTag = GameTags.Creatures.WantsToStore,
      inhaleAnimPre = "liquid_consume_pre",
      inhaleAnimLoop = "liquid_consume_loop",
      inhaleAnimPst = "liquid_consume_pst",
      useStorage = true,
      alwaysPlayPstAnim = true,
      inhaleTime = StaterpillarLiquidConfig.INHALE_TIME,
      storageStatusItem = Db.Get().CreatureStatusItems.LookingForLiquid
    };
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarLiquidBaseTrait", is_baby, ObjectLayer.LiquidConduit, StaterpillarLiquidConnectorConfig.ID, GameTags.Unbreathable, "wtr_", 263.15f, 313.15f, 173.15f, 373.15f, inhaleDef), TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
    if (!is_baby)
    {
      GasAndLiquidConsumerMonitor.Def def = wildCreature.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
      def.behaviourTag = GameTags.Creatures.WantsToStore;
      def.consumableElementTag = GameTags.Liquid;
      def.transitionTag = new Tag[1]{ GameTags.Creature };
      def.minCooldown = StaterpillarLiquidConfig.COOLDOWN_MIN;
      def.maxCooldown = StaterpillarLiquidConfig.COOLDOWN_MAX;
      def.consumptionRate = StaterpillarLiquidConfig.CONSUMPTION_RATE;
    }
    Trait trait = Db.Get().CreateTrait("StaterpillarLiquidBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    List<Diet.Info> diet_infos = new List<Diet.Info>();
    diet_infos.AddRange((IEnumerable<Diet.Info>) BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarLiquidConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, (string) null, 0.0f));
    diet_infos.AddRange((IEnumerable<Diet.Info>) BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarLiquidConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, (string) null, 0.0f));
    GameObject staterpillarLiquid = BaseStaterpillarConfig.SetupDiet(wildCreature, diet_infos);
    Storage storage = staterpillarLiquid.AddComponent<Storage>();
    storage.capacityKg = StaterpillarLiquidConfig.STORAGE_CAPACITY;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    return staterpillarLiquid;
  }

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public virtual GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarLiquidConfig.CreateStaterpillarLiquid("StaterpillarLiquid", (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.NAME, (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "caterpillar_kanim", false), (IHasDlcRestrictions) this, "StaterpillarLiquidEgg", (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_LIQUID.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarLiquidBaby", 60.0000038f, 20f, StaterpillarTuning.EGG_CHANCES_LIQUID, 2);
  }

  public void OnPrefabInit(GameObject prefab)
  {
    KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
    component.SetSymbolVisiblity((KAnimHashedString) "electric_bolt_c_bloom", false);
    component.SetSymbolVisiblity((KAnimHashedString) "gulp", false);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
