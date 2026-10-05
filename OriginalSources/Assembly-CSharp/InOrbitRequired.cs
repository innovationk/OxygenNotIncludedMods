// Decompiled with JetBrains decompiler
// Type: InOrbitRequired
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InOrbitRequired")]
public class InOrbitRequired : KMonoBehaviour, IGameObjectEffectDescriptor
{
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private Operational operational;
  public static readonly Operational.Flag inOrbitFlag = new Operational.Flag("in_orbit", Operational.Flag.Type.Requirement);
  private CraftModuleInterface craftModuleInterface;

  protected override void OnSpawn()
  {
    this.craftModuleInterface = this.GetMyWorld().GetComponent<CraftModuleInterface>();
    base.OnSpawn();
    this.UpdateFlag(this.craftModuleInterface.HasTag(GameTags.RocketNotOnGround));
    this.craftModuleInterface.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
  }

  protected override void OnCleanUp()
  {
    if (!((UnityEngine.Object) this.craftModuleInterface != (UnityEngine.Object) null))
      return;
    this.craftModuleInterface.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
  }

  private void OnTagsChanged(object data)
  {
    TagChangedEventData changedEventData = ((Boxed<TagChangedEventData>) data).value;
    if (!(changedEventData.tag == GameTags.RocketNotOnGround))
      return;
    this.UpdateFlag(changedEventData.added);
  }

  private void UpdateFlag(bool newInOrbit)
  {
    this.operational.SetFlag(InOrbitRequired.inOrbitFlag, newInOrbit);
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InOrbitRequired, !newInOrbit, (object) this);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor((string) UI.BUILDINGEFFECTS.IN_ORBIT_REQUIRED, (string) UI.BUILDINGEFFECTS.TOOLTIPS.IN_ORBIT_REQUIRED, Descriptor.DescriptorType.Requirement)
    };
  }
}
