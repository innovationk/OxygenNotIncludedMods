// Decompiled with JetBrains decompiler
// Type: SingleSliderSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SingleSliderSideScreen : SideScreenContent
{
  private ISingleSliderControl target;
  public List<SliderSet> sliderSets;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.sliderSets.Count; ++index)
      this.sliderSets[index].SetupSlider(index);
  }

  public override bool IsValidForTarget(GameObject target)
  {
    KPrefabID component = target.GetComponent<KPrefabID>();
    ISingleSliderControl singleSliderControl = target.GetComponent<ISingleSliderControl>() ?? target.GetSMI<ISingleSliderControl>();
    return singleSliderControl != null && !component.IsPrefabID("HydrogenGenerator".ToTag()) && !component.IsPrefabID("MethaneGenerator".ToTag()) && !component.IsPrefabID("PetroleumGenerator".ToTag()) && !component.IsPrefabID("DevGenerator".ToTag()) && !component.HasTag(GameTags.DeadReactor) && (double) singleSliderControl.GetSliderMin(0) != (double) singleSliderControl.GetSliderMax(0);
  }

  public override void SetTarget(GameObject new_target)
  {
    if ((Object) new_target == (Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<ISingleSliderControl>();
      if (this.target == null)
      {
        this.target = new_target.GetSMI<ISingleSliderControl>();
        if (this.target == null)
        {
          Debug.LogError((object) "The gameObject received does not contain a ISingleSliderControl implementation");
          return;
        }
      }
      this.titleKey = this.target.SliderTitleKey;
      for (int index = 0; index < this.sliderSets.Count; ++index)
        this.sliderSets[index].SetTarget((ISliderControl) this.target, index);
    }
  }
}
