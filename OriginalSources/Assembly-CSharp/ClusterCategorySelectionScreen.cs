// Decompiled with JetBrains decompiler
// Type: ClusterCategorySelectionScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ClusterCategorySelectionScreen : NewGameFlowScreen
{
  public ClusterCategorySelectionScreen.ButtonConfig vanillaStyle;
  public ClusterCategorySelectionScreen.ButtonConfig classicStyle;
  public ClusterCategorySelectionScreen.ButtonConfig spacedOutStyle;
  public ClusterCategorySelectionScreen.ButtonConfig eventStyle;
  [SerializeField]
  private LocText descriptionArea;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private RectTransform panel;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.closeButton.onClick += new System.Action(((NewGameFlowScreen) this).NavigateBackward);
    int num = 0;
    foreach (ClusterLayout clusterLayout in SettingsCache.clusterLayouts.clusterCache.Values)
    {
      if (clusterLayout.clusterCategory == ClusterLayout.ClusterCategory.Special)
        ++num;
    }
    if (num > 0)
    {
      this.eventStyle.button.gameObject.SetActive(true);
      this.eventStyle.Init(this.descriptionArea, (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_DESC, (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_TITLE);
      this.eventStyle.button.onClick += (System.Action) (() => this.OnClickOption(ClusterLayout.ClusterCategory.Special));
    }
    if (DlcManager.IsExpansion1Active())
    {
      this.classicStyle.button.gameObject.SetActive(true);
      this.classicStyle.Init(this.descriptionArea, (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_DESC, (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_TITLE);
      this.classicStyle.button.onClick += (System.Action) (() => this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutVanillaStyle));
      this.spacedOutStyle.button.gameObject.SetActive(true);
      this.spacedOutStyle.Init(this.descriptionArea, (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_DESC, (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_TITLE);
      this.spacedOutStyle.button.onClick += (System.Action) (() => this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutStyle));
      this.panel.sizeDelta = num > 0 ? new Vector2(622f, this.panel.sizeDelta.y) : new Vector2(480f, this.panel.sizeDelta.y);
    }
    else
    {
      this.vanillaStyle.button.gameObject.SetActive(true);
      this.vanillaStyle.Init(this.descriptionArea, (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_DESC, (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_TITLE);
      this.vanillaStyle.button.onClick += (System.Action) (() => this.OnClickOption(ClusterLayout.ClusterCategory.Vanilla));
      this.panel.sizeDelta = new Vector2(480f, this.panel.sizeDelta.y);
      this.eventStyle.kanim.Play((HashedString) "lab_asteroid_standard");
    }
  }

  private void OnClickOption(ClusterLayout.ClusterCategory clusterCategory)
  {
    this.Deactivate();
    DestinationSelectPanel.ChosenClusterCategorySetting = (int) clusterCategory;
    this.NavigateForward();
  }

  [Serializable]
  public class ButtonConfig
  {
    public MultiToggle button;
    public Image headerImage;
    public LocText headerLabel;
    public Image selectionFrame;
    public KAnimControllerBase kanim;
    private string hoverDescriptionText;
    private LocText descriptionArea;

    public void Init(LocText descriptionArea, string hoverDescriptionText, string headerText)
    {
      this.descriptionArea = descriptionArea;
      this.hoverDescriptionText = hoverDescriptionText;
      this.headerLabel.SetText(headerText);
      this.button.onEnter += new System.Action(this.OnHoverEnter);
      this.button.onExit += new System.Action(this.OnHoverExit);
      HierarchyReferences component = this.button.GetComponent<HierarchyReferences>();
      this.headerImage = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
      this.selectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
    }

    private void OnHoverEnter()
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
      this.selectionFrame.SetAlpha(1f);
      this.headerImage.color = new Color(0.7019608f, 0.3647059f, 0.533333361f, 1f);
      this.descriptionArea.text = this.hoverDescriptionText;
    }

    private void OnHoverExit()
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
      this.selectionFrame.SetAlpha(0.0f);
      this.headerImage.color = new Color(0.309803933f, 0.34117648f, 0.384313732f, 1f);
      this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.BLANK_DESC;
    }
  }
}
