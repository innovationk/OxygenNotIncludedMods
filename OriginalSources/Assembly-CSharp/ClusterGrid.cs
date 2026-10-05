// Decompiled with JetBrains decompiler
// Type: ClusterGrid
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ClusterGrid
{
  public static ClusterGrid Instance;
  public const float NodeDistanceScale = 600f;
  private const float MAX_OFFSET_RADIUS = 0.5f;
  public int numRings;
  private ClusterFogOfWarManager.Instance m_fowManager;
  private Action<object> m_onClusterLocationChangedDelegate;
  public Dictionary<AxialI, List<ClusterGridEntity>> cellContents = new Dictionary<AxialI, List<ClusterGridEntity>>();

  public static void DestroyInstance() => ClusterGrid.Instance = (ClusterGrid) null;

  private ClusterFogOfWarManager.Instance GetFOWManager()
  {
    if (this.m_fowManager == null)
      this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
    return this.m_fowManager;
  }

  public bool IsValidCell(AxialI cell) => this.cellContents.ContainsKey(cell);

  public ClusterGrid(int numRings)
  {
    ClusterGrid.Instance = this;
    this.GenerateGrid(numRings);
    this.m_onClusterLocationChangedDelegate = new Action<object>(this.OnClusterLocationChanged);
  }

  public ClusterRevealLevel GetCellRevealLevel(AxialI cell)
  {
    return this.GetFOWManager().GetCellRevealLevel(cell);
  }

  public bool IsCellVisible(AxialI cell) => this.GetFOWManager().IsLocationRevealed(cell);

  public float GetRevealCompleteFraction(AxialI cell)
  {
    return this.GetFOWManager().GetRevealCompleteFraction(cell);
  }

  public bool IsVisible(ClusterGridEntity entity)
  {
    return entity.IsVisible && this.IsCellVisible(entity.Location);
  }

  public List<ClusterGridEntity> GetVisibleEntitiesAtCell(AxialI cell)
  {
    return this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell) ? this.cellContents[cell].Where<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (entity => entity.IsVisible)).ToList<ClusterGridEntity>() : new List<ClusterGridEntity>();
  }

  public ClusterGridEntity GetVisibleEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
  {
    if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
    {
      foreach (ClusterGridEntity entityOfLayerAtCell in this.cellContents[cell])
      {
        if (entityOfLayerAtCell.IsVisible && entityOfLayerAtCell.Layer == entityLayer)
          return entityOfLayerAtCell;
      }
    }
    return (ClusterGridEntity) null;
  }

  public ClusterGridEntity GetVisibleEntityOfLayerAtAdjacentCell(
    AxialI cell,
    EntityLayer entityLayer)
  {
    return AxialUtil.GetRing(cell, 1).SelectMany<AxialI, ClusterGridEntity>(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetVisibleEntitiesAtCell)).FirstOrDefault<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (entity => entity.Layer == entityLayer));
  }

  public List<ClusterGridEntity> GetHiddenEntitiesOfLayerAtCell(
    AxialI cell,
    EntityLayer entityLayer)
  {
    return AxialUtil.GetRing(cell, 0).SelectMany<AxialI, ClusterGridEntity>(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell)).Where<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (entity => entity.Layer == entityLayer)).ToList<ClusterGridEntity>();
  }

  public List<ClusterGridEntity> GetEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
  {
    return AxialUtil.GetRing(cell, 0).SelectMany<AxialI, ClusterGridEntity>(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell)).Where<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (entity => entity.Layer == entityLayer)).ToList<ClusterGridEntity>();
  }

  public StarmapHexCellInventory AddOrGetHexCellInventory(AxialI cell)
  {
    StarmapHexCellInventory hexCellInventory = (StarmapHexCellInventory) null;
    if (!StarmapHexCellInventory.AllInventories.TryGetValue(cell, out hexCellInventory))
    {
      StarmapHexCellInventoryVisuals component = Util.KInstantiate(Assets.GetPrefab((Tag) "StarmapHexCellInventory")).GetComponent<StarmapHexCellInventoryVisuals>();
      component.gameObject.name = $"{component.gameObject.name} [{cell.ToString()}]";
      component.Location = cell;
      component.gameObject.SetActive(true);
      hexCellInventory = component.inventory;
      StarmapHexCellInventory.AllInventories.Add(cell, hexCellInventory);
    }
    return hexCellInventory;
  }

  public ClusterGridEntity GetEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
  {
    return AxialUtil.GetRing(cell, 0).SelectMany<AxialI, ClusterGridEntity>(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell)).FirstOrDefault<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (entity => entity.Layer == entityLayer));
  }

  public List<ClusterGridEntity> GetHiddenEntitiesAtCell(AxialI cell)
  {
    return this.cellContents.ContainsKey(cell) && !this.GetFOWManager().IsLocationRevealed(cell) ? this.cellContents[cell].Where<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (entity => entity.IsVisible)).ToList<ClusterGridEntity>() : new List<ClusterGridEntity>();
  }

  public List<ClusterGridEntity> GetNotVisibleEntitiesAtAdjacentCell(AxialI cell)
  {
    return AxialUtil.GetRing(cell, 1).SelectMany<AxialI, ClusterGridEntity>(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell)).ToList<ClusterGridEntity>();
  }

  public AxialI GetRandomVisibleAdjacentCellLocation(AxialI cell)
  {
    return this.GetRandomVisibleAdjacentCellLocation(cell, AxialI.INVALID);
  }

  public AxialI GetRandomVisibleAdjacentCellLocation(AxialI cell, AxialI forbidden)
  {
    List<AxialI> all = AxialUtil.GetRing(cell, 1).FindAll((Predicate<AxialI>) (e =>
    {
      if (!this.IsCellVisible(e))
        return false;
      return forbidden == AxialI.INVALID || forbidden != e;
    }));
    return all != null ? all.GetRandom<AxialI>() : AxialI.INVALID;
  }

  public List<ClusterGridEntity> GetNotVisibleEntitiesOfLayerAtAdjacentCell(
    AxialI cell,
    EntityLayer entityLayer)
  {
    return AxialUtil.GetRing(cell, 1).SelectMany<AxialI, ClusterGridEntity>(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell)).Where<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (entity => entity.Layer == entityLayer)).ToList<ClusterGridEntity>();
  }

  public bool GetVisibleUnidentifiedMeteorShowerWithinRadius(
    AxialI center,
    int radius,
    out ClusterMapMeteorShower.Instance result)
  {
    for (int radius1 = 0; radius1 <= radius; ++radius1)
    {
      foreach (AxialI axialI in AxialUtil.GetRing(center, radius1))
      {
        if (this.IsValidCell(axialI) && this.GetFOWManager().IsLocationRevealed(axialI))
        {
          foreach (Component cmp in this.GetEntitiesOfLayerAtCell(axialI, EntityLayer.Meteor))
          {
            ClusterMapMeteorShower.Instance smi = cmp.GetSMI<ClusterMapMeteorShower.Instance>();
            if (smi != null && !smi.HasBeenIdentified)
            {
              result = smi;
              return true;
            }
          }
        }
      }
    }
    result = (ClusterMapMeteorShower.Instance) null;
    return false;
  }

  public ClusterGridEntity GetAsteroidAtCell(AxialI cell)
  {
    return !this.cellContents.ContainsKey(cell) ? (ClusterGridEntity) null : this.cellContents[cell].Where<ClusterGridEntity>((Func<ClusterGridEntity, bool>) (e => e.Layer == EntityLayer.Asteroid)).FirstOrDefault<ClusterGridEntity>();
  }

  public bool HasVisibleAsteroidAtCell(AxialI cell)
  {
    return (UnityEngine.Object) this.GetVisibleEntityOfLayerAtCell(cell, EntityLayer.Asteroid) != (UnityEngine.Object) null;
  }

  public void RegisterEntity(ClusterGridEntity entity)
  {
    this.cellContents[entity.Location].Add(entity);
    entity.Subscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
  }

  public void UnregisterEntity(ClusterGridEntity entity)
  {
    this.cellContents[entity.Location].Remove(entity);
    entity.Unsubscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
  }

  public void OnClusterLocationChanged(object data)
  {
    ClusterLocationChangedEvent locationChangedEvent = (ClusterLocationChangedEvent) data;
    Debug.Assert(this.IsValidCell(locationChangedEvent.oldLocation), (object) $"ChangeEntityCell move order FROM invalid location: {locationChangedEvent.oldLocation} {locationChangedEvent.entity}");
    Debug.Assert(this.IsValidCell(locationChangedEvent.newLocation), (object) $"ChangeEntityCell move order TO invalid location: {locationChangedEvent.newLocation} {locationChangedEvent.entity}");
    this.cellContents[locationChangedEvent.oldLocation].Remove(locationChangedEvent.entity);
    this.cellContents[locationChangedEvent.newLocation].Add(locationChangedEvent.entity);
  }

  private AxialI GetNeighbor(AxialI cell, AxialI direction) => cell + direction;

  public int GetCellRing(AxialI cell)
  {
    Vector3I cube = cell.ToCube();
    Vector3I vector3I1 = new Vector3I(cube.x, cube.y, cube.z);
    Vector3I vector3I2 = new Vector3I(0, 0, 0);
    return (int) (float) ((Mathf.Abs(vector3I1.x - vector3I2.x) + Mathf.Abs(vector3I1.y - vector3I2.y) + Mathf.Abs(vector3I1.z - vector3I2.z)) / 2);
  }

  private void CleanUpGrid() => this.cellContents.Clear();

  private int GetHexDistance(AxialI a, AxialI b)
  {
    Vector3I cube1 = a.ToCube();
    Vector3I cube2 = b.ToCube();
    return Mathf.Max(Mathf.Abs(cube1.x - cube2.x), Mathf.Abs(cube1.y - cube2.y), Mathf.Abs(cube1.z - cube2.z));
  }

  public List<ClusterGridEntity> GetEntitiesInRange(AxialI center, int range = 1)
  {
    List<ClusterGridEntity> entitiesInRange = new List<ClusterGridEntity>();
    foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> cellContent in this.cellContents)
    {
      if (this.GetHexDistance(cellContent.Key, center) <= range)
        entitiesInRange.AddRange((IEnumerable<ClusterGridEntity>) cellContent.Value);
    }
    return entitiesInRange;
  }

  public List<ClusterGridEntity> GetEntitiesOnCell(AxialI cell) => this.cellContents[cell];

  public bool IsInRange(AxialI a, AxialI b, int range = 1) => this.GetHexDistance(a, b) <= range;

  private void GenerateGrid(int rings)
  {
    this.CleanUpGrid();
    this.numRings = rings;
    for (int a = -rings + 1; a < rings; ++a)
    {
      for (int b = -rings + 1; b < rings; ++b)
      {
        for (int index = -rings + 1; index < rings; ++index)
        {
          if (a + b + index == 0)
            this.cellContents.Add(new AxialI(a, b), new List<ClusterGridEntity>());
        }
      }
    }
  }

  public AxialI GetRandomCellAtEdgeOfUniverse()
  {
    int num = this.numRings - 1;
    List<AxialI> rings = AxialUtil.GetRings(AxialI.ZERO, num, num);
    return rings.ElementAt<AxialI>(UnityEngine.Random.Range(0, rings.Count));
  }

  public Vector3 GetPosition(ClusterGridEntity entity)
  {
    float r = (float) entity.Location.R;
    float q = (float) entity.Location.Q;
    List<ClusterGridEntity> cellContent = this.cellContents[entity.Location];
    if (cellContent.Count <= 1 || !entity.SpaceOutInSameHex())
      return AxialUtil.AxialToWorld(r, q);
    int num1 = 0;
    int f1 = 0;
    foreach (ClusterGridEntity clusterGridEntity in cellContent)
    {
      if ((UnityEngine.Object) entity == (UnityEngine.Object) clusterGridEntity)
        num1 = f1;
      if (clusterGridEntity.SpaceOutInSameHex())
        ++f1;
    }
    if (cellContent.Count > f1)
    {
      f1 += 5;
      num1 += 5;
    }
    else if (f1 > 0)
    {
      ++f1;
      ++num1;
    }
    if (f1 == 0 || f1 == 1)
      return AxialUtil.AxialToWorld(r, q);
    float num2 = Mathf.Min(Mathf.Pow((float) f1, 0.5f), 1f) * 0.5f;
    float num3 = Mathf.Pow((float) num1 / (float) f1, 0.5f);
    float num4 = 0.81f;
    double f2 = 6.2831854820251465 * (double) (Mathf.Pow((float) f1, 0.5f) * num4) * (double) num3;
    float x = Mathf.Cos((float) f2) * num2 * num3;
    float y = Mathf.Sin((float) f2) * num2 * num3;
    return AxialUtil.AxialToWorld(r, q) + new Vector3(x, y, 0.0f);
  }

  public List<AxialI> GetPath(
    AxialI start,
    AxialI end,
    ClusterDestinationSelector destination_selector)
  {
    return this.GetPath(start, end, destination_selector, out string _);
  }

  public List<AxialI> GetPath(
    AxialI start,
    AxialI end,
    ClusterDestinationSelector destination_selector,
    out string fail_reason,
    bool dodgeHiddenAsteroids = false)
  {
    fail_reason = (string) null;
    if (!destination_selector.canNavigateFogOfWar && !this.IsCellVisible(end))
    {
      fail_reason = (string) UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR;
      return (List<AxialI>) null;
    }
    ClusterGridEntity entityOfLayerAtCell = this.GetVisibleEntityOfLayerAtCell(end, EntityLayer.Asteroid);
    if ((UnityEngine.Object) entityOfLayerAtCell != (UnityEngine.Object) null && destination_selector.requireLaunchPadOnAsteroidDestination)
    {
      bool flag = false;
      foreach (KMonoBehaviour launchPad in Components.LaunchPads)
      {
        if (launchPad.GetMyWorldLocation() == entityOfLayerAtCell.Location)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        fail_reason = (string) UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD;
        return (List<AxialI>) null;
      }
    }
    if ((UnityEngine.Object) entityOfLayerAtCell == (UnityEngine.Object) null && destination_selector.requireAsteroidDestination)
    {
      fail_reason = (string) UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID;
      return (List<AxialI>) null;
    }
    if (destination_selector.requiredEntityLayer != EntityLayer.None && (UnityEngine.Object) this.GetVisibleEntityOfLayerAtCell(end, destination_selector.requiredEntityLayer) == (UnityEngine.Object) null)
    {
      fail_reason = (string) UI.CLUSTERMAP.TOOLTIP_INVALID_METEOR_TARGET;
      return (List<AxialI>) null;
    }
    HashSet<AxialI> frontier = new HashSet<AxialI>();
    HashSet<AxialI> visited = new HashSet<AxialI>();
    HashSet<AxialI> buffer = new HashSet<AxialI>();
    Dictionary<AxialI, AxialI> cameFrom = new Dictionary<AxialI, AxialI>();
    frontier.Add(start);
    while (!frontier.Contains(end) && frontier.Count > 0)
      ExpandFrontier();
    if (frontier.Contains(end))
    {
      List<AxialI> path = new List<AxialI>();
      for (AxialI key = end; key != start; key = cameFrom[key])
        path.Add(key);
      path.Reverse();
      return path;
    }
    fail_reason = (string) UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_PATH;
    return (List<AxialI>) null;

    void ExpandFrontier()
    {
      buffer.Clear();
      foreach (AxialI cell in frontier)
      {
        foreach (AxialI direction in AxialI.DIRECTIONS)
        {
          AxialI neighbor = this.GetNeighbor(cell, direction);
          if (!visited.Contains(neighbor) && this.IsValidCell(neighbor) && (this.IsCellVisible(neighbor) || destination_selector.canNavigateFogOfWar) && (!this.HasVisibleAsteroidAtCell(neighbor) || !(neighbor != start) || !(neighbor != end)) && (!dodgeHiddenAsteroids || !((UnityEngine.Object) ClusterGrid.Instance.GetAsteroidAtCell(neighbor) != (UnityEngine.Object) null) || ClusterGrid.Instance.GetAsteroidAtCell(neighbor).IsVisibleInFOW == ClusterRevealLevel.Visible || !(neighbor != start) || !(neighbor != end)))
          {
            buffer.Add(neighbor);
            if (!cameFrom.ContainsKey(neighbor))
              cameFrom.Add(neighbor, cell);
          }
        }
        visited.Add(cell);
      }
      HashSet<AxialI> frontier = frontier;
      frontier = buffer;
      buffer = frontier;
    }
  }

  public void GetLocationDescription(
    AxialI location,
    out Sprite sprite,
    out string label,
    out string sublabel)
  {
    this.GetLocationDescription(location, out sprite, out label, out sublabel, out EntityLayer _);
  }

  public void GetLocationDescription(
    AxialI location,
    out Sprite sprite,
    out string label,
    out string sublabel,
    out EntityLayer locationEntity)
  {
    locationEntity = EntityLayer.None;
    List<ClusterGridEntity> visibleEntitiesAtCell = this.GetVisibleEntitiesAtCell(location);
    ClusterGridEntity clusterGridEntity1 = visibleEntitiesAtCell.Find((Predicate<ClusterGridEntity>) (x => x.Layer == EntityLayer.Asteroid));
    ClusterGridEntity clusterGridEntity2 = visibleEntitiesAtCell.Find((Predicate<ClusterGridEntity>) (x => x.Layer == EntityLayer.POI));
    ClusterGridEntity layerAtAdjacentCell = this.GetVisibleEntityOfLayerAtAdjacentCell(location, EntityLayer.Asteroid);
    if ((UnityEngine.Object) clusterGridEntity1 != (UnityEngine.Object) null)
    {
      sprite = clusterGridEntity1.GetUISprite();
      label = clusterGridEntity1.Name;
      WorldContainer component = clusterGridEntity1.GetComponent<WorldContainer>();
      sublabel = (string) Strings.Get(component.worldType);
      locationEntity = EntityLayer.Asteroid;
    }
    else if ((UnityEngine.Object) clusterGridEntity2 != (UnityEngine.Object) null)
    {
      sprite = clusterGridEntity2.GetUISprite();
      label = clusterGridEntity2.Name;
      sublabel = clusterGridEntity2.Name;
      locationEntity = EntityLayer.POI;
    }
    else if ((UnityEngine.Object) layerAtAdjacentCell != (UnityEngine.Object) null)
    {
      sprite = layerAtAdjacentCell.GetUISprite();
      label = UI.SPACEDESTINATIONS.ORBIT.NAME_FMT.Replace("{Name}", layerAtAdjacentCell.Name);
      WorldContainer component = layerAtAdjacentCell.GetComponent<WorldContainer>();
      sublabel = (string) Strings.Get(component.worldType);
      locationEntity = EntityLayer.None;
    }
    else if (this.IsCellVisible(location))
    {
      sprite = Assets.GetSprite((HashedString) "hex_empty");
      label = (string) UI.SPACEDESTINATIONS.EMPTY_SPACE.NAME;
      sublabel = "";
    }
    else
    {
      sprite = Assets.GetSprite((HashedString) "unknown_far");
      label = (string) UI.SPACEDESTINATIONS.FOG_OF_WAR_SPACE.NAME;
      sublabel = "";
    }
  }
}
