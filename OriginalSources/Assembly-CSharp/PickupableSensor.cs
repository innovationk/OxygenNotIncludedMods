// Decompiled with JetBrains decompiler
// Type: PickupableSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class PickupableSensor : Sensor
{
  private Navigator navigator;
  private WorkerBase worker;

  public PickupableSensor(Sensors sensors)
    : base(sensors)
  {
    this.worker = this.GetComponent<WorkerBase>();
    this.navigator = this.GetComponent<Navigator>();
  }

  public override void Update()
  {
    GlobalChoreProvider.Instance.UpdateFetches(this.navigator);
    Game.Instance.fetchManager.UpdatePickups(this.navigator, this.worker);
  }
}
