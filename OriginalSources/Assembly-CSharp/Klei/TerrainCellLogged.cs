// Decompiled with JetBrains decompiler
// Type: Klei.TerrainCellLogged
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGenGame;
using System.Collections.Generic;
using VoronoiTree;

#nullable disable
namespace Klei;

public class TerrainCellLogged : TerrainCell
{
  public TerrainCellLogged()
  {
  }

  public TerrainCellLogged(ProcGen.Map.Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags)
    : base(node, site, distancesToTags)
  {
  }

  public override void LogInfo(string evt, string param, float value)
  {
  }
}
