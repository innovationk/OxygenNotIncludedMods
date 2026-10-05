// Decompiled with JetBrains decompiler
// Type: RocketModulePerformance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class RocketModulePerformance
{
  public float burden;
  public float fuelKilogramPerDistance;
  public float enginePower;

  public RocketModulePerformance(float burden, float fuelKilogramPerDistance, float enginePower)
  {
    this.burden = burden;
    this.fuelKilogramPerDistance = fuelKilogramPerDistance;
    this.enginePower = enginePower;
  }

  public float Burden => this.burden;

  public float FuelKilogramPerDistance => this.fuelKilogramPerDistance;

  public float EnginePower => this.enginePower;
}
