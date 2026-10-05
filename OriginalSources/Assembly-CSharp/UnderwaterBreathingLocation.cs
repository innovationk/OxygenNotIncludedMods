// Decompiled with JetBrains decompiler
// Type: UnderwaterBreathingLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UnderwaterBreathingLocation : KMonoBehaviour
{
  [MyCmpGet]
  public Storage storage;
  [MyCmpAdd]
  private Reservable reservable;
  public bool allowLandUse;

  public int breathableCell { get; private set; }

  public float GetAvailableBreathableMass()
  {
    PrimaryElement firstWithMass = this.storage.FindFirstWithMass(GameTags.Breathable);
    return !((Object) firstWithMass != (Object) null) ? 0.0f : firstWithMass.Mass;
  }

  public void MarkCells()
  {
    Components.UnderwaterBreathingLocations.Add(this);
    this.breathableCell = Grid.PosToCell((KMonoBehaviour) this);
  }

  public void UnmarkCells()
  {
    this.breathableCell = -1;
    Components.UnderwaterBreathingLocations.Remove(this);
  }

  public bool ReserveLocation(GameObject reserver, bool reserve)
  {
    bool flag = false;
    if (reserve)
      flag = this.reservable.Reserve(reserver);
    else if (this.reservable.IsReservableBy(reserver))
    {
      this.reservable.ClearReservation();
      flag = true;
    }
    return flag;
  }

  public bool CanReserve(GameObject reserver)
  {
    return !this.reservable.IsReserved || this.reservable.IsReservableBy(reserver);
  }
}
