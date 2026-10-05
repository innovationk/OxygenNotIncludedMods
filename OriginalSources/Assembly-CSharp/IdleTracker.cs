// Decompiled with JetBrains decompiler
// Type: IdleTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class IdleTracker(int worldID) : WorldTracker(worldID)
{
  public override void UpdateData()
  {
    this.objectsOfInterest.Clear();
    int num = 0;
    List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(this.WorldID);
    for (int index = 0; index < worldItems.Count; ++index)
    {
      if (worldItems[index].HasTag(GameTags.Idle))
      {
        ++num;
        this.objectsOfInterest.Add(worldItems[index].gameObject);
      }
    }
    this.AddPoint((float) num);
  }

  public override string FormatValueString(float value) => value.ToString();
}
