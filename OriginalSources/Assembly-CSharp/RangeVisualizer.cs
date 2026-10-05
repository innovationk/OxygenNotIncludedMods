// Decompiled with JetBrains decompiler
// Type: RangeVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/RangeVisualizer")]
public class RangeVisualizer : KMonoBehaviour
{
  public Vector2I OriginOffset;
  public Vector2I RangeMin;
  public Vector2I RangeMax;
  public Vector2I TexSize = new Vector2I(64 /*0x40*/, 64 /*0x40*/);
  public bool TestLineOfSight = true;
  public bool BlockingTileVisible;
  public Func<int, bool> BlockingVisibleCb;
  public Func<int, bool> BlockingCb = new Func<int, bool>(Grid.IsSolidCell);
  public bool AllowLineOfSightInvalidCells;
}
