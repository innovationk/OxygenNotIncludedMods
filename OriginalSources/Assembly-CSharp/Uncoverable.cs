// Decompiled with JetBrains decompiler
// Type: Uncoverable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/Uncoverable")]
public class Uncoverable : KMonoBehaviour
{
  [MyCmpReq]
  private OccupyArea occupyArea;
  [Serialize]
  private bool hasBeenUncovered;
  private HandleVector<int>.Handle partitionerEntry;
  private static readonly Func<int, object, bool> IsCellBlockedDelegate = (Func<int, object, bool>) ((cell, data) => Uncoverable.IsCellBlocked(cell, data));

  public bool IsUncovered => this.hasBeenUncovered;

  private bool IsAnyCellShowing()
  {
    return !this.occupyArea.TestArea(Grid.PosToCell((KMonoBehaviour) this), (object) null, Uncoverable.IsCellBlockedDelegate);
  }

  private static bool IsCellBlocked(int cell, object data)
  {
    return Grid.Element[cell].IsSolid && !Grid.Foundation[cell];
  }

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.IsAnyCellShowing())
      this.hasBeenUncovered = true;
    if (this.hasBeenUncovered)
      return;
    this.GetComponent<KSelectable>().IsSelectable = false;
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Uncoverable.OnSpawn", (object) this.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
  }

  private void OnSolidChanged(object data)
  {
    if (!this.IsAnyCellShowing() || this.hasBeenUncovered || !this.partitionerEntry.IsValid())
      return;
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.hasBeenUncovered = true;
    this.GetComponent<KSelectable>().IsSelectable = true;
    Notification notification = new Notification((string) MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION, NotificationType.Good, new Func<List<Notification>, object, string>(Uncoverable.OnNotificationToolTip), (object) this);
    this.gameObject.AddOrGet<Notifier>().Add(notification);
  }

  private static string OnNotificationToolTip(List<Notification> notifications, object data)
  {
    Uncoverable cmp = (Uncoverable) data;
    return MISC.STATUSITEMS.BURIEDITEM.NOTIFICATION_TOOLTIP.Replace("{Uncoverable}", cmp.GetProperName());
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }
}
