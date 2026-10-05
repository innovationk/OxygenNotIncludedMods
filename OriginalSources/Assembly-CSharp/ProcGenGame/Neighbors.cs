// Decompiled with JetBrains decompiler
// Type: ProcGenGame.Neighbors
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

#nullable disable
namespace ProcGenGame;

[SerializationConfig(MemberSerialization.OptOut)]
public struct Neighbors
{
  public TerrainCell n0;
  public TerrainCell n1;

  public Neighbors(TerrainCell a, TerrainCell b)
  {
    Debug.Assert(a != null && b != null, (object) "NULL Neighbor");
    this.n0 = a;
    this.n1 = b;
  }
}
