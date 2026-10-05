// Decompiled with JetBrains decompiler
// Type: DetailsScreenMaterialPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DetailsScreenMaterialPanel : TargetScreen
{
  [Header("Current Material")]
  [SerializeField]
  private Image currentMaterialIcon;
  [SerializeField]
  private RectTransform currentMaterialDescriptionRow;
  [SerializeField]
  private LocText currentMaterialLabel;
  [SerializeField]
  private LocText currentMaterialDescription;
  [SerializeField]
  private DescriptorPanel descriptorPanel;
  [Header("Change Material")]
  [SerializeField]
  private MaterialSelectionPanel materialSelectionPanel;
  [SerializeField]
  private RectTransform confirmChangeRow;
  [SerializeField]
  private KButton orderChangeMaterialButton;
  [SerializeField]
  private KButton openChangeMaterialPanelButton;
  private int subHandle = -1;
  private Building building;

  public override bool IsValidForTarget(GameObject target) => true;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.openChangeMaterialPanelButton.onClick += (System.Action) (() =>
    {
      this.OpenMaterialSelectionPanel();
      this.RefreshMaterialSelectionPanel();
      this.RefreshOrderChangeMaterialButton();
    });
  }

  public override void SetTarget(GameObject target)
  {
    if ((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null)
      this.selectedTarget.Unsubscribe(this.subHandle);
    this.building = (Building) null;
    base.SetTarget(target);
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      return;
    this.materialSelectionPanel.gameObject.SetActive(false);
    this.orderChangeMaterialButton.ClearOnClick();
    this.orderChangeMaterialButton.isInteractable = false;
    this.CloseMaterialSelectionPanel();
    this.building = this.selectedTarget.GetComponent<Building>();
    bool flag = Db.Get().TechItems.IsTechItemComplete(this.building.Def.PrefabID) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
    this.openChangeMaterialPanelButton.isInteractable = ((!((UnityEngine.Object) target.GetComponent<Reconstructable>() != (UnityEngine.Object) null) ? 0 : (target.GetComponent<Reconstructable>().AllowReconstruct ? 1 : 0)) & (flag ? 1 : 0)) != 0;
    this.openChangeMaterialPanelButton.GetComponent<ToolTip>().SetSimpleTooltip(flag ? "" : string.Format((string) STRINGS.UI.PRODUCTINFO_REQUIRESRESEARCHDESC, (object) Db.Get().TechItems.GetTechFromItemID(this.building.Def.PrefabID).Name));
    this.Refresh();
    this.subHandle = target.Subscribe(954267658, new Action<object>(this.RefreshOrderChangeMaterialButton));
    Game.Instance.Subscribe(1980521255, new Action<object>(this.Refresh));
  }

  private void OpenMaterialSelectionPanel()
  {
    this.openChangeMaterialPanelButton.gameObject.SetActive(false);
    this.materialSelectionPanel.gameObject.SetActive(true);
    this.RefreshMaterialSelectionPanel();
    if (!((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null) || !((UnityEngine.Object) this.building != (UnityEngine.Object) null))
      return;
    this.materialSelectionPanel.SelectSourcesMaterials(this.building);
  }

  private void CloseMaterialSelectionPanel()
  {
    this.currentMaterialDescriptionRow.gameObject.SetActive(true);
    this.openChangeMaterialPanelButton.gameObject.SetActive(true);
    this.materialSelectionPanel.gameObject.SetActive(false);
  }

  public override void OnDeselectTarget(GameObject target)
  {
    base.OnDeselectTarget(target);
    this.Refresh();
  }

  private void Refresh(object data = null)
  {
    this.RefreshCurrentMaterial();
    this.RefreshMaterialSelectionPanel();
  }

  private void RefreshCurrentMaterial()
  {
    if ((UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null)
      return;
    CellSelectionObject component = this.selectedTarget.GetComponent<CellSelectionObject>();
    Element element = (UnityEngine.Object) component == (UnityEngine.Object) null ? this.selectedTarget.GetComponent<PrimaryElement>().Element : component.element;
    Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) element);
    this.currentMaterialIcon.sprite = uiSprite.first;
    this.currentMaterialIcon.color = uiSprite.second;
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      this.currentMaterialLabel.SetText($"{element.name} x {GameUtil.GetFormattedMass(this.selectedTarget.GetComponent<PrimaryElement>().Mass)}");
    else
      this.currentMaterialLabel.SetText(element.name);
    this.currentMaterialDescription.SetText(element.description);
    List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
    if (materialDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, (string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER);
      materialDescriptors.Insert(0, descriptor);
      this.descriptorPanel.gameObject.SetActive(true);
      this.descriptorPanel.SetDescriptors((IList<Descriptor>) materialDescriptors);
    }
    else
      this.descriptorPanel.gameObject.SetActive(false);
  }

  private void RefreshMaterialSelectionPanel()
  {
    if ((UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null)
      return;
    this.materialSelectionPanel.ClearSelectActions();
    if (!((UnityEngine.Object) this.building == (UnityEngine.Object) null) && !(this.building is BuildingUnderConstruction))
    {
      this.materialSelectionPanel.ConfigureScreen(this.building.Def.CraftRecipe, (MaterialSelectionPanel.GetBuildableStateDelegate) (data => true), (MaterialSelectionPanel.GetBuildableTooltipDelegate) (data => ""));
      this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshOrderChangeMaterialButton));
      Reconstructable component = this.selectedTarget.GetComponent<Reconstructable>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.ReconstructRequested)
      {
        if (!this.materialSelectionPanel.gameObject.activeSelf)
        {
          this.OpenMaterialSelectionPanel();
          this.materialSelectionPanel.RefreshSelectors();
        }
        this.materialSelectionPanel.ForceSelectPrimaryTag(component.PrimarySelectedElementTag);
      }
    }
    this.confirmChangeRow.transform.SetAsLastSibling();
  }

  private void RefreshOrderChangeMaterialButton()
  {
    this.RefreshOrderChangeMaterialButton((object) null);
  }

  private void RefreshOrderChangeMaterialButton(object data = null)
  {
    if ((UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null)
      return;
    Reconstructable reconstructable = this.selectedTarget.GetComponent<Reconstructable>();
    this.orderChangeMaterialButton.isInteractable = this.materialSelectionPanel.CurrentSelectedElement != (Tag) (string) null && this.building.GetComponent<PrimaryElement>().Element.tag != this.materialSelectionPanel.CurrentSelectedElement;
    this.orderChangeMaterialButton.ClearOnClick();
    this.orderChangeMaterialButton.onClick += (System.Action) (() =>
    {
      reconstructable.RequestReconstruct(this.materialSelectionPanel.CurrentSelectedElement);
      this.RefreshOrderChangeMaterialButton();
    });
    this.orderChangeMaterialButton.GetComponentInChildren<LocText>().SetText((string) (reconstructable.ReconstructRequested ? STRINGS.UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT : STRINGS.UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT));
    this.orderChangeMaterialButton.GetComponent<ToolTip>().SetSimpleTooltip((string) (reconstructable.ReconstructRequested ? STRINGS.UI.USERMENUACTIONS.RECONSTRUCT.CANCEL_RECONSTRUCT_TOOLTIP : STRINGS.UI.USERMENUACTIONS.RECONSTRUCT.REQUEST_RECONSTRUCT_TOOLTIP));
  }
}
