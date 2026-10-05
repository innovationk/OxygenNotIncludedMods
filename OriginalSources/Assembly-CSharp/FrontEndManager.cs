// Decompiled with JetBrains decompiler
// Type: FrontEndManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FrontEndManager")]
public class FrontEndManager : KMonoBehaviour
{
  public static FrontEndManager Instance;
  public static bool firstInit = true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    FrontEndManager.Instance = this;
    GameObject gameObject = this.gameObject;
    Util.KInstantiateUI(DlcManager.IsExpansion1Active() ? ScreenPrefabs.Instance.MainMenuForSpacedOut : ScreenPrefabs.Instance.MainMenuForVanilla, gameObject, true);
    if (!FrontEndManager.firstInit)
      return;
    FrontEndManager.firstInit = false;
    GameObject[] gameObjectArray = new GameObject[3]
    {
      ScreenPrefabs.Instance.MainMenuIntroShort,
      ScreenPrefabs.Instance.MainMenuHealthyGameMessage,
      ScreenPrefabs.Instance.DLCBetaWarningScreen
    };
    foreach (GameObject original in gameObjectArray)
      Util.KInstantiateUI(original, gameObject, true);
    GameObject[] screensPrefabsToSpawn = new GameObject[3]
    {
      ScreenPrefabs.Instance.KleiItemDropScreen,
      ScreenPrefabs.Instance.LockerMenuScreen,
      ScreenPrefabs.Instance.LockerNavigator
    };
    List<GameObject> gameObjectsToDestroyOnNextCreate = new List<GameObject>();
    CoroutineRunner coroutineRunner = CoroutineRunner.Create();
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) coroutineRunner);
    CreateCanvases();
    Singleton<KBatchedAnimUpdater>.Instance.OnClear += new System.Action(RecreateCanvases);

    void CreateCanvases()
    {
      int num = 30;
      foreach (GameObject original in screensPrefabsToSpawn)
      {
        GameObject gameObject = this.MakeKleiCanvas(original.name + " Canvas");
        gameObject.GetComponent<Canvas>().sortingOrder = num++;
        Util.KInstantiateUI(original, gameObject, true);
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) gameObject);
        gameObjectsToDestroyOnNextCreate.Add(gameObject);
      }
    }
    // ISSUE: variable of a compiler-generated type
    FrontEndManager.\u003C\u003Ec__DisplayClass2_0 cDisplayClass20;

    void RecreateCanvases()
    {
      if ((UnityEngine.Object) coroutineRunner == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) coroutineRunner)
        return;
      foreach (UnityEngine.Object @object in gameObjectsToDestroyOnNextCreate)
        UnityEngine.Object.Destroy(@object);
      gameObjectsToDestroyOnNextCreate.Clear();
      coroutineRunner.StopAllCoroutines();
      // ISSUE: method pointer
      coroutineRunner.Run((IEnumerator) Updater.Series(Updater.WaitOneFrame(), Updater.Do(new System.Action((object) cDisplayClass20, __methodptr(\u003COnPrefabInit\u003Eg__CreateCanvases\u007C0)))));
    }
  }

  protected override void OnForcedCleanUp()
  {
    FrontEndManager.Instance = (FrontEndManager) null;
    base.OnForcedCleanUp();
  }

  private void LateUpdate()
  {
    if (Debug.developerConsoleVisible)
      Debug.developerConsoleVisible = false;
    KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
    KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
    KAnimBatchManager.Instance().Render();
  }

  public GameObject MakeKleiCanvas(string gameObjectName = "Canvas")
  {
    GameObject gameObject = new GameObject(gameObjectName, new System.Type[1]
    {
      typeof (RectTransform)
    });
    this.ConfigureAsKleiCanvas(gameObject);
    return gameObject;
  }

  public void ConfigureAsKleiCanvas(GameObject gameObject)
  {
    Canvas canvas = gameObject.AddOrGet<Canvas>();
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    canvas.sortingOrder = 10;
    canvas.pixelPerfect = false;
    canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
    GraphicRaycaster graphicRaycaster = gameObject.AddOrGet<GraphicRaycaster>();
    graphicRaycaster.ignoreReversedGraphics = true;
    graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
    graphicRaycaster.blockingMask = (LayerMask) -1;
    CanvasScaler canvasScaler = gameObject.AddOrGet<CanvasScaler>();
    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    canvasScaler.referencePixelsPerUnit = 100f;
    gameObject.AddOrGet<KCanvasScaler>();
  }
}
