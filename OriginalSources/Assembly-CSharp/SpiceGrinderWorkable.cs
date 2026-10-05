// Decompiled with JetBrains decompiler
// Type: SpiceGrinderWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Linq;
using TUNING;
using UnityEngine;

#nullable disable
public class SpiceGrinderWorkable : Workable, IConfigurableConsumer
{
  [MyCmpAdd]
  public Notifier notifier;
  [SerializeField]
  public Vector3 finishedSeedDropOffset;
  public SpiceGrinder.StatesInstance Grinder;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.requiredSkillPerk = Db.Get().SkillPerks.CanSpiceGrinder.Id;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Spicing;
    this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_spice_grinder_kanim")
    };
    this.SetWorkTime(5f);
    this.showProgressBar = true;
    this.lightEfficiencyBonus = true;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    if ((Object) this.Grinder.CurrentFood != (Object) null)
    {
      this.SetWorkTime((float) ((double) this.Grinder.CurrentFood.Calories * (1.0 / 1000.0) / 1000.0) * 5f);
    }
    else
    {
      Debug.LogWarning((object) "SpiceGrider attempted to start spicing with no food");
      this.StopWork(worker, true);
    }
    this.Grinder.UpdateFoodSymbol();
  }

  protected override void OnAbortWork(WorkerBase worker)
  {
    if ((Object) this.Grinder.CurrentFood == (Object) null)
      return;
    this.Grinder.UpdateFoodSymbol();
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    if ((Object) this.Grinder.CurrentFood == (Object) null)
      return;
    this.Grinder.SpiceFood();
  }

  public IConfigurableConsumerOption[] GetSettingOptions()
  {
    return (IConfigurableConsumerOption[]) SpiceGrinder.SettingOptions.Values.ToArray<SpiceGrinder.Option>();
  }

  public IConfigurableConsumerOption GetSelectedOption()
  {
    return (IConfigurableConsumerOption) this.Grinder.SelectedOption;
  }

  public void SetSelectedOption(IConfigurableConsumerOption option)
  {
    this.Grinder.OnOptionSelected(option as SpiceGrinder.Option);
  }
}
