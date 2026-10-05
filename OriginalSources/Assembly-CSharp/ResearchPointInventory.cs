// Decompiled with JetBrains decompiler
// Type: ResearchPointInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ResearchPointInventory
{
  public Dictionary<string, float> PointsByTypeID = new Dictionary<string, float>();

  public ResearchPointInventory()
  {
    foreach (ResearchType type in Research.Instance.researchTypes.Types)
      this.PointsByTypeID.Add(type.id, 0.0f);
  }

  public void AddResearchPoints(string researchTypeID, float points)
  {
    if (!this.PointsByTypeID.ContainsKey(researchTypeID))
      Debug.LogWarning((object) ("Research inventory is missing research point key " + researchTypeID));
    else
      this.PointsByTypeID[researchTypeID] += points;
  }

  public void RemoveResearchPoints(string researchTypeID, float points)
  {
    this.AddResearchPoints(researchTypeID, -points);
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    foreach (ResearchType type in Research.Instance.researchTypes.Types)
    {
      if (!this.PointsByTypeID.ContainsKey(type.id))
        this.PointsByTypeID.Add(type.id, 0.0f);
    }
  }
}
