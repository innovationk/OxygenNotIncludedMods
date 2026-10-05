// Decompiled with JetBrains decompiler
// Type: ArtifactAnalysisStationWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using TUNING;
using UnityEngine;

#nullable disable
public class ArtifactAnalysisStationWorkable : Workable
{
  [MyCmpAdd]
  public Notifier notifier;
  [MyCmpReq]
  public Storage storage;
  [SerializeField]
  public Vector3 finishedArtifactDropOffset;
  private Notification notification;
  public ArtifactAnalysisStation.StatesInstance statesInstance;
  private KBatchedAnimController animController;
  [Serialize]
  private float nextYeildRoll = -1f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyArtifact.Id;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingArtifact;
    this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_artifact_analysis_kanim")
    };
    this.SetWorkTime(150f);
    this.showProgressBar = true;
    this.lightEfficiencyBonus = true;
    Components.ArtifactAnalysisStations.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.animController = this.GetComponent<KBatchedAnimController>();
    this.animController.SetSymbolVisiblity((KAnimHashedString) "snapTo_artifact", false);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.ArtifactAnalysisStations.Remove(this);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.InitialDisplayStoredArtifact();
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    this.PositionArtifact();
    return base.OnWorkTick(worker, dt);
  }

  private void InitialDisplayStoredArtifact()
  {
    GameObject data = this.GetComponent<Storage>().GetItems()[0];
    KBatchedAnimController component = data.GetComponent<KBatchedAnimController>();
    if ((Object) component != (Object) null)
      component.GetBatchInstanceData().ClearOverrideTransformMatrix();
    data.transform.SetPosition(new Vector3(this.transform.position.x, this.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)));
    data.SetActive(true);
    component.enabled = false;
    component.enabled = true;
    this.PositionArtifact();
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, (object) data);
  }

  private void ReleaseStoredArtifact()
  {
    Storage component1 = this.GetComponent<Storage>();
    GameObject gameObject = component1.GetItems()[0];
    KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
    gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Ore)));
    component2.enabled = false;
    component2.enabled = true;
    component1.Drop(gameObject, true);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, (bool) (Object) gameObject);
  }

  private void PositionArtifact()
  {
    this.GetComponent<Storage>().GetItems()[0].transform.SetPosition((Vector3) this.animController.GetSymbolTransform((HashedString) "snapTo_artifact", out bool _).GetColumn(3) with
    {
      z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)
    });
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    this.ConsumeCharm();
    this.ReleaseStoredArtifact();
  }

  private void ConsumeCharm()
  {
    GameObject first = this.storage.FindFirst(GameTags.CharmedArtifact);
    DebugUtil.DevAssertArgs(((Object) first != (Object) null ? 1 : 0) != 0, (object) "ArtifactAnalysisStation finished studying a charmed artifact but there is not one in its storage");
    if ((Object) first != (Object) null)
    {
      this.YieldPayload(first.GetComponent<SpaceArtifact>());
      first.GetComponent<SpaceArtifact>().RemoveCharm();
    }
    if (!ArtifactSelector.Instance.RecordArtifactAnalyzed(first.GetComponent<KPrefabID>().PrefabID().ToString()))
      return;
    if (first.HasTag(GameTags.TerrestrialArtifact))
      ArtifactSelector.Instance.IncrementAnalyzedTerrestrialArtifacts();
    else
      ArtifactSelector.Instance.IncrementAnalyzedSpaceArtifacts();
  }

  private void YieldPayload(SpaceArtifact artifact)
  {
    if ((double) this.nextYeildRoll == -1.0)
      this.nextYeildRoll = Random.Range(0.0f, 1f);
    if ((double) this.nextYeildRoll <= (double) artifact.GetArtifactTier().payloadDropChance)
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) "GeneShufflerRecharge"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore).SetActive(true);
    int num = Mathf.FloorToInt(artifact.GetArtifactTier().payloadDropChance * 20f);
    for (int index = 0; index < num; ++index)
      GameUtil.KInstantiate(Assets.GetPrefab((Tag) "OrbitalResearchDatabank"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore).SetActive(true);
    this.nextYeildRoll = Random.Range(0.0f, 1f);
  }
}
