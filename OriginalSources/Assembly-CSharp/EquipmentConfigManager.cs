// Decompiled with JetBrains decompiler
// Type: EquipmentConfigManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/EquipmentConfigManager")]
public class EquipmentConfigManager : KMonoBehaviour
{
  public static EquipmentConfigManager Instance;

  public static void DestroyInstance()
  {
    EquipmentConfigManager.Instance = (EquipmentConfigManager) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    EquipmentConfigManager.Instance = this;
  }

  public void RegisterEquipment(IEquipmentConfig config)
  {
    string[] requiredDlcIds = (string[]) null;
    string[] forbiddenDlcIds = (string[]) null;
    if (config.GetDlcIds() != null)
    {
      DlcManager.ConvertAvailableToRequireAndForbidden(config.GetDlcIds(), out requiredDlcIds, out forbiddenDlcIds);
      DebugUtil.DevLogError($"{config.GetType()} implements GetDlcIds, which is obsolete.");
    }
    else if (config is IHasDlcRestrictions hasDlcRestrictions)
    {
      requiredDlcIds = hasDlcRestrictions.GetRequiredDlcIds();
      forbiddenDlcIds = hasDlcRestrictions.GetForbiddenDlcIds();
    }
    if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
      return;
    EquipmentDef equipmentDef = config.CreateEquipmentDef();
    GameObject looseEntity1 = EntityTemplates.CreateLooseEntity(equipmentDef.Id, equipmentDef.Name, equipmentDef.RecipeDescription, equipmentDef.Mass, true, equipmentDef.Anim, "object", Grid.SceneLayer.Ore, equipmentDef.CollisionShape, equipmentDef.width, equipmentDef.height, true, element: equipmentDef.OutputElement);
    Equippable equippable = looseEntity1.AddComponent<Equippable>();
    equippable.def = equipmentDef;
    Debug.Assert((UnityEngine.Object) equippable.def != (UnityEngine.Object) null);
    equippable.slotID = equipmentDef.Slot;
    Debug.Assert(equippable.slot != null);
    config.DoPostConfigure(looseEntity1);
    foreach (Tag additionalTag in equipmentDef.AdditionalTags)
      looseEntity1.GetComponent<KPrefabID>().AddTag(additionalTag);
    Assets.AddPrefab(looseEntity1.GetComponent<KPrefabID>());
    if (equipmentDef.wornID == null)
      return;
    GameObject looseEntity2 = EntityTemplates.CreateLooseEntity(equipmentDef.wornID, equipmentDef.WornName, equipmentDef.WornDesc, equipmentDef.Mass, true, equipmentDef.Anim, "worn_out", Grid.SceneLayer.Ore, equipmentDef.CollisionShape, equipmentDef.width, equipmentDef.height, true);
    RepairableEquipment repairableEquipment = looseEntity2.AddComponent<RepairableEquipment>();
    repairableEquipment.def = equipmentDef;
    Debug.Assert((UnityEngine.Object) repairableEquipment.def != (UnityEngine.Object) null);
    SymbolOverrideControllerUtil.AddToPrefab(looseEntity2);
    foreach (Tag additionalTag in equipmentDef.AdditionalTags)
      looseEntity2.GetComponent<KPrefabID>().AddTag(additionalTag);
    Assets.AddPrefab(looseEntity2.GetComponent<KPrefabID>());
  }

  private void LoadRecipe(EquipmentDef def, Equippable equippable)
  {
    Recipe recipe = new Recipe(def.Id, recipeDescription: def.RecipeDescription);
    recipe.SetFabricator(def.FabricatorId, def.FabricationTime);
    recipe.TechUnlock = def.RecipeTechUnlock;
    foreach (KeyValuePair<string, float> inputElementMass in def.InputElementMassMap)
      recipe.AddIngredient(new Recipe.Ingredient(inputElementMass.Key, inputElementMass.Value));
  }

  [Conditional("UNITY_EDITOR")]
  private void ValidateEquipmentConfig(IEquipmentConfig equipmentConfig)
  {
    System.Type c = equipmentConfig != null ? equipmentConfig.GetType() : throw new ArgumentNullException(nameof (equipmentConfig));
    System.Type type = typeof (IHasDlcRestrictions);
    int num1 = c.GetMethod("GetRequiredDlcIds", System.Type.EmptyTypes) != (MethodInfo) null ? 1 : 0;
    bool flag1 = c.GetMethod("GetForbiddenDlcIds", System.Type.EmptyTypes) != (MethodInfo) null;
    bool flag2 = type.IsAssignableFrom(c);
    int num2 = flag1 ? 1 : 0;
    if ((num1 | num2) == 0 || flag2)
      return;
    DebugUtil.LogErrorArgs((object) (c.Name + " is an IEquipmentConfig and has GetRequiredDlcIds or GetForbiddenDlcIds but does not implement IHasDlcRestrictions."));
  }
}
