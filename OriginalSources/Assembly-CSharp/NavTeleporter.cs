// Decompiled with JetBrains decompiler
// Type: NavTeleporter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using TUNING;

#nullable disable
public class NavTeleporter : KMonoBehaviour
{
  private NavTeleporter target;
  private int lastRegisteredCell = Grid.InvalidCell;
  public CellOffset offset;
  private int overrideCell = -1;
  private ulong cellChangeHandlerID;
  private static readonly Action<object> OnCellChangedDispatcher = (Action<object>) (obj => Unsafe.As<NavTeleporter>(obj).OnCellChanged());

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetComponent<KPrefabID>().AddTag(GameTags.NavTeleporters);
    this.Register();
    this.cellChangeHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, NavTeleporter.OnCellChangedDispatcher, (object) this, "NavTeleporterCellChanged");
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    int cell = this.GetCell();
    if (cell != Grid.InvalidCell)
      Grid.HasNavTeleporter[cell] = false;
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangeHandlerID);
    this.Deregister();
    Components.NavTeleporters.Remove(this);
  }

  public void SetOverrideCell(int cell) => this.overrideCell = cell;

  public int GetCell()
  {
    return this.overrideCell >= 0 ? this.overrideCell : Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.offset);
  }

  public void TwoWayTarget(NavTeleporter nt)
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) nt != (UnityEngine.Object) null)
        nt.SetTarget((NavTeleporter) null);
      this.BreakLink();
    }
    this.target = nt;
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null))
      return;
    this.SetLink();
    if (!((UnityEngine.Object) nt != (UnityEngine.Object) null))
      return;
    nt.SetTarget(this);
  }

  public void EnableTwoWayTarget(bool enable)
  {
    if (enable)
    {
      this.target.SetLink();
      this.SetLink();
    }
    else
    {
      this.target.BreakLink();
      this.BreakLink();
    }
  }

  public void SetTarget(NavTeleporter nt)
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
      this.BreakLink();
    this.target = nt;
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null))
      return;
    this.SetLink();
  }

  private void Register()
  {
    int cell = this.GetCell();
    if (!Grid.IsValidCell(cell))
    {
      this.lastRegisteredCell = Grid.InvalidCell;
    }
    else
    {
      Grid.HasNavTeleporter[cell] = true;
      Pathfinding.Instance.AddDirtyNavGridCell(cell);
      this.lastRegisteredCell = cell;
      if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null))
        return;
      this.SetLink();
    }
  }

  private void SetLink()
  {
    int cell = this.target.GetCell();
    Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions[this.lastRegisteredCell] = cell;
    Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
  }

  public void Deregister()
  {
    if (this.lastRegisteredCell == Grid.InvalidCell)
      return;
    this.BreakLink();
    Grid.HasNavTeleporter[this.lastRegisteredCell] = false;
    Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
    this.lastRegisteredCell = Grid.InvalidCell;
  }

  private void BreakLink()
  {
    Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions.Remove(this.lastRegisteredCell);
    Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
  }

  private void OnCellChanged()
  {
    this.Deregister();
    this.Register();
    if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null))
      return;
    NavTeleporter component = this.target.GetComponent<NavTeleporter>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetTarget(this);
  }
}
