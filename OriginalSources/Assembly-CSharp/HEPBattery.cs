// Decompiled with JetBrains decompiler
// Type: HEPBattery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class HEPBattery : 
  GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>
{
  public static readonly HashedString FIRE_PORT_ID = (HashedString) "HEPBatteryFire";
  public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State inoperational;
  public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State operational;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inoperational;
    this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational).Update((System.Action<HEPBattery.Instance, float>) ((smi, dt) =>
    {
      smi.DoConsumeParticlesWhileDisabled(dt);
      smi.UpdateDecayStatusItem(false);
    }));
    this.operational.Enter("SetActive(true)", (StateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State.Callback) (smi => smi.operational.SetActive(true))).Exit("SetActive(false)", (StateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State.Callback) (smi => smi.operational.SetActive(false))).PlayAnim("on", KAnim.PlayMode.Loop).TagTransition(GameTags.Operational, this.inoperational, true).Update(new System.Action<HEPBattery.Instance, float>(this.LauncherUpdate));
  }

  public void LauncherUpdate(HEPBattery.Instance smi, float dt)
  {
    smi.UpdateDecayStatusItem(true);
    smi.UpdateMeter();
    smi.operational.SetActive((double) smi.particleStorage.Particles > 0.0);
    smi.launcherTimer += dt;
    if ((double) smi.launcherTimer < (double) smi.def.minLaunchInterval || !smi.AllowSpawnParticles || (double) smi.particleStorage.Particles < (double) smi.particleThreshold)
      return;
    smi.launcherTimer = 0.0f;
    this.Fire(smi);
  }

  public void Fire(HEPBattery.Instance smi)
  {
    int particleOutputCell = smi.GetComponent<Building>().GetHighEnergyParticleOutputCell();
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "HighEnergyParticle"), Grid.CellToPosCCC(particleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2);
    gameObject.SetActive(true);
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
    component.payload = smi.particleStorage.ConsumeAndGet(smi.particleThreshold);
    component.SetDirection(smi.def.direction);
  }

  public class Def : StateMachine.BaseDef
  {
    public float particleDecayRate;
    public float minLaunchInterval;
    public float minSlider;
    public float maxSlider;
    public EightDirection direction;
  }

  public new class Instance : 
    GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.GameInstance,
    ISingleSliderControl,
    ISliderControl
  {
    [MyCmpReq]
    public HighEnergyParticleStorage particleStorage;
    [MyCmpGet]
    public Operational operational;
    [MyCmpAdd]
    public CopyBuildingSettings copyBuildingSettings;
    [Serialize]
    public float launcherTimer;
    [Serialize]
    public float particleThreshold = 50f;
    public bool ShowWorkingStatus;
    private bool m_skipFirstUpdate = true;
    private MeterController meterController;
    private Guid statusHandle = Guid.Empty;
    private bool hasLogicWire;
    private bool isLogicActive;

    public Instance(IStateMachineTarget master, HEPBattery.Def def)
      : base(master, def)
    {
      this.Subscribe(-801688580, new System.Action<object>(this.OnLogicValueChanged));
      this.Subscribe(-905833192, new System.Action<object>(this.OnCopySettings));
      this.meterController = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
      this.UpdateMeter();
    }

    public void DoConsumeParticlesWhileDisabled(float dt)
    {
      if (this.m_skipFirstUpdate)
      {
        this.m_skipFirstUpdate = false;
      }
      else
      {
        double num = (double) this.particleStorage.ConsumeAndGet(dt * this.def.particleDecayRate);
        this.UpdateMeter();
      }
    }

    public void UpdateMeter(object data = null)
    {
      this.meterController.SetPositionPercent(this.particleStorage.Particles / this.particleStorage.Capacity());
    }

    public void UpdateDecayStatusItem(bool hasPower)
    {
      if (!hasPower)
      {
        if ((double) this.particleStorage.Particles > 0.0)
        {
          if (!(this.statusHandle == Guid.Empty))
            return;
          this.statusHandle = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts);
        }
        else
        {
          if (!(this.statusHandle != Guid.Empty))
            return;
          this.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle);
          this.statusHandle = Guid.Empty;
        }
      }
      else
      {
        if (!(this.statusHandle != Guid.Empty))
          return;
        this.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle);
        this.statusHandle = Guid.Empty;
      }
    }

    public bool AllowSpawnParticles => this.hasLogicWire && this.isLogicActive;

    public bool HasLogicWire => this.hasLogicWire;

    public bool IsLogicActive => this.isLogicActive;

    private LogicCircuitNetwork GetNetwork()
    {
      return Game.Instance.logicCircuitManager.GetNetworkForCell(this.GetComponent<LogicPorts>().GetPortCell(HEPBattery.FIRE_PORT_ID));
    }

    private void OnLogicValueChanged(object data)
    {
      LogicValueChanged logicValueChanged = (LogicValueChanged) data;
      if (!(logicValueChanged.portID == HEPBattery.FIRE_PORT_ID))
        return;
      this.isLogicActive = logicValueChanged.newValue > 0;
      this.hasLogicWire = this.GetNetwork() != null;
    }

    private void OnCopySettings(object data)
    {
      if (!(data is GameObject go))
        return;
      HEPBattery.Instance smi = go.GetSMI<HEPBattery.Instance>();
      if (smi == null)
        return;
      this.particleThreshold = smi.particleThreshold;
    }

    public string SliderTitleKey => "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";

    public string SliderUnits => (string) UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;

    public int SliderDecimalPlaces(int index) => 0;

    public float GetSliderMin(int index) => this.def.minSlider;

    public float GetSliderMax(int index) => this.def.maxSlider;

    public float GetSliderValue(int index) => this.particleThreshold;

    public void SetSliderValue(float value, int index) => this.particleThreshold = value;

    public string GetSliderTooltipKey(int index)
    {
      return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
    }

    string ISliderControl.GetSliderTooltip(int index)
    {
      return string.Format((string) Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), (object) this.particleThreshold);
    }
  }
}
