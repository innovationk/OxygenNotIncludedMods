// Decompiled with JetBrains decompiler
// Type: AssignableSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AssignableSideScreenRow")]
public class AssignableSideScreenRow : KMonoBehaviour
{
  [SerializeField]
  private CrewPortrait crewPortraitPrefab;
  [SerializeField]
  private LocText assignmentText;
  public AssignableSideScreen sideScreen;
  private CrewPortrait portraitInstance;
  [MyCmpReq]
  private MultiToggle toggle;
  public IAssignableIdentity targetIdentity;
  public AssignableSideScreenRow.AssignableState currentState;
  private int refreshHandle = -1;

  public void Refresh(object data = null)
  {
    if (!this.sideScreen.targetAssignable.CanAssignTo(this.targetIdentity))
    {
      this.currentState = AssignableSideScreenRow.AssignableState.Disabled;
      this.assignmentText.text = (string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED;
    }
    else if (this.sideScreen.targetAssignable.assignee == this.targetIdentity)
    {
      this.currentState = AssignableSideScreenRow.AssignableState.Selected;
      this.assignmentText.text = (string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGNED;
    }
    else
    {
      bool flag = false;
      KMonoBehaviour targetIdentity = this.targetIdentity as KMonoBehaviour;
      if ((UnityEngine.Object) targetIdentity != (UnityEngine.Object) null)
      {
        Ownables component1 = targetIdentity.GetComponent<Ownables>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          AssignableSlotInstance[] slots = component1.GetSlots(this.sideScreen.targetAssignable.slot);
          if (slots != null && slots.Length != 0)
          {
            AssignableSlotInstance assignableSlotInstance = slots.FindFirst<AssignableSlotInstance>((Func<AssignableSlotInstance, bool>) (s => !s.IsAssigned())) ?? slots[0];
            if (assignableSlotInstance != null && assignableSlotInstance.IsAssigned())
            {
              this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
              this.assignmentText.text = assignableSlotInstance.assignable.GetProperName();
              flag = true;
            }
          }
        }
        Equipment component2 = targetIdentity.GetComponent<Equipment>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          AssignableSlotInstance[] slots = component2.GetSlots(this.sideScreen.targetAssignable.slot);
          if (slots != null && slots.Length != 0)
          {
            AssignableSlotInstance assignableSlotInstance = slots.FindFirst<AssignableSlotInstance>((Func<AssignableSlotInstance, bool>) (s => !s.IsAssigned())) ?? slots[0];
            if (assignableSlotInstance != null && assignableSlotInstance.IsAssigned())
            {
              this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
              this.assignmentText.text = assignableSlotInstance.assignable.GetProperName();
              flag = true;
            }
          }
        }
      }
      if (!flag)
      {
        this.currentState = AssignableSideScreenRow.AssignableState.Unassigned;
        this.assignmentText.text = (string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED;
      }
    }
    this.toggle.ChangeState((int) this.currentState);
  }

  protected override void OnCleanUp()
  {
    Game.Instance.Unsubscribe(ref this.refreshHandle);
    base.OnCleanUp();
  }

  public void SetContent(
    IAssignableIdentity identity_object,
    Action<IAssignableIdentity> selectionCallback,
    AssignableSideScreen assignableSideScreen)
  {
    Game.Instance.Unsubscribe(ref this.refreshHandle);
    this.refreshHandle = Game.Instance.Subscribe(-2146166042, (Action<object>) (o =>
    {
      if (!((UnityEngine.Object) this != (UnityEngine.Object) null) || !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null) || !this.gameObject.activeInHierarchy)
        return;
      this.Refresh();
    }));
    this.toggle = this.GetComponent<MultiToggle>();
    this.sideScreen = assignableSideScreen;
    this.targetIdentity = identity_object;
    if ((UnityEngine.Object) this.portraitInstance == (UnityEngine.Object) null)
    {
      this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, this.gameObject);
      this.portraitInstance.transform.SetSiblingIndex(1);
      this.portraitInstance.SetAlpha(1f);
    }
    this.toggle.onClick = (System.Action) (() => selectionCallback(this.targetIdentity));
    this.portraitInstance.SetIdentityObject(identity_object, false);
    this.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.GetTooltip);
    this.Refresh();
  }

  private string GetTooltip()
  {
    ToolTip component = this.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    if (this.sideScreen.targetAssignable.customAssignablesUITooltipFunc != null)
      return this.sideScreen.targetAssignable.customAssignablesUITooltipFunc((Assignables) this.targetIdentity.GetSoleOwner());
    if (this.targetIdentity != null && !this.targetIdentity.IsNull())
    {
      switch (this.currentState)
      {
        case AssignableSideScreenRow.AssignableState.Selected:
          component.AddMultiStringTooltip(string.Format((string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGN_TOOLTIP, (object) this.targetIdentity.GetProperName()), (TextStyleSetting) null);
          break;
        case AssignableSideScreenRow.AssignableState.Disabled:
          component.AddMultiStringTooltip(string.Format((string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED_TOOLTIP, (object) this.targetIdentity.GetProperName()), (TextStyleSetting) null);
          break;
        default:
          component.AddMultiStringTooltip(string.Format((string) UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGN_TO_TOOLTIP, (object) this.targetIdentity.GetProperName()), (TextStyleSetting) null);
          break;
      }
    }
    return "";
  }

  public enum AssignableState
  {
    Selected,
    AssignedToOther,
    Unassigned,
    Disabled,
  }
}
