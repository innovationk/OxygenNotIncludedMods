// Decompiled with JetBrains decompiler
// Type: Repairable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Repairable")]
public class Repairable : Workable
{
  public float expectedRepairTime = -1f;
  [MyCmpGet]
  private BuildingHP hp;
  private Repairable.SMInstance smi;
  private Storage storageProxy;
  [Serialize]
  private byte[] storedData;
  private float timeSpentRepairing;
  private static readonly Operational.Flag repairedFlag = new Operational.Flag("repaired", Operational.Flag.Type.Functional);
  private static readonly EventSystem.IntraObjectHandler<Repairable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Repairable>((Action<Repairable, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
    this.Subscribe<Repairable>(493375141, Repairable.OnRefreshUserMenuDelegate);
    this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.showProgressBar = false;
    this.faceTargetWhenWorking = true;
    this.multitoolContext = (HashedString) "build";
    this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
    this.workingPstComplete = (HashedString[]) null;
    this.workingPstFailed = (HashedString[]) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new Repairable.SMInstance(this);
    this.smi.StartSM();
    this.workTime = float.PositiveInfinity;
    this.workTimeRemaining = float.PositiveInfinity;
  }

  private void OnProxyStorageChanged(object data) => this.Trigger(-1697596308, data);

  protected override void OnLoadLevel()
  {
    this.smi = (Repairable.SMInstance) null;
    base.OnLoadLevel();
  }

  protected override void OnCleanUp()
  {
    if (this.smi != null)
      this.smi.StopSM("Destroy Repairable");
    base.OnCleanUp();
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || this.smi == null)
      return;
    if (this.smi.GetCurrentState() == this.smi.sm.forbidden)
      Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", (string) STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.NAME, new System.Action(this.AllowRepair), tooltipText: (string) STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.TOOLTIP), 0.5f);
    else
      Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", (string) STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.NAME, new System.Action(this.CancelRepair), tooltipText: (string) STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.TOOLTIP), 0.5f);
  }

  private void AllowRepair()
  {
    if (DebugHandler.InstantBuildMode)
    {
      this.hp.Repair(this.hp.MaxHitPoints);
      this.OnCompleteWork((WorkerBase) null);
    }
    this.smi.sm.allow.Trigger(this.smi);
    this.OnRefreshUserMenu((object) null);
  }

  public void CancelRepair()
  {
    if (this.smi != null)
      this.smi.sm.forbid.Trigger(this.smi);
    this.OnRefreshUserMenu((object) null);
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    Operational component = this.GetComponent<Operational>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.SetFlag(Repairable.repairedFlag, false);
    this.smi.sm.worker.Set((KMonoBehaviour) worker, this.smi);
    this.timeSpentRepairing = 0.0f;
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    float num1 = Mathf.Sqrt(this.GetComponent<PrimaryElement>().Mass);
    float num2 = (float) (((double) this.expectedRepairTime < 0.0 ? (double) num1 : (double) this.expectedRepairTime) * 0.10000000149011612);
    if ((double) this.timeSpentRepairing >= (double) num2)
    {
      this.timeSpentRepairing -= num2;
      int num3 = 0;
      if ((UnityEngine.Object) worker != (UnityEngine.Object) null)
        num3 = (int) Db.Get().Attributes.Machinery.Lookup((Component) worker).GetTotalValue();
      this.hp.Repair(Mathf.CeilToInt((float) (10 + Math.Max(0, num3 * 10)) * 0.1f));
      if (this.hp.HitPoints >= this.hp.MaxHitPoints)
        return true;
    }
    this.timeSpentRepairing += dt;
    return false;
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    Operational component = this.GetComponent<Operational>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetFlag(Repairable.repairedFlag, true);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    Operational component = this.GetComponent<Operational>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetFlag(Repairable.repairedFlag, true);
  }

  public void CreateStorageProxy()
  {
    if (!((UnityEngine.Object) this.storageProxy == (UnityEngine.Object) null))
      return;
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) RepairableStorageProxy.ID), this.transform.gameObject);
    gameObject.transform.SetLocalPosition(Vector3.zero);
    this.storageProxy = gameObject.GetComponent<Storage>();
    this.storageProxy.prioritizable = this.transform.GetComponent<Prioritizable>();
    this.storageProxy.prioritizable.AddRef();
    gameObject.GetComponent<KSelectable>().entityName = this.transform.gameObject.GetProperName();
    gameObject.SetActive(true);
  }

  [System.Runtime.Serialization.OnSerializing]
  private void OnSerializing()
  {
    this.storedData = (byte[]) null;
    if (!((UnityEngine.Object) this.storageProxy != (UnityEngine.Object) null) || this.storageProxy.IsEmpty())
      return;
    using (MemoryStream output = new MemoryStream())
    {
      using (BinaryWriter writer = new BinaryWriter((Stream) output))
        this.storageProxy.Serialize(writer);
      this.storedData = output.ToArray();
    }
  }

  [System.Runtime.Serialization.OnSerialized]
  private void OnSerialized() => this.storedData = (byte[]) null;

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (this.storedData == null)
      return;
    FastReader fastReader = new FastReader(this.storedData);
    this.CreateStorageProxy();
    this.storageProxy.Deserialize((IReader) fastReader);
    this.storedData = (byte[]) null;
  }

  public class SMInstance(Repairable smi) : 
    GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.GameInstance(smi)
  {
    private const float REQUIRED_MASS_SCALE = 0.1f;

    public bool HasRequiredMass()
    {
      PrimaryElement component = this.GetComponent<PrimaryElement>();
      float num = component.Mass * 0.1f;
      PrimaryElement primaryElement = this.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
      return (UnityEngine.Object) primaryElement != (UnityEngine.Object) null && (double) primaryElement.Mass >= (double) num;
    }

    public KeyValuePair<Tag, float> GetRequiredMass()
    {
      PrimaryElement component = this.GetComponent<PrimaryElement>();
      float num1 = component.Mass * 0.1f;
      PrimaryElement primaryElement = this.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
      float num2 = (UnityEngine.Object) primaryElement != (UnityEngine.Object) null ? Math.Max(0.0f, num1 - primaryElement.Mass) : num1;
      return new KeyValuePair<Tag, float>(component.Element.tag, num2);
    }

    public void ConsumeRepairMaterials()
    {
      this.smi.master.storageProxy.ConsumeAllIgnoringDisease();
    }

    public void DestroyStorageProxy()
    {
      if (!((UnityEngine.Object) this.smi.master.storageProxy != (UnityEngine.Object) null))
        return;
      this.smi.master.transform.GetComponent<Prioritizable>().RemoveRef();
      List<GameObject> gameObjectList1 = new List<GameObject>();
      Storage storageProxy = this.smi.master.storageProxy;
      List<GameObject> gameObjectList2 = gameObjectList1;
      Vector3 offset = new Vector3();
      List<GameObject> collect_dropped_items = gameObjectList2;
      storageProxy.DropAll(false, false, offset, true, collect_dropped_items);
      GameObject gameObject = this.smi.sm.worker.Get(this.smi);
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        foreach (GameObject go in gameObjectList1)
          go.Trigger(580035959, (object) gameObject.GetComponent<WorkerBase>());
      }
      this.smi.sm.worker.Set((KMonoBehaviour) null, this.smi);
      Util.KDestroyGameObject(this.smi.master.storageProxy.gameObject);
    }

    public bool NeedsRepairs() => this.smi.master.GetComponent<BuildingHP>().NeedsRepairs;
  }

  public class States : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable>
  {
    public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal allow;
    public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal forbid;
    public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State forbidden;
    public Repairable.States.AllowedState allowed;
    public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repaired;
    public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.TargetParameter worker;
    public static readonly Chore.Precondition IsNotBeingAttacked = new Chore.Precondition()
    {
      id = nameof (IsNotBeingAttacked),
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        bool flag = true;
        if (data != null)
          flag = (UnityEngine.Object) ((Workable) data).worker == (UnityEngine.Object) null;
        return flag;
      })
    };
    public static readonly Chore.Precondition IsNotAngry = new Chore.Precondition()
    {
      id = nameof (IsNotAngry),
      description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_ANGRY,
      fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
      {
        Traits traits = context.consumerState.traits;
        AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject);
        return !((UnityEngine.Object) traits != (UnityEngine.Object) null) || amountInstance == null || (double) amountInstance.value < (double) TUNING.STRESS.ACTING_OUT_RESET || !traits.HasTrait("Aggressive");
      })
    };

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.repaired;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.forbidden.OnSignal(this.allow, this.repaired);
      this.allowed.Enter((StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State.Callback) (smi => smi.master.CreateStorageProxy())).DefaultState(this.allowed.needMass).EventHandler(GameHashes.BuildingFullyRepaired, (StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State.Callback) (smi => smi.ConsumeRepairMaterials())).EventTransition(GameHashes.BuildingFullyRepaired, this.repaired).OnSignal(this.forbid, this.forbidden).Exit((StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State.Callback) (smi => smi.DestroyStorageProxy()));
      this.allowed.needMass.Enter((StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State.Callback) (smi => Prioritizable.AddRef(smi.master.storageProxy.transform.parent.gameObject))).Exit((StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State.Callback) (smi =>
      {
        if (smi.isMasterNull || !((UnityEngine.Object) smi.master.storageProxy != (UnityEngine.Object) null))
          return;
        Prioritizable.RemoveRef(smi.master.storageProxy.transform.parent.gameObject);
      })).EventTransition(GameHashes.OnStorageChange, this.allowed.repairable, (StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Transition.ConditionCallback) (smi => smi.HasRequiredMass())).ToggleChore(new Func<Repairable.SMInstance, Chore>(this.CreateFetchChore), this.allowed.repairable, this.allowed.needMass).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForRepairMaterials, (Func<Repairable.SMInstance, object>) (smi => (object) smi.GetRequiredMass()));
      this.allowed.repairable.ToggleRecurringChore(new Func<Repairable.SMInstance, Chore>(this.CreateRepairChore)).ToggleStatusItem(Db.Get().BuildingStatusItems.PendingRepair);
      this.repaired.EventTransition(GameHashes.BuildingReceivedDamage, (GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State) this.allowed, (StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Transition.ConditionCallback) (smi => smi.NeedsRepairs())).OnSignal(this.allow, (GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State) this.allowed).OnSignal(this.forbid, this.forbidden);
    }

    private Chore CreateFetchChore(Repairable.SMInstance smi)
    {
      PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
      PrimaryElement primaryElement = smi.master.storageProxy.FindPrimaryElement(component.ElementID);
      float amount = (float) ((double) component.Mass * 0.10000000149011612 - ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null ? (double) primaryElement.Mass : 0.0));
      HashSet<Tag> tags = new HashSet<Tag>()
      {
        GameTagExtensions.Create(component.ElementID)
      };
      return (Chore) new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storageProxy, amount, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, operational_requirement: Operational.State.None);
    }

    private Chore CreateRepairChore(Repairable.SMInstance smi)
    {
      WorkChore<Repairable> repairChore = new WorkChore<Repairable>(Db.Get().ChoreTypes.Repair, (IStateMachineTarget) smi.master, only_when_operational: false, ignore_building_assignment: true);
      Deconstructable component1 = smi.master.GetComponent<Deconstructable>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        repairChore.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, (object) component1);
      Breakable component2 = smi.master.GetComponent<Breakable>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        repairChore.AddPrecondition(Repairable.States.IsNotBeingAttacked, (object) component2);
      repairChore.AddPrecondition(Repairable.States.IsNotAngry, (object) null);
      return (Chore) repairChore;
    }

    public class AllowedState : 
      GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State
    {
      public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State needMass;
      public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repairable;
    }
  }
}
