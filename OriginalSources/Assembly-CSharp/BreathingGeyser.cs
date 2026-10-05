// Decompiled with JetBrains decompiler
// Type: BreathingGeyser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using UnityEngine;

#nullable disable
public class BreathingGeyser : 
  GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>
{
  public StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.BoolParameter exhaleFlowDirection;
  public GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State inactive;
  public BreathingGeyser.ActiveStates active;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.inactive;
    this.root.Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.RestrictElementConsumer));
    this.inactive.EventTransition(GameHashes.SubmergedStateChanged, (GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State) this.active, new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.Transition.ConditionCallback(BreathingGeyser.IsSubmerged)).EventTransition(GameHashes.OnStorageChange, (GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State) this.active.exhale, new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.Transition.ConditionCallback(BreathingGeyser.HasStoredMass)).PlayAnim("inactive").Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.DisableElementConsumer)).Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.RestrictElementConsumer));
    this.active.Transition(this.inactive, new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.Transition.ConditionCallback(BreathingGeyser.ShouldGoInactive), UpdateRate.SIM_1000ms).DefaultState((GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State) this.active.inhale);
    this.active.inhale.ParamTransition<bool>((StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.Parameter<bool>) this.exhaleFlowDirection, (GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State) this.active.exhale, GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.IsTrue).DefaultState(this.active.inhale.pre);
    this.active.inhale.pre.PlayAnim("inhale_pre").OnAnimQueueComplete(this.active.inhale.loop);
    this.active.inhale.loop.Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.UnRestrictElementConsumer)).Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.EnableElementConsumer)).PlayAnim("inhale_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.active.inhale.pst, new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.Transition.ConditionCallback(BreathingGeyser.IsStorageFull)).EventTransition(GameHashes.SubmergedStateChanged, this.active.inhale.pst, new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.Transition.ConditionCallback(BreathingGeyser.IsNotSubmergedWithStorage));
    this.active.inhale.pst.PlayAnim("inhale_pst").OnAnimQueueComplete(this.active.inhale.done);
    this.active.inhale.done.Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.StopInhaling));
    this.active.exhale.ParamTransition<bool>((StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.Parameter<bool>) this.exhaleFlowDirection, (GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State) this.active.inhale, GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.IsFalse).DefaultState(this.active.exhale.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeyserExpelling, (Func<BreathingGeyser.Instance, object>) (smi => (object) new Tuple<Element, float>(smi.GetStoredElementTag(), smi.def.exhaleRate))).ToggleTag(GameTags.GeyserExhaling).Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.DisableElementConsumer)).Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.RestrictElementConsumer));
    this.active.exhale.pre.PlayAnim("exhale_pre").OnAnimQueueComplete(this.active.exhale.loop);
    this.active.exhale.loop.EventTransition(GameHashes.OnStorageChange, this.active.exhale.pst, new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.Transition.ConditionCallback(BreathingGeyser.IsStorageEmpty)).PlayAnim("exhale_loop", KAnim.PlayMode.Loop).Update(new System.Action<BreathingGeyser.Instance, float>(BreathingGeyser.ExhaleUpdate), UpdateRate.SIM_1000ms);
    this.active.exhale.pst.PlayAnim("exhale_pst").OnAnimQueueComplete(this.active.exhale.done);
    this.active.exhale.done.Enter(new StateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State.Callback(BreathingGeyser.StopExhaling));
  }

  public static void StopInhaling(BreathingGeyser.Instance smi)
  {
    smi.sm.exhaleFlowDirection.Set(true, smi);
  }

  public static void StopExhaling(BreathingGeyser.Instance smi)
  {
    smi.sm.exhaleFlowDirection.Set(false, smi);
  }

  public static void RestrictElementConsumer(BreathingGeyser.Instance smi)
  {
    smi.RestrictElementConsumerState();
  }

  public static void UnRestrictElementConsumer(BreathingGeyser.Instance smi)
  {
    smi.UnrestrictElementConsumerState();
  }

  public static void EnableElementConsumer(BreathingGeyser.Instance smi)
  {
    smi.SetElementConsumerState(true);
  }

  public static void DisableElementConsumer(BreathingGeyser.Instance smi)
  {
    smi.SetElementConsumerState(false);
  }

  public static bool IsSubmerged(BreathingGeyser.Instance smi) => smi.IsSubmerged;

  public static bool IsStorageFull(BreathingGeyser.Instance smi) => smi.IsStorageFull;

  public static bool IsStorageEmpty(BreathingGeyser.Instance smi) => smi.IsStorageEmpty;

  public static bool HasStoredMass(BreathingGeyser.Instance smi) => !smi.IsStorageEmpty;

  public static bool ShouldGoInactive(BreathingGeyser.Instance smi)
  {
    return !smi.IsSubmerged && smi.IsStorageEmpty;
  }

  public static bool IsNotSubmergedWithStorage(BreathingGeyser.Instance smi)
  {
    return !smi.IsSubmerged && !smi.IsStorageEmpty;
  }

  public static void ExhaleUpdate(BreathingGeyser.Instance smi, float dt) => smi.ExhaleUpdate(dt);

  public class Def : StateMachine.BaseDef
  {
    public float inhaleRate;
    public float exhaleRate;
    public byte diseaseIdx;
    public float germsPerKg;
  }

  public class ActiveStates : 
    GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State
  {
    public BreathingGeyser.ActiveStates.AnimStates inhale;
    public BreathingGeyser.ActiveStates.AnimStates exhale;

    public class AnimStates : 
      GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State
    {
      public GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State pre;
      public GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State loop;
      public GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State pst;
      public GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.State done;
    }
  }

  public new class Instance : 
    GameStateMachine<BreathingGeyser, BreathingGeyser.Instance, IStateMachineTarget, BreathingGeyser.Def>.GameInstance
  {
    private ElementConsumer elementConsumer;
    private Submergable submergable;
    private Storage storage;
    private int exhaleCell;
    private KPrefabID prefabID;

    public bool IsSubmerged => this.submergable.IsSubmerged;

    public Instance(IStateMachineTarget master, BreathingGeyser.Def def)
      : base(master, def)
    {
      this.elementConsumer = this.GetComponent<ElementConsumer>();
      this.storage = this.GetComponent<Storage>();
      this.submergable = this.GetComponent<Submergable>();
      this.exhaleCell = Grid.PosToCell(this.transform.GetPosition() + this.elementConsumer.sampleCellOffset);
      this.prefabID = this.GetComponent<KPrefabID>();
    }

    public bool IsExhaling => this.prefabID.HasTag(GameTags.GeyserExhaling);

    public void ExhaleUpdate(float dt)
    {
      if ((double) dt == 0.0)
        return;
      PrimaryElement firstWithMass = this.storage.FindFirstWithMass(GameTags.Liquid);
      if ((UnityEngine.Object) firstWithMass == (UnityEngine.Object) null || (double) firstWithMass.Mass == 0.0)
        return;
      float b = this.smi.def.exhaleRate * dt;
      float amount = Mathf.Min(firstWithMass.Mass, b);
      SimHashes elementId = firstWithMass.ElementID;
      float amount_consumed;
      SimUtil.DiseaseInfo disease_info;
      float aggregate_temperature;
      this.storage.ConsumeAndGetDisease(firstWithMass.Element.tag, amount, out amount_consumed, out disease_info, out aggregate_temperature);
      if ((double) amount_consumed <= 0.0)
        return;
      SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_info.idx, disease_info.count, this.smi.def.diseaseIdx, (int) ((double) this.smi.def.germsPerKg * (double) amount_consumed));
      SimMessages.AddRemoveSubstance(this.exhaleCell, elementId, CellEventLogger.Instance.BreathingGeyser, amount_consumed, aggregate_temperature, finalDiseaseInfo.idx, finalDiseaseInfo.count);
    }

    public Element GetStoredElementTag()
    {
      PrimaryElement firstWithMass = this.storage.FindFirstWithMass(GameTags.Liquid);
      return (UnityEngine.Object) firstWithMass == (UnityEngine.Object) null ? (Element) null : firstWithMass.Element;
    }

    public bool IsStorageFull => (double) this.storage.RemainingCapacity() <= 0.0;

    public bool IsStorageEmpty => (double) this.storage.ExactMassStored() == 0.0;

    public float GetStoredMass() => this.storage.MassStored();

    public string GetExpellingElementName()
    {
      PrimaryElement firstWithMass = this.storage.FindFirstWithMass(GameTags.Liquid);
      return (UnityEngine.Object) firstWithMass != (UnityEngine.Object) null && (double) firstWithMass.Mass > 0.0 ? firstWithMass.Element.name : (string) Strings.Get("STRINGS.ELEMENTS.STATE.LIQUID");
    }

    public void RestrictElementConsumerState() => this.elementConsumer.consumptionRate = 0.0f;

    public void UnrestrictElementConsumerState()
    {
      this.elementConsumer.consumptionRate = this.def.inhaleRate;
    }

    public void SetElementConsumerState(bool enabled)
    {
      this.elementConsumer.EnableConsumption(enabled);
    }
  }
}
