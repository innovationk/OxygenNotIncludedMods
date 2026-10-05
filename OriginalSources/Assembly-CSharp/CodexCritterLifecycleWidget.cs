// Decompiled with JetBrains decompiler
// Type: CodexCritterLifecycleWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CodexCritterLifecycleWidget : CodexWidget<CodexCritterLifecycleWidget>
{
  private CodexCritterLifecycleWidget()
  {
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
    component.GetReference<Image>("EggIcon").sprite = (Sprite) null;
    component.GetReference<Image>("EggIcon").color = Color.white;
    component.GetReference<LocText>("EggToBabyLabel").text = "";
    component.GetReference<Image>("BabyIcon").sprite = (Sprite) null;
    component.GetReference<Image>("BabyIcon").color = Color.white;
    component.GetReference<LocText>("BabyToAdultLabel").text = "";
    component.GetReference<Image>("AdultIcon").sprite = (Sprite) null;
    component.GetReference<Image>("AdultIcon").color = Color.white;
  }
}
