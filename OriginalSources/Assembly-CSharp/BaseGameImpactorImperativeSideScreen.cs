// Decompiled with JetBrains decompiler
// Type: BaseGameImpactorImperativeSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BaseGameImpactorImperativeSideScreen : SideScreenContent
{
  private MissileLauncher.Instance targetMissileLauncher;
  [SerializeField]
  private Image healthBarFill;
  [SerializeField]
  private Image timeBarFill;
  [SerializeField]
  private LocText healthBarLabel;
  [SerializeField]
  private LocText timeBarLabel;
  [SerializeField]
  private ToolTip healthBarTooltip;
  [SerializeField]
  private ToolTip timeBarTooltip;
  private LargeImpactorStatus.Instance statusMonitor;

  public override bool IsValidForTarget(GameObject target)
  {
    if (DlcManager.IsExpansion1Active())
      return false;
    MissileLauncher.Instance smi = target.GetSMI<MissileLauncher.Instance>();
    return smi != null && this.StatusMonitor != null && smi.AmmunitionIsAllowed((Tag) "MissileLongRange");
  }

  private LargeImpactorStatus.Instance StatusMonitor
  {
    get
    {
      if (this.statusMonitor == null)
      {
        GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance((HashedString) Db.Get().GameplayEvents.LargeImpactor.Id);
        if (gameplayEventInstance != null)
          this.statusMonitor = ((LargeImpactorEvent.StatesInstance) gameplayEventInstance.smi).impactorInstance.GetSMI<LargeImpactorStatus.Instance>();
      }
      return this.statusMonitor;
    }
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.targetMissileLauncher = target.GetSMI<MissileLauncher.Instance>();
    this.Build();
  }

  private void Build()
  {
    if (this.StatusMonitor == null)
      return;
    this.healthBarFill.fillAmount = Mathf.Max((float) this.StatusMonitor.Health / (float) this.StatusMonitor.def.MAX_HEALTH, 0.0f);
    this.healthBarTooltip.toolTip = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.MISSILESELECTIONSIDESCREEN.VANILLALARGEIMPACTOR.HEALTH_BAR_TOOLTIP, (object) this.StatusMonitor.Health, (object) this.StatusMonitor.def.MAX_HEALTH);
    this.timeBarFill.fillAmount = this.StatusMonitor.TimeRemainingBeforeCollision / LargeImpactorEvent.GetImpactTime();
    this.timeBarTooltip.toolTip = GameUtil.SafeStringFormat((string) STRINGS.UI.UISIDESCREENS.MISSILESELECTIONSIDESCREEN.VANILLALARGEIMPACTOR.TIME_UNTIL_COLLISION_TOOLTIP, (object) GameUtil.GetFormattedCycles(this.StatusMonitor.TimeRemainingBeforeCollision).Split(' ', StringSplitOptions.None)[0]);
  }
}
