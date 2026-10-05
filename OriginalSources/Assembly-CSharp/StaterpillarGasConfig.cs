// Decompiled with JetBrains decompiler
// Type: StaterpillarGasConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class StaterpillarGasConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "StaterpillarGas";
  public const string BASE_TRAIT_ID = "StaterpillarGasBaseTrait";
  public const string EGG_ID = "StaterpillarGasEgg";
  public const int EGG_SORT_ORDER = 1;
  private static float KG_ORE_EATEN_PER_CYCLE = 30f;
  private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarGasConfig.KG_ORE_EATEN_PER_CYCLE;
  private static float STORAGE_CAPACITY = 100f;
  private static float COOLDOWN_MIN = 20f;
  private static float COOLDOWN_MAX = 40f;
  private static float CONSUMPTION_RATE = 0.5f;
  private static float INHALE_TIME = 6f;

  public static GameObject CreateStaterpillarGas(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    InhaleStates.Def inhaleDef = new InhaleStates.Def()
    {
      behaviourTag = GameTags.Creatures.WantsToStore,
      inhaleAnimPre = "gas_consume_pre",
      inhaleAnimLoop = "gas_consume_loop",
      inhaleAnimPst = "gas_consume_pst",
      useStorage = true,
      alwaysPlayPstAnim = true,
      inhaleTime = StaterpillarGasConfig.INHALE_TIME,
      storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas
    };
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarGasBaseTrait", is_baby, ObjectLayer.GasConduit, StaterpillarGasConnectorConfig.ID, GameTags.Unbreathable, "gas_", 263.15f, 313.15f, 173.15f, 373.15f, inhaleDef), TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
    if (!is_baby)
    {
      GasAndLiquidConsumerMonitor.Def def = wildCreature.AddOrGetDef<GasAndLiquidConsumerMonitor.Def>();
      def.behaviourTag = GameTags.Creatures.WantsToStore;
      def.consumableElementTag = GameTags.Unbreathable;
      def.transitionTag = new Tag[1]{ GameTags.Creature };
      def.minCooldown = StaterpillarGasConfig.COOLDOWN_MIN;
      def.maxCooldown = StaterpillarGasConfig.COOLDOWN_MAX;
      def.consumptionRate = StaterpillarGasConfig.CONSUMPTION_RATE;
    }
    Trait trait = Db.Get().CreateTrait("StaterpillarGasBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), (string) UI.TOOLTIPS.BASE_VALUE));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name));
    List<Diet.Info> diet_infos = new List<Diet.Info>();
    diet_infos.AddRange((IEnumerable<Diet.Info>) BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarGasConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, (string) null, 0.0f));
    diet_infos.AddRange((IEnumerable<Diet.Info>) BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarGasConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, (string) null, 0.0f));
    GameObject staterpillarGas = BaseStaterpillarConfig.SetupDiet(wildCreature, diet_infos);
    Storage storage = staterpillarGas.AddComponent<Storage>();
    storage.capacityKg = StaterpillarGasConfig.STORAGE_CAPACITY;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    return staterpillarGas;
  }

  public string[] GetRequiredDlcIds() => DlcManager.EXPANSION1;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public virtual GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarGasConfig.CreateStaterpillarGas("StaterpillarGas", (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.NAME, (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "caterpillar_kanim", false), (IHasDlcRestrictions) this, "StaterpillarGasEgg", (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.STATERPILLAR.VARIANT_GAS.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarGasBaby", 60.0000038f, 20f, StaterpillarTuning.EGG_CHANCES_GAS, 1);
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
