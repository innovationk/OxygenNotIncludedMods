// Decompiled with JetBrains decompiler
// Type: TUNING.MEDICINE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace TUNING;

public class MEDICINE
{
  public const float DEFAULT_MASS = 1f;
  public const float RECUPERATION_DISEASE_MULTIPLIER = 1.1f;
  public const float RECUPERATION_DOCTORED_DISEASE_MULTIPLIER = 1.2f;
  public const float WORK_TIME = 10f;
  public static readonly MedicineInfo BASICBOOSTER = new MedicineInfo("BasicBooster", "Medicine_BasicBooster", MedicineInfo.MedicineType.Booster, (string) null);
  public static readonly MedicineInfo INTERMEDIATEBOOSTER = new MedicineInfo("IntermediateBooster", "Medicine_IntermediateBooster", MedicineInfo.MedicineType.Booster, (string) null);
  public static readonly MedicineInfo BASICCURE = new MedicineInfo("BasicCure", (string) null, MedicineInfo.MedicineType.CureSpecific, (string) null, new string[1]
  {
    "FoodSickness"
  });
  public static readonly MedicineInfo ANTIHISTAMINE = new MedicineInfo("Antihistamine", "HistamineSuppression", MedicineInfo.MedicineType.CureSpecific, (string) null, new string[1]
  {
    "Allergies"
  }, new string[1]{ "DupeMosquitoBite" });
  public static readonly MedicineInfo INTERMEDIATECURE = new MedicineInfo("IntermediateCure", (string) null, MedicineInfo.MedicineType.CureSpecific, "DoctorStation", new string[1]
  {
    "SlimeSickness"
  });
  public static readonly MedicineInfo ADVANCEDCURE = new MedicineInfo("AdvancedCure", (string) null, MedicineInfo.MedicineType.CureSpecific, "AdvancedDoctorStation", new string[1]
  {
    "ZombieSickness"
  });
  public static readonly MedicineInfo BASICRADPILL = new MedicineInfo("BasicRadPill", "Medicine_BasicRadPill", MedicineInfo.MedicineType.Booster, (string) null);
  public static readonly MedicineInfo INTERMEDIATERADPILL = new MedicineInfo("IntermediateRadPill", "Medicine_IntermediateRadPill", MedicineInfo.MedicineType.Booster, "AdvancedDoctorStation");
}
