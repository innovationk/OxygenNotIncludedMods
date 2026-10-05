// Decompiled with JetBrains decompiler
// Type: UnderwaterVent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnderwaterVent : 
  GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>
{
  private const string IDLE_ANIM_NAME = "off";
  private const string ERUPTING_ANIM_NAME = "erupting";
  private const string BLOCKED_ANIM_NAME = "blocked";
  private const string METER_TARGET_NAME = "target_meter";
  private const string METER_ANIM_NAME = "meter";
  private const string METER_ANIM_COLLAPSE_NAME = "collapsed";
  private static readonly string[] ROCK_SYMBOLS_NAME = new string[4]
  {
    "rock_1",
    "rock_2",
    "rock_3",
    "rock_4"
  };
  public GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State off;
  public UnderwaterVent.OnStates on;
  public StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.FloatParameter BuildUp;
  public StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.TargetParameter MeterController;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.off;
    this.off.EventTransition(GameHashes.EntombedChanged, (GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State) this.on, GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.Not(new StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.Transition.ConditionCallback(UnderwaterVent.ShouldBeOff))).EventTransition(GameHashes.SubmergedStateChanged, (GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State) this.on, GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.Not(new StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.Transition.ConditionCallback(UnderwaterVent.ShouldBeOff))).PlayAnim("off");
    this.on.EventTransition(GameHashes.EntombedChanged, this.off, new StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.Transition.ConditionCallback(UnderwaterVent.ShouldBeOff)).EventTransition(GameHashes.SubmergedStateChanged, this.off, new StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.Transition.ConditionCallback(UnderwaterVent.ShouldBeOff)).DefaultState(this.on.erupting);
    this.on.erupting.ParamTransition<float>((StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.Parameter<float>) this.BuildUp, (GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State) this.on.blocked, GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.IsGTEOne).PlayAnim("erupting", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().MiscStatusItems.UnderwaterVentEmiting).ToggleStatusItem(Db.Get().MiscStatusItems.UnderwaterVentBuildUpProgress).Enter(new StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State.Callback(UnderwaterVent.RefreshBuildUpMeter)).Update(new System.Action<UnderwaterVent.Instance, float>(UnderwaterVent.EruptionUpdate), UpdateRate.SIM_1000ms);
    this.on.blocked.TriggerOnEnter(GameHashes.VentBlocked).DefaultState(this.on.blocked.idle);
    this.on.blocked.idle.ParamTransition<float>((StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.Parameter<float>) this.BuildUp, this.on.blocked.unblock, GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.IsZero).PlayAnim("blocked", KAnim.PlayMode.Once).ToggleStatusItem(Db.Get().MiscStatusItems.UnderwaterVentBlocked);
    this.on.blocked.unblock.Target(this.MeterController).PlayAnim("collapsed", KAnim.PlayMode.Once).OnAnimQueueComplete(this.on.erupting).Target(this.masterTarget).Exit(new StateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State.Callback(UnderwaterVent.SpawnSolidDebri));
  }

  private static bool ShouldBeOff(UnderwaterVent.Instance smi)
  {
    return UnderwaterVent.IsEntombed(smi) || !UnderwaterVent.IsSubmerged(smi);
  }

  private static bool IsEntombed(UnderwaterVent.Instance smi) => smi.IsEntombed;

  private static bool IsSubmerged(UnderwaterVent.Instance smi) => smi.IsSubmerged;

  private static void RefreshBuildUpMeter(UnderwaterVent.Instance smi) => smi.RefreshBuildUpMeter();

  private static void SpawnSolidDebri(UnderwaterVent.Instance smi) => smi.SpawnSolidDebri();

  private static void EruptionUpdate(UnderwaterVent.Instance smi, float dt)
  {
    smi.EruptionUpdate(dt);
  }

  public struct Data(
    Vector3 bubbleSpawnOffset,
    Vector3 solidSpawnOffset,
    SimHashes bubbleElement,
    float bubbleTemp,
    float bubbleMassPerSecond,
    SimHashes solidElement,
    float solidMass,
    float solidTemp,
    float buildUpDuration)
  {
    public Vector3 BubbleSpawnOffset = bubbleSpawnOffset;
    public Vector3 SolidSpawnOffset = solidSpawnOffset;
    public SimHashes BubbleElement = bubbleElement;
    public float BubbleTemp = bubbleTemp;
    public float BubbleMassRate = bubbleMassPerSecond;
    public SimHashes SolidElement = solidElement;
    public float SolidMass = solidMass;
    public float SolidTemp = solidTemp;
    public float BuildUpDuration = buildUpDuration;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public UnderwaterVent.Data data;
    private List<Descriptor> cachedDescriptor;

    public List<Descriptor> GetDescriptors(GameObject go)
    {
      if (this.cachedDescriptor == null)
      {
        this.cachedDescriptor = new List<Descriptor>();
        this.cachedDescriptor.Add(new Descriptor(GameUtil.SafeStringFormat((string) UI.BUILDINGEFFECTS.UNDERWATERVENT_SHEARING, (object) GameUtil.GetFormattedMass(this.data.SolidMass)), GameUtil.SafeStringFormat((string) UI.BUILDINGEFFECTS.TOOLTIPS.UNDERWATERVENT_SHEARING, (object) this.data.SolidElement.CreateTag().ProperName())));
      }
      return this.cachedDescriptor;
    }
  }

  public class OnStates : 
    GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State
  {
    public GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State erupting;
    public UnderwaterVent.BlockStates blocked;
  }

  public class BlockStates : 
    GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State
  {
    public GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State idle;
    public GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.State unblock;
  }

  public new class Instance : 
    GameStateMachine<UnderwaterVent, UnderwaterVent.Instance, IStateMachineTarget, UnderwaterVent.Def>.GameInstance
  {
    private EntombVulnerable entombVulnerable;
    private Submergable submergable;
    private global::MeterController buildUpMeter;

    public float BuildUpProgress => this.sm.BuildUp.Get(this);

    public bool IsBlocked => (double) this.BuildUpProgress >= 1.0;

    public bool IsSubmerged => this.submergable.IsSubmerged;

    public bool IsEntombed => this.entombVulnerable.GetEntombed;

    public Instance(IStateMachineTarget master, UnderwaterVent.Def def)
      : base(master, def)
    {
      this.entombVulnerable = this.GetComponent<EntombVulnerable>();
      this.submergable = this.GetComponent<Submergable>();
      this.buildUpMeter = new global::MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "target_meter", "meter", Meter.Offset.Infront, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
      this.sm.MeterController.Set(this.buildUpMeter.meterController.gameObject, this, false);
    }

    public override void StartSM()
    {
      base.StartSM();
      this.RefreshBuildUpMeter();
    }

    public void EruptionUpdate(float dt)
    {
      float num1 = dt / this.def.data.BuildUpDuration;
      float mass = this.def.data.BubbleMassRate * dt;
      if ((double) mass >= 9.9999997171806854E-10)
      {
        float bubbleTemp = this.def.data.BubbleTemp;
        Vector3 position = Grid.CellToPos(Grid.PosToCell(this.gameObject)) + this.def.data.BubbleSpawnOffset;
        SimHashes bubbleElement = this.def.data.BubbleElement;
        BubbleManager.instance.SpawnBubble(bubbleElement, (Vector2) position, mass, bubbleTemp, BubbleManager.Disease.None);
      }
      double num2 = (double) this.sm.BuildUp.Set(this.BuildUpProgress + num1, this);
      this.RefreshBuildUpMeter();
    }

    public void RefreshBuildUpMeter()
    {
      if (this.buildUpMeter.meterController.currentAnim != (HashedString) "meter")
        this.buildUpMeter.meterController.Play((HashedString) "meter", KAnim.PlayMode.Paused);
      this.buildUpMeter.SetPositionPercent(this.BuildUpProgress);
    }

    public void SpawnSolidDebri()
    {
      KBatchedAnimController meterController = this.buildUpMeter.meterController;
      List<Vector3> vector3List = new List<Vector3>(UnderwaterVent.ROCK_SYMBOLS_NAME.Length);
      float layerZ = Grid.GetLayerZ(Grid.SceneLayer.Ore);
      for (int index = 0; index < UnderwaterVent.ROCK_SYMBOLS_NAME.Length; ++index)
      {
        string symbol = UnderwaterVent.ROCK_SYMBOLS_NAME[index];
        bool symbolVisible;
        Matrix4x4 symbolTransform = meterController.GetSymbolTransform((HashedString) symbol, out symbolVisible);
        if (symbolVisible)
        {
          Vector3 column = (Vector3) symbolTransform.GetColumn(3) with
          {
            z = layerZ
          };
          vector3List.Add(column);
        }
      }
      if (vector3List.Count == 0)
      {
        Vector3 vector3 = (Grid.CellToPos(Grid.PosToCell(this.gameObject)) + this.def.data.SolidSpawnOffset) with
        {
          z = layerZ
        };
        vector3List.Add(vector3);
      }
      float massPerRock = this.def.data.SolidMass / (float) vector3List.Count;
      for (int index = 0; index < vector3List.Count; ++index)
        this.SpawnRockDebri(vector3List[index], massPerRock);
    }

    private void SpawnRockDebri(Vector3 spawnPos, float massPerRock)
    {
      GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.def.data.SolidElement.CreateTag()), Grid.SceneLayer.Ore);
      gameObject.transform.position = spawnPos;
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      component.Mass = massPerRock;
      component.Temperature = this.def.data.SolidTemp;
      gameObject.gameObject.SetActive(true);
    }

    public void Unblock()
    {
      double num = (double) this.sm.BuildUp.Set(0.0f, this);
    }
  }
}
