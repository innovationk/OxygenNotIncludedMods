// Decompiled with JetBrains decompiler
// Type: VideoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

#nullable disable
public class VideoScreen : KModalScreen
{
  public static VideoScreen Instance;
  [SerializeField]
  private VideoPlayer videoPlayer;
  [SerializeField]
  private Slideshow slideshow;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton proceedButton;
  [SerializeField]
  private RectTransform overlayContainer;
  [SerializeField]
  private List<VideoOverlay> overlayPrefabs;
  private RawImage screen;
  private RenderTexture renderTexture;
  private EventReference activeAudioSnapshot;
  [SerializeField]
  private Image fadeOverlay;
  private EventInstance audioHandle;
  private bool victoryLoopQueued;
  private string victoryLoopMessage = "";
  private string victoryLoopClip = "";
  private bool victoryLoopSyncAudio;
  private bool videoSkippable = true;
  public System.Action OnStop;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
    this.closeButton.onClick += (System.Action) (() => this.Stop());
    this.proceedButton.onClick += (System.Action) (() => this.Stop());
    this.videoPlayer.isLooping = false;
    this.videoPlayer.loopPointReached += (VideoPlayer.EventHandler) (data =>
    {
      if (this.victoryLoopQueued)
      {
        this.StartCoroutine(this.SwitchToVictoryLoop());
      }
      else
      {
        if (this.videoPlayer.isLooping)
          return;
        this.Stop();
      }
    });
    VideoScreen.Instance = this;
    this.Show(false);
  }

  protected override void OnForcedCleanUp()
  {
    VideoScreen.Instance = (VideoScreen) null;
    base.OnForcedCleanUp();
  }

  protected override void OnShow(bool show)
  {
    this.transform.SetAsLastSibling();
    base.OnShow(show);
    this.screen = this.videoPlayer.gameObject.GetComponent<RawImage>();
  }

  public void DisableAllMedia()
  {
    this.overlayContainer.gameObject.SetActive(false);
    this.videoPlayer.gameObject.SetActive(false);
    this.slideshow.gameObject.SetActive(false);
  }

  public void PlaySlideShow(Sprite[] sprites)
  {
    this.Show();
    this.DisableAllMedia();
    this.slideshow.updateType = SlideshowUpdateType.preloadedSprites;
    this.slideshow.gameObject.SetActive(true);
    this.slideshow.SetSprites(sprites);
    this.slideshow.SetPaused(false);
  }

  public void PlaySlideShow(string[] files)
  {
    this.Show();
    this.DisableAllMedia();
    this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
    this.slideshow.gameObject.SetActive(true);
    this.slideshow.SetFiles(files, 0);
    this.slideshow.SetPaused(false);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.IsAction(Action.Escape))
    {
      if (this.slideshow.gameObject.activeSelf && e.TryConsume(Action.Escape))
      {
        this.Stop();
        return;
      }
      if (e.TryConsume(Action.Escape))
      {
        if (!this.videoSkippable)
          return;
        this.Stop();
        return;
      }
    }
    base.OnKeyDown(e);
  }

  public void PlayVideo(
    VideoClip clip,
    bool unskippable = false,
    EventReference overrideAudioSnapshot = default (EventReference),
    bool showProceedButton = false,
    bool syncAudio = true)
  {
    Debug.Assert((UnityEngine.Object) clip != (UnityEngine.Object) null);
    for (int index = 0; index < this.overlayContainer.childCount; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.overlayContainer.GetChild(index).gameObject);
    this.Show();
    this.videoPlayer.isLooping = false;
    this.activeAudioSnapshot = overrideAudioSnapshot.IsNull ? AudioMixerSnapshots.Get().TutorialVideoPlayingSnapshot : overrideAudioSnapshot;
    AudioMixer.instance.Start(this.activeAudioSnapshot);
    this.DisableAllMedia();
    this.videoPlayer.gameObject.SetActive(true);
    this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16 /*0x10*/);
    this.screen.texture = (Texture) this.renderTexture;
    this.videoPlayer.targetTexture = this.renderTexture;
    this.videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
    this.videoPlayer.clip = clip;
    this.videoPlayer.timeReference = syncAudio ? VideoTimeReference.ExternalTime : VideoTimeReference.Freerun;
    this.videoPlayer.Play();
    if (this.audioHandle.isValid())
    {
      KFMOD.EndOneShot(this.audioHandle);
      this.audioHandle.clearHandle();
    }
    this.audioHandle = KFMOD.BeginOneShot(GlobalAssets.GetSound("vid_" + clip.name), Vector3.zero);
    KFMOD.EndOneShot(this.audioHandle);
    this.videoSkippable = !unskippable;
    this.closeButton.gameObject.SetActive(this.videoSkippable);
    this.proceedButton.gameObject.SetActive(showProceedButton && this.videoSkippable);
  }

  public void QueueVictoryVideoLoop(
    bool queue,
    string message = "",
    string victoryAchievement = "",
    string loopVideo = "",
    bool showAchievements = true,
    bool syncAudio = false)
  {
    this.victoryLoopQueued = queue;
    this.victoryLoopMessage = message;
    this.victoryLoopClip = loopVideo;
    this.victoryLoopSyncAudio = syncAudio;
    this.OnStop += (System.Action) (() =>
    {
      if (!showAchievements)
        return;
      RetireColonyUtility.SaveColonySummaryData();
      MainMenu.ActivateRetiredColoniesScreenFromData(this.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
    });
  }

  public void PlayShortWithVictoryLoop(
    string shortVideo,
    string message,
    string victoryAchievement,
    string loopVideo,
    bool showAchievements = true,
    EventReference overrideAudioSnapshot = default (EventReference))
  {
    this.PlayVideo(Assets.GetVideo(shortVideo), true, overrideAudioSnapshot);
    this.QueueVictoryVideoLoop(true, message, victoryAchievement, loopVideo, showAchievements);
  }

  public void PlayVictoryLoop(
    string message,
    string victoryAchievement,
    string loopVideo,
    bool showAchievements = true,
    EventReference overrideAudioSnapshot = default (EventReference),
    bool fadeIn = false)
  {
    this.PlayVideo(Assets.GetVideo(loopVideo), true, overrideAudioSnapshot);
    this.videoPlayer.isLooping = true;
    this.videoPlayer.timeReference = VideoTimeReference.Freerun;
    MusicManager.instance.PlaySong("Music_Victory_03_StoryAndSummary");
    MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 1f);
    this.closeButton.gameObject.SetActive(true);
    this.proceedButton.gameObject.SetActive(true);
    this.SetOverlayText("VictoryEnd", new List<string>()
    {
      message
    });
    if (fadeIn)
    {
      this.fadeOverlay.color = new Color(0.0f, 0.0f, 0.0f, 1f);
      this.StartCoroutine(this.FadeInOverlay());
    }
    this.OnStop += (System.Action) (() =>
    {
      if (!showAchievements)
        return;
      RetireColonyUtility.SaveColonySummaryData();
      MainMenu.ActivateRetiredColoniesScreenFromData(this.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
    });
  }

  private IEnumerator FadeInOverlay()
  {
    yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
    for (float i = 1f; (double) i >= 0.0; i -= Time.unscaledDeltaTime)
    {
      this.fadeOverlay.color = new Color(0.0f, 0.0f, 0.0f, i);
      yield return (object) SequenceUtil.WaitForNextFrame;
    }
    this.fadeOverlay.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  }

  public void SetOverlayText(string overlayTemplate, List<string> strings)
  {
    VideoOverlay videoOverlay = (VideoOverlay) null;
    foreach (VideoOverlay overlayPrefab in this.overlayPrefabs)
    {
      if (overlayPrefab.name == overlayTemplate)
      {
        videoOverlay = overlayPrefab;
        break;
      }
    }
    DebugUtil.Assert((UnityEngine.Object) videoOverlay != (UnityEngine.Object) null, "Could not find a template named ", overlayTemplate);
    Util.KInstantiateUI<VideoOverlay>(videoOverlay.gameObject, this.overlayContainer.gameObject, true).SetText(strings);
    this.overlayContainer.gameObject.SetActive(true);
  }

  private IEnumerator SwitchToVictoryLoop()
  {
    this.victoryLoopQueued = false;
    Color color = this.fadeOverlay.color;
    float i;
    for (i = 0.0f; (double) i < 1.0; i += Time.unscaledDeltaTime)
    {
      this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
      yield return (object) SequenceUtil.WaitForNextFrame;
    }
    this.fadeOverlay.color = new Color(color.r, color.g, color.b, 1f);
    MusicManager.instance.PlaySong("Music_Victory_03_StoryAndSummary");
    MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 1f);
    this.closeButton.gameObject.SetActive(true);
    this.proceedButton.gameObject.SetActive(true);
    this.SetOverlayText("VictoryEnd", new List<string>()
    {
      this.victoryLoopMessage
    });
    this.videoPlayer.clip = Assets.GetVideo(this.victoryLoopClip);
    this.videoPlayer.isLooping = true;
    this.videoPlayer.Play();
    this.proceedButton.gameObject.SetActive(true);
    this.videoPlayer.timeReference = this.victoryLoopSyncAudio ? VideoTimeReference.ExternalTime : VideoTimeReference.Freerun;
    yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
    for (i = 1f; (double) i >= 0.0; i -= Time.unscaledDeltaTime)
    {
      this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
      yield return (object) SequenceUtil.WaitForNextFrame;
    }
    this.fadeOverlay.color = new Color(color.r, color.g, color.b, 0.0f);
  }

  public void Stop()
  {
    this.videoPlayer.Stop();
    this.screen.texture = (Texture) null;
    this.videoPlayer.targetTexture = (RenderTexture) null;
    if (!this.activeAudioSnapshot.IsNull)
    {
      AudioMixer.instance.Stop(this.activeAudioSnapshot);
      int num = (int) this.audioHandle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    if (this.OnStop != null)
      this.OnStop();
    this.Show(false);
  }

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    if (!this.audioHandle.isValid())
      return;
    int position;
    int timelinePosition = (int) this.audioHandle.getTimelinePosition(out position);
    this.videoPlayer.externalReferenceTime = (double) position / 1000.0;
  }
}
