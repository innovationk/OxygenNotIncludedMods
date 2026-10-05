// Decompiled with JetBrains decompiler
// Type: EquippableBalloonConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
public class EquippableBalloonConfig : IEquipmentConfig
{
  public const string ID = "EquippableBalloon";

  public EquipmentDef CreateEquipmentDef()
  {
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("EquippableBalloon", EQUIPMENT.TOYS.SLOT, SimHashes.Carbon, EQUIPMENT.TOYS.BALLOON_MASS, EQUIPMENT.VESTS.WARM_VEST_ICON0, (string) null, (string) null, 0, AttributeModifiers, CollisionShape: EntityTemplates.CollisionShape.RECTANGLE, width: 0.75f, height: 0.4f, additional_tags: new Tag[1]
    {
      GameTags.Clothes
    });
    equipmentDef.OnEquipCallBack = new Action<Equippable>(this.OnEquipBalloon);
    equipmentDef.OnUnequipCallBack = new Action<Equippable>(this.OnUnequipBalloon);
    return equipmentDef;
  }

  private void OnEquipBalloon(Equippable eq)
  {
    if (eq.IsNullOrDestroyed() || eq.assignee.IsNullOrDestroyed())
      return;
    Ownables soleOwner = eq.assignee.GetSoleOwner();
    if (soleOwner.IsNullOrDestroyed())
      return;
    KMonoBehaviour target = (KMonoBehaviour) soleOwner.GetComponent<MinionAssignablesProxy>().target;
    Effects component1 = target.GetComponent<Effects>();
    KSelectable component2 = target.GetComponent<KSelectable>();
    if (component1.IsNullOrDestroyed())
      return;
    component1.Add("HasBalloon", false);
    EquippableBalloon component3 = eq.GetComponent<EquippableBalloon>();
    EquippableBalloon.StatesInstance smi = (EquippableBalloon.StatesInstance) component3.GetSMI();
    component2.AddStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon, (object) smi);
    this.SpawnFxInstanceFor(target);
    component3.ApplyBalloonOverrideToBalloonFx();
  }

  private void OnUnequipBalloon(Equippable eq)
  {
    if (!eq.IsNullOrDestroyed() && !eq.assignee.IsNullOrDestroyed())
    {
      Ownables soleOwner = eq.assignee.GetSoleOwner();
      if (soleOwner.IsNullOrDestroyed())
        return;
      MinionAssignablesProxy component1 = soleOwner.GetComponent<MinionAssignablesProxy>();
      if (!component1.target.IsNullOrDestroyed())
      {
        KMonoBehaviour target = (KMonoBehaviour) component1.target;
        Effects component2 = target.GetComponent<Effects>();
        KSelectable component3 = target.GetComponent<KSelectable>();
        if (!component2.IsNullOrDestroyed())
        {
          component2.Remove("HasBalloon");
          component3.RemoveStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon);
          this.DestroyFxInstanceFor(target);
        }
      }
    }
    Util.KDestroyGameObject(eq.gameObject);
  }

  public void DoPostConfigure(GameObject go)
  {
    Equippable equippable = go.GetComponent<Equippable>();
    if (equippable.IsNullOrDestroyed())
      equippable = go.AddComponent<Equippable>();
    equippable.hideInCodex = true;
    equippable.unequippable = false;
    go.AddOrGet<EquippableBalloon>();
  }

  private void SpawnFxInstanceFor(KMonoBehaviour target)
  {
    new BalloonFX.Instance((IStateMachineTarget) target.GetComponent<KMonoBehaviour>()).StartSM();
  }

  private void DestroyFxInstanceFor(KMonoBehaviour target)
  {
    target.GetSMI<BalloonFX.Instance>().StopSM("Unequipped");
  }
}
