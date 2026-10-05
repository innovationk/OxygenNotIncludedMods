// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.CustomMixingSettingsConfigs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

#nullable disable
namespace Klei.CustomSettings;

public static class CustomMixingSettingsConfigs
{
  public static SettingConfig DLC2Mixing = (SettingConfig) new DlcMixingSettingConfig("DLC2_ID", (string) UI.DLC2.NAME, (string) UI.DLC2.MIXING_TOOLTIP, required_content: DlcManager.DLC2, dlcIdFrom: "DLC2_ID");
  public static SettingConfig DLC3Mixing = (SettingConfig) new DlcMixingSettingConfig("DLC3_ID", (string) UI.DLC3.NAME, (string) UI.DLC3.MIXING_TOOLTIP, required_content: DlcManager.DLC3, dlcIdFrom: "DLC3_ID");
  public static SettingConfig DLC4Mixing = (SettingConfig) new DlcMixingSettingConfig("DLC4_ID", (string) UI.DLC4.NAME, (string) UI.DLC4.MIXING_TOOLTIP, required_content: DlcManager.DLC4, dlcIdFrom: "DLC4_ID");
  public static SettingConfig DLC5Mixing = (SettingConfig) new DlcMixingSettingConfig("DLC5_ID", (string) UI.DLC5.NAME, (string) UI.DLC5.MIXING_TOOLTIP, required_content: DlcManager.DLC5, dlcIdFrom: "DLC5_ID");
  public static SettingConfig CeresAsteroidMixing = (SettingConfig) new WorldMixingSettingConfig(nameof (CeresAsteroidMixing), "dlc2::worldMixing/CeresMixingSettings", DlcManager.DLC2, "DLC2_ID");
  public static SettingConfig PrehistoricAsteroidMixing = (SettingConfig) new WorldMixingSettingConfig(nameof (PrehistoricAsteroidMixing), "dlc4::worldMixing/PrehistoricMixingSettings", DlcManager.DLC4, "DLC4_ID");
  public static SettingConfig AquaticAsteroidMixing = (SettingConfig) new WorldMixingSettingConfig(nameof (AquaticAsteroidMixing), "dlc5::worldMixing/AquaticMixingSettings", DlcManager.DLC5, "DLC5_ID");
  public static SettingConfig IceCavesMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (IceCavesMixing), "dlc2::subworldMixing/IceCavesMixingSettings", DlcManager.DLC2, "DLC2_ID");
  public static SettingConfig CarrotQuarryMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (CarrotQuarryMixing), "dlc2::subworldMixing/CarrotQuarryMixingSettings", DlcManager.DLC2, "DLC2_ID");
  public static SettingConfig SugarWoodsMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (SugarWoodsMixing), "dlc2::subworldMixing/SugarWoodsMixingSettings", DlcManager.DLC2, "DLC2_ID");
  public static SettingConfig GardenMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (GardenMixing), "dlc4::subworldMixing/GardenMixingSettings", DlcManager.DLC4, "DLC4_ID");
  public static SettingConfig RaptorMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (RaptorMixing), "dlc4::subworldMixing/RaptorMixingSettings", DlcManager.DLC4, "DLC4_ID");
  public static SettingConfig WetlandsMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (WetlandsMixing), "dlc4::subworldMixing/WetlandsMixingSettings", DlcManager.DLC4, "DLC4_ID");
  public static SettingConfig BeachMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (BeachMixing), "dlc5::subworldMixing/BeachMixingSettings", DlcManager.DLC5, "DLC5_ID");
  public static SettingConfig ReefMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (ReefMixing), "dlc5::subworldMixing/ReefMixingSettings", DlcManager.DLC5, "DLC5_ID");
  public static SettingConfig KelpForestMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (KelpForestMixing), "dlc5::subworldMixing/KelpForestMixingSettings", DlcManager.DLC5, "DLC5_ID");
  public static SettingConfig AbyssMixing = (SettingConfig) new SubworldMixingSettingConfig(nameof (AbyssMixing), "dlc5::subworldMixing/AbyssMixingSettings", DlcManager.DLC5, "DLC5_ID");
}
