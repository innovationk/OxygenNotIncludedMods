// Decompiled with JetBrains decompiler
// Type: Grave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class Grave : StateMachineComponent<Grave.StatesInstance>
{
  [Serialize]
  public string graveName;
  [Serialize]
  public string graveAnim = "closed";
  [Serialize]
  public int epitaphIdx;
  [Serialize]
  public float burialTime = -1f;
  private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1];
  private static readonly EventSystem.IntraObjectHandler<Grave> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Grave>((Action<Grave, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Grave>(-1697596308, Grave.OnStorageChangedDelegate);
    this.epitaphIdx = UnityEngine.Random.Range(0, int.MaxValue);
  }

  protected override void OnSpawn()
  {
    this.GetComponent<Storage>().SetOffsets(Grave.DELIVERY_OFFSETS);
    Storage component = this.GetComponent<Storage>();
    Storage storage = component;
    storage.OnWorkableEventCB = storage.OnWorkableEventCB + new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent);
    KAnimFile anim1 = Assets.GetAnim((HashedString) "anim_bury_dupe_kanim");
    int index = 0;
    KAnim.Anim anim2;
    while (true)
    {
      anim2 = anim1.GetData().GetAnim(index);
      if (anim2 != null)
      {
        if (!(anim2.name == "working_pre"))
          ++index;
        else
          break;
      }
      else
        goto label_5;
    }
    float work_time = (float) (anim2.numFrames - 3) / anim2.frameRate;
    component.SetWorkTime(work_time);
label_5:
    base.OnSpawn();
    this.smi.StartSM();
    Components.Graves.Add(this);
  }

  protected override void OnCleanUp()
  {
    Components.Graves.Remove(this);
    base.OnCleanUp();
  }

  private void OnStorageChanged(object data)
  {
    GameObject original = (GameObject) data;
    if (!((UnityEngine.Object) original != (UnityEngine.Object) null))
      return;
    this.graveName = original.name;
    MinionIdentity component = original.GetComponent<MinionIdentity>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      Personality personality = Db.Get().Personalities.TryGet(component.personalityResourceId);
      KAnimFile anim = Assets.GetAnim((HashedString) "gravestone_kanim");
      if (personality != null && anim.GetData().GetAnim(personality.graveStone) != null)
        this.graveAnim = personality.graveStone;
    }
    Util.KDestroyGameObject(original);
  }

  private void OnWorkEvent(Workable workable, Workable.WorkableEvent evt)
  {
  }

  public class StatesInstance(Grave master) : 
    GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.GameInstance(master)
  {
    private FetchChore chore;

    public void CreateFetchTask()
    {
      ChoreType fetchCritical = Db.Get().ChoreTypes.FetchCritical;
      Storage component = this.GetComponent<Storage>();
      double defaultMass = (double) DUPLICANTSTATS.STANDARD.BaseStats.DEFAULT_MASS;
      HashSet<Tag> tags = new HashSet<Tag>();
      tags.Add(GameTags.BaseMinion);
      Tag corpse = GameTags.Corpse;
      this.chore = new FetchChore(fetchCritical, component, (float) defaultMass, tags, FetchChore.MatchCriteria.MatchTags, corpse);
      this.chore.allowMultifetch = false;
    }

    public void CancelFetchTask()
    {
      this.chore.Cancel("Exit State");
      this.chore = (FetchChore) null;
    }
  }

  public class States : GameStateMachine<Grave.States, Grave.StatesInstance, Grave>
  {
    public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State empty;
    public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State full;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.empty;
      this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
      this.empty.PlayAnim("open").Enter("CreateFetchTask", (StateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State.Callback) (smi => smi.CreateFetchTask())).Exit("CancelFetchTask", (StateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State.Callback) (smi => smi.CancelFetchTask())).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GraveEmpty).EventTransition(GameHashes.OnStorageChange, this.full);
      this.full.PlayAnim((Func<Grave.StatesInstance, string>) (smi => smi.master.graveAnim)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Grave).Enter((StateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State.Callback) (smi =>
      {
        if ((double) smi.master.burialTime >= 0.0)
          return;
        smi.master.burialTime = GameClock.Instance.GetTime();
      }));
    }
  }
}
