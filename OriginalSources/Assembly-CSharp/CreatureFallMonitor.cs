// Decompiled with JetBrains decompiler
// Type: CreatureFallMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CreatureFallMonitor : 
  GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>
{
  public static float FLOOR_DISTANCE = -0.065f;
  private const float SWIM_SETTLE_EPSILON = 0.1f;
  private const float SWIM_MAX_FALL_SPEED = 2f;
  private static readonly NavType[] SNAP_NAV_TYPES = new NavType[3]
  {
    NavType.Floor,
    NavType.Hover,
    NavType.Swim
  };
  private static readonly NavType[] WALL_CRAWLER_NAV_TYPES = new NavType[3]
  {
    NavType.Ceiling,
    NavType.LeftWall,
    NavType.RightWall
  };
  public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State grounded;
  public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State falling;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.grounded;
    this.grounded.ToggleBehaviour(GameTags.Creatures.Falling, (StateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.Transition.ConditionCallback) (smi => smi.ShouldFall()));
  }

  public class Def : StateMachine.BaseDef
  {
    public bool canSwim;
  }

  public new class Instance(IStateMachineTarget master, CreatureFallMonitor.Def def) : 
    GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.GameInstance(master, def)
  {
    public string anim = "fall";
    [MyCmpReq]
    private KPrefabID kprefabId;
    [MyCmpReq]
    private Navigator navigator;

    private Vector3 GetNavAnchor(Vector3 pos)
    {
      Vector3 controllerOffset = (Vector3) this.navigator.NavGrid.GetNavTypeData(this.navigator.CurrentNavType).animControllerOffset;
      return pos - controllerOffset;
    }

    public void SnapToGround()
    {
      Vector3 navAnchor = this.GetNavAnchor(this.smi.transform.GetPosition());
      this.smi.transform.SetPosition(Grid.CellToPosCBC(Grid.PosToCell(navAnchor), Grid.SceneLayer.Creatures) with
      {
        x = navAnchor.x
      });
      foreach (NavType nav_type in CreatureFallMonitor.SNAP_NAV_TYPES)
      {
        if (this.navigator.IsValidNavType(nav_type))
        {
          this.navigator.SetCurrentNavType(nav_type);
          break;
        }
      }
    }

    public bool ShouldFall()
    {
      if (this.kprefabId.HasTag(GameTags.Stored))
        return false;
      Vector3 position = this.smi.transform.GetPosition();
      int cell1 = Grid.PosToCell(position);
      if ((!Grid.IsValidCell(cell1) ? 0 : (Grid.Solid[cell1] ? 1 : 0)) != 0 || this.navigator.IsMoving() || this.ShouldSettleIntoSwim(position))
        return false;
      if (this.navigator.CurrentNavType != NavType.Swim)
      {
        if (this.navigator.NavGrid.NavTable.IsValid(cell1, this.navigator.CurrentNavType))
          return false;
        foreach (NavType navType in CreatureFallMonitor.WALL_CRAWLER_NAV_TYPES)
        {
          if (this.navigator.CurrentNavType == navType)
            return true;
        }
      }
      Vector3 pos = position;
      pos.y += CreatureFallMonitor.FLOOR_DISTANCE;
      int cell2 = Grid.PosToCell(pos);
      return (!Grid.IsValidCell(cell2) ? 0 : (Grid.Solid[cell2] ? 1 : 0)) == 0;
    }

    public bool CanSwimAtCurrentLocation()
    {
      return this.CanSwimAtCell(Grid.PosToCell(this.transform.GetPosition()));
    }

    private bool CanSwimAtCell(int cell)
    {
      return this.def.canSwim && this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Swim) && (!GameComps.Gravities.Has((object) this.gameObject) || (double) GameComps.Gravities.GetData(GameComps.Gravities.GetHandle(this.gameObject)).velocity.magnitude < 2.0);
    }

    public bool ShouldSettleIntoSwim() => this.ShouldSettleIntoSwim(this.transform.GetPosition());

    private bool ShouldSettleIntoSwim(Vector3 pos)
    {
      Vector3 navAnchor = this.GetNavAnchor(pos);
      int cell = Grid.PosToCell(navAnchor);
      if (!this.CanSwimAtCell(cell))
        return false;
      float y = Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures).y;
      return (double) navAnchor.y <= (double) y + 0.10000000149011612;
    }
  }
}
