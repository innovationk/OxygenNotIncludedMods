// Decompiled with JetBrains decompiler
// Type: OfflineWorldGen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/OfflineWorldGen")]
public class OfflineWorldGen : KMonoBehaviour
{
  [SerializeField]
  private RectTransform buttonRoot;
  [SerializeField]
  private GameObject buttonPrefab;
  [SerializeField]
  private RectTransform chooseLocationPanel;
  [SerializeField]
  private GameObject locationButtonPrefab;
  private const float baseScale = 0.005f;
  private Mutex errorMutex = new Mutex();
  private List<OfflineWorldGen.ErrorInfo> errors = new List<OfflineWorldGen.ErrorInfo>();
  private OfflineWorldGen.ValidDimensions[] validDimensions = new OfflineWorldGen.ValidDimensions[1]
  {
    new OfflineWorldGen.ValidDimensions()
    {
      width = 256 /*0x0100*/,
      height = 384,
      name = UI.FRONTEND.WORLDGENSCREEN.SIZES.STANDARD.key
    }
  };
  public string frontendGameLevel = "frontend";
  public string mainGameLevel = "backend";
  private bool shouldStop;
  private StringKey currentConvertedCurrentStage;
  private float currentPercent;
  public bool debug;
  private bool trackProgress = true;
  private bool doWorldGen;
  [SerializeField]
  private LocText titleText;
  [SerializeField]
  private LocText mainText;
  [SerializeField]
  private LocText updateText;
  [SerializeField]
  private LocText percentText;
  [SerializeField]
  private LocText seedText;
  [SerializeField]
  private KBatchedAnimController meterAnim;
  [SerializeField]
  private KBatchedAnimController asteriodAnim;
  private Cluster cluster;
  private StringKey currentStringKeyRoot;
  private List<LocString> convertList = new List<LocString>()
  {
    UI.WORLDGEN.SETTLESIM,
    UI.WORLDGEN.BORDERS,
    UI.WORLDGEN.PROCESSING,
    UI.WORLDGEN.COMPLETELAYOUT,
    UI.WORLDGEN.WORLDLAYOUT,
    UI.WORLDGEN.GENERATENOISE,
    UI.WORLDGEN.BUILDNOISESOURCE,
    UI.WORLDGEN.GENERATESOLARSYSTEM
  };
  private WorldGenProgressStages.Stages currentStage;
  private bool loadTriggered;
  private bool startedExitFlow;
  private int seed;

  private void TrackProgress(string text)
  {
    if (!this.trackProgress)
      return;
    Debug.Log((object) text);
  }

  public static bool CanLoadSave()
  {
    bool flag = WorldGen.CanLoad(SaveLoader.GetActiveSaveFilePath());
    if (!flag)
    {
      SaveLoader.SetActiveSaveFilePath((string) null);
      flag = WorldGen.CanLoad(WorldGen.WORLDGEN_SAVE_FILENAME);
    }
    return flag;
  }

  public void Generate()
  {
    this.doWorldGen = !OfflineWorldGen.CanLoadSave();
    this.updateText.gameObject.SetActive(false);
    this.percentText.gameObject.SetActive(false);
    this.doWorldGen |= this.debug;
    if (this.doWorldGen)
    {
      this.seedText.text = string.Format((string) UI.WORLDGEN.USING_PLAYER_SEED, (object) this.seed);
      this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.TITLE.ToString();
      this.mainText.text = UI.WORLDGEN.CHOOSEWORLDSIZE.ToString();
      for (int index = 0; index < this.validDimensions.Length; ++index)
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
        gameObject.SetActive(true);
        RectTransform component = gameObject.GetComponent<RectTransform>();
        component.SetParent((Transform) this.buttonRoot);
        component.localScale = Vector3.one;
        gameObject.GetComponentInChildren<LocText>().text = this.validDimensions[index].name.ToString();
        int idx = index;
        gameObject.GetComponent<KButton>().onClick += (System.Action) (() =>
        {
          this.DoWorldGen(idx);
          this.ToggleGenerationUI();
        });
      }
      if (this.validDimensions.Length == 1)
      {
        this.DoWorldGen(0);
        this.ToggleGenerationUI();
      }
      ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
      this.OnResize();
    }
    else
    {
      this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.LOADINGGAME.ToString();
      this.mainText.gameObject.SetActive(false);
      this.currentConvertedCurrentStage = UI.WORLDGEN.COMPLETE.key;
      this.currentPercent = 1f;
      this.updateText.gameObject.SetActive(false);
      this.percentText.gameObject.SetActive(false);
      this.RemoveButtons();
    }
    this.buttonPrefab.SetActive(false);
  }

  private void OnResize()
  {
    float canvasScale = this.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
    if (!((UnityEngine.Object) this.asteriodAnim != (UnityEngine.Object) null))
      return;
    this.asteriodAnim.animScale = (float) (0.004999999888241291 * (1.0 / (double) canvasScale));
  }

  private void ToggleGenerationUI()
  {
    this.percentText.gameObject.SetActive(false);
    this.updateText.gameObject.SetActive(true);
    this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.GENERATINGWORLD.ToString();
    if ((UnityEngine.Object) this.titleText != (UnityEngine.Object) null && (UnityEngine.Object) this.titleText.gameObject != (UnityEngine.Object) null)
      this.titleText.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this.buttonRoot != (UnityEngine.Object) null) || !((UnityEngine.Object) this.buttonRoot.gameObject != (UnityEngine.Object) null))
      return;
    this.buttonRoot.gameObject.SetActive(false);
  }

  private bool UpdateProgress(
    StringKey stringKeyRoot,
    float completePercent,
    WorldGenProgressStages.Stages stage)
  {
    if (this.currentStage != stage)
      this.currentStage = stage;
    if (this.currentStringKeyRoot.Hash != stringKeyRoot.Hash)
    {
      this.currentConvertedCurrentStage = stringKeyRoot;
      this.currentStringKeyRoot = stringKeyRoot;
    }
    else
    {
      int num = (int) completePercent * 10;
      LocString locString = this.convertList.Find((Predicate<LocString>) (s => s.key.Hash == stringKeyRoot.Hash));
      if (num != 0 && locString != null)
        this.currentConvertedCurrentStage = new StringKey(locString.key.String + num.ToString());
    }
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = WorldGenProgressStages.StageWeights[(int) stage].Value * completePercent;
    for (int index = 0; index < WorldGenProgressStages.StageWeights.Length; ++index)
    {
      num2 += WorldGenProgressStages.StageWeights[index].Value;
      if ((WorldGenProgressStages.Stages) index < this.currentStage)
        num1 += WorldGenProgressStages.StageWeights[index].Value;
    }
    this.currentPercent = (num1 + num3) / num2;
    return !this.shouldStop;
  }

  private void Update()
  {
    if (this.cluster.HasGenerationStopped)
      this.cluster.Join();
    if (this.loadTriggered || this.currentConvertedCurrentStage.String == null)
      return;
    this.errorMutex.WaitOne();
    int count = this.errors.Count;
    this.errorMutex.ReleaseMutex();
    if (count > 0)
    {
      this.DoExitFlow();
    }
    else
    {
      this.updateText.text = (string) Strings.Get(this.currentConvertedCurrentStage.String);
      if (!this.debug && this.currentConvertedCurrentStage.Hash == UI.WORLDGEN.COMPLETE.key.Hash && (double) this.currentPercent >= 1.0 && this.cluster.HasGenerationStopped)
      {
        if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
          return;
        this.percentText.text = "";
        this.loadTriggered = true;
        App.LoadScene(this.mainGameLevel);
      }
      else if ((double) this.currentPercent < 0.0)
      {
        this.DoExitFlow();
      }
      else
      {
        if ((double) this.currentPercent > 0.0 && !this.percentText.gameObject.activeSelf)
          this.percentText.gameObject.SetActive(false);
        this.percentText.text = GameUtil.GetFormattedPercent(this.currentPercent * 100f);
        this.meterAnim.SetPositionPercent(this.currentPercent);
      }
    }
  }

  private void DisplayErrors()
  {
    this.errorMutex.WaitOne();
    if (this.errors.Count > 0)
    {
      foreach (OfflineWorldGen.ErrorInfo error in this.errors)
        Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, FrontEndManager.Instance.gameObject, true).PopupConfirmDialog(error.errorDesc, new System.Action(this.OnConfirmExit), (System.Action) null);
    }
    this.errorMutex.ReleaseMutex();
  }

  private void DoExitFlow()
  {
    if (this.startedExitFlow)
      return;
    this.startedExitFlow = true;
    this.percentText.text = UI.WORLDGEN.RESTARTING.ToString();
    this.loadTriggered = true;
    Sim.Shutdown();
    this.DisplayErrors();
  }

  private void OnConfirmExit() => App.LoadScene(this.frontendGameLevel);

  private void RemoveButtons()
  {
    for (int index = this.buttonRoot.childCount - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.buttonRoot.GetChild(index).gameObject);
  }

  private void DoWorldGen(int selectedDimension)
  {
    this.RemoveButtons();
    this.DoWorldGenInitialize();
  }

  private void DoWorldGenInitialize()
  {
    Func<int, WorldGen, bool> func = (Func<int, WorldGen, bool>) null;
    this.seed = CustomGameSettings.Instance.GetCurrentWorldgenSeed();
    string id = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.ClusterLayout).id;
    List<string> chosenStoryTraitIds = new List<string>();
    foreach (string currentStorey in CustomGameSettings.Instance.GetCurrentStories())
      chosenStoryTraitIds.Add(Db.Get().Stories.Get(currentStorey).worldgenStoryTraitKey);
    this.cluster = new Cluster(id, this.seed, chosenStoryTraitIds, true, false);
    this.cluster.ShouldSkipWorldCallback = func;
    this.cluster.Generate(new WorldGen.OfflineCallbackFunction(this.UpdateProgress), new Action<OfflineWorldGen.ErrorInfo>(this.OnError), this.seed, this.seed, this.seed, this.seed);
  }

  private void OnError(OfflineWorldGen.ErrorInfo error)
  {
    this.errorMutex.WaitOne();
    this.errors.Add(error);
    this.errorMutex.ReleaseMutex();
  }

  public struct ErrorInfo
  {
    public string errorDesc;
    public Exception exception;
  }

  [Serializable]
  private struct ValidDimensions
  {
    public int width;
    public int height;
    public StringKey name;
  }
}
