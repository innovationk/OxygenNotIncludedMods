// Decompiled with JetBrains decompiler
// Type: KleiInventoryDLCFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KleiInventoryDLCFilter : KMonoBehaviour
{
  [SerializeField]
  private Transform dlcFilterButtonContainer;
  [SerializeField]
  private GameObject dlcFilterButtonPrefab;
  [SerializeField]
  private Image selectedDLCIcon;
  [SerializeField]
  private Image selectedDLCStripe;
  [SerializeField]
  private KButton dropdownButton;
  public System.Action onDLCFilterChanged;

  [HideInInspector]
  public string SelectedDLCID { get; set; }

  private void ShowDropdown(bool show) => this.dlcFilterButtonContainer.gameObject.SetActive(show);

  public void ResetToDefault() => this.SetDLCFilter((string) null);

  public void ConfigButtons()
  {
    this.dropdownButton.ClearOnClick();
    this.dropdownButton.onClick += (System.Action) (() => this.ShowDropdown(!this.dlcFilterButtonContainer.gameObject.activeSelf));
    this.MakeButton((string) null);
    List<string> stringList = new List<string>((IEnumerable<string>) DlcManager.GetActiveDLCIds());
    for (int index = stringList.Count - 1; index >= 0; --index)
      this.MakeButton(stringList[index]);
    this.SetDLCFilter((string) null);
  }

  private void MakeButton(string dlcID)
  {
    HierarchyReferences component = Util.KInstantiateUI(this.dlcFilterButtonPrefab, this.dlcFilterButtonContainer.gameObject, true).GetComponent<HierarchyReferences>();
    component.GetReference<Image>("Logo").sprite = dlcID == null ? Assets.GetSprite((HashedString) "ONI_mini_logo") : Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(dlcID));
    component.GetReference<Image>("Stripe").sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcBannerSprite(dlcID));
    component.GetReference<Image>("Stripe").color = dlcID == null ? Color.white : DlcManager.GetDlcBannerColor(dlcID);
    component.GetReference<KButton>("Button").ClearOnClick();
    component.GetReference<KButton>("Button").onClick += (System.Action) (() =>
    {
      this.SetDLCFilter(dlcID);
      this.ShowDropdown(false);
    });
    this.ShowDropdown(false);
  }

  private void SetDLCFilter(string DLCID)
  {
    this.SelectedDLCID = DLCID;
    System.Action dlcFilterChanged = this.onDLCFilterChanged;
    if (dlcFilterChanged != null)
      dlcFilterChanged();
    this.selectedDLCIcon.sprite = DLCID == null ? Assets.GetSprite((HashedString) "ONI_mini_logo") : Assets.GetSprite((HashedString) DlcManager.GetDlcSmallLogo(DLCID));
    this.selectedDLCStripe.color = DLCID == null ? Color.white : DlcManager.GetDlcBannerColor(DLCID);
    this.dropdownButton.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.SafeStringFormat((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.TOOLTIP_DLC_FILTER, DLCID == null ? (object) STRINGS.UI.KLEI_INVENTORY_SCREEN.TOOLTIP_DLC_FILTER_ALL : (object) DlcManager.GetDlcTitle(DLCID)));
  }

  public void HideDropdown() => this.ShowDropdown(false);

  public bool IsDropdownVisible() => this.dlcFilterButtonContainer.gameObject.activeSelf;
}
