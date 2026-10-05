// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.ToggleSettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Klei.CustomSettings;

public class ToggleSettingConfig : SettingConfig
{
  public SettingLevel on_level { get; private set; }

  public SettingLevel off_level { get; private set; }

  public ToggleSettingConfig(
    string id,
    string label,
    string tooltip,
    SettingLevel off_level,
    SettingLevel on_level,
    string default_level_id,
    string nosweat_default_level_id,
    long coordinate_range = -1,
    bool debug_only = false,
    bool triggers_custom_game = true,
    string[] required_content = null,
    string missing_content_default = "")
    : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default)
  {
    this.off_level = off_level;
    this.on_level = on_level;
  }

  public void StompLevels(
    SettingLevel off_level,
    SettingLevel on_level,
    string default_level_id,
    string nosweat_default_level_id)
  {
    this.off_level = off_level;
    this.on_level = on_level;
    this.default_level_id = default_level_id;
    this.nosweat_default_level_id = nosweat_default_level_id;
  }

  public override SettingLevel GetLevel(string level_id)
  {
    if (this.on_level.id == level_id)
      return this.on_level;
    if (this.off_level.id == level_id)
      return this.off_level;
    if (this.default_level_id == this.on_level.id)
    {
      Debug.LogWarning((object) $"Unable to find level for setting:{this.id}({level_id}) Using default level.");
      return this.on_level;
    }
    if (this.default_level_id == this.off_level.id)
    {
      Debug.LogWarning((object) $"Unable to find level for setting:{this.id}({level_id}) Using default level.");
      return this.off_level;
    }
    Debug.LogError((object) $"Unable to find setting level for setting:{this.id} level: {level_id}");
    return (SettingLevel) null;
  }

  public override List<SettingLevel> GetLevels()
  {
    return new List<SettingLevel>()
    {
      this.off_level,
      this.on_level
    };
  }

  public string ToggleSettingLevelID(string current_id)
  {
    return this.on_level.id == current_id ? this.off_level.id : this.on_level.id;
  }

  public bool IsOnLevel(string level_id) => level_id == this.on_level.id;
}
