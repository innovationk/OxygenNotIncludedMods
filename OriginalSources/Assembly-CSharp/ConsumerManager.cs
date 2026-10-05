// Decompiled with JetBrains decompiler
// Type: ConsumerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConsumerManager")]
public class ConsumerManager : KMonoBehaviour, ISaveLoadable
{
  public static ConsumerManager instance;
  [Serialize]
  private List<Tag> undiscoveredConsumableTags = new List<Tag>();
  [Serialize]
  private List<Tag> defaultForbiddenTagsList = new List<Tag>();
  public static string OXYGEN_TANK_ID = ClosestOxygenCanisterSensor.GenericBreathableGassesTankTag.ToString();

  public static void DestroyInstance() => ConsumerManager.instance = (ConsumerManager) null;

  public event Action<Tag> OnDiscover;

  public List<Tag> DefaultForbiddenTagsList => this.defaultForbiddenTagsList;

  public List<Tag> StandardDuplicantDietaryRestrictions
  {
    get
    {
      List<Tag> dietaryRestrictions = new List<Tag>();
      foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.ChargedPortableBattery))
        dietaryRestrictions.Add(go.PrefabID());
      dietaryRestrictions.Add((Tag) ConsumerManager.OXYGEN_TANK_ID);
      return dietaryRestrictions;
    }
  }

  public List<Tag> BionicDuplicantDietaryRestrictions
  {
    get
    {
      List<Tag> dietaryRestrictions = new List<Tag>();
      foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Edible))
        dietaryRestrictions.Add(go.PrefabID());
      Tag[] array = new Tag[GameTags.BionicIncompatibleBatteries.Count];
      GameTags.BionicIncompatibleBatteries.CopyTo(array, 0);
      foreach (Tag tag in array)
        dietaryRestrictions.Add(tag);
      return dietaryRestrictions;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    ConsumerManager.instance = this;
    this.RefreshDiscovered();
    DiscoveredResources.Instance.OnDiscover += new Action<Tag, Tag>(this.OnWorldInventoryDiscover);
    Game.Instance.Subscribe(-107300940, new Action<object>(this.RefreshDiscovered));
  }

  public bool isDiscovered(Tag id) => !this.undiscoveredConsumableTags.Contains(id);

  private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
  {
    if (!this.undiscoveredConsumableTags.Contains(tag))
      return;
    this.RefreshDiscovered();
  }

  public void RefreshDiscovered(object data = null)
  {
    foreach (EdiblesManager.FoodInfo allFoodType in EdiblesManager.GetAllFoodTypes())
    {
      if (!this.ShouldBeDiscovered(allFoodType.Id.ToTag()) && !this.undiscoveredConsumableTags.Contains(allFoodType.Id.ToTag()))
      {
        this.undiscoveredConsumableTags.Add(allFoodType.Id.ToTag());
        if (this.OnDiscover != null)
          this.OnDiscover("UndiscoveredSomething".ToTag());
      }
      else if (this.undiscoveredConsumableTags.Contains(allFoodType.Id.ToTag()) && this.ShouldBeDiscovered(allFoodType.Id.ToTag()))
      {
        this.undiscoveredConsumableTags.Remove(allFoodType.Id.ToTag());
        if (this.OnDiscover != null)
          this.OnDiscover(allFoodType.Id.ToTag());
        if (!DiscoveredResources.Instance.IsDiscovered(allFoodType.Id.ToTag()))
        {
          if ((double) allFoodType.CaloriesPerUnit == 0.0)
            DiscoveredResources.Instance.Discover(allFoodType.Id.ToTag(), GameTags.CookingIngredient);
          else
            DiscoveredResources.Instance.Discover(allFoodType.Id.ToTag(), GameTags.Edible);
        }
      }
    }
  }

  private bool ShouldBeDiscovered(Tag food_id)
  {
    if (DiscoveredResources.Instance.IsDiscovered(food_id))
      return true;
    foreach (Recipe recipe in RecipeManager.Get().recipes)
    {
      if (recipe.Result == food_id)
      {
        foreach (string fabricator in recipe.fabricators)
        {
          if (Db.Get().TechItems.IsTechItemComplete(fabricator))
            return true;
        }
      }
    }
    foreach (Crop crop in Components.Crops.Items)
    {
      if (Grid.IsVisible(Grid.PosToCell(crop.gameObject)) && crop.cropId == food_id.Name)
        return true;
    }
    return false;
  }
}
