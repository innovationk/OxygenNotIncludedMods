// Decompiled with JetBrains decompiler
// Type: TallowLubricationStickConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TallowLubricationStickConfig : IEntityConfig, IHasDlcRestrictions
{
  public const string ID = "TallowLubricationStick";
  public static ComplexRecipe recipe;
  public static float MASS_PER_RECIPE = GunkMonitor.GUNK_CAPACITY;

  public string[] GetRequiredDlcIds() => DlcManager.DLC3;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("TallowLubricationStick", (string) ITEMS.TALLOWLUBRICATIONSTICK.NAME, (string) ITEMS.TALLOWLUBRICATIONSTICK.DESC, TallowLubricationStickConfig.MASS_PER_RECIPE, true, Assets.GetAnim((HashedString) "lubricant_applicator_tallow_kanim"), "idle1", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.4f, isPickupable: true, element: SimHashes.Tallow);
    looseEntity.AddOrGet<EntitySplitter>();
    looseEntity.AddTag(GameTags.MedicalSupplies);
    looseEntity.AddTag(GameTags.SolidLubricant);
    looseEntity.AddTag(GameTags.PedestalDisplayable);
    ComplexRecipe.RecipeElement[] recipeElementArray1 = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement(SimHashes.Tallow.CreateTag(), 10f),
      new ComplexRecipe.RecipeElement(SimHashes.Water.CreateTag(), GunkMonitor.GUNK_CAPACITY - 10f)
    };
    ComplexRecipe.RecipeElement[] recipeElementArray2 = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement("TallowLubricationStick".ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
    };
    TallowLubricationStickConfig.recipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("Apothecary", (IList<ComplexRecipe.RecipeElement>) recipeElementArray1, (IList<ComplexRecipe.RecipeElement>) recipeElementArray2), recipeElementArray1, recipeElementArray2)
    {
      time = 100f,
      description = (string) ITEMS.TALLOWLUBRICATIONSTICK.RECIPEDESC,
      nameDisplay = ComplexRecipe.RecipeNameDisplay.Result,
      fabricators = new List<Tag>() { (Tag) "Apothecary" },
      sortOrder = 1,
      requiredTech = Db.Get().TechItems.lubricationStick.parentTechId
    };
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
