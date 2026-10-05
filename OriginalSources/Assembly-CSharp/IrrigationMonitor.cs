// Decompiled with JetBrains decompiler
// Type: IrrigationMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class IrrigationMonitor : 
  GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>
{
  public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.TargetParameter resourceStorage;
  public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter hasCorrectLiquid;
  public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter hasIncorrectLiquid;
  public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter enoughCorrectLiquidToRecover;
  public GameHashes ResourceRecievedEvent = GameHashes.LiquidResourceRecieved;
  public GameHashes ResourceDepletedEvent = GameHashes.LiquidResourceEmpty;
  public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State wild;
  public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State unfertilizable;
  public IrrigationMonitor.ReplantedStates replanted;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.wild;
    this.serializable = StateMachine.SerializeType.Never;
    this.wild.ParamTransition<GameObject>((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<GameObject>) this.resourceStorage, this.unfertilizable, (StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<GameObject>.Callback) ((smi, p) => (UnityEngine.Object) p != (UnityEngine.Object) null));
    this.unfertilizable.Enter((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State.Callback) (smi =>
    {
      if (!smi.AcceptsLiquid())
        return;
      smi.GoTo((StateMachine.BaseState) this.replanted.irrigated);
    }));
    this.replanted.Enter((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State.Callback) (smi =>
    {
      foreach (ManualDeliveryKG component in smi.gameObject.GetComponents<ManualDeliveryKG>())
        component.Pause(false, "replanted");
      smi.UpdateIrrigation(0.2f);
    })).Target(this.resourceStorage).EventHandler(GameHashes.OnStorageChange, (StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State.Callback) (smi => smi.UpdateIrrigation(0.2f))).Target(this.masterTarget);
    this.replanted.irrigated.DefaultState((GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State) this.replanted.irrigated.absorbing).TriggerOnEnter(this.ResourceRecievedEvent);
    this.replanted.irrigated.absorbing.DefaultState(this.replanted.irrigated.absorbing.normal).ParamTransition<bool>((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>) this.hasCorrectLiquid, (GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State) this.replanted.starved, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (Func<IrrigationMonitor.Instance, AttributeModifier>) (smi => smi.absorptionRate)).Enter((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State.Callback) (smi => smi.UpdateAbsorbing(true))).EventHandler(GameHashes.TagsChanged, (StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State.Callback) (smi => smi.UpdateAbsorbing(true))).Exit((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State.Callback) (smi => smi.UpdateAbsorbing(false)));
    this.replanted.irrigated.absorbing.normal.ParamTransition<bool>((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>) this.hasIncorrectLiquid, this.replanted.irrigated.absorbing.wrongLiquid, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsTrue);
    this.replanted.irrigated.absorbing.wrongLiquid.ParamTransition<bool>((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>) this.hasIncorrectLiquid, this.replanted.irrigated.absorbing.normal, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse);
    this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent).ParamTransition<bool>((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>) this.enoughCorrectLiquidToRecover, (GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State) this.replanted.irrigated.absorbing, (StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>.Callback) ((smi, p) => p && this.hasCorrectLiquid.Get(smi))).ParamTransition<bool>((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>) this.hasCorrectLiquid, (GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State) this.replanted.irrigated.absorbing, (StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>.Callback) ((smi, p) => p && this.enoughCorrectLiquidToRecover.Get(smi)));
    this.replanted.starved.normal.ParamTransition<bool>((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>) this.hasIncorrectLiquid, this.replanted.starved.wrongLiquid, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsTrue);
    this.replanted.starved.wrongLiquid.ParamTransition<bool>((StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.Parameter<bool>) this.hasIncorrectLiquid, this.replanted.starved.normal, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public Tag wrongIrrigationTestTag;
    public PlantElementAbsorber.ConsumeInfo[] consumedElements;

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      if (this.consumedElements.Length == 0)
        return (List<Descriptor>) null;
      List<Descriptor> descriptors = new List<Descriptor>();
      float modifiedAttributeValue = obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.FertilizerUsageMod);
      if (this.consumedElements.Length > 1)
      {
        string[] strArray = new string[this.consumedElements.Length];
        for (int index = 0; index < this.consumedElements.Length; ++index)
          strArray[index] = this.consumedElements[index].tag.ProperName();
        string str = string.Join((string) UI.GAMEOBJECTEFFECTS.REQUIREMETS_OR, strArray);
        float mass = this.consumedElements[0].massConsumptionRate * modifiedAttributeValue;
        descriptors.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, (object) str, (object) GameUtil.GetFormattedMass(-mass, GameUtil.TimeSlice.PerCycle)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.IDEAL_FERTILIZER, (object) str, (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Requirement));
      }
      else
      {
        foreach (PlantElementAbsorber.ConsumeInfo consumedElement in this.consumedElements)
        {
          float mass = consumedElement.massConsumptionRate * modifiedAttributeValue;
          descriptors.Add(new Descriptor(string.Format((string) UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, (object) consumedElement.tag.ProperName(), (object) GameUtil.GetFormattedMass(-mass, GameUtil.TimeSlice.PerCycle)), string.Format((string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.IDEAL_FERTILIZER, (object) consumedElement.tag.ProperName(), (object) GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Requirement));
        }
      }
      return descriptors;
    }
  }

  public class VariableIrrigationStates : 
    GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
  {
    public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State normal;
    public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State wrongLiquid;
  }

  public class Irrigated : 
    GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
  {
    public IrrigationMonitor.VariableIrrigationStates absorbing;
  }

  public class ReplantedStates : 
    GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
  {
    public IrrigationMonitor.Irrigated irrigated;
    public IrrigationMonitor.VariableIrrigationStates starved;
  }

  public new class Instance : 
    GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.GameInstance,
    IWiltCause
  {
    public AttributeModifier consumptionRate;
    public AttributeModifier absorptionRate;
    protected AmountInstance irrigation;
    private float total_available_mass;
    private Storage storage;
    private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;

    public float total_fertilizer_available => this.total_available_mass;

    public Instance(IStateMachineTarget master, IrrigationMonitor.Def def)
      : base(master, def)
    {
      this.AddAmounts(this.gameObject);
      this.MakeModifiers();
      master.Subscribe(1309017699, new System.Action<object>(this.SetStorage));
    }

    public virtual StatusItem GetStarvedStatusItem()
    {
      return Db.Get().CreatureStatusItems.NeedsIrrigation;
    }

    public virtual StatusItem GetIncorrectLiquidStatusItem()
    {
      return Db.Get().CreatureStatusItems.WrongIrrigation;
    }

    public virtual StatusItem GetIncorrectLiquidStatusItemMajor()
    {
      return Db.Get().CreatureStatusItems.WrongIrrigationMajor;
    }

    protected virtual void AddAmounts(GameObject gameObject)
    {
      this.irrigation = gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Irrigation, gameObject));
    }

    protected virtual void MakeModifiers()
    {
      this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Irrigation.deltaAttribute.Id, -0.166666672f, (string) CREATURES.STATS.IRRIGATION.CONSUME_MODIFIER);
      this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Irrigation.deltaAttribute.Id, 1.66666663f, (string) CREATURES.STATS.IRRIGATION.ABSORBING_MODIFIER);
    }

    public static void DumpIncorrectFertilizers(Storage storage, GameObject go)
    {
      if ((UnityEngine.Object) storage == (UnityEngine.Object) null || (UnityEngine.Object) go == (UnityEngine.Object) null)
        return;
      IrrigationMonitor.Instance smi1 = go.GetSMI<IrrigationMonitor.Instance>();
      PlantElementAbsorber.ConsumeInfo[] consumed_infos1 = (PlantElementAbsorber.ConsumeInfo[]) null;
      if (smi1 != null)
        consumed_infos1 = smi1.def.consumedElements;
      IrrigationMonitor.Instance.DumpIncorrectFertilizers(storage, consumed_infos1, false);
      FertilizationMonitor.Instance smi2 = go.GetSMI<FertilizationMonitor.Instance>();
      PlantElementAbsorber.ConsumeInfo[] consumed_infos2 = (PlantElementAbsorber.ConsumeInfo[]) null;
      if (smi2 != null)
        consumed_infos2 = smi2.def.consumedElements;
      IrrigationMonitor.Instance.DumpIncorrectFertilizers(storage, consumed_infos2, true);
    }

    private static void DumpIncorrectFertilizers(
      Storage storage,
      PlantElementAbsorber.ConsumeInfo[] consumed_infos,
      bool validate_solids)
    {
      if ((UnityEngine.Object) storage == (UnityEngine.Object) null || consumed_infos == null)
        return;
      for (int index = storage.items.Count - 1; index >= 0; --index)
      {
        GameObject go = storage.items[index];
        PrimaryElement component1;
        if (!((UnityEngine.Object) go == (UnityEngine.Object) null) && go.TryGetComponent<PrimaryElement>(out component1) && go.TryGetComponent<ElementChunk>(out ElementChunk _))
        {
          if (validate_solids)
          {
            if (!component1.Element.IsSolid)
              continue;
          }
          else if (!component1.Element.IsLiquid)
            continue;
          bool flag = false;
          KPrefabID component2 = component1.GetComponent<KPrefabID>();
          foreach (PlantElementAbsorber.ConsumeInfo consumedInfo in consumed_infos)
          {
            if (component2.HasTag(consumedInfo.tag))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
            storage.Drop(go, true);
        }
      }
    }

    public void SetStorage(object obj)
    {
      this.storage = (Storage) obj;
      this.sm.resourceStorage.Set((KMonoBehaviour) this.storage, this.smi);
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
          component.enabled = !this.storage.gameObject.GetComponent<PlantablePlot>().has_liquid_pipe_input;
        }
      }
    }

    public WiltCondition.Condition[] Conditions
    {
      get
      {
        return new WiltCondition.Condition[1]
        {
          WiltCondition.Condition.Irrigation
        };
      }
    }

    public string WiltStateString
    {
      get
      {
        string wiltStateString = "";
        if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.replanted.irrigated.absorbing.wrongLiquid))
          wiltStateString = this.GetIncorrectLiquidStatusItem().resolveStringCallback((string) CREATURES.STATUSITEMS.WRONGIRRIGATION.NAME, (object) this);
        else if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.replanted.starved.wrongLiquid))
          wiltStateString = this.GetIncorrectLiquidStatusItemMajor().resolveStringCallback((string) CREATURES.STATUSITEMS.WRONGIRRIGATIONMAJOR.NAME, (object) this);
        else if (this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.replanted.starved))
          wiltStateString = this.GetStarvedStatusItem().resolveStringCallback((string) CREATURES.STATUSITEMS.NEEDSIRRIGATION.NAME, (object) this);
        return wiltStateString;
      }
    }

    public virtual bool AcceptsLiquid()
    {
      PlantablePlot component = this.sm.resourceStorage.Get(this).GetComponent<PlantablePlot>();
      return (UnityEngine.Object) component != (UnityEngine.Object) null && component.AcceptsIrrigation;
    }

    public bool Starved() => (double) this.irrigation.value == 0.0;

    public void UpdateIrrigation(float dt)
    {
      if (this.def.consumedElements == null)
        return;
      Storage storage = this.sm.resourceStorage.Get<Storage>(this.smi);
      bool flag1 = false;
      bool flag2;
      bool flag3;
      if ((UnityEngine.Object) storage != (UnityEngine.Object) null)
      {
        List<GameObject> items = storage.items;
        flag2 = false;
        flag3 = false;
        float totalValue = this.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
        for (int index1 = 0; index1 < this.def.consumedElements.Length; ++index1)
        {
          PlantElementAbsorber.ConsumeInfo consumedElement = this.def.consumedElements[index1];
          float num = 0.0f;
          for (int index2 = 0; index2 < items.Count; ++index2)
          {
            if (items[index2].HasTag(consumedElement.tag))
              num += items[index2].GetComponent<PrimaryElement>().Mass;
          }
          if ((double) num > (double) this.total_available_mass)
            this.total_available_mass = num;
          if ((double) num >= (double) consumedElement.massConsumptionRate * (double) totalValue * (double) dt)
          {
            flag2 = true;
            flag3 = (double) num >= (double) consumedElement.massConsumptionRate * (double) totalValue * ((double) dt * 30.0);
            break;
          }
        }
        for (int index3 = 0; index3 < items.Count; ++index3)
        {
          GameObject go = items[index3];
          if (go.HasTag(this.def.wrongIrrigationTestTag))
          {
            bool flag4 = false;
            for (int index4 = 0; index4 < this.def.consumedElements.Length; ++index4)
            {
              if (go.HasTag(this.def.consumedElements[index4].tag))
              {
                flag4 = true;
                break;
              }
            }
            if (!flag4)
              flag1 = true;
          }
        }
      }
      else
      {
        flag2 = false;
        flag3 = false;
        flag1 = false;
      }
      this.sm.hasCorrectLiquid.Set(flag2, this.smi);
      this.sm.hasIncorrectLiquid.Set(flag1, this.smi);
      this.sm.enoughCorrectLiquidToRecover.Set(flag3 & flag2, this.smi);
    }

    public void UpdateAbsorbing(bool allow)
    {
      bool flag = allow && !this.smi.gameObject.HasTag(GameTags.Wilting);
      if (flag == this.absorberHandle.IsValid())
        return;
      if (flag)
      {
        if (this.def.consumedElements == null || this.def.consumedElements.Length == 0)
          return;
        float totalValue = this.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
        PlantElementAbsorber.ConsumeInfo[] consumed_elements = new PlantElementAbsorber.ConsumeInfo[this.def.consumedElements.Length];
        for (int index = 0; index < this.def.consumedElements.Length; ++index)
        {
          PlantElementAbsorber.ConsumeInfo consumedElement = this.def.consumedElements[index];
          consumedElement.massConsumptionRate *= totalValue;
          consumed_elements[index] = consumedElement;
        }
        this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, consumed_elements);
      }
      else
        this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
    }
  }
}
