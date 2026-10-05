// Decompiled with JetBrains decompiler
// Type: FoodRehydrator.ResourceRequirementMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace FoodRehydrator;

public class ResourceRequirementMonitor : KMonoBehaviour
{
  [MyCmpReq]
  private Operational operational;
  private Storage packages;
  private Storage water;
  private static readonly Operational.Flag flag = new Operational.Flag("HasSufficientResources", Operational.Flag.Type.Requirement);
  private static readonly EventSystem.IntraObjectHandler<ResourceRequirementMonitor> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ResourceRequirementMonitor>((Action<ResourceRequirementMonitor, object>) ((component, data) => component.OnStorageChanged(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Storage[] components = this.GetComponents<Storage>();
    DebugUtil.DevAssert(components.Length == 2, "Incorrect number of storages on foodrehydrator");
    this.packages = components[0];
    this.water = components[1];
    this.Subscribe<ResourceRequirementMonitor>(-1697596308, ResourceRequirementMonitor.OnStorageChangedDelegate);
  }

  protected float GetAvailableWater() => this.water.GetMassAvailable(GameTags.Water);

  protected bool HasSufficientResources()
  {
    return this.packages.items.Count > 0 && (double) this.GetAvailableWater() > 1.0;
  }

  protected void OnStorageChanged(object _)
  {
    this.operational.SetFlag(ResourceRequirementMonitor.flag, this.HasSufficientResources());
  }
}
