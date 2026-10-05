// Decompiled with JetBrains decompiler
// Type: Database.MinnowRecruited
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Database;

public class MinnowRecruited : VictoryColonyAchievementRequirement
{
  private static string[] requirementNamePATH = new string[3]
  {
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_NAME_A",
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_NAME_B",
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_NAME_C"
  };
  private static string[] requirementDescriptionPATH = new string[3]
  {
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_DESCRIPTION_A",
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_DESCRIPTION_B",
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_DESCRIPTION_C"
  };
  private static string[] requirementNamePATH_HIDDEN = new string[3]
  {
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_NAME_A_HIDDEN",
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_NAME_B_HIDDEN",
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_NAME_C_HIDDEN"
  };
  private static string[] requirementDescriptionPATH_HIDDEN = new string[3]
  {
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_DESCRIPTION_A_HIDDEN",
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_DESCRIPTION_B_HIDDEN",
    "STRINGS.COLONY_ACHIEVEMENTS.FINDING_MINNOW.ACHIEVEMENTS.REQUIREMENT_DESCRIPTION_C_HIDDEN"
  };
  private MinnowImperativePOIStates.MinnowPOIIdentity minnowIdentity;

  public MinnowRecruited(
    MinnowImperativePOIStates.MinnowPOIIdentity minnowIdentity)
  {
    this.shouldUpdateNameAndDescription = true;
    this.minnowIdentity = minnowIdentity;
  }

  public override string GetProgress(bool complete)
  {
    int minnowIdentity = (int) this.minnowIdentity;
    MinnowImperativePOIStates.Instance minnowInstance = this.GetMinnowInstance();
    return (string) Strings.Get((complete ? 0 : (minnowInstance == null ? 1 : (!minnowInstance.HasUserEverClicked ? 1 : 0))) != 0 ? MinnowRecruited.requirementDescriptionPATH_HIDDEN[minnowIdentity] : MinnowRecruited.requirementDescriptionPATH[minnowIdentity]);
  }

  public override string Description() => (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.DESCRIPTION;

  private MinnowImperativePOIStates.Instance GetMinnowInstance()
  {
    foreach (MinnowImperativePOIStates.Instance minnowInstance in Components.MinnowImperativePOIs.Items)
    {
      if (minnowInstance.def.minnowPOIIdentity == this.minnowIdentity)
        return minnowInstance;
    }
    return (MinnowImperativePOIStates.Instance) null;
  }

  public override bool Success()
  {
    if (SaveGame.Instance.ColonyAchievementTracker.allMinnowQuestsCompleted)
      return true;
    MinnowImperativePOIStates.Instance minnowInstance = this.GetMinnowInstance();
    return minnowInstance != null && minnowInstance.WasCompletedAndAcknowledged && !MinnowImperativePOIStates.Instance.AllPOIsCompleted();
  }

  public override string Name()
  {
    int minnowIdentity = (int) this.minnowIdentity;
    MinnowImperativePOIStates.Instance minnowInstance = this.GetMinnowInstance();
    int num = (minnowInstance == null ? 0 : (minnowInstance.WasCompletedAndAcknowledged ? 1 : 0)) != 0 ? 0 : (minnowInstance == null ? 1 : (!minnowInstance.HasUserEverClicked ? 1 : 0));
    float mass = minnowInstance != null ? minnowInstance.def.requiredMass : 0.0f;
    Tag requestedTag = minnowInstance != null ? minnowInstance.def.requestedTag : (Tag) (string) null;
    EdiblesManager.FoodInfo foodInfo = requestedTag != (Tag) (string) null ? EdiblesManager.GetFoodInfo(requestedTag.Name) : (EdiblesManager.FoodInfo) null;
    string str = (string) Strings.Get(num != 0 ? MinnowRecruited.requirementNamePATH_HIDDEN[minnowIdentity] : MinnowRecruited.requirementNamePATH[minnowIdentity]);
    float calories = foodInfo != null ? foodInfo.CaloriesPerUnit * mass : 0.0f;
    string newValue = (double) calories != 0.0 ? GameUtil.GetFormattedCalories(calories) : GameUtil.GetFormattedMass(mass);
    return str.Replace("{AMOUNT}", newValue);
  }
}
