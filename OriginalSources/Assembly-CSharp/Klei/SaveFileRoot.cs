// Decompiled with JetBrains decompiler
// Type: Klei.SaveFileRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Klei;

internal class SaveFileRoot
{
  public int WidthInCells;
  public int HeightInCells;
  public Dictionary<string, byte[]> streamed;
  public string clusterID;
  public List<ModInfo> requiredMods;
  public List<KMod.Label> active_mods;

  public SaveFileRoot() => this.streamed = new Dictionary<string, byte[]>();
}
