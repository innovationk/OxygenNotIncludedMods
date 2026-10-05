// Decompiled with JetBrains decompiler
// Type: Rendering.World.Tile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Rendering.World;

public struct Tile(int idx, int tile_x, int tile_y, int mask_count)
{
  public int Idx = idx;
  public TileCells TileCells = new TileCells(tile_x, tile_y);
  public int MaskCount = mask_count;
}
