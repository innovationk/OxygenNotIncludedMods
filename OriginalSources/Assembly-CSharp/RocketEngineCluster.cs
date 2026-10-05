// Decompiled with JetBrains decompiler
// Type: RocketEngineCluster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngineCluster : StateMachineComponent<RocketEngineCluster.StatesInstance>
{
  public float exhaustEmitRate = 50f;
  public float exhaustTemperature = 1500f;
  public SpawnFXHashes explosionEffectHash;
  public SimHashes exhaustElement = SimHashes.CarbonDioxide;
  public Tag fuelTag;
  public float efficiency = 1f;
  public bool requireOxidizer = true;
  public int maxModules = 32 /*0x20*/;
  public int maxHeight;
  public bool mainEngine = true;
  public byte exhaustDiseaseIdx = byte.MaxValue;
  public int exhaustDiseaseCount;
  public bool emitRadiation;
  [MyCmpGet]
  private RadiationEmitter radiationEmitter;
  [MyCmpGet]
  private Generator powerGenerator;
  [MyCmpReq]
  private KBatchedAnimController animController;
  public Light2D flameLight;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    if (!this.mainEngine)
      return;
    this.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new RequireAttachedComponent(this.gameObject.GetComponent<AttachableBuilding>(), typeof (IFuelTank), (string) UI.STARMAP.COMPONENT.FUEL_TANK));
    if (this.requireOxidizer)
      this.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new RequireAttachedComponent(this.gameObject.GetComponent<AttachableBuilding>(), typeof (OxidizerTank), (string) UI.STARMAP.COMPONENT.OXIDIZER_TANK));
    this.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, (ProcessCondition) new ConditionRocketHeight(this));
  }

  private void ConfigureFlameLight()
  {
    this.flameLight = this.gameObject.AddOrGet<Light2D>();
    this.flameLight.Color = Color.white;
    this.flameLight.overlayColour = LIGHT2D.LIGHTBUG_OVERLAYCOLOR;
    this.flameLight.Range = 10f;
    this.flameLight.Angle = 0.0f;
    this.flameLight.Direction = LIGHT2D.LIGHTBUG_DIRECTION;
    this.flameLight.Offset = LIGHT2D.LIGHTBUG_OFFSET;
    this.flameLight.shape = LightShape.Circle;
    this.flameLight.drawOverlay = true;
    this.flameLight.Lux = 80000;
    this.flameLight.emitter.RemoveFromGrid();
    this.gameObject.AddOrGet<LightSymbolTracker>().targetSymbol = this.GetComponent<KBatchedAnimController>().CurrentAnim.rootSymbol;
    this.flameLight.enabled = false;
  }

  private void UpdateFlameLight(int cell)
  {
    int num = (int) this.smi.master.flameLight.RefreshShapeAndPosition();
    if (Grid.IsValidCell(cell))
    {
      if (this.smi.master.flameLight.enabled || (double) this.smi.timeinstate <= 3.0)
        return;
      this.smi.master.flameLight.enabled = true;
    }
    else
      this.smi.master.flameLight.enabled = false;
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public class StatesInstance : 
    GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.GameInstance
  {
    public Vector3 radiationEmissionBaseOffset;
    private int pad_cell;

    public StatesInstance(RocketEngineCluster smi)
      : base(smi)
    {
      if (!smi.emitRadiation)
        return;
      DebugUtil.Assert((UnityEngine.Object) smi.radiationEmitter != (UnityEngine.Object) null, "emitRadiation enabled but no RadiationEmitter component");
      this.radiationEmissionBaseOffset = smi.radiationEmitter.emissionOffset;
    }

    public void BeginBurn()
    {
      if (this.smi.master.emitRadiation)
        this.smi.master.radiationEmitter.SetEmitting(true);
      LaunchPad currentPad = this.smi.master.GetComponent<RocketModuleCluster>().CraftInterface.CurrentPad;
      if ((UnityEngine.Object) currentPad != (UnityEngine.Object) null)
      {
        this.pad_cell = Grid.PosToCell(currentPad.gameObject.transform.GetPosition());
        if (this.smi.master.exhaustDiseaseIdx == byte.MaxValue)
          return;
        currentPad.GetComponent<PrimaryElement>().AddDisease(this.smi.master.exhaustDiseaseIdx, this.smi.master.exhaustDiseaseCount, "rocket exhaust");
      }
      else
      {
        Debug.LogWarning((object) "RocketEngineCluster missing LaunchPad for burn.");
        this.pad_cell = Grid.InvalidCell;
      }
    }

    public void DoBurn(float dt)
    {
      int cell = Grid.PosToCell(this.smi.master.gameObject.transform.GetPosition() + this.smi.master.animController.Offset);
      if (Grid.AreCellsInSameWorld(cell, this.pad_cell))
        SimMessages.EmitMass(cell, ElementLoader.GetElementIndex(this.smi.master.exhaustElement), dt * this.smi.master.exhaustEmitRate, this.smi.master.exhaustTemperature, this.smi.master.exhaustDiseaseIdx, this.smi.master.exhaustDiseaseCount);
      if (this.smi.master.emitRadiation)
      {
        Vector3 emissionOffset = this.smi.master.radiationEmitter.emissionOffset;
        this.smi.master.radiationEmitter.emissionOffset = this.smi.radiationEmissionBaseOffset + this.smi.master.animController.Offset;
        if (Grid.AreCellsInSameWorld(this.smi.master.radiationEmitter.GetEmissionCell(), this.pad_cell))
        {
          this.smi.master.radiationEmitter.Refresh();
        }
        else
        {
          this.smi.master.radiationEmitter.emissionOffset = emissionOffset;
          this.smi.master.radiationEmitter.SetEmitting(false);
        }
      }
      int num1 = 10;
      for (int index = 1; index < num1; ++index)
      {
        int num2 = Grid.OffsetCell(cell, -1, -index);
        int num3 = Grid.OffsetCell(cell, 0, -index);
        int num4 = Grid.OffsetCell(cell, 1, -index);
        if (Grid.AreCellsInSameWorld(num2, this.pad_cell))
        {
          if (this.smi.master.exhaustDiseaseIdx != byte.MaxValue)
            SimMessages.ModifyDiseaseOnCell(num2, this.smi.master.exhaustDiseaseIdx, (int) ((double) this.smi.master.exhaustDiseaseCount / ((double) index + 1.0)));
          SimMessages.ModifyEnergy(num2, this.smi.master.exhaustTemperature / (float) (index + 1), 3200f, SimMessages.EnergySourceID.Burner);
        }
        if (Grid.AreCellsInSameWorld(num3, this.pad_cell))
        {
          if (this.smi.master.exhaustDiseaseIdx != byte.MaxValue)
            SimMessages.ModifyDiseaseOnCell(num3, this.smi.master.exhaustDiseaseIdx, (int) ((double) this.smi.master.exhaustDiseaseCount / (double) index));
          SimMessages.ModifyEnergy(num3, this.smi.master.exhaustTemperature / (float) index, 3200f, SimMessages.EnergySourceID.Burner);
        }
        if (Grid.AreCellsInSameWorld(num4, this.pad_cell))
        {
          if (this.smi.master.exhaustDiseaseIdx != byte.MaxValue)
            SimMessages.ModifyDiseaseOnCell(num4, this.smi.master.exhaustDiseaseIdx, (int) ((double) this.smi.master.exhaustDiseaseCount / ((double) index + 1.0)));
          SimMessages.ModifyEnergy(num4, this.smi.master.exhaustTemperature / (float) (index + 1), 3200f, SimMessages.EnergySourceID.Burner);
        }
      }
    }

    public void EndBurn()
    {
      if (this.smi.master.emitRadiation)
      {
        this.smi.master.radiationEmitter.emissionOffset = this.smi.radiationEmissionBaseOffset;
        this.smi.master.radiationEmitter.SetEmitting(false);
      }
      this.pad_cell = Grid.InvalidCell;
    }
  }

  public class States : 
    GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster>
  {
    public RocketEngineCluster.States.InitializingStates initializing;
    public RocketEngineCluster.States.IdleStates idle;
    public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burning_pre;
    public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burning;
    public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State burnComplete;
    public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State space;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.initializing.load;
      this.initializing.load.ScheduleGoTo(0.0f, (StateMachine.BaseState) this.initializing.decide);
      this.initializing.decide.Transition(this.space, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketInSpace)).Transition(this.burning, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketAirborne)).Transition((GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State) this.idle, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsRocketGrounded));
      this.idle.DefaultState(this.idle.grounded).EventTransition(GameHashes.RocketLaunched, this.burning_pre);
      this.idle.grounded.EventTransition(GameHashes.LaunchConditionChanged, this.idle.ready, new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsReadyToLaunch)).QueueAnim("grounded", true);
      this.idle.ready.EventTransition(GameHashes.LaunchConditionChanged, this.idle.grounded, GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Not(new StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.Transition.ConditionCallback(this.IsReadyToLaunch))).PlayAnim("pre_ready_to_launch", KAnim.PlayMode.Once).QueueAnim("ready_to_launch", true).Exit((StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State.Callback) (smi =>
      {
        KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          return;
        component.Play((HashedString) "pst_ready_to_launch");
      }));
      this.burning_pre.PlayAnim("launch_pre").OnAnimQueueComplete(this.burning);
      this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete).PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter((StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State.Callback) (smi => smi.BeginBurn())).Update((Action<RocketEngineCluster.StatesInstance, float>) ((smi, dt) => smi.DoBurn(dt))).Exit((StateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State.Callback) (smi => smi.EndBurn())).TagTransition(GameTags.RocketInSpace, this.space);
      this.space.EventTransition(GameHashes.DoReturnRocket, this.burning);
      this.burnComplete.PlayAnim("launch_pst", KAnim.PlayMode.Loop).GoTo((GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State) this.idle);
    }

    private bool IsReadyToLaunch(RocketEngineCluster.StatesInstance smi)
    {
      return smi.GetComponent<RocketModuleCluster>().CraftInterface.CheckPreppedForLaunch();
    }

    public bool IsRocketAirborne(RocketEngineCluster.StatesInstance smi)
    {
      return smi.master.HasTag(GameTags.RocketNotOnGround) && !smi.master.HasTag(GameTags.RocketInSpace);
    }

    public bool IsRocketGrounded(RocketEngineCluster.StatesInstance smi)
    {
      return smi.master.HasTag(GameTags.RocketOnGround);
    }

    public bool IsRocketInSpace(RocketEngineCluster.StatesInstance smi)
    {
      return smi.master.HasTag(GameTags.RocketInSpace);
    }

    public class InitializingStates : 
      GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State
    {
      public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State load;
      public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State decide;
    }

    public class IdleStates : 
      GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State
    {
      public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State grounded;
      public GameStateMachine<RocketEngineCluster.States, RocketEngineCluster.StatesInstance, RocketEngineCluster, object>.State ready;
    }
  }
}
