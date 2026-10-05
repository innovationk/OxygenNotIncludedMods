// Decompiled with JetBrains decompiler
// Type: DetailScreenTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class DetailScreenTab : TargetPanel
{
  public abstract override bool IsValidForTarget(GameObject target);

  protected override void OnSelectTarget(GameObject target) => base.OnSelectTarget(target);

  protected CollapsibleDetailContentPanel CreateCollapsableSection(string title = null)
  {
    CollapsibleDetailContentPanel collapsableSection = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject);
    if (!string.IsNullOrEmpty(title))
      collapsableSection.SetTitle(title);
    return collapsableSection;
  }

  private void Update() => this.Refresh();

  protected virtual void Refresh(bool force = false)
  {
  }
}
