// Decompiled with JetBrains decompiler
// Type: GasBreatherFromWorldProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
public class GasBreatherFromWorldProvider : OxygenBreather.IGasProvider
{
  public static CellOffset[] DEFAULT_BREATHABLE_OFFSETS = new CellOffset[6]
  {
    new CellOffset(0, 0),
    new CellOffset(0, 1),
    new CellOffset(1, 1),
    new CellOffset(-1, 1),
    new CellOffset(1, 0),
    new CellOffset(-1, 0)
  };
  private OxygenBreather oxygenBreather;
  private Navigator nav;
  private static Action<Sim.MassConsumedCallback, object> OnSimConsumeCallbackAction = new Action<Sim.MassConsumedCallback, object>(GasBreatherFromWorldProvider.OnSimConsumeCallback);

  public GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAtCurrentLocation()
  {
    return GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(Grid.PosToCell((KMonoBehaviour) this.oxygenBreather), GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, this.oxygenBreather);
  }

  public static GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAroundSpecificCell(
    int theSpecificCell,
    CellOffset[] breathRange,
    OxygenBreather breather)
  {
    return GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(theSpecificCell, breathRange, breather, out float _);
  }

  public static GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAroundSpecificCell(
    int theSpecificCell,
    CellOffset[] breathRange,
    OxygenBreather breather,
    out float totalBreathableMassAroundCell)
  {
    if (breathRange == null)
      breathRange = GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS;
    float num1 = 0.0f;
    int num2 = theSpecificCell;
    SimHashes simHashes = SimHashes.Vacuum;
    totalBreathableMassAroundCell = 0.0f;
    foreach (CellOffset offset in breathRange)
    {
      int cell = Grid.OffsetCell(theSpecificCell, offset);
      SimHashes elementID;
      float breathableCellMass = GasBreatherFromWorldProvider.GetBreathableCellMass(cell, out elementID);
      totalBreathableMassAroundCell += breathableCellMass;
      if ((double) breathableCellMass > (double) num1 && (double) breathableCellMass > (double) breather.noOxygenThreshold)
      {
        num1 = breathableCellMass;
        num2 = cell;
        simHashes = elementID;
      }
    }
    return new GasBreatherFromWorldProvider.BreathableCellData()
    {
      Cell = num2,
      ElementID = simHashes,
      Mass = num1,
      IsBreathable = simHashes != SimHashes.Vacuum
    };
  }

  private static float GetBreathableCellMass(int cell, out SimHashes elementID)
  {
    elementID = SimHashes.Vacuum;
    if (Grid.IsValidCell(cell))
    {
      Element element = Grid.Element[cell];
      if (element.HasTag(GameTags.Breathable))
      {
        elementID = element.id;
        return Grid.Mass[cell];
      }
    }
    return 0.0f;
  }

  public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
  {
    this.oxygenBreather = oxygen_breather;
    this.nav = this.oxygenBreather.GetComponent<Navigator>();
  }

  public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
  {
  }

  public bool ShouldEmitCO2() => this.nav.CurrentNavType != NavType.Tube;

  public bool ShouldStoreCO2() => false;

  public bool IsLowOxygen()
  {
    GasBreatherFromWorldProvider.BreathableCellData atCurrentLocation = this.GetBestBreathableCellAtCurrentLocation();
    return atCurrentLocation.IsBreathable && (double) atCurrentLocation.Mass < (double) this.oxygenBreather.lowOxygenThreshold;
  }

  public bool HasOxygen()
  {
    return this.oxygenBreather.prefabID.HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.prefabID.HasTag(GameTags.InTransitTube) || this.GetBestBreathableCellAtCurrentLocation().IsBreathable;
  }

  public bool IsBlocked() => this.oxygenBreather.HasTag(GameTags.HasSuitTank);

  public bool ConsumeGas(OxygenBreather oxygen_breather, float mass_to_consume)
  {
    if (this.nav.CurrentNavType != NavType.Tube)
    {
      GasBreatherFromWorldProvider.BreathableCellData atCurrentLocation = this.GetBestBreathableCellAtCurrentLocation();
      if (!atCurrentLocation.IsBreathable)
        return false;
      SimHashes elementId = atCurrentLocation.ElementID;
      HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(GasBreatherFromWorldProvider.OnSimConsumeCallbackAction, (object) oxygen_breather, nameof (GasBreatherFromWorldProvider));
      SimMessages.ConsumeMass(atCurrentLocation.Cell, elementId, mass_to_consume, (byte) 3, handle.index);
    }
    return true;
  }

  private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
  {
    SimHashes id = ElementLoader.elements[(int) mass_cb_info.elemIdx].id;
    OxygenBreather.BreathableGasConsumed(data as OxygenBreather, id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
  }

  public struct BreathableCellData
  {
    public int Cell;
    public SimHashes ElementID;
    public float Mass;
    public bool IsBreathable;
  }
}
