// Decompiled with JetBrains decompiler
// Type: GeothermalVent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
public class GeothermalVent : 
  StateMachineComponent<GeothermalVent.StatesInstance>,
  ISim200ms,
  ISaveLoadable
{
  [MyCmpGet]
  private Operational operational;
  [MyCmpAdd]
  private ConnectionManager connectedToggler;
  [MyCmpAdd]
  private EntombVulnerable entombVulnerable;
  [MyCmpReq]
  private LogicPorts logicPorts;
  [Serialize]
  private float recentMass = 1f;
  private MeterController massMeter;
  [Serialize]
  private GeothermalVent.QuestProgress progress;
  protected GeothermalVent.EmitterInfo emitterInfo;
  [Serialize]
  protected List<GeothermalVent.ElementInfo> availableMaterial = new List<GeothermalVent.ElementInfo>();
  protected bool overpressure;
  protected int debrisEmissionCell;
  private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
  private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

  public bool IsQuestEntombed() => this.progress == GeothermalVent.QuestProgress.Entombed;

  public void SetQuestComplete()
  {
    this.progress = GeothermalVent.QuestProgress.Complete;
    this.connectedToggler.showButton = true;
    this.GetComponent<InfoDescription>().description = $"{(string) BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT}\n\n{(string) BUILDINGS.PREFABS.GEOTHERMALVENT.DESC}";
    this.Trigger(-1514841199, (object) null);
  }

  public static string GenerateName()
  {
    string replacement = "";
    for (int index = 0; index < 2; ++index)
      replacement += "0123456789"[UnityEngine.Random.Range(0, "0123456789".Length)].ToString();
    return BUILDINGS.PREFABS.GEOTHERMALVENT.NAME_FMT.Replace("{ID}", replacement);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.entombVulnerable.SetStatusItem(Db.Get().BuildingStatusItems.Entombed);
    this.GetComponent<PrimaryElement>().SetElement(SimHashes.Katairite);
    this.emitterInfo = new GeothermalVent.EmitterInfo();
    this.emitterInfo.cell = Grid.PosToCell(this.gameObject) + Grid.WidthInCells * 3;
    this.emitterInfo.element = new GeothermalVent.ElementInfo();
    this.emitterInfo.simHandle = -1;
    Components.GeothermalVents.Add(this.gameObject.GetMyWorldId(), this);
    if (this.progress == GeothermalVent.QuestProgress.Uninitialized)
      this.progress = Components.GeothermalVents.GetItems(this.gameObject.GetMyWorldId()).Count != 3 ? GeothermalVent.QuestProgress.Complete : GeothermalVent.QuestProgress.Entombed;
    if (this.progress == GeothermalVent.QuestProgress.Complete)
    {
      this.connectedToggler.showButton = true;
    }
    else
    {
      this.GetComponent<InfoDescription>().description = $"{(string) BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT}\n\n{(string) BUILDINGS.PREFABS.GEOTHERMALVENT.BLOCKED_DESC}";
      this.Trigger(-1514841199, (object) null);
    }
    this.massMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.BAROMETER_SYMBOLS);
    UserNameable component = this.GetComponent<UserNameable>();
    if (component.savedName == "" || component.savedName == (string) BUILDINGS.PREFABS.GEOTHERMALVENT.NAME)
      component.SetName(GeothermalVent.GenerateName());
    this.SimRegister();
    this.smi.StartSM();
  }

  [OnDeserialized]
  internal void OnDeserializedMethod()
  {
    bool flag = false;
    for (int index = 0; index < this.availableMaterial.Count; ++index)
    {
      GeothermalVent.ElementInfo elementInfo = this.availableMaterial[index];
      Element elementByHash = ElementLoader.FindElementByHash(elementInfo.elementHash);
      if (elementByHash == null)
      {
        elementByHash = ElementLoader.FindElementByHash(SimHashes.Steam);
        elementInfo.elementHash = SimHashes.Steam;
        elementInfo.isSolid = false;
      }
      elementInfo.elementIdx = elementByHash.idx;
      this.availableMaterial[index] = elementInfo;
    }
    if (!flag)
      return;
    Debug.LogWarning((object) "Invalid geothermal vent content in save was converted to steam on load.");
  }

  protected void SimRegister()
  {
    this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimBlockedCallback), true));
    this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimUnblockedCallback), true));
    SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(GeothermalVent.OnSimRegisteredCallback), (object) this, "GeothermalVentElementEmitter").index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
  }

  protected void OnSimBlockedCallback() => this.overpressure = true;

  protected void OnSimUnblockedCallback() => this.overpressure = false;

  protected static void OnSimRegisteredCallback(int handle, object data)
  {
    ((GeothermalVent) data).OnSimRegisteredImpl(handle);
  }

  protected void OnSimRegisteredImpl(int handle)
  {
    Debug.Assert(this.emitterInfo.simHandle == -1, (object) "?! too many handles registered");
    this.emitterInfo.simHandle = handle;
  }

  protected void SimUnregister()
  {
    if (Sim.IsValidHandle(this.emitterInfo.simHandle))
      SimMessages.RemoveElementEmitter(-1, this.emitterInfo.simHandle);
    this.emitterInfo.simHandle = -1;
  }

  protected override void OnCleanUp()
  {
    Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
    Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
    Components.GeothermalVents.Remove(this.gameObject.GetMyWorldId(), this);
    base.OnCleanUp();
  }

  protected void OnMassEmitted(ushort element, float mass)
  {
    bool flag = false;
    for (int index = 0; index < this.availableMaterial.Count; ++index)
    {
      if ((int) this.availableMaterial[index].elementIdx == (int) element)
      {
        GeothermalVent.ElementInfo elementInfo = this.availableMaterial[index];
        elementInfo.mass -= mass;
        flag |= (double) elementInfo.mass <= 0.0;
        this.availableMaterial[index] = elementInfo;
        break;
      }
    }
    if (!flag)
      return;
    this.RecomputeEmissions();
  }

  public void SpawnKeepsake()
  {
    GameObject keepsakePrefab = Assets.GetPrefab((Tag) "keepsake_geothermalplant");
    if (!((UnityEngine.Object) keepsakePrefab != (UnityEngine.Object) null))
      return;
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "pooped");
    GameScheduler.Instance.Schedule("UncorkPoopAnim", 1.5f, (Action<object>) (data => this.GetComponent<KBatchedAnimController>().Play((HashedString) "uncork")), (object) null, (SchedulerGroup) null);
    GameScheduler.Instance.Schedule("UncorkPoopFX", 2f, (Action<object>) (data => Game.Instance.SpawnFX(SpawnFXHashes.MissileExplosion, this.transform.GetPosition() + Vector3.up * 3f, 0.0f)), (object) null, (SchedulerGroup) null);
    GameScheduler.Instance.Schedule("SpawnGeothermalKeepsake", 3.75f, (Action<object>) (data =>
    {
      Vector3 position = this.transform.GetPosition() with
      {
        z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront)
      };
      GameObject gameObject = Util.KInstantiate(keepsakePrefab, position);
      gameObject.SetActive(true);
      new UpgradeFX.Instance((IStateMachineTarget) gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0.0f, -0.5f, -0.1f)).StartSM();
    }), (object) null, (SchedulerGroup) null);
  }

  public bool IsOverPressure() => this.overpressure;

  protected void RecomputeEmissions()
  {
    this.availableMaterial.Sort();
    while (this.availableMaterial.Count > 0 && (double) this.availableMaterial[this.availableMaterial.Count - 1].mass <= 0.0)
      this.availableMaterial.RemoveAt(this.availableMaterial.Count - 1);
    int num = 0;
    foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
    {
      if (!elementInfo.isSolid)
        ++num;
    }
    if (num > 0)
    {
      int index = UnityEngine.Random.Range(0, this.availableMaterial.Count);
      while (this.availableMaterial[index].isSolid)
        index = (index + 1) % this.availableMaterial.Count;
      this.emitterInfo.element = this.availableMaterial[index];
      this.emitterInfo.element.diseaseCount = (int) ((double) this.availableMaterial[index].diseaseCount * (double) this.emitterInfo.element.mass / (double) this.availableMaterial[index].mass);
    }
    else
    {
      this.emitterInfo.element.elementIdx = (ushort) 0;
      this.emitterInfo.element.mass = 0.0f;
    }
    this.emitterInfo.dirty = true;
  }

  public void addMaterial(GeothermalVent.ElementInfo info)
  {
    this.availableMaterial.Add(info);
    this.recentMass = this.MaterialAvailable();
  }

  public bool HasMaterial()
  {
    bool flag = this.availableMaterial.Count != 0;
    if (flag != this.logicPorts.GetOutputValue((HashedString) "GEOTHERMAL_VENT_STATUS_PORT") > 0)
      this.logicPorts.SendSignal((HashedString) "GEOTHERMAL_VENT_STATUS_PORT", flag ? 1 : 0);
    return flag;
  }

  public float MaterialAvailable()
  {
    float num = 0.0f;
    foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
      num += elementInfo.mass;
    return num;
  }

  public bool IsEntombed() => this.entombVulnerable.GetEntombed;

  public bool CanVent() => !this.HasMaterial() && !this.IsEntombed();

  public bool IsVentConnected()
  {
    return !((UnityEngine.Object) this.connectedToggler == (UnityEngine.Object) null) && this.connectedToggler.IsConnected;
  }

  public void EmitSolidChunk()
  {
    int num1 = 0;
    foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
    {
      if (elementInfo.isSolid && (double) elementInfo.mass > 0.0)
        ++num1;
    }
    if (num1 == 0)
      return;
    int index = UnityEngine.Random.Range(0, this.availableMaterial.Count);
    while (!this.availableMaterial[index].isSolid)
      index = (index + 1) % this.availableMaterial.Count;
    GeothermalVent.ElementInfo elementInfo1 = this.availableMaterial[index];
    if (ElementLoader.elements[(int) this.availableMaterial[index].elementIdx] == null)
      return;
    int num2 = (double) UnityEngine.Random.value >= 0.5 ? 1 : 0;
    float f = (float) ((double) GeothermalVentConfig.INITIAL_DEBRIS_ANGLE.Get() * 3.1415927410125732 / 180.0);
    Vector2 vector2 = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
    if (num2 != 0)
      vector2.x = -vector2.x;
    vector2 = vector2.normalized;
    Vector3 vector3 = (Vector3) (vector2 * GeothermalVentConfig.INITIAL_DEBRIS_VELOCIOTY.Get());
    float num3 = Math.Min(GeothermalVentConfig.DEBRIS_MASS_KG.Get(), elementInfo1.mass);
    if ((double) elementInfo1.mass - (double) num3 < (double) GeothermalVentConfig.DEBRIS_MASS_KG.min)
      num3 = elementInfo1.mass;
    if ((double) num3 < 0.0099999997764825821)
    {
      elementInfo1.mass = 0.0f;
      this.availableMaterial[index] = elementInfo1;
    }
    else
    {
      int num4 = (int) ((double) elementInfo1.diseaseCount * (double) num3 / (double) elementInfo1.mass);
      Vector3 pos = Grid.CellToPos(this.emitterInfo.cell, CellAlignment.Top, Grid.SceneLayer.BuildingFront);
      Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, pos, 0.0f);
      GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) MiniCometConfig.ID), pos);
      PrimaryElement component1 = gameObject.GetComponent<PrimaryElement>();
      component1.SetElement(ElementLoader.elements[(int) elementInfo1.elementIdx].id);
      component1.Mass = num3;
      component1.Temperature = elementInfo1.temperature;
      MiniComet component2 = gameObject.GetComponent<MiniComet>();
      component2.diseaseIdx = elementInfo1.diseaseIdx;
      component2.addDiseaseCount = num4;
      gameObject.SetActive(true);
      elementInfo1.diseaseCount -= num4;
      elementInfo1.mass -= num3;
      this.availableMaterial[index] = elementInfo1;
    }
  }

  public void Sim200ms(float dt)
  {
    if ((double) dt <= 0.0)
      return;
    this.unsafeSim200ms(dt);
  }

  private unsafe void unsafeSim200ms(float dt)
  {
    if (Sim.IsValidHandle(this.emitterInfo.simHandle))
    {
      if (this.emitterInfo.dirty)
      {
        SimMessages.ModifyElementEmitter(this.emitterInfo.simHandle, this.emitterInfo.cell, 1, ElementLoader.elements[(int) this.emitterInfo.element.elementIdx].id, 0.2f, Math.Min(3f, this.emitterInfo.element.mass), this.emitterInfo.element.temperature, 120f, this.emitterInfo.element.diseaseIdx, this.emitterInfo.element.diseaseCount);
        this.emitterInfo.dirty = false;
      }
      Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[Sim.GetHandleIndex(this.emitterInfo.simHandle)];
      if ((double) emittedMassInfo.mass > 0.0)
        this.OnMassEmitted(emittedMassInfo.elemIdx, emittedMassInfo.mass);
    }
    this.massMeter.SetPositionPercent(this.MaterialAvailable() / this.recentMass);
  }

  protected static bool HasProblem(GeothermalVent.StatesInstance smi)
  {
    return smi.master.IsEntombed() || smi.master.IsOverPressure();
  }

  private enum QuestProgress
  {
    Uninitialized,
    Entombed,
    Complete,
  }

  public struct ElementInfo : IComparable
  {
    public bool isSolid;
    public SimHashes elementHash;
    public ushort elementIdx;
    public float mass;
    public float temperature;
    public byte diseaseIdx;
    public int diseaseCount;

    public int CompareTo(object obj)
    {
      return -this.mass.CompareTo(((GeothermalVent.ElementInfo) obj).mass);
    }
  }

  public struct EmitterInfo
  {
    public int simHandle;
    public int cell;
    public GeothermalVent.ElementInfo element;
    public bool dirty;
  }

  public class States : 
    GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent>
  {
    public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State questEntombed;
    public GeothermalVent.States.OnlineStates online;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.root;
      this.root.EnterTransition(this.questEntombed, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => smi.master.IsQuestEntombed())).EnterTransition((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !smi.master.IsQuestEntombed()));
      this.questEntombed.PlayAnim("pooped").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentQuestBlockage, (Func<GeothermalVent.StatesInstance, object>) (smi => (object) smi.master)).Transition((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => smi.master.progress == GeothermalVent.QuestProgress.Complete));
      this.online.PlayAnim("on", KAnim.PlayMode.Once).defaultState = (StateMachine.BaseState) this.online.identify;
      this.online.identify.EnterTransition((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem)).EnterTransition((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online.active, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !GeothermalVent.HasProblem(smi) && smi.master.HasMaterial())).EnterTransition(this.online.ready, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && smi.master.IsVentConnected())).EnterTransition(this.online.disconnected, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && !smi.master.IsVentConnected()));
      this.online.active.defaultState = (StateMachine.BaseState) this.online.active.preVent;
      this.online.active.preVent.PlayAnim("working_pre").OnAnimQueueComplete((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online.active.loopVent);
      this.online.active.loopVent.Enter((StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State.Callback) (smi => smi.master.RecomputeEmissions())).Exit((StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State.Callback) (smi => smi.master.RecomputeEmissions())).Transition(this.online.active.postVent, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !smi.master.HasMaterial())).Transition(this.online.inactive.identify, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsVenting, (Func<GeothermalVent.StatesInstance, object>) (smi => (object) smi.master)).Update((Action<GeothermalVent.StatesInstance, float>) ((smi, dt) =>
      {
        if ((double) dt <= 0.0)
          return;
        smi.master.RecomputeEmissions();
      }), UpdateRate.SIM_4000ms).defaultState = (StateMachine.BaseState) this.online.active.loopVent.start;
      this.online.active.loopVent.start.PlayAnim("working1").OnAnimQueueComplete(this.online.active.loopVent.finish);
      this.online.active.loopVent.finish.Enter((StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State.Callback) (smi => smi.master.EmitSolidChunk())).PlayAnim("working2").OnAnimQueueComplete(this.online.active.loopVent.start);
      this.online.active.postVent.QueueAnim("working_pst").OnAnimQueueComplete(this.online.ready);
      this.online.ready.PlayAnim("on", KAnim.PlayMode.Once).Transition((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online.active, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => smi.master.HasMaterial())).Transition((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem)).Transition(this.online.disconnected, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !smi.master.IsVentConnected())).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsReady, (Func<GeothermalVent.StatesInstance, object>) (smi => (object) smi.master));
      this.online.disconnected.PlayAnim("on", KAnim.PlayMode.Once).Transition((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online.active, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => smi.master.HasMaterial())).Transition((GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State) this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem)).Transition(this.online.ready, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => smi.master.IsVentConnected())).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsDisconnected, (Func<GeothermalVent.StatesInstance, object>) (smi => (object) smi.master));
      this.online.inactive.PlayAnim("over_pressure", KAnim.PlayMode.Once).Transition(this.online.identify, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !GeothermalVent.HasProblem(smi))).defaultState = (StateMachine.BaseState) this.online.inactive.identify;
      this.online.inactive.identify.EnterTransition(this.online.inactive.entombed, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => smi.master.IsEntombed())).EnterTransition(this.online.inactive.overpressure, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => smi.master.IsOverPressure()));
      this.online.inactive.entombed.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Entombed).Transition(this.online.inactive.identify, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !smi.master.IsEntombed()));
      this.online.inactive.overpressure.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsOverpressure).EnterTransition(this.online.inactive.identify, (StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback) (smi => !smi.master.IsOverPressure()));
    }

    public class ActiveStates : 
      GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
    {
      public GeothermalVent.States.ActiveStates.LoopStates loopVent;
      public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State preVent;
      public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State postVent;

      public class LoopStates : 
        GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
      {
        public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State start;
        public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State finish;
      }
    }

    public class ProblemStates : 
      GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
    {
      public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;
      public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State entombed;
      public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State overpressure;
    }

    public class OnlineStates : 
      GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
    {
      public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;
      public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State ready;
      public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State disconnected;
      public GeothermalVent.States.ActiveStates active;
      public GeothermalVent.States.ProblemStates inactive;
    }
  }

  public class StatesInstance(GeothermalVent smi) : 
    GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.GameInstance(smi)
  {
  }
}
