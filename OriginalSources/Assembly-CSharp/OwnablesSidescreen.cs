// Decompiled with JetBrains decompiler
// Type: OwnablesSidescreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class OwnablesSidescreen : SideScreenContent
{
  public OwnablesSecondSideScreen selectedSlotScreenPrefab;
  public OwnablesSidescreenCategoryRow originalCategoryRow;
  [Header("Editor Settings")]
  public bool usingSlider = true;
  public GameObject titleSection;
  public GameObject scrollbarSection;
  public VerticalLayoutGroup mainLayoutGroup;
  public KScrollRect scrollRect;
  private OwnablesSidescreenCategoryRow[] categoryRows;
  private AssignableSlotInstance lastSelectedSlot;
  private OwnablesSidescreen.Category[] categories;
  public Action<AssignableSlotInstance> OnSlotInstanceSelected;
  private MinionIdentity lastTarget;
  private int minionDestroyedCallbackIDX = -1;

  private void DefineCategories()
  {
    if (this.categories != null)
      return;
    this.categories = new OwnablesSidescreen.Category[2]
    {
      new OwnablesSidescreen.Category((Func<IAssignableIdentity, Assignables>) (assignableIdentity => (Assignables) (assignableIdentity as MinionIdentity).GetEquipment()), new OwnablesSidescreenCategoryRow.Data((string) STRINGS.UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.SUITS, new OwnablesSidescreenCategoryRow.AssignableSlotData[3]
      {
        new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Suit, new Func<IAssignableIdentity, bool>(this.Always)),
        new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Outfit, new Func<IAssignableIdentity, bool>(this.Always)),
        new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Shoes, new Func<IAssignableIdentity, bool>(this.Always))
      })),
      new OwnablesSidescreen.Category((Func<IAssignableIdentity, Assignables>) (assignableIdentity => (Assignables) assignableIdentity.GetSoleOwner()), new OwnablesSidescreenCategoryRow.Data((string) STRINGS.UI.UISIDESCREENS.OWNABLESSIDESCREEN.CATEGORIES.AMENITIES, new OwnablesSidescreenCategoryRow.AssignableSlotData[3]
      {
        new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Bed, new Func<IAssignableIdentity, bool>(this.Always)),
        new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.Toilet, new Func<IAssignableIdentity, bool>(this.Always)),
        new OwnablesSidescreenCategoryRow.AssignableSlotData(Db.Get().AssignableSlots.MessStation, new Func<IAssignableIdentity, bool>(MessStation.CanBeAssignedTo))
      }))
    };
  }

  private bool Always(IAssignableIdentity identity) => true;

  private Func<IAssignableIdentity, bool> HasAmount(string amountID)
  {
    return (Func<IAssignableIdentity, bool>) (identity =>
    {
      if (identity == null)
        return false;
      GameObject targetGameObject = identity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
      return Db.Get().Amounts.Get(amountID).Lookup(targetGameObject) != null;
    });
  }

  protected override void OnSpawn() => base.OnSpawn();

  private void ActivateSecondSidescreen(AssignableSlotInstance slot)
  {
    ((OwnablesSecondSideScreen) DetailsScreen.Instance.SetSecondarySideScreen((KScreen) this.selectedSlotScreenPrefab, slot.slot.Name)).SetSlot(slot);
    if (slot == null || this.OnSlotInstanceSelected == null)
      return;
    this.OnSlotInstanceSelected(slot);
  }

  private void DeactivateSecondScreen() => DetailsScreen.Instance.ClearSecondarySideScreen();

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.UnsubscribeFromLastTarget();
    this.lastSelectedSlot = (AssignableSlotInstance) null;
    this.DefineCategories();
    this.CreateCategoryRows();
    this.DeactivateSecondScreen();
    this.RefreshSelectedStatusOnRows();
    IAssignableIdentity component = target.GetComponent<IAssignableIdentity>();
    for (int index = 0; index < this.categoryRows.Length; ++index)
    {
      Assignables owner = this.categories[index].getAssignablesFn(component);
      this.categoryRows[index].SetOwner(owner);
    }
    this.titleSection.SetActive(target.GetComponent<MinionIdentity>().model == BionicMinionConfig.MODEL);
    MinionIdentity minionIdentity = component as MinionIdentity;
    if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
      return;
    this.lastTarget = minionIdentity;
    this.minionDestroyedCallbackIDX = minionIdentity.gameObject.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
  }

  private void OnTargetDestroyed(object o) => this.ClearTarget();

  public override void ClearTarget()
  {
    base.ClearTarget();
    this.lastSelectedSlot = (AssignableSlotInstance) null;
    this.RefreshSelectedStatusOnRows();
    for (int index = 0; index < this.categoryRows.Length; ++index)
      this.categoryRows[index].SetOwner((Assignables) null);
    this.DeactivateSecondScreen();
    this.UnsubscribeFromLastTarget();
  }

  private void CreateCategoryRows()
  {
    if (this.categoryRows != null)
      return;
    this.originalCategoryRow.gameObject.SetActive(false);
    this.categoryRows = new OwnablesSidescreenCategoryRow[this.categories.Length];
    for (int index = 0; index < this.categories.Length; ++index)
    {
      OwnablesSidescreenCategoryRow.Data data = this.categories[index].data;
      OwnablesSidescreenCategoryRow component = Util.KInstantiateUI(this.originalCategoryRow.gameObject, this.originalCategoryRow.transform.parent.gameObject).GetComponent<OwnablesSidescreenCategoryRow>();
      component.OnSlotRowClicked += new Action<OwnablesSidescreenItemRow>(this.OnSlotRowClicked);
      component.gameObject.SetActive(true);
      component.SetCategoryData(data);
      this.categoryRows[index] = component;
    }
    this.RefreshSelectedStatusOnRows();
  }

  private void OnSlotRowClicked(OwnablesSidescreenItemRow slotRow)
  {
    if (slotRow.IsLocked || slotRow.SlotInstance == this.lastSelectedSlot)
      this.SetSelectedSlot((AssignableSlotInstance) null);
    else
      this.SetSelectedSlot(slotRow.SlotInstance);
  }

  public void RefreshSelectedStatusOnRows()
  {
    if (this.categoryRows == null)
      return;
    for (int index = 0; index < this.categoryRows.Length; ++index)
      this.categoryRows[index].SetSelectedRow_VisualsOnly(this.lastSelectedSlot);
  }

  public void SetSelectedSlot(AssignableSlotInstance slot)
  {
    this.lastSelectedSlot = slot;
    if (slot != null)
      this.ActivateSecondSidescreen(slot);
    else
      this.DeactivateSecondScreen();
    this.RefreshSelectedStatusOnRows();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.categoryRows != null)
    {
      for (int index = 0; index < this.categoryRows.Length; ++index)
      {
        if ((UnityEngine.Object) this.categoryRows[index] != (UnityEngine.Object) null)
          this.categoryRows[index].SetOwner((Assignables) null);
      }
    }
    this.UnsubscribeFromLastTarget();
  }

  private void UnsubscribeFromLastTarget()
  {
    if ((UnityEngine.Object) this.lastTarget != (UnityEngine.Object) null && this.minionDestroyedCallbackIDX != -1)
      this.lastTarget.Unsubscribe(this.minionDestroyedCallbackIDX);
    this.minionDestroyedCallbackIDX = -1;
    this.lastTarget = (MinionIdentity) null;
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<IAssignableIdentity>() != null;
  }

  public void OnValidate()
  {
  }

  private void SetScrollBarVisibility(bool isVisible)
  {
    this.scrollbarSection.gameObject.SetActive(isVisible);
    this.mainLayoutGroup.padding.right = isVisible ? 20 : 0;
    this.scrollRect.enabled = isVisible;
  }

  public struct Category(
    Func<IAssignableIdentity, Assignables> getAssignablesFn,
    OwnablesSidescreenCategoryRow.Data categoryData)
  {
    public Func<IAssignableIdentity, Assignables> getAssignablesFn = getAssignablesFn;
    public OwnablesSidescreenCategoryRow.Data data = categoryData;
  }
}
