// Decompiled with JetBrains decompiler
// Type: PopFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/PopFX")]
public class PopFX : KMonoBehaviour
{
  public const float Speed = 2f;
  private Sprite mainIcon;
  private Sprite icon;
  private string text;
  private Transform targetTransform;
  private Vector3 offset;
  private Vector3 canvasPaddingMultiplier;
  public RectTransform Pivot;
  public Image bg;
  public Image MainIconDisplay;
  public Image IconDisplay;
  public Image mask;
  public LocText TextDisplay;
  public CanvasGroup canvasGroup;
  private Camera uiCamera;
  private float lifetime;
  private float lifeElapsed;
  private bool trackTarget;
  private bool positionToGroup = true;
  private Vector3 startPos;
  private bool isLive;
  private bool isActiveWorld;
  private int eventid = -1;
  private static Action<object, object> OnActiveWorldChangedDispatcher = (Action<object, object>) ((context, data) => Unsafe.As<PopFX>(context).OnActiveWorldChanged(data));

  public Vector3 StartPos => this.startPos;

  public void Recycle()
  {
    this.icon = (Sprite) null;
    this.mainIcon = (Sprite) null;
    this.text = "";
    this.targetTransform = (Transform) null;
    this.lifeElapsed = 0.0f;
    this.trackTarget = false;
    this.startPos = Vector3.zero;
    this.positionToGroup = true;
    this.canvasPaddingMultiplier = Vector3.zero;
    this.IconDisplay.color = Color.white;
    this.TextDisplay.color = Color.white;
    this.MainIconDisplay.color = Color.white;
    PopFXManager.Instance.RecycleFX(this);
    this.canvasGroup.alpha = 0.0f;
    this.IconDisplay.gameObject.SetActive(false);
    this.gameObject.SetActive(false);
    this.isLive = false;
    this.isActiveWorld = false;
    Game.Instance.Unsubscribe(ref this.eventid);
  }

  public void SetIconTint(Color color) => this.MainIconDisplay.color = color;

  public void Run(Vector3 groupSpawnPosition, Vector3 canvasPaddingMultiplier)
  {
    this.gameObject.SetActive(true);
    this.canvasPaddingMultiplier = canvasPaddingMultiplier;
    if (this.positionToGroup && groupSpawnPosition != PopFxGroup.INVALID_SPAWN_POSITION)
      this.startPos = groupSpawnPosition;
    if (this.trackTarget && (UnityEngine.Object) this.targetTransform != (UnityEngine.Object) null)
    {
      this.startPos = this.targetTransform.GetPosition();
      Grid.PosToXY(this.startPos, out int _, out int _);
      this.startPos.x -= 0.5f;
    }
    this.TextDisplay.text = this.text;
    this.IconDisplay.sprite = this.icon;
    this.IconDisplay.Opacity(1f);
    this.MainIconDisplay.Opacity(1f);
    this.MainIconDisplay.sprite = this.mainIcon;
    this.IconDisplay.gameObject.SetActive((UnityEngine.Object) this.icon != (UnityEngine.Object) null);
    this.canvasGroup.alpha = 1f;
    this.isLive = true;
    this.eventid = Game.Instance.Subscribe(1983128072, PopFX.OnActiveWorldChangedDispatcher, (object) this);
    this.SetWorldActive(ClusterManager.Instance.activeWorldId);
    this.Update();
  }

  public void Setup(
    Sprite MainIcon,
    Sprite SecondaryIcon,
    string Text,
    Transform TargetTransform,
    Vector3 Offset,
    bool PositionToGroup,
    float LifeTime = 1.5f,
    bool TrackTarget = false)
  {
    this.mainIcon = MainIcon;
    this.icon = SecondaryIcon;
    this.text = Text;
    this.targetTransform = TargetTransform;
    this.trackTarget = TrackTarget;
    this.lifetime = LifeTime;
    this.offset = Offset;
    this.positionToGroup = PositionToGroup;
    if ((UnityEngine.Object) this.targetTransform != (UnityEngine.Object) null)
      this.startPos = this.targetTransform.GetPosition();
    Grid.PosToXY(this.startPos, out int _, out int _);
    this.startPos.x -= 0.5f;
  }

  private void OnActiveWorldChanged(object data)
  {
    Tuple<int, int> tuple = (Tuple<int, int>) data;
    if (!this.isLive)
      return;
    this.SetWorldActive(tuple.first);
  }

  private void SetWorldActive(int worldId)
  {
    int cell = Grid.PosToCell(!this.trackTarget || !((UnityEngine.Object) this.targetTransform != (UnityEngine.Object) null) ? this.startPos + this.offset : this.targetTransform.position);
    this.isActiveWorld = !Grid.IsValidCell(cell) || (int) Grid.WorldIdx[cell] == worldId;
  }

  private void Update()
  {
    if (!this.isLive || !PopFXManager.Instance.Ready())
      return;
    this.lifeElapsed += Time.unscaledDeltaTime;
    if ((double) this.lifeElapsed >= (double) this.lifetime)
      this.Recycle();
    if (this.trackTarget && (UnityEngine.Object) this.targetTransform != (UnityEngine.Object) null)
    {
      Vector3 screen = PopFXManager.Instance.WorldToScreen(this.targetTransform.GetPosition() + this.offset + Vector3.up * this.lifeElapsed * (2f * this.lifeElapsed)) with
      {
        z = 0.0f
      };
      this.gameObject.rectTransform().anchoredPosition = (Vector2) screen;
    }
    else
    {
      Vector3 screen = PopFXManager.Instance.WorldToScreen(this.startPos + this.offset + Vector3.up * this.lifeElapsed * (float) (2.0 * ((double) this.lifeElapsed / 2.0))) with
      {
        z = 0.0f
      };
      Vector3 size = (Vector3) this.Pivot.rect.size;
      size.x *= this.canvasPaddingMultiplier.x;
      size.y *= this.canvasPaddingMultiplier.y;
      size.z *= this.canvasPaddingMultiplier.z;
      screen += size;
      this.gameObject.rectTransform().anchoredPosition = (Vector2) screen;
    }
    float t1 = (float) (((double) CameraController.Instance.OrthographicSize - (double) CameraController.Instance.minOrthographicSize) / ((CameraController.Instance.FreeCameraEnabled ? (double) TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug : 20.0) - (double) CameraController.Instance.minOrthographicSize));
    this.gameObject.rectTransform().localScale = Vector3.one * Mathf.Lerp(1f, 0.7f, t1);
    float num1 = Mathf.Clamp01((this.lifetime - this.lifeElapsed) / this.lifetime);
    float t2 = Mathf.Clamp01((float) ((1.0 - (double) num1) / 0.10000000149011612));
    float num2 = Mathf.Clamp01(num1 / 0.2f);
    this.mask.fillAmount = Mathf.Lerp(0.16f * num2, 1f, t2);
    this.canvasGroup.alpha = this.isActiveWorld ? num2 : 0.0f;
  }
}
