// Decompiled with JetBrains decompiler
// Type: BarterConfirmationScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BarterConfirmationScreen : KModalScreen
{
  [SerializeField]
  private GameObject itemIcon;
  [SerializeField]
  private GameObject filamentIcon;
  [SerializeField]
  private LocText largeCostLabel;
  [SerializeField]
  private LocText largeQuantityLabel;
  [SerializeField]
  private LocText itemLabel;
  [SerializeField]
  private LocText transactionDescriptionLabel;
  [SerializeField]
  private KButton confirmButton;
  [SerializeField]
  private KButton cancelButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private LocText panelHeaderLabel;
  [SerializeField]
  private LocText confirmButtonActionLabel;
  [SerializeField]
  private LocText confirmButtonFilamentLabel;
  [SerializeField]
  private LocText resultLabel;
  [SerializeField]
  private KBatchedAnimController loadingAnimation;
  [SerializeField]
  private GameObject contentContainer;
  [SerializeField]
  private GameObject loadingContainer;
  [SerializeField]
  private GameObject resultContainer;
  [SerializeField]
  private Image resultIcon;
  [SerializeField]
  private LocText mainResultLabel;
  [SerializeField]
  private LocText resultFilamentLabel;
  private bool shouldCloseScreen;

  protected override void OnActivate()
  {
    base.OnActivate();
    this.closeButton.onClick += (System.Action) (() => this.Show(false));
    this.cancelButton.onClick += (System.Action) (() => this.Show(false));
  }

  public void Present(PermitResource permit, bool isPurchase)
  {
    this.Show();
    this.ShowContentContainer(true);
    this.ShowLoadingPanel(false);
    this.HideResultPanel();
    if (isPurchase)
    {
      this.itemIcon.transform.SetAsLastSibling();
      this.filamentIcon.transform.SetAsFirstSibling();
    }
    else
    {
      this.itemIcon.transform.SetAsFirstSibling();
      this.filamentIcon.transform.SetAsLastSibling();
    }
    this.confirmButton.onClick += (System.Action) (() =>
    {
      string serverTypeFromPermit = PermitItems.GetServerTypeFromPermit(permit);
      if (serverTypeFromPermit == null)
        return;
      this.ShowContentContainer(false);
      this.HideResultPanel();
      this.ShowLoadingPanel(true);
      if (isPurchase)
        KleiItems.AddRequestBarterGainItem(serverTypeFromPermit, (KleiItems.ResponseCallback) (result =>
        {
          if (this.IsNullOrDestroyed())
            return;
          this.ShowContentContainer(false);
          this.ShowLoadingPanel(false);
          if (!result.Success)
            this.ShowResultPanel(permit, true, false);
          else
            this.ShowResultPanel(permit, true, true);
        }));
      else
        KleiItems.AddRequestBarterLoseItem(KleiItems.GetItemInstanceID(serverTypeFromPermit), (KleiItems.ResponseCallback) (result =>
        {
          if (this.IsNullOrDestroyed())
            return;
          this.ShowContentContainer(false);
          this.ShowLoadingPanel(false);
          if (!result.Success)
            this.ShowResultPanel(permit, false, false);
          else
            this.ShowResultPanel(permit, false, true);
        }));
    });
    ulong buy_price;
    ulong sell_price;
    PermitItems.TryGetBarterPrice(permit.Id, out buy_price, out sell_price);
    PermitPresentationInfo presentationInfo = permit.GetPermitPresentationInfo();
    this.itemIcon.GetComponent<Image>().sprite = presentationInfo.sprite;
    this.itemLabel.SetText(permit.Name);
    this.transactionDescriptionLabel.SetText((string) (isPurchase ? STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_PRINT : STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_RECYCLE));
    this.panelHeaderLabel.SetText((string) (isPurchase ? STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_PRINT_HEADER : STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_RECYCLE_HEADER));
    this.confirmButtonActionLabel.SetText((string) (isPurchase ? STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.BUY : STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL));
    this.confirmButtonFilamentLabel.SetText(isPurchase ? buy_price.ToString() : $"{UIConstants.ColorPrefixGreen}+{sell_price.ToString()}{UIConstants.ColorSuffix}");
    this.largeCostLabel.SetText(isPurchase ? "x" + buy_price.ToString() : "x" + sell_price.ToString());
  }

  private void Update()
  {
    if (!this.shouldCloseScreen)
      return;
    this.ShowContentContainer(false);
    this.ShowLoadingPanel(false);
    this.HideResultPanel();
    this.Show(false);
  }

  private void ShowContentContainer(bool show) => this.contentContainer.SetActive(show);

  private void ShowLoadingPanel(bool show)
  {
    this.loadingContainer.SetActive(show);
    this.resultLabel.SetText((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.LOADING);
    if (show)
      this.loadingAnimation.Play((HashedString) "loading_rocket", KAnim.PlayMode.Loop);
    else
      this.loadingAnimation.Stop();
    if (show)
      return;
    this.shouldCloseScreen = false;
  }

  private void HideResultPanel() => this.resultContainer.SetActive(false);

  private void ShowResultPanel(PermitResource permit, bool isPurchase, bool transationResult)
  {
    this.resultContainer.SetActive(true);
    if (!transationResult)
    {
      this.resultIcon.sprite = Assets.GetSprite((HashedString) "error_message");
      this.mainResultLabel.SetText((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_ERROR);
      this.panelHeaderLabel.SetText((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_INCOMPLETE_HEADER);
      this.resultFilamentLabel.SetText("");
      KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Failed"));
    }
    else
    {
      this.panelHeaderLabel.SetText((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_COMPLETE_HEADER);
      if (isPurchase)
      {
        this.resultIcon.sprite = permit.GetPermitPresentationInfo().sprite;
        this.resultFilamentLabel.SetText("");
        this.mainResultLabel.SetText((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.PURCHASE_SUCCESS);
        KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Print_Succeed"));
      }
      else
      {
        ulong sell_price;
        PermitItems.TryGetBarterPrice(permit.Id, out ulong _, out sell_price);
        this.resultIcon.sprite = Assets.GetSprite((HashedString) "filament");
        this.resultFilamentLabel.GetComponent<LocText>().SetText("x" + sell_price.ToString());
        this.mainResultLabel.SetText((string) STRINGS.UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL_SUCCESS);
        KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Succeed"));
      }
    }
  }
}
