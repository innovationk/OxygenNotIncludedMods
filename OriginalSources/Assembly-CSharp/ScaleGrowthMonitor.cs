// Decompiled with JetBrains decompiler
// Type: ScaleGrowthMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ScaleGrowthMonitor : 
  GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>
{
  public ScaleGrowthMonitor.GrowingState growing;
  public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State fullyGrown;
  private AttributeModifier scaleGrowthModifier;
  private static HashedString[] SCALE_SYMBOL_NAMES = new HashedString[5]
  {
    (HashedString) "scale_0",
    (HashedString) "scale_1",
    (HashedString) "scale_2",
    (HashedString) "scale_3",
    (HashedString) "scale_4"
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.growing;
    this.root.Enter((StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback) (smi => ScaleGrowthMonitor.UpdateScales(smi, 0.0f))).Update(new System.Action<ScaleGrowthMonitor.Instance, float>(ScaleGrowthMonitor.UpdateScales), UpdateRate.SIM_1000ms);
    this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
    this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere)), UpdateRate.SIM_1000ms).Enter(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.RemoveModifier));
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state = this.growing.stunted.Transition(this.growing.growing, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere), UpdateRate.SIM_1000ms);
    string name = (string) CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.NAME;
    string tooltip = (string) CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state.ToggleStatusItem(name, tooltip, render_overlay: render_overlay, category: category);
    this.fullyGrown.ToggleBehaviour(GameTags.Creatures.ScalesGrown, (StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback) (smi => smi.HasTag(GameTags.Creatures.CanMolt))).Transition((GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State) this.growing, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
  }

  private static bool IsInCorrectAtmosphere(ScaleGrowthMonitor.Instance smi)
  {
    if (smi.def.targetAtmosphere == (SimHashes) 0)
      return true;
    int cell = Grid.PosToCell((StateMachine.Instance) smi);
    return Grid.IsValidCell(cell) && Grid.Element[cell].id == smi.def.targetAtmosphere;
  }

  private static bool AreScalesFullyGrown(ScaleGrowthMonitor.Instance smi)
  {
    return (double) smi.scaleGrowth.value >= (double) smi.scaleGrowth.GetMax();
  }

  private static void ApplyModifier(ScaleGrowthMonitor.Instance smi)
  {
    smi.scaleGrowth.deltaAttribute.Add(smi.scaleGrowthModifier);
  }

  private static void RemoveModifier(ScaleGrowthMonitor.Instance smi)
  {
    smi.scaleGrowth.deltaAttribute.Remove(smi.scaleGrowthModifier);
  }

  private static void UpdateScales(ScaleGrowthMonitor.Instance smi, float dt)
  {
    int num = (int) ((double) smi.def.levelCount * (double) smi.scaleGrowth.value / 100.0);
    if (smi.currentScaleLevel == num)
      return;
    KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
    for (int index = 0; index < smi.def.symbolNames.Length; ++index)
    {
      bool is_visible = index <= num - 1;
      component.SetSymbolVisiblity((KAnimHashedString) smi.def.symbolNames[index], is_visible);
    }
    smi.currentScaleLevel = num;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public int levelCount;
    public HashedString[] symbolNames;
    public float defaultGrowthRate;
    public SimHashes targetAtmosphere;
    public Tag itemDroppedOnShear;
    public float dropMass;

    public override void Configure(GameObject prefab)
    {
      prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
      if (this.symbolNames != null)
        return;
      this.symbolNames = ScaleGrowthMonitor.SCALE_SYMBOL_NAMES;
    }

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      List<Descriptor> descriptors = new List<Descriptor>();
      if (this.targetAtmosphere == (SimHashes) 0)
        descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate))));
      else
        descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName())));
      return descriptors;
    }
  }

  public class GrowingState : 
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State
  {
    public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State growing;
    public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State stunted;
  }

  public new class Instance : 
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.GameInstance,
    IShearable
  {
    public AmountInstance scaleGrowth;
    public AttributeModifier scaleGrowthModifier;
    public int currentScaleLevel = -1;

    public Instance(IStateMachineTarget master, ScaleGrowthMonitor.Def def)
      : base(master, def)
    {
      this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(this.gameObject);
      this.scaleGrowth.value = this.scaleGrowth.GetMax();
      this.scaleGrowthModifier = new AttributeModifier(this.scaleGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, (string) CREATURES.MODIFIERS.SCALE_GROWTH_RATE.NAME);
    }

    public bool IsFullyGrown() => this.currentScaleLevel == this.def.levelCount;

    public void Shear()
    {
      this.scaleGrowth.value = 0.0f;
      ScaleGrowthMonitor.UpdateScales(this, 0.0f);
    }

    public Tuple<Tag, float> GetItemDroppedOnShear()
    {
      return new Tuple<Tag, float>(this.def.itemDroppedOnShear, this.def.dropMass);
    }
  }
}
