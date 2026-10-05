// Decompiled with JetBrains decompiler
// Type: OwnablesSidescreenItemRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
public class OwnablesSidescreenItemRow : KMonoBehaviour
{
  private static string EMPTY_TEXT = (string) UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_ASSIGNED;
  public KImage lockedIcon;
  public KImage itemIcon;
  public LocText textLabel;
  public ToolTip tooltip;
  [Header("Icon settings")]
  public KImage frameOuterBorder;
  public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;
  public MultiToggle toggle;
  private int subscribe_IDX = -1;

  public bool IsLocked { private set; get; }

  public bool SlotIsAssigned
  {
    get
    {
      return this.Slot != null && this.SlotInstance != null && !this.SlotInstance.IsUnassigning() && this.SlotInstance.IsAssigned();
    }
  }

  public AssignableSlotInstance SlotInstance { private set; get; }

  public AssignableSlot Slot { private set; get; }

  public Assignables Owner { private set; get; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.toggle.onClick += new System.Action(this.OnRowClicked);
    this.SetSelectedVisualState(false);
  }

  private void OnRowClicked()
  {
    Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
    if (onSlotRowClicked == null)
      return;
    onSlotRowClicked(this);
  }

  public void SetLockState(bool locked)
  {
    this.IsLocked = locked;
    this.Refresh();
  }

  public void SetData(Assignables owner, AssignableSlot slot, bool IsLocked)
  {
    if ((UnityEngine.Object) this.Owner != (UnityEngine.Object) null)
      this.ClearData();
    this.Owner = owner;
    this.Slot = slot;
    this.SlotInstance = owner.GetSlot(slot);
    this.subscribe_IDX = this.Owner.Subscribe(-1585839766, (Action<object>) (o => this.Refresh()));
    this.SetLockState(IsLocked);
    if (IsLocked)
      return;
    this.Refresh();
  }

  public void ClearData()
  {
    if ((UnityEngine.Object) this.Owner != (UnityEngine.Object) null && this.subscribe_IDX != -1)
      this.Owner.Unsubscribe(this.subscribe_IDX);
    this.Owner = (Assignables) null;
    this.Slot = (AssignableSlot) null;
    this.SlotInstance = (AssignableSlotInstance) null;
    this.IsLocked = false;
    this.subscribe_IDX = -1;
    this.DisplayAsEmpty();
  }

  private void Refresh()
  {
    if (this.IsNullOrDestroyed())
      return;
    if (this.IsLocked)
      this.DisplayAsLocked();
    else if (!this.SlotIsAssigned)
      this.DisplayAsEmpty();
    else
      this.DisplayAsOccupied();
  }

  public void SetSelectedVisualState(bool shouldDisplayAsSelected)
  {
    this.toggle.ChangeState(shouldDisplayAsSelected ? 1 : 0);
  }

  private void DisplayAsOccupied()
  {
    Assignable assignable = this.SlotInstance.assignable;
    string properName = assignable.GetProperName();
    this.textLabel.SetText($"{this.Slot.Name}: {properName}");
    this.itemIcon.sprite = Def.GetUISprite((object) assignable.gameObject).first;
    this.itemIcon.gameObject.SetActive(true);
    this.lockedIcon.gameObject.SetActive(false);
    InfoDescription component = assignable.gameObject.GetComponent<InfoDescription>();
    string message = string.Format((string) UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED_GENERIC, (object) properName);
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && !string.IsNullOrEmpty(component.description))
      message = string.Format((string) UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED, (object) properName, (object) component.description);
    this.tooltip.SetSimpleTooltip(message);
  }

  private void DisplayAsEmpty()
  {
    this.textLabel.SetText((this.Slot != null ? this.Slot.Name + ": " : "") + OwnablesSidescreenItemRow.EMPTY_TEXT);
    this.lockedIcon.gameObject.SetActive(false);
    this.itemIcon.sprite = (Sprite) null;
    this.itemIcon.gameObject.SetActive(false);
    this.tooltip.SetSimpleTooltip(this.Slot != null ? string.Format((string) UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_ITEM_ASSIGNED, (object) this.Slot.Name) : (string) null);
  }

  private void DisplayAsLocked()
  {
    this.lockedIcon.gameObject.SetActive(true);
    this.itemIcon.sprite = (Sprite) null;
    this.itemIcon.gameObject.SetActive(false);
    this.textLabel.SetText(string.Format((string) UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_APPLICABLE, (object) this.Slot.Name));
    this.tooltip.SetSimpleTooltip(string.Format((string) UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_APPLICABLE, (object) this.Slot.Name));
  }

  protected override void OnCleanUp() => this.ClearData();
}
