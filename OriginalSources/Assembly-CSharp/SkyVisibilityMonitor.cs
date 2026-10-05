// Decompiled with JetBrains decompiler
// Type: SkyVisibilityMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using UnityEngine;

#nullable disable
public class SkyVisibilityMonitor : 
  GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>
{
  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    this.root.Update(new System.Action<SkyVisibilityMonitor.Instance, float>(SkyVisibilityMonitor.CheckSkyVisibility), UpdateRate.SIM_1000ms);
  }

  public static void CheckSkyVisibility(SkyVisibilityMonitor.Instance smi, float dt)
  {
    int num1 = smi.HasSkyVisibility ? 1 : 0;
    (bool isAnyVisible, float num2) = smi.def.skyVisibilityInfo.GetVisibilityOf(smi.gameObject);
    smi.Internal_SetPercentClearSky(num2);
    KSelectable component = smi.GetComponent<KSelectable>();
    component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !isAnyVisible, (object) smi);
    component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, isAnyVisible && (double) num2 < 1.0, (object) smi);
    int num3 = isAnyVisible ? 1 : 0;
    if (num1 == num3)
      return;
    smi.TriggerVisibilityChange();
  }

  public class Def : StateMachine.BaseDef
  {
    public SkyVisibilityInfo skyVisibilityInfo;
  }

  public new class Instance(IStateMachineTarget master, SkyVisibilityMonitor.Def def) : 
    GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>.GameInstance(master, def),
    BuildingStatusItems.ISkyVisInfo
  {
    private float percentClearSky01;
    public System.Action SkyVisibilityChanged;
    private StatusItem visibilityStatusItem;
    private static readonly Operational.Flag skyVisibilityFlag = new Operational.Flag("sky visibility", Operational.Flag.Type.Requirement);

    public bool HasSkyVisibility
    {
      get
      {
        return (double) this.PercentClearSky > 0.0 && !Mathf.Approximately(0.0f, this.PercentClearSky);
      }
    }

    public float PercentClearSky => this.percentClearSky01;

    public void Internal_SetPercentClearSky(float percent01) => this.percentClearSky01 = percent01;

    float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01() => this.percentClearSky01;

    public override void StartSM()
    {
      base.StartSM();
      SkyVisibilityMonitor.CheckSkyVisibility(this, 0.0f);
      this.TriggerVisibilityChange();
    }

    public void TriggerVisibilityChange()
    {
      if (this.visibilityStatusItem != null)
        this.smi.GetComponent<KSelectable>().ToggleStatusItem(this.visibilityStatusItem, !this.HasSkyVisibility, (object) this);
      this.smi.GetComponent<Operational>().SetFlag(SkyVisibilityMonitor.Instance.skyVisibilityFlag, this.HasSkyVisibility);
      if (this.SkyVisibilityChanged == null)
        return;
      this.SkyVisibilityChanged();
    }
  }
}
