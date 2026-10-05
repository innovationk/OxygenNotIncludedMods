// Decompiled with JetBrains decompiler
// Type: Telepad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Telepad : StateMachineComponent<Telepad.StatesInstance>
{
  [MyCmpReq]
  private KSelectable selectable;
  private MeterController meter;
  private const float MAX_IMMIGRATION_TIME = 120f;
  private const int NUM_METER_NOTCHES = 8;
  private List<MinionStartingStats> minionStats;
  public float startingSkillPoints;
  [Serialize]
  private List<Ref<MinionIdentity>> aNewHopeEvents = new List<Ref<MinionIdentity>>();
  [Serialize]
  private List<Ref<MinionIdentity>> extraPowerBanksEvents = new List<Ref<MinionIdentity>>();
  public static readonly HashedString[] PortalBirthAnim = new HashedString[1]
  {
    (HashedString) "portalbirth"
  };

  public void AddNewBaseMinion(GameObject minion, bool extra_power_banks)
  {
    Ref<MinionIdentity> @ref = new Ref<MinionIdentity>(minion.GetComponent<MinionIdentity>());
    this.aNewHopeEvents.Add(@ref);
    if (!extra_power_banks)
      return;
    this.extraPowerBanksEvents.Add(@ref);
  }

  public void ScheduleNewBaseEvents()
  {
    this.aNewHopeEvents.RemoveAll((Predicate<Ref<MinionIdentity>>) (entry => entry == null || (UnityEngine.Object) entry.Get() == (UnityEngine.Object) null));
    this.extraPowerBanksEvents.RemoveAll((Predicate<Ref<MinionIdentity>>) (entry => entry == null || (UnityEngine.Object) entry.Get() == (UnityEngine.Object) null));
    Effect a_new_hope = Db.Get().effects.Get("AnewHope");
    for (int index = 0; index < this.aNewHopeEvents.Count; ++index)
    {
      GameObject gameObject = this.aNewHopeEvents[index].Get().gameObject;
      GameScheduler.Instance.Schedule("ANewHope", (float) (3.0 + 0.5 * (double) index), (Action<object>) (m =>
      {
        GameObject go = m as GameObject;
        if ((UnityEngine.Object) go == (UnityEngine.Object) null)
          return;
        this.RemoveFromEvents(this.aNewHopeEvents, go);
        go.GetComponent<Effects>().Add(a_new_hope, true);
      }), (object) gameObject, (SchedulerGroup) null);
    }
    for (int index = 0; index < this.extraPowerBanksEvents.Count; ++index)
    {
      GameObject gameObject = this.extraPowerBanksEvents[index].Get().gameObject;
      GameScheduler.Instance.Schedule("ExtraPowerBanks", (float) (3.0 + 4.5 * (double) index), (Action<object>) (m =>
      {
        GameObject go = m as GameObject;
        if ((UnityEngine.Object) go == (UnityEngine.Object) null)
          return;
        this.RemoveFromEvents(this.extraPowerBanksEvents, go);
        GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id).Trigger(1982288670);
      }), (object) gameObject, (SchedulerGroup) null);
    }
  }

  private void RemoveFromEvents(List<Ref<MinionIdentity>> listToRemove, GameObject go)
  {
    for (int index = listToRemove.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) listToRemove[index].Get() != (UnityEngine.Object) null && (UnityEngine.Object) listToRemove[index].Get() == (UnityEngine.Object) go.GetComponent<MinionIdentity>())
      {
        listToRemove.RemoveAt(index);
        break;
      }
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetComponent<Deconstructable>().allowDeconstruction = false;
    int x = 0;
    int y = 0;
    Grid.CellToXY(Grid.PosToCell((KMonoBehaviour) this), out x, out y);
    if (x != 0)
      return;
    Debug.LogError((object) $"Headquarters spawned at: ({x.ToString()},{y.ToString()})");
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Telepads.Add(this);
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[4]
    {
      "meter_target",
      "meter_fill",
      "meter_frame",
      "meter_OL"
    });
    this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
    this.smi.StartSM();
    this.ScheduleNewBaseEvents();
  }

  protected override void OnCleanUp()
  {
    Components.Telepads.Remove(this);
    base.OnCleanUp();
  }

  public void Update()
  {
    if (this.smi.IsColonyLost())
      return;
    if (Immigration.Instance.ImmigrantsAvailable && this.GetComponent<Operational>().IsOperational)
    {
      this.smi.sm.openPortal.Trigger(this.smi);
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NewDuplicantsAvailable, (object) this);
    }
    else
    {
      this.smi.sm.closePortal.Trigger(this.smi);
      this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, (object) this);
    }
    if ((double) this.GetTimeRemaining() >= -120.0)
      return;
    Messenger.Instance.QueueMessage((Message) new DuplicantsLeftMessage());
    Immigration.Instance.EndImmigration();
  }

  public void RejectAll()
  {
    Immigration.Instance.EndImmigration();
    this.smi.sm.closePortal.Trigger(this.smi);
  }

  public void OnAcceptDelivery(ITelepadDeliverable delivery)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    Immigration.Instance.EndImmigration();
    GameObject context = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
    MinionIdentity component1 = context.GetComponent<MinionIdentity>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      ReportManager.Instance.ReportValueWithGameObjectContext(ReportManager.ReportType.PersonalTime, GameClock.Instance.GetTimeSinceStartOfReport(), context, string.Format((string) UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, (object) DUPLICANTS.CHORES.NOT_EXISTING_TASK));
      foreach (Component worldItem in Components.LiveMinionIdentities.GetWorldItems(this.gameObject.GetComponent<KSelectable>().GetMyWorldId()))
        worldItem.GetComponent<Effects>().Add("NewCrewArrival", true);
      MinionResume component2 = component1.GetComponent<MinionResume>();
      for (int index = 0; (double) index < (double) this.startingSkillPoints; ++index)
        component2.ForceAddSkillPoint();
      if (component1.HasTag(GameTags.Minions.Models.Bionic))
        GameScheduler.Instance.Schedule("BonusBatteryDelivery", 5f, (Action<object>) (data => this.Trigger(1982288670, (object) null)), (object) null, (SchedulerGroup) null);
    }
    this.smi.sm.closePortal.Trigger(this.smi);
  }

  public float GetTimeRemaining() => Immigration.Instance.GetTimeRemaining();

  public class StatesInstance(Telepad master) : 
    GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.GameInstance(master)
  {
    public bool IsColonyLost()
    {
      return (UnityEngine.Object) GameFlowManager.Instance != (UnityEngine.Object) null && GameFlowManager.Instance.IsGameOver();
    }

    public void UpdateMeter()
    {
      this.master.meter.SetPositionPercent(Mathf.Clamp01((float) (1.0 - (double) Immigration.Instance.GetTimeRemaining() / (double) Immigration.Instance.GetTotalWaitTime())));
    }

    public IEnumerator SpawnExtraPowerBanks()
    {
      int cellTarget = Grid.OffsetCell(Grid.PosToCell(this.gameObject), 1, 2);
      int count = 5;
      for (int i = 0; i < count; ++i)
      {
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, (string) MISC.POPFX.EXTRA_POWERBANKS_BIONIC, this.gameObject.transform, new Vector3(0.0f, 0.5f, 0.0f));
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner"));
        GameObject go = Util.KInstantiate(Assets.GetPrefab((Tag) "DisposableElectrobank_RawMetal"), Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) - Vector3.right / 2f);
        go.SetActive(true);
        Vector2 initial_velocity = new Vector2((float) ((5.0 * ((double) i / 5.0) - 2.5) / 2.0), 2f);
        if (GameComps.Fallers.Has((object) go))
          GameComps.Fallers.Remove(go);
        GameComps.Fallers.Add(go, initial_velocity);
        yield return (object) new WaitForSeconds(0.25f);
      }
      yield return (object) new WaitForSeconds(0.35f);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, (string) ITEMS.LUBRICATIONSTICK.NAME, this.gameObject.transform, new Vector3(0.0f, 0.5f, 0.0f));
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner"));
      GameObject go1 = Util.KInstantiate(Assets.GetPrefab((Tag) "LubricationStick"), Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) - Vector3.right / 2f);
      go1.SetActive(true);
      Vector2 initial_velocity1 = new Vector2(3.75f, 2.5f);
      if (GameComps.Fallers.Has((object) go1))
        GameComps.Fallers.Remove(go1);
      GameComps.Fallers.Add(go1, initial_velocity1);
      yield return (object) 0;
    }
  }

  public class States : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad>
  {
    public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal openPortal;
    public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal closePortal;
    public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal idlePortal;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State idle;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State resetToIdle;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State opening;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State open;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State close;
    public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State unoperational;
    public Telepad.States.BonusDeliveryStates bonusDelivery;
    private static readonly HashedString[] workingAnims = new HashedString[2]
    {
      (HashedString) "working_loop",
      (HashedString) "working_pst"
    };

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.root.OnSignal(this.idlePortal, this.resetToIdle).EventTransition(GameHashes.BonusTelepadDelivery, this.bonusDelivery.pre);
      this.resetToIdle.GoTo(this.idle);
      this.idle.Enter((StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.UpdateMeter())).Update("TelepadMeter", (Action<Telepad.StatesInstance, float>) ((smi, dt) => smi.UpdateMeter()), UpdateRate.SIM_4000ms).EventTransition(GameHashes.OperationalChanged, this.unoperational, (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).PlayAnim("idle").OnSignal(this.openPortal, this.opening);
      this.unoperational.PlayAnim("idle").Enter("StopImmigration", (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.master.meter.SetPositionPercent(0.0f))).EventTransition(GameHashes.OperationalChanged, this.idle, (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.opening.Enter((StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.master.meter.SetPositionPercent(1f))).PlayAnim("working_pre").OnAnimQueueComplete(this.open);
      this.open.OnSignal(this.closePortal, this.close).Enter((StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.master.meter.SetPositionPercent(1f))).PlayAnim("working_loop", KAnim.PlayMode.Loop).Transition(this.close, (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Transition.ConditionCallback) (smi => smi.IsColonyLost())).EventTransition(GameHashes.OperationalChanged, this.close, (StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.close.Enter((StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State.Callback) (smi => smi.master.meter.SetPositionPercent(0.0f))).PlayAnims((Func<Telepad.StatesInstance, HashedString[]>) (smi => Telepad.States.workingAnims)).OnAnimQueueComplete(this.idle);
      this.bonusDelivery.pre.PlayAnim("bionic_working_pre").OnAnimQueueComplete(this.bonusDelivery.loop);
      this.bonusDelivery.loop.PlayAnim("bionic_working_loop", KAnim.PlayMode.Loop).ScheduleAction("SpawnBonusDelivery", 1f, (Action<Telepad.StatesInstance>) (smi => smi.master.StartCoroutine(smi.SpawnExtraPowerBanks()))).ScheduleGoTo(3f, (StateMachine.BaseState) this.bonusDelivery.pst);
      this.bonusDelivery.pst.PlayAnim("bionic_working_pst").OnAnimQueueComplete(this.idle);
    }

    public class BonusDeliveryStates : 
      GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State
    {
      public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pre;
      public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State loop;
      public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pst;
    }
  }
}
