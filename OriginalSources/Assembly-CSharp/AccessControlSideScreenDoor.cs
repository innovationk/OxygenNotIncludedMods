// Decompiled with JetBrains decompiler
// Type: AccessControlSideScreenDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AccessControlSideScreenDoor")]
public class AccessControlSideScreenDoor : KMonoBehaviour
{
  public KToggle leftButton;
  public KToggle rightButton;
  private Action<MinionAssignablesProxy, AccessControl.Permission> permissionChangedCallback;
  private Action<Tag, AccessControl.Permission> permissionChangedTagCallback;
  private bool isUpDown;
  protected MinionAssignablesProxy targetIdentity;
  protected Tag targetTag;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.leftButton.onClick += new System.Action(this.OnPermissionButtonClicked);
    this.rightButton.onClick += new System.Action(this.OnPermissionButtonClicked);
  }

  private void OnPermissionButtonClicked()
  {
    AccessControl.Permission permission = !this.leftButton.isOn ? (!this.rightButton.isOn ? AccessControl.Permission.Neither : AccessControl.Permission.GoRight) : (!this.rightButton.isOn ? AccessControl.Permission.GoLeft : AccessControl.Permission.Both);
    this.UpdateButtonStates(false);
    if (this.permissionChangedCallback != null)
      this.permissionChangedCallback(this.targetIdentity, permission);
    else
      this.permissionChangedTagCallback(this.targetTag, permission);
  }

  protected virtual void UpdateButtonStates(bool isDefault)
  {
    ToolTip component1 = this.leftButton.GetComponent<ToolTip>();
    ToolTip component2 = this.rightButton.GetComponent<ToolTip>();
    if (this.isUpDown)
    {
      component1.SetSimpleTooltip((string) (this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_DISABLED));
      component2.SetSimpleTooltip((string) (this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_DISABLED));
    }
    else
    {
      component1.SetSimpleTooltip((string) (this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_DISABLED));
      component2.SetSimpleTooltip((string) (this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_DISABLED));
    }
  }

  public void SetRotated(bool rotated) => this.isUpDown = rotated;

  public void SetContent(
    AccessControl.Permission permission,
    Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange)
  {
    this.permissionChangedCallback = onPermissionChange;
    this.leftButton.isOn = permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft;
    this.rightButton.isOn = permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight;
    this.UpdateButtonStates(false);
  }

  public void SetDefaultContent(
    Tag defaultTag,
    AccessControl.Permission permission,
    Action<Tag, AccessControl.Permission> onPermissionChange)
  {
    this.SetContent(permission, onPermissionChange);
    this.targetTag = defaultTag;
  }

  public void SetContent(
    AccessControl.Permission permission,
    Action<Tag, AccessControl.Permission> onPermissionChange)
  {
    this.permissionChangedTagCallback = onPermissionChange;
    this.leftButton.isOn = permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft;
    this.rightButton.isOn = permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight;
    this.UpdateButtonStates(false);
  }
}
