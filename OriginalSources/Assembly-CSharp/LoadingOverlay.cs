// Decompiled with JetBrains decompiler
// Type: LoadingOverlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class LoadingOverlay : KModalScreen
{
  private bool loadNextFrame;
  private bool showLoad;
  private System.Action loadCb;
  private static LoadingOverlay instance;

  protected override void OnPrefabInit()
  {
    this.pause = false;
    this.fadeIn = false;
    base.OnPrefabInit();
  }

  private void Update()
  {
    if (!this.loadNextFrame && this.showLoad)
    {
      this.loadNextFrame = true;
      this.showLoad = false;
    }
    else
    {
      if (!this.loadNextFrame)
        return;
      this.loadNextFrame = false;
      this.loadCb();
    }
  }

  public static void DestroyInstance() => LoadingOverlay.instance = (LoadingOverlay) null;

  public static void Load(System.Action cb)
  {
    GameObject gameObject = GameObject.Find("/SceneInitializerFE/FrontEndManager");
    if ((UnityEngine.Object) LoadingOverlay.instance == (UnityEngine.Object) null)
    {
      LoadingOverlay.instance = Util.KInstantiateUI<LoadingOverlay>(ScreenPrefabs.Instance.loadingOverlay.gameObject, (UnityEngine.Object) GameScreenManager.Instance == (UnityEngine.Object) null ? gameObject : GameScreenManager.Instance.ssOverlayCanvas);
      LoadingOverlay.instance.GetComponentInChildren<LocText>().SetText((string) UI.FRONTEND.LOADING);
    }
    if ((UnityEngine.Object) GameScreenManager.Instance != (UnityEngine.Object) null)
    {
      LoadingOverlay.instance.transform.SetParent(GameScreenManager.Instance.ssOverlayCanvas.transform);
      LoadingOverlay.instance.transform.SetSiblingIndex(GameScreenManager.Instance.ssOverlayCanvas.transform.childCount - 1);
    }
    else
    {
      LoadingOverlay.instance.transform.SetParent(gameObject.transform);
      LoadingOverlay.instance.transform.SetSiblingIndex(gameObject.transform.childCount - 1);
      if ((UnityEngine.Object) MainMenu.Instance != (UnityEngine.Object) null)
        MainMenu.Instance.StopAmbience();
    }
    LoadingOverlay.instance.loadCb = cb;
    LoadingOverlay.instance.showLoad = true;
    LoadingOverlay.instance.Activate();
  }

  public static void Clear()
  {
    if (!((UnityEngine.Object) LoadingOverlay.instance != (UnityEngine.Object) null))
      return;
    LoadingOverlay.instance.Deactivate();
  }
}
