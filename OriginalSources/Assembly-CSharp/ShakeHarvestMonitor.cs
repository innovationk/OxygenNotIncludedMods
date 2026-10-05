// Decompiled with JetBrains decompiler
// Type: ShakeHarvestMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ShakeHarvestMonitor : 
  GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>
{
  public static readonly Tag Reserved = GameTags.Creatures.ReservedByCreature;
  public GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State cooldown;
  public ShakeHarvestMonitor.HarvestStates harvest;
  public StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.FloatParameter elapsedTime = new StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.FloatParameter(float.MaxValue);
  public StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.TargetParameter plant;
  public StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.Signal failed;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.cooldown;
    double num1;
    this.cooldown.Update((System.Action<ShakeHarvestMonitor.Instance, float>) ((smi, dt) => num1 = (double) this.elapsedTime.Set(this.elapsedTime.Get(smi) + dt, smi))).ParamTransition<float>((StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.Parameter<float>) this.elapsedTime, (GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State) this.harvest, (StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.Parameter<float>.Callback) ((smi, elapsedTime) => (double) elapsedTime > (double) smi.def.cooldownDuration));
    this.harvest.DefaultState(this.harvest.seek).ParamTransition<float>((StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.Parameter<float>) this.elapsedTime, this.cooldown, GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.IsLTEZero);
    this.harvest.seek.PreBrainUpdate((System.Action<ShakeHarvestMonitor.Instance>) (smi => this.plant.Set((KMonoBehaviour) smi.Seek(), smi))).ParamTransition<GameObject>((StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.Parameter<GameObject>) this.plant, this.harvest.execute, GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.IsNotNull);
    double num2;
    this.harvest.execute.Enter((StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State.Callback) (smi => this.plant.Get(smi).AddTag(ShakeHarvestMonitor.Reserved))).OnSignal(this.failed, this.harvest.seek).ToggleBehaviour(GameTags.Creatures.WantsToHarvest, (StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.Transition.ConditionCallback) (smi => (UnityEngine.Object) this.plant.Get(smi) != (UnityEngine.Object) null), (System.Action<ShakeHarvestMonitor.Instance>) (smi => num2 = (double) this.elapsedTime.Set(0.0f, smi))).Exit((StateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State.Callback) (smi =>
    {
      GameObject go = this.plant.Get(smi);
      if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
        return;
      go.RemoveTag(ShakeHarvestMonitor.Reserved);
      this.plant.Set((KMonoBehaviour) null, smi);
    }));
  }

  public class Def : StateMachine.BaseDef
  {
    public float cooldownDuration;
    public HashSet<Tag> harvestablePlants = new HashSet<Tag>();
    public int radius = 10;
    private Navigator.Scanner<KPrefabID> plantSeeker;

    public Navigator.Scanner<KPrefabID> PlantSeeker
    {
      get
      {
        if (this.plantSeeker == null)
        {
          this.plantSeeker = new Navigator.Scanner<KPrefabID>(this.radius, GameScenePartitioner.Instance.plants, new Func<KPrefabID, bool>(this.IsHarvestablePlant));
          this.plantSeeker.SetDynamicOffsetsFn((System.Action<KPrefabID, List<CellOffset>>) ((plant, offsets) => ShakeHarvestMonitor.Def.GetApproachOffsets(plant.gameObject, offsets)));
        }
        return this.plantSeeker;
      }
    }

    private bool IsHarvestablePlant(KPrefabID plant)
    {
      if ((UnityEngine.Object) plant == (UnityEngine.Object) null || plant.pendingDestruction || plant.HasTag(ShakeHarvestMonitor.Reserved) || !this.harvestablePlants.Contains(plant.PrefabID()))
        return false;
      Harvestable component = plant.GetComponent<Harvestable>();
      return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.CanBeHarvested;
    }

    public static void GetApproachOffsets(GameObject plant, List<CellOffset> offsets)
    {
      Extents extents = plant.GetComponent<OccupyArea>().GetExtents();
      int x = -1;
      int width = extents.width;
      for (int index = 0; index != extents.height; ++index)
      {
        int y = index;
        offsets.Add(new CellOffset(x, y));
        offsets.Add(new CellOffset(width, y));
      }
    }
  }

  public class HarvestStates : 
    GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State
  {
    public GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State seek;
    public GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.State execute;
  }

  public new class Instance : 
    GameStateMachine<ShakeHarvestMonitor, ShakeHarvestMonitor.Instance, IStateMachineTarget, ShakeHarvestMonitor.Def>.GameInstance
  {
    private readonly Navigator navigator;

    public Instance(IStateMachineTarget master, ShakeHarvestMonitor.Def def)
      : base(master, def)
    {
      this.navigator = this.GetComponent<Navigator>();
    }

    public KPrefabID Seek()
    {
      return this.def.PlantSeeker.Scan(Grid.PosToXY(this.smi.transform.GetPosition()), this.navigator);
    }
  }
}
