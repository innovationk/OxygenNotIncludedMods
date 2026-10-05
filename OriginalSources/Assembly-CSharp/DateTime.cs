// Decompiled with JetBrains decompiler
// Type: DateTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DateTime : KScreen
{
  public static DateTime Instance;
  private const string MILESTONE_ANTICIPATION_ANIMATION_NAME = "100fx_pre";
  private const string MILESTONE_ANIMATION_NAME = "100fx";
  public LocText day;
  private int displayedDayCount = -1;
  [SerializeField]
  private KBatchedAnimController milestoneEffect;
  [SerializeField]
  private LocText text;
  [SerializeField]
  private ToolTip tooltip;
  [SerializeField]
  private TextStyleSetting tooltipstyle_Days;
  [SerializeField]
  private TextStyleSetting tooltipstyle_Playtime;
  [SerializeField]
  public KToggle scheduleToggle;

  public static void DestroyInstance() => DateTime.Instance = (DateTime) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DateTime.Instance = this;
    this.milestoneEffect.gameObject.SetActive(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.tooltip.OnComplexToolTip = new ToolTip.ComplexTooltipDelegate(this.BuildTooltip);
    Game.Instance.Subscribe(2070437606, new Action<object>(this.OnMilestoneDayReached));
    Game.Instance.Subscribe(-720092972, new Action<object>(this.OnMilestoneDayApproaching));
  }

  private List<Tuple<string, TextStyleSetting>> BuildTooltip()
  {
    List<Tuple<string, TextStyleSetting>> colonyToolTip = SaveGame.Instance.GetColonyToolTip();
    if (TimeOfDay.IsMilestoneApproaching)
    {
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>(" ", (TextStyleSetting) null));
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_TITLE.text, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
      colonyToolTip.Add(new Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_DESCRIPTION.text.Replace("{0}", (GameClock.Instance.GetCycle() + 2).ToString()), ToolTipScreen.Instance.defaultTooltipBodyStyle));
    }
    return colonyToolTip;
  }

  private void Update()
  {
    if (!((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null) || this.displayedDayCount == GameUtil.GetCurrentCycle())
      return;
    this.text.text = this.Days();
    this.displayedDayCount = GameUtil.GetCurrentCycle();
  }

  private void OnMilestoneDayApproaching(object data)
  {
    int num = ((Boxed<int>) data).value;
    this.milestoneEffect.gameObject.SetActive(true);
    this.milestoneEffect.Play((HashedString) "100fx_pre", KAnim.PlayMode.Loop);
  }

  private void OnMilestoneDayReached(object data)
  {
    int num = ((Boxed<int>) data).value;
    this.milestoneEffect.gameObject.SetActive(true);
    this.milestoneEffect.Play((HashedString) "100fx");
  }

  private string Days() => GameUtil.GetCurrentCycle().ToString();
}
