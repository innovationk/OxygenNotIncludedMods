// Decompiled with JetBrains decompiler
// Type: FogOfWarMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/FogOfWarMask")]
public class FogOfWarMask : KMonoBehaviour
{
  private static readonly Func<int, FloodFill.BoundaryCheckResult> revealFogOfWarMask = new Func<int, FloodFill.BoundaryCheckResult>(FogOfWarMask.RevealFogOfWarMask);

  protected override void OnSpawn()
  {
    Debug.Assert(false, (object) "Unmaintained, presumed dead, code is being invoked!");
  }

  protected override void OnCmpEnable()
  {
    Debug.Assert(false, (object) "Unmaintained, presumed dead, code is being invoked!");
  }

  public static void ClearMask(int cell)
  {
    FloodFill.BreadthTraverse<FloodFill.PredicateCondition, FloodFill.HashSetVisitTracker, FloodFill.NoMaxDepth, FogOfWarMask.ThresholdVisitor>(cell, new FloodFill.PredicateCondition(FogOfWarMask.revealFogOfWarMask), FloodFill.HashSetVisitTracker.Default(), new FloodFill.NoMaxDepth(), new FogOfWarMask.ThresholdVisitor(300));
  }

  public static FloodFill.BoundaryCheckResult RevealFogOfWarMask(int cell)
  {
    if (!Grid.PreventFogOfWarReveal[cell])
      return FloodFill.BoundaryCheckResult.Halt;
    Grid.PreventFogOfWarReveal[cell] = false;
    Grid.Reveal(cell);
    return FloodFill.BoundaryCheckResult.Continue;
  }

  private struct ThresholdVisitor(int threshold) : FloodFill.IVisitor
  {
    private int threshold = threshold;

    public readonly bool EarlyOut => this.threshold <= 0;

    public void VisitCell(int cell) => --this.threshold;

    public readonly void VisitBoundary(int cell)
    {
    }
  }
}
