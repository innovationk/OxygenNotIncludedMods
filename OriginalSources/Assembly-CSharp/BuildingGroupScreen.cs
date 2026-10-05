// Decompiled with JetBrains decompiler
// Type: BuildingGroupScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class BuildingGroupScreen : KScreen
{
  public static BuildingGroupScreen Instance;
  public KInputTextField inputField;
  [SerializeField]
  public KButton clearButton;

  public static bool SearchIsEmpty
  {
    get
    {
      return (UnityEngine.Object) BuildingGroupScreen.Instance == (UnityEngine.Object) null || BuildingGroupScreen.Instance.inputField.text.IsNullOrWhiteSpace();
    }
  }

  public static bool IsEditing
  {
    get
    {
      return !((UnityEngine.Object) BuildingGroupScreen.Instance == (UnityEngine.Object) null) && BuildingGroupScreen.Instance.isEditing;
    }
  }

  protected override void OnPrefabInit()
  {
    BuildingGroupScreen.Instance = this;
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    KInputTextField inputField = this.inputField;
    inputField.onFocus = inputField.onFocus + (System.Action) (() =>
    {
      this.isEditing = true;
      UISounds.PlaySound(UISounds.Sound.Find);
      this.ConfigurePlanScreenForSearch();
    });
    this.inputField.onEndEdit.AddListener((UnityAction<string>) (value => this.isEditing = false));
    this.inputField.OnValueChangesPaused = (System.Action) (() =>
    {
      PlanScreen.Instance.RefreshCategoryPanelTitle();
      PlanScreen.Instance.RefreshSearch();
    });
    this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = (string) UI.BUILDMENU.SEARCH_TEXT_PLACEHOLDER;
    this.clearButton.onClick += new System.Action(this.ClearSearch);
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.ConsumeMouseScroll = true;
    this.BindTooltip();
    KInputManager.InputChange.AddListener(new UnityAction(this.BindTooltip));
  }

  protected override void OnDeactivate()
  {
    KInputManager.InputChange.RemoveListener(new UnityAction(this.BindTooltip));
    base.OnDeactivate();
  }

  private void BindTooltip()
  {
    this.inputField.GetComponent<ToolTip>().toolTip = GameUtil.ReplaceHotkeyString((string) UI.BUILDMENU.SEARCH_TOOLTIP, Action.Find);
  }

  public void ClearSearch()
  {
    this.inputField.text = "";
    this.inputField.ForceChangeValueRefresh();
  }

  private void ConfigurePlanScreenForSearch()
  {
    PlanScreen.Instance.SoftCloseRecipe();
    PlanScreen.Instance.ClearSelection();
    PlanScreen.Instance.ForceRefreshAllBuildingToggles();
    PlanScreen.Instance.ConfigurePanelSize();
  }
}
