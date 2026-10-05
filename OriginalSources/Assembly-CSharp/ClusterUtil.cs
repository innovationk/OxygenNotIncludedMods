// Decompiled with JetBrains decompiler
// Type: ClusterUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class ClusterUtil
{
  public static WorldContainer GetMyWorld(this StateMachine.Instance smi)
  {
    return smi.GetComponent<StateMachineController>().GetMyWorld();
  }

  public static WorldContainer GetMyWorld(this KMonoBehaviour component)
  {
    return component.gameObject.GetMyWorld();
  }

  public static WorldContainer GetMyWorld(this GameObject gameObject)
  {
    int cell = Grid.PosToCell(gameObject);
    return Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != byte.MaxValue ? ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[cell]) : (WorldContainer) null;
  }

  public static int GetMyWorldId(this StateMachine.Instance smi)
  {
    return smi.GetComponent<StateMachineController>().GetMyWorldId();
  }

  public static int GetMyWorldId(this KMonoBehaviour component)
  {
    return component.gameObject.GetMyWorldId();
  }

  public static int GetMyWorldId(this GameObject gameObject)
  {
    int cell = Grid.PosToCell(gameObject);
    return Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != byte.MaxValue ? (int) Grid.WorldIdx[cell] : -1;
  }

  public static int GetMyParentWorldId(this StateMachine.Instance smi)
  {
    return smi.GetComponent<StateMachineController>().GetMyParentWorldId();
  }

  public static int GetMyParentWorldId(this KMonoBehaviour component)
  {
    return component.gameObject.GetMyParentWorldId();
  }

  public static int GetMyParentWorldId(this GameObject gameObject)
  {
    WorldContainer myWorld = gameObject.GetMyWorld();
    return (Object) myWorld == (Object) null ? gameObject.GetMyWorldId() : myWorld.ParentWorldId;
  }

  public static AxialI GetMyWorldLocation(this StateMachine.Instance smi)
  {
    return smi.GetComponent<StateMachineController>().GetMyWorldLocation();
  }

  public static AxialI GetMyWorldLocation(this KMonoBehaviour component)
  {
    return component.gameObject.GetMyWorldLocation();
  }

  public static AxialI GetMyWorldLocation(this GameObject gameObject)
  {
    ClusterGridEntity component1 = gameObject.GetComponent<ClusterGridEntity>();
    if ((Object) component1 != (Object) null)
      return component1.Location;
    RocketModuleCluster component2 = gameObject.GetComponent<RocketModuleCluster>();
    if ((Object) component2 != (Object) null)
      return component2.CraftInterface.GetMyWorldLocation();
    WorldContainer myWorld = gameObject.GetMyWorld();
    DebugUtil.DevAssertArgs(((Object) myWorld != (Object) null ? 1 : 0) != 0, (object) "GetMyWorldLocation called on object with no world", (object) gameObject);
    return myWorld.GetComponent<ClusterGridEntity>().Location;
  }

  public static bool IsMyWorld(this GameObject go, GameObject otherGo)
  {
    int cell = Grid.PosToCell(otherGo);
    return go.IsMyWorld(cell);
  }

  public static bool IsMyWorld(this GameObject go, int otherCell)
  {
    int cell = Grid.PosToCell(go);
    return Grid.IsValidCell(cell) && Grid.IsValidCell(otherCell) && (int) Grid.WorldIdx[cell] == (int) Grid.WorldIdx[otherCell];
  }

  public static bool IsMyParentWorld(this GameObject go, GameObject otherGo)
  {
    int cell = Grid.PosToCell(otherGo);
    return go.IsMyParentWorld(cell);
  }

  public static bool IsMyParentWorld(this GameObject go, int otherCell)
  {
    int cell = Grid.PosToCell(go);
    if (Grid.IsValidCell(cell) && Grid.IsValidCell(otherCell))
    {
      if ((int) Grid.WorldIdx[cell] == (int) Grid.WorldIdx[otherCell])
        return true;
      WorldContainer world1 = ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[cell]);
      WorldContainer world2 = ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[otherCell]);
      if ((Object) world1 == (Object) null)
        DebugUtil.DevLogError($"{go} at {cell} has a valid cell but no world");
      if ((Object) world2 == (Object) null)
        DebugUtil.DevLogError($"{otherCell} is a valid cell but no world");
      if ((Object) world1 != (Object) null && (Object) world2 != (Object) null && world1.ParentWorldId == world2.ParentWorldId)
        return true;
    }
    return false;
  }

  public static int GetAsteroidWorldIdAtLocation(AxialI location)
  {
    foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.cellContents[location])
    {
      if (clusterGridEntity.Layer == EntityLayer.Asteroid)
      {
        WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
        if ((Object) component != (Object) null)
          return component.id;
      }
    }
    return -1;
  }

  public static bool ActiveWorldIsRocketInterior()
  {
    return ClusterManager.Instance.activeWorld.IsModuleInterior;
  }

  public static bool ActiveWorldHasPrinter()
  {
    return ClusterManager.Instance.activeWorld.IsModuleInterior || Components.Telepads.GetWorldItems(ClusterManager.Instance.activeWorldId).Count > 0;
  }

  public static float GetAmountFromRelatedWorlds(WorldInventory worldInventory, Tag element)
  {
    WorldContainer worldContainer1 = worldInventory.WorldContainer;
    float fromRelatedWorlds = 0.0f;
    int parentWorldId = worldContainer1.ParentWorldId;
    foreach (WorldContainer worldContainer2 in ClusterManager.Instance.WorldContainers)
    {
      if (worldContainer2.ParentWorldId == parentWorldId)
        fromRelatedWorlds += worldContainer2.worldInventory.GetAmount(element, false);
    }
    return fromRelatedWorlds;
  }

  public static List<Pickupable> GetPickupablesFromRelatedWorlds(
    WorldInventory worldInventory,
    Tag tag)
  {
    List<Pickupable> fromRelatedWorlds = new List<Pickupable>();
    int parentWorldId = worldInventory.GetComponent<WorldContainer>().ParentWorldId;
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (worldContainer.ParentWorldId == parentWorldId)
      {
        ICollection<Pickupable> pickupables = worldContainer.worldInventory.GetPickupables(tag);
        if (pickupables != null)
          fromRelatedWorlds.AddRange((IEnumerable<Pickupable>) pickupables);
      }
    }
    return fromRelatedWorlds;
  }

  public static void GetPickupablesFromRelatedWorlds(
    WorldInventory worldInventory,
    Tag tag,
    ref List<Pickupable> pickupables)
  {
    int parentWorldId = worldInventory.GetComponent<WorldContainer>().ParentWorldId;
    foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
    {
      if (worldContainer.ParentWorldId == parentWorldId)
      {
        ICollection<Pickupable> pickupables1 = worldContainer.worldInventory.GetPickupables(tag);
        if (pickupables1 != null)
          pickupables.AddRange((IEnumerable<Pickupable>) pickupables1);
      }
    }
  }

  public static string DebugGetMyWorldName(this GameObject gameObject)
  {
    WorldContainer myWorld = gameObject.GetMyWorld();
    return (Object) myWorld != (Object) null ? myWorld.worldName : $"InvalidWorld(pos={gameObject.transform.GetPosition()})";
  }

  public static ClusterGridEntity ClosestVisibleAsteroidToLocation(AxialI location)
  {
    foreach (AxialI cell in AxialUtil.SpiralOut(location, ClusterGrid.Instance.numRings))
    {
      if (ClusterGrid.Instance.IsValidCell(cell) && ClusterGrid.Instance.IsCellVisible(cell))
      {
        ClusterGridEntity asteroidAtCell = ClusterGrid.Instance.GetAsteroidAtCell(cell);
        if ((Object) asteroidAtCell != (Object) null)
          return asteroidAtCell;
      }
    }
    return (ClusterGridEntity) null;
  }
}
