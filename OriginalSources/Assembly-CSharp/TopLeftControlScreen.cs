// Decompiled with JetBrains decompiler
// Type: TopLeftControlScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class TopLeftControlScreen : KScreen
{
  public static TopLeftControlScreen Instance;
  [SerializeField]
  private MultiToggle sandboxToggle;
  [SerializeField]
  private MultiToggle kleiItemDropButton;
  [SerializeField]
  private LocText locText;
  [SerializeField]
  private RectTransform secondaryRow;

  public static void DestroyInstance()
  {
    TopLeftControlScreen.Instance = (TopLeftControlScreen) null;
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    TopLeftControlScreen.Instance = this;
    this.RefreshName();
    KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
    this.UpdateSandboxToggleState();
    this.sandboxToggle.onClick += new System.Action(this.OnClickSandboxToggle);
    this.kleiItemDropButton.onClick += new System.Action(this.OnClickKleiItemDropButton);
    KleiItemsStatusRefresher.AddOrGetListener((Component) this).OnRefreshUI(new System.Action(this.RefreshKleiItemDropButton));
    this.RefreshKleiItemDropButton();
    Game.Instance.Subscribe(-1948169901, (Action<object>) (data => this.UpdateSandboxToggleState()));
    LayoutRebuilder.ForceRebuildLayoutImmediate(this.secondaryRow);
  }

  protected override void OnForcedCleanUp()
  {
    KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
    base.OnForcedCleanUp();
  }

  public void RefreshName()
  {
    if (!((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null))
      return;
    this.locText.text = SaveGame.Instance.BaseName;
  }

  public void ResetToolTip()
  {
    if (this.CheckSandboxModeLocked())
      this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, Action.ToggleSandboxTools));
    else
      this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, Action.ToggleSandboxTools));
  }

  public void UpdateSandboxToggleState()
  {
    if (this.CheckSandboxModeLocked())
    {
      this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, Action.ToggleSandboxTools));
      this.sandboxToggle.ChangeState(0);
    }
    else
    {
      this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) STRINGS.UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, Action.ToggleSandboxTools));
      this.sandboxToggle.ChangeState(Game.Instance.SandboxModeActive ? 2 : 1);
    }
    this.sandboxToggle.gameObject.SetActive(SaveGame.Instance.sandboxEnabled);
  }

  private void OnClickSandboxToggle()
  {
    if (this.CheckSandboxModeLocked())
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
    }
    else
    {
      Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
      KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On") : GlobalAssets.GetSound("SandboxTool_Toggle_Off"));
    }
    this.UpdateSandboxToggleState();
  }

  private void RefreshKleiItemDropButton()
  {
    if (!KleiItemDropScreen.HasItemsToShow())
    {
      this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ERROR_NO_ITEMS);
      this.kleiItemDropButton.ChangeState(1);
    }
    else
    {
      this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip((string) STRINGS.UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ITEMS_AVAILABLE);
      this.kleiItemDropButton.ChangeState(2);
    }
  }

  private void OnClickKleiItemDropButton()
  {
    this.RefreshKleiItemDropButton();
    if (!KleiItemDropScreen.HasItemsToShow())
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
    }
    else
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
      UnityEngine.Object.FindFirstObjectByType<KleiItemDropScreen>(FindObjectsInactive.Include).Show(true);
    }
  }

  private bool CheckSandboxModeLocked() => !SaveGame.Instance.sandboxEnabled;

  private enum MultiToggleState
  {
    Disabled,
    Off,
    On,
  }
}
