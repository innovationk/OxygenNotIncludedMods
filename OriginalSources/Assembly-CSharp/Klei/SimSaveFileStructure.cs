// Decompiled with JetBrains decompiler
// Type: Klei.SimSaveFileStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Klei;

public class SimSaveFileStructure
{
  public int WidthInCells;
  public int HeightInCells;
  public int x;
  public int y;
  public byte[] Sim;
  public WorldDetailSave worldDetail;

  public SimSaveFileStructure() => this.worldDetail = new WorldDetailSave();
}
