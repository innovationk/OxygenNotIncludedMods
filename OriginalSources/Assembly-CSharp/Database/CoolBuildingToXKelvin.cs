// Decompiled with JetBrains decompiler
// Type: Database.CoolBuildingToXKelvin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class CoolBuildingToXKelvin : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  private int kelvinToCoolTo;

  public CoolBuildingToXKelvin(int kelvinToCoolTo) => this.kelvinToCoolTo = kelvinToCoolTo;

  public override bool Success()
  {
    return (double) BuildingComplete.MinKelvinSeen <= (double) this.kelvinToCoolTo;
  }

  public void Deserialize(IReader reader) => this.kelvinToCoolTo = reader.ReadInt32();

  public override string GetProgress(bool complete)
  {
    float minKelvinSeen = BuildingComplete.MinKelvinSeen;
    return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.KELVIN_COOLING, (object) minKelvinSeen);
  }
}
