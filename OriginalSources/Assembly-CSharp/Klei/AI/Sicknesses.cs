// Decompiled with JetBrains decompiler
// Type: Klei.AI.Sicknesses
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Klei.AI;

public class Sicknesses(GameObject go) : Modifications<Sickness, SicknessInstance>(go, (ResourceSet<Sickness>) Db.Get().Sicknesses)
{
  public void Infect(SicknessExposureInfo exposure_info)
  {
    Sickness modifier = Db.Get().Sicknesses.Get(exposure_info.sicknessID);
    if (this.Has(modifier))
      return;
    this.CreateInstance(modifier).ExposureInfo = exposure_info;
  }

  public override SicknessInstance CreateInstance(Sickness sickness)
  {
    SicknessInstance instance = new SicknessInstance(this.gameObject, sickness);
    this.Add(instance);
    this.Trigger(GameHashes.SicknessAdded, (object) instance);
    ReportManager.Instance.ReportValueWithGameObjectContext(ReportManager.ReportType.DiseaseAdded, 1f, this.gameObject);
    return instance;
  }

  public bool IsInfected() => this.Count > 0;

  public bool Cure(Sickness sickness) => this.Cure(sickness.Id);

  public bool Cure(string sickness_id)
  {
    SicknessInstance sicknessInstance = (SicknessInstance) null;
    foreach (SicknessInstance modifier in this.ModifierList)
    {
      if (modifier.modifier.Id == sickness_id)
      {
        sicknessInstance = modifier;
        break;
      }
    }
    if (sicknessInstance == null)
      return false;
    this.Remove(sicknessInstance);
    this.Trigger(GameHashes.SicknessCured, (object) sicknessInstance);
    ReportManager.Instance.ReportValueWithGameObjectContext(ReportManager.ReportType.DiseaseAdded, -1f, this.gameObject);
    return true;
  }
}
