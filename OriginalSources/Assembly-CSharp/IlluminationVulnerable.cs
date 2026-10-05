// Decompiled with JetBrains decompiler
// Type: IlluminationVulnerable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[SkipSaveFileSerialization]
public class IlluminationVulnerable : 
  StateMachineComponent<IlluminationVulnerable.StatesInstance>,
  IGameObjectEffectDescriptor,
  IWiltCause,
  IIlluminationTracker
{
  private OccupyArea _occupyArea;
  private SchedulerHandle handle;
  public bool prefersDarkness;
  private AttributeInstance minLuxAttributeInstance;

  public int LightIntensityThreshold
  {
    get
    {
      return this.minLuxAttributeInstance != null ? Mathf.RoundToInt(this.minLuxAttributeInstance.GetTotalValue()) : Mathf.RoundToInt(this.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.MinLightLux));
    }
  }

  public string GetIlluminationUITooltip()
  {
    return this.prefersDarkness && this.IsComfortable() || !this.prefersDarkness && !this.IsComfortable() ? (string) UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_DARK : (string) UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_LIGHT;
  }

  public string GetIlluminationUILabel()
  {
    return $"{Db.Get().Amounts.Illumination.Name}\n    • {(this.prefersDarkness ? UI.GAMEOBJECTEFFECTS.DARKNESS.ToString() : GameUtil.GetFormattedLux(this.LightIntensityThreshold))}";
  }

  public bool ShouldIlluminationUICheckboxBeChecked() => this.IsComfortable();

  private OccupyArea occupyArea
  {
    get
    {
      if ((UnityEngine.Object) this._occupyArea == (UnityEngine.Object) null)
        this._occupyArea = this.GetComponent<OccupyArea>();
      return this._occupyArea;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Illumination, this.gameObject));
    this.minLuxAttributeInstance = this.gameObject.GetAttributes().Add(Db.Get().PlantAttributes.MinLightLux);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public void SetPrefersDarkness(bool prefersDarkness = false)
  {
    this.prefersDarkness = prefersDarkness;
  }

  protected override void OnCleanUp()
  {
    this.handle.ClearScheduler();
    base.OnCleanUp();
  }

  public bool IsCellSafe(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    return this.prefersDarkness ? Grid.LightIntensity[cell] == 0 : Grid.LightIntensity[cell] >= this.LightIntensityThreshold;
  }

  WiltCondition.Condition[] IWiltCause.Conditions
  {
    get
    {
      return new WiltCondition.Condition[2]
      {
        WiltCondition.Condition.Darkness,
        WiltCondition.Condition.IlluminationComfort
      };
    }
  }

  public string WiltStateString
  {
    get
    {
      if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.too_bright))
        return Db.Get().CreatureStatusItems.Crop_Too_Bright.GetName((object) this);
      return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.too_dark) ? Db.Get().CreatureStatusItems.Crop_Too_Dark.GetName((object) this) : "";
    }
  }

  public bool IsComfortable()
  {
    return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.comfortable);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    if (this.prefersDarkness)
      return new List<Descriptor>()
      {
        new Descriptor((string) UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_DARKNESS, Descriptor.DescriptorType.Requirement)
      };
    return new List<Descriptor>()
    {
      new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.LightIntensityThreshold)), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.LightIntensityThreshold)), Descriptor.DescriptorType.Requirement)
    };
  }

  public class StatesInstance(IlluminationVulnerable master) : 
    GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.GameInstance(master)
  {
    public bool hasMaturity;
  }

  public class States : 
    GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable>
  {
    public StateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.BoolParameter illuminated;
    public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State comfortable;
    public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_dark;
    public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_bright;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.comfortable;
      this.root.Update("Illumination", (Action<IlluminationVulnerable.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject);
        if (Grid.IsValidCell(cell))
        {
          double num1 = (double) smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue((float) Grid.LightCount[cell]);
        }
        else
        {
          double num2 = (double) smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue(0.0f);
        }
      }), UpdateRate.SIM_1000ms);
      this.comfortable.Update("Illumination.Comfortable", (Action<IlluminationVulnerable.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject);
        if (smi.master.IsCellSafe(cell))
          return;
        GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State state = smi.master.prefersDarkness ? this.too_bright : this.too_dark;
        smi.GoTo((StateMachine.BaseState) state);
      }), UpdateRate.SIM_1000ms).Enter((StateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State.Callback) (smi => smi.master.Trigger(1113102781, (object) null)));
      this.too_dark.TriggerOnEnter(GameHashes.IlluminationDiscomfort).Update("Illumination.too_dark", (Action<IlluminationVulnerable.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject);
        if (!smi.master.IsCellSafe(cell))
          return;
        smi.GoTo((StateMachine.BaseState) this.comfortable);
      }), UpdateRate.SIM_1000ms);
      this.too_bright.TriggerOnEnter(GameHashes.IlluminationDiscomfort).Update("Illumination.too_bright", (Action<IlluminationVulnerable.StatesInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.gameObject);
        if (!smi.master.IsCellSafe(cell))
          return;
        smi.GoTo((StateMachine.BaseState) this.comfortable);
      }), UpdateRate.SIM_1000ms);
    }
  }
}
