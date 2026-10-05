// Decompiled with JetBrains decompiler
// Type: LargeImpactorRevealSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public static class LargeImpactorRevealSequence
{
  private const string SongName = "Stinger_LargeImpactor_Reveal";

  public static Coroutine Start(
    KMonoBehaviour controller,
    LargeImpactorSequenceUIReticle reticle,
    WorldContainer world)
  {
    return controller.StartCoroutine(LargeImpactorRevealSequence.Sequence(controller, reticle, world));
  }

  private static IEnumerator Sequence(
    KMonoBehaviour controller,
    LargeImpactorSequenceUIReticle reticle,
    WorldContainer world)
  {
    LargeImpactorCrashStamp component = controller.GetComponent<LargeImpactorCrashStamp>();
    LargeImpactorVisualizer visualizer = controller.GetComponent<LargeImpactorVisualizer>();
    ParallaxBackgroundObject parallaxBackgroundObject = controller.GetComponent<ParallaxBackgroundObject>();
    float scaleMin = parallaxBackgroundObject.scaleMin;
    parallaxBackgroundObject.scaleMin = 0.0f;
    int cell = Grid.XYToCell(component.stampLocation.x, component.stampLocation.y);
    int alignedWithCellInWorld = Grid.FindMidSkyCellAlignedWithCellInWorld(cell, world.id);
    Vector3 templatePosition = Grid.CellToPos(cell);
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    CameraController.Instance.SetWorldInteractive(false);
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
    ManagementMenu.Instance.CloseAll();
    StoryMessageScreen.HideInterface(true);
    OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
    CameraController.Instance.SetOverrideZoomSpeed(0.6f);
    CameraController.Instance.SetTargetPos(Grid.CellToPos(alignedWithCellInWorld), 20f, false);
    yield return (object) null;
    RootMenu.Instance.canTogglePauseScreen = false;
    CameraController.Instance.DisableUserCameraControl = true;
    MusicManager.instance.PlaySong("Stinger_LargeImpactor_Reveal");
    do
    {
      yield return (object) 0;
      parallaxBackgroundObject.scaleMin += Time.unscaledDeltaTime * 0.04f;
    }
    while ((double) parallaxBackgroundObject.scaleMin < 0.25);
    bool reticleSequenceCompleted = false;
    bool reticlePhase1SequenceCompleted = false;
    reticle.Run((System.Action) (() => reticlePhase1SequenceCompleted = true), (System.Action) (() =>
    {
      reticleSequenceCompleted = true;
      reticle.Hide();
    }));
    yield return (object) new WaitUntil((Func<bool>) (() => reticlePhase1SequenceCompleted));
    yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
    float num = (float) (visualizer.RangeMax.x - visualizer.RangeMin.x) * 0.6f;
    CameraController.Instance.SetOverrideZoomSpeed(0.4f);
    CameraController.Instance.SetMaxOrthographicSize(num);
    CameraController.Instance.SetTargetPos(templatePosition, num, false);
    yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
    visualizer.Active = true;
    visualizer.SetFoldedState(false);
    visualizer.BeginEntryEffect(3.5f);
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Imperative_calculating"));
    yield return (object) new WaitUntil((Func<bool>) (() => reticleSequenceCompleted));
    MusicManager.instance.StopSong("Stinger_LargeImpactor_Reveal");
    StoryMessageScreen.HideInterface(false);
    RootMenu.Instance.canTogglePauseScreen = true;
    CameraController.Instance.DisableUserCameraControl = false;
    CameraController.Instance.SetOverrideZoomSpeed(1f);
    CameraController.Instance.SetMaxOrthographicSize(20f);
    CameraController.Instance.SetWorldInteractive(true);
    HoverTextScreen.Instance.Show();
    RootMenu.Instance.canTogglePauseScreen = true;
    CameraController.Instance.SetTargetPos(templatePosition, 20f, true);
    controller.Trigger(-467702038, (object) null);
    if (SpeedControlScreen.Instance.IsPaused)
    {
      SpeedControlScreen.Instance.Unpause(false);
      SpeedControlScreen.Instance.SetSpeed(0);
    }
    parallaxBackgroundObject.scaleMin = scaleMin;
  }
}
