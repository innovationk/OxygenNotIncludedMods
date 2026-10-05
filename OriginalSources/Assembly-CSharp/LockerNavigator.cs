// Decompiled with JetBrains decompiler
// Type: LockerNavigator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LockerNavigator : KModalScreen
{
  public static LockerNavigator Instance;
  [SerializeField]
  private RectTransform slot;
  [SerializeField]
  private KButton backButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  public GameObject kleiInventoryScreen;
  [SerializeField]
  public GameObject duplicantCatalogueScreen;
  [SerializeField]
  public GameObject outfitDesignerScreen;
  [SerializeField]
  public GameObject outfitBrowserScreen;
  [SerializeField]
  public GameObject joyResponseDesignerScreen;
  private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";
  private const string MUSIC_PARAMETER = "SupplyClosetView";
  private List<LockerNavigator.HistoryEntry> navigationHistory = new List<LockerNavigator.HistoryEntry>();
  private Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();
  private static bool didDisplayDataCollectionWarningPopupOnce;
  public List<Func<bool>> preventScreenPop = new List<Func<bool>>();

  public GameObject ContentSlot => this.slot.gameObject;

  protected override void OnActivate()
  {
    LockerNavigator.Instance = this;
    this.Show(false);
    this.backButton.onClick += new System.Action(this.OnClickBack);
  }

  public override float GetSortKey() => 41f;

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.PopScreen();
    base.OnKeyDown(e);
  }

  public override void Show(bool show = true)
  {
    base.Show(show);
    if (!show)
      this.PopAllScreens();
    StreamedTextures.SetBundlesLoaded(show);
  }

  private void OnClickBack() => this.PopScreen();

  public void PushScreen(GameObject screen, System.Action onClose = null)
  {
    if ((UnityEngine.Object) screen == (UnityEngine.Object) null)
      return;
    if (this.navigationHistory.Count == 0)
    {
      this.Show(true);
      if (!LockerNavigator.didDisplayDataCollectionWarningPopupOnce && KPrivacyPrefs.instance.disableDataCollection)
      {
        LockerNavigator.MakeDataCollectionWarningPopup(this.gameObject.transform.parent.gameObject);
        LockerNavigator.didDisplayDataCollectionWarningPopupOnce = true;
      }
    }
    if (this.navigationHistory.Count > 0 && (UnityEngine.Object) screen == (UnityEngine.Object) this.navigationHistory[this.navigationHistory.Count - 1].screen)
      return;
    if (this.navigationHistory.Count > 0)
      this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(false);
    this.navigationHistory.Add(new LockerNavigator.HistoryEntry(screen, onClose));
    this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
    if (!this.gameObject.activeSelf)
      this.gameObject.SetActive(true);
    this.RefreshButtons();
  }

  public bool PopScreen()
  {
    while (this.preventScreenPop.Count > 0)
    {
      int index = this.preventScreenPop.Count - 1;
      Func<bool> func = this.preventScreenPop[index];
      this.preventScreenPop.RemoveAt(index);
      if (func())
        return true;
    }
    int index1 = this.navigationHistory.Count - 1;
    LockerNavigator.HistoryEntry historyEntry = this.navigationHistory[index1];
    historyEntry.screen.SetActive(false);
    if (historyEntry.onClose.IsSome())
      historyEntry.onClose.Unwrap()();
    this.navigationHistory.RemoveAt(index1);
    if (this.navigationHistory.Count > 0)
    {
      this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
      this.RefreshButtons();
      return true;
    }
    this.Show(false);
    MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "initial");
    return false;
  }

  public void PopAllScreens()
  {
    if (this.navigationHistory.Count == 0 && this.preventScreenPop.Count == 0)
      return;
    int num = 0;
    while (this.PopScreen())
    {
      if (num > 100)
      {
        DebugUtil.DevAssert(false, $"Can't close all LockerNavigator screens, hit limit of trying to close {100} screens");
        break;
      }
      ++num;
    }
  }

  private void RefreshButtons() => this.backButton.isInteractable = true;

  public void ShowDialogPopup(Action<InfoDialogScreen> configureDialogFn)
  {
    InfoDialogScreen dialog = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.ContentSlot);
    configureDialogFn(dialog);
    dialog.Activate();
    dialog.gameObject.AddOrGet<LayoutElement>().ignoreLayout = true;
    dialog.gameObject.AddOrGet<RectTransform>().Fill();
    Func<bool> preventScreenPopFn = (Func<bool>) (() =>
    {
      dialog.Deactivate();
      return true;
    });
    this.preventScreenPop.Add(preventScreenPopFn);
    dialog.onDeactivateFn += (System.Action) (() => this.preventScreenPop.Remove(preventScreenPopFn));
  }

  public static void MakeDataCollectionWarningPopup(GameObject fullscreenParent)
  {
    LockerNavigator.Instance.ShowDialogPopup((Action<InfoDialogScreen>) (dialog => dialog.SetHeader((string) STRINGS.UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.HEADER).AddPlainText((string) STRINGS.UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BODY).AddOption((string) STRINGS.UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OK, (Action<InfoDialogScreen>) (d => d.Deactivate()), true).AddOption((string) STRINGS.UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OPEN_SETTINGS, (Action<InfoDialogScreen>) (d =>
    {
      d.Deactivate();
      LockerNavigator.Instance.PopAllScreens();
      LockerMenuScreen.Instance.Show(false);
      Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, fullscreenParent, true).ShowMetricsScreen();
    }))));
  }

  public readonly struct HistoryEntry(GameObject screen, System.Action onClose = null)
  {
    public readonly GameObject screen = screen;
    public readonly Option<System.Action> onClose = (Option<System.Action>) onClose;
  }
}
