// Decompiled with JetBrains decompiler
// Type: RubberBootsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RubberBootsConfig : IEquipmentConfig, IHasDlcRestrictions
{
  public static readonly string ID = "RubberBoots";

  public EquipmentDef CreateEquipmentDef()
  {
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef(RubberBootsConfig.ID, TUNING.EQUIPMENT.SHOES.SLOT, SimHashes.Rubber, 30f, "rubber_boots_item_kanim", TUNING.EQUIPMENT.SHOES.SNAPON0, "", 6, AttributeModifiers, width: 0.28f, height: 0.28f, additional_tags: new Tag[2]
    {
      GameTags.PedestalDisplayable,
      GameTags.Clothes
    });
    equipmentDef.RecipeDescription = (string) (DlcManager.IsContentSubscribed("DLC3_ID") ? STRINGS.EQUIPMENT.PREFABS.RUBBERBOOTS.RECIPE_DESC_DLC3 : STRINGS.EQUIPMENT.PREFABS.RUBBERBOOTS.RECIPE_DESC);
    ResourceSet<Effect> effects = Db.Get().effects;
    equipmentDef.EffectImmunites.Add(effects.Get("WetFeet"));
    equipmentDef.EffectImmunites.Add(effects.Get("RecentlySlippedTracker"));
    equipmentDef.OnEquipCallBack = (Action<Equippable>) (eq =>
    {
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
        return;
      GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      if (!(bool) (UnityEngine.Object) targetGameObject)
        return;
      targetGameObject.AddTag(GameTags.FeetProtection);
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
      targetGameObject.RemoveTag(GameTags.FeetProtection);
    });
    return equipmentDef;
  }

  public void DoPostConfigure(GameObject go)
  {
    go.AddOrGet<Equippable>().SetQuality(QualityLevel.Poor);
    KBatchedAnimController component;
    if (!go.TryGetComponent<KBatchedAnimController>(out component))
      return;
    component.sceneLayer = Grid.SceneLayer.BuildingBack;
  }

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public string[] GetRequiredDlcIds() => DlcManager.DLC5;
}
