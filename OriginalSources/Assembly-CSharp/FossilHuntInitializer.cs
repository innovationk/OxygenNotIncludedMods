// Decompiled with JetBrains decompiler
// Type: FossilHuntInitializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FossilHuntInitializer : 
  StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>
{
  private GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State Inactive;
  private FossilHuntInitializer.ActiveState Active;
  public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.BoolParameter storyCompleted;
  public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.BoolParameter wasStoryStarted;
  public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.Signal CompleteStory;
  public const string LINK_OVERRIDE_PREFIX = "MOVECAMERATO";

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.Inactive;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.Inactive.ParamTransition<bool>((StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.Parameter<bool>) this.storyCompleted, this.Active.StoryComplete, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue).ParamTransition<bool>((StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.Parameter<bool>) this.wasStoryStarted, this.Active.inProgress, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue);
    this.Active.inProgress.ParamTransition<bool>((StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.Parameter<bool>) this.storyCompleted, this.Active.StoryComplete, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue).OnSignal(this.CompleteStory, this.Active.StoryComplete);
    this.Active.StoryComplete.Enter(new StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State.Callback(FossilHuntInitializer.CompleteStoryTrait));
  }

  public static bool OnUI_Quest_ObjectiveRowClicked(string rowLinkID)
  {
    rowLinkID = rowLinkID.ToUpper();
    if (!rowLinkID.Contains("MOVECAMERATO"))
      return true;
    string str = rowLinkID.Replace("MOVECAMERATO", "");
    Tag tag;
    if (Components.MajorFossilDigSites.Count > 0)
    {
      tag = Components.MajorFossilDigSites[0].gameObject.PrefabID();
      if (CodexCache.FormatLinkID(tag.ToString()) == str)
      {
        GameUtil.FocusCamera(Components.MajorFossilDigSites[0].transform);
        return false;
      }
    }
    foreach (MinorFossilDigSite.Instance minorFossilDigSite in Components.MinorFossilDigSites)
    {
      tag = minorFossilDigSite.PrefabID();
      if (CodexCache.FormatLinkID(tag.ToString()) == str)
      {
        GameUtil.FocusCamera(minorFossilDigSite.transform.GetPosition());
        SelectTool.Instance.Select(minorFossilDigSite.gameObject.GetComponent<KSelectable>());
        return false;
      }
    }
    return false;
  }

  public static void CompleteStoryTrait(FossilHuntInitializer.Instance smi)
  {
    StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
    if (storyInstance == null)
      return;
    smi.sm.storyCompleted.Set(true, smi);
    if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
      return;
    smi.CompleteEvent();
  }

  public static string ResolveStrings_QuestObjectivesRowTooltips(string originalText, object obj)
  {
    return originalText + (string) CODEX.STORY_TRAITS.FOSSILHUNT.QUEST.LINKED_TOOLTIP;
  }

  public static string ResolveQuestTitle(string title, QuestInstance quest)
  {
    int digsitesRequired = FossilDigSiteConfig.DiscoveredDigsitesRequired;
    string str = $"{Mathf.RoundToInt(quest.CurrentProgress * (float) digsitesRequired).ToString()}/{digsitesRequired.ToString()}";
    return $"{title} - {str}";
  }

  public static ICheckboxListGroupControl.ListGroup[] GetFossilHuntQuestData()
  {
    QuestInstance quest = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
    ICheckboxListGroupControl.CheckboxItem[] checkBoxData = quest.GetCheckBoxData();
    for (int index = 0; index < checkBoxData.Length; ++index)
    {
      checkBoxData[index].overrideLinkActions = new Func<string, bool>(FossilHuntInitializer.OnUI_Quest_ObjectiveRowClicked);
      checkBoxData[index].resolveTooltipCallback = new Func<string, object, string>(FossilHuntInitializer.ResolveStrings_QuestObjectivesRowTooltips);
    }
    if (quest == null)
      return new ICheckboxListGroupControl.ListGroup[0];
    return new ICheckboxListGroupControl.ListGroup[1]
    {
      new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.FossilHuntQuest.Title, checkBoxData, (Func<string, string>) (title => FossilHuntInitializer.ResolveQuestTitle(title, quest)))
    };
  }

  public class Def : 
    StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>.TraitDef
  {
    public const string LORE_UNLOCK_PREFIX = "story_trait_fossilhunt_";
    public bool IsMainDigsite;

    public override void Configure(GameObject prefab)
    {
      this.Story = Db.Get().Stories.FossilHunt;
      this.CompletionData = new StoryCompleteData()
      {
        KeepSakeSpawnOffset = new CellOffset(1, 2),
        CameraTargetOffset = new CellOffset(0, 3)
      };
      this.InitalLoreId = "story_trait_fossilhunt_initial";
      this.EventIntroInfo = new StoryManager.PopupInfo()
      {
        Title = (string) CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.NAME,
        Description = (string) CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.DESCRIPTION,
        CloseButtonText = (string) CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.BUTTON,
        TextureName = "fossildigdiscovered_kanim",
        DisplayImmediate = true,
        PopupType = EventInfoDataHelper.PopupType.BEGIN
      };
      this.CompleteLoreId = "story_trait_fossilhunt_complete";
      this.EventCompleteInfo = new StoryManager.PopupInfo()
      {
        Title = (string) CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.NAME,
        Description = (string) CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.DESCRIPTION,
        CloseButtonText = (string) CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.BUTTON,
        TextureName = "fossildigmining_kanim",
        PopupType = EventInfoDataHelper.PopupType.COMPLETE
      };
    }
  }

  public class ActiveState : 
    GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State
  {
    public GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State inProgress;
    public GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State StoryComplete;
  }

  public new class Instance(StateMachineController master, FossilHuntInitializer.Def def) : 
    StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>.TraitInstance(master, def)
  {
    public string Title => (string) CODEX.STORY_TRAITS.FOSSILHUNT.NAME;

    public string Description => (string) CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION;

    public override void StartSM()
    {
      base.StartSM();
      this.gameObject.GetSMI<MajorFossilDigSite>();
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
      if (storyInstance == null)
        return;
      if (this.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
      {
        switch (storyInstance.CurrentState)
        {
          case StoryInstance.State.IN_PROGRESS:
            this.sm.wasStoryStarted.Set(true, this);
            break;
          case StoryInstance.State.COMPLETE:
            this.GoTo((StateMachine.BaseState) this.sm.Active.StoryComplete);
            break;
        }
      }
      storyInstance.StoryStateChanged += new System.Action<StoryInstance.State>(this.OnStoryStateChanged);
    }

    protected override void OnCleanUp()
    {
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
      if (storyInstance != null)
        storyInstance.StoryStateChanged -= new System.Action<StoryInstance.State>(this.OnStoryStateChanged);
      base.OnCleanUp();
    }

    private void OnStoryStateChanged(StoryInstance.State state)
    {
      if (state != StoryInstance.State.IN_PROGRESS)
        return;
      this.sm.wasStoryStarted.Set(true, this);
    }

    protected override void OnObjectSelect(object clicked)
    {
      if (!StoryManager.Instance.HasDisplayedPopup(this.def.Story, EventInfoDataHelper.PopupType.BEGIN))
      {
        this.RevealMajorFossilDigSites();
        this.RevealMinorFossilDigSites();
      }
      if (!Boxed<bool>.Unbox(clicked))
        return;
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(this.def.Story.HashId);
      if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE && (storyInstance.PendingType != EventInfoDataHelper.PopupType.COMPLETE || this.def.IsMainDigsite))
      {
        this.OnNotificationClicked();
      }
      else
      {
        if (StoryManager.Instance.HasDisplayedPopup(this.def.Story, EventInfoDataHelper.PopupType.BEGIN))
          return;
        this.DisplayPopup(this.def.EventIntroInfo);
      }
    }

    public override void OnPopupClosed()
    {
      if (!StoryManager.Instance.HasDisplayedPopup(this.def.Story, EventInfoDataHelper.PopupType.COMPLETE))
        this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
      base.OnPopupClosed();
    }

    protected override void OnBuildingActivated(object activated)
    {
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
      if (storyInstance == null || this.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
        return;
      this.RevealMinorFossilDigSites();
      this.RevealMajorFossilDigSites();
      base.OnBuildingActivated(activated);
    }

    public void RevealMajorFossilDigSites() => this.RevealAll(8, (Tag) "FossilDig");

    public void RevealMinorFossilDigSites()
    {
      this.RevealAll(3, (Tag) "FossilResin", (Tag) "FossilIce", (Tag) "FossilRock");
    }

    private void RevealAll(int radius, params Tag[] tags)
    {
      foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(false, tags))
      {
        int x;
        int y;
        Grid.CellToXY(spawnable.cell, out x, out y);
        GridVisibility.Reveal(x, y, radius, (float) radius);
      }
    }

    public override void OnCompleteStorySequence()
    {
      if (!this.def.IsMainDigsite)
        return;
      base.OnCompleteStorySequence();
    }

    public void ShowLoreUnlockedPopup(int popupID)
    {
      InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader((string) CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.NAME).AddDefaultOK();
      int num = CodexCache.GetEntryForLock(FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(popupID)) != null ? 1 : 0;
      Option<string> contentForFossil = (Option<string>) FossilDigSiteConfig.GetBodyContentForFossil(popupID);
      if (num != 0 && contentForFossil.HasValue)
        infoDialogScreen.AddPlainText(contentForFossil.Value).AddOption((string) CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITFOSSILHUNT"));
      else
        infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
    }

    public void ShowObjectiveCompletedNotification()
    {
      QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
      if (instance == null)
        return;
      int objectivesCompleted = Mathf.RoundToInt(instance.CurrentProgress * (float) instance.CriteriaCount);
      if (objectivesCompleted == 0)
      {
        this.ShowFirstFossilExcavatedNotification();
      }
      else
      {
        Game.Instance.unlocks.Unlock(FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(objectivesCompleted), false);
        ShowNotificationAndWaitForClick().Then((System.Action) (() => this.ShowLoreUnlockedPopup(objectivesCompleted)));
      }

      Promise ShowNotificationAndWaitForClick()
      {
        return new Promise((System.Action<System.Action>) (resolve =>
        {
          Notification notification = new Notification((string) CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_NOTIFICATION.NAME, NotificationType.Event, (Func<List<Notification>, object, string>) ((notifications, obj) => (string) CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_NOTIFICATION.TOOLTIP), expires: false, custom_click_callback: (Notification.ClickCallback) (obj => resolve()), clear_on_click: true);
          this.gameObject.AddOrGet<Notifier>().Add(notification);
        }));
      }
    }

    public void ShowFirstFossilExcavatedNotification()
    {
      ShowNotificationAndWaitForClick().Then((System.Action) (() => this.ShowQuestUnlockedPopup()));

      Promise ShowNotificationAndWaitForClick()
      {
        return new Promise((System.Action<System.Action>) (resolve =>
        {
          Notification notification = new Notification((string) CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.NAME, NotificationType.Event, (Func<List<Notification>, object, string>) ((notifications, obj) => (string) CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.TOOLTIP), expires: false, custom_click_callback: (Notification.ClickCallback) (obj => resolve()), clear_on_click: true);
          this.gameObject.AddOrGet<Notifier>().Add(notification);
        }));
      }
    }

    public void ShowQuestUnlockedPopup()
    {
      LoreBearer.ShowPopupDialog().SetHeader((string) CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.NAME).AddDefaultOK().AddPlainText(((Option<string>) CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.DESCRIPTION.text).Value).AddOption((string) CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.CHECK_BUTTON, (System.Action<InfoDialogScreen>) (dialog =>
      {
        dialog.Deactivate();
        GameUtil.FocusCamera(this.transform);
      }));
    }
  }
}
