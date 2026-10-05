// Decompiled with JetBrains decompiler
// Type: EggCrackerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[EntityConfigOrder(2)]
public class EggCrackerConfig : IBuildingConfig
{
  public const string ID = "EggCracker";
  public static Dictionary<Tag, List<EggCrackerConfig.EggData>> EggsBySpecies = new Dictionary<Tag, List<EggCrackerConfig.EggData>>();
  private static List<EggCrackerConfig.EggData> uncategorizedEggData = new List<EggCrackerConfig.EggData>();

  public static void RegisterEgg(
    Tag eggPrefabTag,
    string name,
    string description,
    float mass,
    string[] requiredDLC,
    string[] forbiddenDLC)
  {
    EggCrackerConfig.RegisterEgg(eggPrefabTag, name, description, mass, requiredDLC, forbiddenDLC, (Tuple<Tag, float>[]) null);
  }

  public static void RegisterEgg(
    Tag eggPrefabTag,
    string name,
    string description,
    float mass,
    string[] requiredDLC,
    string[] forbiddenDLC,
    Tuple<Tag, float>[] customDrops)
  {
    EggCrackerConfig.RegisterEgg(eggPrefabTag, name, description, mass, requiredDLC, forbiddenDLC, customDrops, 0.5f);
  }

  public static void RegisterEgg(
    Tag eggPrefabTag,
    string name,
    string description,
    float mass,
    string[] requiredDLC,
    string[] forbiddenDLC,
    Tuple<Tag, float>[] customDrops,
    float customShellRatio,
    bool allowCrackerRecipeCreation = true)
  {
    EggCrackerConfig.uncategorizedEggData.Add(new EggCrackerConfig.EggData(eggPrefabTag, name, description, mass, requiredDLC, forbiddenDLC, allowCrackerRecipeCreation, customShellRatio)
    {
      customOutput = customDrops
    });
  }

  public static void CategorizeEggs()
  {
    foreach (EggCrackerConfig.EggData eggData in EggCrackerConfig.uncategorizedEggData)
    {
      Tag spawnedCreature = Assets.GetPrefab(eggData.id).GetDef<IncubationMonitor.Def>().spawnedCreature;
      Tag species = Assets.GetPrefab(spawnedCreature).GetComponent<CreatureBrain>().species;
      eggData.isBaseMorph = Assets.GetPrefab(spawnedCreature).HasTag(GameTags.OriginalCreature);
      if (!EggCrackerConfig.EggsBySpecies.ContainsKey(species))
        EggCrackerConfig.EggsBySpecies.Add(species, new List<EggCrackerConfig.EggData>());
      EggCrackerConfig.EggsBySpecies[species].Add(eggData);
    }
  }

  public override BuildingDef CreateBuildingDef()
  {
    float[] tieR1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] rawMetals = TUNING.MATERIALS.RAW_METALS;
    EffectorValues none = NOISE_POLLUTION.NONE;
    EffectorValues tieR0 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues noise = none;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("EggCracker", 2, 2, "egg_cracker_kanim", 30, 10f, tieR1, rawMetals, 1600f, BuildLocationRule.OnFloor, tieR0, noise);
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
    buildingDef.AddSearchTerms((string) SEARCH_TERMS.FOOD);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "snapto_egg", false);
    ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
    fabricator.labelByResult = false;
    fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
    fabricator.duplicantOperated = true;
    go.AddOrGet<FabricatorIngredientStatusManager>();
    go.AddOrGet<CopyBuildingSettings>();
    ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
    BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
    KAnimFile[] kanimFileArray = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_egg_cracker_kanim")
    };
    fabricatorWorkable.overrideAnims = kanimFileArray;
    fabricator.outputOffset = new Vector3(1f, 1f, 0.0f);
    Prioritizable.AddRef(go);
    go.AddOrGet<EggCracker>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGet<LogicOperationalController>();
  }

  public override void ConfigurePost(BuildingDef def)
  {
    base.ConfigurePost(def);
    this.MakeRecipes();
  }

  public void MakeRecipes()
  {
    EggCrackerConfig.CategorizeEggs();
    foreach (KeyValuePair<Tag, List<EggCrackerConfig.EggData>> eggsBySpecy in EggCrackerConfig.EggsBySpecies)
    {
      Tag[] materialOptions = new Tag[eggsBySpecy.Value.Count];
      for (int index = 0; index < materialOptions.Length; ++index)
        materialOptions[index] = eggsBySpecy.Value[index].id;
      EggCrackerConfig.EggData eggData = eggsBySpecy.Value[0];
      if (eggData.hasCrackerRecipe)
      {
        string str1 = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION, (object) eggData.name);
        ComplexRecipe.RecipeElement[] recipeElementArray = new ComplexRecipe.RecipeElement[1]
        {
          new ComplexRecipe.RecipeElement(materialOptions, 1f)
          {
            material = materialOptions[0]
          }
        };
        float num1 = (double) eggData.customShellRatio < 0.0 ? 0.5f : eggData.customShellRatio;
        float num2 = 1f - num1;
        List<ComplexRecipe.RecipeElement> recipeElementList = new List<ComplexRecipe.RecipeElement>();
        ComplexRecipe.RecipeElement recipeElement1 = (ComplexRecipe.RecipeElement) null;
        ComplexRecipe.RecipeElement recipeElement2 = (ComplexRecipe.RecipeElement) null;
        if ((double) num1 > 0.0)
          recipeElementList.Add(recipeElement1 = new ComplexRecipe.RecipeElement((Tag) "EggShell", num1 * eggData.mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature));
        if ((double) num2 > 0.0)
          recipeElementList.Add(recipeElement2 = new ComplexRecipe.RecipeElement((Tag) "RawEgg", num2 * eggData.mass, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature));
        if (eggData.customOutput != null)
        {
          foreach (Tuple<Tag, float> tuple in eggData.customOutput)
          {
            if (tuple.first == (Tag) "EggShell")
            {
              recipeElement1 = recipeElement2 != null ? recipeElement1 : new ComplexRecipe.RecipeElement((Tag) "EggShell", 0.0f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature);
              recipeElement1.amount += tuple.second;
            }
            else if (tuple.first == (Tag) "RawEgg")
            {
              recipeElement2 = recipeElement2 ?? new ComplexRecipe.RecipeElement((Tag) "RawEgg", 0.0f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature);
              recipeElement2.amount += tuple.second;
            }
            else
              recipeElementList.Add(new ComplexRecipe.RecipeElement(tuple.first, tuple.second, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature));
          }
        }
        ComplexRecipe.RecipeElement[] array = recipeElementList.ToArray();
        string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("EggCracker", (Tag) "RawEgg");
        string str2 = ComplexRecipeManager.MakeRecipeID("EggCracker", (IList<ComplexRecipe.RecipeElement>) recipeElementArray, (IList<ComplexRecipe.RecipeElement>) array);
        ComplexRecipe complexRecipe = new ComplexRecipe(str2, recipeElementArray, array, eggData.requiredDlcIds, eggData.forbiddenDlcIds)
        {
          description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) eggData.name, (object) str1),
          fabricators = new List<Tag>()
          {
            (Tag) "EggCracker"
          },
          time = 5f,
          nameDisplay = ComplexRecipe.RecipeNameDisplay.Custom,
          customName = eggsBySpecy.Key.ProperName(),
          customSpritePrefabID = recipeElementArray[0].material != (Tag) (string) null ? recipeElementArray[0].material.Name : recipeElementArray[0].possibleMaterials[0].Name
        };
        ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str2);
      }
    }
  }

  public class EggData : IHasDlcRestrictions
  {
    public Tag id;
    public float mass;
    public string name;
    public string description;
    public string[] requiredDlcIds;
    public string[] forbiddenDlcIds;
    public bool hasCrackerRecipe;
    public Tuple<Tag, float>[] customOutput;
    public float customShellRatio = -1f;
    public bool isBaseMorph;

    public EggData(
      Tag id,
      string name,
      string description,
      float mass,
      string[] requiredDLC,
      string[] forbiddenDLC)
    {
      this.Config(id, name, description, mass, requiredDLC, forbiddenDLC);
    }

    public EggData(
      Tag id,
      string name,
      string description,
      float mass,
      string[] requiredDLC,
      string[] forbiddenDLC,
      bool hasCrackerRecipe = true)
    {
      this.Config(id, name, description, mass, requiredDLC, forbiddenDLC, hasCrackerRecipe);
    }

    public EggData(
      Tag id,
      string name,
      string description,
      float mass,
      string[] requiredDLC,
      string[] forbiddenDLC,
      bool hasCrackerRecipe = true,
      float customShellRatio = -1f)
    {
      this.Config(id, name, description, mass, requiredDLC, forbiddenDLC, hasCrackerRecipe, customShellRatio);
    }

    private void Config(
      Tag id,
      string name,
      string description,
      float mass,
      string[] requiredDLC,
      string[] forbiddenDLC,
      bool hasCrackerRecipe = true,
      float customShellRatio = -1f)
    {
      this.id = id;
      this.name = name;
      this.description = description;
      this.mass = mass;
      this.requiredDlcIds = requiredDLC;
      this.forbiddenDlcIds = forbiddenDLC;
      this.hasCrackerRecipe = hasCrackerRecipe;
      this.customShellRatio = customShellRatio;
    }

    public string[] GetRequiredDlcIds() => this.requiredDlcIds;

    public string[] GetForbiddenDlcIds() => this.forbiddenDlcIds;
  }
}
