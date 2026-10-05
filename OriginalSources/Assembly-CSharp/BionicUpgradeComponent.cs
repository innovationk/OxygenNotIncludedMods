// Decompiled with JetBrains decompiler
// Type: BionicUpgradeComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BionicUpgradeComponent : Assignable, IGameObjectEffectDescriptor
{
  private BionicUpgradeComponentConfig.BionicUpgradeData data;
  public System.Action OnWattageCostChanged;
  private Guid unassignedStatusItem = Guid.Empty;

  public BionicUpgradeComponent.IWattageController WattageController { private set; get; }

  public float CurrentWattage
  {
    get => !this.HasWattageController ? 0.0f : this.WattageController.GetCurrentWattageCost();
  }

  public string CurrentWattageName
  {
    get
    {
      return !this.HasWattageController ? string.Format((string) DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, (object) this.GetProperName(), (object) GameUtil.GetFormattedWattage(this.PotentialWattage)) : this.WattageController.GetCurrentWattageCostName();
    }
  }

  public bool HasWattageController => this.WattageController != null;

  public float PotentialWattage => this.data.WattageCost;

  public BionicUpgradeComponentConfig.BoosterType Booster => this.data.Booster;

  public Func<global::StateMachine.Instance, global::StateMachine.Instance> StateMachine
  {
    get => this.data.stateMachine;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.data = BionicUpgradeComponentConfig.UpgradesData[this.gameObject.PrefabID()];
    this.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_OnlyOnBionics));
    this.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_HasAvailableSlots));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.assignmentManager.Remove((Assignable) this);
    this.customAssignmentUITooltipFunc = new Func<Assignables, string>(this.GetTooltipForBoosterAssignment);
    this.customAssignablesUITooltipFunc = new Func<Assignables, string>(this.GetTooltipForMinionAssigment);
    this.Subscribe(856640610, new Action<object>(this.RefreshStatusItem));
    this.RefreshStatusItem();
  }

  private void RefreshStatusItem(object _ = null)
  {
    if (this.assignee == null && !this.gameObject.HasTag(GameTags.Stored))
    {
      if (!(this.unassignedStatusItem == Guid.Empty))
        return;
      this.unassignedStatusItem = this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.UnassignedBionicBooster);
    }
    else
    {
      if (!(this.unassignedStatusItem != Guid.Empty))
        return;
      this.unassignedStatusItem = this.GetComponent<KSelectable>().RemoveStatusItem(this.unassignedStatusItem);
    }
  }

  public string GetTooltipForMinionAssigment(Assignables assignables)
  {
    MinionAssignablesProxy component = assignables.GetComponent<MinionAssignablesProxy>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return "ERROR N/A";
    GameObject targetGameObject = component.GetTargetGameObject();
    if ((UnityEngine.Object) targetGameObject == (UnityEngine.Object) null)
      return "ERROR N/A";
    BionicUpgradesMonitor.Instance smi = targetGameObject.GetSMI<BionicUpgradesMonitor.Instance>();
    if (smi == null)
      return "This Duplicant cannot install boosters";
    int num = smi.CountBoosterAssignments(this.PrefabID());
    string str1 = num == 0 ? string.Format((string) UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NOT_ALREADY_ASSIGNED, (object) smi.gameObject.GetProperName(), (object) num) : string.Format((string) UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.ALREADY_ASSIGNED, (object) smi.gameObject.GetProperName(), (object) num);
    string str2 = string.Format((string) (smi.AssignedSlotCount < smi.UnlockedSlotCount ? UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.AVAILABLE_SLOTS : UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NO_AVAILABLE_SLOTS), (object) targetGameObject.GetProperName(), (object) smi.AssignedSlotCount, (object) smi.UnlockedSlotCount);
    string str3 = "";
    List<AttributeInstance> all = new List<AttributeInstance>((IEnumerable<AttributeInstance>) targetGameObject.GetAttributes().AttributeTable).FindAll((Predicate<AttributeInstance>) (a => a.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill));
    for (int index = 0; index < all.Count; ++index)
    {
      string str4 = UIConstants.ColorPrefixWhite;
      if ((double) all[index].GetTotalValue() > 0.0)
        str4 = UIConstants.ColorPrefixGreen;
      else if ((double) all[index].GetTotalValue() < 0.0)
        str4 = UIConstants.ColorPrefixRed;
      str3 += $"{all[index].Name}: {str4 + all[index].GetFormattedValue() + UIConstants.ColorSuffix}";
      if (index != all.Count - 1)
        str3 += "\n";
    }
    return $"{targetGameObject.GetProperName()}\n\n{str1}\n\n{str2}\n\n{str3}";
  }

  public string GetTooltipForBoosterAssignment(Assignables assignables)
  {
    MinionAssignablesProxy component = assignables.GetComponent<MinionAssignablesProxy>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return "ERROR N/A";
    GameObject targetGameObject = component.GetTargetGameObject();
    if ((UnityEngine.Object) targetGameObject == (UnityEngine.Object) null)
      return "ERROR N/A";
    BionicUpgradesMonitor.Instance smi = targetGameObject.GetSMI<BionicUpgradesMonitor.Instance>();
    if (smi == null)
      return "ERROR N/A";
    int num = smi.CountBoosterAssignments(this.PrefabID());
    string str = num == 0 ? string.Format((string) UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NOT_ALREADY_ASSIGNED, (object) smi.gameObject.GetProperName(), (object) num) : string.Format((string) UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.ALREADY_ASSIGNED, (object) smi.gameObject.GetProperName(), (object) num);
    return $"{BionicUpgradeComponentConfig.GenerateTooltipForBooster(this)}\n\n{str}";
  }

  public void InformOfWattageChanged()
  {
    System.Action wattageCostChanged = this.OnWattageCostChanged;
    if (wattageCostChanged == null)
      return;
    wattageCostChanged();
  }

  public void SetWattageController(
    BionicUpgradeComponent.IWattageController wattageController)
  {
    this.WattageController = wattageController;
  }

  public override void Assign(IAssignableIdentity new_assignee)
  {
    AssignableSlotInstance specificSlotInstance = (AssignableSlotInstance) null;
    if (new_assignee == this.assignee)
      return;
    if (new_assignee != this.assignee)
    {
      switch (new_assignee)
      {
        case MinionIdentity _:
        case StoredMinionIdentity _:
        case MinionAssignablesProxy _:
          Ownables soleOwner = new_assignee.GetSoleOwner();
          if ((UnityEngine.Object) soleOwner != (UnityEngine.Object) null)
          {
            BionicUpgradesMonitor.Instance smi = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetSMI<BionicUpgradesMonitor.Instance>();
            if (smi != null)
            {
              BionicUpgradesMonitor.UpgradeComponentSlot emptyAvailableSlot = smi.GetFirstEmptyAvailableSlot();
              if (emptyAvailableSlot != null)
              {
                specificSlotInstance = emptyAvailableSlot.GetAssignableSlotInstance();
                break;
              }
              break;
            }
            break;
          }
          break;
      }
    }
    this.Assign(new_assignee, specificSlotInstance);
    this.Trigger(1980521255, (object) null);
    this.RefreshStatusItem();
  }

  public override void Unassign()
  {
    base.Unassign();
    this.Trigger(1980521255, (object) null);
    this.RefreshStatusItem();
  }

  private bool AssignablePrecondition_OnlyOnBionics(MinionAssignablesProxy worker)
  {
    return worker.GetMinionModel() == BionicMinionConfig.MODEL;
  }

  private bool AssignablePrecondition_HasAvailableSlots(MinionAssignablesProxy worker)
  {
    if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null && (UnityEngine.Object) SelectTool.Instance.selected.gameObject == (UnityEngine.Object) worker.GetTargetGameObject())
      return true;
    MinionIdentity target = worker.target as MinionIdentity;
    if ((UnityEngine.Object) target != (UnityEngine.Object) null)
    {
      BionicUpgradesMonitor.Instance smi = target.GetSMI<BionicUpgradesMonitor.Instance>();
      if (smi == null)
        return true;
      foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
      {
        if (!upgradeComponentSlot.IsLocked && ((UnityEngine.Object) upgradeComponentSlot.assignedUpgradeComponent == (UnityEngine.Object) null || (UnityEngine.Object) upgradeComponentSlot.assignedUpgradeComponent == (UnityEngine.Object) this))
          return true;
      }
    }
    return false;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(BionicUpgradeComponentConfig.UpgradesData[this.gameObject.PrefabID()].stateMachineDescription, (string) null)
    };
  }

  public interface IWattageController
  {
    float GetCurrentWattageCost();

    string GetCurrentWattageCostName();
  }
}
