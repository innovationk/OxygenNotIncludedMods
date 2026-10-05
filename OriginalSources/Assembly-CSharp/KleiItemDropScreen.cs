// Decompiled with JetBrains decompiler
// Type: KleiItemDropScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KleiItemDropScreen : KModalScreen
{
  [SerializeField]
  private RectTransform shieldMaskRect;
  [SerializeField]
  private KButton closeButton;
  [Header("Animated Item")]
  [SerializeField]
  private KleiItemDropScreen_PermitVis permitVisualizer;
  [SerializeField]
  private KBatchedAnimController animatedPod;
  [SerializeField]
  private LocText userMessageLabel;
  [SerializeField]
  private LocText unopenedItemCountLabel;
  [Header("Item Info")]
  [SerializeField]
  private RectTransform itemTextContainer;
  [SerializeField]
  private LocText itemNameLabel;
  [SerializeField]
  private LocText itemDescriptionLabel;
  [SerializeField]
  private LocText itemRarityLabel;
  [SerializeField]
  private LocText itemCategoryLabel;
  [Header("Accept Button")]
  [SerializeField]
  private RectTransform acceptButtonRect;
  [SerializeField]
  private KButton acceptButton;
  [SerializeField]
  private KBatchedAnimController animatedLoadingIcon;
  [SerializeField]
  private KButton acknowledgeButton;
  [SerializeField]
  private LocText errorMessage;
  private Coroutine activePresentationRoutine;
  private KleiItemDropScreen.ServerRequestState serverRequestState;
  private bool giftAcknowledged;
  private bool noItemAvailableAcknowledged;
  public static KleiItemDropScreen Instance;
  private bool shouldDoCloseRoutine;
  private const float TEXT_AND_BUTTON_ANIMATE_OFFSET_Y = -30f;
  private PrefabDefinedUIPosition acceptButtonPosition = new PrefabDefinedUIPosition();
  private PrefabDefinedUIPosition itemTextContainerPosition = new PrefabDefinedUIPosition();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    KleiItemDropScreen.Instance = this;
    this.closeButton.onClick += (System.Action) (() => this.Show(false));
    if (!string.IsNullOrEmpty(KleiAccount.KleiToken))
      return;
    base.Show(false);
  }

  protected override void OnActivate()
  {
    KleiItemDropScreen.Instance = this;
    this.Show(false);
  }

  public override void Show(bool show = true)
  {
    this.serverRequestState.Reset();
    if (!show)
    {
      this.animatedLoadingIcon.gameObject.SetActive(false);
      if (this.activePresentationRoutine != null)
        this.StopCoroutine(this.activePresentationRoutine);
      if (this.shouldDoCloseRoutine)
      {
        this.closeButton.gameObject.SetActive(false);
        Updater.RunRoutine((MonoBehaviour) this, this.AnimateScreenOutRoutine()).Then((System.Action) (() => base.Show(false)));
        this.shouldDoCloseRoutine = false;
      }
      else
        base.Show(false);
      AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot);
    }
    else
    {
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot);
      base.Show();
    }
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Show(false);
    base.OnKeyDown(e);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!show)
      return;
    if (PermitItems.HasUnopenedItem())
    {
      this.PresentNextUnopenedItem();
      this.shouldDoCloseRoutine = true;
    }
    else
    {
      this.userMessageLabel.SetText((string) STRINGS.UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
      this.PresentNoItemAvailablePrompt(true);
      this.shouldDoCloseRoutine = true;
    }
  }

  public void PresentNextUnopenedItem(bool firstItemPresentation = true)
  {
    int num = 0;
    foreach (KleiItems.ItemData itemData in PermitItems.IterateInventory())
    {
      if (!itemData.IsOpened)
        ++num;
    }
    this.RefreshUnopenedItemsLabel();
    foreach (KleiItems.ItemData itemData in PermitItems.IterateInventory())
    {
      if (!itemData.IsOpened)
      {
        this.PresentItem(itemData, firstItemPresentation, num == 1);
        return;
      }
    }
    this.PresentNoItemAvailablePrompt(false);
  }

  private void RefreshUnopenedItemsLabel()
  {
    int num = 0;
    foreach (KleiItems.ItemData itemData in PermitItems.IterateInventory())
    {
      if (!itemData.IsOpened)
        ++num;
    }
    if (num > 1)
    {
      this.unopenedItemCountLabel.gameObject.SetActive(true);
      this.unopenedItemCountLabel.SetText((string) STRINGS.UI.ITEM_DROP_SCREEN.UNOPENED_ITEM_COUNT, (float) num);
    }
    else if (num == 1)
    {
      this.unopenedItemCountLabel.gameObject.SetActive(true);
      this.unopenedItemCountLabel.SetText((string) STRINGS.UI.ITEM_DROP_SCREEN.UNOPENED_ITEM, (float) num);
    }
    else
      this.unopenedItemCountLabel.gameObject.SetActive(false);
  }

  public void PresentItem(
    KleiItems.ItemData item,
    bool firstItemPresentation,
    bool lastItemPresentation)
  {
    this.userMessageLabel.SetText((string) STRINGS.UI.ITEM_DROP_SCREEN.THANKS_FOR_PLAYING);
    this.giftAcknowledged = false;
    this.serverRequestState.revealConfirmedByServer = false;
    this.serverRequestState.revealRejectedByServer = false;
    if (this.activePresentationRoutine != null)
      this.StopCoroutine(this.activePresentationRoutine);
    this.activePresentationRoutine = this.StartCoroutine(this.PresentItemRoutine(item, firstItemPresentation, lastItemPresentation));
    this.acceptButton.ClearOnClick();
    this.acknowledgeButton.ClearOnClick();
    this.acceptButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.ITEM_DROP_SCREEN.PRINT_ITEM_BUTTON);
    this.acceptButton.onClick += (System.Action) (() => this.RequestReveal(item));
    this.acknowledgeButton.onClick += (System.Action) (() =>
    {
      if (!this.serverRequestState.revealConfirmedByServer)
        return;
      this.giftAcknowledged = true;
    });
  }

  private void RequestReveal(KleiItems.ItemData item)
  {
    this.serverRequestState.revealRequested = true;
    PermitItems.QueueRequestOpenOrUnboxItem(item, new KleiItems.ResponseCallback(this.OnOpenItemRequestResponse));
  }

  public void OnOpenItemRequestResponse(KleiItems.Result result)
  {
    if (!this.serverRequestState.revealRequested)
      return;
    this.serverRequestState.revealRequested = false;
    if (result.Success)
    {
      this.serverRequestState.revealRejectedByServer = false;
      this.serverRequestState.revealConfirmedByServer = true;
    }
    else
    {
      this.serverRequestState.revealRejectedByServer = true;
      this.serverRequestState.revealConfirmedByServer = false;
    }
  }

  public void PresentNoItemAvailablePrompt(bool firstItemPresentation)
  {
    this.userMessageLabel.SetText((string) STRINGS.UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
    this.noItemAvailableAcknowledged = false;
    this.acknowledgeButton.ClearOnClick();
    this.acceptButton.ClearOnClick();
    this.acceptButton.GetComponentInChildren<LocText>().SetText((string) STRINGS.UI.ITEM_DROP_SCREEN.DISMISS_BUTTON);
    this.acceptButton.onClick += (System.Action) (() => this.noItemAvailableAcknowledged = true);
    if (this.activePresentationRoutine != null)
      this.StopCoroutine(this.activePresentationRoutine);
    this.activePresentationRoutine = this.StartCoroutine(this.PresentNoItemAvailableRoutine(firstItemPresentation));
  }

  private IEnumerator AnimateScreenInRoutine()
  {
    float scaleFactor = this.transform.parent.GetComponent<CanvasScaler>().scaleFactor;
    float OPEN_WIDTH = (float) Screen.width / scaleFactor;
    float y = Mathf.Clamp((float) Screen.height / scaleFactor, 720f, 900f);
    KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Open"));
    this.userMessageLabel.gameObject.SetActive(false);
    yield return (object) Updater.Ease((Action<Vector2>) (v2 => this.shieldMaskRect.sizeDelta = v2), this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, y), 0.5f, Easing.CircInOut);
    yield return (object) Updater.Ease((Action<Vector2>) (v2 => this.shieldMaskRect.sizeDelta = v2), this.shieldMaskRect.sizeDelta, new Vector2(OPEN_WIDTH, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut);
    this.userMessageLabel.gameObject.SetActive(true);
  }

  private IEnumerator AnimateScreenOutRoutine()
  {
    KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Close"));
    this.userMessageLabel.gameObject.SetActive(false);
    yield return (object) Updater.Ease((Action<Vector2>) (v2 => this.shieldMaskRect.sizeDelta = v2), this.shieldMaskRect.sizeDelta, new Vector2(8f, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut);
    yield return (object) Updater.Ease((Action<Vector2>) (v2 => this.shieldMaskRect.sizeDelta = v2), this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, 0.0f), 0.25f, Easing.CircInOut);
  }

  private IEnumerator PresentNoItemAvailableRoutine(bool firstItem)
  {
    yield return (object) null;
    this.itemNameLabel.SetText("");
    this.itemDescriptionLabel.SetText("");
    this.itemRarityLabel.SetText("");
    this.itemCategoryLabel.SetText("");
    if (firstItem)
    {
      this.animatedPod.Play((HashedString) "idle", KAnim.PlayMode.Loop);
      this.acceptButtonRect.gameObject.SetActive(false);
      this.shieldMaskRect.sizeDelta = new Vector2(8f, 0.0f);
      this.shieldMaskRect.gameObject.SetActive(true);
    }
    if (firstItem)
    {
      this.closeButton.gameObject.SetActive(false);
      yield return (object) Updater.WaitForSeconds(0.5f);
      yield return (object) this.AnimateScreenInRoutine();
      yield return (object) Updater.WaitForSeconds(0.125f);
      this.closeButton.gameObject.SetActive(true);
    }
    else
      yield return (object) Updater.WaitForSeconds(0.25f);
    Vector2 animate_offset = new Vector2(0.0f, -30f);
    this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0.0f;
    this.acceptButtonRect.gameObject.SetActive(true);
    this.acceptButtonPosition.SetOn((Component) this.acceptButtonRect);
    yield return (object) Updater.WaitForSeconds(0.75f);
    yield return (object) PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
    yield return (object) Updater.Until((Func<bool>) (() => this.noItemAvailableAcknowledged));
    yield return (object) PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0.0f, 0.125f, Easing.SmoothStep);
    this.Show(false);
  }

  private IEnumerator PresentItemRoutine(KleiItems.ItemData item, bool firstItem, bool lastItem)
  {
    yield return (object) null;
    if (item.ItemId == 0UL)
    {
      Debug.LogError((object) "Could not find dropped item inventory.");
    }
    else
    {
      this.itemNameLabel.SetText("");
      this.itemDescriptionLabel.SetText("");
      this.itemRarityLabel.SetText("");
      this.itemCategoryLabel.SetText("");
      this.permitVisualizer.ResetState();
      if (firstItem)
      {
        this.animatedPod.Play((HashedString) "idle", KAnim.PlayMode.Loop);
        this.acceptButtonRect.gameObject.SetActive(false);
        this.shieldMaskRect.sizeDelta = new Vector2(8f, 0.0f);
        this.shieldMaskRect.gameObject.SetActive(true);
      }
      if (firstItem)
      {
        this.closeButton.gameObject.SetActive(false);
        yield return (object) Updater.WaitForSeconds(0.5f);
        yield return (object) this.AnimateScreenInRoutine();
        yield return (object) Updater.WaitForSeconds(0.125f);
        this.closeButton.gameObject.SetActive(true);
      }
      Vector2 animate_offset = new Vector2(0.0f, -30f);
      if (firstItem)
      {
        this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0.0f;
        this.acceptButtonRect.gameObject.SetActive(true);
        this.acceptButtonPosition.SetOn((Component) this.acceptButtonRect);
        this.animatedPod.Play((HashedString) "powerup");
        this.animatedPod.Queue((HashedString) "working_loop", KAnim.PlayMode.Loop);
        yield return (object) Updater.WaitForSeconds(1.25f);
        yield return (object) PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
        yield return (object) Updater.Until((Func<bool>) (() => this.serverRequestState.revealRequested));
        yield return (object) PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0.0f, 0.125f, Easing.SmoothStep);
      }
      else
        this.RequestReveal(item);
      this.animatedLoadingIcon.gameObject.rectTransform().anchoredPosition = new Vector2(0.0f, -352f);
      if ((UnityEngine.Object) this.animatedLoadingIcon.GetComponent<CanvasGroup>() != (UnityEngine.Object) null)
        this.animatedLoadingIcon.GetComponent<CanvasGroup>().alpha = 1f;
      yield return (object) new WaitForSecondsRealtime(0.3f);
      if (!this.serverRequestState.revealConfirmedByServer && !this.serverRequestState.revealRejectedByServer)
      {
        this.animatedLoadingIcon.gameObject.SetActive(true);
        this.animatedLoadingIcon.Play((HashedString) "loading_rocket", KAnim.PlayMode.Loop);
        yield return (object) Updater.Until((Func<bool>) (() => this.serverRequestState.revealConfirmedByServer || this.serverRequestState.revealRejectedByServer));
        yield return (object) new WaitForSecondsRealtime(2f);
        yield return (object) PresUtil.OffsetFromAndFade(this.animatedLoadingIcon.gameObject.rectTransform(), new Vector2(0.0f, -512f), 0.0f, 0.25f, Easing.SmoothStep);
        this.animatedLoadingIcon.gameObject.SetActive(false);
      }
      if (this.serverRequestState.revealRejectedByServer)
      {
        this.animatedPod.Play((HashedString) "idle", KAnim.PlayMode.Loop);
        this.errorMessage.gameObject.SetActive(true);
        yield return (object) Updater.WaitForSeconds(3f);
        this.errorMessage.gameObject.SetActive(false);
      }
      else if (this.serverRequestState.revealConfirmedByServer)
      {
        float num = 1f;
        this.animatedPod.PlaySpeedMultiplier = firstItem ? 1f : 1f * num;
        this.animatedPod.Play((HashedString) "additional_pre");
        this.animatedPod.Queue((HashedString) "working_loop", KAnim.PlayMode.Loop);
        yield return (object) Updater.WaitForSeconds(firstItem ? 1f : 1f / num);
        this.animatedPod.PlaySpeedMultiplier = 1f;
        this.RefreshUnopenedItemsLabel();
        DropScreenPresentationInfo info;
        info.UseEquipmentVis = false;
        info.BuildOverride = (string) null;
        info.Sprite = (Sprite) null;
        string name = "";
        string desc = "";
        PermitRarity rarity = PermitRarity.Unknown;
        string categoryString = "";
        string icon_name;
        if (PermitItems.TryGetBoxInfo(item, out name, out desc, out icon_name))
        {
          info.UseEquipmentVis = false;
          info.BuildOverride = (string) null;
          info.Sprite = Assets.GetSprite((HashedString) icon_name);
          rarity = PermitRarity.Loyalty;
        }
        else
        {
          PermitResource permitResource = Db.Get().Permits.Get(item.Id);
          info.Sprite = permitResource.GetPermitPresentationInfo().sprite;
          info.UseEquipmentVis = permitResource.Category == PermitCategory.Equipment;
          if (permitResource is EquippableFacadeResource)
            info.BuildOverride = (permitResource as EquippableFacadeResource).BuildOverride;
          name = permitResource.Name;
          desc = permitResource.Description;
          rarity = permitResource.Rarity;
          switch (permitResource.Category)
          {
            case PermitCategory.Building:
              categoryString = Assets.GetPrefab((Tag) (permitResource as BuildingFacadeResource).PrefabID).GetProperName();
              break;
            case PermitCategory.Artwork:
              categoryString = PermitCategories.GetDisplayName(permitResource.Category);
              if (permitResource is ArtableStage)
              {
                categoryString = Assets.GetPrefab((Tag) (permitResource as ArtableStage).prefabId).GetProperName();
                break;
              }
              break;
            case PermitCategory.JoyResponse:
              categoryString = PermitCategories.GetDisplayName(permitResource.Category);
              if (permitResource is BalloonArtistFacadeResource)
              {
                categoryString = $"{PermitCategories.GetDisplayName(permitResource.Category)}: {(string) STRINGS.UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST}";
                break;
              }
              break;
            default:
              categoryString = PermitCategories.GetDisplayName(permitResource.Category);
              break;
          }
        }
        this.permitVisualizer.ConfigureWith(info);
        yield return (object) this.permitVisualizer.AnimateIn();
        KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("GiftItemDrop_Rarity"), "GiftItemRarity", $"{rarity}");
        this.itemNameLabel.SetText(name);
        this.itemDescriptionLabel.SetText(desc);
        this.itemRarityLabel.SetText(rarity.GetLocStringName());
        this.itemCategoryLabel.SetText(categoryString);
        this.itemTextContainerPosition.SetOn((Component) this.itemTextContainer);
        yield return (object) Updater.Parallel((Updater) PresUtil.OffsetToAndFade(this.itemTextContainer.rectTransform(), animate_offset, 1f, 0.125f, Easing.CircInOut));
        yield return (object) Updater.Until((Func<bool>) (() => this.giftAcknowledged));
        if (lastItem)
        {
          this.animatedPod.Play((HashedString) "working_pst");
          this.animatedPod.Queue((HashedString) "idle", KAnim.PlayMode.Loop);
          yield return (object) Updater.Parallel((Updater) PresUtil.OffsetFromAndFade(this.itemTextContainer.rectTransform(), animate_offset, 0.0f, 0.125f, Easing.CircInOut));
          this.itemNameLabel.SetText("");
          this.itemDescriptionLabel.SetText("");
          this.itemRarityLabel.SetText("");
          this.itemCategoryLabel.SetText("");
          yield return (object) this.permitVisualizer.AnimateOut();
        }
        else
        {
          this.itemNameLabel.SetText("");
          this.itemDescriptionLabel.SetText("");
          this.itemRarityLabel.SetText("");
          this.itemCategoryLabel.SetText("");
        }
        name = (string) null;
        desc = (string) null;
        categoryString = (string) null;
      }
      this.PresentNextUnopenedItem(false);
    }
  }

  public static bool HasItemsToShow() => PermitItems.HasUnopenedItem();

  private struct ServerRequestState
  {
    public bool revealRequested;
    public bool revealConfirmedByServer;
    public bool revealRejectedByServer;

    public void Reset()
    {
      this.revealRequested = false;
      this.revealConfirmedByServer = false;
      this.revealRejectedByServer = false;
    }
  }
}
