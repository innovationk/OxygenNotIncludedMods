// Decompiled with JetBrains decompiler
// Type: DevToolSimDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
public class DevToolSimDebug : DevTool
{
  private Vector3 worldPos = Vector3.zero;
  private string[] elementNames;
  private Dictionary<SimHashes, double> elementCounts = new Dictionary<SimHashes, double>();
  public static DevToolSimDebug Instance;
  private const string INVALID_OVERLAY_MODE_STR = "None";
  private bool shouldDrawBoundingBox = true;
  private Option<DevToolEntityTarget.ForSimCell> boundBoxSimCellTarget;
  private int xBound = 8;
  private int yBound = 8;
  private bool showElementData;
  private bool showMouseData = true;
  private bool showAccessRestrictions;
  private bool showGridContents;
  private bool showScenePartitionerContents;
  private bool showLayerToggles;
  private bool showCavityInfo;
  private bool showPropertyInfo;
  private bool showBuildings;
  private bool showCreatures;
  private bool showPhysicsData;
  private bool showGasConduitData;
  private bool showLiquidConduitData;
  private string[] overlayModes;
  private int selectedOverlayMode;
  private string[] gameGridModes;
  private Dictionary<string, HashedString> modeLookup;
  private Dictionary<HashedString, string> revModeLookup;
  private HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();

  public DevToolSimDebug()
  {
    this.elementNames = Enum.GetNames(typeof (SimHashes));
    Array.Sort<string>(this.elementNames);
    DevToolSimDebug.Instance = this;
    List<string> stringList = new List<string>();
    this.modeLookup = new Dictionary<string, HashedString>();
    this.revModeLookup = new Dictionary<HashedString, string>();
    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      foreach (System.Type type in assembly.GetTypes())
      {
        if (typeof (OverlayModes.Mode).IsAssignableFrom(type))
        {
          FieldInfo field = type.GetField("ID");
          if (field != (FieldInfo) null)
          {
            object obj = field.GetValue((object) null);
            if (obj != null)
            {
              HashedString key = (HashedString) obj;
              stringList.Add(type.Name);
              this.modeLookup[type.Name] = key;
              this.revModeLookup[key] = type.Name;
            }
          }
        }
      }
    }
    foreach (FieldInfo field in typeof (SimDebugView.OverlayModes).GetFields())
    {
      if (field.FieldType == typeof (HashedString))
      {
        object obj = field.GetValue((object) null);
        if (obj != null)
        {
          HashedString key = (HashedString) obj;
          stringList.Add(field.Name);
          this.modeLookup[field.Name] = key;
          this.revModeLookup[key] = field.Name;
        }
      }
    }
    stringList.Sort();
    stringList.Insert(0, "None");
    this.modeLookup["None"] = (HashedString) "None";
    this.revModeLookup[(HashedString) "None"] = "None";
    stringList.RemoveAll((Predicate<string>) (s => s == null));
    this.overlayModes = stringList.ToArray();
    this.gameGridModes = Enum.GetNames(typeof (SimDebugView.GameGridMode));
  }

  protected override void RenderTo(DevPanel panel)
  {
    if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
      return;
    HashedString hashedString1 = SimDebugView.Instance.GetMode();
    HashedString hashedString2 = hashedString1;
    if (this.overlayModes != null)
    {
      this.selectedOverlayMode = Array.IndexOf<string>(this.overlayModes, this.revModeLookup[hashedString1]);
      this.selectedOverlayMode = this.selectedOverlayMode == -1 ? 0 : this.selectedOverlayMode;
      ImGui.Combo("Debug Mode", ref this.selectedOverlayMode, this.overlayModes, this.overlayModes.Length);
      hashedString1 = this.modeLookup[this.overlayModes[this.selectedOverlayMode]];
      if (hashedString1 == (HashedString) "None")
        hashedString1 = OverlayModes.None.ID;
    }
    if (hashedString1 != hashedString2)
      SimDebugView.Instance.SetMode(hashedString1);
    if (hashedString1 == OverlayModes.Temperature.ID)
    {
      ImGui.InputFloat("Min Expected Temp:", ref SimDebugView.Instance.minTempExpected);
      ImGui.InputFloat("Max Expected Temp:", ref SimDebugView.Instance.maxTempExpected);
    }
    else if (hashedString1 == SimDebugView.OverlayModes.Mass)
    {
      ImGui.InputFloat("Min Expected Mass:", ref SimDebugView.Instance.minMassExpected);
      ImGui.InputFloat("Max Expected Mass:", ref SimDebugView.Instance.maxMassExpected);
    }
    else if (hashedString1 == SimDebugView.OverlayModes.Pressure)
    {
      ImGui.InputFloat("Min Expected Pressure:", ref SimDebugView.Instance.minPressureExpected);
      ImGui.InputFloat("Max Expected Pressure:", ref SimDebugView.Instance.maxPressureExpected);
    }
    else if (hashedString1 == SimDebugView.OverlayModes.GameGrid)
    {
      int gameGridMode = (int) SimDebugView.Instance.GetGameGridMode();
      ImGui.Combo("Grid Mode", ref gameGridMode, this.gameGridModes, this.gameGridModes.Length);
      SimDebugView.Instance.SetGameGridMode((SimDebugView.GameGridMode) gameGridMode);
    }
    int x;
    int y;
    Grid.PosToXY(this.worldPos, out x, out y);
    int v1 = y * Grid.WidthInCells + x;
    ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
    if (ImGui.CollapsingHeader("Overlay Box") && this.shouldDrawBoundingBox)
    {
      if (ImGui.Button("Pick cell"))
        panel.PushDevTool((DevTool) new DevToolEntity_EyeDrop((Action<DevToolEntityTarget>) (target => this.boundBoxSimCellTarget = (Option<DevToolEntityTarget.ForSimCell>) (DevToolEntityTarget.ForSimCell) target), (Func<DevToolEntityTarget, Option<string>>) (uncastTarget => !(uncastTarget is DevToolEntityTarget.ForSimCell) ? (Option<string>) "Target is not a sim cell" : (Option<string>) Option.None)));
      this.DrawBoundingBoxOverlay();
    }
    this.showMouseData = ImGui.CollapsingHeader("Mouse Data");
    if (this.showMouseData)
    {
      ImGui.Indent();
      ImGui.Text("WorldPos: " + this.worldPos.ToString());
      ImGui.Unindent();
    }
    if (v1 < 0 || Grid.CellCount <= v1)
      return;
    bool flag1;
    byte property1;
    int num1;
    if (this.showMouseData)
    {
      ImGui.Indent();
      ImGui.Text($"CellPos: {x.ToString()}, {y.ToString()}");
      int v2 = (y + 1) * (Grid.WidthInCells + 2) + (x + 1);
      if (ImGui.InputInt("Sim Cell:", ref v2))
      {
        x = Mathf.Max(0, v2 % (Grid.WidthInCells + 2) - 1);
        y = Mathf.Max(0, v2 / (Grid.WidthInCells + 2) - 1);
        this.worldPos = Grid.CellToPosCCC(Grid.XYToCell(x, y), Grid.SceneLayer.Front);
      }
      if (ImGui.InputInt("Game Cell:", ref v1))
      {
        x = v1 % Grid.WidthInCells;
        y = v1 / Grid.WidthInCells;
        this.worldPos = Grid.CellToPosCCC(Grid.XYToCell(x, y), Grid.SceneLayer.Front);
      }
      int num2 = Grid.WidthInCells / 32 /*0x20*/;
      int num3 = x / 32 /*0x20*/;
      int num4 = y / 32 /*0x20*/;
      int num5 = num4 * num2 + num3;
      ImGui.Text($"Chunk Idx ({num3}, {num4}): {num5}");
      bool flag2 = Grid.RenderedByWorld[v1];
      ImGui.Text("RenderedByWorld: " + flag2.ToString());
      flag2 = Grid.Solid[v1];
      ImGui.Text("Solid: " + flag2.ToString());
      ImGui.Text("Damage: " + Grid.Damage[v1].ToString());
      ImGui.Text("Foundation: " + Grid.Foundation[v1].ToString());
      ImGui.Text("Revealed: " + Grid.Revealed[v1].ToString());
      ImGui.Text("Visible: " + Grid.Visible[v1].ToString());
      ImGui.Text("DupePassable: " + Grid.DupePassable[v1].ToString());
      ImGui.Text("DupeImpassable: " + Grid.DupeImpassable[v1].ToString());
      ImGui.Text("CritterImpassable: " + Grid.CritterImpassable[v1].ToString());
      flag1 = Grid.FakeFloor[v1];
      ImGui.Text("FakeFloor: " + flag1.ToString());
      flag1 = Grid.HasDoor[v1];
      ImGui.Text("HasDoor: " + flag1.ToString());
      flag1 = Grid.HasLadder[v1];
      ImGui.Text("HasLadder: " + flag1.ToString());
      flag1 = Grid.HasPole[v1];
      ImGui.Text("HasPole: " + flag1.ToString());
      ImGui.Text("GravitasFacility: " + Grid.GravitasFacility[v1].ToString());
      flag1 = Grid.HasNavTeleporter[v1];
      ImGui.Text("HasNavTeleporter: " + flag1.ToString());
      flag1 = Grid.IsTileUnderConstruction[v1];
      ImGui.Text("IsTileUnderConstruction: " + flag1.ToString());
      UtilityConnections connections = Game.Instance.liquidConduitSystem.GetConnections(v1, false);
      ImGui.Text("LiquidVisPlacers: " + connections.ToString());
      connections = Game.Instance.liquidConduitSystem.GetConnections(v1, true);
      ImGui.Text("LiquidPhysPlacers: " + connections.ToString());
      connections = Game.Instance.gasConduitSystem.GetConnections(v1, false);
      ImGui.Text("GasVisPlacers: " + connections.ToString());
      connections = Game.Instance.gasConduitSystem.GetConnections(v1, true);
      ImGui.Text("GasPhysPlacers: " + connections.ToString());
      connections = Game.Instance.electricalConduitSystem.GetConnections(v1, false);
      ImGui.Text("ElecVisPlacers: " + connections.ToString());
      connections = Game.Instance.electricalConduitSystem.GetConnections(v1, true);
      ImGui.Text("ElecPhysPlacers: " + connections.ToString());
      ImGui.Text("World Idx: " + Grid.WorldIdx[v1].ToString());
      ImGui.Text("ZoneType: " + World.Instance.zoneRenderData.GetSubWorldZoneType(v1).ToString());
      ImGui.Text("Light Intensity: " + Grid.LightIntensity[v1].ToString());
      property1 = Grid.ExposedToSunlight[v1];
      ImGui.Text("Sunlight: " + property1.ToString());
      ImGui.Text("Radiation: " + Grid.Radiation[v1].ToString());
      this.showAccessRestrictions = ImGui.CollapsingHeader("Access Restrictions");
      if (this.showAccessRestrictions)
      {
        ImGui.Indent();
        Grid.Restriction restriction;
        if (!Grid.DEBUG_GetRestrictions(v1, out restriction))
        {
          ImGui.Text("No access control.");
        }
        else
        {
          ImGui.Text("Orientation: " + restriction.orientation.ToString());
          ImGui.Text("Default Minion: " + restriction.DirectionMasksForMinionInstanceID[GridRestrictionSerializer.Instance.GetTagId(GameTags.Minions.Models.Standard)].ToString());
          ImGui.Text("Default Bionic: " + restriction.DirectionMasksForMinionInstanceID[GridRestrictionSerializer.Instance.GetTagId(GameTags.Minions.Models.Bionic)].ToString());
          ImGui.Text("Default Robot: " + restriction.DirectionMasksForMinionInstanceID[GridRestrictionSerializer.Instance.GetTagId(GameTags.Robot)].ToString());
          ImGui.Indent();
          foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
          {
            int instanceId = minionIdentity.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<KPrefabID>().InstanceID;
            Grid.Restriction.Directions directions;
            if (restriction.DirectionMasksForMinionInstanceID.TryGetValue(instanceId, out directions))
              ImGui.Text($"{minionIdentity.name} Restriction: {directions.ToString()}");
            else
              ImGui.Text(minionIdentity.name + ": Has No restriction");
          }
          foreach (Tag validRobotType in GridRestrictionSerializer.Instance.ValidRobotTypes)
          {
            int tagId = GridRestrictionSerializer.Instance.GetTagId(validRobotType);
            Grid.Restriction.Directions directions;
            if (restriction.DirectionMasksForMinionInstanceID.TryGetValue(tagId, out directions))
              ImGui.Text($"{validRobotType.ProperName()} Restriction: {directions.ToString()}");
            else
              ImGui.Text(validRobotType.ProperName() + ": Has No restriction");
          }
          ImGui.Unindent();
        }
        ImGui.Unindent();
      }
      this.showGridContents = ImGui.CollapsingHeader("Grid Objects");
      if (this.showGridContents)
      {
        ImGui.Indent();
        for (int layer = 0; layer < 45; ++layer)
        {
          GameObject gameObject = Grid.Objects[v1, layer];
          ImGui.Text($"{Enum.GetName(typeof (ObjectLayer), (object) layer)}: {((UnityEngine.Object) gameObject != (UnityEngine.Object) null ? gameObject.name : "None")}");
        }
        ImGui.Unindent();
      }
      this.showScenePartitionerContents = ImGui.CollapsingHeader("Scene Partitioner");
      if (this.showScenePartitionerContents)
      {
        ImGui.Indent();
        if ((UnityEngine.Object) GameScenePartitioner.Instance != (UnityEngine.Object) null)
        {
          this.showLayerToggles = ImGui.CollapsingHeader("Layers");
          if (this.showLayerToggles)
          {
            bool flag3 = false;
            foreach (ScenePartitionerLayer layer in GameScenePartitioner.Instance.GetLayers())
            {
              bool flag4 = this.toggledLayers.Contains(layer);
              bool v3 = flag4;
              ImGui.Checkbox(HashCache.Get().Get(layer.name), ref v3);
              if (v3 != flag4)
              {
                flag3 = true;
                if (v3)
                  this.toggledLayers.Add(layer);
                else
                  this.toggledLayers.Remove(layer);
              }
            }
            if (flag3)
            {
              GameScenePartitioner.Instance.SetToggledLayers(this.toggledLayers);
              if (this.toggledLayers.Count > 0)
                SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.ScenePartitioner);
            }
          }
          ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
          foreach (ScenePartitionerLayer layer in GameScenePartitioner.Instance.GetLayers())
          {
            gathered_entries.Clear();
            GameScenePartitioner.Instance.GatherEntries(x, y, 1, 1, layer, (List<ScenePartitionerEntry>) gathered_entries);
            foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
            {
              GameObject gameObject = partitionerEntry.obj as GameObject;
              MonoBehaviour monoBehaviour = partitionerEntry.obj as MonoBehaviour;
              if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
                ImGui.Text(gameObject.name);
              else if ((UnityEngine.Object) monoBehaviour != (UnityEngine.Object) null)
                ImGui.Text(monoBehaviour.name);
            }
          }
          gathered_entries.Recycle();
        }
        ImGui.Unindent();
      }
      this.showCavityInfo = ImGui.CollapsingHeader("Cavity Info");
      if (this.showCavityInfo)
      {
        ImGui.Indent();
        CavityInfo cavityInfo = (CavityInfo) null;
        if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null && Game.Instance.roomProber != null)
          cavityInfo = Game.Instance.roomProber.GetCavityForCell(v1);
        if (cavityInfo != null)
        {
          num1 = cavityInfo.NumCells;
          ImGui.Text("Cell Count: " + num1.ToString());
          Room room = cavityInfo.room;
          if (room != null)
          {
            ImGui.Text("Is Room: True");
            num1 = room.buildings.Count;
            this.showBuildings = ImGui.CollapsingHeader($"Buildings ({num1.ToString()})");
            if (this.showBuildings)
            {
              foreach (object building in room.buildings)
                ImGui.Text(building.ToString());
            }
            num1 = room.cavity.creatures.Count;
            this.showCreatures = ImGui.CollapsingHeader($"Creatures ({num1.ToString()})");
            if (this.showCreatures)
            {
              foreach (object creature in room.cavity.creatures)
                ImGui.Text(creature.ToString());
            }
          }
          else
            ImGui.Text("Is Room: False");
        }
        else
          ImGui.Text("No Cavity Detected");
        ImGui.Unindent();
      }
      this.showPropertyInfo = ImGui.CollapsingHeader("Property Info");
      if (this.showPropertyInfo)
      {
        ImGui.Indent();
        bool flag5 = true;
        byte property2 = Grid.Properties[v1];
        foreach (object obj in Enum.GetValues(typeof (Sim.Cell.Properties)))
        {
          if (((int) property2 & (int) obj) != 0)
          {
            ImGui.Text(obj.ToString());
            flag5 = false;
          }
        }
        if (flag5)
          ImGui.Text("No properties");
        ImGui.Unindent();
      }
      ImGui.Unindent();
    }
    float num6;
    if (Grid.ObjectLayers != null)
    {
      Element element = Grid.Element[v1];
      this.showElementData = ImGui.CollapsingHeader("Element");
      ImGui.SameLine();
      ImGui.Text($"[{element.name}]");
      ImGui.Indent();
      num6 = Grid.Mass[v1];
      ImGui.Text("Mass:" + num6.ToString());
      if (this.showElementData)
        this.DrawElem(element);
      num6 = Grid.AccumulatedFlow[v1] / 3f;
      ImGui.Text("Average Flow Rate (kg/s):" + num6.ToString());
      ImGui.Unindent();
    }
    this.showPhysicsData = ImGui.CollapsingHeader("Physics Data");
    if (this.showPhysicsData)
    {
      ImGui.Indent();
      flag1 = Grid.Solid[v1];
      ImGui.Text("Solid: " + flag1.ToString());
      num6 = Grid.Pressure[v1];
      ImGui.Text("Pressure: " + num6.ToString());
      num6 = Grid.Temperature[v1];
      ImGui.Text("Temperature (kelvin -272.15): " + num6.ToString());
      num6 = Grid.Radiation[v1];
      ImGui.Text("Radiation: " + num6.ToString());
      num6 = Grid.Mass[v1];
      ImGui.Text("Mass: " + num6.ToString());
      num6 = (float) Grid.Insulation[v1] / (float) byte.MaxValue;
      ImGui.Text("Insulation: " + num6.ToString());
      property1 = Grid.StrengthInfo[v1];
      ImGui.Text("Strength Multiplier: " + property1.ToString());
      property1 = Grid.Properties[v1];
      ImGui.Text("Properties: 0x: " + property1.ToString("X"));
      ImGui.Text("Disease: " + (Grid.DiseaseIdx[v1] == byte.MaxValue ? "None" : Db.Get().Diseases[(int) Grid.DiseaseIdx[v1]].Name));
      num1 = Grid.DiseaseCount[v1];
      ImGui.Text("Disease Count: " + num1.ToString());
      ImGui.Unindent();
    }
    this.showGasConduitData = ImGui.CollapsingHeader("Gas Conduit Data");
    if (this.showGasConduitData)
      this.DrawConduitFlow(Game.Instance.gasConduitFlow, v1);
    this.showLiquidConduitData = ImGui.CollapsingHeader("Liquid Conduit Data");
    if (!this.showLiquidConduitData)
      return;
    this.DrawConduitFlow(Game.Instance.liquidConduitFlow, v1);
  }

  private void DrawElem(Element element)
  {
    ImGui.Indent();
    ImGui.Text("State: " + element.state.ToString());
    ImGui.Text("Thermal Conductivity: " + element.thermalConductivity.ToString());
    ImGui.Text("Specific Heat Capacity: " + element.specificHeatCapacity.ToString());
    if (element.lowTempTransition != null)
    {
      ImGui.Text("Low Temperature: " + element.lowTemp.ToString());
      ImGui.Text("Low Temperature Transition: " + element.lowTempTransitionTarget.ToString());
    }
    if (element.highTempTransition != null)
    {
      ImGui.Text("High Temperature: " + element.highTemp.ToString());
      ImGui.Text("HighTemp Temperature Transition: " + element.highTempTransitionTarget.ToString());
      if (element.highTempTransitionOreID != (SimHashes) 0)
        ImGui.Text("HighTemp Temperature Transition: " + element.highTempTransitionOreID.ToString());
    }
    ImGui.Text("Light Absorption Factor: " + element.lightAbsorptionFactor.ToString());
    ImGui.Text("Radiation Absorption Factor: " + element.radiationAbsorptionFactor.ToString());
    ImGui.Text("Radiation Per 1000 Mass: " + element.radiationPer1000Mass.ToString());
    ImGui.Text("Sublimate ID: " + element.sublimateId.ToString());
    ImGui.Text("Sublimate FX: " + element.sublimateFX.ToString());
    ImGui.Text("Sublimate Rate: " + element.sublimateRate.ToString());
    ImGui.Text("Sublimate Efficiency: " + element.sublimateEfficiency.ToString());
    ImGui.Text("Sublimate Probability: " + element.sublimateProbability.ToString());
    ImGui.Text("Off Gas Percentage: " + element.offGasPercentage.ToString());
    if (element.IsGas)
      ImGui.Text("Default Pressure: " + element.defaultValues.pressure.ToString());
    else
      ImGui.Text("Default Mass: " + element.defaultValues.mass.ToString());
    ImGui.Text("Default Temperature: " + element.defaultValues.temperature.ToString());
    if (element.IsGas)
      ImGui.Text("Flow: " + element.flow.ToString());
    if (element.IsLiquid)
    {
      ImGui.Text("Max Comp: " + element.maxCompression.ToString());
      ImGui.Text("Max Mass: " + element.maxMass.ToString());
    }
    if (element.IsSolid)
    {
      ImGui.Text("Hardness: " + element.hardness.ToString());
      ImGui.Text("Unstable: " + element.IsUnstable.ToString());
    }
    ImGui.Unindent();
  }

  private void DrawConduitFlow(ConduitFlow flow_mgr, int cell)
  {
    ImGui.Indent();
    ConduitFlow.ConduitContents contents = flow_mgr.GetContents(cell);
    ImGui.Text("Element: " + contents.element.ToString());
    ImGui.Text($"Mass: {contents.mass}");
    ImGui.Text($"Movable Mass: {contents.movable_mass}");
    ImGui.Text("Temperature: " + contents.temperature.ToString());
    ImGui.Text("Disease: " + (contents.diseaseIdx == byte.MaxValue ? "None" : Db.Get().Diseases[(int) contents.diseaseIdx].Name));
    ImGui.Text("Disease Count: " + contents.diseaseCount.ToString());
    ImGui.Text($"Update Order: {flow_mgr.ComputeUpdateOrder(cell)}");
    flow_mgr.SetContents(cell, contents);
    ConduitFlow.FlowDirections permittedFlow = flow_mgr.GetPermittedFlow(cell);
    if (permittedFlow == ConduitFlow.FlowDirections.None)
    {
      ImGui.Text("PermittedFlow: None");
    }
    else
    {
      string str = "";
      if ((permittedFlow & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
        str += " Up ";
      if ((permittedFlow & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
        str += " Down ";
      if ((permittedFlow & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
        str += " Left ";
      if ((permittedFlow & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
        str += " Right ";
      ImGui.Text("PermittedFlow: " + str);
    }
    ImGui.Unindent();
  }

  private void DrawBoundingBoxOverlay()
  {
    ImGui.InputInt("Width:", ref this.xBound, 2);
    ImGui.InputInt("Height:", ref this.yBound, 2);
    Vector2I vector2I1 = this.boundBoxSimCellTarget.HasValue ? Grid.CellToXY(this.boundBoxSimCellTarget.Unwrap().cellIndex) : Grid.PosToXY(this.worldPos);
    Vector2I vector2I2 = new Vector2I(Math.Max(0, vector2I1.x - this.xBound / 2), Math.Max(0, vector2I1.y - this.yBound / 2));
    Vector2I vector2I3 = new Vector2I(Math.Min(vector2I1.x + this.xBound / 2, Grid.WidthInCells), Math.Min(vector2I1.y + this.yBound / 2, Grid.HeightInCells));
    Option<(Vector2, Vector2)> screenRect1 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(vector2I2.X, vector2I2.Y)).GetScreenRect();
    Option<(Vector2, Vector2)> screenRect2 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(vector2I3.X, vector2I3.Y)).GetScreenRect();
    if (!screenRect1.IsSome() || !screenRect2.IsSome())
      return;
    for (int y = vector2I2.Y; y <= vector2I3.Y; ++y)
    {
      for (int x = vector2I2.X; x <= vector2I3.X; ++x)
      {
        Option<(Vector2 cornerA, Vector2 cornerB)> screenRect3 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(x, y)).GetScreenRect();
        Option<(Vector2 cornerA, Vector2 cornerB)> screenRect4 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(x, y)).GetScreenRect();
        DevToolEntity.DrawScreenRect((screenRect3.Unwrap().cornerA, screenRect4.Unwrap().cornerB), (Option<string>) Grid.XYToCell(x, y).ToString(), (Option<Color>) new Color(1f, 1f, 1f, 0.7f), (Option<Color>) new Color(1f, 1f, 1f, 0.2f), new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment.Center));
      }
    }
  }

  public void SetCell(int cell) => this.worldPos = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
}
