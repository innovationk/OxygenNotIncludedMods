// Decompiled with JetBrains decompiler
// Type: WorldContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Delaunay.Geo;
using Klei;
using KSerialization;
using ProcGen;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.Linq;
using TemplateClasses;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class WorldContainer : KMonoBehaviour
{
  [Serialize]
  public int id = -1;
  [Serialize]
  public Tag prefabTag;
  [Serialize]
  private Vector2I worldOffset;
  [Serialize]
  private Vector2I worldSize;
  [Serialize]
  private bool fullyEnclosedBorder;
  [Serialize]
  private int hiddenYOffset;
  [Serialize]
  private bool isModuleInterior;
  [Serialize]
  private WorldDetailSave.OverworldCell overworldCell;
  [Serialize]
  private bool isDiscovered;
  [Serialize]
  private bool isStartWorld;
  [Serialize]
  private bool isDupeVisited;
  [Serialize]
  private float dupeVisitedTimestamp = -1f;
  [Serialize]
  private float discoveryTimestamp = -1f;
  [Serialize]
  private bool isRoverVisited;
  [Serialize]
  private bool isSurfaceRevealed;
  [Serialize]
  public string worldName;
  [Serialize]
  public string[] nameTables;
  [Serialize]
  public Tag[] worldTags;
  [Serialize]
  public string overrideName;
  [Serialize]
  public string worldType;
  [Serialize]
  public string worldDescription;
  [Serialize]
  public int northernlights = FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;
  [Serialize]
  public int largeImpactorFragments = FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.DEFAULT_VALUE;
  [Serialize]
  public int sunlight = FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
  [Serialize]
  public int cosmicRadiation = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
  [Serialize]
  public float currentSunlightIntensity;
  [Serialize]
  public float currentCosmicIntensity = (float) FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
  [Serialize]
  public string sunlightFixedTrait;
  [Serialize]
  public string cosmicRadiationFixedTrait;
  [Serialize]
  public string northernLightFixedTrait;
  [Serialize]
  public string largeImpactorFragmentsFixedTrait;
  [Serialize]
  public int fixedTraitsUpdateVersion = 1;
  private Dictionary<string, int> sunlightFixedTraits = new Dictionary<string, int>()
  {
    {
      FIXEDTRAITS.SUNLIGHT.NAME.NONE,
      FIXEDTRAITS.SUNLIGHT.NONE
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW,
      FIXEDTRAITS.SUNLIGHT.VERY_VERY_LOW
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW,
      FIXEDTRAITS.SUNLIGHT.VERY_LOW
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.LOW,
      FIXEDTRAITS.SUNLIGHT.LOW
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW,
      FIXEDTRAITS.SUNLIGHT.MED_LOW
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.MED,
      FIXEDTRAITS.SUNLIGHT.MED
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH,
      FIXEDTRAITS.SUNLIGHT.MED_HIGH
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.HIGH,
      FIXEDTRAITS.SUNLIGHT.HIGH
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH,
      FIXEDTRAITS.SUNLIGHT.VERY_HIGH
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH,
      FIXEDTRAITS.SUNLIGHT.VERY_VERY_HIGH
    },
    {
      FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_VERY_HIGH,
      FIXEDTRAITS.SUNLIGHT.VERY_VERY_VERY_HIGH
    }
  };
  private Dictionary<string, int> northernLightsFixedTraits = new Dictionary<string, int>()
  {
    {
      FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE,
      FIXEDTRAITS.NORTHERNLIGHTS.NONE
    },
    {
      FIXEDTRAITS.NORTHERNLIGHTS.NAME.ENABLED,
      FIXEDTRAITS.NORTHERNLIGHTS.ENABLED
    }
  };
  private Dictionary<string, int> largeImpactorFragmentsFixedTraits = new Dictionary<string, int>()
  {
    {
      FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NAME.NONE,
      FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NONE
    },
    {
      FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NAME.ALLOWED,
      FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.ALLOWED
    }
  };
  private Dictionary<string, int> cosmicRadiationFixedTraits = new Dictionary<string, int>()
  {
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.NONE,
      FIXEDTRAITS.COSMICRADIATION.NONE
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW,
      FIXEDTRAITS.COSMICRADIATION.VERY_VERY_LOW
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW,
      FIXEDTRAITS.COSMICRADIATION.VERY_LOW
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.LOW,
      FIXEDTRAITS.COSMICRADIATION.LOW
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW,
      FIXEDTRAITS.COSMICRADIATION.MED_LOW
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.MED,
      FIXEDTRAITS.COSMICRADIATION.MED
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH,
      FIXEDTRAITS.COSMICRADIATION.MED_HIGH
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.HIGH,
      FIXEDTRAITS.COSMICRADIATION.HIGH
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH,
      FIXEDTRAITS.COSMICRADIATION.VERY_HIGH
    },
    {
      FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH,
      FIXEDTRAITS.COSMICRADIATION.VERY_VERY_HIGH
    }
  };
  [Serialize]
  private List<string> m_seasonIds;
  [Serialize]
  private List<string> m_subworldNames;
  [Serialize]
  private List<string> m_worldTraitIds;
  [Serialize]
  private List<string> m_storyTraitIds;
  [Serialize]
  private List<string> m_generatedSubworlds;
  [Serialize]
  private List<BiomeSizeData> m_biomesData;
  [Serialize]
  private Vector4[] m_biomesSize = new Vector4[1]
  {
    Vector4.zero
  };
  private WorldParentChangedEventArgs parentChangeArgs = new WorldParentChangedEventArgs();
  [MySmiReq]
  private AlertStateManager.Instance m_alertManager;
  private List<Prioritizable> yellowAlertTasks = new List<Prioritizable>();
  private List<int> m_childWorlds = new List<int>();

  [Serialize]
  public WorldInventory worldInventory { get; private set; }

  public Dictionary<Tag, float> materialNeeds { get; private set; }

  public bool IsModuleInterior => this.isModuleInterior;

  public bool IsDiscovered => this.isDiscovered || DebugHandler.RevealFogOfWar;

  public bool IsStartWorld => this.isStartWorld;

  public bool IsDupeVisited => this.isDupeVisited;

  public float DupeVisitedTimestamp => this.dupeVisitedTimestamp;

  public float DiscoveryTimestamp => this.discoveryTimestamp;

  public bool IsRoverVisted => this.isRoverVisited;

  public bool IsSurfaceRevealed => this.isSurfaceRevealed;

  public Dictionary<string, int> SunlightFixedTraits => this.sunlightFixedTraits;

  public Dictionary<string, int> NorthernLightsFixedTraits => this.northernLightsFixedTraits;

  public Dictionary<string, int> LargeImpactorFragmentsFixedTraits
  {
    get => this.largeImpactorFragmentsFixedTraits;
  }

  public Dictionary<string, int> CosmicRadiationFixedTraits => this.cosmicRadiationFixedTraits;

  public Vector4[] BiomesOnlySizeData => this.m_biomesSize;

  public List<BiomeSizeData> BiomesData => this.m_biomesData;

  public List<string> Biomes => this.m_subworldNames;

  public List<string> GeneratedBiomes => this.m_generatedSubworlds;

  public List<string> WorldTraitIds => this.m_worldTraitIds;

  public List<string> StoryTraitIds => this.m_storyTraitIds;

  public AlertStateManager.Instance AlertManager
  {
    get
    {
      if (this.m_alertManager == null)
        this.m_alertManager = this.GetComponent<StateMachineController>().GetSMI<AlertStateManager.Instance>();
      Debug.Assert(this.m_alertManager != null, (object) "AlertStateManager should never be null.");
      return this.m_alertManager;
    }
  }

  public void AddTopPriorityPrioritizable(Prioritizable prioritizable)
  {
    if (!this.yellowAlertTasks.Contains(prioritizable))
      this.yellowAlertTasks.Add(prioritizable);
    this.RefreshHasTopPriorityChore();
  }

  public void RemoveTopPriorityPrioritizable(Prioritizable prioritizable)
  {
    for (int index = this.yellowAlertTasks.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.yellowAlertTasks[index] == (UnityEngine.Object) prioritizable || this.yellowAlertTasks[index].Equals((object) null))
        this.yellowAlertTasks.RemoveAt(index);
    }
    this.RefreshHasTopPriorityChore();
  }

  public int ParentWorldId { get; private set; }

  public ICollection<int> GetChildWorldIds() => (ICollection<int>) this.m_childWorlds;

  private void OnWorldRemoved(object data)
  {
    int num = ((Boxed<int>) data).value;
    if (num == (int) byte.MaxValue)
      return;
    this.m_childWorlds.Remove(num);
  }

  private void OnWorldParentChanged(object data)
  {
    if (!(data is WorldParentChangedEventArgs changedEventArgs))
      return;
    if (changedEventArgs.world.ParentWorldId == this.id)
      this.m_childWorlds.Add(changedEventArgs.world.id);
    if (changedEventArgs.lastParentId != this.ParentWorldId)
      return;
    this.m_childWorlds.Remove(changedEventArgs.world.id);
  }

  public Quadrant[] GetQuadrantOfCell(int cell, int depth = 1)
  {
    Vector2 vector2_1 = new Vector2((float) this.WorldSize.x * Grid.CellSizeInMeters, (float) this.worldSize.y * Grid.CellSizeInMeters);
    Vector2 pos2D1 = (Vector2) Grid.CellToPos2D(Grid.XYToCell(this.WorldOffset.x, this.WorldOffset.y));
    Vector2 pos2D2 = (Vector2) Grid.CellToPos2D(cell);
    Quadrant[] quadrantOfCell = new Quadrant[depth];
    Vector2 vector2_2 = new Vector2(pos2D1.x, (float) this.worldOffset.y + vector2_1.y);
    Vector2 vector2_3 = new Vector2(pos2D1.x + vector2_1.x, (float) this.worldOffset.y);
    for (int index = 0; index < depth; ++index)
    {
      float num1 = vector2_3.x - vector2_2.x;
      double num2 = (double) vector2_2.y - (double) vector2_3.y;
      float num3 = num1 * 0.5f;
      float num4 = (float) (num2 * 0.5);
      if ((double) pos2D2.x >= (double) vector2_2.x + (double) num3 && (double) pos2D2.y >= (double) vector2_3.y + (double) num4)
        quadrantOfCell[index] = Quadrant.NE;
      if ((double) pos2D2.x >= (double) vector2_2.x + (double) num3 && (double) pos2D2.y < (double) vector2_3.y + (double) num4)
        quadrantOfCell[index] = Quadrant.SE;
      if ((double) pos2D2.x < (double) vector2_2.x + (double) num3 && (double) pos2D2.y < (double) vector2_3.y + (double) num4)
        quadrantOfCell[index] = Quadrant.SW;
      if ((double) pos2D2.x < (double) vector2_2.x + (double) num3 && (double) pos2D2.y >= (double) vector2_3.y + (double) num4)
        quadrantOfCell[index] = Quadrant.NW;
      switch (quadrantOfCell[index])
      {
        case Quadrant.NE:
          vector2_2.x += num3;
          vector2_3.y += num4;
          break;
        case Quadrant.NW:
          vector2_3.x -= num3;
          vector2_3.y += num4;
          break;
        case Quadrant.SW:
          vector2_2.y -= num4;
          vector2_3.x -= num3;
          break;
        case Quadrant.SE:
          vector2_2.x += num3;
          vector2_2.y -= num4;
          break;
      }
    }
    return quadrantOfCell;
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized() => this.ParentWorldId = this.id;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.worldInventory = this.GetComponent<WorldInventory>();
    this.materialNeeds = new Dictionary<Tag, float>();
    ClusterManager.Instance.RegisterWorldContainer(this);
    Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
    ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.gameObject.AddOrGet<InfoDescription>().DescriptionLocString = this.worldDescription;
    this.RefreshHasTopPriorityChore();
    this.UpgradeFixedTraits();
    this.RefreshFixedTraits();
    if (!DlcManager.IsPureVanilla())
      return;
    this.isStartWorld = true;
    this.isDupeVisited = true;
  }

  protected override void OnCleanUp()
  {
    SaveGame.Instance.materialSelectorSerializer.WipeWorldSelectionData(this.id);
    Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
    ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
    base.OnCleanUp();
  }

  private void UpgradeFixedTraits()
  {
    if (this.sunlightFixedTrait == null || this.sunlightFixedTrait == "")
      new Dictionary<int, string>()
      {
        {
          160000,
          FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
        },
        {
          0,
          FIXEDTRAITS.SUNLIGHT.NAME.NONE
        },
        {
          10000,
          FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW
        },
        {
          20000,
          FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW
        },
        {
          30000,
          FIXEDTRAITS.SUNLIGHT.NAME.LOW
        },
        {
          35000,
          FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW
        },
        {
          40000,
          FIXEDTRAITS.SUNLIGHT.NAME.MED
        },
        {
          50000,
          FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH
        },
        {
          60000,
          FIXEDTRAITS.SUNLIGHT.NAME.HIGH
        },
        {
          80000,
          FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH
        },
        {
          120000,
          FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
        }
      }.TryGetValue(this.sunlight, out this.sunlightFixedTrait);
    if (this.cosmicRadiationFixedTrait != null && !(this.cosmicRadiationFixedTrait == ""))
      return;
    new Dictionary<int, string>()
    {
      {
        0,
        FIXEDTRAITS.COSMICRADIATION.NAME.NONE
      },
      {
        6,
        FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW
      },
      {
        12,
        FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW
      },
      {
        18,
        FIXEDTRAITS.COSMICRADIATION.NAME.LOW
      },
      {
        21,
        FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW
      },
      {
        25,
        FIXEDTRAITS.COSMICRADIATION.NAME.MED
      },
      {
        31 /*0x1F*/,
        FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH
      },
      {
        37,
        FIXEDTRAITS.COSMICRADIATION.NAME.HIGH
      },
      {
        50,
        FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH
      },
      {
        75,
        FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH
      }
    }.TryGetValue(this.cosmicRadiation, out this.cosmicRadiationFixedTrait);
  }

  private void RefreshFixedTraits()
  {
    this.sunlight = this.GetSunlightValueFromFixedTrait();
    this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
    this.northernlights = this.GetNorthernlightValueFromFixedTrait();
    this.largeImpactorFragments = this.GetLargeImpactorFragmentsValueFromFixedTrait();
  }

  private void RefreshHasTopPriorityChore()
  {
    if (this.AlertManager == null)
      return;
    this.AlertManager.SetHasTopPriorityChore(this.yellowAlertTasks.Count > 0);
  }

  public List<string> GetSeasonIds() => this.m_seasonIds;

  public bool IsRedAlert() => this.m_alertManager.IsRedAlert();

  public bool IsYellowAlert() => this.m_alertManager.IsYellowAlert();

  public string GetRandomName()
  {
    return !this.overrideName.IsNullOrWhiteSpace() ? (string) Strings.Get(this.overrideName) : GameUtil.GenerateRandomWorldName(this.nameTables);
  }

  public void SetID(int id)
  {
    this.id = id;
    this.ParentWorldId = id;
  }

  public void SetParentIdx(int parentIdx)
  {
    this.parentChangeArgs.lastParentId = this.ParentWorldId;
    this.parentChangeArgs.world = this;
    this.ParentWorldId = parentIdx;
    Game.Instance.Trigger(880851192, (object) this.parentChangeArgs);
    this.parentChangeArgs.lastParentId = (int) byte.MaxValue;
  }

  public Vector2 minimumBounds
  {
    get => new Vector2((float) this.worldOffset.x, (float) this.worldOffset.y);
  }

  public Vector2 maximumBounds
  {
    get
    {
      return new Vector2((float) (this.worldOffset.x + (this.worldSize.x - 1)), (float) (this.worldOffset.y + (this.worldSize.y - this.hiddenYOffset - 1)));
    }
  }

  public Vector2I WorldSize => this.worldSize;

  public Vector2I WorldOffset => this.worldOffset;

  public int HiddenYOffset => this.hiddenYOffset;

  public bool FullyEnclosedBorder => this.fullyEnclosedBorder;

  public int Height => this.worldSize.y;

  public int Width => this.worldSize.x;

  public void SetDiscovered(bool reveal_surface = false)
  {
    if (!this.isDiscovered)
      this.discoveryTimestamp = GameUtil.GetCurrentTimeInCycles();
    this.isDiscovered = true;
    if (reveal_surface)
      this.LookAtSurface();
    Game.Instance.Trigger(-521212405, (object) this);
  }

  public void SetDupeVisited()
  {
    if (this.isDupeVisited)
      return;
    this.dupeVisitedTimestamp = GameUtil.GetCurrentTimeInCycles();
    this.isDupeVisited = true;
    Game.Instance.Trigger(-434755240, (object) this);
  }

  public void SetRoverLanded() => this.isRoverVisited = true;

  public void SetRocketInteriorWorldDetails(int world_id, Vector2I size, Vector2I offset)
  {
    this.SetID(world_id);
    this.fullyEnclosedBorder = true;
    this.worldOffset = offset;
    this.worldSize = size;
    this.isDiscovered = true;
    this.isModuleInterior = true;
    this.m_seasonIds = new List<string>();
  }

  private static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
  {
    if (first == second)
      return 0;
    Vector2 vector2_1 = first - origin;
    Vector2 vector2_2 = second - origin;
    float num1 = Mathf.Atan2(vector2_1.x, vector2_1.y);
    float num2 = Mathf.Atan2(vector2_2.x, vector2_2.y);
    return (double) num1 < (double) num2 || (double) num1 <= (double) num2 && (double) vector2_1.sqrMagnitude < (double) vector2_2.sqrMagnitude ? 1 : -1;
  }

  public void PlaceInteriorTemplate(string template_name, System.Action callback)
  {
    TemplateContainer template = TemplateCache.GetTemplate(template_name);
    Vector2 pos = new Vector2((float) (this.worldSize.x / 2 + this.worldOffset.x), (float) (this.worldSize.y / 2 + this.worldOffset.y));
    TemplateLoader.Stamp(template, pos, callback);
    float num = Math.Max(template.info.size.X / 2f, template.info.size.Y / 2f);
    GridVisibility.Reveal((int) pos.x, (int) pos.y, (int) num + 3 + 5, num + 3f);
    WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
    this.overworldCell = new WorldDetailSave.OverworldCell();
    List<Vector2> verts = new List<Vector2>(template.cells.Count);
    foreach (Prefab building in template.buildings)
    {
      if (building.id == "RocketWallTile")
      {
        Vector2 vector2 = new Vector2((float) building.location_x + pos.x, (float) building.location_y + pos.y);
        if ((double) vector2.x > (double) pos.x)
          vector2.x += 0.5f;
        if ((double) vector2.y > (double) pos.y)
          vector2.y += 0.5f;
        verts.Add(vector2);
      }
    }
    verts.Sort((Comparison<Vector2>) ((v1, v2) => WorldContainer.IsClockwise(v1, v2, pos)));
    Polygon polygon = new Polygon(verts);
    this.overworldCell.poly = polygon;
    this.overworldCell.zoneType = SubWorld.ZoneType.RocketInterior;
    this.overworldCell.tags = new TagSet()
    {
      WorldGenTags.RocketInterior
    };
    clusterDetailSave.overworldCells.Add(this.overworldCell);
    for (int index1 = 0; index1 < this.worldSize.y; ++index1)
    {
      for (int index2 = 0; index2 < this.worldSize.x; ++index2)
      {
        Vector2I vector2I = new Vector2I(this.worldOffset.x + index2, this.worldOffset.y + index1);
        int cell = Grid.XYToCell(vector2I.x, vector2I.y);
        if (polygon.Contains(new Vector2((float) vector2I.x, (float) vector2I.y)))
        {
          SimMessages.ModifyCellWorldZone(cell, (byte) 14);
          World.Instance.zoneRenderData.worldZoneTypes[cell] = SubWorld.ZoneType.RocketInterior;
        }
        else
        {
          SimMessages.ModifyCellWorldZone(cell, byte.MaxValue);
          World.Instance.zoneRenderData.worldZoneTypes[cell] = SubWorld.ZoneType.Space;
        }
      }
    }
  }

  private int GetDefaultValueForFixedTraitCategory(Dictionary<string, int> traitCategory)
  {
    if (traitCategory == this.largeImpactorFragmentsFixedTraits)
      return FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.DEFAULT_VALUE;
    if (traitCategory == this.northernLightsFixedTraits)
      return FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;
    if (traitCategory == this.sunlightFixedTraits)
      return FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
    return traitCategory == this.cosmicRadiationFixedTraits ? FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE : 0;
  }

  private string GetDefaultFixedTraitFor(Dictionary<string, int> traitCategory)
  {
    if (traitCategory == this.largeImpactorFragmentsFixedTraits)
      return FIXEDTRAITS.LARGEIMPACTORFRAGMENTS.NAME.DEFAULT;
    if (traitCategory == this.northernLightsFixedTraits)
      return FIXEDTRAITS.NORTHERNLIGHTS.NAME.DEFAULT;
    if (traitCategory == this.sunlightFixedTraits)
      return FIXEDTRAITS.SUNLIGHT.NAME.DEFAULT;
    return traitCategory == this.cosmicRadiationFixedTraits ? FIXEDTRAITS.COSMICRADIATION.NAME.DEFAULT : (string) null;
  }

  private string GetFixedTraitsFor(Dictionary<string, int> traitCategory, WorldGen world)
  {
    foreach (string fixedTrait in world.Settings.world.fixedTraits)
    {
      if (traitCategory.ContainsKey(fixedTrait))
        return fixedTrait;
    }
    return this.GetDefaultFixedTraitFor(traitCategory);
  }

  private int GetFixedTraitValueForTrait(Dictionary<string, int> traitCategory, ref string trait)
  {
    if (trait == null)
      trait = this.GetDefaultFixedTraitFor(traitCategory);
    return traitCategory.ContainsKey(trait) ? traitCategory[trait] : this.GetDefaultValueForFixedTraitCategory(traitCategory);
  }

  private string GetLargeImpactorFragmentsFixedTraits(WorldGen world)
  {
    return this.GetFixedTraitsFor(this.LargeImpactorFragmentsFixedTraits, world);
  }

  private string GetNorthernlightFixedTraits(WorldGen world)
  {
    return this.GetFixedTraitsFor(this.northernLightsFixedTraits, world);
  }

  private string GetSunlightFromFixedTraits(WorldGen world)
  {
    return this.GetFixedTraitsFor(this.sunlightFixedTraits, world);
  }

  private string GetCosmicRadiationFromFixedTraits(WorldGen world)
  {
    return this.GetFixedTraitsFor(this.cosmicRadiationFixedTraits, world);
  }

  private int GetLargeImpactorFragmentsValueFromFixedTrait()
  {
    return this.GetFixedTraitValueForTrait(this.largeImpactorFragmentsFixedTraits, ref this.largeImpactorFragmentsFixedTrait);
  }

  private int GetNorthernlightValueFromFixedTrait()
  {
    return this.GetFixedTraitValueForTrait(this.northernLightsFixedTraits, ref this.northernLightFixedTrait);
  }

  private int GetSunlightValueFromFixedTrait()
  {
    return this.GetFixedTraitValueForTrait(this.sunlightFixedTraits, ref this.sunlightFixedTrait);
  }

  private int GetCosmicRadiationValueFromFixedTrait()
  {
    return this.GetFixedTraitValueForTrait(this.cosmicRadiationFixedTraits, ref this.cosmicRadiationFixedTrait);
  }

  public void SetWorldDetails(WorldGen world)
  {
    if (world != null)
    {
      this.fullyEnclosedBorder = world.Settings.GetBoolSetting("DrawWorldBorder") && world.Settings.GetBoolSetting("DrawWorldBorderOverVacuum");
      this.worldOffset = world.GetPosition();
      this.worldSize = world.GetSize();
      this.hiddenYOffset = world.HiddenYOffset;
      this.isDiscovered = world.isStartingWorld;
      this.isStartWorld = world.isStartingWorld;
      this.worldName = world.Settings.world.filePath;
      this.nameTables = world.Settings.world.nameTables;
      this.worldTags = world.Settings.world.worldTags != null ? world.Settings.world.worldTags.ToArray().ToTagArray() : new Tag[0];
      this.worldDescription = world.Settings.world.description;
      this.worldType = world.Settings.world.name;
      this.isModuleInterior = world.Settings.world.moduleInterior;
      this.m_seasonIds = new List<string>((IEnumerable<string>) world.Settings.world.seasons);
      this.m_generatedSubworlds = world.Settings.world.generatedSubworlds;
      this.largeImpactorFragmentsFixedTrait = this.GetLargeImpactorFragmentsFixedTraits(world);
      this.northernLightFixedTrait = this.GetNorthernlightFixedTraits(world);
      this.sunlightFixedTrait = this.GetSunlightFromFixedTraits(world);
      this.cosmicRadiationFixedTrait = this.GetCosmicRadiationFromFixedTraits(world);
      this.sunlight = this.GetSunlightValueFromFixedTrait();
      this.northernlights = this.GetNorthernlightValueFromFixedTrait();
      this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
      this.currentCosmicIntensity = (float) this.cosmicRadiation;
      HashSet<string> source = new HashSet<string>();
      foreach (string generatedSubworld in world.Settings.world.generatedSubworlds)
      {
        string str1 = generatedSubworld.Substring(0, generatedSubworld.LastIndexOf('/'));
        string str2 = str1.Substring(str1.LastIndexOf('/') + 1, str1.Length - (str1.LastIndexOf('/') + 1));
        source.Add(str2);
      }
      this.m_subworldNames = source.ToList<string>();
      this.m_biomesData = world.data.biomes;
      Vector4[] vector4Array = new Vector4[world.data.biomes.Count];
      for (int index = 0; index < vector4Array.Length; ++index)
        vector4Array[index] = world.data.biomes[index].size;
      this.m_biomesSize = vector4Array;
      this.m_worldTraitIds = new List<string>();
      this.m_worldTraitIds.AddRange((IEnumerable<string>) world.Settings.GetWorldTraitIDs());
      this.m_storyTraitIds = new List<string>();
      this.m_storyTraitIds.AddRange((IEnumerable<string>) world.Settings.GetStoryTraitIDs());
    }
    else
    {
      this.fullyEnclosedBorder = false;
      this.worldOffset = Vector2I.zero;
      this.worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
      this.isDiscovered = true;
      this.isStartWorld = true;
      this.isDupeVisited = true;
      this.m_seasonIds = new List<string>()
      {
        Db.Get().GameplaySeasons.MeteorShowers.Id
      };
    }
  }

  public bool ContainsPoint(Vector2 point)
  {
    return (double) point.x >= (double) this.worldOffset.x && (double) point.y >= (double) this.worldOffset.y && (double) point.x < (double) (this.worldOffset.x + this.worldSize.x) && (double) point.y < (double) (this.worldOffset.y + this.worldSize.y);
  }

  public void LookAtSurface()
  {
    if (!this.IsDupeVisited)
      this.RevealSurface();
    Vector3? nullable = this.SetSurfaceCameraPos();
    if (ClusterManager.Instance.activeWorldId != this.id || !nullable.HasValue)
      return;
    CameraController.Instance.SnapTo(nullable.Value);
  }

  public void RevealSurface()
  {
    if (this.isSurfaceRevealed)
      return;
    this.isSurfaceRevealed = true;
    for (int index1 = 0; index1 < this.worldSize.x; ++index1)
    {
      for (int index2 = this.worldSize.y - 1; index2 >= 0; --index2)
      {
        int cell = Grid.XYToCell(index1 + this.worldOffset.x, index2 + this.worldOffset.y);
        if (Grid.IsValidCell(cell) && !Grid.IsSolidCell(cell) && !Grid.IsLiquid(cell))
          GridVisibility.Reveal(index1 + this.worldOffset.X, index2 + this.worldOffset.y, 7, 1f);
        else
          break;
      }
    }
  }

  public void RevealHiddenY() => this.hiddenYOffset = 0;

  private Vector3? SetSurfaceCameraPos()
  {
    if (!((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null))
      return new Vector3?();
    int val1 = (int) this.maximumBounds.y;
    for (int index1 = 0; index1 < this.worldSize.X; ++index1)
    {
      for (int index2 = this.worldSize.y - 1; index2 >= 0; --index2)
      {
        int num = index2 + this.worldOffset.y;
        int cell = Grid.XYToCell(index1 + this.worldOffset.x, num);
        if (Grid.IsValidCell(cell) && (Grid.Solid[cell] || Grid.IsLiquid(cell)))
        {
          val1 = Math.Min(val1, num);
          break;
        }
      }
    }
    Vector3 start_pos = new Vector3((float) (this.WorldOffset.x + this.Width / 2), (float) ((val1 + this.worldOffset.y + this.worldSize.y) / 2), 0.0f);
    SaveGame.Instance.GetComponent<UserNavigation>().SetWorldCameraStartPosition(this.id, start_pos);
    return new Vector3?(start_pos);
  }

  public void EjectAllDupes(Vector3 spawn_pos)
  {
    foreach (KMonoBehaviour worldItem in Components.MinionIdentities.GetWorldItems(this.id))
      worldItem.transform.SetLocalPosition(spawn_pos);
  }

  public void SpacePodAllDupes(AxialI sourceLocation, SimHashes podElement)
  {
    foreach (MinionIdentity worldItem in Components.MinionIdentities.GetWorldItems(this.id))
    {
      if (!worldItem.HasTag(GameTags.Dead))
      {
        Vector3 position = new Vector3(-1f, -1f, 0.0f);
        GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) "EscapePod"), position);
        go.GetComponent<PrimaryElement>().SetElement(podElement);
        go.SetActive(true);
        go.GetComponent<MinionStorage>().SerializeMinion(worldItem.gameObject);
        TravellingCargoLander.StatesInstance smi = go.GetSMI<TravellingCargoLander.StatesInstance>();
        smi.StartSM();
        smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
      }
    }
  }

  public void DestroyWorldBuildings(out HashSet<int> noRefundTiles)
  {
    this.TransferBuildingMaterials(out noRefundTiles);
    foreach (Component worldItem in Components.ClusterCraftInteriorDoors.GetWorldItems(this.id))
      worldItem.DeleteObject();
    this.ClearWorldZones();
  }

  public void TransferResourcesToParentWorld(Vector3 spawn_pos, HashSet<int> noRefundTiles)
  {
    this.TransferPickupables(spawn_pos);
    this.TransferLiquidsSolidsAndGases(spawn_pos, noRefundTiles);
  }

  public void TransferResourcesToDebris(
    AxialI sourceLocation,
    HashSet<int> noRefundTiles,
    SimHashes debrisContainerElement)
  {
    List<Storage> debrisObjects = new List<Storage>();
    this.TransferPickupablesToDebris(ref debrisObjects, debrisContainerElement);
    this.TransferLiquidsSolidsAndGasesToDebris(ref debrisObjects, noRefundTiles, debrisContainerElement);
    foreach (Component cmp in debrisObjects)
    {
      RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
      smi.StartSM();
      smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
    }
  }

  private void TransferBuildingMaterials(out HashSet<int> noRefundTiles)
  {
    HashSet<int> retTemplateFoundationCells = new HashSet<int>();
    ListPool<ScenePartitionerEntry, ClusterManager>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
    GameScenePartitioner.Instance.GatherEntries((int) this.minimumBounds.x, (int) this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.completeBuildings, (List<ScenePartitionerEntry>) gathered_entries);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      BuildingComplete cmp = partitionerEntry.obj as BuildingComplete;
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null)
      {
        Deconstructable component1 = cmp.GetComponent<Deconstructable>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && !cmp.HasTag(GameTags.NoRocketRefund))
        {
          PrimaryElement component2 = cmp.GetComponent<PrimaryElement>();
          float temperature = component2.Temperature;
          byte diseaseIdx = component2.DiseaseIdx;
          int diseaseCount = component2.DiseaseCount;
          for (int index1 = 0; index1 < component1.constructionElements.Length && cmp.Def.Mass.Length > index1; ++index1)
          {
            Element element = ElementLoader.GetElement(component1.constructionElements[index1]);
            if (element != null)
            {
              element.substance.SpawnResource(cmp.transform.GetPosition(), cmp.Def.Mass[index1], temperature, diseaseIdx, diseaseCount);
            }
            else
            {
              GameObject prefab = Assets.GetPrefab(component1.constructionElements[index1]);
              for (int index2 = 0; (double) index2 < (double) cmp.Def.Mass[index1]; ++index2)
                GameUtil.KInstantiate(prefab, cmp.transform.GetPosition(), Grid.SceneLayer.Ore).SetActive(true);
            }
          }
        }
        SimCellOccupier component3 = cmp.GetComponent<SimCellOccupier>();
        if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && component3.doReplaceElement)
          cmp.RunOnArea((Action<int>) (cell => retTemplateFoundationCells.Add(cell)));
        Storage component4 = cmp.GetComponent<Storage>();
        if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
          component4.DropAll();
        PlantablePlot component5 = cmp.GetComponent<PlantablePlot>();
        if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
        {
          SeedProducer component6 = (UnityEngine.Object) component5.Occupant != (UnityEngine.Object) null ? component5.Occupant.GetComponent<SeedProducer>() : (SeedProducer) null;
          if ((UnityEngine.Object) component6 != (UnityEngine.Object) null)
            component6.DropSeed();
        }
        cmp.DeleteObject();
      }
    }
    gathered_entries.Clear();
    noRefundTiles = retTemplateFoundationCells;
  }

  private void TransferPickupables(Vector3 pos)
  {
    int cell = Grid.PosToCell(pos);
    ListPool<ScenePartitionerEntry, ClusterManager>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
    GameScenePartitioner.Instance.GatherEntries((int) this.minimumBounds.x, (int) this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) gathered_entries);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      if (partitionerEntry.obj != null)
      {
        Pickupable pickupable = partitionerEntry.obj as Pickupable;
        if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
          pickupable.gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
      }
    }
    gathered_entries.Recycle();
  }

  private void TransferLiquidsSolidsAndGases(Vector3 pos, HashSet<int> noRefundTiles)
  {
    for (int x = (int) this.minimumBounds.x; (double) x <= (double) this.maximumBounds.x; ++x)
    {
      for (int y = (int) this.minimumBounds.y; (double) y <= (double) this.maximumBounds.y; ++y)
      {
        int cell = Grid.XYToCell(x, y);
        if (!noRefundTiles.Contains(cell))
        {
          Element element = Grid.Element[cell];
          if (element != null && !element.IsVacuum)
            element.substance.SpawnResource(pos, Grid.Mass[cell], Grid.Temperature[cell], Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell]);
        }
      }
    }
  }

  private void TransferPickupablesToDebris(
    ref List<Storage> debrisObjects,
    SimHashes debrisContainerElement)
  {
    ListPool<ScenePartitionerEntry, ClusterManager>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
    GameScenePartitioner.Instance.GatherEntries((int) this.minimumBounds.x, (int) this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) gathered_entries);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      if (partitionerEntry.obj != null)
      {
        Pickupable pickupable1 = partitionerEntry.obj as Pickupable;
        if ((UnityEngine.Object) pickupable1 != (UnityEngine.Object) null)
        {
          if (pickupable1.KPrefabID.HasTag(GameTags.BaseMinion))
          {
            Util.KDestroyGameObject(pickupable1.gameObject);
          }
          else
          {
            pickupable1.PrimaryElement.Units = (float) Mathf.Max(1, Mathf.RoundToInt(pickupable1.PrimaryElement.Units * 0.5f));
            if ((debrisObjects.Count == 0 || (double) debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0.0) && (double) pickupable1.PrimaryElement.Mass > 0.0)
              debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement));
            Storage storage = debrisObjects[debrisObjects.Count - 1];
            while ((double) pickupable1.PrimaryElement.Mass > (double) storage.RemainingCapacity())
            {
              int amount = Mathf.Max(1, Mathf.RoundToInt(storage.RemainingCapacity() / pickupable1.PrimaryElement.MassPerUnit));
              Pickupable pickupable2 = pickupable1.Take((float) amount);
              storage.Store(pickupable2.gameObject);
              storage = CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement);
              debrisObjects.Add(storage);
            }
            if ((double) pickupable1.PrimaryElement.Mass > 0.0)
              storage.Store(pickupable1.gameObject);
          }
        }
      }
    }
    gathered_entries.Recycle();
  }

  private void TransferLiquidsSolidsAndGasesToDebris(
    ref List<Storage> debrisObjects,
    HashSet<int> noRefundTiles,
    SimHashes debrisContainerElement)
  {
    for (int x = (int) this.minimumBounds.x; (double) x <= (double) this.maximumBounds.x; ++x)
    {
      for (int y = (int) this.minimumBounds.y; (double) y <= (double) this.maximumBounds.y; ++y)
      {
        int cell = Grid.XYToCell(x, y);
        if (!noRefundTiles.Contains(cell))
        {
          Element element = Grid.Element[cell];
          if (element != null && !element.IsVacuum)
          {
            float a = Grid.Mass[cell] * 0.5f;
            if ((debrisObjects.Count == 0 || (double) debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0.0) && (double) a > 0.0)
              debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement));
            Storage storage = debrisObjects[debrisObjects.Count - 1];
            while ((double) a > 0.0)
            {
              float mass = Mathf.Min(a, storage.RemainingCapacity());
              a -= mass;
              storage.AddOre(element.id, mass, Grid.Temperature[cell], Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell]);
              if ((double) a > 0.0)
              {
                storage = CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement);
                debrisObjects.Add(storage);
              }
            }
          }
        }
      }
    }
  }

  public void CancelChores()
  {
    for (int layer = 0; layer < 45; ++layer)
    {
      for (int x = (int) this.minimumBounds.x; (double) x <= (double) this.maximumBounds.x; ++x)
      {
        for (int y = (int) this.minimumBounds.y; (double) y <= (double) this.maximumBounds.y; ++y)
        {
          int cell = Grid.XYToCell(x, y);
          GameObject go = Grid.Objects[cell, layer];
          if ((UnityEngine.Object) go != (UnityEngine.Object) null)
            go.Trigger(2127324410, (object) BoxedBools.True);
        }
      }
    }
    List<Chore> choreList;
    GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(this.id, out choreList);
    for (int index = 0; choreList != null && index < choreList.Count; ++index)
    {
      Chore chore = choreList[index];
      if (chore != null && chore.target != null && !chore.isNull)
        chore.Cancel("World destroyed");
    }
    List<FetchChore> fetchChoreList;
    GlobalChoreProvider.Instance.fetchMap.TryGetValue(this.id, out fetchChoreList);
    for (int index = 0; fetchChoreList != null && index < fetchChoreList.Count; ++index)
    {
      FetchChore fetchChore = fetchChoreList[index];
      if (fetchChore != null && fetchChore.target != null && !fetchChore.isNull)
        fetchChore.Cancel("World destroyed");
    }
  }

  public void ClearWorldZones()
  {
    if (this.overworldCell != null)
    {
      WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
      int index1 = -1;
      for (int index2 = 0; index2 < SaveLoader.Instance.clusterDetailSave.overworldCells.Count; ++index2)
      {
        WorldDetailSave.OverworldCell overworldCell = SaveLoader.Instance.clusterDetailSave.overworldCells[index2];
        if (overworldCell.zoneType == this.overworldCell.zoneType && overworldCell.tags != null && this.overworldCell.tags != null && overworldCell.tags.ContainsAll(this.overworldCell.tags) && overworldCell.poly.bounds == this.overworldCell.poly.bounds)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 >= 0)
        clusterDetailSave.overworldCells.RemoveAt(index1);
    }
    for (int y = (int) this.minimumBounds.y; (double) y <= (double) this.maximumBounds.y; ++y)
    {
      for (int x = (int) this.minimumBounds.x; (double) x <= (double) this.maximumBounds.x; ++x)
        SimMessages.ModifyCellWorldZone(Grid.XYToCell(x, y), byte.MaxValue);
    }
  }

  public int GetSafeCell()
  {
    if (this.IsModuleInterior)
    {
      foreach (RocketControlStation rocketControlStation in Components.RocketControlStations.Items)
      {
        if (rocketControlStation.GetMyWorldId() == this.id)
          return Grid.PosToCell((KMonoBehaviour) rocketControlStation);
      }
    }
    else
    {
      foreach (Telepad telepad in Components.Telepads.Items)
      {
        if (telepad.GetMyWorldId() == this.id)
          return Grid.PosToCell((KMonoBehaviour) telepad);
      }
    }
    return Grid.XYToCell(this.worldOffset.x + this.worldSize.x / 2, this.worldOffset.y + this.worldSize.y / 2);
  }

  public string GetStatus()
  {
    return ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultStatus(this.id);
  }
}
