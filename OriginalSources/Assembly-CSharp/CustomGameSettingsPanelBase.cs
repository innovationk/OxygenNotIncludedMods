// Decompiled with JetBrains decompiler
// Type: CustomGameSettingsPanelBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class CustomGameSettingsPanelBase : MonoBehaviour
{
  protected List<CustomGameSettingWidget> widgets = new List<CustomGameSettingWidget>();
  private bool isDirty;

  public virtual void Init()
  {
  }

  public virtual void Uninit()
  {
  }

  private void OnEnable() => this.isDirty = true;

  private void Update()
  {
    if (!this.isDirty)
      return;
    this.isDirty = false;
    this.Refresh();
  }

  protected void AddWidget(CustomGameSettingWidget widget)
  {
    widget.onSettingChanged += new Action<CustomGameSettingWidget>(this.OnWidgetChanged);
    this.widgets.Add(widget);
  }

  private void OnWidgetChanged(CustomGameSettingWidget widget) => this.isDirty = true;

  public virtual void Refresh()
  {
    foreach (CustomGameSettingWidget widget in this.widgets)
      widget.Refresh();
  }
}
