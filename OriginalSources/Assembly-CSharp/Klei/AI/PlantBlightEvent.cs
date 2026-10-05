// Decompiled with JetBrains decompiler
// Type: Klei.AI.PlantBlightEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Klei.AI;

public class PlantBlightEvent : GameplayEvent<PlantBlightEvent.StatesInstance>
{
  private const float BLIGHT_DISTANCE = 6f;
  public string targetPlantPrefab;
  public float infectionDuration;
  public float incubationDuration;

  public PlantBlightEvent(
    string id,
    string targetPlantPrefab,
    float infectionDuration,
    float incubationDuration)
    : base(id)
  {
    this.targetPlantPrefab = targetPlantPrefab;
    this.infectionDuration = infectionDuration;
    this.incubationDuration = incubationDuration;
    this.title = (string) GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.NAME;
    this.description = (string) GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.DESCRIPTION;
  }

  public override StateMachine.Instance GetSMI(
    GameplayEventManager manager,
    GameplayEventInstance eventInstance)
  {
    return (StateMachine.Instance) new PlantBlightEvent.StatesInstance(manager, eventInstance, this);
  }

  public class States : 
    GameplayEventStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, PlantBlightEvent>
  {
    public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State planning;
    public PlantBlightEvent.States.RunningStates running;
    public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State finished;
    public StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.Signal doFinish;
    public StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.FloatParameter nextInfection;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      base.InitializeStates(out default_state);
      default_state = (StateMachine.BaseState) this.planning;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.planning.Enter((StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.InfectAPlant(true))).GoTo((GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State) this.running);
      this.running.ToggleNotification((Func<PlantBlightEvent.StatesInstance, Notification>) (smi => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi)))).EventHandlerTransition(GameHashes.Uprooted, this.finished, new Func<PlantBlightEvent.StatesInstance, object, bool>(this.NoBlightedPlants)).DefaultState(this.running.waiting).OnSignal(this.doFinish, this.finished);
      double num;
      this.running.waiting.ParamTransition<float>((StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.Parameter<float>) this.nextInfection, this.running.infect, (StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.Parameter<float>.Callback) ((smi, p) => (double) p <= 0.0)).Update((Action<PlantBlightEvent.StatesInstance, float>) ((smi, dt) => num = (double) this.nextInfection.Delta(-dt, smi)), UpdateRate.SIM_4000ms);
      this.running.infect.Enter((StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State.Callback) (smi => smi.InfectAPlant(false))).GoTo(this.running.waiting);
      this.finished.DoNotification((Func<PlantBlightEvent.StatesInstance, Notification>) (smi => this.CreateSuccessNotification(smi, this.GenerateEventPopupData(smi)))).ReturnSuccess();
    }

    public override EventInfoData GenerateEventPopupData(PlantBlightEvent.StatesInstance smi)
    {
      EventInfoData eventPopupData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
      string str = smi.gameplayEvent.targetPlantPrefab.ToTag().ProperName();
      eventPopupData.location = (string) GAMEPLAY_EVENTS.LOCATIONS.COLONY_WIDE;
      eventPopupData.whenDescription = (string) GAMEPLAY_EVENTS.TIMES.NOW;
      eventPopupData.SetTextParameter("plant", str);
      return eventPopupData;
    }

    private Notification CreateSuccessNotification(
      PlantBlightEvent.StatesInstance smi,
      EventInfoData eventInfoData)
    {
      string plantName = smi.gameplayEvent.targetPlantPrefab.ToTag().ProperName();
      return new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.SUCCESS.Replace("{plant}", plantName), NotificationType.Neutral, (Func<List<Notification>, object, string>) ((list, data) => GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.SUCCESS_TOOLTIP.Replace("{plant}", plantName)));
    }

    private bool NoBlightedPlants(PlantBlightEvent.StatesInstance smi, object obj)
    {
      GameObject go = (GameObject) obj;
      if (!go.HasTag(GameTags.Blighted))
        return true;
      foreach (Crop cmp in Components.Crops.Items.FindAll((Predicate<Crop>) (p => p.name == smi.gameplayEvent.targetPlantPrefab)))
      {
        if (!((UnityEngine.Object) go.gameObject == (UnityEngine.Object) cmp.gameObject) && cmp.HasTag(GameTags.Blighted))
          return false;
      }
      return true;
    }

    public class RunningStates : 
      GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State
    {
      public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State waiting;
      public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State infect;
    }
  }

  public class StatesInstance : 
    GameplayEventStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, PlantBlightEvent>.GameplayEventStateMachineInstance
  {
    [Serialize]
    private float startTime;

    public StatesInstance(
      GameplayEventManager master,
      GameplayEventInstance eventInstance,
      PlantBlightEvent blightEvent)
      : base(master, eventInstance, blightEvent)
    {
      this.startTime = Time.time;
    }

    public void InfectAPlant(bool initialInfection)
    {
      if ((double) Time.time - (double) this.startTime > (double) this.smi.gameplayEvent.infectionDuration)
      {
        this.sm.doFinish.Trigger(this.smi);
      }
      else
      {
        List<Crop> all = Components.Crops.Items.FindAll((Predicate<Crop>) (p => p.name == this.smi.gameplayEvent.targetPlantPrefab));
        if (all.Count == 0)
        {
          this.sm.doFinish.Trigger(this.smi);
        }
        else
        {
          if (all.Count > 0)
          {
            List<Crop> cropList = new List<Crop>();
            List<Crop> list = new List<Crop>();
            foreach (Crop cmp in all)
            {
              if (cmp.HasTag(GameTags.Blighted))
                cropList.Add(cmp);
              else
                list.Add(cmp);
            }
            if (cropList.Count == 0)
            {
              if (initialInfection)
              {
                Crop context = list[UnityEngine.Random.Range(0, list.Count)];
                Debug.Log((object) "Blighting a random plant", (UnityEngine.Object) context);
                context.GetComponent<BlightVulnerable>().MakeBlighted();
              }
              else
                this.sm.doFinish.Trigger(this.smi);
            }
            else if (list.Count > 0)
            {
              Crop context = cropList[UnityEngine.Random.Range(0, cropList.Count)];
              Debug.Log((object) "Spreading blight from a plant", (UnityEngine.Object) context);
              list.Shuffle<Crop>();
              foreach (Crop crop in list)
              {
                if ((double) (crop.transform.GetPosition() - context.transform.GetPosition()).magnitude < 6.0)
                {
                  crop.GetComponent<BlightVulnerable>().MakeBlighted();
                  break;
                }
              }
            }
          }
          double num = (double) this.sm.nextInfection.Set(this.smi.gameplayEvent.incubationDuration, this);
        }
      }
    }
  }
}
