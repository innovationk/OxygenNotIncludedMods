// Decompiled with JetBrains decompiler
// Type: BipedSwimTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BipedSwimTransitionLayer : TransitionDriver.OverrideLayer
{
  private Vector3 offset;
  private KBatchedAnimController animcontroller;
  private bool lerpingOffset;
  private float startOffsetY;
  private float targetOffsetY;
  private Vector3 startPos;
  private Vector3 endPos;

  public BipedSwimTransitionLayer(Navigator navigator)
    : base(navigator)
  {
    this.animcontroller = navigator.GetComponent<KBatchedAnimController>();
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    this.lerpingOffset = false;
    int cell = Grid.CellAbove(navigator.cachedCell);
    bool flag1 = Grid.IsWorldValidCell(cell) && !Grid.IsLiquid(cell);
    int num = transition.start != NavType.Swim ? 0 : (transition.end == NavType.Swim ? 1 : 0);
    bool flag2 = transition.x != 0 && transition.y != 0;
    if ((num & (transition.x == 0 ? (false ? 1 : 0) : (transition.y == 0 ? 1 : 0)) & (flag1 ? 1 : 0)) != 0)
    {
      transition.anim = (HashedString) "shallow_swim_1_0_loop";
      transition.isLooping = true;
      this.SetupOffsets(navigator, transition);
    }
    else
    {
      Dictionary<HashedString, HashedString> dictionary;
      HashedString hashedString;
      if (this.animcontroller.currentAnim != transition.anim && SwimMonitor.transitionAnims.TryGetValue(this.animcontroller.currentAnim, out dictionary) && dictionary.TryGetValue(transition.anim, out hashedString))
        transition.preAnim = hashedString;
    }
    if (num == 0 || transition.isLooping)
      return;
    if ((double) transition.speed > 0.0)
      transition.animSpeed = transition.speed;
    if (!flag2)
      return;
    transition.animSpeed *= 0.9f;
  }

  public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    if (transition.start == NavType.Swim && transition.end == NavType.Swim && this.lerpingOffset)
    {
      Vector3 position = navigator.transform.GetPosition();
      float num1 = Vector3.Distance(this.startPos, this.endPos);
      float num2 = Vector3.Distance(this.startPos, position);
      this.offset.y = Mathf.Lerp(this.startOffsetY, this.targetOffsetY, (double) num1 > 0.0 ? Mathf.Clamp01(num2 / num1) : 1f);
      if ((double) MathF.Abs(this.offset.y - this.animcontroller.Offset.y) > (double) SwimMonitor.OffsetEpsilon)
        this.animcontroller.Offset = this.offset;
    }
    base.UpdateTransition(navigator, transition);
  }

  private void SetupOffsets(Navigator navigator, Navigator.ActiveTransition transition)
  {
    int cachedCell = navigator.cachedCell;
    int cell = Grid.OffsetCell(cachedCell, transition.x, transition.y);
    this.startOffsetY = SwimMonitor.ComputeSwimOffsetY(cachedCell);
    this.targetOffsetY = BipedSwimTransitionLayer.IsSurfaceSwimCell(cell) ? SwimMonitor.ComputeSwimOffsetY(cell) : 0.0f;
    this.startPos = navigator.transform.GetPosition();
    this.endPos = Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
    this.lerpingOffset = true;
    this.offset.y = this.startOffsetY;
    this.animcontroller.Offset = this.offset;
  }

  private static bool IsSurfaceSwimCell(int cell)
  {
    if (!Grid.IsWorldValidCell(cell) || !Grid.IsLiquid(cell))
      return false;
    int cell1 = Grid.CellAbove(cell);
    return Grid.IsWorldValidCell(cell1) && !Grid.IsLiquid(cell1);
  }

  public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    this.lerpingOffset = false;
    base.EndTransition(navigator, transition);
    if ((double) MathF.Abs(this.animcontroller.Offset.y) <= (double) SwimMonitor.OffsetEpsilon)
      return;
    this.offset = Vector3.zero;
    this.animcontroller.Offset = this.offset;
  }
}
