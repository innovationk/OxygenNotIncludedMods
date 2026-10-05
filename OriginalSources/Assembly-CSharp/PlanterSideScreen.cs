// Decompiled with JetBrains decompiler
// Type: PlanterSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PlanterSideScreen : ReceptacleSideScreen
{
  public DescriptorPanel RequirementsDescriptorPanel;
  public DescriptorPanel HarvestDescriptorPanel;
  public DescriptorPanel EffectsDescriptorPanel;
  public GameObject mutationPanel;
  public GameObject mutationViewport;
  public GameObject mutationContainer;
  public GameObject mutationOption;
  public GameObject blankMutationOption;
  public GameObject selectSpeciesPrompt;
  private bool mutationPanelCollapsed = true;
  public Dictionary<GameObject, Tag> subspeciesToggles = new Dictionary<GameObject, Tag>();
  private List<GameObject> blankMutationObjects = new List<GameObject>();
  private Dictionary<PlantablePlot, Tag> entityPreviousSubSelectionMap = new Dictionary<PlantablePlot, Tag>();
  private Coroutine activeAnimationRoutine;
  private const float EXPAND_DURATION = 0.33f;
  private const float EXPAND_MIN = 24f;
  private const float EXPAND_MAX = 118f;

  private Tag selectedSubspecies
  {
    get
    {
      if (!this.entityPreviousSubSelectionMap.ContainsKey((PlantablePlot) this.targetReceptacle))
        this.entityPreviousSubSelectionMap.Add((PlantablePlot) this.targetReceptacle, Tag.Invalid);
      return this.entityPreviousSubSelectionMap[(PlantablePlot) this.targetReceptacle];
    }
    set
    {
      if (!this.entityPreviousSubSelectionMap.ContainsKey((PlantablePlot) this.targetReceptacle))
        this.entityPreviousSubSelectionMap.Add((PlantablePlot) this.targetReceptacle, Tag.Invalid);
      this.entityPreviousSubSelectionMap[(PlantablePlot) this.targetReceptacle] = value;
      this.selectedDepositObjectAdditionalTag = value;
    }
  }

  private void LoadTargetSubSpeciesRequest()
  {
    PlantablePlot targetReceptacle = (PlantablePlot) this.targetReceptacle;
    Tag tag = Tag.Invalid;
    if (targetReceptacle.requestedEntityTag != Tag.Invalid && targetReceptacle.requestedEntityTag != GameTags.Empty)
      tag = targetReceptacle.requestedEntityTag;
    else if ((UnityEngine.Object) this.selectedEntityToggle != (UnityEngine.Object) null)
      tag = this.depositObjectMap[this.selectedEntityToggle].tag;
    if (!DlcManager.FeaturePlantMutationsEnabled() || !tag.IsValid)
      return;
    MutantPlant component = Assets.GetPrefab(tag).GetComponent<MutantPlant>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      this.selectedSubspecies = Tag.Invalid;
    else if (targetReceptacle.requestedEntityAdditionalFilterTag != Tag.Invalid && targetReceptacle.requestedEntityAdditionalFilterTag != GameTags.Empty)
    {
      this.selectedSubspecies = targetReceptacle.requestedEntityAdditionalFilterTag;
    }
    else
    {
      if (!(this.selectedSubspecies == Tag.Invalid))
        return;
      PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo;
      if (PlantSubSpeciesCatalog.Instance.GetOriginalSubSpecies(component.SpeciesID, out subSpeciesInfo))
        this.selectedSubspecies = subSpeciesInfo.ID;
      targetReceptacle.requestedEntityAdditionalFilterTag = this.selectedSubspecies;
    }
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<PlantablePlot>() != (UnityEngine.Object) null;
  }

  protected override void ToggleClicked(ReceptacleToggle toggle)
  {
    this.LoadTargetSubSpeciesRequest();
    base.ToggleClicked(toggle);
    this.UpdateState((object) null);
  }

  protected void MutationToggleClicked(GameObject toggle)
  {
    this.selectedSubspecies = this.subspeciesToggles[toggle];
    this.UpdateState((object) null);
  }

  protected override void UpdateState(object data)
  {
    base.UpdateState(data);
    this.requestSelectedEntityBtn.onClick += new System.Action(this.RefreshSubspeciesToggles);
    this.RefreshSubspeciesToggles();
  }

  private IEnumerator ExpandMutations()
  {
    LayoutElement le = this.mutationViewport.GetComponent<LayoutElement>();
    float travelPerSecond = 94f / 0.33f;
    while ((double) le.minHeight < 118.0)
    {
      float num = Mathf.Min(le.minHeight + Time.unscaledDeltaTime * travelPerSecond, 118f);
      le.minHeight = num;
      le.preferredHeight = num;
      yield return (object) SequenceUtil.WaitForEndOfFrame;
    }
    this.mutationPanelCollapsed = false;
    this.activeAnimationRoutine = (Coroutine) null;
    yield return (object) 0;
  }

  private IEnumerator CollapseMutations()
  {
    LayoutElement le = this.mutationViewport.GetComponent<LayoutElement>();
    float travelPerSecond = -94f / 0.33f;
    while ((double) le.minHeight > 24.0)
    {
      float num = Mathf.Max(le.minHeight + Time.unscaledDeltaTime * travelPerSecond, 24f);
      le.minHeight = num;
      le.preferredHeight = num;
      yield return (object) SequenceUtil.WaitForEndOfFrame;
    }
    this.mutationPanelCollapsed = true;
    this.activeAnimationRoutine = (Coroutine) null;
    yield return (object) SequenceUtil.WaitForNextFrame;
  }

  private void RefreshSubspeciesToggles()
  {
    foreach (KeyValuePair<GameObject, Tag> subspeciesToggle in this.subspeciesToggles)
      UnityEngine.Object.Destroy((UnityEngine.Object) subspeciesToggle.Key);
    this.subspeciesToggles.Clear();
    if (!PlantSubSpeciesCatalog.Instance.AnyNonOriginalDiscovered)
    {
      this.mutationPanel.SetActive(false);
    }
    else
    {
      this.mutationPanel.SetActive(true);
      foreach (UnityEngine.Object blankMutationObject in this.blankMutationObjects)
        UnityEngine.Object.Destroy(blankMutationObject);
      this.blankMutationObjects.Clear();
      this.selectSpeciesPrompt.SetActive(false);
      if (this.selectedDepositObjectTag.IsValid)
      {
        Tag plantId = Assets.GetPrefab(this.selectedDepositObjectTag).GetComponent<PlantableSeed>().PlantID;
        List<PlantSubSpeciesCatalog.SubSpeciesInfo> speciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(plantId);
        if (speciesForSpecies != null)
        {
          foreach (PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo in speciesForSpecies)
          {
            GameObject option = Util.KInstantiateUI(this.mutationOption, this.mutationContainer, true);
            option.GetComponentInChildren<LocText>().text = subSpeciesInfo.GetNameWithMutations(plantId.ProperName(), PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subSpeciesInfo.ID), false);
            option.GetComponent<MultiToggle>().onClick += (System.Action) (() => this.MutationToggleClicked(option));
            option.GetComponent<ToolTip>().SetSimpleTooltip(subSpeciesInfo.GetMutationsTooltip());
            this.subspeciesToggles.Add(option, subSpeciesInfo.ID);
          }
          for (int count = speciesForSpecies.Count; count < 5; ++count)
            this.blankMutationObjects.Add(Util.KInstantiateUI(this.blankMutationOption, this.mutationContainer, true));
          if (!this.selectedSubspecies.IsValid || !this.subspeciesToggles.ContainsValue(this.selectedSubspecies))
            this.selectedSubspecies = speciesForSpecies[0].ID;
        }
      }
      else
        this.selectSpeciesPrompt.SetActive(true);
      ICollection<Pickupable> pickupables1 = (ICollection<Pickupable>) new List<Pickupable>();
      bool flag1 = this.CheckReceptacleOccupied();
      bool flag2 = this.targetReceptacle.GetActiveRequest != null;
      WorldContainer myWorld = this.targetReceptacle.GetMyWorld();
      ICollection<Pickupable> pickupables2 = myWorld.worldInventory.GetPickupables(this.selectedDepositObjectTag, myWorld.IsModuleInterior);
      foreach (KeyValuePair<GameObject, Tag> subspeciesToggle in this.subspeciesToggles)
      {
        float num = 0.0f;
        bool flag3 = PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subspeciesToggle.Value);
        if (pickupables2 != null)
        {
          foreach (Pickupable cmp in (IEnumerable<Pickupable>) pickupables2)
          {
            if (cmp.HasTag(subspeciesToggle.Value))
              num += cmp.GetComponent<PrimaryElement>().Units;
          }
        }
        if ((double) num > 0.0 & flag3)
          subspeciesToggle.Key.GetComponent<MultiToggle>().ChangeState(subspeciesToggle.Value == this.selectedSubspecies ? 1 : 0);
        else
          subspeciesToggle.Key.GetComponent<MultiToggle>().ChangeState(subspeciesToggle.Value == this.selectedSubspecies ? 3 : 2);
        subspeciesToggle.Key.GetComponentInChildren<LocText>().text += $" ({num})";
        if (flag1 | flag2)
        {
          if (subspeciesToggle.Value == this.selectedSubspecies)
          {
            subspeciesToggle.Key.SetActive(true);
            subspeciesToggle.Key.GetComponent<MultiToggle>().ChangeState(1);
          }
          else
            subspeciesToggle.Key.SetActive(false);
        }
        else
          subspeciesToggle.Key.SetActive((UnityEngine.Object) this.selectedEntityToggle != (UnityEngine.Object) null);
      }
      bool flag4 = !flag1 && !flag2 && (UnityEngine.Object) this.selectedEntityToggle != (UnityEngine.Object) null && this.subspeciesToggles.Count >= 1;
      if (flag4 && this.mutationPanelCollapsed)
      {
        if (this.activeAnimationRoutine != null)
          this.StopCoroutine(this.activeAnimationRoutine);
        this.activeAnimationRoutine = this.StartCoroutine(this.ExpandMutations());
      }
      else
      {
        if (flag4 || this.mutationPanelCollapsed)
          return;
        if (this.activeAnimationRoutine != null)
          this.StopCoroutine(this.activeAnimationRoutine);
        this.activeAnimationRoutine = this.StartCoroutine(this.CollapseMutations());
      }
    }
  }

  protected override Sprite GetEntityIcon(Tag prefabTag)
  {
    PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? base.GetEntityIcon(new Tag(component.PlantID)) : base.GetEntityIcon(prefabTag);
  }

  protected override string GetEntityName(Tag prefabTag)
  {
    PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null ? Assets.GetPrefab(component.PlantID).GetProperName() : base.GetEntityName(prefabTag);
  }

  protected override string GetEntityTooltip(Tag prefabTag)
  {
    PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
    return string.Format((string) STRINGS.UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANT_TOGGLE_TOOLTIP, (object) this.GetEntityName(prefabTag), (object) component.domesticatedDescription, (object) this.GetAvailableAmount(prefabTag));
  }

  protected override void SetResultDescriptions(GameObject seed_or_plant)
  {
    string sourceText = "";
    GameObject go = seed_or_plant;
    PlantableSeed component1 = seed_or_plant.GetComponent<PlantableSeed>();
    List<Descriptor> descriptorList = new List<Descriptor>();
    bool flag = true;
    if ((UnityEngine.Object) seed_or_plant.GetComponent<MutantPlant>() != (UnityEngine.Object) null && this.selectedDepositObjectAdditionalTag != Tag.Invalid)
      flag = PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.selectedDepositObjectAdditionalTag);
    if (!flag)
      sourceText = $"{(string) CREATURES.PLANT_MUTATIONS.UNIDENTIFIED}\n\n{(string) CREATURES.PLANT_MUTATIONS.UNIDENTIFIED_DESC}";
    else if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      descriptorList = component1.GetDescriptors(component1.gameObject);
      if ((UnityEngine.Object) this.targetReceptacle.rotatable != (UnityEngine.Object) null && this.targetReceptacle.Direction != component1.direction)
      {
        if (component1.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
          sourceText += (string) STRINGS.UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_FLOOR;
        else if (component1.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
          sourceText += (string) STRINGS.UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_WALL;
        else if (component1.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
          sourceText += (string) STRINGS.UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_CEILING;
        sourceText += "\n\n";
      }
      go = Assets.GetPrefab(component1.PlantID);
      MutantPlant component2 = go.GetComponent<MutantPlant>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && this.selectedDepositObjectAdditionalTag.IsValid)
      {
        PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(component1.PlantID, this.selectedDepositObjectAdditionalTag);
        component2.DummySetSubspecies(subSpecies.mutationIDs);
      }
      if (!string.IsNullOrEmpty(component1.domesticatedDescription))
        sourceText += component1.domesticatedDescription;
    }
    else
    {
      InfoDescription component3 = go.GetComponent<InfoDescription>();
      if ((bool) (UnityEngine.Object) component3)
        sourceText += component3.description;
    }
    this.descriptionLabel.SetText(sourceText);
    List<Descriptor> cycleDescriptors = GameUtil.GetPlantLifeCycleDescriptors(go);
    if (cycleDescriptors.Count > 0 & flag)
    {
      this.HarvestDescriptorPanel.SetDescriptors((IList<Descriptor>) cycleDescriptors);
      this.HarvestDescriptorPanel.gameObject.SetActive(true);
    }
    else
      this.HarvestDescriptorPanel.gameObject.SetActive(false);
    List<Descriptor> requirementDescriptors = GameUtil.GetPlantRequirementDescriptors(go);
    if (descriptorList.Count > 0)
    {
      GameUtil.IndentListOfDescriptors(descriptorList);
      requirementDescriptors.InsertRange(requirementDescriptors.Count, (IEnumerable<Descriptor>) descriptorList);
    }
    if (requirementDescriptors.Count > 0 && flag)
    {
      this.RequirementsDescriptorPanel.SetDescriptors((IList<Descriptor>) requirementDescriptors);
      this.RequirementsDescriptorPanel.gameObject.SetActive(true);
    }
    else
      this.RequirementsDescriptorPanel.gameObject.SetActive(false);
    List<Descriptor> effectDescriptors = GameUtil.GetPlantEffectDescriptors(go);
    if (effectDescriptors.Count > 0 && flag)
    {
      this.EffectsDescriptorPanel.SetDescriptors((IList<Descriptor>) effectDescriptors);
      this.EffectsDescriptorPanel.gameObject.SetActive(true);
    }
    else
      this.EffectsDescriptorPanel.gameObject.SetActive(false);
  }

  protected override bool AdditionalCanDepositTest()
  {
    bool flag = false;
    if (this.selectedDepositObjectTag.IsValid)
      flag = !DlcManager.FeaturePlantMutationsEnabled() ? this.selectedDepositObjectTag.IsValid : PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
    PlantablePlot targetReceptacle = this.targetReceptacle as PlantablePlot;
    WorldContainer myWorld = this.targetReceptacle.GetMyWorld();
    return flag && targetReceptacle.ValidPlant && myWorld.worldInventory.GetCountWithAdditionalTag(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag, myWorld.IsModuleInterior) > 0;
  }

  public override void SetTarget(GameObject target)
  {
    this.selectedDepositObjectTag = Tag.Invalid;
    this.selectedDepositObjectAdditionalTag = Tag.Invalid;
    base.SetTarget(target);
    this.LoadTargetSubSpeciesRequest();
    this.RefreshSubspeciesToggles();
  }

  protected override void RestoreSelectionFromOccupant()
  {
    base.RestoreSelectionFromOccupant();
    PlantablePlot targetReceptacle = (PlantablePlot) this.targetReceptacle;
    Tag tag1 = Tag.Invalid;
    Tag tag2 = Tag.Invalid;
    bool flag = false;
    if ((UnityEngine.Object) targetReceptacle.Occupant != (UnityEngine.Object) null)
    {
      tag1 = (Tag) targetReceptacle.Occupant.GetComponent<SeedProducer>().seedInfo.seedId;
      MutantPlant component = targetReceptacle.Occupant.GetComponent<MutantPlant>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        tag2 = component.SubSpeciesID;
    }
    else if (targetReceptacle.GetActiveRequest != null)
    {
      tag1 = targetReceptacle.requestedEntityTag;
      tag2 = targetReceptacle.requestedEntityAdditionalFilterTag;
      this.selectedDepositObjectTag = tag1;
      this.selectedDepositObjectAdditionalTag = tag2;
      flag = true;
    }
    if (!(tag1 != Tag.Invalid))
      return;
    if (!this.entityPreviousSelectionMap.ContainsKey((SingleEntityReceptacle) targetReceptacle) | flag)
    {
      int num = 0;
      foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObject in this.depositObjectMap)
      {
        if (depositObject.Value.tag == tag1)
          num = this.entityToggles.IndexOf(depositObject.Key);
      }
      if (!this.entityPreviousSelectionMap.ContainsKey((SingleEntityReceptacle) targetReceptacle))
        this.entityPreviousSelectionMap.Add((SingleEntityReceptacle) targetReceptacle, -1);
      this.entityPreviousSelectionMap[(SingleEntityReceptacle) targetReceptacle] = num;
    }
    if (!this.entityPreviousSubSelectionMap.ContainsKey(targetReceptacle))
      this.entityPreviousSubSelectionMap.Add(targetReceptacle, Tag.Invalid);
    if (!(this.entityPreviousSubSelectionMap[targetReceptacle] == Tag.Invalid | flag))
      return;
    this.entityPreviousSubSelectionMap[targetReceptacle] = tag2;
  }
}
