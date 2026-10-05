// Decompiled with JetBrains decompiler
// Type: ModInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

#nullable disable
[Serializable]
public struct ModInfo
{
  [JsonConverter(typeof (StringEnumConverter))]
  public ModInfo.Source source;
  [JsonConverter(typeof (StringEnumConverter))]
  public ModInfo.ModType type;
  public string assetID;
  public string assetPath;
  public bool enabled;
  public bool markedForDelete;
  public bool markedForUpdate;
  public string description;
  public ulong lastModifiedTime;

  public enum Source
  {
    Local,
    Steam,
    Rail,
  }

  public enum ModType
  {
    WorldGen,
    Scenario,
    Mod,
  }
}
