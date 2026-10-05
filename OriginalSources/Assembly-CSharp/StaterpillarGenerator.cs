// Decompiled with JetBrains decompiler
// Type: StaterpillarGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using UnityEngine;

#nullable disable
public class StaterpillarGenerator : Generator
{
  private StaterpillarGenerator.StatesInstance smi;
  [Serialize]
  public Ref<Staterpillar> parent = new Ref<Staterpillar>();

  protected override void OnSpawn()
  {
    Staterpillar staterpillar = this.parent.Get();
    if ((Object) staterpillar == (Object) null || (Object) staterpillar.GetGenerator() != (Object) this)
    {
      Util.KDestroyGameObject(this.gameObject);
    }
    else
    {
      this.smi = new StaterpillarGenerator.StatesInstance(this);
      this.smi.StartSM();
      base.OnSpawn();
    }
  }

  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    ushort circuitId = this.CircuitID;
    this.operational.SetFlag(Generator.wireConnectedFlag, circuitId != ushort.MaxValue);
    if (!this.operational.IsOperational)
      return;
    float wattageRating = this.GetComponent<Generator>().WattageRating;
    if ((double) wattageRating <= 0.0)
      return;
    this.GenerateJoules(Mathf.Max(wattageRating * dt, 1f * dt));
  }

  public class StatesInstance(StaterpillarGenerator master) : 
    GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.GameInstance(master)
  {
    private Attributes attributes;
  }

  public class States : 
    GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator>
  {
    public GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.State idle;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.EventTransition(GameHashes.OperationalChanged, this.idle, (StateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.idle.EventTransition(GameHashes.OperationalChanged, this.root, (StateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).Enter((StateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(true)));
    }
  }
}
