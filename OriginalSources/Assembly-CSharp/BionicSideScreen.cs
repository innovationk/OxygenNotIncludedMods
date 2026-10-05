// Decompiled with JetBrains decompiler
// Type: BionicSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BionicSideScreen : SideScreenContent
{
  public OwnablesSecondSideScreen ownableSecondSideScreenPrefab;
  public BionicSideScreenUpgradeSlot originalBionicSlot;
  private BionicUpgradesMonitor.Instance upgradeMonitor;
  private BionicBatteryMonitor.Instance batteryMonitor;
  private BionicBedTimeMonitor.Instance bedTimeMonitor;
  private List<BionicSideScreenUpgradeSlot> bionicSlots = new List<BionicSideScreenUpgradeSlot>();
  private OwnablesSidescreen ownableSidescreen;
  private AssignableSlotInstance lastSlotSelected;
  private int onBionicBecameOnlineHandle = -1;
  private int onBionicBecameOfflineHandle = -1;
  private int onBionicUpgradeChangedHandle = -1;
  private int onBionicUpgradeComponentSlotCountChangedHandle = -1;
  private int onBionicWattageChangedHandle = -1;
  private int onBionicTagsChangedHandle = -1;

  private void OnBionicUpgradeSlotClicked(BionicSideScreenUpgradeSlot slotClicked)
  {
    bool flag1 = (UnityEngine.Object) slotClicked == (UnityEngine.Object) null || this.lastSlotSelected == slotClicked.upgradeSlot.GetAssignableSlotInstance();
    bool flag2 = !flag1 && slotClicked.upgradeSlot.IsLocked;
    this.lastSlotSelected = flag1 ? (AssignableSlotInstance) null : slotClicked.upgradeSlot.GetAssignableSlotInstance();
    this.RefreshSelectedStateInSlots();
    AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
    AssignableSlotInstance assignableSlotInstance = flag1 | flag2 ? (AssignableSlotInstance) null : slotClicked.upgradeSlot.GetAssignableSlotInstance();
    if ((UnityEngine.Object) this.ownableSidescreen != (UnityEngine.Object) null)
      this.ownableSidescreen.SetSelectedSlot(assignableSlotInstance);
    else if (flag1 | flag2)
      DetailsScreen.Instance.ClearSecondarySideScreen();
    else
      ((OwnablesSecondSideScreen) DetailsScreen.Instance.SetSecondarySideScreen((KScreen) this.ownableSecondSideScreenPrefab, bionicUpgrade.Name)).SetSlot(assignableSlotInstance);
  }

  private void RefreshSelectedStateInSlots()
  {
    for (int index = 0; index < this.bionicSlots.Count; ++index)
    {
      BionicSideScreenUpgradeSlot bionicSlot = this.bionicSlots[index];
      bionicSlot.SetSelected(bionicSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
    }
  }

  public void RecreateBionicSlots()
  {
    int length = this.upgradeMonitor != null ? this.upgradeMonitor.upgradeComponentSlots.Length : 0;
    for (int index = 0; index < Mathf.Max(length, this.bionicSlots.Count); ++index)
    {
      if (index >= this.bionicSlots.Count)
        this.bionicSlots.Add(this.CreateBionicSlot());
      BionicSideScreenUpgradeSlot bionicSlot = this.bionicSlots[index];
      if (index < length)
      {
        BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeMonitor.upgradeComponentSlots[index];
        bionicSlot.gameObject.SetActive(true);
        bionicSlot.Setup(upgradeComponentSlot);
        bionicSlot.SetSelected(bionicSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
      }
      else
      {
        bionicSlot.Setup((BionicUpgradesMonitor.UpgradeComponentSlot) null);
        bionicSlot.gameObject.SetActive(false);
      }
    }
  }

  private BionicSideScreenUpgradeSlot CreateBionicSlot()
  {
    BionicSideScreenUpgradeSlot bionicSlot = Util.KInstantiateUI<BionicSideScreenUpgradeSlot>(this.originalBionicSlot.gameObject, this.originalBionicSlot.transform.parent.gameObject);
    bionicSlot.OnClick += new Action<BionicSideScreenUpgradeSlot>(this.OnBionicUpgradeSlotClicked);
    return bionicSlot;
  }

  private void OnBionicBecameOnline(object o) => this.RefreshSlots();

  private void OnBionicBecameOffline(object o) => this.RefreshSlots();

  private void OnBionicWattageChanged(object o) => this.RefreshSlots();

  private void OnBionicBedTimeChoreStateChanged(object o) => this.RefreshSlots();

  private void OnBionicUpgradeComponentSlotCountChanged(object o) => this.RefreshSlots();

  private void OnBionicUpgradeChanged(object o) => this.RecreateBionicSlots();

  private void OnBionicTagsChanged(object o)
  {
    if (o == null || !(((Boxed<TagChangedEventData>) o).value.tag == GameTags.BionicBedTime))
      return;
    this.OnBionicBedTimeChoreStateChanged(o);
  }

  private void RefreshSlots()
  {
    for (int index = this.bionicSlots.Count - 1; index >= 0; --index)
    {
      BionicSideScreenUpgradeSlot bionicSlot = this.bionicSlots[index];
      if ((UnityEngine.Object) bionicSlot != (UnityEngine.Object) null)
      {
        bionicSlot.Refresh();
        bionicSlot.gameObject.transform.SetAsFirstSibling();
      }
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.originalBionicSlot.gameObject.SetActive(false);
    this.ownableSidescreen = this.transform.parent.GetComponentInChildren<OwnablesSidescreen>();
    if (!((UnityEngine.Object) this.ownableSidescreen != (UnityEngine.Object) null))
      return;
    this.ownableSidescreen.OnSlotInstanceSelected += new Action<AssignableSlotInstance>(this.OnOwnableSidescreenRowSelected);
  }

  private void OnOwnableSidescreenRowSelected(AssignableSlotInstance slot)
  {
    this.lastSlotSelected = slot;
    this.RefreshSelectedStateInSlots();
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.lastSlotSelected = (AssignableSlotInstance) null;
    if (this.upgradeMonitor != null)
    {
      this.upgradeMonitor.Unsubscribe(ref this.onBionicBecameOnlineHandle);
      this.upgradeMonitor.Unsubscribe(ref this.onBionicBecameOfflineHandle);
      this.upgradeMonitor.Unsubscribe(ref this.onBionicUpgradeChangedHandle);
      this.upgradeMonitor.Unsubscribe(ref this.onBionicUpgradeComponentSlotCountChangedHandle);
    }
    if (this.batteryMonitor != null)
      this.batteryMonitor.Unsubscribe(ref this.onBionicWattageChangedHandle);
    if (this.bedTimeMonitor != null)
      this.bedTimeMonitor.Unsubscribe(ref this.onBionicTagsChangedHandle);
    this.batteryMonitor = target.GetSMI<BionicBatteryMonitor.Instance>();
    this.upgradeMonitor = target.GetSMI<BionicUpgradesMonitor.Instance>();
    this.bedTimeMonitor = target.GetSMI<BionicBedTimeMonitor.Instance>();
    this.onBionicBecameOnlineHandle = this.upgradeMonitor.Subscribe(160824499, new Action<object>(this.OnBionicBecameOnline));
    this.onBionicBecameOfflineHandle = this.upgradeMonitor.Subscribe(-1730800797, new Action<object>(this.OnBionicBecameOffline));
    this.onBionicUpgradeChangedHandle = this.upgradeMonitor.Subscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
    this.onBionicUpgradeComponentSlotCountChangedHandle = this.batteryMonitor.Subscribe(1095596132, new Action<object>(this.OnBionicUpgradeComponentSlotCountChanged));
    this.onBionicWattageChangedHandle = this.batteryMonitor.Subscribe(1361471071, new Action<object>(this.OnBionicWattageChanged));
    this.onBionicTagsChangedHandle = this.bedTimeMonitor.Subscribe(-1582839653, new Action<object>(this.OnBionicTagsChanged));
    this.RecreateBionicSlots();
    this.RefreshSlots();
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!show)
      return;
    this.RefreshSlots();
  }

  public override void ClearTarget()
  {
    base.ClearTarget();
    if (this.upgradeMonitor != null)
      this.upgradeMonitor.Unsubscribe(ref this.onBionicUpgradeChangedHandle);
    this.bedTimeMonitor = (BionicBedTimeMonitor.Instance) null;
    this.upgradeMonitor = (BionicUpgradesMonitor.Instance) null;
    this.lastSlotSelected = (AssignableSlotInstance) null;
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetSMI<BionicBatteryMonitor.Instance>() != null;
  }

  public override int GetSideScreenSortOrder() => 300;
}
