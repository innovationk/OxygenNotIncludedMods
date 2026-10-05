// Decompiled with JetBrains decompiler
// Type: KAnimGraphTileVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGraphTileVisualizer")]
public class KAnimGraphTileVisualizer : KMonoBehaviour, ISaveLoadable, IUtilityItem
{
  [Serialize]
  private UtilityConnections _connections;
  public bool isPhysicalBuilding;
  public bool skipCleanup;
  public bool skipRefresh;
  public KAnimGraphTileVisualizer.ConnectionSource connectionSource;
  [NonSerialized]
  public IUtilityNetworkMgr connectionManager;

  public UtilityConnections Connections
  {
    get => this._connections;
    set
    {
      this._connections = value;
      this.BoxingTrigger<UtilityConnections>(-1041684577, this._connections);
    }
  }

  public IUtilityNetworkMgr ConnectionManager
  {
    get
    {
      switch (this.connectionSource)
      {
        case KAnimGraphTileVisualizer.ConnectionSource.Gas:
          return (IUtilityNetworkMgr) Game.Instance.gasConduitSystem;
        case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
          return (IUtilityNetworkMgr) Game.Instance.liquidConduitSystem;
        case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
          return (IUtilityNetworkMgr) Game.Instance.electricalConduitSystem;
        case KAnimGraphTileVisualizer.ConnectionSource.Logic:
          return (IUtilityNetworkMgr) Game.Instance.logicCircuitSystem;
        case KAnimGraphTileVisualizer.ConnectionSource.Tube:
          return (IUtilityNetworkMgr) Game.Instance.travelTubeSystem;
        case KAnimGraphTileVisualizer.ConnectionSource.Solid:
          return (IUtilityNetworkMgr) Game.Instance.solidConduitSystem;
        default:
          return (IUtilityNetworkMgr) null;
      }
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.connectionManager = this.ConnectionManager;
    int cell = Grid.PosToCell(this.transform.GetPosition());
    this.connectionManager.SetConnections(this.Connections, cell, this.isPhysicalBuilding);
    Building component = this.GetComponent<Building>();
    TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
  }

  protected override void OnCleanUp()
  {
    if (this.connectionManager == null || this.skipCleanup)
      return;
    this.skipRefresh = true;
    int cell = Grid.PosToCell(this.transform.GetPosition());
    this.connectionManager.ClearCell(cell, this.isPhysicalBuilding);
    Building component = this.GetComponent<Building>();
    TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
  }

  [ContextMenu("Refresh")]
  public void Refresh()
  {
    if (this.connectionManager == null || this.skipRefresh)
      return;
    int cell = Grid.PosToCell(this.transform.GetPosition());
    this.Connections = this.connectionManager.GetConnections(cell, this.isPhysicalBuilding);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    string visualizerString = this.connectionManager.GetVisualizerString(cell);
    if ((UnityEngine.Object) this.GetComponent<BuildingUnderConstruction>() != (UnityEngine.Object) null && component.HasAnimation((HashedString) (visualizerString + "_place")))
      visualizerString += "_place";
    if (visualizerString == null || !(visualizerString != ""))
      return;
    component.Play((HashedString) visualizerString);
  }

  public int GetNetworkID()
  {
    UtilityNetwork network = this.GetNetwork();
    return network == null ? -1 : network.id;
  }

  private UtilityNetwork GetNetwork()
  {
    return this.connectionManager.GetNetworkForDirection(Grid.PosToCell(this.transform.GetPosition()), Direction.None);
  }

  public UtilityNetwork GetNetworkForDirection(Direction d)
  {
    return this.connectionManager.GetNetworkForDirection(Grid.PosToCell(this.transform.GetPosition()), d);
  }

  public void UpdateConnections(UtilityConnections new_connections)
  {
    this._connections = new_connections;
    if (this.connectionManager == null)
      return;
    int cell = Grid.PosToCell(this.transform.GetPosition());
    this.connectionManager.SetConnections(new_connections, cell, this.isPhysicalBuilding);
  }

  public KAnimGraphTileVisualizer GetNeighbour(Direction d)
  {
    KAnimGraphTileVisualizer neighbour = (KAnimGraphTileVisualizer) null;
    Vector2I xy;
    Grid.PosToXY(this.transform.GetPosition(), out xy);
    int cell = -1;
    switch (d)
    {
      case Direction.Up:
        if (xy.y < Grid.HeightInCells - 1)
        {
          cell = Grid.XYToCell(xy.x, xy.y + 1);
          break;
        }
        break;
      case Direction.Right:
        if (xy.x < Grid.WidthInCells - 1)
        {
          cell = Grid.XYToCell(xy.x + 1, xy.y);
          break;
        }
        break;
      case Direction.Down:
        if (xy.y > 0)
        {
          cell = Grid.XYToCell(xy.x, xy.y - 1);
          break;
        }
        break;
      case Direction.Left:
        if (xy.x > 0)
        {
          cell = Grid.XYToCell(xy.x - 1, xy.y);
          break;
        }
        break;
    }
    if (cell != -1)
    {
      ObjectLayer layer;
      switch (this.connectionSource)
      {
        case KAnimGraphTileVisualizer.ConnectionSource.Gas:
          layer = ObjectLayer.GasConduitTile;
          break;
        case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
          layer = ObjectLayer.LiquidConduitTile;
          break;
        case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
          layer = ObjectLayer.WireTile;
          break;
        case KAnimGraphTileVisualizer.ConnectionSource.Logic:
          layer = ObjectLayer.LogicWireTile;
          break;
        case KAnimGraphTileVisualizer.ConnectionSource.Tube:
          layer = ObjectLayer.TravelTubeTile;
          break;
        case KAnimGraphTileVisualizer.ConnectionSource.Solid:
          layer = ObjectLayer.SolidConduitTile;
          break;
        default:
          throw new ArgumentNullException("wtf");
      }
      GameObject gameObject = Grid.Objects[cell, (int) layer];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        neighbour = gameObject.GetComponent<KAnimGraphTileVisualizer>();
    }
    return neighbour;
  }

  public enum ConnectionSource
  {
    Gas,
    Liquid,
    Electrical,
    Logic,
    Tube,
    Solid,
  }
}
