// Decompiled with JetBrains decompiler
// Type: PollinateMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
public class PollinateMonitor : 
  GameStateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>
{
  public static Tag ID = new Tag(nameof (PollinateMonitor));
  public GameStateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.State lookingForPlant;
  public GameStateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.State satisfied;
  private StateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.FloatParameter remainingSecondsForEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.lookingForPlant;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.lookingForPlant.PreBrainUpdate(new System.Action<PollinateMonitor.Instance>(PollinateMonitor.FindPollinateTarget)).ToggleBehaviour(GameTags.Creatures.WantsToPollinate, (StateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsValidTarget()), (System.Action<PollinateMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.satisfied)));
    double num;
    this.satisfied.Enter((StateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.State.Callback) (smi => num = (double) this.remainingSecondsForEffect.Set(ButterflyTuning.SEARCH_COOLDOWN, smi))).ScheduleGoTo((Func<PollinateMonitor.Instance, float>) (smi => this.remainingSecondsForEffect.Get(smi)), (StateMachine.BaseState) this.lookingForPlant);
  }

  private static void FindPollinateTarget(PollinateMonitor.Instance smi)
  {
    if (smi.IsValidTarget())
      return;
    KPrefabID kprefabId = smi.def.PlantSeeker.Scan(Grid.PosToXY(smi.transform.GetPosition()), smi.navigator);
    GameObject gameObject = (UnityEngine.Object) kprefabId != (UnityEngine.Object) null ? kprefabId.gameObject : (GameObject) null;
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) smi.target))
      return;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
    {
      smi.target = (GameObject) null;
      smi.targetCell = -1;
    }
    else
    {
      smi.target = gameObject;
      smi.targetCell = Grid.PosToCell(smi.target);
    }
    smi.Trigger(-255880159);
  }

  public class Def : StateMachine.BaseDef
  {
    public int radius = 10;
    private Navigator.Scanner<KPrefabID> plantSeeker;

    public Navigator.Scanner<KPrefabID> PlantSeeker
    {
      get
      {
        if (this.plantSeeker == null)
        {
          this.plantSeeker = new Navigator.Scanner<KPrefabID>(this.radius, GameScenePartitioner.Instance.plants, new Func<KPrefabID, bool>(PollinateMonitor.Def.IsHarvestablePlant));
          this.plantSeeker.SetEarlyOutThreshold(5);
        }
        return this.plantSeeker;
      }
    }

    private static bool IsHarvestablePlant(KPrefabID plant)
    {
      if ((UnityEngine.Object) plant == (UnityEngine.Object) null || plant.HasTag(GameTags.Creatures.ReservedByCreature) || plant.HasTag((Tag) "ButterflyPlant") || !plant.HasTag(GameTags.GrowingPlant) || plant.HasTag(GameTags.FullyGrown))
        return false;
      Effects component = plant.GetComponent<Effects>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return false;
      for (int index = 0; index < PollinationMonitor.PollinationEffects.Length; ++index)
      {
        HashedString pollinationEffect = PollinationMonitor.PollinationEffects[index];
        if (component.HasEffect(pollinationEffect))
          return false;
      }
      return true;
    }
  }

  public new class Instance : 
    GameStateMachine<PollinateMonitor, PollinateMonitor.Instance, IStateMachineTarget, PollinateMonitor.Def>.GameInstance,
    IApproachableBehaviour,
    ICreatureMonitor
  {
    public GameObject target;
    public int targetCell;
    public Navigator navigator;

    public Instance(IStateMachineTarget master, PollinateMonitor.Def def)
      : base(master, def)
    {
      this.navigator = master.GetComponent<Navigator>();
    }

    public Tag Id => PollinateMonitor.ID;

    public bool IsValidTarget()
    {
      return !this.target.IsNullOrDestroyed() && this.navigator.GetNavigationCost(this.targetCell) != -1;
    }

    public GameObject GetTarget() => this.target;

    public StatusItem GetApproachStatusItem() => Db.Get().CreatureStatusItems.TravelingToPollinate;

    public StatusItem GetBehaviourStatusItem() => Db.Get().CreatureStatusItems.Pollinating;

    public void OnSuccess()
    {
      Effects component = this.target.GetComponent<Effects>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.Add(Db.Get().effects.Get("ButterflyPollinated"), true);
      this.target = (GameObject) null;
    }
  }
}
