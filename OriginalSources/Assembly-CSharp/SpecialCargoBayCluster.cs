// Decompiled with JetBrains decompiler
// Type: SpecialCargoBayCluster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SpecialCargoBayCluster : 
  GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>
{
  public const string DOOR_METER_TARGET_NAME = "fg_meter_target";
  public const string TRAPPED_CRITTER_PIVOT_SYMBOL_NAME = "critter";
  public const string LOOT_SYMBOL_NAME = "loot";
  public const string DEATH_CLOUD_ANIM_NAME = "play_cloud";
  private const string OPEN_DOOR_ANIM_NAME = "open";
  private const string CLOSE_DOOR_ANIM_NAME = "close";
  private const string OPEN_DOOR_IDLE_ANIM_NAME = "open_idle";
  private const string CLOSE_DOOR_IDLE_ANIM_NAME = "close_idle";
  public SpecialCargoBayCluster.OpenStates open;
  public SpecialCargoBayCluster.CloseStates close;
  public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.BoolParameter IsDoorOpen;
  public StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.TargetParameter Door;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.close;
    this.close.DefaultState(this.close.idle);
    this.close.closing.Target(this.Door).PlayAnim("close").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
    this.close.idle.Target(this.Door).PlayAnim("close_idle").ParamTransition<bool>((StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.Parameter<bool>) this.IsDoorOpen, this.open.opening, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsTrue).Target(this.masterTarget);
    this.close.cloud.Target(this.Door).PlayAnim("play_cloud").OnAnimQueueComplete(this.close.idle).Target(this.masterTarget);
    this.open.DefaultState(this.close.idle);
    this.open.opening.Target(this.Door).PlayAnim("open").OnAnimQueueComplete(this.open.idle).Target(this.masterTarget);
    this.open.idle.Target(this.Door).PlayAnim("open_idle").Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.DropInventory)).Enter(new StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State.Callback(SpecialCargoBayCluster.CloseDoorAutomatically)).ParamTransition<bool>((StateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.Parameter<bool>) this.IsDoorOpen, this.close.closing, GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.IsFalse).Target(this.masterTarget);
  }

  public static void CloseDoorAutomatically(SpecialCargoBayCluster.Instance smi)
  {
    smi.CloseDoorAutomatically();
  }

  public static void DropInventory(SpecialCargoBayCluster.Instance smi) => smi.DropInventory();

  public class Def : StateMachine.BaseDef
  {
    public Vector2 trappedOffset = new Vector2(0.0f, -0.3f);
  }

  public class OpenStates : 
    GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
  {
    public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State opening;
    public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;
  }

  public class CloseStates : 
    GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State
  {
    public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State closing;
    public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State idle;
    public GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.State cloud;
  }

  public new class Instance : 
    GameStateMachine<SpecialCargoBayCluster, SpecialCargoBayCluster.Instance, IStateMachineTarget, SpecialCargoBayCluster.Def>.GameInstance,
    IHexCellCollector
  {
    public MeterController doorMeter;
    private Storage critterStorage;
    private Storage sideProductStorage;
    private KBatchedAnimController buildingAnimController;
    private KBatchedAnimController doorAnimController;
    [MyCmpGet]
    private RocketModuleCluster rocketModuleCluster;

    public void PlayDeathCloud()
    {
      if (!this.IsInsideState((StateMachine.BaseState) this.sm.close.idle))
        return;
      this.GoTo((StateMachine.BaseState) this.sm.close.cloud);
    }

    public void CloseDoor() => this.sm.IsDoorOpen.Set(false, this.smi);

    public void OpenDoor() => this.sm.IsDoorOpen.Set(true, this.smi);

    public Instance(IStateMachineTarget master, SpecialCargoBayCluster.Def def)
      : base(master, def)
    {
      this.buildingAnimController = this.GetComponent<KBatchedAnimController>();
      this.doorMeter = new MeterController((KAnimControllerBase) this.buildingAnimController, "fg_meter_target", "close_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
      this.doorAnimController = this.doorMeter.meterController;
      KBatchedAnimTracker componentInChildren = this.doorAnimController.GetComponentInChildren<KBatchedAnimTracker>();
      componentInChildren.forceAlwaysAlive = true;
      componentInChildren.matchParentOffset = true;
      this.sm.Door.Set(this.doorAnimController.gameObject, this.smi, false);
      Storage[] components = this.gameObject.GetComponents<Storage>();
      this.critterStorage = components[0];
      this.sideProductStorage = components[1];
      this.Subscribe(1655598572, new System.Action<object>(this.OnLaunchConditionChanged));
    }

    public void CloseDoorAutomatically() => this.CloseDoor();

    public override void StartSM() => base.StartSM();

    private void OnLaunchConditionChanged(object obj)
    {
      if (!((UnityEngine.Object) this.rocketModuleCluster.CraftInterface != (UnityEngine.Object) null))
        return;
      Clustercraft component = this.rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.Status != Clustercraft.CraftStatus.Launching)
        return;
      this.CloseDoor();
    }

    public void DropInventory()
    {
      List<GameObject> gameObjectList1 = new List<GameObject>();
      List<GameObject> gameObjectList2 = new List<GameObject>();
      foreach (GameObject gameObject in this.critterStorage.items)
      {
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          Baggable component = gameObject.GetComponent<Baggable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.keepWrangledNextTimeRemovedFromStorage = true;
        }
      }
      Storage critterStorage = this.critterStorage;
      List<GameObject> gameObjectList3 = gameObjectList1;
      Vector3 offset1 = new Vector3();
      List<GameObject> collect_dropped_items1 = gameObjectList3;
      critterStorage.DropAll(false, false, offset1, true, collect_dropped_items1);
      Storage sideProductStorage = this.sideProductStorage;
      List<GameObject> gameObjectList4 = gameObjectList2;
      Vector3 offset2 = new Vector3();
      List<GameObject> collect_dropped_items2 = gameObjectList4;
      sideProductStorage.DropAll(false, false, offset2, true, collect_dropped_items2);
      foreach (GameObject critter in gameObjectList1)
      {
        KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
        Vector3 positionForCritter = this.GetStorePositionForCritter(critter);
        critter.transform.SetPosition(positionForCritter);
        component.SetSceneLayer(Grid.SceneLayer.Creatures);
        component.Play((HashedString) "trussed", KAnim.PlayMode.Loop);
      }
      foreach (GameObject gameObject in gameObjectList2)
      {
        KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
        gameObject.transform.SetPosition(this.GetStorePositionForDrops());
        component.SetSceneLayer(Grid.SceneLayer.Ore);
      }
    }

    public Vector3 GetCritterPositionOffet(GameObject critter)
    {
      KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
      return Vector3.zero with
      {
        x = this.def.trappedOffset.x - component.Offset.x,
        y = this.def.trappedOffset.y - component.Offset.y
      };
    }

    public Vector3 GetStorePositionForCritter(GameObject critter)
    {
      Vector3 critterPositionOffet = this.GetCritterPositionOffet(critter);
      return (Vector3) this.buildingAnimController.GetSymbolTransform((HashedString) nameof (critter), out bool _).GetColumn(3) + critterPositionOffet;
    }

    public Vector3 GetStorePositionForDrops()
    {
      return (Vector3) this.buildingAnimController.GetSymbolTransform((HashedString) "loot", out bool _).GetColumn(3);
    }

    public bool CheckIsCollecting() => false;

    public string GetProperName() => this.GetComponent<RocketModuleCluster>().GetProperName();

    public Sprite GetUISprite()
    {
      return global::Def.GetUISprite((object) this.master.gameObject.GetComponent<KPrefabID>().PrefabID()).first;
    }

    public float GetCapacity() => 1f;

    public float GetMassStored() => (float) this.critterStorage.items.Count;

    public float TimeInState() => this.timeinstate;

    public string GetCapacityBarText() => $"{this.GetMassStored()} / {this.GetCapacity()}";
  }
}
