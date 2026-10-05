// Decompiled with JetBrains decompiler
// Type: LargeImpactorLandingSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class LargeImpactorLandingSequence
{
  private const string SongName = "Stinger_Demolior_Falling";
  private const string IncomingSFX = "Asteroid_incoming_LP";
  private const string ImpactSFX = "Asteroid_explode";

  public static Coroutine Start(
    KMonoBehaviour controller,
    LargeComet comet,
    LargeImpactorCrashStamp stamp,
    int worldID)
  {
    return controller.StartCoroutine(LargeImpactorLandingSequence.Sequence(controller, comet, stamp, worldID));
  }

  private static IEnumerator Sequence(
    KMonoBehaviour controller,
    LargeComet comet,
    LargeImpactorCrashStamp stamp,
    int worldID)
  {
    yield return (object) null;
    LargeImpactorVisualizer component = controller.GetComponent<LargeImpactorVisualizer>();
    Vector3 templatePosition = Grid.CellToPos(Grid.XYToCell(stamp.stampLocation.x, stamp.stampLocation.y));
    bool cometImpacted = false;
    comet.OnImpact += (System.Action) (() => cometImpacted = true);
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause(false);
    SpeedControlScreen.Instance.SetSpeed(0);
    RootMenu.Instance.canTogglePauseScreen = false;
    CameraController.Instance.DisableUserCameraControl = true;
    CameraController.Instance.SetWorldInteractive(false);
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
    ManagementMenu.Instance.CloseAll();
    StoryMessageScreen.HideInterface(true);
    OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
    CameraController.Instance.SetOverrideZoomSpeed(0.6f);
    float templateWidth = (float) (component.RangeMax.x - component.RangeMin.x);
    float initialOrthogonalSize = templateWidth * 0.72f;
    float finalOrthogonalSize = templateWidth * 0.62f;
    yield return (object) null;
    AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
    MusicManager.instance.PlaySong("Stinger_Demolior_Falling");
    EventInstance incomingSFXInstance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Asteroid_incoming_LP"), Vector3.zero);
    int num1 = (int) incomingSFXInstance.start();
    CameraController.Instance.SetMaxOrthographicSize(finalOrthogonalSize);
    CameraController.Instance.SetTargetPos(comet.transform.position, initialOrthogonalSize, false);
    yield return (object) new WaitUntil((Func<bool>) (() =>
    {
      float f = (UnityEngine.Object) comet == (UnityEngine.Object) null ? 1f : comet.LandingProgress;
      CameraController.Instance.SetTargetPos((UnityEngine.Object) comet == (UnityEngine.Object) null ? templatePosition : (Grid.IsValidCellInWorld(Grid.PosToCell(comet.VisualPosition), worldID) ? comet.VisualPosition : comet.transform.position), Mathf.Lerp(initialOrthogonalSize, finalOrthogonalSize, (double) f <= 0.0 ? 0.0f : Mathf.Pow(f, 2f)), false);
      return cometImpacted;
    }));
    int num2 = (int) incomingSFXInstance.stop(STOP_MODE.IMMEDIATE);
    int num3 = (int) incomingSFXInstance.release();
    KFMOD.PlayUISound(GlobalAssets.GetSound("Asteroid_explode"));
    CameraController.Instance.FadeOutColor(Color.white);
    bool templateSpawned = false;
    TemplateLoader.Stamp(stamp.asteroidTemplate, (Vector2) stamp.stampLocation, (System.Action) (() => templateSpawned = true));
    List<WorldGenSpawner.Spawnable> unspawnedGeysers = new List<WorldGenSpawner.Spawnable>();
    foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(worldID))
      unspawnedGeysers.Add(spawnable);
    yield return (object) null;
    foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag((Tag) "GeyserGeneric", worldID))
      unspawnedGeysers.Add(spawnable);
    yield return (object) null;
    yield return (object) SequenceUtil.WaitForSecondsRealtime(1.8f);
    yield return (object) new WaitUntil((Func<bool>) (() => templateSpawned));
    float num4 = templateWidth * 0.3f;
    CameraController.Instance.SetPosition(templatePosition);
    CameraController.Instance.OrthographicSize = num4;
    float orthographic_size = templateWidth * 0.68f;
    CameraController.Instance.SetOverrideZoomSpeed(0.1f);
    CameraController.Instance.SetTargetPos(templatePosition, orthographic_size, false);
    bool fadeOutCompleted = false;
    CameraController.Instance.FadeInColor(Color.white, callback: (System.Action) (() => fadeOutCompleted = true));
    yield return (object) new WaitUntil((Func<bool>) (() => fadeOutCompleted));
    yield return (object) SequenceUtil.WaitForSecondsRealtime(8f);
    MusicManager.instance.StopSong("Stinger_Demolior_Falling");
    AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
    StoryMessageScreen.HideInterface(false);
    foreach (WorldGenSpawner.Spawnable spawnable in unspawnedGeysers)
    {
      Vector2I xy = Grid.CellToXY(Grid.OffsetCell(spawnable.cell, 0, 2));
      GridVisibility.Reveal(xy.x, xy.y, 6, 1f);
      SimMessages.Dig(spawnable.cell);
    }
    yield return (object) null;
    List<Geyser> geysers = Components.Geysers.GetItems(worldID);
    geysers.Sort((Comparison<Geyser>) ((a, b) => (a.transform.position - templatePosition).magnitude.CompareTo((b.transform.position - templatePosition).magnitude)));
    float geyserRevealTimer = 0.0f;
    int geyserCount = geysers.Count;
    Action<int, int> MakeGeyserRangeErupt = (Action<int, int>) ((from_notInclusive, to) =>
    {
      for (int index = 0; index < geyserCount; ++index)
      {
        if (index > from_notInclusive && index <= to)
        {
          Geyser geyser = geysers[index];
          LargeImpactorLandingSequence.UnentombGeyser(geyser);
          geyser.ShiftTimeTo(Geyser.TimeShiftStep.ActiveState, true);
          Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, new Vector3(geyser.transform.position.x, geyser.transform.position.y + 2f, geyser.transform.position.z - 0.1f), 0.0f);
          LargeImpactorLandingSequence.CreateGeyserEruptionNotification(geyser);
        }
      }
    });
    int lastGeyserIndexRevealed = -1;
    while ((double) geyserRevealTimer < 8.0)
    {
      int num5 = Mathf.FloorToInt(Mathf.Pow(geyserRevealTimer / 8f, 4f) * (float) geyserCount);
      MakeGeyserRangeErupt(lastGeyserIndexRevealed, num5);
      lastGeyserIndexRevealed = num5;
      geyserRevealTimer += Time.deltaTime;
      yield return (object) null;
    }
    int num6 = geyserCount;
    if (lastGeyserIndexRevealed != num6)
      MakeGeyserRangeErupt(lastGeyserIndexRevealed, num6);
    yield return (object) null;
    RootMenu.Instance.canTogglePauseScreen = true;
    CameraController.Instance.DisableUserCameraControl = false;
    CameraController.Instance.SetOverrideZoomSpeed(1f);
    CameraController.Instance.SetMaxOrthographicSize(20f);
    CameraController.Instance.SetWorldInteractive(true);
    HoverTextScreen.Instance.Show();
    RootMenu.Instance.canTogglePauseScreen = true;
    CameraController.Instance.SetTargetPos(templatePosition, 20f, true);
    controller.Trigger(-467702038, (object) null);
  }

  private static void CreateGeyserEruptionNotification(Geyser geyser)
  {
    Vector3 pos = geyser.transform.GetPosition();
    Notifier cmp = geyser.gameObject.AddOrGet<Notifier>();
    Notification notification = new Notification((string) MISC.NOTIFICATIONS.LARGE_IMPACTOR_GEYSER_ERUPTION.NAME, NotificationType.Bad, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.LARGE_IMPACTOR_GEYSER_ERUPTION.TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + cmp.GetProperName()), false, custom_click_callback: (Notification.ClickCallback) (o => GameUtil.FocusCamera(pos)), clear_on_click: true);
    cmp.Add(notification);
  }

  private static void UnentombGeyser(Geyser geyser)
  {
    geyser.Unentomb();
    int cell = Grid.PosToCell((KMonoBehaviour) geyser);
    Vector3 pos = Grid.CellToPos(cell);
    int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
    for (int x = -6; x < 6; ++x)
    {
      for (int y = 0; y < 6; ++y)
      {
        int index = Grid.OffsetCell(cell, x, y);
        float magnitude = (Grid.CellToPos(index) - pos).magnitude;
        float num1 = (float) new KRandom(globalWorldSeed + index).Next() / (float) int.MaxValue;
        float num2 = Mathf.Clamp01((float) (1.0 - ((double) magnitude - 4.0) / 2.0));
        if (((double) magnitude < 4.0 ? 1 : ((double) num1 <= 1.0 * (double) num2 ? 1 : 0)) != 0 && Grid.IsSolidCell(index) && !Grid.Foundation[index] && Grid.Element[index].id != SimHashes.Unobtanium)
          SimMessages.Dig(index);
      }
    }
  }
}
