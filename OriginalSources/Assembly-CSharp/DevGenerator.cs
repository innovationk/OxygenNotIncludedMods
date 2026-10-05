// Decompiled with JetBrains decompiler
// Type: DevGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DevGenerator : Generator
{
  public float wattageRating = 100000f;

  public override void EnergySim200ms(float dt)
  {
    base.EnergySim200ms(dt);
    ushort circuitId = this.CircuitID;
    this.operational.SetFlag(Generator.wireConnectedFlag, circuitId != ushort.MaxValue);
    if (!this.operational.IsOperational)
      return;
    float wattageRating = this.wattageRating;
    if ((double) wattageRating <= 0.0)
      return;
    this.GenerateJoules(Mathf.Max(wattageRating * dt, 1f * dt));
  }
}
