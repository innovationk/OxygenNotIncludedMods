// Decompiled with JetBrains decompiler
// Type: OwnablesSecondSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class OwnablesSecondSideScreen : KScreen
{
  public MultiToggle noneRow;
  public OwnablesSecondSideScreenRow originalRow;
  public System.Action OnScreenDeactivated;
  private List<OwnablesSecondSideScreenRow> itemRows = new List<OwnablesSecondSideScreenRow>();

  public AssignableSlotInstance Slot { private set; get; }

  public IAssignableIdentity OwnerIdentity { private set; get; }

  public AssignableSlot SlotType => this.Slot != null ? this.Slot.slot : (AssignableSlot) null;

  public Assignable CurrentSlotItem => !this.HasItem ? (Assignable) null : this.Slot.assignable;

  public bool HasItem => this.Slot != null && this.Slot.IsAssigned();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.originalRow.gameObject.SetActive(false);
    this.noneRow.onClick += new System.Action(this.OnNoneRowClicked);
  }

  private void OnNoneRowClicked()
  {
    this.UnassignCurrentItem();
    this.RefreshNoneRow();
  }

  protected override void OnCmpDisable()
  {
    this.SetSlot((AssignableSlotInstance) null);
    base.OnCmpDisable();
  }

  public void SetSlot(AssignableSlotInstance slot)
  {
    Components.AssignableItems.Unregister(new Action<Assignable>(this.OnNewItemAvailable), new Action<Assignable>(this.OnItemUnregistered));
    this.Slot = slot;
    this.OwnerIdentity = slot == null ? (IAssignableIdentity) null : slot.assignables.GetComponent<IAssignableIdentity>();
    if (this.Slot != null)
      Components.AssignableItems.Register(new Action<Assignable>(this.OnNewItemAvailable), new Action<Assignable>(this.OnItemUnregistered));
    this.RefreshItemListOptions(true);
  }

  public void SortRows()
  {
    if (this.itemRows != null)
    {
      this.itemRows.Sort((Comparison<OwnablesSecondSideScreenRow>) ((a, b) => string.Compare(UI.StripLinkFormatting(a.nameLabel.text), UI.StripLinkFormatting(b.nameLabel.text)) * -1));
      OwnablesSecondSideScreenRow secondSideScreenRow = (OwnablesSecondSideScreenRow) null;
      for (int index = 0; index < this.itemRows.Count; ++index)
      {
        OwnablesSecondSideScreenRow itemRow = this.itemRows[index];
        if ((UnityEngine.Object) itemRow.item == (UnityEngine.Object) null || itemRow.item.IsAssigned())
        {
          if ((UnityEngine.Object) secondSideScreenRow == (UnityEngine.Object) null && (UnityEngine.Object) itemRow != (UnityEngine.Object) null && (UnityEngine.Object) itemRow.item != (UnityEngine.Object) null && itemRow.item.IsAssigned() && (UnityEngine.Object) itemRow.item == (UnityEngine.Object) this.CurrentSlotItem)
            secondSideScreenRow = itemRow;
          else
            itemRow.transform.SetAsLastSibling();
        }
        else
          itemRow.transform.SetAsFirstSibling();
      }
      if ((UnityEngine.Object) secondSideScreenRow != (UnityEngine.Object) null)
        secondSideScreenRow.transform.SetAsFirstSibling();
    }
    this.noneRow.transform.SetAsFirstSibling();
  }

  public void RefreshItemListOptions(bool sortRows = false)
  {
    GameObject targetGameObject = this.OwnerIdentity == null ? (GameObject) null : this.OwnerIdentity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    int worldID = this.OwnerIdentity == null ? (int) byte.MaxValue : targetGameObject.GetMyWorldId();
    List<Assignable> assignableList = (List<Assignable>) null;
    int b = 0;
    bool showItemsAssignedToOthers = true;
    if (this.Slot != null && (this.Slot is EquipmentSlotInstance || this.Slot.ID.Contains("BionicUpgrade")))
      showItemsAssignedToOthers = false;
    if (worldID != (int) byte.MaxValue)
    {
      assignableList = Components.AssignableItems.Items.FindAll((Predicate<Assignable>) (i =>
      {
        bool flag1 = i.slotID == this.SlotType.Id && i.CanAssignTo(this.OwnerIdentity);
        if (flag1 && i is Equippable)
        {
          Equippable equippable = i as Equippable;
          GameObject gameObject = equippable.gameObject;
          if (equippable.isEquipped)
            gameObject = equippable.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
          flag1 = flag1 && gameObject.GetMyWorldId() == worldID;
        }
        bool flag2 = i.assignee != null && (UnityEngine.Object) i.assignee.GetSoleOwner() == (UnityEngine.Object) this.OwnerIdentity.GetSoleOwner();
        bool flag3 = flag2 && (UnityEngine.Object) this.Slot.assignable == (UnityEngine.Object) i;
        if (!showItemsAssignedToOthers)
        {
          if (i.assignee != null && !flag2)
            flag1 = false;
          if (flag2 && !flag3)
            flag1 = false;
        }
        return flag1;
      }));
      b = assignableList.Count;
    }
    for (int index = 0; index < Mathf.Max(this.itemRows.Count, b); ++index)
    {
      if (assignableList != null && index < assignableList.Count)
      {
        Assignable item_assignable = assignableList[index];
        if (index >= this.itemRows.Count)
          this.itemRows.Add(this.CreateItemRow(item_assignable));
        OwnablesSecondSideScreenRow itemRow = this.itemRows[index];
        itemRow.gameObject.SetActive(true);
        itemRow.SetData(this.Slot, item_assignable);
      }
      else
      {
        OwnablesSecondSideScreenRow itemRow = this.itemRows[index];
        itemRow.ClearData();
        itemRow.gameObject.SetActive(false);
      }
    }
    if (sortRows)
      this.SortRows();
    this.RefreshNoneRow();
  }

  private void RefreshNoneRow() => this.noneRow.ChangeState(this.HasItem ? 0 : 1);

  private OwnablesSecondSideScreenRow CreateItemRow(Assignable item)
  {
    OwnablesSecondSideScreenRow component = Util.KInstantiateUI(this.originalRow.gameObject, this.originalRow.transform.parent.gameObject).GetComponent<OwnablesSecondSideScreenRow>();
    component.OnRowClicked += new Action<OwnablesSecondSideScreenRow>(this.OnItemRowClicked);
    component.OnRowItemAssigneeChanged += new Action<OwnablesSecondSideScreenRow>(this.OnItemRowAsigneeChanged);
    component.OnRowItemDestroyed += new Action<OwnablesSecondSideScreenRow>(this.OnItemDestroyed);
    return component;
  }

  private void OnItemDestroyed(OwnablesSecondSideScreenRow correspondingItemRow)
  {
    correspondingItemRow.ClearData();
    correspondingItemRow.gameObject.SetActive(false);
  }

  private void OnItemRowAsigneeChanged(OwnablesSecondSideScreenRow correspondingItemRow)
  {
    correspondingItemRow.Refresh();
    this.RefreshNoneRow();
  }

  private void OnItemRowClicked(OwnablesSecondSideScreenRow rowClicked)
  {
    Assignable assignable = rowClicked.item;
    bool flag = assignable.IsAssigned() && assignable.assignee is AssignmentGroup;
    int num = !assignable.IsAssigned() || !assignable.IsAssignedTo(this.OwnerIdentity) || flag || !this.Slot.IsAssigned() ? 0 : ((UnityEngine.Object) this.Slot.assignable == (UnityEngine.Object) assignable ? 1 : 0);
    if (assignable.IsAssigned())
      assignable.Unassign();
    if (num == 0)
      assignable.Assign(this.OwnerIdentity, this.Slot);
    rowClicked.Refresh();
    this.RefreshNoneRow();
  }

  private void UnassignCurrentItem()
  {
    if (this.Slot == null)
      return;
    this.Slot.Unassign();
    this.RefreshItemListOptions();
  }

  private void OnNewItemAvailable(Assignable item)
  {
    if (this.Slot == null || !(item.slotID == this.SlotType.Id))
      return;
    this.RefreshItemListOptions();
  }

  private void OnItemUnregistered(Assignable item)
  {
    if (this.Slot == null || !(item.slotID == this.SlotType.Id))
      return;
    this.RefreshItemListOptions();
  }
}
