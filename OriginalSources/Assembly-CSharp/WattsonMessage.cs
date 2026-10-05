// Decompiled with JetBrains decompiler
// Type: WattsonMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WattsonMessage : KScreen
{
  private const float STARTTIME = 0.1f;
  private const float ENDTIME = 6.6f;
  private const float ALPHA_SPEED = 0.01f;
  private const float expandedHeight = 300f;
  [SerializeField]
  private GameObject dialog;
  [SerializeField]
  private RectTransform content;
  [SerializeField]
  private LocText message;
  [SerializeField]
  private Image bg;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private EventReference dialogSound;
  private List<KScreen> hideScreensWhileActive = new List<KScreen>();
  private bool startFade;
  private List<SchedulerHandle> scheduleHandles = new List<SchedulerHandle>();
  private static readonly HashedString[] WorkLoopAnims = new HashedString[2]
  {
    (HashedString) "working_pre",
    (HashedString) "working_loop"
  };
  private int birthsComplete;

  public override float GetSortKey() => 8f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Game.Instance.Subscribe(-122303817, new Action<object>(this.OnNewBaseCreated));
    string welcomeMessage = CustomGameSettings.Instance.GetCurrentClusterLayout().welcomeMessage;
    if (welcomeMessage != null)
    {
      StringEntry result;
      this.message.SetText(Strings.TryGet(welcomeMessage, out result) ? result.String : welcomeMessage);
    }
    else if (DlcManager.IsExpansion1Active())
      this.message.SetText((string) STRINGS.UI.WELCOMEMESSAGEBODY_SPACEDOUT);
    else
      this.message.SetText((string) STRINGS.UI.WELCOMEMESSAGEBODY);
  }

  private IEnumerator ExpandPanel()
  {
    this.button.isInteractable = false;
    if (CustomGameSettings.Instance.GetSettingsCoordinate().StartsWith("KF23"))
      this.dialog.rectTransform().rotation = Quaternion.Euler(0.0f, 0.0f, -90f);
    yield return (object) SequenceUtil.WaitForSecondsRealtime(0.2f);
    float height = 0.0f;
    while ((double) height < 299.0)
    {
      height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
      this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
      yield return (object) 0;
    }
    if (CustomGameSettings.Instance.GetSettingsCoordinate().StartsWith("KF23"))
    {
      Quaternion initialOrientation = Quaternion.Euler(0.0f, 0.0f, -90f);
      yield return (object) SequenceUtil.WaitForSecondsRealtime(1f);
      float t = 0.0f;
      float duration = 0.5f;
      while ((double) t < (double) duration)
      {
        t += Time.unscaledDeltaTime;
        this.dialog.rectTransform().rotation = Quaternion.Slerp(initialOrientation, Quaternion.identity, t / duration);
        yield return (object) 0;
      }
      initialOrientation = new Quaternion();
    }
    this.button.isInteractable = true;
    yield return (object) null;
  }

  private IEnumerator CollapsePanel()
  {
    float height = 300f;
    while ((double) height > 1.0)
    {
      height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 0.0f, Time.unscaledDeltaTime * 15f);
      this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
      yield return (object) 0;
    }
    this.Deactivate();
    yield return (object) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.hideScreensWhileActive.Add((KScreen) NotificationScreen.Instance);
    this.hideScreensWhileActive.Add((KScreen) OverlayMenu.Instance);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      this.hideScreensWhileActive.Add((KScreen) PlanScreen.Instance);
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      this.hideScreensWhileActive.Add((KScreen) BuildMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ManagementMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ToolMenu.Instance);
    this.hideScreensWhileActive.Add((KScreen) ToolMenu.Instance.PriorityScreen);
    this.hideScreensWhileActive.Add((KScreen) PinnedResourcesPanel.Instance);
    this.hideScreensWhileActive.Add((KScreen) TopLeftControlScreen.Instance);
    this.hideScreensWhileActive.Add((KScreen) DateTime.Instance);
    this.hideScreensWhileActive.Add((KScreen) BuildWatermark.Instance);
    this.hideScreensWhileActive.Add((KScreen) BuildWatermark.Instance);
    this.hideScreensWhileActive.Add((KScreen) ColonyDiagnosticScreen.Instance);
    if ((UnityEngine.Object) WorldSelector.Instance != (UnityEngine.Object) null)
      this.hideScreensWhileActive.Add((KScreen) WorldSelector.Instance);
    foreach (KScreen kscreen in this.hideScreensWhileActive)
      kscreen.Show(false);
  }

  public void Update()
  {
    if (!this.startFade)
      return;
    Color color = this.bg.color;
    color.a -= 0.01f;
    if ((double) color.a <= 0.0)
      color.a = 0.0f;
    this.bg.color = color;
  }

  protected override void OnActivate()
  {
    Debug.Log((object) "WattsonMessage OnActivate");
    base.OnActivate();
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().IntroNIS);
    AudioMixer.instance.activeNIS = true;
    this.button.onClick += (System.Action) (() => this.StartCoroutine(this.CollapsePanel()));
    this.dialog.GetComponent<KScreen>().Show(false);
    this.startFade = false;
    GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
    if ((UnityEngine.Object) telepad != (UnityEngine.Object) null)
    {
      KAnimControllerBase kac = telepad.GetComponent<KAnimControllerBase>();
      kac.Play(WattsonMessage.WorkLoopAnims, KAnim.PlayMode.Loop);
      NameDisplayScreen.Instance.gameObject.SetActive(false);
      for (int idx1 = 0; idx1 < Components.LiveMinionIdentities.Count; ++idx1)
      {
        int idx = idx1 + 1;
        MinionIdentity liveMinionIdentity = Components.LiveMinionIdentities[idx1];
        liveMinionIdentity.gameObject.transform.SetPosition(new Vector3((float) ((double) telepad.transform.GetPosition().x + (double) idx - 1.5), telepad.transform.GetPosition().y, liveMinionIdentity.gameObject.transform.GetPosition().z));
        ChoreProvider chore_provider = liveMinionIdentity.gameObject.GetComponent<ChoreProvider>();
        EmoteChore chorePre = new EmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_portal_kanim", new HashedString[1]
        {
          (HashedString) ("portalbirth_pre_" + idx.ToString())
        }, KAnim.PlayMode.Loop);
        UIScheduler.Instance.Schedule("DupeBirth", (float) idx * 0.5f, (Action<object>) (data =>
        {
          chorePre.Cancel("Done looping");
          EmoteChore emoteChore = new EmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_interacts_portal_kanim", new HashedString[1]
          {
            (HashedString) ("portalbirth_" + idx.ToString())
          });
          emoteChore.onComplete = emoteChore.onComplete + (Action<Chore>) (param =>
          {
            ++this.birthsComplete;
            if (this.birthsComplete != Components.LiveMinionIdentities.Count - 1 || !this.IsActive())
              return;
            NameDisplayScreen.Instance.gameObject.SetActive(true);
            this.PauseAndShowMessage();
          });
        }), (object) null, (SchedulerGroup) null);
      }
      UIScheduler.Instance.Schedule("Welcome", 6.6f, (Action<object>) (data => kac.Play(new HashedString[2]
      {
        (HashedString) "working_pst",
        (HashedString) "idle"
      })), (object) null, (SchedulerGroup) null);
      CameraController.Instance.DisableUserCameraControl = true;
    }
    else
    {
      Debug.LogWarning((object) "Failed to spawn telepad - does the starting base template lack a 'Headquarters' ?");
      this.PauseAndShowMessage();
    }
    this.scheduleHandles.Add(UIScheduler.Instance.Schedule("GoHome", 0.1f, (Action<object>) (data =>
    {
      CameraController.Instance.OrthographicSize = TuningData<WattsonMessage.Tuning>.Get().initialOrthographicSize;
      CameraController.Instance.CameraGoHome(0.5f);
      this.startFade = true;
      MusicManager.instance.PlaySong(this.WelcomeMusic);
    }), (object) null, (SchedulerGroup) null));
  }

  private string WelcomeMusic
  {
    get
    {
      string musicWelcome = CustomGameSettings.Instance.GetCurrentClusterLayout().clusterAudio.musicWelcome;
      return !musicWelcome.IsNullOrWhiteSpace() ? musicWelcome : "Music_WattsonMessage";
    }
  }

  protected void PauseAndShowMessage()
  {
    SpeedControlScreen.Instance.Pause(false);
    this.StartCoroutine(this.ExpandPanel());
    KFMOD.PlayUISound(this.dialogSound);
    this.dialog.GetComponent<KScreen>().Activate();
    this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
    this.dialog.GetComponent<KScreen>().Show();
  }

  protected override void OnDeactivate()
  {
    base.OnDeactivate();
    AudioMixer.instance.Stop(AudioMixerSnapshots.Get().IntroNIS);
    AudioMixer.instance.StartPersistentSnapshots();
    MusicManager.instance.StopSong(this.WelcomeMusic);
    MusicManager.instance.WattsonStartDynamicMusic();
    AudioMixer.instance.activeNIS = false;
    DemoTimer.Instance.CountdownActive = true;
    SpeedControlScreen.Instance.Unpause(false);
    CameraController.Instance.DisableUserCameraControl = false;
    foreach (SchedulerHandle scheduleHandle in this.scheduleHandles)
      scheduleHandle.ClearScheduler();
    UIScheduler.Instance.Schedule("fadeInUI", 0.5f, (Action<object>) (d =>
    {
      foreach (KScreen kscreen in this.hideScreensWhileActive)
      {
        if (!((UnityEngine.Object) kscreen == (UnityEngine.Object) null))
        {
          kscreen.SetShouldFadeIn(true);
          kscreen.Show();
        }
      }
      CameraController.Instance.SetMaxOrthographicSize(20f);
      Game.Instance.StartDelayedInitialSave();
      UIScheduler.Instance.Schedule("InitialScreenshot", 1f, (Action<object>) (data => Game.Instance.timelapser.InitialScreenshot()), (object) null, (SchedulerGroup) null);
      GameScheduler.Instance.Schedule("BasicTutorial", 1.5f, (Action<object>) (data => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Basics)), (object) null, (SchedulerGroup) null);
      GameScheduler.Instance.Schedule("WelcomeTutorial", 2f, (Action<object>) (data => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Welcome)), (object) null, (SchedulerGroup) null);
      GameScheduler.Instance.Schedule("DiggingTutorial", 420f, (Action<object>) (data => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging)), (object) null, (SchedulerGroup) null);
    }), (object) null, (SchedulerGroup) null);
    Game.Instance.SetGameStarted();
    if (!((UnityEngine.Object) TopLeftControlScreen.Instance != (UnityEngine.Object) null))
      return;
    TopLeftControlScreen.Instance.RefreshName();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
    {
      CameraController.Instance.CameraGoHome();
      this.Deactivate();
    }
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e) => e.Consumed = true;

  private void OnNewBaseCreated(object data) => this.gameObject.SetActive(true);

  public class Tuning : TuningData<WattsonMessage.Tuning>
  {
    public float initialOrthographicSize;
  }
}
