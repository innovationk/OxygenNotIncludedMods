// Decompiled with JetBrains decompiler
// Type: ModuleBattery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class ModuleBattery : Battery
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.connectedTags = new Tag[0];
    this.IsVirtual = true;
  }

  protected override void OnSpawn()
  {
    this.VirtualCircuitKey = (object) this.GetComponent<RocketModuleCluster>().CraftInterface;
    base.OnSpawn();
    this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
  }
}
