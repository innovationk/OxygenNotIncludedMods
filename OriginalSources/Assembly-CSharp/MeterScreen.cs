// Decompiled with JetBrains decompiler
// Type: MeterScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class MeterScreen : KScreen, IRender1000ms
{
  [SerializeField]
  private LocText currentMinions;
  public ToolTip MinionsTooltip;
  public MeterScreen_ValueTrackerDisplayer[] valueDisplayers;
  public TextStyleSetting ToolTipStyle_Header;
  public TextStyleSetting ToolTipStyle_Property;
  private bool startValuesSet;
  public MultiToggle RedAlertButton;
  public ToolTip RedAlertTooltip;
  private MeterScreen.DisplayInfo immunityDisplayInfo = new MeterScreen.DisplayInfo()
  {
    selectedIndex = -1
  };
  private List<MinionIdentity> worldLiveMinionIdentities;
  private int cachedMinionCount = -1;

  public static MeterScreen Instance { get; private set; }

  public static void DestroyInstance() => MeterScreen.Instance = (MeterScreen) null;

  public bool StartValuesSet => this.startValuesSet;

  protected override void OnPrefabInit() => MeterScreen.Instance = this;

  protected override void OnSpawn()
  {
    this.RedAlertTooltip.OnToolTip = new Func<string>(this.OnRedAlertTooltip);
    this.RedAlertButton.onClick += (System.Action) (() => this.OnRedAlertClick());
    Game.Instance.Subscribe(1983128072, (Action<object>) (data => this.Refresh()));
    Game.Instance.Subscribe(1585324898, (Action<object>) (data => this.RefreshRedAlertButtonState()));
    Game.Instance.Subscribe(-1393151672, (Action<object>) (data => this.RefreshRedAlertButtonState()));
  }

  private void OnRedAlertClick()
  {
    bool on = !ClusterManager.Instance.activeWorld.AlertManager.IsRedAlertToggledOn();
    ClusterManager.Instance.activeWorld.AlertManager.ToggleRedAlert(on);
    if (on)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open"));
    else
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close"));
  }

  private void RefreshRedAlertButtonState()
  {
    this.RedAlertButton.ChangeState(ClusterManager.Instance.activeWorld.IsRedAlert() ? 1 : 0);
  }

  public void Render1000ms(float dt) => this.Refresh();

  public void InitializeValues()
  {
    if (this.startValuesSet)
      return;
    this.startValuesSet = true;
    this.Refresh();
  }

  private void Refresh()
  {
    this.RefreshWorldMinionIdentities();
    this.RefreshMinions();
    for (int index = 0; index < this.valueDisplayers.Length; ++index)
      this.valueDisplayers[index].Refresh();
    this.RefreshRedAlertButtonState();
  }

  private void RefreshWorldMinionIdentities()
  {
    this.worldLiveMinionIdentities = new List<MinionIdentity>(Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId).Where<MinionIdentity>((Func<MinionIdentity, bool>) (x => !x.IsNullOrDestroyed())));
  }

  private List<MinionIdentity> GetWorldMinionIdentities()
  {
    if (this.worldLiveMinionIdentities == null)
      this.RefreshWorldMinionIdentities();
    return this.worldLiveMinionIdentities;
  }

  private void RefreshMinions()
  {
    int count1 = Components.LiveMinionIdentities.Count;
    int count2 = this.GetWorldMinionIdentities().Count;
    if (count2 == this.cachedMinionCount)
      return;
    this.cachedMinionCount = count2;
    string newString;
    if (DlcManager.FeatureClusterSpaceEnabled())
    {
      ClusterGridEntity component = ClusterManager.Instance.activeWorld.GetComponent<ClusterGridEntity>();
      newString = string.Format((string) UI.TOOLTIPS.METERSCREEN_POPULATION_CLUSTER, (object) component.Name, (object) count2, (object) count1);
      this.currentMinions.text = $"{count2}/{count1}";
    }
    else
    {
      this.currentMinions.text = $"{count1}";
      newString = string.Format((string) UI.TOOLTIPS.METERSCREEN_POPULATION, (object) count1.ToString("0"));
    }
    this.MinionsTooltip.ClearMultiStringTooltip();
    this.MinionsTooltip.AddMultiStringTooltip(newString, this.ToolTipStyle_Header);
  }

  private string OnRedAlertTooltip()
  {
    this.RedAlertTooltip.ClearMultiStringTooltip();
    this.RedAlertTooltip.AddMultiStringTooltip((string) UI.TOOLTIPS.RED_ALERT_TITLE, this.ToolTipStyle_Header);
    this.RedAlertTooltip.AddMultiStringTooltip((string) UI.TOOLTIPS.RED_ALERT_CONTENT, this.ToolTipStyle_Property);
    return "";
  }

  private struct DisplayInfo
  {
    public int selectedIndex;
  }
}
