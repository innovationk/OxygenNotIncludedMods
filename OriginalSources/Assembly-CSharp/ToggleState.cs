// Decompiled with JetBrains decompiler
// Type: ToggleState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct ToggleState
{
  public string Name;
  public string on_click_override_sound_path;
  public string on_release_override_sound_path;
  public string sound_parameter_name;
  public float sound_parameter_value;
  public bool has_sound_parameter;
  public Sprite sprite;
  public Color color;
  public Color color_on_hover;
  public bool use_color_on_hover;
  public bool use_rect_margins;
  public Vector2 rect_margins;
  public StatePresentationSetting[] additional_display_settings;
}
