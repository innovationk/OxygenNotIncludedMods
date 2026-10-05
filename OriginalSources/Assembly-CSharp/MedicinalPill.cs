// Decompiled with JetBrains decompiler
// Type: MedicinalPill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/game/MedicinalPill")]
public class MedicinalPill : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public MedicineInfo info;

  protected override void OnSpawn() => base.OnSpawn();

  public List<Descriptor> EffectDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (string.IsNullOrEmpty(this.info.doctorStationId))
    {
      if (this.info.medicineType == MedicineInfo.MedicineType.Booster)
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_BOOSTER), string.Format((string) DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_BOOSTER_TOOLTIP)));
      else
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_CURE), string.Format((string) DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_CURE_TOOLTIP)));
    }
    else
    {
      string properName = Assets.GetPrefab((Tag) this.info.doctorStationId).GetProperName();
      if (this.info.medicineType == MedicineInfo.MedicineType.Booster)
        descriptorList.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_BOOSTER.Replace("{Station}", properName)), string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_BOOSTER_TOOLTIP.Replace("{Station}", properName))));
      else
        descriptorList.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_CURE.Replace("{Station}", properName)), string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_CURE_TOOLTIP.Replace("{Station}", properName))));
    }
    switch (this.info.medicineType)
    {
      case MedicineInfo.MedicineType.CureAny:
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.MEDICINE.CURES_ANY), string.Format((string) DUPLICANTS.DISEASES.MEDICINE.CURES_ANY_TOOLTIP)));
        break;
      case MedicineInfo.MedicineType.CureSpecific:
        List<string> stringList = new List<string>();
        foreach (string curedSickness in this.info.curedSicknesses)
          stringList.Add((string) Strings.Get($"STRINGS.DUPLICANTS.DISEASES.{curedSickness.ToUpper()}.NAME"));
        string str = string.Join(",", stringList.ToArray());
        descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.DISEASES.MEDICINE.CURES, (object) str), string.Format((string) DUPLICANTS.DISEASES.MEDICINE.CURES_TOOLTIP, (object) str)));
        break;
    }
    if (!string.IsNullOrEmpty(this.info.effect))
    {
      Effect effect = Db.Get().effects.Get(this.info.effect);
      descriptorList.Add(new Descriptor(string.Format((string) DUPLICANTS.MODIFIERS.MEDICINE_GENERICPILL.EFFECT_DESC, (object) effect.Name), $"{effect.description}\n{Effect.CreateTooltip(effect, true)}"));
    }
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(GameObject go) => this.EffectDescriptors(go);
}
