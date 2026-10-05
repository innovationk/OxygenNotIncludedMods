// Decompiled with JetBrains decompiler
// Type: MeterScreen_ValueTrackerDisplayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public abstract class MeterScreen_ValueTrackerDisplayer : KMonoBehaviour
{
  public LocText Label;
  public ToolTip Tooltip;
  public GameObject diagnosticGraph;
  public TextStyleSetting ToolTipStyle_Header;
  public TextStyleSetting ToolTipStyle_Property;
  protected Func<List<MinionIdentity>, List<MinionIdentity>> minionListCustomSortOperation;
  private List<MinionIdentity> worldLiveMinionIdentities;

  protected override void OnSpawn()
  {
    this.Tooltip.OnToolTip = new Func<string>(this.OnTooltip);
    base.OnSpawn();
  }

  public void Refresh()
  {
    this.RefreshWorldMinionIdentities();
    this.InternalRefresh();
  }

  protected abstract void InternalRefresh();

  protected abstract string OnTooltip();

  public virtual void OnClick(BaseEventData base_ev_data)
  {
  }

  private void RefreshWorldMinionIdentities()
  {
    this.worldLiveMinionIdentities = new List<MinionIdentity>(Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId).Where<MinionIdentity>((Func<MinionIdentity, bool>) (x => !x.IsNullOrDestroyed())));
  }

  protected virtual List<MinionIdentity> GetWorldMinionIdentities()
  {
    if (this.worldLiveMinionIdentities == null)
      this.RefreshWorldMinionIdentities();
    if (this.minionListCustomSortOperation != null)
      this.worldLiveMinionIdentities = this.minionListCustomSortOperation(this.worldLiveMinionIdentities);
    return this.worldLiveMinionIdentities;
  }

  protected virtual List<MinionIdentity> GetAllMinionsFromAllWorlds()
  {
    List<MinionIdentity> minionsFromAllWorlds = new List<MinionIdentity>(Components.LiveMinionIdentities.Items.Where<MinionIdentity>((Func<MinionIdentity, bool>) (x => !x.IsNullOrDestroyed())));
    if (this.minionListCustomSortOperation != null)
      this.worldLiveMinionIdentities = this.minionListCustomSortOperation(minionsFromAllWorlds);
    return minionsFromAllWorlds;
  }
}
