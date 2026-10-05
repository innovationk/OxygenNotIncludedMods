// Decompiled with JetBrains decompiler
// Type: OwnablesSidescreenCategoryRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class OwnablesSidescreenCategoryRow : KMonoBehaviour
{
  public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;
  public LocText titleLabel;
  public OwnablesSidescreenItemRow originalItemRow;
  private Assignables owner;
  private OwnablesSidescreenCategoryRow.Data data;
  private OwnablesSidescreenItemRow[] itemRows;

  private AssignableSlot[] slots => this.data.slots;

  public void SetCategoryData(OwnablesSidescreenCategoryRow.Data categoryData)
  {
    this.DeleteAllRows();
    this.data = categoryData;
    this.titleLabel.text = categoryData.name;
  }

  public void SetOwner(Assignables owner)
  {
    this.owner = owner;
    if ((UnityEngine.Object) owner != (UnityEngine.Object) null)
      this.RecreateAllItemRows();
    else
      this.DeleteAllRows();
  }

  private void RecreateAllItemRows()
  {
    this.DeleteAllRows();
    this.itemRows = new OwnablesSidescreenItemRow[this.slots.Length];
    IAssignableIdentity component = this.owner.gameObject.GetComponent<IAssignableIdentity>();
    for (int index = 0; index < this.slots.Length; ++index)
    {
      AssignableSlot slot = this.slots[index];
      this.itemRows[index] = this.CreateRow(slot, component);
    }
  }

  private OwnablesSidescreenItemRow CreateRow(
    AssignableSlot slot,
    IAssignableIdentity ownerIdentity)
  {
    this.originalItemRow.gameObject.SetActive(false);
    OwnablesSidescreenItemRow component = Util.KInstantiateUI(this.originalItemRow.gameObject, this.originalItemRow.transform.parent.gameObject).GetComponent<OwnablesSidescreenItemRow>();
    component.OnSlotRowClicked += new Action<OwnablesSidescreenItemRow>(this.OnRowClicked);
    component.gameObject.SetActive(true);
    component.SetData(this.owner, slot, !this.data.IsSlotApplicable(ownerIdentity, slot));
    return component;
  }

  private void OnRowClicked(OwnablesSidescreenItemRow row)
  {
    Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
    if (onSlotRowClicked == null)
      return;
    onSlotRowClicked(row);
  }

  private void DeleteAllRows()
  {
    this.originalItemRow.gameObject.SetActive(false);
    if (this.itemRows == null)
      return;
    for (int index = 0; index < this.itemRows.Length; ++index)
    {
      this.itemRows[index].ClearData();
      this.itemRows[index].DeleteObject();
    }
    this.itemRows = (OwnablesSidescreenItemRow[]) null;
  }

  public void SetSelectedRow_VisualsOnly(AssignableSlotInstance slotInstance)
  {
    if (this.itemRows == null)
      return;
    for (int index = 0; index < this.itemRows.Length; ++index)
    {
      OwnablesSidescreenItemRow itemRow = this.itemRows[index];
      itemRow.SetSelectedVisualState(itemRow.SlotInstance == slotInstance);
    }
  }

  public struct AssignableSlotData(
    AssignableSlot slot,
    Func<IAssignableIdentity, bool> isApplicableCallback)
  {
    public AssignableSlot slot = slot;
    public Func<IAssignableIdentity, bool> IsApplicableCallback = isApplicableCallback;
  }

  public struct Data
  {
    public string name;
    public AssignableSlot[] slots;
    private OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData;

    public Data(
      string name,
      OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData)
    {
      this.name = name;
      this.slotsData = slotsData;
      this.slots = new AssignableSlot[slotsData.Length];
      for (int index = 0; index < slotsData.Length; ++index)
        this.slots[index] = slotsData[index].slot;
    }

    public bool IsSlotApplicable(IAssignableIdentity identity, AssignableSlot slot)
    {
      for (int index = 0; index < this.slotsData.Length; ++index)
      {
        OwnablesSidescreenCategoryRow.AssignableSlotData assignableSlotData = this.slotsData[index];
        if (assignableSlotData.slot == slot)
          return assignableSlotData.IsApplicableCallback(identity);
      }
      return false;
    }
  }
}
