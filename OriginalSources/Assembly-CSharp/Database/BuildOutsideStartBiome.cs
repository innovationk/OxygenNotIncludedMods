// Decompiled with JetBrains decompiler
// Type: Database.BuildOutsideStartBiome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;

#nullable disable
namespace Database;

public class BuildOutsideStartBiome : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  public override bool Success()
  {
    WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
    foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
    {
      if (!buildingComplete.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
      {
        for (int index = 0; index < clusterDetailSave.overworldCells.Count; ++index)
        {
          WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[index];
          if (overworldCell.tags != null && !overworldCell.tags.Contains(WorldGenTags.StartWorld) && overworldCell.poly.PointInPolygon((Vector2) buildingComplete.transform.GetPosition()))
          {
            Game.Instance.unlocks.Unlock("buildoutsidestartingbiome");
            return true;
          }
        }
      }
    }
    return false;
  }

  public void Deserialize(IReader reader)
  {
  }

  public override string GetProgress(bool complete)
  {
    return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_OUTSIDE_START;
  }
}
