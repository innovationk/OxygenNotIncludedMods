// Decompiled with JetBrains decompiler
// Type: ModeSelectScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ModeSelectScreen : NewGameFlowScreen
{
  [SerializeField]
  private MultiToggle nosweatButton;
  private Image nosweatButtonHeader;
  private Image nosweatButtonSelectionFrame;
  [SerializeField]
  private MultiToggle survivalButton;
  private Image survivalButtonHeader;
  private Image survivalButtonSelectionFrame;
  [SerializeField]
  private LocText descriptionArea;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KBatchedAnimController nosweatAnim;
  [SerializeField]
  private KBatchedAnimController survivalAnim;
  private static bool dataLoaded;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.LoadWorldAndClusterData();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    HierarchyReferences component1 = this.survivalButton.GetComponent<HierarchyReferences>();
    this.survivalButtonHeader = component1.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
    this.survivalButtonSelectionFrame = component1.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
    this.survivalButton.onEnter += new System.Action(this.OnHoverEnterSurvival);
    this.survivalButton.onExit += new System.Action(this.OnHoverExitSurvival);
    this.survivalButton.onClick += new System.Action(this.OnClickSurvival);
    HierarchyReferences component2 = this.nosweatButton.GetComponent<HierarchyReferences>();
    this.nosweatButtonHeader = component2.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
    this.nosweatButtonSelectionFrame = component2.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
    this.nosweatButton.onEnter += new System.Action(this.OnHoverEnterNosweat);
    this.nosweatButton.onExit += new System.Action(this.OnHoverExitNosweat);
    this.nosweatButton.onClick += new System.Action(this.OnClickNosweat);
    this.closeButton.onClick += new System.Action(((NewGameFlowScreen) this).NavigateBackward);
  }

  private void OnHoverEnterSurvival()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
    this.survivalButtonSelectionFrame.SetAlpha(1f);
    this.survivalButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.533333361f, 1f);
    this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.MODESELECTSCREEN.SURVIVAL_DESC;
  }

  private void OnHoverExitSurvival()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
    this.survivalButtonSelectionFrame.SetAlpha(0.0f);
    this.survivalButtonHeader.color = new Color(0.309803933f, 0.34117648f, 0.384313732f, 1f);
    this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
  }

  private void OnClickSurvival()
  {
    this.Deactivate();
    CustomGameSettings.Instance.SetSurvivalDefaults();
    this.NavigateForward();
  }

  private void LoadWorldAndClusterData()
  {
    if (ModeSelectScreen.dataLoaded)
      return;
    CustomGameSettings.Instance.LoadClusters();
    Global.Instance.modManager.Report(this.gameObject);
    ModeSelectScreen.dataLoaded = true;
  }

  private void OnHoverEnterNosweat()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
    this.nosweatButtonSelectionFrame.SetAlpha(1f);
    this.nosweatButtonHeader.color = new Color(0.7019608f, 0.3647059f, 0.533333361f, 1f);
    this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.MODESELECTSCREEN.NOSWEAT_DESC;
  }

  private void OnHoverExitNosweat()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
    this.nosweatButtonSelectionFrame.SetAlpha(0.0f);
    this.nosweatButtonHeader.color = new Color(0.309803933f, 0.34117648f, 0.384313732f, 1f);
    this.descriptionArea.text = (string) STRINGS.UI.FRONTEND.MODESELECTSCREEN.BLANK_DESC;
  }

  private void OnClickNosweat()
  {
    this.Deactivate();
    CustomGameSettings.Instance.SetNosweatDefaults();
    this.NavigateForward();
  }
}
