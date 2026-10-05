// Decompiled with JetBrains decompiler
// Type: Klei.WorldGenSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace Klei;

public class WorldGenSave
{
  public Vector2I version;
  public Data data;
  public string worldID;
  public List<string> traitIDs;
  public List<string> storyTraitIDs;

  public WorldGenSave() => this.data = new Data();
}
