// Decompiled with JetBrains decompiler
// Type: ProcGenGame.WorldgenSimData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace ProcGenGame;

public struct WorldgenSimData
{
  public Sim.Cell[] cells;
  public Sim.DiseaseCell[] diseaseCells;
  public Sim.SimBackwall[] backwallCells;

  public void Init(int cellCount)
  {
    this.cells = new Sim.Cell[cellCount];
    this.diseaseCells = new Sim.DiseaseCell[cellCount];
    this.backwallCells = new Sim.SimBackwall[cellCount];
    ushort elementIndex = ElementLoader.GetElementIndex(SimHashes.Vacuum);
    for (int index = 0; index < cellCount; ++index)
      this.backwallCells[index].elementIdx = elementIndex;
  }
}
