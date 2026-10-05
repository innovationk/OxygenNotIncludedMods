// Decompiled with JetBrains decompiler
// Type: ColdBreather
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class ColdBreather : 
  StateMachineComponent<ColdBreather.StatesInstance>,
  IGameObjectEffectDescriptor
{
  [MyCmpReq]
  private WiltCondition wiltCondition;
  [MyCmpReq]
  private KAnimControllerBase animController;
  [MyCmpReq]
  private Storage storage;
  [MyCmpReq]
  private ElementConsumer elementConsumer;
  [MyCmpGet]
  private RadiationEmitter radiationEmitter;
  [MyCmpReq]
  private ReceptacleMonitor receptacleMonitor;
  private const float EXHALE_PERIOD = 1f;
  public float consumptionRate;
  public float deltaEmitTemperature = -5f;
  public Vector3 emitOffsetCell = new Vector3(0.0f, 0.0f);
  private List<GameObject> gases = new List<GameObject>();
  private Tag lastEmitTag;
  private int nextGasEmitIndex;
  private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;
  private static readonly EventSystem.IntraObjectHandler<ColdBreather> OnReplantedDelegate = new EventSystem.IntraObjectHandler<ColdBreather>((Action<ColdBreather, object>) ((component, data) => component.OnReplanted(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(ColdBreather.OnSimEmittedCallback), (object) this, nameof (ColdBreather));
    this.smi.StartSM();
  }

  protected override void OnPrefabInit()
  {
    this.elementConsumer.EnableConsumption(false);
    this.Subscribe<ColdBreather>(1309017699, ColdBreather.OnReplantedDelegate);
    base.OnPrefabInit();
  }

  private void OnReplanted(object data = null)
  {
    ReceptacleMonitor component = this.GetComponent<ReceptacleMonitor>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.GetComponent<ElementConsumer>().consumptionRate = !component.Replanted ? this.consumptionRate * 0.25f : this.consumptionRate;
    if (!((UnityEngine.Object) this.radiationEmitter != (UnityEngine.Object) null))
      return;
    this.radiationEmitter.emitRads = 480f;
    this.radiationEmitter.Refresh();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "coldbreather");
    this.simEmitCBHandle.Clear();
    if ((bool) (UnityEngine.Object) this.storage)
      this.storage.DropAll(true);
    base.OnCleanUp();
  }

  protected void DestroySelf(object callbackParam)
  {
    CreatureHelpers.DeselectCreature(this.gameObject);
    Util.KDestroyGameObject(this.gameObject);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor((string) UI.GAMEOBJECTEFFECTS.COLDBREATHER, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.COLDBREATHER)
    };
  }

  private void SetEmitting(bool emitting)
  {
    if (!((UnityEngine.Object) this.radiationEmitter != (UnityEngine.Object) null))
      return;
    this.radiationEmitter.SetEmitting(emitting);
  }

  private void Exhale()
  {
    if (this.lastEmitTag != Tag.Invalid)
      return;
    this.gases.Clear();
    this.storage.Find(GameTags.Gas, this.gases);
    if (this.nextGasEmitIndex >= this.gases.Count)
      this.nextGasEmitIndex = 0;
    while (this.nextGasEmitIndex < this.gases.Count)
    {
      PrimaryElement component = this.gases[this.nextGasEmitIndex++].GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.Mass > 0.0 && this.simEmitCBHandle.IsValid())
      {
        float num = Mathf.Max(component.Element.lowTemp + 5f, component.Temperature + this.deltaEmitTemperature);
        int cell = Grid.PosToCell(this.transform.GetPosition() + this.emitOffsetCell);
        ushort idx = component.Element.idx;
        Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
        int element_idx = (int) idx;
        double mass = (double) component.Mass;
        double temperature = (double) num;
        int diseaseIdx = (int) component.DiseaseIdx;
        int diseaseCount = component.DiseaseCount;
        int index = this.simEmitCBHandle.index;
        SimMessages.EmitMass(cell, (ushort) element_idx, (float) mass, (float) temperature, (byte) diseaseIdx, diseaseCount, index);
        this.lastEmitTag = component.Element.tag;
        break;
      }
    }
  }

  private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
  {
    ((ColdBreather) data).OnSimEmitted(info);
  }

  private void OnSimEmitted(Sim.MassEmittedCallback info)
  {
    if (info.suceeded == (byte) 1 && (bool) (UnityEngine.Object) this.storage && this.lastEmitTag.IsValid)
      this.storage.ConsumeIgnoringDisease(this.lastEmitTag, info.mass);
    this.lastEmitTag = Tag.Invalid;
  }

  public class StatesInstance(ColdBreather master) : 
    GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.GameInstance(master)
  {
  }

  public class States : 
    GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather>
  {
    public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State grow;
    public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State blocked_from_growing;
    public ColdBreather.States.AliveStates alive;
    public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State dead;
    private StatusItem statusItemCooling;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      default_state = (StateMachine.BaseState) this.grow;
      this.statusItemCooling = new StatusItem("cooling", (string) CREATURES.STATUSITEMS.COOLING.NAME, (string) CREATURES.STATUSITEMS.COOLING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State dead = this.dead;
      string name = (string) CREATURES.STATUSITEMS.DEAD.NAME;
      string tooltip = (string) CREATURES.STATUSITEMS.DEAD.TOOLTIP;
      StatusItemCategory main = Db.Get().StatusItemCategories.Main;
      HashedString render_overlay = new HashedString();
      StatusItemCategory category = main;
      dead.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category).Enter((StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State.Callback) (smi =>
      {
        GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(true);
        smi.master.Trigger(1623392196, (object) null);
        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
        UnityEngine.Object.Destroy((UnityEngine.Object) smi.master.GetComponent<KBatchedAnimController>());
        smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), (object) null);
      }));
      this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked).EventTransition(GameHashes.EntombedChanged, (GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State) this.alive, (StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooColdWarning, (GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State) this.alive, (StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).EventTransition(GameHashes.TooHotWarning, (GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State) this.alive, (StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.Transition.ConditionCallback) (smi => this.alive.ForceUpdateStatus(smi.master.gameObject))).TagTransition(GameTags.Uprooted, this.dead);
      this.grow.Enter((StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State.Callback) (smi =>
      {
        if (!smi.master.receptacleMonitor.HasReceptacle() || this.alive.ForceUpdateStatus(smi.master.gameObject))
          return;
        smi.GoTo((StateMachine.BaseState) this.blocked_from_growing);
      })).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, (GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State) this.alive);
      this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update((Action<ColdBreather.StatesInstance, float>) ((smi, dt) => smi.master.Exhale()));
      this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.Transition.ConditionCallback) (smi => smi.master.wiltCondition.IsWilting())).PlayAnim("idle", KAnim.PlayMode.Loop).ToggleMainStatusItem(this.statusItemCooling).Enter((StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State.Callback) (smi =>
      {
        smi.master.elementConsumer.EnableConsumption(true);
        smi.master.SetEmitting(true);
      })).Exit((StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State.Callback) (smi =>
      {
        smi.master.elementConsumer.EnableConsumption(false);
        smi.master.SetEmitting(false);
      }));
      this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.Transition.ConditionCallback) (smi => !smi.master.wiltCondition.IsWilting())).Enter((StateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State.Callback) (smi => smi.master.SetEmitting(false)));
    }

    public class AliveStates : 
      GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.PlantAliveSubState
    {
      public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State mature;
      public GameStateMachine<ColdBreather.States, ColdBreather.StatesInstance, ColdBreather, object>.State wilting;
    }
  }
}
