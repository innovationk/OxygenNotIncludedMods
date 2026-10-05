// Decompiled with JetBrains decompiler
// Type: ThreatMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ThreatMonitor : 
  GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>
{
  public ThreatMonitor.SafeStates safe;
  public ThreatMonitor.ThreatenedStates threatened;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.safe;
    this.root.EventHandler(GameHashes.SafeFromThreats, (GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameEvent.Callback) ((smi, d) => smi.OnSafe(d))).EventHandler(GameHashes.Attacked, (GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameEvent.Callback) ((smi, d) => smi.OnAttacked(d))).EventHandler(GameHashes.ObjectDestroyed, (GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameEvent.Callback) ((smi, d) => smi.Cleanup(d)));
    this.safe.Enter((StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback) (smi => smi.revengeThreat.Clear())).Enter(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats)).EventHandler(GameHashes.FactionChanged, new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats));
    this.safe.passive.DoNothing();
    this.safe.seeking.PreBrainUpdate((System.Action<ThreatMonitor.Instance>) (smi => smi.RefreshThreat((object) null)));
    this.threatened.duplicant.ShouldFight.Transition((GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State) this.safe, GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Not(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback(ThreatMonitor.DupeHasValidTarget))).ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateAttackChore), (GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State) this.safe).Update("DupeUpdateTarget", new System.Action<ThreatMonitor.Instance, float>(ThreatMonitor.DupeUpdateTarget));
    this.threatened.duplicant.ShoudFlee.Transition((GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State) this.safe, GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Not(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback(ThreatMonitor.MainThreatExists))).ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateFleeChore), (GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State) this.safe);
    this.threatened.creature.ToggleBehaviour(GameTags.Creatures.Flee, (StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback) (smi => !smi.WillFight()), (System.Action<ThreatMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.safe))).ToggleBehaviour(GameTags.Creatures.Attack, (StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback) (smi => smi.WillFight()), (System.Action<ThreatMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.safe))).Update("CritterCalmUpdate", new System.Action<ThreatMonitor.Instance, float>(ThreatMonitor.CritterCalmUpdate)).PreBrainUpdate(new System.Action<ThreatMonitor.Instance>(ThreatMonitor.CritterUpdateThreats));
  }

  private static void SeekThreats(ThreatMonitor.Instance smi)
  {
    Faction faction = FactionManager.Instance.GetFaction(smi.alignment.Alignment);
    if (smi.IAmADuplicant || faction.CanAttack)
      smi.GoTo((StateMachine.BaseState) smi.sm.safe.seeking);
    else
      smi.GoTo((StateMachine.BaseState) smi.sm.safe.passive);
  }

  private static bool MainThreatExists(ThreatMonitor.Instance smi)
  {
    return (UnityEngine.Object) smi.MainThreat != (UnityEngine.Object) null;
  }

  private static bool DupeHasValidTarget(ThreatMonitor.Instance smi)
  {
    bool flag = false;
    if ((UnityEngine.Object) smi.MainThreat != (UnityEngine.Object) null && smi.MainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
    {
      IApproachable component = (IApproachable) smi.MainThreat.GetComponent<RangedAttackable>();
      if (component != null)
        flag = smi.navigator.GetNavigationCost(component) != -1;
    }
    return flag;
  }

  private static void DupeUpdateTarget(ThreatMonitor.Instance smi, float dt)
  {
    if (ThreatMonitor.DupeHasValidTarget(smi))
      return;
    smi.Trigger(2144432245);
  }

  private static void CritterCalmUpdate(ThreatMonitor.Instance smi, float dt)
  {
    if (smi.isMasterNull || !((UnityEngine.Object) smi.revengeThreat.target != (UnityEngine.Object) null) || !smi.revengeThreat.Calm(dt, smi.alignment))
      return;
    smi.Trigger(-21431934);
  }

  private static void CritterUpdateThreats(ThreatMonitor.Instance smi)
  {
    if (smi.isMasterNull || smi.CheckForThreats() || ThreatMonitor.IsInSafeState(smi))
      return;
    smi.GoTo((StateMachine.BaseState) smi.sm.safe);
  }

  private static bool IsInSafeState(ThreatMonitor.Instance smi)
  {
    return smi.GetCurrentState() == smi.sm.safe.passive || smi.GetCurrentState() == smi.sm.safe.seeking;
  }

  private Chore CreateAttackChore(ThreatMonitor.Instance smi)
  {
    return (Chore) new AttackChore(smi.master, smi.MainThreat);
  }

  private Chore CreateFleeChore(ThreatMonitor.Instance smi)
  {
    return (Chore) new FleeChore(smi.master, smi.MainThreat);
  }

  public class Def : StateMachine.BaseDef
  {
    public Health.HealthState fleethresholdState = Health.HealthState.Injured;
    public Tag[] friendlyCreatureTags;
    public int maxSearchEntities = 50;
    public int maxSearchDistance = 20;
    public CellOffset[] offsets = OffsetGroups.Use;
  }

  public class SafeStates : 
    GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
  {
    public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State passive;
    public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State seeking;
  }

  public class ThreatenedStates : 
    GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
  {
    public ThreatMonitor.ThreatenedDuplicantStates duplicant;
    public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State creature;
  }

  public class ThreatenedDuplicantStates : 
    GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
  {
    public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShoudFlee;
    public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShouldFight;
  }

  public struct Grudge
  {
    public FactionAlignment target;
    public float grudgeTime;

    public void Reset(FactionAlignment revengeTarget)
    {
      this.target = revengeTarget;
      this.grudgeTime = 10f;
    }

    public bool Calm(float dt, FactionAlignment self)
    {
      if ((double) this.grudgeTime <= 0.0)
        return true;
      this.grudgeTime = Mathf.Max(0.0f, this.grudgeTime - dt);
      if ((double) this.grudgeTime != 0.0)
        return false;
      if (FactionManager.Instance.GetDisposition(self.Alignment, this.target.Alignment) != FactionManager.Disposition.Attack)
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, (string) UI.GAMEOBJECTEFFECTS.FORGAVEATTACKER, self.transform, 2f, true);
      this.Clear();
      return true;
    }

    public void Clear()
    {
      this.grudgeTime = 0.0f;
      this.target = (FactionAlignment) null;
    }

    public bool IsValidRevengeTarget(bool isDuplicant)
    {
      if (!((UnityEngine.Object) this.target != (UnityEngine.Object) null) || !this.target.IsAlignmentActive() || !((UnityEngine.Object) this.target.health == (UnityEngine.Object) null) && this.target.health.IsDefeated())
        return false;
      return !isDuplicant || !this.target.IsPlayerTargeted();
    }
  }

  public new class Instance : 
    GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameInstance
  {
    public FactionAlignment alignment;
    public Navigator navigator;
    public ChoreDriver choreDriver;
    private Health health;
    private ChoreConsumer choreConsumer;
    public ThreatMonitor.Grudge revengeThreat;
    public bool attackOwnFaction;
    public int currentUpdateIndex;
    private GameObject mainThreat;
    private FactionManager.FactionID mainThreatFaction;
    private List<FactionAlignment> threats = new List<FactionAlignment>();
    private System.Action<object> refreshThreatDelegate;

    public GameObject MainThreat => this.mainThreat;

    public bool IAmADuplicant => this.alignment.Alignment == FactionManager.FactionID.Duplicant;

    public Instance(IStateMachineTarget master, ThreatMonitor.Def def)
      : base(master, def)
    {
      this.alignment = master.GetComponent<FactionAlignment>();
      this.navigator = master.GetComponent<Navigator>();
      this.choreDriver = master.GetComponent<ChoreDriver>();
      this.health = master.GetComponent<Health>();
      this.choreConsumer = master.GetComponent<ChoreConsumer>();
      this.refreshThreatDelegate = new System.Action<object>(this.RefreshThreat);
    }

    public void ClearMainThreat() => this.SetMainThreat((GameObject) null);

    public void SetMainThreat(GameObject threat)
    {
      if ((UnityEngine.Object) threat == (UnityEngine.Object) this.mainThreat)
        return;
      if ((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null)
      {
        this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
        this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
        if ((UnityEngine.Object) threat == (UnityEngine.Object) null)
          this.Trigger(2144432245);
      }
      if ((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null)
      {
        this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
        this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
      }
      this.mainThreat = threat;
      if (!((UnityEngine.Object) this.mainThreat != (UnityEngine.Object) null))
        return;
      this.mainThreatFaction = this.mainThreat.GetComponent<FactionAlignment>().Alignment;
      this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
      this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
    }

    public bool HasThreat() => (UnityEngine.Object) this.MainThreat != (UnityEngine.Object) null;

    public void OnSafe(object data)
    {
      if (!((UnityEngine.Object) this.revengeThreat.target != (UnityEngine.Object) null))
        return;
      if (!this.revengeThreat.target.GetComponent<FactionAlignment>().IsAlignmentActive())
        this.revengeThreat.Clear();
      this.ClearMainThreat();
    }

    public void OnAttacked(object data)
    {
      FactionAlignment revengeTarget = (FactionAlignment) data;
      this.revengeThreat.Reset(revengeTarget);
      Game.BrainScheduler.PrioritizeBrain(this.GetComponent<Brain>());
      if ((UnityEngine.Object) this.mainThreat == (UnityEngine.Object) null)
      {
        this.SetMainThreat(revengeTarget.gameObject);
        this.GoToThreatened();
      }
      else if (!this.WillFight())
        this.GoToThreatened();
      if (!(bool) (UnityEngine.Object) revengeTarget.GetComponent<Bee>())
        return;
      Chore currentChore = (UnityEngine.Object) this.choreDriver != (UnityEngine.Object) null ? this.choreDriver.GetCurrentChore() : (Chore) null;
      if (currentChore == null || !((UnityEngine.Object) currentChore.gameObject.GetComponent<HiveWorkableEmpty>() != (UnityEngine.Object) null))
        return;
      currentChore.gameObject.GetComponent<HiveWorkableEmpty>().wasStung = true;
    }

    public bool WillFight()
    {
      return (!((UnityEngine.Object) this.choreConsumer != (UnityEngine.Object) null) || this.choreConsumer.IsPermittedByUser(Db.Get().ChoreGroups.Combat) && this.choreConsumer.IsPermittedByTraits(Db.Get().ChoreGroups.Combat)) && (this.IAmADuplicant || this.smi.mainThreatFaction != FactionManager.FactionID.Predator) && this.health.State < this.smi.def.fleethresholdState;
    }

    private void GotoThreatResponse()
    {
      Chore currentChore = this.smi.master.GetComponent<ChoreDriver>().GetCurrentChore();
      if (this.WillFight() && this.mainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
      {
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.threatened.duplicant.ShouldFight);
      }
      else
      {
        if (currentChore != null && currentChore.target != null && currentChore.target != this.master && (UnityEngine.Object) currentChore.target.GetComponent<Pickupable>() != (UnityEngine.Object) null)
          return;
        this.smi.GoTo((StateMachine.BaseState) this.smi.sm.threatened.duplicant.ShoudFlee);
      }
    }

    public void GoToThreatened()
    {
      if (this.IAmADuplicant)
        this.GotoThreatResponse();
      else
        this.smi.GoTo((StateMachine.BaseState) this.sm.threatened.creature);
    }

    public void Cleanup(object data)
    {
      if (!(bool) (UnityEngine.Object) this.mainThreat)
        return;
      this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
      this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
    }

    public void RefreshThreat(object data)
    {
      if (!this.IsRunning())
        return;
      if (this.smi.CheckForThreats())
      {
        this.GoToThreatened();
      }
      else
      {
        if (ThreatMonitor.IsInSafeState(this.smi))
          return;
        this.Trigger(-21431934);
        this.smi.GoTo((StateMachine.BaseState) this.sm.safe);
      }
    }

    public bool CheckForThreats()
    {
      if (this.isMasterNull)
        return false;
      GameObject threat = !this.revengeThreat.IsValidRevengeTarget(this.IAmADuplicant) ? (!this.IAmADuplicant ? this.FindThreatOther() : this.FindThreatDuplicant()) : this.revengeThreat.target.gameObject;
      this.SetMainThreat(threat);
      return (UnityEngine.Object) threat != (UnityEngine.Object) null;
    }

    private GameObject FindThreatDuplicant()
    {
      this.threats.Clear();
      if (this.WillFight())
      {
        foreach (FactionAlignment factionAlignment in Components.PlayerTargeted)
        {
          if (!factionAlignment.IsNullOrDestroyed() && factionAlignment.IsPlayerTargeted() && !factionAlignment.health.IsDefeated() && this.navigator.CanReach(factionAlignment.attackable.GetCell(), this.smi.def.offsets))
            this.threats.Add(factionAlignment);
        }
      }
      return this.PickBestTarget(this.threats);
    }

    private GameObject FindThreatOther()
    {
      this.threats.Clear();
      this.GatherThreats();
      return this.PickBestTarget(this.threats);
    }

    private static Util.IterationInstruction collectFactionAlignments(
      object obj,
      List<FactionAlignment> alignments)
    {
      alignments.Add(obj as FactionAlignment);
      return Util.IterationInstruction.Continue;
    }

    private void GatherThreats()
    {
      ListPool<FactionAlignment, ThreatMonitor>.PooledList context = ListPool<FactionAlignment, ThreatMonitor>.Allocate();
      Extents extents = new Extents(Grid.PosToCell(this.gameObject), this.def.maxSearchDistance);
      GameScenePartitioner.Instance.VisitEntries<ListPool<FactionAlignment, ThreatMonitor>.PooledList>(extents.x, extents.y, extents.width, extents.height, GameScenePartitioner.Instance.attackableEntitiesLayer, new Func<object, ListPool<FactionAlignment, ThreatMonitor>.PooledList, Util.IterationInstruction>(ThreatMonitor.Instance.collectFactionAlignments), context);
      int count = context.Count;
      int num = Mathf.Min(count, this.def.maxSearchEntities);
      for (int index = 0; index < num; ++index)
      {
        if (this.currentUpdateIndex >= count)
          this.currentUpdateIndex = 0;
        FactionAlignment factionAlignment = context[this.currentUpdateIndex];
        ++this.currentUpdateIndex;
        if (!((UnityEngine.Object) factionAlignment.transform == (UnityEngine.Object) null) && !((UnityEngine.Object) factionAlignment == (UnityEngine.Object) this.alignment) && (this.def.friendlyCreatureTags == null || !factionAlignment.kprefabID.HasAnyTags(this.def.friendlyCreatureTags)) && factionAlignment.IsAlignmentActive() && (FactionManager.Instance.GetDisposition(this.alignment.Alignment, factionAlignment.Alignment) == FactionManager.Disposition.Attack || this.attackOwnFaction && factionAlignment.Alignment == this.alignment.Alignment) && this.navigator.CanReach(factionAlignment.attackable.GetCell(), this.smi.def.offsets))
          this.threats.Add(factionAlignment);
      }
      context.Recycle();
    }

    public GameObject PickBestTarget(List<FactionAlignment> threats)
    {
      float num1 = 1f;
      Vector2 position = (Vector2) this.gameObject.transform.GetPosition();
      GameObject gameObject = (GameObject) null;
      float num2 = float.PositiveInfinity;
      for (int index = threats.Count - 1; index >= 0; --index)
      {
        FactionAlignment threat = threats[index];
        float num3 = Vector2.Distance(position, (Vector2) threat.transform.GetPosition()) / num1;
        if ((double) num3 < (double) num2)
        {
          num2 = num3;
          gameObject = threat.gameObject;
        }
      }
      return gameObject;
    }
  }
}
