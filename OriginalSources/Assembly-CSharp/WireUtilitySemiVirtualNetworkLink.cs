// Decompiled with JetBrains decompiler
// Type: WireUtilitySemiVirtualNetworkLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WireUtilitySemiVirtualNetworkLink : 
  UtilityNetworkLink,
  IHaveUtilityNetworkMgr,
  ICircuitConnected
{
  [SerializeField]
  public Wire.WattageRating maxWattageRating;

  public Wire.WattageRating GetMaxWattageRating() => this.maxWattageRating;

  protected override void OnPrefabInit() => base.OnPrefabInit();

  protected override void OnSpawn()
  {
    RocketModuleCluster component1 = this.GetComponent<RocketModuleCluster>();
    if ((Object) component1 != (Object) null)
    {
      this.VirtualCircuitKey = (object) component1.CraftInterface;
    }
    else
    {
      CraftModuleInterface component2 = this.GetMyWorld().GetComponent<CraftModuleInterface>();
      if ((Object) component2 != (Object) null)
        this.VirtualCircuitKey = (object) component2;
    }
    Game.Instance.electricalConduitSystem.AddToVirtualNetworks(this.VirtualCircuitKey, (object) this, true);
    base.OnSpawn();
  }

  public void SetLinkConnected(bool connect)
  {
    if (connect && this.visualizeOnly)
    {
      this.visualizeOnly = false;
      if (!this.isSpawned)
        return;
      this.Connect();
    }
    else
    {
      if (connect || this.visualizeOnly)
        return;
      if (this.isSpawned)
        this.Disconnect();
      this.visualizeOnly = true;
    }
  }

  protected override void OnDisconnect(int cell1, int cell2)
  {
    Game.Instance.electricalConduitSystem.RemoveSemiVirtualLink(cell1, this.VirtualCircuitKey);
  }

  protected override void OnConnect(int cell1, int cell2)
  {
    Game.Instance.electricalConduitSystem.AddSemiVirtualLink(cell1, this.VirtualCircuitKey);
  }

  public IUtilityNetworkMgr GetNetworkManager()
  {
    return (IUtilityNetworkMgr) Game.Instance.electricalConduitSystem;
  }

  public bool IsVirtual { get; private set; }

  public int PowerCell => this.GetNetworkCell();

  public object VirtualCircuitKey { get; private set; }

  public void AddNetworks(ICollection<UtilityNetwork> networks)
  {
    int networkCell = this.GetNetworkCell();
    UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
    if (networkForCell == null)
      return;
    networks.Add(networkForCell);
  }

  public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
  {
    int networkCell = this.GetNetworkCell();
    UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
    return networks.Contains(networkForCell);
  }
}
