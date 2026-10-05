// Decompiled with JetBrains decompiler
// Type: Activatable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Activatable")]
public class Activatable : Workable, ISidescreenButtonControl
{
  public bool Required = true;
  private static readonly Operational.Flag activatedFlagRequirement = new Operational.Flag(nameof (activated), Operational.Flag.Type.Requirement);
  private static readonly Operational.Flag activatedFlagFunctional = new Operational.Flag(nameof (activated), Operational.Flag.Type.Functional);
  [Serialize]
  private bool activated;
  [Serialize]
  private bool awaitingActivation;
  public Func<bool> activationCondition;
  private Guid statusItem;
  private Chore activateChore;
  public System.Action onActivate;
  [SerializeField]
  private ButtonMenuTextOverride textOverride;

  public bool IsActivated => this.activated;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    this.UpdateFlag();
    if (!this.awaitingActivation || this.activateChore != null || this.activationCondition != null && !this.activationCondition())
      return;
    this.CreateChore();
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    this.activated = true;
    if (this.onActivate != null)
      this.onActivate();
    this.awaitingActivation = false;
    this.UpdateFlag();
    Prioritizable.RemoveRef(this.gameObject);
    base.OnCompleteWork(worker);
  }

  private void UpdateFlag()
  {
    this.GetComponent<Operational>().SetFlag(this.Required ? Activatable.activatedFlagRequirement : Activatable.activatedFlagFunctional, this.activated);
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.DuplicantActivationRequired, !this.activated);
    this.Trigger(-1909216579, (object) BoxedBools.Box(this.IsActivated));
  }

  private void CreateChore()
  {
    if (this.activateChore != null)
      return;
    Prioritizable.AddRef(this.gameObject);
    this.activateChore = (Chore) new WorkChore<Activatable>(Db.Get().ChoreTypes.Toggle, (IStateMachineTarget) this, only_when_operational: false);
    if (string.IsNullOrEmpty(this.requiredSkillPerk))
      return;
    this.shouldShowSkillPerkStatusItem = true;
    this.requireMinionToWork = true;
    this.UpdateStatusItem();
  }

  public void CancelChore()
  {
    if (this.activateChore == null)
      return;
    this.activateChore.Cancel("User cancelled");
    this.activateChore = (Chore) null;
  }

  public int HorizontalGroupID() => -1;

  public string SidescreenButtonText
  {
    get
    {
      return this.activateChore != null ? (string) (this.textOverride.IsValid ? this.textOverride.CancelText : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE_CANCEL) : (string) (this.textOverride.IsValid ? this.textOverride.Text : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE);
    }
  }

  public string SidescreenButtonTooltip
  {
    get
    {
      return this.activateChore != null ? (string) (this.textOverride.IsValid ? this.textOverride.CancelToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_CANCEL) : (string) (this.textOverride.IsValid ? this.textOverride.ToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_ACTIVATE);
    }
  }

  public bool SidescreenEnabled() => !this.activated;

  public void SetButtonTextOverride(ButtonMenuTextOverride text) => this.textOverride = text;

  public void OnSidescreenButtonPressed()
  {
    if (this.activateChore == null)
      this.CreateChore();
    else
      this.CancelChore();
    this.awaitingActivation = this.activateChore != null;
  }

  public bool SidescreenButtonInteractable()
  {
    if (this.activated)
      return false;
    return this.activationCondition == null || this.activationCondition();
  }

  public int ButtonSideScreenSortOrder() => 20;
}
