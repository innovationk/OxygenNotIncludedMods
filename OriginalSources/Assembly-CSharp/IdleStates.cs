// Decompiled with JetBrains decompiler
// Type: IdleStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;

#nullable disable
public class IdleStates : 
  GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>
{
  private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State loop;
  private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State move;
  public static StatusItem IdleStatus = new StatusItem(nameof (IdleStatus), (string) CREATURES.STATUSITEMS.IDLE.NAME, (string) CREATURES.STATUSITEMS.IDLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Messages, false, OverlayModes.None.ID);

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    this.root.Exit("StopNavigator", new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(IdleStates.StopNavigator)).ToggleMainStatusItem(IdleStates.IdleStatus).ToggleTag(GameTags.Idle);
    this.loop.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(IdleStates.PlayIdle)).ToggleScheduleCallback("IdleMove", new Func<IdleStates.Instance, float>(IdleStates.GetIdleTime), new System.Action<IdleStates.Instance>(IdleStates.GoMove));
    this.move.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(IdleStates.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.loop).EventTransition(GameHashes.NavigationFailed, this.loop);
  }

  private static float GetIdleTime(IdleStates.Instance smi) => (float) UnityEngine.Random.Range(3, 10);

  private static void GoMove(IdleStates.Instance smi)
  {
    smi.GoTo((StateMachine.BaseState) smi.sm.move);
  }

  private static void StopNavigator(IdleStates.Instance smi) => smi.navigator.Stop();

  private static void MoveToNewCell(IdleStates.Instance smi)
  {
    if (smi.kpid.HasTag(GameTags.StationaryIdling))
    {
      smi.GoTo((StateMachine.BaseState) smi.sm.loop);
    }
    else
    {
      IdleStates.MoveCellQuery instance = IdleStates.MoveCellQuery.Instance;
      instance.Reset(smi.navigator.CurrentNavType);
      instance.allowLiquid = smi.kpid.HasTag(GameTags.Amphibious);
      instance.submerged = smi.kpid.HasTag(GameTags.Creatures.Submerged);
      int cell1 = Grid.PosToCell((KMonoBehaviour) smi.navigator);
      if (smi.navigator.CurrentNavType == NavType.Hover && CellSelectionObject.IsExposedToSpace(cell1))
      {
        int num = 0;
        int cell2 = cell1;
        for (int index = 0; index < 10; ++index)
        {
          cell2 = Grid.CellBelow(cell2);
          if (Grid.IsValidCell(cell2) && !Grid.IsSolidCell(cell2) && CellSelectionObject.IsExposedToSpace(cell2))
            ++num;
          else
            break;
        }
        instance.lowerCellBias = num == 10;
      }
      smi.navigator.RunQuery((PathFinderQuery) instance);
      if (smi.navigator.CanReach(instance.GetResultCell()))
        smi.navigator.GoTo(instance.GetResultCell());
      else
        smi.GoTo((StateMachine.BaseState) smi.sm.loop);
    }
  }

  private static void PlayIdle(IdleStates.Instance smi)
  {
    NavType nav_type = smi.navigator.CurrentNavType;
    if (smi.facing.GetFacing())
      nav_type = NavGrid.MirrorNavType(nav_type);
    if (smi.def.customIdleAnim != null)
    {
      HashedString invalid = HashedString.Invalid;
      HashedString anim_name = smi.def.customIdleAnim(smi, ref invalid);
      if (anim_name != HashedString.Invalid)
      {
        if (invalid != HashedString.Invalid)
          smi.kac.Play(invalid);
        smi.kac.Queue(anim_name, KAnim.PlayMode.Loop);
        return;
      }
    }
    HashedString idleAnim = smi.navigator.NavGrid.GetIdleAnim(nav_type);
    smi.kac.Play(idleAnim, KAnim.PlayMode.Loop);
  }

  public class Def : StateMachine.BaseDef
  {
    public IdleStates.Def.IdleAnimCallback customIdleAnim;
    public PriorityScreen.PriorityClass priorityClass;

    public delegate HashedString IdleAnimCallback(
      IdleStates.Instance smi,
      ref HashedString pre_anim);
  }

  public new class Instance : 
    GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.GameInstance
  {
    public Navigator navigator;
    public KPrefabID kpid;
    public KBatchedAnimController kac;
    public Facing facing;

    public Instance(Chore<IdleStates.Instance> chore, IdleStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      this.navigator = this.GetComponent<Navigator>();
      this.kpid = this.GetComponent<KPrefabID>();
      this.kac = this.GetComponent<KBatchedAnimController>();
      this.facing = this.GetComponent<Facing>();
      chore.masterPriority.priority_class = def.priorityClass;
    }
  }

  public class MoveCellQuery : PathFinderQuery
  {
    private NavType navType;
    private int targetCell = Grid.InvalidCell;
    private int maxIterations;
    public static IdleStates.MoveCellQuery Instance = new IdleStates.MoveCellQuery(NavType.Floor);

    public bool allowLiquid { get; set; }

    public bool submerged { get; set; }

    public bool lowerCellBias { get; set; }

    public MoveCellQuery(NavType navType) => this.Reset(navType);

    public void Reset(NavType navType)
    {
      this.navType = navType;
      this.maxIterations = UnityEngine.Random.Range(5, 25);
      this.targetCell = Grid.InvalidCell;
      this.allowLiquid = false;
      this.submerged = false;
      this.lowerCellBias = false;
    }

    public override bool IsMatch(int cell, int parent_cell, int cost)
    {
      if (!Grid.IsValidCell(cell) || Grid.ObjectLayers[9].ContainsKey(cell))
        return false;
      bool flag1 = this.submerged || Grid.IsNavigatableLiquid(cell);
      bool flag2 = this.navType != NavType.Swim;
      bool flag3 = this.navType == NavType.Swim || this.allowLiquid;
      if (flag1 && !flag3 || !flag1 && !flag2)
        return false;
      if (this.targetCell == Grid.InvalidCell || !this.lowerCellBias)
      {
        this.targetCell = cell;
      }
      else
      {
        int num = Grid.CellRow(this.targetCell);
        if (Grid.CellRow(cell) < num)
          this.targetCell = cell;
      }
      return --this.maxIterations <= 0;
    }

    public override int GetResultCell() => this.targetCell;
  }
}
