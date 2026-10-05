// Decompiled with JetBrains decompiler
// Type: LogicHEPSensor
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
public class LogicHEPSensor : Switch, ISaveLoadable, IThresholdSwitch, ISimEveryTick
{
  [Serialize]
  public float thresholdPayload;
  [Serialize]
  public bool activateOnHigherThan;
  [Serialize]
  public bool dirty = true;
  private readonly float minPayload;
  private readonly float maxPayload = 500f;
  private float foundPayload;
  private bool waitForLogicTick;
  private bool wasOn;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<LogicHEPSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicHEPSensor>((Action<LogicHEPSensor, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LogicHEPSensor>(-905833192, LogicHEPSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    LogicHEPSensor component = ((GameObject) data).GetComponent<LogicHEPSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.OnToggle += new Action<bool>(this.OnSwitchToggled);
    this.UpdateLogicCircuit();
    this.UpdateVisualState(true);
    this.wasOn = this.switchedOn;
    Game.Instance.logicCircuitManager.onLogicTick += new System.Action(this.LogicTick);
  }

  protected override void OnCleanUp()
  {
    Game.Instance.logicCircuitManager.onLogicTick -= new System.Action(this.LogicTick);
    base.OnCleanUp();
  }

  public void SimEveryTick(float dt)
  {
    if (this.waitForLogicTick)
      return;
    Vector2I xy = Grid.CellToXY(Grid.PosToCell((KMonoBehaviour) this));
    ListPool<ScenePartitionerEntry, LogicHEPSensor>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, LogicHEPSensor>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(xy.x, xy.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, (List<ScenePartitionerEntry>) gathered_entries);
    float num = 0.0f;
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
    {
      HighEnergyParticle component = (partitionerEntry.obj as KCollider2D).gameObject.GetComponent<HighEnergyParticle>();
      if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.isCollideable)
        num += component.payload;
    }
    gathered_entries.Recycle();
    this.foundPayload = num;
    bool on = this.activateOnHigherThan && (double) num > (double) this.thresholdPayload || !this.activateOnHigherThan && (double) num < (double) this.thresholdPayload;
    if (on != this.switchedOn)
      this.waitForLogicTick = true;
    this.SetState(on);
  }

  private void LogicTick() => this.waitForLogicTick = false;

  private void OnSwitchToggled(bool toggled_on)
  {
    this.UpdateLogicCircuit();
    this.UpdateVisualState();
  }

  private void UpdateLogicCircuit()
  {
    this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
  }

  private void UpdateVisualState(bool force = false)
  {
    if (!(this.wasOn != this.switchedOn | force))
      return;
    this.wasOn = this.switchedOn;
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) (this.switchedOn ? "on_pre" : "on_pst"));
    component.Queue((HashedString) (this.switchedOn ? "on" : "off"));
  }

  protected override void UpdateSwitchStatus()
  {
    StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
  }

  public float Threshold
  {
    get => this.thresholdPayload;
    set
    {
      this.thresholdPayload = value;
      this.dirty = true;
    }
  }

  public bool ActivateAboveThreshold
  {
    get => this.activateOnHigherThan;
    set
    {
      this.activateOnHigherThan = value;
      this.dirty = true;
    }
  }

  public float CurrentValue => this.foundPayload;

  public float RangeMin => this.minPayload;

  public float RangeMax => this.maxPayload;

  public float GetRangeMinInputField() => this.minPayload;

  public float GetRangeMaxInputField() => this.maxPayload;

  public LocString Title => UI.UISIDESCREENS.HEPSWITCHSIDESCREEN.TITLE;

  public LocString ThresholdValueName => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS;

  public string AboveToolTip
  {
    get => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_ABOVE;
  }

  public string BelowToolTip
  {
    get => (string) UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_BELOW;
  }

  public string Format(float value, bool units)
  {
    return GameUtil.GetFormattedHighEnergyParticles(value, displayUnits: units);
  }

  public float ProcessedSliderValue(float input) => Mathf.Round(input);

  public float ProcessedInputValue(float input) => input;

  public LocString ThresholdValueUnits() => UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;

  public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

  public int IncrementScale => 1;

  public NonLinearSlider.Range[] GetRanges
  {
    get
    {
      return new NonLinearSlider.Range[3]
      {
        new NonLinearSlider.Range(30f, 50f),
        new NonLinearSlider.Range(30f, 200f),
        new NonLinearSlider.Range(40f, 500f)
      };
    }
  }
}
