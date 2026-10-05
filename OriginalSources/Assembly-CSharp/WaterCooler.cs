// Decompiled with JetBrains decompiler
// Type: WaterCooler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterCooler : 
  StateMachineComponent<WaterCooler.StatesInstance>,
  IApproachable,
  IGameObjectEffectDescriptor,
  FewOptionSideScreen.IFewOptionSideScreen
{
  public const float DRINK_MASS = 1f;
  public const string SPECIFIC_EFFECT = "Socialized";
  public CellOffset[] socializeOffsets = new CellOffset[4]
  {
    new CellOffset(-1, 0),
    new CellOffset(2, 0),
    new CellOffset(0, 0),
    new CellOffset(1, 0)
  };
  public int choreCount = 2;
  public float workTime = 5f;
  private CellOffset[] drinkOffsets = new CellOffset[2]
  {
    new CellOffset(0, 0),
    new CellOffset(1, 0)
  };
  public static Action<GameObject, GameObject> OnDuplicantDrank;
  private Chore[] chores;
  private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;
  private SocialGatheringPointWorkable[] workables;
  [MyCmpGet]
  private Storage storage;
  public bool choresDirty;
  [Serialize]
  private Tag chosenBeverage = GameTags.Water;
  private static readonly EventSystem.IntraObjectHandler<WaterCooler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<WaterCooler>((Action<WaterCooler, object>) ((component, data) => component.OnStorageChange(data)));

  public Tag ChosenBeverage
  {
    get => this.chosenBeverage;
    set
    {
      if (!(this.chosenBeverage != value))
        return;
      this.chosenBeverage = value;
      this.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
      this.storage.DropAll();
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
    GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, (Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule)), (object) null, (SchedulerGroup) null);
    this.workables = new SocialGatheringPointWorkable[this.socializeOffsets.Length];
    for (int index = 0; index < this.workables.Length; ++index)
    {
      SocialGatheringPointWorkable gatheringPointWorkable = ChoreHelpers.CreateLocator("WaterCoolerWorkable", Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), this.socializeOffsets[index]), Grid.SceneLayer.Move)).AddOrGet<SocialGatheringPointWorkable>();
      gatheringPointWorkable.specificEffect = "Socialized";
      gatheringPointWorkable.SetWorkTime(this.workTime);
      this.workables[index] = gatheringPointWorkable;
    }
    this.chores = new Chore[this.socializeOffsets.Length];
    this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add(nameof (WaterCooler), (object) this, new Extents(Grid.PosToCell((KMonoBehaviour) this), this.socializeOffsets), GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
    this.Subscribe<WaterCooler>(-1697596308, WaterCooler.OnStorageChangeDelegate);
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
    this.CancelDrinkChores();
    for (int index = 0; index < this.workables.Length; ++index)
    {
      if ((bool) (UnityEngine.Object) this.workables[index])
      {
        Util.KDestroyGameObject((Component) this.workables[index]);
        this.workables[index] = (SocialGatheringPointWorkable) null;
      }
    }
    base.OnCleanUp();
  }

  public void UpdateDrinkChores(bool force = true)
  {
    if (!force && !this.choresDirty)
      return;
    float massAvailable = this.storage.GetMassAvailable(this.ChosenBeverage);
    int num = 0;
    for (int index = 0; index < this.socializeOffsets.Length; ++index)
    {
      CellOffset socializeOffset = this.socializeOffsets[index];
      Chore chore = this.chores[index];
      if ((num >= this.choreCount || !this.IsOffsetValid(socializeOffset) ? 0 : ((double) massAvailable >= 1.0 ? 1 : 0)) != 0)
      {
        ++num;
        --massAvailable;
        if (chore == null || chore.isComplete)
          this.chores[index] = (Chore) new WaterCoolerChore((IStateMachineTarget) this, (Workable) this.workables[index], on_end: new Action<Chore>(this.OnChoreEnd));
      }
      else if (chore != null)
      {
        chore.Cancel("invalid");
        this.chores[index] = (Chore) null;
      }
    }
    this.choresDirty = false;
  }

  public void CancelDrinkChores()
  {
    for (int index = 0; index < this.socializeOffsets.Length; ++index)
    {
      Chore chore = this.chores[index];
      if (chore != null)
      {
        chore.Cancel("cancelled");
        this.chores[index] = (Chore) null;
      }
    }
  }

  private bool IsOffsetValid(CellOffset offset)
  {
    int cell = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), offset);
    return GameNavGrids.FloorValidator.IsWalkableCell(cell, Grid.CellBelow(cell), false);
  }

  private void OnChoreEnd(Chore chore) => this.choresDirty = true;

  private void OnCellChanged(object data) => this.choresDirty = true;

  private void OnStorageChange(object data) => this.choresDirty = true;

  public CellOffset[] GetOffsets() => this.drinkOffsets;

  public int GetCell() => Grid.PosToCell((KMonoBehaviour) this);

  private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
  {
    string str = tag.ProperName();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(mass, floatFormat: "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(mass, floatFormat: "{0:0.##}")), Descriptor.DescriptorType.Requirement);
    descs.Add(descriptor);
  }

  List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
  {
    List<Descriptor> descs = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.RECREATION, (string) UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION);
    descs.Add(descriptor);
    Effect.AddModifierDescriptions(this.gameObject, descs, "Socialized", true);
    foreach (Tuple<Tag, string> tuple in WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS)
      this.AddRequirementDesc(descs, tuple.first, 1f);
    return descs;
  }

  public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
  {
    Effect.CreateTooltip(Db.Get().effects.Get("DuplicantGotMilk"), true);
    FewOptionSideScreen.IFewOptionSideScreen.Option[] options = new FewOptionSideScreen.IFewOptionSideScreen.Option[WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS.Length];
    for (int index = 0; index < options.Length; ++index)
    {
      string tooltipText = (string) Strings.Get("STRINGS.BUILDINGS.PREFABS.WATERCOOLER.OPTION_TOOLTIPS." + WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[index].first.ToString().ToUpper());
      if (!WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[index].second.IsNullOrWhiteSpace())
        tooltipText = $"{tooltipText}\n\n{Effect.CreateTooltip(Db.Get().effects.Get(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[index].second), false)}";
      options[index] = new FewOptionSideScreen.IFewOptionSideScreen.Option(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[index].first, ElementLoader.GetElement(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[index].first).name, Def.GetUISprite((object) WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[index].first), tooltipText);
    }
    return options;
  }

  public void OnOptionSelected(
    FewOptionSideScreen.IFewOptionSideScreen.Option option)
  {
    this.ChosenBeverage = option.tag;
  }

  public Tag GetSelectedOption() => this.ChosenBeverage;

  public class States : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler>
  {
    public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State unoperational;
    public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State waitingfordelivery;
    public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State dispensing;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.unoperational;
      this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery).PlayAnim("off");
      this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.dispensing, (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.Transition.ConditionCallback) (smi => smi.HasMinimumMass())).EventTransition(GameHashes.OnStorageChange, this.dispensing, (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.Transition.ConditionCallback) (smi => smi.HasMinimumMass())).PlayAnim("off");
      this.dispensing.Enter("StartMeter", (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State.Callback) (smi => smi.StartMeter())).Enter("Set Active", (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State.Callback) (smi => smi.SetOperationalActiveState(true))).Enter("UpdateDrinkChores.force", (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State.Callback) (smi => smi.master.UpdateDrinkChores())).Update("UpdateDrinkChores", (Action<WaterCooler.StatesInstance, float>) ((smi, dt) => smi.master.UpdateDrinkChores())).Exit("CancelDrinkChores", (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State.Callback) (smi => smi.master.CancelDrinkChores())).Exit("Set Inactive", (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State.Callback) (smi => smi.SetOperationalActiveState(false))).TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (StateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.Transition.ConditionCallback) (smi => !smi.HasMinimumMass())).PlayAnim("working");
    }
  }

  public class StatesInstance : 
    GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.GameInstance
  {
    [MyCmpGet]
    private Operational operational;
    private Storage storage;
    private MeterController meter;

    public StatesInstance(WaterCooler smi)
      : base(smi)
    {
      this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_bottle", nameof (meter), Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[1]
      {
        "meter_bottle"
      });
      this.storage = this.master.GetComponent<Storage>();
      this.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
    }

    public void Drink(GameObject druplicant, bool triggerOnDrinkCallback = true)
    {
      if (!this.HasMinimumMass())
        return;
      Tag tag = this.storage.items[0].PrefabID();
      SimUtil.DiseaseInfo disease_info;
      this.storage.ConsumeAndGetDisease(tag, 1f, out float _, out disease_info, out float _);
      druplicant.GetSMI<GermExposureMonitor.Instance>()?.TryInjectDisease(disease_info.idx, disease_info.count, tag, Sickness.InfectionVector.Digestion);
      Effects component = druplicant.GetComponent<Effects>();
      for (int index = 0; index < WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS.Length; ++index)
      {
        Tuple<Tag, string> tuple = WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[index];
        Tag first = tuple.first;
        string second = tuple.second;
        if (tag == first && !string.IsNullOrEmpty(second))
        {
          component.Add(second, true);
          break;
        }
      }
      component.Remove("Thirsty");
      if (!triggerOnDrinkCallback)
        return;
      Action<GameObject, GameObject> onDuplicantDrank = WaterCooler.OnDuplicantDrank;
      if (onDuplicantDrank == null)
        return;
      onDuplicantDrank(druplicant, this.gameObject);
    }

    private void OnStorageChange(object data)
    {
      this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
    }

    public void SetOperationalActiveState(bool isActive) => this.operational.SetActive(isActive);

    public void StartMeter()
    {
      PrimaryElement firstWithMass = this.storage.FindFirstWithMass(this.smi.master.ChosenBeverage);
      if ((UnityEngine.Object) firstWithMass == (UnityEngine.Object) null)
        return;
      this.meter.SetSymbolTint(new KAnimHashedString("meter_water"), firstWithMass.Element.substance.colour);
      this.OnStorageChange((object) null);
    }

    public bool HasMinimumMass()
    {
      return (double) this.storage.GetMassAvailable(ElementLoader.GetElement(this.smi.master.ChosenBeverage).id) >= 1.0;
    }
  }
}
