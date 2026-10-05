// Decompiled with JetBrains decompiler
// Type: RootMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RootMenu : KScreen
{
  private DetailsScreen detailsScreen;
  private UserMenuScreen userMenu;
  [SerializeField]
  private GameObject detailsScreenPrefab;
  [SerializeField]
  private UserMenuScreen userMenuPrefab;
  private GameObject userMenuParent;
  [SerializeField]
  private TileScreen tileScreen;
  public KScreen buildMenu;
  private List<KScreen> subMenus = new List<KScreen>();
  private TileScreen tileScreenInst;
  public bool canTogglePauseScreen = true;
  public GameObject selectedGO;

  public static void DestroyInstance() => RootMenu.Instance = (RootMenu) null;

  public static RootMenu Instance { get; private set; }

  public override float GetSortKey() => -1f;

  protected override void OnPrefabInit()
  {
    RootMenu.Instance = this;
    this.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
    this.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
    this.Subscribe(Game.Instance.gameObject, -809948329, new Action<object>(this.OnBuildingStatechanged));
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.detailsScreen = Util.KInstantiateUI(this.detailsScreenPrefab, this.gameObject, true).GetComponent<DetailsScreen>();
    this.detailsScreen.gameObject.SetActive(true);
    this.userMenuParent = this.detailsScreen.UserMenuPanel.gameObject;
    this.userMenu = Util.KInstantiateUI(this.userMenuPrefab.gameObject, this.userMenuParent).GetComponent<UserMenuScreen>();
    this.detailsScreen.gameObject.SetActive(false);
    this.userMenu.gameObject.SetActive(false);
  }

  private void OnClickCommon() => this.CloseSubMenus();

  public void AddSubMenu(KScreen sub_menu)
  {
    if (sub_menu.activateOnSpawn)
      sub_menu.Show();
    this.subMenus.Add(sub_menu);
  }

  public void RemoveSubMenu(KScreen sub_menu) => this.subMenus.Remove(sub_menu);

  private void CloseSubMenus()
  {
    foreach (KScreen subMenu in this.subMenus)
    {
      if ((UnityEngine.Object) subMenu != (UnityEngine.Object) null)
      {
        if (subMenu.activateOnSpawn)
          subMenu.gameObject.SetActive(false);
        else
          subMenu.Deactivate();
      }
    }
    this.subMenus.Clear();
  }

  private void OnSelectObject(object data)
  {
    GameObject go = (GameObject) data;
    bool flag = false;
    if ((UnityEngine.Object) go != (UnityEngine.Object) null)
    {
      KPrefabID component = go.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.IsInitialized())
        return;
      flag = (UnityEngine.Object) component != (UnityEngine.Object) null || ICellSelectionProxy.IsSelectionProxy(go);
    }
    if ((UnityEngine.Object) go != (UnityEngine.Object) this.selectedGO)
    {
      if ((UnityEngine.Object) this.selectedGO != (UnityEngine.Object) null)
        this.selectedGO.Unsubscribe(1980521255, new Action<object>(this.TriggerRefresh));
      this.selectedGO = (GameObject) null;
      this.CloseSubMenus();
      if (flag)
      {
        this.selectedGO = go;
        this.selectedGO.Subscribe(1980521255, new Action<object>(this.TriggerRefresh));
        this.AddSubMenu((KScreen) this.detailsScreen);
        this.AddSubMenu((KScreen) this.userMenu);
      }
      this.userMenu.SetSelected(this.selectedGO);
    }
    this.Refresh();
  }

  public void TriggerRefresh(object obj) => this.Refresh();

  public void Refresh()
  {
    if ((UnityEngine.Object) this.selectedGO == (UnityEngine.Object) null)
      return;
    this.detailsScreen.Refresh(this.selectedGO);
    this.userMenu.Refresh(this.selectedGO);
  }

  private void OnBuildingStatechanged(object data)
  {
    GameObject data1 = (GameObject) data;
    if (!((UnityEngine.Object) data1 == (UnityEngine.Object) this.selectedGO))
      return;
    this.OnSelectObject((object) data1);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed && e.TryConsume(Action.Escape) && SelectTool.Instance.enabled)
    {
      if (!this.canTogglePauseScreen)
        return;
      if (this.AreSubMenusOpen())
      {
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back"));
        this.CloseSubMenus();
        SelectTool.Instance.Select((KSelectable) null);
      }
      else if (e.IsAction(Action.Escape))
      {
        if (!SelectTool.Instance.enabled)
          KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
        if (PlayerController.Instance.IsUsingDefaultTool())
        {
          if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
          {
            SelectTool.Instance.Select((KSelectable) null);
          }
          else
          {
            CameraController.Instance.ForcePanningState(false);
            this.TogglePauseScreen();
          }
        }
        else
          Game.Instance.Trigger(288942073, (object) null);
        ToolMenu.Instance.ClearSelection();
        SelectTool.Instance.Activate();
      }
    }
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    base.OnKeyUp(e);
    if (e.Consumed || !e.TryConsume(Action.AlternateView) || !((UnityEngine.Object) this.tileScreenInst != (UnityEngine.Object) null))
      return;
    this.tileScreenInst.Deactivate();
    this.tileScreenInst = (TileScreen) null;
  }

  public void TogglePauseScreen() => PauseScreen.Instance.Show();

  public void ExternalClose() => this.OnClickCommon();

  private void OnUIClear(object data)
  {
    this.CloseSubMenus();
    SelectTool.Instance.Select((KSelectable) null, true);
    if ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current != (UnityEngine.Object) null)
      UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject((GameObject) null);
    else
      Debug.LogWarning((object) "OnUIClear() Event system is null");
  }

  protected override void OnActivate() => base.OnActivate();

  private bool AreSubMenusOpen() => this.subMenus.Count > 0;

  private KToggleMenu.ToggleInfo[] GetFillers()
  {
    HashSet<Tag> tagSet = new HashSet<Tag>();
    List<KToggleMenu.ToggleInfo> toggleInfoList = new List<KToggleMenu.ToggleInfo>();
    foreach (Pickupable pickupable in Components.Pickupables.Items)
    {
      KPrefabID kprefabId = pickupable.KPrefabID;
      if (kprefabId.HasTag(GameTags.Filler) && tagSet.Add(kprefabId.PrefabTag))
      {
        string text = kprefabId.GetComponent<PrimaryElement>().Element.id.ToString();
        toggleInfoList.Add(new KToggleMenu.ToggleInfo(text));
      }
    }
    return toggleInfoList.ToArray();
  }

  public bool IsBuildingChorePanelActive()
  {
    return (UnityEngine.Object) this.detailsScreen != (UnityEngine.Object) null && this.detailsScreen.GetActiveTab() is BuildingChoresPanel;
  }
}
