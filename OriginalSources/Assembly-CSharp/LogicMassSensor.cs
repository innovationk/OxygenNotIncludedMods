// Decompiled with JetBrains decompiler
// Type: LogicMassSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMassSensor : Switch, ISaveLoadable, IThresholdSwitch
{
  [SerializeField]
  [Serialize]
  private float threshold;
  [SerializeField]
  [Serialize]
  private bool activateAboveThreshold = true;
  [MyCmpGet]
  private LogicPorts logicPorts;
  private bool was_pressed;
  private bool was_on;
  public float rangeMin;
  public float rangeMax = 1f;
  [Serialize]
  private float massSolid;
  [Serialize]
  private float massPickupables;
  [Serialize]
  private float massActivators;
  private const float MIN_TOGGLE_TIME = 0.15f;
  private float toggleCooldown = 0.15f;
  private HandleVector<int>.Handle solidChangedEntry;
  private HandleVector<int>.Handle pickupablesChangedEntry;
  private HandleVector<int>.Handle floorSwitchActivatorChangedEntry;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicMassSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicMassSensor>((Action<LogicMassSensor, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicMassSensor>(-905833192, LogicMassSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicMassSensor component = ((GameObject) data).GetComponent<LogicMassSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateVisualState(true);
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    this.solidChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SolidChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
    this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.PickupablesChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
    this.floorSwitchActivatorChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SwitchActivatorChanged", (object) this.gameObject, cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, new Action<object>(this.OnActivatorsChanged));
    this.OnToggle += new Action<bool>(this.SwitchToggled);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.solidChangedEntry);
    GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
    GameScenePartitioner.Instance.Free(ref this.floorSwitchActivatorChangedEntry);
    base.OnCleanUp();
  }

  private void Update()
  {
    this.toggleCooldown = Mathf.Max(0.0f, this.toggleCooldown - Time.deltaTime);
    if ((double) this.toggleCooldown != 0.0)
      return;
    float currentValue = this.CurrentValue;
    if ((this.activateAboveThreshold ? ((double) currentValue > (double) this.threshold ? 1 : 0) : ((double) currentValue < (double) this.threshold ? 1 : 0)) != (this.IsSwitchedOn ? 1 : 0))
    {
      this.Toggle();
      this.toggleCooldown = 0.15f;
    }
    this.UpdateVisualState();
  }

  private void OnSolidChanged(object data)
  {
    int i = Grid.CellAbove(this.NaturalBuildingCell());
    if (Grid.Solid[i])
      this.massSolid = Grid.Mass[i];
    else
      this.massSolid = 0.0f;
  }

  private void OnPickupablesChanged(object data)
  {
    float num = 0.0f;
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) gathered_entries);
    for (int index = 0; index < gathered_entries.Count; ++index)
    {
      Pickupable pickupable = gathered_entries[index].obj as Pickupable;
      if (!((UnityEngine.Object) pickupable == (UnityEngine.Object) null) && !pickupable.wasAbsorbed)
      {
        KPrefabID kprefabId = pickupable.KPrefabID;
        if (!kprefabId.HasTag(GameTags.Creature) || (kprefabId.HasTag(GameTags.Creatures.Walker) || kprefabId.HasTag(GameTags.Creatures.Hoverer) ? 1 : (kprefabId.HasTag(GameTags.Creatures.Flopping) ? 1 : 0)) != 0)
          num += pickupable.PrimaryElement.Mass;
      }
    }
    gathered_entries.Recycle();
    this.massPickupables = num;
  }

  private void OnActivatorsChanged(object data)
  {
    float num = 0.0f;
    int cell = Grid.CellAbove(this.NaturalBuildingCell());
    ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.floorSwitchActivatorLayer, (List<ScenePartitionerEntry>) gathered_entries);
    for (int index = 0; index < gathered_entries.Count; ++index)
    {
      FloorSwitchActivator floorSwitchActivator = gathered_entries[index].obj as FloorSwitchActivator;
      if (!((UnityEngine.Object) floorSwitchActivator == (UnityEngine.Object) null))
        num += floorSwitchActivator.PrimaryElement.Mass;
    }
    gathered_entries.Recycle();
    this.massActivators = num;
  }

  public LocString Title => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;

  public float Threshold
  {
    get => this.threshold;
    set => this.threshold = value;
  }

  public bool ActivateAboveThreshold
  {
    get => this.activateAboveThreshold;
    set => this.activateAboveThreshold = value;
  }

  public float CurrentValue => this.massSolid + this.massPickupables + this.massActivators;

  public float RangeMin => this.rangeMin;

  public float RangeMax => this.rangeMax;

  public float GetRangeMinInputField() => this.rangeMin;

  public float GetRangeMaxInputField() => this.rangeMax;

  public LocString ThresholdValueName => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;

  public string AboveToolTip
  {
    get => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
  }

  public string BelowToolTip
  {
    get => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
  }

  public string Format(float value, bool units)
  {
    GameUtil.MetricMassFormat metricMassFormat = GameUtil.MetricMassFormat.Kilogram;
    double mass = (double) value;
    bool flag = units;
    int massFormat = (int) metricMassFormat;
    int num = flag ? 1 : 0;
    return GameUtil.GetFormattedMass((float) mass, massFormat: (GameUtil.MetricMassFormat) massFormat, includeSuffix: num != 0);
  }

  public float ProcessedSliderValue(float input)
  {
    input = Mathf.Round(input);
    return input;
  }

  public float ProcessedInputValue(float input) => input;

  public LocString ThresholdValueUnits() => GameUtil.GetCurrentMassUnit();

  public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

  public int IncrementScale => 1;

  public NonLinearSlider.Range[] GetRanges => NonLinearSlider.GetDefaultRange(this.RangeMax);

  private void SwitchToggled(bool toggled_on)
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, toggled_on ? 1 : 0);
  }

  private void UpdateVisualState(bool force = false)
  {
    bool flag = (double) this.CurrentValue > (double) this.threshold;
    if (((flag != this.was_pressed ? 1 : (this.was_on != this.IsSwitchedOn ? 1 : 0)) | (force ? 1 : 0)) == 0)
      return;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (flag)
    {
      if (force)
      {
        component.Play((HashedString) (this.IsSwitchedOn ? "on_down" : "off_down"));
      }
      else
      {
        component.Play((HashedString) (this.IsSwitchedOn ? "on_down_pre" : "off_down_pre"));
        component.Queue((HashedString) (this.IsSwitchedOn ? "on_down" : "off_down"));
      }
    }
    else if (force)
    {
      component.Play((HashedString) (this.IsSwitchedOn ? "on_up" : "off_up"));
    }
    else
    {
      component.Play((HashedString) (this.IsSwitchedOn ? "on_up_pre" : "off_up_pre"));
      component.Queue((HashedString) (this.IsSwitchedOn ? "on_up" : "off_up"));
    }
    this.was_pressed = flag;
    this.was_on = this.IsSwitchedOn;
  }

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }
}
