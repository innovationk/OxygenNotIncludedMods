// Decompiled with JetBrains decompiler
// Type: WarpConduitSender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WarpConduitSender : 
  StateMachineComponent<WarpConduitSender.StatesInstance>,
  ISecondaryInput
{
  [MyCmpReq]
  private Operational operational;
  public Storage gasStorage;
  public Storage liquidStorage;
  public Storage solidStorage;
  public WarpConduitReceiver receiver;
  [SerializeField]
  public ConduitPortInfo liquidPortInfo;
  private WarpConduitSender.ConduitPort liquidPort;
  [SerializeField]
  public ConduitPortInfo gasPortInfo;
  private WarpConduitSender.ConduitPort gasPort;
  [SerializeField]
  public ConduitPortInfo solidPortInfo;
  private WarpConduitSender.ConduitPort solidPort;

  private bool IsSending()
  {
    return this.smi.master.gasPort.IsOn() || this.smi.master.liquidPort.IsOn() || this.smi.master.solidPort.IsOn();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Storage[] components = this.GetComponents<Storage>();
    this.gasStorage = components[0];
    this.liquidStorage = components[1];
    this.solidStorage = components[2];
    this.gasPort = new WarpConduitSender.ConduitPort(this.gameObject, this.gasPortInfo, 1, this.gasStorage);
    this.liquidPort = new WarpConduitSender.ConduitPort(this.gameObject, this.liquidPortInfo, 2, this.liquidStorage);
    this.solidPort = new WarpConduitSender.ConduitPort(this.gameObject, this.solidPortInfo, 3, this.solidStorage);
    Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
    this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0.0f, 0.0f, -0.1f);
    this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
    this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
    this.FindPartner();
    WarpConduitStatus.UpdateWarpConduitsOperational(this.gameObject, (UnityEngine.Object) this.receiver != (UnityEngine.Object) null ? this.receiver.gameObject : (GameObject) null);
    this.smi.StartSM();
  }

  public void OnActivatedChanged(object _)
  {
    WarpConduitStatus.UpdateWarpConduitsOperational(this.gameObject, (UnityEngine.Object) this.receiver != (UnityEngine.Object) null ? this.receiver.gameObject : (GameObject) null);
  }

  private void FindPartner()
  {
    SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitReceiver");
    foreach (WarpConduitReceiver component in UnityEngine.Object.FindObjectsByType<WarpConduitReceiver>(FindObjectsSortMode.None))
    {
      if (component.GetMyWorldId() != this.GetMyWorldId())
      {
        this.receiver = component;
        break;
      }
    }
    if ((UnityEngine.Object) this.receiver == (UnityEngine.Object) null)
      Debug.LogWarning((object) "No warp conduit receiver found - maybe POI stomping or failure to spawn?");
    else
      this.receiver.SetStorage(this.gasStorage, this.liquidStorage, this.solidStorage);
  }

  protected override void OnCleanUp()
  {
    Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.inputCell, (object) this.liquidPort.networkItem, true);
    Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasPort.inputCell, (object) this.gasPort.networkItem, true);
    Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.inputCell, (object) this.solidPort.solidConsumer, true);
    base.OnCleanUp();
  }

  bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
  {
    return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
  }

  public CellOffset GetSecondaryConduitOffset(ConduitType type)
  {
    if (this.liquidPortInfo.conduitType == type)
      return this.liquidPortInfo.offset;
    if (this.gasPortInfo.conduitType == type)
      return this.gasPortInfo.offset;
    return this.solidPortInfo.conduitType == type ? this.solidPortInfo.offset : CellOffset.none;
  }

  private class ConduitPort
  {
    public ConduitPortInfo portInfo;
    public int inputCell;
    public FlowUtilityNetwork.NetworkItem networkItem;
    private ConduitConsumer conduitConsumer;
    public SolidConduitConsumer solidConsumer;
    public MeterController airlock;
    private bool open;
    private string pre;
    private string loop;
    private string pst;

    public ConduitPort(GameObject parent, ConduitPortInfo info, int number, Storage targetStorage)
    {
      this.portInfo = info;
      this.inputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
      if (this.portInfo.conduitType != ConduitType.Solid)
      {
        ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
        conduitConsumer.conduitType = this.portInfo.conduitType;
        conduitConsumer.useSecondaryInput = true;
        conduitConsumer.storage = targetStorage;
        conduitConsumer.capacityKG = targetStorage.capacityKg;
        conduitConsumer.alwaysConsume = false;
        this.conduitConsumer = conduitConsumer;
        this.conduitConsumer.keepZeroMassObject = false;
        IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
        this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.inputCell, parent);
        int inputCell = this.inputCell;
        FlowUtilityNetwork.NetworkItem networkItem = this.networkItem;
        networkManager.AddToNetworks(inputCell, (object) networkItem, true);
      }
      else
      {
        this.solidConsumer = parent.AddComponent<SolidConduitConsumer>();
        this.solidConsumer.useSecondaryInput = true;
        this.solidConsumer.storage = targetStorage;
        this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, this.inputCell, parent);
        Game.Instance.solidConduitSystem.AddToNetworks(this.inputCell, (object) this.networkItem, true);
      }
      string meter_animation = "airlock_" + number.ToString();
      string meter_target = "airlock_target_" + number.ToString();
      this.pre = $"airlock_{number.ToString()}_pre";
      this.loop = $"airlock_{number.ToString()}_loop";
      this.pst = $"airlock_{number.ToString()}_pst";
      this.airlock = new MeterController((KAnimControllerBase) parent.GetComponent<KBatchedAnimController>(), meter_target, meter_animation, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[1]
      {
        meter_target
      });
    }

    public bool IsOn()
    {
      if ((UnityEngine.Object) this.solidConsumer != (UnityEngine.Object) null)
        return this.solidConsumer.IsConsuming;
      return (UnityEngine.Object) this.conduitConsumer != (UnityEngine.Object) null && this.conduitConsumer.IsConnected && this.conduitConsumer.IsSatisfied && this.conduitConsumer.consumedLastTick;
    }

    public void Update()
    {
      bool flag = this.IsOn();
      if (flag == this.open)
        return;
      this.open = flag;
      if (this.open)
      {
        this.airlock.meterController.Play((HashedString) this.pre);
        this.airlock.meterController.Queue((HashedString) this.loop, KAnim.PlayMode.Loop);
      }
      else
        this.airlock.meterController.Play((HashedString) this.pst);
    }
  }

  public class StatesInstance(WarpConduitSender smi) : 
    GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.GameInstance(smi)
  {
  }

  public class States : 
    GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender>
  {
    public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State off;
    public WarpConduitSender.States.onStates on;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.EventHandler(GameHashes.BuildingActivated, (GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.GameEvent.Callback) ((smi, data) => smi.master.OnActivatedChanged(data)));
      this.off.PlayAnim("off").Enter((StateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State.Callback) (smi =>
      {
        smi.master.gasPort.Update();
        smi.master.liquidPort.Update();
        smi.master.solidPort.Update();
      })).EventTransition(GameHashes.OperationalChanged, (GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State) this.on, (StateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.on.DefaultState(this.on.waiting).Update((Action<WarpConduitSender.StatesInstance, float>) ((smi, dt) =>
      {
        smi.master.gasPort.Update();
        smi.master.liquidPort.Update();
        smi.master.solidPort.Update();
      }), UpdateRate.SIM_1000ms);
      this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working).Update((Action<WarpConduitSender.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsSending())
          return;
        smi.GoTo((StateMachine.BaseState) this.on.waiting);
      }), UpdateRate.SIM_1000ms).Exit((StateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State.Callback) (smi => smi.Play("working_pst")));
      this.on.waiting.QueueAnim("idle").ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal).Update((Action<WarpConduitSender.StatesInstance, float>) ((smi, dt) =>
      {
        if (!smi.master.IsSending())
          return;
        smi.GoTo((StateMachine.BaseState) this.on.working);
      }), UpdateRate.SIM_1000ms);
    }

    public class onStates : 
      GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State
    {
      public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State working;
      public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State waiting;
    }
  }
}
