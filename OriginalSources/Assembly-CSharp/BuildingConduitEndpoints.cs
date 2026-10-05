// Decompiled with JetBrains decompiler
// Type: BuildingConduitEndpoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConduitEndpoints")]
public class BuildingConduitEndpoints : KMonoBehaviour
{
  private FlowUtilityNetwork.NetworkItem itemInput;
  private FlowUtilityNetwork.NetworkItem itemOutput;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.AddEndpoint();
  }

  protected override void OnCleanUp()
  {
    this.RemoveEndPoint();
    base.OnCleanUp();
  }

  public void RemoveEndPoint()
  {
    if (this.itemInput != null)
    {
      if (this.itemInput.ConduitType == ConduitType.Solid)
        Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemInput.Cell, (object) this.itemInput, true);
      else
        Conduit.GetNetworkManager(this.itemInput.ConduitType).RemoveFromNetworks(this.itemInput.Cell, (object) this.itemInput, true);
      this.itemInput = (FlowUtilityNetwork.NetworkItem) null;
    }
    if (this.itemOutput == null)
      return;
    if (this.itemOutput.ConduitType == ConduitType.Solid)
      Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemOutput.Cell, (object) this.itemOutput, true);
    else
      Conduit.GetNetworkManager(this.itemOutput.ConduitType).RemoveFromNetworks(this.itemOutput.Cell, (object) this.itemOutput, true);
    this.itemOutput = (FlowUtilityNetwork.NetworkItem) null;
  }

  public void AddEndpoint()
  {
    Building component = this.GetComponent<Building>();
    BuildingDef def = component.Def;
    this.RemoveEndPoint();
    if (def.InputConduitType != ConduitType.None)
    {
      int utilityInputCell = component.GetUtilityInputCell();
      this.itemInput = new FlowUtilityNetwork.NetworkItem(def.InputConduitType, Endpoint.Sink, utilityInputCell, this.gameObject);
      if (def.InputConduitType == ConduitType.Solid)
        Game.Instance.solidConduitSystem.AddToNetworks(utilityInputCell, (object) this.itemInput, true);
      else
        Conduit.GetNetworkManager(def.InputConduitType).AddToNetworks(utilityInputCell, (object) this.itemInput, true);
    }
    if (def.OutputConduitType == ConduitType.None)
      return;
    int utilityOutputCell = component.GetUtilityOutputCell();
    this.itemOutput = new FlowUtilityNetwork.NetworkItem(def.OutputConduitType, Endpoint.Source, utilityOutputCell, this.gameObject);
    if (def.OutputConduitType == ConduitType.Solid)
      Game.Instance.solidConduitSystem.AddToNetworks(utilityOutputCell, (object) this.itemOutput, true);
    else
      Conduit.GetNetworkManager(def.OutputConduitType).AddToNetworks(utilityOutputCell, (object) this.itemOutput, true);
  }
}
