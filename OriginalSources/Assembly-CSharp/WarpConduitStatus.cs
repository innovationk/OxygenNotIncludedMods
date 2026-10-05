// Decompiled with JetBrains decompiler
// Type: WarpConduitStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class WarpConduitStatus
{
  public static readonly Operational.Flag warpConnectedFlag = new Operational.Flag("warp_conduit_connected", Operational.Flag.Type.Requirement);

  public static void UpdateWarpConduitsOperational(GameObject sender, GameObject receiver)
  {
    int num = !((Object) sender != (Object) null) ? 0 : (sender.GetComponent<Activatable>().IsActivated ? 1 : 0);
    bool flag1 = (Object) receiver != (Object) null && receiver.GetComponent<Activatable>().IsActivated;
    bool flag2 = (num & (flag1 ? 1 : 0)) != 0;
    int data = 0;
    if (num != 0)
      ++data;
    if (flag1)
      ++data;
    if ((Object) sender != (Object) null)
    {
      sender.GetComponent<Operational>().SetFlag(WarpConduitStatus.warpConnectedFlag, flag2);
      if (data != 2)
      {
        sender.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled);
        sender.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, (object) data);
      }
      else
        sender.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled);
    }
    if (!((Object) receiver != (Object) null))
      return;
    receiver.GetComponent<Operational>().SetFlag(WarpConduitStatus.warpConnectedFlag, flag2);
    if (data != 2)
    {
      receiver.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled);
      receiver.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled, (object) data);
    }
    else
      receiver.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.WarpConduitPartnerDisabled);
  }
}
