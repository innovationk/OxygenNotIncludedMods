// Decompiled with JetBrains decompiler
// Type: GeneticAnalysisStationWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

#nullable disable
public class GeneticAnalysisStationWorkable : Workable
{
  [MyCmpAdd]
  public Notifier notifier;
  [MyCmpReq]
  public Storage storage;
  [SerializeField]
  public Vector3 finishedSeedDropOffset;
  private Notification notification;
  public GeneticAnalysisStation.StatesInstance statesInstance;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.requiredSkillPerk = Db.Get().SkillPerks.CanIdentifyMutantSeeds.Id;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingGenes;
    this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_genetic_analysisstation_kanim")
    };
    this.SetWorkTime(150f);
    this.showProgressBar = true;
    this.lightEfficiencyBonus = true;
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, (object) this.storage.FindFirst(GameTags.UnidentifiedSeed));
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    this.IdentifyMutant();
  }

  public void IdentifyMutant()
  {
    GameObject first = this.storage.FindFirst(GameTags.UnidentifiedSeed);
    DebugUtil.DevAssertArgs(((Object) first != (Object) null ? 1 : 0) != 0, (object) "AAACCCCKKK!! GeneticAnalysisStation finished studying a seed but we don't have one in storage??");
    if (!((Object) first != (Object) null))
      return;
    Pickupable component1 = first.GetComponent<Pickupable>();
    Pickupable pickupable = (double) component1.PrimaryElement.Units <= 1.0 ? this.storage.Drop(first, true).GetComponent<Pickupable>() : component1.TakeUnit(1f);
    pickupable.transform.SetPosition(this.transform.GetPosition() + this.finishedSeedDropOffset);
    MutantPlant component2 = pickupable.GetComponent<MutantPlant>();
    PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(component2.SubSpeciesID);
    component2.Analyze();
    SaveGame.Instance.ColonyAchievementTracker.LogAnalyzedSeed(component2.SpeciesID);
  }
}
