// Decompiled with JetBrains decompiler
// Type: TUNING.TRAITS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace TUNING;

public class TRAITS
{
  public static float EARLYBIRD_MODIFIER = 2f;
  public static int EARLYBIRD_SCHEDULEBLOCK = 5;
  public static float NIGHTOWL_MODIFIER = 3f;
  public static float METEORPHILE_MODIFIER = 3f;
  public const float FLATULENCE_EMIT_MASS = 0.1f;
  public static float FLATULENCE_EMIT_INTERVAL_MIN = 10f;
  public static float FLATULENCE_EMIT_INTERVAL_MAX = 40f;
  public static float STINKY_EMIT_INTERVAL_MIN = 10f;
  public static float STINKY_EMIT_INTERVAL_MAX = 30f;
  public static float NARCOLEPSY_INTERVAL_MIN = 300f;
  public static float NARCOLEPSY_INTERVAL_MAX = 600f;
  public static float NARCOLEPSY_SLEEPDURATION_MIN = 15f;
  public static float NARCOLEPSY_SLEEPDURATION_MAX = 30f;
  public const float INTERRUPTED_SLEEP_STRESS_DELTA = 10f;
  public const float INTERRUPTED_SLEEP_ATHLETICS_DELTA = -2f;
  public static int NO_ATTRIBUTE_BONUS = 0;
  public static int GOOD_ATTRIBUTE_BONUS = 3;
  public static int GREAT_ATTRIBUTE_BONUS = 5;
  public static int BAD_ATTRIBUTE_PENALTY = -3;
  public static int HORRIBLE_ATTRIBUTE_PENALTY = -5;
  public static float GLOWSTICK_LUX_VALUE = 500f;
  public static float GLOWSTICK_RADIATION_RESISTANCE = 0.33f;
  public static float RADIATION_EATER_RECOVERY = -0.25f;
  public static float RADS_TO_CALS = 333.33f;
  private const string MINNOW_ID = "Minnow";
  public const float MINNOW_SWIMMING_ATTRIBUTE_BONUS = 4f;
  public static readonly List<System.Action> TRAIT_CREATORS = new List<System.Action>()
  {
    TraitUtil.CreateAttributeEffectTrait("None", (string) DUPLICANTS.CONGENITALTRAITS.NONE.NAME, (string) DUPLICANTS.CONGENITALTRAITS.NONE.DESC, "", (float) TRAITS.NO_ATTRIBUTE_BONUS),
    TraitUtil.CreateComponentTrait<Stinky>("Stinky", (string) DUPLICANTS.CONGENITALTRAITS.STINKY.NAME, (string) DUPLICANTS.CONGENITALTRAITS.STINKY.DESC),
    TraitUtil.CreateAttributeEffectTrait("Ellie", (string) DUPLICANTS.CONGENITALTRAITS.ELLIE.NAME, (string) DUPLICANTS.CONGENITALTRAITS.ELLIE.DESC, "AirConsumptionRate", (float) (-(double) DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * 0.44999998807907104), "DecorExpectation", -5f),
    TraitUtil.CreateDisabledTaskTrait("Joshua", (string) DUPLICANTS.CONGENITALTRAITS.JOSHUA.NAME, (string) DUPLICANTS.CONGENITALTRAITS.JOSHUA.DESC, "Combat", true),
    TraitUtil.CreateComponentTrait<Stinky>("Liam", (string) DUPLICANTS.CONGENITALTRAITS.LIAM.NAME, (string) DUPLICANTS.CONGENITALTRAITS.LIAM.DESC),
    TraitUtil.CreateAttributeEffectTrait("Minnow", (string) DUPLICANTS.CONGENITALTRAITS.MINNOW.NAME, (string) DUPLICANTS.CONGENITALTRAITS.MINNOW.DESC, new string[2]
    {
      "BreathMax",
      "ThermalConductivityBarrier"
    }, new float[2]
    {
      DUPLICANTSTATS.STANDARD.Breath.MINNOW_LUNG_CAPACITY_BONUS,
      0.008f
    }, true, (Action<GameObject>) (go =>
    {
      MinionResume component1;
      if (go.TryGetComponent<MinionResume>(out component1) && !component1.HasBeenGrantedSkill("Swimming"))
      {
        component1.GrantSkill("Swimming");
        AmountInstance amountInstance = Db.Get().Amounts.Breath.Lookup(go);
        if (amountInstance != null)
        {
          double num = (double) amountInstance.SetValue(amountInstance.GetMax());
        }
      }
      go.AddOrGet<MinnowSwimmer>();
      Effects component2;
      if (!go.TryGetComponent<Effects>(out component2))
        return;
      component2.AddImmunity(Db.Get().effects.Get("SoakingWet"), "Minnow");
      component2.AddImmunity(Db.Get().effects.Get("WetFeet"), "Minnow");
    })),
    TraitUtil.CreateNamedTrait("AncientKnowledge", (string) DUPLICANTS.TRAITS.ANCIENTKNOWLEDGE.NAME, (string) DUPLICANTS.TRAITS.ANCIENTKNOWLEDGE.DESC, true),
    TraitUtil.CreateAttributeEffectTrait("BionicBug1", (string) DUPLICANTS.TRAITS.BIONICBUG1.NAME, (string) DUPLICANTS.TRAITS.BIONICBUG1.DESC, new string[2]
    {
      "Learning",
      "Strength"
    }, new float[2]{ -3f, -3f }, false),
    TraitUtil.CreateAttributeEffectTrait("BionicBug2", (string) DUPLICANTS.TRAITS.BIONICBUG2.NAME, (string) DUPLICANTS.TRAITS.BIONICBUG2.DESC, new string[2]
    {
      "Ranching",
      "Caring"
    }, new float[2]{ -3f, -3f }, false),
    TraitUtil.CreateAttributeEffectTrait("BionicBug3", (string) DUPLICANTS.TRAITS.BIONICBUG3.NAME, (string) DUPLICANTS.TRAITS.BIONICBUG3.DESC, new string[2]
    {
      "Digging",
      "Machinery"
    }, new float[2]{ -3f, -3f }, false),
    TraitUtil.CreateAttributeEffectTrait("BionicBug4", (string) DUPLICANTS.TRAITS.BIONICBUG4.NAME, (string) DUPLICANTS.TRAITS.BIONICBUG4.DESC, new string[2]
    {
      "Construction",
      "Art"
    }, new float[2]{ -3f, -3f }, false),
    TraitUtil.CreateAttributeEffectTrait("BionicBug5", (string) DUPLICANTS.TRAITS.BIONICBUG5.NAME, (string) DUPLICANTS.TRAITS.BIONICBUG5.DESC, new string[2]
    {
      "Athletics",
      "Botanist"
    }, new float[2]{ -3f, -3f }, false),
    TraitUtil.CreateAttributeEffectTrait("BionicBug6", (string) DUPLICANTS.TRAITS.BIONICBUG6.NAME, (string) DUPLICANTS.TRAITS.BIONICBUG6.DESC, new string[2]
    {
      "Cooking",
      "Botanist"
    }, new float[2]{ -3f, -3f }, false),
    TraitUtil.CreateAttributeEffectTrait("BionicBug7", (string) DUPLICANTS.TRAITS.BIONICBUG7.NAME, (string) DUPLICANTS.TRAITS.BIONICBUG7.DESC, new string[2]
    {
      "Caring",
      "Learning"
    }, new float[2]{ -3f, -3f }, false),
    TraitUtil.CreateComponentTrait<Chatty>("Chatty", (string) DUPLICANTS.TRAITS.CHATTY.NAME, (string) DUPLICANTS.TRAITS.CHATTY.DESC, true),
    TraitUtil.CreateDisabledTaskTrait("CantResearch", (string) DUPLICANTS.TRAITS.CANTRESEARCH.NAME, (string) DUPLICANTS.TRAITS.CANTRESEARCH.DESC, "Research", true),
    TraitUtil.CreateDisabledTaskTrait("CantBuild", (string) DUPLICANTS.TRAITS.CANTBUILD.NAME, (string) DUPLICANTS.TRAITS.CANTBUILD.DESC, "Build", false),
    TraitUtil.CreateDisabledTaskTrait("CantCook", (string) DUPLICANTS.TRAITS.CANTCOOK.NAME, (string) DUPLICANTS.TRAITS.CANTCOOK.DESC, "Cook", true),
    TraitUtil.CreateDisabledTaskTrait("CantDig", (string) DUPLICANTS.TRAITS.CANTDIG.NAME, (string) DUPLICANTS.TRAITS.CANTDIG.DESC, "Dig", false),
    TraitUtil.CreateDisabledTaskTrait("Hemophobia", (string) DUPLICANTS.TRAITS.HEMOPHOBIA.NAME, (string) DUPLICANTS.TRAITS.HEMOPHOBIA.DESC, "MedicalAid", true),
    TraitUtil.CreateDisabledTaskTrait("ScaredyCat", (string) DUPLICANTS.TRAITS.SCAREDYCAT.NAME, (string) DUPLICANTS.TRAITS.SCAREDYCAT.DESC, "Combat", true),
    TraitUtil.CreateNamedTrait("Allergies", (string) DUPLICANTS.TRAITS.ALLERGIES.NAME, (string) DUPLICANTS.TRAITS.ALLERGIES.DESC),
    TraitUtil.CreateNamedTrait("NightLight", (string) DUPLICANTS.TRAITS.NIGHTLIGHT.NAME, (string) DUPLICANTS.TRAITS.NIGHTLIGHT.DESC),
    TraitUtil.CreateAttributeEffectTrait("MouthBreather", (string) DUPLICANTS.TRAITS.MOUTHBREATHER.NAME, (string) DUPLICANTS.TRAITS.MOUTHBREATHER.DESC, "AirConsumptionRate", DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND),
    TraitUtil.CreateAttributeEffectTrait("CalorieBurner", (string) DUPLICANTS.TRAITS.CALORIEBURNER.NAME, (string) DUPLICANTS.TRAITS.CALORIEBURNER.DESC, "CaloriesDelta", DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 0.5f),
    TraitUtil.CreateAttributeEffectTrait("SmallBladder", (string) DUPLICANTS.TRAITS.SMALLBLADDER.NAME, (string) DUPLICANTS.TRAITS.SMALLBLADDER.DESC, "BladderDelta", DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND / 600f),
    TraitUtil.CreateAttributeEffectTrait("Anemic", (string) DUPLICANTS.TRAITS.ANEMIC.NAME, (string) DUPLICANTS.TRAITS.ANEMIC.DESC, "Athletics", (float) TRAITS.HORRIBLE_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("SlowLearner", (string) DUPLICANTS.TRAITS.SLOWLEARNER.NAME, (string) DUPLICANTS.TRAITS.SLOWLEARNER.DESC, "Learning", (float) TRAITS.BAD_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("NoodleArms", (string) DUPLICANTS.TRAITS.NOODLEARMS.NAME, (string) DUPLICANTS.TRAITS.NOODLEARMS.DESC, "Strength", (float) TRAITS.BAD_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("ConstructionDown", (string) DUPLICANTS.TRAITS.CONSTRUCTIONDOWN.NAME, (string) DUPLICANTS.TRAITS.CONSTRUCTIONDOWN.DESC, "Construction", (float) TRAITS.BAD_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("RanchingDown", (string) DUPLICANTS.TRAITS.RANCHINGDOWN.NAME, (string) DUPLICANTS.TRAITS.RANCHINGDOWN.DESC, "Ranching", (float) TRAITS.BAD_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("DiggingDown", (string) DUPLICANTS.TRAITS.DIGGINGDOWN.NAME, (string) DUPLICANTS.TRAITS.DIGGINGDOWN.DESC, "Digging", (float) TRAITS.BAD_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("MachineryDown", (string) DUPLICANTS.TRAITS.MACHINERYDOWN.NAME, (string) DUPLICANTS.TRAITS.MACHINERYDOWN.DESC, "Machinery", (float) TRAITS.BAD_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("CookingDown", (string) DUPLICANTS.TRAITS.COOKINGDOWN.NAME, (string) DUPLICANTS.TRAITS.COOKINGDOWN.DESC, "Cooking", (float) TRAITS.BAD_ATTRIBUTE_PENALTY, "FoodExpectation", 1f),
    TraitUtil.CreateAttributeEffectTrait("ArtDown", (string) DUPLICANTS.TRAITS.ARTDOWN.NAME, (string) DUPLICANTS.TRAITS.ARTDOWN.DESC, "Art", (float) TRAITS.BAD_ATTRIBUTE_PENALTY, "DecorExpectation", 5f),
    TraitUtil.CreateAttributeEffectTrait("CaringDown", (string) DUPLICANTS.TRAITS.CARINGDOWN.NAME, (string) DUPLICANTS.TRAITS.CARINGDOWN.DESC, "Caring", (float) TRAITS.BAD_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("BotanistDown", (string) DUPLICANTS.TRAITS.BOTANISTDOWN.NAME, (string) DUPLICANTS.TRAITS.BOTANISTDOWN.DESC, "Botanist", (float) TRAITS.BAD_ATTRIBUTE_PENALTY),
    TraitUtil.CreateAttributeEffectTrait("DecorDown", (string) DUPLICANTS.TRAITS.DECORDOWN.NAME, (string) DUPLICANTS.TRAITS.DECORDOWN.DESC, "Decor", (float) BUILDINGS.DECOR.PENALTY.TIER2.amount),
    TraitUtil.CreateAttributeEffectTrait("Regeneration", (string) DUPLICANTS.TRAITS.REGENERATION.NAME, (string) DUPLICANTS.TRAITS.REGENERATION.DESC, "HitPointsDelta", 0.0333333351f),
    TraitUtil.CreateAttributeEffectTrait("DeeperDiversLungs", (string) DUPLICANTS.TRAITS.DEEPERDIVERSLUNGS.NAME, (string) DUPLICANTS.TRAITS.DEEPERDIVERSLUNGS.DESC, "AirConsumptionRate", (float) (-(double) DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * 0.5)),
    TraitUtil.CreateAttributeEffectTrait("SunnyDisposition", (string) DUPLICANTS.TRAITS.SUNNYDISPOSITION.NAME, (string) DUPLICANTS.TRAITS.SUNNYDISPOSITION.DESC, "StressDelta", -0.0333333351f, on_add: (Action<GameObject>) (go => go.GetComponent<KBatchedAnimController>().AddAnimOverrides(Assets.GetAnim((HashedString) "anim_loco_happy_kanim")))),
    TraitUtil.CreateAttributeEffectTrait("RockCrusher", (string) DUPLICANTS.TRAITS.ROCKCRUSHER.NAME, (string) DUPLICANTS.TRAITS.ROCKCRUSHER.DESC, "Strength", 10f),
    TraitUtil.CreateTrait("Uncultured", (string) DUPLICANTS.TRAITS.UNCULTURED.NAME, (string) DUPLICANTS.TRAITS.UNCULTURED.DESC, "DecorExpectation", 20f, new string[1]
    {
      "Art"
    }, true),
    TraitUtil.CreateNamedTrait("Archaeologist", (string) DUPLICANTS.TRAITS.ARCHAEOLOGIST.NAME, (string) DUPLICANTS.TRAITS.ARCHAEOLOGIST.DESC),
    TraitUtil.CreateAttributeEffectTrait("WeakImmuneSystem", (string) DUPLICANTS.TRAITS.WEAKIMMUNESYSTEM.NAME, (string) DUPLICANTS.TRAITS.WEAKIMMUNESYSTEM.DESC, "GermResistance", -1f),
    TraitUtil.CreateAttributeEffectTrait("IrritableBowel", (string) DUPLICANTS.TRAITS.IRRITABLEBOWEL.NAME, (string) DUPLICANTS.TRAITS.IRRITABLEBOWEL.DESC, "ToiletEfficiency", (float) (-(double) DUPLICANTSTATS.STANDARD.BaseStats.TOILET_EFFICIENCY * 0.5)),
    TraitUtil.CreateComponentTrait<Flatulence>("Flatulence", (string) DUPLICANTS.TRAITS.FLATULENCE.NAME, (string) DUPLICANTS.TRAITS.FLATULENCE.DESC),
    TraitUtil.CreateComponentTrait<Snorer>("Snorer", (string) DUPLICANTS.TRAITS.SNORER.NAME, (string) DUPLICANTS.TRAITS.SNORER.DESC),
    TraitUtil.CreateComponentTrait<Narcolepsy>("Narcolepsy", (string) DUPLICANTS.TRAITS.NARCOLEPSY.NAME, (string) DUPLICANTS.TRAITS.NARCOLEPSY.DESC),
    TraitUtil.CreateComponentTrait<Thriver>("Thriver", (string) DUPLICANTS.TRAITS.THRIVER.NAME, (string) DUPLICANTS.TRAITS.THRIVER.DESC, true),
    TraitUtil.CreateComponentTrait<Loner>("Loner", (string) DUPLICANTS.TRAITS.LONER.NAME, (string) DUPLICANTS.TRAITS.LONER.DESC, true),
    TraitUtil.CreateComponentTrait<StarryEyed>("StarryEyed", (string) DUPLICANTS.TRAITS.STARRYEYED.NAME, (string) DUPLICANTS.TRAITS.STARRYEYED.DESC, true),
    TraitUtil.CreateComponentTrait<GlowStick>("GlowStick", (string) DUPLICANTS.TRAITS.GLOWSTICK.NAME, (string) DUPLICANTS.TRAITS.GLOWSTICK.DESC, true),
    TraitUtil.CreateComponentTrait<RadiationEater>("RadiationEater", (string) DUPLICANTS.TRAITS.RADIATIONEATER.NAME, (string) DUPLICANTS.TRAITS.RADIATIONEATER.DESC, true),
    TraitUtil.CreateAttributeEffectTrait("Twinkletoes", (string) DUPLICANTS.TRAITS.TWINKLETOES.NAME, (string) DUPLICANTS.TRAITS.TWINKLETOES.DESC, "Athletics", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateAttributeEffectTrait("Greasemonkey", (string) DUPLICANTS.TRAITS.GREASEMONKEY.NAME, (string) DUPLICANTS.TRAITS.GREASEMONKEY.DESC, "Machinery", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateAttributeEffectTrait("MoleHands", (string) DUPLICANTS.TRAITS.MOLEHANDS.NAME, (string) DUPLICANTS.TRAITS.MOLEHANDS.DESC, "Digging", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateAttributeEffectTrait("FastLearner", (string) DUPLICANTS.TRAITS.FASTLEARNER.NAME, (string) DUPLICANTS.TRAITS.FASTLEARNER.DESC, "Learning", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateAttributeEffectTrait("DiversLung", (string) DUPLICANTS.TRAITS.DIVERSLUNG.NAME, (string) DUPLICANTS.TRAITS.DIVERSLUNG.DESC, "AirConsumptionRate", (float) (-(double) DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * 0.25), true),
    TraitUtil.CreateAttributeEffectTrait("StrongArm", (string) DUPLICANTS.TRAITS.STRONGARM.NAME, (string) DUPLICANTS.TRAITS.STRONGARM.DESC, "Strength", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateAttributeEffectTrait("DecorUp", (string) DUPLICANTS.TRAITS.DECORUP.NAME, (string) DUPLICANTS.TRAITS.DECORUP.DESC, "Decor", (float) BUILDINGS.DECOR.BONUS.TIER4.amount, true),
    TraitUtil.CreateAttributeEffectTrait("GreenThumb", (string) DUPLICANTS.TRAITS.GREENTHUMB.NAME, (string) DUPLICANTS.TRAITS.GREENTHUMB.DESC, "Botanist", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateAttributeEffectTrait("InteriorDecorator", (string) DUPLICANTS.TRAITS.INTERIORDECORATOR.NAME, (string) DUPLICANTS.TRAITS.INTERIORDECORATOR.DESC, "Art", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, "DecorExpectation", -5f, true),
    TraitUtil.CreateAttributeEffectTrait("SimpleTastes", (string) DUPLICANTS.TRAITS.SIMPLETASTES.NAME, (string) DUPLICANTS.TRAITS.SIMPLETASTES.DESC, "FoodExpectation", 1f, true),
    TraitUtil.CreateAttributeEffectTrait("Foodie", (string) DUPLICANTS.TRAITS.FOODIE.NAME, (string) DUPLICANTS.TRAITS.FOODIE.DESC, "Cooking", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, "FoodExpectation", -1f, true),
    TraitUtil.CreateAttributeEffectTrait("BedsideManner", (string) DUPLICANTS.TRAITS.BEDSIDEMANNER.NAME, (string) DUPLICANTS.TRAITS.BEDSIDEMANNER.DESC, "Caring", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateAttributeEffectTrait("ConstructionUp", (string) DUPLICANTS.TRAITS.CONSTRUCTIONUP.NAME, (string) DUPLICANTS.TRAITS.CONSTRUCTIONUP.DESC, "Construction", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateAttributeEffectTrait("RanchingUp", (string) DUPLICANTS.TRAITS.RANCHINGUP.NAME, (string) DUPLICANTS.TRAITS.RANCHINGUP.DESC, "Ranching", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true),
    TraitUtil.CreateEffectModifierTrait("FrostProof", (string) DUPLICANTS.TRAITS.FROSTPROOF.NAME, (string) DUPLICANTS.TRAITS.FROSTPROOF.DESC, new string[1]
    {
      "ColdAir"
    }, true),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Mining1", (string) DUPLICANTS.TRAITS.GRANTSKILL_MINING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_MINING1.DESC, "Mining1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Mining2", (string) DUPLICANTS.TRAITS.GRANTSKILL_MINING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_MINING2.DESC, "Mining2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Mining3", (string) DUPLICANTS.TRAITS.GRANTSKILL_MINING3.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_MINING3.DESC, "Mining3"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Mining4", (string) DUPLICANTS.TRAITS.GRANTSKILL_MINING4.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_MINING4.DESC, "Mining4"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Building1", (string) DUPLICANTS.TRAITS.GRANTSKILL_BUILDING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_BUILDING1.DESC, "Building1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Building2", (string) DUPLICANTS.TRAITS.GRANTSKILL_BUILDING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_BUILDING2.DESC, "Building2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Building3", (string) DUPLICANTS.TRAITS.GRANTSKILL_BUILDING3.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_BUILDING3.DESC, "Building3"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Farming1", (string) DUPLICANTS.TRAITS.GRANTSKILL_FARMING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_FARMING1.DESC, "Farming1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Farming2", (string) DUPLICANTS.TRAITS.GRANTSKILL_FARMING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_FARMING2.DESC, "Farming2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Farming3", (string) DUPLICANTS.TRAITS.GRANTSKILL_FARMING3.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_FARMING3.DESC, "Farming3"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Ranching1", (string) DUPLICANTS.TRAITS.GRANTSKILL_RANCHING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_RANCHING1.DESC, "Ranching1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Ranching2", (string) DUPLICANTS.TRAITS.GRANTSKILL_RANCHING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_RANCHING2.DESC, "Ranching2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Researching1", (string) DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING1.DESC, "Researching1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Researching2", (string) DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING2.DESC, "Researching2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Researching3", (string) DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING3.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING3.DESC, "Researching3"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Researching4", (string) DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING4.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_RESEARCHING4.DESC, "Researching4"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Cooking1", (string) DUPLICANTS.TRAITS.GRANTSKILL_COOKING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_COOKING1.DESC, "Cooking1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Cooking2", (string) DUPLICANTS.TRAITS.GRANTSKILL_COOKING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_COOKING2.DESC, "Cooking2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Arting1", (string) DUPLICANTS.TRAITS.GRANTSKILL_ARTING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_ARTING1.DESC, "Arting1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Arting2", (string) DUPLICANTS.TRAITS.GRANTSKILL_ARTING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_ARTING2.DESC, "Arting2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Arting3", (string) DUPLICANTS.TRAITS.GRANTSKILL_ARTING3.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_ARTING3.DESC, "Arting3"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Hauling1", (string) DUPLICANTS.TRAITS.GRANTSKILL_HAULING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_HAULING1.DESC, "Hauling1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Hauling2", (string) DUPLICANTS.TRAITS.GRANTSKILL_HAULING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_HAULING2.DESC, "Hauling2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Suits1", (string) DUPLICANTS.TRAITS.GRANTSKILL_SUITS1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_SUITS1.DESC, "Suits1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Technicals1", (string) DUPLICANTS.TRAITS.GRANTSKILL_TECHNICALS1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_TECHNICALS1.DESC, "Technicals1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Technicals2", (string) DUPLICANTS.TRAITS.GRANTSKILL_TECHNICALS2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_TECHNICALS2.DESC, "Technicals2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Engineering1", (string) DUPLICANTS.TRAITS.GRANTSKILL_ENGINEERING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_ENGINEERING1.DESC, "Engineering1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Basekeeping1", (string) DUPLICANTS.TRAITS.GRANTSKILL_BASEKEEPING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_BASEKEEPING1.DESC, "Basekeeping1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Basekeeping2", (string) DUPLICANTS.TRAITS.GRANTSKILL_BASEKEEPING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_BASEKEEPING2.DESC, "Basekeeping2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Astronauting1", (string) DUPLICANTS.TRAITS.GRANTSKILL_ASTRONAUTING1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_ASTRONAUTING1.DESC, "Astronauting1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Astronauting2", (string) DUPLICANTS.TRAITS.GRANTSKILL_ASTRONAUTING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_ASTRONAUTING2.DESC, "Astronauting2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Medicine1", (string) DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE1.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE1.DESC, "Medicine1"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Medicine2", (string) DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE2.DESC, "Medicine2"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Medicine3", (string) DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE3.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_MEDICINE3.DESC, "Medicine3"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Pyrotechnics", (string) DUPLICANTS.TRAITS.GRANTSKILL_PYROTECHNICS.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_PYROTECHNICS.DESC, "Pyrotechnics"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Swimming", (string) DUPLICANTS.TRAITS.GRANTSKILL_SWIMMING.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_SWIMMING.DESC, "Swimming"),
    TraitUtil.CreateSkillGrantingTrait("GrantSkill_Swimming2", (string) DUPLICANTS.TRAITS.GRANTSKILL_SWIMMING2.NAME, (string) DUPLICANTS.TRAITS.GRANTSKILL_SWIMMING2.DESC, new string[2]
    {
      "Swimming",
      "Swimming2"
    }),
    TraitUtil.CreateNamedTrait("IronGut", (string) DUPLICANTS.TRAITS.IRONGUT.NAME, (string) DUPLICANTS.TRAITS.IRONGUT.DESC, true),
    TraitUtil.CreateAttributeEffectTrait("StrongImmuneSystem", (string) DUPLICANTS.TRAITS.STRONGIMMUNESYSTEM.NAME, (string) DUPLICANTS.TRAITS.STRONGIMMUNESYSTEM.DESC, "GermResistance", 1f, true),
    TraitUtil.CreateTrait("Aggressive", (string) DUPLICANTS.TRAITS.AGGRESSIVE.NAME, (string) DUPLICANTS.TRAITS.AGGRESSIVE.DESC, new Action<GameObject>(TRAITS.OnAddAggressive), extendedDescFn: (Func<string>) (() => (string) DUPLICANTS.TRAITS.AGGRESSIVE.NOREPAIR)),
    TraitUtil.CreateTrait("UglyCrier", (string) DUPLICANTS.TRAITS.UGLYCRIER.NAME, (string) DUPLICANTS.TRAITS.UGLYCRIER.DESC, new Action<GameObject>(TRAITS.OnAddUglyCrier)),
    TraitUtil.CreateTrait("BingeEater", (string) DUPLICANTS.TRAITS.BINGEEATER.NAME, (string) DUPLICANTS.TRAITS.BINGEEATER.DESC, new Action<GameObject>(TRAITS.OnAddBingeEater)),
    TraitUtil.CreateTrait("StressVomiter", (string) DUPLICANTS.TRAITS.STRESSVOMITER.NAME, (string) DUPLICANTS.TRAITS.STRESSVOMITER.DESC, new Action<GameObject>(TRAITS.OnAddStressVomiter)),
    TraitUtil.CreateTrait("Banshee", (string) DUPLICANTS.TRAITS.BANSHEE.NAME, (string) DUPLICANTS.TRAITS.BANSHEE.DESC, new Action<GameObject>(TRAITS.OnAddBanshee)),
    TraitUtil.CreateTrait("StressShocker", (string) DUPLICANTS.TRAITS.STRESSSHOCKER.NAME, (string) DUPLICANTS.TRAITS.STRESSSHOCKER.DESC, new Action<GameObject>(TRAITS.OnAddShocker), DlcManager.DLC3),
    TraitUtil.CreateTrait("BalloonArtist", (string) DUPLICANTS.TRAITS.BALLOONARTIST.NAME, (string) DUPLICANTS.TRAITS.BALLOONARTIST.DESC, new Action<GameObject>(TRAITS.OnAddBalloonArtist)),
    TraitUtil.CreateTrait("SparkleStreaker", (string) DUPLICANTS.TRAITS.SPARKLESTREAKER.NAME, (string) DUPLICANTS.TRAITS.SPARKLESTREAKER.DESC, new Action<GameObject>(TRAITS.OnAddSparkleStreaker)),
    TraitUtil.CreateTrait("StickerBomber", (string) DUPLICANTS.TRAITS.STICKERBOMBER.NAME, (string) DUPLICANTS.TRAITS.STICKERBOMBER.DESC, new Action<GameObject>(TRAITS.OnAddStickerBomber)),
    TraitUtil.CreateTrait("SuperProductive", (string) DUPLICANTS.TRAITS.SUPERPRODUCTIVE.NAME, (string) DUPLICANTS.TRAITS.SUPERPRODUCTIVE.DESC, new Action<GameObject>(TRAITS.OnAddSuperProductive)),
    TraitUtil.CreateTrait("HappySinger", (string) DUPLICANTS.TRAITS.HAPPYSINGER.NAME, (string) DUPLICANTS.TRAITS.HAPPYSINGER.DESC, new Action<GameObject>(TRAITS.OnAddHappySinger)),
    TraitUtil.CreateTrait("DataRainer", (string) DUPLICANTS.TRAITS.DATARAINER.NAME, (string) DUPLICANTS.TRAITS.DATARAINER.DESC, new Action<GameObject>(TRAITS.OnAddDataRainer), DlcManager.DLC3),
    TraitUtil.CreateTrait("RoboDancer", (string) DUPLICANTS.TRAITS.ROBODANCER.NAME, (string) DUPLICANTS.TRAITS.ROBODANCER.DESC, new Action<GameObject>(TRAITS.OnAddRoboDancer), DlcManager.DLC3),
    TraitUtil.CreateComponentTrait<EarlyBird>("EarlyBird", (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, (string) DUPLICANTS.TRAITS.EARLYBIRD.DESC, true, (Func<string>) (() => string.Format((string) DUPLICANTS.TRAITS.EARLYBIRD.EXTENDED_DESC, (object) GameUtil.AddPositiveSign(TRAITS.EARLYBIRD_MODIFIER.ToString(), true)))),
    TraitUtil.CreateComponentTrait<NightOwl>("NightOwl", (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, (string) DUPLICANTS.TRAITS.NIGHTOWL.DESC, true, (Func<string>) (() => string.Format((string) DUPLICANTS.TRAITS.NIGHTOWL.EXTENDED_DESC, (object) GameUtil.AddPositiveSign(TRAITS.NIGHTOWL_MODIFIER.ToString(), true)))),
    TraitUtil.CreateComponentTrait<Meteorphile>("Meteorphile", (string) DUPLICANTS.TRAITS.METEORPHILE.NAME, (string) DUPLICANTS.TRAITS.METEORPHILE.DESC, true, (Func<string>) (() => string.Format((string) DUPLICANTS.TRAITS.METEORPHILE.EXTENDED_DESC, (object) GameUtil.AddPositiveSign(TRAITS.METEORPHILE_MODIFIER.ToString(), true)))),
    TraitUtil.CreateComponentTrait<Claustrophobic>("Claustrophobic", (string) DUPLICANTS.TRAITS.NEEDS.CLAUSTROPHOBIC.NAME, (string) DUPLICANTS.TRAITS.NEEDS.CLAUSTROPHOBIC.DESC),
    TraitUtil.CreateComponentTrait<PrefersWarmer>("PrefersWarmer", (string) DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, (string) DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.DESC),
    TraitUtil.CreateComponentTrait<PrefersColder>("PrefersColder", (string) DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (string) DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.DESC),
    TraitUtil.CreateComponentTrait<SensitiveFeet>("SensitiveFeet", (string) DUPLICANTS.TRAITS.NEEDS.SENSITIVEFEET.NAME, (string) DUPLICANTS.TRAITS.NEEDS.SENSITIVEFEET.DESC),
    TraitUtil.CreateComponentTrait<Fashionable>("Fashionable", (string) DUPLICANTS.TRAITS.NEEDS.FASHIONABLE.NAME, (string) DUPLICANTS.TRAITS.NEEDS.FASHIONABLE.DESC),
    TraitUtil.CreateComponentTrait<Climacophobic>("Climacophobic", (string) DUPLICANTS.TRAITS.NEEDS.CLIMACOPHOBIC.NAME, (string) DUPLICANTS.TRAITS.NEEDS.CLIMACOPHOBIC.DESC),
    TraitUtil.CreateComponentTrait<SolitarySleeper>("SolitarySleeper", (string) DUPLICANTS.TRAITS.NEEDS.SOLITARYSLEEPER.NAME, (string) DUPLICANTS.TRAITS.NEEDS.SOLITARYSLEEPER.DESC),
    TraitUtil.CreateComponentTrait<Workaholic>("Workaholic", (string) DUPLICANTS.TRAITS.NEEDS.WORKAHOLIC.NAME, (string) DUPLICANTS.TRAITS.NEEDS.WORKAHOLIC.DESC)
  };

  private static void OnAddStressVomiter(GameObject go)
  {
    Notification notification = new Notification((string) DUPLICANTS.STATUSITEMS.STRESSVOMITING.NOTIFICATION_NAME, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) DUPLICANTS.STATUSITEMS.STRESSVOMITING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)));
    StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalVomiter", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_VOMITER.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_VOMITER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_interrupt_vomiter_kanim", new HashedString[1]
    {
      (HashedString) "interrupt_vomiter"
    }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new VomitChore(Db.Get().ChoreTypes.StressVomit, (IStateMachineTarget) chore_provider, Db.Get().DuplicantStatusItems.Vomiting, notification)), "anim_loco_vomiter_kanim").StartSM();
  }

  private static void OnAddBanshee(GameObject go)
  {
    Notification notification = new Notification((string) DUPLICANTS.MODIFIERS.BANSHEE_WAILING.NOTIFICATION_NAME, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) DUPLICANTS.MODIFIERS.BANSHEE_WAILING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)));
    StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalBanshee", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BANSHEE.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BANSHEE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_interrupt_banshee_kanim", new HashedString[1]
    {
      (HashedString) "interrupt_banshee"
    }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new BansheeChore(Db.Get().ChoreTypes.BansheeWail, (IStateMachineTarget) chore_provider, notification)), "anim_loco_banshee_60_kanim").StartSM();
  }

  private static void OnAddShocker(GameObject go)
  {
    Notification notification = new Notification((string) DUPLICANTS.MODIFIERS.STRESSSHOCKER.NOTIFICATION_NAME, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) DUPLICANTS.MODIFIERS.STRESSSHOCKER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)));
    StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalShocker", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_STRESS_SHOCKER.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_STRESS_SHOCKER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_loco_stressshocker_kanim", new HashedString[1]
    {
      (HashedString) "interrupt_shocker"
    }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressShockChore(Db.Get().ChoreTypes.StressShock, (IStateMachineTarget) chore_provider, notification)), "anim_loco_stressshocker_kanim", 12f).StartSM();
  }

  private static void OnAddAggressive(GameObject go)
  {
    StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalAggresive", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_AGGRESIVE.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_AGGRESIVE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_interrupt_destructive_kanim", new HashedString[1]
    {
      (HashedString) "interrupt_destruct"
    }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new AggressiveChore((IStateMachineTarget) chore_provider)), "anim_loco_destructive_kanim").StartSM();
  }

  private static void OnAddUglyCrier(GameObject go)
  {
    StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalUglyCrier", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_UGLY_CRIER.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_UGLY_CRIER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_cry_kanim", new HashedString[3]
    {
      (HashedString) "working_pre",
      (HashedString) "working_loop",
      (HashedString) "working_pst"
    }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new UglyCryChore(Db.Get().ChoreTypes.UglyCry, (IStateMachineTarget) chore_provider)), "anim_loco_cry_kanim").StartSM();
  }

  private static void OnAddBingeEater(GameObject go)
  {
    StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalBingeEater", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BINGE_EAT.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BINGE_EAT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID);
    new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_interrupt_binge_eat_kanim", new HashedString[1]
    {
      (HashedString) "interrupt_binge_eat"
    }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new BingeEatChore((IStateMachineTarget) chore_provider)), "anim_loco_binge_eat_kanim", 8f).StartSM();
  }

  private static void OnAddBalloonArtist(GameObject go)
  {
    new BalloonArtist.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>()).StartSM();
    new JoyBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), "anim_loco_happy_balloon_kanim", (string) null, Db.Get().Expressions.Balloon).StartSM();
  }

  private static void OnAddSparkleStreaker(GameObject go)
  {
    new SparkleStreaker.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>()).StartSM();
    new JoyBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), "anim_loco_sparkle_kanim", (string) null, Db.Get().Expressions.Sparkle).StartSM();
  }

  private static void OnAddStickerBomber(GameObject go)
  {
    new StickerBomber.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>()).StartSM();
    new JoyBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), "anim_loco_stickers_kanim", (string) null, Db.Get().Expressions.Sticker).StartSM();
  }

  private static void OnAddSuperProductive(GameObject go)
  {
    new SuperProductive.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>()).StartSM();
    new JoyBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), "anim_loco_productive_kanim", "anim_loco_walk_productive_kanim", Db.Get().Expressions.Productive).StartSM();
  }

  private static void OnAddHappySinger(GameObject go)
  {
    new HappySinger.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>()).StartSM();
    new JoyBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), "anim_loco_singer_kanim", (string) null, Db.Get().Expressions.Music).StartSM();
  }

  private static void OnAddDataRainer(GameObject go)
  {
    new DataRainer.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>()).StartSM();
    new JoyBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), "anim_loco_productive_kanim", "anim_loco_walk_productive_kanim", Db.Get().Expressions.Happy).StartSM();
  }

  private static void OnAddRoboDancer(GameObject go)
  {
    new RoboDancer.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>()).StartSM();
    new JoyBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), "anim_loco_run_robotdance_kanim", "anim_loco_walk_robotdance_kanim", Db.Get().Expressions.Happy).StartSM();
  }

  public class JOY_REACTIONS
  {
    public static float MIN_MORALE_EXCESS = 8f;
    public static float MAX_MORALE_EXCESS = 20f;
    public static float MIN_REACTION_CHANCE = 0.0833333358f;
    public static float MAX_REACTION_CHANCE = 0.208333328f;
    public static float JOY_REACTION_DURATION = 1800f;
    public const float CHARISMATIC_CHANCE = 1f;

    public class SUPER_PRODUCTIVE
    {
      public static float INSTANT_SUCCESS_CHANCE = 10f;
    }

    public class BALLOON_ARTIST
    {
      public static float MINIMUM_BALLOON_MOVESPEED = 5f;
      public static int NUM_BALLOONS_TO_GIVE = 4;
    }

    public class STICKER_BOMBER
    {
      public static float TIME_PER_STICKER_BOMB = 150f;
      public static float STICKER_DURATION = 12000f;
    }

    public class DATA_RAINER
    {
      public static int NUM_MICROCHIPS = 10;
    }

    public class ROBO_DANCER
    {
      public static float DANCE_DURATION = 75f;
    }
  }
}
