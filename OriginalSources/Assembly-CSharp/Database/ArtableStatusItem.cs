// Decompiled with JetBrains decompiler
// Type: Database.ArtableStatusItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Database;

public class ArtableStatusItem : StatusItem
{
  public ArtableStatuses.ArtableStatusType StatusType;

  public ArtableStatusItem(string id, ArtableStatuses.ArtableStatusType statusType)
    : base(id, "BUILDING", "", StatusItem.IconType.Info, statusType == ArtableStatuses.ArtableStatusType.AwaitingArting ? NotificationType.BadMinor : NotificationType.Neutral, false, OverlayModes.None.ID)
  {
    this.StatusType = statusType;
  }
}
