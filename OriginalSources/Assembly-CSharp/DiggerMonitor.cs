// Decompiled with JetBrains decompiler
// Type: DiggerMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using ProcGen;
using System;
using UnityEngine;

#nullable disable
public class DiggerMonitor : 
  GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>
{
  public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State loop;
  public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State dig;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.loop;
    this.loop.EventTransition(GameHashes.BeginMeteorBombardment, (Func<DiggerMonitor.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) Game.Instance), this.dig, (StateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.Transition.ConditionCallback) (smi => smi.CanTunnel()));
    this.dig.ToggleBehaviour(GameTags.Creatures.Tunnel, (StateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.Transition.ConditionCallback) (smi => true)).GoTo(this.loop);
  }

  public class Def : StateMachine.BaseDef
  {
    public int depthToDig { get; set; }
  }

  public new class Instance : 
    GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.GameInstance
  {
    [Serialize]
    public int lastDigCell = -1;
    private System.Action<object> OnDestinationReachedDelegate;
    private static Func<int, bool> isValidDigCell = new Func<int, bool>(DiggerMonitor.Instance.IsValidDigCell);

    public Instance(IStateMachineTarget master, DiggerMonitor.Def def)
      : base(master, def)
    {
      World.Instance.OnSolidChanged += new System.Action<int>(this.OnSolidChanged);
      this.OnDestinationReachedDelegate = new System.Action<object>(this.OnDestinationReached);
      master.Subscribe(387220196, this.OnDestinationReachedDelegate);
      master.Subscribe(-766531887, this.OnDestinationReachedDelegate);
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      World.Instance.OnSolidChanged -= new System.Action<int>(this.OnSolidChanged);
      this.master.Unsubscribe(387220196, this.OnDestinationReachedDelegate);
      this.master.Unsubscribe(-766531887, this.OnDestinationReachedDelegate);
    }

    private void OnDestinationReached(object data) => this.CheckInSolid();

    private void CheckInSolid()
    {
      Navigator component = this.gameObject.GetComponent<Navigator>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      int cell = Grid.PosToCell(this.gameObject);
      if (component.CurrentNavType != NavType.Solid && Grid.IsSolidCell(cell))
      {
        component.SetCurrentNavType(NavType.Solid);
      }
      else
      {
        if (component.CurrentNavType != NavType.Solid || Grid.IsSolidCell(cell))
          return;
        component.SetCurrentNavType(NavType.Floor);
        this.gameObject.AddTag(GameTags.Creatures.Falling);
      }
    }

    private void OnSolidChanged(int cell) => this.CheckInSolid();

    public bool CanTunnel()
    {
      int cell = Grid.PosToCell((StateMachine.Instance) this);
      if (World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space)
      {
        int num = cell;
        while (Grid.IsValidCell(num) && !Grid.Solid[num])
          num = Grid.CellAbove(num);
        if (!Grid.IsValidCell(num))
          return this.FoundValidDigCell();
      }
      return false;
    }

    private bool FoundValidDigCell()
    {
      int depthToDig = this.smi.def.depthToDig;
      int cell1 = Grid.PosToCell(this.smi.master.gameObject);
      this.lastDigCell = cell1;
      int cell2;
      for (cell2 = Grid.CellBelow(cell1); DiggerMonitor.Instance.IsValidDigCell(cell2) && depthToDig > 0; --depthToDig)
        cell2 = Grid.CellBelow(cell2);
      if (depthToDig > 0)
        cell2 = FloodFill.Find<FloodFill.MaxDepth>(DiggerMonitor.Instance.isValidDigCell, cell1, new FloodFill.MaxDepth(this.smi.def.depthToDig), false, true);
      this.lastDigCell = cell2;
      return this.lastDigCell != -1;
    }

    private static bool IsValidDigCell(int cell)
    {
      if (Grid.IsValidCell(cell) && Grid.Solid[cell])
      {
        if (Grid.HasDoor[cell] || Grid.Foundation[cell])
        {
          GameObject gameObject = Grid.Objects[cell, 1];
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          {
            PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
            return Grid.Element[cell].hardness < (byte) 150 && !component.Element.HasTag(GameTags.RefinedMetal);
          }
        }
        else
        {
          ushort index = Grid.ElementIdx[cell];
          Element element = ElementLoader.elements[(int) index];
          return Grid.Element[cell].hardness < (byte) 150 && !element.HasTag(GameTags.RefinedMetal);
        }
      }
      return false;
    }
  }
}
