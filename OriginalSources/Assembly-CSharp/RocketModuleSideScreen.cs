// Decompiled with JetBrains decompiler
// Type: RocketModuleSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RocketModuleSideScreen : SideScreenContent
{
  public static RocketModuleSideScreen instance;
  private ReorderableBuilding reorderable;
  public KScreen changeModuleSideScreen;
  public Image moduleIcon;
  [Header("Buttons")]
  public KButton addNewModuleButton;
  public KButton removeModuleButton;
  public KButton changeModuleButton;
  public KButton moveModuleUpButton;
  public KButton moveModuleDownButton;
  [Header("Labels")]
  public LocText removeButtonLabel;
  public LocText moduleNameLabel;
  public LocText moduleDescriptionLabel;
  public TextStyleSetting nameSetting;
  public TextStyleSetting descriptionSetting;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    RocketModuleSideScreen.instance = this;
  }

  protected override void OnForcedCleanUp()
  {
    RocketModuleSideScreen.instance = (RocketModuleSideScreen) null;
    base.OnForcedCleanUp();
  }

  public override int GetSideScreenSortOrder() => 104;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.addNewModuleButton.onClick += (System.Action) (() =>
    {
      Vector2 vector2 = Vector2.zero;
      if ((UnityEngine.Object) SelectModuleSideScreen.Instance != (UnityEngine.Object) null)
        vector2 = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
      this.ClickAddNew(vector2.y);
    });
    this.removeModuleButton.onClick += new System.Action(this.ClickRemove);
    this.moveModuleUpButton.onClick += new System.Action(this.ClickSwapUp);
    this.moveModuleDownButton.onClick += new System.Action(this.ClickSwapDown);
    this.changeModuleButton.onClick += (System.Action) (() =>
    {
      Vector2 vector2 = Vector2.zero;
      if ((UnityEngine.Object) SelectModuleSideScreen.Instance != (UnityEngine.Object) null)
        vector2 = SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.rectTransform().anchoredPosition;
      this.ClickChangeModule(vector2.y);
    });
    this.moduleNameLabel.textStyleSetting = this.nameSetting;
    this.moduleDescriptionLabel.textStyleSetting = this.descriptionSetting;
    this.moduleNameLabel.ApplySettings();
    this.moduleDescriptionLabel.ApplySettings();
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    DetailsScreen.Instance.ClearSecondarySideScreen();
  }

  protected override void OnCleanUp() => base.OnCleanUp();

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<ReorderableBuilding>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.reorderable = new_target.GetComponent<ReorderableBuilding>();
      this.moduleIcon.sprite = Def.GetUISprite((object) this.reorderable.gameObject).first;
      this.moduleNameLabel.SetText(this.reorderable.GetProperName());
      this.moduleDescriptionLabel.SetText(this.reorderable.GetComponent<Building>().Desc);
      this.UpdateButtonStates();
    }
  }

  public void UpdateButtonStates()
  {
    this.changeModuleButton.isInteractable = this.reorderable.CanChangeModule();
    this.changeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.changeModuleButton.isInteractable ? STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.DESC.text : STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONCHANGEMODULE.INVALID.text);
    this.addNewModuleButton.isInteractable = true;
    this.addNewModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.ADDMODULE.DESC.text);
    Deconstructable component = this.reorderable.GetComponent<Deconstructable>();
    bool flag = (UnityEngine.Object) component != (UnityEngine.Object) null && component.IsMarkedForDeconstruction();
    this.removeModuleButton.isInteractable = (UnityEngine.Object) component != (UnityEngine.Object) null && this.reorderable.CanRemoveModule();
    this.removeModuleButton.GetComponent<ToolTip>().SetSimpleTooltip(this.removeModuleButton.isInteractable ? (flag ? STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.DESC_CANCEL.text : STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.DESC.text) : STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.INVALID.text);
    this.removeButtonLabel.SetText((string) (flag ? STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.LABEL_CANCEL : STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONREMOVEMODULE.LABEL));
    this.moveModuleDownButton.isInteractable = this.reorderable.CanSwapDown();
    this.moveModuleDownButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleDownButton.isInteractable ? STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.DESC.text : STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEDOWN.INVALID.text);
    this.moveModuleUpButton.isInteractable = this.reorderable.CanSwapUp();
    this.moveModuleUpButton.GetComponent<ToolTip>().SetSimpleTooltip(this.moveModuleUpButton.isInteractable ? STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.DESC.text : STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONSWAPMODULEUP.INVALID.text);
  }

  public void ClickAddNew(float scrollViewPosition, BuildingDef autoSelectDef = null)
  {
    SelectModuleSideScreen moduleSideScreen = (SelectModuleSideScreen) DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, (string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
    moduleSideScreen.addingNewModule = true;
    moduleSideScreen.SetTarget(this.reorderable.gameObject);
    if ((UnityEngine.Object) autoSelectDef != (UnityEngine.Object) null)
      moduleSideScreen.SelectModule(autoSelectDef);
    this.ScrollToTargetPoint(scrollViewPosition);
  }

  private void ScrollToTargetPoint(float scrollViewPosition)
  {
    if (!((UnityEngine.Object) SelectModuleSideScreen.Instance != (UnityEngine.Object) null))
      return;
    SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0.0f, scrollViewPosition);
    if (!this.gameObject.activeInHierarchy)
      return;
    this.StartCoroutine(this.DelayedScrollToTargetPoint(scrollViewPosition));
  }

  private IEnumerator DelayedScrollToTargetPoint(float scrollViewPosition)
  {
    if ((UnityEngine.Object) SelectModuleSideScreen.Instance != (UnityEngine.Object) null)
    {
      yield return (object) SequenceUtil.WaitForEndOfFrame;
      SelectModuleSideScreen.Instance.mainContents.GetComponent<KScrollRect>().content.anchoredPosition = new Vector2(0.0f, scrollViewPosition);
    }
  }

  private void ClickRemove()
  {
    Deconstructable component = this.reorderable.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    if (component.IsMarkedForDeconstruction())
      component.CancelDeconstruction();
    else
      this.reorderable.Trigger(-790448070, (object) null);
    this.UpdateButtonStates();
    component.Trigger(1980521255, (object) null);
  }

  private void ClickSwapUp()
  {
    this.reorderable.SwapWithAbove();
    this.UpdateButtonStates();
  }

  private void ClickSwapDown()
  {
    this.reorderable.SwapWithBelow();
    this.UpdateButtonStates();
  }

  private void ClickChangeModule(float scrollViewPosition)
  {
    SelectModuleSideScreen moduleSideScreen = (SelectModuleSideScreen) DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, (string) STRINGS.UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL);
    moduleSideScreen.addingNewModule = false;
    moduleSideScreen.SetTarget(this.reorderable.gameObject);
    this.ScrollToTargetPoint(scrollViewPosition);
  }
}
