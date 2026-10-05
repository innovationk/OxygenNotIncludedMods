// Decompiled with JetBrains decompiler
// Type: SuitEquipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/scripts/SuitEquipper")]
public class SuitEquipper : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<SuitEquipper> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitEquipper>((Action<SuitEquipper, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SuitEquipper>(493375141, SuitEquipper.OnRefreshUserMenuDelegate);
  }

  private void OnRefreshUserMenu(object data)
  {
    foreach (EquipmentSlotInstance slot in this.GetComponent<MinionIdentity>().GetEquipment().Slots)
    {
      Equippable equippable = slot.assignable as Equippable;
      if ((bool) (UnityEngine.Object) equippable && equippable.unequippable)
        Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("iconDown", string.Format((string) UI.USERMENUACTIONS.UNEQUIP.NAME, (object) equippable.def.GenericName), (System.Action) (() => equippable.Unassign())), 2f);
    }
  }

  public Equippable IsWearingAirtightSuit()
  {
    Equippable equippable = (Equippable) null;
    foreach (AssignableSlotInstance slot in this.GetComponent<MinionIdentity>().GetEquipment().Slots)
    {
      Equippable assignable = slot.assignable as Equippable;
      if ((bool) (UnityEngine.Object) assignable && assignable.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit) && assignable.isEquipped)
      {
        equippable = assignable;
        break;
      }
    }
    return equippable;
  }
}
