// Decompiled with JetBrains decompiler
// Type: PlantAirConditioner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantAirConditioner : AirConditioner
{
  private static readonly Operational.Flag fertilizedFlag = new Operational.Flag("fertilized", Operational.Flag.Type.Requirement);
  private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnFertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>((Action<PlantAirConditioner, object>) ((component, data) => component.OnFertilized(data)));
  private static readonly EventSystem.IntraObjectHandler<PlantAirConditioner> OnUnfertilizedDelegate = new EventSystem.IntraObjectHandler<PlantAirConditioner>((Action<PlantAirConditioner, object>) ((component, data) => component.OnUnfertilized(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<PlantAirConditioner>(-1396791468, PlantAirConditioner.OnFertilizedDelegate);
    this.Subscribe<PlantAirConditioner>(-1073674739, PlantAirConditioner.OnUnfertilizedDelegate);
  }

  private void OnFertilized(object data)
  {
    this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, true);
  }

  private void OnUnfertilized(object data)
  {
    this.operational.SetFlag(PlantAirConditioner.fertilizedFlag, false);
  }
}
