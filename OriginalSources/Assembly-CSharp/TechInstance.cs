// Decompiled with JetBrains decompiler
// Type: TechInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TechInstance
{
  public Tech tech;
  private bool complete;
  public ResearchPointInventory progressInventory = new ResearchPointInventory();
  public List<string> UnlockedPOITechIds = new List<string>();

  public TechInstance(Tech tech) => this.tech = tech;

  public bool IsComplete() => this.complete;

  public void Purchased()
  {
    if (this.complete)
      return;
    this.complete = true;
  }

  public void UnlockPOITech(string tech_id)
  {
    TechItem techItem = Db.Get().TechItems.Get(tech_id);
    if (techItem == null || !techItem.isPOIUnlock || this.UnlockedPOITechIds.Contains(tech_id))
      return;
    this.UnlockedPOITechIds.Add(tech_id);
    BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
    if (!((Object) buildingDef != (Object) null))
      return;
    Game.Instance.Trigger(-107300940, (object) buildingDef);
  }

  public float GetTotalPercentageComplete()
  {
    float num1 = 0.0f;
    int num2 = 0;
    foreach (string key in this.progressInventory.PointsByTypeID.Keys)
    {
      if (this.tech.RequiresResearchType(key))
      {
        num1 += this.PercentageCompleteResearchType(key);
        ++num2;
      }
    }
    return num1 / (float) num2;
  }

  public float PercentageCompleteResearchType(string type)
  {
    return !this.tech.RequiresResearchType(type) ? 1f : Mathf.Clamp01(this.progressInventory.PointsByTypeID[type] / this.tech.costsByResearchTypeID[type]);
  }

  public TechInstance.SaveData Save()
  {
    string[] array1 = new string[this.progressInventory.PointsByTypeID.Count];
    this.progressInventory.PointsByTypeID.Keys.CopyTo(array1, 0);
    float[] array2 = new float[this.progressInventory.PointsByTypeID.Count];
    this.progressInventory.PointsByTypeID.Values.CopyTo(array2, 0);
    string[] array3 = this.UnlockedPOITechIds.ToArray();
    return new TechInstance.SaveData()
    {
      techId = this.tech.Id,
      complete = this.complete,
      inventoryIDs = array1,
      inventoryValues = array2,
      unlockedPOIIDs = array3
    };
  }

  public void Load(TechInstance.SaveData save_data)
  {
    this.complete = save_data.complete;
    for (int index = 0; index < save_data.inventoryIDs.Length; ++index)
      this.progressInventory.AddResearchPoints(save_data.inventoryIDs[index], save_data.inventoryValues[index]);
    if (save_data.unlockedPOIIDs == null)
      return;
    this.UnlockedPOITechIds = new List<string>((IEnumerable<string>) save_data.unlockedPOIIDs);
  }

  public struct SaveData
  {
    public string techId;
    public bool complete;
    public string[] inventoryIDs;
    public float[] inventoryValues;
    public string[] unlockedPOIIDs;
  }
}
