// Decompiled with JetBrains decompiler
// Type: BeehiveCalorieMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class BeehiveCalorieMonitor : 
  GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>
{
  public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State normal;
  public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State hungry;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.normal;
    this.serializable = StateMachine.SerializeType.Both_DEPRECATED;
    this.root.EventHandler(GameHashes.CaloriesConsumed, (GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.GameEvent.Callback) ((smi, data) => smi.OnCaloriesConsumed(data))).Update(new System.Action<BeehiveCalorieMonitor.Instance, float>(BeehiveCalorieMonitor.UpdateMetabolismCalorieModifier));
    this.normal.Transition(this.hungry, (StateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsHungry()), UpdateRate.SIM_1000ms);
    this.hungry.ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (StateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsHungry())).ToggleStatusItem(Db.Get().CreatureStatusItems.HiveHungry).Transition(this.normal, (StateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsHungry()), UpdateRate.SIM_1000ms);
  }

  private static bool ReadyToPoop(BeehiveCalorieMonitor.Instance smi)
  {
    return smi.stomach.IsReadyToPoop() && (double) Time.time - (double) smi.lastMealOrPoopTime >= (double) smi.def.minimumTimeBeforePooping;
  }

  private static void UpdateMetabolismCalorieModifier(BeehiveCalorieMonitor.Instance smi, float dt)
  {
    smi.deltaCalorieMetabolismModifier.SetValue((float) (1.0 - (double) smi.metabolism.GetTotalValue() / 100.0));
    if (!BeehiveCalorieMonitor.ReadyToPoop(smi))
      return;
    smi.Poop();
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public Diet diet;
    public float minConsumedCaloriesBeforePooping = 100f;
    public float minimumTimeBeforePooping = 10f;

    public override void Configure(GameObject prefab)
    {
      prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
    }

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      List<Descriptor> descriptors = new List<Descriptor>();
      descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.DIET_HEADER, (string) UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER));
      float calorie_loss_per_second = 0.0f;
      foreach (AttributeModifier selfModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
      {
        if (selfModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
          calorie_loss_per_second = selfModifier.Value;
      }
      BeehiveCalorieMonitor.Instance smi = obj.GetSMI<BeehiveCalorieMonitor.Instance>();
      string newValue1 = string.Join(", ", smi.stomach.diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>());
      string newValue2 = string.Join("\n", smi.stomach.diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram)))).ToArray<string>());
      descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue1), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue2)));
      string newValue3 = string.Join(", ", smi.stomach.diet.producedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>());
      string newValue4 = string.Join("\n", smi.stomach.diet.producedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f)))).ToArray<string>());
      descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue3), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", newValue4)));
      return descriptors;
    }
  }

  public new class Instance : 
    GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.GameInstance
  {
    public const float HUNGRY_RATIO = 0.9f;
    public AmountInstance calories;
    [Serialize]
    public CreatureCalorieMonitor.Stomach stomach;
    public float lastMealOrPoopTime;
    public AttributeInstance metabolism;
    public AttributeModifier deltaCalorieMetabolismModifier;

    public Instance(IStateMachineTarget master, BeehiveCalorieMonitor.Def def)
      : base(master, def)
    {
      this.calories = Db.Get().Amounts.Calories.Lookup(this.gameObject);
      this.calories.value = this.calories.GetMax() * 0.9f;
      this.stomach = new CreatureCalorieMonitor.Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, -1f);
      this.metabolism = this.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
      this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, (string) DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, is_readonly: false);
      this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
    }

    public void OnCaloriesConsumed(object data)
    {
      CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = ((Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>) data).value;
      this.calories.value += caloriesConsumedEvent.calories;
      this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
      this.lastMealOrPoopTime = Time.time;
    }

    public void Poop()
    {
      this.lastMealOrPoopTime = Time.time;
      this.stomach.PoopInStorage(this.GetComponent<Storage>());
    }

    public float GetCalories0to1() => this.calories.value / this.calories.GetMax();

    public bool IsHungry() => (double) this.GetCalories0to1() < 0.89999997615814209;
  }
}
