// Decompiled with JetBrains decompiler
// Type: Equippable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class Equippable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor, IQuality
{
  private QualityLevel quality;
  [MyCmpAdd]
  private EquippableWorkable equippableWorkable;
  [MyCmpAdd]
  private EquippableFacade facade;
  [MyCmpReq]
  private KSelectable selectable;
  public DefHandle defHandle;
  [Serialize]
  public bool isEquipped;
  private bool destroyed;
  [Serialize]
  public bool unequippable = true;
  [Serialize]
  public bool hideInCodex;
  private static readonly EventSystem.IntraObjectHandler<Equippable> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equippable>((Action<Equippable, object>) ((component, data) => component.destroyed = true));

  public QualityLevel GetQuality() => this.quality;

  public void SetQuality(QualityLevel level) => this.quality = level;

  public EquipmentDef def
  {
    get => this.defHandle.Get<EquipmentDef>();
    set => this.defHandle.Set<EquipmentDef>(value);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.def.AdditionalTags == null)
      return;
    foreach (Tag additionalTag in this.def.AdditionalTags)
      this.GetComponent<KPrefabID>().AddTag(additionalTag);
  }

  protected override void OnSpawn()
  {
    Components.AssignableItems.Add((Assignable) this);
    if (this.isEquipped)
    {
      if (this.assignee != null && this.assignee is MinionIdentity)
      {
        this.assignee = (IAssignableIdentity) (this.assignee as MinionIdentity).assignableProxy.Get();
        this.assignee_identityRef.Set(this.assignee as KMonoBehaviour);
      }
      if (this.assignee == null && (UnityEngine.Object) this.assignee_identityRef.Get() != (UnityEngine.Object) null)
        this.assignee = this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
      if (this.assignee != null)
      {
        Equipment component1 = this.assignee.GetSoleOwner().GetComponent<Equipment>();
        bool flag = true;
        MinionAssignablesProxy component2 = component1.GetComponent<MinionAssignablesProxy>();
        GameObject go = (GameObject) null;
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          go = component1.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
          if ((UnityEngine.Object) go != (UnityEngine.Object) null)
            flag = go.GetComponent<KPrefabID>().isSpawned;
        }
        if (flag)
          this.EquipToAssignable();
        else
          go.Subscribe(1589886948, new Action<object>(this.OnAsigneeSpawnedAndReadyForEquip));
      }
      else
      {
        Debug.LogWarning((object) "Equippable trying to be equipped to missing prefab");
        this.isEquipped = false;
      }
    }
    this.Subscribe<Equippable>(1969584890, Equippable.SetDestroyedTrueDelegate);
  }

  private void EquipToAssignable()
  {
    if (this.assignee == null)
      return;
    this.assignee.GetSoleOwner().GetComponent<Equipment>().Equip(this);
  }

  private void OnAsigneeSpawnedAndReadyForEquip(object o)
  {
    GameObject go = (GameObject) o;
    this.EquipToAssignable();
    Action<object> handler = new Action<object>(this.OnAsigneeSpawnedAndReadyForEquip);
    go.Unsubscribe(1589886948, handler);
  }

  public KAnimFile GetBuildOverride()
  {
    EquippableFacade component = this.GetComponent<EquippableFacade>();
    return (UnityEngine.Object) component == (UnityEngine.Object) null || component.BuildOverride == null ? this.def.BuildOverride : Assets.GetAnim((HashedString) component.BuildOverride);
  }

  public override void Assign(IAssignableIdentity new_assignee)
  {
    if (new_assignee == this.assignee)
      return;
    if (this.slot != null && new_assignee is MinionIdentity)
      new_assignee = (IAssignableIdentity) (new_assignee as MinionIdentity).assignableProxy.Get();
    if (this.slot != null && new_assignee is StoredMinionIdentity)
      new_assignee = (IAssignableIdentity) (new_assignee as StoredMinionIdentity).assignableProxy.Get();
    if (new_assignee is MinionAssignablesProxy)
    {
      AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Equipment>().GetSlot(this.slot);
      if (slot != null)
      {
        Assignable assignable = slot.assignable;
        if ((UnityEngine.Object) assignable != (UnityEngine.Object) null)
          assignable.Unassign();
      }
    }
    base.Assign(new_assignee);
  }

  public override void Unassign()
  {
    if (this.isEquipped)
    {
      (this.assignee is MinionIdentity ? ((MinionIdentity) this.assignee).assignableProxy.Get().GetComponent<Equipment>() : ((Component) this.assignee).GetComponent<Equipment>()).Unequip(this);
      this.OnUnequip();
    }
    base.Unassign();
  }

  public void OnEquip(AssignableSlotInstance slot)
  {
    this.isEquipped = true;
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.selectable)
      SelectTool.Instance.Select((KSelectable) null);
    this.GetComponent<KBatchedAnimController>().enabled = false;
    this.GetComponent<KSelectable>().IsSelectable = false;
    string name = this.GetComponent<KPrefabID>().PrefabTag.Name;
    GameObject targetGameObject = slot.gameObject.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    Effects component = targetGameObject.GetComponent<Effects>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      foreach (Effect effectImmunite in this.def.EffectImmunites)
        component.AddImmunity(effectImmunite, name);
    }
    if (this.def.OnEquipCallBack != null)
      this.def.OnEquipCallBack(this);
    this.GetComponent<KPrefabID>().AddTag(GameTags.Equipped);
    targetGameObject.Trigger(-210173199, (object) this);
  }

  public void OnUnequip()
  {
    this.isEquipped = false;
    if (this.destroyed)
      return;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Equipped);
    this.GetComponent<KBatchedAnimController>().enabled = true;
    this.GetComponent<KSelectable>().IsSelectable = true;
    string name = this.GetComponent<KPrefabID>().PrefabTag.Name;
    if (this.assignee != null)
    {
      Ownables soleOwner = this.assignee.GetSoleOwner();
      if ((bool) (UnityEngine.Object) soleOwner)
      {
        GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
        if ((bool) (UnityEngine.Object) targetGameObject)
        {
          Effects component = targetGameObject.GetComponent<Effects>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            foreach (Effect effectImmunite in this.def.EffectImmunites)
              component.RemoveImmunity(effectImmunite, name);
          }
        }
      }
    }
    if (this.def.OnUnequipCallBack != null)
      this.def.OnUnequipCallBack(this);
    if (this.assignee == null)
      return;
    Ownables soleOwner1 = this.assignee.GetSoleOwner();
    if (!(bool) (UnityEngine.Object) soleOwner1)
      return;
    GameObject targetGameObject1 = soleOwner1.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    if (!(bool) (UnityEngine.Object) targetGameObject1)
      return;
    targetGameObject1.Trigger(-1841406856, (object) this);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    if (!((UnityEngine.Object) this.def != (UnityEngine.Object) null))
      return new List<Descriptor>();
    List<Descriptor> equipmentEffects = GameUtil.GetEquipmentEffects(this.def);
    if (this.def.additionalDescriptors != null)
    {
      foreach (Descriptor additionalDescriptor in this.def.additionalDescriptors)
        equipmentEffects.Add(additionalDescriptor);
    }
    return equipmentEffects;
  }
}
