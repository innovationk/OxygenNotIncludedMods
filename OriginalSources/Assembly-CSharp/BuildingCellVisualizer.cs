// Decompiled with JetBrains decompiler
// Type: BuildingCellVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/BuildingCellVisualizer")]
public class BuildingCellVisualizer : EntityCellVisualizer
{
  [MyCmpReq]
  private Building building;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void LoadDiseaseIcon()
  {
    DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo((HashedString) this.building.Def.DiseaseCellVisName);
    if (info.name == null)
      return;
    this.diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
    this.diseaseSourceColour = GlobalAssets.Instance.colorSet.GetColorByName(info.overlayColourName);
  }

  protected override void DefinePorts()
  {
    BuildingDef def = this.building.Def;
    if (def.CheckRequiresPowerInput())
      this.AddPort(EntityCellVisualizer.Ports.PowerIn, this.building.Def.PowerInputOffset, this.Resources.electricityInputColor, Color.gray, 1f);
    if (def.CheckRequiresPowerOutput())
      this.AddPort(EntityCellVisualizer.Ports.PowerOut, this.building.Def.PowerOutputOffset, this.building.Def.UseWhitePowerOutputConnectorColour ? this.Resources.electricityInputColor : this.Resources.electricityOutputColor, Color.gray, 1f);
    if (def.CheckRequiresGasInput())
      this.AddPort(EntityCellVisualizer.Ports.GasIn, this.building.Def.UtilityInputOffset, (Color) this.Resources.gasIOColours.input.connected, (Color) this.Resources.gasIOColours.input.disconnected);
    if (def.CheckRequiresGasOutput())
      this.AddPort(EntityCellVisualizer.Ports.GasOut, this.building.Def.UtilityOutputOffset, (Color) this.Resources.gasIOColours.output.connected, (Color) this.Resources.gasIOColours.output.disconnected);
    if (def.CheckRequiresLiquidInput())
      this.AddPort(EntityCellVisualizer.Ports.LiquidIn, this.building.Def.UtilityInputOffset, (Color) this.Resources.liquidIOColours.input.connected, (Color) this.Resources.liquidIOColours.input.disconnected);
    if (def.CheckRequiresLiquidOutput())
      this.AddPort(EntityCellVisualizer.Ports.LiquidOut, this.building.Def.UtilityOutputOffset, (Color) this.Resources.liquidIOColours.output.connected, (Color) this.Resources.liquidIOColours.output.disconnected);
    if (def.CheckRequiresSolidInput())
      this.AddPort(EntityCellVisualizer.Ports.SolidIn, this.building.Def.UtilityInputOffset, (Color) this.Resources.liquidIOColours.input.connected, (Color) this.Resources.liquidIOColours.input.disconnected);
    if (def.CheckRequiresSolidOutput())
      this.AddPort(EntityCellVisualizer.Ports.SolidOut, this.building.Def.UtilityOutputOffset, (Color) this.Resources.liquidIOColours.output.connected, (Color) this.Resources.liquidIOColours.output.disconnected);
    if (def.CheckRequiresHighEnergyParticleInput())
      this.AddPort(EntityCellVisualizer.Ports.HighEnergyParticleIn, this.building.Def.HighEnergyParticleInputOffset, this.Resources.highEnergyParticleInputColour, Color.white, 3f);
    if (def.CheckRequiresHighEnergyParticleOutput())
      this.AddPort(EntityCellVisualizer.Ports.HighEnergyParticleOut, this.building.Def.HighEnergyParticleOutputOffset, this.Resources.highEnergyParticleOutputColour, Color.white, 3f);
    if ((double) def.SelfHeatKilowattsWhenActive > 0.0 || (double) def.ExhaustKilowattsWhenActive > 0.0)
      this.AddPort(EntityCellVisualizer.Ports.HeatSource, new CellOffset());
    if ((double) def.SelfHeatKilowattsWhenActive < 0.0 || (double) def.ExhaustKilowattsWhenActive < 0.0)
      this.AddPort(EntityCellVisualizer.Ports.HeatSink, new CellOffset());
    if ((Object) this.diseaseSourceSprite != (Object) null)
      this.AddPort(EntityCellVisualizer.Ports.DiseaseOut, this.building.Def.UtilityOutputOffset, (Color) this.diseaseSourceColour);
    foreach (ISecondaryInput component in def.BuildingComplete.GetComponents<ISecondaryInput>())
    {
      if (component != null)
      {
        if (component.HasSecondaryConduitType(ConduitType.Gas))
        {
          BuildingCellVisualizerResources.ConnectedDisconnectedColours disconnectedColours = def.CheckRequiresGasInput() ? this.Resources.alternateIOColours.input : this.Resources.gasIOColours.input;
          this.AddPort(EntityCellVisualizer.Ports.GasIn, component.GetSecondaryConduitOffset(ConduitType.Gas), (Color) disconnectedColours.connected, (Color) disconnectedColours.disconnected);
        }
        if (component.HasSecondaryConduitType(ConduitType.Liquid))
        {
          if (!def.CheckRequiresLiquidInput())
          {
            BuildingCellVisualizerResources.IOColours liquidIoColours = this.Resources.liquidIOColours;
          }
          else
          {
            BuildingCellVisualizerResources.IOColours alternateIoColours = this.Resources.alternateIOColours;
          }
          this.AddPort(EntityCellVisualizer.Ports.LiquidIn, component.GetSecondaryConduitOffset(ConduitType.Liquid));
        }
        if (component.HasSecondaryConduitType(ConduitType.Solid))
        {
          if (!def.CheckRequiresSolidInput())
          {
            BuildingCellVisualizerResources.IOColours liquidIoColours = this.Resources.liquidIOColours;
          }
          else
          {
            BuildingCellVisualizerResources.IOColours alternateIoColours = this.Resources.alternateIOColours;
          }
          this.AddPort(EntityCellVisualizer.Ports.SolidIn, component.GetSecondaryConduitOffset(ConduitType.Solid));
        }
      }
    }
    foreach (ISecondaryOutput component in def.BuildingComplete.GetComponents<ISecondaryOutput>())
    {
      if (component != null)
      {
        if (component.HasSecondaryConduitType(ConduitType.Gas))
        {
          BuildingCellVisualizerResources.ConnectedDisconnectedColours disconnectedColours = def.CheckRequiresGasOutput() ? this.Resources.alternateIOColours.output : this.Resources.gasIOColours.output;
          this.AddPort(EntityCellVisualizer.Ports.GasOut, component.GetSecondaryConduitOffset(ConduitType.Gas), (Color) disconnectedColours.connected, (Color) disconnectedColours.disconnected);
        }
        if (component.HasSecondaryConduitType(ConduitType.Liquid))
        {
          BuildingCellVisualizerResources.ConnectedDisconnectedColours disconnectedColours = def.CheckRequiresLiquidOutput() ? this.Resources.alternateIOColours.output : this.Resources.liquidIOColours.output;
          this.AddPort(EntityCellVisualizer.Ports.LiquidOut, component.GetSecondaryConduitOffset(ConduitType.Liquid), (Color) disconnectedColours.connected, (Color) disconnectedColours.disconnected);
        }
        if (component.HasSecondaryConduitType(ConduitType.Solid))
        {
          BuildingCellVisualizerResources.ConnectedDisconnectedColours disconnectedColours = def.CheckRequiresSolidOutput() ? this.Resources.alternateIOColours.output : this.Resources.liquidIOColours.output;
          this.AddPort(EntityCellVisualizer.Ports.SolidOut, component.GetSecondaryConduitOffset(ConduitType.Solid), (Color) disconnectedColours.connected, (Color) disconnectedColours.disconnected);
        }
      }
    }
  }

  protected override void OnCmpEnable()
  {
    this.enableRaycast = (Object) (this.building as BuildingComplete) != (Object) null;
    base.OnCmpEnable();
  }
}
