// Decompiled with JetBrains decompiler
// Type: IEnergyConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public interface IEnergyConsumer : ICircuitConnected
{
  float WattsUsed { get; }

  float WattsNeededWhenActive { get; }

  int PowerSortOrder { get; }

  void SetConnectionStatus(CircuitManager.ConnectionStatus status);

  string Name { get; }

  bool IsConnected { get; }

  bool IsPowered { get; }
}
