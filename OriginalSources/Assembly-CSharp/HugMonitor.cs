// Decompiled with JetBrains decompiler
// Type: HugMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class HugMonitor : 
  GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>
{
  private static string soundPath = GlobalAssets.GetSound("Squirrel_hug_frenzyFX");
  private static Effect hugEffect;
  private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugFrenzyTimer;
  private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter wantsHugCooldownTimer;
  private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugEggCooldownTimer;
  public HugMonitor.NormalStates normal;
  public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State hugFrenzy;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.normal;
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    this.root.Update(new System.Action<HugMonitor.Instance, float>(this.UpdateHugEggCooldownTimer), UpdateRate.SIM_1000ms).ToggleBehaviour(GameTags.Creatures.WantsToTendEgg, (StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.Transition.ConditionCallback) (smi => smi.UpdateHasTarget()), (System.Action<HugMonitor.Instance>) (smi => smi.hugTarget = (KPrefabID) null));
    this.normal.DefaultState(this.normal.idle).ParamTransition<float>((StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.Parameter<float>) this.hugFrenzyTimer, this.hugFrenzy, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsGTZero);
    this.normal.idle.ParamTransition<float>((StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.Parameter<float>) this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new System.Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms);
    this.normal.hugReady.ToggleReactable(new Func<HugMonitor.Instance, Reactable>(this.GetHugReactable));
    GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State state = this.normal.hugReady.passiveHug.ParamTransition<float>((StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.Parameter<float>) this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new System.Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms);
    string name = (string) CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.normal.hugReady.seekingHug.ToggleBehaviour(GameTags.Creatures.WantsAHug, (StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.Transition.ConditionCallback) (smi => true), (System.Action<HugMonitor.Instance>) (smi =>
    {
      double num = (double) this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldownFailed, smi);
      smi.GoTo((StateMachine.BaseState) this.normal.hugReady.passiveHug);
    }));
    this.hugFrenzy.ParamTransition<float>((StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.Parameter<float>) this.hugFrenzyTimer, (GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State) this.normal, (StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.Parameter<float>.Callback) ((smi, p) => (double) p <= 0.0 && !smi.IsHugging())).Update(new System.Action<HugMonitor.Instance, float>(this.UpdateHugFrenzyTimer), UpdateRate.SIM_1000ms).ToggleEffect((Func<HugMonitor.Instance, Effect>) (smi => smi.frenzyEffect)).ToggleLoopingSound(HugMonitor.soundPath).Enter((StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State.Callback) (smi =>
    {
      smi.hugParticleFx = Util.KInstantiate(EffectPrefabs.Instance.HugFrenzyFX, smi.master.transform.GetPosition() + smi.hugParticleOffset);
      smi.hugParticleFx.transform.SetParent(smi.master.transform);
      smi.hugParticleFx.SetActive(true);
    })).Exit((StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State.Callback) (smi =>
    {
      Util.KDestroyGameObject(smi.hugParticleFx);
      double num = (double) this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldown, smi);
    }));
  }

  private Reactable GetHugReactable(HugMonitor.Instance smi)
  {
    return (Reactable) new HugMinionReactable(smi.gameObject);
  }

  private void UpdateWantsHugCooldownTimer(HugMonitor.Instance smi, float dt)
  {
    double num = (double) this.wantsHugCooldownTimer.DeltaClamp(-dt, 0.0f, float.MaxValue, smi);
  }

  private void UpdateHugEggCooldownTimer(HugMonitor.Instance smi, float dt)
  {
    double num = (double) this.hugEggCooldownTimer.DeltaClamp(-dt, 0.0f, float.MaxValue, smi);
  }

  private void UpdateHugFrenzyTimer(HugMonitor.Instance smi, float dt)
  {
    double num = (double) this.hugFrenzyTimer.DeltaClamp(-dt, 0.0f, float.MaxValue, smi);
  }

  public class HUGTUNING
  {
    public const float HUG_EGG_TIME = 15f;
    public const float HUG_DUPE_WAIT = 60f;
    public const float FRENZY_EGGS_PER_CYCLE = 6f;
    public const float FRENZY_EGG_TRAVEL_TIME_BUFFER = 5f;
    public const float HUG_FRENZY_DURATION = 120f;
  }

  public class Def : StateMachine.BaseDef
  {
    public float hugsPerCycle = 2f;
    public float scanningInterval = 30f;
    public float hugFrenzyDuration = 120f;
    public float hugFrenzyCooldown = 480f;
    public float hugFrenzyCooldownFailed = 120f;
    public float scanningIntervalFrenzy = 15f;
    public int maxSearchCost = 30;
  }

  public class HugReadyStates : 
    GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
  {
    public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State passiveHug;
    public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State seekingHug;
  }

  public class NormalStates : 
    GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
  {
    public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State idle;
    public HugMonitor.HugReadyStates hugReady;
  }

  public new class Instance : 
    GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.GameInstance
  {
    public GameObject hugParticleFx;
    public Vector3 hugParticleOffset;
    public Effect frenzyEffect;
    public KPrefabID hugTarget;
    [MyCmpGet]
    private Navigator navigator;

    public Instance(IStateMachineTarget master, HugMonitor.Def def)
      : base(master, def)
    {
      this.frenzyEffect = Db.Get().effects.Get("HuggingFrenzy");
      this.RefreshSearchTime();
      if (HugMonitor.hugEffect == null)
        HugMonitor.hugEffect = Db.Get().effects.Get("EggHug");
      double num = (double) this.smi.sm.wantsHugCooldownTimer.Set(UnityEngine.Random.Range(this.smi.def.hugFrenzyCooldownFailed, this.smi.def.hugFrenzyCooldown), this.smi);
    }

    private void RefreshSearchTime()
    {
      if ((UnityEngine.Object) this.hugTarget == (UnityEngine.Object) null)
      {
        double num1 = (double) this.smi.sm.hugEggCooldownTimer.Set(this.GetScanningInterval(), this.smi);
      }
      else
      {
        double num2 = (double) this.smi.sm.hugEggCooldownTimer.Set(this.GetHugInterval(), this.smi);
      }
    }

    private float GetScanningInterval()
    {
      return !this.IsHuggingFrenzy() ? this.def.scanningInterval : this.def.scanningIntervalFrenzy;
    }

    private float GetHugInterval() => this.IsHuggingFrenzy() ? 0.0f : 600f / this.def.hugsPerCycle;

    public bool IsHuggingFrenzy() => this.smi.GetCurrentState() == this.smi.sm.hugFrenzy;

    public bool IsHugging() => this.smi.GetSMI<AnimInterruptMonitor.Instance>().anims != null;

    public bool UpdateHasTarget()
    {
      if ((UnityEngine.Object) this.hugTarget == (UnityEngine.Object) null)
      {
        if ((double) this.smi.sm.hugEggCooldownTimer.Get(this.smi) > 0.0)
          return false;
        this.FindEgg();
        this.RefreshSearchTime();
      }
      return (UnityEngine.Object) this.hugTarget != (UnityEngine.Object) null;
    }

    public void EnterHuggingFrenzy()
    {
      double num1 = (double) this.smi.sm.hugFrenzyTimer.Set(this.smi.def.hugFrenzyDuration, this.smi);
      double num2 = (double) this.smi.sm.hugEggCooldownTimer.Set(0.0f, this.smi);
    }

    private void FindEgg()
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(this.gameObject));
      int num = this.def.maxSearchCost;
      this.hugTarget = (KPrefabID) null;
      if (cavityForCell == null)
        return;
      foreach (KPrefabID egg in cavityForCell.eggs)
      {
        KPrefabID cmp = egg;
        if (!cmp.HasTag(GameTags.Creatures.ReservedByCreature) && !cmp.GetComponent<Effects>().HasEffect(HugMonitor.hugEffect))
        {
          int cell = Grid.PosToCell((KMonoBehaviour) cmp);
          if (cmp.HasTag(GameTags.Stored))
          {
            GameObject go;
            KPrefabID component;
            if (Grid.ObjectLayers[1].TryGetValue(cell, out go) && go.TryGetComponent<KPrefabID>(out component) && component.IsPrefabID((Tag) "EggIncubator"))
            {
              cell = Grid.PosToCell(go);
              cmp = component;
            }
            else
              continue;
          }
          int navigationCost = this.navigator.GetNavigationCost(cell);
          if (navigationCost != -1 && navigationCost < num)
          {
            this.hugTarget = cmp;
            num = navigationCost;
          }
        }
      }
    }
  }
}
