// Decompiled with JetBrains decompiler
// Type: ExposureType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ExposureType
{
  public string germ_id;
  public string sickness_id;
  public string infection_effect;
  public int exposure_threshold;
  public bool infect_immediately;
  public List<string> required_traits;
  public List<string> excluded_traits;
  public List<string> excluded_effects;
  public int base_resistance;
}
