// Decompiled with JetBrains decompiler
// Type: FindingMinnowCompleteSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public static class FindingMinnowCompleteSequence
{
  public static void Start(KMonoBehaviour controller)
  {
    controller.StartCoroutine(FindingMinnowCompleteSequence.Sequence());
  }

  private static IEnumerator Sequence()
  {
    bool videoCompleted = false;
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    VideoScreen screen = (VideoScreen) null;
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    CameraController.Instance.FadeOut();
    yield return (object) SequenceUtil.WaitForSecondsRealtime(2f);
    screen = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
    screen.PlayVictoryLoop(Db.Get().ColonyAchievements.MinnowRecruited.messageBody, Db.Get().ColonyAchievements.MinnowRecruited.Id, Db.Get().ColonyAchievements.MinnowRecruited.loopVideoName, overrideAudioSnapshot: AudioMixerSnapshots.Get().VictoryCinematicSnapshot, fadeIn: true);
    System.Action onVideoCompletedCallback = (System.Action) (() => videoCompleted = true);
    screen.OnStop += onVideoCompletedCallback;
    yield return (object) new WaitUntil((Func<bool>) (() => videoCompleted));
    screen.OnStop -= onVideoCompletedCallback;
    SpeedControlScreen.Instance.SetSpeed(0);
    CameraController.Instance.FadeIn();
    CameraController.Instance.SetOverrideZoomSpeed(1f);
    CameraController.Instance.SetWorldInteractive(true);
    CameraController.Instance.DisableUserCameraControl = false;
    CameraController.Instance.SetMaxOrthographicSize(20f);
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
    RootMenu.Instance.canTogglePauseScreen = true;
    HoverTextScreen.Instance.Show();
    StoryMessageScreen.HideInterface(false);
  }
}
