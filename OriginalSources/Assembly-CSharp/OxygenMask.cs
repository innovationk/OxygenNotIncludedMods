// Decompiled with JetBrains decompiler
// Type: OxygenMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class OxygenMask : KMonoBehaviour, ISim200ms
{
  private static readonly EventSystem.IntraObjectHandler<OxygenMask> OnSuitTankDeltaDelegate = new EventSystem.IntraObjectHandler<OxygenMask>((Action<OxygenMask, object>) ((component, data) => component.CheckOxygenLevels(data)));
  [MyCmpGet]
  private SuitTank suitTank;
  [MyCmpGet]
  private Storage storage;
  private float leakRate = 0.1f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<OxygenMask>(608245985, OxygenMask.OnSuitTankDeltaDelegate);
  }

  private void CheckOxygenLevels(object data)
  {
    if (!this.suitTank.IsEmpty())
      return;
    Equippable component = this.GetComponent<Equippable>();
    if (component.assignee == null)
      return;
    Ownables soleOwner = component.assignee.GetSoleOwner();
    if (!((UnityEngine.Object) soleOwner != (UnityEngine.Object) null))
      return;
    soleOwner.GetComponent<Equipment>().Unequip(component);
  }

  public void Sim200ms(float dt)
  {
    if (this.GetComponent<Equippable>().assignee == null)
      this.storage.DropSome(this.suitTank.elementTag, Mathf.Min(this.leakRate * dt, this.storage.GetMassAvailable(this.suitTank.elementTag)), true, true);
    if (!this.suitTank.IsEmpty())
      return;
    Util.KDestroyGameObject(this.gameObject);
  }
}
