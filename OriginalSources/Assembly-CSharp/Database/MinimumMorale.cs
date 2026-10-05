// Decompiled with JetBrains decompiler
// Type: Database.MinimumMorale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
namespace Database;

public class MinimumMorale : 
  VictoryColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  public int minimumMorale;

  public override string Name()
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE, (object) this.minimumMorale);
  }

  public override string Description()
  {
    return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE_DESCRIPTION, (object) this.minimumMorale);
  }

  public MinimumMorale(int minimumMorale = 16 /*0x10*/) => this.minimumMorale = minimumMorale;

  public override bool Success()
  {
    bool flag = true;
    foreach (MinionAssignablesProxy assignablesProxy in Components.MinionAssignablesProxy)
    {
      GameObject targetGameObject = assignablesProxy.GetTargetGameObject();
      if ((Object) targetGameObject != (Object) null && !targetGameObject.HasTag(GameTags.Dead))
      {
        AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup((Component) targetGameObject.GetComponent<MinionModifiers>());
        flag = ((attributeInstance == null ? 0 : ((double) attributeInstance.GetTotalValue() >= (double) this.minimumMorale ? 1 : 0)) & (flag ? 1 : 0)) != 0;
      }
    }
    return flag;
  }

  public void Deserialize(IReader reader) => this.minimumMorale = reader.ReadInt32();

  public override string GetProgress(bool complete) => this.Description();
}
