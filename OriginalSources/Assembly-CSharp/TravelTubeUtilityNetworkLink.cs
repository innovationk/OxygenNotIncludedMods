// Decompiled with JetBrains decompiler
// Type: TravelTubeUtilityNetworkLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class TravelTubeUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr
{
  protected override void OnSpawn() => base.OnSpawn();

  protected override void OnConnect(int cell1, int cell2)
  {
    Game.Instance.travelTubeSystem.AddLink(cell1, cell2);
  }

  protected override void OnDisconnect(int cell1, int cell2)
  {
    Game.Instance.travelTubeSystem.RemoveLink(cell1, cell2);
  }

  public IUtilityNetworkMgr GetNetworkManager()
  {
    return (IUtilityNetworkMgr) Game.Instance.travelTubeSystem;
  }
}
