// Decompiled with JetBrains decompiler
// Type: DevHEPSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class DevHEPSpawner : 
  StateMachineComponent<DevHEPSpawner.StatesInstance>,
  IHighEnergyParticleDirection,
  ISingleSliderControl,
  ISliderControl
{
  [MyCmpGet]
  private Operational operational;
  [Serialize]
  private EightDirection _direction;
  public float boltAmount;
  private EightDirectionController directionController;
  private float launcherTimer;
  private MeterController particleController;
  private MeterController progressMeterController;
  [Serialize]
  public Ref<HighEnergyParticlePort> capturedByRef = new Ref<HighEnergyParticlePort>();
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<DevHEPSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DevHEPSpawner>((Action<DevHEPSpawner, object>) ((component, data) => component.OnCopySettings(data)));

  public EightDirection Direction
  {
    get => this._direction;
    set
    {
      this._direction = value;
      if (this.directionController == null)
        return;
      this.directionController.SetRotation((float) (45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
      this.directionController.controller.enabled = false;
      this.directionController.controller.enabled = true;
    }
  }

  private void OnCopySettings(object data)
  {
    DevHEPSpawner component = ((GameObject) data).GetComponent<DevHEPSpawner>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Direction = component.Direction;
    this.boltAmount = component.boltAmount;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<DevHEPSpawner>(-905833192, DevHEPSpawner.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    this.directionController = new EightDirectionController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
    this.Direction = this.Direction;
    this.particleController = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "orb_target", "orb_off", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, Array.Empty<string>());
    this.particleController.gameObject.AddOrGet<LoopingSounds>();
    this.progressMeterController = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
  }

  public void LauncherUpdate(float dt)
  {
    if ((double) this.boltAmount <= 0.0)
      return;
    this.launcherTimer += dt;
    this.progressMeterController.SetPositionPercent(this.launcherTimer / 5f);
    if ((double) this.launcherTimer <= 5.0)
      return;
    this.launcherTimer -= 5f;
    int particleOutputCell = this.GetComponent<Building>().GetHighEnergyParticleOutputCell();
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "HighEnergyParticle"), Grid.CellToPosCCC(particleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2);
    gameObject.SetActive(true);
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
    component.payload = this.boltAmount;
    component.SetDirection(this.Direction);
    this.directionController.PlayAnim("redirect_send");
    this.directionController.controller.Queue((HashedString) "redirect");
    this.particleController.meterController.Play((HashedString) "orb_send");
    this.particleController.meterController.Queue((HashedString) "orb_off");
  }

  public string SliderTitleKey => "";

  public string SliderUnits => (string) UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;

  public int SliderDecimalPlaces(int index) => 0;

  public float GetSliderMin(int index) => 0.0f;

  public float GetSliderMax(int index) => 500f;

  public float GetSliderValue(int index) => this.boltAmount;

  public void SetSliderValue(float value, int index) => this.boltAmount = value;

  public string GetSliderTooltipKey(int index) => "";

  string ISliderControl.GetSliderTooltip(int index) => "";

  public class StatesInstance(DevHEPSpawner smi) : 
    GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.GameInstance(smi)
  {
  }

  public class States : 
    GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner>
  {
    public StateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.BoolParameter isAbsorbingRadiation;
    public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State ready;
    public GameStateMachine<DevHEPSpawner.States, DevHEPSpawner.StatesInstance, DevHEPSpawner, object>.State inoperational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.inoperational;
      this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready);
      this.ready.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).Update((Action<DevHEPSpawner.StatesInstance, float>) ((smi, dt) => smi.master.LauncherUpdate(dt)), UpdateRate.SIM_EVERY_TICK);
    }
  }
}
