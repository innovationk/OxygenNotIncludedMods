// Decompiled with JetBrains decompiler
// Type: ConduitBridgeBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class ConduitBridgeBase : KMonoBehaviour
{
  public ConduitBridgeBase.DesiredMassTransfer desiredMassTransfer;
  public ConduitBridgeBase.ConduitBridgeEvent OnMassTransfer;

  protected void SendEmptyOnMassTransfer()
  {
    if (this.OnMassTransfer == null)
      return;
    this.OnMassTransfer(SimHashes.Void, 0.0f, 0.0f, (byte) 0, 0, (Pickupable) null);
  }

  public delegate float DesiredMassTransfer(
    float dt,
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    Pickupable pickupable);

  public delegate void ConduitBridgeEvent(
    SimHashes element,
    float mass,
    float temperature,
    byte disease_idx,
    int disease_count,
    Pickupable pickupable);
}
