// Decompiled with JetBrains decompiler
// Type: WorldGenSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/WorldGenSpawner")]
public class WorldGenSpawner : KMonoBehaviour
{
  [Serialize]
  private Prefab[] spawnInfos;
  [Serialize]
  private bool hasPlacedTemplates;
  private List<WorldGenSpawner.Spawnable> spawnables = new List<WorldGenSpawner.Spawnable>();

  public bool SpawnsRemain() => this.spawnables.Count > 0;

  public void SpawnEverything()
  {
    for (int index = 0; index < this.spawnables.Count; ++index)
      this.spawnables[index].TrySpawn();
  }

  public void SpawnTag(string id)
  {
    for (int index = 0; index < this.spawnables.Count; ++index)
    {
      if (this.spawnables[index].spawnInfo.id == id)
        this.spawnables[index].TrySpawn();
    }
  }

  public void ClearSpawnersInArea(Vector2 root_position, CellOffset[] area)
  {
    for (int index = 0; index < this.spawnables.Count; ++index)
    {
      if (Grid.IsCellOffsetOf(Grid.PosToCell(root_position), this.spawnables[index].cell, area))
        this.spawnables[index].FreeResources();
    }
  }

  public IReadOnlyList<WorldGenSpawner.Spawnable> GetSpawnables()
  {
    return (IReadOnlyList<WorldGenSpawner.Spawnable>) this.spawnables;
  }

  protected override void OnSpawn()
  {
    if (!this.hasPlacedTemplates)
    {
      Debug.Assert(SaveLoader.Instance.Cluster != null, (object) "Trying to place templates for an already-loaded save, no worldgen data available");
      this.DoReveal(SaveLoader.Instance.Cluster);
      this.PlaceTemplates(SaveLoader.Instance.Cluster);
      this.hasPlacedTemplates = true;
    }
    if (this.spawnInfos == null)
      return;
    for (int index = 0; index < this.spawnInfos.Length; ++index)
      this.AddSpawnable(this.spawnInfos[index]);
  }

  [System.Runtime.Serialization.OnSerializing]
  private void OnSerializing()
  {
    List<Prefab> prefabList = new List<Prefab>();
    for (int index = 0; index < this.spawnables.Count; ++index)
    {
      WorldGenSpawner.Spawnable spawnable = this.spawnables[index];
      if (!spawnable.isSpawned)
        prefabList.Add(spawnable.spawnInfo);
    }
    this.spawnInfos = prefabList.ToArray();
  }

  private void AddSpawnable(Prefab prefab)
  {
    this.spawnables.Add(new WorldGenSpawner.Spawnable(prefab));
  }

  public void AddLegacySpawner(Tag tag, int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    this.AddSpawnable(new Prefab(tag.Name, Prefab.Type.Other, xy.x, xy.y, SimHashes.Carbon));
  }

  public List<Tag> GetUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
  {
    List<Tag> unspawnedWithType = new List<Tag>();
    foreach (WorldGenSpawner.Spawnable spawnable in this.spawnables.FindAll((Predicate<WorldGenSpawner.Spawnable>) (match => !match.isSpawned && (int) Grid.WorldIdx[match.cell] == worldID && (UnityEngine.Object) Assets.GetPrefab((Tag) match.spawnInfo.id) != (UnityEngine.Object) null && (UnityEngine.Object) Assets.GetPrefab((Tag) match.spawnInfo.id).GetComponent<T>() != (UnityEngine.Object) null)))
      unspawnedWithType.Add((Tag) spawnable.spawnInfo.id);
    return unspawnedWithType;
  }

  public List<WorldGenSpawner.Spawnable> GeInfoOfUnspawnedWithType<T>(int worldID) where T : KMonoBehaviour
  {
    List<WorldGenSpawner.Spawnable> spawnableList = new List<WorldGenSpawner.Spawnable>();
    foreach (WorldGenSpawner.Spawnable spawnable in this.spawnables.FindAll((Predicate<WorldGenSpawner.Spawnable>) (match => !match.isSpawned && (int) Grid.WorldIdx[match.cell] == worldID && (UnityEngine.Object) Assets.GetPrefab((Tag) match.spawnInfo.id) != (UnityEngine.Object) null && (UnityEngine.Object) Assets.GetPrefab((Tag) match.spawnInfo.id).GetComponent<T>() != (UnityEngine.Object) null)))
      spawnableList.Add(spawnable);
    return spawnableList;
  }

  public WorldGenSpawner.Spawnable GetSpawnableInCellWithTag(int cell, Tag tag)
  {
    return this.spawnables.Find((Predicate<WorldGenSpawner.Spawnable>) (s => s.cell == cell && (Tag) s.spawnInfo.id == tag));
  }

  public WorldGenSpawner.Spawnable GetSpawnableInCell(int cell)
  {
    return this.spawnables.Find((Predicate<WorldGenSpawner.Spawnable>) (s => s.cell == cell));
  }

  public List<Tag> GetSpawnersWithTag(Tag tag, int worldID, bool includeSpawned = false)
  {
    List<Tag> spawnersWithTag = new List<Tag>();
    foreach (WorldGenSpawner.Spawnable spawnable in this.spawnables.FindAll((Predicate<WorldGenSpawner.Spawnable>) (match => (includeSpawned || !match.isSpawned) && (int) Grid.WorldIdx[match.cell] == worldID && (Tag) match.spawnInfo.id == tag)))
      spawnersWithTag.Add((Tag) spawnable.spawnInfo.id);
    return spawnersWithTag;
  }

  public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(
    Tag tag,
    int worldID,
    bool includeSpawned = false)
  {
    List<WorldGenSpawner.Spawnable> spawnablesWithTag = new List<WorldGenSpawner.Spawnable>();
    foreach (WorldGenSpawner.Spawnable spawnable in this.spawnables.FindAll((Predicate<WorldGenSpawner.Spawnable>) (match => (includeSpawned || !match.isSpawned) && (int) Grid.WorldIdx[match.cell] == worldID && (Tag) match.spawnInfo.id == tag)))
      spawnablesWithTag.Add(spawnable);
    return spawnablesWithTag;
  }

  public List<WorldGenSpawner.Spawnable> GetSpawnablesWithTag(
    bool includeSpawned = false,
    params Tag[] tags)
  {
    List<WorldGenSpawner.Spawnable> spawnablesWithTag = new List<WorldGenSpawner.Spawnable>();
    foreach (WorldGenSpawner.Spawnable spawnable in this.spawnables.FindAll((Predicate<WorldGenSpawner.Spawnable>) (match => includeSpawned || !match.isSpawned)))
    {
      foreach (Tag tag in tags)
      {
        if ((Tag) spawnable.spawnInfo.id == tag)
        {
          spawnablesWithTag.Add(spawnable);
          break;
        }
      }
    }
    return spawnablesWithTag;
  }

  private void PlaceTemplates(Cluster clusterLayout)
  {
    this.spawnables = new List<WorldGenSpawner.Spawnable>();
    foreach (WorldGen world in clusterLayout.worlds)
    {
      foreach (Prefab building in world.SpawnData.buildings)
      {
        building.location_x += world.data.world.offset.x;
        building.location_y += world.data.world.offset.y;
        building.type = Prefab.Type.Building;
        this.AddSpawnable(building);
      }
      foreach (Prefab elementalOre in world.SpawnData.elementalOres)
      {
        elementalOre.location_x += world.data.world.offset.x;
        elementalOre.location_y += world.data.world.offset.y;
        elementalOre.type = Prefab.Type.Ore;
        this.AddSpawnable(elementalOre);
      }
      foreach (Prefab backwallEntity in world.SpawnData.backwallEntities)
      {
        backwallEntity.location_x += world.data.world.offset.x;
        backwallEntity.location_y += world.data.world.offset.y;
        backwallEntity.type = Prefab.Type.Other;
        this.AddSpawnable(backwallEntity);
      }
      foreach (Prefab otherEntity in world.SpawnData.otherEntities)
      {
        otherEntity.location_x += world.data.world.offset.x;
        otherEntity.location_y += world.data.world.offset.y;
        otherEntity.type = Prefab.Type.Other;
        this.AddSpawnable(otherEntity);
      }
      foreach (Prefab pickupable in world.SpawnData.pickupables)
      {
        pickupable.location_x += world.data.world.offset.x;
        pickupable.location_y += world.data.world.offset.y;
        pickupable.type = Prefab.Type.Pickupable;
        this.AddSpawnable(pickupable);
      }
      foreach (Tag discoveredResource in world.SpawnData.discoveredResources)
        DiscoveredResources.Instance.Discover(discoveredResource);
      world.SpawnData.buildings.Clear();
      world.SpawnData.backwallEntities.Clear();
      world.SpawnData.elementalOres.Clear();
      world.SpawnData.otherEntities.Clear();
      world.SpawnData.pickupables.Clear();
      world.SpawnData.discoveredResources.Clear();
    }
  }

  private void DoReveal(Cluster clusterLayout)
  {
    foreach (WorldGen world in clusterLayout.worlds)
      Game.Instance.Reset(world.SpawnData, world.WorldOffset);
    for (int i = 0; i < Grid.CellCount; ++i)
    {
      Grid.Revealed[i] = false;
      Grid.Spawnable[i] = (byte) 0;
    }
    float innerRadius = 16.5f;
    int radius = 18;
    Vector2I vector2I = clusterLayout.currentWorld.SpawnData.baseStartPos + clusterLayout.currentWorld.WorldOffset;
    GridVisibility.Reveal(vector2I.x, vector2I.y, radius, innerRadius);
  }

  public class Spawnable
  {
    private HandleVector<int>.Handle fogOfWarPartitionerEntry;
    private HandleVector<int>.Handle solidChangedPartitionerEntry;

    public Prefab spawnInfo { get; private set; }

    public bool isSpawned { get; private set; }

    public int cell { get; private set; }

    public Spawnable(Prefab spawn_info)
    {
      this.spawnInfo = spawn_info;
      int num = Grid.XYToCell(this.spawnInfo.location_x, this.spawnInfo.location_y);
      GameObject prefab = Assets.GetPrefab((Tag) spawn_info.id);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        WorldSpawnableMonitor.Def def = prefab.GetDef<WorldSpawnableMonitor.Def>();
        if (def != null && def.adjustSpawnLocationCb != null)
          num = def.adjustSpawnLocationCb(num);
      }
      this.cell = num;
      Debug.Assert(Grid.IsValidCell(this.cell));
      if (Grid.Spawnable[this.cell] > (byte) 0)
        this.TrySpawn();
      else
        this.fogOfWarPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnReveal", (object) this, this.cell, GameScenePartitioner.Instance.fogOfWarChangedLayer, new Action<object>(this.OnReveal));
    }

    private void OnReveal(object data)
    {
      if (Grid.Spawnable[this.cell] <= (byte) 0)
        return;
      this.TrySpawn();
    }

    private void OnSolidChanged(object data)
    {
      if (Grid.Solid[this.cell])
        return;
      GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
      Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
      this.Spawn();
    }

    public void FreeResources()
    {
      if (this.solidChangedPartitionerEntry.IsValid())
      {
        GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
        if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
          Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.cell);
      }
      GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
      this.isSpawned = true;
    }

    public void TrySpawn()
    {
      if (this.isSpawned || this.solidChangedPartitionerEntry.IsValid())
        return;
      WorldContainer world = ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[this.cell]);
      bool flag1 = (UnityEngine.Object) world != (UnityEngine.Object) null && world.IsDiscovered;
      GameObject prefab = Assets.GetPrefab(this.GetPrefabTag());
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        if (!(flag1 | prefab.HasTag(GameTags.WarpTech)))
          return;
        GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
        bool flag2 = false;
        if ((UnityEngine.Object) prefab.GetComponent<Pickupable>() != (UnityEngine.Object) null && !prefab.HasTag(GameTags.Creatures.Digger))
          flag2 = true;
        else if (prefab.GetDef<BurrowMonitor.Def>() != null)
          flag2 = true;
        if (flag2 && Grid.Solid[this.cell])
        {
          this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WorldGenSpawner.OnSolidChanged", (object) this, this.cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
          Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(this.cell);
        }
        else
          this.Spawn();
      }
      else
      {
        if (!flag1)
          return;
        GameScenePartitioner.Instance.Free(ref this.fogOfWarPartitionerEntry);
        this.Spawn();
      }
    }

    private Tag GetPrefabTag()
    {
      Mob mob = SettingsCache.mobs.GetMob(this.spawnInfo.id);
      return mob != null && mob.prefabName != null ? new Tag(mob.prefabName) : new Tag(this.spawnInfo.id);
    }

    private void Spawn()
    {
      this.isSpawned = true;
      GameObject go = WorldGenSpawner.Spawnable.GetSpawnableCallback(this.spawnInfo.type)(this.spawnInfo, 0);
      if ((UnityEngine.Object) go != (UnityEngine.Object) null && (bool) (UnityEngine.Object) go)
      {
        go.SetActive(true);
        go.Trigger(1119167081, (object) this.spawnInfo);
      }
      this.FreeResources();
    }

    public static WorldGenSpawner.Spawnable.PlaceEntityFn GetSpawnableCallback(Prefab.Type type)
    {
      switch (type)
      {
        case Prefab.Type.Building:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceBuilding);
        case Prefab.Type.Ore:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceElementalOres);
        case Prefab.Type.Pickupable:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlacePickupables);
        case Prefab.Type.Other:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
        default:
          return new WorldGenSpawner.Spawnable.PlaceEntityFn(TemplateLoader.PlaceOtherEntities);
      }
    }

    public delegate GameObject PlaceEntityFn(Prefab prefab, int root_cell);
  }
}
