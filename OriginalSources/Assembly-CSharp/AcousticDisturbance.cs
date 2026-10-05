// Decompiled with JetBrains decompiler
// Type: AcousticDisturbance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;
using UnityEngine;

#nullable disable
public class AcousticDisturbance
{
  private static readonly HashedString[] PreAnims = new HashedString[2]
  {
    (HashedString) "grid_pre",
    (HashedString) "grid_loop"
  };
  private static readonly HashedString PostAnim = (HashedString) "grid_pst";
  private static float distanceDelay = 0.25f;
  private static float duration = 3f;
  private static readonly Func<int, FloodFill.BoundaryCheckResult> notSolid = (Func<int, FloodFill.BoundaryCheckResult>) (cell => !Grid.Solid[cell] ? FloodFill.BoundaryCheckResult.Continue : FloodFill.BoundaryCheckResult.Halt);
  private static readonly HybridListHashSet<int> cellsInRange = new HybridListHashSet<int>();

  public static void Emit(object data, int EmissionRadius)
  {
    GameObject gameObject = (GameObject) data;
    Components.Cmps<MinionIdentity> minionIdentities = Components.LiveMinionIdentities;
    Vector2 position1 = (Vector2) gameObject.transform.GetPosition();
    int cell1 = Grid.PosToCell(position1);
    int num = EmissionRadius * EmissionRadius;
    AcousticDisturbance.cellsInRange.Clear();
    FloodFill.DepthTraverse<FloodFill.PredicateCondition, FloodFill.HashSetVisitTracker, FloodFill.MaxDepth, AcousticDisturbance.CellCollector>(cell1, new FloodFill.PredicateCondition(AcousticDisturbance.notSolid), FloodFill.HashSetVisitTracker.Default(), new FloodFill.MaxDepth(EmissionRadius), new AcousticDisturbance.CellCollector());
    AcousticDisturbance.DrawVisualEffect(cell1, AcousticDisturbance.cellsInRange);
    for (int idx = 0; idx < minionIdentities.Count; ++idx)
    {
      MinionIdentity cmp = minionIdentities[idx];
      if (!((UnityEngine.Object) cmp.gameObject == (UnityEngine.Object) gameObject.gameObject))
      {
        Vector2 position2 = (Vector2) cmp.transform.GetPosition();
        if ((double) Vector2.SqrMagnitude(position1 - position2) <= (double) num)
        {
          int cell2 = Grid.PosToCell(position2);
          if (AcousticDisturbance.cellsInRange.Contains(cell2))
          {
            StaminaMonitor.Instance smi = cmp.GetSMI<StaminaMonitor.Instance>();
            if (smi != null && smi.IsSleeping())
            {
              cmp.Trigger(-527751701, data);
              cmp.Trigger(1621815900, data);
            }
          }
        }
      }
    }
  }

  private static void DrawVisualEffect(int center_cell, HybridListHashSet<int> cells)
  {
    SoundEvent.PlayOneShot(GlobalResources.Instance().AcousticDisturbanceSound, Grid.CellToPos(center_cell));
    for (int index = 0; index != cells.Count; ++index)
    {
      int cell = cells[index];
      int gridDistance = AcousticDisturbance.GetGridDistance(cell, center_cell);
      GameScheduler.Instance.Schedule("radialgrid_pre", AcousticDisturbance.distanceDelay * (float) gridDistance, new Action<object>(AcousticDisturbance.SpawnEffect), (object) cell, (SchedulerGroup) null);
    }
  }

  private static void SpawnEffect(object data)
  {
    Grid.SceneLayer layer = Grid.SceneLayer.InteriorWall;
    KBatchedAnimController effect = FXHelpers.CreateEffect("radialgrid_kanim", Grid.CellToPosCCC((int) data, layer), layer: layer);
    effect.destroyOnAnimComplete = false;
    effect.Play(AcousticDisturbance.PreAnims, KAnim.PlayMode.Loop);
    GameScheduler.Instance.Schedule("radialgrid_loop", AcousticDisturbance.duration, new Action<object>(AcousticDisturbance.DestroyEffect), (object) effect, (SchedulerGroup) null);
  }

  private static void DestroyEffect(object data)
  {
    KBatchedAnimController kbatchedAnimController = (KBatchedAnimController) data;
    kbatchedAnimController.destroyOnAnimComplete = true;
    kbatchedAnimController.Play(AcousticDisturbance.PostAnim);
  }

  private static int GetGridDistance(int cell, int center_cell)
  {
    Vector2I vector2I = Grid.CellToXY(cell) - Grid.CellToXY(center_cell);
    return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
  }

  [StructLayout(LayoutKind.Sequential, Size = 1)]
  private readonly struct CellCollector : FloodFill.IVisitor
  {
    public bool EarlyOut => false;

    public void VisitCell(int cell) => AcousticDisturbance.cellsInRange.Add(cell);

    public void VisitBoundary(int cell)
    {
    }
  }
}
