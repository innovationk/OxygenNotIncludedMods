// Decompiled with JetBrains decompiler
// Type: Klei.Data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using ProcGenGame;
using System.Collections.Generic;
using VoronoiTree;

#nullable disable
namespace Klei;

public class Data
{
  public int globalWorldSeed;
  public int globalWorldLayoutSeed;
  public int globalTerrainSeed;
  public int globalNoiseSeed;
  public int chunkEdgeSize = 32 /*0x20*/;
  public WorldLayout worldLayout;
  public List<TerrainCell> terrainCells;
  public List<TerrainCell> overworldCells;
  public List<BiomeSizeData> biomes;
  public List<River> rivers;
  public GameSpawnData gameSpawnData;
  public Chunk world;
  public Tree voronoiTree;
  public AxialI clusterLocation;

  public Data()
  {
    this.worldLayout = new WorldLayout((WorldGen) null, 0);
    this.terrainCells = new List<TerrainCell>();
    this.overworldCells = new List<TerrainCell>();
    this.biomes = new List<BiomeSizeData>();
    this.rivers = new List<River>();
    this.gameSpawnData = new GameSpawnData();
    this.world = new Chunk();
    this.voronoiTree = new Tree(0);
  }
}
