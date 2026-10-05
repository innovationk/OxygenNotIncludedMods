// Decompiled with JetBrains decompiler
// Type: Database.Quests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Database;

public class Quests : ResourceSet<Quest>
{
  public Quest LonelyMinionGreetingQuest;
  public Quest LonelyMinionFoodQuest;
  public Quest LonelyMinionPowerQuest;
  public Quest LonelyMinionDecorQuest;
  public Quest FossilHuntQuest;

  public Quests(ResourceSet parent)
    : base(nameof (Quests), parent)
  {
    this.LonelyMinionGreetingQuest = this.Add(new Quest("KnockQuest", new QuestCriteria[1]
    {
      new QuestCriteria((Tag) "Neighbor")
    }));
    this.LonelyMinionFoodQuest = this.Add(new Quest("FoodQuest", new QuestCriteria[1]
    {
      (QuestCriteria) new QuestCriteria_GreaterOrEqual((Tag) "FoodQuality", new float[1]
      {
        4f
      }, 3, new HashSet<Tag>() { GameTags.Edible }, QuestCriteria.BehaviorFlags.UniqueItems)
    }));
    this.LonelyMinionPowerQuest = this.Add(new Quest("PluggedIn", new QuestCriteria[1]
    {
      (QuestCriteria) new QuestCriteria_GreaterOrEqual((Tag) "SuppliedPower", new float[1]
      {
        3000f
      })
    }));
    this.LonelyMinionDecorQuest = this.Add(new Quest("HighDecor", new QuestCriteria[1]
    {
      (QuestCriteria) new QuestCriteria_GreaterOrEqual((Tag) "Decor", new float[1]
      {
        120f
      }, flags: QuestCriteria.BehaviorFlags.AllowsRegression | QuestCriteria.BehaviorFlags.TrackValues)
    }));
    this.FossilHuntQuest = this.Add(new Quest(nameof (FossilHuntQuest), new QuestCriteria[4]
    {
      (QuestCriteria) new QuestCriteria_Equals((Tag) "LostSpecimen", new float[1]
      {
        1f
      }),
      (QuestCriteria) new QuestCriteria_Equals((Tag) "LostIceFossil", new float[1]
      {
        1f
      }),
      (QuestCriteria) new QuestCriteria_Equals((Tag) "LostResinFossil", new float[1]
      {
        1f
      }),
      (QuestCriteria) new QuestCriteria_Equals((Tag) "LostRockFossil", new float[1]
      {
        1f
      })
    }));
  }
}
