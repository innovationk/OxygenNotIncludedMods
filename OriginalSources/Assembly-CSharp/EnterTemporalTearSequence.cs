// Decompiled with JetBrains decompiler
// Type: EnterTemporalTearSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public static class EnterTemporalTearSequence
{
  public static GameObject tearOpenerGameObject;

  public static void Start(KMonoBehaviour controller)
  {
    controller.StartCoroutine(EnterTemporalTearSequence.Sequence());
  }

  private static IEnumerator Sequence()
  {
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    CameraController.Instance.SetWorldInteractive(false);
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
    CameraController.Instance.FadeOut();
    yield return (object) SequenceUtil.WaitForSecondsRealtime(3f);
    ManagementMenu.Instance.CloseAll();
    AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
    MusicManager.instance.PlaySong("Music_Victory_02_NIS");
    Vector3 cameraBiasUp = Vector3.up * 5f;
    GameObject cameraTaget = EnterTemporalTearSequence.tearOpenerGameObject;
    if ((UnityEngine.Object) cameraTaget != (UnityEngine.Object) null)
    {
      CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 10f, false);
      CameraController.Instance.SetOverrideZoomSpeed(10f);
      yield return (object) SequenceUtil.WaitForSecondsRealtime(0.4f);
      if (SpeedControlScreen.Instance.IsPaused)
        SpeedControlScreen.Instance.Unpause(false);
      SpeedControlScreen.Instance.SetSpeed(1);
      CameraController.Instance.SetOverrideZoomSpeed(0.1f);
      CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 20f, false);
      CameraController.Instance.FadeIn(speed: 2f);
      foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
      {
        if ((UnityEngine.Object) liveMinionIdentity != (UnityEngine.Object) null)
        {
          liveMinionIdentity.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
          Db db = Db.Get();
          EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) liveMinionIdentity.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2);
        }
      }
      yield return (object) SequenceUtil.WaitForSecondsRealtime(0.5f);
      yield return (object) SequenceUtil.WaitForSecondsRealtime(1.5f);
      CameraController.Instance.FadeOut();
      yield return (object) SequenceUtil.WaitForSecondsRealtime(1.5f);
    }
    cameraTaget = (GameObject) null;
    cameraTaget = (GameObject) null;
    foreach (Telepad telepad in Components.Telepads)
    {
      if ((UnityEngine.Object) telepad != (UnityEngine.Object) null)
      {
        cameraTaget = telepad.gameObject;
        CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 10f, false);
        CameraController.Instance.SetOverrideZoomSpeed(10f);
        yield return (object) SequenceUtil.WaitForSecondsRealtime(0.4f);
        if (SpeedControlScreen.Instance.IsPaused)
          SpeedControlScreen.Instance.Unpause(false);
        SpeedControlScreen.Instance.SetSpeed(1);
        CameraController.Instance.SetOverrideZoomSpeed(0.05f);
        CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 20f, false);
        CameraController.Instance.FadeIn(speed: 2f);
        foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
        {
          if ((UnityEngine.Object) liveMinionIdentity != (UnityEngine.Object) null)
          {
            liveMinionIdentity.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
            Db db = Db.Get();
            EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) liveMinionIdentity.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2);
          }
        }
        yield return (object) SequenceUtil.WaitForSecondsRealtime(0.5f);
        yield return (object) SequenceUtil.WaitForSecondsRealtime(1.5f);
        CameraController.Instance.FadeOut();
        yield return (object) SequenceUtil.WaitForSecondsRealtime(1.5f);
      }
    }
    cameraTaget = (GameObject) null;
    MusicManager.instance.StopSong("Music_Victory_02_NIS");
    AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
    yield return (object) SequenceUtil.WaitForSecondsRealtime(2f);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
    component.PlayShortWithVictoryLoop(Db.Get().ColonyAchievements.ReachedDistantPlanet.shortVideoName, Db.Get().ColonyAchievements.ReachedDistantPlanet.messageBody, Db.Get().ColonyAchievements.ReachedDistantPlanet.Id, Db.Get().ColonyAchievements.ReachedDistantPlanet.loopVideoName, overrideAudioSnapshot: AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    component.OnStop += (System.Action) (() =>
    {
      StoryMessageScreen.HideInterface(false);
      CameraController.Instance.FadeIn();
      CameraController.Instance.SetWorldInteractive(true);
      HoverTextScreen.Instance.Show();
      CameraController.Instance.SetOverrideZoomSpeed(1f);
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
      RootMenu.Instance.canTogglePauseScreen = true;
    });
  }
}
