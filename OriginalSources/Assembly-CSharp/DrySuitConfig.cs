// Decompiled with JetBrains decompiler
// Type: DrySuitConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DrySuitConfig : IEquipmentConfig, IHasDlcRestrictions
{
  public const string ID = "DrySuit";
  public static ComplexRecipe recipe;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;

  public EquipmentDef CreateEquipmentDef()
  {
    ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.DRY_SUIT;
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("DrySuit", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float) TUNING.EQUIPMENT.SUITS.DRY_SUIT_MASS, "wetsuit_item_kanim", TUNING.EQUIPMENT.VESTS.SNAPON0, "body_wetsuit_kanim", 4, AttributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, new Tag[2]
    {
      GameTags.Clothes,
      GameTags.PedestalDisplayable
    });
    int decorMod = ClothingWearer.ClothingInfo.DRY_SUIT.decorMod;
    Descriptor descriptor1 = new Descriptor($"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.DRY_SUIT.conductivityMod)}", $"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.DRY_SUIT.conductivityMod)}");
    Descriptor descriptor2 = new Descriptor($"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {decorMod}", $"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {decorMod}");
    equipmentDef.additionalDescriptors.Add(descriptor1);
    if (decorMod != 0)
      equipmentDef.additionalDescriptors.Add(descriptor2);
    equipmentDef.RecipeDescription = (string) (DlcManager.IsContentSubscribed("DLC3_ID") ? STRINGS.EQUIPMENT.PREFABS.DRYSUIT.RECIPE_DESC_DLC3 : STRINGS.EQUIPMENT.PREFABS.DRYSUIT.RECIPE_DESC);
    ResourceSet<Effect> effects = Db.Get().effects;
    equipmentDef.EffectImmunites.Add(effects.Get("WetFeet"));
    equipmentDef.EffectImmunites.Add(effects.Get("SoakingWet"));
    equipmentDef.OnEquipCallBack = (Action<Equippable>) (eq =>
    {
      ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo);
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if (!(bool) (UnityEngine.Object) targetGameObject)
        return;
      targetGameObject.AddTag(GameTags.FeetAndWaistProtection);
    });
    equipmentDef.OnUnequipCallBack = (Action<Equippable>) (eq =>
    {
      ClothingWearer.ClothingInfo.OnUnequipVest(eq);
      if (eq.assignee == null)
        return;
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if (!(bool) (UnityEngine.Object) targetGameObject)
        return;
      targetGameObject.RemoveTag(GameTags.FeetAndWaistProtection);
    });
    return equipmentDef;
  }

  public static void SetupVest(GameObject go)
  {
    Equippable equippable = go.GetComponent<Equippable>();
    if ((UnityEngine.Object) equippable == (UnityEngine.Object) null)
      equippable = go.AddComponent<Equippable>();
    equippable.SetQuality(QualityLevel.Poor);
    go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
  }

  public void DoPostConfigure(GameObject go) => DrySuitConfig.SetupVest(go);
}
