// Decompiled with JetBrains decompiler
// Type: ProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/ProgressBar")]
public class ProgressBar : KMonoBehaviour
{
  public Image bar;
  private Func<float> updatePercentFull;
  private int overlayUpdateHandle = -1;
  public bool autoHide = true;
  private Vector3 offset = Vector3.zero;
  private bool lastVisibilityValue = true;
  private bool hasBeenInitialize;
  private int activeWorldChangedHandlerID = -1;

  public Color barColor
  {
    get => this.bar.color;
    set => this.bar.color = value;
  }

  public float PercentFull
  {
    get => this.bar.fillAmount;
    set => this.bar.fillAmount = value;
  }

  public void SetVisibility(bool visible)
  {
    this.lastVisibilityValue = visible;
    this.RefreshVisibility();
  }

  private void RefreshVisibility()
  {
    int myWorldId = this.gameObject.GetMyWorldId();
    this.gameObject.SetActive(((((this.lastVisibilityValue ? 1 : 0) & (!this.hasBeenInitialize ? 1 : (myWorldId == ClusterManager.Instance.activeWorldId ? 1 : 0))) != 0 ? 1 : 0) & (!this.autoHide || (UnityEngine.Object) SimDebugView.Instance == (UnityEngine.Object) null ? 1 : (SimDebugView.Instance.GetMode() == OverlayModes.None.ID ? 1 : 0))) != 0);
    if (this.updatePercentFull != null && !this.updatePercentFull.Target.IsNullOrDestroyed())
      return;
    this.gameObject.SetActive(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.hasBeenInitialize = true;
    if (this.autoHide)
    {
      this.overlayUpdateHandle = Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
      if ((UnityEngine.Object) SimDebugView.Instance != (UnityEngine.Object) null && SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
        this.gameObject.SetActive(false);
    }
    this.activeWorldChangedHandlerID = Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
    this.SetWorldActive(ClusterManager.Instance.activeWorldId);
    this.enabled = this.updatePercentFull != null;
    this.RefreshVisibility();
  }

  private void OnActiveWorldChanged(object data)
  {
    this.SetWorldActive(((Tuple<int, int>) data).first);
  }

  private void SetWorldActive(int worldId) => this.RefreshVisibility();

  public void SetUpdateFunc(Func<float> func)
  {
    this.updatePercentFull = func;
    this.enabled = this.updatePercentFull != null;
  }

  public virtual void Update()
  {
    if (this.updatePercentFull == null || this.updatePercentFull.Target.IsNullOrDestroyed())
      return;
    this.PercentFull = this.updatePercentFull();
  }

  public virtual void OnOverlayChanged(object _ = null) => this.RefreshVisibility();

  public void Retarget(GameObject entity)
  {
    Vector3 vector3 = entity.transform.GetPosition() + Vector3.down * 0.5f;
    Building component = entity.GetComponent<Building>();
    this.transform.SetPosition((!((UnityEngine.Object) component != (UnityEngine.Object) null) ? vector3 - Vector3.right * 0.5f : vector3 - Vector3.right * 0.5f * (float) (component.Def.WidthInCells % 2)) + this.offset);
  }

  protected override void OnCleanUp()
  {
    if (this.overlayUpdateHandle != -1)
      Game.Instance.Unsubscribe(this.overlayUpdateHandle);
    Game.Instance.Unsubscribe(ref this.activeWorldChangedHandlerID);
    base.OnCleanUp();
  }

  private void OnBecameInvisible() => this.enabled = false;

  private void OnBecameVisible() => this.enabled = true;

  public static ProgressBar CreateProgressBar(GameObject entity, Func<float> updateFunc)
  {
    return ProgressBar.CreateProgressBar(entity, updateFunc, Vector3.zero);
  }

  public static ProgressBar CreateProgressBar(
    GameObject entity,
    Func<float> updateFunc,
    Vector3 offset)
  {
    ProgressBar progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab);
    progressBar.offset = offset;
    progressBar.SetUpdateFunc(updateFunc);
    progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
    progressBar.name = ((UnityEngine.Object) entity != (UnityEngine.Object) null ? entity.name + "_" : "") + " ProgressBar";
    progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor(nameof (ProgressBar));
    progressBar.Update();
    progressBar.Retarget(entity);
    return progressBar;
  }
}
