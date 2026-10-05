// Decompiled with JetBrains decompiler
// Type: MinorFossilDigSite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;

#nullable disable
public class MinorFossilDigSite : 
  GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>
{
  public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Idle;
  public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Completed;
  public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State WaitingForQuestCompletion;
  public MinorFossilDigSite.ReadyToBeWorked Workable;
  public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter MarkedForDig;
  public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsRevealed;
  public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsQuestCompleted;
  private const string UNEXCAVATED_ANIM_NAME = "object_dirty";
  private const string EXCAVATED_ANIM_NAME = "object";

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.Idle;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.Idle.Enter((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback) (smi => MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false))).Enter((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback) (smi => smi.SetDecorState(true))).PlayAnim("object_dirty").ParamTransition<bool>((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.Parameter<bool>) this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.Parameter<bool>) this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.Parameter<bool>) this.MarkedForDig, (GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State) this.Workable, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
    this.Workable.PlayAnim("object_dirty").Toggle("Activate Entombed Status Item If Required", (StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback) (smi => MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true)), (StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback) (smi => MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false))).DefaultState(this.Workable.NonOperational).ParamTransition<bool>((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.Parameter<bool>) this.MarkedForDig, this.Idle, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsFalse);
    this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational);
    this.Workable.Operational.Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((Func<MinorFossilDigSite.Instance, global::Workable>) (smi => smi.GetWorkable()), this.WaitingForQuestCompletion);
    this.WaitingForQuestCompletion.Enter((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback) (smi => smi.SetDecorState(false))).Enter((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback) (smi => MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true))).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.Reveal)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.ProgressStoryTrait)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable)).PlayAnim("object").ParamTransition<bool>((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.Parameter<bool>) this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
    this.Completed.Enter((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback) (smi => smi.SetDecorState(false))).Enter((StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback) (smi => MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true))).PlayAnim("object").Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable));
  }

  public static void MakeItDemolishable(MinorFossilDigSite.Instance smi)
  {
    smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
  }

  public static void DestroyUIExcavateButton(MinorFossilDigSite.Instance smi)
  {
    smi.DestroyExcavateButton();
  }

  public static void SetEntombedStatusItemVisibility(MinorFossilDigSite.Instance smi, bool val)
  {
    smi.SetEntombStatusItemVisibility(val);
  }

  public static void UnregisterFromComponents(MinorFossilDigSite.Instance smi)
  {
    Components.MinorFossilDigSites.Remove(smi);
  }

  public static void SelfDestroy(MinorFossilDigSite.Instance smi)
  {
    Util.KDestroyGameObject(smi.gameObject);
  }

  public static void StartWorkChore(MinorFossilDigSite.Instance smi) => smi.CreateWorkableChore();

  public static void CancelWorkChore(MinorFossilDigSite.Instance smi) => smi.CancelWorkChore();

  public static void Reveal(MinorFossilDigSite.Instance smi)
  {
    int num = !smi.sm.IsRevealed.Get(smi) ? 1 : 0;
    smi.sm.IsRevealed.Set(true, smi);
    if (num == 0)
      return;
    smi.ShowCompletionNotification();
    MinorFossilDigSite.DropLoot(smi);
  }

  public static void DropLoot(MinorFossilDigSite.Instance smi)
  {
    PrimaryElement component = smi.GetComponent<PrimaryElement>();
    int cell = Grid.PosToCell(smi.transform.GetPosition());
    Element element = ElementLoader.GetElement(component.Element.tag);
    if (element == null)
      return;
    float mass1 = component.Mass;
    for (int index = 0; (double) index < (double) component.Mass / 400.0; ++index)
    {
      float mass2 = mass1;
      if ((double) mass1 > 400.0)
      {
        mass2 = 400f;
        mass1 -= 400f;
      }
      int disease_count = (int) ((double) component.DiseaseCount * ((double) mass2 / (double) component.Mass));
      element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), mass2, component.Temperature, component.DiseaseIdx, disease_count);
    }
  }

  public static void ProgressStoryTrait(MinorFossilDigSite.Instance smi)
  {
    MinorFossilDigSite.ProgressQuest(smi);
  }

  public static QuestInstance ProgressQuest(MinorFossilDigSite.Instance smi)
  {
    QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
    if (instance == null)
      return (QuestInstance) null;
    Quest.ItemData data = new Quest.ItemData()
    {
      CriteriaId = smi.def.fossilQuestCriteriaID,
      CurrentValue = 1f
    };
    instance.TrackProgress(data, out bool _, out bool _);
    return instance;
  }

  public class Def : StateMachine.BaseDef
  {
    public HashedString fossilQuestCriteriaID;
  }

  public class ReadyToBeWorked : 
    GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State
  {
    public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Operational;
    public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State NonOperational;
  }

  public new class Instance : 
    GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.GameInstance,
    ICheckboxListGroupControl
  {
    [MyCmpGet]
    private MinorDigSiteWorkable workable;
    [MyCmpGet]
    private EntombVulnerable entombComponent;
    private ExcavateButton excavateButton;
    private Chore chore;
    private AttributeModifier negativeDecorModifier;

    public Instance(IStateMachineTarget master, MinorFossilDigSite.Def def)
      : base(master, def)
    {
      Components.MinorFossilDigSites.Add(this);
      this.negativeDecorModifier = new AttributeModifier(Db.Get().Attributes.Decor.Id, -1f, (string) CODEX.STORY_TRAITS.FOSSILHUNT.MISC.DECREASE_DECOR_ATTRIBUTE, true);
    }

    public void SetDecorState(bool isDusty)
    {
      if (isDusty)
        this.gameObject.GetComponent<DecorProvider>().decor.Add(this.negativeDecorModifier);
      else
        this.gameObject.GetComponent<DecorProvider>().decor.Remove(this.negativeDecorModifier);
    }

    public override void StartSM()
    {
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
      if (storyInstance != null)
        storyInstance.StoryStateChanged += new System.Action<StoryInstance.State>(this.OnStorytraitProgressChanged);
      if (!this.sm.IsRevealed.Get(this))
        this.CreateExcavateButton();
      QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest).QuestProgressChanged += new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
      this.workable.SetShouldShowSkillPerkStatusItem(this.sm.MarkedForDig.Get(this));
      base.StartSM();
      this.RefreshUI();
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
      this.sm.IsQuestCompleted.Set(true, this);
      quest.QuestProgressChanged -= new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
    }

    protected override void OnCleanUp()
    {
      MinorFossilDigSite.ProgressQuest(this.smi);
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
      if (storyInstance != null)
        storyInstance.StoryStateChanged -= new System.Action<StoryInstance.State>(this.OnStorytraitProgressChanged);
      QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
      if (instance != null)
        instance.QuestProgressChanged -= new System.Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged);
      Components.MinorFossilDigSites.Remove(this);
      base.OnCleanUp();
    }

    public void OnStorytraitProgressChanged(StoryInstance.State state)
    {
      if (state == StoryInstance.State.IN_PROGRESS)
        this.RefreshUI();
    }

    public void RefreshUI() => this.Trigger(1980521255);

    public global::Workable GetWorkable() => (global::Workable) this.workable;

    public void CreateWorkableChore()
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<MinorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, (IStateMachineTarget) this.workable, only_when_operational: false);
    }

    public void CancelWorkChore()
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("MinorFossilDigsite.CancelChore");
      this.chore = (Chore) null;
    }

    public void SetEntombStatusItemVisibility(bool visible)
    {
      this.entombComponent.SetShowStatusItemOnEntombed(visible);
    }

    public void ShowCompletionNotification()
    {
      this.gameObject.GetSMI<FossilHuntInitializer.Instance>()?.ShowObjectiveCompletedNotification();
    }

    public void OnExcavateButtonPressed()
    {
      this.sm.MarkedForDig.Set(!this.sm.MarkedForDig.Get(this), this);
      this.workable.SetShouldShowSkillPerkStatusItem(this.sm.MarkedForDig.Get(this));
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
      this.workable.SetShouldShowSkillPerkStatusItem(false);
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

    public ICheckboxListGroupControl.ListGroup[] GetData()
    {
      return FossilHuntInitializer.GetFossilHuntQuestData();
    }

    public int CheckboxSideScreenSortOrder() => 20;
  }
}
