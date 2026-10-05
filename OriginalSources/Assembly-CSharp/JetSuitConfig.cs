// Decompiled with JetBrains decompiler
// Type: JetSuitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class JetSuitConfig : IEquipmentConfig
{
  public const string ID = "Jet_Suit";
  public const string WORN_ID = "Worn_Jet_Suit";
  public static ComplexRecipe recipe;
  private const PathFinder.PotentialPath.Flags suit_flags = PathFinder.PotentialPath.Flags.HasJetPack;
  private AttributeModifier expertAthleticsModifier;

  public EquipmentDef CreateEquipmentDef()
  {
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    AttributeModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_INSULATION, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.THERMAL_CONDUCTIVITY_BARRIER, TUNING.EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(Db.Get().Attributes.Digging.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_DIGGING, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(Db.Get().Attributes.ScaldingThreshold.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCALDING, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    AttributeModifiers.Add(new AttributeModifier(Db.Get().Attributes.ScoldingThreshold.Id, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_SCOLDING, (string) STRINGS.EQUIPMENT.PREFABS.ATMO_SUIT.NAME));
    this.expertAthleticsModifier = new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) -TUNING.EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS, Db.Get().Skills.Suits1.Name);
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Jet_Suit", TUNING.EQUIPMENT.SUITS.SLOT, SimHashes.Steel, (float) TUNING.EQUIPMENT.SUITS.ATMOSUIT_MASS, "suit_jetpack_kanim", "", "body_jetpack_kanim", 6, AttributeModifiers, IsBody: true, additional_tags: new Tag[4]
    {
      GameTags.Suit,
      GameTags.Clothes,
      GameTags.PedestalDisplayable,
      GameTags.AirtightSuit
    }, RecipeTechUnlock: "JetSuit");
    equipmentDef.wornID = "Worn_Jet_Suit";
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.JET_SUIT.RECIPE_DESC;
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("SoakingWet"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WetFeet"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("ColdAir"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("WarmAir"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("PoppedEarDrums"));
    equipmentDef.EffectImmunites.Add(Db.Get().effects.Get("RecentlySlippedTracker"));
    equipmentDef.OnEquipCallBack = (Action<Equippable>) (eq =>
    {
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      Navigator component1 = targetGameObject.GetComponent<Navigator>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetFlags(PathFinder.PotentialPath.Flags.HasJetPack);
      MinionResume component2 = targetGameObject.GetComponent<MinionResume>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.HasPerk((HashedString) Db.Get().SkillPerks.ExosuitExpertise.Id))
        targetGameObject.GetAttributes().Get(Db.Get().Attributes.Athletics).Add(this.expertAthleticsModifier);
      KAnimControllerBase component3 = targetGameObject.GetComponent<KAnimControllerBase>();
      if ((bool) (UnityEngine.Object) component3)
        component3.AddAnimOverrides(Assets.GetAnim((HashedString) "anim_loco_hover_kanim"));
      targetGameObject.AddTag(GameTags.HasAirtightSuit);
    });
    equipmentDef.OnUnequipCallBack = (Action<Equippable>) (eq =>
    {
      if (eq.assignee == null)
        return;
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!(bool) (UnityEngine.Object) soleOwner)
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if ((bool) (UnityEngine.Object) targetGameObject)
      {
        targetGameObject.GetAttributes()?.Get(Db.Get().Attributes.Athletics).Remove(this.expertAthleticsModifier);
        Navigator component4 = targetGameObject.GetComponent<Navigator>();
        if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
          component4.ClearFlags(PathFinder.PotentialPath.Flags.HasJetPack);
        KAnimControllerBase component5 = targetGameObject.GetComponent<KAnimControllerBase>();
        if ((bool) (UnityEngine.Object) component5)
          component5.RemoveAnimOverrides(Assets.GetAnim((HashedString) "anim_loco_hover_kanim"));
        Effects component6 = targetGameObject.GetComponent<Effects>();
        if ((UnityEngine.Object) component6 != (UnityEngine.Object) null && component6.HasEffect("SoiledSuit"))
          component6.Remove("SoiledSuit");
        targetGameObject.RemoveTag(GameTags.HasAirtightSuit);
      }
      Tag elementTag = eq.GetComponent<SuitTank>().elementTag;
      eq.GetComponent<Storage>().DropUnlessHasTag(elementTag);
    });
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Jet_Suit");
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Helmet");
    return equipmentDef;
  }

  public void DoPostConfigure(GameObject go)
  {
    SuitTank suitTank = go.AddComponent<SuitTank>();
    suitTank.element = "Oxygen";
    suitTank.capacity = (float) ((double) DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * 600.0 * 1.25);
    suitTank.elementTag = GameTags.Breathable;
    suitTank.SafeCellFlagsToIgnoreOnEquipped = SafeCellQuery.SafeFlags.CorrectTemperature | SafeCellQuery.SafeFlags.IsBreathable | SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace | SafeCellQuery.SafeFlags.IsNotLiquid;
    go.AddComponent<JetSuitTank>();
    go.AddComponent<HelmetController>().has_jets = true;
    Durability durability = go.AddComponent<Durability>();
    durability.wornEquipmentPrefabID = "Worn_Jet_Suit";
    durability.durabilityLossPerCycle = TUNING.EQUIPMENT.SUITS.ATMOSUIT_DECAY;
    Storage storage = go.AddOrGet<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    storage.showInUI = true;
    go.AddOrGet<AtmoSuit>();
    go.AddComponent<SuitDiseaseHandler>();
  }
}
