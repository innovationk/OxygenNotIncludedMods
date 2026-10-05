// Decompiled with JetBrains decompiler
// Type: MedicineInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class MedicineInfo
{
  public string id;
  public string effect;
  public MedicineInfo.MedicineType medicineType;
  public List<string> curedSicknesses;
  public List<string> curedEffects;
  public string doctorStationId;

  public MedicineInfo(
    string id,
    string effect,
    MedicineInfo.MedicineType medicineType,
    string doctorStationId,
    string[] curedDiseases = null)
    : this(id, effect, medicineType, doctorStationId, curedDiseases, (string[]) null)
  {
  }

  public MedicineInfo(
    string id,
    string effect,
    MedicineInfo.MedicineType medicineType,
    string doctorStationId,
    string[] curedDiseases,
    string[] curedEffects)
  {
    Debug.Assert(!string.IsNullOrEmpty(effect) || curedDiseases != null && curedDiseases.Length != 0, (object) "Medicine should have an effect or cure diseases");
    this.id = id;
    this.effect = effect;
    this.medicineType = medicineType;
    this.doctorStationId = doctorStationId;
    this.curedSicknesses = curedDiseases == null ? new List<string>() : new List<string>((IEnumerable<string>) curedDiseases);
    if (curedEffects != null)
      this.curedEffects = new List<string>((IEnumerable<string>) curedEffects);
    else
      this.curedEffects = new List<string>();
  }

  public Tag GetSupplyTag() => MedicineInfo.GetSupplyTagForStation(this.doctorStationId);

  public static Tag GetSupplyTagForStation(string stationID)
  {
    Tag tag = TagManager.Create(stationID + GameTags.MedicalSupplies.Name);
    Assets.AddCountableTag(tag);
    return tag;
  }

  public enum MedicineType
  {
    Booster,
    CureAny,
    CureSpecific,
  }
}
