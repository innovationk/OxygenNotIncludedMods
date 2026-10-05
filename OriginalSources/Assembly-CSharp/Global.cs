// Decompiled with JetBrains decompiler
// Type: Global
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KMod;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.U2D;

#nullable disable
public class Global : MonoBehaviour
{
  public SpriteAtlas[] forcedAtlasInitializationList;
  public GameObject modErrorsPrefab;
  public GameObject globalCanvas;
  private static GameInputManager mInputManager;
  private DevToolManager DevTools = new DevToolManager();
  public KMod.Manager modManager;
  private bool gotKleiUserID;
  private static string saveFolderTestResult = "unknown";
  private bool updated_with_initialized_distribution_platform;
  public static readonly string LanguageModKey = "LanguageMod";
  public static readonly string LanguageCodeKey = "LanguageCode";

  public static Global Instance { get; private set; }

  public static BindingEntry[] GenerateDefaultBindings(bool hotKeyBuildMenuPermitted = true)
  {
    List<BindingEntry> bindings = new List<BindingEntry>()
    {
      new BindingEntry((string) null, GamepadButton.Start, KKeyCode.Escape, Modifier.None, Action.Escape, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.W, Modifier.None, Action.PanUp),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.S, Modifier.None, Action.PanDown),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.A, Modifier.None, Action.PanLeft),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.D, Modifier.None, Action.PanRight),
      new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.O, Modifier.None, Action.RotateBuilding),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.L, Modifier.None, Action.ManagePriorities),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.F, Modifier.None, Action.ManageConsumables),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.V, Modifier.None, Action.ManageVitals),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.R, Modifier.None, Action.ManageResearch),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.E, Modifier.None, Action.ManageReport),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.U, Modifier.None, Action.ManageDatabase),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.J, Modifier.None, Action.ManageSkills),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.Period, Modifier.None, Action.ManageSchedule),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.Z, Modifier.None, Action.ManageStarmap),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.G, Modifier.None, Action.Dig),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.M, Modifier.None, Action.Mop),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.K, Modifier.None, Action.Clear),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.I, Modifier.None, Action.Disinfect),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.T, Modifier.None, Action.Attack),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.N, Modifier.None, Action.Capture),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Y, Modifier.None, Action.Harvest),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Insert, Modifier.None, Action.EmptyPipe),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.D, Modifier.Shift, Action.Disconnect),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.P, Modifier.None, Action.Prioritize),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.S, Modifier.Alt, Action.ToggleScreenshotMode),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.C, Modifier.None, Action.BuildingCancel),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.X, Modifier.None, Action.BuildingDeconstruct),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Tab, Modifier.None, Action.CycleSpeed),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.H, Modifier.None, Action.CameraHome),
      new BindingEntry("Root", GamepadButton.A, KKeyCode.Mouse0, Modifier.None, Action.MouseLeft, false),
      new BindingEntry("Root", GamepadButton.A, KKeyCode.Mouse0, Modifier.Shift, Action.ShiftMouseLeft, false),
      new BindingEntry("Root", GamepadButton.B, KKeyCode.Mouse1, Modifier.None, Action.MouseRight, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Mouse2, Modifier.None, Action.MouseMiddle, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.None, Action.Plan1),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.None, Action.Plan2),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.None, Action.Plan3),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.None, Action.Plan4),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.None, Action.Plan5),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha6, Modifier.None, Action.Plan6),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha7, Modifier.None, Action.Plan7),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha8, Modifier.None, Action.Plan8),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.None, Action.Plan9),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.None, Action.Plan10),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Minus, Modifier.None, Action.Plan11),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Equals, Modifier.None, Action.Plan12),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Minus, Modifier.Shift, Action.Plan13),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Equals, Modifier.Shift, Action.Plan14),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Backspace, Modifier.Shift, Action.Plan15),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.B, Modifier.None, Action.CopyBuilding),
      new BindingEntry("Root", GamepadButton.RT, KKeyCode.MouseScrollUp, Modifier.None, Action.ZoomIn),
      new BindingEntry("Root", GamepadButton.LT, KKeyCode.MouseScrollDown, Modifier.None, Action.ZoomOut),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F1, Modifier.None, Action.Overlay1),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F2, Modifier.None, Action.Overlay2),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F3, Modifier.None, Action.Overlay3),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F4, Modifier.None, Action.Overlay4),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F5, Modifier.None, Action.Overlay5),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F6, Modifier.None, Action.Overlay6),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F7, Modifier.None, Action.Overlay7),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F8, Modifier.None, Action.Overlay8),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F9, Modifier.None, Action.Overlay9),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F10, Modifier.None, Action.Overlay10),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F11, Modifier.None, Action.Overlay11),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F1, Modifier.Shift, Action.Overlay12),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F2, Modifier.Shift, Action.Overlay13),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Shift, Action.Overlay14),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F4, Modifier.Shift, Action.Overlay15, DlcManager.EXPANSION1),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.KeypadPlus, Modifier.None, Action.SpeedUp),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.KeypadMinus, Modifier.None, Action.SlowDown),
      new BindingEntry("Root", GamepadButton.Back, KKeyCode.Space, Modifier.None, Action.TogglePause),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F, Modifier.Ctrl, Action.Find),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.Ctrl, Action.SetUserNav1),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.Ctrl, Action.SetUserNav2),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.Ctrl, Action.SetUserNav3),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.Ctrl, Action.SetUserNav4),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.Ctrl, Action.SetUserNav5),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha6, Modifier.Ctrl, Action.SetUserNav6),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha7, Modifier.Ctrl, Action.SetUserNav7),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha8, Modifier.Ctrl, Action.SetUserNav8),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.Ctrl, Action.SetUserNav9),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.Ctrl, Action.SetUserNav10),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.Shift, Action.GotoUserNav1),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.Shift, Action.GotoUserNav2),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.Shift, Action.GotoUserNav3),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.Shift, Action.GotoUserNav4),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.Shift, Action.GotoUserNav5),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha6, Modifier.Shift, Action.GotoUserNav6),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha7, Modifier.Shift, Action.GotoUserNav7),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha8, Modifier.Shift, Action.GotoUserNav8),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.Shift, Action.GotoUserNav9),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.Shift, Action.GotoUserNav10),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.C, Modifier.None, Action.CinemaCamEnable, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.A, Modifier.None, Action.CinemaPanLeft, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.D, Modifier.None, Action.CinemaPanRight, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.W, Modifier.None, Action.CinemaPanUp, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.S, Modifier.None, Action.CinemaPanDown, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.I, Modifier.None, Action.CinemaZoomIn, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.O, Modifier.None, Action.CinemaZoomOut, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.Z, Modifier.None, Action.CinemaZoomSpeedPlus, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.Z, Modifier.Shift, Action.CinemaZoomSpeedMinus, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.P, Modifier.None, Action.CinemaUnpauseOnMove, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.T, Modifier.None, Action.CinemaToggleLock, ignore_root_conflicts: true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.E, Modifier.None, Action.CinemaToggleEasing, ignore_root_conflicts: true),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.Slash, Modifier.None, Action.ToggleOpen),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.Return, Modifier.None, Action.ToggleEnabled),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.Backslash, Modifier.None, Action.BuildingUtility1),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.LeftBracket, Modifier.None, Action.BuildingUtility2),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.RightBracket, Modifier.None, Action.BuildingUtility3),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.LeftAlt, Modifier.Alt, Action.AlternateView),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.RightAlt, Modifier.Alt, Action.AlternateView),
      new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.LeftShift, Modifier.Shift, Action.DragStraight),
      new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.RightShift, Modifier.Shift, Action.DragStraight),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.T, Modifier.Ctrl, Action.DebugFocus),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.U, Modifier.Ctrl, Action.DebugUltraTestMode),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F1, Modifier.Alt, Action.DebugToggleUI),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Alt, Action.DebugCollectGarbage),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F7, Modifier.Alt, Action.DebugInvincible),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Alt, Action.DebugForceLightEverywhere),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Shift, Action.DebugElementTest),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F12, Modifier.Shift, Action.DebugTileTest),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.N, Modifier.Alt, Action.DebugRefreshNavCell),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Q, Modifier.Ctrl, Action.DebugGotoTarget),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.S, Modifier.Ctrl, Action.DebugSelectMaterial),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.M, Modifier.Ctrl, Action.DebugToggleMusic),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F, Modifier.Alt, Action.DebugToggleClusterFX, DlcManager.EXPANSION1),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Backspace, Modifier.None, Action.DebugToggle),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Backspace, Modifier.Ctrl, Action.DebugToggleFastWorkers),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Q, Modifier.Alt, Action.DebugTeleport),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F2, Modifier.Alt, Action.DebugSpawnMinionAtmoSuit),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F2, Modifier.Ctrl, Action.DebugSpawnMinion),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Ctrl, Action.DebugPlace),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F4, Modifier.Ctrl, Action.DebugInstantBuildMode),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F5, Modifier.Ctrl, Action.DebugSlowTestMode),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F6, Modifier.Ctrl, Action.DebugDig),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F8, Modifier.Ctrl, Action.DebugExplosion),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F9, Modifier.Ctrl, Action.DebugDiscoverAllElements),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.T, Modifier.Alt, Action.DebugToggleSelectInEditor),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.P, Modifier.Alt, Action.DebugPathFinding),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.C, Modifier.Ctrl, Action.DebugCheerEmote),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Z, Modifier.Alt, Action.DebugSuperSpeed),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Equals, Modifier.Alt, Action.DebugGameStep),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Minus, Modifier.Alt, Action.DebugSimStep),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.X, Modifier.Alt, Action.DebugNotification),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.C, Modifier.Alt, Action.DebugNotificationMessage),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.BackQuote, Modifier.None, Action.ToggleProfiler),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.BackQuote, Modifier.Alt, Action.ToggleChromeProfiler),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F1, Modifier.Ctrl, Action.DebugDumpSceneParitionerLeakData),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F12, Modifier.Ctrl, Action.DebugTriggerException),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F12, Modifier.Ctrl | Modifier.Shift, Action.DebugTriggerError),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Ctrl, Action.DebugDumpGCRoots),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Alt | Modifier.Ctrl, Action.DebugDumpGarbageReferences),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F11, Modifier.Ctrl, Action.DebugDumpEventData),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F7, Modifier.Alt | Modifier.Ctrl, Action.DebugCrashSim),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.Alt, Action.DebugNextCall),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.Alt, Action.SreenShot1x),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.Alt, Action.SreenShot2x),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.Alt, Action.SreenShot8x),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.Alt, Action.SreenShot32x),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.Alt, Action.DebugLockCursor),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.Alt, Action.DebugTogglePersonalPriorityComparison),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Return, Modifier.None, Action.DialogSubmit, false),
      new BindingEntry("Analog", GamepadButton.NumButtons, KKeyCode.None, Modifier.None, Action.AnalogCamera, false),
      new BindingEntry("Analog", GamepadButton.NumButtons, KKeyCode.None, Modifier.None, Action.AnalogCursor, false),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.A, Modifier.None, Action.BuildMenuKeyA, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.B, Modifier.None, Action.BuildMenuKeyB, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.C, Modifier.None, Action.BuildMenuKeyC, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.D, Modifier.None, Action.BuildMenuKeyD, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.E, Modifier.None, Action.BuildMenuKeyE, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.F, Modifier.None, Action.BuildMenuKeyF, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.G, Modifier.None, Action.BuildMenuKeyG, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.H, Modifier.None, Action.BuildMenuKeyH, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.I, Modifier.None, Action.BuildMenuKeyI, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.J, Modifier.None, Action.BuildMenuKeyJ, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.K, Modifier.None, Action.BuildMenuKeyK, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.L, Modifier.None, Action.BuildMenuKeyL, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.M, Modifier.None, Action.BuildMenuKeyM, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.N, Modifier.None, Action.BuildMenuKeyN, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.O, Modifier.None, Action.BuildMenuKeyO, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.P, Modifier.None, Action.BuildMenuKeyP, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.Q, Modifier.None, Action.BuildMenuKeyQ, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.R, Modifier.None, Action.BuildMenuKeyR, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.S, Modifier.None, Action.BuildMenuKeyS, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.T, Modifier.None, Action.BuildMenuKeyT, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.U, Modifier.None, Action.BuildMenuKeyU, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.V, Modifier.None, Action.BuildMenuKeyV, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.W, Modifier.None, Action.BuildMenuKeyW, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.X, Modifier.None, Action.BuildMenuKeyX, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.Y, Modifier.None, Action.BuildMenuKeyY, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.Z, Modifier.None, Action.BuildMenuKeyZ, false, true),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.B, Modifier.Shift, Action.SandboxBrush),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.N, Modifier.Shift, Action.SandboxSprinkle),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.F, Modifier.Shift, Action.SandboxFlood),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.K, Modifier.Shift, Action.SandboxSample),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.H, Modifier.Shift, Action.SandboxHeatGun),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.J, Modifier.Shift, Action.SandboxStressTool),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.C, Modifier.Shift, Action.SandboxClearFloor),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.X, Modifier.Shift, Action.SandboxDestroy),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.E, Modifier.Shift, Action.SandboxSpawnEntity),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.S, Modifier.Shift, Action.ToggleSandboxTools),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.R, Modifier.Shift, Action.SandboxReveal),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.Z, Modifier.Shift, Action.SandboxCritterTool),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.T, Modifier.Shift, Action.SandboxStoryTraitTool),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.Mouse0, Modifier.Ctrl, Action.SandboxCopyElement),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.Backtick, Action.SwitchActiveWorld1, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.Backtick, Action.SwitchActiveWorld2, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.Backtick, Action.SwitchActiveWorld3, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.Backtick, Action.SwitchActiveWorld4, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.Backtick, Action.SwitchActiveWorld5, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha6, Modifier.Backtick, Action.SwitchActiveWorld6, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha7, Modifier.Backtick, Action.SwitchActiveWorld7, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha8, Modifier.Backtick, Action.SwitchActiveWorld8, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.Backtick, Action.SwitchActiveWorld9, true, false, DlcManager.EXPANSION1),
      new BindingEntry("SwitchActiveWorld", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.Backtick, Action.SwitchActiveWorld10, true, false, DlcManager.EXPANSION1)
    };
    IList<BuildMenu.DisplayInfo> data = (IList<BuildMenu.DisplayInfo>) BuildMenu.OrderedBuildings.data;
    if (BuildMenu.UseHotkeyBuildMenu() & hotKeyBuildMenuPermitted)
    {
      foreach (BuildMenu.DisplayInfo display_info in (IEnumerable<BuildMenu.DisplayInfo>) data)
        Global.AddBindings(HashedString.Invalid, display_info, bindings);
    }
    return bindings.ToArray();
  }

  private static void AddBindings(
    HashedString parent_category,
    BuildMenu.DisplayInfo display_info,
    List<BindingEntry> bindings)
  {
    if (display_info.data == null)
      return;
    System.Type type = display_info.data.GetType();
    if (typeof (IList<BuildMenu.DisplayInfo>).IsAssignableFrom(type))
    {
      foreach (BuildMenu.DisplayInfo display_info1 in (IEnumerable<BuildMenu.DisplayInfo>) display_info.data)
        Global.AddBindings(display_info.category, display_info1, bindings);
    }
    else
    {
      if (!typeof (IList<BuildMenu.BuildingInfo>).IsAssignableFrom(type))
        return;
      BindingEntry bindingEntry = new BindingEntry(new CultureInfo("en-US", false).TextInfo.ToTitleCase(HashCache.Get().Get(parent_category)) + " Menu", GamepadButton.NumButtons, display_info.keyCode, Modifier.None, display_info.hotkey, ignore_root_conflicts: true);
      bindings.Add(bindingEntry);
    }
  }

  private void Awake()
  {
    KCrashReporter crash_reporter = this.GetComponent<KCrashReporter>();
    if ((UnityEngine.Object) crash_reporter != (UnityEngine.Object) null & SceneInitializerLoader.ReportDeferredError == null)
      SceneInitializerLoader.ReportDeferredError = (SceneInitializerLoader.DeferredErrorDelegate) (deferred_error => crash_reporter.ShowDialog(deferred_error.msg, deferred_error.stack_trace));
    this.globalCanvas = GameObject.Find("Canvas");
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.globalCanvas.gameObject);
    this.OutputSystemInfo();
    Debug.Assert((UnityEngine.Object) Global.Instance == (UnityEngine.Object) null);
    Global.Instance = this;
    Debug.Log((object) ("Initializing at " + System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")));
    Debug.Log((object) ("Save path: " + Util.RootFolder()));
    MyCmp.Init();
    MySmi.Init();
    DevToolManager.Instance.Init();
    if (this.forcedAtlasInitializationList != null)
    {
      foreach (SpriteAtlas atlasInitialization in this.forcedAtlasInitializationList)
      {
        Sprite[] sprites = new Sprite[atlasInitialization.spriteCount];
        atlasInitialization.GetSprites(sprites);
        foreach (Sprite sprite in sprites)
        {
          Texture2D texture = sprite.texture;
          if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
          {
            texture.filterMode = FilterMode.Bilinear;
            texture.anisoLevel = 4;
            texture.mipMapBias = 0.0f;
          }
        }
      }
    }
    FileSystem.Initialize();
    Singleton<StateMachineUpdater>.CreateInstance();
    Singleton<StateMachineManager>.CreateInstance();
    Localization.RegisterForTranslation(typeof (UI));
    this.modManager = new KMod.Manager();
    this.modManager.LoadModDBAndInitialize();
    this.modManager.SetModLoadingInProgress(true);
    this.modManager.Load(Content.DLL);
    this.modManager.Load(Content.Strings);
    KSerialization.Manager.Initialize();
    Global.InitializeGlobalInput();
    Global.InitializeGlobalSound();
    Global.InitializeGlobalAnimation();
    Localization.Initialize();
    this.modManager.Load(Content.Translation);
    this.modManager.distribution_platforms.Add((IDistributionPlatform) new Local("Local", KMod.Label.DistributionPlatform.Local, false));
    this.modManager.distribution_platforms.Add((IDistributionPlatform) new Local("Dev", KMod.Label.DistributionPlatform.Dev, true));
    TuningSystem.main_thread = Thread.CurrentThread;
    this.RestoreLegacyMetricsSetting();
    this.TestDataLocations();
    DistributionPlatform.onExitRequest += new System.Action(this.OnExitRequest);
    DistributionPlatform.onDlcAuthenticationFailed += new System.Action(this.OnDlcAuthenticationFailed);
    if (DistributionPlatform.Initialized)
    {
      if (!KPrivacyPrefs.instance.disableDataCollection)
      {
        Debug.Log((object) $"Logged into {DistributionPlatform.Inst.Name} with ID:{DistributionPlatform.Inst.LocalUser.Id?.ToString()}, NAME:{DistributionPlatform.Inst.LocalUser.Name}");
        ThreadedHttps<KleiAccount>.Instance.AuthenticateUser(new KleiAccount.GetUserIDdelegate(this.OnGetUserIdKey));
      }
      else
        Debug.Log((object) "Data collection disabled, account will not be used.");
    }
    else
    {
      Debug.LogWarning((object) $"Can't init {DistributionPlatform.Inst.Name} distribution platform...");
      this.OnGetUserIdKey();
    }
    ThreadedHttps<KleiItems>.Instance.LoadInventoryCache();
    this.modManager.Load(Content.LayerableFiles);
    WorldGen.LoadSettings(true);
    this.StartCoroutine(WorldGen.ListenForLoadSettingsErrorRoutine());
    GlobalResources.Instance();
  }

  private static void InitializeGlobalInput()
  {
    if (Game.IsQuitting())
      return;
    Global.mInputManager = new GameInputManager(Global.GenerateDefaultBindings());
  }

  private static void InitializeGlobalSound()
  {
    Audio.Get();
    Singleton<SoundEventVolumeCache>.CreateInstance();
  }

  private static void InitializeGlobalAnimation()
  {
    KAnimBatchManager.CreateInstance();
    Singleton<AnimEventManager>.CreateInstance();
    Singleton<KBatchedAnimUpdater>.CreateInstance();
  }

  private void OnExitRequest()
  {
    bool flag = true;
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
    {
      string filename = SaveLoader.GetActiveSaveFilePath();
      if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
      {
        flag = false;
        KScreen component = KScreenManager.AddChild(this.globalCanvas, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
        component.Activate();
        component.GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format((string) UI.FRONTEND.RAILFORCEQUIT.SAVE_EXIT, (object) System.IO.Path.GetFileNameWithoutExtension(filename)), (System.Action) (() =>
        {
          SaveLoader.Instance.Save(filename);
          App.Quit();
        }), (System.Action) (() => App.Quit()));
      }
    }
    if (!flag)
      return;
    KScreen component1 = KScreenManager.AddChild(this.globalCanvas, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
    component1.Activate();
    component1.GetComponent<ConfirmDialogScreen>().PopupConfirmDialog((string) UI.FRONTEND.RAILFORCEQUIT.WARN_EXIT, (System.Action) (() => App.Quit()), (System.Action) null);
  }

  private void OnDlcAuthenticationFailed()
  {
    if (!DlcManager.IsExpansion1Active())
      return;
    KScreen component1 = KScreenManager.AddChild(this.globalCanvas, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
    component1.Activate();
    ConfirmDialogScreen component2 = component1.GetComponent<ConfirmDialogScreen>();
    component2.deactivateOnCancelAction = false;
    component2.PopupConfirmDialog((string) UI.FRONTEND.RAILFORCEQUIT.DLC_NOT_PURCHASED, (System.Action) (() => App.Quit()), (System.Action) null);
  }

  private void RestoreLegacyMetricsSetting()
  {
    if (KPlayerPrefs.GetInt("ENABLE_METRICS", 1) != 0)
      return;
    KPlayerPrefs.DeleteKey("ENABLE_METRICS");
    KPlayerPrefs.Save();
    KPrivacyPrefs.instance.disableDataCollection = true;
    KPrivacyPrefs.Save();
  }

  private void TestDataLocations()
  {
    if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
      return;
    try
    {
      string str1 = Util.RootFolder();
      string str2 = System.IO.Path.Combine(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Klei"), Util.GetTitleFolderName());
      Debug.Log((object) ("Test Data Location / docs / " + str1));
      Debug.Log((object) ("Test Data Location / local / " + str2));
      if (!System.IO.Directory.Exists(str2))
        System.IO.Directory.CreateDirectory(str2);
      if (!System.IO.Directory.Exists(str1))
        System.IO.Directory.CreateDirectory(str1);
      string[] strArray = new string[2]
      {
        System.IO.Path.Combine(str1, "test"),
        System.IO.Path.Combine(str2, "test")
      };
      bool[] flagArray1 = new bool[2];
      bool[] flagArray2 = new bool[2];
      bool[] flagArray3 = new bool[2];
      for (int index = 0; index < strArray.Length; ++index)
      {
        try
        {
          using (FileStream fileStream = File.Open(strArray[index], FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
          {
            byte[] bytes = Encoding.UTF8.GetBytes("test");
            fileStream.Write(bytes, 0, bytes.Length);
            flagArray1[index] = true;
          }
        }
        catch (Exception ex)
        {
          flagArray1[index] = false;
          DebugUtil.LogWarningArgs((object) $"Test Data Locations / failed to write {strArray[index]}: {ex.Message}");
        }
        try
        {
          using (FileStream fileStream = File.Open(strArray[index], FileMode.Open, FileAccess.Read))
          {
            Encoding utF8 = Encoding.UTF8;
            byte[] numArray = new byte[fileStream.Length];
            if ((long) fileStream.Read(numArray, 0, numArray.Length) == fileStream.Length)
            {
              string str3 = utF8.GetString(numArray);
              if (str3 == "test")
              {
                flagArray2[index] = true;
              }
              else
              {
                flagArray2[index] = false;
                DebugUtil.LogWarningArgs((object) $"Test Data Locations / failed to validate contents {strArray[index]}, got: `{str3}`");
              }
            }
          }
        }
        catch (Exception ex)
        {
          flagArray2[index] = false;
          DebugUtil.LogWarningArgs((object) $"Test Data Locations / failed to read {strArray[index]}: {ex.Message}");
        }
        try
        {
          File.Delete(strArray[index]);
          flagArray3[index] = true;
        }
        catch (Exception ex)
        {
          flagArray3[index] = false;
          DebugUtil.LogWarningArgs((object) $"Test Data Locations / failed to remove {strArray[index]}: {ex.Message}");
        }
      }
      for (int index = 0; index < strArray.Length; ++index)
        Debug.Log((object) $"Test Data Locations / {strArray[index]} / write {flagArray1[index].ToString()} / read {flagArray2[index].ToString()} / removed {flagArray3[index].ToString()}");
      bool flag1 = flagArray1[0] && flagArray2[0];
      bool flag2 = flagArray1[1] && flagArray2[1];
      if (flag1 & flag2)
        Global.saveFolderTestResult = "both";
      else if (flag1 && !flag2)
        Global.saveFolderTestResult = "docs_only";
      else if (!flag1 & flag2)
        Global.saveFolderTestResult = "local_only";
      else
        Global.saveFolderTestResult = "neither";
    }
    catch (Exception ex)
    {
      KCrashReporter.Assert(false, "Test Data Locations / failed: " + ex.Message, new string[1]
      {
        KCrashReporter.CRASH_CATEGORY.FILEIO
      });
    }
  }

  public static GameInputManager GetInputManager()
  {
    if (Global.mInputManager == null)
      Global.InitializeGlobalInput();
    return Global.mInputManager;
  }

  private void OnApplicationFocus(bool focus)
  {
    if (Global.mInputManager == null)
      return;
    Global.mInputManager.OnApplicationFocus(focus);
  }

  private void OnGetUserIdKey() => this.gotKleiUserID = true;

  private void Update()
  {
    ImGuiRenderer instance = ImGuiRenderer.GetInstance();
    if ((bool) (UnityEngine.Object) instance)
    {
      this.DevTools.UpdateShouldShowTools();
      instance.gameObject.transform.parent.gameObject.SetActive(this.DevTools.Show);
      if (this.DevTools.Show)
        instance.NewFrame();
      this.DevTools.UpdateTools();
    }
    Global.mInputManager.Update();
    if (Singleton<AnimEventManager>.Instance != null)
      Singleton<AnimEventManager>.Instance.Update();
    if (DistributionPlatform.Initialized && !this.updated_with_initialized_distribution_platform)
    {
      this.updated_with_initialized_distribution_platform = true;
      SteamUGCService.Initialize();
      Steam steam = new Steam();
      SteamUGCService.Instance.AddClient((SteamUGCService.IClient) steam);
      this.modManager.distribution_platforms.Add((IDistributionPlatform) steam);
      SteamAchievementService.Initialize();
    }
    if (this.gotKleiUserID)
    {
      this.gotKleiUserID = false;
      ThreadedHttps<KleiMetrics>.Instance.SetCallBacks(new System.Action(this.SetONIStaticSessionVariables), new Action<Dictionary<string, object>>(this.SetONIDynamicSessionVariables));
      ThreadedHttps<KleiMetrics>.Instance.StartSession();
      KleiItems.AddRequestInventoryRefresh();
      KleiItems.AddRequestGetPricingInfo();
    }
    ThreadedHttps<KleiMetrics>.Instance.SetLastUserAction(KInputManager.lastUserActionTicks);
    Localization.VerifyTranslationModSubscription(this.globalCanvas);
    if (!DistributionPlatform.Initialized)
      return;
    ThreadedHttps<KleiItems>.Instance.Update();
  }

  private void SetONIStaticSessionVariables()
  {
    ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable("Branch", (object) "release");
    ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable("Build", (object) 744825U);
    ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable("SaveFolderWriteTest", (object) Global.saveFolderTestResult);
    if (KPlayerPrefs.HasKey(UnitConfigurationScreen.MassUnitKey))
      ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(UnitConfigurationScreen.MassUnitKey, (object) ((GameUtil.MassUnit) KPlayerPrefs.GetInt(UnitConfigurationScreen.MassUnitKey)).ToString());
    if (KPlayerPrefs.HasKey(UnitConfigurationScreen.TemperatureUnitKey))
      ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(UnitConfigurationScreen.TemperatureUnitKey, (object) ((GameUtil.TemperatureUnit) KPlayerPrefs.GetInt(UnitConfigurationScreen.TemperatureUnitKey)).ToString());
    int selectedLanguageType = (int) Localization.GetSelectedLanguageType();
    ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(Global.LanguageCodeKey, (object) Localization.GetCurrentLanguageCode());
    if (selectedLanguageType != 2)
      return;
    ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(Global.LanguageModKey, (object) LanguageOptionsScreen.GetSavedLanguageMod());
  }

  private void SetONIDynamicSessionVariables(Dictionary<string, object> data)
  {
    if (!((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null))
      return;
    data.Add("GameTimeSeconds", (object) (uint) GameClock.Instance.GetTime());
    data.Add("CycleTime", (object) GameUtil.GetCurrentTimeInCycles());
    data.Add("WasDebugEverUsed", (object) Game.Instance.debugWasUsed);
    data.Add("IsSandboxEnabled", (object) SaveGame.Instance.sandboxEnabled);
    data.Add("ColonyGuid", (object) SaveLoader.Instance.GameInfo.colonyGuid.ToString());
  }

  private void LateUpdate()
  {
    StreamedTextures.UpdateRequests();
    Singleton<KBatchedAnimUpdater>.Instance.LateUpdate();
    if (!this.DevTools.Show)
      return;
    ImGuiRenderer.GetInstance()?.EndFrame();
  }

  private void OnDestroy()
  {
    if (this.modManager != null)
      this.modManager.Shutdown();
    Global.Instance = (Global) null;
    if (Singleton<AnimEventManager>.Instance != null)
      Singleton<AnimEventManager>.Instance.FreeResources();
    Singleton<KBatchedAnimUpdater>.DestroyInstance();
  }

  private void OnApplicationQuit()
  {
    KGlobalAnimParser.DestroyInstance();
    ThreadedHttps<KleiMetrics>.Instance.EndSession();
  }

  private void OutputSystemInfo()
  {
    try
    {
      Console.WriteLine("SYSTEM INFO:");
      foreach (KeyValuePair<string, object> hardwareStat in KleiMetrics.GetHardwareStats())
      {
        try
        {
          Console.WriteLine($"    {hardwareStat.Key.ToString()}={hardwareStat.Value.ToString()}");
        }
        catch
        {
        }
      }
      Console.WriteLine($"    {"System Language"}={Application.systemLanguage.ToString()}");
    }
    catch
    {
    }
  }
}
