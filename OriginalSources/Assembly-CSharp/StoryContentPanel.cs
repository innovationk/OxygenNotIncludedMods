// Decompiled with JetBrains decompiler
// Type: StoryContentPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.CustomSettings;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class StoryContentPanel : KMonoBehaviour
{
  [SerializeField]
  private GameObject storyRowPrefab;
  [SerializeField]
  private GameObject storyRowContainer;
  private Dictionary<string, GameObject> storyRows = new Dictionary<string, GameObject>();
  public const int DEFAULT_RANDOMIZE_STORY_COUNT = 5;
  private Dictionary<string, StoryContentPanel.StoryState> storyStates = new Dictionary<string, StoryContentPanel.StoryState>();
  private string selectedStoryId = "";
  [SerializeField]
  private ColonyDestinationSelectScreen mainScreen;
  [Header("Trait Count")]
  [Header("SelectedStory")]
  [SerializeField]
  private Image selectedStoryImage;
  [SerializeField]
  private LocText selectedStoryTitleLabel;
  [SerializeField]
  private LocText selectedStoryDescriptionLabel;
  [SerializeField]
  private Sprite spriteForbidden;
  [SerializeField]
  private Sprite spritePossible;
  [SerializeField]
  private Sprite spriteGuaranteed;
  private StoryContentPanel.StoryState _defaultStoryState;
  private List<string> storyTraitSettings = new List<string>()
  {
    "None",
    "Few",
    "Lots"
  };

  public List<string> GetActiveStories()
  {
    List<string> activeStories = new List<string>();
    foreach (KeyValuePair<string, StoryContentPanel.StoryState> storyState in this.storyStates)
    {
      if (storyState.Value == StoryContentPanel.StoryState.Guaranteed)
        activeStories.Add(storyState.Key);
    }
    return activeStories;
  }

  public void Init()
  {
    this.SpawnRows();
    this.RefreshRows();
    this.RefreshDescriptionPanel();
    this.SelectDefault();
    CustomGameSettings.Instance.OnStorySettingChanged += new Action<SettingConfig, SettingLevel>(this.OnStorySettingChanged);
  }

  public void Cleanup()
  {
    CustomGameSettings.Instance.OnStorySettingChanged -= new Action<SettingConfig, SettingLevel>(this.OnStorySettingChanged);
  }

  private void OnStorySettingChanged(SettingConfig config, SettingLevel level)
  {
    this.storyStates[config.id] = level.id == "Guaranteed" ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden;
    this.RefreshStoryDisplay(config.id);
  }

  private void SpawnRows()
  {
    foreach (Story resource in Db.Get().Stories.resources)
    {
      Story story = resource;
      GameObject gameObject = Util.KInstantiateUI(this.storyRowPrefab, this.storyRowContainer, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("Label").SetText((string) Strings.Get(story.StoryTrait.name));
      gameObject.GetComponent<MultiToggle>().onClick += (System.Action) (() => this.SelectRow(story.Id));
      this.storyRows.Add(story.Id, gameObject);
      component.GetReference<Image>("Icon").sprite = Assets.GetSprite((HashedString) story.StoryTrait.icon);
      component.GetReference<MultiToggle>("checkbox").onClick += (System.Action) (() =>
      {
        this.IncrementStorySetting(story.Id);
        this.RefreshStoryDisplay(story.Id);
      });
      this.storyStates.Add(story.Id, this._defaultStoryState);
    }
    this.RefreshAllStoryStates();
    this.mainScreen.RefreshStoryLabel();
  }

  private void SelectRow(string id)
  {
    this.selectedStoryId = id;
    this.RefreshRows();
    this.RefreshDescriptionPanel();
  }

  public void SelectDefault()
  {
    foreach (KeyValuePair<string, StoryContentPanel.StoryState> storyState in this.storyStates)
    {
      if (storyState.Value == StoryContentPanel.StoryState.Guaranteed)
      {
        this.SelectRow(storyState.Key);
        return;
      }
    }
    using (Dictionary<string, StoryContentPanel.StoryState>.Enumerator enumerator = this.storyStates.GetEnumerator())
    {
      if (!enumerator.MoveNext())
        return;
      this.SelectRow(enumerator.Current.Key);
    }
  }

  private void IncrementStorySetting(string storyId, bool forward = true)
  {
    int num = (int) (this.storyStates[storyId] + (forward ? 1 : -1));
    if (num < 0)
      num += 2;
    int state = num % 2;
    this.SetStoryState(storyId, (StoryContentPanel.StoryState) state);
    this.mainScreen.RefreshRowsAndDescriptions();
  }

  private void SetStoryState(string storyId, StoryContentPanel.StoryState state)
  {
    this.storyStates[storyId] = state;
    CustomGameSettings.Instance.SetStorySetting(CustomGameSettings.Instance.StorySettings[storyId], this.storyStates[storyId] == StoryContentPanel.StoryState.Guaranteed);
  }

  public void SelectRandomStories(int min = 5, int max = 5, bool useBias = false)
  {
    int num1 = UnityEngine.Random.Range(min, max);
    List<Story> list1 = new List<Story>((IEnumerable<Story>) Db.Get().Stories.resources);
    List<Story> source = new List<Story>();
    list1.Shuffle<Story>();
    for (int index = 0; index < num1 && list1.Count - 1 >= index; ++index)
      source.Add(list1[index]);
    float num2 = 0.7f;
    int num3 = source.Count<Story>((Func<Story, bool>) (x => x.IsNew()));
    if (useBias && num3 == 0 && (double) UnityEngine.Random.value < (double) num2)
    {
      List<Story> list2 = Db.Get().Stories.resources.Where<Story>((Func<Story, bool>) (x => x.IsNew())).ToList<Story>();
      list2.Shuffle<Story>();
      if (list2.Count > 0)
      {
        source.RemoveAt(0);
        source.Add(list2[0]);
      }
    }
    if (!source.Contains(Db.Get().Stories.HijackedHeadquarters))
    {
      source.RemoveAt(0);
      source.Add(Db.Get().Stories.HijackedHeadquarters);
    }
    foreach (Story story in list1)
      this.SetStoryState(story.Id, source.Contains(story) ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
    this.RefreshAllStoryStates();
    this.mainScreen.RefreshRowsAndDescriptions();
  }

  private void RefreshAllStoryStates()
  {
    foreach (string key in this.storyRows.Keys)
      this.RefreshStoryDisplay(key);
  }

  private void RefreshStoryDisplay(string id)
  {
    MultiToggle reference = this.storyRows[id].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("checkbox");
    switch (this.storyStates[id])
    {
      case StoryContentPanel.StoryState.Forbidden:
        reference.ChangeState(0);
        break;
      case StoryContentPanel.StoryState.Guaranteed:
        reference.ChangeState(1);
        break;
    }
  }

  private void RefreshRows()
  {
    foreach (KeyValuePair<string, GameObject> storyRow in this.storyRows)
      storyRow.Value.GetComponent<MultiToggle>().ChangeState(storyRow.Key == this.selectedStoryId ? 1 : 0);
  }

  private void RefreshDescriptionPanel()
  {
    if (this.selectedStoryId.IsNullOrWhiteSpace())
    {
      this.selectedStoryTitleLabel.SetText("");
      this.selectedStoryDescriptionLabel.SetText("");
    }
    else
    {
      WorldTrait storyTrait = Db.Get().Stories.GetStoryTrait(this.selectedStoryId, true);
      this.selectedStoryTitleLabel.SetText((string) Strings.Get(storyTrait.name));
      this.selectedStoryDescriptionLabel.SetText((string) Strings.Get(storyTrait.description));
      this.selectedStoryImage.sprite = Assets.GetSprite((HashedString) storyTrait.icon.Replace("_icon", "_image"));
    }
  }

  public string GetTraitsString(bool tooltip = false)
  {
    int num1 = 0;
    int num2 = 5;
    foreach (KeyValuePair<string, StoryContentPanel.StoryState> storyState in this.storyStates)
    {
      if (storyState.Value == StoryContentPanel.StoryState.Guaranteed)
        ++num1;
    }
    string storyTraitsHeader = (string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER;
    string str;
    switch (num1)
    {
      case 0:
        str = (string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
        break;
      case 1:
        str = (string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.SINGLE_TRAIT;
        break;
      default:
        str = GameUtil.SafeStringFormat((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.TRAIT_COUNT, (object) num1);
        break;
    }
    string traitsString = $"{storyTraitsHeader}: {str}";
    if (num1 > num2)
      traitsString = $"{traitsString} {(string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING}";
    if (tooltip)
      traitsString = num1 <= num2 ? $"{traitsString}\n\n{(string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.TRAIT_COUNT_TOOLTIP}" : $"{traitsString}\n\n{(string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING_TOOLTIP}";
    return traitsString;
  }

  private enum StoryState
  {
    Forbidden,
    Guaranteed,
    LENGTH,
  }
}
