// Decompiled with JetBrains decompiler
// Type: EquippableWorkable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/EquippableWorkable")]
public class EquippableWorkable : Workable, ISaveLoadable
{
  [MyCmpReq]
  private Equippable equippable;
  private Chore chore;
  private IAssignableIdentity currentTarget;
  private QualityLevel quality;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Equipping;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_equip_clothing_kanim")
    };
    this.synchronizeAnims = false;
  }

  public QualityLevel GetQuality() => this.quality;

  public void SetQuality(QualityLevel level) => this.quality = level;

  protected override void OnSpawn()
  {
    this.SetWorkTime(1.5f);
    this.equippable.OnAssign += new Action<IAssignableIdentity>(this.RefreshChore);
  }

  private void CreateChore()
  {
    Debug.Assert(this.chore == null, (object) "chore should be null");
    this.chore = (Chore) new EquipChore((IStateMachineTarget) this);
    this.chore.onExit += new Action<Chore>(this.OnChoreExit);
  }

  private void OnChoreExit(Chore chore)
  {
    if (chore.isComplete)
      return;
    this.RefreshChore(this.currentTarget);
  }

  public void CancelChore(string reason = "")
  {
    if (this.chore == null)
      return;
    this.chore.Cancel(reason);
    Prioritizable.RemoveRef(this.equippable.gameObject);
    this.chore = (Chore) null;
  }

  private void RefreshChore(IAssignableIdentity target)
  {
    if (this.chore != null)
      this.CancelChore("Equipment Reassigned");
    this.currentTarget = target;
    if (target == null || target.GetSoleOwner().GetComponent<Equipment>().IsEquipped(this.equippable))
      return;
    this.CreateChore();
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    if (this.equippable.assignee == null)
      return;
    Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
    if (!(bool) (UnityEngine.Object) soleOwner)
      return;
    soleOwner.GetComponent<Equipment>().Equip(this.equippable);
    Prioritizable.RemoveRef(this.equippable.gameObject);
    this.chore = (Chore) null;
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    this.workTimeRemaining = this.GetWorkTime();
    base.OnStopWork(worker);
  }
}
