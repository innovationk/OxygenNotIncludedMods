// Decompiled with JetBrains decompiler
// Type: Diet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Diet
{
  public List<KeyValuePair<Tag, float>> consumedTags;
  public List<KeyValuePair<Tag, float>> producedTags;
  private Dictionary<Tag, Diet.Info> consumedTagToInfo = new Dictionary<Tag, Diet.Info>();

  public Diet.Info[] infos { get; private set; }

  public Diet.Info[] solidEdiblesInfo { get; private set; }

  public Diet.Info[] directlyEatenPlantInfos { get; private set; }

  public Diet.Info[] preyInfos { get; private set; }

  public bool CanEatAnySolid => this.solidEdiblesInfo != null && this.solidEdiblesInfo.Length != 0;

  public bool CanEatAnyPlantDirectly
  {
    get => this.directlyEatenPlantInfos != null && this.directlyEatenPlantInfos.Length != 0;
  }

  public bool CanEatPreyCritter => this.preyInfos != null && this.preyInfos.Length != 0;

  public bool IsConsumedTagAbleToBeEatenDirectly(Tag tag)
  {
    if (this.directlyEatenPlantInfos == null && this.preyInfos == null)
      return false;
    for (int index = 0; index < this.directlyEatenPlantInfos.Length; ++index)
    {
      if (this.directlyEatenPlantInfos[index].consumedTags.Contains(tag))
        return true;
    }
    for (int index = 0; index < this.preyInfos.Length; ++index)
    {
      if (this.preyInfos[index].consumedTags.Contains(tag))
        return true;
    }
    return false;
  }

  private void UpdateSecondaryInfoArrays()
  {
    this.directlyEatenPlantInfos = this.infos == null ? (Diet.Info[]) null : ((IEnumerable<Diet.Info>) this.infos).Where<Diet.Info>((Func<Diet.Info, bool>) (i => i.foodType == Diet.Info.FoodType.EatPlantDirectly || i.foodType == Diet.Info.FoodType.EatPlantStorage)).ToArray<Diet.Info>();
    this.solidEdiblesInfo = this.infos == null ? (Diet.Info[]) null : ((IEnumerable<Diet.Info>) this.infos).Where<Diet.Info>((Func<Diet.Info, bool>) (i => i.foodType == Diet.Info.FoodType.EatSolid)).ToArray<Diet.Info>();
    this.preyInfos = this.infos == null ? (Diet.Info[]) null : ((IEnumerable<Diet.Info>) this.infos).Where<Diet.Info>((Func<Diet.Info, bool>) (i => i.foodType == Diet.Info.FoodType.EatPrey || i.foodType == Diet.Info.FoodType.EatButcheredPrey)).ToArray<Diet.Info>();
  }

  public Diet(params Diet.Info[] infos)
  {
    this.infos = infos;
    this.consumedTags = new List<KeyValuePair<Tag, float>>();
    this.producedTags = new List<KeyValuePair<Tag, float>>();
    foreach (Diet.Info info1 in infos)
    {
      Diet.Info info = info1;
      foreach (Tag consumedTag in info.consumedTags)
      {
        Tag tag = consumedTag;
        if (-1 == this.consumedTags.FindIndex((Predicate<KeyValuePair<Tag, float>>) (e => e.Key == tag)))
          this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
        if (this.consumedTagToInfo.ContainsKey(tag))
          Debug.LogError((object) ("Duplicate diet entry: " + tag.ToString()));
        this.consumedTagToInfo[tag] = info;
      }
      if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((Predicate<KeyValuePair<Tag, float>>) (e => e.Key == info.producedElement)))
        this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
    }
    this.UpdateSecondaryInfoArrays();
  }

  public Diet(Diet diet)
  {
    this.infos = new Diet.Info[diet.infos.Length];
    for (int index = 0; index < diet.infos.Length; ++index)
      this.infos[index] = new Diet.Info(diet.infos[index]);
    this.consumedTags = new List<KeyValuePair<Tag, float>>();
    this.producedTags = new List<KeyValuePair<Tag, float>>();
    foreach (Diet.Info info1 in this.infos)
    {
      Diet.Info info = info1;
      foreach (Tag consumedTag in info.consumedTags)
      {
        Tag tag = consumedTag;
        if (-1 == this.consumedTags.FindIndex((Predicate<KeyValuePair<Tag, float>>) (e => e.Key == tag)))
          this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
        if (this.consumedTagToInfo.ContainsKey(tag))
          Debug.LogError((object) ("Duplicate diet entry: " + tag.ToString()));
        this.consumedTagToInfo[tag] = info;
      }
      if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((Predicate<KeyValuePair<Tag, float>>) (e => e.Key == info.producedElement)))
        this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
    }
    this.UpdateSecondaryInfoArrays();
  }

  public Diet.Info GetDietInfo(Tag tag)
  {
    Diet.Info dietInfo = (Diet.Info) null;
    this.consumedTagToInfo.TryGetValue(tag, out dietInfo);
    return dietInfo;
  }

  public float AvailableCaloriesInPrey(Tag tag)
  {
    Diet.Info dietInfo1 = this.GetDietInfo(tag);
    if (dietInfo1 == null)
      return 0.0f;
    GameObject prefab = Assets.GetPrefab(tag);
    if (dietInfo1.foodType == Diet.Info.FoodType.EatPrey)
      return prefab.GetComponent<PrimaryElement>().Mass * dietInfo1.caloriesPerKg;
    Butcherable component = prefab.GetComponent<Butcherable>();
    float num = 0.0f;
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return 0.0f;
    foreach (KeyValuePair<string, float> drop in component.drops)
    {
      Diet.Info dietInfo2 = this.GetDietInfo(new Tag(drop.Key));
      if (dietInfo2 != null)
        num += drop.Value * dietInfo2.caloriesPerKg;
    }
    return num;
  }

  public void FilterDLC()
  {
    foreach (Diet.Info info in this.infos)
    {
      List<Tag> tagList = new List<Tag>();
      foreach (Tag consumedTag in info.consumedTags)
      {
        GameObject prefab = Assets.GetPrefab(consumedTag);
        if ((UnityEngine.Object) prefab == (UnityEngine.Object) null || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) prefab.GetComponent<KPrefabID>()))
          tagList.Add(consumedTag);
      }
      foreach (Tag tag in tagList)
      {
        Tag invalid_tag = tag;
        info.consumedTags.Remove(invalid_tag);
        this.consumedTags.RemoveAll((Predicate<KeyValuePair<Tag, float>>) (t => t.Key == invalid_tag));
        this.consumedTagToInfo.Remove(invalid_tag);
      }
      if (info.producedElement != Tag.Invalid)
      {
        GameObject prefab = Assets.GetPrefab(info.producedElement);
        if ((UnityEngine.Object) prefab == (UnityEngine.Object) null || !Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) prefab.GetComponent<KPrefabID>()))
          info.consumedTags.Clear();
      }
    }
    this.infos = ((IEnumerable<Diet.Info>) this.infos).Where<Diet.Info>((Func<Diet.Info, bool>) (i => i.consumedTags.Count > 0)).ToArray<Diet.Info>();
    this.UpdateSecondaryInfoArrays();
  }

  public class Info
  {
    public HashSet<Tag> consumedTags { get; private set; }

    public Tag producedElement { get; private set; }

    public float caloriesPerKg { get; private set; }

    public float producedConversionRate { get; private set; }

    public byte diseaseIdx { get; private set; }

    public float diseasePerKgProduced { get; private set; }

    public bool emmitDiseaseOnCell { get; private set; }

    public bool produceSolidTile { get; private set; }

    public Diet.Info.FoodType foodType { get; private set; }

    public string[] eatAnims { get; set; }

    public Info(
      HashSet<Tag> consumed_tags,
      Tag produced_element,
      float calories_per_kg,
      float produced_conversion_rate = 1f,
      string disease_id = null,
      float disease_per_kg_produced = 0.0f,
      bool produce_solid_tile = false,
      Diet.Info.FoodType food_type = Diet.Info.FoodType.EatSolid,
      bool emmit_disease_on_cell = false,
      string[] eat_anims = null)
    {
      this.consumedTags = consumed_tags;
      this.producedElement = produced_element;
      this.caloriesPerKg = calories_per_kg;
      this.producedConversionRate = produced_conversion_rate;
      this.diseaseIdx = string.IsNullOrEmpty(disease_id) ? byte.MaxValue : Db.Get().Diseases.GetIndex((HashedString) disease_id);
      this.diseasePerKgProduced = disease_per_kg_produced;
      this.emmitDiseaseOnCell = emmit_disease_on_cell;
      this.produceSolidTile = produce_solid_tile;
      this.foodType = food_type;
      if (eat_anims == null)
        eat_anims = new string[3]
        {
          "eat_pre",
          "eat_loop",
          "eat_pst"
        };
      this.eatAnims = eat_anims;
    }

    public Info(Diet.Info info)
    {
      this.consumedTags = new HashSet<Tag>((IEnumerable<Tag>) info.consumedTags);
      this.producedElement = info.producedElement;
      this.caloriesPerKg = info.caloriesPerKg;
      this.producedConversionRate = info.producedConversionRate;
      this.diseaseIdx = info.diseaseIdx;
      this.diseasePerKgProduced = info.diseasePerKgProduced;
      this.emmitDiseaseOnCell = info.emmitDiseaseOnCell;
      this.produceSolidTile = info.produceSolidTile;
      this.foodType = info.foodType;
      this.eatAnims = info.eatAnims;
    }

    public bool IsMatch(Tag tag) => this.consumedTags.Contains(tag);

    public bool IsMatch(HashSet<Tag> tags)
    {
      if (tags.Count < this.consumedTags.Count)
      {
        foreach (Tag tag in tags)
        {
          if (this.consumedTags.Contains(tag))
            return true;
        }
        return false;
      }
      foreach (Tag consumedTag in this.consumedTags)
      {
        if (tags.Contains(consumedTag))
          return true;
      }
      return false;
    }

    public float ConvertCaloriesToConsumptionMass(float calories) => calories / this.caloriesPerKg;

    public float ConvertConsumptionMassToCalories(float mass) => this.caloriesPerKg * mass;

    public float ConvertConsumptionMassToProducedMass(float consumed_mass)
    {
      return consumed_mass * this.producedConversionRate;
    }

    public float ConvertProducedMassToConsumptionMass(float produced_mass)
    {
      return produced_mass / this.producedConversionRate;
    }

    public enum FoodType
    {
      EatSolid,
      EatPlantDirectly,
      EatPlantStorage,
      EatPrey,
      EatButcheredPrey,
    }
  }
}
