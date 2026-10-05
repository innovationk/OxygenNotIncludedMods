// Decompiled with JetBrains decompiler
// Type: FileErrorReporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FileErrorReporter")]
public class FileErrorReporter : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    this.OnFileError();
    FileUtil.onErrorMessage += new System.Action(this.OnFileError);
  }

  private void OnFileError()
  {
    if (FileUtil.errorType == FileUtil.ErrorType.None)
      return;
    string text;
    switch (FileUtil.errorType)
    {
      case FileUtil.ErrorType.UnauthorizedAccess:
        text = string.Format((string) (FileUtil.errorSubject.Contains("OneDrive") ? STRINGS.UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED_ONEDRIVE : STRINGS.UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED), (object) FileUtil.errorSubject);
        break;
      case FileUtil.ErrorType.IOError:
        text = string.Format((string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.IO_SUFFICIENT_SPACE, (object) FileUtil.errorSubject);
        break;
      default:
        text = string.Format((string) STRINGS.UI.FRONTEND.SUPPORTWARNINGS.IO_UNKNOWN, (object) FileUtil.errorSubject);
        break;
    }
    GameObject gameObject;
    if ((UnityEngine.Object) FrontEndManager.Instance != (UnityEngine.Object) null)
      gameObject = FrontEndManager.Instance.gameObject;
    else if ((UnityEngine.Object) GameScreenManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) GameScreenManager.Instance.ssOverlayCanvas != (UnityEngine.Object) null)
    {
      gameObject = GameScreenManager.Instance.ssOverlayCanvas;
    }
    else
    {
      gameObject = new GameObject();
      gameObject.name = "FileErrorCanvas";
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) gameObject);
      Canvas canvas = gameObject.AddComponent<Canvas>();
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
      canvas.sortingOrder = 10;
      gameObject.AddComponent<GraphicRaycaster>();
    }
    if ((FileUtil.exceptionMessage != null || FileUtil.exceptionStackTrace != null) && !KCrashReporter.hasReportedError)
      KCrashReporter.ReportError(FileUtil.exceptionMessage, FileUtil.exceptionStackTrace, (ConfirmDialogScreen) null, (GameObject) null, (string) null, extraCategories: new string[1]
      {
        KCrashReporter.CRASH_CATEGORY.FILEIO
      });
    ConfirmDialogScreen component = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, gameObject, true).GetComponent<ConfirmDialogScreen>();
    component.PopupConfirmDialog(text, (System.Action) null, (System.Action) null);
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) component.gameObject);
  }

  private void OpenMoreInfo()
  {
  }
}
