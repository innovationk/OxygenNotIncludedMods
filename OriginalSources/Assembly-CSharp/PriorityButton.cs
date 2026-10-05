// Decompiled with JetBrains decompiler
// Type: PriorityButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/PriorityButton")]
public class PriorityButton : KMonoBehaviour
{
  public KToggle toggle;
  public LocText text;
  public ToolTip tooltip;
  [MyCmpGet]
  private ImageToggleState its;
  public ColorStyleSetting normalStyle;
  public ColorStyleSetting highStyle;
  public bool playSelectionSound = true;
  public Action<PrioritySetting> onClick;
  private PrioritySetting _priority;

  public PrioritySetting priority
  {
    get => this._priority;
    set
    {
      this._priority = value;
      if (!((UnityEngine.Object) this.its != (UnityEngine.Object) null))
        return;
      this.its.colorStyleSetting = this.priority.priority_class != PriorityScreen.PriorityClass.high ? this.normalStyle : this.highStyle;
      this.its.RefreshColorStyle();
      this.its.ResetColor();
    }
  }

  protected override void OnPrefabInit() => this.toggle.onClick += new System.Action(this.OnClick);

  private void OnClick()
  {
    if (this.playSelectionSound)
      PriorityScreen.PlayPriorityConfirmSound(this.priority);
    if (this.onClick == null)
      return;
    this.onClick(this.priority);
  }
}
