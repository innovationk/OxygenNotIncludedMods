// Decompiled with JetBrains decompiler
// Type: FertilityShearable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FertilityShearable : 
  GameStateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>
{
  public GameStateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>.State hasMilk;
  public GameStateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>.State noMilk;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.noMilk;
    this.noMilk.Transition(this.hasMilk, (StateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>.Transition.ConditionCallback) (smi => FertilityShearable.IsFertileEnoughToHarvest(smi))).Enter((StateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>.State.Callback) (smi => FertilityShearable.ShowBellySymbol(smi, false)));
    this.hasMilk.Transition(this.noMilk, (StateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>.Transition.ConditionCallback) (smi => !FertilityShearable.IsFertileEnoughToHarvest(smi))).Enter((StateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>.State.Callback) (smi => FertilityShearable.ShowBellySymbol(smi, true))).Update((System.Action<FertilityShearable.Instance, float>) ((smi, dt) => smi.milkReadyStatusGuid = smi.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.FishFullMilk, smi.milkReadyStatusGuid, smi.IsReadyToBeMilked(), (object) smi)), UpdateRate.SIM_1000ms).Exit((StateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>.State.Callback) (smi => smi.milkReadyStatusGuid = smi.selectable.RemoveStatusItem(smi.milkReadyStatusGuid)));
  }

  private static bool IsFertileEnoughToHarvest(FertilityShearable.Instance smi)
  {
    return (double) smi.fertility.value >= (double) smi.def.minimumFertility;
  }

  private static void ShowBellySymbol(FertilityShearable.Instance smi, bool show)
  {
    if (show)
      FertilityShearable.AddBellyOverride(smi);
    else
      FertilityShearable.RemoveBellyOverride(smi);
  }

  private static void AddBellyOverride(FertilityShearable.Instance smi)
  {
    SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
    KAnim.Build.Symbol symbol = smi.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) "belly_full");
    if (symbol == null)
      return;
    component.AddSymbolOverride((HashedString) "belly", symbol, 1);
  }

  private static void RemoveBellyOverride(FertilityShearable.Instance smi)
  {
    smi.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride((HashedString) "belly", 1);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    private const float DEFAULT_MINIMUM_FERTILITY = 50f;
    private const float DEFAULT_PERCENT_FERTILITY_CONSUMED_PER_MILKING = 1f;
    public SimHashes milkElement;
    public float dropMass;
    public float minimumFertility = 50f;
    public float percentFertilityConsumedPerMilking = 1f;
    public bool requiresHappy;
    public bool suppressedByElderly;

    public override void Configure(GameObject prefab)
    {
    }

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      string newValue = ElementLoader.FindElementByHash(this.milkElement).tag.ProperName();
      string formattedMass = GameUtil.GetFormattedMass(this.dropMass);
      return new List<Descriptor>()
      {
        new Descriptor(GlobalStringBuilderPool.ReturnAndFree(GlobalStringBuilderPool.Alloc().Append((string) UI.BUILDINGEFFECTS.SCALE_GROWTH_FERTILITY).Replace("{Item}", newValue).Replace("{Amount}", formattedMass).Replace("{Percent}", GameUtil.GetFormattedPercent(this.minimumFertility))), GlobalStringBuilderPool.ReturnAndFree(GlobalStringBuilderPool.Alloc().Append((string) UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_FERTILE).Replace("{Item}", newValue).Replace("{Amount}", formattedMass)))
      };
    }
  }

  public new class Instance : 
    GameStateMachine<FertilityShearable, FertilityShearable.Instance, IStateMachineTarget, FertilityShearable.Def>.GameInstance,
    IMilkable
  {
    [MyCmpGet]
    private Effects effects;
    [MyCmpGet]
    public KBatchedAnimController animController;
    [MyCmpReq]
    private KPrefabID prefabId;
    [MyCmpReq]
    public KSelectable selectable;
    public AmountInstance fertility;
    public Guid milkReadyStatusGuid;
    private WildnessMonitor.Instance wildnessMonitor;
    private AgeMonitor.Instance ageMonitor;

    public Instance(IStateMachineTarget master, FertilityShearable.Def def)
      : base(master, def)
    {
      this.fertility = Db.Get().Amounts.Fertility.Lookup(this.gameObject);
    }

    public override void StartSM()
    {
      base.StartSM();
      this.wildnessMonitor = this.gameObject.GetSMI<WildnessMonitor.Instance>();
      this.ageMonitor = this.gameObject.GetSMI<AgeMonitor.Instance>();
    }

    public bool IsReadyToBeMilked()
    {
      return (double) this.fertility.value >= (double) this.def.minimumFertility && (this.wildnessMonitor == null || !this.wildnessMonitor.IsWild()) && (!this.def.requiresHappy || this.prefabId.HasTag(GameTags.Creatures.Happy)) && (!this.def.suppressedByElderly || this.ageMonitor == null || !this.ageMonitor.IsElderly);
    }

    public SimHashes GetMilkElement() => this.def.milkElement;

    public void MilkingComplete(Storage storage)
    {
      storage.GetComponent<Storage>().AddLiquid(this.def.milkElement, this.def.dropMass, 310.15f, byte.MaxValue, 0);
      this.fertility.value = Mathf.Max(0.0f, this.fertility.value - this.fertility.GetMax() * this.def.percentFertilityConsumedPerMilking);
    }
  }
}
