// Decompiled with JetBrains decompiler
// Type: DevLifeSupport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DevLifeSupport : KMonoBehaviour, ISim200ms
{
  [MyCmpReq]
  private ElementConsumer elementConsumer;
  public float targetTemperature = 294.15f;
  public int effectRadius = 7;
  private const float temperatureControlK = 0.2f;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!((Object) this.elementConsumer != (Object) null))
      return;
    this.elementConsumer.EnableConsumption(true);
  }

  public void Sim200ms(float dt)
  {
    Vector2I vector2I1 = new Vector2I(-this.effectRadius, -this.effectRadius);
    Vector2I vector2I2 = new Vector2I(this.effectRadius, this.effectRadius);
    int x1;
    int y1;
    Grid.PosToXY(this.transform.GetPosition(), out x1, out y1);
    int cell1 = Grid.XYToCell(x1, y1);
    if (!Grid.IsValidCell(cell1))
      return;
    int world = (int) Grid.WorldIdx[cell1];
    for (int y2 = vector2I1.y; y2 <= vector2I2.y; ++y2)
    {
      for (int x2 = vector2I1.x; x2 <= vector2I2.x; ++x2)
      {
        int cell2 = Grid.XYToCell(x1 + x2, y1 + y2);
        if (Grid.IsValidCellInWorld(cell2, world))
        {
          float b = (this.targetTemperature - Grid.Temperature[cell2]) * Grid.Element[cell2].specificHeatCapacity * Grid.Mass[cell2];
          if (!Mathf.Approximately(0.0f, b))
            SimMessages.ModifyEnergy(cell2, b * 0.2f, 5000f, (double) b > 0.0 ? SimMessages.EnergySourceID.DebugHeat : SimMessages.EnergySourceID.DebugCool);
        }
      }
    }
  }
}
