// Decompiled with JetBrains decompiler
// Type: MajorFossilDigSite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class MajorFossilDigSite : 
  GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>
{
  public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Idle;
  public MajorFossilDigSite.ReadyToBeWorked Workable;
  public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State WaitingForQuestCompletion;
  public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Completed;
  public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter MarkedForDig;
  public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsRevealed;
  public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsQuestCompleted;
  public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Signal CompleteStorySignal;
  public const string ANIM_COVERED_NAME = "covered";
  public const string ANIM_REVEALED_NAME = "reveal";

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.Idle;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.Idle.PlayAnim("covered").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOffLight)).Enter((StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback) (smi => MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, false))).ParamTransition<bool>((StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Parameter<bool>) this.IsQuestCompleted, this.Completed, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>((StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Parameter<bool>) this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>((StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Parameter<bool>) this.MarkedForDig, (GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State) this.Workable, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue);
    this.Workable.PlayAnim("covered").Enter((StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback) (smi => MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true))).DefaultState(this.Workable.NonOperational).ParamTransition<bool>((StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Parameter<bool>) this.MarkedForDig, this.Idle, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsFalse);
    this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational);
    this.Workable.Operational.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((Func<MajorFossilDigSite.Instance, global::Workable>) (smi => smi.GetWorkable()), this.WaitingForQuestCompletion);
    this.WaitingForQuestCompletion.OnSignal(this.CompleteStorySignal, this.Completed).Enter((StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback) (smi => MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true))).PlayAnim("reveal").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.Reveal)).ScheduleActionNextFrame("Refresh UI", new System.Action<MajorFossilDigSite.Instance>(MajorFossilDigSite.RefreshUI)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CheckForQuestCompletion)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.ProgressStoryTrait));
    this.Completed.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOnLight)).Enter((StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback) (smi => MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true))).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CompleteStory)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.UnlockFossilMine)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.MakeItDemolishable));
  }

  public static void MakeItDemolishable(MajorFossilDigSite.Instance smi)
  {
    smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
  }

  public static void ProgressStoryTrait(MajorFossilDigSite.Instance smi)
  {
    QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
    if (instance == null)
      return;
    Quest.ItemData data = new Quest.ItemData()
    {
      CriteriaId = smi.def.questCriteria,
      CurrentValue = 1f
    };
    instance.TrackProgress(data, out bool _, out bool _);
  }

  public static void TurnOnLight(MajorFossilDigSite.Instance smi) => smi.SetLightOnState(true);

  public static void TurnOffLight(MajorFossilDigSite.Instance smi) => smi.SetLightOnState(false);

  public static void CheckForQuestCompletion(MajorFossilDigSite.Instance smi)
  {
    QuestInstance quest = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
    if (quest == null || quest.CurrentState != Quest.State.Completed)
      return;
    smi.OnQuestCompleted(quest);
  }

  public static void SetEntombedStatusItemVisibility(MajorFossilDigSite.Instance smi, bool val)
  {
    smi.SetEntombStatusItemVisibility(val);
  }

  public static void UnlockFossilMine(MajorFossilDigSite.Instance smi) => smi.UnlockFossilMine();

  public static void DestroyUIExcavateButton(MajorFossilDigSite.Instance smi)
  {
    smi.DestroyExcavateButton();
  }

  public static void CompleteStory(MajorFossilDigSite.Instance smi)
  {
    if (smi.sm.IsQuestCompleted.Get(smi))
      return;
    smi.sm.IsQuestCompleted.Set(true, smi);
    smi.CompleteStoryTrait();
  }

  public static void Reveal(MajorFossilDigSite.Instance smi)
  {
    int num = !smi.sm.IsRevealed.Get(smi) ? 1 : 0;
    smi.sm.IsRevealed.Set(true, smi);
    if (num == 0)
      return;
    QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
    if (instance == null || instance.IsComplete)
      return;
    smi.ShowCompletionNotification();
  }

  public static void RevealMinorDigSites(MajorFossilDigSite.Instance smi)
  {
    smi.RevealMinorDigSites();
  }

  public static void RefreshUI(MajorFossilDigSite.Instance smi) => smi.RefreshUI();

  public static void StartWorkChore(MajorFossilDigSite.Instance smi) => smi.CreateWorkableChore();

  public static void CancelWorkChore(MajorFossilDigSite.Instance smi) => smi.CancelWorkChore();

  public class Def : StateMachine.BaseDef
  {
    public HashedString questCriteria;
  }

  public class ReadyToBeWorked : 
    GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State
  {
    public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Operational;
    public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State NonOperational;
  }

  public new class Instance : 
    GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.GameInstance,
    ICheckboxListGroupControl
  {
    [MyCmpGet]
    private Operational operational;
    [MyCmpGet]
    private MajorDigSiteWorkable excavateWorkable;
    [MyCmpGet]
    private FossilMine fossilMine;
    [MyCmpGet]
    private EntombVulnerable entombComponent;
    private Chore chore;
    private FossilHuntInitializer.Instance storyInitializer;
    private ExcavateButton excavateButton;

    public Instance(IStateMachineTarget master, MajorFossilDigSite.Def def)
      : base(master, def)
    {
      Components.MajorFossilDigSites.Add(this);
    }

    public override void StartSM()
    {
      this.entombComponent.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
      this.storyInitializer = this.gameObject.GetSMI<FossilHuntInitializer.Instance>();
      this.GetComponent<KPrefabID>();
      QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest).QuestProgressChanged += new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
      if (!this.sm.IsRevealed.Get(this))
        this.CreateExcavateButton();
      this.fossilMine.SetActiveState(this.sm.IsQuestCompleted.Get(this));
      if (this.sm.IsQuestCompleted.Get(this))
      {
        this.UnlockStandarBuildingButtons();
        this.ScheduleNextFrame((System.Action<object>) (obj => this.ChangeUIDescriptionToCompleted()), (object) null);
      }
      this.excavateWorkable.SetShouldShowSkillPerkStatusItem(this.sm.MarkedForDig.Get(this));
      base.StartSM();
      this.RefreshUI();
    }

    public void SetLightOnState(bool isOn)
    {
      FossilDigsiteLampLight component = this.gameObject.GetComponent<FossilDigsiteLampLight>();
      component.SetIndependentState(isOn);
      if (isOn)
        return;
      component.enabled = false;
    }

    public global::Workable GetWorkable() => (global::Workable) this.excavateWorkable;

    public void CreateWorkableChore()
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<MajorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, (IStateMachineTarget) this.excavateWorkable, only_when_operational: false);
    }

    public void CancelWorkChore()
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("MajorFossilDigsite.CancelChore");
      this.chore = (Chore) null;
    }

    public void SetEntombStatusItemVisibility(bool visible)
    {
      this.entombComponent.SetShowStatusItemOnEntombed(visible);
    }

    public void OnExcavateButtonPressed()
    {
      this.sm.MarkedForDig.Set(!this.sm.MarkedForDig.Get(this), this);
      this.excavateWorkable.SetShouldShowSkillPerkStatusItem(this.sm.MarkedForDig.Get(this));
    }

    public ExcavateButton CreateExcavateButton()
    {
      if ((UnityEngine.Object) this.excavateButton == (UnityEngine.Object) null)
      {
        this.excavateButton = this.gameObject.AddComponent<ExcavateButton>();
        this.excavateButton.OnButtonPressed += new System.Action(this.OnExcavateButtonPressed);
        this.excavateButton.isMarkedForDig = (Func<bool>) (() => this.sm.MarkedForDig.Get(this));
      }
      return this.excavateButton;
    }

    public void DestroyExcavateButton()
    {
      this.excavateWorkable.SetShouldShowSkillPerkStatusItem(false);
      if (!((UnityEngine.Object) this.excavateButton != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.excavateButton);
      this.excavateButton = (ExcavateButton) null;
    }

    public string Title => (string) CODEX.STORY_TRAITS.FOSSILHUNT.NAME;

    public string Description
    {
      get
      {
        return this.sm.IsRevealed.Get(this) ? (string) CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_REVEALED : (string) CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_BUILDINGMENU_COVERED;
      }
    }

    public bool SidescreenEnabled() => !this.sm.IsQuestCompleted.Get(this);

    public void RevealMinorDigSites()
    {
      if (this.storyInitializer == null)
        this.storyInitializer = this.gameObject.GetSMI<FossilHuntInitializer.Instance>();
      if (this.storyInitializer == null)
        return;
      this.storyInitializer.RevealMinorFossilDigSites();
    }

    private void OnQuestProgressChanged(
      QuestInstance quest,
      Quest.State previousState,
      float progressIncreased)
    {
      if (quest.CurrentState == Quest.State.Completed && this.sm.IsRevealed.Get(this))
        this.OnQuestCompleted(quest);
      this.RefreshUI();
    }

    public void OnQuestCompleted(QuestInstance quest)
    {
      this.sm.CompleteStorySignal.Trigger(this);
      quest.QuestProgressChanged -= new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
    }

    public void CompleteStoryTrait()
    {
      FossilHuntInitializer.Instance smi = this.gameObject.GetSMI<FossilHuntInitializer.Instance>();
      smi.sm.CompleteStory.Trigger(smi);
    }

    public void UnlockFossilMine()
    {
      this.fossilMine.SetActiveState(true);
      this.UnlockStandarBuildingButtons();
      this.ChangeUIDescriptionToCompleted();
    }

    private void ChangeUIDescriptionToCompleted()
    {
      BuildingComplete component = this.gameObject.GetComponent<BuildingComplete>();
      this.gameObject.GetComponent<KSelectable>().SetName((string) BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.NAME);
      component.SetDescriptionFlavour((string) BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.EFFECT);
      component.SetDescription((string) BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.DESC);
    }

    private void UnlockStandarBuildingButtons()
    {
      this.gameObject.AddOrGet<BuildingEnabledButton>();
    }

    public void RefreshUI() => this.gameObject.Trigger(1980521255);

    protected override void OnCleanUp()
    {
      QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
      if (instance != null)
        instance.QuestProgressChanged -= new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
      Components.MajorFossilDigSites.Remove(this);
      base.OnCleanUp();
    }

    public int CheckboxSideScreenSortOrder() => 20;

    public ICheckboxListGroupControl.ListGroup[] GetData()
    {
      return FossilHuntInitializer.GetFossilHuntQuestData();
    }

    public void ShowCompletionNotification()
    {
      this.gameObject.GetSMI<FossilHuntInitializer.Instance>()?.ShowObjectiveCompletedNotification();
    }
  }
}
