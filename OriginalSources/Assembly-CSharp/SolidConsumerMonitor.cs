// Decompiled with JetBrains decompiler
// Type: SolidConsumerMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6999FFE9-E355-44B6-B5D9-B5C530D5F1A8
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SolidConsumerMonitor : 
  GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>
{
  public static Vector3 PLANT_ON_FLOOR_VESSEL_OFFSET = Vector3.down;
  private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State satisfied;
  private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State lookingforfood;
  private GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.State sportHunting;
  private static Tag[] creatureTags = new Tag[2]
  {
    GameTags.Creatures.ReservedByCreature,
    GameTags.CreatureBrain
  };

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.satisfied;
    this.root.EventHandler(GameHashes.EatSolidComplete, (GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.GameEvent.Callback) ((smi, data) => smi.OnEatSolidComplete(data))).ToggleBehaviour(GameTags.Creatures.WantsToEat, (StateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.Transition.ConditionCallback) (smi => (UnityEngine.Object) smi.targetEdible != (UnityEngine.Object) null && !smi.targetEdible.HasTag(GameTags.Creatures.ReservedByCreature)));
    this.satisfied.TagTransition(GameTags.Creatures.Hungry, this.lookingforfood).Transition(this.sportHunting, new StateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.Transition.ConditionCallback(SolidConsumerMonitor.IsSportHuntingConditionMet));
    this.lookingforfood.TagTransition(GameTags.Creatures.Hungry, this.satisfied, true).PreBrainUpdate(new System.Action<SolidConsumerMonitor.Instance>(SolidConsumerMonitor.FindFood));
    this.sportHunting.TagTransition(GameTags.Creatures.Hungry, this.lookingforfood).Transition(this.satisfied, (StateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.Transition.ConditionCallback) (smi => !SolidConsumerMonitor.IsSportHuntingConditionMet(smi))).PreBrainUpdate(new System.Action<SolidConsumerMonitor.Instance>(SolidConsumerMonitor.FindSportHuntPrey));
  }

  private static bool IsSportHuntingConditionMet(SolidConsumerMonitor.Instance smi)
  {
    return smi.def.sportHuntWhenOvercrowded && smi.diet.CanEatPreyCritter && smi.gameObject.HasTag(GameTags.Creatures.Overcrowded) && !smi.gameObject.HasTag(GameTags.Creatures.Hungry);
  }

  private static void FindSportHuntPrey(SolidConsumerMonitor.Instance smi)
  {
    if (smi.IsTargetEdibleValid())
      return;
    smi.ClearTargetEdible();
    SolidConsumerMonitor.FindPreyInCavity(smi);
  }

  private static bool FindPreyInCavity(SolidConsumerMonitor.Instance smi)
  {
    Diet diet = smi.diet;
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((StateMachine.Instance) smi));
    KPrefabID kprefabId = (KPrefabID) null;
    int cost1 = int.MaxValue;
    if (cavityForCell != null)
    {
      foreach (KPrefabID creature in cavityForCell.creatures)
      {
        if (!creature.HasTag(GameTags.Creatures.ReservedByCreature) && diet.GetDietInfo(creature.PrefabTag) != null)
        {
          int cost2 = smi.GetCost(creature.gameObject);
          if (cost2 != -1 && (cost2 < cost1 || cost1 == -1))
          {
            kprefabId = creature;
            cost1 = cost2;
          }
        }
        if ((UnityEngine.Object) kprefabId != (UnityEngine.Object) null)
        {
          if (cost1 < 3)
            break;
        }
      }
    }
    if (!((UnityEngine.Object) kprefabId != (UnityEngine.Object) null))
      return false;
    smi.SetTargetEdible(kprefabId.gameObject, cost1);
    smi.targetEdibleOffset = smi.GetBestEdibleOffset(kprefabId.gameObject);
    return true;
  }

  private static void FindFood(SolidConsumerMonitor.Instance smi)
  {
    if (smi.IsTargetEdibleValid())
      return;
    smi.ClearTargetEdible();
    Diet diet = smi.diet;
    int x = 0;
    int y = 0;
    Grid.PosToXY(smi.gameObject.transform.GetPosition(), out x, out y);
    x -= 8;
    int y_bottomLeft = y - 8;
    bool flag1 = false;
    if (diet.CanEatPreyCritter)
      flag1 = SolidConsumerMonitor.FindPreyInCavity(smi);
    bool flag2 = false;
    if (!flag1 && diet.CanEatAnySolid)
    {
      int num = 32 /*0x20*/;
      foreach (CreatureFeeder creatureFeeder in Components.CreatureFeeders.GetItems(smi.GetMyWorldId()))
      {
        Vector2I targetFeederCell = creatureFeeder.GetTargetFeederCell();
        if (targetFeederCell.x >= x && targetFeederCell.x <= x + num && targetFeederCell.y >= y_bottomLeft && targetFeederCell.y <= y_bottomLeft + num && !creatureFeeder.StoragesAreEmpty())
        {
          int cost = smi.GetCost(Grid.XYToCell(targetFeederCell.x, targetFeederCell.y));
          if (smi.IsCloserThanTargetEdible(cost))
          {
            foreach (Storage storage in creatureFeeder.storages)
            {
              if (!((UnityEngine.Object) storage == (UnityEngine.Object) null) && !storage.IsEmpty() && smi.GetCost(Grid.PosToCell(storage.items[0])) != -1)
              {
                foreach (GameObject gameObject in storage.items)
                {
                  if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) null))
                  {
                    KPrefabID component = gameObject.GetComponent<KPrefabID>();
                    if (!component.HasAnyTags(SolidConsumerMonitor.creatureTags) && diet.GetDietInfo(component.PrefabTag) != null)
                    {
                      smi.SetTargetEdible(gameObject, cost);
                      smi.targetEdibleOffset = Vector3.zero;
                      flag2 = true;
                      break;
                    }
                  }
                }
                if (flag2)
                  break;
              }
            }
          }
        }
      }
    }
    bool flag3 = false;
    if (!flag1 && !flag2 && diet.CanEatAnyPlantDirectly)
    {
      ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList gathered_entries = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
      GameScenePartitioner.Instance.GatherEntries(x, y_bottomLeft, 16 /*0x10*/, 16 /*0x10*/, GameScenePartitioner.Instance.plants, (List<ScenePartitionerEntry>) gathered_entries);
      foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries)
      {
        KPrefabID kprefabId = (KPrefabID) partitionerEntry.obj;
        Diet.Info dietInfo = diet.GetDietInfo(kprefabId.PrefabTag);
        Vector3 position = kprefabId.transform.GetPosition();
        bool flag4 = kprefabId.HasTag(GameTags.PlantedOnFloorVessel);
        if (flag4)
          position += SolidConsumerMonitor.PLANT_ON_FLOOR_VESSEL_OFFSET;
        int cost1 = smi.GetCost(Grid.PosToCell(position));
        Vector3 vector3 = Vector3.zero;
        if (smi.IsCloserThanTargetEdible(cost1) && !kprefabId.HasAnyTags(SolidConsumerMonitor.creatureTags) && dietInfo != null)
        {
          if (kprefabId.HasTag(GameTags.Plant))
          {
            IPlantConsumptionInstructions[] consumptionInstructions1 = GameUtil.GetPlantConsumptionInstructions(kprefabId.gameObject);
            if (consumptionInstructions1 != null && consumptionInstructions1.Length != 0)
            {
              bool flag5 = false;
              foreach (IPlantConsumptionInstructions consumptionInstructions2 in consumptionInstructions1)
              {
                if (consumptionInstructions2.CanPlantBeEaten() && dietInfo.foodType == consumptionInstructions2.GetDietFoodType())
                {
                  CellOffset[] allowedOffsets = consumptionInstructions2.GetAllowedOffsets();
                  if (allowedOffsets != null)
                  {
                    cost1 = -1;
                    foreach (CellOffset offset in allowedOffsets)
                    {
                      int cost2 = smi.GetCost(Grid.OffsetCell(Grid.PosToCell(position), offset));
                      if (cost2 != -1 && (cost1 == -1 || cost2 < cost1))
                      {
                        cost1 = cost2;
                        vector3 = offset.ToVector3();
                      }
                    }
                    if (cost1 != -1)
                    {
                      flag5 = true;
                      break;
                    }
                  }
                  else
                    flag5 = true;
                }
              }
              if (!flag5)
                continue;
            }
            else
              continue;
          }
          smi.SetTargetEdible(kprefabId.gameObject, cost1);
          smi.targetEdibleOffset = vector3 + (flag4 ? SolidConsumerMonitor.PLANT_ON_FLOOR_VESSEL_OFFSET : Vector3.zero);
          flag3 = true;
        }
      }
      gathered_entries.Recycle();
    }
    if (flag1 || flag2 || flag3 || !diet.CanEatAnySolid)
      return;
    bool flag6 = false;
    ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList gathered_entries1 = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(x, y_bottomLeft, 16 /*0x10*/, 16 /*0x10*/, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) gathered_entries1);
    foreach (ScenePartitionerEntry partitionerEntry in (List<ScenePartitionerEntry>) gathered_entries1)
    {
      Pickupable pickupable = (Pickupable) partitionerEntry.obj;
      KPrefabID kprefabId = pickupable.KPrefabID;
      if (!kprefabId.HasAnyTags(SolidConsumerMonitor.creatureTags) && diet.GetDietInfo(kprefabId.PrefabTag) != null)
      {
        bool isReachable;
        smi.ProcessEdible(pickupable.gameObject, out isReachable);
        smi.targetEdibleOffset = Vector3.zero;
        flag6 |= isReachable;
      }
    }
    gathered_entries1.Recycle();
  }

  public class Def : StateMachine.BaseDef
  {
    public Diet diet;
    public Vector3[] possibleEatPositionOffsets = new Vector3[1]
    {
      Vector3.zero
    };
    public Vector2 navigatorSize = Vector2.one;
    public bool sportHuntWhenOvercrowded;
  }

  public new class Instance : 
    GameStateMachine<SolidConsumerMonitor, SolidConsumerMonitor.Instance, IStateMachineTarget, SolidConsumerMonitor.Def>.GameInstance
  {
    private const int RECALC_THRESHOLD = 4;
    public GameObject targetEdible;
    public Vector3 targetEdibleOffset;
    private int targetEdibleCost;
    [MyCmpGet]
    private Navigator navigator;
    [MyCmpGet]
    private DrowningMonitor drowningMonitor;
    public Diet diet;

    public Instance(IStateMachineTarget master, SolidConsumerMonitor.Def def)
      : base(master, def)
    {
      this.diet = DietManager.Instance.GetPrefabDiet(this.gameObject);
    }

    public bool CanSearchForPickupables(bool foodAtFeeder) => !foodAtFeeder;

    public bool IsCloserThanTargetEdible(int cost)
    {
      if (cost == -1)
        return false;
      return cost < this.targetEdibleCost || this.targetEdibleCost == -1;
    }

    public bool IsTargetEdibleValid()
    {
      if ((UnityEngine.Object) this.targetEdible == (UnityEngine.Object) null)
        return false;
      int cost = this.GetCost(Grid.PosToCell(this.targetEdible.transform.GetPosition() + this.targetEdibleOffset));
      return cost != -1 && cost <= this.targetEdibleCost + 4;
    }

    public void ClearTargetEdible()
    {
      this.targetEdibleCost = -1;
      this.targetEdible = (GameObject) null;
      this.targetEdibleOffset = Vector3.zero;
    }

    public bool ProcessEdible(GameObject edible, out bool isReachable)
    {
      int cost = this.GetCost(edible);
      isReachable = cost != -1;
      if (cost == -1 || cost >= this.targetEdibleCost && this.targetEdibleCost != -1)
        return false;
      this.SetTargetEdible(edible, cost);
      return true;
    }

    public void SetTargetEdible(GameObject gameObject, int cost)
    {
      if ((UnityEngine.Object) this.targetEdible == (UnityEngine.Object) gameObject)
        return;
      this.targetEdibleCost = cost;
      this.targetEdible = gameObject;
    }

    public int GetCost(GameObject edible)
    {
      return this.GetCost(Grid.PosToCell(edible.transform.GetPosition() + this.smi.GetBestEdibleOffset(edible)));
    }

    public int GetCost(int cell)
    {
      return (!((UnityEngine.Object) this.drowningMonitor != (UnityEngine.Object) null) || !this.drowningMonitor.canDrownToDeath ? 0 : (!this.drowningMonitor.livesUnderWater ? 1 : 0)) != 0 && !this.drowningMonitor.IsCellSafe(cell) ? -1 : this.navigator.GetNavigationCost(cell);
    }

    public void OnEatSolidComplete(object data)
    {
      KPrefabID cmp = data as KPrefabID;
      if ((UnityEngine.Object) cmp == (UnityEngine.Object) null)
        return;
      PrimaryElement component1 = cmp.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
        return;
      Diet.Info dietInfo = this.diet.GetDietInfo(cmp.PrefabTag);
      if (dietInfo == null)
        return;
      AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(this.smi.gameObject);
      string properName = cmp.GetProperName();
      PopFXManager.Instance.SpawnFX(global::Def.GetUISprite((object) cmp.gameObject).first, PopFXManager.Instance.sprite_Negative, properName, cmp.transform, Vector3.zero);
      float num1 = amountInstance.GetMax() - amountInstance.value;
      float consumptionMass = dietInfo.ConvertCaloriesToConsumptionMass(num1);
      IPlantConsumptionInstructions consumptionInstructions = (IPlantConsumptionInstructions) null;
      foreach (IPlantConsumptionInstructions consumptionInstruction in GameUtil.GetPlantConsumptionInstructions(cmp.gameObject))
      {
        if (dietInfo.foodType == consumptionInstruction.GetDietFoodType())
          consumptionInstructions = consumptionInstruction;
      }
      float num2;
      if (consumptionInstructions != null)
      {
        float mass = consumptionInstructions.ConsumePlant(consumptionMass);
        num2 = dietInfo.ConvertConsumptionMassToCalories(mass);
      }
      else if (dietInfo.foodType == Diet.Info.FoodType.EatPrey || dietInfo.foodType == Diet.Info.FoodType.EatButcheredPrey)
      {
        float b = this.diet.AvailableCaloriesInPrey(cmp.PrefabTag);
        float multiplier = Mathf.Clamp((float) (1.0 - (double) num1 / (double) b), 0.0f, 1f);
        if ((double) multiplier > 0.0)
        {
          Butcherable component2 = cmp.GetComponent<Butcherable>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
            component2.CreateDrops(multiplier);
        }
        component1.Mass = 0.0f;
        num2 = Mathf.Min(num1, b);
        if (this.IsInsideState((StateMachine.BaseState) this.sm.sportHunting))
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) CREATURES.STATUSITEMS.SPORTHUNT.POPFX, this.gameObject.transform);
      }
      else
      {
        float mass = Mathf.Min(consumptionMass, component1.Mass);
        component1.Mass -= mass;
        Pickupable component3 = component1.GetComponent<Pickupable>();
        if ((UnityEngine.Object) component3.storage != (UnityEngine.Object) null)
        {
          component3.storage.Trigger(-1452790913, (object) this.gameObject);
          component3.storage.Trigger(-1697596308, (object) this.gameObject);
        }
        num2 = dietInfo.ConvertConsumptionMassToCalories(mass);
      }
      Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent> data1 = Boxed<CreatureCalorieMonitor.CaloriesConsumedEvent>.Get(new CreatureCalorieMonitor.CaloriesConsumedEvent()
      {
        tag = cmp.PrefabTag,
        calories = num2
      });
      this.Trigger(-2038961714, (object) data1);
      data1.Release();
      this.targetEdible = (GameObject) null;
    }

    public string[] GetTargetEdibleEatAnims()
    {
      return this.diet.GetDietInfo(this.targetEdible.PrefabID()).eatAnims;
    }

    public Vector3 GetBestEdibleOffset(GameObject edible)
    {
      int num = int.MaxValue;
      Vector3 bestEdibleOffset = Vector3.zero;
      foreach (Vector3 eatPositionOffset in this.def.possibleEatPositionOffsets)
      {
        Vector3 pos = edible.transform.position + eatPositionOffset;
        if ((double) eatPositionOffset.x > 0.0)
          pos += new Vector3(this.def.navigatorSize.x / 2f, 0.0f, 0.0f);
        else if ((double) eatPositionOffset.x < 0.0)
          pos -= new Vector3(this.def.navigatorSize.x / 2f, 0.0f, 0.0f);
        if ((double) eatPositionOffset.y > 0.0)
          pos += new Vector3(0.0f, this.def.navigatorSize.y / 2f, 0.0f);
        else if ((double) eatPositionOffset.y < 0.0)
          pos -= new Vector3(0.0f, this.def.navigatorSize.y / 2f, 0.0f);
        int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(pos));
        if (navigationCost != -1 && navigationCost < num)
        {
          num = navigationCost;
          bestEdibleOffset = eatPositionOffset;
        }
      }
      return bestEdibleOffset;
    }
  }
}
