// Decompiled with JetBrains decompiler
// Type: MinnowImperativePOIStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class MinnowImperativePOIStates : 
  GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>
{
  public const int TOTALPOICOUNT = 3;
  private const int STARTING_ATTRIBUTE_LEVEL = 4;
  private const int STARTING_SKILL_POINTS = 3;
  public GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State off;
  public MinnowImperativePOIStates.OnStates on;
  public GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State disabling;
  public GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State poi_completed_pending;
  public GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State poi_completed_acknowledged;
  public GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State off_poi_completed;
  public StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.BoolParameter hasShownQuestPopup;
  public StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.BoolParameter hasShownCompletedPopup;
  public StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.BoolParameter isCompleted;
  public StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.BoolParameter hasClickedSideScreen;
  public const int GASKET_REWARD_COUNT = 1;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.off;
    this.off.PlayAnim("empty_water").ParamTransition<bool>((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Parameter<bool>) this.hasShownCompletedPopup, this.off_poi_completed, GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.IsTrue).ParamTransition<bool>((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Parameter<bool>) this.isCompleted, this.poi_completed_pending, GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.IsTrue).Transition(this.on.working, (StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Transition.ConditionCallback) (smi => MinnowImperativePOIStates.HasLiquid(smi) && smi.sm.hasClickedSideScreen.Get(smi))).Transition(this.on.enter, (StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Transition.ConditionCallback) (smi => MinnowImperativePOIStates.HasLiquid(smi) && MinnowImperativePOIStates.IsVisibleOnCamera(smi))).ToggleStatusItem(Db.Get().MiscStatusItems.MinnowPOIDehydratedStatus);
    this.on.DoNothing().Transition(this.disabling, (StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Transition.ConditionCallback) (smi => !MinnowImperativePOIStates.HasLiquid(smi)), UpdateRate.SIM_1000ms);
    this.disabling.PlayAnim("exit").OnAnimQueueComplete(this.off);
    this.on.enter.PlayAnim("enter").OnAnimQueueComplete(this.on.waiting);
    this.on.waiting.PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Parameter<bool>) this.hasClickedSideScreen, this.on.working, GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.IsTrue);
    this.on.working.PlayAnim("on", KAnim.PlayMode.Loop).ToggleComponent<ManualDeliveryKG>().Enter((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback) (smi => smi.GetComponent<ManualDeliveryKG>().Pause(false, "Delivery enabled"))).ParamTransition<bool>((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Parameter<bool>) this.hasClickedSideScreen, this.on.waiting, GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.IsFalse).Transition(this.poi_completed_pending, new StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Transition.ConditionCallback(MinnowImperativePOIStates.HasEnoughMass), UpdateRate.SIM_1000ms).Exit((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback) (smi => smi.GetComponent<ManualDeliveryKG>().Pause(true, "Delivery disabled")));
    this.poi_completed_pending.ParamTransition<bool>((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.Parameter<bool>) this.hasShownCompletedPopup, this.poi_completed_acknowledged, GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.IsTrue).PlayAnim("on", KAnim.PlayMode.Loop).Enter((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback) (smi =>
    {
      this.isCompleted.Set(true, smi);
      smi.ClearUserPriority();
      smi.ShowCompletedNotification();
    }));
    this.poi_completed_acknowledged.PlayAnim((Func<MinnowImperativePOIStates.Instance, string>) (smi => !MinnowImperativePOIStates.Instance.AllPOIsCompleted() ? "exit" : "victory")).Enter((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback) (smi =>
    {
      smi.ClearUserPriority();
      if (!MinnowImperativePOIStates.Instance.AllPOIsCompleted())
        return;
      MusicManager.instance.PlaySong("Stinger_NewDuplicant");
    })).OnAnimQueueComplete(this.off_poi_completed).Toggle("Toggle selectable", new StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback(MinnowImperativePOIStates.MakeItUnselectable), new StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback(MinnowImperativePOIStates.MakeItSelectable));
    this.off_poi_completed.PlayAnim("off").Toggle("Toggle selectable", new StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback(MinnowImperativePOIStates.MakeItUnselectable), new StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback(MinnowImperativePOIStates.MakeItSelectable)).Enter((StateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State.Callback) (smi => smi.ClearUserPriority()));
  }

  private static void MakeItUnselectable(MinnowImperativePOIStates.Instance smi)
  {
    smi.SetSelectable(false);
  }

  private static void MakeItSelectable(MinnowImperativePOIStates.Instance smi)
  {
    smi.SetSelectable(true);
  }

  private static bool IsVisibleOnCamera(MinnowImperativePOIStates.Instance smi)
  {
    return (UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null && CameraController.Instance.IsVisiblePos(smi.transform.GetPosition());
  }

  private static bool HasEnoughMass(MinnowImperativePOIStates.Instance smi)
  {
    return (double) smi.GetComponent<Storage>().GetMassAvailable(smi.def.requestedTag) >= (double) smi.def.requiredMass;
  }

  private static bool HasLiquid(MinnowImperativePOIStates.Instance smi)
  {
    int cell = Grid.CellAbove(Grid.PosToCell(smi.transform.GetPosition()));
    return Grid.IsValidCell(cell) && Grid.Element[cell].IsLiquid;
  }

  public static int GetPOIStartedCount()
  {
    int poiStartedCount = 0;
    foreach (MinnowImperativePOIStates.Instance smi in Components.MinnowImperativePOIs.Items)
    {
      if (smi.sm.hasShownQuestPopup.Get(smi))
        ++poiStartedCount;
    }
    return poiStartedCount;
  }

  public static int GetPOICompletedCount()
  {
    int poiCompletedCount = 0;
    foreach (MinnowImperativePOIStates.Instance smi in Components.MinnowImperativePOIs.Items)
    {
      if (smi.sm.isCompleted.Get(smi))
        ++poiCompletedCount;
    }
    return poiCompletedCount;
  }

  private static void UnlockWinAchievement(MinnowImperativePOIStates.Instance smi)
  {
    SaveGame.Instance.ColonyAchievementTracker.allMinnowQuestsCompleted = true;
  }

  private static void SpawnReward(MinnowImperativePOIStates.Instance smi)
  {
    Vector3 posCbc = Grid.CellToPosCBC(Grid.PosToCell(smi.gameObject), Grid.SceneLayer.Ore);
    switch (smi.def.minnowPOIIdentity)
    {
      case MinnowImperativePOIStates.MinnowPOIIdentity.POI_A:
        Util.KInstantiate(Assets.GetPrefab((Tag) "PlasticGasket"), posCbc).SetActive(true);
        Util.KInstantiate(Assets.GetPrefab((Tag) "OxyCoralSeed"), posCbc + Vector3.right).SetActive(true);
        Util.KInstantiate(Assets.GetPrefab((Tag) "OxyCoralSeed"), posCbc + Vector3.left).SetActive(true);
        break;
      case MinnowImperativePOIStates.MinnowPOIIdentity.POI_B:
        Util.KInstantiate(Assets.GetPrefab((Tag) "PlasticGasket"), posCbc).SetActive(true);
        Util.KInstantiate(Assets.GetPrefab((Tag) DewPalmConfig.SEED_ID), posCbc + Vector3.right).SetActive(true);
        Util.KInstantiate(Assets.GetPrefab((Tag) DewPalmConfig.SEED_ID), posCbc + Vector3.left).SetActive(true);
        break;
      case MinnowImperativePOIStates.MinnowPOIIdentity.POI_C:
        Util.KInstantiate(Assets.GetPrefab((Tag) "PlasticGasket"), posCbc).SetActive(true);
        break;
    }
  }

  public enum MinnowPOIIdentity
  {
    POI_A,
    POI_B,
    POI_C,
  }

  public class OnStates : 
    GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State
  {
    public GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State enter;
    public GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State waiting;
    public GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.State working;
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag requestedTag;
    public float requiredMass;
    public MinnowImperativePOIStates.MinnowPOIIdentity minnowPOIIdentity;
  }

  public new class Instance(IStateMachineTarget master, MinnowImperativePOIStates.Def def) : 
    GameStateMachine<MinnowImperativePOIStates, MinnowImperativePOIStates.Instance, IStateMachineTarget, MinnowImperativePOIStates.Def>.GameInstance(master, def),
    ISidescreenButtonControl
  {
    private int onSelectHandle = -1;
    private Notification completedNotification;
    private static readonly string[] MinnowPOIPrefabIDs = new string[3]
    {
      "MinnowImperativePOIA",
      "MinnowImperativePOIB",
      "MinnowImperativePOIC"
    };

    public string SidescreenTitle => (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_A.UI_HEADER;

    public bool HasUserEverClicked => this.sm.hasShownQuestPopup.Get(this);

    public bool WasCompletedAndAcknowledged
    {
      get
      {
        return this.smi.sm.isCompleted.Get(this.smi) && this.smi.sm.hasShownCompletedPopup.Get(this.smi);
      }
    }

    public override void StartSM()
    {
      base.StartSM();
      Components.MinnowImperativePOIs.Add(this);
      this.onSelectHandle = this.Subscribe(-1503271301, new System.Action<object>(this.OnObjectSelected));
    }

    public override void StopSM(string reason)
    {
      if (this.onSelectHandle != -1)
        this.Unsubscribe(ref this.onSelectHandle);
      this.ClearCompletedNotification();
      Components.MinnowImperativePOIs.Remove(this);
      base.StopSM(reason);
    }

    public void SetSelectable(bool selectable)
    {
      KSelectable component = this.gameObject.GetComponent<KSelectable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.IsSelectable = selectable;
    }

    private bool IsInPopupEligibleState()
    {
      return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.on);
    }

    private void OnObjectSelected(object data)
    {
      if (!((Boxed<bool>) data).value)
        return;
      if (this.completedNotification != null)
      {
        Notification.ClickCallback customClickCallback = this.completedNotification.customClickCallback;
        if (customClickCallback == null)
          return;
        customClickCallback((object) this.completedNotification);
      }
      else
      {
        if (!this.IsInPopupEligibleState() || this.smi.sm.hasShownQuestPopup.Get(this.smi))
          return;
        this.ShowQuestPopup();
      }
    }

    private string GetStartPopupTitle(
      MinnowImperativePOIStates.MinnowPOIIdentity identity)
    {
      string str = "";
      switch (identity)
      {
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_A:
          str = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_START_POI_A_TITLE;
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_B:
          str = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_START_POI_B_TITLE;
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_C:
          str = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_START_POI_C_TITLE;
          break;
      }
      return str.Replace("{0}", (MinnowImperativePOIStates.GetPOIStartedCount() + 1).ToString()).Replace("{1}", 3.ToString());
    }

    private string GetStartPopupDescription(
      MinnowImperativePOIStates.MinnowPOIIdentity identity)
    {
      string popupDescription = "";
      switch (identity)
      {
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_A:
          popupDescription = ((string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_START_POI_A_DESCRIPTION).Replace("{AMOUNT2}", GameUtil.GetFormattedMass(200f)).Replace("{AMOUNT1}", "1");
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_B:
          popupDescription = ((string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_START_POI_B_DESCRIPTION).Replace("{AMOUNT1}", GameUtil.GetFormattedCaloriesForItem(MinnowImperativePOIBConfig.RequiredDeliveryTag, 10f)).Replace("{AMOUNT2}", "1");
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_C:
          popupDescription = ((string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_START_POI_C_DESCRIPTION).Replace("{AMOUNT1}", GameUtil.GetFormattedMass(10f)).Replace("{AMOUNT2}", "1");
          break;
      }
      return popupDescription;
    }

    private string GetCompletePopupTitle(
      MinnowImperativePOIStates.MinnowPOIIdentity identity)
    {
      string completePopupTitle = "";
      switch (identity)
      {
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_A:
          completePopupTitle = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_COMPLETE_POI_A_TITLE;
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_B:
          completePopupTitle = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_COMPLETE_POI_B_TITLE;
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_C:
          completePopupTitle = (string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_COMPLETE_POI_C_TITLE;
          break;
      }
      return completePopupTitle;
    }

    private string GetCompletePopupDescription(
      MinnowImperativePOIStates.MinnowPOIIdentity identity)
    {
      string popupDescription = "";
      switch (identity)
      {
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_A:
          popupDescription = ((string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_COMPLETE_POI_A_DESCRIPTION).Replace("{AMOUNT1}", GameUtil.GetFormattedMass(200f)).Replace("{AMOUNT2}", "1");
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_B:
          popupDescription = ((string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_COMPLETE_POI_B_DESCRIPTION).Replace("{AMOUNT1}", GameUtil.GetFormattedMass(10f)).Replace("{AMOUNT2}", "1");
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_C:
          popupDescription = ((string) COLONY_ACHIEVEMENTS.FINDING_MINNOW.POPUPS.POPUP_COMPLETE_POI_C_DESCRIPTION).Replace("{AMOUNT1}", GameUtil.GetFormattedMass(10f)).Replace("{AMOUNT2}", "1");
          break;
      }
      return popupDescription;
    }

    private string GetStartPopupImage(
      MinnowImperativePOIStates.MinnowPOIIdentity identity)
    {
      string startPopupImage = "";
      switch (identity)
      {
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_A:
          startPopupImage = "MinnowDiscoveryA_kanim";
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_B:
          startPopupImage = "MinnowDiscoveryB_kanim";
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_C:
          startPopupImage = "MinnowDiscoveryC_kanim";
          break;
      }
      return startPopupImage;
    }

    private string GetCompletedPopupImage(
      MinnowImperativePOIStates.MinnowPOIIdentity identity)
    {
      string completedPopupImage = "";
      switch (identity)
      {
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_A:
          completedPopupImage = "MinnowCompleteA_kanim";
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_B:
          completedPopupImage = "MinnowCompleteB_kanim";
          break;
        case MinnowImperativePOIStates.MinnowPOIIdentity.POI_C:
          completedPopupImage = "MinnowCompleteC_kanim";
          break;
      }
      return completedPopupImage;
    }

    public void ShowCompletedNotification()
    {
      this.completedNotification = EventInfoScreen.CreateNotification(EventInfoDataHelper.GenerateStoryTraitData(this.GetCompletePopupTitle(this.smi.def.minnowPOIIdentity), this.GetCompletePopupDescription(this.smi.def.minnowPOIIdentity), (string) UI.TOOLTIPS.CLOSETOOLTIP, this.GetCompletedPopupImage(this.smi.def.minnowPOIIdentity), MinnowImperativePOIStates.Instance.AllPOIsCompleted() ? EventInfoDataHelper.PopupType.COMPLETE : EventInfoDataHelper.PopupType.NORMAL, callback: new System.Action(this.OnCompletionPopupAcknowledged)));
      this.gameObject.AddOrGet<Notifier>().Add(this.completedNotification);
    }

    public void ClearUserPriority()
    {
      Prioritizable component = this.smi.GetComponent<Prioritizable>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
    }

    private void OnCompletionPopupAcknowledged()
    {
      this.ClearCompletedNotification();
      MinnowImperativePOIStates.SpawnReward(this.smi);
      int poiCompletedCount = MinnowImperativePOIStates.GetPOICompletedCount();
      SaveGame.Instance.ColonyAchievementTracker.minnowQuestsCompleted = Mathf.Max(SaveGame.Instance.ColonyAchievementTracker.minnowQuestsCompleted, poiCompletedCount);
      Game.Instance.StartCoroutine(this.CompletionCameraSequence(poiCompletedCount >= 3));
    }

    private IEnumerator CompletionCameraSequence(bool wasLastPOI)
    {
      Vector3 cameraStartPos = CameraController.Instance.transform.position;
      Vector3 position = this.transform.GetPosition();
      if (!SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Pause(false);
      RootMenu.Instance.canTogglePauseScreen = false;
      CameraController.Instance.DisableUserCameraControl = true;
      CameraController.Instance.SetWorldInteractive(false);
      ManagementMenu.Instance.CloseAll();
      StoryMessageScreen.HideInterface(true);
      OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
      CameraController.Instance.SetOverrideZoomSpeed(10f);
      CameraController.Instance.SetTargetPos(position, 8f, false);
      yield return (object) SequenceUtil.WaitForSecondsRealtime(0.5f);
      this.smi.sm.hasShownCompletedPopup.Set(true, this.smi);
      if (SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Unpause(false);
      SpeedControlScreen.Instance.SetSpeed(0);
      yield return (object) SequenceUtil.WaitForSecondsRealtime(2.5f);
      if (!wasLastPOI)
      {
        Vector3 nextPOIPos;
        if (MinnowImperativePOIStates.Instance.FindNextUncompletedPOIPosition(out nextPOIPos))
        {
          int x;
          int y;
          Grid.CellToXY(Grid.PosToCell(nextPOIPos), out x, out y);
          GridVisibility.Reveal(x, y, 16 /*0x10*/, 16f);
          yield return (object) null;
          CameraController.Instance.SetOverrideZoomSpeed(2f);
          CameraController.Instance.SetTargetPos(nextPOIPos, 8f, false);
          yield return (object) SequenceUtil.WaitForSecondsRealtime(2.5f);
        }
        nextPOIPos = new Vector3();
      }
      else if (!MinnowImperativePOIStates.Instance.MinnowAlreadyExists())
      {
        MinnowImperativePOIStates.UnlockWinAchievement(this.smi);
        this.SpawnMinnow();
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.off_poi_completed);
      }
      CameraController.Instance.SetOverrideZoomSpeed(1f);
      CameraController.Instance.SetWorldInteractive(true);
      CameraController.Instance.DisableUserCameraControl = false;
      RootMenu.Instance.canTogglePauseScreen = true;
      StoryMessageScreen.HideInterface(false);
      NotificationScreen_TemporaryActions.Instance.CreateCameraReturnActionButton(cameraStartPos);
    }

    private static bool FindNextUncompletedPOIPosition(out Vector3 position)
    {
      for (int index = 0; index < 3; ++index)
      {
        MinnowImperativePOIStates.MinnowPOIIdentity minnowPoiIdentity = (MinnowImperativePOIStates.MinnowPOIIdentity) index;
        foreach (MinnowImperativePOIStates.Instance smi in Components.MinnowImperativePOIs.Items)
        {
          if (smi.def.minnowPOIIdentity == minnowPoiIdentity && !smi.sm.isCompleted.Get(smi))
          {
            position = smi.transform.GetPosition();
            return true;
          }
        }
        string minnowPoiPrefabId = MinnowImperativePOIStates.Instance.MinnowPOIPrefabIDs[index];
        List<WorldGenSpawner.Spawnable> spawnablesWithTag = SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(false, new Tag(minnowPoiPrefabId));
        if (spawnablesWithTag.Count > 0)
        {
          position = Grid.CellToPosCCC(spawnablesWithTag[0].cell, Grid.SceneLayer.Creatures);
          return true;
        }
      }
      position = Vector3.zero;
      return false;
    }

    public void ClearCompletedNotification()
    {
      if (this.completedNotification == null)
        return;
      this.gameObject.AddOrGet<Notifier>().Remove(this.completedNotification);
      this.completedNotification = (Notification) null;
    }

    public static bool AllPOIsCompleted() => MinnowImperativePOIStates.GetPOICompletedCount() >= 3;

    private static bool MinnowAlreadyExists()
    {
      foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      {
        if (minionIdentity.personalityResourceId == (HashedString) "MINNOW")
          return true;
      }
      return false;
    }

    private void SpawnMinnow()
    {
      MinionStartingStats minionStartingStats = new MinionStartingStats(Db.Get().Personalities.Get("MINNOW"), guaranteedTraitID: "AncientKnowledge");
      foreach (string key in DUPLICANTSTATS.ALL_ATTRIBUTES)
        minionStartingStats.StartingLevels[key] += 4;
      GameObject prefab = Assets.GetPrefab((Tag) BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
      GameObject gameObject = Util.KInstantiate(prefab);
      gameObject.name = prefab.name;
      Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
      Vector3 posCbc = Grid.CellToPosCBC(Grid.PosToCell(this.gameObject), Grid.SceneLayer.Move);
      gameObject.transform.SetLocalPosition(posCbc);
      gameObject.SetActive(true);
      MinionResume component = gameObject.GetComponent<MinionResume>();
      for (int index = 0; index < 3; ++index)
        component.ForceAddSkillPoint();
      minionStartingStats.Apply(gameObject);
      gameObject.GetComponent<MinionIdentity>().arrivalTime = (float) (-1 * UnityEngine.Random.Range(2050, 2180));
      gameObject.GetMyWorld().SetDupeVisited();
    }

    private void ShowQuestPopup()
    {
      EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(this.GetStartPopupTitle(this.smi.def.minnowPOIIdentity), this.GetStartPopupDescription(this.smi.def.minnowPOIIdentity), (string) UI.TOOLTIPS.CLOSETOOLTIP, this.GetStartPopupImage(this.smi.def.minnowPOIIdentity), EventInfoDataHelper.PopupType.BEGIN, callback: (System.Action) (() =>
      {
        this.smi.sm.hasShownQuestPopup.Set(true, this.smi);
        KSelectable component = this.gameObject.GetComponent<KSelectable>();
        SelectTool.Instance.Select((KSelectable) null);
        SelectTool.Instance.Select(component);
      })));
    }

    public string SidescreenButtonText
    {
      get
      {
        ManualDeliveryKG component = this.gameObject.GetComponent<ManualDeliveryKG>();
        return (UnityEngine.Object) component != (UnityEngine.Object) null && component.enabled ? (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_A.UI_BUTTON_DISABLE : (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_A.UI_BUTTON_ENABLE;
      }
    }

    public string SidescreenButtonTooltip
    {
      get
      {
        ManualDeliveryKG component = this.gameObject.GetComponent<ManualDeliveryKG>();
        return (UnityEngine.Object) component != (UnityEngine.Object) null && component.enabled ? (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_A.UI_BUTTON_DISABLE_TOOLTIP : (string) STRINGS.BUILDINGS.PREFABS.MINNOW_IMPERATIVE_POI_A.UI_BUTTON_ENABLE_TOOLTIP;
      }
    }

    public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
    {
    }

    public bool SidescreenEnabled()
    {
      return this.IsInPopupEligibleState() && this.smi.sm.hasShownQuestPopup.Get(this.smi);
    }

    public bool SidescreenButtonInteractable() => true;

    public void OnSidescreenButtonPressed()
    {
      this.smi.sm.hasClickedSideScreen.Set(!this.smi.sm.hasClickedSideScreen.Get(this.smi), this.smi);
    }

    public int HorizontalGroupID() => -1;

    public int ButtonSideScreenSortOrder() => 20;
  }
}
