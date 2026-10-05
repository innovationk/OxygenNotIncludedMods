// Decompiled with JetBrains decompiler
// Type: LargeImpactorNotificationUI_CycleLabelEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LargeImpactorNotificationUI_CycleLabelEffects : MonoBehaviour
{
  public ToolTip notificationTooltipComponent;
  public Image cyclesLabelBackground;
  public LocText numberOfCyclesLabel;
  private Coroutine cycleLabelFocusCoroutine;
  private float cycleFocusSpeed = 0.2f;
  private float cycleUnfocusSpeed = 0.4f;

  public void InitializeCycleLabelFocusMonitor()
  {
    this.AbortCycleLabelFocusMonitor();
    this.cycleLabelFocusCoroutine = this.StartCoroutine(this.CycleLabelFocusMonitor());
  }

  public void AbortCycleLabelFocusMonitor()
  {
    if (this.cycleLabelFocusCoroutine == null)
      return;
    this.StopCoroutine(this.cycleLabelFocusCoroutine);
    this.cycleLabelFocusCoroutine = (Coroutine) null;
  }

  private IEnumerator CycleLabelFocusMonitor()
  {
    float previousVisibleValue = -1f;
    float visibleValue = 0.0f;
    while (true)
    {
      visibleValue = Mathf.Clamp(visibleValue + (float) ((double) Time.unscaledDeltaTime / (this.notificationTooltipComponent.isHovering ? (double) this.cycleFocusSpeed : (double) this.cycleUnfocusSpeed) * (this.notificationTooltipComponent.isHovering ? 1.0 : -1.0)), 0.0f, 1f);
      if ((double) visibleValue != (double) previousVisibleValue)
      {
        previousVisibleValue = visibleValue;
        this.cyclesLabelBackground.Opacity(visibleValue);
        this.numberOfCyclesLabel.Opacity(visibleValue);
      }
      yield return (object) null;
    }
  }
}
