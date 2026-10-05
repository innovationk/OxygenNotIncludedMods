// Decompiled with JetBrains decompiler
// Type: KModalScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KModalScreen : KScreen
{
  private bool shown;
  public bool pause = true;
  [Tooltip("Only used for main menu")]
  public bool canBackoutWithRightClick;
  private RectTransform backgroundRectTransform;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.backgroundRectTransform = KModalScreen.MakeScreenModal((KScreen) this);
  }

  public static RectTransform MakeScreenModal(KScreen screen)
  {
    screen.ConsumeMouseScroll = true;
    screen.activateOnSpawn = true;
    GameObject gameObject = new GameObject("background");
    gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
    gameObject.AddComponent<CanvasRenderer>();
    Image image = gameObject.AddComponent<Image>();
    image.color = (Color) new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 160 /*0xA0*/);
    image.raycastTarget = true;
    RectTransform component = gameObject.GetComponent<RectTransform>();
    component.SetParent(screen.transform);
    KModalScreen.ResizeBackground(component);
    return component;
  }

  public static void ResizeBackground(RectTransform rectTransform)
  {
    rectTransform.SetAsFirstSibling();
    rectTransform.SetLocalPosition(Vector3.zero);
    rectTransform.localScale = Vector3.one;
    rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
    rectTransform.anchorMax = new Vector2(1f, 1f);
    rectTransform.sizeDelta = new Vector2(0.0f, 0.0f);
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
      CameraController.Instance.DisableUserCameraControl = true;
    if (!((UnityEngine.Object) ScreenResize.Instance != (UnityEngine.Object) null))
      return;
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
      CameraController.Instance.DisableUserCameraControl = false;
    this.Trigger(476357528, (object) null);
    if (!((UnityEngine.Object) ScreenResize.Instance != (UnityEngine.Object) null))
      return;
    ScreenResize.Instance.OnResize -= new System.Action(this.OnResize);
  }

  private void OnResize() => KModalScreen.ResizeBackground(this.backgroundRectTransform);

  public override bool IsModal() => true;

  public override float GetSortKey() => 100f;

  protected override void OnActivate() => this.OnShow(true);

  protected override void OnDeactivate() => this.OnShow(false);

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!this.pause || !((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null))
      return;
    if (show && !this.shown)
      SpeedControlScreen.Instance.Pause(false);
    else if (!show && this.shown)
      SpeedControlScreen.Instance.Unpause(false);
    this.shown = show;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null && (e.TryConsume(Action.TogglePause) || e.TryConsume(Action.CycleSpeed)))
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
    if (!e.Consumed && (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight) && this.canBackoutWithRightClick))
      this.Deactivate();
    base.OnKeyDown(e);
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    base.OnKeyUp(e);
    e.Consumed = true;
  }
}
