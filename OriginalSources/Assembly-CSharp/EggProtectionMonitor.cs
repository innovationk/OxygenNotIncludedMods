// Decompiled with JetBrains decompiler
// Type: EggProtectionMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EggProtectionMonitor : 
  GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>
{
  public StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.BoolParameter hasEggToGuard;
  public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State find_egg;
  public EggProtectionMonitor.GuardEggStates guard;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.find_egg;
    this.find_egg.BatchUpdate(new UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.BatchUpdateDelegate(EggProtectionMonitor.Instance.FindEggToGuard)).ParamTransition<bool>((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.Parameter<bool>) this.hasEggToGuard, this.guard.safe, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsTrue);
    this.guard.Enter((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State.Callback) (smi =>
    {
      smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) smi.def.build), smi.def.animPrefix, "_heat");
      smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Hostile);
      smi.threatMonitor.attackOwnFaction = true;
    })).Exit((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State.Callback) (smi =>
    {
      if (!smi.def.animPrefix.IsNullOrWhiteSpace())
        smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) smi.def.build), smi.def.animPrefix);
      else
        smi.gameObject.AddOrGet<SymbolOverrideController>().RemoveBuildOverride(Assets.GetAnim((HashedString) smi.def.build).GetData());
      smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(smi.def.defaultFaction);
      smi.threatMonitor.attackOwnFaction = false;
    })).Update("CanProtectEgg", (System.Action<EggProtectionMonitor.Instance, float>) ((smi, dt) => smi.CanProtectEgg()), UpdateRate.SIM_1000ms, true).ParamTransition<bool>((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.Parameter<bool>) this.hasEggToGuard, this.find_egg, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsFalse);
    this.guard.safe.Enter((StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State.Callback) (smi => smi.RefreshThreat((object) null))).Update("EggProtectionMonitor.safe", (System.Action<EggProtectionMonitor.Instance, float>) ((smi, dt) => smi.RefreshThreat((object) null)), load_balance: true).ToggleStatusItem((string) CREATURES.STATUSITEMS.PROTECTINGENTITY.NAME, (string) CREATURES.STATUSITEMS.PROTECTINGENTITY.TOOLTIP);
    this.guard.threatened.ToggleBehaviour(GameTags.Creatures.Defend, (StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.Transition.ConditionCallback) (smi => smi.threatMonitor.HasThreat()), (System.Action<EggProtectionMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.guard.safe))).Update("Threatened", new System.Action<EggProtectionMonitor.Instance, float>(EggProtectionMonitor.CritterUpdateThreats));
  }

  private static void CritterUpdateThreats(EggProtectionMonitor.Instance smi, float dt)
  {
    if (smi.isMasterNull || smi.threatMonitor.HasThreat())
      return;
    smi.GoTo((StateMachine.BaseState) smi.sm.guard.safe);
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag[] allyTags;
    public string animPrefix;
    public string build = "pincher_build_kanim";
    public FactionManager.FactionID defaultFaction = FactionManager.FactionID.Pest;
    public List<Tag> eggTags = new List<Tag>()
    {
      "CrabEgg".ToTag(),
      "CrabWoodEgg".ToTag(),
      "CrabFreshWaterEgg".ToTag()
    };
  }

  public class GuardEggStates : 
    GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State
  {
    public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State safe;
    public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State threatened;
  }

  public new class Instance : 
    GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.GameInstance
  {
    [MySmiReq]
    public ThreatMonitor.Instance threatMonitor;
    public GameObject eggToProtect;
    private Navigator navigator;
    private System.Action<object> refreshThreatDelegate;
    private static WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>> find_eggs_job = new WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>>();

    public Instance(IStateMachineTarget master, EggProtectionMonitor.Def def)
      : base(master, def)
    {
      this.navigator = master.GetComponent<Navigator>();
      this.refreshThreatDelegate = new System.Action<object>(this.RefreshThreat);
    }

    public void CanProtectEgg()
    {
      bool flag = true;
      if ((UnityEngine.Object) this.eggToProtect == (UnityEngine.Object) null)
        flag = false;
      if (flag)
      {
        int num = 150;
        int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(this.eggToProtect));
        if (navigationCost == -1 || navigationCost >= num)
          flag = false;
      }
      if (flag)
        return;
      this.SetEggToGuard((GameObject) null);
    }

    public static void FindEggToGuard(
      List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry> instances,
      float time_delta)
    {
      if (instances.Count == 0)
        return;
      ListPool<KPrefabID, EggProtectionMonitor>.PooledList pooledList1 = ListPool<KPrefabID, EggProtectionMonitor>.Allocate();
      pooledList1.Capacity = Mathf.Max(pooledList1.Capacity, Components.IncubationMonitors.Count);
      foreach (IncubationMonitor.Instance incubationMonitor in Components.IncubationMonitors)
        pooledList1.Add(incubationMonitor.gameObject.GetComponent<KPrefabID>());
      ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.PooledList eggs = ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.Allocate();
      ListPool<Tag, EggProtectionMonitor>.PooledList eggTags1 = ListPool<Tag, EggProtectionMonitor>.Allocate();
      foreach (UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry instance in instances)
      {
        foreach (Tag eggTag in instance.data.def.eggTags)
        {
          if (!eggTags1.Contains(eggTag))
            eggTags1.Add(eggTag);
        }
      }
      EggProtectionMonitor.Instance.find_eggs_job.Reset((List<KPrefabID>) pooledList1);
      for (int start = 0; start < pooledList1.Count; start += 256 /*0x0100*/)
        EggProtectionMonitor.Instance.find_eggs_job.Add(new EggProtectionMonitor.Instance.FindEggsTask(start, Mathf.Min(start + 256 /*0x0100*/, pooledList1.Count), (List<Tag>) eggTags1));
      GlobalJobManager.Run((IWorkItemCollection) EggProtectionMonitor.Instance.find_eggs_job);
      for (int idx = 0; idx != EggProtectionMonitor.Instance.find_eggs_job.Count; ++idx)
        EggProtectionMonitor.Instance.find_eggs_job.GetWorkItem(idx).Finish((List<KPrefabID>) pooledList1, (List<EggProtectionMonitor.Instance.Egg>) eggs);
      pooledList1.Recycle();
      EggProtectionMonitor.Instance.find_eggs_job.Reset((List<KPrefabID>) null);
      ListPool<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry, EggProtectionMonitor>.PooledList pooledList2 = ListPool<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry, EggProtectionMonitor>.Allocate();
      pooledList2.AddRange((IEnumerable<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry>) instances);
      foreach (UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry entry in (List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry>) pooledList2)
      {
        GameObject egg1 = (GameObject) null;
        int num = 100;
        List<Tag> eggTags2 = entry.data.def.eggTags;
        foreach (EggProtectionMonitor.Instance.Egg egg2 in (List<EggProtectionMonitor.Instance.Egg>) eggs)
        {
          if (eggTags2.Contains(egg2.tag))
          {
            int navigationCost = entry.data.navigator.GetNavigationCost(egg2.cell);
            if (navigationCost != -1 && navigationCost < num)
            {
              egg1 = egg2.game_object;
              num = navigationCost;
            }
          }
        }
        entry.data.SetEggToGuard(egg1);
      }
      pooledList2.Recycle();
      eggTags1.Recycle();
      eggs.Recycle();
    }

    public void SetEggToGuard(GameObject egg)
    {
      this.eggToProtect = egg;
      this.sm.hasEggToGuard.Set((UnityEngine.Object) egg != (UnityEngine.Object) null, this.smi);
    }

    public void GoToThreatened()
    {
      this.smi.GoTo((StateMachine.BaseState) this.sm.guard.threatened);
    }

    public void RefreshThreat(object data)
    {
      if (!this.IsRunning() || (UnityEngine.Object) this.eggToProtect == (UnityEngine.Object) null)
        return;
      if (this.smi.threatMonitor.HasThreat())
      {
        this.GoToThreatened();
      }
      else
      {
        if (this.smi.GetCurrentState() == this.sm.guard.safe)
          return;
        this.Trigger(-21431934);
        this.smi.GoTo((StateMachine.BaseState) this.sm.guard.safe);
      }
    }

    private struct Egg
    {
      public GameObject game_object;
      public int cell;
      public Tag tag;
    }

    private struct FindEggsTask(int start, int end, List<Tag> eggTags) : IWorkItem<List<KPrefabID>>
    {
      private List<Tag> eggTags = eggTags;
      private ListPool<int, EggProtectionMonitor>.PooledList eggs = ListPool<int, EggProtectionMonitor>.Allocate();
      private int start = start;
      private int end = end;

      public void Run(List<KPrefabID> prefab_ids, int threadIndex)
      {
        for (int start = this.start; start != this.end; ++start)
        {
          if (this.eggTags.Contains(prefab_ids[start].PrefabTag))
            this.eggs.Add(start);
        }
      }

      public void Finish(List<KPrefabID> prefab_ids, List<EggProtectionMonitor.Instance.Egg> eggs)
      {
        foreach (int egg in (List<int>) this.eggs)
        {
          KPrefabID prefabId = prefab_ids[egg];
          GameObject gameObject = prefabId.gameObject;
          eggs.Add(new EggProtectionMonitor.Instance.Egg()
          {
            game_object = gameObject,
            cell = Grid.PosToCell(gameObject),
            tag = prefabId.PrefabTag
          });
        }
        this.eggs.Recycle();
      }
    }
  }
}
