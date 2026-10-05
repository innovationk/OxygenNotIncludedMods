// Decompiled with JetBrains decompiler
// Type: EggIncubator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class EggIncubator : SingleEntityReceptacle, ISaveLoadable, ISim1000ms
{
  [MyCmpAdd]
  private EggIncubatorWorkable workable;
  [MyCmpAdd]
  private CopyBuildingSettings copySettings;
  private Chore chore;
  private EggIncubatorStates.Instance smi;
  private KBatchedAnimTracker tracker;
  private MeterController meter;
  private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>((Action<EggIncubator, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>((Action<EggIncubator, object>) ((component, data) => component.OnOccupantChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<EggIncubator>((Action<EggIncubator, object>) ((component, data) => component.OnStorageChange(data)));
  private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EggIncubator>((Action<EggIncubator, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.autoReplaceEntity = true;
    this.choreType = Db.Get().ChoreTypes.RanchingFetch;
    this.statusItemNeed = Db.Get().BuildingStatusItems.NeedEgg;
    this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableEgg;
    this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingEggDelivery;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    this.occupyingObjectRelativePosition = new Vector3(0.5f, 1f, -1f);
    this.synchronizeAnims = false;
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "egg_target", false);
    this.meter = new MeterController((KMonoBehaviour) this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
    this.Subscribe<EggIncubator>(-905833192, EggIncubator.OnCopySettingsDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((bool) (UnityEngine.Object) this.occupyingObject)
    {
      if (this.occupyingObject.HasTag(GameTags.Creature))
        this.storage.allowItemRemoval = true;
      this.storage.RenotifyAll();
      this.PositionOccupyingObject();
    }
    this.Subscribe<EggIncubator>(-592767678, EggIncubator.OnOperationalChangedDelegate);
    this.Subscribe<EggIncubator>(-731304873, EggIncubator.OnOccupantChangedDelegate);
    this.Subscribe<EggIncubator>(-1697596308, EggIncubator.OnStorageChangeDelegate);
    this.smi = new EggIncubatorStates.Instance((IStateMachineTarget) this);
    this.smi.StartSM();
  }

  private void OnCopySettings(object data)
  {
    EggIncubator component1 = ((GameObject) data).GetComponent<EggIncubator>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
      return;
    this.autoReplaceEntity = component1.autoReplaceEntity;
    if ((UnityEngine.Object) this.occupyingObject == (UnityEngine.Object) null)
    {
      if ((!(this.requestedEntityTag == component1.requestedEntityTag) ? 0 : (this.requestedEntityAdditionalFilterTag == component1.requestedEntityAdditionalFilterTag ? 1 : 0)) == 0)
        this.CancelActiveRequest();
      if (this.fetchChore == null)
        this.CreateOrder(component1.requestedEntityTag, component1.requestedEntityAdditionalFilterTag);
    }
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    Prioritizable component2 = this.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    Prioritizable component3 = this.occupyingObject.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null))
      return;
    component3.SetMasterPriority(component2.GetMasterPriority());
  }

  protected override void OnCleanUp()
  {
    this.smi.StopSM("cleanup");
    base.OnCleanUp();
  }

  protected override void SubscribeToOccupant()
  {
    base.SubscribeToOccupant();
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
    {
      this.tracker = this.occupyingObject.AddComponent<KBatchedAnimTracker>();
      this.tracker.symbol = (HashedString) "egg_target";
      this.tracker.forceAlwaysVisible = true;
    }
    this.UpdateProgress();
  }

  protected override void UnsubscribeFromOccupant()
  {
    base.UnsubscribeFromOccupant();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.tracker);
    this.tracker = (KBatchedAnimTracker) null;
    this.UpdateProgress();
  }

  private new void OnOperationalChanged(object _ = null)
  {
    if ((bool) (UnityEngine.Object) this.occupyingObject)
      return;
    this.storage.DropAll();
  }

  private void OnOccupantChanged(object _ = null)
  {
    if ((bool) (UnityEngine.Object) this.occupyingObject)
      return;
    this.storage.allowItemRemoval = false;
  }

  private void OnStorageChange(object _ = null)
  {
    if (!(bool) (UnityEngine.Object) this.occupyingObject || this.storage.items.Contains(this.occupyingObject))
      return;
    this.UnsubscribeFromOccupant();
    this.ClearOccupant();
  }

  protected override void ClearOccupant()
  {
    bool flag = false;
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
      flag = !this.occupyingObject.HasTag(GameTags.Egg);
    base.ClearOccupant();
    if (!(this.autoReplaceEntity & flag) || !this.requestedEntityTag.IsValid)
      return;
    this.CreateOrder(this.requestedEntityTag, Tag.Invalid);
  }

  protected override void PositionOccupyingObject()
  {
    base.PositionOccupyingObject();
    this.occupyingObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
    KSelectable component = this.occupyingObject.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.IsSelectable = true;
  }

  public override void OrderRemoveOccupant()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.tracker);
    this.tracker = (KBatchedAnimTracker) null;
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null && this.occupyingObject.HasTag(GameTags.Egg))
      this.requestedEntityTag = Tag.Invalid;
    this.storage.DropAll();
    this.occupyingObject = (GameObject) null;
    this.ClearOccupant();
  }

  public float GetProgress()
  {
    float progress = 0.0f;
    if ((bool) (UnityEngine.Object) this.occupyingObject)
    {
      AmountInstance amountInstance = this.occupyingObject.GetAmounts().Get(Db.Get().Amounts.Incubation);
      progress = amountInstance == null ? 1f : amountInstance.value / amountInstance.GetMax();
    }
    return progress;
  }

  private void UpdateProgress() => this.meter.SetPositionPercent(this.GetProgress());

  public void Sim1000ms(float dt)
  {
    this.UpdateProgress();
    this.UpdateChore();
  }

  public void StoreBaby(GameObject baby)
  {
    this.UnsubscribeFromOccupant();
    this.storage.DropAll();
    this.storage.allowItemRemoval = true;
    this.storage.Store(baby);
    this.occupyingObject = baby;
    this.SubscribeToOccupant();
    this.Trigger(-731304873, (object) this.occupyingObject);
  }

  private void UpdateChore()
  {
    if (this.operational.IsOperational && this.EggNeedsAttention())
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<EggIncubatorWorkable>(Db.Get().ChoreTypes.EggSing, (IStateMachineTarget) this.workable);
    }
    else
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("now is not the time for song");
      this.chore = (Chore) null;
    }
  }

  private bool EggNeedsAttention()
  {
    if (!(bool) (UnityEngine.Object) this.Occupant)
      return false;
    IncubationMonitor.Instance smi = this.Occupant.GetSMI<IncubationMonitor.Instance>();
    return smi != null && !smi.HasSongBuff();
  }
}
