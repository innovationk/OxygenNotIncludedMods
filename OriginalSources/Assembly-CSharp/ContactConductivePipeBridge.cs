// Decompiled with JetBrains decompiler
// Type: ContactConductivePipeBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

#nullable disable
public class ContactConductivePipeBridge : 
  GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>
{
  private const string loopAnimName = "on";
  private const string loopAnim_noWater = "off";
  private GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.State withLiquid;
  private GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.State noLiquid;
  private StateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.FloatParameter noLiquidTimer;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.noLiquid;
    this.noLiquid.PlayAnim("off", KAnim.PlayMode.Once).ParamTransition<float>((StateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.Parameter<float>) this.noLiquidTimer, this.withLiquid, GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.IsGTZero);
    this.withLiquid.Update(new System.Action<ContactConductivePipeBridge.Instance, float>(ContactConductivePipeBridge.ExpirationTimerUpdate)).PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<float>((StateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.Parameter<float>) this.noLiquidTimer, this.noLiquid, GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.IsLTEZero);
  }

  private static void ExpirationTimerUpdate(ContactConductivePipeBridge.Instance smi, float dt)
  {
    float num1 = smi.sm.noLiquidTimer.Get(smi) - dt;
    double num2 = (double) smi.sm.noLiquidTimer.Set(num1, smi);
  }

  private static float CalculateMaxWattsTransfered(
    float buildingTemperature,
    float building_thermal_conductivity,
    float content_temperature,
    float content_thermal_conductivity)
  {
    float num1 = 1f;
    float num2 = 1f;
    float num3 = 50f;
    return (float) (((double) content_temperature - (double) buildingTemperature) * (((double) content_thermal_conductivity + (double) building_thermal_conductivity) * 0.5)) * num1 * num3 / num2;
  }

  private static float GetKilloJoulesTransfered(
    float maxWattsTransfered,
    float dt,
    float building_Temperature,
    float building_heat_capacity,
    float content_temperature,
    float content_heat_capacity)
  {
    float num1 = (float) ((double) maxWattsTransfered * (double) dt / 1000.0);
    float min1 = Mathf.Min(content_temperature, building_Temperature);
    float max1 = Mathf.Max(content_temperature, building_Temperature);
    double num2 = (double) content_temperature - (double) num1 / (double) content_heat_capacity;
    float num3 = building_Temperature + num1 / building_heat_capacity;
    double min2 = (double) min1;
    double max2 = (double) max1;
    double num4 = (double) Mathf.Clamp((float) num2, (float) min2, (float) max2);
    float num5 = Mathf.Clamp(num3, min1, max1);
    double num6 = (double) content_temperature;
    float num7 = Mathf.Abs((float) (num4 - num6));
    double num8 = (double) Mathf.Abs(num5 - building_Temperature);
    float a = num7 * content_heat_capacity;
    double num9 = (double) building_heat_capacity;
    float b = (float) (num8 * num9);
    return Mathf.Min(a, b) * Mathf.Sign(maxWattsTransfered);
  }

  private static float GetFinalContentTemperature(
    float KJT,
    float building_Temperature,
    float building_heat_capacity,
    float content_temperature,
    float content_heat_capacity)
  {
    float num1 = -KJT;
    float num2 = Mathf.Max(0.0f, content_temperature + num1 / content_heat_capacity);
    float num3 = Mathf.Max(0.0f, building_Temperature - num1 / building_heat_capacity);
    return ((double) content_temperature - (double) building_Temperature) * ((double) num2 - (double) num3) < 0.0 ? (float) ((double) content_temperature * (double) content_heat_capacity / ((double) content_heat_capacity + (double) building_heat_capacity) + (double) building_Temperature * (double) building_heat_capacity / ((double) content_heat_capacity + (double) building_heat_capacity)) : num2;
  }

  private static float GetFinalBuildingTemperature(
    float content_temperature,
    float content_final_temperature,
    float content_heat_capacity,
    float building_temperature,
    float building_heat_capacity)
  {
    double num1 = ((double) content_temperature - (double) content_final_temperature) * (double) content_heat_capacity;
    float min = Mathf.Min(content_temperature, building_temperature);
    float max = Mathf.Max(content_temperature, building_temperature);
    double num2 = (double) building_heat_capacity;
    float num3 = (float) (num1 / num2);
    return Mathf.Clamp(building_temperature + num3, min, max);
  }

  public class Def : StateMachine.BaseDef
  {
    public ConduitType type = ConduitType.Liquid;
    public float pumpKGRate;
  }

  public new class Instance(IStateMachineTarget master, ContactConductivePipeBridge.Def def) : 
    GameStateMachine<ContactConductivePipeBridge, ContactConductivePipeBridge.Instance, IStateMachineTarget, ContactConductivePipeBridge.Def>.GameInstance(master, def)
  {
    public ConduitType type = ConduitType.Liquid;
    public HandleVector<int>.Handle structureHandle;
    public int inputCell = -1;
    public int outputCell = -1;
    [MyCmpGet]
    public Building building;

    public Tag tag => this.type != ConduitType.Liquid ? GameTags.Gas : GameTags.Liquid;

    public override void StartSM()
    {
      base.StartSM();
      this.inputCell = this.building.GetUtilityInputCell();
      this.outputCell = this.building.GetUtilityOutputCell();
      this.structureHandle = GameComps.StructureTemperatures.GetHandle(this.gameObject);
      Conduit.GetFlowManager(this.type).AddConduitUpdater(new System.Action<float>(this.Flow), ConduitFlowPriority.Default);
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new System.Action<float>(this.Flow));
    }

    private void Flow(float dt)
    {
      ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
      if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
        return;
      ConduitFlow.ConduitContents contents1 = flowManager.GetContents(this.inputCell);
      ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.outputCell);
      float num1 = Mathf.Min(contents1.mass, this.def.pumpKGRate * dt);
      if (!flowManager.CanMergeContents(contents1, contents2, num1))
        return;
      double num2 = (double) this.smi.sm.noLiquidTimer.Set(1.5f, this.smi);
      float allowedForMerging = flowManager.GetAmountAllowedForMerging(contents1, contents2, num1);
      if ((double) allowedForMerging <= 0.0)
        return;
      float temperature = this.ExchangeStorageTemperatureWithBuilding(contents1, allowedForMerging, dt);
      float delta = (this.def.type == ConduitType.Liquid ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow).AddElement(this.outputCell, contents1.element, allowedForMerging, temperature, contents1.diseaseIdx, contents1.diseaseCount);
      if ((double) allowedForMerging != (double) delta)
        Debug.Log((object) ("Mass Differs By: " + (allowedForMerging - delta).ToString()));
      flowManager.RemoveElement(this.inputCell, delta);
    }

    private float ExchangeStorageTemperatureWithBuilding(
      ConduitFlow.ConduitContents content,
      float mass,
      float dt)
    {
      PrimaryElement component = this.building.GetComponent<PrimaryElement>();
      float building_thermal_conductivity = component.Element.thermalConductivity * this.building.Def.ThermalConductivity;
      if ((double) mass > 0.0)
      {
        Element elementByHash = ElementLoader.FindElementByHash(content.element);
        float content_heat_capacity = mass * elementByHash.specificHeatCapacity;
        float building_heat_capacity = this.building.Def.MassForTemperatureModification * component.Element.specificHeatCapacity;
        float temperature1 = component.Temperature;
        float temperature2 = content.temperature;
        double maxWattsTransfered = (double) ContactConductivePipeBridge.CalculateMaxWattsTransfered(temperature1, building_thermal_conductivity, temperature2, elementByHash.thermalConductivity);
        float contentTemperature = ContactConductivePipeBridge.GetFinalContentTemperature(ContactConductivePipeBridge.GetKilloJoulesTransfered((float) maxWattsTransfered, dt, temperature1, building_heat_capacity, temperature2, content_heat_capacity), temperature1, building_heat_capacity, temperature2, content_heat_capacity);
        float buildingTemperature = ContactConductivePipeBridge.GetFinalBuildingTemperature(temperature2, contentTemperature, content_heat_capacity, temperature1, building_heat_capacity);
        float delta_kilojoules = Mathf.Sign((float) maxWattsTransfered) * Mathf.Abs(buildingTemperature - temperature1) * building_heat_capacity;
        if ((((double) buildingTemperature < 0.0 ? 0 : ((double) buildingTemperature <= 10000.0 ? 1 : 0)) & ((double) contentTemperature < 0.0 ? 0 : ((double) contentTemperature <= 10000.0 ? 1 : 0))) != 0)
        {
          GameComps.StructureTemperatures.ProduceEnergy(this.smi.structureHandle, delta_kilojoules, (string) BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time);
          return contentTemperature;
        }
      }
      return 0.0f;
    }
  }
}
