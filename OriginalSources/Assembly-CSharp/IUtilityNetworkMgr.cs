// Decompiled with JetBrains decompiler
// Type: IUtilityNetworkMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public interface IUtilityNetworkMgr
{
  bool CanAddConnection(
    UtilityConnections new_connection,
    int cell,
    bool is_physical_building,
    out string fail_reason);

  void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building);

  void StashVisualGrids();

  void UnstashVisualGrids();

  string GetVisualizerString(int cell);

  string GetVisualizerString(UtilityConnections connections);

  UtilityConnections GetConnections(int cell, bool is_physical_building);

  UtilityConnections GetDisplayConnections(int cell);

  void SetConnections(UtilityConnections connections, int cell, bool is_physical_building);

  void ClearCell(int cell, bool is_physical_building);

  void ForceRebuildNetworks();

  void AddToNetworks(int cell, object item, bool is_endpoint);

  void RemoveFromNetworks(int cell, object vent, bool is_endpoint);

  object GetEndpoint(int cell);

  UtilityNetwork GetNetworkForDirection(int cell, Direction direction);

  UtilityNetwork GetNetworkForCell(int cell);

  void AddNetworksRebuiltListener(
    Action<IList<UtilityNetwork>, ICollection<int>> listener);

  void RemoveNetworksRebuiltListener(
    Action<IList<UtilityNetwork>, ICollection<int>> listener);

  IList<UtilityNetwork> GetNetworks();
}
