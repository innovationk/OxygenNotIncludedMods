// Decompiled with JetBrains decompiler
// Type: TurboModeSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TurboModeSideScreen : SideScreenContent
{
  public MultiToggle toggle;
  public LocText label;
  private SpaceHeater target;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.toggle.onClick += new System.Action(this.OnClick);
  }

  private void Refresh()
  {
    this.toggle.ChangeState((double) this.target.UserSliderSetting == 0.0 ? 0 : 1);
  }

  private void OnClick()
  {
    this.target.SetUserSpecifiedPowerConsumptionValue((double) this.target.UserSliderSetting == 0.0 ? this.target.maxPower : this.target.minPower);
    this.Refresh();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    SpaceHeater component = target.GetComponent<SpaceHeater>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.heatLiquid;
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The target object provided was null");
    }
    else
    {
      this.target = target.GetComponent<SpaceHeater>();
      if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
        Debug.LogError((object) "The target provided does not have an ICheckboxControl component");
      else
        this.Refresh();
    }
  }

  public override void ClearTarget()
  {
    base.ClearTarget();
    this.target = (SpaceHeater) null;
  }
}
