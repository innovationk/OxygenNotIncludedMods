// Decompiled with JetBrains decompiler
// Type: MoistureMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class MoistureMonitor : 
  GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>
{
  private GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.State onDryLand;
  private GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.State inLiquid;
  private GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.State secreting;
  public GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.State dead;
  public static readonly HashedString RECENTLY_PRODUCED_LUBRICANT_EFFECT = (HashedString) "RecentlyProducedLubricant";

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.onDryLand;
    this.root.ToggleStateMachine((Func<MoistureMonitor.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new LubricatedMovementMonitor.Instance(smi.master))).EventHandler(GameHashes.Happy, (StateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.State.Callback) (smi => this.ToggleUnhappyModifier(smi, false))).EventHandler(GameHashes.Unhappy, (StateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.State.Callback) (smi => this.ToggleUnhappyModifier(smi, true))).EventHandler(GameHashes.TagsChanged, new GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.GameEvent.Callback(this.UpdateTags)).EventTransition(GameHashes.Died, this.dead).Enter((StateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.State.Callback) (smi =>
    {
      if (!smi.HasTag(GameTags.Creatures.Wild))
        return;
      smi.attributes.Add(smi.wildMucusModifier);
    }));
    this.onDryLand.UpdateTransition(this.inLiquid, new Func<MoistureMonitor.Instance, float, bool>(MoistureMonitor.IsInLiquid)).ToggleAttributeModifier("dry", (Func<MoistureMonitor.Instance, AttributeModifier>) (smi => smi.baseMoistureModifier)).ToggleAttributeModifier("mucus", (Func<MoistureMonitor.Instance, AttributeModifier>) (smi => smi.onDryLandMucusModifier)).UpdateTransition(this.secreting, new Func<MoistureMonitor.Instance, float, bool>(this.IsMucusEnough));
    this.inLiquid.UpdateTransition(this.onDryLand, (Func<MoistureMonitor.Instance, float, bool>) ((smi, dt) => !MoistureMonitor.IsInLiquid(smi, dt))).ToggleAttributeModifier("wet", (Func<MoistureMonitor.Instance, AttributeModifier>) (smi => smi.wetMoistureModifier));
    this.secreting.ToggleBehaviour(GameTags.Creatures.Behaviours.SecretingMucusBehavior, new StateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.Transition.ConditionCallback(MoistureMonitor.CanProduceLubricant), (System.Action<MoistureMonitor.Instance>) (smi => smi.GoTo((StateMachine.BaseState) this.onDryLand)));
    this.dead.DoNothing();
  }

  private static bool IsInLiquid(MoistureMonitor.Instance smi, float _)
  {
    return Grid.IsSubstantialLiquid(Grid.PosToCell((StateMachine.Instance) smi), 0.02f);
  }

  private bool IsMucusEnough(MoistureMonitor.Instance smi, float _)
  {
    return (double) smi.mucusAmount.value >= (smi.HasTag(GameTags.Creatures.Dry) ? 2.0 : 10.0);
  }

  private void UpdateTags(MoistureMonitor.Instance smi, object data)
  {
    if (!(data is TagChangedEventData changedEventData) || !(changedEventData.tag == GameTags.Creatures.Wild))
      return;
    if (changedEventData.added)
      smi.attributes.Add(smi.wildMucusModifier);
    else
      smi.attributes.Remove(smi.wildMucusModifier);
  }

  private void ToggleUnhappyModifier(MoistureMonitor.Instance smi, bool enabled)
  {
    if (enabled)
      smi.attributes.Add(smi.unhappyMucusModifier);
    else
      smi.attributes.Remove(smi.unhappyMucusModifier);
  }

  private static bool CanProduceLubricant(MoistureMonitor.Instance smi)
  {
    if (smi.effects.HasEffect(MoistureMonitor.RECENTLY_PRODUCED_LUBRICANT_EFFECT))
      return false;
    int cell = Grid.CellBelow(Grid.PosToCell((StateMachine.Instance) smi));
    return Grid.IsValidCell(cell) && Grid.IsSolidCell(cell);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    private const float DEFAULT_ON_DRY_LAND_MODIFIER = 0.05f;
    private const float DEFAULT_DRY_RATE = -0.05f;
    private const float DEFAULT_SOAK_RATE = 10f;
    public float onDryLandModifier = 0.05f;
    public SimHashes lubricant;
    public float lubricantTemperatureKelvin;
    [Tooltip("Stop producing more Mucus when this much is stored")]
    public float sufficientMoistureThreshold = 10f;
    [Tooltip("Decrease moisture at this rate while on dry land")]
    public float dryRate = -0.05f;
    [Tooltip("Increase moisture at this rate while inside liquid")]
    public float soakRate = 10f;

    public override void Configure(GameObject prefab)
    {
      Database.Amounts amounts = Db.Get().Amounts;
      List<string> initialAmounts = prefab.GetComponent<Modifiers>().initialAmounts;
      initialAmounts.Add(amounts.Moisture.Id);
      initialAmounts.Add(amounts.Mucus.Id);
    }

    private static void AppendMucusModifierTooltipBullet(
      StringBuilder tooltipBuilder,
      AttributeModifier modifier,
      bool showSign)
    {
      string text = modifier.IsMultiplier ? GameUtil.GetFormattedPercent(modifier.Value * 100f) : GameUtil.GetFormattedMass(modifier.Value, GameUtil.TimeSlice.PerCycle);
      if (showSign)
        text = GameUtil.AddPositiveSign(text, (double) modifier.Value > 0.0);
      tooltipBuilder.AppendFormat((string) DUPLICANTS.ATTRIBUTES.MODIFIER_ENTRY, (object) modifier.GetDescription(), (object) text);
    }

    public float GetMaxModification() => this.onDryLandModifier;

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      string newValue = ElementLoader.FindElementByHash(this.lubricant).tag.ProperName();
      float mass = this.onDryLandModifier;
      AmountInstance amountInstance = Db.Get().Amounts.Mucus.Lookup(obj);
      if (amountInstance != null)
        mass = amountInstance.GetDelta();
      string formattedMass = GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerCycle);
      string txt = GlobalStringBuilderPool.ReturnAndFree(GlobalStringBuilderPool.Alloc().Append((string) UI.BUILDINGEFFECTS.MUCUS_SECRETION).Replace("{Item}", newValue).Replace("{Rate}", formattedMass));
      StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
      stringBuilder.Append((string) UI.BUILDINGEFFECTS.TOOLTIPS.MUCUS_SECRETION);
      stringBuilder.Replace("{Item}", newValue);
      stringBuilder.Replace("{Rate}", formattedMass);
      if (amountInstance != null)
      {
        ArrayRef<AttributeModifier> modifiers = amountInstance.deltaAttribute.Modifiers;
        int i1 = -1;
        for (int i2 = 0; i2 < modifiers.Count; ++i2)
        {
          if (modifiers[i2].GetDescription() == (string) CREATURES.MODIFIERS.MUCUS.BASE_RATE)
          {
            i1 = i2;
            break;
          }
        }
        if (i1 >= 0)
          MoistureMonitor.Def.AppendMucusModifierTooltipBullet(stringBuilder, modifiers[i1], false);
        for (int i3 = 0; i3 < modifiers.Count; ++i3)
        {
          if (i3 != i1)
            MoistureMonitor.Def.AppendMucusModifierTooltipBullet(stringBuilder, modifiers[i3], true);
        }
      }
      string tooltip = GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
      return new List<Descriptor>()
      {
        new Descriptor(txt, tooltip)
      };
    }
  }

  public new class Instance : 
    GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>.GameInstance
  {
    private const float UNHAPPY_MUCUS_MODIFIER = -0.5f;
    private const float WILD_MUCUS_MODIFIER = -0.75f;
    [MyCmpReq]
    public Effects effects;
    public AmountInstance mucusAmount;
    public AttributeModifier onDryLandMucusModifier;
    public AttributeModifier wildMucusModifier;
    public AttributeModifier unhappyMucusModifier;
    public Klei.AI.Attributes attributes;
    public WildnessMonitor.Instance wildnessMonitor;
    public AmountInstance moisture;
    public AttributeModifier baseMoistureModifier;
    public AttributeModifier wetMoistureModifier;

    public Instance(IStateMachineTarget master, MoistureMonitor.Def def)
      : base(master, def)
    {
      Database.Amounts amounts = Db.Get().Amounts;
      this.moisture = amounts.Moisture.Lookup(this.gameObject);
      this.moisture.value = this.moisture.GetMax();
      this.baseMoistureModifier = new AttributeModifier(this.moisture.amount.deltaAttribute.Id, def.dryRate, (string) CREATURES.MODIFIERS.MOISTURE_LOSS_RATE.NAME);
      this.wetMoistureModifier = new AttributeModifier(this.moisture.amount.deltaAttribute.Id, def.soakRate, (string) CREATURES.MODIFIERS.MOISTURE_GAIN_RATE.NAME);
      this.attributes = master.gameObject.GetAttributes();
      this.mucusAmount = amounts.Mucus.Lookup(this.gameObject);
      this.mucusAmount.value = this.mucusAmount.GetMax();
      this.onDryLandMucusModifier = new AttributeModifier(this.mucusAmount.amount.deltaAttribute.Id, def.onDryLandModifier, (string) CREATURES.MODIFIERS.MUCUS.ON_DRY_LAND);
      this.unhappyMucusModifier = new AttributeModifier(this.mucusAmount.amount.deltaAttribute.Id, -0.5f, (string) CREATURES.MODIFIERS.MUCUS.UNHAPPY, true);
      this.wildMucusModifier = new AttributeModifier(this.mucusAmount.amount.deltaAttribute.Id, -0.75f, (string) CREATURES.MODIFIERS.MUCUS.WILD, true);
    }

    public void ProduceLubricant()
    {
      float mass = this.mucusAmount.value;
      if ((double) mass <= 0.0)
        return;
      BubbleManager.instance.SpawnBubble(this.def.lubricant, (Vector2) this.transform.GetPosition(), mass, this.def.lubricantTemperatureKelvin, BubbleManager.Disease.None);
      this.Trigger(1151073968);
      this.effects.Add(MoistureMonitor.RECENTLY_PRODUCED_LUBRICANT_EFFECT, true);
      this.mucusAmount.value = 0.0f;
    }
  }
}
