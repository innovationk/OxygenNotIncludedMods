// Decompiled with JetBrains decompiler
// Type: VirtualCursorOverlayFix
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class VirtualCursorOverlayFix : MonoBehaviour
{
  private RenderTexture cursorRendTex;
  public Camera screenSpaceCamera;
  public Image screenSpaceOverlayImage;
  public RawImage actualCursor;

  private void Awake()
  {
    this.cursorRendTex = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 0);
    this.screenSpaceCamera.enabled = true;
    this.screenSpaceCamera.targetTexture = this.cursorRendTex;
    this.screenSpaceOverlayImage.material.SetTexture("_MainTex", (Texture) this.cursorRendTex);
    this.StartCoroutine(this.RenderVirtualCursor());
  }

  private IEnumerator RenderVirtualCursor()
  {
    bool ShowCursor = KInputManager.currentControllerIsGamepad;
    while (Application.isPlaying)
    {
      ShowCursor = KInputManager.currentControllerIsGamepad;
      if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.C))
        ShowCursor = true;
      this.screenSpaceCamera.enabled = true;
      if (!this.screenSpaceOverlayImage.enabled & ShowCursor)
        yield return (object) SequenceUtil.WaitForSecondsRealtime(0.1f);
      this.actualCursor.enabled = ShowCursor;
      this.screenSpaceOverlayImage.enabled = ShowCursor;
      this.screenSpaceOverlayImage.material.SetTexture("_MainTex", (Texture) this.cursorRendTex);
      yield return (object) null;
    }
  }
}
