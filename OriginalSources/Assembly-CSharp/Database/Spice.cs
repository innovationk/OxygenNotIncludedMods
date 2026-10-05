// Decompiled with JetBrains decompiler
// Type: Database.Spice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
namespace Database;

public class Spice : Resource, IHasDlcRestrictions
{
  public readonly Spice.Ingredient[] Ingredients;
  public readonly float TotalKG;

  public AttributeModifier StatBonus { get; private set; }

  public AttributeModifier FoodModifier { get; private set; }

  public AttributeModifier CalorieModifier { get; private set; }

  public Color PrimaryColor { get; private set; }

  public Color SecondaryColor { get; private set; }

  public string Image { private set; get; }

  public string[] requiredDlcIds { private set; get; }

  public Spice(
    ResourceSet parent,
    string id,
    Spice.Ingredient[] ingredients,
    Color primaryColor,
    Color secondaryColor,
    AttributeModifier foodMod = null,
    AttributeModifier statBonus = null,
    string imageName = "unknown",
    string[] dlcID = null)
    : base(id, parent)
  {
    this.requiredDlcIds = this.requiredDlcIds;
    this.StatBonus = statBonus;
    this.FoodModifier = foodMod;
    this.Ingredients = ingredients;
    this.Image = imageName;
    this.PrimaryColor = primaryColor;
    this.SecondaryColor = secondaryColor;
    for (int index = 0; index < this.Ingredients.Length; ++index)
      this.TotalKG += this.Ingredients[index].AmountKG;
  }

  public string[] GetRequiredDlcIds() => this.requiredDlcIds;

  public string[] GetForbiddenDlcIds() => (string[]) null;

  public class Ingredient : IConfigurableConsumerIngredient
  {
    public Tag[] IngredientSet;
    public float AmountKG;

    public float GetAmount() => this.AmountKG;

    public Tag[] GetIDSets() => this.IngredientSet;
  }
}
