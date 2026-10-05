// Decompiled with JetBrains decompiler
// Type: FertilizationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FertilizationMonitor : 
  GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>
{
  public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.TargetParameter fertilizerStorage;
  public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter isFertilized;
  public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wild;
  public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State unfertilizable;
  public FertilizationMonitor.ReplantedStates replanted;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.wild;
    this.serializable = StateMachine.SerializeType.Never;
    this.wild.ParamTransition<GameObject>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<GameObject>) this.fertilizerStorage, this.unfertilizable, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p != (UnityEngine.Object) null));
    this.unfertilizable.EnterTransition((GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State) this.replanted, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Transition.ConditionCallback) (smi => smi.AcceptsFertilizer()));
    this.replanted.Enter((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi =>
    {
      foreach (ManualDeliveryKG component in smi.gameObject.GetComponents<ManualDeliveryKG>())
        component.Pause(false, "replanted");
      smi.UpdateFertilization(0.2f);
    })).ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.isFertilized, (GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State) this.replanted.fertilized, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>.Callback) ((_, status) => status)).ParamTransition<bool>((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>) this.isFertilized, this.replanted.starved, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Parameter<bool>.Callback) ((_, status) => !status)).Target(this.fertilizerStorage).EventHandler(GameHashes.OnStorageChange, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi => smi.UpdateFertilization(0.2f))).Target(this.masterTarget);
    this.replanted.fertilized.DefaultState(this.replanted.fertilized.absorbing).TriggerOnEnter(GameHashes.Fertilized).EnterTransition(this.replanted.fertilized.wilting, (StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.Transition.ConditionCallback) (smi => smi.wiltCondition.IsWilting()));
    this.replanted.fertilized.absorbing.ToggleAttributeModifier("Absorbing", (Func<FertilizationMonitor.Instance, AttributeModifier>) (smi => smi.absorptionRate)).EventTransition(GameHashes.Wilt, this.replanted.fertilized.wilting).Enter((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi => smi.StartAbsorbing())).Exit((StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State.Callback) (smi => smi.StopAbsorbing()));
    this.replanted.fertilized.wilting.EventTransition(GameHashes.WiltRecover, this.replanted.fertilized.absorbing);
    this.replanted.starved.TriggerOnEnter(GameHashes.Unfertilized);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public PlantElementAbsorber.ConsumeInfo[] consumedElements;

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      if (this.consumedElements.Length == 0)
        return (List<Descriptor>) null;
      List<Descriptor> descriptors = new List<Descriptor>();
      float modifiedAttributeValue = obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.FertilizerUsageMod);
      foreach (PlantElementAbsorber.ConsumeInfo consumedElement in this.consumedElements)
      {
        float mass = consumedElement.massConsumptionRate * modifiedAttributeValue;
        descriptors.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, (object) consumedElement.tag.ProperName(), (object) GameUtil.GetFormattedMass(-mass, GameUtil.TimeSlice.PerCycle)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.IDEAL_FERTILIZER, (object) consumedElement.tag.ProperName(), (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Requirement));
      }
      return descriptors;
    }

    public PlantElementAbsorber.ConsumeInfo[] ScaleConsumedElements(float scale)
    {
      PlantElementAbsorber.ConsumeInfo[] consumeInfoArray = new PlantElementAbsorber.ConsumeInfo[this.consumedElements.Length];
      for (int index = 0; index < this.consumedElements.Length; ++index)
      {
        PlantElementAbsorber.ConsumeInfo consumedElement = this.consumedElements[index];
        consumedElement.massConsumptionRate *= scale;
        consumeInfoArray[index] = consumedElement;
      }
      return consumeInfoArray;
    }
  }

  public enum FertilizerStatus
  {
    Starved,
    Correct,
  }

  public class FertilizedStates : 
    GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
  {
    public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State absorbing;
    public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wilting;
  }

  public class ReplantedStates : 
    GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
  {
    public FertilizationMonitor.FertilizedStates fertilized;
    public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State starved;
  }

  public new class Instance : 
    GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GameInstance,
    IWiltCause
  {
    public AttributeModifier absorptionRate;
    protected AmountInstance fertilization;
    private Storage storage;
    private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;
    [MyCmpReq]
    public WiltCondition wiltCondition;
    private readonly PlantElementAbsorber.ConsumeInfo[] consumedElements;

    public float total_fertilizer_available
    {
      get => PlantElementAbsorber.FindLargest(this.storage, this.consumedElements);
    }

    public Instance(IStateMachineTarget master, FertilizationMonitor.Def def)
      : base(master, def)
    {
      this.AddAmounts(this.gameObject);
      this.MakeModifiers();
      master.Subscribe(1309017699, new System.Action<object>(this.SetStorage));
      float totalValue = this.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
      this.consumedElements = def.ScaleConsumedElements(totalValue);
    }

    public virtual StatusItem GetStarvedStatusItem()
    {
      return Db.Get().CreatureStatusItems.NeedsFertilizer;
    }

    protected virtual void AddAmounts(GameObject gameObject)
    {
      this.fertilization = gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Fertilization, gameObject));
    }

    public WiltCondition.Condition[] Conditions
    {
      get
      {
        return new WiltCondition.Condition[1]
        {
          WiltCondition.Condition.Fertilized
        };
      }
    }

    public string WiltStateString
    {
      get
      {
        return !this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.replanted.starved) ? "" : this.GetStarvedStatusItem().resolveStringCallback((string) CREATURES.STATUSITEMS.NEEDSFERTILIZER.NAME, (object) this);
      }
    }

    protected virtual void MakeModifiers()
    {
      this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, 1.66666663f, (string) CREATURES.STATS.FERTILIZATION.ABSORBING_MODIFIER);
    }

    public void SetStorage(object obj)
    {
      this.storage = (Storage) obj;
      this.sm.fertilizerStorage.Set((KMonoBehaviour) this.storage, this.smi);
      IrrigationMonitor.Instance.DumpIncorrectFertilizers(this.storage, this.smi.gameObject);
      foreach (ManualDeliveryKG component in this.smi.gameObject.GetComponents<ManualDeliveryKG>())
      {
        bool flag = false;
        foreach (PlantElementAbsorber.ConsumeInfo consumedElement in this.def.consumedElements)
        {
          if (component.RequestedItemTag == consumedElement.tag)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          component.SetStorage(this.storage);
          component.enabled = true;
        }
      }
    }

    public virtual bool AcceptsFertilizer()
    {
      PlantablePlot component;
      return this.sm.fertilizerStorage.Get(this).TryGetComponent<PlantablePlot>(out component) && component.AcceptsFertilizer;
    }

    public void UpdateFertilization(float dt)
    {
      if (this.def.consumedElements == null || (UnityEngine.Object) this.storage == (UnityEngine.Object) null || (double) dt == 0.0)
        return;
      this.sm.isFertilized.Set(PlantElementAbsorber.PlanConsume(this.storage, this.consumedElements, dt, (List<PlantElementAbsorber.Planner.ConsumeCommand>) null), this.smi);
    }

    public void StartAbsorbing()
    {
      if (this.absorberHandle.IsValid() || this.def.consumedElements == null || this.def.consumedElements.Length == 0)
        return;
      this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, this.consumedElements);
    }

    public void StopAbsorbing()
    {
      if (!this.absorberHandle.IsValid())
        return;
      this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
    }
  }
}
