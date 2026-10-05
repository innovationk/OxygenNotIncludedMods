// Decompiled with JetBrains decompiler
// Type: MultipleRenderTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class MultipleRenderTarget : MonoBehaviour
{
  private MultipleRenderTargetProxy renderProxy;
  private FullScreenQuad quad;
  public bool isFrontEnd;

  public event Action<Camera> onSetupComplete;

  private void Start() => this.StartCoroutine(this.SetupProxy());

  private IEnumerator SetupProxy()
  {
    yield return (object) null;
    Camera component = this.GetComponent<Camera>();
    Camera camera = new GameObject().AddComponent<Camera>();
    camera.CopyFrom(component);
    this.renderProxy = camera.gameObject.AddComponent<MultipleRenderTargetProxy>();
    camera.name = component.name + " MRT";
    camera.transform.parent = component.transform;
    camera.transform.SetLocalPosition(Vector3.zero);
    camera.depth = component.depth - 1f;
    camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Water"));
    component.cullingMask = 0;
    component.clearFlags = CameraClearFlags.Color;
    this.quad = new FullScreenQuad(nameof (MultipleRenderTarget), component, true);
    if (this.onSetupComplete != null)
      this.onSetupComplete(camera);
  }

  private void OnPreCull()
  {
    if (!((UnityEngine.Object) this.renderProxy != (UnityEngine.Object) null))
      return;
    this.quad.Draw((Texture) this.renderProxy.Textures[0]);
  }

  public void ToggleColouredOverlayView(bool enabled)
  {
    if (!((UnityEngine.Object) this.renderProxy != (UnityEngine.Object) null))
      return;
    this.renderProxy.ToggleColouredOverlayView(enabled);
  }
}
