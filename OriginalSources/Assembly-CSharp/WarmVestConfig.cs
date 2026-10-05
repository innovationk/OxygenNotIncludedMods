// Decompiled with JetBrains decompiler
// Type: WarmVestConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WarmVestConfig : IEquipmentConfig
{
  public const string ID = "Warm_Vest";
  public static ComplexRecipe recipe;

  public EquipmentDef CreateEquipmentDef()
  {
    new Dictionary<string, float>()
    {
      {
        "BasicFabric",
        (float) TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS
      }
    };
    ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.WARM_CLOTHING;
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("Warm_Vest", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float) TUNING.EQUIPMENT.VESTS.WARM_VEST_MASS, TUNING.EQUIPMENT.VESTS.WARM_VEST_ICON0, TUNING.EQUIPMENT.VESTS.SNAPON0, TUNING.EQUIPMENT.VESTS.WARM_VEST_ANIM0, 4, AttributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, new Tag[2]
    {
      GameTags.Clothes,
      GameTags.PedestalDisplayable
    });
    int decorMod = ClothingWearer.ClothingInfo.WARM_CLOTHING.decorMod;
    Descriptor descriptor1 = new Descriptor($"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.WARM_CLOTHING.conductivityMod)}", $"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.WARM_CLOTHING.conductivityMod)}");
    Descriptor descriptor2 = new Descriptor($"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {decorMod}", $"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {decorMod}");
    equipmentDef.additionalDescriptors.Add(descriptor1);
    if (decorMod != 0)
      equipmentDef.additionalDescriptors.Add(descriptor2);
    equipmentDef.OnEquipCallBack = (Action<Equippable>) (eq => ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo));
    equipmentDef.OnUnequipCallBack = new Action<Equippable>(ClothingWearer.ClothingInfo.OnUnequipVest);
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.WARM_VEST.RECIPE_DESC;
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

  public void DoPostConfigure(GameObject go) => WarmVestConfig.SetupVest(go);
}
