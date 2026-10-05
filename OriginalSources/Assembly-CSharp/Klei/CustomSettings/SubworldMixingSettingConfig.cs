// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.SubworldMixingSettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.CustomSettings;

public class SubworldMixingSettingConfig : MixingSettingConfig
{
  private const int COORDINATE_RANGE = 5;
  public const string DisabledLevelId = "Disabled";
  public const string TryMixingLevelId = "TryMixing";
  public const string GuaranteeMixingLevelId = "GuranteeMixing";

  public override string label
  {
    get
    {
      SubworldMixingSettings subworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(this.worldgenPath);
      StringEntry result;
      return !Strings.TryGet(subworldMixingSetting.name, out result) ? subworldMixingSetting.name : (string) result;
    }
  }

  public override string tooltip
  {
    get
    {
      SubworldMixingSettings subworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(this.worldgenPath);
      StringEntry result;
      return !Strings.TryGet(subworldMixingSetting.description, out result) ? subworldMixingSetting.description : (string) result;
    }
  }

  public override Sprite icon
  {
    get
    {
      SubworldMixingSettings subworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(this.worldgenPath);
      Sprite sprite = subworldMixingSetting.icon != null ? Assets.GetSprite((HashedString) subworldMixingSetting.icon) : (Sprite) null;
      if ((Object) sprite == (Object) null)
        sprite = Assets.GetSprite((HashedString) "unknown");
      return sprite;
    }
  }

  public override List<string> forbiddenClusterTags
  {
    get => SettingsCache.GetCachedSubworldMixingSetting(this.worldgenPath).forbiddenClusterTags;
  }

  public override bool isModded
  {
    get => SettingsCache.GetCachedSubworldMixingSetting(this.worldgenPath).isModded;
  }

  public SubworldMixingSettingConfig(
    string id,
    string worldgenPath,
    string[] required_content = null,
    string dlcIdFrom = null,
    bool triggers_custom_game = true,
    long coordinate_range = 5)
    : base(id, (List<SettingLevel>) null, (string) null, (string) null, worldgenPath, coordinate_range, triggers_custom_game: triggers_custom_game, required_content: required_content)
  {
    this.dlcIdFrom = dlcIdFrom;
    this.StompLevels(new List<SettingLevel>()
    {
      new SettingLevel("Disabled", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.NAME, (string) (DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP_BASEGAME)),
      new SettingLevel("TryMixing", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.NAME, (string) (DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP_BASEGAME), 1L),
      new SettingLevel("GuranteeMixing", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, (string) (DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP_BASEGAME), 2L)
    }, "Disabled", "Disabled");
  }
}
