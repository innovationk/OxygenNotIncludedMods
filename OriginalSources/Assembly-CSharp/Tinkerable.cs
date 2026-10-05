// Decompiled with JetBrains decompiler
// Type: Tinkerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/Tinkerable")]
public class Tinkerable : Workable
{
  private Chore chore;
  [MyCmpGet]
  private Storage storage;
  [MyCmpGet]
  private Effects effects;
  [MyCmpGet]
  private RoomTracker roomTracker;
  public Tag tinkerMaterialTag;
  public float tinkerMaterialAmount;
  public float tinkerMass;
  public string addedEffect;
  public string effectAttributeId;
  public float effectMultiplier;
  public string[] boostSymbolNames;
  public string onCompleteSFX;
  public HashedString choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
  public HashedString choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
  [Serialize]
  private bool userMenuAllowed = true;
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnEffectRemovedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((Action<Tinkerable, object>) ((component, data) => component.OnEffectRemoved(data)));
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((Action<Tinkerable, object>) ((component, data) => component.OnStorageChange(data)));
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((Action<Tinkerable, object>) ((component, data) => component.OnUpdateRoom(data)));
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((Action<Tinkerable, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Tinkerable>((Action<Tinkerable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private bool prioritizableAdded;
  private SchedulerHandle updateHandle;
  private bool hasReservedMaterial;

  public static Tinkerable MakePowerTinkerable(GameObject prefab)
  {
    RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
    roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
    Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
    tinkerable.tinkerMaterialTag = PowerControlStationConfig.TINKER_TOOLS;
    tinkerable.tinkerMaterialAmount = 1f;
    tinkerable.tinkerMass = 5f;
    tinkerable.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
    tinkerable.onCompleteSFX = "Generator_Microchip_installed";
    tinkerable.boostSymbolNames = new string[2]
    {
      "booster",
      "blue_light_bloom"
    };
    tinkerable.SetWorkTime(30f);
    tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
    tinkerable.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    tinkerable.choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
    tinkerable.choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
    tinkerable.addedEffect = "PowerTinker";
    tinkerable.effectAttributeId = Db.Get().Attributes.Machinery.Id;
    tinkerable.effectMultiplier = 0.025f;
    tinkerable.multitoolContext = (HashedString) "powertinker";
    tinkerable.multitoolHitEffectTag = (Tag) "fx_powertinker_splash";
    tinkerable.shouldShowSkillPerkStatusItem = false;
    prefab.AddOrGet<Storage>();
    prefab.AddOrGet<Effects>();
    prefab.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable));
    return tinkerable;
  }

  public static Tinkerable MakeFarmTinkerable(GameObject prefab)
  {
    RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
    roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
    roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
    Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
    tinkerable.tinkerMaterialTag = FarmStationConfig.TINKER_TOOLS;
    tinkerable.tinkerMaterialAmount = 1f;
    tinkerable.tinkerMass = 5f;
    tinkerable.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
    tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
    tinkerable.addedEffect = "FarmTinker";
    tinkerable.effectAttributeId = Db.Get().Attributes.Botanist.Id;
    tinkerable.effectMultiplier = 0.1f;
    tinkerable.SetWorkTime(15f);
    tinkerable.attributeConverter = Db.Get().AttributeConverters.PlantTendSpeed;
    tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    tinkerable.choreTypeTinker = Db.Get().ChoreTypes.CropTend.IdHash;
    tinkerable.choreTypeFetch = Db.Get().ChoreTypes.FarmFetch.IdHash;
    tinkerable.multitoolContext = (HashedString) "tend";
    tinkerable.multitoolHitEffectTag = (Tag) "fx_tend_splash";
    tinkerable.shouldShowSkillPerkStatusItem = false;
    prefab.AddOrGet<Storage>();
    prefab.AddOrGet<Effects>();
    prefab.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable));
    return tinkerable;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_machine_kanim")
    };
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.Subscribe<Tinkerable>(-1157678353, Tinkerable.OnEffectRemovedDelegate);
    this.Subscribe<Tinkerable>(-1697596308, Tinkerable.OnStorageChangeDelegate);
    this.Subscribe<Tinkerable>(144050788, Tinkerable.OnUpdateRoomDelegate);
    this.Subscribe<Tinkerable>(-592767678, Tinkerable.OnOperationalChangedDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Prioritizable.AddRef(this.gameObject);
    this.prioritizableAdded = true;
    this.Subscribe<Tinkerable>(493375141, Tinkerable.OnRefreshUserMenuDelegate);
    this.UpdateVisual();
  }

  protected override void OnCleanUp()
  {
    this.UpdateMaterialReservation(false);
    if (this.updateHandle.IsValid)
      this.updateHandle.ClearScheduler();
    if (this.prioritizableAdded)
      Prioritizable.RemoveRef(this.gameObject);
    base.OnCleanUp();
  }

  private void OnOperationalChanged(object _) => this.QueueUpdateChore();

  private void OnEffectRemoved(object _) => this.QueueUpdateChore();

  private void OnUpdateRoom(object _) => this.QueueUpdateChore();

  private void OnStorageChange(object data)
  {
    if (!((GameObject) data).IsPrefabID(this.tinkerMaterialTag))
      return;
    this.QueueUpdateChore();
  }

  private void QueueUpdateChore()
  {
    if (this.updateHandle.IsValid)
      this.updateHandle.ClearScheduler();
    this.updateHandle = GameScheduler.Instance.Schedule("UpdateTinkerChore", 1.2f, new Action<object>(this.UpdateChoreCallback), (object) null, (SchedulerGroup) null);
  }

  private void UpdateChoreCallback(object obj) => this.UpdateChore();

  private void UpdateChore()
  {
    Operational component = this.GetComponent<Operational>();
    bool flag1 = (UnityEngine.Object) component == (UnityEngine.Object) null || component.IsFunctional;
    int num = this.HasEffect() ? 1 : 0;
    bool flag2 = this.HasCorrectRoom();
    bool flag3 = num == 0 & flag1 & flag2 && this.userMenuAllowed;
    bool flag4 = num != 0 || !flag2 || !this.userMenuAllowed;
    if (this.chore == null & flag3)
    {
      this.UpdateMaterialReservation(true);
      if (this.HasMaterial())
      {
        this.chore = (Chore) new WorkChore<Tinkerable>(Db.Get().ChoreTypes.GetByHash(this.choreTypeTinker), (IStateMachineTarget) this, only_when_operational: false);
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          this.chore.AddPrecondition(ChorePreconditions.instance.IsFunctional, (object) component);
      }
      else
      {
        ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.choreTypeFetch);
        Storage storage = this.storage;
        double amount = (double) this.tinkerMaterialAmount * (double) this.tinkerMass;
        HashSet<Tag> tags = new HashSet<Tag>();
        tags.Add(this.tinkerMaterialTag);
        Tag invalid = Tag.Invalid;
        Action<Chore> on_complete = new Action<Chore>(this.OnFetchComplete);
        this.chore = (Chore) new FetchChore(byHash, storage, (float) amount, tags, FetchChore.MatchCriteria.MatchID, invalid, on_complete: on_complete, operational_requirement: Operational.State.Functional);
      }
      this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) this.requiredSkillPerk);
      if (string.IsNullOrEmpty(this.GetComponent<RoomTracker>().requiredRoomType))
        return;
      this.chore.AddPrecondition(ChorePreconditions.instance.IsInMyRoom, (object) Grid.PosToCell(this.transform.GetPosition()));
    }
    else
    {
      if (!(this.chore != null & flag4))
        return;
      this.UpdateMaterialReservation(false);
      this.chore.Cancel("No longer needed");
      this.chore = (Chore) null;
    }
  }

  private bool HasCorrectRoom() => this.roomTracker.IsInCorrectRoom();

  private bool RoomHasTinkerstation()
  {
    if (!this.roomTracker.IsInCorrectRoom() || this.roomTracker.room == null)
      return false;
    foreach (KPrefabID building in this.roomTracker.room.buildings)
    {
      if (!((UnityEngine.Object) building == (UnityEngine.Object) null))
      {
        TinkerStation component = building.GetComponent<TinkerStation>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.outputPrefab == this.tinkerMaterialTag)
          return true;
      }
    }
    return false;
  }

  private void UpdateMaterialReservation(bool shouldReserve)
  {
    if (shouldReserve && !this.hasReservedMaterial)
    {
      MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, this.tinkerMaterialAmount, this.gameObject.GetMyWorldId());
      this.hasReservedMaterial = shouldReserve;
    }
    else
    {
      if (shouldReserve || !this.hasReservedMaterial)
        return;
      MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, -this.tinkerMaterialAmount, this.gameObject.GetMyWorldId());
      this.hasReservedMaterial = shouldReserve;
    }
  }

  private void OnFetchComplete(Chore data)
  {
    this.UpdateMaterialReservation(false);
    this.chore = (Chore) null;
    this.UpdateChore();
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    this.storage.ConsumeIgnoringDisease(this.tinkerMaterialTag, this.tinkerMaterialAmount);
    float totalValue = worker.GetAttributes().Get(Db.Get().Attributes.Get(this.effectAttributeId)).GetTotalValue();
    this.effects.Add(this.addedEffect, true).timeRemaining *= (float) (1.0 + (double) totalValue * (double) this.effectMultiplier);
    this.UpdateVisual();
    this.UpdateMaterialReservation(false);
    this.chore = (Chore) null;
    this.UpdateChore();
    string sound = GlobalAssets.GetSound(this.onCompleteSFX);
    if (sound == null)
      return;
    SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, this.transform.position));
  }

  private void UpdateVisual()
  {
    if (this.boostSymbolNames == null)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    bool is_visible = this.effects.HasEffect(this.addedEffect);
    foreach (string boostSymbolName in this.boostSymbolNames)
      component.SetSymbolVisiblity((KAnimHashedString) boostSymbolName, is_visible);
  }

  private bool HasMaterial()
  {
    return (double) this.storage.GetAmountAvailable(this.tinkerMaterialTag) >= (double) this.tinkerMaterialAmount;
  }

  private bool HasEffect() => this.effects.HasEffect(this.addedEffect);

  private void OnRefreshUserMenu(object data)
  {
    if (!this.roomTracker.IsInCorrectRoom())
      return;
    string name = Db.Get().effects.Get(this.addedEffect).Name;
    string properName = this.GetProperName();
    Game.Instance.userMenu.AddButton(this.gameObject, this.userMenuAllowed ? new KIconButtonMenu.ButtonInfo("action_switch_toggle", (string) UI.USERMENUACTIONS.TINKER.DISALLOW, new System.Action(this.OnClickToggleTinker), tooltipText: string.Format((string) UI.USERMENUACTIONS.TINKER.TOOLTIP_DISALLOW, (object) name, (object) properName)) : new KIconButtonMenu.ButtonInfo("action_switch_toggle", (string) UI.USERMENUACTIONS.TINKER.ALLOW, new System.Action(this.OnClickToggleTinker), tooltipText: string.Format((string) UI.USERMENUACTIONS.TINKER.TOOLTIP_ALLOW, (object) name, (object) properName)));
  }

  private void OnClickToggleTinker()
  {
    this.userMenuAllowed = !this.userMenuAllowed;
    this.UpdateChore();
  }
}
