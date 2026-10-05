// Decompiled with JetBrains decompiler
// Type: OxygenMaskConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OxygenMaskConfig : IEquipmentConfig
{
  public const string ID = "Oxygen_Mask";
  public const string WORN_ID = "Worn_Oxygen_Mask";
  private const PathFinder.PotentialPath.Flags suit_flags = PathFinder.PotentialPath.Flags.HasOxygenMask;
  private AttributeModifier expertAthleticsModifier;

  public EquipmentDef CreateEquipmentDef()
  {
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    AttributeModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) TUNING.EQUIPMENT.SUITS.OXYGEN_MASK_ATHLETICS, (string) STRINGS.EQUIPMENT.PREFABS.OXYGEN_MASK.NAME));
    this.expertAthleticsModifier = new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.ATHLETICS, (float) -TUNING.EQUIPMENT.SUITS.OXYGEN_MASK_ATHLETICS, Db.Get().Skills.Suits1.Name);
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Oxygen_Mask", TUNING.EQUIPMENT.SUITS.SLOT, SimHashes.Dirt, 15f, "oxygen_mask_kanim", "mask_oxygen", "", 6, AttributeModifiers, additional_tags: new Tag[3]
    {
      GameTags.Suit,
      GameTags.Clothes,
      GameTags.PedestalDisplayable
    });
    equipmentDef.wornID = "Worn_Oxygen_Mask";
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.OXYGEN_MASK.RECIPE_DESC;
    equipmentDef.OnEquipCallBack = (Action<Equippable>) (eq =>
    {
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      Navigator component1 = targetGameObject.GetComponent<Navigator>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.SetFlags(PathFinder.PotentialPath.Flags.HasOxygenMask);
      MinionResume component2 = targetGameObject.GetComponent<MinionResume>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || !component2.HasPerk((HashedString) Db.Get().SkillPerks.ExosuitExpertise.Id))
        return;
      targetGameObject.GetAttributes().Get(Db.Get().Attributes.Athletics).Add(this.expertAthleticsModifier);
    });
    equipmentDef.OnUnequipCallBack = (Action<Equippable>) (eq =>
    {
      if (eq.assignee == null)
        return;
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if (!(bool) (UnityEngine.Object) targetGameObject)
        return;
      targetGameObject.GetAttributes()?.Get(Db.Get().Attributes.Athletics).Remove(this.expertAthleticsModifier);
      Navigator component = targetGameObject.GetComponent<Navigator>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.ClearFlags(PathFinder.PotentialPath.Flags.HasOxygenMask);
    });
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Oxygen_Mask");
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "Helmet");
    return equipmentDef;
  }

  public void DoPostConfigure(GameObject go)
  {
    Storage storage = go.AddComponent<Storage>();
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
    storage.showInUI = true;
    SuitTank suitTank = go.AddComponent<SuitTank>();
    suitTank.element = "Oxygen";
    suitTank.capacity = 20f;
    suitTank.elementTag = GameTags.Breathable;
    suitTank.SafeCellFlagsToIgnoreOnEquipped = SafeCellQuery.SafeFlags.IsBreathable;
    Durability durability = go.AddComponent<Durability>();
    durability.wornEquipmentPrefabID = "Worn_Oxygen_Mask";
    durability.durabilityLossPerCycle = TUNING.EQUIPMENT.SUITS.OXYGEN_MASK_DECAY;
    go.AddComponent<SuitDiseaseHandler>();
  }
}
