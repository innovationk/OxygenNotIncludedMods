// Decompiled with JetBrains decompiler
// Type: Database.AutomateABuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Database;

public class AutomateABuilding : 
  ColonyAchievementRequirement,
  AchievementRequirementSerialization_Deprecated
{
  public override bool Success()
  {
    foreach (LogicCircuitNetwork network in (IEnumerable<UtilityNetwork>) Game.Instance.logicCircuitSystem.GetNetworks())
    {
      if (network.Receivers.Count > 0 && network.Senders.Count > 0)
      {
        bool flag1 = false;
        foreach (ILogicEventReceiver receiver in network.Receivers)
        {
          if (!receiver.IsNullOrDestroyed())
          {
            GameObject gameObject = Grid.Objects[receiver.GetLogicCell(), 1];
            if ((Object) gameObject != (Object) null && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
            {
              flag1 = true;
              break;
            }
          }
        }
        bool flag2 = false;
        foreach (ILogicEventSender sender in network.Senders)
        {
          if (!sender.IsNullOrDestroyed())
          {
            GameObject gameObject = Grid.Objects[sender.GetLogicCell(), 1];
            if ((Object) gameObject != (Object) null && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
            {
              flag2 = true;
              break;
            }
          }
        }
        if (flag1 & flag2)
          return true;
      }
    }
    return false;
  }

  public void Deserialize(IReader reader)
  {
  }

  public override string GetProgress(bool complete)
  {
    return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.AUTOMATE_A_BUILDING;
  }
}
