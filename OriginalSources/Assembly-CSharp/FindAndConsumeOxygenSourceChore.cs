// Decompiled with JetBrains decompiler
// Type: FindAndConsumeOxygenSourceChore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FindAndConsumeOxygenSourceChore : Chore<FindAndConsumeOxygenSourceChore.Instance>
{
  public const string CANISTER_BODY_SYMBOL_NAME = "canister";
  public const string CANISTER_CAP_SYMBOL_NAME = "cap";
  public const string CANISTER_CAP_COLOR_SYMBOL_NAME = "substance_tinter_cap";
  public const string CANISTER_BODY_COLOR_SYMBOL_NAME = "substance_tinter";
  public const float MAX_LOOP_DURATION = 24f;
  public const float MIN_LOOP_DURATION = 4.333f;
  public static readonly Chore.Precondition OxygenSourceItemIsNotNull = new Chore.Precondition()
  {
    id = "OxygenSourceIsNotNull",
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Pickupable closestOxygenSource = context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>().GetClosestOxygenSource();
      return (UnityEngine.Object) closestOxygenSource != (UnityEngine.Object) null && (double) closestOxygenSource.UnreservedAmount > 0.0;
    })
  };

  public FindAndConsumeOxygenSourceChore(IStateMachineTarget target, bool critical)
    : base(critical ? Db.Get().ChoreTypes.FindOxygenSourceItem_Critical : Db.Get().ChoreTypes.FindOxygenSourceItem, target, target.GetComponent<ChoreProvider>(), false, master_priority_class: critical ? PriorityScreen.PriorityClass.compulsory : PriorityScreen.PriorityClass.personalNeeds)
  {
    this.smi = new FindAndConsumeOxygenSourceChore.Instance(this, target.gameObject);
    this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, (object) null);
    this.AddPrecondition(FindAndConsumeOxygenSourceChore.OxygenSourceItemIsNotNull, (object) null);
  }

  public override void Begin(Chore.Precondition.Context context)
  {
    if ((UnityEngine.Object) context.consumerState.consumer == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "FindAndConsumeOxygenSourceChore null context.consumer");
    }
    else
    {
      BionicOxygenTankMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>();
      if (smi == null)
      {
        Debug.LogError((object) "FindAndConsumeOxygenSourceChore null BionicOxygenTankMonitor.Instance");
      }
      else
      {
        Pickupable closestOxygenSource = smi.GetClosestOxygenSource();
        if ((UnityEngine.Object) closestOxygenSource == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "FindAndConsumeOxygenSourceChore null oxygenSourceItem.gameObject");
        }
        else
        {
          this.smi.sm.oxygenSourceItem.Set(closestOxygenSource.gameObject, this.smi, false);
          double num = (double) this.smi.sm.amountRequested.Set(Mathf.Min(smi.SpaceAvailableInTank, closestOxygenSource.UnreservedAmount), this.smi);
          this.smi.sm.dupe.Set((KMonoBehaviour) context.consumerState.consumer, this.smi);
          base.Begin(context);
        }
      }
    }
  }

  public static bool IsNotAllowedByScheduleAndChoreIsNotCritical(
    FindAndConsumeOxygenSourceChore.Instance smi)
  {
    return !FindAndConsumeOxygenSourceChore.IsCriticalChore(smi) && !FindAndConsumeOxygenSourceChore.IsAllowedBySchedule(smi);
  }

  public static bool IsAllowedBySchedule(FindAndConsumeOxygenSourceChore.Instance smi)
  {
    return BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi.oxygenTankMonitor);
  }

  public static bool IsCriticalChore(FindAndConsumeOxygenSourceChore.Instance smi)
  {
    return smi.master.choreType == Db.Get().ChoreTypes.FindOxygenSourceItem_Critical;
  }

  public static void ExtractOxygenFromItem(FindAndConsumeOxygenSourceChore.Instance smi)
  {
    GameObject original = smi.sm.pickedUpItem.Get(smi);
    PrimaryElement component = original.GetComponent<PrimaryElement>();
    if (component.Element.IsGas)
    {
      Storage[] components = smi.gameObject.GetComponents<Storage>();
      for (int index = 0; index < components.Length; ++index)
      {
        if ((UnityEngine.Object) components[index] != (UnityEngine.Object) smi.oxygenTankMonitor.storage)
        {
          List<GameObject> result = new List<GameObject>();
          components[index].Find(GameTags.Breathable, result);
          foreach (UnityEngine.Object @object in result)
          {
            if (@object != (UnityEngine.Object) null)
            {
              float amount_consumed;
              SimUtil.DiseaseInfo disease_info;
              float aggregate_temperature;
              components[index].ConsumeAndGetDisease(component.Element.tag, component.Mass, out amount_consumed, out disease_info, out aggregate_temperature);
              smi.oxygenTankMonitor.storage.AddGasChunk(component.Element.id, amount_consumed, aggregate_temperature, disease_info.idx, disease_info.count, false);
              break;
            }
          }
        }
      }
    }
    else
    {
      SimHashes element = SimHashes.Oxygen;
      if (ElementLoader.GetElement(component.Element.sublimateId.CreateTag()).HasTag(GameTags.Breathable))
        element = component.Element.sublimateId;
      smi.oxygenTankMonitor.storage.AddGasChunk(element, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false);
      Util.KDestroyGameObject(original);
    }
  }

  public static void SetOverrideAnimSymbol(
    FindAndConsumeOxygenSourceChore.Instance smi,
    bool overriding)
  {
    GameObject go = smi.sm.pickedUpItem.Get(smi);
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
    {
      KBatchedAnimTracker component = go.GetComponent<KBatchedAnimTracker>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.enabled = !overriding;
      Storage.MakeItemInvisible(go, overriding, false);
    }
    if (!overriding)
    {
      smi.RemoveSymbolOverrideObject();
    }
    else
    {
      if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
        return;
      PrimaryElement component = go.GetComponent<PrimaryElement>();
      smi.ShowBottleSymbolOverrideObject(component.Element);
    }
  }

  public static void TriggerOxygenItemLostSignal(FindAndConsumeOxygenSourceChore.Instance smi)
  {
    if (smi.oxygenTankMonitor == null)
      return;
    smi.oxygenTankMonitor.sm.OxygenSourceItemLostSignal.Trigger(smi.oxygenTankMonitor);
  }

  public static float GetConsumeDuration(FindAndConsumeOxygenSourceChore.Instance smi)
  {
    return Mathf.Max(24f * (smi.sm.actualunits.Get(smi) / BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG), 4.333f);
  }

  public class States : 
    GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore>
  {
    public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FetchSubState fetch;
    public FindAndConsumeOxygenSourceChore.States.InstallState consume;
    public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State complete;
    public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State oxygenSourceLost;
    public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State scheduleFailure;
    public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter dupe;
    public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter oxygenSourceItem;
    public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.TargetParameter pickedUpItem;
    public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FloatParameter actualunits;
    public StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.FloatParameter amountRequested;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.fetch;
      this.Target(this.dupe);
      this.fetch.InitializeStates(this.dupe, this.oxygenSourceItem, this.pickedUpItem, this.amountRequested, this.actualunits, (GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State) this.consume).OnTargetLost(this.oxygenSourceItem, this.oxygenSourceLost).ScheduleChange(this.scheduleFailure, new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.Transition.ConditionCallback(FindAndConsumeOxygenSourceChore.IsNotAllowedByScheduleAndChoreIsNotCritical));
      this.consume.Target(this.pickedUpItem).OnTargetLost(this.pickedUpItem, this.oxygenSourceLost).Target(this.dupe).ScheduleChange(this.scheduleFailure, new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.Transition.ConditionCallback(FindAndConsumeOxygenSourceChore.IsNotAllowedByScheduleAndChoreIsNotCritical)).DefaultState(this.consume.pre).ToggleAnims("anim_bionic_kanim").ToggleTag(GameTags.RecoveringBreath).Enter("Add Symbol Override", (StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback) (smi => FindAndConsumeOxygenSourceChore.SetOverrideAnimSymbol(smi, true))).Exit("Revert Symbol Override", (StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback) (smi => FindAndConsumeOxygenSourceChore.SetOverrideAnimSymbol(smi, false)));
      this.consume.pre.PlayAnim("consume_canister_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.consume.loop).ScheduleGoTo(3f, (StateMachine.BaseState) this.consume.loop);
      this.consume.loop.PlayAnim("consume_canister_loop", KAnim.PlayMode.Loop).ScheduleGoTo(new Func<FindAndConsumeOxygenSourceChore.Instance, float>(FindAndConsumeOxygenSourceChore.GetConsumeDuration), (StateMachine.BaseState) this.consume.pst);
      this.consume.pst.PlayAnim("consume_canister_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, (StateMachine.BaseState) this.complete);
      this.complete.Enter(new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback(FindAndConsumeOxygenSourceChore.ExtractOxygenFromItem)).ReturnSuccess();
      this.scheduleFailure.Target(this.dupe).ReturnFailure();
      this.oxygenSourceLost.Target(this.dupe).Enter(new StateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State.Callback(FindAndConsumeOxygenSourceChore.TriggerOxygenItemLostSignal)).ReturnFailure();
    }

    public class InstallState : 
      GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State
    {
      public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State pre;
      public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State loop;
      public GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.State pst;
    }
  }

  public class Instance(FindAndConsumeOxygenSourceChore master, GameObject duplicant) : 
    GameStateMachine<FindAndConsumeOxygenSourceChore.States, FindAndConsumeOxygenSourceChore.Instance, FindAndConsumeOxygenSourceChore, object>.GameInstance(master),
    BionicOxygenTankMonitor.IChore
  {
    public KBatchedAnimController canisterBodySymbolOverrideObject;
    public KBatchedAnimController canisterCapSymbolOverrideObject;

    public BionicOxygenTankMonitor.Instance oxygenTankMonitor
    {
      get => this.sm.dupe.Get(this).GetSMI<BionicOxygenTankMonitor.Instance>();
    }

    public bool IsConsumingOxygen() => !this.IsInsideState((StateMachine.BaseState) this.sm.fetch);

    public void ShowBottleSymbolOverrideObject(Element elementOfCanister)
    {
      if ((UnityEngine.Object) this.canisterBodySymbolOverrideObject == (UnityEngine.Object) null)
      {
        KAnimFile[] anims = elementOfCanister.substance.anims;
        GameObject gameObject = Util.NewGameObject(this.gameObject, "canister_symbol");
        gameObject.transform.SetParent(this.gameObject.transform, false);
        gameObject.SetActive(false);
        this.canisterBodySymbolOverrideObject = gameObject.AddComponent<KBatchedAnimController>();
        this.canisterBodySymbolOverrideObject.AnimFiles = anims;
        this.canisterBodySymbolOverrideObject.initialAnim = "idle1";
        this.canisterBodySymbolOverrideObject.SetSymbolVisiblity((KAnimHashedString) "cap", false);
        this.canisterBodySymbolOverrideObject.SetSymbolVisiblity((KAnimHashedString) "substance_tinter_cap", false);
        KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
        kbatchedAnimTracker.symbol = new HashedString("canister");
        kbatchedAnimTracker.offset = Vector3.zero;
        kbatchedAnimTracker.matchParentOffset = true;
        kbatchedAnimTracker.forceAlwaysAlive = true;
        kbatchedAnimTracker.forceAlwaysVisible = true;
        gameObject.SetActive(true);
        this.canisterBodySymbolOverrideObject.SetSymbolTint(new KAnimHashedString("substance_tinter"), (Color) (elementOfCanister.substance.colour with
        {
          a = byte.MaxValue
        }));
      }
      if ((UnityEngine.Object) this.canisterCapSymbolOverrideObject == (UnityEngine.Object) null)
      {
        KAnimFile[] anims = elementOfCanister.substance.anims;
        GameObject gameObject = Util.NewGameObject(this.gameObject, "canister_cap_symbol");
        gameObject.transform.SetParent(this.gameObject.transform, false);
        gameObject.SetActive(false);
        this.canisterCapSymbolOverrideObject = gameObject.AddComponent<KBatchedAnimController>();
        this.canisterCapSymbolOverrideObject.AnimFiles = anims;
        this.canisterCapSymbolOverrideObject.initialAnim = "cap";
        KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
        kbatchedAnimTracker.symbol = new HashedString("cap");
        kbatchedAnimTracker.offset = Vector3.zero;
        kbatchedAnimTracker.matchParentOffset = true;
        kbatchedAnimTracker.forceAlwaysAlive = true;
        kbatchedAnimTracker.forceAlwaysVisible = true;
        gameObject.SetActive(true);
        this.canisterCapSymbolOverrideObject.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), (Color) (elementOfCanister.substance.colour with
        {
          a = byte.MaxValue
        }));
      }
      KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
      Vector3 column = (Vector3) component.GetSymbolTransform((HashedString) "canister", out bool _).GetColumn(3) with
      {
        z = this.canisterBodySymbolOverrideObject.transform.parent.position.z - 0.01f
      };
      this.canisterBodySymbolOverrideObject.transform.position = column;
      this.canisterCapSymbolOverrideObject.transform.position = (Vector3) component.GetSymbolTransform((HashedString) "canister", out bool _).GetColumn(3) with
      {
        z = column.z - 0.01f
      };
      component.SetSymbolVisiblity((KAnimHashedString) "canister", false);
      component.SetSymbolVisiblity((KAnimHashedString) "cap", false);
    }

    public void RemoveSymbolOverrideObject()
    {
      if ((UnityEngine.Object) this.canisterBodySymbolOverrideObject != (UnityEngine.Object) null)
      {
        this.canisterBodySymbolOverrideObject.gameObject.DeleteObject();
        this.canisterBodySymbolOverrideObject = (KBatchedAnimController) null;
      }
      if (!((UnityEngine.Object) this.canisterCapSymbolOverrideObject != (UnityEngine.Object) null))
        return;
      this.canisterCapSymbolOverrideObject.gameObject.DeleteObject();
      this.canisterCapSymbolOverrideObject = (KBatchedAnimController) null;
    }

    protected override void OnCleanUp()
    {
      this.RemoveSymbolOverrideObject();
      base.OnCleanUp();
    }
  }
}
