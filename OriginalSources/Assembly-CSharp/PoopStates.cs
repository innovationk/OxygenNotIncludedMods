// Decompiled with JetBrains decompiler
// Type: PoopStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PoopStates : 
  GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>
{
  public const float POOP_LOOP_DURATION = 5f;
  public const float ATTEMPT_COOLDOWN = 10f;
  public const int ATTEMPT_TIMES = 3;
  public const string POOP_ANIM_NAME = "poop";
  public const string IDLE_ANIM_NAME = "idle_loop";
  public const string COMPLAIN_ANIM_NAME = "react_neg";
  public const string WAITING_ANIM_NAME = "idle_loop";
  public static Chore.Precondition IsInCooldownPrecondition = new Chore.Precondition()
  {
    id = "IsPoopStateInCooldown",
    sortOrder = 1,
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_POOP_COOLDOWN,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) => !((PoopStates.Instance) data).IsInCooldown)
  };
  public GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State assess;
  public PoopStates.ComplainState complain;
  public PoopStates.PoopOnStationState stationPoop;
  public PoopStates.WildPoopState wildPoop;
  public StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.IntParameter RemainingAttempts = new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.IntParameter(3);
  public StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.TargetParameter PoopStation;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    this.serializable = StateMachine.SerializeType.ParamsOnly;
    default_state = (StateMachine.BaseState) this.assess;
    this.assess.Enter(new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback(PoopStates.FindPoopStation)).EnterTransition((GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) this.stationPoop, new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.Transition.ConditionCallback(PoopStates.AttemptToReservePoopStation)).EnterTransition((GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) this.complain, new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.Transition.ConditionCallback(PoopStates.IsThereAPoopStationNearby)).EnterGoTo((GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) this.wildPoop);
    this.complain.DefaultState(this.complain.complain);
    this.complain.complain.ParamTransition<int>((StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.Parameter<int>) this.RemainingAttempts, (GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) this.wildPoop, GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.IsLTEZero_int).EnterTransition(this.complain.end, new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.Transition.ConditionCallback(PoopStates.CanNotComplain)).ToggleAnims((Func<PoopStates.Instance, KAnimFile>) (smi => smi.def.emoteAnimFile)).Enter(new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback(PoopStates.DisplayThoughtBubble)).PlayAnim("react_neg").OnAnimQueueComplete(this.complain.end).Exit(new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback(PoopStates.ClearThoughtBubble));
    this.complain.end.Enter(new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback(PoopStates.ConsumeAttempt)).EnterGoTo((GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) null);
    GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State state = this.stationPoop.ParamTransition<GameObject>((StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.Parameter<GameObject>) this.PoopStation, (GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) this.wildPoop, GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.IsNull);
    StatusItemCategory main1 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay1 = new HashedString();
    StatusItemCategory category1 = main1;
    state.ToggleStatusItem("Unused", "Unused", render_overlay: render_overlay1, resolve_string_callback: (Func<string, PoopStates.Instance, string>) ((str, smi) => smi.def.statusItemName), resolve_tooltip_callback: (Func<string, PoopStates.Instance, string>) ((str, smi) => smi.def.statusItemTooltip), category: category1).DefaultState(this.stationPoop.approachPoopSpot).EventHandlerTransition(GameHashes.PoopStationUpdate, (GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) this.wildPoop, new Func<PoopStates.Instance, object, bool>(PoopStates.IsPoopStationStillValid)).Exit(new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback(PoopStates.ClearReservationFromPoopStation));
    this.stationPoop.approachPoopSpot.MoveTo(new Func<PoopStates.Instance, int>(PoopStates.GetPoopStationCell), (GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) this.stationPoop.pooping, (GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State) this.wildPoop);
    this.stationPoop.pooping.DefaultState(this.stationPoop.pooping.pre);
    this.stationPoop.pooping.pre.EnterTransition(this.stationPoop.pooping.loop, (StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.Transition.ConditionCallback) (smi => PoopStates.GetPoopStationPoop_PRE_AnimName(smi) == null)).Enter((StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback) (smi => PoopStates.PlayAnimOnStation(smi, PoopStates.GetPoopStationPoop_PRE_AnimName(smi), KAnim.PlayMode.Once))).PlayAnim(new Func<PoopStates.Instance, string>(PoopStates.GetPoopStationPoop_PRE_AnimName)).OnAnimQueueComplete(this.stationPoop.pooping.loop);
    this.stationPoop.pooping.loop.EnterTransition(this.stationPoop.pooping.pst, (StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.Transition.ConditionCallback) (smi => PoopStates.GetPoopStationPoop_LOOP_AnimName(smi) == null)).Enter((StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback) (smi => PoopStates.PlayAnimOnStation(smi, PoopStates.GetPoopStationPoop_LOOP_AnimName(smi), KAnim.PlayMode.Loop))).PlayAnim(new Func<PoopStates.Instance, string>(PoopStates.GetPoopStationPoop_LOOP_AnimName), KAnim.PlayMode.Loop).ScheduleGoTo(5f, (StateMachine.BaseState) this.stationPoop.pooping.pst);
    this.stationPoop.pooping.pst.EnterTransition(this.stationPoop.end, (StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.Transition.ConditionCallback) (smi => PoopStates.GetPoopStationPoop_PST_AnimName(smi) == null)).Enter((StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback) (smi => PoopStates.PlayAnimOnStation(smi, PoopStates.GetPoopStationPoop_PST_AnimName(smi), KAnim.PlayMode.Once))).PlayAnim(new Func<PoopStates.Instance, string>(PoopStates.GetPoopStationPoop_PST_AnimName)).OnAnimQueueComplete(this.stationPoop.end);
    this.stationPoop.end.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback(PoopStates.ResetAttempts)).TriggerOnEnter(GameHashes.PoopStatesCompleted, new Func<PoopStates.Instance, object>(PoopStates.GetPoopData)).BehaviourComplete(GameTags.Creatures.Poop);
    PoopStates.WildPoopState wildPoop = this.wildPoop;
    StatusItemCategory main2 = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay2 = new HashedString();
    StatusItemCategory category2 = main2;
    wildPoop.ToggleStatusItem("Unused", "Unused", render_overlay: render_overlay2, resolve_string_callback: (Func<string, PoopStates.Instance, string>) ((str, smi) => smi.def.statusItemName), resolve_tooltip_callback: (Func<string, PoopStates.Instance, string>) ((str, smi) => smi.def.statusItemTooltip), category: category2).DefaultState(this.wildPoop.pooping);
    this.wildPoop.pooping.PlayAnim("poop", KAnim.PlayMode.Once).OnAnimQueueComplete(this.wildPoop.end);
    this.wildPoop.end.PlayAnim("idle_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State.Callback(PoopStates.ResetAttempts)).TriggerOnEnter(GameHashes.PoopStatesCompleted, (Func<PoopStates.Instance, object>) (smi => (object) null)).BehaviourComplete(GameTags.Creatures.Poop);
  }

  private static void DisplayThoughtBubble(PoopStates.Instance smi)
  {
    Tuple<Sprite, Color> uiSprite = global::Def.GetUISprite((object) smi.PoopStationObject);
    NameDisplayScreen.Instance.SetThoughtBubbleDisplay(smi.gameObject, true, "", Assets.GetSprite((HashedString) "bubble_alert"), uiSprite.first);
  }

  private static void ClearThoughtBubble(PoopStates.Instance smi)
  {
    NameDisplayScreen.Instance.SetThoughtBubbleDisplay(smi.gameObject, false, (string) null, (Sprite) null, (Sprite) null);
  }

  private static int GetPoopStationCell(PoopStates.Instance smi) => smi.GetPoopStationCell();

  private static bool IsThereAPoopStationNearby(PoopStates.Instance smi)
  {
    return (UnityEngine.Object) smi.PoopStationObject != (UnityEngine.Object) null;
  }

  private static bool AttemptToReservePoopStation(PoopStates.Instance smi)
  {
    return smi.AttemptToReservePoopStation();
  }

  private static bool IsPoopStationStillValid(PoopStates.Instance smi, object o)
  {
    return smi.IsPoopStationStillValid();
  }

  private static bool CanNotComplain(PoopStates.Instance smi) => !smi.def.canComplain;

  private static void ConsumeAttempt(PoopStates.Instance smi) => smi.ConsumeAttempt();

  private static void ResetAttempts(PoopStates.Instance smi) => smi.ResetAttempt();

  private static void FindPoopStation(PoopStates.Instance smi) => smi.FindPoopStation();

  private static void PlayAnimOnStation(
    PoopStates.Instance smi,
    string animName,
    KAnim.PlayMode playmode)
  {
    smi.PlayAnimOnStation(animName, playmode);
  }

  private static void ClearReservationFromPoopStation(PoopStates.Instance smi)
  {
    smi.ClearReservationFromPoopStation();
  }

  private static PoopData GetPoopData(PoopStates.Instance smi) => smi.GetPoopData();

  private static string GetPoopStationPoop_PRE_AnimName(PoopStates.Instance smi)
  {
    return smi.GetPoopStationAnimName(0) ?? "poop";
  }

  private static string GetPoopStationPoop_LOOP_AnimName(PoopStates.Instance smi)
  {
    return smi.GetPoopStationAnimName(1);
  }

  private static string GetPoopStationPoop_PST_AnimName(PoopStates.Instance smi)
  {
    return smi.GetPoopStationAnimName(2);
  }

  public class Def : StateMachine.BaseDef
  {
    public KAnimFile emoteAnimFile;
    public string statusItemName;
    public string statusItemTooltip;
    public bool canComplain;

    public Def(
      KAnimFile emoteAnimFile,
      string status_item_name,
      string status_item_tooltip,
      bool canComplain)
    {
      this.canComplain = canComplain;
      this.emoteAnimFile = emoteAnimFile;
      this.statusItemName = status_item_name;
      this.statusItemTooltip = status_item_tooltip;
    }
  }

  public class PoopOnStationState : 
    GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State
  {
    public GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State approachPoopSpot;
    public GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.PreLoopPostState pooping;
    public GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State end;
  }

  public class ComplainState : 
    GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State
  {
    public GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State complain;
    public GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State end;
  }

  public class WildPoopState : 
    GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State
  {
    public GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State pooping;
    public GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.State end;
  }

  public new class Instance : 
    GameStateMachine<PoopStates, PoopStates.Instance, IStateMachineTarget, PoopStates.Def>.GameInstance
  {
    private KPrefabID prefabID;
    private Navigator navigator;
    private float lastTimeWeAttemptedToGo = -10f;

    public bool IsInCooldown => (double) this.TimePassedSinceLastAttempt < 10.0;

    public float TimePassedSinceLastAttempt => Time.time - this.lastTimeWeAttemptedToGo;

    public GameObject PoopStationObject => this.sm.PoopStation.Get(this);

    public IPoopStation PoopStation
    {
      get
      {
        if ((UnityEngine.Object) this.PoopStationObject == (UnityEngine.Object) null)
          return (IPoopStation) null;
        IPoopStation component = this.PoopStationObject.GetComponent<IPoopStation>();
        return component == null ? this.PoopStationObject.GetSMI<IPoopStation>() : component;
      }
    }

    public Instance(Chore<PoopStates.Instance> chore, PoopStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      this.prefabID = this.GetComponent<KPrefabID>();
      this.navigator = this.GetComponent<Navigator>();
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.Poop);
      chore.AddPrecondition(PoopStates.IsInCooldownPrecondition, (object) this);
    }

    public void ConsumeAttempt()
    {
      this.lastTimeWeAttemptedToGo = Time.time;
      this.sm.RemainingAttempts.Set(this.sm.RemainingAttempts.Get(this.smi) - 1, this);
    }

    public void ResetAttempt()
    {
      this.lastTimeWeAttemptedToGo = -10f;
      this.sm.RemainingAttempts.Set(3, this);
    }

    public int GetPoopStationCell()
    {
      return (UnityEngine.Object) this.PoopStationObject == (UnityEngine.Object) null ? Grid.InvalidCell : Grid.PosToCell(this.PoopStationObject);
    }

    public bool IsPoopStationStillValid()
    {
      IPoopStation poopStation = this.PoopStation;
      return poopStation != null && poopStation.IsPoopStationOperational() && (UnityEngine.Object) poopStation.GetCurrentPoopStationUser() == (UnityEngine.Object) this.gameObject;
    }

    public bool AttemptToReservePoopStation()
    {
      IPoopStation poopStation = this.PoopStation;
      return poopStation != null && poopStation.AttemptToReservePoopStation(this.gameObject);
    }

    public bool IsPoopStationOperational()
    {
      IPoopStation poopStation = this.PoopStation;
      return poopStation != null && poopStation.IsPoopStationOperational();
    }

    public void ClearReservationFromPoopStation()
    {
      this.PoopStation?.ClearPoopStationUser(this.gameObject);
    }

    public void PlayAnimOnStation(string animName, KAnim.PlayMode playMode)
    {
      this.PoopStation?.PlayPoopStationAnim(animName, playMode);
    }

    public string GetPoopStationAnimName(int index)
    {
      IPoopStation poopStation = this.PoopStation;
      if (poopStation == null)
        return (string) null;
      string[] poopingAnimNames = poopStation.GetPoopingAnimNames();
      return poopingAnimNames == null || index >= poopingAnimNames.Length ? (string) null : poopingAnimNames[index];
    }

    public PoopData GetPoopData() => this.PoopStation?.GetPoopData();

    public void FindPoopStation()
    {
      IPoopStation poopStation1 = (IPoopStation) null;
      bool flag1 = false;
      int num1 = (UnityEngine.Object) this.navigator == (UnityEngine.Object) null ? 32 /*0x20*/ : this.navigator.maxProbeRadiusX;
      int myWorldId = this.gameObject.GetMyWorldId();
      int cell1 = Grid.PosToCell(this.gameObject);
      List<IPoopStation> items = Components.PoopStations.GetItems(myWorldId);
      int num2 = -1;
      float num3 = -1f;
      foreach (IPoopStation poopStation2 in items)
      {
        if (poopStation2.IsUserCompatibleWithPoopStation(this.prefabID))
        {
          bool flag2 = poopStation2.IsPoopStationOperational();
          int cell2 = Grid.PosToCell(poopStation2.GetPoopStationObject());
          if (Grid.GetCellDistance(cell1, cell2) <= num1 && flag2)
          {
            int navigationCost = this.navigator.GetNavigationCost(cell2);
            if (navigationCost != -1)
            {
              float capacityPercentage = poopStation2.GetAvailablePoopCapacityPercentage();
              bool flag3 = (double) capacityPercentage > (double) num3;
              if (num2 == -1 | flag3 || navigationCost < num2 && (double) capacityPercentage == (double) num3)
              {
                GameObject currentPoopStationUser = poopStation2.GetCurrentPoopStationUser();
                bool flag4 = (UnityEngine.Object) currentPoopStationUser == (UnityEngine.Object) null || (UnityEngine.Object) currentPoopStationUser == (UnityEngine.Object) this.gameObject;
                if (((poopStation1 == null ? 1 : (!flag1 ? 1 : 0)) | (flag4 ? 1 : 0)) != 0)
                {
                  num2 = navigationCost;
                  poopStation1 = poopStation2;
                  num3 = capacityPercentage;
                  flag1 = flag4;
                }
              }
            }
          }
        }
      }
      this.sm.PoopStation.Set(poopStation1 == null ? (GameObject) null : poopStation1.GetPoopStationObject(), this, false);
    }
  }
}
