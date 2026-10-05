// Decompiled with JetBrains decompiler
// Type: MorbRoverMakerStorytrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

#nullable disable
public class MorbRoverMakerStorytrait : 
  StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>
{
  public StateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, StateMachineController, MorbRoverMakerStorytrait.Def>.BoolParameter HasAnyBioBotBeenReleased;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.root;
  }

  public class Def : 
    StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitDef
  {
    public const string LORE_UNLOCK_PREFIX = "story_trait_morbrover_";
    public string MachineRevealedLoreId = "story_trait_morbrover_reveal";
    public string MachineRevealedLoreId2 = "story_trait_morbrover_reveal_lore";
    public string CompleteLoreId2 = "story_trait_morbrover_complete_lore";
    public string CompleteLoreId3 = "story_trait_morbrover_biobot";
    public System.Action NormalPopupOpenCodexButtonPressed;
    public StoryManager.PopupInfo EventMachineRevealedInfo;

    public override void Configure(GameObject prefab)
    {
      this.Story = Db.Get().Stories.MorbRoverMaker;
      this.CompletionData = new StoryCompleteData()
      {
        KeepSakeSpawnOffset = new CellOffset(0, 2),
        CameraTargetOffset = new CellOffset(0, 3)
      };
      this.InitalLoreId = "story_trait_morbrover_initial";
      this.EventIntroInfo = new StoryManager.PopupInfo()
      {
        Title = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.NAME,
        Description = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.DESCRIPTION,
        CloseButtonText = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.BUTTON,
        TextureName = "biobotdiscovered_kanim",
        DisplayImmediate = true,
        PopupType = EventInfoDataHelper.PopupType.BEGIN
      };
      StoryManager.PopupInfo popupInfo = new StoryManager.PopupInfo();
      popupInfo.Title = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.NAME;
      popupInfo.Description = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.DESCRIPTION;
      popupInfo.CloseButtonText = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_CLOSE;
      // ISSUE: explicit reference operation
      (^ref popupInfo).extraButtons = new StoryManager.ExtraButtonInfo[1]
      {
        new StoryManager.ExtraButtonInfo()
        {
          ButtonText = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_READLORE,
          OnButtonClick = (System.Action) (() =>
          {
            System.Action codexButtonPressed = this.NormalPopupOpenCodexButtonPressed;
            if (codexButtonPressed != null)
              codexButtonPressed();
            this.UnlockRevealEntries();
            string entryForLock = CodexCache.GetEntryForLock(this.MachineRevealedLoreId);
            if (entryForLock == null)
              DebugUtil.DevLogError("Missing codex entry for lock: " + this.MachineRevealedLoreId);
            else
              ManagementMenu.Instance.OpenCodexToEntry(entryForLock);
          })
        }
      };
      popupInfo.TextureName = "BioBotCleanedUp_kanim";
      popupInfo.PopupType = EventInfoDataHelper.PopupType.NORMAL;
      this.EventMachineRevealedInfo = popupInfo;
      this.CompleteLoreId = "story_trait_morbrover_complete";
      this.EventCompleteInfo = new StoryManager.PopupInfo()
      {
        Title = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.NAME,
        Description = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.DESCRIPTION,
        CloseButtonText = (string) CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.BUTTON,
        TextureName = "BioBotComplete_kanim",
        PopupType = EventInfoDataHelper.PopupType.COMPLETE
      };
    }

    public void UnlockRevealEntries()
    {
      Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId);
      Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId2);
    }
  }

  public new class Instance : 
    StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitInstance
  {
    private MorbRoverMaker.Instance machine;
    private StoryInstance storyInstance;

    public Instance(StateMachineController master, MorbRoverMakerStorytrait.Def def)
      : base(master, def)
    {
      def.NormalPopupOpenCodexButtonPressed += new System.Action(this.OnNormalPopupOpenCodexButtonPressed);
    }

    public override void StartSM()
    {
      base.StartSM();
      this.machine = this.gameObject.GetSMI<MorbRoverMaker.Instance>();
      this.storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MorbRoverMaker.HashId);
      if (this.storyInstance == null || this.machine == null)
        return;
      this.machine.OnUncovered += new System.Action(this.OnMachineUncovered);
      this.machine.OnRoverSpawned += new System.Action<GameObject>(this.OnRoverSpawned);
      if (this.machine.HasBeenRevealed && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE && this.storyInstance.CurrentState != StoryInstance.State.IN_PROGRESS)
        this.DisplayPopup(this.def.EventMachineRevealedInfo);
      if (!this.machine.HasBeenRevealed || !this.sm.HasAnyBioBotBeenReleased.Get(this) || this.storyInstance.CurrentState == StoryInstance.State.COMPLETE)
        return;
      this.CompleteEvent();
    }

    private void OnMachineUncovered()
    {
      if (this.storyInstance == null || this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
        return;
      this.DisplayPopup(this.def.EventMachineRevealedInfo);
    }

    protected override void ShowEventNormalUI()
    {
      base.ShowEventNormalUI();
      if (this.storyInstance == null || this.storyInstance.PendingType != EventInfoDataHelper.PopupType.NORMAL)
        return;
      EventInfoScreen.ShowPopup(this.storyInstance.EventInfo);
    }

    public override void OnPopupClosed()
    {
      base.OnPopupClosed();
      if (this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
      {
        Game.Instance.unlocks.Unlock(this.def.CompleteLoreId2);
        Game.Instance.unlocks.Unlock(this.def.CompleteLoreId3);
      }
      else
      {
        if (this.storyInstance == null || !this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
          return;
        this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
        this.def.UnlockRevealEntries();
      }
    }

    private void OnNormalPopupOpenCodexButtonPressed()
    {
      this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
    }

    private void OnRoverSpawned(GameObject rover)
    {
      this.smi.sm.HasAnyBioBotBeenReleased.Set(true, this.smi);
      if (this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
        return;
      this.CompleteEvent();
    }

    protected override void OnCleanUp()
    {
      if (this.machine != null)
      {
        this.machine.OnUncovered -= new System.Action(this.OnMachineUncovered);
        this.machine.OnRoverSpawned -= new System.Action<GameObject>(this.OnRoverSpawned);
      }
      base.OnCleanUp();
    }
  }
}
