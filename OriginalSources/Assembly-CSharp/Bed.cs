// Decompiled with JetBrains decompiler
// Type: Bed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Bed")]
public class Bed : Workable, IBasicBuilding
{
  [MyCmpReq]
  private Sleepable sleepable;
  private WorkerBase targetWorker;
  public string[] effects;
  public static readonly Dictionary<string, string> roomSleepingEffects = new Dictionary<string, string>()
  {
    {
      "Barracks",
      "BarracksStamina"
    },
    {
      "Bedroom",
      "LuxuryBarracksStamina"
    },
    {
      "Private Bedroom",
      "BedroomStamina"
    }
  };

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.showProgressBar = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.BasicBuildings.Add((IBasicBuilding) this);
    this.sleepable = this.GetComponent<Sleepable>();
    Sleepable sleepable = this.sleepable;
    sleepable.OnWorkableEventCB = sleepable.OnWorkableEventCB + new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
  }

  private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
  {
    if (workable_event == Workable.WorkableEvent.WorkStarted)
    {
      this.AddEffects();
    }
    else
    {
      if (workable_event != Workable.WorkableEvent.WorkStopped)
        return;
      this.RemoveEffects();
    }
  }

  private void AddEffects()
  {
    this.targetWorker = this.sleepable.worker;
    if (this.effects != null)
    {
      foreach (string effect in this.effects)
        this.targetWorker.GetComponent<Effects>().Add(effect, false);
    }
    Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(this.gameObject);
    if (roomOfGameObject == null)
      return;
    RoomType roomType = roomOfGameObject.roomType;
    foreach (KeyValuePair<string, string> roomSleepingEffect in Bed.roomSleepingEffects)
    {
      if (roomSleepingEffect.Key == roomType.Id)
        this.targetWorker.GetComponent<Effects>().Add(roomSleepingEffect.Value, false);
    }
    roomType.TriggerRoomEffects(this.GetComponent<KPrefabID>(), this.targetWorker.GetComponent<Effects>());
  }

  private void RemoveEffects()
  {
    if ((UnityEngine.Object) this.targetWorker == (UnityEngine.Object) null)
      return;
    if (this.effects != null)
    {
      foreach (string effect in this.effects)
        this.targetWorker.GetComponent<Effects>().Remove(effect);
    }
    foreach (KeyValuePair<string, string> roomSleepingEffect in Bed.roomSleepingEffects)
      this.targetWorker.GetComponent<Effects>().Remove(roomSleepingEffect.Value);
    this.targetWorker = (WorkerBase) null;
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descs = new List<Descriptor>();
    if (this.effects != null)
    {
      foreach (string effect in this.effects)
      {
        if (effect != null && effect != "")
          Effect.AddModifierDescriptions(this.gameObject, descs, effect);
      }
    }
    return descs;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.BasicBuildings.Remove((IBasicBuilding) this);
    if (!((UnityEngine.Object) this.sleepable != (UnityEngine.Object) null))
      return;
    Sleepable sleepable = this.sleepable;
    sleepable.OnWorkableEventCB = sleepable.OnWorkableEventCB - new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
  }
}
