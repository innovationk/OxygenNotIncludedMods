// Decompiled with JetBrains decompiler
// Type: MultiSliderSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MultiSliderSideScreen : SideScreenContent
{
  public LayoutElement sliderPrefab;
  public RectTransform sliderContainer;
  private IMultiSliderControl target;
  private List<GameObject> liveSliders = new List<GameObject>();
  private List<SliderSet> sliderSets = new List<SliderSet>();

  public override bool IsValidForTarget(GameObject target)
  {
    IMultiSliderControl component = target.GetComponent<IMultiSliderControl>();
    return component != null && component.SidescreenEnabled();
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((Object) new_target == (Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<IMultiSliderControl>();
      this.titleKey = this.target.SidescreenTitleKey;
      this.Refresh();
    }
  }

  private void Refresh()
  {
    while (this.liveSliders.Count < this.target.sliderControls.Length)
    {
      GameObject gameObject = Util.KInstantiateUI(this.sliderPrefab.gameObject, this.sliderContainer.gameObject, true);
      HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
      SliderSet sliderSet = new SliderSet();
      sliderSet.valueSlider = component.GetReference<KSlider>("Slider");
      sliderSet.numberInput = component.GetReference<KNumberInputField>("NumberInputField");
      if ((Object) sliderSet.numberInput != (Object) null)
        sliderSet.numberInput.Activate();
      sliderSet.targetLabel = component.GetReference<LocText>("TargetLabel");
      sliderSet.unitsLabel = component.GetReference<LocText>("UnitsLabel");
      sliderSet.minLabel = component.GetReference<LocText>("MinLabel");
      sliderSet.maxLabel = component.GetReference<LocText>("MaxLabel");
      sliderSet.SetupSlider(this.liveSliders.Count);
      this.liveSliders.Add(gameObject);
      this.sliderSets.Add(sliderSet);
    }
    for (int index = 0; index < this.liveSliders.Count; ++index)
    {
      if (index >= this.target.sliderControls.Length)
      {
        this.liveSliders[index].SetActive(false);
      }
      else
      {
        if (!this.liveSliders[index].activeSelf)
        {
          this.liveSliders[index].SetActive(true);
          this.liveSliders[index].gameObject.SetActive(true);
        }
        this.sliderSets[index].SetTarget(this.target.sliderControls[index], index);
      }
    }
  }
}
