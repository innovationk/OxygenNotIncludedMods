// Decompiled with JetBrains decompiler
// Type: ProcGenGame.SymbolicMapElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace ProcGenGame;

public interface SymbolicMapElement
{
  void ConvertToMap(
    Chunk world,
    TerrainCell.ISimDataSetter setter,
    float temperatureMin,
    float temperatureRange,
    SeededRandom rnd);
}
