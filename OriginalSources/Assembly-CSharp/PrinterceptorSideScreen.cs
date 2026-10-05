// Decompiled with JetBrains decompiler
// Type: PrinterceptorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PrinterceptorSideScreen : SideScreenContent
{
  private HijackedHeadquarters.Instance target;
  [SerializeField]
  private KButton printButton;
  [SerializeField]
  private KButton interceptButton;
  [SerializeField]
  private LocText interceptStateLabel;
  [SerializeField]
  private GameObject[] progressIndicators;
  [SerializeField]
  private Image[] databankIcon;
  [SerializeField]
  private LocText databankCountLabel;
  [SerializeField]
  private GameObject meterSection;
  [SerializeField]
  private GameObject lockedSection;

  public override bool IsValidForTarget(GameObject target)
  {
    HijackedHeadquarters.Instance smi = target.GetSMI<HijackedHeadquarters.Instance>();
    return smi != null && smi.IsInsideState((StateMachine.BaseState) smi.sm.operational);
  }

  public override int GetSideScreenSortOrder() => 0;

  protected override void OnSpawn() => base.OnSpawn();

  public override void ScreenUpdate(bool topLevel)
  {
    base.ScreenUpdate(topLevel);
    this.RefreshDisplay();
  }

  private void RefreshDisplay()
  {
    HijackedHeadquarters.Instance smi = this.target.GetSMI<HijackedHeadquarters.Instance>();
    this.interceptStateLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.INTERCEPT_METER, (object) smi.sm.interceptCharges.Get(smi), (object) 3);
    bool flag1 = smi.sm.passcodeUnlocked.Get(smi) && Immigration.Instance.ImmigrantsAvailable && smi.sm.interceptCharges.Get(smi) < 3;
    bool flag2 = this.target.IsInsideState((StateMachine.BaseState) this.target.sm.operational.readyToPrint.pre) || this.target.IsInsideState((StateMachine.BaseState) this.target.sm.operational.readyToPrint.loop);
    this.interceptButton.isInteractable = flag1;
    this.printButton.isInteractable = flag2;
    this.interceptButton.GetComponent<ToolTip>().SetSimpleTooltip((string) (flag1 ? STRINGS.UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.INTERCEPT_TOOLTIP : (smi.sm.interceptCharges.Get(smi) >= 3 ? STRINGS.UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.INTERCEPT_TOOLTIP_DISABLED_TOO_FULL : STRINGS.UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.INTERCEPT_TOOLTIP_DISABLED)));
    this.printButton.GetComponent<ToolTip>().SetSimpleTooltip((string) (flag2 ? STRINGS.UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.PRINT_TOOLTIP : STRINGS.UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.PRINT_TOOLTIP_DISABLED));
    for (int index = 0; index < this.progressIndicators.Length; ++index)
    {
      Image componentInChildren = this.progressIndicators[index].GetComponentInChildren<Image>();
      componentInChildren.sprite = Def.GetUISprite((object) "Headquarters").first;
      componentInChildren.color = index < smi.sm.interceptCharges.Get(smi) ? Color.white : Color.gray;
    }
    this.databankCountLabel.SetText(GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.PRINTERCEPTORSIDESCREEN.DATABANK_COUNT, (object) this.target.GetComponent<Storage>().GetAmountAvailable((Tag) DatabankHelper.ID).ToString()));
    foreach (Image image in this.databankIcon)
      image.sprite = Def.GetUISprite((object) DatabankHelper.ID).first;
    if (this.target.GetSMI<HijackedHeadquarters.Instance>().sm.passcodeUnlocked.Get(this.target))
    {
      this.lockedSection.SetActive(false);
      this.meterSection.SetActive(true);
    }
    else
    {
      this.lockedSection.SetActive(true);
      this.meterSection.SetActive(false);
    }
  }

  public override void SetTarget(GameObject new_target)
  {
    this.target = new_target.GetSMI<HijackedHeadquarters.Instance>();
    this.printButton.ClearOnClick();
    this.interceptButton.ClearOnClick();
    this.printButton.onClick += (System.Action) (() => this.target.ActivatePrintInterface());
    this.interceptButton.onClick += (System.Action) (() => this.target.Intercept());
    this.RefreshDisplay();
  }
}
