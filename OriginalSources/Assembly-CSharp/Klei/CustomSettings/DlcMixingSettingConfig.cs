// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.DlcMixingSettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Klei.CustomSettings;

public class DlcMixingSettingConfig : ToggleSettingConfig
{
  private const int COORDINATE_RANGE = 5;
  public const string DisabledLevelId = "Disabled";
  public const string EnabledLevelId = "Enabled";

  public virtual string dlcIdFrom { get; protected set; }

  public DlcMixingSettingConfig(
    string id,
    string label,
    string tooltip,
    long coordinate_range = 5,
    bool triggers_custom_game = false,
    string[] required_content = null,
    string dlcIdFrom = null,
    string missing_content_default = "")
    : base(id, label, tooltip, (SettingLevel) null, (SettingLevel) null, (string) null, "Disabled", coordinate_range, triggers_custom_game: triggers_custom_game, required_content: required_content, missing_content_default: missing_content_default)
  {
    this.dlcIdFrom = dlcIdFrom;
    this.StompLevels(new SettingLevel("Disabled", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.NAME, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.TOOLTIP), new SettingLevel("Enabled", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.NAME, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.TOOLTIP, 1L), "Disabled", "Disabled");
  }
}
