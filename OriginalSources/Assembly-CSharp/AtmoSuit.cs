// Decompiled with JetBrains decompiler
// Type: AtmoSuit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/AtmoSuit")]
public class AtmoSuit : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<AtmoSuit> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<AtmoSuit>((Action<AtmoSuit, object>) ((component, data) => component.RefreshStatusEffects(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<AtmoSuit>(-1697596308, AtmoSuit.OnStorageChangedDelegate);
  }

  private void RefreshStatusEffects(object data)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    Equippable component1 = this.GetComponent<Equippable>();
    Storage component2 = this.GetComponent<Storage>();
    bool flag = component2.Has(GameTags.AnyWater) || component2.Has(SimHashes.LiquidGunk.CreateTag());
    if (!(component1.assignee != null & flag))
      return;
    Ownables soleOwner = component1.assignee.GetSoleOwner();
    if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
      return;
    GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    if (!(bool) (UnityEngine.Object) targetGameObject)
      return;
    AssignableSlotInstance slot = ((Component) component1.assignee).GetComponent<Equipment>().GetSlot(component1.slot);
    Effects component3 = targetGameObject.GetComponent<Effects>();
    if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null) || component3.HasEffect("SoiledSuit") || slot.IsUnassigning())
      return;
    component3.Add("SoiledSuit", true);
  }
}
