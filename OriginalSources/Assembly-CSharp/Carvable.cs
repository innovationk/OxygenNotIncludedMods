// Decompiled with JetBrains decompiler
// Type: Carvable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Carvable")]
public class Carvable : Workable, IDigActionEntity
{
  [Serialize]
  protected bool isMarkedForCarve;
  protected Chore chore;
  private string buttonLabel;
  private string buttonTooltip;
  private string cancelButtonLabel;
  private string cancelButtonTooltip;
  private StatusItem pendingStatusItem;
  public bool showUserMenuButtons = true;
  public string dropItemPrefabId;
  public HandleVector<int>.Handle partitionerEntry;
  private static readonly EventSystem.IntraObjectHandler<Carvable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Carvable>((Action<Carvable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<Carvable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Carvable>((Action<Carvable, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  public bool IsMarkedForCarve => this.isMarkedForCarve;

  protected Carvable()
  {
    this.buttonLabel = (string) UI.USERMENUACTIONS.CARVE.NAME;
    this.buttonTooltip = (string) UI.USERMENUACTIONS.CARVE.TOOLTIP;
    this.cancelButtonLabel = (string) UI.USERMENUACTIONS.CANCELCARVE.NAME;
    this.cancelButtonTooltip = (string) UI.USERMENUACTIONS.CANCELCARVE.TOOLTIP;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.pendingStatusItem = new StatusItem("PendingCarve", "MISC", "status_item_pending_carve", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.workerStatusItem = new StatusItem("Carving", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID);
    this.workerStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      Workable workable = (Workable) data;
      if ((UnityEngine.Object) workable != (UnityEngine.Object) null && (UnityEngine.Object) workable.GetComponent<KSelectable>() != (UnityEngine.Object) null)
        str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
      return str;
    });
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_sculpture_kanim")
    };
    this.synchronizeAnims = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.SetWorkTime(10f);
    this.Subscribe<Carvable>(2127324410, Carvable.OnCancelDelegate);
    this.Subscribe<Carvable>(493375141, Carvable.OnRefreshUserMenuDelegate);
    this.faceTargetWhenWorking = true;
    Prioritizable.AddRef(this.gameObject);
    OccupyArea component = this.gameObject.GetComponent<OccupyArea>();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    foreach (CellOffset occupiedCellsOffset in component.OccupiedCellsOffsets)
      Grid.ObjectLayers[5][Grid.OffsetCell(cell, occupiedCellsOffset)] = this.gameObject;
    if (!this.isMarkedForCarve)
      return;
    this.MarkForCarve();
  }

  public void Carve()
  {
    this.isMarkedForCarve = false;
    this.chore = (Chore) null;
    this.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem);
    this.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem);
    Game.Instance.userMenu.Refresh(this.gameObject);
    this.ProducePickupable(this.dropItemPrefabId);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void MarkForCarve(bool instantOnDebug = true)
  {
    if (DebugHandler.InstantBuildMode & instantOnDebug)
    {
      this.Carve();
    }
    else
    {
      if (this.chore != null)
        return;
      this.isMarkedForCarve = true;
      this.chore = (Chore) new WorkChore<Carvable>(Db.Get().ChoreTypes.Dig, (IStateMachineTarget) this);
      this.chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot);
      this.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, (object) this);
    }
  }

  protected override void OnCompleteWork(WorkerBase worker) => this.Carve();

  private void OnCancel(object _)
  {
    if (this.chore != null)
    {
      this.chore.Cancel("Cancel uproot");
      this.chore = (Chore) null;
      this.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem);
    }
    this.isMarkedForCarve = false;
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  private void OnClickCarve() => this.MarkForCarve();

  protected void OnClickCancelCarve() => this.OnCancel((object) null);

  private void OnRefreshUserMenu(object data)
  {
    if (!this.showUserMenuButtons)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore != null ? new KIconButtonMenu.ButtonInfo("action_carve", this.cancelButtonLabel, new System.Action(this.OnClickCancelCarve), tooltipText: this.cancelButtonTooltip) : new KIconButtonMenu.ButtonInfo("action_carve", this.buttonLabel, new System.Action(this.OnClickCarve), tooltipText: this.buttonTooltip));
  }

  protected override void OnCleanUp()
  {
    OccupyArea component = this.gameObject.GetComponent<OccupyArea>();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    foreach (CellOffset occupiedCellsOffset in component.OccupiedCellsOffsets)
    {
      if ((UnityEngine.Object) Grid.ObjectLayers[5][Grid.OffsetCell(cell, occupiedCellsOffset)] == (UnityEngine.Object) this.gameObject)
        Grid.ObjectLayers[5][Grid.OffsetCell(cell, occupiedCellsOffset)] = (GameObject) null;
    }
    base.OnCleanUp();
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem);
  }

  private GameObject ProducePickupable(string pickupablePrefabId)
  {
    if (pickupablePrefabId == null)
      return (GameObject) null;
    Vector3 position = this.gameObject.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f);
    GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(pickupablePrefabId)), position, Grid.SceneLayer.Ore);
    PrimaryElement component = this.gameObject.GetComponent<PrimaryElement>();
    go.GetComponent<PrimaryElement>().Temperature = component.Temperature;
    go.SetActive(true);
    string properName = go.GetProperName();
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, properName, go.transform);
    return go;
  }

  public void Dig() => this.Carve();

  public void MarkForDig(bool instantOnDebug = true) => this.MarkForCarve(instantOnDebug);
}
