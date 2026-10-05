// Decompiled with JetBrains decompiler
// Type: ProgressBarSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ProgressBarSideScreen : SideScreenContent, IRender1000ms
{
  public LocText label;
  public GenericUIProgressBar progressBar;
  public IProgressBarSideScreen targetObject;

  protected override void OnSpawn() => base.OnSpawn();

  public override int GetSideScreenSortOrder() => -10;

  public override bool IsValidForTarget(GameObject target)
  {
    return target.GetComponent<IProgressBarSideScreen>() != null;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetObject = target.GetComponent<IProgressBarSideScreen>();
    this.RefreshBar();
  }

  private void RefreshBar()
  {
    this.progressBar.SetMaxValue(this.targetObject.GetProgressBarMaxValue());
    this.progressBar.SetFillPercentage(this.targetObject.GetProgressBarFillPercentage());
    this.progressBar.label.SetText(this.targetObject.GetProgressBarLabel());
    this.label.SetText(this.targetObject.GetProgressBarTitleLabel());
    this.progressBar.GetComponentInChildren<ToolTip>().SetSimpleTooltip(this.targetObject.GetProgressBarTooltip());
  }

  public void Render1000ms(float dt) => this.RefreshBar();
}
