// Decompiled with JetBrains decompiler
// Type: Chlorinator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using UnityEngine;

#nullable disable
public class Chlorinator : 
  GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>
{
  private GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State inoperational;
  private Chlorinator.ReadyStates ready;
  public StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.TargetParameter hopper;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.inoperational;
    this.inoperational.TagTransition(GameTags.Operational, (GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State) this.ready);
    this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle);
    this.ready.idle.EventTransition(GameHashes.OnStorageChange, this.ready.wait, (StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.Transition.ConditionCallback) (smi => smi.CanEmit())).EnterTransition(this.ready.wait, (StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.Transition.ConditionCallback) (smi => smi.CanEmit())).Target(this.hopper).PlayAnim("hopper_idle_loop");
    this.ready.wait.ScheduleGoTo(new Func<Chlorinator.StatesInstance, float>(Chlorinator.GetPoppingDelay), (StateMachine.BaseState) this.ready.popPre).EnterTransition(this.ready.idle, (StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.Transition.ConditionCallback) (smi => !smi.CanEmit())).Target(this.hopper).PlayAnim("hopper_idle_loop");
    this.ready.popPre.Target(this.hopper).PlayAnim("meter_hopper_pre").OnAnimQueueComplete(this.ready.pop);
    this.ready.pop.Enter((StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State.Callback) (smi => smi.TryEmit())).Target(this.hopper).PlayAnim("meter_hopper_loop").OnAnimQueueComplete(this.ready.popPst);
    this.ready.popPst.Target(this.hopper).PlayAnim("meter_hopper_pst").OnAnimQueueComplete(this.ready.wait);
  }

  public static float GetPoppingDelay(Chlorinator.StatesInstance smi) => smi.def.popWaitRange.Get();

  public class Def : StateMachine.BaseDef
  {
    public MathUtil.MinMax popWaitRange = new MathUtil.MinMax(0.2f, 0.8f);
    public Tag primaryOreTag;
    public float primaryOreMassPerOre;
    public MathUtil.MinMaxInt primaryOreCount = new MathUtil.MinMaxInt(1, 1);
    public Tag secondaryOreTag;
    public float secondaryOreMassPerOre;
    public MathUtil.MinMaxInt secondaryOreCount = new MathUtil.MinMaxInt(1, 1);
    public Vector3 offset = Vector3.zero;
    public MathUtil.MinMax initialVelocity = new MathUtil.MinMax(1f, 3f);
    public MathUtil.MinMax initialDirectionHalfAngleDegreesRange = new MathUtil.MinMax(160f, 20f);
  }

  public class ReadyStates : 
    GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State
  {
    public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State idle;
    public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State wait;
    public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPre;
    public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State pop;
    public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPst;
  }

  public class StatesInstance : 
    GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.GameInstance
  {
    public Storage storage;
    public MeterController hopperMeter;

    public StatesInstance(IStateMachineTarget master, Chlorinator.Def def)
      : base(master, def)
    {
      this.storage = this.GetComponent<ComplexFabricator>().outStorage;
      this.hopperMeter = new MeterController(master.GetComponent<KAnimControllerBase>(), "meter_target", "meter_hopper_pre", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[1]
      {
        "meter_target"
      });
      this.sm.hopper.Set(this.hopperMeter.gameObject, this, false);
    }

    public bool CanEmit() => !this.storage.IsEmpty();

    public void TryEmit()
    {
      this.TryEmit(this.smi.def.primaryOreCount.Get(), this.def.primaryOreTag, this.def.primaryOreMassPerOre);
      this.TryEmit(this.smi.def.secondaryOreCount.Get(), this.def.secondaryOreTag, this.def.secondaryOreMassPerOre);
    }

    private void TryEmit(int oreSpawnCount, Tag emitTag, float amount)
    {
      GameObject first = this.storage.FindFirst(emitTag);
      if ((UnityEngine.Object) first == (UnityEngine.Object) null)
        return;
      PrimaryElement component = first.GetComponent<PrimaryElement>();
      Substance substance = component.Element.substance;
      float amount_consumed;
      SimUtil.DiseaseInfo disease_info;
      float aggregate_temperature;
      this.storage.ConsumeAndGetDisease(emitTag, amount, out amount_consumed, out disease_info, out aggregate_temperature);
      if ((double) amount_consumed <= 0.0)
        return;
      float mass = amount_consumed * component.MassPerUnit / (float) oreSpawnCount;
      Vector3 vector3_1 = this.smi.gameObject.transform.position + this.def.offset;
      bool flag = (double) UnityEngine.Random.value >= 0.5;
      for (int index = 0; index < oreSpawnCount; ++index)
      {
        float f = (float) ((double) this.def.initialDirectionHalfAngleDegreesRange.Get() * 3.1415927410125732 / 180.0);
        Vector2 vector2 = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
        if (flag)
          vector2.x = -vector2.x;
        flag = !flag;
        vector2 = vector2.normalized;
        Vector3 initial_velocity = (Vector3) (vector2 * this.def.initialVelocity.Get());
        Vector3 vector3_2 = vector3_1 + (Vector3) (vector2 * 0.1f);
        GameObject go = substance.SpawnResource(vector3_2, mass, aggregate_temperature, disease_info.idx, disease_info.count / oreSpawnCount);
        KFMOD.PlayOneShot(GlobalAssets.GetSound("Chlorinator_popping"), CameraController.Instance.GetVerticallyScaledPosition(vector3_2));
        if (GameComps.Fallers.Has((object) go))
          GameComps.Fallers.Remove(go);
        GameComps.Fallers.Add(go, (Vector2) initial_velocity);
      }
    }
  }
}
