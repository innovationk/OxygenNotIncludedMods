// Decompiled with JetBrains decompiler
// Type: ConfigureConsumerSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ConfigureConsumerSideScreen : SideScreenContent
{
  [SerializeField]
  private RectTransform consumptionSettingToggleContainer;
  [SerializeField]
  private GameObject consumptionSettingTogglePrefab;
  [SerializeField]
  private RectTransform settingRequirementRowsContainer;
  [SerializeField]
  private RectTransform settingEffectRowsContainer;
  [SerializeField]
  private LocText selectedOptionNameLabel;
  [SerializeField]
  private GameObject settingDescriptorPrefab;
  private IConfigurableConsumer targetProducer;
  private IConfigurableConsumerOption[] settings;
  private LocText descriptor;
  private List<HierarchyReferences> settingToggles = new List<HierarchyReferences>();
  private List<GameObject> requirementRows = new List<GameObject>();

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<IConfigurableConsumer>() != null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetProducer = target.GetComponent<IConfigurableConsumer>();
    if (this.settings == null)
      this.settings = this.targetProducer.GetSettingOptions();
    this.PopulateOptions();
  }

  private void ClearOldOptions()
  {
    if ((UnityEngine.Object) this.descriptor != (UnityEngine.Object) null)
      this.descriptor.gameObject.SetActive(false);
    for (int index = 0; index < this.settingToggles.Count; ++index)
      this.settingToggles[index].gameObject.SetActive(false);
  }

  private void PopulateOptions()
  {
    this.ClearOldOptions();
    for (int count = this.settingToggles.Count; count < this.settings.Length; ++count)
    {
      IConfigurableConsumerOption setting = this.settings[count];
      HierarchyReferences component = Util.KInstantiateUI(this.consumptionSettingTogglePrefab, this.consumptionSettingToggleContainer.gameObject, true).GetComponent<HierarchyReferences>();
      this.settingToggles.Add(component);
      component.GetReference<LocText>("Label").text = setting.GetName();
      component.GetReference<Image>("Image").sprite = setting.GetIcon();
      component.GetReference<MultiToggle>("Toggle").onClick += (System.Action) (() => this.SelectOption(setting));
    }
    this.RefreshToggles();
    this.RefreshDetails();
  }

  private void SelectOption(IConfigurableConsumerOption option)
  {
    this.targetProducer.SetSelectedOption(option);
    this.RefreshToggles();
    this.RefreshDetails();
  }

  private void RefreshToggles()
  {
    for (int index = 0; index < this.settingToggles.Count; ++index)
    {
      MultiToggle reference = this.settingToggles[index].GetReference<MultiToggle>("Toggle");
      reference.ChangeState(this.settings[index] == this.targetProducer.GetSelectedOption() ? 1 : 0);
      reference.gameObject.SetActive(true);
    }
  }

  private void RefreshDetails()
  {
    if ((UnityEngine.Object) this.descriptor == (UnityEngine.Object) null)
      this.descriptor = Util.KInstantiateUI(this.settingDescriptorPrefab, this.settingEffectRowsContainer.gameObject, true).GetComponent<LocText>();
    IConfigurableConsumerOption selectedOption = this.targetProducer.GetSelectedOption();
    if (selectedOption != null)
    {
      this.descriptor.text = selectedOption.GetDetailedDescription();
      this.selectedOptionNameLabel.text = $"<b>{selectedOption.GetName()}</b>";
      this.descriptor.gameObject.SetActive(true);
    }
    else
      this.selectedOptionNameLabel.text = (string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPESELECTED;
  }

  public override int GetSideScreenSortOrder() => 1;
}
