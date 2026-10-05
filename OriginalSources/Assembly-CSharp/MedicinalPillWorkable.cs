// Decompiled with JetBrains decompiler
// Type: MedicinalPillWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/MedicinalPillWorkable")]
public class MedicinalPillWorkable : Workable, IConsumableUIItem
{
  public MedicinalPill pill;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(10f);
    this.showProgressBar = false;
    this.synchronizeAnims = false;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal);
    this.CreateChore();
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Effects component = worker.GetComponent<Effects>();
    if (!string.IsNullOrEmpty(this.pill.info.effect))
    {
      EffectInstance effectInstance = component.Get(this.pill.info.effect);
      if (effectInstance != null)
        effectInstance.timeRemaining = effectInstance.effect.duration;
      else
        component.Add(this.pill.info.effect, true);
    }
    Sicknesses sicknesses = worker.GetSicknesses();
    foreach (string curedSickness in this.pill.info.curedSicknesses)
    {
      SicknessInstance sicknessInstance = sicknesses.Get(curedSickness);
      if (sicknessInstance != null)
      {
        Game.Instance.savedInfo.curedDisease = true;
        sicknessInstance.Cure();
      }
    }
    foreach (string curedEffect in this.pill.info.curedEffects)
    {
      if (component.HasEffect(curedEffect))
      {
        Game.Instance.savedInfo.curedDisease = true;
        component.Remove(curedEffect);
      }
    }
    this.gameObject.DeleteObject();
  }

  private void CreateChore()
  {
    TakeMedicineChore takeMedicineChore = new TakeMedicineChore(this);
  }

  public bool CanBeTakenBy(GameObject consumer)
  {
    if (!string.IsNullOrEmpty(this.pill.info.effect))
    {
      Effects component = consumer.GetComponent<Effects>();
      if ((Object) component == (Object) null || component.HasEffect(this.pill.info.effect))
        return false;
    }
    if (this.pill.info.medicineType == MedicineInfo.MedicineType.Booster)
      return true;
    Sicknesses sicknesses = consumer.GetSicknesses();
    if (this.pill.info.medicineType == MedicineInfo.MedicineType.CureAny && sicknesses.Count > 0)
      return true;
    foreach (ModifierInstance<Sickness> modifier in sicknesses.ModifierList)
    {
      if (this.pill.info.curedSicknesses.Contains(modifier.modifier.Id))
        return true;
    }
    Effects component1 = consumer.GetComponent<Effects>();
    for (int index = 0; index < this.pill.info.curedEffects.Count; ++index)
    {
      string curedEffect = this.pill.info.curedEffects[index];
      if (component1.HasEffect(curedEffect))
        return true;
    }
    return false;
  }

  public string ConsumableId => this.PrefabID().Name;

  public string ConsumableName => this.GetProperName();

  public int MajorOrder => (int) (this.pill.info.medicineType + 1000);

  public int MinorOrder => 0;

  public bool Display => true;
}
