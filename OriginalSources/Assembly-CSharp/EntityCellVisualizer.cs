// Decompiled with JetBrains decompiler
// Type: EntityCellVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class EntityCellVisualizer : KMonoBehaviour
{
  protected List<EntityCellVisualizer.PortEntry> ports = new List<EntityCellVisualizer.PortEntry>();
  public EntityCellVisualizer.Ports addedPorts;
  private GameObject switchVisualizer;
  private GameObject wireVisualizerAlpha;
  private GameObject wireVisualizerBeta;
  public const EntityCellVisualizer.Ports HEAT_PORTS = EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink;
  public const EntityCellVisualizer.Ports POWER_PORTS = EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut;
  public const EntityCellVisualizer.Ports GAS_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut;
  public const EntityCellVisualizer.Ports LIQUID_PORTS = EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut;
  public const EntityCellVisualizer.Ports SOLID_PORTS = EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;
  public const EntityCellVisualizer.Ports ENERGY_PARTICLES_PORTS = EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut;
  public const EntityCellVisualizer.Ports DISEASE_PORTS = EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut;
  public const EntityCellVisualizer.Ports MATTER_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut | EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut | EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;
  protected Sprite diseaseSourceSprite;
  protected Color32 diseaseSourceColour;
  [MyCmpGet]
  private Rotatable rotatable;
  protected bool enableRaycast = true;
  protected Dictionary<GameObject, Image> icons;
  public string DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;

  public BuildingCellVisualizerResources Resources => BuildingCellVisualizerResources.Instance();

  protected int CenterCell => Grid.PosToCell((KMonoBehaviour) this);

  protected virtual void DefinePorts()
  {
  }

  protected override void OnPrefabInit()
  {
    this.LoadDiseaseIcon();
    this.DefinePorts();
  }

  public void ConnectedEventWithDelay(
    float delay,
    int connectionCount,
    int cell,
    string soundName)
  {
    this.StartCoroutine(this.ConnectedDelay(delay, connectionCount, cell, soundName));
  }

  private IEnumerator ConnectedDelay(float delay, int connectionCount, int cell, string soundName)
  {
    float startTime = Time.realtimeSinceStartup;
    float currentTime = startTime;
    while ((double) currentTime < (double) startTime + (double) delay)
    {
      currentTime += Time.unscaledDeltaTime;
      yield return (object) SequenceUtil.WaitForEndOfFrame;
    }
    this.ConnectedEvent(cell);
    string sound = GlobalAssets.GetSound(soundName);
    if (sound != null)
    {
      Vector3 position = this.transform.GetPosition() with
      {
        z = 0.0f
      };
      EventInstance instance = SoundEvent.BeginOneShot(sound, position);
      int num = (int) instance.setParameterByName("connectedCount", (float) connectionCount);
      SoundEvent.EndOneShot(instance);
    }
  }

  private int ComputeCell(CellOffset cellOffset)
  {
    CellOffset offset = cellOffset;
    if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
      offset = this.rotatable.GetRotatedCellOffset(cellOffset);
    return Grid.OffsetCell(Grid.PosToCell(this.gameObject), offset);
  }

  public void ConnectedEvent(int cell)
  {
    foreach (EntityCellVisualizer.PortEntry port in this.ports)
    {
      if (this.ComputeCell(port.cellOffset) == cell && (UnityEngine.Object) port.visualizer != (UnityEngine.Object) null)
      {
        SizePulse pulse = port.visualizer.AddComponent<SizePulse>();
        pulse.speed = 20f;
        pulse.multiplier = 0.75f;
        pulse.updateWhenPaused = true;
        pulse.onComplete += (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) pulse));
      }
    }
  }

  public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell)
  {
    this.AddPort(type, cell, Color.white);
  }

  public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color tint)
  {
    this.AddPort(type, cell, tint, tint);
  }

  public virtual void AddPort(
    EntityCellVisualizer.Ports type,
    CellOffset cell,
    Color connectedTint,
    Color disconnectedTint,
    float scale = 1.5f,
    bool hideBG = false)
  {
    this.ports.Add(new EntityCellVisualizer.PortEntry(type, cell, connectedTint, disconnectedTint, scale, hideBG));
    this.addedPorts |= type;
  }

  protected override void OnCleanUp()
  {
    foreach (EntityCellVisualizer.PortEntry port in this.ports)
    {
      if ((UnityEngine.Object) port.visualizer != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) port.visualizer);
    }
    GameObject[] gameObjectArray = new GameObject[3]
    {
      this.switchVisualizer,
      this.wireVisualizerAlpha,
      this.wireVisualizerBeta
    };
    foreach (UnityEngine.Object @object in gameObjectArray)
      UnityEngine.Object.Destroy(@object);
    base.OnCleanUp();
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (this.icons == null)
      this.icons = new Dictionary<GameObject, Image>();
    Components.EntityCellVisualizers.Add(this);
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    Components.EntityCellVisualizers.Remove(this);
  }

  public void DrawIcons(HashedString mode)
  {
    EntityCellVisualizer.Ports ports = (EntityCellVisualizer.Ports) 0;
    if (this.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
      ports = (EntityCellVisualizer.Ports) 0;
    else if (mode == OverlayModes.Power.ID)
      ports = EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut;
    else if (mode == OverlayModes.GasConduits.ID)
      ports = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut;
    else if (mode == OverlayModes.LiquidConduits.ID)
      ports = EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut;
    else if (mode == OverlayModes.SolidConveyor.ID)
      ports = EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;
    else if (mode == OverlayModes.Radiation.ID)
      ports = EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut;
    else if (mode == OverlayModes.Disease.ID)
      ports = EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut;
    else if (mode == OverlayModes.Temperature.ID || mode == OverlayModes.HeatFlow.ID)
      ports = EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink;
    bool flag = false;
    foreach (EntityCellVisualizer.PortEntry port in this.ports)
    {
      if ((port.type & ports) == port.type)
      {
        this.DrawUtilityIcon(port);
        flag = true;
      }
      else if ((UnityEngine.Object) port.visualizer != (UnityEngine.Object) null && port.visualizer.activeInHierarchy)
        port.visualizer.SetActive(false);
    }
    if (mode == OverlayModes.Power.ID)
    {
      if (flag)
        return;
      Switch component1 = this.GetComponent<Switch>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        this.DrawUtilityIcon(Grid.PosToCell(this.transform.GetPosition()), this.Resources.switchIcon, ref this.switchVisualizer, (Color) (component1.IsHandlerOn() ? this.Resources.switchColor : this.Resources.switchOffColor), 1f);
      }
      else
      {
        WireUtilityNetworkLink component2 = this.GetComponent<WireUtilityNetworkLink>();
        if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
          return;
        int linked_cell1;
        int linked_cell2;
        component2.GetCells(out linked_cell1, out linked_cell2);
        this.DrawUtilityIcon(linked_cell1, Game.Instance.circuitManager.GetCircuitID(linked_cell1) == ushort.MaxValue ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerAlpha, this.Resources.electricityInputColor, 1f);
        this.DrawUtilityIcon(linked_cell2, Game.Instance.circuitManager.GetCircuitID(linked_cell2) == ushort.MaxValue ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerBeta, this.Resources.electricityInputColor, 1f);
      }
    }
    else
    {
      GameObject[] gameObjectArray = new GameObject[3]
      {
        this.switchVisualizer,
        this.wireVisualizerAlpha,
        this.wireVisualizerBeta
      };
      foreach (GameObject gameObject in gameObjectArray)
      {
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && gameObject.activeInHierarchy)
          gameObject.SetActive(false);
      }
    }
  }

  private Sprite GetSpriteForPortType(EntityCellVisualizer.Ports type, bool connected)
  {
    switch (type)
    {
      case EntityCellVisualizer.Ports.PowerIn:
        return !connected ? this.Resources.electricityInputIcon : this.Resources.electricityBridgeConnectedIcon;
      case EntityCellVisualizer.Ports.PowerOut:
        return !connected ? this.Resources.electricityOutputIcon : this.Resources.electricityBridgeConnectedIcon;
      case EntityCellVisualizer.Ports.GasIn:
        return this.Resources.gasInputIcon;
      case EntityCellVisualizer.Ports.GasOut:
        return this.Resources.gasOutputIcon;
      case EntityCellVisualizer.Ports.LiquidIn:
        return this.Resources.liquidInputIcon;
      case EntityCellVisualizer.Ports.LiquidOut:
        return this.Resources.liquidOutputIcon;
      case EntityCellVisualizer.Ports.SolidIn:
        return this.Resources.liquidInputIcon;
      case EntityCellVisualizer.Ports.SolidOut:
        return this.Resources.liquidOutputIcon;
      case EntityCellVisualizer.Ports.HighEnergyParticleIn:
        return this.Resources.highEnergyParticleInputIcon;
      case EntityCellVisualizer.Ports.HighEnergyParticleOut:
        return this.GetIconForHighEnergyOutput();
      case EntityCellVisualizer.Ports.DiseaseIn:
        return this.diseaseSourceSprite;
      case EntityCellVisualizer.Ports.DiseaseOut:
        return this.diseaseSourceSprite;
      case EntityCellVisualizer.Ports.HeatSource:
        return this.Resources.heatSourceIcon;
      case EntityCellVisualizer.Ports.HeatSink:
        return this.Resources.heatSinkIcon;
      default:
        return (Sprite) null;
    }
  }

  protected virtual void DrawUtilityIcon(EntityCellVisualizer.PortEntry port)
  {
    int cell = this.ComputeCell(port.cellOffset);
    bool flag = true;
    bool connected = true;
    switch (port.type)
    {
      case EntityCellVisualizer.Ports.PowerIn:
      case EntityCellVisualizer.Ports.PowerOut:
        int num = (UnityEngine.Object) (this.GetComponent<Building>() as BuildingPreview) != (UnityEngine.Object) null ? 1 : 0;
        BuildingEnabledButton component = this.GetComponent<BuildingEnabledButton>();
        connected = num == 0 && Game.Instance.circuitManager.GetCircuitID(cell) != ushort.MaxValue;
        flag = num != 0 || (UnityEngine.Object) component == (UnityEngine.Object) null || component.IsEnabled;
        break;
      case EntityCellVisualizer.Ports.GasIn:
      case EntityCellVisualizer.Ports.GasOut:
        flag = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[cell, 12];
        break;
      case EntityCellVisualizer.Ports.LiquidIn:
      case EntityCellVisualizer.Ports.LiquidOut:
        flag = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[cell, 16 /*0x10*/];
        break;
      case EntityCellVisualizer.Ports.SolidIn:
      case EntityCellVisualizer.Ports.SolidOut:
        flag = (UnityEngine.Object) null != (UnityEngine.Object) Grid.Objects[cell, 20];
        break;
    }
    this.DrawUtilityIcon(cell, this.GetSpriteForPortType(port.type, connected), ref port.visualizer, flag ? port.connectedTint : port.disconnectedTint, port.scale, port.hideBG);
  }

  protected virtual void LoadDiseaseIcon()
  {
    DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo((HashedString) this.DiseaseCellVisName);
    if (info.name == null)
      return;
    this.diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
    this.diseaseSourceColour = GlobalAssets.Instance.colorSet.GetColorByName(info.overlayColourName);
  }

  protected virtual Sprite GetIconForHighEnergyOutput()
  {
    IHighEnergyParticleDirection component = this.GetComponent<IHighEnergyParticleDirection>();
    Sprite particleOutputIcon = this.Resources.highEnergyParticleOutputIcons[0];
    if (component != null)
      particleOutputIcon = this.Resources.highEnergyParticleOutputIcons[EightDirectionUtil.GetDirectionIndex(component.Direction)];
    return particleOutputIcon;
  }

  private void DrawUtilityIcon(
    int cell,
    Sprite icon_img,
    ref GameObject visualizerObj,
    Color tint,
    float scaleMultiplier = 1.5f,
    bool hideBG = false)
  {
    Vector3 posCcc = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);
    if ((UnityEngine.Object) visualizerObj == (UnityEngine.Object) null)
    {
      visualizerObj = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas);
      visualizerObj.transform.SetAsFirstSibling();
      this.icons.Add(visualizerObj, visualizerObj.transform.GetChild(0).GetComponent<Image>());
    }
    if (!visualizerObj.gameObject.activeInHierarchy)
      visualizerObj.gameObject.SetActive(true);
    visualizerObj.GetComponent<Image>().enabled = !hideBG;
    this.icons[visualizerObj].raycastTarget = this.enableRaycast;
    this.icons[visualizerObj].sprite = icon_img;
    visualizerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = tint;
    visualizerObj.transform.SetPosition(posCcc);
    if (!((UnityEngine.Object) visualizerObj.GetComponent<SizePulse>() == (UnityEngine.Object) null))
      return;
    visualizerObj.transform.localScale = Vector3.one * scaleMultiplier;
  }

  public Image GetPowerOutputIcon()
  {
    foreach (EntityCellVisualizer.PortEntry port in this.ports)
    {
      if (port.type == EntityCellVisualizer.Ports.PowerOut)
        return (UnityEngine.Object) port.visualizer != (UnityEngine.Object) null ? port.visualizer.transform.GetChild(0).GetComponent<Image>() : (Image) null;
    }
    return (Image) null;
  }

  public Image GetPowerInputIcon()
  {
    foreach (EntityCellVisualizer.PortEntry port in this.ports)
    {
      if (port.type == EntityCellVisualizer.Ports.PowerIn)
        return (UnityEngine.Object) port.visualizer != (UnityEngine.Object) null ? port.visualizer.transform.GetChild(0).GetComponent<Image>() : (Image) null;
    }
    return (Image) null;
  }

  [Flags]
  public enum Ports
  {
    PowerIn = 1,
    PowerOut = 2,
    GasIn = 4,
    GasOut = 8,
    LiquidIn = 16, // 0x00000010
    LiquidOut = 32, // 0x00000020
    SolidIn = 64, // 0x00000040
    SolidOut = 128, // 0x00000080
    HighEnergyParticleIn = 256, // 0x00000100
    HighEnergyParticleOut = 512, // 0x00000200
    DiseaseIn = 1024, // 0x00000400
    DiseaseOut = 2048, // 0x00000800
    HeatSource = 4096, // 0x00001000
    HeatSink = 8192, // 0x00002000
  }

  protected class PortEntry
  {
    public EntityCellVisualizer.Ports type;
    public CellOffset cellOffset;
    public GameObject visualizer;
    public Color connectedTint;
    public Color disconnectedTint;
    public float scale;
    public bool hideBG;

    public PortEntry(
      EntityCellVisualizer.Ports type,
      CellOffset cellOffset,
      Color connectedTint,
      Color disconnectedTint,
      float scale,
      bool hideBG)
    {
      this.type = type;
      this.cellOffset = cellOffset;
      this.visualizer = (GameObject) null;
      this.connectedTint = connectedTint;
      this.disconnectedTint = disconnectedTint;
      this.scale = scale;
      this.hideBG = hideBG;
    }
  }
}
