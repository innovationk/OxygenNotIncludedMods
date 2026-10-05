// Decompiled with JetBrains decompiler
// Type: ComplexFabricator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ComplexFabricator")]
public class ComplexFabricator : RemoteDockWorkTargetComponent, ISim200ms, ISim1000ms
{
  private const int MaxPrefetchCount = 2;
  public bool duplicantOperated = true;
  protected ComplexFabricatorWorkable workable;
  public string SideScreenSubtitleLabel = (string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.SUBTITLE;
  public string SideScreenRecipeScreenTitle = (string) UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_DETAILS;
  [SerializeField]
  public HashedString fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;
  [SerializeField]
  public float heatedTemperature;
  [SerializeField]
  public bool storeProduced;
  [SerializeField]
  public bool allowManualFluidDelivery = true;
  public ComplexFabricatorSideScreen.StyleSetting sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
  public bool labelByResult = true;
  public Vector3 outputOffset = Vector3.zero;
  public ChoreType choreType;
  public bool keepExcessLiquids;
  public Tag keepAdditionalTag = Tag.Invalid;
  public StatusItem workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorProducing;
  public static int MAX_QUEUE_SIZE = 99;
  public static int QUEUE_INFINITE = -1;
  [Serialize]
  private Dictionary<string, int> recipeQueueCounts = new Dictionary<string, int>();
  [Serialize]
  public Dictionary<string, string> mostRecentRecipeSelectionByCategory = new Dictionary<string, string>();
  private int nextOrderIdx;
  private bool nextOrderIsWorkable;
  private int workingOrderIdx = -1;
  [Serialize]
  private string lastWorkingRecipe;
  [Serialize]
  private float orderProgress;
  private List<int> openOrderCounts = new List<int>();
  [Serialize]
  private bool forbidMutantSeeds;
  private Tag[] forbiddenMutantTags = new Tag[1]
  {
    GameTags.MutatedSeed
  };
  private bool queueDirty = true;
  private bool hasOpenOrders;
  private List<FetchList2> fetchListList = new List<FetchList2>();
  private Chore chore;
  private bool cancelling;
  private ComplexRecipe[] recipe_list;
  private Dictionary<Tag, float> materialNeedCache = new Dictionary<Tag, float>();
  [SerializeField]
  public Storage inStorage;
  [SerializeField]
  public Storage buildStorage;
  [SerializeField]
  public Storage outStorage;
  [MyCmpAdd]
  private LoopingSounds loopingSounds;
  [MyCmpReq]
  protected Operational operational;
  [MyCmpAdd]
  protected ComplexFabricatorSM fabricatorSM;
  private ProgressBar progressBar;
  public bool showProgressBar;
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((Action<ComplexFabricator, object>) ((component, data) => component.OnStorageChange(data)));
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnParticleStorageChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((Action<ComplexFabricator, object>) ((component, data) => component.OnStorageChange(data)));
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnDroppedAllDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((Action<ComplexFabricator, object>) ((component, data) => component.OnDroppedAll(data)));
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((Action<ComplexFabricator, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((Action<ComplexFabricator, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>((Action<ComplexFabricator, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  public ComplexFabricatorWorkable Workable => this.workable;

  public bool ForbidMutantSeeds
  {
    get => this.forbidMutantSeeds;
    set
    {
      this.forbidMutantSeeds = value;
      this.ToggleMutantSeedFetches();
      this.UpdateMutantSeedStatusItem();
    }
  }

  public Tag[] ForbiddenTags => !this.forbidMutantSeeds ? (Tag[]) null : this.forbiddenMutantTags;

  public int CurrentOrderIdx => this.nextOrderIdx;

  public ComplexRecipe CurrentWorkingOrder
  {
    get => !this.HasWorkingOrder ? (ComplexRecipe) null : this.recipe_list[this.workingOrderIdx];
  }

  public ComplexRecipe NextOrder
  {
    get => !this.nextOrderIsWorkable ? (ComplexRecipe) null : this.recipe_list[this.nextOrderIdx];
  }

  public float OrderProgress
  {
    get => this.orderProgress;
    set => this.orderProgress = value;
  }

  public bool HasAnyOrder => this.HasWorkingOrder || this.hasOpenOrders;

  public bool HasWorker
  {
    get => !this.duplicantOperated || (UnityEngine.Object) this.workable.worker != (UnityEngine.Object) null;
  }

  public bool WaitingForWorker => this.HasWorkingOrder && !this.HasWorker;

  private bool HasWorkingOrder => this.workingOrderIdx > -1;

  public List<ComplexRecipe> GetRecipesWithCategoryID(string categoryID)
  {
    return ((IEnumerable<ComplexRecipe>) this.recipe_list).Where<ComplexRecipe>((Func<ComplexRecipe, bool>) (match => match.recipeCategoryID == categoryID)).ToList<ComplexRecipe>();
  }

  public List<FetchList2> DebugFetchLists => this.fetchListList;

  [OnDeserialized]
  protected virtual void OnDeserializedMethod()
  {
    List<string> stringList = new List<string>();
    foreach (string key in this.recipeQueueCounts.Keys)
    {
      if (ComplexRecipeManager.Get().GetRecipe(key) == null)
        stringList.Add(key);
    }
    foreach (string key in stringList)
    {
      Debug.LogWarningFormat("{1} removing missing recipe from queue: {0}", (object) key, (object) this.name);
      this.recipeQueueCounts.Remove(key);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.GetRecipes();
    this.simRenderLoadBalance = true;
    this.choreType = Db.Get().ChoreTypes.Fabricate;
    this.Subscribe<ComplexFabricator>(-1957399615, ComplexFabricator.OnDroppedAllDelegate);
    this.Subscribe<ComplexFabricator>(-592767678, ComplexFabricator.OnOperationalChangedDelegate);
    this.Subscribe<ComplexFabricator>(-905833192, ComplexFabricator.OnCopySettingsDelegate);
    this.Subscribe<ComplexFabricator>(-1697596308, ComplexFabricator.OnStorageChangeDelegate);
    this.Subscribe<ComplexFabricator>(-1837862626, ComplexFabricator.OnParticleStorageChangedDelegate);
    this.workable = this.GetComponent<ComplexFabricatorWorkable>();
    Components.ComplexFabricators.Add(this);
    this.Subscribe<ComplexFabricator>(493375141, ComplexFabricator.OnRefreshUserMenuDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.InitRecipeQueueCount();
    foreach (string key in this.recipeQueueCounts.Keys)
    {
      if (this.recipeQueueCounts[key] == 100)
        this.recipeQueueCounts[key] = ComplexFabricator.QUEUE_INFINITE;
    }
    this.buildStorage.Transfer(this.inStorage, true, true);
    this.DropExcessIngredients(this.inStorage);
    int recipeIndex = this.FindRecipeIndex(this.lastWorkingRecipe);
    if (recipeIndex > -1)
      this.nextOrderIdx = recipeIndex;
    this.UpdateMutantSeedStatusItem();
  }

  protected override void OnCleanUp()
  {
    this.CancelAllOpenOrders();
    this.CancelChore();
    Components.ComplexFabricators.Remove(this);
    base.OnCleanUp();
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!Game.IsDlcActiveForCurrentSave("EXPANSION1_ID") || !this.HasRecipiesWithSeeds())
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", (string) (this.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT), (System.Action) (() =>
    {
      this.ForbidMutantSeeds = !this.ForbidMutantSeeds;
      this.OnRefreshUserMenu((object) null);
    }), tooltipText: (string) UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP));
  }

  private bool HasRecipiesWithSeeds()
  {
    bool flag = false;
    foreach (ComplexRecipe recipe in this.recipe_list)
    {
      foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
      {
        GameObject prefab = Assets.GetPrefab(ingredient.material);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && (UnityEngine.Object) prefab.GetComponent<PlantableSeed>() != (UnityEngine.Object) null)
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  private void UpdateMutantSeedStatusItem()
  {
    this.gameObject.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.FabricatorAcceptsMutantSeeds, Game.IsDlcActiveForCurrentSave("EXPANSION1_ID") && this.HasRecipiesWithSeeds() && !this.forbidMutantSeeds);
  }

  private void OnOperationalChanged(object data)
  {
    if (((Boxed<bool>) data).value)
      this.queueDirty = true;
    else
      this.CancelAllOpenOrders();
    this.UpdateChore();
  }

  public virtual void Sim1000ms(float dt)
  {
    this.RefreshAndStartNextOrder();
    if (this.materialNeedCache.Count <= 0 || this.fetchListList.Count != 0)
      return;
    Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "{0} has material needs cached, but no open fetches. materialNeedCache={1}, fetchListList={2}", (object) this.gameObject, (object) this.materialNeedCache.Count, (object) this.fetchListList.Count);
    this.queueDirty = true;
  }

  protected virtual float ComputeWorkProgress(float dt, ComplexRecipe recipe) => dt / recipe.time;

  public void Sim200ms(float dt)
  {
    if (!this.operational.IsOperational)
      return;
    this.operational.SetActive(this.HasWorkingOrder && this.HasWorker);
    if (this.duplicantOperated || !this.HasWorkingOrder)
      return;
    this.orderProgress += this.ComputeWorkProgress(dt, this.recipe_list[this.workingOrderIdx]);
    if ((double) this.orderProgress < 1.0)
      return;
    this.ShowProgressBar(false);
    this.CompleteWorkingOrder();
  }

  private void RefreshAndStartNextOrder()
  {
    if (!this.operational.IsOperational)
      return;
    if (this.queueDirty)
      this.RefreshQueue();
    if (this.HasWorkingOrder || !this.nextOrderIsWorkable)
      return;
    this.ShowProgressBar(true);
    this.StartWorkingOrder(this.nextOrderIdx);
  }

  public virtual float GetPercentComplete() => this.orderProgress;

  private void ShowProgressBar(bool show)
  {
    if (show && this.showProgressBar && !this.duplicantOperated)
    {
      if ((UnityEngine.Object) this.progressBar == (UnityEngine.Object) null)
        this.progressBar = ProgressBar.CreateProgressBar(this.gameObject, new Func<float>(this.GetPercentComplete));
      this.progressBar.enabled = true;
      this.progressBar.SetVisibility(true);
    }
    else
    {
      if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
        return;
      this.progressBar.gameObject.DeleteObject();
      this.progressBar = (ProgressBar) null;
    }
  }

  public void SetQueueDirty() => this.queueDirty = true;

  private void RefreshQueue()
  {
    this.queueDirty = false;
    this.ValidateWorkingOrder();
    this.ValidateNextOrder();
    this.UpdateOpenOrders();
    this.DropExcessIngredients(this.inStorage);
    this.Trigger(1721324763, (object) this);
  }

  private void StartWorkingOrder(int index)
  {
    Debug.Assert(!this.HasWorkingOrder, (object) "machineOrderIdx already set");
    this.workingOrderIdx = index;
    if (this.recipe_list[this.workingOrderIdx].id != this.lastWorkingRecipe)
    {
      this.orderProgress = 0.0f;
      this.lastWorkingRecipe = this.recipe_list[this.workingOrderIdx].id;
    }
    this.TransferCurrentRecipeIngredientsForBuild();
    Debug.Assert(this.openOrderCounts[this.workingOrderIdx] > 0, (object) "openOrderCount invalid");
    this.openOrderCounts[this.workingOrderIdx]--;
    this.UpdateChore();
    this.Trigger(2023536846, (object) this.recipe_list[this.workingOrderIdx]);
    this.AdvanceNextOrder();
  }

  private void CancelWorkingOrder()
  {
    Debug.Assert(this.HasWorkingOrder, (object) "machineOrderIdx not set");
    this.buildStorage.Transfer(this.inStorage, true, true);
    this.workingOrderIdx = -1;
    this.orderProgress = 0.0f;
    this.UpdateChore();
  }

  public virtual void CompleteWorkingOrder()
  {
    if (!this.HasWorkingOrder)
    {
      Debug.LogWarning((object) "CompleteWorkingOrder called with no working order.", (UnityEngine.Object) this.gameObject);
    }
    else
    {
      ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
      this.SpawnOrderProduct(recipe);
      float num = this.buildStorage.MassStored();
      if ((double) num != 0.0)
      {
        Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "{0} build storage contains mass {1} after order completion.", (object) this.gameObject, (object) num);
        this.buildStorage.Transfer(this.inStorage, true, true);
      }
      this.DecrementRecipeQueueCountInternal(recipe);
      this.workingOrderIdx = -1;
      this.orderProgress = 0.0f;
      this.CancelChore();
      this.Trigger(1355439576, (object) recipe);
      if (this.cancelling)
        return;
      this.RefreshAndStartNextOrder();
    }
  }

  private void ValidateWorkingOrder()
  {
    if (!this.HasWorkingOrder || this.IsRecipeQueued(this.recipe_list[this.workingOrderIdx]))
      return;
    this.CancelWorkingOrder();
  }

  private void UpdateChore()
  {
    if (!this.duplicantOperated)
      return;
    bool flag = this.operational.IsOperational && this.HasWorkingOrder;
    if (flag && this.chore == null)
    {
      this.CreateChore();
    }
    else
    {
      if (flag || this.chore == null)
        return;
      this.CancelChore();
    }
  }

  private void AdvanceNextOrder()
  {
    for (int index = 0; index < this.recipe_list.Length; ++index)
    {
      this.nextOrderIdx = (this.nextOrderIdx + 1) % this.recipe_list.Length;
      ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
      this.nextOrderIsWorkable = this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage);
      if (this.nextOrderIsWorkable)
        break;
    }
  }

  private void ValidateNextOrder()
  {
    ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
    this.nextOrderIsWorkable = this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage);
    if (this.nextOrderIsWorkable)
      return;
    this.AdvanceNextOrder();
  }

  private void CancelAllOpenOrders()
  {
    for (int index = 0; index < this.openOrderCounts.Count; ++index)
      this.openOrderCounts[index] = 0;
    this.ClearMaterialNeeds();
    this.CancelFetches();
  }

  private void UpdateOpenOrders()
  {
    ComplexRecipe[] recipes = this.GetRecipes();
    if (recipes.Length != this.openOrderCounts.Count)
      Debug.LogErrorFormat((UnityEngine.Object) this.gameObject, "Recipe count {0} doesn't match open order count {1}", (object) recipes.Length, (object) this.openOrderCounts.Count);
    bool flag = false;
    this.hasOpenOrders = false;
    for (int index = 0; index < recipes.Length; ++index)
    {
      int recipePrefetchCount = this.GetRecipePrefetchCount(recipes[index]);
      if (recipePrefetchCount > 0)
        this.hasOpenOrders = true;
      int openOrderCount = this.openOrderCounts[index];
      if (openOrderCount != recipePrefetchCount)
      {
        if (recipePrefetchCount < openOrderCount)
          flag = true;
        this.openOrderCounts[index] = recipePrefetchCount;
      }
    }
    DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
    DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
    for (int index = 0; index < this.openOrderCounts.Count; ++index)
    {
      if (this.openOrderCounts[index] > 0)
      {
        foreach (ComplexRecipe.RecipeElement ingredient in this.recipe_list[index].ingredients)
          pooledDictionary[ingredient.material] = this.inStorage.GetAmountAvailable(ingredient.material);
      }
    }
    for (int index = 0; index < this.recipe_list.Length; ++index)
    {
      int openOrderCount = this.openOrderCounts[index];
      if (openOrderCount > 0)
      {
        foreach (ComplexRecipe.RecipeElement ingredient in this.recipe_list[index].ingredients)
        {
          float num1 = ingredient.amount * (float) openOrderCount;
          float num2 = num1 - pooledDictionary[ingredient.material];
          if ((double) num2 > 0.0)
          {
            float num3;
            missingAmounts.TryGetValue(ingredient.material, out num3);
            float num4 = num2 * FetchChore.GetMinimumFetchAmount(ingredient.material, 1f);
            missingAmounts[ingredient.material] = num3 + num4;
            pooledDictionary[ingredient.material] = 0.0f;
          }
          else
            pooledDictionary[ingredient.material] -= num1;
        }
      }
    }
    if (flag)
      this.CancelFetches();
    if (missingAmounts.Count > 0)
      this.UpdateFetches(missingAmounts);
    this.UpdateMaterialNeeds((Dictionary<Tag, float>) missingAmounts);
    missingAmounts.Recycle();
    pooledDictionary.Recycle();
  }

  private void UpdateMaterialNeeds(Dictionary<Tag, float> missingAmounts)
  {
    this.ClearMaterialNeeds();
    foreach (KeyValuePair<Tag, float> missingAmount in missingAmounts)
    {
      MaterialNeeds.UpdateNeed(missingAmount.Key, missingAmount.Value, this.gameObject.GetMyWorldId());
      this.materialNeedCache.Add(missingAmount.Key, missingAmount.Value);
    }
  }

  private void ClearMaterialNeeds()
  {
    foreach (KeyValuePair<Tag, float> keyValuePair in this.materialNeedCache)
      MaterialNeeds.UpdateNeed(keyValuePair.Key, -keyValuePair.Value, this.gameObject.GetMyWorldId());
    this.materialNeedCache.Clear();
  }

  public int HighestHEPQueued()
  {
    int val2 = 0;
    foreach (KeyValuePair<string, int> recipeQueueCount in this.recipeQueueCounts)
    {
      if (recipeQueueCount.Value > 0)
        val2 = Math.Max(this.recipe_list[this.FindRecipeIndex(recipeQueueCount.Key)].consumedHEP, val2);
    }
    return val2;
  }

  private void OnFetchComplete()
  {
    for (int index = this.fetchListList.Count - 1; index >= 0; --index)
    {
      if (this.fetchListList[index].IsComplete)
      {
        this.fetchListList.RemoveAt(index);
        this.queueDirty = true;
      }
    }
  }

  private void OnStorageChange(object data) => this.queueDirty = true;

  private void OnDroppedAll(object data)
  {
    if (this.HasWorkingOrder)
      this.CancelWorkingOrder();
    this.CancelAllOpenOrders();
    this.RefreshQueue();
  }

  private void DropExcessIngredients(Storage storage)
  {
    HashSet<Tag> tagSet = new HashSet<Tag>();
    if (this.keepAdditionalTag != Tag.Invalid)
      tagSet.Add(this.keepAdditionalTag);
    for (int index = 0; index < this.recipe_list.Length; ++index)
    {
      ComplexRecipe recipe = this.recipe_list[index];
      if (this.IsRecipeQueued(recipe))
      {
        foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
          tagSet.Add(ingredient.material);
      }
    }
    for (int index = storage.items.Count - 1; index >= 0; --index)
    {
      GameObject go = storage.items[index];
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
      {
        PrimaryElement component1 = go.GetComponent<PrimaryElement>();
        if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null) && (!this.keepExcessLiquids || !component1.Element.IsLiquid))
        {
          KPrefabID component2 = go.GetComponent<KPrefabID>();
          if ((bool) (UnityEngine.Object) component2 && !tagSet.Contains(component2.PrefabID()))
            storage.Drop(go, true);
        }
      }
    }
  }

  private void OnCopySettings(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
      return;
    ComplexFabricator component = gameObject.GetComponent<ComplexFabricator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    this.ForbidMutantSeeds = component.ForbidMutantSeeds;
    foreach (ComplexRecipe recipe in this.recipe_list)
    {
      int count;
      if (!component.recipeQueueCounts.TryGetValue(recipe.id, out count))
        count = 0;
      this.SetRecipeQueueCountInternal(recipe, count);
    }
    this.RefreshQueue();
  }

  private int CompareRecipe(ComplexRecipe a, ComplexRecipe b)
  {
    return a.sortOrder != b.sortOrder ? a.sortOrder - b.sortOrder : StringComparer.InvariantCulture.Compare(a.id, b.id);
  }

  public ComplexRecipe GetRecipe(string id)
  {
    if (this.recipe_list != null)
    {
      foreach (ComplexRecipe recipe in this.recipe_list)
      {
        if (recipe.id == id)
          return recipe;
      }
    }
    return (ComplexRecipe) null;
  }

  public ComplexRecipe[] GetRecipes()
  {
    if (this.recipe_list == null)
    {
      Tag prefabTag = this.GetComponent<KPrefabID>().PrefabTag;
      List<ComplexRecipe> recipes = ComplexRecipeManager.Get().recipes;
      List<ComplexRecipe> complexRecipeList = new List<ComplexRecipe>();
      foreach (ComplexRecipe restrictions in recipes)
      {
        foreach (Tag fabricator in restrictions.fabricators)
        {
          if (fabricator == prefabTag && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) restrictions))
            complexRecipeList.Add(restrictions);
        }
      }
      this.recipe_list = complexRecipeList.ToArray();
      Array.Sort<ComplexRecipe>(this.recipe_list, new Comparison<ComplexRecipe>(this.CompareRecipe));
      foreach (ComplexRecipe recipe in this.recipe_list)
      {
        if (!this.mostRecentRecipeSelectionByCategory.ContainsKey(recipe.recipeCategoryID))
          this.mostRecentRecipeSelectionByCategory.Add(recipe.recipeCategoryID, (string) null);
      }
    }
    return this.recipe_list;
  }

  private void InitRecipeQueueCount()
  {
    foreach (ComplexRecipe recipe in this.GetRecipes())
    {
      bool flag = false;
      foreach (string key in this.recipeQueueCounts.Keys)
      {
        if (key == recipe.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.recipeQueueCounts.Add(recipe.id, 0);
      this.openOrderCounts.Add(0);
    }
  }

  private int FindRecipeIndex(string id)
  {
    for (int recipeIndex = 0; recipeIndex < this.recipe_list.Length; ++recipeIndex)
    {
      if (this.recipe_list[recipeIndex].id == id)
        return recipeIndex;
    }
    return -1;
  }

  public int GetRecipeQueueCount(ComplexRecipe recipe) => this.recipeQueueCounts[recipe.id];

  public int GetIngredientQueueCount(string recipeCategoryID, Tag tag)
  {
    int ingredientQueueCount = 0;
    foreach (ComplexRecipe recipe in this.GetRecipesWithCategoryID(recipeCategoryID))
    {
      foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
      {
        if (ingredient.material == tag)
        {
          ingredientQueueCount += this.GetRecipeQueueCount(recipe);
          break;
        }
      }
    }
    return ingredientQueueCount;
  }

  public int GetRecipeCategoryQueueCount(string recipeCategoryID)
  {
    int categoryQueueCount = 0;
    foreach (ComplexRecipe complexRecipe in ((IEnumerable<ComplexRecipe>) this.recipe_list).Where<ComplexRecipe>((Func<ComplexRecipe, bool>) (match => match.recipeCategoryID == recipeCategoryID)))
    {
      if (this.recipeQueueCounts[complexRecipe.id] == ComplexFabricator.QUEUE_INFINITE)
        return ComplexFabricator.QUEUE_INFINITE;
      categoryQueueCount += this.recipeQueueCounts[complexRecipe.id];
    }
    return categoryQueueCount;
  }

  public bool IsRecipeQueued(ComplexRecipe recipe)
  {
    int recipeQueueCount = this.recipeQueueCounts[recipe.id];
    Debug.Assert(recipeQueueCount >= 0 || recipeQueueCount == ComplexFabricator.QUEUE_INFINITE);
    return recipeQueueCount != 0;
  }

  public int GetRecipePrefetchCount(ComplexRecipe recipe)
  {
    int remainingQueueCount = this.GetRemainingQueueCount(recipe);
    Debug.Assert(remainingQueueCount >= 0);
    return Mathf.Min(2, remainingQueueCount);
  }

  private int GetRemainingQueueCount(ComplexRecipe recipe)
  {
    int recipeQueueCount = this.recipeQueueCounts[recipe.id];
    Debug.Assert(recipeQueueCount >= 0 || recipeQueueCount == ComplexFabricator.QUEUE_INFINITE);
    if (recipeQueueCount == ComplexFabricator.QUEUE_INFINITE)
      return ComplexFabricator.MAX_QUEUE_SIZE;
    if (recipeQueueCount <= 0)
      return 0;
    if (this.IsCurrentRecipe(recipe))
      --recipeQueueCount;
    return recipeQueueCount;
  }

  private bool IsCurrentRecipe(ComplexRecipe recipe)
  {
    return this.workingOrderIdx >= 0 && this.recipe_list[this.workingOrderIdx].id == recipe.id;
  }

  public void SetRecipeQueueCount(ComplexRecipe recipe, int count)
  {
    this.SetRecipeQueueCountInternal(recipe, count);
    this.RefreshQueue();
  }

  private void SetRecipeQueueCountInternal(ComplexRecipe recipe, int count)
  {
    this.recipeQueueCounts[recipe.id] = count;
  }

  public void IncrementRecipeQueueCount(ComplexRecipe recipe)
  {
    if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
      this.recipeQueueCounts[recipe.id] = 0;
    else if (this.recipeQueueCounts[recipe.id] >= ComplexFabricator.MAX_QUEUE_SIZE)
      this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
    else
      this.recipeQueueCounts[recipe.id]++;
    this.RefreshQueue();
  }

  public void DecrementRecipeQueueCount(ComplexRecipe recipe, bool respectInfinite = true)
  {
    this.DecrementRecipeQueueCountInternal(recipe, respectInfinite);
    this.RefreshQueue();
  }

  private void DecrementRecipeQueueCountInternal(ComplexRecipe recipe, bool respectInfinite = true)
  {
    if (respectInfinite && this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
      return;
    if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
      this.recipeQueueCounts[recipe.id] = ComplexFabricator.MAX_QUEUE_SIZE;
    else if (this.recipeQueueCounts[recipe.id] == 0)
      this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
    else
      this.recipeQueueCounts[recipe.id]--;
  }

  private void CreateChore()
  {
    Debug.Assert(this.chore == null, (object) "chore should be null");
    this.chore = this.workable.CreateWorkChore(this.choreType, this.orderProgress);
  }

  public override Chore RemoteDockChore => !this.duplicantOperated ? (Chore) null : this.chore;

  private void CancelChore()
  {
    if (this.cancelling)
      return;
    this.cancelling = true;
    if (this.chore != null)
    {
      this.chore.Cancel("order cancelled");
      this.chore = (Chore) null;
    }
    this.cancelling = false;
  }

  private void UpdateFetches(
    DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts)
  {
    ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
    foreach (KeyValuePair<Tag, float> missingAmount in (Dictionary<Tag, float>) missingAmounts)
    {
      if (!this.allowManualFluidDelivery)
      {
        Element element = ElementLoader.GetElement(missingAmount.Key);
        if (element != null && (element.IsLiquid || element.IsGas))
          continue;
      }
      if ((double) missingAmount.Value >= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.HasPendingFetch(missingAmount.Key))
      {
        FetchList2 fetchList2_1 = new FetchList2(this.inStorage, byHash);
        FetchList2 fetchList2_2 = fetchList2_1;
        Tag key = missingAmount.Key;
        float num = missingAmount.Value;
        Tag[] forbiddenTags = this.ForbiddenTags;
        double amount = (double) num;
        fetchList2_2.Add(key, forbiddenTags, (float) amount);
        fetchList2_1.ShowStatusItem = false;
        fetchList2_1.Submit(new System.Action(this.OnFetchComplete), false);
        this.fetchListList.Add(fetchList2_1);
      }
    }
  }

  private bool HasPendingFetch(Tag tag)
  {
    foreach (FetchList2 fetchList in this.fetchListList)
    {
      float num;
      fetchList.MinimumAmount.TryGetValue(tag, out num);
      if ((double) num > 0.0)
        return true;
    }
    return false;
  }

  private void CancelFetches()
  {
    foreach (FetchList2 fetchList in this.fetchListList)
      fetchList.Cancel("cancel all orders");
    this.fetchListList.Clear();
  }

  protected virtual void TransferCurrentRecipeIngredientsForBuild()
  {
    foreach (ComplexRecipe.RecipeElement ingredient in this.recipe_list[this.workingOrderIdx].ingredients)
    {
      float unitAmount;
      while (true)
      {
        unitAmount = ingredient.amount - this.buildStorage.GetAmountAvailable(ingredient.material);
        if ((double) unitAmount > 0.0)
        {
          if ((double) this.inStorage.GetAmountAvailable(ingredient.material) > 0.0)
            this.inStorage.TransferUnitMass(this.buildStorage, ingredient.material, unitAmount, hide_popups: true);
          else
            break;
        }
        else
          goto label_7;
      }
      Debug.LogWarningFormat("TransferCurrentRecipeIngredientsForBuild ran out of {0} but still needed {1} more.", (object) ingredient.material, (object) unitAmount);
      continue;
label_7:;
    }
  }

  protected virtual bool HasIngredients(ComplexRecipe recipe, Storage storage)
  {
    ComplexRecipe.RecipeElement[] ingredients = recipe.ingredients;
    if (recipe.consumedHEP > 0)
    {
      HighEnergyParticleStorage component = this.GetComponent<HighEnergyParticleStorage>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null || (double) component.Particles < (double) recipe.consumedHEP)
        return false;
    }
    foreach (ComplexRecipe.RecipeElement recipeElement in ingredients)
    {
      float amountAvailable = storage.GetAmountAvailable(recipeElement.material);
      if ((double) recipeElement.amount - (double) amountAvailable >= (double) PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
        return false;
    }
    return true;
  }

  private void ToggleMutantSeedFetches()
  {
    if (!this.HasAnyOrder)
      return;
    ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
    List<FetchList2> fetchList2List = new List<FetchList2>();
    foreach (FetchList2 fetchList in this.fetchListList)
    {
      foreach (FetchOrder2 fetchOrder in fetchList.FetchOrders)
      {
        foreach (Tag tag in fetchOrder.Tags)
        {
          GameObject prefab = Assets.GetPrefab(tag);
          if ((UnityEngine.Object) prefab != (UnityEngine.Object) null && (UnityEngine.Object) prefab.GetComponent<PlantableSeed>() != (UnityEngine.Object) null)
          {
            fetchList.Cancel("MutantSeedTagChanged");
            fetchList2List.Add(fetchList);
          }
        }
      }
    }
    foreach (FetchList2 fetchList2_1 in fetchList2List)
    {
      this.fetchListList.Remove(fetchList2_1);
      foreach (FetchOrder2 fetchOrder in fetchList2_1.FetchOrders)
      {
        foreach (Tag tag1 in fetchOrder.Tags)
        {
          FetchList2 fetchList2_2 = new FetchList2(this.inStorage, byHash);
          FetchList2 fetchList2_3 = fetchList2_2;
          Tag tag2 = tag1;
          float totalAmount = fetchOrder.TotalAmount;
          Tag[] forbiddenTags = this.ForbiddenTags;
          double amount = (double) totalAmount;
          fetchList2_3.Add(tag2, forbiddenTags, (float) amount);
          fetchList2_2.ShowStatusItem = false;
          fetchList2_2.Submit(new System.Action(this.OnFetchComplete), false);
          this.fetchListList.Add(fetchList2_2);
        }
      }
    }
  }

  protected virtual List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    SimUtil.DiseaseInfo diseaseInfo;
    diseaseInfo.count = 0;
    diseaseInfo.idx = (byte) 0;
    float num1 = 0.0f;
    float num2 = 0.0f;
    string str = (string) null;
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
      num2 += ingredient.amount;
    ComplexRecipe.RecipeElement recipeElement = (ComplexRecipe.RecipeElement) null;
    Element element1 = (Element) null;
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      float num3 = ingredient.amount / num2;
      if (recipe.ProductHasFacade && str.IsNullOrWhiteSpace())
      {
        RepairableEquipment component = this.buildStorage.FindFirst(ingredient.material).GetComponent<RepairableEquipment>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          str = component.facadeID;
      }
      if (ingredient.inheritElement)
      {
        recipeElement = ingredient;
        element1 = this.buildStorage.FindFirst(ingredient.material).GetComponent<PrimaryElement>().Element;
      }
      if (ingredient.doNotConsume)
      {
        recipeElement = ingredient;
        this.buildStorage.TransferMass(this.outStorage, ingredient.material, ingredient.amount, true, true, true);
      }
      else
      {
        SimUtil.DiseaseInfo disease_info;
        float aggregate_temperature;
        this.buildStorage.ConsumeAndGetDisease(ingredient.material, ingredient.amount, out float _, out disease_info, out aggregate_temperature);
        if (disease_info.count > diseaseInfo.count)
          diseaseInfo = disease_info;
        num1 += aggregate_temperature * num3;
      }
    }
    if (recipe.consumedHEP > 0)
    {
      double num4 = (double) this.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet((float) recipe.consumedHEP);
    }
    foreach (ComplexRecipe.RecipeElement result in recipe.results)
    {
      GameObject first = this.buildStorage.FindFirst(result.material);
      if ((UnityEngine.Object) first != (UnityEngine.Object) null)
      {
        Edible component = first.GetComponent<Edible>();
        if ((bool) (UnityEngine.Object) component)
          ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component.GetProperName()), (string) UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
      }
      switch (result.temperatureOperation)
      {
        case ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature:
        case ComplexRecipe.RecipeElement.TemperatureOperation.Heated:
          GameObject prefab1 = Assets.GetPrefab(result.material);
          GameObject go1 = GameUtil.KInstantiate(prefab1, Grid.SceneLayer.Ore);
          int cell1 = Grid.PosToCell((KMonoBehaviour) this);
          go1.transform.SetPosition(Grid.CellToPosCCC(cell1, Grid.SceneLayer.Ore) + this.outputOffset);
          PrimaryElement component1 = go1.GetComponent<PrimaryElement>();
          component1.Units = result.amount;
          component1.Temperature = result.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature ? num1 : this.heatedTemperature;
          if (element1 != null)
            component1.SetElement(element1.id, false);
          if (recipe.ProductHasFacade && !str.IsNullOrWhiteSpace())
          {
            Equippable component2 = go1.GetComponent<Equippable>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
              EquippableFacade.AddFacadeToEquippable(component2, str);
          }
          go1.SetActive(true);
          float num5 = result.amount / recipe.TotalResultUnits();
          component1.AddDisease(diseaseInfo.idx, Mathf.RoundToInt((float) diseaseInfo.count * num5), "ComplexFabricator.CompleteOrder");
          if (!result.facadeID.IsNullOrWhiteSpace())
          {
            Equippable component3 = go1.GetComponent<Equippable>();
            if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
              EquippableFacade.AddFacadeToEquippable(component3, result.facadeID);
          }
          go1.GetComponent<KMonoBehaviour>().Trigger(748399584, (object) null);
          gameObjectList.Add(go1);
          if (this.storeProduced || result.storeElement)
            this.outStorage.Store(go1);
          PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) prefab1).first, PopFXManager.Instance.sprite_Plus, prefab1.GetProperName(), go1.transform, Vector3.zero);
          break;
        case ComplexRecipe.RecipeElement.TemperatureOperation.Melted:
          if (this.storeProduced || result.storeElement)
          {
            Element element2 = ElementLoader.GetElement(result.material);
            float temperature = element2.defaultValues.temperature;
            this.outStorage.AddLiquid(element2.id, result.amount, temperature, (byte) 0, 0);
            PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) element2).first, PopFXManager.Instance.sprite_Plus, element2.name, this.outStorage.transform, Vector3.zero);
            break;
          }
          break;
        case ComplexRecipe.RecipeElement.TemperatureOperation.Dehydrated:
          for (int index = 0; index < (int) result.amount; ++index)
          {
            GameObject prefab2 = Assets.GetPrefab(result.material);
            GameObject go2 = GameUtil.KInstantiate(prefab2, Grid.SceneLayer.Ore);
            int cell2 = Grid.PosToCell((KMonoBehaviour) this);
            go2.transform.SetPosition(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Ore) + this.outputOffset);
            float amount = recipeElement.amount / result.amount;
            go2.GetComponent<PrimaryElement>().Temperature = result.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature ? num1 : this.heatedTemperature;
            DehydratedFoodPackage component4 = go2.GetComponent<DehydratedFoodPackage>();
            if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
              this.outStorage.TransferMass(component4.GetComponent<Storage>(), recipeElement.material, amount, true);
            go2.SetActive(true);
            go2.GetComponent<KMonoBehaviour>().Trigger(748399584, (object) null);
            gameObjectList.Add(go2);
            if (this.storeProduced || result.storeElement)
              this.outStorage.Store(go2);
            PopFXManager.Instance.SpawnFX(Def.GetUISprite((object) prefab2).first, PopFXManager.Instance.sprite_Plus, prefab2.GetProperName(), go2.transform, Vector3.zero);
          }
          break;
      }
      if (gameObjectList.Count > 0)
      {
        SymbolOverrideController component5 = this.GetComponent<SymbolOverrideController>();
        if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
        {
          KAnim.Build build = gameObjectList[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
          KAnim.Build.Symbol symbol = build.GetSymbol((KAnimHashedString) build.name);
          if (symbol != null)
          {
            component5.TryRemoveSymbolOverride((HashedString) "output_tracker");
            component5.AddSymbolOverride((HashedString) "output_tracker", symbol);
          }
          else
            Debug.LogWarning((object) $"{component5.name} is missing symbol {build.name}");
        }
      }
    }
    if (recipe.producedHEP > 0)
    {
      double num6 = (double) this.GetComponent<HighEnergyParticleStorage>().Store((float) recipe.producedHEP);
    }
    return gameObjectList;
  }

  public virtual List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = new List<Descriptor>();
    ComplexRecipe[] recipes = this.GetRecipes();
    if (recipes.Length != 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.PROCESSES, (string) UI.BUILDINGEFFECTS.TOOLTIPS.PROCESSES);
      descriptors.Add(descriptor);
    }
    foreach (ComplexRecipe complexRecipe in recipes)
    {
      string str = "";
      string uiName = complexRecipe.GetUIName(false);
      foreach (ComplexRecipe.RecipeElement ingredient in complexRecipe.ingredients)
        str = $"{str}• {string.Format((string) UI.BUILDINGEFFECTS.PROCESSEDITEM, (object) ingredient.material.ProperName(), (object) ingredient.amount)}\n";
      Descriptor descriptor = new Descriptor(uiName, string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.FABRICATOR_INGREDIENTS, (object) str));
      descriptor.IncreaseIndent();
      descriptors.Add(descriptor);
    }
    return descriptors;
  }

  public virtual List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe)
  {
    return new List<Descriptor>();
  }

  public string GetConversationTopic()
  {
    if (this.HasWorkingOrder)
    {
      ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
      if (recipe != null)
        return recipe.results[0].material.Name;
    }
    return (string) null;
  }

  public bool NeedsMoreHEPForQueuedRecipe()
  {
    if (this.hasOpenOrders)
    {
      HighEnergyParticleStorage component = this.GetComponent<HighEnergyParticleStorage>();
      foreach (KeyValuePair<string, int> recipeQueueCount in this.recipeQueueCounts)
      {
        if (recipeQueueCount.Value > 0)
        {
          foreach (ComplexRecipe recipe in this.GetRecipes())
          {
            if (recipe.id == recipeQueueCount.Key && (double) recipe.consumedHEP > (double) component.Particles)
              return true;
          }
        }
      }
    }
    return false;
  }
}
