// Decompiled with JetBrains decompiler
// Type: CosmeticsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CosmeticsPanel : TargetPanel
{
  [SerializeField]
  private GameObject cosmeticSlotContainer;
  [SerializeField]
  private FacadeSelectionPanel selectionPanel;
  [SerializeField]
  private LocText nameLabel;
  [SerializeField]
  private LocText descriptionLabel;
  [SerializeField]
  private KButton editButton;
  [SerializeField]
  private UIMannequin mannequin;
  [SerializeField]
  private Image buildingIcon;
  [SerializeField]
  private Dictionary<ClothingOutfitUtility.OutfitType, GameObject> outfitCategories = new Dictionary<ClothingOutfitUtility.OutfitType, GameObject>();
  [SerializeField]
  private GameObject outfitCategoryButtonPrefab;
  [SerializeField]
  private GameObject outfitCategoryButtonContainer;
  private ClothingOutfitUtility.OutfitType selectedOutfitCategory;
  private static Dictionary<ClothingOutfitUtility.OutfitType, string> categoryIcons = new Dictionary<ClothingOutfitUtility.OutfitType, string>()
  {
    {
      ClothingOutfitUtility.OutfitType.Clothing,
      "icon_inventory_equipment"
    },
    {
      ClothingOutfitUtility.OutfitType.AtmoSuit,
      "icon_inventory_atmosuits"
    },
    {
      ClothingOutfitUtility.OutfitType.JetSuit,
      "icon_inventory_jetsuits"
    }
  };

  public override bool IsValidForTarget(GameObject target) => true;

  protected override void OnSelectTarget(GameObject target)
  {
    base.OnSelectTarget(target);
    BuildingFacade buildingFacade = this.selectedTarget.GetComponent<BuildingFacade>();
    MinionIdentity component = this.selectedTarget.GetComponent<MinionIdentity>();
    this.selectionPanel.OnFacadeSelectionChanged = (System.Action) null;
    this.outfitCategoryButtonContainer.SetActive((UnityEngine.Object) component != (UnityEngine.Object) null);
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      ClothingOutfitTarget outfitTarget = ClothingOutfitTarget.FromMinion(this.selectedOutfitCategory, component.gameObject);
      this.selectionPanel.SetOutfitTarget(outfitTarget, this.selectedOutfitCategory);
      this.selectionPanel.OnFacadeSelectionChanged += (System.Action) (() =>
      {
        if (this.selectionPanel.SelectedFacade == null || this.selectionPanel.SelectedFacade == "DEFAULT_FACADE")
          outfitTarget.WriteItems(this.selectedOutfitCategory, new string[0]);
        else
          outfitTarget.WriteItems(this.selectedOutfitCategory, ClothingOutfitTarget.FromTemplateId(this.selectionPanel.SelectedFacade).ReadItems());
        this.Refresh();
      });
    }
    else if ((UnityEngine.Object) buildingFacade != (UnityEngine.Object) null)
    {
      this.selectionPanel.SetBuildingDef(this.selectedTarget.GetComponent<Building>().Def.PrefabID, this.selectedTarget.GetComponent<BuildingFacade>().CurrentFacade);
      this.selectionPanel.OnFacadeSelectionChanged = (System.Action) null;
      this.selectionPanel.OnFacadeSelectionChanged += (System.Action) (() =>
      {
        if (this.selectionPanel.SelectedFacade == null || this.selectionPanel.SelectedFacade == "DEFAULT_FACADE" || Db.GetBuildingFacades().TryGet(this.selectionPanel.SelectedFacade).IsNullOrDestroyed())
          buildingFacade.ApplyDefaultFacade(true);
        else
          buildingFacade.ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.selectionPanel.SelectedFacade), true);
        this.Refresh();
      });
    }
    this.Refresh();
  }

  public override void OnDeselectTarget(GameObject target) => base.OnDeselectTarget(target);

  public void Refresh()
  {
    MinionIdentity component1 = this.selectedTarget.GetComponent<MinionIdentity>();
    BuildingFacade component2 = this.selectedTarget.GetComponent<BuildingFacade>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      ClothingOutfitTarget outfit = ClothingOutfitTarget.FromMinion(this.selectedOutfitCategory, this.selectedTarget);
      this.editButton.gameObject.SetActive(true);
      this.mannequin.gameObject.SetActive(true);
      this.mannequin.SetOutfit(outfit);
      Vector2 vector2 = new Vector2(0.0f, 0.0f);
      if (outfit.OutfitType == ClothingOutfitUtility.OutfitType.AtmoSuit)
        vector2 = new Vector2(-8f, -8f);
      else if (outfit.OutfitType == ClothingOutfitUtility.OutfitType.JetSuit)
        vector2 = new Vector2(-12f, -12f);
      this.mannequin.rectTransform().sizeDelta = vector2;
      this.buildingIcon.gameObject.SetActive(false);
      this.editButton.ClearOnClick();
      this.editButton.onClick += new System.Action(this.OnClickEditOutfit);
      this.nameLabel.SetText(outfit.ReadName());
      this.descriptionLabel.SetText("");
    }
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      this.editButton.gameObject.SetActive(false);
      this.mannequin.gameObject.SetActive(false);
      this.buildingIcon.gameObject.SetActive(true);
      if (component2.CurrentFacade != null && component2.CurrentFacade != "DEFAULT_FACADE" && !Db.GetBuildingFacades().TryGet(component2.CurrentFacade).IsNullOrDestroyed())
      {
        BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(component2.CurrentFacade);
        this.nameLabel.SetText(buildingFacadeResource.Name);
        this.descriptionLabel.SetText(buildingFacadeResource.Description);
        this.buildingIcon.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim((HashedString) buildingFacadeResource.AnimFile));
      }
      else
      {
        string prefabId = component2.GetComponent<Building>().Def.PrefabID;
        StringEntry result1;
        Strings.TryGet($"STRINGS.BUILDINGS.PREFABS.{prefabId.ToString().ToUpperInvariant()}.FACADES.DEFAULT_{prefabId.ToString().ToUpperInvariant()}.NAME", out result1);
        if (result1 == null)
          Strings.TryGet($"STRINGS.BUILDINGS.PREFABS.{prefabId.ToString().ToUpperInvariant()}.NAME", out result1);
        StringEntry result2;
        Strings.TryGet($"STRINGS.BUILDINGS.PREFABS.{prefabId.ToString().ToUpperInvariant()}.FACADES.DEFAULT_{prefabId.ToString().ToUpperInvariant()}.DESC", out result2);
        if (result2 == null)
          Strings.TryGet($"STRINGS.BUILDINGS.PREFABS.{prefabId.ToString().ToUpperInvariant()}.DESC", out result2);
        this.nameLabel.SetText(result1 != null ? (string) result1 : "");
        this.descriptionLabel.SetText(result2 != null ? (string) result2 : "");
        this.buildingIcon.sprite = Def.GetUISprite((object) prefabId).first;
      }
    }
    this.RefreshOutfitCategories();
    this.selectionPanel.Refresh();
  }

  public void OnClickEditOutfit()
  {
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
    MinionBrowserScreenConfig.MinionInstances((Option<GameObject>) this.selectedTarget).ApplyAndOpenScreen((System.Action) (() => AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot)), this.selectedOutfitCategory);
  }

  private void RefreshOutfitCategories()
  {
    foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, GameObject> outfitCategory in this.outfitCategories)
      Util.KDestroyGameObject(outfitCategory.Value);
    this.outfitCategories.Clear();
    string[] strArray = new string[2]
    {
      "outfit",
      "atmosuit"
    };
    Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary = new Dictionary<ClothingOutfitUtility.OutfitType, string>();
    dictionary.Add(ClothingOutfitUtility.OutfitType.Clothing, (string) STRINGS.UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_OUTFIT);
    dictionary.Add(ClothingOutfitUtility.OutfitType.AtmoSuit, (string) STRINGS.UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_ATMOSUIT);
    dictionary.Add(ClothingOutfitUtility.OutfitType.JetSuit, (string) STRINGS.UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_JETSUIT);
    for (int key = 0; key < 4; ++key)
    {
      if (key != 1)
      {
        int idx = key;
        GameObject gameObject = Util.KInstantiateUI(this.outfitCategoryButtonPrefab, this.outfitCategoryButtonContainer, true);
        this.outfitCategories.Add((ClothingOutfitUtility.OutfitType) idx, gameObject);
        HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
        component1.GetReference<LocText>("Label").SetText(dictionary[(ClothingOutfitUtility.OutfitType) key]);
        component1.GetReference<Image>("FG").sprite = Assets.GetSprite((HashedString) CosmeticsPanel.categoryIcons[(ClothingOutfitUtility.OutfitType) key]);
        MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
        component2.onClick += (System.Action) (() =>
        {
          this.selectedOutfitCategory = (ClothingOutfitUtility.OutfitType) idx;
          this.Refresh();
          this.selectionPanel.SelectedOutfitCategory = this.selectedOutfitCategory;
        });
        component2.ChangeState(this.selectedOutfitCategory == (ClothingOutfitUtility.OutfitType) idx ? 1 : 0);
      }
    }
  }
}
