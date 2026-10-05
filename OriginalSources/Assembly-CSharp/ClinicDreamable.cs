// Decompiled with JetBrains decompiler
// Type: ClinicDreamable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Clinic Dreamable")]
public class ClinicDreamable : Workable
{
  private static GameObject dreamJournalPrefab;
  private static Effect sleepClinic;
  public bool HasStartedThoughts_Dreaming;
  private ChoreDriver dreamer;
  private Equippable equippable;
  private Effects effects;
  private Sleepable sleepable;
  private KSelectable selectable;
  private HashedString dreamAnimName = (HashedString) "portal rocket comp";

  public bool DreamIsDisturbed { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.resetProgressOnStop = false;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Dreaming;
    this.workingStatusItem = (StatusItem) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) ClinicDreamable.dreamJournalPrefab == (UnityEngine.Object) null)
    {
      ClinicDreamable.dreamJournalPrefab = Assets.GetPrefab(DreamJournalConfig.ID);
      ClinicDreamable.sleepClinic = Db.Get().effects.Get("SleepClinic");
    }
    this.equippable = this.GetComponent<Equippable>();
    Debug.Assert((UnityEngine.Object) this.equippable != (UnityEngine.Object) null);
    this.equippable.def.OnEquipCallBack += new Action<Equippable>(this.OnEquipPajamas);
    this.equippable.def.OnUnequipCallBack += new Action<Equippable>(this.OnUnequipPajamas);
    this.OnEquipPajamas(this.equippable);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if ((UnityEngine.Object) this.equippable == (UnityEngine.Object) null)
      return;
    this.equippable.def.OnEquipCallBack -= new Action<Equippable>(this.OnEquipPajamas);
    this.equippable.def.OnUnequipCallBack -= new Action<Equippable>(this.OnUnequipPajamas);
  }

  protected override bool OnWorkTick(WorkerBase worker, float dt)
  {
    if ((double) this.GetPercentComplete() >= 1.0)
    {
      Vector3 position = this.dreamer.transform.position;
      ++position.y;
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
      Util.KInstantiate(ClinicDreamable.dreamJournalPrefab, position, Quaternion.identity).SetActive(true);
      this.workTimeRemaining = this.GetWorkTime();
    }
    return false;
  }

  public void OnEquipPajamas(Equippable eq)
  {
    if ((UnityEngine.Object) this.equippable == (UnityEngine.Object) null || (UnityEngine.Object) this.equippable != (UnityEngine.Object) eq)
      return;
    MinionAssignablesProxy assignee = this.equippable.assignee as MinionAssignablesProxy;
    if ((UnityEngine.Object) assignee == (UnityEngine.Object) null || assignee.target is StoredMinionIdentity)
      return;
    GameObject targetGameObject = assignee.GetTargetGameObject();
    this.effects = targetGameObject.GetComponent<Effects>();
    this.dreamer = targetGameObject.GetComponent<ChoreDriver>();
    this.selectable = targetGameObject.GetComponent<KSelectable>();
    this.dreamer.Subscribe(-1283701846, new Action<object>(this.WorkerStartedSleeping));
    this.dreamer.Subscribe(-2090444759, new Action<object>(this.WorkerStoppedSleeping));
    this.effects.Add(ClinicDreamable.sleepClinic, true);
    this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing);
  }

  public void OnUnequipPajamas(Equippable eq)
  {
    if ((UnityEngine.Object) this.dreamer == (UnityEngine.Object) null || (UnityEngine.Object) this.equippable == (UnityEngine.Object) null || (UnityEngine.Object) this.equippable != (UnityEngine.Object) eq)
      return;
    this.dreamer.Unsubscribe(-1283701846, new Action<object>(this.WorkerStartedSleeping));
    this.dreamer.Unsubscribe(-2090444759, new Action<object>(this.WorkerStoppedSleeping));
    this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing);
    this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping);
    this.effects.Remove(ClinicDreamable.sleepClinic.Id);
    this.StopDreamingThought();
    this.dreamer = (ChoreDriver) null;
    this.selectable = (KSelectable) null;
    this.effects = (Effects) null;
  }

  public void WorkerStartedSleeping(object data)
  {
    SleepChore currentChore = this.dreamer.GetCurrentChore() as SleepChore;
    currentChore.smi.sm.isDisturbedByLight.GetContext(currentChore.smi).onDirty += new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed);
    currentChore.smi.sm.isDisturbedByMovement.GetContext(currentChore.smi).onDirty += new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed);
    currentChore.smi.sm.isDisturbedByNoise.GetContext(currentChore.smi).onDirty += new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed);
    currentChore.smi.sm.isScaredOfDark.GetContext(currentChore.smi).onDirty += new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed);
    this.sleepable = data as Sleepable;
    this.sleepable.Dreamable = this;
    this.StartWork(this.sleepable.worker);
    this.progressBar.Retarget(this.sleepable.gameObject);
    this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, (object) this);
    this.StartDreamingThought();
  }

  public void WorkerStoppedSleeping(object data)
  {
    this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping);
    SleepChore currentChore = this.dreamer.GetCurrentChore() as SleepChore;
    if (!currentChore.IsNullOrDestroyed() && !currentChore.smi.IsNullOrDestroyed() && !currentChore.smi.sm.IsNullOrDestroyed())
    {
      currentChore.smi.sm.isDisturbedByLight.GetContext(currentChore.smi).onDirty -= new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed);
      currentChore.smi.sm.isDisturbedByMovement.GetContext(currentChore.smi).onDirty -= new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed);
      currentChore.smi.sm.isDisturbedByNoise.GetContext(currentChore.smi).onDirty -= new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed);
      currentChore.smi.sm.isScaredOfDark.GetContext(currentChore.smi).onDirty -= new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed);
    }
    this.StopDreamingThought();
    this.DreamIsDisturbed = false;
    if ((UnityEngine.Object) this.worker != (UnityEngine.Object) null)
      this.StopWork(this.worker, false);
    if (!((UnityEngine.Object) this.sleepable != (UnityEngine.Object) null))
      return;
    this.sleepable.Dreamable = (ClinicDreamable) null;
    this.sleepable = (Sleepable) null;
  }

  private void OnSleepDisturbed(SleepChore.StatesInstance smi)
  {
    SleepChore currentChore = this.dreamer.GetCurrentChore() as SleepChore;
    bool flag = currentChore.smi.sm.isDisturbedByLight.Get(currentChore.smi) | currentChore.smi.sm.isDisturbedByMovement.Get(currentChore.smi) | currentChore.smi.sm.isDisturbedByNoise.Get(currentChore.smi) | currentChore.smi.sm.isScaredOfDark.Get(currentChore.smi);
    this.DreamIsDisturbed = flag;
    if (!flag)
      return;
    this.StopDreamingThought();
  }

  private void StartDreamingThought()
  {
    if (!((UnityEngine.Object) this.dreamer != (UnityEngine.Object) null) || this.HasStartedThoughts_Dreaming)
      return;
    this.dreamer.GetSMI<Dreamer.Instance>().SetDream(Db.Get().Dreams.CommonDream);
    this.dreamer.GetSMI<Dreamer.Instance>().StartDreaming();
    this.HasStartedThoughts_Dreaming = true;
  }

  private void StopDreamingThought()
  {
    if (!((UnityEngine.Object) this.dreamer != (UnityEngine.Object) null) || !this.HasStartedThoughts_Dreaming)
      return;
    this.dreamer.GetSMI<Dreamer.Instance>().StopDreaming();
    this.HasStartedThoughts_Dreaming = false;
  }
}
