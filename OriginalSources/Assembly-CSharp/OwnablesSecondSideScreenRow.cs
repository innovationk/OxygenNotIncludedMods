// Decompiled with JetBrains decompiler
// Type: OwnablesSecondSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class OwnablesSecondSideScreenRow : KMonoBehaviour
{
  public static string NO_DATA_MESSAGE = (string) STRINGS.UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_FOUND;
  public static string NOT_ASSIGNED = (string) STRINGS.UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.NOT_ASSIGNED;
  public static string ASSIGNED_TO_SELF = (string) STRINGS.UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_SELF_STATUS;
  public static string ASSIGNED_TO_OTHER = (string) STRINGS.UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_OTHER_STATUS;
  public KImage icon;
  public KImage emptyIcon;
  public LocText nameLabel;
  public LocText statusLabel;
  public UnityEngine.UI.Button eyeButton;
  public ToolTip tooltip;
  public Action<OwnablesSecondSideScreenRow> OnRowItemAssigneeChanged;
  public Action<OwnablesSecondSideScreenRow> OnRowItemDestroyed;
  public Action<OwnablesSecondSideScreenRow> OnRowClicked;
  public Func<Assignables, string> customTooltipFunc;
  private MultiToggle toggle;
  private int changeAssignmentListenerIDX = -1;
  private int destroyListenerIDX = -1;

  public AssignableSlotInstance minionSlotInstance { private set; get; }

  public Assignable item { private set; get; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.toggle = this.GetComponent<MultiToggle>();
    this.toggle.onClick += new System.Action(this.OnMultitoggleClicked);
    this.eyeButton.onClick.AddListener(new UnityAction(this.FocusCameraOnAssignedItem));
  }

  public void SetData(AssignableSlotInstance minion, Assignable item_assignable)
  {
    this.minionSlotInstance = minion;
    this.item = item_assignable;
    this.changeAssignmentListenerIDX = this.item.Subscribe(684616645, new Action<object>(this._OnItemAssignationChanged));
    this.destroyListenerIDX = this.item.Subscribe(1969584890, new Action<object>(this._OnRowItemDestroyed));
    this.customTooltipFunc = this.item.customAssignmentUITooltipFunc;
    this.Refresh();
  }

  public void Refresh()
  {
    if ((UnityEngine.Object) this.item != (UnityEngine.Object) null)
    {
      this.item.PrefabID();
      string properName = this.item.GetProperName();
      this.nameLabel.text = properName;
      this.icon.sprite = Def.GetUISprite((object) this.item.gameObject).first;
      bool flag1 = this.item.IsAssigned() && !this.minionSlotInstance.IsUnassigning() && (UnityEngine.Object) this.minionSlotInstance.assignable != (UnityEngine.Object) this.item;
      if (this.item.IsAssigned())
        this.statusLabel.SetText(string.Format(flag1 ? OwnablesSecondSideScreenRow.ASSIGNED_TO_OTHER : OwnablesSecondSideScreenRow.ASSIGNED_TO_SELF, (object) this.item.assignee.GetProperName()));
      else
        this.statusLabel.SetText(OwnablesSecondSideScreenRow.NOT_ASSIGNED);
      if (this.customTooltipFunc == null)
      {
        InfoDescription component = this.item.gameObject.GetComponent<InfoDescription>();
        bool flag2 = (UnityEngine.Object) component != (UnityEngine.Object) null && !string.IsNullOrEmpty(component.description);
        string message = flag2 ? component.description : properName;
        this.tooltip.SizingSetting = flag2 ? ToolTip.ToolTipSizeSetting.MaxWidthWrapContent : ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap;
        this.tooltip.SetSimpleTooltip(message);
      }
      else
      {
        this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
        this.tooltip.SetSimpleTooltip(this.customTooltipFunc(this.minionSlotInstance.assignables));
      }
    }
    else
    {
      this.nameLabel.text = OwnablesSecondSideScreenRow.NO_DATA_MESSAGE;
      this.tooltip.SetSimpleTooltip((string) null);
    }
    this.toggle.ChangeState((UnityEngine.Object) this.item != (UnityEngine.Object) null && this.minionSlotInstance != null && !this.minionSlotInstance.IsUnassigning() && (UnityEngine.Object) this.minionSlotInstance.assignable == (UnityEngine.Object) this.item ? 1 : 0);
    this.emptyIcon.gameObject.SetActive((UnityEngine.Object) this.item == (UnityEngine.Object) null);
    this.icon.gameObject.SetActive((UnityEngine.Object) this.item != (UnityEngine.Object) null);
    this.eyeButton.gameObject.SetActive((UnityEngine.Object) this.item != (UnityEngine.Object) null);
    this.statusLabel.gameObject.SetActive((UnityEngine.Object) this.item != (UnityEngine.Object) null);
  }

  public void ClearData()
  {
    if ((UnityEngine.Object) this.item != (UnityEngine.Object) null)
    {
      if (this.destroyListenerIDX != -1)
        this.item.Unsubscribe(this.destroyListenerIDX);
      if (this.changeAssignmentListenerIDX != -1)
        this.item.Unsubscribe(this.changeAssignmentListenerIDX);
    }
    this.minionSlotInstance = (AssignableSlotInstance) null;
    this.item = (Assignable) null;
    this.destroyListenerIDX = -1;
    this.changeAssignmentListenerIDX = -1;
    this.Refresh();
  }

  private void _OnItemAssignationChanged(object o)
  {
    Action<OwnablesSecondSideScreenRow> itemAssigneeChanged = this.OnRowItemAssigneeChanged;
    if (itemAssigneeChanged == null)
      return;
    itemAssigneeChanged(this);
  }

  private void _OnRowItemDestroyed(object o)
  {
    Action<OwnablesSecondSideScreenRow> rowItemDestroyed = this.OnRowItemDestroyed;
    if (rowItemDestroyed == null)
      return;
    rowItemDestroyed(this);
  }

  private void OnMultitoggleClicked()
  {
    Action<OwnablesSecondSideScreenRow> onRowClicked = this.OnRowClicked;
    if (onRowClicked == null)
      return;
    onRowClicked(this);
  }

  private void FocusCameraOnAssignedItem()
  {
    if (!((UnityEngine.Object) this.item != (UnityEngine.Object) null))
      return;
    GameObject gameObject = this.item.gameObject;
    if (this.item.HasTag(GameTags.Equipped))
      gameObject = this.item.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    GameUtil.FocusCamera(gameObject.transform, false);
  }
}
