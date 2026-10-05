// Decompiled with JetBrains decompiler
// Type: HijackedHeadquarters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HijackedHeadquarters : 
  GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>
{
  public StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.IntParameter interceptCharges;
  public StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.BoolParameter passcodeUnlocked;
  public StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.BoolParameter hasBeenCompleted;
  public const int MAX_INTERCEPT_CHARGES = 3;
  public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State inoperational;
  public HijackedHeadquarters.OperationalStates operational;

  public static bool IsReadyToPrint(HijackedHeadquarters.Instance smi, int charges) => charges >= 3;

  public static bool IsOperational(HijackedHeadquarters.Instance smi)
  {
    return smi.GetComponent<Operational>().IsOperational;
  }

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inoperational;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.Enter((StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State.Callback) (smi => smi.UpdateMeter())).EventHandler(GameHashes.BuildingActivated, (GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.GameEvent.Callback) ((smi, activated) =>
    {
      if (!((Boxed<bool>) activated).value)
        return;
      StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.HijackedHeadquarters);
    }));
    this.inoperational.PlayAnim("inactive").EventTransition(GameHashes.OperationalChanged, this.operational.passcode.idle_locked, (StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.operational.DefaultState(this.operational.passcode.idle_locked).ParamTransition<int>((StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.Parameter<int>) this.interceptCharges, this.operational.readyToPrint.pre, new StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.Parameter<int>.Callback(HijackedHeadquarters.IsReadyToPrint)).EventTransition(GameHashes.OperationalChanged, this.inoperational, (StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).Update((System.Action<HijackedHeadquarters.Instance, float>) ((smi, dt) => smi.UpdateMeter()));
    this.operational.passcode.idle_locked.ParamTransition<bool>((StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.Parameter<bool>) this.passcodeUnlocked, this.operational.passcode.unlocking, GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.IsTrue).PlayAnim("idle_locked", KAnim.PlayMode.Once);
    this.operational.passcode.unlocking.PlayAnim("unlocking", KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.passcode.idle_unlocked);
    this.operational.passcode.idle_unlocked.PlayAnim("idle_unlocked", KAnim.PlayMode.Loop).Enter((StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State.Callback) (smi => smi.AddLore())).Enter((StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State.Callback) (smi => smi.ChangeUIDescriptionToCompleted())).Update((System.Action<HijackedHeadquarters.Instance, float>) ((smi, dt) =>
    {
      if (!Immigration.Instance.ImmigrantsAvailable)
        return;
      smi.GoTo((StateMachine.BaseState) this.operational.interceptPre);
    }));
    this.operational.interceptPre.PlayAnim("intercept_pre").OnAnimQueueComplete(this.operational.interceptLoop);
    this.operational.interceptLoop.PlayAnim("intercept_loop", KAnim.PlayMode.Loop).Update((System.Action<HijackedHeadquarters.Instance, float>) ((smi, dt) =>
    {
      if (Immigration.Instance.ImmigrantsAvailable)
        return;
      smi.GoTo((StateMachine.BaseState) this.operational.interceptPst);
    }));
    this.operational.interceptPst.PlayAnim("intercept").OnAnimQueueComplete(this.operational.passcode.idle_unlocked);
    this.operational.readyToPrint.DefaultState(this.operational.readyToPrint.pre).EventTransition(GameHashes.PrinterceptorPrint, this.operational.readyToPrint.pst);
    this.operational.readyToPrint.pre.PlayAnim("print_ready_pre").OnAnimQueueComplete(this.operational.readyToPrint.loop);
    this.operational.readyToPrint.loop.QueueAnim("print_ready").QueueAnim("print_ready_loop", true);
    this.operational.readyToPrint.pst.PlayAnim("printing").ScheduleAction("PrinterceptorPrintDelay", 1f, (System.Action<HijackedHeadquarters.Instance>) (smi => smi.PrintSelectedEntity())).Exit((StateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State.Callback) (smi =>
    {
      if (smi.sm.hasBeenCompleted.Get(smi))
        return;
      smi.sm.hasBeenCompleted.Set(true, smi);
      smi.ShowCompletedNotification();
    })).OnAnimQueueComplete(this.operational.passcode.idle_unlocked);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class OperationalStates : 
    GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State
  {
    public HijackedHeadquarters.PasscodeStates passcode;
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State interceptPre;
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State interceptLoop;
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State interceptPst;
    public HijackedHeadquarters.ReadyToPrintStates readyToPrint;
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State printing;
  }

  public class PasscodeStates : 
    GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State
  {
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State idle_locked;
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State unlocking;
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State idle_unlocked;
  }

  public class ReadyToPrintStates : 
    GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State
  {
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State pre;
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State loop;
    public GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.State pst;
  }

  public new class Instance : 
    GameStateMachine<HijackedHeadquarters, HijackedHeadquarters.Instance, IStateMachineTarget, HijackedHeadquarters.Def>.GameInstance,
    IUserControlledCapacity
  {
    [MyCmpGet]
    private Storage m_storage;
    [Serialize]
    private bool m_introPopupSeen;
    private EventInfoData eventInfo;
    private Notification m_endNotification;
    private MeterController m_progressMeter;
    [Serialize]
    public Dictionary<Tag, int> printCounts = new Dictionary<Tag, int>();
    public static GameObject PrinterceptorInstance;
    private int onBuildingSelectHandle = -1;
    [Serialize]
    public float userMaxCapacity = 500f;

    float IUserControlledCapacity.UserMaxCapacity
    {
      get => this.userMaxCapacity;
      set
      {
        this.userMaxCapacity = value;
        this.ApplyMaxCapacity();
      }
    }

    float IUserControlledCapacity.AmountStored => this.m_storage.MassStored();

    float IUserControlledCapacity.MinCapacity => 0.0f;

    float IUserControlledCapacity.MaxCapacity => 500f;

    bool IUserControlledCapacity.WholeValues => true;

    LocString IUserControlledCapacity.CapacityUnits => (LocString) DatabankHelper.NAME_PLURAL;

    bool IUserControlledCapacity.ControlEnabled() => this.smi.sm.passcodeUnlocked.Get(this.smi);

    public void ApplyMaxCapacity()
    {
      this.m_storage.capacityKg = this.userMaxCapacity;
      this.m_storage.GetComponent<ManualDeliveryKG>().AbortDelivery("Switching to new delivery request");
      this.m_storage.GetComponent<ManualDeliveryKG>().capacity = this.userMaxCapacity;
      this.m_storage.GetComponent<ManualDeliveryKG>().refillMass = this.userMaxCapacity;
      this.m_storage.GetComponent<ManualDeliveryKG>().FillToCapacity = true;
      this.m_storage.Trigger(-945020481, (object) this);
      if ((double) this.m_storage.MassStored() <= (double) this.userMaxCapacity)
        return;
      this.m_storage.DropSome((Tag) DatabankHelper.ID, this.m_storage.MassStored() - this.userMaxCapacity);
    }

    public Instance(IStateMachineTarget master, HijackedHeadquarters.Def def)
      : base(master, def)
    {
      this.m_progressMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
      HijackedHeadquarters.Instance.PrinterceptorInstance = this.smi.master.gameObject;
    }

    public void ChangeUIDescriptionToCompleted()
    {
      BuildingComplete component = this.gameObject.GetComponent<BuildingComplete>();
      this.gameObject.GetComponent<KSelectable>().SetName((string) BUILDINGS.PREFABS.HIJACKEDHEADQUARTERS_COMPLETED.NAME);
      component.SetDescriptionFlavour((string) BUILDINGS.PREFABS.HIJACKEDHEADQUARTERS_COMPLETED.EFFECT);
      component.SetDescription((string) BUILDINGS.PREFABS.HIJACKEDHEADQUARTERS_COMPLETED.DESC);
    }

    public void AddLore()
    {
      if (!StoryManager.Instance.IsStoryComplete(Db.Get().Stories.HijackedHeadquarters) || !((UnityEngine.Object) this.smi.master.GetComponent<LoreBearer>() == (UnityEngine.Object) null))
        return;
      LoreBearerUtil.AddLoreTo(this.smi.master.gameObject, LoreBearerUtil.UnlockSpecificEntryThenNext("story_trait_hijackheadquarters_complete", (string) UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH6, new System.Action<InfoDialogScreen>(LoreBearerUtil.UnlockNextEmail), true));
    }

    public void Intercept()
    {
      this.smi.sm.interceptCharges.Delta(1, this.smi);
      ImmigrantScreen.instance.ClearRejectedShuffleState();
      Immigration.Instance.EndImmigration();
      if (this.smi.sm.interceptCharges.Get(this.smi) >= 3)
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.operational.readyToPrint);
      SelectTool.Instance.Select((KSelectable) null, true);
    }

    public void ActivatePrintInterface()
    {
      SelectTool.Instance.Select((KSelectable) null, true);
      PrinterceptorScreen.Instance.SetTarget(this);
      PrinterceptorScreen.Instance.Show(true);
    }

    public void UnlockPrinterceptor()
    {
      this.GetComponent<BuildingEnabledButton>().IsEnabled = true;
      this.smi.sm.passcodeUnlocked.Set(true, this.smi);
    }

    public void PrintSelectedEntity()
    {
      this.smi.sm.interceptCharges.Set(0, this.smi);
      GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(PrinterceptorScreen.Instance.selectedEntityTag), Grid.CellToPosCCC(Grid.PosToCell(this.gameObject), Grid.SceneLayer.Creatures) + Vector3.up * 1.5f, Quaternion.identity);
      this.smi.master.GetComponent<Storage>().ConsumeIgnoringDisease((Tag) DatabankHelper.ID, (float) HijackedHeadquartersConfig.GetDataBankCost(PrinterceptorScreen.Instance.selectedEntityTag, this.smi.printCounts.ContainsKey(PrinterceptorScreen.Instance.selectedEntityTag) ? this.smi.printCounts[PrinterceptorScreen.Instance.selectedEntityTag] : 0));
      gameObject.SetActive(true);
      if (!this.smi.printCounts.ContainsKey(PrinterceptorScreen.Instance.selectedEntityTag))
        this.smi.printCounts[PrinterceptorScreen.Instance.selectedEntityTag] = 0;
      this.smi.printCounts[PrinterceptorScreen.Instance.selectedEntityTag]++;
    }

    public override void StartSM()
    {
      base.StartSM();
      this.UpdateStatusItems();
      this.UpdateMeter();
      StoryManager.Instance.ForceCreateStory(Db.Get().Stories.HijackedHeadquarters, this.gameObject.GetMyWorldId());
      this.onBuildingSelectHandle = this.Subscribe(-1503271301, new System.Action<object>(this.OnBuildingSelect));
      StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.HijackedHeadquarters);
      if (StoryManager.Instance.IsStoryComplete(Db.Get().Stories.HijackedHeadquarters))
        this.smi.AddLore();
      this.m_storage.capacityKg = this.userMaxCapacity;
      this.ApplyMaxCapacity();
    }

    public override void StopSM(string reason)
    {
      this.Unsubscribe(ref this.onBuildingSelectHandle);
      base.StopSM(reason);
    }

    private void OnBuildingSelect(object obj)
    {
      if (!((Boxed<bool>) obj).value)
        return;
      if (!this.m_introPopupSeen)
        this.ShowIntroNotification();
      if (this.m_endNotification == null)
        return;
      this.m_endNotification.customClickCallback(this.m_endNotification.customClickData);
    }

    private void UpdateStatusItems() => this.gameObject.GetComponent<KSelectable>();

    public void UpdateMeter()
    {
      this.m_progressMeter.SetPositionPercent(Mathf.Clamp01((float) this.smi.sm.interceptCharges.Get(this.smi) / 3f));
    }

    public void ShowIntroNotification()
    {
      this.m_introPopupSeen = true;
      EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData((string) CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.BEGIN_POPUP.NAME, (string) CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.BEGIN_POPUP.DESCRIPTION, (string) CODEX.STORY_TRAITS.CLOSE_BUTTON, "printerceptordiscovered_kanim", EventInfoDataHelper.PopupType.BEGIN));
    }

    public void ShowCompletedNotification()
    {
      this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData((string) CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.END_POPUP.NAME, (string) CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.END_POPUP.DESCRIPTION, (string) CODEX.STORY_TRAITS.HIJACK_HEADQUARTERS.END_POPUP.BUTTON, "printerceptorprintready_kanim", EventInfoDataHelper.PopupType.COMPLETE);
      this.m_endNotification = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.CompleteStory));
      this.gameObject.AddOrGet<Notifier>().Add(this.m_endNotification);
      this.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, (object) this.smi);
    }

    public void ClearEndNotification()
    {
      this.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired);
      if (this.m_endNotification != null)
        this.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
      this.m_endNotification = (Notification) null;
    }

    public void CompleteStory(object _)
    {
      if (this.m_endNotification != null)
        this.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
      this.UpdateStatusItems();
      this.ClearEndNotification();
      Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) this.smi), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
      StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.HijackedHeadquarters, this.gameObject.GetComponent<MonoBehaviour>(), new FocusTargetSequence.Data()
      {
        WorldId = this.smi.GetMyWorldId(),
        OrthographicSize = 6f,
        TargetSize = 6f,
        Target = posCcc,
        PopupData = this.eventInfo,
        CompleteCB = new System.Action(this.OnStorySequenceComplete),
        CanCompleteCB = (Func<bool>) null
      });
      this.AddLore();
    }

    private void OnStorySequenceComplete()
    {
      Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) this.smi), new CellOffset(-1, 1)), Grid.SceneLayer.Ore);
      StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.HijackedHeadquarters, posCcc);
      this.eventInfo = (EventInfoData) null;
    }

    protected override void OnCleanUp()
    {
      if (this.m_endNotification == null)
        return;
      this.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
    }
  }
}
