// Decompiled with JetBrains decompiler
// Type: ResearchScreenSideBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ResearchScreenSideBar : KScreen
{
  [Header("Containers")]
  [SerializeField]
  private GameObject queueContainer;
  [SerializeField]
  private GameObject projectsContainer;
  [SerializeField]
  private GameObject searchFiltersContainer;
  [Header("Prefabs")]
  [SerializeField]
  private GameObject headerTechTypePrefab;
  [SerializeField]
  private GameObject filterButtonPrefab;
  [SerializeField]
  private GameObject techWidgetRootPrefab;
  [SerializeField]
  private GameObject techWidgetRootAltPrefab;
  [SerializeField]
  private GameObject techItemPrefab;
  [SerializeField]
  private GameObject techWidgetUnlockedItemPrefab;
  [SerializeField]
  private GameObject techWidgetRowPrefab;
  [SerializeField]
  private GameObject techCategoryPrefab;
  [SerializeField]
  private GameObject techCategoryPrefabAlt;
  [Header("Other references")]
  [SerializeField]
  private KInputTextField searchBox;
  [SerializeField]
  private MultiToggle allFilter;
  [SerializeField]
  private MultiToggle availableFilter;
  [SerializeField]
  private MultiToggle completedFilter;
  [SerializeField]
  private ResearchScreen researchScreen;
  [SerializeField]
  private KButton clearSearchButton;
  [SerializeField]
  private Color evenRowColor;
  [SerializeField]
  private Color oddRowColor;
  private GraphicRaycaster raycaster;
  private ResearchScreenSideBar.CompletionState completionFilter;
  private Dictionary<string, bool> filterStates = new Dictionary<string, bool>();
  private Dictionary<string, bool> categoryExpanded = new Dictionary<string, bool>();
  private string currentSearchString = "";
  private string currentSearchStringUpper = "";
  private const string SEARCH_RESULTS_CATEGORY_ID = "SearchResults";
  private Dictionary<string, SearchUtil.TechCache> techCaches;
  private readonly Dictionary<string, SearchUtil.TechItemCache> techItemCaches = new Dictionary<string, SearchUtil.TechItemCache>();
  private readonly List<GameObject> orderedTechs = new List<GameObject>();
  private Dictionary<string, GameObject> queueTechs = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> projectTechs = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> projectCategories = new Dictionary<string, GameObject>();
  private Dictionary<string, GameObject> filterButtons = new Dictionary<string, GameObject>();
  private Dictionary<string, Dictionary<string, GameObject>> projectTechItems = new Dictionary<string, Dictionary<string, GameObject>>();
  private Dictionary<string, List<Tag>> filterPresets = new Dictionary<string, List<Tag>>()
  {
    {
      "Oxygen",
      new List<Tag>()
    },
    {
      "Food",
      new List<Tag>()
    },
    {
      "Water",
      new List<Tag>()
    },
    {
      "Power",
      new List<Tag>()
    },
    {
      "Morale",
      new List<Tag>()
    },
    {
      "Ranching",
      new List<Tag>()
    },
    {
      "Filter",
      new List<Tag>()
    },
    {
      "Tile",
      new List<Tag>()
    },
    {
      "Transport",
      new List<Tag>()
    },
    {
      "Automation",
      new List<Tag>()
    },
    {
      "Medicine",
      new List<Tag>()
    },
    {
      "Rocket",
      new List<Tag>()
    }
  };
  private List<GameObject> QueuedActivations = new List<GameObject>();
  private List<GameObject> QueuedDeactivations = new List<GameObject>();
  public ButtonSoundPlayer soundPlayer;
  [SerializeField]
  private int activationPerFrame = 5;
  private Comparer<Tuple<GameObject, string>> techWidgetComparer;
  private bool evenRow;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.raycaster = this.projectsContainer.GetComponent<GraphicRaycaster>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.PopulateProjects();
    this.PopulateFilterButtons();
    this.RefreshCategoriesContentExpanded();
    this.RefreshWidgets();
    this.searchBox.OnValueChangesPaused = (System.Action) (() => this.SetTextFilter(this.searchBox.text, false));
    KInputTextField searchBox = this.searchBox;
    searchBox.onFocus = searchBox.onFocus + (System.Action) (() =>
    {
      this.isEditing = true;
      UISounds.PlaySound(UISounds.Sound.Find);
    });
    this.searchBox.onEndEdit.AddListener((UnityAction<string>) (value => this.isEditing = false));
    this.clearSearchButton.onClick += (System.Action) (() => this.ResetFilter());
    this.ConfigCompletionFilters();
    this.ConsumeMouseScroll = true;
    Game.Instance.Subscribe(-107300940, new Action<object>(this.UpdateProjectFilter));
  }

  private void Update()
  {
    for (int index = 0; index < Math.Min(this.QueuedActivations.Count, this.activationPerFrame); ++index)
      this.QueuedActivations[index].SetActive(true);
    this.QueuedActivations.RemoveRange(0, Math.Min(this.QueuedActivations.Count, this.activationPerFrame));
    for (int index = 0; index < Math.Min(this.QueuedDeactivations.Count, this.activationPerFrame); ++index)
      this.QueuedDeactivations[index].SetActive(false);
    this.QueuedDeactivations.RemoveRange(0, Math.Min(this.QueuedDeactivations.Count, this.activationPerFrame));
  }

  public override bool IsScreenActive() => this.researchScreen.IsScreenActive();

  private void ConfigCompletionFilters()
  {
    this.allFilter.onClick += (System.Action) (() => this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All, false));
    this.completedFilter.onClick += (System.Action) (() => this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.Completed, false));
    this.availableFilter.onClick += (System.Action) (() => this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.Available, false));
    this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All, false);
  }

  private void SetCompletionFilter(ResearchScreenSideBar.CompletionState state, bool suppressUpdate)
  {
    this.completionFilter = state;
    this.allFilter.GetComponent<MultiToggle>().ChangeState(this.completionFilter == ResearchScreenSideBar.CompletionState.All ? 1 : 0);
    this.completedFilter.GetComponent<MultiToggle>().ChangeState(this.completionFilter == ResearchScreenSideBar.CompletionState.Completed ? 1 : 0);
    this.availableFilter.GetComponent<MultiToggle>().ChangeState(this.completionFilter == ResearchScreenSideBar.CompletionState.Available ? 1 : 0);
    if (suppressUpdate)
      return;
    this.UpdateProjectFilter();
  }

  public override float GetSortKey() => this.isEditing ? 50f : 21f;

  public override void OnKeyDown(KButtonEvent e)
  {
    if ((UnityEngine.Object) this.researchScreen != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this.researchScreen.canvas && !this.researchScreen.canvas.enabled)
      return;
    if (this.isEditing)
    {
      e.Consumed = true;
    }
    else
    {
      if (e.Consumed)
        return;
      Vector2 vector2 = (Vector2) this.transform.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
      if ((double) vector2.x < 0.0 || (double) vector2.x > (double) this.transform.rectTransform().rect.width || e.TryConsume(Action.MouseRight) || e.TryConsume(Action.MouseLeft) || KInputManager.currentControllerIsGamepad || e.TryConsume(Action.ZoomIn))
        return;
      e.TryConsume(Action.ZoomOut);
    }
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.raycaster.enabled = show;
    this.RefreshWidgets();
  }

  public override void Show(bool show = true)
  {
    this.mouseOver = false;
    this.OnShow(show);
  }

  private void SetTextFilter(string newValue, bool suppressUpdate)
  {
    if (this.isEditing)
    {
      foreach (KeyValuePair<string, GameObject> filterButton in this.filterButtons)
      {
        this.filterStates[filterButton.Key] = false;
        filterButton.Value.GetComponent<MultiToggle>().ChangeState(0);
      }
    }
    bool flag = this.IsTextFilterActive();
    this.currentSearchString = newValue;
    this.currentSearchStringUpper = this.currentSearchString.ToUpper().Trim();
    if (this.IsTextFilterActive())
    {
      Transform reference = this.projectCategories["SearchResults"].GetComponent<HierarchyReferences>().GetReference<Transform>("Content");
      foreach (KeyValuePair<string, GameObject> projectTech in this.projectTechs)
        this.projectTechs[projectTech.Key].transform.SetParent(reference);
    }
    else if (flag)
    {
      foreach (KeyValuePair<string, GameObject> projectTech in this.projectTechs)
      {
        Transform reference = this.projectCategories[Db.Get().Techs.Get(projectTech.Key).category].GetComponent<HierarchyReferences>().GetReference<Transform>("Content");
        this.projectTechs[projectTech.Key].transform.SetParent(reference);
      }
    }
    if (suppressUpdate)
      return;
    this.UpdateProjectFilter();
  }

  private void UpdateProjectFilter(object data = null)
  {
    Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
    foreach (KeyValuePair<string, GameObject> projectCategory in this.projectCategories)
      dictionary.Add(projectCategory.Key, false);
    bool flag = this.IsTextFilterActive();
    if (flag)
    {
      dictionary["SearchResults"] = true;
      this.categoryExpanded["SearchResults"] = true;
    }
    this.RefreshProjectsActive();
    foreach (KeyValuePair<string, GameObject> projectTech in this.projectTechs)
    {
      if ((projectTech.Value.activeSelf || this.QueuedActivations.Contains(projectTech.Value)) && !this.QueuedDeactivations.Contains(projectTech.Value))
      {
        dictionary[Db.Get().Techs.Get(projectTech.Key).category] = !flag;
        this.categoryExpanded[Db.Get().Techs.Get(projectTech.Key).category] = true;
      }
    }
    foreach (KeyValuePair<string, bool> keyValuePair in dictionary)
      this.ChangeGameObjectActive(this.projectCategories[keyValuePair.Key], keyValuePair.Value);
    this.RefreshCategoriesContentExpanded();
    foreach (GameObject orderedTech in this.orderedTechs)
      orderedTech.transform.SetAsLastSibling();
  }

  private int CompareTechScores(Tuple<GameObject, string> a, Tuple<GameObject, string> b)
  {
    int num = -this.GetTechMatchScore(a.second).CompareTo(this.GetTechMatchScore(b.second));
    return num != 0 || !this.IsTextFilterActive() ? num : this.techCaches[a.second].CompareTo((object) this.techCaches[b.second]);
  }

  private Comparer<Tuple<GameObject, string>> TechWidgetComparer
  {
    get
    {
      if (this.techWidgetComparer == null)
        this.techWidgetComparer = Comparer<Tuple<GameObject, string>>.Create(new Comparison<Tuple<GameObject, string>>(this.CompareTechScores));
      return this.techWidgetComparer;
    }
  }

  private void RefreshProjectsActive()
  {
    if (this.projectTechItems.Count == 0)
      return;
    Techs techs = Db.Get().Techs;
    if (this.techCaches == null)
      this.techCaches = SearchUtil.CacheTechs();
    if (this.IsTextFilterActive())
    {
      foreach (KeyValuePair<string, SearchUtil.TechCache> techCach in this.techCaches)
      {
        try
        {
          techCach.Value.Bind(this.currentSearchStringUpper);
        }
        catch (Exception ex)
        {
          KCrashReporter.ReportDevNotification("Fuzzy score bind failed", Environment.StackTrace, ex.Message);
          techCach.Value.Reset();
        }
      }
    }
    else
    {
      foreach (KeyValuePair<string, SearchUtil.TechCache> techCach in this.techCaches)
        techCach.Value.Reset();
    }
    for (int idx = 0; idx != techs.Count; ++idx)
    {
      Tech resource = (Tech) techs.GetResource(idx);
      SearchUtil.TechCache techCach = this.techCaches[resource.Id];
      foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectTechItems[resource.Id])
      {
        bool flag = SearchUtil.IsPassingScore(this.GetTechItemMatchScore(techCach, keyValuePair.Key));
        HierarchyReferences component = keyValuePair.Value.GetComponent<HierarchyReferences>();
        component.GetReference<LocText>("Label").color = flag ? Color.white : Color.grey;
        component.GetReference<Image>("Icon").color = flag ? Color.white : new Color(1f, 1f, 1f, 0.5f);
      }
    }
    ListPool<Tuple<int, int>, ResearchScreen>.PooledList pooledList1 = ListPool<Tuple<int, int>, ResearchScreen>.Allocate();
    for (int index = 0; index != techs.Count; ++index)
    {
      Tech resource = (Tech) techs.GetResource(index);
      pooledList1.Add(new Tuple<int, int>(index, resource.tier));
    }
    pooledList1.Sort((Comparison<Tuple<int, int>>) ((a, b) => a.second.CompareTo(b.second)));
    ListPool<Tuple<GameObject, string>, ResearchScreenSideBar>.PooledList pooledList2 = ListPool<Tuple<GameObject, string>, ResearchScreenSideBar>.Allocate();
    foreach (Tuple<int, int> tuple in (List<Tuple<int, int>>) pooledList1)
    {
      Tech resource = (Tech) techs.GetResource(tuple.first);
      GameObject projectTech = this.projectTechs[resource.Id];
      bool flag = SearchUtil.IsPassingScore(this.GetTechMatchScore(resource.Id));
      this.ChangeGameObjectActive(projectTech, flag);
      this.researchScreen.GetEntry(resource).UpdateFilterState(flag);
      if (flag)
      {
        Tuple<GameObject, string> a = new Tuple<GameObject, string>(projectTech, resource.Id);
        int index = pooledList2.BinarySearch(a, (IComparer<Tuple<GameObject, string>>) this.TechWidgetComparer);
        if (index < 0)
          index = ~index;
        while (index < pooledList2.Count && this.CompareTechScores(a, pooledList2[index]) == 0)
          ++index;
        pooledList2.Insert(index, a);
      }
    }
    pooledList1.Recycle();
    this.orderedTechs.Clear();
    foreach (Tuple<GameObject, string> tuple in (List<Tuple<GameObject, string>>) pooledList2)
      this.orderedTechs.Add(tuple.first);
    pooledList2.Recycle();
  }

  private void RefreshCategoriesContentExpanded()
  {
    foreach (KeyValuePair<string, GameObject> projectCategory in this.projectCategories)
    {
      projectCategory.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject.SetActive(this.categoryExpanded[projectCategory.Key]);
      projectCategory.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.categoryExpanded[projectCategory.Key] ? 1 : 0);
    }
  }

  private void CreateCategory(string categoryID, string title = null)
  {
    GameObject gameObject = Util.KInstantiateUI(this.techCategoryPrefabAlt, this.projectsContainer, true);
    gameObject.name = categoryID;
    if (title == null)
      title = (string) Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + categoryID.ToUpper());
    gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(title);
    this.categoryExpanded.Add(categoryID, false);
    this.projectCategories.Add(categoryID, gameObject);
    gameObject.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = (System.Action) (() =>
    {
      this.categoryExpanded[categoryID] = !this.categoryExpanded[categoryID];
      this.RefreshCategoriesContentExpanded();
    });
  }

  private void PopulateProjects()
  {
    ListPool<Tuple<Tuple<string, GameObject>, int>, ResearchScreen>.PooledList pooledList = ListPool<Tuple<Tuple<string, GameObject>, int>, ResearchScreen>.Allocate();
    for (int idx = 0; idx < Db.Get().Techs.Count; ++idx)
    {
      Tech tech = (Tech) Db.Get().Techs.GetResource(idx);
      if (!this.projectCategories.ContainsKey(tech.category))
        this.CreateCategory(tech.category);
      GameObject b = this.SpawnTechWidget(tech.Id, this.projectCategories[tech.category]);
      pooledList.Add(new Tuple<Tuple<string, GameObject>, int>(new Tuple<string, GameObject>(tech.Id, b), tech.tier));
      this.projectTechs.Add(tech.Id, b);
      b.GetComponent<ToolTip>().SetSimpleTooltip(tech.desc);
      b.GetComponent<MultiToggle>().onEnter += (System.Action) (() =>
      {
        this.researchScreen.TurnEverythingOff();
        this.researchScreen.GetEntry(tech).OnHover(true, tech);
        this.soundPlayer.Play(1);
      });
      b.GetComponent<MultiToggle>().onExit += (System.Action) (() => this.researchScreen.TurnEverythingOff());
    }
    this.CreateCategory("SearchResults", (string) STRINGS.UI.RESEARCHSCREEN.SEARCH_RESULTS_CATEGORY);
    foreach (KeyValuePair<string, GameObject> projectTech in this.projectTechs)
    {
      Transform reference = this.projectCategories[Db.Get().Techs.Get(projectTech.Key).category].GetComponent<HierarchyReferences>().GetReference<Transform>("Content");
      this.projectTechs[projectTech.Key].transform.SetParent(reference);
    }
    pooledList.Sort((Comparison<Tuple<Tuple<string, GameObject>, int>>) ((a, b) => a.second.CompareTo(b.second)));
    foreach (Tuple<Tuple<string, GameObject>, int> tuple in (List<Tuple<Tuple<string, GameObject>, int>>) pooledList)
      tuple.first.second.transform.SetAsLastSibling();
    pooledList.Recycle();
  }

  private void PopulateFilterButtons()
  {
    foreach (KeyValuePair<string, List<Tag>> filterPreset in this.filterPresets)
    {
      KeyValuePair<string, List<Tag>> kvp = filterPreset;
      GameObject gameObject = Util.KInstantiateUI(this.filterButtonPrefab, this.searchFiltersContainer, true);
      this.filterButtons.Add(kvp.Key, gameObject);
      this.filterStates.Add(kvp.Key, false);
      MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
      LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
      StringEntry text = Strings.Get("STRINGS.UI.RESEARCHSCREEN.FILTER_BUTTONS." + kvp.Key.ToUpper());
      string sourceText = (string) text;
      componentInChildren.SetText(sourceText);
      toggle.onClick += (System.Action) (() =>
      {
        foreach (KeyValuePair<string, GameObject> filterButton in this.filterButtons)
        {
          if (filterButton.Key != kvp.Key)
          {
            this.filterStates[filterButton.Key] = false;
            this.filterButtons[filterButton.Key].GetComponent<MultiToggle>().ChangeState(this.filterStates[filterButton.Key] ? 1 : 0);
          }
        }
        this.filterStates[kvp.Key] = !this.filterStates[kvp.Key];
        toggle.ChangeState(this.filterStates[kvp.Key] ? 1 : 0);
        this.searchBox.text = this.filterStates[kvp.Key] ? text.String : "";
      });
    }
  }

  public void RefreshQueue()
  {
  }

  private void RefreshWidgets()
  {
    List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
    foreach (KeyValuePair<string, GameObject> projectTech in this.projectTechs)
    {
      KeyValuePair<string, GameObject> kvp = projectTech;
      if (Db.Get().Techs.Get(kvp.Key).IsComplete())
        kvp.Value.GetComponent<MultiToggle>().ChangeState(2);
      else if (researchQueue.Find((Predicate<TechInstance>) (match => match.tech.Id == kvp.Key)) != null)
        kvp.Value.GetComponent<MultiToggle>().ChangeState(1);
      else
        kvp.Value.GetComponent<MultiToggle>().ChangeState(0);
    }
  }

  private void RefreshWidgetProgressBars(string techID, GameObject widget)
  {
    HierarchyReferences component1 = widget.GetComponent<HierarchyReferences>();
    ResearchPointInventory progressInventory = Research.Instance.GetTechInstance(techID).progressInventory;
    int num1 = 0;
    for (int index = 0; index < Research.Instance.researchTypes.Types.Count; ++index)
    {
      if (Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID.ContainsKey(Research.Instance.researchTypes.Types[index].id) && (double) Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[index].id] > 0.0)
      {
        HierarchyReferences component2 = component1.GetReference<RectTransform>("BarRows").GetChild(1 + num1).GetComponent<HierarchyReferences>();
        float num2 = progressInventory.PointsByTypeID[Research.Instance.researchTypes.Types[index].id] / Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[index].id];
        RectTransform rectTransform = component2.GetReference<Image>("Bar").rectTransform;
        rectTransform.sizeDelta = new Vector2(rectTransform.parent.rectTransform().rect.width * num2, rectTransform.sizeDelta.y);
        component2.GetReference<LocText>("Label").SetText($"{progressInventory.PointsByTypeID[Research.Instance.researchTypes.Types[index].id].ToString()}/{Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[index].id].ToString()}");
        ++num1;
      }
    }
  }

  private GameObject SpawnTechWidget(string techID, GameObject parentContainer)
  {
    GameObject gameObject1 = Util.KInstantiateUI(this.techWidgetRootAltPrefab, parentContainer, true);
    HierarchyReferences component = gameObject1.GetComponent<HierarchyReferences>();
    gameObject1.name = Db.Get().Techs.Get(techID).Name;
    component.GetReference<LocText>("Label").SetText(Db.Get().Techs.Get(techID).Name);
    if (!this.projectTechItems.ContainsKey(techID))
      this.projectTechItems.Add(techID, new Dictionary<string, GameObject>());
    RectTransform reference = component.GetReference<RectTransform>("UnlockContainer");
    foreach (TechItem unlockedItem in Db.Get().Techs.Get(techID).unlockedItems)
    {
      if (Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) unlockedItem))
      {
        GameObject gameObject2 = Util.KInstantiateUI(this.techItemPrefab, reference.gameObject, true);
        gameObject2.GetComponentsInChildren<Image>()[1].sprite = unlockedItem.UISprite();
        gameObject2.GetComponentsInChildren<LocText>()[0].SetText(unlockedItem.Name);
        gameObject2.GetComponent<MultiToggle>().onClick += (System.Action) (() => this.researchScreen.ZoomToTech(techID));
        gameObject2.GetComponentsInChildren<Image>()[0].color = this.evenRow ? this.evenRowColor : this.oddRowColor;
        this.evenRow = !this.evenRow;
        if (!this.projectTechItems[techID].ContainsKey(unlockedItem.Id))
          this.projectTechItems[techID].Add(unlockedItem.Id, gameObject2);
      }
    }
    gameObject1.GetComponent<MultiToggle>().onClick += (System.Action) (() => this.researchScreen.ZoomToTech(techID));
    return gameObject1;
  }

  private void ChangeGameObjectActive(GameObject target, bool targetActiveState)
  {
    if (target.activeSelf == targetActiveState)
      return;
    if (targetActiveState)
    {
      this.QueuedActivations.Add(target);
      if (!this.QueuedDeactivations.Contains(target))
        return;
      this.QueuedDeactivations.Remove(target);
    }
    else
    {
      this.QueuedDeactivations.Add(target);
      if (!this.QueuedActivations.Contains(target))
        return;
      this.QueuedActivations.Remove(target);
    }
  }

  private bool IsTextFilterActive() => !string.IsNullOrEmpty(this.currentSearchString);

  private bool AnyFilterActive()
  {
    return this.completionFilter != ResearchScreenSideBar.CompletionState.All || this.IsTextFilterActive();
  }

  private int GetTechItemMatchScore(SearchUtil.TechCache techCache, string techItemID)
  {
    TechItem restrictions = Db.Get().TechItems.Get(techItemID);
    if (!Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
      return 0;
    switch (this.completionFilter)
    {
      case ResearchScreenSideBar.CompletionState.Available:
        if (restrictions.IsComplete() || !restrictions.ParentTech.ArePrerequisitesComplete())
          return 0;
        break;
      case ResearchScreenSideBar.CompletionState.Completed:
        if (!restrictions.IsComplete())
          return 0;
        break;
    }
    return !this.IsTextFilterActive() ? 100 : techCache.techItems[techItemID].Score;
  }

  private int GetTechMatchScore(string techID)
  {
    Tech tech = Db.Get().Techs.Get(techID);
    switch (this.completionFilter)
    {
      case ResearchScreenSideBar.CompletionState.Available:
        if (tech.IsComplete() || !tech.ArePrerequisitesComplete())
          return 0;
        break;
      case ResearchScreenSideBar.CompletionState.Completed:
        if (!tech.IsComplete())
          return 0;
        break;
    }
    return !this.IsTextFilterActive() ? 100 : this.techCaches[techID].Score;
  }

  public void ResetFilter()
  {
    this.SetTextFilter("", true);
    this.searchBox.text = "";
    foreach (KeyValuePair<string, GameObject> filterButton in this.filterButtons)
    {
      this.filterStates[filterButton.Key] = false;
      this.filterButtons[filterButton.Key].GetComponent<MultiToggle>().ChangeState(this.filterStates[filterButton.Key] ? 1 : 0);
    }
    this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All, true);
    this.UpdateProjectFilter();
  }

  public void FocusSearchBox() => this.searchBox.Select();

  public void SetSearch(string newSearch)
  {
    newSearch = STRINGS.UI.StripLinkFormatting(newSearch);
    this.searchBox.text = newSearch;
    this.SetTextFilter(newSearch, false);
  }

  private enum CompletionState
  {
    All,
    Available,
    Completed,
  }
}
