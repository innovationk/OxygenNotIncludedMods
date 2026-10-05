// Decompiled with JetBrains decompiler
// Type: FocusTargetSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public static class FocusTargetSequence
{
  private static Coroutine sequenceCoroutine = (Coroutine) null;
  private static KSelectable prevSelected = (KSelectable) null;
  private static bool wasPaused = false;
  private static int prevSpeed = -1;

  public static void Start(MonoBehaviour coroutineRunner, FocusTargetSequence.Data sequenceData)
  {
    FocusTargetSequence.sequenceCoroutine = coroutineRunner.StartCoroutine(FocusTargetSequence.RunSequence(sequenceData));
  }

  public static void Cancel(MonoBehaviour coroutineRunner)
  {
    if (FocusTargetSequence.sequenceCoroutine == null)
      return;
    coroutineRunner.StopCoroutine(FocusTargetSequence.sequenceCoroutine);
    FocusTargetSequence.sequenceCoroutine = (Coroutine) null;
    if (FocusTargetSequence.prevSpeed >= 0)
      SpeedControlScreen.Instance.SetSpeed(FocusTargetSequence.prevSpeed);
    if (SpeedControlScreen.Instance.IsPaused && !FocusTargetSequence.wasPaused)
      SpeedControlScreen.Instance.Unpause(false);
    if (!SpeedControlScreen.Instance.IsPaused && FocusTargetSequence.wasPaused)
      SpeedControlScreen.Instance.Pause(false);
    FocusTargetSequence.SetUIVisible(true);
    CameraController.Instance.SetWorldInteractive(true);
    SelectTool.Instance.Select(FocusTargetSequence.prevSelected, true);
    FocusTargetSequence.prevSelected = (KSelectable) null;
    FocusTargetSequence.wasPaused = false;
    FocusTargetSequence.prevSpeed = -1;
  }

  public static IEnumerator RunSequence(FocusTargetSequence.Data sequenceData)
  {
    SaveGame.Instance.GetComponent<UserNavigation>();
    CameraController.Instance.FadeOut();
    FocusTargetSequence.prevSpeed = SpeedControlScreen.Instance.GetSpeed();
    SpeedControlScreen.Instance.SetSpeed(0);
    FocusTargetSequence.wasPaused = SpeedControlScreen.Instance.IsPaused;
    if (!FocusTargetSequence.wasPaused)
      SpeedControlScreen.Instance.Pause(false);
    PlayerController.Instance.CancelDragging();
    CameraController.Instance.SetWorldInteractive(false);
    yield return (object) CameraController.Instance.activeFadeRoutine;
    FocusTargetSequence.prevSelected = SelectTool.Instance.selected;
    SelectTool.Instance.Select((KSelectable) null, true);
    FocusTargetSequence.SetUIVisible(false);
    ClusterManager.Instance.SetActiveWorld(sequenceData.WorldId);
    ManagementMenu.Instance.CloseAll();
    CameraController.Instance.SnapTo(sequenceData.Target, sequenceData.OrthographicSize);
    if (sequenceData.PopupData != null)
      EventInfoScreen.ShowPopup(sequenceData.PopupData);
    CameraController.Instance.FadeIn(speed: 2f);
    if ((double) sequenceData.TargetSize - (double) sequenceData.OrthographicSize > (double) Mathf.Epsilon)
      CameraController.Instance.StartCoroutine(CameraController.Instance.DoCinematicZoom(sequenceData.TargetSize));
    if (sequenceData.CanCompleteCB != null)
    {
      SpeedControlScreen.Instance.Unpause(false);
      while (!sequenceData.CanCompleteCB())
        yield return (object) SequenceUtil.WaitForNextFrame;
      SpeedControlScreen.Instance.Pause(false);
    }
    CameraController.Instance.SetWorldInteractive(true);
    SpeedControlScreen.Instance.SetSpeed(FocusTargetSequence.prevSpeed);
    if (SpeedControlScreen.Instance.IsPaused && !FocusTargetSequence.wasPaused)
      SpeedControlScreen.Instance.Unpause(false);
    if (sequenceData.CompleteCB != null)
      sequenceData.CompleteCB();
    FocusTargetSequence.SetUIVisible(true);
    SelectTool.Instance.Select(FocusTargetSequence.prevSelected, true);
    sequenceData.Clear();
    FocusTargetSequence.sequenceCoroutine = (Coroutine) null;
    FocusTargetSequence.prevSpeed = -1;
    FocusTargetSequence.wasPaused = false;
    FocusTargetSequence.prevSelected = (KSelectable) null;
  }

  private static void SetUIVisible(bool visible)
  {
    NotificationScreen.Instance.Show(visible);
    OverlayMenu.Instance.Show(visible);
    ManagementMenu.Instance.Show(visible);
    ToolMenu.Instance.Show(visible);
    ToolMenu.Instance.PriorityScreen.Show(visible);
    PinnedResourcesPanel.Instance.Show(visible);
    TopLeftControlScreen.Instance.Show(visible);
    DateTime.Instance.Show(visible);
    BuildWatermark.Instance.Show(visible);
    BuildWatermark.Instance.Show(visible);
    ColonyDiagnosticScreen.Instance.Show(visible);
    RootMenu.Instance.Show(visible);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      PlanScreen.Instance.Show(visible);
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      BuildMenu.Instance.Show(visible);
    if (!((UnityEngine.Object) WorldSelector.Instance != (UnityEngine.Object) null))
      return;
    WorldSelector.Instance.Show(visible);
  }

  public struct Data
  {
    public int WorldId;
    public float OrthographicSize;
    public float TargetSize;
    public Vector3 Target;
    public EventInfoData PopupData;
    public System.Action CompleteCB;
    public Func<bool> CanCompleteCB;

    public void Clear()
    {
      this.PopupData = (EventInfoData) null;
      this.CompleteCB = (System.Action) null;
      this.CanCompleteCB = (Func<bool>) null;
    }
  }
}
