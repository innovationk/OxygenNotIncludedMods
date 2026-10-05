// Decompiled with JetBrains decompiler
// Type: GeothermalVictorySequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public static class GeothermalVictorySequence
{
  public static GeothermalVent VictoryVent;

  public static void Start(KMonoBehaviour controller)
  {
    controller.StartCoroutine(GeothermalVictorySequence.Sequence());
  }

  private static IEnumerator Sequence()
  {
    if ((UnityEngine.Object) GeothermalVictorySequence.VictoryVent == (UnityEngine.Object) null)
    {
      StoryMessageScreen.HideInterface(false);
      CameraController.Instance.FadeIn();
      CameraController.Instance.SetWorldInteractive(true);
      CameraController.Instance.SetOverrideZoomSpeed(1f);
      CameraController.Instance.DisableUserCameraControl = false;
      RootMenu.Instance.canTogglePauseScreen = true;
    }
    else
    {
      if (!SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Pause(false);
      CameraController.Instance.SetWorldInteractive(false);
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
      AudioMixer.instance.Start(Db.Get().ColonyAchievements.ActivateGeothermalPlant.victoryNISSnapshot);
      MusicManager.instance.PlaySong("Music_Victory_02_NIS");
      Vector3 vector3 = Vector3.up * 5f;
      CameraController.Instance.FadeOut();
      yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
      Vector3 ventTargetPositon = GeothermalVictorySequence.VictoryVent.transform.position + Vector3.up * 3f;
      CameraController.Instance.SetTargetPos(ventTargetPositon, 10f, false);
      CameraController.Instance.SetOverrideZoomSpeed(10f);
      yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
      CameraController.Instance.FadeIn();
      if (SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Unpause(false);
      SpeedControlScreen.Instance.SetSpeed(0);
      CameraController.Instance.SetOverrideZoomSpeed(0.05f);
      CameraController.Instance.SetTargetPos(ventTargetPositon, 20f, false);
      GeothermalVictorySequence.VictoryVent.SpawnKeepsake();
      yield return (object) SequenceUtil.WaitForSecondsRealtime(4f);
      foreach (GeothermalVent.ElementInfo info in GeothermalControllerConfig.GetClearingEntombedVentReward())
        GeothermalVictorySequence.VictoryVent.addMaterial(info);
      yield return (object) SequenceUtil.WaitForSecondsRealtime(5f);
      CameraController.Instance.FadeOut();
      ventTargetPositon = new Vector3();
      yield return (object) SequenceUtil.WaitForSecondsRealtime(0.5f);
      MusicManager.instance.StopSong("Music_Victory_02_NIS");
      AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ActivateGeothermalPlant.victoryNISSnapshot);
      yield return (object) SequenceUtil.WaitForSecondsRealtime(2f);
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
      if (!SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Pause(false);
      VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
      component.PlayShortWithVictoryLoop(Db.Get().ColonyAchievements.ActivateGeothermalPlant.shortVideoName, Db.Get().ColonyAchievements.ActivateGeothermalPlant.messageBody, Db.Get().ColonyAchievements.ActivateGeothermalPlant.Id, Db.Get().ColonyAchievements.ActivateGeothermalPlant.loopVideoName, Db.Get().ColonyAchievements.ActivateGeothermalPlant.IsValidForSave(), AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
      component.OnStop += (System.Action) (() =>
      {
        StoryMessageScreen.HideInterface(false);
        CameraController.Instance.FadeIn();
        CameraController.Instance.SetWorldInteractive(true);
        CameraController.Instance.SetOverrideZoomSpeed(1f);
        HoverTextScreen.Instance.Show();
        AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
        AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
        RootMenu.Instance.canTogglePauseScreen = true;
        Game.Instance.unlocks.Unlock("notes_earthquake");
      });
      GeothermalVictorySequence.VictoryVent = (GeothermalVent) null;
    }
  }
}
