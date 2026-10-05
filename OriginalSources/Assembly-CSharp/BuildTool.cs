// Decompiled with JetBrains decompiler
// Type: BuildTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using Rendering;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BuildTool : DragTool
{
  [SerializeField]
  private TextStyleSetting tooltipStyle;
  private int lastCell = -1;
  private int lastDragCell = -1;
  private Orientation lastDragOrientation;
  private IList<Tag> selectedElements;
  private BuildingDef def;
  private Orientation buildingOrientation;
  private string facadeID;
  private ToolTip tooltip;
  public static BuildTool Instance;
  private bool active;
  private int buildingCount;

  public static void DestroyInstance() => BuildTool.Instance = (BuildTool) null;

  protected override void OnPrefabInit()
  {
    BuildTool.Instance = this;
    this.tooltip = this.GetComponent<ToolTip>();
    this.buildingCount = UnityEngine.Random.Range(1, 14);
    this.canChangeDragAxis = false;
  }

  protected override void OnActivateTool()
  {
    this.lastDragCell = -1;
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
    {
      this.ClearTilePreview();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.visualizer);
    }
    this.active = true;
    base.OnActivateTool();
    Vector3 world = this.ClampPositionToWorld(PlayerController.GetCursorPos(KInputManager.GetMousePos()), ClusterManager.Instance.activeWorld);
    this.visualizer = GameUtil.KInstantiate(this.def.BuildingPreview, world, Grid.SceneLayer.Ore, gameLayer: LayerMask.NameToLayer("Place"));
    KBatchedAnimController component1 = this.visualizer.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.visibilityType = KAnimControllerBase.VisibilityType.Always;
      component1.isMovable = true;
      component1.Offset = this.def.GetVisualizerOffset();
      component1.name = component1.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
    }
    if (!this.facadeID.IsNullOrWhiteSpace() && this.facadeID != "DEFAULT_FACADE")
      this.visualizer.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeID));
    Rotatable component2 = this.visualizer.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      this.buildingOrientation = this.def.InitialOrientation;
      component2.SetOrientation(this.buildingOrientation);
    }
    this.visualizer.SetActive(true);
    this.UpdateVis(world);
    this.GetComponent<BuildToolHoverTextCard>().currentDef = this.def;
    ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
      this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
    else
      component1.SetLayer(LayerMask.NameToLayer("Place"));
    GridCompositor.Instance.ToggleMajor(true);
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    this.lastDragCell = -1;
    if (!this.active)
      return;
    this.active = false;
    GridCompositor.Instance.ToggleMajor(false);
    this.buildingOrientation = Orientation.Neutral;
    this.HideToolTip();
    ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
    this.ClearTilePreview();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.visualizer);
    if ((UnityEngine.Object) new_tool == (UnityEngine.Object) SelectTool.Instance)
      Game.Instance.Trigger(-1190690038, (object) null);
    base.OnDeactivateTool(new_tool);
  }

  public void Activate(BuildingDef def, IList<Tag> selected_elements)
  {
    this.selectedElements = selected_elements;
    this.def = def;
    this.viewMode = def.ViewMode;
    ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
    this.OnActivateTool();
  }

  public void Activate(BuildingDef def, IList<Tag> selected_elements, string facadeID)
  {
    this.facadeID = facadeID;
    this.Activate(def, selected_elements);
  }

  public void Deactivate()
  {
    this.selectedElements = (IList<Tag>) null;
    SelectTool.Instance.Activate();
    this.def = (BuildingDef) null;
    this.facadeID = (string) null;
    ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
  }

  public int GetLastCell => this.lastCell;

  public Orientation GetBuildingOrientation => this.buildingOrientation;

  private void ClearTilePreview()
  {
    if (!Grid.IsValidBuildingCell(this.lastCell) || !this.def.IsTilePiece)
      return;
    GameObject gameObject1 = Grid.Objects[this.lastCell, (int) this.def.TileLayer];
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) gameObject1)
      Grid.Objects[this.lastCell, (int) this.def.TileLayer] = (GameObject) null;
    if (!this.def.isKAnimTile)
      return;
    GameObject gameObject2 = (GameObject) null;
    if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
      gameObject2 = Grid.Objects[this.lastCell, (int) this.def.ReplacementLayer];
    if (!((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null) && !((UnityEngine.Object) gameObject1.GetComponent<Constructable>() == (UnityEngine.Object) null) || !((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null) && !((UnityEngine.Object) gameObject2 == (UnityEngine.Object) this.visualizer))
      return;
    World.Instance.blockTileRenderer.RemoveBlock(this.def, false, SimHashes.Void, this.lastCell);
    World.Instance.blockTileRenderer.RemoveBlock(this.def, true, SimHashes.Void, this.lastCell);
    TileVisualizer.RefreshCell(this.lastCell, this.def.TileLayer, this.def.ReplacementLayer);
  }

  public override void OnMouseMove(Vector3 cursorPos)
  {
    base.OnMouseMove(cursorPos);
    cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
    this.UpdateVis(cursorPos);
  }

  private void UpdateVis(Vector3 pos)
  {
    bool flag1 = this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out string _);
    bool isReplacement = this.def.IsValidReplaceLocation(pos, this.buildingOrientation, this.def.ReplacementLayer, this.def.ObjectLayer);
    bool flag2 = flag1 | isReplacement;
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null)
    {
      Color c = Color.white;
      float strength = 0.0f;
      if (!flag2)
      {
        c = Color.red;
        strength = 1f;
      }
      this.SetColor(this.visualizer, c, strength);
    }
    int cell = Grid.PosToCell(pos);
    if (!((UnityEngine.Object) this.def != (UnityEngine.Object) null))
      return;
    Vector3 posCbc = Grid.CellToPosCBC(cell, this.def.SceneLayer);
    this.visualizer.transform.SetPosition(posCbc);
    this.transform.SetPosition(posCbc - Vector3.up * 0.5f);
    if (this.def.IsTilePiece)
    {
      this.ClearTilePreview();
      if (Grid.IsValidBuildingCell(cell))
      {
        GameObject gameObject1 = Grid.Objects[cell, (int) this.def.TileLayer];
        if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null)
          Grid.Objects[cell, (int) this.def.TileLayer] = this.visualizer;
        if (this.def.isKAnimTile)
        {
          GameObject gameObject2 = (GameObject) null;
          if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
            gameObject2 = Grid.Objects[cell, (int) this.def.ReplacementLayer];
          if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null || (UnityEngine.Object) gameObject1.GetComponent<Constructable>() == (UnityEngine.Object) null && (UnityEngine.Object) gameObject2 == (UnityEngine.Object) null)
          {
            TileVisualizer.RefreshCell(cell, this.def.TileLayer, this.def.ReplacementLayer);
            if ((UnityEngine.Object) this.def.BlockTileAtlas != (UnityEngine.Object) null)
            {
              int layer = LayerMask.NameToLayer("Overlay");
              BlockTileRenderer blockTileRenderer = World.Instance.blockTileRenderer;
              blockTileRenderer.SetInvalidPlaceCell(cell, !flag2);
              if (this.lastCell != cell)
                blockTileRenderer.SetInvalidPlaceCell(this.lastCell, false);
              blockTileRenderer.AddBlock(layer, this.def, isReplacement, SimHashes.Void, cell, true);
            }
          }
        }
      }
    }
    if (this.lastCell == cell)
      return;
    this.lastCell = cell;
  }

  public PermittedRotations? GetPermittedRotations()
  {
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null)
      return new PermittedRotations?();
    Rotatable component = this.visualizer.GetComponent<Rotatable>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null ? new PermittedRotations?() : new PermittedRotations?(component.permittedRotations);
  }

  public bool CanRotate()
  {
    return !((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null) && !((UnityEngine.Object) this.visualizer.GetComponent<Rotatable>() == (UnityEngine.Object) null);
  }

  public void TryRotate()
  {
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null)
      return;
    Rotatable component = this.visualizer.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Rotate"));
    this.buildingOrientation = component.Rotate();
    if (Grid.IsValidBuildingCell(this.lastCell))
      this.UpdateVis(Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building));
    if (!this.Dragging || this.lastDragCell == -1)
      return;
    this.TryBuild(this.lastDragCell);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.RotateBuilding))
      this.TryRotate();
    else
      base.OnKeyDown(e);
  }

  protected override void OnDragTool(int cell, int distFromOrigin) => this.TryBuild(cell);

  private void TryBuild(int cell)
  {
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null || cell == this.lastDragCell && this.buildingOrientation == this.lastDragOrientation || Grid.PosToCell(this.visualizer) != cell && ((bool) (UnityEngine.Object) this.def.BuildingComplete.GetComponent<LogicPorts>() || (bool) (UnityEngine.Object) this.def.BuildingComplete.GetComponent<LogicGateBase>()))
      return;
    this.lastDragCell = cell;
    this.lastDragOrientation = this.buildingOrientation;
    this.ClearTilePreview();
    Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.Building);
    GameObject builtItem = (GameObject) null;
    PlanScreen.Instance.LastSelectedBuildingFacade = this.facadeID;
    bool instantBuild = DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild;
    if (!instantBuild)
      builtItem = this.def.TryPlace(this.visualizer, posCbc, this.buildingOrientation, this.selectedElements, this.facadeID);
    else if (this.def.IsValidBuildLocation(this.visualizer, posCbc, this.buildingOrientation) && this.def.IsValidPlaceLocation(this.visualizer, posCbc, this.buildingOrientation, out string _))
    {
      if (this.def.ObjectLayer == ObjectLayer.Building)
        this.def.RunOnArea(cell, this.buildingOrientation, (Action<int>) (offset_cell =>
        {
          Uprootable uprootable;
          if (!Uprootable.CanUproot(Grid.Objects[offset_cell, (int) this.def.ObjectLayer], out uprootable))
            return;
          uprootable.CompleteWork((WorkerBase) null);
        }));
      else if (this.def.ObjectLayer == ObjectLayer.Backwall)
        this.def.RunOnArea(cell, this.buildingOrientation, (Action<int>) (offset_cell =>
        {
          if (!BackwallManager.HasBackwall(offset_cell))
            return;
          SimMessages.Dig(offset_cell, skipEvent: true, backwall: true);
        }));
      float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
      builtItem = this.def.Build(cell, this.buildingOrientation, (Storage) null, this.selectedElements, Mathf.Min(this.def.Temperature, b), this.facadeID, false, GameClock.Instance.GetTime());
    }
    if ((UnityEngine.Object) builtItem == (UnityEngine.Object) null && this.def.ReplacementLayer != ObjectLayer.NumLayers)
    {
      GameObject replacementCandidate = this.def.GetReplacementCandidate(cell);
      bool replacementLayerOccupied = false;
      this.def.RunOnArea(cell, this.buildingOrientation, (Action<int>) (offset_cell =>
      {
        if (!this.def.IsReplacementLayerOccupied(offset_cell))
          return;
        replacementLayerOccupied = true;
      }));
      if ((UnityEngine.Object) replacementCandidate != (UnityEngine.Object) null && !replacementLayerOccupied)
      {
        BuildingComplete component = replacementCandidate.GetComponent<BuildingComplete>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.Replaceable && this.def.CanReplace(replacementCandidate))
        {
          Tag tag = replacementCandidate.GetComponent<PrimaryElement>().Element.tag;
          if (tag.GetHash() == 1542131326)
            tag = SimHashes.Snow.CreateTag();
          if ((UnityEngine.Object) component.Def != (UnityEngine.Object) this.def || this.selectedElements[0] != tag)
          {
            if (!instantBuild)
            {
              builtItem = this.def.TryReplaceTile(this.visualizer, posCbc, this.buildingOrientation, this.selectedElements, this.facadeID);
              Grid.Objects[cell, (int) this.def.ReplacementLayer] = builtItem;
            }
            else if (this.def.IsValidBuildLocation(this.visualizer, posCbc, this.buildingOrientation, true) && this.def.IsValidPlaceLocation(this.visualizer, posCbc, this.buildingOrientation, true, out string _))
              builtItem = this.InstantBuildReplace(cell, posCbc, replacementCandidate);
          }
        }
      }
    }
    this.PostProcessBuild(instantBuild, posCbc, builtItem);
  }

  private GameObject InstantBuildReplace(int cell, Vector3 pos, GameObject tile)
  {
    if (this.def.PlacementOffsets.Length > 1)
      this.def.RunOnArea(cell, this.buildingOrientation, (Action<int>) (offset_cell =>
      {
        if (offset_cell == cell)
          return;
        GameObject neighborTile = this.def.GetReplacementCandidate(offset_cell);
        if ((UnityEngine.Object) neighborTile == (UnityEngine.Object) null)
          return;
        SimCellOccupier component = neighborTile.GetComponent<SimCellOccupier>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.DestroySelf((System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) neighborTile)));
        else
          UnityEngine.Object.Destroy((UnityEngine.Object) neighborTile);
      }));
    if ((UnityEngine.Object) tile.GetComponent<SimCellOccupier>() == (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) tile);
      float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
      return this.def.Build(cell, this.buildingOrientation, (Storage) null, this.selectedElements, Mathf.Min(this.def.Temperature, b), this.facadeID, false, GameClock.Instance.GetTime());
    }
    tile.GetComponent<SimCellOccupier>().DestroySelf((System.Action) (() =>
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) tile);
      this.PostProcessBuild(true, pos, this.def.Build(cell, this.buildingOrientation, (Storage) null, this.selectedElements, Mathf.Min(this.def.Temperature, ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f), this.facadeID, false, GameClock.Instance.GetTime()));
    }));
    return (GameObject) null;
  }

  private void PostProcessBuild(bool instantBuild, Vector3 pos, GameObject builtItem)
  {
    if ((UnityEngine.Object) builtItem == (UnityEngine.Object) null)
      return;
    if (!instantBuild)
    {
      Prioritizable component = builtItem.GetComponent<Prioritizable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
          component.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
        if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
          component.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
      }
    }
    if (this.def.MaterialsAvailable(this.selectedElements, ClusterManager.Instance.activeWorld) || DebugHandler.InstantBuildMode)
    {
      this.placeSound = GlobalAssets.GetSound("Place_Building_" + this.def.AudioSize);
      if (this.placeSound != null)
      {
        this.buildingCount = this.buildingCount % 14 + 1;
        EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, pos with
        {
          z = 0.0f
        });
        if (this.def.AudioSize == "small")
        {
          int num = (int) instance.setParameterByName("tileCount", (float) this.buildingCount);
        }
        SoundEvent.EndOneShot(instance);
      }
    }
    else
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, (string) UI.TOOLTIPS.NOMATERIAL, (Transform) null, pos);
    if (!this.def.OnePerWorld)
      return;
    PlayerController.Instance.ActivateTool((InterfaceTool) SelectTool.Instance);
  }

  protected override DragTool.Mode GetMode() => DragTool.Mode.Brush;

  private void SetColor(GameObject root, Color c, float strength)
  {
    KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.TintColour = (Color32) c;
  }

  private void ShowToolTip() => ToolTipScreen.Instance.SetToolTip(this.tooltip);

  private void HideToolTip() => ToolTipScreen.Instance.ClearToolTip(this.tooltip);

  public void Update()
  {
    if (!this.active)
      return;
    KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetLayer(LayerMask.NameToLayer("Place"));
  }

  public override string GetDeactivateSound() => "HUD_Click_Deselect";

  public override void OnLeftClickDown(Vector3 cursor_pos) => base.OnLeftClickDown(cursor_pos);

  public override void OnLeftClickUp(Vector3 cursor_pos) => base.OnLeftClickUp(cursor_pos);

  public void SetToolOrientation(Orientation orientation)
  {
    if (!((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null))
      return;
    Rotatable component = this.visualizer.GetComponent<Rotatable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.buildingOrientation = orientation;
    component.SetOrientation(orientation);
    if (Grid.IsValidBuildingCell(this.lastCell))
      this.UpdateVis(Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building));
    if (!this.Dragging || this.lastDragCell == -1)
      return;
    this.TryBuild(this.lastDragCell);
  }
}
