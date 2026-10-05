// Decompiled with JetBrains decompiler
// Type: TelepadSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class TelepadSideScreen : SideScreenContent
{
  [SerializeField]
  private KButton viewImmigrantsBtn;
  [SerializeField]
  private LocText viewImmigrantsBtnText;
  [SerializeField]
  private GameObject immigrantBtnAttentionIcon;
  [SerializeField]
  private Telepad targetTelepad;
  [SerializeField]
  private KButton viewColonySummaryBtn;
  [SerializeField]
  private Image newAchievementsEarned;
  [SerializeField]
  private KButton openRolesScreenButton;
  [SerializeField]
  private Image skillPointsAvailable;
  [SerializeField]
  private KButton viewResearchBtn;
  [SerializeField]
  private GameObject researchAttentionIcon;
  [SerializeField]
  private GameObject victoryConditionsContainer;
  [SerializeField]
  private GameObject conditionContainerTemplate;
  [SerializeField]
  private GameObject checkboxLinePrefab;
  private Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>> entries = new Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>>();
  private Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> victoryAchievementWidgets = new Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.viewImmigrantsBtn.onClick += (System.Action) (() =>
    {
      ImmigrantScreen.InitializeImmigrantScreen(this.targetTelepad);
      Game.Instance.Trigger(288942073, (object) null);
    });
    this.viewColonySummaryBtn.onClick += (System.Action) (() =>
    {
      this.newAchievementsEarned.gameObject.SetActive(false);
      MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
    });
    this.openRolesScreenButton.onClick += (System.Action) (() => ManagementMenu.Instance.ToggleSkills());
    this.viewResearchBtn.onClick += (System.Action) (() => ManagementMenu.Instance.ToggleResearch());
    this.researchAttentionIcon.gameObject.SetActive(ManagementMenu.Instance.HasActiveResearch);
    this.BuildVictoryConditions();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<Telepad>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    Telepad component = target.GetComponent<Telepad>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Target doesn't have a telepad associated with it.");
    }
    else
    {
      this.targetTelepad = component;
      if ((UnityEngine.Object) this.targetTelepad != (UnityEngine.Object) null)
        this.gameObject.SetActive(false);
      else
        this.gameObject.SetActive(true);
    }
  }

  private void Update()
  {
    if (!((UnityEngine.Object) this.targetTelepad != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) GameFlowManager.Instance != (UnityEngine.Object) null && GameFlowManager.Instance.IsGameOver())
    {
      this.gameObject.SetActive(false);
      this.viewImmigrantsBtnText.text = (string) STRINGS.UI.UISIDESCREENS.TELEPADSIDESCREEN.GAMEOVER;
      this.SetContentState(true);
    }
    else
    {
      if (this.targetTelepad.GetComponent<Operational>().IsOperational)
        this.viewImmigrantsBtnText.text = this.TimeRemainingString;
      else
        this.gameObject.SetActive(false);
      this.SetContentState(!Immigration.Instance.ImmigrantsAvailable);
    }
    this.UpdateVictoryConditions();
    this.UpdateAchievementsUnlocked();
    this.UpdateSkills();
  }

  private string TimeRemainingString
  {
    get
    {
      return string.Format((string) STRINGS.UI.UISIDESCREENS.TELEPADSIDESCREEN.NEXTPRODUCTION, (object) GameUtil.GetFormattedCycles(this.targetTelepad.GetTimeRemaining()));
    }
  }

  private void SetContentState(bool isLabel)
  {
    this.viewResearchBtn.isInteractable = ManagementMenu.Instance.CheckHasResearchCenter();
    this.viewImmigrantsBtn.isInteractable = !isLabel;
    this.viewImmigrantsBtnText.text = isLabel ? this.TimeRemainingString : (string) STRINGS.UI.VIEWDUPLICANTS;
    this.immigrantBtnAttentionIcon.SetActive(!isLabel);
  }

  private void BuildVictoryConditions()
  {
    foreach (ColonyAchievement resource in Db.Get().ColonyAchievements.resources)
    {
      if (resource.isVictoryCondition && !resource.Disabled && resource.IsValidForSave())
      {
        Dictionary<ColonyAchievementRequirement, GameObject> dictionary = new Dictionary<ColonyAchievementRequirement, GameObject>();
        this.victoryAchievementWidgets.Add(resource, dictionary);
        GameObject parent = Util.KInstantiateUI(this.conditionContainerTemplate, this.victoryConditionsContainer, true);
        parent.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(resource.Name);
        foreach (ColonyAchievementRequirement key in resource.requirementChecklist)
        {
          if (key is VictoryColonyAchievementRequirement achievementRequirement)
          {
            GameObject gameObject = Util.KInstantiateUI(this.checkboxLinePrefab, parent, true);
            gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(achievementRequirement.Name());
            gameObject.GetComponent<ToolTip>().SetSimpleTooltip(achievementRequirement.Description());
            dictionary.Add(key, gameObject);
          }
          else
            Debug.LogWarning((object) $"Colony achievement {key.GetType().ToString()} is not a victory requirement but it is attached to a victory achievement {resource.Name}.");
        }
        this.entries.Add(resource.Id, dictionary);
      }
    }
  }

  private void UpdateVictoryConditions()
  {
    foreach (ColonyAchievement resource in Db.Get().ColonyAchievements.resources)
    {
      if (resource.isVictoryCondition && !resource.Disabled && resource.IsValidForSave())
      {
        foreach (ColonyAchievementRequirement key in resource.requirementChecklist)
        {
          this.entries[resource.Id][key].GetComponent<HierarchyReferences>().GetReference<Image>("Check").enabled = key.Success();
          if (key is VictoryColonyAchievementRequirement achievementRequirement && achievementRequirement.shouldUpdateNameAndDescription)
          {
            this.entries[resource.Id][key].GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(achievementRequirement.Name());
            this.entries[resource.Id][key].GetComponent<ToolTip>().SetSimpleTooltip(achievementRequirement.Description());
          }
        }
      }
    }
    foreach (KeyValuePair<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> achievementWidget in this.victoryAchievementWidgets)
    {
      foreach (KeyValuePair<ColonyAchievementRequirement, GameObject> keyValuePair in achievementWidget.Value)
        keyValuePair.Value.GetComponent<ToolTip>().SetSimpleTooltip(keyValuePair.Key.GetProgress(keyValuePair.Key.Success()));
    }
  }

  private void UpdateAchievementsUnlocked()
  {
    if (SaveGame.Instance.ColonyAchievementTracker.achievementsToDisplay.Count <= 0)
      return;
    this.newAchievementsEarned.gameObject.SetActive(true);
  }

  private void UpdateSkills()
  {
    bool flag = false;
    foreach (MinionResume minionResume in Components.MinionResumes)
    {
      if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
      {
        flag = true;
        break;
      }
    }
    this.skillPointsAvailable.gameObject.SetActive(flag);
  }
}
