// Decompiled with JetBrains decompiler
// Type: Equipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using KSerialization;
using System;
using TUNING;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class Equipment : Assignables
{
  private SchedulerHandle refreshHandle;
  private static readonly EventSystem.IntraObjectHandler<Equipment> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equipment>((Action<Equipment, object>) ((component, data) => component.destroyed = true));

  public bool destroyed { get; private set; }

  public GameObject GetTargetGameObject()
  {
    MinionAssignablesProxy assignableIdentity = (MinionAssignablesProxy) this.GetAssignableIdentity();
    return (bool) (UnityEngine.Object) assignableIdentity ? assignableIdentity.GetTargetGameObject() : (GameObject) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Equipment.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Equipment>(1502190696, Equipment.SetDestroyedTrueDelegate);
    this.Subscribe<Equipment>(1969584890, Equipment.SetDestroyedTrueDelegate);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.refreshHandle.ClearScheduler();
    Components.Equipment.Remove(this);
  }

  public void Equip(Equippable equippable)
  {
    GameObject targetGameObject = this.GetTargetGameObject();
    bool flag = (UnityEngine.Object) targetGameObject.GetComponent<KBatchedAnimController>() == (UnityEngine.Object) null;
    if (!flag)
    {
      PrimaryElement component1 = equippable.GetComponent<PrimaryElement>();
      SimUtil.DiseaseInfo invalid1 = SimUtil.DiseaseInfo.Invalid with
      {
        idx = component1.DiseaseIdx,
        count = (int) ((double) component1.DiseaseCount * 0.33000001311302185)
      };
      PrimaryElement component2 = targetGameObject.GetComponent<PrimaryElement>();
      SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid with
      {
        idx = component2.DiseaseIdx,
        count = (int) ((double) component2.DiseaseCount * 0.33000001311302185)
      };
      component2.ModifyDiseaseCount(-invalid2.count, "Equipment.Equip");
      component1.ModifyDiseaseCount(-invalid1.count, "Equipment.Equip");
      if (invalid2.count > 0)
        component1.AddDisease(invalid2.idx, invalid2.count, "Equipment.Equip");
      if (invalid1.count > 0)
        component2.AddDisease(invalid1.idx, invalid1.count, "Equipment.Equip");
    }
    AssignableSlotInstance slot = this.GetSlot(equippable.slot);
    slot.Assign((Assignable) equippable);
    Debug.Assert((bool) (UnityEngine.Object) targetGameObject, (object) "GetTargetGameObject returned null in Equip");
    targetGameObject.Trigger(-448952673, (object) equippable.GetComponent<KPrefabID>());
    equippable.Trigger(-1617557748, (object) this);
    Attributes attributes = targetGameObject.GetAttributes();
    if (attributes != null)
    {
      foreach (AttributeModifier attributeModifier in equippable.def.AttributeModifiers)
        attributes.Add(attributeModifier);
    }
    SnapOn component3 = targetGameObject.GetComponent<SnapOn>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
    {
      component3.AttachSnapOnByName(equippable.def.SnapOn);
      if (equippable.def.SnapOn1 != null)
        component3.AttachSnapOnByName(equippable.def.SnapOn1);
    }
    if ((bool) (UnityEngine.Object) equippable.transform.parent)
    {
      Storage component4 = equippable.transform.parent.GetComponent<Storage>();
      if ((bool) (UnityEngine.Object) component4)
        component4.Drop(equippable.gameObject, true);
    }
    equippable.transform.parent = slot.gameObject.transform;
    equippable.transform.SetLocalPosition(Vector3.zero);
    equippable.GetComponent<Pickupable>().UpdateCachedCell(Grid.PosToCell((KMonoBehaviour) equippable));
    this.SetEquippableStoredModifiers(equippable, true);
    equippable.OnEquip(slot);
    if ((double) this.refreshHandle.TimeRemaining > 0.0)
    {
      Debug.LogWarning((object) (targetGameObject.GetProperName() + " is already in the process of changing equipment (equip)"));
      this.refreshHandle.ClearScheduler();
    }
    CreatureSimTemperatureTransfer transferer = targetGameObject.GetComponent<CreatureSimTemperatureTransfer>();
    if (!flag)
      this.refreshHandle = GameScheduler.Instance.Schedule("ChangeEquipment", 2f, (Action<object>) (obj =>
      {
        if (!((UnityEngine.Object) transferer != (UnityEngine.Object) null))
          return;
        transferer.RefreshRegistration();
      }), (object) null, (SchedulerGroup) null);
    Game.Instance.Trigger(-2146166042, (object) null);
  }

  public void Unequip(Equippable equippable)
  {
    AssignableSlotInstance slot = this.GetSlot(equippable.slot);
    slot.Unassign();
    GameObject targetGameObject1 = this.GetTargetGameObject();
    MinionResume component1 = (UnityEngine.Object) targetGameObject1 != (UnityEngine.Object) null ? targetGameObject1.GetComponent<MinionResume>() : (MinionResume) null;
    Durability component2 = equippable.GetComponent<Durability>();
    if ((bool) (UnityEngine.Object) component2 && (bool) (UnityEngine.Object) component1 && !slot.IsUnassigning() && component1.HasPerk((HashedString) Db.Get().SkillPerks.ExosuitDurability.Id))
    {
      float num = (GameClock.Instance.GetTimeInCycles() - component2.TimeEquipped) * EQUIPMENT.SUITS.SUIT_DURABILITY_SKILL_BONUS;
      component2.TimeEquipped += num;
    }
    equippable.Trigger(-170173755, (object) this);
    if (!(bool) (UnityEngine.Object) targetGameObject1)
      return;
    targetGameObject1.Trigger(-1285462312, (object) equippable.GetComponent<KPrefabID>());
    KBatchedAnimController component3 = targetGameObject1.GetComponent<KBatchedAnimController>();
    if (!this.destroyed)
    {
      Attributes attributes = targetGameObject1.GetAttributes();
      if (attributes != null)
      {
        foreach (AttributeModifier attributeModifier in equippable.def.AttributeModifiers)
          attributes.Remove(attributeModifier);
      }
      if (!equippable.def.IsBody)
      {
        SnapOn component4 = targetGameObject1.GetComponent<SnapOn>();
        if (equippable.def.SnapOn != null)
          component4.DetachSnapOnByName(equippable.def.SnapOn);
        if (equippable.def.SnapOn1 != null)
          component4.DetachSnapOnByName(equippable.def.SnapOn1);
      }
      if ((bool) (UnityEngine.Object) equippable.transform.parent)
      {
        Storage component5 = equippable.transform.parent.GetComponent<Storage>();
        if ((bool) (UnityEngine.Object) component5)
          component5.Drop(equippable.gameObject, true);
      }
      this.SetEquippableStoredModifiers(equippable, false);
      equippable.transform.parent = (Transform) null;
      equippable.transform.SetPosition(targetGameObject1.transform.GetPosition() + Vector3.up / 2f);
      KBatchedAnimController component6 = equippable.GetComponent<KBatchedAnimController>();
      if ((bool) (UnityEngine.Object) component6)
        component6.SetSceneLayer(Grid.SceneLayer.Ore);
      if (!((UnityEngine.Object) component3 == (UnityEngine.Object) null))
      {
        if ((double) this.refreshHandle.TimeRemaining > 0.0)
          this.refreshHandle.ClearScheduler();
        Equipment instance = this;
        this.refreshHandle = GameScheduler.Instance.Schedule("ChangeEquipment", 1f, (Action<object>) (obj =>
        {
          GameObject targetGameObject2 = (UnityEngine.Object) instance != (UnityEngine.Object) null ? instance.GetTargetGameObject() : (GameObject) null;
          if (!(bool) (UnityEngine.Object) targetGameObject2)
            return;
          CreatureSimTemperatureTransfer component7 = targetGameObject2.GetComponent<CreatureSimTemperatureTransfer>();
          if (!((UnityEngine.Object) component7 != (UnityEngine.Object) null))
            return;
          component7.RefreshRegistration();
        }), (object) null, (SchedulerGroup) null);
      }
      if (!slot.IsUnassigning())
      {
        PrimaryElement component8 = equippable.GetComponent<PrimaryElement>();
        PrimaryElement component9 = targetGameObject1.GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component8 != (UnityEngine.Object) null && (UnityEngine.Object) component9 != (UnityEngine.Object) null)
        {
          SimUtil.DiseaseInfo invalid1 = SimUtil.DiseaseInfo.Invalid with
          {
            idx = component8.DiseaseIdx,
            count = (int) ((double) component8.DiseaseCount * 0.33000001311302185)
          };
          SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid with
          {
            idx = component9.DiseaseIdx,
            count = (int) ((double) component9.DiseaseCount * 0.33000001311302185)
          };
          component9.ModifyDiseaseCount(-invalid2.count, "Equipment.Unequip");
          component8.ModifyDiseaseCount(-invalid1.count, "Equipment.Unequip");
          if (invalid2.count > 0)
            component8.AddDisease(invalid2.idx, invalid2.count, "Equipment.Unequip");
          if (invalid1.count > 0)
            component9.AddDisease(invalid1.idx, invalid1.count, "Equipment.Unequip");
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.IsWornOut())
            component2.ConvertToWornObject();
        }
      }
    }
    Game.Instance.Trigger(-2146166042, (object) null);
  }

  public bool IsEquipped(Equippable equippable)
  {
    Ownables soleOwner = equippable.assignee != null ? equippable.assignee.GetSoleOwner() : (Ownables) null;
    Equipment component = (UnityEngine.Object) soleOwner != (UnityEngine.Object) null ? soleOwner.GetComponent<Equipment>() : (Equipment) null;
    return (UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component == (UnityEngine.Object) this && equippable.isEquipped;
  }

  public bool IsSlotOccupied(AssignableSlot slot)
  {
    EquipmentSlotInstance slot1 = this.GetSlot(slot) as EquipmentSlotInstance;
    return slot1.IsAssigned() && (slot1.assignable as Equippable).isEquipped;
  }

  public void UnequipAll()
  {
    foreach (AssignableSlotInstance slot in this.slots)
    {
      if ((UnityEngine.Object) slot.assignable != (UnityEngine.Object) null)
        slot.assignable.Unassign();
    }
  }

  private void SetEquippableStoredModifiers(Equippable equippable, bool isStoring)
  {
    GameObject gameObject = equippable.gameObject;
    Storage.MakeItemTemperatureInsulated(gameObject, isStoring, false);
    Storage.MakeItemInvisible(gameObject, isStoring, false);
  }
}
