// Decompiled with JetBrains decompiler
// Type: ChoreCountTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ChoreCountTracker : WorldTracker
{
  public ChoreGroup choreGroup;

  public ChoreCountTracker(int worldID, ChoreGroup group)
    : base(worldID)
  {
    this.choreGroup = group;
  }

  public override void UpdateData()
  {
    float num = 0.0f;
    List<Chore> choreList;
    GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(this.WorldID, out choreList);
    for (int index = 0; choreList != null && index < choreList.Count; ++index)
    {
      Chore chore = choreList[index];
      if (chore != null && !chore.target.Equals((object) null) && !((Object) chore.gameObject == (Object) null))
      {
        foreach (ChoreGroup group in chore.choreType.groups)
        {
          if (group == this.choreGroup)
          {
            ++num;
            break;
          }
        }
      }
    }
    List<FetchChore> fetchChoreList;
    GlobalChoreProvider.Instance.fetchMap.TryGetValue(this.WorldID, out fetchChoreList);
    for (int index = 0; fetchChoreList != null && index < fetchChoreList.Count; ++index)
    {
      Chore chore = (Chore) fetchChoreList[index];
      if (chore != null && !chore.target.Equals((object) null) && !((Object) chore.gameObject == (Object) null))
      {
        foreach (ChoreGroup group in chore.choreType.groups)
        {
          if (group == this.choreGroup)
          {
            ++num;
            break;
          }
        }
      }
    }
    this.AddPoint(num);
  }

  public override string FormatValueString(float value) => value.ToString();
}
