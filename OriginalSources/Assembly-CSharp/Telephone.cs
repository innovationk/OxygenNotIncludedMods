// Decompiled with JetBrains decompiler
// Type: Telephone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Telephone : StateMachineComponent<Telephone.StatesInstance>, IGameObjectEffectDescriptor
{
  public string babbleEffect;
  public string chatEffect;
  public string longDistanceEffect;
  public string trackingEffect;
  public bool isInUse;
  public bool wasAnswered;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    Components.Telephones.Add(this);
    GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, (Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule)), (object) null, (SchedulerGroup) null);
  }

  protected override void OnCleanUp()
  {
    Components.Telephones.Remove(this);
    base.OnCleanUp();
  }

  public void AddModifierDescriptions(List<Descriptor> descs, string effect_id)
  {
    Effect effect = Db.Get().effects.Get(effect_id);
    string str1;
    string str2;
    if (effect.Id == this.babbleEffect)
    {
      str1 = (string) BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE;
      str2 = (string) BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE_TOOLTIP;
    }
    else if (effect.Id == this.chatEffect)
    {
      str1 = (string) BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT;
      str2 = (string) BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT_TOOLTIP;
    }
    else
    {
      str1 = (string) BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE;
      str2 = (string) BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE_TOOLTIP;
    }
    foreach (AttributeModifier selfModifier in effect.SelfModifiers)
    {
      Descriptor descriptor = new Descriptor(str1.Replace("{attrib}", (string) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{selfModifier.AttributeId.ToUpper()}.NAME")).Replace("{amount}", selfModifier.GetFormattedString()), str2.Replace("{attrib}", (string) Strings.Get($"STRINGS.DUPLICANTS.ATTRIBUTES.{selfModifier.AttributeId.ToUpper()}.NAME")).Replace("{amount}", selfModifier.GetFormattedString()));
      descriptor.IncreaseIndent();
      descs.Add(descriptor);
    }
  }

  List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
  {
    List<Descriptor> descs = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.RECREATION, (string) UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION);
    descs.Add(descriptor);
    this.AddModifierDescriptions(descs, this.babbleEffect);
    this.AddModifierDescriptions(descs, this.chatEffect);
    this.AddModifierDescriptions(descs, this.longDistanceEffect);
    return descs;
  }

  public void HangUp()
  {
    this.isInUse = false;
    this.wasAnswered = false;
    this.RemoveTag(GameTags.LongDistanceCall);
  }

  public class States : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone>
  {
    private GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State unoperational;
    private Telephone.States.ReadyStates ready;
    private static StatusItem partyLine;
    private static StatusItem babbling;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      Telephone.States.CreateStatusItems();
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, (GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State) this.ready);
      this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleRecurringChore(new Func<Telephone.StatesInstance, Chore>(this.CreateChore)).Enter((StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State.Callback) (smi =>
      {
        foreach (Telephone telephone in Components.Telephones.Items)
        {
          if (telephone.isInUse)
            smi.GoTo((StateMachine.BaseState) this.ready.speaker);
        }
      }));
      this.ready.idle.WorkableStartTransition((Func<Telephone.StatesInstance, Workable>) (smi => (Workable) smi.master.GetComponent<TelephoneCallerWorkable>()), this.ready.calling.dial).TagTransition(GameTags.TelephoneRinging, this.ready.ringing).PlayAnim("off");
      this.ready.calling.ScheduleGoTo(15f, (StateMachine.BaseState) this.ready.talking.babbling);
      this.ready.calling.dial.PlayAnim("on_pre").OnAnimQueueComplete(this.ready.calling.animHack);
      this.ready.calling.animHack.ScheduleActionNextFrame("animHack_delay", (Action<Telephone.StatesInstance>) (smi => smi.GoTo((StateMachine.BaseState) this.ready.calling.pre)));
      this.ready.calling.pre.PlayAnim("on").Enter((StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State.Callback) (smi => this.RingAllTelephones(smi))).OnAnimQueueComplete(this.ready.calling.wait);
      this.ready.calling.wait.PlayAnim("on", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.Transition.ConditionCallback) (smi => smi.CallAnswered()), UpdateRate.SIM_4000ms);
      this.ready.ringing.PlayAnim("on_receiving", KAnim.PlayMode.Loop).Transition(this.ready.answer, (StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Telephone>().isInUse), UpdateRate.SIM_33ms).TagTransition(GameTags.TelephoneRinging, this.ready.speaker, true).ScheduleGoTo(15f, (StateMachine.BaseState) this.ready.speaker).Exit((StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State.Callback) (smi => smi.GetComponent<Telephone>().RemoveTag(GameTags.TelephoneRinging)));
      this.ready.answer.PlayAnim("on_pre_loop_receiving").OnAnimQueueComplete(this.ready.talking.chatting);
      this.ready.talking.ScheduleGoTo(25f, (StateMachine.BaseState) this.ready.hangup).Enter((StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State.Callback) (smi => this.UpdatePartyLine(smi)));
      this.ready.talking.babbling.PlayAnim("on_loop", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.Transition.ConditionCallback) (smi => smi.CallAnswered()), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.babbling);
      this.ready.talking.chatting.PlayAnim("on_loop_pre").QueueAnim("on_loop", true).Transition(this.ready.talking.babbling, (StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.Transition.ConditionCallback) (smi => !smi.CallAnswered()), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.partyLine);
      this.ready.speaker.PlayAnim("on_loop_nobody", KAnim.PlayMode.Loop).Transition((GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State) this.ready, (StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.Transition.ConditionCallback) (smi => !smi.CallAnswered()), UpdateRate.SIM_4000ms).Transition(this.ready.answer, (StateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Telephone>().isInUse), UpdateRate.SIM_33ms);
      this.ready.hangup.OnAnimQueueComplete((GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State) this.ready);
    }

    private Chore CreateChore(Telephone.StatesInstance smi)
    {
      Workable component = (Workable) smi.master.GetComponent<TelephoneCallerWorkable>();
      WorkChore<TelephoneCallerWorkable> chore = new WorkChore<TelephoneCallerWorkable>(Db.Get().ChoreTypes.Relax, (IStateMachineTarget) component, allow_in_red_alert: false, schedule_block: Db.Get().ScheduleBlockTypes.Recreation, allow_prioritization: false, priority_class: PriorityScreen.PriorityClass.high);
      chore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, (object) component);
      return (Chore) chore;
    }

    public void UpdatePartyLine(Telephone.StatesInstance smi)
    {
      int myWorldId = smi.GetMyWorldId();
      bool flag = false;
      foreach (Telephone telephone in Components.Telephones.Items)
      {
        telephone.RemoveTag(GameTags.TelephoneRinging);
        if (telephone.isInUse && myWorldId != telephone.GetMyWorldId())
        {
          flag = true;
          telephone.AddTag(GameTags.LongDistanceCall);
        }
      }
      Telephone component = smi.GetComponent<Telephone>();
      component.RemoveTag(GameTags.TelephoneRinging);
      if (!flag)
        return;
      component.AddTag(GameTags.LongDistanceCall);
    }

    public void RingAllTelephones(Telephone.StatesInstance smi)
    {
      Telephone component1 = smi.master.GetComponent<Telephone>();
      foreach (Telephone cmp in Components.Telephones.Items)
      {
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) cmp && cmp.GetComponent<Operational>().IsOperational)
        {
          TelephoneCallerWorkable component2 = cmp.GetComponent<TelephoneCallerWorkable>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) component2.worker == (UnityEngine.Object) null)
            cmp.AddTag(GameTags.TelephoneRinging);
        }
      }
    }

    private static void CreateStatusItems()
    {
      if (Telephone.States.partyLine == null)
      {
        Telephone.States.partyLine = new StatusItem("PartyLine", (string) BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO, "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
        Telephone.States.partyLine.resolveStringCallback = (Func<string, object, string>) ((str, obj) =>
        {
          Telephone component1 = ((StateMachine.Instance) obj).GetComponent<Telephone>();
          int num = 0;
          foreach (Telephone component2 in Components.Telephones.Items)
          {
            if (component2.isInUse && (UnityEngine.Object) component2 != (UnityEngine.Object) component1)
            {
              ++num;
              if (num == 1)
              {
                str = str.Replace("{Asteroid}", component2.GetMyWorld().GetProperName());
                str = str.Replace("{Duplicant}", component2.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
              }
            }
          }
          if (num > 1)
            str = string.Format((string) BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO_NUM, (object) num);
          return str;
        });
        Telephone.States.partyLine.resolveTooltipCallback = (Func<string, object, string>) ((str, obj) =>
        {
          Telephone component3 = ((StateMachine.Instance) obj).GetComponent<Telephone>();
          foreach (Telephone component4 in Components.Telephones.Items)
          {
            if (component4.isInUse && (UnityEngine.Object) component4 != (UnityEngine.Object) component3)
            {
              string str1 = ((string) BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO).Replace("{Duplicant}", component4.GetComponent<TelephoneCallerWorkable>().worker.GetProperName()).Replace("{Asteroid}", component4.GetMyWorld().GetProperName());
              str = $"{str}{str1}\n";
            }
          }
          return str;
        });
      }
      if (Telephone.States.babbling != null)
        return;
      Telephone.States.babbling = new StatusItem("Babbling", (string) BUILDING.STATUSITEMS.TELEPHONE.BABBLE.NAME, (string) BUILDING.STATUSITEMS.TELEPHONE.BABBLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
      Telephone.States.babbling.resolveTooltipCallback = (Func<string, object, string>) ((str, obj) =>
      {
        Telephone.StatesInstance statesInstance = (Telephone.StatesInstance) obj;
        str = str.Replace("{Duplicant}", statesInstance.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
        return str;
      });
    }

    public class ReadyStates : 
      GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
    {
      public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State idle;
      public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State ringing;
      public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State answer;
      public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State speaker;
      public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State hangup;
      public Telephone.States.ReadyStates.CallingStates calling;
      public Telephone.States.ReadyStates.TalkingStates talking;

      public class CallingStates : 
        GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
      {
        public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State dial;
        public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State animHack;
        public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State pre;
        public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State wait;
      }

      public class TalkingStates : 
        GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
      {
        public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State babbling;
        public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State chatting;
      }
    }
  }

  public class StatesInstance(Telephone smi) : 
    GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.GameInstance(smi)
  {
    public bool CallAnswered()
    {
      foreach (Telephone telephone in Components.Telephones.Items)
      {
        if (telephone.isInUse && (UnityEngine.Object) telephone != (UnityEngine.Object) this.smi.GetComponent<Telephone>())
        {
          telephone.wasAnswered = true;
          return true;
        }
      }
      return false;
    }

    public bool CallEnded()
    {
      foreach (Telephone telephone in Components.Telephones.Items)
      {
        if (telephone.isInUse)
          return false;
      }
      return true;
    }
  }
}
