// Decompiled with JetBrains decompiler
// Type: WideFarmTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WideFarmTile : 
  GameStateMachine<WideFarmTile, WideFarmTile.Instance, IStateMachineTarget, WideFarmTile.Def>
{
  private const string LIQUID_METER_ANIM_NAME = "meter";
  private const string LIQUID_METER_TARGET_NAME = "meter_target";
  private const string LIQUID_METER_TINT_SYMBOL_NAME = "meter_fill";

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.root;
    this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<WideFarmTile, WideFarmTile.Instance, IStateMachineTarget, WideFarmTile.Def>.State.Callback(WideFarmTile.RefreshLiquidMeter));
  }

  private static void RefreshLiquidMeter(WideFarmTile.Instance smi) => smi.RefreshLiquidMeter();

  public class Def : StateMachine.BaseDef
  {
  }

  public new class Instance : 
    GameStateMachine<WideFarmTile, WideFarmTile.Instance, IStateMachineTarget, WideFarmTile.Def>.GameInstance
  {
    private MeterController liquidMeter;
    private Storage storage;
    private ConduitConsumer conduitConsumer;

    public Instance(IStateMachineTarget master, WideFarmTile.Def def)
      : base(master, def)
    {
      this.liquidMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.Building, Array.Empty<string>());
      this.conduitConsumer = this.GetComponent<ConduitConsumer>();
      this.storage = this.GetComponent<Storage>();
    }

    public override void StartSM()
    {
      base.StartSM();
      this.RefreshLiquidMeter();
    }

    public void RefreshLiquidMeter()
    {
      this.liquidMeter.SetPositionPercent(this.conduitConsumer.stored_mass / this.conduitConsumer.capacityKG);
      GameObject first = this.storage.FindFirst(GameTags.Liquid);
      if ((UnityEngine.Object) first == (UnityEngine.Object) null)
        return;
      GameUtil.TintLiquidSymbolOnBuilding("meter_fill", this.liquidMeter.meterController, first.GetComponent<PrimaryElement>().Element);
    }
  }
}
