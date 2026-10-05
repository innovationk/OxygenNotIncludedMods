// Decompiled with JetBrains decompiler
// Type: SkillMinionWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SkillMinionWidget")]
public class SkillMinionWidget : 
  KMonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler,
  IPointerClickHandler
{
  [SerializeField]
  private SkillsScreen skillsScreen;
  [SerializeField]
  private CrewPortrait portrait;
  [SerializeField]
  private LocText masteryPoints;
  [SerializeField]
  private LocText morale;
  [SerializeField]
  private Image background;
  [SerializeField]
  private Image hat_background;
  [SerializeField]
  private Color selected_color;
  [SerializeField]
  private Color unselected_color;
  [SerializeField]
  private Color hover_color;
  [SerializeField]
  private DropDown hatDropDown;
  [SerializeField]
  private TextStyleSetting TooltipTextStyle_Header;
  [SerializeField]
  private TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;
  public ButtonSoundPlayer soundPlayer;

  public IAssignableIdentity assignableIdentity { get; private set; }

  public void SetMinon(IAssignableIdentity identity)
  {
    this.assignableIdentity = identity;
    this.portrait.SetIdentityObject(this.assignableIdentity);
    this.GetComponent<NotificationHighlightTarget>().targetKey = identity.GetSoleOwner().gameObject.GetInstanceID().ToString();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.ToggleHover(true);
    this.soundPlayer.Play(1);
  }

  public void OnPointerExit(PointerEventData eventData) => this.ToggleHover(false);

  private void ToggleHover(bool on)
  {
    if (this.skillsScreen.CurrentlySelectedMinion == this.assignableIdentity)
      return;
    this.SetColor(on ? this.hover_color : this.unselected_color);
  }

  private void SetColor(Color color)
  {
    this.background.color = color;
    if (this.assignableIdentity == null || !((UnityEngine.Object) (this.assignableIdentity as StoredMinionIdentity) != (UnityEngine.Object) null))
      return;
    this.GetComponent<CanvasGroup>().alpha = 0.6f;
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left)
      return;
    this.skillsScreen.CurrentlySelectedMinion = this.assignableIdentity;
    this.GetComponent<NotificationHighlightTarget>().View();
    KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click"));
  }

  public void Refresh()
  {
    if (this.assignableIdentity.IsNullOrDestroyed())
      return;
    this.portrait.SetIdentityObject(this.assignableIdentity);
    MinionIdentity minionIdentity;
    StoredMinionIdentity storedMinionIdentity;
    this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
    this.hatDropDown.gameObject.SetActive(true);
    string hat;
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
    {
      MinionResume component = minionIdentity.GetComponent<MinionResume>();
      int availableSkillpoints = component.AvailableSkillpoints;
      int skillPointsGained = component.TotalSkillPointsGained;
      this.masteryPoints.text = availableSkillpoints > 0 ? GameUtil.ApplyBoldString(GameUtil.ColourizeString((Color32) new Color(0.5f, 1f, 0.5f, 1f), availableSkillpoints.ToString())) : "0";
      AttributeInstance attributeInstance1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) component);
      AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) component);
      this.morale.text = $"{attributeInstance1.GetTotalValue()}/{attributeInstance2.GetTotalValue()}";
      this.RefreshToolTip(component);
      List<IListableOption> contentKeys = new List<IListableOption>();
      foreach (MinionResume.HatInfo allHat in component.GetAllHats())
        contentKeys.Add((IListableOption) new HatListable(allHat.Source, allHat.Hat));
      this.hatDropDown.Initialize((IEnumerable<IListableOption>) contentKeys, new Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, (object) minionIdentity);
      hat = string.IsNullOrEmpty(component.TargetHat) ? component.CurrentHat : component.TargetHat;
    }
    else
    {
      ToolTip component = this.GetComponent<ToolTip>();
      component.ClearMultiStringTooltip();
      component.AddMultiStringTooltip(string.Format((string) STRINGS.UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) storedMinionIdentity.GetStorageReason(), (object) storedMinionIdentity.GetProperName()), (TextStyleSetting) null);
      hat = string.IsNullOrEmpty(storedMinionIdentity.targetHat) ? storedMinionIdentity.currentHat : storedMinionIdentity.targetHat;
      this.masteryPoints.text = (string) STRINGS.UI.TABLESCREENS.NA;
      this.morale.text = (string) STRINGS.UI.TABLESCREENS.NA;
    }
    bool flag = this.skillsScreen.CurrentlySelectedMinion == this.assignableIdentity;
    if (this.skillsScreen.CurrentlySelectedMinion != null && this.assignableIdentity != null)
      flag = flag || (UnityEngine.Object) this.skillsScreen.CurrentlySelectedMinion.GetSoleOwner() == (UnityEngine.Object) this.assignableIdentity.GetSoleOwner();
    this.SetColor(flag ? this.selected_color : this.unselected_color);
    HierarchyReferences component1 = this.GetComponent<HierarchyReferences>();
    this.RefreshHat(hat);
    component1.GetReference("openButton").gameObject.SetActive((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null);
  }

  private void RefreshToolTip(MinionResume resume)
  {
    if (!((UnityEngine.Object) resume != (UnityEngine.Object) null))
      return;
    AttributeInstance attributeInstance1 = Db.Get().Attributes.QualityOfLife.Lookup((Component) resume);
    AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) resume);
    ToolTip component = this.GetComponent<ToolTip>();
    component.ClearMultiStringTooltip();
    component.AddMultiStringTooltip(this.assignableIdentity.GetProperName() + "\n\n", this.TooltipTextStyle_Header);
    component.AddMultiStringTooltip(string.Format((string) STRINGS.UI.SKILLS_SCREEN.CURRENT_MORALE, (object) attributeInstance1.GetTotalValue(), (object) attributeInstance2.GetTotalValue()), (TextStyleSetting) null);
    component.AddMultiStringTooltip($"\n{(string) STRINGS.UI.DETAILTABS.STATS.NAME}\n\n", this.TooltipTextStyle_Header);
    foreach (AttributeInstance attribute in resume.GetAttributes())
    {
      if (attribute.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill)
      {
        string str = UIConstants.ColorPrefixWhite;
        if ((double) attribute.GetTotalValue() > 0.0)
          str = UIConstants.ColorPrefixGreen;
        else if ((double) attribute.GetTotalValue() < 0.0)
          str = UIConstants.ColorPrefixRed;
        component.AddMultiStringTooltip($"    • {attribute.Name}: {str}{attribute.GetTotalValue().ToString()}{UIConstants.ColorSuffix}", (TextStyleSetting) null);
      }
    }
  }

  public void RefreshHat(string hat)
  {
    this.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((HashedString) (string.IsNullOrEmpty(hat) ? "hat_role_none" : hat));
  }

  private void OnHatDropEntryClick(IListableOption hatOption, object data)
  {
    MinionIdentity minionIdentity;
    this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out StoredMinionIdentity _);
    if ((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null)
      return;
    MinionResume component = minionIdentity.GetComponent<MinionResume>();
    if (hatOption != null)
    {
      this.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((HashedString) (hatOption as HatListable).hat);
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        string hat = (hatOption as HatListable).hat;
        component.SetHats(component.CurrentHat, hat);
        if (component.OwnsHat(hat))
          component.CreateHatChangeChore();
      }
    }
    else
    {
      this.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((HashedString) "hat_role_none");
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.SetHats(component.CurrentHat, (string) null);
        component.ApplyTargetHat();
      }
    }
    this.skillsScreen.RefreshAll();
  }

  private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
  {
    if (entry.entryData == null)
      return;
    HatListable entryData = entry.entryData as HatListable;
    entry.image.sprite = Assets.GetSprite((HashedString) entryData.hat);
  }

  private int hatDropDownSort(IListableOption a, IListableOption b, object targetData) => 0;
}
