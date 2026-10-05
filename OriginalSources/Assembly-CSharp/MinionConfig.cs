// Decompiled with JetBrains decompiler
// Type: MinionConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using TUNING;
using UnityEngine;

#nullable disable
public class MinionConfig : IEntityConfig
{
  public static Tag MODEL = GameTags.Minions.Models.Standard;
  public static string NAME = (string) DUPLICANTS.MODEL.STANDARD.NAME;
  public static string ID = MinionConfig.MODEL.ToString();
  public Func<RationalAi.Instance, StateMachine.Instance>[] RATIONAL_AI_STATE_MACHINES = BaseMinionConfig.BaseRationalAiStateMachines().Append<Func<RationalAi.Instance, StateMachine.Instance>>(new Func<RationalAi.Instance, StateMachine.Instance>[9]
  {
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BreathMonitor.Instance(smi.master)),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SteppedInMonitor.Instance(smi.master)),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new Dreamer.Instance(smi.master)),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new StaminaMonitor.Instance(smi.master)),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RationMonitor.Instance(smi.master)),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new CalorieMonitor.Instance(smi.master)),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BladderMonitor.Instance(smi.master)),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new HygieneMonitor.Instance(smi.master)),
    (Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TiredMonitor.Instance(smi.master))
  });

  public static string[] GetAttributes()
  {
    return BaseMinionConfig.BaseMinionAttributes().Append<string>(new string[2]
    {
      Db.Get().Attributes.FoodExpectation.Id,
      Db.Get().Attributes.ToiletEfficiency.Id
    });
  }

  public static string[] GetAmounts()
  {
    return BaseMinionConfig.BaseMinionAmounts().Append<string>(new string[3]
    {
      Db.Get().Amounts.Bladder.Id,
      Db.Get().Amounts.Stamina.Id,
      Db.Get().Amounts.Calories.Id
    });
  }

  public static AttributeModifier[] GetTraits()
  {
    return BaseMinionConfig.BaseMinionTraits(MinionConfig.MODEL).Append<AttributeModifier>(new AttributeModifier[6]
    {
      new AttributeModifier(Db.Get().Attributes.FoodExpectation.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.FOOD_QUALITY_EXPECTATION, MinionConfig.NAME),
      new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.MAX_CALORIES, MinionConfig.NAME),
      new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.CALORIES_BURNED_PER_SECOND, MinionConfig.NAME),
      new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.STAMINA_USED_PER_SECOND, MinionConfig.NAME),
      new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.BLADDER_INCREASE_PER_SECOND, MinionConfig.NAME),
      new AttributeModifier(Db.Get().Attributes.ToiletEfficiency.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.TOILET_EFFICIENCY, MinionConfig.NAME)
    });
  }

  public GameObject CreatePrefab()
  {
    GameObject go = BaseMinionConfig.BaseMinion(MinionConfig.MODEL, MinionConfig.GetAttributes(), MinionConfig.GetAmounts(), MinionConfig.GetTraits());
    go.AddOrGet<CodexEntryRedirector>().CodexID = "DUPLICANTS";
    return go;
  }

  public void OnPrefabInit(GameObject go)
  {
    BaseMinionConfig.BasePrefabInit(go, MinionConfig.MODEL);
    DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL);
    Db.Get().Amounts.Bladder.Lookup(go).value = UnityEngine.Random.Range(0.0f, 10f);
    AmountInstance amountInstance1 = Db.Get().Amounts.Calories.Lookup(go);
    amountInstance1.value = (float) (((double) statsFor.BaseStats.HUNGRY_THRESHOLD + (double) statsFor.BaseStats.SATISFIED_THRESHOLD) * 0.5) * amountInstance1.GetMax();
    AmountInstance amountInstance2 = Db.Get().Amounts.Stamina.Lookup(go);
    amountInstance2.value = amountInstance2.GetMax();
  }

  public void OnSpawn(GameObject go)
  {
    Sensors component = go.GetComponent<Sensors>();
    component.Add((Sensor) new ToiletSensor(component));
    BaseMinionConfig.BaseOnSpawn(go, MinionConfig.MODEL, this.RATIONAL_AI_STATE_MACHINES);
    go.GetComponent<OxygenBreather>().AddGasProvider((OxygenBreather.IGasProvider) new GasBreatherFromWorldProvider());
    go.Trigger(1589886948, (object) go);
  }
}
