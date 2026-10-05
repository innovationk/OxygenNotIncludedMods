// Decompiled with JetBrains decompiler
// Type: SleepClinicPajamas
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SleepClinicPajamas : IEquipmentConfig
{
  public const string ID = "SleepClinicPajamas";
  public const string EFFECT_ID = "SleepClinic";

  public EquipmentDef CreateEquipmentDef()
  {
    ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.FANCY_CLOTHING;
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef(nameof (SleepClinicPajamas), TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float) TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, "pajamas_kanim", TUNING.EQUIPMENT.VESTS.SNAPON0, "body_pajamas_kanim", 4, AttributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, new Tag[2]
    {
      GameTags.Clothes,
      GameTags.PedestalDisplayable
    });
    equipmentDef.RecipeDescription = $"{(string) STRINGS.EQUIPMENT.PREFABS.SLEEPCLINICPAJAMAS.DESC}\n\n{(string) STRINGS.EQUIPMENT.PREFABS.SLEEPCLINICPAJAMAS.EFFECT}";
    Descriptor descriptor1 = new Descriptor($"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)}", $"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)}");
    Descriptor descriptor2 = new Descriptor($"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {ClothingWearer.ClothingInfo.FANCY_CLOTHING.decorMod}", $"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {ClothingWearer.ClothingInfo.FANCY_CLOTHING.decorMod}");
    equipmentDef.additionalDescriptors.Add(descriptor1);
    equipmentDef.additionalDescriptors.Add(descriptor2);
    Effect.AddModifierDescriptions((GameObject) null, equipmentDef.additionalDescriptors, "SleepClinic");
    equipmentDef.OnEquipCallBack = (Action<Equippable>) (eq => ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo));
    equipmentDef.OnUnequipCallBack = (Action<Equippable>) (eq =>
    {
      ClothingWearer.ClothingInfo.OnUnequipVest(eq);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) STRINGS.EQUIPMENT.PREFABS.SLEEPCLINICPAJAMAS.DESTROY_TOAST, eq.transform);
      eq.gameObject.DeleteObject();
    });
    return equipmentDef;
  }

  public void DoPostConfigure(GameObject go)
  {
    go.AddOrGet<ClinicDreamable>().workTime = 300f;
    go.AddOrGet<Equippable>().SetQuality(QualityLevel.Poor);
    go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingFront;
  }
}
