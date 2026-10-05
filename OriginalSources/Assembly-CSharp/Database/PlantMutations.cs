// Decompiled with JetBrains decompiler
// Type: Database.PlantMutations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Database;

public class PlantMutations : ResourceSet<PlantMutation>
{
  public PlantMutation moderatelyLoose;
  public PlantMutation moderatelyTight;
  public PlantMutation extremelyTight;
  public PlantMutation bonusLice;
  public PlantMutation sunnySpeed;
  public PlantMutation slowBurn;
  public PlantMutation blooms;
  public PlantMutation loadedWithFruit;
  public PlantMutation heavyFruit;
  public PlantMutation rottenHeaps;

  public PlantMutation AddPlantMutation(string id)
  {
    StringEntry name = Strings.Get(new StringKey($"STRINGS.CREATURES.PLANT_MUTATIONS.{id.ToUpper()}.NAME"));
    StringEntry desc = Strings.Get(new StringKey($"STRINGS.CREATURES.PLANT_MUTATIONS.{id.ToUpper()}.DESCRIPTION"));
    PlantMutation resource = new PlantMutation(id, (string) name, (string) desc);
    this.Add(resource);
    return resource;
  }

  public PlantMutations(ResourceSet parent)
    : base(nameof (PlantMutations), parent)
  {
    this.moderatelyLoose = this.AddPlantMutation(nameof (moderatelyLoose)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, 0.5f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, -0.25f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, -0.5f, true).VisualTint(-0.4f, -0.4f, -0.4f);
    this.moderatelyTight = this.AddPlantMutation(nameof (moderatelyTight)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, -0.5f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 0.5f, true).VisualTint(0.2f, 0.2f, 0.2f);
    this.extremelyTight = this.AddPlantMutation(nameof (extremelyTight)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().PlantAttributes.WiltTempRangeMod, -0.8f, true).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 1f, true).VisualTint(0.3f, 0.3f, 0.3f).VisualBGFX("mutate_glow_fx_kanim");
    this.bonusLice = this.AddPlantMutation(nameof (bonusLice)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).BonusCrop((Tag) "BasicPlantFood", 1f).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "meal_lice_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "meal_lice_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_MealLice"));
    this.sunnySpeed = this.AddPlantMutation(nameof (sunnySpeed)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().PlantAttributes.MinLightLux, 1000f).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, -0.5f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "leaf_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "leaf_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_Leaf"));
    this.slowBurn = this.AddPlantMutation(nameof (slowBurn)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, -0.9f, true).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, 3.5f, true).VisualTint(-0.3f, -0.3f, -0.5f);
    this.blooms = this.AddPlantMutation(nameof (blooms)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().BuildingAttributes.Decor, 20f).VisualSymbolOverride("snapTo_mutate1", "mutate_snaps_kanim", "blossom_mutate1").VisualSymbolOverride("snapTo_mutate2", "mutate_snaps_kanim", "blossom_mutate2").AddSoundEvent(GlobalAssets.GetSound("Plant_mutation_PrickleFlower"));
    this.loadedWithFruit = this.AddPlantMutation(nameof (loadedWithFruit)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().PlantAttributes.YieldAmount, 1f, true).AttributeModifier(Db.Get().PlantAttributes.HarvestTime, 4f, true).AttributeModifier(Db.Get().PlantAttributes.MinLightLux, 200f).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.2f, true).VisualSymbolScale("swap_crop01", 1.3f).VisualSymbolScale("swap_crop02", 1.3f);
    this.rottenHeaps = this.AddPlantMutation(nameof (rottenHeaps)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().Amounts.Maturity.maxAttribute, -0.75f, true).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.5f, true).BonusCrop((Tag) RotPileConfig.ID, 4f).AddDiseaseToHarvest(Db.Get().Diseases.GetIndex((HashedString) Db.Get().Diseases.FoodGerms.Id), 10000).ForcePrefersDarkness().VisualFGFX("mutate_stink_fx_kanim").VisualSymbolTint("swap_crop01", -0.2f, -0.1f, -0.5f).VisualSymbolTint("swap_crop02", -0.2f, -0.1f, -0.5f);
    this.heavyFruit = this.AddPlantMutation(nameof (heavyFruit)).AttributeModifier(Db.Get().PlantAttributes.MinRadiationThreshold, 250f).AttributeModifier(Db.Get().PlantAttributes.FertilizerUsageMod, 0.25f, true).ForceSelfHarvestOnGrown().VisualSymbolTint("swap_crop01", -0.1f, -0.5f, -0.5f).VisualSymbolTint("swap_crop02", -0.1f, -0.5f, -0.5f);
  }

  public List<string> GetNamesForMutations(List<string> mutationIDs)
  {
    List<string> namesForMutations = new List<string>(mutationIDs.Count);
    foreach (string mutationId in mutationIDs)
      namesForMutations.Add(this.Get(mutationId).Name);
    return namesForMutations;
  }

  public PlantMutation GetRandomMutation(string targetPlantPrefabID)
  {
    return this.resources.Where<PlantMutation>((Func<PlantMutation, bool>) (m =>
    {
      if (m.originalMutation || m.restrictedPrefabIDs.Contains(targetPlantPrefabID))
        return false;
      return m.requiredPrefabIDs.Count == 0 || m.requiredPrefabIDs.Contains(targetPlantPrefabID);
    })).ToList<PlantMutation>().GetRandom<PlantMutation>();
  }
}
