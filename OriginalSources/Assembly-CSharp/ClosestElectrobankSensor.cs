// Decompiled with JetBrains decompiler
// Type: ClosestElectrobankSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ClosestElectrobankSensor : ClosestPickupableSensor<Electrobank>
{
  private Tag[] bionicIncompatiobleElectrobankTags;

  public ClosestElectrobankSensor(Sensors sensors, bool shouldStartActive)
    : base(sensors, GameTags.ChargedPortableBattery, shouldStartActive)
  {
    this.bionicIncompatiobleElectrobankTags = new Tag[GameTags.BionicIncompatibleBatteries.Count];
    GameTags.BionicIncompatibleBatteries.CopyTo(this.bionicIncompatiobleElectrobankTags, 0);
  }

  public override HashSet<Tag> GetForbbidenTags()
  {
    HashSet<Tag> forbbidenTags1 = base.GetForbbidenTags();
    if (this.bionicIncompatiobleElectrobankTags == null || this.bionicIncompatiobleElectrobankTags.Length == 0)
      return forbbidenTags1;
    HashSet<Tag> forbbidenTags2 = forbbidenTags1;
    foreach (Tag incompatiobleElectrobankTag in this.bionicIncompatiobleElectrobankTags)
    {
      if (!forbbidenTags1.Contains(incompatiobleElectrobankTag))
        forbbidenTags2.Add(incompatiobleElectrobankTag);
    }
    return forbbidenTags2;
  }
}
