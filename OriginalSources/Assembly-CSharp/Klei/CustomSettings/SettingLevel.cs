// Decompiled with JetBrains decompiler
// Type: Klei.CustomSettings.SettingLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Klei.CustomSettings;

public class SettingLevel
{
  public SettingLevel(
    string id,
    string label,
    string tooltip,
    long coordinate_value = 0,
    object userdata = null)
  {
    this.id = id;
    this.label = label;
    this.tooltip = tooltip;
    this.userdata = userdata;
    this.coordinate_value = coordinate_value;
  }

  public string id { get; private set; }

  public string tooltip { get; private set; }

  public string label { get; private set; }

  public object userdata { get; private set; }

  public long coordinate_value { get; private set; }
}
