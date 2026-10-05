// Decompiled with JetBrains decompiler
// Type: ProductInfoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class ProductInfoScreen : KScreen
{
  public TitleBar titleBar;
  public GameObject ProductDescriptionPane;
  public LocText productDescriptionText;
  public DescriptorPanel ProductRequirementsPane;
  public DescriptorPanel ProductEffectsPane;
  public DescriptorPanel RoomConstrainsPanel;
  public GameObject ProductFlavourPane;
  public LocText productFlavourText;
  public RectTransform BGPanel;
  public MaterialSelectionPanel materialSelectionPanelPrefab;
  public FacadeSelectionPanel facadeSelectionPanelPrefab;
  private Dictionary<string, GameObject> descLabels = new Dictionary<string, GameObject>();
  public MultiToggle sandboxInstantBuildToggle;
  private List<Tag> HiddenRoomConstrainTags = new List<Tag>()
  {
    RoomConstraints.ConstraintTags.KitchenRefrigerator,
    RoomConstraints.ConstraintTags.FarmStationType,
    RoomConstraints.ConstraintTags.LuxuryBedType,
    RoomConstraints.ConstraintTags.MassageTable,
    RoomConstraints.ConstraintTags.MessTable,
    RoomConstraints.ConstraintTags.NatureReserve,
    RoomConstraints.ConstraintTags.Park,
    RoomConstraints.ConstraintTags.SpiceStation,
    RoomConstraints.ConstraintTags.DeStressingBuilding,
    RoomConstraints.ConstraintTags.MachineShopType
  };
  [NonSerialized]
  public MaterialSelectionPanel materialSelectionPanel;
  [SerializeField]
  private FacadeSelectionPanel facadeSelectionPanel;
  [NonSerialized]
  public BuildingDef currentDef;
  public System.Action onElementsFullySelected;
  private bool expandedInfo = true;
  private bool configuring;

  public FacadeSelectionPanel FacadeSelectionPanel => this.facadeSelectionPanel;

  private void RefreshScreen()
  {
    if ((UnityEngine.Object) this.currentDef != (UnityEngine.Object) null)
      this.SetTitle(this.currentDef);
    else
      this.ClearProduct();
  }

  public void ClearProduct(bool deactivateTool = true)
  {
    if ((UnityEngine.Object) this.materialSelectionPanel == (UnityEngine.Object) null)
      return;
    this.currentDef = (BuildingDef) null;
    this.materialSelectionPanel.ClearMaterialToggles();
    if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) BuildTool.Instance & deactivateTool)
      BuildTool.Instance.Deactivate();
    if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) UtilityBuildTool.Instance || (UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) WireBuildTool.Instance)
      ToolMenu.Instance.ClearSelection();
    this.ClearLabels();
    this.Show(false);
  }

  public new void Awake()
  {
    base.Awake();
    this.facadeSelectionPanel = Util.KInstantiateUI<FacadeSelectionPanel>(this.facadeSelectionPanelPrefab.gameObject, this.gameObject);
    this.facadeSelectionPanel.OnFacadeSelectionChanged += new System.Action(this.OnFacadeSelectionChanged);
    this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, this.gameObject);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) BuildingGroupScreen.Instance != (UnityEngine.Object) null)
    {
      BuildingGroupScreen instance1 = BuildingGroupScreen.Instance;
      instance1.pointerEnterActions = instance1.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
      BuildingGroupScreen instance2 = BuildingGroupScreen.Instance;
      instance2.pointerExitActions = instance2.pointerExitActions + new KScreen.PointerExitActions(this.CheckMouseOver);
    }
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
    {
      PlanScreen instance3 = PlanScreen.Instance;
      instance3.pointerEnterActions = instance3.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
      PlanScreen instance4 = PlanScreen.Instance;
      instance4.pointerExitActions = instance4.pointerExitActions + new KScreen.PointerExitActions(this.CheckMouseOver);
    }
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
    {
      BuildMenu instance5 = BuildMenu.Instance;
      instance5.pointerEnterActions = instance5.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
      BuildMenu instance6 = BuildMenu.Instance;
      instance6.pointerExitActions = instance6.pointerExitActions + new KScreen.PointerExitActions(this.CheckMouseOver);
    }
    this.pointerEnterActions = this.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
    this.pointerExitActions = this.pointerExitActions + new KScreen.PointerExitActions(this.CheckMouseOver);
    this.ConsumeMouseScroll = true;
    this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
    this.sandboxInstantBuildToggle.onClick += (System.Action) (() =>
    {
      SandboxToolParameterMenu.instance.settings.InstantBuild = !SandboxToolParameterMenu.instance.settings.InstantBuild;
      this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
    });
    this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
    Game.Instance.Subscribe(-1948169901, (Action<object>) (data => this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive)));
  }

  public void ConfigureScreen(BuildingDef def)
  {
    this.ConfigureScreen(def, this.FacadeSelectionPanel.SelectedFacade);
  }

  public void ConfigureScreen(BuildingDef def, string facadeID)
  {
    this.configuring = true;
    this.currentDef = def;
    this.SetTitle(def);
    this.SetDescription(def);
    this.SetEffects(def);
    this.facadeSelectionPanel.SetBuildingDef(def.PrefabID);
    BuildingFacadeResource buildingFacadeResource = (BuildingFacadeResource) null;
    if ("DEFAULT_FACADE" != facadeID)
      buildingFacadeResource = Db.GetBuildingFacades().TryGet(facadeID);
    this.facadeSelectionPanel.SelectedFacade = buildingFacadeResource == null || !(buildingFacadeResource.PrefabID == def.PrefabID) || !buildingFacadeResource.IsUnlocked() ? "DEFAULT_FACADE" : facadeID;
    this.SetMaterials(def);
    this.configuring = false;
  }

  private void ExpandInfo(PointerEventData data) => this.ToggleExpandedInfo(true);

  private void CollapseInfo(PointerEventData data) => this.ToggleExpandedInfo(false);

  public void ToggleExpandedInfo(bool state)
  {
    this.expandedInfo = state;
    if ((UnityEngine.Object) this.ProductDescriptionPane != (UnityEngine.Object) null)
      this.ProductDescriptionPane.SetActive(this.expandedInfo);
    if ((UnityEngine.Object) this.ProductRequirementsPane != (UnityEngine.Object) null)
      this.ProductRequirementsPane.gameObject.SetActive(this.expandedInfo && this.ProductRequirementsPane.HasDescriptors());
    if ((UnityEngine.Object) this.RoomConstrainsPanel != (UnityEngine.Object) null)
      this.RoomConstrainsPanel.gameObject.SetActive(this.expandedInfo && this.RoomConstrainsPanel.HasDescriptors());
    if ((UnityEngine.Object) this.ProductEffectsPane != (UnityEngine.Object) null)
      this.ProductEffectsPane.gameObject.SetActive(this.expandedInfo && this.ProductEffectsPane.HasDescriptors());
    if ((UnityEngine.Object) this.ProductFlavourPane != (UnityEngine.Object) null)
      this.ProductFlavourPane.SetActive(this.expandedInfo);
    if (!((UnityEngine.Object) this.materialSelectionPanel != (UnityEngine.Object) null) || !(this.materialSelectionPanel.CurrentSelectedElement != (Tag) (string) null))
      return;
    this.materialSelectionPanel.ToggleShowDescriptorPanels(this.expandedInfo);
  }

  private void CheckMouseOver(PointerEventData data)
  {
    this.ToggleExpandedInfo(this.GetMouseOver || (UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null && (PlanScreen.Instance.IsScreenActive() && PlanScreen.Instance.GetMouseOver || BuildingGroupScreen.Instance.GetMouseOver) || (UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null && BuildMenu.Instance.IsScreenActive() && BuildMenu.Instance.GetMouseOver);
  }

  private void Update()
  {
    if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || !((UnityEngine.Object) this.currentDef != (UnityEngine.Object) null) || !(this.materialSelectionPanel.CurrentSelectedElement != (Tag) (string) null) || MaterialSelector.AllowInsufficientMaterialBuild() || (double) this.currentDef.Mass[0] <= (double) ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.materialSelectionPanel.CurrentSelectedElement, true))
      return;
    this.materialSelectionPanel.AutoSelectAvailableMaterial();
  }

  private void SetTitle(BuildingDef def)
  {
    this.titleBar.SetTitle(def.Name);
    bool flag = (UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null && PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.IsDefBuildable(def) || (UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete;
    this.titleBar.GetComponentInChildren<KImage>().ColorState = flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Disabled;
  }

  private void SetDescription(BuildingDef def)
  {
    if ((UnityEngine.Object) def == (UnityEngine.Object) null || (UnityEngine.Object) this.productFlavourText == (UnityEngine.Object) null)
      return;
    string str1 = "" + def.Desc;
    Dictionary<Klei.AI.Attribute, float> dictionary1 = new Dictionary<Klei.AI.Attribute, float>();
    Dictionary<Klei.AI.Attribute, float> dictionary2 = new Dictionary<Klei.AI.Attribute, float>();
    foreach (Klei.AI.Attribute attribute in def.attributes)
    {
      if (!dictionary1.ContainsKey(attribute))
        dictionary1[attribute] = 0.0f;
    }
    foreach (AttributeModifier attributeModifier in def.attributeModifiers)
    {
      float num1 = 0.0f;
      Klei.AI.Attribute key = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
      dictionary1.TryGetValue(key, out num1);
      float num2 = num1 + attributeModifier.Value;
      dictionary1[key] = num2;
    }
    if (this.materialSelectionPanel.CurrentSelectedElement != (Tag) (string) null)
    {
      Element element = ElementLoader.GetElement(this.materialSelectionPanel.CurrentSelectedElement);
      if (element != null)
      {
        foreach (AttributeModifier attributeModifier in element.attributeModifiers)
        {
          float num3 = 0.0f;
          Klei.AI.Attribute key = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
          dictionary2.TryGetValue(key, out num3);
          float num4 = num3 + attributeModifier.Value;
          dictionary2[key] = num4;
        }
      }
      else
      {
        PrefabAttributeModifiers component = Assets.TryGetPrefab(this.materialSelectionPanel.CurrentSelectedElement).GetComponent<PrefabAttributeModifiers>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (AttributeModifier descriptor in component.descriptors)
          {
            float num5 = 0.0f;
            Klei.AI.Attribute key = Db.Get().BuildingAttributes.Get(descriptor.AttributeId);
            dictionary2.TryGetValue(key, out num5);
            float num6 = num5 + descriptor.Value;
            dictionary2[key] = num6;
          }
        }
      }
    }
    if (dictionary1.Count > 0)
    {
      str1 += "\n\n";
      foreach (KeyValuePair<Klei.AI.Attribute, float> keyValuePair in dictionary1)
      {
        float num7 = 0.0f;
        dictionary1.TryGetValue(keyValuePair.Key, out num7);
        float num8 = 0.0f;
        string str2 = "";
        if (dictionary2.TryGetValue(keyValuePair.Key, out num8))
        {
          num8 = Mathf.Abs(num7 * num8);
          str2 = $"(+{num8.ToString()})";
        }
        str1 = $"{str1}\n{keyValuePair.Key.Name}: {(num7 + num8).ToString()}{str2}";
      }
    }
    this.productFlavourText.text = str1;
  }

  private void SetEffects(BuildingDef def)
  {
    if (this.productDescriptionText.text != null)
      this.productDescriptionText.text = $"{def.Effect}";
    ListPool<(ElementConverter, List<Descriptor>), ProductInfoScreen>.PooledList converterDescCache = ListPool<(ElementConverter, List<Descriptor>), ProductInfoScreen>.Allocate();
    ListPool<Descriptor, ProductInfoScreen>.PooledList nonConverterReqs = ListPool<Descriptor, ProductInfoScreen>.Allocate();
    ListPool<Descriptor, ProductInfoScreen>.PooledList nonConverterEffects = ListPool<Descriptor, ProductInfoScreen>.Allocate();
    bool hasConverterReqs;
    GameUtil.PartitionBuildingDescriptors(def.BuildingComplete, false, out List<Descriptor> _, (List<(ElementConverter, List<Descriptor>)>) converterDescCache, (List<Descriptor>) nonConverterReqs, (List<Descriptor>) nonConverterEffects, out hasConverterReqs);
    bool flag1 = nonConverterReqs.Count > 0 || converterDescCache.Count > 0;
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    if (flag1)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.OPERATIONREQUIREMENTS, (string) UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONREQUIREMENTS);
      descriptorList1.Add(descriptor);
      GameUtil.BuildPartitionedRequirements(descriptorList1, (List<Descriptor>) nonConverterReqs, (List<(ElementConverter, List<Descriptor>)>) converterDescCache, hasConverterReqs);
    }
    this.ProductRequirementsPane.gameObject.SetActive(flag1);
    this.ProductRequirementsPane.SetDescriptors((IList<Descriptor>) descriptorList1);
    bool flag2 = nonConverterEffects.Count > 0 || converterDescCache.Count > 0;
    List<Descriptor> descriptorList2 = new List<Descriptor>();
    if (flag2)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.OPERATIONEFFECTS, (string) UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONEFFECTS);
      descriptorList2.Add(descriptor);
      GameUtil.BuildPartitionedEffects(descriptorList2, (List<Descriptor>) nonConverterEffects, (List<(ElementConverter, List<Descriptor>)>) converterDescCache);
    }
    this.ProductEffectsPane.gameObject.SetActive(flag2);
    this.ProductEffectsPane.SetDescriptors((IList<Descriptor>) descriptorList2);
    nonConverterReqs.Recycle();
    nonConverterEffects.Recycle();
    converterDescCache.Recycle();
    List<Descriptor> descriptors = new List<Descriptor>();
    foreach (Tag tag in def.BuildingComplete.GetComponent<KPrefabID>().Tags)
    {
      if (RoomConstraints.ConstraintTags.AllTags.Contains(tag) && !this.HiddenRoomConstrainTags.Contains(tag))
      {
        Descriptor descriptor = new Descriptor();
        descriptor.SetupDescriptor(RoomConstraints.ConstraintTags.GetRoomConstraintLabelText(tag), (string) null);
        descriptors.Add(descriptor);
      }
    }
    if (descriptors.Count > 0)
    {
      descriptors = GameUtil.GetEffectDescriptors(descriptors);
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) CODEX.HEADERS.BUILDINGTYPE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.BUILDINGROOMREQUIREMENTCLASS);
      descriptors.Insert(0, descriptor);
      this.RoomConstrainsPanel.gameObject.SetActive(true);
    }
    else
      this.RoomConstrainsPanel.gameObject.SetActive(false);
    this.RoomConstrainsPanel.SetDescriptors((IList<Descriptor>) descriptors);
  }

  public void ClearLabels()
  {
    List<string> stringList = new List<string>((IEnumerable<string>) this.descLabels.Keys);
    if (stringList.Count <= 0)
      return;
    foreach (string key in stringList)
    {
      GameObject descLabel = this.descLabels[key];
      if ((UnityEngine.Object) descLabel != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) descLabel);
      this.descLabels.Remove(key);
    }
  }

  public void SetMaterials(BuildingDef def)
  {
    this.materialSelectionPanel.gameObject.SetActive(true);
    Recipe craftRecipe = def.CraftRecipe;
    this.materialSelectionPanel.ClearSelectActions();
    this.materialSelectionPanel.ConfigureScreen(craftRecipe, new MaterialSelectionPanel.GetBuildableStateDelegate(PlanScreen.Instance.IsDefBuildable), new MaterialSelectionPanel.GetBuildableTooltipDelegate(PlanScreen.Instance.GetTooltipForBuildable));
    this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
    this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshScreen));
    this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.onMenuMaterialChanged));
    this.materialSelectionPanel.AutoSelectAvailableMaterial();
    this.ActivateAppropriateTool(def);
  }

  private void OnFacadeSelectionChanged()
  {
    if ((UnityEngine.Object) this.currentDef == (UnityEngine.Object) null)
      return;
    this.ActivateAppropriateTool(this.currentDef);
  }

  private void onMenuMaterialChanged()
  {
    if ((UnityEngine.Object) this.currentDef == (UnityEngine.Object) null)
      return;
    this.ActivateAppropriateTool(this.currentDef);
    this.SetDescription(this.currentDef);
  }

  private void ActivateAppropriateTool(BuildingDef def)
  {
    Debug.Assert((UnityEngine.Object) def != (UnityEngine.Object) null, (object) "def was null");
    if (((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null ? (PlanScreen.Instance.IsDefBuildable(def) ? 1 : 0) : ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null ? (BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete ? 1 : 0) : 0)) != 0 && this.materialSelectionPanel.AllSelectorsSelected() && this.facadeSelectionPanel.SelectedFacade != null)
    {
      this.onElementsFullySelected.Signal();
    }
    else
    {
      if (MaterialSelector.AllowInsufficientMaterialBuild() || DebugHandler.InstantBuildMode)
        return;
      if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) BuildTool.Instance)
        BuildTool.Instance.Deactivate();
      PrebuildTool.Instance.Activate(def, PlanScreen.Instance.GetTooltipForBuildable(def));
    }
  }

  public static bool MaterialsMet(Recipe recipe)
  {
    if (recipe == null)
    {
      Debug.LogError((object) "Trying to verify the materials on a null recipe!");
      return false;
    }
    if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
    {
      Debug.LogError((object) "Trying to verify the materials on a recipe with no MaterialCategoryTags!");
      return false;
    }
    bool flag = true;
    for (int index = 0; index < recipe.Ingredients.Count; ++index)
    {
      if ((double) MaterialSelectionPanel.Filter(recipe.Ingredients[index].tag).kgAvailable < (double) recipe.Ingredients[index].amount)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public void Close()
  {
    if (this.configuring)
      return;
    this.ClearProduct();
    this.Show(false);
  }
}
