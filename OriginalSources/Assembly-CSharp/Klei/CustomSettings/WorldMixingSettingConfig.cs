// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.WorldMixingSettingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.CustomSettings;

public class WorldMixingSettingConfig : MixingSettingConfig
{
  private const int COORDINATE_RANGE = 5;
  public const string DisabledLevelId = "Disabled";
  public const string TryMixingLevelId = "TryMixing";
  public const string GuaranteeMixingLevelId = "GuranteeMixing";

  public override string label
  {
    get
    {
      WorldMixingSettings worldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(this.worldgenPath);
      StringEntry result;
      return !Strings.TryGet(worldMixingSetting.name, out result) ? worldMixingSetting.name : (string) result;
    }
  }

  public override string tooltip
  {
    get
    {
      WorldMixingSettings worldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(this.worldgenPath);
      StringEntry result;
      return !Strings.TryGet(worldMixingSetting.description, out result) ? worldMixingSetting.description : (string) result;
    }
  }

  public override Sprite icon
  {
    get
    {
      WorldMixingSettings worldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(this.worldgenPath);
      Sprite icon = worldMixingSetting.icon != null ? ColonyDestinationAsteroidBeltData.GetUISprite(worldMixingSetting.icon) : (Sprite) null;
      if ((Object) icon == (Object) null)
        icon = Assets.GetSprite((HashedString) worldMixingSetting.icon);
      if ((Object) icon == (Object) null)
        icon = Assets.GetSprite((HashedString) "unknown");
      return icon;
    }
  }

  public override List<string> forbiddenClusterTags
  {
    get => SettingsCache.GetCachedWorldMixingSetting(this.worldgenPath).forbiddenClusterTags;
  }

  public override bool isModded
  {
    get => SettingsCache.GetCachedWorldMixingSetting(this.worldgenPath).isModded;
  }

  public WorldMixingSettingConfig(
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
      new SettingLevel("Disabled", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.NAME, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.TOOLTIP),
      new SettingLevel("TryMixing", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.NAME, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP, 1L),
      new SettingLevel("GuranteeMixing", (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP, 2L)
    }, "Disabled", "Disabled");
  }
}
