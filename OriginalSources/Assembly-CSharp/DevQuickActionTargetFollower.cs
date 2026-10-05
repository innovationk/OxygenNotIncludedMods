// Decompiled with JetBrains decompiler
// Type: DevQuickActionTargetFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class DevQuickActionTargetFollower : MonoBehaviour
{
  public Toggle toggle;
  public RectTransform targetPivot;
  public RectTransform line;
  private ColorBlock toggleOnColorBlock;
  private ColorBlock toggleOffColorBlock;
  private GameObject Target;
  public Action<bool> OnToggleChanged;

  public RectTransform transform => base.transform as RectTransform;

  public bool IsToggleOn => this.toggle.isOn;

  private void Awake()
  {
    this.toggleOffColorBlock = this.toggle.colors;
    this.toggleOnColorBlock = this.toggle.colors;
    this.toggleOnColorBlock.normalColor = this.toggleOffColorBlock.pressedColor;
    this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
    this.toggle.SetIsOnWithoutNotify(true);
    this.RefreshToggleVisuals();
  }

  public void ManualToggle(bool val) => this.toggle.isOn = val;

  public void OnToggleValueChanged(bool newValue)
  {
    this.RefreshToggleVisuals();
    Action<bool> onToggleChanged = this.OnToggleChanged;
    if (onToggleChanged == null)
      return;
    onToggleChanged(newValue);
  }

  public void RefreshToggleVisuals()
  {
    this.toggle.colors = this.toggle.isOn ? this.toggleOnColorBlock : this.toggleOffColorBlock;
  }

  public void SetTarget(GameObject target) => this.Target = target;

  private void Update() => this.Refresh();

  public void Refresh()
  {
    if (!((UnityEngine.Object) this.Target != (UnityEngine.Object) null))
      return;
    Vector3 screenPoint = CameraController.Instance.overlayCamera.WorldToScreenPoint(this.Target.transform.position);
    this.targetPivot.transform.SetPosition(screenPoint);
    this.targetPivot.localPosition = this.targetPivot.localPosition with
    {
      z = 0.0f
    };
    this.line.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, ((this.transform.position - screenPoint) with
    {
      z = 0.0f
    }).normalized));
    this.line.sizeDelta = this.line.sizeDelta with
    {
      x = this.targetPivot.localPosition.magnitude
    };
  }

  public void SetVisibleState(bool visible) => this.gameObject.SetActive(visible);
}
