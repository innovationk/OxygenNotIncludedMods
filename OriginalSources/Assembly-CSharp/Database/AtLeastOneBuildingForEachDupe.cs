// Decompiled with JetBrains decompiler
// Type: Database.AtLeastOneBuildingForEachDupe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

#nullable disable
namespace Database;

public class AtLeastOneBuildingForEachDupe : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private List<Tag> validBuildingTypes = new List<Tag>();

  public AtLeastOneBuildingForEachDupe(List<Tag> validBuildingTypes)
  {
    this.validBuildingTypes = validBuildingTypes;
  }

  public override bool Success()
  {
    if (Components.LiveMinionIdentities.Items.Count <= 0)
      return false;
    int num = 0;
    foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
    {
      Tag prefabTag = basicBuilding.transform.GetComponent<KPrefabID>().PrefabTag;
      if (this.validBuildingTypes.Contains(prefabTag))
      {
        ++num;
        if (prefabTag == (Tag) "FlushToilet" || prefabTag == (Tag) "Outhouse")
          return true;
      }
    }
    return num >= Components.LiveMinionIdentities.Items.Count;
  }

  public override bool Fail() => false;

  public void Deserialize(IReader reader)
  {
    int capacity = reader.ReadInt32();
    this.validBuildingTypes = new List<Tag>(capacity);
    for (int index = 0; index < capacity; ++index)
      this.validBuildingTypes.Add(new Tag(reader.ReadKleiString()));
  }

  public override string GetProgress(bool complete)
  {
    if (this.validBuildingTypes.Contains((Tag) "FlushToilet"))
      return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_TOILET;
    if (complete)
      return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_BED_PER_DUPLICANT;
    int num = 0;
    foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
    {
      if (this.validBuildingTypes.Contains(basicBuilding.transform.GetComponent<KPrefabID>().PrefabTag))
        ++num;
    }
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILING_BEDS, (object) (complete ? Components.LiveMinionIdentities.Items.Count : num), (object) Components.LiveMinionIdentities.Items.Count);
  }
}
