// Decompiled with JetBrains decompiler
// Type: HarvestDesignatable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/HarvestDesignatable")]
public class HarvestDesignatable : KMonoBehaviour
{
  public Vector2 iconOffset = Vector2.zero;
  public bool defaultHarvestStateWhenPlanted = true;
  public OccupyArea area;
  [Serialize]
  protected bool isMarkedForHarvest;
  [Serialize]
  private bool isInPlanterBox;
  public bool showUserMenuButtons = true;
  public IManageGrowingStates growingStateManager;
  [Serialize]
  protected bool harvestWhenReady;
  public RectTransform HarvestWhenReadyOverlayIcon;
  private Action<object> onEnableOverlayDelegate;
  private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnCancelDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>((Action<HarvestDesignatable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>((Action<HarvestDesignatable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> SetInPlanterBoxTrueDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>((Action<HarvestDesignatable, object>) ((component, data) => component.SetInPlanterBox(true)));

  public bool InPlanterBox => this.isInPlanterBox;

  public bool MarkedForHarvest
  {
    get => this.isMarkedForHarvest;
    set => this.isMarkedForHarvest = value;
  }

  public bool HarvestWhenReady => this.harvestWhenReady;

  protected HarvestDesignatable()
  {
    this.onEnableOverlayDelegate = new Action<object>(this.OnEnableOverlay);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<HarvestDesignatable>(1309017699, HarvestDesignatable.SetInPlanterBoxTrueDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.growingStateManager = this.GetComponent<IManageGrowingStates>();
    if (this.growingStateManager == null)
      this.growingStateManager = this.gameObject.GetSMI<IManageGrowingStates>();
    if (this.isMarkedForHarvest)
      this.MarkForHarvest();
    Components.HarvestDesignatables.Add(this);
    this.Subscribe<HarvestDesignatable>(493375141, HarvestDesignatable.OnRefreshUserMenuDelegate);
    this.Subscribe<HarvestDesignatable>(2127324410, HarvestDesignatable.OnCancelDelegate);
    Game.Instance.Subscribe(1248612973, this.onEnableOverlayDelegate);
    Game.Instance.Subscribe(1798162660, this.onEnableOverlayDelegate);
    Game.Instance.Subscribe(2015652040, new Action<object>(this.OnDisableOverlay));
    Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
    this.area = this.GetComponent<OccupyArea>();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.HarvestDesignatables.Remove(this);
    this.DestroyOverlayIcon();
    Game.Instance.Unsubscribe(1248612973, this.onEnableOverlayDelegate);
    Game.Instance.Unsubscribe(2015652040, new Action<object>(this.OnDisableOverlay));
    Game.Instance.Unsubscribe(1798162660, this.onEnableOverlayDelegate);
    Game.Instance.Unsubscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
  }

  private void DestroyOverlayIcon()
  {
    if (!((UnityEngine.Object) this.HarvestWhenReadyOverlayIcon != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.HarvestWhenReadyOverlayIcon.gameObject);
    this.HarvestWhenReadyOverlayIcon = (RectTransform) null;
  }

  private void CreateOverlayIcon()
  {
    if ((UnityEngine.Object) this.HarvestWhenReadyOverlayIcon != (UnityEngine.Object) null || !((UnityEngine.Object) this.GetComponent<AttackableBase>() == (UnityEngine.Object) null))
      return;
    this.HarvestWhenReadyOverlayIcon = Util.KInstantiate((Component) Assets.UIPrefabs.HarvestWhenReadyOverlayIcon, GameScreenManager.Instance.worldSpaceCanvas).GetComponent<RectTransform>();
    Extents extents = this.GetComponent<OccupyArea>().GetExtents();
    this.HarvestWhenReadyOverlayIcon.transform.SetPosition(!this.GetComponent<KPrefabID>().HasTag(GameTags.Hanging) ? new Vector3((float) (extents.x + extents.width / 2) + 0.5f, (float) extents.y) + (Vector3) this.iconOffset : new Vector3((float) (extents.x + extents.width / 2) + 0.5f, (float) (extents.y + extents.height)) + (Vector3) this.iconOffset);
    this.RefreshOverlayIcon();
  }

  private void OnDisableOverlay(object data) => this.DestroyOverlayIcon();

  private void OnEnableOverlay(object data)
  {
    if (((Boxed<HashedString>) data).value == OverlayModes.Harvest.ID)
      this.CreateOverlayIcon();
    else
      this.DestroyOverlayIcon();
  }

  private void RefreshOverlayIcon(object data = null)
  {
    if (!((UnityEngine.Object) this.HarvestWhenReadyOverlayIcon != (UnityEngine.Object) null))
      return;
    if (Grid.IsVisible(Grid.PosToCell(this.gameObject)) && this.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId || (UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null && CameraController.Instance.FreeCameraEnabled)
    {
      if (!this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
        this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(true);
    }
    else if (this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
      this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(false);
    HierarchyReferences component = this.HarvestWhenReadyOverlayIcon.GetComponent<HierarchyReferences>();
    if (this.harvestWhenReady)
    {
      Image reference = (Image) component.GetReference("On");
      reference.gameObject.SetActive(true);
      reference.color = (Color) GlobalAssets.Instance.colorSet.harvestEnabled;
      component.GetReference("Off").gameObject.SetActive(false);
    }
    else
    {
      component.GetReference("On").gameObject.SetActive(false);
      Image reference = (Image) component.GetReference("Off");
      reference.gameObject.SetActive(true);
      reference.color = (Color) GlobalAssets.Instance.colorSet.harvestDisabled;
    }
  }

  public bool CanBeHarvested()
  {
    Harvestable component = this.GetComponent<Harvestable>();
    return !((UnityEngine.Object) component != (UnityEngine.Object) null) || component.CanBeHarvested;
  }

  public void SetInPlanterBox(bool state)
  {
    if (state)
    {
      if (this.isInPlanterBox)
        return;
      this.isInPlanterBox = true;
      this.SetHarvestWhenReady(this.defaultHarvestStateWhenPlanted);
    }
    else
      this.isInPlanterBox = false;
  }

  public void SetHarvestWhenReady(bool state)
  {
    this.harvestWhenReady = state;
    if (this.harvestWhenReady && this.CanBeHarvested() && !this.isMarkedForHarvest)
      this.MarkForHarvest();
    if (this.isMarkedForHarvest && !this.harvestWhenReady)
    {
      this.OnCancel();
      if (this.CanBeHarvested() && this.isInPlanterBox)
        this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, (object) this);
    }
    this.Trigger(-266953818, (object) null);
    this.RefreshOverlayIcon();
  }

  protected virtual void OnCancel(object _ = null)
  {
  }

  public virtual void MarkForHarvest()
  {
    if (!this.CanBeHarvested())
      return;
    this.isMarkedForHarvest = true;
    Harvestable component = this.GetComponent<Harvestable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.OnMarkedForHarvest();
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest);
  }

  protected virtual void OnClickHarvestWhenReady() => this.SetHarvestWhenReady(true);

  protected virtual void OnClickCancelHarvestWhenReady()
  {
    Harvestable component = this.GetComponent<Harvestable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.Trigger(2127324410, (object) null);
    this.SetHarvestWhenReady(false);
  }

  public virtual void OnRefreshUserMenu(object data)
  {
    if (!this.showUserMenuButtons)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.harvestWhenReady ? new KIconButtonMenu.ButtonInfo("action_harvest", (string) STRINGS.UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.NAME, (System.Action) (() =>
    {
      this.OnClickCancelHarvestWhenReady();
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) STRINGS.UI.GAMEOBJECTEFFECTS.PLANT_DO_NOT_HARVEST, this.transform);
    }), tooltipText: (string) STRINGS.UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_harvest", (string) STRINGS.UI.USERMENUACTIONS.HARVEST_WHEN_READY.NAME, (System.Action) (() =>
    {
      this.OnClickHarvestWhenReady();
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, (string) STRINGS.UI.GAMEOBJECTEFFECTS.PLANT_MARK_FOR_HARVEST, this.transform);
    }), tooltipText: (string) STRINGS.UI.USERMENUACTIONS.HARVEST_WHEN_READY.TOOLTIP));
  }
}
