// Decompiled with JetBrains decompiler
// Type: ClusterFogOfWarManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class ClusterFogOfWarManager : 
  GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>
{
  public const int AUTOMATIC_PEEK_RADIUS = 2;

  public override void InitializeStates(out StateMachine.BaseState defaultState)
  {
    defaultState = (StateMachine.BaseState) this.root;
    this.root.Enter((StateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>.State.Callback) (smi => smi.Initialize())).EventHandler(GameHashes.DiscoveredWorldsChanged, (Func<ClusterFogOfWarManager.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), (StateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>.State.Callback) (smi => smi.UpdateRevealedCellsFromDiscoveredWorlds()));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance(IStateMachineTarget master, ClusterFogOfWarManager.Def def) : 
    GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>.GameInstance(master, def)
  {
    [Serialize]
    private Dictionary<AxialI, float> m_revealPointsByCell = new Dictionary<AxialI, float>();

    public void Initialize()
    {
      this.UpdateRevealedCellsFromDiscoveredWorlds();
      this.EnsureRevealedTilesHavePeek();
    }

    public ClusterRevealLevel GetCellRevealLevel(AxialI location)
    {
      if ((double) this.GetRevealCompleteFraction(location) >= 1.0)
        return ClusterRevealLevel.Visible;
      return (double) this.GetRevealCompleteFraction(location) > 0.0 ? ClusterRevealLevel.Peeked : ClusterRevealLevel.Hidden;
    }

    public void DEBUG_REVEAL_ENTIRE_MAP() => this.RevealLocation(AxialI.ZERO, 100);

    public bool IsLocationRevealed(AxialI location)
    {
      return ClusterGrid.Instance.IsValidCell(location) && (double) this.GetRevealCompleteFraction(location) >= 1.0;
    }

    private void EnsureRevealedTilesHavePeek()
    {
      foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> cellContent in ClusterGrid.Instance.cellContents)
      {
        if (this.IsLocationRevealed(cellContent.Key))
          this.PeekLocation(cellContent.Key, 2);
      }
    }

    public void PeekLocation(AxialI location, int radius)
    {
      foreach (AxialI pointsWithinRadiu in AxialUtil.GetAllPointsWithinRadius(location, radius))
        this.m_revealPointsByCell[pointsWithinRadiu] = !this.m_revealPointsByCell.ContainsKey(pointsWithinRadiu) ? 0.01f : Mathf.Max(this.m_revealPointsByCell[pointsWithinRadiu], 0.01f);
    }

    public void RevealLocation(AxialI location, int radius = 0, int peekRadius = 2)
    {
      if (ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(location, EntityLayer.Asteroid).Count > 0 || (UnityEngine.Object) ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != (UnityEngine.Object) null)
        radius = Mathf.Max(radius, 1);
      bool flag = false;
      foreach (AxialI pointsWithinRadiu in AxialUtil.GetAllPointsWithinRadius(location, radius))
        flag |= this.RevealCellIfValid(pointsWithinRadiu, peekRadius);
      if (!flag)
        return;
      Game.Instance.BoxingTrigger<AxialI>(-1991583975, location);
    }

    public bool EarnRevealPointsForLocation(AxialI location, float points)
    {
      Debug.Assert(ClusterGrid.Instance.IsValidCell(location), (object) $"EarnRevealPointsForLocation called with invalid location: {location}");
      if (this.IsLocationRevealed(location))
        return false;
      if (this.m_revealPointsByCell.ContainsKey(location))
      {
        this.m_revealPointsByCell[location] += points;
      }
      else
      {
        this.m_revealPointsByCell[location] = points;
        Game.Instance.BoxingTrigger<AxialI>(-1554423969, location);
      }
      if (!this.IsLocationRevealed(location))
        return false;
      this.RevealLocation(location);
      this.PeekLocation(location, 2);
      Game.Instance.BoxingTrigger<AxialI>(-1991583975, location);
      return true;
    }

    public float GetRevealCompleteFraction(AxialI location)
    {
      if (!ClusterGrid.Instance.IsValidCell(location))
        Debug.LogError((object) $"GetRevealCompleteFraction called with invalid location: {location.r}, {location.q}");
      if (DebugHandler.RevealFogOfWar)
        return 1f;
      float num;
      return this.m_revealPointsByCell.TryGetValue(location, out num) ? Mathf.Min(num / ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL, 1f) : 0.0f;
    }

    private bool RevealCellIfValid(AxialI cell, int peekRadius = 2)
    {
      if (!ClusterGrid.Instance.IsValidCell(cell) || this.IsLocationRevealed(cell))
        return false;
      this.m_revealPointsByCell[cell] = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL;
      this.PeekLocation(cell, peekRadius);
      return true;
    }

    public bool GetUnrevealedLocationWithinRadius(AxialI center, int radius, out AxialI result)
    {
      for (int radius1 = 0; radius1 <= radius; ++radius1)
      {
        foreach (AxialI axialI in AxialUtil.GetRing(center, radius1))
        {
          if (ClusterGrid.Instance.IsValidCell(axialI) && !this.IsLocationRevealed(axialI))
          {
            result = axialI;
            return true;
          }
        }
      }
      result = AxialI.ZERO;
      return false;
    }

    public void UpdateRevealedCellsFromDiscoveredWorlds()
    {
      int radius = DlcManager.IsExpansion1Active() ? 0 : 2;
      foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
      {
        if (worldContainer.IsDiscovered && !DebugHandler.RevealFogOfWar)
          this.RevealLocation(worldContainer.GetComponent<ClusterGridEntity>().Location, radius);
      }
    }
  }
}
