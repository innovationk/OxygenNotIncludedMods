// Decompiled with JetBrains decompiler
// Type: DebugHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.IO;
using System.Threading;
using UnityEngine;

#nullable disable
public class DebugHandler : IInputHandler
{
  public static bool InstantBuildMode;
  public static bool InvincibleMode;
  public static bool SelectInEditor;
  public static bool DebugPathFinding;
  public static bool ScreenshotMode;
  public static bool TimelapseMode;
  public static bool HideUI;
  public static bool DebugCellInfo;
  public static bool DebugNextCall;
  public static bool RevealFogOfWar;
  private bool superTestMode;
  private bool ultraTestMode;
  private bool slowTestMode;
  private static int activeWorldBeforeOverride = -1;

  public static bool NotificationsDisabled { get; private set; }

  public static bool enabled { get; private set; }

  public DebugHandler()
  {
    DebugHandler.enabled = File.Exists(System.IO.Path.Combine(Application.dataPath, "debug_enable.txt"));
    DebugHandler.enabled = DebugHandler.enabled || File.Exists(System.IO.Path.Combine(Application.dataPath, "../debug_enable.txt"));
    DebugHandler.enabled = DebugHandler.enabled || GenericGameSettings.instance.debugEnable;
  }

  public string handlerName => nameof (DebugHandler);

  public KInputHandler inputHandler { get; set; }

  public static int GetMouseCell()
  {
    return Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos() with
    {
      z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters
    }));
  }

  public static Vector3 GetMousePos()
  {
    return Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos() with
    {
      z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters
    });
  }

  private void SpawnMinion(bool addAtmoSuit = false)
  {
    if ((UnityEngine.Object) Immigration.Instance == (UnityEngine.Object) null)
      return;
    if (!Grid.IsValidBuildingCell(DebugHandler.GetMouseCell()))
    {
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.DEBUG_TOOLS.INVALID_LOCATION, (Transform) null, DebugHandler.GetMousePos(), force_spawn: true);
    }
    else
    {
      MinionStartingStats minionStartingStats = new MinionStartingStats(false, isDebugMinion: true);
      GameObject prefab = Assets.GetPrefab((Tag) BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
      GameObject gameObject = Util.KInstantiate(prefab);
      gameObject.name = prefab.name;
      Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
      Vector3 posCbc = Grid.CellToPosCBC(DebugHandler.GetMouseCell(), Grid.SceneLayer.Move);
      gameObject.transform.SetLocalPosition(posCbc);
      gameObject.SetActive(true);
      minionStartingStats.Apply(gameObject);
      if (addAtmoSuit)
        gameObject.Subscribe(1589886948, new Action<object>(this.AddAtmosuitAfterSpawn));
      gameObject.GetMyWorld().SetDupeVisited();
    }
  }

  private void AddAtmosuitAfterSpawn(object o)
  {
    GameObject go1 = (GameObject) o;
    Vector3 localPosition = go1.transform.localPosition;
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "Atmo_Suit"), localPosition, Grid.SceneLayer.Creatures);
    gameObject.SetActive(true);
    SuitTank component1 = gameObject.GetComponent<SuitTank>();
    GameObject go2 = GameUtil.KInstantiate(Assets.GetPrefab(GameTags.Oxygen), localPosition, Grid.SceneLayer.Ore);
    go2.GetComponent<PrimaryElement>().Units = component1.capacity;
    go2.SetActive(true);
    component1.storage.Store(go2, true);
    Equippable component2 = gameObject.GetComponent<Equippable>();
    go1.GetComponent<MinionIdentity>().ValidateProxy();
    component2.Assign(go1.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<Equipment>().GetComponent<IAssignableIdentity>());
    component2.isEquipped = true;
    gameObject.GetComponent<EquippableWorkable>().CancelChore("Debug Handler");
    go1.Unsubscribe(1589886948, new Action<object>(this.AddAtmosuitAfterSpawn));
  }

  public static void SetDebugEnabled(bool debugEnabled) => DebugHandler.enabled = debugEnabled;

  public static void ToggleDisableNotifications()
  {
    DebugHandler.NotificationsDisabled = !DebugHandler.NotificationsDisabled;
  }

  public static void ToggleInstantBuildMode()
  {
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      return;
    DebugHandler.InstantBuildMode = !DebugHandler.InstantBuildMode;
    InterfaceTool.ToggleConfig(Action.DebugInstantBuildMode);
    Game.Instance.Trigger(1557339983, (object) null);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      PlanScreen.Instance.Refresh();
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      BuildMenu.Instance.Refresh();
    if ((UnityEngine.Object) OverlayMenu.Instance != (UnityEngine.Object) null)
      OverlayMenu.Instance.Refresh();
    if ((UnityEngine.Object) ConsumerManager.instance != (UnityEngine.Object) null)
      ConsumerManager.instance.RefreshDiscovered();
    if ((UnityEngine.Object) ManagementMenu.Instance != (UnityEngine.Object) null)
      ManagementMenu.Instance.Refresh();
    if ((UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
      DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
    Game.Instance.Trigger(1594320620, (object) "all_the_things");
  }

  private string GetScreenshotFileName()
  {
    string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
    string str = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(activeSaveFilePath), "screenshot");
    string fileName = System.IO.Path.GetFileName(activeSaveFilePath);
    Directory.CreateDirectory(str);
    string path2 = $"{System.IO.Path.GetFileNameWithoutExtension(fileName)}_{GameClock.Instance.GetCycle().ToString()}_{System.DateTime.Now.ToString("yyyy-MM-dd_HH\\hmm\\mss\\s")}.png";
    return System.IO.Path.Combine(str, path2);
  }

  public unsafe void OnKeyDown(KButtonEvent e)
  {
    if (!DebugHandler.enabled)
      return;
    if (e.TryConsume(Action.DebugSpawnMinion))
      this.SpawnMinion();
    else if (e.TryConsume(Action.DebugSpawnMinionAtmoSuit))
      this.SpawnMinion(true);
    else if (e.TryConsume(Action.DebugCheerEmote))
    {
      for (int idx = 0; idx < Components.MinionIdentities.Count; ++idx)
      {
        EmoteChore emoteChore1 = new EmoteChore((IStateMachineTarget) Components.MinionIdentities[idx].GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[3]
        {
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst"
        });
        EmoteChore emoteChore2 = new EmoteChore((IStateMachineTarget) Components.MinionIdentities[idx].GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[3]
        {
          (HashedString) "cheer_pre",
          (HashedString) "cheer_loop",
          (HashedString) "cheer_pst"
        });
      }
    }
    else if (e.TryConsume(Action.DebugSpawnStressTest))
    {
      for (int index = 0; index < 60; ++index)
        this.SpawnMinion();
    }
    else if (e.TryConsume(Action.DebugSuperTestMode))
    {
      if (!this.superTestMode)
      {
        Time.timeScale = 15f;
        this.superTestMode = true;
      }
      else
      {
        Time.timeScale = 1f;
        this.superTestMode = false;
      }
    }
    else if (e.TryConsume(Action.DebugUltraTestMode))
    {
      if (!this.ultraTestMode)
      {
        Time.timeScale = 30f;
        this.ultraTestMode = true;
      }
      else
      {
        Time.timeScale = 1f;
        this.ultraTestMode = false;
      }
    }
    else if (e.TryConsume(Action.DebugSlowTestMode))
    {
      if (!this.slowTestMode)
      {
        Time.timeScale = 0.06f;
        this.slowTestMode = true;
      }
      else
      {
        Time.timeScale = 1f;
        this.slowTestMode = false;
      }
    }
    else if (e.TryConsume(Action.DebugDig) && (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      SimMessages.Dig(DebugHandler.GetMouseCell());
    else if (e.TryConsume(Action.DebugToggleFastWorkers) && (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      Game.Instance.FastWorkersModeActive = !Game.Instance.FastWorkersModeActive;
    else if (e.TryConsume(Action.DebugInstantBuildMode) && (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      DebugHandler.ToggleInstantBuildMode();
    else if (e.TryConsume(Action.DebugExplosion) && (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      GameUtil.CreateExplosion(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos() with
      {
        z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters
      }));
    else if (e.TryConsume(Action.DebugLockCursor) && GenericGameSettings.instance != null)
    {
      if (GenericGameSettings.instance.developerDebugEnable)
      {
        KInputManager.isMousePosLocked = !KInputManager.isMousePosLocked;
        KInputManager.lockedMousePos = KInputManager.GetMousePos();
      }
    }
    else if (e.TryConsume(Action.DebugDiscoverAllElements))
    {
      if ((UnityEngine.Object) DiscoveredResources.Instance != (UnityEngine.Object) null)
      {
        foreach (Element element in ElementLoader.elements)
          DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
      }
    }
    else if (e.TryConsume(Action.DebugToggleUI))
      DebugHandler.ToggleScreenshotMode();
    else if (e.TryConsume(Action.SreenShot1x))
      ScreenCapture.CaptureScreenshot(this.GetScreenshotFileName(), 1);
    else if (e.TryConsume(Action.SreenShot2x))
      ScreenCapture.CaptureScreenshot(this.GetScreenshotFileName(), 2);
    else if (e.TryConsume(Action.SreenShot8x))
      ScreenCapture.CaptureScreenshot(this.GetScreenshotFileName(), 8);
    else if (e.TryConsume(Action.SreenShot32x))
      ScreenCapture.CaptureScreenshot(this.GetScreenshotFileName(), 32 /*0x20*/);
    else if (e.TryConsume(Action.DebugCellInfo))
      DebugHandler.DebugCellInfo = !DebugHandler.DebugCellInfo;
    else if (e.TryConsume(Action.DebugToggle))
    {
      if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
        SaveGame.Instance.worldGenSpawner.SpawnEverything();
      InterfaceTool.ToggleConfig(Action.DebugToggle);
      if ((UnityEngine.Object) DebugPaintElementScreen.Instance != (UnityEngine.Object) null)
      {
        bool activeSelf = DebugPaintElementScreen.Instance.gameObject.activeSelf;
        DebugPaintElementScreen.Instance.gameObject.SetActive(!activeSelf);
        if ((bool) (UnityEngine.Object) DebugElementMenu.Instance && DebugElementMenu.Instance.root.activeSelf)
          DebugElementMenu.Instance.root.SetActive(false);
        DebugBaseTemplateButton.Instance.gameObject.SetActive(!activeSelf);
        PropertyTextures.FogOfWarScale = !activeSelf ? 1f : 0.0f;
        if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
          CameraController.Instance.EnableFreeCamera(!activeSelf);
        DebugHandler.RevealFogOfWar = !DebugHandler.RevealFogOfWar;
        Game.Instance.Trigger(-1991583975, (object) null);
      }
    }
    else if (e.TryConsume(Action.DebugCollectGarbage))
      GC.Collect();
    else if (e.TryConsume(Action.DebugInvincible))
      DebugHandler.InvincibleMode = !DebugHandler.InvincibleMode;
    else if (e.TryConsume(Action.DebugVisualTest) && (UnityEngine.Object) Scenario.Instance != (UnityEngine.Object) null)
      Scenario.Instance.SetupVisualTest();
    else if (e.TryConsume(Action.DebugGameplayTest) && (UnityEngine.Object) Scenario.Instance != (UnityEngine.Object) null)
      Scenario.Instance.SetupGameplayTest();
    else if (e.TryConsume(Action.DebugElementTest) && (UnityEngine.Object) Scenario.Instance != (UnityEngine.Object) null)
      Scenario.Instance.SetupElementTest();
    else if (e.TryConsume(Action.ToggleProfiler) && (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      Sim.SIM_HandleMessage(-409964931, 0, (byte*) null);
    else if (e.TryConsume(Action.DebugRefreshNavCell) && (UnityEngine.Object) Pathfinding.Instance != (UnityEngine.Object) null)
      Pathfinding.Instance.RefreshNavCell(DebugHandler.GetMouseCell());
    else if (e.TryConsume(Action.DebugToggleSelectInEditor))
      DebugHandler.SetSelectInEditor(!DebugHandler.SelectInEditor);
    else if (e.TryConsume(Action.DebugGotoTarget) && (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
    {
      Debug.Log((object) "Debug GoTo");
      Game.Instance.Trigger(775300118, (object) null);
      foreach (Brain cmp in Components.Brains.Items)
      {
        cmp.GetSMI<DebugGoToMonitor.Instance>()?.GoToCursor();
        cmp.GetSMI<CreatureDebugGoToMonitor.Instance>()?.GoToCursor();
      }
    }
    else if (e.TryConsume(Action.DebugTeleport))
    {
      if ((UnityEngine.Object) SelectTool.Instance == (UnityEngine.Object) null)
        return;
      KSelectable selected = SelectTool.Instance.selected;
      if ((UnityEngine.Object) selected != (UnityEngine.Object) null)
      {
        Navigator component = selected.GetComponent<Navigator>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.IsMoving())
          component.Stop();
        int mouseCell = DebugHandler.GetMouseCell();
        if (!Grid.IsValidBuildingCell(mouseCell))
        {
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.DEBUG_TOOLS.INVALID_LOCATION, (Transform) null, DebugHandler.GetMousePos(), force_spawn: true);
          return;
        }
        selected.transform.SetPosition(Grid.CellToPosCBC(mouseCell, Grid.SceneLayer.Move));
      }
    }
    else if (!e.TryConsume(Action.DebugQuickDevActions) && !e.TryConsume(Action.DebugPlace) && (!e.TryConsume(Action.DebugSelectMaterial) || !((UnityEngine.Object) Camera.main != (UnityEngine.Object) null)))
    {
      if (e.TryConsume(Action.DebugNotification) && GenericGameSettings.instance != null && (UnityEngine.Object) Tutorial.Instance != (UnityEngine.Object) null)
      {
        if (GenericGameSettings.instance.developerDebugEnable)
          Tutorial.Instance.DebugNotification();
      }
      else if (e.TryConsume(Action.DebugNotificationMessage) && GenericGameSettings.instance != null && (UnityEngine.Object) Tutorial.Instance != (UnityEngine.Object) null)
      {
        if (GenericGameSettings.instance.developerDebugEnable)
          Tutorial.Instance.DebugNotificationMessage();
      }
      else if (e.TryConsume(Action.DebugSuperSpeed))
      {
        if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
          SpeedControlScreen.Instance.ToggleRidiculousSpeed();
      }
      else if (e.TryConsume(Action.DebugGameStep))
      {
        if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
          SpeedControlScreen.Instance.DebugStepFrame();
      }
      else if (e.TryConsume(Action.DebugSimStep) && (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
        Game.Instance.ForceSimStep();
      else if (e.TryConsume(Action.DebugToggleMusic))
        AudioDebug.Get().ToggleMusic();
      else if (e.TryConsume(Action.DebugTileTest) && (UnityEngine.Object) Scenario.Instance != (UnityEngine.Object) null)
        Scenario.Instance.SetupTileTest();
      else if (e.TryConsume(Action.DebugForceLightEverywhere) && (UnityEngine.Object) PropertyTextures.instance != (UnityEngine.Object) null)
        PropertyTextures.instance.ForceLightEverywhere = !PropertyTextures.instance.ForceLightEverywhere;
      else if (e.TryConsume(Action.DebugPathFinding))
      {
        DebugHandler.DebugPathFinding = !DebugHandler.DebugPathFinding;
        Debug.Log((object) ("DebugPathFinding=" + DebugHandler.DebugPathFinding.ToString()));
      }
      else if (!e.TryConsume(Action.DebugFocus))
      {
        if (e.TryConsume(Action.DebugReportBug) && GenericGameSettings.instance != null)
        {
          if (GenericGameSettings.instance.developerDebugEnable)
          {
            int num = 0;
            string validSaveFilename;
            while (true)
            {
              validSaveFilename = SaveScreen.GetValidSaveFilename("bug_report_savefile_" + num.ToString());
              if (File.Exists(validSaveFilename))
                ++num;
              else
                break;
            }
            if ((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null)
              SaveLoader.Instance.Save(validSaveFilename, updateSavePointer: false);
            KCrashReporter.ReportBug("Bug Report", GameObject.Find("ScreenSpaceOverlayCanvas"));
          }
          else
            Debug.Log((object) "Debug crash keys are not enabled.");
        }
        else if (e.TryConsume(Action.DebugTriggerException) && GenericGameSettings.instance != null)
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            throw new ArgumentException("My test exception");
        }
        else if (e.TryConsume(Action.DebugTriggerError) && GenericGameSettings.instance != null)
        {
          if (GenericGameSettings.instance.developerDebugEnable)
          {
            UnityEngine.Debug.Log((object) "trigger error");
            KCrashReporter.disableDeduping = true;
            Debug.LogError((object) "Oooops! Testing error!");
          }
        }
        else if (e.TryConsume(Action.DebugDumpGCRoots) && GenericGameSettings.instance != null)
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            GarbageProfiler.DebugDumpRootItems();
        }
        else if (e.TryConsume(Action.DebugDumpGarbageReferences) && GenericGameSettings.instance != null)
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            GarbageProfiler.DebugDumpGarbageStats();
        }
        else if (e.TryConsume(Action.DebugDumpEventData) && GenericGameSettings.instance != null)
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            KObjectManager.Instance.DumpEventData();
        }
        else if (e.TryConsume(Action.DebugDumpSceneParitionerLeakData) && GenericGameSettings.instance != null)
        {
          if (!GenericGameSettings.instance.developerDebugEnable)
            ;
        }
        else if (e.TryConsume(Action.DebugCrashSim) && GenericGameSettings.instance != null)
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            new Thread((ThreadStart) (() => Sim.SIM_DebugCrash())).Start();
        }
        else if (e.TryConsume(Action.DebugNextCall))
          DebugHandler.DebugNextCall = true;
        else if (e.TryConsume(Action.DebugTogglePersonalPriorityComparison))
          Chore.ENABLE_PERSONAL_PRIORITIES = !Chore.ENABLE_PERSONAL_PRIORITIES;
        else if (e.TryConsume(Action.DebugToggleClusterFX) && (UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
          CameraController.Instance.ToggleClusterFX();
      }
    }
    if (!e.Consumed || !((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null))
      return;
    Game.Instance.debugWasUsed = true;
    KCrashReporter.debugWasUsed = true;
  }

  public static void SetSelectInEditor(bool select_in_editor)
  {
  }

  public static void ToggleScreenshotMode()
  {
    DebugHandler.ScreenshotMode = !DebugHandler.ScreenshotMode;
    DebugHandler.UpdateUI();
    if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
      CameraController.Instance.EnableFreeCamera(DebugHandler.ScreenshotMode);
    if (!((UnityEngine.Object) KScreenManager.Instance != (UnityEngine.Object) null))
      return;
    KScreenManager.Instance.DisableInput(DebugHandler.ScreenshotMode);
  }

  public static void SetTimelapseMode(bool enabled, int world_id = 0)
  {
    DebugHandler.TimelapseMode = enabled;
    if (enabled)
    {
      DebugHandler.activeWorldBeforeOverride = ClusterManager.Instance.activeWorldId;
      ClusterManager.Instance.TimelapseModeOverrideActiveWorld(world_id);
    }
    else
      ClusterManager.Instance.TimelapseModeOverrideActiveWorld(DebugHandler.activeWorldBeforeOverride);
    World.Instance.zoneRenderData.OnActiveWorldChanged();
    DebugHandler.UpdateUI();
  }

  private static void UpdateUI()
  {
    if ((UnityEngine.Object) GameScreenManager.Instance == (UnityEngine.Object) null)
      return;
    DebugHandler.HideUI = DebugHandler.TimelapseMode || DebugHandler.ScreenshotMode;
    float num = DebugHandler.HideUI ? 0.0f : 1f;
    GameScreenManager.Instance.ssHoverTextCanvas.GetComponent<CanvasGroup>().alpha = num;
    GameScreenManager.Instance.ssCameraCanvas.GetComponent<CanvasGroup>().alpha = num;
    GameScreenManager.Instance.ssOverlayCanvas.GetComponent<CanvasGroup>().alpha = num;
    GameScreenManager.Instance.worldSpaceCanvas.GetComponent<CanvasGroup>().alpha = num;
    GameScreenManager.Instance.screenshotModeCanvas.GetComponent<CanvasGroup>().alpha = 1f - num;
  }

  public enum PaintMode
  {
    None,
    Element,
    Hot,
    Cold,
  }
}
