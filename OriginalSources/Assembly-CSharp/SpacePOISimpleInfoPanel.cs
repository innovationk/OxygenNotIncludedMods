// Decompiled with JetBrains decompiler
// Type: SpacePOISimpleInfoPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SpacePOISimpleInfoPanel(SimpleInfoScreen simpleInfoScreen) : SimpleInfoPanel(simpleInfoScreen)
{
  private Dictionary<Tag, GameObject> elementRows = new Dictionary<Tag, GameObject>();
  private Dictionary<Clustercraft, GameObject> rocketRows = new Dictionary<Clustercraft, GameObject>();
  private GameObject massHeader;
  private GameObject rocketsSpacer;
  private GameObject rocketsHeader;
  private GameObject artifactsSpacer;
  private GameObject artifactRow;

  public override void Refresh(
    CollapsibleDetailContentPanel spacePOIPanel,
    GameObject selectedTarget)
  {
    spacePOIPanel.SetTitle((string) STRINGS.UI.CLUSTERMAP.POI.TITLE);
    if ((UnityEngine.Object) selectedTarget == (UnityEngine.Object) null)
    {
      spacePOIPanel.gameObject.SetActive(false);
    }
    else
    {
      HarvestablePOIClusterGridEntity component1 = (UnityEngine.Object) selectedTarget == (UnityEngine.Object) null ? (HarvestablePOIClusterGridEntity) null : selectedTarget.GetComponent<HarvestablePOIClusterGridEntity>();
      Clustercraft component2 = selectedTarget.GetComponent<Clustercraft>();
      ArtifactPOIConfigurator component3 = selectedTarget.GetComponent<ArtifactPOIConfigurator>();
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null && (UnityEngine.Object) component3 == (UnityEngine.Object) null)
      {
        spacePOIPanel.gameObject.SetActive(false);
      }
      else
      {
        bool flag = (UnityEngine.Object) component1 != (UnityEngine.Object) null || (UnityEngine.Object) component3 != (UnityEngine.Object) null;
        spacePOIPanel.gameObject.SetActive(flag);
        if (!flag)
          return;
        HarvestablePOIStates.Instance smi = (UnityEngine.Object) component1 == (UnityEngine.Object) null ? (HarvestablePOIStates.Instance) null : component1.GetSMI<HarvestablePOIStates.Instance>();
        this.RefreshMassHeader(smi, selectedTarget, spacePOIPanel);
        this.RefreshElements(smi, selectedTarget, spacePOIPanel);
        this.RefreshArtifacts(component3, selectedTarget, spacePOIPanel);
      }
    }
  }

  private void RefreshMassHeader(
    HarvestablePOIStates.Instance harvestable,
    GameObject selectedTarget,
    CollapsibleDetailContentPanel spacePOIPanel)
  {
    if ((UnityEngine.Object) this.massHeader == (UnityEngine.Object) null)
      this.massHeader = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true);
    this.massHeader.SetActive(harvestable != null);
    if (harvestable == null)
      return;
    HierarchyReferences component = this.massHeader.GetComponent<HierarchyReferences>();
    Sprite sprite = Assets.GetSprite((HashedString) "icon_asteroid_type");
    if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
      component.GetReference<Image>("Icon").sprite = sprite;
    component.GetReference<LocText>("NameLabel").text = (string) STRINGS.UI.CLUSTERMAP.POI.MASS_REMAINING;
    component.GetReference<LocText>("ValueLabel").text = GameUtil.GetFormattedMass(harvestable.poiCapacity);
    component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
  }

  private void RefreshElements(
    HarvestablePOIStates.Instance harvestable,
    GameObject selectedTarget,
    CollapsibleDetailContentPanel spacePOIPanel)
  {
    foreach (KeyValuePair<Tag, GameObject> elementRow in this.elementRows)
    {
      if ((UnityEngine.Object) elementRow.Value != (UnityEngine.Object) null)
        elementRow.Value.SetActive(false);
    }
    if (harvestable == null)
      return;
    Dictionary<SimHashes, float> elementsWithWeights = harvestable.configuration.GetElementsWithWeights();
    float num = 0.0f;
    List<KeyValuePair<SimHashes, float>> keyValuePairList = new List<KeyValuePair<SimHashes, float>>();
    foreach (KeyValuePair<SimHashes, float> keyValuePair in elementsWithWeights)
    {
      num += keyValuePair.Value;
      keyValuePairList.Add(keyValuePair);
    }
    keyValuePairList.Sort((Comparison<KeyValuePair<SimHashes, float>>) ((a, b) => b.Value.CompareTo(a.Value)));
    foreach (KeyValuePair<SimHashes, float> keyValuePair in keyValuePairList)
    {
      SimHashes key = keyValuePair.Key;
      Tag tag = key.CreateTag();
      if (!this.elementRows.ContainsKey(key.CreateTag()))
        this.elementRows.Add(tag, Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true));
      this.elementRows[tag].SetActive(true);
      HierarchyReferences component = this.elementRows[tag].GetComponent<HierarchyReferences>();
      Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) tag);
      component.GetReference<Image>("Icon").sprite = uiSprite.first;
      component.GetReference<Image>("Icon").color = uiSprite.second;
      component.GetReference<LocText>("NameLabel").text = ElementLoader.GetElement(tag).name;
      component.GetReference<LocText>("ValueLabel").text = GameUtil.GetFormattedPercent((float) ((double) keyValuePair.Value / (double) num * 100.0));
      component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
    }
  }

  private void RefreshRocketsAtThisLocation(
    HarvestablePOIStates.Instance harvestable,
    GameObject selectedTarget,
    CollapsibleDetailContentPanel spacePOIPanel)
  {
    if ((UnityEngine.Object) this.rocketsHeader == (UnityEngine.Object) null)
    {
      this.rocketsSpacer = Util.KInstantiateUI(this.simpleInfoRoot.spacerRow, spacePOIPanel.Content.gameObject, true);
      this.rocketsHeader = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true);
      HierarchyReferences component = this.rocketsHeader.GetComponent<HierarchyReferences>();
      Sprite sprite = Assets.GetSprite((HashedString) "ic_rocket");
      if ((UnityEngine.Object) sprite != (UnityEngine.Object) null)
      {
        component.GetReference<Image>("Icon").sprite = sprite;
        component.GetReference<Image>("Icon").color = Color.black;
      }
      component.GetReference<LocText>("NameLabel").text = (string) STRINGS.UI.CLUSTERMAP.POI.ROCKETS_AT_THIS_LOCATION;
      component.GetReference<LocText>("ValueLabel").text = "";
    }
    this.rocketsSpacer.rectTransform().SetAsLastSibling();
    this.rocketsHeader.rectTransform().SetAsLastSibling();
    foreach (KeyValuePair<Clustercraft, GameObject> rocketRow in this.rocketRows)
      rocketRow.Value.SetActive(false);
    bool flag1 = true;
    for (int idx = 0; idx < Components.Clustercrafts.Count; ++idx)
    {
      Clustercraft clustercraft = Components.Clustercrafts[idx];
      if (!this.rocketRows.ContainsKey(clustercraft))
      {
        GameObject gameObject = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true);
        this.rocketRows.Add(clustercraft, gameObject);
      }
      bool flag2 = clustercraft.Location == selectedTarget.GetComponent<KMonoBehaviour>().GetMyWorldLocation();
      flag1 = flag1 && !flag2;
      this.rocketRows[clustercraft].SetActive(flag2);
      if (flag2)
      {
        HierarchyReferences component = this.rocketRows[clustercraft].GetComponent<HierarchyReferences>();
        component.GetReference<Image>("Icon").sprite = clustercraft.GetUISprite();
        component.GetReference<Image>("Icon").color = Color.grey;
        component.GetReference<LocText>("NameLabel").text = clustercraft.Name;
        component.GetReference<LocText>("ValueLabel").text = "";
        component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
        this.rocketRows[clustercraft].rectTransform().SetAsLastSibling();
      }
    }
    this.rocketsHeader.SetActive(!flag1);
    this.rocketsSpacer.SetActive(this.rocketsHeader.activeSelf);
  }

  private void RefreshArtifacts(
    ArtifactPOIConfigurator artifactConfigurator,
    GameObject selectedTarget,
    CollapsibleDetailContentPanel spacePOIPanel)
  {
    if ((UnityEngine.Object) artifactConfigurator == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) this.artifactsSpacer != (UnityEngine.Object) null)
        this.artifactsSpacer.SetActive(false);
      if (!((UnityEngine.Object) this.artifactRow != (UnityEngine.Object) null))
        return;
      this.artifactRow.SetActive(false);
    }
    else
    {
      if ((UnityEngine.Object) this.artifactsSpacer == (UnityEngine.Object) null)
      {
        this.artifactsSpacer = Util.KInstantiateUI(this.simpleInfoRoot.spacerRow, spacePOIPanel.Content.gameObject, true);
        this.artifactRow = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true);
      }
      this.artifactsSpacer.rectTransform().SetAsLastSibling();
      this.artifactRow.rectTransform().SetAsLastSibling();
      this.artifactsSpacer.SetActive(true);
      this.artifactRow.SetActive(true);
      ArtifactPOIStates.Instance smi = artifactConfigurator.GetSMI<ArtifactPOIStates.Instance>();
      smi.configuration.GetArtifactID();
      HierarchyReferences component = this.artifactRow.GetComponent<HierarchyReferences>();
      component.GetReference<LocText>("NameLabel").text = (string) STRINGS.UI.CLUSTERMAP.POI.ARTIFACTS;
      component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
      component.GetReference<Image>("Icon").sprite = Assets.GetSprite((HashedString) "ic_artifacts");
      component.GetReference<Image>("Icon").color = Color.black;
      if (smi.HasArtifactAvailableInHexCell())
        component.GetReference<LocText>("ValueLabel").text = (string) STRINGS.UI.CLUSTERMAP.POI.ARTIFACTS_AVAILABLE;
      else
        component.GetReference<LocText>("ValueLabel").text = string.Format((string) STRINGS.UI.CLUSTERMAP.POI.ARTIFACTS_DEPLETED, (object) GameUtil.GetFormattedCycles(smi.RechargeTimeRemaining(), forceCycles: true));
    }
  }
}
