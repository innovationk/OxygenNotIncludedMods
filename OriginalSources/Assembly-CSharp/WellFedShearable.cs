// Decompiled with JetBrains decompiler
// Type: WellFedShearable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WellFedShearable : 
  GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>
{
  public WellFedShearable.GrowingState growing;
  public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State fullyGrown;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.growing;
    this.root.Enter((StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State.Callback) (smi => WellFedShearable.UpdateScales(smi, 0.0f))).Enter((StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State.Callback) (smi =>
    {
      if (smi.def.hideSymbols == null)
        return;
      foreach (KAnimHashedString hideSymbol in smi.def.hideSymbols)
        smi.animController.SetSymbolVisiblity(hideSymbol, false);
    })).Update(new System.Action<WellFedShearable.Instance, float>(WellFedShearable.UpdateScales), UpdateRate.SIM_1000ms).EventHandler(GameHashes.CaloriesConsumed, (GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.GameEvent.Callback) ((smi, data) => smi.OnCaloriesConsumed(data)));
    this.growing.Enter((StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State.Callback) (smi => WellFedShearable.UpdateScales(smi, 0.0f))).DefaultState(this.growing.stalled).Transition(this.fullyGrown, new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
    this.growing.stalled.ToggleCritterEmotion(Db.Get().CritterEmotions.WellFed).Transition(this.growing.growing, (StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback) (smi => smi.HasWellFedEffect()), UpdateRate.SIM_1000ms);
    this.growing.growing.Transition(this.growing.stalled, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not((StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback) (smi => smi.HasWellFedEffect())), UpdateRate.SIM_1000ms).ToggleCritterEmotion(Db.Get().CritterEmotions.WellFed);
    this.fullyGrown.Enter((StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State.Callback) (smi => WellFedShearable.UpdateScales(smi, 0.0f))).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback) (smi => smi.HasTag(GameTags.Creatures.CanMolt))).EventTransition(GameHashes.Molt, (GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State) this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown))).Transition((GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State) this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
  }

  private static bool AreScalesFullyGrown(WellFedShearable.Instance smi)
  {
    return (double) smi.scaleGrowth.value >= (double) smi.scaleGrowth.GetMax();
  }

  private static void UpdateScales(WellFedShearable.Instance smi, float dt)
  {
    int num = (int) ((double) smi.def.levelCount * (double) smi.scaleGrowth.value / 100.0);
    if (smi.currentScaleLevel == num)
      return;
    for (int index = 0; index < smi.def.scaleGrowthSymbols.Length; ++index)
    {
      bool is_visible = index <= num - 1;
      smi.animController.SetSymbolVisiblity(smi.def.scaleGrowthSymbols[index], is_visible);
    }
    smi.currentScaleLevel = num;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public string effectId;
    public float caloriesPerCycle;
    public float growthDurationCycles;
    public int levelCount;
    public Tag itemDroppedOnShear;
    public float dropMass;
    public Tag requiredDiet = (Tag) (string) null;
    public KAnimHashedString[] scaleGrowthSymbols = WellFedShearable.Def.SCALE_SYMBOL_NAMES;
    public KAnimHashedString[] hideSymbols;
    public static KAnimHashedString[] SCALE_SYMBOL_NAMES = new KAnimHashedString[5]
    {
      (KAnimHashedString) "scale_0",
      (KAnimHashedString) "scale_1",
      (KAnimHashedString) "scale_2",
      (KAnimHashedString) "scale_3",
      (KAnimHashedString) "scale_4"
    };

    public override void Configure(GameObject prefab)
    {
      prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
    }

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      return new List<Descriptor>()
      {
        new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_FED.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f)))
      };
    }
  }

  public class GrowingState : 
    GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State
  {
    public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State growing;
    public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State stalled;
  }

  public new class Instance : 
    GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.GameInstance,
    IShearable
  {
    [MyCmpGet]
    private Effects effects;
    [MyCmpGet]
    public KBatchedAnimController animController;
    public AmountInstance scaleGrowth;
    public int currentScaleLevel = -1;

    public Instance(IStateMachineTarget master, WellFedShearable.Def def)
      : base(master, def)
    {
      this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(this.gameObject);
      this.scaleGrowth.value = this.scaleGrowth.GetMax();
    }

    public bool IsFullyGrown() => this.currentScaleLevel == this.def.levelCount;

    public void OnCaloriesConsumed(object data)
    {
      CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>.Unbox(data);
      if (this.def.requiredDiet != (Tag) (string) null && caloriesConsumedEvent.tag != this.def.requiredDiet)
        return;
      (this.effects.Get(this.smi.def.effectId) ?? this.effects.Add(this.smi.def.effectId, true)).timeRemaining += (float) ((double) caloriesConsumedEvent.calories / (double) this.smi.def.caloriesPerCycle * 600.0);
    }

    public bool HasWellFedEffect() => this.effects.Get(this.def.effectId) != null;

    public void Shear()
    {
      this.scaleGrowth.value = 0.0f;
      WellFedShearable.UpdateScales(this, 0.0f);
    }

    public Tuple<Tag, float> GetItemDroppedOnShear()
    {
      return new Tuple<Tag, float>(this.def.itemDroppedOnShear, this.def.dropMass);
    }
  }
}
