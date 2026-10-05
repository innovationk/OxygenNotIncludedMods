// Decompiled with JetBrains decompiler
// Type: LargeImpactorDestroyedSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public static class LargeImpactorDestroyedSequence
{
  private const string SongName = "Music_Victory_02_NIS";
  private const string Sound_Destroyed_Victory_Start_Sequence = "Asteroid_destroyed_start";

  public static Coroutine Start()
  {
    GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) Db.Get().GameplayEvents.LargeImpactor.Id);
    if (gameplayEventInstance != null)
    {
      LargeImpactorEvent.StatesInstance smi = (LargeImpactorEvent.StatesInstance) gameplayEventInstance.smi;
      if (smi != null && (UnityEngine.Object) smi.impactorInstance != (UnityEngine.Object) null)
      {
        LargeImpactorCrashStamp component = smi.impactorInstance.GetComponent<LargeImpactorCrashStamp>();
        return component.StartCoroutine(LargeImpactorDestroyedSequence.Sequence((KMonoBehaviour) component, smi.eventInstance.worldId));
      }
    }
    return (Coroutine) null;
  }

  private static IEnumerator Sequence(KMonoBehaviour controller, int worldID)
  {
    yield return (object) null;
    WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
    ParallaxBackgroundObject parallaxBackgroundObj = controller.GetComponent<ParallaxBackgroundObject>();
    GameObject telepad = GameUtil.GetTelepad(worldID);
    int centredCell = 0;
    if ((UnityEngine.Object) telepad != (UnityEngine.Object) null)
    {
      centredCell = Grid.PosToCell(telepad);
    }
    else
    {
      Vector2 pos = (Vector2) world.WorldOffset * Grid.CellSizeInMeters;
      pos.x += (float) ((double) world.Width * (double) Grid.CellSizeInMeters * 0.5);
      pos.y += (float) ((double) world.Height * (double) Grid.CellSizeInMeters * 0.5);
      centredCell = Grid.PosToCell(pos);
    }
    int cell1 = Grid.XYToCell(Grid.CellToXY(centredCell).x, world.WorldOffset.y + world.Height);
    int cell2 = centredCell;
    int midSkyCell = Grid.InvalidCell;
    int cell3;
    for (cell3 = Grid.InvalidCell; cell3 == Grid.InvalidCell && Grid.CellToXY(cell2).y < world.WorldOffset.y + world.Height; cell2 = Grid.CellAbove(cell2))
    {
      if (Grid.IsCellBiomeSpaceBiome(cell2))
      {
        cell3 = cell2;
        break;
      }
    }
    midSkyCell = Grid.XYToCell(Grid.CellToXY(centredCell).x, (int) ((double) (Grid.CellToXY(cell1).y + Grid.CellToXY(cell3).y) * 0.5));
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    RootMenu.Instance.canTogglePauseScreen = false;
    CameraController.Instance.DisableUserCameraControl = true;
    CameraController.Instance.SetWorldInteractive(false);
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
    ManagementMenu.Instance.CloseAll();
    StoryMessageScreen.HideInterface(true);
    OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
    CameraController.Instance.SetOverrideZoomSpeed(0.6f);
    yield return (object) null;
    CameraController.Instance.FadeIn();
    AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
    MusicManager.instance.PlaySong("Music_Victory_02_NIS");
    KFMOD.PlayUISound(GlobalAssets.GetSound("Asteroid_destroyed_start"));
    CameraController.Instance.SetTargetPos(Grid.CellToPos(midSkyCell), 20f, false);
    yield return (object) SequenceUtil.WaitForSecondsRealtime(4f);
    parallaxBackgroundObj.PlayExplosion();
    yield return (object) SequenceUtil.WaitForSecondsRealtime(2.2f);
    TerrainBG.preventLargeImpactorFragmentsFromProgressing = false;
    bool fadeOutCompleted = false;
    CameraController.Instance.FadeOutColor(Color.white, 0.0f, callback: (System.Action) (() => fadeOutCompleted = true));
    yield return (object) new WaitUntil((Func<bool>) (() => fadeOutCompleted));
    MissileLauncher.Instance instance = (MissileLauncher.Instance) null;
    float num1 = float.MaxValue;
    Vector3 position1 = CameraController.Instance.overlayCamera.transform.position with
    {
      z = 0.0f
    };
    foreach (MissileLauncher.Instance missileLauncher in Components.MissileLaunchers)
    {
      if (missileLauncher != null && missileLauncher.GetMyWorldId() == worldID)
      {
        Vector3 position2 = missileLauncher.transform.position with
        {
          z = 0.0f
        };
        float magnitude = (position1 - position2).magnitude;
        if ((double) magnitude < (double) num1)
        {
          num1 = magnitude;
          instance = missileLauncher;
        }
      }
    }
    int cell4 = Grid.InvalidCell;
    int invalidCell = Grid.InvalidCell;
    int num2 = instance != null ? 1 : 0;
    int num3;
    if (num2 != 0)
    {
      num3 = Grid.PosToCell(instance.gameObject);
    }
    else
    {
      cell4 = Grid.XYToCell(Grid.CellToXY(centredCell).x, world.WorldOffset.y + world.Height);
      num3 = cell4;
    }
    if (num2 != 0)
    {
      int cell5 = num3;
      int num4;
      for (int y = CameraController.Instance.VisibleArea.CurrentArea.Max.Y; Grid.CellToXY(cell5).y < y; cell5 = num4)
      {
        num4 = Grid.CellAbove(cell5);
        if (!Grid.IsValidCellInWorld(num4, worldID) || Grid.Solid[num4])
          break;
      }
      cell4 = cell5;
    }
    LargeImpactorDestroyedSequence.SpawnKeepsake(Grid.CellToPos(cell4));
    yield return (object) SequenceUtil.WaitForSecondsRealtime(2f);
    MusicManager.instance.StopSong("Music_Victory_02_NIS");
    AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
    yield return (object) null;
    bool videoCompleted = false;
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
    VideoScreen screen = (VideoScreen) null;
    if (!SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Pause(false);
    screen = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
    screen.PlayShortWithVictoryLoop(Db.Get().ColonyAchievements.AsteroidDestroyed.shortVideoName, Db.Get().ColonyAchievements.AsteroidDestroyed.messageBody, Db.Get().ColonyAchievements.AsteroidDestroyed.Id, Db.Get().ColonyAchievements.AsteroidDestroyed.loopVideoName, overrideAudioSnapshot: AudioMixerSnapshots.Get().VictoryNISGenericSnapshot);
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
    Game.Instance.Subscribe(-821118536, new Action<object>(LargeImpactorDestroyedSequence.OnScreenClosed));
    controller.Trigger(-467702038, (object) null);
  }

  private static void OnScreenClosed(object screenData)
  {
    if (screenData == null || !(screenData is RetiredColonyInfoScreen))
      return;
    LargeImpactorDestroyedSequence.OnAchievementScreenClosed();
  }

  private static void OnAchievementScreenClosed()
  {
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null && SpeedControlScreen.Instance.IsPaused)
    {
      SpeedControlScreen.Instance.Unpause(false);
      SpeedControlScreen.Instance.SetSpeed(0);
    }
    Game.Instance.Unsubscribe(-821118536, new Action<object>(LargeImpactorDestroyedSequence.OnScreenClosed));
  }

  private static void SpawnKeepsake(Vector3 position)
  {
    GameObject prefab = Assets.GetPrefab((Tag) "keepsake_largeimpactor");
    if (!((UnityEngine.Object) prefab != (UnityEngine.Object) null))
      return;
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
    GameObject gameObject = Util.KInstantiate(prefab, position);
    gameObject.SetActive(true);
    new UpgradeFX.Instance((IStateMachineTarget) gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0.0f, -0.5f, -0.1f)).StartSM();
  }
}
