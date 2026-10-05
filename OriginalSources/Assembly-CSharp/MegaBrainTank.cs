// Decompiled with JetBrains decompiler
// Type: MegaBrainTank
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MegaBrainTank : StateMachineComponent<MegaBrainTank.StatesInstance>
{
  [Serialize]
  private bool introDisplayed;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    StoryManager.Instance.ForceCreateStory(Db.Get().Stories.MegaBrainTank, this.gameObject.GetMyWorldId());
    this.smi.StartSM();
    this.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
    this.GetComponent<Activatable>().SetWorkTime(5f);
    this.smi.JournalDelivery.refillMass = 25f;
    this.smi.JournalDelivery.FillToCapacity = true;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.Unsubscribe(-1503271301);
  }

  private void OnBuildingSelect(object obj)
  {
    if (!((Boxed<bool>) obj).value)
      return;
    if (!this.introDisplayed)
    {
      this.introDisplayed = true;
      EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData((string) CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.NAME, (string) CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.DESCRIPTION, (string) CODEX.STORY_TRAITS.CLOSE_BUTTON, "braintankdiscovered_kanim", EventInfoDataHelper.PopupType.BEGIN, callback: new System.Action(this.DoInitialUnlock)));
    }
    this.smi.ShowEventCompleteUI();
  }

  private void DoInitialUnlock()
  {
    Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_initial");
  }

  public class States : 
    GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank>
  {
    public MegaBrainTank.States.CommonState common;
    public StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.Signal storyTraitCompleted;
    public Effect StatBonus;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.ParamsOnly;
      default_state = (StateMachine.BaseState) this.root;
      this.root.Enter((StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State.Callback) (smi =>
      {
        if (StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.MegaBrainTank))
        {
          if (smi.IsHungry)
            smi.GoTo((StateMachine.BaseState) this.common.idle);
          else
            smi.GoTo((StateMachine.BaseState) this.common.active);
        }
        else
          smi.GoTo((StateMachine.BaseState) this.common.dormant);
      }));
      this.common.Update((Action<MegaBrainTank.StatesInstance, float>) ((smi, dt) =>
      {
        smi.IncrementMeter(dt);
        if (smi.UnitsFromLastStore != (short) 0)
          smi.ShelveJournals(dt);
        bool flag = smi.ElementConverter.HasEnoughMass(GameTags.Oxygen, true);
        smi.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainNotEnoughOxygen, !flag);
      }), UpdateRate.SIM_33ms, false);
      this.common.dormant.Enter((StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State.Callback) (smi =>
      {
        smi.SetBonusActive(false);
        smi.ElementConverter.SetAllConsumedActive(false);
        smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, false);
        smi.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis);
        smi.master.GetComponent<Light2D>().enabled = false;
      })).Exit((StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State.Callback) (smi =>
      {
        smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, true);
        smi.ElementConverter.SetConsumedElementActive(GameTags.Oxygen, true);
        RequireInputs component = smi.GetComponent<RequireInputs>();
        component.requireConduitHasMass = true;
        component.visualizeRequirements = RequireInputs.Requirements.All;
      })).Update((Action<MegaBrainTank.StatesInstance, float>) ((smi, dt) => smi.ActivateBrains(dt)), UpdateRate.SIM_33ms).OnSignal(this.storyTraitCompleted, this.common.active);
      this.common.idle.Enter((StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State.Callback) (smi => smi.CleanTank(false))).UpdateTransition(this.common.active, (Func<MegaBrainTank.StatesInstance, float, bool>) ((smi, _) => !smi.IsHungry && smi.gameObject.GetComponent<Operational>().enabled), UpdateRate.SIM_1000ms);
      this.common.active.Enter((StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State.Callback) (smi => smi.CleanTank(true))).Update((Action<MegaBrainTank.StatesInstance, float>) ((smi, dt) => smi.Digest()), UpdateRate.SIM_33ms).UpdateTransition(this.common.idle, (Func<MegaBrainTank.StatesInstance, float, bool>) ((smi, _) => smi.IsHungry || !smi.gameObject.GetComponent<Operational>().enabled), UpdateRate.SIM_1000ms);
      this.StatBonus = new Effect("MegaBrainTankBonus", (string) DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, (string) DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.TOOLTIP, 0.0f, true, true, false);
      object[,] statBonuses = MegaBrainTankConfig.STAT_BONUSES;
      int length = statBonuses.GetLength(0);
      for (int index = 0; index < length; ++index)
        this.StatBonus.Add(new AttributeModifier(statBonuses[index, 0] as string, ModifierSet.ConvertValue(((float?) statBonuses[index, 1]).Value, ((Units?) statBonuses[index, 2]).Value), (string) DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME));
    }

    public class CommonState : 
      GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State
    {
      public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State dormant;
      public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State idle;
      public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State active;
    }
  }

  public class StatesInstance : 
    GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.GameInstance
  {
    private static List<Effects> minionEffects;
    public short UnitsFromLastStore;
    private float meterPositionValue;
    private float meterTarget;
    private float journalActivationTimer;
    private float lastRemainingTime;
    private byte activatedJournals;
    private bool currentlyActivating;
    private short nextActiveBrain = 1;
    private string brainHum;
    private KBatchedAnimController[] controllers;
    private KAnimLink fxLink;
    private MeterController meter;
    private EventInfoData eventInfo;
    private Notification eventComplete;
    private Notifier notifier;

    public KBatchedAnimController BrainController => this.controllers[0];

    public KBatchedAnimController ShelfController => this.controllers[1];

    public Storage BrainStorage { get; private set; }

    public KSelectable Selectable { get; private set; }

    public Operational Operational { get; private set; }

    public ElementConverter ElementConverter { get; private set; }

    public ManualDeliveryKG JournalDelivery { get; private set; }

    public LoopingSounds BrainSounds { get; private set; }

    public bool IsHungry => !this.ElementConverter.HasEnoughMassToStartConverting(true);

    public int JournalsStored => (int) this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);

    private float DesiredMeterPosition => (float) this.JournalsStored / 25f;

    public float DigestionTimeRemaining => (float) this.JournalsStored * 60f;

    public HashedString CurrentActivationAnim
    {
      get => MegaBrainTankConfig.ACTIVATION_ANIMS[(int) this.nextActiveBrain - 1];
    }

    private HashedString currentActivationLoop
    {
      get
      {
        int index = (int) this.nextActiveBrain - 1 + 5;
        return MegaBrainTankConfig.ACTIVATION_ANIMS[index];
      }
    }

    private float MeterPosition
    {
      get => this.meterPositionValue;
      set
      {
        this.meterPositionValue = value;
        this.meter.SetPositionPercent(value);
      }
    }

    public StatesInstance(MegaBrainTank master)
      : base(master)
    {
      this.BrainSounds = this.GetComponent<LoopingSounds>();
      this.BrainStorage = this.GetComponent<Storage>();
      this.ElementConverter = this.GetComponent<ElementConverter>();
      this.JournalDelivery = this.GetComponent<ManualDeliveryKG>();
      this.Operational = this.GetComponent<Operational>();
      this.Selectable = this.GetComponent<KSelectable>();
      this.notifier = this.GetComponent<Notifier>();
      this.controllers = this.gameObject.GetComponentsInChildren<KBatchedAnimController>();
      this.meter = new MeterController((KAnimControllerBase) this.BrainController, "meter_oxygen_target", nameof (meter), Meter.Offset.Infront, Grid.SceneLayer.NoLayer, MegaBrainTankConfig.METER_SYMBOLS);
      this.fxLink = new KAnimLink((KAnimControllerBase) this.BrainController, (KAnimControllerBase) this.ShelfController);
    }

    public override void StartSM()
    {
      this.InitializeEffectsList();
      base.StartSM();
      this.BrainController.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.OnAnimComplete);
      this.ShelfController.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.OnAnimComplete);
      Storage brainStorage = this.BrainStorage;
      brainStorage.OnWorkableEventCB = brainStorage.OnWorkableEventCB + new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged);
      this.brainHum = GlobalAssets.GetSound("MegaBrainTank_brain_wave_LP");
      StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.MegaBrainTank);
      this.MeterPosition = this.DesiredMeterPosition;
      if (this.GetCurrentState() == this.sm.common.dormant)
      {
        short num = (short) (5.0 * (double) this.MeterPosition);
        if (num <= (short) 0)
          return;
        this.nextActiveBrain = num;
        this.BrainSounds.StartSound(this.brainHum);
        this.BrainSounds.SetParameter(this.brainHum, (HashedString) "BrainTankProgress", (float) num);
        this.CompleteBrainActivation();
      }
      else
      {
        StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.MegaBrainTank);
        this.nextActiveBrain = (short) 5;
        this.CompleteBrainActivation();
      }
    }

    public override void StopSM(string reason)
    {
      this.BrainController.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.OnAnimComplete);
      this.ShelfController.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.OnAnimComplete);
      Storage brainStorage = this.BrainStorage;
      brainStorage.OnWorkableEventCB = brainStorage.OnWorkableEventCB - new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged);
      base.StopSM(reason);
    }

    private void InitializeEffectsList()
    {
      Components.Cmps<MinionIdentity> minionIdentities = Components.LiveMinionIdentities;
      minionIdentities.OnAdd += new Action<MinionIdentity>(this.OnLiveMinionIdAdded);
      minionIdentities.OnRemove += new Action<MinionIdentity>(this.OnLiveMinionIdRemoved);
      MegaBrainTank.StatesInstance.minionEffects = new List<Effects>(minionIdentities.Count > 32 /*0x20*/ ? minionIdentities.Count : 32 /*0x20*/);
      for (int idx = 0; idx < minionIdentities.Count; ++idx)
        this.OnLiveMinionIdAdded(minionIdentities[idx]);
    }

    private void OnLiveMinionIdAdded(MinionIdentity id)
    {
      Effects component = id.GetComponent<Effects>();
      MegaBrainTank.StatesInstance.minionEffects.Add(component);
      if (this.GetCurrentState() != this.sm.common.active)
        return;
      component.Add(this.sm.StatBonus, false);
    }

    private void OnLiveMinionIdRemoved(MinionIdentity id)
    {
      Effects component = id.GetComponent<Effects>();
      MegaBrainTank.StatesInstance.minionEffects.Remove(component);
    }

    public void SetBonusActive(bool active)
    {
      for (int index = 0; index < MegaBrainTank.StatesInstance.minionEffects.Count; ++index)
      {
        if (active)
          MegaBrainTank.StatesInstance.minionEffects[index].Add(this.sm.StatBonus, false);
        else
          MegaBrainTank.StatesInstance.minionEffects[index].Remove(this.sm.StatBonus);
      }
    }

    private void OnAnimComplete(HashedString anim)
    {
      if (anim == MegaBrainTankConfig.KACHUNK)
      {
        this.StoreJournals();
      }
      else
      {
        if (!(anim == this.smi.CurrentActivationAnim) && !(anim == MegaBrainTankConfig.ACTIVATE_ALL) || this.GetCurrentState() == this.sm.common.idle)
          return;
        this.CompleteBrainActivation();
      }
    }

    private void OnJournalDeliveryStateChanged(Workable w, Workable.WorkableEvent state)
    {
      switch (state)
      {
        case Workable.WorkableEvent.WorkStarted:
          FetchAreaChore.StatesInstance smi = w.worker.GetSMI<FetchAreaChore.StatesInstance>();
          if (smi.IsNullOrStopped())
            break;
          GameObject gameObject = smi.sm.deliveryObject.Get(smi);
          if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
            break;
          Pickupable component = gameObject.GetComponent<Pickupable>();
          this.UnitsFromLastStore = (short) component.PrimaryElement.Units;
          this.BrainStorage.SetWorkTime(Mathf.Clamp01(component.PrimaryElement.Units / 5f) * this.BrainStorage.storageWorkTime);
          break;
        case Workable.WorkableEvent.WorkStopped:
          break;
        default:
          this.ShelfController.Play(MegaBrainTankConfig.KACHUNK);
          break;
      }
    }

    public void ShelveJournals(float dt)
    {
      float num1 = this.lastRemainingTime - this.BrainStorage.WorkTimeRemaining;
      if ((double) num1 <= 0.0)
        num1 = this.BrainStorage.storageWorkTime - this.BrainStorage.WorkTimeRemaining;
      this.lastRemainingTime = this.BrainStorage.WorkTimeRemaining;
      if ((double) this.BrainStorage.storageWorkTime / 5.0 - (double) this.journalActivationTimer > 1.0 / 1000.0)
      {
        this.journalActivationTimer += num1;
      }
      else
      {
        int index1 = -1;
        this.journalActivationTimer = 0.0f;
        for (int index2 = 0; index2 < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; ++index2)
        {
          byte num2 = (byte) (1 << index2);
          int num3 = ((int) this.activatedJournals & (int) num2) == 0 ? 1 : 0;
          if (num3 != 0 && index1 == -1)
            index1 = index2;
          if ((num3 & ((double) UnityEngine.Random.Range(0.0f, 1f) >= 0.5 ? 1 : 0)) != 0)
          {
            index1 = -1;
            this.activatedJournals |= num2;
            this.ShelfController.SetSymbolVisiblity((KAnimHashedString) MegaBrainTankConfig.JOURNAL_SYMBOLS[index2], true);
            break;
          }
        }
        if (index1 != -1)
          this.ShelfController.SetSymbolVisiblity((KAnimHashedString) MegaBrainTankConfig.JOURNAL_SYMBOLS[index1], true);
        --this.UnitsFromLastStore;
      }
    }

    public void StoreJournals()
    {
      this.lastRemainingTime = 0.0f;
      this.activatedJournals = (byte) 0;
      for (int index = 0; index < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; ++index)
        this.ShelfController.SetSymbolVisiblity((KAnimHashedString) MegaBrainTankConfig.JOURNAL_SYMBOLS[index], false);
      this.ShelfController.PlayMode = KAnim.PlayMode.Paused;
      this.ShelfController.SetPositionPercent(0.0f);
      this.meterTarget = this.DesiredMeterPosition;
    }

    public void ActivateBrains(float dt)
    {
      if (this.currentlyActivating)
        return;
      this.currentlyActivating = (double) this.nextActiveBrain / 5.0 - (double) this.MeterPosition <= 1.0 / 1000.0;
      if (!this.currentlyActivating)
        return;
      this.BrainController.QueueAndSyncTransition(this.CurrentActivationAnim);
      if (this.nextActiveBrain <= (short) 0)
        return;
      this.BrainSounds.StartSound(this.brainHum);
      this.BrainSounds.SetParameter(this.brainHum, (HashedString) "BrainTankProgress", (float) this.nextActiveBrain);
    }

    public void CompleteBrainActivation()
    {
      this.BrainController.Play(this.currentActivationLoop, KAnim.PlayMode.Loop);
      ++this.nextActiveBrain;
      this.currentlyActivating = false;
      if (this.nextActiveBrain <= (short) 5)
        return;
      this.CompleteEvent();
    }

    public void Digest()
    {
      if ((double) this.meterTarget > (double) this.MeterPosition)
        return;
      this.meterTarget = 0.0f;
      this.MeterPosition = this.DesiredMeterPosition;
    }

    public void CleanTank(bool active)
    {
      this.SetBonusActive(active);
      this.GetComponent<Light2D>().enabled = active;
      this.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, active, (object) this);
      this.ElementConverter.SetAllConsumedActive(active);
      this.BrainController.ClearQueue();
      if (active)
      {
        this.nextActiveBrain = (short) 5;
        this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.ACTIVATE_ALL);
        this.BrainSounds.StartSound(this.brainHum);
        this.BrainSounds.SetParameter(this.brainHum, (HashedString) "BrainTankProgress", (float) this.nextActiveBrain);
      }
      else
      {
        if ((double) this.BrainStorage.GetMassAvailable(DreamJournalConfig.ID) > 0.0 && !this.ElementConverter.HasEnoughMassToStartConverting(true))
        {
          this.BrainStorage.ConsumeAllIgnoringDisease(DreamJournalConfig.ID);
          this.MeterPosition = 0.0f;
        }
        this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.DEACTIVATE_ALL);
        this.BrainSounds.StopSound(this.brainHum);
      }
    }

    public bool IncrementMeter(float dt)
    {
      if ((double) this.meterTarget <= (double) this.MeterPosition)
        return false;
      this.MeterPosition = Mathf.Min(this.MeterPosition + 0.04f * dt, this.meterTarget);
      return (double) this.meterTarget > (double) this.MeterPosition;
    }

    public void CompleteEvent()
    {
      this.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankActivationProgress);
      this.Selectable.AddStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankComplete, (object) this.smi);
      StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
      if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
        return;
      this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData((string) CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.NAME, (string) CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.DESCRIPTION, (string) CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.BUTTON, "braintankcomplete_kanim", EventInfoDataHelper.PopupType.COMPLETE);
      this.smi.Selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, (object) this.smi);
      this.eventComplete = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.ShowEventCompleteUI));
      this.notifier.Add(this.eventComplete);
    }

    public void ShowEventCompleteUI(object _ = null)
    {
      if (this.eventComplete == null)
        return;
      this.smi.Selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
      this.notifier.Remove(this.eventComplete);
      this.eventComplete = (Notification) null;
      Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_competed");
      Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this.master), new CellOffset(0, 3)), Grid.SceneLayer.Ore);
      StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, (MonoBehaviour) this.master, new FocusTargetSequence.Data()
      {
        WorldId = this.master.GetMyWorldId(),
        OrthographicSize = 6f,
        TargetSize = 6f,
        Target = posCcc,
        PopupData = this.eventInfo,
        CompleteCB = new System.Action(this.OnCompleteStorySequence),
        CanCompleteCB = (Func<bool>) null
      });
    }

    private void OnCompleteStorySequence()
    {
      Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this.master), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
      StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, posCcc);
      this.eventInfo = (EventInfoData) null;
      this.sm.storyTraitCompleted.Trigger(this);
    }
  }
}
