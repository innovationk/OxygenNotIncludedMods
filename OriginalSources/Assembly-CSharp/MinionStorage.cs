// Decompiled with JetBrains decompiler
// Type: MinionStorage
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
[AddComponentMenu("KMonoBehaviour/scripts/MinionStorage")]
public class MinionStorage : KMonoBehaviour
{
  [Serialize]
  private List<MinionStorage.Info> serializedMinions = new List<MinionStorage.Info>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.MinionStorages.Add(this);
  }

  protected override void OnCleanUp()
  {
    Components.MinionStorages.Remove(this);
    base.OnCleanUp();
  }

  private KPrefabID CreateSerializedMinion(GameObject src_minion)
  {
    GameObject gameObject = Util.KInstantiate(SaveLoader.Instance.saveManager.GetPrefab((Tag) StoredMinionConfig.ID), Vector3.zero);
    gameObject.SetActive(true);
    MinionIdentity component1 = src_minion.GetComponent<MinionIdentity>();
    StoredMinionIdentity component2 = gameObject.GetComponent<StoredMinionIdentity>();
    this.CopyMinion(component1, component2);
    MinionStorage.RedirectInstanceTracker(src_minion, gameObject);
    component1.assignableProxy.Get().SetTarget((IAssignableIdentity) component2, gameObject);
    Util.KDestroyGameObject(src_minion);
    return gameObject.GetComponent<KPrefabID>();
  }

  private void CopyMinion(MinionIdentity src_id, StoredMinionIdentity dest_id)
  {
    dest_id.storedName = src_id.name;
    dest_id.nameStringKey = src_id.nameStringKey;
    dest_id.personalityResourceId = src_id.personalityResourceId;
    dest_id.model = src_id.model;
    dest_id.gender = src_id.gender;
    dest_id.genderStringKey = src_id.genderStringKey;
    dest_id.arrivalTime = src_id.arrivalTime;
    dest_id.voiceIdx = src_id.voiceIdx;
    dest_id.bodyData = src_id.GetComponent<Accessorizer>().bodyData;
    Traits component1 = src_id.GetComponent<Traits>();
    dest_id.traitIDs = new List<string>((IEnumerable<string>) component1.GetTraitIds());
    dest_id.assignableProxy.Set(src_id.assignableProxy.Get());
    dest_id.assignableProxy.Get().SetTarget((IAssignableIdentity) dest_id, dest_id.gameObject);
    Accessorizer component2 = src_id.GetComponent<Accessorizer>();
    dest_id.accessories = component2.GetAccessories();
    WearableAccessorizer component3 = src_id.GetComponent<WearableAccessorizer>();
    dest_id.customClothingItems = component3.GetCustomClothingItems();
    dest_id.wearables = component3.Wearables;
    ConsumableConsumer component4 = src_id.GetComponent<ConsumableConsumer>();
    if (component4.forbiddenTagSet != null)
      dest_id.forbiddenTagSet = new HashSet<Tag>((IEnumerable<Tag>) component4.forbiddenTagSet);
    MinionResume component5 = src_id.GetComponent<MinionResume>();
    dest_id.MasteryBySkillID = component5.MasteryBySkillID;
    dest_id.grantedSkillIDs = component5.GrantedSkillIDs;
    dest_id.AptitudeBySkillGroup = component5.AptitudeBySkillGroup;
    dest_id.TotalExperienceGained = component5.TotalExperienceGained;
    dest_id.currentHat = component5.CurrentHat;
    dest_id.targetHat = component5.TargetHat;
    ChoreConsumer component6 = src_id.GetComponent<ChoreConsumer>();
    dest_id.choreGroupPriorities = component6.GetChoreGroupPriorities();
    AttributeLevels component7 = src_id.GetComponent<AttributeLevels>();
    component7.OnSerializing();
    dest_id.attributeLevels = new List<AttributeLevels.LevelSaveLoad>((IEnumerable<AttributeLevels.LevelSaveLoad>) component7.SaveLoadLevels);
    Effects component8 = src_id.GetComponent<Effects>();
    dest_id.saveLoadEffects = component8.GetAllEffectsForSerialization();
    dest_id.saveLoadImmunities = component8.GetAllImmunitiesForSerialization();
    MinionStorage.StoreModifiers(src_id, dest_id);
    Schedulable component9 = src_id.GetComponent<Schedulable>();
    Schedule schedule = component9.GetSchedule();
    if (schedule != null)
    {
      schedule.Unassign(component9);
      Schedulable component10 = dest_id.GetComponent<Schedulable>();
      schedule.Assign(component10);
    }
    foreach (StoredMinionIdentity.IStoredMinionExtension component11 in src_id.GetComponents<StoredMinionIdentity.IStoredMinionExtension>())
      component11.PushTo(dest_id);
  }

  private static void StoreModifiers(MinionIdentity src_id, StoredMinionIdentity dest_id)
  {
    foreach (AttributeInstance attribute in src_id.GetComponent<MinionModifiers>().attributes)
    {
      if (dest_id.minionModifiers.attributes.Get(attribute.Attribute.Id) == null)
        dest_id.minionModifiers.attributes.Add(attribute.Attribute);
      for (int i = 0; i < attribute.Modifiers.Count; ++i)
        dest_id.minionModifiers.attributes.Get(attribute.Id).Add(attribute.Modifiers[i]);
    }
  }

  private static void CopyMinion(StoredMinionIdentity src_id, MinionIdentity dest_id)
  {
    dest_id.Subscribe(1589886948, new Action<object>(MinionStorage.OnDeserializedMinionSpawned));
    dest_id.SetName(src_id.storedName);
    dest_id.nameStringKey = src_id.nameStringKey;
    dest_id.model = src_id.model;
    dest_id.personalityResourceId = src_id.personalityResourceId;
    dest_id.gender = src_id.gender;
    dest_id.genderStringKey = src_id.genderStringKey;
    dest_id.arrivalTime = src_id.arrivalTime;
    dest_id.voiceIdx = src_id.voiceIdx;
    dest_id.GetComponent<Accessorizer>().bodyData = src_id.bodyData;
    if (src_id.traitIDs != null)
      dest_id.GetComponent<Traits>().SetTraitIds(src_id.traitIDs);
    if (src_id.accessories != null)
      dest_id.GetComponent<Accessorizer>().SetAccessories(src_id.accessories);
    dest_id.GetComponent<WearableAccessorizer>().RestoreWearables(src_id.wearables, src_id.customClothingItems);
    ConsumableConsumer component1 = dest_id.GetComponent<ConsumableConsumer>();
    if (src_id.forbiddenTagSet != null)
      component1.forbiddenTagSet = new HashSet<Tag>((IEnumerable<Tag>) src_id.forbiddenTagSet);
    if (src_id.MasteryBySkillID != null)
    {
      MinionResume component2 = dest_id.GetComponent<MinionResume>();
      component2.RestoreResume(src_id.MasteryBySkillID, src_id.AptitudeBySkillGroup, src_id.grantedSkillIDs, src_id.TotalExperienceGained);
      component2.SetHats(src_id.currentHat, src_id.targetHat);
    }
    if (src_id.choreGroupPriorities != null)
      dest_id.GetComponent<ChoreConsumer>().SetChoreGroupPriorities(src_id.choreGroupPriorities);
    AttributeLevels component3 = dest_id.GetComponent<AttributeLevels>();
    if (src_id.attributeLevels != null)
    {
      component3.SaveLoadLevels = src_id.attributeLevels.ToArray();
      component3.OnDeserialized();
    }
    Effects component4 = dest_id.GetComponent<Effects>();
    if (src_id.saveLoadImmunities != null)
    {
      foreach (Effects.SaveLoadImmunities saveLoadImmunity in src_id.saveLoadImmunities)
      {
        if (Db.Get().effects.Exists(saveLoadImmunity.effectID))
        {
          Effect effect = Db.Get().effects.Get(saveLoadImmunity.effectID);
          component4.AddImmunity(effect, saveLoadImmunity.giverID, saveLoadImmunity.saved);
        }
      }
    }
    if (src_id.saveLoadEffects != null)
    {
      foreach (Effects.SaveLoadEffect saveLoadEffect in src_id.saveLoadEffects)
      {
        if (Db.Get().effects.Exists(saveLoadEffect.id))
        {
          Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
          EffectInstance effectInstance = component4.Add(newEffect, saveLoadEffect.saved);
          if (effectInstance != null)
            effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
        }
      }
    }
    dest_id.GetComponent<Accessorizer>().ApplyAccessories();
    dest_id.assignableProxy = new Ref<MinionAssignablesProxy>();
    dest_id.assignableProxy.Set(src_id.assignableProxy.Get());
    dest_id.assignableProxy.Get().SetTarget((IAssignableIdentity) dest_id, dest_id.gameObject);
    Schedulable component5 = src_id.GetComponent<Schedulable>();
    Schedule schedule = component5.GetSchedule();
    if (schedule != null)
    {
      schedule.Unassign(component5);
      Schedulable component6 = dest_id.GetComponent<Schedulable>();
      schedule.Assign(component6);
    }
    foreach (StoredMinionIdentity.IStoredMinionExtension component7 in dest_id.GetComponents<StoredMinionIdentity.IStoredMinionExtension>())
      component7.PullFrom(src_id);
  }

  private static void OnDeserializedMinionSpawned(object deserializedMinionOBJ)
  {
    MinionIdentity component = ((GameObject) deserializedMinionOBJ).GetComponent<MinionIdentity>();
    Equipment equipment = component.GetEquipment();
    foreach (AssignableSlotInstance slot in equipment.Slots)
    {
      Equippable assignable = slot.assignable as Equippable;
      if ((UnityEngine.Object) assignable != (UnityEngine.Object) null)
        equipment.Equip(assignable);
    }
    component.Unsubscribe(1589886948, new Action<object>(MinionStorage.OnDeserializedMinionSpawned));
  }

  public static void RedirectInstanceTracker(GameObject src_minion, GameObject dest_minion)
  {
    KPrefabID component = src_minion.GetComponent<KPrefabID>();
    dest_minion.GetComponent<KPrefabID>().InstanceID = component.InstanceID;
    component.InstanceID = -1;
  }

  public void SerializeMinion(GameObject minion)
  {
    this.CleanupBadReferences();
    KPrefabID serializedMinion = this.CreateSerializedMinion(minion);
    this.serializedMinions.Add(new MinionStorage.Info(serializedMinion.GetComponent<StoredMinionIdentity>().storedName, new Ref<KPrefabID>(serializedMinion)));
  }

  private void CleanupBadReferences()
  {
    for (int index = this.serializedMinions.Count - 1; index >= 0; --index)
    {
      if (this.serializedMinions[index].serializedMinion == null || (UnityEngine.Object) this.serializedMinions[index].serializedMinion.Get() == (UnityEngine.Object) null)
        this.serializedMinions.RemoveAt(index);
    }
  }

  private int GetMinionIndex(Guid id)
  {
    int minionIndex = -1;
    for (int index = 0; index < this.serializedMinions.Count; ++index)
    {
      if (this.serializedMinions[index].id == id)
      {
        minionIndex = index;
        break;
      }
    }
    return minionIndex;
  }

  public GameObject DeserializeMinion(Guid id, Vector3 pos)
  {
    int minionIndex = this.GetMinionIndex(id);
    if (minionIndex < 0 || minionIndex >= this.serializedMinions.Count)
      return (GameObject) null;
    KPrefabID kprefabId = this.serializedMinions[minionIndex].serializedMinion.Get();
    this.serializedMinions.RemoveAt(minionIndex);
    return (UnityEngine.Object) kprefabId == (UnityEngine.Object) null ? (GameObject) null : MinionStorage.DeserializeMinion(kprefabId.gameObject, pos);
  }

  public static GameObject DeserializeMinion(GameObject sourceMinion, Vector3 pos)
  {
    StoredMinionIdentity component1 = sourceMinion.GetComponent<StoredMinionIdentity>();
    GameObject gameObject = Util.KInstantiate(SaveLoader.Instance.saveManager.GetPrefab((Tag) BaseMinionConfig.GetMinionIDForModel(component1.model)), pos);
    MinionIdentity component2 = gameObject.GetComponent<MinionIdentity>();
    MinionStorage.RedirectInstanceTracker(sourceMinion, gameObject);
    gameObject.SetActive(true);
    MinionStorage.CopyMinion(component1, component2);
    component1.assignableProxy.Get().SetTarget((IAssignableIdentity) component2, gameObject);
    Util.KDestroyGameObject(sourceMinion);
    return gameObject;
  }

  public void DeleteStoredMinion(Guid id)
  {
    int minionIndex = this.GetMinionIndex(id);
    if (minionIndex < 0)
      return;
    if (this.serializedMinions[minionIndex].serializedMinion != null)
    {
      this.serializedMinions[minionIndex].serializedMinion.Get().GetComponent<StoredMinionIdentity>().OnHardDelete();
      Util.KDestroyGameObject(this.serializedMinions[minionIndex].serializedMinion.Get().gameObject);
    }
    this.serializedMinions.RemoveAt(minionIndex);
  }

  public List<MinionStorage.Info> GetStoredMinionInfo() => this.serializedMinions;

  public void SetStoredMinionInfo(List<MinionStorage.Info> info) => this.serializedMinions = info;

  public struct Info(string name, Ref<KPrefabID> ref_obj)
  {
    public Guid id = Guid.NewGuid();
    public string name = name;
    public Ref<KPrefabID> serializedMinion = ref_obj;

    public static MinionStorage.Info CreateEmpty()
    {
      return new MinionStorage.Info()
      {
        id = Guid.Empty,
        name = (string) null,
        serializedMinion = (Ref<KPrefabID>) null
      };
    }
  }
}
