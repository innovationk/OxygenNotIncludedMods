// Decompiled with JetBrains decompiler
// Type: MosquitoHungerMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MosquitoHungerMonitor : StateMachineComponent<MosquitoHungerMonitor.Instance>
{
  public const string DupeMosquitoBiteEffectName = "DupeMosquitoBite";
  public const string CritterMosquitoBiteEffectName = "CritterMosquitoBite";
  public const string Dupe_SUPPRESSED_MosquitoBiteEffectName = "DupeMosquitoBiteSuppressed";
  public const string Critter_SUPPRESSED_MosquitoBiteEffectName = "CritterMosquitoBiteSuppressed";
  public const string MosquitoFedEffectName = "MosquitoFed";
  public const int ReachabilityPadding = 1;
  public bool CanBiteMinions = true;
  public List<Tag> AllowedTargetTags;
  public List<Tag> ForbiddenTargetTags;
  public static string[] ImmunityEffectNames = new string[1]
  {
    "HistamineSuppression"
  };

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  private static void ClearTarget(MosquitoHungerMonitor.Instance smi)
  {
    smi.sm.victim.Set((KMonoBehaviour) null, smi);
  }

  public static bool IsFed(MosquitoHungerMonitor.Instance smi) => smi.IsFed;

  public static bool HasValidVictim(MosquitoHungerMonitor.Instance smi)
  {
    return MosquitoHungerMonitor.HasValidVictim(smi, smi.Victim);
  }

  public static bool HasValidVictim(MosquitoHungerMonitor.Instance smi, GameObject victimParam)
  {
    return (UnityEngine.Object) victimParam != (UnityEngine.Object) null && !MosquitoHungerMonitor.IsVictimForbidden(smi, victimParam.GetComponent<KPrefabID>(), true);
  }

  public static void LookForVictim(MosquitoHungerMonitor.Instance smi)
  {
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((StateMachine.Instance) smi));
    if (cavityForCell == null)
      return;
    int myWorldId = smi.GetMyWorldId();
    List<KPrefabID> tList = new List<KPrefabID>();
    if (smi.master.CanBiteMinions)
    {
      List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(myWorldId);
      for (int index = 0; index < worldItems.Count; ++index)
      {
        KPrefabID component = worldItems[index].GetComponent<KPrefabID>();
        if (!MosquitoHungerMonitor.IsVictimForbidden(smi, component, true))
          tList.Add(component);
      }
    }
    for (int index = 0; index < cavityForCell.creatures.Count; ++index)
    {
      KPrefabID creature = cavityForCell.creatures[index];
      if (creature.HasAnyTags(smi.master.AllowedTargetTags) && !MosquitoHungerMonitor.IsVictimForbidden(smi, creature))
        tList.Add(creature);
    }
    KPrefabID random = tList.Count > 0 ? tList.GetRandom<KPrefabID>() : (KPrefabID) null;
    smi.sm.victim.Set((KMonoBehaviour) random, smi);
  }

  private static bool IsVictimForbidden(
    MosquitoHungerMonitor.Instance smi,
    KPrefabID victim,
    bool mustBeInSameCavity = false)
  {
    int cell = Grid.PosToCell((KMonoBehaviour) victim);
    if (mustBeInSameCavity)
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((StateMachine.Instance) smi));
      if (Game.Instance.roomProber.GetCavityForCell(cell) != cavityForCell)
        return true;
    }
    if (victim.HasAnyTags(smi.master.ForbiddenTargetTags))
      return true;
    Effects component1 = victim.GetComponent<Effects>();
    if (component1.HasEffect("DupeMosquitoBite") || component1.HasEffect("CritterMosquitoBite") || component1.HasEffect("DupeMosquitoBiteSuppressed") || component1.HasEffect("CritterMosquitoBiteSuppressed"))
      return true;
    OccupyArea component2 = victim.GetComponent<OccupyArea>();
    return !smi.navigator.CanReach(cell, component2.OccupiedCellsOffsets);
  }

  public static void InitiatePokeBehaviour(MosquitoHungerMonitor.Instance smi)
  {
    PokeMonitor.Instance smi1 = smi.GetSMI<PokeMonitor.Instance>();
    CellOffset[] cellOffsetArray = smi.Victim.GetComponent<OccupyArea>().OccupiedCellsOffsets;
    for (int index = 0; index < 1; ++index)
      cellOffsetArray = cellOffsetArray.Expand();
    smi1.InitiatePoke(smi.Victim, cellOffsetArray);
  }

  public static void AbortPokeBehaviour(MosquitoHungerMonitor.Instance smi)
  {
    smi.GetSMI<PokeMonitor.Instance>()?.AbortPoke();
  }

  public static void OnVictimPoked(MosquitoHungerMonitor.Instance smi, object victimOBJ)
  {
    if (victimOBJ == null)
      return;
    GameObject go = (GameObject) victimOBJ;
    Effects component = go.GetComponent<Effects>();
    bool flag1 = go.HasTag(GameTags.BaseMinion);
    bool flag2 = false;
    foreach (string immunityEffectName in MosquitoHungerMonitor.ImmunityEffectNames)
      flag2 = flag2 || component.HasEffect(immunityEffectName);
    if (flag1)
      component.Add(flag2 ? "DupeMosquitoBiteSuppressed" : "DupeMosquitoBite", true);
    else
      component.Add(flag2 ? "CritterMosquitoBiteSuppressed" : "CritterMosquitoBite", true);
    smi.ApplyFedEffect();
  }

  public class States : 
    GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor>
  {
    public GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State satisfied;
    public MosquitoHungerMonitor.States.HungryStates hungry;
    public StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.TargetParameter victim;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.Never;
      default_state = (StateMachine.BaseState) this.satisfied;
      this.satisfied.EventTransition(GameHashes.EffectRemoved, (GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State) this.hungry, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Not(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Transition.ConditionCallback(MosquitoHungerMonitor.IsFed))).Enter(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State.Callback(MosquitoHungerMonitor.ClearTarget));
      this.hungry.EventTransition(GameHashes.EffectAdded, this.satisfied, new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Transition.ConditionCallback(MosquitoHungerMonitor.IsFed)).DefaultState(this.hungry.lookingForVictim);
      this.hungry.lookingForVictim.ToggleStatusItem((string) CREATURES.STATUSITEMS.HUNGRY.NAME, (string) CREATURES.STATUSITEMS.HUNGRY.TOOLTIP).ParamTransition<GameObject>((StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Parameter<GameObject>) this.victim, this.hungry.chaseVictim, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.IsNotNull).PreBrainUpdate(new Action<MosquitoHungerMonitor.Instance>(MosquitoHungerMonitor.LookForVictim));
      this.hungry.chaseVictim.ParamTransition<GameObject>((StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Parameter<GameObject>) this.victim, this.hungry.lookingForVictim, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.IsNull).EventTransition(GameHashes.TargetLost, this.hungry.lookingForVictim).Enter(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State.Callback(MosquitoHungerMonitor.InitiatePokeBehaviour)).EventHandler(GameHashes.EntityPoked, new GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.GameEvent.Callback(MosquitoHungerMonitor.OnVictimPoked)).Exit(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State.Callback(MosquitoHungerMonitor.AbortPokeBehaviour)).Exit(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State.Callback(MosquitoHungerMonitor.ClearTarget)).Target(this.victim).EventTransition(GameHashes.TagsChanged, this.hungry.lookingForVictim, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Not(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Transition.ConditionCallback(MosquitoHungerMonitor.HasValidVictim))).EventTransition(GameHashes.EffectAdded, this.hungry.lookingForVictim, GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Not(new StateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.Transition.ConditionCallback(MosquitoHungerMonitor.HasValidVictim)));
    }

    public class HungryStates : 
      GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State
    {
      public GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State lookingForVictim;
      public GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.State chaseVictim;
    }
  }

  public class Instance : 
    GameStateMachine<MosquitoHungerMonitor.States, MosquitoHungerMonitor.Instance, MosquitoHungerMonitor, object>.GameInstance
  {
    private Effects effects;

    public GameObject Victim => this.sm.victim.Get(this);

    public bool IsFed => this.effects.HasEffect("MosquitoFed");

    public Navigator navigator { private set; get; }

    public Instance(MosquitoHungerMonitor master)
      : base(master)
    {
      this.effects = this.GetComponent<Effects>();
      this.navigator = this.GetComponent<Navigator>();
    }

    public void ApplyFedEffect() => this.effects.Add("MosquitoFed", true);
  }
}
