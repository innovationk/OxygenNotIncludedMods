// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.MixingSettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.CustomSettings;

public class MixingSettingConfig : ListSettingConfig
{
  public string worldgenPath { get; private set; }

  public virtual Sprite icon { get; }

  public virtual List<string> forbiddenClusterTags { get; }

  public virtual string dlcIdFrom { get; protected set; }

  public virtual bool isModded { get; }

  protected MixingSettingConfig(
    string id,
    List<SettingLevel> levels,
    string default_level_id,
    string nosweat_default_level_id,
    string worldgenPath,
    long coordinate_range = -1,
    bool debug_only = false,
    bool triggers_custom_game = false,
    string[] required_content = null,
    string missing_content_default = "",
    bool hide_in_ui = false)
    : base(id, "", "", levels, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
  {
    this.worldgenPath = worldgenPath;
  }
}
