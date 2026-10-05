// Decompiled with JetBrains decompiler
// Type: Database.EatXKCalProducedByY
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Database;

public class EatXKCalProducedByY : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private int numCalories;
  private List<Tag> foodProducers;

  public EatXKCalProducedByY(int numCalories, List<Tag> foodProducers)
  {
    this.numCalories = numCalories;
    this.foodProducers = foodProducers;
  }

  public override bool Success()
  {
    List<string> source = new List<string>();
    foreach (ComplexRecipe recipe in ComplexRecipeManager.Get().recipes)
    {
      foreach (Tag foodProducer in this.foodProducers)
      {
        foreach (Tag fabricator in recipe.fabricators)
        {
          if (fabricator == foodProducer)
            source.Add(recipe.FirstResult.ToString());
        }
      }
    }
    return (double) WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(source.Distinct<string>().ToList<string>()) / 1000.0 > (double) this.numCalories;
  }

  public void Deserialize(IReader reader)
  {
    int capacity = reader.ReadInt32();
    this.foodProducers = new List<Tag>(capacity);
    for (int index = 0; index < capacity; ++index)
      this.foodProducers.Add(new Tag(reader.ReadKleiString()));
    this.numCalories = reader.ReadInt32();
  }

  public override string GetProgress(bool complete)
  {
    string str = "";
    for (int index = 0; index < this.foodProducers.Count; ++index)
    {
      if (index != 0)
        str += (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR;
      BuildingDef buildingDef = Assets.GetBuildingDef(this.foodProducers[index].Name);
      if ((Object) buildingDef != (Object) null)
        str += buildingDef.Name;
    }
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_ITEM, (object) str);
  }
}
