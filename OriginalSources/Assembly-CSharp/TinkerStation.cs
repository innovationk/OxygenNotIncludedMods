// Decompiled with JetBrains decompiler
// Type: TinkerStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

#nullable disable
[AddComponentMenu("KMonoBehaviour/Workable/TinkerStation")]
public class TinkerStation : Workable, ISim1000ms
{
  public HashedString choreType;
  public HashedString fetchChoreType;
  private Chore chore;
  [MyCmpAdd]
  private Operational operational;
  [MyCmpAdd]
  private Storage storage;
  public bool useFilteredStorage;
  protected FilteredStorage filteredStorage;
  public float toolProductionTime = 160f;
  public bool alwaysTinker;
  public float massPerTinker;
  public Tag inputMaterial;
  public Tag outputPrefab;
  public float outputTemperature;
  public string EffectTitle = (string) UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS;
  public string EffectTooltip = (string) UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS;
  public string EffectItemString = (string) UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS_ITEM;
  public string EffectItemTooltip = (string) UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS_ITEM;
  private static readonly EventSystem.IntraObjectHandler<TinkerStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TinkerStation>((Action<TinkerStation, object>) ((component, data) => component.OnOperationalChanged(data)));

  public AttributeConverter AttributeConverter
  {
    set => this.attributeConverter = value;
  }

  public float AttributeExperienceMultiplier
  {
    set => this.attributeExperienceMultiplier = value;
  }

  public string SkillExperienceSkillGroup
  {
    set => this.skillExperienceSkillGroup = value;
  }

  public float SkillExperienceMultiplier
  {
    set => this.skillExperienceMultiplier = value;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    if (this.useFilteredStorage)
      this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, (IUserControlledCapacity) null, false, Db.Get().ChoreTypes.GetByHash(this.fetchChoreType));
    this.Subscribe<TinkerStation>(-592767678, TinkerStation.OnOperationalChangedDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!this.useFilteredStorage || this.filteredStorage == null)
      return;
    this.filteredStorage.SetHasMeter(false);
    this.filteredStorage.FilterChanged();
  }

  protected override void OnCleanUp()
  {
    if (this.filteredStorage != null)
      this.filteredStorage.CleanUp();
    base.OnCleanUp();
  }

  private bool CorrectRolePrecondition(MinionIdentity worker)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && component.HasPerk((HashedString) this.requiredSkillPerk);
  }

  private void OnOperationalChanged(object _)
  {
    RoomTracker component = this.GetComponent<RoomTracker>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.room == null)
      return;
    component.room.RetriggerBuildings();
  }

  protected override void OnStartWork(WorkerBase worker)
  {
    base.OnStartWork(worker);
    if (!this.operational.IsOperational)
      return;
    this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, (object) this);
    this.operational.SetActive(true);
  }

  protected override void OnStopWork(WorkerBase worker)
  {
    base.OnStopWork(worker);
    this.ShowProgressBar(false);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, (bool) (UnityEngine.Object) this);
    this.operational.SetActive(false);
  }

  protected override void OnCompleteWork(WorkerBase worker)
  {
    base.OnCompleteWork(worker);
    PrimaryElement firstWithMass = this.storage.FindFirstWithMass(this.inputMaterial, this.massPerTinker);
    if ((UnityEngine.Object) firstWithMass != (UnityEngine.Object) null)
    {
      SimHashes elementId = firstWithMass.ElementID;
      float aggregate_temperature = 1f;
      this.storage.ConsumeAndGetDisease(elementId.CreateTag(), this.massPerTinker, out float _, out SimUtil.DiseaseInfo _, out aggregate_temperature);
      GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.outputPrefab), this.transform.GetPosition() + Vector3.up, Grid.SceneLayer.Ore);
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      component.SetElement(elementId);
      component.Temperature = aggregate_temperature;
      gameObject.SetActive(true);
    }
    this.chore = (Chore) null;
  }

  public void Sim1000ms(float dt) => this.UpdateChore();

  private void UpdateChore()
  {
    if (this.operational.IsOperational && (this.ToolsRequested() || this.alwaysTinker) && this.HasMaterial())
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<TinkerStation>(Db.Get().ChoreTypes.GetByHash(this.choreType), (IStateMachineTarget) this);
      this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) this.requiredSkillPerk);
      this.SetWorkTime(this.toolProductionTime);
    }
    else
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Can't tinker");
      this.chore = (Chore) null;
    }
  }

  private bool HasMaterial()
  {
    return (UnityEngine.Object) this.storage.FindFirstWithMass(this.inputMaterial, this.massPerTinker) != (UnityEngine.Object) null;
  }

  private bool ToolsRequested()
  {
    return (double) MaterialNeeds.GetAmount(this.outputPrefab, this.gameObject.GetMyWorldId(), false) > 0.0 && (double) this.GetMyWorld().worldInventory.GetAmount(this.outputPrefab, true) <= 0.0;
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    string str = this.inputMaterial.ProperName();
    List<Descriptor> descriptors = base.GetDescriptors(go);
    descriptors.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(this.massPerTinker)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(this.massPerTinker)), Descriptor.DescriptorType.Requirement));
    descriptors.AddRange((IEnumerable<Descriptor>) GameUtil.GetAllDescriptors(Assets.GetPrefab(this.outputPrefab)));
    List<Tinkerable> tinkerableList = new List<Tinkerable>();
    foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<Tinkerable>())
    {
      Tinkerable component = gameObject.GetComponent<Tinkerable>();
      if (component.tinkerMaterialTag == this.outputPrefab)
        tinkerableList.Add(component);
    }
    if (tinkerableList.Count > 0)
    {
      Effect effect = Db.Get().effects.Get(tinkerableList[0].addedEffect);
      descriptors.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ADDED_EFFECT, (object) effect.Name), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ADDED_EFFECT, (object) effect.Name, (object) Effect.CreateTooltip(effect, true))));
      descriptors.Add(new Descriptor(this.EffectTitle, this.EffectTooltip));
      foreach (Tinkerable cmp in tinkerableList)
      {
        Descriptor descriptor = new Descriptor(string.Format(this.EffectItemString, (object) cmp.GetProperName()), string.Format(this.EffectItemTooltip, (object) cmp.GetProperName()));
        descriptor.IncreaseIndent();
        descriptors.Add(descriptor);
      }
    }
    return descriptors;
  }

  public static TinkerStation AddTinkerStation(GameObject go, string required_room_type)
  {
    TinkerStation tinkerStation = go.AddOrGet<TinkerStation>();
    go.AddOrGet<RoomTracker>().requiredRoomType = required_room_type;
    return tinkerStation;
  }
}
