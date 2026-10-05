// Decompiled with JetBrains decompiler
// Type: FacadeSelectionPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FacadeSelectionPanel : KMonoBehaviour
{
  [SerializeField]
  private GameObject togglePrefab;
  [SerializeField]
  private RectTransform toggleContainer;
  [SerializeField]
  private bool usesScrollRect;
  [SerializeField]
  private LayoutElement scrollRect;
  private Dictionary<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggles = new Dictionary<string, FacadeSelectionPanel.FacadeToggle>();
  private List<GameObject> pooledFacadeToggles = new List<GameObject>();
  [SerializeField]
  private KButton getMoreButton;
  [SerializeField]
  private bool showGetMoreButton;
  [SerializeField]
  private bool hideWhenEmpty = true;
  [SerializeField]
  private bool useDummyPlaceholder;
  private GridLayoutGroup gridLayout;
  [SerializeField]
  private List<GameObject> dummyGridPlaceholders;
  public System.Action OnFacadeSelectionChanged;
  private ClothingOutfitUtility.OutfitType selectedOutfitCategory;
  private string selectedBuildingDefID;
  private FacadeSelectionPanel.ConfigType currentConfigType;
  private string _selectedFacade;
  public const string DEFAULT_FACADE_ID = "DEFAULT_FACADE";

  private int GridLayoutConstraintCount
  {
    get => (UnityEngine.Object) this.gridLayout != (UnityEngine.Object) null ? this.gridLayout.constraintCount : 3;
  }

  public ClothingOutfitUtility.OutfitType SelectedOutfitCategory
  {
    set
    {
      this.selectedOutfitCategory = value;
      this.Refresh();
    }
    get => this.selectedOutfitCategory;
  }

  public string SelectedBuildingDefID => this.selectedBuildingDefID;

  public string SelectedFacade
  {
    get => this._selectedFacade;
    set
    {
      if (!(this._selectedFacade != value))
        return;
      this._selectedFacade = value;
      switch (this.currentConfigType)
      {
        case FacadeSelectionPanel.ConfigType.BuildingFacade:
          this.RefreshTogglesForBuilding();
          break;
        case FacadeSelectionPanel.ConfigType.MinionOutfit:
          this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
          break;
      }
      if (this.OnFacadeSelectionChanged == null)
        return;
      this.OnFacadeSelectionChanged();
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gridLayout = this.toggleContainer.GetComponent<GridLayoutGroup>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.getMoreButton.ClearOnClick();
    this.getMoreButton.onClick += new System.Action(LockerMenuScreen.Instance.ShowInventoryScreen);
  }

  public void SetBuildingDef(string defID, string currentFacadeID = null)
  {
    this.currentConfigType = FacadeSelectionPanel.ConfigType.BuildingFacade;
    this.ClearToggles();
    this.selectedBuildingDefID = defID;
    this.SelectedFacade = currentFacadeID == null ? "DEFAULT_FACADE" : currentFacadeID;
    this.RefreshTogglesForBuilding();
    if (!this.hideWhenEmpty)
      return;
    this.gameObject.SetActive(Assets.GetBuildingDef(defID).AvailableFacades.Count != 0);
  }

  public void SetOutfitTarget(
    ClothingOutfitTarget outfitTarget,
    ClothingOutfitUtility.OutfitType outfitType)
  {
    this.currentConfigType = FacadeSelectionPanel.ConfigType.MinionOutfit;
    this.ClearToggles();
    this.SelectedFacade = outfitTarget.OutfitId;
    this.gameObject.SetActive(true);
  }

  private void ClearToggles()
  {
    foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggle in this.activeFacadeToggles)
    {
      List<GameObject> pooledFacadeToggles = this.pooledFacadeToggles;
      FacadeSelectionPanel.FacadeToggle facadeToggle = activeFacadeToggle.Value;
      GameObject gameObject = facadeToggle.gameObject;
      pooledFacadeToggles.Add(gameObject);
      facadeToggle = activeFacadeToggle.Value;
      facadeToggle.gameObject.SetActive(false);
    }
    this.activeFacadeToggles.Clear();
  }

  public void Refresh()
  {
    switch (this.currentConfigType)
    {
      case FacadeSelectionPanel.ConfigType.BuildingFacade:
        this.RefreshTogglesForBuilding();
        break;
      case FacadeSelectionPanel.ConfigType.MinionOutfit:
        this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
        break;
    }
    this.getMoreButton.gameObject.SetActive(this.showGetMoreButton);
    if (this.useDummyPlaceholder)
    {
      for (int index = 0; index < this.dummyGridPlaceholders.Count; ++index)
        this.dummyGridPlaceholders[index].SetActive(false);
      int num = 0;
      for (int index = 0; index < this.toggleContainer.transform.childCount; ++index)
      {
        if (this.toggleContainer.GetChild(index).gameObject.activeInHierarchy)
          ++num;
      }
      this.getMoreButton.transform.SetAsLastSibling();
      if (num % this.GridLayoutConstraintCount == 0)
        return;
      for (int index = 0; index < this.GridLayoutConstraintCount - 1; ++index)
      {
        this.dummyGridPlaceholders[index].SetActive(index < this.GridLayoutConstraintCount - num % this.GridLayoutConstraintCount);
        this.dummyGridPlaceholders[index].transform.SetAsLastSibling();
      }
    }
    else
      this.getMoreButton.transform.SetAsLastSibling();
  }

  private void RefreshTogglesForOutfit(ClothingOutfitUtility.OutfitType outfitType)
  {
    IEnumerable<ClothingOutfitTarget> clothingOutfitTargets = ClothingOutfitTarget.GetAllTemplates().Where<ClothingOutfitTarget>((Func<ClothingOutfitTarget, bool>) (outfit => outfit.OutfitType == outfitType));
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggle in this.activeFacadeToggles)
    {
      KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> toggle = activeFacadeToggle;
      if (!clothingOutfitTargets.Any<ClothingOutfitTarget>((Func<ClothingOutfitTarget, bool>) (match => match.OutfitId == toggle.Key)))
        stringList.Add(toggle.Key);
    }
    foreach (string key in stringList)
    {
      List<GameObject> pooledFacadeToggles = this.pooledFacadeToggles;
      FacadeSelectionPanel.FacadeToggle activeFacadeToggle = this.activeFacadeToggles[key];
      GameObject gameObject = activeFacadeToggle.gameObject;
      pooledFacadeToggles.Add(gameObject);
      activeFacadeToggle = this.activeFacadeToggles[key];
      activeFacadeToggle.gameObject.SetActive(false);
      this.activeFacadeToggles.Remove(key);
    }
    stringList.Clear();
    this.AddDefaultOutfitToggle();
    foreach (ClothingOutfitTarget clothingOutfitTarget in (IEnumerable<ClothingOutfitTarget>) clothingOutfitTargets.StableSort<ClothingOutfitTarget>((Comparison<ClothingOutfitTarget>) ((a, b) => a.OutfitId.CompareTo(b.OutfitId))))
    {
      if (!clothingOutfitTarget.DoesContainLockedItems())
        this.AddNewOutfitToggle(clothingOutfitTarget.OutfitId);
    }
    foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggle in this.activeFacadeToggles)
      activeFacadeToggle.Value.multiToggle.ChangeState(this.SelectedFacade == null || !(this.SelectedFacade == activeFacadeToggle.Key) ? 0 : 1);
    this.RefreshHeight();
  }

  private void RefreshTogglesForBuilding()
  {
    BuildingDef buildingDef = Assets.GetBuildingDef(this.selectedBuildingDefID);
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggle in this.activeFacadeToggles)
    {
      if (!buildingDef.AvailableFacades.Contains(activeFacadeToggle.Key))
        stringList.Add(activeFacadeToggle.Key);
    }
    foreach (string key in stringList)
    {
      List<GameObject> pooledFacadeToggles = this.pooledFacadeToggles;
      FacadeSelectionPanel.FacadeToggle activeFacadeToggle = this.activeFacadeToggles[key];
      GameObject gameObject = activeFacadeToggle.gameObject;
      pooledFacadeToggles.Add(gameObject);
      activeFacadeToggle = this.activeFacadeToggles[key];
      activeFacadeToggle.gameObject.SetActive(false);
      this.activeFacadeToggles.Remove(key);
    }
    stringList.Clear();
    this.AddDefaultBuildingFacadeToggle();
    foreach (string availableFacade in buildingDef.AvailableFacades)
    {
      PermitResource permitResource = Db.Get().Permits.TryGet(availableFacade);
      if (permitResource != null && permitResource.IsUnlocked())
        this.AddNewBuildingToggle(availableFacade);
    }
    foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggle in this.activeFacadeToggles)
      activeFacadeToggle.Value.multiToggle.ChangeState(this.SelectedFacade == activeFacadeToggle.Key ? 1 : 0);
    this.activeFacadeToggles["DEFAULT_FACADE"].gameObject.transform.SetAsFirstSibling();
    this.RefreshHeight();
  }

  private void RefreshHeight()
  {
    if (!this.usesScrollRect)
      return;
    LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
    component.minHeight = (float) (58 * Math.Clamp(Mathf.CeilToInt((float) this.activeFacadeToggles.Count / 5f), 1, 6));
    component.preferredHeight = component.minHeight;
  }

  private void AddDefaultBuildingFacadeToggle() => this.AddNewBuildingToggle("DEFAULT_FACADE");

  private void AddDefaultOutfitToggle() => this.AddNewOutfitToggle("DEFAULT_FACADE", true);

  private void AddNewBuildingToggle(string facadeID)
  {
    if (this.activeFacadeToggles.ContainsKey(facadeID))
      return;
    GameObject gameObject;
    if (this.pooledFacadeToggles.Count > 0)
    {
      gameObject = this.pooledFacadeToggles[0];
      this.pooledFacadeToggles.RemoveAt(0);
    }
    else
      gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject);
    FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(facadeID, this.selectedBuildingDefID, gameObject);
    newToggle.multiToggle.onClick += (System.Action) (() => this.SelectFacade(newToggle.id));
    this.activeFacadeToggles.Add(newToggle.id, newToggle);
  }

  private void AddNewOutfitToggle(string outfitID, bool setAsFirstSibling = false)
  {
    if (this.activeFacadeToggles.ContainsKey(outfitID))
    {
      if (!setAsFirstSibling)
        return;
      this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
    }
    else
    {
      GameObject gameObject;
      if (this.pooledFacadeToggles.Count > 0)
      {
        gameObject = this.pooledFacadeToggles[0];
        this.pooledFacadeToggles.RemoveAt(0);
      }
      else
        gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject);
      FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(outfitID, gameObject, this.selectedOutfitCategory);
      newToggle.multiToggle.onClick += (System.Action) (() => this.SelectFacade(newToggle.id));
      this.activeFacadeToggles.Add(newToggle.id, newToggle);
      if (!setAsFirstSibling)
        return;
      this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
    }
  }

  private void SelectFacade(string id) => this.SelectedFacade = id;

  private struct FacadeToggle
  {
    public FacadeToggle(string buildingFacadeID, string buildingPrefabID, GameObject gameObject)
    {
      this.id = buildingFacadeID;
      this.gameObject = gameObject;
      gameObject.SetActive(true);
      this.multiToggle = gameObject.GetComponent<MultiToggle>();
      this.multiToggle.onClick = (System.Action) null;
      HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
      component1.GetReference<UIMannequin>("Mannequin").gameObject.SetActive(false);
      component1.GetReference<Image>("FGImage").SetAlpha(1f);
      Sprite sprite;
      string message;
      string dlcId;
      if (buildingFacadeID != "DEFAULT_FACADE")
      {
        BuildingFacadeResource permit = Db.GetBuildingFacades().Get(buildingFacadeID);
        sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim((HashedString) permit.AnimFile));
        message = KleiItemsUI.GetTooltipStringFor((PermitResource) permit);
        dlcId = permit.GetDlcIdFrom();
      }
      else
      {
        GameObject prefab = Assets.GetPrefab((Tag) buildingPrefabID);
        Building component2 = prefab.GetComponent<Building>();
        StringEntry result1;
        string text = !Strings.TryGet($"STRINGS.BUILDINGS.PREFABS.{buildingPrefabID.ToUpperInvariant()}.FACADES.DEFAULT_{buildingPrefabID.ToUpperInvariant()}.NAME", out result1) ? (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) ? prefab.GetProperName() : component2.Def.Name) : (string) result1;
        StringEntry result2;
        string str = !Strings.TryGet($"STRINGS.BUILDINGS.PREFABS.{buildingPrefabID.ToUpperInvariant()}.FACADES.DEFAULT_{buildingPrefabID.ToUpperInvariant()}.DESC", out result2) ? (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) ? "" : component2.Def.Desc) : (string) result2;
        sprite = Def.GetUISprite((object) buildingPrefabID).first;
        message = $"{KleiItemsUI.WrapAsToolTipTitle(text)}\n{str}";
        dlcId = (string) null;
      }
      component1.GetReference<Image>("FGImage").sprite = sprite;
      this.gameObject.GetComponent<ToolTip>().SetSimpleTooltip(message);
      Image reference = component1.GetReference<Image>("DlcBanner");
      if (DlcManager.IsDlcId(dlcId))
      {
        reference.gameObject.SetActive(true);
        reference.sprite = Assets.GetSprite((HashedString) DlcManager.GetDlcBannerSprite(dlcId));
        reference.color = DlcManager.GetDlcBannerColor(dlcId);
      }
      else
        reference.gameObject.SetActive(false);
    }

    public FacadeToggle(
      string outfitID,
      GameObject gameObject,
      ClothingOutfitUtility.OutfitType outfitType)
    {
      this.id = outfitID;
      this.gameObject = gameObject;
      gameObject.SetActive(true);
      this.multiToggle = gameObject.GetComponent<MultiToggle>();
      this.multiToggle.onClick = (System.Action) null;
      HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
      UIMannequin reference1 = component1.GetReference<UIMannequin>("Mannequin");
      reference1.gameObject.SetActive(true);
      component1.GetReference<Image>("FGImage").SetAlpha(0.0f);
      ToolTip component2 = this.gameObject.GetComponent<ToolTip>();
      component2.SetSimpleTooltip("");
      if (outfitID != "DEFAULT_FACADE")
      {
        ClothingOutfitTarget outfit = ClothingOutfitTarget.FromTemplateId(outfitID);
        component1.GetReference<UIMannequin>("Mannequin").SetOutfit(outfit);
        component2.SetSimpleTooltip(GameUtil.ApplyBoldString(outfit.ReadName()));
      }
      else
      {
        component1.GetReference<UIMannequin>("Mannequin").ClearOutfit(outfitType);
        component2.SetSimpleTooltip(GameUtil.ApplyBoldString((string) STRINGS.UI.OUTFIT_NAME.NONE));
      }
      string dlcId = (string) null;
      if (outfitID != "DEFAULT_FACADE" && ClothingOutfitTarget.FromTemplateId(outfitID).impl is ClothingOutfitTarget.DatabaseAuthoredTemplate impl)
        dlcId = impl.resource.GetDlcIdFrom();
      Image reference2 = component1.GetReference<Image>("DlcBanner");
      if (DlcManager.IsDlcId(dlcId))
      {
        reference2.gameObject.SetActive(true);
        reference2.color = DlcManager.GetDlcBannerColor(dlcId);
      }
      else
        reference2.gameObject.SetActive(false);
      Vector2 vector2 = new Vector2(0.0f, 0.0f);
      switch (outfitType)
      {
        case ClothingOutfitUtility.OutfitType.AtmoSuit:
          vector2 = new Vector2(-16f, -16f);
          break;
        case ClothingOutfitUtility.OutfitType.JetSuit:
          vector2 = new Vector2(-32f, -24f);
          break;
      }
      reference1.rectTransform().sizeDelta = vector2;
    }

    public string id { get; set; }

    public GameObject gameObject { get; set; }

    public MultiToggle multiToggle { get; set; }
  }

  private enum ConfigType
  {
    BuildingFacade,
    MinionOutfit,
  }
}
