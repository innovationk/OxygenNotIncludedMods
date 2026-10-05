// Decompiled with JetBrains decompiler
// Type: KModalButtonMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class KModalButtonMenu : KButtonMenu
{
  private bool shown;
  [SerializeField]
  private GameObject panelRoot;
  private GameObject childDialog;
  private RectTransform modalBackground;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.modalBackground = KModalScreen.MakeScreenModal((KScreen) this);
  }

  protected override void OnCmpEnable()
  {
    KModalScreen.ResizeBackground(this.modalBackground);
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if ((UnityEngine.Object) this.childDialog == (UnityEngine.Object) null)
      this.Trigger(476357528, (object) null);
    ScreenResize.Instance.OnResize -= new System.Action(this.OnResize);
  }

  private void OnResize() => KModalScreen.ResizeBackground(this.modalBackground);

  public override bool IsModal() => true;

  public override float GetSortKey() => 100f;

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
    {
      if (show && !this.shown)
        SpeedControlScreen.Instance.Pause(false);
      else if (!show && this.shown)
        SpeedControlScreen.Instance.Unpause(false);
      this.shown = show;
    }
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null))
      return;
    CameraController.Instance.DisableUserCameraControl = show;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    base.OnKeyDown(e);
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    base.OnKeyUp(e);
    e.Consumed = true;
  }

  public void SetBackgroundActive(bool active)
  {
  }

  protected GameObject ActivateChildScreen(GameObject screenPrefab)
  {
    GameObject go = Util.KInstantiateUI(screenPrefab, this.transform.parent.gameObject);
    this.childDialog = go;
    go.Subscribe(476357528, new Action<object>(this.Unhide));
    this.Hide();
    return go;
  }

  private void Hide() => this.panelRoot.rectTransform().localScale = Vector3.zero;

  private void Unhide(object data = null)
  {
    this.panelRoot.rectTransform().localScale = Vector3.one;
    this.childDialog.Unsubscribe(476357528, new Action<object>(this.Unhide));
    this.childDialog = (GameObject) null;
  }
}
