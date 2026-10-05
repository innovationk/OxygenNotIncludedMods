// Decompiled with JetBrains decompiler
// Type: ReceptacleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ReceptacleSideScreen : SideScreenContent, IRender1000ms
{
  protected bool ALLOW_ORDER_IGNORING_WOLRD_NEED = true;
  [SerializeField]
  protected KButton requestSelectedEntityBtn;
  [SerializeField]
  private string requestStringDeposit;
  [SerializeField]
  private string requestStringCancelDeposit;
  [SerializeField]
  private string requestStringRemove;
  [SerializeField]
  private string requestStringCancelRemove;
  public GameObject activeEntityContainer;
  public GameObject nothingDiscoveredContainer;
  [SerializeField]
  private bool categoryStartExpanded;
  [SerializeField]
  private GameObject categoryContainerPrefab;
  private Dictionary<Tag, GameObject> contentContainers = new Dictionary<Tag, GameObject>();
  [SerializeField]
  protected LocText descriptionLabel;
  protected Dictionary<SingleEntityReceptacle, int> entityPreviousSelectionMap = new Dictionary<SingleEntityReceptacle, int>();
  [SerializeField]
  private string subtitleStringSelect;
  [SerializeField]
  private string subtitleStringSelectDescription;
  [SerializeField]
  private string subtitleStringAwaitingSelection;
  [SerializeField]
  private string subtitleStringAwaitingDelivery;
  [SerializeField]
  private string subtitleStringEntityDeposited;
  [SerializeField]
  private string subtitleStringAwaitingRemoval;
  [SerializeField]
  private LocText subtitleLabel;
  [SerializeField]
  private List<DescriptorPanel> descriptorPanels;
  public Material defaultMaterial;
  public Material desaturatedMaterial;
  [SerializeField]
  private GameObject requestObjectListContainer;
  [SerializeField]
  private GameObject requestObjectListContainerContent;
  [SerializeField]
  private GameObject scrollBarContainer;
  [SerializeField]
  private GameObject entityToggle;
  [SerializeField]
  private Sprite buttonSelectedBG;
  [SerializeField]
  private Sprite buttonNormalBG;
  [SerializeField]
  private Sprite elementPlaceholderSpr;
  [SerializeField]
  private bool hideUndiscoveredEntities;
  protected ReceptacleToggle selectedEntityToggle;
  protected SingleEntityReceptacle targetReceptacle;
  protected Tag selectedDepositObjectTag;
  protected Tag selectedDepositObjectAdditionalTag;
  protected Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObjectMap;
  protected List<ReceptacleToggle> entityToggles = new List<ReceptacleToggle>();
  private List<GameObject> recycledEntityToggles = new List<GameObject>();
  private Dictionary<Tag, bool> categoryExpandedStatus = new Dictionary<Tag, bool>();
  private int onObjectDestroyedHandle = -1;
  private int onOccupantValidChangedHandle = -1;
  private int onStorageChangedHandle = -1;

  public override string GetTitle()
  {
    return (UnityEngine.Object) this.targetReceptacle == (UnityEngine.Object) null ? Strings.Get(this.titleKey).ToString().Replace("{0}", "") : string.Format((string) Strings.Get(this.titleKey), (object) this.targetReceptacle.GetProperName());
  }

  private void RecycleToggle(GameObject toggle)
  {
    toggle.SetActive(false);
    this.recycledEntityToggles.Add(toggle);
  }

  private GameObject SpawnToggle(GameObject parent)
  {
    if (this.recycledEntityToggles.Count <= 0)
      return Util.KInstantiateUI(this.entityToggle, parent, true);
    GameObject recycledEntityToggle = this.recycledEntityToggles[this.recycledEntityToggles.Count - 1];
    this.recycledEntityToggles.RemoveAt(this.recycledEntityToggles.Count - 1);
    recycledEntityToggle.transform.SetParent(parent.transform);
    recycledEntityToggle.SetActive(true);
    return recycledEntityToggle;
  }

  private void RefreshCategoryOpen(GameObject categoryHeader, GameObject categoryGrid, Tag tag)
  {
    categoryHeader.GetComponent<MultiToggle>().ChangeState(this.categoryExpandedStatus[tag] ? 0 : 1);
    categoryGrid.gameObject.SetActive(this.categoryExpandedStatus[tag]);
  }

  public void Initialize(SingleEntityReceptacle target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "SingleObjectReceptacle provided was null.");
    }
    else
    {
      this.targetReceptacle = target;
      this.gameObject.SetActive(true);
      this.depositObjectMap = new Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity>();
      this.entityToggles.ForEach((Action<ReceptacleToggle>) (rbi => this.RecycleToggle(rbi.gameObject)));
      this.entityToggles.Clear();
      List<GameObject> gameObjectList = new List<GameObject>();
      if (this.targetReceptacle.possibleDepositObjectTags.Count == 1)
        this.categoryStartExpanded = true;
      foreach (Tag depositObjectTag in (IEnumerable<Tag>) this.targetReceptacle.possibleDepositObjectTags)
      {
        Tag tag = depositObjectTag;
        List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(tag);
        int count = prefabsWithTag.Count;
        if (this.categoryExpandedStatus.ContainsKey(tag))
          this.categoryExpandedStatus[tag] = this.categoryStartExpanded;
        if (!this.contentContainers.ContainsKey(tag))
        {
          GameObject gameObject = Util.KInstantiateUI(this.categoryContainerPrefab, this.requestObjectListContainerContent, true);
          this.contentContainers.Add(tag, gameObject);
          HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
          component.GetReference<LocText>("HeaderLabel").SetText(tag.ProperName());
          this.categoryExpandedStatus.Add(tag, this.categoryStartExpanded);
          MultiToggle toggle = gameObject.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("HeaderToggle");
          GridLayoutGroup grid = component.GetReference<GridLayoutGroup>("GridLayout");
          toggle.onClick += (System.Action) (() =>
          {
            this.categoryExpandedStatus[tag] = !this.categoryExpandedStatus[tag];
            this.RefreshCategoryOpen(toggle.gameObject, grid.gameObject, tag);
          });
          this.RefreshCategoryOpen(toggle.gameObject, grid.gameObject, tag);
        }
        this.RefreshCategoryOpen(this.contentContainers[tag].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("HeaderToggle").gameObject, this.contentContainers[tag].GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("GridLayout").gameObject, tag);
        List<IHasSortOrder> hasSortOrderList = new List<IHasSortOrder>();
        foreach (GameObject candidate in prefabsWithTag)
        {
          if (!this.targetReceptacle.IsValidEntity(candidate) || gameObjectList.Contains(candidate))
          {
            --count;
          }
          else
          {
            IHasSortOrder component = candidate.GetComponent<IHasSortOrder>();
            if (component != null)
            {
              gameObjectList.Add(candidate);
              hasSortOrderList.Add(component);
            }
          }
        }
        Debug.Assert(hasSortOrderList.Count == count, (object) "Not all entities in this receptacle implement IHasSortOrder!");
        hasSortOrderList.Sort((Comparison<IHasSortOrder>) ((a, b) => a.sortOrder - b.sortOrder));
        foreach (IHasSortOrder hasSortOrder in hasSortOrderList)
        {
          GameObject gameObject1 = (hasSortOrder as MonoBehaviour).gameObject;
          GameObject gameObject2 = this.SpawnToggle(this.contentContainers[tag].GetComponent<HierarchyReferences>().GetReference("GridLayout").gameObject);
          gameObject2.transform.SetAsLastSibling();
          gameObject2.SetActive(true);
          ReceptacleToggle newToggle = gameObject2.GetComponent<ReceptacleToggle>();
          IReceptacleDirection component1 = gameObject1.GetComponent<IReceptacleDirection>();
          string entityName = this.GetEntityName(gameObject1.PrefabID());
          newToggle.title.text = entityName;
          Sprite sprite = this.GetEntityIcon(gameObject1.PrefabID());
          if ((UnityEngine.Object) sprite == (UnityEngine.Object) null)
            sprite = this.elementPlaceholderSpr;
          newToggle.image.sprite = sprite;
          if ((UnityEngine.Object) newToggle.toggle == (UnityEngine.Object) null)
            newToggle.toggle = newToggle.GetComponentInChildren<MultiToggle>();
          newToggle.toggle.onClick += (System.Action) (() => this.ToggleClicked(newToggle));
          ToolTip component2 = newToggle.GetComponent<ToolTip>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            component2.SetSimpleTooltip(this.GetEntityTooltip(gameObject1.PrefabID()));
          this.depositObjectMap.Add(newToggle, new ReceptacleSideScreen.SelectableEntity()
          {
            tag = gameObject1.PrefabID(),
            direction = component1 != null ? component1.Direction : SingleEntityReceptacle.ReceptacleDirection.Top,
            asset = gameObject1
          });
          this.entityToggles.Add(newToggle);
        }
      }
      this.RestoreSelectionFromOccupant();
      this.selectedEntityToggle = (ReceptacleToggle) null;
      if (this.entityToggles.Count > 0)
      {
        if (this.entityPreviousSelectionMap.ContainsKey(this.targetReceptacle))
        {
          this.ToggleClicked(this.entityToggles[this.entityPreviousSelectionMap[this.targetReceptacle]]);
        }
        else
        {
          this.subtitleLabel.SetText(Strings.Get(this.subtitleStringSelect).ToString());
          this.requestSelectedEntityBtn.isInteractable = false;
          this.descriptionLabel.SetText(Strings.Get(this.subtitleStringSelectDescription).ToString());
          this.HideAllDescriptorPanels();
        }
      }
      this.onStorageChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1697596308, new Action<object>(this.CheckAmountsAndUpdate));
      this.onOccupantValidChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1820564715, new Action<object>(this.OnOccupantValidChanged));
      this.UpdateState((object) null);
      SimAndRenderScheduler.instance.Add((object) this);
    }
  }

  protected virtual void UpdateState(object data)
  {
    this.requestSelectedEntityBtn.ClearOnClick();
    if ((UnityEngine.Object) this.targetReceptacle == (UnityEngine.Object) null)
      return;
    if (this.CheckReceptacleOccupied())
    {
      Uprootable uprootable = this.targetReceptacle.Occupant.GetComponent<Uprootable>();
      if ((UnityEngine.Object) uprootable != (UnityEngine.Object) null && uprootable.IsMarkedForUproot)
      {
        this.requestSelectedEntityBtn.onClick += (System.Action) (() =>
        {
          uprootable.ForceCancelUproot();
          this.UpdateState((object) null);
        });
        this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelRemove).ToString();
        this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingRemoval).ToString(), (object) this.targetReceptacle.Occupant.GetProperName()));
      }
      else
      {
        this.requestSelectedEntityBtn.onClick += (System.Action) (() =>
        {
          this.targetReceptacle.OrderRemoveOccupant();
          this.UpdateState((object) null);
        });
        this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringRemove).ToString();
        this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringEntityDeposited).ToString(), (object) this.targetReceptacle.Occupant.GetProperName()));
      }
      this.requestSelectedEntityBtn.isInteractable = true;
      this.ToggleObjectPicker(false);
      this.ConfigureActiveEntity(this.targetReceptacle.Occupant.GetComponent<KSelectable>().PrefabID());
      this.SetResultDescriptions(this.targetReceptacle.Occupant);
    }
    else if (this.targetReceptacle.GetActiveRequest != null)
    {
      this.requestSelectedEntityBtn.onClick += (System.Action) (() =>
      {
        this.targetReceptacle.CancelActiveRequest();
        this.ClearSelection();
        this.UpdateAvailableAmounts((object) null);
        this.UpdateState((object) null);
      });
      this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelDeposit).ToString();
      this.requestSelectedEntityBtn.isInteractable = true;
      this.ToggleObjectPicker(false);
      this.ConfigureActiveEntity(this.targetReceptacle.GetActiveRequest.tagsFirst);
      GameObject prefab = Assets.GetPrefab(this.targetReceptacle.GetActiveRequest.tagsFirst);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingDelivery).ToString(), (object) prefab.GetProperName()));
        this.SetResultDescriptions(prefab);
      }
    }
    else if ((UnityEngine.Object) this.selectedEntityToggle != (UnityEngine.Object) null)
    {
      this.requestSelectedEntityBtn.onClick += (System.Action) (() =>
      {
        this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
        this.UpdateAvailableAmounts((object) null);
        this.UpdateState((object) null);
      });
      this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
      this.targetReceptacle.SetPreview(this.depositObjectMap[this.selectedEntityToggle].tag);
      this.requestSelectedEntityBtn.isInteractable = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle], true);
      this.ToggleObjectPicker(true);
      GameObject prefab = Assets.GetPrefab(this.selectedDepositObjectTag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingSelection).ToString(), (object) prefab.GetProperName()));
        this.SetResultDescriptions(prefab);
      }
    }
    else
    {
      this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
      this.requestSelectedEntityBtn.isInteractable = false;
      this.ToggleObjectPicker(true);
    }
    this.UpdateAvailableAmounts((object) null);
    this.RefreshToggleStates();
    this.UpdateListeners();
  }

  private void UpdateListeners()
  {
    if (this.CheckReceptacleOccupied())
    {
      if (this.onObjectDestroyedHandle != -1)
        return;
      this.onObjectDestroyedHandle = this.targetReceptacle.Occupant.gameObject.Subscribe(1969584890, (Action<object>) (d => this.UpdateState((object) null)));
    }
    else
    {
      if (this.onObjectDestroyedHandle == -1)
        return;
      this.onObjectDestroyedHandle = -1;
    }
  }

  private void OnOccupantValidChanged(object _)
  {
    if ((UnityEngine.Object) this.targetReceptacle == (UnityEngine.Object) null || this.CheckReceptacleOccupied() || this.targetReceptacle.GetActiveRequest == null)
      return;
    bool flag = false;
    ReceptacleSideScreen.SelectableEntity entity;
    if (this.depositObjectMap.TryGetValue(this.selectedEntityToggle, out entity))
      flag = this.CanDepositEntity(entity, true);
    if (flag)
      return;
    this.targetReceptacle.CancelActiveRequest();
    this.ClearSelection();
    this.UpdateState((object) null);
    this.UpdateAvailableAmounts((object) null);
  }

  protected bool CanDepositEntity(
    ReceptacleSideScreen.SelectableEntity entity,
    bool runAdditionalCanDepositTest = false)
  {
    if (!this.ValidRotationForDeposit(entity.direction) || this.RequiresAvailableAmountToDeposit() && (double) this.GetAvailableAmount(entity.tag) <= 0.0)
      return false;
    return !runAdditionalCanDepositTest || this.AdditionalCanDepositTest();
  }

  protected virtual bool AdditionalCanDepositTest() => true;

  protected virtual bool RequiresAvailableAmountToDeposit() => true;

  private void ClearSelection()
  {
    this.selectedEntityToggle = (ReceptacleToggle) null;
    this.RefreshToggleStates();
  }

  private void ToggleObjectPicker(bool Show)
  {
    this.requestObjectListContainer.SetActive(Show);
    if ((UnityEngine.Object) this.scrollBarContainer != (UnityEngine.Object) null)
      this.scrollBarContainer.SetActive(Show);
    this.requestObjectListContainer.SetActive(Show);
    this.activeEntityContainer.SetActive(!Show);
  }

  private void ConfigureActiveEntity(Tag tag)
  {
    string properName = Assets.GetPrefab(tag).GetProperName();
    HierarchyReferences component = this.activeEntityContainer.GetComponent<HierarchyReferences>();
    component.GetReference<LocText>("Label").text = properName;
    component.GetReference<Image>("Icon").sprite = this.GetEntityIcon(tag);
  }

  protected virtual string GetEntityName(Tag prefabTag)
  {
    return Assets.GetPrefab(prefabTag).GetProperName();
  }

  protected virtual string GetEntityTooltip(Tag prefabTag)
  {
    InfoDescription component = Assets.GetPrefab(prefabTag).GetComponent<InfoDescription>();
    string entityTooltip = this.GetEntityName(prefabTag);
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      entityTooltip = $"{entityTooltip}\n\n{component.description}";
    return entityTooltip;
  }

  protected virtual Sprite GetEntityIcon(Tag prefabTag)
  {
    return Def.GetUISprite((object) Assets.GetPrefab(prefabTag)).first;
  }

  public override bool IsValidForTarget(GameObject target)
  {
    SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.enabled && (UnityEngine.Object) target.GetComponent<PlantablePlot>() == (UnityEngine.Object) null && (UnityEngine.Object) target.GetComponent<EggIncubator>() == (UnityEngine.Object) null && (UnityEngine.Object) target.GetComponent<SpecialCargoBayClusterReceptacle>() == (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The object selected doesn't have a SingleObjectReceptacle!");
    }
    else
    {
      this.Initialize(component);
      this.UpdateState((object) null);
    }
  }

  protected virtual void RestoreSelectionFromOccupant()
  {
  }

  public override void ClearTarget()
  {
    if (!((UnityEngine.Object) this.targetReceptacle != (UnityEngine.Object) null))
      return;
    if (this.CheckReceptacleOccupied())
    {
      this.targetReceptacle.Occupant.gameObject.Unsubscribe(this.onObjectDestroyedHandle);
      this.onObjectDestroyedHandle = -1;
    }
    this.targetReceptacle.Unsubscribe(this.onStorageChangedHandle);
    this.onStorageChangedHandle = -1;
    this.targetReceptacle.Unsubscribe(this.onOccupantValidChangedHandle);
    this.onOccupantValidChangedHandle = -1;
    if (this.targetReceptacle.GetActiveRequest == null)
      this.targetReceptacle.SetPreview(Tag.Invalid);
    SimAndRenderScheduler.instance.Remove((object) this);
    this.targetReceptacle = (SingleEntityReceptacle) null;
  }

  protected void RefreshToggleStates()
  {
    foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObject in this.depositObjectMap)
    {
      if ((UnityEngine.Object) this.selectedEntityToggle != (UnityEngine.Object) depositObject.Key)
      {
        if (this.CanDepositEntity(depositObject.Value))
          this.SetToggleState(depositObject.Key.toggle, ImageToggleState.State.Inactive);
        else
          this.SetToggleState(depositObject.Key.toggle, ImageToggleState.State.Disabled);
      }
      else if (this.CanDepositEntity(depositObject.Value))
        this.SetToggleState(depositObject.Key.toggle, ImageToggleState.State.Active);
      else
        this.SetToggleState(depositObject.Key.toggle, ImageToggleState.State.DisabledActive);
    }
  }

  protected void SetToggleState(MultiToggle toggle, ImageToggleState.State state)
  {
    switch (state)
    {
      case ImageToggleState.State.Disabled:
        toggle.ChangeState(2);
        toggle.gameObject.GetComponentsInChildrenOnly<Image>()[1].material = this.desaturatedMaterial;
        break;
      case ImageToggleState.State.Inactive:
        toggle.ChangeState(0);
        toggle.gameObject.GetComponentsInChildrenOnly<Image>()[1].material = this.defaultMaterial;
        break;
      case ImageToggleState.State.Active:
        toggle.ChangeState(1);
        toggle.gameObject.GetComponentsInChildrenOnly<Image>()[1].material = this.defaultMaterial;
        break;
      case ImageToggleState.State.DisabledActive:
        toggle.ChangeState(3);
        toggle.gameObject.GetComponentsInChildrenOnly<Image>()[1].material = this.desaturatedMaterial;
        break;
    }
  }

  public void Render1000ms(float dt) => this.CheckAmountsAndUpdate((object) null);

  private void CheckAmountsAndUpdate(object data)
  {
    if ((UnityEngine.Object) this.targetReceptacle == (UnityEngine.Object) null || !this.UpdateAvailableAmounts((object) null))
      return;
    this.UpdateState((object) null);
  }

  private bool UpdateAvailableAmounts(object data)
  {
    bool flag1 = false;
    foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObject in this.depositObjectMap)
    {
      if (!DebugHandler.InstantBuildMode && this.hideUndiscoveredEntities && !DiscoveredResources.Instance.IsDiscovered(depositObject.Value.tag))
        depositObject.Key.gameObject.SetActive(false);
      else if (!depositObject.Key.gameObject.activeSelf)
        depositObject.Key.gameObject.SetActive(true);
      float availableAmount = this.GetAvailableAmount(depositObject.Value.tag);
      if ((double) depositObject.Value.lastAmount != (double) availableAmount)
      {
        flag1 = true;
        depositObject.Value.lastAmount = availableAmount;
        depositObject.Key.amount.text = availableAmount.ToString();
      }
      if (!this.ValidRotationForDeposit(depositObject.Value.direction) || (double) availableAmount <= 0.0)
      {
        if ((UnityEngine.Object) this.selectedEntityToggle != (UnityEngine.Object) depositObject.Key)
          depositObject.Key.toggle.ChangeState(2);
        else
          depositObject.Key.toggle.ChangeState(3);
      }
      else if ((UnityEngine.Object) this.selectedEntityToggle != (UnityEngine.Object) depositObject.Key)
        depositObject.Key.toggle.ChangeState(0);
      else
        depositObject.Key.toggle.ChangeState(1);
    }
    foreach (KeyValuePair<Tag, GameObject> contentContainer in this.contentContainers)
    {
      Transform transform = contentContainer.Value.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("GridLayout").transform;
      bool flag2 = false;
      for (int index = 0; index < transform.childCount; ++index)
      {
        if (transform.GetChild(index).gameObject.activeSelf)
        {
          flag2 = true;
          break;
        }
      }
      if (contentContainer.Value.activeSelf != flag2)
        contentContainer.Value.SetActive(flag2);
    }
    return flag1;
  }

  protected float GetAvailableAmount(Tag tag)
  {
    if (!this.ALLOW_ORDER_IGNORING_WOLRD_NEED)
      return this.targetReceptacle.GetMyWorld().worldInventory.GetAmount(tag, true);
    ICollection<Pickupable> pickupables = this.targetReceptacle.GetMyWorld().worldInventory.GetPickupables(tag, true);
    float availableAmount = 0.0f;
    foreach (Pickupable pickupable in (IEnumerable<Pickupable>) pickupables)
      availableAmount += (float) Mathf.CeilToInt(pickupable.TotalAmount);
    return availableAmount;
  }

  private bool ValidRotationForDeposit(
    SingleEntityReceptacle.ReceptacleDirection depositDir)
  {
    return (UnityEngine.Object) this.targetReceptacle.rotatable == (UnityEngine.Object) null || depositDir == this.targetReceptacle.Direction;
  }

  protected virtual void ToggleClicked(ReceptacleToggle toggle)
  {
    if (!this.depositObjectMap.ContainsKey(toggle))
    {
      Debug.LogError((object) "Recipe not found on recipe list.");
    }
    else
    {
      this.selectedEntityToggle = toggle;
      this.entityPreviousSelectionMap[this.targetReceptacle] = this.entityToggles.IndexOf(toggle);
      this.selectedDepositObjectTag = this.depositObjectMap[toggle].tag;
      MutantPlant component = this.depositObjectMap[toggle].asset.GetComponent<MutantPlant>();
      this.selectedDepositObjectAdditionalTag = (bool) (UnityEngine.Object) component ? component.SubSpeciesID : Tag.Invalid;
      this.RefreshToggleStates();
      this.UpdateAvailableAmounts((object) null);
      this.UpdateState((object) null);
    }
  }

  private void CreateOrder(bool isInfinite)
  {
    this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
  }

  protected bool CheckReceptacleOccupied()
  {
    return (UnityEngine.Object) this.targetReceptacle != (UnityEngine.Object) null && (UnityEngine.Object) this.targetReceptacle.Occupant != (UnityEngine.Object) null;
  }

  protected virtual void SetResultDescriptions(GameObject go)
  {
    string sourceText = "";
    InfoDescription component1 = go.GetComponent<InfoDescription>();
    if ((bool) (UnityEngine.Object) component1)
    {
      sourceText = component1.description;
    }
    else
    {
      KPrefabID component2 = go.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        Element element = ElementLoader.GetElement(component2.PrefabID());
        if (element != null)
          sourceText = element.Description();
      }
      else
        sourceText = go.GetProperName();
    }
    this.descriptionLabel.SetText(sourceText);
  }

  protected virtual void HideAllDescriptorPanels()
  {
    for (int index = 0; index < this.descriptorPanels.Count; ++index)
      this.descriptorPanels[index].gameObject.SetActive(false);
  }

  protected class SelectableEntity
  {
    public Tag tag;
    public SingleEntityReceptacle.ReceptacleDirection direction;
    public GameObject asset;
    public float lastAmount = -1f;
  }
}
