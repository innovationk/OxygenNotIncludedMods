// Decompiled with JetBrains decompiler
// Type: FlytrapConsumptionMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FlytrapConsumptionMonitor : 
  StateMachineComponent<FlytrapConsumptionMonitor.Instance>,
  IGameObjectEffectDescriptor,
  IPlantConsumeEntities
{
  public const string AWAIT_PREY_ANIM_NAME = "awaiting_prey";
  public const string EAT_ANIM_NAME = "consume";
  private const string CONSUMED_ENTITY_NAME_FALLBACK = "Unknown Critter";
  private static Tag CONSUMABLE_TAG = GameTags.Creatures.Flyer;
  public static readonly StandardCropPlant.AnimSet HUNGRY_STATE_ANIM_SET = new StandardCropPlant.AnimSet(FlyTrapPlantConfig.Default_StandardCropAnimSet)
  {
    grow = "awaiting_prey",
    wilt_base = "flower_wilt",
    grow_playmode = KAnim.PlayMode.Loop
  };
  public static readonly StandardCropPlant.AnimSet EATING_STATE_ANIM_SET = new StandardCropPlant.AnimSet(FlyTrapPlantConfig.Default_StandardCropAnimSet)
  {
    pre_grow = "consume",
    grow_playmode = KAnim.PlayMode.Paused
  };

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  public string GetConsumableEntitiesCategoryName()
  {
    return (string) CREATURES.SPECIES.FLYTRAPPLANT.VICTIM_IDENTIFIER;
  }

  public bool AreEntitiesConsumptionRequirementsSatisfied()
  {
    return this.smi != null && this.smi.HasEaten;
  }

  public string GetRequirementText()
  {
    return (string) CREATURES.SPECIES.FLYTRAPPLANT.PLANT_HUNGER_REQUIREMENT;
  }

  public string GetConsumedEntityName()
  {
    return this.smi != null ? this.smi.LastConsumedEntityName : "Unknown Critter";
  }

  public List<KPrefabID> GetPrefabsOfPossiblePrey()
  {
    List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(FlytrapConsumptionMonitor.CONSUMABLE_TAG);
    List<KPrefabID> prefabsOfPossiblePrey = new List<KPrefabID>();
    for (int index = 0; index < prefabsWithTag.Count; ++index)
    {
      KPrefabID component = prefabsWithTag[index].GetComponent<KPrefabID>();
      if (this.IsEntityEdible(component) && !prefabsOfPossiblePrey.Contains(component) && Game.IsCorrectDlcActiveForCurrentSave((IHasDlcRestrictions) component))
        prefabsOfPossiblePrey.Add(component);
    }
    return prefabsOfPossiblePrey;
  }

  public string[] GetFormattedPossiblePreyList()
  {
    List<string> stringList = new List<string>();
    foreach (Component component1 in this.GetPrefabsOfPossiblePrey())
    {
      CreatureBrain component2 = component1.GetComponent<CreatureBrain>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      {
        string str = component2.species.ProperName();
        if (!stringList.Contains(str))
          stringList.Add(str);
      }
    }
    return stringList.ToArray();
  }

  public bool IsEntityEdible(GameObject entity)
  {
    return !((UnityEngine.Object) entity == (UnityEngine.Object) null) && this.IsEntityEdible(entity.GetComponent<KPrefabID>());
  }

  public bool IsEntityEdible(KPrefabID entity)
  {
    return !((UnityEngine.Object) entity == (UnityEngine.Object) null) && entity.HasTag(FlytrapConsumptionMonitor.CONSUMABLE_TAG) && (UnityEngine.Object) entity.GetComponent<CreatureBrain>() != (UnityEngine.Object) null && entity.GetComponent<OccupyArea>().OccupiedCellsOffsets.Length <= 1;
  }

  public List<Descriptor> GetDescriptors(GameObject obj)
  {
    return new List<Descriptor>()
    {
      new Descriptor(this.GetRequirementText(), "", Descriptor.DescriptorType.Requirement)
    };
  }

  public static bool IsWilted(FlytrapConsumptionMonitor.Instance smi) => smi.IsWilted;

  public static void CompleteEat(FlytrapConsumptionMonitor.Instance smi)
  {
    smi.sm.HasEaten.Set(true, smi);
  }

  public static void RetriggerGrowAnimationIfInGrowState(FlytrapConsumptionMonitor.Instance smi)
  {
    StandardCropPlant component1 = smi.GetComponent<StandardCropPlant>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null || component1.smi == null || !component1.smi.IsInsideState((StateMachine.BaseState) component1.smi.sm.alive.idle))
      return;
    KBatchedAnimController component2 = smi.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    component2.Play((HashedString) component1.anims.grow, component1.anims.grow_playmode);
  }

  public static void BecomeHungry(FlytrapConsumptionMonitor.Instance smi)
  {
    smi.sm.HasEaten.Set(false, smi);
  }

  public static void RegisterVictimProximityMonitor(FlytrapConsumptionMonitor.Instance smi)
  {
    smi.RegisterVictimProximityMonitor();
  }

  public static void UnregisterVictimProximityMonitor(FlytrapConsumptionMonitor.Instance smi)
  {
    smi.UnregisterVictimProximityMonitor();
  }

  public static void SetAndPlayConsumeCropPlantAnimations(FlytrapConsumptionMonitor.Instance smi)
  {
    StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.smi == null)
      return;
    component.anims = FlytrapConsumptionMonitor.EATING_STATE_ANIM_SET;
    component.smi.GoTo((StateMachine.BaseState) component.smi.sm.alive.pre_idle);
  }

  public static void SetCropPlantAnimationsToAwaitPrey(FlytrapConsumptionMonitor.Instance smi)
  {
    FlytrapConsumptionMonitor.SetCropPlantAnimationSet(smi, FlytrapConsumptionMonitor.HUNGRY_STATE_ANIM_SET);
    FlytrapConsumptionMonitor.RetriggerGrowAnimationIfInGrowState(smi);
    StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.smi == null)
      return;
    component.preventGrowPositionUpdate = true;
  }

  public static void RestoreDefaultCropPlantAnimations(FlytrapConsumptionMonitor.Instance smi)
  {
    FlytrapConsumptionMonitor.SetCropPlantAnimationSet(smi, FlyTrapPlantConfig.Default_StandardCropAnimSet);
    StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.smi == null)
      return;
    component.preventGrowPositionUpdate = false;
  }

  private static void SetCropPlantAnimationSet(
    FlytrapConsumptionMonitor.Instance smi,
    StandardCropPlant.AnimSet set)
  {
    StandardCropPlant component = smi.GetComponent<StandardCropPlant>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.smi == null)
      return;
    component.anims = set;
  }

  public class States : 
    GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor>
  {
    public FlytrapConsumptionMonitor.States.HungryStates hungry;
    public GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State satisfied;
    public StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.BoolParameter HasEaten;
    public StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Signal EatSignal;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = StateMachine.SerializeType.ParamsOnly;
      default_state = (StateMachine.BaseState) this.hungry;
      this.hungry.ParamTransition<bool>((StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Parameter<bool>) this.HasEaten, this.satisfied, GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.IsTrue).Toggle("Toggle Standard Crop Plant Animations", new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.SetCropPlantAnimationsToAwaitPrey), new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.RestoreDefaultCropPlantAnimations)).ToggleAttributeModifier("Pause Growing", (Func<FlytrapConsumptionMonitor.Instance, AttributeModifier>) (smi => smi.pauseGrowing)).DefaultState(this.hungry.idle);
      this.hungry.idle.EventTransition(GameHashes.Wilt, this.hungry.wilt, new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Transition.ConditionCallback(FlytrapConsumptionMonitor.IsWilted)).ToggleStatusItem(Db.Get().CreatureStatusItems.CarnivorousPlantAwaitingVictim, (Func<FlytrapConsumptionMonitor.Instance, object>) (smi => (object) smi.master.GetComponent<IPlantConsumeEntities>())).Enter(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.RegisterVictimProximityMonitor)).TriggerOnEnter(GameHashes.CropSleep).OnSignal(this.EatSignal, this.hungry.complete).Exit(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.UnregisterVictimProximityMonitor));
      this.hungry.complete.Enter(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.SetAndPlayConsumeCropPlantAnimations)).Enter(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.CompleteEat));
      this.hungry.wilt.EventTransition(GameHashes.WiltRecover, this.hungry.idle, GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Not(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Transition.ConditionCallback(FlytrapConsumptionMonitor.IsWilted)));
      this.satisfied.Enter(new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.RetriggerGrowAnimationIfInGrowState)).TriggerOnEnter(GameHashes.CropWakeUp).ParamTransition<bool>((StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.Parameter<bool>) this.HasEaten, (GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State) this.hungry, GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.IsFalse).EventHandler(GameHashes.Harvest, new StateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State.Callback(FlytrapConsumptionMonitor.BecomeHungry));
    }

    public class HungryStates : 
      GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State
    {
      public GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State wilt;
      public GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State idle;
      public GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.State complete;
    }
  }

  public class Instance : 
    GameStateMachine<FlytrapConsumptionMonitor.States, FlytrapConsumptionMonitor.Instance, FlytrapConsumptionMonitor, object>.GameInstance
  {
    public AttributeModifier pauseGrowing;
    [Serialize]
    private string lastConsumedEntityPrefabID;
    private Growing growing;
    private WiltCondition wiltCondition;
    private AmountInstance maturity;
    private HandleVector<int>.Handle partitionerEntry = HandleVector<int>.InvalidHandle;

    public bool HasEaten => this.sm.HasEaten.Get(this);

    public bool IsWilted => this.wiltCondition.IsWilting();

    public string LastConsumedEntityName
    {
      get
      {
        return !string.IsNullOrEmpty(this.lastConsumedEntityPrefabID) ? Assets.GetPrefab((Tag) this.lastConsumedEntityPrefabID).GetProperName() : "Unknown Critter";
      }
    }

    public Instance(FlytrapConsumptionMonitor master)
      : base(master)
    {
      this.maturity = this.gameObject.GetAmounts().Get(Db.Get().Amounts.Maturity);
      this.pauseGrowing = new AttributeModifier(this.maturity.deltaAttribute.Id, -1f, (string) CREATURES.SPECIES.FLYTRAPPLANT.HUNGRY, true);
      this.wiltCondition = this.GetComponent<WiltCondition>();
      this.growing = this.GetComponent<Growing>();
      this.growing.CustomGrowStallCondition_IsStalled = new Func<GameObject, bool>(this.ShouldStallGrowingComponent);
    }

    private bool ShouldStallGrowingComponent(GameObject plantGameObject) => !this.HasEaten;

    public void RegisterVictimProximityMonitor()
    {
      this.partitionerEntry = GameScenePartitioner.Instance.Add("FlytrapConsumptionMonitor.hungry.idle", (object) this.gameObject, this.GetComponent<OccupyArea>().GetExtents(), GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupableLayerObjectDetected));
    }

    public void UnregisterVictimProximityMonitor()
    {
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
      this.partitionerEntry = HandleVector<int>.InvalidHandle;
    }

    public void OnPickupableLayerObjectDetected(object obj)
    {
      Pickupable cmp = obj as Pickupable;
      if (!this.master.IsEntityEdible(cmp.gameObject))
        return;
      this.lastConsumedEntityPrefabID = cmp.PrefabID().ToString();
      cmp.gameObject.DeleteObject();
      this.sm.EatSignal.Trigger(this);
    }
  }
}
