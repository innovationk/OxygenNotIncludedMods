// Decompiled with JetBrains decompiler
// Type: AsteroidDescriptorPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using ProcGen;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AsteroidDescriptorPanel")]
public class AsteroidDescriptorPanel : KMonoBehaviour
{
  [Header("Destination Details")]
  [SerializeField]
  private GameObject customLabelPrefab;
  [SerializeField]
  private GameObject prefabTraitWidget;
  [SerializeField]
  private GameObject prefabTraitCategoryWidget;
  [SerializeField]
  private GameObject prefabParameterWidget;
  [SerializeField]
  private GameObject startingAsteroidRowContainer;
  [SerializeField]
  private GameObject nearbyAsteroidRowContainer;
  [SerializeField]
  private GameObject distantAsteroidRowContainer;
  [SerializeField]
  private LocText clusterNameLabel;
  [SerializeField]
  private LocText clusterDifficultyLabel;
  [SerializeField]
  public LocText headerLabel;
  [SerializeField]
  public MultiToggle clusterDetailsButton;
  [SerializeField]
  public GameObject storyTraitHeader;
  private List<GameObject> labels = new List<GameObject>();
  [Header("Selected Asteroid Details")]
  [SerializeField]
  private GameObject SpacedOutContentContainer;
  public Image selectedAsteroidIcon;
  public LocText selectedAsteroidLabel;
  public LocText selectedAsteroidDescription;
  [SerializeField]
  private GameObject prefabAsteroidLine;
  private Dictionary<ProcGen.World, GameObject> asteroidLines = new Dictionary<ProcGen.World, GameObject>();
  private List<GameObject> traitWidgets = new List<GameObject>();
  private List<GameObject> traitCategoryWidgets = new List<GameObject>();
  private List<GameObject> parameterWidgets = new List<GameObject>();

  public bool HasDescriptors() => this.labels.Count > 0;

  public void EnableClusterDetails(bool setActive)
  {
    this.clusterNameLabel.gameObject.SetActive(setActive);
    this.clusterDifficultyLabel.gameObject.SetActive(setActive);
  }

  protected override void OnPrefabInit() => base.OnPrefabInit();

  public void SetClusterDetailLabels(ColonyDestinationAsteroidBeltData cluster)
  {
    StringEntry result;
    Strings.TryGet(cluster.properName, out result);
    this.clusterNameLabel.SetText(result == null ? "" : string.Format((string) WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, (object) result.String));
    int index = Mathf.Clamp(cluster.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
    Tuple<string, string, string> survivalOption = ColonyDestinationAsteroidBeltData.survivalOptions[index];
    this.clusterDifficultyLabel.SetText(string.Format((string) WORLDS.SURVIVAL_CHANCE.TITLE, (object) survivalOption.first, (object) survivalOption.third).Trim('\n'));
  }

  public void SetParameterDescriptors(IList<AsteroidDescriptor> descriptors)
  {
    for (int index = 0; index < this.parameterWidgets.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.parameterWidgets[index]);
    this.parameterWidgets.Clear();
    for (int index = 0; index < descriptors.Count; ++index)
    {
      GameObject gameObject = Util.KInstantiateUI(this.prefabParameterWidget, this.gameObject, true);
      gameObject.GetComponent<LocText>().SetText(descriptors[index].text);
      ToolTip component = gameObject.GetComponent<ToolTip>();
      if (!string.IsNullOrEmpty(descriptors[index].tooltip))
        component.SetSimpleTooltip(descriptors[index].tooltip);
      this.parameterWidgets.Add(gameObject);
    }
  }

  private void ClearTraitDescriptors()
  {
    for (int index = 0; index < this.traitWidgets.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.traitWidgets[index]);
    this.traitWidgets.Clear();
    for (int index = 0; index < this.traitCategoryWidgets.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.traitCategoryWidgets[index]);
    this.traitCategoryWidgets.Clear();
  }

  public void SetTraitDescriptors(
    IList<AsteroidDescriptor> descriptors,
    List<string> stories,
    bool includeDescriptions = true)
  {
    foreach (string storey in stories)
    {
      WorldTrait storyTrait = Db.Get().Stories.Get(storey).StoryTrait;
      string tooltip = (string) (DlcManager.IsPureVanilla() ? Strings.Get(storyTrait.description + "_SHORT") : Strings.Get(storyTrait.description));
      descriptors.Add(new AsteroidDescriptor(Strings.Get(storyTrait.name).String, tooltip, Color.white, associatedIcon: storyTrait.icon));
    }
    this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>()
    {
      descriptors
    }, includeDescriptions);
    if (stories.Count != 0)
    {
      this.storyTraitHeader.rectTransform().SetSiblingIndex(this.storyTraitHeader.rectTransform().parent.childCount - stories.Count - 1);
      this.storyTraitHeader.SetActive(true);
    }
    else
      this.storyTraitHeader.SetActive(false);
  }

  public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, bool includeDescriptions = true)
  {
    this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>()
    {
      descriptors
    }, includeDescriptions);
  }

  public void SetTraitDescriptors(
    List<IList<AsteroidDescriptor>> descriptorSets,
    bool includeDescriptions = true,
    List<Tuple<string, Sprite>> headerData = null)
  {
    this.ClearTraitDescriptors();
    for (int index1 = 0; index1 < descriptorSets.Count; ++index1)
    {
      IList<AsteroidDescriptor> descriptorSet = descriptorSets[index1];
      GameObject gameObject1 = this.gameObject;
      if (descriptorSets.Count > 1)
      {
        Debug.Assert(headerData != null, (object) "Asteroid Header data is null - traits wont have their world as contex in the selection UI");
        GameObject gameObject2 = Util.KInstantiate(this.prefabTraitCategoryWidget, this.gameObject);
        HierarchyReferences component = gameObject2.GetComponent<HierarchyReferences>();
        gameObject2.transform.localScale = Vector3.one;
        StringEntry result;
        string sourceText = Strings.TryGet(headerData[index1].first, out result) ? result.String : headerData[index1].first;
        component.GetReference<LocText>("NameLabel").SetText(sourceText);
        component.GetReference<Image>("Icon").sprite = headerData[index1].second;
        gameObject2.SetActive(true);
        gameObject1 = component.GetReference<RectTransform>("Contents").gameObject;
        this.traitCategoryWidgets.Add(gameObject2);
      }
      for (int index2 = 0; index2 < descriptorSet.Count; ++index2)
      {
        GameObject gameObject3 = Util.KInstantiate(this.prefabTraitWidget, gameObject1);
        HierarchyReferences component = gameObject3.GetComponent<HierarchyReferences>();
        gameObject3.SetActive(true);
        component.GetReference<LocText>("NameLabel").SetText($"<b>{descriptorSet[index2].text}</b>");
        Image reference1 = component.GetReference<Image>("Icon");
        reference1.color = descriptorSet[index2].associatedColor;
        if (descriptorSet[index2].associatedIcon != null)
        {
          Sprite sprite = Assets.GetSprite((HashedString) descriptorSet[index2].associatedIcon);
          if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
            reference1.sprite = sprite;
        }
        if ((UnityEngine.Object) gameObject3.GetComponent<ToolTip>() != (UnityEngine.Object) null)
          gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(descriptorSet[index2].tooltip);
        LocText reference2 = component.GetReference<LocText>("DescLabel");
        if (includeDescriptions && !string.IsNullOrEmpty(descriptorSet[index2].tooltip))
          reference2.SetText(descriptorSet[index2].tooltip);
        else
          reference2.gameObject.SetActive(false);
        gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject3.SetActive(true);
        this.traitWidgets.Add(gameObject3);
      }
    }
  }

  public void EnableClusterLocationLabels(bool enable)
  {
    this.startingAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
    this.nearbyAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
    this.distantAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
  }

  public void RefreshAsteroidLines(
    ColonyDestinationAsteroidBeltData cluster,
    AsteroidDescriptorPanel selectedAsteroidDetailsPanel,
    List<string> storyTraits)
  {
    cluster.RemixClusterLayout();
    foreach (KeyValuePair<ProcGen.World, GameObject> asteroidLine in this.asteroidLines)
    {
      if (!asteroidLine.Value.IsNullOrDestroyed())
        UnityEngine.Object.Destroy((UnityEngine.Object) asteroidLine.Value);
    }
    this.asteroidLines.Clear();
    this.SpawnAsteroidLine(cluster.GetStartWorld, this.startingAsteroidRowContainer, cluster);
    for (int index1 = 0; index1 < cluster.worlds.Count; ++index1)
    {
      ProcGen.World world = cluster.worlds[index1];
      WorldPlacement worldPlacement = (WorldPlacement) null;
      for (int index2 = 0; index2 < cluster.Layout.worldPlacements.Count; ++index2)
      {
        if (cluster.Layout.worldPlacements[index2].world == world.filePath)
        {
          worldPlacement = cluster.Layout.worldPlacements[index2];
          break;
        }
      }
      this.SpawnAsteroidLine(world, worldPlacement.locationType == WorldPlacement.LocationType.InnerCluster ? this.nearbyAsteroidRowContainer : this.distantAsteroidRowContainer, cluster);
    }
    foreach (KeyValuePair<ProcGen.World, GameObject> asteroidLine in this.asteroidLines)
    {
      KeyValuePair<ProcGen.World, GameObject> line = asteroidLine;
      line.Value.GetComponent<MultiToggle>().onClick += (System.Action) (() => this.SelectAsteroidInCluster(line.Key, cluster, selectedAsteroidDetailsPanel));
    }
    this.SelectWholeClusterDetails(cluster, selectedAsteroidDetailsPanel, storyTraits);
  }

  private void SelectAsteroidInCluster(
    ProcGen.World asteroid,
    ColonyDestinationAsteroidBeltData cluster,
    AsteroidDescriptorPanel selectedAsteroidDetailsPanel)
  {
    selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(true);
    this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(0);
    foreach (KeyValuePair<ProcGen.World, GameObject> asteroidLine in this.asteroidLines)
    {
      asteroidLine.Value.GetComponent<MultiToggle>().ChangeState(asteroidLine.Key == asteroid ? 1 : 0);
      if (asteroidLine.Key == asteroid)
        this.SetSelectedAsteroid(asteroidLine.Key, selectedAsteroidDetailsPanel, cluster.GenerateTraitDescriptors(asteroidLine.Key));
    }
  }

  public void SelectWholeClusterDetails(
    ColonyDestinationAsteroidBeltData cluster,
    AsteroidDescriptorPanel selectedAsteroidDetailsPanel,
    List<string> stories)
  {
    selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(false);
    foreach (KeyValuePair<ProcGen.World, GameObject> asteroidLine in this.asteroidLines)
      asteroidLine.Value.GetComponent<MultiToggle>().ChangeState(0);
    this.SetSelectedCluster(cluster, selectedAsteroidDetailsPanel, stories);
    this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(1);
  }

  private void SpawnAsteroidLine(
    ProcGen.World asteroid,
    GameObject parentContainer,
    ColonyDestinationAsteroidBeltData cluster)
  {
    if (this.asteroidLines.ContainsKey(asteroid))
      return;
    GameObject gameObject = Util.KInstantiateUI(this.prefabAsteroidLine, parentContainer.gameObject, true);
    HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
    Image reference1 = component1.GetReference<Image>("Icon");
    LocText reference2 = component1.GetReference<LocText>("Label");
    RectTransform reference3 = component1.GetReference<RectTransform>("TraitsRow");
    LocText reference4 = component1.GetReference<LocText>("TraitLabel");
    ToolTip component2 = gameObject.GetComponent<ToolTip>();
    Image component3 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
    Sprite uiSprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
    reference1.sprite = uiSprite;
    reference2.SetText(asteroid.GetProperName());
    List<WorldTrait> worldTraits = cluster.GetWorldTraits(asteroid);
    reference4.gameObject.SetActive(worldTraits.Count == 0);
    reference4.SetText((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS);
    RectTransform reference5 = component1.GetReference<RectTransform>("TraitIconPrefab");
    foreach (WorldTrait worldTrait in worldTraits)
    {
      Image component4 = Util.KInstantiateUI(reference5.gameObject, reference3.gameObject, true).GetComponent<Image>();
      Sprite sprite = Assets.GetSprite((HashedString) worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1));
      if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
        component4.sprite = sprite;
      component4.color = Util.ColorFromHex(worldTrait.colorHex);
    }
    string message = "";
    if (worldTraits.Count > 0)
    {
      for (int index = 0; index < worldTraits.Count; ++index)
      {
        StringEntry result1;
        Strings.TryGet(worldTraits[index].name, out result1);
        StringEntry result2;
        Strings.TryGet(worldTraits[index].description, out result2);
        message = $"{message}<color=#{worldTraits[index].colorHex}>{result1.String}</color>\n{result2.String}";
        if (index != worldTraits.Count - 1)
          message += "\n\n";
      }
    }
    else
      message = (string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
    if (DlcManager.IsDlcId(asteroid.dlcIdFrom))
      message = $"{message}\n\n{string.Format((string) STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_CONTENT, (object) DlcManager.GetDlcTitle(asteroid.dlcIdFrom))}";
    component2.SetSimpleTooltip(message);
    if (DlcManager.IsDlcId(asteroid.dlcIdFrom))
    {
      component3.color = DlcManager.GetDlcBannerColor(asteroid.dlcIdFrom);
      component3.gameObject.SetActive(true);
    }
    else
      component3.gameObject.SetActive(false);
    this.asteroidLines.Add(asteroid, gameObject);
  }

  private void SetSelectedAsteroid(
    ProcGen.World asteroid,
    AsteroidDescriptorPanel detailPanel,
    List<AsteroidDescriptor> traitDescriptors)
  {
    detailPanel.SetTraitDescriptors((IList<AsteroidDescriptor>) traitDescriptors);
    detailPanel.selectedAsteroidIcon.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
    detailPanel.selectedAsteroidIcon.gameObject.SetActive(true);
    detailPanel.selectedAsteroidLabel.SetText(asteroid.GetProperName());
    detailPanel.selectedAsteroidDescription.SetText(asteroid.GetProperDescription());
  }

  private void SetSelectedCluster(
    ColonyDestinationAsteroidBeltData cluster,
    AsteroidDescriptorPanel detailPanel,
    List<string> stories)
  {
    List<IList<AsteroidDescriptor>> descriptorSets = new List<IList<AsteroidDescriptor>>();
    List<Tuple<string, Sprite>> headerData = new List<Tuple<string, Sprite>>();
    List<AsteroidDescriptor> traitDescriptors1 = cluster.GenerateTraitDescriptors(cluster.GetStartWorld, false);
    if (traitDescriptors1.Count != 0)
    {
      headerData.Add(new Tuple<string, Sprite>(cluster.GetStartWorld.name, ColonyDestinationAsteroidBeltData.GetUISprite(cluster.GetStartWorld.asteroidIcon)));
      descriptorSets.Add((IList<AsteroidDescriptor>) traitDescriptors1);
    }
    foreach (ProcGen.World world in cluster.worlds)
    {
      List<AsteroidDescriptor> traitDescriptors2 = cluster.GenerateTraitDescriptors(world, false);
      if (traitDescriptors2.Count != 0)
      {
        headerData.Add(new Tuple<string, Sprite>(world.name, ColonyDestinationAsteroidBeltData.GetUISprite(world.asteroidIcon)));
        descriptorSets.Add((IList<AsteroidDescriptor>) traitDescriptors2);
      }
    }
    headerData.Add(new Tuple<string, Sprite>("STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER", Assets.GetSprite((HashedString) "codexIconStoryTraits")));
    List<AsteroidDescriptor> asteroidDescriptorList = new List<AsteroidDescriptor>();
    foreach (string storey in stories)
    {
      Story story = Db.Get().Stories.Get(storey);
      string icon = story.StoryTrait.icon;
      AsteroidDescriptor asteroidDescriptor = new AsteroidDescriptor(Strings.Get(story.StoryTrait.name).String, Strings.Get(story.StoryTrait.description).String, Color.white, associatedIcon: icon);
      asteroidDescriptorList.Add(asteroidDescriptor);
    }
    descriptorSets.Add((IList<AsteroidDescriptor>) asteroidDescriptorList);
    detailPanel.SetTraitDescriptors(descriptorSets, false, headerData);
    detailPanel.selectedAsteroidIcon.gameObject.SetActive(false);
    string properName = cluster.properName;
    StringEntry result;
    if (Strings.TryGet(cluster.properName, out result))
      properName = result.String;
    detailPanel.selectedAsteroidLabel.SetText(properName);
    detailPanel.selectedAsteroidDescription.SetText("");
  }
}
