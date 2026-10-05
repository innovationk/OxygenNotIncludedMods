// Decompiled with JetBrains decompiler
// Type: CustomClothingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CustomClothingConfig : IEquipmentConfig
{
  public const string ID = "CustomClothing";
  public static ComplexRecipe recipe;

  public EquipmentDef CreateEquipmentDef()
  {
    Dictionary<string, float> dictionary = new Dictionary<string, float>()
    {
      {
        "Funky_Vest",
        (float) TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS
      },
      {
        "BasicFabric",
        3f
      }
    };
    ClothingWearer.ClothingInfo clothingInfo = ClothingWearer.ClothingInfo.CUSTOM_CLOTHING;
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("CustomClothing", TUNING.EQUIPMENT.CLOTHING.SLOT, SimHashes.Carbon, (float) TUNING.EQUIPMENT.VESTS.CUSTOM_CLOTHING_MASS, "shirt_decor01_kanim", TUNING.EQUIPMENT.VESTS.SNAPON0, "body_shirt_decor01_kanim", 4, AttributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, new Tag[2]
    {
      GameTags.Clothes,
      GameTags.PedestalDisplayable
    });
    Descriptor descriptor1 = new Descriptor($"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.CUSTOM_CLOTHING.conductivityMod)}", $"{DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME}: {GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.CUSTOM_CLOTHING.conductivityMod)}");
    Descriptor descriptor2 = new Descriptor($"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {ClothingWearer.ClothingInfo.CUSTOM_CLOTHING.decorMod}", $"{DUPLICANTS.ATTRIBUTES.DECOR.NAME}: {ClothingWearer.ClothingInfo.CUSTOM_CLOTHING.decorMod}");
    equipmentDef.additionalDescriptors.Add(descriptor1);
    equipmentDef.additionalDescriptors.Add(descriptor2);
    equipmentDef.OnEquipCallBack = (Action<Equippable>) (eq => ClothingWearer.ClothingInfo.OnEquipVest(eq, clothingInfo));
    equipmentDef.OnUnequipCallBack = new Action<Equippable>(ClothingWearer.ClothingInfo.OnUnequipVest);
    equipmentDef.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.CUSTOMCLOTHING.RECIPE_DESC;
    foreach (EquippableFacadeResource resource in Db.GetEquippableFacades().resources)
    {
      if (!(resource.DefID != "CustomClothing"))
        TagManager.Create(resource.Id, EquippableFacade.GetNameOverride("CustomClothing", resource.Id));
    }
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

  public void DoPostConfigure(GameObject go) => CustomClothingConfig.SetupVest(go);
}
