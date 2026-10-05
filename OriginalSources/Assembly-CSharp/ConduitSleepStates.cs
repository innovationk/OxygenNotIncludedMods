// Decompiled with JetBrains decompiler
// Type: ConduitSleepStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class ConduitSleepStates : 
  GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>
{
  public ConduitSleepStates.DrowsyStates drowsy;
  public ConduitSleepStates.HasConnectorStates connector;
  public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State behaviourcomplete;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.connector.moveToSleepLocation;
    this.root.EventTransition(GameHashes.NewDay, (Func<ConduitSleepStates.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), this.behaviourcomplete).Exit(new StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback(ConduitSleepStates.CleanUp));
    GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State moveToSleepLocation = this.connector.moveToSleepLocation;
    string name1 = (string) CREATURES.STATUSITEMS.DROWSY.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.DROWSY.TOOLTIP;
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    moveToSleepLocation.ToggleStatusItem(name1, tooltip1, render_overlay: render_overlay1, category: category1).MoveTo((Func<ConduitSleepStates.Instance, int>) (smi =>
    {
      ConduitSleepMonitor.Instance smi1 = smi.GetSMI<ConduitSleepMonitor.Instance>();
      return smi1.sm.targetSleepCell.Get(smi1);
    }), (GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State) this.drowsy, this.behaviourcomplete);
    ConduitSleepStates.DrowsyStates drowsy = this.drowsy;
    string name2 = (string) CREATURES.STATUSITEMS.DROWSY.NAME;
    string tooltip2 = (string) CREATURES.STATUSITEMS.DROWSY.TOOLTIP;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    drowsy.ToggleStatusItem(name2, tooltip2, render_overlay: render_overlay2, category: category2).Enter((StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback) (smi => smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling))).Enter((StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback) (smi =>
    {
      if (!GameClock.Instance.IsNighttime())
        return;
      smi.GoTo((StateMachine.BaseState) this.connector.sleep);
    })).DefaultState(this.drowsy.loop);
    this.drowsy.loop.PlayAnim("drowsy_pre").QueueAnim("drowsy_loop", true).EventTransition(GameHashes.Nighttime, (Func<ConduitSleepStates.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), this.drowsy.pst, (StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.Transition.ConditionCallback) (smi => GameClock.Instance.IsNighttime()));
    this.drowsy.pst.PlayAnim("drowsy_pst").OnAnimQueueComplete((GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State) this.connector.sleep);
    ConduitSleepStates.SleepStates sleep = this.connector.sleep;
    string name3 = (string) CREATURES.STATUSITEMS.SLEEPING.NAME;
    string tooltip3 = (string) CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
    StatusItemCategory main3 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay3 = new HashedString();
    StatusItemCategory category3 = main3;
    sleep.ToggleStatusItem(name3, tooltip3, render_overlay: render_overlay3, category: category3).Enter((StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback) (smi =>
    {
      if (!smi.staterpillar.IsConnectorBuildingSpawned())
      {
        smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
      }
      else
      {
        smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
        smi.staterpillar.EnableConnector();
        if (smi.staterpillar.IsConnected())
          smi.GoTo((StateMachine.BaseState) this.connector.sleep.connected);
        else
          smi.GoTo((StateMachine.BaseState) this.connector.sleep.noConnection);
      }
    }));
    this.connector.sleep.connected.Enter((StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback) (smi => smi.animController.SetSceneLayer(ConduitSleepStates.GetSleepingLayer(smi)))).Exit((StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback) (smi => smi.animController.SetSceneLayer(Grid.SceneLayer.Creatures))).EventTransition(GameHashes.NewDay, (Func<ConduitSleepStates.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), this.connector.connectedWake).Transition(this.connector.sleep.noConnection, (StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.Transition.ConditionCallback) (smi => !smi.staterpillar.IsConnected())).PlayAnim("sleep_charging_pre").QueueAnim("sleep_charging_loop", true).Update(new System.Action<ConduitSleepStates.Instance, float>(ConduitSleepStates.UpdateGulpSymbol), UpdateRate.SIM_1000ms).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.GameEvent.Callback(ConduitSleepStates.OnStorageChanged));
    this.connector.sleep.noConnection.PlayAnim("sleep_pre").QueueAnim("sleep_loop", true).ToggleStatusItem(new Func<ConduitSleepStates.Instance, StatusItem>(ConduitSleepStates.GetStatusItem)).EventTransition(GameHashes.NewDay, (Func<ConduitSleepStates.Instance, KMonoBehaviour>) (smi => (KMonoBehaviour) GameClock.Instance), this.connector.noConnectionWake).Transition(this.connector.sleep.connected, (StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.Transition.ConditionCallback) (smi => smi.staterpillar.IsConnected()));
    this.connector.connectedWake.QueueAnim("sleep_charging_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.connector.noConnectionWake.QueueAnim("sleep_pst").OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsConduitConnection);
  }

  private static Grid.SceneLayer GetSleepingLayer(ConduitSleepStates.Instance smi)
  {
    Grid.SceneLayer sleepingLayer;
    switch (smi.staterpillar.conduitLayer)
    {
      case ObjectLayer.GasConduit:
        sleepingLayer = Grid.SceneLayer.Gas;
        break;
      case ObjectLayer.LiquidConduit:
        sleepingLayer = Grid.SceneLayer.GasConduitBridges;
        break;
      case ObjectLayer.Wire:
        sleepingLayer = Grid.SceneLayer.SolidConduitBridges;
        break;
      default:
        sleepingLayer = Grid.SceneLayer.SolidConduitBridges;
        break;
    }
    return sleepingLayer;
  }

  private static StatusItem GetStatusItem(ConduitSleepStates.Instance smi)
  {
    StatusItem statusItem;
    switch (smi.staterpillar.conduitLayer)
    {
      case ObjectLayer.GasConduit:
        statusItem = Db.Get().BuildingStatusItems.NeedGasOut;
        break;
      case ObjectLayer.LiquidConduit:
        statusItem = Db.Get().BuildingStatusItems.NeedLiquidOut;
        break;
      case ObjectLayer.Wire:
        statusItem = Db.Get().BuildingStatusItems.NoWireConnected;
        break;
      default:
        statusItem = Db.Get().BuildingStatusItems.Normal;
        break;
    }
    return statusItem;
  }

  private static void OnStorageChanged(ConduitSleepStates.Instance smi, object obj)
  {
    GameObject gameObject = obj as GameObject;
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    smi.amountDeposited += gameObject.GetComponent<PrimaryElement>().Mass;
  }

  private static void UpdateGulpSymbol(ConduitSleepStates.Instance smi, float dt)
  {
    smi.SetGulpSymbolVisibility((double) smi.amountDeposited > 0.0);
    smi.amountDeposited = 0.0f;
  }

  private static void CleanUp(ConduitSleepStates.Instance smi)
  {
    ConduitSleepMonitor.Instance smi1 = smi.GetSMI<ConduitSleepMonitor.Instance>();
    smi1?.sm.targetSleepCell.Set(Grid.InvalidCell, smi1);
    smi.staterpillar.DestroyOrphanedConnectorBuilding();
  }

  public class Def : StateMachine.BaseDef
  {
    public HashedString gulpSymbol = (HashedString) "gulp";
  }

  public new class Instance : 
    GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.GameInstance
  {
    [MyCmpReq]
    public KBatchedAnimController animController;
    [MyCmpReq]
    public Staterpillar staterpillar;
    [MyCmpAdd]
    private LoopingSounds loopingSounds;
    public bool gulpSymbolVisible;
    public float amountDeposited;

    public Instance(Chore<ConduitSleepStates.Instance> chore, ConduitSleepStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsConduitConnection);
    }

    public void SetGulpSymbolVisibility(bool state)
    {
      string sound = GlobalAssets.GetSound("PlugSlug_Charging_Gulp_LP");
      if (this.gulpSymbolVisible == state)
        return;
      this.gulpSymbolVisible = state;
      this.animController.SetSymbolVisiblity((KAnimHashedString) this.def.gulpSymbol, state);
      if (state)
        this.loopingSounds.StartSound(sound);
      else
        this.loopingSounds.StopSound(sound);
    }
  }

  public class SleepStates : 
    GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
  {
    public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State connected;
    public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State noConnection;
  }

  public class DrowsyStates : 
    GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
  {
    public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State loop;
    public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State pst;
  }

  public class HasConnectorStates : 
    GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
  {
    public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State moveToSleepLocation;
    public ConduitSleepStates.SleepStates sleep;
    public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State noConnectionWake;
    public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State connectedWake;
  }
}
