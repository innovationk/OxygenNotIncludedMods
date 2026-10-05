// Decompiled with JetBrains decompiler
// Type: BuildingDef
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using ProcGen;
using Rendering;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class BuildingDef : Def, IHasDlcRestrictions
{
  public string[] RequiredDlcIds;
  public string[] ForbiddenDlcIds;
  public float EnergyConsumptionWhenActive;
  public float GeneratorWattageRating;
  public float GeneratorBaseCapacity;
  public float MassForTemperatureModification;
  public float ExhaustKilowattsWhenActive;
  public float SelfHeatKilowattsWhenActive;
  public float BaseMeltingPoint;
  public float ConstructionTime;
  public float WorkTime;
  public float ThermalConductivity = 1f;
  public int WidthInCells;
  public int HeightInCells;
  public int HitPoints;
  public float Temperature = 293.15f;
  public bool RequiresPowerInput;
  public bool AddLogicPowerPort = true;
  public bool RequiresPowerOutput;
  public bool UseWhitePowerOutputConnectorColour;
  public CellOffset ElectricalArrowOffset;
  public ConduitType InputConduitType;
  public ConduitType OutputConduitType;
  public bool ModifiesTemperature;
  public bool Floodable = true;
  public bool Disinfectable = true;
  public bool Entombable = true;
  public bool Replaceable = true;
  public bool Invincible;
  public bool Overheatable = true;
  public bool Repairable = true;
  public float OverheatTemperature = 348.15f;
  public float FatalHot = 533.15f;
  public bool Breakable;
  public bool ContinuouslyCheckFoundation;
  public bool IsFoundation;
  [Obsolete]
  public bool isSolidTile;
  public bool DragBuild;
  public bool UseStructureTemperature = true;
  public Action HotKey = Action.NumActions;
  public CellOffset attachablePosition = new CellOffset(0, 0);
  public bool CanMove;
  public bool Cancellable = true;
  public bool OnePerWorld;
  public bool PlayConstructionSounds = true;
  public Func<CodexEntry, CodexEntry> ExtendCodexEntry;
  public bool POIUnlockable;
  public List<Tag> ReplacementTags;
  private readonly List<string> searchTerms = new List<string>();
  public List<ObjectLayer> ReplacementCandidateLayers;
  public List<ObjectLayer> EquivalentReplacementLayers;
  [HashedEnum]
  [NonSerialized]
  public HashedString ViewMode = OverlayModes.None.ID;
  public BuildLocationRule BuildLocationRule;
  public ObjectLayer ObjectLayer = ObjectLayer.Building;
  public ObjectLayer TileLayer = ObjectLayer.NumLayers;
  public ObjectLayer ReplacementLayer = ObjectLayer.NumLayers;
  public string DiseaseCellVisName;
  public string[] MaterialCategory;
  public string AudioCategory = "Metal";
  public string AudioSize = "medium";
  public float[] Mass;
  public bool AlwaysOperational;
  public List<LogicPorts.Port> LogicInputPorts;
  public List<LogicPorts.Port> LogicOutputPorts;
  public bool Upgradeable;
  public float BaseTimeUntilRepair = 600f;
  public bool ShowInBuildMenu = true;
  public bool DebugOnly;
  public PermittedRotations PermittedRotations;
  public Orientation InitialOrientation;
  public bool Deprecated;
  public bool UseHighEnergyParticleInputPort;
  public bool UseHighEnergyParticleOutputPort;
  public CellOffset HighEnergyParticleInputOffset;
  public CellOffset HighEnergyParticleOutputOffset;
  public CellOffset PowerInputOffset;
  public CellOffset PowerOutputOffset;
  public CellOffset UtilityInputOffset = new CellOffset(0, 1);
  public CellOffset UtilityOutputOffset = new CellOffset(1, 0);
  public Grid.SceneLayer SceneLayer = Grid.SceneLayer.Building;
  public Grid.SceneLayer ForegroundLayer = Grid.SceneLayer.BuildingFront;
  public string RequiredAttribute = "";
  public int RequiredAttributeLevel;
  public List<Descriptor> EffectDescription;
  public float MassTier;
  public float HeatTier;
  public float ConstructionTimeTier;
  public string PrimaryUse;
  public string SecondaryUse;
  public string PrimarySideEffect;
  public string SecondarySideEffect;
  public Recipe CraftRecipe;
  public Sprite UISprite;
  public bool isKAnimTile;
  public bool isUtility;
  public KAnimFile[] AnimFiles;
  public string DefaultAnimState = "off";
  public bool BlockTileIsTransparent;
  public TextureAtlas BlockTileAtlas;
  public TextureAtlas BlockTilePlaceAtlas;
  public TextureAtlas BlockTileShineAtlas;
  public Material BlockTileMaterial;
  public TilesBlendActiveOptions BlockTileBlendOptions;
  public BlockTileDecorInfo DecorBlockTileInfo;
  public BlockTileDecorInfo DecorPlaceBlockTileInfo;
  public List<Klei.AI.Attribute> attributes = new List<Klei.AI.Attribute>();
  public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
  public Tag AttachmentSlotTag;
  public bool PreventIdleTraversalPastBuilding;
  public bool PreventBuildOverPlants;
  public GameObject BuildingComplete;
  public GameObject BuildingPreview;
  public GameObject BuildingUnderConstruction;
  public CellOffset[] PlacementOffsets;
  public CellOffset[] ConstructionOffsetFilter;
  public static CellOffset[] ConstructionOffsetFilter_OneDown = new CellOffset[1]
  {
    new CellOffset(0, -1)
  };
  public float BaseDecor;
  public float BaseDecorRadius;
  public int BaseNoisePollution;
  public int BaseNoisePollutionRadius;
  public List<string> AvailableFacades = new List<string>();
  public string RequiredSkillPerkID;
  private static Dictionary<CellOffset, CellOffset[]> placementOffsetsCache = new Dictionary<CellOffset, CellOffset[]>();

  public IReadOnlyList<string> SearchTerms => (IReadOnlyList<string>) this.searchTerms;

  public override string Name
  {
    get => (string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{this.PrefabID.ToUpper()}.NAME");
  }

  public string Desc
  {
    get => (string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{this.PrefabID.ToUpper()}.DESC");
  }

  public string Flavor
  {
    get
    {
      return $"\"{(string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{this.PrefabID.ToUpper()}.FLAVOR")}\"";
    }
  }

  public string Effect
  {
    get => (string) Strings.Get($"STRINGS.BUILDINGS.PREFABS.{this.PrefabID.ToUpper()}.EFFECT");
  }

  public bool IsTilePiece => this.TileLayer != ObjectLayer.NumLayers;

  public bool CanReplace(GameObject go)
  {
    return this.ReplacementTags != null && go.GetComponent<KPrefabID>().HasAnyTags(this.ReplacementTags);
  }

  public bool IsAvailable()
  {
    if (this.Deprecated)
      return false;
    return !this.DebugOnly || Game.Instance.DebugOnlyBuildingsAllowed;
  }

  public bool ShouldShowInBuildMenu() => this.ShowInBuildMenu;

  public bool IsReplacementLayerOccupied(int cell)
  {
    if ((UnityEngine.Object) Grid.Objects[cell, (int) this.ReplacementLayer] != (UnityEngine.Object) null)
      return true;
    if (this.EquivalentReplacementLayers != null)
    {
      foreach (ObjectLayer replacementLayer in this.EquivalentReplacementLayers)
      {
        if ((UnityEngine.Object) Grid.Objects[cell, (int) replacementLayer] != (UnityEngine.Object) null)
          return true;
      }
    }
    return false;
  }

  public GameObject GetReplacementCandidate(int cell)
  {
    if (this.ReplacementCandidateLayers != null)
    {
      foreach (ObjectLayer replacementCandidateLayer in this.ReplacementCandidateLayers)
      {
        if (Grid.ObjectLayers[(int) replacementCandidateLayer].ContainsKey(cell))
        {
          GameObject replacementCandidate = Grid.ObjectLayers[(int) replacementCandidateLayer][cell];
          if ((UnityEngine.Object) replacementCandidate != (UnityEngine.Object) null && (UnityEngine.Object) replacementCandidate.GetComponent<global::BuildingComplete>() != (UnityEngine.Object) null)
            return replacementCandidate;
        }
      }
    }
    else if (Grid.ObjectLayers[(int) this.TileLayer].ContainsKey(cell))
      return Grid.ObjectLayers[(int) this.TileLayer][cell];
    return (GameObject) null;
  }

  public GameObject Create(
    Vector3 pos,
    Storage resource_storage,
    IList<Tag> selected_elements,
    Recipe recipe,
    float temperature,
    GameObject obj)
  {
    SimUtil.DiseaseInfo a = SimUtil.DiseaseInfo.Invalid;
    if ((UnityEngine.Object) resource_storage != (UnityEngine.Object) null)
    {
      Recipe.Ingredient[] allIngredients = recipe.GetAllIngredients(selected_elements);
      if (allIngredients != null)
      {
        foreach (Recipe.Ingredient ingredient in allIngredients)
        {
          SimUtil.DiseaseInfo disease_info;
          resource_storage.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out float _, out disease_info, out float _);
          a = SimUtil.CalculateFinalDiseaseInfo(a, disease_info);
        }
      }
    }
    GameObject gameObject = GameUtil.KInstantiate(obj, pos, this.SceneLayer);
    Element element = ElementLoader.GetElement(selected_elements[0]);
    Debug.Assert(element != null);
    PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
    component.ElementID = element.id;
    component.Temperature = temperature;
    component.AddDisease(a.idx, a.count, "BuildingDef.Create");
    gameObject.name = obj.name;
    gameObject.SetActive(true);
    return gameObject;
  }

  public List<Tag> DefaultElements()
  {
    List<Tag> tagList = new List<Tag>();
    foreach (string _materialTypeTag in this.MaterialCategory)
    {
      List<Tag> validMaterials = MaterialSelector.GetValidMaterials((Tag) _materialTypeTag);
      if (validMaterials.Count != 0)
        tagList.Add(validMaterials[0]);
    }
    return tagList;
  }

  public List<Tag> DefaultElementsWithPrimary(Tag primaryElement)
  {
    List<Tag> tagList = this.DefaultElements();
    tagList[0] = primaryElement;
    return tagList;
  }

  public GameObject Build(
    int cell,
    Orientation orientation,
    Storage resource_storage,
    IList<Tag> selected_elements,
    float temperature,
    string facadeID,
    bool playsound = true,
    float timeBuilt = -1f)
  {
    GameObject gameObject = this.Build(cell, orientation, resource_storage, selected_elements, temperature, playsound, timeBuilt);
    if (facadeID != null && facadeID != "DEFAULT_FACADE")
      gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID));
    return gameObject;
  }

  public GameObject Build(
    int cell,
    Orientation orientation,
    Storage resource_storage,
    IList<Tag> selected_elements,
    float temperature,
    bool playsound = true,
    float timeBuilt = -1f)
  {
    GameObject gameObject = this.Create(Grid.CellToPosCBC(cell, this.SceneLayer), resource_storage, selected_elements, this.CraftRecipe, temperature, this.BuildingComplete);
    Rotatable component1 = gameObject.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetOrientation(orientation);
    this.MarkArea(cell, orientation, this.ObjectLayer, gameObject);
    if (this.IsTilePiece)
    {
      this.MarkArea(cell, orientation, this.TileLayer, gameObject);
      this.RunOnArea(cell, orientation, (Action<int>) (c => TileVisualizer.RefreshCell(c, this.TileLayer, this.ReplacementLayer)));
    }
    if (this.PlayConstructionSounds)
    {
      string sound = GlobalAssets.GetSound("Finish_Building_" + this.AudioSize);
      if (playsound && sound != null)
      {
        Vector3 position = gameObject.transform.GetPosition() with
        {
          z = 0.0f
        };
        KFMOD.PlayOneShot(sound, position);
      }
    }
    Deconstructable component2 = gameObject.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      component2.constructionElements = new Tag[selected_elements.Count];
      for (int index = 0; index < selected_elements.Count; ++index)
        component2.constructionElements[index] = selected_elements[index];
    }
    global::BuildingComplete component3 = gameObject.GetComponent<global::BuildingComplete>();
    if ((bool) (UnityEngine.Object) component3)
      component3.SetCreationTime(timeBuilt);
    Game.Instance.Trigger(-1661515756, (object) gameObject);
    gameObject.Trigger(-1661515756, (object) gameObject);
    return gameObject;
  }

  public GameObject TryPlace(
    GameObject src_go,
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    int layer = 0)
  {
    return this.TryPlace(src_go, pos, orientation, selected_elements, (string) null);
  }

  public GameObject TryPlace(
    GameObject src_go,
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    string facadeID,
    int layer = 0)
  {
    return this.TryPlace(src_go, pos, orientation, selected_elements, facadeID, true, layer);
  }

  public GameObject TryPlace(
    GameObject src_go,
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    string facadeID,
    bool restrictToActiveWorld,
    int layer = 0)
  {
    GameObject gameObject = (GameObject) null;
    if (this.IsValidPlaceLocation(src_go, Grid.PosToCell(pos), orientation, false, out string _, restrictToActiveWorld))
    {
      gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
      if (orientation != Orientation.Neutral)
      {
        Rotatable component = gameObject.GetComponent<Rotatable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.SetOrientation(orientation);
      }
    }
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && facadeID != null && facadeID != "DEFAULT_FACADE")
    {
      gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID));
      gameObject.GetComponent<KBatchedAnimController>().Play((HashedString) "place");
    }
    return gameObject;
  }

  public GameObject TryReplaceTile(
    GameObject src_go,
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    int layer = 0)
  {
    GameObject gameObject = (GameObject) null;
    if (this.IsValidPlaceLocation(src_go, pos, orientation, true, out string _))
    {
      Constructable component1 = this.BuildingUnderConstruction.GetComponent<Constructable>();
      component1.IsReplacementTile = true;
      gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
      component1.IsReplacementTile = false;
      if (orientation != Orientation.Neutral)
      {
        Rotatable component2 = gameObject.GetComponent<Rotatable>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.SetOrientation(orientation);
      }
    }
    return gameObject;
  }

  public GameObject TryReplaceTile(
    GameObject src_go,
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    string facadeID,
    int layer = 0)
  {
    GameObject gameObject = this.TryReplaceTile(src_go, pos, orientation, selected_elements, layer);
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      if (facadeID != null && facadeID != "DEFAULT_FACADE")
        gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID));
      if (orientation != Orientation.Neutral)
      {
        Rotatable component = gameObject.GetComponent<Rotatable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.SetOrientation(orientation);
      }
    }
    return gameObject;
  }

  public GameObject Instantiate(
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    int layer = 0)
  {
    float num = -0.15f;
    pos.z += num;
    GameObject gameObject = GameUtil.KInstantiate(this.BuildingUnderConstruction, pos, Grid.SceneLayer.Front, gameLayer: layer);
    Element element = ElementLoader.GetElement(selected_elements[0]);
    Debug.Assert(element != null, (object) "Missing primary element for BuildingDef");
    gameObject.GetComponent<PrimaryElement>().ElementID = element.id;
    gameObject.GetComponent<Constructable>().SelectedElementsTags = selected_elements;
    gameObject.SetActive(true);
    return gameObject;
  }

  private bool IsAreaClear(
    GameObject source_go,
    int cell,
    Orientation orientation,
    ObjectLayer layer,
    ObjectLayer tile_layer,
    bool replace_tile,
    out string fail_reason)
  {
    return this.IsAreaClear(source_go, cell, orientation, layer, tile_layer, replace_tile, true, out fail_reason, !this.PreventBuildOverPlants);
  }

  private bool IsAreaClear(
    GameObject source_go,
    int cell,
    Orientation orientation,
    ObjectLayer layer,
    ObjectLayer tile_layer,
    bool replace_tile,
    bool restrictToActiveWorld,
    out string fail_reason,
    bool permitUproots = true)
  {
    bool flag1 = true;
    fail_reason = (string) null;
    for (int index1 = 0; index1 < this.PlacementOffsets.Length; ++index1)
    {
      CellOffset rotatedCellOffset1 = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index1], orientation);
      if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset1))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        flag1 = false;
        break;
      }
      int cell1 = Grid.OffsetCell(cell, rotatedCellOffset1);
      if (restrictToActiveWorld && (int) Grid.WorldIdx[cell1] != ClusterManager.Instance.activeWorldId)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        return false;
      }
      if (!Grid.IsValidBuildingCell(cell1))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        flag1 = false;
        break;
      }
      if (Grid.Element[cell1].id == SimHashes.Unobtanium)
      {
        fail_reason = (string) null;
        flag1 = false;
        break;
      }
      int num = this.BuildLocationRule == BuildLocationRule.LogicBridge || this.BuildLocationRule == BuildLocationRule.Conduit ? 1 : (this.BuildLocationRule == BuildLocationRule.WireBridge ? 1 : 0);
      GameObject gameObject1 = (GameObject) null;
      if (replace_tile)
        gameObject1 = this.GetReplacementCandidate(cell1);
      if (num == 0)
      {
        GameObject gameObject2 = Grid.Objects[cell1, (int) layer];
        bool flag2 = false;
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
        {
          Building component = gameObject2.GetComponent<Building>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            switch (component.Def.BuildLocationRule)
            {
              case BuildLocationRule.Conduit:
              case BuildLocationRule.LogicBridge:
              case BuildLocationRule.WireBridge:
                flag2 = true;
                break;
            }
          }
        }
        if (!flag2 && (!permitUproots || !Uprootable.CanUproot(gameObject2) ? 0 : (!gameObject2.HasTag(GameTags.BlockBuildOverPlantFeature) ? 1 : 0)) == 0)
        {
          if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject2 != (UnityEngine.Object) source_go && ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null || (UnityEngine.Object) gameObject1 != (UnityEngine.Object) gameObject2) && ((UnityEngine.Object) gameObject2.GetComponent<Wire>() == (UnityEngine.Object) null || (UnityEngine.Object) this.BuildingComplete.GetComponent<Wire>() == (UnityEngine.Object) null))
          {
            fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
            flag1 = false;
            break;
          }
          if (tile_layer != ObjectLayer.NumLayers && ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null || (UnityEngine.Object) gameObject1 == (UnityEngine.Object) source_go) && (UnityEngine.Object) Grid.Objects[cell1, (int) tile_layer] != (UnityEngine.Object) null && (UnityEngine.Object) Grid.Objects[cell1, (int) tile_layer].GetComponent<global::BuildingPreview>() == (UnityEngine.Object) null)
          {
            fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
            flag1 = false;
            break;
          }
        }
      }
      if (layer == ObjectLayer.Building)
      {
        if (this.AttachmentSlotTag != GameTags.Rocket && (UnityEngine.Object) Grid.Objects[cell1, 39] != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) this.BuildingComplete.GetComponent<Wire>() == (UnityEngine.Object) null)
          {
            fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
            flag1 = false;
            break;
          }
          break;
        }
        GameObject gameObject3 = Grid.Objects[cell1, 38];
        if ((UnityEngine.Object) gameObject3 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject3 != (UnityEngine.Object) source_go && (UnityEngine.Object) gameObject3.GetComponent<global::BuildingPreview>() == (UnityEngine.Object) null)
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
          flag1 = false;
          break;
        }
      }
      if (layer == ObjectLayer.Gantry)
      {
        bool flag3 = false;
        MakeBaseSolid.Def def = source_go.GetDef<MakeBaseSolid.Def>();
        for (int index2 = 0; index2 < def.solidOffsets.Length; ++index2)
        {
          CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(def.solidOffsets[index2], orientation);
          flag3 |= rotatedCellOffset2 == rotatedCellOffset1;
        }
        if (flag3 && !this.IsValidTileLocation(source_go, cell1, replace_tile, ref fail_reason))
        {
          flag1 = false;
          break;
        }
        GameObject gameObject4 = Grid.Objects[cell1, 1];
        if ((UnityEngine.Object) gameObject4 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject4.GetComponent<global::BuildingPreview>() == (UnityEngine.Object) null)
        {
          Building component = gameObject4.GetComponent<Building>();
          if (flag3 || (UnityEngine.Object) component == (UnityEngine.Object) null || component.Def.AttachmentSlotTag != GameTags.Rocket)
          {
            fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
            flag1 = false;
            break;
          }
        }
      }
      if (this.BuildLocationRule == BuildLocationRule.Tile)
      {
        if (!this.IsValidTileLocation(source_go, cell1, replace_tile, ref fail_reason))
        {
          flag1 = false;
          break;
        }
      }
      else if (this.BuildLocationRule == BuildLocationRule.OnFloorOverSpace)
      {
        if (World.Instance.zoneRenderData.GetSubWorldZoneType(cell1) != SubWorld.ZoneType.Space)
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
          flag1 = false;
          break;
        }
      }
      else if (this.BuildLocationRule == BuildLocationRule.BuildingAttachPoint)
      {
        GameObject gameObject5 = Grid.Objects[cell1, 1];
        if ((UnityEngine.Object) gameObject5 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject5 != (UnityEngine.Object) source_go && (UnityEngine.Object) gameObject5.GetComponent<global::BuildingPreview>() == (UnityEngine.Object) null)
        {
          int cell2 = Grid.OffsetCell(cell, this.attachablePosition);
          BuildingAttachPoint component = gameObject5.GetComponent<BuildingAttachPoint>();
          if ((!((UnityEngine.Object) component != (UnityEngine.Object) null) ? 0 : (component.AcceptsAttachment(this.AttachmentSlotTag, cell2) ? 1 : 0)) == 0)
          {
            fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
            flag1 = false;
            break;
          }
        }
      }
    }
    if (!flag1)
      return false;
    if (layer == ObjectLayer.LiquidConduit)
    {
      GameObject gameObject = Grid.Objects[cell, 19];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        Building component = gameObject.GetComponent<Building>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.BuildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin && component.GetCell() == cell)
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN;
          return false;
        }
      }
    }
    switch (this.BuildLocationRule)
    {
      case BuildLocationRule.NotInTiles:
        GameObject gameObject6 = Grid.Objects[cell, 9];
        if (!replace_tile && (UnityEngine.Object) gameObject6 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject6 != (UnityEngine.Object) source_go)
          flag1 = false;
        else if (Grid.HasDoor[cell])
        {
          flag1 = false;
        }
        else
        {
          GameObject gameObject7 = Grid.Objects[cell, (int) this.ObjectLayer];
          if ((UnityEngine.Object) gameObject7 != (UnityEngine.Object) null)
          {
            if (this.ReplacementLayer == ObjectLayer.NumLayers)
            {
              if ((UnityEngine.Object) gameObject7 != (UnityEngine.Object) source_go)
                flag1 = false;
            }
            else
            {
              Building component = gameObject7.GetComponent<Building>();
              if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.ReplacementLayer != this.ReplacementLayer)
                flag1 = false;
            }
          }
        }
        if (!flag1)
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
          break;
        }
        break;
      case BuildLocationRule.WireBridge:
        return this.IsValidWireBridgeLocation(source_go, cell, orientation, out fail_reason);
      case BuildLocationRule.HighWattBridgeTile:
        flag1 = this.IsValidTileLocation(source_go, cell, replace_tile, ref fail_reason) && this.IsValidHighWattBridgeLocation(source_go, cell, orientation, out fail_reason);
        break;
      case BuildLocationRule.BuildingAttachPoint:
        flag1 = false;
        for (int idx = 0; idx < Components.BuildingAttachPoints.Count && !flag1; ++idx)
        {
          for (int index = 0; index < Components.BuildingAttachPoints[idx].points.Length; ++index)
          {
            if (Components.BuildingAttachPoints[idx].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
            {
              flag1 = true;
              break;
            }
          }
        }
        if (!flag1)
        {
          fail_reason = string.Format((string) UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, (object) this.AttachmentSlotTag);
          break;
        }
        break;
      case BuildLocationRule.NoLiquidConduitAtOrigin:
        flag1 = (UnityEngine.Object) Grid.Objects[cell, 16 /*0x10*/] == (UnityEngine.Object) null && ((UnityEngine.Object) Grid.Objects[cell, 19] == (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[cell, 19] == (UnityEngine.Object) source_go);
        break;
    }
    return flag1 && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason) && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason) && this.AreLogicPortsInValidPositions(source_go, cell, out fail_reason);
  }

  private bool IsValidTileLocation(
    GameObject source_go,
    int cell,
    bool replacement_tile,
    ref string fail_reason)
  {
    GameObject gameObject1 = Grid.Objects[cell, 27];
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) source_go && gameObject1.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.NotInTiles)
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
      return false;
    }
    GameObject gameObject2 = Grid.Objects[cell, 29];
    if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject2 != (UnityEngine.Object) source_go && gameObject2.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
      return false;
    }
    GameObject gameObject3 = Grid.Objects[cell, 2];
    if ((UnityEngine.Object) gameObject3 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject3 != (UnityEngine.Object) source_go)
    {
      Building component = gameObject3.GetComponent<Building>();
      if (!replacement_tile && (UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_BACK_WALL;
        return false;
      }
    }
    return true;
  }

  public void RunOnArea(int cell, Orientation orientation, Action<int> callback)
  {
    for (int index = 0; index < this.PlacementOffsets.Length; ++index)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index], orientation);
      int num = Grid.OffsetCell(cell, rotatedCellOffset);
      callback(num);
    }
  }

  public void MarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
  {
    if (this.BuildLocationRule != BuildLocationRule.Conduit && this.BuildLocationRule != BuildLocationRule.WireBridge && this.BuildLocationRule != BuildLocationRule.LogicBridge)
    {
      for (int index = 0; index < this.PlacementOffsets.Length; ++index)
      {
        CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index], orientation);
        int cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
        GameObject plant = Grid.Objects[cell1, (int) layer];
        if (Uprootable.CanUproot(plant))
          Grid.Objects[cell1, 5] = plant;
        Grid.Objects[cell1, (int) layer] = go;
      }
    }
    if (this.InputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
      int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
      this.MarkOverlappingPorts(Grid.Objects[cell2, (int) layerForConduitType], go);
      Grid.Objects[cell2, (int) layerForConduitType] = go;
    }
    if (this.OutputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
      int cell3 = Grid.OffsetCell(cell, rotatedCellOffset);
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
      this.MarkOverlappingPorts(Grid.Objects[cell3, (int) layerForConduitType], go);
      Grid.Objects[cell3, (int) layerForConduitType] = go;
    }
    if (this.RequiresPowerInput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
      int cell4 = Grid.OffsetCell(cell, rotatedCellOffset);
      this.MarkOverlappingPorts(Grid.Objects[cell4, 29], go);
      Grid.Objects[cell4, 29] = go;
    }
    if (this.RequiresPowerOutput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
      int cell5 = Grid.OffsetCell(cell, rotatedCellOffset);
      this.MarkOverlappingPorts(Grid.Objects[cell5, 29], go);
      Grid.Objects[cell5, 29] = go;
    }
    if (this.BuildLocationRule == BuildLocationRule.WireBridge || this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
    {
      int linked_cell1;
      int linked_cell2;
      go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out linked_cell1, out linked_cell2);
      this.MarkOverlappingPorts(Grid.Objects[linked_cell1, 29], go);
      this.MarkOverlappingPorts(Grid.Objects[linked_cell2, 29], go);
      Grid.Objects[linked_cell1, 29] = go;
      Grid.Objects[linked_cell2, 29] = go;
    }
    if (this.BuildLocationRule == BuildLocationRule.LogicBridge)
    {
      LogicPorts component = go.GetComponent<LogicPorts>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.inputPortInfo != null)
      {
        foreach (LogicPorts.Port port in component.inputPortInfo)
        {
          CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(port.cellOffset, orientation);
          int cell6 = Grid.OffsetCell(cell, rotatedCellOffset);
          this.MarkOverlappingLogicPorts(Grid.Objects[cell6, (int) layer], go, cell6);
          Grid.Objects[cell6, (int) layer] = go;
        }
      }
    }
    ISecondaryInput[] components1 = this.BuildingComplete.GetComponents<ISecondaryInput>();
    if (components1 != null)
    {
      foreach (ISecondaryInput secondaryInput in components1)
      {
        for (int index = 0; index < 4; ++index)
        {
          ConduitType conduitType = (ConduitType) index;
          if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
          {
            ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(conduitType);
            CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
            int cell7 = Grid.OffsetCell(cell, rotatedCellOffset);
            this.MarkOverlappingPorts(Grid.Objects[cell7, (int) layerForConduitType], go);
            Grid.Objects[cell7, (int) layerForConduitType] = go;
          }
        }
      }
    }
    ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
    if (components2 == null)
      return;
    foreach (ISecondaryOutput secondaryOutput in components2)
    {
      for (int index = 0; index < 4; ++index)
      {
        ConduitType conduitType = (ConduitType) index;
        if (conduitType != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType))
        {
          ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(conduitType);
          CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType), orientation);
          int cell8 = Grid.OffsetCell(cell, rotatedCellOffset);
          this.MarkOverlappingPorts(Grid.Objects[cell8, (int) layerForConduitType], go);
          Grid.Objects[cell8, (int) layerForConduitType] = go;
        }
      }
    }
  }

  public void MarkOverlappingPorts(GameObject existing, GameObject replaced)
  {
    if ((UnityEngine.Object) existing == (UnityEngine.Object) null)
    {
      if (!((UnityEngine.Object) replaced != (UnityEngine.Object) null))
        return;
      replaced.RemoveTag(GameTags.HasInvalidPorts);
    }
    else
    {
      if (!((UnityEngine.Object) existing != (UnityEngine.Object) replaced))
        return;
      existing.AddTag(GameTags.HasInvalidPorts);
    }
  }

  public void MarkOverlappingLogicPorts(GameObject existing, GameObject replaced, int cell)
  {
    if ((UnityEngine.Object) existing == (UnityEngine.Object) null)
    {
      if (!((UnityEngine.Object) replaced != (UnityEngine.Object) null))
        return;
      replaced.RemoveTag(GameTags.HasInvalidPorts);
    }
    else
    {
      if (!((UnityEngine.Object) existing != (UnityEngine.Object) replaced))
        return;
      LogicGate component1 = existing.GetComponent<LogicGate>();
      LogicPorts component2 = existing.GetComponent<LogicPorts>();
      if ((!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || !component2.TryGetPortAtCell(cell, out LogicPorts.Port _, out bool _)) && (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) || !component1.TryGetPortAtCell(cell, out LogicGateBase.PortId _)))
        return;
      existing.AddTag(GameTags.HasInvalidPorts);
    }
  }

  public void UnmarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
  {
    if (cell == Grid.InvalidCell)
      return;
    for (int index = 0; index < this.PlacementOffsets.Length; ++index)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index], orientation);
      int cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
      if ((UnityEngine.Object) Grid.Objects[cell1, (int) layer] == (UnityEngine.Object) go)
        Grid.Objects[cell1, (int) layer] = (GameObject) null;
    }
    if (this.InputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
      int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
      if ((UnityEngine.Object) Grid.Objects[cell2, (int) layerForConduitType] == (UnityEngine.Object) go)
        Grid.Objects[cell2, (int) layerForConduitType] = (GameObject) null;
    }
    if (this.OutputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
      int cell3 = Grid.OffsetCell(cell, rotatedCellOffset);
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
      if ((UnityEngine.Object) Grid.Objects[cell3, (int) layerForConduitType] == (UnityEngine.Object) go)
        Grid.Objects[cell3, (int) layerForConduitType] = (GameObject) null;
    }
    if (this.RequiresPowerInput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
      int cell4 = Grid.OffsetCell(cell, rotatedCellOffset);
      if ((UnityEngine.Object) Grid.Objects[cell4, 29] == (UnityEngine.Object) go)
        Grid.Objects[cell4, 29] = (GameObject) null;
    }
    if (this.RequiresPowerOutput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
      int cell5 = Grid.OffsetCell(cell, rotatedCellOffset);
      if ((UnityEngine.Object) Grid.Objects[cell5, 29] == (UnityEngine.Object) go)
        Grid.Objects[cell5, 29] = (GameObject) null;
    }
    if (this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
    {
      int linked_cell1;
      int linked_cell2;
      go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out linked_cell1, out linked_cell2);
      if ((UnityEngine.Object) Grid.Objects[linked_cell1, 29] == (UnityEngine.Object) go)
        Grid.Objects[linked_cell1, 29] = (GameObject) null;
      if ((UnityEngine.Object) Grid.Objects[linked_cell2, 29] == (UnityEngine.Object) go)
        Grid.Objects[linked_cell2, 29] = (GameObject) null;
    }
    ISecondaryInput[] components1 = this.BuildingComplete.GetComponents<ISecondaryInput>();
    if (components1 != null)
    {
      foreach (ISecondaryInput secondaryInput in components1)
      {
        for (int index = 0; index < 4; ++index)
        {
          ConduitType conduitType = (ConduitType) index;
          if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
          {
            ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(conduitType);
            CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
            int cell6 = Grid.OffsetCell(cell, rotatedCellOffset);
            if ((UnityEngine.Object) Grid.Objects[cell6, (int) layerForConduitType] == (UnityEngine.Object) go)
              Grid.Objects[cell6, (int) layerForConduitType] = (GameObject) null;
          }
        }
      }
    }
    ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
    if (components2 == null)
      return;
    foreach (ISecondaryOutput secondaryOutput in components2)
    {
      for (int index = 0; index < 4; ++index)
      {
        ConduitType conduitType = (ConduitType) index;
        if (conduitType != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType))
        {
          ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(conduitType);
          CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType), orientation);
          int cell7 = Grid.OffsetCell(cell, rotatedCellOffset);
          if ((UnityEngine.Object) Grid.Objects[cell7, (int) layerForConduitType] == (UnityEngine.Object) go)
            Grid.Objects[cell7, (int) layerForConduitType] = (GameObject) null;
        }
      }
    }
  }

  public int GetBuildingCell(int cell) => cell + (this.WidthInCells - 1) / 2;

  public Vector3 GetVisualizerOffset()
  {
    return Vector3.right * (0.5f * (float) ((this.WidthInCells + 1) % 2));
  }

  public bool IsValidPlaceLocation(
    GameObject source_go,
    Vector3 pos,
    Orientation orientation,
    out string fail_reason)
  {
    int cell = Grid.PosToCell(pos);
    return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
  }

  public bool IsValidPlaceLocation(
    GameObject source_go,
    Vector3 pos,
    Orientation orientation,
    bool replace_tile,
    out string fail_reason)
  {
    int cell = Grid.PosToCell(pos);
    return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason);
  }

  public bool IsValidPlaceLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
  }

  public bool IsValidPlaceLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    bool replace_tile,
    out string fail_reason)
  {
    return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason, false);
  }

  public bool IsValidPlaceLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    bool replace_tile,
    out string fail_reason,
    bool restrictToActiveWorld)
  {
    if (!Grid.IsValidBuildingCell(cell))
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
      return false;
    }
    if (restrictToActiveWorld && (int) Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
      return false;
    }
    if (this.BuildLocationRule == BuildLocationRule.OnRocketEnvelope)
    {
      if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
        return false;
      }
    }
    else if (this.BuildLocationRule == BuildLocationRule.OnBackWall)
    {
      if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_BACK_WALL_REQUIRED;
        return false;
      }
    }
    else if (this.BuildLocationRule == BuildLocationRule.OnWall)
    {
      if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
        return false;
      }
    }
    else if (this.BuildLocationRule == BuildLocationRule.InCorner)
    {
      if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
        return false;
      }
    }
    else if (this.BuildLocationRule == BuildLocationRule.WallFloor)
    {
      if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
        return false;
      }
    }
    else if (this.BuildLocationRule == BuildLocationRule.BelowRocketCeiling)
    {
      WorldContainer world = ClusterManager.Instance.GetWorld((int) Grid.WorldIdx[cell]);
      if ((double) (Grid.CellToXY(cell).y + 35 + source_go.GetComponent<Building>().Def.HeightInCells) >= (double) world.maximumBounds.y - (double) Grid.TopBorderHeight)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_BELOWROCKETCEILING;
        return false;
      }
    }
    return this.IsAreaClear(source_go, cell, orientation, this.ObjectLayer, this.TileLayer, replace_tile, restrictToActiveWorld, out fail_reason, !this.PreventBuildOverPlants);
  }

  public bool IsValidReplaceLocation(
    Vector3 pos,
    Orientation orientation,
    ObjectLayer replace_layer,
    ObjectLayer obj_layer)
  {
    if (replace_layer == ObjectLayer.NumLayers)
      return false;
    bool flag = true;
    int cell1 = Grid.PosToCell(pos);
    for (int index = 0; index < this.PlacementOffsets.Length; ++index)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index], orientation);
      int cell2 = Grid.OffsetCell(cell1, rotatedCellOffset);
      if (!Grid.IsValidBuildingCell(cell2))
        return false;
      if ((UnityEngine.Object) Grid.Objects[cell2, (int) obj_layer] == (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[cell2, (int) replace_layer] != (UnityEngine.Object) null)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public bool IsValidBuildLocation(
    GameObject source_go,
    Vector3 pos,
    Orientation orientation,
    bool replace_tile = false)
  {
    string reason = "";
    return this.IsValidBuildLocation(source_go, pos, orientation, out reason, replace_tile);
  }

  public bool IsValidBuildLocation(
    GameObject source_go,
    Vector3 pos,
    Orientation orientation,
    out string reason,
    bool replace_tile = false)
  {
    int cell = Grid.PosToCell(pos);
    return this.IsValidBuildLocation(source_go, cell, orientation, replace_tile, out reason);
  }

  public bool IsValidBuildLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    bool replace_tile,
    out string fail_reason)
  {
    if (!Grid.IsValidBuildingCell(cell))
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
      return false;
    }
    if (!this.IsAreaValid(cell, orientation, out fail_reason))
      return false;
    bool flag = true;
    fail_reason = (string) null;
    switch (this.BuildLocationRule)
    {
      case BuildLocationRule.Anywhere:
      case BuildLocationRule.Conduit:
      case BuildLocationRule.OnFloorOrBuildingAttachPoint:
        flag = true;
        break;
      case BuildLocationRule.OnFloor:
      case BuildLocationRule.OnCeiling:
      case BuildLocationRule.OnFoundationRotatable:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
          break;
        }
        break;
      case BuildLocationRule.OnFloorOverSpace:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
          break;
        }
        if (!BuildingDef.AreAllCellsValid(cell, orientation, this.WidthInCells, this.HeightInCells, (Func<int, bool>) (check_cell => World.Instance.zoneRenderData.GetSubWorldZoneType(check_cell) == SubWorld.ZoneType.Space)))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
          break;
        }
        break;
      case BuildLocationRule.OnWall:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
          break;
        }
        break;
      case BuildLocationRule.InCorner:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
          break;
        }
        break;
      case BuildLocationRule.Tile:
        flag = true;
        GameObject gameObject1 = Grid.Objects[cell, 27];
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
        {
          Building component = gameObject1.GetComponent<Building>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
            flag = false;
        }
        GameObject gameObject2 = Grid.Objects[cell, 2];
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
        {
          Building component = gameObject2.GetComponent<Building>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
          {
            flag = replace_tile;
            break;
          }
          break;
        }
        break;
      case BuildLocationRule.NotInTiles:
        GameObject gameObject3 = Grid.Objects[cell, 9];
        flag = (replace_tile || (UnityEngine.Object) gameObject3 == (UnityEngine.Object) null || (UnityEngine.Object) gameObject3 == (UnityEngine.Object) source_go) && !Grid.HasDoor[cell];
        if (flag)
        {
          GameObject gameObject4 = Grid.Objects[cell, (int) this.ObjectLayer];
          if ((UnityEngine.Object) gameObject4 != (UnityEngine.Object) null)
          {
            if (this.ReplacementLayer == ObjectLayer.NumLayers)
            {
              flag = flag && ((UnityEngine.Object) gameObject4 == (UnityEngine.Object) null || (UnityEngine.Object) gameObject4 == (UnityEngine.Object) source_go);
            }
            else
            {
              Building component = gameObject4.GetComponent<Building>();
              flag = (UnityEngine.Object) component == (UnityEngine.Object) null || component.Def.ReplacementLayer == this.ReplacementLayer;
            }
          }
        }
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
        break;
      case BuildLocationRule.BuildingAttachPoint:
        flag = false;
        for (int idx = 0; idx < Components.BuildingAttachPoints.Count && !flag; ++idx)
        {
          for (int index = 0; index < Components.BuildingAttachPoints[idx].points.Length; ++index)
          {
            if (Components.BuildingAttachPoints[idx].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
            {
              flag = true;
              break;
            }
          }
        }
        fail_reason = string.Format((string) UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, (object) this.AttachmentSlotTag);
        break;
      case BuildLocationRule.OnRocketEnvelope:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
          break;
        }
        break;
      case BuildLocationRule.WallFloor:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
          break;
        }
        break;
    }
    return flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason) && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason);
  }

  private bool IsAreaValid(int cell, Orientation orientation, out string fail_reason)
  {
    bool flag = true;
    fail_reason = (string) null;
    for (int index = 0; index < this.PlacementOffsets.Length; ++index)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index], orientation);
      if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        flag = false;
        break;
      }
      int cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
      if (!Grid.IsValidBuildingCell(cell1))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        flag = false;
        break;
      }
      if (Grid.Element[cell1].id == SimHashes.Unobtanium)
      {
        fail_reason = (string) null;
        flag = false;
        break;
      }
    }
    return flag;
  }

  private bool ArePowerPortsInValidPositions(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    fail_reason = (string) null;
    if ((UnityEngine.Object) source_go == (UnityEngine.Object) null)
      return true;
    if (this.RequiresPowerInput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
      int cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
      GameObject gameObject = Grid.Objects[cell1, 29];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject != (UnityEngine.Object) source_go)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
        return false;
      }
    }
    if (this.RequiresPowerOutput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
      int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
      GameObject gameObject = Grid.Objects[cell2, 29];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject != (UnityEngine.Object) source_go)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
        return false;
      }
    }
    return true;
  }

  private bool AreConduitPortsInValidPositions(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    fail_reason = (string) null;
    if ((UnityEngine.Object) source_go == (UnityEngine.Object) null)
      return true;
    bool flag = true;
    if (this.InputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
      int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
      flag = this.IsValidConduitConnection(source_go, this.InputConduitType, utility_cell, ref fail_reason);
    }
    if (flag && this.OutputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
      int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
      flag = this.IsValidConduitConnection(source_go, this.OutputConduitType, utility_cell, ref fail_reason);
    }
    Building component = source_go.GetComponent<Building>();
    if (flag && (bool) (UnityEngine.Object) component)
    {
      ISecondaryInput[] components = component.Def.BuildingComplete.GetComponents<ISecondaryInput>();
      if (components != null)
      {
        foreach (ISecondaryInput secondaryInput in components)
        {
          for (int index = 0; index < 4; ++index)
          {
            ConduitType conduitType = (ConduitType) index;
            if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
            {
              CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
              int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
              flag = this.IsValidConduitConnection(source_go, conduitType, utility_cell, ref fail_reason);
            }
          }
        }
      }
    }
    if (flag)
    {
      ISecondaryOutput[] components = component.Def.BuildingComplete.GetComponents<ISecondaryOutput>();
      if (components != null)
      {
        foreach (ISecondaryOutput secondaryOutput in components)
        {
          for (int index = 0; index < 4; ++index)
          {
            ConduitType conduitType = (ConduitType) index;
            if (conduitType != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType))
            {
              CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType), orientation);
              int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
              flag = this.IsValidConduitConnection(source_go, conduitType, utility_cell, ref fail_reason);
            }
          }
        }
      }
    }
    return flag;
  }

  private bool IsValidWireBridgeLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    if ((UnityEngine.Object) source_go == (UnityEngine.Object) null)
    {
      fail_reason = (string) null;
      return true;
    }
    UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      int linked_cell1;
      int linked_cell2;
      component.GetCells(out linked_cell1, out linked_cell2);
      if ((UnityEngine.Object) Grid.Objects[linked_cell1, 29] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[linked_cell2, 29] != (UnityEngine.Object) null)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
        return false;
      }
    }
    fail_reason = (string) null;
    return true;
  }

  private bool IsValidHighWattBridgeLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    if ((UnityEngine.Object) source_go == (UnityEngine.Object) null)
    {
      fail_reason = (string) null;
      return true;
    }
    UtilityNetworkLink component1 = source_go.GetComponent<UtilityNetworkLink>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      if (!component1.AreCellsValid(cell, orientation))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        return false;
      }
      int linked_cell1;
      int linked_cell2;
      component1.GetCells(out linked_cell1, out linked_cell2);
      if ((UnityEngine.Object) Grid.Objects[linked_cell1, 29] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[linked_cell2, 29] != (UnityEngine.Object) null)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
        return false;
      }
      if ((UnityEngine.Object) Grid.Objects[linked_cell1, 9] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[linked_cell2, 9] != (UnityEngine.Object) null)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
        return false;
      }
      if (Grid.HasDoor[linked_cell1] || Grid.HasDoor[linked_cell2])
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
        return false;
      }
      GameObject gameObject1 = Grid.Objects[linked_cell1, 1];
      GameObject gameObject2 = Grid.Objects[linked_cell2, 1];
      if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null || (UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
      {
        global::BuildingUnderConstruction component2 = (bool) (UnityEngine.Object) gameObject1 ? gameObject1.GetComponent<global::BuildingUnderConstruction>() : (global::BuildingUnderConstruction) null;
        global::BuildingUnderConstruction component3 = (bool) (UnityEngine.Object) gameObject2 ? gameObject2.GetComponent<global::BuildingUnderConstruction>() : (global::BuildingUnderConstruction) null;
        if ((bool) (UnityEngine.Object) component2 && (bool) (UnityEngine.Object) component2.Def.BuildingComplete.GetComponent<Door>() || (bool) (UnityEngine.Object) component3 && (bool) (UnityEngine.Object) component3.Def.BuildingComplete.GetComponent<Door>())
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
          return false;
        }
      }
    }
    fail_reason = (string) null;
    return true;
  }

  private bool AreLogicPortsInValidPositions(
    GameObject source_go,
    int cell,
    out string fail_reason)
  {
    fail_reason = (string) null;
    if ((UnityEngine.Object) source_go == (UnityEngine.Object) null)
      return true;
    List<ILogicUIElement> visElements = Game.Instance.logicCircuitManager.GetVisElements();
    LogicPorts component1 = source_go.GetComponent<LogicPorts>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.HackRefreshVisualizers();
      if (this.DoLogicPortsConflict((IList<ILogicUIElement>) component1.inputPorts, (IList<ILogicUIElement>) visElements) || this.DoLogicPortsConflict((IList<ILogicUIElement>) component1.outputPorts, (IList<ILogicUIElement>) visElements))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
        return false;
      }
    }
    else
    {
      LogicGateBase component2 = source_go.GetComponent<LogicGateBase>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (this.IsLogicPortObstructed(component2.InputCellOne, (IList<ILogicUIElement>) visElements) || this.IsLogicPortObstructed(component2.OutputCellOne, (IList<ILogicUIElement>) visElements) || (component2.RequiresTwoInputs || component2.RequiresFourInputs) && this.IsLogicPortObstructed(component2.InputCellTwo, (IList<ILogicUIElement>) visElements) || component2.RequiresFourInputs && (this.IsLogicPortObstructed(component2.InputCellThree, (IList<ILogicUIElement>) visElements) || this.IsLogicPortObstructed(component2.InputCellFour, (IList<ILogicUIElement>) visElements)) || component2.RequiresFourOutputs && (this.IsLogicPortObstructed(component2.OutputCellTwo, (IList<ILogicUIElement>) visElements) || this.IsLogicPortObstructed(component2.OutputCellThree, (IList<ILogicUIElement>) visElements) || this.IsLogicPortObstructed(component2.OutputCellFour, (IList<ILogicUIElement>) visElements)) || component2.RequiresControlInputs && (this.IsLogicPortObstructed(component2.ControlCellOne, (IList<ILogicUIElement>) visElements) || this.IsLogicPortObstructed(component2.ControlCellTwo, (IList<ILogicUIElement>) visElements))))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
        return false;
      }
    }
    return true;
  }

  private bool DoLogicPortsConflict(IList<ILogicUIElement> ports_a, IList<ILogicUIElement> ports_b)
  {
    if (ports_a == null || ports_b == null)
      return false;
    foreach (ILogicUIElement logicUiElement1 in (IEnumerable<ILogicUIElement>) ports_a)
    {
      int logicUiCell = logicUiElement1.GetLogicUICell();
      foreach (ILogicUIElement logicUiElement2 in (IEnumerable<ILogicUIElement>) ports_b)
      {
        if (logicUiElement1 != logicUiElement2 && logicUiCell == logicUiElement2.GetLogicUICell())
          return true;
      }
    }
    return false;
  }

  private bool IsLogicPortObstructed(int cell, IList<ILogicUIElement> ports)
  {
    int num = 0;
    foreach (ILogicUIElement port in (IEnumerable<ILogicUIElement>) ports)
    {
      if (port.GetLogicUICell() == cell)
        ++num;
    }
    return num > 0;
  }

  private bool IsValidConduitConnection(
    GameObject source_go,
    ConduitType conduit_type,
    int utility_cell,
    ref string fail_reason)
  {
    bool flag = true;
    switch (conduit_type)
    {
      case ConduitType.Gas:
        GameObject gameObject1 = Grid.Objects[utility_cell, 15];
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) source_go)
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_GASPORTS_OVERLAP;
          break;
        }
        break;
      case ConduitType.Liquid:
        GameObject gameObject2 = Grid.Objects[utility_cell, 19];
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject2 != (UnityEngine.Object) source_go)
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP;
          break;
        }
        break;
      case ConduitType.Solid:
        GameObject gameObject3 = Grid.Objects[utility_cell, 23];
        if ((UnityEngine.Object) gameObject3 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject3 != (UnityEngine.Object) source_go)
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP;
          break;
        }
        break;
    }
    return flag;
  }

  public static int GetXOffset(int width) => -(width - 1) / 2;

  public static bool CheckFoundation(
    int cell,
    Orientation orientation,
    BuildLocationRule location_rule,
    int width,
    int height,
    Tag optionalFoundationRequiredTag = default (Tag))
  {
    switch (location_rule)
    {
      case BuildLocationRule.OnWall:
        return BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
      case BuildLocationRule.InCorner:
        return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnCeiling, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
      case BuildLocationRule.WallFloor:
        return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnFloor, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
      case BuildLocationRule.OnBackWall:
        return BuildingDef.CheckBackWallFoundation(cell, width, height, orientation);
      default:
        return BuildingDef.CheckBaseFoundation(cell, orientation, location_rule, width, height, optionalFoundationRequiredTag);
    }
  }

  public static bool CheckBaseFoundation(
    int cell,
    Orientation orientation,
    BuildLocationRule location_rule,
    int width,
    int height,
    Tag optionalFoundationRequiredTag = default (Tag))
  {
    int num1 = -(width - 1) / 2;
    int num2 = width / 2;
    for (int x = num1; x <= num2; ++x)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(location_rule == BuildLocationRule.OnCeiling ? new CellOffset(x, height) : new CellOffset(x, -1), orientation);
      int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
      if (!Grid.IsValidBuildingCell(num3) || !Grid.Solid[num3] || optionalFoundationRequiredTag.IsValid && (!Grid.ObjectLayers[9].ContainsKey(num3) || !Grid.ObjectLayers[9][num3].HasTag(optionalFoundationRequiredTag)))
        return false;
    }
    return true;
  }

  public static bool CheckBackWallFoundation(
    int cell,
    int width,
    int height,
    Orientation orientation)
  {
    int num1 = height;
    for (int y = 0; y < num1; ++y)
    {
      int num2 = -(width - 1) / 2;
      int num3 = width / 2;
      for (int x = num2; x <= num3; ++x)
      {
        CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(x, y, orientation);
        int num4 = Grid.OffsetCell(cell, rotatedCellOffset);
        if (Grid.Solid[num4])
          return false;
        if (!BackwallManager.HasBackwall(num4))
        {
          GameObject go = Grid.ObjectLayers[2].ContainsKey(num4) ? Grid.ObjectLayers[2][num4] : (GameObject) null;
          if ((UnityEngine.Object) go == (UnityEngine.Object) null || !go.HasTag(GameTags.Backwall))
            return false;
        }
      }
    }
    return true;
  }

  public static bool CheckWallFoundation(int cell, int width, int height, bool leftWall)
  {
    int num1 = height;
    for (int y = 0; y < num1; ++y)
    {
      CellOffset offset = new CellOffset(leftWall ? -(width - 1) / 2 - 1 : width / 2 + 1, y);
      int num2 = Grid.OffsetCell(cell, offset);
      GameObject gameObject = Grid.Objects[num2, 1];
      bool flag = false;
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        global::BuildingUnderConstruction component = gameObject.GetComponent<global::BuildingUnderConstruction>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.IsFoundation)
          flag = true;
      }
      if (!Grid.IsValidBuildingCell(num2) || !(Grid.Solid[num2] | flag))
        return false;
    }
    return true;
  }

  public static bool AreAllCellsValid(
    int base_cell,
    Orientation orientation,
    int width,
    int height,
    Func<int, bool> valid_cell_check)
  {
    int num1 = -(width - 1) / 2;
    int num2 = width / 2;
    if (orientation == Orientation.FlipH)
    {
      int num3 = num1;
      num1 = -num2;
      num2 = -num3;
    }
    for (int y = 0; y < height; ++y)
    {
      for (int x = num1; x <= num2; ++x)
      {
        int num4 = Grid.OffsetCell(base_cell, x, y);
        if (!valid_cell_check(num4))
          return false;
      }
    }
    return true;
  }

  public Sprite GetUISprite(string animName = "ui", bool centered = false)
  {
    return Def.GetUISpriteFromMultiObjectAnim(this.AnimFiles[0], animName, centered);
  }

  public void GenerateOffsets() => this.GenerateOffsets(this.WidthInCells, this.HeightInCells);

  public void GenerateOffsets(int width, int height)
  {
    if (BuildingDef.placementOffsetsCache.TryGetValue(new CellOffset(width, height), out this.PlacementOffsets))
      return;
    int num1 = width / 2 - width + 1;
    this.PlacementOffsets = new CellOffset[width * height];
    for (int index1 = 0; index1 != height; ++index1)
    {
      int num2 = index1 * width;
      for (int index2 = 0; index2 != width; ++index2)
      {
        int index3 = num2 + index2;
        this.PlacementOffsets[index3].x = index2 + num1;
        this.PlacementOffsets[index3].y = index1;
      }
    }
    BuildingDef.placementOffsetsCache.Add(new CellOffset(width, height), this.PlacementOffsets);
  }

  public void PostProcess()
  {
    this.CraftRecipe = new Recipe(this.BuildingComplete.PrefabID().Name, nameOverride: this.Name);
    this.CraftRecipe.Icon = this.UISprite;
    for (int index = 0; index < this.MaterialCategory.Length; ++index)
    {
      TagManager.Create(this.MaterialCategory[index], TUNING.MATERIALS.GetMaterialString(this.MaterialCategory[index]));
      this.CraftRecipe.Ingredients.Add(new Recipe.Ingredient(this.MaterialCategory[index], (float) (int) this.Mass[index]));
    }
    if ((UnityEngine.Object) this.DecorBlockTileInfo != (UnityEngine.Object) null)
      this.DecorBlockTileInfo.PostProcess();
    if ((UnityEngine.Object) this.DecorPlaceBlockTileInfo != (UnityEngine.Object) null)
      this.DecorPlaceBlockTileInfo.PostProcess();
    if (this.Deprecated)
      return;
    Db.Get().TechItems.AddTechItem(this.PrefabID, this.Name, this.Effect, new Func<string, bool, Sprite>(this.GetUISprite), this.RequiredDlcIds, this.ForbiddenDlcIds, this.POIUnlockable)?.AddSearchTerms(this.searchTerms);
  }

  public bool MaterialsAvailable(IList<Tag> selected_elements, WorldContainer world)
  {
    bool flag = true;
    foreach (Recipe.Ingredient allIngredient in this.CraftRecipe.GetAllIngredients(selected_elements))
    {
      if ((double) world.worldInventory.GetAmount(allIngredient.tag, true) < (double) allIngredient.amount)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public bool CheckRequiresBuildingCellVisualizer()
  {
    return this.CheckRequiresPowerInput() || this.CheckRequiresPowerOutput() || this.CheckRequiresGasInput() || this.CheckRequiresGasOutput() || this.CheckRequiresLiquidInput() || this.CheckRequiresLiquidOutput() || this.CheckRequiresSolidInput() || this.CheckRequiresSolidOutput() || this.CheckRequiresHighEnergyParticleInput() || this.CheckRequiresHighEnergyParticleOutput() || (double) this.SelfHeatKilowattsWhenActive != 0.0 || (double) this.ExhaustKilowattsWhenActive != 0.0 || this.DiseaseCellVisName != null;
  }

  public bool CheckRequiresPowerInput() => this.RequiresPowerInput;

  public bool CheckRequiresPowerOutput() => this.RequiresPowerOutput;

  public bool CheckRequiresGasInput() => this.InputConduitType == ConduitType.Gas;

  public bool CheckRequiresGasOutput() => this.OutputConduitType == ConduitType.Gas;

  public bool CheckRequiresLiquidInput() => this.InputConduitType == ConduitType.Liquid;

  public bool CheckRequiresLiquidOutput() => this.OutputConduitType == ConduitType.Liquid;

  public bool CheckRequiresSolidInput() => this.InputConduitType == ConduitType.Solid;

  public bool CheckRequiresSolidOutput() => this.OutputConduitType == ConduitType.Solid;

  public bool CheckRequiresHighEnergyParticleInput() => this.UseHighEnergyParticleInputPort;

  public bool CheckRequiresHighEnergyParticleOutput() => this.UseHighEnergyParticleOutputPort;

  public void AddFacade(string db_facade_id)
  {
    if (this.AvailableFacades == null)
      this.AvailableFacades = new List<string>();
    if (this.AvailableFacades.Contains(db_facade_id))
      return;
    this.AvailableFacades.Add(db_facade_id);
  }

  [Obsolete]
  public bool IsValidDLC() => Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) this);

  public void AddSearchTerms(string newSearchTerms)
  {
    SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
  }

  public static void CollectFabricationRecipes(Tag fabricatorId, List<ComplexRecipe> recipes)
  {
    foreach (ComplexRecipe recipe in ComplexRecipeManager.Get().recipes)
    {
      if (recipe.fabricators.Contains(fabricatorId))
        recipes.Add(recipe);
    }
  }

  public string[] GetRequiredDlcIds() => this.RequiredDlcIds;

  public string[] GetForbiddenDlcIds() => this.ForbiddenDlcIds;
}
